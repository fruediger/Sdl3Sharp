using Sdl3Sharp.Internal;
using Sdl3Sharp.Utilities;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.InteropServices;

namespace Sdl3Sharp.Events;

partial struct Event
{
	[FieldOffset(0)] internal JoyBatteryEvent JBattery;

	/// <summary>
	/// Creates a new <see cref="Event"/> from a <see cref="JoyBatteryEvent"/>
	/// </summary>
	/// <param name="event">The <see cref="JoyBatteryEvent"/> to store into the newly created <see cref="Event"/></param>
	public Event(in JoyBatteryEvent @event) :
#pragma warning disable IDE0034 // Leave it for explicitness sake
		this(default(IUnsafeConstructorDispatch?))
#pragma warning restore IDE0034
		=> JBattery = @event;
}

/// <summary>
/// Represents an event that occurs when a joystick battery state is updated
/// </summary>
/// <remarks>
/// <para>
/// Associated <see cref="EventType"/>s:
/// <list type="bullet">
/// <item><description><see cref="EventType.JoystickBatteryUpdated"/></description></item>
/// </list>
/// </para>
/// </remarks>
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
[StructLayout(LayoutKind.Sequential)]
public struct JoyBatteryEvent : ICommonEvent<JoyBatteryEvent>, IFormattable, ISpanFormattable
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly string DebuggerDisplay => ToString(formatProvider: CultureInfo.InvariantCulture);

	private static bool Accepts(EventType type) => type is EventType.JoystickBatteryUpdated;

	static bool ICommonEvent<JoyBatteryEvent>.Accepts(EventType type) => Accepts(type);

	static ref JoyBatteryEvent ICommonEvent<JoyBatteryEvent>.GetReference(ref Event @event) => ref @event.JBattery;
	private CommonEvent mCommon;
	private uint mWhich;
	private PowerState mState;
	private int mPercent;

	/// <remarks>
	/// <para>
	/// When setting this property, the value must be <see cref="EventType.JoystickBatteryUpdated"/>.
	/// Otherwise, it will lead the property to throw an <see cref="ArgumentException"/>!
	/// </para>
	/// </remarks>
	/// <exception cref="ArgumentException">
	/// When setting this property, the value was not <see cref="EventType.JoystickBatteryUpdated"/>
	/// </exception>
	/// <inheritdoc/>
	public EventType Type
	{
		readonly get => mCommon.Type;

		set
		{
			if (!Accepts(value))
			{
				failValueArgumentIsNotValid();
			}

			mCommon.Type = value;

			[DoesNotReturn]
			static void failValueArgumentIsNotValid() => throw new ArgumentException($"The given {nameof(value)} is not a valid value for the {nameof(Type)} of a {nameof(JoyBatteryEvent)}", paramName: nameof(value));
		}
	}

	/// <inheritdoc/>
	public ulong Timestamp
	{
		readonly get => mCommon.Timestamp;
		set => mCommon.Timestamp = value;
	}

	/// <summary>
	/// Gets or sets the joystick device ID for the <see cref="Joystick"/> associated with the event
	/// </summary>
	/// <value>
	/// The joystick device ID for the <see cref="Joystick"/> associated with the event
	/// </value>
	public uint JoystickId
	{
		readonly get => mWhich;
		set => mWhich = value;
	}

	/// <summary>
	/// Gets or sets the current power state of the joystick battery
	/// </summary>
	/// <value>
	/// The current power state of the joystick battery
	/// </value>
	public PowerState State
	{
		readonly get => mState;
		set => mState = value;
	}

	/// <summary>
	/// Gets or sets the percentage of battery life remaining for the joystick
	/// </summary>
	/// <value>
	/// The percentage of battery life remaining for the joystick
	/// </value>
	public int PercentageRemaining
	{
		readonly get => mPercent;
		set => mPercent = value;
	}

	/// <inheritdoc/>
	public readonly override string ToString() => ToString(format: default, formatProvider: default);

	/// <inheritdoc cref="IFormattable.ToString(string?, IFormatProvider?)"/>
	public readonly string ToString(IFormatProvider? formatProvider) => ToString(format: default, formatProvider);

	/// <inheritdoc cref="IFormattable.ToString(string?, IFormatProvider?)"/>
	public readonly string ToString(string? format) => ToString(format, formatProvider: default);

	/// <inheritdoc/>
	public readonly string ToString(string? format, IFormatProvider? formatProvider)
	{
		var builder = Shared.StringBuilder;
		try
		{
			return ICommonEvent.PartiallyAppend(in this, builder.Append("{ "), format)
							   .Append($", {JoystickId}: ")
							   .Append(JoystickId.ToString(format, formatProvider))
							   .Append($", {State}: ")
							   .Append(State)
							   .Append($", {PercentageRemaining}: ")
							   .Append(PercentageRemaining.ToString(format, formatProvider))
							   .Append(" }")
							   .ToString();
		}
		finally
		{
			builder.Clear();
		}
	}

	/// <inheritdoc/>
	public readonly bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format = default, IFormatProvider? provider = default)
	{
		charsWritten = 0;

		return SpanFormat.TryWrite("{ ", ref destination, ref charsWritten)
			&& ICommonEvent.TryPartiallyFormat(in this, ref destination, ref charsWritten, format)
			&& SpanFormat.TryWrite($", {nameof(JoystickId)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(JoystickId, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite($", {nameof(State)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(State, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite($", {nameof(PercentageRemaining)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(PercentageRemaining, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite(" }", ref destination, ref charsWritten);
	}

	/// <inheritdoc/>
	public static implicit operator Event(in JoyBatteryEvent @event) => new(in @event);

	/// <remarks>
	/// <para>
	/// The <see cref="Event.Type"/> of the given <paramref name="event"/> must be <see cref="EventType.JoystickBatteryUpdated"/>.
	/// Otherwise, it will lead the method to throw an <see cref="ArgumentException"/>!
	/// </para>
	/// </remarks>
	/// <exception cref="ArgumentException">
	/// The <see cref="Event.Type"/> of the given <paramref name="event"/> was not <see cref="EventType.JoystickBatteryUpdated"/>
	/// </exception>
	/// <inheritdoc/>
	public static explicit operator JoyBatteryEvent(in Event @event)
	{
		if (!Accepts(@event.Type))
		{
			failEventArgumentIsNotJoyBatteryEvent();
		}

		return @event.JBattery;

		[DoesNotReturn]
		static void failEventArgumentIsNotJoyBatteryEvent() => throw new ArgumentException($"{nameof(@event)} must be a {nameof(JoyBatteryEvent)} by {nameof(Type)}", paramName: nameof(@event));
	}
}
