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
	[FieldOffset(0)] internal KeyboardDeviceEvent KDevice;
	
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public Event(in KeyboardDeviceEvent @event) : this(default(IUnsafeConstructorDispatch?)) => KDevice = @event;
}

[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
[StructLayout(LayoutKind.Sequential)]
public struct KeyboardDeviceEvent : ICommonEvent<KeyboardDeviceEvent>, IFormattable, ISpanFormattable
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly string DebuggerDisplay => ToString(formatProvider: CultureInfo.InvariantCulture);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private static bool Accepts(EventType type) => type == EventType.Keyboard.Added || type == EventType.Keyboard.Removed;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static bool ICommonEvent<KeyboardDeviceEvent>.Accepts(EventType type) => Accepts(type);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static ref KeyboardDeviceEvent ICommonEvent<KeyboardDeviceEvent>.GetReference(ref Event @event) => ref @event.KDevice;

	private CommonEvent<KeyboardDeviceEvent> mCommon;
	private uint mWhich;

	/// <inheritdoc/>
	public EventType<KeyboardDeviceEvent> Type
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mCommon.Type;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mCommon.Type = value;
	}

	public ulong Timestamp
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mCommon.Timestamp;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mCommon.Timestamp = value;
	}

	public uint KeyboardId
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mWhich;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mWhich = value;
	}

	public readonly override string ToString() => ToString(format: default, formatProvider: default);

	public readonly string ToString(IFormatProvider? formatProvider) => ToString(format: default, formatProvider);

	public readonly string ToString(string? format) => ToString(format, formatProvider: default);

	public readonly string ToString(string? format, IFormatProvider? formatProvider)
		=> $"{{ {ICommonEvent.PartialToString(in this, format, formatProvider)}, {
			nameof(KeyboardId)}: {KeyboardId.ToString(format, formatProvider)} }}";

	public readonly bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format = default, IFormatProvider? provider = default)
	{
		charsWritten = 0;

		return SpanFormat.TryWrite("{ ", ref destination, ref charsWritten)
			&& ICommonEvent.TryPartialFormat(in this, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite($", {nameof(KeyboardId)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(KeyboardId, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite(" }", ref destination, ref charsWritten);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static implicit operator Event(in KeyboardDeviceEvent @event) => new(in @event);

	public static explicit operator KeyboardDeviceEvent(in Event @event)
	{
		if (!Accepts(@event.Type))
		{
			failEventArgumentIsNotKeyboardDeviceEvent();
		}

		return @event.KDevice;

		[DoesNotReturn]
		static void failEventArgumentIsNotKeyboardDeviceEvent() => throw new ArgumentException($"{nameof(@event)} must be an {nameof(KeyboardDeviceEvent)} by {nameof(Type)}", paramName: nameof(@event));
	}
}
