namespace Sdl3Sharp.Video.Coloring;

/// <summary>
/// Represents the color primaries used in <see cref="ColorSpace"/>s
/// </summary>
/// <remarks>
/// <para>
/// The <see cref="ColorPrimaries"/> defined here are as described by <see href="https://www.itu.int/rec/T-REC-H.273-201612-S/en"/>.
/// </para>
/// </remarks>
public enum ColorPrimaries
{
    /// <summary>Unknown</summary>
    Unknown = 0,

    /// <summary>ITU-R BT.709-6</summary>
    Bt709 = 1,

    /// <summary>Unspecified</summary>
    Unspecified = 2,

    /// <summary>ITU-R BT.470-6 System M</summary>
    Bt470M = 4,

    /// <summary>ITU-R BT.470-6 System B, G / ITU-R BT.601-7 625</summary>
    Bt470Bg = 5,

    /// <summary>ITU-R BT.601-7 525, SMPTE 170M</summary>
    Bt601 = 6,

    /// <summary>MPTE 240M, </summary>
    /// <remarks>
    /// <para>
    /// This is functionally the same as <see cref="Bt601"/>.
    /// </para>
    /// </remarks>
    Smpte240 = 7,

    /// <summary>Generic film (color filters using Illuminant C)</summary>
    GenericFilm = 8,

    /// <summary>ITU-R BT.2020-2 / ITU-R BT.2100-0</summary>
    Bt2020 = 9,

    /// <summary>SMPTE ST 428-1</summary>
    Xyz = 10,

    /// <summary>SMPTE RP 431-2</summary>
    Smpte431 = 11,

    /// <summary>SMPTE EG 432-1 / DCI P3</summary>
    Smpte432 = 12,

    /// <summary>EBU Tech. 3213-E</summary>
    Ebu3213 = 22,

    /// <summary>Custom</summary>
    /// <remarks>
    /// <para>
    /// This specifies that the <see cref="ColorSpace"/> uses some kind of custom color primaries.
    /// </para>
    /// </remarks>
    Custom = 31
}
