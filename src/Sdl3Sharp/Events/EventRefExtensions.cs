using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Events;

public static class EventRefExtensions
{
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool TryAs<TEvent>(this EventRef<Event> sourceEventRef, EventType<TEvent> type, out EventRef<TEvent> resultEventRef)
		where TEvent : struct, ICommonEvent<TEvent>
	{
		if (sourceEventRef.Type == type)
		{
			resultEventRef = new(ref TEvent.GetReference(ref sourceEventRef.Event));
			return true;
		}

		resultEventRef = default;
		return false;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool TryAs<TEvent>(this EventRefReadOnly<Event> sourceEventRef, EventType<TEvent> type, out EventRefReadOnly<TEvent> resultEventRef)
		where TEvent : struct, ICommonEvent<TEvent>
	{
		if (sourceEventRef.Type == type)
		{
			resultEventRef = new(ref TEvent.GetReference(ref Unsafe.AsRef(in sourceEventRef.Event)));
			return true;
		}

		resultEventRef = default;
		return false;
	}
}
