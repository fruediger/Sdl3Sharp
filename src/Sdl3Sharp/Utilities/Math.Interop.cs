using Sdl3Sharp.Internal.Interop;
using Sdl3Sharp.Internal.Interop.NativeImportConditions;
using Sdl3Sharp.SourceGeneration;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Utilities;

partial class Math
{	
	/// <summary>
	/// Compute the absolute value of <c><paramref name="x"/></c>
	/// </summary>
	/// <param name="x">An integer value</param>
	/// <returns>Returns the absolute value of <c><paramref name="x"/></c></returns>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_abs">SDL_abs</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl), typeof(CallConvSuppressGCTransition)])]
	internal static partial int SDL_abs(int x);

	/// <summary>
	/// Compute the arc cosine of <c><paramref name="x"/></c>
	/// </summary>
	/// <param name="x">A floating point value</param>
	/// <returns>Returns arc cosine of <c><paramref name="x"/></c>, in radians</returns>
	/// <remarks>
	/// <para>
	/// The definition of <c>y = acos(x)</c> is <c>x = cos(y)</c>.
	/// </para>
	/// <para>
	/// Domain: <c>-1 &lt;= x &lt;= 1</c>
	/// </para>
	/// <para>
	/// Range: <c>0 &lt;= y &lt;= Pi</c>
	/// </para>
	/// <para>
	/// This function operates on double-precision floating point values, use <see href="https://wiki.libsdl.org/SDL3/SDL_acosf">SDL_acosf</see> for single-precision floats.
	/// </para>
	/// <para>
	/// This function may use a different approximation across different versions, platforms and configurations.
	/// i.e, it can return a different value given the same input on different machines or operating systems, or if SDL is updated.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_acos">SDL_acos</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl), typeof(CallConvSuppressGCTransition)])]
	internal static partial double SDL_acos(double x);

	/// <summary>
	/// Compute the arc cosine of <c><paramref name="x"/></c>
	/// </summary>
	/// <param name="x">A floating point value</param>
	/// <returns>Returns arc cosine of <c><paramref name="x"/></c>, in radians</returns>
	/// <remarks>
	/// <para>
	/// The definition of <c>y = acos(x)</c> is <c>x = cos(y)</c>.
	/// </para>
	/// <para>
	/// Domain: <c>-1 &lt;= x &lt;= 1</c>
	/// </para>
	/// <para>
	/// Range: <c>0 &lt;= y &lt;= Pi</c>
	/// </para>
	/// <para>
	/// This function operates on single-precision floating point values, use <see href="https://wiki.libsdl.org/SDL3/SDL_acos">SDL_acos</see> for double-precision floats.
	/// </para>
	/// <para>
	/// This function may use a different approximation across different versions, platforms and configurations.
	/// i.e, it can return a different value given the same input on different machines or operating systems, or if SDL is updated.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_acosf">SDL_acosf</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl), typeof(CallConvSuppressGCTransition)])]
	internal static partial float SDL_acosf(float x);

	/// <summary>
	/// Compute the arc sine of <c><paramref name="x"/></c>
	/// </summary>
	/// <param name="x">A floating point value</param>
	/// <returns>Returns arc sine of <c><paramref name="x"/></c>, in radians</returns>
	/// <remarks>
	/// <para>
	/// The definition of <c>y = asin(x)</c> is <c>x = sin(y)</c>.
	/// </para>
	/// <para>
	/// Domain: <c>-1 &lt;= x &lt;= 1</c>
	/// </para>
	/// <para>
	/// Range: <c>-Pi/2 &lt;= y &lt;= Pi/2</c>
	/// </para>
	/// <para>
	/// This function operates on double-precision floating point values, use <see href="https://wiki.libsdl.org/SDL3/SDL_asinf">SDL_asinf</see> for single-precision floats.
	/// </para>
	/// <para>
	/// This function may use a different approximation across different versions, platforms and configurations.
	/// i.e, it can return a different value given the same input on different machines or operating systems, or if SDL is updated.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_asin">SDL_asin</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl), typeof(CallConvSuppressGCTransition)])]
	internal static partial double SDL_asin(double x);

	/// <summary>
	/// Compute the arc sine of <c><paramref name="x"/></c>
	/// </summary>
	/// <param name="x">A floating point value</param>
	/// <returns>Returns arc sine of <c><paramref name="x"/></c>, in radians</returns>
	/// <remarks>
	/// <para>
	/// The definition of <c>y = asin(x)</c> is <c>x = sin(y)</c>.
	/// </para>
	/// <para>
	/// Domain: <c>-1 &lt;= x &lt;= 1</c>
	/// </para>
	/// <para>
	/// Range: <c>-Pi/2 &lt;= y &lt;= Pi/2</c>
	/// </para>
	/// <para>
	/// This function operates on single-precision floating point values, use <see href="https://wiki.libsdl.org/SDL3/SDL_asin">SDL_asin</see> for double-precision floats.
	/// </para>
	/// <para>
	/// This function may use a different approximation across different versions, platforms and configurations.
	/// i.e, it can return a different value given the same input on different machines or operating systems, or if SDL is updated.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_asinf">SDL_asinf</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl), typeof(CallConvSuppressGCTransition)])]
	internal static partial float SDL_asinf(float x);

	/// <summary>
	/// Compute the arc tangent of <c><paramref name="x"/></c>
	/// </summary>
	/// <param name="x">A floating point value</param>
	/// <returns>Returns arc tangent of <c><paramref name="x"/></c> in radians, or 0 if <c>x = 0</c></returns>
	/// <remarks>
	/// <para>
	/// The definition of <c>y = atan(x)</c> is <c>x = tan(y)</c>.
	/// </para>
	/// <para>
	/// Domain: <c>-INF &lt;= x &lt;= INF</c>
	/// </para>
	/// <para>
	/// Range: <c>-Pi/2 &lt;= y &lt;= Pi/2</c>
	/// </para>
	/// <para>
	/// This function operates on double-precision floating point values, use <see href="https://wiki.libsdl.org/SDL3/SDL_atanf">SDL_atanf</see> for single-precision floats.
	/// </para>
	/// <para>
	/// To calculate the arc tangent of y / x, use <see href="https://wiki.libsdl.org/SDL3/SDL_atan2">SDL_atan2</see>.
	/// </para>
	/// <para>
	/// This function may use a different approximation across different versions, platforms and configurations.
	/// i.e, it can return a different value given the same input on different machines or operating systems, or if SDL is updated.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_atan">SDL_atan</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl), typeof(CallConvSuppressGCTransition)])]
	internal static partial double SDL_atan(double x);

