using Sdl3Sharp.Internal;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
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
/// (this can be achieved through <see cref="ICommonEvent{TSelf}.explicit operator TSelf(in Event)"/>,
/// or copyless through <see cref="EventExtensions.TryAs{TEvent}(ref Event, out Sdl3Sharp.Utilities.NullableRef{TEvent})"/> or <see cref="EventExtensions.TryAsReadOnly{TEvent}(ref readonly Event, out Sdl3Sharp.Utilities.NullableRefReadOnly{TEvent})"/>)
/// </para>
/// </remarks>
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
[StructLayout(LayoutKind.Explicit)]
public partial struct Event : ICommonEvent<Event>, IFormattable, ISpanFormattable
{
	internal interface IUnsafeConstructorDispatch;

	[InlineArray(128)] private struct Padding { private byte _; }

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly string DebuggerDisplay => ToString(formatProvider: CultureInfo.InvariantCulture);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static bool ICommonEvent<Event>.Accepts(EventType type) => true; // Event accepts all EventTypes

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static ref Event ICommonEvent<Event>.GetReference(ref Event @event) => ref @event;

	[FieldOffset(0)] private readonly Padding mPadding;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	internal Event(IUnsafeConstructorDispatch? _ = default) => Unsafe.SkipInit(out this);

	/// <remarks>
	/// <para>
	/// Do not attempt to set this property on a base <see cref="Event"/>, instead set the <see cref="ICommonEvent.Type"/> property on an instance of a more specialized *Event structure.
	/// Setting this property on a base <see cref="Event"/> is not supported and will lead the property to throw a <see cref="NotSupportedException"/>.
	/// </para>
	/// </remarks>
	/// <exception cref="NotSupportedException">When setting this property</exception>
	/// <inheritdoc/>
	public EventType Type
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => Common.Type;

		[Obsolete($"Setting the {nameof(Type)} of a base {nameof(Event)} is not supported.")]
		[DoesNotReturn]
		set => throw new NotSupportedException($"Setting the {nameof(Type)} of a base {nameof(Event)} is not supported");
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
	{
		var builder = Shared.StringBuilder;
		try
		{
			return ICommonEvent.PartiallyAppend(in this, builder.Append("{ "), format)
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

		return SpanFormat.TryWrite(" {", ref destination, ref charsWritten)
			&& ICommonEvent.TryPartiallyFormat(in this, ref destination, ref charsWritten, format)
			&& SpanFormat.TryWrite(" }", ref destination, ref charsWritten);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static implicit ICommonEvent<Event>.operator Event(in Event @event) => @event;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static explicit ICommonEvent<Event>.operator Event(in Event @event) => @event;
}
