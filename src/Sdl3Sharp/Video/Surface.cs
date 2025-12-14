using Sdl3Sharp.IO;
using Sdl3Sharp.Utilities;
using Sdl3Sharp.Video.Blending;
using Sdl3Sharp.Video.Coloring;
using Sdl3Sharp.Video.Drawing;
using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.Marshalling;

namespace Sdl3Sharp.Video;

public partial class Surface : IDisposable
{
	private interface IUnsafeConstructorDispatch;

	private static readonly ConcurrentDictionary<IntPtr, WeakReference<Surface>> mKnownInstances = [];

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

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private unsafe static byte* Utf8Convert(string? str, out byte* strUtf8) => strUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(str);

	private unsafe SDL_Surface* mSurface;
	private NativeMemory mNativeMemory = default;
	private NativeMemoryPin? mNativeMemoryPin = null;
	private Memory<byte> mMemory = default;
	private MemoryHandle mMemoryHandle = default;
	private Palette? mPalette = null; // Use this to keep the managed Palette alive

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private unsafe Surface(SDL_Surface* surface, IUnsafeConstructorDispatch? _ = default) => mSurface = surface;

	private protected unsafe Surface(SDL_Surface* surface) :
#pragma warning disable IDE0034 // Keep it that way for explicitness sake
		this(surface, default(IUnsafeConstructorDispatch?))
#pragma warning restore IDE0034
	{
		if (mSurface is not null)
		{
			mKnownInstances.AddOrUpdate(unchecked((IntPtr)mSurface), addRef, updateRef, this);
		}

		static WeakReference<Surface> addRef(IntPtr surface, Surface newSurface) => new(newSurface);

		static WeakReference<Surface> updateRef(IntPtr surface, WeakReference<Surface> previousSurfaceRef, Surface newSurface)
		{
			if (previousSurfaceRef.TryGetTarget(out var previousSurface))
			{
#pragma warning disable IDE0079
#pragma warning disable CA1816
				GC.SuppressFinalize(previousSurface);
#pragma warning restore CA1816
#pragma warning restore IDE0079
				previousSurface.Dispose(disposing: true, forget: true);
			}

			previousSurfaceRef.SetTarget(newSurface);

			return previousSurfaceRef;
		}
	}

	/// <exception cref="SdlException">The <see cref="Surface"/> could not be created (check <see cref="Error.TryGet(out string?)"/> for more information)</exception>
	private unsafe Surface(int width, int height, PixelFormat format, IUnsafeConstructorDispatch? _ = default) :
		this(SDL_CreateSurface(width, height, format))
	{
		if (mSurface is null)
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
		if (mSurface is null)
		{
			failCouldNotCreateSurface();
		}

		[DoesNotReturn]
		static void failCouldNotCreateSurface() => throw new SdlException($"Could not create the {nameof(Surface)}");
	}

#if SDL3_4_0_OR_GREATER
	/// <exception cref="SdlException">The <see cref="Surface"/> could not be created (check <see cref="Error.TryGet(out string?)"/> for more information)</exception>
	private unsafe Surface(string file, IUnsafeConstructorDispatch? _ = default) :
		this(SDL_LoadSurface(Utf8Convert(file, out var fileUtf8)))
	{
		try
		{
			if (mSurface is null)
			{
				failCouldNotCreateSurface();
			}
		}
		finally
		{
			Utf8StringMarshaller.Free(fileUtf8);
		}

		[DoesNotReturn]
		static void failCouldNotCreateSurface() => throw new SdlException($"Could not create the {nameof(Surface)}");
	}

	// /// <inheritdoc cref="Surface(string, IUnsafeConstructorDispatch?)"/>
	public Surface(string file) :
#pragma warning disable IDE0034 // Keep it that way for explicitness sake
		this(file, default(IUnsafeConstructorDispatch?))
#pragma warning restore IDE0034
	{ }
#endif

#if SDL3_4_0_OR_GREATER
	/// <exception cref="SdlException">The <see cref="Surface"/> could not be created (check <see cref="Error.TryGet(out string?)"/> for more information)</exception>
	private unsafe Surface(Stream source, bool closeAfterwards, IUnsafeConstructorDispatch? _ = default) :
		this(SDL_LoadSurface_IO(source is not null ? source.Pointer : null, closeAfterwards))
	{
		try
		{
			if (mSurface is null)
			{
				failCouldNotCreateSurface();
			}
		}
		finally
		{
			if (closeAfterwards)
			{
				source?.Dispose(close: false /* SDL_LoadSurface_IO already closed the stream */);
			}
		}

		[DoesNotReturn]
		static void failCouldNotCreateSurface() => throw new SdlException($"Could not create the {nameof(Surface)}");
	}

	// /// <inheritdoc cref="Surface.Surface(Stream, bool, IUnsafeConstructorDispatch?)"/>
	public Surface(Stream source, bool closeAfterwards = false) :
#pragma warning disable IDE0034 // Keep it that way for explicitness sake
		this(source, closeAfterwards, default(IUnsafeConstructorDispatch?))
#pragma warning restore IDE0034
	{ }
#endif

