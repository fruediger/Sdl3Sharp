using Sdl3Sharp.Internal.Interop;
using Sdl3Sharp.SourceGeneration;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Threading;

partial struct AtomicUInt32
{
	/// <summary>
	/// Set an atomic variable to a new value if it is currently an old value
	/// </summary>
	/// <param name="a">a pointer to an <see href="https://wiki.libsdl.org/SDL3/SDL_AtomicU32">SDL_AtomicU32</see> variable to be modified</param>
	/// <param name="oldval">the old value</param>
	/// <param name="newval">the new value</param>
	/// <returns>Returns true if the atomic variable was set, false otherwise</returns>
	/// <remarks>
	/// <em>Note: If you don't know what this function is for, you shouldn't use it!</em>
	/// </remarks>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl), typeof(CallConvSuppressGCTransition)])]
	internal unsafe static partial CBool SDL_CompareAndSwapAtomicU32(AtomicUInt32* a, uint oldval, uint newval);

	/// <summary>
	/// Get the value of an atomic variable
	/// </summary>
	/// <param name="a">a pointer to an <see href="https://wiki.libsdl.org/SDL3/SDL_AtomicU32">SDL_AtomicU32</see> variable</param>
	/// <returns>Returns the current value of an atomic variable</returns>
	/// <remarks>
	/// <em>Note: If you don't know what this function is for, you shouldn't use it!</em>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetAtomicU32">SDL_GetAtomicU32</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl), typeof(CallConvSuppressGCTransition)])]
	internal unsafe static partial uint SDL_GetAtomicU32(AtomicUInt32* a);

	/// <summary>
	/// Set an atomic variable to a value
	/// </summary>
	/// <param name="a">a pointer to an <see href="https://wiki.libsdl.org/SDL3/SDL_AtomicU32">SDL_AtomicU32</see> variable to be modified</param>
	/// <param name="v">the desired value</param>
	/// <returns>Returns the previous value of the atomic variable</returns>
	/// <remarks>
	/// This function also acts as a full memory barrier.
	///
	/// <em>Note: If you don't know what this function is for, you shouldn't use it!</em>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_SetAtomicU32">SDL_SetAtomicU32</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl), typeof(CallConvSuppressGCTransition)])]
	internal unsafe static partial uint SDL_SetAtomicU32(AtomicUInt32* a, uint v);
}
