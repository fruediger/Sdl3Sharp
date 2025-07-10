using Sdl3Sharp.Internal.NativeImportConditions;
using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Utilities;

/// <summary>
/// Provides mathematical constants and methods for various mathematical operations
/// </summary>
public static partial class Math
{
	/// <summary>
	/// The value of the mathematical constant π as a <see cref="double"/> constant
	/// </summary>
	public const double Pi = 3.141592653589793238462643383279502884;

	/// <summary>
	/// The value of the mathematical constant π as a <see cref="float"/> constant
	/// </summary>
	public const float PiF = 3.141592653589793238462643383279502884f;

	/// <summary>
	/// A type only used to dispatch some method calls to signatures with types <typeparamref name="T"/> that implement <see cref="IComparable{T}"/>
	/// </summary>
	/// <typeparam name="T">A type implementing <see cref="IComparable{T}"/></typeparam>
	/// <remarks>
	/// <em>Do not use in user code! This type is only used to dispatch method calls.</em>
	/// </remarks>
	public interface IDispatchComparable<T> where T : IComparable<T>
	{
		private protected void _(); // this makes the interface unimplementable outside of this assembly, so it can only be used to dispatch to the correct method
	}

	/// <summary>
	/// A type only used to dispatch some method calls to signatures with types <typeparamref name="T"/> that implement <see cref="IComparisonOperators{TSelf, TOther, TResult}"/>
	/// </summary>
	/// <typeparam name="T">A type implementing <see cref="IComparisonOperators{TSelf, TOther, TResult}"/></typeparam>
	/// <remarks>
	/// <em>Do not use in user code! This type is only used to dispatch method calls.</em>
	/// </remarks>
	public interface IDispatchComparisonOperators<T> where T : IComparisonOperators<T, T, bool>
	{
		private protected void _(); // this makes the interface unimplementable outside of this assembly, so it can only be used to dispatch to the correct method
	}

	/// <summary>
	/// Computes the absolute value of a specified integer value
	/// </summary>
	/// <param name="x">The integer value whose absolute value should be calculated</param>
	/// <returns>The absolute value of <paramref name="x"/></returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static int Abs(int x) => SDL_abs(x);

	/// <summary>
	/// Computes the absolute value of a specified real value
	/// </summary>
	/// <param name="x">The real value whose absolute value should be calculated</param>
	/// <returns>The absolute value of <paramref name="x"/>. The return value is in the range [0, ∞].</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static double Abs(double x) => SDL_fabs(x);

	/// <summary>
	/// Computes the absolute value of a specified real value
	/// </summary>
	/// <param name="x">The real value whose absolute value should be calculated</param>
	/// <returns>The absolute value of <paramref name="x"/>. The return value is in the range [0, ∞].</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static float Abs(float x) => SDL_fabsf(x);

	/// <summary>
	/// Computes the arc cosine of a specified real value
	/// </summary>
	/// <param name="x">The real value whose arc cosine should be calculated. Should be in the range [-1, 1].</param>
	/// <returns>The arc cosine of <paramref name="x"/>, in radians. The return value is in the range [0, π].</returns>
	/// <remarks>
	/// <para>
	/// This method may use a different approximation across different versions, platforms and configurations.
	/// It could return a different value given the same input on different machines or operating systems, or if SDL is updated.
	/// </para>
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static double Acos(double x) => SDL_acos(x);

	/// <summary>
	/// Computes the arc cosine of a specified real value
	/// </summary>
	/// <param name="x">The real value whose arc cosine should be calculated. Should be in the range [-1, 1].</param>
	/// <returns>The arc cosine of <paramref name="x"/>, in radians. The return value is in the range [0, π].</returns>
	/// <remarks>
	/// <para>
	/// This method may use a different approximation across different versions, platforms and configurations.
	/// It could return a different value given the same input on different machines or operating systems, or if SDL is updated.
	/// </para>
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static float Acos(float x) => SDL_acosf(x);

	/// <summary>
	/// Computes the arc sine of a specified real value
	/// </summary>
	/// <param name="x">The real value whose arc sine should be calculated. Should be in the range [-1, 1].</param>
	/// <returns>The arc sine of <paramref name="x"/>, in radians. The return value is in the range [-½π, ½π].</returns>
	/// <remarks>
	/// <para>
	/// This method may use a different approximation across different versions, platforms and configurations.
	/// It could return a different value given the same input on different machines or operating systems, or if SDL is updated.
	/// </para>
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static double Asin(double x) => SDL_asin(x);

	/// <summary>
	/// Computes the arc sine of a specified real value
	/// </summary>
	/// <param name="x">The real value whose arc sine should be calculated. Should be in the range [-1, 1].</param>
	/// <returns>The arc sine of <paramref name="x"/>, in radians. The return value is in the range [-½π, ½π].</returns>
	/// <remarks>
	/// <para>
	/// This method may use a different approximation across different versions, platforms and configurations.
	/// It could return a different value given the same input on different machines or operating systems, or if SDL is updated.
	/// </para>
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static float Asin(float x) => SDL_asinf(x);

