using Sdl3Sharp.Utilities;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace Sdl3Sharp.IO;

/// <summary>
/// A base storage container for abstracting file storage operations
/// </summary>
/// <remarks>
/// <para>
/// All paths in the Storage API use Unix-style path separators (<c>'/'</c>). Using a different path separator will not work, even if the underlying platform would otherwise accept it.
/// This is to keep code using the Storage API portable between platforms and <see cref="Storage"/> implementations and simplify app code.
/// </para>
/// <para>
/// Paths with relative directories (<c>"."</c> and <c>".."</c>) are forbidden by the Storage API.
/// </para>
/// <para>
/// All valid Unicode strings (discounting the <c>'\0'</c> character and the <c>'/'</c> path separator character) are usable for filenames, however,
/// an underlying <see cref="Storage"/> implementation may not support particularly strange sequences and refuse to create files with those names.
/// </para>
/// </remarks>
public abstract partial class Storage : IDisposable
{
	private unsafe SDL_Storage* mStorage = null;
	private GCHandle mSelfHandle = default;

	/// <remarks>
	/// <para>
	/// This constructor does neiter <see langword="throw"/> nor fail otherwise
	/// </para>
	/// </remarks>
	private protected unsafe Storage(SDL_Storage* storage) => mStorage = storage;

	/// <summary>
	/// Creates a new <see cref="Storage"/> instance that uses a custom storage implementation provided by the derived class
	/// </summary>
	/// <exception cref="SdlException">The custom storage container instance could not be opened (check <see cref="Error.TryGet(out string?)"/> for more information)</exception>
	protected Storage()
	{
		unsafe
		{
			var iface = new SDL_StorageInterface(this, out mSelfHandle);
			mStorage = SDL_OpenStorage(&iface, unchecked((void*)GCHandle.ToIntPtr(mSelfHandle)));

			if (mStorage is null)
			{
				mSelfHandle.Free();
				mSelfHandle = default;

				failCouldNotCreateStorage();
			}
		}
			
		[DoesNotReturn]
		static void failCouldNotCreateStorage() => throw new SdlException("Could not open the custom storage");
	}

	/// <inheritdoc/>
	~Storage() => Dispose(disposing: false, close: true);

	/// <summary>
	/// Closes the storage container and release associated resources
	/// </summary>
	/// <returns><c><see langword="true"/></c> if the storage container was closed successfully; otherwise, <c><see langword="false"/></c></returns>
	/// <seealso cref="TryClose"/>
	protected abstract bool CloseCore();

	/// <summary>
	/// Gets a value indicating whether the storage container is ready to use
	/// </summary>
	/// <value>
	/// A value indicating whether the storage container is ready to use
	/// </value>
	/// <remarks>
	/// <para>
	/// Supporting this property is optional (just always have to value of this property be <c><see langword="true"/></c>).
	/// </para>
	/// </remarks>
	/// <seealso cref="IsReady"/>
	protected abstract bool IsReadyCore { get; }

	/// <summary>
	/// Enumerate entries in a directory within the storage container
	/// </summary>
	/// <param name="path">The path to the directory to enumerate, or the empty string (<c>""</c>) to request the root of the storage container</param>
	/// <param name="callback">The callback to invoke for each entry</param>
	/// <returns><c><see langword="true"/></c>, if the enumeration was successful; otherwise, <c><see langword="false"/></c></returns>	
	/// <remarks>
	/// <para>
	/// If <paramref name="path"/> is the empty string (<c>""</c>), this should be treated as a request to enumerate the root of the storage container.
	/// </para>
	/// <para>
	/// Supporting this method is optional for write-only storage containers (just always return <c><see langword="false"/></c>).
	/// </para>
	/// </remarks>
	/// <seealso cref="TryEnumerateDirectory(string?, EnumerateDirectoryCallback)"/>
	protected abstract bool EnumerateDirectoryCore(string path, EnumerateDirectoryCallback callback);

