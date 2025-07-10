using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using unsafe CallocFunc = delegate* unmanaged[Cdecl, SuppressGCTransition]<System.UIntPtr, System.UIntPtr, void*>;
using unsafe FreeFunc = delegate* unmanaged[Cdecl, SuppressGCTransition]<void*, void>;
using unsafe MallocFunc = delegate* unmanaged[Cdecl, SuppressGCTransition]<System.UIntPtr, void*>;
using unsafe ReallocFunc = delegate* unmanaged[Cdecl, SuppressGCTransition]<void*, System.UIntPtr, void*>;

namespace Sdl3Sharp.Utilities;

/// <summary>
/// Provides methods for managing native memory allocations, including managing SDL's internal allocators.
/// </summary>
/// <remarks>
/// <para>
/// Note: Many methods in this class require the caller to manually free the allocated memory using the appropriate free method (e.g., <see cref="Free(void*)"/> or <see cref="AlignedFree(void*)"/>). Failure to do so may result in memory leaks. 
/// </para>
/// </remarks>
public static partial class NativeMemory
{
	/// <summary>
	/// Gets the number of outstanding (unfreed) allocations
	/// </summary>
	/// <value>
	/// The number of outstanding (unfreed) allocations, or <c>-1</c> if allocation counting is disabled
	/// </value>
	public static int Allocations => SDL_GetNumAllocations();

	/// <summary>
	/// Allocates uninitialized memory that is aligned to a specific alignment
	/// </summary>
	/// <param name="alignment">The alignment of the memory</param>
	/// <param name="size">The size to allocate</param>
	/// <returns>A pointer to the allocated aligned memory, or <c><see langword="null"/></c> if the allocation failed</returns>
	/// <remarks>
	/// <para>
	/// The memory returned by this method must be freed with <see cref="AlignedFree(void*)"/>, <em>not</em> <see cref="Free(void*)"/>.
	/// </para>
	/// <para>
	/// If the <paramref name="alignment"/> is less than <c><see langword="sizeof"/>(<see langword="void"/>*)</c>, it will be increased to match that instead.
	/// </para>
	/// <para>
	/// The returned memory address will be a multiple of the <paramref name="alignment"/>, and the actual size of the memory allocated will be a multiple of the <paramref name="alignment"/> too.
	/// </para>
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public unsafe static void* AlignedAlloc(nuint alignment, nuint size)
		=> SDL_aligned_alloc(alignment, size);

	/// <summary>
	/// Allocates uninitialized memory to hold a given number of elements of a given type and is aligned to a specific alignment
	/// </summary>
	/// <typeparam name="T">The type of the elements in the array</typeparam>
	/// <param name="alignment">The alignment of the memory</param>
	/// <param name="elementCount">The number of elements of type <typeparamref name="T"/> in the array</param>
	/// <returns>A pointer to the allocated aligned memory, or <c><see langword="null"/></c> if the allocation failed</returns>
	/// <remarks>
	/// <para>
	/// The memory returned by this method must be freed with <see cref="AlignedFree(void*)"/>, <em>not</em> <see cref="Free(void*)"/>.
	/// </para>
	/// <para>
	/// If the <paramref name="elementCount"/> is <c>0</c>, it will be set to <c>1</c> instead.
	/// </para>
	/// <para>
	/// If the size of a single <typeparamref name="T"/> instance happens to be <c>0</c>, memory with an overall size of <c>1</c> will be allocated.
	/// </para>
	/// <para>
	/// If the <paramref name="alignment"/> is less than <c><see langword="sizeof"/>(<see langword="void"/>*)</c>, it will be increased to match that instead.
	/// Note: that also means that the <paramref name="alignment"/> doesn't necessarily have to be of a multiple of the size of a single <typeparamref name="T"/> instance,
	/// yet it's still highly advised to use such an alignment.
	/// </para>
	/// <para>
	/// The returned memory address will be a multiple of the <paramref name="alignment"/>, and the actual size of the memory allocated will be a multiple of the <paramref name="alignment"/> too.
	/// </para>
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public unsafe static T* AlignedAlloc<T>(nuint alignment, nuint elementCount)
		where T : unmanaged
		=> unchecked((T*)SDL_aligned_alloc(alignment, unchecked((elementCount is > 0 ? elementCount : 1) * (nuint)Unsafe.SizeOf<T>())));

