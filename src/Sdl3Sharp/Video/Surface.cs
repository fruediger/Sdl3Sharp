using Sdl3Sharp.Utilities;
using Sdl3Sharp.Video.Blending;
using Sdl3Sharp.Video.Coloring;
using Sdl3Sharp.Video.Drawing;
using System;
using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Video;

public partial class Surface : ICloneable, IDisposable
{
	private interface IUnsafeConstructorDispatch;

	/// <exception cref="ArgumentException">
	/// <paramref name="pixels"/> is <see cref="NativeMemory.IsValid">invalid</see>
	/// - or -
	/// <paramref name="pixels"/> is too small
	/// </exception>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private static unsafe void* ValidateAndPinNativeMemory(Utilities.NativeMemory pixels, int height, int pitch, out NativeMemoryPin nativeMemoryPin)
	{
		if (!pixels.IsValid)
		{
			failPixelsArgumentInvalid();
		}

		var required = unchecked((height is > 0 ? (nuint)height : 0) * (pitch is > 0 ? (nuint)pitch : 0));

		if (pixels.Length < required)
		{
			failPixelsArgumentTooSmall(required);
		}

		nativeMemoryPin = pixels.Pin();
		
		return pixels.RawPointer;

		[DoesNotReturn]
		static void failPixelsArgumentInvalid() => throw new ArgumentException(message: $"{nameof(pixels)} is invalid", paramName: nameof(pixels));

		[DoesNotReturn]
		static void failPixelsArgumentTooSmall(nuint required) => throw new ArgumentException(message: $"{nameof(pixels)} is too small. The buffer must be at least {required} in size.", paramName: nameof(pixels));
	}

	/// <exception cref="ArgumentException"><paramref name="pixels"/> is too small</exception>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private static unsafe void* ValidateAndPinMemory(Memory<byte> pixels, int height, int pitch, out MemoryHandle memoryHandle)
	{
		var required = unchecked((height is > 0 ? (nuint)height : 0) * (pitch is > 0 ? (nuint)pitch : 0));

		if (unchecked((nuint)pixels.Length) < required)
		{
			failPixelsArgumentTooSmall(required);
		}

		memoryHandle = pixels.Pin();

		return memoryHandle.Pointer;

		[DoesNotReturn]
		static void failPixelsArgumentTooSmall(nuint required) => throw new ArgumentException(message: $"{nameof(pixels)} is too small. The buffer must be at least {required} in size.", paramName: nameof(pixels));
	}

	private protected unsafe SDL_Surface* SurfacePointer; // make the SurfacePointer field accessible from derived types inside this assembly,
														  // so that they can manipulate it in their own Dispose mechanism without relying on the base implementation
														  // (e.g. 'VulkanSurface' ('VulkanSurface's should call 'SDL_Vulkan_DestroySurface' instead of 'SDL_DestroySurface',
														  // and therefore shouldn't call the 'base.Dispose'))
	private NativeMemory mNativeMemory = default;
	private NativeMemoryPin? mNativeMemoryPin = null;
	private Memory<byte> mMemory = default;
	private MemoryHandle mMemoryHandle = default;

	private protected unsafe Surface(SDL_Surface* surface) => SurfacePointer = surface;

	/// <exception cref="SdlException">The <see cref="Surface"/> could not be created (check <see cref="Error.TryGet(out string?)"/> for more information)</exception>
	private unsafe Surface(int width, int height, PixelFormat format, IUnsafeConstructorDispatch? _ = default) :
		this(SDL_CreateSurface(width, height, format))
	{
		if (SurfacePointer is null)
		{
			failCouldNotCreateSurface();
		}

		[DoesNotReturn]
		static void failCouldNotCreateSurface() => throw new SdlException($"Could not create the {nameof(Surface)}");
	}

	// /// <inheritdoc cref="Surface(int, int, PixelFormat, IUnsafeConstructorDispatch?)"/>
	public Surface(int width, int height, PixelFormat format) :
#pragma warning disable IDE0034 // Keep it that way for explicitness sake
		this(width, height, format, default(IUnsafeConstructorDispatch?))
#pragma warning restore IDE0034
	{ }

