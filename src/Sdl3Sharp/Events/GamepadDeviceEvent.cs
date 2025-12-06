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
	[FieldOffset(0)] internal GamepadDeviceEvent GDevice;

	/// <summary>
	/// Creates a new <see cref="Event"/> from a <see cref="GamepadDeviceEvent"/>
	/// </summary>
	/// <param name="event">The <see cref="GamepadDeviceEvent"/> to store into the newly created <see cref="Event"/></param>
	public Event(in GamepadDeviceEvent @event) :
#pragma warning disable IDE0034 // Leave it for explicitness sake
		this(default(IUnsafeConstructorDispatch?))
#pragma warning restore IDE0034
		=> GDevice = @event;


}

/// <summary>
/// Represents an event that occurs when a <see cref="Gamepad">gamepad device</see> is being <see cref="EventType.GamepadRemoved">added</see> into the system, <see cref="EventType.GamepadRemoved">removed</see> from the system, <see cref="EventType.GamepadRemapped">remapped</see>, or <see cref="EventType.GamepadUpdateCompleted">updated</see> or that gets it's <see cref="EventType.GamepadSteamHandleUpdated">Steam handle updated</see>
/// </summary>
/// <remarks>
/// <para>
/// Joysticks that are supported gamepads receive both, a <see cref="JoyDeviceEvent"/> and a <see cref="GamepadDeviceEvent"/>.
/// </para>
/// <para>
/// SDL will send a <see cref="GamepadDeviceEvent"/> with <see cref="Type"/> <see cref="EventType.GamepadAdded"/> for every gamepad device it discovers during initialization.
/// After that, <see cref="GamepadDeviceEvent"/>s with <see cref="Type"/> <see cref="EventType.GamepadAdded"/> will only arrive when a gamepad device is hotplugged or a <see cref="Joystick">joystick device</see> gets a <see cref="Gamepad">gamepad</see> mapping during the application's runtime.
/// </para>
/// </remarks>
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
[StructLayout(LayoutKind.Sequential)]
public struct GamepadDeviceEvent : ICommonEvent<GamepadDeviceEvent>, IFormattable, ISpanFormattable
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly string DebuggerDisplay => ToString(formatProvider: CultureInfo.InvariantCulture);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private static bool Accepts(EventType type) => type is (>= EventType.GamepadAdded and <= EventType.GamepadRemapped) or EventType.GamepadUpdateCompleted or EventType.GamepadSteamHandleUpdated;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static bool ICommonEvent<GamepadDeviceEvent>.Accepts(EventType type) => Accepts(type);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static ref GamepadDeviceEvent ICommonEvent<GamepadDeviceEvent>.GetReference(ref Event @event) => ref @event.GDevice;

	private CommonEvent mCommon;
	private uint mWhich;

	/// <remarks>
	/// <para>
	/// When setting this property, the value must be either <see cref="EventType.GamepadAdded"/>, <see cref="EventType.GamepadRemoved"/>, <see cref="EventType.GamepadRemapped"/>, <see cref="EventType.GamepadUpdateCompleted"/>, or <see cref="EventType.GamepadSteamHandleUpdated"/>.
	/// Otherwise, it will lead the property to throw an <see cref="ArgumentException"/>!
	/// </para>
	/// </remarks>
	/// <exception cref="ArgumentException">
	/// When setting this property, the value was neither <see cref="EventType.GamepadAdded"/>, <see cref="EventType.GamepadRemoved"/>, <see cref="EventType.GamepadRemapped"/>, <see cref="EventType.GamepadUpdateCompleted"/>, nor <see cref="EventType.GamepadSteamHandleUpdated"/>
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
			static void failValueArgumentIsNotValid() => throw new ArgumentException($"The given {nameof(value)} is not a valid value for the {nameof(Type)} of a {nameof(GamepadDeviceEvent)}", paramName: nameof(value));
		}
	}

	/// <inheritdoc/>
	public ulong Timestamp
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mCommon.Timestamp;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mCommon.Timestamp = value;
	}

	/// <summary>
	/// Gets or sets the joystick device ID for the <see cref="Gamepad"/> being <see cref="EventType.GamepadAdded">added</see>, <see cref="EventType.GamepadRemoved">removed</see>, <see cref="EventType.GamepadRemapped">remapped</see>, or <see cref="EventType.GamepadUpdateCompleted">updated</see> or that gets it's <see cref="EventType.GamepadSteamHandleUpdated">Steam handle updated</see>
	/// </summary>
	/// <value>
	/// The joystick device ID for the <see cref="Gamepad"/> being <see cref="EventType.GamepadAdded">added</see>, <see cref="EventType.GamepadRemoved">removed</see>, <see cref="EventType.GamepadRemapped">remapped</see>, or <see cref="EventType.GamepadUpdateCompleted">updated</see> or that gets it's <see cref="EventType.GamepadSteamHandleUpdated">Steam handle updated</see>
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
							   .Append($", {JoystickId}: ")
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
	public static implicit operator Event(in GamepadDeviceEvent @event) => new(in @event);

	/// <remarks>
	/// <para>
	/// The <see cref="Event.Type"/> of the given <paramref name="event"/> must be either <see cref="EventType.GamepadAdded"/>, <see cref="EventType.GamepadRemoved"/>, <see cref="EventType.GamepadRemapped"/>, <see cref="EventType.GamepadUpdateCompleted"/>, or <see cref="EventType.GamepadSteamHandleUpdated"/>.
	/// Otherwise, it will lead the method to throw an <see cref="ArgumentException"/>!
	/// </para>
	/// </remarks>
	/// <exception cref="ArgumentException">
	/// The <see cref="Event.Type"/> of the given <paramref name="event"/> was neither <see cref="EventType.GamepadAdded"/>, <see cref="EventType.GamepadRemoved"/>, <see cref="EventType.GamepadRemapped"/>, <see cref="EventType.GamepadUpdateCompleted"/>, nor <see cref="EventType.GamepadSteamHandleUpdated"/>
	/// </exception>
	/// <inheritdoc/>
	public static explicit operator GamepadDeviceEvent(in Event @event)
	{
		if (!Accepts(@event.Type))
		{
			failEventArgumentIsNotGamepadDeviceEvent();
		}

		return @event.GDevice;

		[DoesNotReturn]
		static void failEventArgumentIsNotGamepadDeviceEvent() => throw new ArgumentException($"{nameof(@event)} must be a {nameof(GamepadDeviceEvent)} by {nameof(Type)}", paramName: nameof(@event));
	}
}
