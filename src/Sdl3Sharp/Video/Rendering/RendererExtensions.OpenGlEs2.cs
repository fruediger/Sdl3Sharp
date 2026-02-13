using Sdl3Sharp.Video.Coloring;
using Sdl3Sharp.Video.Rendering.Drivers;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Video.Rendering;

partial class RendererExtensions
{
	extension(Renderer<OpenGlEs2> renderer)
	{
		/// <inheritdoc cref="Renderer{TDriver}.TryCreateTexture(out Texture{TDriver}?, ColorSpace?, PixelFormat?, TextureAccess?, int?, int?, Palette?, float?, float?, Properties?)"/>
		/// <param name="openGlEs2Texture">A <c>GLuint</c> texture to associate with the newly created texture, if you want to wrap an existing texture</param>
		/// <param name="openGlEs2TextureUv">A <c>GLuint</c> texture to associate with the UV plane of the newly created NV12 texture, if you want to wrap an existing texture</param>
		/// <param name="openGlEs2TextureU">A <c>GLuint</c> texture to associate with the U plane of the newly created YUV texture, if you want to wrap an existing texture</param>
		/// <param name="openGlEs2TextureV">A <c>GLuint</c> texture to associate with the V plane of the newly created YUV texture, if you want to wrap an existing texture</param>
#pragma warning disable CS1573 // we get these from inheritdoc
		public bool TryCreateTexture([NotNullWhen(true)] out Texture<OpenGlEs2>? texture, ColorSpace? colorSpace = default, PixelFormat? format = default, TextureAccess? access = default, int? width = default, int? height = default,
#if SDL3_4_0_OR_GREATER
			Palette? palette = default,
#endif
			float? sdrWhitePoint = default, float? hdrHeadroom = default,
			uint? openGlEs2Texture = default, uint? openGlEs2TextureUv = default, uint? openGlEs2TextureU = default, uint? openGlEs2TextureV = default, Properties? properties = default)
#pragma warning restore CS1573
		{
			if (renderer is null)
			{
				texture = null;
				return false;
			}

			Properties propertiesUsed;
			Unsafe.SkipInit(out uint? openGlEs2TextureBackup);
			Unsafe.SkipInit(out uint? openGlEs2TextureUvBackup);
			Unsafe.SkipInit(out uint? openGlEs2TextureUBackup);
			Unsafe.SkipInit(out uint? openGlEs2TextureVBackup);

			if (properties is null)
			{
				propertiesUsed = [];

				if (openGlEs2Texture is uint openGlEs2TextureValue)
				{
					propertiesUsed.TrySetNumberValue(Texture<OpenGlEs2>.PropertyNames.CreateOpenGlEs2TextureNumber, openGlEs2TextureValue);
				}

				if (openGlEs2TextureUv is uint openGlEs2TextureUvValue)
				{
					propertiesUsed.TrySetNumberValue(Texture<OpenGlEs2>.PropertyNames.CreateOpenGlEs2TextureUvNumber, openGlEs2TextureUvValue);
				}

				if (openGlEs2TextureU is uint openGlEs2TextureUValue)
				{
					propertiesUsed.TrySetNumberValue(Texture<OpenGlEs2>.PropertyNames.CreateOpenGlEs2TextureUNumber, openGlEs2TextureUValue);
				}

				if (openGlEs2TextureV is uint openGlEs2TextureVValue)
				{
					propertiesUsed.TrySetNumberValue(Texture<OpenGlEs2>.PropertyNames.CreateOpenGlEs2TextureVNumber, openGlEs2TextureVValue);
				}
			}
			else
			{
				propertiesUsed = properties;

				if (openGlEs2Texture is uint openGlEs2TextureValue)
				{
					openGlEs2TextureBackup = propertiesUsed.TryGetNumberValue(Texture<OpenGlEs2>.PropertyNames.CreateOpenGlEs2TextureNumber, out var existingOpenGlEs2TextureValue)
						? unchecked((uint)existingOpenGlEs2TextureValue)
						: null;

					propertiesUsed.TrySetNumberValue(Texture<OpenGlEs2>.PropertyNames.CreateOpenGlEs2TextureNumber, openGlEs2TextureValue);
				}

				if (openGlEs2TextureUv is uint openGlEs2TextureUvValue)
				{
					openGlEs2TextureUvBackup = propertiesUsed.TryGetNumberValue(Texture<OpenGlEs2>.PropertyNames.CreateOpenGlEs2TextureUvNumber, out var existingOpenGlEs2TextureUvValue)
						? unchecked((uint)existingOpenGlEs2TextureUvValue)
						: null;

					propertiesUsed.TrySetNumberValue(Texture<OpenGlEs2>.PropertyNames.CreateOpenGlEs2TextureUvNumber, openGlEs2TextureUvValue);
				}

				if (openGlEs2TextureU is uint openGlEs2TextureUValue)
				{
					openGlEs2TextureUBackup = propertiesUsed.TryGetNumberValue(Texture<OpenGlEs2>.PropertyNames.CreateOpenGlEs2TextureUNumber, out var existingOpenGlEs2TextureUValue)
						? unchecked((uint)existingOpenGlEs2TextureUValue)
						: null;

					propertiesUsed.TrySetNumberValue(Texture<OpenGlEs2>.PropertyNames.CreateOpenGlEs2TextureUNumber, openGlEs2TextureUValue);
				}

				if (openGlEs2TextureV is uint openGlEs2TextureVValue)
				{
					openGlEs2TextureVBackup = propertiesUsed.TryGetNumberValue(Texture<OpenGlEs2>.PropertyNames.CreateOpenGlEs2TextureVNumber, out var existingOpenGlEs2TextureVValue)
						? unchecked((uint)existingOpenGlEs2TextureVValue)
						: null;

					propertiesUsed.TrySetNumberValue(Texture<OpenGlEs2>.PropertyNames.CreateOpenGlEs2TextureVNumber, openGlEs2TextureVValue);
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

					if (openGlEs2Texture.HasValue)
					{
						if (openGlEs2TextureBackup is uint openGlEs2TextureValue)
						{
							propertiesUsed.TrySetNumberValue(Texture<OpenGlEs2>.PropertyNames.CreateOpenGlEs2TextureNumber, openGlEs2TextureValue);
						}
						else
						{
							propertiesUsed.TryRemove(Texture<OpenGlEs2>.PropertyNames.CreateOpenGlEs2TextureNumber);
						}
					}

					if (openGlEs2TextureUv.HasValue)
					{
						if (openGlEs2TextureUvBackup is uint openGlEs2TextureUvValue)
						{
							propertiesUsed.TrySetNumberValue(Texture<OpenGlEs2>.PropertyNames.CreateOpenGlEs2TextureUvNumber, openGlEs2TextureUvValue);
						}
						else
						{
							propertiesUsed.TryRemove(Texture<OpenGlEs2>.PropertyNames.CreateOpenGlEs2TextureUvNumber);
						}
					}

					if (openGlEs2TextureU.HasValue)
					{
						if (openGlEs2TextureUBackup is uint openGlEs2TextureUValue)
						{
							propertiesUsed.TrySetNumberValue(Texture<OpenGlEs2>.PropertyNames.CreateOpenGlEs2TextureUNumber, openGlEs2TextureUValue);
						}
						else
						{
							propertiesUsed.TryRemove(Texture<Direct3D12>.PropertyNames.CreateDirect3D12TextureUPointer);
						}
					}

					if (openGlEs2TextureV.HasValue)
					{
						if (openGlEs2TextureVBackup is uint openGlEs2TextureVValue)
						{
							propertiesUsed.TrySetNumberValue(Texture<OpenGlEs2>.PropertyNames.CreateOpenGlEs2TextureVNumber, openGlEs2TextureVValue);
						}
						else
						{
							propertiesUsed.TryRemove(Texture<OpenGlEs2>.PropertyNames.CreateOpenGlEs2TextureVNumber);
						}
					}
				}
			}
		}
	}
}
