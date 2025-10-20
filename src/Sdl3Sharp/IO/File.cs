using Sdl3Sharp.Utilities;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices.Marshalling;

namespace Sdl3Sharp.IO;

/// <summary>
/// Provides static methods to load data from files and save data into files
/// </summary>
public static partial class File
{
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

				data = new(dataPtr, dataSize, &NativeMemory.SDL_free);
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
}
