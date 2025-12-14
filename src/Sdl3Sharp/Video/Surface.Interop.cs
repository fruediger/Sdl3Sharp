using Sdl3Sharp.Internal.Interop;
using Sdl3Sharp.IO;
using Sdl3Sharp.SourceGeneration;
using Sdl3Sharp.Video.Blending;
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
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_ConvertPixelsAndColorspace">SDL_ConvertPixelsAndColorspace</seealso>
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
	/// Get the blend mode used for blit operations.
	/// </summary>
	/// <param name="surface">The <see href="https://wiki.libsdl.org/SDL3/SDL_Surface">SDL_Surface</see> structure to query</param>
	/// <param name="blendMode">A pointer filled in with the current <see href="https://wiki.libsdl.org/SDL3/SDL_BlendMode">SDL_BlendMode</see></param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetSurfaceBlendMode">SDL_GetSurfaceBlendMode</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_GetSurfaceBlendMode(SDL_Surface* surface, BlendMode* blendMode);

	/// <summary>
	/// Get the clipping rectangle for a surface
	/// </summary>
	/// <param name="surface">The <see href="https://wiki.libsdl.org/SDL3/SDL_Surface">SDL_Surface</see> structure representing the surface to be clipped</param>
	/// <param name="rect">An <see href="https://wiki.libsdl.org/SDL3/SDL_Rect">SDL_Rect</see> structure filled in with the clipping rectangle for the surface</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// When <c><paramref name="surface"/></c> is the destination of a blit, only the area within the clip rectangle is drawn into.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetSurfaceClipRect">SDL_GetSurfaceClipRect</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_GetSurfaceClipRect(SDL_Surface* surface, Rect<int>* rect);

	/// <summary>
	/// Get the color key (transparent pixel) for a surface
	/// </summary>
	/// <param name="surface">The <see href="https://wiki.libsdl.org/SDL3/SDL_Surface">SDL_Surface</see> structure to query</param>
	/// <param name="key">A pointer filled in with the transparent pixel</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// The color key is a pixel of the format used by the surface, as generated by <see href="https://wiki.libsdl.org/SDL3/SDL_MapRGB">SDL_MapRGB</see>().
	/// </para>
	/// <para>
	/// If the surface doesn't have color key enabled this function returns false.
	/// </para>
	/// </remarks>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_GetSurfaceColorKey(SDL_Surface* surface, uint* key);

	/// <summary>
	/// Get the additional color value multiplied into blit operations
	/// </summary>
	/// <param name="surface">The <see href="https://wiki.libsdl.org/SDL3/SDL_Surface">SDL_Surface</see> structure to query</param>
	/// <param name="r">A pointer filled in with the current red color value</param>
	/// <param name="g">A pointer filled in with the current green color value</param>
	/// <param name="b">A pointer filled in with the current blue color value</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetSurfaceColorMod">SDL_GetSurfaceColorMod</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_GetSurfaceColorMod(SDL_Surface* surface, byte* r, byte* g, byte* b);

	/// <summary>
	/// Get the colorspace used by a surface
	/// </summary>
	/// <param name="surface">The <see href="https://wiki.libsdl.org/SDL3/SDL_Surface">SDL_Surface</see> structure to query</param>
	/// <returns>Returns the colorspace used by the surface, or SDL_COLORSPACE_UNKNOWN if the surface is NULL</returns>
	/// <remarks>
	/// <para>
	/// The colorspace defaults to <see href="https://wiki.libsdl.org/SDL3/SDL_COLORSPACE_SRGB_LINEAR">SDL_COLORSPACE_SRGB_LINEAR</see> for floating point formats,
	/// <see href="https://wiki.libsdl.org/SDL3/SDL_COLORSPACE_HDR10">SDL_COLORSPACE_HDR10</see> for 10-bit formats,
	/// <see href="https://wiki.libsdl.org/SDL3/SDL_COLORSPACE_SRGB">SDL_COLORSPACE_SRGB</see> for other RGB surfaces
	/// and <see href="https://wiki.libsdl.org/SDL3/SDL_COLORSPACE_BT709_FULL">SDL_COLORSPACE_BT709_FULL</see> for YUV textures.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetSurfaceColorspace">SDL_GetSurfaceColorspace</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial ColorSpace SDL_GetSurfaceColorspace(SDL_Surface* surface);

	/// <summary>
	/// Get an array including all versions of a surface.
	/// </summary>
	/// <param name="surface">The <see href="https://wiki.libsdl.org/SDL3/SDL_Surface">SDL_Surface</see> structure to query</param>
	/// <param name="count">A pointer filled in with the number of surface pointers returned, may be NULL</param>
	/// <returns>
	/// Returns a NULL terminated array of <see href="https://wiki.libsdl.org/SDL3/SDL_Surface">SDL_Surface</see> pointers or NULL on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information.
	/// This should be freed with <see href="https://wiki.libsdl.org/SDL3/SDL_free">SDL_free</see>() when it is no longer needed.
	/// </returns>
	/// <remarks>
	/// <para>
	/// This returns all versions of a surface, with the surface being queried as the first element in the returned array.
	/// </para>
	/// <para>
	/// Freeing the array of surfaces does not affect the surfaces in the array. They are still referenced by the surface being queried and will be cleaned up normally.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetSurfaceImages">SDL_GetSurfaceImages</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial SDL_Surface** SDL_GetSurfaceImages(SDL_Surface* surface, int* count);

	/// <summary>
	/// Get the palette used by a surface
	/// </summary>
	/// <param name="surface">The <see href="https://wiki.libsdl.org/SDL3/SDL_Surface">SDL_Surface</see> structure to query</param>
	/// <returns>Returns a pointer to the palette used by the surface, or NULL if there is no palette used</returns>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetSurfacePalette"></seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial Palette.SDL_Palette* SDL_GetSurfacePalette(SDL_Surface* surface);

	/// <summary>
	/// Get the properties associated with a surface.
	/// </summary>
	/// <param name="surface">The <see href="https://wiki.libsdl.org/SDL3/SDL_Surface">SDL_Surface</see> structure to query</param>
	/// <returns>Returns a valid property ID on success or 0 on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// The following properties are understood by SDL:
	/// <list type="bullet">
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_SURFACE_SDR_WHITE_POINT_FLOAT"><c>SDL_PROP_SURFACE_SDR_WHITE_POINT_FLOAT</c></see></term>
	///			<description>
	///			For HDR10 and floating point surfaces, this defines the value of 100% diffuse white, with higher values being displayed in the High Dynamic Range headroom.
	///			This defaults to 203 for HDR10 surfaces and 1.0 for floating point surfaces.
	///			</description>
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_SURFACE_HDR_HEADROOM_FLOAT"><c>SDL_PROP_SURFACE_HDR_HEADROOM_FLOAT</c></see></term>
	///			<description>
	///			For HDR10 and floating point surfaces, this defines the maximum dynamic range used by the content, in terms of the SDR white point.
	///			This defaults to 0.0, which disables tone mapping.
	///			</description>
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_SURFACE_TONEMAP_OPERATOR_STRING"><c>SDL_PROP_SURFACE_TONEMAP_OPERATOR_STRING</c></see></term>
	///			<description>
	///			The tone mapping operator used when compressing from a surface with high dynamic range to another with lower dynamic range.
	///			Currently this supports "chrome", which uses the same tone mapping that Chrome uses for HDR content,
	///			the form "*=N", where N is a floating point scale factor applied in linear space,
	///			and "none", which disables tone mapping.
	///			This defaults to "chrome".
	///			</description>
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_SURFACE_HOTSPOT_X_NUMBER"><c>SDL_PROP_SURFACE_HOTSPOT_X_NUMBER</c></see></term>
	///			<description>
	///			The hotspot pixel offset from the left edge of the image, if this surface is being used as a cursor.
	///			</description>
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_SURFACE_HOTSPOT_Y_NUMBER"><c>SDL_PROP_SURFACE_HOTSPOT_Y_NUMBER</c></see></term>
	///			<description>
	///			The hotspot pixel offset from the top edge of the image, if this surface is being used as a cursor.
	///			</description>
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_SURFACE_ROTATION_FLOAT"><c>SDL_PROP_SURFACE_ROTATION_FLOAT</c></see></term>
	///			<description>
	///			The number of degrees a surface's data is meant to be rotated clockwise to make the image right-side up. Default 0.
	///			This is used by the camera API, if a mobile device is oriented differently than what its camera provides (i.e. - the camera always provides portrait images but the phone is being held in landscape orientation).
	///			Since SDL 3.4.0.
	///			</description>
	///		</item>
	/// </list>
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetSurfaceProperties">SDL_GetSurfaceProperties</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial uint SDL_GetSurfaceProperties(SDL_Surface* surface);

	/// <summary>
	/// Load a BMP image from a file
	/// </summary>
	/// <param name="file">The BMP file to load</param>
	/// <returns>Returns a pointer to a new <see href="https://wiki.libsdl.org/SDL3/SDL_Surface">SDL_Surface</see> structure or NULL on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// The new surface should be freed with <see href="https://wiki.libsdl.org/SDL3/SDL_DestroySurface">SDL_DestroySurface</see>(). Not doing so will result in a memory leak.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_LoadBMP">SDL_LoadBMP</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial SDL_Surface* SDL_LoadBMP(byte* file);

	/// <summary>
	/// Load a BMP image from a seekable SDL data stream
	/// </summary>
	/// <param name="src">The data stream for the surface</param>
	/// <param name="closeio">If true, calls <see href="https://wiki.libsdl.org/SDL3/SDL_CloseIO">SDL_CloseIO</see>() on src before returning, even in the case of an error</param>
	/// <returns>Returns a pointer to a new <see href="https://wiki.libsdl.org/SDL3/SDL_Surface">SDL_Surface</see> structure or NULL on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// The new surface should be freed with <see href="https://wiki.libsdl.org/SDL3/SDL_DestroySurface">SDL_DestroySurface</see>(). Not doing so will result in a memory leak.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_LoadBMP_IO">SDL_LoadBMP_IO</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial SDL_Surface* SDL_LoadBMP_IO(Stream.SDL_IOStream* src, CBool closeio);

