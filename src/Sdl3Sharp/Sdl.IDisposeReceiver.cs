using Sdl3Sharp.Internal;
using System;
using System.Collections.Concurrent;

namespace Sdl3Sharp;

partial class Sdl
{
	/// <summary>
	/// Provides a mechanism for objects to get informed when a <see cref="Sdl"/> instance gets <see cref="Dispose">disposed</see>
	/// </summary>
	public interface IDisposeReceiver
	{
		/// <summary>
		/// When this method is called, it informs the implementer that the provided <paramref name="sdl"/> instance is in the process of getting disposed
		/// </summary>
		/// <param name="sdl">The <see cref="Sdl"/> instance that gets disposed</param>
		/// <remarks>
		/// You don't need to <see cref="TryDeregisterDisposable(IDisposeReceiver)">deregister</see> the implementer from the <paramref name="sdl"/> instance here, as it would be during the process of the <paramref name="sdl"/> instance getting disposed anyway.
		/// Though, it's not an error to do so.
		/// </remarks>
		void DisposeFromSdl(Sdl sdl);
	}

	private readonly ConcurrentDictionary<WeakReference<IDisposeReceiver>, byte> mRegisteredDisposeReceivers = new(WeakReferenceEqualityComparer<IDisposeReceiver>.Instance);

	/// <summary>
	/// Tries to deregister a formerly registered <see cref="IDisposeReceiver"/> from the <see cref="Sdl"/> instance,
	/// so that it won't longer receive a call to <see cref="IDisposeReceiver.DisposeFromSdl(Sdl)"/> from that <see cref="Sdl"/> instance
	/// </summary>
	/// <param name="disposeReceiver">The receiver to be deregistered</param>
	/// <returns><c><see langword="true"/></c>, if <paramref name="disposeReceiver"/> was successfully deregistered from the <see cref="Sdl"/> instance; otherwise, <c><see langword="false"/></c></returns>
	public bool TryDeregisterDisposable(IDisposeReceiver disposeReceiver)
		// for performance sake, we don't lock here
		// the worst that can happen are some dangling references, which will get eventually collected by the GC
		=> mLifetimeState is LifetimeState.BeforeRun or LifetimeState.Running
		&& mRegisteredDisposeReceivers.TryRemove(new(disposeReceiver), out _);

	/// <summary>
	/// Tries to register an <see cref="IDisposeReceiver"/> with the <see cref="Sdl"/> instance,
	/// so it will receive a call to <see cref="IDisposeReceiver.DisposeFromSdl(Sdl)"/> when that <see cref="Sdl"/> instance gets <see cref="Dispose">disposed</see>
	/// </summary>
	/// <param name="disposeReceiver">The receiver to be registered</param>
	/// <returns><c><see langword="true"/></c>, if <paramref name="disposeReceiver"/> was successfully registered with the <see cref="Sdl"/> instance; otherwise, <c><see langword="false"/></c></returns>
	/// <remarks>
	/// This method also returns <c><see langword="true"/></c>, if the <paramref name="disposeReceiver"/> was already registered with the <see cref="Sdl"/> instance before
	/// </remarks>
	public bool TryRegisterDisposable(IDisposeReceiver disposeReceiver)
		// for performance sake, we don't lock here
		// the worst that can happen are some dangling references, which will get eventually collected by the GC
		=> mLifetimeState is LifetimeState.BeforeRun or LifetimeState.Running
		&& new WeakReference<IDisposeReceiver>(disposeReceiver) switch
		{
			var disposeReceiverReference
				=> mRegisteredDisposeReceivers.TryAdd(disposeReceiverReference, default)
				|| mRegisteredDisposeReceivers.ContainsKey(disposeReceiverReference)
		};
}
