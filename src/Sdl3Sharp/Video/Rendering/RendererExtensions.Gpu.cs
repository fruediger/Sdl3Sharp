#if SDL3_4_0_OR_GREATER

using Sdl3Sharp.Video.Coloring;
using Sdl3Sharp.Video.Gpu;
using Sdl3Sharp.Video.Windowing;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Video.Rendering;

partial class RendererExtensions
{
	extension(Renderer<Drivers.Gpu>.PropertyNames)
	{
		/// <summary>
		/// The name of a <see cref="Window.TryCreateRenderer(out Renderer{Drivers.Gpu}?, ColorSpace?, RendererVSync?, GpuDevice?, bool?, bool?, bool?, Properties?)">property used when creating a <see cref="Renderer{TDriver}">Renderer&lt;<see cref="Drivers.Gpu">Gpu</see>&gt;</see></see>
		/// that holds a pointer to the native <see href="https://wiki.libsdl.org/SDL3/SDL_GPUDevice">SDL_GPUDevice</see> to use with the renderer
		/// </summary>
		/// <remarks>
		/// <para>
		/// If the associated property is not specified or is <c><see langword="null"/></c>, SDL will automatically select or create a suitable GPU device for you.
		/// </para>
		/// </remarks>
		public static string CreateGpuDevicePointer => "SDL.renderer.create.gpu.device";

		/// <summary>
		/// The name of a <see cref="Window.TryCreateRenderer(out Renderer{Drivers.Gpu}?, ColorSpace?, RendererVSync?, GpuDevice?, bool?, bool?, bool?, Properties?)">property used when creating a <see cref="Renderer{TDriver}">Renderer&lt;<see cref="Drivers.Gpu">Gpu</see>&gt;</see></see>
		/// that holds a boolean value indicating whether the application is able to provide SPIR-V shaders to the renderer
		/// </summary>
		public static string CreateGpuShadersSpirVBoolean => "SDL.renderer.create.gpu.shaders_spirv";

		/// <summary>
		/// The name of a <see cref="Window.TryCreateRenderer(out Renderer{Drivers.Gpu}?, ColorSpace?, RendererVSync?, GpuDevice?, bool?, bool?, bool?, Properties?)">property used when creating a <see cref="Renderer{TDriver}">Renderer&lt;<see cref="Drivers.Gpu">Gpu</see>&gt;</see></see>
		/// that holds a boolean value indicating whether the application is able to provide DXIL shaders to the renderer
		/// </summary>
		public static string CreateGpuShadersDxilBoolean => "SDL.renderer.create.gpu.shaders_dxil";

		/// <summary>
		/// The name of a <see cref="Window.TryCreateRenderer(out Renderer{Drivers.Gpu}?, ColorSpace?, RendererVSync?, GpuDevice?, bool?, bool?, bool?, Properties?)">property used when creating a <see cref="Renderer{TDriver}">Renderer&lt;<see cref="Drivers.Gpu">Gpu</see>&gt;</see></see>
		/// that holds a boolean value indicating whether the application is able to provide MSL shaders to the renderer
		/// </summary>
		public static string CreateGpuShadersMslBoolean => "SDL.renderer.create.gpu.shaders_msl";

		/// <summary>
		/// The name of a <em>read-only</em> <see cref="IRenderer.Properties">property</see> that holds
		/// a pointer to the native <see href="https://wiki.libsdl.org/SDL3/SDL_GPUDevice">SDL_GPUDevice</see> that the renderer is using
		/// </summary>
		public static string GpuDevicePointer => "SDL.renderer.gpu.device";
	}

