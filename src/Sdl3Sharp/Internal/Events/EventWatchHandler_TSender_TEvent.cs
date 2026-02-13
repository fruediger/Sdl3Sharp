using Sdl3Sharp.Events;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Internal.Events;

internal sealed class EventWatchHandler<TSender, TEvent>(TSender sender, EventType type) : EventWatchHandler()
	where TSender : class
	where TEvent : unmanaged, ICommonEvent<TEvent>
{
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private static EventType ValidateType(EventType type)
	{
		Debug.Assert(TEvent.Accepts(type));

		return type;
	}

	private System.WeakReference<TSender>? mReference = new(sender);
	private readonly EventType mType = ValidateType(type);

	public EventHandler<TSender, TEvent>? EventHandler { get; set; }

	protected override void Dispose(bool disposing)
	{
		mReference = null;
		EventHandler = null;

		base.Dispose(disposing);
	}

	private protected override bool Invoke(ref Event @event)
	{
		if (mReference?.TryGetTarget(out var sender) is not true)
		{
			return false;
		}

		if (@event.Type == mType)
		{
			EventHandler?.Invoke(sender, ref @event.UnsafeAs<TEvent>());
		}

		return true;
	}
}
