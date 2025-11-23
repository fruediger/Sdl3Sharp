using Sdl3Sharp.Internal.Interop;
using Sdl3Sharp.SourceGeneration;
using Sdl3Sharp.Video.Coloring;
using Sdl3Sharp.Video.Drawing;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using static System.Net.WebRequestMethods;

namespace Sdl3Sharp.Video;

partial class Surface
{
	[StructLayout(LayoutKind.Sequential)]
	internal readonly struct SDL_Surface
	{
		public readonly SurfaceFlags Flags;
		public readonly PixelFormat Format;
		public readonly int W;
		public readonly int H;
		public readonly int Pitch;
		public unsafe readonly void* Pixels;
		public readonly int RefCount;
		private unsafe readonly void* Reserved;
	}

	/// <summary>
	/// Add an alternate version of a surface
	/// </summary>
	/// <param name="surface">The <see href="https://wiki.libsdl.org/SDL3/SDL_Surface">SDL_Surface</see> structure to update</param>
	/// <param name="image">A pointer to an alternate <see href="https://wiki.libsdl.org/SDL3/SDL_Surface">SDL_Surface</see> to associate with this surface</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// This function adds an alternate version of this surface, usually used for content with high DPI representations like cursors or icons.
	/// The size, format, and content do not need to match the original surface, and these alternate versions will not be updated when the original surface changes.
	/// </para>
	/// <para>
	/// This function adds a reference to the alternate version, so you should call <see href="https://wiki.libsdl.org/SDL3/SDL_DestroySurface">SDL_DestroySurface</see>() on the image after this call.
	/// </para>
	/// <para>
	/// This function can be called on different threads with different surfaces.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_AddSurfaceAlternateImage">SDL_AddSurfaceAlternateImage</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_AddSurfaceAlternateImage(SDL_Surface* surface, SDL_Surface* image);

	/// <summary>
	/// Performs a fast blit from the source surface to the destination surface with clipping
	/// </summary>
	/// <param name="src">The <see href="https://wiki.libsdl.org/SDL3/SDL_Surface">SDL_Surface</see> structure to be copied from</param>
	/// <param name="srcrect">The <see href="https://wiki.libsdl.org/SDL3/SDL_Rect">SDL_Rect</see> structure representing the rectangle to be copied, or NULL to copy the entire surface</param>
	/// <param name="dst">The <see href="https://wiki.libsdl.org/SDL3/SDL_Surface">SDL_Surface</see> structure that is the blit target</param>
	/// <param name="dstrect">
	/// The <see href="https://wiki.libsdl.org/SDL3/SDL_Rect">SDL_Rect</see> structure representing the x and y position in the destination surface, or NULL for (0,0).
	/// The width and height are ignored, and are copied from srcrect.
	/// If you want a specific width and height, you should use <see href="https://wiki.libsdl.org/SDL3/SDL_BlitSurfaceScaled">SDL_BlitSurfaceScaled()</see>.
	/// </param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// If either <c><paramref name="srcrect"/></c> or <c><paramref name="dstrect"/></c> are NULL, the entire surface (<c><paramref name="src"/></c> or <c><paramref name="dst"/></c>) is copied while ensuring clipping to <c><paramref name="dst"/>-&gt;clip_rect</c>.
	/// </para>
	/// <para>
	/// The blit function should not be called on a locked surface.
	/// </para>
	/// <para>
	/// The blit semantics for surfaces with and without blending and colorkey are defined as follows:
	/// <code>
	///   RGBA->RGB:
	///     Source surface blend mode set to SDL_BLENDMODE_BLEND:
	///      alpha-blend (using the source alpha-channel and per-surface alpha)
	///      SDL_SRCCOLORKEY ignored.
	///    Source surface blend mode set to SDL_BLENDMODE_NONE:
	///      copy RGB.
	///      if SDL_SRCCOLORKEY set, only copy the pixels that do not match the
	///      RGB values of the source color key, ignoring alpha in the
	///      comparison.
	///
	///  RGB->RGBA:
	///    Source surface blend mode set to SDL_BLENDMODE_BLEND:
	///      alpha-blend (using the source per-surface alpha)
	///    Source surface blend mode set to SDL_BLENDMODE_NONE:
	///      copy RGB, set destination alpha to source per-surface alpha value.
	///    both:
	///      if SDL_SRCCOLORKEY set, only copy the pixels that do not match the
	///      source color key.
	///
	///  RGBA->RGBA:
	///    Source surface blend mode set to SDL_BLENDMODE_BLEND:
	///      alpha-blend (using the source alpha-channel and per-surface alpha)
	///      SDL_SRCCOLORKEY ignored.
	///    Source surface blend mode set to SDL_BLENDMODE_NONE:
	///      copy all of RGBA to the destination.
	///      if SDL_SRCCOLORKEY set, only copy the pixels that do not match the
	///      RGB values of the source color key, ignoring alpha in the
	///      comparison.
	///
	///  RGB->RGB:
	///    Source surface blend mode set to SDL_BLENDMODE_BLEND:
	///      alpha-blend (using the source per-surface alpha)
	///    Source surface blend mode set to SDL_BLENDMODE_NONE:
	///      copy RGB.
	///    both:
	///      if SDL_SRCCOLORKEY set, only copy the pixels that do not match the
	///      source color key.
	/// </code>
	/// </para>
	/// <para>
	/// Only one thread should be using the <c><paramref name="src"/></c> and <c><paramref name="dst"/></c> surfaces at any given time.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_BlitSurface"></seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_BlitSurface(SDL_Surface* src, Rect<int>* srcrect, SDL_Surface* dst, Rect<int>* dstrect);

