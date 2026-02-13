using Sdl3Sharp.Video.Coloring;
using Sdl3Sharp.Video.Rendering.Drivers;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Video.Rendering;

partial class RendererExtensions
{
	extension(Renderer<OpenGl> renderer)
	{
		/// <inheritdoc cref="Renderer{TDriver}.TryCreateTexture(out Texture{TDriver}?, ColorSpace?, PixelFormat?, TextureAccess?, int?, int?, Palette?, float?, float?, Properties?)"/>
		/// <param name="openGlTexture">A <c>GLuint</c> texture to associate with the newly created texture, if you want to wrap an existing texture</param>
		/// <param name="openGlTextureUv">A <c>GLuint</c> texture to associate with the UV plane of the newly created NV12 texture, if you want to wrap an existing texture</param>
		/// <param name="openGlTextureU">A <c>GLuint</c> texture to associate with the U plane of the newly created YUV texture, if you want to wrap an existing texture</param>
		/// <param name="openGlTextureV">A <c>GLuint</c> texture to associate with the V plane of the newly created YUV texture, if you want to wrap an existing texture</param>
#pragma warning disable CS1573 // we get these from inheritdoc
		public bool TryCreateTexture([NotNullWhen(true)] out Texture<OpenGl>? texture, ColorSpace? colorSpace = default, PixelFormat? format = default, TextureAccess? access = default, int? width = default, int? height = default,
#if SDL3_4_0_OR_GREATER
			Palette? palette = default,
#endif
			float? sdrWhitePoint = default, float? hdrHeadroom = default,
			uint? openGlTexture = default, uint? openGlTextureUv = default, uint? openGlTextureU = default, uint? openGlTextureV = default, Properties? properties = default)
#pragma warning restore CS1573
		{
			if (renderer is null)
			{
				texture = null;
				return false;
			}

			Properties propertiesUsed;
			Unsafe.SkipInit(out uint? openGlTextureBackup);
			Unsafe.SkipInit(out uint? openGlTextureUvBackup);
			Unsafe.SkipInit(out uint? openGlTextureUBackup);
			Unsafe.SkipInit(out uint? openGlTextureVBackup);

			if (properties is null)
			{
				propertiesUsed = [];

				if (openGlTexture is uint openGlTextureValue)
				{
					propertiesUsed.TrySetNumberValue(Texture<OpenGl>.PropertyNames.CreateOpenGlTextureNumber, openGlTextureValue);
				}

				if (openGlTextureUv is uint openGlTextureUvValue)
				{
					propertiesUsed.TrySetNumberValue(Texture<OpenGl>.PropertyNames.CreateOpenGlTextureUvNumber, openGlTextureUvValue);
				}

				if (openGlTextureU is uint openGlTextureUValue)
				{
					propertiesUsed.TrySetNumberValue(Texture<OpenGl>.PropertyNames.CreateOpenGlTextureUNumber, openGlTextureUValue);
				}

				if (openGlTextureV is uint openGlTextureVValue)
				{
					propertiesUsed.TrySetNumberValue(Texture<OpenGl>.PropertyNames.CreateOpenGlTextureVNumber, openGlTextureVValue);
				}
			}
			else
			{
				propertiesUsed = properties;

				if (openGlTexture is uint openGlTextureValue)
				{
					openGlTextureBackup = propertiesUsed.TryGetNumberValue(Texture<OpenGl>.PropertyNames.CreateOpenGlTextureNumber, out var existingOpenGlTextureValue)
						? unchecked((uint)existingOpenGlTextureValue)
						: null;

					propertiesUsed.TrySetNumberValue(Texture<OpenGl>.PropertyNames.CreateOpenGlTextureNumber, openGlTextureValue);
				}

				if (openGlTextureUv is uint openGlTextureUvValue)
				{
					openGlTextureUvBackup = propertiesUsed.TryGetNumberValue(Texture<OpenGl>.PropertyNames.CreateOpenGlTextureUvNumber, out var existingOpenGlTextureUvValue)
						? unchecked((uint)existingOpenGlTextureUvValue)
						: null;

					propertiesUsed.TrySetNumberValue(Texture<OpenGl>.PropertyNames.CreateOpenGlTextureUvNumber, openGlTextureUvValue);
				}

				if (openGlTextureU is uint openGlTextureUValue)
				{
					openGlTextureUBackup = propertiesUsed.TryGetNumberValue(Texture<OpenGl>.PropertyNames.CreateOpenGlTextureUNumber, out var existingOpenGlTextureUValue)
						? unchecked((uint)existingOpenGlTextureUValue)
						: null;

					propertiesUsed.TrySetNumberValue(Texture<OpenGl>.PropertyNames.CreateOpenGlTextureUNumber, openGlTextureUValue);
				}

				if (openGlTextureV is uint openGlTextureVValue)
				{
					openGlTextureVBackup = propertiesUsed.TryGetNumberValue(Texture<OpenGl>.PropertyNames.CreateOpenGlTextureVNumber, out var existingOpenGlTextureVValue)
						? unchecked((uint)existingOpenGlTextureVValue)
						: null;

					propertiesUsed.TrySetNumberValue(Texture<OpenGl>.PropertyNames.CreateOpenGlTextureVNumber, openGlTextureVValue);
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

					if (openGlTexture.HasValue)
					{
						if (openGlTextureBackup is uint openGlTextureValue)
						{
							propertiesUsed.TrySetNumberValue(Texture<OpenGl>.PropertyNames.CreateOpenGlTextureNumber, openGlTextureValue);
						}
						else
						{
							propertiesUsed.TryRemove(Texture<OpenGl>.PropertyNames.CreateOpenGlTextureNumber);
						}
					}

					if (openGlTextureUv.HasValue)
					{
						if (openGlTextureUvBackup is uint openGlTextureUvValue)
						{
							propertiesUsed.TrySetNumberValue(Texture<OpenGl>.PropertyNames.CreateOpenGlTextureUvNumber, openGlTextureUvValue);
						}
						else
						{
							propertiesUsed.TryRemove(Texture<OpenGl>.PropertyNames.CreateOpenGlTextureUvNumber);
						}
					}

					if (openGlTextureU.HasValue)
					{
						if (openGlTextureUBackup is uint openGlTextureUValue)
						{
							propertiesUsed.TrySetNumberValue(Texture<OpenGl>.PropertyNames.CreateOpenGlTextureUNumber, openGlTextureUValue);
						}
						else
						{
							propertiesUsed.TryRemove(Texture<Direct3D12>.PropertyNames.CreateDirect3D12TextureUPointer);
						}
					}

					if (openGlTextureV.HasValue)
					{
						if (openGlTextureVBackup is uint openGlTextureVValue)
						{
							propertiesUsed.TrySetNumberValue(Texture<OpenGl>.PropertyNames.CreateOpenGlTextureVNumber, openGlTextureVValue);
						}
						else
						{
							propertiesUsed.TryRemove(Texture<OpenGl>.PropertyNames.CreateOpenGlTextureVNumber);
						}
					}
				}
			}
		}
	}
}
