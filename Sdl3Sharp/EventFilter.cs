using Sdl3Sharp.Events;

namespace Sdl3Sharp;

public delegate bool EventFilter(EventRef<Event> eventRef);
