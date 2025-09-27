using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System.Text;

namespace Sdl3Sharp.SourceGeneration;

partial class SourceGenerator
{
    private const string NativeImportAttributesNamespaceName = $"{nameof(Sdl3Sharp)}.{nameof(SourceGeneration)}";

    private const string NativeImportSymbolKindTypeName = "NativeImportSymbolKind";
    private const string NativeImportSymbolKindAutoMemberName = "Auto";
    private const string NativeImportSymbolKindGetterMemberName = "Getter";
    private const string NativeImportSymbolKindSetterMemberName = "Setter";
    private const string NativeImportSymbolKindReferenceMemberName = "Reference";

    private const string NativeImportLibraryTypeName = "INativeImportLibrary";

    private const string NativeImportLibraryGetLibraryNameAndSearchPathMethodName = "GetLibraryNameAndSearchPath";
    private const string NativeImportLibraryGetLibraryNameAndSearchPathResultLibraryNamePartName = "libraryName";
    private const string NativeImportLibraryGetLibraryNameAndSearchPathResultSearchPathPartName = "searchPath";

    private const string NativeImportLibraryHandleLibraryImportErrorMethodName = "HandleLibraryImportError";
    private const string NativeImportLibraryHandleSymbolImportErrorMethodName = "HandleSymbolImportError";

    private const string NativeImportLibraryAfterSuccessfullyLoadedMethodName = "AfterSuccessfullyLoaded";

    private const string NativeImportConditionTypeName = "INativeImportCondition";
    private const string NativeImportConditionEvaluateMethodName = "Evaluate";

    private const string NativeImportAttributeLibraryTypeParameterName = "TLibrary";
    private const string NativeImportAttributeConditionTypeParameterName = "TCondition";

    private const string NativeImportAttributeSymbolNamePropertyName = "SymbolName";

    private const string NativeImportSymbolAttributeTypeName = "NativeImportSymbolAttribute";
    private const string NativeImportSymbolAttributeKindPropertyName = "Kind";

    private const string NativeImportFunctionAttributeTypeName = "NativeImportFunctionAttribute";
    private const string NativeImportFunctionAttributeCallConvsPropertyName = "CallConvs";


    private const string GeneratedAttributesOutputFileName = $"{nameof(Sdl3Sharp)}.{nameof(SourceGeneration)}.NativeImportAttributes.g.cs";

