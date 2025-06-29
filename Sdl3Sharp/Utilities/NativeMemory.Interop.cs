using Sdl3Sharp.Interop;
using Sdl3Sharp.SourceGeneration;
using System;
using System.Runtime.CompilerServices;
using unsafe CallocFunc = delegate* unmanaged[Cdecl, SuppressGCTransition]<System.UIntPtr, System.UIntPtr, void*>;
using unsafe FreeFunc = delegate* unmanaged[Cdecl, SuppressGCTransition]<void*, void>;
using unsafe MallocFunc = delegate* unmanaged[Cdecl, SuppressGCTransition]<System.UIntPtr, void*>;
using unsafe ReallocFunc = delegate* unmanaged[Cdecl, SuppressGCTransition]<void*, System.UIntPtr, void*>;

namespace Sdl3Sharp.Utilities;

partial class NativeMemory
{
	/// <summary>
	/// Get the current set of SDL memory functions
	/// </summary>
	/// <param name="malloc_func">filled with malloc function</param>
	/// <param name="calloc_func">filled with calloc function</param>
	/// <param name="realloc_func">filled with realloc function</param>
	/// <param name="free_func">filled with free function</param>
	/// <remarks>
	/// This does not hold a lock, so do not call this in the unlikely event of a background thread calling <see href="https://wiki.libsdl.org/SDL3/SDL_SetMemoryFunctions">SDL_SetMemoryFunctions</see> simultaneously.
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetMemoryFunctions">SDL_GetMemoryFunctions</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial void SDL_GetMemoryFunctions(MallocFunc* malloc_func, CallocFunc* calloc_func, ReallocFunc* realloc_func, FreeFunc* free_func);

	/// <summary>
	/// Get the number of outstanding (unfreed) allocations
	/// </summary>
	/// <returns>Returns the number of allocations or -1 if allocation counting is disabled</returns>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetNumAllocations">SDL_GetNumAllocations</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal static partial int SDL_GetNumAllocations();

	/// <summary>
	/// Get the original set of SDL memory functions
	/// </summary>
	/// <param name="malloc_func">filled with malloc function</param>
	/// <param name="calloc_func">filled with calloc function</param>
	/// <param name="realloc_func">filled with realloc function</param>
	/// <param name="free_func">filled with free function</param>
	/// <remarks>
	/// This is what <see href="https://wiki.libsdl.org/SDL3/SDL_malloc">SDL_malloc</see> and friends will use by default, if there has been no call to <see href="https://wiki.libsdl.org/SDL3/SDL_SetMemoryFunctions">SDL_SetMemoryFunctions</see>.
	/// This is not necessarily using the C runtime's <c>malloc</c> functions behind the scenes! Different platforms and build configurations might do any number of unexpected things.
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetOriginalMemoryFunctions">SDL_GetOriginalMemoryFunctions</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial void SDL_GetOriginalMemoryFunctions(MallocFunc* malloc_func, CallocFunc* calloc_func, ReallocFunc* realloc_func, FreeFunc* free_func);

	/// <summary>
	/// Replace SDL's memory allocation functions with a custom set
	/// </summary>
	/// <param name="malloc_func">custom malloc function</param>
	/// <param name="calloc_func">custom calloc function</param>
	/// <param name="realloc_func">custom realloc function</param>
	/// <param name="free_func">custom free function</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// It is not safe to call this function once any allocations have been made, as future calls to <see href="https://wiki.libsdl.org/SDL3/SDL_free">SDL_free</see> will use the new allocator, even if they came from an <see href="https://wiki.libsdl.org/SDL3/SDL_malloc">SDL_malloc</see> made with the old one!
	///
	/// If used, usually this needs to be the first call made into the SDL library, if not the very first thing done at program startup time.
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_SetMemoryFunctions">SDL_SetMemoryFunctions</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_SetMemoryFunctions(MallocFunc malloc_func, CallocFunc calloc_func, ReallocFunc realloc_func, FreeFunc free_func);

	/// <summary>
	/// Allocate memory aligned to a specific alignment
	/// </summary>
	/// <param name="alignment">the alignment of the memory</param>
	/// <param name="size">the size to allocate</param>
	/// <returns>Returns a pointer to the aligned memory, or NULL if allocation failed</returns>
	/// <remarks>
	/// The memory returned by this function must be freed with <see href="https://wiki.libsdl.org/SDL3/SDL_aligned_free">SDL_aligned_free</see>(), <em>not</em> <see href="https://wiki.libsdl.org/SDL3/SDL_free">SDL_free</see>().
	///
	/// If <c><paramref name="alignment"/></c> is less than the size of <c>void *</c>, it will be increased to match that.
	///
	/// The returned memory address will be a multiple of the alignment value, and the size of the memory allocated will be a multiple of the alignment value.
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_aligned_alloc">SDL_aligned_alloc</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl), typeof(CallConvSuppressGCTransition)])]
	internal unsafe static partial void* SDL_aligned_alloc(UIntPtr alignment, UIntPtr size);

