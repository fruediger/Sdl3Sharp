using Sdl3Sharp.Internal.Interop;
using Sdl3Sharp.SourceGeneration;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sdl3Sharp.Threading;

partial class Condition
{
	// Used for opaque pointers
	[StructLayout(LayoutKind.Sequential, Size = 0)]
	internal readonly struct SDL_Condition;

	/// <summary>
	/// Restart all threads that are waiting on the condition variable
	/// </summary>
	/// <param name="cond">the condition variable to signal</param>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_BroadcastCondition">SDL_BroadcastCondition</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial void SDL_BroadcastCondition(SDL_Condition* cond);

	/// <summary>
	/// Create a condition variable
	/// </summary>
	/// <returns>Returns a new condition variable or NULL on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_CreateCondition">SDL_CreateCondition</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial SDL_Condition* SDL_CreateCondition();

	/// <summary>
	/// Destroy a condition variable
	/// </summary>
	/// <param name="cond">the condition variable to destroy</param>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_DestroyCondition">SDL_DestroyCondition</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial void SDL_DestroyCondition(SDL_Condition* cond);

	/// <summary>
	/// Restart one of the threads that are waiting on the condition variable
	/// </summary>
	/// <param name="cond">the condition variable to signal</param>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_SignalCondition">SDL_SignalCondition</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial void SDL_SignalCondition(SDL_Condition* cond);

	/// <summary>
	/// Wait until a condition variable is signaled
	/// </summary>
	/// <param name="cond">the condition variable to wait on</param>
	/// <param name="mutex">the mutex used to coordinate thread access</param>
	/// <remarks>
	/// This function unlocks the specified <c>mutex</c> and waits for another thread to call <see href="https://wiki.libsdl.org/SDL3/SDL_SignalCondition">SDL_SignalCondition</see>() or <see href="https://wiki.libsdl.org/SDL3/SDL_BroadcastCondition">SDL_BroadcastCondition</see>() on the condition variable <c>cond</c>. Once the condition variable is signaled, the mutex is re-locked and the function returns.
	/// 
	/// The mutex must be locked before calling this function. Locking the mutex recursively (more than once) is not supported and leads to undefined behavior.
	///
	/// This function is the equivalent of calling <see href="https://wiki.libsdl.org/SDL3/SDL_WaitConditionTimeout">SDL_WaitConditionTimeout</see>() with a time length of -1.
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_WaitCondition">SDL_WaitCondition</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial void SDL_WaitCondition(SDL_Condition* cond, Mutex.SDL_Mutex* mutex);


	/// <summary>
	/// Wait until a condition variable is signaled or a certain time has passed
	/// </summary>
	/// <param name="cond">the condition variable to wait on</param>
	/// <param name="mutex">the mutex used to coordinate thread access</param>
	/// <param name="timeoutMS">the maximum time to wait, in milliseconds, or -1 to wait indefinitely</param>
	/// <returns>Returns true if the condition variable is signaled, false if the condition is not signaled in the allotted time</returns>
	/// <remarks>
	/// This function unlocks the specified <c>mutex</c> and waits for another thread to call <see href="https://wiki.libsdl.org/SDL3/SDL_SignalCondition">SDL_SignalCondition</see>() or <see href="https://wiki.libsdl.org/SDL3/SDL_BroadcastCondition">SDL_BroadcastCondition</see>() on the condition variable <c>cond</c>, or for the specified time to elapse. Once the condition variable is signaled or the time elapsed, the mutex is re-locked and the function returns.
	///
	/// The mutex must be locked before calling this function. Locking the mutex recursively (more than once) is not supported and leads to undefined behavior.
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_WaitConditionTimeout">SDL_WaitConditionTimeout</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_WaitConditionTimeout(SDL_Condition* cond, Mutex.SDL_Mutex* mutex, int timeoutMS);
}
