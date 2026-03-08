using Sdl3Sharp.Video.Rendering;
using Sdl3Sharp.Video.Windowing.Drivers;
using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Video.Windowing;

public sealed partial class Window<TDriver> : Window
	where TDriver : notnull, IWindowingDriver
{
	internal unsafe Window(SDL_Window* window, bool register) :
		base(window, register)
	{ }

	private protected override Display GetDisplayImpl() => Display;

	public new Display<TDriver> Display
	{
		get
		{
			unsafe
			{
				if (!Display<TDriver>.TryGetOrCreate(SDL_GetDisplayForWindow(Pointer), out var display))
				{
					// This very rarely should fail, SDL tries it's very best to find a reasonable display to associate with the window,
					// and even goes as far as to return the primary display if it can't find the one the window is actually on (even for offscreen windows).
					// This pretty much only fails on a system without any connected displays.

					failCouldNotGetDisplay();
				}

				return display;
			}

			[DoesNotReturn]
			static void failCouldNotGetDisplay() => throw new SdlException("Couldn't get the display for the window");
		}
	}

	internal static bool TryCreate([NotNullWhen(true)] out Window<TDriver>? window, bool? alwaysOnTop = default, bool? bordered = default, bool? constrainPopup = default, bool? externalGraphicsContext = default, bool? focusable = default,
		bool? fullscreen = default, int? height = default, bool? hidden = default, bool? highPixelDensity = default, bool? maximized = default, bool? menu = default, bool? metal = default, bool? minimized = default,
		bool? modal = default, bool? mouseGrabbed = default, bool? openGL = default, Window? parent = default, bool? resizable = default, string? title = default, bool? transparent = default, bool? tooltip = default,
		bool? utility = default, bool? vulkan = default, int? width = default, int? x = default, int? y = default, Properties? properties = default)
		=> TryCreateUnchecked(
			out window, alwaysOnTop, bordered, constrainPopup, externalGraphicsContext, focusable,
			fullscreen, height, hidden, highPixelDensity, maximized, menu, metal, minimized,
			modal, mouseGrabbed, openGL, parent, resizable, title, transparent, tooltip,
			utility, vulkan, width, x, y, properties
		);

	private protected override bool TryCreatePopupImpl(int x, int y, int width, int height, [NotNullWhen(true)] out Window? popupWindow, WindowFlags flags = default)
	{
		var result = TryCreatePopup(x, y, width, height, out Window<TDriver>? typedPopupWindow, flags);

		popupWindow = typedPopupWindow;

		return result;
	}

	// See Video/Rendering/Renderer_TDriver.cs for an explanation for the following pattern:

	// Hide the non-specialized base method in favor of a strongly-typed overload and divert the user to it
	/// <summary>Use <see cref="TryCreatePopup(int, int, int, int, out Window{TDriver}?, WindowFlags)"/> instead</summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	[OverloadResolutionPriority(-1)]
	[Obsolete(
		$"Use {nameof(TryCreatePopup)}(int, int, int, int, out {nameof(Window<>)}<{nameof(TDriver)}>?, {nameof(WindowFlags)}) instead.",
		error: true
	)]
	public new bool TryCreatePopup(int x, int y, int width, int height, [NotNullWhen(true)] out Window? popupWindow, WindowFlags flags = default)
		=> base.TryCreatePopup(x, y, width, height, out popupWindow, flags);

	public bool TryCreatePopup(int x, int y, int width, int height, [NotNullWhen(true)] out Window<TDriver>? popupWindow, WindowFlags flags = default)
	{
		unsafe
		{
			var windowPtr = SDL_CreatePopupWindow(Pointer, x, y, width, height, flags);

			if (windowPtr is null)
			{
				popupWindow = null;
				return false;
			}

			popupWindow = new(windowPtr, register: true);
			return true;
		}
	}

	internal unsafe static bool TryGetOrCreate(SDL_Window* window, [NotNullWhen(true)] out Window<TDriver>? result)
		=> Window.TryGetOrCreate(window, out result);
}
