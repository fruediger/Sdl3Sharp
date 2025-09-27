namespace Sdl3Sharp.Threading;

/// <summary>
/// Represents a SDL thread priority
/// </summary>
/// <remarks>
/// <para>
/// SDL will make system changes as necessary in order to apply the thread priority.
/// Code which attempts to control thread state related to priority should be aware that calling <see cref="SDL_SetCurrentThreadPriority "/> may alter such state.
/// <see cref="SDL_HINT_THREAD_PRIORITY_POLICY"/> can be used to control aspects of this behavior.
/// </para>
/// <para>
/// This explicitly <em>does not</em> represent a Unix nice level!
/// </para>
/// </remarks>
public enum ThreadPriority : int
{
	/// <summary>Low thread priority</summary>
	Low,

	/// <summary>Normal thread priority</summary>
	Normal,

	/// <summary>High thread priority</summary>
	High,

	/// <summary>Time critical thread priority</summary>
	TimeCritical
}
