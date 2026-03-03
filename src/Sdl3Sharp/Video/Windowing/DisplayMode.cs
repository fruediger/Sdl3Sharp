using Sdl3Sharp.Internal;
using Sdl3Sharp.Video.Coloring;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sdl3Sharp.Video.Windowing;

/// <summary>
/// Represents a display mode for a specific <see cref="Display"/>
/// </summary>
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
[StructLayout(LayoutKind.Sequential)]
public readonly partial struct DisplayMode : IFormattable, ISpanFormattable
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly string DebuggerDisplay => ToString(formatProvider: CultureInfo.InvariantCulture);

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
	/// Gets the <see cref="Display"/> associated with this display mode
	/// </summary>
	/// <value>
	/// The <see cref="Display"/> associated with this display mode, or <c><see langword="null"/></c> if this display mode is invalid
	/// </value>
	/// <remarks>
	/// <para>
	/// The value of this property will only be <c><see langword="null"/></c> if this display mode is invalid.
	/// </para>
	/// </remarks>
	public readonly Display? Display
	{
		get
		{
			Display.TryGetOrCreate(mDisplayID, out var result);
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

	/// <inheritdoc/>
	public readonly override string ToString() => ToString(format: default, formatProvider: default);

	/// <inheritdoc cref="IFormattable.ToString(string?, IFormatProvider?)"/>
	public readonly string ToString(IFormatProvider? formatProvider) => ToString(format: default, formatProvider);

	/// <inheritdoc cref="IFormattable.ToString(string?, IFormatProvider?)"/>
	public readonly string ToString(string? format) => ToString(format, formatProvider: default);

	/// <inheritdoc/>
	public readonly string ToString(string? format, IFormatProvider? formatProvider)
		=> $"{{ {nameof(Display)}: {Display switch { null => "null", var display => display.ToString(format, formatProvider) }}, {
			nameof(Format)}: {mFormat}, {
			nameof(Width)}: {mW.ToString(format, formatProvider)}, {
			nameof(Height)}: {mH.ToString(format, formatProvider)}, {
			nameof(PixelDensity)}: {mPixelDensity.ToString(format, formatProvider)}, {
			nameof(RefreshRate)}: {mRefreshRate.ToString(format, formatProvider)}, {
			nameof(RefreshRateRatio)}: ({
				nameof(RefreshRateRatio.Numerator)}: {mRefreshRateNumerator.ToString(format, formatProvider)}, {
				nameof(RefreshRateRatio.Denominator)}: {mRefreshRateDenominator.ToString(format, formatProvider)}) }}";

	/// <inheritdoc/>
	public readonly bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format = default, IFormatProvider? provider = default)
	{
		charsWritten = 0;

		return SpanFormat.TryWrite($"{{ {nameof(Display)}: ", ref destination, ref charsWritten)
			&& Display switch
			{
				null => SpanFormat.TryWrite("null", ref destination, ref charsWritten),
				var display => display.TryFormat(destination, out var displayCharsWritten, format, provider)
			}
			&& SpanFormat.TryWrite($", {nameof(Format)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(mFormat, ref destination, ref charsWritten)
			&& SpanFormat.TryWrite($", {nameof(Width)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(mW, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite($", {nameof(Height)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(mH, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite($", {nameof(PixelDensity)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(mPixelDensity, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite($", {nameof(RefreshRate)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(mRefreshRate, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite($", {nameof(RefreshRateRatio)}: ({nameof(RefreshRateRatio.Numerator)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(mRefreshRateNumerator, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite($", {nameof(RefreshRateRatio.Denominator)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(mRefreshRateDenominator, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite(") }", ref destination, ref charsWritten);
	}
}
