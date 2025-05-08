using System.Runtime.InteropServices;

namespace Sdl3Sharp.Threading.Tasks;

partial struct AsyncIO
{
	[StructLayout(LayoutKind.Sequential, Size = 0)]
	internal readonly struct SDL_AsyncIO;
}
