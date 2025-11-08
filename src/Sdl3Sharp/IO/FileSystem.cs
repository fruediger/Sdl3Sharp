using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace Sdl3Sharp.IO;

/// <summary>
/// Provides static methods and properties to access file system related functionality
/// </summary>
/// <remarks>
/// <para>
/// SDL offers an APIs for examining and manipulating the system's file system.
/// This covers most things one would need to do with directories, except for actual file I/O (which is covered by <see cref="File"/>, <see cref="Stream"/>, <see cref="AsyncIO"/>, and partially by <see cref="Storage"/> instead).
/// </para>
/// <para>
/// There are the members to answer necessary path questions:
/// <list type="bullet">
///		<item>
///			<term>Where is my app's data?</term>
///			<description><see cref="BasePath"/></description>
///		</item>
///		<item>
///			<term>Where can I safely write files?</term>
///			<description><see cref="TryGetPreferencesPath(string?, string, out string?)"/></description>
///		</item>
///		<item>
///			<term>Where are paths like <see cref="Folder.Downloads">Downloads</see>, <see cref="Folder.Desktop">Desktop</see>, <see cref="Folder.Music">Music</see>?</term>
///			<description><see cref="TryGetUserFolder(Folder, out string?)"/></description>
///		</item>
///		<item>
///			<term>What is this thing at this location?</term>
///			<description><see cref="TryGetPathInfo(string, out PathInfo)"/></description>
///		</item>
///		<item>
///			<term>What items live in this folder?</term>
///			<description><see cref="TryEnumerateDirectory(string, EnumerateDirectoryCallback)"/> or <see cref="EnumerateDirectory(string)"/></description>
///		</item>
///		<item>
///			<term>What items live in this folder by wildcard?</term>
///			<description><see cref="TryGlobDirectory(string, string?, GlobFlags, out string[])"/></description>
///		</item>
///		<item>
///			<term>What is my current working directory?</term>
///			<description><see cref="CurrentDirectory"/></description>
///		</item>
/// </list>
/// </para>
/// <para>
/// SDL'S APIs also offers functionality to manipulate the directory tree: <see cref="TryRenamePath(string, string)">renaming</see>, <see cref="TryRemovePath(string)">removing</see>, <see cref="TryCopyFile(string, string)">copying</see> files.
/// </para>
/// </remarks>
public static partial class FileSystem
{
	/// <summary>
	/// Gets the path where the application was run from
	/// </summary>
	/// <value>
	/// The path where the application was run from, or <c><see langword="null"/></c> on error or when the plaform does not support this functionality (check <see cref="Error.TryGet(out string?)"/> for more information)
	/// </value>
	/// <remarks>
	/// <para>
	/// While SDL caches the value of this property internally, the first access to this property is not necessarily fast, so plan accordingly.
	/// </para>
	/// <para>
	/// <list type="bullet">
	///		<item>
	///			<term>macOS and iOS Specific Functionality</term>
	///			<description>
	///				<para>
	///				If the application is in a ".app" bundle, the value of this property will be the resource directory (e.g. <c>"MyApp.app/Contents/Resources/"</c>).
	///				This behaviour can be overridden by adding a property to the Info.plist file.
	///				Adding a string key with the name <c>SDL_FILESYSTEM_BASE_DIR_TYPE</c> with a supported value will change the behaviour.				
	///				</para>
	///				<para>
	///				Supported values for the <c>SDL_FILESYSTEM_BASE_DIR_TYPE</c> property are:
	///				<list type="bullet">
	///					<item>
	///						<term><c>resource</c></term>
	///						<description>The bundle's resource directory. For example: <c>"/Applications/SDLApp/MyApp.app/"</c>. This is the default.</description>
	///					</item>
	///					<item>
	///						<term><c>bundle</c></term>
	///						<description>The bundle directory. For example: <c>"/Applications/SDLApp/MyApp.app/"</c>.</description>
	///					</item>
	///					<item>
	///						<term><c>parent</c></term>
	///						<description>The containing directory of the bundle. For example: <c>"/Applications/SDLApp/"</c>.</description>
	///					</item>
	///				</list>
	///				</para>
	///			</description>
	///		</item>
	///		<item>
	///			<term>Nintendo 3DS Specific Functionality</term>
	///			<description>
	///				<para>
	///					The value of this property will be "romfs" directory of the application, as it is uncommon to store resources outside the executable.
	///					As such it is not a writable directory.
	///				</para>
	///			</description>
	///		</item>
	/// </list>
	/// </para>
	/// <para>
	/// The value of this property is guaranteed to end with a directory separator character (<c>'\\'</c> on Windows; <c>'/'</c> on most other platforms).
	/// </para>
	/// </remarks>
	public static string? BasePath
	{
		get
		{
			unsafe
			{
				// at first glance, it might seem stupid to retry later if a previous call to SDL_GetBasePath returned null (aka. not caching null-values too),
				// but this way, we'll most likely get the error message again (Error.TryGet)

				return field ??= Utf8StringMarshaller.ConvertToManaged(SDL_GetBasePath());
			}
		}
	}

