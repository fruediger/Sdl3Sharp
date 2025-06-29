namespace Sdl3Sharp.Utilities;

/// <summary>
/// Represents a set of native memory functions that SDL uses to allocate, free, and reallocate memory
/// </summary>
public interface INativeMemoryFunctions
{
	/// <summary>
	/// A callback used to implement <see cref="NativeMemory.Calloc(nuint, nuint)"/>
	/// </summary>
	/// <param name="elementCount">The number of elements in the array</param>
	/// <param name="elementSize">The size of each element of the array</param>
	/// <returns>A pointer to the allocated memory, or <c><see langword="null"/></c> if the allocation failed</returns>
	/// <remarks>
	/// <para>
	/// SDL will always ensure that the passed <paramref name="elementCount"/> and <paramref name="elementSize"/> are both greater than <c>0</c>.
	/// </para>
	/// </remarks>
	/// <seealso cref="NativeMemory.Calloc(nuint, nuint)"/>
	unsafe void* Calloc(nuint elementCount, nuint elementSize);

	/// <summary>
	/// A callback used to implement <see cref="NativeMemory.Free(void*)"/>
	/// </summary>
	/// <param name="memory">A pointer to allocated memory</param>
	/// <remarks>
	/// <para>
	/// SDL will always ensure that the passed <paramref name="memory"/> pointer is a non-<c><see langword="null"/></c> pointer.
	/// </para>
	/// </remarks>
	/// <seealso cref="NativeMemory.Free(void*)"/>
	unsafe void Free(void* memory);

	/// <summary>
	/// A callback used to implement <see cref="NativeMemory.Malloc(nuint)"/>
	/// </summary>
	/// <param name="size">The size to allocate</param>
	/// <returns>A pointer to the allocated memory, or <c><see langword="null"/></c> if the allocation failed</returns>
	/// <remarks>
	/// <para>
	/// SDL will always ensure that the passed <paramref name="size"/> is greater than <c>0</c>.
	/// </para>
	/// </remarks>
	/// <seealso cref="NativeMemory.Malloc(nuint)"/>
	unsafe void* Malloc(nuint size);

	/// <summary>
	/// A callback used to implement <see cref="NativeMemory.Realloc(void*, nuint)"/>
	/// </summary>
	/// <param name="memory">A pointer to allocated memory to reallocate, or <c><see langword="null"/></c></param>
	/// <param name="newSize">The new size of the memory</param>
	/// <returns>A pointer to the newly allocated memory, or <c><see langword="null"/></c> if the allocation failed</returns>
	/// <remarks>
	/// <para>
	/// SDL will always ensure that the passed <paramref name="newSize"/> is greater than <c>0</c>.
	/// </para>
	/// </remarks>
	/// <seealso cref="NativeMemory.Realloc(void*, nuint)"/>
	unsafe void* Realloc(void* memory, nuint newSize);
}
