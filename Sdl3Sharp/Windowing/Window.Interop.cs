using Sdl3Sharp.Interop;
using Sdl3Sharp.SourceGeneration;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sdl3Sharp.Windowing;

partial class Window
{
	// Used for opaque pointers
	[StructLayout(LayoutKind.Sequential, Size = 0)]
	internal readonly struct SDL_Window;

	/// <summary>
	/// Create a child popup window of the specified parent window
	/// </summary>
	/// <param name="parent">the parent of the window, must not be NULL</param>
	/// <param name="offset_x">the x position of the popup window relative to the origin of the parent</param>
	/// <param name="offset_y">the y position of the popup window relative to the origin of the parent window</param>
	/// <param name="w">the width of the window</param>
	/// <param name="h">the height of the window</param>
	/// <param name="flags"><see href="https://wiki.libsdl.org/SDL3/SDL_WINDOW_TOOLTIP">SDL_WINDOW_TOOLTIP</see> or <see href="https://wiki.libsdl.org/SDL3/SDL_WINDOW_POPUP_MENU">SDL_WINDOW_POPUP_MENU</see>, and zero or more additional <see href="https://wiki.libsdl.org/SDL3/SDL_WindowFlags">SDL_WindowFlags</see> OR'd togethe</param>
	/// <returns>Returns the window that was created or NULL on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information.</returns>
	/// <remarks>
	/// </remarks>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial SDL_Window* SDL_CreatePopupWindow(SDL_Window* parent, int offset_x, int offset_y, int w, int h, WindowFlags flags);
}
