using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sdl3Sharp.Threading;

[StructLayout(LayoutKind.Sequential)]
#pragma warning disable IDE0079
#pragma warning disable CA2231
public partial struct AtomicIntPtr
#pragma warning restore CA2231
#pragma warning restore IDE0079
{
#pragma warning disable IDE0044
	private IntPtr mValue;
#pragma warning restore IDE0044

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static IntPtr Get(ref readonly AtomicIntPtr atomic)
	{
		unsafe
		{
			// there's not much else that we could do aside from fixing the reference,
			// as it could be pointing to a memory which is part of an object on the managed heap
			fixed (AtomicIntPtr* atomicPtr = &atomic)
			{
				return unchecked((IntPtr)SDL_GetAtomicPointer(unchecked((void**)atomicPtr)));
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static IntPtr Set(ref AtomicIntPtr atomic, IntPtr value)
	{
		unsafe
		{
			// there's not much else that we could do aside from fixing the reference,
			// as it could be pointing to a memory which is part of an object on the managed heap
			fixed (AtomicIntPtr* atomicPtr = &atomic)
			{
				return unchecked((IntPtr)SDL_SetAtomicPointer(unchecked((void**)atomicPtr), unchecked((void*)value)));
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool TryCompareAndSwap(ref AtomicIntPtr atomic, IntPtr oldValue, IntPtr newValue)
	{
		unsafe
		{
			// there's not much else that we could do aside from fixing the reference,
			// as it could be pointing to a memory which is part of an object on the managed heap
			fixed (AtomicIntPtr* atomicPtr = &atomic)
			{
				return SDL_CompareAndSwapAtomicPointer(unchecked((void**)atomicPtr), unchecked((void*)oldValue), unchecked((void*)newValue));
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
	/// Calls to this method are not supported. Check the values returned by <see cref="Get(ref readonly AtomicIntPtr)"/> for equality instead.
	/// </remarks>
	[DoesNotReturn]
	public readonly override bool Equals([NotNullWhen(true)] object? obj) => throw new NotSupportedException();

	/// <summary>
	/// Calls to this method are not supported
	/// </summary>
	/// <returns>Calls to this method are not supported</returns>
	/// <exception cref="NotSupportedException">always</exception>
	/// <remarks>
	/// Calls to this method are not supported. Calculate a hash code from the value returned by <see cref="Get(ref readonly AtomicIntPtr)"/> instead.
	/// </remarks>
	[DoesNotReturn]
	public readonly override int GetHashCode() => throw new NotSupportedException();

	/// <summary>
	/// Calls to this method are not supported
	/// </summary>
	/// <returns>Calls to this method are not supported</returns>
	/// <exception cref="NotSupportedException">always</exception>
	/// <remarks>
	/// Calls to this method are not supported. Get a string reprensentation for the value returned by <see cref="Get(ref readonly AtomicIntPtr)"/> instead.
	/// </remarks>
	[DoesNotReturn]
	public readonly override string ToString() => throw new NotSupportedException();
}
