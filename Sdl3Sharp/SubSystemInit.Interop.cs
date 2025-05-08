using Sdl3Sharp.Interop;
using Sdl3Sharp.SourceGeneration;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp;

partial class SubSystemInit
{
	/// <summary>
	/// Compatibility function to initialize the SDL library
	/// </summary>
	/// <param name="flags">any of the flags used by <see href="https://wiki.libsdl.org/SDL3/SDL_Init">SDL_Init</see>(); see <see href="https://wiki.libsdl.org/SDL3/SDL_Init">SDL_Init</see> for details</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>This function and <see href="https://wiki.libsdl.org/SDL3/SDL_Init">SDL_Init</see>() are interchangeable</remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_InitSubSystem">SDL_InitSubSystem</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal static partial CBool SDL_InitSubSystem(InitFlags flags);

	/// <summary>
	/// Shut down specific SDL subsystems
	/// </summary>
	/// <param name="flags">any of the flags used by <see href="https://wiki.libsdl.org/SDL3/SDL_Init">SDL_Init</see>(); see <see href="https://wiki.libsdl.org/SDL3/SDL_Init">SDL_Init</see> for details</param>
	/// <remarks>
	/// You still need to call <see href="https://wiki.libsdl.org/SDL3/SDL_Quit">SDL_Quit</see>() even if you close all open subsystems with <see href="https://wiki.libsdl.org/SDL3/SDL_QuitSubSystem">SDL_QuitSubSystem</see>()
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_QuitSubSystem">SDL_QuitSubSystem</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal static partial void SDL_QuitSubSystem(InitFlags flags);
}
