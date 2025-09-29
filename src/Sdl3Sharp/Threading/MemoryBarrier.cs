using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Threading;

public static partial class MemoryBarrier
{
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void Acquire() => SDL_MemoryBarrierAcquireFunction();

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void Release() => SDL_MemoryBarrierReleaseFunction();
}