#if SDL3_4_0_OR_GREATER
	/// <summary>
	/// Load a PNG image from a file
	/// </summary>
	/// <param name="file">The BMP file to load</param>
	/// <returns>Returns a pointer to a new <see href="https://wiki.libsdl.org/SDL3/SDL_Surface">SDL_Surface</see> structure or NULL on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// This is intended as a convenience function for loading images from trusted sources.
	/// If you want to load arbitrary images you should use libpng or another image loading library designed with security in mind.
	/// </para>
	/// <para>
	/// The new surface should be freed with <see href="https://wiki.libsdl.org/SDL3/SDL_DestroySurface">SDL_DestroySurface</see>(). Not doing so will result in a memory leak.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_LoadPNG">SDL_LoadPNG</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial SDL_Surface* SDL_LoadPNG(byte* file);
#endif

#if SDL3_4_0_OR_GREATER
	/// <summary>
	/// Load a PNG image from a seekable SDL data stream
	/// </summary>
	/// <param name="src">The data stream for the surface</param>
	/// <param name="closeio">If true, calls <see href="https://wiki.libsdl.org/SDL3/SDL_CloseIO">SDL_CloseIO</see>() on src before returning, even in the case of an error</param>
	/// <returns>Returns a pointer to a new <see href="https://wiki.libsdl.org/SDL3/SDL_Surface">SDL_Surface</see> structure or NULL on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// This is intended as a convenience function for loading images from trusted sources.
	/// If you want to load arbitrary images you should use libpng or another image loading library designed with security in mind.
	/// </para>
	/// <para>
	/// The new surface should be freed with <see href="https://wiki.libsdl.org/SDL3/SDL_DestroySurface">SDL_DestroySurface</see>(). Not doing so will result in a memory leak.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_LoadBMP_IO">SDL_LoadBMP_IO</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial SDL_Surface* SDL_LoadPNG_IO(Stream.SDL_IOStream* src, CBool closeio);
