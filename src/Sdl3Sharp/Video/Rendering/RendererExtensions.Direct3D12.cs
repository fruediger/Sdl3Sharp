using Sdl3Sharp.Video.Coloring;
using Sdl3Sharp.Video.Rendering.Drivers;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Video.Rendering;

partial class RendererExtensions
{
	extension(Renderer<Direct3D12>.PropertyNames)
	{
		/// <summary>
		/// The name of a <em>read-only</em> <see cref="IRenderer.Properties">property</see> that holds
		/// a pointer to the <c><see href="https://docs.microsoft.com/en-us/windows/win32/api/d3d12/nn-d3d12-id3d12device">ID3D12Device</see></c> associated with the renderer
		/// </summary>
		public static string Direct3D12DevicePointer => "SDL.renderer.d3d12.device";

		/// <summary>
		/// The name of a <em>read-only</em> <see cref="IRenderer.Properties">property</see> that holds
		/// a pointer to the <c><see href="https://learn.microsoft.com/en-us/windows/win32/api/dxgi1_5/nn-dxgi1_5-idxgiswapchain4">IDXGISwapChain4</see></c> associated with the renderer
		/// </summary>
		public static string Direct3D12SwapChainPointer => "SDL.renderer.d3d12.swap_chain";

		/// <summary>
		/// The name of a <em>read-only</em> <see cref="IRenderer.Properties">property</see> that holds
		/// a pointer to the <c><see href="https://docs.microsoft.com/en-us/windows/win32/api/d3d12/nn-d3d12-id3d12commandqueue">ID3D12CommandQueue</see></c> associated with the renderer
		/// </summary>
		public static string Direct3D12CommandQueuePointer => "SDL.renderer.d3d12.command_queue";
	}

	extension(Renderer<Direct3D12> renderer)
	{
		/// <summary>
		/// Gets the <c><see href="https://docs.microsoft.com/en-us/windows/win32/api/d3d12/nn-d3d12-id3d12commandqueue">ID3D12CommandQueue</see></c> associated with the renderer
		/// </summary>
		/// <value>
		/// The <c><see href="https://docs.microsoft.com/en-us/windows/win32/api/d3d12/nn-d3d12-id3d12commandqueue">ID3D12CommandQueue</see></c> associated with the renderer
		/// </value>
		/// <remarks>
		/// <para>
		/// The value of this property can be directly cast to an <c><see href="https://docs.microsoft.com/en-us/windows/win32/api/d3d12/nn-d3d12-id3d12commandqueue">ID3D12CommandQueue</see>*</c> pointer.
		/// </para>
		/// </remarks>
		public IntPtr Direct3D12CommandQueue => renderer?.Properties?.TryGetPointerValue(Renderer<Direct3D12>.PropertyNames.Direct3D12CommandQueuePointer, out var direct3D12CommandQueue) is true
			? direct3D12CommandQueue
			: default;

		/// <summary>
		/// Gets the <c><see href="https://docs.microsoft.com/en-us/windows/win32/api/d3d12/nn-d3d12-id3d12device">ID3D12Device</see></c> associated with the renderer
		/// </summary>
		/// <value>
		/// The <c><see href="https://docs.microsoft.com/en-us/windows/win32/api/d3d12/nn-d3d12-id3d12device">ID3D12Device</see></c> associated with the renderer
		/// </value>
		/// <remarks>
		/// <para>
		/// The value of this property can be directly cast to an <c><see href="https://docs.microsoft.com/en-us/windows/win32/api/d3d12/nn-d3d12-id3d12device">ID3D12Device</see>*</c> pointer.
		/// </para>
		/// </remarks>
		public IntPtr Direct3D12Device => renderer?.Properties?.TryGetPointerValue(Renderer<Direct3D12>.PropertyNames.Direct3D12DevicePointer, out var direct3D12Device) is true
			? direct3D12Device
			: default;

