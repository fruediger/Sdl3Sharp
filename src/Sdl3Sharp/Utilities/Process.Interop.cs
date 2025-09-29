using Sdl3Sharp.Internal.Interop;
using Sdl3Sharp.SourceGeneration;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

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
}