	/// <summary>
	/// Frees memory allocated by <see cref="AlignedAlloc(nuint, nuint)"/> or <see cref="AlignedAlloc{T}(nuint, nuint)"/>
	/// </summary>
	/// <param name="memory">A pointer previously returned by <see cref="AlignedAlloc(nuint, nuint)"/> or <see cref="AlignedAlloc{T}(nuint, nuint)"/>, or <c><see langword="null"/></c></param>
	/// <remarks>
	/// <para>
	/// The <paramref name="memory"/> is no longer valid after a call to this method and pointers into it should not be dereferenced anymore.
	/// </para>
	/// <para>
	/// If the <paramref name="memory"/> pointer is <c><see langword="null"/></c>, this method does nothing.
	/// </para>
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public unsafe static void AlignedFree(void* memory)
		=> SDL_aligned_free(memory);

	/// <summary>
	/// Allocates zero-initialized memory to hold a given number of elements of a given size
	/// </summary>
	/// <param name="elementCount">The number of elements in the array</param>
	/// <param name="elementSize">The size of each element of the array</param>
	/// <returns>A pointer to the allocated memory, or <c><see langword="null"/></c> if the allocation failed</returns>
	/// <remarks>
	/// <para>
	/// The memory returned by this method must be freed with <see cref="Free(void*)"/>.
	/// </para>
	/// <para>
	/// If either of <paramref name="elementCount"/> or <paramref name="elementSize"/> is <c>0</c>, they will both be set to <c>1</c> instead.
	/// </para>
	/// <para>
	/// The returned memory is guaranteed to be aligned to either the <em>fundamental alignment</em> (<c>alignof(max_align_t)</c> in C11 and later) or <c>2 * <see langword="sizeof"/>(<see langword="void"/>*)</c>, whichever is smaller.
	/// </para>
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public unsafe static void* Calloc(nuint elementCount, nuint elementSize)
		=> SDL_calloc(elementCount, elementSize);

	/// <summary>
	/// Allocates zero-initialized memory to hold a given count of elements of a given type
	/// </summary>
	/// <typeparam name="T">The type of the elements in the array</typeparam>
	/// <param name="elementCount">The number of elements in the array</param>
	/// <returns>A pointer to the allocated memory, or <c><see langword="null"/></c> if the allocation failed</returns>
	/// <remarks>
	/// <para>
	/// The memory returned by this method must be freed with <see cref="Free(void*)"/>.
	/// </para>
	/// <para>
	/// If the <paramref name="elementCount"/> is <c>0</c>, it will be set to <c>1</c> instead.
	/// </para>
	/// <para>
	/// If the size of a single <typeparamref name="T"/> instance happens to be <c>0</c>, memory with an overall size of <c>1</c> will be allocated.
	/// </para>
	/// <para>
	/// The returned memory is guaranteed to be aligned to either the <em>fundamental alignment</em> (<c>alignof(max_align_t)</c> in C11 and later) or <c>2 * <see langword="sizeof"/>(<see langword="void"/>*)</c>, whichever is smaller.
	/// </para>
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public unsafe static T* Calloc<T>(nuint elementCount)
		where T : unmanaged
		=> unchecked((T*)SDL_calloc(elementCount, (nuint)Unsafe.SizeOf<T>()));

	/// <summary>
	/// Frees allocated memory
	/// </summary>
	/// <param name="memory">A pointer to allocated memory, or <c><see langword="null"/></c></param>
	/// <remarks>
	/// <para>
	/// The <paramref name="memory"/> is no longer valid after a call to this method and pointers into it should not be dereferenced anymore.
	/// </para>
	/// <para>
	/// If the <paramref name="memory"/> pointer is <c><see langword="null"/></c>, this method does nothing.
	/// </para>
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public unsafe static void Free(void* memory)
		=> SDL_free(memory);

