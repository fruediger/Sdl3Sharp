using Sdl3Sharp.Internal.Interop;
using Sdl3Sharp.SourceGeneration;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Video.Windowing;

partial class ScreenSaver
{
	/// <summary>
	/// Prevents the screen from being blanked by a screen saver
	/// </summary>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// If you disable the screensaver, it is automatically re-enabled when SDL quits.
	/// </para>
	/// <para>
	/// The screensaver is disabled by default, but this may by changed by <see href="https://wiki.libsdl.org/SDL3/SDL_HINT_VIDEO_ALLOW_SCREENSAVER">SDL_HINT_VIDEO_ALLOW_SCREENSAVER</see>.
	/// </para>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_DisableScreenSaver">SDL_DisableScreenSaver</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal static partial CBool SDL_DisableScreenSaver();

	/// <summary>
	/// Allows the screen to be blanked by a screen saver
	/// </summary>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_EnableScreenSaver">SDL_EnableScreenSaver</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal static partial CBool SDL_EnableScreenSaver();

	/// <summary>
	/// Checks whether the screensaver is currently enabled
	/// </summary>
	/// <returns>Returns true if the screensaver is enabled, false if it is disabled</returns>
	/// <remarks>
	/// <para>
	/// The screensaver is disabled by default.
	/// </para>
	/// <para>
	/// The default can also be changed using <see href="https://wiki.libsdl.org/SDL3/SDL_HINT_VIDEO_ALLOW_SCREENSAVER"><c>SDL_HINT_VIDEO_ALLOW_SCREENSAVER</c></see>.
	/// </para>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_ScreenSaverEnabled">SDL_ScreenSaverEnabled</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal static partial CBool SDL_ScreenSaverEnabled();
}
