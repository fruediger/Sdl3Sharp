using Sdl3Sharp.Internal;
using Sdl3Sharp.Utilities;
using Sdl3Sharp.Video.Blending;
using Sdl3Sharp.Video.Coloring;
using Sdl3Sharp.Video.Drawing;
using Sdl3Sharp.Video.Gpu;
using Sdl3Sharp.Video.Rendering.Drivers;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sdl3Sharp.Video.Rendering;

/// <summary>
/// Represents a texture, created by a <see cref="Renderer{TDriver}"/>
/// </summary>
/// <typeparam name="TDriver">The rendering driver type associated with this texture</typeparam>
/// <remarks>
/// <para>
/// This is an efficient driver-specific representation of pixel data.
/// </para>
/// <para>
/// You can create new textures using the <see cref="Renderer{TDriver}.TryCreateTexture(PixelFormat, TextureAccess, int, int, out Texture{TDriver}?)"/>,
/// <see cref="Renderer{TDriver}.TryCreateTexture(out Texture{TDriver}?, ColorSpace?, PixelFormat?, TextureAccess?, int?, int?, Palette?, float?, float?, Properties?)"/>,
/// or <see cref="Renderer{TDriver}.TryCreateTextureFromSurface(Surface, out Texture{TDriver}?)"/> instance methods on a <see cref="Renderer{TDriver}"/> instance.
/// </para>
/// <para>
/// Additionally, there are some driver-specific methods for creating textures, such as
/// <see cref="RendererExtensions.TryCreateTexture(Renderer{Direct3D11}, out Texture{Direct3D11}?, ColorSpace?, PixelFormat?, TextureAccess?, int?, int?, Palette?, float?, float?, nint?, nint?, nint?, Properties?)">Direct3D 11</see>,
/// <see cref="RendererExtensions.TryCreateTexture(Renderer{Direct3D12}, out Texture{Direct3D12}?, ColorSpace?, PixelFormat?, TextureAccess?, int?, int?, Palette?, float?, float?, nint?, nint?, nint?, Properties?)">Direct3D 12</see>,
/// <see cref="RendererExtensions.TryCreateTexture(Renderer{Drivers.Gpu}, out Texture{Drivers.Gpu}?, ColorSpace?, PixelFormat?, TextureAccess?, int?, int?, Palette?, float?, float?, GpuTexture?, GpuTexture?, GpuTexture?, GpuTexture?, Properties?)">GPU</see>,
/// <see cref="RendererExtensions.TryCreateTexture(Renderer{Metal}, out Texture{Metal}?, ColorSpace?, PixelFormat?, TextureAccess?, int?, int?, Palette?, float?, float?, nint?, Properties?)">Metal</see>,
/// <see cref="RendererExtensions.TryCreateTexture(Renderer{OpenGl}, out Texture{OpenGl}?, ColorSpace?, PixelFormat?, TextureAccess?, int?, int?, Palette?, float?, float?, uint?, uint?, uint?, uint?, Properties?)">OpenGL</see>,
/// <see cref="RendererExtensions.TryCreateTexture(Renderer{OpenGlEs2}, out Texture{OpenGlEs2}?, ColorSpace?, PixelFormat?, TextureAccess?, int?, int?, Palette?, float?, float?, uint?, uint?, uint?, uint?, Properties?)">OpenGL ES 2</see>,
/// and <see cref="RendererExtensions.TryCreateTexture(Renderer{Vulkan}, out Texture{Vulkan}?, ColorSpace?, PixelFormat?, TextureAccess?, int?, int?, Palette?, float?, float?, ulong?, uint?, Properties?)">Vulkan</see>.
/// </para>
/// <para>
/// Please remember to dispose <see cref="Texture{TDriver}"/>s <em>before</em> disposing the <see cref="Renderer{TDriver}"/> that created them!
/// Using an <see cref="Texture{TDriver}"/> after its associated <see cref="Renderer{TDriver}"/> has been disposed can lead to undefined behavior, including corruption and crashes.
/// </para>
/// <para>
/// <see cref="Texture{TDriver}"/>s are concrete texture types, associated with a specific rendering driver.
/// They are used in driver-specific rendering operations with the <see cref="Renderer{TDriver}"/> that created them.
/// </para>
/// <para>
/// If you want to use them in a more general way, you can use them as <see cref="ITexture"/> instances, which serve as abstractions to use them in common rendering operations with the <see cref="IRenderer"/> instance that created them.
/// </para>
/// </remarks>
public sealed partial class Texture<TDriver> : ITexture
	where TDriver : notnull, IDriver // we don't need to worry about putting type argument independent code in the Renderer<TDriver> class,
									 // because TDriver surely is always going to be a reference type
									 // (that's because all of our predefined drivers types, implementing IDriver, are reference types and it's impossible for user code to implement the IDriver interface),
									 // and the JIT will share code for all reference type instantiations
{
	private unsafe ITexture.SDL_Texture* mTexture;

	internal unsafe Texture(ITexture.SDL_Texture* texture, bool register)
	{
		mTexture = texture;

		if (register)
		{
			ITexture.Register(this);
		}
	}

	/// <inheritdoc/>
	~Texture() => DisposeImpl(forget: true);

	/// <inheritdoc/>
	public TextureAccess Access => Properties?.TryGetNumberValue(ITexture.PropertyNames.AccessNumber, out var access) is true
		? unchecked((TextureAccess)access)
		: default;

	/// <inheritdoc/>
	public byte AlphaMod
	{
		get
		{
			unsafe
			{
				Unsafe.SkipInit(out byte alpha);

				ITexture.SDL_GetTextureAlphaMod(mTexture, &alpha);

				return alpha;
			}
		}

		set
		{
			unsafe
			{
				ITexture.SDL_SetTextureAlphaMod(mTexture, value);
			}
		}
	}

	/// <inheritdoc/>
	public float AlphaModFloat
	{
		get
		{
			unsafe
			{
				Unsafe.SkipInit(out float alpha);

				ITexture.SDL_GetTextureAlphaModFloat(mTexture, &alpha);

				return alpha;
			}
		}

		set
		{
			unsafe
			{
				ITexture.SDL_SetTextureAlphaModFloat(mTexture, value);
			}
		}
	}

	/// <inheritdoc/>
	public BlendMode BlendMode
	{
		get
		{
			unsafe
			{
				Unsafe.SkipInit(out BlendMode blendMode);

				ITexture.SDL_GetTextureBlendMode(mTexture, &blendMode);

				return blendMode;
			}

		}

		set
		{
			unsafe
			{
				ErrorHelper.ThrowIfFailed(ITexture.SDL_SetTextureBlendMode(mTexture, value), filterError: ITexture.GetTextureInvalidTextureErrorMessage());
				// throws if value is BlendMode.Invalid or value is an unsupported blend mode for the renderer
				// Although the offical SDL docs say that "If the blend mode is not supported, the closest supported mode is chosen and this function returns false.",
				// that doesn't appear to be the case looking at the SDL source code.
				// It just fails early if the blend mode is not supported and doesn't try to choose a "closest supported mode".
			}
		}
	}

	/// <inheritdoc/>
	public (byte R, byte G, byte B) ColorMod
	{
		get
		{
			unsafe
			{
				Unsafe.SkipInit(out (byte R, byte G, byte B) color);

				ITexture.SDL_GetTextureColorMod(mTexture, &color.R, &color.G, &color.B);

				return color;
			}
		}

		set
		{
			unsafe
			{
				ITexture.SDL_SetTextureColorMod(mTexture, value.R, value.G, value.B);
			}
		}
	}

	/// <inheritdoc/>
	public (float R, float G, float B) ColorModFloat
	{
		get
		{
			unsafe
			{
				Unsafe.SkipInit(out (float R, float G, float B) color);

				ITexture.SDL_GetTextureColorModFloat(mTexture, &color.R, &color.G, &color.B);

				return color;
			}
		}

		set
		{
			unsafe
			{
				ITexture.SDL_SetTextureColorModFloat(mTexture, value.R, value.G, value.B);
			}
		}
	}

	/// <inheritdoc/>
	public ColorSpace ColorSpace => Properties?.TryGetNumberValue(ITexture.PropertyNames.ColorSpaceNumber, out var colorSpace) is true
		? unchecked((ColorSpace)colorSpace)
		: default;

	/// <inheritdoc/>
	public PixelFormat Format
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		get
		{
			unsafe
			{
				return mTexture is var texture
					&& texture is not null
						? texture->Format
						: default;
			}
		}
	}

	/// <inheritdoc/>
	public float HdrHeadroom => Properties?.TryGetFloatValue(ITexture.PropertyNames.HdrHeadroomFloat, out var hdrHeadroom) is true
		? hdrHeadroom
		: default;

	/// <inheritdoc/>
	public int Height
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		get
		{
			unsafe
			{
				return mTexture is var texture
					&& texture is not null
						? texture->H
						: default;
			}
		}
	}

