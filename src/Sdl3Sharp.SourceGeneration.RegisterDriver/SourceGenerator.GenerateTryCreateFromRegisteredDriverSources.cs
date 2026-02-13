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
	private abstract class RenderDriverTreeNode;

	private sealed class RenderDriverTreeLeaf(INamedTypeSymbol targetType, Location? location) : RenderDriverTreeNode
	{
		public INamedTypeSymbol TargetType => targetType;
		public Location? Location => location;
	}

	private sealed class RenderDriverTree : RenderDriverTreeNode
	{
		private readonly SortedDictionary<byte, RenderDriverTreeNode> mChildren = [];

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
					mChildren.Add((byte)'\0', new RenderDriverTreeLeaf(targetType, location));
				}
				else if (childNode is RenderDriverTreeLeaf leaf)
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
					mChildren.Add(currentByte, childNode = new RenderDriverTree());
				}

				if (childNode is RenderDriverTree childTree)
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
					case RenderDriverTreeLeaf leaf:
						printLeaf(builder, leaf.TargetType, indentationP2);
						break;

					case RenderDriverTree tree:
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
		title: "Duplicate render driver registration",
		messageFormat: "The render driver '{0}' is already registered with a different type '{1}'",
		category: DiagnosticDescriptorCategory,
		defaultSeverity: DiagnosticSeverity.Error,
		isEnabledByDefault: true
	);

	private static readonly DiagnosticDescriptor mDriverNamePrefixConflictDescriptor = new(
		id: $"{DiagnosticDescriptorIdPrefix}002",
		title: "Render driver name prefix conflict",
		messageFormat: "The render driver name '{0}' conflicts with an existing driver name (one is a prefix of the other)",
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
		messageFormat: "The type '{0}' must have a constructor with the signature '({1}, bool)'",
		category: DiagnosticDescriptorCategory,
		defaultSeverity: DiagnosticSeverity.Error,
		isEnabledByDefault: true
	);

	private static readonly DiagnosticDescriptor mMissingRequiredInterfaceImplementation = new(
		id: $"{DiagnosticDescriptorIdPrefix}005",
		title: "Missing required interface implementation",
		messageFormat: "The render driver type '{0}' must implement the interface '{1}'",
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
		const string renderingNamespaceName     = "Sdl3Sharp.Video.Rendering",
					 iDriversNamespaceName      = $"{renderingNamespaceName}.Drivers",
					 iDriverTypeName            = "IDriver",
					 iDriverFullTypeName        = $"{iDriversNamespaceName}.{iDriverTypeName}",
					 iRendererTypeName          = "IRenderer",
					 iRendererFullTypeName      = $"{renderingNamespaceName}.{iRendererTypeName}",
					 sdlRendererTypeName        = "SDL_Renderer",
					 sdlRendererFullTypeName    = $"{iRendererFullTypeName}+{sdlRendererTypeName}",
					 rendererTypeName           = "Renderer`1",
					 rendererFullTypeName       = $"{renderingNamespaceName}.{rendererTypeName}",
					 iTextureTypeName           = "ITexture",
					 iTextureFullTypeName       = $"{renderingNamespaceName}.{iTextureTypeName}",
					 sdlTextureTypeName         = "SDL_Texture",
					 sdlTextureFullTypeName     = $"{iTextureFullTypeName}+{sdlTextureTypeName}",
					 textureTypeName            = "Texture`1",
					 textureFullTypeName        = $"{renderingNamespaceName}.{textureTypeName}",
					 sdlUnsupportedDriverDiagId = "SDL3001";

		var (compilation, values) = data;

		var iDriverType = compilation.GetTypeByMetadataName(iDriverFullTypeName);
		if (iDriverType is null)
		{
			foreach (var (_, _, location) in values)
			{
				spc.ReportDiagnostic(Diagnostic.Create(
					mMissingRequiredTypeDescriptor,
					location,
					iDriverFullTypeName
				));
			}
			return;
		}

		var iRendererType = compilation.GetTypeByMetadataName(iRendererFullTypeName);
		if (iRendererType is null)
		{
			foreach (var (_, _, location) in values)
			{
				spc.ReportDiagnostic(Diagnostic.Create(
					mMissingRequiredTypeDescriptor,
					location,
					iRendererFullTypeName
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

		if (!rendererType.InstanceConstructors.Any(ctor
			=> ctor is { Parameters: [{ Type: var rendererType }, { Type.SpecialType: SpecialType.System_Boolean }] }
			&& SymbolEqualityComparer.Default.Equals(rendererType, sdlRendererPointerType)
		))
		{
			foreach (var (_, _, location) in values)
			{
				spc.ReportDiagnostic(Diagnostic.Create(
					mMissingRequiredConstructorDescriptor,
					location,
					rendererType.ToDisplayString(mDiagnosticTypeSymbolDisplayFormat), sdlRendererPointerType.ToDisplayString(mDiagnosticTypeSymbolDisplayFormat)
				));
			}
			return;
		}

		var iTextureType = compilation.GetTypeByMetadataName(iTextureFullTypeName);
		if (iTextureType is null)
		{
			foreach (var (_, _, location) in values)
			{
				spc.ReportDiagnostic(Diagnostic.Create(
					mMissingRequiredTypeDescriptor,
					location,
					iTextureFullTypeName
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

		if (!textureType.InstanceConstructors.Any(ctor
			=> ctor is { Parameters: [{ Type: var textureType }, { Type.SpecialType: SpecialType.System_Boolean }] }
			&& SymbolEqualityComparer.Default.Equals(textureType, sdlTexturePointerType)
		))
		{
			foreach (var (_, _, location) in values)
			{
				spc.ReportDiagnostic(Diagnostic.Create(
					mMissingRequiredConstructorDescriptor,
					location,
					textureType.ToDisplayString(mDiagnosticTypeSymbolDisplayFormat), sdlTexturePointerType.ToDisplayString(mDiagnosticTypeSymbolDisplayFormat)
				));
			}
			return;
		}

		var drivers = new RenderDriverTree();

		foreach (var (targetType, driverName, location) in values)
		{
			if (!targetType.AllInterfaces.Contains(iDriverType, SymbolEqualityComparer.Default))
			{
				spc.ReportDiagnostic(Diagnostic.Create(
					mMissingRequiredInterfaceImplementation,
					location,
					targetType.ToDisplayString(mDiagnosticTypeSymbolDisplayFormat), iDriverType.ToDisplayString(mDiagnosticTypeSymbolDisplayFormat)
				));
				break;
			}

			drivers.Add(spc, driverName, targetType, location);
		}

		var builder = new StringBuilder();

		builder.Append($$"""
			#nullable enable
			
			namespace {{renderingNamespaceName}};
			
			partial interface {{iRendererTypeName}}
			{
				[global::System.CodeDom.Compiler.GeneratedCode("{{mTool.Name}}", "{{mTool.Version}}")]
				internal unsafe static bool TryCreateFromRegisteredDriver({{sdlRendererType.ToDisplayString(mDefaultTypeSymbolDisplayFormat)}}* renderer, bool register, [global::System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out {{iRendererTypeName}}? result)
				{
			""");

		if (!drivers.IsEmpty)
		{
			builder.Append($$"""

						var name = SDL_GetRendererName(renderer);

						if (name is not null)
						{
				#pragma warning disable {{sdlUnsupportedDriverDiagId}}
				""");

			drivers.Print(builder, "name", (builder, targetType, indentation) =>
				builder.Append($$"""

					{{indentation}}result = new {{rendererType.Construct(targetType).ToDisplayString(mDefaultTypeSymbolDisplayFormat)}}(renderer, register);
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

		spc.AddSource($"{iRendererFullTypeName}_TryCreateFromRegisteredDriver.g.cs", SourceText.From(
			text: builder.ToString(),
			encoding: Encoding.UTF8
		));

		builder.Clear();

		builder.Append($$"""
			#nullable enable
			
			namespace {{renderingNamespaceName}};
			
			partial interface {{iTextureTypeName}}
			{
				[global::System.CodeDom.Compiler.GeneratedCode("{{mTool.Name}}", "{{mTool.Version}}")]
				internal unsafe static bool TryCreateFromRegisteredDriver({{sdlTextureType.ToDisplayString(mDefaultTypeSymbolDisplayFormat)}}* texture, bool register, [global::System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out {{iTextureTypeName}}? result)
				{
			""");

		if (!drivers.IsEmpty)
		{
			builder.Append($$"""

						var renderer = SDL_GetRendererFromTexture(texture);

						if (renderer is not null)
						{				
							var name = {{iRendererTypeName}}.SDL_GetRendererName(renderer);

							if (name is not null)
							{
				#pragma warning disable {{sdlUnsupportedDriverDiagId}}
				""");

			drivers.Print(builder, "name", (builder, targetType, indentation) =>
				builder.Append($$"""

					{{indentation}}result = new {{textureType.Construct(targetType).ToDisplayString(mDefaultTypeSymbolDisplayFormat)}}(texture, register);
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

		spc.AddSource($"{iTextureFullTypeName}_TryCreateFromRegisteredDriver.g.cs", SourceText.From(
				text: builder.ToString(),
				encoding: Encoding.UTF8
			));
	}
}
