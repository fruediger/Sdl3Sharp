namespace Sdl3Sharp;

/// <summary>
/// Represents action to take in <see cref="Sdl.TryPeepEvents(System.Span{Sdl3Sharp.Events.Event}, Sdl3Sharp.EventAction, Sdl3Sharp.Events.EventType, Sdl3Sharp.Events.EventType, out int)"/>
/// </summary>
public enum EventAction
{
	/// <summary>Adds events to the event queue</summary>
	Add,

	/// <summary>Retrieves events from the event queue without removing them</summary>
	Peek,

	/// <summary>Retrieves and removes events from the event queue</summary>
	Get
}
