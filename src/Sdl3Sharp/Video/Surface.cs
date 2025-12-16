using Sdl3Sharp.Internal.Interop.NativeImportConditions;
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
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace Sdl3Sharp.Video;

public partial class Surface : IDisposable
{
	private interface IUnsafeConstructorDispatch;

	private static readonly ConcurrentDictionary<IntPtr, WeakReference<Surface>> mKnownInstances = [];

	/// <exception cref="ArgumentException">
	/// <paramref name="pixels"/> is <see cref="Utilities.NativeMemory.IsValid">invalid</see>
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
	private Utilities.NativeMemory mNativeMemory = default;
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

	/// <inheritdoc/>
	~Surface() => Dispose(disposing: false, forget: true);

	/// <summary>
	/// Gets or sets the alpha modulation value used in blit operations
	/// </summary>
	/// <value>
	/// The alpha modulation value used in blit operations
	/// </value>
	/// <remarks>
	/// <para>
	/// When this surface is blitted, during the blit operation the source alpha value is modulated by this alpha value according to the following formula:
	///	<code>srcA = srcA * (alpha / 255)</code>
	/// </para>
	/// </remarks>
	public byte AlphaMod
	{
		get
		{
			unsafe
			{
				Unsafe.SkipInit(out byte alpha);

				SDL_GetSurfaceAlphaMod(mSurface, &alpha);

				return alpha;
			}
		}
		
		set
		{
			unsafe
			{
				SDL_SetSurfaceAlphaMod(mSurface, value);
			}
		}
	}

	/// <summary>
	/// Gets or sets the blend mode used for blit operations
	/// </summary>
	/// <value>
	/// The blend mode used for blit operations
	/// </value>
	/// <remarks>
	/// <para>
	/// To copy a surface to another surface without blending with the existing data, the <see cref="BlendMode"/> of the source surface should be set to <see cref="BlendMode.None"/>.
	/// </para>
	/// </remarks>
	/// <exception cref="SdlException">
	/// When setting this property, the specified blend mode is <see cref="BlendMode.Invalid"/> or not supported by the platform or renderer (check <see cref="Error.TryGet(out string?)"/> for more information)
	/// </exception>
	public BlendMode BlendMode
	{
		get
		{
			unsafe
			{
				Unsafe.SkipInit(out BlendMode blendMode);

				SDL_GetSurfaceBlendMode(mSurface, &blendMode);

				return blendMode;
			}
		}

		set
		{
			unsafe
			{
				if (!(bool)SDL_SetSurfaceBlendMode(mSurface, value)
					&& Error.SDL_GetError() is var message
					&& message is not null
					&& !MemoryMarshal.CreateReadOnlySpanFromNullTerminated(message).SequenceEqual("Parameter 'surface' is invalid"u8) /* filter out "surface" argument errors */)
				{
					// value is BlendMode.Invalid or value is an unsupported blend mode

					failSdlError(message);
				}
			}

			[DoesNotReturn]
			static unsafe void failSdlError(byte* message) => throw new SdlException(Utf8StringMarshaller.ConvertToManaged(message));
		}
	}

	/// <summary>
	/// Gets or sets the clipping rectangle for the surface
	/// </summary>
	/// <value>
	/// The clipping rectangle for the surface, or <c><see langword="null"/></c> if clipping is disabled
	/// </value>
	/// <remarks>
	/// <para>
	/// When this surface is blitted onto, only the area within the clipping rectangle is drawn into.
	/// Note that blits are automatically clipped to the edges of the source and destination surfaces.
	/// </para>
	/// <para>
	/// If the value of this property is <c><see langword="null"/></c>, clipping is disabled.
	/// </para>
	/// <para>
	/// Alternatively, you can use the methods <see cref="ResetClippingRect"/> and <see cref="SetClippingRect(in Rect{int})"/> to manipulate the clipping rectangle.
	/// The latter returns a value indicating whether the set clipping rectangle intersects the surface area.
	/// </para>
	/// </remarks>
	public Rect<int>? ClippingRect
	{
		get
		{
			unsafe
			{
				Unsafe.SkipInit(out Rect<int> rect);

				return (bool)SDL_GetSurfaceClipRect(mSurface, &rect)
					? rect
					: null;
			}
		}

		set
		{
			unsafe
			{
				SDL_SetSurfaceClipRect(mSurface, value is Rect<int> rect
					? &rect
					: null
				);
			}
		}	
	}

	/// <summary>
	/// Gets or sets the color key (transparent pixel value) in the surface
	/// </summary>
	/// <value>
	/// The color key (transparent pixel value) in the surface, as an encoded pixel value used by the surface, or <c><see langword="null"/></c> if color keying is disabled
	/// </value>
	/// <remarks>
	/// <para>
	/// The color key defines a pixel value that will be treated as transparent in a blit.
	/// For example, one can use this to specify that cyan pixels should be considered transparent, and therefore not rendered.
	/// </para>
	/// <para>
	/// The value of this property uses the encoded pixel value format used by the surface, as generated by <see cref="MapColor(byte, byte, byte)"/>.
	/// </para>
	/// <para>
	/// Alternatively, if you just want to check whether color keying is enabled, you can use the <see cref="HasColorKey"/> property instead.
	/// </para>
	/// </remarks>
	/// <exception cref="SdlException">
	/// When setting this property, the specified color key is invalid for the surface's pixel format (check <see cref="Error.TryGet(out string?)"/> for more information).
	/// For example, this can happen when the key is out of range for palettized formats.
	/// </exception>
	public uint? ColorKey
	{
		get
		{
			unsafe
			{
				Unsafe.SkipInit(out uint key);

				return SDL_GetSurfaceColorKey(mSurface, &key)
					? key
					: null;
			}
		}

		set
		{
			unsafe
			{
				if (!(bool)SDL_SetSurfaceColorKey(mSurface, enabled: value.HasValue, key: value.GetValueOrDefault())
					&& Error.SDL_GetError() is var message
					&& message is not null
					&& !MemoryMarshal.CreateReadOnlySpanFromNullTerminated(message).SequenceEqual("Parameter 'surface' is invalid"u8) /* filter out "surface" argument errors */)
				{
					// the key as palette index is invalid

					failSdlError(message);
				}
			}

			[DoesNotReturn]
			static unsafe void failSdlError(byte* message) => throw new SdlException(Utf8StringMarshaller.ConvertToManaged(message));
		}
	}

	/// <summary>
	/// Gets or sets the color modulation values used in blit operations
	/// </summary>
	/// <value>
	/// The color modulation values used in blit operations
	/// </value>
	/// <remarks>
	/// <para>
	/// When this surface is blitted, during the blit operation each source color channel is modulated by the appropriate color value according to the following formula:
	/// <code>srcC = srcC * (color / 255)</code>
	/// </para>
	/// </remarks>
	public (byte R, byte G, byte B) ColorMod
	{
		get
		{
			unsafe
			{
				Unsafe.SkipInit(out (byte R, byte G, byte B) color);

				SDL_GetSurfaceColorMod(mSurface, &color.R, &color.G, &color.B);

				return color;
			}
		}

		set
		{
			unsafe
			{
				SDL_SetSurfaceColorMod(mSurface, value.R, value.G, value.B);
			}
		}
	}

