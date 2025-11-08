using Sdl3Sharp.Internal.Interop;
using Sdl3Sharp.SourceGeneration;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
using unsafe SDL_EnumerateDirectoryCallback = delegate* unmanaged[Cdecl]<void*, byte*, byte*, Sdl3Sharp.IO.EnumerationResult>;
using unsafe SDL_StorageInterface_close = delegate* unmanaged[Cdecl]<void*, Sdl3Sharp.Internal.Interop.CBool>;
using unsafe SDL_StorageInterface_copy = delegate* unmanaged[Cdecl]<void*, byte*, byte*, Sdl3Sharp.Internal.Interop.CBool>;
using unsafe SDL_StorageInterface_enumerate = delegate* unmanaged[Cdecl]<void*, byte*, delegate* unmanaged[Cdecl]<void*, byte*, byte*, Sdl3Sharp.IO.EnumerationResult>, void*, Sdl3Sharp.Internal.Interop.CBool>;
using unsafe SDL_StorageInterface_info = delegate* unmanaged[Cdecl]<void*, byte*, Sdl3Sharp.IO.PathInfo*, Sdl3Sharp.Internal.Interop.CBool>;
using unsafe SDL_StorageInterface_mkdir = delegate* unmanaged[Cdecl]<void*, byte*, Sdl3Sharp.Internal.Interop.CBool>;
using unsafe SDL_StorageInterface_read_file = delegate* unmanaged[Cdecl]<void*, byte*, void*, ulong, Sdl3Sharp.Internal.Interop.CBool>;
using unsafe SDL_StorageInterface_ready = delegate* unmanaged[Cdecl]<void*, Sdl3Sharp.Internal.Interop.CBool>;
using unsafe SDL_StorageInterface_remove = delegate* unmanaged[Cdecl]<void*, byte*, Sdl3Sharp.Internal.Interop.CBool>;
using unsafe SDL_StorageInterface_rename = delegate* unmanaged[Cdecl]<void*, byte*, byte*, Sdl3Sharp.Internal.Interop.CBool>;
using unsafe SDL_StorageInterface_space_remaining = delegate* unmanaged[Cdecl]<void*, ulong>;
using unsafe SDL_StorageInterface_write_file = delegate* unmanaged[Cdecl]<void*, byte*, void*, ulong, Sdl3Sharp.Internal.Interop.CBool>;

namespace Sdl3Sharp.IO;

partial class Storage
{
	// Used for opaque pointers
	[StructLayout(LayoutKind.Sequential, Size = 0)]
	internal readonly struct SDL_Storage;

