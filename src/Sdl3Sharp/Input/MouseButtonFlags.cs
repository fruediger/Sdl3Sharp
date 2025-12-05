using System;

namespace Sdl3Sharp.Input;

/// <summary>
/// Represents a bitmask of mouse buttons
/// </summary>
[Flags]
public enum MouseButtonFlags : uint
{
    /// <summary>No mouse button</summary>
    None = 0,

    /// <summary>Left mouse button</summary>
    Left = 1u << (MouseButton.Left - 1),

    /// <summary>Middle mouse button</summary>
    Middle = 1u << (MouseButton.Middle - 1),

    /// <summary>Right mouse button</summary>
    Right = 1u << (MouseButton.Right - 1),

    /// <summary>Side mouse button 1</summary>
    X1 = 1u << (MouseButton.X1 - 1),

    /// <summary>Side mouse button 2</summary>
    X2 = 1u << (MouseButton.X2 - 1)
}
