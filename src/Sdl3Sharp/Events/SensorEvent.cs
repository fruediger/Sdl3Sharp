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
	[FieldOffset(0)] internal SensorEvent Sensor;

	/// <summary>
	/// Creates a new <see cref="Event"/> from a <see cref="SensorEvent"/>
	/// </summary>
	/// <param name="event">The <see cref="SensorEvent"/> to store into the newly created <see cref="Event"/></param>
	public Event(in SensorEvent @event) :
#pragma warning disable IDE0034 // Leave it for explicitness sake
		this(default(IUnsafeConstructorDispatch?))
#pragma warning restore IDE0034
		=> Sensor = @event;
}

/// <summary>
/// Represents an event that occurs when a <see cref="Sensor">sensor</see> is being <see cref="EventType.SensorUpdated">updated</see>
/// </summary>
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
[StructLayout(LayoutKind.Sequential)]
public struct SensorEvent : ICommonEvent<SensorEvent>, IFormattable, ISpanFormattable
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly string DebuggerDisplay => ToString(formatProvider: CultureInfo.InvariantCulture);

	private static bool Accepts(EventType type) => type is EventType.SensorUpdated;

	static bool ICommonEvent<SensorEvent>.Accepts(EventType type) => Accepts(type);

	static ref SensorEvent ICommonEvent<SensorEvent>.GetReference(ref Event @event) => ref @event.Sensor;

	private CommonEvent mCommon;
	private uint mWhich;
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
		readonly get => mCommon.Type;

		set
		{
			if (!Accepts(value))
			{
				failValueArgumentIsNotValid();
			}

			mCommon.Type = value;

			[DoesNotReturn]
			static void failValueArgumentIsNotValid() => throw new ArgumentException($"The given {nameof(value)} is not a valid value for the {nameof(Type)} of a {nameof(SensorEvent)}", paramName: nameof(value));
		}
	}

	/// <inheritdoc/>
	public ulong Timestamp
	{
		readonly get => mCommon.Timestamp;
		set => mCommon.Timestamp = value;
	}

	/// <summary>
	/// Gets or sets the sensor device ID for the <see cref="Sensor"/> being <see cref="EventType.SensorUpdated">updated</see>
	/// </summary>
	/// <value>
	/// The sensor device ID for the <see cref="Sensor"/> being <see cref="EventType.SensorUpdated">updated</see>
	/// </value>
	public uint SensorId
	{
		readonly get => mWhich;
		set => mWhich = value;
	}

	/// <summary>
	/// Gets or sets the data from the sensor
	/// </summary>
	/// <value>
	/// The data from the sensor, as a span of six <see cref="float"/> values
	/// </value>
	/// <remarks>
	/// <para>
	/// Notice that this property always returns a <see cref="ReadOnlySpan{T}"/>.
	/// If you want to modify the data, you can either set the entire event data by setting this property from a different span, or modify the individual <see cref="Data0"/>, <see cref="Data1"/>, …, and <see cref="Data5"/> properties.
	/// </para>
	/// <para>
	/// Notice that when setting this property, the given values of the given span will be copied into the event data.
	/// That means you can assign spans that are larger than six elements, in which case only the first six elements will be copied.
	/// If you try to assign a span that is smaller than six elements, this property will throw an <see cref="ArgumentException"/>.
	/// </para>
	/// </remarks>
	/// <exception cref="ArgumentException">
	/// When setting this property, the length of the given <paramref name="value"/> argument is less than 6
	/// </exception>
	public ReadOnlySpan<float> Data
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		readonly get { unsafe { return MemoryMarshal.CreateReadOnlySpan(ref Unsafe.As<DataArray, float>(ref Unsafe.AsRef(in mData)), DataLength); } }

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
		readonly get => mData[0];
		set => mData[0] = value;
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
		readonly get => mData[1];
		set => mData[1] = value;
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
		readonly get => mData[2];
		set => mData[2] = value;
	}

	/// <summary>
	/// Gets or sets the fourth data value from the sensor
	/// </summary>
	/// <value>
	/// The fourth data value from the sensor
	/// </value>
	/// <remarks>
	/// <para>
	/// The value of this property is the same as <c><see cref="Data">Data</see><see cref="ReadOnlySpan{T}.this[int]">[</see>3<see cref="ReadOnlySpan{T}.this[int]">]</see></c>.
	/// </para>
	/// <para>
	/// If you want to retrieve the entire event data from the sensor at once, consider using the <see cref="Data"/> property instead.
	/// </para>
	/// </remarks>
	public float Data3
	{
		readonly get => mData[3];
		set => mData[3] = value;
	}

	/// <summary>
	/// Gets or sets the fifth data value from the sensor
	/// </summary>
	/// <value>
	/// The fifth data value from the sensor
	/// </value>
	/// <remarks>
	/// <para>
	/// The value of this property is the same as <c><see cref="Data">Data</see><see cref="ReadOnlySpan{T}.this[int]">[</see>4<see cref="ReadOnlySpan{T}.this[int]">]</see></c>.
	/// </para>
	/// <para>
	/// If you want to retrieve the entire event data from the sensor at once, consider using the <see cref="Data"/> property instead.
	/// </para>
	/// </remarks>
	public float Data4
	{
		readonly get => mData[4];
		set => mData[4] = value;
	}

	/// <summary>
	/// Gets or sets the sixth data value from the sensor
	/// </summary>
	/// <value>
	/// The sixth data value from the sensor
	/// </value>
	/// <remarks>
	/// <para>
	/// The value of this property is the same as <c><see cref="Data">Data</see><see cref="ReadOnlySpan{T}.this[int]">[</see>5<see cref="ReadOnlySpan{T}.this[int]">]</see></c>.
	/// </para>
	/// <para>
	/// If you want to retrieve the entire event data from the sensor at once, consider using the <see cref="Data"/> property instead.
	/// </para>
	/// </remarks>
	public float Data5
	{
		readonly get => mData[5];
		set => mData[5] = value;
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
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		readonly get => mSensorTimestamp;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		set => mSensorTimestamp = value;
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
																			 .Append($", {nameof(SensorId)}: ")
																			 .Append(SensorId.ToString(format, formatProvider))
																			 .Append($", {nameof(Data)}: [ ")
																			 .Append(Data0.ToString(format, formatProvider))
																			 .Append(", ")
																			 .Append(Data1.ToString(format, formatProvider))
																			 .Append(", ")
																			 .Append(Data2.ToString(format, formatProvider))
																			 .Append(", ")
																			 .Append(Data3.ToString(format, formatProvider))
																			 .Append(", ")
																			 .Append(Data4.ToString(format, formatProvider))
																			 .Append(", ")
																			 .Append(Data5.ToString(format, formatProvider))
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
			&& SpanFormat.TryWrite($", {nameof(SensorId)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(SensorId, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite($", {nameof(Data)}: [ ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(Data0, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite(", ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(Data1, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite(", ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(Data2, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite(", ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(Data3, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite(", ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(Data4, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite(", ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(Data5, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite(" ]", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite($", {nameof(SensorTimestamp)}: ", ref destination, ref charsWritten)
			&& ICommonEvent.TryFormatTimestamp(SensorTimestamp, ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(" }", ref destination, ref charsWritten);
	}

	/// <inheritdoc/>
	public static implicit operator Event(in SensorEvent @event) => new(in @event);

	/// <remarks>
	/// <para>
	/// The <see cref="Event.Type"/> of the given <paramref name="event"/> must be <see cref="EventType.SensorUpdated"/>.
	/// Otherwise, it will lead the method to throw an <see cref="ArgumentException"/>!
	/// </para>
	/// </remarks>
	/// <exception cref="ArgumentException">
	/// The <see cref="Event.Type"/> of the given <paramref name="event"/> was not <see cref="EventType.SensorUpdated"/>
	/// </exception>
	/// <inheritdoc/>
	public static explicit operator SensorEvent(in Event @event)
	{
		if (!Accepts(@event.Type))
		{
			failEventArgumentIsNotSensorEvent();
		}

		return @event.Sensor;

		[DoesNotReturn]
		static void failEventArgumentIsNotSensorEvent() => throw new ArgumentException($"{nameof(@event)} must be a {nameof(SensorEvent)} by {nameof(Type)}", paramName: nameof(@event));
	}

	private const int DataLength = 6;

	[InlineArray(DataLength)]
	private struct DataArray { private float _; }
}