    private static void GenerateNativeImportAttributesSource(IncrementalGeneratorPostInitializationContext pic)
    {
        var symbolNameParameterName = NativeImportAttributeSymbolNamePropertyName switch
        {
            [var head, .. var tail] => $"{char.ToLowerInvariant(head)}{tail}",
            _ => string.Empty
        };

        pic.AddSource(GeneratedAttributesOutputFileName, SourceText.From(
            text: $$"""
                #nullable enable

                namespace {{NativeImportAttributesNamespaceName}};

                /// <summary>Controls the kind of access to an <see cref="{{NativeImportSymbolAttributeTypeName}}{{{NativeImportAttributeLibraryTypeParameterName}}}">imported native symbol</see></summary>
                internal enum {{NativeImportSymbolKindTypeName}}
                {
                    /// <summary>Automatically chooses between <see cref="{{NativeImportSymbolKindGetterMemberName}}">{{NativeImportSymbolKindGetterMemberName}}</see> and <see cref="{{NativeImportSymbolKindSetterMemberName}}">{{NativeImportSymbolKindSetterMemberName}}</see> based off the target method's declaration signature</summary>
                    /// <remarks><see cref="{{NativeImportSymbolKindReferenceMemberName}}">{{NativeImportSymbolKindReferenceMemberName}}</see> is not considered when using <see cref="{{NativeImportSymbolKindAutoMemberName}}">{{NativeImportSymbolKindAutoMemberName}}</see></remarks>
                    {{NativeImportSymbolKindAutoMemberName}} = {{unchecked((int)ImportSymbolKind.Auto)}},

                    /// <summary>Provides getter access to the imported native symbol</summary>
                    /// <remarks>
                    /// If the target method returns by <c>ref</c> or <c>ref readonly</c>, then the imported native symbol is considered to be of a by-ref type to underlying type itself.
                    /// This is in contrast to how <see cref="{{NativeImportSymbolKindReferenceMemberName}}">{{NativeImportSymbolKindReferenceMemberName}}</see> is handled.
                    /// </remarks>
                    {{NativeImportSymbolKindGetterMemberName}} = {{unchecked((int)ImportSymbolKind.Getter)}},

                    /// <summary>Provides setter access to the imported native symbol</summary>
                    /// <remarks>
                    /// If the target method accepts a <c>ref</c> or <c>ref readonly</c>/<c>in</c> parameter, then the imported native symbol is considered to be of a by-ref type to underlying type itself.
                    /// NOTE: When using such a setter, the provided reference is temporarily <c>fixed</c> and if the provided reference is managed, its address might unnoticeably change later on.
                    /// This is in contrast to how <see cref="{{NativeImportSymbolKindReferenceMemberName}}">{{NativeImportSymbolKindReferenceMemberName}}</see> is handled.    
                    /// </remarks>
                    {{NativeImportSymbolKindSetterMemberName}} = {{unchecked((int)ImportSymbolKind.Setter)}},

                    /// <summary>Provides reference access to the imported native symbol</summary>
                    /// <remarks>
                    /// The target methods needs to return by <c>ref</c> or <c>ref readonly</c> and the imported native symbol is considered to be of the underlying type of the by-ref return.
                    /// This is in contrast to how <see cref="{{NativeImportSymbolKindGetterMemberName}}">{{NativeImportSymbolKindGetterMemberName}}</see> and <see cref="{{NativeImportSymbolKindSetterMemberName}}">{{NativeImportSymbolKindSetterMemberName}}</see> are handled.
                    /// </remarks>
                    {{NativeImportSymbolKindReferenceMemberName}} = {{unchecked((int)ImportSymbolKind.Reference)}}
                }

                /// <summary>Represents a native library</summary>
                /// <remarks>More specifically, this type defines a way to resolve and import a native library</remarks>
                internal interface {{NativeImportLibraryTypeName}}
                {                    
                    /// <summary>Gets the name and the search path of the native library to import</summary>
                    /// <returns>
                    /// The name and the search path of the native library to import as a value tuple.
                    /// The <c>{{NativeImportLibraryGetLibraryNameAndSearchPathResultLibraryNamePartName}}</c> part of this result can be <see langword="null"/>, in which case no more attempts to import the native library are made
                    /// and all subsequent symbol imports from this native library are skipped.
                    /// The <c>{{NativeImportLibraryGetLibraryNameAndSearchPathResultSearchPathPartName}}</c> part of this result has the same meaning as the <c>searchPath</c> parameter of <see cref="System.Runtime.InteropServices.NativeLibrary.Load(string, System.Reflection.Assembly, System.Runtime.InteropServices.DllImportSearchPath?)"/>
                    /// and can be <see langword="null"/>.
                    /// </returns>
                    /// <remarks>This method might get called multiple times as a result of a call to <see cref="{{NativeImportLibraryHandleLibraryImportErrorMethodName}}(string?, System.Runtime.InteropServices.DllImportSearchPath?, System.Runtime.ExceptionServices.ExceptionDispatchInfo?)">{{NativeImportLibraryHandleLibraryImportErrorMethodName}}</see></remarks>
                    internal static abstract (string? {{NativeImportLibraryGetLibraryNameAndSearchPathResultLibraryNamePartName}}, global::System.Runtime.InteropServices.DllImportSearchPath? {{NativeImportLibraryGetLibraryNameAndSearchPathResultSearchPathPartName}}) {{NativeImportLibraryGetLibraryNameAndSearchPathMethodName}}();

                    /// <summary>Handles an error that occured during an attempt to import the native library</summary>
                    /// <param name="libraryName">
                    /// The <c>{{NativeImportLibraryGetLibraryNameAndSearchPathResultLibraryNamePartName}}</c> part of the result of the last call to <see cref="{{NativeImportLibraryGetLibraryNameAndSearchPathMethodName}}">{{NativeImportLibraryGetLibraryNameAndSearchPathMethodName}}</see> and the name of the native library that should have been imported.
                    /// This value might be <see langword="null"/>, or an empty or whitespace-only string, in which case <paramref name="libraryLoadErrorInfo"/> might be <see langword="null"/>
                    /// and the error to handle is an invalid library name.
                    /// </param>
                    /// <param name="searchPath">
                    /// The <c>{{NativeImportLibraryGetLibraryNameAndSearchPathResultSearchPathPartName}}</c> part of the result of the last call to <see cref="{{NativeImportLibraryGetLibraryNameAndSearchPathMethodName}}">{{NativeImportLibraryGetLibraryNameAndSearchPathMethodName}}</see> and the search path of the native library that should have been imported.
                    /// This value might be <see langword="null"/>.
                    /// </param>
                    /// <param name="libraryLoadErrorInfo">
                    /// The captured exception that occured during the attempt to import the native library.
                    /// This value might be <see langword="null"/>, in which case the error to handle is that <paramref name="libraryName"/> represents an invalid library name.
                    /// </param>
                    /// <returns>
                    /// <see langword="true"/> if the error handling is considered successful and another attempt to import the native library is made
                    /// by firstly calling <see cref="{{NativeImportLibraryGetLibraryNameAndSearchPathMethodName}}">{{NativeImportLibraryGetLibraryNameAndSearchPathMethodName}}</see> again;
                    /// otherwise <see langword="false"/>, in which case no more attempts to import the native library are made
                    /// and all subsequent symbol imports from this native library are skipped
                    /// </returns>
                    /// <remarks>
                    /// This methods defaults to rethrowing any non-<see langword="null"/> <paramref name="libraryLoadErrorInfo"/>s or otherwise returning <see langword="false"/>,
                    /// if not overwritten
                    /// </remarks>
                    internal static virtual bool {{NativeImportLibraryHandleLibraryImportErrorMethodName}}(string? libraryName, global::System.Runtime.InteropServices.DllImportSearchPath? searchPath, global::System.Runtime.ExceptionServices.ExceptionDispatchInfo? libraryLoadErrorInfo)
                    {
                        libraryLoadErrorInfo?.Throw();

                        return false;
                    }

                    /// <summary>Handles an error that occured during an attempt to import a symbol from the native library</summary>
                    /// <param name="symbolName">
                    /// The name of the symbol to should have been imported from the native library.
                    /// This value might be <see langword="null"/>, or an empty or whitespace-only string, in which case <paramref name="symbolLoadErrorInfo"/> might be <see langword="null"/>
                    /// and the error to handle is an invalid symbol name.
                    /// </param>
                    /// <param name="symbolLoadErrorInfo">
                    /// The exception that occured during the attempt to import the symbol from the native libary.
                    /// This value might be <see langword="null"/>, in which case the error to handle is that <paramref name="symbolName"/> represents an invalid symbol name.
                    /// </param>
                    /// <returns>
                    /// <see langword="true"/> if the error handling is considered successful and subsequent symbol imports from this native library should proceed as normal;
                    /// otherwise <see langword="false"/>, in which case all subsequent symbol imports from this native library are skipped
                    /// </returns>
                    /// <remarks>
                    /// This methods defaults to rethrowing any non-<see langword="null"/> <paramref name="symbolLoadErrorInfo"/>s or otherwise returning <see langword="true"/>,
                    /// if not overwritten
                    /// </remarks>
                    internal static virtual bool {{NativeImportLibraryHandleSymbolImportErrorMethodName}}(string? symbolName, global::System.Runtime.ExceptionServices.ExceptionDispatchInfo? symbolLoadErrorInfo)
                    {
                        symbolLoadErrorInfo?.Throw();

                        return true;
                    }

                    /// <summary>Called after a library and all of its symbol imports were successfully loaded</summary>
                    /// <param name="libraryName">
                    /// The name of the successfully loaded library (that is the <c>{{NativeImportLibraryGetLibraryNameAndSearchPathResultLibraryNamePartName}}</c> part of the result of the last call to
                    /// <see cref="{{NativeImportLibraryGetLibraryNameAndSearchPathMethodName}}">{{NativeImportLibraryGetLibraryNameAndSearchPathMethodName}}</see> which lead to the successful importation)
                    /// </param>
                    /// <param name="searchPath">
                    /// The search path of the successfully loaded library (that is the <c>{{NativeImportLibraryGetLibraryNameAndSearchPathResultSearchPathPartName}}</c> part of the result of the last call to
                    /// <see cref="{{NativeImportLibraryGetLibraryNameAndSearchPathMethodName}}">{{NativeImportLibraryGetLibraryNameAndSearchPathMethodName}}</see> which lead to the successful importation).
                    /// This value might be <see langword="null"/>.
                    /// </param>
                    internal static virtual void {{NativeImportLibraryAfterSuccessfullyLoadedMethodName}}(string libraryName, global::System.Runtime.InteropServices.DllImportSearchPath? searchPath)
                    { }

                    /// <remarks>For internal use only</remarks>
                    [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining | global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization)]
                    [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
                    internal static (string? {{NativeImportLibraryGetLibraryNameAndSearchPathResultLibraryNamePartName}}, global::System.Runtime.InteropServices.DllImportSearchPath? {{NativeImportLibraryGetLibraryNameAndSearchPathResultSearchPathPartName}}) {{NativeImportLibraryGetLibraryNameAndSearchPathMethodName}}<{{NativeImportAttributeLibraryTypeParameterName}}>()
                        where {{NativeImportAttributeLibraryTypeParameterName}} : notnull, {{NativeImportLibraryTypeName}}
                        => {{NativeImportAttributeLibraryTypeParameterName}}.{{NativeImportLibraryGetLibraryNameAndSearchPathMethodName}}();

                    /// <remarks>For internal use only</remarks>
                    [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining | global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization)]
                    [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
                    internal static bool {{NativeImportLibraryHandleLibraryImportErrorMethodName}}<{{NativeImportAttributeLibraryTypeParameterName}}>(string? libraryName, global::System.Runtime.InteropServices.DllImportSearchPath? searchPath, global::System.Runtime.ExceptionServices.ExceptionDispatchInfo? libraryLoadErrorInfo)
                        where {{NativeImportAttributeLibraryTypeParameterName}} : notnull, {{NativeImportLibraryTypeName}}
                        => {{NativeImportAttributeLibraryTypeParameterName}}.{{NativeImportLibraryHandleLibraryImportErrorMethodName}}(libraryName, searchPath, libraryLoadErrorInfo);

                    /// <remarks>For internal use only</remarks>
                    [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining | global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization)]
                    [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
                    internal static bool {{NativeImportLibraryHandleSymbolImportErrorMethodName}}<{{NativeImportAttributeLibraryTypeParameterName}}>(string? symbolName, global::System.Runtime.ExceptionServices.ExceptionDispatchInfo? symbolLoadErrorInfo)
                        where {{NativeImportAttributeLibraryTypeParameterName}} : notnull, {{NativeImportLibraryTypeName}}
                        => {{NativeImportAttributeLibraryTypeParameterName}}.{{NativeImportLibraryHandleSymbolImportErrorMethodName}}(symbolName, symbolLoadErrorInfo);

                    /// <remarks>For internal use only</remarks>
                    [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining | global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization)]
                    [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
                    internal static void {{NativeImportLibraryAfterSuccessfullyLoadedMethodName}}<{{NativeImportAttributeLibraryTypeParameterName}}>(string libraryName, global::System.Runtime.InteropServices.DllImportSearchPath? searchPath)
                        where {{NativeImportAttributeLibraryTypeParameterName}} : notnull, {{NativeImportLibraryTypeName}}
                        => {{NativeImportAttributeLibraryTypeParameterName}}.{{NativeImportLibraryAfterSuccessfullyLoadedMethodName}}(libraryName, searchPath);
                }

                /// <summary>Defines a condition for a <see cref="{{NativeImportSymbolAttributeTypeName}}{{{NativeImportAttributeLibraryTypeParameterName}}, {{NativeImportAttributeConditionTypeParameterName}}}">conditionally imported native symbol</see> or <see cref="{{NativeImportSymbolAttributeTypeName}}{{{NativeImportAttributeLibraryTypeParameterName}}, {{NativeImportAttributeConditionTypeParameterName}}}">function</see></summary>
                internal interface {{NativeImportConditionTypeName}}
                {
                    /// <summary>Gets a value indicating of conditionally imported native symbol of function should be imported</summary>
                    /// <returns><see langword="true"/> of the conditionally imported native symbol or function should be imported; otherwise, <see langword="false"/></returns>
                    internal static abstract bool {{NativeImportConditionEvaluateMethodName}}();

                    /// <remarks>For internal use only</remarks>
                    [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
                    private static class _Cached<{{NativeImportAttributeConditionTypeParameterName}}>
                        where {{NativeImportAttributeConditionTypeParameterName}} : notnull, {{NativeImportConditionTypeName}}
                    {
                        private static bool? _Result;

                        internal static bool Result
                        {
                            [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining | global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization)]
                            get => _Result ??= {{NativeImportAttributeConditionTypeParameterName}}.{{NativeImportConditionEvaluateMethodName}}();
                        }
                    }

                    /// <remarks>For internal use only</remarks>
                    [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining | global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization)]
                    [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
                    internal static bool {{NativeImportConditionEvaluateMethodName}}<{{NativeImportAttributeConditionTypeParameterName}}>()
                        where {{NativeImportAttributeConditionTypeParameterName}} : notnull, {{NativeImportConditionTypeName}}
                        => _Cached<{{NativeImportAttributeConditionTypeParameterName}}>.Result;
                }

                /// <summary>Indicates that the attributed target method serves as an accessor to an imported native symbol</summary>
                /// <param name="{{symbolNameParameterName}}"><inheritdoc cref="{{NativeImportAttributeSymbolNamePropertyName}}" path="/value"/></param>
                /// <typeparam name="{{NativeImportAttributeLibraryTypeParameterName}}">The type that represent the native library that should get imported</typeparam>
                /// <seealso cref="{{NativeImportSymbolKindTypeName}}"/>
                [global::System.AttributeUsage(global::System.AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
                internal sealed class {{NativeImportSymbolAttributeTypeName}}<{{NativeImportAttributeLibraryTypeParameterName}}>(string? {{symbolNameParameterName}} = default) : global::System.Attribute
                    where {{NativeImportAttributeLibraryTypeParameterName}} : notnull, {{NativeImportLibraryTypeName}}
                {
                    /// <summary>Gets the name of the symbol which should get imported</summary>
                    /// <value>The name of the symbol which should get imported. If omitted (<see langword="null"/>), the target method's name is used instead</value>
                    public string? {{NativeImportAttributeSymbolNamePropertyName}} => {{symbolNameParameterName}};

                    /// <summary>Gets or initializes the kind of access to the imported native symbol</summary>
                    /// <value>The kind of access to the imported native symbol</value>
                    public {{NativeImportSymbolKindTypeName}} {{NativeImportSymbolAttributeKindPropertyName}} { get; init; } = {{NativeImportSymbolKindTypeName}}.{{NativeImportSymbolKindAutoMemberName}};
                }

                /// <summary>Indicates that the attributed target method serves as an accessor to a conditionally imported native symbol</summary>
                /// <param name="{{symbolNameParameterName}}"><inheritdoc cref="{{NativeImportAttributeSymbolNamePropertyName}}" path="/value"/></param>
                /// <typeparam name="{{NativeImportAttributeLibraryTypeParameterName}}">The type that represent the native library that should get imported</typeparam>
                /// <typeparam name="{{NativeImportAttributeConditionTypeParameterName}}">The type that represent the condition that determines if a native symbol should get imported or not</typeparam>
                /// <remarks>
                /// NOTE: when a native symbol is not imported (the given condition <see cref="{{NativeImportConditionTypeName}}.{{NativeImportConditionEvaluateMethodName}}">evaluates</see> to <see langword="false"/>),
                /// calling the attributed target method results in at least undefined and erroneous behavior.
                /// No further indicating is given if a native symbol is not imported.
                /// </remarks>
                /// <seealso cref="{{NativeImportSymbolKindTypeName}}"/>
                [global::System.AttributeUsage(global::System.AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
                internal sealed class {{NativeImportSymbolAttributeTypeName}}<{{NativeImportAttributeLibraryTypeParameterName}}, {{NativeImportAttributeConditionTypeParameterName}}>(string? {{symbolNameParameterName}} = default) : global::System.Attribute
                    where {{NativeImportAttributeLibraryTypeParameterName}} : notnull, {{NativeImportLibraryTypeName}}
                    where {{NativeImportAttributeConditionTypeParameterName}} : notnull, {{NativeImportConditionTypeName}}
                {
                    /// <summary>Gets the name of the symbol which should get imported</summary>
                    /// <value>The name of the symbol which should get imported. If omitted (<see langword="null"/>), the target method's name is used instead</value>
                    public string? {{NativeImportAttributeSymbolNamePropertyName}} => {{symbolNameParameterName}};

                    /// <summary>Gets or initializes the kind of access to the imported native symbol</summary>
                    /// <value>The kind of access to the imported native symbol</value>
                    public {{NativeImportSymbolKindTypeName}} {{NativeImportSymbolAttributeKindPropertyName}} { get; init; } = {{NativeImportSymbolKindTypeName}}.{{NativeImportSymbolKindAutoMemberName}};
                }

                /// <summary>Indicates that the attributed target method serves as an entry point to an imported native function</summary>
                /// <param name="{{symbolNameParameterName}}"><inheritdoc cref="{{NativeImportAttributeSymbolNamePropertyName}}" path="/value"/></param>
                /// <typeparam name="{{NativeImportAttributeLibraryTypeParameterName}}">The type that represent the native library that should get imported</typeparam>
                [global::System.AttributeUsage(global::System.AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
                internal sealed class {{NativeImportFunctionAttributeTypeName}}<{{NativeImportAttributeLibraryTypeParameterName}}>(string? {{symbolNameParameterName}} = default) : global::System.Attribute
                    where {{NativeImportAttributeLibraryTypeParameterName}} : notnull, {{NativeImportLibraryTypeName}}
                {
                    /// <summary>Gets the name of the symbol which should get imported</summary>
                    /// <value>The name of the symbol which should get imported. If omitted (<see langword="null"/>), the target method's name is used instead</value>
                    public string? {{NativeImportAttributeSymbolNamePropertyName}} => {{symbolNameParameterName}};

                    /// <summary>Gets or initializes a collection of types indicating calling conventions for the imported native function</summary>
                    /// <value>
                    /// A collection of types indicating calling conventions for the imported native function.
                    /// If omitted or empty, the <see href="https://learn.microsoft.com/en-us/dotnet/standard/native-interop/calling-conventions#platform-default-calling-convention">the default platform calling convention</see> is used instead.
                    /// </value>
                    public global::System.Type[]? {{NativeImportFunctionAttributeCallConvsPropertyName}} { get; init; } = null;
                }

                /// <summary>Indicates that the attributed target method serves as an entry point to an conditionally imported native function</summary>
                /// <param name="{{symbolNameParameterName}}"><inheritdoc cref="{{NativeImportAttributeSymbolNamePropertyName}}" path="/value"/></param>
                /// <typeparam name="{{NativeImportAttributeLibraryTypeParameterName}}">The type that represent the native library that should get imported</typeparam>
                /// <typeparam name="{{NativeImportAttributeConditionTypeParameterName}}">The type that represent the condition that determines if a native symbol should get imported or not</typeparam>
                /// <remarks>
                /// NOTE: when a native symbol is not imported (the given condition <see cref="{{NativeImportConditionTypeName}}.{{NativeImportConditionEvaluateMethodName}}">evaluates</see> to <see langword="false"/>),
                /// calling the attributed target method results in at least undefined and erroneous behavior.
                /// No further indicating is given if a native symbol is not imported.
                /// </remarks>
                [global::System.AttributeUsage(global::System.AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
                internal sealed class {{NativeImportFunctionAttributeTypeName}}<{{NativeImportAttributeLibraryTypeParameterName}}, {{NativeImportAttributeConditionTypeParameterName}}>(string? {{symbolNameParameterName}} = default) : global::System.Attribute
                    where {{NativeImportAttributeLibraryTypeParameterName}} : notnull, {{NativeImportLibraryTypeName}}
                    where {{NativeImportAttributeConditionTypeParameterName}} : notnull, {{NativeImportConditionTypeName}}
                {
                    /// <summary>Gets the name of the symbol which should get imported</summary>
                    /// <value>The name of the symbol which should get imported. If omitted (<see langword="null"/>), the target method's name is used instead</value>
                    public string? {{NativeImportAttributeSymbolNamePropertyName}} => {{symbolNameParameterName}};

                    /// <summary>Gets or initializes a collection of types indicating calling conventions for the imported native function</summary>
                    /// <value>
                    /// A collection of types indicating calling conventions for the imported native function.
                    /// If omitted or empty, the <see href="https://learn.microsoft.com/en-us/dotnet/standard/native-interop/calling-conventions#platform-default-calling-convention">the default platform calling convention</see> is used instead.
                    /// </value>
                    public global::System.Type[]? {{NativeImportFunctionAttributeCallConvsPropertyName}} { get; init; } = null;
                }

                #nullable restore
                """,
            encoding: Encoding.UTF8
        ));
    }
}
