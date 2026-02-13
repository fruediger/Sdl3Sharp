using Sdl3Sharp.Video.Coloring;
using Sdl3Sharp.Video.Rendering.Drivers;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Video.Rendering;

partial class RendererExtensions
{
	extension(Renderer<Direct3D11>.PropertyNames)
	{
		/// <summary>
		/// The name of a <em>read-only</em> <see cref="IRenderer.Properties">property</see> that holds
		/// a pointer to the <c><see href="https://docs.microsoft.com/en-us/windows/win32/api/d3d11/nn-d3d11-id3d11device">ID3D11Device</see></c> associated with the renderer
		/// </summary>
		public static string Direct3D11DevicePointer => "SDL.renderer.d3d11.device";

		/// <summary>
		/// The name of a <em>read-only</em> <see cref="IRenderer.Properties">property</see> that holds
		/// a pointer to the <c><see href="https://learn.microsoft.com/en-us/windows/win32/api/dxgi1_2/nn-dxgi1_2-idxgiswapchain1">IDXGISwapChain1</see></c> associated with the renderer
		/// </summary>
		public static string Direct3D11SwapChainPointer => "SDL.renderer.d3d11.swap_chain";
	}

	extension(Renderer<Direct3D11> renderer)
	{
		/// <summary>
		/// Gets the <c><see href="https://docs.microsoft.com/en-us/windows/win32/api/d3d11/nn-d3d11-id3d11device">ID3D11Device</see></c> associated with the renderer
		/// </summary>
		/// <value>
		/// The <c><see href="https://docs.microsoft.com/en-us/windows/win32/api/d3d11/nn-d3d11-id3d11device">ID3D11Device</see></c> associated with the renderer
		/// </value>
		/// <remarks>
		/// <para>
		/// The value of this property can be directly cast to an <c><see href="https://docs.microsoft.com/en-us/windows/win32/api/d3d11/nn-d3d11-id3d11device">ID3D11Device</see>*</c> pointer.
		/// </para>
		/// </remarks>
		public IntPtr Direct3D11Device => renderer?.Properties?.TryGetPointerValue(Renderer<Direct3D11>.PropertyNames.Direct3D11DevicePointer, out var direct3D11Device) is true
			? direct3D11Device
			: default;

		/// <summary>
		/// Gets the <c><see href="https://learn.microsoft.com/en-us/windows/win32/api/dxgi1_2/nn-dxgi1_2-idxgiswapchain1">IDXGISwapChain1</see></c> associated with the renderer
		/// </summary>
		/// <value>
		/// The <c><see href="https://learn.microsoft.com/en-us/windows/win32/api/dxgi1_2/nn-dxgi1_2-idxgiswapchain1">IDXGISwapChain1</see></c> associated with the renderer
		/// </value>
		/// <remarks>
		/// <para>
		/// The value of this property can be directly cast to an <c><see href="https://learn.microsoft.com/en-us/windows/win32/api/dxgi1_2/nn-dxgi1_2-idxgiswapchain1">IDXGISwapChain1</see>*</c> pointer.
		/// </para>
		/// </remarks>
		public IntPtr Direct3D11SwapChain => renderer?.Properties?.TryGetPointerValue(Renderer<Direct3D11>.PropertyNames.Direct3D11SwapChainPointer, out var direct3D11SwapChain) is true
			? direct3D11SwapChain
			: default;

