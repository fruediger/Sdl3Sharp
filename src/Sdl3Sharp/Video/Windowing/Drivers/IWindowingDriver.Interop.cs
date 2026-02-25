using Sdl3Sharp.Internal.Interop;
using Sdl3Sharp.SourceGeneration;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Video.Windowing.Drivers;

partial interface IWindowingDriver
{
	/// <summary>
	/// Get the name of the currently initialized video driver
	/// </summary>
	/// <returns>Returns the name of the current video driver or NULL if no driver has been initialized</returns>
	/// <remarks>
	/// <para>
	/// The names of drivers are all simple, low-ASCII identifiers, like "cocoa", "x11" or "windows". These never have Unicode characters, and are not meant to be proper names.
	/// </para>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetCurrentVideoDriver">SDL_GetCurrentVideoDriver</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial byte* SDL_GetCurrentVideoDriver();

	/// <summary>
	/// Get the number of video drivers compiled into SDL
	/// </summary>
	/// <returns>Returns the number of built in video drivers</returns>
	/// <remarks>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetNumVideoDrivers">SDL_GetNumVideoDrivers</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal static partial int SDL_GetNumVideoDrivers();

	/// <summary>
	/// Get the name of a built in video driver
	/// </summary>
	/// <param name="index">The index of a video driver</param>
	/// <returns>Returns the name of the video driver with the given <paramref name="index"/>, or NULL if index is out of bounds</returns>
	/// <remarks>
	/// <para>
	/// The video drivers are presented in the order in which they are normally checked during initialization.
	/// </para>
	/// <para>
	/// The names of drivers are all simple, low-ASCII identifiers, like "cocoa", "x11" or "windows". These never have Unicode characters, and are not meant to be proper names.
	/// </para>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetVideoDriver">SDL_GetVideoDriver</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial byte* SDL_GetVideoDriver(int index);
}
