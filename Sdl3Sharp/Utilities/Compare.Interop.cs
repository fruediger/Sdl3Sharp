using Sdl3Sharp.Internal.Interop;
using Sdl3Sharp.SourceGeneration;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using unsafe SDL_CompareCallback_r = delegate* unmanaged[Cdecl]<void*, void*, void*, int>;

namespace Sdl3Sharp.Utilities;

partial class Compare
{
	[UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
	private static unsafe int CompareCallback(void* userdata, void* a, void* b)
	{
		if (userdata is not null && GCHandle.FromIntPtr(unchecked((IntPtr)userdata)) is { IsAllocated: true, Target: ComparerWrapper comparerWrapper })
		{
			return comparerWrapper.Compare(a, b);
		}

		return default;
	}

	/// <summary>
	/// Perform a binary search on a previously sorted array, passing a userdata pointer to the compare function
	/// </summary>
	/// <param name="key">A pointer to a key equal to the element being searched for</param>
	/// <param name="base">A pointer to the start of the array</param>
	/// <param name="nmemb">The number of elements in the array</param>
	/// <param name="size">The size of the elements in the array</param>
	/// <param name="compare">A function used to compare elements in the array</param>
	/// <param name="userdata">A pointer to pass to the compare function</param>
	/// <returns>Returns a pointer to the matching element in the array, or NULL if not found</returns>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_bsearch_r">SDL_bsearch_r</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial void* SDL_bsearch_r(void* key, void* @base, nuint nmemb, nuint size, SDL_CompareCallback_r compare, void* userdata);

	/// <summary>
	/// Sort an array, passing a userdata pointer to the compare function
	/// </summary>
	/// <param name="base">A pointer to the start of the array</param>
	/// <param name="nmemb">The number of elements in the array</param>
	/// <param name="size">The size of the elements in the array</param>
	/// <param name="compare">A function used to compare elements in the array</param>
	/// <param name="userdata">A pointer to pass to the compare function</param>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_qsort_r">SDL_qsort_r</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial void SDL_qsort_r(void* @base, nuint nmemb, nuint size, SDL_CompareCallback_r compare, void* userdata);
}
