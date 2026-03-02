using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace Sdl3Sharp.SourceGeneration.RegisterDriver;

partial class SourceGenerator
{
	private abstract class BuildTreeNode;

	private sealed class BuildTreeLeaf(INamedTypeSymbol targetType, Location? location) : BuildTreeNode
	{
		public INamedTypeSymbol TargetType => targetType;
		public Location? Location => location;
	}

	private sealed class BuildTree : BuildTreeNode
	{
		private readonly SortedDictionary<byte, BuildTreeNode> mChildren = [];

		public bool IsEmpty => mChildren.Count is not > 0;

		public void Add(SourceProductionContext spc, string driverName, INamedTypeSymbol targetType, Location? location)
		{
			Add(spc, driverName, Encoding.UTF8.GetBytes(driverName), 0, targetType, location);
		}

		private void Add(SourceProductionContext spc, string driverName, byte[] driverNameUtf8, int driverNameIndex, INamedTypeSymbol targetType, Location? location)
		{
			if (driverNameIndex >= driverNameUtf8.Length)
			{
				if (!mChildren.TryGetValue((byte)'\0', out var childNode))
				{
					mChildren.Add((byte)'\0', new BuildTreeLeaf(targetType, location));
				}
				else if (childNode is BuildTreeLeaf leaf)
				{
					if (!SymbolEqualityComparer.Default.Equals(leaf.TargetType, targetType))
					{
						spc.ReportDiagnostic(Diagnostic.Create(
							mDuplicateDriverRegistrationDescriptor,
							location,
							driverName,
							leaf.TargetType.ToDisplayString(mDefaultTypeSymbolDisplayFormat)
						));
					}
				}
			}
			else
			{
				var currentByte = driverNameUtf8[driverNameIndex];

				if (!mChildren.TryGetValue(currentByte, out var childNode))
				{
					mChildren.Add(currentByte, childNode = new BuildTree());
				}

				if (childNode is BuildTree childTree)
				{
					childTree.Add(spc, driverName, driverNameUtf8, driverNameIndex + 1, targetType, location);
				}
				else
				{
					spc.ReportDiagnostic(Diagnostic.Create(
						mDriverNamePrefixConflictDescriptor,
						location,
						driverName
					));
				}
			}
		}

		public void Print(StringBuilder builder, string ntStringPointerArgumentName, Action<StringBuilder, INamedTypeSymbol, string> printLeaf, string indentation = "", string indentationStep = "\t")
		{
			builder.Append($$"""

				{{indentation}}switch (*{{ntStringPointerArgumentName}}++)
				{{indentation}}{
				""");

			var indentationP1 = indentation + indentationStep;
			var indentationP2 = indentationP1 + indentationStep;

			foreach (var (b, child) in mChildren)
			{
				builder.Append($$"""

					{{indentationP1}}case (byte)'{{SymbolDisplay.FormatLiteral(unchecked((char)b), quote: false)}}':
					""");

				switch (child)
				{
					case BuildTreeLeaf leaf:
						printLeaf(builder, leaf.TargetType, indentationP2);
						break;

					case BuildTree tree:
						tree.Print(builder, ntStringPointerArgumentName, printLeaf, indentationP2, indentationStep);

						builder.Append($$"""

							{{indentationP2}}break;

							""");
						break;
				}
			}

			builder.Append($$"""
				{{indentation}}}
				""");
		}
	}

	private static readonly DiagnosticDescriptor mDuplicateDriverRegistrationDescriptor = new(
		id: $"{DiagnosticDescriptorIdPrefix}001",
		title: "Duplicate driver registration",
		messageFormat: "The driver '{0}' is already registered with a different type '{1}'",
		category: DiagnosticDescriptorCategory,
		defaultSeverity: DiagnosticSeverity.Error,
		isEnabledByDefault: true
	);

	private static readonly DiagnosticDescriptor mDriverNamePrefixConflictDescriptor = new(
		id: $"{DiagnosticDescriptorIdPrefix}002",
		title: "Render name prefix conflict",
		messageFormat: "The driver name '{0}' conflicts with an existing driver name (one is a prefix of the other)",
		category: DiagnosticDescriptorCategory,
		defaultSeverity: DiagnosticSeverity.Error,
		isEnabledByDefault: true
	);

	private static readonly DiagnosticDescriptor mMissingRequiredTypeDescriptor = new(
		id: $"{DiagnosticDescriptorIdPrefix}003",
		title: "Missing required type",
		messageFormat: "The required type '{0}' could not be found in the compilation",
		category: DiagnosticDescriptorCategory,
		defaultSeverity: DiagnosticSeverity.Error,
		isEnabledByDefault: true
	);