	/// <summary>
	/// Gets what the system considers the "current working directory"
	/// </summary>
	/// <value>
	/// The current working directory, or <c><see langword="null"/></c> on error
	/// </value>
	/// <remarks>
	/// <para>
	/// For systems without a concept of a current working directory, this will still attempt to provide something reasonable.
	/// </para>
	/// <para>
	/// SDL does not provide the means to change the current working directory (for platforms without this concept). Please use the provided .NET APIs to change the current working directory if needed.
	/// </para>
	/// <para>
	/// The value of this property is guaranteed to end with a directory separator character (<c>'\\'</c> on Windows; <c>'/'</c> on most other platforms).
	/// </para>
	/// </remarks>
	public static string? CurrentDirectory
	{
		get
		{
			unsafe
			{
				var cwd = SDL_GetBasePath();

				try
				{
					return Utf8StringMarshaller.ConvertToManaged(cwd);
				}
				finally
				{
					Utilities.NativeMemory.SDL_free(cwd);
				}
			}
		}
	}

	/// <summary>
	/// Enumerates file system entries for a specified directory path
	/// </summary>
	/// <param name="path">The path to the directory to enumerate</param>
	/// <returns>A <see cref="DirectoryEnumerable"/> that can be used to enumerate the file system entries for the specified directory <paramref name="path"/></returns>
	/// <inheritdoc cref="DirectoryEnumerable"/>
	public static DirectoryEnumerable EnumerateDirectory(string path) => new(path);

	/// <summary>
	/// Determines whether a file system entry exists at a specified path
	/// </summary>
	/// <param name="path">The path to the file system entry</param>
	/// <returns><c><see langword="true"/></c>, if the file system entry at <paramref name="path"/> exists; otherwise, <c><see langword="false"/></c></returns>
	public static bool PathExists(string path)
	{
		unsafe
		{
			var pathUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(path);

			try
			{
				return SDL_GetPathInfo(pathUtf8, null);
			}
			finally
			{
				Utf8StringMarshaller.Free(pathUtf8);
			}
		}
	}

