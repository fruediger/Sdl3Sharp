using Sdl3Sharp.Video.Rendering.Drivers;

namespace Sdl3Sharp.Video.Rendering;

partial class TextureExtensions
{
	extension(Texture<OpenGl>.PropertyNames)
	{
		/// <summary>
		/// The name of a <see cref="RendererExtensions.TryCreateTexture(Renderer{OpenGl}, out Texture{OpenGl}?, Coloring.ColorSpace?, Coloring.PixelFormat?, TextureAccess?, int?, int?, Coloring.Palette?, float?, float?, uint?, uint?, uint?, uint?, Properties?)">property used when creating a <see cref="Texture{TDriver}">Texture&lt;<see cref="OpenGl">OpenGl</see>&gt;</see></see>
		/// that holds the <c>GLuint</c> texture associated with the texture, if you want to wrap an existing texture
		/// </summary>
		public static string CreateOpenGlTextureNumber => "SDL.texture.opengl.create.texture";

		/// <summary>
		/// The name of a <see cref="RendererExtensions.TryCreateTexture(Renderer{OpenGl}, out Texture{OpenGl}?, Coloring.ColorSpace?, Coloring.PixelFormat?, TextureAccess?, int?, int?, Coloring.Palette?, float?, float?, uint?, uint?, uint?, uint?, Properties?)">property used when creating a <see cref="Texture{TDriver}">Texture&lt;<see cref="OpenGl">OpenGl</see>&gt;</see></see>
		/// that holds the <c>GLuint</c> texture associated with the UV plane of the NV12 texture, if you want to wrap an existing texture
		/// </summary>
		public static string CreateOpenGlTextureUvNumber => "SDL.texture.opengl.create.texture_uv";

		/// <summary>
		/// The name of a <see cref="RendererExtensions.TryCreateTexture(Renderer{OpenGl}, out Texture{OpenGl}?, Coloring.ColorSpace?, Coloring.PixelFormat?, TextureAccess?, int?, int?, Coloring.Palette?, float?, float?, uint?, uint?, uint?, uint?, Properties?)">property used when creating a <see cref="Texture{TDriver}">Texture&lt;<see cref="OpenGl">OpenGl</see>&gt;</see></see>
		/// that holds the <c>GLuint</c> texture associated with the U plane of the YUV texture, if you want to wrap an existing texture
		/// </summary>
		public static string CreateOpenGlTextureUNumber => "SDL.texture.opengl.create.texture_u";

		/// <summary>
		/// The name of a <see cref="RendererExtensions.TryCreateTexture(Renderer{OpenGl}, out Texture{OpenGl}?, Coloring.ColorSpace?, Coloring.PixelFormat?, TextureAccess?, int?, int?, Coloring.Palette?, float?, float?, uint?, uint?, uint?, uint?, Properties?)">property used when creating a <see cref="Texture{TDriver}">Texture&lt;<see cref="OpenGl">OpenGl</see>&gt;</see></see>
		/// that holds the <c>GLuint</c> texture associated with the V plane of the YUV texture, if you want to wrap an existing texture
		/// </summary>
		public static string CreateOpenGlTextureVNumber => "SDL.texture.opengl.create.texture_v";

		/// <summary>
		/// The name of a <em>read-only</em> <see cref="ITexture.Properties">property</see> that holds
		/// the <c>GLuint</c> texture associated with the texture
		/// </summary>
		public static string OpenGlTextureNumber => "SDL.texture.opengl.texture";

		/// <summary>
		/// The name of a <em>read-only</em> <see cref="ITexture.Properties">property</see> that holds
		/// the <c>GLuint</c> texture associated with the UV plane of the NV12 texture
		/// </summary>
		public static string OpenGlTextureUvNumber => "SDL.texture.opengl.texture_uv";

		/// <summary>
		/// The name of a <em>read-only</em> <see cref="ITexture.Properties">property</see> that holds
		/// the <c>GLuint</c> texture associated with the U plane of the YUV texture
		/// </summary>
		public static string OpenGlTextureUNumber => "SDL.texture.opengl.texture_u";

		/// <summary>
		/// The name of a <em>read-only</em> <see cref="ITexture.Properties">property</see> that holds
		/// the <c>GLuint</c> texture associated with the V plane of the YUV texture
		/// </summary>
		public static string OpenGlTextureVNumber => "SDL.texture.opengl.texture_v";

		/// <summary>
		/// The name of a <em>read-only</em> <see cref="ITexture.Properties">property</see> that holds
		/// the <c>GLenum</c> for the texture target associated with the texture
		/// </summary>
		/// <remarks>
		/// <para>
		/// The value of the associated property can be <c>GL_TEXTURE_2D</c>, <c>GL_TEXTURE_ARB</c>, etc.
		/// </para>
		/// </remarks>
		public static string OpenGlTextureTargetNumber => "SDL.texture.opengl.target";

