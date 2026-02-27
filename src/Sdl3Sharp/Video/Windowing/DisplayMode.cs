using Sdl3Sharp.Video.Coloring;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sdl3Sharp.Video.Windowing;

/// <summary>
/// Represents a display mode for a specific <see cref="IDisplay"/>
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public readonly partial struct DisplayMode
{
	private readonly uint mDisplayID;
	private readonly PixelFormat mFormat;
	private readonly int mW;
	private readonly int mH;
	private readonly float mPixelDensity;
	private readonly float mRefreshRate;
	private readonly int mRefreshRateNumerator;
	private readonly int mRefreshRateDenominator;
	private unsafe readonly SDL_DisplayModeData* mInternal;

	/// <summary>
	/// Gets the <see cref="IDisplay"/> associated with this display mode
	/// </summary>
	/// <value>
	/// The <see cref="IDisplay"/> associated with this display mode, or <c><see langword="null"/></c> if this display mode is invalid
	/// </value>
	/// <remarks>
	/// <para>
	/// The value of this property will only be <c><see langword="null"/></c> if this display mode is invalid.
	/// </para>
	/// </remarks>
	public readonly IDisplay? Display
	{
		get
		{
			IDisplay.TryGetOrCreate(mDisplayID, out var result);
			return result;
		}
	}

	/// <summary>
	/// Gets the pixel format of this display mode
	/// </summary>
	/// <value>
	/// The pixel format of this display mode
	/// </value>
	public readonly PixelFormat Format { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mFormat; }

	/// <summary>
	/// Gets the logical height of this display mode
	/// </summary>
	/// <value>
	/// The logical height of this display mode, in logical pixels
	/// </value>
	/// <remarks>
	/// <para>
	/// To convert the value of this property into actual device pixels, you can multiply it by the value of the <see cref="PixelDensity"/> property. 
	/// </para>
	/// </remarks>
	public readonly int Height { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mH; }

	/// <summary>
	/// Gets the scale factor for converting logical pixels into actual device pixels for this display mode
	/// </summary>
	/// <value>
	/// The scale factor for converting logical pixels into actual device pixels for this display mode
	/// </value>
	/// <remarks>
	/// <para>
	/// You can multiply the values of the <see cref="Width"/> and <see cref="Height"/> properties by the value of this property to get the actual device pixel dimensions of this display mode.
	/// </para>
	/// </remarks>
	public readonly float PixelDensity { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mPixelDensity; }

	/// <summary>
	/// Gets the refresh rate of this display mode
	/// </summary>
	/// <value>
	/// The refresh rate of this display mode, in Hz (hertz), or <c>0</c> if the refresh rate is unknown or unspecified
	/// </value>
	public readonly float RefreshRate { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mRefreshRate; }

	/// <summary>
	/// Gets the refresh rate of this display mode as a ratio of two integers
	/// </summary>
	/// <value>
	/// The refresh rate of this display mode as a ratio of two integers, where the first integer (the numerator) can be <c>0</c> to indicate an unknown or unspecified refresh rate
	/// </value>
	public readonly (int Numerator, int Denominator) RefreshRateRatio { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => (mRefreshRateNumerator, mRefreshRateDenominator); }

	/// <summary>
	/// Gets the logical width of this display mode
	/// </summary>
	/// <value>
	/// The logical width of this display mode, in logical pixels
	/// </value>
	/// <remarks>
	/// <para>
	/// To convert the value of this property into actual device pixels, you can multiply it by the value of the <see cref="PixelDensity"/> property.
	/// </para>
	/// </remarks>
	public readonly int Width { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mW; }
}