	/// <summary>
	/// Compute the arc tangent of <c><paramref name="y"/> / <paramref name="x"/></c>, using the signs of x and y to adjust the result's quadrant
	/// </summary>
	/// <param name="y">A floating point value of the numerator (y coordinate)</param>
	/// <param name="x">A floating point value of the denominator (x coordinate)</param>
	/// <returns>Returns arc tangent of <c><paramref name="y"/> / <paramref name="x"/></c> in radians, or, if <c>x = 0</c>, either <c>-Pi/2</c>, <c>0</c>, or <c>Pi/2</c>, depending on the value of <c><paramref name="y"/></c></returns>
	/// <remarks>
	/// <para>
	/// The definition of <c>z = atan2(x, y)</c> is <c>y = x tan(z)</c>.
	/// </para>
	/// <para>
	/// Domain: <c>-INF &lt;= x &lt;= INF</c>, <c>-INF &lt;= y &lt;= INF</c>
	/// </para>
	/// <para>
	/// Range: <c>-Pi &lt;= z &lt;= Pi</c>
	/// </para>
	/// <para>
	/// This function operates on double-precision floating point values, use <see href="https://wiki.libsdl.org/SDL3/SDL_atan2f">SDL_atan2f</see> for single-precision floats.
	/// </para>
	/// <para>
	/// To calculate the arc tangent of y / x, use <see href="https://wiki.libsdl.org/SDL3/SDL_atan">SDL_atan</see>.
	/// </para>
	/// <para>
	/// This function may use a different approximation across different versions, platforms and configurations.
	/// i.e, it can return a different value given the same input on different machines or operating systems, or if SDL is updated.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_atan">SDL_atan</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl), typeof(CallConvSuppressGCTransition)])]
	internal static partial double SDL_atan2(double y, double x);

	/// <summary>
	/// Compute the arc tangent of <c><paramref name="y"/> / <paramref name="x"/></c>, using the signs of x and y to adjust the result's quadrant
	/// </summary>
	/// <param name="y">A floating point value of the numerator (y coordinate)</param>
	/// <param name="x">A floating point value of the denominator (x coordinate)</param>
	/// <returns>Returns arc tangent of <c><paramref name="y"/> / <paramref name="x"/></c> in radians, or, if <c>x = 0</c>, either <c>-Pi/2</c>, <c>0</c>, or <c>Pi/2</c>, depending on the value of <c><paramref name="y"/></c></returns>
	/// <remarks>
	/// <para>
	/// The definition of <c>z = atan2(x, y)</c> is <c>y = x tan(z)</c>.
	/// </para>
	/// <para>
	/// Domain: <c>-INF &lt;= x &lt;= INF</c>, <c>-INF &lt;= y &lt;= INF</c>
	/// </para>
	/// <para>
	/// Range: <c>-Pi &lt;= z &lt;= Pi</c>
	/// </para>
	/// <para>
	/// This function operates on single-precision floating point values, use <see href="https://wiki.libsdl.org/SDL3/SDL_atan2">SDL_atan2</see> for double-precision floats.
	/// </para>
	/// <para>
	/// To calculate the arc tangent of y / x, use <see href="https://wiki.libsdl.org/SDL3/SDL_atanf">SDL_atanf</see>.
	/// </para>
	/// <para>
	/// This function may use a different approximation across different versions, platforms and configurations.
	/// i.e, it can return a different value given the same input on different machines or operating systems, or if SDL is updated.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_atan">SDL_atan</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl), typeof(CallConvSuppressGCTransition)])]
	internal static partial float SDL_atan2f(float y, float x);

	/// <summary>
	/// Compute the arc tangent of <c><paramref name="x"/></c>
	/// </summary>
	/// <param name="x">A floating point value</param>
	/// <returns>Returns arc tangent of <c><paramref name="x"/></c> in radians, or 0 if <c>x = 0</c></returns>
	/// <remarks>
	/// <para>
	/// The definition of <c>y = atan(x)</c> is <c>x = tan(y)</c>.
	/// </para>
	/// <para>
	/// Domain: <c>-INF &lt;= x &lt;= INF</c>
	/// </para>
	/// <para>
	/// Range: <c>-Pi/2 &lt;= y &lt;= Pi/2</c>
	/// </para>
	/// <para>
	/// This function operates on single-precision floating point values, use <see href="https://wiki.libsdl.org/SDL3/SDL_atan">SDL_atan</see> for double-precision floats.
	/// </para>
	/// <para>
	/// To calculate the arc tangent of y / x, use <see href="https://wiki.libsdl.org/SDL3/SDL_atan2f">SDL_atan2f</see>.
	/// </para>
	/// <para>
	/// This function may use a different approximation across different versions, platforms and configurations.
	/// i.e, it can return a different value given the same input on different machines or operating systems, or if SDL is updated.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_atanf">SDL_atanf</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl), typeof(CallConvSuppressGCTransition)])]
	internal static partial float SDL_atanf(float x);

	/// <summary>
	/// Compute the ceiling of <c><paramref name="x"/></c>
	/// </summary>
	/// <param name="x">A floating point value</param>
	/// <returns>Returns the ceiling of <c><paramref name="x"/></c></returns>
	/// <remarks> 
	/// <para>
	/// The ceiling of <c><paramref name="x"/></c> is the smallest integer <c>y</c> such that <c>y &gt;= x</c>, i.e <c>x</c> rounded up to the nearest integer.
	/// </para>
	/// <para>
	/// Domain: <c>-INF &lt;= x &lt;= INF</c>
	/// </para>
	/// <para>
	/// Range: <c>-INF &lt;= y &lt;= INF</c>, y integer
	/// </para>
	/// <para>
	/// This function operates on double-precision floating point values, use <see href="https://wiki.libsdl.org/SDL3/SDL_ceilf">SDL_ceilf</see> for single-precision floats.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_ceil">SDL_ceil</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl), typeof(CallConvSuppressGCTransition)])]
	internal static partial double SDL_ceil(double x);

	/// <summary>
	/// Compute the ceiling of <c><paramref name="x"/></c>
	/// </summary>
	/// <param name="x">A floating point value</param>
	/// <returns>Returns the ceiling of <c><paramref name="x"/></c></returns>
	/// <remarks> 
	/// <para>
	/// The ceiling of <c><paramref name="x"/></c> is the smallest integer <c>y</c> such that <c>y &gt;= x</c>, i.e <c>x</c> rounded up to the nearest integer.
	/// </para>
	/// <para>
	/// Domain: <c>-INF &lt;= x &lt;= INF</c>
	/// </para>
	/// <para>
	/// Range: <c>-INF &lt;= y &lt;= INF</c>, y integer
	/// </para>
	/// <para>
	/// This function operates on single-precision floating point values, use <see href="https://wiki.libsdl.org/SDL3/SDL_ceil">SDL_ceil</see> for double-precision floats.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_ceilf">SDL_ceilf</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl), typeof(CallConvSuppressGCTransition)])]
	internal static partial float SDL_ceilf(float x);