	/// <summary>
	/// Computes the arc tangent of a specified real value
	/// </summary>
	/// <param name="x">The real value whose arc tangent should be calculated</param>
	/// <returns>The arc tangent of <paramref name="x"/>, in radians, or <c>0</c> if <paramref name="x"/> is <c>0</c>. The return value is in the range [-½π, ½π].</returns>
	/// <remarks>
	/// <para>
	/// To calculate the arc tangent of a quotient of two real values, use <see cref="Atan2(double, double)"/> instead.
	/// </para>
	/// <para>
	/// This method may use a different approximation across different versions, platforms and configurations.
	/// It could return a different value given the same input on different machines or operating systems, or if SDL is updated.
	/// </para>
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static double Atan(double x) => SDL_atan(x);

	/// <summary>
	/// Computes the arc tangent of a specified real value
	/// </summary>
	/// <param name="x">The real value whose arc tangent should be calculated</param>
	/// <returns>The arc tangent of <paramref name="x"/>, in radians, or <c>0</c> if <paramref name="x"/> is <c>0</c>. The return value is in the range [-½π, ½π].</returns>
	/// <remarks>
	/// <para>
	/// To calculate the arc tangent of a quotient of two real values, use <see cref="Atan2(float, float)"/> instead.
	/// </para>
	/// <para>
	/// This method may use a different approximation across different versions, platforms and configurations.
	/// It could return a different value given the same input on different machines or operating systems, or if SDL is updated.
	/// </para>
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static float Atan(float x) => SDL_atanf(x);

	/// <summary>
	/// Compute the arc tangent of a quotient of two specified real values, using their signs to adjust the result's quadrant
	/// </summary>
	/// <param name="y">The real value of the numerator of the quotient (y coordinate) whose arc tangent should be calculated</param>
	/// <param name="x">The real value of the denominator of the quotient (x coordinate) whose arc tangent should be calculated</param>
	/// <returns>The arc tangent of <c><paramref name="y"/> / <paramref name="x"/></c>, in radians, or if <paramref name="x"/> is <c>0</c>, either -½π, 0, ½π, depending on the value of <paramref name="y"/>. The return value is in the range [-π, π].</returns>
	/// <remarks>
	/// <para>
	/// To calculate the arc tangent of a single real value, use <see cref="Atan(double)"/> instead.
	/// </para>
	/// <para>
	/// This method may use a different approximation across different versions, platforms and configurations.
	/// It could return a different value given the same input on different machines or operating systems, or if SDL is updated.
	/// </para>
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static double Atan2(double y, double x) => SDL_atan2(y, x);

	/// <summary>
	/// Compute the arc tangent of a quotient of two specified real values, using their signs to adjust the result's quadrant
	/// </summary>
	/// <param name="y">The real value of the numerator of the quotient (y coordinate) whose arc tangent should be calculated</param>
	/// <param name="x">The real value of the denominator of the quotient (x coordinate) whose arc tangent should be calculated</param>
	/// <returns>The arc tangent of <c><paramref name="y"/> / <paramref name="x"/></c>, in radians, or if <paramref name="x"/> is <c>0</c>, either -½π, 0, ½π, depending on the value of <paramref name="y"/>. The return value is in the range [-π, π].</returns>
	/// <remarks>
	/// <para>
	/// To calculate the arc tangent of a single real value, use <see cref="Atan(float)"/> instead.
	/// </para>
	/// <para>
	/// This method may use a different approximation across different versions, platforms and configurations.
	/// It could return a different value given the same input on different machines or operating systems, or if SDL is updated.
	/// </para>
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static float Atan2(float y, float x) => SDL_atan2f(y, x);

	/// <summary>
	/// Computes the ceiling of a specified real value
	/// </summary>
	/// <param name="x">The real value whose ceiling should be calculated</param>
	/// <returns>The ceiling of <paramref name="x"/>, which is the smallest integer <c>y</c> such that <c>y ≥ <paramref name="x"/></c></returns>
	public static double Ceil(double x) => SDL_ceil(x);

	/// <summary>
	/// Computes the ceiling of a specified real value
	/// </summary>
	/// <param name="x">The real value whose ceiling should be calculated</param>
	/// <returns>The ceiling of <paramref name="x"/>, which is the smallest integer <c>y</c> such that <c>y ≥ <paramref name="x"/></c></returns>
	public static float Ceil(float x) => SDL_ceilf(x);

	/// <summary>
	/// Clamps a specified value to a specified range
	/// </summary>
	/// <typeparam name="T">The type of value to clamp</typeparam>
	/// <param name="value">The value which should be clamped to the specified range</param>
	/// <param name="min">The lower end value of the range to which the value should be clamped</param>
	/// <param name="max">The upper end value of the range to which the value should be clamped</param>
	/// <param name="_"><em>Please ignore and do not explicitly set this parameter</em></param>
	/// <returns><paramref name="min"/> or <paramref name="max"/> as appropriate, if <paramref name="value"/> is outside the range [<paramref name="min"/>, <paramref name="max"/>]; otherwise <paramref name="value"/></returns>
	/// <remarks>
	/// <para>
	/// This method will produce incorrect results if <paramref name="max"/> is less than <paramref name="min"/>. Note: There are no additional checks in place to prevent that kind of behavior.
	/// </para>
	/// <para>
	/// This method works for types of <paramref name="value"/> <typeparamref name="T"/> which implement <see cref="IComparable{T}"/>.
	/// If <typeparamref name="T"/> implements both <see cref="IComparable{T}"/> and <see cref="IComparisonOperators{TSelf, TOther, TResult}"/>,
	/// overload resolution prioritizes <see cref="Clamp{T}(T, T, T, IDispatchComparisonOperators{T}?)"/> instead.
	/// </para>
	/// <para>
	/// <em>Please ignore and do not explicitly set the <paramref name="_"/> parameter</em>. This parameter is just used in the method's signature to dispatch calls to this method for types of <paramref name="value"/> <typeparamref name="T"/> which implement <see cref="IComparable{T}"/>.
	/// </para>
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static T Clamp<T>(T value, T min, T max, IDispatchComparable<T>? _ = default)
		where T : IComparable<T>
		=> value.CompareTo(min) is < 0 ? min : value.CompareTo(max) is > 0 ? max : value;
	
