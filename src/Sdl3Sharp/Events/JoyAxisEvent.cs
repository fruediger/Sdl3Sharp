using Sdl3Sharp.Internal;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sdl3Sharp.Events;

partial struct Event
{
	[FieldOffset(0)] internal JoyAxisEvent JAxis;

	/// <summary>
	/// Creates a new <see cref="Event"/> from a <see cref="JoyAxisEvent"/>
	/// </summary>
	/// <param name="event">The <see cref="JoyAxisEvent"/> to store into the newly created <see cref="Event"/></param>
	public Event(in JoyAxisEvent @event) :
#pragma warning disable IDE0034 // Leave it for explicitness sake
		this(default(IUnsafeConstructorDispatch?))
#pragma warning restore IDE0034
		=> JAxis = @event;
}

/// <summary>
/// Represents an event that occurs when a joystick axis changes position
/// </summary>
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
[StructLayout(LayoutKind.Sequential)]
public struct JoyAxisEvent : ICommonEvent<JoyAxisEvent>, IFormattable, ISpanFormattable
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly string DebuggerDisplay => ToString(formatProvider: CultureInfo.InvariantCulture);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private static bool Accepts(EventType type) => type is EventType.JoystickAxisMotion;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static bool ICommonEvent<JoyAxisEvent>.Accepts(EventType type) => Accepts(type);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static ref JoyAxisEvent ICommonEvent<JoyAxisEvent>.GetReference(ref Event @event) => ref @event.JAxis;

	private CommonEvent mCommon;
	private uint mWhich;
	private byte mAxis;
	private readonly byte mPadding1, mPadding2, mPadding3;
	private short mValue;
	private readonly ushort mPadding4;

	/// <remarks>
	/// <para>
	/// When setting this property, the value must be <see cref="EventType.JoystickAxisMotion"/>.
	/// Otherwise, it will lead the property to throw an <see cref="ArgumentException"/>!
	/// </para>
	/// </remarks>
	/// <exception cref="ArgumentException">
	/// When setting this property, the value was not <see cref="EventType.JoystickAxisMotion"/>
	/// </exception>
	/// <inheritdoc/>
	public EventType Type
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mCommon.Type;

		set
		{
			if (!Accepts(value))
			{
				failValueArgumentIsNotValid();
			}

			mCommon.Type = value;

			[DoesNotReturn]
			static void failValueArgumentIsNotValid() => throw new ArgumentException($"The given {nameof(value)} is not a valid value for the {nameof(Type)} of a {nameof(JoyAxisEvent)}", paramName: nameof(value));
		}
	}

	/// <inheritdoc/>
	public ulong Timestamp
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mCommon.Timestamp;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mCommon.Timestamp = value;
	}

	/// <summary>
	/// Gets or sets the joystick device ID for the <see cref="Joystick"/> associated with the event
	/// </summary>
	/// <value>
	/// The joystick device ID for the <see cref="Joystick"/> associated with the event
	/// </value>
	public uint JoystickId
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mWhich;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mWhich = value;
	}

	/// <summary>
	/// Gets or sets the axis index for the joystick axis that changed
	/// </summary>
	/// <value>
	/// The axis index for the joystick axis that changed
	/// </value>
	public byte Axis
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mAxis;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mAxis = value;
	}

	/// <summary>
	/// Gets or sets the value for the axis that changed
	/// </summary>
	/// <value>
	/// The value for the axis that changed, in the range from <see cref="short.MinValue"/> to <see cref="short.MaxValue"/>
	/// </value>
	public short Value
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mValue;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mValue = value;
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
							   .Append($", {nameof(JoystickId)}: ")
							   .Append(JoystickId.ToString(format, formatProvider))
							   .Append($", {nameof(Axis)}: ")
							   .Append(Axis.ToString(format, formatProvider))
							   .Append($", {nameof(Value)}: ")
							   .Append(Value.ToString(format, formatProvider))
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
			&& SpanFormat.TryWrite($", {nameof(Axis)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(Axis, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite($", {nameof(Value)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(Value, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite(" }", ref destination, ref charsWritten);
	}

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static implicit operator Event(in JoyAxisEvent @event) => new(in @event);

	/// <remarks>
	/// <para>
	/// The <see cref="Event.Type"/> of the given <paramref name="event"/> must be <see cref="EventType.JoystickAxisMotion"/>.
	/// Otherwise, it will lead the method to throw an <see cref="ArgumentException"/>!
	/// </para>
	/// </remarks>
	/// <exception cref="ArgumentException">
	/// The <see cref="Event.Type"/> of the given <paramref name="event"/> was not <see cref="EventType.JoystickAxisMotion"/>
	/// </exception>
	/// <inheritdoc/>
	public static explicit operator JoyAxisEvent(in Event @event)
	{
		if (!Accepts(@event.Type))
		{
			failEventArgumentIsNotJoyAxisEvent();
		}

		return @event.JAxis;

		[DoesNotReturn]
		static void failEventArgumentIsNotJoyAxisEvent() => throw new ArgumentException($"{nameof(@event)} must be a {nameof(JoyAxisEvent)} by {nameof(Type)}", paramName: nameof(@event));
	}
}
