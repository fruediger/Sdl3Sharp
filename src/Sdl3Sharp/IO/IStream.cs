namespace Sdl3Sharp.IO;

public interface IStream
{
	long Length { get; }

	long Seek(long offset, StreamWhence whence);

	nuint Read(ref byte data, nuint length, ref StreamStatus status);

	nuint Write(ref readonly byte data, nuint length, ref StreamStatus status);

	bool Flush(ref StreamStatus status);

	bool Close();
}