	/// <summary>
	/// Gets information about a path within the storage container
	/// </summary>
	/// <param name="path">The path to get information about</param>
	/// <param name="info">The information about <paramref name="path"/></param>
	/// <returns><c><see langword="true"/></c>, if the information about the path was retrieved successfully; otherwise, <c><see langword="false"/></c></returns>
	/// <remarks>
	/// <para>
	/// Supporting this method is optional for write-only storage containers (just always return <c><see langword="false"/></c>).
	/// </para>
	/// </remarks>
	/// <seealso cref="TryGetPathInfo(string, out PathInfo)"/>
	protected abstract bool GetPathInfoCore(string path, out PathInfo info);

	/// <summary>
	/// Reads all the data from a specified file within the storage container
	/// </summary>
	/// <param name="path">The path to the file, where to read all available data from</param>
	/// <param name="destination">The <see cref="Utilities.NativeMemory">memory buffer</see> to read data into</param>
	/// <returns><c><see langword="true"/></c> if the data was successfully read into the specified <see cref="Utilities.NativeMemory">memory buffer</see>; otherwise, <c><see langword="false"/></c></returns>
	/// <remarks>
	/// <para>
	/// Supporting this method is optional for write-only storage containers (just always return <c><see langword="false"/></c>).
	/// </para>
	/// </remarks>
	/// <seealso cref="TryReadFile(string, Utilities.NativeMemory)"/>
	protected abstract bool ReadFileCore(string path, Utilities.NativeMemory destination);

	/// <summary>
	/// Writes all specified data into a specified file within a writable storage container
	/// </summary> 
	/// <param name="path">The path to the file, where to write all available data into</param>
	/// <param name="source">The <see cref="ReadOnlyNativeMemory">memory buffer</see> containing all the data to be written into the file</param>
	/// <returns><c><see langword="true"/></c> if the data from the specified <see cref="ReadOnlyNativeMemory">memory buffer</see> was successfully written into the file; otherwise, <c><see langword="false"/></c></returns>
	/// <remarks>
	/// <para>
	/// Supporting this method is optional for read-only storage containers (just always return <c><see langword="false"/></c>).
	/// </para>
	/// </remarks>
	/// <seealso cref="TryWriteFile(string, ReadOnlyNativeMemory)"/>
	protected abstract bool WriteFileCore(string path, ReadOnlyNativeMemory source);

	/// <summary>
	/// Creates a directory at the specified path within a writable storage container
	/// </summary>
	/// <param name="path">The path to the directory to create</param>
	/// <returns><c><see langword="true"/></c>, if the directory was created successfully; otherwise, <c><see langword="false"/></c></returns>
	/// <remarks>
	/// <para>
	/// Supporting this method is optional for read-only storage containers (just always return <c><see langword="false"/></c>).
	/// </para>
	/// </remarks>
	/// <seealso cref="TryCreateDirectory(string)"/>
	protected abstract bool CreateDirectoryCore(string path);

	/// <summary>
	/// Removes a file or an empty directory at the specified path within a writable storage container
	/// </summary>
	/// <param name="path">The path to the file or directory to remove</param>
	/// <returns><c><see langword="true"/></c>, if the file or directory was removed successfully; otherwise, <c><see langword="false"/></c></returns>
	/// <remarks>
	/// <para>
	/// Supporting this method is optional for read-only storage containers (just always return <c><see langword="false"/></c>).
	/// </para>
	/// </remarks>
	/// <seealso cref="TryRemovePath(string)"/>
	protected abstract bool RemovePathCore(string path);

	/// <summary>
	/// Renames a file or directory from one path to another within a writable storage container
	/// </summary>
	/// <param name="oldPath">The old path to rename</param>
	/// <param name="newPath">The new path</param>
	/// <returns><c><see langword="true"/></c>, if the file or directory was renamed successfully; otherwise, <c><see langword="false"/></c></returns>
	/// <remarks>
	/// <para>
	/// Supporting this method is optional for read-only storage containers (just always return <c><see langword="false"/></c>).
	/// </para>
	/// </remarks>
	/// <seealso cref="TryRenamePath(string, string)"/>
	protected abstract bool RenamePathCore(string oldPath, string newPath);

	/// <summary>
	/// Copies a file from one path to another within a writable storage container
	/// </summary>
	/// <param name="oldPath">The original file path</param>
	/// <param name="newPath">The new file path</param>
	/// <returns><c><see langword="true"/></c>, if the file was copied successfully; otherwise, <c><see langword="false"/></c></returns>
	/// <remarks>
	/// <para>
	/// Supporting this method is optional for read-only storage containers (just always return <c><see langword="false"/></c>).
	/// </para>
	/// </remarks>
	/// <seealso cref="TryCopyFile(string, string)"/>
	protected abstract bool CopyFileCore(string oldPath, string newPath);

