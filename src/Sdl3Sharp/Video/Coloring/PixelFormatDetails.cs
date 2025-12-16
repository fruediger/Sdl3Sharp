using Sdl3Sharp.Internal;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sdl3Sharp.Video.Coloring;

/// <summary>
/// Represents details about a specific pixel format
/// </summary>
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
[StructLayout(LayoutKind.Sequential)]
public readonly partial struct PixelFormatDetails : IFormattable, ISpanFormattable
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly string DebuggerDisplay => ToString(formatProvider: CultureInfo.InvariantCulture);

	private unsafe readonly SDL_PixelFormatDetails* mFormat;
	
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	internal unsafe PixelFormatDetails(SDL_PixelFormatDetails* formatDetails) => mFormat = formatDetails;

	/// <summary>
	/// Gets the number of bits used for the alpha component
	/// </summary>
	/// <value>
	/// The number of bits used for the alpha component
	/// </value>
	public readonly byte ABits { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get { unsafe { return mFormat is not null ? mFormat->ABits : default; } } }

	/// <summary>
	/// Gets a bit mask for the alpha component
	/// </summary>
	/// <value>
	/// A bit mask for the alpha component
	/// </value>
	public readonly uint AMask { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get { unsafe { return mFormat is not null ? mFormat->AMask : default; } } }

	/// <summary>
	/// Gets the shift value for the alpha component
	/// </summary>
	/// <value>
	/// The shift value for the alpha component
	/// </value>
	public readonly byte AShift { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get { unsafe { return mFormat is not null ? mFormat->AShift : default; } } }

	/// <summary>
	/// Gets the number of bits used for the blue component
	/// </summary>
	/// <value>
	/// The number of bits used for the blue component
	/// </value>
	public readonly byte BBits { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get { unsafe { return mFormat is not null ? mFormat->BBits : default; } } }

	/// <summary>
	/// Gets a bit mask for the blue component
	/// </summary>
	/// <value>
	/// A bit mask for the blue component
	/// </value>
	public readonly uint BMask { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get { unsafe { return mFormat is not null ? mFormat->BMask : default; } } }

	/// <summary>
	/// Gets the shift value for the blue component
	/// </summary>
	/// <value>
	/// The shift value for the blue component
	/// </value>
	public readonly byte BShift { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get { unsafe { return mFormat is not null ? mFormat->BShift : default; } } }

	/// <summary>
	/// Gets the number of bits per pixel
	/// </summary>
	/// <value>
	/// The number of bits per pixel
	/// </value>
	public readonly byte BitsPerPixel { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get { unsafe { return mFormat is not null ? mFormat->BitsPerPixel : default; } } }

	/// <summary>
	/// Gets the number of bytes per pixel
	/// </summary>
	/// <value>
	/// The number of bytes per pixel
	/// </value>
	public readonly byte BytesPerPixel { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get { unsafe { return mFormat is not null ? mFormat->BytesPerPixel : default; } } }

	/// <summary>
	/// Gets the pixel format this <see cref="PixelFormatDetails"/> describes
	/// </summary>
	/// <value>
	/// The pixel format this <see cref="PixelFormatDetails"/> describes
	/// </value>
	public readonly PixelFormat Format { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get { unsafe { return mFormat is not null ? mFormat->Format : default; } } }

	/// <summary>
	/// Gets the number of bits used for the green component
	/// </summary>
	/// <value>
	/// The number of bits used for the green component
	/// </value>
	public readonly byte GBits { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get { unsafe { return mFormat is not null ? mFormat->GBits : default; } } }

	/// <summary>
	/// Gets a bit mask for the green component
	/// </summary>
	/// <value>
	/// A bit mask for the green component
	/// </value>
	public readonly uint GMask { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get { unsafe { return mFormat is not null ? mFormat->GMask : default; } } }

	/// <summary>
	/// Gets the shift value for the green component
	/// </summary>
	/// <value>
	/// The shift value for the green component
	/// </value>
	public readonly byte GShift { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get { unsafe { return mFormat is not null ? mFormat->GShift : default; } } }

	/// <summary>
	/// Gets the number of bits used for the red component
	/// </summary>
	/// <value>
	/// The number of bits used for the red component
	/// </value>
	public readonly byte RBits { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get { unsafe { return mFormat is not null ? mFormat->RBits : default; } } }

	/// <summary>
	/// Gets a bit mask for the red component
	/// </summary>
	/// <value>
	/// A bit mask for the red component
	/// </value>
	public readonly uint RMask { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get { unsafe { return mFormat is not null ? mFormat->RMask : default; } } }

	/// <summary>
	/// Gets the shift value for the red component
	/// </summary>
	/// <value>
	/// The shift value for the red component
	/// </value>
	public readonly byte RShift { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get { unsafe { return mFormat is not null ? mFormat->RShift : default; } } }

	/// <summary>
	/// Gets the RGB component values from an encoded pixel value
	/// </summary>
	/// <param name="pixelValue">An an encoded pixel value in the pixel format of this <see cref="PixelFormatDetails"/></param>
	/// <param name="palette">An optional <see cref="Palette"/> to use for indexed pixel formats</param>
	/// <param name="r">The red component value of the encoded pixel value</param>
	/// <param name="g">The green component value of the encoded pixel value</param>
	/// <param name="b">The blue component value of the encoded pixel value</param>
	/// <remarks>
	/// <para>
	/// This method uses the entire 8-bit (<c>0</c>-<c>255</c>) range when converting color components from pixel formats with less than 8-bits per RGB component
	/// (e.g., a completely white pixel in 16-bit <see cref="PixelFormat.Rgb565"/> format would return (<paramref name="r"/>, <paramref name="g"/>, <paramref name="b"/>) = (<c>0xff</c>, <c>0xff</c>, <c>0xff</c>),
	/// <em>not</em> (<c>0xf8</c>, <c>0xfc</c>, <c>0xf8</c>)).
	/// </para>
	/// </remarks>
	public readonly void GetColor(uint pixelValue, Palette? palette, out byte r, out byte g, out byte b)
	{
		unsafe
		{
			Unsafe.SkipInit(out byte rTmp);
			Unsafe.SkipInit(out byte gTmp);
			Unsafe.SkipInit(out byte bTmp);

			SDL_GetRGB(pixelValue, mFormat, palette is not null ? palette.Pointer : null, &rTmp, &gTmp, &bTmp);

			r = rTmp;
			g = gTmp;
			b = bTmp;
		}
	}

	/// <summary>
	/// Gets the RGBA component values from an encoded pixel value
	/// </summary>
	/// <param name="pixelValue">An an encoded pixel value in the pixel format of this <see cref="PixelFormatDetails"/></param>
	/// <param name="palette">An optional <see cref="Palette"/> to use for indexed pixel formats</param>
	/// <param name="r">The red component value of the encoded pixel value</param>
	/// <param name="g">The green component value of the encoded pixel value</param>
	/// <param name="b">The blue component value of the encoded pixel value</param>
	/// <param name="a">The alpha component value of the encoded pixel value</param>
	/// <remarks>
	/// <para>
	/// This method uses the entire 8-bit (<c>0</c>-<c>255</c>) range when converting color components from pixel formats with less than 8-bits per RGB component
	/// (e.g., a completely white pixel in 16-bit <see cref="PixelFormat.Rgb565"/> format would return (<paramref name="r"/>, <paramref name="g"/>, <paramref name="b"/>) = (<c>0xff</c>, <c>0xff</c>, <c>0xff</c>),
	/// <em>not</em> (<c>0xf8</c>, <c>0xfc</c>, <c>0xf8</c>)).
	/// </para>
	/// <para>
	/// If the surface has no alpha component, the <paramref name="a"/>lpha component value will be returned as <c>0xff</c> (fully opaque).
	/// </para>
	/// </remarks>
	public readonly void GetColor(uint pixelValue, Palette? palette, out byte r, out byte g, out byte b, out byte a)
	{
		unsafe
		{
			Unsafe.SkipInit(out byte rTmp);
			Unsafe.SkipInit(out byte gTmp);
			Unsafe.SkipInit(out byte bTmp);
			Unsafe.SkipInit(out byte aTmp);

			SDL_GetRGBA(pixelValue, mFormat, palette is not null ? palette.Pointer : null, &rTmp, &gTmp, &bTmp, &aTmp);

			r = rTmp;
			g = gTmp;
			b = bTmp;
			a = aTmp;
		}
	}

	/// <summary>
	/// Gets a <see cref="Color{T}">Color</see>&lt;<see cref="byte"/>&gt; from an encoded pixel value
	/// </summary>
	/// <param name="pixelValue">An an encoded pixel value in the pixel format of this <see cref="PixelFormatDetails"/></param>
	/// <param name="palette">An optional <see cref="Palette"/> to use for indexed pixel formats</param>
	/// <param name="color">The <see cref="Color{T}">Color</see>&lt;<see cref="byte"/>&gt; of the encoded pixel value</param>
	/// <remarks>
	/// <para>
	/// This method uses the entire 8-bit (<c>0</c>-<c>255</c>) range when converting color components from pixel formats with less than 8-bits per RGB component
	/// (e.g., a completely white pixel in 16-bit <see cref="PixelFormat.Rgb565"/> format would return (<see cref="Color{T}.R"/>, <see cref="Color{T}.G"/>, <see cref="Color{T}.B"/>) = (<c>0xff</c>, <c>0xff</c>, <c>0xff</c>),
	/// <em>not</em> (<c>0xf8</c>, <c>0xfc</c>, <c>0xf8</c>)).
	/// </para>
	/// <para>
	/// If the surface has no alpha component, the <see cref="Color{T}.A"/> value will be returned as <c>0xff</c> (fully opaque).
	/// </para>
	/// </remarks>
	public readonly void GetColor(uint pixelValue, Palette? palette, out Color<byte> color)
	{
		GetColor(pixelValue, palette, out var r, out var g, out var b, out var a);
		color = new(r, g, b, a);
	}

	/// <summary>
	/// Maps RGB component values to an <em>opaque</em> encoded pixel value
	/// </summary>
	/// <param name="palette">An optional <see cref="Palette"/> to use for indexed pixel formats</param>
	/// <param name="r">The red component value to encode</param>
	/// <param name="g">The green component value to encode</param>
	/// <param name="b">The blue component value to encode</param>
	/// <returns>An encoded pixel value in the pixel format of this <see cref="PixelFormatDetails"/></returns>
	/// <remarks>
	/// <para>
	/// This method maps the RGB color value to the specified pixel format and returns the encoded pixel value best approximating the given RGB color value for the pixel format.
	/// </para>
	/// <para>
	/// If the pixel format is indexed and uses a <see cref="Palette"/> the index of the closest matching color in the palette will be returned.
	/// </para>
	/// <para>
	/// If the pixel format has an alpha component, the encoded pixel value's alpha component value will be set to be fully opaque.
	/// </para>
	/// <para>
	/// If the pixel format's <see cref="BitsPerPixel">bits-per-pixel</see> value (color depth) is less than 32-bits per pixel then the unused upper bits of the returned encoded pixel value can safely be ignored
	/// (e.g., with a 16-bits per pixel format the returned encoded pixel value can be safely cast (truncated) to an <see cref="ushort"/> value, and similarly to a <see cref="byte"/> value for an 8-bits per pixel formats).
	/// </para>
	/// </remarks>
	public readonly uint MapColor(Palette? palette, byte r, byte g, byte b)
	{
		unsafe
		{
			return SDL_MapRGB(mFormat, palette is not null ? palette.Pointer : null, r, g, b);
		}
	}

	/// <summary>
	/// Maps RGBA component values to an encoded pixel value
	/// </summary>
	/// <param name="palette">An optional <see cref="Palette"/> to use for indexed pixel formats</param>
	/// <param name="r">The red component value to encode</param>
	/// <param name="g">The green component value to encode</param>
	/// <param name="b">The blue component value to encode</param>
	/// <param name="a">The alpha component value to encode</param>
	/// <returns>An encoded pixel value in the pixel format of this <see cref="PixelFormatDetails"/></returns>
	/// <remarks>
	/// <para>
	/// This method maps the RGBA color value to the specified pixel format and returns the encoded pixel value best approximating the given RGBA color value for the pixel format.
	/// </para>
	/// <para>
	/// If the pixel format is indexed and uses a <see cref="Palette"/> the index of the closest matching color in the palette will be returned.
	/// </para>
	/// <para>
	/// If the pixel format has no alpha component, the given alpha component value will be ignored (as it will be in indexed formats with a <see cref="Palette"/>).
	/// </para>
	/// <para>
	/// If the pixel format's <see cref="BitsPerPixel">bits-per-pixel</see> value (color depth) is less than 32-bits per pixel then the unused upper bits of the returned encoded pixel value can safely be ignored
	/// (e.g., with a 16-bits per pixel format the returned encoded pixel value can be safely cast (truncated) to an <see cref="ushort"/> value, and similarly to a <see cref="byte"/> value for an 8-bits per pixel formats).
	/// </para>
	/// </remarks>
	public readonly uint MapColor(Palette? palette, byte r, byte g, byte b, byte a)
	{
		unsafe
		{
			return SDL_MapRGBA(mFormat, palette is not null ? palette.Pointer : null, r, g, b, a);
		}
	}

	/// <summary>
	/// Maps a <see cref="Color{T}">Color</see>&lt;<see cref="byte"/>&gt; to an encoded pixel value
	/// </summary>
	/// <param name="palette">An optional <see cref="Palette"/> to use for indexed pixel formats</param>
	/// <param name="color">The <see cref="Color{T}">Color</see>&lt;<see cref="byte"/>&gt; to encoded</param>
	/// <returns>An encoded pixel value in the pixel format of this <see cref="PixelFormatDetails"/></returns>
	/// <remarks>
	/// <para>
	/// This method maps the <see cref="Color{T}">Color</see>&lt;<see cref="byte"/>&gt; to the specified pixel format and returns the encoded pixel value best approximating the given <see cref="Color{T}">Color</see>&lt;<see cref="byte"/>&gt; for the pixel format.
	/// </para>
	/// <para>
	/// If the pixel format is indexed and uses a <see cref="Palette"/> the index of the closest matching color in the palette will be returned.
	/// </para>
	/// <para>
	/// If the pixel format has no alpha component, the given alpha component value will be ignored (as it will be in indexed formats with a <see cref="Palette"/>).
	/// </para>
	/// <para>
	/// If the pixel format's <see cref="BitsPerPixel">bits-per-pixel</see> value (color depth) is less than 32-bits per pixel then the unused upper bits of the returned encoded pixel value can safely be ignored
	/// (e.g., with a 16-bits per pixel format the returned encoded pixel value can be safely cast (truncated) to an <see cref="ushort"/> value, and similarly to a <see cref="byte"/> value for an 8-bits per pixel formats).
	/// </para>
	/// </remarks>
	public readonly uint MapColor(Palette? palette, Color<byte> color) => MapColor(palette, color.R, color.G, color.B, color.A);

	/// <inheritdoc/>
	public readonly override string ToString() => ToString(format: default, formatProvider: default);

	/// <inheritdoc cref="IFormattable.ToString(string?, IFormatProvider?)"/>
	public readonly string ToString(IFormatProvider? formatProvider) => ToString(format: default, formatProvider);

	/// <inheritdoc cref="IFormattable.ToString(string?, IFormatProvider?)"/>
	public readonly string ToString(string? format) => ToString(format, formatProvider: default);

	/// <inheritdoc/>
	public readonly string ToString(string? format, IFormatProvider? formatProvider)
	{
		unsafe
		{
			return mFormat is not null
				? $"{{ {nameof(Format)}: {mFormat->Format}, {
					nameof(BitsPerPixel)}: {mFormat->BitsPerPixel.ToString(format, formatProvider)}, {
					nameof(BytesPerPixel)}: {mFormat->BytesPerPixel.ToString(format, formatProvider)}, {
					nameof(RMask)}: {mFormat->RMask:X8}, {
					nameof(GMask)}: {mFormat->GMask:X8}, {
					nameof(BMask)}: {mFormat->BMask:X8}, {
					nameof(AMask)}: {mFormat->AMask:X8}, {
					nameof(RBits)}: {mFormat->RBits.ToString(format, formatProvider)}, {
					nameof(GBits)}: {mFormat->GBits.ToString(format, formatProvider)}, {
					nameof(BBits)}: {mFormat->BBits.ToString(format, formatProvider)}, {
					nameof(ABits)}: {mFormat->ABits.ToString(format, formatProvider)}, {
					nameof(RShift)}: {mFormat->RShift.ToString(format, formatProvider)}, {
					nameof(GShift)}: {mFormat->GShift.ToString(format, formatProvider)}, {
					nameof(BShift)}: {mFormat->BShift.ToString(format, formatProvider)}, {
					nameof(AShift)}: {mFormat->AShift.ToString(format, formatProvider)} }}"
				: "{}";
		}
	}

	/// <inheritdoc/>
	public readonly bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format = default, IFormatProvider? provider = default)
	{
		unsafe
		{
			charsWritten = 0;

			return mFormat is not null
				?  SpanFormat.TryWrite($"{{ {nameof(Format)}: ", ref destination, ref charsWritten)
				&& SpanFormat.TryWrite(mFormat->Format, ref destination, ref charsWritten)
				&& SpanFormat.TryWrite($", {nameof(BitsPerPixel)}: ", ref destination, ref charsWritten)
				&& SpanFormat.TryWrite(mFormat->BitsPerPixel, ref destination, ref charsWritten, format, provider)
				&& SpanFormat.TryWrite($", {nameof(BytesPerPixel)}: ", ref destination, ref charsWritten)
				&& SpanFormat.TryWrite(mFormat->BytesPerPixel, ref destination, ref charsWritten, format, provider)
				&& SpanFormat.TryWrite($", {nameof(RMask)}: ", ref destination, ref charsWritten)
				&& SpanFormat.TryWrite(mFormat->RMask, ref destination, ref charsWritten, "X8")
				&& SpanFormat.TryWrite($", {nameof(GMask)}: ", ref destination, ref charsWritten)
				&& SpanFormat.TryWrite(mFormat->GMask, ref destination, ref charsWritten, "X8")
				&& SpanFormat.TryWrite($", {nameof(BMask)}: ", ref destination, ref charsWritten)
				&& SpanFormat.TryWrite(mFormat->BMask, ref destination, ref charsWritten, "X8")
				&& SpanFormat.TryWrite($", {nameof(AMask)}: ", ref destination, ref charsWritten)
				&& SpanFormat.TryWrite(mFormat->AMask, ref destination, ref charsWritten, "X8")
				&& SpanFormat.TryWrite($", {nameof(RBits)}: ", ref destination, ref charsWritten)
				&& SpanFormat.TryWrite(mFormat->RBits, ref destination, ref charsWritten, format, provider)
				&& SpanFormat.TryWrite($", {nameof(GBits)}: ", ref destination, ref charsWritten)
				&& SpanFormat.TryWrite(mFormat->GBits, ref destination, ref charsWritten, format, provider)
				&& SpanFormat.TryWrite($", {nameof(BBits)}: ", ref destination, ref charsWritten)
				&& SpanFormat.TryWrite(mFormat->BBits, ref destination, ref charsWritten, format, provider)
				&& SpanFormat.TryWrite($", {nameof(ABits)}: ", ref destination, ref charsWritten)
				&& SpanFormat.TryWrite(mFormat->ABits, ref destination, ref charsWritten, format, provider)
				&& SpanFormat.TryWrite($", {nameof(RShift)}: ", ref destination, ref charsWritten)
				&& SpanFormat.TryWrite(mFormat->RShift, ref destination, ref charsWritten, format, provider)
				&& SpanFormat.TryWrite($", {nameof(GShift)}: ", ref destination, ref charsWritten)
				&& SpanFormat.TryWrite(mFormat->GShift, ref destination, ref charsWritten, format, provider)
				&& SpanFormat.TryWrite($", {nameof(BShift)}: ", ref destination, ref charsWritten)
				&& SpanFormat.TryWrite(mFormat->BShift, ref destination, ref charsWritten, format, provider)
				&& SpanFormat.TryWrite($", {nameof(AShift)}: ", ref destination, ref charsWritten)
				&& SpanFormat.TryWrite(mFormat->AShift, ref destination, ref charsWritten, format, provider)
				&& SpanFormat.TryWrite(" }", ref destination, ref charsWritten)
				: SpanFormat.TryWrite("{}", ref destination, ref charsWritten);
		}
	}
}