	~Surface() => Dispose(disposing: false, forget: true);

	public SurfaceFlags Flags
	{
		get
		{
			unsafe
			{
				return mSurface is var surface
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
				return mSurface is var surface
					&& surface is not null
						? surface->Format
						: default;
			}
		}
	}

	public bool HasAlternateImages
	{
		get
		{
			unsafe
			{
				return SDL_SurfaceHasAlternateImages(mSurface);
			}
		}
	}

	public bool HasColorKey
	{
		get
		{
			unsafe
			{
				return SDL_SurfaceHasColorKey(mSurface);
			}
		}
	}

	public float HdrHeadroom
	{
		get => Properties?.TryGetFloatValue(PropertyNames.HdrHeadroomFloat, out var hdrHeadroom) is true
			? hdrHeadroom
			: default;

		set => Properties?.TrySetFloatValue(PropertyNames.HdrHeadroomFloat, value);
	}

	public int Height
	{
		get
		{
			unsafe
			{
				return mSurface is var surface
					&& surface is not null
						? surface->H
						: default;
			}
		}
	}

	public long HotspotX
	{
		get => Properties?.TryGetNumberValue(PropertyNames.HotspotXNumber, out var hotspotX) is true
			? hotspotX
			: default;

		set => Properties?.TrySetNumberValue(PropertyNames.HotspotXNumber, value);
	}

	public long HotspotY
	{
		get => Properties?.TryGetNumberValue(PropertyNames.HotspotYNumber, out var hotspotY) is true
			? hotspotY
			: default;

		set => Properties?.TrySetNumberValue(PropertyNames.HotspotYNumber, value);
	}

	public Surface[] Images
	{
		get
		{
			unsafe
			{
				if (mSurface is var surface
					&& surface is null)
				{
					return [];
				}

				Unsafe.SkipInit(out int count);

				var images = SDL_GetSurfaceImages(mSurface, &count);

				var result = GC.AllocateUninitializedArray<Surface>(count);

				foreach (ref var image in result.AsSpan())
				{
					TryGetOrCreate(*images++, out image!);
				}

				Utilities.NativeMemory.SDL_free(images);

				return result;
			}
		}
	}

	public bool IsLocked
	{
		get
		{
			unsafe
			{
#pragma warning disable IDE0075 // It's more readable that way
				return mSurface is var surface
					&& surface is not null
					? (surface->Flags & SurfaceFlags.Locked) is SurfaceFlags.Locked
					: default;
#pragma warning restore IDE0075
			}
		}
	}

	public bool IsPreallocated
	{
		get
		{
			unsafe
			{
#pragma warning disable IDE0075 // It's more readable that way
				return mSurface is var surface
					&& surface is not null
					? (surface->Flags & SurfaceFlags.PreAllocated) is SurfaceFlags.PreAllocated
					: default;
#pragma warning restore IDE0075
			}
		}
	}

	public bool IsRle
	{
		get
		{
			unsafe
			{
				return SDL_SurfaceHasRLE(mSurface);
			}
		}

		// TODO: doc: silent error if pixel format is FourCC
		set
		{
			unsafe
			{
				SDL_SetSurfaceRLE(mSurface, value);
			}
		}
	}

	public bool IsSimdAligned
	{
		get
		{
			unsafe
			{
#pragma warning disable IDE0075 // It's more readable that way
				return mSurface is var surface
					&& surface is not null
					? (surface->Flags & SurfaceFlags.SimdAligned) is SurfaceFlags.SimdAligned
					: default;
#pragma warning restore IDE0075
			}
		}
	}

	public bool MustLock
	{
		get
		{
			unsafe
			{
#pragma warning disable IDE0075 // It's more readable that way
				return mSurface is var surface
					&& surface is not null
					? (surface->Flags & SurfaceFlags.LockNeeded) is SurfaceFlags.LockNeeded
					: default;
#pragma warning restore IDE0075
			}
		}
	}

	public int Pitch
	{
		get
		{
			unsafe
			{
				return mSurface is var surface
					&& surface is not null
					? surface->Pitch
					: default;
			}
		}
	}

	internal unsafe SDL_Surface* Pointer { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mSurface; }

	public Properties? Properties
	{
		get
		{
			unsafe
			{
				return SDL_GetSurfaceProperties(mSurface) switch
				{
					0 => null,
					var id => Properties.GetOrCreate(sdl: null, id)
				};
			}
		}
	}

#if SDL3_4_0_OR_GREATER
	public float Rotation
	{
		get => Properties?.TryGetFloatValue(PropertyNames.RotationFloat, out var rotation) is true
			? rotation
			: default;

		set => Properties?.TrySetFloatValue(PropertyNames.RotationFloat, value);
	}
#endif

	public float SdrWhitePoint
	{
		get => Properties?.TryGetFloatValue(PropertyNames.SdrWhitePointFloat, out var sdrWhitePoint) is true
			? sdrWhitePoint
			: default;

		set => Properties?.TrySetFloatValue(PropertyNames.SdrWhitePointFloat, value);
	}