	/// <summary>
	/// Perform a scaled blit using the 9-grid algorithm to a destination surface, which may be of a different format.
	/// </summary>
	/// <param name="src">The <see href="https://wiki.libsdl.org/SDL3/SDL_Surface">SDL_Surface</see> structure to be copied from</param>
	/// <param name="srcrect">The <see href="https://wiki.libsdl.org/SDL3/SDL_Rect">SDL_Rect</see> structure representing the rectangle to be used for the 9-grid, or NULL to use the entire surface</param>
	/// <param name="left_width">The width, in pixels, of the left corners in <c><paramref name="srcrect"/></c></param>
	/// <param name="right_width">The width, in pixels, of the right corners in <c><paramref name="srcrect"/></c></param>
	/// <param name="top_height">The height, in pixels, of the top corners in <c><paramref name="srcrect"/></c></param>
	/// <param name="bottom_height">The height, in pixels, of the bottom corners in <c><paramref name="srcrect"/></c></param>
	/// <param name="scale">The scale used to transform the corner of <c><paramref name="srcrect"/></c>into the corner of <c><paramref name="dstrect"/></c>, or 0.0f for an unscaled blit</param>
	/// <param name="scaleMode">Scale algorithm to be used</param>
	/// <param name="dst">The <see href="https://wiki.libsdl.org/SDL3/SDL_Surface">SDL_Surface</see> structure that is the blit target</param>
	/// <param name="dstrect">The <see href="https://wiki.libsdl.org/SDL3/SDL_Rect">SDL_Rect</see> structure representing the target rectangle in the destination surface, or NULL to fill the entire surface</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// The pixels in the source surface are split into a 3x3 grid, using the different corner sizes for each corner, and the sides and center making up the remaining pixels.
	/// The corners are then scaled using <c><paramref name="scale"/></c> and fit into the corners of the destination rectangle.
	/// The sides and center are then stretched into place to cover the remaining destination rectangle.
	/// </para>
	/// <para>
	/// Only one thread should be using the <c><paramref name="src"/></c> and <c><paramref name="dst"/></c> surfaces at any given time.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_BlitSurface9Grid">SDL_BlitSurface9Grid</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_BlitSurface9Grid(SDL_Surface* src, Rect<int>* srcrect, int left_width, int right_width, int top_height, int bottom_height, float scale, ScaleMode scaleMode, SDL_Surface* dst, Rect<int>* dstrect);

	/// <summary>
	/// Perform a scaled blit to a destination surface, which may be of a different format
	/// </summary>
	/// <param name="src">The <see href="https://wiki.libsdl.org/SDL3/SDL_Surface">SDL_Surface</see> structure to be copied from</param>
	/// <param name="srcrect">The <see href="https://wiki.libsdl.org/SDL3/SDL_Rect">SDL_Rect</see> structure representing the rectangle to be copied, or NULL to copy the entire surface</param>
	/// <param name="dst">The <see href="https://wiki.libsdl.org/SDL3/SDL_Surface">SDL_Surface</see> structure that is the blit target</param>
	/// <param name="dstrect">The <see href="https://wiki.libsdl.org/SDL3/SDL_Rect">SDL_Rect</see> structure representing the target rectangle in the destination surface, or NULL to fill the entire destination surface</param>
	/// <param name="scaleMode">The <see href="https://wiki.libsdl.org/SDL3/SDL_ScaleMode">SDL_ScaleMode</see> to be used</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks> 
	/// <para>
	/// Only one thread should be using the <c><paramref name="src"/></c> and <c><paramref name="dst"/></c> surfaces at any given time.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_BlitSurfaceScaled">SDL_BlitSurfaceScaled</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_BlitSurfaceScaled(SDL_Surface* src, Rect<int>* srcrect, SDL_Surface* dst, Rect<int>* dstrect, ScaleMode scaleMode);

