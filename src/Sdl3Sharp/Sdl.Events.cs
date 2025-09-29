using Sdl3Sharp.Events;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using unsafe SDL_EventFilter = delegate* unmanaged[Cdecl]<void*, Sdl3Sharp.Events.Event*, Sdl3Sharp.Internal.Interop.CBool>;

namespace Sdl3Sharp;

partial class Sdl
{	
	private GCHandle mFilterHandle = default;
	private EventWatchWrapper? mWatchWrapper = null;

	private void DisposeEventQueue()
	{
		if (mWatchWrapper is not null)
		{
			mWatchWrapper.Watch = null;

			mWatchWrapper.Dispose();

			mWatchWrapper = null;
		}

		if (mFilterHandle.IsAllocated)
		{
			mFilterHandle.Free();

			mFilterHandle = default;
		}
	}

	public EventFilter? EventFilter
	{
		get
		{
			unsafe
			{
				SDL_EventFilter filter;
				void* userdata;

				if (!SDL_GetEventFilter(&filter, &userdata) || filter is null)
				{
					return null;
				}

				if (userdata is not null && GCHandle.FromIntPtr(unchecked((IntPtr)userdata)) is { IsAllocated: true, Target: EventFilter managedFilter })
				{
					return managedFilter;
				}

				var filterCopy = filter;
				var userdataCopy = userdata;					

				bool wrappedFilter(EventRef<Event> eventRef)
				{
					ref var @event = ref eventRef.Event;

					fixed (Event* fixedEvent = &@event)
					{
						return filterCopy(userdataCopy, fixedEvent);
					}
				}

				return wrappedFilter;
			}
		}

		set
		{
			unsafe
			{
				//if there was a previously set managed filter, we deref it here
				if (mFilterHandle is { IsAllocated: true, Target: Sdl3Sharp.EventFilter })
				{
					mFilterHandle.Free();

					mFilterHandle = default;
				}

				if (value is null)
				{
					SDL_SetEventFilter(null, null);
				}
				else
				{
					mFilterHandle = GCHandle.Alloc(value, GCHandleType.Normal);

					SDL_SetEventFilter(&EventFilterImpl, unchecked((void*)GCHandle.ToIntPtr(mFilterHandle)));
				}
			}
		}
	}

	public event EventWatch? EventWatch
	{
		add
		{
			if (mWatchWrapper is null)
			{
				if (value is null)
				{
					return;
				}

				mWatchWrapper = new();
			}

			mWatchWrapper.Watch += value;
		}

		remove
		{
			if (mWatchWrapper is not null)
			{
				mWatchWrapper.Watch -= value;

				if (mWatchWrapper.Watch is null)
				{
					mWatchWrapper.Dispose();

					mWatchWrapper = null;
				}
			}
		}
	}

#pragma warning disable IDE0079
#pragma warning disable CA1822 // this is intentionally an instance method
	public void FilterEvents(EventFilter filter)
#pragma warning restore CA1822
#pragma warning restore IDE0079
	{
		unsafe
		{
			if (filter is null)
			{
				failFilterArgumentNull();
			}

			var gcHandle = GCHandle.Alloc(filter, GCHandleType.Normal);

			try
			{
				SDL_FilterEvents(&EventFilterImpl, unchecked((void*)GCHandle.ToIntPtr(gcHandle)));
			}
			finally
			{
				gcHandle.Free();
			}
		}

		[DoesNotReturn]
		static void failFilterArgumentNull() => throw new ArgumentNullException(nameof(filter));
	}

#pragma warning disable IDE0079
#pragma warning disable CA1822 // this is intentionally an instance method
	public void FlushEvents(EventType type)
#pragma warning restore CA1822
#pragma warning restore IDE0079
		=> SDL_FlushEvent(type);

#pragma warning disable IDE0079
#pragma warning disable CA1822 // this is intentionally an instance method
	public void FlushEvents(EventType minType, EventType maxType)
#pragma warning restore CA1822
#pragma warning restore IDE0079
		=> SDL_FlushEvents(minType, maxType);

#pragma warning disable IDE0079
#pragma warning disable CA1822 // this is intentionally an instance method
	public bool HasEvents(EventType type)
#pragma warning restore CA1822
#pragma warning restore IDE0079
		=> SDL_HasEvent(type);

#pragma warning disable IDE0079
#pragma warning disable CA1822 // this is intentionally an instance method
	public bool HasEvents(EventType minType, EventType maxType)
#pragma warning restore CA1822
#pragma warning restore IDE0079
		=> SDL_HasEvents(minType, maxType);

#pragma warning disable IDE0079
#pragma warning disable CA1822 // this is intentionally an instance method
	public void PumpEvents()
#pragma warning restore CA1822
#pragma warning restore IDE0079
		=> SDL_PumpEvents();

	public bool TryAddEvents(ReadOnlySpan<Event> events, out int eventsStored)
		=> TryPeepEvents(events: MemoryMarshal.CreateSpan(ref MemoryMarshal.GetReference(events), events.Length), EventAction.Add, EventType.First, EventType.Last, out eventsStored);

	public bool TryGetEvents(Span<Event> events, EventType minType, EventType maxType, out int eventsStored)
		=> TryPeepEvents(events, EventAction.Get, minType, maxType, out eventsStored);

	public bool TryPeekEvents(EventType minType, EventType maxType, out int eventsStored)
		=> TryPeekEvents(events: default, minType, maxType, out eventsStored);

	public bool TryPeekEvents(Span<Event> events, EventType minType, EventType maxType, out int eventsStored)
		=> TryPeepEvents(events, EventAction.Peek, minType, maxType, out eventsStored);

	public bool TryPeepEvents(Span<Event> events, EventAction action, EventType minType, EventType maxType, out int eventsStored)
	{
		unsafe
		{
			fixed (Event* eventsPtr = events)
			{
				eventsStored = SDL_PeepEvents(eventsPtr, events.Length, action, minType, maxType);

				return eventsStored is >= 0;
			}
		}
	}

#pragma warning disable IDE0079
#pragma warning disable CA1822 // this is intentionally an instance method
	public bool TryPollEvent()
#pragma warning restore CA1822
#pragma warning restore IDE0079
	{
		unsafe
		{
			return SDL_PollEvent(null);
		}
	}

	public bool TryPollEvent(out Event @event)
	{
		unsafe
		{
			fixed (Event* eventPtr = &@event)
			{
				return SDL_PollEvent(eventPtr);
			}
		}
	}

	public bool TryPushEvent(in Event @event)
	{
		unsafe
		{
			fixed (Event* eventPtr = &@event)
			{
				return SDL_PushEvent(eventPtr);
			}
		}
	}

	public bool TryWaitForEvent(out Event @event)
	{
		unsafe
		{
			fixed (Event* eventPtr = &@event)
			{
				return SDL_WaitEvent(eventPtr);
			}
		}
	}

	public bool TryWaitForEvent(out Event @event, int timeoutMs)
	{
		unsafe
		{
			fixed (Event* eventPtr = &@event)
			{
				return SDL_WaitEventTimeout(eventPtr, timeoutMs);
			}
		}
	}

	public bool TryWaitForEvent(out Event @event, TimeSpan timeout)
		=> TryWaitForEvent(out @event, unchecked((int)Math.Truncate(timeout.TotalMilliseconds)));
}