	extension(Renderer<Drivers.Gpu>)
	{
		/// <summary>
		/// Tries to create a new GPU renderer
		/// </summary>
		/// <param name="renderer">The resulting renderer, if this method returns <c><see langword="true"/></c>; otherwise, <c><see langword="null"/></c></param>
		/// <param name="gpuDevice">The <see cref="GpuDevice"/> to use with the renderer, or <c><see langword="null"/></c> to let SDL select or create a suitable GPU device automatically</param>
		/// <param name="window">The <see cref="Window"/> to associate the renderer with, or <c><see langword="null"/></c> to create an off-screen renderer</param>
		/// <returns><c><see langword="true"/></c>, if the renderer was created successfully; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
		/// <remarks>
		/// <para>
		/// If the <paramref name="gpuDevice"/> is <c><see langword="null"/></c>, SDL will automatically select or create a suitable GPU device for you.
		/// The <see cref="GpuDevice"/> can be later retrieved from the renderer using the <see cref="get_GpuDevice(Renderer{Drivers.Gpu})"/> property.
		/// </para>
		/// <para>
		/// If the <paramref name="window"/> is <c><see langword="null"/></c>, the created renderer will be an off-screen renderer, not associated with any window.
		/// In that case, you should set a valid <see cref="Renderer{TDriver}.Target"/> for the renderer and then <em>still call <see cref="Renderer{TDriver}.TryRenderPresent"/> normally to complete drawing a frame</em>
		/// (note that that's different from using a target texture with a window-associated renderer, where you shouldn't call <see cref="Renderer{TDriver}.TryRenderPresent"/> when rendering to the target texture).
		/// </para>
		/// <para>
		/// If this method is called with a non-<see langword="null"/> <see cref="GpuDevice"/>, it should be called on the thread that created the device.
		/// If it is called with a non-<see langword="null"/> <see cref="Window"/>, it should be called on the thread that created the window.
		/// </para>
		/// </remarks>
		public static bool TryCreate([NotNullWhen(true)] out Renderer<Drivers.Gpu>? renderer, GpuDevice? gpuDevice = null, Window? window = null)
		{
			unsafe
			{
				var rendererPtr = IRenderer.SDL_CreateGPURenderer(gpuDevice is not null ? gpuDevice.Pointer : null, window is not null ? window.WindowPtr : null);

				if (rendererPtr is null)
				{
					renderer = null;
					return false;
				}

				renderer = new(rendererPtr, register: true);
				return true;
			}
		}
	}

	extension(Renderer<Drivers.Gpu> renderer)
	{
		/// <summary>
		/// Gets the <see cref="GpuDevice"/> that the renderer is using
		/// </summary>
		/// <value>
		/// The <see cref="GpuDevice"/> that the renderer is using, or <c><see langword="null"/></c> if there was an error retrieving it (check <see cref="Error.TryGet(out string?)"/> for more information)
		/// </value>
		public GpuDevice? GpuDevice
		{
			get
			{
				unsafe
				{
					GpuDevice.TryGetOrCreate(SDL_GetGPURendererDevice(renderer is not null ? renderer.Pointer : null), out var gpuDevice);
					return gpuDevice;
				}
			}
		}

