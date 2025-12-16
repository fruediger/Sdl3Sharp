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
/// Provides extension methods for <see cref="Rect{T}"/>
/// </summary>
public static partial class Rect
{
	/// <summary>
	/// Indicates whether the specified point is contained within the rectangle
	/// </summary>
	/// <typeparam name="T">The type of the rectangle's coordinates and dimensions</typeparam>
	/// <param name="rect">The rectangle to check</param>
	/// <param name="point">The point to check</param>
	/// <param name="_"><em>Please ignore and do not explicitly set this parameter</em></param>
	/// <returns><c><see langword="true"/></c>, if the point is contained within the rectangle; otherwise, <c><see langword="false"/></c></returns>
	/// <remarks>
	/// <para>
	/// A <paramref name="point"/> is considered part of a <paramref name="rect"/>angle if the <paramref name="point"/>'s <see cref="Point{T}.X">X</see> and <see cref="Point{T}.Y">Y</see> coordinates are ≥ to the <paramref name="rect"/>angle's <see cref="Rect{T}.Top">Top</see>-<see cref="Rect{T}.Left">Left</see> corner,
	/// and &lt; to the <paramref name="rect"/>angle's <see cref="Rect{T}.Left">Left</see> + <see cref="Rect{T}.Width">Width</see> and <see cref="Rect{T}.Top">Top</see> + <see cref="Rect{T}.Height">Height</see> coordinates.
	/// So a 1⨯1 rectangle with it's origin at (0, 0) would consider a point (0, 0) as "inside" and a point (0, 1) as not.
	/// </para>
	/// <para>
	/// This method works for types of <paramref name="rect"/> and <paramref name="point"/> <typeparamref name="T"/> which implement <see cref="IComparable{T}"/>.
	/// If <typeparamref name="T"/> implements both <see cref="IComparable{T}"/> and <see cref="IComparisonOperators{TSelf, TOther, TResult}"/>,
	/// overload resolution prioritizes <see cref="ContainsPoint{T}(in Rect{T}, in Point{T}, Utilities.Math.IDispatchComparisonOperators{T}?)"/> instead.
	/// </para>
	/// <para>
	/// <em>Please ignore and do not explicitly set the <paramref name="_"/> parameter</em>. This parameter is just used in the method's signature to dispatch calls to this method for types of <paramref name="rect"/> and <paramref name="point"/> <typeparamref name="T"/> which implement <see cref="IComparable{T}"/>.
	/// </para>
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool ContainsPoint<T>(in this Rect<T> rect, in Point<T> point, Utilities.Math.IDispatchComparable<T>? _ = default)
		where T : unmanaged, IEquatable<T>, IFormattable, ISpanFormattable, IEqualityOperators<T, T, bool>, IComparable<T>, IAdditionOperators<T, T, T>
		=> point.X.CompareTo(rect.Left) is >= 0
		&& point.X.CompareTo(rect.Left + rect.Width) is < 0
		&& point.Y.CompareTo(rect.Top) is >= 0
		&& point.Y.CompareTo(rect.Top + rect.Height) is < 0;

