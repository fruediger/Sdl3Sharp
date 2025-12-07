using Sdl3Sharp.Input;
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
	[FieldOffset(0)] internal PenTouchEvent PTouch;

	/// <summary>
	/// Creates a new <see cref="Event"/> from a <see cref="PenTouchEvent"/>
	/// </summary>
	/// <param name="event">The <see cref="PenTouchEvent"/> to store into the newly created <see cref="Event"/></param>
	public Event(in PenTouchEvent @event) :
#pragma warning disable IDE0034 // Leave it for explicitness sake
		this(default(IUnsafeConstructorDispatch?))
#pragma warning restore IDE0034
		=> PTouch = @event;
}

/// <summary>
/// Represents an event that occurs when a pen <see cref="EventType.PenDown">touches</see> or <see cref="EventType.PenUp">lifts</see> from a touch surface
/// </summary>
/// <remarks>
/// <para>
/// Associated <see cref="EventType"/>s:
/// <list type="bullet">
/// <item><description><see cref="EventType.PenDown"/></description></item>
/// <item><description><see cref="EventType.PenUp"/></description></item>
/// </list>
/// </para>
/// </remarks>
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
[StructLayout(LayoutKind.Sequential)]
public struct PenTouchEvent : ICommonEvent<PenTouchEvent>, IFormattable, ISpanFormattable
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly string DebuggerDisplay => ToString(formatProvider: CultureInfo.InvariantCulture);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private static bool Accepts(EventType type) => type is EventType.PenDown or EventType.PenUp;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static bool ICommonEvent<PenTouchEvent>.Accepts(EventType type) => Accepts(type);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static ref PenTouchEvent ICommonEvent<PenTouchEvent>.GetReference(ref Event @event) => ref @event.PTouch;

	private CommonEvent mCommon;
	private uint mWindowID;
	private uint mWhich;
	private PenInputFlags mPenState;
	private float mX;
	private float mY;
	private CBool mEraser;
	private CBool mDown;

	/// <remarks>
	/// <para>
	/// When setting this property, the value must be either <see cref="EventType.PenDown"/> or <see cref="EventType.PenUp"/>.
	/// Otherwise, it will lead the property to throw an <see cref="ArgumentException"/>!
	/// </para>
	/// </remarks>
	/// <exception cref="ArgumentException">
	/// When setting this property, the value was neither <see cref="EventType.PenDown"/> nor <see cref="EventType.PenUp"/>
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
			static void failValueArgumentIsNotValid() => throw new ArgumentException($"The given {nameof(value)} is not a valid value for the {nameof(Type)} of a {nameof(PenTouchEvent)}", paramName: nameof(value));
		}
	}

	/// <inheritdoc/>
	public ulong Timestamp
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mCommon.Timestamp;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mCommon.Timestamp = value;
	}

	/// <summary>
	/// Gets or sets the window Id of the <see cref="Window"/> with pen focus, if any
	/// </summary>
	/// <value>
	/// The window Id of the <see cref="Window"/> with pen focus, if any, or <c>0</c>
	/// </value>
	public uint WindowId
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mWindowID;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mWindowID = value;
	}

	/// <summary>
	/// Gets or sets the pen instance ID for the <see cref="Pen"/> associated with the event
	/// </summary>
	/// <value>
	/// The pen instance ID for the <see cref="Pen"/> associated with the event
	/// </value>
	public uint PenId
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mWhich;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mWhich = value;
	}

	/// <summary>
	/// Gets or sets the state of the pen input at the time of the event
	/// </summary>
	/// <value>
	/// The state of the pen input at the time of the event
	/// </value>
	public PenInputFlags PenState
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mPenState;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mPenState = value;
	}

	/// <summary>
	/// Gets or sets the X coordinate of the pen touch
	/// </summary>
	/// <value>
	/// The X coordinate of the pen touch, relative to the <see cref="Window"/> specified through <see cref="WindowId"/>
	/// </value>
	public float X
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mX;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mX = value;
	}

	/// <summary>
	/// Gets or sets the Y coordinate of the pen touch
	/// </summary>
	/// <value>
	/// The Y coordinate of the pen touch, relative to the <see cref="Window"/> specified through <see cref="WindowId"/>
	/// </value>
	public float Y
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mY;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mY = value;
	}

	/// <summary>
	/// Gets or sets a value indicating whether the eraser end of the pen is in use
	/// </summary>
	/// <value>
	/// A value indicating whether the eraser end of the pen is in use
	/// </value>
	/// <remarks>
	/// <para>
	/// Not all pens support this.
	/// </para>
	/// </remarks>
	public bool IsEraserInUse
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mEraser;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mEraser = value;
	}

	/// <summary>
	/// Gets or sets a value indicating whether the pen is touching the surface
	/// </summary>
	/// <value>
	/// A value indicating whether the pen is touching the surface (or is lifted off)
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
							   .Append($", {nameof(WindowId)}: ")
							   .Append(WindowId.ToString(format, formatProvider))
							   .Append($", {nameof(PenId)}: ")
							   .Append(PenId.ToString(format, formatProvider))
							   .Append($", {nameof(PenState)}: ")
							   .Append(PenState)
							   .Append($", {nameof(X)}: ")
							   .Append(X.ToString(format, formatProvider))
							   .Append($", {nameof(Y)}: ")
							   .Append(Y.ToString(format, formatProvider))
							   .Append($", {nameof(IsEraserInUse)}: ")
							   .Append(IsEraserInUse)
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
			&& SpanFormat.TryWrite($", {nameof(WindowId)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(WindowId, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite($", {nameof(PenId)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(PenId, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite($", {nameof(PenState)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(PenState, ref destination, ref charsWritten)
			&& SpanFormat.TryWrite($", {nameof(X)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(X, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite($", {nameof(Y)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(Y, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite($", {nameof(IsEraserInUse)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(IsEraserInUse, ref destination, ref charsWritten)
			&& SpanFormat.TryWrite($", {nameof(IsDown)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(IsDown, ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(" }", ref destination, ref charsWritten);
	}

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static implicit operator Event(in PenTouchEvent @event) => new(in @event);

	/// <remarks>
	/// <para>
	/// The <see cref="Event.Type"/> of the given <paramref name="event"/> must be either <see cref="EventType.PenDown"/> or <see cref="EventType.PenUp"/>.
	/// Otherwise, it will lead the method to throw an <see cref="ArgumentException"/>!
	/// </para>
	/// </remarks>
	/// <exception cref="ArgumentException">
	/// The <see cref="Event.Type"/> of the given <paramref name="event"/> was neither <see cref="EventType.PenDown"/> nor <see cref="EventType.PenUp"/>
	/// </exception>
	/// <inheritdoc/>
	public static explicit operator PenTouchEvent(in Event @event)
	{
		if (!Accepts(@event.Type))
		{
			failEventArgumentIsNotPenTouchEvent();
		}

		return @event.PTouch;

		[DoesNotReturn]
		static void failEventArgumentIsNotPenTouchEvent() => throw new ArgumentException($"{nameof(@event)} must be a {nameof(PenTouchEvent)} by {nameof(Type)}", paramName: nameof(@event));
	}
}
