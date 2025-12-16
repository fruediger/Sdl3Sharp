using Sdl3Sharp.Internal.Interop;
using Sdl3Sharp.SourceGeneration;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp;

partial class Error
{
	/// <summary>
	/// Clear any previous error message for this thread
	/// </summary>
	/// <returns>Returns true</returns>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_ClearError">SDL_ClearError</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal static partial CBool SDL_ClearError();

	/// <summary>
	/// Retrieve a message about the last error that occurred on the current thread
	/// </summary>
	/// <returns>Returns a message with information about the specific error that occurred, or an empty string if there hasn't been an error message set since the last call to <see href="https://wiki.libsdl.org/SDL3/SDL_ClearError">SDL_ClearError</see>()</returns>
	/// <remarks>
	/// <para>
	/// It is possible for multiple errors to occur before calling <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>(). Only the last error is returned.
	/// </para>
	/// <para>
	/// The message is only applicable when an SDL function has signaled an error. You must check the return values of SDL function calls to determine when to appropriately call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>().
	/// You should not use the results of <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() to decide if an error has occurred! Sometimes SDL will set an error string even when reporting success.
	/// </para>
	/// <para>
	/// SDL will <em>not</em> clear the error string for successful API calls. You <em>must</em> check return values for failure cases before you can assume the error string applies.
	/// </para>
	/// <para>
	/// Error strings are set per-thread, so an error set in a different thread will not interfere with the current thread's operation.
	/// </para>
	/// <para>
	/// The returned value is a thread-local string which will remain valid until the current thread's error string is changed. The caller should make a copy if the value is needed after the next SDL API call.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial byte* SDL_GetError();

	/// <summary>
	/// Set an error indicating that memory allocation failed
	/// </summary>
	/// <returns>Returns false</returns>
	/// <remarks>
	/// <para>
	/// This function does not do any memory allocation
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_OutOfMemory">SDL_OutOfMemory</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal static partial CBool SDL_OutOfMemory();

	/// <summary>
	/// Set the SDL error message for the current thread
	/// </summary>
	/// <param name="fmt">a printf()-style message format string</param>
	/// <returns>Returns false</returns>
	/// <remarks>
	/// <para>
	/// Calling this function will replace any previous error message that was set.
	/// </para>
	/// <para>
	/// This function always returns false, since SDL frequently uses false to signify a failing result, leading to this idiom: 
	/// <code>
	/// if (error_code) {
	///		return SDL_SetError("This operation has failed: %d", error_code);
	/// }
	/// </code>
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_SetError">SDL_SetError</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_SetError(byte* fmt);

	/// <seealso cref="SDL_SetError(byte*)"/>
	[NativeImportSymbol<Library>("SDL_SetError", Kind = NativeImportSymbolKind.Reference)]
	internal static partial ref readonly byte SDL_SetError_var();
}
