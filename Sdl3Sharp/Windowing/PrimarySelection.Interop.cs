using Sdl3Sharp.Internal.Interop;
using Sdl3Sharp.SourceGeneration;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Windowing;

partial class PrimarySelection
{
	/// <summary>
	/// Get UTF-8 text from the primary selection
	/// </summary>
	/// <returns>
	/// Returns the primary selection text on success or an empty string on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information.
	/// This should be freed with <see href="https://wiki.libsdl.org/SDL3/SDL_free">SDL_free</see>() when it is no longer needed.
	/// </returns>
	/// <remarks>
	/// This functions returns an empty string if there was not enough memory left for a copy of the primary selection's content
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetPrimarySelectionText">SDL_GetPrimarySelectionText</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial byte* SDL_GetPrimarySelectionText();	

	/// <summary>
	/// Query whether the primary selection exists and contains a non-empty text string
	/// </summary>
	/// <returns>Returns true if the primary selection has text, or false if it does not</returns>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_HasPrimarySelectionText">SDL_HasPrimarySelectionText</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal static partial CBool SDL_HasPrimarySelectionText();

	/// <summary>
	/// Put UTF-8 text into the primary selection
	/// </summary>
	/// <param name="text">the text to store in the primary selection</param>
	/// <returns> Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_SetPrimarySelectionText">SDL_SetPrimarySelectionText</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_SetPrimarySelectionText(byte* text);
}