	private static volatile NativeMemoryFunctions? mCachedMemoryFunctions = null;

	/// <summary>
	/// Gets the current set of SDL's native memory functions
	/// </summary>
	/// <returns>The current set of SDL's native memory functions</returns>
	/// <remarks>
	/// This does not hold a lock, so do not call this in the unlikely event of a background thread calling <see cref="TrySetMemoryFunctions(INativeMemoryFunctions)"/> simultaneously.
	/// </remarks>
	public static INativeMemoryFunctions GetMemoryFunctions()
	{
		unsafe
		{
			MallocFunc malloc; CallocFunc calloc; ReallocFunc realloc; FreeFunc free;
			SDL_GetMemoryFunctions(&malloc, &calloc, &realloc, &free);

			if (mCachedMemoryFunctions is { Malloc: var cachedMalloc, Calloc: var cachedCalloc, Realloc: var cachedRealloc, Free: var cachedFree }
				&& (void*)malloc == (void*)cachedMalloc
				&& (void*)calloc == (void*)cachedCalloc
				&& (void*)realloc == (void*)cachedRealloc
				&& (void*)free == (void*)cachedFree)
			{
				return mCachedMemoryFunctions;
			}

			return mCachedMemoryFunctions = new NativeMemoryFunctions(malloc, calloc, realloc, free);
		}
	}

	private static volatile NativeMemoryFunctions? mCachedOriginalMemoryFuntions = null;

	/// <summary>
	/// Gets the original set of SDL's native memory functions
	/// </summary>
	/// <returns>The original set of SDL's native memory functions</returns>
	/// <remarks>
	/// This is what <see cref="Malloc(nuint)"/> and friends will use by default, if there has been no change made by a call to <see cref="TrySetMemoryFunctions(INativeMemoryFunctions)"/>.
	/// These functions are not necessarily using the C runtime's <c>malloc</c> and friends functions behind the scenes!
	/// Different platforms and build configurations might do any number of unexpected things.
	/// </remarks>
	public static INativeMemoryFunctions GetOriginalMemoryFunctions()
	{
		if (mCachedOriginalMemoryFuntions is not null)
		{
			return mCachedOriginalMemoryFuntions;
		}

		unsafe
		{
			MallocFunc malloc; CallocFunc calloc; ReallocFunc realloc; FreeFunc free;
			SDL_GetOriginalMemoryFunctions(&malloc, &calloc, &realloc, &free);

			return mCachedOriginalMemoryFuntions = new NativeMemoryFunctions(malloc, calloc, realloc, free);
		}
	}

	/// <summary>
	/// Allocates uninitialized memory
	/// </summary>
	/// <param name="size">The size to allocate</param>
	/// <returns>A pointer to the allocated memory, or <c><see langword="null"/></c> if the allocation failed</returns>
	/// <remarks>
	/// <para>
	/// The memory returned by this method must be freed with <see cref="Free(void*)"/>.
	/// </para>
	/// <para>
	/// If the <paramref name="size"/> is <c>0</c>, it will be set to <c>1</c> instead.
	/// </para>
	/// <para>
	/// The returned memory is guaranteed to be aligned to either the <em>fundamental alignment</em> (<c>alignof(max_align_t)</c> in C11 and later) or <c>2 * <see langword="sizeof"/>(<see langword="void"/>*)</c>, whichever is smaller.
	/// Use <see cref="AlignedAlloc(nuint, nuint)"/> if you need to allocate memory aligned to an alignment greater than this guarantee.
	/// </para>
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public unsafe static void* Malloc(nuint size) => SDL_malloc(size);

