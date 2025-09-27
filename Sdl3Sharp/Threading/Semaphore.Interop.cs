using Sdl3Sharp.Internal.Interop;
using Sdl3Sharp.SourceGeneration;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sdl3Sharp.Threading;

partial class Semaphore
{
	// Used for opaque pointers
	[StructLayout(LayoutKind.Sequential, Size = 0)]
	internal readonly struct SDL_Semaphore;

	/// <summary>
	/// Create a semaphore
	/// </summary>
	/// <param name="initial_value">the starting value of the semaphore</param>
	/// <returns>Returns a new semaphore or NULL on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// This function creates a new semaphore and initializes it with the value <c>initial_value</c>.
	/// Each wait operation on the semaphore will atomically decrement the semaphore value and potentially block if the semaphore value is 0.
	/// Each post operation will atomically increment the semaphore value and wake waiting threads and allow them to retry the wait operation.
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_CreateSemaphore">SDL_CreateSemaphore</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial SDL_Semaphore* SDL_CreateSemaphore(uint initial_value);

	/// <summary>
	/// Destroy a semaphore
	/// </summary>
	/// <param name="sem">the semaphore to destroy</param>
	/// <remarks>
	/// It is not safe to destroy a semaphore if there are threads currently waiting on it
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_DestroySemaphore">SDL_DestroySemaphore</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial void SDL_DestroySemaphore(SDL_Semaphore* sem);

	/// <summary>
	/// Get the current value of a semaphore
	/// </summary>
	/// <param name="sem">the semaphore to query</param>
	/// <returns>Returns the current value of the semaphore</returns>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetSemaphoreValue">SDL_GetSemaphoreValue</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial uint SDL_GetSemaphoreValue(SDL_Semaphore* sem);

	/// <summary>
	/// Atomically increment a semaphore's value and wake waiting threads
	/// </summary>
	/// <param name="sem">the semaphore to increment</param>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_SignalSemaphore">SDL_SignalSemaphore</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial void SDL_SignalSemaphore(SDL_Semaphore* sem);

	/// <summary>
	/// See if a semaphore has a positive value and decrement it if it does
	/// </summary>
	/// <param name="sem">the semaphore to wait on</param>
	/// <returns>Returns true if the wait succeeds, false if the wait would block</returns>
	/// <remarks>
	/// This function checks to see if the semaphore pointed to by <c>sem</c> has a positive value and atomically decrements the semaphore value if it does.
	/// If the semaphore doesn't have a positive value, the function immediately returns false.
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_TryWaitSemaphore">SDL_TryWaitSemaphore</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_TryWaitSemaphore(SDL_Semaphore* sem);

	/// <summary>
	/// Wait until a semaphore has a positive value and then decrements it
	/// </summary>
	/// <param name="sem">the semaphore wait on</param>
	/// <remarks>
	/// This function suspends the calling thread until the semaphore pointed to by <c>sem</c> has a positive value, and then atomically decrement the semaphore value.
	///
	/// This function is the equivalent of calling <see href="https://wiki.libsdl.org/SDL3/SDL_WaitSemaphoreTimeout">SDL_WaitSemaphoreTimeout</see>() with a time length of -1.
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_WaitSemaphore">SDL_WaitSemaphore</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial void SDL_WaitSemaphore(SDL_Semaphore* sem);

	/// <summary>
	/// Wait until a semaphore has a positive value and then decrements it
	/// </summary>
	/// <param name="sem">the semaphore to wait on</param>
	/// <param name="timeoutMS">the length of the timeout, in milliseconds, or -1 to wait indefinitely</param>
	/// <returns>Returns true if the wait succeeds or false if the wait times out</returns>
	/// <remarks>
	/// This function suspends the calling thread until either the semaphore pointed to by <c>sem</c> has a positive value or the specified time has elapsed.
	/// If the call is successful it will atomically decrement the semaphore value.
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_WaitSemaphoreTimeout">SDL_WaitSemaphoreTimeout</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_WaitSemaphoreTimeout(SDL_Semaphore* sem, int timeoutMS);
}
