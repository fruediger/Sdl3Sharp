namespace Sdl3Sharp.Events;

/// <summary>
/// Represents a method that handles events of a specific type
/// </summary>
/// <typeparam name="TEvent">The type of event to handle</typeparam>
/// <param name="event">The event to handle</param>
public delegate void EventHandler<TEvent>(ref readonly TEvent @event);

/// <summary>
/// Represents a method that handles events of a specific type from a specific sender
/// </summary>
/// <typeparam name="TSender">The type of the sender</typeparam>
/// <typeparam name="TEvent">The type of event to handle</typeparam>
/// <param name="sender">The sender of the event</param>
/// <param name="event">The event to handle</param>
public delegate void EventHandler<in TSender, TEvent>(TSender sender, ref readonly TEvent @event);
