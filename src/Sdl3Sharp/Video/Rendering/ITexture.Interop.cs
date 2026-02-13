using Sdl3Sharp.Internal;
using Sdl3Sharp.Internal.Interop;
using Sdl3Sharp.SourceGeneration;
using Sdl3Sharp.Video.Blending;
using Sdl3Sharp.Video.Coloring;
using Sdl3Sharp.Video.Drawing;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sdl3Sharp.Video.Rendering;

partial interface ITexture
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct SDL_Texture
	{
		public readonly PixelFormat Format;
		public readonly int W;
		public readonly int H;
		public int RefCount;
	}

	[FormattedConstant(ErrorHelper.ParameterInvalidErrorFormat, nameof(texture))]
	internal unsafe static partial ReadOnlySpan<byte> GetTextureInvalidTextureErrorMessage(SDL_Texture* texture = default);

	/// <summary>
	/// Create a texture for a rendering context
	/// </summary>
	/// <param name="renderer">The rendering context</param>
	/// <param name="format">One of the enumerated values in <see href="https://wiki.libsdl.org/SDL3/SDL_PixelFormat">SDL_PixelFormat</see></param>
	/// <param name="access">One of the enumerated values in <see href="https://wiki.libsdl.org/SDL3/SDL_TextureAccess">SDL_TextureAccess</see></param>
	/// <param name="w">The width of the texture in pixels</param>
	/// <param name="h">The height of the texture in pixels</param>
	/// <returns>Returns the created texture or NULL on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_CreateTexture">SDL_CreateTexture</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial SDL_Texture* SDL_CreateTexture(IRenderer.SDL_Renderer* renderer, PixelFormat format, TextureAccess access, int w, int h);

	/// <summary>
	/// Create a texture from an existing surface
	/// </summary>
	/// <param name="renderer">The rendering context</param>
	/// <param name="surface">The <see href="https://wiki.libsdl.org/SDL3/SDL_Surface">SDL_Surface</see> structure containing pixel data used to fill the texture</param>
	/// <returns>Returns the created texture or NULL on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// The surface is not modified or freed by this function.
	/// </para>
	/// <para>
	/// The <see href="https://wiki.libsdl.org/SDL3/SDL_TextureAccess">SDL_TextureAccess</see> hint for the created texture is <see href="https://wiki.libsdl.org/SDL3/SDL_TEXTUREACCESS_STATIC"><c>SDL_TEXTUREACCESS_STATIC</c></see>.
	/// </para>
	/// <para>
	/// The pixel format of the created texture may be different from the pixel format of the surface, and can be queried using the <see href="https://wiki.libsdl.org/SDL3/SDL_PROP_TEXTURE_FORMAT_NUMBER">SDL_PROP_TEXTURE_FORMAT_NUMBER</see> property.
	/// </para>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_CreateTextureFromSurface">SDL_CreateTextureFromSurface</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial SDL_Texture* SDL_CreateTextureFromSurface(IRenderer.SDL_Renderer* renderer, Surface.SDL_Surface* surface);

	/// <summary>
	/// Create a texture for a rendering context with the specified properties
	/// </summary>
	/// <param name="renderer">The rendering context</param>
	/// <param name="props">The properties to use</param>
	/// <returns>Returns the created texture or NULL on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// These are the supported properties:
	/// <list type="bullet">
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_TEXTURE_CREATE_COLORSPACE_NUMBER"><c>SDL_PROP_TEXTURE_CREATE_COLORSPACE_NUMBER</c></see></term>
	///			<description>
	///				An <see href="https://wiki.libsdl.org/SDL3/SDL_Colorspace">SDL_Colorspace</see> value describing the texture colorspace,
	///				defaults to <see href="https://wiki.libsdl.org/SDL3/SDL_COLORSPACE_SRGB_LINEAR">SDL_COLORSPACE_SRGB_LINEAR</see> for floating point textures,
	///				<see href="https://wiki.libsdl.org/SDL3/SDL_COLORSPACE_HDR10">SDL_COLORSPACE_HDR10</see> for 10-bit textures,
	///				<see href="https://wiki.libsdl.org/SDL3/SDL_COLORSPACE_SRGB">SDL_COLORSPACE_SRGB</see> for other RGB textures
	///				and <see href="https://wiki.libsdl.org/SDL3/SDL_COLORSPACE_JPEG">SDL_COLORSPACE_JPEG</see> for YUV textures
	///			</description>
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_TEXTURE_CREATE_FORMAT_NUMBER"><c>SDL_PROP_TEXTURE_CREATE_FORMAT_NUMBER</c></see></term>
	///			<description>One of the enumerated values in <see href="https://wiki.libsdl.org/SDL3/SDL_PixelFormat">SDL_PixelFormat</see>, defaults to the best RGBA format for the renderer</description>
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_TEXTURE_CREATE_WIDTH_NUMBER"><c>SDL_PROP_TEXTURE_CREATE_ACCESS_NUMBER</c></see></term>
	///			<description>One of the enumerated values in <see href="https://wiki.libsdl.org/SDL3/SDL_TextureAccess">SDL_TextureAccess</see>, defaults to <see href="https://wiki.libsdl.org/SDL3/SDL_TEXTUREACCESS_STATIC">SDL_TEXTUREACCESS_STATIC</see></description>
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_TEXTURE_CREATE_HEIGHT_NUMBER"><c>SDL_PROP_TEXTURE_CREATE_WIDTH_NUMBER</c></see></term>
	///			<description>The width of the texture in pixels, required</description>
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_TEXTURE_CREATE_ACCESS_NUMBER"><c>SDL_PROP_TEXTURE_CREATE_HEIGHT_NUMBER</c></see></term>
	///			<description>The height of the texture in pixels, required</description>
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_TEXTURE_CREATE_SDR_WHITE_POINT_FLOAT"><c>SDL_PROP_TEXTURE_CREATE_SDR_WHITE_POINT_FLOAT</c></see></term>
	///			<description>
	///				For HDR10 and floating point textures, this defines the value of 100% diffuse white, with higher values being displayed in the High Dynamic Range headroom.
	///				This defaults to 100 for HDR10 textures and 1.0 for floating point textures.
	///			</description>
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_TEXTURE_CREATE_HDR_HEADROOM_FLOAT"><c>SDL_PROP_TEXTURE_CREATE_HDR_HEADROOM_FLOAT</c></see></term>
	///			<description>
	///				For HDR10 and floating point textures, this defines the maximum dynamic range used by the content, in terms of the SDR white point.
	///				This would be equivalent to maxCLL / <see href="https://wiki.libsdl.org/SDL3/SDL_PROP_TEXTURE_CREATE_SDR_WHITE_POINT_FLOAT">SDL_PROP_TEXTURE_CREATE_SDR_WHITE_POINT_FLOAT</see> for HDR10 content.
	///				If this is defined, any values outside the range supported by the display will be scaled into the available HDR headroom, otherwise they are clipped.
	///			</description>
	///		</item>
	/// </list>
	/// With the direct3d11 renderer:
	/// <list type="bullet">
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_TEXTURE_CREATE_D3D11_TEXTURE_POINTER"><c>SDL_PROP_TEXTURE_CREATE_D3D11_TEXTURE_POINTER</c></see></term>
	///			<description>The ID3D11Texture2D associated with the texture, if you want to wrap an existing texture</description>
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_TEXTURE_CREATE_D3D11_TEXTURE_U_POINTER"><c>SDL_PROP_TEXTURE_CREATE_D3D11_TEXTURE_U_POINTER</c></see></term>
	///			<description>The ID3D11Texture2D associated with the U plane of a YUV texture, if you want to wrap an existing texture</description>
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_TEXTURE_CREATE_D3D11_TEXTURE_V_POINTER"><c>SDL_PROP_TEXTURE_CREATE_D3D11_TEXTURE_V_POINTER</c></see></term>
	///			<description>The ID3D11Texture2D associated with the V plane of a YUV texture, if you want to wrap an existing texture</description>
	///		</item>
	/// </list>
	/// With the direct3d12 renderer:
	/// <list type="bullet">
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_TEXTURE_CREATE_D3D12_TEXTURE_POINTER"><c>SDL_PROP_TEXTURE_CREATE_D3D12_TEXTURE_POINTER</c></see></term>
	///			<description>The ID3D12Resource associated with the texture, if you want to wrap an existing texture</description>
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_TEXTURE_CREATE_D3D12_TEXTURE_U_POINTER"><c>SDL_PROP_TEXTURE_CREATE_D3D12_TEXTURE_U_POINTER</c></see></term>
	///			<description>The ID3D12Resource associated with the U plane of a YUV texture, if you want to wrap an existing texture</description>
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_TEXTURE_CREATE_D3D12_TEXTURE_V_POINTER"><c>SDL_PROP_TEXTURE_CREATE_D3D12_TEXTURE_V_POINTER</c></see></term>
	///			<description>The ID3D12Resource associated with the V plane of a YUV texture, if you want to wrap an existing texture</description>
	///		</item>
	/// </list>
	/// With the metal renderer:
	/// <list type="bullet">
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_TEXTURE_CREATE_METAL_PIXELBUFFER_POINTER"><c>SDL_PROP_TEXTURE_CREATE_METAL_PIXELBUFFER_POINTER</c></see></term>
	///			<description>The CVPixelBufferRef associated with the texture, if you want to create a texture from an existing pixel buffer</description>
	///		</item>
	/// </list>
	/// With the opengl renderer:
	/// <list type="bullet">
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_TEXTURE_CREATE_D3D12_TEXTURE_POINTER"><c>SDL_PROP_TEXTURE_CREATE_OPENGL_TEXTURE_NUMBER</c></see></term>
	///			<description>The GLuint texture associated with the texture, if you want to wrap an existing texture</description>
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_TEXTURE_CREATE_OPENGL_TEXTURE_UV_NUMBER"><c>SDL_PROP_TEXTURE_CREATE_OPENGL_TEXTURE_UV_NUMBER</c></see></term>
	///			<description>The GLuint texture associated with the UV plane of an NV12 texture, if you want to wrap an existing texture</description>
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_TEXTURE_CREATE_OPENGL_TEXTURE_U_NUMBER"><c>SDL_PROP_TEXTURE_CREATE_OPENGL_TEXTURE_U_NUMBER</c></see></term>
	///			<description>The GLuint texture associated with the U plane of a YUV texture, if you want to wrap an existing texture</description>
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_TEXTURE_CREATE_OPENGL_TEXTURE_V_NUMBER"><c>SDL_PROP_TEXTURE_CREATE_OPENGL_TEXTURE_V_NUMBER</c></see></term>
	///			<description>The GLuint texture associated with the V plane of a YUV texture, if you want to wrap an existing texture</description>
	///		</item>
	/// </list>
	/// With the opengles2 renderer:
	/// <list type="bullet">
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_TEXTURE_CREATE_OPENGLES2_TEXTURE_NUMBER"><c>SDL_PROP_TEXTURE_CREATE_OPENGLES2_TEXTURE_NUMBER</c></see></term>
	///			<description>The GLuint texture associated with the texture, if you want to wrap an existing texture</description>
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_TEXTURE_CREATE_OPENGLES2_TEXTURE_UV_NUMBER"><c>SDL_PROP_TEXTURE_CREATE_OPENGLES2_TEXTURE_UV_NUMBER</c></see></term>
	///			<description>The GLuint texture associated with the UV plane of an NV12 texture, if you want to wrap an existing texture</description>
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_TEXTURE_CREATE_OPENGLES2_TEXTURE_U_NUMBER"><c>SDL_PROP_TEXTURE_CREATE_OPENGLES2_TEXTURE_U_NUMBER</c></see></term>
	///			<description>The GLuint texture associated with the U plane of a YUV texture, if you want to wrap an existing texture</description>
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_TEXTURE_CREATE_OPENGLES2_TEXTURE_V_NUMBER"><c>SDL_PROP_TEXTURE_CREATE_OPENGLES2_TEXTURE_V_NUMBER</c></see></term>
	///			<description>The GLuint texture associated with the V plane of a YUV texture, if you want to wrap an existing texture</description>
	///		</item>
	/// </list>
	/// With the vulkan renderer:
	/// <list type="bullet">
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_TEXTURE_CREATE_VULKAN_TEXTURE_NUMBER"><c>SDL_PROP_TEXTURE_CREATE_VULKAN_TEXTURE_NUMBER</c></see></term>
	///			<description>The VkImage associated with the texture, if you want to wrap an existing texture</description>
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_TEXTURE_CREATE_VULKAN_LAYOUT_NUMBER"><c>SDL_PROP_TEXTURE_CREATE_VULKAN_LAYOUT_NUMBER</c></see></term>
	///			<description>The VkImageLayout for the VkImage, defaults to VK_IMAGE_LAYOUT_SHADER_READ_ONLY_OPTIMAL</description>
	///		</item>
	/// </list>
	/// With the GPU renderer:
	/// <list type="bullet">
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_TEXTURE_CREATE_GPU_TEXTURE_POINTER"><c>SDL_PROP_TEXTURE_CREATE_GPU_TEXTURE_POINTER</c></see></term>
	///			<description>The <see href="https://wiki.libsdl.org/SDL3/SDL_GPUTexture">SDL_GPUTexture</see> associated with the texture, if you want to wrap an existing texture</description>
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_TEXTURE_CREATE_GPU_TEXTURE_UV_NUMBER"><c>SDL_PROP_TEXTURE_CREATE_GPU_TEXTURE_UV_NUMBER</c></see></term>
	///			<description>The <see href="https://wiki.libsdl.org/SDL3/SDL_GPUTexture">SDL_GPUTexture</see> associated with the UV plane of an NV12 texture, if you want to wrap an existing texture</description>
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_TEXTURE_CREATE_GPU_TEXTURE_U_NUMBER"><c>SDL_PROP_TEXTURE_CREATE_GPU_TEXTURE_U_NUMBER</c></see></term>
	///			<description>The <see href="https://wiki.libsdl.org/SDL3/SDL_GPUTexture">SDL_GPUTexture</see> associated with the U plane of a YUV texture, if you want to wrap an existing texture</description>
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_TEXTURE_CREATE_GPU_TEXTURE_V_NUMBER"><c>SDL_PROP_TEXTURE_CREATE_GPU_TEXTURE_V_NUMBER</c></see></term>
	///			<description>the <see href="https://wiki.libsdl.org/SDL3/SDL_GPUTexture">SDL_GPUTexture</see> associated with the V plane of a YUV texture, if you want to wrap an existing texture</description>
	///		</item>
	/// </list>
	/// </para>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_CreateTextureWithProperties">SDL_CreateTextureWithProperties</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial SDL_Texture* SDL_CreateTextureWithProperties(IRenderer.SDL_Renderer* renderer, uint props);

	/// <summary>
	/// Destroy the specified texture
	/// </summary>
	/// <param name="texture">The texture to destroy</param>
	/// <remarks>
	/// <para>
	/// Passing NULL or an otherwise invalid texture will set the SDL error message to "Invalid texture".
	/// </para>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_DestroyTexture">SDL_DestroyTexture</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial void SDL_DestroyTexture(SDL_Texture* texture);

	/// <summary>
	/// Get the renderer that created an <see href="https://wiki.libsdl.org/SDL3/SDL_Texture">SDL_Texture</see>
	/// </summary>
	/// <param name="texture">The texture to query</param>
	/// <returns>Returns a pointer to the <see href="https://wiki.libsdl.org/SDL3/SDL_Renderer">SDL_Renderer</see> that created the texture, or NULL on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetRendererFromTexture">SDL_GetRendererFromTexture</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial IRenderer.SDL_Renderer* SDL_GetRendererFromTexture(SDL_Texture* texture);

	/// <summary>
	/// Get the additional alpha value multiplied into render copy operations
	/// </summary>
	/// <param name="texture">The texture to query</param>
	/// <param name="alpha">A pointer filled in with the current alpha value</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetTextureAlphaMod">SDL_GetTextureAlphaMod</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_GetTextureAlphaMod(SDL_Texture* texture, byte* alpha);

	/// <summary>
	/// Get the additional alpha value multiplied into render copy operations
	/// </summary>
	/// <param name="texture">The texture to query</param>
	/// <param name="alpha">A pointer filled in with the current alpha value</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetTextureAlphaModFloat">SDL_GetTextureAlphaModFloat</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_GetTextureAlphaModFloat(SDL_Texture* texture, float* alpha);

	/// <summary>
	/// Get the blend mode used for texture copy operations
	/// </summary>
	/// <param name="texture">The texture to query</param>
	/// <param name="blendMode">A pointer filled in with the current <see href="https://wiki.libsdl.org/SDL3/SDL_BlendMode">SDL_BlendMode</see></param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetTextureBlendMode">SDL_GetTextureBlendMode</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_GetTextureBlendMode(SDL_Texture* texture, BlendMode* blendMode);

	/// <summary>
	/// Get the additional color value multiplied into render copy operations
	/// </summary>
	/// <param name="texture">The texture to query</param>
	/// <param name="r">A pointer filled in with the current red color value</param>
	/// <param name="g">A pointer filled in with the current green color value</param>
	/// <param name="b">A pointer filled in with the current blue color value</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetTextureColorMod">SDL_GetTextureColorMod</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_GetTextureColorMod(SDL_Texture* texture, byte* r, byte* g, byte* b);

	/// <summary>
	/// Get the additional color value multiplied into render copy operations
	/// </summary>
	/// <param name="texture">The texture to query</param>
	/// <param name="r">A pointer filled in with the current red color value</param>
	/// <param name="g">A pointer filled in with the current green color value</param>
	/// <param name="b">A pointer filled in with the current blue color value</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetTextureColorMod">SDL_GetTextureColorMod</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_GetTextureColorModFloat(SDL_Texture* texture, float* r, float* g, float* b);

