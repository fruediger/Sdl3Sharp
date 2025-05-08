using Sdl3Sharp.Interop;
using Sdl3Sharp.SourceGeneration;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sdl3Sharp.Threading;

partial class ReadWriteLock
{
	// Used for opaque pointers
	[StructLayout(LayoutKind.Sequential, Size = 0)]
	internal readonly struct SDL_RWLock;

	/// <summary>
	/// Create a new read/write lock
	/// </summary>
	/// <returns>Returns the initialized and unlocked read/write lock or NULL on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// A read/write lock is useful for situations where you have multiple threads trying to access a resource that is rarely updated. All threads requesting a read-only lock will be allowed to run in parallel; if a thread requests a write lock, it will be provided exclusive access. This makes it safe for multiple threads to use a resource at the same time if they promise not to change it, and when it has to be changed, the rwlock will serve as a gateway to make sure those changes can be made safely.
	/// 
	/// In the right situation, a rwlock can be more efficient than a mutex, which only lets a single thread proceed at a time, even if it won't be modifying the data.
	///
	/// All newly-created read/write locks begin in the <em>unlocked</em> state.
	///
	/// Calls to <see href="https://wiki.libsdl.org/SDL3/SDL_LockRWLockForReading">SDL_LockRWLockForReading</see>() and <see href="https://wiki.libsdl.org/SDL3/SDL_LockRWLockForWriting">SDL_LockRWLockForWriting</see>() will not return while the rwlock is locked <em>for writing</em> by another thread. See <see href="https://wiki.libsdl.org/SDL3/SDL_TryLockRWLockForReading">SDL_TryLockRWLockForReading</see>() and <see href="https://wiki.libsdl.org/SDL3/SDL_TryLockRWLockForWriting">SDL_TryLockRWLockForWriting</see>() to attempt to lock without blocking.
	///
	/// SDL read/write locks are only recursive for read-only locks! They are not guaranteed to be fair, or provide access in a FIFO manner! They are not guaranteed to favor writers. You may not lock a rwlock for both read-only and write access at the same time from the same thread (so you can't promote your read-only lock to a write lock without unlocking first).
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_CreateRWLock">SDL_CreateRWLock</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial SDL_RWLock* SDL_CreateRWLock();

	/// <summary>
	/// Destroy a read/write lock created with <see href="https://wiki.libsdl.org/SDL3/SDL_CreateRWLock">SDL_CreateRWLock</see>()
	/// </summary>
	/// <param name="rwlock">the rwlock to destroy</param>
	/// <remarks>
	/// This function must be called on any read/write lock that is no longer needed.
	/// Failure to destroy a rwlock will result in a system memory or resource leak.
	/// While it is safe to destroy a rwlock that is <em>unlocked</em>, it is not safe to attempt to destroy a locked rwlock, and may result in undefined behavior depending on the platform.
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_DestroyRWLock">SDL_DestroyRWLock</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial void SDL_DestroyRWLock(SDL_RWLock* rwlock);

	/// <summary>
	/// Lock the read/write lock for <em>read only</em> operations
	/// </summary>
	/// <param name="rwlock">the read/write lock to lock</param>
	/// <remarks>
	/// This will block until the rwlock is available, which is to say it is not locked for writing by any other thread. Of all threads waiting to lock the rwlock, all may do so at the same time as long as they are requesting read-only access; if a thread wants to lock for writing, only one may do so at a time, and no other threads, read-only or not, may hold the lock at the same time.
	///
	/// It is legal for the owning thread to lock an already-locked rwlock for reading. It must unlock it the same number of times before it is actually made available for other threads in the system (this is known as a "recursive rwlock").
	///
	/// Note that locking for writing is not recursive (this is only available to read-only locks).
	///
	/// It is illegal to request a read-only lock from a thread that already holds the write lock. Doing so results in undefined behavior. Unlock the write lock before requesting a read-only lock. (But, of course, if you have the write lock, you don't need further locks to read in any case.)
	///
	/// This function does not fail; if rwlock is NULL, it will return immediately having locked nothing. If the rwlock is valid, this function will always block until it can lock the mutex, and return with it locked.
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_LockRWLockForReading">SDL_LockRWLockForReading</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial void SDL_LockRWLockForReading(SDL_RWLock* rwlock);