	/// <summary>
	/// Copy the sign of one floating-point value to another
	/// </summary>
	/// <param name="x">A floating point value to use as the magnitude</param>
	/// <param name="y">A floating point value to use as the sign</param>
	/// <returns>Returns the floating point value with the sign of <paramref name="y"/> and the magnitude of <paramref name="x"/></returns>
	/// <remarks>
	/// <para>
	/// The definition of copysign is that <c>copysign(x, y) = abs(x) * sign(y)</c>.
	/// </para>
	/// <para>
	/// Domain: <c>-INF &lt;= x &lt;= INF</c>, <c>-INF &lt;= y &lt;= INF</c>
	/// </para>
	/// <para>
	/// Range: <c>-Pi &lt;= z &lt;= Pi</c>
	/// </para>
	/// <para>
	/// This function operates on double-precision floating point values, use <see href="https://wiki.libsdl.org/SDL3/SDL_copysignf">SDL_copysignf</see> for single-precision floats.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_copysign">SDL_copysign</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl), typeof(CallConvSuppressGCTransition)])]
	internal static partial double SDL_copysign(double x, double y);

	/// <summary>
	/// Copy the sign of one floating-point value to another
	/// </summary>
	/// <param name="x">A floating point value to use as the magnitude</param>
	/// <param name="y">A floating point value to use as the sign</param>
	/// <returns>Returns the floating point value with the sign of <paramref name="y"/> and the magnitude of <paramref name="x"/></returns>
	/// <remarks>
	/// <para>
	/// The definition of copysign is that <c>copysign(x, y) = abs(x) * sign(y)</c>.
	/// </para>
	/// <para>
	/// Domain: <c>-INF &lt;= x &lt;= INF</c>, <c>-INF &lt;= y &lt;= INF</c>
	/// </para>
	/// <para>
	/// Range: <c>-Pi &lt;= z &lt;= Pi</c>
	/// </para>
	/// <para>
	/// This function operates on single-precision floating point values, use <see href="https://wiki.libsdl.org/SDL3/SDL_copysign">SDL_copysign</see> for double-precision floats.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_copysignf">SDL_copysignf</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl), typeof(CallConvSuppressGCTransition)])]
	internal static partial float SDL_copysignf(float x, float y);

	/// <summary>
	/// Compute the cosine of <c><paramref name="x"/></c>
	/// </summary>
	/// <param name="x">A foating point value, in radians</param>
	/// <returns>Returns cosine of <c><paramref name="x"/></c></returns>
	/// <remarks>
	/// <para>
	/// Domain: <c>-INF &lt;= x &lt;= INF</c>
	/// </para>
	/// <para>
	/// Range: <c>-1 &lt;= y &lt;= 1</c>
	/// </para>
	/// <para>
	/// This function operates on double-precision floating point values, use <see href="https://wiki.libsdl.org/SDL3/SDL_cosf">SDL_cosf</see> for single-precision floats.
	/// </para> 
	/// <para>
	/// This function may use a different approximation across different versions, platforms and configurations.
	/// i.e, it can return a different value given the same input on different machines or operating systems, or if SDL is updated.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_cos">SDL_cos</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl), typeof(CallConvSuppressGCTransition)])]
	internal static partial double SDL_cos(double x);

	/// <summary>
	/// Compute the cosine of <c><paramref name="x"/></c>
	/// </summary>
	/// <param name="x">A foating point value, in radians</param>
	/// <returns>Returns cosine of <c><paramref name="x"/></c></returns>
	/// <remarks>
	/// <para>
	/// Domain: <c>-INF &lt;= x &lt;= INF</c>
	/// </para>
	/// <para>
	/// Range: <c>-1 &lt;= y &lt;= 1</c>
	/// </para>
	/// <para>
	/// This function operates on single-precision floating point values, use <see href="https://wiki.libsdl.org/SDL3/SDL_cos">SDL_cos</see> for double-precision floats.
	/// </para> 
	/// <para>
	/// This function may use a different approximation across different versions, platforms and configurations.
	/// i.e, it can return a different value given the same input on different machines or operating systems, or if SDL is updated.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_cosf">SDL_cosf</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl), typeof(CallConvSuppressGCTransition)])]
	internal static partial float SDL_cosf(float x);

	/// <summary>
	/// Compute the exponential of <c><paramref name="x"/></c>
	/// </summary>
	/// <param name="x">A foating point value</param>
	/// <returns>Returns value of <c>e^<paramref name="x"/></c></returns>
	/// <remarks>
	/// <para>
	/// The definition of <c>y = exp(x)</c> is <c>y = e^x</c>, where <c>e</c> is the base of the natural logarithm. The inverse is the natural logarithm, <see href="https://wiki.libsdl.org/SDL3/SDL_log">SDL_log</see>.
	/// </para>
	/// <para>
	/// Domain: <c>-INF &lt;= x &lt;= INF</c>
	/// </para>
	/// <para>
	/// Range: <c>0 &lt;= y &lt;= INF</c>
	/// </para>
	/// <para>
	/// The output will overflow if <c>exp(<paramref name="x"/>)</c> is too large to be represented.
	/// </para>
	/// <para>
	/// This function operates on double-precision floating point values, use <see href="https://wiki.libsdl.org/SDL3/SDL_expf">SDL_expf</see> for single-precision floats.
	/// </para> 
	/// <para>
	/// This function may use a different approximation across different versions, platforms and configurations.
	/// i.e, it can return a different value given the same input on different machines or operating systems, or if SDL is updated.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_exp">SDL_exp</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl), typeof(CallConvSuppressGCTransition)])]
	internal static partial double SDL_exp(double x);

	/// <summary>
	/// Compute the exponential of <c><paramref name="x"/></c>
	/// </summary>
	/// <param name="x">A foating point value</param>
	/// <returns>Returns value of <c>e^<paramref name="x"/></c></returns>
	/// <remarks>
	/// <para>
	/// The definition of <c>y = exp(x)</c> is <c>y = e^x</c>, where <c>e</c> is the base of the natural logarithm. The inverse is the natural logarithm, <see href="https://wiki.libsdl.org/SDL3/SDL_logf">SDL_logf</see>.
	/// </para>
	/// <para>
	/// Domain: <c>-INF &lt;= x &lt;= INF</c>
	/// </para>
	/// <para>
	/// Range: <c>0 &lt;= y &lt;= INF</c>
	/// </para>
	/// <para>
	/// The output will overflow if <c>exp(<paramref name="x"/>)</c> is too large to be represented.
	/// </para>
	/// <para>
	/// This function operates on single-precision floating point values, use <see href="https://wiki.libsdl.org/SDL3/SDL_exp">SDL_exp</see> for double-precision floats.
	/// </para> 
	/// <para>
	/// This function may use a different approximation across different versions, platforms and configurations.
	/// i.e, it can return a different value given the same input on different machines or operating systems, or if SDL is updated.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_expf">SDL_expf</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl), typeof(CallConvSuppressGCTransition)])]
	internal static partial float SDL_expf(float x);

