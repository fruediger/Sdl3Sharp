using Sdl3Sharp.Utilities;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace Sdl3Sharp.IO;

/// <summary>
/// Provides static methods to load data from files and save data into files
/// </summary>
public static partial class File
{
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

	/// <summary>
	/// Tries to load all the data from a specified file
	/// </summary>
	/// <param name="fileName">The file path to the file, where to read all available data from</param>
	/// <param name="data">A <see cref="NativeMemoryManager"/> managing native memory containing all available data from the specified file, when this method returns <c><see langword="true"/></c>; otherwise, <c><see langword="null"/></c></param>
	/// <returns><c><see langword="true"/></c>, if all available data from the specified file was succesfully read; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// The resulting <see cref="NativeMemoryManager"/> should be <see cref="NativeMemoryManager.Dispose()">disposed</see> if the memory it's managing is no longer needed. That also frees the allocated memory.
	/// </para>
	/// <para>
	/// This method is not threadsafe.
	/// </para>
	/// </remarks>
	public static bool TryLoad(string fileName, [NotNullWhen(true)] out NativeMemoryManager? data)
	{
		unsafe
		{
			var fileNameUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(fileName);
			try
			{
				nuint dataSize;

				var dataPtr = SDL_LoadFile(fileNameUtf8, &dataSize);

				if (dataPtr is null)
				{
					data = null;
					return false;
				}

				data = new(dataPtr, dataSize, &Utilities.NativeMemory.SDL_free);
				return true;
			}
			finally
			{
				Utf8StringMarshaller.Free(fileNameUtf8);
			}
		}
	}


	/// <summary>
	/// Tries to load all the data from a specified file asynchronously
	/// </summary>
	/// <param name="fileName">The file path to the file, where to read all available data from</param>
	/// <param name="queue">The <see cref="AsyncIOQueue"/> to add the asynchronous reading operation to</param>
	/// <param name="userdata">User-defined data to associate with the asynchronous reading operation. This will be provided as the <see cref="AsyncIOOutcome.Userdata"/> property's value of the <see cref="AsyncIOOutcome"/> returned by the operation.</param>
	/// <returns><c><see langword="true"/></c>, if the reading operation was initiated successfully; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method returns as quickly as possible; it does not wait for the reading operation to complete.
	/// On a successful return, this work will continue in the background.
	/// If the work begins, even a failure is asynchronous: a failing return value from this method only means the reading operation couldn't get initiated.
	/// </para>
	/// <para>
	/// This method will allocate the buffer to contain the file for you. Make sure to <see cref="AsyncIOOutcome.Dispose">dispose</see> the resulting <see cref="AsyncIOOutcome"/> to free that buffer when it's no longer needed.
	/// </para>
	/// <para>
	/// The newly created asynchronous reading operation will be added to the specified <paramref name="queue"/>.
	/// </para>
	/// </remarks>
	public static bool TryLoadAsync(string fileName, AsyncIOQueue queue, object? userdata = null)
	{
		unsafe
		{
			ValidateQueue(queue);

			var fileNameUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(fileName);
			try
			{
				var managed = new AsyncIOOutcome.Managed { Userdata = userdata };
				var gcHandle = GCHandle.Alloc(managed, GCHandleType.Normal);

				bool result = SDL_LoadFileAsync(fileNameUtf8, queue.Pointer, unchecked((void*)GCHandle.ToIntPtr(gcHandle)));

				if (!result)
				{
					gcHandle.Free();
				
					return false;
				}

				return true;
			}
			finally
			{
				Utf8StringMarshaller.Free(fileNameUtf8);
			}
		}
	}

	/// <summary>
	/// Tries to save all specified data into a specified file
	/// </summary>
	/// <param name="fileName">The file path to the file, where to write all available data into</param>
	/// <param name="data">The <see cref="ReadOnlyNativeMemory">memory buffer</see> containing all the data to be written into the specified file</param>
	/// <returns><c><see langword="true"/></c>, if the data from the specified <see cref="ReadOnlyNativeMemory">memory buffer</see> was succesfully written into the specified file; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// If <paramref name="data"/> is <see cref="ReadOnlyNativeMemory.IsValid">invalid</see>, no data will be written and this method returns <c><see langword="false"/></c>.
	/// </para>
	/// <para>
	/// This method is not threadsafe.
	/// </para>
	/// </remarks>
	public static bool TrySave(string fileName, ReadOnlyNativeMemory data)
	{
		unsafe
		{
			if (!data.IsValid)
			{
				return false;
			}

			var fileNameUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(fileName);
			try
			{
				return SDL_SaveFile(fileNameUtf8, data.RawPointer, data.Length);
			}
			finally
			{
				Utf8StringMarshaller.Free(fileNameUtf8);
			}
		}
	}

