using Sdl3Sharp.Events;

namespace Sdl3Sharp.Input;

/// <summary>
/// Represents the various axes of pen input
/// </summary>
/// <remarks>
/// <para>
/// The values defined here are the valid values for the <see cref="PenAxisEvent.Axis"/> property in the <see cref="PenAxisEvent"/>.
/// <see cref="PenAxisEvent.Value"/>s of these axes are either normalised to the range from <c>0</c> to <c>1</c> or report a (positive or negative) angle in degrees, with <c>0</c> representing the centre.
/// Not all pens/backends support all axes. Values of unsupported axes are always zero.
/// </para>
/// <para>
/// To convert angles for tilt and rotation into vector representation, use the following formula for values (<c>x</c>) of the respective axis:
/// <code>sin(x ⋅ π / 180°)</code>
/// where <c>x</c> is the value of either the <see cref="XTilt"/>, the <see cref="YTilt"/>, or the <see cref="Rotation"/> axis.
/// </para>
/// </remarks>
public enum PenAxis
{
	/// <summary>The pressure applied by the pen</summary>
	/// <remarks>
	/// <para>
	/// Values are unidirectional, in the range from 0 to 1 (no pressure to maximum pressure).
	/// </para>
	/// </remarks>
	Pressure,

	/// <summary>The pen's horizontal tilt angle</summary>
	/// <remarks>
	/// <para>
	/// Values are bidirectional, in the range from -90° to 90° (full left tilt to full right tilt).
	/// </para>
	/// </remarks>
	XTilt,

	/// <summary>The pen's vertical tilt angle</summary>
	/// <remarks>
	/// <para>
	/// Values are bidirectional, in the range from -90° to 90° (full up tilt to full down tilt).
	/// </para>
	/// </remarks>
	YTilt,

	/// <summary>The pen's distance to the drawing surface</summary>
	/// <remarks>
	/// <para>
	/// Values are unidirectional, in the range from 0 to 1 (no distance to maximum distance).
	/// </para>
	/// </remarks>
	Distance,

	/// <summary>The pen's barrel rotation angle</summary>
	/// <remarks>
	/// <para>
	/// Values are bidirectional, in the range from -180° to 179.9° (clockwise, 0 is facing up, -180° is facing down).
	/// </para>
	/// </remarks>
	Rotation,

	/// <summary>The pen's finger wheel or slider position (e.g. for Airbrush pens)</summary>
	/// <remarks>
	/// <para>
	/// Values are unidirectional, in the range from 0 to 1.
	/// </para>
	/// </remarks>
	Slider,

	/// <summary>The pressure applied from squeezing the pen ("barrel pressure")</summary>
	TangentialPressure
}