	/// <summary>
	/// Perform a tiled blit to a destination surface, which may be of a different format
	/// </summary>
	/// <param name="src">The <see href="https://wiki.libsdl.org/SDL3/SDL_Surface">SDL_Surface</see> structure to be copied from</param>
	/// <param name="srcrect">The <see href="https://wiki.libsdl.org/SDL3/SDL_Rect">SDL_Rect</see> structure representing the rectangle to be copied, or NULL to copy the entire surface</param>
	/// <param name="dst">The <see href="https://wiki.libsdl.org/SDL3/SDL_Surface">SDL_Surface</see> structure that is the blit target</param>
	/// <param name="dstrect">The <see href="https://wiki.libsdl.org/SDL3/SDL_Rect">SDL_Rect</see> structure representing the target rectangle in the destination surface, or NULL to fill the entire destination surface</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// The pixels in <c><paramref name="srcrect"/></c> will be repeated as many times as needed to completely fill <c><paramref name="dstrect"/></c>.
	/// </para>
	/// <para>
	/// Only one thread should be using the <c><paramref name="src"/></c> and <c><paramref name="dst"/></c> surfaces at any given time.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_BlitSurfaceTiled">SDL_BlitSurfaceTiled</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_BlitSurfaceTiled(SDL_Surface* src, Rect<int>* srcrect, SDL_Surface* dst, Rect<int>* dstrect);

	/// <summary>
	/// Perform a scaled and tiled blit to a destination surface, which may be of a different format
	/// </summary>
	/// <param name="src">The <see href="https://wiki.libsdl.org/SDL3/SDL_Surface">SDL_Surface</see> structure to be copied from</param>
	/// <param name="srcrect">The <see href="https://wiki.libsdl.org/SDL3/SDL_Rect">SDL_Rect</see> structure representing the rectangle to be copied, or NULL to copy the entire surface</param>
	/// <param name="scale">The scale used to transform <c><paramref name="srcrect"/></c> into the destination rectangle, e.g. a 32x32 texture with a scale of 2 would fill 64x64 tiles</param>
	/// <param name="scaleMode">Scale algorithm to be used</param>
	/// <param name="dst">The <see href="https://wiki.libsdl.org/SDL3/SDL_Surface">SDL_Surface</see> structure that is the blit target</param>
	/// <param name="dstrect">The <see href="https://wiki.libsdl.org/SDL3/SDL_Rect">SDL_Rect</see> structure representing the target rectangle in the destination surface, or NULL to fill the entire destination surface</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// The pixels in <c><paramref name="srcrect"/></c> will be repeated as many times as needed to completely fill <c><paramref name="dstrect"/></c>.
	/// </para>
	/// <para>
	/// Only one thread should be using the <c><paramref name="src"/></c> and <c><paramref name="dst"/></c> surfaces at any given time.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_BlitSurfaceTiledWithScale">SDL_BlitSurfaceTiledWithScale</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_BlitSurfaceTiledWithScale(SDL_Surface* src, Rect<int>* srcrect, float scale, ScaleMode scaleMode, SDL_Surface* dst, Rect<int>* dstrect);