	private static readonly DiagnosticDescriptor mMissingRequiredConstructorDescriptor = new(
		id: $"{DiagnosticDescriptorIdPrefix}004",
		title: "Missing required constructor",
		messageFormat: "The type '{0}' must have a constructor with the signature '({1})'",
		category: DiagnosticDescriptorCategory,
		defaultSeverity: DiagnosticSeverity.Error,
		isEnabledByDefault: true
	);

	private static readonly DiagnosticDescriptor mMissingRequiredInterfaceImplementation = new(
		id: $"{DiagnosticDescriptorIdPrefix}005",
		title: "Missing required interface implementation",
		messageFormat: "The driver type '{0}' must implement either the interface '{1}' or the interface '{2}'",
		category: DiagnosticDescriptorCategory,
		defaultSeverity: DiagnosticSeverity.Error,
		isEnabledByDefault: true
	);

	private static readonly DiagnosticDescriptor mConflictingInterfaceImplementationDescriptor = new(
		id: $"{DiagnosticDescriptorIdPrefix}006",
		title: "Conflicting interface implementation",
		messageFormat: "The driver type '{0}' cannot implement both the interface '{1}' and the interface '{2}'",
		category: DiagnosticDescriptorCategory,
		defaultSeverity: DiagnosticSeverity.Error,
		isEnabledByDefault: true
	);

	private static readonly SymbolDisplayFormat mDefaultTypeSymbolDisplayFormat = new(
		globalNamespaceStyle: SymbolDisplayGlobalNamespaceStyle.Included,
		typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces,
		genericsOptions: SymbolDisplayGenericsOptions.IncludeTypeParameters,
		delegateStyle: SymbolDisplayDelegateStyle.NameOnly,
		miscellaneousOptions: SymbolDisplayMiscellaneousOptions.UseSpecialTypes | SymbolDisplayMiscellaneousOptions.EscapeKeywordIdentifiers | SymbolDisplayMiscellaneousOptions.IncludeNullableReferenceTypeModifier | SymbolDisplayMiscellaneousOptions.CollapseTupleTypes
	);

	private static readonly SymbolDisplayFormat mDiagnosticTypeSymbolDisplayFormat = mDefaultTypeSymbolDisplayFormat.WithGlobalNamespaceStyle(SymbolDisplayGlobalNamespaceStyle.Omitted);

