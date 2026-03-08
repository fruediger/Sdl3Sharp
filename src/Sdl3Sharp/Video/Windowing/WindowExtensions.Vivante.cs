using Sdl3Sharp.Video.Windowing.Drivers;
using System;

namespace Sdl3Sharp.Video.Windowing;

partial class WindowExtensions
{
	extension(Window<Vivante>.PropertyNames)
	{
		public static string VivanteDisplayPointer => "SDL.window.vivante.display";

		public static string VivanteWindowPointer => "SDL.window.vivante.window";

		public static string VivanteSurfacePointer => "SDL.window.vivante.surface";
	}

	extension(Window<Vivante> window)
	{
		public IntPtr VivanteDisplay => window?.Properties?.TryGetPointerValue(Window<Vivante>.PropertyNames.VivanteDisplayPointer, out var vivanteDisplayPtr) is true
			? vivanteDisplayPtr
			: default;

		public IntPtr VivanteWindow => window?.Properties?.TryGetPointerValue(Window<Vivante>.PropertyNames.VivanteWindowPointer, out var vivanteWindowPtr) is true
			? vivanteWindowPtr
			: default;

		public IntPtr VivanteSurface => window?.Properties?.TryGetPointerValue(Window<Vivante>.PropertyNames.VivanteSurfacePointer, out var vivanteSurfacePtr) is true
			? vivanteSurfacePtr
			: default;
	}
}
