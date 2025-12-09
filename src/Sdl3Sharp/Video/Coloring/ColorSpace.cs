namespace Sdl3Sharp.Video.Coloring;

/// <summary>
/// Represents a colorspace
/// </summary>
public enum Colorspace : uint
{
#pragma warning disable IDE0079 // Leave this here to remind ourselves in case we want to document these in the future
#pragma warning disable CS1591 // Not sure if it's actually necessary and reasonable to document all of these predefined colorspaces

	/// <summary>The unknown <see cref="Colorspace"/></summary>
	Unknown = 0,

	/// <summary>
	/// <see cref="Srgb">sRGB</see> is a gamma corrected colorspace, and the default colorspace for SDL rendering and 8-bit RGB surface
	/// </summary>
	/// <remarks>
	/// <para>
	/// This is equivalent to the DirectX color space <c>DXGI_COLOR_SPACE_RGB_FULL_G22_NONE_P709</c> (see <see href="https://learn.microsoft.com/en-us/windows/win32/api/dxgicommon/ne-dxgicommon-dxgi_color_space_type"/>).
	/// </para>
	/// </remarks>
	Srgb = ((uint)ColorType.Rgb                << 28)
		 | ((uint)ColorRange.Full              << 24)
		 | ((uint)ChromaLocation.None          << 20)
		 | ((uint)ColorPrimaries.Bt709         << 10)
		 | ((uint)TransferCharacteristics.Srgb << 5)
		 | ((uint)MatrixCoefficients.Identity  << 0),

	/// <summary>
	/// <see cref="SrgbLinear">sRGB linear</see> is a linear colorspace and the default colorspace for floating point surfaces.
	/// On Windows this is the scRGB colorspace, and on Apple platforms this is kCGColorSpaceExtendedLinearSRGB for EDR content.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This is equivalent to the DirectX color space <c>DXGI_COLOR_SPACE_RGB_FULL_G10_NONE_P709</c> (see <see href="https://learn.microsoft.com/en-us/windows/win32/api/dxgicommon/ne-dxgicommon-dxgi_color_space_type"/>).
	/// </para>
	/// </remarks>
	SrgbLinear = ((uint)ColorType.Rgb                  << 28)
		       | ((uint)ColorRange.Full                << 24)
		       | ((uint)ChromaLocation.None            << 20)
		       | ((uint)ColorPrimaries.Bt709           << 10)
		       | ((uint)TransferCharacteristics.Linear << 5)
		       | ((uint)MatrixCoefficients.Identity    << 0),

	/// <summary>
	/// <see cref="Hdr10">HDR10</see> is a non-linear HDR colorspace and the default colorspace for 10-bit surfaces
	/// </summary>
	/// <remarks>
	/// <para>
	/// This is equivalent to the DirectX color space <c>DXGI_COLOR_SPACE_RGB_FULL_G2084_NONE_P2020</c> (see <see href="https://learn.microsoft.com/en-us/windows/win32/api/dxgicommon/ne-dxgicommon-dxgi_color_space_type"/>).
	/// </para>
	/// </remarks>
	Hdr10 = ((uint)ColorType.Rgb               << 28)
		  | ((uint)ColorRange.Full             << 24)
		  | ((uint)ChromaLocation.None         << 20)
		  | ((uint)ColorPrimaries.Bt2020       << 10)
		  | ((uint)TransferCharacteristics.Pq  << 5)
		  | ((uint)MatrixCoefficients.Identity << 0),

	/// <remarks>
	/// <para>
	/// This is equivalent to the DirectX color space <c>DXGI_COLOR_SPACE_YCBCR_FULL_G22_NONE_P709_X601</c> (see <see href="https://learn.microsoft.com/en-us/windows/win32/api/dxgicommon/ne-dxgicommon-dxgi_color_space_type"/>).
	/// </para>
	/// </remarks>
	Jpeg = ((uint)ColorType.YCbCr               << 28)
		 | ((uint)ColorRange.Full               << 24)
		 | ((uint)ChromaLocation.None           << 20)
		 | ((uint)ColorPrimaries.Bt709          << 10)
		 | ((uint)TransferCharacteristics.Bt601 << 5)
		 | ((uint)MatrixCoefficients.Bt601      << 0),

	/// <remarks>
	/// <para>
	/// This is equivalent to the DirectX color space <c>DXGI_COLOR_SPACE_YCBCR_STUDIO_G22_LEFT_P601</c> (see <see href="https://learn.microsoft.com/en-us/windows/win32/api/dxgicommon/ne-dxgicommon-dxgi_color_space_type"/>).
	/// </para>
	/// </remarks>
	Bt601Limited = ((uint)ColorType.YCbCr               << 28)
		         | ((uint)ColorRange.Limited            << 24)
		         | ((uint)ChromaLocation.Left           << 20)
		         | ((uint)ColorPrimaries.Bt601          << 10)
		         | ((uint)TransferCharacteristics.Bt601 << 5)
		         | ((uint)MatrixCoefficients.Bt601      << 0),