	/// <summary>
	/// Lock the read/write lock for <em>write</em> operations
	/// </summary>
	/// <param name="rwlock">the read/write lock to lock</param>
	/// <remarks>
	/// This will block until the rwlock is available, which is to say it is not locked for reading or writing by any other thread. Only one thread may hold the lock when it requests write access; all other threads, whether they also want to write or only want read-only access, must wait until the writer thread has released the lock.
	///
	/// It is illegal for the owning thread to lock an already-locked rwlock for writing (read-only may be locked recursively, writing can not). Doing so results in undefined behavior.
	///
	/// It is illegal to request a write lock from a thread that already holds a read-only lock. Doing so results in undefined behavior. Unlock the read-only lock before requesting a write lock.
	///
	/// This function does not fail; if rwlock is NULL, it will return immediately having locked nothing. If the rwlock is valid, this function will always block until it can lock the mutex, and return with it locked.
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_LockRWLockForWriting">SDL_LockRWLockForWriting</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial void SDL_LockRWLockForWriting(SDL_RWLock* rwlock);

	/// <summary>
	/// Try to lock a read/write lock <em>for reading</em> without blocking
	/// </summary>
	/// <param name="rwlock">the rwlock to try to lock</param>
	/// <returns>Returns true on success, false if the lock would block</returns>
	/// <remarks>
	/// This works just like <see href="https://wiki.libsdl.org/SDL3/SDL_LockRWLockForReading">SDL_LockRWLockForReading</see>(), but if the rwlock is not available, then this function returns false immediately.
	///
	/// This technique is useful if you need access to a resource but don't want to wait for it, and will return to it to try again later.
	///
	/// Trying to lock for read-only access can succeed if other threads are holding read-only locks, as this won't prevent access.
	///
	/// This function returns true if passed a NULL rwlock.
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_TryLockRWLockForReading">SDL_TryLockRWLockForReading</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_TryLockRWLockForReading(SDL_RWLock* rwlock);

	/// <summary>
	/// Try to lock a read/write lock <em>for writing</em> without blocking
	/// </summary>
	/// <param name="rwlock">the rwlock to try to lock</param>
	/// <returns>Returns true on success, false if the lock would block</returns>
	/// <remarks>
	/// This works just like <see href="https://wiki.libsdl.org/SDL3/SDL_LockRWLockForWriting">SDL_LockRWLockForWriting</see>(), but if the rwlock is not available, then this function returns false immediately.
	///
	/// This technique is useful if you need exclusive access to a resource but don't want to wait for it, and will return to it to try again later.
	///
	/// It is illegal for the owning thread to lock an already-locked rwlock for writing (read-only may be locked recursively, writing can not). Doing so results in undefined behavior.
	///
	/// It is illegal to request a write lock from a thread that already holds a read-only lock. Doing so results in undefined behavior. Unlock the read-only lock before requesting a write lock.
	///
	/// This function returns true if passed a NULL rwlock.
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_TryLockRWLockForWriting">SDL_TryLockRWLockForWriting</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_TryLockRWLockForWriting(SDL_RWLock* rwlock);

	/// <summary>
	/// Unlock the read/write lock
	/// </summary>
	/// <param name="rwlock">the rwlock to unlock</param>
	/// <remarks>
	/// Use this function to unlock the rwlock, whether it was locked for read-only or write operations.
	///
	/// It is legal for the owning thread to lock an already-locked read-only lock. It must unlock it the same number of times before it is actually made available for other threads in the system (this is known as a "recursive rwlock").
	///
	/// It is illegal to unlock a rwlock that has not been locked by the current thread, and doing so results in undefined behavior.
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_UnlockRWLock">SDL_UnlockRWLock</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial void SDL_UnlockRWLock(SDL_RWLock* rwlock);
}
