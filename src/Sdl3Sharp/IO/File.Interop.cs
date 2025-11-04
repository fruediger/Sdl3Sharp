using Sdl3Sharp.Internal.Interop;
using Sdl3Sharp.SourceGeneration;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.IO;

partial class File
{
	/// <summary>
	/// Load all the data from a file path
	/// </summary>
	/// <param name="file">The path to read all available data from</param>
	/// <param name="datasize">If not NULL, will store the number of bytes read</param>
	/// <returns>Returns the data or NULL on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// The data is allocated with a zero byte at the end (null terminated) for convenience.
	/// This extra byte is not included in the value reported via <c><paramref name="datasize"/></c>.
	/// </para>
	/// <para>
	/// The data should be freed with <see href="https://wiki.libsdl.org/SDL3/SDL_free">SDL_free</see>().
	/// </para>
	/// <para>
	/// This function is not thread safe.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_LoadFile">SDL_LoadFile</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial void* SDL_LoadFile(byte* file, nuint* datasize);

	/// <summary>
	/// Load all the data from a file path, asynchronously
	/// </summary>
	/// <param name="file">The path to read all available data from</param>
	/// <param name="queue">A queue to add the new SDL_AsyncIO to</param>
	/// <param name="userdata">An app-defined pointer that will be provided with the task results</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// This function returns as quickly as possible; it does not wait for the read to complete. On a successful return, this work will continue in the background.
	/// If the work begins, even failure is asynchronous: a failing return value from this function only means the work couldn't start at all.
	/// </para>
	/// <para>
	/// The data is allocated with a zero byte at the end (null terminated) for convenience.
	/// This extra byte is not included in <see href="https://wiki.libsdl.org/SDL3/SDL_AsyncIOOutcome">SDL_AsyncIOOutcome</see>'s bytes_transferred value.
	/// </para>
	/// <para>
	/// This function will allocate the buffer to contain the file.
	/// It must be deallocated by calling <see href="https://wiki.libsdl.org/SDL3/SDL_free">SDL_free</see>() on <see href="https://wiki.libsdl.org/SDL3/SDL_AsyncIOOutcome">SDL_AsyncIOOutcome</see>'s buffer field after completion.
	/// </para>
	/// <para>
	/// An <see href="https://wiki.libsdl.org/SDL3/SDL_AsyncIOQueue">SDL_AsyncIOQueue</see> must be specified. The newly-created task will be added to it when it completes its work.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_LoadFileAsync">SDL_LoadFileAsync</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_LoadFileAsync(byte* file, AsyncIOQueue.SDL_AsyncIOQueue* queue, void* userdata);

	/// <summary>
	/// Save all the data into a file path
	/// </summary>
	/// <param name="file">The path to write all available data into</param>
	/// <param name="data">The data to be written. If datasize is 0, may be NULL or a invalid pointer</param>
	/// <param name="datasize">The number of bytes to be written</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// This function is not thread safe.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_SaveFile">SDL_SaveFile</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_SaveFile(byte* file, void* data, nuint datasize);
}
