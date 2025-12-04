/*
namespace Sdl3Sharp.Threading;

//TODO: make this a reference type
[StructLayout(LayoutKind.Sequential)]
#pragma warning disable IDE0079
#pragma warning disable CA2231
public partial struct InitState
#pragma warning restore CA2231
#pragma warning restore IDE0079
{
	private AtomicInt32 mStatus;
#pragma warning disable IDE0044
	private ulong mThreadId;
	private unsafe void* mReserved;
#pragma warning restore IDE0044

	public static bool ConditionallyInitialize(ref InitState state, Func<bool> initializer, bool defaultReturnValue = true)
	{
		if (initializer is null)
		{
			failInitializerArgumentNull();
		}

		if (!ShouldInit(ref state))
		{
			return defaultReturnValue;
		}

		var initialized = initializer();
		
		SetInitialized(ref state, initialized);

		return initialized;

		[DoesNotReturn]
		static void failInitializerArgumentNull() => throw new ArgumentNullException(nameof(initializer));
	}

	public static bool ConditionallyInvoke(ref InitState state, Action action)
	{
		if (action is null)
		{
			failActionArgumentNull();
		}

		if (ShouldInit(ref state))
		{
			SetInitialized(ref state, false);

			return false;
		}

		action();

		return true;

		[DoesNotReturn]
		static void failActionArgumentNull() => throw new ArgumentNullException(nameof(action));
	}

	public static void ConditionallyUninitialize(ref InitState state, Action uninitializer)
	{
		if (uninitializer is null)
		{
			failUninitializerArgumentNull();
		}

		if (!ShouldQuit(ref state))
		{
			return;
		}

		uninitializer();

		SetInitialized(ref state, false);

		[DoesNotReturn]
		static void failUninitializerArgumentNull() => throw new ArgumentNullException(nameof(uninitializer));
	}

	internal static void SetInitialized(ref InitState state, bool initialized)
	{
		unsafe
		{
			// there's not much else that we could do aside from fixing the reference,
			// as it could be pointing to a memory which is part of an object on the managed heap
			fixed (InitState* statePtr = &state)
			{
				SDL_SetInitialized(statePtr, initialized);
			}
		}
	}

	internal static bool ShouldInit(ref InitState state)
	{
		unsafe
		{
			// there's not much else that we could do aside from fixing the reference,
			// as it could be pointing to a memory which is part of an object on the managed heap
			fixed (InitState* statePtr = &state)
			{
				return SDL_ShouldInit(statePtr);
			}
		}
	}

	internal static bool ShouldQuit(ref InitState state)
	{
		unsafe
		{
			// there's not much else that we could do aside from fixing the reference,
			// as it could be pointing to a memory which is part of an object on the managed heap
			fixed (InitState* statePtr = &state)
			{
				return SDL_ShouldQuit(statePtr);
			}
		}
	}

	public readonly InitStatus Status { get => unchecked((InitStatus)mStatus.Get()); }

	public readonly ulong ThreadId { get => mThreadId; }

	[DoesNotReturn]
	public readonly override bool Equals([NotNullWhen(true)] object? obj) => throw new NotSupportedException();

	[DoesNotReturn]
	public readonly override int GetHashCode() => throw new NotSupportedException();

	[DoesNotReturn]
	public readonly override string ToString() => throw new NotSupportedException();
}
*/