	/// <summary>
	/// Gets the remaining space, in bytes, in the storage container
	/// </summary>
	/// <value>
	/// The remaining space, in bytes, in the storage container
	/// </value>
	/// <remarks>
	/// <para>
	/// Supporting this property is optional for read-only storage containers (just always have to value of this property be <c>0</c>).
	/// </para>
	/// </remarks>
	protected abstract ulong RemainingSpaceCore { get; }

	/// <summary>
	/// Gets a value indicating whether the storage container is ready to use
	/// </summary>
	/// <value>
	/// A value indicating whether the storage container is ready to use
	/// </value>
	/// <remarks>
	/// <para>
	/// The value of this property should be examined in regular intervals until it becomes <c><see langword="true"/></c>.
	/// However, it is not recommended to spin-wait on examining this property, as the backend may depend on a synchronous message loop.
	/// You might instead poll the value of this property in your app's main loop while processing events and drawing some kind of loading indicator.
	/// </para>
	/// </remarks>
	public bool IsReady
	{
		get
		{
			unsafe
			{
				return SDL_StorageReady(mStorage);
			}
		}
	}

	/// <summary>
	/// Gets a value indicating whether the storage container is valid
	/// </summary>
	/// <value>
	/// A value indicating whether the storage container is valid
	/// </value>
	/// <remarks>
	/// <para>
	/// A storage container becomes invalid after it has been <see cref="TryClose">closed</see> or <see cref="Dispose()">disposed</see>.
	/// </para>
	/// </remarks>
	public bool IsValid { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get { unsafe { return mStorage is not null; } } }

	private protected unsafe SDL_Storage* Pointer { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mStorage; }

	/// <summary>
	/// Gets the remaining space, in bytes, in the storage container
	/// </summary>
	/// <value>
	/// The remaining space, in bytes, in the storage container
	/// </value>
	public ulong RemainingSpace
	{
		get
		{
			unsafe
			{
				return SDL_GetStorageSpaceRemaining(mStorage);
			}
		}
	}

	/// <summary>
	/// Gets a pointer to the underlying SDL <c>SDL_Storage</c> that the <see cref="Storage"/> represents
	/// </summary>
	/// <value>
	/// A pointer to the underlying SDL <c>SDL_Storage</c> that the <see cref="Storage"/> represents. This can be cast to a SDL <c>SDL_Storage*</c>.
	/// </value>
	public IntPtr SdlStorage { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get { unsafe { return unchecked((IntPtr)mStorage); } } }

	/// <inheritdoc/>
	public void Dispose()
	{
		Dispose(disposing: true, close: true);
		GC.SuppressFinalize(this);
	}

	/// <summary>
	/// Disposes the storage container
	/// </summary>
	/// <param name="disposing">A value indicating whether the call came from a call to <see cref="Dispose()"/> or from the finalizer</param>
	/// <param name="close">A value indicating whether the storage container should be <see cref="TryClose">closed</see></param>
	/// <seealso cref="TryClose"/>
	protected virtual void Dispose(bool disposing, bool close)
	{
		unsafe
		{
			if (mStorage is not null)
			{
				if (close)
				{
					SDL_CloseStorage(mStorage);
				}

				mStorage = null;
			}

			if (mSelfHandle.IsAllocated)
			{
				mSelfHandle.Free();
				mSelfHandle = default;
			}
		}
	}

	/// <summary>
	/// Enumerates entries for a specified directory path within the storage container
	/// </summary>
	/// <param name="path">The path to the directory to enumerate, or <c><see langword="null"/></c> or the empty string (<c>""</c>) to request the root of the storage container</param>
	/// <returns>A <see cref="DirectoryEnumerable"/> that can be used to enumerate the entries for the specified directory <paramref name="path"/></returns>
	/// <remarks> 
	/// <para>
	/// If <paramref name="path"/> is <c><see langword="null"/></c> or the empty string (<c>""</c>), this is treated as a request to enumerate the root of the storage container.
	/// </para>
	/// </remarks>
	/// <inheritdoc cref="DirectoryEnumerable"/>
	public DirectoryEnumerable EnumerateDirectory(string? path) => new(this, path);

