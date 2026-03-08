namespace Sdl3Sharp.Video.Windowing;

/// <summary>
/// Represents the vertical synchronization (VSync) mode or interval for the surface of a window
/// </summary>
public enum WindowSurfaceVSync : int
{
	/// <summary>VSync is disabled</summary>
	Disabled = 0,

	/// <summary>Adaptive VSync (late swap tearing)</summary>
	Adaptive = -1,
}
