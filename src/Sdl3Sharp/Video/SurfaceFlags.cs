using System;

namespace Sdl3Sharp.Video;

/// <summary>
/// Represents flags for a <see cref="Surface"/>
/// </summary>
[Flags]
public enum SurfaceFlags : uint
{
	/// <summary>The <see cref="Surface"/> uses pre-allocated pixel memory</summary>
	/// <seealso cref="Surface.IsPreAllocated"/>
	PreAllocated = 0x00000001u,

	/// <summary>Locking is needed for the <see cref="Surface"/> before accessing it's pixel memory</summary>
	/// <seealso cref="Surface.MustLock"/>
	LockNeeded = 0x00000002u,

	/// <summary>The <see cref="Surface"/> is currently locked</summary>
	/// <seealso cref="Surface.IsLocked"/>
	Locked = 0x00000004u,

	/// <summary>The <see cref="Surface"/> uses pixel memory that's aligned for SIMD operations</summary>
	/// <seealso cref="Surface.IsSimdAligned"/>
	SimdAligned = 0x00000008u
}
