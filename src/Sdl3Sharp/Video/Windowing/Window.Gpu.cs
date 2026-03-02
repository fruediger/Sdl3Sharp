#if SDL3_4_0_OR_GREATER

using Sdl3Sharp.Video.Coloring;
using Sdl3Sharp.Video.Gpu;
using Sdl3Sharp.Video.Rendering;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Video.Windowing;

partial class Window
{
	/// <summary>
	/// Tries to create a new GPU renderer for this window
	/// </summary>
	/// <param name="renderer">The resulting renderer, if this method returns <c><see langword="true"/></c>; otherwise, <c><see langword="null"/></c></param>
	/// <param name="gpuDevice">The <see cref="GpuDevice"/> to use with the renderer, or <c><see langword="null"/></c> to let SDL select or create a suitable GPU device automatically</param>
	/// <returns><c><see langword="true"/></c>, if the renderer was created successfully; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// If the <paramref name="gpuDevice"/> is <c><see langword="null"/></c>, SDL will automatically select or create a suitable GPU device for you.
	/// The <see cref="GpuDevice"/> can be later retrieved from the renderer using the <see cref="RendererExtensions.get_GpuDevice(Renderer{Rendering.Drivers.Gpu})"/> property.
	/// </para>
	/// <para>
	/// If this method is called with a non-<see langword="null"/> <see cref="GpuDevice"/>, it should be called on the thread that created the device.
	/// And it should be definitely be called on the thread that created the window.
	/// </para>
	/// </remarks>
	public bool TryCreateRenderer([NotNullWhen(true)] out Renderer<Rendering.Drivers.Gpu>? renderer, GpuDevice? gpuDevice = default)
	{
		unsafe
		{
			var rendererPtr = Renderer.SDL_CreateGPURenderer(gpuDevice is not null ? gpuDevice.Pointer : null, WindowPtr);

			if (rendererPtr is null)
			{
				renderer = null;
				return false;
			}

			renderer = new(rendererPtr, register: true);
			return true;
		}
	}

