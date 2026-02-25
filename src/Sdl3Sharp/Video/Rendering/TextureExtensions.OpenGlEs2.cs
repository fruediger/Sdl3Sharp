using Sdl3Sharp.Video.Rendering.Drivers;

namespace Sdl3Sharp.Video.Rendering;

partial class TextureExtensions
{
	extension(Texture<OpenGLEs2>.PropertyNames)
	{
		/// <summary>
		/// The name of a <see cref="RendererExtensions.TryCreateTexture(Renderer{OpenGLEs2}, out Texture{OpenGLEs2}?, Coloring.ColorSpace?, Coloring.PixelFormat?, TextureAccess?, int?, int?, Coloring.Palette?, float?, float?, uint?, uint?, uint?, uint?, Properties?)">property used when creating a <see cref="Texture{TDriver}">Texture&lt;<see cref="OpenGLEs2">OpenGLEs2</see>&gt;</see></see>
		/// that holds <c>GLuint</c> texture associated with the texture, if you want to wrap an existing texture
		/// </summary>
		public static string CreateOpenGLEs2TextureNumber => "SDL.texture.opengles2.create.texture";

		/// <summary>
		/// The name of a <see cref="RendererExtensions.TryCreateTexture(Renderer{OpenGLEs2}, out Texture{OpenGLEs2}?, Coloring.ColorSpace?, Coloring.PixelFormat?, TextureAccess?, int?, int?, Coloring.Palette?, float?, float?, uint?, uint?, uint?, uint?, Properties?)">property used when creating a <see cref="Texture{TDriver}">Texture&lt;<see cref="OpenGLEs2">OpenGLEs2</see>&gt;</see></see>
		/// that holds <c>GLuint</c> texture associated with the UV plane of the NV12 texture, if you want to wrap an existing texture
		/// </summary>
		public static string CreateOpenGLEs2TextureUvNumber => "SDL.texture.opengles2.create.texture_uv";

		/// <summary>
		/// The name of a <see cref="RendererExtensions.TryCreateTexture(Renderer{OpenGLEs2}, out Texture{OpenGLEs2}?, Coloring.ColorSpace?, Coloring.PixelFormat?, TextureAccess?, int?, int?, Coloring.Palette?, float?, float?, uint?, uint?, uint?, uint?, Properties?)">property used when creating a <see cref="Texture{TDriver}">Texture&lt;<see cref="OpenGLEs2">OpenGLEs2</see>&gt;</see></see>
		/// that holds <c>GLuint</c> texture associated with the U plane of the YUV texture, if you want to wrap an existing texture
		/// </summary>
		public static string CreateOpenGLEs2TextureUNumber => "SDL.texture.opengles2.create.texture_u";

		/// <summary>
		/// The name of a <see cref="RendererExtensions.TryCreateTexture(Renderer{OpenGLEs2}, out Texture{OpenGLEs2}?, Coloring.ColorSpace?, Coloring.PixelFormat?, TextureAccess?, int?, int?, Coloring.Palette?, float?, float?, uint?, uint?, uint?, uint?, Properties?)">property used when creating a <see cref="Texture{TDriver}">Texture&lt;<see cref="OpenGLEs2">OpenGLEs2</see>&gt;</see></see>
		/// that holds <c>GLuint</c> texture associated with the V plane of the YUV texture, if you want to wrap an existing texture
		/// </summary>
		public static string CreateOpenGLEs2TextureVNumber => "SDL.texture.opengles2.create.texture_v";

		/// <summary>
		/// The name of a <em>read-only</em> <see cref="ITexture.Properties">property</see> that holds
		/// the <c>GLuint</c> texture associated with the texture
		/// </summary>
		public static string OpenGLEs2TextureNumber => "SDL.texture.opengles2.texture";

		/// <summary>
		/// The name of a <em>read-only</em> <see cref="ITexture.Properties">property</see> that holds
		/// the <c>GLuint</c> texture associated with the UV plane of the NV12 texture
		/// </summary>
		public static string OpenGLEs2TextureUvNumber => "SDL.texture.opengles2.texture_uv";