	/// <inheritdoc cref="ValidateAndPinNativeMemory(Utilities.NativeMemory, int, int, out NativeMemoryPin)"/>
	private unsafe Surface(int width, int height, PixelFormat format, Utilities.NativeMemory pixels, int pitch, IUnsafeConstructorDispatch? _ = default) :
		this(width, height, format, ValidateAndPinNativeMemory(pixels, height, pitch, out var nativeMemoryPin), pitch)
	{
		mNativeMemory = pixels;
		mNativeMemoryPin = nativeMemoryPin;
	}

	// /// <inheritdoc cref="Surface(int, int, PixelFormat, Utilities.NativeMemory, int, IUnsafeConstructorDispatch?)"/>
	public Surface(int width, int height, PixelFormat format, Utilities.NativeMemory pixels, int pitch) :
#pragma warning disable IDE0034 // Keep it that way for explicitness sake
		this(width, height, format, pixels, pitch, default(IUnsafeConstructorDispatch?))
#pragma warning restore IDE0034
	{ }

	/// <inheritdoc cref="ValidateAndPinMemory(Memory{byte}, int, int, out MemoryHandle)"/>
	private unsafe Surface(int width, int height, PixelFormat format, Memory<byte> pixels, int pitch, IUnsafeConstructorDispatch? _ = default) :
		this(width, height, format, ValidateAndPinMemory(pixels, height, pitch, out var memoryHandle), pitch)
	{
		mMemory = pixels;
		mMemoryHandle = memoryHandle;
	}

	// /// <inheritdoc cref="Surface(int, int, PixelFormat, Memory{byte}, int, IUnsafeConstructorDispatch?)"/>
	public Surface(int width, int height, PixelFormat format, Memory<byte> pixels, int pitch) :
#pragma warning disable IDE0034 // Keep it that way for explicitness sake
		this(width, height, format, pixels, pitch, default(IUnsafeConstructorDispatch?))
#pragma warning restore IDE0034
	{ }

	// /// <exception cref="SdlException">The <see cref="Surface"/> could not be created (check <see cref="Error.TryGet(out string?)"/> for more information)</exception>
	public unsafe Surface(int width, int height, PixelFormat format, void* pixels, int pitch) :
		this(SDL_CreateSurfaceFrom(width, height, format, pixels, pitch))
	{
		if (SurfacePointer is null)
		{
			failCouldNotCreateSurface();
		}

		[DoesNotReturn]
		static void failCouldNotCreateSurface() => throw new SdlException($"Could not create the {nameof(Surface)}");
	}

	~Surface() => Dispose(disposing: false);

	public SurfaceFlags Flags
	{
		get
		{
			unsafe
			{
				return SurfacePointer is var surface
					&& surface is not null
						? surface->Flags
						: default;
			}
		}
	}

	public PixelFormat Format
	{
		get
		{
			unsafe
			{
				return SurfacePointer is var surface
					&& surface is not null
						? surface->Format
						: default;
			}
		}
	}

	public bool HasColorKey
	{
		get
		{
			unsafe
			{
				return SDL_SurfaceHasColorKey(SurfacePointer);
			}
		}
	}

	public int Height
	{
		get
		{
			unsafe
			{
				return SurfacePointer is var surface
					&& surface is not null
						? surface->H
						: default;
			}
		}
	}

	public int Pitch
	{
		get
		{
			unsafe
			{
				return SurfacePointer is var surface
					&& surface is not null
					? surface->Pitch
					: default;
			}
		}
	}

	public Utilities.NativeMemory Pixels
	{
		get
		{
			unsafe
			{
				return SurfacePointer is var surface
					&& surface is not null
					&& surface->Pixels is var pixels
					&& pixels is not null
						? new(pixels, unchecked((nuint)surface->H * (nuint)surface->Pitch))
						: Utilities.NativeMemory.Empty;
			}
		}
	}

	public int Width
	{
		get
		{
			unsafe
			{
				return SurfacePointer is var surface
					&& surface is not null
						? surface->W
						: default;
			}
		}
	}

