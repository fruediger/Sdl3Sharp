using Sdl3Sharp.Video.Windowing.Drivers;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Video.Windowing;

partial class WindowExtensions
{
	extension(Window<Wayland>.PropertyNames)
	{
		public static string CreateWaylandSurfaceRoleCustomBoolean => "SDL.window.create.wayland.surface_role_custom";

		public static string CreateWaylandCreateEglWindowBoolean => "SDL.window.create.wayland.create_egl_window";

		public static string CreateWaylandWlSurfacePointer => "SDL.window.create.wayland.wl_surface";

		public static string WaylandDisplayPointer => "SDL.window.wayland.display";

		public static string WaylandSurfacePointer => "SDL.window.wayland.surface";

		public static string WaylandViewportPointer => "SDL.window.wayland.viewport";

		public static string WaylandEglWindowPointer => "SDL.window.wayland.egl_window";

		public static string WaylandXdgSurfacePointer => "SDL.window.wayland.xdg_surface";

		public static string WaylandXdgToplevelPointer => "SDL.window.wayland.xdg_toplevel";

		public static string WaylandXdgToplevelExportHandleString => "SDL.window.wayland.xdg_toplevel_export_handle";

		public static string WaylandXdgPopupPointer => "SDL.window.wayland.xdg_popup";

		public static string WaylandXdgPositionerPointer => "SDL.window.wayland.xdg_positioner";
	}

	extension(Window<Wayland>)
	{
		public static bool TryCreate([NotNullWhen(true)] out Window<Wayland>? window, bool? alwaysOnTop = default, bool? bordered = default, bool? constrainPopup = default, bool? externalGraphicsContext = default, bool? focusable = default,
			bool? fullscreen = default, int? height = default, bool? hidden = default, bool? highPixelDensity = default, bool? maximized = default, bool? menu = default, bool? metal = default, bool? minimized = default,
			bool? modal = default, bool? mouseGrabbed = default, bool? openGL = default, Window? parent = default, bool? resizable = default, string? title = default, bool? transparent = default, bool? tooltip = default,
			bool? utility = default, bool? vulkan = default, int? width = default, int? x = default, int? y = default,
			bool? waylandSurfaceRoleCustom = default, bool? waylandEglWindow = default, IntPtr? waylandWlSurface = default, Properties? properties = default)
		{
			if (!Wayland.IsActive)
			{
				window = null;
				return false;
			}

			Properties propertiesUsed;
			Unsafe.SkipInit(out bool? waylandSurfaceRoleCustomBackup);
			Unsafe.SkipInit(out bool? waylandEglWindowBackup);
			Unsafe.SkipInit(out IntPtr? waylandWlSurfaceBackup);

			if (properties is null)
			{
				propertiesUsed = [];

				if (waylandSurfaceRoleCustom is bool waylandSurfaceRoleCustomValue)
				{
					propertiesUsed.Add(Window<Wayland>.PropertyNames.CreateWaylandSurfaceRoleCustomBoolean, waylandSurfaceRoleCustomValue);
				}

				if (waylandEglWindow is bool waylandEglWindowValue)
				{
					propertiesUsed.Add(Window<Wayland>.PropertyNames.CreateWaylandCreateEglWindowBoolean, waylandEglWindowValue);
				}

				if (waylandWlSurface is IntPtr waylandWlSurfaceValue)
				{
					propertiesUsed.Add(Window<Wayland>.PropertyNames.CreateWaylandWlSurfacePointer, waylandWlSurfaceValue);
				}
			}
			else
			{
				propertiesUsed = properties;

				if (waylandSurfaceRoleCustom is bool waylandSurfaceRoleCustomValue)
				{
					waylandSurfaceRoleCustomBackup = propertiesUsed.TryGetBooleanValue(Window<Wayland>.PropertyNames.CreateWaylandSurfaceRoleCustomBoolean, out var existingWaylandSurfaceRoleCustom) is true
						? existingWaylandSurfaceRoleCustom
						: null;

					propertiesUsed.TrySetBooleanValue(Window<Wayland>.PropertyNames.CreateWaylandSurfaceRoleCustomBoolean, waylandSurfaceRoleCustomValue);
				}

				if (waylandEglWindow is bool waylandEglWindowValue)
				{
					waylandEglWindowBackup = propertiesUsed.TryGetBooleanValue(Window<Wayland>.PropertyNames.CreateWaylandCreateEglWindowBoolean, out var existingWaylandEglWindow) is true
						? existingWaylandEglWindow
						: null;

					propertiesUsed.TrySetBooleanValue(Window<Wayland>.PropertyNames.CreateWaylandCreateEglWindowBoolean, waylandEglWindowValue);
				}

				if (waylandWlSurface is IntPtr waylandWlSurfaceValue)
				{
					waylandWlSurfaceBackup = propertiesUsed.TryGetPointerValue(Window<Wayland>.PropertyNames.CreateWaylandWlSurfacePointer, out var existingWaylandWlSurface) is true
						? existingWaylandWlSurface
						: null;

					propertiesUsed.TrySetPointerValue(Window<Wayland>.PropertyNames.CreateWaylandWlSurfacePointer, waylandWlSurfaceValue);
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

					if (waylandSurfaceRoleCustom.HasValue)
					{
						if (waylandSurfaceRoleCustomBackup is bool waylandSurfaceRoleCustomValue)
						{
							propertiesUsed.TrySetBooleanValue(Window<Wayland>.PropertyNames.CreateWaylandSurfaceRoleCustomBoolean, waylandSurfaceRoleCustomValue);
						}
						else
						{
							propertiesUsed.TryRemove(Window<Wayland>.PropertyNames.CreateWaylandSurfaceRoleCustomBoolean);
						}
					}

					if (waylandEglWindow.HasValue)
					{
						if (waylandEglWindowBackup is bool waylandEglWindowValue)
						{
							propertiesUsed.TrySetBooleanValue(Window<Wayland>.PropertyNames.CreateWaylandCreateEglWindowBoolean, waylandEglWindowValue);
						}
						else
						{
							propertiesUsed.TryRemove(Window<Wayland>.PropertyNames.CreateWaylandCreateEglWindowBoolean);
						}
					}

					if (waylandWlSurface.HasValue)
					{
						if (waylandWlSurfaceBackup is IntPtr waylandWlSurfaceValue)
						{
							propertiesUsed.TrySetPointerValue(Window<Wayland>.PropertyNames.CreateWaylandWlSurfacePointer, waylandWlSurfaceValue);
						}
						else
						{
							propertiesUsed.TryRemove(Window<Wayland>.PropertyNames.CreateWaylandWlSurfacePointer);
						}
					}
				}
			}
		}
	}

