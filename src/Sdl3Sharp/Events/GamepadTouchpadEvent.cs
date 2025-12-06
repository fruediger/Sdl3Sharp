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
	[FieldOffset(0)] internal GamepadTouchpadEvent GTouchpad;

	/// <summary>
	/// Creates a new <see cref="Event"/> from a <see cref="GamepadTouchpadEvent"/>
	/// </summary>
	/// <param name="event">The <see cref="GamepadTouchpadEvent"/> to store into the newly created <see cref="Event"/></param>
	public Event(in GamepadTouchpadEvent @event) :
#pragma warning disable IDE0034 // Leave it for explicitness sake
		this(default(IUnsafeConstructorDispatch?))
#pragma warning restore IDE0034
		=> GTouchpad = @event;
}

/// <summary>
/// Represents an event that occurs when a finger interacts with a gamepad touchpad
/// </summary>
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
[StructLayout(LayoutKind.Sequential)]
public struct GamepadTouchpadEvent : ICommonEvent<GamepadTouchpadEvent>, IFormattable, ISpanFormattable
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly string DebuggerDisplay => ToString(formatProvider: CultureInfo.InvariantCulture);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private static bool Accepts(EventType type) => type is >= EventType.GamepadTouchpadDown and <= EventType.GamepadTouchpadUp;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static bool ICommonEvent<GamepadTouchpadEvent>.Accepts(EventType type) => Accepts(type);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static ref GamepadTouchpadEvent ICommonEvent<GamepadTouchpadEvent>.GetReference(ref Event @event) => ref @event.GTouchpad;

	private CommonEvent mCommon;
	private uint mWhich;
	private int mTouchpad;
	private int mFinger;
	private float mX;
	private float mY;
	private float mPressure;

	/// <remarks>
	/// <para>
	/// When setting this property, the value must be either <see cref="EventType.GamepadTouchpadDown"/>, <see cref="EventType.GamepadTouchpadMotion"/>, or <see cref="EventType.GamepadTouchpadUp"/>.
	/// Otherwise, it will lead the property to throw an <see cref="ArgumentException"/>!
	/// </para>
	/// </remarks>
	/// <exception cref="ArgumentException">
	/// When setting this property, the value was neither <see cref="EventType.GamepadTouchpadDown"/>, <see cref="EventType.GamepadTouchpadMotion"/>, nor <see cref="EventType.GamepadTouchpadUp"/>
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
			static void failValueArgumentIsNotValid() => throw new ArgumentException($"The given {nameof(value)} is not a valid value for the {nameof(Type)} of a {nameof(GamepadTouchpadEvent)}", paramName: nameof(value));
		}
	}

	/// <inheritdoc/>
	public ulong Timestamp
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mCommon.Timestamp;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mCommon.Timestamp = value;
	}

	/// <summary>
	/// Gets or sets the joystick device ID for the <see cref="Gamepad"/> associated with the event
	/// </summary>
	/// <value>
	/// The joystick device ID for the <see cref="Gamepad"/> associated with the event
	/// </value>
	public uint JoystickId
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mWhich;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mWhich = value;
	}

	/// <summary>
	/// Gets or sets the index of the touchpad on the gamepad
	/// </summary>
	/// <value>
	/// The index of the touchpad on the gamepad
	/// </value>
	public int Touchpad
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mTouchpad;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mTouchpad = value;
	}

	/// <summary>
	/// Gets or sets the index of the finger on the touchpad
	/// </summary>
	/// <value>
	/// The index of the finger on the touchpad
	/// </value>
	public int Finger
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mFinger;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mFinger = value;
	}

	/// <summary>
	/// Gets or sets the X position of the finger on the touchpad
	/// </summary>
	/// <value>
	/// The X position of the finger on the touchpad, normalized between <c>0</c> and <c>1</c>, where <c>0</c> is the left edge and <c>1</c> is the right edge
	/// </value>
	public float X
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mX;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mX = value;
	}

	/// <summary>
	/// Gets or sets the Y position of the finger on the touchpad
	/// </summary>
	/// <value>
	/// The Y position of the finger on the touchpad, normalized between <c>0</c> and <c>1</c>, where <c>0</c> is the top edge and <c>1</c> is the bottom edge
	/// </value>
	public float Y
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mY;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mY = value;
	}

	/// <summary>
	/// Gets or sets the pressure of the finger applied on the touchpad
	/// </summary>
	/// <value>
	/// The pressure of the finger applied on the touchpad, normalized between <c>0</c> and <c>1</c>, where <c>0</c> is no pressure and <c>1</c> is maximum pressure
	/// </value>
	public float Pressure
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mPressure;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mPressure = value;
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
							   .Append($", {nameof(Touchpad)}: ")
							   .Append(Touchpad.ToString(format, formatProvider))
							   .Append($", {nameof(Finger)}: ")
							   .Append(Finger.ToString(format, formatProvider))
							   .Append($", {nameof(X)}: ")
							   .Append(X.ToString(format, formatProvider))
							   .Append($", {nameof(Y)}: ")
							   .Append(Y.ToString(format, formatProvider))
							   .Append($", {nameof(Pressure)}: ")
							   .Append(Pressure.ToString(format, formatProvider))
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
			&& SpanFormat.TryWrite($", {nameof(Touchpad)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(Touchpad, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite($", {nameof(Finger)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(Finger, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite($", {nameof(X)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(X, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite($", {nameof(Y)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(Y, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite($", {nameof(Pressure)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(Pressure, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite(" }", ref destination, ref charsWritten);
	}

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static implicit operator Event(in GamepadTouchpadEvent @event) => new(in @event);

	/// <remarks>
	/// <para>
	/// The <see cref="Event.Type"/> of the given <paramref name="event"/> must be either <see cref="EventType.GamepadTouchpadDown"/>, <see cref="EventType.GamepadTouchpadMotion"/>, or <see cref="EventType.GamepadTouchpadUp"/>.
	/// Otherwise, it will lead the method to throw an <see cref="ArgumentException"/>!
	/// </para>
	/// </remarks>
	/// <exception cref="ArgumentException">
	/// The <see cref="Event.Type"/> of the given <paramref name="event"/> was neither <see cref="EventType.GamepadTouchpadDown"/>, <see cref="EventType.GamepadTouchpadMotion"/>, nor <see cref="EventType.GamepadTouchpadUp"/>
	/// </exception>
	/// <inheritdoc/>
	public static explicit operator GamepadTouchpadEvent(in Event @event)
	{
		if (!Accepts(@event.Type))
		{
			failEventArgumentIsNotGamepadTouchpadEvent();
		}

		return @event.GTouchpad;

		[DoesNotReturn]
		static void failEventArgumentIsNotGamepadTouchpadEvent() => throw new ArgumentException($"{nameof(@event)} must be a {nameof(GamepadTouchpadEvent)} by {nameof(Type)}", paramName: nameof(@event));
	}
}
