using Sdl3Sharp.Internal;
using Sdl3Sharp.Video.Coloring;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Video.Windowing;

/// <summary>
/// Represents a display mode for a specific <see cref="Display"/>
/// </summary>
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
public abstract partial class DisplayMode : IFormattable, ISpanFormattable
{
	private static readonly ConcurrentDictionary<IntPtr, WeakReference<Unmanaged>> mKnownUnmanagedInstances = [];

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private string DebuggerDisplay => ToString(formatProvider: CultureInfo.InvariantCulture);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private DisplayMode() { }

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
	public Display? Display
	{
		get
		{
			unsafe
			{
				Display.TryGetOrCreate(Mode.DisplayID, out var result);
				return result;
			}
		}
	}

	/// <summary>
	/// Gets the pixel format of this display mode
	/// </summary>
	/// <value>
	/// The pixel format of this display mode
	/// </value>
	public PixelFormat Format { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => Mode.Format; }

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
	public int Height { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => Mode.H; }

	internal abstract ref readonly SDL_DisplayMode Mode { get; }

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
	public float PixelDensity { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => Mode.PixelDensity; }

	/// <summary>
	/// Gets the refresh rate of this display mode
	/// </summary>
	/// <value>
	/// The refresh rate of this display mode, in Hz (hertz), or <c>0</c> if the refresh rate is unknown or unspecified
	/// </value>
	public float RefreshRate { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => Mode.RefreshRate; }

	/// <summary>
	/// Gets the refresh rate of this display mode as a ratio of two integers
	/// </summary>
	/// <value>
	/// The refresh rate of this display mode as a ratio of two integers, where the first integer (the numerator) can be <c>0</c> to indicate an unknown or unspecified refresh rate
	/// </value>
	public (int Numerator, int Denominator) RefreshRateRatio
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		get
		{
			ref readonly var mode = ref Mode;
			return (mode.RefreshRateNumerator, mode.RefreshRateDenominator);
		}
	}

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
	public int Width { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => Mode.W; }

	internal static ref SDL_DisplayMode CreateManaged(out DisplayMode result)
	{
		var managed = new Managed();
		result = managed;
		return ref managed.MutableMode;
	}

	/// <inheritdoc/>
	public override string ToString() => ToString(format: default, formatProvider: default);

	/// <inheritdoc cref="IFormattable.ToString(string?, IFormatProvider?)"/>
	public string ToString(IFormatProvider? formatProvider) => ToString(format: default, formatProvider);

	/// <inheritdoc cref="IFormattable.ToString(string?, IFormatProvider?)"/>
	public string ToString(string? format) => ToString(format, formatProvider: default);

	/// <inheritdoc/>
	public string ToString(string? format, IFormatProvider? formatProvider)
	{
		unsafe
		{
			ref readonly var mode = ref Mode;

			if (mode.DisplayID is 0)
			{
				return "invalid";
			}

			return $"{{ {nameof(Display)}: {(Display.TryGetOrCreate(mode.DisplayID, out var display) ? display.ToString(format, formatProvider) : "null")}, {
				nameof(Format)}: {mode.Format}, {
				nameof(Width)}: {mode.W.ToString(format, formatProvider)}, {
				nameof(Height)}: {mode.H.ToString(format, formatProvider)}, {
				nameof(PixelDensity)}: {mode.PixelDensity.ToString(format, formatProvider)}, {
				nameof(RefreshRate)}: {mode.RefreshRate.ToString(format, formatProvider)}, {
				nameof(RefreshRateRatio)}: ({
					nameof(RefreshRateRatio.Numerator)}: {mode.RefreshRateNumerator.ToString(format, formatProvider)}, {
					nameof(RefreshRateRatio.Denominator)}: {mode.RefreshRateDenominator.ToString(format, formatProvider)}) }}";
		}
	}

	/// <inheritdoc/>
	public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format = default, IFormatProvider? provider = default)
	{
		unsafe
		{
			charsWritten = 0;

			ref readonly var mode = ref Mode;

			if (mode.DisplayID is 0)
			{
				return SpanFormat.TryWrite("invalid", ref destination, ref charsWritten);
			}

			return SpanFormat.TryWrite($"{{ {nameof(Display)}: ", ref destination, ref charsWritten)
				&& (Display.TryGetOrCreate(mode.DisplayID, out var display)
					? SpanFormat.TryWrite(display, ref destination, ref charsWritten, format, provider)
					: SpanFormat.TryWrite("null", ref destination, ref charsWritten))
				&& SpanFormat.TryWrite($", {nameof(Format)}: ", ref destination, ref charsWritten)
				&& SpanFormat.TryWrite(mode.Format, ref destination, ref charsWritten)
				&& SpanFormat.TryWrite($", {nameof(Width)}: ", ref destination, ref charsWritten)
				&& SpanFormat.TryWrite(mode.W, ref destination, ref charsWritten, format, provider)
				&& SpanFormat.TryWrite($", {nameof(Height)}: ", ref destination, ref charsWritten)
				&& SpanFormat.TryWrite(mode.H, ref destination, ref charsWritten, format, provider)
				&& SpanFormat.TryWrite($", {nameof(PixelDensity)}: ", ref destination, ref charsWritten)
				&& SpanFormat.TryWrite(mode.PixelDensity, ref destination, ref charsWritten, format, provider)
				&& SpanFormat.TryWrite($", {nameof(RefreshRate)}: ", ref destination, ref charsWritten)
				&& SpanFormat.TryWrite(mode.RefreshRate, ref destination, ref charsWritten, format, provider)
				&& SpanFormat.TryWrite($", {nameof(RefreshRateRatio)}: ({nameof(RefreshRateRatio.Numerator)}: ", ref destination, ref charsWritten)
				&& SpanFormat.TryWrite(mode.RefreshRateNumerator, ref destination, ref charsWritten, format, provider)
				&& SpanFormat.TryWrite($", {nameof(RefreshRateRatio.Denominator)}: ", ref destination, ref charsWritten)
				&& SpanFormat.TryWrite(mode.RefreshRateDenominator, ref destination, ref charsWritten, format, provider)
				&& SpanFormat.TryWrite(") }", ref destination, ref charsWritten);
		}
	}
	internal unsafe static bool TryGetOrCreateUnmanaged(SDL_DisplayMode* mode, [NotNullWhen(true)] out DisplayMode? result)
	{
		if (mode is null)
		{
			result = null;
			return false;
		}

		var modeRef = mKnownUnmanagedInstances.GetOrAdd(unchecked((IntPtr)mode), createRef);

		if (!modeRef.TryGetTarget(out var target))
		{
			modeRef.SetTarget(target = create(mode));
		}

		result = target;
		return true;

		static WeakReference<Unmanaged> createRef(IntPtr mode) => new(create(unchecked((SDL_DisplayMode*)mode)));

		static Unmanaged create(SDL_DisplayMode* mode) => new(mode);
	}
}
