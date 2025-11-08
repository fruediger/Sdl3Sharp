using Sdl3Sharp.Utilities;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sdl3Sharp.IO;

/// <summary>
/// Represents the outcome of an asynchronous I/O operation
/// </summary>
public sealed partial class AsyncIOOutcome : IDisposable
{
	private SDL_AsyncIOOutcome mOutcome;

	/// <remarks>
	/// This constructor does <em>not</em> initialize internal data! Callers <em>must</em> initialize it using the <see cref="Outcome"/> reference property immediately after!
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	internal AsyncIOOutcome()
	{
		Unsafe.SkipInit(out mOutcome);
	}

	/// <inheritdoc/>
	~AsyncIOOutcome() => DisposeImpl();

	/// <summary>
	/// Gets the <see cref="IO.AsyncIO"/> that initiated the operation that resulted in this <see cref="AsyncIOOutcome"/>
	/// </summary>
	/// <value>
	/// The <see cref="IO.AsyncIO"/> that initiated the operation that resulted in this <see cref="AsyncIOOutcome"/>, or <c><see langword="null"/></c> if it cannot be determined
	/// </value>
	/// <remarks>
	/// <para>
	/// Please note that the associated <see cref="IO.AsyncIO"/> may have already been become <see cref="AsyncIO.IsValid">invalid</see> (for example, if it has been <see cref="AsyncIO.TryClose(bool, AsyncIOQueue, object?)">closed</see>) by the time this property is accessed.
	/// </para>
	/// </remarks>
	public AsyncIO? AsyncIO
	{
		get
		{
			unsafe
			{
				if (mOutcome.AsyncIO is null)
				{
					return null;
				}

				if (mOutcome.Userdata is not null
					&& GCHandle.FromIntPtr(unchecked((IntPtr)mOutcome.Userdata)) is { IsAllocated: true, Target: Managed managed })
				{
					if (managed.AsyncIO is { Pointer: var asyncIOPtr } asyncIO
						&& asyncIOPtr == mOutcome.AsyncIO)
					{
						return asyncIO;
					}

					return managed.AsyncIO = AsyncIO.GetOrCreate(mOutcome.AsyncIO);
				}

				return AsyncIO.GetOrCreate(mOutcome.AsyncIO);
			}
		}
	}

