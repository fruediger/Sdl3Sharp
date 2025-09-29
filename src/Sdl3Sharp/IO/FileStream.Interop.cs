using Sdl3Sharp.Internal.Interop;
using Sdl3Sharp.SourceGeneration;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.IO;

partial class FileStream
{
	/// <summary>
	/// Use this function to create a new <see href="https://wiki.libsdl.org/SDL3/SDL_IOStream">SDL_IOStream</see> structure for reading from and/or writing to a named file
	/// </summary>
	/// <param name="file">A UTF-8 string representing the filename to open</param>
	/// <param name="mode">An ASCII string representing the mode to be used for opening the file</param>
	/// <returns>Returns a pointer to the <see href="https://wiki.libsdl.org/SDL3/SDL_IOStream">SDL_IOStream</see> structure that is created or NULL on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// The <c><paramref name="mode"/></c> string is treated roughly the same as in a call to the C library's fopen(), even if SDL doesn't happen to use fopen() behind the scenes.
	/// </para>
	/// <para>
	/// Available <c><paramref name="mode"/></c> strings:
	/// <list type="bullet">
	///		<item>
	///			<tem>"r"</tem>
	///			<description>Open a file for reading. The file must exist.</description>
	///		</item>
	///		<item>
	///			<tem>"w"</tem>
	///			<description>
	///				Create an empty file for writing.
	///				If a file with the same name already exists its content is erased and the file is treated as a new empty file.
	///			</description>
	///		</item>
	///		<item>
	///			<tem>"a"</tem>
	///			<description>Append to a file. Writing operations append data at the end of the file. The file is created if it does not exist.</description>
	///		</item>
	///		<item>
	///			<tem>"r+"</tem>
	///			<description>Open a file for update both reading and writing. The file must exist.</description>
	///		</item>
	///		<item>
	///			<tem>"w+"</tem>
	///			<description>
	///				Create an empty file for both reading and writing.
	///				If a file with the same name already exists its content is erased and the file is treated as a new empty file.
	///			</description>
	///		</item>
	///		<item>
	///			<tem>"a+"</tem>
	///			<description>
	///				Open a file for reading and appending.
	///				All writing operations are performed at the end of the file, protecting the previous content to be overwritten.
	///				You can reposition (fseek, rewind) the internal pointer to anywhere in the file for reading, but writing operations will move it back to the end of file.
	///				The file is created if it does not exist.
	///			</description>
	///		</item>
	/// </list>
	/// <em>NOTE</em>: In order to open a file as a binary file, a "b" character has to be included in the mode string.
	/// This additional "b" character can either be appended at the end of the string (thus making the following compound modes: "rb", "wb", "ab", "r+b", "w+b", "a+b") or be inserted between the letter and the "+" sign for the mixed modes ("rb+", "wb+", "ab+").
	/// Additional characters may follow the sequence, although they should have no effect. For example, "t" is sometimes appended to make explicit the file is a text file.
	/// </para>
	/// <para>
	/// This function supports Unicode filenames, but they must be encoded in UTF-8 format, regardless of the underlying operating system.
	/// </para>
	/// <para>
	/// In Android, <see href="https://wiki.libsdl.org/SDL3/SDL_IOFromFile">SDL_IOFromFile</see>() can be used to open content:// URIs.
	/// As a fallback, <see href="https://wiki.libsdl.org/SDL3/SDL_IOFromFile">SDL_IOFromFile</see>() will transparently open a matching filename in the app's <c>assets</c>.
	/// </para>
	/// <para>
	/// Closing the <see href="https://wiki.libsdl.org/SDL3/SDL_IOStream">SDL_IOStream</see> will close SDL's internal file handle.
	/// </para>
	/// <para>
	/// The following properties may be set at creation time by SDL:
	/// <list type="bullet">
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_IOSTREAM_WINDOWS_HANDLE_POINTER"><c>SDL_PROP_IOSTREAM_WINDOWS_HANDLE_POINTER</c></see></term>
	///			<description>
	///				A pointer, that can be cast to a win32 <c>HANDLE</c>, that this <see href="https://wiki.libsdl.org/SDL3/SDL_IOStream">SDL_IOStream</see> is using to access the filesystem.
	///				If the program isn't running on Windows, or SDL used some other method to access the filesystem, this property will not be set.
	///			</description>
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_IOSTREAM_STDIO_FILE_POINTER"><c>SDL_PROP_IOSTREAM_STDIO_FILE_POINTER</c></see></term>
	///			<description>
	///				A pointer, that can be cast to a stdio <c>FILE *</c>, that this <see href="https://wiki.libsdl.org/SDL3/SDL_PROP_IOSTREAM_STDIO_FILE_POINTER">SDL_IOStream</see> is using to access the filesystem.
	///				If SDL used some other method to access the filesystem, this property will not be set.
	///				PLEASE NOTE that if SDL is using a different C runtime than your app, trying to use this pointer will almost certainly result in a crash!
	///				This is mostly a problem on Windows; make sure you build SDL and your app with the same compiler and settings to avoid it.
	///			</description>
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_IOSTREAM_FILE_DESCRIPTOR_NUMBER"><c>SDL_PROP_IOSTREAM_FILE_DESCRIPTOR_NUMBER</c></see></term>
	///			<description>A file descriptor that this SDL_IOStream is using to access the filesystem</description>
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_IOSTREAM_ANDROID_AASSET_POINTER"><c>SDL_PROP_IOSTREAM_ANDROID_AASSET_POINTER</c></see></term>
	///			<description>
	///				A pointer, that can be cast to an Android NDK <c>AAsset *</c>, that this SDL_IOStream is using to access the filesystem.
	///				If SDL used some other method to access the filesystem, this property will not be set.
	///			</description>
	///		</item>
	/// </list>
	/// </para>
	/// <para>
	/// This function is not thread safe.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_IOFromFile">SDL_IOFromFile</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial Stream.SDL_IOStream* SDL_IOFromFile(byte* file, byte* mode);
}