#if SDL3_4_0_OR_GREATER

	/// <inheritdoc/>
	public Palette? Palette
	{
		get
		{
			unsafe
			{
				if (!Palette.TryGetOrCreate(ITexture.SDL_GetTexturePalette(mTexture), out var palette))
				{
					return null;
				}

				return palette;
			}
		}

		set
		{
			unsafe
			{
				// We don't need to worry about SDL_SetTexturePalette potentially destroying the old palette,
				// as we should handle that correctly and consistently via the way we handle ref counting in the managed Palette wrapper.
				// If SDL_SetTexturePalette does destroy the old palette, there shouldn't be a registered managed wrapper around it anymore,
				// and if there would be a registered managed wrapper, SDL_SetTexturePalette shouldn't destroy the native instance yet (as it's ref counter shouldn't go to zero).

				ErrorHelper.ThrowIfFailed(ITexture.SDL_SetTexturePalette(mTexture, value is not null ? value.Pointer : null), filterError: ITexture.GetTextureInvalidTextureErrorMessage());
			}
		}
	}

#endif

	internal unsafe ITexture.SDL_Texture* Pointer { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mTexture; }

	unsafe ITexture.SDL_Texture* ITexture.Pointer { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => Pointer; }

	/// <inheritdoc/>
	public Properties? Properties
	{
		get
		{
			unsafe
			{
				return ITexture.SDL_GetTextureProperties(mTexture) switch
				{
					0 => null,
					var id => Properties.GetOrCreate(sdl: null, id)
				};
			}
		}
	}

	/// <inheritdoc cref="ITexture.Renderer"/>
	public Renderer<TDriver>? Renderer
	{
		get
		{
			unsafe
			{
				Renderer<TDriver>.TryGetOrCreate(ITexture.SDL_GetRendererFromTexture(mTexture), out var renderer);
				return renderer;
			}
		}
	}

	/// <inheritdoc/>
	IRenderer? ITexture.Renderer => Renderer;

	/// <inheritdoc/>
	public ScaleMode ScaleMode
	{
		get
		{
			unsafe
			{
				Unsafe.SkipInit(out ScaleMode scaleMode);

				ITexture.SDL_GetTextureScaleMode(mTexture, &scaleMode);

				return scaleMode;
			}
		}

		set
		{
			unsafe
			{
				ErrorHelper.ThrowIfFailed(ITexture.SDL_SetTextureScaleMode(mTexture, value), filterError: ITexture.GetTextureInvalidTextureErrorMessage());
				// if value is ScaleMode.Invalid or none of the defined scale modes
				// Although the offical SDL docs say that "If the scale mode is not supported, the closest supported mode is chosen.",
				// that doesn't appear to be the case looking at the SDL source code.
				// It just fails early if the scale mode is not supported and doesn't try to choose a "closest supported mode".
			}
		}
	}

	/// <inheritdoc/>
	public float SdrWhitePoint => Properties?.TryGetFloatValue(ITexture.PropertyNames.SdrWhitePointFloat, out var sdrWhitePoint) is true
		? sdrWhitePoint
		: default;

	/// <inheritdoc/>
	public (float Width, float Height) Size
	{
		get
		{
			unsafe
			{
				Unsafe.SkipInit(out (float Width, float Height) size);

				ITexture.SDL_GetTextureSize(mTexture, &size.Width, &size.Height);

				return size;
			}
		}
	}

	/// <inheritdoc/>
	public int Width
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		get
		{
			unsafe
			{
				return mTexture is var texture
					&& texture is not null
						? texture->W
						: default;
			}
		}
	}

	/// <inheritdoc/>
	public void Dispose()
	{
		GC.SuppressFinalize(this);
		DisposeImpl(forget: true);
	}

	private void DisposeImpl(bool forget)
	{
		unsafe
		{
			if (mTexture is not null)
			{
				if (forget)
				{
					ITexture.Deregister(this);
				}

				// SDL_DestroyTexture decreases the native ref counter, so we don't need to do that manually here
				ITexture.SDL_DestroyTexture(mTexture);
				mTexture = null;
			}
		}
	}


	void ITexture.Dispose(bool disposing, bool forget) => DisposeImpl(forget);

	internal unsafe static bool TryGetOrCreate(ITexture.SDL_Texture* texture, [NotNullWhen(true)] out Texture<TDriver>? result)
		=> ITexture.TryGetOrCreate(texture, out result);

	/// <inheritdoc cref="ITexture.TryLock(in Rect{int}, out TexturePixelMemoryManager?)"/>
	public bool TryLock(in Rect<int> rect, [NotNullWhen(true)] out TexturePixelMemoryManager<TDriver>? pixelManager)
		=> TexturePixelMemoryManager<TDriver>.TryCreate(this, in rect, out pixelManager);

	/// <inheritdoc/>
	bool ITexture.TryLock(in Rect<int> rect, [NotNullWhen(true)] out TexturePixelMemoryManager? pixelManager)
	{
		var result = TryLock(in rect, out var typedPixelManager);

		pixelManager = typedPixelManager;

		return result;
	}

	/// <inheritdoc cref="ITexture.TryLock(out TexturePixelMemoryManager?)"/>
	public bool TryLock([NotNullWhen(true)] out TexturePixelMemoryManager<TDriver>? pixelManager)
		=> TexturePixelMemoryManager<TDriver>.TryCreate(this, out pixelManager);

	/// <inheritdoc/>
	bool ITexture.TryLock([NotNullWhen(true)] out TexturePixelMemoryManager? pixelManager)
	{
		var result = TryLock(out var typedPixelManager);

		pixelManager = typedPixelManager;

		return result;
	}

	/// <inheritdoc cref="ITexture.TryLockToSurface(in Rect{int}, out TextureSurfaceManager?)"/>
	public bool TryLockToSurface(in Rect<int> rect, [NotNullWhen(true)] out TextureSurfaceManager<TDriver>? surfaceManager)
		=> TextureSurfaceManager<TDriver>.TryCreate(this, in rect, out surfaceManager);

	/// <inheritdoc/>
	bool ITexture.TryLockToSurface(in Rect<int> rect, [NotNullWhen(true)] out TextureSurfaceManager? surfaceManager)
	{
		var result = TryLockToSurface(in rect, out var typedSurfaceManager);

		surfaceManager = typedSurfaceManager;

		return result;
	}

	/// <inheritdoc cref="ITexture.TryLockToSurface(out TextureSurfaceManager?)"/>
	public bool TryLockToSurface([NotNullWhen(true)] out TextureSurfaceManager<TDriver>? surfaceManager)
		=> TextureSurfaceManager<TDriver>.TryCreate(this, out surfaceManager);

	/// <inheritdoc/>
	bool ITexture.TryLockToSurface([NotNullWhen(true)] out TextureSurfaceManager? surfaceManager)
	{
		var result = TryLockToSurface(out var typedSurfaceManager);

		surfaceManager = typedSurfaceManager;

		return result;
	}

	/// <inheritdoc/>
	public bool TryUnsafeLock(in Rect<int> rect, out Utilities.NativeMemory pixels, out int pitch)
	{
		unsafe
		{
			void* pixelsTmp;
			Unsafe.SkipInit(out int pitchTmp);

			bool result;
			fixed (Rect<int>* rectPtr = &rect)
			{
				result = ITexture.SDL_LockTexture(mTexture, rectPtr, &pixelsTmp, &pitchTmp);
			}

			pitch = pitchTmp;

			if (!result)
			{
				pixels = Utilities.NativeMemory.Empty;
				return false;
			}

			pixels = new(pixelsTmp, unchecked((nuint)rect.Height * (nuint)pitch));
			return true;
		}
	}

	/// <inheritdoc/>
	public bool TryUnsafeLock(in Rect<int> rect, out Span<byte> pixels, out int pitch)
	{
		unsafe
		{
			void* pixelsTmp;
			Unsafe.SkipInit(out int pitchTmp);

			bool result;
			fixed (Rect<int>* rectPtr = &rect)
			{
				result = ITexture.SDL_LockTexture(mTexture, rectPtr, &pixelsTmp, &pitchTmp);
			}

			pitch = pitchTmp;

			if (!result)
			{
				pixels = [];
				return false;
			}

			pixels = MemoryMarshal.CreateSpan(ref Unsafe.AsRef<byte>(pixelsTmp), unchecked(rect.Height * pitch) switch { < 0 => 0, var length => length });
			return true;
		}
	}

	/// <inheritdoc/>
	public unsafe bool TryUnsafeLock(in Rect<int> rect, out void* pixels, out int pitch)
	{
		void* pixelsTmp;
		Unsafe.SkipInit(out int pitchTmp);

		bool result;
		fixed (Rect<int>* rectPtr = &rect)
		{
			result = ITexture.SDL_LockTexture(mTexture, rectPtr, &pixelsTmp, &pitchTmp);
		}

		pitch = pitchTmp;
		pixels = pixelsTmp;
		return result;
	}

	/// <inheritdoc/>
	public bool TryUnsafeLock(out Utilities.NativeMemory pixels, out int pitch)
	{
		unsafe
		{
			void* pixelsTmp;
			Unsafe.SkipInit(out int pitchTmp);

			bool result = ITexture.SDL_LockTexture(mTexture, null, &pixelsTmp, &pitchTmp);

			pitch = pitchTmp;

			if (!result)
			{
				pixels = Utilities.NativeMemory.Empty;
				return false;
			}

			pixels = new(pixelsTmp, unchecked((nuint)Height * (nuint)pitch));
			return true;
		}
	}

	/// <inheritdoc/>
	public bool TryUnsafeLock(out Span<byte> pixels, out int pitch)
	{
		unsafe
		{
			void* pixelsTmp;
			Unsafe.SkipInit(out int pitchTmp);

			bool result = ITexture.SDL_LockTexture(mTexture, null, &pixelsTmp, &pitchTmp);

			pitch = pitchTmp;

			if (!result)
			{
				pixels = [];
				return false;
			}

			pixels = MemoryMarshal.CreateSpan(ref Unsafe.AsRef<byte>(pixelsTmp), unchecked(Height * pitch) switch { < 0 => 0, var length => length });
			return true;
		}
	}

	/// <inheritdoc/>
	public unsafe bool TryUnsafeLock(out void* pixels, out int pitch)
	{
		void* pixelsTmp;
		Unsafe.SkipInit(out int pitchTmp);

		bool result = ITexture.SDL_LockTexture(mTexture, null, &pixelsTmp, &pitchTmp);

		pitch = pitchTmp;
		pixels = pixelsTmp;

		return result;
	}

	/// <inheritdoc/>
	public bool TryUnsafeLockToSurface(in Rect<int> rect, [NotNullWhen(true)] out Surface? surface)
	{
		unsafe
		{
			Surface.SDL_Surface* surfacePtr;

			fixed (Rect<int>* rectPtr = &rect)
			{
				if (!(bool)ITexture.SDL_LockTextureToSurface(mTexture, rectPtr, &surfacePtr))
				{
					surface = null;
					return false;
				}
			}

			if (!Surface.TryGetOrCreate(surfacePtr, out surface))
			{
				// if we somehow fail to create the surface, we need to unlock the texture in order for the native surface to be safely disposed
				ITexture.SDL_UnlockTexture(mTexture);

				surface = null;
				return false;
			}

			// TODO: docs: warn user that they must dispose the returned surface before unlocking the texture!
			return true;
		}
	}

	/// <inheritdoc/>
	public bool TryUnsafeLockToSurface([NotNullWhen(true)] out Surface? surface)
	{
		unsafe
		{
			Surface.SDL_Surface* surfacePtr;

			if (!(bool)ITexture.SDL_LockTextureToSurface(mTexture, null, &surfacePtr))
			{
				surface = null;
				return false;
			}

			if (!Surface.TryGetOrCreate(surfacePtr, out surface))
			{
				// if we somehow fail to create the surface, we need to unlock the texture in order for the native surface to be safely disposed
				ITexture.SDL_UnlockTexture(mTexture);

				surface = null;
				return false;
			}

			// TODO: docs: warn user that they must dispose the returned surface before unlocking the texture!
			return true;
		}
	}

	/// <inheritdoc/>
	public bool TryUpdate(in Rect<int> rect, ReadOnlyNativeMemory pixels, int pitch)
	{
		unsafe
		{
			if (!pixels.IsValid)
			{
				return false;
			}

			var required = unchecked((rect.Height is > 0 ? (nuint)rect.Height : 0) * (pitch is > 0 ? (nuint)pitch : 0));

			if (pixels.Length < required)
			{
				return false;
			}

			fixed (Rect<int>* rectPtr = &rect)
			{
				return ITexture.SDL_UpdateTexture(mTexture, rectPtr, pixels.RawPointer, pitch);
			}
		}
	}

	/// <inheritdoc/>
	public bool TryUpdate(in Rect<int> rect, ReadOnlySpan<byte> pixels, int pitch)
	{
		unsafe
		{
			var required = unchecked((rect.Height is > 0 ? (nuint)rect.Height : 0) * (pitch is > 0 ? (nuint)pitch : 0));

			if (unchecked((nuint)pixels.Length) < required)
			{
				return false;
			}

			fixed (byte* pixelsPtr = pixels)
			fixed (Rect<int>* rectPtr = &rect)
			{
				return ITexture.SDL_UpdateTexture(mTexture, rectPtr, pixelsPtr, pitch);
			}
		}
	}

	/// <inheritdoc/>
	public unsafe bool TryUpdate(in Rect<int> rect, void* pixels, int pitch)
	{
		fixed (Rect<int>* rectPtr = &rect)
		{
			return ITexture.SDL_UpdateTexture(mTexture, rectPtr, pixels, pitch);
		}
	}

	/// <inheritdoc/>
	public bool TryUpdate(ReadOnlyNativeMemory pixels, int pitch)
	{
		unsafe
		{
			if (!pixels.IsValid)
			{
				return false;
			}

			var height = Height;

			var required = unchecked((height is > 0 ? (nuint)height : 0) * (pitch is > 0 ? (nuint)pitch : 0));

			if (pixels.Length < required)
			{
				return false;
			}

			return ITexture.SDL_UpdateTexture(mTexture, rect: null, pixels.RawPointer, pitch);
		}
	}

	/// <inheritdoc/>
	public bool TryUpdate(ReadOnlySpan<byte> pixels, int pitch)
	{
		unsafe
		{
			var height = Height;

			var required = unchecked((height is > 0 ? (nuint)height : 0) * (pitch is > 0 ? (nuint)pitch : 0));

			if (unchecked((nuint)pixels.Length) < required)
			{
				return false;
			}

			fixed (byte* pixelsPtr = pixels)
			{
				return ITexture.SDL_UpdateTexture(mTexture, rect: null, pixelsPtr, pitch);
			}
		}
	}

	/// <inheritdoc/>
	public unsafe bool TryUpdate(void* pixels, int pitch)
	{
		return ITexture.SDL_UpdateTexture(mTexture, rect: null, pixels, pitch);
	}

	/// <inheritdoc/>
	public bool TryUpdateNv(in Rect<int> rect, ReadOnlyNativeMemory<byte> yPixels, int yPitch, ReadOnlyNativeMemory<byte> uvPixels, int uvPitch)
	{
		unsafe
		{
			if (!yPixels.IsValid || !uvPixels.IsValid)
			{
				return false;
			}

			var yRequired = unchecked((rect.Height is > 0 ? (nuint)rect.Height : 0) * (yPitch is > 0 ? (nuint)yPitch : 0));

			if (yPixels.Length < yRequired)
			{
				return false;
			}

			var uvRequired = unchecked((rect.Height is > 0 ? (nuint)rect.Height : 0) * (uvPitch is > 0 ? (nuint)uvPitch : 0));

			if (uvPixels.Length < uvRequired)
			{
				return false;
			}

			fixed (Rect<int>* rectPtr = &rect)
			{
				return ITexture.SDL_UpdateNVTexture(mTexture, rectPtr, yPixels.RawPointer, yPitch, uvPixels.RawPointer, uvPitch);
			}
		}
	}

	/// <inheritdoc/>
	public bool TryUpdateNv(in Rect<int> rect, ReadOnlySpan<byte> yPixels, int yPitch, ReadOnlySpan<byte> uvPixels, int uvPitch)
	{
		unsafe
		{
			var yRequired = unchecked((rect.Height is > 0 ? (nuint)rect.Height : 0) * (yPitch is > 0 ? (nuint)yPitch : 0));

			if (unchecked((nuint)yPixels.Length) < yRequired)
			{
				return false;
			}

			var uvRequired = unchecked((rect.Height is > 0 ? (nuint)rect.Height : 0) * (uvPitch is > 0 ? (nuint)uvPitch : 0));

			if (unchecked((nuint)uvPixels.Length) < uvRequired)
			{
				return false;
			}

			fixed (Rect<int>* rectPtr = &rect)
			fixed (byte* yPixelsPtr = yPixels, uvPixelsPtr = uvPixels)
			{
				return ITexture.SDL_UpdateNVTexture(mTexture, rectPtr, yPixelsPtr, yPitch, uvPixelsPtr, uvPitch);
			}
		}
	}

	/// <inheritdoc/>
	public unsafe bool TryUpdateNv(in Rect<int> rect, byte* yPixels, int yPitch, byte* uvPixels, int uvPitch)
	{
		fixed (Rect<int>* rectPtr = &rect)
		{
			return ITexture.SDL_UpdateNVTexture(mTexture, rectPtr, yPixels, yPitch, uvPixels, uvPitch);
		}
	}

	/// <inheritdoc/>
	public bool TryUpdateNv(ReadOnlyNativeMemory<byte> yPixels, int yPitch, ReadOnlyNativeMemory<byte> uvPixels, int uvPitch)
	{
		unsafe
		{
			if (!yPixels.IsValid || !uvPixels.IsValid)
			{
				return false;
			}

			var height = Height;

			var yRequired = unchecked((height is > 0 ? (nuint)height : 0) * (yPitch is > 0 ? (nuint)yPitch : 0));

			if (yPixels.Length < yRequired)
			{
				return false;
			}

			var uvRequired = unchecked((height is > 0 ? (nuint)height : 0) * (uvPitch is > 0 ? (nuint)uvPitch : 0));

			if (uvPixels.Length < uvRequired)
			{
				return false;
			}

			return ITexture.SDL_UpdateNVTexture(mTexture, rect: null, yPixels.RawPointer, yPitch, uvPixels.RawPointer, uvPitch);
		}
	}

	/// <inheritdoc/>
	public bool TryUpdateNv(ReadOnlySpan<byte> yPixels, int yPitch, ReadOnlySpan<byte> uvPixels, int uvPitch)
	{
		unsafe
		{
			var height = Height;

			var yRequired = unchecked((height is > 0 ? (nuint)height : 0) * (yPitch is > 0 ? (nuint)yPitch : 0));

			if (unchecked((nuint)yPixels.Length) < yRequired)
			{
				return false;
			}

			var uvRequired = unchecked((height is > 0 ? (nuint)height : 0) * (uvPitch is > 0 ? (nuint)uvPitch : 0));

			if (unchecked((nuint)uvPixels.Length) < uvRequired)
			{
				return false;
			}

			fixed (byte* yPixelsPtr = yPixels, uvPixelsPtr = uvPixels)
			{
				return ITexture.SDL_UpdateNVTexture(mTexture, rect: null, yPixelsPtr, yPitch, uvPixelsPtr, uvPitch);
			}
		}
	}

	/// <inheritdoc/>
	public unsafe bool TryUpdateNv(byte* yPixels, int yPitch, byte* uvPixels, int uvPitch)
	{
		return ITexture.SDL_UpdateNVTexture(mTexture, rect: null, yPixels, yPitch, uvPixels, uvPitch);
	}

	/// <inheritdoc/>
	public bool TryUpdateYuv(in Rect<int> rect, ReadOnlyNativeMemory<byte> yPixels, int yPitch, ReadOnlyNativeMemory<byte> uPixels, int uPitch, ReadOnlyNativeMemory<byte> vPixels, int vPitch)
	{
		unsafe
		{
			if (!yPixels.IsValid || !uPixels.IsValid || !vPixels.IsValid)
			{
				return false;
			}

			var yRequired = unchecked((rect.Height is > 0 ? (nuint)rect.Height : 0) * (yPitch is > 0 ? (nuint)yPitch : 0));

			if (yPixels.Length < yRequired)
			{
				return false;
			}

			var uRequired = unchecked((rect.Height is > 0 ? (nuint)rect.Height : 0) * (uPitch is > 0 ? (nuint)uPitch : 0));

			if (uPixels.Length < uRequired)
			{
				return false;
			}

			var vRequired = unchecked((rect.Height is > 0 ? (nuint)rect.Height : 0) * (vPitch is > 0 ? (nuint)vPitch : 0));

			if (vPixels.Length < vRequired)
			{
				return false;
			}

			fixed (Rect<int>* rectPtr = &rect)
			{
				return ITexture.SDL_UpdateYUVTexture(mTexture, rectPtr, yPixels.RawPointer, yPitch, uPixels.RawPointer, uPitch, vPixels.RawPointer, vPitch);
			}
		}
	}

	/// <inheritdoc/>
	public bool TryUpdateYuv(in Rect<int> rect, ReadOnlySpan<byte> yPixels, int yPitch, ReadOnlySpan<byte> uPixels, int uPitch, ReadOnlySpan<byte> vPixels, int vPitch)
	{
		unsafe
		{
			var yRequired = unchecked((rect.Height is > 0 ? (nuint)rect.Height : 0) * (yPitch is > 0 ? (nuint)yPitch : 0));

			if (unchecked((nuint)yPixels.Length) < yRequired)
			{
				return false;
			}

			var uRequired = unchecked((rect.Height is > 0 ? (nuint)rect.Height : 0) * (uPitch is > 0 ? (nuint)uPitch : 0));

			if (unchecked((nuint)uPixels.Length) < uRequired)
			{
				return false;
			}

			var vRequired = unchecked((rect.Height is > 0 ? (nuint)rect.Height : 0) * (vPitch is > 0 ? (nuint)vPitch : 0));

			if (unchecked((nuint)vPixels.Length) < vRequired)
			{
				return false;
			}

			fixed (Rect<int>* rectPtr = &rect)
			fixed (byte* yPixelsPtr = yPixels, uPixelsPtr = uPixels, vPixelsPtr = vPixels)
			{
				return ITexture.SDL_UpdateYUVTexture(mTexture, rectPtr, yPixelsPtr, yPitch, uPixelsPtr, uPitch, vPixelsPtr, vPitch);
			}
		}
	}

	/// <inheritdoc/>
	public unsafe bool TryUpdateYuv(in Rect<int> rect, byte* yPixels, int yPitch, byte* uPixels, int uPitch, byte* vPixels, int vPitch)
	{
		fixed (Rect<int>* rectPtr = &rect)
		{
			return ITexture.SDL_UpdateYUVTexture(mTexture, rectPtr, yPixels, yPitch, uPixels, uPitch, vPixels, vPitch);
		}
	}

	/// <inheritdoc/>
	public bool TryUpdateYuv(ReadOnlyNativeMemory<byte> yPixels, int yPitch, ReadOnlyNativeMemory<byte> uPixels, int uPitch, ReadOnlyNativeMemory<byte> vPixels, int vPitch)
	{
		unsafe
		{
			if (!yPixels.IsValid || !uPixels.IsValid || !vPixels.IsValid)
			{
				return false;
			}

			var height = Height;

			var yRequired = unchecked((height is > 0 ? (nuint)height : 0) * (yPitch is > 0 ? (nuint)yPitch : 0));

			if (yPixels.Length < yRequired)
			{
				return false;
			}

			var uRequired = unchecked((height is > 0 ? (nuint)height : 0) * (uPitch is > 0 ? (nuint)uPitch : 0));

			if (uPixels.Length < uRequired)
			{
				return false;
			}

			var vRequired = unchecked((height is > 0 ? (nuint)height : 0) * (vPitch is > 0 ? (nuint)vPitch : 0));

			if (vPixels.Length < vRequired)
			{
				return false;
			}

			return ITexture.SDL_UpdateYUVTexture(mTexture, rect: null, yPixels.RawPointer, yPitch, uPixels.RawPointer, uPitch, vPixels.RawPointer, vPitch);
		}
	}

	/// <inheritdoc/>
	public bool TryUpdateYuv(ReadOnlySpan<byte> yPixels, int yPitch, ReadOnlySpan<byte> uPixels, int uPitch, ReadOnlySpan<byte> vPixels, int vPitch)
	{
		unsafe
		{
			var height = Height;

			var yRequired = unchecked((height is > 0 ? (nuint)height : 0) * (yPitch is > 0 ? (nuint)yPitch : 0));

			if (unchecked((nuint)yPixels.Length) < yRequired)
			{
				return false;
			}

			var uRequired = unchecked((height is > 0 ? (nuint)height : 0) * (uPitch is > 0 ? (nuint)uPitch : 0));

			if (unchecked((nuint)uPixels.Length) < uRequired)
			{
				return false;
			}

			var vRequired = unchecked((height is > 0 ? (nuint)height : 0) * (vPitch is > 0 ? (nuint)vPitch : 0));

			if (unchecked((nuint)vPixels.Length) < vRequired)
			{
				return false;
			}

			fixed (byte* yPixelsPtr = yPixels, uPixelsPtr = uPixels, vPixelsPtr = vPixels)
			{
				return ITexture.SDL_UpdateYUVTexture(mTexture, rect: null, yPixelsPtr, yPitch, uPixelsPtr, uPitch, vPixelsPtr, vPitch);
			}
		}
	}

	/// <inheritdoc/>
	public unsafe bool TryUpdateYuv(byte* yPixels, int yPitch, byte* uPixels, int uPitch, byte* vPixels, int vPitch)
	{
		return ITexture.SDL_UpdateYUVTexture(mTexture, rect: null, yPixels, yPitch, uPixels, uPitch, vPixels, vPitch);
	}

	/// <inheritdoc/>
	public void UnsafeUnlock()
	{
		unsafe
		{
			ITexture.SDL_UnlockTexture(mTexture);
		}
	}
}
