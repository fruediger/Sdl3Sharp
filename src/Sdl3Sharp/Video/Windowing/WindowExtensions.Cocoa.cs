using Sdl3Sharp.Video.Windowing.Drivers;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Video.Windowing;

partial class WindowExtensions
{
	extension(Window<Cocoa>.PropertyNames)
	{
		/// <summary>
		/// The name of a <see cref="TryCreate(out Window{Cocoa}?, bool?, bool?, bool?, bool?, WindowFlags?, bool?, bool?, int?, bool?, bool?, bool?, bool?, bool?, bool?, bool?, bool?, bool?, Window?, bool?, string?, bool?, bool?, bool?, bool?, int?, WindowPosition?, WindowPosition?, nint?, nint?, Properties?)">property used when creating a <see cref="Window{TDriver}">Window&lt;<see cref="Cocoa">Cocoa</see>&gt;</see></see>
		/// that holds a pointer to the <c>(__unsafe_unretained) <see href="https://developer.apple.com/documentation/appkit/nswindow">NSWindow</see></c> associated with the window, if you want to wrap an existing window
		/// </summary>
		public static string CreateCocoaWindowPointer => "SDL.window.create.cocoa.window";

		/// <summary>
		/// The name of a <see cref="TryCreate(out Window{Cocoa}?, bool?, bool?, bool?, bool?, WindowFlags?, bool?, bool?, int?, bool?, bool?, bool?, bool?, bool?, bool?, bool?, bool?, bool?, Window?, bool?, string?, bool?, bool?, bool?, bool?, int?, WindowPosition?, WindowPosition?, nint?, nint?, Properties?)">property used when creating a <see cref="Window{TDriver}">Window&lt;<see cref="Cocoa">Cocoa</see>&gt;</see></see>
		/// that holds a pointer to the <c>(__unsafe_unretained) <see href="https://developer.apple.com/documentation/appkit/nsview">NSView</see></c> associated with the window
		/// </summary>
		/// <remarks>
		/// <para>
		/// The value of the associated property defaults to <c>[window contentView]</c>.
		/// </para>
		/// </remarks>
		public static string CreateCocoaViewPointer => "SDL.window.create.cocoa.view";

		/// <summary>
		/// The name of a <em>read-only</em> <see cref="Window.Properties">property</see> that holds
		/// a pointer to the<c>(__unsafe_unretained) <see href="https://developer.apple.com/documentation/appkit/nswindow">NSWindow</see></c> associated with the window
		/// </summary>
		public static string CocoaWindowPointer => "SDL.window.cocoa.window";

		/// <summary>
		/// The name of a <em>read-only</em> <see cref="Window.Properties">property</see> that holds
		/// the <c><see href="https://developer.apple.com/documentation/objectivec/nsinteger">NSInteger</see></c> tag associated with the window's Metal view
		/// </summary>
		public static string CocoaMetalViewTagNumber => "SDL.window.cocoa.metal_view_tag";
	}

	extension(Window<Cocoa>)
	{
		/// <inheritdoc cref="Window.TryCreate(out Window?, bool?, bool?, bool?, bool?, WindowFlags?, bool?, bool?, int?, bool?, bool?, bool?, bool?, bool?, bool?, bool?, bool?, bool?, Window?, bool?, string?, bool?, bool?, bool?, bool?, int?, WindowPosition?, WindowPosition?, Properties?)"/>
		/// <param name="cocoaWindow">
		/// A pointer to the <c>(__unsafe_unretained) <see href="https://developer.apple.com/documentation/appkit/nswindow">NSWindow</see></c> to associate with the created window, if you want to wrap an existing window.
		/// Must be directly cast to an <see cref="IntPtr"/> from a <c>(__unsafe_unretained) <see href="https://developer.apple.com/documentation/appkit/nswindow">NSWindow</see>*</c> pointer.
		/// </param>
		/// <param name="cocoaView">
		/// A pointer to the <c>(__unsafe_unretained) <see href="https://developer.apple.com/documentation/appkit/nsview">NSView</see></c> to associate with the created window.
		/// Must be directly cast to an <see cref="IntPtr"/> from a <c>(__unsafe_unretained) <see href="https://developer.apple.com/documentation/appkit/nsview">NSView</see>*</c> pointer.
		/// If this parameter is not provided or <c><see langword="null"/></c> (the default), the associated view will default to <c>[window contentView]</c>.
		/// </param>
#pragma warning disable CS1573 // we get these from inheritdoc
		public static bool TryCreate([NotNullWhen(true)] out Window<Cocoa>? window, bool? alwaysOnTop = default, bool? bordered = default, bool? constrainPopup = default, bool? externalGraphicsContext = default, WindowFlags? flags = default,
			bool? focusable = default, bool? fullscreen = default, int? height = default, bool? hidden = default, bool? highPixelDensity = default, bool? maximized = default, bool? menu = default, bool? metal = default,
			bool? minimized = default, bool? modal = default, bool? mouseGrabbed = default, bool? openGL = default, Window? parent = default, bool? resizable = default, string? title = default, bool? transparent = default,
			bool? tooltip = default, bool? utility = default, bool? vulkan = default, int? width = default, WindowPosition? x = default, WindowPosition? y = default,
			IntPtr? cocoaWindow = default, IntPtr? cocoaView = default, Properties? properties = default)
#pragma warning restore CS1573
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
		/// <summary>
		/// Gets a pointer to the <c>(__unsafe_unretained) <see href="https://developer.apple.com/documentation/appkit/nswindow">NSWindow</see></c> associated with this window
		/// </summary>
		/// <value>
		/// A pointer to the <c>(__unsafe_unretained) <see href="https://developer.apple.com/documentation/appkit/nswindow">NSWindow</see></c> associated with this window
		/// </value>
		/// <remarks>
		/// <para>
		/// The value of this property can be directly cast to an <c>(__unsafe_unretained) <see href="https://developer.apple.com/documentation/appkit/nswindow">NSWindow</see>*</c> pointer.
		/// </para>
		/// <para>
		/// This property should only be accessed from the main thread.
		/// </para>
		/// </remarks>
		public IntPtr CocoaWindow => window?.Properties?.TryGetPointerValue(Window<Cocoa>.PropertyNames.CocoaWindowPointer, out var cocoaWindowPtr) is true
			? cocoaWindowPtr
			: default;

		/// <summary>
		/// Gets the <c><see href="https://developer.apple.com/documentation/objectivec/nsinteger">NSInteger</see></c> tag associated with the window's Metal view
		/// </summary>
		/// <value>
		/// The <c><see href="https://developer.apple.com/documentation/objectivec/nsinteger">NSInteger</see></c> tag associated with the window's Metal view
		/// </value>
		/// <remarks>
		/// <para>
		/// The value of this property can be directly cast to a <c><see href="https://developer.apple.com/documentation/objectivec/nsinteger">NSInteger</see></c>.
		/// </para>
		/// <para>
		/// This property should only be accessed from the main thread.
		/// </para>
		/// </remarks>
		public nint CocoaMetalViewTag => window?.Properties?.TryGetNumberValue(Window<Cocoa>.PropertyNames.CocoaMetalViewTagNumber, out var cocoaMetalViewTag) is true
			? unchecked((nint)cocoaMetalViewTag)
			: default;
	}
}
