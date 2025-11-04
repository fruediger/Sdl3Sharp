using Sdl3Sharp.Utilities;
using System;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace Sdl3Sharp.IO;

/// <summary>
/// An asynchronous I/O stream handle
/// </summary>
public sealed partial class AsyncIO : IDisposable
{
	private interface IUnsafeConstructorDispatch;

	private static readonly ConcurrentDictionary<IntPtr, WeakReference<AsyncIO>> mKnownInstances = [];

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private unsafe static byte* Utf8Convert(string? str, out byte* strUtf8) => strUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(str);

	/// <exception cref="ArgumentException">The combination of <paramref name="access"/> and <paramref name="mode"/> is invalid</exception>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private static string ValidateMode(FileAccess access, FileMode mode)
	{
		if (!TryGetModeString(access, mode, out var modeString))
		{
			failArgumentsInvalid();
		}

		return modeString;

		[DoesNotReturn]
		static void failArgumentsInvalid() => throw new ArgumentException($"Invalid combination of values for the {nameof(access)} and {nameof(mode)} arguments");
	}

	/// <exception cref="ArgumentNullException"><paramref name="queue"/> is <c><see langword="null"/></c></exception>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private static void ValidateQueue([NotNull] AsyncIOQueue queue)
	{
		if (queue is null)
		{
			failQueueArgumentNull();
		}

		[DoesNotReturn]
		static void failQueueArgumentNull() => throw new ArgumentNullException(nameof(queue));
	}

	private unsafe SDL_AsyncIO* mPtr;

	private unsafe AsyncIO(SDL_AsyncIO* ptr, AsyncIOQueue? closeOnDisposeQueue, bool closeOnDisposeFlush, IUnsafeConstructorDispatch? _ = default)
	{
		mPtr = ptr;
		CloseOnDisposeQueue = closeOnDisposeQueue;
		CloseOnDisposeFlush = closeOnDisposeFlush;
	}

	private unsafe AsyncIO(SDL_AsyncIO* asyncIO, AsyncIOQueue? closeOnDisposeQueue, bool closeOnDisposeFlush) :
#pragma warning disable IDE0034 // Keep it that way for explicitness sake
		this(asyncIO, closeOnDisposeQueue, closeOnDisposeFlush, default(IUnsafeConstructorDispatch?))
#pragma warning restore
	{
		mKnownInstances.AddOrUpdate(unchecked((IntPtr)asyncIO), addRef, updateRef, this);

		static WeakReference<AsyncIO> addRef(IntPtr asyncIO, AsyncIO newAsyncIO) => new(newAsyncIO);

		static WeakReference<AsyncIO> updateRef(IntPtr asyncIO, WeakReference<AsyncIO> previousAsyncIORef, AsyncIO newAsyncIO)
		{
			if (previousAsyncIORef.TryGetTarget(out var previousAsyncIO))
			{
#pragma warning disable IDE0079
#pragma warning disable CA1816
				GC.SuppressFinalize(asyncIO);
#pragma warning restore CA1816
#pragma warning restore IDE0079
				previousAsyncIO.Dispose(forget: false);
			}

			previousAsyncIORef.SetTarget(newAsyncIO);

			return previousAsyncIORef;
		}
	}

	/// <exception cref="SdlException">The <see cref="AsyncIO"/> could not be created (check <see cref="Error.TryGet(out string?)"/> for more information)</exception>
	private unsafe AsyncIO(string fileName, string modeString, AsyncIOQueue? closeOnDisposeQueue, bool closeOnDisposeFlush, IUnsafeConstructorDispatch? _ = default) :
		this(SDL_AsyncIOFromFile(Utf8Convert(fileName, out var fileNameUtf8), Utf8Convert(modeString, out var modeStringUtf8)), closeOnDisposeQueue, closeOnDisposeFlush)
	{
		try
		{
			if (mPtr is null)
			{
				failCouldNotAsyncIO();
			}
		}
		finally
		{
			Utf8StringMarshaller.Free(modeStringUtf8);
			Utf8StringMarshaller.Free(fileNameUtf8);
		}

		[DoesNotReturn]
		static void failCouldNotAsyncIO() => throw new SdlException($"Could not create the {nameof(AsyncIO)}");
	}

	/// <summary>
	/// Creates a new <see cref="AsyncIO"/> instance for the specified file name and mode string
	/// </summary>
	/// <param name="fileName">The name of the file to open</param>
	/// <param name="modeString">The mode string to use when opening the file</param>
	/// <param name="closeOnDisposeQueue">
	/// An <see cref="AsyncIOQueue"/> to use if the newly created <see cref="AsyncIO"/> should be automatically <see cref="TryClose(bool, AsyncIOQueue, object?)">closed</see> when <see cref="Dispose()">disposed</see>.
	/// Set to <c><see langword="null"/></c> to disable automatic closing behavior.
	/// This parameter is reflected by the <see cref="CloseOnDisposeQueue"/> property. Changing the value of the <see cref="CloseOnDisposeQueue"/> property after construction also changes the automatic closing behavior.
	/// </param>
	/// <param name="closeOnDisposeFlush">
	/// A value indicating whether to flush the <see cref="AsyncIO"/> when automatically closing it.
	/// Has no effect if <paramref name="closeOnDisposeQueue"/> is <c><see langword="null"/></c>.
	/// This parameter is reflected by the <see cref="CloseOnDisposeFlush"/> property. Changing the value of the <see cref="CloseOnDisposeFlush"/> property after construction also changes the automatic closing behavior.
	/// </param>
	/// <remarks>
	/// <para>
	/// Opening a file is <em>not</em> an asynchronous operation under the assumption that doing so is generally a fast operation. Future reading and writing operations to the <see cref="AsyncIO"/> will be asynchronous though.
	/// </para>
	/// <para>
	/// The mode strings used by this method are roughly the same as the ones used by the C standard library's <c>fopen</c> function with some exceptions.
	/// You can use <see cref="TryGetModeString(FileAccess, FileMode, out string?)"/> to construct valid mode strings.
	/// There's no support for binary/text modes (<c>"b"</c>/<c>"t"</c>), as all asynchronous file I/O is performed in binary mode, and there's no support for appending mode (<c>"a"</c>), since you specify offsets explicitly when reading and writing.
	/// </para>
	/// </remarks>
	/// <inheritdoc cref="AsyncIO(string, string, AsyncIOQueue?, bool, IUnsafeConstructorDispatch?)"/>
	public AsyncIO(string fileName, string modeString, AsyncIOQueue? closeOnDisposeQueue = null, bool closeOnDisposeFlush = true) :
#pragma warning disable IDE0034 // Keep it that way for explicitness sake
		this(fileName, modeString, closeOnDisposeQueue, closeOnDisposeFlush, default(IUnsafeConstructorDispatch?))
#pragma warning restore
	{ }