	/// <summary>
	/// Perform low-level surface blitting only
	/// </summary>
	/// <param name="src">The <see href="https://wiki.libsdl.org/SDL3/SDL_Surface">SDL_Surface</see> structure to be copied from</param>
	/// <param name="srcrect">The <see href="https://wiki.libsdl.org/SDL3/SDL_Rect">SDL_Rect</see> structure representing the rectangle to be copied, may not be NULL</param>
	/// <param name="dst">The <see href="https://wiki.libsdl.org/SDL3/SDL_Surface">SDL_Surface</see> structure that is the blit target</param>
	/// <param name="dstrect">The <see href="https://wiki.libsdl.org/SDL3/SDL_Rect">SDL_Rect</see> structure representing the target rectangle in the destination surface, may not be NULL</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// This is a semi-private blit function and it performs low-level surface blitting, assuming the input rectangles have already been clipped.
	/// </para>
	/// <para>
	/// Only one thread should be using the <c><paramref name="src"/></c> and <c><paramref name="dst"/></c> surfaces at any given time.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_BlitSurfaceUnchecked">SDL_BlitSurfaceUnchecked</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_BlitSurfaceUnchecked(SDL_Surface* src, Rect<int>* srcrect, SDL_Surface* dst, Rect<int>* dstrect);

	/// <summary>
	/// Perform low-level surface scaled blitting only
	/// </summary>
	/// <param name="src">The <see href="https://wiki.libsdl.org/SDL3/SDL_Surface">SDL_Surface</see> structure to be copied from</param>
	/// <param name="srcrect">The <see href="https://wiki.libsdl.org/SDL3/SDL_Rect">SDL_Rect</see> structure representing the rectangle to be copied, may not be NULL</param>
	/// <param name="dst">The <see href="https://wiki.libsdl.org/SDL3/SDL_Surface">SDL_Surface</see> structure that is the blit target</param>
	/// <param name="dstrect">The <see href="https://wiki.libsdl.org/SDL3/SDL_Rect">SDL_Rect</see> structure representing the target rectangle in the destination surface, may not be NULL</param>
	/// <param name="scaleMode">The <see href="https://wiki.libsdl.org/SDL3/SDL_ScaleMode">SDL_ScaleMode</see> to be used</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information.</returns>
	/// <remarks>
	/// <para>
	/// This is a semi-private function and it performs low-level surface blitting, assuming the input rectangles have already been clipped.
	/// </para>
	/// <para>
	/// Only one thread should be using the <c><paramref name="src"/></c> and <c><paramref name="dst"/></c> surfaces at any given time.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_BlitSurfaceUncheckedScaled">SDL_BlitSurfaceUncheckedScaled</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_BlitSurfaceUncheckedScaled(SDL_Surface* src, Rect<int>* srcrect, SDL_Surface* dst, Rect<int>* dstrect, ScaleMode scaleMode);

	/// <summary>
	/// Clear a surface with a specific color, with floating point precision
	/// </summary>
	/// <param name="surface">The <see href="https://wiki.libsdl.org/SDL3/SDL_Surface">SDL_Surface</see> to clear</param>
	/// <param name="r">The red component of the pixel, normally in the range 0-1</param>
	/// <param name="g">The green component of the pixel, normally in the range 0-1</param>
	/// <param name="b">The blue component of the pixel, normally in the range 0-1</param>
	/// <param name="a">The alpha component of the pixel, normally in the range 0-1</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// This function handles all surface formats, and ignores any clip rectangle.
	/// </para>
	/// <para>
	/// If the surface is YUV, the color is assumed to be in the sRGB colorspace, otherwise the color is assumed to be in the colorspace of the suface.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_ClearSurface">SDL_ClearSurface</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_ClearSurface(SDL_Surface* surface, float r, float g, float b, float a);

	/// <summary>
	/// Copy a block of pixels of one format to another format
	/// </summary>
	/// <param name="width">The width of the block to copy, in pixels</param>
	/// <param name="height">The height of the block to copy, in pixels</param>
	/// <param name="src_format">An <see href="https://wiki.libsdl.org/SDL3/SDL_PixelFormat">SDL_PixelFormat</see> value of the <c><paramref name="src"/></c> pixels format</param>
	/// <param name="src">A pointer to the source pixels</param>
	/// <param name="src_pitch">The pitch of the source pixels, in bytes</param>
	/// <param name="dst_format">An <see href="https://wiki.libsdl.org/SDL3/SDL_PixelFormat">SDL_PixelFormat</see> value of the <c><paramref name="dst"/></c> pixels format</param>
	/// <param name="dst">A pointer to be filled in with new pixel data</param>
	/// <param name="dst_pitch">The pitch of the destination pixels, in bytes</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// The same destination pixels should not be used from two threads at once. It is safe to use the same source pixels from multiple threads.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_ConvertPixels">SDL_ConvertPixels</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial bool SDL_ConvertPixels(int width, int height, PixelFormat src_format, void* src, int src_pitch, PixelFormat dst_format, void* dst, int dst_pitch);

