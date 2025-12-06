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
	[FieldOffset(0)] internal JoyDeviceEvent JDevice;

	/// <summary>
	/// Creates a new <see cref="Event"/> from a <see cref="JoyDeviceEvent"/>
	/// </summary>
	/// <param name="event">The <see cref="JoyDeviceEvent"/> to store into the newly created <see cref="Event"/></param>
	public Event(in JoyDeviceEvent @event) :
#pragma warning disable IDE0034 // Leave it for explicitness sake
		this(default(IUnsafeConstructorDispatch?))
#pragma warning restore IDE0034
		=> JDevice = @event;
}

/// <summary>
/// Represents an event that occurs when a <see cref="Joystick">joystick device</see> is being <see cref="EventType.JoystickAdded">added</see> into the system, <see cref="EventType.KeyboardRemoved">removed</see> from the system, or <see cref="EventType.JoystickUpdateCompleted">updated</see>
/// </summary>
/// <remarks>
/// <para>
/// SDL will send an <see cref="JoyDeviceEvent"/> with <see cref="Type"/> <see cref="EventType.JoystickAdded"/> for every joystick device it discovers during initialization.
/// After that, <see cref="JoyDeviceEvent"/>s with <see cref="Type"/> <see cref="EventType.JoystickAdded"/> will only arrive when a joystick device is hotplugged during the program's run.
/// </para>
/// </remarks>
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
[StructLayout(LayoutKind.Sequential)]
public struct JoyDeviceEvent : ICommonEvent<JoyDeviceEvent>, IFormattable, ISpanFormattable
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly string DebuggerDisplay => ToString(formatProvider: CultureInfo.InvariantCulture);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private static bool Accepts(EventType type) => type is EventType.JoystickAdded or EventType.JoystickRemoved or EventType.JoystickUpdateCompleted;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static bool ICommonEvent<JoyDeviceEvent>.Accepts(EventType type) => Accepts(type);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static ref JoyDeviceEvent ICommonEvent<JoyDeviceEvent>.GetReference(ref Event @event) => ref @event.JDevice;

	private CommonEvent mCommon;
	private uint mWhich;

	/// <remarks>
	/// <para>
	/// When setting this property, the value must be either <see cref="EventType.JoystickAdded"/>, <see cref="EventType.JoystickRemoved"/>, or <see cref="EventType.JoystickUpdateCompleted"/>.
	/// Otherwise, it will lead the property to throw an <see cref="ArgumentException"/>!
	/// </para>
	/// </remarks>
	/// <exception cref="ArgumentException">
	/// When setting this property, the value was neither <see cref="EventType.JoystickAdded"/>, <see cref="EventType.JoystickRemoved"/>, nor <see cref="EventType.JoystickUpdateCompleted"/>
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
			static void failValueArgumentIsNotValid() => throw new ArgumentException($"The given {nameof(value)} is not a valid value for the {nameof(Type)} of a {nameof(JoyDeviceEvent)}", paramName: nameof(value));
		}
	}

	/// <inheritdoc/>
	public ulong Timestamp
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mCommon.Timestamp;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mCommon.Timestamp = value;
	}

	/// <summary>
	/// Gets or sets the joystick device ID for the <see cref="Joystick"/> being <see cref="EventType.JoystickAdded">added</see>, <see cref="EventType.JoystickRemoved">removed</see>, or <see cref="EventType.JoystickUpdateCompleted">updated</see>
	/// </summary>
	/// <value>
	/// The joystick device ID for the <see cref="Joystick"/> being <see cref="EventType.JoystickAdded">added</see>, <see cref="EventType.JoystickRemoved">removed</see>, or <see cref="EventType.JoystickUpdateCompleted">updated</see>
	/// </value>
	public uint JoystickId
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
							   .Append($", {nameof(JoystickId)}: ")
							   .Append(JoystickId.ToString(format, formatProvider))
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
			&& SpanFormat.TryWrite($", {nameof(JoystickId)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(JoystickId, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite(" }", ref destination, ref charsWritten);
	}

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static implicit operator Event(in JoyDeviceEvent @event) => new(in @event);

	/// <remarks>
	/// <para>
	/// The <see cref="Event.Type"/> of the given <paramref name="event"/> must be either <see cref="EventType.JoystickAdded"/>, <see cref="EventType.JoystickRemoved"/>, or <see cref="EventType.JoystickUpdateCompleted"/>.
	/// Otherwise, it will lead the method to throw an <see cref="ArgumentException"/>!
	/// </para>
	/// </remarks>
	/// <exception cref="ArgumentException">
	/// The <see cref="Event.Type"/> of the given <paramref name="event"/> was neither <see cref="EventType.JoystickAdded"/>, <see cref="EventType.JoystickRemoved"/>, nor <see cref="EventType.JoystickUpdateCompleted"/>
	/// </exception>
	/// <inheritdoc/>
	public static explicit operator JoyDeviceEvent(in Event @event)
	{
		if (!Accepts(@event.Type))
		{
			failEventArgumentIsNotJoyDeviceEvent();
		}

		return @event.JDevice;

		[DoesNotReturn]
		static void failEventArgumentIsNotJoyDeviceEvent() => throw new ArgumentException($"{nameof(@event)} must be a {nameof(JoyDeviceEvent)} by {nameof(Type)}", paramName: nameof(@event));
	}
}
