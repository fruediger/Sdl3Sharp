namespace Sdl3Sharp.Video.Coloring;

/// <summary>
/// Represtents a bitmap pixel order, from highest bit to lowest bit
/// </summary>
public enum BitmapOrder
{
    /// <summary>No specific order</summary>
    None,

    /// <summary>Least significant bit order</summary>
    _4321,

    /// <summary>Most significant bit order</summary>
    _1234
}
