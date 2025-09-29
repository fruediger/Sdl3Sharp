using Sdl3Sharp.Events;
using Sdl3Sharp.Internal.Interop;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sdl3Sharp.Internal.Events;

internal abstract class EventWatchHandler : IDisposable
{
	[UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
	private static unsafe CBool EventWatch(void* userdata, Event* @event)
	{
		if (userdata is not null && GCHandle.FromIntPtr(unchecked((IntPtr)userdata)) is { IsAllocated: true, Target: EventWatchHandler handler })
		{
			if (!handler.TryInvoke(new(ref Unsafe.AsRef<Event>(@event))))
			{
				handler.Dispose();
			}
		}

		return true;
	}

	private GCHandle mHandle = default;

	public unsafe EventWatchHandler()
	{
		mHandle = GCHandle.Alloc(this, GCHandleType.Normal);

		Sdl.SDL_AddEventWatch(&EventWatch, unchecked((void*)GCHandle.ToIntPtr(mHandle)));
	}

	~EventWatchHandler() => Dispose(disposing: false);

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	protected unsafe virtual void Dispose(bool disposing)
	{
		if (mHandle != default)
		{			
			Sdl.SDL_RemoveEventWatch(&EventWatch, unchecked((void*)GCHandle.ToIntPtr(mHandle)));

			mHandle.Free();
			mHandle = default;
		}
	}

	private protected abstract bool TryInvoke(EventRef<Event> eventRef);
}
