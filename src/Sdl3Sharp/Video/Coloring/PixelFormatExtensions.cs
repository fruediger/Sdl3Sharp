using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.Marshalling;

namespace Sdl3Sharp.Video.Coloring;

/// <summary>
/// Provides extension methods and properties for <see cref="PixelFormat"/>
/// </summary>
public static partial class PixelFormatExtensions
{
	extension(PixelFormat)
	{
#pragma warning disable IDE0079 // Leave this here to remind ourselves in case we want to document these in the future
#pragma warning disable CS1591 // Not sure if it's actually necessary and reasonable to document all of these predefined formats

		/// <remarks>
		/// <para>
		/// <see cref="PixelFormat.Abgr8888"/> on little-endian platforms, <see cref="PixelFormat.Rgba8888"/> on big-endian platforms.
		/// </para>
		/// </remarks>
		public static PixelFormat Rgba32
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
			get => BitConverter.IsLittleEndian
				? PixelFormat.Abgr8888
				: PixelFormat.Rgba8888;
		}

		/// <remarks>
		/// <para>
		/// <see cref="PixelFormat.Bgra8888"/> on little-endian platforms, <see cref="PixelFormat.Argb8888"/> on big-endian platforms.
		/// </para>
		/// </remarks>
		public static PixelFormat Argb32
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
			get => BitConverter.IsLittleEndian
				? PixelFormat.Bgra8888
				: PixelFormat.Argb8888;
		}

		/// <remarks>
		/// <para>
		/// <see cref="PixelFormat.Argb8888"/> on little-endian platforms, <see cref="PixelFormat.Bgra8888"/> on big-endian platforms.
		/// </para>
		/// </remarks>
		public static PixelFormat Bgra32
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
			get => BitConverter.IsLittleEndian
				? PixelFormat.Argb8888
				: PixelFormat.Bgra8888;
		}

		/// <remarks>
		/// <para>
		/// <see cref="PixelFormat.Rgba8888"/> on little-endian platforms, <see cref="PixelFormat.Abgr8888"/> on big-endian platforms.
		/// </para>
		/// </remarks>
		public static PixelFormat Abgr32
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
			get => BitConverter.IsLittleEndian
				? PixelFormat.Rgba8888
				: PixelFormat.Abgr8888;
		}

		/// <remarks>
		/// <para>
		/// <see cref="PixelFormat.Xbgr8888"/> on little-endian platforms, <see cref="PixelFormat.Rgbx8888"/> on big-endian platforms.
		/// </para>
		/// </remarks>
		public static PixelFormat Rgbx32
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
			get => BitConverter.IsLittleEndian
				? PixelFormat.Xbgr8888
				: PixelFormat.Rgbx8888;
		}

		/// <remarks>
		/// <para>
		/// <see cref="PixelFormat.Bgrx8888"/> on little-endian platforms, <see cref="PixelFormat.Xrgb8888"/> on big-endian platforms.
		/// </para>
		/// </remarks>
		public static PixelFormat Xrgb32
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
			get => BitConverter.IsLittleEndian
				? PixelFormat.Bgrx8888
				: PixelFormat.Xrgb8888;
		}

		/// <remarks>
		/// <para>
		/// <see cref="PixelFormat.Xrgb8888"/> on little-endian platforms, <see cref="PixelFormat.Bgrx8888"/> on big-endian platforms.
		/// </para>
		/// </remarks>
		public static PixelFormat Bgrx32
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
			get => BitConverter.IsLittleEndian
				? PixelFormat.Xrgb8888
				: PixelFormat.Bgrx8888;
		}

		/// <remarks>
		/// <para>
		/// <see cref="PixelFormat.Rgbx8888"/> on little-endian platforms, <see cref="PixelFormat.Xbgr8888"/> on big-endian platforms.
		/// </para>
		/// </remarks>
		public static PixelFormat Xbgr32
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
			get => BitConverter.IsLittleEndian
				? PixelFormat.Rgbx8888
				: PixelFormat.Xbgr8888;
		}

