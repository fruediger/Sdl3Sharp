using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Video.Windowing;

partial class DisplayMode
{
	private sealed class ManagedDisplayMode : DisplayMode
	{
		private SDL_DisplayMode mMode;

		internal override ref readonly SDL_DisplayMode Mode
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
			get => ref mMode;
		}

		internal ref SDL_DisplayMode MutableMode
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
			get => ref mMode;
		}
	}
}