	/// <summary>
	/// Compute the absolute value of <c><paramref name="x"/></c>
	/// </summary>
	/// <param name="x">A floating point value to use as the magnitude</param>
	/// <returns>Returns the absolute value of <c><paramref name="x"/></c></returns>
	/// <remarks>
	/// <para>
	/// Domain: <c>-INF &lt;= x &lt;= INF</c>
	/// </para>
	/// <para>
	/// Range: <c>0 &lt;= y &lt;= INF</c>
	/// </para>
	/// <para>
	/// This function operates on double-precision floating point values, use <see href="https://wiki.libsdl.org/SDL3/SDL_fabsf">SDL_fabsf</see> for single-precision floats.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_fabs">SDL_fabs</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl), typeof(CallConvSuppressGCTransition)])]
	internal static partial double SDL_fabs(double x);

	/// <summary>
	/// Compute the absolute value of <c><paramref name="x"/></c>
	/// </summary>
	/// <param name="x">A floating point value to use as the magnitude</param>
	/// <returns>Returns the absolute value of <c><paramref name="x"/></c></returns>
	/// <remarks>
	/// <para>
	/// Domain: <c>-INF &lt;= x &lt;= INF</c>
	/// </para>
	/// <para>
	/// Range: <c>0 &lt;= y &lt;= INF</c>
	/// </para>
	/// <para>
	/// This function operates on single-precision floating point values, use <see href="https://wiki.libsdl.org/SDL3/SDL_fabs">SDL_fabs</see> for double-precision floats.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_fabsf">SDL_fabsf</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl), typeof(CallConvSuppressGCTransition)])]
	internal static partial float SDL_fabsf(float x);

	/// <summary>
	/// Compute the floor of <c><paramref name="x"/></c>
	/// </summary>
	/// <param name="x">A floating point value</param>
	/// <returns>Returns the floor of <c><paramref name="x"/></c></returns>
	/// <remarks>
	/// <para>
	/// The floor of <c><paramref name="x"/></c> is the largest integer <c>y</c> such that <c>y &lt;= x</c>, i.e <c>x</c> rounded down to the nearest integer.
	/// </para>
	/// <para>
	/// Domain: <c>-INF &lt;= x &lt;= INF</c>
	/// </para>
	/// <para>
	/// Range: <c>-INF &lt;= y &lt;= INF</c>, y integer
	/// </para>
	/// <para>
	/// This function operates on double-precision floating point values, use <see href="https://wiki.libsdl.org/SDL3/SDL_floorf">SDL_floorf</see> for single-precision floats.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_floor">SDL_floor</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl), typeof(CallConvSuppressGCTransition)])]
	internal static partial double SDL_floor(double x);

	/// <summary>
	/// Compute the floor of <c><paramref name="x"/></c>
	/// </summary>
	/// <param name="x">A floating point value</param>
	/// <returns>Returns the floor of <c><paramref name="x"/></c></returns>
	/// <remarks>
	/// <para>
	/// The floor of <c><paramref name="x"/></c> is the largest integer <c>y</c> such that <c>y &lt;= x</c>, i.e <c>x</c> rounded down to the nearest integer.
	/// </para>
	/// <para>
	/// Domain: <c>-INF &lt;= x &lt;= INF</c>
	/// </para>
	/// <para>
	/// Range: <c>-INF &lt;= y &lt;= INF</c>, y integer
	/// </para>
	/// <para>
	/// This function operates on single-precision floating point values, use <see href="https://wiki.libsdl.org/SDL3/SDL_floor">SDL_floor</see> for double-precision floats.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_floorf">SDL_floorf</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl), typeof(CallConvSuppressGCTransition)])]
	internal static partial float SDL_floorf(float x);

	/// <summary>
	/// Return the floating-point remainder of <c><paramref name="x"/> / <paramref name="y"/></c>
	/// </summary>
	/// <param name="x">The numerator</param>
	/// <param name="y">The denominator. Must not be 0.</param>
	/// <returns>Returns the remainder of <c><paramref name="x"/> / <paramref name="y"/></c></returns>
	/// <remarks>
	/// <para>
	/// Divides <c><paramref name="x"/></c> by <c><paramref name="y"/></c>, and returns the remainder.
	/// </para>
	/// <para>
	/// Domain: <c>-INF &lt;= x &lt;= INF</c>, <c>-INF &lt;= y &lt;= INF</c>, <c>y != 0</c>
	/// </para>
	/// <para>
	/// Range: <c>-y &lt;= z &lt;= y</c>
	/// </para>
	/// <para>
	/// This function operates on double-precision floating point values, use <see href="https://wiki.libsdl.org/SDL3/SDL_fmodf">SDL_fmodf</see> for single-precision floats.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_fmod">SDL_fmod</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl), typeof(CallConvSuppressGCTransition)])]
	internal static partial double SDL_fmod(double x, double y);

	/// <summary>
	/// Return the floating-point remainder of <c><paramref name="x"/> / <paramref name="y"/></c>
	/// </summary>
	/// <param name="x">The numerator</param>
	/// <param name="y">The denominator. Must not be 0.</param>
	/// <returns>Returns the remainder of <c><paramref name="x"/> / <paramref name="y"/></c></returns>
	/// <remarks>
	/// <para>
	/// Divides <c><paramref name="x"/></c> by <c><paramref name="y"/></c>, and returns the remainder.
	/// </para>
	/// <para>
	/// Domain: <c>-INF &lt;= x &lt;= INF</c>, <c>-INF &lt;= y &lt;= INF</c>, <c>y != 0</c>
	/// </para>
	/// <para>
	/// Range: <c>-y &lt;= z &lt;= y</c>
	/// </para>
	/// <para>
	/// This function operates on single-precision floating point values, use <see href="https://wiki.libsdl.org/SDL3/SDL_fmod">SDL_fmod</see> for double-precision floats.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_fmodf"></seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl), typeof(CallConvSuppressGCTransition)])]
	internal static partial float SDL_fmodf(float x, float y);

	/// <summary>
	/// Return whether the value is infinity
	/// </summary>
	/// <param name="x">A double-precision floating point value</param>
	/// <returns>Returns non-zero if the value is infinity, 0 otherwise</returns>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_isinf">SDL_isinf</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl), typeof(CallConvSuppressGCTransition)])]
	internal static partial int SDL_isinf(double x);

	/// <summary>
	/// Return whether the value is infinity
	/// </summary>
	/// <param name="x">A floating point value</param>
	/// <returns>Returns non-zero if the value is infinity, 0 otherwise</returns>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_isinff">SDL_isinff</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl), typeof(CallConvSuppressGCTransition)])]
	internal static partial int SDL_isinff(float x);

	/// <summary>
	/// Return whether the value is NaN
	/// </summary>
	/// <param name="x">A double-precision floating point value</param>
	/// <returns>Returns non-zero if the value is NaN, 0 otherwise</returns>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_isnan">SDL_isnan</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl), typeof(CallConvSuppressGCTransition)])]
	internal static partial int SDL_isnan(double x);

	/// <summary>
	/// Return whether the value is NaN
	/// </summary>
	/// <param name="x">A floating point value</param>
	/// <returns>Returns non-zero if the value is NaN, 0 otherwise</returns>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_isnanf">SDL_isnanf</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl), typeof(CallConvSuppressGCTransition)])]
	internal static partial int SDL_isnanf(float x);

