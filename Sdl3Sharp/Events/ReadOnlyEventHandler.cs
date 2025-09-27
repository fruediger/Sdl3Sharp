namespace Sdl3Sharp.Events;

public delegate void ReadOnlyEventHandler<TSender, TEvent>(TSender sender, EventRefReadOnly<TEvent> eventRef) where TEvent : struct, ICommonEvent<TEvent>;
