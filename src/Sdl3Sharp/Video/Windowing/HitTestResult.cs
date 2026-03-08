namespace Sdl3Sharp.Video.Windowing;

/// <summary>
/// Represents the result of a <see cref="HitTest">custom hit test</see> performed for a <see cref="Window"/>
/// </summary>
public enum HitTestResult
{
	/// <summary>The region tested is normal, and has no special properties</summary>
	Normal,

	/// <summary>The region tested is draggable, and can be used to drag the entire window</summary>
	Draggable,

	/// <summary>The region tested is the top-left corner border of the window, and can be used to resize the window from that corner</summary>
	ResizeTopLeft,

	/// <summary>The region tested is the top border of the window, and can be used to resize the window from that border</summary>
	ResizeTop,

	/// <summary>The region tested is the top-right corner border of the window, and can be used to resize the window from that corner</summary>
	ResizeTopRight,

	/// <summary>The region tested is the right border of the window, and can be used to resize the window from that border</summary>
	ResizeRight,

	/// <summary>The region tested is the bottom-right corner border of the window, and can be used to resize the window from that corner</summary>
	ResizeBottomRight,

	/// <summary>The region tested is the bottom border of the window, and can be used to resize the window from that border</summary>
	ResizeBottom,

	/// <summary>The region tested is the bottom-left corner border of the window, and can be used to resize the window from that corner</summary>
	ResizeBottomLeft,

	/// <summary>The region tested is the left border of the window, and can be used to resize the window from that border</summary>
	ResizeLeft,
}