	/// <summary>
	/// Copy a block of pixels of one format and colorspace to another format and colorspace
	/// </summary>
	/// <param name="width">The width of the block to copy, in pixels</param>
	/// <param name="height">The height of the block to copy, in pixels</param>
	/// <param name="src_format">An <see href="https://wiki.libsdl.org/SDL3/SDL_PixelFormat">SDL_PixelFormat</see> value of the <c><paramref name="src"/></c> pixels format</param>
	/// <param name="src_colorspace">An <see href="https://wiki.libsdl.org/SDL3/SDL_Colorspace">SDL_Colorspace</see> value describing the colorspace of the <c><paramref name="src"/></c> pixels</param>
	/// <param name="src_properties">An <see href="https://wiki.libsdl.org/SDL3/SDL_PropertiesID">SDL_PropertiesID</see> with additional source color properties, or 0</param>
	/// <param name="src">A pointer to the source pixels</param>
	/// <param name="src_pitch">The pitch of the source pixels, in bytes</param>
	/// <param name="dst_format">An <see href="https://wiki.libsdl.org/SDL3/SDL_PixelFormat">SDL_PixelFormat</see> value of the <c><paramref name="dst"/></c> pixels format</param>
	/// <param name="dst_colorspace">An <see href="https://wiki.libsdl.org/SDL3/SDL_Colorspace">SDL_Colorspace</see> value describing the colorspace of the dst pixels</param>
	/// <param name="dst_properties">An <see href="https://wiki.libsdl.org/SDL3/SDL_PropertiesID">SDL_PropertiesID</see> with additional destination color properties, or 0</param>
	/// <param name="dst">A pointer to be filled in with new pixel data</param>
	/// <param name="dst_pitch">The pitch of the destination pixels, in bytes</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// The same destination pixels should not be used from two threads at once. It is safe to use the same source pixels from multiple threads.
	/// </para>
	/// </remarks>
	/// <seealso cref="https://wiki.libsdl.org/SDL3/SDL_ConvertPixelsAndColorspace">SDL_ConvertPixelsAndColorspace</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial bool SDL_ConvertPixelsAndColorspace(int width, int height, PixelFormat src_format, ColorSpace src_colorspace, uint src_properties, void* src, int src_pitch, PixelFormat dst_format, ColorSpace dst_colorspace, uint dst_properties, void* dst, int dst_pitch);

	/// <summary>
	/// Copy an existing surface to a new surface of the specified format
	/// </summary>
	/// <param name="surface">The existing <see href="https://wiki.libsdl.org/SDL3/SDL_Surface">SDL_Surface</see> structure to convert</param>
	/// <param name="format">The new pixel format</param>
	/// <returns>Returns the new <see href="https://wiki.libsdl.org/SDL3/SDL_Surface">SDL_Surface</see> structure that is created or NULL on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// This function is used to optimize images for faster <em>repeat</em> blitting.
	/// This is accomplished by converting the original and storing the result as a new surface.
	/// The new, optimized surface can then be used as the source for future blits, making them faster.
	/// </para>
	/// <para>
	/// If you are converting to an indexed surface and want to map colors to a palette, you can use <see href="https://wiki.libsdl.org/SDL3/SDL_ConvertSurfaceAndColorspace">SDL_ConvertSurfaceAndColorspace</see>() instead.
	/// </para>
	/// <para>
	/// If the original surface has alternate images, the new surface will have a reference to them as well.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_ConvertSurface">SDL_ConvertSurface</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial SDL_Surface* SDL_ConvertSurface(SDL_Surface* surface, PixelFormat format);

	/// <summary>
	/// Copy an existing surface to a new surface of the specified format and colorspace
	/// </summary>
	/// <param name="surface">The existing <see href="https://wiki.libsdl.org/SDL3/SDL_Surface">SDL_Surface</see> structure to convert</param>
	/// <param name="format">The new pixel format</param>
	/// <param name="palette">An optional palette to use for indexed formats, may be NULL</param>
	/// <param name="colorspace">The new colorspace</param>
	/// <param name="props">An <see href="https://wiki.libsdl.org/SDL3/SDL_PropertiesID">SDL_PropertiesID</see> with additional color properties, or 0</param>
	/// <returns>Returns the new <see href="https://wiki.libsdl.org/SDL3/SDL_Surface">SDL_Surface</see> structure that is created or NULL on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// This function converts an existing surface to a new format and colorspace and returns the new surface. This will perform any pixel format and colorspace conversion needed.
	/// </para>
	/// <para>
	/// If the original surface has alternate images, the new surface will have a reference to them as well.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_ConvertSurfaceAndColorspace">SDL_ConvertSurfaceAndColorspace</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial SDL_Surface* SDL_ConvertSurfaceAndColorspace(SDL_Surface* surface, PixelFormat format, Palette.SDL_Palette* palette, ColorSpace colorspace, uint props);