	/// <summary>
	/// Allocates uninitialized memory to hold a given count of elements of a given type
	/// </summary>
	/// <typeparam name="T">The type of the elements in the array</typeparam>
	/// <param name="elementCount">The number of elements in the array</param>
	/// <returns>A pointer to the allocated memory, or <c><see langword="null"/></c> if the allocation failed</returns>
	/// <remarks>
	/// <para>
	/// The memory returned by this method must be freed with <see cref="Free(void*)"/>.
	/// </para>
	/// <para>
	/// If the <paramref name="elementCount"/> is <c>0</c>, it will be set to <c>1</c> instead.
	/// </para>
	/// <para>
	/// If the size of a single <typeparamref name="T"/> instance happens to be <c>0</c>, memory with an overall size of <c>1</c> will be allocated.
	/// </para>
	/// <para>
	/// The returned memory is guaranteed to be aligned to either the <em>fundamental alignment</em> (<c>alignof(max_align_t)</c> in C11 and later) or <c>2 * <see langword="sizeof"/>(<see langword="void"/>*)</c>, whichever is smaller.
	/// Use <see cref="AlignedAlloc{T}(nuint, nuint)"/> if you need to allocate memory aligned to an alignment greater than this guarantee.
	/// </para>
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public unsafe static T* Malloc<T>(nuint elementCount)
		where T : unmanaged
		=> unchecked((T*)SDL_malloc(unchecked((elementCount is > 0 ? elementCount : 1) * (nuint)Unsafe.SizeOf<T>())));

	/// <summary>
	/// Compares two buffers of memory
	/// </summary>
	/// <param name="firstBuffer">The first buffer to compare</param>
	/// <param name="secondBuffer">The second buffer to compare</param>
	/// <param name="length">The number of bytes to compare between the buffers</param>
	/// <returns>
	/// A value less than <c>0</c>, if <paramref name="firstBuffer"/> is considered "less than" <paramref name="secondBuffer"/> after the first <paramref name="length"/> bytes;
	/// otherwise a value greater than <c>0</c>, if <paramref name="firstBuffer"/> is considered "greater than" <paramref name="secondBuffer"/> after the first <paramref name="length"/> bytes;
	/// otherwise <c>0</c>, which means <paramref name="firstBuffer"/> matches <paramref name="secondBuffer"/> exactly for <paramref name="length"/> bytes
	/// </returns>
	/// <remarks>
	/// <para>
	/// There are special checks in place, for when <paramref name="firstBuffer"/> or <paramref name="secondBuffer"/> are <c><see langword="null"/></c>:
	/// <list type="bullet">
	/// <item>
	///		<term><paramref name="firstBuffer"/> and <paramref name="secondBuffer"/> are both <c><see langword="null"/></c></term>
	///		<description>This method returns <c>0</c></description>
	/// </item>
	/// <item>
	///		<term><paramref name="firstBuffer"/> is <c><see langword="null"/></c> while <paramref name="secondBuffer"/> is not</term>
	///		<description>This method returns <c>-1</c></description>
	/// </item>
	/// <item>
	///		<term><paramref name="firstBuffer"/> is not <c><see langword="null"/></c> while <paramref name="secondBuffer"/> is</term>
	///		<description>This method returns <c>1</c></description>
	/// </item>
	/// </list>
	/// Notice that in all of the special cases above the <paramref name="length"/> doesn't play a role.
	/// </para>
	/// <para>
	/// Note: There are no additional checks in place regarding the length of the buffers and the given <paramref name="length"/> parameter.
	/// Make sure that both buffers, <paramref name="firstBuffer"/> and <paramref name="secondBuffer"/>, are safely dereferencable for reading for at least <paramref name="length"/> bytes!
	/// </para>
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public unsafe static int MemCmp(void* firstBuffer, void* secondBuffer, nuint length) => (unchecked((IntPtr)firstBuffer), unchecked((IntPtr)secondBuffer)) switch
	{
		(0, 0) => 0,
		(0, _) => -1,
		(_, 0) => 1,
		_ => SDL_memcmp(firstBuffer, secondBuffer, length)
	};

