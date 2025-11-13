namespace Sdl3Sharp.Video.Coloring;

/// <summary>
/// Represents the chroma sample location used in <see cref="ColorSpace"/>s
/// </summary>
public enum ChromaLocation
{
	/// <summary>No chroma sampling</summary>
	/// <remarks>
	/// <para>
	/// This is most likely used for RGB <see cref="ColorSpace"/>s
	/// </para>
	/// </remarks>
	None = 0,

	/// <summary>Chroma sampling at the group's left pixel column, vertically center</summary>
	/// <remarks>
	/// <para>
	/// In MPEG-2, MPEG-4, and AVC, Cb and Cr are taken on midpoint of the left-edge of the 2x2 square.
	/// In other words, they have the same horizontal location as the top-left pixel, but is shifted one-half pixel down vertically.
	/// </para>
	/// </remarks>
	Left = 1,

	/// <summary>Chroma sampling at the group's center</summary>
	/// <remarks>
	/// <para>
	/// In JPEG/JFIF, H.261, and MPEG-1, Cb and Cr are taken at the center of the 2x2 square.
	/// In other words, they are offset one-half pixel to the right and one-half pixel down compared to the top-left pixel.
	/// </para>
	/// </remarks>
	Center = 2,

	/// <summary>Chroma sampling at the group's top-left pixel</summary>
	/// <remarks>
	/// <para>
	/// In HEVC for BT.2020 and BT.2100 content (in particular on Blu-rays), Cb and Cr are sampled at the same location as the group's top-left Y pixel ("co-sited", "co-located").
	/// </para>
	/// </remarks>
	TopLeft = 3
}
