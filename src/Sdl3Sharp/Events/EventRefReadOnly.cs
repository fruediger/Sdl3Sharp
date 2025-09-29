using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sdl3Sharp.Events;

/// <summary>
/// Represents a reference to a readonly event structure
/// </summary>
/// <typeparam name="TEvent">The structural type of the readonly event that the <see cref="EventRefReadOnly{TEvent}"/> references</typeparam>
/// <param name="event">A reference to a readonly event structure of structural type <typeparamref name="TEvent"/></param>
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
[StructLayout(LayoutKind.Sequential)]
[method: MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
public readonly ref struct EventRefReadOnly<TEvent>(ref readonly TEvent @event) : IEquatable<EventRefReadOnly<TEvent>>
	where TEvent : struct, ICommonEvent<TEvent>
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly string DebuggerDisplay => !Unsafe.IsNullRef(in Event) ? $"{{ {nameof(Event)}: {Event} }}" : "null";

	/// <summary>The reference to the readonly event structure of structural type <typeparamref name="TEvent"/> that the current <see cref="EventRefReadOnly{TEvent}"/> represents</summary>
	public readonly ref readonly TEvent Event = ref @event;

	public readonly EventType<TEvent> Type
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => Event.Type;
	}

	public readonly ulong Timestamp
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => Event.Timestamp;
	}

	/// <summary>Calls to this method are not supported</summary>
	/// <param name="obj">Not supported</param>
	/// <returns>Not supported</returns>
	/// <exception cref="NotSupportedException">always</exception>
	[Obsolete($"Calls to this method are not supported. This method will always throw an exception. Use the {nameof(Equals)}({nameof(EventRefReadOnly<>)}<{nameof(TEvent)}>) method or the equality operators instead.")]
	[DoesNotReturn]
#pragma warning disable CS0809
	public readonly override bool Equals([NotNullWhen(true)] object? obj) => throw new NotSupportedException("Calls to this method are not supported.");
#pragma warning restore CS0809

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly bool Equals(EventRefReadOnly<TEvent> other) => Unsafe.AreSame(in Event, in other.Event);

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public override int GetHashCode() { unsafe { return unchecked((nint)Unsafe.AsPointer(ref Unsafe.AsRef(in Event))).GetHashCode(); } }

	/// <inheritdoc cref="IEqualityOperators{TSelf, TOther, TResult}.operator=="/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator ==(EventRefReadOnly<TEvent> left, EventRefReadOnly<TEvent> right) => left.Equals(right);

	/// <inheritdoc cref="IEqualityOperators{TSelf, TOther, TResult}.operator!="/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator !=(EventRefReadOnly<TEvent> left, EventRefReadOnly<TEvent> right) => !(left == right);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static implicit operator EventRefReadOnly<TEvent>(EventRef<TEvent> eventRef) => new(ref eventRef.Event);
}
