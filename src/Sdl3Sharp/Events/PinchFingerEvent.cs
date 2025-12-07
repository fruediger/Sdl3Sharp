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
	[FieldOffset(0)] internal PinchFingerEvent Pinch;

	/// <summary>
	/// Creates a new <see cref="Event"/> from a <see cref="PinchFingerEvent"/>
	/// </summary>
	/// <param name="event">The <see cref="PinchFingerEvent"/> to store into the newly created <see cref="Event"/></param>
	public Event(in PinchFingerEvent @event) :
#pragma warning disable IDE0034 // Leave it for explicitness sake
		this(default(IUnsafeConstructorDispatch?))
#pragma warning restore IDE0034
		=> Pinch = @event;
}

/// <summary>
/// Represents an event that occurs when a pinch gesture is detected on a touch device
/// </summary>
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
[StructLayout(LayoutKind.Sequential)]
public struct PinchFingerEvent : ICommonEvent<PinchFingerEvent>, IFormattable, ISpanFormattable
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly string DebuggerDisplay => ToString(formatProvider: CultureInfo.InvariantCulture);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private static bool Accepts(EventType type) => type is >= EventType.PinchBegin and <= EventType.PinchEnd;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static bool ICommonEvent<PinchFingerEvent>.Accepts(EventType type) => Accepts(type);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static ref PinchFingerEvent ICommonEvent<PinchFingerEvent>.GetReference(ref Event @event) => ref @event.Pinch;

	private CommonEvent mCommon;
	private float mScale;
	private uint mWindowID;

	/// <remarks>
	/// <para>
	/// When setting this property, the value must be either <see cref="EventType.PinchBegin"/>, <see cref="EventType.PinchUpdate"/>, or <see cref="EventType.PinchEnd"/>.
	/// Otherwise, it will lead the property to throw an <see cref="ArgumentException"/>!
	/// </para>
	/// </remarks>
	/// <exception cref="ArgumentException">
	/// When setting this property, the value was neither <see cref="EventType.PinchBegin"/>, <see cref="EventType.PinchUpdate"/>, nor <see cref="EventType.PinchEnd"/>
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
			static void failValueArgumentIsNotValid() => throw new ArgumentException($"The given {nameof(value)} is not a valid value for the {nameof(Type)} of a {nameof(PinchFingerEvent)}", paramName: nameof(value));
		}
	}

	/// <inheritdoc/>
	public ulong Timestamp
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mCommon.Timestamp;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mCommon.Timestamp = value;
	}

	/// <summary>
	/// Gets or sets the scale <em>change</em> since the last <see cref="EventType.PinchUpdate"/> event
	/// </summary>
	/// <value>
	/// The scale <em>change</em> since the last <see cref="EventType.PinchUpdate"/> event, where values less than <c>1</c> mean "zoom-out" and values greater than <c>1</c> mean a "zoom-in"
	/// </value>
	public float Scale
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mScale;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mScale = value;
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
							   .Append($", {nameof(Scale)}: ")
							   .Append(Scale.ToString(format, formatProvider))
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
			&& SpanFormat.TryWrite($", {nameof(Scale)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(Scale, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite($", {nameof(WindowId)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(WindowId, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite(" }", ref destination, ref charsWritten);
	}

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static implicit operator Event(in PinchFingerEvent @event) => new(in @event);

	/// <remarks>
	/// <para>
	/// The <see cref="Event.Type"/> of the given <paramref name="event"/> must be either <see cref="EventType.PinchBegin"/>, <see cref="EventType.PinchUpdate"/>, or <see cref="EventType.PinchEnd"/>.
	/// Otherwise, it will lead the method to throw an <see cref="ArgumentException"/>!
	/// </para>
	/// </remarks>
	/// <exception cref="ArgumentException">
	/// The <see cref="Event.Type"/> of the given <paramref name="event"/> was neither <see cref="EventType.PinchBegin"/>, <see cref="EventType.PinchUpdate"/>, nor <see cref="EventType.PinchEnd"/>
	/// </exception>
	/// <inheritdoc/>
	public static explicit operator PinchFingerEvent(in Event @event)
	{
		if (!Accepts(@event.Type))
		{
			failEventArgumentIsNotPinchFingerEvent();
		}

		return @event.Pinch;

		[DoesNotReturn]
		static void failEventArgumentIsNotPinchFingerEvent() => throw new ArgumentException($"{nameof(@event)} must be a {nameof(PinchFingerEvent)} by {nameof(Type)}", paramName: nameof(@event));
	}
}
