using Sdl3Sharp.Internal.Interop;
using Sdl3Sharp.SourceGeneration;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using static Sdl3Sharp.Hint.Joystick.HidApi;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Sdl3Sharp.Utilities;

partial class Process
{
    // Used for opaque pointers
    [StructLayout(LayoutKind.Sequential, Size = 0)]
    internal readonly struct SDL_Process;

    /// <summary>
    /// Create a new process
    /// </summary>
    /// <param name="args">The path and arguments for the new process</param>
    /// <param name="pipe_stdio">true to create pipes to the process's standard input and from the process's standard output, false for the process to have no input and inherit the application's standard output</param>
    /// <returns>Returns the newly created and running process, or NULL if the process couldn't be created</returns>
    /// <remarks>
    /// <para>
    /// The path to the executable is supplied in args[0]. args[1..N] are additional arguments passed on the command line of the new process,
    /// and the argument list should be terminated with a NULL, e.g.:
    /// <code>
    /// const char *args[] = { "myprogram", "argument", NULL }; 
    /// </code>
    /// </para>
    /// <para>
    /// Setting pipe_stdio to true is equivalent to setting <see href="https://wiki.libsdl.org/SDL3/SDL_PROP_PROCESS_CREATE_STDIN_NUMBER">SDL_PROP_PROCESS_CREATE_STDIN_NUMBER</see> and <see href="https://wiki.libsdl.org/SDL3/SDL_PROP_PROCESS_CREATE_STDOUT_NUMBER">SDL_PROP_PROCESS_CREATE_STDOUT_NUMBER</see> to <see href="https://wiki.libsdl.org/SDL3/SDL_PROCESS_STDIO_APP">SDL_PROCESS_STDIO_APP</see>,
    /// and will allow the use of <see href="https://wiki.libsdl.org/SDL3/SDL_ReadProcess">SDL_ReadProcess</see>() or <see href="https://wiki.libsdl.org/SDL3/SDL_GetProcessInput">SDL_GetProcessInput</see>() and <see href="https://wiki.libsdl.org/SDL3/SDL_GetProcessOutput">SDL_GetProcessOutput</see>().
    /// </para>
    /// <para>
    /// See <see href="https://wiki.libsdl.org/SDL3/SDL_CreateProcessWithProperties">SDL_CreateProcessWithProperties</see>() for more details.
    /// </para>
    /// </remarks>
    [NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
    internal unsafe static partial SDL_Process* SDL_CreateProcess(byte** args, CBool pipe_stdio);

    /// <summary>
    /// Create a new process with the specified properties
    /// </summary>
    /// <param name="props">The properties to use</param>
    /// <returns>Returns the newly created and running process, or NULL if the process couldn't be created</returns>
    /// <remarks>
    /// <para>
    /// These are the supported properties:
    /// <list type="bullet">
    ///		<item>
    ///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_PROCESS_CREATE_ARGS_POINTER"><c>SDL_PROP_PROCESS_CREATE_ARGS_POINTER</c></see></term>
    ///			<description>
    ///				An array of strings containing the program to run, any arguments, and a NULL pointer, e.g. const char *args[] = { "myprogram", "argument", NULL }. This is a required property.
    ///			</description>
    ///		</item>
    ///		<item>
    ///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_PROCESS_CREATE_ENVIRONMENT_POINTER"><c>SDL_PROP_PROCESS_CREATE_ARGS_POINTER</c></see></term>
    ///			<description>
    ///				An <see href="https://wiki.libsdl.org/SDL3/SDL_Environment">SDL_Environment</see> pointer. If this property is set, it will be the entire environment for the process, otherwise the current environment is used.
    ///			</description>
    ///		</item>
    ///		<item>
    ///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_PROCESS_CREATE_WORKING_DIRECTORY_STRING"><c>SDL_PROP_PROCESS_CREATE_WORKING_DIRECTORY_STRING</c></see></term>
    ///			<description>
    ///				A UTF-8 encoded string representing the working directory for the process, defaults to the current working directory.
    ///			</description>
    ///		</item>
    ///		<item>
    ///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_PROCESS_CREATE_STDIN_NUMBER"><c>SDL_PROP_PROCESS_CREATE_STDIN_NUMBER</c></see></term>
    ///			<description>
    ///				An <see href="https://wiki.libsdl.org/SDL3/SDL_ProcessIO">SDL_ProcessIO</see> value describing where standard input for the process comes from,
    ///				defaults to <see href="https://wiki.libsdl.org/SDL3/SDL_PROCESS_STDIO_NULL"><c>SDL_PROCESS_STDIO_NULL</c></see>.
    ///			</description>
    ///		</item>
    ///		<item>
    ///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_PROCESS_CREATE_STDIN_POINTER"><c>SDL_PROP_PROCESS_CREATE_STDIN_POINTER</c></see></term>
    ///			<description>
    ///				An <see href="https://wiki.libsdl.org/SDL3/SDL_IOStream">SDL_IOStream</see> pointer used for standard input when <see href="https://wiki.libsdl.org/SDL3/SDL_PROP_PROCESS_CREATE_STDIN_NUMBER"><c>SDL_PROP_PROCESS_CREATE_STDIN_NUMBER</c></see>
    ///				is set to <see href="https://wiki.libsdl.org/SDL3/SDL_PROCESS_STDIO_REDIRECT"><c>SDL_PROCESS_STDIO_REDIRECT</c></see>.
    ///			</description>
    ///		</item>
    ///		<item>
    ///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_PROCESS_CREATE_STDOUT_NUMBER"><c>SDL_PROP_PROCESS_CREATE_STDOUT_NUMBER</c></see></term>
    ///			<description>
    ///				An <see href="https://wiki.libsdl.org/SDL3/SDL_ProcessIO">SDL_ProcessIO</see> value describing where standard output for the process goes to,
    ///				defaults to <see href="https://wiki.libsdl.org/SDL3/SDL_PROCESS_STDIO_INHERITED"><c>SDL_PROCESS_STDIO_INHERITED</c></see>.
    ///			</description>
    ///		</item>
    /// </list>
    // TODO:
    //SDL_PROP_PROCESS_CREATE_STDOUT_NUMBER: an SDL_ProcessIO value describing where standard output for the process goes to, defaults to SDL_PROCESS_STDIO_INHERITED.
    //SDL_PROP_PROCESS_CREATE_STDOUT_POINTER: an SDL_IOStream pointer used for standard output when SDL_PROP_PROCESS_CREATE_STDOUT_NUMBER is set to SDL_PROCESS_STDIO_REDIRECT.
    //SDL_PROP_PROCESS_CREATE_STDERR_NUMBER: an SDL_ProcessIO value describing where standard error for the process goes to, defaults to SDL_PROCESS_STDIO_INHERITED.
    //SDL_PROP_PROCESS_CREATE_STDERR_POINTER: an SDL_IOStream pointer used for standard error when SDL_PROP_PROCESS_CREATE_STDERR_NUMBER is set to SDL_PROCESS_STDIO_REDIRECT.
    //SDL_PROP_PROCESS_CREATE_STDERR_TO_STDOUT_BOOLEAN: true if the error output of the process should be redirected into the standard output of the process. This property has no effect if SDL_PROP_PROCESS_CREATE_STDERR_NUMBER is set.
    //SDL_PROP_PROCESS_CREATE_BACKGROUND_BOOLEAN: true if the process should run in the background. In this case the default input and output is SDL_PROCESS_STDIO_NULL and the exitcode of the process is not available, and will always be 0.
    //SDL_PROP_PROCESS_CREATE_CMDLINE_STRING: a string containing the program to run and any parameters. This string is passed directly to CreateProcess on Windows, and does nothing on other platforms. This property is only important if you want to start programs that does non-standard command-line processing, and in most cases using SDL_PROP_PROCESS_CREATE_ARGS_POINTER is sufficient.

    /// </para>
    /// </remarks>
    [NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
    internal unsafe static partial SDL_Process* SDL_CreateProcessWithProperties(uint props);
}
