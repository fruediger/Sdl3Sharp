namespace Sdl3Sharp.Video.Rendering;

/// <summary>
/// Represents the vertical synchronization (VSync) mode or interval for a renderer
/// </summary>
public enum RendererVSync : int
{
	/// <summary>VSync is disabled</summary>
	Disabled = 0,

	/// <summary>Adaptive VSync (late swap tearing)</summary>
	Adaptive = -1,
}
