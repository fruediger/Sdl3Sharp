using Sdl3Sharp.Utilities;
using Sdl3Sharp.Video.Blending;
using Sdl3Sharp.Video.Coloring;
using Sdl3Sharp.Video.Drawing;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace Sdl3Sharp.Video.Rendering;

public sealed partial class Texture
{
	private unsafe SDL_Texture* mTexture;

	public TextureAccess Access => Properties?.TryGetNumberValue(PropertyNames.AccessNumber, out var access) is true
		? unchecked((TextureAccess)access)
		: default;

	public byte AlphaMod
	{
		get
		{
			unsafe
			{
				Unsafe.SkipInit(out byte alpha);

				SDL_GetTextureAlphaMod(mTexture, &alpha);

				return alpha;
			}
		}

		set
		{
			unsafe
			{
				SDL_SetTextureAlphaMod(mTexture, value);
			}
		}
	}

	public float AlphaModFloat
	{
		get
		{
			unsafe
			{
				Unsafe.SkipInit(out float alpha);

				SDL_GetTextureAlphaModFloat(mTexture, &alpha);

				return alpha;
			}
		}

		set
		{
			unsafe
			{
				SDL_SetTextureAlphaModFloat(mTexture, value);
			}
		}
	}

	public BlendMode BlendMode
	{
		get
		{
			unsafe
			{
				Unsafe.SkipInit(out BlendMode blendMode);

				SDL_GetTextureBlendMode(mTexture, &blendMode);

				return blendMode;
			}

		}

		set
		{
			unsafe
			{
				if (!(bool)SDL_SetTextureBlendMode(mTexture, value)
					&& Error.SDL_GetError() is var message
					&& message is not null
					&& !MemoryMarshal.CreateReadOnlySpanFromNullTerminated(message).SequenceEqual("Parameter 'texture' is invalid"u8) /* filter out "texture" argument errors */)
				{
					// value is BlendMode.Invalid or value is an unsupported blend mode for the renderer
					// Although the offical SDL docs say that "If the blend mode is not supported, the closest supported mode is chosen and this function returns false.",
					// that doesn't appear to be the case looking at the SDL source code.
					// It just fails early if the blend mode is not supported and doesn't try to choose a "closest supported mode".

					failSdlError(message);
				}
			}

			[DoesNotReturn]
			static unsafe void failSdlError(byte* message) => throw new SdlException(Utf8StringMarshaller.ConvertToManaged(message));
		}
	}

	public (byte R, byte G, byte B) ColorMod
	{
		get
		{
			unsafe
			{
				Unsafe.SkipInit(out (byte R, byte G, byte B) color);

				SDL_GetTextureColorMod(mTexture, &color.R, &color.G, &color.B);

				return color;
			}
		}

		set
		{
			unsafe
			{
				SDL_SetTextureColorMod(mTexture, value.R, value.G, value.B);
			}
		}
	}

	public (float R, float G, float B) ColorModFloat
	{
		get
		{
			unsafe
			{
				Unsafe.SkipInit(out (float R, float G, float B) color);

				SDL_GetTextureColorModFloat(mTexture, &color.R, &color.G, &color.B);

				return color;
			}
		}

		set
		{
			unsafe
			{
				SDL_SetTextureColorModFloat(mTexture, value.R, value.G, value.B);
			}
		}
	}

	public ColorSpace ColorSpace => Properties?.TryGetNumberValue(PropertyNames.ColorSpaceNumber, out var colorSpace) is true
		? unchecked((ColorSpace)colorSpace)
		: default;

	public IntPtr D3D11Texture => Properties?.TryGetPointerValue(PropertyNames.D3D11TexturePointer, out var texture) is true
		? texture
		: default;

	public IntPtr D3D11TextureU => Properties?.TryGetPointerValue(PropertyNames.D3D11TextureUPointer, out var textureU) is true
		? textureU
		: default;

	public IntPtr D3D11TextureV => Properties?.TryGetPointerValue(PropertyNames.D3D11TextureVPointer, out var textureV) is true
		? textureV
		: default;

	public IntPtr D3D12Texture => Properties?.TryGetPointerValue(PropertyNames.D3D12TexturePointer, out var texture) is true
		? texture
		: default;

	public IntPtr D3D12TextureU => Properties?.TryGetPointerValue(PropertyNames.D3D12TextureUPointer, out var textureU) is true
		? textureU
		: default;

	public IntPtr D3D12TextureV => Properties?.TryGetPointerValue(PropertyNames.D3D12TextureVPointer, out var textureV) is true
		? textureV
		: default;

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

