using Sdl3Sharp.Internal.Interop;
using Sdl3Sharp.SourceGeneration;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Utilities;

partial class PowerStateExtensions
{
	/// <summary>
	/// Get the current power supply details
	/// </summary>
	/// <param name="seconds">A pointer filled in with the seconds of battery life left, or NULL to ignore. This will be filled in with -1 if we can't determine a value or there is no battery</param>
	/// <param name="percent">A pointer filled in with the percentage of battery life left, between 0 and 100, or NULL to ignore. This will be filled in with -1 we can't determine a value or there is no battery</param>
	/// <returns>Returns the current battery state or <see href="https://wiki.libsdl.org/SDL3/SDL_POWERSTATE_ERROR"><c>SDL_POWERSTATE_ERROR</c></see> on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information.</returns>
	/// <remarks>
	/// <para>
	/// You should never take a battery status as absolute truth. Batteries (especially failing batteries) are delicate hardware, and the values reported here are best estimates based on what that hardware reports.
	/// It's not uncommon for older batteries to lose stored power much faster than it reports, or completely drain when reporting it has 20 percent left, etc.
	/// </para>
	/// <para>
	/// Battery status can change at any time; if you are concerned with power state, you should call this function frequently, and perhaps ignore changes until they seem to be stable for a few seconds.
	/// </para>
	/// <para>
	/// It's possible a platform can only report battery percentage or time left but not both.
	/// </para>
	/// <para>
	/// On some platforms, retrieving power supply details might be expensive.
	/// If you want to display continuous status you could call this function every minute or so.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetPowerInfo">SDL_GetPowerInfo</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial PowerState SDL_GetPowerInfo(int* seconds, int* percent);
}
