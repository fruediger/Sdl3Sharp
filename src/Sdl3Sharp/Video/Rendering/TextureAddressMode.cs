namespace Sdl3Sharp.Video.Rendering;

#if SDL3_4_0_OR_GREATER

/// <summary>
/// Represents how texture coordinates outside the range from 0 to 1 are handled
/// </summary>
/// <remarks>
/// <para>
/// This is primarily used in <see cref="SDL_RenderGeometry"/>.
/// </para>
/// <para>
/// Texture wrapping is always supported for power of two texture sizes, and is supported for other texture sizes if <see cref="SDL_PROP_RENDERER_TEXTURE_WRAPPING_BOOLEAN"/> is set to <c><see langword="true"/></c>.
/// </para>
/// </remarks>
public enum TextureAddressMode
{
	/// <summary>Invalid address mode</summary>
	Invalid = -1,

	/// <summary>Wrapping is enabled if the texture coordinates are outside the range from 0 to 1</summary>
	Auto,

	/// <summary>The texture coordinates are clamped to the range from 0 to 1</summary>
	Clamp,

	/// <summary>The texture coordinates are wrapped around resulting the texture being repeated (tiled)</summary>
	Wrap,
}

#endif