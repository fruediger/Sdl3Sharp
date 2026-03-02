#if SDL3_4_0_OR_GREATER

using Sdl3Sharp.Video.Gpu;

namespace Sdl3Sharp.Video.Rendering;

partial class TextureExtensions
{
	extension(Texture<Drivers.Gpu>.PropertyNames)
	{
		/// <summary>
		/// The name of a <see cref="RendererExtensions.TryCreateTexture(Renderer{Drivers.Gpu}, out Texture{Drivers.Gpu}?, Coloring.ColorSpace?, Coloring.PixelFormat?, TextureAccess?, int?, int?, Coloring.Palette?, float?, float?, GpuTexture?, GpuTexture?, GpuTexture?, GpuTexture?, Properties?)">property used when creating a <see cref="Texture{TDriver}">Texture&lt;<see cref="Drivers.Gpu">Gpu</see>&gt;</see></see>
		/// that holds a pointer to the native <c><see href="https://wiki.libsdl.org/SDL3/SDL_GPUTexture">SDL_GPUTexture</see></c> associated with the texture, if you want to wrap an existing texture
		/// </summary>
		public static string CreateGpuTexturePointer => "SDL.texture.create.gpu.texture";

		/// <summary>
		/// The name of a <see cref="RendererExtensions.TryCreateTexture(Renderer{Drivers.Gpu}, out Texture{Drivers.Gpu}?, Coloring.ColorSpace?, Coloring.PixelFormat?, TextureAccess?, int?, int?, Coloring.Palette?, float?, float?, GpuTexture?, GpuTexture?, GpuTexture?, GpuTexture?, Properties?)">property used when creating a <see cref="Texture{TDriver}">Texture&lt;<see cref="Drivers.Gpu">Gpu</see>&gt;</see></see>
		/// that holds a pointer to the native <c><see href="https://wiki.libsdl.org/SDL3/SDL_GPUTexture">SDL_GPUTexture</see></c> associated with the UV plane of the NV12 texture, if you want to wrap an existing texture
		/// </summary>
		public static string CreateGpuTextureUvPointer => "SDL.texture.create.gpu.texture_uv";

		/// <summary>
		/// The name of a <see cref="RendererExtensions.TryCreateTexture(Renderer{Drivers.Gpu}, out Texture{Drivers.Gpu}?, Coloring.ColorSpace?, Coloring.PixelFormat?, TextureAccess?, int?, int?, Coloring.Palette?, float?, float?, GpuTexture?, GpuTexture?, GpuTexture?, GpuTexture?, Properties?)">property used when creating a <see cref="Texture{TDriver}">Texture&lt;<see cref="Drivers.Gpu">Gpu</see>&gt;</see></see>
		/// that holds a pointer to the native <c><see href="https://wiki.libsdl.org/SDL3/SDL_GPUTexture">SDL_GPUTexture</see></c> associated with the U plane of the YUV texture, if you want to wrap an existing texture
		/// </summary>
		public static string CreateGpuTextureUPointer => "SDL.texture.create.gpu.texture_u";

		/// <summary>
		/// The name of a <see cref="RendererExtensions.TryCreateTexture(Renderer{Drivers.Gpu}, out Texture{Drivers.Gpu}?, Coloring.ColorSpace?, Coloring.PixelFormat?, TextureAccess?, int?, int?, Coloring.Palette?, float?, float?, GpuTexture?, GpuTexture?, GpuTexture?, GpuTexture?, Properties?)">property used when creating a <see cref="Texture{TDriver}">Texture&lt;<see cref="Drivers.Gpu">Gpu</see>&gt;</see></see>
		/// that holds a pointer to the native <c><see href="https://wiki.libsdl.org/SDL3/SDL_GPUTexture">SDL_GPUTexture</see></c> associated with the V plane of the YUV texture, if you want to wrap an existing texture
		/// </summary>
		public static string CreateGpuTextureVPointer => "SDL.texture.create.gpu.texture_v";

		/// <summary>
		/// The name of a <em>read-only</em> <see cref="Texture.Properties">property</see> that holds
		/// a pointer to the native <c><see href="https://wiki.libsdl.org/SDL3/SDL_GPUTexture">SDL_GPUTexture</see></c> associated with the texture
		/// </summary>
		public static string GpuTexturePointer => "SDL.texture.gpu.texture";