	/// <summary>
	/// Clamps a specified value to a specified range
	/// </summary>
	/// <typeparam name="T">The type of value to clamp</typeparam>
	/// <param name="value">The value which should be clamped to the specified range</param>
	/// <param name="min">The lower end value of the range to which the value should be clamped</param>
	/// <param name="max">The upper end value of the range to which the value should be clamped</param>
	/// <param name="_"><em>Please ignore and do not explicitly set this parameter</em></param>
	/// <returns><paramref name="min"/> or <paramref name="max"/> as appropriate, if <paramref name="value"/> is outside the range [<paramref name="min"/>, <paramref name="max"/>]; otherwise <paramref name="value"/></returns>
	/// <remarks>
	/// <para>
	/// This method will produce incorrect results if <paramref name="max"/> is less than <paramref name="min"/>. Note: There are no additional checks in place to prevent that kind of behavior.
	/// </para>
	/// <para>
	/// This method works for types of <paramref name="value"/> <typeparamref name="T"/> which implement <see cref="IComparisonOperators{TSelf, TOther, TResult}"/>.
	/// If <typeparamref name="T"/> implements both <see cref="IComparisonOperators{TSelf, TOther, TResult}"/> and <see cref="IComparable{T}"/>,
	/// overload resolution prioritizes this method.
	/// </para>
	/// <para>
	/// <em>Please ignore and do not explicitly set the <paramref name="_"/> parameter</em>. This parameter is just used in the method's signature to dispatch calls to this method for types of <paramref name="value"/> <typeparamref name="T"/> which implement <see cref="IComparisonOperators{TSelf, TOther, TResult}"/>.
	/// </para>
	/// </remarks>
	[OverloadResolutionPriority(1)]
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static T Clamp<T>(T value, T min, T max, IDispatchComparisonOperators<T>? _ = default)
		where T : IComparisonOperators<T, T, bool>
		=> value < min ? min : value > max ? max : value;

	/// <summary>
	/// Copies the sign of a real value to another
	/// </summary>
	/// <param name="x">The real value to use as the results magnitude</param>
	/// <param name="y">The real value to use as the results sign</param>
	/// <returns>A real value with the sign of <paramref name="y"/> and the magnitude of <paramref name="x"/></returns>
	/// <remarks>
	/// <para>
	/// This method essentially returns <em>abs</em>(<paramref name="x"/>) ⋅ <em>sgn</em>(<paramref name="y"/>).
	/// </para>
	/// <para>
	/// <list type="bullet">
	///		<item>
	///			<description>
	///				If you want to compute the magnitude of a real number <c>x</c> (a.k.a absolute value or <em>abs</em>), you can use <c><see cref="CopySign(double, double)">CopySign</see>(x, 1)</c>.
	///				You also could use <see cref="Abs(double)"/> instead.
	///			</description>
	///		</item>
	///		<item>
	///			<description>
	///				If you want to get the sign of a real number <c>x</c> (a.k.a <em>sgn</em>), you can use <c><see cref="CopySign(double, double)">CopySign</see>(1, x)</c>
	///			</description>
	///		</item>
	/// </list>
	/// </para>
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static double CopySign(double x, double y) => SDL_copysign(x, y);

	/// <summary>
	/// Copies the sign of a real value to another
	/// </summary>
	/// <param name="x">The real value to use as the results magnitude</param>
	/// <param name="y">The real value to use as the results sign</param>
	/// <returns>A real value with the sign of <paramref name="y"/> and the magnitude of <paramref name="x"/></returns>
	/// <remarks>
	/// <para>
	/// This method essentially returns <em>abs</em>(<paramref name="x"/>) ⋅ <em>sgn</em>(<paramref name="y"/>).
	/// </para>
	/// <para>
	/// <list type="bullet">
	///		<item>
	///			<description>
	///				If you want to compute the magnitude of a real number <c>x</c> (a.k.a absolute value or <em>abs</em>), you can use <c><see cref="CopySign(float, float)">CopySign</see>(x, 1)</c>	///				
	///				You also could use <see cref="Abs(float)"/> instead.
	///			</description>
	///		</item>
	///		<item>
	///			<description>
	///				If you want to get the sign of a real number <c>x</c> (a.k.a <em>sgn</em>), you can use <c><see cref="CopySign(float, float)">CopySign</see>(1, x)</c>
	///			</description>
	///		</item>
	/// </list>
	/// </para>
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static float CopySign(float x, float y) => SDL_copysignf(x, y);

	/// <summary>
	/// Computes the cosine of a specified real value
	/// </summary>
	/// <param name="x">The real value, in radians, whose cosine should be calculated</param>
	/// <returns>The cosine of <paramref name="x"/>. The return value is in the range [-1, 1].</returns>
	/// <remarks>
	/// <para>
	/// This method may use a different approximation across different versions, platforms and configurations.
	/// It could return a different value given the same input on different machines or operating systems, or if SDL is updated.
	/// </para>
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static double Cos(double x) => SDL_cos(x);

