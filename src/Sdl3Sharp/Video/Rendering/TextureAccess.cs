namespace Sdl3Sharp.Video.Rendering;

/// <summary>
/// Represents the access mode for a texture
/// </summary>
public enum TextureAccess
{
	/// <summary>Changes are rare, the texture is not lockable</summary>
	Static,

	/// <summary>Changes are frequent, the texture is lockable</summary>
	Streaming,

	/// <summary>The texture can be used as a render target</summary>
	Target
}
