using Sdl3Sharp.Internal;
using Sdl3Sharp.Timing;
using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace Sdl3Sharp.Events;

/// <summary>
/// A commonly shared interface, implemented by all of SDL's *Event structures
/// </summary>
/// <remarks>
/// <para>
/// This interface is usually implemented by implementing <see cref="ICommonEvent{TSelf}"/> (which extends this interface) instead.
/// </para>
/// </remarks>
public interface ICommonEvent
{
	/// <summary>
	/// Gets or sets the <see cref="EventType">type</see> of the current event
	/// </summary>
	/// <value>
	/// The <see cref="EventType">type</see> of the current event
	/// </value>
	EventType Type { get; set; }

	/// <summary>
	/// Gets or sets the timestamp of the current event
	/// </summary>
	/// <value>
	/// The timestamp of the current event
	/// </value>
	/// <remarks>
	/// <para>
	/// The value of this property usually describes the time passed, in nanoseconds, since the <see cref="Sdl(Sdl.BuildAction?)">initialization of SDL</see>.
	/// It can be properly populated by using <see cref="Timer.NanosecondTicks"/>.
	/// </para>
	/// </remarks>
	ulong Timestamp { get; set; }

	private static string FormatTimestamp(ulong timestamp)
	{
		var builder = Shared.StringBuilder;

		try
		{
			(timestamp, var ns) = Math.DivRem(timestamp, 1_000_000);

			if (timestamp is > 0)
			{
				(timestamp, var ms) = Math.DivRem(timestamp, 1_000);

				if (timestamp is > 0)
				{
					(timestamp, var s) = Math.DivRem(timestamp, 60);

					if (timestamp is > 0)
					{
						(timestamp, var min) = Math.DivRem(timestamp, 60);

						if (timestamp is > 0)
						{
							builder.Append(timestamp /* (hours) */).Append("h ");
						}

						builder.Append(min).Append("min ");
					}

					builder.Append(s).Append("s ");
				}

				builder.Append(ms).Append("ms ");
			}

			builder.Append(ns).Append("ns");

			return builder.ToString();
		}
		finally
		{
			builder.Clear();
		}
	}

	private static bool TryFormatTimestamp(ulong timestamp, ref Span<char> destination, ref int charsWritten)
	{
		(timestamp, var ns) = Math.DivRem(timestamp, 1_000_000);

		if (timestamp is > 0)
		{
			(timestamp, var ms) = Math.DivRem(timestamp, 1_000);

			if (timestamp is > 0)
			{
				(timestamp, var s) = Math.DivRem(timestamp, 60);

				if (timestamp is > 0)
				{
					(timestamp, var min) = Math.DivRem(timestamp, 60);

					if (timestamp is > 0)
					{
						if ( !(SpanFormat.TryWrite(timestamp /* (hours) */, ref destination, ref charsWritten)
							&& SpanFormat.TryWrite("h ", ref destination, ref charsWritten)))
						{
							return false;
						}
					}

					if ( !(SpanFormat.TryWrite(min, ref destination, ref charsWritten)
						&& SpanFormat.TryWrite("min ", ref destination, ref charsWritten)))
					{
						return false;
					}
				}

				if ( !(SpanFormat.TryWrite(s, ref destination, ref charsWritten)
					&& SpanFormat.TryWrite("s ", ref destination, ref charsWritten)))
				{
					return false;
				}
			}

			if ( !(SpanFormat.TryWrite(ms, ref destination, ref charsWritten)
				&& SpanFormat.TryWrite("ms ", ref destination, ref charsWritten)))
			{
				return false;
			}
		}

		return SpanFormat.TryWrite(ns, ref destination, ref charsWritten)
			&& SpanFormat.TryWrite("ns", ref destination, ref charsWritten);
	}

	internal static StringBuilder PartialAppend<TEvent>(StringBuilder builder, ref readonly TEvent @event, string? format, IFormatProvider? formatProvider)
		where TEvent : struct, ICommonEvent
		=> builder.Append($"{nameof(Type)}: ")
			.Append(@event.Type.ToString(format, formatProvider))
			.Append($", {nameof(Timestamp)}: ")
			.Append(FormatTimestamp(@event.Timestamp));

	internal static string PartialToString<TEvent>(ref readonly TEvent @event, string? format, IFormatProvider? formatProvider)
		where TEvent : struct, ICommonEvent
		=> $"{nameof(Type)}: {@event.Type.ToString(format, formatProvider)}, {nameof(Timestamp)}: {FormatTimestamp(@event.Timestamp)}";

	internal static bool TryPartialFormat<TEvent>(ref readonly TEvent @event, ref Span<char> destination, ref int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
		where TEvent : struct, ICommonEvent
		=> SpanFormat.TryWrite($"{nameof(Type)}: ", ref destination, ref charsWritten)
		&& SpanFormat.TryWrite(@event.Type, ref destination, ref charsWritten, format, provider)
		&& SpanFormat.TryWrite($", {nameof(Timestamp)}: ", ref destination, ref charsWritten)
		&& TryFormatTimestamp(@event.Timestamp, ref destination, ref charsWritten);
}

/// <summary>
/// A commonly shared interface, implemented by all *Event structures
/// </summary>
/// <typeparam name="TSelf">The type of event structure that implements this interface itself</typeparam>
public interface ICommonEvent<TSelf> : ICommonEvent
	where TSelf : struct, ICommonEvent<TSelf>
{
	/// <summary>
	/// Gets or sets the <see cref="EventType{TEvent}">type</see> of the current event
	/// </summary>
	/// <value>
	/// The <see cref="EventType{TEvent}">type</see> of the current event
	/// </value>
	new EventType<TSelf> Type { get; set; }

	/// <inheritdoc/>
	/// <inheritdoc cref="EventType{TEvent}.explicit operator EventType{TEvent}(EventType)"/>
	EventType ICommonEvent.Type
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => Type;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => Type = (EventType<TSelf>)value;
	}

	internal static abstract bool Accepts(EventType type);

	internal static abstract ref TSelf GetReference(ref Event @event);

	/// <summary>
	/// Converts an event of type <typeparamref name="TSelf"/> to a general <see cref="Event"/>
	/// </summary>
	/// <param name="event">The event of type <typeparamref name="TSelf"/> to convert to a general <see cref="Event"/></param>
	/// <remarks>
	/// <para>
	/// This conversion usually requires the <paramref name="event"/> to be copied into a new general <see cref="Event"/> structure. Note: This can impact performance!
	/// </para>
	/// </remarks>
	static abstract implicit operator Event(in TSelf @event);

	/// <summary>
	/// Converts a general <see cref="Event"/> to an event of type <typeparamref name="TSelf"/>
	/// </summary>
	/// <param name="event">The general <see cref="Event"/> to convert to an event of type <typeparamref name="TSelf"/></param>
	/// <remarks>
	/// <para>
	/// This conversion usually requires that a certain part of the general <see cref="Event"/> structure given by <paramref name="event"/>, which represents the event of type <typeparamref name="TSelf"/>, to be copied from it.
	/// Note: This can impact performance!
	/// </para>
	/// </remarks>
	static abstract explicit operator TSelf(in Event @event);
}
