using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Threading;

public static class SpinLockExtensions
{
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void Lock(this ref SpinLock spinLock) => SpinLock.Lock(ref spinLock);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool TryLock(this ref SpinLock spinLock) => SpinLock.TryLock(ref spinLock);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void Unlock(this ref SpinLock spinLock) => SpinLock.Unlock(ref spinLock);
}
