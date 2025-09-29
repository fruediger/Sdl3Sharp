using System.Runtime.InteropServices;

namespace Sdl3Sharp.Threading.Tasks;

[StructLayout(LayoutKind.Sequential)]
public readonly partial struct AsyncIOOutcome
{
	private unsafe readonly AsyncIO.SDL_AsyncIO* mAsyncio;
	private readonly AsyncIOTaskType mType;
	private readonly AsyncIOResult mResult;
	private unsafe readonly void* mBuffer;
	private readonly ulong mOffset;
	private readonly ulong mBytesRequested;
	private readonly ulong mBytesTransferred;
	private unsafe readonly void* mUserdata;
}
