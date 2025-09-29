namespace Sdl3Sharp.IO;

public enum StreamWhence
{
	/// <summary><see cref="Seek"/> from the beginning of the stream</summary>
	Set,

	/// <summary><see cref="Seek"/> relative to the current position inside of the stream</summary>
	Current,

	/// <summary><see cref="Seek"/> relative to the end of the stream</summary>
	End
}
