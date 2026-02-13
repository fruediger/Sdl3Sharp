#if SDL3_4_0_OR_GREATER

using Sdl3Sharp.Utilities;
using Sdl3Sharp.Video.Gpu;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Video.Rendering;

/// <summary>
/// Represents the information needed to create a <see cref="GpuRenderState"/>
/// </summary>
/// <remarks>
/// <para>
/// This type can be used to share the same creation parameters across multiple <see cref="GpuRenderState"/> instances, or if you want to prepare the creation parameters in advance and move the heavy lifting away from the constructor call.
/// Alternatively, you can use the <see cref="RendererExtensions.TryCreateGpuRenderState(Renderer{Drivers.Gpu}, GpuShader, out GpuRenderState?, ReadOnlySpan{GpuTextureSamplerBinding}, ReadOnlySpan{GpuTexture}, ReadOnlySpan{GpuBuffer}, Sdl3Sharp.Properties?)"/> method
/// to create a <see cref="GpuRenderState"/> without needing to create a separate <see cref="GpuRenderStateCreateInfo"/> instance.
/// </para>
/// </remarks>
public sealed partial class GpuRenderStateCreateInfo : IDisposable
{
	private NativeMemoryManager? mSamplerBindingsManager = null, mStorageTexturesManager = null, mStorageBuffersManager = null;
	private SDL_GPURenderStateCreateInfo mCreateInfo = default;
	private bool mFragmentShaderValid = false, mSamplerBindingsValid = false, mStorageTexturesValid = false, mStorageBuffersValid = false, mPropertiesValid = false;

	internal ref readonly SDL_GPURenderStateCreateInfo AsNative { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => ref mCreateInfo; }

	/// <summary>
	/// Gets or sets the fragment shader to use when the render state created from this <see cref="GpuRenderStateCreateInfo"/> is active
	/// </summary>
	/// <value>
	/// The fragment shader to use when the render state created from this <see cref="GpuRenderStateCreateInfo"/> is active
	/// </value>
	/// <exception cref="ArgumentNullException">
	/// When setting this property to <see langword="null"/>
	/// </exception>
	public required GpuShader FragmentShader
	{
		get => field;

		set
		{
			if (value is null)
			{
				failValueArgumentNull();
			}

			field = value;
			mFragmentShaderValid = false;

			[DoesNotReturn]
			static void failValueArgumentNull() => throw new ArgumentNullException(nameof(value));
		}
	}

	/// <summary>
	/// Gets or sets the additional fragment samplers to bind when the render state created from this <see cref="GpuRenderStateCreateInfo"/> is active
	/// </summary>
	/// <value>
	/// The additional fragment samplers to bind when the render state created from this <see cref="GpuRenderStateCreateInfo"/> is active
	/// </value>
	public IEnumerable<GpuTextureSamplerBinding>? SamplerBindings
	{
		get => field;

		set
		{
			field = value;
			mSamplerBindingsValid = false;
		}
	}

	/// <summary>
	/// Gets or sets the storage textures to bind when the render state created from this <see cref="GpuRenderStateCreateInfo"/> is active
	/// </summary>
	/// <value>
	/// The storage textures to bind when the render state created from this <see cref="GpuRenderStateCreateInfo"/> is active
	/// </value>
	public IEnumerable<GpuTexture>? StorageTextures
	{
		get => field;

		set
		{
			field = value;
			mStorageTexturesValid = false;
		}
	}

	/// <summary>
	/// Gets or sets the storage buffers to bind when the render state created from this <see cref="GpuRenderStateCreateInfo"/> is active
	/// </summary>
	/// <value>
	/// The storage buffers to bind when the render state created from this <see cref="GpuRenderStateCreateInfo"/> is active
	/// </value>
	public IEnumerable<GpuBuffer>? StorageBuffers
	{
		get => field;

		set
		{
			field = value;
			mStorageBuffersValid = false;
		}
	}

	/// <summary>
	/// Gets or sets optional properties for extensions
	/// </summary>
	/// <value>
	/// The properties used for extensions, or <see langword="null"/> if no extensions are needed
	/// </value>
	public Properties? Properties
	{
		get => field;

		set
		{
			field = value;
			mPropertiesValid = false;
		}
	}

	/// <inheritdoc/>
	~GpuRenderStateCreateInfo() => DisposeImpl();

	/// <inheritdoc/>
	public void Dispose()
	{
		DisposeImpl();
		GC.SuppressFinalize(this);
	}