	/// <summary>
	/// Indicates whether the specified point is contained within the rectangle
	/// </summary>
	/// <typeparam name="T">The type of the rectangle's coordinates and dimensions</typeparam>
	/// <param name="rect">The rectangle to check</param>
	/// <param name="point">The point to check</param>
	/// <param name="_"><em>Please ignore and do not explicitly set this parameter</em></param>
	/// <returns><c><see langword="true"/></c>, if the point is contained within the rectangle; otherwise, <c><see langword="false"/></c></returns>
	/// <remarks>
	/// <para>
	/// A <paramref name="point"/> is considered part of a <paramref name="rect"/>angle if the <paramref name="point"/>'s <see cref="Point{T}.X">X</see> and <see cref="Point{T}.Y">Y</see> coordinates are ≥ to the <paramref name="rect"/>angle's <see cref="Rect{T}.Top">Top</see>-<see cref="Rect{T}.Left">Left</see> corner,
	/// and &lt; to the <paramref name="rect"/>angle's <see cref="Rect{T}.Left">Left</see> + <see cref="Rect{T}.Width">Width</see> and <see cref="Rect{T}.Top">Top</see> + <see cref="Rect{T}.Height">Height</see> coordinates.
	/// So a 1⨯1 rectangle with it's origin at (0, 0) would consider a point (0, 0) as "inside" and a point (0, 1) as not.
	/// </para>
	/// <para>
	/// This method works for types of <paramref name="rect"/> and <paramref name="point"/> <typeparamref name="T"/> which implement <see cref="IComparisonOperators{TSelf, TOther, TResult}"/>.
	/// If <typeparamref name="T"/> implements both <see cref="IComparisonOperators{TSelf, TOther, TResult}"/> and <see cref="IComparable{T}"/>,
	/// overload resolution prioritizes this method.
	/// </para>
	/// <para>
	/// <em>Please ignore and do not explicitly set the <paramref name="_"/> parameter</em>. This parameter is just used in the method's signature to dispatch calls to this method for types of <paramref name="rect"/> and <paramref name="point"/> <typeparamref name="T"/> which implement <see cref="IComparisonOperators{TSelf, TOther, TResult}"/>.
	/// </para>
	/// </remarks>
	[OverloadResolutionPriority(1)]
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool ContainsPoint<T>(in this Rect<T> rect, in Point<T> point, Utilities.Math.IDispatchComparisonOperators<T>? _ = default)
		where T : unmanaged, IEquatable<T>, IFormattable, ISpanFormattable, IEqualityOperators<T, T, bool>, IComparisonOperators<T, T, bool>, IAdditionOperators<T, T, T>
		=> point.X >= rect.Left
		&& point.X <= rect.Left + rect.Width
		&& point.Y >= rect.Top
		&& point.Y <= rect.Top + rect.Height;

	/// <summary>
	/// Indicates whether two rectangles are equal within a specified epsilon
	/// </summary>
	/// <typeparam name="T">The type of the rectangle's coordinates and dimensions</typeparam>
	/// <param name="rect">The first rectangle</param>
	/// <param name="other">The second rectangle</param>
	/// <param name="epsilon">The tolerance for equality</param>
	/// <param name="_"><em>Please ignore and do not explicitly set this parameter</em></param>
	/// <returns><c><see langword="true"/></c>, if the rectangles are considered equal within the specified epsilon; otherwise, <c><see langword="false"/></c></returns>
	/// <remarks>
	/// <para>
	/// Two rectangles are considered equal if their <see cref="Rect{T}.Left">Left</see> and <see cref="Rect{T}.Top">Top</see> coordinates, and their <see cref="Rect{T}.Width">Width</see> and <see cref="Rect{T}.Height">Height dimensions</see> are within <paramref name="epsilon"/> of each other.
	/// </para>
	/// <para>
	/// If the type <typeparamref name="T"/> is <see cref="float"/>, there's the <see cref="EqualsWithinEpsilon(in Rect{float}, in Rect{float}, in float)"/> overload which defaults <paramref name="epsilon"/> to <see cref="Utilities.Math.EpsilonF"/>.
	/// </para>
	/// <para>
	/// This method works for types of <paramref name="rect"/> and <paramref name="other"/> <typeparamref name="T"/> which implement <see cref="IComparable{T}"/>.
	/// If <typeparamref name="T"/> implements both <see cref="IComparable{T}"/> and <see cref="IComparisonOperators{TSelf, TOther, TResult}"/>,
	/// overload resolution prioritizes <see cref="EqualsWithinEpsilon{T}(in Rect{T}, in Rect{T}, in T, Utilities.Math.IDispatchComparisonOperators{T}?)"/> instead.
	/// </para>
	/// <para>
	/// <em>Please ignore and do not explicitly set the <paramref name="_"/> parameter</em>. This parameter is just used in the method's signature to dispatch calls to this method for types of <paramref name="rect"/> and <paramref name="other"/> <typeparamref name="T"/> which implement <see cref="IComparable{T}"/>.
	/// </para>
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool EqualsWithinEpsilon<T>(in this Rect<T> rect, in Rect<T> other, in T epsilon, Utilities.Math.IDispatchComparable<T>? _ = default)
		where T : unmanaged, IEquatable<T>, IFormattable, ISpanFormattable, IEqualityOperators<T, T, bool>, IComparable<T>, INumberBase<T>
		=> T.Abs(rect.Left - other.Left).CompareTo(epsilon) is <= 0
		&& T.Abs(rect.Top - other.Top).CompareTo(epsilon) is <= 0
		&& T.Abs(rect.Width - other.Width).CompareTo(epsilon) is <= 0
		&& T.Abs(rect.Height - other.Height).CompareTo(epsilon) is <= 0;