#endif

#if SDL3_4_0_OR_GREATER
	/// <summary>
	/// Load a BMP or PNG image from a file
	/// </summary>
	/// <param name="file">The file to load</param>
	/// <returns>Returns a pointer to a new <see href="https://wiki.libsdl.org/SDL3/SDL_Surface">SDL_Surface</see> structure or NULL on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// The new surface should be freed with <see href="https://wiki.libsdl.org/SDL3/SDL_DestroySurface">SDL_DestroySurface</see>(). Not doing so will result in a memory leak.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_LoadSurface">SDL_LoadSurface</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial SDL_Surface* SDL_LoadSurface(byte* file);
#endif

#if SDL_3_4_0_OR_GREATER
	/// <summary>
	/// Load a BMP or PNG image from a seekable SDL data stream
	/// </summary>
	/// <param name="src">The data stream for the surface</param>
	/// <param name="closeio">If true, calls <see href="https://wiki.libsdl.org/SDL3/SDL_CloseIO">SDL_CloseIO</see>() on src before returning, even in the case of an error</param>
	/// <returns>Returns a pointer to a new <see href="https://wiki.libsdl.org/SDL3/SDL_Surface">SDL_Surface</see> structure or NULL on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// The new surface should be freed with <see href="https://wiki.libsdl.org/SDL3/SDL_DestroySurface">SDL_DestroySurface</see>(). Not doing so will result in a memory leak.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_LoadSurface_IO">SDL_LoadSurface_IO</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial SDL_Surface* SDL_LoadSurface_IO(Stream.SDL_IOStream* src, CBool closeio);