	/// <summary>
	/// Allocate a new surface with a specific pixel format
	/// </summary>
	/// <param name="width">The width of the surface</param>
	/// <param name="height">The height of the surface</param>
	/// <param name="format">The <see href="https://wiki.libsdl.org/SDL3/SDL_PixelFormat">SDL_PixelFormat</see> for the new surface's pixel format</param>
	/// <returns>Returns the new <see href="https://wiki.libsdl.org/SDL3/SDL_Surface">SDL_Surface</see> structure that is created or NULL on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// The pixels of the new surface are initialized to zero.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_CreateSurface">SDL_CreateSurface</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial SDL_Surface* SDL_CreateSurface(int width, int height, PixelFormat format);

	/// <summary>
	/// Allocate a new surface with a specific pixel format and existing pixel data
	/// </summary>
	/// <param name="width">The width of the surface</param>
	/// <param name="height">The height of the surface</param>
	/// <param name="format">The <see href="https://wiki.libsdl.org/SDL3/SDL_PixelFormat">SDL_PixelFormat</see> for the new surface's pixel format</param>
	/// <param name="pixels">A pointer to existing pixel data</param>
	/// <param name="pitch">The number of bytes between each row, including padding</param>
	/// <returns>Returns the new <see href="https://wiki.libsdl.org/SDL3/SDL_Surface">SDL_Surface</see> structure that is created or NULL on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// No copy is made of the pixel data. Pixel data is not managed automatically; you must free the surface before you free the pixel data.
	/// </para>
	/// <para>
	/// Pitch is the offset in bytes from one row of pixels to the next, e.g. <c>width*4</c> for <see href="https://wiki.libsdl.org/SDL3/SDL_PIXELFORMAT_RGBA8888"><c>SDL_PIXELFORMAT_RGBA8888</c></see>.
	/// </para>
	/// <para>
	/// You may pass NULL for pixels and 0 for pitch to create a surface that you will fill in with valid values later.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_CreateSurfaceFrom">SDL_CreateSurfaceFrom</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial SDL_Surface* SDL_CreateSurfaceFrom(int width, int height, PixelFormat format, void* pixels, int pitch);

	/// <summary>
	/// Create a palette and associate it with a surface
	/// </summary>
	/// <param name="surface">The <see href="https://wiki.libsdl.org/SDL3/SDL_Surface">SDL_Surface</see> structure to update</param>
	/// <returns>Returns a new <see href="https://wiki.libsdl.org/SDL3/SDL_Palette">SDL_Palette</see> structure on success or NULL on failure (e.g. if the surface didn't have an index format); call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// This function creates a palette compatible with the provided surface. The palette is then returned for you to modify, and the surface will automatically use the new palette in future operations.
	/// You do not need to destroy the returned palette, it will be freed when the reference count reaches 0, usually when the surface is destroyed.
	/// </para>
	/// <para>
	/// Bitmap surfaces (with format <see href="https://wiki.libsdl.org/SDL3/SDL_PIXELFORMAT_INDEX1LSB">SDL_PIXELFORMAT_INDEX1LSB</see> or <see href="https://wiki.libsdl.org/SDL3/SDL_PIXELFORMAT_INDEX1MSB">SDL_PIXELFORMAT_INDEX1MSB</see>) will have the palette initialized with 0 as white and 1 as black.
	/// Other surfaces will get a palette initialized with white in every entry.
	/// </para>
	/// <para>
	/// If this function is called for a surface that already has a palette, a new palette will be created to replace it.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_CreateSurfacePalette">SDL_CreateSurfacePalette</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial Palette.SDL_Palette* SDL_CreateSurfacePalette(SDL_Surface* surface);

	/// <summary>
	/// Free a surface
	/// </summary>
	/// <param name="surface">The <see href="https://wiki.libsdl.org/SDL3/SDL_Surface">SDL_Surface</see> to free</param>
	/// <remarks>
	/// <para>
	/// It is safe to pass NULL to this function.
	/// </para>
	/// <para>
	/// No other thread should be using the surface when it is freed.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_DestroySurface">SDL_DestroySurface</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial void SDL_DestroySurface(SDL_Surface* surface);