	/// <summary>
	/// Compute the natural logarithm of <c><paramref name="x"/></c>
	/// </summary>
	/// <param name="x">A floating point value. Must be greater than 0.</param>
	/// <returns>Returns the natural logarithm of <c><paramref name="x"/></c></returns>
	/// <remarks> 
	/// <para>
	/// Domain: <c>0 &lt; x &lt;= INF</c>
	/// </para>
	/// <para>
	/// Range: <c>-INF &lt;= y &lt;= INF</c>
	/// </para>
	/// <para>
	/// It is an error for <c><paramref name="x"/></c> to be less than or equal to 0.
	/// </para>
	/// <para>
	/// This function operates on double-precision floating point values, use <see href="https://wiki.libsdl.org/SDL3/SDL_logf">SDL_logf</see> for single-precision floats.
	/// </para> 
	/// <para>
	/// This function may use a different approximation across different versions, platforms and configurations.
	/// i.e, it can return a different value given the same input on different machines or operating systems, or if SDL is updated.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_log">SDL_log</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl), typeof(CallConvSuppressGCTransition)])]
	internal static partial double SDL_log(double x);

	/// <summary>
	/// Compute the base-10 logarithm of <c><paramref name="x"/></c>
	/// </summary>
	/// <param name="x">A floating point value. Must be greater than 0.</param>
	/// <returns>Returns the logarithm of <c><paramref name="x"/></c></returns>
	/// <remarks>
	/// <para>
	/// Domain: <c>0 &lt; x &lt;= INF</c>
	/// </para>
	/// <para>
	/// Range: <c>-INF &lt;= y &lt;= INF</c>
	/// </para>
	/// <para>
	/// It is an error for <c><paramref name="x"/></c> to be less than or equal to 0.
	/// </para>
	/// <para>
	/// This function operates on double-precision floating point values, use <see href="https://wiki.libsdl.org/SDL3/SDL_log10f">SDL_log10f</see> for single-precision floats.
	/// </para> 
	/// <para>
	/// This function may use a different approximation across different versions, platforms and configurations.
	/// i.e, it can return a different value given the same input on different machines or operating systems, or if SDL is updated.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_log10">SDL_log10</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl), typeof(CallConvSuppressGCTransition)])]
	internal static partial double SDL_log10(double x);

	/// <summary>
	/// Compute the base-10 logarithm of <c><paramref name="x"/></c>
	/// </summary>
	/// <param name="x">A floating point value. Must be greater than 0.</param>
	/// <returns>Returns the logarithm of <c><paramref name="x"/></c></returns>
	/// <remarks>
	/// <para>
	/// Domain: <c>0 &lt; x &lt;= INF</c>
	/// </para>
	/// <para>
	/// Range: <c>-INF &lt;= y &lt;= INF</c>
	/// </para>
	/// <para>
	/// It is an error for <c><paramref name="x"/></c> to be less than or equal to 0.
	/// </para>
	/// <para>
	/// This function operates on single-precision floating point values, use <see href="https://wiki.libsdl.org/SDL3/SDL_log10">SDL_log10</see> for double-precision floats.
	/// </para> 
	/// <para>
	/// This function may use a different approximation across different versions, platforms and configurations.
	/// i.e, it can return a different value given the same input on different machines or operating systems, or if SDL is updated.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_log10f">SDL_log10f</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl), typeof(CallConvSuppressGCTransition)])]
	internal static partial float SDL_log10f(float x);

	/// <summary>
	/// Compute the natural logarithm of <c><paramref name="x"/></c>
	/// </summary>
	/// <param name="x">A floating point value. Must be greater than 0.</param>
	/// <returns>Returns the natural logarithm of <c><paramref name="x"/></c></returns>
	/// <remarks> 
	/// <para>
	/// Domain: <c>0 &lt; x &lt;= INF</c>
	/// </para>
	/// <para>
	/// Range: <c>-INF &lt;= y &lt;= INF</c>
	/// </para>
	/// <para>
	/// It is an error for <c><paramref name="x"/></c> to be less than or equal to 0.
	/// </para>
	/// <para>
	/// This function operates on single-precision floating point values, use <see href="https://wiki.libsdl.org/SDL3/SDL_log">SDL_log</see> for double-precision floats.
	/// </para> 
	/// <para>
	/// This function may use a different approximation across different versions, platforms and configurations.
	/// i.e, it can return a different value given the same input on different machines or operating systems, or if SDL is updated.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_logf">SDL_logf</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl), typeof(CallConvSuppressGCTransition)])]
	internal static partial float SDL_logf(float x);

	/* NOTE:
	 * The C 'long' (a.k.a. 'long int') datatype is notoriously difficult to handle in imported 'cdecl' function signatures.
	 * Windows uses the so called LLP64 data model where a 'long' is a 32-bit integer,
	 * while most of the other noticable platforms use the LP64 data model where a 'long' is a 64-bit integer.
	 * Therefore, the following 2 function imports are separated into a *_LP64 and a *_LLP64 counterpart.
	 * Both of those method declarations import the same function, but from different platforms with a different signature adjusted for their data model.
	 * 
	 * WARNING: Do NOT call the *_LP64 variant from a LLP64 platform and vice-versa!
	 * 
	 * See also:
	 * - https://en.wikipedia.org/wiki/64-bit_computing#64-bit_data_models
	 * - https://de.wikipedia.org/wiki/Datentypen_in_C#Datenmodell
	 */

	/// <summary>
	/// Round <c><paramref name="x"/></c> to the nearest integer representable as a long
	/// </summary>
	/// <param name="x">A floating point value</param>
	/// <returns>Returns the nearest integer to <c><paramref name="x"/></c></returns>
	/// <remarks>
	/// <para>
	/// Rounds <c><paramref name="x"/></c> to the nearest integer. Values halfway between integers will be rounded away from zero.
	/// </para>
	/// <para>
	/// Domain: <c>-INF &lt;= x &lt;= INF</c>
	/// </para>
	/// <para>
	/// Range: <c>MIN_LONG &lt;= y &lt;= MAX_LONG</c>
	/// </para>
	/// <para>
	/// This function operates on double-precision floating point values, use <see href="https://wiki.libsdl.org/SDL3/SDL_lroundf">SDL_lroundf</see> for single-precision floats.
	/// To get the result as a floating-point type, use <see href="https://wiki.libsdl.org/SDL3/SDL_round">SDL_round</see>.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_lround">SDL_lround</seealso>
	[NativeImportFunction<Library, Not<IsLLP64>>("SDL_lround", CallConvs = [typeof(CallConvCdecl), typeof(CallConvSuppressGCTransition)])]
	internal static partial long SDL_lround_LP64(double x);

