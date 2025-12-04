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
	[FieldOffset(0)] internal CameraDeviceEvent CDevice;

	/// <summary>
	/// Creates a new <see cref="Event"/> from a <see cref="CameraDeviceEvent"/>
	/// </summary>
	/// <param name="event">The <see cref="CameraDeviceEvent"/> to store into the newly created <see cref="Event"/></param>
	public Event(in CameraDeviceEvent @event) :
#pragma warning disable IDE0034 // Leave it for explicitness sake
		this(default(IUnsafeConstructorDispatch?))
#pragma warning restore IDE0034
		=> CDevice = @event;
}

/// <summary>
/// Represents an event that occurs when a <see cref="Camera">camera device</see> is being <see cref="EventType.CameraDeviceAdded">added</see>, <see cref="EventType.CameraDeviceRemoved">removed</see>, <see cref="EventType.CameraDeviceApproved">approved</see>, or <see cref="EventType.CameraDeviceDenied">denied</see>
/// </summary>
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
[StructLayout(LayoutKind.Sequential)]
public struct CameraDeviceEvent : ICommonEvent<CameraDeviceEvent>, IFormattable, ISpanFormattable
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly string DebuggerDisplay => ToString(formatProvider: CultureInfo.InvariantCulture);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private static bool Accepts(EventType type) => type is >= EventType.CameraDeviceAdded and <= EventType.CameraDeviceDenied;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static bool ICommonEvent<CameraDeviceEvent>.Accepts(EventType type) => Accepts(type);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static ref CameraDeviceEvent ICommonEvent<CameraDeviceEvent>.GetReference(ref Event @event) => ref @event.CDevice;

	private CommonEvent mCommon;
	private uint mWhich;

	/// <remarks>
	/// <para>
	/// When setting this property, the value must be either <see cref="EventType.CameraDeviceAdded"/>, <see cref="EventType.CameraDeviceRemoved"/>, <see cref="EventType.CameraDeviceApproved"/>, or <see cref="EventType.CameraDeviceDenied"/>.
	/// Otherwise, it will lead the property to throw an <see cref="ArgumentException"/>!
	/// </para>
	/// </remarks>
	/// <exception cref="ArgumentException">
	/// When setting this property, the value was neither <see cref="EventType.CameraDeviceAdded"/>, <see cref="EventType.CameraDeviceRemoved"/>, <see cref="EventType.CameraDeviceApproved"/>, nor <see cref="EventType.CameraDeviceDenied"/>
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
			static void failValueArgumentIsNotValid() => throw new ArgumentException($"The given {nameof(value)} is not a valid value for the {nameof(Type)} of a {nameof(CameraDeviceEvent)}", paramName: nameof(value));
		}
	}

	///  <inheritdoc/>
	public ulong Timestamp
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mCommon.Timestamp;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mCommon.Timestamp = value;
	}

	/// <summary>
	/// Gets or sets the camera device ID for the <see cref="Camera"/> being <see cref="EventType.CameraDeviceAdded">added</see>, <see cref="EventType.CameraDeviceRemoved">removed</see>, <see cref="EventType.CameraDeviceApproved">approved</see>, or <see cref="EventType.CameraDeviceDenied">denied</see>
	/// </summary>
	/// <value>
	/// The camera device ID for the <see cref="Camera"/> being <see cref="EventType.CameraDeviceAdded">added</see>, <see cref="EventType.CameraDeviceRemoved">removed</see>, <see cref="EventType.CameraDeviceApproved">approved</see>, or <see cref="EventType.CameraDeviceDenied">denied</see>
	/// </value>
	public uint CameraId
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
							   .Append($", {nameof(CameraId)}: ")
							   .Append(CameraId.ToString(format, formatProvider))
							   .Append(" }")
							   .ToString();
		}
		finally
		{
			builder.Clear();
		}
	}

	/// <inheritdoc/>
	public readonly bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
	{
		charsWritten = 0;

		return SpanFormat.TryWrite("{ ", ref destination, ref charsWritten)
			&& ICommonEvent.TryPartiallyFormat(in this, ref destination, ref charsWritten, format)
			&& SpanFormat.TryWrite($", {nameof(CameraId)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(CameraId, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite(" }", ref destination, ref charsWritten);
	}

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static implicit operator Event(in CameraDeviceEvent @event) => new(in @event);

	/// <remarks>
	/// <para>
	/// The <see cref="Event.Type"/> of the given <paramref name="event"/> must be either <see cref="EventType.CameraDeviceAdded"/>, <see cref="EventType.CameraDeviceRemoved"/>, <see cref="EventType.CameraDeviceApproved"/>, or <see cref="EventType.CameraDeviceDenied"/>.
	/// Otherwise, it will lead the method to throw an <see cref="ArgumentException"/>!
	/// </para>
	/// </remarks>
	/// <exception cref="ArgumentException">
	/// The <see cref="Event.Type"/> of the given <paramref name="event"/> was neither <see cref="EventType.CameraDeviceAdded"/>, <see cref="EventType.CameraDeviceRemoved"/>, <see cref="EventType.CameraDeviceApproved"/>, nor <see cref="EventType.CameraDeviceDenied"/>
	/// </exception>
	/// <inheritdoc/>
	public static explicit operator CameraDeviceEvent(in Event @event)
	{
		if (!Accepts(@event.Type))
		{
			failEventArgumentIsNotCameraDeviceEvent();
		}

		return @event.CDevice;

		[DoesNotReturn]
		static void failEventArgumentIsNotCameraDeviceEvent() => throw new ArgumentException($"{nameof(@event)} must be a {nameof(CameraDeviceEvent)} by {nameof(Type)}", paramName: nameof(@event));
	}
}
