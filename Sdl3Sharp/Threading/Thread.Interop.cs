using Sdl3Sharp.Interop;
using Sdl3Sharp.SourceGeneration;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using unsafe FunctionPointer = delegate* unmanaged[Cdecl]<void>;
using unsafe MainThreadCallback = delegate* unmanaged[Cdecl]<void*, void>;
using unsafe ThreadFunction = delegate* unmanaged[Cdecl]<void*, int>;

namespace Sdl3Sharp.Threading;

partial class Thread
{
	// Used for opaque pointers
	[StructLayout(LayoutKind.Sequential, Size = 0)]
	internal readonly struct SDL_Thread;
	
	[UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
	private unsafe static void MainThreadCallback(void* userdata)
	{
		if (userdata is not null && GCHandle.FromIntPtr(unchecked((nint)userdata)) is { IsAllocated: true, Target: Action action } gcHandle)
		{
			try
			{
				action();
			}
			finally
			{
				gcHandle.Free();
			}
		}
	}

	/// <summary>
	/// Create a new thread with a default stack size
	/// </summary>
	/// <param name="fn">the <see href="https://wiki.libsdl.org/SDL3/SDL_ThreadFunction">SDL_ThreadFunction</see> function to call in the new thread</param>
	/// <param name="name">the name of the thread</param>
	/// <param name="data">a pointer that is passed to <c>fn</c></param>
	/// <param name="pfnBeginThread">set to NULL</param>
	/// <param name="pfnEndThread">set to NULL</param>
	/// <returns>Returns an opaque pointer to the new thread object on success, NULL if the new thread could not be created; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// This is a convenience function, equivalent to calling <see href="https://wiki.libsdl.org/SDL3/SDL_CreateThreadWithProperties">SDL_CreateThreadWithProperties</see> with the following properties set:
	/// <list type="bullet">
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_THREAD_CREATE_ENTRY_FUNCTION_POINTER"><c>DL_PROP_THREAD_CREATE_ENTRY_FUNCTION_POINTER</c></see></term>
	///			<description><c>fn</c></description>
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_THREAD_CREATE_NAME_STRING"><c>SDL_PROP_THREAD_CREATE_NAME_STRING</c></see></term>
	///			<description><c>name</c></description>
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_THREAD_CREATE_USERDATA_POINTER"><c>SDL_PROP_THREAD_CREATE_USERDATA_POINTER</c></see></term>
	///			<description><c>data</c></description>
	///		</item>
	/// </list>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_CreateThread">SDL_CreateThread</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial SDL_Thread* SDL_CreateThreadRuntime(ThreadFunction fn, byte* name, void* data, FunctionPointer pfnBeginThread, FunctionPointer pfnEndThread);

	/// <summary>
	/// Create a new thread with with the specified properties
	/// </summary>
	/// <param name="props">the properties to use</param>
	/// <param name="pfnBeginThread">set to NULL</param>
	/// <param name="pfnEndThread">set to NULL</param>
	/// <returns>Returns an opaque pointer to the new thread object on success, NULL if the new thread could not be created; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// These are the supported properties:
	/// <list type="bullet">
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_THREAD_CREATE_ENTRY_FUNCTION_POINTER"><c>SDL_PROP_THREAD_CREATE_ENTRY_FUNCTION_POINTER</c></see></term>
	///			<description>an <see href="https://wiki.libsdl.org/SDL3/SDL_ThreadFunction">SDL_ThreadFunction</see> value that will be called at the start of the new thread's life. Required.</description>
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_THREAD_CREATE_NAME_STRING"><c>SDL_PROP_THREAD_CREATE_NAME_STRING</c></see></term>
	///			<description>the name of the new thread, which might be available to debuggers. Optional, defaults to NULL.</description>
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_THREAD_CREATE_USERDATA_POINTER"><c>SDL_PROP_THREAD_CREATE_USERDATA_POINTER</c></see></term>
	///			<description>an arbitrary app-defined pointer, which is passed to the entry function on the new thread, as its only parameter. Optional, defaults to NULL.</description>
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_THREAD_CREATE_STACKSIZE_NUMBER"><c>SDL_PROP_THREAD_CREATE_STACKSIZE_NUMBER</c></see></term>
	///			<description>the size, in bytes, of the new thread's stack. Optional, defaults to 0 (system-defined default).</description>
	///		</item>
	/// </list>
	/// 
	/// SDL makes an attempt to report <see href="https://wiki.libsdl.org/SDL3/SDL_PROP_THREAD_CREATE_NAME_STRING"><c>SDL_PROP_THREAD_CREATE_NAME_STRING</c></see> to the system, so that debuggers can display it. Not all platforms support this.
	///
	/// Thread naming is a little complicated: Most systems have very small limits for the string length (Haiku has 32 bytes, Linux currently has 16, Visual C++ 6.0 has <em>nine</em>!), and possibly other arbitrary rules. You'll have to see what happens with your system's debugger.
	/// The name should be UTF-8 (but using the naming limits of C identifiers is a better bet). There are no requirements for thread naming conventions, so long as the string is null-terminated UTF-8, but these guidelines are helpful in choosing a name:
	///
	/// <see href="https://stackoverflow.com/questions/149932/naming-conventions-for-threads"/>
	///
	/// If a system imposes requirements, SDL will try to munge the string for it (truncate, etc), but the original string contents will be available from <see href="https://wiki.libsdl.org/SDL3/SDL_GetThreadName">SDL_GetThreadName</see>().
	///
	/// The size (in bytes) of the new stack can be specified with <see href="https://wiki.libsdl.org/SDL3/SDL_PROP_THREAD_CREATE_STACKSIZE_NUMBER"><c>SDL_PROP_THREAD_CREATE_STACKSIZE_NUMBER</c></see>. Zero means "use the system default" which might be wildly different between platforms. x86 Linux generally defaults to eight megabytes, an embedded device might be a few kilobytes instead.
	/// You generally need to specify a stack that is a multiple of the system's page size (in many cases, this is 4 kilobytes, but check your system documentation).
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_CreateThreadWithProperties">SDL_CreateThreadWithProperties</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial SDL_Thread* SDL_CreateThreadWithPropertiesRuntime(uint props, FunctionPointer pfnBeginThread, FunctionPointer pfnEndThread);

	/// <summary>
	/// Wait a specified number of milliseconds before returning
	/// </summary>
	/// <param name="ms">the number of milliseconds to delay</param>
	/// <remarks>
	/// This function waits a specified number of milliseconds before returning. It waits at least the specified time, but possibly longer due to OS scheduling.
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_Delay">SDL_Delay</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal static partial void SDL_Delay(uint ms);

	/// <summary>
	/// Wait a specified number of nanoseconds before returning
	/// </summary>
	/// <param name="ns">the number of nanoseconds to delay</param>
	/// <remarks>
	/// This function waits a specified number of nanoseconds before returning. It waits at least the specified time, but possibly longer due to OS scheduling.
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_DelayNS">SDL_DelayNS</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal static partial void SDL_DelayNS(ulong ns);

	/// <summary>
	/// Wait a specified number of nanoseconds before returning
	/// </summary>
	/// <param name="ns">the number of nanoseconds to delay</param>
	/// <remarks>
	/// This function waits a specified number of nanoseconds before returning. It will attempt to wait as close to the requested time as possible, busy waiting if necessary, but could return later due to OS scheduling.
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_DelayPrecise">SDL_DelayPrecise</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal static partial void SDL_DelayPrecise(ulong ns);

	/// <summary>
	/// Let a thread clean up on exit without intervention
	/// </summary>
	/// <param name="thread">the <see href="https://wiki.libsdl.org/SDL3/SDL_Thread">SDL_Thread</see> pointer that was returned from the <see href="https://wiki.libsdl.org/SDL3/SDL_CreateThread">SDL_CreateThread</see>() call that started this thread</param>
	/// <remarks>
	/// A thread may be "detached" to signify that it should not remain until another thread has called <see href="https://wiki.libsdl.org/SDL3/SDL_WaitThread">SDL_WaitThread</see>() on it. Detaching a thread is useful for long-running threads that nothing needs to synchronize with or further manage. When a detached thread is done, it simply goes away.
	///
	/// There is no way to recover the return code of a detached thread. If you need this, don't detach the thread and instead use <see href="https://wiki.libsdl.org/SDL3/SDL_WaitThread">SDL_WaitThread</see>().
	///
	/// Once a thread is detached, you should usually assume the <see href="https://wiki.libsdl.org/SDL3/SDL_Thread">SDL_Thread</see> isn't safe to reference again, as it will become invalid immediately upon the detached thread's exit, instead of remaining until someone has called <see href="https://wiki.libsdl.org/SDL3/SDL_WaitThread">SDL_WaitThread</see>() to finally clean it up. As such, don't detach the same thread more than once.
	///
	/// If a thread has already exited when passed to <see href="https://wiki.libsdl.org/SDL3/SDL_DetachThread">SDL_DetachThread</see>(), it will stop waiting for a call to <see href="https://wiki.libsdl.org/SDL3/SDL_WaitThread">SDL_WaitThread</see>() and clean up immediately. It is not safe to detach a thread that might be used with <see href="https://wiki.libsdl.org/SDL3/SDL_WaitThread">SDL_WaitThread</see>().
	///
	/// You may not call <see href="https://wiki.libsdl.org/SDL3/SDL_WaitThread">SDL_WaitThread</see>() on a thread that has been detached. Use either that function or this one, but not both, or behavior is undefined.
	///
	/// It is safe to pass NULL to this function; it is a no-op.
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_DetachThread">SDL_DetachThread</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial void SDL_DetachThread(SDL_Thread* thread);

	/// <summary>
	/// Get the thread identifier for the current thread
	/// </summary>
	/// <returns>Returns the ID of the current thread</returns>
	/// <remarks>
	/// This thread identifier is as reported by the underlying operating system. If SDL is running on a platform that does not support threads the return value will always be zero.
	///
	/// This function also returns a valid thread ID when called from the main thread.
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetCurrentThreadID">SDL_GetCurrentThreadID</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial ulong SDL_GetCurrentThreadID();

	/// <summary>
	/// Get the thread identifier for the specified thread
	/// </summary>
	/// <param name="thread">the thread to query</param>
	/// <returns>Returns the ID of the specified thread, or the ID of the current thread if thread is NULL</returns>
	/// <remarks>
	/// This thread identifier is as reported by the underlying operating system. If SDL is running on a platform that does not support threads the return value will always be zero.
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetThreadID">SDL_GetThreadID</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial ulong SDL_GetThreadID(SDL_Thread* thread);

	/// <summary>
	/// Get the thread name as it was specified in <see href="https://wiki.libsdl.org/SDL3/SDL_CreateThread">SDL_CreateThread</see>()
	/// </summary>
	/// <param name="thread">the thread to query</param>
	/// <returns>Returns a pointer to a UTF-8 string that names the specified thread, or NULL if it doesn't have a name</returns>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetThreadName">SDL_GetThreadName</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial byte* SDL_GetThreadName(SDL_Thread* thread);

	/// <summary>
	/// Get the current state of a thread
	/// </summary>
	/// <param name="thread">the thread to query</param>
	/// <returns>Returns the current state of a thread, or <see href="https://wiki.libsdl.org/SDL3/SDL_THREAD_UNKNOWN">SDL_THREAD_UNKNOWN</see> if the thread isn't valid</returns>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetThreadState">SDL_GetThreadState</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial ThreadState SDL_GetThreadState(SDL_Thread* thread);

	/// <summary>
	/// Return whether this is the main thread
	/// </summary>
	/// <returns>Returns true if this thread is the main thread, or false otherwise</returns>
	/// <remarks>
	/// On Apple platforms, the main thread is the thread that runs your program's main() entry point. On other platforms, the main thread is the one that calls <see href="https://wiki.libsdl.org/SDL3/SDL_Init">SDL_Init</see>(<see href="https://wiki.libsdl.org/SDL3/SDL_INIT_VIDEO">SDL_INIT_VIDEO</see>), which should usually be the one that runs your program's main() entry point. If you are using the main callbacks, <see href="https://wiki.libsdl.org/SDL3/SDL_AppInit">SDL_AppInit</see>(), <see href="https://wiki.libsdl.org/SDL3/SDL_AppIterate">SDL_AppIterate</see>(), and <see href="https://wiki.libsdl.org/SDL3/SDL_AppQuit">SDL_AppQuit</see>() are all called on the main thread.
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_IsMainThread">SDL_IsMainThread</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal static partial CBool SDL_IsMainThread();

	/// <summary>
	/// Call a function on the main thread during event processing
	/// </summary>
	/// <param name="callback">the callback to call on the main thread</param>
	/// <param name="userdata">a pointer that is passed to callback</param>
	/// <param name="wait_complete">true to wait for the callback to complete, false to return immediately</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// If this is called on the main thread, the callback is executed immediately. If this is called on another thread, this callback is queued for execution on the main thread during event processing.
	/// 
	/// Be careful of deadlocks when using this functionality. You should not have the main thread wait for the current thread while this function is being called with <c>wait_complete</c> true.
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_RunOnMainThread">SDL_RunOnMainThread</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_RunOnMainThread(MainThreadCallback callback, void* userdata, CBool wait_complete);

	/// <summary>
	/// Set the priority for the current thread
	/// </summary>
	/// <param name="priority">the <see href="https://wiki.libsdl.org/SDL3/SDL_ThreadPriority">SDL_ThreadPriority</see> to set</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// Note that some platforms will not let you alter the priority (or at least, promote the thread to a higher priority) at all, and some require you to be an administrator account. Be prepared for this to fail.
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_SetCurrentThreadPriority">SDL_SetCurrentThreadPriority</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal static partial CBool SDL_SetCurrentThreadPriority(ThreadPriority priority);

	/// <summary>
	/// Wait for a thread to finish
	/// </summary>
	/// <param name="thread">the <see href="https://wiki.libsdl.org/SDL3/SDL_Thread">SDL_Thread</see> pointer that was returned from the <see href="https://wiki.libsdl.org/SDL3/SDL_CreateThread">SDL_CreateThread</see>() call that started this thread</param>
	/// <param name="status">a pointer filled in with the value returned from the thread function by its 'return', or -1 if the thread has been detached or isn't valid, may be NULL</param>
	/// <remarks>
	/// Threads that haven't been detached will remain until this function cleans them up. Not doing so is a resource leak.
	///
	/// Once a thread has been cleaned up through this function, the <see href="https://wiki.libsdl.org/SDL3/SDL_Thread">SDL_Thread</see> that references it becomes invalid and should not be referenced again. As such, only one thread may call <see href="https://wiki.libsdl.org/SDL3/SDL_WaitThread">SDL_WaitThread</see>() on another.
	///
	/// The return code from the thread function is placed in the area pointed to by <c>status</c>, if <c>status</c> is not NULL.
	///
	/// You may not wait on a thread that has been used in a call to <see href="https://wiki.libsdl.org/SDL3/SDL_DetachThread">SDL_DetachThread</see>(). Use either that function or this one, but not both, or behavior is undefined.
	///
	/// It is safe to pass a NULL thread to this function; it is a no-op.
	///
	/// Note that the thread pointer is freed by this function and is not valid afterward.
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_WaitThread">SDL_WaitThread</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial void SDL_WaitThread(SDL_Thread* thread, int* status);
}
