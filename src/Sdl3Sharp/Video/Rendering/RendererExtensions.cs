using Sdl3Sharp.Video.Drawing;
using System;

namespace Sdl3Sharp.Video.Rendering;

/// <summary>
/// Provides extension methods and properties for <see cref="IRenderer"/> and various driver-specific implementations of <see cref="Renderer{TDriver}"/>
/// </summary>
public static partial class RendererExtensions
{
	extension<TRenderer>(TRenderer renderer)
		where TRenderer : notnull, IRenderer
	{
		/// <summary>
		/// Tries to get a point in window coordinates for a specified point in render coordinates
		/// </summary>
		/// <param name="point">The point in render coordinates to convert to window coordinates</param>
		/// <param name="windowPoint">The point in window coordinates corresponding to the given render coordinates, if this method returns <c><see langword="true"/></c></param>
		/// <returns><c><see langword="true"/></c>, if the conversion was successful; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
		/// <remarks>
		/// <para>
		/// The conversion takes into account several factors:
		/// <list type="bullet">
		///	<item><description>The <see cref="IRenderer.Window"/> dimensions</description></item>
		///	<item><description>The <see cref="IRenderer.LogicalPresentation"/> settings</description></item>
		///	<item><description>The <see cref="IRenderer.Scale"/> settings</description></item>
		///	<item><description>The <see cref="IRenderer.Viewport"/> settings</description></item>
		/// </list>
		/// </para>
		/// <para>
		/// This method should only be called from the main thread.
		/// </para>
		/// </remarks>
		public bool TryConvertRenderToWindowCoordinates(Point<float> point, out Point<float> windowPoint)
		{
			var result = renderer.TryConvertRenderToWindowCoordinates(point.X, point.Y, out var windowX, out var windowY);
			windowPoint = new(windowX, windowY);
			return result;
		}

		/// <summary>
		/// Tries to get a point in render coordinates for a specified point in window coordinates
		/// </summary>
		/// <param name="windowPoint">The point in window coordinates to convert to render coordinates</param>
		/// <param name="point">The point in render coordinates corresponding to the given window coordinates, if this method returns <c><see langword="true"/></c></param>
		/// <returns><c><see langword="true"/></c>, if the conversion was successful; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
		/// <remarks>
		/// <para>
		/// The conversion takes into account several factors:
		/// <list type="bullet">
		///	<item><description>The <see cref="IRenderer.Window"/> dimensions</description></item>
		///	<item><description>The <see cref="IRenderer.LogicalPresentation"/> settings</description></item>
		///	<item><description>The <see cref="IRenderer.Scale"/> settings</description></item>
		///	<item><description>The <see cref="IRenderer.Viewport"/> settings</description></item>
		/// </list>
		/// </para>
		/// <para>
		/// This method should only be called from the main thread.
		/// </para>
		/// </remarks>
		public bool TryConvertWindowToRenderCoordinates(Point<float> windowPoint, out Point<float> point)
		{
			var result = renderer.TryConvertWindowToRenderCoordinates(windowPoint.X, windowPoint.Y, out var x, out var y);
			point = new(x, y);
			return result;
		}

		//TODO: replace SDL_ttf in the xml doc with Sdl3Sharp's adaptation of it, once it's done
		/// <summary>
		/// Tries to draw debug text to the current target
		/// </summary>
		/// <param name="point">The top-left point where the text should be drawn</param>
		/// <param name="text">The text to be drawn</param>
		/// <returns><c><see langword="true"/></c>, if the text was drawn successfully; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
		/// <remarks>
		/// <para>
		/// This method will render text to an <see cref="IRenderer"/>.
		/// Note that this is a convenience method for debugging, with severe limitations, and not intended to be used for production applications and games.
		/// </para>
		/// <para>
		/// Among these limitations are:
		/// <list type="bullet">
		/// <item><description>
		/// It accepts Unicode strings, but will only render ASCII characters
		/// </description></item>
		/// <item><description>
		/// It has a single, tiny size (8x8 pixels). You can use <see cref="IRenderer.LogicalPresentation"/> or <see cref="IRenderer.Scale"/> to adjust for that.
		/// </description></item>
		/// <item><description>
		/// It uses a simple, hardcoded bitmap font. It does not allow different font selections and it does not support truetype, for proper scaling.
		/// </description></item>
		/// <item><description>
		/// It doesn't do word-wrapping and doesn't treat newline characters as a line break. If the text goes out of the target, it's gone.
		/// </description></item>
		/// </list>
		/// </para>
		/// <para>
		/// For more serious text rendering, there are several good options, such as <see href="https://wiki.libsdl.org/SDL3/SDL_ttf">SDL_ttf</see>.
		/// </para>
		/// <para>
		/// On first use, this will create an internal texture for rendering glyphs. This texture will live until the renderer is disposed.
		/// </para>
		/// <para>
		/// The text is drawn in the color specified by <see cref="IRenderer.DrawColor"/> (or <see cref="IRenderer.DrawColorFloat"/>) and <see cref="IRenderer.DrawBlendMode"/> determine how the text is blended with the existing content of the target.
		/// </para>
		/// <para>
		/// This method should only be called from the main thread.
		/// </para>
		/// </remarks>
		public bool TryRenderDebugText(Point<float> point, string text) => renderer.TryRenderDebugText(point.X, point.Y, text);

		/// <summary>
		/// Tries to draw a debug format string to the current target
		/// </summary>
		/// <param name="point">The top-left point where the text should be drawn</param>
		/// <param name="format">The C-style <c>printf</c> format string</param>
		/// <param name="args">The arguments corresponding to the format specifiers in <paramref name="format"/></param>
		/// <returns><c><see langword="true"/></c>, if the text was drawn successfully; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
		/// <remarks>
		/// <para>
		/// This method will render text to an <see cref="IRenderer"/>.
		/// Note that this is a convenience method for debugging, with severe limitations, and not intended to be used for production applications and games.
		/// </para>
		/// <para>
		/// Among these limitations are:
		/// <list type="bullet">
		/// <item><description>
		/// It accepts Unicode strings, but will only render ASCII characters
		/// </description></item>
		/// <item><description>
		/// It has a single, tiny size (8x8 pixels). You can use <see cref="IRenderer.LogicalPresentation"/> or <see cref="IRenderer.Scale"/> to adjust for that.
		/// </description></item>
		/// <item><description>
		/// It uses a simple, hardcoded bitmap font. It does not allow different font selections and it does not support truetype, for proper scaling.
		/// </description></item>
		/// <item><description>
		/// It doesn't do word-wrapping and doesn't treat newline characters as a line break. If the text goes out of the target, it's gone.
		/// </description></item>
		/// </list>
		/// </para>
		/// <para>
		/// For more serious text rendering, there are several good options, such as <see href="https://wiki.libsdl.org/SDL3/SDL_ttf">SDL_ttf</see>.
		/// </para>
		/// <para>
		/// On first use, this will create an internal texture for rendering glyphs. This texture will live until the renderer is disposed.
		/// </para>
		/// <para>
		/// The text is drawn in the color specified by <see cref="IRenderer.DrawColor"/> (or <see cref="IRenderer.DrawColorFloat"/>) and <see cref="IRenderer.DrawBlendMode"/> determine how the text is blended with the existing content of the target.
		/// </para>
		/// <para>
		/// The <paramref name="format"/> parameter is interpreted as a C-style <c>printf</c> format string, and 
		/// the <paramref name="args"/> parameter supplies the values for its format specifiers. Supported argument 
		/// types include all integral types up to 64-bit (including <c><see langword="bool"/></c> and <c><see langword="char"/></c>), 
		/// floating point types (<c><see langword="float"/></c> and <c><see langword="double"/></c>), pointer types 
		/// (<c><see cref="IntPtr"/></c> and <c><see cref="UIntPtr"/></c>), and <c><see langword="string"/></c>.
		/// </para>
		/// <para>
		/// For a detailed explanation of C-style <c>printf</c> format strings and their specifiers, see 
		/// <see href="https://en.wikipedia.org/wiki/Printf#Format_specifier"/>.
		/// </para>
		/// <para>
		/// Consider using <see cref="RendererExtensions.TryRenderDebugText{TRenderer}(TRenderer, Point{float}, string)"/> instead when possible, as it may be more efficient. 
		/// In many cases, you can use C# string interpolation to construct the message before logging.
		/// </para>
		/// <para>
		/// This method should only be called from the main thread.
		/// </para>
		/// </remarks>
		public bool TryRenderDebugText(Point<float> point, string format, params ReadOnlySpan<object> args) => renderer.TryRenderDebugText(point.X, point.Y, format, args);

		/// <summary>
		/// Tries to draw a line to the current target
		/// </summary>
		/// <param name="startPoint">The start point of the line</param>
		/// <param name="endPoint">The end point of the line</param>
		/// <returns><c><see langword="true"/></c>, if the line was drawn successfully; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)s</returns>
		/// <remarks>
		/// <para>
		/// The line is drawn with the color specified by <see cref="IRenderer.DrawColor"/> (or <see cref="IRenderer.DrawColorFloat"/>) and <see cref="IRenderer.DrawBlendMode"/> determines how the line is blended with the existing content of the target.
		/// </para>
		/// <para>
		/// Drawing the line works with sub-pixel precision.
		/// </para>
		/// <para>
		/// This method should only be called from the main thread.
		/// </para>
		/// </remarks>
		public bool TryRenderLine(Point<float> startPoint, Point<float> endPoint) => renderer.TryRenderLine(startPoint.X, startPoint.Y, endPoint.X, endPoint.Y);

		/// <summary>
		/// Tries to draw a point to the current target
		/// </summary>
		/// <param name="point">The point to draw</param>
		/// <returns><c><see langword="true"/></c> if the point was drawn successfully; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
		/// <remarks>
		/// <para>
		/// The point is drawn with the color specified by <see cref="IRenderer.DrawColor"/> (or <see cref="IRenderer.DrawColorFloat"/>) and <see cref="IRenderer.DrawBlendMode"/> determines how the point is blended with the existing content of the target.
		/// </para>
		/// <para>
		/// Drawing the point works with sub-pixel precision.
		/// </para>
		/// <para>
		/// This method should only be called from the main thread.
		/// </para>
		/// </remarks>
		public bool TryRenderPoint(Point<float> point) => renderer.TryRenderPoint(point.X, point.Y);
	}
}
