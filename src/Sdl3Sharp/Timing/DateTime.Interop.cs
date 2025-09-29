using Sdl3Sharp.Internal.Interop;
using Sdl3Sharp.SourceGeneration;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Timing;

partial struct DateTime
{
	/// <summary>
	/// Gets the current preferred date and time format for the system locale
	/// </summary>
	/// <param name="dateFormat">a pointer to the <see href="https://wiki.libsdl.org/SDL3/SDL_DateFormat">SDL_DateFormat</see> to hold the returned date format, may be NULL</param>
	/// <param name="timeFormat">a pointer to the <see href="https://wiki.libsdl.org/SDL3/SDL_TimeFormat">SDL_TimeFormat</see> to hold the returned time format, may be NULL</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// This might be a "slow" call that has to query the operating system.
	/// It's best to ask for this once and save the results.
	/// However, the preferred formats can change, usually because the user has changed a system preference outside of your program.
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetDateTimeLocalePreferences">SDL_GetDateTimeLocalePreferences</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_GetDateTimeLocalePreferences(DateFormat* dateFormat, TimeFormat* timeFormat);

	/// <summary>
	/// Get the day of week for a calendar date
	/// </summary>
	/// <param name="year">the year component of the date</param>
	/// <param name="month">the month component of the date</param>
	/// <param name="day">the day component of the date</param>
	/// <returns>Returns a value between 0 and 6 (0 being Sunday) if the date is valid or -1 on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetDayOfWeek">SDL_GetDayOfWeek</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal static partial int SDL_GetDayOfWeek(int year, int month, int day);

	/// <summary>
	/// Get the day of year for a calendar date
	/// </summary>
	/// <param name="year">the year component of the date</param>
	/// <param name="month">the month component of the date</param>
	/// <param name="day">the day component of the date</param>
	/// <returns>Returns the day of year [0-365] if the date is valid or -1 on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetDayOfYear">SDL_GetDayOfYear</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal static partial int SDL_GetDayOfYear(int year, int month, int day);

	/// <summary>
	/// Get the number of days in a month for a given year
	/// </summary>
	/// <param name="year">the year</param>
	/// <param name="month">the month [1-12]</param>
	/// <returns>Returns the number of days in the requested month or -1 on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetDaysInMonth">SDL_GetDaysInMonth</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal static partial int SDL_GetDaysInMonth(int year, int month);
}
