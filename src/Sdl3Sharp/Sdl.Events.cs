using Sdl3Sharp.Events;
using System;
using System.Diagnostics.CodeAnalysis;
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

	/// <summary>
	/// Gets or sets the global event filter
	/// </summary>
	/// <value>
	/// The global event filter
	/// </value>
	/// <remarks>
	/// <para>
	/// The global event filter is used for filtering all events before they get pushed onto SDL's event queue.
	/// </para>
	/// <para>
	/// Returning <c><see langword="false"/></c> from the filter will prevent the event from being pushed onto the event queue.
	/// </para>
	/// <para>
	/// The filter is called for every event that gets pushed onto the event queue using <see cref="TryPushEvent(in Event)"/>,
	/// but events added to the queue using <see cref="TryAddEvents(ReadOnlySpan{Event}, out int)"/> bypass the filter.
	/// </para>
	/// <para>
	/// <see cref="EventTypeExtensions.set_Enabled(EventType, bool)">Disabled</see> events will never be passed to the filter nor will they be added to the event queue.
	/// </para>
	/// <para>
	/// You can use this property to chain multiple filters together by getting the current filter as a delegate, and then setting a new filter delegate that calls the previous filter as part of its implementation.
	/// </para>
	/// <para>
	/// <em>WARNING</em>: Be very careful of what you do in the event filter, as it may run in a different thread!
	/// The exception to that is the handling of <see cref="EventType.WindowExposed"/>, which is guaranteed to be sent from the OS on the main thread and you are expected to redraw your window in response to this event.
	/// </para>
	/// </remarks>
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

				bool wrappedFilter(ref Event @event)
				{
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

	/// <summary>
	/// Occurs when an event is pushed onto the event queue
	/// </summary>
	/// <remarks>
	/// <para>
	/// The event handler to this event is called for every event that gets pushed onto SDL's event queue using <see cref="TryPushEvent(in Event)"/>,
	/// but not for <see cref="EventTypeExtensions.set_Enabled(EventType, bool)">disabled</see> events,
	/// nor for events that were filtered out by the <see cref="EventFilter"/>,
	/// nor for events added to the queue using <see cref="TryAddEvents(ReadOnlySpan{Event}, out int)"/>.
	/// </para>
	/// <para>
	/// <em>WARNING</em>: Be very careful of what you do in the event handler to this event, as it may run in a different thread!
	/// </para>
	/// </remarks>
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

	/// <summary>
	/// Filters events on the current event queue using the specified filter
	/// </summary>
	/// <param name="filter">The event filter to apply</param>
	/// <exception cref="ArgumentNullException"><paramref name="filter"/> is <c><see langword="null"/></c></exception>
	/// <remarks>
	/// <para>
	/// Unlike the <see cref="EventFilter"/> property, this method only applies the filter to events that are already in the event queue <em>once</em> and potentially removes some of them.
	/// It doesn't apply permanently.
	/// </para>
	/// </remarks>
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

	/// <summary>
	/// Clears <em>all</em> events from the current event queue
	/// </summary>
	/// <remarks>
	/// <para>
	/// This will unconditionally remove any events from the event queue.
	/// If you need to remove specific events, use <see cref="FlushEvents(EventType)"/> or <see cref="FlushEvents(EventType, EventType)"/> instead.
	/// </para>
	/// <para>
	/// It's also normal to just ignore events you don't care about in your event loop without calling this method.
	/// </para>
	/// <para>
	/// This method only affects currently queued events.
	/// If you want to make sure that all pending OS events are flushed, you can call <see cref="PumpEvents"/> on the main thread immediately before calling this method.
	/// </para>
	/// <para>
	/// If you have user defined events with custom data that needs to be handled (e.g. disposed or freed),
	/// you should use <see cref="TryGetEvents(Span{Event}, EventType, EventType, out int)"/> or <see cref="TryPeekEvents(Span{Event}, EventType, EventType, out int)"/>
	/// to retrieve and handle those events before flushing.
	/// </para>
	/// </remarks>
#pragma warning disable IDE0079
#pragma warning disable CA1822 // this is intentionally an instance method
	public void FlushEvents()
#pragma warning restore CA1822
#pragma warning restore IDE0079
#pragma warning disable CS0618 // we allow that here
		=> SDL_FlushEvents(EventType.First, EventType.Last);
#pragma warning restore CS0618

	/// <summary>
	/// Clears events of the specified type from the current event queue
	/// </summary>
	/// <param name="type">The type of events to clear</param>
	/// <remarks>
	/// <para>
	/// This will unconditionally remove any events from the event queue that match the specified <paramref name="type"/>.
	/// If you need to remove a specific range of event types, use <see cref="FlushEvents(EventType, EventType)"/> instead.
	/// </para>
	/// <para>
	/// It's also normal to just ignore events you don't care about in your event loop without calling this method.
	/// </para>
	/// <para>
	/// This method only affects currently queued events.
	/// If you want to make sure that all pending OS events are flushed, you can call <see cref="PumpEvents"/> on the main thread immediately before calling this method.
	/// </para>
	/// <para>
	/// If you have user defined events with custom data that needs to be handled (e.g. disposed or freed),
	/// you should use <see cref="TryGetEvents(Span{Event}, EventType, EventType, out int)"/> or <see cref="TryPeekEvents(Span{Event}, EventType, EventType, out int)"/>
	/// to retrieve and handle those events before flushing.
	/// </para>
	/// </remarks>
#pragma warning disable IDE0079
#pragma warning disable CA1822 // this is intentionally an instance method
	public void FlushEvents(EventType type)
#pragma warning restore CA1822
#pragma warning restore IDE0079
		=> SDL_FlushEvent(type);

	/// <summary>
	/// Clears events in the specified type range from the current event queue
	/// </summary>
	/// <param name="minType">The inclusive lower bound of the event type range to be cleared</param>
	/// <param name="maxType">The inclusive upper bound of the event type range to be cleared</param>
	/// <remarks>
	/// <para>
	/// This will unconditionally remove any events from the event queue in the range between <paramref name="minType"/> and <paramref name="maxType"/>, inclusive.
	/// If you need to remove a specific single event type only, use <see cref="FlushEvents(EventType)"/> instead.
	/// </para>
	/// <para>
	/// It's also normal to just ignore events you don't care about in your event loop without calling this method.
	/// </para>
	/// <para>
	/// This method only affects currently queued events.
	/// If you want to make sure that all pending OS events are flushed, you can call <see cref="PumpEvents"/> on the main thread immediately before calling this method.
	/// </para>
	/// <para>
	/// If you have user defined events with custom data that needs to be handled (e.g. disposed or freed),
	/// you should use <see cref="TryGetEvents(Span{Event}, EventType, EventType, out int)"/> or <see cref="TryPeekEvents(Span{Event}, EventType, EventType, out int)"/>
	/// to retrieve and handle those events before flushing.
	/// </para>
	/// </remarks>
#pragma warning disable IDE0079
#pragma warning disable CA1822 // this is intentionally an instance method
	public void FlushEvents(EventType minType, EventType maxType)
#pragma warning restore CA1822
#pragma warning restore IDE0079
		=> SDL_FlushEvents(minType, maxType);

	/// <summary>
	/// Determines whether there are any events in the current event queue
	/// </summary>
	/// <returns><c><see langword="true"/></c>, if there are events in the queue; otherwise, <c><see langword="false"/></c></returns>
	/// <remarks>
	/// <para>
	/// If you need to check for specific event types, use <see cref="HasEvents(EventType)"/> or <see cref="HasEvents(EventType, EventType)"/> instead.
	/// </para>
	/// </remarks>
#pragma warning disable IDE0079
#pragma warning disable CA1822 // this is intentionally an instance method
	public bool HasEvents()
#pragma warning restore CA1822
#pragma warning restore IDE0079
#pragma warning disable CS0618 // we allow that here
		=> SDL_HasEvents(EventType.First, EventType.Last);
#pragma warning restore CS0618

	/// <summary>
	/// Determines whether there are any events of the specified type in the current event queue
	/// </summary>
	/// <param name="type">The type of the event to check for</param>
	/// <returns><c><see langword="true"/></c>, if there are events of the specified type in the queue; otherwise, <c><see langword="false"/></c></returns>
	/// <remarks>
	/// <para>
	/// If you need to check for a specific range of event types, use <see cref="HasEvents(EventType, EventType)"/> instead.
	/// </para>
	/// </remarks>
#pragma warning disable IDE0079
#pragma warning disable CA1822 // this is intentionally an instance method
	public bool HasEvents(EventType type)
#pragma warning restore CA1822
#pragma warning restore IDE0079
		=> SDL_HasEvent(type);

	/// <summary>
	/// Determines whether there are any events in the specified type range in the current event queue
	/// </summary>
	/// <param name="minType">The inclusive lower bound of the event type range to check</param>
	/// <param name="maxType">The inclusive upper bound of the event type range to check</param>
	/// <returns><c><see langword="true"/></c>, if there are events in the specified type range; otherwise, <c><see langword="false"/></c></returns>
	/// <remarks>
	/// <para>
	/// If you need to check for a specific single event type only, use <see cref="HasEvents(EventType)"/> instead.
	/// </para>
	/// </remarks>
#pragma warning disable IDE0079
#pragma warning disable CA1822 // this is intentionally an instance method
	public bool HasEvents(EventType minType, EventType maxType)
#pragma warning restore CA1822
#pragma warning restore IDE0079
		=> SDL_HasEvents(minType, maxType);

	/// <summary>
	/// Pumps the event loop, gathering events from the input devices
	/// </summary>
	/// <remarks>
	/// <para>
	/// This method updates the event queue and internal input device state.
	/// </para>
	/// <para>
	/// This method gathers all the pending input information from devices and places it in the event queue.
	/// Without calls to <see cref="PumpEvents"/> no events would ever be placed on the queue.
	/// Often the need for calls to <see cref="PumpEvents"/> is hidden from the user since methods like <see cref="TryPollEvent()"/> and <see cref="TryWaitForEvent(out Event)"/> implicitly call <see cref="PumpEvents"/>.
	/// However, if you are not polling or waiting for events (e.g. you are filtering them), then you must call <see cref="PumpEvents"/> to force an event queue update.
	/// </para>
	/// <para>
	/// You can only call this method in the thread that set the video mode (most probably that's the main thread).
	/// </para>
	/// </remarks>
#pragma warning disable IDE0079
#pragma warning disable CA1822 // this is intentionally an instance method
	public void PumpEvents()
#pragma warning restore CA1822
#pragma warning restore IDE0079
		=> SDL_PumpEvents();

	/// <summary>
	/// Tries to add new events to the current event queue
	/// </summary>
	/// <param name="events">The events to be added</param>
	/// <param name="added">The number of events that were added to the event queue</param>
	/// <returns><c><see langword="true"/></c>, if the operation was successful; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// Calling this method is effectively calling <c><see cref="TryPeepEvents(Span{Event}, EventAction, out int)">TryPeepEvents</see>(<paramref name="events"/>, <see cref="EventAction"/>.<see cref="EventAction.Add">Add</see>, <see langword="out"/> <paramref name="added"/>)</c>.
	/// </para>
	/// </remarks>
	public bool TryAddEvents(ReadOnlySpan<Event> events, out int added)
		=> TryPeepEvents(events: MemoryMarshal.CreateSpan(ref MemoryMarshal.GetReference(events), events.Length), EventAction.Add, stored: out added);

	/// <summary>
	/// Tries to count any events in the current event queue
	/// </summary>
	/// <param name="counted">The number of events in the current event queue</param>
	/// <returns><c><see langword="true"/></c>, if the operation was successful; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// Calling this method is effectively calling <c><see cref="TryPeepEvents(int, EventAction, EventType, EventType, out int)">TryPeepEvents</see>(<see cref="int"/>.<see cref="int.MaxValue">MaxValue</see>, <see cref="EventAction"/>.<see cref="EventAction.Peek">Peek</see>, <see langword="out"/> <paramref name="counted"/>)</c>.
	/// </para>
	/// </remarks>
	public bool TryCountEvents(out int counted)
		=> TryPeepEvents(count: int.MaxValue, EventAction.Peek, out counted);

	/// <summary>
	/// Tries to count events in the specified type range in the current event queue
	/// </summary>
	/// <param name="type">The type of the event to count</param>
	/// <param name="counted">The number of events that match the specified <paramref name="type"/> in the current event queue</param>
	/// <returns><c><see langword="true"/></c>, if the operation was successful; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// Calling this method is effectively calling <c><see cref="TryPeepEvents(int, EventAction, EventType, EventType, out int)">TryPeepEvents</see>(<see cref="int"/>.<see cref="int.MaxValue">MaxValue</see>, <see cref="EventAction"/>.<see cref="EventAction.Peek">Peek</see>, <paramref name="type"/>, <see langword="out"/> <paramref name="counted"/>)</c>.
	/// </para>
	/// </remarks>
	public bool TryCountEvents(EventType type, out int counted)
		=> TryPeepEvents(count: int.MaxValue, EventAction.Peek, type, out counted);

	/// <summary>
	/// Tries to count events in the specified type range in the current event queue
	/// </summary>
	/// <param name="minType">The inclusive lower bound of the event type range to count</param>
	/// <param name="maxType">The inclusive upper bound of the event type range to count</param>
	/// <param name="counted">The number of events that match the specified <paramref name="minType"/> and <paramref name="maxType"/> range in the current event queue</param>
	/// <returns><c><see langword="true"/></c>, if the operation was successful; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// Calling this method is effectively calling <c><see cref="TryPeepEvents(int, EventAction, EventType, EventType, out int)">TryPeepEvents</see>(<see cref="int"/>.<see cref="int.MaxValue">MaxValue</see>, <see cref="EventAction"/>.<see cref="EventAction.Peek">Peek</see>, <paramref name="minType"/>, <paramref name="maxType"/>, <see langword="out"/> <paramref name="counted"/>)</c>.
	/// </para>
	/// </remarks>
	public bool TryCountEvents(EventType minType, EventType maxType, out int counted)
		=> TryPeepEvents(count: int.MaxValue, EventAction.Peek, minType, maxType, out counted);

	/// <summary>
	/// Tries to retrieve any events from the current event queue and removes them
	/// </summary>
	/// <param name="events">The destination span to store the retrieved events</param>
	/// <param name="stored">The number of events that were retrieved and removed from the current event queue</param>
	/// <returns><c><see langword="true"/></c>, if the operation was successful; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// Calling this method is effectively calling <c><see cref="TryPeepEvents(Span{Event}, EventAction, EventType, EventType, out int)">TryPeepEvents</see>(<paramref name="events"/>, <see cref="EventAction"/>.<see cref="EventAction.Get">Get</see>, <see langword="out"/> <paramref name="stored"/>)</c>.
	/// </para>
	/// </remarks>
	public bool TryGetEvents(Span<Event> events, out int stored)
		=> TryPeepEvents(events, EventAction.Get, out stored);

	/// <summary>
	/// Tries to retrieve events of a specified type from the current event queue and removes them
	/// </summary>
	/// <param name="events">The destination span to store the retrieved events</param>
	/// <param name="type">The type of events to retrieve</param>
	/// <param name="stored">The number of events that were retrieved and removed from the current event queue</param>
	/// <returns><c><see langword="true"/></c>, if the operation was successful; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// Calling this method is effectively calling <c><see cref="TryPeepEvents(Span{Event}, EventAction, EventType, EventType, out int)">TryPeepEvents</see>(<paramref name="events"/>, <see cref="EventAction"/>.<see cref="EventAction.Get">Get</see>, <paramref name="type"/>, <see langword="out"/> <paramref name="stored"/>)</c>.
	/// </para>
	/// </remarks>
	public bool TryGetEvents(Span<Event> events, EventType type, out int stored)
		=> TryPeepEvents(events, EventAction.Get, type, out stored);

	/// <summary>
	/// Tries to retrieve events in the specified type range from the current event queue and removes them
	/// </summary>
	/// <param name="events">The destination span to store the retrieved events</param>
	/// <param name="minType">The inclusive lower bound of the event type range to retrieve</param>
	/// <param name="maxType">The inclusive upper bound of the event type range to retrieve</param>
	/// <param name="stored">The number of events that were retrieved and removed from the current event queue</param>
	/// <returns><c><see langword="true"/></c>, if the operation was successful; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// Calling this method is effectively calling <c><see cref="TryPeepEvents(Span{Event}, EventAction, EventType, EventType, out int)">TryPeepEvents</see>(<paramref name="events"/>, <see cref="EventAction"/>.<see cref="EventAction.Get">Get</see>, <paramref name="minType"/>, <paramref name="maxType"/>, <see langword="out"/> <paramref name="stored"/>)</c>.
	/// </para>
	/// </remarks>
	public bool TryGetEvents(Span<Event> events, EventType minType, EventType maxType, out int stored)
		=> TryPeepEvents(events, EventAction.Get, minType, maxType, out stored);

	/// <summary>
	/// Tries to retrieve any events from the current event queue without removing them
	/// </summary>
	/// <param name="events">The destination span to store the retrieved events</param>
	/// <param name="stored">The number of events that were retrieved from the current event queue</param>
	/// <returns><c><see langword="true"/></c>, if the operation was successful; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// Calling this method is effectively calling <c><see cref="TryPeepEvents(Span{Event}, EventAction, out int)">TryPeepEvents</see>(<paramref name="events"/>, <see cref="EventAction"/>.<see cref="EventAction.Peek">Peek</see>, <see langword="out"/> <paramref name="stored"/>)</c>.
	/// </para>
	/// </remarks>
	public bool TryPeekEvents(Span<Event> events, out int stored)
		=> TryPeepEvents(events, EventAction.Peek, out stored);

	/// <summary>
	/// Tries to retrieve events of a specified type from the current event queue without removing them
	/// </summary>
	/// <param name="events">The destination span to store the retrieved events</param>
	/// <param name="type">The event type to retrieve</param>
	/// <param name="stored">The number of events that were retrieved from the current event queue</param>
	/// <returns><c><see langword="true"/></c>, if the operation was successful; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// Calling this method is effectively calling <c><see cref="TryPeepEvents(Span{Event}, EventAction, EventType, EventType, out int)">TryPeepEvents</see>(<paramref name="events"/>, <see cref="EventAction"/>.<see cref="EventAction.Peek">Peek</see>, <paramref name="type"/>, <see langword="out"/> <paramref name="stored"/>)</c>.
	/// </para>
	/// </remarks>
	public bool TryPeekEvents(Span<Event> events, EventType type, out int stored)
		=> TryPeepEvents(events, EventAction.Peek, type, out stored);

	/// <summary>
	/// Tries to retrieve events in the specified type range from the current event queue without removing them
	/// </summary>
	/// <param name="events">The destination span to store the retrieved events</param>
	/// <param name="minType">The inclusive lower bound of the event type range to retrieve</param>
	/// <param name="maxType">The inclusive upper bound of the event type range to retrieve</param>
	/// <param name="stored">The number of events that were retrieved from the current event queue</param>
	/// <returns><c><see langword="true"/></c>, if the operation was successful; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// Calling this method is effectively calling <c><see cref="TryPeepEvents(Span{Event}, EventAction, EventType, EventType, out int)">TryPeepEvents</see>(<paramref name="events"/>, <see cref="EventAction"/>.<see cref="EventAction.Peek">Peek</see>, <paramref name="minType"/>, <paramref name="maxType"/>, <see langword="out"/> <paramref name="stored"/>)</c>.
	/// </para>
	/// </remarks>
	public bool TryPeekEvents(Span<Event> events, EventType minType, EventType maxType, out int stored)
		=> TryPeepEvents(events, EventAction.Peek, minType, maxType, out stored);

	/// <summary>
	/// Tries to check the current event queue for any events and optionally removes them
	/// </summary>
	/// <param name="count">The maximum number of events affected</param>
	/// <param name="action">The action to perform on the events</param>
	/// <param name="counted">The number of events that were affected</param>
	/// <returns><c><see langword="true"/></c>, if the operation was successful; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// <list type="bullet">
	///		<item>
	///			<term><see cref="EventAction.Add"/></term>
	///			<description>Does nothing and will result in a failure (this method will return <c><see langword="false"/></c>)</description>
	///		</item>
	///		<item>
	///			<term><see cref="EventAction.Peek"/></term>
	///			<description>
	///				Up to <paramref name="count"/> events from the front of the event queue are counted <em>without removing them from the queue</em>.
	///				The <paramref name="counted"/> output parameter contains the number of events that were counted.
	///			</description>
	///		</item>
	///		<item>
	///			<term><see cref="EventAction.Get"/></term>
	///			<description>
	///				Up to <paramref name="count"/> events from the front of the event queue are counted <em>and removed from the queue</em>.
	///				The <paramref name="counted"/> output parameter contains the number of events that were counted and removed from the current event queue.
	///			</description>
	///		</item>
	///	</list>
	/// </para>
	/// <para>
	///	You may have to call <see cref="PumpEvents"/> before calling this method. Otherwise, events may not be ready to be filtered when you call this method.
	/// </para>
	/// </remarks>
	public bool TryPeepEvents(int count, EventAction action, out int counted)
#pragma warning disable CS0618 // we allow that here
		=> TryPeepEvents(count, action, minType: EventType.First, maxType: EventType.Last, out counted);
#pragma warning restore CS0618

	/// <summary>
	/// Tries to check the current event queue for events of a specified type and optionally removes them
	/// </summary>
	/// <param name="count">The maximum number of events affected</param>
	/// <param name="action">The action to perform on the events</param>
	/// <param name="type">The type of the event to check for</param>
	/// <param name="counted">The number of events that were affected</param>
	/// <returns><c><see langword="true"/></c>, if the operation was successful; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// <list type="bullet">
	///		<item>
	///			<term><see cref="EventAction.Add"/></term>
	///			<description>Does nothing and will result in a failure (this method will return <c><see langword="false"/></c>)</description>
	///		</item>
	///		<item>
	///			<term><see cref="EventAction.Peek"/></term>
	///			<description>
	///				Up to <paramref name="count"/> events from the front of the event queue that match the specified <paramref name="type"/> are counted <em>without removing them from the queue</em>.
	///				The <paramref name="counted"/> output parameter contains the number of events that were counted.
	///			</description>
	///		</item>
	///		<item>
	///			<term><see cref="EventAction.Get"/></term>
	///			<description>
	///				Up to <paramref name="count"/> events from the front of the event queue that match the specified <paramref name="type"/> are counted <em>and removed from the queue</em>.
	///				The <paramref name="counted"/> output parameter contains the number of events that were counted and removed from the current event queue.
	///			</description>
	///		</item>
	///	</list>
	/// </para>
	/// <para>
	///	You may have to call <see cref="PumpEvents"/> before calling this method. Otherwise, events may not be ready to be filtered when you call this method.
	/// </para>
	/// </remarks>
	public bool TryPeepEvents(int count, EventAction action, EventType type, out int counted)
		=> TryPeepEvents(count, action, minType: type, maxType: type, out counted);

	/// <summary>
	/// Tries to check the current event queue for events in specified type range and optionally removes them
	/// </summary>
	/// <param name="count">The maximum number of events affected</param>
	/// <param name="action">The action to perform on the events</param>
	/// <param name="minType">The inclusive lower bound of the event type range to check</param>
	/// <param name="maxType">The inclusive upper bound of the event type range to check</param>
	/// <param name="counted">The number of events that were affected</param>
	/// <returns><c><see langword="true"/></c>, if the operation was successful; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// <list type="bullet">
	///		<item>
	///			<term><see cref="EventAction.Add"/></term>
	///			<description>Does nothing and will result in a failure (this method will return <c><see langword="false"/></c>)</description>
	///		</item>
	///		<item>
	///			<term><see cref="EventAction.Peek"/></term>
	///			<description>
	///				Up to <paramref name="count"/> events from the front of the event queue that match the specified <paramref name="minType"/> and <paramref name="maxType"/> range are counted <em>without removing them from the queue</em>.
	///				The <paramref name="counted"/> output parameter contains the number of events that were counted.
	///			</description>
	///		</item>
	///		<item>
	///			<term><see cref="EventAction.Get"/></term>
	///			<description>
	///				Up to <paramref name="count"/> events from the front of the event queue that match the specified <paramref name="minType"/> and <paramref name="maxType"/> range are counted <em>and removed from the queue</em>.
	///				The <paramref name="counted"/> output parameter contains the number of events that were counted and removed from the current event queue.
	///			</description>
	///		</item>
	///	</list>
	/// </para>
	/// <para>
	///	You may have to call <see cref="PumpEvents"/> before calling this method. Otherwise, events may not be ready to be filtered when you call this method.
	/// </para>
	/// </remarks>
#pragma warning disable IDE0079
#pragma warning disable CA1822 // this is intentionally an instance method
	public bool TryPeepEvents(int count, EventAction action, EventType minType, EventType maxType, out int counted)
#pragma warning restore CA1822
#pragma warning restore IDE0079
	{
		unsafe
		{
			counted = SDL_PeepEvents(events: null, numevents: count, action, minType, maxType);

			return counted is >= 0;
		}
	}

	/// <summary>
	/// Tries to check the current event queue for any events and retrieves them, or tries to add new events to the the current event queue
	/// </summary>
	/// <param name="events">A destination span to store the retrieved events or a source span of new events to add</param>
	/// <param name="action">The action to perform on the events</param>
	/// <param name="stored">The number of events stored in the destination span or the number of events added to the queue</param>
	/// <returns><c><see langword="true"/></c>, if the operation was successful; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method tries to perform different actions on the event queue depending on the specified <paramref name="action"/>:
	/// <list type="bullet">
	///		<item>
	///			<term><see cref="EventAction.Add"/></term>
	///			<description>
	///				All events from the source <paramref name="events"/> span are added to the event queue.
	///				The <paramref name="stored"/> output parameter contains the number of events that were added to the queue.
	///			</description>
	///		</item>
	///		<item>
	///			<term><see cref="EventAction.Peek"/></term>
	///			<description>
	///				Up to <paramref name="events"/>.<see cref="Span{T}.Length">Length</see> events from the front of the event queue are copied into the span <em>without removing them from the queue</em>.
	///				The <paramref name="stored"/> output parameter contains the number of events that were copied into the span.
	///			</description>
	///		</item>
	///		<item>
	///			<term><see cref="EventAction.Get"/></term>
	///			<description>
	///				Up to <paramref name="events"/>.<see cref="Span{T}.Length">Length</see> events from the front of the event queue are copied into the span <em>and removed from the queue</em>.
	///				The <paramref name="stored"/> output parameter contains the number of events that were copied into the span and removed from the current event queue.
	///			</description>
	///		</item>
	/// </list>
	/// </para>
	/// <para>
	///	You may have to call <see cref="PumpEvents"/> before calling this method. Otherwise, events may not be ready to be filtered when you call this method.
	/// </para>
	/// </remarks>
	public bool TryPeepEvents(Span<Event> events, EventAction action, out int stored)
#pragma warning disable CS0618 // we allow that here
		=> TryPeepEvents(events, action, minType: EventType.First, EventType.Last, out stored);
#pragma warning restore CS0618

	/// <summary>
	/// Tries to check the current event queue for events of a specified type and retrieves them, or tries to add new events to the the current event queue
	/// </summary>
	/// <param name="events">A destination span to store the retrieved events or a source span of new events to add</param>
	/// <param name="action">The action to perform on the events</param>
	/// <param name="type">The type of the event to check for</param>
	/// <param name="stored">The number of events stored in the destination span or the number of events added to the queue</param>
	/// <returns><c><see langword="true"/></c>, if the operation was successful; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method tries to perform different actions on the event queue depending on the specified <paramref name="action"/>:
	/// <list type="bullet">
	///		<item>
	///			<term><see cref="EventAction.Add"/></term>
	///			<description>
	///				All events from the source <paramref name="events"/> span are added to the event queue.
	///				The <paramref name="type"/> parameter is ignored in this case.
	///				The <paramref name="stored"/> output parameter contains the number of events that were added to the queue.
	///			</description>
	///		</item>
	///		<item>
	///			<term><see cref="EventAction.Peek"/></term>
	///			<description>
	///				Up to <paramref name="events"/>.<see cref="Span{T}.Length">Length</see> events from the front of the event queue that match the specified <paramref name="type"/> are copied into the span <em>without removing them from the queue</em>.
	///				The <paramref name="stored"/> output parameter contains the number of events that were copied into the span.
	///			</description>
	///		</item>
	///		<item>
	///			<term><see cref="EventAction.Get"/></term>
	///			<description>
	///				Up to <paramref name="events"/>.<see cref="Span{T}.Length">Length</see> events from the front of the event queue that match the specified <paramref name="type"/> are copied into the span <em>and removed from the queue</em>.
	///				The <paramref name="stored"/> output parameter contains the number of events that were copied into the span and removed from the current event queue.
	///			</description>
	///		</item>
	/// </list>
	/// </para>
	/// <para>
	///	You may have to call <see cref="PumpEvents"/> before calling this method. Otherwise, events may not be ready to be filtered when you call this method.
	/// </para>
	/// </remarks>
	public bool TryPeepEvents(Span<Event> events, EventAction action, EventType type, out int stored)
		=> TryPeepEvents(events, action, minType: type, maxType: type, out stored);

	/// <summary>
	/// Tries to check the current event queue for events in specified type range and retrieves them, or tries to add new events to the the current event queue
	/// </summary>
	/// <param name="events">A destination span to store the retrieved events or a source span of new events to add</param>
	/// <param name="action">The action to perform on the events</param>
	/// <param name="minType">The inclusive lower bound of the event type range to check</param>
	/// <param name="maxType">The inclusive upper bound of the event type range to check</param>
	/// <param name="stored">The number of events stored in the destination span or the number of events added to the queue</param>
	/// <returns><c><see langword="true"/></c>, if the operation was successful; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method tries to perform different actions on the event queue depending on the specified <paramref name="action"/>:
	/// <list type="bullet">
	///		<item>
	///			<term><see cref="EventAction.Add"/></term>
	///			<description>
	///				All events from the source <paramref name="events"/> span are added to the event queue.
	///				The <paramref name="minType"/> and <paramref name="maxType"/> parameters are ignored in this case.
	///				The <paramref name="stored"/> output parameter contains the number of events that were added to the queue.
	///			</description>
	///		</item>
	///		<item>
	///			<term><see cref="EventAction.Peek"/></term>
	///			<description>
	///				Up to <paramref name="events"/>.<see cref="Span{T}.Length">Length</see> events from the front of the event queue that match the specified <paramref name="minType"/> and <paramref name="maxType"/> range are copied into the span <em>without removing them from the queue</em>.
	///				The <paramref name="stored"/> output parameter contains the number of events that were copied into the span.
	///			</description>
	///		</item>
	///		<item>
	///			<term><see cref="EventAction.Get"/></term>
	///			<description>
	///				Up to <paramref name="events"/>.<see cref="Span{T}.Length">Length</see> events from the front of the event queue that match the specified <paramref name="minType"/> and <paramref name="maxType"/> range are copied into the span <em>and removed from the queue</em>.
	///				The <paramref name="stored"/> output parameter contains the number of events that were copied into the span and removed from the current event queue.
	///			</description>
	///		</item>
	/// </list>
	/// </para>
	/// <para>
	///	You may have to call <see cref="PumpEvents"/> before calling this method. Otherwise, events may not be ready to be filtered when you call this method.
	/// </para>
	/// </remarks>
#pragma warning disable IDE0079
#pragma warning disable CA1822 // this is intentionally an instance method
	public bool TryPeepEvents(Span<Event> events, EventAction action, EventType minType, EventType maxType, out int stored)
#pragma warning restore CA1822
#pragma warning restore IDE0079
	{
		unsafe
		{
			fixed (Event* eventsPtr = events)
			{
				stored = SDL_PeepEvents(eventsPtr, events.Length, action, minType, maxType);

				return stored is >= 0;
			}
		}
	}

	/// <summary>
	/// Tries to remove any events from the current event queue
	/// </summary>
	/// <param name="count">The maximum number of events to remove</param>
	/// <param name="removed">The number of events that were removed from the current event queue</param>
	/// <returns><c><see langword="true"/></c>, if the operation was successful; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// Calling this method is effectively calling <c><see cref="TryPeepEvents(int, EventAction, EventType, EventType, out int)">TryPeepEvents</see>(<paramref name="count"/>, <see cref="EventAction"/>.<see cref="EventAction.Get">Get</see>, <see langword="out"/> <paramref name="removed"/>)</c>.
	/// </para>
	/// </remarks>
	public bool TryRemoveEvents(int count, out int removed)
		=> TryPeepEvents(count, EventAction.Get, counted: out removed);

	/// <summary>
	/// Tries to remove events of a specified type from the current event queue
	/// </summary>
	/// <param name="count">The maximum number of events to remove</param>
	/// <param name="type">The type of events to remove</param>
	/// <param name="removed">The number of events that were removed from the current event queue</param>
	/// <returns><c><see langword="true"/></c>, if the operation was successful; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// Calling this method is effectively calling <c><see cref="TryPeepEvents(int, EventAction, EventType, EventType, out int)">TryPeepEvents</see>(<paramref name="count"/>, <see cref="EventAction"/>.<see cref="EventAction.Get">Get</see>, <paramref name="type"/>, <see langword="out"/> <paramref name="removed"/>)</c>.
	/// </para>
	/// </remarks>
	public bool TryRemoveEvents(int count, EventType type, out int removed)
		=> TryPeepEvents(count, EventAction.Get, type, counted: out removed);

	/// <summary>
	/// Tries to remove events in the specified type range from the current event queue
	/// </summary>
	/// <param name="count">The maximum number of events to remove</param>
	/// <param name="minType">The inclusive lower bound of the event type range to remove</param>
	/// <param name="maxType">The inclusive upper bound of the event type range to remove</param>
	/// <param name="removed">The number of events that were removed from the current event queue</param>
	/// <returns><c><see langword="true"/></c>, if the operation was successful; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// Calling this method is effectively calling <c><see cref="TryPeepEvents(int, EventAction, EventType, EventType, out int)">TryPeepEvents</see>(<paramref name="count"/>, <see cref="EventAction"/>.<see cref="EventAction.Get">Get</see>, <paramref name="minType"/>, <paramref name="maxType"/>, <see langword="out"/> <paramref name="removed"/>)</c>.
	/// </para>
	/// </remarks>
	public bool TryRemoveEvents(int count, EventType minType, EventType maxType, out int removed)
		=> TryPeepEvents(count, EventAction.Get, minType, maxType, counted: out removed);

	/// <summary>
	/// Tries to poll for pending events from the current event queue
	/// </summary>
	/// <returns><c><see langword="true"/></c>, if there is a pending event in the event queue; otherwise, <c><see langword="false"/></c></returns>
	/// <remarks>
	/// <para>
	/// If there is a pending event in the current event queue, it will <em>not</em> be removed from the event queue, and this method will simply return <c><see langword="true"/></c>.
	/// In the case this method returned <c><see langword="false"/></c>, there was no pending event in the current event queue.
	/// The event queue remains unchanged in any case. Therefore you can use this method to check if there are pending events in the current event queue.
	/// </para>
	/// <para>
	/// As this method may implicitly call <see cref="PumpEvents"/>, you can only call this method in the thread that set the video mode.
	/// </para>
	/// <para>
	/// Note that Windows (and possibly other platforms) has a quirk about how it handles events while dragging/resizing a window, which can cause this function to block for significant amounts of time.
	/// Technical explanations and solutions are discussed on the wiki: <see href="https://wiki.libsdl.org/SDL3/AppFreezeDuringDrag"/>.
	/// </para>
	/// </remarks>
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

	/// <summary>
	/// Tries to poll for pending events from the current event queue
	/// </summary>
	/// <param name="event">The next pending event from the event queue, if this method returns <c><see langword="true"/></c></param>
	/// <returns><c><see langword="true"/></c>, if there was a pending event in the event queue and it was copied into <paramref name="event"/>; otherwise, <c><see langword="false"/></c></returns>
	/// <remarks>
	/// <para>
	/// If there is a pending event in the current event queue, it will be copied into the <paramref name="event"/> argument and removed from the event queue, and then this method will return <c><see langword="true"/></c>.
	/// In the case this method returned <c><see langword="false"/></c>, there was no pending event in the current event queue and the event queue remained unchanged.
	/// </para>
	/// <para>
	/// As this method may implicitly call <see cref="PumpEvents"/>, you can only call this method in the thread that set the video mode.
	/// </para>
	/// <para>
	/// <see cref="TryPollEvent(out Event)"/> is the favored way of receiving system events since it can be done from the main loop and does not suspend the main loop while waiting on an event to be posted.
	/// </para>
	/// <para>
	/// The common practice is to fully process the current event queue once every frame, usually as a first step before updating the game's state.
	/// </para>
	/// <para>
	/// Note that Windows (and possibly other platforms) has a quirk about how it handles events while dragging/resizing a window, which can cause this function to block for significant amounts of time.
	/// Technical explanations and solutions are discussed on the wiki: <see href="https://wiki.libsdl.org/SDL3/AppFreezeDuringDrag"/>.
	/// </para>
	/// </remarks>
#pragma warning disable IDE0079
#pragma warning disable CA1822 // this is intentionally an instance method
	public bool TryPollEvent(out Event @event)
#pragma warning restore CA1822
#pragma warning restore IDE0079
	{
		unsafe
		{
			fixed (Event* eventPtr = &@event)
			{
				return SDL_PollEvent(eventPtr);
			}
		}
	}

	/// <summary>
	/// Tries to add a new event to the current event queue
	/// </summary>
	/// <param name="event">The event to add to the queue</param>
	/// <returns><c><see langword="true"/></c>, if the event was successfully added to the queue; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// Events that are supposed to be added via this method are passed to the <see cref="EventFilter"/> and are susceptible to being <see cref="EventTypeExtensions.set_Enabled(EventType, bool)">disabled</see>.
	/// If the event is filtered out, this method will return <c><see langword="false"/></c> (use <see cref="Error.TryGet(out string?)"/> to check if the event was filtered out or if there was another kind of failure).
	/// If you want to bypass the filter you can use <see cref="TryAddEvents(ReadOnlySpan{Event}, out int)"/> instead.
	/// </para>
	/// <para>
	/// Another common reason for this method failing is the event queue being full.
	/// </para>
	/// <para>
	/// The event queue can actually be used as a two way communication channel.
	/// Not only can events be read from the queue, but the user can also push their own events onto it.
	/// The <paramref name="event"/> will be copied into the queue, and afterwards user can forget about it until it's (<see cref="TryPollEvent(out Event)">polled</see>) by themselves or somewhere else.
	/// </para>
	/// <para>
	/// Note: Pushing device input events onto the queue doesn't modify the state of the device within SDL.
	/// </para>
	/// <para>
	/// For pushing user defined custom events, please use <see cref="EventTypeExtensions.TryRegister(out EventType)"/> or <see cref="EventTypeExtensions.TryRegister(Span{EventType})"/> to get event types that does not conflict with other code that also wants its own custom event types.
	/// </para>
	/// </remarks>
#pragma warning disable IDE0079
#pragma warning disable CA1822 // this is intentionally an instance method
	public bool TryPushEvent(in Event @event)
#pragma warning restore CA1822
#pragma warning restore IDE0079
	{
		unsafe
		{
			fixed (Event* eventPtr = &@event)
			{
				return SDL_PushEvent(eventPtr);
			}
		}
	}

	/// <summary>
	/// Tries to wait for pending events from the current event queue indefinitely
	/// </summary>
	/// <returns><c><see langword="true"/></c>, if there was a pending event received in the event queue; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// If there is a pending event recieved in the current event queue, it will <em>not</em> be removed from the event queue, and this method will simply return <c><see langword="true"/></c>.
	/// In the case this method returned <c><see langword="false"/></c>, there was an error while waiting and you should check <see cref="Error.TryGet(out string?)"/> for more information.
	/// The event queue remains unchanged in any case. Therefore you can use this method to block and wait until there are pending events recieved in the current event queue.
	/// </para>
	/// <para>
	/// As this method may implicitly call <see cref="PumpEvents"/>, you can only call this method in the thread that set the video mode.
	/// </para>
	/// </remarks>
#pragma warning disable IDE0079
#pragma warning disable CA1822 // this is intentionally an instance method
	public bool TryWaitForEvent()
#pragma warning restore CA1822
#pragma warning restore IDE0079
	{
		unsafe
		{
			return SDL_WaitEvent(@event: null);
		}
	}

	/// <summary>
	/// Tries to wait for pending events from the current event queue indefinitely
	/// </summary>
	/// <param name="event">The next pending event from the event queue</param>
	/// <returns><c><see langword="true"/></c>, if there was a pending event received in the event queue and it was copied into <paramref name="event"/>; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// If there is a pending event recieved in the current event queue, it will be copied into the <paramref name="event"/> argument and removed from the event queue, and then this method will return <c><see langword="true"/></c>.
	/// In the case this method returned <c><see langword="false"/></c>, there was an error while waiting and you should check <see cref="Error.TryGet(out string?)"/> for more information.
	/// </para>
	/// <para>
	/// As this method may implicitly call <see cref="PumpEvents"/>, you can only call this method in the thread that set the video mode.
	/// </para>
	/// </remarks>
#pragma warning disable IDE0079
#pragma warning disable CA1822 // this is intentionally an instance method
	public bool TryWaitForEvent(out Event @event)
#pragma warning restore CA1822
#pragma warning restore IDE0079
	{
		unsafe
		{
			fixed (Event* eventPtr = &@event)
			{
				return SDL_WaitEvent(eventPtr);
			}
		}
	}

	/// <summary>
	/// Tries to wait until a specified timeout for pending events from the current event queue
	/// </summary>
	/// <param name="timeoutMs">The timeout in milliseconds</param>
	/// <returns><c><see langword="true"/></c>, if there was a pending event recieved in the event queue before the timeout; otherwise, <c><see langword="false"/></c></returns>
	/// <remarks>
	/// <para>
	/// If there is a pending event recieved in the current event queue before the timeout, it will <em>not</em> be removed from the event queue, and this method will simply return <c><see langword="true"/></c>.
	/// In the case this method returned <c><see langword="false"/></c>, there was no pending event recieved in the current event queue until the specified timeout.
	/// The event queue remains unchanged in any case. Therefore you can use this method to block and wait until there are pending events recieved in the current event queue.
	/// </para>
	/// <para>
	/// As this method may implicitly call <see cref="PumpEvents"/>, you can only call this method in the thread that set the video mode.
	/// </para>
	/// <para>
	/// The specified timeout is not guaranteed, the actual wait time could be longer due to system scheduling.
	/// </para>
	/// </remarks>
#pragma warning disable IDE0079
#pragma warning disable CA1822 // this is intentionally an instance method
	public bool TryWaitForEvent(int timeoutMs)
#pragma warning restore CA1822
#pragma warning restore IDE0079
	{
		unsafe
		{
			return SDL_WaitEventTimeout(@event: null, timeoutMs);
		}
	}

	/// <summary>
	/// Tries to wait until a specified timeout for pending events from the current event queue
	/// </summary>
	/// <param name="timeout">The timeout</param>
	/// <returns><c><see langword="true"/></c>, if there was a pending event recieved in the event queue before the timeout; otherwise, <c><see langword="false"/></c></returns>
	/// <remarks>
	/// <para>
	/// If there is a pending event recieved in the current event queue before the timeout, it will <em>not</em> be removed from the event queue, and this method will simply return <c><see langword="true"/></c>.
	/// In the case this method returned <c><see langword="false"/></c>, there was no pending event recieved in the current event queue until the specified timeout.
	/// The event queue remains unchanged in any case. Therefore you can use this method to block and wait until there are pending events recieved in the current event queue.
	/// </para>
	/// <para>
	/// As this method may implicitly call <see cref="PumpEvents"/>, you can only call this method in the thread that set the video mode.
	/// </para>
	/// <para>
	/// The specified timeout is not guaranteed, the actual wait time could be longer due to system scheduling.
	/// </para>
	/// </remarks>
	public bool TryWaitForEvent(TimeSpan timeout)
		=> TryWaitForEvent(timeoutMs: unchecked((int)double.Ceiling(timeout.TotalMilliseconds)));

	/// <summary>
	/// Tries to wait until a specified timeout for pending events from the current event queue
	/// </summary>
	/// <param name="event">The next pending event from the event queue, if this method returns <c><see langword="true"/></c></param>
	/// <param name="timeoutMs">The timeout in milliseconds</param>
	/// <returns><c><see langword="true"/></c>, if there was a pending event recieved in the event queue before the timeout and it was copied into <paramref name="event"/>; otherwise, <c><see langword="false"/></c></returns>
	/// <remarks>
	/// <para>
	/// If there is a pending event recieved in the current event queue before the timeout, it will be copied into the <paramref name="event"/> argument and removed from the event queue, and then this method will return <c><see langword="true"/></c>.
	/// In the case this method returned <c><see langword="false"/></c>, there was no pending event recieved in the current event queue until the specified timeout.
	/// </para>
	/// <para>
	/// As this method may implicitly call <see cref="PumpEvents"/>, you can only call this method in the thread that set the video mode.
	/// </para>
	/// <para>
	/// The specified timeout is not guaranteed, the actual wait time could be longer due to system scheduling.
	/// </para>
	/// </remarks>
#pragma warning disable IDE0079
#pragma warning disable CA1822 // this is intentionally an instance method
	public bool TryWaitForEvent(out Event @event, int timeoutMs)
#pragma warning restore CA1822
#pragma warning restore IDE0079
	{
		unsafe
		{
			fixed (Event* eventPtr = &@event)
			{
				return SDL_WaitEventTimeout(eventPtr, timeoutMs);
			}
		}
	}

	/// <summary>
	/// Tries to wait until a specified timeout for pending events from the current event queue
	/// </summary>
	/// <param name="event">The next pending event from the event queue, if this method returns <c><see langword="true"/></c></param>
	/// <param name="timeout">The timeout</param>
	/// <returns><c><see langword="true"/></c>, if there was a pending event recieved in the event queue before the timeout and it was copied into <paramref name="event"/>; otherwise, <c><see langword="false"/></c></returns>
	/// <remarks>
	/// <para>
	/// If there is a pending event recieved in the current event queue before the timeout, it will be copied into the <paramref name="event"/> argument and removed from the event queue, and then this method will return <c><see langword="true"/></c>.
	/// In the case this method returned <c><see langword="false"/></c>, there was no pending event recieved in the current event queue until the specified timeout.
	/// </para>
	/// <para>
	/// As this method may implicitly call <see cref="PumpEvents"/>, you can only call this method in the thread that set the video mode.
	/// </para>
	/// <para>
	/// The specified timeout is not guaranteed, the actual wait time could be longer due to system scheduling.
	/// </para>
	/// </remarks>
	public bool TryWaitForEvent(out Event @event, TimeSpan timeout)
		=> TryWaitForEvent(out @event, timeoutMs: unchecked((int)double.Ceiling(timeout.TotalMilliseconds)));
}
