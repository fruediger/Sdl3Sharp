using Sdl3Sharp.Video.Rendering.Drivers;
using System;

namespace Sdl3Sharp.Video.Rendering;

partial class TextureExtensions
{
	extension(Texture<Direct3D12>.PropertyNames)
	{
		/// <summary>
		/// The name of a <see cref="RendererExtensions.TryCreateTexture(Renderer{Direct3D12}, out Texture{Direct3D12}?, Coloring.ColorSpace?, Coloring.PixelFormat?, TextureAccess?, int?, int?, Coloring.Palette?, float?, float?, nint?, nint?, nint?, Properties?)">property used when creating a <see cref="Texture{TDriver}">Texture&lt;<see cref="Direct3D12">Direct3D12</see>&gt;</see></see>
		/// that holds a pointer to the <c><see href="https://learn.microsoft.com/en-us/windows/win32/api/d3d12/nn-d3d12-id3d12resource">ID3D12Resource</see></c> associated with the texture, if you want to wrap an existing texture
		/// </summary>
		public static string CreateDirect3D12TexturePointer => "SDL.texture.create.d3d12.texture";

		/// <summary>
		/// The name of a <see cref="RendererExtensions.TryCreateTexture(Renderer{Direct3D12}, out Texture{Direct3D12}?, Coloring.ColorSpace?, Coloring.PixelFormat?, TextureAccess?, int?, int?, Coloring.Palette?, float?, float?, nint?, nint?, nint?, Properties?)">property used when creating a <see cref="Texture{TDriver}">Texture&lt;<see cref="Direct3D12">Direct3D12</see>&gt;</see></see>
		/// that holds a pointer to the <c><see href="https://learn.microsoft.com/en-us/windows/win32/api/d3d12/nn-d3d12-id3d12resource">ID3D12Resource</see></c> associated with the U plane of the YUV texture, if you want to wrap an existing texture
		/// </summary>
		public static string CreateDirect3D12TextureUPointer => "SDL.texture.create.d3d12.texture_u";

		/// <summary>
		/// The name of a <see cref="RendererExtensions.TryCreateTexture(Renderer{Direct3D12}, out Texture{Direct3D12}?, Coloring.ColorSpace?, Coloring.PixelFormat?, TextureAccess?, int?, int?, Coloring.Palette?, float?, float?, nint?, nint?, nint?, Properties?)">property used when creating a <see cref="Texture{TDriver}">Texture&lt;<see cref="Direct3D12">Direct3D12</see>&gt;</see></see>
		/// that holds a pointer to the <c><see href="https://learn.microsoft.com/en-us/windows/win32/api/d3d12/nn-d3d12-id3d12resource">ID3D12Resource</see></c> associated with the V plane of the YUV texture, if you want to wrap an existing texture
		/// </summary>
		public static string CreateDirect3D12TextureVPointer => "SDL.texture.create.d3d12.texture_v";

		/// <summary>
		/// The name of a <em>read-only</em> <see cref="ITexture.Properties">property</see> that holds
		/// a pointer to the <c><see href="https://learn.microsoft.com/en-us/windows/win32/api/d3d12/nn-d3d12-id3d12resource">ID3D12Resource</see></c> associated with the texture
		/// </summary>
		public static string Direct3D12TexturePointer => "SDL.texture.d3d12.texture";

		/// <summary>
		/// The name of a <em>read-only</em> <see cref="ITexture.Properties">property</see> that holds
		/// a pointer to the <c><see href="https://learn.microsoft.com/en-us/windows/win32/api/d3d12/nn-d3d12-id3d12resource">ID3D12Resource</see></c> associated with the U plane of the YUV texture
		/// </summary>
		public static string Direct3D12TextureUPointer => "SDL.texture.d3d12.texture_u";

		/// <summary>
		/// The name of a <em>read-only</em> <see cref="ITexture.Properties">property</see> that holds
		/// a pointer to the <c><see href="https://learn.microsoft.com/en-us/windows/win32/api/d3d12/nn-d3d12-id3d12resource">ID3D12Resource</see></c> associated with the V plane of the YUV texture
		/// </summary>
		public static string Direct3D12TextureVPointer => "SDL.texture.d3d12.texture_v";
	}

	extension(Texture<Direct3D12> texture)
	{
		/// <summary>
		/// Gets the <c><see href="https://learn.microsoft.com/en-us/windows/win32/api/d3d12/nn-d3d12-id3d12resource">ID3D12Resource</see></c> associated with the texture
		/// </summary>
		/// <value>
		/// The <c><see href="https://learn.microsoft.com/en-us/windows/win32/api/d3d12/nn-d3d12-id3d12resource">ID3D12Resource</see></c> associated with the texture
		/// </value>
		/// <remarks>
		/// <para>
		/// The value of this property can be directly cast to an <c><see href="https://learn.microsoft.com/en-us/windows/win32/api/d3d12/nn-d3d12-id3d12resource">ID3D12Resource</see>*</c> pointer.
		/// </para>
		/// </remarks>
		public IntPtr Direct3D12Texture => texture?.Properties?.TryGetPointerValue(Texture<Direct3D12>.PropertyNames.Direct3D12TexturePointer, out var direct3D12Texture) is true
			? direct3D12Texture
			: default;

		/// <summary>
		/// Gets the <c><see href="https://learn.microsoft.com/en-us/windows/win32/api/d3d12/nn-d3d12-id3d12resource">ID3D12Resource</see></c> associated with the U plane of the YUV texture
		/// </summary>
		/// <value>
		/// The <c><see href="https://learn.microsoft.com/en-us/windows/win32/api/d3d12/nn-d3d12-id3d12resource">ID3D12Resource</see></c> associated with the U plane of the YUV texture
		/// </value>
		/// <remarks>
		/// <para>
		/// The value of this property can be directly cast to an <c><see href="https://learn.microsoft.com/en-us/windows/win32/api/d3d12/nn-d3d12-id3d12resource">ID3D12Resource</see>*</c> pointer.
		/// </para>
		/// </remarks>
		public IntPtr Direct3D12TextureU => texture?.Properties?.TryGetPointerValue(Texture<Direct3D12>.PropertyNames.Direct3D12TextureUPointer, out var direct3D12TextureU) is true
			? direct3D12TextureU
			: default;

		/// <summary>
		/// Gets the <c><see href="https://learn.microsoft.com/en-us/windows/win32/api/d3d12/nn-d3d12-id3d12resource">ID3D12Resource</see></c> associated with the V plane of the YUV texture
		/// </summary>
		/// <value>
		/// The <c><see href="https://learn.microsoft.com/en-us/windows/win32/api/d3d12/nn-d3d12-id3d12resource">ID3D12Resource</see></c> associated with the V plane of the YUV texture
		/// </value>
		/// <remarks>
		/// <para>
		/// The value of this property can be directly cast to an <c><see href="https://learn.microsoft.com/en-us/windows/win32/api/d3d12/nn-d3d12-id3d12resource">ID3D12Resource</see>*</c> pointer.
		/// </para>
		/// </remarks>
		public IntPtr Direct3D12TextureV => texture?.Properties?.TryGetPointerValue(Texture<Direct3D12>.PropertyNames.Direct3D12TextureVPointer, out var direct3D12TextureV) is true
			? direct3D12TextureV
			: default;
	}
}
