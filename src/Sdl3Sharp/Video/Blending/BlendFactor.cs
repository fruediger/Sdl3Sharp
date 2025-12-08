namespace Sdl3Sharp.Video.Blending;

/// <summary>
/// Represents normalized blending factors used to multiply pixel components in blending operations
/// </summary>
/// <remarks>
/// <para>
/// In blending operations, the pixel components of the source and destination pixels are multiplied with blending factors before they get combined using a blend operation.
/// </para>
/// </remarks>
public enum BlendFactor
{
	/// <summary>All-zero factors</summary>
	/// <remarks>
	/// <para>
	/// <list type="bullet">
	///		<item>
	///			<term>Red factor</term>
	///			<description><c>0</c></description>
	///		</item>
	///		<item>
	///			<term>Green factor</term>
	///			<description><c>0</c></description>
	///		</item>
	///		<item>
	///			<term>Blue factor</term>
	///			<description><c>0</c></description>
	///		</item>
	///		<item>
	///			<term>Alpha factor</term>
	///			<description><c>0</c></description>
	///		</item>
	/// </list>
	/// </para>
	/// </remarks>
	Zero = 0x1,

	/// <summary>All-one factors</summary>
	/// <remarks>
	/// <para>
	/// <list type="bullet">
	///		<item>
	///			<term>Red factor</term>
	///			<description><c>1</c></description>
	///		</item>
	///		<item>
	///			<term>Green factor</term>
	///			<description><c>1</c></description>
	///		</item>
	///		<item>
	///			<term>Blue factor</term>
	///			<description><c>1</c></description>
	///		</item>
	///		<item>
	///			<term>Alpha factor</term>
	///			<description><c>1</c></description>
	///		</item>
	/// </list>
	/// </para>
	/// </remarks>
	One = 0x2,

	/// <summary>Source-color factors</summary>
	/// <remarks>
	/// <para>
	/// <list type="bullet">
	///		<item>
	///			<term>Red factor</term>
	///			<description><c>srcR</c></description>
	///		</item>
	///		<item>
	///			<term>Green factor</term>
	///			<description><c>srcG</c></description>
	///		</item>
	///		<item>
	///			<term>Blue factor</term>
	///			<description><c>srcB</c></description>
	///		</item>
	///		<item>
	///			<term>Alpha factor</term>
	///			<description><c>srcA</c></description>
	///		</item>
	/// </list>
	/// </para>
	/// </remarks>
	SourceColor = 0x3,

	/// <summary>One-minus-source-color factors</summary>
	/// <remarks>
	/// <para>
	/// <list type="bullet">
	///		<item>
	///			<term>Red factor</term>
	///			<description><c>1 - srcR</c></description>
	///		</item>
	///		<item>
	///			<term>Green factor</term>
	///			<description><c>1 - srcG</c></description>
	///		</item>
	///		<item>
	///			<term>Blue factor</term>
	///			<description><c>1 - srcB</c></description>
	///		</item>
	///		<item>
	///			<term>Alpha factor</term>
	///			<description><c>1 - srcA</c></description>
	///		</item>
	/// </list>
	/// </para>
	/// </remarks>
	OneMinusSourceColor = 0x4,

	/// <summary>Source-alpha factors</summary>
	/// <remarks>
	/// <para>
	/// <list type="bullet">
	///		<item>
	///			<term>Red factor</term>
	///			<description><c>srcA</c></description>
	///		</item>
	///		<item>
	///			<term>Green factor</term>
	///			<description><c>srcA</c></description>
	///		</item>
	///		<item>
	///			<term>Blue factor</term>
	///			<description><c>srcA</c></description>
	///		</item>
	///		<item>
	///			<term>Alpha factor</term>
	///			<description><c>srcA</c></description>
	///		</item>
	/// </list>
	/// </para>
	/// </remarks>
	SourceAlpha = 0x5,

	/// <summary>One-minus-source-alpha factors</summary>
	/// <remarks>
	/// <para>
	/// <list type="bullet">
	///		<item>
	///			<term>Red factor</term>
	///			<description><c>1 - srcA</c></description>
	///		</item>
	///		<item>
	///			<term>Green factor</term>
	///			<description><c>1 - srcA</c></description>
	///		</item>
	///		<item>
	///			<term>Blue factor</term>
	///			<description><c>1 - srcA</c></description>
	///		</item>
	///		<item>
	///			<term>Alpha factor</term>
	///			<description><c>1 - srcA</c></description>
	///		</item>
	/// </list>
	/// </para>
	/// </remarks>
	OneMinusSourceAlpha = 0x6,

	/// <summary>Destination-color factors</summary>
	/// <remarks>
	/// <para>
	/// <list type="bullet">
	///		<item>
	///			<term>Red factor</term>
	///			<description><c>dstR</c></description>
	///		</item>
	///		<item>
	///			<term>Green factor</term>
	///			<description><c>dstG</c></description>
	///		</item>
	///		<item>
	///			<term>Blue factor</term>
	///			<description><c>dstB</c></description>
	///		</item>
	///		<item>
	///			<term>Alpha factor</term>
	///			<description><c>dstA</c></description>
	///		</item>
	/// </list>
	/// </para>
	/// </remarks>
	DestinationColor = 0x7,

	/// <summary>One-minus-destination-color factors</summary>
	/// <remarks>
	/// <para>
	/// <list type="bullet">
	///		<item>
	///			<term>Red factor</term>
	///			<description><c>1 - dstR</c></description>
	///		</item>
	///		<item>
	///			<term>Green factor</term>
	///			<description><c>1 - dstG</c></description>
	///		</item>
	///		<item>
	///			<term>Blue factor</term>
	///			<description><c>1 - dstB</c></description>
	///		</item>
	///		<item>
	///			<term>Alpha factor</term>
	///			<description><c>1 - dstA</c></description>
	///		</item>
	/// </list>
	/// </para>
	/// </remarks>
	OneMinusDestinationColor = 0x8,

	/// <summary>Destination-alpha factors</summary>
	/// <remarks>
	/// <para>
	/// <list type="bullet">
	///		<item>
	///			<term>Red factor</term>
	///			<description><c>dstA</c></description>
	///		</item>
	///		<item>
	///			<term>Green factor</term>
	///			<description><c>dstA</c></description>
	///		</item>
	///		<item>
	///			<term>Blue factor</term>
	///			<description><c>dstA</c></description>
	///		</item>
	///		<item>
	///			<term>Alpha factor</term>
	///			<description><c>dstA</c></description>
	///		</item>
	/// </list>
	/// </para>
	/// </remarks>
	DestinationAlpha = 0x9,

	/// <summary>One-minus-destination-alpha factors</summary>
	/// <remarks>
	/// <para>
	/// <list type="bullet">
	///		<item>
	///			<term>Red factor</term>
	///			<description><c>1 - dstA</c></description>
	///		</item>
	///		<item>
	///			<term>Green factor</term>
	///			<description><c>1 - dstA</c></description>
	///		</item>
	///		<item>
	///			<term>Blue factor</term>
	///			<description><c>1 - dstA</c></description>
	///		</item>
	///		<item>
	///			<term>Alpha factor</term>
	///			<description><c>1 - dstA</c></description>
	///		</item>
	/// </list>
	/// </para>
	/// </remarks>
	OneMinusDestinationAlpha = 0xA
}
