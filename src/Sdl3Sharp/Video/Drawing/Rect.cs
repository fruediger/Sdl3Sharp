using Sdl3Sharp.Internal;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sdl3Sharp.Video.Drawing;

public static partial class Rect
{
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool ContainsPoint<T>(in this Rect<T> rect, in Point<T> point, Utilities.Math.IDispatchComparable<T>? _ = default)
		where T : unmanaged, IEquatable<T>, IFormattable, ISpanFormattable, IEqualityOperators<T, T, bool>, IComparable<T>, IAdditionOperators<T, T, T>
		=> point.X.CompareTo(rect.Left) is >= 0
		&& point.X.CompareTo(rect.Left + rect.Width) is <= 0
		&& point.Y.CompareTo(rect.Top) is >= 0
		&& point.Y.CompareTo(rect.Top + rect.Height) is <= 0;

	[OverloadResolutionPriority(1)]
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool ContainsPoint<T>(in this Rect<T> rect, in Point<T> point, Utilities.Math.IDispatchComparisonOperators<T>? _ = default)
		where T : unmanaged, IEquatable<T>, IFormattable, ISpanFormattable, IEqualityOperators<T, T, bool>, IComparisonOperators<T, T, bool>, IAdditionOperators<T, T, T>
		=> point.X >= rect.Left
		&& point.X <= rect.Left + rect.Width
		&& point.Y >= rect.Top
		&& point.Y <= rect.Top + rect.Height;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool EqualsWithinEpsilon<T>(in this Rect<T> rect, in Rect<T> other, in T epsilon, Utilities.Math.IDispatchComparable<T>? _ = default)
		where T : unmanaged, IEquatable<T>, IFormattable, ISpanFormattable, IEqualityOperators<T, T, bool>, IComparable<T>, INumberBase<T>
		=> T.Abs(rect.Left - other.Left).CompareTo(epsilon) is <= 0
		&& T.Abs(rect.Top - other.Top).CompareTo(epsilon) is <= 0
		&& T.Abs(rect.Width - other.Width).CompareTo(epsilon) is <= 0
		&& T.Abs(rect.Height - other.Height).CompareTo(epsilon) is <= 0;

	[OverloadResolutionPriority(1)]
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool EqualsWithinEpsilon<T>(in this Rect<T> rect, in Rect<T> other, in T epsilon, Utilities.Math.IDispatchComparisonOperators<T>? _ = default)
		where T : unmanaged, IEquatable<T>, IFormattable, ISpanFormattable, IEqualityOperators<T, T, bool>, IComparisonOperators<T, T, bool>, INumberBase<T>
		=> T.Abs(rect.Left - other.Left) <= epsilon
		&& T.Abs(rect.Top - other.Top) <= epsilon
		&& T.Abs(rect.Width - other.Width) <= epsilon
		&& T.Abs(rect.Height - other.Height) <= epsilon;

	public static bool HasIntersection(in this Rect<int> rect, in Rect<int> other)
	{
		unsafe
		{
			fixed (Rect<int>* rectPtr = &rect, otherPtr = &other)
			{
				return SDL_HasRectIntersection(rectPtr, otherPtr);
			}
		}
	}