	/// <summary>
	/// Round <c><paramref name="x"/></c> to the nearest integer representable as a long
	/// </summary>
	/// <param name="x">A floating point value</param>
	/// <returns>Returns the nearest integer to <c><paramref name="x"/></c></returns>
	/// <remarks>
	/// <para>
	/// Rounds <c><paramref name="x"/></c> to the nearest integer. Values halfway between integers will be rounded away from zero.
	/// </para>
	/// <para>
	/// Domain: <c>-INF &lt;= x &lt;= INF</c>
	/// </para>
	/// <para>
	/// Range: <c>MIN_LONG &lt;= y &lt;= MAX_LONG</c>
	/// </para>
	/// <para>
	/// This function operates on double-precision floating point values, use <see href="https://wiki.libsdl.org/SDL3/SDL_lroundf">SDL_lroundf</see> for single-precision floats.
	/// To get the result as a floating-point type, use <see href="https://wiki.libsdl.org/SDL3/SDL_round">SDL_round</see>.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_lround">SDL_lround</seealso>
	[NativeImportFunction<Library, IsLLP64>("SDL_lround", CallConvs = [typeof(CallConvCdecl), typeof(CallConvSuppressGCTransition)])]
	internal static partial int SDL_lround_LLP64(double x);

	/* NOTE:
	 * The C 'long' (a.k.a. 'long int') datatype is notoriously difficult to handle in imported 'cdecl' function signatures.
	 * Windows uses the so called LLP64 data model where a 'long' is a 32-bit integer,
	 * while most of the other noticable platforms use the LP64 data model where a 'long' is a 64-bit integer.
	 * Therefore, the following 2 function imports are separated into a *_LP64 and a *_LLP64 counterpart.
	 * Both of those method declarations import the same function, but from different platforms with a different signature adjusted for their data model.
	 * 
	 * WARNING: Do NOT call the *_LP64 variant from a LLP64 platform and vice-versa!
	 * 
	 * See also:
	 * - https://en.wikipedia.org/wiki/64-bit_computing#64-bit_data_models
	 * - https://de.wikipedia.org/wiki/Datentypen_in_C#Datenmodell
	 */

	/// <summary>
	/// Round <c><paramref name="x"/></c> to the nearest integer representable as a long
	/// </summary>
	/// <param name="x">A floating point value</param>
	/// <returns>Returns the nearest integer to <c><paramref name="x"/></c></returns>
	/// <remarks>
	/// <para>
	/// Rounds <c><paramref name="x"/></c> to the nearest integer. Values halfway between integers will be rounded away from zero.
	/// </para>
	/// <para>
	/// Domain: <c>-INF &lt;= x &lt;= INF</c>
	/// </para>
	/// <para>
	/// Range: <c>MIN_LONG &lt;= y &lt;= MAX_LONG</c>
	/// </para>
	/// <para>
	/// This function operates on single-precision floating point values, use <see href="https://wiki.libsdl.org/SDL3/SDL_lround">SDL_lround</see> for double-precision floats.
	/// To get the result as a floating-point type, use <see href="https://wiki.libsdl.org/SDL3/SDL_roundf">SDL_roundf</see>.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_lroundf">SDL_lroundf</seealso>
	[NativeImportFunction<Library, Not<IsLLP64>>("SDL_lroundf", CallConvs = [typeof(CallConvCdecl), typeof(CallConvSuppressGCTransition)])]
	internal static partial long SDL_lroundf_LP64(float x);

	/// <summary>
	/// Round <c><paramref name="x"/></c> to the nearest integer representable as a long
	/// </summary>
	/// <param name="x">A floating point value</param>
	/// <returns>Returns the nearest integer to <c><paramref name="x"/></c></returns>
	/// <remarks>
	/// <para>
	/// Rounds <c><paramref name="x"/></c> to the nearest integer. Values halfway between integers will be rounded away from zero.
	/// </para>
	/// <para>
	/// Domain: <c>-INF &lt;= x &lt;= INF</c>
	/// </para>
	/// <para>
	/// Range: <c>MIN_LONG &lt;= y &lt;= MAX_LONG</c>
	/// </para>
	/// <para>
	/// This function operates on single-precision floating point values, use <see href="https://wiki.libsdl.org/SDL3/SDL_lround">SDL_lround</see> for double-precision floats.
	/// To get the result as a floating-point type, use <see href="https://wiki.libsdl.org/SDL3/SDL_roundf">SDL_roundf</see>.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_lroundf">SDL_lroundf</seealso>
	[NativeImportFunction<Library, IsLLP64>("SDL_lroundf", CallConvs = [typeof(CallConvCdecl), typeof(CallConvSuppressGCTransition)])]
	internal static partial int SDL_lroundf_LLP64(float x);

	/// <summary>
	/// Split <c><paramref name="x"/></c> into integer and fractional parts
	/// </summary>
	/// <param name="x">A floating point value</param>
	/// <param name="y">An output pointer to store the integer part of <c>x</c></param> 
	/// <returns>Returns the fractional part of <c>x</c></returns>
	/// <remarks>
	/// <para>
	/// This function operates on double-precision floating point values, use <see href="https://wiki.libsdl.org/SDL3/SDL_modff">SDL_modff</see> for single-precision floats.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_modf">SDL_modf</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl), typeof(CallConvSuppressGCTransition)])]
	internal unsafe static partial double SDL_modf(double x, double* y);

	/// <summary>
	/// Split <c><paramref name="x"/></c> into integer and fractional parts
	/// </summary>
	/// <param name="x">A floating point value</param>
	/// <param name="y">An output pointer to store the integer part of <c>x</c></param> 
	/// <returns>Returns the fractional part of <c>x</c></returns>
	/// <remarks>
	/// <para>
	/// This function operates on double-precision floating point values, use <see href="https://wiki.libsdl.org/SDL3/SDL_modf">SDL_modf</see> for single-precision floats.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_modff">SDL_modff</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl), typeof(CallConvSuppressGCTransition)])]
	internal unsafe static partial float SDL_modff(float x, float* y);

	/// <summary>
	/// Raise <c><paramref name="x"/></c> to the power <c><paramref name="y"/></c>
	/// </summary>
	/// <param name="x">The base</param>
	/// <param name="y">The exponent</param>
	/// <returns>Returns <c><paramref name="x"/></c> raised to the power <c><paramref name="y"/></c></returns>
	/// <remarks>
	/// <para>
	/// Domain: <c>-INF &lt;= x &lt;= INF</c>, <c>-INF &lt;= y &lt;= INF</c>
	/// </para>
	/// <para>
	/// Range: <c>-INF &lt;= z &lt;= INF</c>
	/// </para>
	/// <para>
	/// If <c>y</c> is the base of the natural logarithm (e), consider using <see href="https://wiki.libsdl.org/SDL3/SDL_exp">SDL_exp</see> instead.
	/// </para>
	/// <para>
	/// This function operates on double-precision floating point values, use <see href="https://wiki.libsdl.org/SDL3/SDL_powf">SDL_powf</see> for single-precision floats.
	/// </para> 
	/// <para>
	/// This function may use a different approximation across different versions, platforms and configurations.
	/// i.e, it can return a different value given the same input on different machines or operating systems, or if SDL is updated.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_pow">SDL_pow</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl), typeof(CallConvSuppressGCTransition)])]
	internal static partial double SDL_pow(double x, double y);

