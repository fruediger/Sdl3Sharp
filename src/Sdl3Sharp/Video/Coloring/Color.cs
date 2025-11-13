using Sdl3Sharp.Internal;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sdl3Sharp.Video.Coloring;

/// <summary>
/// Provides static methods for creating <see cref="Color{T}"/> values
/// </summary>
public static class Color
{
	/// <summary>
	/// Creates a new <see cref="Color{T}">Color</see>&lt;<see cref="byte"/>&gt; value from the specified red, green, blue and alpha component values as <see cref="byte"/> values
	/// </summary>
	/// <param name="r">The red component value</param>
	/// <param name="g">The green component value</param>
	/// <param name="b">The blue component value</param>
	/// <param name="a">The alpha component value</param>
	/// <returns>A new <see cref="Color{T}">Color</see>&lt;<see cref="byte"/>&gt; value</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static Color<byte> From(byte r, byte g, byte b, byte a) => new(r, g, b, a);

	/// <summary>
	/// Creates a new <see cref="Color{T}">Color</see>&lt;<see cref="byte"/>&gt; value from the specified red, green and blue component values as <see cref="byte"/> values
	/// </summary>
	/// <param name="r">The red component value</param>
	/// <param name="g">The green component value</param>
	/// <param name="b">The blue component value</param>
	/// <returns>A new <see cref="Color{T}">Color</see>&lt;<see cref="byte"/>&gt; value</returns>
	/// <remarks>
	/// <para>
	/// The alpha component value of the resulting <see cref="Color{T}">Color</see>&lt;<see cref="byte"/>&gt; value will be set to <c>255</c> (fully opaque).
	/// </para>
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static Color<byte> From(byte r, byte g, byte b) => From(r, g, b, 255);

	/// <summary>
	/// Creates a new <see cref="Color{T}">Color</see>&lt;<see cref="byte"/>&gt; value from a specified <see cref="byte"/> value
	/// </summary>
	/// <param name="value">The <see cref="byte"/> value to use for the red, green and blue component value</param>
	/// <returns>A new <see cref="Color{T}">Color</see>&lt;<see cref="byte"/>&gt; value</returns>
	/// <remarks>
	/// <para>
	/// The red, green and blue component values of the resulting <see cref="Color{T}">Color</see>&lt;<see cref="byte"/>&gt; value will all be set to the specified <paramref name="value"/>.
	/// This results in a gray-scale color where <c>0</c> is black and <c>255</c> is white.
	/// </para>
	/// <para>
	/// The alpha component value of the resulting <see cref="Color{T}">Color</see>&lt;<see cref="byte"/>&gt; value will be set to <c>255</c> (fully opaque).
	/// </para>
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static Color<byte> From(byte value) => From(value, value, value);

	/// <summary>
	/// Creates a new <see cref="Color{T}">Color</see>&lt;<see cref="float"/>&gt; value from the specified red, green, blue and alpha component values as <see cref="float"/> values
	/// </summary>
	/// <param name="r">The red component value</param>
	/// <param name="g">The green component value</param>
	/// <param name="b">The blue component value</param>
	/// <param name="a">The alpha component value</param>
	/// <returns>A new <see cref="Color{T}">Color</see>&lt;<see cref="float"/>&gt; value</returns>
	/// <remarks>
	/// <para>
	/// The <paramref name="r"/>ed, <paramref name="g"/>reen, <paramref name="b"/>lue and <paramref name="a"/>lpha component values are expected to be in the range from <c>0</c> to <c>1</c>, where <c>0</c> is the minimum component intensity and <c>1</c> is the maximum component intensity.
	/// </para>
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static Color<float> From(float r, float g, float b, float a) => new(r, g, b, a);

