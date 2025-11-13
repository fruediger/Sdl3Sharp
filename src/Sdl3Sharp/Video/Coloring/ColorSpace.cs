using Sdl3Sharp.Internal;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sdl3Sharp.Video.Coloring;

/// <summary>
/// Represents a color space
/// </summary>
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
[StructLayout(LayoutKind.Sequential)]
public readonly partial struct ColorSpace :
	IEquatable<ColorSpace>, IFormattable, ISpanFormattable, IEqualityOperators<ColorSpace, ColorSpace, bool>
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly string DebuggerDisplay => ToString();

	private readonly uint mCSpace;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private ColorSpace(uint cspace) => mCSpace = cspace;

	/// <summary>
	/// Creates a new <see cref="ColorSpace"/> from specified color type, color range, color primaries, transfer characteristics, matrix coefficients and chroma sampling location values
	/// </summary>
	/// <param name="type">The color type of the <see cref="ColorSpace"/></param>
	/// <param name="range">The color range of the <see cref="ColorSpace"/></param>
	/// <param name="primaries">The color primaries of the <see cref="ColorSpace"/></param>
	/// <param name="transfer">The transfer characteristics of the <see cref="ColorSpace"/></param>
	/// <param name="matrix">The matrix coefficients of the <see cref="ColorSpace"/></param>
	/// <param name="chroma">The chroma sampling location of the <see cref="ColorSpace"/></param>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public ColorSpace(ColorType type, ColorRange range, ColorPrimaries primaries, TransferCharacteristics transfer, MatrixCoefficients matrix, ChromaLocation chroma) :
		this(unchecked(
			  (((uint)type      & 0x0fu) << 28)
			| (((uint)range     & 0x0fu) << 24)
			| (((uint)chroma    & 0x0fu) << 20)
			| (((uint)primaries & 0x1fu) << 10)
			| (((uint)transfer  & 0x1fu) << 5)
			| (((uint)matrix    & 0x1fu) << 0)
		))
	{ }

	/// <summary>
	/// Gets the chroma sampling location of the <see cref="ColorSpace"/>
	/// </summary>
	/// <value>
	/// The chroma sampling location of the <see cref="ColorSpace"/>
	/// </value> 
	public readonly ChromaLocation Chroma { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => unchecked((ChromaLocation)((mCSpace >> 20) & 0x0fu)); }

	/// <summary>
	/// Gets a value indicating whether the <see cref="ColorSpace"/> uses full color range
	/// </summary>
	/// <value>
	/// A value indicating whether the <see cref="ColorSpace"/> uses full color range
	/// </value>
	public readonly bool IsFullRange { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => Range is ColorRange.Full; }

	/// <summary>
	/// Gets a value indicating whether the <see cref="ColorSpace"/> uses limited color range
	/// </summary>
	/// <value>
	/// A value indicating whether the <see cref="ColorSpace"/> uses limited color range
	/// </value>
	public readonly bool IsLimitedRange { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => Range is not ColorRange.Full; }

	/// <summary>
	/// Gets a value indicating whether the <see cref="ColorSpace"/> uses <see cref="MatrixCoefficients.Bt2020Ncl">BT2020 non-constant luminance</see> matrix coefficients
	/// </summary>
	/// <value>
	/// A value indicating whether the <see cref="ColorSpace"/> uses <see cref="MatrixCoefficients.Bt2020Ncl">BT2020 non-constant luminance</see> matrix coefficients
	/// </value>
	public readonly bool IsMatrixBt2020Ncl { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => Matrix is MatrixCoefficients.Bt2020Ncl; }

	/// <summary>
	/// Gets a value indicating whether the <see cref="ColorSpace"/> uses <see cref="MatrixCoefficients.Bt601">BT601</see> (or <see cref="MatrixCoefficients.Bt470Bg">BT470BG</see>) matrix coefficients
	/// </summary>
	/// <value>
	/// A value indicating whether the <see cref="ColorSpace"/> uses <see cref="MatrixCoefficients.Bt601">BT601</see> (or <see cref="MatrixCoefficients.Bt470Bg">BT470BG</see>) matrix coefficients
	/// </value>
	public readonly bool IsMatrixBt601 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => Matrix is MatrixCoefficients.Bt601 or MatrixCoefficients.Bt470Bg; }

	/// <summary>
	/// Gets a value indicating whether the <see cref="ColorSpace"/> uses <see cref="MatrixCoefficients.Bt709">BT709</see> matrix coefficients
	/// </summary>
	/// <value>
	/// A value indicating whether the <see cref="ColorSpace"/> uses <see cref="MatrixCoefficients.Bt709">BT709</see> matrix coefficients
	/// </value>
	public readonly bool IsMatrixBt709 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => Matrix is MatrixCoefficients.Bt709; }

	/// <summary>
	/// Gets the matrix coefficients of the <see cref="ColorSpace"/>
	/// </summary>
	/// <value>
	/// The matrix coefficients of the <see cref="ColorSpace"/>
	/// </value>
	public readonly MatrixCoefficients Matrix { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => unchecked((MatrixCoefficients)((mCSpace >> 0) & 0x1fu)); }

	/// <summary>
	/// Gets the color primaries of the <see cref="ColorSpace"/>
	/// </summary>
	/// <value>
	/// The color primaries of the <see cref="ColorSpace"/>
	/// </value>
	public readonly ColorPrimaries Primaries { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => unchecked((ColorPrimaries)((mCSpace >> 10) & 0x1f)); }

	/// <summary>
	/// Gets the color range of the <see cref="ColorSpace"/>
	/// </summary>
	/// <value>
	/// The color range of the <see cref="ColorSpace"/>
	/// </value>
	public readonly ColorRange Range { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => unchecked((ColorRange)((mCSpace >> 24) & 0x0fu)); }

	/// <summary>
	/// Gets the transfer characteristics of the <see cref="ColorSpace"/>
	/// </summary>
	/// <value>
	/// The transfer characteristics of the <see cref="ColorSpace"/>
	/// </value>
	public readonly TransferCharacteristics Transfer { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => unchecked((TransferCharacteristics)((mCSpace >> 5) & 0x1fu)); }

	/// <summary>
	/// Gets the color type of the <see cref="ColorSpace"/>
	/// </summary>
	/// <value>
	/// The color type of the <see cref="ColorSpace"/>
	/// </value>
	public readonly ColorType Type { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => unchecked((ColorType)((mCSpace >> 28) & 0x0fu)); }

	/// <inheritdoc/>
	public readonly override bool Equals([NotNullWhen(true)] object? obj) => obj is ColorSpace other && Equals(other);

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly bool Equals(ColorSpace other) => mCSpace == other.mCSpace;

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly override int GetHashCode() => mCSpace.GetHashCode();

	/// <inheritdoc/>
	public readonly override string ToString()
		=> $"{{ {nameof(Type)}: {Type}, {
			nameof(Range)}: {Range}, {
			nameof(Primaries)}: {Primaries}, {
			nameof(Transfer)}: {Transfer}, {
			nameof(Matrix)}: {Matrix}, {
			nameof(Chroma)}: {Chroma} }}";

	/// <inheritdoc/>
	readonly string IFormattable.ToString(string? format, IFormatProvider? formatProvider) => ToString();

	/// <inheritdoc cref="ISpanFormattable.TryFormat(Span{char}, out int, ReadOnlySpan{char}, IFormatProvider?)"/>
	public readonly bool TryFormat(Span<char> destination, out int charsWritten)
	{
		charsWritten = 0;

		return SpanFormat.TryWrite($"{{ {nameof(Type)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(Type, ref destination, ref charsWritten)
			&& SpanFormat.TryWrite($", {nameof(Range)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(Range, ref destination, ref charsWritten)
			&& SpanFormat.TryWrite($", {nameof(Primaries)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(Primaries, ref destination, ref charsWritten)
			&& SpanFormat.TryWrite($", {nameof(Transfer)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(Transfer, ref destination, ref charsWritten)
			&& SpanFormat.TryWrite($", {nameof(Matrix)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(Matrix, ref destination, ref charsWritten)
			&& SpanFormat.TryWrite($", {nameof(Chroma)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(Chroma, ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(" }", ref destination, ref charsWritten);
	}

	/// <inheritdoc/>
	readonly bool ISpanFormattable.TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider) => TryFormat(destination, out charsWritten);

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator ==(ColorSpace left, ColorSpace right) => left.mCSpace == right.mCSpace;

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator !=(ColorSpace left, ColorSpace right) => left.mCSpace != right.mCSpace;
}
