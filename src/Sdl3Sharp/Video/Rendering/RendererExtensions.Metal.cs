using Sdl3Sharp.Video.Coloring;
using Sdl3Sharp.Video.Rendering.Drivers;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Video.Rendering;

partial class RendererExtensions
{
	extension(Renderer<Metal> renderer)
	{
		/// <summary>
		/// Gets the Metal command encoder for the current frame
		/// </summary>
		/// <value>
		/// The Metal command encoder for the current frame, as an <c>id&lt;<see href="https://developer.apple.com/documentation/metal/mtlcommandencoder">MTLCommandEncoder</see>&gt;</c> handle
		/// </value>
		/// <remarks>
		/// <para>
		/// The value of this property can be directly cast to an <c>id&lt;<see href="https://developer.apple.com/documentation/metal/mtlcommandencoder">MTLCommandEncoder</see>&gt;</c> handle.
		/// </para>
		/// <para>
		/// The value of this property can be <see langword="null"/> (<see cref="IntPtr.Zero"/>) if Metal refuses to give SDL a drawable to render to, which might happen if the window is hidden, minimized, or off-screen.
		/// However, this doesn't apply to command encoders for render targets though, just the window's backbuffer.
		/// </para>
		/// <para>
		/// This property should only be accessed from the main thread.
		/// </para>
		/// </remarks>
		public IntPtr MetalCommandEncoder
		{
			get
			{
				unsafe
				{
					return unchecked((IntPtr)SDL_GetRenderMetalCommandEncoder(renderer is not null ? renderer.Pointer : null));
				}
			}
		}

		/// <summary>
		/// Gets the <c><see href="https://developer.apple.com/documentation/quartzcore/cametallayer">CAMetalLayer</see></c> associated with the renderer
		/// </summary>
		/// <value>
		/// The <c><see href="https://developer.apple.com/documentation/quartzcore/cametallayer">CAMetalLayer</see></c> associated with the renderer
		/// </value>
		/// <remarks>
		/// <para>
		/// The value of this property can be directly cast to an <c><see href="https://developer.apple.com/documentation/quartzcore/cametallayer">CAMetalLayer</see>*</c> pointer.
		/// </para>
		/// </remarks>
		public IntPtr MetalLayer
		{
			get
			{
				unsafe
				{
					return unchecked((IntPtr)SDL_GetRenderMetalLayer(renderer is not null ? renderer.Pointer : null));
				}
			}
		}

		/// <inheritdoc cref="Renderer{TDriver}.TryCreateTexture(out Texture{TDriver}?, ColorSpace?, PixelFormat?, TextureAccess?, int?, int?, Palette?, float?, float?, Properties?)"/>
		/// <param name="metalPixelBuffer">
		/// A pointer to a <c><see href="https://developer.apple.com/documentation/corevideo/cvpixelbuffer">CVPixelBuffer</see></c>, if you want to create the texture from an existing pixel buffer.
		/// Must be directly cast to an <see cref="IntPtr"/> from a <c><see href="https://developer.apple.com/documentation/corevideo/cvpixelbuffer">CVPixelBuffer</see>*</c> pointer or an <c>CVPixelBufferRef</c> handle.
		/// </param>
#pragma warning disable CS1573 // we get these from inheritdoc
		public bool TryCreateTexture([NotNullWhen(true)] out Texture<Metal>? texture, ColorSpace? colorSpace = default, PixelFormat? format = default, TextureAccess? access = default, int? width = default, int? height = default,
#if SDL3_4_0_OR_GREATER
			Palette? palette = default,
#endif
			float? sdrWhitePoint = default, float? hdrHeadroom = default,
			IntPtr? metalPixelBuffer = default, Properties? properties = default)
#pragma warning restore CS1573
		{
			if (renderer is null)
			{
				texture = null;
				return false;
			}

			Properties propertiesUsed;
			Unsafe.SkipInit(out IntPtr? metalPixelBufferBackup);

			if (properties is null)
			{
				propertiesUsed = [];

				if (metalPixelBuffer is IntPtr metalPixelBufferValue)
				{
					propertiesUsed.TrySetPointerValue(Texture<Metal>.PropertyNames.CreateMetalPixelBufferPointer, metalPixelBufferValue);
				}
			}
			else
			{
				propertiesUsed = properties;

				if (metalPixelBuffer is IntPtr metalPixelBufferValue)
				{
					metalPixelBufferBackup = propertiesUsed.TryGetPointerValue(Texture<Metal>.PropertyNames.CreateMetalPixelBufferPointer, out var existingMetalPixelBuffer)
						? existingMetalPixelBuffer
						: null;

					propertiesUsed.TrySetPointerValue(Texture<Metal>.PropertyNames.CreateMetalPixelBufferPointer, metalPixelBufferValue);
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

					if (metalPixelBuffer.HasValue)
					{
						if (metalPixelBufferBackup is IntPtr metalPixelBufferValue)
						{
							propertiesUsed.TrySetPointerValue(Texture<Metal>.PropertyNames.CreateMetalPixelBufferPointer, metalPixelBufferValue);
						}
						else
						{
							propertiesUsed.TryRemove(Texture<Metal>.PropertyNames.CreateMetalPixelBufferPointer);
						}
					}
				}
			}
		}
	}
}
