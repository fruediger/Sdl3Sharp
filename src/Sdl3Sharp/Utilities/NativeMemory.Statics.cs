using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using unsafe SDL_calloc_func = delegate* unmanaged[Cdecl, SuppressGCTransition]<System.UIntPtr, System.UIntPtr, void*>;
using unsafe SDL_free_func = delegate* unmanaged[Cdecl, SuppressGCTransition]<void*, void>;
using unsafe SDL_malloc_func = delegate* unmanaged[Cdecl, SuppressGCTransition]<System.UIntPtr, void*>;
using unsafe SDL_realloc_func = delegate* unmanaged[Cdecl, SuppressGCTransition]<void*, System.UIntPtr, void*>;

namespace Sdl3Sharp.Utilities;

partial struct NativeMemory
{
	/// <summary>
	/// Gets the number of outstanding (unfreed) allocations
	/// </summary>
	/// <value>
	/// The number of outstanding (unfreed) allocations, or <c>-1</c> if allocation counting is disabled
	/// </value>
	public static int AllocationCount => SDL_GetNumAllocations();

	/// <summary>
	/// Allocates uninitialized memory that is aligned to a specific alignment
	/// </summary>
	/// <param name="alignment">The alignment of the memory</param>
	/// <param name="size">The size in bytes to allocate</param>
	/// <returns>A pointer to the allocated uninitialized aligned memory, or <c><see langword="null"/></c> if the allocation failed</returns>
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
	/// Frees memory allocated by <see cref="AlignedAlloc(nuint, nuint)"/>
	/// </summary>
	/// <param name="memory">A pointer previously returned by <see cref="AlignedAlloc(nuint, nuint)"/>, or <c><see langword="null"/></c></param>
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
	/// <param name="elementCount">The number of elements to allocate</param>
	/// <param name="elementSize">The size in bytes of each individual element</param>
	/// <returns>A pointer to the allocated zero-initialized memory, or <c><see langword="null"/></c> if the allocation failed</returns>
	/// <remarks>
	/// <para>
	/// The memory returned by this method must be freed with <see cref="Free(void*)"/>.
	/// </para>
	/// <para>
	/// If either of <paramref name="elementCount"/> or <paramref name="elementSize"/> are <c>0</c>, they will both be set to <c>1</c> instead.
	/// </para>
	/// <para>
	/// The returned memory is guaranteed to be aligned to either the <em>fundamental alignment</em> (<c>alignof(max_align_t)</c> in C11 and later) or <c>2 * <see langword="sizeof"/>(<see langword="void"/>*)</c>, whichever is smaller.
	/// </para>
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public unsafe static void* Calloc(nuint elementCount, nuint elementSize)
		=> SDL_calloc(elementCount, elementSize);

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
			SDL_malloc_func malloc; SDL_calloc_func calloc; SDL_realloc_func realloc; SDL_free_func free;
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
			SDL_malloc_func malloc; SDL_calloc_func calloc; SDL_realloc_func realloc; SDL_free_func free;
			SDL_GetOriginalMemoryFunctions(&malloc, &calloc, &realloc, &free);