	/// <summary>
	/// Copies specified number of bytes form one buffer of memory into another non-overlapping one
	/// </summary>
	/// <param name="destination">The destination memory region. Must not overlap with <paramref name="source"/>.</param>
	/// <param name="source">The source memory region. Must not overlap with <paramref name="destination"/>.</param>
	/// <param name="length">The length in bytes of both <paramref name="destination"/> and <paramref name="source"/></param>
	/// <returns>The same pointer as the given <paramref name="destination"/></returns>
	/// <remarks>
	/// <para>
	/// There are special checks in place, for when <paramref name="destination"/> or <paramref name="source"/> are <c><see langword="null"/></c>.
	/// In those cases <paramref name="destination"/> is just returned (even if it's <c><see langword="null"/></c>) without doing anything else.
	/// </para>
	/// <para>
	/// The memory regions must not overlap! If they might do, use <see cref="MemMove(void*, void*, nuint)"/> instead.
	/// </para>
	/// <para>
	/// Note: There are no additional checks in place regarding the length of the buffers and the given <paramref name="length"/> parameter.
	/// Make sure that <paramref name="destination"/> is safely dereferenable for writing and <paramref name="source"/> is safely dereferencable for reading, both for at least <paramref name="length"/> bytes!
	/// </para>
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public unsafe static void* MemCpy(void* destination, void* source, nuint length) => (unchecked((IntPtr)destination), unchecked((IntPtr)source)) switch
	{
		(0, _) or (_, 0) => destination,
		_ => SDL_memcpy(destination, source, length)
	};

	/// <summary>
	/// Copies specified number of bytes form one buffer of memory into another one that might overlap
	/// </summary>
	/// <param name="destination">The destination memory region</param>
	/// <param name="source">The source memory region</param>
	/// <param name="length">The length in bytes of both <paramref name="destination"/> and <paramref name="source"/></param>
	/// <returns>The same pointer as the given <paramref name="destination"/></returns>
	/// <remarks>
	/// <para>
	/// There are special checks in place, for when <paramref name="destination"/> or <paramref name="source"/> are <c><see langword="null"/></c>.
	/// In those cases <paramref name="destination"/> is just returned (even if it's <c><see langword="null"/></c>) without doing anything else.
	/// </para>
	/// <para>
	/// It is okay for the memory regions to overlap. If you are confident that the regions never overlap, you might want to uss <see cref="MemCpy(void*, void*, nuint)"/> instead to improve performance.
	/// </para>
	/// <para>
	/// Note: There are no additional checks in place regarding the length of the buffers and the given <paramref name="length"/> parameter.
	/// Make sure that <paramref name="destination"/> is safely dereferenable for writing and <paramref name="source"/> is safely dereferencable for reading, both for at least <paramref name="length"/> bytes!
	/// </para>
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public unsafe static void* MemMove(void* destination, void* source, nuint length) => (unchecked((IntPtr)destination), unchecked((IntPtr)source)) switch
	{
		(0, _) or (_, 0) => destination,
		_ => SDL_memcpy(destination, source, length)
	};

	/// <summary>
	/// Initialize all bytes of a buffer of memory to a specified value
	/// </summary>
	/// <param name="destination">The destination memory region</param>
	/// <param name="value">The <see cref="byte"/> value to set</param>
	/// <param name="length">The length in bytes to set in <paramref name="destination"/></param>
	/// <returns>The same pointer as the given <paramref name="destination"/></returns>
	/// <remarks>
	/// <para>
	/// There is a special check in place, for when <paramref name="destination"/> is <c><see langword="null"/></c>.
	/// In that case <c><see langword="null"/></c> is just returned without doing anything else.
	/// </para>
	/// <para>
	/// Note: There are no additional checks in place regarding the length of <paramref name="destination"/> and the given <paramref name="length"/> parameter.
	/// Make sure that <paramref name="destination"/> is safely dereferenable for writing for at least <paramref name="length"/> bytes!
	/// </para>
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public unsafe static void* MemSet(void* destination, byte value, nuint length) => destination switch
	{
		null => null,
		_ => SDL_memset(destination, value, length)
	};

