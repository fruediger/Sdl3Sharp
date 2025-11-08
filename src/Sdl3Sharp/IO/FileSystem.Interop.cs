using Sdl3Sharp.Internal.Interop;
using Sdl3Sharp.SourceGeneration;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
using unsafe SDL_EnumerateDirectoryCallback = delegate* unmanaged[Cdecl]<void*, byte*, byte*, Sdl3Sharp.IO.EnumerationResult>;

namespace Sdl3Sharp.IO;

partial class FileSystem
{
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
	/// Copy a file
	/// </summary>
	/// <param name="oldpath">The old path</param>
	/// <param name="newpath">The new path</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// If the file at <c><paramref name="newpath"/></c> already exists, it will be overwritten with the contents of the file at <c><paramref name="oldpath"/></c>.
	/// </para>
	/// <para>
	/// This function will block until the copy is complete, which might be a significant time for large files on slow disks.
	/// On some platforms, the copy can be handed off to the OS itself, but on others SDL might just open both paths, and read from one and write to the other.
	/// </para>
	/// <para>
	/// Note that this is not an atomic operation! If something tries to read from <c><paramref name="newpath"/></c> while the copy is in progress, it will see an incomplete copy of the data, and if the calling thread terminates (or the power goes out) during the copy, <c><paramref name="newpath"/></c>'s previous contents will be gone, replaced with an incomplete copy of the data.
	/// To avoid this risk, it is recommended that the app copy to a temporary file in the same directory as <c><paramref name="newpath"/></c>, and if the copy is successful, use <see href="https://wiki.libsdl.org/SDL3/SDL_RenamePath">SDL_RenamePath</see>() to replace <c><paramref name="newpath"/></c> with the temporary file.
	/// This will ensure that reads of <c><paramref name="newpath"/></c> will either see a complete copy of the data, or it will see the pre-copy state of <c><paramref name="newpath"/></c>.
	/// </para>
	/// <para>
	/// This function attempts to synchronize the newly-copied data to disk before returning, if the platform allows it, so that the renaming trick will not have a problem in a system crash or power failure, where the file could be renamed but the contents never made it from the system file cache to the physical disk.
	/// </para>
	/// <para>
	/// If the copy fails for any reason, the state of <c><paramref name="newpath"/></c> is undefined. It might be half a copy, it might be the untouched data of what was already there, or it might be a zero-byte file, etc.
	/// </para>
	/// <para>
	/// It is safe to call this function from any thread, but this operation is not atomic, so the app might need to protect access to specific paths from other threads if appropriate.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_CopyFile">SDL_CopyFile</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_CopyFile(byte* oldpath, byte* newpath);

	/// <summary>
	/// Create a directory, and any missing parent directories
	/// </summary>
	/// <param name="path">The path of the directory to create</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// This reports success if <c><paramref name="path"/></c> already exists as a directory.
	/// </para>
	/// <para>
	/// If parent directories are missing, it will also create them. Note that if this fails, it will not remove any parent directories it already made.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_CreateDirectory">SDL_CreateDirectory</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_CreateDirectory(byte* path);

	/// <summary>
	/// Enumerate a directory through a callback function
	/// </summary>
	/// <param name="path">The path of the directory to enumerate</param>
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
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_EnumerateDirectory">SDL_EnumerateDirectory</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_EnumerateDirectory(byte* path, SDL_EnumerateDirectoryCallback callback, void* userdata);

	/// <summary>
	/// Get the directory where the application was run from
	/// </summary>
	/// <returns>
	/// Returns an absolute path in UTF-8 encoding to the application data directory.
	/// NULL will be returned on error or when the platform doesn't implement this functionality, call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information.
	/// </returns>
	/// <remarks>
	/// <para>
	/// SDL caches the result of this call internally, but the first call to this function is not necessarily fast, so plan accordingly.
	/// </para>
	/// <para>
	/// <em>macOS and iOS Specific Functionality</em>: If the application is in a ".app" bundle, this function returns the Resource directory (e.g. MyApp.app/Contents/Resources/).
	/// This behaviour can be overridden by adding a property to the Info.plist file.
	/// Adding a string key with the name <see href="https://wiki.libsdl.org/SDL3/SDL_FILESYSTEM_BASE_DIR_TYPE">SDL_FILESYSTEM_BASE_DIR_TYPE</see> with a supported value will change the behaviour.
	/// </para>
	/// <para>
	/// Supported values for the <see href="https://wiki.libsdl.org/SDL3/SDL_FILESYSTEM_BASE_DIR_TYPE">SDL_FILESYSTEM_BASE_DIR_TYPE</see> property (Given an application in /Applications/SDLApp/MyApp.app):
	/// <list type="bullet">
	///		<item>
	///			<term><c>resource</c></term>
	///			<description>Bundle resource directory (the default). For example: <c>/Applications/SDLApp/MyApp.app/Contents/Resources</c></description>
	///		</item>
	///		<item>
	///			<term><c>bundle</c></term>
	///			<description>The Bundle directory. For example: <c>/Applications/SDLApp/MyApp.app/</c></description>
	///		</item>
	///		<item>
	///			<term><c>parent</c></term>
	///			<description>The containing directory of the bundle. For example: <c>/Applications/SDLApp/</c></description>
	///		</item>
	/// </list>
	/// </para>
	/// <para>
	/// <em>Nintendo 3DS Specific Functionality</em>: This function returns "romfs" directory of the application as it is uncommon to store resources outside the executable. As such it is not a writable directory.
	/// </para>
	/// <para>
	/// The returned path is guaranteed to end with a path separator ('\' on Windows, '/' on most other platforms).
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetBasePath">SDL_GetBasePath</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial byte* SDL_GetBasePath();