	/// <summary>
	/// Indicates whether two rectangles are equal within a specified epsilon
	/// </summary>
	/// <typeparam name="T">The type of the rectangle's coordinates and dimensions</typeparam>
	/// <param name="rect">The first rectangle</param>
	/// <param name="other">The second rectangle</param>
	/// <param name="epsilon">The tolerance for equality</param>
	/// <param name="_"><em>Please ignore and do not explicitly set this parameter</em></param>
	/// <returns><c><see langword="true"/></c>, if the rectangles are considered equal within the specified epsilon; otherwise, <c><see langword="false"/></c></returns>
	/// <remarks>
	/// <para>
	/// Two rectangles are considered equal if their <see cref="Rect{T}.Left">Left</see> and <see cref="Rect{T}.Top">Top</see> coordinates, and their <see cref="Rect{T}.Width">Width</see> and <see cref="Rect{T}.Height">Height dimensions</see> are within <paramref name="epsilon"/> of each other.
	/// </para>
	/// <para>
	/// If the type <typeparamref name="T"/> is <see cref="float"/>, there's the <see cref="EqualsWithinEpsilon(in Rect{float}, in Rect{float}, in float)"/> overload which defaults <paramref name="epsilon"/> to <see cref="Utilities.Math.EpsilonF"/>.
	/// </para>
	/// <para>
	/// This method works for types of <paramref name="rect"/> and <paramref name="other"/> <typeparamref name="T"/> which implement <see cref="IComparisonOperators{TSelf, TOther, TResult}"/>.
	/// If <typeparamref name="T"/> implements both <see cref="IComparisonOperators{TSelf, TOther, TResult}"/> and <see cref="IComparable{T}"/>,
	/// overload resolution prioritizes this method.
	/// </para>
	/// <para>
	/// <em>Please ignore and do not explicitly set the <paramref name="_"/> parameter</em>. This parameter is just used in the method's signature to dispatch calls to this method for types of <paramref name="rect"/> and <paramref name="other"/> <typeparamref name="T"/> which implement <see cref="IComparisonOperators{TSelf, TOther, TResult}"/>.
	/// </para>
	/// </remarks>
	[OverloadResolutionPriority(1)]
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool EqualsWithinEpsilon<T>(in this Rect<T> rect, in Rect<T> other, in T epsilon, Utilities.Math.IDispatchComparisonOperators<T>? _ = default)
		where T : unmanaged, IEquatable<T>, IFormattable, ISpanFormattable, IEqualityOperators<T, T, bool>, IComparisonOperators<T, T, bool>, INumberBase<T>
		=> T.Abs(rect.Left - other.Left) <= epsilon
		&& T.Abs(rect.Top - other.Top) <= epsilon
		&& T.Abs(rect.Width - other.Width) <= epsilon
		&& T.Abs(rect.Height - other.Height) <= epsilon;

	/// <summary>
	/// Indicates whether two rectangles are equal within a specified epsilon
	/// </summary>
	/// <param name="rect">The first rectangle</param>
	/// <param name="other">The second rectangle</param>
	/// <param name="epsilon">The tolerance for equality. Defaults to <see cref="Utilities.Math.EpsilonF"/>.</param>
	/// <returns><c><see langword="true"/></c>, if the rectangles are considered equal within the specified epsilon; otherwise, <c><see langword="false"/></c></returns>
	/// <remarks>
	/// <para>
	/// Two rectangles are considered equal if their <see cref="Rect{T}.Left">Left</see> and <see cref="Rect{T}.Top">Top</see> coordinates, and their <see cref="Rect{T}.Width">Width</see> and <see cref="Rect{T}.Height">Height dimensions</see> are within <paramref name="epsilon"/> of each other.
	/// </para>
	/// <para>
	/// The parameter <paramref name="epsilon"/> defaults to <see cref="Utilities.Math.EpsilonF"/>.
	/// This is often a reasonable way to compare two floating point rectangles and deal with the slight precision variations in floating point calculations that tend to pop up.
	/// </para>
	/// </remarks>
	[OverloadResolutionPriority(2)]
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool EqualsWithinEpsilon(in this Rect<float> rect, in Rect<float> other, in float epsilon = Utilities.Math.EpsilonF)
		=> float.Abs(rect.Left - other.Left) <= epsilon
		&& float.Abs(rect.Top - other.Top) <= epsilon
		&& float.Abs(rect.Width - other.Width) <= epsilon
		&& float.Abs(rect.Height - other.Height) <= epsilon;

