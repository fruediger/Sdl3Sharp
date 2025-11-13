namespace Sdl3Sharp.Video.Coloring;

/// <summary>
/// Represents the transfer characteristics used in <see cref="ColorSpace"/>s
/// </summary>
/// <remarks>
/// The <see cref="TransferCharacteristics"/> defined here are as described by <see href="https://www.itu.int/rec/T-REC-H.273-201612-S/en"/>.
/// </remarks>
public enum TransferCharacteristics
{
    /// <summary>Unknown</summary>
    Unknown = 0,

    /// <summary>Rec. ITU-R BT.709-6 / ITU-R BT1361</summary>
    Bt709 = 1,

    /// <summary>Unspecified</summary>
    Unspecified = 2,

    /// <summary>ITU-R BT.470-6 System M / ITU-R BT1700 625 PAL &amp; SECAM</summary>
    Gamma22 = 4,

    /// <summary>ITU-R BT.470-6 System B, G</summary>
    Gamma28 = 5, 

    /// <summary>SMPTE ST 170M / ITU-R BT.601-7 525 or 625</summary>
    Bt601 = 6,

    /// <summary>SMPTE ST 240M</summary>
    Smpte240 = 7,

    /// <summary>Linear transfer characteristics</summary>
    Linear = 8,

    /// <summary>Logarithmic transfer characteristic (100 : 1 range)</summary>
    Log100 = 9,

    /// <summary>Logarithmic transfer characteristic (100⋅√(10) : 1 range)</summary>
    Log100Sqrt10 = 10,

    /// <summary>IEC 61966-2-4</summary>
    Iec61966 = 11,

    /// <summary>ITU-R BT1361 Extended Colour Gamut</summary>
    Bt1361 = 12,

    /// <summary>IEC 61966-2-1 (sRGB or sYCC)</summary>
    Srgb = 13,

    /// <summary>ITU-R BT2020 for 10-bit system</summary>
    Bt2020_10Bit = 14,

    /// <summary>ITU-R BT2020 for 12-bit system</summary>
    Bt2020_12Bit = 15,

    /// <summary>SMPTE ST 2084 for 10-, 12-, 14- and 16-bit systems</summary>
    Pq = 16,

    /// <summary>SMPTE ST 428-1</summary>
    Smpte428 = 17, 

    /// <summary>ARIB STD-B67, also known as "hybrid log-gamma" (HLG)</summary>
    Hlg = 18,

    /// <summary>Custom</summary>
    /// <remarks>
    /// <para>
    /// This specifies that the <see cref="ColorSpace"/> uses some kind of custom transfer characteristics.
    /// </para>
    /// </remarks>
    Custom = 31
}