	/// <summary>
	/// Creates a new surface identical to the existing surface
	/// </summary>
	/// <param name="surface">The surface to duplicate</param>
	/// <returns>Returns a copy of the surface or NULL on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information.</returns>
	/// <remarks>
	/// <para>
	/// If the original surface has alternate images, the new surface will have a reference to them as well.
	/// </para>
	/// <para>
	/// The returned surface should be freed with <see href="https://wiki.libsdl.org/SDL3/SDL_DestroySurface">SDL_DestroySurface</see>().
	/// </para>
	/// <para>
	/// This function can be called on different threads with different surfaces.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_DuplicateSurface">SDL_DuplicateSurface</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial SDL_Surface* SDL_DuplicateSurface(SDL_Surface* surface);

	/// <summary>
	/// Perform a fast fill of a rectangle with a specific color
	/// </summary>
	/// <param name="dst">The <see href="https://wiki.libsdl.org/SDL3/SDL_Surface">SDL_Surface</see> structure that is the drawing target</param>
	/// <param name="rect">The <see href="https://wiki.libsdl.org/SDL3/SDL_Rect">SDL_Rect</see> structure representing the rectangle to fill, or NULL to fill the entire surface</param>
	/// <param name="color">The color to fill with</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// <c><paramref name="color"/></c> should be a pixel of the format used by the surface, and can be generated by <see href="https://wiki.libsdl.org/SDL3/SDL_MapRGB">SDL_MapRGB</see>() or <see href="https://wiki.libsdl.org/SDL3/SDL_MapRGBA">SDL_MapRGBA</see>().
	/// If the color value contains an alpha component then the destination is simply filled with that alpha information, no blending takes place.
	/// </para>
	/// <para>
	/// If there is a clip rectangle set on the destination (set via <see href="https://wiki.libsdl.org/SDL3/SDL_SetSurfaceClipRect">SDL_SetSurfaceClipRect</see>()), then this function will fill based on the intersection of the clip rectangle and <c><paramref name="rect"/></c>.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_FillSurfaceRect">SDL_FillSurfaceRect</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_FillSurfaceRect(SDL_Surface* dst, Rect<int>* rect, uint color);

	/// <summary>
	/// Perform a fast fill of a set of rectangles with a specific color
	/// </summary>
	/// <param name="dst">The <see href="https://wiki.libsdl.org/SDL3/SDL_Surface">SDL_Surface</see> structure that is the drawing target</param>
	/// <param name="rects">An array of <see href="https://wiki.libsdl.org/SDL3/SDL_Rect">SDL_Rect</see> representing the rectangles to fill</param>
	/// <param name="count">The number of rectangles in the array</param>
	/// <param name="color">The color to fill with</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// <c><paramref name="color"/></c> should be a pixel of the format used by the surface, and can be generated by <see href="https://wiki.libsdl.org/SDL3/SDL_MapRGB">SDL_MapRGB</see>() or <see href="https://wiki.libsdl.org/SDL3/SDL_MapRGBA">SDL_MapRGBA</see>().
	/// If the color value contains an alpha component then the destination is simply filled with that alpha information, no blending takes place.
	/// </para>
	/// <para>
	/// If there is a clip rectangle set on the destination (set via <see href="https://wiki.libsdl.org/SDL3/SDL_SetSurfaceClipRect">SDL_SetSurfaceClipRect</see>()), then this function will fill based on the intersection of the clip rectangle and <c><paramref name="rects"/></c>.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_FillSurfaceRects">SDL_FillSurfaceRects</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_FillSurfaceRects(SDL_Surface* dst, Rect<int>* rects, int count, uint color);

	/// <summary>
	/// Flip a surface vertically or horizontally
	/// </summary>
	/// <param name="surface">The surface to flip</param>
	/// <param name="flip">The direction to flip</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_FlipSurface">SDL_FlipSurface</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_FlipSurface(SDL_Surface* surface, FlipMode flip);

	/// <summary>
	/// Get the additional alpha value used in blit operations
	/// </summary>
	/// <param name="surface">The <see href="https://wiki.libsdl.org/SDL3/SDL_Surface">SDL_Surface</see> structure to query</param>
	/// <param name="alpha">Aa pointer filled in with the current alpha value</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetSurfaceAlphaMod">SDL_GetSurfaceAlphaMod</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_GetSurfaceAlphaMod(SDL_Surface* surface, byte* alpha);