	/// <summary>
	/// Determines whether two rectangles intersect
	/// </summary>
	/// <param name="rect">The first rectangle</param>
	/// <param name="other">The second rectangle</param>
	/// <returns><c><see langword="true"/></c>, if the rectangles intersect; otherwise, <c><see langword="false"/></c></returns>
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

	/// <summary>
	/// Determines whether two rectangles intersect
	/// </summary>
	/// <param name="rect">The first rectangle</param>
	/// <param name="other">The second rectangle</param>
	/// <returns><c><see langword="true"/></c>, if the rectangles intersect; otherwise, <c><see langword="false"/></c></returns>
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

	/// <summary>
	/// Determines whether a rectangle has no area
	/// </summary>
	/// <typeparam name="T">The type of the rectangle's coordinates and dimensions</typeparam>
	/// <param name="rect">The rectangle to check</param>
	/// <param name="_"><em>Please ignore and do not explicitly set this parameter</em></param>
	/// <returns><c><see langword="true"/></c>, if the rectangle is considered empty; otherwise, <c><see langword="false"/></c></returns>
	/// <remarks>
	/// <para>
	/// A rectangles is considered empty if it's <see cref="Rect{T}.Width">Width</see> and/or it's <see cref="Rect{T}.Height">Height</see> are ≤ <c>0</c>.
	/// </para>
	/// <para>
	/// This method works for types of <paramref name="rect"/> <typeparamref name="T"/> which implement <see cref="IComparable{T}"/>.
	/// If <typeparamref name="T"/> implements both <see cref="IComparable{T}"/> and <see cref="IComparisonOperators{TSelf, TOther, TResult}"/>,
	/// overload resolution prioritizes <see cref="IsEmpty{T}(in Rect{T}, Utilities.Math.IDispatchComparisonOperators{T}?)"/> instead.
	/// </para>
	/// <para>
	/// <em>Please ignore and do not explicitly set the <paramref name="_"/> parameter</em>. This parameter is just used in the method's signature to dispatch calls to this method for types of <paramref name="rect"/> <typeparamref name="T"/> which implement <see cref="IComparable{T}"/>.
	/// </para>
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool IsEmpty<T>(in this Rect<T> rect, Utilities.Math.IDispatchComparable<T>? _ = default)
		where T : unmanaged, IEquatable<T>, IFormattable, ISpanFormattable, IEqualityOperators<T, T, bool>, IComparable<T>, INumberBase<T>
		=> rect.Width.CompareTo(T.Zero) is <= 0
		|| rect.Height.CompareTo(T.Zero) is <= 0;