	/// <summary>
	/// Tries to create a new GPU renderer for this window
	/// </summary>
	/// <inheritdoc cref="TryCreateRenderer{TDriver}(out Renderer{TDriver}?, ColorSpace?, RendererVSync?, Properties?)"/>
	/// <param name="gpuDevice">The <see cref="GpuDevice"/> to use with the renderer, or <c><see langword="null"/></c> (the default) to let SDL select or create a suitable GPU device automatically</param>
	/// <param name="gpuShadersSpirV">An optional value indicating whether the application is able to provide SPIR-V shaders to the renderer</param>
	/// <param name="gpuShadersDxil">An optional value indicating whether the application is able to provide DXIL shaders to the renderer</param>
	/// <param name="gpuShadersMsl">An optional value indicating whether the application is able to provide MSL shaders to the renderer</param>
#pragma warning disable CS1573 // we get these from inheritdoc
	public bool TryCreateRenderer([NotNullWhen(true)] out Renderer<Rendering.Drivers.Gpu>? renderer, ColorSpace? outputColorSpace = default, RendererVSync? presentVSync = default,
		GpuDevice? gpuDevice = default, bool? gpuShadersSpirV = default, bool? gpuShadersDxil = default, bool? gpuShadersMsl = default, Properties? properties = default)
#pragma warning restore CS1573
	{
		unsafe
		{
			Properties propertiesUsed;
			Unsafe.SkipInit(out IntPtr? gpuDeviceBackup);
			Unsafe.SkipInit(out bool? gpuShadersSpirVBackup);
			Unsafe.SkipInit(out bool? gpuShadersDxilBackup);
			Unsafe.SkipInit(out bool? gpuShadersMslBackup);

			if (properties is null)
			{
				propertiesUsed = [];

				if (gpuDevice is not null)
				{
					propertiesUsed.TrySetPointerValue(Renderer<Rendering.Drivers.Gpu>.PropertyNames.CreateGpuDevicePointer, unchecked((IntPtr)gpuDevice.Pointer));
				}

				if (gpuShadersSpirV is bool gpuShadersSpirVValue)
				{
					propertiesUsed.TrySetBooleanValue(Renderer<Rendering.Drivers.Gpu>.PropertyNames.CreateGpuShadersSpirVBoolean, gpuShadersSpirVValue);
				}

				if (gpuShadersDxil is bool gpuShadersDxilValue)
				{
					propertiesUsed.TrySetBooleanValue(Renderer<Rendering.Drivers.Gpu>.PropertyNames.CreateGpuShadersDxilBoolean, gpuShadersDxilValue);
				}

				if (gpuShadersMsl is bool gpuShadersMslValue)
				{
					propertiesUsed.TrySetBooleanValue(Renderer<Rendering.Drivers.Gpu>.PropertyNames.CreateGpuShadersMslBoolean, gpuShadersMslValue);
				}
			}
			else
			{
				propertiesUsed = properties;

				if (gpuDevice is not null)
				{
					gpuDeviceBackup = propertiesUsed.TryGetPointerValue(Renderer<Rendering.Drivers.Gpu>.PropertyNames.CreateGpuDevicePointer, out var exisitingGpuDevicePtr)
						? exisitingGpuDevicePtr
						: null;

					propertiesUsed.TrySetPointerValue(Renderer<Rendering.Drivers.Gpu>.PropertyNames.CreateGpuDevicePointer, unchecked((IntPtr)gpuDevice.Pointer));
				}

				if (gpuShadersSpirV is bool gpuShadersSpirVValue)
				{
					gpuShadersSpirVBackup = propertiesUsed.TryGetBooleanValue(Renderer<Rendering.Drivers.Gpu>.PropertyNames.CreateGpuShadersSpirVBoolean, out var existingGpuShadersSpirV)
						? existingGpuShadersSpirV
						: null;

					propertiesUsed.TrySetBooleanValue(Renderer<Rendering.Drivers.Gpu>.PropertyNames.CreateGpuShadersSpirVBoolean, gpuShadersSpirVValue);
				}

				if (gpuShadersDxil is bool gpuShadersDxilValue)
				{
					gpuShadersDxilBackup = propertiesUsed.TryGetBooleanValue(Renderer<Rendering.Drivers.Gpu>.PropertyNames.CreateGpuShadersDxilBoolean, out var existingGpuShadersDxil)
						? existingGpuShadersDxil
						: null;

					propertiesUsed.TrySetBooleanValue(Renderer<Rendering.Drivers.Gpu>.PropertyNames.CreateGpuShadersDxilBoolean, gpuShadersDxilValue);
				}

				if (gpuShadersMsl is bool gpuShadersMslValue)
				{
					gpuShadersMslBackup = propertiesUsed.TryGetBooleanValue(Renderer<Rendering.Drivers.Gpu>.PropertyNames.CreateGpuShadersMslBoolean, out var existingGpuShadersMsl)
						? existingGpuShadersMsl
						: null;

					propertiesUsed.TrySetBooleanValue(Renderer<Rendering.Drivers.Gpu>.PropertyNames.CreateGpuShadersMslBoolean, gpuShadersMslValue);
				}
			}

			try
			{
				return TryCreateRenderer<Rendering.Drivers.Gpu>(out renderer, outputColorSpace, presentVSync, propertiesUsed);
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

					if (gpuDevice is not null)
					{
						if (gpuDeviceBackup is IntPtr gpuDevicePtr)
						{
							propertiesUsed.TrySetPointerValue(Renderer<Rendering.Drivers.Gpu>.PropertyNames.CreateGpuDevicePointer, gpuDevicePtr);
						}
						else
						{
							propertiesUsed.TryRemove(Renderer<Rendering.Drivers.Gpu>.PropertyNames.CreateGpuDevicePointer);
						}
					}

					if (gpuShadersSpirV.HasValue)
					{
						if (gpuShadersSpirVBackup is bool gpuShadersSpirVValue)
						{
							propertiesUsed.TrySetBooleanValue(Renderer<Rendering.Drivers.Gpu>.PropertyNames.CreateGpuShadersSpirVBoolean, gpuShadersSpirVValue);
						}
						else
						{
							propertiesUsed.TryRemove(Renderer<Rendering.Drivers.Gpu>.PropertyNames.CreateGpuShadersSpirVBoolean);
						}
					}

					if (gpuShadersDxil.HasValue)
					{
						if (gpuShadersDxilBackup is bool gpuShadersDxilValue)
						{
							propertiesUsed.TrySetBooleanValue(Renderer<Rendering.Drivers.Gpu>.PropertyNames.CreateGpuShadersDxilBoolean, gpuShadersDxilValue);
						}
						else
						{
							propertiesUsed.TryRemove(Renderer<Rendering.Drivers.Gpu>.PropertyNames.CreateGpuShadersDxilBoolean);
						}
					}

					if (gpuShadersMsl.HasValue)
					{
						if (gpuShadersMslBackup is bool gpuShadersMslValue)
						{
							propertiesUsed.TrySetBooleanValue(Renderer<Rendering.Drivers.Gpu>.PropertyNames.CreateGpuShadersMslBoolean, gpuShadersMslValue);
						}
						else
						{
							propertiesUsed.TryRemove(Renderer<Rendering.Drivers.Gpu>.PropertyNames.CreateGpuShadersMslBoolean);
						}
					}
				}
			}
		}
	}
}

#endif
