using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Video.Coloring;

partial struct ColorSpace
{
#pragma warning disable CS1591 // Not sure if it's actually necessary and reasonable to document all of these predefined color spaces

	/// <summary>The unknown <see cref="ColorSpace"/></summary>
	/// <remarks>
	/// <para>
	/// This is also the <see langword="default"/> value of <see cref="ColorSpace"/>.
	/// </para>
	/// </remarks>
	public static ColorSpace Unknown { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(0); }

	/// <summary>
	/// <see cref="Srgb">sRGB</see> is a gamma corrected colorspace, and the default colorspace for SDL rendering and 8-bit RGB surface
	/// </summary>
	/// <remarks>
	/// <para>
	/// This is equivalent to the DirectX color space <c>DXGI_COLOR_SPACE_RGB_FULL_G22_NONE_P709</c> (see <see href="https://learn.microsoft.com/en-us/windows/win32/api/dxgicommon/ne-dxgicommon-dxgi_color_space_type"/>).
	/// </para>
	/// </remarks>
	public static ColorSpace Srgb { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(ColorType.Rgb, ColorRange.Full, ColorPrimaries.Bt709, TransferCharacteristics.Srgb, MatrixCoefficients.Identity, ChromaLocation.None); }

	/// <summary>
	/// <see cref="SrgbLinear">sRGB linear</see> is a linear colorspace and the default colorspace for floating point surfaces.
	/// On Windows this is the scRGB colorspace, and on Apple platforms this is kCGColorSpaceExtendedLinearSRGB for EDR content.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This is equivalent to the DirectX color space <c>DXGI_COLOR_SPACE_RGB_FULL_G10_NONE_P709</c> (see <see href="https://learn.microsoft.com/en-us/windows/win32/api/dxgicommon/ne-dxgicommon-dxgi_color_space_type"/>).
	/// </para>
	/// </remarks>
	public static ColorSpace SrgbLinear { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(ColorType.Rgb, ColorRange.Full, ColorPrimaries.Bt709, TransferCharacteristics.Linear, MatrixCoefficients.Identity, ChromaLocation.None); }

