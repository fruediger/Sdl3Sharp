using Sdl3Sharp.Internal.Interop;
using Sdl3Sharp.SourceGeneration;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Threading;

partial struct AtomicIntPtr
{
	/// <summary>
	/// Set a pointer to a new value if it is currently an old value
	/// </summary>
	/// <param name="a">a pointer to a pointer</param>
	/// <param name="oldval">the old pointer value</param>
	/// <param name="newval">the new pointer value</param>
	/// <returns>Returns true if the pointer was set, false otherwise</returns>
	/// <remarks>
	/// <em>Note: If you don't know what this function is for, you shouldn't use it!</em>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_CompareAndSwapAtomicPointer">SDL_CompareAndSwapAtomicPointer</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl), typeof(CallConvSuppressGCTransition)])]
	internal unsafe static partial CBool SDL_CompareAndSwapAtomicPointer(void** a, void* oldval, void* newval);

	/// <summary>
	/// Get the value of a pointer atomically
	/// </summary>
	/// <param name="a">a pointer to a pointer</param>
	/// <returns>Returns the current value of a pointer</returns>
	/// <remarks>
	/// <em>Note: If you don't know what this function is for, you shouldn't use it!</em>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetAtomicPointer"></seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl), typeof(CallConvSuppressGCTransition)])]
	internal unsafe static partial void* SDL_GetAtomicPointer(void** a);

	/// <summary>
	/// Set a pointer to a value atomically
	/// </summary>
	/// <param name="a">a pointer to a pointer</param>
	/// <param name="v">the desired pointer value</param>
	/// <returns>Returns the previous value of the pointer</returns>
	/// <remarks>
	/// <em>Note: If you don't know what this function is for, you shouldn't use it!</em>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_SetAtomicPointer">SDL_SetAtomicPointer</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl), typeof(CallConvSuppressGCTransition)])]
	internal unsafe static partial void* SDL_SetAtomicPointer(void** a, void* v);
}
