using Sdl3Sharp.Video.Coloring;

namespace Sdl3Sharp.Video.Rendering;

partial class Texture
{
	/// <summary>
	/// Provides property names for <see cref="Texture"/> <see cref="Properties">properties</see>
	/// </summary>
	public abstract class PropertyNames
	{
		/// <summary>
		/// The name of a <see cref="Renderer.TryCreateTexture(out Texture?, ColorSpace?, PixelFormat?, TextureAccess?, int?, int?, Palette?, float?, float?, Properties?)">property used when creating a <see cref="Texture"/></see>
		/// that holds the color space of the texture to create
		/// </summary>
		/// <remarks>
		/// <para>
		/// The value of the associated property should be a <see cref="ColorSpace"/> describing the color space that should be used by the newly created texture.
		/// This defaults to <see cref="ColorSpace.SrgbLinear"/> for floating point textures, <see cref="ColorSpace.Hdr10"/> for 10-bit textures,
		/// <see cref="ColorSpace.Srgb"/> for RGB textures and <see cref="ColorSpace.Jpeg"/> for YUV textures.
		/// </para>
		/// </remarks>
		public const string CreateColorSpaceNumber = "SDL.texture.create.colorspace";

		/// <summary>
		/// The name of a <see cref="Renderer.TryCreateTexture(out Texture?, ColorSpace?, PixelFormat?, TextureAccess?, int?, int?, Palette?, float?, float?, Properties?)">property used when creating a <see cref="Texture"/></see>
		/// that holds the pixel format of the texture to create
		/// </summary>
		/// <remarks>
		/// <para>
		/// The value of the associated property should be any of the pre-defined <see cref="PixelFormat"/> values describing the pixel format that should be used by the newly created texture.
		/// This defaults to the best RGBA format for the renderer that creates the texture.
		/// </para>
		/// </remarks>
		public const string CreateFormatNumber = "SDL.texture.create.format";

		/// <summary>
		/// The name of a <see cref="Renderer.TryCreateTexture(out Texture?, ColorSpace?, PixelFormat?, TextureAccess?, int?, int?, Palette?, float?, float?, Properties?)">property used when creating a <see cref="Texture"/></see>
		/// that holds the access mode of the texture to create
		/// </summary>
		/// <remarks>
		/// <para>
		/// The value of the associated property should be any of the pre-defined <see cref="TextureAccess"/> values describing the access mode that should be used by the newly created texture.
		/// This defaults to <see cref="TextureAccess.Static"/>.
		/// </para>
		/// </remarks>
		public const string CreateAccessNumber = "SDL.texture.create.access";

		/// <summary>
		/// The name of a <see cref="Renderer.TryCreateTexture(out Texture?, ColorSpace?, PixelFormat?, TextureAccess?, int?, int?, Palette?, float?, float?, Properties?)">property used when creating a <see cref="Texture"/></see>
		/// that holds the width of the texture to create
		/// </summary>
		/// <remarks>
		/// <para>
		/// The width of the texture to create is given in pixels.
		/// This property is required when creating a texture. Although, you don't need to explicitly specify this property if you set the <em>width</em> parameter in a call to the <see cref="Renderer.TryCreateTexture(out Texture?, ColorSpace?, PixelFormat?, TextureAccess?, int?, int?, Palette?, float?, float?, Properties?)"/> method.
		/// </para>
		/// </remarks>
		public const string CreateWidthNumber = "SDL.texture.create.width";

		/// <summary>
		/// The name of a <see cref="Renderer.TryCreateTexture(out Texture?, ColorSpace?, PixelFormat?, TextureAccess?, int?, int?, Palette?, float?, float?, Properties?)">property used when creating a <see cref="Texture"/></see>
		/// that holds the height of the texture to create
		/// </summary>
		///	<remarks>
		///	<para>
		///	The height of the texture to create is given in pixels.
		///	This property is required when creating a texture. Although, you don't need to explicitly specify this property if you set the <em>height</em> parameter in a call to the <see cref="Renderer.TryCreateTexture(out Texture?, ColorSpace?, PixelFormat?, TextureAccess?, int?, int?, Palette?, float?, float?, Properties?)"/> method.
		///	</para>
		///	</remarks>
		public const string CreateHeightNumber = "SDL.texture.create.height";

#if SDL3_4_0_OR_GREATER

		/// <summary>
		/// The name of a <see cref="Renderer.TryCreateTexture(out Texture?, ColorSpace?, PixelFormat?, TextureAccess?, int?, int?, Palette?, float?, float?, Properties?)">property used when creating a <see cref="Texture"/></see>
		/// that holds the palette to use when creating a <see cref="Texture"/> with a palettized pixel format
		/// </summary>
		/// <remarks>
		/// <para>
		/// This can be set or changed later using the <see cref="Palette"/> property of a <see cref="Texture"/>.
		/// </para>
		/// </remarks>
		public const string CreatePalettePointer = "SDL.texture.create.palette";

#endif

