using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Sdl3Sharp.SourceGeneration.RegisterDriver;

[Generator(LanguageNames.CSharp)]
internal sealed partial class SourceGenerator : IIncrementalGenerator
{
	private const string DiagnosticDescriptorIdPrefix = "SDL3REGDRV";
	private const string DiagnosticDescriptorCategory = $"{nameof(Sdl3Sharp)}.{nameof(SourceGeneration)}.{nameof(RegisterDriver)}";

	private static readonly (string Name, string Version) mTool = typeof(SourceGenerator).Assembly.GetName() switch { var assemblyName => (assemblyName.Name, assemblyName.Version.ToString(3)) };

	public void Initialize(IncrementalGeneratorInitializationContext context)
	{
		context.RegisterPostInitializationOutput(static pic => pic.AddEmbeddedAttributeDefinition());

		context.RegisterPostInitializationOutput(GenerateRegisterDriverAttributeSource);

		context.RegisterImplementationSourceOutput(
			source: context.CompilationProvider
				.Combine(context.SyntaxProvider.ForAttributeWithMetadataName("Sdl3Sharp.SourceGeneration.RegisterDriver.RegisterDriverAttribute",
					predicate: static (node, _) => node is ClassDeclarationSyntax,
					transform: static (gasc, cancellationToken) => gasc switch
					{
						{
							TargetSymbol: INamedTypeSymbol targetSymbol,
							Attributes: [{ 
								ConstructorArguments: [{ IsNull: false, Kind: TypedConstantKind.Primitive, Value: string driverName }, ..],
								ApplicationSyntaxReference: var syntaxRef
							}, ..],							
						}
							=> (targetSymbol, driverName, location: syntaxRef?.GetSyntax(cancellationToken).GetLocation()),
						_ => (targetSymbol: default(INamedTypeSymbol?), driverName: default(string?), location: default(Location?))
					}
				)
					.Where(static t => t.targetSymbol is not null && !string.IsNullOrWhiteSpace(t.driverName))
					.Select(static (t, _) => (t.targetSymbol!, t.driverName!, t.location))
					.Collect()
				),
			action: GenerateRenderDriverDispatchSources
		);
	}
}
