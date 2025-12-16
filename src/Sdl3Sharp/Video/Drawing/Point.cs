using Sdl3Sharp.Internal;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sdl3Sharp.Video.Drawing;

/// <summary>
/// Provides extension methods for <see cref="Point{T}"/>
/// </summary>
public static class Point
{
	/// <summary>
	/// Indicates whether two points are equal within a specified epsilon
	/// </summary>
	/// <typeparam name="T">The type of the coordinates of the points</typeparam>
	/// <param name="value">The first point</param>
	/// <param name="other">The second point</param>
	/// <param name="epsilon">The tolerance for equality</param>
	/// <param name="_"><em>Please ignore and do not explicitly set this parameter</em></param>
	/// <returns><c><see langword="true"/></c>, if the points are considered equal within the specified epsilon; otherwise, <c><see langword="false"/></c></returns>
	/// <remarks>
	/// <para>
	/// This method works for types of <paramref name="value"/> and <paramref name="other"/> <typeparamref name="T"/> which implement <see cref="IComparable{T}"/>.
	/// If <typeparamref name="T"/> implements both <see cref="IComparable{T}"/> and <see cref="IComparisonOperators{TSelf, TOther, TResult}"/>,
	/// overload resolution prioritizes <see cref="EqualsWithinEpsilon{T}(in Point{T}, in Point{T}, in T, Utilities.Math.IDispatchComparisonOperators{T}?)"/> instead.
	/// </para>
	/// <para>
	/// <em>Please ignore and do not explicitly set the <paramref name="_"/> parameter</em>. This parameter is just used in the method's signature to dispatch calls to this method for types of <paramref name="value"/> and <paramref name="other"/> <typeparamref name="T"/> which implement <see cref="IComparable{T}"/>.
	/// </para>
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool EqualsWithinEpsilon<T>(in this Point<T> value, in Point<T> other, in T epsilon, Utilities.Math.IDispatchComparable<T>? _ = default)
		where T : unmanaged, IEquatable<T>, IFormattable, ISpanFormattable, IEqualityOperators<T, T, bool>, IComparable<T>, INumberBase<T>
		=> T.Abs(value.X - other.X).CompareTo(epsilon) is <= 0
		&& T.Abs(value.Y - other.Y).CompareTo(epsilon) is <= 0;

	/// <summary>
	/// Indicates whether two points are equal within a specified epsilon
	/// </summary>
	/// <typeparam name="T">The type of the coordinates of the points</typeparam>
	/// <param name="value">The first point</param>
	/// <param name="other">The second point</param>
	/// <param name="epsilon">The tolerance for equality</param>
	/// <param name="_"><em>Please ignore and do not explicitly set this parameter</em></param>
	/// <returns><c><see langword="true"/></c>, if the points are considered equal within the specified epsilon; otherwise, <c><see langword="false"/></c></returns>
	/// <remarks>
	/// <para>
	/// This method works for types of <paramref name="value"/> and <paramref name="other"/> <typeparamref name="T"/> which implement <see cref="IComparisonOperators{TSelf, TOther, TResult}"/>.
	/// If <typeparamref name="T"/> implements both <see cref="IComparisonOperators{TSelf, TOther, TResult}"/> and <see cref="IComparable{T}"/>,
	/// overload resolution prioritizes this method.
	/// </para>
	/// <para>
	/// <em>Please ignore and do not explicitly set the <paramref name="_"/> parameter</em>. This parameter is just used in the method's signature to dispatch calls to this method for types of <paramref name="value"/> and <paramref name="other"/> <typeparamref name="T"/> which implement <see cref="IComparisonOperators{TSelf, TOther, TResult}"/>.
	/// </para>
	/// </remarks>
	[OverloadResolutionPriority(1)]
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool EqualsWithinEpsilon<T>(in this Point<T> value, in Point<T> other, in T epsilon, Utilities.Math.IDispatchComparisonOperators<T>? _ = default)
		where T : unmanaged, IEquatable<T>, IFormattable, ISpanFormattable, IEqualityOperators<T, T, bool>, IComparisonOperators<T, T, bool>, INumberBase<T>
		=> T.Abs(value.X - other.X) <= epsilon
		&& T.Abs(value.Y - other.Y) <= epsilon;
}

