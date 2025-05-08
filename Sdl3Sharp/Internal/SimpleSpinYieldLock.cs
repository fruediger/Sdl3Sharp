using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace Sdl3Sharp.Internal;

internal struct SimpleSpinYieldLock()
{
	private readonly ManualResetEventSlim mLockEvent = new(initialState: true, spinCount: 0);
	private volatile uint mLockState = 0;

	public void Enter(int index)
	{
		if (index is < 0 or > 31)
		{
			failIndexArgumentOutOfRange();
		}

		var mask = 0b1u << index;

		var spinWait = new SpinWait();
		while ((Interlocked.Or(ref mLockState, mask) & mask) is not 0)
		{
			if (spinWait.NextSpinWillYield)
			{
				mLockEvent.Wait();
				spinWait.Reset();
			}
			else
			{
				spinWait.SpinOnce();
			}
		}
		mLockEvent.Reset();

		[DoesNotReturn]
		static void failIndexArgumentOutOfRange() => throw new ArgumentOutOfRangeException(nameof(index), $"{nameof(index)} must be greater or equal 0 and less than 32");
	}

	public void Exit(int index)
	{
		if (index is < 0 or > 31)
		{
			failIndexArgumentOutOfRange();
		}

		var mask = 0b1u << index;

		mLockEvent.Set();
		Interlocked.And(ref mLockState, ~mask);

		[DoesNotReturn]
		static void failIndexArgumentOutOfRange() => throw new ArgumentOutOfRangeException(nameof(index), $"{nameof(index)} must be greater or equal 0 and less than 32");
	}
}