	/// <summary>
	/// Determines whether a rectangle has no area
	/// </summary>
	/// <typeparam name="T">The type of the rectangle's coordinates and dimensions</typeparam>
	/// <param name="rect">The rectangle to check</param>
	/// <param name="_"><em>Please ignore and do not explicitly set this parameter</em></param>
	/// <returns><c><see langword="true"/></c>, if the rectangle is considered empty; otherwise, <c><see langword="false"/></c></returns>
	/// <remarks>
	/// <para>
	/// A rectangles is considered empty if it's <see cref="Rect{T}.Width">Width</see> and/or it's <see cref="Rect{T}.Height">Height</see> are ≤ <c>0</c>.
	/// </para>
	/// <para>
	/// This method works for types of <paramref name="rect"/> <typeparamref name="T"/> which implement <see cref="IComparisonOperators{TSelf, TOther, TResult}"/>.
	/// If <typeparamref name="T"/> implements both <see cref="IComparisonOperators{TSelf, TOther, TResult}"/> and <see cref="IComparable{T}"/>,
	/// overload resolution prioritizes this method.
	/// </para>
	/// <para>
	/// <em>Please ignore and do not explicitly set the <paramref name="_"/> parameter</em>. This parameter is just used in the method's signature to dispatch calls to this method for types of <paramref name="rect"/> <typeparamref name="T"/> which implement <see cref="IComparisonOperators{TSelf, TOther, TResult}"/>.
	/// </para>
	/// </remarks>
	[OverloadResolutionPriority(1)]
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool IsEmpty<T>(in this Rect<T> rect, Utilities.Math.IDispatchComparisonOperators<T>? _ = default)
		where T : unmanaged, IEquatable<T>, IFormattable, ISpanFormattable, IEqualityOperators<T, T, bool>, IComparisonOperators<T, T, bool>, INumberBase<T>
		=> rect.Width <= T.Zero
		|| rect.Height <= T.Zero;

	/// <summary>
	/// Converts a rectangle with integer coordinates and dimensions to a rectangle with floating point coordinates and dimensions
	/// </summary>
	/// <param name="rect">The rectangle to convert</param>
	/// <param name="result">The resulting rectangle with floating point coordinates and dimensions</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void ToFloatingPoint(in this Rect<int> rect, out Rect<float> result)
		=> result = new(
			unchecked((float)rect.Left),
			unchecked((float)rect.Top),
			unchecked((float)rect.Width),
			unchecked((float)rect.Height)
		);

	/// <summary>
	/// Converts a rectangle with floating point coordinates and dimensions to a rectangle with integer coordinates and dimensions
	/// </summary>
	/// <param name="rect">The rectangle to convert</param>
	/// <param name="result">The resulting rectangle with integer coordinates and dimensions</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void ToInteger(in this Rect<float> rect, out Rect<int> result)
		=> result = new(
			unchecked((int)rect.Left),
			unchecked((int)rect.Top),
			unchecked((int)rect.Width),
			unchecked((int)rect.Height)
		);

	/// <summary>
	/// Creates the smallest rectangle that encloses all the specified points
	/// </summary>
	/// <param name="points">The points to enclose</param>
	/// <param name="result">The resulting rectangle, if this method returns <c><see langword="true"/></c></param>
	/// <returns><c><see langword="true"/></c>, if the rectangle was created successfully; otherwise, <c><see langword="false"/></c></returns>
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

	/// <summary>
	/// Creates the smallest rectangle that encloses all the specified points within the specified clipping rectangle
	/// </summary>
	/// <param name="points">The points to enclose</param>
	/// <param name="clip">The clipping rectangle</param>
	/// <param name="result">The resulting rectangle, if this method returns <c><see langword="true"/></c></param>
	/// <returns><c><see langword="true"/></c>, if the rectangle was created successfully; otherwise, <c><see langword="false"/></c></returns>
	/// <remarks>
	/// <para>
	/// Only points that are within the specified <paramref name="clip"/> rectangle are considered when creating the enclosing rectangle.
	/// </para>
	/// </remarks>
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

	/// <summary>
	/// Creates the smallest rectangle that encloses all the specified points
	/// </summary>
	/// <param name="points">The points to enclose</param>
	/// <param name="result">The resulting rectangle, if this method returns <c><see langword="true"/></c></param>
	/// <returns><c><see langword="true"/></c>, if the rectangle was created successfully; otherwise, <c><see langword="false"/></c></returns>
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

	/// <summary>
	/// Creates the smallest rectangle that encloses all the specified points within the specified clipping rectangle
	/// </summary>
	/// <param name="points">The points to enclose</param>
	/// <param name="clip">The clipping rectangle</param>
	/// <param name="result">The resulting rectangle, if this method returns <c><see langword="true"/></c></param>
	/// <returns><c><see langword="true"/></c>, if the rectangle was created successfully; otherwise, <c><see langword="false"/></c></returns>
	/// <remarks>
	/// <para>
	/// Only points that are within the specified <paramref name="clip"/> rectangle are considered when creating the enclosing rectangle.
	/// </para>
	/// </remarks>
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

