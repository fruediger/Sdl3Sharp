using System;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Video.Coloring;

/// <remarks>
/// <para>
/// SDL's pixel formats have the following naming convention:
/// <list type="bullet">
///		<item>
///			<description>
///				Names with a list of components and a single bit count, such as <see cref="Rgb24"/> and <see cref="Abgr32"/>, define a platform-independent encoding into bytes in the order specified.
///				For example, in <see cref="Rgb24"/> data, each pixel is encoded in 3 bytes (red, green, blue) in that order, and in <see cref="Abgr32"/> data, each pixel is encoded in 4 bytes (alpha, blue, green, red) in that order.
///				Use these names if the property of a format that is important to you is the order of the bytes in memory or on disk.
///			</description>
///		</item>
///		<item>
///			<description>
///				 Names with a bit count per component, such as <see cref="Argb8888"/> and <see cref="Xrgb1555"/>, are "packed" into an appropriately-sized integer in the platform's native endianness.
///				 For example, <see cref="Argb8888"/> is a sequence of 32-bit integers; in each integer, the most significant bits are alpha, and the least significant bits are blue.
///				 On a little-endian CPU such as x86, the least significant bits of each integer are arranged first in memory, but on a big-endian CPU such as s390x, the most significant bits are arranged first.
///				 Use these names if the property of a format that is important to you is the meaning of each bit position within a native-endianness integer.
///			</description>
///		</item>
///		<item>
///			<description>
///				In indexed formats such as <see cref="Index4Lsb"/>, each pixel is represented by encoding an index into the <see cref="Palette"/> into the indicated number of bits, with multiple pixels packed into each byte if appropriate.
///				In "Lsb" formats, the first (leftmost) pixel is stored in the least-significant bits of the byte; in "Msb" formats, it's stored in the most-significant bits.
///				<see cref="Index8"/> does not need "Lsb"/"Msb" variants, because each pixel exactly fills one byte.
///			</description>
///		</item>
/// </list>
/// The 32-bit byte-array encodings such as <see cref="Rgba32"/> are aliases for the appropriate "8888" encoding for the current platform.
/// For example, <see cref="Rgba32"/> is an alias for <see cref="Abgr8888"/> on little-endian CPUs like x86, or an alias for <see cref="Rgba8888"/> on big-endian CPUs.
/// </para>
/// </remarks>
partial struct PixelFormat
{
#pragma warning disable CS1591 // Not sure if it's actually necessary and reasonable to document all of these predefined formats

	/// <summary>The unknown <see cref="PixelFormat"/></summary>
	/// <remarks>
	/// <para>
	/// This is also the <see langword="default"/> value of <see cref="PixelFormat"/>.
	/// </para>
	/// </remarks>
	public static PixelFormat Unknown { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(0); }

	public static PixelFormat Index1Lsb { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(PixelType.Index1, BitmapOrder._4321, PackedLayout.None, 1, 0); }

	public static PixelFormat Index1Msb { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(PixelType.Index1, BitmapOrder._1234, PackedLayout.None, 1, 0); }

	public static PixelFormat Index2Lsb { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(PixelType.Index2, BitmapOrder._4321, PackedLayout.None, 2, 0); }

	public static PixelFormat Index2Msb { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(PixelType.Index2, BitmapOrder._1234, PackedLayout.None, 2, 0); }

	public static PixelFormat Index4Lsb { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(PixelType.Index4, BitmapOrder._4321, PackedLayout.None, 4, 0); }

	public static PixelFormat Index4Msb { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(PixelType.Index4, BitmapOrder._1234, PackedLayout.None, 4, 0); }

