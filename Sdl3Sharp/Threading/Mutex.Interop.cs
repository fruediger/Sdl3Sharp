using Sdl3Sharp.Internal.Interop;
using Sdl3Sharp.SourceGeneration;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sdl3Sharp.Threading;

partial class Mutex
{
	// Used for opaque pointers
	[StructLayout(LayoutKind.Sequential, Size = 0)]
	internal readonly struct SDL_Mutex;

	/// <summary>
	/// Create a new mutex
	/// </summary>
	/// <returns>Returns the initialized and unlocked mutex or NULL on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// All newly-created mutexes begin in the <em>unlocked</em> state.
	///
	/// Calls to <see href="https://wiki.libsdl.org/SDL3/SDL_LockMutex">SDL_LockMutex</see>() will not return while the mutex is locked by another thread. See <see href="https://wiki.libsdl.org/SDL3/SDL_TryLockMutex">SDL_TryLockMutex</see>() to attempt to lock without blocking.
	///
	/// SDL mutexes are reentrant.
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_CreateMutex">SDL_CreateMutex</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial SDL_Mutex* SDL_CreateMutex();

	/// <summary>
	/// Destroy a mutex created with <see href="https://wiki.libsdl.org/SDL3/SDL_CreateMutex">SDL_CreateMutex</see>()
	/// </summary>
	/// <param name="mutex">the mutex to destroy</param>
	/// <remarks>
	/// This function must be called on any mutex that is no longer needed.
	/// Failure to destroy a mutex will result in a system memory or resource leak.
	/// While it is safe to destroy a mutex that is <em>unlocked</em>, it is not safe to attempt to destroy a locked mutex, and may result in undefined behavior depending on the platform.
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_DestroyMutex">SDL_DestroyMutex</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial void SDL_DestroyMutex(SDL_Mutex* mutex);


	/// <summary>
	/// Lock the mutex
	/// </summary>
	/// <param name="mutex">the mutex to lock</param>
	/// <remarks>
	/// This will block until the mutex is available, which is to say it is in the unlocked state and the OS has chosen the caller as the next thread to lock it. Of all threads waiting to lock the mutex, only one may do so at a time.
	///
	/// It is legal for the owning thread to lock an already-locked mutex. It must unlock it the same number of times before it is actually made available for other threads in the system (this is known as a "recursive mutex").
	///
	/// This function does not fail; if mutex is NULL, it will return immediately having locked nothing. If the mutex is valid, this function will always block until it can lock the mutex, and return with it locked.
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_LockMutex">SDL_LockMutex</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial void SDL_LockMutex(SDL_Mutex* mutex);

	/// <summary>
	/// Try to lock a mutex without blocking
	/// </summary>
	/// <param name="mutex">the mutex to try to lock</param>
	/// <returns>Returns true on success, false if the mutex would block</returns>
	/// <remarks>
	/// This works just like <see href="https://wiki.libsdl.org/SDL3/SDL_LockMutex">SDL_LockMutex</see>(), but if the mutex is not available, this function returns false immediately.
	///
	/// This technique is useful if you need exclusive access to a resource but don't want to wait for it, and will return to it to try again later.
	///
	/// This function returns true if passed a NULL mutex.
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_TryLockMutex">SDL_TryLockMutex</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_TryLockMutex(SDL_Mutex* mutex);

	/// <summary>
	/// Unlock the mutex
	/// </summary>
	/// <param name="mutex">the mutex to unlock</param>
	/// <remarks>
	/// It is legal for the owning thread to lock an already-locked mutex. It must unlock it the same number of times before it is actually made available for other threads in the system (this is known as a "recursive mutex").
	///
	/// It is illegal to unlock a mutex that has not been locked by the current thread, and doing so results in undefined behavior.
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_UnlockMutex">SDL_UnlockMutex</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial void SDL_UnlockMutex(SDL_Mutex* mutex);
}