#endif

	/// <summary>
	/// Set up a surface for directly accessing the pixels
	/// </summary>
	/// <param name="surface">The <see href="https://wiki.libsdl.org/SDL3/SDL_Surface">SDL_Surface</see> structure to be locked</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// Between calls to <see href="https://wiki.libsdl.org/SDL3/SDL_LockSurface">SDL_LockSurface</see>() / <see href="https://wiki.libsdl.org/SDL3/SDL_UnlockSurface">SDL_UnlockSurface</see>(), you can write to and read from <c><paramref name="surface"/>-&gt;pixels</c>, using the pixel format stored in <c><paramref name="surface"/>-&gt;format</c>.
	/// Once you are done accessing the surface, you should use <see href="https://wiki.libsdl.org/SDL3/SDL_UnlockSurface">SDL_UnlockSurface</see>() to release it.
	/// </para>
	/// <para>
	/// Not all surfaces require locking.
	/// If <c>SDL_MUSTLOCK(<paramref name="surface"/>)</c> evaluates to 0, then you can read and write to the surface at any time, and the pixel format of the surface will not change.
	/// </para>
	/// <para>
	/// The locking referred to by this function is making the pixels available for direct access, not thread-safe locking.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_LockSurface">SDL_LockSurface</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_LockSurface(SDL_Surface* surface);

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
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_MapSurfaceRGB">SDL_MapSurfaceRGB</seealso>
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
	/// Premultiply the alpha on a block of pixels
	/// </summary>
	/// <param name="width">The width of the block to convert, in pixels</param>
	/// <param name="height">Zhe height of the block to convert, in pixels</param>
	/// <param name="src_format">An <see href="https://wiki.libsdl.org/SDL3/SDL_PixelFormat">SDL_PixelFormat</see> value of the <c><paramref name="src"/></c> pixels format</param>
	/// <param name="src">A pointer to the source pixels</param>
	/// <param name="src_pitch">The pitch of the source pixels, in bytes</param>
	/// <param name="dst_format">An <see href="https://wiki.libsdl.org/SDL3/SDL_PixelFormat">SDL_PixelFormat</see> value of the <c><paramref name="dst"/></c> pixels format</param>
	/// <param name="dst">A pointer to be filled in with premultiplied pixel data</param>
	/// <param name="dst_pitch">The pitch of the destination pixels, in bytes</param>
	/// <param name="linear">true to convert from sRGB to linear space for the alpha multiplication, false to do multiplication in sRGB space</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// This is safe to use with src == dst, but not for other overlapping areas.
	/// </para>
	/// <para>
	/// The same destination pixels should not be used from two threads at once. It is safe to use the same source pixels from multiple threads.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_PremultiplyAlpha">SDL_PremultiplyAlpha</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_PremultiplyAlpha(int width, int height, PixelFormat src_format, void* src, int src_pitch, PixelFormat dst_format, void* dst, int dst_pitch, CBool linear);

	/// <summary>
	/// Premultiply the alpha in a surface
	/// </summary>
	/// <param name="surface">The surface to modify</param>
	/// <param name="linear">true to convert from sRGB to linear space for the alpha multiplication, false to do multiplication in sRGB space</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// This is safe to use with src == dst, but not for other overlapping areas.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_PremultiplySurfaceAlpha">SDL_PremultiplySurfaceAlpha</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_PremultiplySurfaceAlpha(SDL_Surface* surface, CBool linear);

	/// <summary>
	/// Retrieves a single pixel from a surface
	/// </summary>
	/// <param name="surface">The surface to read</param>
	/// <param name="x">The horizontal coordinate, 0 &lt;= x &lt; width</param>
	/// <param name="y">The vertical coordinate, 0 &lt;= y &lt; height</param>
	/// <param name="r">A pointer filled in with the red channel, 0-255, or NULL to ignore this channel</param>
	/// <param name="g">A pointer filled in with the green channel, 0-255, or NULL to ignore this channel</param>
	/// <param name="b">A pointer filled in with the blue channel, 0-255, or NULL to ignore this channel</param>
	/// <param name="a">A pointer filled in with the alpha channel, 0-255, or NULL to ignore this channel</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// This function prioritizes correctness over speed: it is suitable for unit tests, but is not intended for use in a game engine.
	/// </para>
	/// <para>
	/// Like <see href="https://wiki.libsdl.org/SDL3/SDL_GetRGBA">SDL_GetRGBA</see>, this uses the entire 0..255 range when converting color components from pixel formats with less than 8 bits per RGB component.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_ReadSurfacePixel">SDL_ReadSurfacePixel</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_ReadSurfacePixel(SDL_Surface* surface, int x, int y, byte* r, byte* g, byte* b, byte* a);

	/// <summary>
	/// Retrieves a single pixel from a surface
	/// </summary>
	/// <param name="surface">The surface to read</param>
	/// <param name="x">The horizontal coordinate, 0 &lt;= x &lt; width</param>
	/// <param name="y">The vertical coordinate, 0 &lt;= y &lt; height</param>
	/// <param name="r">A pointer filled in with the red channel, 0-1, or NULL to ignore this channel</param>
	/// <param name="g">A pointer filled in with the green channel, 0-1, or NULL to ignore this channel</param>
	/// <param name="b">A pointer filled in with the blue channel, 0-1, or NULL to ignore this channel</param>
	/// <param name="a">A pointer filled in with the alpha channel, 0-1, or NULL to ignore this channel</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// This function prioritizes correctness over speed: it is suitable for unit tests, but is not intended for use in a game engine.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_ReadSurfacePixelFloat">SDL_ReadSurfacePixelFloat</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_ReadSurfacePixelFloat(SDL_Surface* surface, int x, int y, float* r, float* g, float* b, float* a);

	/// <summary>
	/// Remove all alternate versions of a surface
	/// </summary>
	/// <param name="surface">The <see href="https://wiki.libsdl.org/SDL3/SDL_Surface">SDL_Surface</see> structure to update</param>
	/// <remarks>
	/// <para>
	/// This function removes a reference from all the alternative versions, destroying them if this is the last reference to them.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_RemoveSurfaceAlternateImages">SDL_RemoveSurfaceAlternateImages</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial void SDL_RemoveSurfaceAlternateImages(SDL_Surface* surface);