	/// <summary>
	/// Determines the intersecting segment of a line and a rectangle, if any
	/// </summary>
	/// <param name="rect">The rectangle to intersect with</param>
	/// <param name="x1">The x-coordinate of the start point of the line. If this method returns <c><see langword="true"/></c>, this value will be updated to the x-coordinate of the nearer intersection point or remain unchanged if the start point is contained within the rectangle.</param>
	/// <param name="y1">The y-coordinate of the start point of the line. If this method returns <c><see langword="true"/></c>, this value will be updated to the y-coordinate of the nearer intersection point or remain unchanged if the start point is contained within the rectangle.</param>
	/// <param name="x2">The x-coordinate of the end point of the line. If this method returns <c><see langword="true"/></c>, this value will be updated to the x-coordinate of the farther intersection point or remain unchanged if the end point is contained within the rectangle.</param>
	/// <param name="y2">The y-coordinate of the end point of the line. If this method returns <c><see langword="true"/></c>, this value will be updated to the y-coordinate of the farther intersection point or remain unchanged if the end point is contained within the rectangle.</param>
	/// <returns><c><see langword="true"/></c>, if the line intersects with the rectangle; otherwise, <c><see langword="false"/></c></returns>
	/// <remarks>
	/// <para>
	/// This method is used to clip a line segment to a rectangle.
	/// A line segment contained entirely within the rectangle or that does not intersect will remain unchanged.
	/// A line segment that crosses the rectangle at either or both ends will be clipped to the boundary of the rectangle and the new coordinates stored in <paramref name="x1"/>, <paramref name="y1"/>, <paramref name="x2"/>, and/or <paramref name="y2"/> as necessary.
	/// </para>
	/// </remarks>
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

	/// <summary>
	/// Determines the intersecting segment of a line and a rectangle, if any
	/// </summary>
	/// <param name="rect">The rectangle to intersect with</param>
	/// <param name="lineStart">The start point of the line. If this method returns <c><see langword="true"/></c>, this value will be updated to the start point of the intersection segment or remain unchanged if the point is contained within the rectangle.</param>
	/// <param name="lineEnd">The end point of the line. If this method returns <c><see langword="true"/></c>, this value will be updated to the end point of the intersection segment or remain unchanged if the point is contained within the rectangle.</param>
	/// <returns><c><see langword="true"/></c>, if the line intersects with the rectangle; otherwise, <c><see langword="false"/></c></returns>
	/// <remarks>
	/// <para>
	/// This method is used to clip a line segment to a rectangle.
	/// A line segment contained entirely within the rectangle or that does not intersect will remain unchanged.
	/// A line segment that crosses the rectangle at either or both ends will be clipped to the boundary of the rectangle and the new coordinates stored in <paramref name="lineStart"/> and/or <paramref name="lineEnd"/> as necessary.
	/// </para>
	/// </remarks>
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

	/// <summary>
	/// Determines the intersecting segment of a line and a rectangle, if any
	/// </summary>
	/// <param name="rect">The rectangle to intersect with</param>
	/// <param name="x1">The x-coordinate of the start point of the line. If this method returns <c><see langword="true"/></c>, this value will be updated to the x-coordinate of the nearer intersection point or remain unchanged if the start point is contained within the rectangle.</param>
	/// <param name="y1">The y-coordinate of the start point of the line. If this method returns <c><see langword="true"/></c>, this value will be updated to the y-coordinate of the nearer intersection point or remain unchanged if the start point is contained within the rectangle.</param>
	/// <param name="x2">The x-coordinate of the end point of the line. If this method returns <c><see langword="true"/></c>, this value will be updated to the x-coordinate of the farther intersection point or remain unchanged if the end point is contained within the rectangle.</param>
	/// <param name="y2">The y-coordinate of the end point of the line. If this method returns <c><see langword="true"/></c>, this value will be updated to the y-coordinate of the farther intersection point or remain unchanged if the end point is contained within the rectangle.</param>
	/// <returns><c><see langword="true"/></c>, if the line intersects with the rectangle; otherwise, <c><see langword="false"/></c></returns>
	/// <remarks>
	/// <para>
	/// This method is used to clip a line segment to a rectangle.
	/// A line segment contained entirely within the rectangle or that does not intersect will remain unchanged.
	/// A line segment that crosses the rectangle at either or both ends will be clipped to the boundary of the rectangle and the new coordinates stored in <paramref name="x1"/>, <paramref name="y1"/>, <paramref name="x2"/>, and/or <paramref name="y2"/> as necessary.
	/// </para>
	/// </remarks>
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