	/// <summary>
	/// Initialize all 32-bit words of a buffer of memory to a specific value
	/// </summary>
	/// <param name="destination">The destination memory region</param>
	/// <param name="value">The <see cref="uint"/> value to set</param>
	/// <param name="dwords">The number of <see cref="uint"/> values (32-bit words) to set in <paramref name="destination"/></param>
	/// <returns>The same pointer as the given <paramref name="destination"/></returns>
	/// <remarks>
	/// <para>
	/// There is a special check in place, for when <paramref name="destination"/> is <c><see langword="null"/></c>.
	/// In that case <c><see langword="null"/></c> is just returned without doing anything else.
	/// </para>
	/// <para>
	/// Note: There are no additional checks in place regarding the length of <paramref name="destination"/> and the given <paramref name="dwords"/> parameter.
	/// Make sure that <paramref name="destination"/> is safely dereferenable for writing for at least the number of <see cref="uint"/> values given by <paramref name="dwords"/> (that is <c><see langword="sizeof"/>(<see cref="uint"/>) * <paramref name="dwords"/></c> bytes)!
	/// </para>
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public unsafe static void* MemSet(void* destination, uint value, nuint dwords) => destination switch
	{
		null => null,
		_ => SDL_memset4(destination, value, dwords)
	};

	/// <summary>
	/// Changes the size of allocated memory
	/// </summary>
	/// <param name="memory">A pointer to allocated memory to reallocate, or <c><see langword="null"/></c></param>
	/// <param name="newSize">The new size of the memory</param>
	/// <returns>A pointer to the newly allocated memory, or <c><see langword="null"/></c> if the allocation failed</returns>
	/// <remarks>
	/// <para>
	/// The memory returned by this method must be freed with <see cref="Free(void*)"/>.
	/// </para>
	/// <para>
	/// If the <paramref name="newSize"/> is <c>0</c>, it will be set to <c>1</c> instead.
	/// Note that this is unlike some other C runtime <c>realloc</c> implementations, which may treat <c><see cref="Realloc(void*, nuint)">Realloc</see>(<paramref name="memory"/>, 0)</c> the same way as <c><see cref="Free(void*)">Free</see>(<paramref name="memory"/>)</c>.
	/// </para>
	/// <para>
	/// If the <paramref name="memory"/> pointer is <c><see langword="null"/></c>, the behavior of this method is equivalent to <c><see cref="Malloc(nuint)">Malloc</see>(<paramref name="newSize"/>)</c>.
	/// Otherwise, the result of this method can have one of three possible outcomes:
	/// <list type="bullet">
	///		<item>
	///			<description>If it returns the same pointer as <paramref name="memory"/>, it means that the <paramref name="memory"/> was resized in place without freeing</description>
	///		</item>
	///		<item>
	///			<description>If it returns a different non-<c><see langword="null"/></c> pointer, it means that the original <paramref name="memory"/> was freed and cannot be dereferenced there anymore</description>
	///		</item>
	///		<item>
	///			<description>If it returns <c><see langword="null"/></c> (indicating failure), then the <paramref name="memory"/> will remain valid and must still be freed with <see cref="Free(void*)"/></description>
	///		</item>
	/// </list>
	/// </para>
	/// <para>
	/// The returned memory is guaranteed to be aligned to either the <em>fundamental alignment</em> (<c>alignof(max_align_t)</c> in C11 and later) or <c>2 * <see langword="sizeof"/>(<see langword="void"/>*)</c>, whichever is smaller.
	/// </para>
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public unsafe static void* Realloc(void* memory, nuint newSize) => SDL_realloc(memory, newSize);

