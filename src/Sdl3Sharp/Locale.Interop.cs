using Sdl3Sharp.Internal.Interop;
using Sdl3Sharp.SourceGeneration;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sdl3Sharp;

partial struct Locale
{
	[StructLayout(LayoutKind.Sequential)]
	internal unsafe readonly struct SDL_Locale
	{
		public readonly byte* Language, Country;
	}

	/// <summary>
	/// Report the user's preferred locale
	/// </summary>
	/// <param name="count">A pointer filled in with the number of locales returned, may be NULL</param>
	/// <returns>
	/// Returns a NULL terminated array of locale pointers, or NULL on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information.
	/// This is a single allocation that should be freed with <see href="https://wiki.libsdl.org/SDL3/SDL_free">SDL_free</see>() when it is no longer needed.
	/// </returns>
	/// <remarks>
	/// <para>
	/// Returned language strings are in the format xx, where 'xx' is an ISO-639 language specifier (such as "en" for English, "de" for German, etc).
	/// Country strings are in the format YY, where "YY" is an ISO-3166 country code (such as "US" for the United States, "CA" for Canada, etc).
	/// Country might be NULL if there's no specific guidance on them (so you might get { "en", "US" } for American English, but { "en", NULL } means "English language, generically").
	/// Language strings are never NULL, except to terminate the array.
	/// </para>
	/// <para>
	/// Please note that not all of these strings are 2 characters; some are three or more.
	/// </para>
	/// <para>
	/// The returned list of locales are in the order of the user's preference.
	/// For example, a German citizen that is fluent in US English and knows enough Japanese to navigate around Tokyo might have a list like: { "de", "en_US", "jp", NULL }.
	/// Someone from England might prefer British English (where "color" is spelled "colour", etc), but will settle for anything like it: { "en_GB", "en", NULL }.
	/// </para>
	/// <para>
	/// This function returns NULL on error, including when the platform does not supply this information at all.
	/// </para>
	/// <para>
	/// This might be a "slow" call that has to query the operating system.
	/// It's best to ask for this once and save the results.
	/// However, this list can change, usually because the user has changed a system preference outside of your program;
	/// SDL will send an <see href="https://wiki.libsdl.org/SDL3/SDL_EVENT_LOCALE_CHANGED">SDL_EVENT_LOCALE_CHANGED</see> event in this case, if possible, and you can call this function again to get an updated copy of preferred locales.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetPreferredLocales">SDL_GetPreferredLocales</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial SDL_Locale** SDL_GetPreferredLocales(int* count);
}