	/// <summary>
	/// Determines whether a entry exists at a specified path within the storage container
	/// </summary>
	/// <param name="path">The path to the entry</param>
	/// <returns><c><see langword="true"/></c>, if the entry at <paramref name="path"/> exists; otherwise, <c><see langword="false"/></c></returns>
	public bool PathExists(string path)
	{
		unsafe
		{
			var pathUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(path);

			try
			{
				return SDL_GetStoragePathInfo(mStorage, pathUtf8, null);
			}
			finally
			{
				Utf8StringMarshaller.Free(pathUtf8);
			}
		}
	}

	/// <summary>
	/// Tries to close the storage container and releases associated resources
	/// </summary>
	/// <returns><c><see langword="true"/></c> if the storage container was closed without failure; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// Note: Even if this method returns <c><see langword="false"/></c>, the storage container is considered closed, its associated resources are released, and the storage container becomes <see cref="IsValid">invalid</see> afterwards.
	/// The return value is just for informational purposes and you can check <see cref="Error.TryGet(out string?)"/> for more information.
	/// </para>
	/// </remarks>
	public bool TryClose()
	{
		unsafe
		{
			bool result = SDL_CloseStorage(mStorage);

			mStorage = null;

			if (mSelfHandle.IsAllocated)
			{
				mSelfHandle.Free();
				mSelfHandle = default;
			}

			return result;
		}
	}

	/// <summary>
	/// Tries to copy a file from one path to another within a writable storage container
	/// </summary>
	/// <param name="oldPath">The original file path</param>
	/// <param name="newPath">The new file path</param>
	/// <returns><c><see langword="true"/></c>, if the file was copied successfully; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <seealso cref="FileSystem.TryCopyFile(string, string)"/>
	public bool TryCopyFile(string oldPath, string newPath)
	{
		unsafe
		{
			var oldPathUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(oldPath);
			var newPathUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(newPath);

			try
			{
				return SDL_CopyStorageFile(mStorage, oldPathUtf8, newPathUtf8);
			}
			finally
			{
				Utf8StringMarshaller.Free(newPathUtf8);
				Utf8StringMarshaller.Free(oldPathUtf8);
			}
		}
	}

	/// <summary>
	/// Tries to create a directory at the specified path within a writable storage container
	/// </summary>
	/// <param name="path">The path to the directory to create</param>
	/// <returns><c><see langword="true"/></c>, if the directory was created successfully; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <seealso cref="FileSystem.TryCreateDirectory(string)"/>
	public bool TryCreateDirectory(string path)
	{
		unsafe
		{
			var pathUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(path);

			try
			{
				return SDL_CreateStorageDirectory(mStorage, pathUtf8);
			}
			finally
			{
				Utf8StringMarshaller.Free(pathUtf8);
			}
		}
	}

	/// <summary>
	/// Tries to enumerate entries in a directory within the storage container
	/// </summary>
	/// <param name="path">The path to the directory to enumerate, or <c><see langword="null"/></c> or the empty string (<c>""</c>) to request the root of the storage container</param>
	/// <param name="callback">The callback to invoke for each entry</param>
	/// <returns><c><see langword="true"/></c>, if the enumeration was successful; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>	
	/// <remarks>
	/// <para>
	/// This method will continue enumerating entries until all entries have been enumerated, or until the provided <paramref name="callback"/> returns either <see cref="EnumerationResult.Success"/> or <see cref="EnumerationResult.Failure"/>.
	/// </para>
	/// <para>
	/// This method will return <c><see langword="false"/></c>, if there was an error in general, or if the provided <paramref name="callback"/> returned <see cref="EnumerationResult.Failure"/>.
	/// A return value of <c><see langword="true"/></c> indicates that all entries were enumerated successfully, or that the provided <paramref name="callback"/> returned <see cref="EnumerationResult.Success"/>.
	/// </para>
	/// <para>
	/// If <paramref name="path"/> is <c><see langword="null"/></c> or the empty string (<c>""</c>), this is treated as a request to enumerate the root of the storage container.
	/// </para>
	/// </remarks>
	/// <exception cref="ArgumentNullException"><paramref name="callback"/> is <c><see langword="null"/></c></exception>
	/// <seealso cref="FileSystem.TryEnumerateDirectory(string, EnumerateDirectoryCallback)"/>
	public bool TryEnumerateDirectory(string? path, EnumerateDirectoryCallback callback)
	{
		unsafe
		{
			if (callback is null)
			{
				failCallbackArgumentNull();
			}

			var pathUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(path);

			try
			{
				var gcHandle = GCHandle.Alloc(callback, GCHandleType.Normal);

				try
				{
					return SDL_EnumerateStorageDirectory(mStorage, pathUtf8, &EnumerateDirectoryCallback, unchecked((void*)GCHandle.ToIntPtr(gcHandle)));
				}
				finally
				{
					gcHandle.Free();
				}
			}
			finally
			{
				Utf8StringMarshaller.Free(pathUtf8);
			}
		}

		[DoesNotReturn]
		static void failCallbackArgumentNull() => throw new ArgumentNullException(nameof(callback));
	}

