using Sdl3Sharp.Internal.Interop;
using Sdl3Sharp.SourceGeneration;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.IO;

partial class UserStorage
{
	/// <summary>
	/// Opens up a container for a user's unique read/write filesystem
	/// </summary>
	/// <param name="org">The name of your organization</param>
	/// <param name="app">The name of your application</param>
	/// <param name="props">A property list that may contain backend-specific information</param>
	/// <returns>Returns a user storage container on success or NULL on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// While title storage can generally be kept open throughout runtime, user storage should only be opened when the client is ready to read/write files.
	/// This allows the backend to properly batch file operations and flush them when the container has been closed; ensuring safe and optimal save I/O.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_OpenUserStorage">SDL_OpenUserStorage</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial SDL_Storage* SDL_OpenUserStorage(byte* org, byte* app, uint props);
}