	/// <summary>
	/// <see cref="Hdr10">HDR10</see> is a non-linear HDR colorspace and the default colorspace for 10-bit surfaces
	/// </summary>
	/// <remarks>
	/// <para>
	/// This is equivalent to the DirectX color space <c>DXGI_COLOR_SPACE_RGB_FULL_G2084_NONE_P2020</c> (see <see href="https://learn.microsoft.com/en-us/windows/win32/api/dxgicommon/ne-dxgicommon-dxgi_color_space_type"/>).
	/// </para>
	/// </remarks>
	public static ColorSpace Hdr10 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(ColorType.Rgb, ColorRange.Full, ColorPrimaries.Bt2020, TransferCharacteristics.Pq, MatrixCoefficients.Identity, ChromaLocation.None); }

	/// <remarks>
	/// <para>
	/// This is equivalent to the DirectX color space <c>DXGI_COLOR_SPACE_YCBCR_FULL_G22_NONE_P709_X601</c> (see <see href="https://learn.microsoft.com/en-us/windows/win32/api/dxgicommon/ne-dxgicommon-dxgi_color_space_type"/>).
	/// </para>
	/// </remarks>
	public static ColorSpace Jpeg { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(ColorType.YCbCr, ColorRange.Full, ColorPrimaries.Bt709, TransferCharacteristics.Bt601, MatrixCoefficients.Bt601, ChromaLocation.None); }

	/// <remarks>
	/// <para>
	/// This is equivalent to the DirectX color space <c>DXGI_COLOR_SPACE_YCBCR_STUDIO_G22_LEFT_P601</c> (see <see href="https://learn.microsoft.com/en-us/windows/win32/api/dxgicommon/ne-dxgicommon-dxgi_color_space_type"/>).
	/// </para>
	/// </remarks>
	public static ColorSpace Bt601Limited { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(ColorType.YCbCr, ColorRange.Limited, ColorPrimaries.Bt601, TransferCharacteristics.Bt601, MatrixCoefficients.Bt601, ChromaLocation.Left); }

	/// <remarks>
	/// <para>
	/// This is equivalent to the DirectX color space <c>DXGI_COLOR_SPACE_YCBCR_STUDIO_G22_LEFT_P601</c> (see <see href="https://learn.microsoft.com/en-us/windows/win32/api/dxgicommon/ne-dxgicommon-dxgi_color_space_type"/>).
	/// </para>
	/// </remarks>
	public static ColorSpace Bt601Full { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(ColorType.YCbCr, ColorRange.Full, ColorPrimaries.Bt601, TransferCharacteristics.Bt601, MatrixCoefficients.Bt601, ChromaLocation.Left); }

	/// <remarks>
	/// <para>
	/// This is equivalent to the DirectX color space <c>DXGI_COLOR_SPACE_YCBCR_STUDIO_G22_LEFT_P709</c> (see <see href="https://learn.microsoft.com/en-us/windows/win32/api/dxgicommon/ne-dxgicommon-dxgi_color_space_type"/>).
	/// </para>
	/// </remarks>
	public static ColorSpace Bt709Limited { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(ColorType.YCbCr, ColorRange.Limited, ColorPrimaries.Bt709, TransferCharacteristics.Bt709, MatrixCoefficients.Bt709, ChromaLocation.Left); }

	/// <remarks>
	/// <para>
	/// This is equivalent to the DirectX color space <c>DXGI_COLOR_SPACE_YCBCR_STUDIO_G22_LEFT_P709</c> (see <see href="https://learn.microsoft.com/en-us/windows/win32/api/dxgicommon/ne-dxgicommon-dxgi_color_space_type"/>).
	/// </para>
	/// </remarks>
	public static ColorSpace Bt709Full { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(ColorType.YCbCr, ColorRange.Full, ColorPrimaries.Bt709, TransferCharacteristics.Bt709, MatrixCoefficients.Bt709, ChromaLocation.Left); }

	/// <remarks>
	/// <para>
	/// This is equivalent to the DirectX color space <c>DXGI_COLOR_SPACE_YCBCR_STUDIO_G22_LEFT_P2020</c> (see <see href="https://learn.microsoft.com/en-us/windows/win32/api/dxgicommon/ne-dxgicommon-dxgi_color_space_type"/>).
	/// </para>
	/// </remarks>
	public static ColorSpace Bt2020Limited { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(ColorType.YCbCr, ColorRange.Limited, ColorPrimaries.Bt2020, TransferCharacteristics.Pq, MatrixCoefficients.Bt2020Ncl, ChromaLocation.Left); }

	/// <remarks>
	/// <para>
	/// This is equivalent to the DirectX color space <c>DXGI_COLOR_SPACE_YCBCR_FULL_G22_LEFT_P2020</c> (see <see href="https://learn.microsoft.com/en-us/windows/win32/api/dxgicommon/ne-dxgicommon-dxgi_color_space_type"/>).
	/// </para>
	/// </remarks>
	public static ColorSpace Bt2020Full { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(ColorType.YCbCr, ColorRange.Full, ColorPrimaries.Bt2020, TransferCharacteristics.Pq, MatrixCoefficients.Bt2020Ncl, ChromaLocation.Left); }

	/// <summary>The default color space for RGB surfaces if no color space is specified</summary>
	/// <remarks>
	/// <para>
	/// This is <see cref="Srgb"/>.
	/// </para>
	/// </remarks>
	public static ColorSpace RgbDefault { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => Srgb; }

	/// <summary>The default colorspace for YUV surfaces if no colorspace is specified</summary>
	/// <remarks>
	/// <para>
	/// This is <see cref="Bt601Limited"/>.
	/// </para>
	/// </remarks>
	public static ColorSpace YuvDefault { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => Bt601Limited; }

#pragma warning restore CS1591
}