	public float HdrHeadroom => Properties?.TryGetFloatValue(PropertyNames.HdrHeadroomFloat, out var hdrHeadroom) is true
		? hdrHeadroom
		: default;

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

	public uint OpenGlTexture => Properties?.TryGetNumberValue(PropertyNames.OpenGlTextureNumber, out var texture) is true
		? unchecked((uint)texture)
		: default;

	public uint OpenGlTextureU => Properties?.TryGetNumberValue(PropertyNames.OpenGlTextureUNumber, out var textureU) is true
		? unchecked((uint)textureU)
		: default;

	public uint OpenGlTextureUv => Properties?.TryGetNumberValue(PropertyNames.OpenGlTextureUvNumber, out var textureUv) is true
		? unchecked((uint)textureUv)
		: default;

	public uint OpenGlTextureV => Properties?.TryGetNumberValue(PropertyNames.OpenGlTextureVNumber, out var textureV) is true
		? unchecked((uint)textureV)
		: default;

	public uint OpenGlTextureTarget => Properties?.TryGetNumberValue(PropertyNames.OpenGlTextureTargetNumber, out var target) is true
		? unchecked((uint)target)
		: default;

	public float OpenGlTexH => Properties?.TryGetFloatValue(PropertyNames.OpenGlTexHFloat, out var texH) is true
		? texH
		: default;

	public float OpenGlTexW => Properties?.TryGetFloatValue(PropertyNames.OpenGlTexWFloat, out var texW) is true
		? texW
		: default;

#if SDL3_4_0_OR_GREATER

	public Palette? Palette
	{
		get
		{
			unsafe
			{
				if (!Palette.TryGetOrCreate(SDL_GetTexturePalette(mTexture), out var palette))
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

				if (!(bool)SDL_SetTexturePalette(mTexture, value is not null ? value.Pointer : null)
					&& Error.SDL_GetError() is var message
					&& message is not null
					&& !MemoryMarshal.CreateReadOnlySpanFromNullTerminated(message).SequenceEqual("Parameter 'texture' is invalid"u8) /* filter out "texture" argument errors */)
				{
					failSdlError(message);
				}
			}

			[DoesNotReturn]
			static unsafe void failSdlError(byte* message) => throw new SdlException(Utf8StringMarshaller.ConvertToManaged(message));
		}
	}

#endif

	internal unsafe SDL_Texture* Pointer { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mTexture; }

	public Properties? Properties
	{
		get
		{
			unsafe
			{
				return SDL_GetTextureProperties(mTexture) switch
				{
					0 => null,
					var id => Properties.GetOrCreate(sdl: null, id)
				};
			}
		}
	}

	public ScaleMode ScaleMode
	{
		get
		{
			unsafe
			{
				Unsafe.SkipInit(out ScaleMode scaleMode);

				SDL_GetTextureScaleMode(mTexture, &scaleMode);

				return scaleMode;
			}
		}

		set
		{
			unsafe
			{
				if (!(bool)SDL_SetTextureScaleMode(mTexture, value)
					&& Error.SDL_GetError() is var message
					&& message is not null
					&& !MemoryMarshal.CreateReadOnlySpanFromNullTerminated(message).SequenceEqual("Parameter 'texture' is invalid"u8) /* filter out "texture" argument errors */)
				{
					// value is ScaleMode.Invalid or none of the defined scale modes
					// Although the offical SDL docs say that "If the scale mode is not supported, the closest supported mode is chosen.",
					// that doesn't appear to be the case looking at the SDL source code.
					// It just fails early if the scale mode is not supported and doesn't try to choose a "closest supported mode".

					failSdlError(message);
				}
			}

			[DoesNotReturn]
			static unsafe void failSdlError(byte* message) => throw new SdlException(Utf8StringMarshaller.ConvertToManaged(message));
		}
	}

	public float SdrWhitePoint => Properties?.TryGetFloatValue(PropertyNames.SdrWhitePointFloat, out var sdrWhitePoint) is true
		? sdrWhitePoint
		: default;

	public (float Width, float Height) Size
	{
		get
		{
			unsafe
			{
				Unsafe.SkipInit(out (float Width, float Height) size);

				SDL_GetTextureSize(mTexture, &size.Width, &size.Height);

				return size;
			}
		}
	}

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

	public bool TryLock(in Rect<int> rect, [NotNullWhen(true)] out TexturePixelMemoryManager? pixelManager)
		=> TexturePixelMemoryManager.TryCreate(this, in rect, out pixelManager);