#if SDL3_4_0_OR_GREATER
	/// <summary>
	/// Return a copy of a surface rotated clockwise a number of degrees
	/// </summary>
	/// <param name="surface">The surface to rotate</param>
	/// <param name="angle">The rotation angle, in degrees</param>
	/// <returns>Returns a rotated copy of the surface or NULL on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// The angle of rotation can be negative for counter-clockwise rotation.
	/// </para>
	/// <para>
	/// When the rotation isn't a multiple of 90 degrees, the resulting surface is larger than the original, with the background filled in with the colorkey, if available, or RGBA 255/255/255/0 if not.
	/// </para>
	/// <para>
	/// If <c><paramref name="surface"/></c> has the <see href="">SDL_PROP_SURFACE_ROTATION_FLOAT</see> property set on it, the new copy will have the adjusted value set:
	/// if the rotation property is 90 and <c><paramref name="angle"/></c> was 30, the new surface will have a property value of 60 (that is: to be upright vs gravity, this surface needs to rotate 60 more degrees).
	/// However, note that further rotations on the new surface in this example will produce unexpected results, since the image will have resized and padded to accommodate the not-90 degree angle.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_RotateSurface"></seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial SDL_Surface* SDL_RotateSurface(SDL_Surface* surface, float angle);
#endif

	/// <summary>
	/// Save a surface to a file in BMP format
	/// </summary>
	/// <param name="surface">The <see href="https://wiki.libsdl.org/SDL3/SDL_Surface">SDL_Surface</see> structure containing the image to be saved</param>
	/// <param name="file">A file to save to</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// Surfaces with a 24-bit, 32-bit and paletted 8-bit format get saved in the BMP directly.
	/// Other RGB formats with 8-bit or higher get converted to a 24-bit surface or, if they have an alpha mask or a colorkey, to a 32-bit surface before they are saved.
	/// YUV and paletted 1-bit and 4-bit formats are not supported.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_SaveBMP">SDL_SaveBMP</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_SaveBMP(SDL_Surface* surface, byte* file);

	/// <summary>
	/// Save a surface to a seekable SDL data stream in BMP format
	/// </summary>
	/// <param name="surface">The <see href="https://wiki.libsdl.org/SDL3/SDL_Surface">SDL_Surface</see> structure containing the image to be saved</param>
	/// <param name="dst">A data stream to save to</param>
	/// <param name="closeio">if true, calls <see href="">SDL_CloseIO</see>() on <c><paramref name="dst"/></c> before returning, even in the case of an error</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// Surfaces with a 24-bit, 32-bit and paletted 8-bit format get saved in the BMP directly.
	/// Other RGB formats with 8-bit or higher get converted to a 24-bit surface or, if they have an alpha mask or a colorkey, to a 32-bit surface before they are saved.
	/// YUV and paletted 1-bit and 4-bit formats are not supported.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_SaveBMP_IO">SDL_SaveBMP_IO</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_SaveBMP_IO(SDL_Surface* surface, Stream.SDL_IOStream* dst, CBool closeio);

