namespace Sdl3Sharp.IO;

/// <summary>
/// Represents the reference point for seeking inside of a <see cref="Stream"/>
/// </summary>
public enum StreamWhence
{
	/// <summary><see cref="Stream.TrySeek(long, StreamWhence, out long)">Seek</see> from the beginning of the stream</summary>
	Set,

	/// <summary><see cref="Stream.TrySeek(long, StreamWhence, out long)">Seek</see> relative to the current position inside of the stream</summary>
	Current,

	/// <summary><see cref="Stream.TrySeek(long, StreamWhence, out long)">Seek</see> relative to the end of the stream</summary>
	End
}