	/// <summary>
	/// Creates a new <see cref="AsyncIO"/> instance for the specified file name, <see cref="FileAccess">access</see>, and <see cref="FileMode">mode</see>
	/// </summary>
	/// <param name="fileName">The name of the file to open</param>
	/// <param name="access">The <see cref="FileAccess"/> value representing the access mode</param>
	/// <param name="mode">The <see cref="FileMode"/> value representing the file mode</param>
	/// <param name="closeOnDisposeQueue">
	/// A value indicating whether to flush the <see cref="AsyncIO"/> when automatically closing it.
	/// Has no effect if <paramref name="closeOnDisposeQueue"/> is <c><see langword="null"/></c>.
	/// This parameter is reflected by the <see cref="CloseOnDisposeFlush"/> property. Changing the value of the <see cref="CloseOnDisposeFlush"/> property after construction also changes the automatic closing behavior.
	/// </param>
	/// <param name="closeOnDisposeFlush">
	/// A value indicating whether to flush the <see cref="AsyncIO"/> when automatically closing it.
	/// Has no effect if <paramref name="closeOnDisposeQueue"/> is <c><see langword="null"/></c>.
	/// This parameter is reflected by the <see cref="CloseOnDisposeFlush"/> property. Changing the value of the <see cref="CloseOnDisposeFlush"/> property after construction also changes the automatic closing behavior.
	/// </param>
	/// <remarks>
	/// <para>
	/// Opening a file is <em>not</em> an asynchronous operation under the assumption that doing so is generally a fast operation. Future reading and writing operations to the <see cref="AsyncIO"/> will be asynchronous though.
	/// </para>
	/// <para>
	/// There's no support for specifying a <see cref="FileKind"/> value, as all asynchronous file I/O is performed in binary mode, and there's no support for <see cref="FileMode.Append"/>, since you specify offsets explicitly when reading and writing.
	/// </para>
	/// </remarks>
	/// <inheritdoc cref="AsyncIO(string, string, AsyncIOQueue?, bool, IUnsafeConstructorDispatch?)"/>
	/// <inheritdoc cref="ValidateMode(FileAccess, FileMode)"/>
	public AsyncIO(string fileName, FileAccess access, FileMode mode, AsyncIOQueue? closeOnDisposeQueue = null, bool closeOnDisposeFlush = true) :
		this(fileName, ValidateMode(access, mode), closeOnDisposeQueue, closeOnDisposeFlush)
	{ }

	/// <inheritdoc/>
	~AsyncIO() => Dispose(forget: true);

	/// <summary>
	/// Gets or sets a value indicating whether to flush the <see cref="AsyncIO"/> when automatically closing it
	/// </summary>
	/// <value>
	/// A value indicating whether to flush the <see cref="AsyncIO"/> when automatically closing it
	/// </value>
	/// <remarks>
	/// <para>
	/// The value of this property has no effect if the value of the <see cref="CloseOnDisposeQueue"/> property is <c><see langword="null"/></c>.
	/// </para>
	/// </remarks>
	public bool CloseOnDisposeFlush { get; set; } = false;

	/// <summary>
	/// Gets or sets an <see cref="AsyncIOQueue"/> to use if the <see cref="AsyncIO"/> should be automatically <see cref="TryClose(bool, AsyncIOQueue, object?)">closed</see> when <see cref="Dispose()">disposed</see>
	/// </summary>
	/// <value>
	/// An <see cref="AsyncIOQueue"/> to use if the <see cref="AsyncIO"/> should be automatically <see cref="TryClose(bool, AsyncIOQueue, object?)">closed</see> when <see cref="Dispose()">disposed</see>
	/// </value>
	/// <remarks>
	/// <para>
	/// If the value of this property is <c><see langword="null"/></c>, the <see cref="AsyncIO"/> will not be automatically <see cref="TryClose(bool, AsyncIOQueue, object?)">closed</see> when <see cref="Dispose()">disposed</see>.
	/// Changing the value of this property will also change the automatic closing behavior.
	/// </para>
	/// </remarks>
	public AsyncIOQueue? CloseOnDisposeQueue { get; set; } = null;

