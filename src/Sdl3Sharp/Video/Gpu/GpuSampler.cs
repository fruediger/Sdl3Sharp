using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Video.Gpu;

public sealed partial class GpuSampler
{
	private unsafe SDL_GPUSampler* mSampler;

	internal unsafe SDL_GPUSampler* Pointer { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mSampler; }
}
