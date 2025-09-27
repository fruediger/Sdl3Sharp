using System.Runtime.CompilerServices;
using unsafe SDL_calloc_func = delegate* unmanaged[Cdecl, SuppressGCTransition]<System.UIntPtr, System.UIntPtr, void*>;
using unsafe SDL_free_func = delegate* unmanaged[Cdecl, SuppressGCTransition]<void*, void>;
using unsafe SDL_malloc_func = delegate* unmanaged[Cdecl, SuppressGCTransition]<System.UIntPtr, void*>;
using unsafe SDL_realloc_func = delegate* unmanaged[Cdecl, SuppressGCTransition]<void*, System.UIntPtr, void*>;

namespace Sdl3Sharp.Utilities;

partial struct NativeMemory
{
	private unsafe sealed class NativeMemoryFunctions(SDL_malloc_func malloc, SDL_calloc_func calloc, SDL_realloc_func realloc, SDL_free_func free) : INativeMemoryFunctions
	{
		public readonly SDL_malloc_func Malloc = malloc;
		public readonly SDL_calloc_func Calloc = calloc;
		public readonly SDL_realloc_func Realloc = realloc;
		public readonly SDL_free_func Free = free;

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		unsafe void* INativeMemoryFunctions.Calloc(nuint elementCount, nuint elementSize) => Calloc(elementCount, elementSize);

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		unsafe void INativeMemoryFunctions.Free(void* memory) => Free(memory);

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		unsafe void* INativeMemoryFunctions.Malloc(nuint size) => Malloc(size);

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		unsafe void* INativeMemoryFunctions.Realloc(void* memory, nuint newSize) => Realloc(memory, newSize);
	}
}