	/// <summary>
	/// Gets or sets user-defined data to associate if the <see cref="AsyncIO"/> should be automatically <see cref="TryClose(bool, AsyncIOQueue, object?)">closed</see> when <see cref="Dispose()">disposed</see>
	/// </summary>
	/// <value>
	/// User-defined data to associate if the <see cref="AsyncIO"/> should be automatically <see cref="TryClose(bool, AsyncIOQueue, object?)">closed</see> when <see cref="Dispose()">disposed</see>
	/// </value>
	/// <remarks>
	/// <para>
	/// The value of this property will be provided as the <see cref="AsyncIOOutcome.Userdata"/> property's value of the <see cref="AsyncIOOutcome"/> returned by the automatic closing operation.
	/// </para>
	/// </remarks>
	public object? CloseOnDisposeUserdata { get; set; } = null;

	/// <summary>
	/// Gets a value indicating whether the <see cref="AsyncIO"/> is valid
	/// </summary>
	/// <value>
	/// A value indicating whether the <see cref="AsyncIO"/> is valid
	/// </value>
	/// <remarks>
	/// <para>
	/// An <see cref="AsyncIO"/> becomes invalid after it has been <see cref="TryClose(bool, AsyncIOQueue, object?)">closed</see>.
	/// </para>
	/// </remarks>
	public bool IsValid { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get { unsafe { return mPtr is not null; } } }

	/// <summary>
	/// Gets the length, in bytes, of the underlying data stream
	/// </summary>
	/// <value>
	/// The length, in bytes, of the underlying data stream, or <c>-1</c> if the length could not be determined (check <see cref="Error.TryGet(out string?)"/> for more information)
	/// </value>
	/// <remarks>
	/// <para>
	/// Obtaining the length of the underlying data stream is <em>not</em> an asynchronous operation under the assumption that it is non-blocking in most reasonable scenarios.
	/// </para>
	/// </remarks>
	public long Length { get { unsafe { return SDL_GetAsyncIOSize(mPtr); } } }

	internal unsafe SDL_AsyncIO* Pointer { get => mPtr; }

	/// <summary>
	/// Disposes the <see cref="AsyncIO"/> and potentially <see cref="TryClose(bool, AsyncIOQueue, object?)">closes</see> it asynchronously
	/// </summary>
	/// <remarks>
	/// <para>
	/// If the <see cref="CloseOnDisposeQueue"/> property is set to a non-<c><see langword="null"/></c> value, the <see cref="AsyncIO"/> will be automatically <see cref="TryClose(bool, AsyncIOQueue, object?)">closed</see> when this method is called.
	/// The automatic closing behavior of the <see cref="AsyncIO"/> (if any) will always just attempt a single try to initiate a closing operation.
	/// </para>
	/// </remarks>
	public void Dispose()
	{
		GC.SuppressFinalize(this);
		Dispose(forget: true);
	}

	private unsafe void Dispose(bool forget)
	{
		if (mPtr is not null)
		{
			if (CloseOnDisposeQueue is not null)
			{
				TryClose(CloseOnDisposeFlush, CloseOnDisposeQueue, CloseOnDisposeUserdata);

				CloseOnDisposeQueue = null;
				CloseOnDisposeUserdata = null;
			}

			if (forget)
			{
				mKnownInstances.TryRemove(unchecked((IntPtr)mPtr), out _);
			}

			mPtr = null;
		}
	}

	internal unsafe static AsyncIO GetOrCreate(SDL_AsyncIO* asyncIO, AsyncIOQueue? closeOnDisposeQueue = null, bool closeOnDisposeFlush = true)
	{
		var asyncIORef = mKnownInstances.GetOrAdd(unchecked((IntPtr)asyncIO), createRef, (closeOnDisposeQueue, closeOnDisposeFlush));

		if (!asyncIORef.TryGetTarget(out var result))
		{
			asyncIORef.SetTarget(result = create(asyncIO, closeOnDisposeQueue, closeOnDisposeFlush));
		}

		return result;

		static WeakReference<AsyncIO> createRef(IntPtr asyncIO, (AsyncIOQueue? closeOnDisposeQueue, bool closeOnDisposeFlush) arg) => new(create(unchecked((SDL_AsyncIO*)asyncIO), arg.closeOnDisposeQueue, arg.closeOnDisposeFlush));

		static AsyncIO create(SDL_AsyncIO* asyncIO, AsyncIOQueue? closeOnDisposeQueue, bool closeOnDisposeFlush) => new(asyncIO, closeOnDisposeQueue, closeOnDisposeFlush,
#pragma warning disable IDE0034 // Keep it that way for explicitness sake
			default(IUnsafeConstructorDispatch?)
#pragma warning restore
		);
	}

