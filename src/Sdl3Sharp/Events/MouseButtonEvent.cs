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
	[FieldOffset(0)] internal MouseButtonEvent Button;

	/// <summary>
	/// Creates a new <see cref="Event"/> from a <see cref="MouseButtonEvent"/>
	/// </summary>
	/// <param name="event">The <see cref="MouseButtonEvent"/> to store into the newly created <see cref="Event"/></param>
	public Event(in MouseButtonEvent @event) :
#pragma warning disable IDE0034 // Leave it for explicitness sake
		this(default(IUnsafeConstructorDispatch?))
#pragma warning restore IDE0034
		=> Button = @event;
}

/// <summary>
/// Represents an event that occurs when a mouse button is being <see cref="EventType.MouseButtonDown">pressed</see> or <see cref="EventType.MouseButtonUp">released</see>
/// </summary>
/// <remarks>
/// <para>
/// Associated <see cref="EventType"/>s:
/// <list type="bullet">
/// <item><description><see cref="EventType.MouseButtonDown"/></description></item>
/// <item><description><see cref="EventType.MouseButtonUp"/></description></item>
/// </list>
/// </para>
/// </remarks>
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
[StructLayout(LayoutKind.Sequential)]
public struct MouseButtonEvent : ICommonEvent<MouseButtonEvent>, IFormattable, ISpanFormattable
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly string DebuggerDisplay => ToString(formatProvider: CultureInfo.InvariantCulture);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private static bool Accepts(EventType type) => type is EventType.MouseButtonDown or EventType.MouseButtonUp;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static bool ICommonEvent<MouseButtonEvent>.Accepts(EventType type) => Accepts(type);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static ref MouseButtonEvent ICommonEvent<MouseButtonEvent>.GetReference(ref Event @event) => ref @event.Button;

	private CommonEvent mCommon;
	private uint mWindowID;
	private uint mWhich;
	private MouseButton mButton;
	private CBool mDown;
	private byte mClicks;
	private readonly byte mPadding;
	private float mX;
	private float mY;

	/// <remarks>
	/// <para>
	/// When setting this property, the value must be either <see cref="EventType.MouseButtonDown"/> or <see cref="EventType.MouseButtonUp"/>.
	/// Otherwise, it will lead the property to throw an <see cref="ArgumentException"/>!
	/// </para>
	/// </remarks>
	/// <exception cref="ArgumentException">
	/// When setting this property, the value was neither <see cref="EventType.MouseButtonDown"/> nor <see cref="EventType.MouseButtonUp"/>
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
			static void failValueArgumentIsNotValid() => throw new ArgumentException($"The given {nameof(value)} is not a valid value for the {nameof(Type)} of a {nameof(MouseButtonEvent)}", paramName: nameof(value));
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
	/// Gets or sets the mouse button
	/// </summary>
	/// <value>
	/// The mouse button
	/// </value>
	public MouseButton Button
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

	/// <summary>
	/// Gets or sets the number of repeated clicks
	/// </summary>
	/// <value>
	/// The number of repeated clicks
	/// </value>
	/// <remarks>
	/// <para>
	/// <c>1</c> for a single-click, <c>2</c> for a double-click, etc.
	/// </para>
	/// </remarks>
	public byte Clicks
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mClicks;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mClicks = value;
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
							   .Append($", {nameof(Button)}: ")
							   .Append(Button)
							   .Append($", {nameof(IsDown)}: ")
							   .Append(IsDown)
							   .Append($", {nameof(X)}: ")
							   .Append(X.ToString(format, formatProvider))
							   .Append($", {nameof(Y)}: ")
							   .Append(Y.ToString(format, formatProvider))
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
			&& SpanFormat.TryWrite($", {nameof(Button)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(Button, ref destination, ref charsWritten)
			&& SpanFormat.TryWrite($", {nameof(IsDown)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(IsDown, ref destination, ref charsWritten)
			&& SpanFormat.TryWrite($", {nameof(X)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(X, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite($", {nameof(Y)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(Y, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite(" }", ref destination, ref charsWritten);
	}

	/// <<inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static implicit operator Event(in MouseButtonEvent @event) => new(in @event);

	/// <remarks>
	/// <para>
	/// The <see cref="Event.Type"/> of the given <paramref name="event"/> must be either <see cref="EventType.MouseButtonDown"/> or <see cref="EventType.MouseButtonUp"/>.
	/// Otherwise, it will lead the method to throw an <see cref="ArgumentException"/>!
	/// </para>
	/// </remarks>
	/// <exception cref="ArgumentException">
	/// The <see cref="Event.Type"/> of the given <paramref name="event"/> was neither <see cref="EventType.MouseButtonDown"/> nor <see cref="EventType.MouseButtonUp"/>
	/// </exception>
	/// <inheritdoc/>
	public static explicit operator MouseButtonEvent(in Event @event)
	{
		if (!Accepts(@event.Type))
		{
			failEventArgumentIsNotMouseButtonEvent();
		}

		return @event.Button;

		[DoesNotReturn]
		static void failEventArgumentIsNotMouseButtonEvent() => throw new ArgumentException($"{nameof(@event)} must be a {nameof(MouseButtonEvent)} by {nameof(Type)}", paramName: nameof(@event));
	}
}
