using Sdl3Sharp.Utilities;
using System.Buffers;

namespace Sdl3Sharp.IO;

partial class AsyncIOOutcome
{
	internal sealed class Managed
	{
		public AsyncIO? AsyncIO { get; set; } = null;
		public MemoryHandle MemoryHandle { get; set; } = default;
		public NativeMemoryPin? NativeMemoryPin { get; set; } = null;
		public object? Userdata { get; set; } = null;
	}
}