		/// <summary>
		/// Tries to create a new <see cref="GpuRenderState"/>
		/// </summary>
		/// <param name="createInfo">Information describing the render state to create</param>
		/// <param name="gpuRenderState">The newly created <see cref="GpuRenderState"/>, if this method returns <c><see langword="true"/></c>; otherwise, <c><see langword="null"/></c></param> 
		/// <returns><c><see langword="true"/></c>, if the render state was created successfully; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
		/// <remarks>
		/// <para>
		/// Use this method if you want to use a shared <see cref="GpuRenderStateCreateInfo"/>.
		/// Notice that the given <paramref name="createInfo"/> is automatically <see cref="GpuRenderStateCreateInfo.TryPrepare">prepared</see> if it requires preparation, this might impact performance.
		/// You can call <see cref="GpuRenderStateCreateInfo.TryPrepare"/> manually any time before calling this method to avoid that overhead.
		/// Alternatively, you can use the <see cref="TryCreateGpuRenderState(Renderer{Drivers.Gpu}, GpuShader, out GpuRenderState?, ReadOnlySpan{GpuTextureSamplerBinding}, ReadOnlySpan{GpuTexture}, ReadOnlySpan{GpuBuffer}, Properties?)"/> method to create a <see cref="GpuRenderState"/> without needing to create a separate <see cref="GpuRenderStateCreateInfo"/> instance.
		/// </para>
		/// <para>
		/// In addition to SDL errors, this method returns <c><see langword="false"/></c> if <paramref name="createInfo"/> is <c><see langword="null"/></c> or can't be prepared successfully.
		/// </para>
		/// <para>
		/// This method should be called on the thread that created the <see cref="Renderer{TDriver}"/>.
		/// </para>
		/// </remarks>
		public bool TryCreateGpuRenderState(GpuRenderStateCreateInfo createInfo, [NotNullWhen(true)] out GpuRenderState? gpuRenderState)
		{
			unsafe
			{
				if (renderer is null
					|| createInfo?.TryPrepare() is not true)
				{
					gpuRenderState = null;
					return false;
				}

				GpuRenderState.SDL_GPURenderState* renderStatePtr;
				fixed (GpuRenderStateCreateInfo.SDL_GPURenderStateCreateInfo* createInfoPtr = &createInfo.AsNative)
				{
					renderStatePtr = GpuRenderState.SDL_CreateGPURenderState(renderer.Pointer, createInfoPtr);
				}

				if (renderStatePtr is null)
				{
					gpuRenderState = null;
					return false;
				}

				gpuRenderState = new(renderStatePtr);
				return true;
			}
		}