	public string TonemapOperator
	{
		get => Properties?.TryGetStringValue(PropertyNames.TonemapOperatorString, out var tonemapOperator) is true
			&& tonemapOperator is not null
			? tonemapOperator
			: string.Empty;

		set => Properties?.TrySetStringValue(PropertyNames.TonemapOperatorString, value);
	}

	public Utilities.NativeMemory UnsafePixels
	{
		get
		{
			unsafe
			{
				return mSurface is var surface
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
				return mSurface is var surface
					&& surface is not null
						? surface->W
						: default;
			}
		}
	}

	public void ClearAlternateImages()
	{
		unsafe
		{
			SDL_RemoveSurfaceAlternateImages(mSurface);

			// TODO: do we need to consider implicit surface destruction on the SDL-side?
		}
	}

	public void Dispose()
	{
		GC.SuppressFinalize(this);
		Dispose(disposing: true, forget: true);
	}

	protected virtual void Dispose(bool disposing, bool forget)
	{
		unsafe
		{
			if (mSurface is not null)
			{
				if (forget)
				{
					mKnownInstances.TryRemove(unchecked((IntPtr)mSurface), out _);
				}

				SDL_DestroySurface(mSurface);
				mSurface = null;
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
			return SDL_MapSurfaceRGB(mSurface, r, g, b);
		}
	}

	public uint MapColor(byte r, byte g, byte b, byte a)
	{
		unsafe
		{
			return SDL_MapSurfaceRGBA(mSurface, r, g, b, a);
		}
	}

	public uint MapColor(Color<byte> color) => MapColor(color.R, color.G, color.B, color.A);

	public void ResetClippingRect()
	{
		unsafe
		{
			SDL_SetSurfaceClipRect(mSurface, rect: null);
		}
	}

	public void ResetColorKey()
	{
		unsafe
		{
			SDL_SetSurfaceColorKey(mSurface, enabled: false, key: 0);
		}
	}

	public bool SetClippingRect(in Rect<int> rect)
	{
		unsafe
		{
			fixed (Rect<int>* rectPtr = &rect)
			{
				return SDL_SetSurfaceClipRect(mSurface, rectPtr);
			}
		}
	}

	public bool TryAddAlternateImage(Surface image)
	{
		unsafe
		{
			return SDL_AddSurfaceAlternateImage(mSurface, image is not null ? image.mSurface : null);

			// TODO: what about SDL_DestroySurface on the image?
		}
	}

	public bool TryBlit(in Rect<int> destinationRect, Surface source, in Rect<int> sourceRect)
	{
		unsafe
		{
			fixed (Rect<int>* dstrect = &destinationRect, srcrect = &sourceRect)
			{
				return SDL_BlitSurface(source is not null ? source.mSurface : null, srcrect, mSurface, dstrect);
			}
		}
	}

	public bool TryBlit(in Rect<int> destinationRect, Surface source)
	{
		unsafe
		{
			fixed (Rect<int>* dstrect = &destinationRect)
			{
				return SDL_BlitSurface(source is not null ? source.mSurface : null, srcrect: null, mSurface, dstrect);
			}
		}
	}

	public bool TryBlit(Surface source, in Rect<int> sourceRect)
	{
		unsafe
		{
			fixed (Rect<int>* srcrect = &sourceRect)
			{
				return SDL_BlitSurface(source is not null ? source.mSurface : null, srcrect, mSurface, dstrect: null);
			}
		}
	}

	public bool TryBlit(Surface source)
	{
		unsafe
		{
			return SDL_BlitSurface(source is not null ? source.mSurface : null, srcrect: null, mSurface, dstrect: null);
		}
	}

	public bool TryBlit9Grid(in Rect<int> destinationRect, Surface source, in Rect<int> sourceRect, int leftWidth, int rightWidth, int topHeight, int bottomHeight, float scale, ScaleMode scaleMode)
	{
		unsafe
		{
			fixed (Rect<int>* dstrect = &destinationRect, srcrect = &sourceRect)
			{
				return SDL_BlitSurface9Grid(source is not null ? source.mSurface : null, srcrect, leftWidth, rightWidth, topHeight, bottomHeight, scale, scaleMode, mSurface, dstrect);
			}
		}
	}

	public bool TryBlit9Grid(in Rect<int> destinationRect, Surface source, int leftWidth, int rightWidth, int topHeight, int bottomHeight, float scale, ScaleMode scaleMode)
	{
		unsafe
		{
			fixed (Rect<int>* dstrect = &destinationRect)
			{
				return SDL_BlitSurface9Grid(source is not null ? source.mSurface : null, srcrect: null, leftWidth, rightWidth, topHeight, bottomHeight, scale, scaleMode, mSurface, dstrect);
			}
		}
	}

	public bool TryBlit9Grid(Surface source, in Rect<int> sourceRect, int leftWidth, int rightWidth, int topHeight, int bottomHeight, float scale, ScaleMode scaleMode)
	{
		unsafe
		{
			fixed (Rect<int>* srcrect = &sourceRect)
			{
				return SDL_BlitSurface9Grid(source is not null ? source.mSurface : null, srcrect, leftWidth, rightWidth, topHeight, bottomHeight, scale, scaleMode, mSurface, dstrect: null);
			}
		}
	}

	public bool TryBlit9Grid(Surface source, int leftWidth, int rightWidth, int topHeight, int bottomHeight, float scale, ScaleMode scaleMode)
	{
		unsafe
		{
			return SDL_BlitSurface9Grid(source is not null ? source.mSurface : null, srcrect: null, leftWidth, rightWidth, topHeight, bottomHeight, scale, scaleMode, mSurface, dstrect: null);
		}
	}

	public bool TryBlitScaled(in Rect<int> destinationRect, Surface source, in Rect<int> sourceRect, ScaleMode scaleMode)
	{
		unsafe
		{
			fixed (Rect<int>* dstrect = &destinationRect, srcrect = &sourceRect)
			{
				return SDL_BlitSurfaceScaled(source is not null ? source.mSurface : null, srcrect, mSurface, dstrect, scaleMode);
			}
		}
	}

	public bool TryBlitScaled(in Rect<int> destinationRect, Surface source, ScaleMode scaleMode)
	{
		unsafe
		{
			fixed (Rect<int>* dstrect = &destinationRect)
			{
				return SDL_BlitSurfaceScaled(source is not null ? source.mSurface : null, srcrect: null, mSurface, dstrect, scaleMode);
			}
		}
	}

	public bool TryBlitScaled(Surface source, in Rect<int> sourceRect, ScaleMode scaleMode)
	{
		unsafe
		{
			fixed (Rect<int>* srcrect = &sourceRect)
			{
				return SDL_BlitSurfaceScaled(source is not null ? source.mSurface : null, srcrect, mSurface, dstrect: null, scaleMode);
			}
		}
	}

	public bool TryBlitScaled(Surface source, ScaleMode scaleMode)
	{
		unsafe
		{
			return SDL_BlitSurfaceScaled(source is not null ? source.mSurface : null, srcrect: null, mSurface, dstrect: null, scaleMode);
		}
	}

#if SDL3_4_0_GREATER
	public bool TryBlitStretched(in Rect<int> destinationRect, Surface source, in Rect<int> sourceRect, ScaleMode scaleMode)
	{
		unsafe
		{
			fixed (Rect<int>* dstrect = &destinationRect, srcrect = &sourceRect)
			{
				return SDL_StretchSurface(source is not null ? source.mSurface : null, srcrect, mSurface, dstrect, scaleMode);
			}
		}
	}

	public bool TryBlitStretched(in Rect<int> destinationRect, Surface source, ScaleMode scaleMode)
	{
		unsafe
		{
			fixed (Rect<int>* dstrect = &destinationRect)
			{
				return SDL_StretchSurface(source is not null ? source.mSurface : null, srcrect: null, mSurface, dstrect, scaleMode);
			}
		}
	}

	public bool TryBlitStretched(Surface source, in Rect<int> sourceRect, ScaleMode scaleMode)
	{
		unsafe
		{
			fixed (Rect<int>* srcrect = &sourceRect)
			{
				return SDL_StretchSurface(source is not null ? source.mSurface : null, srcrect, mSurface, dstrect: null, scaleMode);
			}
		}
	}

	public bool TryBlitStretched(Surface source, ScaleMode scaleMode)
	{
		unsafe
		{
			return SDL_StretchSurface(source is not null ? source.mSurface : null, srcrect: null, mSurface, dstrect: null, scaleMode);
		}
	}
#endif

	public bool TryBlitTiled(in Rect<int> destinationRect, Surface source, in Rect<int> sourceRect)
	{
		unsafe
		{
			fixed (Rect<int>* dstrect = &destinationRect, srcrect = &sourceRect)
			{
				return SDL_BlitSurfaceTiled(source is not null ? source.mSurface : null, srcrect, mSurface, dstrect);
			}
		}
	}

	public bool TryBlitTiled(in Rect<int> destinationRect, Surface source)
	{
		unsafe
		{
			fixed (Rect<int>* dstrect = &destinationRect)
			{
				return SDL_BlitSurfaceTiled(source is not null ? source.mSurface : null, srcrect: null, mSurface, dstrect);
			}
		}
	}

	public bool TryBlitTiled(Surface source, in Rect<int> sourceRect)
	{
		unsafe
		{
			fixed (Rect<int>* srcrect = &sourceRect)
			{
				return SDL_BlitSurfaceTiled(source is not null ? source.mSurface : null, srcrect, mSurface, dstrect: null);
			}
		}
	}
	public bool TryBlitTiled(Surface source)
	{
		unsafe
		{
			return SDL_BlitSurfaceTiled(source is not null ? source.mSurface : null, srcrect: null, mSurface, dstrect: null);
		}
	}

	public bool TryBlitTiledWithScale(in Rect<int> destinationRect, Surface source, in Rect<int> sourceRect, float scale, ScaleMode scaleMode)
	{
		unsafe
		{
			fixed (Rect<int>* dstrect = &destinationRect, srcrect = &sourceRect)
			{
				return SDL_BlitSurfaceTiledWithScale(source is not null ? source.mSurface : null, srcrect, scale, scaleMode, mSurface, dstrect);
			}
		}
	}

	public bool TryBlitTiledWithScale(in Rect<int> destinationRect, Surface source, float scale, ScaleMode scaleMode)
	{
		unsafe
		{
			fixed (Rect<int>* dstrect = &destinationRect)
			{
				return SDL_BlitSurfaceTiledWithScale(source is not null ? source.mSurface : null, srcrect: null, scale, scaleMode, mSurface, dstrect);
			}
		}
	}

	public bool TryBlitTiledWithScale(Surface source, in Rect<int> sourceRect, float scale, ScaleMode scaleMode)
	{
		unsafe
		{
			fixed (Rect<int>* srcrect = &sourceRect)
			{
				return SDL_BlitSurfaceTiledWithScale(source is not null ? source.mSurface : null, srcrect, scale, scaleMode, mSurface, dstrect: null);
			}
		}
	}

	public bool TryBlitTiledWithScale(Surface source, float scale, ScaleMode scaleMode)
	{
		unsafe
		{
			return SDL_BlitSurfaceTiledWithScale(source is not null ? source.mSurface : null, srcrect: null, scale, scaleMode, mSurface, dstrect: null);
		}
	}

	public bool TryBlitUnchecked(in Rect<int> destinationRect, Surface source, in Rect<int> sourceRect)
	{
		unsafe
		{
			fixed (Rect<int>* dstrect = &destinationRect, srcrect = &sourceRect)
			{
				return SDL_BlitSurfaceUnchecked(source is not null ? source.mSurface : null, srcrect, mSurface, dstrect);
			}
		}
	}

	public bool TryBlitUncheckedScaled(in Rect<int> destinationRect, Surface source, in Rect<int> sourceRect, ScaleMode scaleMode)
	{
		unsafe
		{
			fixed (Rect<int>* dstrect = &destinationRect, srcrect = &sourceRect)
			{
				return SDL_BlitSurfaceUncheckedScaled(source is not null ? source.mSurface : null, srcrect, mSurface, dstrect, scaleMode);
			}
		}
	}

	public bool TryClear(float r, float g, float b, float a)
	{
		unsafe
		{
			return SDL_ClearSurface(mSurface, r, g, b, a);
		}
	}

	public bool TryClear(Color<float> color) => TryClear(color.R, color.G, color.B, color.A);

	public bool TryConvert(PixelFormat format, [NotNullWhen(true)] out Surface? result)
	{
		unsafe
		{
			var surface = SDL_ConvertSurface(mSurface, format);

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
			var surface = SDL_ConvertSurfaceAndColorspace(mSurface, format, palette is not null ? palette.Pointer : null, colorSpace, properties?.Id ?? 0);

			if (surface is null)
			{
				result = null;
				return false;
			}

			result = new(surface);
			return true;
		}
	}

	public static bool TryConvert(int width, int height, PixelFormat sourceFormat, ReadOnlyNativeMemory source, int sourcePitch, PixelFormat destinationFormat, NativeMemory destination, int destinationPitch)
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

	public static bool TryConvert(int width, int height, PixelFormat sourceFormat, ReadOnlySpan<byte> source, int sourcePitch, PixelFormat destinationFormat, Span<byte> destination, int destinationPitch)
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

	public unsafe static bool TryConvert(int width, int height, PixelFormat sourceFormat, void* source, int sourcePitch, PixelFormat destinationFormat, void* destination, int destinationPitch)
	{
		return SDL_ConvertPixels(width, height, sourceFormat, source, sourcePitch, destinationFormat, destination, destinationPitch);
	}

	public static bool TryConvert(int width, int height, PixelFormat sourceFormat, ColorSpace sourceColorSpace, Properties? sourceProperties, ReadOnlyNativeMemory source, int sourcePitch, PixelFormat destinationFormat, ColorSpace destinationColorSpace, Properties? destinationProperties, NativeMemory destination, int destinationPitch)
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

	public static bool TryConvert(int width, int height, PixelFormat sourceFormat, ColorSpace sourceColorSpace, Properties? sourceProperties, ReadOnlySpan<byte> source, int sourcePitch, PixelFormat destinationFormat, ColorSpace destinationColorSpace, Properties? destinationProperties, Span<byte> destination, int destinationPitch)
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

	public unsafe static bool TryConvert(int width, int height, PixelFormat sourceFormat, ColorSpace sourceColorSpace, Properties? sourceProperties, void* source, int sourcePitch, PixelFormat destinationFormat, ColorSpace destinationColorSpace, Properties? destinationProperties, void* destination, int destinationPitch)
	{
		return SDL_ConvertPixelsAndColorspace(width, height, sourceFormat, sourceColorSpace, sourceProperties?.Id ?? 0, source, sourcePitch, destinationFormat, destinationColorSpace, destinationProperties?.Id ?? 0, destination, destinationPitch);
	}

	public bool TryCreateNewPalette([NotNullWhen(true)] out Palette? palette)
	{
		unsafe
		{
			var result = SDL_CreateSurfacePalette(mSurface);

			if (result is null)
			{
				palette = null;
				return false;
			}

			mPalette = palette =  new Palette(result); // we actually want to override any existing managed Palette here
			return true;
		}
	}

	public bool TryDuplicate([NotNullWhen(true)] out Surface? duplicate)
	{
		unsafe
		{
			var duplicatePtr = SDL_DuplicateSurface(mSurface);

			if (duplicatePtr is null)
			{
				duplicate = null;

				return false;
			}

			duplicate = new(duplicatePtr)
			{
				mNativeMemory = mNativeMemory,
				mNativeMemoryPin = mNativeMemoryPin is not null ? mNativeMemory.Pin() : null,
				mMemory = mMemory,
				mMemoryHandle = mMemoryHandle.Pointer is not null ? mMemory.Pin() : default
			};

			return true;
		}
	}

	public bool TryFill(in Rect<int> destinationRect, uint pixelValue)
	{
		unsafe
		{
			fixed (Rect<int>* rect = &destinationRect)
			{
				return SDL_FillSurfaceRect(mSurface, rect, pixelValue);
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
				return SDL_FillSurfaceRects(mSurface, rects, destinationRects.Length, pixelValue);
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
			return SDL_FillSurfaceRect(mSurface, rect: null, pixelValue);
		}
	}

	public bool TryFill(byte r, byte g, byte b) => TryFill(MapColor(r, g, b));

	public bool TryFill(byte r, byte g, byte b, byte a) => TryFill(MapColor(r, g, b, a));

	public bool TryFill(Color<byte> color) => TryFill(MapColor(color));

	public bool TryFlip(FlipMode flipMode)
	{
		unsafe
		{
			return SDL_FlipSurface(mSurface, flipMode);
		}
	}

	public bool TryGetAlphaModulator(out byte alpha)
	{
		unsafe
		{
			Unsafe.SkipInit(out byte alphaTmp);

			bool result = SDL_GetSurfaceAlphaMod(mSurface, &alphaTmp);

			alpha = alphaTmp;

			return result;
		}
	}

	public bool TryGetBlendMode(out BlendMode blendMode)
	{
		unsafe
		{
			Unsafe.SkipInit(out BlendMode blendModeTmp);

			bool result = SDL_GetSurfaceBlendMode(mSurface, &blendModeTmp);

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
				return SDL_GetSurfaceClipRect(mSurface, rectPtr);
			}
		}
	}

	// TODO: see ResetColorKey too!
	public bool TryGetColorKey(out uint keyValue)
	{
		unsafe
		{
			Unsafe.SkipInit(out uint keyTmp);

			bool result = SDL_GetSurfaceColorKey(mSurface, &keyTmp);

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
				&& mSurface->Format.TryGetPixelFormatDetails(out var details) // SurfacePointer is here non-null because of 'TryGetColorKey'
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
			Unsafe.SkipInit(out byte rTmp);
			Unsafe.SkipInit(out byte gTmp);
			Unsafe.SkipInit(out byte bTmp);

			bool result = SDL_GetSurfaceColorMod(mSurface, &rTmp, &gTmp, &bTmp);

			r = rTmp;
			g = gTmp;
			b = bTmp;
			
			return result;
		}
	}

	public bool TryGetColorSpace(out ColorSpace colorSpace)
	{
		unsafe
		{
			colorSpace = SDL_GetSurfaceColorspace(mSurface);

			return colorSpace is not ColorSpace.Unknown;
		}
	}

	internal unsafe static bool TryGetOrCreate(SDL_Surface* surface, [NotNullWhen(true)] out Surface? result)
	{
		if (surface is null)
		{
			result = null;
			return false;
		}

		var surfaceRef = mKnownInstances.GetOrAdd(unchecked((IntPtr)surface), createRef);

		if (!surfaceRef.TryGetTarget(out result))
		{
			surfaceRef.SetTarget(result = create(surface));
		}

		return true;

		static WeakReference<Surface> createRef(IntPtr surface) => new(create(unchecked((SDL_Surface*)surface)));

		static Surface create(SDL_Surface* surface) => new(surface,
#pragma warning disable IDE0034 // Keep it that way for explicitness sake
			default(IUnsafeConstructorDispatch)
#pragma warning restore IDE0034
		);
	}

	public bool TryGetPalette([NotNullWhen(true)] out Palette? palette)
	{
		unsafe
		{
			var palettePtr = SDL_GetSurfacePalette(mSurface);

			bool result = Palette.TryGetOrCreate(palettePtr, out palette);

			if (mPalette is null || mPalette.Pointer != palettePtr)
			{
				mPalette = palette;
			}

			return result;
		}
	}

	public static bool TryLoadBmp(string file, [NotNullWhen(true)] out Surface? surface)
	{
		unsafe
		{
			var fileUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(file);

			try
			{
				var surfacePtr = SDL_LoadBMP(fileUtf8);

				if (surfacePtr is null)
				{
					surface = null;
					return false;
				}

				surface = new(surfacePtr);
				return true;
			}
			finally
			{
				Utf8StringMarshaller.Free(fileUtf8);
			}
		}
	}

	public static bool TryLoadBmp(Stream source, [NotNullWhen(true)] out Surface? surface, bool closeAfterwards = false)
	{
		unsafe
		{
			try
			{
				var surfacePtr = SDL_LoadBMP_IO(source is not null ? source.Pointer : null, closeAfterwards);

				if (surfacePtr is null)
				{
					surface = null;
					return false;
				}

				surface = new(surfacePtr);
				return true;
			}
			finally
			{
				if (closeAfterwards)
				{
					source?.Dispose(close: false /* SDL_LoadBMP_IO already closed the stream */);
				}
			}
		}
	}

#if SDL3_4_0_OR_GREATER
	public static bool TryLoadPng(string file, [NotNullWhen(true)] out Surface? surface)
	{
		unsafe
		{
			var fileUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(file);

			try
			{
				var surfacePtr = SDL_LoadPNG(fileUtf8);

				if (surfacePtr is null)
				{
					surface = null;
					return false;
				}

				surface = new(surfacePtr);
				return true;
			}
			finally
			{
				Utf8StringMarshaller.Free(fileUtf8);
			}
		}
	}
#endif

#if SDL3_4_0_OR_GREATER
	public static bool TryLoadPng(Stream source, [NotNullWhen(true)] out Surface? surface, bool closeAfterwards = false)
	{
		unsafe
		{
			try
			{
				var surfacePtr = SDL_LoadPNG_IO(source is not null ? source.Pointer : null, closeAfterwards);

				if (surfacePtr is null)
				{
					surface = null;
					return false;
				}

				surface = new(surfacePtr);
				return true;
			}
			finally
			{
				if (closeAfterwards)
				{
					source?.Dispose(close: false /* SDL_LoadPNG_IO already closed the stream */);
				}
			}
		}
	}
#endif

	public bool TryLock([NotNullWhen(true)] out SurfacePixelMemoryManager? pixelManager) => SurfacePixelMemoryManager.TryCreate(this, out pixelManager);

	public bool TryPremultiplyAlpha(bool linear)
	{
		unsafe
		{
			return SDL_PremultiplySurfaceAlpha(mSurface, linear);
		}
	}

	public static bool TryPremultiplyAlpha(int width, int height, PixelFormat sourceFormat, ReadOnlyNativeMemory source, int sourcePitch, PixelFormat destinationFormat, NativeMemory destination, int destinationPitch, bool linear)
	{
		unsafe
		{
			if (source.IsValid || destination.IsValid)
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

			return SDL_PremultiplyAlpha(width, height, sourceFormat, source.RawPointer, sourcePitch, destinationFormat, destination.RawPointer, destinationPitch, linear);
		}
	}

	public static bool TryPremultiplyAlpha(int width, int height, PixelFormat sourceFormat, ReadOnlySpan<byte> source, int sourcePitch, PixelFormat destinationFormat, Span<byte> destination, int destinationPitch, bool linear)
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
				return SDL_PremultiplyAlpha(width, height, sourceFormat, src, sourcePitch, destinationFormat, dst, destinationPitch, linear);
			}
		}
	}

	public unsafe static bool TryPremultiplyAlpha(int width, int height, PixelFormat sourceFormat, void* source, int sourcePitch, PixelFormat destinationFormat, void* destination, int destinationPitch, bool linear)
	{
		return SDL_PremultiplyAlpha(width, height, sourceFormat, source, sourcePitch, destinationFormat, destination, destinationPitch, linear);
	}

	[OverloadResolutionPriority(1)] // To choose TryReadPixel(int, int, out byte, out byte, out byte, out byte) over TryReadPixel(int, int, out float, out float, out float, out float) when using 'out var'
	public bool TryReadPixel(int x, int y, out byte r, out byte g, out byte b, out byte a)
	{
		unsafe
		{
			Unsafe.SkipInit(out byte rTmp);
			Unsafe.SkipInit(out byte gTmp);
			Unsafe.SkipInit(out byte bTmp);
			Unsafe.SkipInit(out byte aTmp);

			bool result = SDL_ReadSurfacePixel(mSurface, x, y, &rTmp, &gTmp, &bTmp, &aTmp);

			r = rTmp;
			g = gTmp;
			b = bTmp;
			a = aTmp;

			return result;
		}
	}

	[OverloadResolutionPriority(1)] // To choose TryReadPixel(int, int, out Color<byte>) over TryReadPixel(int, int, out Color<float>) when using 'out var'
	public bool TryReadPixel(int x, int y, out Color<byte> color)
	{
		if (TryReadPixel(x, y, out byte r, out byte g, out byte b, out byte a))
		{
			color = new(r, g, b, a);
			return true;
		}

		color = default;
		return false;
	}

	public bool TryReadPixel(int x, int y, out float r, out float g, out float b, out float a)
	{
		unsafe
		{
			Unsafe.SkipInit(out float rTmp);
			Unsafe.SkipInit(out float gTmp);
			Unsafe.SkipInit(out float bTmp);
			Unsafe.SkipInit(out float aTmp);

			bool result = SDL_ReadSurfacePixelFloat(mSurface, x, y, &rTmp, &gTmp, &bTmp, &aTmp);

			r = rTmp;
			g = gTmp;
			b = bTmp;
			a = aTmp;

			return result;
		}
	}

	public bool TryReadPixel(int x, int y, out Color<float> color)
	{
		if (TryReadPixel(x, y, out float r, out float g, out float b, out float a))
		{
			color = new(r, g, b, a);
			return true;
		}

		color = default;
		return false;
	}