	/// <summary>
	/// Determines the intersecting segment of a line and a rectangle, if any
	/// </summary>
	/// <param name="rect">The rectangle to intersect with</param>
	/// <param name="lineStart">The start point of the line. If this method returns <c><see langword="true"/></c>, this value will be updated to the start point of the intersection segment or remain unchanged if the point is contained within the rectangle.</param>
	/// <param name="lineEnd">The end point of the line. If this method returns <c><see langword="true"/></c>, this value will be updated to the end point of the intersection segment or remain unchanged if the point is contained within the rectangle.</param>
	/// <returns><c><see langword="true"/></c>, if the line intersects with the rectangle; otherwise, <c><see langword="false"/></c></returns>
	/// <remarks>
	/// <para>
	/// This method is used to clip a line segment to a rectangle.
	/// A line segment contained entirely within the rectangle or that does not intersect will remain unchanged.
	/// A line segment that crosses the rectangle at either or both ends will be clipped to the boundary of the rectangle and the new coordinates stored in <paramref name="lineStart"/> and/or <paramref name="lineEnd"/> as necessary.
	/// </para>
	/// </remarks>
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

	/// <summary>
	/// Determines the intersection of two rectangles, if any
	/// </summary>
	/// <param name="rect">The first rectangle</param>
	/// <param name="other">The second rectangle</param>
	/// <param name="result">The resulting intersection rectangle, if this method returns <c><see langword="true"/></c></param>
	/// <returns><c><see langword="true"/></c>, if the rectangles intersect; otherwise, <c><see langword="false"/></c></returns>
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

	/// <summary>
	/// Determines the intersection of two rectangles, if any
	/// </summary>
	/// <param name="rect">The first rectangle</param>
	/// <param name="other">The second rectangle</param>
	/// <param name="result">The resulting intersection rectangle, if this method returns <c><see langword="true"/></c></param>
	/// <returns><c><see langword="true"/></c>, if the rectangles intersect; otherwise, <c><see langword="false"/></c></returns>
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

	/// <summary>
	/// Determines the union of two rectangles
	/// </summary>
	/// <param name="rect">The first rectangle</param>
	/// <param name="other">The second rectangle</param>
	/// <param name="result">The resulting union rectangle, if this method returns <c><see langword="true"/></c></param>
	/// <returns><c><see langword="true"/></c>, if the rectangles intersect; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
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

	/// <summary>
	/// Determines the union of two rectangles
	/// </summary>
	/// <param name="rect">The first rectangle</param>
	/// <param name="other">The second rectangle</param>
	/// <param name="result">The resulting union rectangle, if this method returns <c><see langword="true"/></c></param>
	/// <returns><c><see langword="true"/></c>, if the rectangles intersect; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
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

/// <summary>
/// Represents a rectangle defined by a position (Left, Top) in a two-dimensional coordinate system and sizes (Width, Height)
/// </summary>
/// <typeparam name="T">The type of the rectangle's coordinates and dimensions</typeparam>
/// <param name="left">The left position of the rectangle</param>
/// <param name="top">The top position of the rectangle</param>
/// <param name="width">The width of the rectangle</param>
/// <param name="height">The height of the rectangle</param>
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

