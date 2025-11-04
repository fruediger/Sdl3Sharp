using System.Runtime.InteropServices;

namespace Sdl3Sharp.IO;

partial class AsyncIOOutcome
{
	[StructLayout(LayoutKind.Sequential)]
	internal readonly struct SDL_AsyncIOOutcome
	{
		public unsafe readonly AsyncIO.SDL_AsyncIO* AsyncIO;
		public readonly AsyncIOTaskType Type;
		public readonly AsyncIOResult Result;
		public unsafe readonly void* Buffer;
		public readonly ulong Offset;
		public readonly ulong BytesRequested;
		public readonly ulong BytesTransferred;
		public unsafe readonly void* Userdata;
	}
}
