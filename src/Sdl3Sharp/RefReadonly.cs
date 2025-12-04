using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sdl3Sharp;

/// <summary>
/// Warps a readonly reference to a value of type <typeparamref name="T"/>
/// </summary>
/// <typeparam name="T">The type of value that the <see cref="RefReadonly{T}"/> wraps a readonly reference to</typeparam>
/// <param name="value">The readonly reference to a value of type <typeparamref name="T"/> that should be wrapped</param>
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
[StructLayout(LayoutKind.Sequential)]
[method: MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
internal readonly ref struct RefReadonly<T>(ref readonly T value) : IEquatable<RefReadonly<T>>
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly string DebuggerDisplay => !Unsafe.IsNullRef(in Value)
		? $"ref {{ {(Value is not null ? $"{Value}" : "null")} }}"
		: "null";

	/// <summary>The readonly reference to a value of type <typeparamref name="T"/> that the current <see cref="RefReadonly{T}"/> wraps</summary>
	public readonly ref readonly T Value = ref value;

	/// <summary>Calls to this method are not supported</summary>
	/// <param name="obj">Not supported</param>
	/// <returns>Not supported</returns>
	/// <exception cref="NotSupportedException">always</exception>
	[Obsolete("Calls to this method are not supported. This method will always throw an exception. Use the Equals(RefReadonly<T>) method or the equality operators instead.")]
	[DoesNotReturn]
#pragma warning disable CS0809
	public readonly override bool Equals([NotNullWhen(true)] object? obj) => throw new NotSupportedException("Calls to this method are not supported.");
#pragma warning restore CS0809

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly bool Equals(RefReadonly<T> other) => Unsafe.AreSame(in Value, in other.Value);

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly override int GetHashCode() { unsafe { return unchecked((nint)Unsafe.AsPointer(ref Unsafe.AsRef(in Value))).GetHashCode(); } }

	/// <inheritdoc cref="IEqualityOperators{TSelf, TOther, TResult}.operator=="/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator ==(RefReadonly<T> left, RefReadonly<T> right) => left.Equals(right);

	/// <inheritdoc cref="IEqualityOperators{TSelf, TOther, TResult}.operator!="/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator !=(RefReadonly<T> left, RefReadonly<T> right) => !(left == right);

	/// <summary>
	/// Converts a <see cref="Ref{T}"/> to a <see cref="RefReadonly{T}"/>
	/// </summary>
	/// <param name="ref">The <see cref="Ref{T}"/> to get converted to a <see cref="RefReadonly{T}"/></param>
	/// <remarks>
	/// <para>
	/// Converting random-access references to readonly ones should be always safe.
	/// </para>
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static implicit operator RefReadonly<T>(Ref<T> @ref) => new(ref @ref.Value);
}
