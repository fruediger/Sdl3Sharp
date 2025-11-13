using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
using System.Text;

namespace Sdl3Sharp.Video.Coloring;

/// <summary>
/// Represents a pixel format
/// </summary>
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
[StructLayout(LayoutKind.Sequential)]
public readonly partial struct PixelFormat :
	IEquatable<PixelFormat>, IFormattable, ISpanFormattable, IEqualityOperators<PixelFormat, PixelFormat, bool>
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly string DebuggerDisplay => ToString();

	private readonly uint mFormat;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private PixelFormat(uint format) => mFormat = format;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private PixelFormat(byte a, byte b, byte c, byte d) :
		this(unchecked(
			  ((uint)a << 0)
			| ((uint)b << 8)
			| ((uint)c << 16)
			| ((uint)d << 24)
		))
	{ }

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
	public PixelFormat(char a, char b, char c, char d) :
		this(unchecked((byte)a), unchecked((byte)b), unchecked((byte)c), unchecked((byte)d))
	{ }

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private PixelFormat(PixelType type, int order, PackedLayout layout, int bitsPerPixel, int bytesPerPixel) :
		this(unchecked(
			  (1u                            << 28)
			| (((uint)(int)type     & 0x0fu) << 24)
			| (((uint)order         & 0x0fu) << 20)
			| (((uint)layout        & 0x0fu) << 16)
			| (((uint)bitsPerPixel  & 0xffu) << 8)
			| (((uint)bytesPerPixel & 0xffu) << 0)
		))
	{ }

	/// <summary>
	/// Creates new <see cref="PixelFormat"/> from specified type, order, layout, bits-per-pixel and bytes-per-pixel values
	/// </summary>
	/// <param name="type">The type of the pixel format. Should be one of the <c><see cref="PixelType"/>.Index*</c> values.</param>
	/// <param name="order">The bit order of the pixel format</param>
	/// <param name="layout">The packed layout of the pixel format. Should be <c><see cref="PackedLayout.None"/></c>.</param>
	/// <param name="bitsPerPixel">The number of bits per pixel of the pixel format</param>
	/// <param name="bytesPerPixel">The number of bytes per pixel of the pixel format</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public PixelFormat(PixelType type, BitmapOrder order, PackedLayout layout, int bitsPerPixel, int bytesPerPixel) :
		this(type, unchecked((int)order), layout, bitsPerPixel, bytesPerPixel)
	{ }

	/// <summary>
	/// Creates new <see cref="PixelFormat"/> from specified type, order, layout, bits-per-pixel and bytes-per-pixel values
	/// </summary>
	/// <param name="type">The type of the pixel format. Should be one of the <c><see cref="PixelType"/>.Packed*</c> values.</param>
	/// <param name="order">The component order of the packed pixel format</param>
	/// <param name="layout">The packed layout of the packed pixel format</param>
	/// <param name="bitsPerPixel">The number of bits per pixel of the pixel format</param>
	/// <param name="bytesPerPixel">The number of bytes per pixel of the pixel format</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public PixelFormat(PixelType type, PackedOrder order, PackedLayout layout, int bitsPerPixel, int bytesPerPixel) :
		this(type, unchecked((int)order), layout, bitsPerPixel, bytesPerPixel)
	{ }

	/// <summary>
	/// Creates new <see cref="PixelFormat"/> from specified type, order, layout, bits-per-pixel and bytes-per-pixel values
	/// </summary>
	/// <param name="type">The type of the pixel format. Should be one of the <c><see cref="PixelType"/>.Array*</c> values.</param>
	/// <param name="order">The component order of the array pixel format</param>
	/// <param name="layout">The packed layout of the pixel format. Should be <c><see cref="PackedLayout.None"/></c>.</param>
	/// <param name="bitsPerPixel">The number of bits per pixel of the pixel format</param>
	/// <param name="bytesPerPixel">The number of bytes per pixel of the pixel format</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public PixelFormat(PixelType type, ArrayOrder order, PackedLayout layout, int bitsPerPixel, int bytesPerPixel) :
		this(type, unchecked((int)order), layout, bitsPerPixel, bytesPerPixel)
	{ }

	/// <summary>
	/// Gets the number of bits per pixel
	/// </summary>
	/// <value>
	/// The number of bits per pixel
	/// </value>
	public readonly int BitsPerPixel
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		get => IsFourCC
			? 0
			: unchecked((int)((mFormat >> 8) & 0xff));
	}

	/// <summary>
	/// Gets the number of bytes per pixel
	/// </summary>
	/// <value>
	/// The number of bytes per pixel
	/// </value>
	public readonly int BytesPerPixel
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		get => IsFourCC
			?  this == Yuy2
			|| this == Uyvy
			|| this == Yvyu
			|| this == P010
				? 2
				: 1
			: unchecked((int)(mFormat >> 0) & 0xff);
	}

	private readonly uint Flag { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => unchecked((mFormat >> 28) & 0x0f); }

	/// <summary>
	/// Gets the layout of a packed pixel format
	/// </summary>
	/// <value>
	/// The layout of a packed pixel format
	/// </value>
	/// <remarks>
	/// <para>
	/// Note: The value of this property is only valid if <see cref="IsFourCC"/> is <c><see langword="false"/></c>.
	/// </para>
	/// <para>
	/// If the pixel format is not a packed format (<see cref="IsPacked"/> is <c><see langword="false"/></c>), the value of this property should be <see cref="PackedLayout.None"/>.
	/// </para>
	/// </remarks>
	public readonly PackedLayout Layout { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => unchecked((PackedLayout)((mFormat >> 16) & 0x0f)); }

	/// <summary>
	/// Gets a value indicating whether the pixel format is a 10-bit per component format
	/// </summary>
	/// <value>
	/// A value indicating whether the pixel format is a 10-bit per component format
	/// </value>
	public readonly bool Is10Bit
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		get => !IsFourCC
			&& Type is PixelType.Packed32
			&& Layout is PackedLayout._2101010;
	}

	/// <summary>
	/// Gets a value indicating whether the pixel format contains an alpha component
	/// </summary>
	/// <value>
	/// A value indicating whether the pixel format contains an alpha component
	/// </value>
	public readonly bool IsAlpha
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		get => IsPacked
			? unchecked((PackedOrder)Order) is PackedOrder.Argb
			                                or PackedOrder.Rgba
			                                or PackedOrder.Abgr
			                                or PackedOrder.Bgra
			: IsArray
			&& unchecked((ArrayOrder)Order) is ArrayOrder.Argb
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
	public readonly bool IsArray
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		get => !IsFourCC
			&& Type is PixelType.ArrayU8
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
	public readonly bool IsFloat
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		get => !IsFourCC
			&& Type is PixelType.ArrayF16
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
	public readonly bool IsFourCC
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		get => mFormat is not 0
			&& Flag is not 1;
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
	public readonly bool IsIndexed
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		get => !IsFourCC
			&& Type is PixelType.Index1
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
	public readonly bool IsPacked
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		get => !IsFourCC
			&& Type is PixelType.Packed8
			        or PixelType.Packed16
			        or PixelType.Packed32;
	}

	/// <summary>
	/// Gets the component or bit order of the pixel format
	/// </summary>
	/// <value>
	/// The component or bit order of the pixel format
	/// </value>
	/// <remarks> 
	/// <para>
	/// Note: The value of this property is only valid if <see cref="IsFourCC"/> is <c><see langword="false"/></c>.
	/// </para>
	/// <para>
	/// The value of this property can be cast to either <see cref="BitmapOrder"/>, <see cref="PackedOrder"/>, or <see cref="ArrayOrder"/>, depending on the <see cref="Type">type of pixel format</see>.
	/// <list type="bullet">
	///		<item>
	///			<term><see cref="IsIndexed"/> is <c><see langword="true"/></c></term>
	///			<description>The value of this property should be interpreted as/cast to a <see cref="BitmapOrder"/> value</description>
	///		</item>
	///		<item>
	///			<term><see cref="IsPacked"/> is <c><see langword="true"/></c></term>
	///			<description>The value of this property should be interpreted as/cast to a <see cref="PackedOrder"/> value</description>
	///		</item>
	///		<item>
	///			<term><see cref="IsArray"/> is <c><see langword="true"/></c></term>
	///			<description>The value of this property should be interpreted as/cast to an <see cref="ArrayOrder"/> value</description>
	///		</item>
	/// </list>
	/// </para>
	/// </remarks>
	public readonly int Order { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => unchecked((int)(mFormat >> 20) & 0x0f); }

	/// <summary>
	/// Gets the type of the pixel format
	/// </summary>
	/// <value>
	/// The type of the pixel format
	/// </value>
	/// <remarks>
	/// <para>
	/// Note: The value of this property is only valid if <see cref="IsFourCC"/> is <c><see langword="false"/></c>.
	/// </para>
	/// </remarks>
	public readonly PixelType Type { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => unchecked((PixelType)((mFormat >> 24) & 0x0f)); }

	/// <inheritdoc/>
	public readonly override bool Equals([NotNullWhen(true)] object? obj) => obj is PixelFormat other && Equals(other);

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly bool Equals(PixelFormat other) => mFormat == other.mFormat;

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly override int GetHashCode() => mFormat.GetHashCode();

	/// <inheritdoc/>
	public readonly override string ToString()
	{
		unsafe
		{
			return Utf8StringMarshaller.ConvertToManaged(SDL_GetPixelFormatName(this)) ?? string.Empty;
		}
	}

	/// <inheritdoc/>
	readonly string IFormattable.ToString(string? format, IFormatProvider? formatProvider) => ToString();

	/// <inheritdoc cref="ISpanFormattable.TryFormat(Span{char}, out int, ReadOnlySpan{char}, IFormatProvider?)"/>
	public readonly bool TryFormat(Span<char> destination, out int charsWritten)
	{
		unsafe
		{
			return Encoding.UTF8.TryGetChars(MemoryMarshal.CreateReadOnlySpanFromNullTerminated(SDL_GetPixelFormatName(this)), destination, out charsWritten);
		}
	}

	/// <inheritdoc/>
	readonly bool ISpanFormattable.TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider) => TryFormat(destination, out charsWritten);

	/// <summary>
	/// Tries to convert a bits-per-pixel value and RGBA masks to a pixel format
	/// </summary>
	/// <param name="bitsPerPixel">The bits-per-pixel value of the pixel format. This is usually <c>15</c>, <c>16</c>, or <c>32</c>.</param>
	/// <param name="rMask">A red component bit mask for the pixel format</param>
	/// <param name="gMask">A green component bit mask for the pixel format</param>
	/// <param name="bMask">A blue component bit mask for the pixel format</param>
	/// <param name="aMask">A alpha component bit mask for the pixel format</param>
	/// <param name="format">The resulting <see cref="PixelFormat"/> corresponding to the specified bits-per-pixel value and RGBA masks, if this method returns <c><see langword="true"/></c>; otherwise, <see cref="Unknown"/></param>
	/// <returns><c><see langword="true"/></c>, if the provided bits-per-pixel value together with the provided RGBA masks were successfully converted into a pixel format; otherwise, <c><see langword="false"/></c></returns>
	public static bool TryGetFromMasks(int bitsPerPixel, uint rMask, uint gMask, uint bMask, uint aMask, out PixelFormat format)
	{
		format = SDL_GetPixelFormatForMasks(bitsPerPixel, rMask, gMask, bMask, aMask);

		return format != Unknown;
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
	public readonly bool TryGetMasks(out int bitsPerPixel, out uint rMask, out uint gMask, out uint bMask, out uint aMask)
	{
		unsafe
		{
			int bpp;
			uint rmask, gmask, bmask, amask;

			bool result = SDL_GetMasksForPixelFormat(this, &bpp, &rmask, &gmask, &bmask, &amask);

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
	public readonly bool TryGetPixelFormatDetails(out PixelFormatDetails formatDetails)
	{
		unsafe
		{
			var formatDetailsPtr = SDL_GetPixelFormatDetails(this);

			formatDetails = new(formatDetailsPtr);

			return formatDetailsPtr is not null;
		}
	}

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator ==(PixelFormat left, PixelFormat right) => left.mFormat == right.mFormat;

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator !=(PixelFormat left, PixelFormat right) => left.mFormat != right.mFormat;
}
