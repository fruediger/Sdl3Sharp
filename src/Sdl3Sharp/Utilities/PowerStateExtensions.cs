namespace Sdl3Sharp.Utilities;

public static partial class PowerStateExtensions
{
	extension(PowerState)
	{
		/// <summary>
		/// Gets the current power supply details
		/// </summary>
		/// <param name="secondsRemaining">The time, in seconds, of battery life left, or <c>-1</c> if the value couldn't be determined or there's no battery</param>
		/// <param name="percentageRemaining">The percentage of battery life left (in the range of <c>0</c> through <c>100</c>), or <c>-1</c> if the value couldn't be determined or there's no battery</param>
		/// <returns>
		/// The current battery's <see cref="PowerState"/> including <see cref="PowerState.Unknown"/> if the value couldn't be determined or <see cref="PowerState.NoBattery"/> when there's no battery,
		/// or <see cref="Error"/> on failure (check <see cref="Error.TryGet(out string?)"/> for more information)
		/// </returns>
		/// <remarks>
		/// <para>
		/// You should never take a battery status as absolute truth. Batteries (especially failing batteries) are delicate hardware, and the values reported here are best estimates based on what that hardware reports.
		/// It's not uncommon for older batteries to lose stored power much faster than it reports, or completely drain when reporting it has 20 percent left, etc.
		/// </para>
		/// <para>
		/// The battery's status can change at any time; if you are concerned with power state, you should call this method frequently, and perhaps ignore changes until they seem to be stable for a few seconds.
		/// </para>
		/// <para>
		/// It's possible for some platforms to only report the battery life time left (<paramref name="secondsRemaining"/>) or the battery percentage left (<paramref name="percentageRemaining"/>) but not both.
		/// </para>
		/// <para>
		/// On some platforms, retrieving power supply details might be expensive. If you want to display continuous status you could call this method every minute or so.
		/// </para>
		/// </remarks>
		public static PowerState GetInfo(out int secondsRemaining, out int percentageRemaining)
		{
			unsafe
			{
				int localSecondsLeft, localPercentLeft;

				var result = SDL_GetPowerInfo(&localSecondsLeft, &localPercentLeft);

				secondsRemaining = localSecondsLeft;
				percentageRemaining = localPercentLeft;

				return result;
			}
		}
	}
}