	/// <summary>
	/// Get what the system believes is the "current working directory"
	/// </summary>
	/// <returns>
	/// Returns a UTF-8 string of the current working directory in platform-dependent notation. NULL if there's a problem.
	/// This should be freed with <see href="https://wiki.libsdl.org/SDL3/SDL_free">SDL_free</see>() when it is no longer needed.
	/// </returns>
	/// <remarks>
	/// <para>
	/// For systems without a concept of a current working directory, this will still attempt to provide something reasonable.
	/// </para>
	/// <para>
	/// SDL does not provide a means to <em>change</em> the current working directory; for platforms without this concept, this would cause surprises with file access outside of SDL.
	/// </para>
	/// <para>
	/// The returned path is guaranteed to end with a path separator ('\' on Windows, '/' on most other platforms).
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetCurrentDirectory">SDL_GetCurrentDirectory</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial byte* SDL_GetCurrentDirectory();

	/// <summary>
	/// Get information about a filesystem path
	/// </summary>
	/// <param name="path">The path to query</param>
	/// <param name="info">A pointer filled in with information about the path, or NULL to check for the existence of a file</param>
	/// <returns>Returns true on success or false if the file doesn't exist, or another failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information.</returns>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetPathInfo">SDL_GetPathInfo</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_GetPathInfo(byte* path, PathInfo* info);

	/// <summary>
	/// Get the user-and-app-specific path where files can be written.
	/// </summary>
	/// <param name="org">The name of your organization</param>
	/// <param name="app">The name of your application</param>
	/// <returns>
	/// Returns a UTF-8 string of the user directory in platform-dependent notation. NULL if there's a problem (creating directory failed, etc.).
	/// This should be freed with <see href="https://wiki.libsdl.org/SDL3/SDL_free">SDL_free</see>() when it is no longer needed.
	/// </returns>
	/// <remarks>
	/// <para>
	/// Get the "pref dir". This is meant to be where users can write personal files (preferences and save games, etc) that are specific to your application. This directory is unique per user, per application.
	/// </para>
	/// <para>
	/// This function will decide the appropriate location in the native filesystem, create the directory if necessary, and return a string of the absolute path to the directory in UTF-8 encoding.
	/// </para>
	/// <para>
	/// On Windows, the string might look like: <c>C:\Users\bob\AppData\Roaming\My Company\My Program Name\</c>
	/// </para>
	/// <para>
	/// On Linux, the string might look like: <c>/home/bob/.local/share/My Program Name/</c>
	/// </para>
	/// <para>
	/// On macOS, the string might look like: <c>/Users/bob/Library/Application Support/My Program Name/</c>
	/// </para>
	/// <para>
	/// You should assume the path returned by this function is the only safe place to write files (and that <see href="https://wiki.libsdl.org/SDL3/SDL_GetBasePath">SDL_GetBasePath</see>(), while it might be writable, or even the parent of the returned path, isn't where you should be writing things).
	/// </para>
	/// <para>
	/// Both the org and app strings may become part of a directory name, so please follow these rules:
	/// <list type="bullet">
	/// <item><description>Try to use the same org string (including case-sensitivity) for all your applications that use this function</description></item>
	/// <item><description>Always use a unique app string for each one, and make sure it never changes for an app once you've decided on it</description></item>
	/// <item><description>Unicode characters are legal, as long as they are UTF-8 encoded, but...</description></item>
	/// <item><description>...only use letters, numbers, and spaces. Avoid punctuation like "Game Name 2: Bad Guy's Revenge!" ... "Game Name 2" is sufficient</description></item>
	/// </list>
	/// </para>
	/// <para>
	/// Due to historical mistakes, <c><paramref name="org"/></c> is allowed to be NULL or "". In such cases, SDL will omit the org subdirectory, including on platforms where it shouldn't, and including on platforms where this would make your app fail certification for an app store.
	/// New apps should definitely specify a real string for <c><paramref name="org"/></c>.
	/// </para>
	/// <para>
	/// The returned path is guaranteed to end with a path separator ('\' on Windows, '/' on most other platforms).
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetPrefPath">SDL_GetPrefPath</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial byte* SDL_GetPrefPath(byte* org, byte* app);

