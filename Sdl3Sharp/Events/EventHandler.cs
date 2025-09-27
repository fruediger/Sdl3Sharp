namespace Sdl3Sharp.Events;

public delegate void EventHandler<TSender, TEvent>(TSender sender, EventRef<TEvent> eventRef) where TEvent : struct, ICommonEvent<TEvent>;