	/// <summary>
	/// Tries to get the size, in bytes, of a file within the storage container
	/// </summary>
	/// <param name="path">The path to the file to get the size of</param>
	/// <param name="length">The size of the file, in bytes</param>
	/// <returns><c><see langword="true"/></c>, if the size was retrieved successfully; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	public bool TryGetFileLength(string path, out ulong length)
	{
		unsafe
		{
			var pathUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(path);

			try
			{
				ulong tmp;

				bool result = SDL_GetStorageFileSize(mStorage, pathUtf8, &tmp);

				length = tmp;

				return result;
			}
			finally
			{
				Utf8StringMarshaller.Free(pathUtf8);
			}
		}
	}

	/// <summary>
	/// Tries to get information about a path within the storage container
	/// </summary>
	/// <param name="path">The path to get information about</param>
	/// <param name="info">The information about <paramref name="path"/></param>
	/// <returns><c><see langword="true"/></c>, if the information about the path was retrieved successfully; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <seealso cref="FileSystem.TryGetPathInfo(string, out PathInfo)"/>
	public bool TryGetPathInfo(string path, out PathInfo info)
	{
		unsafe
		{
			var pathUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(path);

			try
			{
				fixed (PathInfo* infoPtr = &info)
				{
					return SDL_GetStoragePathInfo(mStorage, pathUtf8, infoPtr);
				}
			}
			finally
			{
				Utf8StringMarshaller.Free(pathUtf8);
			}
		}
	}

	/// <summary>
	/// Tries to enumerate entries in a directory within the storage container, filtered by a pattern
	/// </summary>
	/// <param name="path">The path to the directory to enumerate, or <c><see langword="null"/></c> or the empty string (<c>""</c>) to request the root of the storage container</param>
	/// <param name="pattern">The pattern that entries must match. Can be <c><see langword="null"/></c> to not filter at all.</param>
	/// <param name="flags">Flags to modify the pattern matching behavior</param>
	/// <param name="matches">The matching entries, when this method returns <c><see langword="true"/></c>; otherwise, <c><see langword="null"/></c></param>
	/// <returns><c><see langword="true"/></c>, if the directory was successfully enumerated and entries were filtered; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// Entries are filtered out if they don't match the <paramref name="pattern"/>, which may contain wildcard characters <c>'*'</c> (match everything) and <c>'?'</c> (match one character).
	/// If <paramref name="pattern"/> is <c><see langword="null"/></c>, no filtering is done and all results are returned.
	/// Subdirectories are permitted, and are specified with a path separator of <c>'/'</c>. Wildcard characters <c>'*'</c> and <c>'?'</c> never match a path separator.
	/// </para>
	/// <para>
	/// <paramref name="flags"/> may be set to <see cref="GlobFlags.CaseInsensitive"/> to make the pattern matching case-insensitive.
	/// </para>
	/// <para>
	/// If <paramref name="path"/> is <c><see langword="null"/></c> or the empty string (<c>""</c>), this is treated as a request to enumerate the root of the storage container.
	/// </para>
	/// </remarks>
	/// <seealso cref="FileSystem.TryGlobDirectory(string, string?, GlobFlags, out string[])"/>
	public bool TryGlobDirectory(string? path, string? pattern, GlobFlags flags, [NotNullWhen(true)] out string[]? matches)
	{
		unsafe
		{
			var pathUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(path);
			var patternUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(pattern);

			int count;
			byte** matchesUtf8;

			try
			{
				matchesUtf8 = SDL_GlobStorageDirectory(mStorage, pathUtf8, patternUtf8, flags, &count);
			}
			finally
			{
				Utf8StringMarshaller.Free(patternUtf8);
				Utf8StringMarshaller.Free(pathUtf8);
			}

			if (matchesUtf8 is null)
			{
				matches = null;
				return false;
			}

			try
			{
				matches = GC.AllocateUninitializedArray<string>(count);

				var matchesUtf8Ptr = matchesUtf8;
				foreach (ref var match in matches.AsSpan())
				{
					match = Utf8StringMarshaller.ConvertToManaged(*matchesUtf8Ptr++);
				}

				return true;
			}
			finally
			{
				Utilities.NativeMemory.SDL_free(matchesUtf8);
			}
		}
	}