/// <summary>
/// Represents a point in a two-dimensional coordinate system
/// </summary>
/// <typeparam name="T">The type of the coordinates</typeparam>
/// <param name="x">The X coordinate of the point</param>
/// <param name="y">The Y coordinate of the point</param>
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
[StructLayout(LayoutKind.Sequential)]
[method: MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization), SetsRequiredMembers]
public readonly struct Point<T>(in T x, in T y) :
	IEquatable<Point<T>>, IFormattable, ISpanFormattable, IEqualityOperators<Point<T>, Point<T>, bool>
	where T :
		unmanaged, IEquatable<T>, IFormattable, ISpanFormattable, IEqualityOperators<T, T, bool>
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly string DebuggerDisplay => ToString(formatProvider: CultureInfo.InvariantCulture);

	private readonly T mX = x, mY = y;

	/// <summary>
	/// Gets or initializes the X coordinate of the point
	/// </summary>
	/// <value>
	/// The X coordinate of the point
	/// </value>
	public required readonly T X
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		get => mX;

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		init => mX = value;
	}

	/// <summary>
	/// Gets or initializes the Y coordinate of the point
	/// </summary>
	/// <value>
	/// The Y coordinate of the point
	/// </value>
	public required readonly T Y
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		get => mY;

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		init => mY = value;
	}

	/// <summary>
	/// Deconstructs the point into its X and Y coordinates
	/// </summary>
	/// <param name="x">The X coordinate of the point</param>
	/// <param name="y">The Y coordinate of the point</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly void Deconstruct(out T x, out T y) { x = mX; y = mY; }

	/// <inheritdoc/>
	public readonly override bool Equals([NotNullWhen(true)] object? obj) => obj is Point<T> other && Equals(other);

	/// <inheritdoc cref="IEquatable{T}.Equals(T)"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly bool Equals(in Point<T> other)
		=> mX.Equals(other.mX)
		&& mY.Equals(other.mY);

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	readonly bool IEquatable<Point<T>>.Equals(Point<T> other) => Equals(other);

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly override int GetHashCode() => HashCode.Combine(mX, mY);

	/// <inheritdoc/>
	public readonly override string ToString() => ToString(format: default, formatProvider: default);

	/// <inheritdoc cref="IFormattable.ToString(string?, IFormatProvider?)" />
	public readonly string ToString(IFormatProvider? formatProvider) => ToString(format: default, formatProvider);

	/// <inheritdoc cref="IFormattable.ToString(string?, IFormatProvider?)" />
	public readonly string ToString(string? format) => ToString(format, formatProvider: default);

	/// <inheritdoc/>
	public readonly string ToString(string? format, IFormatProvider? formatProvider)
		=> $"{{ {nameof(X)}: {X.ToString(format, formatProvider)}, {
			nameof(Y)}: {Y.ToString(format, formatProvider)} }}";

	/// <inheritdoc/>
	public readonly bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format = default, IFormatProvider? provider = default)
	{
		charsWritten = 0;

		return SpanFormat.TryWrite($"{{ {nameof(X)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(in mX, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite($", {nameof(Y)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(in mY, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite(" }", ref destination, ref charsWritten);
	}

	/// <inheritdoc cref="IEqualityOperators{TSelf, TOther, TResult}.operator==" />
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator ==(in Point<T> left, in Point<T> right)
		=> left.mX == right.mX
		&& left.mY == right.mY;

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static bool IEqualityOperators<Point<T>, Point<T>, bool>.operator ==(Point<T> left, Point<T> right) => left == right;

	/// <inheritdoc cref="IEqualityOperators{TSelf, TOther, TResult}.operator!=" />
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator !=(in Point<T> left, in Point<T> right)
		=> left.mX != right.mX
		|| left.mY != right.mY;

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static bool IEqualityOperators<Point<T>, Point<T>, bool>.operator !=(Point<T> left, Point<T> right) => left != right;

	/// <summary>
	/// Converts a tuple of (X, Y) to a <see cref="Point{T}"/>
	/// </summary>
	/// <param name="xy">The tuple containing the X and Y coordinates</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static implicit operator Point<T>(in (T X, T Y) xy) => new(in xy.X, in xy.Y);

	/// <summary>
	/// Converts a <see cref="Point{T}"/> to a tuple of (X, Y)
	/// </summary>
	/// <param name="point">The point to convert</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static implicit operator (T X, T Y)(in Point<T> point) => (point.mX, point.mY);
}
