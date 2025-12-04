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

	/// <summary>
	/// Creates a new <see cref="Event"/> from a <see cref="KeyboardEvent"/>
	/// </summary>
	/// <param name="event">The <see cref="KeyboardEvent"/> to store into the newly created <see cref="Event"/></param>
	public Event(in KeyboardEvent @event) :
#pragma warning disable IDE0034 // Leave it for explicitness sake
		this(default(IUnsafeConstructorDispatch?))
#pragma warning restore IDE0034
		=> Key = @event;
}

/// <summary>
/// Represents an event that occurs when a keyboard key is being <see cref="EventType.KeyDown">pressed</see> or <see cref="EventType.KeyUp">released</see>
/// </summary>
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
[StructLayout(LayoutKind.Sequential)]
public struct KeyboardEvent : ICommonEvent<KeyboardEvent>, IFormattable, ISpanFormattable
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly string DebuggerDisplay => ToString(formatProvider: CultureInfo.InvariantCulture);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private static bool Accepts(EventType type) => type is EventType.KeyDown or EventType.KeyUp;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static bool ICommonEvent<KeyboardEvent>.Accepts(EventType type) => Accepts(type);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static ref KeyboardEvent ICommonEvent<KeyboardEvent>.GetReference(ref Event @event) => ref @event.Key;

	private CommonEvent mCommon;
	private uint mWindowID;
	private uint mWhich;
	private Scancode mScancode;
	private Keycode mKey;
	private Keymod mMod;
	private ushort mRaw;
	private CBool mDown;
	private CBool mRepeat;

	/// <remarks>
	/// <para>
	/// When setting this property, the value must be either <see cref="EventType.KeyDown"/> or <see cref="EventType.KeyUp"/>.
	/// Otherwise, it will lead the property to throw an <see cref="ArgumentException"/>!
	/// </para>
	/// </remarks>
	/// <exception cref="ArgumentException">
	/// When setting this property, the value was neither <see cref="EventType.KeyDown"/> nor <see cref="EventType.KeyUp"/>
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
			static void failValueArgumentIsNotValid() => throw new ArgumentException($"The given {nameof(value)} is not a valid value for the {nameof(Type)} of a {nameof(KeyboardEvent)}", paramName: nameof(value));
		}
	}

	/// <inheritdoc/>
	public ulong Timestamp
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mCommon.Timestamp;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mCommon.Timestamp = value;
	}

	/// <summary>
	/// Gets or sets the window Id of the <see cref="Window"/> with keyboard focus, if any
	/// </summary>
	/// <value>
	/// The window Id of the <see cref="Window"/> with keyboard focus, if any, or <c>0</c>
	/// </value>
	public uint WindowId
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mWindowID;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mWindowID = value;
	}

	/// <summary>
	/// Gets or sets the keyboard device Id of the <see cref="Keyboard"/> whose key state changed
	/// </summary>
	/// <value>
	/// The keyboard device Id of the <see cref="Keyboard"/> whose key state changed, or <c>0</c> if unknown or virtual
	/// </value>
	public uint KeyboardId
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mWhich;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mWhich = value;
	}

	/// <summary>
	/// Gets or set the physical keyboard scancode of the key
	/// </summary>
	/// <value>
	/// The physical keyboard scancode of the key
	/// </value>
	public Scancode Scancode
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mScancode;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mScancode = value;
	}

	/// <summary>
	/// Gets or set the virtual keycode of the key
	/// </summary>
	/// <value>
	/// The virtual keycode of the key
	/// </value>
	/// <remarks>
	/// <para>
	/// This property reflects the base <see cref="Input.Keycode"/> generated by pressing the <see cref="Scancode"/> using the current keyboard layout, applying any options specified via <see cref="Hint.KeycodeOptions"/>.
	/// You can get the <see cref="Keycode"/> corresponding to the event <see cref="Scancode"/> and <see cref="Modifier"/> directly from the keyboard layout, bypassing <see cref="Hint.KeycodeOptions"/>, by calling <see cref="KeycodeExtensions.TryGetFromScancode(Scancode, Keymod, out Keycode)"/>.
	/// </para>
	/// </remarks>
	public Keycode Keycode
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mKey;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mKey = value;
	}

	/// <summary>
	/// Gets or sets the current key modifiers
	/// </summary>
	/// <value>
	/// The current key modifiers
	/// </value>
	public Keymod Modifier
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mMod;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mMod = value;
	}

	/// <summary>
	/// Gets or sets the raw platform dependent scancode for this event
	/// </summary>
	/// <value>
	/// The raw platform dependent scancode for this event
	/// </value>
	public ushort Raw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mRaw;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mRaw = value;
	}

	/// <summary>
	/// Gets or sets a value indicating whether the key is pressed
	/// </summary>
	/// <value>
	/// A value indicating whether the key is pressed
	/// </value>
	/// <remarks>
	/// <para>
	/// If the value of this property is <c><see langword="false"/></c>, you can assume the key to be released.
	/// </para>
	/// </remarks>
	public bool IsDown
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mDown;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mDown = value;
	}

	/// <summary>
	/// Gets or sets a value indication whether this event represents a repeated key event
	/// </summary>
	/// <value>
	/// A value indication whether this event represents a repeated key event
	/// </value>
	public bool IsRepeat
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mRepeat;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mRepeat = value;
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
							   .Append($", {nameof(KeyboardId)}: ")
							   .Append(KeyboardId.ToString(format, formatProvider))
							   .Append($", {nameof(Scancode)}: ")
							   .Append(Scancode.ToString())
							   .Append($", {nameof(Keycode)}: ")
							   .Append(Keycode.ToString())
							   .Append($", {nameof(Modifier)}: ")
							   .Append(Modifier.ToString())
							   .Append($", {nameof(Raw)}: ")
							   .Append(Raw.ToString(format, formatProvider))
							   .Append($", {nameof(IsDown)}: ")
							   .Append(IsDown)
							   .Append($", {nameof(IsRepeat)}: ")
							   .Append(IsRepeat)
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

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static implicit operator Event(in KeyboardEvent @event) => new(in @event);

	/// <remarks>
	/// <para>
	/// The <see cref="Event.Type"/> of the given <paramref name="event"/> must be either <see cref="EventType.KeyDown"/> or <see cref="EventType.KeyUp"/>.
	/// Otherwise, it will lead the method to throw an <see cref="ArgumentException"/>!
	/// </para>
	/// </remarks>
	/// <exception cref="ArgumentException">
	/// The <see cref="Event.Type"/> of the given <paramref name="event"/> was neither <see cref="EventType.KeyDown"/> nor <see cref="EventType.KeyUp"/>
	/// </exception>
	/// <inheritdoc/>
	public static explicit operator KeyboardEvent(in Event @event)
	{
		if (!Accepts(@event.Type))
		{
			failEventArgumentIsNotKeyboardEvent();
		}

		return @event.Key;

		[DoesNotReturn]
		static void failEventArgumentIsNotKeyboardEvent() => throw new ArgumentException($"{nameof(@event)} must be an {nameof(KeyboardEvent)} by {nameof(Type)}", paramName: nameof(@event));
	}
}