		/// <summary>
		/// Tries to create a new <see cref="GpuRenderState"/>
		/// </summary>
		/// <param name="fragmentShader">The fragment shader to use when this render state is active</param>
		/// <param name="gpuRenderState">The newly created <see cref="GpuRenderState"/>, if this method returns <c><see langword="true"/></c>; otherwise, <c><see langword="null"/></c></param> 
		/// <param name="samplerBindings">The additional fragment samplers to bind when this render state is active</param>
		/// <param name="storageTextures">The storage textures to bind when this render state is active</param>
		/// <param name="storageBuffers">The storage buffers to bind when this render state is active</param>
		/// <param name="properties">Optional properties for extensions, or <see langword="null"/> if no extensions are needed</param>
		/// <returns><c><see langword="true"/></c>, if the render state was created successfully; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
		/// <remarks>
		/// <para>
		/// Notice that even though this method does not require you to create a separate <see cref="GpuRenderStateCreateInfo"/> instance, it may still impact performance due to the preparation of the given arguments.
		/// You can use the <see cref="TryCreateGpuRenderState(Renderer{Drivers.Gpu}, GpuRenderStateCreateInfo, out GpuRenderState?)"/> method with a pre-prepared <see cref="GpuRenderStateCreateInfo"/> instance to avoid that overhead.
		/// </para>
		/// <para>
		/// In addition to SDL errors, this method returns <c><see langword="false"/></c> if <paramref name="fragmentShader"/> is <c><see langword="null"/></c>.
		/// </para>
		/// <para>
		/// This method should be called on the thread that created the <see cref="Renderer{TDriver}"/>.
		/// </para>
		/// </remarks>
		public bool TryCreateGpuRenderState(GpuShader fragmentShader, [NotNullWhen(true)] out GpuRenderState? gpuRenderState, ReadOnlySpan<GpuTextureSamplerBinding> samplerBindings = default, ReadOnlySpan<GpuTexture> storageTextures = default, ReadOnlySpan<GpuBuffer> storageBuffers = default, Properties? properties = default)
		{
			unsafe
			{
				if (renderer is null
					|| fragmentShader is null)
				{
					gpuRenderState = null;
					return false;
				}

				Unsafe.SkipInit(out GpuRenderStateCreateInfo.SDL_GPURenderStateCreateInfo createInfo);

				try
				{
					createInfo.FragmentShader = fragmentShader.Pointer;

					if (samplerBindings.IsEmpty)
					{
						createInfo.NumSamplerBindings = 0;
						createInfo.SamplerBindings = null;
					}
					else
					{
						createInfo.NumSamplerBindings = samplerBindings.Length;
						createInfo.SamplerBindings = unchecked((GpuTextureSamplerBinding.SDL_GPUTextureSamplerBinding*)Utilities.NativeMemory.Malloc(unchecked((nuint)createInfo.NumSamplerBindings * (nuint)Unsafe.SizeOf<GpuTextureSamplerBinding.SDL_GPUTextureSamplerBinding>())));

						var samplerBindingsPtr = createInfo.SamplerBindings;
						foreach (var samplerBinding in samplerBindings)
						{
							*samplerBindingsPtr++ = samplerBinding.ToNative();
						}
					}

					if (storageTextures.IsEmpty)
					{
						createInfo.NumStorageTextures = 0;
						createInfo.StorageTextures = null;
					}
					else
					{
						createInfo.NumStorageTextures = storageTextures.Length;
						createInfo.StorageTextures = unchecked((GpuTexture.SDL_GPUTexture**)Utilities.NativeMemory.Malloc(unchecked((nuint)createInfo.NumStorageTextures * (nuint)sizeof(GpuTexture.SDL_GPUTexture*))));

						var storageTexturesPtr = createInfo.StorageTextures;
						foreach (var storageTexture in storageTextures)
						{
							*storageTexturesPtr++ = storageTexture.Pointer;
						}
					}

					if (storageBuffers.IsEmpty)
					{
						createInfo.NumStorageBuffers = 0;
						createInfo.StorageBuffers = null;
					}
					else
					{
						createInfo.NumStorageBuffers = storageBuffers.Length;
						createInfo.StorageBuffers = unchecked((GpuBuffer.SDL_GPUBuffer**)Utilities.NativeMemory.Malloc(unchecked((nuint)createInfo.NumStorageBuffers * (nuint)sizeof(GpuBuffer.SDL_GPUBuffer*))));

						var storageBuffersPtr = createInfo.StorageBuffers;
						foreach (var storageBuffer in storageBuffers)
						{
							*storageBuffersPtr++ = storageBuffer.Pointer;
						}
					}

					createInfo.Props = properties is { Id: var id } ? id : 0;

					var renderStatePtr = GpuRenderState.SDL_CreateGPURenderState(renderer.Pointer, &createInfo);

					if (renderStatePtr is null)
					{
						gpuRenderState = null;
						return false;
					}

					gpuRenderState = new(renderStatePtr);
					return true;
				}
				finally
				{
					Utilities.NativeMemory.Free(createInfo.StorageBuffers);
					Utilities.NativeMemory.Free(createInfo.StorageTextures);
					Utilities.NativeMemory.Free(createInfo.SamplerBindings);
				}
			}
		}