	private static void GenerateRenderDriverDispatchSources(SourceProductionContext spc, (Compilation compilation, ImmutableArray<(INamedTypeSymbol targetType, string driverName, Location? location)> values) data)
	{
		const string renderingNamespaceName        = "Sdl3Sharp.Video.Rendering",
					 iRenderingDriverNamespaceName = $"{renderingNamespaceName}.Drivers",
					 iRenderingDriverTypeName      = "IRenderingDriver",
					 iRenderingDriverFullTypeName  = $"{iRenderingDriverNamespaceName}.{iRenderingDriverTypeName}",
					 rendererTypeName              = "Renderer",
					 rendererFullTypeName          = $"{renderingNamespaceName}.{rendererTypeName}",
					 sdlRendererTypeName           = "SDL_Renderer",
					 sdlRendererFullTypeName       = $"{rendererFullTypeName}+{sdlRendererTypeName}",
					 rendererTDriverTypeName       = "Renderer`1",
					 rendererTDriverFullTypeName   = $"{renderingNamespaceName}.{rendererTDriverTypeName}",
					 textureTypeName               = "Texture",
					 textureFullTypeName           = $"{renderingNamespaceName}.{textureTypeName}",
					 sdlTextureTypeName            = "SDL_Texture",
					 sdlTextureFullTypeName        = $"{textureFullTypeName}+{sdlTextureTypeName}",
					 textureTDriverTypeName        = "Texture`1",
					 textureTDriverFullTypeName    = $"{renderingNamespaceName}.{textureTDriverTypeName}",
					 sdlUnsupportedDriverDiagId    = "SDL3001",
					 windowingNamespaceName        = "Sdl3Sharp.Video.Windowing",
					 iWindowingDriverNamespaceName = $"{windowingNamespaceName}.Drivers",
					 iWindowingDriverTypeName      = "IWindowingDriver",
					 iWindowingDriverFullTypeName  = $"{iWindowingDriverNamespaceName}.{iWindowingDriverTypeName}",
					 iDisplayTypeName              = "IDisplay",
					 iDisplayFullTypeName          = $"{windowingNamespaceName}.{iDisplayTypeName}",
					 displayTypeName               = "Display`1",
					 displayFullTypeName           = $"{windowingNamespaceName}.{displayTypeName}";

		var (compilation, values) = data;

		var iRenderingDriverType = compilation.GetTypeByMetadataName(iRenderingDriverFullTypeName);
		if (iRenderingDriverType is null)
		{
			foreach (var (_, _, location) in values)
			{
				spc.ReportDiagnostic(Diagnostic.Create(
					mMissingRequiredTypeDescriptor,
					location,
					iRenderingDriverFullTypeName
				));
			}
			return;
		}

		var rendererType = compilation.GetTypeByMetadataName(rendererFullTypeName);
		if (rendererType is null)
		{
			foreach (var (_, _, location) in values)
			{
				spc.ReportDiagnostic(Diagnostic.Create(
					mMissingRequiredTypeDescriptor,
					location,
					rendererFullTypeName
				));
			}
			return;
		}

		var sdlRendererType = compilation.GetTypeByMetadataName(sdlRendererFullTypeName);
		if (sdlRendererType is null)
		{
			foreach (var (_, _, location) in values)
			{
				spc.ReportDiagnostic(Diagnostic.Create(
					mMissingRequiredTypeDescriptor,
					location,
					sdlRendererFullTypeName
				));
			}
			return;
		}

		var sdlRendererPointerType = compilation.CreatePointerTypeSymbol(sdlRendererType);

		var rendererTDriverType = compilation.GetTypeByMetadataName(rendererTDriverFullTypeName);
		if (rendererTDriverType is null)
		{
			foreach (var (_, _, location) in values)
			{
				spc.ReportDiagnostic(Diagnostic.Create(
					mMissingRequiredTypeDescriptor,
					location,
					rendererTDriverFullTypeName
				));
			}
			return;
		}

		if (!rendererTDriverType.InstanceConstructors.Any(ctor
			=> ctor is { Parameters: [{ Type: var rendererType }, { Type.SpecialType: SpecialType.System_Boolean }] }
			&& SymbolEqualityComparer.Default.Equals(rendererType, sdlRendererPointerType)
		))
		{
			foreach (var (_, _, location) in values)
			{
				spc.ReportDiagnostic(Diagnostic.Create(
					mMissingRequiredConstructorDescriptor,
					location,
					rendererTDriverType.ToDisplayString(mDiagnosticTypeSymbolDisplayFormat), $"{sdlRendererPointerType.ToDisplayString(mDiagnosticTypeSymbolDisplayFormat)}, {compilation.GetSpecialType(SpecialType.System_Boolean).ToDisplayString(mDiagnosticTypeSymbolDisplayFormat)}"
				));
			}
			return;
		}

		var textureType = compilation.GetTypeByMetadataName(textureFullTypeName);
		if (textureType is null)
		{
			foreach (var (_, _, location) in values)
			{
				spc.ReportDiagnostic(Diagnostic.Create(
					mMissingRequiredTypeDescriptor,
					location,
					textureFullTypeName
				));
			}
			return;
		}

		var sdlTextureType = compilation.GetTypeByMetadataName(sdlTextureFullTypeName);
		if (sdlTextureType is null)
		{
			foreach (var (_, _, location) in values)
			{
				spc.ReportDiagnostic(Diagnostic.Create(
					mMissingRequiredTypeDescriptor,
					location,
					sdlTextureFullTypeName
				));
			}
			return;
		}

		var sdlTexturePointerType = compilation.CreatePointerTypeSymbol(sdlTextureType);

		var textureTDriverType = compilation.GetTypeByMetadataName(textureTDriverFullTypeName);
		if (textureTDriverType is null)
		{
			foreach (var (_, _, location) in values)
			{
				spc.ReportDiagnostic(Diagnostic.Create(
					mMissingRequiredTypeDescriptor,
					location,
					textureTDriverFullTypeName
				));
			}
			return;
		}

		if (!textureTDriverType.InstanceConstructors.Any(ctor
			=> ctor is { Parameters: [{ Type: var textureType }, { Type.SpecialType: SpecialType.System_Boolean }] }
			&& SymbolEqualityComparer.Default.Equals(textureType, sdlTexturePointerType)
		))
		{
			foreach (var (_, _, location) in values)
			{
				spc.ReportDiagnostic(Diagnostic.Create(
					mMissingRequiredConstructorDescriptor,
					location,
					textureTDriverType.ToDisplayString(mDiagnosticTypeSymbolDisplayFormat), $"{sdlTexturePointerType.ToDisplayString(mDiagnosticTypeSymbolDisplayFormat)}, {compilation.GetSpecialType(SpecialType.System_Boolean).ToDisplayString(mDiagnosticTypeSymbolDisplayFormat)}"
				));
			}
			return;
		}

		var iWindowingDriverType = compilation.GetTypeByMetadataName(iWindowingDriverFullTypeName);
		if (iWindowingDriverType is null)
		{
			foreach (var (_, _, location) in values)
			{
				spc.ReportDiagnostic(Diagnostic.Create(
					mMissingRequiredTypeDescriptor,
					location,
					iWindowingDriverFullTypeName
				));
			}
			return;
		}

		var iDisplayType = compilation.GetTypeByMetadataName(iDisplayFullTypeName);
		if (iDisplayType is null)
		{
			foreach (var (_, _, location) in values)
			{
				spc.ReportDiagnostic(Diagnostic.Create(
					mMissingRequiredTypeDescriptor,
					location,
					iDisplayFullTypeName
				));
			}
			return;
		}

		var displayType = compilation.GetTypeByMetadataName(displayFullTypeName);
		if (displayType is null)
		{
			foreach (var (_, _, location) in values)
			{
				spc.ReportDiagnostic(Diagnostic.Create(
					mMissingRequiredTypeDescriptor,
					location,
					displayFullTypeName
				));
			}
			return;
		}

		if (!displayType.InstanceConstructors.Any(ctor
			=> ctor is { Parameters: [{ Type.SpecialType: SpecialType.System_UInt32 }, { Type.SpecialType: SpecialType.System_Boolean }] }
		))
		{
			foreach (var (_, _, location) in values)
			{
				spc.ReportDiagnostic(Diagnostic.Create(
					mMissingRequiredConstructorDescriptor,
					location,
					displayType.ToDisplayString(mDiagnosticTypeSymbolDisplayFormat), $"{compilation.GetSpecialType(SpecialType.System_UInt32).ToDisplayString(mDiagnosticTypeSymbolDisplayFormat)}, {compilation.GetSpecialType(SpecialType.System_Boolean).ToDisplayString(mDiagnosticTypeSymbolDisplayFormat)}"
				));
			}
			return;
		}

		var renderingDrivers = new BuildTree();
		var windowingDrivers = new BuildTree();

		foreach (var (targetType, driverName, location) in values)
		{
			var interfaces = targetType.AllInterfaces;

			switch (interfaces.Contains(iRenderingDriverType, SymbolEqualityComparer.Default), interfaces.Contains(iWindowingDriverType, SymbolEqualityComparer.Default))
			{
				case (false, false):
					spc.ReportDiagnostic(Diagnostic.Create(
						mMissingRequiredInterfaceImplementation,
						location,
						targetType.ToDisplayString(mDiagnosticTypeSymbolDisplayFormat),	iRenderingDriverType.ToDisplayString(mDiagnosticTypeSymbolDisplayFormat), iWindowingDriverType.ToDisplayString(mDiagnosticTypeSymbolDisplayFormat)
					));
					break;

				case (true, false):
					renderingDrivers.Add(spc, driverName, targetType, location);
					break;

				case (false, true):
					windowingDrivers.Add(spc, driverName, targetType, location);
					break;

				case (true, true):
					spc.ReportDiagnostic(Diagnostic.Create(
						mConflictingInterfaceImplementationDescriptor,
						location,
						targetType.ToDisplayString(mDiagnosticTypeSymbolDisplayFormat), iRenderingDriverType.ToDisplayString(mDiagnosticTypeSymbolDisplayFormat), iWindowingDriverType.ToDisplayString(mDiagnosticTypeSymbolDisplayFormat)
					));
					break;
			}
		}

		var builder = new StringBuilder();

		builder.Append($$"""
			#nullable enable
			
			namespace {{renderingNamespaceName}};
			
			partial class {{rendererTypeName}}
			{
				[global::System.CodeDom.Compiler.GeneratedCode("{{mTool.Name}}", "{{mTool.Version}}")]
				internal unsafe static bool TryCreateFromRegisteredDriver({{sdlRendererType.ToDisplayString(mDefaultTypeSymbolDisplayFormat)}}* renderer, bool register, [global::System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out {{rendererTypeName}}? result)
				{
			""");

		if (!renderingDrivers.IsEmpty)
		{
			builder.Append($$"""

						var name = SDL_GetRendererName(renderer);

						if (name is not null)
						{
				#pragma warning disable {{sdlUnsupportedDriverDiagId}}
				""");

			renderingDrivers.Print(builder, "name", (builder, targetType, indentation) =>
				builder.Append($$"""

					{{indentation}}result = new {{rendererTDriverType.Construct(targetType).ToDisplayString(mDefaultTypeSymbolDisplayFormat)}}(renderer, register);
					{{indentation}}return true;

					"""),
				indentation: "\t\t\t", indentationStep: "\t"
			);

			builder.Append($$"""

				#pragma warning restore {{sdlUnsupportedDriverDiagId}}
						}

				""");
		}

		builder.Append("""

					result = null;
					return false;
				}
			}

			#nullable restore
			""");

		spc.AddSource($"{rendererFullTypeName}_TryCreateFromRegisteredDriver.g.cs", SourceText.From(
			text: builder.ToString(),
			encoding: Encoding.UTF8
		));

		builder.Clear();

		builder.Append($$"""
			#nullable enable
			
			namespace {{renderingNamespaceName}};
			
			partial class {{textureTypeName}}
			{
				[global::System.CodeDom.Compiler.GeneratedCode("{{mTool.Name}}", "{{mTool.Version}}")]
				internal unsafe static bool TryCreateFromRegisteredDriver({{sdlTextureType.ToDisplayString(mDefaultTypeSymbolDisplayFormat)}}* texture, bool register, [global::System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out {{textureTypeName}}? result)
				{
			""");

		if (!renderingDrivers.IsEmpty)
		{
			builder.Append($$"""

						var renderer = SDL_GetRendererFromTexture(texture);

						if (renderer is not null)
						{				
							var name = {{rendererTypeName}}.SDL_GetRendererName(renderer);

							if (name is not null)
							{
				#pragma warning disable {{sdlUnsupportedDriverDiagId}}
				""");

			renderingDrivers.Print(builder, "name", (builder, targetType, indentation) =>
				builder.Append($$"""

					{{indentation}}result = new {{textureTDriverType.Construct(targetType).ToDisplayString(mDefaultTypeSymbolDisplayFormat)}}(texture, register);
					{{indentation}}return true;

					"""),
				indentation: "\t\t\t\t", indentationStep: "\t"
			);

			builder.Append($$"""

				#pragma warning restore {{sdlUnsupportedDriverDiagId}}
							}
						}

				""");
		}

		builder.Append($$"""

					result = null;
					return false;
				}
			}

			#nullable restore
			""");

		spc.AddSource($"{textureFullTypeName}_TryCreateFromRegisteredDriver.g.cs", SourceText.From(
				text: builder.ToString(),
				encoding: Encoding.UTF8
			));

		builder.Clear();

		builder.Append($$"""
			#nullable enable
			
			namespace {{windowingNamespaceName}};
			
			partial interface {{iDisplayTypeName}}
			{
				[global::System.CodeDom.Compiler.GeneratedCode("{{mTool.Name}}", "{{mTool.Version}}")]
				internal unsafe static bool TryCreateFromRegisteredDriver(uint displayId, bool register, [global::System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out {{iDisplayTypeName}}? result)
				{
			""");

		if (!windowingDrivers.IsEmpty)
		{
			builder.Append($$"""

						var name = {{iWindowingDriverType.ToDisplayString(mDefaultTypeSymbolDisplayFormat)}}.SDL_GetCurrentVideoDriver();

						if (name is not null)
						{
				#pragma warning disable {{sdlUnsupportedDriverDiagId}}
				""");

			windowingDrivers.Print(builder, "name", (builder, targetType, indentation) =>
				builder.Append($$"""

					{{indentation}}result = new {{displayType.Construct(targetType).ToDisplayString(mDefaultTypeSymbolDisplayFormat)}}(displayId, register);
					{{indentation}}return true;

					"""),
				indentation: "\t\t\t", indentationStep: "\t"
			);

			builder.Append($$"""

				#pragma warning restore {{sdlUnsupportedDriverDiagId}}
						}

				""");
		}

		builder.Append("""

					result = null;
					return false;
				}
			}

			#nullable restore
			""");

		spc.AddSource($"{iDisplayFullTypeName}_TryCreateFromRegisteredDriver.g.cs", SourceText.From(
			text: builder.ToString(),
			encoding: Encoding.UTF8
		));
	}
}