	/// <summary>
	/// Gets or initializes the left position of the rectangle
	/// </summary>
	/// <value>
	/// The left position of the rectangle
	/// </value>
	public required readonly T Left
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		get => mX;

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		init => mX = value;
	}

	/// <summary>
	/// Gets or initializes the top position of the rectangle
	/// </summary>
	/// <value>
	/// The top position of the rectangle
	/// </value>
	public required readonly T Top
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		get => mY;

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		init => mY = value;
	}

	/// <summary>
	/// Gets or initializes the width of the rectangle
	/// </summary>
	/// <value>
	/// The width of the rectangle
	/// </value>
	public required readonly T Width
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		get => mW;

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		init => mW = value;
	}

	/// <summary>
	/// Gets or initializes the height of the rectangle
	/// </summary>
	/// <value>
	/// The height of the rectangle
	/// </value>
	public required readonly T Height
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		get => mH;

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		init => mH = value;
	}

	/// <summary>
	/// Deconstructs the rectangle into its Left, Top, Width and Height components
	/// </summary>
	/// <param name="left">The left position of the rectangle</param>
	/// <param name="top">The top position of the rectangle</param>
	/// <param name="width">The width of the rectangle</param>
	/// <param name="height">The height of the rectangle</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly void Deconstruct(out T left, out T top, out T width, out T height) { left = mX; top = mY; width = mW; height = mH; }

	/// <inheritdoc/>
	public readonly override bool Equals([NotNullWhen(true)] object? obj) => obj is Rect<T> other && Equals(other);

	/// <inheritdoc cref="IEquatable{T}.Equals(T)"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly bool Equals(in Rect<T> other)
		=> mX.Equals(other.mX)
		&& mY.Equals(other.mY)
		&& mW.Equals(other.mW)
		&& mH.Equals(other.mH);

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	readonly bool IEquatable<Rect<T>>.Equals(Rect<T> other) => Equals(other);

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly override int GetHashCode() => HashCode.Combine(mX, mY, mW, mH);

	/// <inheritdoc/>
	public readonly override string ToString() => ToString(format: default, formatProvider: default);

	/// <inheritdoc cref="IFormattable.ToString(string?, IFormatProvider?)"/>
	public readonly string ToString(IFormatProvider? formatProvider) => ToString(format: default, formatProvider);

	/// <inheritdoc cref="IFormattable.ToString(string?, IFormatProvider?)"/>
	public readonly string ToString(string? format) => ToString(format, formatProvider: default);

	/// <inheritdoc/>
	public readonly string ToString(string? format, IFormatProvider? formatProvider)
		=> $"{{ {nameof(Left)}: {mX.ToString(format, formatProvider)}, {
			nameof(Top)}: {mY.ToString(format, formatProvider)}, {
			nameof(Width)}: {mW.ToString(format, formatProvider)}, {
			nameof(Height)}: {mH.ToString(format, formatProvider)} }}";

	/// <inheritdoc/>
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

	/// <inheritdoc cref="IEqualityOperators{TSelf, TOther, TResult}.operator=="/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator ==(in Rect<T> left, in Rect<T> right)
		=> left.mX == right.mX
		&& left.mY == right.mY
		&& left.mW == right.mW
		&& left.mH == right.mH;

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static bool IEqualityOperators<Rect<T>, Rect<T>, bool>.operator ==(Rect<T> left, Rect<T> right) => left == right;

	/// <inheritdoc cref="IEqualityOperators{TSelf, TOther, TResult}.operator!="/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator !=(in Rect<T> left, in Rect<T> right)
		=> left.mX != right.mX
		|| left.mY != right.mY
		|| left.mW != right.mW
		|| left.mH != right.mH;

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static bool IEqualityOperators<Rect<T>, Rect<T>, bool>.operator !=(Rect<T> left, Rect<T> right) => left != right;

	/// <summary>
	/// Converts a (Left, Top, Width, Height) tuple to a <see cref="Rect{T}"/>
	/// </summary>
	/// <param name="ltwh">The tuple containing the left, top, width, and height of the rectangle</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static implicit operator Rect<T>(in (T Left, T Top, T Width, T Height) ltwh) => new(in ltwh.Left, in ltwh.Top, in ltwh.Width, in ltwh.Height);

	/// <summary>
	/// Converts a <see cref="Rect{T}"/> to a (Left, Top, Width, Height) tuple
	/// </summary>
	/// <param name="rect">The rectangle to convert</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static implicit operator (T Left, T Top, T Width, T Height)(in Rect<T> rect) => (rect.mX, rect.mY, rect.mW, rect.mH);
}
