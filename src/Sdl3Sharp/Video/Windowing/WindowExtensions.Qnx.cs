using Sdl3Sharp.Video.Windowing.Drivers;
using System;

namespace Sdl3Sharp.Video.Windowing;

partial class WindowExtensions
{
	extension(Window<Qnx>.PropertyNames)
	{
		public static string QnxWindowPointer => "SDL.window.qnx.window";

		public static string QnxSurfacePointer => "SDL.window.qnx.surface";
	}

	extension(Window<Qnx> window)
	{
		public IntPtr QnxWindow => window?.Properties?.TryGetPointerValue(Window<Qnx>.PropertyNames.QnxWindowPointer, out var qnxWindowPtr) is true
			? qnxWindowPtr
			: default;

		public IntPtr QnxSurface => window?.Properties?.TryGetPointerValue(Window<Qnx>.PropertyNames.QnxSurfacePointer, out var qnxSurfacePtr) is true
			? qnxSurfacePtr
			: default;
	}
}
