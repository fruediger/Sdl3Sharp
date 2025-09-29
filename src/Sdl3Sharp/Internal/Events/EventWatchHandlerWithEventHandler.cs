using Sdl3Sharp.Events;

namespace Sdl3Sharp.Internal.Events;

internal sealed class EventWatchHandlerWithEventHandler<TSender, TEvent>(TSender sender, EventType<TEvent> type) : EventWatchHandler<TSender, TEvent>(sender, type)
	where TSender : class
	where TEvent : struct, ICommonEvent<TEvent>
{
	public ReadOnlyEventHandler<TSender, TEvent>? EventHandler { get; set; } = null;

	protected override void Dispose(bool disposing)
	{
		EventHandler = null;

		base.Dispose(disposing);
	}

	private protected override void Invoke(TSender sender, EventRef<TEvent> eventRef)
		=> EventHandler?.Invoke(sender, eventRef);
}
