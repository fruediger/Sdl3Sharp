using Sdl3Sharp.Video.Rendering.Drivers;
using System;

namespace Sdl3Sharp.Video.Rendering;

partial class TextureExtensions
{
	extension(Texture<Direct3D11>.PropertyNames)
	{
		/// <summary>
		/// The name of a <see cref="RendererExtensions.TryCreateTexture(Renderer{Direct3D11}, out Texture{Direct3D11}?, Coloring.ColorSpace?, Coloring.PixelFormat?, TextureAccess?, int?, int?, Coloring.Palette?, float?, float?, nint?, nint?, nint?, Properties?)">property used when creating a <see cref="Texture{TDriver}">Texture&lt;<see cref="Direct3D11">Direct3D11</see>&gt;</see></see>
		/// that holds a pointer to the <c><see href="https://learn.microsoft.com/en-us/windows/win32/api/d3d11/nn-d3d11-id3d11texture2d">ID3D11Texture2D</see></c> associated with the texture, if you want to wrap an existing texture
		/// </summary>
		public static string CreateDirect3D11TexturePointer => "SDL.texture.create.d3d11.texture";

		/// <summary>
		/// The name of a <see cref="RendererExtensions.TryCreateTexture(Renderer{Direct3D11}, out Texture{Direct3D11}?, Coloring.ColorSpace?, Coloring.PixelFormat?, TextureAccess?, int?, int?, Coloring.Palette?, float?, float?, nint?, nint?, nint?, Properties?)">property used when creating a <see cref="Texture{TDriver}">Texture&lt;<see cref="Direct3D11">Direct3D11</see>&gt;</see></see>
		/// that holds a pointer to the <c><see href="https://learn.microsoft.com/en-us/windows/win32/api/d3d11/nn-d3d11-id3d11texture2d">ID3D11Texture2D</see></c> associated with the U plane of the YUV texture, if you want to wrap an existing texture
		/// </summary>
		public static string CreateDirect3D11TextureUPointer => "SDL.texture.create.d3d11.texture_u";

		/// <summary>
		/// The name of a <see cref="RendererExtensions.TryCreateTexture(Renderer{Direct3D11}, out Texture{Direct3D11}?, Coloring.ColorSpace?, Coloring.PixelFormat?, TextureAccess?, int?, int?, Coloring.Palette?, float?, float?, nint?, nint?, nint?, Properties?)">property used when creating a <see cref="Texture{TDriver}">Texture&lt;<see cref="Direct3D11">Direct3D11</see>&gt;</see></see>
		/// that holds a pointer to the <c><see href="https://learn.microsoft.com/en-us/windows/win32/api/d3d11/nn-d3d11-id3d11texture2d">ID3D11Texture2D</see></c> associated with the V plane of the YUV texture, if you want to wrap an existing texture
		/// </summary>
		public static string CreateDirect3D11TextureVPointer => "SDL.texture.create.d3d11.texture_v";

		/// <summary>
		/// The name of a <em>read-only</em> <see cref="ITexture.Properties">property</see> that holds
		/// a pointer to the <c><see href="https://learn.microsoft.com/en-us/windows/win32/api/d3d11/nn-d3d11-id3d11texture2d">ID3D11Texture2D</see></c> associated with the texture
		/// </summary>
		public static string Direct3D11TexturePointer => "SDL.texture.d3d11.texture";

		/// <summary>
		/// The name of a <em>read-only</em> <see cref="ITexture.Properties">property</see> that holds
		/// a pointer to the <c><see href="https://learn.microsoft.com/en-us/windows/win32/api/d3d11/nn-d3d11-id3d11texture2d">ID3D11Texture2D</see></c> associated with the U plane of the YUV texture
		/// </summary>
		public static string Direct3D11TextureUPointer => "SDL.texture.d3d11.texture_u";

		/// <summary>
		/// The name of a <em>read-only</em> <see cref="ITexture.Properties">property</see> that holds
		/// a pointer to the <c><see href="https://learn.microsoft.com/en-us/windows/win32/api/d3d11/nn-d3d11-id3d11texture2d">ID3D11Texture2D</see></c> associated with the V plane of the YUV texture
		/// </summary>
		public static string Direct3D11TextureVPointer => "SDL.texture.d3d11.texture_v";
	}

	extension(Texture<Direct3D11> texture)
	{
		/// <summary>
		/// Gets the <c><see href="https://learn.microsoft.com/en-us/windows/win32/api/d3d11/nn-d3d11-id3d11texture2d">ID3D11Texture2D</see></c> associated with the texture
		/// </summary>
		/// <value>
		/// The <c><see href="https://learn.microsoft.com/en-us/windows/win32/api/d3d11/nn-d3d11-id3d11texture2d">ID3D11Texture2D</see></c> associated with the texture
		/// </value>
		/// <remarks>
		/// <para>
		/// The value of this property can be directly cast to an <c><see href="https://learn.microsoft.com/en-us/windows/win32/api/d3d11/nn-d3d11-id3d11texture2d">ID3D11Texture2D</see>*</c> pointer.
		/// </para>
		/// </remarks>
		public IntPtr Direct3D11Texture => texture?.Properties?.TryGetPointerValue(Texture<Direct3D11>.PropertyNames.Direct3D11TexturePointer, out var direct3D11Texture) is true
			? direct3D11Texture
			: default;

		/// <summary>
		/// Gets the <c><see href="https://learn.microsoft.com/en-us/windows/win32/api/d3d11/nn-d3d11-id3d11texture2d">ID3D11Texture2D</see></c> associated with the U plane of the YUV texture
		/// </summary>
		/// <value>
		/// The <c><see href="https://learn.microsoft.com/en-us/windows/win32/api/d3d11/nn-d3d11-id3d11texture2d">ID3D11Texture2D</see></c> associated with the U plane of the YUV texture
		/// </value>
		/// <remarks>
		/// <para>
		/// The value of this property can be directly cast to an <c><see href="https://learn.microsoft.com/en-us/windows/win32/api/d3d11/nn-d3d11-id3d11texture2d">ID3D11Texture2D</see>*</c> pointer.
		/// </para>
		/// </remarks>
		public IntPtr Direct3D11TextureU => texture?.Properties?.TryGetPointerValue(Texture<Direct3D11>.PropertyNames.Direct3D11TextureUPointer, out var direct3D11TextureU) is true
			? direct3D11TextureU
			: default;

		/// <summary>
		/// Gets the <c><see href="https://learn.microsoft.com/en-us/windows/win32/api/d3d11/nn-d3d11-id3d11texture2d">ID3D11Texture2D</see></c> associated with the V plane of the YUV texture
		/// </summary>
		/// <value>
		/// The <c><see href="https://learn.microsoft.com/en-us/windows/win32/api/d3d11/nn-d3d11-id3d11texture2d">ID3D11Texture2D</see></c> associated with the V plane of the YUV texture
		/// </value>
		/// <remarks>
		/// <para>
		/// The value of this property can be directly cast to an <c><see href="https://learn.microsoft.com/en-us/windows/win32/api/d3d11/nn-d3d11-id3d11texture2d">ID3D11Texture2D</see>*</c> pointer.
		/// </para>
		/// </remarks>
		public IntPtr Direct3D11TextureV => texture?.Properties?.TryGetPointerValue(Texture<Direct3D11>.PropertyNames.Direct3D11TextureVPointer, out var direct3D11TextureV) is true
			? direct3D11TextureV
			: default;
	}
}
