using Sdl3Sharp.Internal.Interop;
using Sdl3Sharp.SourceGeneration;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sdl3Sharp.IO;

partial class AsyncIO
{
	// Used for opaque pointers
	[StructLayout(LayoutKind.Sequential, Size = 0)]
	internal readonly struct SDL_AsyncIO;

	/// <summary>
	/// Use this function to create a new <see href="https://wiki.libsdl.org/SDL3/SDL_AsyncIO">SDL_AsyncIO</see> object for reading from and/or writing to a named file
	/// </summary>
	/// <param name="file">A UTF-8 string representing the filename to open</param>
	/// <param name="mode">An ASCII string representing the mode to be used for opening the file</param>
	/// <returns>Returns a pointer to the <see href="https://wiki.libsdl.org/SDL3/SDL_AsyncIO">SDL_AsyncIO</see> structure that is created or NULL on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// The <c><paramref name="mode"/></c> string understands the following values:
	/// <list type="bullet">
	///		<item>
	///			<term>"r"</term>
	///			<description>Open a file for reading only. It must exist.</description>
	///		</item>
	///		<item>
	///			<term>"w"</term>
	///			<description>Open a file for writing only. It will create missing files or truncate existing ones.</description>
	///		</item>
	///		<item>
	///			<term>"r+"</term>
	///			<description>Open a file for update both reading and writing. The file must exist.</description>
	///		</item>
	///		<item>
	///			<term>"w+"</term>
	///			<description>Create an empty file for both reading and writing. If a file with the same name already exists its content is erased and the file is treated as a new empty file.</description>
	///		</item>
	/// </list> 
	/// </para>
	/// <para>
	/// There is no "b" mode, as there is only "binary" style I/O, and no "a" mode for appending, since you specify the position when starting a task.
	/// </para>
	/// <para>
	/// This function supports Unicode filenames, but they must be encoded in UTF-8 format, regardless of the underlying operating system.
	/// </para>
	/// <para>
	/// This call is <em>not</em> asynchronous; it will open the file before returning, under the assumption that doing so is generally a fast operation.
	/// Future reads and writes to the opened file will be async, however.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_AsyncIOFromFile">SDL_AsyncIOFromFile</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial SDL_AsyncIO* SDL_AsyncIOFromFile(byte* file, byte* mode);

	/// <summary>
	/// Close and free any allocated resources for an async I/O object
	/// </summary>
	/// <param name="asyncio">A pointer to an SDL_AsyncIO structure to close</param>
	/// <param name="flush">True if data should sync to disk before the task completes</param>
	/// <param name="queue">A queue to add the new SDL_AsyncIO to</param>
	/// <param name="userdata">An app-defined pointer that will be provided with the task results</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// Closing a file is <em>also</em> an asynchronous task! If a write failure were to happen during the closing process, for example, the task results will report it as usual.
	/// </para>
	/// <para>
	/// Closing a file that has been written to does not guarantee the data has made it to physical media; it may remain in the operating system's file cache, for later writing to disk.
	/// This means that a successfully-closed file can be lost if the system crashes or loses power in this small window.
	/// To prevent this, call this function with the flush parameter set to true.
	/// This will make the operation take longer, and perhaps increase system load in general, but a successful result guarantees that the data has made it to physical storage.
	/// Don't use this for temporary files, caches, and unimportant data, and definitely use it for crucial irreplaceable files, like game saves.
	/// </para>
	/// <para>
	/// This function guarantees that the close will happen after any other pending tasks to <c><paramref name="asyncio"/></c>, so it's safe to open a file, start several operations, close the file immediately, then check for all results later.
	/// This function will not block until the tasks have completed.
	/// </para>
	/// <para>
	/// Once this function returns true, <c><paramref name="asyncio"/></c> is no longer valid, regardless of any future outcomes.
	/// Any completed tasks might still contain this pointer in their <see href="https://wiki.libsdl.org/SDL3/SDL_AsyncIOOutcome">SDL_AsyncIOOutcome</see> data, in case the app was using this value to track information, but it should not be used again.
	/// </para>
	/// <para>
	/// If this function returns false, the close wasn't started at all, and it's safe to attempt to close again later.
	/// </para>
	/// <para>
	/// An <see href="https://wiki.libsdl.org/SDL3/SDL_AsyncIOQueue">SDL_AsyncIOQueue</see> must be specified. The newly-created task will be added to it when it completes its work.
	/// </para>
	/// <para>
	/// It is safe to call this function from any thread, but two threads should not attempt to close the same object.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_CloseAsyncIO">SDL_CloseAsyncIO</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_CloseAsyncIO(SDL_AsyncIO* asyncio, CBool flush, AsyncIOQueue.SDL_AsyncIOQueue* queue, void* userdata);