#if SDL3_4_0_OR_GREATER

	/// <summary>
	/// Get the palette used by a texture
	/// </summary>
	/// <param name="texture">The texture to query</param>
	/// <returns>Returns a pointer to the palette used by the texture, or NULL if there is no palette used</returns>
	/// <remarks>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetTexturePalette">SDL_GetTexturePalette</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial Palette.SDL_Palette* SDL_GetTexturePalette(SDL_Texture* texture);

#endif

	/// <summary>
	/// Get the properties associated with a texture
	/// </summary>
	/// <param name="texture">The texture to query</param>
	/// <returns>Returns a valid property ID on success or 0 on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// The following read-only properties are provided by SDL:
	/// <list type="bullet">
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_TEXTURE_COLORSPACE_NUMBER"><c>SDL_PROP_TEXTURE_COLORSPACE_NUMBER</c></see></term>
	///			<description>An <see href="https://wiki.libsdl.org/SDL3/SDL_Colorspace">SDL_Colorspace</see> value describing the texture colorspace</description>
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_TEXTURE_FORMAT_NUMBER"><c>SDL_PROP_TEXTURE_FORMAT_NUMBER</c></see></term>
	///			<description>One of the enumerated values in <see href="https://wiki.libsdl.org/SDL3/SDL_PixelFormat">SDL_PixelFormat</see></description>
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_TEXTURE_ACCESS_NUMBER"><c>SDL_PROP_TEXTURE_ACCESS_NUMBER</c></see></term>
	///			<description>One of the enumerated values in <see href="https://wiki.libsdl.org/SDL3/SDL_TextureAccess">SDL_TextureAccess</see></description>
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_TEXTURE_WIDTH_NUMBER"><c>SDL_PROP_TEXTURE_WIDTH_NUMBER</c></see></term>
	///			<description>The width of the texture in pixels</description>
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_TEXTURE_HEIGHT_NUMBER"><c>SDL_PROP_TEXTURE_HEIGHT_NUMBER</c></see></term>
	///			<description>The height of the texture in pixels</description>
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_TEXTURE_SDR_WHITE_POINT_FLOAT"><c>SDL_PROP_TEXTURE_SDR_WHITE_POINT_FLOAT</c></see></term>
	///			<description>
	///				For HDR10 and floating point textures, this defines the value of 100% diffuse white, with higher values being displayed in the High Dynamic Range headroom.
	///				This defaults to 100 for HDR10 textures and 1.0 for other textures.
	///			</description>
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_TEXTURE_HDR_HEADROOM_FLOAT"><c>SDL_PROP_TEXTURE_HDR_HEADROOM_FLOAT</c></see></term>
	///			<description>
	///				For HDR10 and floating point textures, this defines the maximum dynamic range used by the content, in terms of the SDR white point.
	///				If this is defined, any values outside the range supported by the display will be scaled into the available HDR headroom, otherwise they are clipped.
	///				This defaults to 1.0 for SDR textures, 4.0 for HDR10 textures, and no default for floating point textures.
	///			</description>
	///		</item>
	/// </list>
	/// With the direct3d11 renderer:
	/// <list type="bullet">
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_TEXTURE_D3D11_TEXTURE_POINTER"><c>SDL_PROP_TEXTURE_D3D11_TEXTURE_POINTER</c></see></term>
	///			<description>The ID3D11Texture2D associated with the texture</description>
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_TEXTURE_D3D11_TEXTURE_U_POINTER"><c>SDL_PROP_TEXTURE_D3D11_TEXTURE_U_POINTER</c></see></term>
	///			<description>The ID3D11Texture2D associated with the U plane of a YUV texture</description>
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_TEXTURE_D3D11_TEXTURE_V_POINTER"><c>SDL_PROP_TEXTURE_D3D11_TEXTURE_V_POINTER</c></see></term>
	///			<description>The ID3D11Texture2D associated with the V plane of a YUV texture</description>
	///		</item>
	/// </list>
	/// With the direct3d12 renderer:
	/// <list type="bullet">
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_TEXTURE_D3D12_TEXTURE_POINTER"><c>SDL_PROP_TEXTURE_D3D12_TEXTURE_POINTER</c></see></term>
	///			<description>The ID3D12Resource associated with the texture</description>
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_TEXTURE_D3D12_TEXTURE_U_POINTER"><c>SDL_PROP_TEXTURE_D3D12_TEXTURE_U_POINTER</c></see></term>
	///			<description>The ID3D12Resource associated with the U plane of a YUV texture</description>
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_TEXTURE_D3D12_TEXTURE_V_POINTER"><c>SDL_PROP_TEXTURE_D3D12_TEXTURE_V_POINTER</c></see></term>
	///			<description>The ID3D12Resource associated with the V plane of a YUV texture</description>
	///		</item>
	/// </list>
	/// With the vulkan renderer:
	/// <list type="bullet">
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_TEXTURE_VULKAN_TEXTURE_NUMBER"><c>SDL_PROP_TEXTURE_VULKAN_TEXTURE_NUMBER</c></see></term>
	///			<description>The VkImage associated with the texture</description>
	///		</item>
	/// </list>
	/// With the opengl renderer:
	/// <list type="bullet">
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_TEXTURE_OPENGL_TEXTURE_NUMBER"><c>SDL_PROP_TEXTURE_OPENGL_TEXTURE_NUMBER</c></see></term>
	///			<description>The GLuint texture associated with the texture</description>
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_TEXTURE_OPENGL_TEXTURE_UV_NUMBER"><c>SDL_PROP_TEXTURE_OPENGL_TEXTURE_UV_NUMBER</c></see></term>
	///			<description>The GLuint texture associated with the UV plane of an NV12 texture</description>
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_TEXTURE_OPENGL_TEXTURE_U_NUMBER"><c>SDL_PROP_TEXTURE_OPENGL_TEXTURE_U_NUMBER</c></see></term>
	///			<description>The GLuint texture associated with the U plane of a YUV texture</description>
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_TEXTURE_OPENGL_TEXTURE_V_NUMBER"><c>SDL_PROP_TEXTURE_OPENGL_TEXTURE_V_NUMBER</c></see></term>
	///			<description>The GLuint texture associated with the V plane of a YUV texture</description>
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_TEXTURE_OPENGL_TEXTURE_TARGET_NUMBER"><c>SDL_PROP_TEXTURE_OPENGL_TEXTURE_TARGET_NUMBER</c></see></term>
	///			<description>The GLenum for the texture target (<c>GL_TEXTURE_2D</c>, <c>GL_TEXTURE_RECTANGLE_ARB</c>, etc)</description>
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_TEXTURE_OPENGL_TEX_W_FLOAT"><c>SDL_PROP_TEXTURE_OPENGL_TEX_W_FLOAT</c></see></term>
	///			<description>The texture coordinate width of the texture (0.0 - 1.0)</description>
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_TEXTURE_OPENGL_TEX_H_FLOAT"><c>SDL_PROP_TEXTURE_OPENGL_TEX_H_FLOAT</c></see></term>
	///			<description>The texture coordinate height of the texture (0.0 - 1.0)</description>
	///		</item>
	/// </list>
	/// With the opengles2 renderer:
	/// <list type="bullet">
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_TEXTURE_OPENGLES2_TEXTURE_NUMBER"><c>SDL_PROP_TEXTURE_OPENGLES2_TEXTURE_NUMBER</c></see></term>
	///			<description>The GLuint texture associated with the texture</description>
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_TEXTURE_OPENGLES2_TEXTURE_UV_NUMBER"><c>SDL_PROP_TEXTURE_OPENGLES2_TEXTURE_UV_NUMBER</c></see></term>
	///			<description>The GLuint texture associated with the UV plane of an NV12 texture</description>
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_TEXTURE_OPENGLES2_TEXTURE_U_NUMBER"><c>SDL_PROP_TEXTURE_OPENGLES2_TEXTURE_U_NUMBER</c></see></term>
	///			<description>The GLuint texture associated with the U plane of a YUV texture</description>
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_TEXTURE_OPENGLES2_TEXTURE_V_NUMBER"><c>SDL_PROP_TEXTURE_OPENGLES2_TEXTURE_V_NUMBER</c></see></term>
	///			<description>The GLuint texture associated with the V plane of a YUV texture</description>
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_TEXTURE_OPENGLES2_TEXTURE_TARGET_NUMBER"><c>SDL_PROP_TEXTURE_OPENGLES2_TEXTURE_TARGET_NUMBER</c></see></term>
	///			<description>The GLenum for the texture target (<c>GL_TEXTURE_2D</c>, <c>GL_TEXTURE_EXTERNAL_OES</c>, etc)</description>
	///		</item>
	/// </list>
	/// With the gpu renderer:
	/// <list type="bullet">
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_TEXTURE_GPU_TEXTURE_POINTER"><c>SDL_PROP_TEXTURE_GPU_TEXTURE_POINTER</c></see></term>
	///			<description>The <see href="https://wiki.libsdl.org/SDL3/SDL_GPUTexture">SDL_GPUTexture</see> associated with the texture</description>
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_TEXTURE_GPU_TEXTURE_UV_POINTER"><c>SDL_PROP_TEXTURE_GPU_TEXTURE_UV_POINTER</c></see></term>
	///			<description>The <see href="https://wiki.libsdl.org/SDL3/SDL_GPUTexture">SDL_GPUTexture</see> associated with the UV plane of an NV12 texture</description>
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_TEXTURE_GPU_TEXTURE_U_POINTER"><c>SDL_PROP_TEXTURE_GPU_TEXTURE_U_POINTER</c></see></term>
	///			<description>The <see href="https://wiki.libsdl.org/SDL3/SDL_GPUTexture">SDL_GPUTexture</see> associated with the U plane of a YUV texture</description>
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_TEXTURE_GPU_TEXTURE_V_POINTER"><c>SDL_PROP_TEXTURE_GPU_TEXTURE_V_POINTER</c></see></term>
	///			<description>The <see href="https://wiki.libsdl.org/SDL3/SDL_GPUTexture">SDL_GPUTexture</see> associated with the V plane of a YUV texture</description>
	///		</item>
	/// </list>
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetTextureProperties">SDL_GetTextureProperties</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial uint SDL_GetTextureProperties(SDL_Texture* texture);

	/// <summary>
	/// Get the scale mode used for texture scale operations
	/// </summary>
	/// <param name="texture">The texture to query</param>
	/// <param name="scaleMode">A pointer filled in with the current scale mode</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetTextureScaleMode">SDL_GetTextureScaleMode</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_GetTextureScaleMode(SDL_Texture* texture, ScaleMode* scaleMode);

	/// <summary>
	/// Get the size of a texture, as floating point values
	/// </summary>
	/// <param name="texture">The texture to query</param>
	/// <param name="w">A pointer filled in with the width of the texture in pixels. This argument can be NULL if you don't need this information.</param>
	/// <param name="h">A pointer filled in with the height of the texture in pixels. This argument can be NULL if you don't need this information.</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetTextureSize">SDL_GetTextureSize</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_GetTextureSize(SDL_Texture* texture, float* w, float* h);

	/// <summary>
	/// Lock a portion of the texture for <em>write-only</em> pixel access
	/// </summary>
	/// <param name="texture">The texture to lock for access, which was created with <see href="https://wiki.libsdl.org/SDL3/SDL_TEXTUREACCESS_STREAMING">SDL_TEXTUREACCESS_STREAMING</see></param>
	/// <param name="rect">An <see href="https://wiki.libsdl.org/SDL3/SDL_Rect">SDL_Rect</see> structure representing the area to lock for access; NULL to lock the entire texture</param>
	/// <param name="pixels">This is filled in with a pointer to the locked pixels, appropriately offset by the locked area</param>
	/// <param name="pitch">This is filled in with the pitch of the locked pixels; the pitch is the length of one row in bytes</param>
	/// <returns>Returns true on success or false if the texture is not valid or was not created with <see href="https://wiki.libsdl.org/SDL3/SDL_TEXTUREACCESS_STREAMING">SDL_TEXTUREACCESS_STREAMING</see>; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// As an optimization, the pixels made available for editing don't necessarily contain the old texture data.
	/// This is a write-only operation, and if you need to keep a copy of the texture data you should do that at the application level.
	/// </para>
	/// <para>
	/// You must use <see href="https://wiki.libsdl.org/SDL3/SDL_UnlockTexture">SDL_UnlockTexture</see>() to unlock the pixels and apply any changes.
	/// </para>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_LockTexture">SDL_LockTexture</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_LockTexture(SDL_Texture* texture, Rect<int>* rect, void** pixels, int* pitch);

	/// <summary>
	/// Lock a portion of the texture for write-only pixel access, and expose it as a SDL surface
	/// </summary>
	/// <param name="texture">The texture to lock for access, which must be created with <see href="https://wiki.libsdl.org/SDL3/SDL_TEXTUREACCESS_STREAMING">SDL_TEXTUREACCESS_STREAMING</see></param>
	/// <param name="rect">An <see href="https://wiki.libsdl.org/SDL3/SDL_Rect">SDL_Rect</see> structure representing the area to lock for access; NULL to lock the entire texture</param>
	/// <param name="surface">A pointer to an SDL surface of size <em><paramref name="rect"/></em>. Don't assume any specific pixel content.</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// Besides providing an <see href="https://wiki.libsdl.org/SDL3/SDL_Surface">SDL_Surface</see> instead of raw pixel data, this function operates like <see href="https://wiki.libsdl.org/SDL3/SDL_LockTexture">SDL_LockTexture</see>.
	/// </para>
	/// <para>
	/// As an optimization, the pixels made available for editing don't necessarily contain the old texture data.
	/// This is a write-only operation, and if you need to keep a copy of the texture data you should do that at the application level.
	/// </para>
	/// <para>
	/// You must use <see href="https://wiki.libsdl.org/SDL3/SDL_UnlockTexture">SDL_UnlockTexture</see>() to unlock the pixels and apply any changes.
	/// </para>
	/// <para>
	/// The returned surface is freed internally after calling <see href="https://wiki.libsdl.org/SDL3/SDL_UnlockTexture">SDL_UnlockTexture</see>() or <see href="https://wiki.libsdl.org/SDL3/SDL_DestroyTexture">SDL_DestroyTexture</see>().
	/// The caller should not free it.
	/// </para>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_LockTextureToSurface">SDL_LockTextureToSurface</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_LockTextureToSurface(SDL_Texture* texture, Rect<int>* rect, Surface.SDL_Surface** surface);

	/// <summary>
	/// Set an additional alpha value multiplied into render copy operations
	/// </summary>
	/// <param name="texture">The texture to update</param>
	/// <param name="alpha">The source alpha value multiplied into copy operations</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// When this texture is rendered, during the copy operation the source alpha value is modulated by this alpha value according to the following formula:
	/// <code>
	/// srcA = srcA * (alpha / 255)
	/// </code>
	/// </para>
	/// <para>
	/// Alpha modulation is not always supported by the renderer; it will return false if alpha modulation is not supported.
	/// </para>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_SetTextureAlphaMod">SDL_SetTextureAlphaMod</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_SetTextureAlphaMod(SDL_Texture* texture, byte alpha);

	/// <summary>
	/// Set an additional alpha value multiplied into render copy operations
	/// </summary>
	/// <param name="texture">The texture to update</param>
	/// <param name="alpha">The source alpha value multiplied into copy operations</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// When this texture is rendered, during the copy operation the source alpha value is modulated by this alpha value according to the following formula:
	/// <code>
	/// srcA = srcA * alpha
	/// </code>
	/// </para>
	/// <para>
	/// Alpha modulation is not always supported by the renderer; it will return false if alpha modulation is not supported.
	/// </para>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_SetTextureAlphaModFloat">SDL_SetTextureAlphaModFloat</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_SetTextureAlphaModFloat(SDL_Texture* texture, float alpha);

	/// <summary>
	/// Set the blend mode for a texture, used by <see href="https://wiki.libsdl.org/SDL3/SDL_RenderTexture">SDL_RenderTexture</see>()
	/// </summary>
	/// <param name="texture">The texture to update</param>
	/// <param name="blendMode">The <see href="https://wiki.libsdl.org/SDL3/SDL_BlendMode">SDL_BlendMode</see> to use for texture blending</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// If the blend mode is not supported, the closest supported mode is chosen and this function returns false.
	/// </para>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_SetTextureBlendMode">SDL_SetTextureBlendMode</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_SetTextureBlendMode(SDL_Texture* texture, BlendMode blendMode);

	/// <summary>
	/// Set an additional color value multiplied into render copy operations
	/// </summary>
	/// <param name="texture">The texture to update</param>
	/// <param name="r">The red color value multiplied into copy operations</param>
	/// <param name="g">The green color value multiplied into copy operations</param>
	/// <param name="b">The blue color value multiplied into copy operations</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// When this texture is rendered, during the copy operation each source color channel is modulated by the appropriate color value according to the following formula:
	/// <code>
	/// srcC = srcC * (color / 255)
	/// </code>
	/// </para>
	/// <para>
	/// Color modulation is not always supported by the renderer; it will return false if color modulation is not supported.
	/// </para>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_SetTextureColorMod">SDL_SetTextureColorMod</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_SetTextureColorMod(SDL_Texture* texture, byte r, byte g, byte b);

	/// <summary>
	/// Set an additional color value multiplied into render copy operations
	/// </summary>
	/// <param name="texture">The texture to update</param>
	/// <param name="r">The red color value multiplied into copy operations</param>
	/// <param name="g">The green color value multiplied into copy operations</param>
	/// <param name="b">The blue color value multiplied into copy operations</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// When this texture is rendered, during the copy operation each source color channel is modulated by the appropriate color value according to the following formula:
	/// <code>
	/// srcC = srcC * color
	/// </code>
	/// </para>
	/// <para>
	/// Color modulation is not always supported by the renderer; it will return false if color modulation is not supported.
	/// </para>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_SetTextureColorMod">SDL_SetTextureColorMod</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_SetTextureColorModFloat(SDL_Texture* texture, float r, float g, float b);

