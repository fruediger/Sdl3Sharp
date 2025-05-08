using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sdl3Sharp.Timing;

partial class Timer
{
	private sealed class NanosecondsCallbackWrapper(Timer timer, TimerNanosecondsCallback callback) : CallbackWrapper(timer)
	{
		private TimerNanosecondsCallback? mCallback = callback;

		[UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
		public unsafe static ulong NSTimerCallback(void* userdata, uint _, ulong interval)
		{
			//TODO
			if (userdata is not null && GCHandle.FromIntPtr(unchecked((IntPtr)userdata)) is { IsAllocated: true, Target: NanosecondsCallbackWrapper { Timer: Timer timer, mCallback: TimerNanosecondsCallback callback } })
			{
				return callback(timer, interval);
			}

			return 0;
		}

		public override void Dispose() { mCallback = null; base.Dispose(); }
	}
}
