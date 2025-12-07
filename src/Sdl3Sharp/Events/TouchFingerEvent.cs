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
	[FieldOffset(0)] internal TouchFingerEvent TFinger;

	/// <summary>
	/// Creates a new <see cref="Event"/> from a <see cref="TouchFingerEvent"/>
	/// </summary>
	/// <param name="event">The <see cref="TouchFingerEvent"/> to store into the newly created <see cref="Event"/></param>
	public Event(in TouchFingerEvent @event) :
#pragma warning disable IDE0034 // Leave it for explicitness sake
		this(default(IUnsafeConstructorDispatch?))
#pragma warning restore IDE0034
		=> TFinger = @event;
}

/// <summary>
/// Represents an event that occurs when a finger is <see cref="EventType.FingerDown">placed on</see>, <see cref="EventType.FingerMotion">moved on</see>, <see cref="EventType.FingerUp">lifted from</see>, or <see cref="EventType.FingerCanceled">canceled</see> on a touch device
/// </summary>
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
[StructLayout(LayoutKind.Sequential)]
public struct TouchFingerEvent : ICommonEvent<TouchFingerEvent>, IFormattable, ISpanFormattable
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly string DebuggerDisplay => ToString(formatProvider: CultureInfo.InvariantCulture);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private static bool Accepts(EventType type) => type is >= EventType.FingerDown and <= EventType.FingerCanceled;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static bool ICommonEvent<TouchFingerEvent>.Accepts(EventType type) => Accepts(type);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static ref TouchFingerEvent ICommonEvent<TouchFingerEvent>.GetReference(ref Event @event) => ref @event.TFinger;

	private CommonEvent mCommon;
	private ulong mTouchID;
	private ulong mFingerID;
	private float mX;
	private float mY;
	private float mDX;
	private float mDY;
	private float mPressure;
	private uint mWindowID;

	/// <remarks>
	/// <para>
	/// When setting this property, the value must be either <see cref="EventType.FingerDown"/>, <see cref="EventType.FingerUp"/>, <see cref="EventType.FingerMotion"/>, or <see cref="EventType.FingerCanceled"/>.
	/// Otherwise, it will lead the property to throw an <see cref="ArgumentException"/>!
	/// </para>
	/// </remarks>
	/// <exception cref="ArgumentException">
	/// When setting this property, the value was neither <see cref="EventType.FingerDown"/>, <see cref="EventType.FingerUp"/>, <see cref="EventType.FingerMotion"/>, nor <see cref="EventType.FingerCanceled"/>
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
			static void failValueArgumentIsNotValid() => throw new ArgumentException($"The given {nameof(value)} is not a valid value for the {nameof(Type)} of a {nameof(TouchFingerEvent)}", paramName: nameof(value));
		}
	}

	/// <inheritdoc/>
	public ulong Timestamp
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mCommon.Timestamp;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mCommon.Timestamp = value;
	}

	/// <summary>
	/// Gets or sets the touch device ID for the <see cref="TouchDevice"/> associated with the event
	/// </summary>
	/// <value>
	/// The touch device ID for the <see cref="TouchDevice"/> associated with the event
	/// </value>
	public ulong TouchId
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mTouchID;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mTouchID = value;
	}

	/// <summary>
	/// Gets or sets the finger ID for the <see cref="Finger"/> associated with the event
	/// </summary>
	/// <value>
	/// The finger ID for the <see cref="Finger"/> associated with the event
	/// </value>
	public ulong FingerId
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mFingerID;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mFingerID = value;
	}

	/// <summary>
	/// Gets or sets the X position of the finger on the touch device
	/// </summary>
	/// <value>
	/// The X position of the finger on the touch device, normalized between <c>0</c> and <c>1</c>, where <c>0</c> is the left edge and <c>1</c> is the right edge
	/// </value>
	/// <remarks>
	/// <para>
	/// Note that while the coordinates are <em>normalized</em>, they are not <em>clamped</em>, which means in some circumstances you can get a value outside of this range.
	/// For example, a renderer using logical presentation might give a negative value when the touch is in the letterboxing.
	/// Some platforms might report a touch outside of the window, which will also be outside of the range.
	/// </para>
	/// </remarks>
	public float X
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mX;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mX = value;
	}

	/// <summary>
	/// Gets or sets the Y position of the finger on the touch device
	/// </summary>
	/// <value>
	/// The Y position of the finger on the touch device, normalized between <c>0</c> and <c>1</c>, where <c>0</c> is the top edge and <c>1</c> is the bottom edge
	/// </value>
	/// <remarks>
	/// <para>
	/// Note that while the coordinates are <em>normalized</em>, they are not <em>clamped</em>, which means in some circumstances you can get a value outside of this range.
	/// For example, a renderer using logical presentation might give a negative value when the touch is in the letterboxing.
	/// Some platforms might report a touch outside of the window, which will also be outside of the range.
	/// </para>
	/// </remarks>
	public float Y
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mY;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mY = value;
	}

	/// <summary>
	/// Gets or sets the change in X position of the finger on the touch device
	/// </summary>
	/// <value>
	/// The change in X position of the finger on the touch device, normalized between <c>-1</c> and <c>1</c>, where <c>-1</c> is a tranversal all the way from the right edge to the left edge, and <c>1</c> is a transversal all the way from the left edge to the right edge
	/// </value>
	public float DX
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mDX;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mDX = value;
	}

	/// <summary>
	/// Gets or sets the change in Y position of the finger on the touch device
	/// </summary>
	/// <value>
	/// The change in Y position of the finger on the touch device, normalized between <c>-1</c> and <c>1</c>, where <c>-1</c> is a tranversal all the way from the bottom edge to the top edge, and <c>1</c> is a transversal all the way from the top edge to the bottom edge
	/// </value>
	public float DY
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mDY;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mDY = value;
	}

	/// <summary>
	/// Gets or sets the pressure of the finger applied on the touch device
	/// </summary>
	/// <value>
	/// The pressure of the finger applied on the touch device, normalized between <c>0</c> and <c>1</c>, where <c>0</c> is no pressure and <c>1</c> is maximum pressure
	/// </value>
	public float Pressure
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mPressure;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mPressure = value;
	}

	/// <summary>
	/// Gets or sets the window Id of the <see cref="Window"/> underneath the finger, if any
	/// </summary>
	/// <value>
	/// The window Id of the <see cref="Window"/> underneath the finger, if any, or <c>0</c>
	/// </value>
	public uint WindowId
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mWindowID;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mWindowID = value;
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
							   .Append($", {nameof(TouchId)}: ")
							   .Append(TouchId.ToString(format, formatProvider))
							   .Append($", {nameof(FingerId)}: ")
							   .Append(FingerId.ToString(format, formatProvider))
							   .Append($", {nameof(X)}: ")
							   .Append(X.ToString(format, formatProvider))
							   .Append($", {nameof(Y)}: ")
							   .Append(Y.ToString(format, formatProvider))
							   .Append($", {nameof(DX)}: ")
							   .Append(DX.ToString(format, formatProvider))
							   .Append($", {nameof(DY)}: ")
							   .Append(DY.ToString(format, formatProvider))
							   .Append($", {nameof(Pressure)}: ")
							   .Append(Pressure.ToString(format, formatProvider))
							   .Append($", {nameof(WindowId)}: ")
							   .Append(WindowId.ToString(format, formatProvider))
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
			&& SpanFormat.TryWrite($", {nameof(TouchId)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(TouchId, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite($", {nameof(FingerId)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(FingerId, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite($", {nameof(X)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(X, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite($", {nameof(Y)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(Y, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite($", {nameof(DX)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(DX, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite($", {nameof(DY)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(DY, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite($", {nameof(Pressure)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(Pressure, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite($", {nameof(WindowId)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(WindowId, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite(" }", ref destination, ref charsWritten);
	}

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static implicit operator Event(in TouchFingerEvent @event) => new(in @event);

	/// <remarks>
	/// <para>
	/// The <see cref="Event.Type"/> of the given <paramref name="event"/> must be either <see cref="EventType.FingerDown"/>, <see cref="EventType.FingerUp"/>, <see cref="EventType.FingerMotion"/>, or <see cref="EventType.FingerCanceled"/>.
	/// Otherwise, it will lead the method to throw an <see cref="ArgumentException"/>!
	/// </para>
	/// </remarks>
	/// <exception cref="ArgumentException">
	/// The <see cref="Event.Type"/> of the given <paramref name="event"/> was neither <see cref="EventType.FingerDown"/>, <see cref="EventType.FingerUp"/>, <see cref="EventType.FingerMotion"/>, nor <see cref="EventType.FingerCanceled"/>
	/// </exception>
	/// <inheritdoc/>
	public static explicit operator TouchFingerEvent(in Event @event)
	{
		if (!Accepts(@event.Type))
		{
			failEventArgumentIsNotTouchFingerEvent();
		}

		return @event.TFinger;

		[DoesNotReturn]
		static void failEventArgumentIsNotTouchFingerEvent() => throw new ArgumentException($"{nameof(@event)} must be a {nameof(TouchFingerEvent)} by {nameof(Type)}", paramName: nameof(@event));
	}
}