	/// <summary>
	/// Tries to asynchronously close the <see cref="AsyncIO"/>
	/// </summary>
	/// <param name="flush">A value indicating whether to flush the <see cref="AsyncIO"/> before closing it</param>
	/// <param name="queue">The <see cref="AsyncIOQueue"/> to add the asynchronous closing operation to</param>
	/// <param name="userdata">User-defined data to associate with the asynchronous closing operation. This will be provided as the <see cref="AsyncIOOutcome.Userdata"/> property's value of the <see cref="AsyncIOOutcome"/> returned by the operation.</param>
	/// <returns><c><see langword="true"/></c>, if the asynchronous closing operation was initiated successfully; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// Closing an <see cref="AsyncIO"/> is <em>also</em> an asynchronous operation.
	/// </para>
	/// <para>
	/// Closing a file that has been written to does not guarantee the data has made it to physical media; it may remain in the operating system's file cache, for later writing to disk.
	/// This means that a successfully-closed file can be lost if the system crashes or loses power in this small window.
	/// To prevent this, call this method with the <paramref name="flush"/> argument set to <c><see langword="true"/></c>.
	/// This will make the operation take longer, and perhaps increase system load in general, but a successful result guarantees that the data has made it to physical storage.
	/// Set the <paramref name="flush"/> argument to <c><see langword="false"/></c> for temporary files, caches, and unimportant data, and definitely set it to <c><see langword="true"/></c> for crucial irreplaceable files, like game saves.
	/// </para>
	/// <para>
	/// This method guarantees that the closing operation will happen <em>after</em> any other pending tasks to the <see cref="AsyncIO"/>.
	/// So it's safe to open a file, start several operations, close the <see cref="AsyncIO"/> immediately, then check for all results later.
	/// This method will not block until the tasks have completed.
	/// </para>
	/// <para>
	/// Once this method returns <c><see langword="true"/></c>, the <see cref="AsyncIO"/> is no longer valid, regardless of any future outcomes.
	/// Any completed operation might still point to this <see cref="AsyncIO"/> in their <see cref="AsyncIOOutcome"/>, in case it's used to track information, <em>but it should not be used again</em>.
	/// </para>
	/// <para>
	/// If this method returns <c><see langword="false"/></c>, the closing operation wasn't initiated at all, and it's safe to attempt to close again later.
	/// The automatic closing behavior of the <see cref="AsyncIO"/> (if any) will always just attempt a single try to initiate a closing operation.
	/// </para>
	/// <para>
	/// The newly created asynchronous closing operation will be added to the specified <paramref name="queue"/>.
	/// </para>
	/// </remarks>
	/// <inheritdoc cref="Validate"/>
	/// <inheritdoc cref="ValidateQueue(AsyncIOQueue)"/>
	public bool TryClose(bool flush, AsyncIOQueue queue, object? userdata = null)
	{
		unsafe
		{
			Validate();
			ValidateQueue(queue);

			var managed = new AsyncIOOutcome.Managed { AsyncIO = this, Userdata = userdata };
			var gcHandle = GCHandle.Alloc(managed, GCHandleType.Normal);

			bool result = SDL_CloseAsyncIO(mPtr, flush, queue.Pointer, unchecked((void*)GCHandle.ToIntPtr(gcHandle)));

			if (result)
			{
				mPtr = null;
				CloseOnDisposeQueue = null;
				CloseOnDisposeUserdata = null;

				return true;
			}
			else
			{
				managed.AsyncIO = null;

				gcHandle.Free();

				return false;
			}
		}
	}

	/// <summary>
	/// Tries to get the mode string for the specified <see cref="FileAccess">access</see> and <see cref="FileMode">mode</see>
	/// </summary>
	/// <param name="access">The <see cref="FileAccess"/> value representing the access mode</param>
	/// <param name="mode">The <see cref="FileMode"/> value representing the file mode</param>
	/// <param name="modeString">The resulting mode string, when this method returns <c><see langword="true"/></c>; otherwise, <c><see langword="null"/></c></param>
	/// <returns><c><see langword="true"/></c>, if the mode string was successfully retrieved; otherwise, <c><see langword="false"/></c></returns>
	/// <remarks>
	/// <para>
	/// A mode string constructed by this method is roughly the same as the ones used by the C standard library's <c>fopen</c> function with some exceptions.
	/// There's no support for appending mode (<c>"a"</c>), since you specify offsets explicitly when reading and writing in asynchronous file I/O.
	/// </para>
	/// </remarks>
	public static bool TryGetModeString(FileAccess access, FileMode mode, [NotNullWhen(true)] out string? modeString)
		=> (access, mode) switch
		{
			(FileAccess.None,      _)                  => modeString = "",
			(FileAccess.Read,      FileMode.Open)      => modeString = "r",
			(FileAccess.Write,     FileMode.Create)    => modeString = "w",		
			(FileAccess.Write,     FileMode.CreateNew) => modeString = "wx",
			(FileAccess.ReadWrite, FileMode.Open)      => modeString = "r+",
			(FileAccess.ReadWrite, FileMode.Create)    => modeString = "w+",
			(FileAccess.ReadWrite, FileMode.CreateNew) => modeString = "w+x",
			_ => modeString = null
		} is not null;

	/// <summary>
	/// Tries to asynchronously read data from the <see cref="AsyncIO"/>
	/// </summary>
	/// <param name="data">The <see cref="Utilities.NativeMemory">memory buffer</see> to read data into</param>
	/// <param name="offset">The offset within underlying data stream to start reading from</param>
	/// <param name="queue">The <see cref="AsyncIOQueue"/> to add the asynchronous reading operation to</param>
	/// <param name="userdata">User-defined data to associate with the asynchronous reading operation. This will be provided as the <see cref="AsyncIOOutcome.Userdata"/> property's value of the <see cref="AsyncIOOutcome"/> returned by the operation.</param>
	/// <returns><c><see langword="true"/></c>, if the reading operation was initiated successfully; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// The asynchronous reading operation might read less bytes than requested by <paramref name="data"/>'s <see cref="Utilities.NativeMemory.Length"/> property.
	/// </para>
	/// <para>
	/// This method returns as quickly as possible; it does not wait for the reading operation to complete.
	/// On a successful return, this work will continue in the background.
	/// If the work begins, even a failure is asynchronous: a failing return value from this method only means the reading operation couldn't get initiated.
	/// </para>
	/// <para>
	/// This method will <see cref="Utilities.NativeMemory.Pin">pin</see> the specified <paramref name="data"/> until the <see cref="AsyncIOOutcome"/> of the reading operation is <see cref="AsyncIOOutcome.Dispose">disposed</see>.
	/// </para>
	/// <para>
	/// The newly created asynchronous reading operation will be added to the specified <paramref name="queue"/>.
	/// </para>
	/// </remarks>
	/// <inheritdoc cref="Validate"/>
	/// <inheritdoc cref="ValidateQueue(AsyncIOQueue)"/>
	public bool TryRead(Utilities.NativeMemory data, ulong offset, AsyncIOQueue queue, object? userdata = null)
	{
		unsafe
		{	
			Validate();
			ValidateQueue(queue);

			if (!data.IsValid)
			{
				return false;
			}

			var pin = data.Pin();
			var managed = new AsyncIOOutcome.Managed { AsyncIO = this, NativeMemoryPin = pin, Userdata = userdata };
			var gcHandle = GCHandle.Alloc(managed, GCHandleType.Normal);

			bool result = SDL_ReadAsyncIO(mPtr, data.RawPointer, offset, data.Length, queue.Pointer, unchecked((void*)GCHandle.ToIntPtr(gcHandle)));

			if (!result)
			{
				managed.AsyncIO = null;

				if (managed.NativeMemoryPin is not null)
				{
					managed.NativeMemoryPin.Dispose();
					managed.NativeMemoryPin = null;
				}

				gcHandle.Free();
				
				return false;
			}

			return true;
		}
	}

