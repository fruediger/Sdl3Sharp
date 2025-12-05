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
	[FieldOffset(0)] internal MouseMotionEvent Motion;

	/// <summary>
	/// Creates a new <see cref="Event"/> from a <see cref="MouseMotionEvent"/>
	/// </summary>
	/// <param name="event">The <see cref="MouseMotionEvent"/> to store into the newly created <see cref="Event"/></param>
	public Event(in MouseMotionEvent @event) :
#pragma warning disable IDE0034 // Leave it for explicitness sake
		this(default(IUnsafeConstructorDispatch?))
#pragma warning restore IDE0034
		=> Motion = @event;
}

/// <summary>
/// Represents an event that occurs when a mouse is moved
/// </summary>
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
[StructLayout(LayoutKind.Sequential)]
public struct MouseMotionEvent : ICommonEvent<MouseMotionEvent>, IFormattable, ISpanFormattable
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly string DebuggerDisplay => ToString(formatProvider: CultureInfo.InvariantCulture);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private static bool Accepts(EventType type) => type is EventType.MouseMotion;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static bool ICommonEvent<MouseMotionEvent>.Accepts(EventType type) => Accepts(type);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static ref MouseMotionEvent ICommonEvent<MouseMotionEvent>.GetReference(ref Event @event) => ref @event.Motion;

	private CommonEvent mCommon;
	private uint mWindowID;
	private uint mWhich;
	private MouseButtonFlags mState;
	private float mX;
	private float mY;
	private float mXRel;
	private float mYRel;

	/// <remarks>
	/// <para>
	/// When setting this property, the value must be <see cref="EventType.MouseMotion"/>.
	/// Otherwise, it will lead the property to throw an <see cref="ArgumentException"/>!
	/// </para>
	/// </remarks>
	/// <exception cref="ArgumentException">
	/// When setting this property, the value was not <see cref="EventType.MouseMotion"/>
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
			static void failValueArgumentIsNotValid() => throw new ArgumentException($"The given {nameof(value)} is not a valid value for the {nameof(Type)} of a {nameof(MouseMotionEvent)}", paramName: nameof(value));
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
	/// The mouse device ID for the <see cref="Mouse"/> associated with the event, <see cref="SDL_TOUCH_MOUSEID"/> for touch events, or <c>0</c>
	/// </value>
	public uint MouseId
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mWhich;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mWhich = value;
	}

	/// <summary>
	/// Gets or sets the current mouse button state
	/// </summary>
	/// <value>
	/// The current mouse button state
	/// </value>
	public MouseButtonFlags State
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mState;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mState = value;
	}

	/// <summary>
	/// Gets or sets the X coordinate of the mouse cursor
	/// </summary>
	/// <value>
	/// The X coordinate of the mouse cursor, relative to the <see cref="Window"/> specified through <see cref="WindowId"/>
	/// </value>
	public float X
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mX;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mX = value;
	}

	/// <summary>
	/// Gets or sets the Y coordinate of the mouse cursor
	/// </summary>
	/// <value>
	/// The Y coordinate of the mouse cursor, relative to the <see cref="Window"/> specified through <see cref="WindowId"/>
	/// </value>
	public float Y
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mY;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mY = value;
	}

	/// <summary>
	/// Gets or sets the relative motion in the X direction
	/// </summary>
	/// <value>
	/// The relative motion in the X direction
	/// </value>
	public float RelativeX
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
	public float RelativeY
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
							   .Append($", {nameof(WindowId)}: ")
							   .Append(WindowId.ToString(format, formatProvider))
							   .Append($", {nameof(MouseId)}: ")
							   .Append(MouseId.ToString(format, formatProvider))
							   .Append($", {nameof(State)}: ")
							   .Append(State)
							   .Append($", {nameof(X)}: ")
							   .Append(X.ToString(format, formatProvider))
							   .Append($", {nameof(Y)}: ")
							   .Append(Y.ToString(format, formatProvider))
							   .Append($", {nameof(RelativeX)}: ")
							   .Append(RelativeX.ToString(format, formatProvider))
							   .Append($", {nameof(RelativeY)}: ")
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
			&& SpanFormat.TryWrite($", {nameof(WindowId)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(WindowId, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite($", {nameof(MouseId)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(MouseId, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite($", {nameof(State)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(State, ref destination, ref charsWritten)
			&& SpanFormat.TryWrite($", {nameof(X)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(X, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite($", {nameof(Y)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(Y, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite($", {nameof(RelativeX)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(RelativeX, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite($", {nameof(RelativeY)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(RelativeY, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite(" }", ref destination, ref charsWritten);
	}

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static implicit operator Event(in MouseMotionEvent @event) => new(in @event);

	/// <remarks>
	/// <para>
	/// The <see cref="Event.Type"/> of the given <paramref name="event"/> must be <see cref="EventType.MouseMotion"/>.
	/// Otherwise, it will lead the method to throw an <see cref="ArgumentException"/>!
	/// </para>
	/// </remarks>
	/// <exception cref="ArgumentException">
	/// The <see cref="Event.Type"/> of the given <paramref name="event"/> was not <see cref="EventType.MouseMotion"/>
	/// </exception>
	/// <inheritdoc/>
	public static explicit operator MouseMotionEvent(in Event @event)
	{
		if (!Accepts(@event.Type))
		{
			failEventArgumentIsNotMouseMotionEvent();
		}

		return @event.Motion;

		[DoesNotReturn]
		static void failEventArgumentIsNotMouseMotionEvent() => throw new ArgumentException($"{nameof(@event)} must be a {nameof(MouseMotionEvent)} by {nameof(Type)}", paramName: nameof(@event));
	}
}