		/// <summary>
		/// The name of a <em>read-only</em> <see cref="ITexture.Properties">property</see> that holds
		/// the <c>GLuint</c> texture associated with the U plane of the YUV texture
		/// </summary>
		public static string OpenGLEs2TextureUNumber => "SDL.texture.opengles2.texture_u";

		/// <summary>
		/// The name of a <em>read-only</em> <see cref="ITexture.Properties">property</see> that holds
		/// the <c>GLuint</c> texture associated with the V plane of the YUV texture
		/// </summary>
		public static string OpenGLEs2TextureVNumber => "SDL.texture.opengles2.texture_v";

		/// <summary>
		/// The name of a <em>read-only</em> <see cref="ITexture.Properties">property</see> that holds
		/// the <c>GLenum</c> for the texture target associated with the texture
		/// </summary>
		/// <remarks>
		/// <para>
		/// The value of the associated property can be <c>GL_TEXTURE_2D</c>, <c>GL_TEXTURE_EXTERNAL_OES</c>, etc.
		/// </para>
		/// </remarks>
		public static string OpenGLEs2TextureTargetNumber => "SDL.texture.opengles2.target";
	}

	extension(Texture<OpenGLEs2> texture)
	{
		/// <summary>
		/// Gets the <c>GLuint</c> texture associated with the texture
		/// </summary>
		/// <value>
		/// The <c>GLuint</c> texture associated with the texture
		/// </value>
		public uint OpenGLEs2Texture => texture?.Properties?.TryGetNumberValue(Texture<OpenGLEs2>.PropertyNames.OpenGLEs2TextureNumber, out var openGlEs2Texture) is true
			? unchecked((uint)openGlEs2Texture)
			: default;

		/// <summary>
		/// Gets the <c>GLenum</c> for the texture target associated with the texture
		/// </summary>
		/// <value>
		/// The <c>GLenum</c> for the texture target associated with the texture
		/// </value>
		/// <remarks>
		/// <para>
		/// The value of this property can be <c>GL_TEXTURE_2D</c>, <c>GL_TEXTURE_EXTERNAL_OES</c>, etc.
		/// </para>
		/// </remarks>
		public uint OpenGLEs2TextureTarget => texture?.Properties?.TryGetNumberValue(Texture<OpenGLEs2>.PropertyNames.OpenGLEs2TextureTargetNumber, out var openGlEs2TextureTarget) is true
			? unchecked((uint)openGlEs2TextureTarget)
			: default;

		/// <summary>
		/// Gets the <c>GLuint</c> texture associated with the UV plane of the NV12 texture
		/// </summary>
		/// <value>
		/// The <c>GLuint</c> texture associated with the UV plane of the NV12 texture
		/// </value>
		public uint OpenGLEs2TextureUv => texture?.Properties?.TryGetNumberValue(Texture<OpenGLEs2>.PropertyNames.OpenGLEs2TextureUvNumber, out var openGlEs2TextureUv) is true
			? unchecked((uint)openGlEs2TextureUv)
			: default;

		/// <summary>
		/// Gets the <c>GLuint</c> texture associated with the U plane of the YUV texture
		/// </summary>
		/// <value>
		/// The <c>GLuint</c> texture associated with the U plane of the YUV texture
		/// </value>
		public uint OpenGLEs2TextureU => texture?.Properties?.TryGetNumberValue(Texture<OpenGLEs2>.PropertyNames.OpenGLEs2TextureUNumber, out var openGlEs2TextureU) is true
			? unchecked((uint)openGlEs2TextureU)
			: default;

		/// <summary>
		/// Gets the <c>GLuint</c> texture associated with the V plane of the YUV texture
		/// </summary>
		/// <value>
		/// The <c>GLuint</c> texture associated with the V plane of the YUV texture
		/// </value>
		public uint OpenGLEs2TextureV => texture?.Properties?.TryGetNumberValue(Texture<OpenGLEs2>.PropertyNames.OpenGLEs2TextureVNumber, out var openGlEs2TextureV) is true
			? unchecked((uint)openGlEs2TextureV)
			: default;
	}
}
