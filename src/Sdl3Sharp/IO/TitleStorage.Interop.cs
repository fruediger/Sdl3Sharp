using Sdl3Sharp.Internal.Interop;
using Sdl3Sharp.SourceGeneration;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.IO;

partial class TitleStorage
{
	/// <summary>
	/// Opens up a read-only container for the application's filesystem
	/// </summary>
	/// <param name="override">A path to override the backend's default title root</param>
	/// <param name="props">A property list that may contain backend-specific information</param>
	/// <returns>Returns a title storage container on success or NULL on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// By default, <see href="https://wiki.libsdl.org/SDL3/SDL_OpenTitleStorage">SDL_OpenTitleStorage</see> uses the generic storage implementation.
	/// When the path override is not provided, the generic implementation will use the output of <see href="https://wiki.libsdl.org/SDL3/SDL_GetBasePath">SDL_GetBasePath</see> as the base path.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_OpenTitleStorage">SDL_OpenTitleStorage</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial SDL_Storage* SDL_OpenTitleStorage(byte* @override, uint props);
}
