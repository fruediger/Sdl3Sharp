using Sdl3Sharp.Video.Windowing.Drivers;
using System;

namespace Sdl3Sharp.Video.Windowing;

partial class WindowExtensions
{
	extension(Window<Android>.PropertyNames)
	{
		public static string AndroidWindowPointer => "SDL.window.android.window";

		public static string AndroidSurfacePointer => "SDL.window.android.surface";
	}

	extension(Window<Android> window)
	{
		public IntPtr AndroidWindow => window?.Properties?.TryGetPointerValue(Window<Android>.PropertyNames.AndroidWindowPointer, out var androidWindowPtr) is true
			? androidWindowPtr
			: default;

		public IntPtr AndroidSurface => window?.Properties?.TryGetPointerValue(Window<Android>.PropertyNames.AndroidSurfacePointer, out var androidSurfacePtr) is true
			? androidSurfacePtr
			: default;
	}
}
