using System;

namespace Sdl3Sharp.Video;

[Flags]
public enum SurfaceFlags : uint
{
	PreAllocated = 0x00000001u,
	LockNeeded = 0x00000002u,
	Locked = 0x00000004u,
	SimdAligned = 0x00000008u
}