	/// <summary>
	/// Computes the cosine of a specified real value
	/// </summary>
	/// <param name="x">The real value, in radians, whose cosine should be calculated</param>
	/// <returns>The cosine of <paramref name="x"/>. The return value is in the range [-1, 1].</returns>
	/// <remarks>
	/// <para>
	/// This method may use a different approximation across different versions, platforms and configurations.
	/// It could return a different value given the same input on different machines or operating systems, or if SDL is updated.
	/// </para>
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static float Cos(float x) => SDL_cosf(x);

	/// <summary>
	/// Computes the exponential of a specified real value
	/// </summary>
	/// <param name="x">The real value whose exponential should be calculated</param>
	/// <returns>The value of <c>e</c> raised to the power of <paramref name="x"/>, where <c>e</c> is <see href="https://en.wikipedia.org/wiki/Euler%27s_number">Euler's number</see>. The return value is in the range [0, ∞].</returns>
	/// <remarks>
	/// <para>
	/// Note: The result will overflow if <c>e</c> raised to power of <paramref name="x"/> is too large to be represented as a <see cref="double"/>.
	/// </para>
	/// <para>
	/// This method may use a different approximation across different versions, platforms and configurations.
	/// It could return a different value given the same input on different machines or operating systems, or if SDL is updated.
	/// </para>
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static double Exp(double x) => SDL_exp(x);

	/// <summary>
	/// Computes the exponential of a specified real value
	/// </summary>
	/// <param name="x">The real value whose exponential should be calculated</param>
	/// <returns>The value of <c>e</c> raised to the power of <paramref name="x"/>, where <c>e</c> is <see href="https://en.wikipedia.org/wiki/Euler%27s_number">Euler's number</see>. The return value is in the range [0, ∞].</returns>
	/// <remarks>
	/// <para>
	/// Note: The result will overflow if <c>e</c> raised to power of <paramref name="x"/> is too large to be represented as a <see cref="float"/>.
	/// </para>
	/// <para>
	/// This method may use a different approximation across different versions, platforms and configurations.
	/// It could return a different value given the same input on different machines or operating systems, or if SDL is updated.
	/// </para>
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static float Exp(float x) => SDL_expf(x);

	/// <summary>
	/// Computes the floor of a specified real value
	/// </summary>
	/// <param name="x">The real value whose floor should be calculated</param>
	/// <returns>The floor of <paramref name="x"/>, which is the largest integer <c>y</c> such that <c>y ≤ <paramref name="x"/></c></returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static double Floor(double x) => SDL_floor(x);

	/// <summary>
	/// Computes the floor of a specified real value
	/// </summary>
	/// <param name="x">The real value whose floor should be calculated</param>
	/// <returns>The floor of <paramref name="x"/>, which is the largest integer <c>y</c> such that <c>y ≤ <paramref name="x"/></c></returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static float Floor(float x) => SDL_floorf(x);

	/// <summary>
	/// Determines whether a floating point value represents infinity
	/// </summary>
	/// <param name="x">The floating point value which should be checked if it represents infinity</param>
	/// <returns><c><see langword="true"/></c> if <paramref name="x"/> represents infinity; otherwise <c><see langword="false"/></c></returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool IsInfinity(double x) => SDL_isinf(x) is not 0;

	/// <summary>
	/// Determines whether a floating point value represents infinity
	/// </summary>
	/// <param name="x">The floating point value which should be checked if it represents infinity</param>
	/// <returns><c><see langword="true"/></c> if <paramref name="x"/> represents infinity; otherwise <c><see langword="false"/></c></returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool IsInfinity(float x) => SDL_isinff(x) is not 0;

	/// <summary>
	/// Determines whether a floating point value represents not a number (NaN)
	/// </summary>
	/// <param name="x">The floating point value which should be checked if it represents not a number (NaN)</param>
	/// <returns><c><see langword="true"/></c> if <paramref name="x"/> represents not a number (NaN); otherwise <c><see langword="false"/></c></returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool IsNaN(double x) => SDL_isnan(x) is not 0;

	/// <summary>
	/// Determines whether a floating point value represents not a number (NaN)
	/// </summary>
	/// <param name="x">The floating point value which should be checked if it represents not a number (NaN)</param>
	/// <returns><c><see langword="true"/></c> if <paramref name="x"/> represents not a number (NaN); otherwise <c><see langword="false"/></c></returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool IsNaN(float x) => SDL_isnanf(x) is not 0;

	/// <summary>
	/// Computes the natural logarithm of a specified real value
	/// </summary>
	/// <param name="x">The real value whose natural logarithm should be calculated. Must be greater than <c>0</c>.</param>
	/// <returns>The natural logarithm of <paramref name="x"/></returns>
	/// <remarks>
	/// <para>
	/// Note: There are no additional checks in place to prevent <paramref name="x"/> from being less than or equal to <c>0</c>. Using such values as arguments for <paramref name="x"/> for <see cref="Log(double)"/> is an error! You must make sure on your own that <paramref name="x"/> is greater than <c>0</c>.
	/// </para>
	/// <para>
	/// This method may use a different approximation across different versions, platforms and configurations.
	/// It could return a different value given the same input on different machines or operating systems, or if SDL is updated.
	/// </para>
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static double Log(double x) => SDL_log(x);

