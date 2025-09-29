namespace Sdl3Sharp.Utilities;

partial struct PowerState
{
	internal enum Kind
	{
		Error = -1,
		Unknown,
		OnBattery,
		NoBattery,
		Charging,
		Charged
	}
}
