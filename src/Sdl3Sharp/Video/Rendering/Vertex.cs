using Sdl3Sharp.Internal;
using Sdl3Sharp.Video.Coloring;
using Sdl3Sharp.Video.Drawing;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sdl3Sharp.Video.Rendering;

/// <summary>
/// Provides static methods for creating <see cref="Vertex{TPosition, TColor, TTexCoord}">vertices</see>
/// </summary>
public static class Vertex
{
	/// <summary>
	/// Creates a new <see cref="Vertex{TPosition, TColor, TTexCoord}">Vertex</see>&lt;<see cref="float"/>, <see cref="float"/>, <see cref="float"/>&gt; from the specified position, color, and texture coordinate
	/// </summary>
	/// <param name="position">The position of the vertex in <see cref="Renderer"/> coordinates</param>
	/// <param name="color">The color of the vertex</param>
	/// <param name="texCoord">The texture coordinate of the vertex, normalized to the range from <c>0</c> to <c>1</c></param>
	/// <returns>A new <see cref="Vertex{TPosition, TColor, TTexCoord}"/> instance</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static Vertex<float, float, float> From(Point<float> position, Color<float> color, Point<float> texCoord)
		=> new(position, color, texCoord);
}

/// <summary>
/// Represents a vertex structure
/// </summary>
/// <typeparam name="TPosition">The type of the position components</typeparam>
/// <typeparam name="TColor">The type of the color components</typeparam>
/// <typeparam name="TTexCoord">The type of the texture coordinate components</typeparam>
/// <param name="position">The position of the vertex in <see cref="Renderer"/> coordinates</param>
/// <param name="color">The color of the vertex</param>
/// <param name="texCoord">The texture coordinate of the vertex; for floating point <typeparamref name="TTexCoord"/> types, this is typically normalized to the range from <c>0</c> to <c>1</c></param>
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
[StructLayout(LayoutKind.Sequential)]
[method: MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization), SetsRequiredMembers]
public readonly struct Vertex<TPosition, TColor, TTexCoord>(in Point<TPosition> position, in Color<TColor> color, in Point<TTexCoord> texCoord) :
	IEquatable<Vertex<TPosition, TColor, TTexCoord>>, IFormattable, ISpanFormattable, IEqualityOperators<Vertex<TPosition, TColor, TTexCoord>, Vertex<TPosition, TColor, TTexCoord>, bool>
	where TPosition : unmanaged, IEquatable<TPosition>, IFormattable, ISpanFormattable, IEqualityOperators<TPosition, TPosition, bool>
	where TColor : unmanaged, IEquatable<TColor>, IFormattable, ISpanFormattable, IEqualityOperators<TColor, TColor, bool>
	where TTexCoord : unmanaged, IEquatable<TTexCoord>, IFormattable, ISpanFormattable, IEqualityOperators<TTexCoord, TTexCoord, bool>
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly string DebuggerDisplay => ToString(formatProvider: CultureInfo.InvariantCulture);

	private readonly Point<TPosition> mPosition = position;
	private readonly Color<TColor> mColor = color;
	private readonly Point<TTexCoord> mTexCoord = texCoord;

	/// <summary>
	/// Gets or initializes the position of the vertex
	/// </summary>
	/// <value>
	/// The position of the vertex in <see cref="Renderer"/> coordinates
	/// </value>
	public required readonly Point<TPosition> Position
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		get => mPosition;

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		init => mPosition = value;
	}

	/// <summary>
	/// Gets or initializes the color of the vertex
	/// </summary>
	/// <value>
	/// The color of the vertex
	/// </value>
	public required readonly Color<TColor> Color
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		get => mColor;

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		init => mColor = value;
	}

	/// <summary>
	/// Gets or initializes the texture coordinate of the vertex
	/// </summary>
	/// <value>
	/// The texture coordinate of the vertex; for floating point <typeparamref name="TTexCoord"/> types, this is typically normalized to the range from <c>0</c> to <c>1</c>
	/// </value>
	public required readonly Point<TTexCoord> TexCoord
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		get => mTexCoord;

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		init => mTexCoord = value;
	}

	/// <summary>
	/// Deconstructs the vertex into its position, color, and texture coordinate components
	/// </summary>
	/// <param name="position">The position of the vertex in <see cref="Renderer"/> coordinates</param>
	/// <param name="color">The color of the vertex</param>
	/// <param name="texCoord">The texture coordinate of the vertex; for floating point <typeparamref name="TTexCoord"/> types, this is typically normalized to the range from <c>0</c> to <c>1</c></param>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly void Deconstruct(out Point<TPosition> position, out Color<TColor> color, out Point<TTexCoord> texCoord)
	{
		position = mPosition;
		color = mColor;
		texCoord = mTexCoord;
	}

	/// <inheritdoc/>
	public readonly override bool Equals([NotNullWhen(true)] object? obj) => obj is Vertex<TPosition, TColor, TTexCoord> other && Equals(other);

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly bool Equals(in Vertex<TPosition, TColor, TTexCoord> other)
		=> mPosition.Equals(in other.mPosition)
		&& mColor.Equals(in other.mColor)
		&& mTexCoord.Equals(in other.mTexCoord);

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	readonly bool IEquatable<Vertex<TPosition, TColor, TTexCoord>>.Equals(Vertex<TPosition, TColor, TTexCoord> other) => Equals(other);

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly override int GetHashCode() => HashCode.Combine(mPosition, mColor, mTexCoord);

	/// <inheritdoc/>
	public readonly override string ToString() => ToString(format: default, formatProvider: default);

	/// <inheritdoc cref="IFormattable.ToString(string?, IFormatProvider?)" />
	public readonly string ToString(IFormatProvider? formatProvider) => ToString(format: default, formatProvider);

	/// <inheritdoc cref="IFormattable.ToString(string?, IFormatProvider?)" />
	public readonly string ToString(string? format) => ToString(format, formatProvider: default);

	/// <inheritdoc/>
	public readonly string ToString(string? format, IFormatProvider? formatProvider)
		=> $"{{ {nameof(Position)}: {Position.ToString(format, formatProvider)}, {
			nameof(Color)}: {Color.ToString(format, formatProvider)}, {
			nameof(TexCoord)}: {TexCoord.ToString(format, formatProvider)} }}";

	/// <inheritdoc/>
	public readonly bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format = default, IFormatProvider? provider = default)
	{
		charsWritten = 0;

		return SpanFormat.TryWrite($"{{ {nameof(Position)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(in mPosition, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite($", {nameof(Color)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(in mColor, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite($", {nameof(TexCoord)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(in mTexCoord, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite(" }", ref destination, ref charsWritten);
	}

	/// <inheritdoc cref="IEqualityOperators{TSelf, TOther, TResult}.operator=="/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator ==(in Vertex<TPosition, TColor, TTexCoord> left, in Vertex<TPosition, TColor, TTexCoord> right)
		=> left.mPosition == right.mPosition
		&& left.mColor == right.mColor
		&& left.mTexCoord == right.mTexCoord;

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static bool IEqualityOperators<Vertex<TPosition, TColor, TTexCoord>, Vertex<TPosition, TColor, TTexCoord>, bool>.operator ==(Vertex<TPosition, TColor, TTexCoord> left, Vertex<TPosition, TColor, TTexCoord> right)
		=> left == right;

	/// <inheritdoc cref="IEqualityOperators{TSelf, TOther, TResult}.operator!="/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator !=(in Vertex<TPosition, TColor, TTexCoord> left, in Vertex<TPosition, TColor, TTexCoord> right)
		=> left.mPosition != right.mPosition
		|| left.mColor != right.mColor
		|| left.mTexCoord != right.mTexCoord;

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static bool IEqualityOperators<Vertex<TPosition, TColor, TTexCoord>, Vertex<TPosition, TColor, TTexCoord>, bool>.operator !=(Vertex<TPosition, TColor, TTexCoord> left, Vertex<TPosition, TColor, TTexCoord> right)
		=> left != right;
}