#if SDL3_4_0_OR_GREATER
	public bool TryRotate(float angle, [NotNullWhen(true)] out Surface? rotatedSurface)
	{
		unsafe
		{
			var rotatedPtr = SDL_RotateSurface(mSurface, angle);

			if (rotatedPtr is null)
			{
				rotatedSurface = null;
				return false;
			}

			rotatedSurface = new(rotatedPtr);
			return true;
		}
	}
#endif

	public bool TrySaveBmp(string file)
	{
		unsafe
		{
			var fileUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(file);
			try
			{
				return SDL_SaveBMP(mSurface, fileUtf8);
			}
			finally
			{
				Utf8StringMarshaller.Free(fileUtf8);
			}
		}
	}

	public bool TrySaveBmp(Stream destination, bool closeAfterwards = false)
	{
		unsafe
		{
			try
			{
				return SDL_SaveBMP_IO(mSurface, destination is not null ? destination.Pointer : null, closeAfterwards);
			}
			finally
			{
				if (closeAfterwards)
				{
					destination?.Dispose(close: false /* SDL_SaveBMP_IO already closed the stream */);
				}
			}
		}
	}

#if SDL3_4_0_OR_GREATER
	public bool TrySavePng(string file)
	{
		unsafe
		{
			var fileUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(file);
			try
			{
				return SDL_SavePNG(mSurface, fileUtf8);
			}
			finally
			{
				Utf8StringMarshaller.Free(fileUtf8);
			}
		}
	}
