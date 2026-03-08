using Sdl3Sharp.Video.Drawing;

namespace Sdl3Sharp.Video.Windowing;

/// <summary>
/// Represents a method that will be called when a custom hit test should be performed for a <see cref="Window"/>
/// </summary>
/// <param name="window">The <see cref="Window"/> for which the hit test should be performed</param>
/// <param name="area">The point which should be checked</param>
/// <returns>The <see cref="HitTestResult"/> of the custom hit test, describing the kind of hit test result for the given point</returns>
/// <remarks>
/// <para>
/// You can set a custom hit test for a <see cref="Window"/> by setting its <see cref="Window.HitTest"/> property.
/// </para>
/// </remarks>
public delegate HitTestResult HitTest(Window window, in Point<int> area);
