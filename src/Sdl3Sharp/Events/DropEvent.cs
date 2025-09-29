using Sdl3Sharp.Internal;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace Sdl3Sharp.Events;

partial struct Event
{
	[FieldOffset(0)] internal DropEvent Drop;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public Event(in DropEvent @event) : this(default(IUnsafeConstructorDispatch?)) => Drop = @event;
}

[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
[StructLayout(LayoutKind.Sequential)]
public struct DropEvent : ICommonEvent<DropEvent>, IFormattable, ISpanFormattable
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly string DebuggerDisplay => ToString(formatProvider: CultureInfo.InvariantCulture);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private static bool Accepts(EventType type) => type >= EventType.DragAndDrop.Begin && type <= EventType.DragAndDrop.Position;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static bool ICommonEvent<DropEvent>.Accepts(EventType type) => Accepts(type);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static ref DropEvent ICommonEvent<DropEvent>.GetReference(ref Event @event) => ref @event.Drop;

	private CommonEvent<DropEvent> mCommon;
	private uint mWindowID;
	private float mX;
	private float mY;
	private readonly unsafe byte* mSource;
	private readonly unsafe byte* mData;

	/// <inheritdoc/>
	public EventType<DropEvent> Type
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

	public float X
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mX;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mX = value;
	}

	public float Y
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mY;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mY = value;
	}

	public string? Source
	{
		readonly get
		{
			unsafe
			{
				return Utf8StringMarshaller.ConvertToManaged(mSource);
			}
		}

		[DoesNotReturn, Experimental("SDL2001")] // TODO: make SDL2001 the diagnostic id for 'Yet not supported API'
		set => throw new NotImplementedException($"Setting {nameof(Source)} is yet not supported.");
	}

	public string? Data
	{
		readonly get
		{
			unsafe
			{
				return Utf8StringMarshaller.ConvertToManaged(mData);
			}
		}

		[DoesNotReturn, Experimental("SDL2001")] // TODO: make SDL2001 the diagnostic id for 'Yet not supported API'
		set => throw new NotImplementedException($"Setting {nameof(Data)} is yet not supported.");
	}

	public readonly override string ToString() => ToString(format: default, formatProvider: default);

	public readonly string ToString(IFormatProvider? formatProvider) => ToString(format: default, formatProvider);

	public readonly string ToString(string? format) => ToString(format, formatProvider: default);

	public readonly string ToString(string? format, IFormatProvider? formatProvider)
		=> $"{{ {ICommonEvent.PartialToString(in this, format, formatProvider)}, {
			nameof(WindowId)}: {WindowId.ToString(format, formatProvider)}, {
			nameof(X)}: {X.ToString(format, formatProvider)}, {
			nameof(Y)}: {Y.ToString(format, formatProvider)}, {
			nameof(Source)}: {Source switch { null => "null", var source => $"\"{source}\"" }}, {
			nameof(Data)}: {Data switch { null => "null", var data => $"\"{data}\"" }} }}";

	public readonly bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format = default, IFormatProvider? provider = default)
	{
		charsWritten = 0;

		if ( !(SpanFormat.TryWrite("{ ", ref destination, ref charsWritten)
			&& ICommonEvent.TryPartialFormat(in this, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite($", {nameof(WindowId)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(WindowId, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite($", {nameof(X)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(X, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite($", {nameof(Y)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(Y, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite($", {nameof(Source)}: ", ref destination, ref charsWritten)))
		{
			return false;
		}

		var source = Source;

		if (source is not null)
		{
			if ( !(SpanFormat.TryWrite('"', ref destination, ref charsWritten)
				&& SpanFormat.TryWrite(source, ref destination, ref charsWritten)
				&& SpanFormat.TryWrite('"', ref destination, ref charsWritten)))
			{
				return false;
			}
		}
		else
		{
			if (!SpanFormat.TryWrite("null", ref destination, ref charsWritten))
			{
				return false;
			}
		}

		if (!SpanFormat.TryWrite($", {nameof(Data)}: ", ref destination, ref charsWritten))
		{
			return false;
		}

		var data = Data;

		if (data is not null)
		{
			if ( !(SpanFormat.TryWrite('"', ref destination, ref charsWritten)
				&& SpanFormat.TryWrite(data, ref destination, ref charsWritten)
				&& SpanFormat.TryWrite('"', ref destination, ref charsWritten)))
			{
				return false;
			}
		}
		else
		{
			if (!SpanFormat.TryWrite("null", ref destination, ref charsWritten))
			{
				return false;
			}
		}

		return SpanFormat.TryWrite(" }", ref destination, ref charsWritten);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static implicit operator Event(in DropEvent @event) => new(in @event);

	public static explicit operator DropEvent(in Event @event)
	{
		if (!Accepts(@event.Type))
		{
			failEventArgumentIsNotDropEvent();
		}

		return @event.Drop;

		[DoesNotReturn]
		static void failEventArgumentIsNotDropEvent() => throw new ArgumentException($"{nameof(@event)} must be an {nameof(DropEvent)} by {nameof(Type)}", paramName: nameof(@event));
	}
}
