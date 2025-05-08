using System.Runtime.InteropServices;

namespace Sdl3Sharp.Events;

[StructLayout(LayoutKind.Sequential)]
public readonly struct CommonEvent
{
	public readonly EventType Type;

	private readonly uint mReserved;

	public readonly ulong Timestamp;
}
