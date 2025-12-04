using Sdl3Sharp.Internal.Interop;
using Sdl3Sharp.SourceGeneration;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Video.Drawing;

partial class Rect
{
	/// <summary>
	/// Calculate the intersection of a rectangle and line segment
	/// </summary>
	/// <param name="rect">An <see href="https://wiki.libsdl.org/SDL3/SDL_Rect">SDL_Rect</see> structure representing the rectangle to intersect</param>
	/// <param name="X1">A pointer to the starting X-coordinate of the line</param>
	/// <param name="X2">A pointer to the starting Y-coordinate of the line</param>
	/// <param name="Y1">A pointer to the ending X-coordinate of the line</param>
	/// <param name="Y2">A pointer to the ending Y-coordinate of the line</param>
	/// <returns>Returns true if there is an intersection, false otherwise</returns>
	/// <remarks>
	/// <para>
	/// This function is used to clip a line segment to a rectangle. A line segment contained entirely within the rectangle or that does not intersect will remain unchanged.
	/// A line segment that crosses the rectangle at either or both ends will be clipped to the boundary of the rectangle and the new coordinates saved in <c><paramref name="X1"/></c>, <c><paramref name="Y1"/></c>, <c><paramref name="X2"/></c>, and/or <c><paramref name="Y2"/></c> as necessary.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetRectAndLineIntersection">SDL_GetRectAndLineIntersection</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_GetRectAndLineIntersection(Rect<int>* rect, int* X1, int* X2, int* Y1, int* Y2);

	/// <summary>
	/// Calculate the intersection of a rectangle and line segment with float precision
	/// </summary>
	/// <param name="rect">An <see href="https://wiki.libsdl.org/SDL3/SDL_FRect">SDL_FRect</see> structure representing the rectangle to intersect</param>
	/// <param name="X1">A pointer to the starting X-coordinate of the line</param>
	/// <param name="X2">A pointer to the starting Y-coordinate of the line</param>
	/// <param name="Y1">A pointer to the ending X-coordinate of the line</param>
	/// <param name="Y2">A pointer to the ending Y-coordinate of the line</param>
	/// <returns>Returns true if there is an intersection, false otherwise</returns>
	/// <remarks>
	/// <para>
	/// This function is used to clip a line segment to a rectangle. A line segment contained entirely within the rectangle or that does not intersect will remain unchanged.
	/// A line segment that crosses the rectangle at either or both ends will be clipped to the boundary of the rectangle and the new coordinates saved in <c><paramref name="X1"/></c>, <c><paramref name="Y1"/></c>, <c><paramref name="X2"/></c>, and/or <c><paramref name="Y2"/></c> as necessary.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/https://wiki.libsdl.org/SDL3/SDL_GetRectAndLineIntersectionFloat">SDL_GetRectAndLineIntersectionFloat</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_GetRectAndLineIntersectionFloat(Rect<float>* rect, float* X1, float* X2, float* Y1, float* Y2);

	/// <summary>
	/// Calculate a minimal rectangle enclosing a set of points
	/// </summary>
	/// <param name="points">An array of <see href="https://wiki.libsdl.org/SDL3/SDL_Point">SDL_Point</see> structures representing points to be enclosed</param>
	/// <param name="count">The number of structures in the <c><paramref name="points"/></c> array</param>
	/// <param name="clip">An <see href="https://wiki.libsdl.org/SDL3/SDL_Rect">SDL_Rect</see> used for clipping or NULL to enclose all points</param>
	/// <param name="result">An <see href="https://wiki.libsdl.org/SDL3/SDL_Rect">SDL_Rect</see> structure filled in with the minimal enclosing rectangle</param>
	/// <returns>Returns true if any points were enclosed or false if all the points were outside of the clipping rectangle</returns>
	/// <remarks>
	/// <para>
	/// If <c><paramref name="clip"/></c> is not NULL then only points inside of the clipping rectangle are considered.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetRectEnclosingPoints">SDL_GetRectEnclosingPoints</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_GetRectEnclosingPoints(Point<int>* points, int count, Rect<int>* clip, Rect<int>* result);

	/// <summary>
	/// Calculate a minimal rectangle enclosing a set of points with float precision
	/// </summary>
	/// <param name="points">An array of <see href="https://wiki.libsdl.org/SDL3/SDL_FPoint">SDL_FPoint</see> structures representing points to be enclosed</param>
	/// <param name="count">The number of structures in the <c><paramref name="points"/></c> array</param>
	/// <param name="clip">An <see href="https://wiki.libsdl.org/SDL3/SDL_FRect">SDL_FRect</see> used for clipping or NULL to enclose all points</param>
	/// <param name="result">An <see href="https://wiki.libsdl.org/SDL3/SDL_FRect">SDL_FRect</see> structure filled in with the minimal enclosing rectangle</param>
	/// <returns>Returns true if any points were enclosed or false if all the points were outside of the clipping rectangle</returns>
	/// <remarks>
	/// <para>
	/// If <c><paramref name="clip"/></c> is not NULL then only points inside of the clipping rectangle are considered.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetRectEnclosingPointsFloat">SDL_GetRectEnclosingPointsFloat</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_GetRectEnclosingPointsFloat(Point<float>* points, int count, Rect<float>* clip, Rect<float>* result);