		/// <inheritdoc cref="Renderer{TDriver}.TryCreateTexture(out Texture{TDriver}?, ColorSpace?, PixelFormat?, TextureAccess?, int?, int?, Palette?, float?, float?, Properties?)"/>
		/// <param name="direct3D11Texture">
		/// A pointer to an <c><see href="https://docs.microsoft.com/en-us/windows/win32/api/d3d11/nn-d3d11-id3d11texture2d">ID3D11Texture2D</see></c> to associate the the newly created texture, if you want to wrap an existing texture.
		/// Must be directly cast to an <see cref="IntPtr"/> from an <c><see href="https://docs.microsoft.com/en-us/windows/win32/api/d3d11/nn-d3d11-id3d11texture2d">ID3D11Texture2D</see>*</c> pointer.
		/// </param>
		/// <param name="direct3D11TextureU">
		/// A pointer to an <c><see href="https://docs.microsoft.com/en-us/windows/win32/api/d3d11/nn-d3d11-id3d11texture2d">ID3D11Texture2D</see></c> to associate with the U plane of the newly created YUV texture, if you want to wrap an existing texture.
		/// Must be directly cast to an <see cref="IntPtr"/> from an <c><see href="https://docs.microsoft.com/en-us/windows/win32/api/d3d11/nn-d3d11-id3d11texture2d">ID3D11Texture2D</see>*</c> pointer.
		/// </param>
		/// <param name="direct3D11TextureV">
		/// A pointer to an <c><see href="https://docs.microsoft.com/en-us/windows/win32/api/d3d11/nn-d3d11-id3d11texture2d">ID3D11Texture2D</see></c> to associate with the V plane of the newly created YUV texture, if you want to wrap an existing texture.
		/// Must be directly cast to an <see cref="IntPtr"/> from an <c><see href="https://docs.microsoft.com/en-us/windows/win32/api/d3d11/nn-d3d11-id3d11texture2d">ID3D11Texture2D</see>*</c> pointer.
		/// </param>
#pragma warning disable CS1573 // we get these from inheritdoc
		public bool TryCreateTexture([NotNullWhen(true)] out Texture<Direct3D11>? texture, ColorSpace? colorSpace = default, PixelFormat? format = default, TextureAccess? access = default, int? width = default, int? height = default,
#if SDL3_4_0_OR_GREATER
			Palette? palette = default,
#endif
			float? sdrWhitePoint = default, float? hdrHeadroom = default,
			IntPtr? direct3D11Texture = default, IntPtr? direct3D11TextureU = default, IntPtr? direct3D11TextureV = default, Properties? properties = default)
#pragma warning restore CS1573
		{
			if (renderer is null)
			{
				texture = null;
				return false;
			}

			Properties propertiesUsed;
			Unsafe.SkipInit(out IntPtr? direct3D11TextureBackup);
			Unsafe.SkipInit(out IntPtr? direct3D11TextureUBackup);
			Unsafe.SkipInit(out IntPtr? direct3D11TextureVBackup);

			if (properties is null)
			{
				propertiesUsed = [];

				if (direct3D11Texture is IntPtr direct3D11TextureValue)
				{
					propertiesUsed.TrySetPointerValue(Texture<Direct3D11>.PropertyNames.CreateDirect3D11TexturePointer, direct3D11TextureValue);
				}

				if (direct3D11TextureU is IntPtr direct3D11TextureUValue)
				{
					propertiesUsed.TrySetPointerValue(Texture<Direct3D11>.PropertyNames.CreateDirect3D11TextureUPointer, direct3D11TextureUValue);
				}

				if (direct3D11TextureV is IntPtr direct3D11TextureVValue)
				{
					propertiesUsed.TrySetPointerValue(Texture<Direct3D11>.PropertyNames.CreateDirect3D11TextureVPointer, direct3D11TextureVValue);
				}
			}
			else
			{
				propertiesUsed = properties;

				if (direct3D11Texture is IntPtr direct3D11TextureValue)
				{
					direct3D11TextureBackup = propertiesUsed.TryGetPointerValue(Texture<Direct3D11>.PropertyNames.CreateDirect3D11TexturePointer, out var existingDirect3D11TextureValue)
						? existingDirect3D11TextureValue
						: null;

					propertiesUsed.TrySetPointerValue(Texture<Direct3D11>.PropertyNames.CreateDirect3D11TexturePointer, direct3D11TextureValue);
				}

				if (direct3D11TextureU is IntPtr direct3D11TextureUValue)
				{
					direct3D11TextureUBackup = propertiesUsed.TryGetPointerValue(Texture<Direct3D11>.PropertyNames.CreateDirect3D11TextureUPointer, out var existingDirect3D11TextureUValue)
						? existingDirect3D11TextureUValue
						: null;

					propertiesUsed.TrySetPointerValue(Texture<Direct3D11>.PropertyNames.CreateDirect3D11TextureUPointer, direct3D11TextureUValue);
				}

				if (direct3D11TextureV is IntPtr direct3D11TextureVValue)
				{
					direct3D11TextureVBackup = propertiesUsed.TryGetPointerValue(Texture<Direct3D11>.PropertyNames.CreateDirect3D11TextureVPointer, out var existingDirect3D11TextureVValue)
						? existingDirect3D11TextureVValue
						: null;

					propertiesUsed.TrySetPointerValue(Texture<Direct3D11>.PropertyNames.CreateDirect3D11TextureVPointer, direct3D11TextureVValue);
				}
			}

			try
			{
				return renderer.TryCreateTexture(out texture, colorSpace, format, access, width, height,
#if SDL3_4_0_OR_GREATER
					palette,
#endif
					sdrWhitePoint, hdrHeadroom, propertiesUsed);
			}
			finally
			{
				if (properties is null)
				{
					// propertiesUsed was just a temporary instance we created for this call, so we need to dispose it now

					propertiesUsed.Dispose();
				}
				else
				{
					// we restored the original properties values from the given properties instance

					if (direct3D11Texture.HasValue)
					{
						if (direct3D11TextureBackup is IntPtr direct3D11TextureValue)
						{
							propertiesUsed.TrySetPointerValue(Texture<Direct3D11>.PropertyNames.CreateDirect3D11TexturePointer, direct3D11TextureValue);
						}
						else
						{
							propertiesUsed.TryRemove(Texture<Direct3D11>.PropertyNames.CreateDirect3D11TexturePointer);
						}
					}

					if (direct3D11TextureU.HasValue)
					{
						if (direct3D11TextureUBackup is IntPtr direct3D11TextureUValue)
						{
							propertiesUsed.TrySetPointerValue(Texture<Direct3D11>.PropertyNames.CreateDirect3D11TextureUPointer, direct3D11TextureUValue);
						}
						else
						{
							propertiesUsed.TryRemove(Texture<Direct3D11>.PropertyNames.CreateDirect3D11TextureUPointer);
						}
					}

					if (direct3D11TextureV.HasValue)
					{
						if (direct3D11TextureVBackup is IntPtr direct3D11TextureVValue)
						{
							propertiesUsed.TrySetPointerValue(Texture<Direct3D11>.PropertyNames.CreateDirect3D11TextureVPointer, direct3D11TextureVValue);
						}
						else
						{
							propertiesUsed.TryRemove(Texture<Direct3D11>.PropertyNames.CreateDirect3D11TextureVPointer);
						}
					}
				}
			}
		}
	}
}
