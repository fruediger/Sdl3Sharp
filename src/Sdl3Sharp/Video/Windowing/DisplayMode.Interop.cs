using Sdl3Sharp.Video.Coloring;
using System.Runtime.InteropServices;

namespace Sdl3Sharp.Video.Windowing;

partial class DisplayMode
{
	[StructLayout(LayoutKind.Sequential)]
	internal unsafe readonly struct SDL_DisplayMode
	{
		public readonly uint DisplayID;
		public readonly PixelFormat Format ;
		public readonly int W;
		public readonly int H;
		public readonly float PixelDensity;
		public readonly float RefreshRate;
		public readonly int RefreshRateNumerator;
		public readonly int RefreshRateDenominator;
		public readonly SDL_DisplayModeData* Internal;
	}

	// opaque struct
	[StructLayout(LayoutKind.Sequential, Size = 0)]
	internal readonly struct SDL_DisplayModeData;
}
