using Sdl3Sharp.Interop;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp;

partial struct SubSystem
{
	/// <summary>
	/// Gets the sub system <c>Audio</c>
	/// </summary>
	/// <value>
	/// The sub system <c>Audio</c>
	/// </value>
	/// <remarks>
	/// Initializing <c><see cref="Audio"/></c> implies initializing <c><see cref="Events"/></c>
	/// </remarks>
	public static SubSystem Audio { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(InitFlags.Audio); }

	/// <summary>
	/// Gets the sub system <c>Video</c>
	/// </summary>
	/// <value>
	/// The sub system <c>Video</c>
	/// </value>
	/// <remarks>
	/// <para>
	/// Initializing <c><see cref="Video"/></c> implies initializing <c><see cref="Events"/></c>.
	/// </para>
	/// <para>
	/// This sub system should be initialized on the main thread.
	/// </para>
	/// </remarks>
	public static SubSystem Video { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(InitFlags.Video); }

	/// <summary>
	/// Gets the sub system <c>Joystick</c>
	/// </summary>
	/// <value>
	/// The sub system <c>Joystick</c>
	/// </value>
	/// <remarks>
	/// <para>
	/// Initializing <c><see cref="Joystick"/></c> implies initializing <c><see cref="Events"/></c>.
	/// </para>
	/// <para>
	/// This sub system should be initialized on the same thread as <c><see cref="Video"/></c> on Windows (which should be the main thread as required by <c><see cref="Video"/></c>),
	/// if you don't set the hint <see cref="Hint.Joystick.Thread"/>.
	/// </para>
	/// </remarks>
	public static SubSystem Joystick { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(InitFlags.Joystick); }

	/// <summary>
	/// Gets the sub system <c>Haptic</c>
	/// </summary>
	/// <value>
	/// The sub system <c>Haptic</c>
	/// </value>
	public static SubSystem Haptic { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(InitFlags.Haptic); }

	/// <summary>
	/// Gets the sub system <c>Gamepad</c>
	/// </summary>
	/// <value>
	/// The sub system <c>Gamepad</c>
	/// </value>
	/// <remarks>
	/// Initializing <c><see cref="Gamepad"/></c> implies initializing <c><see cref="Joystick"/></c>
	/// </remarks>
	public static SubSystem Gamepad { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(InitFlags.Gamepad); }

	/// <summary>
	/// Gets the sub system <c>Events</c>
	/// </summary>
	/// <value>
	/// The sub system <c>Events</c>
	/// </value>
	public static SubSystem Events { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(InitFlags.Events); }

	/// <summary>
	/// Gets the sub system <c>Sensor</c>
	/// </summary>
	/// <value>
	/// The sub system <c>Sensor</c>
	/// </value>
	/// <remarks>
	/// Initializing <c><see cref="Sensor"/></c> implies initializing <c><see cref="Events"/></c>
	/// </remarks>
	public static SubSystem Sensor { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(InitFlags.Sensor); }

	/// <summary>
	/// Gets the sub system <c>Camera</c>
	/// </summary>
	/// <value>
	/// The sub system <c>Camera</c>
	/// </value>
	/// <remarks>
	/// Initializing <c><see cref="Camera"/></c> implies initializing <c><see cref="Events"/></c>
	/// </remarks>
	public static SubSystem Camera { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(InitFlags.Camera); }

	private static string? KnownInitFlagToString(InitFlags initFlag) => initFlag switch
	{
		InitFlags.Audio => nameof(Audio),
		InitFlags.Video => nameof(Video),
		InitFlags.Joystick => nameof(Joystick),
		InitFlags.Haptic => nameof(Haptic),
		InitFlags.Gamepad => nameof(Gamepad),
		InitFlags.Events => nameof(Events),
		InitFlags.Sensor => nameof(Sensor),
		InitFlags.Camera => nameof(Camera),
		_ => null
	};
}