#if SDL3_4_0_OR_GREATER
	/// <summary>
	/// Save a surface to a file in PNG format
	/// </summary>
	/// <param name="surface">The <see href="https://wiki.libsdl.org/SDL3/SDL_Surface">SDL_Surface</see> structure containing the image to be saved</param>
	/// <param name="file">A file to save to</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_SavePNG">SDL_SavePNG</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_SavePNG(SDL_Surface* surface, byte* file);
#endif

#if SDL3_4_0_OR_GREATER
	/// <summary>
	/// Save a surface to a seekable SDL data stream in PNG format
	/// </summary>
	/// <param name="surface">The <see href="https://wiki.libsdl.org/SDL3/SDL_Surface">SDL_Surface</see> structure containing the image to be saved</param>
	/// <param name="dst">A data stream to save to</param>
	/// <param name="closeio">if true, calls <see href="">SDL_CloseIO</see>() on <c><paramref name="dst"/></c> before returning, even in the case of an error</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_SavePNG_IO">SDL_SavePNG_IO</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_SavePNG_IO(SDL_Surface* surface, Stream.SDL_IOStream* dst, CBool closeio);
#endif

	/// <summary>
	/// Creates a new surface identical to the existing surface, scaled to the desired size
	/// </summary>
	/// <param name="surface">The surface to duplicate and scale</param>
	/// <param name="width">The width of the new surface</param>
	/// <param name="height">The height of the new surface</param>
	/// <param name="scaleMode">The <see href="https://wiki.libsdl.org/SDL3/SDL_ScaleMode">SDL_ScaleMode</see> to be used</param>
	/// <returns>Returns a copy of the surface or NULL on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// The returned surface should be freed with <see href="https://wiki.libsdl.org/SDL3/SDL_DestroySurface">SDL_DestroySurface()</see>.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_ScaleSurface">SDL_ScaleSurface</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial SDL_Surface* SDL_ScaleSurface(SDL_Surface* surface, int width, int height, ScaleMode scaleMode);

	/// <summary>
	/// Set an additional alpha value used in blit operations
	/// </summary>
	/// <param name="surface">The <see href="https://wiki.libsdl.org/SDL3/SDL_Surface">SDL_Surface</see> structure to update</param>
	/// <param name="alpha">The alpha value multiplied into blit operations</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
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

	/// <summary>
	/// Set the blend mode used for blit operations
	/// </summary>
	/// <param name="surface">The <see href="https://wiki.libsdl.org/SDL3/SDL_Surface">SDL_Surface</see> structure to update</param>
	/// <param name="blendMode">The <see href="https://wiki.libsdl.org/SDL3/SDL_BlendMode">SDL_BlendMode</see> to use for blit blending</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// To copy a surface to another surface (or texture) without blending with the existing data, the blendmode of the SOURCE surface should be set to <see href="https://wiki.libsdl.org/SDL3/SDL_BLENDMODE_NONE"><c>SDL_BLENDMODE_NONE</c></see>.
	/// </para> 
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_SetSurfaceBlendMode">SDL_SetSurfaceBlendMode</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_SetSurfaceBlendMode(SDL_Surface* surface, BlendMode blendMode);

	/// <summary>
	/// Set the clipping rectangle for a surface
	/// </summary>
	/// <param name="surface">The <see href="https://wiki.libsdl.org/SDL3/SDL_Surface">SDL_Surface</see> structure to be clipped</param>
	/// <param name="rect">The <see href="https://wiki.libsdl.org/SDL3/SDL_Rect">SDL_Rect</see> structure representing the clipping rectangle, or NULL to disable clipping</param>
	/// <returns>Returns true if the rectangle intersects the surface, otherwise false and blits will be completely clipped</returns>
	/// <remarks>
	/// <para>
	/// When <c><paramref name="surface"/></c> is the destination of a blit, only the area within the clip rectangle is drawn into.
	/// </para>
	/// <para>
	/// Note that blits are automatically clipped to the edges of the source and destination surfaces.
	/// </para>
	/// </remarks>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_SetSurfaceClipRect(SDL_Surface* surface, Rect<int>* rect);

	/// <summary>
	/// Set the color key (transparent pixel) in a surface
	/// </summary>
	/// <param name="surface">The <see href="https://wiki.libsdl.org/SDL3/SDL_Surface">SDL_Surface</see> structure to update</param>
	/// <param name="enabled">true to enable color key, false to disable color key</param>
	/// <param name="key">the transparent pixel</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// The color key defines a pixel value that will be treated as transparent in a blit. For example, one can use this to specify that cyan pixels should be considered transparent, and therefore not rendered.
	/// </para>
	/// <para>
	/// It is a pixel of the format used by the surface, as generated by <see href="https://wiki.libsdl.org/SDL3/SDL_MapRGB">SDL_MapRGB</see>().
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_SetSurfaceColorKey">SDL_SetSurfaceColorKey</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_SetSurfaceColorKey(SDL_Surface* surface, CBool enabled, uint key);

	/// <summary>
	/// Set an additional color value multiplied into blit operations
	/// </summary>
	/// <param name="surface">The <see href="https://wiki.libsdl.org/SDL3/SDL_Surface">SDL_Surface</see> structure to update</param>
	/// <param name="r">The red color value multiplied into blit operations</param>
	/// <param name="g">The green color value multiplied into blit operations</param>
	/// <param name="b">The blue color value multiplied into blit operations</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// When this surface is blitted, during the blit operation each source color channel is modulated by the appropriate color value according to the following formula:
	/// <code>
	/// srcC = srcC * (color / 255)
	/// </code>
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_SetSurfaceColorMod">SDL_SetSurfaceColorMod</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_SetSurfaceColorMod(SDL_Surface* surface, byte r, byte g, byte b);

	/// <summary>
	/// Set the colorspace used by a surface
	/// </summary>
	/// <param name="surface">The <see href="https://wiki.libsdl.org/SDL3/SDL_Surface">SDL_Surface</see> structure to update</param>
	/// <param name="colorspace">An <see href="https://wiki.libsdl.org/SDL3/SDL_Colorspace">SDL_Colorspace</see> value describing the surface colorspace</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// Setting the colorspace doesn't change the pixels, only how they are interpreted in color operations.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_SetSurfaceColorspace">SDL_SetSurfaceColorspace</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_SetSurfaceColorspace(SDL_Surface* surface, ColorSpace colorspace);

	/// <summary>
	/// Set the palette used by a surface
	/// </summary>
	/// <param name="surface">The <see href="https://wiki.libsdl.org/SDL3/SDL_Surface">SDL_Surface</see> structure to update</param>
	/// <param name="palette">The <see href="https://wiki.libsdl.org/SDL3/SDL_Palette">SDL_Palette</see> structure to use</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// Setting the palette keeps an internal reference to the palette, which can be safely destroyed afterwards.
	/// </para>
	/// <para>
	/// A single palette can be shared with many surfaces.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_SetSurfacePalette">SDL_SetSurfacePalette</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_SetSurfacePalette(SDL_Surface* surface, Palette.SDL_Palette* palette);

	/// <summary>
	/// Set the RLE acceleration hint for a surface.
	/// </summary>
	/// <param name="surface">The <see href="https://wiki.libsdl.org/SDL3/SDL_Surface">SDL_Surface</see> structure to optimize</param>
	/// <param name="enabled">true to enable RLE acceleration, false to disable it</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks> 
	/// <para>
	/// If RLE is enabled, color key and alpha blending blits are much faster, but the surface must be locked before directly accessing the pixels.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_SetSurfaceRLE">SDL_SetSurfaceRLE</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_SetSurfaceRLE(SDL_Surface* surface, CBool enabled);