	/// <summary>
	/// Gets or sets the color space of the surface
	/// </summary>
	/// <value>
	/// The color space of the surface
	/// </value>
	/// <remarks>
	/// <para>
	/// The color space defaults to <see cref="ColorSpace.SrgbLinear"/> for floating point formats, <see cref="ColorSpace.Hdr10"/> for 10-bit formats, <see cref="ColorSpace.Srgb"/> for other RGB surfaces and <see cref="ColorSpace.Bt709Full"/> for YUV textures.
	/// </para>
	/// <para>
	/// Setting the color space doesn't change the pixels, only how they are interpreted in color operations.
	/// If you want to convert the pixels to a different color space, use an appropriate one of the TryConvert* methods.
	/// </para>
	/// </remarks>
	public ColorSpace ColorSpace
	{
		get
		{
			unsafe
			{
				return SDL_GetSurfaceColorspace(mSurface);
			}
		}

		set
		{
			unsafe
			{
				SDL_SetSurfaceColorspace(mSurface, value);
			}
		}
	}

	/// <summary>
	/// Gets the surface flags
	/// </summary>
	/// <value>
	/// The surface flags
	/// </value>
	/// <remarks>
	/// <para>
	/// Instead of examining the flags directly, you can use the specific properties <see cref="IsLocked"/>, <see cref="IsPreAllocated"/>, <see cref="IsSimdAligned"/>, and <see cref="MustLock"/> to check for particular surface characteristics.
	/// </para>
	/// </remarks>
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

	/// <summary>
	/// Gets the pixel format of the surface
	/// </summary>
	/// <value>
	/// The pixel format of the surface
	/// </value>
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

	/// <summary>
	/// Gets a value indicating whether the surface has alternate images
	/// </summary>
	/// <value>
	/// <c><see langword="true"/></c>, if alternate versions of the surface are available; otherwise, <c><see langword="false"/></c>
	/// </value>
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

	/// <summary>
	/// Gets a value indicating whether the surface has a color key (transparent pixel value) set
	/// </summary>
	/// <value>
	/// <c><see langword="true"/></c>, if the surface has a color key set; otherwise, <c><see langword="false"/></c>
	/// </value>
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

	/// <summary>
	/// Gets or sets the maximum dynamic range, in terms of the <see cref="SdrWhitePoint">SDR white point</see>
	/// </summary>
	/// <value>
	/// The maximum dynamic range, in terms of the <see cref="SdrWhitePoint">SDR white point</see>
	/// </value>
	/// <remarks>
	/// <para>
	/// This property is used for HDR10 and floating point <see cref="Surface"/>s.
	/// </para>
	/// <para>
	/// The value of this property defines the maximum dynamic range used by the content, in terms of the <see cref="SdrWhitePoint">SDR white point</see>.
	/// </para>
	/// <para>
	/// The value defaults to <c>0.0</c>, which disables <see cref="TonemapOperator">tone mapping</see>.
	/// </para>
	/// </remarks>
	public float HdrHeadroom
	{
		get => Properties?.TryGetFloatValue(PropertyNames.HdrHeadroomFloat, out var hdrHeadroom) is true
			? hdrHeadroom
			: default;

		set => Properties?.TrySetFloatValue(PropertyNames.HdrHeadroomFloat, value);
	}

	/// <summary>
	/// Gets the height of the surface, in pixels
	/// </summary>
	/// <value>
	/// The height of the surface, in pixels
	/// </value>
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

	/// <summary>
	/// Gets or sets the hotspot pixel offset from the left edge for a cursor
	/// </summary>
	/// <value>
	/// The hotspot pixel offset from the left edge for a cursor
	/// </value>
	/// <remarks>
	/// <para>
	/// The value of this property defines the pixel offset to the hotspot from the left edge of the <see cref="Surface"/> used as a cursor.
	/// </para>
	/// </remarks>
	public long HotspotX
	{
		get => Properties?.TryGetNumberValue(PropertyNames.HotspotXNumber, out var hotspotX) is true
			? hotspotX
			: default;

		set => Properties?.TrySetNumberValue(PropertyNames.HotspotXNumber, value);
	}

	/// <summary>
	/// Gets or sets the hotspot pixel offset from the top edge for a cursor
	/// </summary>
	/// <value>
	/// The hotspot pixel offset from the top edge for a cursor
	/// </value>
	/// <remarks>
	/// <para>
	/// The value of this property defines the pixel offset to the hotspot from the top edge of the <see cref="Surface"/> used as a cursor.
	/// </para>
	/// </remarks>
	public long HotspotY
	{
		get => Properties?.TryGetNumberValue(PropertyNames.HotspotYNumber, out var hotspotY) is true
			? hotspotY
			: default;

		set => Properties?.TrySetNumberValue(PropertyNames.HotspotYNumber, value);
	}