	/// <summary>
	/// Tries to copy a file from one path to another
	/// </summary>
	/// <param name="oldPath">The original file path</param>
	/// <param name="newPath">The new file path</param>
	/// <returns><c><see langword="true"/></c>, if the file was copied successfully; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// If the file at <paramref name="newPath"/> already exists, it will be overwritten with the contents of the file at <paramref name="oldPath"/>.
	/// </para>
	/// <para>
	/// This method will block until the copy is complete, which might be a significant time for large files on slow disks.
	/// On some platforms, the copy can be handed off to the OS itself, but on others SDL might just open both paths, and read from one and write to the other.
	/// </para>
	/// <para>
	/// Note: this is <em>not</em> an atomic operation! If something tries to read from <paramref name="newPath"/> while the copy is in progress, it will see an incomplete copy of the data, and if the calling thread terminates (or the power goes out) during the copy, <paramref name="newPath"/>'s previous contents will be gone, replaced with an incomplete copy of the data.
	/// To avoid this risk, it is recommended that you copy to a temporary file in the same directory as <paramref name="newPath"/> first, and if the copy is successful, use <see cref="TryRenamePath(string, string)"/> to replace <paramref name="newPath"/> with the temporary file.
	/// This will ensure that reads of <paramref name="newPath"/> will either see a complete copy of the data, or it will see the pre-copy state of the file at <paramref name="newPath"/>.
	/// </para>
	/// <para>
	/// This method attempts to synchronize the newly-copied data to disk before returning, if the platform allows it, so that the renaming trick will not have a problem in a system crash or power failure,
	/// where the file could be renamed but the contents never made it from the system file cache to the physical disk.
	/// </para>
	/// <para>
	/// If the copy operation fails for any reason, the state of the file system entry at <paramref name="newPath"/> is undefined.
	/// It might be half a copy, it might be the untouched data of what was already there, or it might be a zero-byte file, etc.
	/// </para>
	/// <para>
	/// While it's safe to call this method from any thread, the operation itself is <em>not</em> atomic, so you'll might need to protect access to specific paths from other threads if appropriate.
	/// </para>
	/// </remarks>
	public static bool TryCopyFile(string oldPath, string newPath)
	{
		unsafe
		{
			var oldPathUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(oldPath);
			var newPathUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(newPath);

			try
			{
				return SDL_CopyFile(oldPathUtf8, newPathUtf8);
			}
			finally
			{
				Utf8StringMarshaller.Free(newPathUtf8);
				Utf8StringMarshaller.Free(oldPathUtf8);
			}
		}
	}

	/// <summary>
	/// Tries to create a directory, and any missing parent directories, at the specified path
	/// </summary>
	/// <param name="path">The path to the directory to create</param>
	/// <returns><c><see langword="true"/></c>, if the directory was created successfully; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method returns <c><see langword="true"/></c> if the directory already exists.
	/// </para>
	/// <para>
	/// If any parent directories in the specified path do not exist, they will be created as well. Please note, that if the operation fails at any point, parent directories that were already created will not be removed.
	/// </para>
	/// </remarks>
	public static bool TryCreateDirectory(string path)
	{
		unsafe
		{
			var pathUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(path);

			try
			{
				return SDL_CreateDirectory(pathUtf8);
			}
			finally
			{
				Utf8StringMarshaller.Free(pathUtf8);
			}
		}
	}

	/// <summary>
	/// Tries to enumerate file system entries in a directory
	/// </summary>
	/// <param name="path">The path to the directory to enumerate</param>
	/// <param name="callback">The callback to invoke for each file system entry</param>
	/// <returns><c><see langword="true"/></c>, if the enumeration was successful; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method will continue enumerating file system entries until all entries have been enumerated, or until the provided <paramref name="callback"/> returns either <see cref="EnumerationResult.Success"/> or <see cref="EnumerationResult.Failure"/>.
	/// </para>
	/// <para>
	/// This method will return <c><see langword="false"/></c>, if there was an error in general, or if the provided <paramref name="callback"/> returned <see cref="EnumerationResult.Failure"/>.
	/// A return value of <c><see langword="true"/></c> indicates that all file system entries were enumerated successfully, or that the provided <paramref name="callback"/> returned <see cref="EnumerationResult.Success"/>.
	/// </para>
	/// </remarks>
	/// <exception cref="ArgumentNullException"><paramref name="callback"/> is <c><see langword="null"/></c></exception>
	public static bool TryEnumerateDirectory(string path, EnumerateDirectoryCallback callback)
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
					return SDL_EnumerateDirectory(pathUtf8, &EnumerateDirectoryCallback, unchecked((void*)GCHandle.ToIntPtr(gcHandle)));
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
	/// Tries to get information about a file system path
	/// </summary>
	/// <param name="path">The path to get information about</param>
	/// <param name="info">The information about <paramref name="path"/></param>
	/// <returns><c><see langword="true"/></c>, if the information about the file system path was retrieved successfully; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	public static bool TryGetPathInfo(string path, out PathInfo info)
	{
		unsafe
		{
			var pathUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(path);

			try
			{
				fixed (PathInfo* infoPtr = &info)
				{
					return SDL_GetPathInfo(pathUtf8, infoPtr);
				}
			}
			finally
			{
				Utf8StringMarshaller.Free(pathUtf8);
			}
		}
	}

