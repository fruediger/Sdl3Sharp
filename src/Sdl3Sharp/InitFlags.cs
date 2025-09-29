using System;

namespace Sdl3Sharp;

/// <summary>
/// Initialization flags for <see href="https://wiki.libsdl.org/SDL3/SDL_Init">SDL_Init</see> and/or <see href="https://wiki.libsdl.org/SDL3/SDL_InitSubSystem">SDL_InitSubSystem</see>
/// </summary>
/// <remarks>
/// These are the flags which may be passed to <see href="https://wiki.libsdl.org/SDL3/SDL_Init">SDL_Init</see>(). You should specify the subsystems which you will be using in your application.
/// </remarks>
/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_InitFlags">SDL_InitFlags</seealso>
[Flags]
internal enum InitFlags : uint
{
	/// <summary>audio subsystem; automatically initializes the events subsystem</summary>
	/// <remarks>`SDL_INIT_AUDIO` implies `SDL_INIT_EVENTS`</remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_INIT_AUDIO">SDL_INIT_AUDIO</seealso>
	Audio = 0x00000010u,

	/// <summary>video subsystem; automatically initializes the events subsystem, should be initialized on the main thread</summary>
	/// <remarks>`SDL_INIT_VIDEO` implies `SDL_INIT_EVENTS`, should be initialized on the main thread</remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_INIT_VIDEO">SDL_INIT_VIDEO</seealso>
	Video = 0x00000020u,

	/// <summary>joystick subsystem; automatically initializes the events subsystem</summary>
	/// <remarks>`SDL_INIT_JOYSTICK` implies `SDL_INIT_EVENTS`, should be initialized on the same thread as SDL_INIT_VIDEO on Windows if you don't set SDL_HINT_JOYSTICK_THREAD</remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_INIT_JOYSTICK">SDL_INIT_JOYSTICK</seealso>
	Joystick = 0x00000200u,

	/// <summary>haptic (force feedback) subsystem</summary>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_INIT_HAPTIC">SDL_INIT_HAPTIC</seealso>
	Haptic = 0x00001000u,

	/// <summary>gamepad subsystem; automatically initializes the joystick subsystem</summary>
	/// <remarks>`SDL_INIT_GAMEPAD` implies `SDL_INIT_JOYSTICK`</remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_INIT_GAMEPAD">SDL_INIT_GAMEPAD</seealso>
	Gamepad = 0x00002000u,

	/// <summary>events subsystem</summary>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_INIT_EVENTS">SDL_INIT_EVENTS</seealso>
	Events = 0x00004000u,

	/// <summary>sensor subsystem; automatically initializes the events subsystem</summary>
	/// <remarks>`SDL_INIT_SENSOR` implies `SDL_INIT_EVENTS`</remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_INIT_SENSOR">SDL_INIT_SENSOR</seealso>
	Sensor = 0x00008000u,

	/// <summary>camera subsystem; automatically initializes the events subsystem</summary>
	/// <remarks>`SDL_INIT_CAMERA` implies `SDL_INIT_EVENTS`</remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_INIT_CAMERA">SDL_INIT_CAMERA</seealso>
	Camera = 0x00010000u,

	AllKnown = Audio | Video | Joystick | Haptic | Gamepad | Events | Sensor | Camera
}