	internal unsafe readonly struct SDL_StorageInterface
	{
		public readonly uint Version;
		public readonly SDL_StorageInterface_close Close;
		public readonly SDL_StorageInterface_ready Ready;
		public readonly SDL_StorageInterface_enumerate Enumerate;
		public readonly SDL_StorageInterface_info Info;
		public readonly SDL_StorageInterface_read_file ReadFile;
		public readonly SDL_StorageInterface_write_file WriteFile;
		public readonly SDL_StorageInterface_mkdir MkDir;
		public readonly SDL_StorageInterface_remove Remove;
		public readonly SDL_StorageInterface_rename Rename;
		public readonly SDL_StorageInterface_copy Copy;
		public readonly SDL_StorageInterface_space_remaining SpaceRemaining;

		public SDL_StorageInterface(
			SDL_StorageInterface_close close,
			SDL_StorageInterface_ready ready,
			SDL_StorageInterface_enumerate enumerate,
			SDL_StorageInterface_info info,
			SDL_StorageInterface_read_file readFile,
			SDL_StorageInterface_write_file writeFile,
			SDL_StorageInterface_mkdir mkDir,
			SDL_StorageInterface_remove remove,
			SDL_StorageInterface_rename rename,
			SDL_StorageInterface_copy copy,
			SDL_StorageInterface_space_remaining spaceRemaining
		)
		{
			this = default; // make sure we're zeroed

			Version = unchecked((uint)Unsafe.SizeOf<SDL_StorageInterface>());
			Close = close;
			Ready = ready;
			Enumerate = enumerate;
			Info = info;
			ReadFile = readFile;
			WriteFile = writeFile;
			MkDir = mkDir;
			Remove = remove;
			Rename = rename;
			Copy = copy;
			SpaceRemaining = spaceRemaining;
		}

		public SDL_StorageInterface(Storage storage, out GCHandle storageHandle) : this(
			&CloseImpl,
			&ReadyImpl,
			&EnumerateImpl,
			&InfoImpl,
			&ReadFileImpl,
			&WriteFileImpl,
			&MkDirImpl,
			&RemoveImpl,
			&RenameImpl,
			&CopyImpl,
			&SpaceRemainingImpl
		)
		{
			storageHandle = GCHandle.Alloc(storage, GCHandleType.Normal);
		}

		[UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
		private unsafe static CBool CloseImpl(void* userdata)
		{
			if (userdata is not null && GCHandle.FromIntPtr(unchecked((IntPtr)userdata)) is { IsAllocated: true, Target: Storage storage })
			{
				return storage.CloseCore();
			}

			return false;
		}

		[UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
		private unsafe static CBool ReadyImpl(void* userdata)
		{
			if (userdata is not null && GCHandle.FromIntPtr(unchecked((IntPtr)userdata)) is { IsAllocated: true, Target: Storage storage })
			{
				return storage.IsReadyCore;
			}

			return false;
		}

		[UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
		private unsafe static CBool EnumerateImpl(void* userdata, byte* path, SDL_EnumerateDirectoryCallback callback, void* callback_userdata)
		{
			if (userdata is not null && GCHandle.FromIntPtr(unchecked((IntPtr)userdata)) is { IsAllocated: true, Target: Storage storage })
			{				
				if (Utf8StringMarshaller.ConvertToManaged(path) is string pathUtf16)
				{
					EnumerationResult managedCallback(string? directoryName, string entryName)
					{
						var directoryNameUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(directoryName);
						var entryNameUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(entryName);

						try
						{
							return callback(callback_userdata, directoryNameUtf8, entryNameUtf8);
						}
						finally
						{
							Utf8StringMarshaller.Free(entryNameUtf8);
							Utf8StringMarshaller.Free(directoryNameUtf8);
						}
					}

					return storage.EnumerateDirectoryCore(pathUtf16, managedCallback);
				}
			}

			return false;
		}

		[UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
		private unsafe static CBool InfoImpl(void* userdata, byte* path, PathInfo* info)
		{
			if (userdata is not null && GCHandle.FromIntPtr(unchecked((IntPtr)userdata)) is { IsAllocated: true, Target: Storage storage })
			{
				if (Utf8StringMarshaller.ConvertToManaged(path) is string pathUtf16)
				{
					return storage.GetPathInfoCore(pathUtf16, out Unsafe.AsRef<PathInfo>(info));
				}
			}

			return false;
		}

		[UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
		private unsafe static CBool ReadFileImpl(void* userdata, byte* path, void* destination, ulong length)
		{
			if (userdata is not null && GCHandle.FromIntPtr(unchecked((IntPtr)userdata)) is { IsAllocated: true, Target: Storage storage })
			{
				if (Utf8StringMarshaller.ConvertToManaged(path) is string pathUtf16)
				{
					return storage.ReadFileCore(pathUtf16, new(destination, unchecked((nuint)length))); // the cast should be safe as long as we don't cross platform boundaries
				}
			}

			return false;
		}

		[UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
		private unsafe static CBool WriteFileImpl(void* userdata, byte* path, void* source, ulong length)
		{
			if (userdata is not null && GCHandle.FromIntPtr(unchecked((IntPtr)userdata)) is { IsAllocated: true, Target: Storage storage })
			{
				if (Utf8StringMarshaller.ConvertToManaged(path) is string pathUtf16)
				{
					return storage.WriteFileCore(pathUtf16, new Utilities.NativeMemory(source, unchecked((nuint)length))); // the cast should be safe as long as we don't cross platform boundaries
				}
			}

			return false;
		}

		[UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
		private unsafe static CBool MkDirImpl(void* userdata, byte* path)
		{
			if (userdata is not null && GCHandle.FromIntPtr(unchecked((IntPtr)userdata)) is { IsAllocated: true, Target: Storage storage })
			{
				if (Utf8StringMarshaller.ConvertToManaged(path) is string pathUtf16)
				{
					return storage.CreateDirectoryCore(pathUtf16);
				}
			}

			return false;
		}

		[UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
		private unsafe static CBool RemoveImpl(void* userdata, byte* path)
		{
			if (userdata is not null && GCHandle.FromIntPtr(unchecked((IntPtr)userdata)) is { IsAllocated: true, Target: Storage storage })
			{
				if (Utf8StringMarshaller.ConvertToManaged(path) is string pathUtf16)
				{
					return storage.RemovePathCore(pathUtf16);
				}
			}

			return false;
		}

		[UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
		private unsafe static CBool RenameImpl(void* userdata, byte* oldpath, byte* newpath)
		{
			if (userdata is not null && GCHandle.FromIntPtr(unchecked((IntPtr)userdata)) is { IsAllocated: true, Target: Storage storage })
			{
				if (Utf8StringMarshaller.ConvertToManaged(oldpath) is string oldpathUtf16 && Utf8StringMarshaller.ConvertToManaged(newpath) is string newpathUtf16)
				{
					return storage.RenamePathCore(oldpathUtf16, newpathUtf16);
				}
			}

			return false;
		}

		[UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
		private unsafe static CBool CopyImpl(void* userdata, byte* oldpath, byte* newpath)
		{
			if (userdata is not null && GCHandle.FromIntPtr(unchecked((IntPtr)userdata)) is { IsAllocated: true, Target: Storage storage })
			{
				if (Utf8StringMarshaller.ConvertToManaged(oldpath) is string oldpathUtf16 && Utf8StringMarshaller.ConvertToManaged(newpath) is string newpathUtf16)
				{
					return storage.CopyFileCore(oldpathUtf16, newpathUtf16);
				}
			}

			return false;
		}

		[UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
		private unsafe static ulong SpaceRemainingImpl(void* userdata)
		{
			if (userdata is not null && GCHandle.FromIntPtr(unchecked((IntPtr)userdata)) is { IsAllocated: true, Target: Storage storage })
			{
				return storage.RemainingSpaceCore;
			}

			return 0;
		}
	}

	[UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
	private unsafe static EnumerationResult EnumerateDirectoryCallback(void* userdata, byte* dirname, byte* fname)
	{
		if (userdata is not null && GCHandle.FromIntPtr(unchecked((IntPtr)userdata)) is { IsAllocated: true, Target: EnumerateDirectoryCallback callback })
		{
			if (Utf8StringMarshaller.ConvertToManaged(dirname) is string dirnameUtf16 && Utf8StringMarshaller.ConvertToManaged(fname) is string fnameUtf16)
			{
				return callback(dirnameUtf16, fnameUtf16);
			}

			return EnumerationResult.Continue;
		}

		return EnumerationResult.Failure;
	}

	/// <summary>
	/// Closes and frees a storage container
	/// </summary>
	/// <param name="storage">A storage container to close</param>
	/// <returns>
	/// Returns true if the container was freed with no errors, false otherwise; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information.
	/// Even if the function returns an error, the container data will be freed; the error is only for informational purposes.
	/// </returns>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_CloseStorage">SDL_CloseStorage</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_CloseStorage(SDL_Storage *storage);

	/// <summary>
	/// Copy a file in a writable storage container
	/// </summary>
	/// <param name="storage">A storage container</param>
	/// <param name="oldpath">The old path</param>
	/// <param name="newpath">The new path</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_CopyStorageFile">SDL_CopyStorageFile</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_CopyStorageFile(SDL_Storage *storage, byte* oldpath, byte* newpath);

	/// <summary>
	/// Create a directory in a writable storage container
	/// </summary>
	/// <param name="storage">A storage container</param>
	/// <param name="path">The path of the directory to create</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_CreateStorageDirectory">SDL_CreateStorageDirectory</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_CreateStorageDirectory(SDL_Storage *storage, byte* path);

	/// <summary>
	/// Enumerate a directory in a storage container through a callback function
	/// </summary>
	/// <param name="storage">A storage container</param>
	/// <param name="path">The path of the directory to enumerate, or NULL for the root</param>
	/// <param name="callback">A function that is called for each entry in the directory</param>
	/// <param name="userdata">A pointer that is passed to <c><paramref name="callback"/></c></param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// This function provides every directory entry through an app-provided callback, called once for each directory entry,
	/// until all results have been provided or the callback returns either <see href="https://wiki.libsdl.org/SDL3/SDL_ENUM_SUCCESS">SDL_ENUM_SUCCESS</see> or <see href="https://wiki.libsdl.org/SDL3/SDL_ENUM_FAILURE">SDL_ENUM_FAILURE</see>.
	/// </para>
	/// <para>
	/// This will return false if there was a system problem in general, or if a callback returns <see href="https://wiki.libsdl.org/SDL3/SDL_ENUM_FAILURE">SDL_ENUM_FAILURE</see>.
	/// A successful return means a callback returned <see href="https://wiki.libsdl.org/SDL3/SDL_ENUM_SUCCESS">SDL_ENUM_SUCCESS</see> to halt enumeration, or all directory entries were enumerated.
	/// </para>
	/// <para>
	/// If <c><paramref name="path"/></c> is NULL, this is treated as a request to enumerate the root of the storage container's tree. An empty string also works for this.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_EnumerateStorageDirectory">SDL_EnumerateStorageDirectory</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_EnumerateStorageDirectory(SDL_Storage *storage, byte* path, SDL_EnumerateDirectoryCallback callback, void* userdata);

	/// <summary>
	/// Query the size of a file within a storage container
	/// </summary>
	/// <param name="storage">A storage container to query</param>
	/// <param name="path">The relative path of the file to query</param>
	/// <param name="length">A pointer to be filled with the file's length</param>
	/// <returns>Returns true if the file could be queried or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetStorageFileSize">SDL_GetStorageFileSize</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_GetStorageFileSize(SDL_Storage *storage, byte* path, ulong* length);

	/// <summary>
	/// Get information about a filesystem path in a storage container
	/// </summary>
	/// <param name="storage">A storage container</param>
	/// <param name="path">The path to query</param>
	/// <param name="info">A pointer filled in with information about the path, or NULL to check for the existence of a file</param>
	/// <returns>Returns true on success or false if the file doesn't exist, or another failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetStoragePathInfo">SDL_GetStoragePathInfo</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_GetStoragePathInfo(SDL_Storage *storage, byte* path, PathInfo* info);

	/// <summary>
	/// Queries the remaining space in a storage container
	/// </summary>
	/// <param name="storage">A storage container to query</param>
	/// <returns>Returns the amount of remaining space, in bytes</returns>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetStorageSpaceRemaining">SDL_GetStorageSpaceRemaining</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial ulong SDL_GetStorageSpaceRemaining(SDL_Storage *storage);

	/// <summary>
	/// Enumerate a directory tree, filtered by pattern, and return a list
	/// </summary>
	/// <param name="storage">A storage container</param>
	/// <param name="path">The path of the directory to enumerate, or NULL for the root</param>
	/// <param name="pattern">The pattern that files in the directory must match. Can be NULL.</param>
	/// <param name="flags"><c>SDL_GLOB_*</c> bitflags that affect this search.</param>
	/// <param name="count">On return, will be set to the number of items in the returned array. Can be NULL.</param>
	/// <returns>
	/// Returns an array of strings on success or NULL on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information.
	/// The caller should pass the returned pointer to <see href="https://wiki.libsdl.org/SDL3/SDL_free">SDL_free</see> when done with it.
	/// This is a single allocation that should be freed with <see href="https://wiki.libsdl.org/SDL3/SDL_free">SDL_free</see>() when it is no longer needed.
	/// </returns>
	/// <remarks>
	/// <para>
	/// Files are filtered out if they don't match the string in <c><paramref name="pattern"/></c>, which may contain wildcard characters <c>*</c> (match everything) and <c>?</c> (match one character). If pattern is NULL, no filtering is done and all results are returned.
	/// Subdirectories are permitted, and are specified with a path separator of '/'. Wildcard characters <c>*</c> and <c>?</c> never match a path separator.
	/// </para>
	/// <para>
	/// <c><paramref name="flags"/></c> may be set to <see href="https://wiki.libsdl.org/SDL3/SDL_GLOB_CASEINSENSITIVE">SDL_GLOB_CASEINSENSITIVE</see> to make the pattern matching case-insensitive.
	/// </para>
	/// <para>
	/// The returned array is always NULL-terminated, for your iterating convenience, but if <c><paramref name="count"/></c> is non-NULL, on return it will contain the number of items in the array, not counting the NULL terminator.
	/// </para>
	/// <para>
	/// If <c><paramref name="path"/></c> is NULL, this is treated as a request to enumerate the root of the storage container's tree. An empty string also works for this.
	/// </para>
	/// <para>
	/// It is safe to call this function from any thread, assuming the <c><paramref name="storage"/></c> object is thread-safe.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GlobStorageDirectory">SDL_GlobStorageDirectory</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial byte** SDL_GlobStorageDirectory(SDL_Storage *storage, byte* path, byte* pattern, GlobFlags flags, int* count);

	/// <summary>
	/// Opens up a container using a client-provided storage interface
	/// </summary>
	/// <param name="iface">The interface that implements this storage, initialized using <see href="https://wiki.libsdl.org/SDL3/SDL_INIT_INTERFACE">SDL_INIT_INTERFACE</see>()</param>
	/// <param name="userdata">The pointer that will be passed to the interface functions</param>
	/// <returns>Returns a storage container on success or NULL on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// Applications do not need to use this function unless they are providing their own <see href="https://wiki.libsdl.org/SDL3/SDL_Storage">SDL_Storage</see> implementation.
	/// If you just need an <see href="https://wiki.libsdl.org/SDL3/SDL_Storage">SDL_Storage</see>, you should use the built-in implementations in SDL,
	/// like <see href="https://wiki.libsdl.org/SDL3/SDL_OpenTitleStorage">SDL_OpenTitleStorage</see>() or <see href="https://wiki.libsdl.org/SDL3/SDL_OpenUserStorage">SDL_OpenUserStorage</see>(). 
	/// </para>
	/// <para>
	/// This function makes a copy of <c><paramref name="iface"/></c> and the caller does not need to keep it around after this call.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_OpenStorage">SDL_OpenStorage</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial SDL_Storage* SDL_OpenStorage(SDL_StorageInterface* iface, void* userdata);

	/// <summary>
	/// Synchronously read a file from a storage container into a client-provided buffer
	/// </summary>
	/// <param name="storage">A storage container to read from</param>
	/// <param name="path">The relative path of the file to read</param>
	/// <param name="destination">A client-provided buffer to read the file into</param>
	/// <param name="length">The length of the destination buffer</param>
	/// <returns>Returns true if the file was read or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// The value of <c><paramref name="length"/></c> must match the length of the file exactly; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetStorageFileSize">SDL_GetStorageFileSize</see>() to get this value.
	/// This behavior may be relaxed in a future release.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_ReadStorageFile">SDL_ReadStorageFile</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_ReadStorageFile(SDL_Storage* storage, byte* path, void* destination, ulong length);

	/// <summary>
	/// Remove a file or an empty directory in a writable storage container
	/// </summary>
	/// <param name="storage">A storage container</param>
	/// <param name="path">The path of the directory to enumerate</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_RemoveStoragePath">SDL_RemoveStoragePath</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_RemoveStoragePath(SDL_Storage* storage, byte* path);

	/// <summary>
	/// Rename a file or directory in a writable storage container
	/// </summary>
	/// <param name="storage">A storage container</param>
	/// <param name="oldpath">The old path</param>
	/// <param name="newpath">The new path</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_RenameStoragePath">SDL_RenameStoragePath</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_RenameStoragePath(SDL_Storage* storage, byte* oldpath, byte* newpath);

	/// <summary>
	/// Checks if the storage container is ready to use
	/// </summary>
	/// <param name="storage">A storage container to query</param>
	/// <returns>Returns true if the container is ready, false otherwise</returns>
	/// <remarks>
	/// <para>
	/// This function should be called in regular intervals until it returns true - however, it is not recommended to spinwait on this call, as the backend may depend on a synchronous message loop.
	/// You might instead poll this in your game's main loop while processing events and drawing a loading screen.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_StorageReady">SDL_StorageReady</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_StorageReady(SDL_Storage* storage);

	/// <summary>
	/// Synchronously write a file from client memory into a storage container
	/// </summary>
	/// <param name="storage">A storage container to write to</param>
	/// <param name="path">The relative path of the file to write</param>
	/// <param name="source">A client-provided buffer to write from</param>
	/// <param name="length">The length of the source buffer</param>
	/// <returns>Returns true if the file was written or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_WriteStorageFile">SDL_WriteStorageFile</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_WriteStorageFile(SDL_Storage* storage, byte* path, void* source, ulong length);
}
