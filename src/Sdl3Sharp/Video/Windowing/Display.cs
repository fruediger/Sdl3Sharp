using Sdl3Sharp.Events;
using Sdl3Sharp.Internal;
using Sdl3Sharp.Utilities;
using Sdl3Sharp.Video.Drawing;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace Sdl3Sharp.Video.Windowing;

/// <summary>
/// Represents a display
/// </summary>
/// <remarks>
/// <para>
/// The <see cref="Id"/> of a display is unique, remains unchanged while the display is connected to the system, and is never reused for the lifetime of the application.
/// If a display is disconnected and then reconnected, it will get assigned a new <see cref="Id"/>.
/// </para>
/// </remarks>
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
[StructLayout(LayoutKind.Sequential)]
public readonly partial struct Display :
	IEquatable<Display>, IFormattable, ISpanFormattable, IEqualityOperators<Display, Display, bool>
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly string DebuggerDisplay => ToString(formatProvider: CultureInfo.InvariantCulture);

	private readonly uint mDisplayId;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	internal Display(uint id) => mDisplayId = id;

	/// <summary>
	/// Gets the desktop area represented by this display
	/// </summary>
	/// <value>
	/// The desktop area represented by this display
	/// </value>
	/// <remarks>
	/// <para>
	/// The primary display is often located at the origin (0, 0) in virtual screen space, but may be placed differently depending on the monitor layout.
	/// </para>
	/// <para>
	/// This property should only be accessed from the main thread.
	/// </para>
	/// </remarks>
	/// <exception cref="SdlException">Couldn't get the display bounds for the display (check <see cref="Error.TryGet(out string?)"/> for more information)</exception>
	public readonly Rect<int> Bounds
	{
		get
		{
			unsafe
			{
				Unsafe.SkipInit(out Rect<int> rect);

				if (!SDL_GetDisplayBounds(mDisplayId, &rect))
				{
					failCouldNotGetDisplayBounds();
				}

				return rect;
			}

			[DoesNotReturn]
			static void failCouldNotGetDisplayBounds() => throw new SdlException("Couldn't get the display bounds for the display");
		}
	}

	/// <summary>
	/// Gets a collection of all the displays that are currently connected to the system
	/// </summary>
	/// <value>
	/// A collection of all the displays that are currently connected to the system
	/// </value>
	/// <remarks>
	/// <para>
	/// The value of this property remains unchanged until an <see cref="EventType.DisplayAdded"/> or <see cref="EventType.DisplayRemoved"/> event is raised.
	/// You should not query this property too often, but rather cache the result and update the cache when such an event is raised.
	/// </para>
	/// <para>
	/// This property should only be accessed from the main thread.
	/// </para>
	/// </remarks>
	/// <exception cref="SdlException">Couldn't get the displays connected to the system (check <see cref="Error.TryGet(out string?)"/> for more information)</exception>
	public static Display[] ConnectedDisplays
	{
		get
		{
			unsafe
			{
				Unsafe.SkipInit(out int count);

				var displays = SDL_GetDisplays(&count);
				try
				{
					if (displays is null)
					{
						failCouldNotGetDisplays();
					}

					if (count is not > 0)
					{
						return [];
					}

					var result = GC.AllocateUninitializedArray<Display>(count);

					MemoryMarshal.CreateReadOnlySpan(ref Unsafe.AsRef<Display>(displays), count).CopyTo(result.AsSpan());

					return result;
				}
				finally
				{
					Utilities.NativeMemory.SDL_free(displays);
				}
			}

			[DoesNotReturn]
			static void failCouldNotGetDisplays() => throw new SdlException("Couldn't get the displays connected to the system");
		}
	}

	/// <summary>
	/// Gets the content scale for this display
	/// </summary>
	/// <value>
	/// The content scale for this display, or <c>0</c> if the content scale couldn't be retrieved successfully (check <see cref="Error.TryGet(out string?)"/> for more information)
	/// </value>
	/// <remarks>
	/// <para>
	/// The content scale is the expected scale for content based on the DPI settings of the display.
	/// For example, a 4K display might have a <c>2.0</c> (200%) display scale, which means that the user expects UI elements to be twice as big on this display, to aid in readability.
	/// </para>
	/// <para>
	/// After window creation, <see cref="SDL_GetWindowDisplayScale"/>() should be used to determine the content scale factor for individual windows instead of content scale of the display for a window.
	/// That's because the per-window content scale factor may differ from the content scale factor of the display it is on, particularly on high-DPI and/or multi-monitor desktop configurations.
	/// </para>
	/// <para>
	/// This property should only be accessed from the main thread.
	/// </para>
	/// </remarks>
	public float ContentScale
	{
		get
		{
			// we don't throw an exception here, if the content scale is 0,
			// although that's considered an error by SDL, worthy of querying SDL_GetError()
			// -> we just doc, that if the content scale is 0 (which is an non-sensical content scale anyways), the user should check for errors using Error.TryGet(out string?)
			return SDL_GetDisplayContentScale(mDisplayId);
		}
	}

	/// <summary>
	/// Gets a reference to the current display mode used by this display
	/// </summary>
	/// <value>
	/// A reference to the current display mode used by this display
	/// </value>
	/// <remarks>
	/// <para>
	/// There's a difference between this property and <see cref="DesktopDisplayMode"/> when SDL runs fullscreen and has changed the resolution.
	/// In that case the value of this property reflects the current display mode and not the previous native display mode.
	/// </para>
	/// <para>
	/// This property should only be accessed from the main thread.
	/// </para>
	/// </remarks>
	/// <exception cref="SdlException">Couldn't get the current display mode for the display (check <see cref="Error.TryGet(out string?)"/> for more information)</exception>
	public readonly ref readonly DisplayMode CurrentDisplayMode
	{
		get
		{ 	unsafe
			{
				var displayMode = SDL_GetCurrentDisplayMode(mDisplayId);

				if (displayMode is null)
				{
					failDisplayModeNull();
				}

				return ref Unsafe.AsRef<DisplayMode>(displayMode);
			}

			[DoesNotReturn]
			static void failDisplayModeNull() => throw new SdlException("Couldn't get the current display mode for the display");
		}
	}

	/// <summary>
	/// Gets the current orientation of this display
	/// </summary>
	/// <value>
	/// The current orientation of this display, or <see cref="DisplayOrientation.Unknown"/> if the orientation isn't available
	/// </value>
	/// <remarks>
	/// <para>
	/// This property should only be accessed from the main thread.
	/// </para>
	/// </remarks>
	public readonly DisplayOrientation CurrentOrientation
	{
		get
		{
			return SDL_GetCurrentDisplayOrientation(mDisplayId);
		}
	}

	/// <summary>
	/// Gets a reference to the desktop display mode for this display
	/// </summary>
	/// <value>
	/// A reference to the desktop display mode for this display
	/// </value>
	/// <remarks>
	/// <para>
	/// There's a difference between this property and <see cref="CurrentDisplayMode"/> when SDL runs fullscreen and has changed the resolution.
	/// In that case the value of this property reflects the previous native display mode and not the current display mode.
	/// </para>
	/// <para>
	/// This property should only be accessed from the main thread.
	/// </para>
	/// </remarks>
	/// <exception cref="SdlException">Couldn't get the desktop display mode for the display (check <see cref="Error.TryGet(out string?)"/> for more information)</exception>
	public readonly ref readonly DisplayMode DesktopDisplayMode
	{
		get
		{ 	
			unsafe
			{

				var displayMode = SDL_GetDesktopDisplayMode(mDisplayId);

				if (displayMode is null)
				{
					failDisplayModeNull();
				}

				return ref Unsafe.AsRef<DisplayMode>(displayMode);
			}

			[DoesNotReturn]
			static void failDisplayModeNull() => throw new SdlException("Couldn't get the desktop display mode for the display");
		}
	}

	/// <summary>
	/// Gets a collection of references to the fullscreen display modes available for this display
	/// </summary>
	/// <value>
	/// A collection of references to the fullscreen display modes available for this display
	/// </value>
	/// <remarks>
	/// <para>
	/// The collection is sorted with the following priority:
	/// <list type="bullet">
	///		<item>
	///			<term><see cref="DisplayMode.Width"/></term>
	///			<description>Largest to smallest</description>
	///		</item>
	///		<item>
	///			<term><see cref="DisplayMode.Height"/></term>
	///			<description>Largest to smallest</description>
	///		</item>
	///		<item>
	///			<term><see cref="DisplayMode.Format">Bits per pixel</see></term>
	///			<description>More colors to fewer colors</description>
	///		</item>
	///		<item>
	///			<term><see cref="DisplayMode.Format">Packed pixel layout</see></term>
	///			<description>Largest to smallest</description>
	///		</item>
	///		<item>
	///			<term><see cref="DisplayMode.RefreshRate"/></term>
	///			<description>Highest to lowest</description>
	///		</item>
	///		<item>
	///			<term><see cref="DisplayMode.PixelDensity"/></term>
	///			<description>Lowest to highest</description>
	///		</item>
	///	</list>
	/// </para>
	/// <para>
	/// If the collection happens to be empty, you can check <see cref="Error.TryGet(out string?)"/> to see if there was an error while retrieving the display modes.
	/// </para>
	/// <para>
	/// This property should only be accessed from the main thread.
	/// </para>
	/// </remarks>
	public readonly DisplayModesEnumerable FullscreenDisplayModes
	{
		get
		{
			unsafe
			{
				return new(&SDL_GetFullscreenDisplayModes, &Utilities.NativeMemory.SDL_free, mDisplayId);
			}
		}
	}

	/// <summary>
	/// Gets the id of this display
	/// </summary>
	/// <value>
	/// The id of this display
	/// </value>
	/// <remarks>
	/// <para>
	/// The <see cref="Id"/> of a display is unique, remains unchanged while the display is connected to the system, and is never reused for the lifetime of the application.
	/// If a display is disconnected and then reconnected, it will get assigned a new <see cref="Id"/>.
	/// </para>
	/// <para>
	/// An id of <c>0</c> indicates an invalid display.
	/// </para>
	/// </remarks>
	public readonly uint Id { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mDisplayId; }

	/// <summary>
	/// Gets a value indicating whether this display has HDR headroom above the SDR white point
	/// </summary>
	/// <value>
	/// A value indicating whether this display has HDR headroom above the SDR white point
	/// </value>
	/// <remarks>
	/// <para>
	/// This property is for informational and diagnostic purposes only, as not all platforms provide this information at the display level.
	/// </para>
	/// <para>
	/// This property should only be accessed from the main thread.
	/// </para>
	/// </remarks>
	public readonly bool IsHdrEnabled => Properties?.TryGetBooleanValue(PropertyNames.HdrEnabledBoolean, out var hdrEnabled) is true
		&& hdrEnabled;

	/// <summary>
	/// Gets the "panel orientation" of the display
	/// </summary>
	/// <value>
	/// The "panel orientation" of the display, in degrees of clockwise rotation
	/// </value>
	/// <remarks>
	/// <para>
	/// This property is provided only as a hint, and the application is responsible for any coordinate transformations needed to conform to the requested display orientation.
	/// </para>
	/// <para>
	/// This property should only be accessed from the main thread.
	/// </para>
	/// </remarks>
	public readonly long KmsDrmPanelOrientation => Properties?.TryGetNumberValue(PropertyNames.KmsDrmPanelOrientationNumber, out var orientation) is true
		? orientation
		: 0;

	/// <summary>
	/// Gets the name of this display
	/// </summary>
	/// <value>
	/// The name of this display, or <c><see langword="null"/></c> if the name couldn't be retrieved successfully (check <see cref="Error.TryGet(out string?)"/> for more information)
	/// </value>
	/// <remarks>
	/// <para>
	/// This property should only be accessed from the main thread.
	/// </para>
	/// </remarks>
	public readonly string? Name
	{
		get
		{
			unsafe
			{
				// we don't throw an exception here, if the name is null,
				// although that's considered an error by SDL, worthy of querying SDL_GetError()
				// -> we just doc, that if the name is null, the user should check for errors using Error.TryGet(out string?)
				return Utf8StringMarshaller.ConvertToManaged(SDL_GetDisplayName(mDisplayId));
			}
		}
	}

	/// <summary>
	/// Gets the natural orientation of this display
	/// </summary>
	/// <value>
	/// The natural orientation of this display, or <see cref="DisplayOrientation.Unknown"/> if the natural orientation isn't available
	/// </value>
	/// <remarks>
	/// <para>
	/// This property should only be accessed from the main thread.
	/// </para>
	/// </remarks>
	public readonly DisplayOrientation NaturalOrientation
	{
		get
		{
			return SDL_GetNaturalDisplayOrientation(mDisplayId);
		}
	}

	/// <summary>
	/// Gets the primary display of the system
	/// </summary>
	/// <value>
	/// The primary display of the system
	/// </value>
	/// <remarks>
	/// <para>
	/// This property should only be accessed from the main thread.
	/// </para>
	/// </remarks>
	/// <exception cref="SdlException">Couldn't get the primary display (check <see cref="Error.TryGet(out string?)"/> for more information)</exception>
	public static Display PrimaryDisplay
	{
		get
		{
			var display = SDL_GetPrimaryDisplay();

			if (display is 0)
			{
				failCouldNotGetPrimaryDisplay();
			}

			return new(display);

			[DoesNotReturn]
			static void failCouldNotGetPrimaryDisplay() => throw new SdlException("Couldn't get the primary display");
		}
	}

	/// <summary>
	/// Gets the properties associated with this display
	/// </summary>
	/// <value>
	/// The properties associated with this display, or <c><see langword="null"/></c> if the properties could not be retrieved successfully (check <see cref="Error.TryGet(out string?)"/> for more information)
	/// </value>
	/// <remarks>
	/// <para>
	/// This property should only be accessed from the main thread.
	/// </para>
	/// </remarks>
	public readonly Properties? Properties
	{
		get
		{
			unsafe
			{
				// we don't throw an exception here, if the properties id is 0,
				// although that's considered an error by SDL, worthy of querying SDL_GetError()
				// -> we just doc, that if the properties id is 0 (the return value is null), the user should check for errors using Error.TryGet(out string?)
				return SDL_GetDisplayProperties(mDisplayId) switch
				{
					0 => null,
					var id => Properties.GetOrCreate(sdl: null, id)
				};
			}
		}
	}

	/// <summary>
	/// Gets the usable desktop area represented by this display
	/// </summary>
	/// <value>
	/// The usable desktop area represented by this display
	/// </value>
	/// <remarks>
	/// <para>
	/// This is the same area as the <see cref="Bounds"/> property, but with portions reserved by the system removed.
	/// For example, on Apple's macOS, this subtracts the area occupied by the menu bar and dock.
	/// </para>
	/// <para>
	/// Setting a window to be fullscreen generally bypasses these unusable areas, so these are good guidelines for the maximum space available to a non-fullscreen window.
	/// </para>
	/// <para>
	/// This property should only be accessed from the main thread.
	/// </para>
	/// </remarks>
	/// <exception cref="SdlException">Couldn't get the display usable bounds for the display (check <see cref="Error.TryGet(out string?)"/> for more information)</exception>
	public readonly Rect<int> UsableBounds
	{
		get
		{
			unsafe
			{
				Unsafe.SkipInit(out Rect<int> rect);

				if (!SDL_GetDisplayUsableBounds(mDisplayId, &rect))
				{
					failCouldNotGetDisplayUsableBounds();
				}

				return rect;
			}

			[DoesNotReturn]
			static void failCouldNotGetDisplayUsableBounds() => throw new SdlException("Couldn't get the display usable bounds for the display");
		}
	}

	/// <summary>
	/// Gets a pointer to the Wayland <c><see href="https://wayland.freedesktop.org/docs/html/apa.html#protocol-spec-wl_output">wl_output</see></c> associated with this display
	/// </summary>
	/// <value>
	/// A pointer to the Wayland <c><see href="https://wayland.freedesktop.org/docs/html/apa.html#protocol-spec-wl_output">wl_output</see></c> associated with this display
	/// </value>
	/// <remarks>
	/// <para>
	/// The value of this property can be directly cast to a <c><see href="https://wayland.freedesktop.org/docs/html/apa.html#protocol-spec-wl_output">wl_output</see>*</c> pointer.
	/// </para>
	/// <para>
	/// This property should only be accessed from the main thread.
	/// </para>
	/// </remarks>
	public readonly IntPtr WaylandWlOutput => Properties?.TryGetPointerValue(PropertyNames.WaylandWlOutputPointer, out var wlOutput) is true
		? wlOutput
		: IntPtr.Zero;

	/// <summary>
	/// Gets the Windows <c><see href="https://learn.microsoft.com/en-us/windows/win32/gdi/hmonitor-and-the-device-context">HMONITOR</see></c> handle associated with this display
	/// </summary>
	/// <value>
	/// The Windows <c><see href="https://learn.microsoft.com/en-us/windows/win32/gdi/hmonitor-and-the-device-context">HMONITOR</see></c> handle associated with this display
	/// </value>
	/// <remarks>
	/// <para>
	/// The value of this property can be directly cast to a <c><see href="https://learn.microsoft.com/en-us/windows/win32/gdi/hmonitor-and-the-device-context">HMONITOR</see></c> handle.
	/// </para>
	/// <para>
	/// This property should only be accessed from the main thread.
	/// </para>
	/// </remarks>
	public readonly IntPtr WindowsHMonitor => Properties?.TryGetPointerValue(PropertyNames.WindowsHMonitorPointer, out var hMonitor) is true
		? hMonitor
		: IntPtr.Zero;

	/// <inheritdoc/>
	public readonly override bool Equals([NotNullWhen(true)] object? obj) => obj is Display other && Equals(other);

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly bool Equals(Display other) => mDisplayId == other.mDisplayId;

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly override int GetHashCode() => mDisplayId.GetHashCode();

	/// <inheritdoc/>
	public readonly override string ToString() => ToString(format: default, formatProvider: default);

	/// <inheritdoc cref="IFormattable.ToString(string?, IFormatProvider?)"/>
	public readonly string ToString(IFormatProvider? formatProvider) => ToString(format: default, formatProvider);

	/// <inheritdoc cref="IFormattable.ToString(string?, IFormatProvider?)"/>
	public readonly string ToString(string? format) => ToString(format, formatProvider: default);

	/// <inheritdoc/>
	public readonly string ToString(string? format, IFormatProvider? formatProvider)
		=> $"{{ {nameof(Id)}: {Id.ToString(format, formatProvider)} }}";

	/// <inheritdoc/>
	public readonly bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format = default, IFormatProvider? provider = default)
	{
		charsWritten = 0;

		return SpanFormat.TryWrite($"{{ {nameof(Id)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(Id, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite(" }", ref destination, ref charsWritten);
	}

	/// <summary>
	/// Tries to get the closest fullscreen match to the specified display mode for this display
	/// </summary>
	/// <param name="width">The desired width of the display mode, in pixels</param>
	/// <param name="height">The desired height of the display mode, in pixels</param>
	/// <param name="displayMode">The resulting closest matching display mode that is equal to or larger than the specified display mode, if this method returns <c><see langword="true"/></c></param>
	/// <param name="refreshRate">The desired refresh rate of the display mode, in Hz (hertz), or <c>0</c> (the default) for the desktop refresh rate</param>
	/// <param name="includeHighDensityModes">A value indicating whether high-density display modes should be included in the search</param>
	/// <returns><c><see langword="true"/></c>, if a matching display mode was found successfully; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// The available display modes are searched and the <paramref name="displayMode"/> parameter will be filled in with the closest mode matching the requested display mode.
	/// If <paramref name="refreshRate"/> is <c>0</c> then the requested refresh rate will default to the desktop refresh rate.
	/// The display modes are searched with size being a first priority, format being a second priority, and finally checking the refresh rate.
	/// If all the available display modes are too small, then <c><see langword="false"/></c> is returned.
	/// </para>
	/// <para>
	/// This method should only be called from the main thread.
	/// </para>
	/// </remarks>
	public readonly bool TryGetClosestFullscreenDisplayMode(int width, int height, out DisplayMode displayMode, float refreshRate = 0, bool includeHighDensityModes = false)
	{
		unsafe
		{
			fixed(DisplayMode* displayModePtr = &displayMode)
			{
				return SDL_GetClosestFullscreenDisplayMode(mDisplayId, width, height, refreshRate, includeHighDensityModes, displayModePtr);
			}
		}
	}

	/// <summary>
	/// Tries to get the display that contains the specified point in screen space
	/// </summary>
	/// <param name="point">The point in screen space to get the display for</param>
	/// <param name="display">The display that contains the specified point in screen space, if this method returns <c><see langword="true"/></c></param>
	/// <returns><c><see langword="true"/></c>, if the display containing the specified point in screen space was found successfully; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method should only be called from the main thread.
	/// </para>
	/// </remarks>
	public static bool TryGetFromPoint(in Point<int> point, out Display display)
	{
		unsafe
		{
			fixed (Point<int>* pointPtr = &point)
			{
				display = new(SDL_GetDisplayForPoint(pointPtr));
			}

			return display.mDisplayId is not 0;
		}
	}

	/// <summary>
	/// Tries to get the display that contains the majority of the specified rectangle in screen space
	/// </summary>
	/// <param name="rect">The rectangle in screen space to get the display for</param>
	/// <param name="display">The display that contains the majority of the specified rectangle in screen space, if this method returns <c><see langword="true"/></c></param>
	/// <returns><c><see langword="true"/></c>, if a display containing the majority of the specified rectangle in screen space was found successfully; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method should only be called from the main thread.
	/// </para>
	/// </remarks>
	public static bool TryGetFromRect(in Rect<int> rect, out Display display)
	{
		unsafe
		{
			fixed (Rect<int>* rectPtr = &rect)
			{
				display = new(SDL_GetDisplayForRect(rectPtr));
			}

			return display.mDisplayId is not 0;
		}
	}

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator ==(Display left, Display right) => left.mDisplayId == right.mDisplayId;

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator !=(Display left, Display right) => left.mDisplayId != right.mDisplayId;
}