	/// <summary>
	/// Raise <c><paramref name="x"/></c> to the power <c><paramref name="y"/></c>
	/// </summary>
	/// <param name="x">The base</param>
	/// <param name="y">The exponent</param>
	/// <returns>Returns <c><paramref name="x"/></c> raised to the power <c><paramref name="y"/></c></returns>
	/// <remarks>
	/// <para>
	/// Domain: <c>-INF &lt;= x &lt;= INF</c>, <c>-INF &lt;= y &lt;= INF</c>
	/// </para>
	/// <para>
	/// Range: <c>-INF &lt;= z &lt;= INF</c>
	/// </para>
	/// <para>
	/// If <c>y</c> is the base of the natural logarithm (e), consider using <see href="https://wiki.libsdl.org/SDL3/SDL_expf">SDL_expf</see> instead.
	/// </para>
	/// <para>
	/// This function operates on single-precision floating point values, use <see href="https://wiki.libsdl.org/SDL3/SDL_pow">SDL_pow</see> for double-precision floats.
	/// </para> 
	/// <para>
	/// This function may use a different approximation across different versions, platforms and configurations.
	/// i.e, it can return a different value given the same input on different machines or operating systems, or if SDL is updated.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_powf">SDL_powf</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl), typeof(CallConvSuppressGCTransition)])]
	internal static partial float SDL_powf(float x, float y);

	/// <summary>
	/// Round <c><paramref name="x"/></c> to the nearest integer
	/// </summary>
	/// <param name="x">A floating point value</param>
	/// <returns>Returns the nearest integer to <c><paramref name="x"/></c></returns>
	/// <remarks>
	/// <para>
	/// Rounds <c><paramref name="x"/></c> to the nearest integer. Values halfway between integers will be rounded away from zero.
	/// </para>
	/// <para>
	/// Domain: <c>-INF &lt;= x &lt;= INF</c>
	/// </para>
	/// <para>
	/// Range: <c>-INF &lt;= y &lt;= INF</c>, y integer
	/// </para>
	/// <para>
	/// This function operates on double-precision floating point values, use <see href="https://wiki.libsdl.org/SDL3/SDL_roundf">SDL_roundf</see> for single-precision floats.
	/// To get the result as an integer type, use <see href="https://wiki.libsdl.org/SDL3/SDL_lround">SDL_lround</see>.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_round">SDL_round</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl), typeof(CallConvSuppressGCTransition)])]
	internal static partial double SDL_round(double x);

	/// <summary>
	/// Round <c><paramref name="x"/></c> to the nearest integer
	/// </summary>
	/// <param name="x">A floating point value</param>
	/// <returns>Returns the nearest integer to <c><paramref name="x"/></c></returns>
	/// <remarks>
	/// <para>
	/// Rounds <c><paramref name="x"/></c> to the nearest integer. Values halfway between integers will be rounded away from zero.
	/// </para>
	/// <para>
	/// Domain: <c>-INF &lt;= x &lt;= INF</c>
	/// </para>
	/// <para>
	/// Range: <c>-INF &lt;= y &lt;= INF</c>, y integer
	/// </para>
	/// <para>
	/// This function operates on single-precision floating point values, use <see href="https://wiki.libsdl.org/SDL3/SDL_round">SDL_round</see> for double-precision floats.
	/// To get the result as an integer type, use <see href="https://wiki.libsdl.org/SDL3/SDL_lroundf">SDL_lroundf</see>.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_roundf">SDL_roundf</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl), typeof(CallConvSuppressGCTransition)])]
	internal static partial float SDL_roundf(float x);

	/// <summary>
	/// Scale <c><paramref name="x"/></c> by an integer power of two
	/// </summary>
	/// <param name="x">A floating point value to be scaled</param>
	/// <param name="n">An integer exponent</param>
	/// <returns>Returns <c><paramref name="x"/> * 2^<paramref name="n"/></c></returns>
	/// <remarks>
	/// <para>
	/// Multiplies <c><paramref name="x"/></c> by the <c><paramref name="n"/></c>th power of the floating point radix (always 2).
	/// </para>
	/// <para>
	/// Domain: <c>-INF &lt;= x &lt;= INF</c>, <c>n</c> integer
	/// </para>
	/// <para>
	/// Range: <c>-INF &lt;= y &lt;= INF</c>
	/// </para>
	/// <para>
	/// This function operates on double-precision floating point values, use <see href="https://wiki.libsdl.org/SDL3/SDL_scalbnf">SDL_scalbnf</see> for single-precision floats.
	/// </para> 
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_scalbn">SDL_scalbn</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl), typeof(CallConvSuppressGCTransition)])]
	internal static partial double SDL_scalbn(double x, int n);

	/// <summary>
	/// Scale <c><paramref name="x"/></c> by an integer power of two
	/// </summary>
	/// <param name="x">A floating point value to be scaled</param>
	/// <param name="n">An integer exponent</param>
	/// <returns>Returns <c><paramref name="x"/> * 2^<paramref name="n"/></c></returns>
	/// <remarks>
	/// <para>
	/// Multiplies <c><paramref name="x"/></c> by the <c><paramref name="n"/></c>th power of the floating point radix (always 2).
	/// </para>
	/// <para>
	/// Domain: <c>-INF &lt;= x &lt;= INF</c>, <c>n</c> integer
	/// </para>
	/// <para>
	/// Range: <c>-INF &lt;= y &lt;= INF</c>
	/// </para>
	/// <para>
	/// This function operates on single-precision floating point values, use <see href="https://wiki.libsdl.org/SDL3/SDL_scalbn">SDL_scalbn</see> for double-precision floats.
	/// </para> 
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_scalbnf">SDL_scalbnf</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl), typeof(CallConvSuppressGCTransition)])]
	internal static partial float SDL_scalbnf(float x, int n);

	/// <summary>
	/// Compute the sine of <c><paramref name="x"/></c>
	/// </summary>
	/// <param name="x">A foating point value, in radians</param>
	/// <returns>Returns sine of <c><paramref name="x"/></c></returns>
	/// <remarks>
	/// <para>
	/// Domain: <c>-INF &lt;= x &lt;= INF</c>
	/// </para>
	/// <para>
	/// Range: <c>-1 &lt;= y &lt;= 1</c>
	/// </para>
	/// <para>
	/// This function operates on double-precision floating point values, use <see href="https://wiki.libsdl.org/SDL3/SDL_sinf">SDL_sinf</see> for single-precision floats.
	/// </para> 
	/// <para>
	/// This function may use a different approximation across different versions, platforms and configurations.
	/// i.e, it can return a different value given the same input on different machines or operating systems, or if SDL is updated.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_sin">SDL_sin</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl), typeof(CallConvSuppressGCTransition)])]
	internal static partial double SDL_sin(double x);

