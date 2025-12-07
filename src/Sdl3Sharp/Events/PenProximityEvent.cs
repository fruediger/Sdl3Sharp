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
	[FieldOffset(0)] internal PenProximityEvent PProximity;

	/// <summary>
	/// Creates a new <see cref="Event"/> from a <see cref="PenProximityEvent"/>
	/// </summary>
	/// <param name="event">The <see cref="PenProximityEvent"/> to store into the newly created <see cref="Event"/></param>
	public Event(in PenProximityEvent @event) :
#pragma warning disable IDE0034 // Leave it for explicitness sake
		this(default(IUnsafeConstructorDispatch?))
#pragma warning restore IDE0034
		=> PProximity = @event;
}

/// <summary>
/// Represents an event that occurs when a <see cref="Pen">pen</see> <see cref="EventType.PenProximityIn">enters</see> or <see cref="EventType.PenProximityOut">leaves</see> proximity
/// </summary>
/// <remarks>
/// <para>
/// When a pen becomes visible to the system (it is close enough to a tablet, etc), SDL will send a <see cref="EventType.PenProximityIn"/> event with the new pen's ID.
/// This ID is valid until the pen leaves proximity again (has been removed from the tablet's area, the tablet has been unplugged, etc).
/// If the same pen reenters proximity again, it will be given a new ID.
/// </para>
/// <para>
/// Note that "proximity" means "close enough for the tablet to know the tool is there."
/// The pen touching and lifting off from the tablet while not leaving the area are handled by <see cref="EventType.PenDown"/> and <see cref="EventType.PenUp"/> events (<see cref="PenTouchEvent"/>).
/// </para>
/// <para>
/// Notice that not all platforms have a window associated with the pen during proximity events.
/// Some wait until <see cref="PenMotionEvent">motion</see>/<see cref="PenButtonEvent">button</see>/etc. events to offer this info.
/// </para>
/// <para>
/// Associated <see cref="EventType"/>s:
/// <list type="bullet">
/// <item><description><see cref="EventType.PenProximityIn"/></description></item>
/// <item><description><see cref="EventType.PenProximityOut"/></description></item>
/// </list>
/// </para>
/// </remarks>
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
[StructLayout(LayoutKind.Sequential)]
public struct PenProximityEvent : ICommonEvent<PenProximityEvent>, IFormattable, ISpanFormattable
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly string DebuggerDisplay => ToString(formatProvider: CultureInfo.InvariantCulture);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private static bool Accepts(EventType type) => type is EventType.PenProximityIn or EventType.PenProximityOut;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static bool ICommonEvent<PenProximityEvent>.Accepts(EventType type) => Accepts(type);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static ref PenProximityEvent ICommonEvent<PenProximityEvent>.GetReference(ref Event @event) => ref @event.PProximity;

	private CommonEvent mCommon;
	private uint mWindowID;
	private uint mWhich;

	/// <remarks>
	/// <para>
	/// When setting this property, the value must be either <see cref="EventType.PenProximityIn"/> or <see cref="EventType.PenProximityOut"/>.
	/// Otherwise, it will lead the property to throw an <see cref="ArgumentException"/>!
	/// </para>
	/// </remarks>
	/// <exception cref="ArgumentException">
	/// When setting this property, the value was neither <see cref="EventType.PenProximityIn"/> nor <see cref="EventType.PenProximityOut"/>
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
			static void failValueArgumentIsNotValid() => throw new ArgumentException($"The given {nameof(value)} is not a valid value for the {nameof(Type)} of a {nameof(PenProximityEvent)}", paramName: nameof(value));
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
	/// <remarks>
	/// <value>
	/// If a pen leaves proximity and later re-enters proximity, it may be assigned a different device ID.
	/// </value>
	/// </remarks>
	public uint PenId
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mWhich;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mWhich = value;
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
			&& SpanFormat.TryWrite(" }", ref destination, ref charsWritten);
	}

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static implicit operator Event(in PenProximityEvent @event) => new(in @event);

	/// <remarks>
	/// <para>
	/// The <see cref="Event.Type"/> of the given <paramref name="event"/> must be either <see cref="EventType.PenProximityIn"/>, or <see cref="EventType.PenProximityOut"/>.
	/// Otherwise, it will lead the method to throw an <see cref="ArgumentException"/>!
	/// </para>
	/// </remarks>
	/// <exception cref="ArgumentException">
	/// The <see cref="Event.Type"/> of the given <paramref name="event"/> was neither <see cref="EventType.PenProximityIn"/>, nor <see cref="EventType.PenProximityOut"/>
	/// </exception>
	/// <inheritdoc/>
	public static explicit operator PenProximityEvent(in Event @event)
	{
		if (!Accepts(@event.Type))
		{
			failEventArgumentIsNotPenProximityEvent();
		}

		return @event.PProximity;

		[DoesNotReturn]
		static void failEventArgumentIsNotPenProximityEvent() => throw new ArgumentException($"{nameof(@event)} must be a {nameof(PenProximityEvent)} by {nameof(Type)}", paramName: nameof(@event));
	}
}