	/// <summary>
	/// Gets a list of all versions of the surface
	/// </summary>
	/// <value>
	/// A list of all versions of the surface, including itself
	/// </value>
	/// <remarks>
	/// <para>
	/// The returned array includes all alternate images of the surface, including itself as the first element.
	/// </para>
	/// </remarks>
	public Surface[] Images
	{
		get
		{
			unsafe
			{
				// TODO: do we need to handle the surface reference counter for the alternate images coming from SDL here?

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

	/// <summary>
	/// Gets a value indicating whether the surface is currently locked
	/// </summary>
	/// <value>
	/// <c><see langword="true"/></c>, if the surface is currently locked; otherwise, <c><see langword="false"/></c>
	/// </value>
	/// <remarks>
	/// <para>
	/// It's safe to access <see cref="UnsafePixels"/> only if this property is <c><see langword="true"/></c>.
	/// </para>
	/// <para>
	/// This property is meant to use as part of the <see cref="MustLock"/> - <see cref="IsLocked"/> - <see cref="TryUnsafeLock"/> - <see cref="UnsafePixels"/> - <see cref="UnsafeUnlock"/> pattern,
	/// if you want to access the surface's pixel memory directly in a faster and more efficient way.
	/// If you're looking for a simpler and safer way to access the pixel memory, consider using <see cref="TryLock(out SurfacePixelMemoryManager?)"/> instead.
	/// </para>
	/// </remarks>
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

	/// <summary>
	/// Gets a value indicating whether the surface uses pre-allocated pixel memory
	/// </summary>
	/// <value>
	/// <c><see langword="true"/></c>, if the surface uses pre-allocated pixel memory; otherwise, <c><see langword="false"/></c>
	/// </value>
	/// <remarks>
	/// <para>
	/// A surface uses pre-allocated pixel memory, for example, when it was created using <see cref="Surface(int, int, PixelFormat, Utilities.NativeMemory, int)"/>, <see cref="Surface(int, int, PixelFormat, Memory{byte}, int)"/>, or <see cref="Surface(int, int, PixelFormat, void*, int)"/>.
	/// </para>
	/// </remarks>
	public bool IsPreAllocated
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

	/// <summary>
	/// Gets or sets a value indicating whether the surface uses run-length encoding 
	/// </summary>
	/// <value>
	/// <c><see langword="true"/></c>, if the surface uses run-length encoding; otherwise, <c><see langword="false"/></c>
	/// </value>
	/// <remarks>
	/// <para>
	/// If RLE is enabled, color key and alpha blending blits are much faster, but the surface must be locked before directly accessing the pixels.
	/// </para>
	/// <para>
	/// When setting this property, it silently fails if the surface's pixel format is a <see cref="PixelFormatExtensions.get_IsFourCC(PixelFormat)">FourCC</see> format.
	/// </para>
	/// </remarks>
	public bool IsRle
	{
		get
		{
			unsafe
			{
				return SDL_SurfaceHasRLE(mSurface);
			}
		}

		set
		{
			unsafe
			{
				SDL_SetSurfaceRLE(mSurface, value);
			}
		}
	}

	/// <summary>
	/// Gets a value indicating whether the surface uses pixel memory that's aligned for SIMD operations
	/// </summary>
	/// <value>
	/// <c><see langword="true"/></c>, if the surface uses pixel memory that's aligned for SIMD operations; otherwise, <c><see langword="false"/></c>
	/// </value>
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

	/// <summary>
	/// Gets a value indicating whether the surface must be locked before accessing it's pixel memory
	/// </summary>
	/// <value>
	/// <c><see langword="true"/></c>, if the surface must be locked before accessing it's pixel memory; otherwise, <c><see langword="false"/></c>
	/// </value>
	/// <remarks>
	/// <para>
	/// This property is meant to use as part of the <see cref="MustLock"/> - <see cref="IsLocked"/> - <see cref="TryUnsafeLock"/> - <see cref="UnsafePixels"/> - <see cref="UnsafeUnlock"/> pattern,
	/// if you want to access the surface's pixel memory directly in a faster and more efficient way.
	/// If you're looking for a simpler and safer way to access the pixel memory, consider using <see cref="TryLock(out SurfacePixelMemoryManager?)"/> instead.
	/// </para>
	/// </remarks>
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

	/// <summary>
	/// Gets or sets the palette used by the surface
	/// </summary>
	/// <value>
	/// The palette used by the surface, or <c><see langword="null"/></c> if the surface doesn't use a palette
	/// </value>
	/// <remarks>
	/// <para>
	/// A single <see cref="Coloring.Palette"/> can be shared between multiple <see cref="Surface"/>s.
	/// </para>
	/// </remarks>
	/// <exception cref="SdlException">
	/// When setting this property, the specified palette doesn't match the surface's pixel format (check <see cref="Error.TryGet(out string?)"/> for more information)
	/// </exception>
	public Palette? Palette
	{
		get
		{
			unsafe
			{
				if (!Palette.TryGetOrCreate(SDL_GetSurfacePalette(mSurface), out var palette))
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
				// SDL_SetSurfacePalette potentially destroys the old palette if its ref count reaches zero,
				// but we must make sure, if we have a managed wrapper for it around, that it doesn't get destroyed while we still have a reference to it.
				// Therefore, we bump the ref count to 2 before calling SDL_SetSurfacePalette, and restore it if needed.

				// Just on a side note: This is highly illegal behavior, messing with SDL's internal reference counting like this.
				// But I couldn't find a better way to do this.

				var oldPaletteRefCount = int.MaxValue;
				var oldPalette = SDL_GetSurfacePalette(mSurface);				
				if (oldPalette is not null
					&& (value is null || oldPalette != value.Pointer)
					&& Palette.IsKnown(oldPalette)
					&& (oldPaletteRefCount = oldPalette->RefCount) is not > 1)
				{
					oldPalette->RefCount = 2;
				}

				if (!(bool)SDL_SetSurfacePalette(mSurface, value is not null ? value.Pointer : null)
					&& Error.SDL_GetError() is var message
					&& message is not null
					&& !MemoryMarshal.CreateReadOnlySpanFromNullTerminated(message).SequenceEqual("Parameter 'surface' is invalid"u8) /* filter out "surface" argument errors */)
				{
					// the palette doesn't match the surface's format

					// The old palette wasn't replaced, so restore its ref count if we modified it

					if (oldPaletteRefCount is not > 1)
					{
						oldPalette->RefCount = oldPaletteRefCount;
					}

					failSdlError(message);
				}
			}

			[DoesNotReturn]
			static unsafe void failSdlError(byte* message) => throw new SdlException(Utf8StringMarshaller.ConvertToManaged(message));
		}
	}

	/// <summary>
	/// Gets the pitch of the surface, in bytes
	/// </summary>
	/// <value>
	/// The pitch of the surface, in bytes
	/// </value>
	/// <remarks>
	/// <para>
	/// The pitch is the length of a single row of pixels in bytes.
	/// This value may be different from the width of the surface times the number of bytes per pixel for packing and alignment purposes.
	/// </para>
	/// <para>
	/// Pixels are arranged in memory in rows, with the top row first. Each row occupies an amount of memory given by the pitch (sometimes known as the "row stride").
	/// </para>
	/// <para>
	/// Within each row, pixels are arranged from left to right until the width is reached.
	/// Each pixel occupies a number of bits appropriate for its format, with most formats representing each pixel as one or more whole bytes (in some indexed formats, instead multiple pixels are packed into each byte), and a byte order given by the format.
	/// After encoding all pixels, any remaining bytes to reach the pitch are used as padding to reach a desired alignment, and have undefined contents.
	/// </para>
	/// <para>
	/// When a surface holds YUV format data, the planes are assumed to be contiguous without padding between them, e.g. a 32⨯32 surface in NV12 format with a pitch of 32 would consist of 32⨯32 bytes of Y plane followed by 32⨯16 bytes of UV plane.
	/// </para>
	/// <para>
	/// When a surface holds MJPG format data, pixels points at the compressed JPEG image and pitch is the length of that data.
	/// </para>
	/// </remarks>
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

	/// <summary>
	/// Get the properties associated with the surface
	/// </summary>
	/// <value>
	/// The properties associated with the surface, or <see langword="null"/> if the properties could not be retrieved successfully (check <see cref="Error.TryGet(out string?)"/> for more information)
	/// </value>
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
	/// <summary>
	/// Gets or sets the rotation angle, in degrees, for the surface
	/// </summary>
	/// <value>
	/// The rotation angle, in clockwise degrees, for the surface
	/// </value>
	/// <remarks>
	/// <para>
	/// The value of this property defines the number of degrees the surface's pixel data is meant to be rotated clockwise to make it right-side up.
	/// </para>
	/// <para>
	/// This is used by the camera API, if a mobile device is oriented differently than what its camera provides (i.e. the camera always provides portrait images but the phone is being held in landscape orientation).
	/// </para>
	/// <para>
	/// The value defaults to <c>0.0</c>.
	/// </para>
	/// </remarks> 
	public float Rotation
	{
		get => Properties?.TryGetFloatValue(PropertyNames.RotationFloat, out var rotation) is true
			? rotation
			: default;

		set => Properties?.TrySetFloatValue(PropertyNames.RotationFloat, value);
	}
#endif

	/// <summary>
	/// Gets or sets the defining value for 100% white
	/// </summary>
	/// <value>
	/// The defining value for 100% white
	/// </value>
	/// <remarks>
	/// <para>
	/// This property is used by HDR10 and floating point <see cref="Surface"/>s.
	/// </para>
	/// <para>
	/// The value of the property defines the value of 100% diffuse white, with higher values being displayed in the <see cref="HdrHeadroom">High Dynamic Range headroom</see>.
	/// </para>
	/// <para>
	/// The value defaults to <c>203</c> for HDR10 surfaces and <c>1.0</c> for floating point surfaces.
	/// </para>
	/// </remarks>
	public float SdrWhitePoint
	{
		get => Properties?.TryGetFloatValue(PropertyNames.SdrWhitePointFloat, out var sdrWhitePoint) is true
			? sdrWhitePoint
			: default;

		set => Properties?.TrySetFloatValue(PropertyNames.SdrWhitePointFloat, value);
	}

	/// <summary>
	/// Gets or sets the expression of the tonemap operator to used when compressing from a higher dynamic range to a lower dynamic range
	/// </summary>
	/// <value>
	/// The expression of the tonemap operator to used when compressing from a higher dynamic range to a lower dynamic range
	/// </value>
	/// <remarks>
	/// <para>
	/// The value of this property defines the tone mapping operator used when compressing from a <see cref="Surface"/> with high dynamic range to another with lower dynamic range.
	/// Currently this supports the following values:
	/// <list type="bullet">
	///		<item>
	///			<term><c>"chrome"</c></term>
	///			<description>The same tone mapping that Chrome uses for HDR content</description>
	///		</item>
	///		<item>
	///			<term><c>"*=N"</c> where <c>N</c> is a floating point number</term>
	///			<description><c>N</c> is a floating point scale factor applied in linear. E.g. <c>"*=0.5"</c>.</description>
	///		</item>
	///		<item>
	///			<term><c>"none"</c></term>
	///			<description>Tone mapping is disabled</description>
	///		</item>
	/// </list>
	/// </para>
	/// <para>
	/// The value defaults to <c>"chrome"</c>.
	/// </para>
	/// </remarks>
	public string TonemapOperator
	{
		get => Properties?.TryGetStringValue(PropertyNames.TonemapOperatorString, out var tonemapOperator) is true
			&& tonemapOperator is not null
			? tonemapOperator
			: string.Empty;

		set => Properties?.TrySetStringValue(PropertyNames.TonemapOperatorString, value);
	}

	/// <summary>
	/// Gets the underlying pixel memory of the surface
	/// </summary>
	/// <value>
	/// The underlying pixel memory of the surface
	/// </value>
	/// <remarks>
	/// <para>
	/// It's unsafe to access this property for surfaces that <see cref="MustLock">must be locked</see> and are not currently <see cref="IsLocked">locked</see>.
	/// </para>
	/// <para>
	/// This property is meant to use as part of the <see cref="MustLock"/> - <see cref="IsLocked"/> - <see cref="TryUnsafeLock"/> - <see cref="UnsafePixels"/> - <see cref="UnsafeUnlock"/> pattern,
	/// if you want to access the surface's pixel memory directly in a faster and more efficient way.
	/// If you're looking for a simpler and safer way to access the pixel memory, consider using <see cref="TryLock(out SurfacePixelMemoryManager?)"/> instead.
	/// </para>
	/// </remarks>
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

	/// <summary>
	/// Gets the width of the surface, in pixels
	/// </summary>
	/// <value>
	/// The width of the surface, in pixels
	/// </value>
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

	/// <summary>
	/// Removes all alternate versions associated with the surface
	/// </summary>
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

	/// <summary>
	/// Maps RGB component values to an <em>opaque</em> encoded pixel value for the surface
	/// </summary>
	/// <param name="r">The red component value to encode</param>
	/// <param name="g">The green component value to encode</param>
	/// <param name="b">The blue component value to encode</param>
	/// <returns>An encoded pixel value in the pixel format of the surface</returns>
	/// <remarks>
	/// <para>
	/// This method maps the RGB color value to the surface's pixel format and returns the encoded pixel value best approximating the given RGB color value for the pixel format.
	/// </para>
	/// <para>
	/// If the surface has a <see cref="Palette"/>, the index of the closest matching color in the palette will be returned.
	/// </para>
	/// <para>
	/// If the pixel format has an alpha component, the encoded pixel value's alpha component value will be set to be fully opaque.
	/// </para>
	/// <para>
	/// If the pixel format's bits-per-pixel value (color depth) is less than 32-bits per pixel then the unused upper bits of the returned encoded pixel value can safely be ignored
	/// (e.g., with a 16-bits per pixel format the returned encoded pixel value can be safely cast (truncated) to an <see cref="ushort"/> value, and similarly to a <see cref="byte"/> value for an 8-bits per pixel formats).
	/// </para>
	/// </remarks>
	public uint MapColor(byte r, byte g, byte b)
	{
		unsafe
		{
			return SDL_MapSurfaceRGB(mSurface, r, g, b);
		}
	}

	/// <summary>
	/// Maps RGBA component values to an encoded pixel value for the surface
	/// </summary>
	/// <param name="r">The red component value to encode</param>
	/// <param name="g">The green component value to encode</param>
	/// <param name="b">The blue component value to encode</param>
	/// <param name="a">The alpha component value to encode</param>
	/// <returns>An encoded pixel value in the pixel format of the surface</returns>
	/// <remarks>
	/// <para>
	/// This method maps the RGBA color value to the surface's pixel format and returns the encoded pixel value best approximating the given RGBA color value for the pixel format.
	/// </para>
	/// <para>
	/// If the surface has a <see cref="Palette"/>, the index of the closest matching color in the palette will be returned.
	/// </para>
	/// <para>
	/// If the pixel format has no alpha component, the given alpha component value will be ignored (as it will be in indexed formats with a <see cref="Palette"/>).
	/// </para>
	/// <para>
	/// If the pixel format's bits-per-pixel value (color depth) is less than 32-bits per pixel then the unused upper bits of the returned encoded pixel value can safely be ignored
	/// (e.g., with a 16-bits per pixel format the returned encoded pixel value can be safely cast (truncated) to an <see cref="ushort"/> value, and similarly to a <see cref="byte"/> value for an 8-bits per pixel formats).
	/// </para>
	/// </remarks>
	public uint MapColor(byte r, byte g, byte b, byte a)
	{
		unsafe
		{
			return SDL_MapSurfaceRGBA(mSurface, r, g, b, a);
		}
	}

	/// <summary>
	/// Maps a <see cref="Color{T}">Color</see>&lt;<see cref="byte"/>&gt; to an encoded pixel value for the surface
	/// </summary>
	/// <param name="color">The <see cref="Color{T}">Color</see>&lt;<see cref="byte"/>&gt; to encoded</param>
	/// <returns>An encoded pixel value in the pixel format of the surface</returns>
	/// <remarks>
	/// <para>
	/// This method maps the <see cref="Color{T}">Color</see>&lt;<see cref="byte"/>&gt; to the surface's pixel format and returns the encoded pixel value best approximating the given <see cref="Color{T}">Color</see>&lt;<see cref="byte"/>&gt; for the pixel format.
	/// </para>
	/// <para>
	/// If the surface has a <see cref="Palette"/>, the index of the closest matching color in the palette will be returned.
	/// </para>
	/// <para>
	/// If the pixel format has no alpha component, the given alpha component value will be ignored (as it will be in indexed formats with a <see cref="Palette"/>).
	/// </para>
	/// <para>
	/// If the pixel format's bits-per-pixel value (color depth) is less than 32-bits per pixel then the unused upper bits of the returned encoded pixel value can safely be ignored
	/// (e.g., with a 16-bits per pixel format the returned encoded pixel value can be safely cast (truncated) to an <see cref="ushort"/> value, and similarly to a <see cref="byte"/> value for an 8-bits per pixel formats).
	/// </para>
	/// </remarks>
	public uint MapColor(Color<byte> color) => MapColor(color.R, color.G, color.B, color.A);

	/// <summary>
	/// Disables the clipping rectangle for the surface
	/// </summary>
	/// <remarks>
	/// <para>
	/// Alternatively, you can set <see cref="ClippingRect"/> to <c><see langword="null"/></c> instead to disable the clipping rectangle.
	/// </para>
	/// </remarks>
	public void ResetClippingRect()
	{
		unsafe
		{
			SDL_SetSurfaceClipRect(mSurface, rect: null);
		}
	}

	/// <summary>
	/// Sets the clipping rectangle for the surface
	/// </summary>
	/// <param name="rect">The clipping rectangle to set</param>
	/// <returns><c><see langword="true"/></c>, if the clipping rectangle set intersects with the surface area; otherwise, <c><see langword="false"/></c></returns>
	/// <remarks>
	/// <para>
	/// Alternatively, you can set <see cref="ClippingRect"/> instead, if you're not interested in whether the new clipping rectangle intersects with the surface area.
	/// </para>
	/// </remarks>
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

	/// <summary>
	/// Tries to add an alternate version of the surface
	/// </summary>
	/// <param name="image">The alternate version to associate with the surface</param>
	/// <returns><c><see langword="true"/></c>, if the alternate version was added successfully; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method adds an alternate version of this surface, usually used for content with high DPI representations like cursors or icons.
	/// The size, format, and content do not need to match the original surface, and these alternate versions will not be updated when the original surface changes.
	/// </para>
	/// </remarks>
	public bool TryAddAlternateImage(Surface image)
	{
		unsafe
		{
			return SDL_AddSurfaceAlternateImage(mSurface, image is not null ? image.mSurface : null);

			// TODO: what about SDL_DestroySurface on the image?
		}
	}

	/// <summary>
	/// Tries to perform a fast blit from a specified source surface to this surface with clipping
	/// </summary>
	/// <param name="destinationRect">The destination rectangle on this surface to blit onto. The <see cref="Rect{T}.Width">width</see> and the <see cref="Rect{T}.Height">height</see> are ignored and taken from <paramref name="sourceRect"/> instead.</param>
	/// <param name="source">The source surface to blit from</param>
	/// <param name="sourceRect">The source rectangle on the source surface to blit</param>
	/// <returns><c><see langword="true"/></c>, if the blit was successful; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method blits a portion of the <paramref name="source"/> surface defined by <paramref name="sourceRect"/> to this surface at the position defined by <paramref name="destinationRect"/>
	/// while ensuring clipping by <see cref="ClippingRect"/>.
	/// The <see cref="Rect{T}.Width">width</see> and the <see cref="Rect{T}.Height">height</see> of <paramref name="destinationRect"/> are ignored and taken from <paramref name="sourceRect"/> instead.
	/// If you want to specify width and height for the destination rectangle, consider using <see cref="TryBlitScaled(in Rect{int}, Surface, in Rect{int}, ScaleMode)"/> instead.
	/// </para>
	/// <para>
	/// Neither this surface nor the <paramref name="source"/> surface must be locked when calling this method!
	/// </para>
	/// <para>
	/// The blitting semantics for surfaces with and without blending and color keying are defined as follows:
	/// <list type="bullet">
	///		<item>
	///			<term>RGBA 🡒 RGB</term>
	///			<description>
	///				<list type="bullet">
	///					<item>
	///						<term><paramref name="source"/>'s <see cref="BlendMode">BlendMode</see> is <see cref="BlendMode.Blend">Blend</see></term>
	///						<description>
	///							alpha-blend (using the source alpha-channel and per-surface alpha); the <paramref name="source"/>'s <see cref="ColorKey">ColorKey</see> is ignored
	///						</description>
	///					</item>
	///					<item>
	///						<term><paramref name="source"/>'s <see cref="BlendMode">BlendMode</see> is <see cref="BlendMode.None">None</see></term>
	///						<description>
	///							copy RGB; if the <paramref name="source"/>'s <see cref="ColorKey"/> is set, only copy pixels that do not match the color key, ignoring alpha in the comparison
	///						</description>
	///					</item>
	///				</list>
	///			</description>
	///		</item>
	///		<item>
	///			<term>RGB 🡒 RGBA</term>
	///			<description>
	///				<list type="bullet">
	///					<item>
	///						<term><paramref name="source"/>'s <see cref="BlendMode">BlendMode</see> is <see cref="BlendMode.Blend">Blend</see></term>
	///						<description>
	///							alpha-blend (using the source per-surface alpha); if the <paramref name="source"/>'s <see cref="ColorKey"/> is set, only copy pixels that do not match the color key
	///						</description>
	///					</item>
	///					<item>
	///						<term><paramref name="source"/>'s <see cref="BlendMode">BlendMode</see> is <see cref="BlendMode.None">None</see></term>
	///						<description>
	///							copy RGB, set destination alpha to source per-surface alpha value; if the <paramref name="source"/>'s <see cref="ColorKey"/> is set, only copy pixels that do not match the color key
	///						</description>
	///					</item>
	///				</list>
	///			</description>
	///		</item>
	///		<item>
	///			<term>RGBA 🡒 RGBA</term>
	///			<description>
	///				<list type="bullet">
	///					<item>
	///						<term><paramref name="source"/>'s <see cref="BlendMode">BlendMode</see> is <see cref="BlendMode.Blend">Blend</see></term>
	///						<description>
	///							alpha-blend (using the source alpha-channel and per-surface alpha); the <paramref name="source"/>'s <see cref="ColorKey">ColorKey</see> is ignored
	///						</description>
	///					</item>
	///					<item>
	///						<term><paramref name="source"/>'s <see cref="BlendMode">BlendMode</see> is <see cref="BlendMode.None">None</see></term>
	///						<description>
	///							copy all of RGBA; if the <paramref name="source"/>'s <see cref="ColorKey"/> is set, only copy pixels that do not match the color key, ignoring alpha in the comparison
	///						</description>
	///					</item>
	///				</list>
	///			</description>
	///		</item>
	///		<item>
	///			<term>RGB 🡒 RGB</term>
	///			<description>
	///				<list type="bullet">
	///					<item>
	///						<term><paramref name="source"/>'s <see cref="BlendMode">BlendMode</see> is <see cref="BlendMode.Blend">Blend</see></term>
	///						<description>
	///							alpha-blend (using the source per-surface alpha); if the <paramref name="source"/>'s <see cref="ColorKey"/> is set, only copy pixels that do not match the color key
	///						</description>
	///					</item>
	///					<item>
	///						<term><paramref name="source"/>'s <see cref="BlendMode">BlendMode</see> is <see cref="BlendMode.None">None</see></term>
	///						<description>
	///							copy RGB; if the <paramref name="source"/>'s <see cref="ColorKey"/> is set, only copy pixels that do not match the color key
	///						</description>
	///					</item>
	///				</list>
	///			</description>
	///		</item>
	/// </list>
	/// </para>
	/// </remarks>
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

	/// <summary>
	/// Tries to perform a fast blit from a specified source surface to this surface with clipping
	/// </summary>
	/// <param name="destinationRect">The destination rectangle on this surface to blit onto. The <see cref="Rect{T}.Width">width</see> and the <see cref="Rect{T}.Height">height</see> are ignored and taken from <paramref name="source"/> instead.</param>
	/// <param name="source">The source surface to blit from</param>
	/// <returns><c><see langword="true"/></c>, if the blit was successful; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method blits the entire <paramref name="source"/> surface to this surface at the position defined by <paramref name="destinationRect"/>
	/// while ensuring clipping by <see cref="ClippingRect"/>.
	/// The <see cref="Rect{T}.Width">width</see> and the <see cref="Rect{T}.Height">height</see> of <paramref name="destinationRect"/> are ignored and taken from the <paramref name="source"/> instead.
	/// If you want to specify width and height for the destination rectangle, consider using <see cref="TryBlitScaled(in Rect{int}, Surface, ScaleMode)"/> instead.
	/// </para>
	/// <para>
	/// Neither this surface nor the <paramref name="source"/> surface must be locked when calling this method!
	/// </para>
	/// <para>
	/// The blitting semantics for surfaces with and without blending and color keying are defined as follows:
	/// <list type="bullet">
	///		<item>
	///			<term>RGBA 🡒 RGB</term>
	///			<description>
	///				<list type="bullet">
	///					<item>
	///						<term><paramref name="source"/>'s <see cref="BlendMode">BlendMode</see> is <see cref="BlendMode.Blend">Blend</see></term>
	///						<description>
	///							alpha-blend (using the source alpha-channel and per-surface alpha); the <paramref name="source"/>'s <see cref="ColorKey">ColorKey</see> is ignored
	///						</description>
	///					</item>
	///					<item>
	///						<term><paramref name="source"/>'s <see cref="BlendMode">BlendMode</see> is <see cref="BlendMode.None">None</see></term>
	///						<description>
	///							copy RGB; if the <paramref name="source"/>'s <see cref="ColorKey"/> is set, only copy pixels that do not match the color key, ignoring alpha in the comparison
	///						</description>
	///					</item>
	///				</list>
	///			</description>
	///		</item>
	///		<item>
	///			<term>RGB 🡒 RGBA</term>
	///			<description>
	///				<list type="bullet">
	///					<item>
	///						<term><paramref name="source"/>'s <see cref="BlendMode">BlendMode</see> is <see cref="BlendMode.Blend">Blend</see></term>
	///						<description>
	///							alpha-blend (using the source per-surface alpha); if the <paramref name="source"/>'s <see cref="ColorKey"/> is set, only copy pixels that do not match the color key
	///						</description>
	///					</item>
	///					<item>
	///						<term><paramref name="source"/>'s <see cref="BlendMode">BlendMode</see> is <see cref="BlendMode.None">None</see></term>
	///						<description>
	///							copy RGB, set destination alpha to source per-surface alpha value; if the <paramref name="source"/>'s <see cref="ColorKey"/> is set, only copy pixels that do not match the color key
	///						</description>
	///					</item>
	///				</list>
	///			</description>
	///		</item>
	///		<item>
	///			<term>RGBA 🡒 RGBA</term>
	///			<description>
	///				<list type="bullet">
	///					<item>
	///						<term><paramref name="source"/>'s <see cref="BlendMode">BlendMode</see> is <see cref="BlendMode.Blend">Blend</see></term>
	///						<description>
	///							alpha-blend (using the source alpha-channel and per-surface alpha); the <paramref name="source"/>'s <see cref="ColorKey">ColorKey</see> is ignored
	///						</description>
	///					</item>
	///					<item>
	///						<term><paramref name="source"/>'s <see cref="BlendMode">BlendMode</see> is <see cref="BlendMode.None">None</see></term>
	///						<description>
	///							copy all of RGBA; if the <paramref name="source"/>'s <see cref="ColorKey"/> is set, only copy pixels that do not match the color key, ignoring alpha in the comparison
	///						</description>
	///					</item>
	///				</list>
	///			</description>
	///		</item>
	///		<item>
	///			<term>RGB 🡒 RGB</term>
	///			<description>
	///				<list type="bullet">
	///					<item>
	///						<term><paramref name="source"/>'s <see cref="BlendMode">BlendMode</see> is <see cref="BlendMode.Blend">Blend</see></term>
	///						<description>
	///							alpha-blend (using the source per-surface alpha); if the <paramref name="source"/>'s <see cref="ColorKey"/> is set, only copy pixels that do not match the color key
	///						</description>
	///					</item>
	///					<item>
	///						<term><paramref name="source"/>'s <see cref="BlendMode">BlendMode</see> is <see cref="BlendMode.None">None</see></term>
	///						<description>
	///							copy RGB; if the <paramref name="source"/>'s <see cref="ColorKey"/> is set, only copy pixels that do not match the color key
	///						</description>
	///					</item>
	///				</list>
	///			</description>
	///		</item>
	/// </list>
	/// </para>
	/// </remarks>
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

	/// <summary>
	/// Tries to perform a fast blit from a specified source surface to this surface with clipping
	/// </summary>
	/// <param name="source">The source surface to blit from</param>
	/// <param name="sourceRect">The source rectangle on the source surface to blit</param>
	/// <returns><c><see langword="true"/></c>, if the blit was successful; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method blits a portion of the <paramref name="source"/> surface defined by <paramref name="sourceRect"/> to this surface at the position <c>(0, 0)</c>
	/// while ensuring clipping by <see cref="ClippingRect"/>.
	/// The width and the height of the destination blit area are taken from <paramref name="sourceRect"/>.
	/// If you want to specify width and height for the destination blit area, consider using <see cref="TryBlitScaled(Surface, in Rect{int}, ScaleMode)"/> instead.
	/// </para>
	/// <para>
	/// Neither this surface nor the <paramref name="source"/> surface must be locked when calling this method!
	/// </para>
	/// <para>
	/// The blitting semantics for surfaces with and without blending and color keying are defined as follows:
	/// <list type="bullet">
	///		<item>
	///			<term>RGBA 🡒 RGB</term>
	///			<description>
	///				<list type="bullet">
	///					<item>
	///						<term><paramref name="source"/>'s <see cref="BlendMode">BlendMode</see> is <see cref="BlendMode.Blend">Blend</see></term>
	///						<description>
	///							alpha-blend (using the source alpha-channel and per-surface alpha); the <paramref name="source"/>'s <see cref="ColorKey">ColorKey</see> is ignored
	///						</description>
	///					</item>
	///					<item>
	///						<term><paramref name="source"/>'s <see cref="BlendMode">BlendMode</see> is <see cref="BlendMode.None">None</see></term>
	///						<description>
	///							copy RGB; if the <paramref name="source"/>'s <see cref="ColorKey"/> is set, only copy pixels that do not match the color key, ignoring alpha in the comparison
	///						</description>
	///					</item>
	///				</list>
	///			</description>
	///		</item>
	///		<item>
	///			<term>RGB 🡒 RGBA</term>
	///			<description>
	///				<list type="bullet">
	///					<item>
	///						<term><paramref name="source"/>'s <see cref="BlendMode">BlendMode</see> is <see cref="BlendMode.Blend">Blend</see></term>
	///						<description>
	///							alpha-blend (using the source per-surface alpha); if the <paramref name="source"/>'s <see cref="ColorKey"/> is set, only copy pixels that do not match the color key
	///						</description>
	///					</item>
	///					<item>
	///						<term><paramref name="source"/>'s <see cref="BlendMode">BlendMode</see> is <see cref="BlendMode.None">None</see></term>
	///						<description>
	///							copy RGB, set destination alpha to source per-surface alpha value; if the <paramref name="source"/>'s <see cref="ColorKey"/> is set, only copy pixels that do not match the color key
	///						</description>
	///					</item>
	///				</list>
	///			</description>
	///		</item>
	///		<item>
	///			<term>RGBA 🡒 RGBA</term>
	///			<description>
	///				<list type="bullet">
	///					<item>
	///						<term><paramref name="source"/>'s <see cref="BlendMode">BlendMode</see> is <see cref="BlendMode.Blend">Blend</see></term>
	///						<description>
	///							alpha-blend (using the source alpha-channel and per-surface alpha); the <paramref name="source"/>'s <see cref="ColorKey">ColorKey</see> is ignored
	///						</description>
	///					</item>
	///					<item>
	///						<term><paramref name="source"/>'s <see cref="BlendMode">BlendMode</see> is <see cref="BlendMode.None">None</see></term>
	///						<description>
	///							copy all of RGBA; if the <paramref name="source"/>'s <see cref="ColorKey"/> is set, only copy pixels that do not match the color key, ignoring alpha in the comparison
	///						</description>
	///					</item>
	///				</list>
	///			</description>
	///		</item>
	///		<item>
	///			<term>RGB 🡒 RGB</term>
	///			<description>
	///				<list type="bullet">
	///					<item>
	///						<term><paramref name="source"/>'s <see cref="BlendMode">BlendMode</see> is <see cref="BlendMode.Blend">Blend</see></term>
	///						<description>
	///							alpha-blend (using the source per-surface alpha); if the <paramref name="source"/>'s <see cref="ColorKey"/> is set, only copy pixels that do not match the color key
	///						</description>
	///					</item>
	///					<item>
	///						<term><paramref name="source"/>'s <see cref="BlendMode">BlendMode</see> is <see cref="BlendMode.None">None</see></term>
	///						<description>
	///							copy RGB; if the <paramref name="source"/>'s <see cref="ColorKey"/> is set, only copy pixels that do not match the color key
	///						</description>
	///					</item>
	///				</list>
	///			</description>
	///		</item>
	/// </list>
	/// </para>
	/// </remarks>
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

	/// <summary>
	/// Tries to perform a fast blit from a specified source surface to this surface with clipping
	/// </summary>
	/// <param name="source">The source surface to blit from</param>
	/// <returns><c><see langword="true"/></c>, if the blit was successful; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method blits the entire <paramref name="source"/> surface to this surface at the position <c>(0, 0)</c>
	/// while ensuring clipping by <see cref="ClippingRect"/>.
	/// The width and the height of the destination blit area are taken from the <paramref name="source"/>.
	/// If you want to specify width and height for the blit area, consider using <see cref="TryBlitScaled(Surface, ScaleMode)"/> instead.
	/// </para>
	/// <para>
	/// Neither this surface nor the <paramref name="source"/> surface must be locked when calling this method!
	/// </para>
	/// <para>
	/// The blitting semantics for surfaces with and without blending and color keying are defined as follows:
	/// <list type="bullet">
	///		<item>
	///			<term>RGBA 🡒 RGB</term>
	///			<description>
	///				<list type="bullet">
	///					<item>
	///						<term><paramref name="source"/>'s <see cref="BlendMode">BlendMode</see> is <see cref="BlendMode.Blend">Blend</see></term>
	///						<description>
	///							alpha-blend (using the source alpha-channel and per-surface alpha); the <paramref name="source"/>'s <see cref="ColorKey">ColorKey</see> is ignored
	///						</description>
	///					</item>
	///					<item>
	///						<term><paramref name="source"/>'s <see cref="BlendMode">BlendMode</see> is <see cref="BlendMode.None">None</see></term>
	///						<description>
	///							copy RGB; if the <paramref name="source"/>'s <see cref="ColorKey"/> is set, only copy pixels that do not match the color key, ignoring alpha in the comparison
	///						</description>
	///					</item>
	///				</list>
	///			</description>
	///		</item>
	///		<item>
	///			<term>RGB 🡒 RGBA</term>
	///			<description>
	///				<list type="bullet">
	///					<item>
	///						<term><paramref name="source"/>'s <see cref="BlendMode">BlendMode</see> is <see cref="BlendMode.Blend">Blend</see></term>
	///						<description>
	///							alpha-blend (using the source per-surface alpha); if the <paramref name="source"/>'s <see cref="ColorKey"/> is set, only copy pixels that do not match the color key
	///						</description>
	///					</item>
	///					<item>
	///						<term><paramref name="source"/>'s <see cref="BlendMode">BlendMode</see> is <see cref="BlendMode.None">None</see></term>
	///						<description>
	///							copy RGB, set destination alpha to source per-surface alpha value; if the <paramref name="source"/>'s <see cref="ColorKey"/> is set, only copy pixels that do not match the color key
	///						</description>
	///					</item>
	///				</list>
	///			</description>
	///		</item>
	///		<item>
	///			<term>RGBA 🡒 RGBA</term>
	///			<description>
	///				<list type="bullet">
	///					<item>
	///						<term><paramref name="source"/>'s <see cref="BlendMode">BlendMode</see> is <see cref="BlendMode.Blend">Blend</see></term>
	///						<description>
	///							alpha-blend (using the source alpha-channel and per-surface alpha); the <paramref name="source"/>'s <see cref="ColorKey">ColorKey</see> is ignored
	///						</description>
	///					</item>
	///					<item>
	///						<term><paramref name="source"/>'s <see cref="BlendMode">BlendMode</see> is <see cref="BlendMode.None">None</see></term>
	///						<description>
	///							copy all of RGBA; if the <paramref name="source"/>'s <see cref="ColorKey"/> is set, only copy pixels that do not match the color key, ignoring alpha in the comparison
	///						</description>
	///					</item>
	///				</list>
	///			</description>
	///		</item>
	///		<item>
	///			<term>RGB 🡒 RGB</term>
	///			<description>
	///				<list type="bullet">
	///					<item>
	///						<term><paramref name="source"/>'s <see cref="BlendMode">BlendMode</see> is <see cref="BlendMode.Blend">Blend</see></term>
	///						<description>
	///							alpha-blend (using the source per-surface alpha); if the <paramref name="source"/>'s <see cref="ColorKey"/> is set, only copy pixels that do not match the color key
	///						</description>
	///					</item>
	///					<item>
	///						<term><paramref name="source"/>'s <see cref="BlendMode">BlendMode</see> is <see cref="BlendMode.None">None</see></term>
	///						<description>
	///							copy RGB; if the <paramref name="source"/>'s <see cref="ColorKey"/> is set, only copy pixels that do not match the color key
	///						</description>
	///					</item>
	///				</list>
	///			</description>
	///		</item>
	/// </list>
	/// </para>
	/// </remarks>
	public bool TryBlit(Surface source)
	{
		unsafe
		{
			return SDL_BlitSurface(source is not null ? source.mSurface : null, srcrect: null, mSurface, dstrect: null);
		}
	}

	/// <summary>
	/// Tries to perform a 9-grid scaled blit from a specified source surface to this surface, which maybe of a different format
	/// </summary>
	/// <param name="destinationRect">The destination rectangle on this surface to blit onto</param>
	/// <param name="source">The source surface to blit from</param>
	/// <param name="sourceRect">The source rectangle on the source surface to blit</param>
	/// <param name="leftWidth">The width, in pixels, of the left corners in <paramref name="sourceRect"/></param>
	/// <param name="rightWidth">The width, in pixels, of the right corners in <paramref name="sourceRect"/></param>
	/// <param name="topHeight">The height, in pixels, of the top corners in <paramref name="sourceRect"/></param>
	/// <param name="bottomHeight">The height, in pixels, of the bottom corners in <paramref name="sourceRect"/></param>
	/// <param name="scale">The scale to use to transform the corners of <paramref name="sourceRect"/> into the corners of <paramref name="destinationRect"/>, or <c>0</c> for an unscaled blit</param>
	/// <param name="scaleMode">The scale mode to use for the blit</param>
	/// <returns><c><see langword="true"/></c>, if the blit was successful; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// The pixels in the source surface are split into a 3⨯3 grid, using the different corner sizes for each corner, and the sides and center making up the remaining pixels.
	/// The corners are then scaled using scale and fit into the corners of the <paramref name="destinationRect"/>.
	/// The sides and center are then stretched into place to cover the remaining portion of the <paramref name="destinationRect"/>.
	/// </para>
	/// <para>
	/// The region specified by <paramref name="sourceRect"/> from the <paramref name="source"/> surface is used as the 9-grid source and scaled to fit into the region specified by <paramref name="destinationRect"/> on this surface.
	/// </para>
	/// </remarks>
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

	/// <summary>
	/// Tries to perform a 9-grid scaled blit from a specified source surface to this surface, which maybe of a different format
	/// </summary>
	/// <param name="destinationRect">The destination rectangle on this surface to blit onto</param>
	/// <param name="source">The source surface to blit from</param>
	/// <param name="leftWidth">The width, in pixels, of the left corners in the <paramref name="source"/></param>
	/// <param name="rightWidth">The width, in pixels, of the right corners in the <paramref name="source"/></param>
	/// <param name="topHeight">The height, in pixels, of the top corners in the <paramref name="source"/></param>
	/// <param name="bottomHeight">The height, in pixels, of the bottom corners in the <paramref name="source"/></param>
	/// <param name="scale">The scale to use to transform the corners of the <paramref name="source"/> into the corners of <paramref name="destinationRect"/>, or <c>0</c> for an unscaled blit</param>
	/// <param name="scaleMode">The scale mode to use for the blit</param>
	/// <returns><c><see langword="true"/></c>, if the blit was successful; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// The pixels in the source surface are split into a 3⨯3 grid, using the different corner sizes for each corner, and the sides and center making up the remaining pixels.
	/// The corners are then scaled using scale and fit into the corners of the <paramref name="destinationRect"/>.
	/// The sides and center are then stretched into place to cover the remaining portion of the <paramref name="destinationRect"/>.
	/// </para>
	/// <para>
	/// The entire <paramref name="source"/> surface is used as the 9-grid source and scaled to fit into the region specified by <paramref name="destinationRect"/> on this surface.
	/// </para>
	/// </remarks>
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

	/// <summary>
	/// Tries to perform a 9-grid scaled blit from a specified source surface to this surface, which maybe of a different format
	/// </summary>
	/// <param name="source">The source surface to blit from</param>
	/// <param name="sourceRect">The source rectangle on the source surface to blit</param>
	/// <param name="leftWidth">The width, in pixels, of the left corners in <paramref name="sourceRect"/></param>
	/// <param name="rightWidth">The width, in pixels, of the right corners in <paramref name="sourceRect"/></param>
	/// <param name="topHeight">The height, in pixels, of the top corners in <paramref name="sourceRect"/></param>
	/// <param name="bottomHeight">The height, in pixels, of the bottom corners in <paramref name="sourceRect"/></param>
	/// <param name="scale">The scale to use to transform the corners of <paramref name="sourceRect"/> into the corners of this surface, or <c>0</c> for an unscaled blit</param>
	/// <param name="scaleMode">The scale mode to use for the blit</param>
	/// <returns><c><see langword="true"/></c>, if the blit was successful; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// The pixels in the source surface are split into a 3⨯3 grid, using the different corner sizes for each corner, and the sides and center making up the remaining pixels.
	/// The corners are then scaled using scale and fit into the corners of this surface.
	/// The sides and center are then stretched into place to cover the remaining portion of this surface.
	/// </para>
	/// <para>
	/// The region specified by <paramref name="sourceRect"/> from the <paramref name="source"/> surface is used as the 9-grid source and scaled to fit into the entire area of this surface.
	/// </para>
	/// </remarks>
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

	/// <summary>
	/// Tries to perform a 9-grid scaled blit from a specified source surface to this surface, which maybe of a different format
	/// </summary>
	/// <param name="source">The source surface to blit from</param>
	/// <param name="leftWidth">The width, in pixels, of the left corners in the <paramref name="source"/></param>
	/// <param name="rightWidth">The width, in pixels, of the right corners in the <paramref name="source"/></param>
	/// <param name="topHeight">The height, in pixels, of the top corners in the <paramref name="source"/></param>
	/// <param name="bottomHeight">The height, in pixels, of the bottom corners in the <paramref name="source"/></param>
	/// <param name="scale">The scale to use to transform the corners of the <paramref name="source"/> into the corners of this surface, or <c>0</c> for an unscaled blit</param>
	/// <param name="scaleMode">The scale mode to use for the blit</param>
	/// <returns><c><see langword="true"/></c>, if the blit was successful; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// The pixels in the source surface are split into a 3⨯3 grid, using the different corner sizes for each corner, and the sides and center making up the remaining pixels.
	/// The corners are then scaled using scale and fit into the corners of this surface.
	/// The sides and center are then stretched into place to cover the remaining portion of this surface.
	/// </para>
	/// <para>
	/// The entire <paramref name="source"/> surface is used as the 9-grid source and scaled to fit into the entire area of this surface.
	/// </para>
	/// </remarks>
	public bool TryBlit9Grid(Surface source, int leftWidth, int rightWidth, int topHeight, int bottomHeight, float scale, ScaleMode scaleMode)
	{
		unsafe
		{
			return SDL_BlitSurface9Grid(source is not null ? source.mSurface : null, srcrect: null, leftWidth, rightWidth, topHeight, bottomHeight, scale, scaleMode, mSurface, dstrect: null);
		}
	}

	/// <summary>
	/// Tries to perform a scaled blit from a specified source surface to this surface, which maybe of a different format
	/// </summary>
	/// <param name="destinationRect">The destination rectangle on this surface to blit onto</param>
	/// <param name="source">The source surface to blit from</param>
	/// <param name="sourceRect">The source rectangle on the source surface to blit</param>
	/// <param name="scaleMode">The scale mode to use for the blit</param>
	/// <returns><c><see langword="true"/></c>, if the blit was successful; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// The region specified by <paramref name="sourceRect"/> from the <paramref name="source"/> surface is used and scaled to fit into the region specified by <paramref name="destinationRect"/> on this surface.
	/// </para>
	/// </remarks>
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

	/// <summary>
	/// Tries to perform a scaled blit from a specified source surface to this surface, which maybe of a different format
	/// </summary>
	/// <param name="destinationRect">The destination rectangle on this surface to blit onto</param>
	/// <param name="source">The source surface to blit from</param>
	/// <param name="scaleMode">The scale mode to use for the blit</param>
	/// <returns><c><see langword="true"/></c>, if the blit was successful; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// The entire <paramref name="source"/> surface is used and scaled to fit into the region specified by <paramref name="destinationRect"/> on this surface.
	/// </para>
	/// </remarks>
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

	/// <summary>
	/// Tries to perform a scaled blit from a specified source surface to this surface, which maybe of a different format
	/// </summary>
	/// <param name="source">The source surface to blit from</param>
	/// <param name="sourceRect">The source rectangle on the source surface to blit</param>
	/// <param name="scaleMode">The scale mode to use for the blit</param>
	/// <returns><c><see langword="true"/></c>, if the blit was successful; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// The region specified by <paramref name="sourceRect"/> from the <paramref name="source"/> surface is used and scaled to fit into the entire area of this surface.
	/// </para>
	/// </remarks>
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

	/// <summary>
	/// Tries to perform a scaled blit from a specified source surface to this surface, which maybe of a different format
	/// </summary>
	/// <param name="source">The source surface to blit from</param>
	/// <param name="scaleMode">The scale mode to use for the blit</param>
	/// <returns><c><see langword="true"/></c>, if the blit was successful; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// The entire <paramref name="source"/> surface is used and scaled to fit into the entire area of this surface.
	/// </para>
	/// </remarks>
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

	public static bool TryConvert(int width, int height, PixelFormat sourceFormat, ReadOnlyNativeMemory source, int sourcePitch, PixelFormat destinationFormat, Utilities.NativeMemory destination, int destinationPitch)
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

	public static bool TryConvert(int width, int height, PixelFormat sourceFormat, ColorSpace sourceColorSpace, Properties? sourceProperties, ReadOnlyNativeMemory source, int sourcePitch, PixelFormat destinationFormat, ColorSpace destinationColorSpace, Properties? destinationProperties, Utilities.NativeMemory destination, int destinationPitch)
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
			// SDL_CreateSurfacePalette calls SDL_SetSurfacePalette, so we need to do the same kind of "reference rescuing" shenanigans 

			var oldPaletteRefCount = int.MaxValue;
			var oldPalette = SDL_GetSurfacePalette(mSurface);
			if (oldPalette is not null
				&& Palette.IsKnown(oldPalette)
				&& (oldPaletteRefCount = oldPalette->RefCount) is not > 1)
			{
				oldPalette->RefCount = 2;
			}

			var result = SDL_CreateSurfacePalette(mSurface);

			if (result is null)
			{
				if (oldPaletteRefCount is not > 1)
				{
					oldPalette->RefCount = oldPaletteRefCount;
				}

				palette = null;
				return false;
			}

			mPalette = palette = new Palette(result); // we actually want to override any existing managed Palette here
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

	public static bool TryPremultiplyAlpha(int width, int height, PixelFormat sourceFormat, ReadOnlyNativeMemory source, int sourcePitch, PixelFormat destinationFormat, Utilities.NativeMemory destination, int destinationPitch, bool linear)
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
			color = Color.From(r, g, b, a);
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
