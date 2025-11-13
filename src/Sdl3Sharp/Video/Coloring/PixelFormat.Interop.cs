using Sdl3Sharp.Internal.Interop;
using Sdl3Sharp.SourceGeneration;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Video.Coloring;

partial struct PixelFormat
{
	/// <summary>
	/// Convert one of the enumerated pixel formats to a bpp value and RGBA masks
	/// </summary>
	/// <param name="format">One of the <see href="https://wiki.libsdl.org/SDL3/SDL_PixelFormat">SDL_PixelFormat</see> values</param>
	/// <param name="bpp">A bits per pixel value; usually 15, 16, or 32</param>
	/// <param name="Rmask">A pointer filled in with the red mask for the format</param>
	/// <param name="Gmask">A pointer filled in with the green mask for the format</param>
	/// <param name="Bmask">A pointer filled in with the blue mask for the format</param>
	/// <param name="Amask">A pointer filled in with the alpha mask for the format</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetMasksForPixelFormat">SDL_GetMasksForPixelFormat</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_GetMasksForPixelFormat(PixelFormat format, int* bpp, uint* Rmask, uint* Gmask, uint* Bmask, uint* Amask);

	/// <summary>
	/// Create an <see href="https://wiki.libsdl.org/SDL3/SDL_PixelFormatDetails">SDL_PixelFormatDetails</see> structure corresponding to a pixel format
	/// </summary>
	/// <param name="format">One of the <see href="https://wiki.libsdl.org/SDL3/SDL_PixelFormat">SDL_PixelFormat</see> values</param>
	/// <returns>Returns a pointer to a <see href="https://wiki.libsdl.org/SDL3/SDL_PixelFormatDetails">SDL_PixelFormatDetails</see> structure or NULL on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// Returned structure may come from a shared global cache (i.e. not newly allocated), and hence should not be modified, especially the palette.
	/// Weird errors such as <c>Blit combination not supported</c> may occur.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetPixelFormatDetails">SDL_GetPixelFormatDetails</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial PixelFormatDetails.SDL_PixelFormatDetails* SDL_GetPixelFormatDetails(PixelFormat format);

	/// <summary>
	/// Convert a bpp value and RGBA masks to an enumerated pixel format
	/// </summary>
	/// <param name="bpp">A bits per pixel value; usually 15, 16, or 32</param>
	/// <param name="Rmask">The red mask for the format</param>
	/// <param name="Gmask">The green mask for the format</param>
	/// <param name="Bmask">The blue mask for the format</param>
	/// <param name="Amask">The alpha mask for the format</param>
	/// <returns>Returns the <see href="https://wiki.libsdl.org/SDL3/SDL_PixelFormat">SDL_PixelFormat</see> value corresponding to the format masks, or <see href="https://wiki.libsdl.org/SDL3/SDL_PIXELFORMAT_UNKNOWN">SDL_PIXELFORMAT_UNKNOWN</see> if there isn't a match</returns>
	/// <remarks>
	/// <para>
	/// This will return <see href="https://wiki.libsdl.org/SDL3/SDL_PIXELFORMAT_UNKNOWN"><c>SDL_PIXELFORMAT_UNKNOWN</c></see> if the conversion wasn't possible.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetPixelFormatForMasks">SDL_GetPixelFormatForMasks</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal static partial PixelFormat SDL_GetPixelFormatForMasks(int bpp, uint Rmask, uint Gmask, uint Bmask, uint Amask);

	/// <summary>
	/// Get the human readable name of a pixel format
	/// </summary>
	/// <param name="format">The pixel format to query</param>
	/// <returns>Returns the human readable name of the specified pixel format or "<see href="https://wiki.libsdl.org/SDL3/SDL_PIXELFORMAT_UNKNOWN">SDL_PIXELFORMAT_UNKNOWN</see>" if the format isn't recognized</returns>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetPixelFormatName">SDL_GetPixelFormatName</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial byte* SDL_GetPixelFormatName(PixelFormat format);
}
