using Sdl3Sharp.Video.Windowing.Drivers;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Video.Windowing;

partial class WindowExtensions
{
	extension(Window<X11>.PropertyNames)
	{
		/// <summary>
		/// The name of a <see cref="TryCreate(out Window{X11}?, bool?, bool?, bool?, bool?, WindowFlags?, bool?, bool?, int?, bool?, bool?, bool?, bool?, bool?, bool?, bool?, bool?, bool?, Window?, bool?, string?, bool?, bool?, bool?, bool?, int?, WindowPosition?, WindowPosition?, uint?, Properties?)">property used when creating a <see cref="Window{TDriver}">Window&lt;<see cref="X11">X11</see>&gt;</see></see>
		/// that holds the ID of a X11 Window associated with the window, if you want to wrap an existing window
		/// </summary>
		public static string CreateX11WindowNumber => "SDL.window.create.x11.window";

		/// <summary>
		/// The name of a <em>read-only</em> <see cref="Window.Properties">property</see> that holds
		/// a pointer to the X11 Display associated with the window
		/// </summary>
		public static string X11DisplayPointer => "SDL.window.x11.display";

		/// <summary>
		/// The name of a <em>read-only</em> <see cref="Window.Properties">property</see> that holds
		/// the X11 screen number associated with the window
		/// </summary>
		public static string X11ScreenNumber => "SDL.window.x11.screen";

		/// <summary>
		/// The name of a <em>read-only</em> <see cref="Window.Properties">property</see> that holds
		/// the ID of the X11 Window associated with the window
		/// </summary>
		public static string X11WindowNumber => "SDL.window.x11.window";
	}

	extension(Window<X11>)
	{
		/// <inheritdoc cref="Window.TryCreate(out Window?, bool?, bool?, bool?, bool?, WindowFlags?, bool?, bool?, int?, bool?, bool?, bool?, bool?, bool?, bool?, bool?, bool?, bool?, Window?, bool?, string?, bool?, bool?, bool?, bool?, int?, WindowPosition?, WindowPosition?, Properties?)"/>
		/// <param name="x11Window">The ID of a X11 Window to associate with the created window, if you want to wrap an existing window</param>
#pragma warning disable CS1573 // we get these from inheritdoc
		public static bool TryCreate([NotNullWhen(true)] out Window<X11>? window, bool? alwaysOnTop = default, bool? bordered = default, bool? constrainPopup = default, bool? externalGraphicsContext = default, WindowFlags? flags = default,
			bool? focusable = default, bool? fullscreen = default, int? height = default, bool? hidden = default, bool? highPixelDensity = default, bool? maximized = default, bool? menu = default, bool? metal = default,
			bool? minimized = default, bool? modal = default, bool? mouseGrabbed = default, bool? openGL = default, Window? parent = default, bool? resizable = default, string? title = default, bool? transparent = default,
			bool? tooltip = default, bool? utility = default, bool? vulkan = default, int? width = default, WindowPosition? x = default, WindowPosition? y = default,
			uint? x11Window = default, Properties? properties = default)
#pragma warning restore CS1573
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
		/// <summary>
		/// Gets a pointer to the X11 Display associated with this window
		/// </summary>
		/// <value>
		/// A pointer to the X11 Display associated with this window
		/// </value>
		/// <remarks>
		/// <para>
		/// This property should only be accessed from the main thread.
		/// </para>
		/// </remarks>
		public IntPtr X11Display => window?.Properties?.TryGetPointerValue(Window<X11>.PropertyNames.X11DisplayPointer, out var x11DisplayPtr) is true
			? x11DisplayPtr
			: default;

		/// <summary>
		/// Gets the X11 screen number associated with this window
		/// </summary>
		/// <value>
		/// The X11 screen number associated with this window
		/// </value>
		/// <remarks>
		/// <para>
		/// This property should only be accessed from the main thread.
		/// </para>
		/// </remarks>
		public int X11Screen => window?.Properties?.TryGetNumberValue(Window<X11>.PropertyNames.X11ScreenNumber, out var x11Screen) is true
			? (int)x11Screen
			: default;

		/// <summary>
		/// Gets the ID of the X11 Window associated with this window
		/// </summary>
		/// <value>
		/// The ID of the X11 Window associated with this window
		/// </value>
		/// <remarks>
		/// <para>
		/// This property should only be accessed from the main thread.
		/// </para>
		/// </remarks>
		public uint X11Window => window?.Properties?.TryGetNumberValue(Window<X11>.PropertyNames.X11WindowNumber, out var x11Window) is true
			? (uint)x11Window
			: default;
	}
}