	/// <summary>
	/// Clones the <see cref="Surface"/>
	/// </summary>
	/// <returns>An identical clone of the <see cref="Surface"/>. This can be safely cast to the actual type of the calling class.</returns>
	/// <remarks>
	/// <para>
	/// If the <see cref="Surface"/> has alternate images, the resulting clone will have a reference to them as well.
	/// </para>
	/// <para>
	/// The return value of this method can be safely cast to the actual type of the calling class.
	/// </para>
	/// </remarks>
	/// <exception cref="SdlException">Couldn't duplicate the <see cref="Surface"/> (check <see cref="Error.TryGet(out string?)"/> for more information)</exception>
	public virtual Surface Clone()
	{
		unsafe
		{
			var clonePtr = SDL_DuplicateSurface(SurfacePointer);
			if (clonePtr is null)
			{
				failCouldNotDuplicateSurface();
			}

			var clone = Unsafe.As<Surface>(RuntimeHelpers.GetUninitializedObject(GetType()));
			clone.SurfacePointer = clonePtr;

			clone.mNativeMemory = mNativeMemory;
			clone.mNativeMemoryPin = mNativeMemoryPin is not null ? clone.mNativeMemory.Pin() : null;

			clone.mMemory = mMemory;
			clone.mMemoryHandle = mMemoryHandle.Pointer is not null ? clone.mMemory.Pin() : default;

			return clone;
		}

		[DoesNotReturn]
		static void failCouldNotDuplicateSurface() => throw new SdlException($"Could not duplicate the {nameof(Surface)}");
	}