	/// <summary>
	/// Computes the natural logarithm of a specified real value
	/// </summary>
	/// <param name="x">The real value whose natural logarithm should be calculated. Must be greater than <c>0</c>.</param>
	/// <returns>The natural logarithm of <paramref name="x"/></returns>
	/// <remarks>
	/// <para>
	/// Note: There are no additional checks in place to prevent <paramref name="x"/> from being less than or equal to <c>0</c>. Using such values as arguments for <paramref name="x"/> for <see cref="Log(float)"/> is an error! You must make sure on your own that <paramref name="x"/> is greater than <c>0</c>.
	/// </para>
	/// <para>
	/// This method may use a different approximation across different versions, platforms and configurations.
	/// It could return a different value given the same input on different machines or operating systems, or if SDL is updated.
	/// </para>
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static float Log(float x) => SDL_logf(x);

	/// <summary>
	/// Computes the logarithm of a specified real value to base 10
	/// </summary>
	/// <param name="x">The real value whose logarithm to base 10 should be calculated. Must be greater than <c>0</c>.</param>
	/// <returns>The logarithm of <paramref name="x"/> to base 10</returns>
	/// <remarks>
	/// <para>
	/// Note: There are no additional checks in place to prevent <paramref name="x"/> from being less than or equal to <c>0</c>. Using such values as arguments for <paramref name="x"/> for <see cref="Log(double)"/> is an error! You must make sure on your own that <paramref name="x"/> is greater than <c>0</c>.
	/// </para>
	/// <para>
	/// This method may use a different approximation across different versions, platforms and configurations.
	/// It could return a different value given the same input on different machines or operating systems, or if SDL is updated.
	/// </para>
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static double Log10(double x) => SDL_log10(x);

	/// <summary>
	/// Computes the logarithm of a specified real value to base 10
	/// </summary>
	/// <param name="x">The real value whose logarithm to base 10 should be calculated. Must be greater than <c>0</c>.</param>
	/// <returns>The logarithm of <paramref name="x"/> to base 10</returns>
	/// <remarks>
	/// <para>
	/// Note: There are no additional checks in place to prevent <paramref name="x"/> from being less than or equal to <c>0</c>. Using such values as arguments for <paramref name="x"/> for <see cref="Log(double)"/> is an error! You must make sure on your own that <paramref name="x"/> is greater than <c>0</c>.
	/// </para>
	/// <para>
	/// This method may use a different approximation across different versions, platforms and configurations.
	/// It could return a different value given the same input on different machines or operating systems, or if SDL is updated.
	/// </para>
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static float Log10(float x) => SDL_log10f(x);

	/// <summary>
	/// Returns the greater value of two specified values
	/// </summary>
	/// <typeparam name="T">The type of value</typeparam>
	/// <param name="x">The first value to compare</param>
	/// <param name="y">The second value to compare</param>
	/// <param name="_"><em>Please ignore and do not explicitly set this parameter</em></param>
	/// <returns>The greater value of <paramref name="x"/> and <paramref name="y"/></returns>
	/// <remarks> 
	/// <para>
	/// This method works for types of <paramref name="x"/> and <paramref name="y"/> <typeparamref name="T"/> which implement <see cref="IComparable{T}"/>.
	/// If <typeparamref name="T"/> implements both <see cref="IComparable{T}"/> and <see cref="IComparisonOperators{TSelf, TOther, TResult}"/>,
	/// overload resolution prioritizes <see cref="Max{T}(T, T, IDispatchComparisonOperators{T}?)"/> instead.
	/// </para>
	/// <para>
	/// <em>Please ignore and do not explicitly set the <paramref name="_"/> parameter</em>. This parameter is just used in the method's signature to dispatch calls to this method for types of <paramref name="x"/> and <paramref name="y"/> <typeparamref name="T"/> which implement <see cref="IComparable{T}"/>.
	/// </para>
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static T Max<T>(T x, T y, IDispatchComparable<T>? _ = default)
		where T : IComparable<T>
		=> x.CompareTo(y) is > 0 ? x : y;

	/// <summary>
	/// Returns the greater value of two specified values
	/// </summary>
	/// <typeparam name="T">The type of value</typeparam>
	/// <param name="x">The first value to compare</param>
	/// <param name="y">The second value to compare</param>
	/// <param name="_"><em>Please ignore and do not explicitly set this parameter</em></param>
	/// <returns>The greater value of <paramref name="x"/> and <paramref name="y"/></returns>
	/// <remarks> 
	/// <para>
	/// This method works for types of <paramref name="x"/> and <paramref name="y"/> <typeparamref name="T"/> which implement <see cref="IComparisonOperators{TSelf, TOther, TResult}"/>.
	/// If <typeparamref name="T"/> implements both <see cref="IComparable{T}"/> and <see cref="IComparisonOperators{TSelf, TOther, TResult}"/>,
	/// overload resolution prioritizes this method.
	/// </para>
	/// <para>
	/// <em>Please ignore and do not explicitly set the <paramref name="_"/> parameter</em>. This parameter is just used in the method's signature to dispatch calls to this method for types of <paramref name="x"/> and <paramref name="y"/> <typeparamref name="T"/> which implement <see cref="IComparisonOperators{TSelf, TOther, TResult}"/>.
	/// </para>
	/// </remarks>
	[OverloadResolutionPriority(1)]
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static T Max<T>(T x, T y, IDispatchComparisonOperators<T>? _ = default)
		where T : IComparisonOperators<T, T, bool>
		=> x > y ? x : y;

