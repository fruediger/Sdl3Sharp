using Sdl3Sharp.Internal.Interop;
using Sdl3Sharp.SourceGeneration;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sdl3Sharp.IO;

partial class AsyncIOQueue
{
	// Used for opaque pointers
	[StructLayout(LayoutKind.Sequential, Size = 0)]
	internal readonly struct SDL_AsyncIOQueue;

	/// <summary>
	/// Create a task queue for tracking multiple I/O operations
	/// </summary>
	/// <returns>Returns a new task queue object or NULL if there was an error; call <see href="https://wiki.libsdl.org/SDL3/SDL_CreateAsyncIOQueue">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// Async I/O operations are assigned to a queue when started. The queue can be checked for completed tasks thereafter.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_CreateAsyncIOQueue">SDL_CreateAsyncIOQueue</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial SDL_AsyncIOQueue* SDL_CreateAsyncIOQueue();

	/// <summary>
	/// Destroy a previously-created async I/O task queue
	/// </summary>
	/// <param name="queue">The task queue to destroy</param>
	/// <remarks>
	/// <para>
	/// If there are still tasks pending for this queue, this call will block until those tasks are finished. All those tasks will be deallocated. Their results will be lost to the app.
	/// </para>
	/// <para>
	/// Any pending reads from <see href="https://wiki.libsdl.org/SDL3/SDL_LoadFileAsync">SDL_LoadFileAsync</see>() that are still in this queue will have their buffers deallocated by this function, to prevent a memory leak.
	/// </para>
	/// <para>
	/// Once this function is called, the queue is no longer valid and should not be used, including by other threads that might access it while destruction is blocking on pending tasks.
	/// </para>
	/// <para>
	/// Do not destroy a queue that still has threads waiting on it through <see href="https://wiki.libsdl.org/SDL3/SDL_WaitAsyncIOResult">SDL_WaitAsyncIOResult</see>().
	/// You can call <see href="https://wiki.libsdl.org/SDL3/SDL_SignalAsyncIOQueue">SDL_SignalAsyncIOQueue</see>() first to unblock those threads,
	/// and take measures (such as <see href="https://wiki.libsdl.org/SDL3/SDL_WaitThread">SDL_WaitThread</see>()) to make sure they have finished their wait and won't wait on the queue again.
	/// </para>
	/// <para>
	/// It is safe to call this function from any thread, so long as no other thread is waiting on the queue with <see href="https://wiki.libsdl.org/SDL3/SDL_WaitAsyncIOResult">SDL_WaitAsyncIOResult</see>.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_DestroyAsyncIOQueue">SDL_DestroyAsyncIOQueue</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial void SDL_DestroyAsyncIOQueue(SDL_AsyncIOQueue* queue);	

	/// <summary>
	/// Query an async I/O task queue for completed tasks
	/// </summary>
	/// <param name="queue">The async I/O task queue to query</param>
	/// <param name="outcome">Details of a finished task will be written here. May not be NULL.</param>
	/// <returns>Returns true if a task has completed, false otherwise</returns>
	/// <remarks>
	/// <para>
	/// If a task assigned to this queue has finished, this will return true and fill in <c><paramref name="outcome"/></c> with the details of the task.
	/// If no task in the queue has finished, this function will return false. This function does not block.
	/// </para>
	/// <para>
	/// If a task has completed, this function will free its resources and the task pointer will no longer be valid. The task will be removed from the queue.
	/// </para>
	/// <para>
	/// It is safe for multiple threads to call this function on the same queue at once; a completed task will only go to one of the threads.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetAsyncIOResult">SDL_GetAsyncIOResult</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_GetAsyncIOResult(SDL_AsyncIOQueue* queue, AsyncIOOutcome.SDL_AsyncIOOutcome* outcome);

	/// <summary>
	/// Wake up any threads that are blocking in <see href="https://wiki.libsdl.org/SDL3/SDL_WaitAsyncIOResult">SDL_WaitAsyncIOResult</see>()
	/// </summary>
	/// <param name="queue">The async I/O task queue to signal</param>
	/// <remarks>
	/// <para>
	/// This will unblock any threads that are sleeping in a call to <see href="https://wiki.libsdl.org/SDL3/SDL_WaitAsyncIOResult">SDL_WaitAsyncIOResult</see> for the specified queue, and cause them to return from that function.
	/// </para>
	/// <para>
	/// This can be useful when destroying a queue to make sure nothing is touching it indefinitely. In this case, once this call completes, the caller should take measures to make sure any previously-blocked threads have returned from their wait and will not touch the queue again
	/// (perhaps by setting a flag to tell the threads to terminate and then using <see href="https://wiki.libsdl.org/SDL3/SDL_WaitThread">SDL_WaitThread</see>() to make sure they've done so).
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_SignalAsyncIOQueue">SDL_SignalAsyncIOQueue</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial void SDL_SignalAsyncIOQueue(SDL_AsyncIOQueue* queue);

	/// <summary>
	/// Block until an async I/O task queue has a completed task
	/// </summary>
	/// <param name="queue">The async I/O task queue to wait on</param>
	/// <param name="outcome">Details of a finished task will be written here. May not be NULL.</param>
	/// <param name="timeoutMS">The maximum time to wait, in milliseconds, or -1 to wait indefinitely</param>
	/// <returns>Returns true if task has completed, false otherwise</returns>
	/// <remarks>
	/// <para>
	/// This function puts the calling thread to sleep until there a task assigned to the queue that has finished.
	/// </para>
	/// <para>
	/// If a task assigned to the queue has finished, this will return true and fill in <c><paramref name="outcome"/></c> with the details of the task.
	/// If no task in the queue has finished, this function will return false.
	/// </para>
	/// <para>
	/// If a task has completed, this function will free its resources and the task pointer will no longer be valid. The task will be removed from the queue.
	/// </para>
	/// <para>
	/// It is safe for multiple threads to call this function on the same queue at once; a completed task will only go to one of the threads.
	/// </para>
	/// <para>
	/// Note that by the nature of various platforms, more than one waiting thread may wake to handle a single task, but only one will obtain it, so <c><paramref name="timeoutMS"/></c> is a <c>maximum</c> wait time, and this function may return false sooner.
	/// </para>
	/// <para>
	/// This function may return false if there was a system error, the OS inadvertently awoke multiple threads,
	/// or if <see href="https://wiki.libsdl.org/SDL3/SDL_SignalAsyncIOQueue">SDL_SignalAsyncIOQueue</see>() was called to wake up all waiting threads without a finished task.
	/// </para>
	/// <para>
	/// A timeout can be used to specify a maximum wait time, but rather than polling, it is possible to have a timeout of -1 to wait forever,
	/// and use <see href="https://wiki.libsdl.org/SDL3/SDL_SignalAsyncIOQueue">SDL_SignalAsyncIOQueue</see>() to wake up the waiting threads later.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_WaitAsyncIOResult">SDL_WaitAsyncIOResult</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_WaitAsyncIOResult(SDL_AsyncIOQueue* queue, AsyncIOOutcome.SDL_AsyncIOOutcome* outcome, int timeoutMS);
}
