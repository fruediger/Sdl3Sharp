using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Sdl3Sharp.Utilities;

/// <summary>
/// Manages and represents allocated native memory
/// </summary>
/// <remarks>
/// Note: Always remember to <see cref="Dispose()">dispose</see> <see cref="NativeMemoryManager"/> when the memory they're managing is no longer needed. That also frees the allocated memory.
/// </remarks>
/// <seealso cref="NativeMemory"/>
public sealed class NativeMemoryManager : IDisposable
{
	private unsafe void* mPointer;
	private nuint mLength;
	private unsafe delegate* managed<void*, void> mFree;

	private ulong mPinCounter = ulong.MinValue;

	internal unsafe NativeMemoryManager(void* pointer, nuint length, delegate* managed<void*, void> free)
	{
		mPointer = pointer;
		mLength = length;
		mFree = free;
	}

	/// <inheritdoc/>
	~NativeMemoryManager() => Dispose(ignorePinning: true);

	internal unsafe delegate* managed<void*, void> Free
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mFree;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mFree = value;
	}

	/// <summary>
	/// Gets a value indicating whether this <see cref="NativeMemoryManager"/> is pinned
	/// </summary>
	/// <value>
	/// A value indicating whether this <see cref="NativeMemoryManager"/> is pinned
	/// </value>
	/// <remarks>
	/// <para>
	/// A pinned <see cref="NativeMemoryManager"/> can't be <see cref="Dispose()">disposed</see> or be used in operations like <see cref="NativeMemory.TryRealloc(ref NativeMemoryManager?, nuint)"/>.
	/// </para>
	/// </remarks>
	/// <seealso cref="Pin"/>
	public bool IsPinned { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => PinCounter is > ulong.MinValue; }

	/// <summary>
	/// Gets the number of bytes in the allocated memory region this <see cref="NativeMemoryManager"/> represents
	/// </summary>
	/// <value>
	/// The number of bytes in the allocated memory region this <see cref="NativeMemoryManager"/> represents
	/// </value>
	public nuint Length
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mLength;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] internal set => mLength = value;
	}

	/// <summary>
	/// Gets the <see cref="NativeMemory">allocated memory buffer</see> this <see cref="NativeMemoryManager"/> represents
	/// </summary>
	/// <value>
	/// The <see cref="NativeMemory">allocated memory buffer</see> this <see cref="NativeMemoryManager"/> represents
	/// </value>
	public NativeMemory Memory { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(this, 0, mLength); }

	internal ulong PinCounter { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => Interlocked.Read(ref mPinCounter); }

	/// <summary>
	/// Gets a pointer to the start of the allocated memory region this <see cref="NativeMemoryManager"/> represents
	/// </summary>
	/// <value>
	/// A pointer to the start of the allocated memory region this <see cref="NativeMemoryManager"/> represents
	/// </value>
	public IntPtr Pointer { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get { unsafe { return unchecked((IntPtr)mPointer); } } }

	internal unsafe void* RawPointer
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mPointer;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mPointer = value;
	}

	internal ulong DecreasePinCounter()
	{
		while (Interlocked.Read(ref mPinCounter) is var pinCounter
			&& pinCounter is > ulong.MinValue)
		{
			var newPinCounter = unchecked(pinCounter - 1);
			if (Interlocked.CompareExchange(ref mPinCounter, newPinCounter, pinCounter) == pinCounter)
			{
				return newPinCounter;
			}
		}

		return ulong.MinValue;
	}

	/// <remarks>
	/// <para>
	/// This operation will free the allocated memory region this <see cref="NativeMemoryManager"/> represents.
	/// </para>
	/// <para>
	/// All of the <see cref="NativeMemory"/> and <see cref="NativeMemory{T}"/> instances, that are subsequently depending on this <see cref="NativeMemoryManager"/>,
	/// will become <see cref="NativeMemory.IsValid">invalid</see> of this operation.
	/// </para>
	/// <para>
	/// Note: Trying to dipose a <see cref="IsPinned">pinned</see> <see cref="NativeMemoryManager"/> will throw!
	/// Make sure that the <see cref="NativeMemoryManager"/> is not <see cref="IsPinned">pinned</see> anymore before trying to <see cref="Dispose()">dispose</see> it.
	/// </para>
	/// </remarks>
	/// <inheritdoc/>
	public void Dispose()
	{
		Dispose(ignorePinning: false);
		GC.SuppressFinalize(this);
	}

	/// <exception cref="InvalidOperationException">The <see cref="NativeMemoryManager"/> is still pinned</exception>
	private void Dispose(bool ignorePinning)
	{
		unsafe
		{
			if (mPointer is not null)
			{
				if (!ignorePinning && PinCounter is > ulong.MinValue)
				{
					failPinned();
				}

				if (mFree is not null)
				{
					mFree(mPointer);
					mFree = null;
				}

				mPointer = null;
			}

			mLength = 0;
		}

		[DoesNotReturn]
		static void failPinned() => throw new InvalidOperationException($"Cannot dispose a pinned {nameof(NativeMemoryManager)}, Dispose all active {nameof(NativeMemoryPin)}s first.");
	}

	internal ulong IncreasePinCounter()
	{
		while (Interlocked.Read(ref mPinCounter) is var pinCounter
			&& pinCounter is < ulong.MaxValue)
		{
			var newPinCounter = unchecked(pinCounter + 1);
			if (Interlocked.CompareExchange(ref mPinCounter, newPinCounter, pinCounter) == pinCounter)
			{
				return newPinCounter;
			}
		}

		return ulong.MaxValue;
	}

	/// <summary>
	/// Pins this <see cref="NativeMemoryManager"/>
	/// </summary>
	/// <returns>A <see cref="NativeMemoryPin">pin</see> pinning this <see cref="NativeMemoryManager"/></returns>
	/// <remarks>
	/// <para>
	/// A pinned <see cref="NativeMemoryManager"/> can't be <see cref="Dispose()">disposed</see> or be used in operations like <see cref="NativeMemory.TryRealloc(ref NativeMemoryManager?, nuint)"/>.
	/// </para>
	/// </remarks>
	public NativeMemoryPin Pin() => new(this);
}