	/// <summary>
	/// Returns the lesser value of two specified values
	/// </summary>
	/// <typeparam name="T">The type of value</typeparam>
	/// <param name="x">The first value to compare</param>
	/// <param name="y">The second value to compare</param>
	/// <param name="_"><em>Please ignore and do not explicitly set this parameter</em></param>
	/// <returns>The lesser value of <paramref name="x"/> and <paramref name="y"/></returns>
	/// <remarks> 
	/// <para>
	/// This method works for types of <paramref name="x"/> and <paramref name="y"/> <typeparamref name="T"/> which implement <see cref="IComparable{T}"/>.
	/// If <typeparamref name="T"/> implements both <see cref="IComparable{T}"/> and <see cref="IComparisonOperators{TSelf, TOther, TResult}"/>,
	/// overload resolution prioritizes <see cref="Max{T}(T, T, IDispatchComparisonOperators{T}?)"/> instead.
	/// </para>
	/// <para>
	/// <em>Please ignore and do not explicitly set the <paramref name="_"/> parameter</em>. This parameter is just used in the method's signature to dispatch calls to this method for types of <paramref name="x"/> and <paramref name="y"/> <typeparamref name="T"/> which implement <see cref="IComparable{T}"/>.
	/// </para>
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static T Min<T>(T x, T y, IDispatchComparable<T>? _ = default)
		where T : IComparable<T>
		=> x.CompareTo(y) is < 0 ? x : y;

	/// <summary>
	/// Returns the lesser value of two specified values
	/// </summary>
	/// <typeparam name="T">The type of value</typeparam>
	/// <param name="x">The first value to compare</param>
	/// <param name="y">The second value to compare</param>
	/// <param name="_"><em>Please ignore and do not explicitly set this parameter</em></param>
	/// <returns>The lesser value of <paramref name="x"/> and <paramref name="y"/></returns>
	/// <remarks> 
	/// <para>
	/// This method works for types of <paramref name="x"/> and <paramref name="y"/> <typeparamref name="T"/> which implement <see cref="IComparisonOperators{TSelf, TOther, TResult}"/>.
	/// If <typeparamref name="T"/> implements both <see cref="IComparable{T}"/> and <see cref="IComparisonOperators{TSelf, TOther, TResult}"/>,
	/// overload resolution prioritizes this method.
	/// </para>
	/// <para>
	/// <em>Please ignore and do not explicitly set the <paramref name="_"/> parameter</em>. This parameter is just used in the method's signature to dispatch calls to this method for types of <paramref name="x"/> and <paramref name="y"/> <typeparamref name="T"/> which implement <see cref="IComparisonOperators{TSelf, TOther, TResult}"/>.
	/// </para>
	/// </remarks>
	[OverloadResolutionPriority(1)]
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static T Min<T>(T x, T y, IDispatchComparisonOperators<T>? _ = default)
		where T : IComparisonOperators<T, T, bool>
		=> x < y ? x : y;

	/// <summary>
	/// Computes the remainder of the division of two specified real values
	/// </summary>
	/// <param name="x">The real value of the numerator of the division whose remainder should be calculated</param>
	/// <param name="y">The real value of the denominator of the division whose remainder should be calculated. Must not be <c>0</c>.</param>
	/// <returns>The remainder of the division of <paramref name="x"/> by <paramref name="y"/>. The return value is in the range [-<paramref name="y"/>, <paramref name="y"/>].</returns>
	/// <remarks> 
	/// <para>
	/// Note: There are no additional checks in place to prevent <paramref name="y"/> from being <c>0</c>. You must make sure on your own that <paramref name="y"/> is not <c>0</c>.
	/// </para>
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static double Mod(double x, double y) => SDL_fmod(x, y);

	/// <summary>
	/// Computes the remainder of the division of two specified real values
	/// </summary>
	/// <param name="x">The real value of the numerator of the division whose remainder should be calculated</param>
	/// <param name="y">The real value of the denominator of the division whose remainder should be calculated. Must not be <c>0</c>.</param>
	/// <returns>The remainder of the division of <paramref name="x"/> by <paramref name="y"/>. The return value is in the range [-<paramref name="y"/>, <paramref name="y"/>].</returns>
	/// <remarks> 
	/// <para>
	/// Note: There are no additional checks in place to prevent <paramref name="y"/> from being <c>0</c>. You must make sure on your own that <paramref name="y"/> is not <c>0</c>.
	/// </para>
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static float Mod(float x, float y) => SDL_fmodf(x, y);

	/// <summary>
	/// Splits a specified real value into its integer and fractional parts
	/// </summary>
	/// <param name="x">The real value which should be split into its integer and fractional parts</param>
	/// <param name="integerPart">The integer part of <paramref name="x"/></param>
	/// <returns>The fractional part of <paramref name="x"/></returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static double Mod(double x, out double integerPart)
	{
		unsafe
		{
			double y;

			var result = SDL_modf(x, &y);

			integerPart = y;

			return result;
		}
	}

	/// <summary>
	/// Splits a specified real value into its integer and fractional parts
	/// </summary>
	/// <param name="x">The real value which should be split into its integer and fractional part</param>
	/// <returns>The integer and fractional part of <paramref name="x"/></returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static (double Integer, double Fractional) Mod(double x)
	{
		unsafe
		{
			(double Integer, double Fractional) result;

			result.Fractional = SDL_modf(x, &result.Integer);

			return result;
		}
	}