	/// <summary>
	/// Tries to asynchronously read data from the <see cref="AsyncIO"/>
	/// </summary>
	/// <param name="data">The <see cref="Memory{T}"/> to read data into</param>
	/// <param name="offset">The offset within underlying data stream to start reading from</param>
	/// <param name="queue">The <see cref="AsyncIOQueue"/> to add the asynchronous reading operation to</param>
	/// <param name="userdata">User-defined data to associate with the asynchronous reading operation. This will be provided as the <see cref="AsyncIOOutcome.Userdata"/> property's value of the <see cref="AsyncIOOutcome"/> returned by the operation.</param>
	/// <returns><c><see langword="true"/></c>, if the reading operation was initiated successfully; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// The asynchronous reading operation might read less bytes than requested by <paramref name="data"/>'s <see cref="Memory{T}.Length"/> property.
	/// </para>
	/// <para>
	/// This method returns as quickly as possible; it does not wait for the reading operation to complete.
	/// On a successful return, this work will continue in the background.
	/// If the work begins, even a failure is asynchronous: a failing return value from this method only means the reading operation couldn't get initiated.
	/// </para>
	/// <para>
	/// This method will <see cref="Memory{T}.Pin">pin</see> the specified <paramref name="data"/> until the <see cref="AsyncIOOutcome"/> of the reading operation is <see cref="AsyncIOOutcome.Dispose">disposed</see>.
	/// </para>
	/// <para>
	/// The newly created asynchronous reading operation will be added to the specified <paramref name="queue"/>.
	/// </para>
	/// </remarks>
	/// <inheritdoc cref="Validate"/>
	/// <inheritdoc cref="ValidateQueue(AsyncIOQueue)"/>
	public bool TryRead(Memory<byte> data, ulong offset, AsyncIOQueue queue, object? userdata = null)
	{
		unsafe
		{
			Validate();
			ValidateQueue(queue);

			var memHandle = data.Pin();
			var managed = new AsyncIOOutcome.Managed { AsyncIO = this, MemoryHandle = memHandle, Userdata = userdata };
			var gcHandle = GCHandle.Alloc(managed, GCHandleType.Normal);

			bool result = SDL_ReadAsyncIO(mPtr, memHandle.Pointer, offset, unchecked((ulong)data.Length), queue.Pointer, unchecked((void*)GCHandle.ToIntPtr(gcHandle)));

			if (!result)
			{
				managed.AsyncIO = null;

				if (managed.MemoryHandle.Pointer is not null)
				{
					managed.MemoryHandle.Dispose();
					managed.MemoryHandle = default;
				}

				gcHandle.Free();
				
				return false;
			}

			return true;
		}
	}

	/// <summary>
	/// Tries to asynchronously read data from the <see cref="AsyncIO"/>
	/// </summary>
	/// <param name="data">A pointer to the unmananged memory to read data into</param>
	/// <param name="offset">The offset within underlying data stream to start reading from</param>
	/// <param name="size">The number of bytes to read</param>
	/// <param name="queue">The <see cref="AsyncIOQueue"/> to add the asynchronous reading operation to</param>
	/// <param name="userdata">User-defined data to associate with the asynchronous reading operation. This will be provided as the <see cref="AsyncIOOutcome.Userdata"/> property's value of the <see cref="AsyncIOOutcome"/> returned by the operation.</param>
	/// <returns><c><see langword="true"/></c>, if the reading operation was initiated successfully; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// The asynchronous reading operation might read less bytes than requested by <paramref name="size"/>.
	/// </para>
	/// <para>
	/// This method returns as quickly as possible; it does not wait for the reading operation to complete.
	/// On a successful return, this work will continue in the background.
	/// If the work begins, even a failure is asynchronous: a failing return value from this method only means the reading operation couldn't get initiated.
	/// </para>
	/// <para>
	/// The unmanaged memory pointed to by <paramref name="data"/> must be safely dereferencable for at least <paramref name="size"/> bytes and must remain valid until the <see cref="AsyncIOOutcome"/> of the reading operation is <see cref="AsyncIOOutcome.Dispose">disposed</see>.
	/// </para>
	/// <para>
	/// The newly created asynchronous reading operation will be added to the specified <paramref name="queue"/>.
	/// </para>
	/// </remarks>
	/// <inheritdoc cref="Validate"/>
	/// <inheritdoc cref="ValidateQueue(AsyncIOQueue)"/>
	public unsafe bool TryRead(void* data, ulong offset, ulong size, AsyncIOQueue queue, object? userdata = null)
	{
		Validate();
		ValidateQueue(queue);

		var managed = new AsyncIOOutcome.Managed { AsyncIO = this, Userdata = userdata };
		var gcHandle = GCHandle.Alloc(managed, GCHandleType.Normal);

		bool result = SDL_ReadAsyncIO(mPtr, data, offset, size, queue.Pointer, unchecked((void*)GCHandle.ToIntPtr(gcHandle)));

		if (!result)
		{
			managed.AsyncIO = null;

			gcHandle.Free();
				
			return false;
		}

		return true;
	}

