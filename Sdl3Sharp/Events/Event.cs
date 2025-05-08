using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sdl3Sharp.Events;

[StructLayout(LayoutKind.Explicit)]
public readonly partial struct Event
{
	[FieldOffset(0)] public readonly EventType Type;

	[FieldOffset(0)] private readonly CommonEvent mCommonEvent;

	[FieldOffset(0)] private readonly Padding mPadding;

#pragma warning disable IDE0044
#pragma warning disable IDE0051
	[InlineArray(128)] private struct Padding { private byte _; }
#pragma warning restore IDE0051
#pragma warning restore IDE0044
}
