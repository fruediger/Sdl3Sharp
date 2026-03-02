using Sdl3Sharp.Video.Rendering.Drivers;

namespace Sdl3Sharp.Video.Rendering;

partial class TextureExtensions
{
	extension(Texture<OpenGL>.PropertyNames)
	{
		/// <summary>
		/// The name of a <see cref="RendererExtensions.TryCreateTexture(Renderer{OpenGL}, out Texture{OpenGL}?, Coloring.ColorSpace?, Coloring.PixelFormat?, TextureAccess?, int?, int?, Coloring.Palette?, float?, float?, uint?, uint?, uint?, uint?, Properties?)">property used when creating a <see cref="Texture{TDriver}">Texture&lt;<see cref="OpenGL">OpenGL</see>&gt;</see></see>
		/// that holds the <c>GLuint</c> texture associated with the texture, if you want to wrap an existing texture
		/// </summary>
		public static string CreateOpenGLTextureNumber => "SDL.texture.opengl.create.texture";

		/// <summary>
		/// The name of a <see cref="RendererExtensions.TryCreateTexture(Renderer{OpenGL}, out Texture{OpenGL}?, Coloring.ColorSpace?, Coloring.PixelFormat?, TextureAccess?, int?, int?, Coloring.Palette?, float?, float?, uint?, uint?, uint?, uint?, Properties?)">property used when creating a <see cref="Texture{TDriver}">Texture&lt;<see cref="OpenGL">OpenGL</see>&gt;</see></see>
		/// that holds the <c>GLuint</c> texture associated with the UV plane of the NV12 texture, if you want to wrap an existing texture
		/// </summary>
		public static string CreateOpenGLTextureUvNumber => "SDL.texture.opengl.create.texture_uv";

		/// <summary>
		/// The name of a <see cref="RendererExtensions.TryCreateTexture(Renderer{OpenGL}, out Texture{OpenGL}?, Coloring.ColorSpace?, Coloring.PixelFormat?, TextureAccess?, int?, int?, Coloring.Palette?, float?, float?, uint?, uint?, uint?, uint?, Properties?)">property used when creating a <see cref="Texture{TDriver}">Texture&lt;<see cref="OpenGL">OpenGL</see>&gt;</see></see>
		/// that holds the <c>GLuint</c> texture associated with the U plane of the YUV texture, if you want to wrap an existing texture
		/// </summary>
		public static string CreateOpenGLTextureUNumber => "SDL.texture.opengl.create.texture_u";

		/// <summary>
		/// The name of a <see cref="RendererExtensions.TryCreateTexture(Renderer{OpenGL}, out Texture{OpenGL}?, Coloring.ColorSpace?, Coloring.PixelFormat?, TextureAccess?, int?, int?, Coloring.Palette?, float?, float?, uint?, uint?, uint?, uint?, Properties?)">property used when creating a <see cref="Texture{TDriver}">Texture&lt;<see cref="OpenGL">OpenGL</see>&gt;</see></see>
		/// that holds the <c>GLuint</c> texture associated with the V plane of the YUV texture, if you want to wrap an existing texture
		/// </summary>
		public static string CreateOpenGLTextureVNumber => "SDL.texture.opengl.create.texture_v";

		/// <summary>
		/// The name of a <em>read-only</em> <see cref="Texture.Properties">property</see> that holds
		/// the <c>GLuint</c> texture associated with the texture
		/// </summary>
		public static string OpenGLTextureNumber => "SDL.texture.opengl.texture";

		/// <summary>
		/// The name of a <em>read-only</em> <see cref="Texture.Properties">property</see> that holds
		/// the <c>GLuint</c> texture associated with the UV plane of the NV12 texture
		/// </summary>
		public static string OpenGLTextureUvNumber => "SDL.texture.opengl.texture_uv";

		/// <summary>
		/// The name of a <em>read-only</em> <see cref="Texture.Properties">property</see> that holds
		/// the <c>GLuint</c> texture associated with the U plane of the YUV texture
		/// </summary>
		public static string OpenGLTextureUNumber => "SDL.texture.opengl.texture_u";