	public static bool HasIntersection(in this Rect<float> rect, in Rect<float> other)
	{
		unsafe
		{
			fixed (Rect<float>* rectPtr = &rect, otherPtr = &other)
			{
				return SDL_HasRectIntersectionFloat(rectPtr, otherPtr);
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool IsEmpty<T>(in this Rect<T> rect, Utilities.Math.IDispatchComparable<T>? _ = default)
		where T : unmanaged, IEquatable<T>, IFormattable, ISpanFormattable, IEqualityOperators<T, T, bool>, IComparable<T>, INumberBase<T>
		=> rect.Width.CompareTo(T.Zero) is <= 0
		|| rect.Height.CompareTo(T.Zero) is <= 0;

	[OverloadResolutionPriority(1)]
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool IsEmpty<T>(in this Rect<T> rect, Utilities.Math.IDispatchComparisonOperators<T>? _ = default)
		where T : unmanaged, IEquatable<T>, IFormattable, ISpanFormattable, IEqualityOperators<T, T, bool>, IComparisonOperators<T, T, bool>, INumberBase<T>
		=> rect.Width <= T.Zero
		|| rect.Height <= T.Zero;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void ToRectFloat(in this Rect<int> rect, out Rect<float> result)
		=> result = new(
			(float)rect.Left,
			(float)rect.Top,
			(float)rect.Width,
			(float)rect.Height
		);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void ToRectInt(in this Rect<float> rect, out Rect<int> result)
		=> result = new(
			(int)rect.Left,
			(int)rect.Top,
			(int)rect.Width,
			(int)rect.Height
		);

	public static bool TryGetFromInnerPoints(ReadOnlySpan<Point<int>> points, out Rect<int> result)
	{
		unsafe
		{
			fixed (Point<int>* pointsPtr = points)
			fixed (Rect<int>* resultPtr = &result)
			{
				return SDL_GetRectEnclosingPoints(pointsPtr, points.Length, clip: null, resultPtr);
			}
		}
	}

	public static bool TryGetFromInnerPoints(ReadOnlySpan<Point<int>> points, in Rect<int> clip, out Rect<int> result)
	{
		unsafe
		{
			fixed (Point<int>* pointsPtr = points)
			fixed (Rect<int>* clipPtr = &clip, resultPtr = &result)
			{
				return SDL_GetRectEnclosingPoints(pointsPtr, points.Length, clipPtr, resultPtr);
			}
		}
	}

	public static bool TryGetFromInnerPoints(ReadOnlySpan<Point<float>> points, out Rect<float> result)
	{
		unsafe
		{
			fixed (Point<float>* pointsPtr = points)
			fixed (Rect<float>* resultPtr = &result)
			{
				return SDL_GetRectEnclosingPointsFloat(pointsPtr, points.Length, clip: null, resultPtr);
			}
		}
	}

	public static bool TryGetFromInnerPoints(ReadOnlySpan<Point<float>> points, in Rect<float> clip, out Rect<float> result)
	{
		unsafe
		{
			fixed (Point<float>* pointsPtr = points)
			fixed (Rect<float>* clipPtr = &clip, resultPtr = &result)
			{
				return SDL_GetRectEnclosingPointsFloat(pointsPtr, points.Length, clipPtr, resultPtr);
			}
		}
	}

	public static bool TryGetLineIntersection(in this Rect<int> rect, ref int x1, ref int y1, ref int x2, ref int y2)
	{
		unsafe
		{
			fixed (Rect<int>* rectPtr = &rect)
			fixed (int* x1Ptr = &x1, y1Ptr = &y1, x2Ptr = &x2, y2Ptr = &y2)
			{
				return SDL_GetRectAndLineIntersection(rectPtr, x1Ptr, y1Ptr, x2Ptr, y2Ptr);
			}
		}
	}

	public static bool TryGetLineIntersection(in this Rect<int> rect, ref Point<int> lineStart, ref Point<int> lineEnd)
	{
		unsafe
		{
			fixed (Rect<int>* rectPtr = &rect)
			fixed (int* x1Ptr = &Unsafe.As<Point<int>, (int X, int Y)>(ref lineStart).X, y1Ptr = &Unsafe.As<Point<int>, (int X, int Y)>(ref lineStart).Y, x2Ptr = &Unsafe.As<Point<int>, (int X, int Y)>(ref lineEnd).X, y2Ptr = &Unsafe.As<Point<int>, (int X, int Y)>(ref lineEnd).X)
			{
				return SDL_GetRectAndLineIntersection(rectPtr, x1Ptr, y1Ptr, x2Ptr, y2Ptr);
			}
		}
	}

	public static bool TryGetLineIntersection(in this Rect<float> rect, ref float x1, ref float y1, ref float x2, ref float y2)
	{
		unsafe
		{
			fixed (Rect<float>* rectPtr = &rect)
			fixed (float* x1Ptr = &x1, y1Ptr = &y1, x2Ptr = &x2, y2Ptr = &y2)
			{
				return SDL_GetRectAndLineIntersectionFloat(rectPtr, x1Ptr, y1Ptr, x2Ptr, y2Ptr);
			}
		}
	}

	public static bool TryGetLineIntersection(in this Rect<float> rect, ref Point<float> lineStart, ref Point<float> lineEnd)
	{
		unsafe
		{
			fixed (Rect<float>* rectPtr = &rect)
			fixed (float* x1Ptr = &Unsafe.As<Point<float>, (float X, float Y)>(ref lineStart).X, y1Ptr = &Unsafe.As<Point<float>, (float X, float Y)>(ref lineStart).Y, x2Ptr = &Unsafe.As<Point<float>, (float X, float Y)>(ref lineEnd).X, y2Ptr = &Unsafe.As<Point<float>, (float X, float Y)>(ref lineEnd).X)
			{
				return SDL_GetRectAndLineIntersectionFloat(rectPtr, x1Ptr, y1Ptr, x2Ptr, y2Ptr);
			}
		}
	}

	public static bool TryGetIntersection(in this Rect<int> rect, in Rect<int> other, out Rect<int> result)
	{
		unsafe
		{
			fixed (Rect<int>* rectPtr = &rect, otherPtr = &other, resultPtr = &result)
			{
				return SDL_GetRectIntersection(rectPtr, otherPtr, resultPtr);
			}
		}
	}

	public static bool TryGetIntersection(in this Rect<float> rect, in Rect<float> other, out Rect<float> result)
	{
		unsafe
		{
			fixed (Rect<float>* rectPtr = &rect, otherPtr = &other, resultPtr = &result)
			{
				return SDL_GetRectIntersectionFloat(rectPtr, otherPtr, resultPtr);
			}
		}
	}

	public static bool TryGetUnion(in this Rect<int> rect, in Rect<int> other, out Rect<int> result)
	{
		unsafe
		{
			fixed (Rect<int>* rectPtr = &rect, otherPtr = &other, resultPtr = &result)
			{
				return SDL_GetRectUnion(rectPtr, otherPtr, resultPtr);
			}
		}
	}

	public static bool TryGetUnion(in this Rect<float> rect, in Rect<float> other, out Rect<float> result)
	{
		unsafe
		{
			fixed (Rect<float>* rectPtr = &rect, otherPtr = &other, resultPtr = &result)
			{
				return SDL_GetRectUnionFloat(rectPtr, otherPtr, resultPtr);
			}
		}
	}
}

[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
[StructLayout(LayoutKind.Sequential)]
[method: MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization), SetsRequiredMembers]
public readonly struct Rect<T>(in T left, in T top, in T width, in T height) :
	IEquatable<Rect<T>>, IFormattable, ISpanFormattable, IEqualityOperators<Rect<T>, Rect<T>, bool>
	where T : 
		unmanaged, IEquatable<T>, IFormattable, ISpanFormattable, IEqualityOperators<T, T, bool>
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly string DebuggerDisplay => ToString(formatProvider: CultureInfo.InvariantCulture);

	private readonly T mX = left, mY = top, mW = width, mH = height;

	public required readonly T Left
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		get => mX;

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		init => mX = value;
	}

	public required readonly T Top
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		get => mY;

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		init => mY = value;
	}