		/// <inheritdoc cref="Renderer{TDriver}.TryCreateTexture(out Texture{TDriver}?, ColorSpace?, PixelFormat?, TextureAccess?, int?, int?, Palette?, float?, float?, Properties?)"/>
		/// <param name="gpuTexture">A <see cref="GpuTexture"/> to associate with the newly created texture, if you want to wrap an existing texture</param>
		/// <param name="gpuTextureUv">A <see cref="GpuTexture"/> to associate with the UV plane of the newly created NV12 texture, if you want to wrap an existing texture</param>
		/// <param name="gpuTextureU">A <see cref="GpuTexture"/> to associate with the U plane of the newly created YUV texture, if you want to wrap an existing texture</param>
		/// <param name="gpuTextureV">A <see cref="GpuTexture"/> to associate with the V plane of the newly created YUV texture, if you want to wrap an existing texture</param>
#pragma warning disable CS1573 // we get these from inheritdoc
		public bool TryCreateTexture([NotNullWhen(true)] out Texture<Drivers.Gpu>? texture, ColorSpace? colorSpace = default, PixelFormat? format = default, TextureAccess? access = default, int? width = default, int? height = default,
#if SDL3_4_0_OR_GREATER
			Palette? palette = default,
#endif
			float? sdrWhitePoint = default, float? hdrHeadroom = default,
			GpuTexture? gpuTexture = default, GpuTexture? gpuTextureUv = default, GpuTexture? gpuTextureU = default, GpuTexture? gpuTextureV = default, Properties? properties = default)
#pragma warning restore CS1573
		{
			unsafe
			{
				if (renderer is null)
				{
					texture = null;
					return false;
				}

				Properties propertiesUsed;
				Unsafe.SkipInit(out IntPtr? gpuTextureBackup);
				Unsafe.SkipInit(out IntPtr? gpuTextureUvBackup);
				Unsafe.SkipInit(out IntPtr? gpuTextureUBackup);
				Unsafe.SkipInit(out IntPtr? gpuTextureVBackup);

				if (properties is null)
				{
					propertiesUsed = [];

					if (gpuTexture is { Pointer: var gpuTexturePtr })
					{
						propertiesUsed.TrySetPointerValue(Texture<Drivers.Gpu>.PropertyNames.CreateGpuTexturePointer, unchecked((IntPtr)gpuTexturePtr));
					}

					if (gpuTextureUv is { Pointer: var gpuTextureUvPtr })
					{
						propertiesUsed.TrySetPointerValue(Texture<Drivers.Gpu>.PropertyNames.CreateGpuTextureUvPointer, unchecked((IntPtr)gpuTextureUvPtr));
					}

					if (gpuTextureU is { Pointer: var gpuTextureUPtr })
					{
						propertiesUsed.TrySetPointerValue(Texture<Drivers.Gpu>.PropertyNames.CreateGpuTextureUPointer, unchecked((IntPtr)gpuTextureUPtr));
					}

					if (gpuTextureV is { Pointer: var gpuTextureVPtr })
					{
						propertiesUsed.TrySetPointerValue(Texture<Drivers.Gpu>.PropertyNames.CreateGpuTextureVPointer, unchecked((IntPtr)gpuTextureVPtr));
					}
				}
				else
				{
					propertiesUsed = properties;

					if (gpuTexture is { Pointer: var gpuTexturePtr })
					{
						gpuTextureBackup = propertiesUsed.TryGetPointerValue(Texture<Drivers.Gpu>.PropertyNames.CreateGpuTexturePointer, out var existingGpuTexturePtr)
							? existingGpuTexturePtr
							: null;

						propertiesUsed.TrySetPointerValue(Texture<Drivers.Gpu>.PropertyNames.CreateGpuTexturePointer, unchecked((IntPtr)gpuTexturePtr));
					}

					if (gpuTextureUv is { Pointer: var gpuTextureUvPtr })
					{
						gpuTextureUvBackup = propertiesUsed.TryGetPointerValue(Texture<Drivers.Gpu>.PropertyNames.CreateGpuTextureUvPointer, out var existingGpuTextureUvPtr)
							? existingGpuTextureUvPtr
							: null;

						propertiesUsed.TrySetPointerValue(Texture<Drivers.Gpu>.PropertyNames.CreateGpuTextureUvPointer, unchecked((IntPtr)gpuTextureUvPtr));
					}

					if (gpuTextureU is { Pointer: var gpuTextureUPtr })
					{
						gpuTextureUBackup = propertiesUsed.TryGetPointerValue(Texture<Drivers.Gpu>.PropertyNames.CreateGpuTextureUPointer, out var existingGpuTextureUPtr)
							? existingGpuTextureUPtr
							: null;

						propertiesUsed.TrySetPointerValue(Texture<Drivers.Gpu>.PropertyNames.CreateGpuTextureUPointer, unchecked((IntPtr)gpuTextureUPtr));
					}

					if (gpuTextureV is { Pointer: var gpuTextureVPtr })
					{
						gpuTextureVBackup = propertiesUsed.TryGetPointerValue(Texture<Drivers.Gpu>.PropertyNames.CreateGpuTextureVPointer, out var existingGpuTextureVPtr)
							? existingGpuTextureVPtr
							: null;

						propertiesUsed.TrySetPointerValue(Texture<Drivers.Gpu>.PropertyNames.CreateGpuTextureVPointer, unchecked((IntPtr)gpuTextureVPtr));
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

						if (gpuTexture is not null)
						{
							if (gpuTextureBackup is IntPtr gpuTexturePtr)
							{
								propertiesUsed.TrySetPointerValue(Texture<Drivers.Gpu>.PropertyNames.CreateGpuTexturePointer, gpuTexturePtr);
							}
							else
							{
								propertiesUsed.TryRemove(Texture<Drivers.Gpu>.PropertyNames.CreateGpuTexturePointer);
							}
						}

						if (gpuTextureUv is not null)
						{
							if (gpuTextureUvBackup is IntPtr gpuTextureUvPtr)
							{
								propertiesUsed.TrySetPointerValue(Texture<Drivers.Gpu>.PropertyNames.CreateGpuTextureUvPointer, gpuTextureUvPtr);
							}
							else
							{
								propertiesUsed.TryRemove(Texture<Drivers.Gpu>.PropertyNames.CreateGpuTextureUvPointer);
							}
						}

						if (gpuTextureU is not null)
						{
							if (gpuTextureUBackup is IntPtr gpuTextureUPtr)
							{
								propertiesUsed.TrySetPointerValue(Texture<Drivers.Gpu>.PropertyNames.CreateGpuTextureUPointer, gpuTextureUPtr);
							}
							else
							{
								propertiesUsed.TryRemove(Texture<Drivers.Gpu>.PropertyNames.CreateGpuTextureUPointer);
							}
						}

						if (gpuTextureV is not null)
						{
							if (gpuTextureVBackup is IntPtr gpuTextureVPtr)
							{
								propertiesUsed.TrySetPointerValue(Texture<Drivers.Gpu>.PropertyNames.CreateGpuTextureVPointer, gpuTextureVPtr);
							}
							else
							{
								propertiesUsed.TryRemove(Texture<Drivers.Gpu>.PropertyNames.CreateGpuTextureVPointer);
							}
						}
					}
				}
			}
		}

