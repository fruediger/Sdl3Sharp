using System;

namespace Sdl3Sharp.Events;

internal interface IEventType : IComparable, IComparable<IEventType>, IEquatable<IEventType>
{
	internal EventType Base { get; }
}
