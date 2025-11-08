using System;

namespace Sdl3Sharp.IO;

/// <summary>
/// Represents flags that can be used to modify path matching behavior
/// </summary>
[Flags]
public enum GlobFlags : uint
{
	/// <summary>No special matching behavior</summary>
	None            = 0,

	/// <summary>Case-insensitive matching</summary>
	CaseInsensitive = 0b1 << 0
}