	/// <summary>
	/// Tries to get the user-and-application-specific path where files can be written to
	/// </summary>
	/// <param name="orgName">The name of your organization</param>
	/// <param name="appName">The name of your application</param>
	/// <param name="preferencesPath">The path to the preferences directory, when this method returns <c><see langword="true"/></c>; otherwise, <c><see langword="null"/></c></param>
	/// <returns><c><see langword="true"/></c>, if the preferences path was retrieved successfully; otherwise, <c><see langword="false"/></c></returns>
	/// <remarks>
	/// <para>
	/// The "preferences path" is a directory, that is meant to store the user's personal files (preferences and save games, etc) that are specific to your application. This directory is unique per user, per application.
	/// </para>
	/// <para>
	/// This method will decide the appropriate location in the native filesystem, create the directory if necessary, and result in the absolute path to that directory.
	/// </para>
	/// <para>
	/// Examples for "preferences paths" are:
	/// <list type="bullet">
	///		<item>
	///			<term>On Windows</term>
	///			<description><c>"C:\\Users\\bob\\AppData\\Roaming\\My Company\\My Program Name\\"</c></description>
	///		</item>
	///		<item>
	///			<term>On Linux</term>
	///			<description><c>"/home/bob/.local/share/My Program Name/"</c></description>
	///		</item>
	///		<item>
	///			<term>On macOS</term>
	///			<description><c>"/Users/bob/Library/Application Support/My Program Name/"</c></description>
	///		</item>
	/// </list>
	/// </para>
	/// <para>
	/// You should assume that the resulting path of a call to this method is the <em>only</em> safe place to write files to (and that <see cref="BasePath"/>, while it might be writable, or even the parent of the resulting path, isn't where you should be writing things).
	/// </para>
	/// <para>
	/// Both the <paramref name="orgName"/> and <paramref name="appName"/> may become part of a directory name, so please follow these rules:
	/// <list type="bullet">
	///		<item>
	///			<description>
	///				Try to use the same value for <paramref name="orgName"/> (including case-sensitivity) for all your applications that use this functionality.
	///			</description>			
	///		</item>
	///		<item>
	///			<description>
	///				Always use an unique value for <paramref name="appName"/> for each individual application, and make sure it never changes for an application once you've decided on it.
	///			</description>			
	///		</item>
	///		<item>
	///			<description>
	///				Unicode characters are legal, but you should only use letters, numbers, and spaces. Avoid punctuation like "Game Name 2: Bad Guy's Revenge!", "Game Name 2" is sufficient.
	///			</description>			
	///		</item>
	/// </list>
	/// </para>
	/// <para>
	/// Due to historical mistakes, <paramref name="orgName"/> is allowed to be <c><see langword="null"/></c> or the empty string (<c>""</c>).
	/// In such cases, SDL will omit the subdirectory for <paramref name="orgName"/>, including on platforms where it shouldn't, and including on platforms where this would make your app fail certification for an app store.
	/// <em>Newly created application should definitely specify a non-<c><see langword="null"/></c>, non-empty value for <paramref name="orgName"/>!</em>
	/// </para> 
	/// <para>
	/// The resulting <paramref name="preferencesPath"/> is guaranteed to end with a directory separator character (<c>'\\'</c> on Windows; <c>'/'</c> on most other platforms).
	/// </para>
	/// </remarks>
	public static bool TryGetPreferencesPath(string? orgName, string appName, [NotNullWhen(true)] out string? preferencesPath)
	{
		unsafe
		{
			var orgNameUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(orgName);
			var appNameUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(appName);

			try
			{
				preferencesPath = Utf8StringMarshaller.ConvertToManaged(SDL_GetPrefPath(orgNameUtf8, appNameUtf8));

				return preferencesPath is not null;
			}
			finally
			{
				Utf8StringMarshaller.Free(appNameUtf8);
				Utf8StringMarshaller.Free(orgNameUtf8);
			}
		}
	}

