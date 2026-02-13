using Sdl3Sharp.Events;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Internal.Events;

internal sealed class EventWatchHandler<TEvent>(EventType type) : EventWatchHandler()
	where TEvent : unmanaged, ICommonEvent<TEvent>
{
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private static EventType ValidateType(EventType type)
	{
		Debug.Assert(TEvent.Accepts(type));

		return type;
	}

	private readonly EventType mType = ValidateType(type);

	public EventHandler<TEvent>? EventHandler { get; set; }

	protected override void Dispose(bool disposing)
	{
		EventHandler = null;

		base.Dispose(disposing);
	}

	private protected override bool Invoke(ref Event @event)
	{
		if (@event.Type == mType)
		{
			EventHandler?.Invoke(ref @event.UnsafeAs<TEvent>());
		}

		return true;
	}
}
