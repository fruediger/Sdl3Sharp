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
	[FieldOffset(0)] internal JoyBallEvent JBall;

	/// <summary>
	/// Creates a new <see cref="Event"/> from a <see cref="JoyBallEvent"/>
	/// </summary>
	/// <param name="event">The <see cref="JoyBallEvent"/> to store into the newly created <see cref="Event"/></param>
	public Event(in JoyBallEvent @event) :
#pragma warning disable IDE0034 // Leave it for explicitness sake
		this(default(IUnsafeConstructorDispatch?))
#pragma warning restore IDE0034
		=> JBall = @event;
}

/// <summary>
/// Represents an event that occurs when a joystick trackball changes position
/// </summary>
/// <remarks>
/// <para>
/// Associated <see cref="EventType"/>s:
/// <list type="bullet">
/// <item><description><see cref="EventType.JoystickBallMotion"/></description></item>
/// </list>
/// </para>
/// </remarks>
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
[StructLayout(LayoutKind.Sequential)]
public struct JoyBallEvent : ICommonEvent<JoyBallEvent>, IFormattable, ISpanFormattable
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly string DebuggerDisplay => ToString(formatProvider: CultureInfo.InvariantCulture);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private static bool Accepts(EventType type) => type is EventType.JoystickBallMotion;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static bool ICommonEvent<JoyBallEvent>.Accepts(EventType type) => Accepts(type);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static ref JoyBallEvent ICommonEvent<JoyBallEvent>.GetReference(ref Event @event) => ref @event.JBall;

	private CommonEvent mCommon;
	private uint mWhich;
	private byte mBall;
	private readonly byte mPadding1, mPadding2, mPadding3;
	private short mXRel;
	private short mYRel;

	/// <remarks>
	/// <para>
	/// When setting this property, the value must be <see cref="EventType.JoystickBallMotion"/>.
	/// Otherwise, it will lead the property to throw an <see cref="ArgumentException"/>!
	/// </para>
	/// </remarks>
	/// <exception cref="ArgumentException">
	/// When setting this property, the value was not <see cref="EventType.JoystickBallMotion"/>
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
			static void failValueArgumentIsNotValid() => throw new ArgumentException($"The given {nameof(value)} is not a valid value for the {nameof(Type)} of a {nameof(JoyBallEvent)}", paramName: nameof(value));
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
	/// Gets or sets the ball index for the joystick trackball that changed
	/// </summary>
	/// <value>
	/// The ball index for the joystick trackball that changed
	/// </value>
	public byte Ball
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mBall;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mBall = value;
	}

	/// <summary>
	/// Gets or sets the relative motion in the X direction
	/// </summary>
	/// <value>
	/// The relative motion in the X direction
	/// </value>
	public short RelativeX
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mXRel;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mXRel = value;
	}

	/// <summary>
	/// Gets or sets the relative motion in the Y direction
	/// </summary>
	/// <value>
	/// The relative motion in the Y direction
	/// </value>
	public short RelativeY
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mYRel;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mYRel = value;
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
							   .Append($", {Ball}: ")
							   .Append(Ball.ToString(format, formatProvider))
							   .Append($", {RelativeX}: ")
							   .Append(RelativeX.ToString(format, formatProvider))
							   .Append($", {RelativeY}: ")
							   .Append(RelativeY.ToString(format, formatProvider))
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
			&& SpanFormat.TryWrite($", {nameof(Ball)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(Ball, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite($", {nameof(RelativeX)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(RelativeX, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite($", {nameof(RelativeY)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(RelativeY, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite(" }", ref destination, ref charsWritten);
	}

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static implicit operator Event(in JoyBallEvent @event) => new(in @event);

	/// <remarks>
	/// <para>
	/// The <see cref="Event.Type"/> of the given <paramref name="event"/> must be <see cref="EventType.JoystickBallMotion"/>.
	/// Otherwise, it will lead the method to throw an <see cref="ArgumentException"/>!
	/// </para>
	/// </remarks>
	/// <exception cref="ArgumentException">
	/// The <see cref="Event.Type"/> of the given <paramref name="event"/> was not <see cref="EventType.JoystickBallMotion"/>
	/// </exception>
	/// <inheritdoc/>
	public static explicit operator JoyBallEvent(in Event @event)
	{
		if (!Accepts(@event.Type))
		{
			failEventArgumentIsNotJoyBallEvent();
		}

		return @event.JBall;

		[DoesNotReturn]
		static void failEventArgumentIsNotJoyBallEvent() => throw new ArgumentException($"{nameof(@event)} must be a {nameof(JoyBallEvent)} by {nameof(Type)}", paramName: nameof(@event));
	}
}