		/// <summary>
		/// The name of a <em>read-only</em> <see cref="Texture.Properties">property</see> that holds
		/// a pointer to the native <c><see href="https://wiki.libsdl.org/SDL3/SDL_GPUTexture">SDL_GPUTexture</see></c> associated with the UV plane of the NV12 texture
		/// </summary>
		public static string GpuTextureUvPointer => "SDL.texture.gpu.texture_uv";

		/// <summary>
		/// The name of a <em>read-only</em> <see cref="Texture.Properties">property</see> that holds
		/// a pointer to the native <c><see href="https://wiki.libsdl.org/SDL3/SDL_GPUTexture">SDL_GPUTexture</see></c> associated with the U plane of the YUV texture
		/// </summary>
		public static string GpuTextureUPointer => "SDL.texture.gpu.texture_u";
		
		/// <summary>
		/// The name of a <em>read-only</em> <see cref="Texture.Properties">property</see> that holds
		/// a pointer to the native <c><see href="https://wiki.libsdl.org/SDL3/SDL_GPUTexture">SDL_GPUTexture</see></c> associated with the V plane of the YUV texture
		/// </summary>
		public static string GpuTextureVPointer => "SDL.texture.gpu.texture_v";
	}

	extension(Texture<Drivers.Gpu> texture)
	{
		/// <summary>
		/// Gets the <see cref="GpuTexture"/> associated with the texture
		/// </summary>
		/// <value>
		/// The <see cref="GpuTexture"/> associated with the texture
		/// </value>
		public GpuTexture? GpuTexture
		{
			get
			{
				unsafe
				{
					if (texture?.Properties?.TryGetPointerValue(Texture<Drivers.Gpu>.PropertyNames.GpuTexturePointer, out var gpuTexturePtr) is not true)
					{
						return default;
					}

					GpuTexture.TryGetOrCreate(unchecked((GpuTexture.SDL_GPUTexture*)gpuTexturePtr), out var gpuTexture);
					return gpuTexture;
				}
			}
		}

		/// <summary>
		/// Gets the <see cref="GpuTexture"/> associated with the UV plane of the NV12 texture
		/// </summary>
		/// <value>
		/// The <see cref="GpuTexture"/> associated with the UV plane of the NV12 texture
		/// </value>
		public GpuTexture? GpuTextureUv
		{
			get
			{
				unsafe
				{
					if (texture?.Properties?.TryGetPointerValue(Texture<Drivers.Gpu>.PropertyNames.GpuTextureUvPointer, out var gpuTextureUvPtr) is not true)
					{
						return default;
					}

					GpuTexture.TryGetOrCreate(unchecked((GpuTexture.SDL_GPUTexture*)gpuTextureUvPtr), out var gpuTextureUv);
					return gpuTextureUv;
				}
			}
		}

		/// <summary>
		/// Gets the <see cref="GpuTexture"/> associated with the U plane of the YUV texture
		/// </summary>
		/// <value>
		/// The <see cref="GpuTexture"/> associated with the U plane of the YUV texture
		/// </value>
		public GpuTexture? GpuTextureU
		{
			get
			{
				unsafe
				{
					if (texture?.Properties?.TryGetPointerValue(Texture<Drivers.Gpu>.PropertyNames.GpuTextureUPointer, out var gpuTextureUPtr) is not true)
					{
						return default;
					}

					GpuTexture.TryGetOrCreate(unchecked((GpuTexture.SDL_GPUTexture*)gpuTextureUPtr), out var gpuTextureU);
					return gpuTextureU;
				}
			}
		}

		/// <summary>
		/// Gets the <see cref="GpuTexture"/> associated with the V plane of the YUV texture
		/// </summary>
		/// <value>
		/// The <see cref="GpuTexture"/> associated with the V plane of the YUV texture
		/// </value>
		public GpuTexture? GpuTextureV
		{
			get
			{
				unsafe
				{
					if (texture?.Properties?.TryGetPointerValue(Texture<Drivers.Gpu>.PropertyNames.GpuTextureVPointer, out var gpuTextureVPtr) is not true)
					{
						return default;
					}

					GpuTexture.TryGetOrCreate(unchecked((GpuTexture.SDL_GPUTexture*)gpuTextureVPtr), out var gpuTextureV);
					return gpuTextureV;
				}
			}
		}
	}
}

#endif