#pragma warning restore CS1591
#pragma warning restore IDE0079

		/// <summary>
		/// Creates new <see cref="PixelFormat"/> from specified type, order, layout, bits-per-pixel and bytes-per-pixel values
		/// </summary>
		/// <param name="type">
		/// The type of the pixel format.
		/// If the <paramref name="order"/> is an <see cref="ArrayOrder"/> value, this should be one of the <c><see cref="PixelType"/>.Array*</c> values,
		/// if the <paramref name="order"/> is a <see cref="BitmapOrder"/> value, this should be one of the <c><see cref="PixelType"/>.Index*</c> values,
		/// or, if the <paramref name="order"/> is a <see cref="PackedOrder"/> value, this should be one of the <c><see cref="PixelType"/>.Packed*</c> values.
		/// </param>
		/// <param name="order">The order of the pixel format. Should be either an <see cref="ArrayOrder"/> value, a <see cref="BitmapOrder"/> value, or a <see cref="PackedOrder"/> value.</param>
		/// <param name="layout">The packed layout of the packed pixel format. If the <paramref name="order"/> is not a <see cref="PackedOrder"/> value, this should be <see cref="PackedLayout.None"/>.</param>
		/// <param name="bitsPerPixel">The number of bits per pixel of the pixel format</param>
		/// <param name="bytesPerPixel">The number of bytes per pixel of the pixel format</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static PixelFormat Custom(PixelType type, int order, PackedLayout layout, int bitsPerPixel, int bytesPerPixel)
			=> unchecked((PixelFormat)(
				  (1u << 28)
				| (((uint)type & 0x0fu) << 24)
				| (((uint)order & 0x0fu) << 20)
				| (((uint)layout & 0x0fu) << 16)
				| (((uint)bitsPerPixel & 0xffu) << 8)
				| (((uint)bytesPerPixel & 0xffu) << 0)
			));

		/// <summary>
		/// Creates new <see cref="PixelFormat"/> from specified type, order, layout, bits-per-pixel and bytes-per-pixel values
		/// </summary>
		/// <param name="type">The type of the pixel format. Should be one of the <c><see cref="PixelType"/>.Array*</c> values.</param>
		/// <param name="order">The component order of the array pixel format</param>
		/// <param name="layout">The packed layout of the pixel format. Should be <c><see cref="PackedLayout.None"/></c>.</param>
		/// <param name="bitsPerPixel">The number of bits per pixel of the pixel format</param>
		/// <param name="bytesPerPixel">The number of bytes per pixel of the pixel format</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static PixelFormat Custom(PixelType type, ArrayOrder order, PackedLayout layout, int bitsPerPixel, int bytesPerPixel)
			=> Custom(type, unchecked((int)order), layout, bitsPerPixel, bytesPerPixel);

		/// <summary>
		/// Creates new <see cref="PixelFormat"/> from specified type, order, layout, bits-per-pixel and bytes-per-pixel values
		/// </summary>
		/// <param name="type">The type of the pixel format. Should be one of the <c><see cref="PixelType"/>.Index*</c> values.</param>
		/// <param name="order">The bit order of the pixel format</param>
		/// <param name="layout">The packed layout of the pixel format. Should be <c><see cref="PackedLayout.None"/></c>.</param>
		/// <param name="bitsPerPixel">The number of bits per pixel of the pixel format</param>
		/// <param name="bytesPerPixel">The number of bytes per pixel of the pixel format</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static PixelFormat Custom(PixelType type, BitmapOrder order, PackedLayout layout, int bitsPerPixel, int bytesPerPixel)
			=> Custom(type, unchecked((int)order), layout, bitsPerPixel, bytesPerPixel);

		/// <summary>
		/// Creates new <see cref="PixelFormat"/> from specified type, order, layout, bits-per-pixel and bytes-per-pixel values
		/// </summary>
		/// <param name="type">The type of the pixel format. Should be one of the <c><see cref="PixelType"/>.Packed*</c> values.</param>
		/// <param name="order">The component order of the packed pixel format</param>
		/// <param name="layout">The packed layout of the packed pixel format</param>
		/// <param name="bitsPerPixel">The number of bits per pixel of the pixel format</param>
		/// <param name="bytesPerPixel">The number of bytes per pixel of the pixel format</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static PixelFormat Custom(PixelType type, PackedOrder order, PackedLayout layout, int bitsPerPixel, int bytesPerPixel)
			=> Custom(type, unchecked((int)order), layout, bitsPerPixel, bytesPerPixel);

		/// <summary>
		/// Creates new <see cref="PixelFormat"/> in a "FourCC" format
		/// </summary>
		/// <param name="a">The first character of the "FourCC" format</param>
		/// <param name="b">The second character of the "FourCC" format</param>
		/// <param name="c">The third character of the "FourCC" format</param>
		/// <param name="d">The fourth character of the "FourCC" format</param>
		/// <remarks>
		/// <para>
		/// "FourCC" formats are special pixel formats which are identified by a four-character code.
		/// Those are used to cover custom and other unusual pixel formats.
		/// </para>
		/// </remarks>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static PixelFormat Custom(char a, char b, char c, char d)
			=> unchecked((PixelFormat)(
				  (uint)(byte)a << 0
				| (uint)(byte)b << 8
				| (uint)(byte)c << 16
				| (uint)(byte)d << 24
			));

		/// <summary>
		/// Tries to convert a bits-per-pixel value and RGBA masks to a pixel format
		/// </summary>
		/// <param name="bitsPerPixel">The bits-per-pixel value of the pixel format. This is usually <c>15</c>, <c>16</c>, or <c>32</c>.</param>
		/// <param name="rMask">A red component bit mask for the pixel format</param>
		/// <param name="gMask">A green component bit mask for the pixel format</param>
		/// <param name="bMask">A blue component bit mask for the pixel format</param>
		/// <param name="aMask">A alpha component bit mask for the pixel format</param>
		/// <param name="format">The resulting <see cref="PixelFormat"/> corresponding to the specified bits-per-pixel value and RGBA masks, if this method returns <c><see langword="true"/></c>; otherwise, <see cref="PixelFormat.Unknown"/></param>
		/// <returns><c><see langword="true"/></c>, if the provided bits-per-pixel value together with the provided RGBA masks were successfully converted into a pixel format; otherwise, <c><see langword="false"/></c></returns>
		public static bool TryGetFromMasks(int bitsPerPixel, uint rMask, uint gMask, uint bMask, uint aMask, out PixelFormat format)
		{
			format = SDL_GetPixelFormatForMasks(bitsPerPixel, rMask, gMask, bMask, aMask);

			return format is not PixelFormat.Unknown;
		}
	}

	extension(PixelFormat format)
	{
		/// <summary>
		/// Gets the number of bits per pixel
		/// </summary>
		/// <value>
		/// The number of bits per pixel
		/// </value>
		public int BitsPerPixel
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
			get => format.IsFourCC
				? 0
				: unchecked((int)(((uint)format >> 8) & 0xff));
		}

		/// <summary>
		/// Gets the number of bytes per pixel
		/// </summary>
		/// <value>
		/// The number of bytes per pixel
		/// </value>
		public int BytesPerPixel
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
			get => format.IsFourCC
				? format is PixelFormat.Yuy2
				         or PixelFormat.Uyvy
				         or PixelFormat.Yvyu
				         or PixelFormat.P010
					? 2
					: 1
				: unchecked((int)(((uint)format >> 0) & 0xff));
		}

		private uint Flag { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => unchecked(((uint)format >> 28) & 0x0f); }

		/// <summary>
		/// Gets a value indicating whether the pixel format is a 10-bit per component format
		/// </summary>
		/// <value>
		/// A value indicating whether the pixel format is a 10-bit per component format
		/// </value>
		public bool Is10Bit
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
			get => !format.IsFourCC
				&& format.Type is PixelType.Packed32
				&& format.Layout is PackedLayout._2101010;
		}

		/// <summary>
		/// Gets a value indicating whether the pixel format contains an alpha component
		/// </summary>
		/// <value>
		/// A value indicating whether the pixel format contains an alpha component
		/// </value>
		public bool IsAlpha
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
			get => format.IsPacked
				? unchecked((PackedOrder)format.Order) is PackedOrder.Argb
												       or PackedOrder.Rgba
												       or PackedOrder.Abgr
												       or PackedOrder.Bgra
				: format.IsArray
				&& unchecked((ArrayOrder)format.Order) is ArrayOrder.Argb
												       or ArrayOrder.Rgba
												       or ArrayOrder.Abgr
												       or ArrayOrder.Bgra;
		}

		/// <summary>
		/// Gets a value indicating whether the pixel format is an array format
		/// </summary>
		/// <value>
		/// A value indicating whether the pixel format is an array format
		/// </value>
		public bool IsArray
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
			get => !format.IsFourCC
				&& format.Type is PixelType.ArrayU8
						       or PixelType.ArrayU16
						       or PixelType.ArrayU32
						       or PixelType.ArrayF16
						       or PixelType.ArrayF32;
		}

		/// <summary>
		/// Gets a value indicating whether the pixel format is a floating-point format
		/// </summary>
		/// <value>
		/// A value indicating whether the pixel format is a floating-point format
		/// </value>
		public bool IsFloat
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
			get => !format.IsFourCC
				&& format.Type is PixelType.ArrayF16
						       or PixelType.ArrayF32;
		}

		/// <summary>
		/// Get a value indicating whether the pixel format is in a "FourCC" format
		/// </summary>
		/// <value>
		/// A value indicating whether the pixel format is in a "FourCC" format
		/// </value>
		/// <remarks>
		/// <para>
		/// "FourCC" formats are special pixel formats which are identified by a four-character code.
		/// Those are used to cover custom and other unusual pixel formats.
		/// </para>
		/// </remarks>
		public bool IsFourCC
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
			get => format is not 0
				&& format.Flag is not 1;
		}

		/// <summary>
		/// Gets a value indicating whether the pixel format is an indexed format
		/// </summary>
		/// <value>
		/// A value indicating whether the pixel format is an indexed format
		/// </value>
		/// <remarks>
		/// <para>
		/// Indexed formats should be used in conjunction with a <see cref="Palette"/>.
		/// </para>
		/// </remarks>
		public bool IsIndexed
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
			get => !format.IsFourCC
				&& format.Type is PixelType.Index1
						       or PixelType.Index2
						       or PixelType.Index4
						       or PixelType.Index8;
		}

		/// <summary>
		/// Gets a value indicating whether the pixel format is a packed format
		/// </summary>
		/// <value>
		/// A value indicating whether the pixel format is a packed format
		/// </value>
		public bool IsPacked
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
			get => !format.IsFourCC
				&& format.Type is PixelType.Packed8
						       or PixelType.Packed16
						       or PixelType.Packed32;
		}

		/// <summary>
		/// Gets the layout of a packed pixel format
		/// </summary>
		/// <value>
		/// The layout of a packed pixel format
		/// </value>
		/// <remarks>
		/// <para>
		/// Note: The value of this property is only valid if <see cref="get_IsFourCC(PixelFormat)"/> is <c><see langword="false"/></c>.
		/// </para>
		/// <para>
		/// If the pixel format is not a packed format (<see cref="get_IsPacked(PixelFormat)"/> is <c><see langword="false"/></c>), the value of this property should be <see cref="PackedLayout.None"/>.
		/// </para>
		/// </remarks>
		public PackedLayout Layout { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => unchecked((PackedLayout)(((uint)format >> 16) & 0x0f)); }

		/// <summary>
		/// Gets the component or bit order of the pixel format
		/// </summary>
		/// <value>
		/// The component or bit order of the pixel format
		/// </value>
		/// <remarks> 
		/// <para>
		/// Note: The value of this property is only valid if <see cref="get_IsFourCC(PixelFormat)"/> is <c><see langword="false"/></c>.
		/// </para>
		/// <para>
		/// The value of this property can be cast to either <see cref="BitmapOrder"/>, <see cref="PackedOrder"/>, or <see cref="ArrayOrder"/>, depending on the <see cref="get_Type(PixelFormat)">type of pixel format</see>.
		/// <list type="bullet">
		///		<item>
		///			<term><see cref="get_IsIndexed(PixelFormat)"/> is <c><see langword="true"/></c></term>
		///			<description>The value of this property should be interpreted as/cast to a <see cref="BitmapOrder"/> value</description>
		///		</item>
		///		<item>
		///			<term><see cref="get_IsPacked(PixelFormat)"/> is <c><see langword="true"/></c></term>
		///			<description>The value of this property should be interpreted as/cast to a <see cref="PackedOrder"/> value</description>
		///		</item>
		///		<item>
		///			<term><see cref="get_IsArray(PixelFormat)"/> is <c><see langword="true"/></c></term>
		///			<description>The value of this property should be interpreted as/cast to an <see cref="ArrayOrder"/> value</description>
		///		</item>
		/// </list>
		/// </para>
		/// </remarks>
		public int Order { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => unchecked((int)((uint)format >> 20) & 0x0f); }

		/// <summary>
		/// Gets the type of the pixel format
		/// </summary>
		/// <value>
		/// The type of the pixel format
		/// </value>
		/// <remarks>
		/// <para>
		/// Note: The value of this property is only valid if <see cref="get_IsFourCC(PixelFormat)"/> is <c><see langword="false"/></c>.
		/// </para>
		/// </remarks>
		public PixelType Type { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => unchecked((PixelType)(((uint)format >> 24) & 0x0f)); }

		/// <summary>
		/// Gets a human-readable name for the pixel format
		/// </summary>
		/// <returns>A human-readable name for the pixel format, or <c>"SDL_PIXELFORMAT_UNKNOWN"</c> if the format isn't recognized</returns>
		public string GetName()
		{
			unsafe
			{
				return Utf8StringMarshaller.ConvertToManaged(SDL_GetPixelFormatName(format))!; // 'SDL_GetPixelFormatName' never returns null
			}
		}

		/// <summary>
		/// Tries to convert the pixel format to a bits-per-pixel value and RGBA masks
		/// </summary>
		/// <param name="bitsPerPixel">The bits-per-pixel value of the pixel format. This is usually <c>15</c>, <c>16</c>, or <c>32</c>.</param>
		/// <param name="rMask">A red component bit mask for the pixel format</param>
		/// <param name="gMask">A green component bit mask for the pixel format</param>
		/// <param name="bMask">A blue component bit mask for the pixel format</param>
		/// <param name="aMask">A alpha component bit mask for the pixel format</param>
		/// <returns><c><see langword="true"/></c>, if the pixel format was successfully converted into a bits-per-pixel value and RGBA masks; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
		public bool TryGetMasks(out int bitsPerPixel, out uint rMask, out uint gMask, out uint bMask, out uint aMask)
		{
			unsafe
			{
				int bpp;
				uint rmask, gmask, bmask, amask;

				bool result = SDL_GetMasksForPixelFormat(format, &bpp, &rmask, &gmask, &bmask, &amask);

				bitsPerPixel = bpp;
				rMask = rmask; gMask = gmask; bMask = bmask; aMask = amask;

				return result;
			}
		}

		/// <summary>
		/// Tries to get <see cref="PixelFormatDetails"/> for the pixel format
		/// </summary>
		/// <param name="formatDetails"><see cref="PixelFormatDetails"/> for the pixel format, if this method returns <c><see langword="true"/></c>; otherwise, the resulting <see cref="PixelFormatDetails"/> might not be valid</param>
		/// <returns><c><see langword="true"/></c>, if the <see cref="PixelFormatDetails"/> for the pixel format were successfully retrieved; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
		public bool TryGetPixelFormatDetails(out PixelFormatDetails formatDetails)
		{
			unsafe
			{
				var formatDetailsPtr = SDL_GetPixelFormatDetails(format);

				formatDetails = new(formatDetailsPtr);

				return formatDetailsPtr is not null;
			}
		}
	}
}
