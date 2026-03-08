using Sdl3Sharp.Video.Windowing.Drivers;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Video.Windowing;

partial class WindowExtensions
{
	extension(Window<Windows>.PropertyNames)
	{
		public static string CreateWindowsHWndPointer => "SDL.window.create.win32.hwnd";

		public static string CreateWindowsPixelFormatHWndPointer => "SDL.window.create.win32.pixel_format_hwnd";

		public static string WindowsHWndPointer => "SDL.window.win32.hwnd";

		public static string WindowsHDcPointer => "SDL.window.win32.hdc";

		public static string WindowsInstancePointer => "SDL.window.win32.instance";
	}

	extension(Window<Windows>)
	{
		public static bool TryCreate([NotNullWhen(true)] out Window<Windows>? window, bool? alwaysOnTop = default, bool? bordered = default, bool? constrainPopup = default, bool? externalGraphicsContext = default, bool? focusable = default,
			bool? fullscreen = default, int? height = default, bool? hidden = default, bool? highPixelDensity = default, bool? maximized = default, bool? menu = default, bool? metal = default, bool? minimized = default,
			bool? modal = default, bool? mouseGrabbed = default, bool? openGL = default, Window? parent = default, bool? resizable = default, string? title = default, bool? transparent = default, bool? tooltip = default,
			bool? utility = default, bool? vulkan = default, int? width = default, int? x = default, int? y = default,
			IntPtr? windowsHWnd = default, IntPtr? windowsPixelFormatHWnd = default, Properties? properties = default)
		{
			if (!Windows.IsActive)
			{
				window = null;
				return false;
			}

			Properties propertiesUsed;
			Unsafe.SkipInit(out IntPtr? windowsHWndBackup);
			Unsafe.SkipInit(out IntPtr? windowsPixelFormatHWndBackup);

			if (properties is null)
			{
				propertiesUsed = [];

				if (windowsHWnd is IntPtr windowsHWndValue)
				{
					propertiesUsed.TrySetPointerValue(Window<Windows>.PropertyNames.CreateWindowsHWndPointer, windowsHWndValue);
				}

				if (windowsPixelFormatHWnd is IntPtr windowsPixelFormatHWndValue)
				{
					propertiesUsed.TrySetPointerValue(Window<Windows>.PropertyNames.CreateWindowsPixelFormatHWndPointer, windowsPixelFormatHWndValue);
				}
			}
			else
			{
				propertiesUsed = properties;

				if (windowsHWnd is IntPtr windowsHWndValue)
				{
					windowsHWndBackup = propertiesUsed.TryGetPointerValue(Window<Windows>.PropertyNames.CreateWindowsHWndPointer, out var existingWindowsHWnd) is true
						? existingWindowsHWnd
						: null;

					propertiesUsed.TrySetPointerValue(Window<Windows>.PropertyNames.CreateWindowsHWndPointer, windowsHWndValue);
				}

				if (windowsPixelFormatHWnd is IntPtr windowsPixelFormatHWndValue)
				{
					windowsPixelFormatHWndBackup = propertiesUsed.TryGetPointerValue(Window<Windows>.PropertyNames.CreateWindowsPixelFormatHWndPointer, out var existingWindowsPixelFormatHWnd) is true
						? existingWindowsPixelFormatHWnd
						: null;

					propertiesUsed.TrySetPointerValue(Window<Windows>.PropertyNames.CreateWindowsPixelFormatHWndPointer, windowsPixelFormatHWndValue);
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

					if (windowsHWnd.HasValue)
					{
						if (windowsHWndBackup is IntPtr windowsHWndValue)
						{
							propertiesUsed.TrySetPointerValue(Window<Windows>.PropertyNames.CreateWindowsHWndPointer, windowsHWndValue);
						}
						else
						{
							propertiesUsed.TryRemove(Window<Windows>.PropertyNames.CreateWindowsHWndPointer);
						}
					}

					if (windowsPixelFormatHWnd.HasValue)
					{
						if (windowsPixelFormatHWndBackup is IntPtr windowsPixelFormatHWndValue)
						{
							propertiesUsed.TrySetPointerValue(Window<Windows>.PropertyNames.CreateWindowsPixelFormatHWndPointer, windowsPixelFormatHWndValue);
						}
						else
						{
							propertiesUsed.TryRemove(Window<Windows>.PropertyNames.CreateWindowsPixelFormatHWndPointer);
						}
					}
				}
			}
		}
	}

	extension(Window<Windows> window)
	{
		public IntPtr WindowsHWnd => window?.Properties?.TryGetPointerValue(Window<Windows>.PropertyNames.WindowsHWndPointer, out var windowsHWndPtr) is true
			? windowsHWndPtr
			: default;

		public IntPtr WindowsHDc => window?.Properties?.TryGetPointerValue(Window<Windows>.PropertyNames.WindowsHDcPointer, out var windowsHDcPtr) is true
			? windowsHDcPtr
			: default;

		public IntPtr WindowsInstance => window?.Properties?.TryGetPointerValue(Window<Windows>.PropertyNames.WindowsInstancePointer, out var windowsInstancePtr) is true
			? windowsInstancePtr
			: default;
	}
}
