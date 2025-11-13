namespace Sdl3Sharp.Video.Coloring;

/// <summary>
/// Represents the matrix coefficients used in <see cref="ColorSpace"/>s
/// </summary>
/// <remarks>
/// <para>
/// The <see cref="MatrixCoefficients"/> defined here are as described by <see href="https://www.itu.int/rec/T-REC-H.273-201612-S/en"/>.
/// </para>
/// </remarks>
public enum MatrixCoefficients
{
    /// <summary>The identity matrix</summary>
    Identity = 0,

    /// <summary>ITU-R BT.709-6</summary>
    Bt709 = 1,

    /// <summary>Unspecified</summary>
    Unspecified = 2,

    /// <summary>US FCC Title 47</summary>
    Fcc = 4,

    /// <summary>ITU-R BT.470-6 System B, G / ITU-R BT.601-7 625</summary>
    /// <remarks>
    /// <para>
    /// This is functionally the same as <see cref="Bt601"/>.
    /// </para>
    /// </remarks>
    Bt470Bg = 5,

    /// <summary>ITU-R BT.601-7 525</summary>
    Bt601 = 6,

    /// <summary>SMPTE 240M</summary>
    Smpte240 = 7,

    /// <summary>YcgCo</summary>
    YCgCo = 8,

    /// <summary>ITU-R BT.2020-2 with non-constant luminance</summary>
    Bt2020Ncl = 9,

    /// <summary>ITU-R BT.2020-2 with constant luminance</summary>
    Bt2020Cl = 10,

    /// <summary>SMPTE ST 2085</summary>
    Smpte2085 = 11, 

    /// <summary>Derived chromaticity with non-constant luminance</summary>
    ChromaDerivedNcl = 12,

    /// <summary>Derived chromaticity with constant luminance</summary>
    ChromaDerivedCl = 13,

    /// <summary>ITU-R BT.2100-0 ICTCP</summary>
    ICtCp = 14,

    /// <summary>Custom</summary>
    /// <remarks>
    /// <para>
    /// This specifies that the <see cref="ColorSpace"/> uses some kind of custom matrix coefficients.
    /// </para>
    /// </remarks>
    Custom = 31
}
