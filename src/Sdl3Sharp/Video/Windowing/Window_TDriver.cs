using Sdl3Sharp.Video.Windowing.Drivers;
using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Video.Windowing;

/// <summary>
/// Represents a window
/// </summary>
/// <remarks>
/// <para>
/// A window is used to display content on the screen and receive input from the user.
/// </para>
/// <para>
/// To display content in a window, you can either use a <see cref="Rendering.Renderer"/>,
/// constructed from any of the <c>TryCreateRenderer*</c> methods (e.g. <see cref="Window.TryCreateRenderer(out Rendering.Renderer?, ReadOnlySpan{string})"/>
/// or from the <see cref="Window.TryCreateWithRenderer(string, int, int, out Window?, out Rendering.Renderer?, WindowFlags)"/> methods,
/// or use the <see cref="Window.Surface"/> property to get a <see cref="WindowSurface"/> you can draw on.
/// Using either one mutually excludes the other.
/// </para>
/// <para>
/// If you use a <see cref="Rendering.Renderer"/> in conjunction with a <see cref="Window{TDriver}"/>, please make sure that you don't use it after the window has been <see cref="Window.Dispose()">disposed</see>.
/// The recommended approach is to <see cref="Rendering.Renderer.Dispose()">dispose</see> the renderer before the window.
/// </para>
/// <para>
/// Windows can be either top-level or be children of other windows. To make a window a child of another window, you just need to set its <see cref="Window.Parent"/> property.
/// </para>
/// <para>
/// Special rules apply for child windows: they will be hidden with their parent and will be destroyed when their parent gets destroyed.
/// </para>
/// <para>
/// You can create new <see cref="Window{TDriver}"/> using the <see cref="TryCreate(out Sdl3Sharp.Video.Windowing.Window{TDriver}?, bool?, bool?, bool?, bool?, Sdl3Sharp.Video.Windowing.WindowFlags?, bool?, bool?, int?, bool?, bool?, bool?, bool?, bool?, bool?, bool?, bool?, bool?, Sdl3Sharp.Video.Windowing.Window?, bool?, string?, bool?, bool?, bool?, bool?, int?, Sdl3Sharp.Video.Windowing.WindowPosition?, Sdl3Sharp.Video.Windowing.WindowPosition?, Sdl3Sharp.Properties?)"/>
/// method, but be aware that if the currently active <see cref="IWindowingDriver">windowing driver</see> doesn't match the specified one, this method will fail.
/// Alternatively, you can create new popup windows using the <see cref="TryCreatePopup(int, int, int, int, out Sdl3Sharp.Video.Windowing.Window{TDriver}?, Sdl3Sharp.Video.Windowing.WindowFlags)"/> method.
/// This method will create the new window as a child window of the window the method was called on.
/// </para>
/// <para>
/// Additionally, there are some driver-specific methods for creating windows, such as
/// <see cref="WindowExtensions.TryCreate(out Sdl3Sharp.Video.Windowing.Window{Wayland}?, bool?, bool?, bool?, bool?, Sdl3Sharp.Video.Windowing.WindowFlags?, bool?, bool?, int?, bool?, bool?, bool?, bool?, bool?, bool?, bool?, bool?, bool?, Sdl3Sharp.Video.Windowing.Window?, bool?, string?, bool?, bool?, bool?, bool?, int?, Sdl3Sharp.Video.Windowing.WindowPosition?, Sdl3Sharp.Video.Windowing.WindowPosition?, bool?, bool?, nint?, Sdl3Sharp.Properties?)">Wayland</see>,
/// <see cref="WindowExtensions.TryCreate(out Sdl3Sharp.Video.Windowing.Window{Windows}?, bool?, bool?, bool?, bool?, Sdl3Sharp.Video.Windowing.WindowFlags?, bool?, bool?, int?, bool?, bool?, bool?, bool?, bool?, bool?, bool?, bool?, bool?, Sdl3Sharp.Video.Windowing.Window?, bool?, string?, bool?, bool?, bool?, bool?, int?, Sdl3Sharp.Video.Windowing.WindowPosition?, Sdl3Sharp.Video.Windowing.WindowPosition?, nint?, nint?, Sdl3Sharp.Properties?)">Windows</see>,
/// <see cref="WindowExtensions.TryCreate(out Sdl3Sharp.Video.Windowing.Window{Cocoa}?, bool?, bool?, bool?, bool?, Sdl3Sharp.Video.Windowing.WindowFlags?, bool?, bool?, int?, bool?, bool?, bool?, bool?, bool?, bool?, bool?, bool?, bool?, Sdl3Sharp.Video.Windowing.Window?, bool?, string?, bool?, bool?, bool?, bool?, int?, Sdl3Sharp.Video.Windowing.WindowPosition?, Sdl3Sharp.Video.Windowing.WindowPosition?, nint?, nint?, Sdl3Sharp.Properties?)">Cocoa</see>,
/// and some more.
/// A similar rules apply to these methods as well: if the currently active <see cref="IWindowingDriver">windowing driver</see> doesn't match the specified one, these method will fail.
/// </para>
/// <para>
/// For the most part <see cref="Window{TDriver}"/>s are not thread-safe, and most of their properties and methods should only be accessed from the main thread!
/// </para>
/// <para>
/// <see cref="Window{TDriver}"/>s are concrete display types, associated with a specific windowing driver.
/// </para>
/// <para>
/// If you want to use them in a more general way, you can use them as <see cref="Window"/> instances, which serve as common abstractions.
/// </para>
/// </remarks>
public sealed partial class Window<TDriver> : Window
	where TDriver : notnull, IWindowingDriver
{
	internal unsafe Window(SDL_Window* window, bool register) :
		base(window, register)
	{ }

	private protected override Display GetDisplayImpl() => Display;

	/// <inheritdoc cref="Window.Display"/>
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

	/// <inheritdoc cref="Window.TryCreate{TDriver}(out Window{TDriver}?, bool?, bool?, bool?, bool?, WindowFlags?, bool?, bool?, int?, bool?, bool?, bool?, bool?, bool?, bool?, bool?, bool?, bool?, Window?, bool?, string?, bool?, bool?, bool?, bool?, int?, WindowPosition?, WindowPosition?, Properties?)"/>
	public static bool TryCreate([NotNullWhen(true)] out Window<TDriver>? window, bool? alwaysOnTop = default, bool? bordered = default, bool? constrainPopup = default, bool? externalGraphicsContext = default, WindowFlags? flags = default,
		bool? focusable = default, bool? fullscreen = default, int? height = default, bool? hidden = default, bool? highPixelDensity = default, bool? maximized = default, bool? menu = default, bool? metal = default,
		bool? minimized = default, bool? modal = default, bool? mouseGrabbed = default, bool? openGL = default, Window? parent = default, bool? resizable = default, string? title = default, bool? transparent = default,
		bool? tooltip = default, bool? utility = default, bool? vulkan = default, int? width = default, WindowPosition? x = default, WindowPosition? y = default, Properties? properties = default)
		=> Window.TryCreate(
			out window, alwaysOnTop, bordered, constrainPopup, externalGraphicsContext, flags,
			focusable, fullscreen, height, hidden, highPixelDensity, maximized, menu, metal,
			minimized, modal, mouseGrabbed, openGL, parent, resizable, title, transparent,
			tooltip, utility, vulkan, width, x, y, properties
		);

	internal static bool TryCreateUnchecked([NotNullWhen(true)] out Window<TDriver>? window, bool? alwaysOnTop = default, bool? bordered = default, bool? constrainPopup = default, bool? externalGraphicsContext = default, WindowFlags? flags = default,
		bool? focusable = default, bool? fullscreen = default, int? height = default, bool? hidden = default, bool? highPixelDensity = default, bool? maximized = default, bool? menu = default, bool? metal = default,
		bool? minimized = default, bool? modal = default, bool? mouseGrabbed = default, bool? openGL = default, Window? parent = default, bool? resizable = default, string? title = default, bool? transparent = default,
		bool? tooltip = default, bool? utility = default, bool? vulkan = default, int? width = default, WindowPosition? x = default, WindowPosition? y = default, Properties? properties = default)
		=> Window.TryCreateUnchecked(
			out window, alwaysOnTop, bordered, constrainPopup, externalGraphicsContext, flags,
			focusable, fullscreen, height, hidden, highPixelDensity, maximized, menu, metal,
			minimized, modal, mouseGrabbed, openGL, parent, resizable, title, transparent,
			tooltip, utility, vulkan, width, x, y, properties
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

	/// <inheritdoc cref="Window.TryCreatePopup(int, int, int, int, out Window?, WindowFlags)"/>
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
