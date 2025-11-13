namespace Sdl3Sharp.Video.Coloring;

/// <summary>
/// Represents the type of pixel format used in a <see cref="PixelFormat"/>s
/// </summary>
public enum PixelType
{
    /// <summary>Unknown</summary>
    Unknown,

    /// <summary>Indexed pixel values, 1 bit per pixel</summary>
    /// <remarks>
    /// <para>
    /// Used in conjuction with a <see cref="Palette"/>.
    /// </para>
    /// </remarks>
    /// <seealso cref="BitmapOrder"/>
    Index1,

    /// <summary>Indexed pixel values, 4 bits per pixel</summary>
    /// <remarks>
    /// <para>
    /// Used in conjuction with a <see cref="Palette"/>.
    /// </para>
    /// </remarks>
    /// <seealso cref="BitmapOrder"/>
    Index4,

    /// <summary>Indexed pixel values, 8 bits per pixel</summary>
    /// <remarks>
    /// <para>
    /// Used in conjuction with a <see cref="Palette"/>.
    /// </para>
    /// </remarks>
    /// <seealso cref="BitmapOrder"/>
    Index8,

    /// <summary>Packed pixel values, totalling 8 bits per pixel</summary>
    /// <seealso cref="PackedOrder"/>
    /// <seealso cref="PackedLayout"/>
    Packed8,

    /// <summary>Packed pixel values, totalling 16 bits per pixel</summary>
    /// <seealso cref="PackedOrder"/>
    /// <seealso cref="PackedLayout"/>
    Packed16,

    /// <summary>Packed pixel values, totalling 32 bits per pixel</summary>
    /// <seealso cref="PackedOrder"/>
    /// <seealso cref="PackedLayout"/>
    Packed32,

    /// <summary>Array of pixel values, each component stored separately as unsigned 8-bit integers</summary>
    /// <seealso cref="ArrayOrder"/>
    ArrayU8,

    /// <summary>Array of pixel values, each component stored separately as unsigned 16-bit integers</summary>
    /// <seealso cref="ArrayOrder"/>
    ArrayU16,

    /// <summary>Array of pixel values, each component stored separately as unsigned 32-bit integers</summary>
    /// <seealso cref="ArrayOrder"/>
    ArrayU32,

    /// <summary>Array of pixel values, each component stored separately as 16-bit floating point values</summary>
    /// <seealso cref="ArrayOrder"/>
    ArrayF16,

    /// <summary>Array of pixel values, each component stored separately as 32-bit floating point values</summary>
    /// <seealso cref="ArrayOrder"/>
    ArrayF32,

    /* appended at the end for compatibility with sdl2-compat:  */

    /// <summary>Indexed pixel values, 2 bits per pixel</summary>
    /// <seealso cref="PackedOrder"/>
    /// <seealso cref="PackedLayout"/>
    Index2
}
