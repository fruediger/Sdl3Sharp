using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sdl3Sharp;

/// <summary>
/// Warps a reference to a value of type <typeparamref name="T"/>
/// </summary>
/// <typeparam name="T">The type of value that the <see cref="Ref{T}"/> wraps a reference to</typeparam>
/// <param name="value">The reference to a value of type <typeparamref name="T"/> that should be wrapped</param>
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
[StructLayout(LayoutKind.Sequential)]
[method: MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
public readonly ref struct Ref<T>(ref T value) : IEquatable<Ref<T>>
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly string DebuggerDisplay => !Unsafe.IsNullRef(ref Value)
		? $"ref {{ {(Value is not null ? $"{Value}" : "null")} }}"
		: "null";

	/// <summary>The reference to a value of type <typeparamref name="T"/> that the current <see cref="Ref{T}"/> wraps</summary>
	public readonly ref T Value = ref value;

	/// <summary>Calls to this method are not supported</summary>
	/// <param name="obj">Not supported</param>
	/// <returns>Not supported</returns>
	/// <exception cref="NotSupportedException">always</exception>
	[Obsolete("Calls to this method are not supported. This method will always throw an exception. Use the Equals(Ref<T>) method or the equality operators instead.")]
	[DoesNotReturn]
#pragma warning disable CS0809
	public readonly override bool Equals([NotNullWhen(true)] object? obj) => throw new NotSupportedException("Calls to this method are not supported.");
#pragma warning restore CS0809

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly bool Equals(Ref<T> other) => Unsafe.AreSame(ref Value, ref other.Value);

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly override int GetHashCode() { unsafe { return unchecked((nint)Unsafe.AsPointer(ref Value)).GetHashCode(); } }

	/// <inheritdoc cref="IEqualityOperators{TSelf, TOther, TResult}.operator=="/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator ==(Ref<T> left, Ref<T> right) => left.Equals(right);

	/// <inheritdoc cref="IEqualityOperators{TSelf, TOther, TResult}.operator!="/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator !=(Ref<T> left, Ref<T> right) => !(left == right);
}
