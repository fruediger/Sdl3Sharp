using System.Runtime.InteropServices;

namespace Sdl3Sharp.Video.Windowing;

partial struct DisplayMode
{
	// opaque struct
	[StructLayout(LayoutKind.Sequential, Size = 0)]
	internal readonly struct SDL_DisplayModeData;
}
