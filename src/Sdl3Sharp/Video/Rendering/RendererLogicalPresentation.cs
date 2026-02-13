namespace Sdl3Sharp.Video.Rendering;

/// <summary>
/// Represents how the logical size is mapped to the output resolution
/// </summary>
public enum RendererLogicalPresentation
{
	/// <summary>No logical sizing is applied</summary>
	Disabled,

	/// <summary>The rendered content is stretched to the output resolution</summary>
	Stretch,

	/// <summary>The rendered content is fit to the largest dimension and the other dimension is letterboxed with the clear color</summary>
	Letterbox,

	/// <summary>The rendered content is fit the smallest dimension and the other dimension extends beyond the output bounds</summary>
	Overscan,

	/// <summary>The rendered content is scaled up by integer multiples to fit the output resolution</summary>
	IntegerScale
}
