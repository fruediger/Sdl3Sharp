using System;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Threading;

public static class AtomicExtensions
{
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static int Add(this ref AtomicInt32 atomic, int value) => AtomicInt32.Add(ref atomic, value);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static int Get(this ref readonly AtomicInt32 atomic) => AtomicInt32.Get(in atomic);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static IntPtr Get(this ref readonly AtomicIntPtr atomic) => AtomicIntPtr.Get(in atomic);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static uint Get(this ref readonly AtomicUInt32 atomic) => AtomicUInt32.Get(in atomic);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static int Set(this ref AtomicInt32 atomic, int value) => AtomicInt32.Set(ref atomic, value);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static IntPtr Set(this ref AtomicIntPtr atomic, IntPtr value) => AtomicIntPtr.Set(ref atomic, value);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static uint Set(this ref AtomicUInt32 atomic, uint value) => AtomicUInt32.Set(ref atomic, value);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool TryCompareAndSwap(this ref AtomicInt32 atomic, int oldValue, int newValue) => AtomicInt32.TryCompareAndSwap(ref atomic, oldValue, newValue);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool TryCompareAndSwap(this ref AtomicIntPtr atomic, IntPtr oldValue, IntPtr newValue) => AtomicIntPtr.TryCompareAndSwap(ref atomic, oldValue, newValue);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool TryCompareAndSwap(this ref AtomicUInt32 atomic, uint oldValue, uint newValue) => AtomicUInt32.TryCompareAndSwap(ref atomic, oldValue, newValue);
}