		/// <summary>
		/// Gets the <c><see href="https://learn.microsoft.com/en-us/windows/win32/api/dxgi1_5/nn-dxgi1_5-idxgiswapchain4">IDXGISwapChain4</see></c> associated with the renderer
		/// </summary>
		/// <value>
		/// The <c><see href="https://learn.microsoft.com/en-us/windows/win32/api/dxgi1_5/nn-dxgi1_5-idxgiswapchain4">IDXGISwapChain4</see></c> associated with the renderer
		/// </value>
		/// <remarks>
		/// The value of this property can be directly cast to an <c><see href="https://learn.microsoft.com/en-us/windows/win32/api/dxgi1_5/nn-dxgi1_5-idxgiswapchain4">IDXGISwapChain4</see>*</c> pointer.
		/// </remarks>
		public IntPtr Direct3D12SwapChain => renderer?.Properties?.TryGetPointerValue(Renderer<Direct3D12>.PropertyNames.Direct3D12SwapChainPointer, out var direct3D12SwapChain) is true
			? direct3D12SwapChain
			: default;

		/// <inheritdoc cref="Renderer{TDriver}.TryCreateTexture(out Texture{TDriver}?, ColorSpace?, PixelFormat?, TextureAccess?, int?, int?, Palette?, float?, float?, Properties?)"/>
		/// <param name="direct3D12Texture">
		/// A pointer to an <c><see href="https://docs.microsoft.com/en-us/windows/win32/api/d3d12/nn-d3d12-id3d12resource">ID3D12Resource</see></c> to associate with the newly created texture, if you want to wrap an existing texture.
		/// Must be directly cast to an <see cref="IntPtr"/> from an <c><see href="https://docs.microsoft.com/en-us/windows/win32/api/d3d12/nn-d3d12-id3d12resource">ID3D12Resource</see>*</c> pointer.
		/// </param>
		/// <param name="direct3D12TextureU">
		/// A pointer to an <c><see href="https://docs.microsoft.com/en-us/windows/win32/api/d3d12/nn-d3d12-id3d12resource">ID3D12Resource</see></c> to associate with the U plane of the newly created YUV texture, if you want to wrap an existing texture.
		/// Must be directly cast to an <see cref="IntPtr"/> from an <c><see href="https://docs.microsoft.com/en-us/windows/win32/api/d3d12/nn-d3d12-id3d12resource">ID3D12Resource</see>*</c> pointer.
		/// </param>
		/// <param name="direct3D12TextureV">
		/// A pointer to an <c><see href="https://docs.microsoft.com/en-us/windows/win32/api/d3d12/nn-d3d12-id3d12resource">ID3D12Resource</see></c> to associate with the V plane of the newly created YUV texture, if you want to wrap an existing texture.
		/// Must be directly cast to an <see cref="IntPtr"/> from an <c><see href="https://docs.microsoft.com/en-us/windows/win32/api/d3d12/nn-d3d12-id3d12resource">ID3D12Resource</see>*</c> pointer.
		/// </param>
#pragma warning disable CS1573 // we get these from inheritdoc
		public bool TryCreateTexture([NotNullWhen(true)] out Texture<Direct3D12>? texture, ColorSpace? colorSpace = default, PixelFormat? format = default, TextureAccess? access = default, int? width = default, int? height = default,
#if SDL3_4_0_OR_GREATER
			Palette? palette = default,
#endif
			float? sdrWhitePoint = default, float? hdrHeadroom = default,
			IntPtr? direct3D12Texture = default, IntPtr? direct3D12TextureU = default, IntPtr? direct3D12TextureV = default, Properties? properties = default)
#pragma warning restore CS1573
		{
			if (renderer is null)
			{
				texture = null;
				return false;
			}

			Properties propertiesUsed;
			Unsafe.SkipInit(out IntPtr? direct3D12TextureBackup);
			Unsafe.SkipInit(out IntPtr? direct3D12TextureUBackup);
			Unsafe.SkipInit(out IntPtr? direct3D12TextureVBackup);

			if (properties is null)
			{
				propertiesUsed = [];

				if (direct3D12Texture is IntPtr direct3D12TextureValue)
				{
					propertiesUsed.TrySetPointerValue(Texture<Direct3D12>.PropertyNames.CreateDirect3D12TexturePointer, direct3D12TextureValue);
				}

				if (direct3D12TextureU is IntPtr direct3D12TextureUValue)
				{
					propertiesUsed.TrySetPointerValue(Texture<Direct3D12>.PropertyNames.CreateDirect3D12TextureUPointer, direct3D12TextureUValue);
				}

				if (direct3D12TextureV is IntPtr direct3D12TextureVValue)
				{
					propertiesUsed.TrySetPointerValue(Texture<Direct3D12>.PropertyNames.CreateDirect3D12TextureVPointer, direct3D12TextureVValue);
				}
			}
			else
			{
				propertiesUsed = properties;

				if (direct3D12Texture is IntPtr direct3D12TextureValue)
				{
					direct3D12TextureBackup = propertiesUsed.TryGetPointerValue(Texture<Direct3D12>.PropertyNames.CreateDirect3D12TexturePointer, out var existingDirect3D12TextureValue)
						? existingDirect3D12TextureValue
						: null;

					propertiesUsed.TrySetPointerValue(Texture<Direct3D12>.PropertyNames.CreateDirect3D12TexturePointer, direct3D12TextureValue);
				}

				if (direct3D12TextureU is IntPtr direct3D12TextureUValue)
				{
					direct3D12TextureUBackup = propertiesUsed.TryGetPointerValue(Texture<Direct3D12>.PropertyNames.CreateDirect3D12TextureUPointer, out var existingDirect3D12TextureUValue)
						? existingDirect3D12TextureUValue
						: null;

					propertiesUsed.TrySetPointerValue(Texture<Direct3D12>.PropertyNames.CreateDirect3D12TextureUPointer, direct3D12TextureUValue);
				}

				if (direct3D12TextureV is IntPtr direct3D12TextureVValue)
				{
					direct3D12TextureVBackup = propertiesUsed.TryGetPointerValue(Texture<Direct3D12>.PropertyNames.CreateDirect3D12TextureVPointer, out var existingDirect3D12TextureVValue)
						? existingDirect3D12TextureVValue
						: null;

					propertiesUsed.TrySetPointerValue(Texture<Direct3D12>.PropertyNames.CreateDirect3D12TextureVPointer, direct3D12TextureVValue);
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

					if (direct3D12Texture.HasValue)
					{
						if (direct3D12TextureBackup is IntPtr direct3D12TextureValue)
						{
							propertiesUsed.TrySetPointerValue(Texture<Direct3D12>.PropertyNames.CreateDirect3D12TexturePointer, direct3D12TextureValue);
						}
						else
						{
							propertiesUsed.TryRemove(Texture<Direct3D12>.PropertyNames.CreateDirect3D12TexturePointer);
						}
					}

					if (direct3D12TextureU.HasValue)
					{
						if (direct3D12TextureUBackup is IntPtr direct3D12TextureUValue)
						{
							propertiesUsed.TrySetPointerValue(Texture<Direct3D12>.PropertyNames.CreateDirect3D12TextureUPointer, direct3D12TextureUValue);
						}
						else
						{
							propertiesUsed.TryRemove(Texture<Direct3D12>.PropertyNames.CreateDirect3D12TextureUPointer);
						}
					}

					if (direct3D12TextureV.HasValue)
					{
						if (direct3D12TextureVBackup is IntPtr direct3D12TextureVValue)
						{
							propertiesUsed.TrySetPointerValue(Texture<Direct3D12>.PropertyNames.CreateDirect3D12TextureVPointer, direct3D12TextureVValue);
						}
						else
						{
							propertiesUsed.TryRemove(Texture<Direct3D12>.PropertyNames.CreateDirect3D12TextureVPointer);
						}
					}
				}
			}
		}
	}
}