	/// <summary>
	/// Use this function to get the size of the data stream in an <see href="https://wiki.libsdl.org/SDL3/SDL_AsyncIO">SDL_AsyncIO</see>
	/// </summary>
	/// <param name="asyncio">The <see href="https://wiki.libsdl.org/SDL3/SDL_AsyncIO">SDL_AsyncIO</see> to get the size of the data stream from</param>
	/// <returns>Returns the size of the data stream in the <see href="https://wiki.libsdl.org/SDL3/SDL_IOStream">SDL_IOStream</see> on success or a negative error code on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information.</returns>
	/// <remarks>
	/// <para>
	/// This call is <em>not</em> asynchronous; it assumes that obtaining this info is a non-blocking operation in most reasonable cases.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetAsyncIOSize">SDL_GetAsyncIOSize</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial long SDL_GetAsyncIOSize(SDL_AsyncIO* asyncio);

	/// <summary>
	/// Start an async read
	/// </summary>
	/// <param name="asyncio">A pointer to an SDL_AsyncIO structure</param>
	/// <param name="ptr">A pointer to a buffer to read data into</param>
	/// <param name="offset">The position to start reading in the data source</param>
	/// <param name="size">The number of bytes to read from the data source</param>
	/// <param name="queue">A queue to add the new SDL_AsyncIO to</param>
	/// <param name="userdata">An app-defined pointer that will be provided with the task results</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// This function reads up to <c><paramref name="size"/></c> bytes from <c><paramref name="offset"/></c> position in the data source to the area pointed at by <c><paramref name="ptr"/></c>.
	/// This function may read less bytes than requested.
	/// </para>
	/// <para>
	/// This function returns as quickly as possible; it does not wait for the read to complete. On a successful return, this work will continue in the background.
	/// If the work begins, even failure is asynchronous: a failing return value from this function only means the work couldn't start at all.
	/// </para>
	/// <para>
	/// <c><paramref name="ptr"/></c> must remain available until the work is done, and may be accessed by the system at any time until then.
	/// Do not allocate it on the stack, as this might take longer than the life of the calling function to complete!
	/// </para>
	/// <para>
	/// An <see href="https://wiki.libsdl.org/SDL3/SDL_AsyncIOQueue">SDL_AsyncIOQueue</see> must be specified. The newly-created task will be added to it when it completes its work.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_ReadAsyncIO">SDL_ReadAsyncIO</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_ReadAsyncIO(SDL_AsyncIO* asyncio, void* ptr, ulong offset, ulong size, AsyncIOQueue.SDL_AsyncIOQueue* queue, void* userdata);

	/// <summary>
	/// Start an async write
	/// </summary>
	/// <param name="asyncio">A pointer to an <see href="https://wiki.libsdl.org/SDL3/SDL_AsyncIO">SDL_AsyncIO</see> structure</param>
	/// <param name="ptr">A pointer to a buffer to write data from</param>
	/// <param name="offset">The position to start writing to the data source</param>
	/// <param name="size">The number of bytes to write to the data source</param>
	/// <param name="queue">A queue to add the new <see href="https://wiki.libsdl.org/SDL3/SDL_AsyncIO">SDL_AsyncIO</see> to</param>
	/// <param name="userdata">An app-defined pointer that will be provided with the task results</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information.</returns>
	/// <remarks>
	/// <para>
	/// This function writes <c><paramref name="size"/></c> bytes from <c><paramref name="offset"/></c> position in the data source to the area pointed at by <c><paramref name="ptr"/></c>.
	/// </para>
	/// <para>
	/// This function returns as quickly as possible; it does not wait for the write to complete. On a successful return, this work will continue in the background.
	/// If the work begins, even failure is asynchronous: a failing return value from this function only means the work couldn't start at all.
	/// </para>
	/// <para>
	/// <c><paramref name="ptr"/></c> must remain available until the work is done, and may be accessed by the system at any time until then.
	/// Do not allocate it on the stack, as this might take longer than the life of the calling function to complete!
	/// </para>
	/// <para>
	/// An <see href="https://wiki.libsdl.org/SDL3/SDL_AsyncIOQueue">SDL_AsyncIOQueue</see> must be specified. The newly-created task will be added to it when it completes its work.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_WriteAsyncIO">SDL_WriteAsyncIO</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_WriteAsyncIO(SDL_AsyncIO* asyncio, void* ptr, ulong offset, ulong size, AsyncIOQueue.SDL_AsyncIOQueue* queue, void* userdata);
}
