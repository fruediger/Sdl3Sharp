using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading;

namespace Sdl3Sharp.SourceGeneration;

partial class SourceGenerator
{
    private static readonly DiagnosticDescriptor mUnsupportedMethodDeclarationSignatureDescriptor = new(
        id: $"{DiagnosticDescriptorIdPrefix}0021",
        title: "Unsupported target method declaration signature",
        messageFormat: "The target method declaration of \"{0}\" must be 'static' and 'partial' without a provided implementation part",
        category: DiagnosticDescriptorCategory,
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    private static readonly DiagnosticDescriptor mInvalidOrInaccessibleImportLibraryTypeDescriptor = new(
        id: $"{DiagnosticDescriptorIdPrefix}0022",
        title: $"Invalid or inaccessible import library type ({NativeImportLibraryTypeName}) for a imported native symbol",
        messageFormat: "The import library type \"{0}\" must be accessible from a global scope within the consuming assembly (at least 'internal'), in order for it be used as such",
        category: DiagnosticDescriptorCategory,
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    private static readonly DiagnosticDescriptor mInaccessibleConditionTypeDescriptor = new(
        id: $"{DiagnosticDescriptorIdPrefix}0023",
        title: $"Inaccessible condition type ({NativeImportConditionTypeName}) for a conditionally imported native symbol",
        messageFormat: "The condition type \"{0}\" must be accessible from a global scope within the consuming assembly (at least 'internal'), in order to use it as a condition for a conditonally imported native symbol",
        category: DiagnosticDescriptorCategory,
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    private static readonly DiagnosticDescriptor mUnsupportedKindForNativeImportSymbolDescriptor = new(
        id: $"{DiagnosticDescriptorIdPrefix}0030",
        title: "Unsupported kind for an imported native symbol",
        messageFormat: "The kind \"{0}\" for an imported native symbol is unknown and unsupported. Only {1} are allowed kind values.",
        category: DiagnosticDescriptorCategory,
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    private static readonly DiagnosticDescriptor mUnsupportedMethodDeclarationSignatureForAutoKindSymbolDescriptor = new(
        id: $"{DiagnosticDescriptorIdPrefix}0031",
        title: "Unsupported target method declaration signature for \"Auto\" kind imported native symbol",
        messageFormat:
            "The target method declaration of \"{0}\" must either take no arguments and return non-void or take exactly one argument and return void, in order to be used as an \"Auto\" kind imported native symbol. " +
            "The return type or the argument type respectively must be either a blittable ('unmanaged') type, a type parameter constrained to be 'unmanaged', or a 'ref'/'ref readonly'/'in' reference to such types.",
        category: DiagnosticDescriptorCategory,
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    private static readonly DiagnosticDescriptor mUnsupportedMethodDeclarationSignatureForGetterKindSymbolDescriptor = new(
        id: $"{DiagnosticDescriptorIdPrefix}0032",
        title: "Unsupported target method declaration signature for \"Getter\" kind imported native symbol",
        messageFormat:
            "The target method declaration of \"{0}\" must take no arguments and return non-void, in order to be used as a getter for an imported native symbol. " +
            "The return type must be either a blittable ('unmanaged') type, a type parameter constrained to be 'unmanaged', or a 'ref'/'ref readonly' reference to such types.",
        category: DiagnosticDescriptorCategory,
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    private static readonly DiagnosticDescriptor mUnsupportedMethodDeclarationSignatureForSetterKindSymbolDescriptor = new(
        id: $"{DiagnosticDescriptorIdPrefix}0033",
        title: "Unsupported target method declaration signature for \"Setter\" kind imported native symbol",
        messageFormat:
            "The target method declaration of \"{0}\" must take exactly one argument and return void, in order to be used as a setter fo an imported native symbol. " +
            "The argument type must be either a blittable ('unmanaged') type, a type parameter constrained to be 'unmanaged', or a 'ref'/'ref readonly'/'in' reference to such types.",
        category: DiagnosticDescriptorCategory,
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    private static readonly DiagnosticDescriptor mUnsupportedMethodDeclarationSignatureForReferenceKindSymbolDescriptor = new(
        id: $"{DiagnosticDescriptorIdPrefix}0034",
        title: "Unsupported target method declaration signature for \"Reference\" kind imported native symbol",
        messageFormat:
            "The target method declaration of \"{0}\" must take no arguments and return non-void by-ref, in order to be used as a reference getter for an imported native symbol. " +
            "The return type must be a 'ref'/'ref readonly' reference to either a blittable ('unmanaged') type, or a type parameter constrained to be 'unmanaged'.",
        category: DiagnosticDescriptorCategory,
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    private static readonly DiagnosticDescriptor mUnsupportedMethodDeclarationSignatureForFunctionDescriptor = new(
        id: $"{DiagnosticDescriptorIdPrefix}0041",
        title: "Unsupported target method declaration signature for imported native function",
        messageFormat: "The return type and all of the argument types of the target method declaration of \"{0}\" must be either a blittable ('unmanaged') type, a type parameter constrained to be 'unmanaged', or a 'ref'/'ref readonly'/'in' reference to such types",
        category: DiagnosticDescriptorCategory,
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    private abstract class ImportData
    {
        private static readonly SymbolDisplayFormat mPartialMethodDeclarationWithoutModifiersDisplayFormat = new(
            globalNamespaceStyle: SymbolDisplayGlobalNamespaceStyle.Included,
            typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces,
            genericsOptions: SymbolDisplayGenericsOptions.IncludeTypeParameters | SymbolDisplayGenericsOptions.IncludeTypeConstraints,
            memberOptions: SymbolDisplayMemberOptions.IncludeType | SymbolDisplayMemberOptions.IncludeExplicitInterface | SymbolDisplayMemberOptions.IncludeParameters | SymbolDisplayMemberOptions.IncludeRef,
            delegateStyle: SymbolDisplayDelegateStyle.NameOnly,
            parameterOptions: SymbolDisplayParameterOptions.IncludeExtensionThis | SymbolDisplayParameterOptions.IncludeModifiers | SymbolDisplayParameterOptions.IncludeType | SymbolDisplayParameterOptions.IncludeName | SymbolDisplayParameterOptions.IncludeDefaultValue,
            miscellaneousOptions: SymbolDisplayMiscellaneousOptions.UseSpecialTypes | SymbolDisplayMiscellaneousOptions.EscapeKeywordIdentifiers | SymbolDisplayMiscellaneousOptions.IncludeNullableReferenceTypeModifier | SymbolDisplayMiscellaneousOptions.AllowDefaultLiteral | SymbolDisplayMiscellaneousOptions.CollapseTupleTypes
        );

        public ITypeSymbol ImportLibraryType { get; }
        public ITypeSymbol? ConditionType { get; }
        public string? LibraryImplementationName { get; set; }
        public string SymbolName { get; set; }
        public IMethodSymbol TargetMethod { get; }
        public SyntaxTokenList TargetModifiers { get; }
        public Location? Location { get; }

        protected ImportData(GeneratorAttributeSyntaxContext gasc, CancellationToken cancellationToken, out AttributeData? attributeData)
            => (ImportLibraryType, ConditionType, LibraryImplementationName, SymbolName, TargetMethod, TargetModifiers, Location, attributeData) = gasc switch
            {
                {
                    TargetSymbol: IMethodSymbol targetMethod,
                    TargetNode: MethodDeclarationSyntax { Modifiers: var targetModifies },
                    Attributes: [{ ApplicationSyntaxReference: var syntaxRef, ConstructorArguments: var ctorArgs } firstAttributeData, ..]
                }
                    => (
                        importLibraryType: firstAttributeData.AttributeClass switch
                        {
                            { TypeArguments: [var importLibraryType, ..] } => importLibraryType,
                            _ => default!
                        },
                        conditionType: firstAttributeData.AttributeClass switch
                        {
                            { TypeArguments: [_, var conditionType, ..] } => conditionType,
                            _ => null
                        },
                        libraryImplementationName: default(string?),
                        symbolName: ctorArgs switch
                        {
                        [{ IsNull: false, Kind: TypedConstantKind.Primitive, Value: string symbolName }, ..] => symbolName,
                            _ => targetMethod.Name
                        },
                        targetMethod,
                        targetModifies,
                        location: syntaxRef?.GetSyntax(cancellationToken).GetLocation(),
                        attributeData: firstAttributeData
                    ),
                _ => default
            };

        public void Print(StringBuilder builder, Compilation compilation, string indentation)
        {
            builder.Append($$"""

                #pragma warning disable CS8500
                {{indentation}}[global::System.CodeDom.Compiler.GeneratedCode("{{mTool.Name}}", "{{mTool.Version}}")]
                {{indentation}}[global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining | global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization)]
                {{indentation}}{{TargetModifiers}} {{TargetMethod.ToDisplayString(mPartialMethodDeclarationWithoutModifiersDisplayFormat)}}
                {{indentation}}{
                {{indentation}}    unsafe
                {{indentation}}    {
                """);

            PrintImpl(builder, compilation, indentation);

            builder.Append($$"""

                {{indentation}}    }
                {{indentation}}}
                #pragma warning restore CS8500

                """);
        }

        protected abstract void PrintImpl(StringBuilder builder, Compilation compilation, string indentation);

        public bool Validate(SourceProductionContext spc, Compilation compilation)
        {
            if (TargetMethod is not { IsAbstract: false, IsStatic: true, IsPartialDefinition: true })
            {
                spc.ReportDiagnostic(Diagnostic.Create(mUnsupportedMethodDeclarationSignatureDescriptor,
                    location: Location,
                    TargetMethod.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat)
                ));

                return false;
            }

            if (ImportLibraryType is null || !compilation.IsSymbolAccessibleWithin(ImportLibraryType, compilation.Assembly))
            {
                spc.ReportDiagnostic(Diagnostic.Create(mInvalidOrInaccessibleImportLibraryTypeDescriptor,
                    location: Location,
                    ImportLibraryType?.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat) ?? "<null>"
                ));

                return false;
            }

            if (ConditionType is not null && !compilation.IsSymbolAccessibleWithin(ConditionType, compilation.Assembly))
            {
                spc.ReportDiagnostic(Diagnostic.Create(mInaccessibleConditionTypeDescriptor,
                    location: Location,
                    ConditionType.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat)
                ));

                return false;
            }

            return ValidateImpl(spc, compilation);
        }

        protected abstract bool ValidateImpl(SourceProductionContext spc, Compilation compilation);
    }

    private enum ImportSymbolKind
    {
        Auto = default,
        Getter,
        Setter,
        Reference
    }

    private sealed class ImportSymbolData : ImportData
    {
        private static readonly SymbolDisplayFormat mPointerDisplayFormat = new(
            globalNamespaceStyle: SymbolDisplayGlobalNamespaceStyle.Included,
            typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces,
            genericsOptions: SymbolDisplayGenericsOptions.IncludeTypeParameters,
            delegateStyle: SymbolDisplayDelegateStyle.NameOnly,
            miscellaneousOptions: SymbolDisplayMiscellaneousOptions.UseSpecialTypes | SymbolDisplayMiscellaneousOptions.EscapeKeywordIdentifiers | SymbolDisplayMiscellaneousOptions.IncludeNullableReferenceTypeModifier | SymbolDisplayMiscellaneousOptions.CollapseTupleTypes
        );

        private static readonly SymbolDisplayFormat mParameterDisplayFormat = new(
            parameterOptions: SymbolDisplayParameterOptions.IncludeName,
            miscellaneousOptions: SymbolDisplayMiscellaneousOptions.EscapeKeywordIdentifiers
        );

        public ImportSymbolKind Kind { get; private set; }

        public ImportSymbolData(GeneratorAttributeSyntaxContext gasc, CancellationToken cancellationToken) : base(gasc, cancellationToken, out var attributeData)
            => Kind = attributeData switch
            {
                { NamedArguments: var namedArgs }
                    when namedArgs.FirstOrDefault(static p => p.Key is NativeImportSymbolAttributeKindPropertyName).Value is { IsNull: false, Kind: TypedConstantKind.Enum, Value: int intKind }
                        => unchecked((ImportSymbolKind)intKind),
                _ => default
            };

        protected override void PrintImpl(StringBuilder builder, Compilation compilation, string indentation)
        {
            switch (Kind, TargetMethod)
            {
                case (ImportSymbolKind.Getter, { ReturnsByRef: true } or { ReturnsByRefReadonly: true }):
                    {
                        builder.Append($$"""

                            {{indentation}}        return ref **unchecked(({{compilation.CreatePointerTypeSymbol(TargetMethod.ReturnType).ToDisplayString(mPointerDisplayFormat)}}*)global::{{LibraryImplementationName}}.{{SymbolName}});
                            """);
                    }
                    break;

                case (ImportSymbolKind.Getter, _):
                    {
                        builder.Append($$"""

                            {{indentation}}        return *unchecked(({{compilation.CreatePointerTypeSymbol(TargetMethod.ReturnType).ToDisplayString(mPointerDisplayFormat)}})global::{{LibraryImplementationName}}.{{SymbolName}});
                            """);
                    }
                    break;

                case (ImportSymbolKind.Setter, { Parameters: [{ RefKind: RefKind.Ref or RefKind.In or RefKind.RefReadOnlyParameter, Name: var paramName, Type: var paramType } param, ..] }):
                    {
                        var pointerTypeName = compilation.CreatePointerTypeSymbol(paramType).ToDisplayString(mPointerDisplayFormat);

                        builder.Append($$"""

                            {{indentation}}        fixed({{pointerTypeName}} p_{{paramName}} = &{{param.ToDisplayString(mParameterDisplayFormat)}})
                            {{indentation}}        {
                            {{indentation}}            *unchecked(({{pointerTypeName}}*)global::{{LibraryImplementationName}}.{{SymbolName}}) = p_{{paramName}};
                            {{indentation}}        }
                            """);
                    }
                    break;

                case (ImportSymbolKind.Setter, { Parameters: [{ Type: var paramType } param, ..] }):
                    {
                        builder.Append($$"""

                            {{indentation}}        *unchecked(({{compilation.CreatePointerTypeSymbol(paramType).ToDisplayString(mPointerDisplayFormat)}})global::{{LibraryImplementationName}}.{{SymbolName}}) = {{param.ToDisplayString(mParameterDisplayFormat)}};
                            """);
                    }
                    break;

                case (ImportSymbolKind.Reference, _):
                    {
                        builder.Append($$"""

                            {{indentation}}        return ref *unchecked(({{compilation.CreatePointerTypeSymbol(TargetMethod.ReturnType).ToDisplayString(mPointerDisplayFormat)}})global::{{LibraryImplementationName}}.{{SymbolName}});
                            """);
                    }
                    break;
            }
        }

        protected override bool ValidateImpl(SourceProductionContext spc, Compilation compilation)
        {
            switch (Kind, TargetMethod)
            {
                case (ImportSymbolKind.Auto, { ReturnsVoid: false, ReturnType.IsUnmanagedType: true, Parameters: [] }):
                    {
                        Kind = ImportSymbolKind.Getter;
                    }
                    break;

                case (ImportSymbolKind.Auto, { ReturnsVoid: true, Parameters: [{ Type.IsUnmanagedType: true }] }):
                    {
                        Kind = ImportSymbolKind.Setter;
                    }
                    break;

                case (ImportSymbolKind.Auto, _):
                    {
                        spc.ReportDiagnostic(Diagnostic.Create(mUnsupportedMethodDeclarationSignatureForAutoKindSymbolDescriptor,
                            location: Location,
                            TargetMethod.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat)
                        ));
                    }
                    return false;

                case (ImportSymbolKind.Getter, not { ReturnsVoid: false, ReturnType.IsUnmanagedType: true, Parameters: [] }):
                    {
                        spc.ReportDiagnostic(Diagnostic.Create(mUnsupportedMethodDeclarationSignatureForGetterKindSymbolDescriptor,
                            location: Location,
                            TargetMethod.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat)
                        ));
                    }
                    return false;

                case (ImportSymbolKind.Setter, not { ReturnsVoid: true, Parameters: [{ Type.IsUnmanagedType: true }] }):
                    {
                        spc.ReportDiagnostic(Diagnostic.Create(mUnsupportedMethodDeclarationSignatureForSetterKindSymbolDescriptor,
                            location: Location,
                            TargetMethod.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat)
                        ));
                    }
                    return false;

                case (ImportSymbolKind.Reference, not { ReturnsVoid: false, ReturnType.IsUnmanagedType: true, Parameters: [] }):
                case (ImportSymbolKind.Reference, not ({ ReturnsByRef: true } or { ReturnsByRefReadonly: true })):
                    {
                        spc.ReportDiagnostic(Diagnostic.Create(mUnsupportedMethodDeclarationSignatureForReferenceKindSymbolDescriptor,
                            location: Location,
                            TargetMethod.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat)
                        ));
                    }
                    return false;

                case (not (ImportSymbolKind.Auto or ImportSymbolKind.Getter or ImportSymbolKind.Setter or ImportSymbolKind.Reference), _):
                    {
                        spc.ReportDiagnostic(Diagnostic.Create(mUnsupportedKindForNativeImportSymbolDescriptor,
                            location: Location,
                            Kind,
                            $"\"{NativeImportSymbolKindAutoMemberName}\", \"{NativeImportSymbolKindGetterMemberName}\", \"{NativeImportSymbolKindSetterMemberName}\", or \"{NativeImportSymbolKindReferenceMemberName}\""
                        ));
                    }
                    return false;
            }

            return true;
        }
    }

    private sealed class ImportFunctionData : ImportData
    {
        private static readonly SymbolDisplayFormat mFunctionPointerDisplayFormat = new(
            globalNamespaceStyle: SymbolDisplayGlobalNamespaceStyle.Included,
            typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces,
            genericsOptions: SymbolDisplayGenericsOptions.IncludeTypeParameters,
            delegateStyle: SymbolDisplayDelegateStyle.NameOnly,
            parameterOptions: SymbolDisplayParameterOptions.IncludeModifiers | SymbolDisplayParameterOptions.IncludeType,
            miscellaneousOptions: SymbolDisplayMiscellaneousOptions.UseSpecialTypes | SymbolDisplayMiscellaneousOptions.EscapeKeywordIdentifiers | SymbolDisplayMiscellaneousOptions.IncludeNullableReferenceTypeModifier | SymbolDisplayMiscellaneousOptions.CollapseTupleTypes
        );

        private static readonly SymbolDisplayFormat mParameterDisplayFormat = new(
            parameterOptions: SymbolDisplayParameterOptions.IncludeName,
            miscellaneousOptions: SymbolDisplayMiscellaneousOptions.EscapeKeywordIdentifiers
        );

        public ImmutableArray<TypedConstant> CallConvs { get; }

        public ImportFunctionData(GeneratorAttributeSyntaxContext gasc, CancellationToken cancellationToken) : base(gasc, cancellationToken, out var attributeData)
            => CallConvs = attributeData switch
            {
                { NamedArguments: var namedArgs }
                    when namedArgs.FirstOrDefault(static p => p.Key is NativeImportFunctionAttributeCallConvsPropertyName).Value is { IsNull: false, Kind: TypedConstantKind.Array, Values: var callConvs }
                        => callConvs,
                _ => []
            };

        protected override void PrintImpl(StringBuilder builder, Compilation compilation, string indentation)
        {
            var functionPointerType = compilation.CreateFunctionPointerTypeSymbol(
                returnType: TargetMethod.ReturnType,
                returnRefKind: TargetMethod switch { { ReturnsByRef: true } => RefKind.Ref, { ReturnsByRefReadonly: true } => RefKind.RefReadOnly, _ => RefKind.None },
                parameterTypes: [..TargetMethod.Parameters.Select(static param => param.Type)],
                parameterRefKinds: [..TargetMethod.Parameters.Select(static param => param.RefKind)],
                callingConvention: System.Reflection.Metadata.SignatureCallingConvention.Unmanaged,
                callingConventionTypes: [..CallConvs.Select(static typedConstant => typedConstant.Value).OfType<INamedTypeSymbol>()]
            );

            builder.Append($$"""

                {{indentation}}        {{TargetMethod switch
            {
                { ReturnsVoid: false, ReturnsByRef: true } or { ReturnsVoid: false, ReturnsByRefReadonly: true } => "return ref ",
                { ReturnsVoid: false } => "return ",
                _ => string.Empty
            }}}unchecked(({{functionPointerType.ToDisplayString(mFunctionPointerDisplayFormat)}})global::{{LibraryImplementationName}}.{{SymbolName}})({{string.Join(", ", TargetMethod.Parameters.Select(static param => $"{param.RefKind switch
            {
                RefKind.Ref => $"ref ",
                RefKind.Out => $"out ",
                RefKind.In or RefKind.RefReadOnlyParameter => $"in ",
                _ => string.Empty
            } }{param.ToDisplayString(mParameterDisplayFormat)}"))}});
                """);
        }

        protected override bool ValidateImpl(SourceProductionContext spc, Compilation compilation)
        {
            if (TargetMethod is { ReturnsVoid: false, ReturnType.IsUnmanagedType: false }
                || TargetMethod.Parameters.Any(static param => param is not { Type.IsUnmanagedType: true }))
            {
                spc.ReportDiagnostic(Diagnostic.Create(mUnsupportedMethodDeclarationSignatureForFunctionDescriptor,
                    location: Location,
                    TargetMethod.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat)
                ));

                return false;
            }

            return true;
        }
    }
}