	extension(Window<Wayland> window)
	{
		public IntPtr WaylandDisplay => window?.Properties?.TryGetPointerValue(Window<Wayland>.PropertyNames.WaylandDisplayPointer, out var waylandDisplayPtr) is true
			? waylandDisplayPtr
			: default;

		public IntPtr WaylandSurface => window?.Properties?.TryGetPointerValue(Window<Wayland>.PropertyNames.WaylandSurfacePointer, out var waylandSurfacePtr) is true
			? waylandSurfacePtr
			: default;

		public IntPtr WaylandViewport => window?.Properties?.TryGetPointerValue(Window<Wayland>.PropertyNames.WaylandViewportPointer, out var waylandViewportPtr) is true
			? waylandViewportPtr
			: default;

		public IntPtr WaylandEglWindow => window?.Properties?.TryGetPointerValue(Window<Wayland>.PropertyNames.WaylandEglWindowPointer, out var waylandEglWindowPtr) is true
			? waylandEglWindowPtr
			: default;

		public IntPtr WaylandXdgSurface => window?.Properties?.TryGetPointerValue(Window<Wayland>.PropertyNames.WaylandXdgSurfacePointer, out var waylandXdgSurfacePtr) is true
			? waylandXdgSurfacePtr
			: default;

		public IntPtr WaylandXdgToplevel => window?.Properties?.TryGetPointerValue(Window<Wayland>.PropertyNames.WaylandXdgToplevelPointer, out var waylandXdgToplevelPtr) is true
			? waylandXdgToplevelPtr
			: default;

		public string? WaylandXdgToplevelExportHandle => window?.Properties?.TryGetStringValue(Window<Wayland>.PropertyNames.WaylandXdgToplevelExportHandleString, out var waylandXdgToplevelExportHandleStr) is true
			? waylandXdgToplevelExportHandleStr
			: default;

		public IntPtr WaylandXdgPopup => window?.Properties?.TryGetPointerValue(Window<Wayland>.PropertyNames.WaylandXdgPopupPointer, out var waylandXdgPopupPtr) is true
			? waylandXdgPopupPtr
			: default;

		public IntPtr WaylandXdgPositioner => window?.Properties?.TryGetPointerValue(Window<Wayland>.PropertyNames.WaylandXdgPositionerPointer, out var waylandXdgPositionerPtr) is true
			? waylandXdgPositionerPtr
			: default;
	}
}
