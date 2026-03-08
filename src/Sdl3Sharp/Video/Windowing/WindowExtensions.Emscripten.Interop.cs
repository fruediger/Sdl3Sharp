using Sdl3Sharp.Internal.Interop;
using Sdl3Sharp.SourceGeneration;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Video.Windowing;

partial class WindowExtensions
{
#if SDL3_4_0_OR_GREATER

	/// <summary>
	/// Sets the window to fill the current document space (Emscripten only)
	/// </summary>
	/// <param name="window">The window of which to change the fill-document state</param>
	/// <param name="fill">true to set the window to fill the document, false to disable</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// This will add or remove the window's <see href="https://wiki.libsdl.org/SDL3/SDL_WINDOW_FILL_DOCUMENT"><c>SDL_WINDOW_FILL_DOCUMENT</c></see> flag.
	/// </para>
	/// <para>
	/// Currently this flag only applies to the Emscripten target.
	/// </para>
	/// <para>
	/// When enabled, the canvas element fills the entire document.
	/// Resize events will be generated as the browser window is resized, as that will adjust the canvas size as well.
	/// The canvas will cover anything else on the page, including any controls provided by Emscripten in its generated HTML file (in fact, any elements on the page that aren't the canvas will be moved into a hidden div element).
	/// </para>
	/// <para>
	/// Often times this is desirable for a browser-based game, but it means several things that we expect of an SDL window on other platforms might not work as expected, such as minimum window sizes and aspect ratios.
	/// </para>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_SetWindowFillDocument">SDL_SetWindowFillDocument</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_SetWindowFillDocument(Window.SDL_Window* window, CBool fill);

#endif
}
