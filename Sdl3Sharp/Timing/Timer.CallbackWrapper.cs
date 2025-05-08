using System;

namespace Sdl3Sharp.Timing;

partial class Timer
{
	private abstract class CallbackWrapper(Timer timer) : IDisposable
	{
		protected Timer? Timer = timer;

		public virtual void Dispose() { Timer = null; }
	}
}