		/// <summary>
		/// Tries to set a custom GPU render state as the current render state of the renderer
		/// </summary>
		/// <param name="gpuRenderState">The <see cref="GpuRenderState"/> to set as the current render state of the renderer, or <c><see langword="null"/></c> to clear and reset the current render state</param>
		/// <returns><c><see langword="true"/></c>, if the render state was set successfully; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
		/// <remarks>
		/// <para>
		/// This method sets custom GPU render state for subsequent draw calls. This allows for using custom shaders with the GPU renderer.
		/// </para>
		/// <para>
		/// If the given <paramref name="gpuRenderState"/> is <c><see langword="null"/></c>, the current render state will be cleared and reset.
		/// </para>
		/// <para>
		/// This method should only be called on the thread that created the renderer.
		/// </para>
		/// </remarks>
		public bool TrySetGpuRenderState(GpuRenderState? gpuRenderState)
		{
			unsafe
			{
				GpuRenderState.SDL_GPURenderState* state;
				if (gpuRenderState is null)
				{
					state = null;
				}
				else if (gpuRenderState.Pointer is var pointer && pointer is not null)
				{
					state = pointer;
				}
				else
				{
					return false;
				}

				return SDL_SetGPURenderState(renderer is not null ? renderer.Pointer : null, state);
			}
		}
	}
}

#endif