using Sdl3Sharp.Internal;
using Sdl3Sharp.Internal.Interop;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sdl3Sharp.Events;

partial struct Event
{
	[FieldOffset(0)] internal GamepadButtonEvent GButton;

	/// <summary>
	/// Creates a new <see cref="Event"/> from a <see cref="GamepadButtonEvent"/>
	/// </summary>
	/// <param name="event">The <see cref="GamepadButtonEvent"/> to store into the newly created <see cref="Event"/></param>
	public Event(in GamepadButtonEvent @event) :
#pragma warning disable IDE0034 // Leave it for explicitness sake
		this(default(IUnsafeConstructorDispatch?))
#pragma warning restore IDE0034
		=> GButton = @event;
}

/// <summary>
/// Represents an event that occurs when a gamepad button is being <see cref="EventType.GamepadButtonDown">pressed</see> or <see cref="EventType.GamepadButtonUp">released</see>
/// </summary>
/// <remarks>
/// <para>
/// Associated <see cref="EventType"/>s:
/// <list type="bullet">
/// <item><description><see cref="EventType.GamepadButtonDown"/></description></item>
/// <item><description><see cref="EventType.GamepadButtonUp"/></description></item>
/// </list>
/// </para>
/// </remarks>
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
[StructLayout(LayoutKind.Sequential)]
public struct GamepadButtonEvent : ICommonEvent<GamepadButtonEvent>, IFormattable, ISpanFormattable
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly string DebuggerDisplay => ToString(formatProvider: CultureInfo.InvariantCulture);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private static bool Accepts(EventType type) => type is EventType.GamepadButtonDown or EventType.GamepadButtonUp;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static bool ICommonEvent<GamepadButtonEvent>.Accepts(EventType type) => Accepts(type);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static ref GamepadButtonEvent ICommonEvent<GamepadButtonEvent>.GetReference(ref Event @event) => ref @event.GButton;

	private CommonEvent mCommon;
	private uint mWhich;
	private byte mButton;
	private CBool mDown;
	private readonly byte mPadding1, mPadding2;

	/// <remarks>
	/// <para>
	/// When setting this property, the value must be either <see cref="EventType.GamepadButtonDown"/> or <see cref="EventType.GamepadButtonUp"/>.
	/// Otherwise, it will lead the property to throw an <see cref="ArgumentException"/>!
	/// </para>
	/// </remarks>
	/// <exception cref="ArgumentException">
	/// When setting this property, the value was neither <see cref="EventType.GamepadButtonDown"/> nor <see cref="EventType.GamepadButtonUp"/>
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
			static void failValueArgumentIsNotValid() => throw new ArgumentException($"The given {nameof(value)} is not a valid value for the {nameof(Type)} of a {nameof(GamepadButtonEvent)}", paramName: nameof(value));
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
	/// Gets or sets the button index for the button that was <see cref="EventType.GamepadButtonDown">pressed</see> or <see cref="EventType.GamepadButtonUp">released</see>
	/// </summary>
	/// <value>
	/// The button index for the button that was <see cref="EventType.GamepadButtonDown">pressed</see> or <see cref="EventType.GamepadButtonUp">released</see>
	/// </value>
	public byte Button
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mButton;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mButton = value;
	}

	/// <summary>
	/// Gets or sets a value indicating whether the button is pressed
	/// </summary>
	/// <value>
	/// A value indicating whether the button is pressed
	/// </value>
	public bool IsDown
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mDown;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mDown = value;
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
							   .Append($", {Button}: ")
							   .Append(Button.ToString(format, formatProvider))
							   .Append($", {nameof(IsDown)}: ")
							   .Append(IsDown)
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
			&& SpanFormat.TryWrite($", {nameof(Button)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(Button, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite($", {nameof(IsDown)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(IsDown, ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(" }", ref destination, ref charsWritten);
	}

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static implicit operator Event(in GamepadButtonEvent @event) => new(in @event);

	/// <remarks>
	/// <para>
	/// The <see cref="Event.Type"/> of the given <paramref name="event"/> must be either <see cref="EventType.GamepadButtonDown"/> or <see cref="EventType.GamepadButtonUp"/>.
	/// Otherwise, it will lead the method to throw an <see cref="ArgumentException"/>!
	/// </para>
	/// </remarks>
	/// <exception cref="ArgumentException">
	/// The <see cref="Event.Type"/> of the given <paramref name="event"/> was neither <see cref="EventType.GamepadButtonDown"/> nor <see cref="EventType.GamepadButtonUp"/>
	/// </exception>
	/// <inheritdoc/>
	public static explicit operator GamepadButtonEvent(in Event @event)
	{
		if (!Accepts(@event.Type))
		{
			failEventArgumentIsNotGamepadButtonEvent();
		}

		return @event.GButton;

		[DoesNotReturn]
		static void failEventArgumentIsNotGamepadButtonEvent() => throw new ArgumentException($"{nameof(@event)} must be a {nameof(GamepadButtonEvent)} by {nameof(Type)}", paramName: nameof(@event));
	}
}
