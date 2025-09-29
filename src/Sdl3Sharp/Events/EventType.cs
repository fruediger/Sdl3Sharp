using Sdl3Sharp.Internal;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sdl3Sharp.Events;

/// <summary>
/// Represents an event type for an <see cref="Event"/>
/// </summary>
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
[StructLayout(LayoutKind.Sequential)]
public readonly partial struct EventType : IEventType,
	IComparable, IComparable<EventType>, IEquatable<EventType>, IFormattable, ISpanFormattable, IComparisonOperators<EventType, EventType, bool>, IEqualityOperators<EventType, EventType, bool>
{
	private readonly Kind mKind;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly string DebuggerDisplay => ToString(formatProvider: CultureInfo.InvariantCulture);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	internal EventType(Kind kind) => mKind = kind;

	/// <summary>
	/// Tries to register new user defined events
	/// </summary>
	/// <param name="count">The number of user defined events that should be registered. Must be greater than <c>0</c>.</param>
	/// <param name="eventTypes">A enumeration of <see cref="EventType"/>s of length <paramref name="count"/> which represent the newly registered user defined events, when this mehtod returns <c><see langword="true"/></c>; otherwise <c><see langword="default"/>(<see cref="Enumerable"/>)</c> (has length <c>0</c>)</param>
	/// <returns><c><see langword="true"/></c> if the requested amount of user defined events was successfully registered; otherwise, <c><see langword="false"/></c> (<paramref name="count"/> might be invalid or there weren't enough free user defined <see cref="EventType"/>s left to register <paramref name="count"/> of them)</returns>
	/// <remarks>
	/// Enumerate the resulting <paramref name="eventTypes"/> to get the newly registered user defined event types.
	/// </remarks>
	public static bool TryRegister(int count, out Enumerable eventTypes)
	{
		var start = SDL_RegisterEvents(count);

		if (start.mKind is 0)
		{
			eventTypes = default;
			return false;
		}

		eventTypes = new(start, unchecked((uint)count));
		return true;
	}

	readonly EventType IEventType.Base { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => this; }

	/// <summary>
	/// Gets or sets a value determining whether the current <see cref="EventType"/> is enabled within SDL's event system or not
	/// </summary>
	/// <value>
	/// A value determining whether the current <see cref="EventType"/> is enabled within SDL's event system or not
	/// </value>
	/// <remarks>
	/// <see cref="EventType"/>s whose <see cref="Enabled"/> property is set to <c><see langword="false"/></c> won't get processed by SDL.
	/// </remarks>
	public readonly bool Enabled
	{
		get => SDL_EventEnabled(this);
		set => SDL_SetEventEnabled(this, value);
	}

	/// <inheritdoc/>
	public int CompareTo(object? obj)
	{
		return obj switch
		{
			null => 1,
			EventType other => CompareTo(other),
			IEventType other => CompareTo(other),
			_ => failObjArgumentIsNotEventType()
		};

		[DoesNotReturn]
		static int failObjArgumentIsNotEventType() => throw new ArgumentException($"{nameof(obj)} is not of type {nameof(EventType)}", nameof(obj));
	}

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly int CompareTo(EventType other) => unchecked((uint)mKind).CompareTo(unchecked((uint)other.mKind));	

	/// <inheritdoc cref="IComparable{IEventType}.CompareTo(IEventType)"/>
	internal readonly int CompareTo(IEventType? other) => other switch
	{
		{ Base: var @base } => CompareTo(@base),
		_ /* null */ => 1,
	};	

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	readonly int IComparable<IEventType>.CompareTo(IEventType? other) => CompareTo(other);

	/// <inheritdoc/>
	public readonly override bool Equals([NotNullWhen(true)] object? obj) => obj switch
	{
		EventType other => Equals(other),
		IEventType other => Equals(other),
		_ => false
	};

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly bool Equals(EventType other) => mKind == other.mKind;	

	/// <inheritdoc cref="IEquatable{IEventType}.Equals(IEventType)"/>
	internal readonly bool Equals([NotNullWhen(true)] IEventType? other) => other switch
	{
		{ Base: var @base } => Equals(@base),
		_ /* null */ => false,
	};

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	readonly bool IEquatable<IEventType>.Equals([NotNullWhen(true)] IEventType? other) => Equals(other);

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly override int GetHashCode() => mKind.GetHashCode();

	/// <inheritdoc/>
	public readonly override string ToString() => ToString(format: default, formatProvider: default);

	/// <inheritdoc cref="IFormattable.ToString(string?, IFormatProvider?)"/>
	public readonly string ToString(IFormatProvider? formatProvider) => ToString(format: default, formatProvider);

	/// <inheritdoc cref="IFormattable.ToString(string?, IFormatProvider?)"/>
	public readonly string ToString(string? format) => ToString(format, formatProvider: default);

	/// <inheritdoc/>
	public readonly string ToString(string? format, IFormatProvider? formatProvider) => mKind switch
	{
		>= Kind.User and <= Kind.Last => $"{nameof(User)}({unchecked(mKind - Kind.User).ToString(format, formatProvider)})",

		_ => KnownKindToString(mKind) switch
		{
			string knonwKind => knonwKind,

			_ => mKind.ToString(format)
		}
	};

	/// <inheritdoc/>
	public readonly bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format = default, IFormatProvider? provider = default)
	{
		charsWritten = 0;

		return mKind switch
		{
			>= Kind.User and < Kind.Last
				=> SpanFormat.TryWrite($"{nameof(User)}(", ref destination, ref charsWritten)
				&& SpanFormat.TryWrite(unchecked(mKind - Kind.User), ref destination, ref charsWritten, format, provider)
				&& SpanFormat.TryWrite(')', ref destination, ref charsWritten),

			_ => KnownKindToString(mKind) switch
			{
				string knownKind => SpanFormat.TryWrite(knownKind, ref destination, ref charsWritten),

				_ => SpanFormat.TryWrite(mKind, ref destination, ref charsWritten, format)
			}
		};
	}

	/// <summary>
	/// Tries to get the user value used to identify this user event type
	/// </summary>
	/// <param name="userValue">The user value used to identify this user event type</param>
	/// <returns><c><see langword="true"/></c> if this instance represents a user event type and <paramref name="userValue"/> is the user value which identifies it; otherwise, <c><see langword="false"/></c></returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly bool TryGetUserValue(out int userValue)
	{
		if (mKind is >= Kind.User and <= Kind.Last)
		{
			userValue = unchecked((int)(mKind - Kind.User));

			return true;
		}

		userValue = default;

		return false;
	}

	/// <inheritdoc/>
	public static bool operator >(EventType left, EventType right) => left.mKind > right.mKind;

	/// <inheritdoc/>
	public static bool operator >=(EventType left, EventType right) => left.mKind >= right.mKind;

	/// <inheritdoc/>
	public static bool operator <(EventType left, EventType right) => left.mKind < right.mKind;

	/// <inheritdoc/>
	public static bool operator <=(EventType left, EventType right) => left.mKind <= right.mKind;

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator ==(EventType left, EventType right) => left.mKind == right.mKind;

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator !=(EventType left, EventType right) => right.mKind != left.mKind;
}
