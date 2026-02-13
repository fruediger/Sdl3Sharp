using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Sdl3Sharp.SourceGeneration.RegisterDriver;

partial class SourceGenerator
{
	private static void GenerateRegisterDriverAttributeSource(IncrementalGeneratorPostInitializationContext context)
	{
		context.AddSource("Sdl3Sharp.SourceGeneration.RegisterDriver.RegisterDriverAttribute.g.cs", SourceText.From(
			text: $$"""
				#nullable enable

				namespace Sdl3Sharp.SourceGeneration.RegisterDriver;

				[global::Microsoft.CodeAnalysis.EmbeddedAttribute]
				[global::System.AttributeUsage(global::System.AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
				internal sealed class RegisterDriverAttribute(string name) : global::System.Attribute
				{
					public string Name => name;
				}

				#nullable restore
				""",
			encoding: System.Text.Encoding.UTF8
		));
	}
}
