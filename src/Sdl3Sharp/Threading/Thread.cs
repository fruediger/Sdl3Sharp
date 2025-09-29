using System.Runtime.InteropServices;
using System;
using System.Runtime.InteropServices.Marshalling;

namespace Sdl3Sharp.Threading;

public sealed partial class Thread
{
	private unsafe SDL_Thread* mThreadPtr;

	public static ulong CurrentThreadId => SDL_GetCurrentThreadID();

	public static bool IsMainThread => SDL_IsMainThread();

	public ulong Id
	{
		get
		{
			unsafe
			{
				return SDL_GetThreadID(mThreadPtr);
			}
		}
	}

	public string? Name
	{
		get
		{
			unsafe
			{
				return Utf8StringMarshaller.ConvertToManaged(SDL_GetThreadName(mThreadPtr));
			}
		}
	}

	public ThreadState State
	{
		get
		{
			unsafe
			{
				return SDL_GetThreadState(mThreadPtr);
			}
		}
	}

	public static void DelayMilliseconds(uint milliseconds) => SDL_Delay(milliseconds);

	public static void DelayNanoseconds(ulong nanoseconds) => SDL_DelayNS(nanoseconds);

	public static void DelayNanosecondsPrecise(ulong nanoseconds) => SDL_DelayPrecise(nanoseconds);

	public static bool TryRunOnMainThread(Action action, bool waitForCompletion = false)
	{
		if (action is not null)
		{
			unsafe
			{
				var gcHandle = GCHandle.Alloc(action, GCHandleType.Normal);
				var result = false;
				try
				{
					result = SDL_RunOnMainThread(&MainThreadCallback, unchecked((void*)GCHandle.ToIntPtr(gcHandle)), waitForCompletion);
				}
				finally
				{
					if (!result)
					{
						gcHandle.Free();
					}
				}

				return result;
			}
		}

		return false;
	}

	public static bool TrySetCurrentThreadPriority(ThreadPriority priority) => SDL_SetCurrentThreadPriority(priority);
}
