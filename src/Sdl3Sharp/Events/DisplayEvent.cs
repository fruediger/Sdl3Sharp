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

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public Event(in DisplayEvent @event) : this(default(IUnsafeConstructorDispatch?)) => Display = @event;
}

[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
[StructLayout(LayoutKind.Sequential)]
public struct DisplayEvent : ICommonEvent<DisplayEvent>, IFormattable, ISpanFormattable
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly string DebuggerDisplay => ToString(formatProvider: CultureInfo.InvariantCulture);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private static bool Accepts(EventType type) => type >= EventType.Display.OrientationChanged && type <= EventType.Display.ContentScaleChanged;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static bool ICommonEvent<DisplayEvent>.Accepts(EventType type) => Accepts(type);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static ref DisplayEvent ICommonEvent<DisplayEvent>.GetReference(ref Event @event) => ref @event.Display;

	private CommonEvent<DisplayEvent> mCommon;
	private uint mDisplayID;
	private int mData1;
	private int mData2;
	
	/// <inheritdoc/>
	public EventType<DisplayEvent> Type
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mCommon.Type;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mCommon.Type = value;
	}

	public ulong Timestamp
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mCommon.Timestamp;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mCommon.Timestamp = value;
	}

	public uint DisplayId
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mDisplayID;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mDisplayID = value;
	}

	public int Data1
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mData1;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mData1 = value;
	}

	public int Data2
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mData2;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mData2 = value;
	}	

	public readonly override string ToString() => ToString(format: default, formatProvider: default);

	public readonly string ToString(IFormatProvider? formatProvider) => ToString(format: default, formatProvider);

	public readonly string ToString(string? format) => ToString(format, formatProvider: default);

	public readonly string ToString(string? format, IFormatProvider? formatProvider)
		=> $"{{ {ICommonEvent.PartialToString(in this, format, formatProvider)}, {
			nameof(DisplayId)}: {DisplayId.ToString(format, formatProvider)}, {
			nameof(Data1)}: {Data1.ToString(format, formatProvider)}, {
			nameof(Data2)}: {Data2.ToString(format, formatProvider)} }}";

	public readonly bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format = default, IFormatProvider? provider = default)
	{
		charsWritten = 0;

		return SpanFormat.TryWrite("{ ", ref destination, ref charsWritten)
			&& ICommonEvent.TryPartialFormat(in this, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite($", {nameof(DisplayId)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(DisplayId, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite($", {nameof(Data1)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(Data1, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite($", {nameof(Data2)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(Data2, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite(" }", ref destination, ref charsWritten);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static implicit operator Event(in DisplayEvent @event) => new(in @event);

	public static explicit operator DisplayEvent(in Event @event)
	{
		if (!Accepts(@event.Type))
		{
			failEventArgumentIsNotDisplayEvent();
		}

		return @event.Display;

		[DoesNotReturn]
		static void failEventArgumentIsNotDisplayEvent() => throw new ArgumentException($"{nameof(@event)} must be an {nameof(DisplayEvent)} by {nameof(Type)}", paramName: nameof(@event));
	}
}
