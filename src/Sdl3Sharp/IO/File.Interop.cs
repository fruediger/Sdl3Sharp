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