	/// <summary>
	/// Finds the most suitable user folder for a specific purpose
	/// </summary>
	/// <param name="folder">The type of folder to find</param>
	/// <returns>Returns either a null-terminated C string containing the full path to the folder, or NULL if an error happened</returns>
	/// <remarks>
	/// <para>
	/// Many OSes provide certain standard folders for certain purposes, such as storing pictures, music or videos for a certain user. This function gives the path for many of those special locations.
	/// </para>
	/// <para>
	/// This function is specifically for <em>user</em> folders, which are meant for the user to access and manage.
	/// For application-specific folders, meant to hold data for the application to manage, see <see href="https://wiki.libsdl.org/SDL3/SDL_GetBasePath">SDL_GetBasePath</see>() and <see href="https://wiki.libsdl.org/SDL3/SDL_GetPrefPath">SDL_GetPrefPath</see>().
	/// </para>
	/// <para>
	/// The returned path is guaranteed to end with a path separator ('\' on Windows, '/' on most other platforms).
	/// </para>
	/// <para>
	/// If NULL is returned, the error may be obtained with <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>().
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetUserFolder">SDL_GetUserFolder</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial byte* SDL_GetUserFolder(Folder folder);

	/// <summary>
	/// Enumerate a directory tree, filtered by pattern, and return a list
	/// </summary>
	/// <param name="path">The path of the directory to enumerate</param>
	/// <param name="pattern">The pattern that files in the directory must match. Can be NULL.</param>
	/// <param name="flags"><c>SDL_GLOB_*</c> bitflags that affect this search</param>
	/// <param name="count">On return, will be set to the number of items in the returned array. Can be NULL.</param>
	/// <returns>
	/// Returns an array of strings on success or NULL on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information.
	/// This is a single allocation that should be freed with <see href="https://wiki.libsdl.org/SDL3/SDL_free">SDL_free</see>() when it is no longer needed.
	/// </returns>
	/// <remarks>
	/// <para>
	/// Files are filtered out if they don't match the string in <c><paramref name="pattern"/></c>, which may contain wildcard characters <c>*</c> (match everything) and <c>?</c> (match one character). If pattern is NULL, no filtering is done and all results are returned.
	/// Subdirectories are permitted, and are specified with a path separator of <c>/</c>. Wildcard characters <c>*</c> and <c>?</c> never match a path separator.
	/// </para>
	/// <para>
	/// <c><paramref name="flags"/></c> may be set to <see href="https://wiki.libsdl.org/SDL3/SDL_GLOB_CASEINSENSITIVE">SDL_GLOB_CASEINSENSITIVE</see> to make the pattern matching case-insensitive.
	/// </para>
	/// <para>
	/// The returned array is always NULL-terminated, for your iterating convenience, but if <c><paramref name="count"/></c> is non-NULL, on return it will contain the number of items in the array, not counting the NULL terminator.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GlobDirectory">SDL_GlobDirectory</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial byte** SDL_GlobDirectory(byte* path, byte* pattern, GlobFlags flags, int* count);

	/// <summary>
	/// Remove a file or an empty directory
	/// </summary>
	/// <param name="path">The path to remove from the filesystem</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// Directories that are not empty will fail; this function will not recursely delete directory trees.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_RemovePath">SDL_RemovePath</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_RemovePath(byte* path);

	/// <summary>
	/// Rename a file or directory
	/// </summary>
	/// <param name="oldpath">The old path</param>
	/// <param name="newpath">The new path</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// If the file at <c><paramref name="newpath"/></c> already exists, it will be replaced.
	/// </para>
	/// <para>
	/// Note that this will not copy files across filesystems/drives/volumes, as that is a much more complicated (and possibly time-consuming) operation.
	/// </para>
	/// <para>
	/// Which is to say, if this function fails, <see href="https://wiki.libsdl.org/SDL3/SDL_CopyFile">SDL_CopyFile</see>() to a temporary file in the same directory as newpath,
	/// then <see href="https://wiki.libsdl.org/SDL3/SDL_RenamePath">SDL_RenamePath</see>() from the temporary file to <c><paramref name="newpath"/></c> and <see href="https://wiki.libsdl.org/SDL3/SDL_RemovePath">SDL_RemovePath</see>() on <c><paramref name="oldpath"/></c> might work for files.
	/// Renaming a non-empty directory across filesystems is dramatically more complex, however.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_RenamePath">SDL_RenamePath</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_RenamePath(byte* oldpath, byte* newpath);
}
