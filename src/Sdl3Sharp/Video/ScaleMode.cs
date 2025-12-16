namespace Sdl3Sharp.Video;

/// <summary>
/// Represents a scale mode for <see cref="Surface"/> and rendering operations
/// </summary>
public enum ScaleMode
{
	/// <summary>An invalid scale mode</summary>
	Invalid = -1,

	/// <summary>Nearest pixel sampling</summary>
	Nearest,

	/// <summary>Linear filtering</summary>
	Linear,

#if SDL3_4_0_OR_GREATER
	/// <summary>Nearest pixel sampling with improved scaling for pixel art</summary>
	PixelArt
#endif
}
