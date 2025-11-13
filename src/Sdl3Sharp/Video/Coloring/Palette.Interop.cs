using Sdl3Sharp.Internal.Interop;
using Sdl3Sharp.SourceGeneration;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sdl3Sharp.Video.Coloring;

partial class Palette
{
	[StructLayout(LayoutKind.Sequential)]
	internal readonly struct SDL_Palette
	{
		public readonly int NColors;
		public unsafe readonly Color<byte>* Colors;
		public readonly uint Version;
		public readonly int RefCount;
	}

	/// <summary>
	/// Create a palette structure with the specified number of color entries
	/// </summary>
	/// <param name="ncolors">Represents the number of color entries in the color palette</param>
	/// <returns>Returns a new <see href="https://wiki.libsdl.org/SDL3/SDL_Palette">SDL_Palette</see> structure on success or NULL on failure (e.g. if there wasn't enough memory); call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// The palette entries are initialized to white.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_CreatePalette">SDL_CreatePalette</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial SDL_Palette* SDL_CreatePalette(int ncolors);

	/// <summary>
	/// Free a palette created with <see href="https://wiki.libsdl.org/SDL3/SDL_CreatePalette">SDL_CreatePalette</see>()
	/// </summary>
	/// <param name="palette">The <see href="https://wiki.libsdl.org/SDL3/SDL_Palette">SDL_Palette</see> structure to be freed.</param>
	/// <remarks>
	/// <para>
	/// It is safe to call this function from any thread, as long as the palette is not modified or destroyed in another thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_DestroyPalette">SDL_DestroyPalette</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial void SDL_DestroyPalette(SDL_Palette* palette);

	/// <summary>
	/// Set a range of colors in a palette
	/// </summary>
	/// <param name="palette">The <see href="https://wiki.libsdl.org/SDL3/SDL_Palette">SDL_Palette</see> structure to modify</param>
	/// <param name="colors">An array of <see href="https://wiki.libsdl.org/SDL3/SDL_Color">SDL_Color</see> structures to copy into the palette</param>
	/// <param name="firstcolor">The index of the first palette entry to modify</param>
	/// <param name="ncolors">The number of entries to modify</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// It is safe to call this function from any thread, as long as the palette is not modified or destroyed in another thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_SetPaletteColors">SDL_SetPaletteColors</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_SetPaletteColors(SDL_Palette* palette, Color<byte>* colors, int firstcolor, int ncolors);
}
