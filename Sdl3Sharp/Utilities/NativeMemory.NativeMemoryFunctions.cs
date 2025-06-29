using System.Runtime.CompilerServices;
using unsafe CallocFunc = delegate* unmanaged[Cdecl, SuppressGCTransition]<System.UIntPtr, System.UIntPtr, void*>;
using unsafe FreeFunc = delegate* unmanaged[Cdecl, SuppressGCTransition]<void*, void>;
using unsafe MallocFunc = delegate* unmanaged[Cdecl, SuppressGCTransition]<System.UIntPtr, void*>;
using unsafe ReallocFunc = delegate* unmanaged[Cdecl, SuppressGCTransition]<void*, System.UIntPtr, void*>;

namespace Sdl3Sharp.Utilities;

partial class NativeMemory
{
	private unsafe sealed class NativeMemoryFunctions(MallocFunc malloc, CallocFunc calloc, ReallocFunc realloc, FreeFunc free) : INativeMemoryFunctions
	{
		public readonly MallocFunc Malloc = malloc;
		public readonly CallocFunc Calloc = calloc;
		public readonly ReallocFunc Realloc = realloc;
		public readonly FreeFunc Free = free;

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