#if SDL3_4_0_OR_GREATER

	/// <summary>
	/// Set the palette used by a texture
	/// </summary>
	/// <param name="texture">The texture to update</param>
	/// <param name="palette">The <see href="https://wiki.libsdl.org/SDL3/SDL_Palette">SDL_Palette</see> structure to use</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// Setting the palette keeps an internal reference to the palette, which can be safely destroyed afterwards.
	/// </para>
	/// <para>
	/// A single palette can be shared with many textures.
	/// </para>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_SetTexturePalette">SDL_SetTexturePalette</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_SetTexturePalette(SDL_Texture* texture, Palette.SDL_Palette* palette);

#endif

	/// <summary>
	/// Set the scale mode used for texture scale operations
	/// </summary>
	/// <param name="texture">The texture to update</param>
	/// <param name="scaleMode">The <see href="https://wiki.libsdl.org/SDL3/SDL_ScaleMode">SDL_ScaleMode</see> to use for texture scaling</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// The default texture scale mode is <see href="https://wiki.libsdl.org/SDL3/SDL_SCALEMODE_LINEAR">SDL_SCALEMODE_LINEAR</see>.
	/// </para>
	/// <para>
	/// If the scale mode is not supported, the closest supported mode is chosen.
	/// </para>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_SetTextureScaleMode">SDL_SetTextureScaleMode</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_SetTextureScaleMode(SDL_Texture* texture, ScaleMode scaleMode);

	/// <summary>
	/// Unlock a texture, uploading the changes to video memory, if needed
	/// </summary>
	/// <param name="texture">A texture locked by <see href="https://wiki.libsdl.org/SDL3/SDL_LockTexture">SDL_LockTexture</see>()</param>
	/// <remarks>
	/// <para>
	/// <em>Warning</em>: Please note that <see href="https://wiki.libsdl.org/SDL3/SDL_LockTexture">SDL_LockTexture</see>() is intended to be write-only; it will not guarantee the previous contents of the texture will be provided.
	/// You must fully initialize any area of a texture that you lock before unlocking it, as the pixels might otherwise be uninitialized memory.
	/// </para>
	/// <para>
	/// Which is to say: locking and immediately unlocking a texture can result in corrupted textures, depending on the renderer in use.
	/// </para>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_UnlockTexture">SDL_UnlockTexture</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial void SDL_UnlockTexture(SDL_Texture* texture);

	/// <summary>
	/// Update a rectangle within a planar NV12 or NV21 texture with new pixels
	/// </summary>
	/// <param name="texture">The texture to update</param>
	/// <param name="rect">A pointer to the rectangle of pixels to update, or NULL to update the entire texture</param>
	/// <param name="Yplane">The raw pixel data for the Y plane</param>
	/// <param name="Ypitch">The number of bytes between rows of pixel data for the Y plane</param>
	/// <param name="UVplane">The raw pixel data for the UV plane</param>
	/// <param name="UVpitch">The number of bytes between rows of pixel data for the UV plane</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// You can use <see href="https://wiki.libsdl.org/SDL3/SDL_UpdateTexture">SDL_UpdateTexture</see>() as long as your pixel data is a contiguous block of NV12/21 planes in the proper order, but this function is available if your pixel data is not contiguous.
	/// </para>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_UpdateNVTexture">SDL_UpdateNVTexture</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_UpdateNVTexture(SDL_Texture* texture, Rect<int>* rect, byte* Yplane, int Ypitch, byte* UVplane, int UVpitch);

	/// <summary>
	/// Update the given texture rectangle with new pixel data
	/// </summary>
	/// <param name="texture">The texture to update</param>
	/// <param name="rect">An <see href="https://wiki.libsdl.org/SDL3/SDL_Rect">SDL_Rect</see> structure representing the area to update, or NULL to update the entire texture</param>
	/// <param name="pixels">The raw pixel data in the format of the texture</param>
	/// <param name="pitch">The number of bytes in a row of pixel data, including padding between lines</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// The pixel data must be in the pixel format of the texture, which can be queried using the <see href="https://wiki.libsdl.org/SDL3/SDL_PROP_TEXTURE_FORMAT_NUMBER">SDL_PROP_TEXTURE_FORMAT_NUMBER</see> property.
	/// </para>
	/// <para>
	/// This is a fairly slow function, intended for use with static textures that do not change often.
	/// </para>
	/// <para>
	/// If the texture is intended to be updated often, it is preferred to create the texture as streaming and use the locking functions referenced below.
	/// While this function will work with streaming textures, for optimization reasons you may not get the pixels back if you lock the texture afterward.
	/// </para>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_UpdateTexture">SDL_UpdateTexture</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_UpdateTexture(SDL_Texture* texture, Rect<int>* rect, void* pixels, int pitch);

	/// <summary>
	/// Update a rectangle within a planar YV12 or IYUV texture with new pixel data
	/// </summary>
	/// <param name="texture">the texture to update</param>
	/// <param name="rect">A pointer to the rectangle of pixels to update, or NULL to update the entire texture</param>
	/// <param name="Yplane">The raw pixel data for the Y plane</param>
	/// <param name="Ypitch">The number of bytes between rows of pixel data for the Y plane</param>
	/// <param name="Uplane">The raw pixel data for the U plane</param>
	/// <param name="Upitch">The number of bytes between rows of pixel data for the U plane</param>
	/// <param name="Vplane">The raw pixel data for the V plane</param>
	/// <param name="Vpitch">The number of bytes between rows of pixel data for the V plane</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// You can use <see href="https://wiki.libsdl.org/SDL3/SDL_UpdateTexture">SDL_UpdateTexture</see>() as long as your pixel data is a contiguous block of Y and U/V planes in the proper order, but this function is available if your pixel data is not contiguous.
	/// </para>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_UpdateYUVTexture">SDL_UpdateYUVTexture</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_UpdateYUVTexture(SDL_Texture* texture, Rect<int>* rect, byte* Yplane, int Ypitch, byte* Uplane, int Upitch, byte* Vplane, int Vpitch);
}
