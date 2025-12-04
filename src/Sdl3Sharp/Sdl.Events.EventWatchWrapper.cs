using Sdl3Sharp.Events;
using Sdl3Sharp.Internal.Interop;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sdl3Sharp;

partial class Sdl
{
	private sealed class EventWatchWrapper : IDisposable
	{
		public EventWatch? Watch { get; set; }
		private GCHandle mHandle;

		public EventWatchWrapper()
		{
			unsafe
			{
				mHandle = GCHandle.Alloc(this, GCHandleType.Normal);

				if (!SDL_AddEventWatch(&EventFilter, unchecked((void*)GCHandle.ToIntPtr(mHandle))))
				{				
					mHandle.Free();

					failCouldNotAddEventWatch();
				}
			}

			[DoesNotReturn]
			static void failCouldNotAddEventWatch() => throw new SdlException("Could not add the event queue watcher");
		}

		~EventWatchWrapper() => DisposeImpl();

		public void Dispose()
		{
			DisposeImpl();
			GC.SuppressFinalize(this);
		}

		private unsafe void DisposeImpl()
		{
			if (mHandle.IsAllocated)
			{
				SDL_RemoveEventWatch(&EventFilter, unchecked((void*)GCHandle.ToIntPtr(mHandle)));

				mHandle.Free();
			}

			Watch = null;
		}

		[UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
		private unsafe static CBool EventFilter(void* userdata, Event* @event)
		{
			if (userdata is not null && GCHandle.FromIntPtr(unchecked((IntPtr)userdata)) is { IsAllocated: true, Target: EventWatchWrapper { Watch: var watch } })
			{
				watch?.Invoke(ref Unsafe.AsRef<Event>(@event));
			}

			return true;
		}
	}
}
