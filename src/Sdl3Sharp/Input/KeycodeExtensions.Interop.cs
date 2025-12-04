using Sdl3Sharp.Internal.Interop;
using Sdl3Sharp.SourceGeneration;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Input;

partial class KeycodeExtensions
{
	/// <summary>
	/// Get a key code from a human-readable name
	/// </summary>
	/// <param name="name">The human-readable key name</param>
	/// <returns>Returns key code, or <see href="https://wiki.libsdl.org/SDL3/SDLK_UNKNOWN"><c>SDLK_UNKNOWN</c></see> if the name wasn't recognized; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information.</returns>
	/// <remarks>
	/// <para>
	/// This function is not thread safe.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetKeyFromName">SDL_GetKeyFromName</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial Keycode SDL_GetKeyFromName(byte* name);

	/// <summary>
	/// Get the key code corresponding to the given scancode according to the current keyboard layout
	/// </summary>
	/// <param name="scancode">The desired <see href="https://wiki.libsdl.org/SDL3/SDL_Scancode">SDL_Scancode</see> to query</param>
	/// <param name="modstate">The modifier state to use when translating the scancode to a keycode</param>
	/// <param name="key_event">true if the keycode will be used in key events</param>
	/// <returns>Returns the SDL_Keycode that corresponds to the given <see href="https://wiki.libsdl.org/SDL3/SDL_Scancode">SDL_Scancode</see></returns>
	/// <remarks>
	/// <para>
	/// If you want to get the keycode as it would be delivered in key events, including options specified in <see href="">SDL_HINT_KEYCODE_OPTIONS</see>,
	/// then you should pass <c><paramref name="key_event"/></c> as true.
	/// Otherwise this function simply translates the scancode based on the given modifier state.
	/// </para>
	/// <para>
	/// This function is not thread safe.
	/// </para>
	/// </remarks>
	/// <see href="https://wiki.libsdl.org/SDL3/SDL_GetKeyFromScancode">SDL_GetKeyFromScancode</see>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal static partial Keycode SDL_GetKeyFromScancode(Scancode scancode, Keymod modstate, CBool key_event);

	/// <summary>
	/// Get a human-readable name for a key
	/// </summary>
	/// <param name="key">The desired <see href="">SDL_Keycode</see> to query</param>
	/// <returns>Returns a UTF-8 encoded string of the key name</returns>
	/// <remarks>
	/// <para>
	/// If the key doesn't have a name, this function returns an empty string ("").
	/// </para>
	/// <para>
	/// Letters will be presented in their uppercase form, if applicable.
	/// </para>
	/// <para>
	/// This function is not thread safe.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetKeyName">SDL_GetKeyName</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial byte* SDL_GetKeyName(Keycode key);
}