	private void DisposeImpl()
	{
		mSamplerBindingsManager?.Dispose();
		mSamplerBindingsManager = null;

		mStorageTexturesManager?.Dispose();
		mStorageTexturesManager = null;

		mStorageBuffersManager?.Dispose();
		mStorageBuffersManager = null;

		mCreateInfo = default;

		FragmentShader = null!;
		SamplerBindings = null;
		StorageTextures = null;
		StorageBuffers = null;
		Properties = null;

		mFragmentShaderValid = false;
		mSamplerBindingsValid = false;
		mStorageTexturesValid = false;
		mStorageBuffersValid = false;
		mPropertiesValid = false;
	}

	/// <summary>
	/// Tries to prepare this <see cref="GpuRenderStateCreateInfo"/> and caches its internal representation
	/// </summary>
	/// <returns><c><see langword="true"/></c>, if this <see cref="GpuRenderStateCreateInfo"/> is ready to be used for creating a <see cref="GpuRenderState"/>; otherwise, <c><see langword="false"/></c></returns>
	/// <remarks>
	/// <para>
	/// This method prepares this <see cref="GpuRenderStateCreateInfo"/>'s internal representation, caching it for future use.
	/// That means calling this method multiple times without modifying any of the properties is essentially a no-op after the first call.
	/// </para>
	/// <para>
	/// Calling this method is not strictly necessary, as the <see cref="RendererExtensions.TryCreateGpuRenderState(Renderer{Drivers.Gpu}, GpuRenderStateCreateInfo, out GpuRenderState?)"/> method would prepare the <see cref="GpuRenderStateCreateInfo"/> itself, if necessary.
	/// But it could be useful to call this method in advance to have a more deterministic point where the preparation happens, and to move away the heavy lifting from the constructor call.
	/// </para>
	/// <para>
	/// This method will return <c><see langword="false"/></c> if any of the required properties is not set to a valid value (e.g. <see cref="FragmentShader"/> is <see langword="null"/>), or if it fails to allocate or reallocate the native memory needed for the sampler bindings, storage textures, or storage buffers.
	/// </para>
	/// </remarks>
	public bool TryPrepare()
		=> TryPrepareFragmentShader()
		&& TryPrepareSamplerBindings()
		&& TryPrepareStorageTextures()
		&& TryPrepareStorageBuffers()
		&& TryPrepareProperties();

	private unsafe bool TryPrepareFragmentShader()
	{
		if (mFragmentShaderValid)
		{
			return true;
		}

		if (FragmentShader is null)
		{
			return false;
		}

		mCreateInfo.FragmentShader = FragmentShader.Pointer;

		mFragmentShaderValid = true;
		return true;
	}

