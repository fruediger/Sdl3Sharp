namespace Sdl3Sharp.Video.Coloring;

/// <summary>
/// Represents the color range used in <see cref="ColorSpace"/>s
/// </summary>
/// <remarks>
/// <para>
/// The <see cref="ColorRange"/>s defined here are as described by <see href="https://www.itu.int/rec/R-REC-BT.2100-2-201807-I/en"/>.
/// </para>
/// </remarks>
public enum ColorRange
{
	/// <summary>Unknown</summary>
	Unknown = 0,

	/// <summary>Narrow range</summary>
	/// <remarks>
	/// <para>
	/// E.g. <c>16</c>-<c>235</c> for 8-bit RGB and luma, and <c>16</c>-<c>240</c> for 8-bit chroma
	/// </para>
	/// </remarks>
	Limited = 1,

	/// <summary>Full range</summary>
	/// <remarks>
	/// <para>
	/// E.g. <c>0</c>-<c>255</c> for 8-bit RGB and luma, and <c>1</c>-<c>255</c> for 8-bit chroma
	/// </para>
	/// </remarks>
	Full = 2
}
