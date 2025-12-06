namespace Sdl3Sharp.Utilities;

/// <summary>
/// Represents the system's power supply state or a battery state
/// </summary>
public enum PowerState
{
	/// <summary>A representative for an erroneous <see cref="PowerState"/></summary>
	/// <remarks>
	/// <para>
	/// If the resulting value of a call to <see cref="PowerStateExtensions.GetInfo(out int, out int)"/> is equal to the value of this property, it means that determining the power state failed.
	/// You can check <see cref="Error.TryGet(out string?)"/> for more information.
	/// </para>
	/// </remarks>
	Error = -1,

	/// <summary>A representative for an unknown <see cref="PowerState"/></summary>
	/// <remarks>
	/// <para>
	/// If the resulting value of a call to <see cref="PowerStateExtensions.GetInfo(out int, out int)"/> is equal to the value of this property, it means that the power state couldn't be determined.
	/// </para>
	/// </remarks>
	Unknown,

	/// <summary>The <see cref="PowerState"/> for "the device is running on battery, and is not plugged in"</summary>
	OnBattery,

	/// <summary>The <see cref="PowerState"/> for "the device is plugged in, and no battery is available"</summary>
	NoBattery,

	/// <summary>The <see cref="PowerState"/> for "the device is plugged in, and is charging the battery"</summary>
	Charging,

	/// <summary>The <see cref="PowerState"/> for "the device is plugged in, and the battery is fully charged"</summary>
	Charged
}
