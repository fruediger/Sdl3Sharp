using Sdl3Sharp.Video.Windowing.Drivers;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Video.Windowing;

partial class WindowExtensions
{
	extension(Window<Wayland>.PropertyNames)
	{
		/// <summary>
		/// The name of a <see cref="TryCreate(out Window{Wayland}?, bool?, bool?, bool?, bool?, WindowFlags?, bool?, bool?, int?, bool?, bool?, bool?, bool?, bool?, bool?, bool?, bool?, bool?, Window?, bool?, string?, bool?, bool?, bool?, bool?, int?, WindowPosition?, WindowPosition?, bool?, bool?, nint?, Properties?)">property used when creating a <see cref="Window{TDriver}">Window&lt;<see cref="Wayland">Wayland</see>&gt;</see></see>
		/// that holds a value indicating whether the application wants to use the Wayland surface for a custom role and does not want it to be attached to an XDG top-level window
		/// </summary>
		/// <remarks>
		/// <para>
		/// See <see href="https://wiki.libsdl.org/SDL3/README-wayland">README-wayland</see> for more information on using custom surfaces.
		/// </para>
		/// </remarks>
		public static string CreateWaylandSurfaceRoleCustomBoolean => "SDL.window.create.wayland.surface_role_custom";

		/// <summary>
		/// The name of a <see cref="TryCreate(out Window{Wayland}?, bool?, bool?, bool?, bool?, WindowFlags?, bool?, bool?, int?, bool?, bool?, bool?, bool?, bool?, bool?, bool?, bool?, bool?, Window?, bool?, string?, bool?, bool?, bool?, bool?, int?, WindowPosition?, WindowPosition?, bool?, bool?, nint?, Properties?)">property used when creating a <see cref="Window{TDriver}">Window&lt;<see cref="Wayland">Wayland</see>&gt;</see></see>
		/// that holds a value indicating whether the application wants an associated <c>wl_egl_window</c> object to be created and attached to the window, even if the window creation does not have the <c>openGL</c> parameter set to <c><see langword="true"/></c> or the <see cref="WindowFlags.OpenGL"/> flag set
		/// </summary>
		public static string CreateWaylandCreateEGLWindowBoolean => "SDL.window.create.wayland.create_egl_window";

		/// <summary>
		/// The name of a <see cref="TryCreate(out Window{Wayland}?, bool?, bool?, bool?, bool?, WindowFlags?, bool?, bool?, int?, bool?, bool?, bool?, bool?, bool?, bool?, bool?, bool?, bool?, Window?, bool?, string?, bool?, bool?, bool?, bool?, int?, WindowPosition?, WindowPosition?, bool?, bool?, nint?, Properties?)">property used when creating a <see cref="Window{TDriver}">Window&lt;<see cref="Wayland">Wayland</see>&gt;</see></see>
		/// that holds pointer to the <c><see href="https://wayland.freedesktop.org/docs/html/apa.html#protocol-spec-wl_surface">wl_surface</see></c> associated with the window, if you want to wrap an existing window
		/// </summary>
		/// <remarks>
		/// <para>
		/// See <see href="https://wiki.libsdl.org/SDL3/README-wayland">README-wayland</see> for more information.
		/// </para>
		/// </remarks>
		public static string CreateWaylandWlSurfacePointer => "SDL.window.create.wayland.wl_surface";

		/// <summary>
		/// The name of a <em>read-only</em> <see cref="Window.Properties">property</see> that holds
		/// a pointer to the <c><see href="https://wayland.freedesktop.org/docs/html/apa.html#protocol-spec-wl_display">wl_display</see></c> associated with the window
		/// </summary>
		public static string WaylandDisplayPointer => "SDL.window.wayland.display";

		/// <summary>
		/// The name of a <em>read-only</em> <see cref="Window.Properties">property</see> that holds
		/// a pointer to the <c><see href="https://wayland.freedesktop.org/docs/html/apa.html#protocol-spec-wl_surface">wl_surface</see></c> associated with the window
		/// </summary>
		public static string WaylandSurfacePointer => "SDL.window.wayland.surface";

		/// <summary>
		/// The name of a <em>read-only</em> <see cref="Window.Properties">property</see> that holds
		/// a pointer to the <c>wp_viewport</c> associated with the window
		/// </summary>
		public static string WaylandViewportPointer => "SDL.window.wayland.viewport";

