using Sdl3Sharp.Internal;
using Sdl3Sharp.Video.Windowing.Drivers;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Video.Windowing;

partial class WindowExtensions
{
	extension(Window<Cocoa>.PropertyNames)
	{
		public static string CreateCocoaWindowPointer => "SDL.window.create.cocoa.window";

		public static string CreateCocoaViewPointer => "SDL.window.create.cocoa.view";

		public static string CocoaWindowPointer => "SDL.window.cocoa.window";

		public static string CocoaMetalViewTagNumber => "SDL.window.cocoa.metal_view_tag";
	}

	extension(Window<Cocoa>)
	{
		public static bool TryCreate([NotNullWhen(true)] out Window<Cocoa>? window, bool? alwaysOnTop = default, bool? bordered = default, bool? constrainPopup = default, bool? externalGraphicsContext = default, bool? focusable = default,
			bool? fullscreen = default, int? height = default, bool? hidden = default, bool? highPixelDensity = default, bool? maximized = default, bool? menu = default, bool? metal = default, bool? minimized = default,
			bool? modal = default, bool? mouseGrabbed = default, bool? openGL = default, Window? parent = default, bool? resizable = default, string? title = default, bool? transparent = default, bool? tooltip = default,
			bool? utility = default, bool? vulkan = default, int? width = default, int? x = default, int? y = default,
			IntPtr? cocoaWindow = default, IntPtr? cocoaView = default, Properties? properties = default)
		{
			if (!Cocoa.IsActive)
			{
				window = null;
				return false;
			}

			Properties propertiesUsed;
			Unsafe.SkipInit(out IntPtr? cocoaWindowBackup);
			Unsafe.SkipInit(out IntPtr? cocoaViewBackup);

			if (properties is null)
			{
				propertiesUsed = [];

				if (cocoaWindow is IntPtr cocoaWindowValue)
				{
					propertiesUsed.TrySetPointerValue(Window<Cocoa>.PropertyNames.CreateCocoaWindowPointer, cocoaWindowValue);
				}

				if (cocoaView is IntPtr cocoaViewValue)
				{
					propertiesUsed.TrySetPointerValue(Window<Cocoa>.PropertyNames.CreateCocoaViewPointer, cocoaViewValue);
				}
			}
			else
			{
				propertiesUsed = properties;

				if (cocoaWindow is IntPtr cocoaWindowValue)
				{
					cocoaWindowBackup = propertiesUsed.TryGetPointerValue(Window<Cocoa>.PropertyNames.CreateCocoaWindowPointer, out var exisitingCocoaWindow) is true
						? exisitingCocoaWindow
						: null;

					propertiesUsed.TrySetPointerValue(Window<Cocoa>.PropertyNames.CreateCocoaWindowPointer, cocoaWindowValue);
				}

				if (cocoaView is IntPtr cocoaViewValue)
				{
					cocoaViewBackup = propertiesUsed.TryGetPointerValue(Window<Cocoa>.PropertyNames.CreateCocoaViewPointer, out var exisitingCocoaView) is true
						? exisitingCocoaView
						: null;

					propertiesUsed.TrySetPointerValue(Window<Cocoa>.PropertyNames.CreateCocoaViewPointer, cocoaViewValue);
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

					if (cocoaWindow.HasValue)
					{
						if (cocoaWindowBackup is IntPtr cocoaWindowValue)
						{
							propertiesUsed.TrySetPointerValue(Window<Cocoa>.PropertyNames.CreateCocoaWindowPointer, cocoaWindowValue);
						}
						else
						{
							propertiesUsed.TryRemove(Window<Cocoa>.PropertyNames.CreateCocoaWindowPointer);
						}
					}

					if (cocoaView.HasValue)
					{
						if (cocoaViewBackup is IntPtr cocoaViewValue)
						{
							propertiesUsed.TrySetPointerValue(Window<Cocoa>.PropertyNames.CreateCocoaViewPointer, cocoaViewValue);
						}
						else
						{
							propertiesUsed.TryRemove(Window<Cocoa>.PropertyNames.CreateCocoaViewPointer);
						}
					}
				}
			}
		}
	}

	extension(Window<Cocoa> window)
	{
		public IntPtr CocoaWindow => window?.Properties?.TryGetPointerValue(Window<Cocoa>.PropertyNames.CocoaWindowPointer, out var cocoaWindowPtr) is true
			? cocoaWindowPtr
			: default;

		public nint CocoaMetalViewTag => window?.Properties?.TryGetNumberValue(Window<Cocoa>.PropertyNames.CocoaMetalViewTagNumber, out var cocoaMetalViewTag) is true
			? unchecked((nint)cocoaMetalViewTag)
			: default;
	}
}