	/// <summary>
	/// Tries to synchronously read all the data from a specified file within the storage container
	/// </summary>
	/// <param name="path">The path to the file, where to read all available data from</param>
	/// <param name="destination">The <see cref="Utilities.NativeMemory">memory buffer</see> to read data into</param>
	/// <returns><c><see langword="true"/></c> if the data was successfully read into the specified <see cref="Utilities.NativeMemory">memory buffer</see>; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// The value of the <paramref name="destination"/>'s <see cref="Utilities.NativeMemory.Length"/> property must match the length of the file exactly.
	/// You can use <see cref="TryGetFileLength(string, out ulong)"/> to get the length of the file.
	/// This behavior may be relaxed in a future release.
	/// </para>
	/// </remarks>
	public bool TryReadFile(string path, Utilities.NativeMemory destination)
	{
		unsafe
		{
			if (!destination.IsValid)
			{
				return false;
			}

			var pathUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(path);

			try
			{
				return SDL_ReadStorageFile(mStorage, pathUtf8, destination.RawPointer, destination.Length);
			}
			finally
			{
				Utf8StringMarshaller.Free(pathUtf8);
			}
		}
	}

	/// <summary>
	/// Tries to read all the data from a specified file within the storage container
	/// </summary>
	/// <param name="path">The path to the file, where to read all available data from</param>
	/// <param name="destination">The <see cref="Span{T}"/> to read data into</param>
	/// <returns><c><see langword="true"/></c> if the data was successfully read into the specified <see cref="Span{T}"/>; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// The value of the <paramref name="destination"/>'s <see cref="Span{T}.Length"/> property must match the length of the file exactly.
	/// You can use <see cref="TryGetFileLength(string, out ulong)"/> to get the length of the file.
	/// This behavior may be relaxed in a future release.
	/// </para>
	/// </remarks>
	public bool TryReadFile(string path, Span<byte> destination)
	{
		unsafe
		{
			var pathUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(path);

			try
			{
				fixed (byte* ptr = destination)
				{
					return SDL_ReadStorageFile(mStorage, pathUtf8, ptr, unchecked((ulong)destination.Length));
				}
			}
			finally
			{
				Utf8StringMarshaller.Free(pathUtf8);
			}
		}
	}

	/// <summary>
	/// Tries to read all the data from a specified file within the storage container
	/// </summary>
	/// <param name="path">The path to the file, where to read all available data from</param>
	/// <param name="destination">A pointer to the unmananged memory to read data into</param>
	/// <param name="length">The length of the <paramref name="destination"/> buffer</param>
	/// <returns><c><see langword="true"/></c> if the data was successfully read into the specified unmanaged memory; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// The <paramref name="length"/> of the <paramref name="destination"/> buffer must match the length of the file exactly.
	/// You can use <see cref="TryGetFileLength(string, out ulong)"/> to get the length of the file.
	/// This behavior may be relaxed in a future release.
	/// </para>
	/// </remarks>
	public unsafe bool TryReadFile(string path, void* destination, ulong length)
	{
		var pathUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(path);

		try
		{
			return SDL_ReadStorageFile(mStorage, pathUtf8, destination, length);
		}
		finally
		{
			Utf8StringMarshaller.Free(pathUtf8);
		}
	}

