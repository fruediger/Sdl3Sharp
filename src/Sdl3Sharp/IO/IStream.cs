using Sdl3Sharp.Utilities;

namespace Sdl3Sharp.IO;

public interface IStream
{
	long Length { get; }

	long Seek(long offset, StreamWhence whence);

	nuint Read(NativeMemory data, ref StreamStatus status);

	nuint Write(ReadOnlyNativeMemory data, ref StreamStatus status);

	bool Flush(ref StreamStatus status);

	bool Close();
}
