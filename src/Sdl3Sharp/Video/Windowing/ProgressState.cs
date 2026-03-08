#if SDL3_4_0_OR_GREATER

namespace Sdl3Sharp.Video.Windowing;

/// <summary>
/// Represents the state of a progress bar associated with an <see cref="Window"/>
/// </summary>
public enum ProgressState
{
	/// <summary>Represents an invalid progress state</summary>
	/// <remarks>
	/// <para>
	/// You might want to check <see cref="Error.TryGet(out string?)"/> for more information.
	/// </para>
	/// </remarks>
	Invalid = -1,

	/// <summary>No progress bar is shown</summary>
	None,

	/// <summary>The progress bar is shown in an indeterminate state</summary>
	Indeterminate,

	/// <summary>The progress bar is shown in a normal state</summary>
	Normal,

	/// <summary>The progress bar is shown in a paused state</summary>
	Paused,

	/// <summary>The progress bar is shown in an error state</summary>
	Error,
}

#endif