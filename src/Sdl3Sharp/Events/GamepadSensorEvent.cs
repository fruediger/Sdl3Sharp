using Sdl3Sharp.Input;
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
	[FieldOffset(0)] internal GamepadSensorEvent GSensor;

	/// <summary>
	/// Creates a new <see cref="Event"/> from a <see cref="GamepadSensorEvent"/>
	/// </summary>
	/// <param name="event">The <see cref="GamepadSensorEvent"/> to store into the newly created <see cref="Event"/></param>
	public Event(in GamepadSensorEvent @event) :
#pragma warning disable IDE0034 // Leave it for explicitness sake
		this(default(IUnsafeConstructorDispatch?))
#pragma warning restore IDE0034
		=> GSensor = @event;
}

/// <summary>
/// Represents an event that occurs when a gamepad sensor value changes
/// </summary>
/// <remarks>
/// <para>
/// Associated <see cref="EventType"/>s:
/// <list type="bullet">
/// <item><description><see cref="EventType.GamepadSensorUpdated"/></description></item>
/// </list>
/// </para>
/// </remarks>
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
[StructLayout(LayoutKind.Sequential)]
public struct GamepadSensorEvent : ICommonEvent<GamepadSensorEvent>, IFormattable, ISpanFormattable
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly string DebuggerDisplay => ToString(formatProvider: CultureInfo.InvariantCulture);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private static bool Accepts(EventType type) => type is EventType.GamepadSensorUpdated;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static bool ICommonEvent<GamepadSensorEvent>.Accepts(EventType type) => Accepts(type);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static ref GamepadSensorEvent ICommonEvent<GamepadSensorEvent>.GetReference(ref Event @event) => ref @event.GSensor;

	private CommonEvent mCommon;
	private uint mWhich;
	private SensorType mSensor;
	private DataArray mData;
	private ulong mSensorTimestamp;

	/// <remarks>
	/// <para>
	/// When setting this property, the value must be <see cref="EventType.GamepadSensorUpdated"/>.
	/// Otherwise, it will lead the property to throw an <see cref="ArgumentException"/>!
	/// </para>
	/// </remarks>
	/// <exception cref="ArgumentException">
	/// When setting this property, the value was <see cref="EventType.GamepadSensorUpdated"/>
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
			static void failValueArgumentIsNotValid() => throw new ArgumentException($"The given {nameof(value)} is not a valid value for the {nameof(Type)} of a {nameof(GamepadSensorEvent)}", paramName: nameof(value));
		}
	}

	/// <inheritdoc/>
	public ulong Timestamp
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mCommon.Timestamp;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mCommon.Timestamp = value;
	}

	/// <summary>
	/// Gets or sets the joystick device ID for the <see cref="Gamepad"/> associated with the event
	/// </summary>
	/// <value>
	/// The joystick device ID for the <see cref="Gamepad"/> associated with the event
	/// </value>
	public uint JoystickId
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mWhich;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mWhich = value;
	}

	/// <summary>
	/// Gets or sets the type of gamepad sensor that changed
	/// </summary>
	/// <value>
	/// The type of gamepad sensor that changed
	/// </value>
	public SensorType Sensor
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mSensor;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mSensor = value;
	}

	/// <summary>
	/// Gets or sets the data from the sensor
	/// </summary>
	/// <value>
	/// The data from the sensor, as a span of three <see cref="float"/> values
	/// </value>
	/// <remarks>
	/// <para>
	/// Notice that this property always returns a <see cref="ReadOnlySpan{T}"/>.
	/// If you want to modify the data, you can either set the entire event data by setting this property from a different span, or modify the individual <see cref="Data0"/>, <see cref="Data1"/>, and <see cref="Data2"/> properties.
	/// </para>
	/// <para>
	/// Notice that when setting this property, the given values of the given span will be copied into the event data.
	/// That means you can assign spans that are larger than three elements, in which case only the first three elements will be copied.
	/// If you try to assign a span that is smaller than three elements, this property will throw an <see cref="ArgumentException"/>.
	/// </para>
	/// </remarks>
	/// <exception cref="ArgumentException">
	/// When setting this property, the length of the given <paramref name="value"/> argument is less than 3
	/// </exception>
	public ReadOnlySpan<float> Data
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get { unsafe { return MemoryMarshal.CreateReadOnlySpan(ref Unsafe.As<DataArray, float>(ref Unsafe.AsRef(in mData)), DataLength); } }

		set
		{
			if (value.Length is < DataLength)
			{
				failValueArgumentTooSmall();
			}

			value[..DataLength].CopyTo(MemoryMarshal.CreateSpan(ref Unsafe.As<DataArray, float>(ref mData), DataLength));

			[DoesNotReturn]
			static void failValueArgumentTooSmall() => throw new ArgumentException($"The length of the given {nameof(value)} argument is less than {DataLength}", paramName: nameof(value));
		}
	}

	/// <summary>
	/// Gets or sets the first data value from the sensor
	/// </summary>
	/// <value>
	/// The first data value from the sensor
	/// </value>
	/// <remarks>
	/// <para>
	/// The value of this property is the same as <c><see cref="Data">Data</see><see cref="ReadOnlySpan{T}.this[int]">[</see>0<see cref="ReadOnlySpan{T}.this[int]">]</see></c>.
	/// </para>
	/// <para>
	/// If you want to retrieve the entire event data from the sensor at once, consider using the <see cref="Data"/> property instead.
	/// </para>
	/// </remarks>
	public float Data0
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mData[0];
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mData[0] = value;
	}

	/// <summary>
	/// Gets or sets the second data value from the sensor
	/// </summary>
	/// <value>
	/// The second data value from the sensor
	/// </value>
	/// <remarks>
	/// <para>
	/// The value of this property is the same as <c><see cref="Data">Data</see><see cref="ReadOnlySpan{T}.this[int]">[</see>1<see cref="ReadOnlySpan{T}.this[int]">]</see></c>.
	/// </para>
	/// <para>
	/// If you want to retrieve the entire event data from the sensor at once, consider using the <see cref="Data"/> property instead.
	/// </para>
	/// </remarks>
	public float Data1
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mData[1];
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mData[1] = value;
	}

	/// <summary>
	/// Gets or sets the third data value from the sensor
	/// </summary>
	/// <value>
	/// The third data value from the sensor
	/// </value>
	/// <remarks> 
	/// <para>
	/// The value of this property is the same as <c><see cref="Data">Data</see><see cref="ReadOnlySpan{T}.this[int]">[</see>2<see cref="ReadOnlySpan{T}.this[int]">]</see></c>.
	/// </para>
	/// <para>
	/// If you want to retrieve the entire event data from the sensor at once, consider using the <see cref="Data"/> property instead.
	/// </para>
	/// </remarks>
	public float Data2
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mData[2];
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mData[2] = value;
	}

	/// <summary>
	/// Gets or sets the timestamp of the sensor reading
	/// </summary>
	/// <value>
	/// The timestamp of the sensor reading, in nanoseconds
	/// </value>
	/// <remarks>
	/// <para>
	/// The value of this property is not necessarily synchronized with the system's clock.
	/// </para>
	/// </remarks>
	public ulong SensorTimestamp
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mSensorTimestamp;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mSensorTimestamp = value;
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
			return ICommonEvent.AppendTimestamp(SensorTimestamp, ICommonEvent.PartiallyAppend(in this, builder.Append("{ "), format)
																			 .Append($", {nameof(JoystickId)}: ")
																			 .Append(JoystickId.ToString(format, formatProvider))
																			 .Append($", {nameof(Sensor)}: ")
																			 .Append(Sensor)
																			 .Append($", {nameof(Data)}: [ ")
																			 .Append(Data0.ToString(format, formatProvider))
																			 .Append(", ")
																			 .Append(Data1.ToString(format, formatProvider))
																			 .Append(", ")
																			 .Append(Data2.ToString(format, formatProvider))
																			 .Append(" ]")
																			 .Append($", {nameof(SensorTimestamp)}: "))
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
			&& SpanFormat.TryWrite($", {nameof(Sensor)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(Sensor, ref destination, ref charsWritten)
			&& SpanFormat.TryWrite($", {nameof(Data)}: [ ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(Data0, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite(", ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(Data1, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite(", ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(Data2, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite(" ]", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite($", {nameof(SensorTimestamp)}: ", ref destination, ref charsWritten)
			&& ICommonEvent.TryFormatTimestamp(SensorTimestamp, ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(" }", ref destination, ref charsWritten);
	}

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static implicit operator Event(in GamepadSensorEvent @event) => new(in @event);

	/// <remarks>
	/// <para>
	/// The <see cref="Event.Type"/> of the given <paramref name="event"/> must be <see cref="EventType.GamepadSensorUpdated"/>.
	/// Otherwise, it will lead the method to throw an <see cref="ArgumentException"/>!
	/// </para>
	/// </remarks>
	/// <exception cref="ArgumentException">
	/// The <see cref="Event.Type"/> of the given <paramref name="event"/> was not <see cref="EventType.GamepadSensorUpdated"/>
	/// </exception>
	/// <inheritdoc/>
	public static explicit operator GamepadSensorEvent(in Event @event)
	{
		if (!Accepts(@event.Type))
		{
			failEventArgumentIsNotGamepadSensorEvent();
		}

		return @event.GSensor;

		[DoesNotReturn]
		static void failEventArgumentIsNotGamepadSensorEvent() => throw new ArgumentException($"{nameof(@event)} must be a {nameof(GamepadSensorEvent)} by {nameof(Type)}", paramName: nameof(@event));
	}

	private const int DataLength = 3;

	[InlineArray(DataLength)] private struct DataArray { private float _; }
}