	/// <summary>
	/// Compute the sine of <c><paramref name="x"/></c>
	/// </summary>
	/// <param name="x">A foating point value, in radians</param>
	/// <returns>Returns sine of <c><paramref name="x"/></c></returns>
	/// <remarks>
	/// <para>
	/// Domain: <c>-INF &lt;= x &lt;= INF</c>
	/// </para>
	/// <para>
	/// Range: <c>-1 &lt;= y &lt;= 1</c>
	/// </para>
	/// <para>
	/// This function operates on single-precision floating point values, use <see href="https://wiki.libsdl.org/SDL3/SDL_sin">SDL_sin</see> for double-precision floats.
	/// </para> 
	/// <para>
	/// This function may use a different approximation across different versions, platforms and configurations.
	/// i.e, it can return a different value given the same input on different machines or operating systems, or if SDL is updated.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_sinf">SDL_sinf</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl), typeof(CallConvSuppressGCTransition)])]
	internal static partial float SDL_sinf(float x);

	/// <summary>
	/// Compute the square root of <c><paramref name="x"/></c>
	/// </summary>
	/// <param name="x">A floating point value. Must be greater than or equal to 0.</param>
	/// <returns>Returns square root of <c><paramref name="x"/></c></returns>
	/// <remarks>
	/// <para>
	/// Domain: <c>0 &lt;= x &lt;= INF</c>
	/// </para>
	/// <para>
	/// Range: <c>0 &lt;= y &lt;= 1</c>
	/// </para>
	/// <para>
	/// This function operates on double-precision floating point values, use <see href="https://wiki.libsdl.org/SDL3/SDL_sqrtf">SDL_sqrtf</see> for single-precision floats.
	/// </para> 
	/// <para>
	/// This function may use a different approximation across different versions, platforms and configurations.
	/// i.e, it can return a different value given the same input on different machines or operating systems, or if SDL is updated.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_sqrt">SDL_sqrt</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl), typeof(CallConvSuppressGCTransition)])]
	internal static partial double SDL_sqrt(double x);

	/// <summary>
	/// Compute the square root of <c><paramref name="x"/></c>
	/// </summary>
	/// <param name="x">A floating point value. Must be greater than or equal to 0.</param>
	/// <returns>Returns square root of <c><paramref name="x"/></c></returns>
	/// <remarks>
	/// <para>
	/// Domain: <c>0 &lt;= x &lt;= INF</c>
	/// </para>
	/// <para>
	/// Range: <c>0 &lt;= y &lt;= 1</c>
	/// </para>
	/// <para>
	/// This function operates on single-precision floating point values, use <see href="https://wiki.libsdl.org/SDL3/SDL_sqrt">SDL_sqrt</see> for double-precision floats.
	/// </para> 
	/// <para>
	/// This function may use a different approximation across different versions, platforms and configurations.
	/// i.e, it can return a different value given the same input on different machines or operating systems, or if SDL is updated.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_sqrtf">SDL_sqrtf</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl), typeof(CallConvSuppressGCTransition)])]
	internal static partial float SDL_sqrtf(float x);

	/// <summary>
	/// Compute the tangent of <c><paramref name="x"/></c>
	/// </summary>
	/// <param name="x">A foating point value, in radians</param>
	/// <returns>Returns sine of <c><paramref name="x"/></c></returns>
	/// <remarks>
	/// <para>
	/// Domain: <c>-INF &lt;= x &lt;= INF</c>
	/// </para>
	/// <para>
	/// Range: <c>-INF &lt;= y &lt;= INF</c>
	/// </para>
	/// <para>
	/// This function operates on double-precision floating point values, use <see href="https://wiki.libsdl.org/SDL3/SDL_tanf">SDL_tanf</see> for single-precision floats.
	/// </para> 
	/// <para>
	/// This function may use a different approximation across different versions, platforms and configurations.
	/// i.e, it can return a different value given the same input on different machines or operating systems, or if SDL is updated.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_tan">SDL_tan</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl), typeof(CallConvSuppressGCTransition)])]
	internal static partial double SDL_tan(double x);

	/// <summary>
	/// Compute the tangent of <c><paramref name="x"/></c>
	/// </summary>
	/// <param name="x">A foating point value, in radians</param>
	/// <returns>Returns sine of <c><paramref name="x"/></c></returns>
	/// <remarks>
	/// <para>
	/// Domain: <c>-INF &lt;= x &lt;= INF</c>
	/// </para>
	/// <para>
	/// Range: <c>-INF &lt;= y &lt;= INF</c>
	/// </para>
	/// <para>
	/// This function operates on single-precision floating point values, use <see href="https://wiki.libsdl.org/SDL3/SDL_tan">SDL_tan</see> for double-precision floats.
	/// </para> 
	/// <para>
	/// This function may use a different approximation across different versions, platforms and configurations.
	/// i.e, it can return a different value given the same input on different machines or operating systems, or if SDL is updated.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_tanf">SDL_tanf</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl), typeof(CallConvSuppressGCTransition)])]
	internal static partial float SDL_tanf(float x);

	/// <summary>
	/// Truncate <c><paramref name="x"/></c> to an integer
	/// </summary>
	/// <param name="x">A floating point value</param>
	/// <returns>Returns <c><paramref name="x"/></c> truncated to an integer</returns>
	/// <remarks>
	/// <para>
	/// Rounds <c><paramref name="x"/></c> to the next closest integer to 0. This is equivalent to removing the fractional part of <c><paramref name="x"/></c>, leaving only the integer part.
	/// </para>	/// 
	/// <para>
	/// Domain: <c>-INF &lt;= x &lt;= INF</c>
	/// </para>
	/// <para>
	/// Range: <c>-INF &lt;= y &lt;= INF</c>, y integer
	/// </para>
	/// <para>
	/// This function operates on double-precision floating point values, use <see href="https://wiki.libsdl.org/SDL3/SDL_truncf">SDL_truncf</see> for single-precision floats.
	/// </para> 
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_trunc">SDL_trunc</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl), typeof(CallConvSuppressGCTransition)])]
	internal static partial double SDL_trunc(double x);

	/// <summary>
	/// Truncate <c><paramref name="x"/></c> to an integer
	/// </summary>
	/// <param name="x">A floating point value</param>
	/// <returns>Returns <c><paramref name="x"/></c> truncated to an integer</returns>
	/// <remarks>
	/// <para>
	/// Rounds <c><paramref name="x"/></c> to the next closest integer to 0. This is equivalent to removing the fractional part of <c><paramref name="x"/></c>, leaving only the integer part.
	/// </para>	/// 
	/// <para>
	/// Domain: <c>-INF &lt;= x &lt;= INF</c>
	/// </para>
	/// <para>
	/// Range: <c>-INF &lt;= y &lt;= INF</c>, y integer
	/// </para>
	/// <para>
	/// This function operates on single-precision floating point values, use <see href="https://wiki.libsdl.org/SDL3/SDL_trunc">SDL_trunc</see> for double-precision floats.
	/// </para> 
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_truncf">SDL_truncf</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl), typeof(CallConvSuppressGCTransition)])]
	internal static partial float SDL_truncf(float x);
}
