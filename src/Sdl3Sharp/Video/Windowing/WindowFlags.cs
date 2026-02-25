using Sdl3Sharp.Video.Windowing.Drivers;
using System;

namespace Sdl3Sharp.Video.Windowing;

/// <summary>
/// Represents the flags of a <see cref="Window"/>
/// </summary>
[Flags]
public enum WindowFlags : ulong
{
	/// <summary>The <see cref="Window"/> is in fullscreen mode</summary>
	Fullscreen        = 0b0000_0000_0000_0000_0000_0000_0000_0001ul, // 0x0000000000000001ul

	/// <summary>The <see cref="Window"/> is usable with an OpenGL context</summary>
	OpenGL            = 0b0000_0000_0000_0000_0000_0000_0000_0010ul, // 0x0000000000000002ul

	/// <summary>The <see cref="Window"/> is occluded</summary>
	Occluded          = 0b0000_0000_0000_0000_0000_0000_0000_0100ul, // 0x0000000000000004ul

	/// <summary> The <see cref="Window"/> is neither mapped onto the desktop nor shown in the taskbar/dock/window list; <see cref="SDL_ShowWindow"/>() is required for it to become visible</summary>
	Hidden            = 0b0000_0000_0000_0000_0000_0000_0000_1000ul, // 0x0000000000000008ul

	/// <summary>The <see cref="Window"/> has no window decoration</summary>
	Borderless        = 0b0000_0000_0000_0000_0000_0000_0001_0000ul, // 0x0000000000000010ul

	/// <summary>The <see cref="Window"/> can be resized</summary>
	Resizable         = 0b0000_0000_0000_0000_0000_0000_0010_0000ul, // 0x0000000000000020ul

	/// <summary>The <see cref="Window"/> is minimized</summary>
	Minimized         = 0b0000_0000_0000_0000_0000_0000_0100_0000ul, // 0x0000000000000040ul

	/// <summary>The <see cref="Window"/> is maximized</summary>
	Maximized         = 0b0000_0000_0000_0000_0000_0000_1000_0000ul, // 0x0000000000000080ul

	/// <summary>The <see cref="Window"/> has grabbed mouse input</summary>
	MouseGrabbed      = 0b0000_0000_0000_0000_0000_0001_0000_0000ul, // 0x0000000000000100ul

	/// <summary>The <see cref="Window"/> has input focus</summary>
	InputFocus        = 0b0000_0000_0000_0000_0000_0010_0000_0000ul, // 0x0000000000000200ul

	/// <summary>The <see cref="Window"/> has mouse focus</summary>
	MouseFocuse       = 0b0000_0000_0000_0000_0000_0100_0000_0000ul, // 0x0000000000000400ul

	/// <summary>The <see cref="Window"/> was not created by SDL</summary>
	External          = 0b0000_0000_0000_0000_0000_1000_0000_0000ul, // 0x0000000000000800ul

	/// <summary>The <see cref="Window"/> is modal</summary>
	Modal             = 0b0000_0000_0000_0000_0001_0000_0000_0000ul, // 0x0000000000001000ul

	/// <summary>The <see cref="Window"/>  uses a high pixel density back buffer if possible</summary>
	HighPixelDensity  = 0b0000_0000_0000_0000_0010_0000_0000_0000ul, // 0x0000000000002000ul

	/// <summary>The <see cref="Window"/> has the mouse captured (this unrelated to <see cref="MouseGrabbed"/>)</summary>
	MouseCapture      = 0b0000_0000_0000_0000_0100_0000_0000_0000ul, // 0x0000000000004000ul

	/// <summary>The <see cref="Window"/> has mouse relative mode enabled</summary>
	MouseRelativeMode = 0b0000_0000_0000_0000_1000_0000_0000_0000ul, // 0x0000000000008000ul

	/// <summary>The <see cref="Window"/> should always be above others</summary>
	AlwaysOnTop       = 0b0000_0000_0000_0001_0000_0000_0000_0000ul, // 0x0000000000010000ul

	/// <summary>The <see cref="Window"/> should be treated as a utility window, not showing in the task bar and window list</summary>
	Utility           = 0b0000_0000_0000_0010_0000_0000_0000_0000ul, // 0x0000000000020000ul

	/// <summary>The <see cref="Window"/> should be treated as a tooltip and does not get mouse or keyboard focus, requiring a parent window</summary>
	Tooltip           = 0b0000_0000_0000_0100_0000_0000_0000_0000ul, // 0x0000000000040000ul

	/// <summary>The <see cref="Window"/> should be treated as a popup menu, requiring a parent window</summary>
	PopupMenu         = 0b0000_0000_0000_1000_0000_0000_0000_0000ul, // 0x0000000000080000ul

	/// <summary>The <see cref="Window"/> has grabbed keyboard input</summary>
	KeyboardGrabbed   = 0b0000_0000_0001_0000_0000_0000_0000_0000ul, // 0x0000000000100000ul

#if SDL3_4_0_OR_GREATER

	/// <summary>The <see cref="Window"/> is in fill-document mode (<see cref="Emscripten"/> only)</summary>
	FillDocument      = 0b0000_0000_0010_0000_0000_0000_0000_0000ul, // 0x0000000000200000ul

#endif

	/// <summary>The <see cref="Window"/> is usable for a Vulkan surface</summary>
	Vulkan            = 0b0001_0000_0000_0000_0000_0000_0000_0000ul, // 0x0000000010000000ul

	/// <summary>The <see cref="Window"/> is usable for a Metal view</summary>
	Metal             = 0b0000_0010_0000_0000_0000_0000_0000_0000ul, // 0x0000000020000000ul

	/// <summary>The <see cref="Window"/> has a transparent buffer</summary>
	Transparent       = 0b0000_0100_0000_0000_0000_0000_0000_0000ul, // 0x0000000040000000ul

	/// <summary>The <see cref="Window"/> should not be focusable</summary>
	NotFocusable      = 0b1000_0000_0000_0000_0000_0000_0000_0000ul, // 0x0000000080000000ul
}
