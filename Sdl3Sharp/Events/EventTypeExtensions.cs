using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Events;

/// <summary>
/// Provides extension methods for <see cref="EventType"/> and <see cref="EventType{TEvent}"/>
/// </summary>
public static class EventTypeExtensions
{
	/// <param name="type">The event type representing a user defined event</param>
	/// <inheritdoc cref="EventType.TryGetUserValue(out int)"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
#pragma warning disable CS1573 // we inheritdoc' that
	public static bool TryGetUserValue(this EventType<UserEvent> type, out int userValue)
#pragma warning restore CS1573
		=> ((EventType)type).TryGetUserValue(out userValue);
}
