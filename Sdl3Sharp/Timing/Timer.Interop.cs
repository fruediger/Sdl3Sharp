using Sdl3Sharp.Interop;
using Sdl3Sharp.SourceGeneration;
using System.Runtime.CompilerServices;
using unsafe NSTimerCallback = delegate* unmanaged[Cdecl]<void*, uint, ulong, ulong>;
using unsafe TimerCallback = delegate* unmanaged[Cdecl]<void*, uint, uint, uint>;

namespace Sdl3Sharp.Timing;

partial class Timer
{
	/// <summary>
	/// Call a callback function at a future time
	/// </summary>
	/// <param name="interval">the timer delay, in milliseconds, passed to <c>callback</c></param>
	/// <param name="callback">the <see href="https://wiki.libsdl.org/SDL3/SDL_TimerCallback">SDL_TimerCallback</see> function to call when the specified <c>interval</c> elapses</param>
	/// <param name="userdata">a pointer that is passed to <c>callback</c></param>
	/// <returns>Returns a timer ID or 0 on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// The callback function is passed the current timer interval and the user supplied parameter from the <see href="https://wiki.libsdl.org/SDL3/SDL_AddTimer">SDL_AddTimer</see>() call and should return the next timer interval. If the value returned from the callback is 0, the timer is canceled and will be removed.
	///
	/// The callback is run on a separate thread, and for short timeouts can potentially be called before this function returns.
	///
	/// Timers take into account the amount of time it took to execute the callback. For example, if the callback took 250 ms to execute and returned 1000 (ms), the timer would only wait another 750 ms before its next iteration.
	///
	/// Timing may be inexact due to OS scheduling. Be sure to note the current time with <see href="https://wiki.libsdl.org/SDL3/SDL_GetTicksNS">SDL_GetTicksNS</see>() or <see href="https://wiki.libsdl.org/SDL3/SDL_GetPerformanceCounter">SDL_GetPerformanceCounter</see>() in case your callback needs to adjust for variances.
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_AddTimer">SDL_AddTimer</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial uint SDL_AddTimer(uint interval, TimerCallback callback, void* userdata);

	/// <summary>
	/// Call a callback function at a future time
	/// </summary>
	/// <param name="interval">the timer delay, in nanoseconds, passed to <c>callback</c></param>
	/// <param name="callback">the <see href="https://wiki.libsdl.org/SDL3/SDL_TimerCallback">SDL_TimerCallback</see> function to call when the specified <c>interval</c> elapses</param>
	/// <param name="userdata">a pointer that is passed to <c>callback</c></param>
	/// <returns>Returns a timer ID or 0 on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// The callback function is passed the current timer interval and the user supplied parameter from the <see href="https://wiki.libsdl.org/SDL3/SDL_AddTimerNS">SDL_AddTimerNS</see>() call and should return the next timer interval. If the value returned from the callback is 0, the timer is canceled and will be removed.
	///
	/// The callback is run on a separate thread, and for short timeouts can potentially be called before this function returns.
	///
	/// Timers take into account the amount of time it took to execute the callback. For example, if the callback took 250 ns to execute and returned 1000 (ns), the timer would only wait another 750 ns before its next iteration.
	///
	/// Timing may be inexact due to OS scheduling. Be sure to note the current time with <see href="https://wiki.libsdl.org/SDL3/SDL_GetTicksNS">SDL_GetTicksNS</see>() or <see href="https://wiki.libsdl.org/SDL3/SDL_GetPerformanceCounter">SDL_GetPerformanceCounter</see>() in case your callback needs to adjust for variances.
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_AddTimerNS">SDL_AddTimerNS</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial uint SDL_AddTimerNS(ulong interval, NSTimerCallback callback, void* userdata);

	/// <summary>
	/// Get the current value of the high resolution counter
	/// </summary>
	/// <returns>Returns the current counter value</returns>
	/// <remarks>
	/// This function is typically used for profiling.
	///
	/// The counter values are only meaningful relative to each other. Differences between values can be converted to times by using <see href="https://wiki.libsdl.org/SDL3/SDL_GetPerformanceFrequency">SDL_GetPerformanceFrequency</see>().
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetPerformanceCounter">SDL_GetPerformanceCounter</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal static partial ulong SDL_GetPerformanceCounter();

	/// <summary>
	/// Get the count per second of the high resolution counter
	/// </summary>
	/// <returns>Returns a platform-specific count per second</returns>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetPerformanceFrequency">SDL_GetPerformanceFrequency</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal static partial ulong SDL_GetPerformanceFrequency();

	/// <summary>
	/// Remove a timer created with <see href="https://wiki.libsdl.org/SDL3/SDL_AddTimer">SDL_AddTimer</see>()
	/// </summary>
	/// <param name="id">the ID of the timer to remove</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_RemoveTimer">SDL_RemoveTimer</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal static partial CBool SDL_RemoveTimer(uint id);

	/// <summary>
	/// Get the number of milliseconds since the SDL library initialization
	/// </summary>
	/// <returns>Returns an unsigned 64-bit value representing the number of milliseconds since the SDL library initialized</returns>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetTicks">SDL_GetTicks</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal static partial ulong SDL_GetTicks();

	/// <summary>
	/// Get the number of nanoseconds since the SDL library initialization
	/// </summary>
	/// <returns>Returns an unsigned 64-bit value representing the number of nanoseconds since the SDL library initialized</returns>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetTicksNS">SDL_GetTicksNS</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal static partial ulong SDL_GetTicksNS();
}