	/// <summary>
	/// Tries to save all specified data into a specified file
	/// </summary>
	/// <param name="fileName">The file path to the file, where to write all available data into</param>
	/// <param name="data">The <see cref="ReadOnlySpan{T}"/> containing all the data to be written into the specified file</param>
	/// <returns><c><see langword="true"/></c>, if all the data from the specified <see cref="ReadOnlySpan{T}"/> was succesfully written into the specified file; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method is not threadsafe.
	/// </para>
	/// </remarks>
	public static bool TrySave(string fileName, ReadOnlySpan<byte> data)
	{
		unsafe
		{
			var fileNameUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(fileName);
			try
			{
				fixed (byte* dataPtr = data)
				{
					return SDL_SaveFile(fileNameUtf8, dataPtr, unchecked((nuint)data.Length));
				}
			}
			finally
			{
				Utf8StringMarshaller.Free(fileNameUtf8);
			}
		}
	}

	/// <summary>
	/// Tries to save all specified data into a specified file
	/// </summary>
	/// <param name="fileName">The file path to the file, where to write all available data into</param>
	/// <param name="data">A pointer to the unmanaged memory containing all the data to be saved into the stream</param>
	/// <param name="size">The size, in bytes, of the data to be saved</param>
	/// <returns><c><see langword="true"></see></c> if all the data from the specified unmanaged memory was successfully written into the specified file; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// The unmanaged memory pointed to by <paramref name="data"/> must be safely dereferencable for at least <paramref name="size"/> bytes.
	/// </para>
	/// <para>
	/// This method is not threadsafe.
	/// </para>
	/// </remarks>
	public unsafe static bool TrySave(string fileName, void* data, nuint size)
	{
		unsafe
		{
			var fileNameUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(fileName);
			try
			{
				return SDL_SaveFile(fileNameUtf8, data, size);
			}
			finally
			{
				Utf8StringMarshaller.Free(fileNameUtf8);
			}
		}
	}

	/// <summary>
	/// Tries to load all the data from a specified file asynchronously
	/// </summary>
	/// <param name="fileName">The file path to the file, where to read all available data from</param>
	/// <param name="queue">The <see cref="AsyncIOQueue"/> to add the asynchronous reading operation to</param>
	/// <param name="userdata">A pointer to unmanaged user-defined data to associate with the asynchronous reading operation. This will be provided as the <see cref="AsyncIOOutcome.UnsafeUserdata"/> property's value of the <see cref="AsyncIOOutcome"/> returned by the operation.</param>
	/// <returns><c><see langword="true"/></c>, if the reading operation was initiated successfully; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method returns as quickly as possible; it does not wait for the reading operation to complete.
	/// On a successful return, this work will continue in the background.
	/// If the work begins, even a failure is asynchronous: a failing return value from this method only means the reading operation couldn't get initiated.
	/// </para>
	/// <para>
	/// This method will allocate the buffer to contain the file for you. Make sure to <see cref="AsyncIOOutcome.Dispose">dispose</see> the resulting <see cref="AsyncIOOutcome"/> to free that buffer when it's no longer needed.
	/// </para>
	/// <para>
	/// The unmanaged user-defined data pointed to by <paramref name="userdata"/> must remain valid until the <see cref="AsyncIOOutcome"/> of the reading operation is <see cref="AsyncIOOutcome.Dispose">disposed</see>.
	/// </para>
	/// <para>
	/// The newly created asynchronous reading operation will be added to the specified <paramref name="queue"/>.
	/// </para>
	/// </remarks>
	public unsafe static bool TryUnsafeLoadAsync(string fileName, AsyncIOQueue queue, void* userdata = null)
	{
		unsafe
		{
			ValidateQueue(queue);

			var fileNameUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(fileName);
			try
			{

				return SDL_LoadFileAsync(fileNameUtf8, queue.Pointer, userdata);
			}
			finally
			{
				Utf8StringMarshaller.Free(fileNameUtf8);
			}
		}
	}
}
