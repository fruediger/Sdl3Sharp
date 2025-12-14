using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Sdl3Sharp.Utilities;

/// <summary>
/// A base class for memory managers that manage and represent allocated native memory
/// </summary>
/// <remarks>
/// <para>
/// Note: Always remember to <see cref="Dispose()">dispose</see> <see cref="NativeMemoryManagerBase"/> when the memory they're managing is no longer needed.
/// </para>
/// </remarks>
/// <seealso cref="NativeMemory"/>
public abstract class NativeMemoryManagerBase : IDisposable
{
	private ulong mPinCounter = 0;

	private protected unsafe NativeMemoryManagerBase() { }

	/// <inheritdoc/>
	~NativeMemoryManagerBase() => Dispose(disposing: false);

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
	public virtual bool IsPinned { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => PinCounter is > 0; }

	/// <summary>
	/// Gets the number of bytes in the allocated memory region this <see cref="NativeMemoryManagerBase"/> represents
	/// </summary>
	/// <value>
	/// The number of bytes in the allocated memory region this <see cref="NativeMemoryManagerBase"/> represents
	/// </value>
	public abstract nuint Length { get; }

	/// <summary>
	/// Gets the <see cref="NativeMemory">allocated memory buffer</see> this <see cref="NativeMemoryManagerBase"/> represents
	/// </summary>
	/// <value>
	/// The <see cref="NativeMemory">allocated memory buffer</see> this <see cref="NativeMemoryManagerBase"/> represents
	/// </value>
	public virtual NativeMemory Memory { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(this, 0, Length); }

	/// <summary>
	/// Gets the current pin counter
	/// </summary>
	protected internal ulong PinCounter { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => Interlocked.Read(ref mPinCounter); }

	/// <summary>
	/// Gets a pointer to the start of the allocated memory region this <see cref="NativeMemoryManagerBase"/> represents
	/// </summary>
	/// <value>
	/// A pointer to the start of the allocated memory region this <see cref="NativeMemoryManagerBase"/> represents
	/// </value>
	public abstract IntPtr Pointer { get; }

	internal unsafe virtual void* RawPointer { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => unchecked((void*)Pointer); }

	/// <summary>
	/// Adds a "pin" to this <see cref="NativeMemoryManagerBase"/>
	/// </summary>
	/// <param name="oldPinCounter">The pin counter before it was increased</param>
	/// <param name="newPinCounter">The current pin counter after it was increased</param>
	/// <remarks>
	/// <para>
	/// This method is called each time the pin counter was successfully increased.
	/// You can override this method to implement custom logic that should be executed each time a "pin" is added to this <see cref="NativeMemoryManagerBase"/>.
	/// </para>
	/// <para>
	/// If you want a more general pinning logic that only triggers when the first pin is added,
	/// you can check whether <paramref name="oldPinCounter"/> is <c>0</c> and <paramref name="newPinCounter"/> greater than <c>0</c> in your custom implementation.
	/// </para>
	/// <para>
	/// When implementing this method, remember that it must work in conjunction with <see cref="RemovePin(ulong, ulong)"/>.
	/// </para>
	/// <para>
	/// There's no guarantee about when this method is called in relation to other threads pinning or unpinning this <see cref="NativeMemoryManagerBase"/>.
	/// Custom implementations must take care of thread-safetiness themselves if needed,
	/// especially in regards to the ordering of <see cref="AddPin(ulong, ulong)"/> and <see cref="RemovePin(ulong, ulong)"/> operations.
	/// </para>
	/// <para>
	/// The only guarantee given is that this method is only ever called immediately after the pin counter was successfully increased by a single pinning operation.
	/// </para>
	/// </remarks>
	protected virtual void AddPin(ulong oldPinCounter, ulong newPinCounter) { }

	/// <summary>
	/// Decreases the pin counter
	/// </summary>
	/// <param name="pinCounter">The current pin counter</param>
	/// <returns>The new pin counter</returns>
	/// <remarks>
	/// <para>
	/// Override this method to implement custom pin counter decreasing step logic.
	/// You can even just ignore pinning altogether and always return the given <paramref name="pinCounter"/>.
	/// </para>
	/// <para>
	/// Notice that custom implementations of this method should ensure that the returned value is <em>less than or equal to</em> the given <paramref name="pinCounter"/>.
	/// Also, when implementing this method, remember that the pin counter stepping must work in conjunction with <see cref="IncreasePinCounter(ulong)"/>.
	/// Furthermore, there's no underflow preventing logic in the consumers of this method, so custom implementations should ensure that underflow doesn't happen.
	/// </para>
	/// </remarks>
	protected virtual ulong DecreasePinCounter(ulong pinCounter) => unchecked(pinCounter - 1);