	/// <summary>
	/// Tries to remove a file or an empty directory at the specified path within a writable storage container
	/// </summary>
	/// <param name="path">The path to the file or directory to remove</param>
	/// <returns><c><see langword="true"/></c>, if the file or directory was removed successfully; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <seealso cref="FileSystem.TryRemovePath(string)"/>
	public bool TryRemovePath(string path)
	{
		unsafe
		{
			var pathUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(path);

			try
			{
				return SDL_RemoveStoragePath(mStorage, pathUtf8);
			}
			finally
			{
				Utf8StringMarshaller.Free(pathUtf8);
			}
		}
	}

	/// <summary>
	/// Tries to rename a file or directory from one path to another within a writable storage container
	/// </summary>
	/// <param name="oldPath">The old path to rename</param>
	/// <param name="newPath">The new path</param>
	/// <returns><c><see langword="true"/></c>, if the file or directory was renamed successfully; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <seealso cref="FileSystem.TryRenamePath(string, string)"/>
	public bool TryRenamePath(string oldPath, string newPath)
	{
		unsafe
		{
			var oldPathUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(oldPath);
			var newPathUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(newPath);

			try
			{
				return SDL_RenameStoragePath(mStorage, oldPathUtf8, newPathUtf8);
			}
			finally
			{
				Utf8StringMarshaller.Free(newPathUtf8);
				Utf8StringMarshaller.Free(oldPathUtf8);
			}
		}
	}

	/// <summary>
	/// Tries to synchronously write all specified data into a specified file within a writable storage container
	/// </summary> 
	/// <param name="path">The path to the file, where to write all available data into</param>
	/// <param name="source">The <see cref="ReadOnlyNativeMemory">memory buffer</see> containing all the data to be written into the file</param>
	/// <returns><c><see langword="true"/></c> if the data from the specified <see cref="ReadOnlyNativeMemory">memory buffer</see> was successfully written into the file; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	public bool TryWriteFile(string path, ReadOnlyNativeMemory source)
	{
		unsafe
		{
			if (!source.IsValid)
			{
				return false;
			}

			var pathUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(path);

			try
			{
				return SDL_WriteStorageFile(mStorage, pathUtf8, source.RawPointer, source.Length);
			}
			finally
			{
				Utf8StringMarshaller.Free(pathUtf8);
			}
		}
	}
	
	/// <summary>
	/// Tries to synchronously write all specified data into a specified file within a writable storage container
	/// </summary> 
	/// <param name="path">The path to the file, where to write all available data into</param>
	/// <param name="source">The <see cref="Span{T}"/> containing all the data to be written into the file</param>
	/// <returns><c><see langword="true"/></c> if the data from the specified <see cref="Span{T}"/> was successfully written into the file; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	public bool TryWriteFile(string path, ReadOnlySpan<byte> source)
	{
		unsafe
		{
			var pathUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(path);

			try
			{
				fixed (byte* ptr = source)
				{
					return SDL_WriteStorageFile(mStorage, pathUtf8, ptr, unchecked((ulong)source.Length));
				}
			}
			finally
			{
				Utf8StringMarshaller.Free(pathUtf8);
			}
		}
	}

	/// <summary>
	/// Tries to synchronously write all specified data into a specified file within a writable storage container
	/// </summary> 
	/// <param name="path">The path to the file, where to write all available data into</param>
	/// <param name="source">A pointer to the unmanaged memory containing all the data to be written into the file</param>
	/// <param name="length">The length of the <paramref name="source"/> buffer</param>
	/// <returns><c><see langword="true"/></c> if the data from the specified unmanaged memory was successfully written into the file; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// The unmanaged memory pointed to by <paramref name="source"/> must be safely dereferencable for at least <paramref name="length"/> bytes.
	/// </para>
	/// </remarks>
	public unsafe bool TryWriteFile(string path, void* source, ulong length)
	{
		var pathUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(path);

		try
		{
			return SDL_WriteStorageFile(mStorage, pathUtf8, source, length);
		}
		finally
		{
			Utf8StringMarshaller.Free(pathUtf8);
		}
	}
}