#if SDL3_4_0_OR_GREATER
	/// <summary>
	/// Perform a stretched pixel copy from one surface to another
	/// </summary>
	/// <param name="src">The <see href="https://wiki.libsdl.org/SDL3/SDL_Surface">SDL_Surface</see> structure to be copied from</param>
	/// <param name="srcrect">The <see href="https://wiki.libsdl.org/SDL3/SDL_Rect">SDL_Rect</see> structure representing the rectangle to be copied, or NULL to copy the entire surface</param>
	/// <param name="dst">The <see href="https://wiki.libsdl.org/SDL3/SDL_Surface">SDL_Surface</see> structure that is the blit target</param>
	/// <param name="dstRect">The <see href="https://wiki.libsdl.org/SDL3/SDL_Rect">SDL_Rect</see> structure representing the target rectangle in the destination surface, or NULL to fill the entire destination surface</param>
	/// <param name="scaleMode">The <see href="https://wiki.libsdl.org/SDL3/SDL_ScaleMode">SDL_ScaleMode</see> to be used</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// Only one thread should be using the <c><paramref name="src"/></c> and <c><paramref name="dst"/></c> surfaces at any given time.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_StretchSurface">SDL_StretchSurface</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_StretchSurface(SDL_Surface* src, Rect<int>* srcrect, SDL_Surface* dst, Rect<int>* dstrect, ScaleMode scaleMode);
