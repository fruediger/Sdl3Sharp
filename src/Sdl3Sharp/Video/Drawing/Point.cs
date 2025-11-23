using Sdl3Sharp.Internal;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sdl3Sharp.Video.Drawing;

public static class Point
{
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool EqualsWithinEpsilon<T>(in this Point<T> value, in Point<T> other, in T epsilon, Utilities.Math.IDispatchComparable<T>? _ = default)
		where T : unmanaged, IEquatable<T>, IFormattable, ISpanFormattable, IEqualityOperators<T, T, bool>, IComparable<T>, INumberBase<T>
		=> T.Abs(value.X - other.X).CompareTo(epsilon) is <= 0
		&& T.Abs(value.Y - other.Y).CompareTo(epsilon) is <= 0;

	[OverloadResolutionPriority(1)]
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool EqualsWithinEpsilon<T>(in this Point<T> value, in Point<T> other, in T epsilon, Utilities.Math.IDispatchComparisonOperators<T>? _ = default)
		where T : unmanaged, IEquatable<T>, IFormattable, ISpanFormattable, IEqualityOperators<T, T, bool>, IComparisonOperators<T, T, bool>, INumberBase<T>
		=> T.Abs(value.X - other.X) <= epsilon
		&& T.Abs(value.Y - other.Y) <= epsilon;
}

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

	public required readonly T X
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		get => mX;

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		init => mX = value;
	}

	public required readonly T Y
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		get => mY;

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		init => mY = value;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly void Deconstruct(out T x, out T y) { x = mX; y = mY; }

	public readonly override bool Equals([NotNullWhen(true)] object? obj) => obj is Point<T> other && Equals(other);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly bool Equals(in Point<T> other)
		=> mX.Equals(other.mX)
		&& mY.Equals(other.mY);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	readonly bool IEquatable<Point<T>>.Equals(Point<T> other) => Equals(other);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly override int GetHashCode() => HashCode.Combine(mX, mY);

	public readonly override string ToString() => ToString(format: default, formatProvider: default);

	public readonly string ToString(IFormatProvider? formatProvider) => ToString(format: default, formatProvider);

	public readonly string ToString(string? format) => ToString(format, formatProvider: default);

	public readonly string ToString(string? format, IFormatProvider? formatProvider)
		=> $"{{ {nameof(X)}: {X.ToString(format, formatProvider)}, {
			nameof(Y)}: {Y.ToString(format, formatProvider)} }}";

	public readonly bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format = default, IFormatProvider? provider = default)
	{
		charsWritten = 0;

		return SpanFormat.TryWrite($"{{ {nameof(X)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(in mX, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite($", {nameof(Y)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(in mY, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite(" }", ref destination, ref charsWritten);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator ==(in Point<T> left, in Point<T> right)
		=> left.mX == right.mX
		&& left.mY == right.mY;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static bool IEqualityOperators<Point<T>, Point<T>, bool>.operator ==(Point<T> left, Point<T> right) => left == right;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator !=(in Point<T> left, in Point<T> right)
		=> left.mX != right.mX
		|| left.mY != right.mY;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static bool IEqualityOperators<Point<T>, Point<T>, bool>.operator !=(Point<T> left, Point<T> right) => left != right;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static implicit operator Point<T>(in (T X, T Y) xy) => new(in xy.X, in xy.Y);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static implicit operator (T X, T Y)(in Point<T> point) => (point.mX, point.mY);
}