	/// <summary>
	/// Creates a new <see cref="Color{T}">Color</see>&lt;<see cref="float"/>&gt; value from the specified red, green and blue component values as <see cref="float"/> values
	/// </summary>
	/// <param name="r">The red component value</param>
	/// <param name="g">The green component value</param>
	/// <param name="b">The blue component value</param>
	/// <returns>A new <see cref="Color{T}">Color</see>&lt;<see cref="float"/>&gt; value</returns>
	/// <remarks>
	/// <para>
	/// The <paramref name="r"/>ed, <paramref name="g"/>reen and <paramref name="b"/>lue component values are expected to be in the range from <c>0</c> to <c>1</c>, where <c>0</c> is the minimum component intensity and <c>1</c> is the maximum component intensity.
	/// </para>
	/// <para>
	/// The alpha component value of the resulting <see cref="Color{T}">Color</see>&lt;<see cref="float"/>&gt; value will be set to <c>1</c> (fully opaque).
	/// </para>
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static Color<float> From(float r, float g, float b) => From(r, g, b, 1);

	/// <summary>
	/// Creates a new <see cref="Color{T}">Color</see>&lt;<see cref="byte"/>&gt; value from a specified <see cref="float"/> value
	/// </summary>
	/// <param name="value">The <see cref="float"/> value to use for the red, green and blue component value</param>
	/// <returns>A new <see cref="Color{T}">Color</see>&lt;<see cref="float"/>&gt; value</returns>
	/// <remarks>
	/// <para>
	/// The <paramref name="value"/> used the the red, green and blue component values is expected to be in the range from <c>0</c> to <c>1</c>.
	/// </para>
	/// <para>
	/// The red, green and blue component values of the resulting <see cref="Color{T}">Color</see>&lt;<see cref="float"/>&gt; value will all be set to the specified <paramref name="value"/>.
	/// This results in a gray-scale color where <c>0</c> is black and <c>1</c> is white.
	/// </para>
	/// <para>
	/// The alpha component value of the resulting <see cref="Color{T}">Color</see>&lt;<see cref="float"/>&gt; value will be set to <c>1</c> (fully opaque).
	/// </para>
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static Color<float> From(float value) => From(value, value, value);
}

