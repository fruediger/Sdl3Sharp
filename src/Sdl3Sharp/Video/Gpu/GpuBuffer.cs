using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Video.Gpu;

public partial class GpuBuffer
{
	private unsafe SDL_GPUBuffer* mBuffer;

	internal unsafe SDL_GPUBuffer* Pointer { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mBuffer; }
}
