using System;

namespace Sdl3Sharp;

/// <summary>
/// Represents a sub system which can be initialized by SDL and used in an application
/// </summary>
[Flags]
public enum SubSystems : uint
{
	/// <summary>No sub systems</summary>
	None = 0,

	/// <summary>The <em>Audio</em> sub system</summary> 
	/// <remarks>
	/// <para>
	/// Initializing <em><see cref="Audio"/></em> implies initializing <em><see cref="Events"/></em>
	/// </para>
	/// </remarks>
	Audio = 0x00000010u,

	/// <summary>The <em>Video</em> sub system</summary>
	/// <remarks>
	/// <para>
	/// Initializing <em><see cref="Video"/></em> implies initializing <em><see cref="Events"/></em>.
	/// </para>
	/// <para>
	/// This sub system should be initialized on the main thread.
	/// </para>
	/// </remarks>
	Video = 0x00000020u,

	/// <summary>The <em>Joystick</em> sub system</summary>
	/// <remarks>
	/// <para>
	/// Initializing <em><see cref="Joystick"/></em> implies initializing <em><see cref="Events"/></em>.
	/// </para>
	/// <para>
	/// This sub system should be initialized on the same thread as <em><see cref="Video"/></em> on Windows (which should be the main thread as required by <em><see cref="Video"/></em>),
	/// if you don't set the hint <see cref="Hint.Joystick.Thread"/>.
	/// </para>
	/// </remarks>
	Joystick = 0x00000200u,

	/// <summary>The <em>Haptic</em> sub system</summary>
	Haptic = 0x00001000u,

	/// <summary>The <em>Gamepad</em> sub system</summary>
	/// <remarks>
	/// <para>
	/// Initializing <em><see cref="Gamepad"/></em> implies initializing <em><see cref="Joystick"/></em>.
	/// </para>
	/// </remarks>
	Gamepad = 0x00002000u,

	/// <summary>The <em>Events</em> sub system</summary>
	Events = 0x00004000u,

	/// <summary>The <em>Sensor</em> sub system</summary>
	/// <remarks>
	/// <para>
	/// Initializing <em><see cref="Sensor"/></em> implies initializing <em><see cref="Events"/></em>.
	/// </para>
	/// </remarks>
	Sensor = 0x00008000u,

	/// <summary>The <em>Camera</em> sub system</summary>
	/// <remarks>
	/// <para>
	/// Initializing <em><see cref="Camera"/></em> implies initializing <em><see cref="Events"/></em>
	/// </para>
	/// </remarks>
	Camera = 0x00010000u
}
