using Sdl3Sharp.Events;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Internal.Events;

internal abstract class EventWatchHandler<TSender, TEvent>(TSender sender, EventType<TEvent> type) : EventWatchHandler<TSender>(sender)
	where TSender : class
	where TEvent : struct, ICommonEvent<TEvent>
{ 
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private protected sealed override void Invoke(TSender sender, EventRef<Event> eventRef)
	{
		if (eventRef.TryAs(type, out var typedEventRef))
		{
			Invoke(sender, typedEventRef);
		}
	}

	private protected abstract void Invoke(TSender sender, EventRef<TEvent> eventRef);
}