/// <summary>
/// Represents a color with red, green, blue and alpha components
/// </summary>
/// <typeparam name="T">The type of the color components</typeparam>
/// <param name="r">The value of the red component</param>
/// <param name="g">The value of the green component</param>
/// <param name="b">The value of the blue component</param>
/// <param name="a">The value of the alpha component</param>
/// <remarks>
/// <para>
/// Most often the type <typeparamref name="T"/> is either <see cref="byte"/> for 32-bit pixel values with unsigned integer ranges from 0 to 255 for each component,
/// or <see cref="float"/> for 128-bit pixel values with floating-point ranges from 0 to 1 for each component.
/// </para>
/// <para>
/// <see cref="Color{T}">Color</see>&lt;<see cref="byte"/>&gt; values can be directly reinterpreted as an integer-packed color which uses the <see cref="PixelFormat.Rgba32"/> format (<see cref="PixelFormat.Abgr8888"/> on little-endian systems and <see cref="PixelFormat.Rgba8888"/> on big-endian systems).
/// </para>
/// <para>
/// <see cref="Color{T}">Color</see>&lt;<see cref="float"/>&gt; values can be directly reinterpreted as a float-packed color which uses the <see cref="PixelFormat.Rgba128Float"/> format.
/// </para>
/// </remarks>
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
[StructLayout(LayoutKind.Sequential)]
[method: MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization), SetsRequiredMembers]
public readonly struct Color<T>(in T r, in T g, in T b, in T a) :
	IEquatable<Color<T>>, IFormattable, ISpanFormattable, IEqualityOperators<Color<T>, Color<T>, bool>
	where T :
		unmanaged, IEquatable<T>, IFormattable, ISpanFormattable, IEqualityOperators<T, T, bool>
{	
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly string DebuggerDisplay => ToString();

	private readonly T mR = r, mG = g, mB = b, mA = a;

	/// <summary>
	/// Gets or initializes the red component value of the color
	/// </summary>
	/// <value>
	/// The red component value of the color
	/// </value>
	public required readonly T R
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		get => mR;

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		init => mR = value;
	}

	/// <summary>
	/// Gets or initializes the green component value of the color
	/// </summary>
	/// <value>
	/// The green component value of the color
	/// </value>
	public required readonly T G
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		get => mG;

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		init => mG = value;
	}

	/// <summary>
	/// Gets or initializes the blue component value of the color
	/// </summary>
	/// <value>
	/// The blue component value of the color
	/// </value>
	public required readonly T B
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		get => mB;

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		init => mB = value;
	}

	/// <summary>
	/// Gets or initializes the alpha component value of the color
	/// </summary>
	/// <value>
	/// The alpha component value of the color
	/// </value>
	public required readonly T A
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		get => mA;

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		init => mA = value;
	}

	/// <summary>
	/// Deconstructs the color into its red, green, blue and alpha component values
	/// </summary>
	/// <param name="r">The red component value of the color</param>
	/// <param name="g">The green component value of the color</param>
	/// <param name="b">The blue component value of the color</param>
	/// <param name="a">The alpha component value of the color</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly void Deconstruct(out T r, out T g, out T b, out T a) { r = mR; g = mG; b = mB; a = mA; }

	/// <inheritdoc/>
	public readonly override bool Equals([NotNullWhen(true)] object? obj) => obj is Color<T> other && Equals(other);

	/// <inheritdoc cref="IEquatable{T}.Equals(T)"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly bool Equals(in Color<T> other)
		=> mR.Equals(other.mR)
		&& mG.Equals(other.mG)
		&& mB.Equals(other.mB)
		&& mA.Equals(other.mA);

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	readonly bool IEquatable<Color<T>>.Equals(Color<T> other) => Equals(other);

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly override int GetHashCode() => HashCode.Combine(mR, mG, mB, mA);

	/// <inheritdoc/>
	public readonly override string ToString() => ToString(format: default, formatProvider: default);

	/// <inheritdoc cref="IFormattable.ToString(string?, IFormatProvider?)"/>
	public readonly string ToString(IFormatProvider? formatProvider) => ToString(format: default, formatProvider);

	/// <inheritdoc cref="IFormattable.ToString(string?, IFormatProvider?)"/>
	public readonly string ToString(string? format) => ToString(format, formatProvider: default);

	/// <inheritdoc/>
	public readonly string ToString(string? format, IFormatProvider? formatProvider)
		=> $"{{ {nameof(R)}: {mR.ToString(format, formatProvider)}, {
			nameof(G)}: {mG.ToString(format, formatProvider)}, {
			nameof(B)}: {mB.ToString(format, formatProvider)}, {
			nameof(A)}: {mA.ToString(format, formatProvider)} }}";

	/// <inheritdoc/>
	public readonly bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format = default, IFormatProvider? provider = default)
	{
		charsWritten = 0;

		return SpanFormat.TryWrite($"{{ {nameof(R)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(in mR, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite($", {nameof(G)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(in mG, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite($", {nameof(B)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(in mB, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite($", {nameof(A)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(in mA, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite(" }", ref destination, ref charsWritten);
	}

	/// <inheritdoc cref="IEqualityOperators{TSelf, TOther, TResult}.operator=="/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator ==(in Color<T> left, in Color<T> right)
		=> left.mR == right.mR
		&& left.mG == right.mG
		&& left.mB == right.mB
		&& left.mA == right.mA;

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static bool IEqualityOperators<Color<T>, Color<T>, bool>.operator ==(Color<T> left, Color<T> right) => left == right;

	/// <inheritdoc cref="IEqualityOperators{TSelf, TOther, TResult}.operator!="/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator !=(in Color<T> left, in Color<T> right)
		=> left.mR != right.mR
		|| left.mG != right.mG
		|| left.mB != right.mB
		|| left.mA != right.mA;

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static bool IEqualityOperators<Color<T>, Color<T>, bool>.operator !=(Color<T> left, Color<T> right) => left != right;
}