	/// <summary>
	/// Changes the size of allocated memory to hold a given count of elements of a given type
	/// </summary>
	/// <typeparam name="T">The type of the elements in the array</typeparam>
	/// <param name="memory">A pointer to allocated memory to reallocate, or <c><see langword="null"/></c></param>
	/// <param name="newElementCount">The new number of elements in the array</param>
	/// <returns>A pointer to the newly allocated memory, or <c><see langword="null"/></c> if the allocation failed</returns>
	/// <remarks>
	/// <para>
	/// The memory returned by this method must be freed with <see cref="Free(void*)"/>.
	/// </para>
	/// <para>
	/// If the <paramref name="newElementCount"/> is <c>0</c>, it will be set to <c>1</c> instead.
	/// Note that this is unlike some other C runtime <c>realloc</c> implementations, which may treat <c><see cref="Realloc{T}(T*, nuint)">Realloc</see>&lt;<typeparamref name="T"/>&gt;(<paramref name="memory"/>, 0)</c> the same way as <c><see cref="Free(void*)">Free</see>(<paramref name="memory"/>)</c>.
	/// </para>
	/// <para>
	/// If the size of a single <typeparamref name="T"/> instance happens to be <c>0</c>, memory with an overall size of <c>1</c> will be allocated.
	/// </para>
	/// <para>
	/// If the <paramref name="memory"/> pointer is <c><see langword="null"/></c>, the behavior of this method is equivalent to <c><see cref="Malloc{T}(nuint)">Malloc</see>&lt;<typeparamref name="T"/>&gt;(<paramref name="newElementCount"/>)</c>.
	/// Otherwise, the result of this method can have one of three possible outcomes:
	/// <list type="bullet">
	///		<item>
	///			<description>If it returns the same pointer as <paramref name="memory"/>, it means that the <paramref name="memory"/> was resized in place without freeing</description>
	///		</item>
	///		<item>
	///			<description>If it returns a different non-<c><see langword="null"/></c> pointer, it means that the original <paramref name="memory"/> was freed and cannot be dereferenced there anymore</description>
	///		</item>
	///		<item>
	///			<description>If it returns <c><see langword="null"/></c> (indicating failure), then the <paramref name="memory"/> will remain valid and must still be freed with <see cref="Free(void*)"/></description>
	///		</item>
	/// </list>
	/// </para>
	/// <para>
	/// The returned memory is guaranteed to be aligned to either the <em>fundamental alignment</em> (<c>alignof(max_align_t)</c> in C11 and later) or <c>2 * <see langword="sizeof"/>(<see langword="void"/>*)</c>, whichever is smaller.
	/// </para>
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public unsafe static T* Realloc<T>(T* memory, nuint newElementCount)
		where T : unmanaged
		=> unchecked((T*)SDL_realloc(memory, unchecked((newElementCount is > 0 ? newElementCount : 1) * (nuint)Unsafe.SizeOf<T>())));

	/// <summary>
	/// Replace SDL's native memory functions with a custom set
	/// </summary>
	/// <param name="memoryFunctions">The set of custom native memory functions to replace SDL's current one</param>
	/// <returns><c><see langword="true"/></c>, if the set of SDL's current native memory functions was successfully replaced by the given custom one; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// It is not safe to call this method once any allocations have been made, as future calls to <see cref="Free(void*)"/> will use the new allocator, even if those allocations were allocated by an older set of native memory functions!
	/// </para>
	/// <para>
	/// If this method is used, usually it needs to be the very first call made using the SDL library, if not the very first thing to do at the program's startup time.
	/// </para>
	/// </remarks>
	public static bool TrySetMemoryFunctions(INativeMemoryFunctions memoryFunctions)
	{
		unsafe
		{
			MallocFunc malloc; CallocFunc calloc; ReallocFunc realloc; FreeFunc free;

			if (memoryFunctions is NativeMemoryFunctions nativeMemoryFunctions)
			{
				malloc = nativeMemoryFunctions.Malloc;
				calloc = nativeMemoryFunctions.Calloc;
				realloc = nativeMemoryFunctions.Realloc;
				free = nativeMemoryFunctions.Free;
			}
			else
			{
				malloc = unchecked((MallocFunc)(void*)Marshal.GetFunctionPointerForDelegate(memoryFunctions.Malloc));
				calloc = unchecked((CallocFunc)(void*)Marshal.GetFunctionPointerForDelegate(memoryFunctions.Calloc));
				realloc = unchecked((ReallocFunc)(void*)Marshal.GetFunctionPointerForDelegate(memoryFunctions.Realloc));
				free = unchecked((FreeFunc)(void*)Marshal.GetFunctionPointerForDelegate(memoryFunctions.Free));
			}

			return SDL_SetMemoryFunctions(malloc, calloc, realloc, free);
		}
	}
}