	/// <summary>
	/// Get the palette used by a surface
	/// </summary>
	/// <param name="surface">The <see href="https://wiki.libsdl.org/SDL3/SDL_Surface">SDL_Surface</see> structure to query</param>
	/// <returns>Returns a pointer to the palette used by the surface, or NULL if there is no palette used</returns>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetSurfacePalette"></seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial Palette.SDL_Palette* SDL_GetSurfacePalette(SDL_Surface* surface);

	/// <summary>
	/// Map an RGB triple to an opaque pixel value for a surface
	/// </summary>
	/// <param name="surface">The surface to use for the pixel format and palette</param>
	/// <param name="r">The red component of the pixel in the range 0-255</param>
	/// <param name="g">The green component of the pixel in the range 0-255</param>
	/// <param name="b">The blue component of the pixel in the range 0-255</param>
	/// <returns>Returns a pixel value</returns>
	/// <remarks>
	/// <para>
	/// This function maps the RGB color value to the specified pixel format and returns the pixel value best approximating the given RGB color value for the given pixel format.
	/// </para>
	/// <para>
	/// If the surface has a palette, the index of the closest matching color in the palette will be returned.
	/// </para>
	/// <para>
	/// If the surface pixel format has an alpha component it will be returned as all 1 bits (fully opaque).
	/// </para>
	/// <para>
	/// If the pixel format bpp (color depth) is less than 32-bpp then the unused upper bits of the return value can safely be ignored
	/// (e.g., with a 16-bpp format the return value can be assigned to a <see href="https://wiki.libsdl.org/SDL3/Uint16">Uint16</see>, and similarly a Uint8 for an 8-bpp format).
	/// </para>
	/// </remarks>
	/// <seealso cref="https://wiki.libsdl.org/SDL3/SDL_MapSurfaceRGB">SDL_MapSurfaceRGB</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial uint SDL_MapSurfaceRGB(SDL_Surface* surface, byte r, byte g, byte b);

	/// <summary>
	/// Map an RGBA quadruple to a pixel value for a surface
	/// </summary>
	/// <param name="surface">The surface to use for the pixel format and palette</param>
	/// <param name="r">The red component of the pixel in the range 0-255</param>
	/// <param name="g">The green component of the pixel in the range 0-255</param>
	/// <param name="b">The blue component of the pixel in the range 0-255</param>
	/// <param name="a">The alpha component of the pixel in the range 0-255</param>
	/// <returns>Returns a pixel value</returns>
	/// <remarks>
	/// <para>
	/// This function maps the RGBA color value to the specified pixel format and returns the pixel value best approximating the given RGBA color value for the given pixel format.
	/// </para>
	/// <para>
	/// If the surface pixel format has no alpha component the alpha value will be ignored (as it will be in formats with a palette).
	/// </para>
	/// <para>
	/// If the surface has a palette, the index of the closest matching color in the palette will be returned.
	/// </para>
	/// <para>
	/// If the pixel format bpp (color depth) is less than 32-bpp then the unused upper bits of the return value can safely be ignored
	/// (e.g., with a 16-bpp format the return value can be assigned to a <see href="https://wiki.libsdl.org/SDL3/Uint16">Uint16</see>, and similarly a Uint8 for an 8-bpp format).
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_MapSurfaceRGBA">SDL_MapSurfaceRGBA</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial uint SDL_MapSurfaceRGBA(SDL_Surface* surface, byte r, byte g, byte b, byte a);

	/// <summary>
	/// Set an additional alpha value used in blit operations
	/// </summary>
	/// <param name="surface">The <see href="https://wiki.libsdl.org/SDL3/SDL_Surface">SDL_Surface</see> structure to update</param>
	/// <param name="alpha">The alpha value multiplied into blit operations</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information.</returns>
	/// <remarks>
	/// <para>
	/// When this surface is blitted, during the blit operation the source alpha value is modulated by this alpha value according to the following formula:
	/// <code>
	/// srcA = srcA * (alpha / 255)
	/// </code>
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_SetSurfaceAlphaMod">SDL_SetSurfaceAlphaMod</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_SetSurfaceAlphaMod(SDL_Surface* surface, byte alpha);
}
