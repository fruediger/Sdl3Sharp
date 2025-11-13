namespace Sdl3Sharp.Video.Coloring;

/// <summary>
/// Represents the packed layout of color components, from most significant bits to least significant bits
/// </summary>
public enum PackedLayout
{
    /// <summary>No specific layout</summary>
    None,

    /// <summary>Layout: 3 bits, 3 bits, 2 bits</summary>
    _332,

    /// <summary>Layout: 4 bits, 4 bits, 4 bits, 4 bits</summary>
    _4444,

    /// <summary>Layout: 1 bit, 5 bits, 5 bits, 5 bits</summary>
    _1555,

    /// <summary>Layout: 5 bits, 5 bits, 5 bits, 1 bit</summary>
    _5551,

    /// <summary>Layout: 5 bits, 6 bits, 5 bits</summary>
    _565,

    /// <summary>Layout: 8 bits, 8 bits, 8 bits, 8 bits</summary>
    _8888,

    /// <summary>Layout: 2 bits, 10 bits, 10 bits, 10 bits</summary>
    _2101010,

    /// <summary>Layout: 10 bits, 10 bits, 10 bits, 2 bits</summary>
    _1010102
}