#endif

	/// <summary>
	/// Return whether a surface has alternate versions available
	/// </summary>
	/// <param name="surface">The <see href="https://wiki.libsdl.org/SDL3/SDL_Surface">SDL_Surface</see> structure to query</param>
	/// <returns>Returns true if alternate versions are available or false otherwise</returns>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_SurfaceHasAlternateImages">SDL_SurfaceHasAlternateImages</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_SurfaceHasAlternateImages(SDL_Surface* surface);

	/// <summary>
	/// Returns whether the surface has a color key.
	/// </summary>
	/// <param name="surface">The <see href="https://wiki.libsdl.org/SDL3/SDL_Surface">SDL_Surface</see> structure to query</param>
	/// <returns>Returns true if the surface has a color key, false otherwise</returns>
	/// <remarks>
	/// <para>
	/// It is safe to pass a NULL <c><paramref name="surface"/></c> here; it will return false.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_SurfaceHasColorKey">SDL_SurfaceHasColorKey</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_SurfaceHasColorKey(SDL_Surface* surface);

	/// <summary>
	/// Returns whether the surface is RLE enabled
	/// </summary>
	/// <param name="surface">The <see href="https://wiki.libsdl.org/SDL3/SDL_Surface">SDL_Surface</see> structure to query</param>
	/// <returns>Returns true if the surface is RLE enabled, false otherwise</returns>
	/// <remarks>
	/// <para>
	/// It is safe to pass a NULL <c><paramref name="surface"/></c> here; it will return false.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_SurfaceHasRLE"></seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_SurfaceHasRLE(SDL_Surface* surface);

	/// <summary>
	/// Release a surface after directly accessing the pixels
	/// </summary>
	/// <param name="surface">The <see href="https://wiki.libsdl.org/SDL3/SDL_Surface">SDL_Surface</see> structure to be unlocked</param>
	/// <remarks>
	/// <para>
	/// This function is not thread safe. The locking referred to by this function is making the pixels available for direct access, not thread-safe locking.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_UnlockSurface">SDL_UnlockSurface</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial void SDL_UnlockSurface(SDL_Surface* surface);

	/// <summary>
	/// Writes a single pixel to a surface
	/// </summary>
	/// <param name="surface">The surface to write</param>
	/// <param name="x">The horizontal coordinate, 0 &lt;= x &lt; width</param>
	/// <param name="y">The vertical coordinate, 0 &lt;= y &lt; height</param>
	/// <param name="r">The red channel value, 0-255</param>
	/// <param name="g">The green channel value, 0-255</param>
	/// <param name="b">The blue channel value, 0-255</param>
	/// <param name="a">The alpha channel value, 0-255</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// This function prioritizes correctness over speed: it is suitable for unit tests, but is not intended for use in a game engine.
	/// </para>
	/// <para>
	/// Like <see href="https://wiki.libsdl.org/SDL3/SDL_MapRGBA">SDL_MapRGBA</see>, this uses the entire 0..255 range when converting color components from pixel formats with less than 8 bits per RGB component.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_WriteSurfacePixel">SDL_WriteSurfacePixel</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_WriteSurfacePixel(SDL_Surface* surface, int x, int y, byte r, byte g, byte b, byte a);

	/// <summary>
	/// Writes a single pixel to a surface
	/// </summary>
	/// <param name="surface">The surface to write</param>
	/// <param name="x">The horizontal coordinate, 0 &lt;= x &lt; width</param>
	/// <param name="y">The vertical coordinate, 0 &lt;= y &lt; height</param>
	/// <param name="r">The red channel value, 0-1</param>
	/// <param name="g">The green channel value, 0-1</param>
	/// <param name="b">The blue channel value, 0-1</param>
	/// <param name="a">The alpha channel value, 0-1</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// This function prioritizes correctness over speed: it is suitable for unit tests, but is not intended for use in a game engine.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_WriteSurfacePixelFloat">SDL_WriteSurfacePixelFloat</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_WriteSurfacePixelFloat(SDL_Surface* surface, int x, int y, float r, float g, float b, float a);
}