	/// <summary>
	/// Splits a specified real value into its integer and fractional parts
	/// </summary>
	/// <param name="x">The real value which should be split into its integer and fractional parts</param>
	/// <param name="integerPart">The integer part of <paramref name="x"/></param>
	/// <returns>The fractional part of <paramref name="x"/></returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static float Mod(float x, out float integerPart)
	{
		unsafe
		{
			float y;

			var result = SDL_modff(x, &y);

			integerPart = y;

			return result;
		}
	}

	/// <summary>
	/// Splits a specified real value into its integer and fractional parts
	/// </summary>
	/// <param name="x">The real value which should be split into its integer and fractional part</param>
	/// <returns>The integer and fractional part of <paramref name="x"/></returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static (float Integer, float Fractional) Mod(float x)
	{
		unsafe
		{
			(float Integer, float Fractional) result;

			result.Fractional = SDL_modff(x, &result.Integer);

			return result;
		}
	}

	/// <summary>
	///  Computes the exponentiation of a specified real value raised to another specified real value
	/// </summary>
	/// <param name="x">The real value which should be the base of the exponentiation to be calculated</param>
	/// <param name="y">The real value which shoudl be the exponent of the exponentiation to be calculated</param>
	/// <returns>The value of <paramref name="x"/> raised to the power of <paramref name="y"/></returns>
	/// <remarks>
	/// <para>
	/// If <paramref name="y"/> shall be <see href="https://en.wikipedia.org/wiki/Euler%27s_number">Euler's number</see> (a.k.a. the base of the natural logarithm <c>e</c>), consider using <see cref="Exp(double)"/> instead.
	/// </para>
	/// <para>
	/// This method may use a different approximation across different versions, platforms and configurations.
	/// It could return a different value given the same input on different machines or operating systems, or if SDL is updated.
	/// </para>
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static double Pow(double x, double y) => SDL_pow(x, y);

	/// <summary>
	///  Computes the exponentiation of a specified real value raised to another specified real value
	/// </summary>
	/// <param name="x">The real value which should be the base of the exponentiation to be calculated</param>
	/// <param name="y">The real value which shoudl be the exponent of the exponentiation to be calculated</param>
	/// <returns>The value of <paramref name="x"/> raised to the power of <paramref name="y"/></returns>
	/// <remarks>
	/// <para>
	/// If <paramref name="y"/> shall be <see href="https://en.wikipedia.org/wiki/Euler%27s_number">Euler's number</see> (a.k.a. the base of the natural logarithm <c>e</c>), consider using <see cref="Exp(float)"/> instead.
	/// </para>
	/// <para>
	/// This method may use a different approximation across different versions, platforms and configurations.
	/// It could return a different value given the same input on different machines or operating systems, or if SDL is updated.
	/// </para>
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static float Pow(float x, float y) => SDL_powf(x, y);

	/// <summary>
	/// Rounds a specified real value to the nearest integer value
	/// </summary>
	/// <param name="x">The real value which should get rounded to the nearest integer value</param>
	/// <returns>The nearest integer value to <paramref name="x"/></returns>
	/// <remarks>
	/// <para>
	/// Values for <paramref name="x"/> which are halfway between integers will be rounded away from zero.
	/// </para>
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static double Round(double x) => SDL_round(x);

	/// <summary>
	/// Rounds a specified real value to the nearest integer value
	/// </summary>
	/// <param name="x">The real value which should get rounded to the nearest integer value</param>
	/// <returns>The nearest integer value to <paramref name="x"/></returns>
	/// <remarks>
	/// <para>
	/// Values for <paramref name="x"/> which are halfway between integers will be rounded away from zero.
	/// </para>
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static float Round(float x) => SDL_roundf(x);

	/// <summary>
	/// Rounds a specified real value to the nearest integer value
	/// </summary>
	/// <param name="x">The real value which should get rounded to the nearest integer value</param>
	/// <returns>The nearest integer value to <paramref name="x"/></returns>
	/// <remarks>
	/// <para>
	/// Values for <paramref name="x"/> which are halfway between integers will be rounded away from zero.
	/// </para>
	/// <para>
	/// On Windows, the return value is capped in the range of [<see cref="int.MinValue"/>, <see cref="int.MaxValue"/>].
	/// </para>
	/// <para>
	/// The get the result as a floating-point value, you can use <see cref="Round(double)"/> instead.
	/// </para>
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static long RoundToInteger(double x) => IsLLP64.Evaluate()
		? SDL_lround_LLP64(x)
		: SDL_lround_LP64(x);

	/// <summary>
	/// Rounds a specified real value to the nearest integer value
	/// </summary>
	/// <param name="x">The real value which should get rounded to the nearest integer value</param>
	/// <returns>The nearest integer value to <paramref name="x"/></returns>
	/// <remarks>
	/// <para>
	/// Values for <paramref name="x"/> which are halfway between integers will be rounded away from zero.
	/// </para>
	/// <para>
	/// On Windows, the return value is capped in the range of [<see cref="int.MinValue"/>, <see cref="int.MaxValue"/>].
	/// </para>
	/// <para>
	/// The get the result as a floating-point value, you can use <see cref="Round(float)"/> instead.
	/// </para>
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static long RoundToInteger(float x) => IsLLP64.Evaluate()
		? SDL_lroundf_LLP64(x)
		: SDL_lroundf_LP64(x);

