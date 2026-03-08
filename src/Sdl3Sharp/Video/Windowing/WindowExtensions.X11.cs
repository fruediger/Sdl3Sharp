using Sdl3Sharp.Video.Windowing.Drivers;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Video.Windowing;

partial class WindowExtensions
{
	extension(Window<X11>.PropertyNames)
	{
		public static string CreateX11WindowNumber => "SDL.window.create.x11.window";

		public static string X11DisplayPointer => "SDL.window.x11.display";

		public static string X11ScreenNumber => "SDL.window.x11.screen";

		public static string X11WindowNumber => "SDL.window.x11.window";
	}

	extension(Window<X11>)
	{
		public static bool TryCreate([NotNullWhen(true)] out Window<Windows>? window, bool? alwaysOnTop = default, bool? bordered = default, bool? constrainPopup = default, bool? externalGraphicsContext = default, bool? focusable = default,
			bool? fullscreen = default, int? height = default, bool? hidden = default, bool? highPixelDensity = default, bool? maximized = default, bool? menu = default, bool? metal = default, bool? minimized = default,
			bool? modal = default, bool? mouseGrabbed = default, bool? openGL = default, Window? parent = default, bool? resizable = default, string? title = default, bool? transparent = default, bool? tooltip = default,
			bool? utility = default, bool? vulkan = default, int? width = default, int? x = default, int? y = default,
			uint? x11Window = default, Properties? properties = default)
		{
			if (!X11.IsActive)
			{
				window = null;
				return false;
			}

			Properties propertiesUsed;
			Unsafe.SkipInit(out uint? x11WindowBackup);

			if (properties is null)
			{
				propertiesUsed = [];

				if (x11Window is uint x11WindowValue)
				{
					propertiesUsed.TrySetNumberValue(Window<X11>.PropertyNames.CreateX11WindowNumber, x11WindowValue);
				}
			}
			else
			{
				propertiesUsed = properties;

				if (x11Window is uint x11WindowValue)
				{
					x11WindowBackup = propertiesUsed.TryGetNumberValue(Window<X11>.PropertyNames.CreateX11WindowNumber, out var existingX11Window)
						? unchecked((uint)existingX11Window)
						: null;

					propertiesUsed.TrySetNumberValue(Window<X11>.PropertyNames.CreateX11WindowNumber, x11WindowValue);
				}
			}

			try
			{
				return Window.TryCreateUnchecked(
					out window, alwaysOnTop, bordered, constrainPopup, externalGraphicsContext, focusable,
					fullscreen, height, hidden, highPixelDensity, maximized, menu, metal, minimized,
					modal, mouseGrabbed, openGL, parent, resizable, title, transparent, tooltip,
					utility, vulkan, width, x, y, propertiesUsed
				);
			}
			finally
			{
				if (properties is null)
				{
					// propertiesUsed was just a temporary instance we created for this call, so we need to dispose it now

					propertiesUsed.Dispose();
				}
				else
				{
					// we restored the original properties values from the given properties instance

					if (x11Window.HasValue)
					{
						if (x11WindowBackup is uint x11WindowValue)
						{
							propertiesUsed.TrySetNumberValue(Window<X11>.PropertyNames.CreateX11WindowNumber, x11WindowValue);
						}
						else
						{
							propertiesUsed.TryRemove(Window<X11>.PropertyNames.CreateX11WindowNumber);
						}
					}
				}
			}
		}
	}

	extension(Window<X11> window)
	{
		public IntPtr X11Display => window?.Properties?.TryGetPointerValue(Window<X11>.PropertyNames.X11DisplayPointer, out var x11DisplayPtr) is true
			? x11DisplayPtr
			: default;

		public int X11Screen => window?.Properties?.TryGetNumberValue(Window<X11>.PropertyNames.X11ScreenNumber, out var x11Screen) is true
			? (int)x11Screen
			: default;

		public uint X11Window => window?.Properties?.TryGetNumberValue(Window<X11>.PropertyNames.X11WindowNumber, out var x11Window) is true
			? (uint)x11Window
			: default;
	}
}
