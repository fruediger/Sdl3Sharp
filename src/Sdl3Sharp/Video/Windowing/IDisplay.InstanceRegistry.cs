using Sdl3Sharp.Video.Windowing.Drivers;
using System;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace Sdl3Sharp.Video.Windowing;

partial interface IDisplay
{
	private static readonly ConcurrentDictionary<uint, WeakReference<IDisplay>> mKnownInstances = [];

	internal static void Register<TDisplay>(TDisplay display)
		where TDisplay : notnull, IDisplay
	{
		if (display is { Id: var id } && id is not 0)
		{
			mKnownInstances.AddOrUpdate(id, addRef, updateRef, display);
		}

		static WeakReference<IDisplay> addRef(uint id, TDisplay newDisplay) => new(newDisplay);

		static WeakReference<IDisplay> updateRef(uint id, WeakReference<IDisplay> previousDisplayRef, TDisplay newDisplay)
		{
			/*
			 * IDisplays aren't disposable, so there's nothing to do for us here
			 * 
			if (previousDisplayRef.TryGetTarget(out var previousDisplay))
			{
				// dispose...
			}
			*/

			previousDisplayRef.SetTarget(newDisplay);

			return previousDisplayRef;
		}
	}

	internal static void Deregister<TDisplay>(TDisplay display)
		where TDisplay : notnull, IDisplay
	{
		if (display is { Id: var id })
		{
			mKnownInstances.TryRemove(id, out _);
		}
	}

	internal static bool TryGetOrCreate(uint displayId, [NotNullWhen(true)] out IDisplay? result)
	{
		if (displayId is 0)
		{
			result = null;
			return false;
		}

		var displayRef = mKnownInstances.GetOrAdd(displayId, createRef);

		if (!displayRef.TryGetTarget(out result))
		{
			displayRef.SetTarget(result = create(displayId));
		}

		return true;

		static WeakReference<IDisplay> createRef(uint displayId) => new(create(displayId)); 

		static IDisplay create(uint displayId)
		{
			if (!TryCreateFromRegisteredDriver(displayId, register: false, out var result))
			{
				result = new Display<GenericFallbackWindowingDriver>(displayId, register: false);
			}

			return result;
		}
	}
}
