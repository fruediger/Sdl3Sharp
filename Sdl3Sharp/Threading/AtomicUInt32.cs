using System.Diagnostics.CodeAnalysis;
using System;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Threading;

[StructLayout(LayoutKind.Sequential)]
#pragma warning disable IDE0079
#pragma warning disable CA2231
public partial struct AtomicUInt32
#pragma warning restore CA2231
#pragma warning restore IDE0079
{
#pragma warning disable IDE0044
	private uint mValue;
#pragma warning restore IDE0044

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static uint Get(ref readonly AtomicUInt32 atomic)
	{
		unsafe
		{
			// there's not much else that we could do aside from fixing the reference,
			// as it could be pointing to a memory which is part of an object on the managed heap
			fixed (AtomicUInt32* atomicPtr = &atomic)
			{
				return SDL_GetAtomicU32(atomicPtr);
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static uint Set(ref AtomicUInt32 atomic, uint value)
	{
		unsafe
		{
			// there's not much else that we could do aside from fixing the reference,
			// as it could be pointing to a memory which is part of an object on the managed heap
			fixed (AtomicUInt32* atomicPtr = &atomic)
			{
				return SDL_SetAtomicU32(atomicPtr, value);
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool TryCompareAndSwap(ref AtomicUInt32 atomic, uint oldValue, uint newValue)
	{
		unsafe
		{
			// there's not much else that we could do aside from fixing the reference,
			// as it could be pointing to a memory which is part of an object on the managed heap
			fixed (AtomicUInt32* atomicPtr = &atomic)
			{
				return SDL_CompareAndSwapAtomicU32(atomicPtr, oldValue, newValue);
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
	/// Calls to this method are not supported. Check the values returned by <see cref="Get(ref readonly AtomicUInt32)"/> for equality instead.
	/// </remarks>
	[DoesNotReturn]
	public readonly override bool Equals([NotNullWhen(true)] object? obj) => throw new NotSupportedException();

	/// <summary>
	/// Calls to this method are not supported
	/// </summary>
	/// <returns>Calls to this method are not supported</returns>
	/// <exception cref="NotSupportedException">always</exception>
	/// <remarks>
	/// Calls to this method are not supported. Calculate a hash code from the value returned by <see cref="Get(ref readonly AtomicUInt32)"/> instead.
	/// </remarks>
	[DoesNotReturn]
	public readonly override int GetHashCode() => throw new NotSupportedException();

	/// <summary>
	/// Calls to this method are not supported
	/// </summary>
	/// <returns>Calls to this method are not supported</returns>
	/// <exception cref="NotSupportedException">always</exception>
	/// <remarks>
	/// Calls to this method are not supported. Get a string reprensentation for the value returned by <see cref="Get(ref readonly AtomicUInt32)"/> instead.
	/// </remarks>
	[DoesNotReturn]
	public readonly override string ToString() => throw new NotSupportedException();
}