	/// <summary>
	/// Tries to asynchronously close the <see cref="AsyncIO"/>
	/// </summary>
	/// <param name="flush">A value indicating whether to flush the <see cref="AsyncIO"/> before closing it</param>
	/// <param name="queue">The <see cref="AsyncIOQueue"/> to add the asynchronous closing operation to</param>
	/// <param name="userdata">A pointer to unmanaged user-defined data to associate with the asynchronous closing operation. This will be provided as the <see cref="AsyncIOOutcome.UnsafeUserdata"/> property's value of the <see cref="AsyncIOOutcome"/> returned by the operation.</param>
	/// <returns><c><see langword="true"/></c>, if the asynchronous closing operation was initiated successfully; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// Closing an <see cref="AsyncIO"/> is <em>also</em> an asynchronous operation.
	/// </para>
	/// <para>
	/// Closing a file that has been written to does not guarantee the data has made it to physical media; it may remain in the operating system's file cache, for later writing to disk.
	/// This means that a successfully-closed file can be lost if the system crashes or loses power in this small window.
	/// To prevent this, call this method with the <paramref name="flush"/> argument set to <c><see langword="true"/></c>.
	/// This will make the operation take longer, and perhaps increase system load in general, but a successful result guarantees that the data has made it to physical storage.
	/// Set the <paramref name="flush"/> argument to <c><see langword="false"/></c> for temporary files, caches, and unimportant data, and definitely set it to <c><see langword="true"/></c> for crucial irreplaceable files, like game saves.
	/// </para>
	/// <para>
	/// This method guarantees that the closing operation will happen <em>after</em> any other pending tasks to the <see cref="AsyncIO"/>.
	/// So it's safe to open a file, start several operations, close the <see cref="AsyncIO"/> immediately, then check for all results later.
	/// This method will not block until the tasks have completed.
	/// </para>
	/// <para>
	/// Once this method returns <c><see langword="true"/></c>, the <see cref="AsyncIO"/> is no longer valid, regardless of any future outcomes.
	/// Any completed operation might still point to this <see cref="AsyncIO"/> in their <see cref="AsyncIOOutcome"/>, in case it's used to track information, <em>but it should not be used again</em>.
	/// </para>
	/// <para>
	/// If this method returns <c><see langword="false"/></c>, the closing operation wasn't initiated at all, and it's safe to attempt to close again later.
	/// The automatic closing behavior of the <see cref="AsyncIO"/> (if any) will always just attempt a single try to initiate a closing operation.
	/// </para>
	/// <para>
	/// The unmanaged user-defined data pointed to by <paramref name="userdata"/> must remain valid until the <see cref="AsyncIOOutcome"/> of the reading operation is <see cref="AsyncIOOutcome.Dispose">disposed</see>.
	/// </para>
	/// <para>
	/// The newly created asynchronous closing operation will be added to the specified <paramref name="queue"/>.
	/// </para>
	/// </remarks>
	/// <inheritdoc cref="Validate"/>
	/// <inheritdoc cref="ValidateQueue(AsyncIOQueue)"/>
	public unsafe bool TryUnsafeClose(bool flush, AsyncIOQueue queue, void* userdata = null)
	{
		Validate();
		ValidateQueue(queue);

		return SDL_CloseAsyncIO(mPtr, flush, queue.Pointer, userdata);
	}

	/// <summary>
	/// Tries to asynchronously read data from the <see cref="AsyncIO"/>
	/// </summary>
	/// <param name="data">A pointer to the unmananged memory to read data into</param>
	/// <param name="offset">The offset within underlying data stream to start reading from</param>
	/// <param name="size">The number of bytes to read</param>
	/// <param name="queue">The <see cref="AsyncIOQueue"/> to add the asynchronous reading operation to</param>
	/// <param name="userdata">A pointer to unmanaged user-defined data to associate with the asynchronous reading operation. This will be provided as the <see cref="AsyncIOOutcome.UnsafeUserdata"/> property's value of the <see cref="AsyncIOOutcome"/> returned by the operation.</param>
	/// <returns><c><see langword="true"/></c>, if the reading operation was initiated successfully; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// The asynchronous reading operation might read less bytes than requested by <paramref name="size"/>.
	/// </para>
	/// <para>
	/// This method returns as quickly as possible; it does not wait for the reading operation to complete.
	/// On a successful return, this work will continue in the background.
	/// If the work begins, even a failure is asynchronous: a failing return value from this method only means the reading operation couldn't get initiated.
	/// </para>
	/// <para>
	/// The unmanaged memory pointed to by <paramref name="data"/> must be safely dereferencable for at least <paramref name="size"/> bytes and must remain valid until the <see cref="AsyncIOOutcome"/> of the reading operation is <see cref="AsyncIOOutcome.Dispose">disposed</see>.
	/// </para>
	/// <para>
	/// The unmanaged user-defined data pointed to by <paramref name="userdata"/> must remain valid until the <see cref="AsyncIOOutcome"/> of the reading operation is <see cref="AsyncIOOutcome.Dispose">disposed</see>.
	/// </para>
	/// <para>
	/// The newly created asynchronous reading operation will be added to the specified <paramref name="queue"/>.
	/// </para>
	/// </remarks>
	/// <inheritdoc cref="Validate"/>
	/// <inheritdoc cref="ValidateQueue(AsyncIOQueue)"/>
	public unsafe bool TryUnsafeRead(void* data, ulong offset, ulong size, AsyncIOQueue queue, void* userdata = null)
	{
		Validate();
		ValidateQueue(queue);

		return SDL_ReadAsyncIO(mPtr, data, offset, size, queue.Pointer, userdata);
	}

