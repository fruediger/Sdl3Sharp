using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sdl3Sharp.Internal;

internal static class SpanEnumerator
{
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static SpanEnumerator<T> GetSpanEnumerator<T>(this ReadOnlySpan<T> span) => new(span);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static SpanEnumerator<T> GetSpanEnumerator<T>(this Span<T> span) => new(span);
}

[StructLayout(LayoutKind.Sequential)]
[method: MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
internal ref struct SpanEnumerator<T>(ReadOnlySpan<T> span) : IEnumerator<T>
{
	private readonly ReadOnlySpan<T> mSpan = span;
	private int mIndex = -1;

	public readonly T Current { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mSpan[mIndex]; }

	readonly object? IEnumerator.Current { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => Current; }

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public bool MoveNext() => ++mIndex < mSpan.Length;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	readonly void IDisposable.Dispose() { }

	[DoesNotReturn]
	readonly void IEnumerator.Reset() => throw new NotSupportedException();
}
