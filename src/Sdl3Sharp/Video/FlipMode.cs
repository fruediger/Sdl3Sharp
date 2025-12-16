namespace Sdl3Sharp.Video;

/// <summary>
/// Represents a flip mode for <see cref="Surface"/> and rendering operations
/// </summary>
public enum FlipMode
{
	/// <summary>Do not flip</summary>
	None,

	/// <summary>Flip horizontally</summary>
	Horizontal,

	/// <summary>Flip vertically</summary>
	Vertical,

	/// <summary>Flip both, horizontally and vertically</summary>
	HorizontalAndVertical = Horizontal | Vertical
}
