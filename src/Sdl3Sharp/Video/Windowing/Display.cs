using Sdl3Sharp.Video.Drawing;
using Sdl3Sharp.Video.Windowing.Drivers;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.Marshalling;

namespace Sdl3Sharp.Video.Windowing;

/// <summary>
/// Represents a display connected to the system
/// </summary>
/// <typeparam name="TDriver">The windowing driver associated with this display</typeparam>
/// <remarks>
/// <para>
/// The <see cref="Id"/> of a display is unique, remains unchanged while the display is connected to the system, and is never reused for the lifetime of the application.
/// If a display is disconnected and then reconnected, it will get assigned a new <see cref="Id"/>.
/// </para>
/// <para>
/// For the most part <see cref="Display{TDriver}"/>s are not thread-safe, and most of their properties and methods should only be accessed from the main thread!
/// </para>
/// <para>
/// <see cref="Display{TDriver}"/>s are concrete display types, associated with a specific windowing driver.
/// </para>
/// <para>
/// If you want to use them in a more general way, you can use them as <see cref="IDisplay"/> instances, which serve as common abstractions.
/// </para>
/// </remarks>
public sealed partial class Display<TDriver> : IDisplay
	where TDriver : IWindowingDriver
{
	private readonly uint mDisplayId;

	internal Display(uint displayId, bool register)
	{
		mDisplayId = displayId;

		if (register)
		{
			IDisplay.Register(this);
		}
	}

	/// <inheritdoc/>
	~Display() => IDisplay.Deregister(this);

	void IDisplay._InternalImplementationOnly() { }

	/// <inheritdoc/>
	public Rect<int> Bounds
	{
		get
		{
			unsafe
			{
				Unsafe.SkipInit(out Rect<int> rect);

				if (!IDisplay.SDL_GetDisplayBounds(mDisplayId, &rect))
				{
					failCouldNotGetDisplayBounds();
				}

				return rect;
			}

			[DoesNotReturn]
			static void failCouldNotGetDisplayBounds() => throw new SdlException("Couldn't get the display bounds for the display");
		}
	}

	/// <inheritdoc/>
	public float ContentScale
	{
		get
		{
			// we don't throw an exception here, if the content scale is 0,
			// although that's considered an error by SDL, worthy of querying SDL_GetError()
			// -> we just doc, that if the content scale is 0 (which is an non-sensical content scale anyways), the user should check for errors using Error.TryGet(out string?)
			return IDisplay.SDL_GetDisplayContentScale(mDisplayId);
		}
	}

	/// <inheritdoc/>
	public ref readonly DisplayMode CurrentDisplayMode
	{
		get
		{
			unsafe
			{
				var displayMode = IDisplay.SDL_GetCurrentDisplayMode(mDisplayId);

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

	/// <inheritdoc/>
	public DisplayOrientation CurrentOrientation
	{
		get
		{
			return IDisplay.SDL_GetCurrentDisplayOrientation(mDisplayId);
		}
	}

	/// <inheritdoc/>
	public ref readonly DisplayMode DesktopDisplayMode
	{
		get
		{
			unsafe
			{

				var displayMode = IDisplay.SDL_GetDesktopDisplayMode(mDisplayId);

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

	/// <inheritdoc/>
	public IDisplay.DisplayModesEnumerable FullscreenDisplayModes
	{
		get
		{
			unsafe
			{
				return new(&IDisplay.SDL_GetFullscreenDisplayModes, &Utilities.NativeMemory.SDL_free, mDisplayId);
			}
		}
	}

	/// <inheritdoc/>
	public uint Id { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mDisplayId; }

	/// <inheritdoc/>
	public bool IsHdrEnabled => Properties?.TryGetBooleanValue(IDisplay.PropertyNames.HdrEnabledBoolean, out var hdrEnabled) is true
		&& hdrEnabled;

	/// <inheritdoc/>
	public string? Name
	{
		get
		{
			unsafe
			{
				// we don't throw an exception here, if the name is null,
				// although that's considered an error by SDL, worthy of querying SDL_GetError()
				// -> we just doc, that if the name is null, the user should check for errors using Error.TryGet(out string?)
				return Utf8StringMarshaller.ConvertToManaged(IDisplay.SDL_GetDisplayName(mDisplayId));
			}
		}
	}

	/// <inheritdoc/>
	public DisplayOrientation NaturalOrientation
	{
		get
		{
			return IDisplay.SDL_GetNaturalDisplayOrientation(mDisplayId);
		}
	}

	/// <inheritdoc/>
	public Properties? Properties
	{
		get
		{
			unsafe
			{
				// we don't throw an exception here, if the properties id is 0,
				// although that's considered an error by SDL, worthy of querying SDL_GetError()
				// -> we just doc, that if the properties id is 0 (the return value is null), the user should check for errors using Error.TryGet(out string?)
				return IDisplay.SDL_GetDisplayProperties(mDisplayId) switch
				{
					0 => null,
					var id => Properties.GetOrCreate(sdl: null, id)
				};
			}
		}
	}

	/// <inheritdoc/>
	public Rect<int> UsableBounds
	{
		get
		{
			unsafe
			{
				Unsafe.SkipInit(out Rect<int> rect);

				if (!IDisplay.SDL_GetDisplayUsableBounds(mDisplayId, &rect))
				{
					failCouldNotGetDisplayUsableBounds();
				}

				return rect;
			}

			[DoesNotReturn]
			static void failCouldNotGetDisplayUsableBounds() => throw new SdlException("Couldn't get the display usable bounds for the display");
		}
	}

	/// <inheritdoc/>
	public bool TryGetClosestFullscreenDisplayMode(int width, int height, out DisplayMode displayMode, float refreshRate = 0, bool includeHighDensityModes = false)
	{
		unsafe
		{
			fixed (DisplayMode* displayModePtr = &displayMode)
			{
				return IDisplay.SDL_GetClosestFullscreenDisplayMode(mDisplayId, width, height, refreshRate, includeHighDensityModes, displayModePtr);
			}
		}
	}
}
