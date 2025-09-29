using Sdl3Sharp.Internal.Interop;
using Sdl3Sharp.SourceGeneration;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Threading;

partial struct AtomicInt32
{
	/// <summary>
	/// Add to an atomic variable
	/// </summary>
	/// <param name="a">a pointer to an <see href="https://wiki.libsdl.org/SDL3/SDL_AtomicInt">SDL_AtomicInt</see> variable to be modified</param>
	/// <param name="v">the desired value to add</param>
	/// <returns>Returns the previous value of the atomic variable</returns>
	/// <remarks>
	/// This function also acts as a full memory barrier.
	///
	/// <em>Note: If you don't know what this function is for, you shouldn't use it!</em>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_AddAtomicInt">SDL_AddAtomicInt</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl), typeof(CallConvSuppressGCTransition)])]
	internal unsafe static partial int SDL_AddAtomicInt(AtomicInt32* a, int v);

	/// <summary>
	/// Set an atomic variable to a new value if it is currently an old value
	/// </summary>
	/// <param name="a">a pointer to an <see href="https://wiki.libsdl.org/SDL3/SDL_AtomicInt">SDL_AtomicInt</see> variable to be modified</param>
	/// <param name="oldval">the old value</param>
	/// <param name="newval">the new value</param>
	/// <returns>Returns true if the atomic variable was set, false otherwise</returns>
	/// <remarks>
	/// <em>Note: If you don't know what this function is for, you shouldn't use it!</em>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_CompareAndSwapAtomicInt">SDL_CompareAndSwapAtomicInt</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl), typeof(CallConvSuppressGCTransition)])]
	internal unsafe static partial CBool SDL_CompareAndSwapAtomicInt(AtomicInt32* a, int oldval, int newval);

	/// <summary>
	/// Get the value of an atomic variable
	/// </summary>
	/// <param name="a">a pointer to an <see href="https://wiki.libsdl.org/SDL3/SDL_AtomicInt">SDL_AtomicInt</see> variable</param>
	/// <returns>Returns the current value of an atomic variable</returns>
	/// <remarks>
	/// <em>Note: If you don't know what this function is for, you shouldn't use it!</em>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetAtomicInt">SDL_GetAtomicInt</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl), typeof(CallConvSuppressGCTransition)])]
	internal unsafe static partial int SDL_GetAtomicInt(AtomicInt32* a);

	/// <summary>
	/// Set an atomic variable to a value
	/// </summary>
	/// <param name="a">a pointer to an <see href="https://wiki.libsdl.org/SDL3/SDL_AtomicInt">SDL_AtomicInt</see> variable to be modified</param>
	/// <param name="v">the desired value</param>
	/// <returns>Returns the previous value of the atomic variable</returns>
	/// <remarks>
	/// This function also acts as a full memory barrier.
	///
	/// <em>Note: If you don't know what this function is for, you shouldn't use it!</em>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_SetAtomicInt">SDL_SetAtomicInt</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial int SDL_SetAtomicInt(AtomicInt32* a, int v);
}