		/// <summary>
		/// The name of a <em>read-only</em> <see cref="Window.Properties">property</see> that holds
		/// a pointer to the <c>wl_egl_window</c> associated with the window
		/// </summary>
		public static string WaylandEglWindowPointer => "SDL.window.wayland.egl_window";

		/// <summary>
		/// The name of a <em>read-only</em> <see cref="Window.Properties">property</see> that holds
		/// a pointer to the <c>xdg_surface</c> associated with the window
		/// </summary>
		/// <remarks>
		/// <para>
		/// The value of the associated property refers to an <c>xgd_*</c> object, and as such they do <em>not</em> persist across <see cref="Window.TryShow">showing</see> or <see cref="Window.TryHide">hiding</see> the window.
		/// The value of the associated property will be the <c><see langword="null"/></c>-pointer if the window is hidden, and must be requeried once the window is shown again.
		/// </para>
		/// </remarks>
		public static string WaylandXdgSurfacePointer => "SDL.window.wayland.xdg_surface";

		/// <summary>
		/// The name of a <em>read-only</em> <see cref="Window.Properties">property</see> that holds
		/// a pointer to the <c>xdg_toplevel</c> associated with the window
		/// </summary>
		/// <remarks>
		/// <para>
		/// The value of the associated property refers to an <c>xgd_*</c> object, and as such they do <em>not</em> persist across <see cref="Window.TryShow">showing</see> or <see cref="Window.TryHide">hiding</see> the window.
		/// The value of the associated property will be the <c><see langword="null"/></c>-pointer if the window is hidden, and must be requeried once the window is shown again.
		/// </para>
		/// </remarks>
		public static string WaylandXdgToplevelPointer => "SDL.window.wayland.xdg_toplevel";

		/// <summary>
		/// The name of a <em>read-only</em> <see cref="Window.Properties">property</see> that holds
		/// the export handle associated with the window's <c>xdg_toplevel</c>
		/// </summary>
		public static string WaylandXdgToplevelExportHandleString => "SDL.window.wayland.xdg_toplevel_export_handle";

		/// <summary>
		/// The name of a <em>read-only</em> <see cref="Window.Properties">property</see> that holds
		/// a pointer to the <c>xdg_popup</c> role associated with the window
		/// </summary>
		/// <remarks>
		/// <para>
		/// The value of the associated property refers to an <c>xgd_*</c> object, and as such they do <em>not</em> persist across <see cref="Window.TryShow">showing</see> or <see cref="Window.TryHide">hiding</see> the window.
		/// The value of the associated property will be the <c><see langword="null"/></c>-pointer if the window is hidden, and must be requeried once the window is shown again.
		/// </para>
		/// </remarks>
		public static string WaylandXdgPopupPointer => "SDL.window.wayland.xdg_popup";

		/// <summary>
		/// The name of a <em>read-only</em> <see cref="Window.Properties">property</see> that holds
		/// a pointer to the <c>xdg_positioner</c> associated with the window
		/// </summary>
		/// <remarks>
		/// <para>
		/// The value of the associated property refers to an <c>xgd_*</c> object, and as such they do <em>not</em> persist across <see cref="Window.TryShow">showing</see> or <see cref="Window.TryHide">hiding</see> the window.
		/// The value of the associated property will be the <c><see langword="null"/></c>-pointer if the window is hidden, and must be requeried once the window is shown again.
		/// </para>
		/// </remarks>
		public static string WaylandXdgPositionerPointer => "SDL.window.wayland.xdg_positioner";
	}

