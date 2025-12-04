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
	[FieldOffset(0)] internal DisplayEvent Display;

	/// <summary>
	/// Creates a new <see cref="Event"/> from a <see cref="DisplayEvent"/>
	/// </summary>
	/// <param name="event">The <see cref="DisplayEvent"/> to store into the newly created <see cref="Event"/></param>
	public Event(in DisplayEvent @event) :
#pragma warning disable IDE0034 // Leave it for explicitness sake
		this(default(IUnsafeConstructorDispatch?))
#pragma warning restore IDE0034
		=> Display = @event;
}

/// <summary>
/// Represents an event that occurs when a <see cref="Display"/> changes its state
/// </summary>
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
[StructLayout(LayoutKind.Sequential)]
public struct DisplayEvent : ICommonEvent<DisplayEvent>, IFormattable, ISpanFormattable
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly string DebuggerDisplay => ToString(formatProvider: CultureInfo.InvariantCulture);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private static bool Accepts(EventType type) => type is >= EventType.DisplayOrientationChanged and <= EventType.DisplayUsableBoundsChanged;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static bool ICommonEvent<DisplayEvent>.Accepts(EventType type) => Accepts(type);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static ref DisplayEvent ICommonEvent<DisplayEvent>.GetReference(ref Event @event) => ref @event.Display;

	private CommonEvent mCommon;
	private uint mDisplayID;
	private int mData1;
	private int mData2;

	/// <remarks>
	/// <para>
	/// When setting this property, the value must be one of the <see cref="EventType"/>.Display* values.
	/// Otherwise, it will lead the property to throw an <see cref="ArgumentException"/>!
	/// </para>
	/// </remarks>
	/// <exception cref="ArgumentException">
	/// When setting this property, the value was not one of the <see cref="EventType"/>.Display* values
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
			static void failValueArgumentIsNotValid() => throw new ArgumentException($"The given {nameof(value)} is not a valid value for the {nameof(Type)} of a {nameof(DisplayEvent)}", paramName: nameof(value));
		}
	}

	/// <inheritdoc/>
	public ulong Timestamp
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mCommon.Timestamp;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mCommon.Timestamp = value;
	}

	/// <summary>
	/// Gets or sets the display ID of the <see cref="Display"/> which changes it's state
	/// </summary>
	/// <value>
	/// The display ID of the <see cref="Display"/> which changes it's state
	/// </value>
	public uint DisplayId
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mDisplayID;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mDisplayID = value;
	}

	/// <summary>
	/// Gets or sets the value of first event dependent data slot
	/// </summary>
	/// <value>
	/// The value of the first event dependent data slot
	/// </value>
	/// <remarks>
	/// <para>
	/// The value of this property may reflect different data semantics dependent on the actual <see cref="Type"/>.
	/// </para>
	/// </remarks>
	public int Data1
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mData1;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mData1 = value;
	}

	/// <summary>
	/// Gets or sets the value of second event dependent data slot
	/// </summary>
	/// <value>
	/// The value of the second event dependent data slot
	/// </value>
	/// <remarks>
	/// <para>
	/// The value of this property may reflect different data semantics dependent on the actual <see cref="Type"/>.
	/// </para>
	/// </remarks>
	public int Data2
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mData2;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mData2 = value;
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
							   .Append($", {nameof(DisplayId)}: ")
							   .Append(DisplayId.ToString(format, formatProvider))
							   .Append($", {nameof(Data1)}: ")
							   .Append(Data1.ToString(format, formatProvider))
							   .Append($", {nameof(Data2)}: ")
							   .Append(Data2.ToString(format, formatProvider))
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
			&& SpanFormat.TryWrite($", {nameof(DisplayId)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(DisplayId, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite($", {nameof(Data1)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(Data1, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite($", {nameof(Data2)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(Data2, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite(" }", ref destination, ref charsWritten);
	}

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static implicit operator Event(in DisplayEvent @event) => new(in @event);

	/// <remarks>
	/// <para>
	/// The <see cref="Event.Type"/> of the given <paramref name="event"/> must be one of the <see cref="EventType"/>.Display* values.
	/// Otherwise, it will lead the method to throw an <see cref="ArgumentException"/>!
	/// </para>
	/// </remarks>
	/// <exception cref="ArgumentException">
	/// The <see cref="Event.Type"/> of the given <paramref name="event"/> was not one of the <see cref="EventType"/>.Display* values
	/// </exception>
	/// <inheritdoc/>
	public static explicit operator DisplayEvent(in Event @event)
	{
		if (!Accepts(@event.Type))
		{
			failEventArgumentIsNotDisplayEvent();
		}

		return @event.Display;

		[DoesNotReturn]
		static void failEventArgumentIsNotDisplayEvent() => throw new ArgumentException($"{nameof(@event)} must be a {nameof(DisplayEvent)} by {nameof(Type)}", paramName: nameof(@event));
	}
}