	public static PixelFormat Index8 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(PixelType.Index8, BitmapOrder.None, PackedLayout.None, 8, 1); }

	public static PixelFormat Rgb332 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(PixelType.Packed8, PackedOrder.Xrgb, PackedLayout._332, 8, 1); }

	public static PixelFormat Xrgb4444 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(PixelType.Packed16, PackedOrder.Xrgb, PackedLayout._4444, 12, 2); }

	public static PixelFormat Xbgr4444 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(PixelType.Packed16, PackedOrder.Xbgr, PackedLayout._4444, 12, 2); }

	public static PixelFormat Xrgb1555 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(PixelType.Packed16, PackedOrder.Xrgb, PackedLayout._1555, 15, 2); }

	public static PixelFormat Xbgr1555 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(PixelType.Packed16, PackedOrder.Xbgr, PackedLayout._1555, 15, 2); }

	public static PixelFormat Argb4444 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(PixelType.Packed16, PackedOrder.Argb, PackedLayout._4444, 16, 2); }

	public static PixelFormat Rgba4444 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(PixelType.Packed16, PackedOrder.Rgba, PackedLayout._4444, 16, 2); }

	public static PixelFormat Abgr4444 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(PixelType.Packed16, PackedOrder.Abgr, PackedLayout._4444, 16, 2); }

	public static PixelFormat Bgra4444 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(PixelType.Packed16, PackedOrder.Bgra, PackedLayout._4444, 16, 2); }

	public static PixelFormat Argb1555 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(PixelType.Packed16, PackedOrder.Argb, PackedLayout._1555, 16, 2); }

	public static PixelFormat Rgba5551 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(PixelType.Packed16, PackedOrder.Rgba, PackedLayout._5551, 16, 2); }

	public static PixelFormat Abgr1555 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(PixelType.Packed16, PackedOrder.Abgr, PackedLayout._1555, 16, 2); }

	public static PixelFormat Bgra5551 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(PixelType.Packed16, PackedOrder.Bgra, PackedLayout._5551, 16, 2); }

	public static PixelFormat Rgb565 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(PixelType.Packed16, PackedOrder.Xrgb, PackedLayout._565, 16, 2); }

	public static PixelFormat Bgr565 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(PixelType.Packed16, PackedOrder.Xbgr, PackedLayout._565, 16, 2); }

	public static PixelFormat Rgb24 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(PixelType.ArrayU8, ArrayOrder.Rgb, PackedLayout.None, 24, 3); }

	public static PixelFormat Bgr24 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(PixelType.ArrayU8, ArrayOrder.Bgr, PackedLayout.None, 24, 3); }

	public static PixelFormat Xrgb8888 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(PixelType.Packed32, PackedOrder.Xrgb, PackedLayout._8888, 24, 4); }

	public static PixelFormat Rgbx8888 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(PixelType.Packed32, PackedOrder.Rgbx, PackedLayout._8888, 24, 4); }

	public static PixelFormat Xbgr8888 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(PixelType.Packed32, PackedOrder.Xbgr, PackedLayout._8888, 24, 4); }

	public static PixelFormat Bgrx8888 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(PixelType.Packed32, PackedOrder.Bgrx, PackedLayout._8888, 24, 4); }

	public static PixelFormat Argb8888 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(PixelType.Packed32, PackedOrder.Argb, PackedLayout._8888, 32, 4); }

	public static PixelFormat Rgba8888 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(PixelType.Packed32, PackedOrder.Rgba, PackedLayout._8888, 32, 4); }

	public static PixelFormat Abgr8888 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(PixelType.Packed32, PackedOrder.Abgr, PackedLayout._8888, 32, 4); }

	public static PixelFormat Bgra8888 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(PixelType.Packed32, PackedOrder.Bgra, PackedLayout._8888, 32, 4); }

	public static PixelFormat Xrgb2101010 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(PixelType.Packed32, PackedOrder.Xrgb, PackedLayout._2101010, 32, 4); }

	public static PixelFormat Xbgr2101010 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(PixelType.Packed32, PackedOrder.Xbgr, PackedLayout._2101010, 32, 4); }

	public static PixelFormat Argb2101010 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(PixelType.Packed32, PackedOrder.Argb, PackedLayout._2101010, 32, 4); }

	public static PixelFormat Abgr2101010 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(PixelType.Packed32, PackedOrder.Abgr, PackedLayout._2101010, 32, 4); }

	public static PixelFormat Rgb48 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(PixelType.ArrayU16, ArrayOrder.Rgb, PackedLayout.None, 48, 6); }

	public static PixelFormat Bgr48 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(PixelType.ArrayU16, ArrayOrder.Bgr, PackedLayout.None, 48, 6); }

	public static PixelFormat Rgba64 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(PixelType.ArrayU16, ArrayOrder.Rgba, PackedLayout.None, 64, 8); }

	public static PixelFormat Argb64 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(PixelType.ArrayU16, ArrayOrder.Argb, PackedLayout.None, 64, 8); }

	public static PixelFormat Bgra64 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(PixelType.ArrayU16, ArrayOrder.Bgra, PackedLayout.None, 64, 8); }

	public static PixelFormat Abgr64 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(PixelType.ArrayU16, ArrayOrder.Abgr, PackedLayout.None, 64, 8); }

	public static PixelFormat Rgb48Float { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(PixelType.ArrayF16, ArrayOrder.Rgb, PackedLayout.None, 48, 6); }

	public static PixelFormat Bgr48Float { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(PixelType.ArrayF16, ArrayOrder.Bgr, PackedLayout.None, 48, 6); }

	public static PixelFormat Rgba64Float { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(PixelType.ArrayF16, ArrayOrder.Rgba, PackedLayout.None, 64, 8); }

	public static PixelFormat Argb64Float { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(PixelType.ArrayF16, ArrayOrder.Argb, PackedLayout.None, 64, 8); }

	public static PixelFormat Bgra64Float { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(PixelType.ArrayF16, ArrayOrder.Bgra, PackedLayout.None, 64, 8); }

	public static PixelFormat Abgr64Float { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(PixelType.ArrayF16, ArrayOrder.Abgr, PackedLayout.None, 64, 8); }

	public static PixelFormat Rgb96Float { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(PixelType.ArrayF32, ArrayOrder.Rgb, PackedLayout.None, 96, 12); }

	public static PixelFormat Bgr96Float { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(PixelType.ArrayF32, ArrayOrder.Bgr, PackedLayout.None, 96, 12); }

	public static PixelFormat Rgba128Float { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(PixelType.ArrayF32, ArrayOrder.Rgba, PackedLayout.None, 128, 16); }

	public static PixelFormat Argb128Float { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(PixelType.ArrayF32, ArrayOrder.Argb, PackedLayout.None, 128, 16); }

	public static PixelFormat Bgra128Float { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(PixelType.ArrayF32, ArrayOrder.Bgra, PackedLayout.None, 128, 16); }

	public static PixelFormat Abgr128Float { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(PixelType.ArrayF32, ArrayOrder.Abgr, PackedLayout.None, 128, 16); }

	/// <summary>Planar mode: Y + V + U  (3 planes)</summary>
	public static PixelFormat Yv12 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new('Y', 'V', '1', '2'); }

	/// <summary>Planar mode: Y + U + V  (3 planes)</summary>
	public static PixelFormat Iyuv { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new('I', 'Y', 'U', 'V'); }

	/// <summary>Packed mode: Y0 + U0 + Y1 + V0 (1 plane)</summary>
	public static PixelFormat Yuy2 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new('Y', 'U', 'Y', '2'); }

	/// <summary>Packed mode: U0 + Y0 + V0 + Y1 (1 plane)</summary>
	public static PixelFormat Uyvy { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new('U', 'Y', 'V', 'Y'); }

	/// <summary>Packed mode: Y0 + V0 + Y1 + U0 (1 plane)</summary>
	public static PixelFormat Yvyu { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new('Y', 'V', 'Y', 'U'); }

	/// <summary>Planar mode: Y + U/V interleaved  (2 planes)</summary>
	public static PixelFormat Nv12 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new('N', 'V', '1', '2'); }

	/// <summary>Planar mode: Y + V/U interleaved  (2 planes)</summary>
	public static PixelFormat Nv21 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new('N', 'V', '2', '1'); }

	/// <summary>Planar mode: Y + U/V interleaved  (2 planes)</summary>
	public static PixelFormat P010 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new('P', '0', '1', '0'); }

	/// <summary>Android video texture format</summary>
	public static PixelFormat ExternalOes { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new('O', 'E', 'S', ' '); }

	/// <summary>Motion JPEG</summary>
	public static PixelFormat Mjpg { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new('M', 'J', 'P', 'G'); }

	/// <remarks>
	/// <para>
	/// <see cref="Abgr8888"/> on little-endian platforms, <see cref="Rgba8888"/> on big-endian platforms.
	/// </para>
	/// </remarks>
	public static PixelFormat Rgba32
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		get => BitConverter.IsLittleEndian
			? Abgr8888
			: Rgba8888;
	}

	/// <remarks>
	/// <para>
	/// <see cref="Bgra8888"/> on little-endian platforms, <see cref="Argb8888"/> on big-endian platforms.
	/// </para>
	/// </remarks>
	public static PixelFormat Argb32
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		get => BitConverter.IsLittleEndian
			? Bgra8888
			: Argb8888;
	}

	/// <remarks>
	/// <para>
	/// <see cref="Argb8888"/> on little-endian platforms, <see cref="Bgra8888"/> on big-endian platforms.
	/// </para>
	/// </remarks>
	public static PixelFormat Bgra32
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		get => BitConverter.IsLittleEndian
			? Argb8888
			: Bgra8888;
	}

	/// <remarks>
	/// <para>
	/// <see cref="Rgba8888"/> on little-endian platforms, <see cref="Abgr8888"/> on big-endian platforms.
	/// </para>
	/// </remarks>
	public static PixelFormat Abgr32
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		get => BitConverter.IsLittleEndian
			? Rgba8888
			: Abgr8888;
	}

	/// <remarks>
	/// <para>
	/// <see cref="Xbgr8888"/> on little-endian platforms, <see cref="Rgbx8888"/> on big-endian platforms.
	/// </para>
	/// </remarks>
	public static PixelFormat Rgbx32
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		get => BitConverter.IsLittleEndian
			? Xbgr8888
			: Rgbx8888;
	}

	/// <remarks>
	/// <para>
	/// <see cref="Bgrx8888"/> on little-endian platforms, <see cref="Xrgb8888"/> on big-endian platforms.
	/// </para>
	/// </remarks>
	public static PixelFormat Xrgb32
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		get => BitConverter.IsLittleEndian
			? Bgrx8888
			: Xrgb8888;
	}

	/// <remarks>
	/// <para>
	/// <see cref="Xrgb8888"/> on little-endian platforms, <see cref="Bgrx8888"/> on big-endian platforms.
	/// </para>
	/// </remarks>
	public static PixelFormat Bgrx32
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		get => BitConverter.IsLittleEndian
			? Xrgb8888
			: Bgrx8888;
	}

	/// <remarks>
	/// <para>
	/// <see cref="Rgbx8888"/> on little-endian platforms, <see cref="Xbgr8888"/> on big-endian platforms.
	/// </para>
	/// </remarks>
	public static PixelFormat Xbgr32
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		get => BitConverter.IsLittleEndian
			? Rgbx8888
			: Xbgr8888;
	}

#pragma warning restore CS1591
}
