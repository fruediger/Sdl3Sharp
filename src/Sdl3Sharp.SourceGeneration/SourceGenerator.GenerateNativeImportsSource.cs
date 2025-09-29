using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Sdl3Sharp.SourceGeneration;

partial class SourceGenerator
{
    private const string LibraryIdentifierFormat = @"_Lib{0}";
    private const string SymbolIdentifierFormat = @"_Sym{0}";
    private const string ConditionLocalFormat = @"b{0}";

    private const string GeneratedImportsOutputFileName = $"{nameof(Sdl3Sharp)}.{nameof(SourceGeneration)}.NativeImports.g.cs";

    private static readonly DiagnosticDescriptor mCompilationDoesNotAllowUnsafeDescriptor = new(
        id: $"{DiagnosticDescriptorIdPrefix}0001",
        title: "Compilation does not allow unsafe regions",
        messageFormat: "The compilation does not allow for usage of unsafe regions/blocks. This is needed in order for the generated source code to work. Allow it by adding \"<AllowUnsafeBlocks>true</AllowUnsafeBlocks>\" to the project file, or by compiling with the \"-unsafe\" option.",
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

    private static void GenerateNativeImportsSource(SourceProductionContext spc, (Compilation compilation, (ImmutableArray<ImportSymbolData> symbolImports, (ImmutableArray<ImportSymbolData> conditionalSymbolImports, (ImmutableArray<ImportFunctionData> functionImports, ImmutableArray<ImportFunctionData> conditionalFunctionImports)))) data)
    {
        var (compilation, (symbolImports, (conditionalSymbolImports, (functionImports, conditionalFunctionImports)))) = data;

        if (symbolImports.IsDefaultOrEmpty && conditionalSymbolImports.IsDefaultOrEmpty && functionImports.IsDefaultOrEmpty && conditionalFunctionImports.IsDefaultOrEmpty)
        {
            return;
        }

        if (compilation is not CSharpCompilation { Options.AllowUnsafe: true })
        {
            spc.ReportDiagnostic(Diagnostic.Create(mCompilationDoesNotAllowUnsafeDescriptor,
                location: default
            ));

            return;
        }

        var libraries = new Dictionary<ITypeSymbol, (Dictionary<string, List<ImportData>> unconditionalImports, Dictionary<ITypeSymbol, Dictionary<string, List<ImportData>>> conditionalImports)>(SymbolEqualityComparer.Default);
        var buildTree = new BuildTree();

        foreach (var import in symbolImports.AsEnumerable<ImportData>().Concat(conditionalSymbolImports.AsEnumerable<ImportData>()).Concat(functionImports.AsEnumerable<ImportData>()).Concat(conditionalFunctionImports.AsEnumerable<ImportData>()))
        {
            if (buildTree.TryAddImportData(import, spc, compilation))
            {
                if (!libraries.TryGetValue(import.ImportLibraryType, out var library))
                {
                    libraries.Add(key: import.ImportLibraryType, library = (unconditionalImports: [], conditionalImports: new(SymbolEqualityComparer.Default)));
                }

                Dictionary<string, List<ImportData>> importsBySymbolName;
                if (import.ConditionType is null)
                {
                    importsBySymbolName = library.unconditionalImports;
                }
                else
                {
                    if (!library.conditionalImports.TryGetValue(import.ConditionType, out importsBySymbolName))
                    {
                        library.conditionalImports.Add(key: import.ConditionType, importsBySymbolName = []);
                    }
                }

                if (!importsBySymbolName.TryGetValue(import.SymbolName, out var imports))
                {
                    importsBySymbolName.Add(key: import.SymbolName, imports = []);
                }

                imports.Add(import);
            }
        }

        buildTree.Consolidate();

        if (buildTree.IsEmpty)
        {
            return;
        }

        var builder = new StringBuilder();

        builder.Append($$"""
            #nullable enable

            """);

        var libraryId = 0;

        foreach (var (importLibraryType, (unconditionalImports, conditionalImports)) in libraries)
        {
            if (unconditionalImports.Count is not > 0 && conditionalImports.Sum(static t => t.Value.Count) is not > 0)
            {
                continue;
            }

            var libraryIdentifier = string.Format(CultureInfo.InvariantCulture, LibraryIdentifierFormat, libraryId++);
            var importLibraryTypeName = importLibraryType.ToDisplayString(mDefaultTypeSymbolDisplayFormat);

            builder.Append($$"""

                [global::System.CodeDom.Compiler.GeneratedCode("{{mTool.Name}}", "{{mTool.Version}}")]
                file static class {{libraryIdentifier}}
                {
                """);

            var symbolId = 0;

            foreach (var (_, imports) in unconditionalImports)
            {
                var symbolIdentifier = string.Format(CultureInfo.InvariantCulture, SymbolIdentifierFormat, symbolId++);

                foreach (var import in imports)
                {
                    import.LibraryImplementationName = libraryIdentifier;
                    import.SymbolName = symbolIdentifier;
                }

                builder.Append($$"""

                        internal static global::System.IntPtr {{symbolIdentifier}};

                    """);
            }

            foreach (var (_, importsBySymbolName) in conditionalImports)
            {
                foreach (var (_, imports) in importsBySymbolName)
                {
                    var symbolIdentifier = string.Format(CultureInfo.InvariantCulture, SymbolIdentifierFormat, symbolId++);

                    foreach (var import in imports)
                    {
                        import.LibraryImplementationName = libraryIdentifier;
                        import.SymbolName = symbolIdentifier;
                    }

                    builder.Append($$"""

                            internal static global::System.IntPtr {{symbolIdentifier}};

                        """);
                }
            }

            builder.Append($$"""

                    [global::System.Runtime.CompilerServices.SkipLocalsInit]
                    [global::System.Runtime.CompilerServices.ModuleInitializer]
                    internal static void ModuleInitializer()
                    {       
                """);

            int conditionId;

            if (unconditionalImports.Count is not > 0)
            {
                conditionId = 0;
                foreach (var (conditionType, importsBySymbolName) in conditionalImports)
                {
                    if (importsBySymbolName.Count is not > 0)
                    {
                        continue;
                    }

                    builder.Append($$"""
                        
                                var {{string.Format(CultureInfo.InvariantCulture, ConditionLocalFormat, conditionId++)}} = global::{{NativeImportAttributesNamespaceName}}.{{NativeImportConditionTypeName}}.{{NativeImportConditionEvaluateMethodName}}<{{conditionType.ToDisplayString(mDefaultTypeSymbolDisplayFormat)}}>();
                        """);
                }

                if (conditionId is > 0)
                {
                    builder.Append($$"""


                                if (!({{string.Join(" || ", Enumerable.Range(0, conditionId).Select(static id => string.Format(CultureInfo.InvariantCulture, ConditionLocalFormat, id)))}}))
                                {
                                    return;
                                }

                        """);
                }
            }

            builder.Append($$"""

                        global::System.Runtime.ExceptionServices.ExceptionDispatchInfo? info;
                        string? libraryName;
                        global::System.Runtime.InteropServices.DllImportSearchPath? searchPath;
                        global::System.IntPtr libraryHandle;

                        do
                        {
                            (libraryName, searchPath) = global::{{NativeImportAttributesNamespaceName}}.{{NativeImportLibraryTypeName}}.{{NativeImportLibraryGetLibraryNameAndSearchPathMethodName}}<{{importLibraryTypeName}}>();

                            if (!string.IsNullOrWhiteSpace(libraryName))
                            {
                                try
                                {
                                    libraryHandle = global::System.Runtime.InteropServices.NativeLibrary.Load(libraryName, typeof(global::{{libraryIdentifier}}).Assembly, searchPath);

                                    break;
                                }
                                catch (global::System.Exception exception)
                                {
                                    info = global::System.Runtime.ExceptionServices.ExceptionDispatchInfo.Capture(exception);
                                }
                            }                
                            else
                            {
                                info = null;
                            }

                            if (!global::{{NativeImportAttributesNamespaceName}}.{{NativeImportLibraryTypeName}}.{{NativeImportLibraryHandleLibraryImportErrorMethodName}}<{{importLibraryTypeName}}>(libraryName, searchPath, info))
                            {
                                return;
                            }
                        }
                        while (true);

                """);

            foreach (var (symbolName, imports) in unconditionalImports)
            {
                if (imports is not [{ SymbolName: var symbolIdentifier }, ..])
                {
                    continue;
                }

                builder.Append($$"""

                            do
                            {                    
                                if (!string.IsNullOrWhiteSpace("{{symbolName}}"))
                                {
                                    try
                                    {
                                        {{symbolIdentifier}} = global::System.Runtime.InteropServices.NativeLibrary.GetExport(libraryHandle, "{{symbolName}}");

                                        break;
                                    }
                                    catch (global::System.Exception exception)
                                    {
                                        info = global::System.Runtime.ExceptionServices.ExceptionDispatchInfo.Capture(exception);
                                    }
                                }
                                else
                                {
                                    info = null;
                                }

                                if (!global::{{NativeImportAttributesNamespaceName}}.{{NativeImportLibraryTypeName}}.{{NativeImportLibraryHandleSymbolImportErrorMethodName}}<{{importLibraryTypeName}}>("{{symbolName}}", info))
                                {
                                    return;
                                }
                            }
                            while (false);

                    """);
            }

            conditionId = 0;
            foreach (var (conditionType, importsBySymbolName) in conditionalImports)
            {
                if (importsBySymbolName.Count is not > 0)
                {
                    continue;
                }

                builder.Append($$"""

                            if ({{unconditionalImports.Count switch
                {
                    > 0 => $"global::{NativeImportAttributesNamespaceName}.{NativeImportConditionTypeName}.{NativeImportConditionEvaluateMethodName}<{conditionType.ToDisplayString(mDefaultTypeSymbolDisplayFormat)}>()",
                    _ => string.Format(CultureInfo.InvariantCulture, ConditionLocalFormat, conditionId++)
                }}})
                            {
                    """);

                foreach (var (symbolName, imports) in importsBySymbolName)
                {
                    if (imports is not [{ SymbolName: var symbolIdentifier }, ..])
                    {
                        continue;
                    }

                    builder.Append($$"""

                                    do
                                    {                    
                                        if (!string.IsNullOrWhiteSpace("{{symbolName}}"))
                                        {
                                            try
                                            {
                                                {{symbolIdentifier}} = global::System.Runtime.InteropServices.NativeLibrary.GetExport(libraryHandle, "{{symbolName}}");

                                                break;
                                            }
                                            catch (global::System.Exception exception)
                                            {
                                                info = global::System.Runtime.ExceptionServices.ExceptionDispatchInfo.Capture(exception);
                                            }
                                        }
                                        else
                                        {
                                            info = null;
                                        }

                                        if (!global::{{NativeImportAttributesNamespaceName}}.{{NativeImportLibraryTypeName}}.{{NativeImportLibraryHandleSymbolImportErrorMethodName}}<{{importLibraryTypeName}}>("{{symbolName}}", info))
                                        {
                                            return;
                                        }
                                    }
                                    while (false);

                        """);
                }

                builder.Append("""
                            }

                    """);
            }

            builder.Append($$"""

                        global::{{NativeImportAttributesNamespaceName}}.{{NativeImportLibraryTypeName}}.{{NativeImportLibraryAfterSuccessfullyLoadedMethodName}}<{{importLibraryTypeName}}>(libraryName, searchPath);
                    }
                }

                """);
        }

        buildTree.Print(builder, compilation);

        builder.Append("""

            #nullable restore
            """);

        spc.AddSource(GeneratedImportsOutputFileName, SourceText.From(text: builder.ToString(), encoding: Encoding.UTF8));
    }
}
