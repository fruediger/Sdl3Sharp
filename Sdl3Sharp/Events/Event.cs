using Sdl3Sharp.Internal;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sdl3Sharp.Events;

/// <summary>
/// Represents a general event in SDL
/// </summary>
/// <remarks>
/// <para>
/// Even though SDL's *Event structures do not use inheritance (as they are all <c><see langword="struct"/></c>s or <see cref="ValueType"/>s),
/// you may think of <see cref="Event"/> as a common base <em>representation</em> of all the other *Event structures.
/// </para>
/// <para>
/// More specifically: all of the other *Event structures <em>can</em> be represented as an <see cref="Event"/>
/// (this is usually achieved through <see cref="ICommonEvent{TSelf}.implicit operator Event(in TSelf)"/>),
/// while an <see cref="Event"/> structure <em>could</em> potentially be represented as one of the *Event structures, depending on it's <see cref="Type"/>
/// (this can be achieved through <see cref="ICommonEvent{TSelf}.explicit operator TSelf(in Event)"/>
/// or through <see cref="EventRefExtensions.TryAs{TEvent}(Sdl3Sharp.Events.EventRef{Sdl3Sharp.Events.Event}, Sdl3Sharp.Events.EventType{TEvent}, out Sdl3Sharp.Events.EventRef{TEvent})"/>, or <see cref="EventRefExtensions.TryAs{TEvent}(Sdl3Sharp.Events.EventRefReadOnly{Sdl3Sharp.Events.Event}, Sdl3Sharp.Events.EventType{TEvent}, out Sdl3Sharp.Events.EventRefReadOnly{TEvent})"/>
/// when dealing with <see cref="EventRef{TEvent}">references</see> to an <see cref="Event"/>)
/// </para>
/// </remarks>
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
[StructLayout(LayoutKind.Explicit)]
public partial struct Event : ICommonEvent<Event>, IFormattable, ISpanFormattable
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly string DebuggerDisplay => ToString(formatProvider: CultureInfo.InvariantCulture);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static bool ICommonEvent<Event>.Accepts(EventType type) => true;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static ref Event ICommonEvent<Event>.GetReference(ref Event @event) => ref @event;

	[FieldOffset(0)] private readonly Padding mPadding;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private Event(IUnsafeConstructorDispatch? _ = default) => Unsafe.SkipInit(out this);

	/// <inheritdoc/>
	public EventType<Event> Type
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => Common.Type;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => Common.Type = value;
	}

	/// <inheritdoc/>
	public ulong Timestamp
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => Common.Timestamp;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => Common.Timestamp = value;
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

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static implicit ICommonEvent<Event>.operator Event(in Event @event) => @event;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static explicit ICommonEvent<Event>.operator Event(in Event @event) => @event;	

	[InlineArray(128)] private struct Padding { private byte _; }

	private interface IUnsafeConstructorDispatch;
}
