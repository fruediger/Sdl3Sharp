using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sdl3Sharp.Threading;

[StructLayout(LayoutKind.Sequential)]
#pragma warning disable IDE0079
#pragma warning disable CA2231
public partial struct AtomicInt32
#pragma warning restore CA2231
#pragma warning restore IDE0079
{
#pragma warning disable IDE0044
	private int mValue;
#pragma warning restore IDE0044

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static int Add(ref AtomicInt32 atomic, int value)
	{
		unsafe
		{
			// there's not much else that we could do aside from fixing the reference,
			// as it could be pointing to a memory which is part of an object on the managed heap
			fixed(AtomicInt32* atomicPtr = &atomic)
			{
				return SDL_AddAtomicInt(atomicPtr, value);
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static int Get(ref readonly AtomicInt32 atomic)
	{
		unsafe
		{
			// there's not much else that we could do aside from fixing the reference,
			// as it could be pointing to a memory which is part of an object on the managed heap
			fixed (AtomicInt32* atomicPtr = &atomic)
			{
				return SDL_GetAtomicInt(atomicPtr);
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static int Set(ref AtomicInt32 atomic, int value)
	{
		unsafe
		{
			// there's not much else that we could do aside from fixing the reference,
			// as it could be pointing to a memory which is part of an object on the managed heap
			fixed (AtomicInt32* atomicPtr = &atomic)
			{
				return SDL_SetAtomicInt(atomicPtr, value);
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool TryCompareAndSwap(ref AtomicInt32 atomic, int oldValue, int newValue)
	{
		unsafe
		{
			// there's not much else that we could do aside from fixing the reference,
			// as it could be pointing to a memory which is part of an object on the managed heap
			fixed (AtomicInt32* atomicPtr = &atomic)
			{
				return SDL_CompareAndSwapAtomicInt(atomicPtr, oldValue, newValue);
			}
		}
	}

	/// <summary>
	/// Calls to this method are not supported
	/// </summary>
	/// <param name="obj">Not supported</param>
	/// <returns>Calls to this method are not supported</returns>
	/// <exception cref="NotSupportedException">always</exception>
	/// <remarks>
	/// Calls to this method are not supported. Check the values returned by <see cref="Get(ref readonly AtomicInt32)"/> for equality instead.
	/// </remarks>
	[DoesNotReturn]
	public readonly override bool Equals([NotNullWhen(true)] object? obj) => throw new NotSupportedException();

	/// <summary>
	/// Calls to this method are not supported
	/// </summary>
	/// <returns>Calls to this method are not supported</returns>
	/// <exception cref="NotSupportedException">always</exception>
	/// <remarks>
	/// Calls to this method are not supported. Calculate a hash code from the value returned by <see cref="Get(ref readonly AtomicInt32)"/> instead.
	/// </remarks>
	[DoesNotReturn]
	public readonly override int GetHashCode() => throw new NotSupportedException();

	/// <summary>
	/// Calls to this method are not supported
	/// </summary>
	/// <returns>Calls to this method are not supported</returns>
	/// <exception cref="NotSupportedException">always</exception>
	/// <remarks>
	/// Calls to this method are not supported. Get a string reprensentation for the value returned by <see cref="Get(ref readonly AtomicInt32)"/> instead.
	/// </remarks>
	[DoesNotReturn]
	public readonly override string ToString() => throw new NotSupportedException();
}
