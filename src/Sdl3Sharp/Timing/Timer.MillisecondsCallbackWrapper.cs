using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sdl3Sharp.Timing;

partial class Timer
{
	private sealed class MillisecondsCallbackWrapper(Timer timer, TimerMillisecondsCallback callback) : CallbackWrapper(timer)
	{
		private TimerMillisecondsCallback? mCallback = callback;

		[UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
		public unsafe static uint TimerCallback(void* userdata, uint _, uint interval)
		{
			//TODO
			if (userdata is not null && GCHandle.FromIntPtr(unchecked((IntPtr)userdata)) is { IsAllocated: true, Target: MillisecondsCallbackWrapper { Timer: Timer timer, mCallback: TimerMillisecondsCallback callback } })
			{
				return callback(timer, interval);
			}

			return 0;
		}

		public override void Dispose() { mCallback = null; base.Dispose(); }
	}
}