		/// <summary>
		/// The name of a <see cref="Renderer.TryCreateTexture(out Texture?, ColorSpace?, PixelFormat?, TextureAccess?, int?, int?, Palette?, float?, float?, Properties?)">property used when creating a <see cref="Texture"/></see>
		/// that holds the defining value for 100% diffuse white for HDR10 and floating point <see cref="Texture"/>s when creating them
		/// </summary>
		/// <remarks>
		/// <para>
		/// For HDR10 and floating point <see cref="Texture"/>s, the value of the property defines the value of 100% diffuse white that should be used by the newly created texture, with higher values being displayed in the <see cref="HdrHeadroomFloat">High Dynamic Range headroom</see>.
		/// This defaults to <c>100</c> for HDR10 textures and <c>1.0</c> for other textures.
		/// </para>
		/// </remarks>
		public const string CreateSdrWhitePointFloat = "SDL.texture.create.SDR_white_point";

		/// <summary>
		/// The name of a <see cref="Renderer.TryCreateTexture(out Texture?, ColorSpace?, PixelFormat?, TextureAccess?, int?, int?, Palette?, float?, float?, Properties?)">property used when creating a <see cref="Texture"/></see>
		/// that holds the maximum dynamic range for HDR10 and floating point <see cref="Texture"/>s when creating them
		/// </summary>
		/// <remarks>
		/// <para>
		/// For HDR10 and floating point <see cref="Texture"/>s, the value of the property defines the maximum dynamic range used by the content of the newly created texture, in terms of the <see cref="CreateSdrWhitePointFloat">SDR white point</see>.
		/// This would be equivalent to the maximum content light level (maxCLL) divided by the <see cref="CreateSdrWhitePointFloat">SDR white point</see> for HDR10 content.
		/// If the associated property is defined, any values outside of the range supported by the display will be scaled into the available HDR headroom, otherwise they will be clipped.
		/// </para>
		/// </remarks>
		public const string CreateHdrHeadroomFloat = "SDL.texture.create.HDR_headroom";

		/// <summary>
		/// The name of a <em>read-only</em> <see cref="Properties">property</see> that holds the color space of the texture
		/// </summary>
		/// <remarks>
		/// <para>
		/// The value of the associated property is a <see cref="ColorSpace"/> describing the color space used by the texture.
		/// </para>
		/// </remarks>
		public const string ColorSpaceNumber = "SDL.texture.colorspace";

		/// <summary>
		/// The name of a <em>read-only</em> <see cref="Properties">property</see> that holds the pixel format of the texture
		/// </summary>
		/// <remarks>
		/// <para>
		/// The value of the associated property is any of the pre-defined <see cref="PixelFormat"/> values.
		/// </para>
		/// </remarks>
		public const string FormatNumber = "SDL.texture.format";

		/// <summary>
		/// The name of a <em>read-only</em> <see cref="Properties">property</see> that holds the access mode of the texture
		/// </summary>
		/// <remarks>
		/// <para>
		/// The value of the associated property is any of the pre-defined <see cref="TextureAccess"/> values.
		/// </para>
		/// </remarks>
		public const string AccessNumber = "SDL.texture.access";

		/// <summary>
		/// The name of a <em>read-only</em> <see cref="Properties">property</see> that holds the width of the texture
		/// </summary>
		/// <remarks>
		/// <para>
		/// The width of the texture is given in pixels.
		/// </para>
		/// </remarks>
		public const string WidthNumber = "SDL.texture.width";

		/// <summary>
		/// The name of a <em>read-only</em> <see cref="Properties">property</see> that holds the height of the texture
		/// </summary>
		/// <remarks>
		/// <para>
		/// The height of the texture is given in pixels.
		/// </para>
		/// </remarks>
		public const string HeightNumber = "SDL.texture.height";

		/// <summary>
		/// The name of a <em>read-only</em> <see cref="Properties">property</see> that holds the defining value for 100% diffuse white for HDR10 and floating point <see cref="Texture"/>s
		/// </summary>
		/// <remarks>
		/// <para>
		/// For HDR10 and floating point <see cref="Texture"/>s, the value of the property defines the value of 100% diffuse white, with higher values being displayed in the <see cref="HdrHeadroomFloat">High Dynamic Range headroom</see>.
		/// This defaults to <c>100</c> for HDR10 textures and <c>1.0</c> for other textures.
		/// </para>
		/// </remarks>
		public const string SdrWhitePointFloat = "SDL.texture.SDR_white_point";

		/// <summary>
		/// The name of a <em>read-only</em> <see cref="Properties">property</see> that holds the maximum dynamic range for HDR10 and floating point <see cref="Texture"/>s
		/// </summary>
		/// <remarks>
		/// <para>
		/// For HDR10 and floating point <see cref="Texture"/>s, the value of the property defines the maximum dynamic range used by the content, in terms of the <see cref="SdrWhitePointFloat">SDR white point</see>.
		/// If the associated property is defined, any values outside of the range supported by the display will be scaled into the available HDR headroom, otherwise they will be clipped.
		/// This defaults to <c>1.0</c> for SDR textures, <c>4.0</c> for HDR10 textures and has no default for floating point textures.
		/// </para>
		/// </remarks>
		public const string HdrHeadroomFloat = "SDL.texture.HDR_headroom";

		private protected PropertyNames() { }
	}
}