	extension(Window<Wayland>)
	{
		/// <inheritdoc cref="Window.TryCreate(out Window?, bool?, bool?, bool?, bool?, WindowFlags?, bool?, bool?, int?, bool?, bool?, bool?, bool?, bool?, bool?, bool?, bool?, bool?, Window?, bool?, string?, bool?, bool?, bool?, bool?, int?, WindowPosition?, WindowPosition?, Properties?)"/>
		/// <param name="waylandSurfaceRoleCustom">
		/// A value indicating whether the application wants to use the Wayland surface for a custom role and does not want it to be attached to an XDG top-level window.
		/// See <see href="https://wiki.libsdl.org/SDL3/README-wayland">README-wayland</see> for more information on using custom surfaces.
		/// </param>
		/// <param name="waylandEGLWindow">
		/// A value indicating whether the application wants an associated <c>wl_egl_window</c> object to be created and attached to the window, even if the window creation does not have the <paramref name="openGL"/> parameter set to <c><see langword="true"/></c> or the <see cref="WindowFlags.OpenGL"/> flag set
		/// </param>
		/// <param name="waylandWlSurface">
		/// A pointer to the <c><see href="https://wayland.freedesktop.org/docs/html/apa.html#protocol-spec-wl_surface">wl_surface</see></c> associated with the window, if you want to wrap an existing window.
		/// Must be directly cast to an <see cref="IntPtr"/> from a <c>wl_surface*</c> pointer.
		/// See <see href="https://wiki.libsdl.org/SDL3/README-wayland">README-wayland</see> for more information.
		/// </param>
#pragma warning disable CS1573 // we get these from inheritdoc
		public static bool TryCreate([NotNullWhen(true)] out Window<Wayland>? window, bool? alwaysOnTop = default, bool? bordered = default, bool? constrainPopup = default, bool? externalGraphicsContext = default, WindowFlags? flags = default,
			bool? focusable = default, bool? fullscreen = default, int? height = default, bool? hidden = default, bool? highPixelDensity = default, bool? maximized = default, bool? menu = default, bool? metal = default,
			bool? minimized = default, bool? modal = default, bool? mouseGrabbed = default, bool? openGL = default, Window? parent = default, bool? resizable = default, string? title = default, bool? transparent = default,
			bool? tooltip = default, bool? utility = default, bool? vulkan = default, int? width = default, WindowPosition? x = default, WindowPosition? y = default,
			bool? waylandSurfaceRoleCustom = default, bool? waylandEGLWindow = default, IntPtr? waylandWlSurface = default, Properties? properties = default)
#pragma warning restore CS1573
		{
			if (!Wayland.IsActive)
			{
				window = null;
				return false;
			}

			Properties propertiesUsed;
			Unsafe.SkipInit(out bool? waylandSurfaceRoleCustomBackup);
			Unsafe.SkipInit(out bool? waylandEGLWindowBackup);
			Unsafe.SkipInit(out IntPtr? waylandWlSurfaceBackup);

			if (properties is null)
			{
				propertiesUsed = [];

				if (waylandSurfaceRoleCustom is bool waylandSurfaceRoleCustomValue)
				{
					propertiesUsed.Add(Window<Wayland>.PropertyNames.CreateWaylandSurfaceRoleCustomBoolean, waylandSurfaceRoleCustomValue);
				}

				if (waylandEGLWindow is bool waylandEglWindowValue)
				{
					propertiesUsed.Add(Window<Wayland>.PropertyNames.CreateWaylandCreateEGLWindowBoolean, waylandEglWindowValue);
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

				if (waylandEGLWindow is bool waylandEglWindowValue)
				{
					waylandEGLWindowBackup = propertiesUsed.TryGetBooleanValue(Window<Wayland>.PropertyNames.CreateWaylandCreateEGLWindowBoolean, out var existingWaylandEGLWindow) is true
						? existingWaylandEGLWindow
						: null;

					propertiesUsed.TrySetBooleanValue(Window<Wayland>.PropertyNames.CreateWaylandCreateEGLWindowBoolean, waylandEglWindowValue);
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
					out window, alwaysOnTop, bordered, constrainPopup, externalGraphicsContext, flags,
					focusable, fullscreen, height, hidden, highPixelDensity, maximized, menu, metal,
					minimized, modal, mouseGrabbed, openGL, parent, resizable, title, transparent,
					tooltip, utility, vulkan, width, x, y, propertiesUsed
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

					if (waylandEGLWindow.HasValue)
					{
						if (waylandEGLWindowBackup is bool waylandEglWindowValue)
						{
							propertiesUsed.TrySetBooleanValue(Window<Wayland>.PropertyNames.CreateWaylandCreateEGLWindowBoolean, waylandEglWindowValue);
						}
						else
						{
							propertiesUsed.TryRemove(Window<Wayland>.PropertyNames.CreateWaylandCreateEGLWindowBoolean);
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
		/// <summary>
		/// Gets a pointer to the <c><see href="https://wayland.freedesktop.org/docs/html/apa.html#protocol-spec-wl_display">wl_display</see></c> associated with this window
		/// </summary>
		/// <value>
		/// A pointer to the <c><see href="https://wayland.freedesktop.org/docs/html/apa.html#protocol-spec-wl_display">wl_display</see></c> associated with this window
		/// </value>
		/// <remarks>
		/// <para>
		/// The value of this property can be directly cast to an <c><see href="https://wayland.freedesktop.org/docs/html/apa.html#protocol-spec-wl_display">wl_display*</see></c> pointer.
		/// </para>
		/// <para>
		/// This property should only be accessed from the main thread.
		/// </para>
		/// </remarks>
		public IntPtr WaylandDisplay => window?.Properties?.TryGetPointerValue(Window<Wayland>.PropertyNames.WaylandDisplayPointer, out var waylandDisplayPtr) is true
			? waylandDisplayPtr
			: default;

		/// <summary>
		/// Gets a pointer to the <c><see href="https://wayland.freedesktop.org/docs/html/apa.html#protocol-spec-wl_surface">wl_surface</see></c> associated with this window
		/// </summary>
		/// <value>
		/// A pointer to the <c><see href="https://wayland.freedesktop.org/docs/html/apa.html#protocol-spec-wl_surface">wl_surface</see></c> associated with this window
		/// </value>
		/// <remarks>
		/// <para>
		/// The value of this property can be directly cast to an <c><see href="https://wayland.freedesktop.org/docs/html/apa.html#protocol-spec-wl_surface">wl_surface*</see></c> pointer.
		/// </para>
		/// <para>
		/// This property should only be accessed from the main thread.
		/// </para>
		/// </remarks>
		public IntPtr WaylandSurface => window?.Properties?.TryGetPointerValue(Window<Wayland>.PropertyNames.WaylandSurfacePointer, out var waylandSurfacePtr) is true
			? waylandSurfacePtr
			: default;

		/// <summary>
		/// Gets a pointer to the <c>wp_viewport</c> associated with this window
		/// </summary>
		/// <value>
		/// A pointer to the <c>wp_viewport</c> associated with this window
		/// </value>
		/// <remarks>
		/// <para>
		/// The value of this property can be directly cast to an <c>wp_viewport*</c> pointer.
		/// </para>
		/// <para>
		/// This property should only be accessed from the main thread.
		/// </para>
		/// </remarks>
		public IntPtr WaylandViewport => window?.Properties?.TryGetPointerValue(Window<Wayland>.PropertyNames.WaylandViewportPointer, out var waylandViewportPtr) is true
			? waylandViewportPtr
			: default;

		/// <summary>
		/// Gets a pointer to the <c>wl_egl_window</c> associated with this window
		/// </summary>
		/// <value>
		/// A pointer to the <c>wl_egl_window</c> associated with this window
		/// </value>
		/// <remarks>
		/// <para>
		/// The value of this property can be directly cast to an <c>wl_egl_window*</c> pointer.
		/// </para>
		/// <para>
		/// This property should only be accessed from the main thread.
		/// </para>
		/// </remarks>
		public IntPtr WaylandEGLWindow => window?.Properties?.TryGetPointerValue(Window<Wayland>.PropertyNames.WaylandEglWindowPointer, out var waylandEGLWindowPtr) is true
			? waylandEGLWindowPtr
			: default;

		/// <summary>
		/// Gets a pointer to the <c>xdg_surface</c> associated with this window
		/// </summary>
		/// <value>
		/// A pointer to the <c>xdg_surface</c> associated with this window
		/// </value>
		/// <remarks>
		/// <para>
		/// The value of this property can be directly cast to an <c>xdg_surface*</c> pointer.
		/// </para>
		/// <para>
		/// The value of the this property refers to an <c>xgd_*</c> object, and as such they do <em>not</em> persist across <see cref="Window.TryShow">showing</see> or <see cref="Window.TryHide">hiding</see> the window.
		/// The value of the this property will be the <c><see langword="null"/></c>-pointer if the window is hidden, and must be requeried once the window is shown again.
		/// </para>
		/// <para>
		/// This property should only be accessed from the main thread.
		/// </para>
		/// </remarks>
		public IntPtr WaylandXdgSurface => window?.Properties?.TryGetPointerValue(Window<Wayland>.PropertyNames.WaylandXdgSurfacePointer, out var waylandXdgSurfacePtr) is true
			? waylandXdgSurfacePtr
			: default;

		/// <summary>
		/// Gets a pointer to the <c>xdg_toplevel</c> associated with this window
		/// </summary>
		/// <value>
		/// A pointer to the <c>xdg_toplevel</c> associated with this window
		/// </value>
		/// <remarks>
		/// <para>
		/// The value of this property can be directly cast to an <c>xdg_toplevel*</c> pointer.
		/// </para>
		/// <para>
		/// The value of the this property refers to an <c>xgd_*</c> object, and as such they do <em>not</em> persist across <see cref="Window.TryShow">showing</see> or <see cref="Window.TryHide">hiding</see> the window.
		/// The value of the this property will be the <c><see langword="null"/></c>-pointer if the window is hidden, and must be requeried once the window is shown again.
		/// </para>
		/// <para>
		/// This property should only be accessed from the main thread.
		/// </para>
		/// </remarks>
		public IntPtr WaylandXdgToplevel => window?.Properties?.TryGetPointerValue(Window<Wayland>.PropertyNames.WaylandXdgToplevelPointer, out var waylandXdgToplevelPtr) is true
			? waylandXdgToplevelPtr
			: default;

		/// <summary>
		/// Gets the export handle associated with the window's <c>xdg_toplevel</c>
		/// </summary>
		/// <value>
		/// The export handle associated with the window's <c>xdg_toplevel</c>
		/// </value>
		/// <remarks>
		/// <para>
		/// This property should only be accessed from the main thread.
		/// </para>
		/// </remarks>
		public string? WaylandXdgToplevelExportHandle => window?.Properties?.TryGetStringValue(Window<Wayland>.PropertyNames.WaylandXdgToplevelExportHandleString, out var waylandXdgToplevelExportHandleStr) is true
			? waylandXdgToplevelExportHandleStr
			: default;

		/// <summary>
		/// Gets a pointer to the <c>xdg_popup</c> role associated with this window
		/// </summary>
		/// <value>
		/// A pointer to the <c>xdg_popup</c> role associated with this window
		/// </value>
		/// <remarks>
		/// <para>
		/// The value of this property can be directly cast to an <c>xdg_popup*</c> pointer.
		/// </para>
		/// <para>
		/// The value of the this property refers to an <c>xgd_*</c> object, and as such they do <em>not</em> persist across <see cref="Window.TryShow">showing</see> or <see cref="Window.TryHide">hiding</see> the window.
		/// The value of the this property will be the <c><see langword="null"/></c>-pointer if the window is hidden, and must be requeried once the window is shown again.
		/// </para>
		/// <para>
		/// This property should only be accessed from the main thread.
		/// </para>
		/// </remarks>
		public IntPtr WaylandXdgPopup => window?.Properties?.TryGetPointerValue(Window<Wayland>.PropertyNames.WaylandXdgPopupPointer, out var waylandXdgPopupPtr) is true
			? waylandXdgPopupPtr
			: default;

		/// <summary>
		/// Gets a pointer to the <c>xdg_positioner</c> associated with this window
		/// </summary>
		/// <value>
		/// A pointer to the <c>xdg_positioner</c> associated with this window
		/// </value>
		/// <remarks>
		/// <para>
		/// The value of this property can be directly cast to an <c>xdg_positioner*</c> pointer.
		/// </para>
		/// <para>
		/// The value of the this property refers to an <c>xgd_*</c> object, and as such they do <em>not</em> persist across <see cref="Window.TryShow">showing</see> or <see cref="Window.TryHide">hiding</see> the window.
		/// The value of the this property will be the <c><see langword="null"/></c>-pointer if the window is hidden, and must be requeried once the window is shown again.
		/// </para>
		/// <para>
		/// This property should only be accessed from the main thread.
		/// </para>
		/// </remarks>
		public IntPtr WaylandXdgPositioner => window?.Properties?.TryGetPointerValue(Window<Wayland>.PropertyNames.WaylandXdgPositionerPointer, out var waylandXdgPositionerPtr) is true
			? waylandXdgPositionerPtr
			: default;
	}
}