	/// <summary>
	/// Tries to get the path to a most suitable user folder for a specific purpose
	/// </summary>
	/// <param name="folder">The type of folder to get</param>
	/// <param name="userFolderPath">The path to the user folder, when this method returns <c><see langword="true"/></c>; otherwise, <c><see langword="null"/></c></param>
	/// <returns><c><see langword="true"/></c>, if the user folder was retrieved successfully; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// Many operating systems provide certain standard folders for certain purposes, such as storing pictures, music or videos for a certain user.
	/// This method retrieves the paths to for many of those special locations.
	/// </para>
	/// <para>
	/// This method is specifically for user folders, which are meant for the user to access and manage.
	/// For application-specific folders, meant to hold data for the application to manage, see <see cref="BasePath"/> and <see cref="TryGetPreferencesPath(string?, string, out string?)"/>.
	/// </para>
	/// <para>
	/// The resulting <paramref name="userFolderPath"/> is guaranteed to end with a directory separator character (<c>'\\'</c> on Windows; <c>'/'</c> on most other platforms).
	/// </para>
	/// </remarks>
	public static bool TryGetUserFolder(Folder folder, [NotNullWhen(true)] out string? userFolderPath)
	{
		unsafe
		{
			userFolderPath = Utf8StringMarshaller.ConvertToManaged(SDL_GetUserFolder(folder));

			return userFolderPath is not null;
		}
	}

	/// <summary>
	/// Tries to enumerate file system entries in a directory, filtered by a pattern
	/// </summary>
	/// <param name="path">The path to the directory to enumerate</param>
	/// <param name="pattern">The pattern that entries must match. Can be <c><see langword="null"/></c> to not filter at all.</param>
	/// <param name="flags">Flags to modify the pattern matching behavior</param>
	/// <param name="matches">The matching file system entries, when this method returns <c><see langword="true"/></c>; otherwise, <c><see langword="null"/></c></param>
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
	/// </remarks>
	public static bool TryGlobDirectory(string path, string? pattern, GlobFlags flags, [NotNullWhen(true)] out string[]? matches)
	{
		unsafe
		{
			var pathUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(path);
			var patternUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(pattern);

			int count;
			byte** matchesUtf8;

			try
			{
				matchesUtf8 = SDL_GlobDirectory(pathUtf8, patternUtf8, flags, &count);
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
	/// Tries to remove a file or an empty directory at the specified path
	/// </summary>
	/// <param name="path">The path to the file or directory to remove</param>
	/// <returns><c><see langword="true"/></c>, if the file or directory was removed successfully; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// Directories that are not empty will lead to this method returning <c><see langword="false"/></c>.
	/// This method does not recursively delete directory trees.
	/// </para>
	/// </remarks>
	public static bool TryRemovePath(string path)
	{
		unsafe
		{
			var pathUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(path);

			try
			{
				return SDL_RemovePath(pathUtf8);
			}
			finally
			{
				Utf8StringMarshaller.Free(pathUtf8);
			}
		}
	}

	/// <summary>
	/// Tries to rename a file or directory from one path to another
	/// </summary>
	/// <param name="oldPath">The old path to rename</param>
	/// <param name="newPath">The new path</param>
	/// <returns><c><see langword="true"/></c>, if the file or directory was renamed successfully; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// If the file at <paramref name="newPath"/> already exists, it will be replaced.
	/// </para>
	/// <para>
	/// Note: this method will not copy files across filesystems/drives/volumes, as that is a much more complicated (and possibly time-consuming) operation.
	/// </para>
	/// <para>
	/// If this method fails, you could instead <see cref="TryCopyFile(string, string)"/> to a temporary file in the same directory as <paramref name="newPath"/>,
	/// then <see cref="TryRenamePath(string, string)"/> from the temporary file to <paramref name="newPath"/> and <see cref="TryRemovePath(string)"/> on <paramref name="oldPath"/>, as that might work for files.
	/// Renaming a non-empty directory across filesystems is dramatically more complex though.
	/// </para>
	/// </remarks>
	public static bool TryRenamePath(string oldPath, string newPath)
	{
		unsafe
		{
			var oldPathUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(oldPath);
			var newPathUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(newPath);

			try
			{
				return SDL_RenamePath(oldPathUtf8, newPathUtf8);
			}
			finally
			{
				Utf8StringMarshaller.Free(newPathUtf8);
				Utf8StringMarshaller.Free(oldPathUtf8);
			}
		}
	}
}
