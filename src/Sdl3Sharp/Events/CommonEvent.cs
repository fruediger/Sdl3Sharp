using Sdl3Sharp.Internal;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sdl3Sharp.Events;

partial struct Event
{	
	[FieldOffset(0)] internal CommonEvent<Event> Common;	

#pragma warning disable IDE0034 // Leave it for explicitness sake
	internal Event(in CommonEvent<Event> @event) : this(default(IUnsafeConstructorDispatch?)) => Common = @event;
#pragma warning restore IDE0034
}

[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
[StructLayout(LayoutKind.Sequential)]
internal struct CommonEvent<TEvent> : ICommonEvent<CommonEvent<TEvent>>, IFormattable, ISpanFormattable
	where TEvent : struct, ICommonEvent<TEvent>
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly string DebuggerDisplay => ToString(formatProvider: CultureInfo.InvariantCulture);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static bool ICommonEvent<CommonEvent<TEvent>>.Accepts(EventType type) => true;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static ref CommonEvent<TEvent> ICommonEvent<CommonEvent<TEvent>>.GetReference(ref Event @event) => ref Unsafe.As<CommonEvent<Event>, CommonEvent<TEvent>>(ref @event.Common);

	private EventType<TEvent> mType;
	private readonly uint mReserved;
	private ulong mTimestamp;

	/// <inheritdoc cref="ICommonEvent{TSelf}.Type"/>
	public EventType<TEvent> Type
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mType;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mType = value;
	}
	
	/// <inheritdoc/>
	/// <inheritdoc cref="EventType{TEvent}.explicit operator EventType{TEvent}(EventType)"/>
	EventType<CommonEvent<TEvent>> ICommonEvent<CommonEvent<TEvent>>.Type
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => (EventType<CommonEvent<TEvent>>)(EventType)mType;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mType = (EventType<TEvent>)(EventType)value;
	}

	/// <inheritdoc/>
	public ulong Timestamp
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mTimestamp;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mTimestamp = value;
	}

	/// <inheritdoc/>
	public readonly override string ToString() => ToString(format: default, formatProvider: default);

	/// <inheritdoc cref="IFormattable.ToString(string?, IFormatProvider?)"/>
	public readonly string ToString(IFormatProvider? formatProvider) => ToString(format: default, formatProvider);

	/// <inheritdoc cref="IFormattable.ToString(string?, IFormatProvider?)"/>
	public readonly string ToString(string? format) => ToString(format, formatProvider: default);

	/// <inheritdoc/>
	public readonly string ToString(string? format, IFormatProvider? formatProvider)
		=> $"{{ {ICommonEvent.PartialToString(in this, format, formatProvider)} }}";

	/// <inheritdoc/>
	public readonly bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format = default, IFormatProvider? provider = default)
	{
		charsWritten = 0;

		return SpanFormat.TryWrite("{ ", ref destination, ref charsWritten)
			&& ICommonEvent.TryPartialFormat(in this, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite(" }", ref destination, ref charsWritten);
	}

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static implicit operator Event(in CommonEvent<TEvent> @event) => new(in Unsafe.As<CommonEvent<TEvent>, CommonEvent<Event>>(ref Unsafe.AsRef(in @event)));

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static explicit operator CommonEvent<TEvent>(in Event @event) => Unsafe.As<CommonEvent<Event>, CommonEvent<TEvent>>(ref Unsafe.AsRef(in @event.Common));
}
