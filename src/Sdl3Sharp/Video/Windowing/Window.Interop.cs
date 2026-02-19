using Sdl3Sharp.Internal.Interop;
using Sdl3Sharp.SourceGeneration;
using Sdl3Sharp.Video.Rendering;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sdl3Sharp.Video.Windowing;

partial class Window
{
	// Used for opaque pointers
	[StructLayout(LayoutKind.Sequential, Size = 0)]
	internal readonly struct SDL_Window;

	// TODO: IMPLEMENT!
	/*
	/// <summary>
	/// Create a window and default renderer
	/// </summary>
	/// <param name="title">The title of the window, in UTF-8 encoding</param>
	/// <param name="width">The width of the window</param>
	/// <param name="height">The height of the window</param>
	/// <param name="window_flags">The flags used to create the window (see <see href="https://wiki.libsdl.org/SDL3/SDL_CreateWindow">SDL_CreateWindow</see>())</param>
	/// <param name="window">A pointer filled with the window, or NULL on error</param>
	/// <param name="renderer">A pointer filled with the renderer, or NULL on error</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_CreateWindowAndRenderer">SDL_CreateWindowAndRenderer</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial SDL_Window* SDL_CreateWindowAndRenderer(byte* title, int width, int height, WindowFlags window_flags, SDL_Window** window, IRenderer.SDL_Renderer** renderer);
	*/

	/// <summary>
	/// Get the renderer associated with a window
	/// </summary>
	/// <param name="window">The window to query</param>
	/// <returns>Returns the rendering context on success or NULL on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetRenderer">SDL_GetRenderer</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial IRenderer.SDL_Renderer* SDL_GetRenderer(SDL_Window* window);
}
