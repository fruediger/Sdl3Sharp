using Sdl3Sharp.Internal.Interop;
using Sdl3Sharp.SourceGeneration;
using Sdl3Sharp.Video.Drawing;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Video.Windowing;

partial class WindowSurface
{
	/// <summary>
	/// Destroys the surface associated with the window
	/// </summary>
	/// <param name="window">The window to update</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_DestroyWindowSurface">SDL_DestroyWindowSurface</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_DestroyWindowSurface(Window.SDL_Window* window);

	/// <summary>
	/// Gets the SDL surface associated with the window
	/// </summary>
	/// <param name="window">The window to query</param>
	/// <returns>Returns the surface associated with the window, or NULL on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information.</returns>
	/// <remarks>
	/// <para>
	/// A new surface will be created with the optimal format for the window, if necessary. This surface will be freed when the window is destroyed. Do not free this surface.
	/// </para>
	/// <para>
	/// This surface will be invalidated if the window is resized. After resizing a window this function must be called again to return a valid surface.
	/// </para>
	/// <para>
	/// You may not combine this with 3D or the rendering API on this window.
	/// </para>
	/// <para>
	/// This function is affected by <see href="https://wiki.libsdl.org/SDL3/SDL_HINT_FRAMEBUFFER_ACCELERATION"><c>SDL_HINT_FRAMEBUFFER_ACCELERATION</c></see>.
	/// </para>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetWindowSurface">SDL_GetWindowSurface</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial SDL_Surface* SDL_GetWindowSurface(Window.SDL_Window* window);

	/// <summary>
	/// Gets VSync for the window surface
	/// </summary>
	/// <param name="window">The window to query</param>
	/// <param name="vsync">An int filled with the current vertical refresh sync interval. <see href="https://wiki.libsdl.org/SDL3/SDL_SetWindowSurfaceVSync">See SDL_SetWindowSurfaceVSync</see>() for the meaning of the value.</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetWindowSurfaceVSync">SDL_GetWindowSurfaceVSync</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_GetWindowSurfaceVSync(Window.SDL_Window* window, WindowSurfaceVSync* vsync);

	/// <summary>
	/// Toggles VSync for the window surface
	/// </summary>
	/// <param name="window">The window</param>
	/// <param name="vsync">The vertical refresh sync interval.</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// When a window surface is created, vsync defaults to <see href="https://wiki.libsdl.org/SDL3/SDL_WINDOW_SURFACE_VSYNC_DISABLED">SDL_WINDOW_SURFACE_VSYNC_DISABLED</see>.
	/// </para>
	/// <para>
	/// The <c><paramref name="vsync"/></c> parameter can be 1 to synchronize present with every vertical refresh, 2 to synchronize present with every second vertical refresh, etc.,
	/// <see href="https://wiki.libsdl.org/SDL3/SDL_WINDOW_SURFACE_VSYNC_ADAPTIVE">SDL_WINDOW_SURFACE_VSYNC_ADAPTIVE</see> for late swap tearing (adaptive vsync),
	/// or <see href="https://wiki.libsdl.org/SDL3/SDL_WINDOW_SURFACE_VSYNC_DISABLED">SDL_WINDOW_SURFACE_VSYNC_DISABLED</see> to disable.
	/// Not every value is supported by every driver, so you should check the return value to see whether the requested setting is supported.
	/// </para>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_SetWindowSurfaceVSync">SDL_SetWindowSurfaceVSync</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_SetWindowSurfaceVSync(Window.SDL_Window* window, WindowSurfaceVSync vsync);

	/// <summary>
	/// Copies the window surface to the screen
	/// </summary>
	/// <param name="window">The window to update</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// This is the function you use to reflect any changes to the surface on the screen.
	/// </para>
	/// <para>
	/// This function is equivalent to the SDL 1.2 API <see href="https://wiki.libsdl.org/SDL3/SDL_Flip">SDL_Flip</see>().
	/// </para>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_UpdateWindowSurface">SDL_UpdateWindowSurface</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_UpdateWindowSurface(Window.SDL_Window* window);

	/// <summary>
	/// Copies areas of the window surface to the screen
	/// </summary>
	/// <param name="window">The window to update</param>
	/// <param name="rects">An array of <see href="https://wiki.libsdl.org/SDL3/SDL_Rect">SDL_Rect</see> structures representing areas of the surface to copy, in pixels</param>
	/// <param name="numrects">The number of rectangles</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// This is the function you use to reflect changes to portions of the surface on the screen.
	/// </para>
	/// <para>
	/// This function is equivalent to the SDL 1.2 API <see href="https://wiki.libsdl.org/SDL3/SDL_UpdateRects">SDL_UpdateRects</see>().
	/// </para>
	/// <para>
	/// Note that this function will update <em>at least</em> the rectangles specified, but this is only intended as an optimization;
	/// in practice, this might update more of the screen (or all of the screen!), depending on what method SDL uses to send pixels to the system.
	/// </para>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_UpdateWindowSurfaceRects">SDL_UpdateWindowSurfaceRects</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_UpdateWindowSurfaceRects(Window.SDL_Window* window, Rect<int>* rects, int numrects);
}
