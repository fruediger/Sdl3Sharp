using Sdl3Sharp.Internal.Interop;
using Sdl3Sharp.SourceGeneration;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Timing;

partial struct Time
{
	/// <summary>
	/// Converts a calendar time to an <see href="https://wiki.libsdl.org/SDL3/SDL_Time">SDL_Time</see> in nanoseconds since the epoch
	/// </summary>
	/// <param name="dt">the source <see href="https://wiki.libsdl.org/SDL3/SDL_DateTime">SDL_DateTime</see></param>
	/// <param name="ticks">the resulting <see href="https://wiki.libsdl.org/SDL3/SDL_Time">SDL_Time</see></param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError()</see> for more information</returns>
	/// <remarks>
	/// This function ignores the day_of_week member of the <see href="https://wiki.libsdl.org/SDL3/SDL_DateTime">SDL_DateTime</see> struct, so it may remain unset
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_DateTimeToTime">SDL_DateTimeToTime</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_DateTimeToTime(DateTime* dt, Time* ticks);

	/// <summary>
	/// Gets the current value of the system realtime clock in nanoseconds since Jan 1, 1970 in Universal Coordinated Time (UTC)
	/// </summary>
	/// <param name="ticks">the <see href="https://wiki.libsdl.org/SDL3/SDL_Time">SDL_Time</see> to hold the returned tick count</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetCurrentTime">SDL_GetCurrentTime</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_GetCurrentTime(Time* ticks);

	/// <summary>
	/// Converts a Windows FILETIME (100-nanosecond intervals since January 1, 1601) to an SDL time
	/// </summary>
	/// <param name="dwLowDateTime">the low portion of the Windows FILETIME value</param>
	/// <param name="dwHighDateTime">the high portion of the Windows FILETIME value</param>
	/// <returns>Returns the converted SDL time</returns>
	/// <remarks>
	/// This function takes the two 32-bit values of the FILETIME structure as parameters
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_TimeFromWindows">SDL_TimeFromWindows</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal static partial Time SDL_TimeFromWindows(uint dwLowDateTime, uint dwHighDateTime);

	/// <summary>
	/// Converts an <see href="https://wiki.libsdl.org/SDL3/SDL_Time">SDL_Time</see> in nanoseconds since the epoch to a calendar time in the <see href="https://wiki.libsdl.org/SDL3/SDL_DateTime">SDL_DateTime</see> format
	/// </summary>
	/// <param name="ticks">the <see href="https://wiki.libsdl.org/SDL3/SDL_Time">SDL_Time</see> to be converted</param>
	/// <param name="dt">the resulting <see href="https://wiki.libsdl.org/SDL3/SDL_DateTime">SDL_DateTime</see></param>
	/// <param name="localTime">the resulting <see href="https://wiki.libsdl.org/SDL3/SDL_DateTime">SDL_DateTime</see> will be expressed in local time if true, otherwise it will be in Universal Coordinated Time (UTC)</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_TimeToDateTime">SDL_TimeToDateTime</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_TimeToDateTime(Time ticks, DateTime* dt, CBool localTime);

	/// <summary>
	/// Converts an SDL time into a Windows FILETIME (100-nanosecond intervals since January 1, 1601)
	/// </summary>
	/// <param name="ticks">the time to convert</param>
	/// <param name="dwLowDateTime">a pointer filled in with the low portion of the Windows FILETIME value</param>
	/// <param name="dwHighDateTime">a pointer filled in with the high portion of the Windows FILETIME value</param>
	/// <remarks>
	/// This function fills in the two 32-bit values of the FILETIME structure
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_TimeToWindows">SDL_TimeToWindows</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial void SDL_TimeToWindows(Time ticks, uint* dwLowDateTime, uint* dwHighDateTime);
}