	/// <summary>
	/// Scales a specified real value by a specified integer power of two
	/// </summary>
	/// <param name="x">The real value which should get scaled by an integer power of two</param>
	/// <param name="n">The integer exponent to raise two to</param>
	/// <returns>The result of <paramref name="x"/> multiplied by two raised to the power of <paramref name="n"/> (<c><paramref name="x"/>*2^<paramref name="n"/></c>)</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static double ScaleByPowerOfTwo(double x, int n) => SDL_scalbn(x, n);

	/// <summary>
	/// Scales a specified real value by a specified integer power of two
	/// </summary>
	/// <param name="x">The real value which should get scaled by an integer power of two</param>
	/// <param name="n">The integer exponent to raise two to</param>
	/// <returns>The result of <paramref name="x"/> multiplied by two raised to the power of <paramref name="n"/> (<c><paramref name="x"/>*2^<paramref name="n"/></c>)</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static float ScaleByPowerOfTwo(float x, int n) => SDL_scalbnf(x, n);

	/// <summary>
	/// Computes the sine of a specified real value
	/// </summary>
	/// <param name="x">The real value, in radians, whose sine should be calculated</param>
	/// <returns>The cosine of <paramref name="x"/>. The return value is in the range [-1, 1].</returns>
	/// <remarks>
	/// <para>
	/// This method may use a different approximation across different versions, platforms and configurations.
	/// It could return a different value given the same input on different machines or operating systems, or if SDL is updated.
	/// </para>
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static double Sin(double x) => SDL_sin(x);

	/// <summary>
	/// Computes the sine of a specified real value
	/// </summary>
	/// <param name="x">The real value, in radians, whose sine should be calculated</param>
	/// <returns>The cosine of <paramref name="x"/>. The return value is in the range [-1, 1].</returns>
	/// <remarks>
	/// <para>
	/// This method may use a different approximation across different versions, platforms and configurations.
	/// It could return a different value given the same input on different machines or operating systems, or if SDL is updated.
	/// </para>
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static float Sin(float x) => SDL_sinf(x);

	/// <summary>
	/// Computes the square root of a specified real value
	/// </summary>
	/// <param name="x">The real value whose square should be calculated. Must be greater than or equal to <c>0</c>.</param>
	/// <returns>The square root of <paramref name="x"/>. The return value is in the range [0, ∞].</returns>
	/// <remarks>
	/// <para>
	/// Note: There are no additional checks in place to prevent <paramref name="x"/> from being less than <c>0</c>. You must make sure on your own that <paramref name="x"/> is greater than or equal to <c>0</c>.
	/// </para>
	/// <para>
	/// This method may use a different approximation across different versions, platforms and configurations.
	/// It could return a different value given the same input on different machines or operating systems, or if SDL is updated.
	/// </para>
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static double Sqrt(double x) => SDL_sqrt(x);

	/// <summary>
	/// Computes the square root of a specified real value
	/// </summary>
	/// <param name="x">The real value whose square should be calculated. Must be greater than or equal to <c>0</c>.</param>
	/// <returns>The square root of <paramref name="x"/>. The return value is in the range [0, ∞].</returns>
	/// <remarks>
	/// <para>
	/// Note: There are no additional checks in place to prevent <paramref name="x"/> from being less than <c>0</c>. You must make sure on your own that <paramref name="x"/> is greater than or equal to <c>0</c>.
	/// </para>
	/// <para>
	/// This method may use a different approximation across different versions, platforms and configurations.
	/// It could return a different value given the same input on different machines or operating systems, or if SDL is updated.
	/// </para>
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static float Sqrt(float x) => SDL_sqrtf(x);

	/// <summary>
	/// Computes the tangent of a specified real value
	/// </summary>
	/// <param name="x">The real value, in radians, whose tangent should be calculated</param>
	/// <returns>The tangent of <paramref name="x"/></returns>
	/// <remarks>
	/// <para>
	/// This method may use a different approximation across different versions, platforms and configurations.
	/// It could return a different value given the same input on different machines or operating systems, or if SDL is updated.
	/// </para>
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static double Tan(double x) => SDL_tan(x);

	/// <summary>
	/// Computes the tangent of a specified real value
	/// </summary>
	/// <param name="x">The real value, in radians, whose tangent should be calculated</param>
	/// <returns>The tangent of <paramref name="x"/></returns>
	/// <remarks>
	/// <para>
	/// This method may use a different approximation across different versions, platforms and configurations.
	/// It could return a different value given the same input on different machines or operating systems, or if SDL is updated.
	/// </para>
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static float Tan(float x) => SDL_tanf(x);

	/// <summary>
	/// Truncates a specified real value to the next closesest integer value to 0
	/// </summary>
	/// <param name="x">The real value which should get truncated to the next closesest integer value to 0</param>
	/// <returns>The next closesest integer value to 0 from <paramref name="x"/></returns>
	/// <remarks>
	/// <para>
	/// This is equivalent to removing the fractional part of <paramref name="x"/>, only leaving the integer part.
	/// </para>
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static double Trunc(double x) => SDL_trunc(x);

	/// <summary>
	/// Truncates a specified real value to the next closesest integer value to 0
	/// </summary>
	/// <param name="x">The real value which should get truncated to the next closesest integer value to 0</param>
	/// <returns>The next closesest integer value to 0 from <paramref name="x"/></returns>
	/// <remarks>
	/// <para>
	/// This is equivalent to removing the fractional part of <paramref name="x"/>, only leaving the integer part.
	/// </para>
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static float Trunc(float x) => SDL_truncf(x);
}
