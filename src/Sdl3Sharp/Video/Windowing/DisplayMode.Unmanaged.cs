using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Video.Windowing;

partial class DisplayMode
{
	[method: MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private unsafe sealed class Unmanaged(SDL_DisplayMode* mode) : DisplayMode
	{
		private readonly SDL_DisplayMode* mMode = mode;

		internal sealed override ref readonly SDL_DisplayMode Mode
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
			get => ref Unsafe.AsRef<SDL_DisplayMode>(mMode);
		}
	}
}