			return mCachedOriginalMemoryFuntions = new NativeMemoryFunctions(malloc, calloc, realloc, free);
		}
	}

	/// <summary>
	/// Allocates uninitialized memory
	/// </summary>
	/// <param name="size">The size in bytes to allocate</param>
	/// <returns>A pointer to the allocated uninitialized memory, or <c><see langword="null"/></c> if the allocation failed</returns>
	/// <remarks>
	/// <para>
	/// The memory returned by this method must be freed with <see cref="Free(void*)"/>.
	/// </para>
	/// <para>
	/// If <paramref name="size"/> is <c>0</c>, it will be set to <c>1</c> instead.
	/// </para>
	/// <para>
	/// The returned memory is guaranteed to be aligned to either the <em>fundamental alignment</em> (<c>alignof(max_align_t)</c> in C11 and later) or <c>2 * <see langword="sizeof"/>(<see langword="void"/>*)</c>, whichever is smaller.
	/// Use <see cref="AlignedAlloc(nuint, nuint)"/> if you need to allocate memory aligned to an alignment greater than this guarantee.
	/// </para>
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public unsafe static void* Malloc(nuint size) => SDL_malloc(size);

	/// <summary>
	/// Compares two buffers of memory
	/// </summary>
	/// <param name="firstBuffer">The first buffer to compare</param>
	/// <param name="secondBuffer">The second buffer to compare</param>
	/// <param name="length">The number of bytes to compare between the buffers</param>
	/// <returns>
	/// A value less than <c>0</c>, if <paramref name="firstBuffer"/> is considered "less than" <paramref name="secondBuffer"/> after the first <paramref name="length"/> bytes;
	/// otherwise, a value greater than <c>0</c>, if <paramref name="firstBuffer"/> is considered "greater than" <paramref name="secondBuffer"/> after the first <paramref name="length"/> bytes;
	/// otherwise, <c>0</c>, which means <paramref name="firstBuffer"/> matches <paramref name="secondBuffer"/> exactly for <paramref name="length"/> bytes
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
	/// Copies specified number of bytes from one buffer of memory into another <em>non-overlapping</em> one
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
	/// It is okay for the memory regions to overlap. If you are confident that the regions never overlap, you might want to use <see cref="MemCpy(void*, void*, nuint)"/> instead to improve performance.
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
		_ => SDL_memmove(destination, source, length)
	};

	/// <summary>
	/// Initializes all bytes of a buffer of memory to a specified value
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
	/// Initializes all 32-bit words of a buffer of memory to a specific value
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
	/// <param name="newSize">The new size in bytes of the memory</param>
	/// <returns>A pointer to the newly allocated memory, or <c><see langword="null"/></c> if the allocation failed</returns>
	/// <remarks>
	/// <para>
	/// The memory returned by this method must be freed with <see cref="Free(void*)"/>.
	/// </para>
	/// <para>
	/// If <paramref name="newSize"/> is <c>0</c>, it will be set to <c>1</c> instead.
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
	/// Tries to allocate uninitialized memory that is aligned to a specific alignment
	/// </summary>
	/// <param name="alignment">The alignment of the memory</param>
	/// <param name="length">The size in bytes to allocate</param>
	/// <param name="memoryManager">The resulting <see cref="NativeMemoryManager"/> that manages the allocated uninitialized aligned memory, when this method returns <c><see langword="true"/></c>; otherwise, <c><see langword="null"/></c></param>
	/// <returns><c><see langword="true"/></c>, if the uninitialized aligned memory could be successfull allocated; otherwise, <c><see langword="false"/></c></returns>
	/// <remarks>
	/// <para>
	/// The resulting <see cref="NativeMemoryManager"/> should be <see cref="NativeMemoryManager.Dispose()">disposed</see> when the memory it's managing is no longer needed. That also frees the allocated memory.
	/// </para>
	/// <para>
	/// If the <paramref name="alignment"/> is less than <c><see langword="sizeof"/>(<see langword="void"/>*)</c>, it will be increased to match that instead.
	/// </para>
	/// <para>
	/// The value of the resulting <see cref="NativeMemoryManager"/>'s <see cref="NativeMemoryManager.Pointer"/> property will be a multiple of the <paramref name="alignment"/>,
	/// and the actual size of the memory allocated (not necessarily identical to <see cref="NativeMemoryManager.Length"/>) will be a multiple of the <paramref name="alignment"/> too.
	/// </para>
	/// </remarks>
	public static bool TryAlignedAlloc(nuint alignment, nuint length, [NotNullWhen(true)] out NativeMemoryManager? memoryManager)
	{
		unsafe
		{
			var ptr = AlignedAlloc(alignment, length);

			if (ptr is null)
			{
				memoryManager = null;
				return false;
			}

			memoryManager = new(ptr, length, &SDL_aligned_free);
			return true;
		}
	}

	/// <summary>
	/// Tries to allocate zero-initialized memory to hold a given number of elements of a given size
	/// </summary>
	/// <param name="elementCount">The number of elements to allocate</param>
	/// <param name="elementSize">The size in bytes of each individual element</param>
	/// <param name="memoryManager">The resulting <see cref="NativeMemoryManager"/> that manages the allocated zero-initialized memory, when this method returns <c><see langword="true"/></c>; otherwise, <c><see langword="null"/></c></param>
	/// <returns><c><see langword="true"/></c>, if the zero-initialized memory could be successfull allocated; otherwise, <c><see langword="false"/></c></returns>
	/// <remarks>
	/// <para>
	/// The resulting <see cref="NativeMemoryManager"/> should be <see cref="NativeMemoryManager.Dispose()">disposed</see> when the memory it's managing is no longer needed. That also frees the allocated memory.
	/// </para>
	/// <para>
	/// If either of <paramref name="elementCount"/> or <paramref name="elementSize"/> are <c>0</c>, they will both be set to <c>1</c> instead.
	/// </para>
	/// <para>
	/// The value of the resulting <see cref="NativeMemoryManager"/>'s <see cref="NativeMemoryManager.Pointer"/> property is guaranteed to be aligned to
	/// either the <em>fundamental alignment</em> (<c>alignof(max_align_t)</c> in C11 and later) or <c>2 * <see langword="sizeof"/>(<see langword="void"/>*)</c>, whichever is smaller.
	/// </para>
	/// </remarks>
	public static bool TryCalloc(nuint elementCount, nuint elementSize, [NotNullWhen(true)] out NativeMemoryManager? memoryManager)
	{
		unsafe
		{
			var ptr = Calloc(elementCount, elementSize);

			if (ptr is null)
			{
				memoryManager = null;
				return false;
			}

			memoryManager = new(ptr, unchecked(elementCount * elementSize) switch { not > 0 => 1, var byteLength => byteLength }, &SDL_free);
			return true;
		}
	}

	/// <summary>
	/// Tries to compare two <see cref="NativeMemory">allocated memory buffers</see>
	/// </summary>
	/// <param name="firstBuffer">The first buffer to compare</param>
	/// <param name="secondBuffer">The second buffer to compare</param>
	/// <param name="result">
	/// <c><see langword="default"/>(<see langword="int"/>)</c>, when this method returns <c><see langword="false"/></c>;
	/// otherwise, a value less than <c>0</c>, if <paramref name="firstBuffer"/> is considered "less than" <paramref name="secondBuffer"/>;
	/// otherwise, a value greater than <c>0</c>, if <paramref name="firstBuffer"/> is considered "greater than" <paramref name="secondBuffer"/>;
	/// otherwise, <c>0</c>, which means <paramref name="firstBuffer"/> matches <paramref name="secondBuffer"/> exactly
	/// </param>
	/// <returns>
	/// <c><see langword="true"/></c>, if <paramref name="firstBuffer"/> and <paramref name="secondBuffer"/> are both <see cref="IsValid">valid</see>
	/// and have the same <see cref="Length"/>;
	/// otherwise, <c><see langword="false"/></c>
	/// </returns>
	/// <remarks>
	/// <para>
	/// If one of <paramref name="firstBuffer"/> or <paramref name="secondBuffer"/> is <see cref="IsEmpty">empty</see> (or both are), this method returns <c><see langword="true"/></c> and <paramref name="result"/> is <c>0</c>.
	/// </para>
	/// </remarks>
	public static bool TryCompare(NativeMemory firstBuffer, NativeMemory secondBuffer, out int result)
	{
		unsafe
		{
			if (!(firstBuffer.IsValid && secondBuffer.IsValid))
			{
				result = default;
				return false;
			}

			var length = firstBuffer.Length;

			if (length != secondBuffer.Length)
			{
				result = default;
				return false;
			}

			if (length is 0)
			{
				result = 0;
				return true;
			}

			result = MemCmp(firstBuffer.RawPointer, secondBuffer.RawPointer, length);
			return true;
		}
	}

	/// <summary>
	/// Tries to copy all bytes from one <see cref="NativeMemory">allocated memory buffer</see> into another <em>non-overlapping</em> one
	/// </summary>
	/// <param name="destination">The destination <see cref="NativeMemory">memory buffer</see>. Must not overlap with <paramref name="source"/>.</param>
	/// <param name="source">The source <see cref="NativeMemory">memory buffer</see>. Must not overlap with <paramref name="destination"/>.</param>
	/// <returns>
	/// <c><see langword="true"/></c>, if <paramref name="destination"/> and <paramref name="source"/> are both <see cref="IsValid">valid</see> and 
	/// <paramref name="destination"/>'s <see cref="Length"/> is at least <paramref name="source"/>'s <see cref="Length"/>;
	/// otherwise, <c><see langword="false"/></c>
	/// </returns>
	/// <remarks>
	/// <para>
	/// The memory regions must not overlap! If they might do, use <see cref="TryMove(NativeMemory, NativeMemory)"/> instead.
	/// </para>
	/// </remarks>
	public static bool TryCopy(NativeMemory destination, NativeMemory source)
	{
		unsafe
		{
			if (!(destination.IsValid && source.IsValid))
			{
				return false;
			}

			if (destination.Length < source.Length)
			{
				return false;
			}

			MemCpy(destination.RawPointer, source.RawPointer, source.Length);
			return true;
		}
	}

	/// <summary>
	/// Tries to copy all bytes from one <see cref="NativeMemory">allocated memory buffer</see> into another one that might overlap
	/// </summary>
	/// <param name="destination">The destination <see cref="NativeMemory">memory buffer</see></param>
	/// <param name="source">The source <see cref="NativeMemory">memory buffer</see></param>
	/// <returns>
	/// <c><see langword="true"/></c>, if <paramref name="destination"/> and <paramref name="source"/> are both <see cref="IsValid">valid</see> and 
	/// <paramref name="destination"/>'s <see cref="Length"/> is at least <paramref name="source"/>'s <see cref="Length"/>;
	/// otherwise, <c><see langword="false"/></c>
	/// </returns>
	/// <remarks>
	/// <para>
	/// It is okay for the memory regions to overlap. If you are confident that the regions never overlap, you might want to use <see cref="TryCopy(NativeMemory, NativeMemory)"/> instead to improve performance.
	/// </para>
	/// </remarks>
	public static bool TryMove(NativeMemory destination, NativeMemory source)
	{
		unsafe
		{
			if (destination.Length < source.Length)
			{
				return false;
			}

			MemMove(destination.RawPointer, source.RawPointer, source.Length);
			return true;
		}
	}

	/// <summary>
	/// Tries to initialize all bytes of an <see cref="NativeMemory">allocated memory buffer</see> to a specified value
	/// </summary>
	/// <param name="destination">The <see cref="NativeMemory">allocated memory buffer</see></param>
	/// <param name="value">The <see cref="byte"/> value to set</param>
	/// <returns><c><see langword="true"/></c>, if <paramref name="destination"/> is <see cref="IsValid">valid</see>; otherwise, <c><see langword="false"/></c></returns>
	public static bool TrySet(NativeMemory destination, byte value)
	{
		unsafe
		{
			if (!destination.IsValid)
			{
				return false;
			}

			MemSet(destination.RawPointer, value, destination.Length);
			return true;
		}
	}	

	/// <summary>
	/// Tries to initialize all 32-bit words of an <see cref="NativeMemory">allocated memory buffer</see> to a specific value
	/// </summary>
	/// <param name="destination">The <see cref="NativeMemory">allocated memory buffer</see></param>
	/// <param name="value">The <see cref="uint"/> value to set</param>
	/// <returns><c><see langword="true"/></c>, if <paramref name="destination"/> is <see cref="IsValid">valid</see>; otherwise, <c><see langword="false"/></c></returns>
	public static bool TrySet(NativeMemory destination, uint value)
	{
		unsafe
		{
			if (!destination.IsValid)
			{
				return false;
			}

			MemSet(destination.RawPointer, value, destination.Length);
			return true;
		}
	}

	/// <summary>
	/// Tries to allocate uninitialized memory
	/// </summary>
	/// <param name="length">The size in bytes to allocate</param>
	/// <param name="memoryManager">The resulting <see cref="NativeMemoryManager"/> that manages the allocated uninitialized memory, when this method returns <c><see langword="true"/></c>; otherwise, <c><see langword="null"/></c></param>
	/// <returns><c><see langword="true"/></c>, if the uninitialized memory could be successfull allocated; otherwise, <c><see langword="false"/></c></returns>
	/// <remarks>
	/// <para>
	/// The resulting <see cref="NativeMemoryManager"/> should be <see cref="NativeMemoryManager.Dispose()">disposed</see> when the memory it's managing is no longer needed. That also frees the allocated memory.
	/// </para>
	/// <para>
	/// If <paramref name="length"/> is <c>0</c>, it will be set to <c>1</c> instead.
	/// </para>
	/// <para>
	/// The value of the resulting <see cref="NativeMemoryManager"/>'s <see cref="NativeMemoryManager.Pointer"/> property is guaranteed to be aligned to
	/// either the <em>fundamental alignment</em> (<c>alignof(max_align_t)</c> in C11 and later) or <c>2 * <see langword="sizeof"/>(<see langword="void"/>*)</c>, whichever is smaller.
	/// Use <see cref="TryAlignedAlloc(nuint, nuint, out NativeMemoryManager?)"/> if you need to allocate memory aligned to an alignment greater than this guarantee.
	/// </para>
	/// </remarks>
	public static bool TryMalloc(nuint length, [NotNullWhen(true)] out NativeMemoryManager? memoryManager)
	{
		unsafe
		{
			var ptr = Malloc(length);

			if (ptr is null)
			{
				memoryManager = null;
				return false;
			}

			memoryManager = new(ptr, length is > 0 ? length : 1, &SDL_free);
			return true;
		}
	}

	/// <summary>
	/// Tries to change the size of allocated native memory that is managed by a <see cref="NativeMemoryManager"/>
	/// </summary>
	/// <param name="memoryManager">
	/// A reference to <see cref="NativeMemoryManager"/> that manages the native memory to reallocate, or a reference to <c><see langword="null"/></c>.
	/// If this reference is referencing a <c><see langword="null"/></c> value, it might be referencing an actual <see cref="NativeMemoryManager"/> when this method returns <c><see langword="true"/></c>.
	/// </param>
	/// <param name="newLength">The new size in bytes of the memory</param>
	/// <returns><c><see langword="true"/></c>, if the memory could be successfull reallocated; otherwise, <c><see langword="false"/></c></returns>
	/// <remarks>
	/// <para>
	/// The resulting <see cref="NativeMemoryManager"/> should be still <see cref="NativeMemoryManager.Dispose()">disposed</see> when the memory it's managing is no longer needed. That also frees the allocated memory.
	/// </para>
	/// <para>
	/// The referenced <see cref="NativeMemoryManager"/> cannot be <see cref="NativeMemoryManager.IsPinned">pinned</see>. Trying to reallocated a pinned memory buffer results in failure.
	/// </para>
	/// <para>
	/// If <paramref name="newLength"/> is <c>0</c>, it will be set to <c>1</c> instead.
	/// Note that this is unlike some other C runtime <c>realloc</c> implementations, which may treat <c><see cref="TryRealloc(ref NativeMemoryManager?, nuint)">TryRealloc</see>(<see langword="ref"/> <paramref name="memoryManager"/>, 0)</c> the same way as <c><paramref name="memoryManager"/>.<see cref="NativeMemoryManager.Dispose()">Dispose</see>()</c>.
	/// </para>
	/// <para>
	/// If the reference to <paramref name="memoryManager"/> is referencing a <c><see langword="null"/></c> value, the behavior of this method is equivalent to <c><see cref="TryMalloc(nuint, out NativeMemoryManager?)">TryMalloc</see>(<paramref name="newLength"/>, <see langword="out"/> <paramref name="memoryManager"/>)</c>.
	/// Otherwise, a call to this method can have one of three possible outcomes:
	/// <list type="bullet">
	///		<item>
	///			<description>
	///				If the result of the call to this method is <c><see langword="true"/></c> and the values of <paramref name="memoryManager"/>'s <see cref="NativeMemoryManager.Pointer"/>, before and after this operation, are the same,
	///				it means that the original memory was resized in place without freeing
	///			</description>
	///		</item>
	///		<item>
	///			<description>
	///				If the result of the call to this method is <c><see langword="true"/></c> and the values of <paramref name="memoryManager"/>'s <see cref="NativeMemoryManager.Pointer"/>, before and after this operation, are different,
	///				it means that the original memory was freed and all <em>active</em> references to it (e.g. lasting <see cref="Span{T}"/>s) are invalid and cannot be dereferenced anymore
	///				(<see cref="NativeMemory"/> and <see cref="NativeMemory{T}"/> instances are safe though)
	///			</description>
	///		</item>
	///		<item>
	///			<description>
	///				The result of the call to this method is <c><see langword="false"/></c> (indicating failure),
	///				then the given <paramref name="memoryManager"/> will remain valid (if it was non-<c><see langword="null"/></c>) and should be still <see cref="NativeMemoryManager.Dispose()">disposed</see> when it's no longer needed
	///			</description>
	///		</item>
	/// </list>
	/// </para>
	/// <para>
	/// The value of the resulting <see cref="NativeMemoryManager"/>'s <see cref="NativeMemoryManager.Pointer"/> property is guaranteed to be aligned to
	/// either the <em>fundamental alignment</em> (<c>alignof(max_align_t)</c> in C11 and later) or <c>2 * <see langword="sizeof"/>(<see langword="void"/>*)</c>, whichever is smaller.
	/// </para>
	/// </remarks>
	public static bool TryRealloc([NotNullWhen(true), NotNullIfNotNull(nameof(memoryManager))] ref NativeMemoryManager? memoryManager, nuint newLength)
	{
		unsafe
		{
			if (memoryManager is null)
			{
				return TryMalloc(newLength, out memoryManager);
			}

			if (memoryManager.IsPinned)
			{
				return false;
			}

			var ptr = Realloc(memoryManager.RawPointer, newLength);

			if (ptr is null)
			{
				return false;
			}

			if (ptr != memoryManager.RawPointer)
			{
				memoryManager.RawPointer = ptr;
				memoryManager.Free = &SDL_free;
			}

			memoryManager.Length = newLength is > 0 ? newLength : 1;
			return true;
		}
	}

	/// <summary>
	/// Tries to replace SDL's native memory functions with a custom set
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
			SDL_malloc_func malloc; SDL_calloc_func calloc; SDL_realloc_func realloc; SDL_free_func free;

			if (memoryFunctions is NativeMemoryFunctions nativeMemoryFunctions)
			{
				malloc = nativeMemoryFunctions.Malloc;
				calloc = nativeMemoryFunctions.Calloc;
				realloc = nativeMemoryFunctions.Realloc;
				free = nativeMemoryFunctions.Free;
			}
			else
			{
				malloc = unchecked((SDL_malloc_func)(void*)Marshal.GetFunctionPointerForDelegate(memoryFunctions.Malloc));
				calloc = unchecked((SDL_calloc_func)(void*)Marshal.GetFunctionPointerForDelegate(memoryFunctions.Calloc));
				realloc = unchecked((SDL_realloc_func)(void*)Marshal.GetFunctionPointerForDelegate(memoryFunctions.Realloc));
				free = unchecked((SDL_free_func)(void*)Marshal.GetFunctionPointerForDelegate(memoryFunctions.Free));
			}

			return SDL_SetMemoryFunctions(malloc, calloc, realloc, free);
		}
	}
}
