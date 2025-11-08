using Sdl3Sharp.Internal.Interop;
using Sdl3Sharp.SourceGeneration;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.IO;

partial class FileStorage
{
	/// <summary>
	/// Opens up a container for local filesystem storage
	/// </summary>
	/// <param name="path">The base path prepended to all storage paths, or NULL for no base path</param>
	/// <returns>Returns a filesystem storage container on success or NULL on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// This is provided for development and tools.
	/// Portable applications should use <see href="https://wiki.libsdl.org/SDL3/SDL_OpenTitleStorage">SDL_OpenTitleStorage</see>() for access to game data and <see href="https://wiki.libsdl.org/SDL3/SDL_OpenUserStorage">SDL_OpenUserStorage</see>() for access to user data.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_OpenFileStorage">SDL_OpenFileStorage</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial SDL_Storage* SDL_OpenFileStorage(byte* path);
}
