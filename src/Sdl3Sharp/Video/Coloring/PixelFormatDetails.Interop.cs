using Sdl3Sharp.Internal.Interop;
using Sdl3Sharp.SourceGeneration;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sdl3Sharp.Video.Coloring;

partial struct PixelFormatDetails
{
	[StructLayout(LayoutKind.Sequential)]
	internal readonly struct SDL_PixelFormatDetails
	{
		public readonly PixelFormat Format;
		public readonly byte BitsPerPixel;
		public readonly byte BytesPerPixel;
		private readonly Padding mPadding;
		public readonly uint RMask;
		public readonly uint GMask;
		public readonly uint BMask;
		public readonly uint AMask;
		public readonly byte RBits;
		public readonly byte GBits;
		public readonly byte BBits;
		public readonly byte ABits;
		public readonly byte RShift;
		public readonly byte GShift;
		public readonly byte BShift;
		public readonly byte AShift;
		
		[InlineArray(2)]
		private struct Padding { private byte _; }
	}

	/// <summary>
	/// Get RGB values from a pixel in the specified format
	/// </summary>
	/// <param name="pixelvalue">A pixel value</param>
	/// <param name="format">A pointer to <see href="https://wiki.libsdl.org/SDL3/SDL_PixelFormatDetails">SDL_PixelFormatDetails</see> describing the pixel format</param>
	/// <param name="palette">An optional palette for indexed formats, may be NULL</param>
	/// <param name="r">A pointer filled in with the red component, may be NULL</param>
	/// <param name="g">A pointer filled in with the green component, may be NULL</param>
	/// <param name="b">A pointer filled in with the blue component, may be NULL</param>
	/// <remarks>
	/// <para>
	/// This function uses the entire 8-bit [0..255] range when converting color components from pixel formats with less than 8-bits per RGB component (e.g., a completely white pixel in 16-bit RGB565 format would return [0xff, 0xff, 0xff] not [0xf8, 0xfc, 0xf8]).
	/// </para>
	/// <para>
	/// It is safe to call this function from any thread, as long as the palette is not modified.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetRGBA">SDL_GetRGBA</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial void SDL_GetRGB(uint pixelvalue, SDL_PixelFormatDetails* format, Palette.SDL_Palette* palette, byte *r, byte* g, byte* b);

	/// <summary>
	/// Get RGBA values from a pixel in the specified format
	/// </summary>
	/// <param name="pixelvalue">A pixel value</param>
	/// <param name="format">A pointer to <see href="https://wiki.libsdl.org/SDL3/SDL_PixelFormatDetails">SDL_PixelFormatDetails</see> describing the pixel format</param>
	/// <param name="palette">An optional palette for indexed formats, may be NULL</param>
	/// <param name="r">A pointer filled in with the red component, may be NULL</param>
	/// <param name="g">A pointer filled in with the green component, may be NULL</param>
	/// <param name="b">A pointer filled in with the blue component, may be NULL</param>
	/// <param name="a">A pointer filled in with the alpha component, may be NULL</param>
	/// <remarks>
	/// <para>
	/// This function uses the entire 8-bit [0..255] range when converting color components from pixel formats with less than 8-bits per RGB component (e.g., a completely white pixel in 16-bit RGB565 format would return [0xff, 0xff, 0xff] not [0xf8, 0xfc, 0xf8]).
	/// </para>
	/// <para>
	/// If the surface has no alpha component, the alpha will be returned as 0xff (100% opaque).
	/// </para>
	/// <para>
	/// It is safe to call this function from any thread, as long as the palette is not modified.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetRGBA">SDL_GetRGBA</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial void SDL_GetRGBA(uint pixelvalue, SDL_PixelFormatDetails* format, Palette.SDL_Palette* palette, byte *r, byte* g, byte* b, byte* a);

	/// <summary>
	/// Map an RGB triple to an opaque pixel value for a given pixel format
	/// </summary>
	/// <param name="format">A pointer to <see href="https://wiki.libsdl.org/SDL3/SDL_PixelFormatDetails">SDL_PixelFormatDetails</see> describing the pixel format</param>
	/// <param name="palette">An optional palette for indexed formats, may be NULL</param>
	/// <param name="r">The red component of the pixel in the range 0-255</param>
	/// <param name="g">The green component of the pixel in the range 0-255</param>
	/// <param name="b">The blue component of the pixel in the range 0-255</param>
	/// <returns>Returns a pixel value</returns>
	/// <remarks>
	/// <para>
	/// This function maps the RGB color value to the specified pixel format and returns the pixel value best approximating the given RGB color value for the given pixel format.
	/// </para>
	/// <para>
	/// If the format has a palette (8-bit) the index of the closest matching color in the palette will be returned.
	/// </para>
	/// <para>
	/// If the specified pixel format has an alpha component it will be returned as all 1 bits (fully opaque).
	/// </para>
	/// <para>
	/// If the pixel format bpp (color depth) is less than 32-bpp then the unused upper bits of the return value can safely be ignored
	/// (e.g., with a 16-bpp format the return value can be assigned to a <see href="https://wiki.libsdl.org/SDL3/Uint16">Uint16</see>, and similarly a Uint8 for an 8-bpp format).
	/// </para>
	/// <para>
	/// It is safe to call this function from any thread, as long as the palette is not modified.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_MapRGB">SDL_MapRGB</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial uint SDL_MapRGB(SDL_PixelFormatDetails* format, Palette.SDL_Palette* palette, byte r, byte g, byte b);

	/// <summary>
	/// Map an RGBA quadruple to a pixel value for a given pixel format
	/// </summary>
	/// <param name="format">A pointer to <see href="https://wiki.libsdl.org/SDL3/SDL_PixelFormatDetails">SDL_PixelFormatDetails</see> describing the pixel format</param>
	/// <param name="palette">An optional palette for indexed formats, may be NULL</param>
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
	/// If the specified pixel format has no alpha component the alpha value will be ignored (as it will be in formats with a palette).
	/// </para>
	/// <para>
	/// If the format has a palette (8-bit) the index of the closest matching color in the palette will be returned.
	/// </para>
	/// <para>
	/// If the pixel format bpp (color depth) is less than 32-bpp then the unused upper bits of the return value can safely be ignored
	/// (e.g., with a 16-bpp format the return value can be assigned to a <see href="https://wiki.libsdl.org/SDL3/Uint16">Uint16</see>, and similarly a Uint8 for an 8-bpp format).
	/// </para>
	/// <para>
	/// It is safe to call this function from any thread, as long as the palette is not modified.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_MapRGBA">SDL_MapRGBA</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial uint SDL_MapRGBA(SDL_PixelFormatDetails* format, Palette.SDL_Palette* palette, byte r, byte g, byte b, byte a);
}