		/// <summary>
		/// The name of a <em>read-only</em> <see cref="Texture.Properties">property</see> that holds
		/// the <c>GLuint</c> texture associated with the V plane of the YUV texture
		/// </summary>
		public static string OpenGLTextureVNumber => "SDL.texture.opengl.texture_v";

		/// <summary>
		/// The name of a <em>read-only</em> <see cref="Texture.Properties">property</see> that holds
		/// the <c>GLenum</c> for the texture target associated with the texture
		/// </summary>
		/// <remarks>
		/// <para>
		/// The value of the associated property can be <c>GL_TEXTURE_2D</c>, <c>GL_TEXTURE_ARB</c>, etc.
		/// </para>
		/// </remarks>
		public static string OpenGLTextureTargetNumber => "SDL.texture.opengl.target";

		/// <summary>
		/// The name of a <em>read-only</em> <see cref="Texture.Properties">property</see> that holds
		/// the texture coordinate width of the texture
		/// </summary>
		/// <remarks>
		/// <para>
		/// The value of this property is most likely in the range from <c>0.0</c> to <c>1.0</c>.
		/// </para>
		/// </remarks>
		public static string OpenGLTexWFloat => "SDL.texture.opengl.tex_w";

		/// <summary>
		/// The name of a <em>read-only</em> <see cref="Texture.Properties">property</see> that holds
		/// the texture coordinate height of the texture
		/// </summary>
		/// <remarks>
		/// <para>
		/// The value of this property is most likely in the range from <c>0.0</c> to <c>1.0</c>.
		/// </para>
		/// </remarks>
		public static string OpenGLTexHFloat => "SDL.texture.opengl.tex_h";
	}

	extension(Texture<OpenGL> texture)
	{
		/// <summary>
		/// Gets the <c>GLuint</c> texture associated with the texture
		/// </summary>
		/// <value>
		/// The <c>GLuint</c> texture associated with the texture
		/// </value>
		public uint OpenGLTexture => texture?.Properties?.TryGetNumberValue(Texture<OpenGL>.PropertyNames.OpenGLTextureNumber, out var openGlTexture) is true
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
		public uint OpenGLTextureTarget => texture?.Properties?.TryGetNumberValue(Texture<OpenGL>.PropertyNames.OpenGLTextureTargetNumber, out var openGlTextureTarget) is true
			? unchecked((uint)openGlTextureTarget)
			: default;

		/// <summary>
		/// Gets the <c>GLuint</c> texture associated with the UV plane of the NV12 texture
		/// </summary>
		/// <value>
		/// The <c>GLuint</c> texture associated with the UV plane of the NV12 texture
		/// </value>
		public uint OpenGLTextureUv => texture?.Properties?.TryGetNumberValue(Texture<OpenGL>.PropertyNames.OpenGLTextureUvNumber, out var openGlTextureUv) is true
			? unchecked((uint)openGlTextureUv)
			: default;

		/// <summary>
		/// Gets the <c>GLuint</c> texture associated with the UV plane of the NV12 texture
		/// </summary>
		/// <value>
		/// The <c>GLuint</c> texture associated with the UV plane of the NV12 texture
		/// </value>
		public uint OpenGLTextureU => texture?.Properties?.TryGetNumberValue(Texture<OpenGL>.PropertyNames.OpenGLTextureUNumber, out var openGlTextureU) is true
			? unchecked((uint)openGlTextureU)
			: default;

		/// <summary>
		/// Gets the <c>GLuint</c> texture associated with the V plane of the YUV texture
		/// </summary>
		/// <value>
		/// The <c>GLuint</c> texture associated with the V plane of the YUV texture
		/// </value>
		public uint OpenGLTextureV => texture?.Properties?.TryGetNumberValue(Texture<OpenGL>.PropertyNames.OpenGLTextureVNumber, out var openGlTextureV) is true
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
		public float OpenGLTexH => texture?.Properties?.TryGetFloatValue(Texture<OpenGL>.PropertyNames.OpenGLTexHFloat, out var opneGlTexH) is true
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
		public float OpenGLTexW => texture?.Properties?.TryGetFloatValue(Texture<OpenGL>.PropertyNames.OpenGLTexWFloat, out var openGlTexW) is true
			? openGlTexW
			: default;
	}
}