	private unsafe bool TryPrepareSamplerBindings()
	{
		const int defaultSamplerBindingsCapacity = 4;

		if (mSamplerBindingsValid)
		{
			return true;
		}

		var samplerBindings = SamplerBindings;

		if (samplerBindings is null)
		{
			mSamplerBindingsManager?.Dispose();
			mSamplerBindingsManager = null;

			mCreateInfo.NumSamplerBindings = 0;
			mCreateInfo.SamplerBindings = null;

			mSamplerBindingsValid = true;
			return true;
		}

		if (mSamplerBindingsManager is null)
		{
			if (!samplerBindings.TryGetNonEnumeratedCount(out var enumerableCount))
			{
				enumerableCount = defaultSamplerBindingsCapacity;
			}

			if (enumerableCount is 0)
			{
				mCreateInfo.NumSamplerBindings = 0;
				mCreateInfo.SamplerBindings = null;

				mSamplerBindingsValid = true;
				return true;
			}

			if (!NativeMemory.TryMalloc(unchecked((nuint)enumerableCount * (nuint)Unsafe.SizeOf<GpuTextureSamplerBinding.SDL_GPUTextureSamplerBinding>()), out mSamplerBindingsManager))
			{
				return false;
			}
		}
		else
		{
			// we could avoid having to realloc during the enumeration phase if we can get the count beforehand

			if (samplerBindings.TryGetNonEnumeratedCount(out var enumerableCount))
			{
				if (enumerableCount is 0)
				{
					mSamplerBindingsManager.Dispose();
					mSamplerBindingsManager = null;

					mCreateInfo.NumSamplerBindings = 0;
					mCreateInfo.SamplerBindings = null;

					mSamplerBindingsValid = true;
					return true;
				}

				var required = unchecked((nuint)enumerableCount * (nuint)Unsafe.SizeOf<GpuTextureSamplerBinding.SDL_GPUTextureSamplerBinding>());

				if (required > mSamplerBindingsManager.Length)
				{
					if (!NativeMemory.TryRealloc(ref mSamplerBindingsManager, required))
					{
						return false;
					}
				}
			}
		}

		var samplerBindingsMemory = (NativeMemory<GpuTextureSamplerBinding.SDL_GPUTextureSamplerBinding>)mSamplerBindingsManager.Memory;
		var samplerBindingPtr = samplerBindingsMemory.RawPointer;

		nuint count = 0;
		foreach (var samplerBinding in samplerBindings)
		{
			var nextCount = count + 1;

			if (nextCount > samplerBindingsMemory.Length)
			{
				if (!NativeMemory.TryRealloc(ref mSamplerBindingsManager, unchecked((mSamplerBindingsManager.Length + mSamplerBindingsManager.Length / 2) * (nuint)Unsafe.SizeOf<GpuTextureSamplerBinding.SDL_GPUTextureSamplerBinding>())))
				{
					return false;
				}

				samplerBindingsMemory = (NativeMemory<GpuTextureSamplerBinding.SDL_GPUTextureSamplerBinding>)mSamplerBindingsManager.Memory;
				samplerBindingPtr = samplerBindingsMemory.RawPointer;
			}

			samplerBindingPtr[count] = samplerBinding.ToNative();

			count = nextCount;
		}

		if (count is 0)
		{
			mSamplerBindingsManager.Dispose();
			mSamplerBindingsManager = null;

			mCreateInfo.NumSamplerBindings = 0;
			mCreateInfo.SamplerBindings = null;

			mSamplerBindingsValid = true;
			return true;
		}

		if (count < samplerBindingsMemory.Length)
		{
			NativeMemory.TryRealloc(ref mSamplerBindingsManager, unchecked(count * (nuint)Unsafe.SizeOf<GpuTextureSamplerBinding.SDL_GPUTextureSamplerBinding>()));
		}

		mCreateInfo.NumSamplerBindings = unchecked((int)count);
		mCreateInfo.SamplerBindings = (GpuTextureSamplerBinding.SDL_GPUTextureSamplerBinding*)mSamplerBindingsManager.RawPointer;

		mSamplerBindingsValid = true;
		return true;
	}
	private unsafe bool TryPrepareStorageTextures()
	{
		const int defaultStorageTexturesCapacity = 4;

		if (mStorageTexturesValid)
		{
			return true;
		}

		var storageTextures = StorageTextures;

		if (storageTextures is null)
		{
			mStorageTexturesManager?.Dispose();
			mStorageTexturesManager = null;

			mCreateInfo.NumStorageTextures = 0;
			mCreateInfo.StorageTextures = null;

			mStorageTexturesValid = true;
			return true;
		}

		if (mStorageTexturesManager is null)
		{
			if (!storageTextures.TryGetNonEnumeratedCount(out var enumerableCount))
			{
				enumerableCount = defaultStorageTexturesCapacity;
			}

			if (enumerableCount is 0)
			{
				mCreateInfo.NumStorageTextures = 0;
				mCreateInfo.StorageTextures = null;

				mStorageTexturesValid = true;
				return true;
			}

			if (!NativeMemory.TryMalloc(unchecked((nuint)enumerableCount * (nuint)sizeof(GpuTexture.SDL_GPUTexture*)), out mStorageTexturesManager))
			{
				return false;
			}
		}
		else
		{
			// we could avoid having to realloc during the enumeration phase if we can get the count beforehand

			if (storageTextures.TryGetNonEnumeratedCount(out var enumerableCount))
			{
				if (enumerableCount is 0)
				{
					mStorageTexturesManager.Dispose();
					mStorageTexturesManager = null;

					mCreateInfo.NumStorageTextures = 0;
					mCreateInfo.StorageTextures = null;

					mStorageTexturesValid = true;
					return true;
				}

				var required = unchecked((nuint)enumerableCount * (nuint)sizeof(GpuTexture.SDL_GPUTexture*));

				if (required > mStorageTexturesManager.Length)
				{
					if (!NativeMemory.TryRealloc(ref mStorageTexturesManager, required))
					{
						return false;
					}
				}
			}
		}

		var storageTexturesMemory = (NativeMemory<IntPtr>)mStorageTexturesManager.Memory;
		var storageTexturePtr = (GpuTexture.SDL_GPUTexture**)storageTexturesMemory.RawPointer;

		nuint count = 0;
		foreach (var storageTexture in storageTextures)
		{
			var nextCount = count + 1;

			if (nextCount > storageTexturesMemory.Length)
			{
				if (!NativeMemory.TryRealloc(ref mStorageTexturesManager, unchecked((mStorageTexturesManager.Length + mStorageTexturesManager.Length / 2) * (nuint)sizeof(GpuTexture.SDL_GPUTexture*))))
				{
					return false;
				}

				storageTexturesMemory = (NativeMemory<IntPtr>)mStorageTexturesManager.Memory;
				storageTexturePtr = (GpuTexture.SDL_GPUTexture**)storageTexturesMemory.RawPointer;
			}

			storageTexturePtr[count] = storageTexture is not null ? storageTexture.Pointer : null;

			count = nextCount;
		}

		if (count is 0)
		{
			mStorageTexturesManager.Dispose();
			mStorageTexturesManager = null;

			mCreateInfo.NumStorageTextures = 0;
			mCreateInfo.StorageTextures = null;

			mStorageTexturesValid = true;
			return true;
		}

		if (count < storageTexturesMemory.Length)
		{
			NativeMemory.TryRealloc(ref mStorageTexturesManager, unchecked(count * (nuint)sizeof(GpuTexture.SDL_GPUTexture*)));
		}

		mCreateInfo.NumStorageTextures = unchecked((int)count);
		mCreateInfo.StorageTextures = (GpuTexture.SDL_GPUTexture**)mStorageTexturesManager.RawPointer;

		mStorageTexturesValid = true;
		return true;
	}

