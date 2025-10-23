namespace Sdl3Sharp.IO;

/// <summary>
/// Represents the status for a <see cref="Stream"/> as a result of read or write operations
/// </summary>
public enum StreamStatus
{
	/// <summary>Everything is ready (no <see cref="Error">errors</see> and not <see cref="Eof">EOF</see>)</summary>
	Ready,

	/// <summary>Read or write error</summary>
	Error,

	/// <summary>End of file (stream)</summary>
	Eof,

	/// <summary>For non blocking streams: not ready</summary>
	NotReady,

	/// <summary>Tried to write a read-only stream</summary>
	ReadOnly,

	/// <summary>Tried to read a write-only stream</summary>
	WriteOnly
}