	/// <summary>
	/// Tries to asynchronously write data to the <see cref="AsyncIO"/>
	/// </summary>
	/// <param name="data">A pointer to the unmananged memory containing all the data to be written to the <see cref="AsyncIO"/></param>
	/// <param name="offset">The offset within underlying data stream to start writing to</param>
	/// <param name="size">The size, in bytes, of the data to be written</param>
	/// <param name="queue">The <see cref="AsyncIOQueue"/> to add the asynchronous writing operation to</param>
	/// <param name="userdata">A pointer to unmanaged user-defined data to associate with the asynchronous writing operation. This will be provided as the <see cref="AsyncIOOutcome.UnsafeUserdata"/> property's value of the <see cref="AsyncIOOutcome"/> returned by the operation.</param>
	/// <returns><c><see langword="true"/></c>, if the writing operation was initiated successfully; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// The asynchronous writing operation tries to write <paramref name="size"/> bytes from buffer pointed to by <paramref name="data"/> to the <see cref="AsyncIO"/>.
	/// </para>
	/// <para>
	/// This method returns as quickly as possible; it does not wait for the writing operation to complete.
	/// On a successful return, this work will continue in the background.
	/// If the work begins, even a failure is asynchronous: a failing return value from this method only means the writing operation couldn't get initiated.
	/// </para>
	/// <para>
	/// The unmanaged memory pointed to by <paramref name="data"/> must be safely dereferencable for at least <paramref name="size"/> bytes and must remain valid until the <see cref="AsyncIOOutcome"/> of the writing operation is <see cref="AsyncIOOutcome.Dispose">disposed</see>.
	/// </para>
	/// <para>
	/// The unmanaged user-defined data pointed to by <paramref name="userdata"/> must remain valid until the <see cref="AsyncIOOutcome"/> of the writing operation is <see cref="AsyncIOOutcome.Dispose">disposed</see>.
	/// </para>
	/// <para>
	/// The newly created asynchronous writing operation will be added to the specified <paramref name="queue"/>.
	/// </para>
	/// </remarks>
	/// <inheritdoc cref="Validate"/>
	/// <inheritdoc cref="ValidateQueue(AsyncIOQueue)"/>
	public unsafe bool TryUnsafeWrite(void* data, ulong offset, ulong size, AsyncIOQueue queue, void* userdata = null)
	{
		Validate();
		ValidateQueue(queue);

		return SDL_WriteAsyncIO(mPtr, data, offset, size, queue.Pointer, userdata);
	}

	/// <summary>
	/// Tries to asynchronously write data to the <see cref="AsyncIO"/>
	/// </summary>
	/// <param name="data">The <see cref="ReadOnlyNativeMemory">memory buffer</see> containing all the data to be written to the <see cref="AsyncIO"/></param>
	/// <param name="offset">The offset within underlying data stream to start writing to</param>
	/// <param name="queue">The <see cref="AsyncIOQueue"/> to add the asynchronous writing operation to</param>
	/// <param name="userdata">User-defined data to associate with the asynchronous writing operation. This will be provided as the <see cref="AsyncIOOutcome.Userdata"/> property's value of the <see cref="AsyncIOOutcome"/> returned by the operation.</param>
	/// <returns><c><see langword="true"/></c>, if the writing operation was initiated successfully; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// The asynchronous writing operation tries to write the whole <paramref name="data"/> buffer to the <see cref="AsyncIO"/>.
	/// </para>
	/// <para>
	/// This method returns as quickly as possible; it does not wait for the writing operation to complete.
	/// On a successful return, this work will continue in the background.
	/// If the work begins, even a failure is asynchronous: a failing return value from this method only means the writing operation couldn't get initiated.
	/// </para>
	/// <para>
	/// This method will <see cref="ReadOnlyNativeMemory.Pin">pin</see> the specified <paramref name="data"/> until the <see cref="AsyncIOOutcome"/> of the writing operation is <see cref="AsyncIOOutcome.Dispose">disposed</see>.
	/// </para>
	/// <para>
	/// The newly created asynchronous writing operation will be added to the specified <paramref name="queue"/>.
	/// </para>
	/// </remarks>
	/// <inheritdoc cref="Validate"/>
	/// <inheritdoc cref="ValidateQueue(AsyncIOQueue)"/>
	public bool TryWrite(ReadOnlyNativeMemory data, ulong offset, AsyncIOQueue queue, object? userdata = null)
	{
		unsafe
		{
			Validate();
			ValidateQueue(queue);

			if (!data.IsValid)
			{
				return false;
			}

			var managed = new AsyncIOOutcome.Managed { AsyncIO = this, NativeMemoryPin = data.Pin(), Userdata = userdata };
			var gcHandle = GCHandle.Alloc(managed, GCHandleType.Normal);

			bool result = SDL_WriteAsyncIO(mPtr, data.RawPointer, offset, data.Length, queue.Pointer, unchecked((void*)GCHandle.ToIntPtr(gcHandle)));

			if (!result)
			{
				managed.AsyncIO = null;

				if (managed.NativeMemoryPin is not null)
				{
					managed.NativeMemoryPin.Dispose();
					managed.NativeMemoryPin = null;
				}

				gcHandle.Free();
				
				return false;
			}

			return true;
		}
	}

