using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Sdl3Sharp.SourceGeneration;

partial class SourceGenerator
{
    private const string IndentationPrintPrefix = "\t";

    private static readonly DiagnosticDescriptor mUnsupportedTypeDeclarationSignatureDescriptor = new(
        id: $"{DiagnosticDescriptorIdPrefix}0011",
        title: "Unsupported type declaration signature",
        messageFormat: "The type declaration of \"{0}\" must be a 'partial' type in order for it to contain native imports or contain nested types containing native imports",
        category: DiagnosticDescriptorCategory,
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    private readonly struct BuildTree()
    {
        private readonly struct NamespaceNode()
        {
            public readonly Dictionary<INamespaceSymbol, NamespaceNode> NestedNamespaces = new(SymbolEqualityComparer.Default);
            public readonly Dictionary<INamedTypeSymbol, TypeNode> ContainedTypes = new(SymbolEqualityComparer.Default);

            public readonly bool IsEmpty => NestedNamespaces.Count is not > 0 && ContainedTypes.Count is not > 0;

            public readonly bool Consolidate(Stack<INamespaceOrTypeSymbol> symbolBuffer)
            {
                var initialBufferSize = symbolBuffer.Count;

                foreach (var (@namespace, namespaceNode) in NestedNamespaces)
                {
                    if (namespaceNode.Consolidate(symbolBuffer))
                    {
                        symbolBuffer.Push(@namespace);
                    }
                }

                while (initialBufferSize < symbolBuffer.Count)
                {
                    NestedNamespaces.Remove((INamespaceSymbol)symbolBuffer.Pop());
                }

                foreach (var (type, typeNode) in ContainedTypes)
                {
                    if (typeNode.Consolidate(symbolBuffer))
                    {
                        symbolBuffer.Push(type);
                    }
                }

                while (initialBufferSize < symbolBuffer.Count)
                {
                    ContainedTypes.Remove((INamedTypeSymbol)symbolBuffer.Pop());
                }

                return IsEmpty;
            }

            public readonly void Print(INamespaceSymbol @namespace, StringBuilder builder, Compilation compilation, string indentation)
            {
                var namespaceName = @namespace.Name;
                var namespaceNode = this;

                for (;
                    namespaceNode is { NestedNamespaces.Count: 1, ContainedTypes.Count: 0 };
                    (@namespace, namespaceNode) = namespaceNode.NestedNamespaces.FirstOrDefault(), namespaceName += $".{@namespace.Name}"
                ) ;

                builder.Append($$"""

                    {{indentation}}namespace {{namespaceName}}
                    {{indentation}}{
                    """);

                namespaceNode.PrintMembers(builder, compilation, indentation + IndentationPrintPrefix);

                builder.Append($$"""
                    {{indentation}}}

                    """);
            }

            public readonly void PrintMembers(StringBuilder builder, Compilation compilation, string indentation)
            {
                foreach (var (type, typeNode) in ContainedTypes)
                {
                    typeNode.Print(type, builder, compilation, indentation);
                }

                foreach (var (@namespace, namespaceNode) in NestedNamespaces)
                {
                    namespaceNode.Print(@namespace, builder, compilation, indentation);
                }
            }
        }

        private readonly struct TypeNode()
        {
            public readonly Dictionary<INamedTypeSymbol, TypeNode> NestedTypes = new(SymbolEqualityComparer.Default);
            public readonly Dictionary<IMethodSymbol, ImportData> ContainedImports = new(SymbolEqualityComparer.Default);

            public readonly bool IsEmpty => NestedTypes.Count is not > 0 && ContainedImports.Count is not > 0;

            public readonly bool Consolidate(Stack<INamespaceOrTypeSymbol> symbolBuffer)
            {
                var initialBufferSize = symbolBuffer.Count;

                foreach (var (type, typeNode) in NestedTypes)
                {
                    if (typeNode.Consolidate(symbolBuffer))
                    {
                        symbolBuffer.Push(type);
                    }
                }

                while (initialBufferSize < symbolBuffer.Count)
                {
                    NestedTypes.Remove((INamedTypeSymbol)symbolBuffer.Pop());
                }

                return IsEmpty;
            }

            public readonly void Print(INamedTypeSymbol type, StringBuilder builder, Compilation compilation, string indentation)
            {
                builder.Append($$"""

                    {{indentation}}partial {{type switch
                {
                    { IsReferenceType: true, IsRecord: true } => "record ",
                    { IsReferenceType: true } => "class ",
                    { IsValueType: true, IsRecord: true } => "record struct ",
                    { IsValueType: true } => "struct ",
                    _ => string.Empty
                }}}{{type.Name}}{{(type.TypeParameters is { Length: > 0 } typeParameters ? $"<{string.Join(", ", typeParameters.Select(static param => param.Name))}>" : string.Empty)}}
                    {{indentation}}{
                    """);

                PrintMembers(builder, compilation, indentation + IndentationPrintPrefix);

                builder.Append($$"""
                    {{indentation}}}

                    """);
            }

            public readonly void PrintMembers(StringBuilder builder, Compilation compilation, string indentation)
            {
                foreach (var import in ContainedImports.Values)
                {
                    import.Print(builder, compilation, indentation);
                }

                foreach (var (type, typeNode) in NestedTypes)
                {
                    typeNode.Print(type, builder, compilation, indentation);
                }
            }
        }

        private readonly NamespaceNode mRootNamespace = new();

        public readonly bool IsEmpty => mRootNamespace.IsEmpty;

        public readonly void Consolidate() => mRootNamespace.Consolidate(new());

        public readonly void Print(StringBuilder builder, Compilation compilation) => mRootNamespace.PrintMembers(builder, compilation, string.Empty);

        public readonly bool TryAddImportData(ImportData import, SourceProductionContext spc, Compilation compilation)
        {
            if (TryAddType(import.TargetMethod.ContainingType, mRootNamespace, spc, out var containingType))
            {
                if (!containingType.ContainedImports.TryGetValue(import.TargetMethod, out _))
                {
                    if (!import.Validate(spc, compilation))
                    {
                        return false;
                    }

                    containingType.ContainedImports.Add(import.TargetMethod, import);
                }

                return true;
            }

            return false;
        }

        private static bool TryAddType(INamedTypeSymbol type, NamespaceNode rootNamespace, SourceProductionContext spc, out TypeNode typeNode)
        {
            if ((type switch
            {
                { ContainingType: not null }
                    => TryAddType(type.ContainingType, rootNamespace, spc, out var containingType) ? containingType.NestedTypes : null,

                not null
                    => TryAddNamespace(type.ContainingNamespace, rootNamespace, out var containingNamespace) ? containingNamespace.ContainedTypes : null,

                _ => null
            }) is { } typeContainer)
            {
                if (!typeContainer.TryGetValue(type!, out typeNode))
                {
                    if (type is not { EnumUnderlyingType: null, DelegateInvokeMethod: null }
                        || !type.DeclaringSyntaxReferences.Any(syntaxRef => (syntaxRef.GetSyntax(spc.CancellationToken) as BaseTypeDeclarationSyntax)?.Modifiers.Any(SyntaxKind.PartialKeyword) is true))
                    {
                        spc.ReportDiagnostic(Diagnostic.Create(mUnsupportedTypeDeclarationSignatureDescriptor,
                            location: type!.OriginalDefinition.Locations.FirstOrDefault(),
                            type!.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat)
                        ));

                        return false;
                    }

                    typeContainer.Add(type, typeNode = new());
                }

                return true;
            }

            Unsafe.SkipInit(out typeNode);
            return false;
        }

        private static bool TryAddNamespace(INamespaceSymbol @namespace, NamespaceNode rootNamespace, out NamespaceNode namespaceNode)
        {
            if (@namespace is { IsGlobalNamespace: false })
            {
                if (TryAddNamespace(@namespace.ContainingNamespace, rootNamespace, out var containingNamespace))
                {
                    if (!containingNamespace.NestedNamespaces.TryGetValue(@namespace, out namespaceNode))
                    {
                        containingNamespace.NestedNamespaces.Add(@namespace, namespaceNode = new());
                    }

                    return true;
                }

                Unsafe.SkipInit(out namespaceNode);
                return false;
            }

            namespaceNode = rootNamespace;
            return true;
        }
    }
}
