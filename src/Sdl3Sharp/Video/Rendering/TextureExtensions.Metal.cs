using Sdl3Sharp.Video.Rendering.Drivers;

namespace Sdl3Sharp.Video.Rendering;

partial class TextureExtensions
{
	extension(Texture<Metal>.PropertyNames)
	{
		/// <summary>
		/// Gets the name of a <see cref="RendererExtensions.TryCreateTexture(Renderer{Metal}, out Texture{Metal}?, Coloring.ColorSpace?, Coloring.PixelFormat?, TextureAccess?, int?, int?, Coloring.Palette?, float?, float?, nint?, Properties?)">property used when creating a <see cref="Texture{TDriver}">Texture&lt;<see cref="Metal">Metal</see>&gt;</see></see>
		/// that holds a pointer to the <c><see href="https://developer.apple.com/documentation/corevideo/cvpixelbuffer">CVPixelBuffer</see></c> associated with the texture, if you want to create a texture from a existing pixel buffer
		/// </summary>
		public static string CreateMetalPixelBufferPointer => "SDL.texture.create.metal.pixelbuffer";
	}
}
