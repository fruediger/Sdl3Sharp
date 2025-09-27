using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sdl3Sharp.Events;

[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
[StructLayout(LayoutKind.Sequential)]
public readonly struct EventType<TEvent> : IEventType,
	IComparable, IComparable<EventType>, IComparable<EventType<TEvent>>, IEquatable<EventType>, IEquatable<EventType<TEvent>>, IFormattable, ISpanFormattable,
	IComparisonOperators<EventType<TEvent>, EventType, bool>, IComparisonOperators<EventType<TEvent>, EventType<TEvent>, bool>, IEqualityOperators<EventType<TEvent>, EventType, bool>, IEqualityOperators<EventType<TEvent>, EventType<TEvent>, bool>
	where TEvent : struct, ICommonEvent<TEvent>
{
	private readonly EventType mBase;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly string DebuggerDisplay => ToString(formatProvider: CultureInfo.InvariantCulture);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	internal EventType(EventType.Kind kind) => mBase = new(kind);

	readonly EventType IEventType.Base { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mBase; }

	public readonly bool Enabled
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mBase.Enabled;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mBase.Enabled = value;
	}

	public readonly int CompareTo(object? obj)
	{
		return obj switch
		{
			null => 1,
			EventType other => CompareTo(other),
			EventType<TEvent> other => CompareTo(other),
			IEventType other => CompareTo(other),
			_ => failObjArgumentIsNotEventType()
		};

		[DoesNotReturn]
		static int failObjArgumentIsNotEventType() => throw new ArgumentException($"{nameof(obj)} is not of type {nameof(EventType)} or {nameof(EventType<>)}<{nameof(TEvent)}>", nameof(obj));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly int CompareTo(EventType other) => mBase.CompareTo(other);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly int CompareTo(EventType<TEvent> other) => mBase.CompareTo(other.mBase);

	internal readonly int CompareTo(IEventType? other) => other switch
	{
		{ Base: var @base } => CompareTo(@base),
		_ /* null */ => 1,
	};	

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	readonly int IComparable<IEventType>.CompareTo(IEventType? other) => CompareTo(other);

	public readonly override bool Equals([NotNullWhen(true)] object? obj) => obj switch
	{
		EventType other => Equals(other),
		EventType<TEvent> other => Equals(other),
		IEventType other => Equals(other),
		_ => false
	};

	public readonly bool Equals(EventType other) => mBase.Equals(other);

	public readonly bool Equals(EventType<TEvent> other) => mBase.Equals(other.mBase);

	internal readonly bool Equals([NotNullWhen(true)] IEventType? other) => other switch
	{
		{ Base: var @base } => Equals(@base),
		_ /* null */ => false
	};

	readonly bool IEquatable<IEventType>.Equals([NotNullWhen(true)] IEventType? other) => Equals(other);

	public readonly override int GetHashCode() => mBase.GetHashCode();

	/// <inheritdoc/>
	public readonly override string ToString() => mBase.ToString();

	/// <inheritdoc cref="IFormattable.ToString(string?, IFormatProvider?)"/>
	public readonly string ToString(IFormatProvider? formatProvider) => mBase.ToString(formatProvider);

	/// <inheritdoc cref="IFormattable.ToString(string?, IFormatProvider?)"/>
	public readonly string ToString(string? format) => mBase.ToString(format);

	/// <inheritdoc/>
	public readonly string ToString(string? format, IFormatProvider? formatProvider) => mBase.ToString(format, formatProvider);

	/// <inheritdoc/>
	public readonly bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format = default, IFormatProvider? provider = default)
		=> mBase.TryFormat(destination, out charsWritten, format, provider);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator >(EventType<TEvent> left, EventType right) => left.mBase > right;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator >(EventType<TEvent> left, EventType<TEvent> right) => left.mBase > right.mBase;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator >=(EventType<TEvent> left, EventType right) => left.mBase >= right;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator >=(EventType<TEvent> left, EventType<TEvent> right) => left.mBase >= right.mBase;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator <(EventType<TEvent> left, EventType right) => left.mBase < right;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator <(EventType<TEvent> left, EventType<TEvent> right) => left.mBase < right.mBase;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator <=(EventType<TEvent> left, EventType right) => left.mBase <= right;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator <=(EventType<TEvent> left, EventType<TEvent> right) => left.mBase <= right.mBase;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator ==(EventType<TEvent> left, EventType right) => left.mBase == right;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator ==(EventType<TEvent> left, EventType<TEvent> right) => left.mBase == right.mBase;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator !=(EventType<TEvent> left, EventType right) => left.mBase != right;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator !=(EventType<TEvent> left, EventType<TEvent> right) => left.mBase != right.mBase;	

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static implicit operator EventType(EventType<TEvent> type) => type.mBase;

	/// <summary>
	/// Converts an <see cref="EventType"/> to an <see cref="EventType{TEvent}"/>
	/// </summary>
	/// <param name="type">The <see cref="EventType"/> value to convert to an <see cref="EventType{TEvent}"/></param>
	/// <remarks>
	/// <para>
	/// If the <paramref name="type"/> argument cannot be converted to an <see cref="EventType{TEvent}"/> because it's kind doesn't reflect an event type of <typeparamref name="TEvent"/>,
	/// then an <see cref="ArgumentException"/> will be thrown.
	/// </para>
	/// </remarks>
	/// <exception cref="ArgumentException"><paramref name="type"/> cannot be converted to <see cref="EventType{TEvent}"/></exception>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static explicit operator EventType<TEvent>(EventType type)
	{
		if (!TEvent.Accepts(type))
		{
			failTypeArgumentNotConvertable();
		}

		return Unsafe.BitCast<EventType, EventType<TEvent>>(type);

		[DoesNotReturn]
		static void failTypeArgumentNotConvertable() => throw new ArgumentException($"{nameof(type)} cannot be converted to {nameof(EventType<>)}<{typeof(TEvent).Name}>", nameof(type));
	}
}