#endif

#if SDL3_4_0_OR_GREATER
	public bool TrySavePng(Stream destination, bool closeAfterwards = false)
	{
		unsafe
		{
			try
			{
				return SDL_SavePNG_IO(mSurface, destination is not null ? destination.Pointer : null, closeAfterwards);
			}
			finally
			{
				if (closeAfterwards)
				{
					destination?.Dispose(close: false /* SDL_SavePNG_IO already closed the stream */);
				}
			}
		}
	}
#endif

	public bool TryScale(int width, int height, ScaleMode scaleMode, [NotNullWhen(true)] out Surface? scaledSurface)
	{
		unsafe
		{
			var scaledPtr = SDL_ScaleSurface(mSurface, width, height, scaleMode);

			if (scaledPtr is null)
			{
				scaledSurface = null;
				return false;
			}

			scaledSurface = new(scaledPtr);
			return true;
		}
	}

	public bool TrySetAlphaModulator(byte alpha)
	{
		unsafe
		{
			return SDL_SetSurfaceAlphaMod(mSurface, alpha);
		}
	}

	public bool TrySetBlendMode(BlendMode blendMode)
	{
		unsafe
		{
			return SDL_SetSurfaceBlendMode(mSurface, blendMode);
		}
	}

	// TODO: see ResetColorKey too!
	public bool TrySetColorKey(uint keyValue)
	{
		unsafe
		{
			return SDL_SetSurfaceColorKey(mSurface, enabled: true, keyValue);
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
			return SDL_SetSurfaceColorMod(mSurface, r, g, b);
		}
	}

	public bool TrySetColorSpace(ColorSpace colorSpace)
	{
		unsafe
		{
			return SDL_SetSurfaceColorspace(mSurface, colorSpace);
		}
	}

	public bool TrySetPalette(Palette palette)
	{
		unsafe
		{
			if (SDL_SetSurfacePalette(mSurface, palette is not null ? palette.Pointer : null))
			{
				mPalette = palette;
				return true;
			}

			return false;
		}
	}

	public bool TryUnsafeLock()
	{
		unsafe
		{
			return SDL_LockSurface(mSurface);
		}
	}

	[OverloadResolutionPriority(1)] // Actually, that shouldn't be an issue since C# doesn't now byte literals aside from the ambiguous 'default', but we do that for symmetry with 'TryReadPixel'
	public bool TryWritePixel(int x, int y, byte r, byte g, byte b, byte a)
	{
		unsafe
		{
			return SDL_WriteSurfacePixel(mSurface, x, y, r, g, b, a);
		}
	}

	[OverloadResolutionPriority(1)] // Actually, that shouldn't be an issue since C# doesn't now literal of custom struct types aside from the ambiguous 'default', but we do that for symmetry with 'TryReadPixel'
	public bool TryWritePixel(int x, int y, Color<byte> color)
		=> TryWritePixel(x, y, color.R, color.G, color.B, color.A);

	public bool TryWritePixel(int x, int y, float r, float g, float b, float a)
	{
		unsafe
		{
			return SDL_WriteSurfacePixelFloat(mSurface, x, y, r, g, b, a);
		}
	}

	public bool TryWritePixel(int x, int y, Color<float> color)
		=> TryWritePixel(x, y, color.R, color.G, color.B, color.A);

	public void UnsafeUnlock()
	{
		unsafe
		{
			SDL_UnlockSurface(mSurface);
		}
	}
}
