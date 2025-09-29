using Sdl3Sharp.Internal.Interop;
using Sdl3Sharp.SourceGeneration;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Input;

partial struct Scancode
{
	/// <summary>
	/// Get the scancode corresponding to the given key code according to the current keyboard layout
	/// </summary>
	/// <param name="key">The desired <see href="https://wiki.libsdl.org/SDL3/SDL_Keycode">SDL_Keycode</see> to query.</param>
	/// <param name="modstate">A pointer to the modifier state that would be used when the scancode generates this key, may be NULL</param>
	/// <returns>Returns the <see href="https://wiki.libsdl.org/SDL3/SDL_Scancode">SDL_Scancode</see> that corresponds to the given <see href="https://wiki.libsdl.org/SDL3/SDL_Keycode">SDL_Keycode</see></returns>
	/// <remarks>
	/// <para>
	/// Note that there may be multiple scancode+modifier states that can generate this keycode, this will just return the first one found.
	/// </para>
	/// <para>
	/// This function is not thread safe.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetScancodeFromKey">SDL_GetScancodeFromKey</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial Scancode SDL_GetScancodeFromKey(Keycode key, Keymod* modstate);

	/// <summary>
	/// Get a scancode from a human-readable name
	/// </summary>
	/// <param name="name">The human-readable scancode name</param>
	/// <returns>Returns the <see href="https://wiki.libsdl.org/SDL3/SDL_Scancode">SDL_Scancode</see>, or <see href="https://wiki.libsdl.org/SDL3/SDL_SCANCODE_UNKNOWN"><c>SDL_SCANCODE_UNKNOWN</c></see> if the name wasn't recognized;
	/// call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information.</returns>
	/// <remarks>
	/// <para>
	/// This function is not thread safe.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetScancodeFromName">SDL_GetScancodeFromName</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial Scancode SDL_GetScancodeFromName(byte* name);

	/// <summary>
	/// Get a human-readable name for a scancode
	/// </summary>
	/// <param name="scancode">The desired <see href="https://wiki.libsdl.org/SDL3/SDL_Scancode">SDL_Scancode</see> to query</param>
	/// <returns>Returns a pointer to the name for the scancode. If the scancode doesn't have a name this function returns an empty string ("")</returns>
	/// <remarks>
	/// <para>
	/// <em>>Warning</em>: The returned name is by design not stable across platforms,
	/// e.g. the name for <see href="https://wiki.libsdl.org/SDL3/SDL_SCANCODE_LGUI"><c>SDL_SCANCODE_LGUI</c></see> is "Left GUI" under Linux but "Left Windows" under Microsoft Windows,
	/// and some scancodes like <see href="https://wiki.libsdl.org/SDL3/SDL_SCANCODE_NONUSBACKSLASH"><c>SDL_SCANCODE_NONUSBACKSLASH</c></see> don't have any name at all.
	/// There are even scancodes that share names, e.g. <see href="https://wiki.libsdl.org/SDL3/SDL_SCANCODE_RETURN"><c>SDL_SCANCODE_RETURN</c></see>
	/// and <see href="https://wiki.libsdl.org/SDL3/SDL_SCANCODE_RETURN2"><c>SDL_SCANCODE_RETURN2</c></see> (both called "Return").
	/// This function is therefore unsuitable for creating a stable cross-platform two-way mapping between strings and scancodes.
	/// </para>
	/// <para>
	/// This function is not thread safe.
	/// </para>
	/// </remarks>
	/// <see href="https://wiki.libsdl.org/SDL3/SDL_GetScancodeName">SDL_GetScancodeName</see>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial byte* SDL_GetScancodeName(Scancode scancode);

	/// <summary>
	/// Set a human-readable name for a scancode
	/// </summary>
	/// <param name="scancode">The desired <see href="https://wiki.libsdl.org/SDL3/SDL_Scancode">SDL_Scancode</see></param>
	/// <param name="name">The name to use for the scancode, encoded as UTF-8. The string is not copied, so the pointer given to this function must stay valid while SDL is being used.</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information.</returns>
	/// <remarks>
	/// <para>
	/// This function is not thread safe.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_SetScancodeName">SDL_SetScancodeName</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_SetScancodeName(Scancode scancode, byte* name);
}