	private unsafe bool TryPrepareStorageBuffers()
	{
		const int defaultStorageBuffersCapacity = 4;

		if (mStorageBuffersValid)
		{
			return true;
		}

		var storageBuffers = StorageBuffers;

		if (storageBuffers is null)
		{
			mStorageBuffersManager?.Dispose();
			mStorageBuffersManager = null;

			mCreateInfo.NumStorageBuffers = 0;
			mCreateInfo.StorageBuffers = null;

			mStorageBuffersValid = true;
			return true;
		}

		if (mStorageBuffersManager is null)
		{
			if (!storageBuffers.TryGetNonEnumeratedCount(out var enumerableCount))
			{
				enumerableCount = defaultStorageBuffersCapacity;
			}

			if (enumerableCount is 0)
			{
				mCreateInfo.NumStorageBuffers = 0;
				mCreateInfo.StorageBuffers = null;

				mStorageBuffersValid = true;
				return true;
			}

			if (!NativeMemory.TryMalloc(unchecked((nuint)enumerableCount * (nuint)sizeof(GpuBuffer.SDL_GPUBuffer*)), out mStorageBuffersManager))
			{
				return false;
			}
		}
		else
		{
			// we could avoid having to realloc during the enumeration phase if we can get the count beforehand

			if (storageBuffers.TryGetNonEnumeratedCount(out var enumerableCount))
			{
				if (enumerableCount is 0)
				{
					mStorageBuffersManager.Dispose();
					mStorageBuffersManager = null;

					mCreateInfo.NumStorageBuffers = 0;
					mCreateInfo.StorageBuffers = null;

					mStorageBuffersValid = true;
					return true;
				}

				var required = unchecked((nuint)enumerableCount * (nuint)sizeof(GpuBuffer.SDL_GPUBuffer*));

				if (required > mStorageBuffersManager.Length)
				{
					if (!NativeMemory.TryRealloc(ref mStorageBuffersManager, required))
					{
						return false;
					}
				}
			}
		}

		var storageBuffersMemory = (NativeMemory<IntPtr>)mStorageBuffersManager.Memory;
		var storageBufferPtr = (GpuBuffer.SDL_GPUBuffer**)storageBuffersMemory.RawPointer;

		nuint count = 0;
		foreach (var storageBuffer in storageBuffers)
		{
			var nextCount = count + 1;

			if (nextCount > storageBuffersMemory.Length)
			{
				if (!NativeMemory.TryRealloc(ref mStorageBuffersManager, unchecked((mStorageBuffersManager.Length + mStorageBuffersManager.Length / 2) * (nuint)sizeof(GpuBuffer.SDL_GPUBuffer*))))
				{
					return false;
				}

				storageBuffersMemory = (NativeMemory<IntPtr>)mStorageBuffersManager.Memory;
				storageBufferPtr = (GpuBuffer.SDL_GPUBuffer**)storageBuffersMemory.RawPointer;
			}

			storageBufferPtr[count] = storageBuffer is not null ? storageBuffer.Pointer : null;

			count = nextCount;
		}

		if (count is 0)
		{
			mStorageBuffersManager.Dispose();
			mStorageBuffersManager = null;

			mCreateInfo.NumStorageBuffers = 0;
			mCreateInfo.StorageBuffers = null;

			mStorageBuffersValid = true;
			return true;
		}

		if (count < storageBuffersMemory.Length)
		{
			NativeMemory.TryRealloc(ref mStorageBuffersManager, unchecked(count * (nuint)sizeof(GpuBuffer.SDL_GPUBuffer*)));
		}

		mCreateInfo.NumStorageBuffers = unchecked((int)count);
		mCreateInfo.StorageBuffers = (GpuBuffer.SDL_GPUBuffer**)mStorageBuffersManager.RawPointer;

		mStorageBuffersValid = true;
		return true;
	}

	private bool TryPrepareProperties()
	{
		if (mPropertiesValid)
		{
			return true;
		}

		mCreateInfo.Props = Properties is { Id: var id } ? id : 0;

		mPropertiesValid = true;
		return true;
	}
}

#endif