	/// <inheritdoc/>
	object ICloneable.Clone() => Clone();

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		unsafe
		{
			if (SurfacePointer is not null)
			{
				SDL_DestroySurface(SurfacePointer);
				SurfacePointer = null;
			}

			mNativeMemory = default;

			if (mNativeMemoryPin is not null)
			{
				mNativeMemoryPin.Dispose();
				mNativeMemoryPin = null;
			}

			mMemory = default;

			if (mMemoryHandle.Pointer is not null)
			{
				mMemoryHandle.Dispose();
				mMemoryHandle = default;
			}
		}
	}

	public uint MapColor(byte r, byte g, byte b)
	{
		unsafe
		{
			return SDL_MapSurfaceRGB(SurfacePointer, r, g, b);
		}
	}

	public uint MapColor(byte r, byte g, byte b, byte a)
	{
		unsafe
		{
			return SDL_MapSurfaceRGBA(SurfacePointer, r, g, b, a);
		}
	}

	public uint MapColor(Color<byte> color) => MapColor(color.R, color.G, color.B, color.A);

	public void ResetClippingRect()
	{
		unsafe
		{
			SDL_SetSurfaceClipRect(SurfacePointer, rect: null);
		}
	}

	public void ResetColorKey()
	{
		unsafe
		{
			SDL_SetSurfaceColorKey(SurfacePointer, enabled: false, key: 0);
		}
	}

	public bool SetClippingRect(in Rect<int> rect)
	{
		unsafe
		{
			fixed (Rect<int>* rectPtr = &rect)
			{
				return SDL_SetSurfaceClipRect(SurfacePointer, rectPtr);
			}
		}
	}

	public bool TryAddAlternateImage(Surface image)
	{
		unsafe
		{
			return SDL_AddSurfaceAlternateImage(SurfacePointer, image is not null ? image.SurfacePointer : null);

			// TODO: what about SDL_DestroySurface on the image?
		}
	}

	public bool TryBlit(in Rect<int> destinationRect, Surface source, in Rect<int> sourceRect)
	{
		unsafe
		{
			fixed (Rect<int>* dstrect = &destinationRect, srcrect = &sourceRect)
			{
				return SDL_BlitSurface(source is not null ? source.SurfacePointer : null, srcrect, SurfacePointer, dstrect);
			}
		}
	}

	public bool TryBlit(in Rect<int> destinationRect, Surface source)
	{
		unsafe
		{
			fixed (Rect<int>* dstrect = &destinationRect)
			{
				return SDL_BlitSurface(source is not null ? source.SurfacePointer : null, srcrect: null, SurfacePointer, dstrect);
			}
		}
	}

	public bool TryBlit(Surface source, in Rect<int> sourceRect)
	{
		unsafe
		{
			fixed (Rect<int>* srcrect = &sourceRect)
			{
				return SDL_BlitSurface(source is not null ? source.SurfacePointer : null, srcrect, SurfacePointer, dstrect: null);
			}
		}
	}

	public bool TryBlit(Surface source)
	{
		unsafe
		{
			return SDL_BlitSurface(source is not null ? source.SurfacePointer : null, srcrect: null, SurfacePointer, dstrect: null);
		}
	}

	public bool TryBlit9Grid(in Rect<int> destinationRect, Surface source, in Rect<int> sourceRect, int leftWidth, int rightWidth, int topHeight, int bottomHeight, float scale, ScaleMode scaleMode)
	{
		unsafe
		{
			fixed (Rect<int>* dstrect = &destinationRect, srcrect = &sourceRect)
			{
				return SDL_BlitSurface9Grid(source is not null ? source.SurfacePointer : null, srcrect, leftWidth, rightWidth, topHeight, bottomHeight, scale, scaleMode, SurfacePointer, dstrect);
			}
		}
	}

	public bool TryBlit9Grid(in Rect<int> destinationRect, Surface source, int leftWidth, int rightWidth, int topHeight, int bottomHeight, float scale, ScaleMode scaleMode)
	{
		unsafe
		{
			fixed (Rect<int>* dstrect = &destinationRect)
			{
				return SDL_BlitSurface9Grid(source is not null ? source.SurfacePointer : null, srcrect: null, leftWidth, rightWidth, topHeight, bottomHeight, scale, scaleMode, SurfacePointer, dstrect);
			}
		}
	}

	public bool TryBlit9Grid(Surface source, in Rect<int> sourceRect, int leftWidth, int rightWidth, int topHeight, int bottomHeight, float scale, ScaleMode scaleMode)
	{
		unsafe
		{
			fixed (Rect<int>* srcrect = &sourceRect)
			{
				return SDL_BlitSurface9Grid(source is not null ? source.SurfacePointer : null, srcrect, leftWidth, rightWidth, topHeight, bottomHeight, scale, scaleMode, SurfacePointer, dstrect: null);
			}
		}
	}

	public bool TryBlit9Grid(Surface source, int leftWidth, int rightWidth, int topHeight, int bottomHeight, float scale, ScaleMode scaleMode)
	{
		unsafe
		{
			return SDL_BlitSurface9Grid(source is not null ? source.SurfacePointer : null, srcrect: null, leftWidth, rightWidth, topHeight, bottomHeight, scale, scaleMode, SurfacePointer, dstrect: null);
		}
	}

	public bool TryBlitScaled(in Rect<int> destinationRect, Surface source, in Rect<int> sourceRect, ScaleMode scaleMode)
	{
		unsafe
		{
			fixed (Rect<int>* dstrect = &destinationRect, srcrect = &sourceRect)
			{
				return SDL_BlitSurfaceScaled(source is not null ? source.SurfacePointer : null, srcrect, SurfacePointer, dstrect, scaleMode);
			}
		}
	}

	public bool TryBlitScaled(in Rect<int> destinationRect, Surface source, ScaleMode scaleMode)
	{
		unsafe
		{
			fixed (Rect<int>* dstrect = &destinationRect)
			{
				return SDL_BlitSurfaceScaled(source is not null ? source.SurfacePointer : null, srcrect: null, SurfacePointer, dstrect, scaleMode);
			}
		}
	}

	public bool TryBlitScaled(Surface source, in Rect<int> sourceRect, ScaleMode scaleMode)
	{
		unsafe
		{
			fixed (Rect<int>* srcrect = &sourceRect)
			{
				return SDL_BlitSurfaceScaled(source is not null ? source.SurfacePointer : null, srcrect, SurfacePointer, dstrect: null, scaleMode);
			}
		}
	}

	public bool TryBlitScaled(Surface source, ScaleMode scaleMode)
	{
		unsafe
		{
			return SDL_BlitSurfaceScaled(source is not null ? source.SurfacePointer : null, srcrect: null, SurfacePointer, dstrect: null, scaleMode);
		}
	}

	public bool TryBlitTiled(in Rect<int> destinationRect, Surface source, in Rect<int> sourceRect)
	{
		unsafe
		{
			fixed (Rect<int>* dstrect = &destinationRect, srcrect = &sourceRect)
			{
				return SDL_BlitSurfaceTiled(source is not null ? source.SurfacePointer : null, srcrect, SurfacePointer, dstrect);
			}
		}
	}

	public bool TryBlitTiled(in Rect<int> destinationRect, Surface source)
	{
		unsafe
		{
			fixed (Rect<int>* dstrect = &destinationRect)
			{
				return SDL_BlitSurfaceTiled(source is not null ? source.SurfacePointer : null, srcrect: null, SurfacePointer, dstrect);
			}
		}
	}

	public bool TryBlitTiled(Surface source, in Rect<int> sourceRect)
	{
		unsafe
		{
			fixed (Rect<int>* srcrect = &sourceRect)
			{
				return SDL_BlitSurfaceTiled(source is not null ? source.SurfacePointer : null, srcrect, SurfacePointer, dstrect: null);
			}
		}
	}
	public bool TryBlitTiled(Surface source)
	{
		unsafe
		{
			return SDL_BlitSurfaceTiled(source is not null ? source.SurfacePointer : null, srcrect: null, SurfacePointer, dstrect: null);
		}
	}

	public bool TryBlitTiledWithScale(in Rect<int> destinationRect, Surface source, in Rect<int> sourceRect, float scale, ScaleMode scaleMode)
	{
		unsafe
		{
			fixed (Rect<int>* dstrect = &destinationRect, srcrect = &sourceRect)
			{
				return SDL_BlitSurfaceTiledWithScale(source is not null ? source.SurfacePointer : null, srcrect, scale, scaleMode, SurfacePointer, dstrect);
			}
		}
	}

	public bool TryBlitTiledWithScale(in Rect<int> destinationRect, Surface source, float scale, ScaleMode scaleMode)
	{
		unsafe
		{
			fixed (Rect<int>* dstrect = &destinationRect)
			{
				return SDL_BlitSurfaceTiledWithScale(source is not null ? source.SurfacePointer : null, srcrect: null, scale, scaleMode, SurfacePointer, dstrect);
			}
		}
	}

	public bool TryBlitTiledWithScale(Surface source, in Rect<int> sourceRect, float scale, ScaleMode scaleMode)
	{
		unsafe
		{
			fixed (Rect<int>* srcrect = &sourceRect)
			{
				return SDL_BlitSurfaceTiledWithScale(source is not null ? source.SurfacePointer : null, srcrect, scale, scaleMode, SurfacePointer, dstrect: null);
			}
		}
	}

	public bool TryBlitTiledWithScale(Surface source, float scale, ScaleMode scaleMode)
	{
		unsafe
		{
			return SDL_BlitSurfaceTiledWithScale(source is not null ? source.SurfacePointer : null, srcrect: null, scale, scaleMode, SurfacePointer, dstrect: null);
		}
	}

	public bool TryBlitUnchecked(in Rect<int> destinationRect, Surface source, in Rect<int> sourceRect)
	{
		unsafe
		{
			fixed (Rect<int>* dstrect = &destinationRect, srcrect = &sourceRect)
			{
				return SDL_BlitSurfaceUnchecked(source is not null ? source.SurfacePointer : null, srcrect, SurfacePointer, dstrect);
			}
		}
	}

	public bool TryBlitUncheckedScaled(in Rect<int> destinationRect, Surface source, in Rect<int> sourceRect, ScaleMode scaleMode)
	{
		unsafe
		{
			fixed (Rect<int>* dstrect = &destinationRect, srcrect = &sourceRect)
			{
				return SDL_BlitSurfaceUncheckedScaled(source is not null ? source.SurfacePointer : null, srcrect, SurfacePointer, dstrect, scaleMode);
			}
		}
	}

	public bool TryClear(float r, float g, float b, float a)
	{
		unsafe
		{
			return SDL_ClearSurface(SurfacePointer, r, g, b, a);
		}
	}

	public bool TryClear(Color<float> color) => TryClear(color.R, color.G, color.B, color.A);



	public bool TryConvert(PixelFormat format, [NotNullWhen(true)] out Surface? result)
	{
		unsafe
		{
			var surface = SDL_ConvertSurface(SurfacePointer, format);

			if (surface is null)
			{
				result = null;
				return false;
			}

			result = new(surface);
			return true;
		}
	}

	public bool TryConvert(PixelFormat format, Palette? palette, ColorSpace colorSpace, Properties? properties, [NotNullWhen(true)] out Surface? result)
	{
		unsafe
		{
			var surface = SDL_ConvertSurfaceAndColorspace(SurfacePointer, format, palette is not null ? palette.Pointer : null, colorSpace, properties?.Id ?? 0);

			if (surface is null)
			{
				result = null;
				return false;
			}

			result = new(surface);
			return true;
		}
	}

	public static bool TryConvertPixels(int width, int height, PixelFormat sourceFormat, ReadOnlyNativeMemory source, int sourcePitch, PixelFormat destinationFormat, NativeMemory destination, int destinationPitch)
	{
		unsafe
		{
			if (!source.IsValid || !destination.IsValid)
			{
				return false;
			}

			var sourceRequired = unchecked((height is > 0 ? (nuint)height : 0) * (sourcePitch is > 0 ? (nuint)sourcePitch : 0));

			if (source.Length < sourceRequired)
			{
				return false;
			}

			var destinationRequired = unchecked((height is > 0 ? (nuint)height : 0) * (destinationPitch is > 0 ? (nuint)destinationPitch : 0));

			if (destination.Length < destinationRequired)
			{
				return false;
			}

			return SDL_ConvertPixels(width, height, sourceFormat, source.RawPointer, sourcePitch, destinationFormat, destination.RawPointer, destinationPitch);
		}
	}

	public static bool TryConvertPixels(int width, int height, PixelFormat sourceFormat, ReadOnlySpan<byte> source, int sourcePitch, PixelFormat destinationFormat, Span<byte> destination, int destinationPitch)
	{
		unsafe
		{
			var sourceRequired = unchecked((height is > 0 ? (nuint)height : 0) * (sourcePitch is > 0 ? (nuint)sourcePitch : 0));

			if (unchecked((nuint)source.Length) < sourceRequired)
			{
				return false;
			}

			var destinationRequired = unchecked((height is > 0 ? (nuint)height : 0) * (destinationPitch is > 0 ? (nuint)destinationPitch : 0));

			if (unchecked((nuint)destination.Length) < destinationRequired)
			{
				return false;
			}

			fixed (byte* src = source, dst = destination)
			{
				return SDL_ConvertPixels(width, height, sourceFormat, src, sourcePitch, destinationFormat, dst, destinationPitch);
			}
		}
	}

	public unsafe static bool TryConvertPixels(int width, int height, PixelFormat sourceFormat, void* source, int sourcePitch, PixelFormat destinationFormat, void* destination, int destinationPitch)
	{
		return SDL_ConvertPixels(width, height, sourceFormat, source, sourcePitch, destinationFormat, destination, destinationPitch);
	}

	public static bool TryConvertPixels(int width, int height, PixelFormat sourceFormat, ColorSpace sourceColorSpace, Properties? sourceProperties, ReadOnlyNativeMemory source, int sourcePitch, PixelFormat destinationFormat, ColorSpace destinationColorSpace, Properties? destinationProperties, NativeMemory destination, int destinationPitch)
	{
		unsafe
		{
			if (!source.IsValid || !destination.IsValid)
			{
				return false;
			}

			var sourceRequired = unchecked((height is > 0 ? (nuint)height : 0) * (sourcePitch is > 0 ? (nuint)sourcePitch : 0));

			if (source.Length < sourceRequired)
			{
				return false;
			}

			var destinationRequired = unchecked((height is > 0 ? (nuint)height : 0) * (destinationPitch is > 0 ? (nuint)destinationPitch : 0));

			if (destination.Length < destinationRequired)
			{
				return false;
			}

			return SDL_ConvertPixelsAndColorspace(width, height, sourceFormat, sourceColorSpace, sourceProperties?.Id ?? 0, source.RawPointer, sourcePitch, destinationFormat, destinationColorSpace, destinationProperties?.Id ?? 0, destination.RawPointer, destinationPitch);
		}
	}

	public static bool TryConvertPixels(int width, int height, PixelFormat sourceFormat, ColorSpace sourceColorSpace, Properties? sourceProperties, ReadOnlySpan<byte> source, int sourcePitch, PixelFormat destinationFormat, ColorSpace destinationColorSpace, Properties? destinationProperties, Span<byte> destination, int destinationPitch)
	{
		unsafe
		{
			var sourceRequired = unchecked((height is > 0 ? (nuint)height : 0) * (sourcePitch is > 0 ? (nuint)sourcePitch : 0));

			if (unchecked((nuint)source.Length) < sourceRequired)
			{
				return false;
			}

			var destinationRequired = unchecked((height is > 0 ? (nuint)height : 0) * (destinationPitch is > 0 ? (nuint)destinationPitch : 0));

			if (unchecked((nuint)destination.Length) < destinationRequired)
			{
				return false;
			}

			fixed (byte* src = source, dst = destination)
			{
				return SDL_ConvertPixelsAndColorspace(width, height, sourceFormat, sourceColorSpace, sourceProperties?.Id ?? 0, src, sourcePitch, destinationFormat, destinationColorSpace, destinationProperties?.Id ?? 0, dst, destinationPitch);
			}
		}
	}

	public unsafe static bool TryConvertPixels(int width, int height, PixelFormat sourceFormat, ColorSpace sourceColorSpace, Properties? sourceProperties, void* source, int sourcePitch, PixelFormat destinationFormat, ColorSpace destinationColorSpace, Properties? destinationProperties, void* destination, int destinationPitch)
	{
		return SDL_ConvertPixelsAndColorspace(width, height, sourceFormat, sourceColorSpace, sourceProperties?.Id ?? 0, source, sourcePitch, destinationFormat, destinationColorSpace, destinationProperties?.Id ?? 0, destination, destinationPitch);
	}

	public bool TryCreateNewPalette([NotNullWhen(true)] out Palette? palette)
	{
		unsafe
		{
			var result = SDL_CreateSurfacePalette(SurfacePointer);

			if (result is null)
			{
				palette = null;
				return false;
			}

			palette = Palette.GetOrCreate(result);
			return true;
		}
	}

	public bool TryFill(in Rect<int> destinationRect, uint pixelValue)
	{
		unsafe
		{
			fixed (Rect<int>* rect = &destinationRect)
			{
				return SDL_FillSurfaceRect(SurfacePointer, rect, pixelValue);
			}
		}
	}

	public bool TryFill(in Rect<int> destinationRect, byte r, byte g, byte b) => TryFill(in destinationRect, MapColor(r, g, b));

	public bool TryFill(in Rect<int> destinationRect, byte r, byte g, byte b, byte a) => TryFill(in destinationRect, MapColor(r, g, b, a));

	public bool TryFill(in Rect<int> destinationRect, Color<byte> color) => TryFill(in destinationRect, MapColor(color));

	public bool TryFill(ReadOnlySpan<Rect<int>> destinationRects, uint pixelValue)
	{
		unsafe
		{
			fixed (Rect<int>* rects = destinationRects)
			{
				return SDL_FillSurfaceRects(SurfacePointer, rects, destinationRects.Length, pixelValue);
			}
		}
	}

	public bool TryFill(ReadOnlySpan<Rect<int>> destinationRects, byte r, byte g, byte b) => TryFill(destinationRects, MapColor(r, g, b));

	public bool TryFill(ReadOnlySpan<Rect<int>> destinationRects, byte r, byte g, byte b, byte a) => TryFill(destinationRects, MapColor(r, g, b, a));

	public bool TryFill(ReadOnlySpan<Rect<int>> destinationRects, Color<byte> color) => TryFill(destinationRects, MapColor(color));

	public bool TryFill(uint pixelValue)
	{
		unsafe
		{
			return SDL_FillSurfaceRect(SurfacePointer, rect: null, pixelValue);
		}
	}

	public bool TryFill(byte r, byte g, byte b) => TryFill(MapColor(r, g, b));

	public bool TryFill(byte r, byte g, byte b, byte a) => TryFill(MapColor(r, g, b, a));

	public bool TryFill(Color<byte> color) => TryFill(MapColor(color));

	public bool TryFlip(FlipMode flipMode)
	{
		unsafe
		{
			return SDL_FlipSurface(SurfacePointer, flipMode);
		}
	}

	public bool TryGetAlphaModulator(out byte alpha)
	{
		unsafe
		{
			byte alphaTmp;

			bool result = SDL_GetSurfaceAlphaMod(SurfacePointer, &alphaTmp);

			alpha = alphaTmp;

			return result;
		}
	}

	public bool TryGetBlendMode(out BlendMode blendMode)
	{
		unsafe
		{
			BlendMode blendModeTmp;

			bool result = SDL_GetSurfaceBlendMode(SurfacePointer, &blendModeTmp);

			blendMode = blendModeTmp;

			return result;
		}
	}

	// TODO: see SetClippingRect and ResetClippingRect; there's not TrySetClippingRect!
	public bool TryGetClippingRect(out Rect<int> rect)
	{
		unsafe
		{
			fixed (Rect<int>* rectPtr = &rect)
			{
				return SDL_GetSurfaceClipRect(SurfacePointer, rectPtr);
			}
		}
	}

	// TODO: see ResetColorKey too!
	public bool TryGetColorKey(out uint keyValue)
	{
		unsafe
		{
			uint keyTmp;

			bool result = SDL_GetSurfaceColorKey(SurfacePointer, &keyTmp);

			keyValue = keyTmp;

			return result;
		}
	}

	// TODO: see ResetColorKey too!
	public bool TryGetColorKey(out byte r, out byte g, out byte b)
	{
		unsafe
		{
			if (TryGetColorKey(out uint keyValue)
				&& SurfacePointer->Format.TryGetPixelFormatDetails(out var details) // SurfacePointer is here non-null because of 'TryGetColorKey'
			)
			{
				TryGetPalette(out var palette); // We don't care for the return value: either 'palette' is non-null if we do have one or it's null if we don't
				details.GetColor(keyValue, palette, out r, out g, out b);
				return true;
			}

			r = default; g = default; b = default;
			return false;
		}
	}

	// TODO: see ResetColorKey too!
	public bool TryGetColorKey(out Color<byte> keyColor)
	{
		if (TryGetColorKey(out var r, out var g, out var b))
		{
			keyColor = Color.From(r, g, b);
			return true;
		}

		keyColor = default;
		return false;
	}

	public bool TryGetColorModulator(out byte r, out byte g, out byte b)
	{
		unsafe
		{
			byte rTmp, gTmp, bTmp;

			bool result = SDL_GetSurfaceColorMod(SurfacePointer, &rTmp, &gTmp, &bTmp);

			r = rTmp; g = gTmp; b = bTmp;
			
			return result;
		}
	}

	public bool TryGetPalette([NotNullWhen(true)] out Palette? palette)
	{
		unsafe
		{
			var palettePtr = SDL_GetSurfacePalette(SurfacePointer);

			if (palettePtr is null)
			{
				palette = null;
				return false;
			}

			palette = Palette.GetOrCreate(palettePtr);
			return true;
		}
	}

	public bool TrySetAlphaModulator(byte alpha)
	{
		unsafe
		{
			return SDL_SetSurfaceAlphaMod(SurfacePointer, alpha);
		}
	}

	public bool TrySetBlendMode(BlendMode blendMode)
	{
		unsafe
		{
			return SDL_SetSurfaceBlendMode(SurfacePointer, blendMode);
		}
	}

	// TODO: see ResetColorKey too!
	public bool TrySetColorKey(uint keyValue)
	{
		unsafe
		{
			return SDL_SetSurfaceColorKey(SurfacePointer, enabled: true, keyValue);
		}
	}

	// TODO: see ResetColorKey too!
	public bool TrySetColorKey(byte r, byte g, byte b) => TrySetColorKey(MapColor(r, g, b));

	// TODO: see ResetColorKey too!
	public bool TrySetColorKey(Color<byte> keyColor) => TrySetColorKey(MapColor(keyColor));

	public bool TrySetColorModulator(byte r, byte g, byte b)
	{
		unsafe
		{
			return SDL_SetSurfaceColorMod(SurfacePointer, r, g, b);
		}
	}
}
