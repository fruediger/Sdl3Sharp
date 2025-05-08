using Sdl3Sharp.Interop;
using Sdl3Sharp.SourceGeneration;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Threading;

partial struct SpinLock
{
	/// <summary>
	/// Lock a spin lock by setting it to a non-zero value
	/// </summary>
	/// <param name="lock">a pointer to a lock variable</param>
	/// <remarks>
	/// <em>Please note that spinlocks are dangerous if you don't know what you're doing. Please be careful using any sort of spinlock!</em>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_LockSpinlock">SDL_LockSpinlock</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl), typeof(CallConvSuppressGCTransition)])]
	internal unsafe static partial void SDL_LockSpinlock(SpinLock* @lock);

	/// <summary>
	/// Try to lock a spin lock by setting it to a non-zero value
	/// </summary>
	/// <param name="lock">a pointer to a lock variable</param>
	/// <returns>Returns true if the lock succeeded, false if the lock is already held</returns>
	/// <remarks>
	/// <em>Please note that spinlocks are dangerous if you don't know what you're doing. Please be careful using any sort of spinlock!</em>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_TryLockSpinlock">SDL_TryLockSpinlock</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl), typeof(CallConvSuppressGCTransition)])]
	internal unsafe static partial CBool SDL_TryLockSpinlock(SpinLock* @lock);


	/// <summary>
	/// Unlock a spin lock by setting it to 0
	/// </summary>
	/// <param name="lock">a pointer to a lock variable</param>
	/// <remarks>
	/// Always returns immediately.
	///
	/// <em>Please note that spinlocks are dangerous if you don't know what you're doing. Please be careful using any sort of spinlock!</em>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_UnlockSpinlock">SDL_UnlockSpinlock</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl), typeof(CallConvSuppressGCTransition)])]
	internal unsafe static partial void SDL_UnlockSpinlock(SpinLock* @lock);
}