	/// <remarks>
	/// <para>
	/// All of the <see cref="NativeMemory"/> and <see cref="NativeMemory{T}"/> instances, that are subsequently depending on this <see cref="NativeMemoryManagerBase"/>,
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
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	/// <summary>
	/// Disposes this <see cref="NativeMemoryManagerBase"/>
	/// </summary>
	/// <param name="disposing">A value indicating whether this method is called from <see cref="Dispose()">Dispose()</see> (<c><see langword="true"/></c>) or from the finalizer (<c><see langword="false"/></c>)</param>
	/// <seealso cref="Dispose()"/>
	protected virtual void Dispose(bool disposing) { }

	/// <summary>
	/// Increases the pin counter
	/// </summary>
	/// <param name="pinCounter">The current pin counter</param>
	/// <returns>The new pin counter</returns>
	/// <remarks>
	/// <para>
	/// Override this method to implement custom pin counter increasing step logic.
	/// You can even just ignore pinning altogether and always return the given <paramref name="pinCounter"/>.
	/// </para>
	/// <para>
	/// Notice that custom implementations of this method should ensure that the returned value is <em>greater than or equal to</em> the given <paramref name="pinCounter"/>.
	/// Also, when implementing this method, remember that the pin counter stepping must work in conjunction with <see cref="DecreasePinCounter(ulong)"/>.
	/// Furthermore, there's no overflow preventing logic in the consumers of this method, so custom implementations should ensure that overflow doesn't happen.
	/// </para>
	/// </remarks>
	protected virtual ulong IncreasePinCounter(ulong pinCounter) => unchecked(pinCounter + 1);

	/// <summary>
	/// Pins this <see cref="NativeMemoryManagerBase"/>
	/// </summary>
	/// <returns>A <see cref="NativeMemoryPin">pin</see> pinning this <see cref="NativeMemoryManagerBase"/></returns>
	/// <remarks>
	/// <para>
	/// A pinned <see cref="NativeMemoryManager"/> can't be <see cref="Dispose()">disposed</see> or be used in operations like <see cref="NativeMemory.TryRealloc(ref NativeMemoryManager?, nuint)"/>.
	/// </para>
	/// </remarks>
	public NativeMemoryPin Pin() => new(this);

	/// <summary>
	/// Pins this <see cref="NativeMemoryManagerBase"/> once
	/// </summary>
	/// <returns>The new pin counter</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	protected internal ulong PinOnce()
	{
		while (Interlocked.Read(ref mPinCounter) is var pinCounter
			&& pinCounter is < ulong.MaxValue)
		{
			var newPinCounter = IncreasePinCounter(pinCounter);

			if (newPinCounter == pinCounter)
			{
				return newPinCounter;
			}

			if (Interlocked.CompareExchange(ref mPinCounter, newPinCounter, pinCounter) == pinCounter)
			{
				AddPin(pinCounter, newPinCounter);

				return newPinCounter;
			}
		}

		return ulong.MaxValue;
	}

	/// <summary>
	/// Removes a "pin" to this <see cref="NativeMemoryManagerBase"/>
	/// </summary>
	/// <param name="oldPinCounter">The pin counter before it was decreased</param>
	/// <param name="newPinCounter">The current pin counter after it was decreased</param>
	/// <remarks>
	/// <para>
	/// This method is called each time the pin counter was successfully decreased.
	/// You can override this method to implement custom logic that should be executed each time a "pin" is removed from this <see cref="NativeMemoryManagerBase"/>.
	/// </para>
	/// <para>
	/// If you want a more general pinning logic that only triggers when the last pin is removed,
	/// you can check whether <paramref name="oldPinCounter"/> is greater than <c>0</c> and <paramref name="newPinCounter"/> is <c>0</c> in your custom implementation.
	/// </para>
	/// <para>
	/// When implementing this method, remember that it must work in conjunction with <see cref="AddPin(ulong, ulong)"/>.
	/// </para>
	/// <para>
	/// There's no guarantee about when this method is called in relation to other threads pinning or unpinning this <see cref="NativeMemoryManagerBase"/>.
	/// Custom implementations must take care of thread-safetiness themselves if needed,
	/// especially in regards to the ordering of <see cref="AddPin(ulong, ulong)"/> and <see cref="RemovePin(ulong, ulong)"/> operations.
	/// </para>
	/// <para>
	/// The only guarantee given is that this method is only ever called immediately after the pin counter was successfully decreased by a single unpinning operation.
	/// </para>
	/// </remarks>
	protected virtual void RemovePin(ulong oldPinCounter, ulong newPinCounter) { }

	/// <summary>
	/// Unpins this <see cref="NativeMemoryManagerBase"/> once
	/// </summary>
	/// <returns>The new pin counter</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	protected internal ulong UnpinOnce()
	{
		while (Interlocked.Read(ref mPinCounter) is var pinCounter
			&& pinCounter is > 0)
		{
			var newPinCounter = DecreasePinCounter(pinCounter);

			if (newPinCounter == pinCounter)
			{
				return newPinCounter;
			}

			if (Interlocked.CompareExchange(ref mPinCounter, newPinCounter, pinCounter) == pinCounter)
			{
				RemovePin(pinCounter, newPinCounter);

				return newPinCounter;
			}
		}

		return 0;
	}
}
