using Sdl3Sharp.Internal.Interop;
using Sdl3Sharp.SourceGeneration;
using System;
using System.Runtime.CompilerServices;
using unsafe SDL_ClipboardCleanupCallback = delegate* unmanaged[Cdecl]<void*, void>;
using unsafe SDL_ClipboardDataCallback = delegate* unmanaged[Cdecl]<void*, byte*, System.UIntPtr*, void*>;

namespace Sdl3Sharp.Windowing;

partial class Clipboard
{
	/// <summary>
	/// Clear the clipboard data
	/// </summary>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_ClearClipboardData">SDL_ClearClipboardData</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal static partial CBool SDL_ClearClipboardData();

	/// <summary>
	/// Get the data from clipboard for a given mime type
	/// </summary>
	/// <param name="mime_type">the mime type to read from the clipboard</param>
	/// <param name="size">a pointer filled in with the length of the returned data</param>
	/// <returns>
	/// Returns the retrieved data buffer or NULL on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information.
	/// This should be freed with <see href="https://wiki.libsdl.org/SDL3/SDL_free">SDL_free</see>() when it is no longer needed.
	/// </returns>
	/// <remarks>
	/// The size of text data does not include the terminator, but the text is guaranteed to be null terminated
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetClipboardData">SDL_GetClipboardData</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial void* SDL_GetClipboardData(byte* mime_type, UIntPtr* size);

	/// <summary>
	/// Retrieve the list of mime types available in the clipboard
	/// </summary>
	/// <param name="num_mime_types">a pointer filled with the number of mime types, may be NULL</param>
	/// <returns>
	/// Returns a null terminated array of strings with mime types, or NULL on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information.
	/// This should be freed with <see href="https://wiki.libsdl.org/SDL3/SDL_free">SDL_free</see>() when it is no longer needed.
	/// </returns>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetClipboardMimeTypes">SDL_GetClipboardMimeTypes</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial byte** SDL_GetClipboardMimeTypes(UIntPtr* num_mime_types);

	/// <summary>
	/// Get UTF-8 text from the clipboard
	/// </summary>
	/// <returns>
	/// Returns the clipboard text on success or an empty string on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information.
	/// This should be freed with <see href="https://wiki.libsdl.org/SDL3/SDL_free">SDL_free</see>() when it is no longer needed.
	/// </returns>
	/// <remarks>
	/// This functions returns an empty string if there was not enough memory left for a copy of the clipboard's content
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetClipboardText">SDL_GetClipboardText</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial byte* SDL_GetClipboardText();

	/// <summary>
	/// Query whether there is data in the clipboard for the provided mime type
	/// </summary>
	/// <param name="mime_type">the mime type to check for data for</param>
	/// <returns>Returns true if there exists data in clipboard for the provided mime type, false if it does not</returns>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_HasClipboardData">SDL_HasClipboardData</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_HasClipboardData(byte* mime_type);

	/// <summary>
	/// Query whether the clipboard exists and contains a non-empty text string
	/// </summary>
	/// <returns>Returns true if the clipboard has text, or false if it does not</returns>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_HasClipboardText">SDL_HasClipboardText</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal static partial CBool SDL_HasClipboardText();

	/// <summary>
	/// Offer clipboard data to the OS
	/// </summary>
	/// <param name="callback">a function pointer to the function that provides the clipboard data</param>
	/// <param name="cleanup">a function pointer to the function that cleans up the clipboard data</param>
	/// <param name="userdate">an opaque pointer that will be forwarded to the callbacks</param>
	/// <param name="mime_types">a list of mime-types that are being offered</param>
	/// <param name="num_mime_types">the number of mime-types in the mime_types list</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// Tell the operating system that the application is offering clipboard data for each of the provided mime-types.
	/// Once another application requests the data the callback function will be called, allowing it to generate and respond with the data for the requested mime-type.
	/// The size of text data does not include any terminator, and the text does not need to be null terminated (e.g. you can directly copy a portion of a document).
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_SetClipboardData">SDL_SetClipboardData</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_SetClipboardData(SDL_ClipboardDataCallback callback, SDL_ClipboardCleanupCallback cleanup, void* userdate, byte** mime_types, UIntPtr num_mime_types);

	/// <summary>
	/// Put UTF-8 text into the clipboard
	/// </summary>
	/// <param name="text">the text to store in the clipboard</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_SetClipboardText">SDL_SetClipboardText</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_SetClipboardText(byte* text);
}