	/// <summary>
	/// Free memory allocated by <see href="https://wiki.libsdl.org/SDL3/SDL_aligned_alloc">SDL_aligned_alloc()</see>
	/// </summary>
	/// <param name="mem">a pointer previously returned by <see href="https://wiki.libsdl.org/SDL3/SDL_aligned_alloc">SDL_aligned_alloc</see>(), or NULL</param>
	/// <remarks>
	/// The pointer is no longer valid after this call and cannot be dereferenced anymore.
	///
	/// If <c><paramref name="mem"/></c> is NULL, this function does nothing.
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_aligned_free">SDL_aligned_free</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl), typeof(CallConvSuppressGCTransition)])]
	internal unsafe static partial void SDL_aligned_free(void* mem);

	/// <summary>
	/// Allocate a zero-initialized array
	/// </summary>
	/// <param name="nmemb">the number of elements in the array</param>
	/// <param name="size">the size of each element of the array</param>
	/// <returns>Returns a pointer to the allocated array, or NULL if allocation failed</returns>
	/// <remarks>
	/// The memory returned by this function must be freed with <see href="https://wiki.libsdl.org/SDL3/SDL_free">SDL_free</see>().
	///
	/// If either of <c><paramref name="nmemb"/></c> or <c><paramref name="size"/></c> is 0, they will both be set to 1.
	///
	/// If the allocation is successful, the returned pointer is guaranteed to be aligned to either the <em>fundamental alignment</em> (<c>alignof(max_align_t)</c> in C11 and later) or <c>2 * sizeof(void *)</c>, whichever is smaller.
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_calloc">SDL_calloc</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl), typeof(CallConvSuppressGCTransition)])]
	internal unsafe static partial void* SDL_calloc(UIntPtr nmemb, UIntPtr size);

	/// <summary>
	/// Free allocated memory
	/// </summary>
	/// <param name="mem">a pointer to allocated memory, or NULL</param>
	/// <remarks>
	/// The pointer is no longer valid after this call and cannot be dereferenced anymore.
	/// 
	/// If <c><paramref name="mem"/></c> is NULL, this function does nothing.
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_free">SDL_free</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl), typeof(CallConvSuppressGCTransition)])]
	internal unsafe static partial void SDL_free(void* mem);

	/// <summary>
	/// Allocate uninitialized memory
	/// </summary>
	/// <param name="size">the size to allocate</param>
	/// <returns>Returns a pointer to the allocated memory, or NULL if allocation failed</returns>
	/// <remarks>
	/// The allocated memory returned by this function must be freed with <see href="https://wiki.libsdl.org/SDL3/SDL_free">SDL_free</see>().
	///
	/// If <c><paramref name="size"/></c> is 0, it will be set to 1.
	///
	/// If the allocation is successful, the returned pointer is guaranteed to be aligned to either the <em>fundamental alignment</em> (<c>alignof(max_align_t)</c> in C11 and later) or <c>2 * sizeof(void *)</c>, whichever is smaller.
	/// Use <see href="https://wiki.libsdl.org/SDL3/SDL_aligned_alloc">SDL_aligned_alloc</see>() if you need to allocate memory aligned to an alignment greater than this guarantee.
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_malloc">SDL_malloc</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl), typeof(CallConvSuppressGCTransition)])]
	internal unsafe static partial void* SDL_malloc(UIntPtr size);

	/// <summary>
	/// Change the size of allocated memor
	/// </summary>
	/// <param name="mem">a pointer to allocated memory to reallocate, or NULL</param>
	/// <param name="size">the new size of the memory</param>
	/// <returns>Returns a pointer to the newly allocated memory, or NULL if allocation failed</returns>
	/// <remarks>
	/// The memory returned by this function must be freed with <see href="https://wiki.libsdl.org/SDL3/SDL_free">SDL_free</see>().
	///
	/// If <c><paramref name="size"/></c> is 0, it will be set to 1.
	/// Note that this is unlike some other C runtime realloc implementations, which may treat <c>realloc(<paramref name="mem"/>, 0)</c> the same way as <c>free(<paramref name="mem"/>)</c>.
	///
	/// If <c><paramref name="mem"/></c> is NULL, the behavior of this function is equivalent to <see href="https://wiki.libsdl.org/SDL3/SDL_malloc">SDL_malloc</see>(). Otherwise, the function can have one of three possible outcomes:
	/// <list type="bullet">
	///		<item>
	///			<description>If it returns the same pointer as <c><paramref name="mem"/></c>, it means that <c><paramref name="mem"/></c> was resized in place without freeing.</description>
	///		</item>
	///		<item>
	///			<description>If it returns a different non-NULL pointer, it means that <c><paramref name="mem"/></c> was freed and cannot be dereferenced anymore.</description>
	///		</item>
	///		<item>
	///			<description>If it returns NULL (indicating failure), then <c><paramref name="mem"/></c> will remain valid and must still be freed with <see href="https://wiki.libsdl.org/SDL3/SDL_free">SDL_free</see>().</description>
	///		</item>
	/// </list>
	///
	/// If the allocation is successfully resized, the returned pointer is guaranteed to be aligned to either the <em>fundamental alignment</em> (<c>alignof(max_align_t)</c> in C11 and later) or <c>2 * sizeof(void *)</c>, whichever is smaller.
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_realloc">SDL_realloc</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl), typeof(CallConvSuppressGCTransition)])]
	internal unsafe static partial void* SDL_realloc(void* mem, UIntPtr size);
}