	public required readonly T Width
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		get => mW;

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		init => mW = value;
	}

	public required readonly T Height
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		get => mH;

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		init => mH = value;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly void Deconstruct(out T left, out T top, out T width, out T height) { left = mX; top = mY; width = mW; height = mH; }

	public readonly override bool Equals([NotNullWhen(true)] object? obj) => obj is Rect<T> other && Equals(other);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly bool Equals(in Rect<T> other)
		=> mX.Equals(other.mX)
		&& mY.Equals(other.mY)
		&& mW.Equals(other.mW)
		&& mH.Equals(other.mH);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	readonly bool IEquatable<Rect<T>>.Equals(Rect<T> other) => Equals(other);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly override int GetHashCode() => HashCode.Combine(mX, mY, mW, mH);

	public readonly override string ToString() => ToString(format: default, formatProvider: default);

	public readonly string ToString(IFormatProvider? formatProvider) => ToString(format: default, formatProvider);

	public readonly string ToString(string? format) => ToString(format, formatProvider: default);

	public readonly string ToString(string? format, IFormatProvider? formatProvider)
		=> $"{{ {nameof(Left)}: {mX.ToString(format, formatProvider)}, {
			nameof(Top)}: {mY.ToString(format, formatProvider)}, {
			nameof(Width)}: {mW.ToString(format, formatProvider)}, {
			nameof(Height)}: {mH.ToString(format, formatProvider)} }}";

	public readonly bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format = default, IFormatProvider? provider = default)
	{
		charsWritten = 0;

		return SpanFormat.TryWrite($"{{ {nameof(Left)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(mX, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite($", {nameof(Top)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(mY, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite($", {nameof(Width)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(mW, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite($", {nameof(Height)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(mH, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite(" }", ref destination, ref charsWritten);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator ==(in Rect<T> left, in Rect<T> right)
		=> left.mX == right.mX
		&& left.mY == right.mY
		&& left.mW == right.mW
		&& left.mH == right.mH;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static bool IEqualityOperators<Rect<T>, Rect<T>, bool>.operator ==(Rect<T> left, Rect<T> right) => left == right;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator !=(in Rect<T> left, in Rect<T> right)
		=> left.mX != right.mX
		|| left.mY != right.mY
		|| left.mW != right.mW
		|| left.mH != right.mH;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static bool IEqualityOperators<Rect<T>, Rect<T>, bool>.operator !=(Rect<T> left, Rect<T> right) => left != right;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static implicit operator Rect<T>(in (T Left, T Top, T Width, T Height) ltwh) => new(in ltwh.Left, in ltwh.Top, in ltwh.Width, in ltwh.Height);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static implicit operator (T Left, T Top, T Width, T Height)(in Rect<T> rect) => (rect.mX, rect.mY, rect.mW, rect.mH);
}