	/// <remarks>
	/// <para>
	/// This is equivalent to the DirectX color space <c>DXGI_COLOR_SPACE_YCBCR_STUDIO_G22_LEFT_P601</c> (see <see href="https://learn.microsoft.com/en-us/windows/win32/api/dxgicommon/ne-dxgicommon-dxgi_color_space_type"/>).
	/// </para>
	/// </remarks>
	Bt601Full = ((uint)ColorType.YCbCr               << 28)
			  | ((uint)ColorRange.Full               << 24)
			  | ((uint)ChromaLocation.Left           << 20)
			  | ((uint)ColorPrimaries.Bt601          << 10)
			  | ((uint)TransferCharacteristics.Bt601 << 5)
			  | ((uint)MatrixCoefficients.Bt601      << 0),

	/// <remarks>
	/// <para>
	/// This is equivalent to the DirectX color space <c>DXGI_COLOR_SPACE_YCBCR_STUDIO_G22_LEFT_P709</c> (see <see href="https://learn.microsoft.com/en-us/windows/win32/api/dxgicommon/ne-dxgicommon-dxgi_color_space_type"/>).
	/// </para>
	/// </remarks>
	Bt709Limited = ((uint)ColorType.YCbCr               << 28)
		         | ((uint)ColorRange.Limited            << 24)
		         | ((uint)ChromaLocation.Left           << 20)
		         | ((uint)ColorPrimaries.Bt709          << 10)
		         | ((uint)TransferCharacteristics.Bt709 << 5)
		         | ((uint)MatrixCoefficients.Bt709      << 0),

	/// <remarks>
	/// <para>
	/// This is equivalent to the DirectX color space <c>DXGI_COLOR_SPACE_YCBCR_STUDIO_G22_LEFT_P709</c> (see <see href="https://learn.microsoft.com/en-us/windows/win32/api/dxgicommon/ne-dxgicommon-dxgi_color_space_type"/>).
	/// </para>
	/// </remarks>
	Bt709Full = ((uint)ColorType.YCbCr               << 28)
			  | ((uint)ColorRange.Full               << 24)
			  | ((uint)ChromaLocation.Left           << 20)
			  | ((uint)ColorPrimaries.Bt709          << 10)
			  | ((uint)TransferCharacteristics.Bt709 << 5)
			  | ((uint)MatrixCoefficients.Bt709      << 0),

	/// <remarks>
	/// <para>
	/// This is equivalent to the DirectX color space <c>DXGI_COLOR_SPACE_YCBCR_STUDIO_G22_LEFT_P2020</c> (see <see href="https://learn.microsoft.com/en-us/windows/win32/api/dxgicommon/ne-dxgicommon-dxgi_color_space_type"/>).
	/// </para>
	/// </remarks>
	Bt2020Limited = ((uint)ColorType.YCbCr              << 28)
		          | ((uint)ColorRange.Limited           << 24)
		          | ((uint)ChromaLocation.Left          << 20)
		          | ((uint)ColorPrimaries.Bt2020        << 10)
		          | ((uint)TransferCharacteristics.Pq   << 5)
		          | ((uint)MatrixCoefficients.Bt2020Ncl << 0),

	/// <remarks>
	/// <para>
	/// This is equivalent to the DirectX color space <c>DXGI_COLOR_SPACE_YCBCR_FULL_G22_LEFT_P2020</c> (see <see href="https://learn.microsoft.com/en-us/windows/win32/api/dxgicommon/ne-dxgicommon-dxgi_color_space_type"/>).
	/// </para>
	/// </remarks>
	Bt2020Full = ((uint)ColorType.YCbCr              << 28)
			   | ((uint)ColorRange.Full              << 24)
			   | ((uint)ChromaLocation.Left          << 20)
			   | ((uint)ColorPrimaries.Bt2020        << 10)
			   | ((uint)TransferCharacteristics.Pq   << 5)
			   | ((uint)MatrixCoefficients.Bt2020Ncl << 0),

	/// <summary>The default color space for RGB surfaces if no color space is specified</summary>
	/// <remarks>
	/// <para>
	/// This is equivalent to <see cref="Srgb"/>.
	/// </para>
	/// </remarks>
	RgbDefault = Srgb,

	/// <summary>The default colorspace for YUV surfaces if no colorspace is specified</summary>
	/// <remarks>
	/// <para>
	/// This is equivalent <see cref="Bt601Limited"/>.
	/// </para>
	/// </remarks>
	YuvDefault = Bt601Limited

#pragma warning restore CS1591
#pragma warning restore IDE0079
}