	public bool TryLock([NotNullWhen(true)] out TexturePixelMemoryManager? pixelManager)
		=> TexturePixelMemoryManager.TryCreate(this, out pixelManager);

	public bool TryLockToSurface(in Rect<int> rect, [NotNullWhen(true)] out TextureSurfaceManager? surfaceManager)
		=> TextureSurfaceManager.TryCreate(this, in rect, out surfaceManager);

	public bool TryLockToSurface([NotNullWhen(true)] out TextureSurfaceManager? surfaceManager)
		=> TextureSurfaceManager.TryCreate(this, out surfaceManager);

	public bool TryUnsafeLock(in Rect<int> rect, out Utilities.NativeMemory pixels, out int pitch)
	{
		unsafe
		{
			void* pixelsTmp;
			Unsafe.SkipInit(out int pitchTmp);

			bool result;
			fixed (Rect<int>* rectPtr = &rect)
			{
				result = SDL_LockTexture(mTexture, rectPtr, &pixelsTmp, &pitchTmp);
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

	public bool TryUnsafeLock(in Rect<int> rect, out Span<byte> pixels, out int pitch)
	{
		unsafe
		{
			void* pixelsTmp;
			Unsafe.SkipInit(out int pitchTmp);

			bool result;
			fixed (Rect<int>* rectPtr = &rect)
			{
				result = SDL_LockTexture(mTexture, rectPtr, &pixelsTmp, &pitchTmp);
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

	public unsafe bool TryUnsafeLock(in Rect<int> rect, out void* pixels, out int pitch)
	{
		void* pixelsTmp;
		Unsafe.SkipInit(out int pitchTmp);

		bool result;
		fixed (Rect<int>* rectPtr = &rect)
		{
			result = SDL_LockTexture(mTexture, rectPtr, &pixelsTmp, &pitchTmp);
		}

		pitch = pitchTmp;
		pixels = pixelsTmp;
		return result;
	}

	public bool TryUnsafeLock(out Utilities.NativeMemory pixels, out int pitch)
	{
		unsafe
		{
			void* pixelsTmp;
			Unsafe.SkipInit(out int pitchTmp);

			bool result = SDL_LockTexture(mTexture, null, &pixelsTmp, &pitchTmp);

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

	public bool TryUnsafeLock(out Span<byte> pixels, out int pitch)
	{
		unsafe
		{
			void* pixelsTmp;
			Unsafe.SkipInit(out int pitchTmp);

			bool result = SDL_LockTexture(mTexture, null, &pixelsTmp, &pitchTmp);

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

	public unsafe bool TryUnsafeLock(out void* pixels, out int pitch)
	{
		void* pixelsTmp;
		Unsafe.SkipInit(out int pitchTmp);

		bool result = SDL_LockTexture(mTexture, null, &pixelsTmp, &pitchTmp);

		pitch = pitchTmp;
		pixels = pixelsTmp;

		return result;
	}

	public bool TryUnsafeLockToSurface(in Rect<int> rect, [NotNullWhen(true)] out Surface? surface)
	{
		unsafe
		{
			Surface.SDL_Surface* surfacePtr;

			fixed (Rect<int>* rectPtr = &rect)
			{
				if (!(bool)SDL_LockTextureToSurface(mTexture, rectPtr, &surfacePtr))
				{
					surface = null;
					return false;
				}
			}

			if (!Surface.TryGetOrCreate(surfacePtr, out surface))
			{
				// if we somehow fail to create the surface, we need to unlock the texture in order for the native surface to be safely disposed
				SDL_UnlockTexture(mTexture);

				surface = null;
				return false;
			}

			// TODO: docs: warn user that they must dispose the returned surface before unlocking the texture!
			return true;
		}
	}

	public bool TryUnsafeLockToSurface([NotNullWhen(true)] out Surface? surface)
	{
		unsafe
		{
			Surface.SDL_Surface* surfacePtr;

			if (!(bool)SDL_LockTextureToSurface(mTexture, null, &surfacePtr))
			{
				surface = null;
				return false;
			}

			if (!Surface.TryGetOrCreate(surfacePtr, out surface))
			{
				// if we somehow fail to create the surface, we need to unlock the texture in order for the native surface to be safely disposed
				SDL_UnlockTexture(mTexture);

				surface = null;
				return false;
			}

			// TODO: docs: warn user that they must dispose the returned surface before unlocking the texture!
			return true;
		}
	}

	public void UnsafeUnlock()
	{
		unsafe
		{
			SDL_UnlockTexture(mTexture);
		}
	}
}
