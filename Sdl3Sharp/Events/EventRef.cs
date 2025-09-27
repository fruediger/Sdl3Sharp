using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sdl3Sharp.Events;

/// <summary>
/// Represents a reference to an event structure
/// </summary>
/// <typeparam name="TEvent">The structural type of the event that the <see cref="EventRef{TEvent}"/> references</typeparam>
/// <param name="event">A reference to an event structure of structural type <typeparamref name="TEvent"/></param>
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
[StructLayout(LayoutKind.Sequential)]
[method: MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
public readonly ref struct EventRef<TEvent>(ref TEvent @event) : IEquatable<EventRef<TEvent>>
	where TEvent : struct, ICommonEvent<TEvent>
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly string DebuggerDisplay => !Unsafe.IsNullRef(ref Event) ? $"{{ {nameof(Event)}: {Event} }}" : "null";

	/// <summary>The reference to the event structure of structural type <typeparamref name="TEvent"/> that the current <see cref="EventRef{TEvent}"/> represents</summary>
	public readonly ref TEvent Event = ref @event;

	public EventType<TEvent> Type
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => Event.Type;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => Event.Type = value;
	}

	public ulong Timestamp
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => Event.Timestamp;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => Event.Timestamp = value;
	}

	/// <summary>Calls to this method are not supported</summary>
	/// <param name="obj">Not supported</param>
	/// <returns>Not supported</returns>
	/// <exception cref="NotSupportedException">always</exception>
	[Obsolete($"Calls to this method are not supported. This method will always throw an exception. Use the {nameof(Equals)}({nameof(EventRef<>)}<{nameof(TEvent)}>) method or the equality operators instead.")]
	[DoesNotReturn]
#pragma warning disable CS0809
	public readonly override bool Equals([NotNullWhen(true)] object? obj) => throw new NotSupportedException("Calls to this method are not supported.");
#pragma warning restore CS0809

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly bool Equals(EventRef<TEvent> other) => Unsafe.AreSame(ref Event, ref other.Event);

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public override int GetHashCode() { unsafe { return unchecked((nint)Unsafe.AsPointer(ref Event)).GetHashCode(); } }

	/// <inheritdoc cref="IEqualityOperators{TSelf, TOther, TResult}.operator=="/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator ==(EventRef<TEvent> left, EventRef<TEvent> right) => left.Equals(right);

	/// <inheritdoc cref="IEqualityOperators{TSelf, TOther, TResult}.operator!="/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator !=(EventRef<TEvent> left, EventRef<TEvent> right) => !(left == right);
}