	/// <summary>
	/// Gets the buffer where data was read into or written from
	/// </summary>
	/// <value>
	/// The buffer where data was read into or written from
	/// </value>
	/// <remarks>
	/// <para>
	/// Please not that the returned <see cref="ReadOnlyNativeMemory">memory buffer</see> is just referencing the originally provided buffer. That means that operations like <see cref="ReadOnlyNativeMemory.Pin">pinning</see> it wont actually pin the original buffer.
	/// If the original buffer was provided as <see cref="Utilities.NativeMemory"/>/<see cref="ReadOnlyNativeMemory"/> or as <see cref="Memory{T}"/>/<see cref="ReadOnlyMemory{T}"/>,
	/// then you can be sure that the original buffer will stay pinned for the lifetime of this <see cref="AsyncIOOutcome"/> instance.
	/// </para>
	/// </remarks>
	public ReadOnlyNativeMemory Buffer
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		get
		{
			unsafe
			{
				return new Utilities.NativeMemory(mOutcome.Buffer, unchecked((nuint)mOutcome.BytesTransferred)); // this cast is safe, since the 'SDL_AsyncIOOutcome' came from a call to 'SDL_GetAsyncIOResult' on the same platform
			}
		}
	}

	/// <summary>
	/// Gets the number of bytes that were <em>requested</em> to be read or written
	/// </summary>
	/// <value>
	/// The number of bytes that were <em>requested</em> to be read or written
	/// </value>
	public ulong BytesRequested { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mOutcome.BytesRequested; }

	/// <summary>
	/// Gets the number of bytes that were <em>actually</em> read or written
	/// </summary>
	/// <value>
	/// The number of bytes that were <em>actually</em> read or written
	/// </value>
	/// <remarks>
	/// <para>
	/// The value of the property is also the value of the <see cref="ReadOnlyNativeMemory.Length"/> property of the <see cref="Buffer"/>.
	/// </para>
	/// </remarks>
	public ulong BytesTransferred { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mOutcome.BytesTransferred; }

	/// <summary>
	/// Gets the offset within underlying data stream where the operation started
	/// </summary>
	/// <value>
	/// The offset within underlying data stream where the operation started
	/// </value>
	public ulong Offset { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mOutcome.Offset; }

	internal ref SDL_AsyncIOOutcome Outcome { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => ref mOutcome; }

	/// <summary>
	/// Gets the result of the asynchronous I/O operation
	/// </summary>
	/// <value>
	/// The result of the asynchronous I/O operation
	/// </value>
	public AsyncIOResult Result { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mOutcome.Result; }

	/// <summary>
	/// Gets the type of the asynchronous I/O operation
	/// </summary>
	/// <value>
	/// The type of the asynchronous I/O operation
	/// </value>
	public AsyncIOTaskType Type { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mOutcome.Type; }

	/// <summary>
	/// Gets the user-defined data that was associated with the asynchronous I/O operation
	/// </summary>
	/// <value>
	/// The user-defined data that was associated with the asynchronous I/O operation
	/// </value>
	/// <remarks>
	/// <para>
	/// The value of this property will only be non-<c><see langword="null"/></c> if user-defined data was provided to one of the <em>safe</em> operations on the initiating <see cref="IO.AsyncIO"/> (for example, via the <c>userdata</c> parameter of <see cref="AsyncIO.TryRead(Utilities.NativeMemory, ulong, AsyncIOQueue, object?)"/>).
	/// If the asynchronous I/O operation was initiated via one of the <em>unsafe</em> operations (for example, via <see cref="AsyncIO.TryUnsafeRead(void*, ulong, ulong, AsyncIOQueue, void*)"/>) or if it was initiated externally, use the <see cref="UnsafeUserdata"/> property to access a raw user-defined data pointer instead.
	/// </para>
	/// </remarks>
	public object? Userdata
	{
		get
		{
			unsafe
			{
				if (mOutcome.Userdata is not null && GCHandle.FromIntPtr(unchecked((IntPtr)mOutcome.Userdata)) is { IsAllocated: true, Target: Managed managed })
				{
					return managed.Userdata;
				}

				return null;
			}
		}
	}

	/// <summary>
	/// Gets a raw pointer to the underlying unmanaged asynchronous I/O structure that initiated the operation that resulted in this <see cref="AsyncIOOutcome"/>
	/// </summary>
	/// <value>
	/// A raw pointer to the underlying unmanaged asynchronous I/O structure that initiated the operation that resulted in this <see cref="AsyncIOOutcome"/>
	/// </value>
	/// <remarks>
	/// <para>
	/// In most cases, you wont need to use this property. <see cref="AsyncIO"/> offers a safe and managed way to access the initiating <see cref="IO.AsyncIO"/>.
	/// </para>
	/// </remarks>
	public unsafe void* UnsafeAsyncIO { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mOutcome.AsyncIO; }

	/// <summary>
	/// Gets a raw user-defined data pointer that was associated with the asynchronous I/O operation
	/// </summary>
	/// <value>
	/// A raw user-defined data pointer that was associated with the asynchronous I/O operation
	/// </value>
	/// <remarks>
	/// <para>
	/// Use this property to access a raw user-defined data pointer if the asynchronous I/O operation was initiated via one of the <em>unsafe</em> operations on the initiating <see cref="AsyncIO"/> (for example, via the <c>userdata</c> parameter of <see cref="AsyncIO.TryUnsafeRead(void*, ulong, ulong, AsyncIOQueue, void*)"/>), or if it was initiated externally.
	/// If the asynchronous I/O operation was initiated via one of the <em>safe</em> operations (for example, via <see cref="AsyncIO.TryRead(Utilities.NativeMemory, ulong, AsyncIOQueue, object?)"/>), you might want to use the <see cref="Userdata"/> property to get the managed user-defined data in a safe way instead.
	/// </para>
	/// </remarks>
	public unsafe void* UnsafeUserdata { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mOutcome.Userdata; }

	/// <inheritdoc/>
	public void Dispose()
	{
		DisposeImpl();
		GC.SuppressFinalize(this);
	}

	private void DisposeImpl()
	{
		unsafe
		{
			if (mOutcome.Userdata is not null && GCHandle.FromIntPtr(unchecked((IntPtr)mOutcome.Userdata)) is { IsAllocated: true, Target: Managed managed } gcHandle)
			{
				managed.AsyncIO = null!;

				if (managed.NativeMemoryPin is not null)
				{
					managed.NativeMemoryPin.Dispose();
					managed.NativeMemoryPin = null;
				}

				if (managed.MemoryHandle.Pointer is not null)
				{
					managed.MemoryHandle.Dispose();
					managed.MemoryHandle = default;
				}

				gcHandle.Free();
			}

			Utilities.NativeMemory.SDL_free(mOutcome.Buffer);

			mOutcome = default;
		}
	}
}
