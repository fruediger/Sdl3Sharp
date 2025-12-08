namespace Sdl3Sharp.Video.Blending;

/// <summary>
/// Represents a blend mode used in drawing operations
/// </summary>
/// <remarks>
/// <para>
/// The defined blend modes in the <see cref="BlendMode"/> enumeration are supported everywhere.
/// </para>
/// <para>
/// Additional custom blend modes can be obtained by using <see cref="BlendModeExtensions.ComposeCustom(BlendFactor, BlendFactor, BlendOperation, BlendFactor, BlendFactor, BlendOperation)"/>.
/// Notice that not all renderers support all custom blend modes.
/// </para>
/// </remarks>
public enum BlendMode : uint
{
	/// <summary>No blending</summary>
	/// <remarks>
	/// <para>
	/// <code>
	/// dstRGBA = srcRGBA
	/// </code>
	/// </para>
	/// </remarks>
	None = 0x00000000u,

	/// <summary>Alpha blending</summary>
	/// <remarks>
	/// <para>
	/// <code>
	/// dstRGB = (srcRGB * srcA) + (dstRGB * (1 - srcA))
	/// dstA = srcA + (dstA * (1 - srcA))
	/// </code>
	/// </para>
	/// </remarks>
	Blend = 0x00000001u,

	/// <summary>Pre-multiplied alpha blending</summary>
	/// <remarks>
	/// <code>
	/// dstRGBA = srcRGBA + (dstRGBA * (1 - srcA))
	/// </code>
	/// </remarks>
	BlendPremultiplied = 0x00000010u,

	/// <summary>Additive blending</summary>
	/// <remarks>
	/// <para>
	/// <code>
	/// dstRGB = (srcRGB * srcA) + dstRGB
	/// dstA = dstA
	/// </code>
	/// </para>
	/// </remarks>
	Add = 0x00000002u,

	/// <summary>Pre-multiplied additive blending</summary>
	/// <remarks>
	/// <para>
	/// <code>
	/// dstRGB = srcRGB + dstRGB
	/// dstA = dstA
	/// </code>
	/// </para>
	/// </remarks>
	AddPremultiplied = 0x00000020u,

	/// <summary>Color modulate</summary>
	/// <remarks>
	/// <para>
	/// <code>
	/// dstRGB = srcRGB * dstRGB
	/// dstA = dstA
	/// </code>
	/// </para>
	/// </remarks>
	Modulate = 0x00000004u,

	/// <summary>Color multiply</summary>
	/// <remarks>
	/// <para>
	/// <code>
	/// dstRGB = (srcRGB * dstRGB) + (dstRGB * (1 - srcA))
	/// dstA = dstA
	/// </code>
	/// </para>
	/// </remarks>
	Multiply = 0x00000008u,

	/// <summary>A representative for an invalid <see cref="BlendMode"/></summary>
	Invalid = 0x7FFFFFFFu
}
