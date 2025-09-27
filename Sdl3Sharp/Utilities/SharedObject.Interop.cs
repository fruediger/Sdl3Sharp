using Sdl3Sharp.Internal.Interop;
using Sdl3Sharp.SourceGeneration;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sdl3Sharp.Utilities;

partial class SharedObject
{
	// Used for opaque pointers
	[StructLayout(LayoutKind.Sequential, Size = 0)]
	internal readonly struct SDL_SharedObject;

	/// <summary>
	/// Dynamically load a shared object
	/// </summary>
	/// <param name="sofile">A system-dependent name of the object file</param>
	/// <returns>Returns an opaque pointer to the object handle or NULL on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_LoadObject">SDL_LoadObject</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial SDL_SharedObject* SDL_LoadObject(byte* sofile);

	/// <summary>
	/// Look up the address of the named function in a shared object
	/// </summary>
	/// <param name="handle">A valid shared object handle returned by <see href="https://wiki.libsdl.org/SDL3/SDL_LoadObject">SDL_LoadObject</see>()</param>
	/// <param name="name">The name of the function to look up</param>
	/// <returns>Returns a pointer to the function or NULL on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information.</returns>
	/// <remarks>
	/// <para>
	/// This function pointer is no longer valid after calling <see href="https://wiki.libsdl.org/SDL3/SDL_UnloadObject">SDL_UnloadObject</see>().
	/// </para>
	/// <para>
	/// This function can only look up C function names. Other languages may have name mangling and intrinsic language support that varies from compiler to compiler.
	/// </para>
	/// <para>
	/// Make sure you declare your function pointers with the same calling convention as the actual library function. Your code will crash mysteriously if you do not do this.
	/// </para>
	/// <para>
	/// If the requested function doesn't exist, NULL is returned.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_LoadFunction">SDL_LoadFunction</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial void* SDL_LoadFunction(SDL_SharedObject* handle, byte* name);

	/// <summary>
	/// Unload a shared object from memory
	/// </summary>
	/// <param name="handle">A valid shared object handle returned by <see href="https://wiki.libsdl.org/SDL3/SDL_LoadObject">SDL_LoadObject</see>()</param>
	/// <remarks>
	/// Note that any pointers from this object looked up through <see href="https://wiki.libsdl.org/SDL3/SDL_LoadFunction">SDL_LoadFunction</see>() will no longer be valid.
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_UnloadObject">SDL_UnloadObject</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial void SDL_UnloadObject(SDL_SharedObject* handle);
}
