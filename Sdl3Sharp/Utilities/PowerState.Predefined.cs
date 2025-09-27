using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Utilities;

partial struct PowerState
{
	/// <summary>
	/// Gets a representative for an erroneous <see cref="PowerState"/>
	/// </summary>
	/// <value>
	/// A representative for an erroneous <see cref="PowerState"/>
	/// </value>
	/// <remarks>
	/// <para>
	/// If the resulting value of a call to <see cref="GetInfo(out int, out int)"/> is equal to the value of this property, it means that determining the power state failed.
	/// You can check <see cref="Error.TryGet(out string?)"/> for more information.
	/// </para>
	/// </remarks>
	public static PowerState Error { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Error); }

	/// <summary>
	/// Gets a representative for an unknown <see cref="PowerState"/>
	/// </summary>
	/// <value>
	/// A representative for an unknown <see cref="PowerState"/>
	/// </value>
	/// <remarks>
	/// <para>
	/// If the resulting value of a call to <see cref="GetInfo(out int, out int)"/> is equal to the value of this property, it means that the power state couldn't be determined.
	/// </para>
	/// </remarks>
	public static PowerState Unknown { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Unknown); }

	/// <summary>
	/// Gets the <see cref="PowerState"/> for "the device is running on battery, and is not plugged in"
	/// </summary>
	/// <value>
	/// The <see cref="PowerState"/> for "the device is running on battery, and is not plugged in"
	/// </value>
	public static PowerState OnBattery { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.OnBattery); }

	/// <summary>
	/// Gets the <see cref="PowerState"/> for "the device is plugged in, and no battery is available"
	/// </summary>
	/// <value>
	/// The <see cref="PowerState"/> for "the device is plugged in, and no battery is available"
	/// </value>
	public static PowerState NoBattery { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.NoBattery); }

	/// <summary>
	/// Gets the <see cref="PowerState"/> for "the device is plugged in, and is charging the battery"
	/// </summary>
	/// <value>
	/// The <see cref="PowerState"/> for "the device is plugged in, and is charging the battery"
	/// </value>
	public static PowerState Charging { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Charging); }

	/// <summary>
	/// Gets the <see cref="PowerState"/> for "the device is plugged in, and the battery is fully charged"
	/// </summary>
	/// <value>
	/// The <see cref="PowerState"/> for "the device is plugged in, and the battery is fully charged"
	/// </value>
	public static PowerState Charged { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Charged); }
}
