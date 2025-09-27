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
	[FieldOffset(0)] internal KeyboardEvent Key;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public Event(in KeyboardEvent @event) : this(default(IUnsafeConstructorDispatch?)) => Key = @event;
}

[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
[StructLayout(LayoutKind.Sequential)]
public struct KeyboardEvent : ICommonEvent<KeyboardEvent>, IFormattable, ISpanFormattable
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly string DebuggerDisplay => ToString(formatProvider: CultureInfo.InvariantCulture);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private static bool Accepts(EventType type) => type == EventType.Keyboard.KeyDown || type == EventType.Keyboard.KeyUp;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static bool ICommonEvent<KeyboardEvent>.Accepts(EventType type) => Accepts(type);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static ref KeyboardEvent ICommonEvent<KeyboardEvent>.GetReference(ref Event @event) => ref @event.Key;

	private CommonEvent<KeyboardEvent> mCommon;
	private uint mWindowID;
	private uint mWhich;
	private Scancode mScancode;
	private Keycode mKey;
	private Keymod mMod;
	private ushort mRaw;
	private CBool mDown;
	private CBool mRepeat;

	/// <inheritdoc/>
	public EventType<KeyboardEvent> Type
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mCommon.Type;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mCommon.Type = value;
	}

	public ulong Timestamp
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mCommon.Timestamp;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mCommon.Timestamp = value;
	}

	public uint WindowId
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mWindowID;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mWindowID = value;
	}

	public uint KeyboardId
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mWhich;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mWhich = value;
	}

	public Scancode Scancode
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mScancode;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mScancode = value;
	}

	public Keycode Keycode
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mKey;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mKey = value;
	}

	public Keymod Modifier
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mMod;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mMod = value;
	}

	public ushort Raw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mRaw;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mRaw = value;
	}

	public bool IsDown
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mDown;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mDown = value;
	}

	public bool IsRepeat
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mRepeat;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mRepeat = value;
	}	

	public readonly override string ToString() => ToString(format: default, formatProvider: default);

	public readonly string ToString(IFormatProvider? formatProvider) => ToString(format: default, formatProvider);

	public readonly string ToString(string? format) => ToString(format, formatProvider: default);

	public readonly string ToString(string? format, IFormatProvider? formatProvider)
		=> $"{{ {ICommonEvent.PartialToString(in this, format, formatProvider)}, {
			nameof(WindowId)}: {WindowId.ToString(format, formatProvider)}, {
			nameof(KeyboardId)}: {KeyboardId.ToString(format, formatProvider)}, {
			nameof(Scancode)}: {Scancode}, {
			nameof(Keycode)}: {Keycode}, {
			nameof(Modifier)}: {Modifier}, {
			nameof(Raw)}: {Raw.ToString(format, formatProvider)}, {
			nameof(IsDown)}: {IsDown}, {
			nameof(IsRepeat)}: {IsRepeat} }}";

	public readonly bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format = default, IFormatProvider? provider = default)
	{
		charsWritten = 0;

		return SpanFormat.TryWrite("{ ", ref destination, ref charsWritten)
			&& ICommonEvent.TryPartialFormat(in this, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite($", {nameof(WindowId)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(WindowId, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite($", {nameof(KeyboardId)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(KeyboardId, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite($", {nameof(Scancode)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(Scancode, ref destination, ref charsWritten)
			&& SpanFormat.TryWrite($", {nameof(Keycode)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(Keycode, ref destination, ref charsWritten)
			&& SpanFormat.TryWrite($", {nameof(Modifier)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(Modifier, ref destination, ref charsWritten)
			&& SpanFormat.TryWrite($", {nameof(Raw)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(Raw, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite($", {nameof(IsDown)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(IsDown, ref destination, ref charsWritten)
			&& SpanFormat.TryWrite($", {nameof(IsRepeat)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(IsRepeat, ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(" }", ref destination, ref charsWritten);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static implicit operator Event(in KeyboardEvent @event) => new(in @event);

	public static explicit operator KeyboardEvent(in Event @event)
	{
		if (!Accepts(@event.Type))
		{
			failEventArgumentIsNotKeyboardEvent();
		}

		return @event.Key;

		[DoesNotReturn]
		static void failEventArgumentIsNotKeyboardEvent() => throw new ArgumentException($"{nameof(@event)} must be a {nameof(KeyboardEvent)} by {nameof(Type)}", paramName: nameof(@event));
	}
}
