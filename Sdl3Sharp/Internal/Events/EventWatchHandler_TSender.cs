using Sdl3Sharp.Events;
using System;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Internal.Events;

internal abstract partial class EventWatchHandler<TSender>(TSender sender) : EventWatchHandler
	where TSender : class
{
	private WeakReference<TSender>? mReference = new(sender);

	protected override void Dispose(bool disposing)
	{
		mReference = null;

		base.Dispose(disposing);
	}

	private protected sealed override bool TryInvoke(EventRef<Event> eventRef)
	{
		if (mReference?.TryGetTarget(out var target) is true && Validate(target, eventRef))
		{
			Invoke(target, eventRef);

			return true;
		}

		return false;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private protected virtual bool Validate(TSender sender, EventRef<Event> eventRef) => true;

	private protected abstract void Invoke(TSender sender, EventRef<Event> eventRef);
}