	/// <summary>
	/// Tries to asynchronously write data to the <see cref="AsyncIO"/>
	/// </summary>
	/// <param name="data">The <see cref="ReadOnlyMemory{T}"/> containing all the data to be written to the <see cref="AsyncIO"/></param>
	/// <param name="offset">The offset within underlying data stream to start writing to</param>
	/// <param name="queue">The <see cref="AsyncIOQueue"/> to add the asynchronous writing operation to</param>
	/// <param name="userdata">User-defined data to associate with the asynchronous writing operation. This will be provided as the <see cref="AsyncIOOutcome.Userdata"/> property's value of the <see cref="AsyncIOOutcome"/> returned by the operation.</param>
	/// <returns><c><see langword="true"/></c>, if the writing operation was initiated successfully; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// The asynchronous writing operation tries to write the whole <paramref name="data"/> buffer to the <see cref="AsyncIO"/>.
	/// </para>
	/// <para>
	/// This method returns as quickly as possible; it does not wait for the writing operation to complete.
	/// On a successful return, this work will continue in the background.
	/// If the work begins, even a failure is asynchronous: a failing return value from this method only means the writing operation couldn't get initiated.
	/// </para>
	/// <para>
	/// This method will <see cref="ReadOnlyMemory{T}.Pin">pin</see> the specified <paramref name="data"/> until the <see cref="AsyncIOOutcome"/> of the writing operation is <see cref="AsyncIOOutcome.Dispose">disposed</see>.
	/// </para>
	/// <para>
	/// The newly created asynchronous writing operation will be added to the specified <paramref name="queue"/>.
	/// </para>
	/// </remarks>
	/// <inheritdoc cref="Validate"/>
	/// <inheritdoc cref="ValidateQueue(AsyncIOQueue)"/>
	public bool TryWrite(ReadOnlyMemory<byte> data, ulong offset, AsyncIOQueue queue, object? userdata = null)
	{
		unsafe
		{
			Validate();
			ValidateQueue(queue);

			var memHandle = data.Pin();
			var managed = new AsyncIOOutcome.Managed { AsyncIO = this, MemoryHandle = memHandle, Userdata = userdata };
			var gcHandle = GCHandle.Alloc(managed, GCHandleType.Normal);

			bool result = SDL_WriteAsyncIO(mPtr, memHandle.Pointer, offset, unchecked((ulong)data.Length), queue.Pointer, unchecked((void*)GCHandle.ToIntPtr(gcHandle)));

			if (!result)
			{
				managed.AsyncIO = null;

				if (managed.MemoryHandle.Pointer is not null)
				{
					managed.MemoryHandle.Dispose();
					managed.MemoryHandle = default;
				}

				gcHandle.Free();
				
				return false;
			}

			return true;
		}
	}

	/// <summary>
	/// Tries to asynchronously write data to the <see cref="AsyncIO"/>
	/// </summary>
	/// <param name="data">A pointer to the unmananged memory containing all the data to be written to the <see cref="AsyncIO"/></param>
	/// <param name="offset">The offset within underlying data stream to start writing to</param>
	/// <param name="size">The size, in bytes, of the data to be written</param>
	/// <param name="queue">The <see cref="AsyncIOQueue"/> to add the asynchronous writing operation to</param>
	/// <param name="userdata">User-defined data to associate with the asynchronous writing operation. This will be provided as the <see cref="AsyncIOOutcome.Userdata"/> property's value of the <see cref="AsyncIOOutcome"/> returned by the operation.</param>
	/// <returns><c><see langword="true"/></c>, if the writing operation was initiated successfully; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// The asynchronous writing operation tries to write <paramref name="size"/> bytes from buffer pointed to by <paramref name="data"/> to the <see cref="AsyncIO"/>.
	/// </para>
	/// <para>
	/// This method returns as quickly as possible; it does not wait for the writing operation to complete.
	/// On a successful return, this work will continue in the background.
	/// If the work begins, even a failure is asynchronous: a failing return value from this method only means the writing operation couldn't get initiated.
	/// </para>
	/// <para>
	/// The unmanaged memory pointed to by <paramref name="data"/> must be safely dereferencable for at least <paramref name="size"/> bytes and must remain valid until the <see cref="AsyncIOOutcome"/> of the writing operation is <see cref="AsyncIOOutcome.Dispose">disposed</see>.
	/// </para>
	/// <para>
	/// The newly created asynchronous writing operation will be added to the specified <paramref name="queue"/>.
	/// </para>
	/// </remarks>
	/// <inheritdoc cref="Validate"/>
	/// <inheritdoc cref="ValidateQueue(AsyncIOQueue)"/>
	public unsafe bool TryWrite(void* data, ulong offset, ulong size, AsyncIOQueue queue, object? userdata = null)
	{
		Validate();
		ValidateQueue(queue);

		var managed = new AsyncIOOutcome.Managed { AsyncIO = this, Userdata = userdata };
		var gcHandle = GCHandle.Alloc(managed, GCHandleType.Normal);

		bool result = SDL_WriteAsyncIO(mPtr, data, offset, size, queue.Pointer, unchecked((void*)GCHandle.ToIntPtr(gcHandle)));

		if (!result)
		{
			managed.AsyncIO = null;

			gcHandle.Free();
				
			return false;
		}

		return true;
	}

	/// <exception cref="InvalidOperationException">The <see cref="AsyncIO"/> is invalid</exception>
	[MemberNotNull(nameof(mPtr))]
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private unsafe void Validate()
	{
		if (mPtr is null)
		{
			failAsyncIOInvalid();
		}

		[DoesNotReturn]
		static void failAsyncIOInvalid() => throw new InvalidOperationException(message: $"{nameof(AsyncIO)} is invalid");
	}
}
