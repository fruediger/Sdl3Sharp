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
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
#pragma warning disable IDE0034 // Leave it for explicitness sake
	public Event(in CameraDeviceEvent @event) : this(default(IUnsafeConstructorDispatch?)) => CDevice = @event;
#pragma warning restore IDE0034
}

/// <summary>
/// Represents an event that occurs when a camera device is being <see cref="EventType.CameraDevice.Added">added</see>, <see cref="EventType.CameraDevice.Removed">removed</see>, <see cref="EventType.CameraDevice.Approved">approved</see>, or <see cref="EventType.CameraDevice.Denied">denied</see>
/// </summary>
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
[StructLayout(LayoutKind.Sequential)]
public struct CameraDeviceEvent : ICommonEvent<CameraDeviceEvent>, IFormattable, ISpanFormattable
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly string DebuggerDisplay => ToString(formatProvider: CultureInfo.InvariantCulture);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private static bool Accepts(EventType type) => type >= EventType.CameraDevice.Added && type <= EventType.CameraDevice.Denied;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static bool ICommonEvent<CameraDeviceEvent>.Accepts(EventType type) => Accepts(type);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static ref CameraDeviceEvent ICommonEvent<CameraDeviceEvent>.GetReference(ref Event @event) => ref @event.CDevice;

	private CommonEvent<CameraDeviceEvent> mCommon;
	private uint mWhich;
	
	/// <inheritdoc/>
	public EventType<CameraDeviceEvent> Type
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mCommon.Type;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mCommon.Type = value;
	}

	/// <inheritdoc/>
	public ulong Timestamp
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mCommon.Timestamp;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mCommon.Timestamp = value;
	}

	/// <summary>
	/// Gets or sets the camera device ID for the device being <see cref="EventType.CameraDevice.Added">added</see>, <see cref="EventType.CameraDevice.Removed">removed</see>, <see cref="EventType.CameraDevice.Approved">approved</see>, or <see cref="EventType.CameraDevice.Denied">denied</see>
	/// </summary>
	/// <value>
	/// The camera device ID for the device being <see cref="EventType.CameraDevice.Added">added</see>, <see cref="EventType.CameraDevice.Removed">removed</see>, <see cref="EventType.CameraDevice.Approved">approved</see>, or <see cref="EventType.CameraDevice.Denied">denied</see>
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
		=> $"{{ {ICommonEvent.PartialToString(in this, format, formatProvider)}, {
			nameof(CameraId)}: {CameraId.ToString(format, formatProvider)} }}";

	/// <inheritdoc/>
	public readonly bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format = default, IFormatProvider? provider = default)
	{
		charsWritten = 0;

		return SpanFormat.TryWrite("{ ", ref destination, ref charsWritten)
			&& ICommonEvent.TryPartialFormat(in this, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite($", {nameof(CameraId)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(CameraId, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite(" }", ref destination, ref charsWritten);
	}

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static implicit operator Event(in CameraDeviceEvent @event) => new(in @event);

	/// <remarks>
	/// <para>
	/// The <see cref="Event.Type"/> of the given <paramref name="event"/> must be either <see cref="EventType.CameraDevice.Added"/>, <see cref="EventType.CameraDevice.Removed"/>, <see cref="EventType.CameraDevice.Approved"/>, or <see cref="EventType.CameraDevice.Denied"/>.
	/// Otherwise, it will lead the method to throw an <see cref="ArgumentException"/>!
	/// </para>
	/// </remarks>
	/// <exception cref="ArgumentException">
	/// The <see cref="Event.Type"/> of the given <paramref name="event"/> was neither <see cref="EventType.CameraDevice.Added"/>, <see cref="EventType.CameraDevice.Removed"/>, <see cref="EventType.CameraDevice.Approved"/>, nor <see cref="EventType.CameraDevice.Denied"/>
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
		static void failEventArgumentIsNotCameraDeviceEvent() => throw new ArgumentException($"{nameof(@event)} must be an {nameof(CameraDeviceEvent)} by {nameof(Type)}", paramName: nameof(@event));
	}
}