		/// <summary>
		/// The name of a <em>read-only</em> <see cref="ITexture.Properties">property</see> that holds
		/// the texture coordinate width of the texture
		/// </summary>
		/// <remarks>
		/// <para>
		/// The value of this property is most likely in the range from <c>0.0</c> to <c>1.0</c>.
		/// </para>
		/// </remarks>
		public static string OpenGlTexWFloat => "SDL.texture.opengl.tex_w";

		/// <summary>
		/// The name of a <em>read-only</em> <see cref="ITexture.Properties">property</see> that holds
		/// the texture coordinate height of the texture
		/// </summary>
		/// <remarks>
		/// <para>
		/// The value of this property is most likely in the range from <c>0.0</c> to <c>1.0</c>.
		/// </para>
		/// </remarks>
		public static string OpenGlTexHFloat => "SDL.texture.opengl.tex_h";
	}

	extension(Texture<OpenGl> texture)
	{
		/// <summary>
		/// Gets the <c>GLuint</c> texture associated with the texture
		/// </summary>
		/// <value>
		/// The <c>GLuint</c> texture associated with the texture
		/// </value>
		public uint OpenGlTexture => texture?.Properties?.TryGetNumberValue(Texture<OpenGl>.PropertyNames.OpenGlTextureNumber, out var openGlTexture) is true
			? unchecked((uint)openGlTexture)
			: default;

		/// <summary>
		/// Gets the <c>GLenum</c> for the texture target associated with the texture
		/// </summary>
		/// <value>
		/// The <c>GLenum</c> for the texture target associated with the texture
		/// </value>
		/// <remarks>
		/// <para>
		/// The value of this property can be <c>GL_TEXTURE_2D</c>, <c>GL_TEXTURE_ARB</c>, etc.
		/// </para>
		/// </remarks>
		public uint OpenGlTextureTarget => texture?.Properties?.TryGetNumberValue(Texture<OpenGl>.PropertyNames.OpenGlTextureTargetNumber, out var openGlTextureTarget) is true
			? unchecked((uint)openGlTextureTarget)
			: default;

		/// <summary>
		/// Gets the <c>GLuint</c> texture associated with the UV plane of the NV12 texture
		/// </summary>
		/// <value>
		/// The <c>GLuint</c> texture associated with the UV plane of the NV12 texture
		/// </value>
		public uint OpenGlTextureUv => texture?.Properties?.TryGetNumberValue(Texture<OpenGl>.PropertyNames.OpenGlTextureUvNumber, out var openGlTextureUv) is true
			? unchecked((uint)openGlTextureUv)
			: default;

		/// <summary>
		/// Gets the <c>GLuint</c> texture associated with the UV plane of the NV12 texture
		/// </summary>
		/// <value>
		/// The <c>GLuint</c> texture associated with the UV plane of the NV12 texture
		/// </value>
		public uint OpenGlTextureU => texture?.Properties?.TryGetNumberValue(Texture<OpenGl>.PropertyNames.OpenGlTextureUNumber, out var openGlTextureU) is true
			? unchecked((uint)openGlTextureU)
			: default;

		/// <summary>
		/// Gets the <c>GLuint</c> texture associated with the V plane of the YUV texture
		/// </summary>
		/// <value>
		/// The <c>GLuint</c> texture associated with the V plane of the YUV texture
		/// </value>
		public uint OpenGlTextureV => texture?.Properties?.TryGetNumberValue(Texture<OpenGl>.PropertyNames.OpenGlTextureVNumber, out var openGlTextureV) is true
			? unchecked((uint)openGlTextureV)
			: default;

		/// <summary>
		/// Gets the texture coordinate height of the texture
		/// </summary>
		/// <value>
		/// The texture coordinate height of the texture
		/// </value>
		/// <remarks>
		/// <para>
		/// The value of this property is most likely in the range from <c>0.0</c> to <c>1.0</c>.
		/// </para>
		/// </remarks>
		public float OpenGlTexH => texture?.Properties?.TryGetFloatValue(Texture<OpenGl>.PropertyNames.OpenGlTexHFloat, out var opneGlTexH) is true
			? opneGlTexH
			: default;

		/// <summary>
		/// Gets the texture coordinate width of the texture
		/// </summary>
		/// <value>
		/// The texture coordinate width of the texture
		/// </value>
		/// <remarks>
		/// <para>
		/// The value of this property is most likely in the range from <c>0.0</c> to <c>1.0</c>.
		/// </para>
		/// </remarks>
		public float OpenGlTexW => texture?.Properties?.TryGetFloatValue(Texture<OpenGl>.PropertyNames.OpenGlTexWFloat, out var openGlTexW) is true
			? openGlTexW
			: default;
	}
}