	/// <summary>
	/// Calculate the intersection of two rectangles 
	/// </summary>
	/// <param name="A">An <see href="https://wiki.libsdl.org/SDL3/SDL_Rect">SDL_Rect</see> structure representing the first rectangle</param>
	/// <param name="B">An <see href="https://wiki.libsdl.org/SDL3/SDL_Rect">SDL_Rect</see> structure representing the second rectangle</param>
	/// <param name="result">An <see href="https://wiki.libsdl.org/SDL3/SDL_Rect">SDL_Rect</see> structure filled in with the intersection of rectangles <c><paramref name="A"/></c> and <c><paramref name="B"/></c></param>
	/// <returns>Returns true if there is an intersection, false otherwise</returns>
	/// <remarks>
	/// <para>
	/// If <c><paramref name="result"/></c> is NULL then this function will return false.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetRectIntersection">SDL_GetRectIntersection</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_GetRectIntersection(Rect<int>* A, Rect<int>* B, Rect<int>* result);

	/// <summary>
	/// Calculate the intersection of two rectangles with float precision
	/// </summary>
	/// <param name="A">An <see href="https://wiki.libsdl.org/SDL3/SDL_FRect">SDL_FRect</see> structure representing the first rectangle</param>
	/// <param name="B">An <see href="https://wiki.libsdl.org/SDL3/SDL_FRect">SDL_FRect</see> structure representing the second rectangle</param>
	/// <param name="result">An <see href="https://wiki.libsdl.org/SDL3/SDL_FRect">SDL_FRect</see> structure filled in with the intersection of rectangles <c><paramref name="A"/></c> and <c><paramref name="B"/></c></param>
	/// <returns>Returns true if there is an intersection, false otherwise</returns>
	/// <remarks>
	/// <para>
	/// If <c><paramref name="result"/></c> is NULL then this function will return false.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetRectIntersectionFloat">SDL_GetRectIntersectionFloat</seealso> 
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_GetRectIntersectionFloat(Rect<float>* A, Rect<float>* B, Rect<float>* result);

	/// <summary>
	/// Calculate the union of two rectangles
	/// </summary>
	/// <param name="A">An <see href="https://wiki.libsdl.org/SDL3/SDL_Rect">SDL_Rect</see> structure representing the first rectangle</param>
	/// <param name="B">An <see href="https://wiki.libsdl.org/SDL3/SDL_Rect">SDL_Rect</see> structure representing the second rectangle</param>
	/// <param name="result">An <see href="https://wiki.libsdl.org/SDL3/SDL_Rect">SDL_Rect</see> structure filled in with the union of rectangles <c><paramref name="A"/></c> and <c><paramref name="B"/></c></param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information.</returns>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetRectUnion">SDL_GetRectUnion</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_GetRectUnion(Rect<int>* A, Rect<int>* B, Rect<int>* result);

	/// <summary>
	/// Calculate the union of two rectangles with float precision
	/// </summary>
	/// <param name="A">An <see href="https://wiki.libsdl.org/SDL3/SDL_FRect">SDL_FRect</see> structure representing the first rectangle</param>
	/// <param name="B">An <see href="https://wiki.libsdl.org/SDL3/SDL_FRect">SDL_FRect</see> structure representing the second rectangle</param>
	/// <param name="result">An <see href="https://wiki.libsdl.org/SDL3/SDL_FRect">SDL_FRect</see> structure filled in with the union of rectangles <c><paramref name="A"/></c> and <c><paramref name="B"/></c></param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information.</returns>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetRectUnionFloat">SDL_GetRectUnionFloat</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_GetRectUnionFloat(Rect<float>* A, Rect<float>* B, Rect<float>* result);

	/// <summary>
	/// Determine whether two rectangles intersect
	/// </summary>
	/// <param name="A">An <see href="https://wiki.libsdl.org/SDL3/SDL_Rect">SDL_Rect</see> structure representing the first rectangle</param>
	/// <param name="B">An <see href="https://wiki.libsdl.org/SDL3/SDL_Rect">SDL_Rect</see> structure representing the second rectangle</param>
	/// <returns>Returns true if there is an intersection, false otherwise</returns>
	/// <remarks>
	/// <para>
	/// If either pointer is NULL the function will return false.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_HasRectIntersection">SDL_HasRectIntersection</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_HasRectIntersection(Rect<int>* A, Rect<int>* B);

	/// <summary>
	/// Determine whether two rectangles intersect with float precision
	/// </summary>
	/// <param name="A">An <see href="https://wiki.libsdl.org/SDL3/SDL_FRect">SDL_FRect</see> structure representing the first rectangle</param>
	/// <param name="B">An <see href="https://wiki.libsdl.org/SDL3/SDL_FRect">SDL_FRect</see> structure representing the second rectangle</param>
	/// <returns>Returns true if there is an intersection, false otherwise</returns>
	/// <remarks>
	/// <para>
	/// If either pointer is NULL the function will return false.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_HasRectIntersectionFloat">SDL_HasRectIntersectionFloat</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_HasRectIntersectionFloat(Rect<float>* A, Rect<float>* B);
}
