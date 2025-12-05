using Sdl3Sharp.Input;
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
	[FieldOffset(0)] internal MouseWheelEvent Wheel;

	/// <summary>
	/// Creates a new <see cref="Event"/> from a <see cref="MouseWheelEvent"/>
	/// </summary>
	/// <param name="event">The <see cref="MouseWheelEvent"/> to store into the newly created <see cref="Event"/></param>
	public Event(in MouseWheelEvent @event) :
#pragma warning disable IDE0034 // Leave it for explicitness sake
		this(default(IUnsafeConstructorDispatch?))
#pragma warning restore IDE0034
		=> Wheel = @event;
}

/// <summary>
/// Represents an event that occurs when a mouse wheel is scrolled
/// </summary>
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
[StructLayout(LayoutKind.Sequential)]
public struct MouseWheelEvent : ICommonEvent<MouseWheelEvent>, IFormattable, ISpanFormattable
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly string DebuggerDisplay => ToString(formatProvider: CultureInfo.InvariantCulture);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private static bool Accepts(EventType type) => type is EventType.MouseWheel;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static bool ICommonEvent<MouseWheelEvent>.Accepts(EventType type) => Accepts(type);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static ref MouseWheelEvent ICommonEvent<MouseWheelEvent>.GetReference(ref Event @event) => ref @event.Wheel;

	private CommonEvent mCommon;
	private uint mWindowID;
	private uint mWhich;
	private float mX;
	private float mY;
	private MouseWheelDirection mDirection;
	private float mMouseX;
	private float mMouseY;
	private int mIntegerX;
	private int mIntegerY;

	/// <remarks>
	/// <para>
	/// When setting this property, the value must be <see cref="EventType.MouseWheel"/>.
	/// Otherwise, it will lead the property to throw an <see cref="ArgumentException"/>!
	/// </para>
	/// </remarks>
	/// <exception cref="ArgumentException">
	/// When setting this property, the value was not <see cref="EventType.MouseWheel"/>
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
			static void failValueArgumentIsNotValid() => throw new ArgumentException($"The given {nameof(value)} is not a valid value for the {nameof(Type)} of a {nameof(MouseWheelEvent)}", paramName: nameof(value));
		}
	}

	/// <inheritdoc/>
	public ulong Timestamp
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mCommon.Timestamp;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mCommon.Timestamp = value;
	}

	/// <summary>
	/// Gets or sets the window Id of the <see cref="Window"/> with mouse focus, if any
	/// </summary>
	/// <value>
	/// The window Id of the <see cref="Window"/> with mouse focus, if any, or <c>0</c>
	/// </value>
	public uint WindowId
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mWindowID;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mWindowID = value;
	}

	/// <summary>
	/// Gets or sets the mouse device ID for the <see cref="Mouse"/> associated with the event
	/// </summary>
	/// <value>
	/// The mouse device ID for the <see cref="Mouse"/> associated with the event, or <c>0</c>
	/// </value>
	public uint MouseId
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mWhich;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mWhich = value;
	}

	/// <summary>
	/// Gets or sets the amount scrolled horizontally
	/// </summary>
	/// <value>
	/// The amount scrolled horizontally
	/// </value>
	/// <remarks>
	/// <para>
	/// If the value of this property is positive, the mouse wheel was scrolled to the right.
	/// If the value is negative, the mouse wheel was scrolled to the left.
	/// </para>
	/// <para>
	/// Notice that if the value of the <see cref="Direction"/> property is <see cref="MouseWheelDirection.Flipped"/>, the value of this property is inverted.
	/// You might want to multiply this value by <c>-1</c> in that case to change it back.
	/// </para>
	/// </remarks>
	public float X
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mX;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mX = value;
	}

	/// <summary>
	/// Gets or sets the amount scrolled vertically
	/// </summary>
	/// <value>
	/// The amount scrolled vertically
	/// </value>
	/// <remarks>
	/// <para>
	/// If the value of this property is positive, the mouse wheel was scrolled away from the user.
	/// If the value is negative, the mouse wheel was scrolled towards the user.
	/// </para>
	/// <para>
	/// Notice that if the value of the <see cref="Direction"/> property is <see cref="MouseWheelDirection.Flipped"/>, the value of this property is inverted.
	/// You might want to multiply this value by <c>-1</c> in that case to change it back.
	/// </para>
	/// </remarks>
	public float Y
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mY;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mY = value;
	}

	/// <summary>
	/// Gets or sets the direction type of the mouse wheel scrolling
	/// </summary>
	/// <value>
	/// The direction type of the mouse wheel scrolling
	/// </value>
	/// <remarks>
	/// <para>
	/// Notice that if the value of this property is <see cref="MouseWheelDirection.Flipped"/>, the values of the <see cref="X"/> and <see cref="Y"/> properties are inverted.
	/// You might want to multiply those values by <c>-1</c> in that case to change them back.
	/// </para>
	/// </remarks>
	public MouseWheelDirection Direction
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mDirection;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mDirection = value;
	}

	/// <summary>
	/// Gets or sets the X coordinate of the mouse cursor
	/// </summary>
	/// <value>
	/// The X coordinate of the mouse cursor, relative to the <see cref="Window"/> specified through <see cref="WindowId"/>
	/// </value>
	public float MouseX
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mMouseX;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mMouseX = value;
	}

	/// <summary>
	/// Gets or sets the Y coordinate of the mouse cursor
	/// </summary>
	/// <value>
	/// The Y coordinate of the mouse cursor, relative to the <see cref="Window"/> specified through <see cref="WindowId"/>
	/// </value>
	public float MouseY
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mMouseY;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mMouseY = value;
	}

	/// <summary>
	/// Gets or sets the amount scrolled horizontally, accumulated to whole scroll "ticks"
	/// </summary>
	/// <value>
	/// The amount scrolled horizontally, accumulated to whole scroll "ticks"
	/// </value>
	public int IntegerX
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mIntegerX;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mIntegerX = value;
	}

	/// <summary>
	/// Gets or sets the amount scrolled vertically, accumulated to whole scroll "ticks"
	/// </summary>
	/// <value>
	/// The amount scrolled vertically, accumulated to whole scroll "ticks"
	/// </value>
	public int IntegerY
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mIntegerY;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mIntegerY = value;
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
							   .Append($", {nameof(WindowId)}: ")
							   .Append(WindowId.ToString(format, formatProvider))
							   .Append($", {nameof(MouseId)}: ")
							   .Append(MouseId.ToString(format, formatProvider))
							   .Append($", {nameof(X)}: ")
							   .Append(X.ToString(format, formatProvider))
							   .Append($", {nameof(Y)}: ")
							   .Append(Y.ToString(format, formatProvider))
							   .Append($", {nameof(Direction)}: ")
							   .Append(Direction)
							   .Append($", {nameof(MouseX)}: ")
							   .Append(MouseX.ToString(format, formatProvider))
							   .Append($", {nameof(MouseY)}: ")
							   .Append(MouseY.ToString(format, formatProvider))
							   .Append($", {nameof(IntegerX)}: ")
							   .Append(IntegerX.ToString(format, formatProvider))
							   .Append($", {nameof(IntegerY)}: ")
							   .Append(IntegerY.ToString(format, formatProvider))
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
			&& SpanFormat.TryWrite($", {nameof(WindowId)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(WindowId, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite($", {nameof(MouseId)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(MouseId, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite($", {nameof(X)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(X, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite($", {nameof(Y)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(Y, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite($", {nameof(Direction)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(Direction, ref destination, ref charsWritten)
			&& SpanFormat.TryWrite($", {nameof(MouseX)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(MouseX, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite($", {nameof(MouseY)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(MouseY, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite($", {nameof(IntegerX)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(IntegerX, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite($", {nameof(IntegerY)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(IntegerY, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite(" }", ref destination, ref charsWritten);
	}

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static implicit operator Event(in MouseWheelEvent @event) => new(in @event);

	/// <remarks>
	/// <para>
	/// The <see cref="Event.Type"/> of the given <paramref name="event"/> must be <see cref="EventType.MouseWheel"/>.
	/// Otherwise, it will lead the method to throw an <see cref="ArgumentException"/>!
	/// </para>
	/// </remarks>
	/// <exception cref="ArgumentException">
	/// The <see cref="Event.Type"/> of the given <paramref name="event"/> was not <see cref="EventType.MouseWheel"/>
	/// </exception>
	/// <inheritdoc/>
	public static explicit operator MouseWheelEvent(in Event @event)
	{
		if (!Accepts(@event.Type))
		{
			failEventArgumentIsNotMouseWheelEvent();
		}

		return @event.Wheel;

		[DoesNotReturn]
		static void failEventArgumentIsNotMouseWheelEvent() => throw new ArgumentException($"{nameof(@event)} must be a {nameof(MouseWheelEvent)} by {nameof(Type)}", paramName: nameof(@event));
	}
}
