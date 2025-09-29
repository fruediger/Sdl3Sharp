using System.Diagnostics.CodeAnalysis;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sdl3Sharp.Threading;

[StructLayout(LayoutKind.Sequential)]
#pragma warning disable IDE0079
#pragma warning disable CA2231
public partial struct SpinLock
#pragma warning restore CA2231
#pragma warning restore IDE0079
{
#pragma warning disable IDE0044
	private int mValue;
#pragma warning restore IDE0044

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void Lock(ref SpinLock spinLock)
	{
		unsafe
		{
			// there's not much else that we could do aside from fixing the reference,
			// as it could be pointing to a memory which is part of an object on the managed heap
			fixed (SpinLock* spinLockPtr = &spinLock)
			{
				SDL_LockSpinlock(spinLockPtr);
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool TryLock(ref SpinLock spinLock)
	{
		unsafe
		{
			// there's not much else that we could do aside from fixing the reference,
			// as it could be pointing to a memory which is part of an object on the managed heap
			fixed (SpinLock* spinLockPtr = &spinLock)
			{
				return SDL_TryLockSpinlock(spinLockPtr);
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void Unlock(ref SpinLock spinLock)
	{
		unsafe
		{
			// there's not much else that we could do aside from fixing the reference,
			// as it could be pointing to a memory which is part of an object on the managed heap
			fixed (SpinLock* spinlockPtr = &spinLock)
			{
				SDL_UnlockSpinlock(spinlockPtr);
			}
		}
	}

	/// <summary>
	/// Calls to this method are not supported
	/// </summary>
	/// <param name="obj">Not supported</param>
	/// <returns>Calls to this method are not supported</returns>
	/// <exception cref="NotSupportedException">always</exception>
	/// <remarks>Calls to this method are not supported</remarks>
	[DoesNotReturn]
	public readonly override bool Equals([NotNullWhen(true)] object? obj) => throw new NotSupportedException();

	/// <summary>
	/// Calls to this method are not supported
	/// </summary>
	/// <returns>Calls to this method are not supported</returns>
	/// <exception cref="NotSupportedException">always</exception>
	/// <remarks>Calls to this method are not supported</remarks>
	[DoesNotReturn]
	public readonly override int GetHashCode() => throw new NotSupportedException();

	/// <summary>
	/// Calls to this method are not supported
	/// </summary>
	/// <returns>Calls to this method are not supported</returns>
	/// <exception cref="NotSupportedException">always</exception>
	/// <remarks>Calls to this method are not supported</remarks>
	[DoesNotReturn]
	public readonly override string ToString() => throw new NotSupportedException();
}
