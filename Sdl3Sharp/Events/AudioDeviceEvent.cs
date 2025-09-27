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
	[FieldOffset(0)] internal AudioDeviceEvent ADevice;

	/// <summary>
	/// Creates a new <see cref="Event"/> from an <see cref="AudioDeviceEvent"/>
	/// </summary>
	/// <param name="event">The <see cref="AudioDeviceEvent"/> to store into the newly created <see cref="Event"/></param>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
#pragma warning disable IDE0034 // Leave it for explicitness sake
	public Event(in AudioDeviceEvent @event) : this(default(IUnsafeConstructorDispatch?)) => ADevice = @event;
#pragma warning restore IDE0034
}

/// <summary>
/// Represents an event that occurs when an audio device is being <see cref="EventType.AudioDevice.Added">added</see>, <see cref="EventType.AudioDevice.Removed">removed</see>, or <see cref="EventType.AudioDevice.FormatChanged">changed</see>
/// </summary>
/// <remarks>
/// SDL will send an <see cref="AudioDeviceEvent"/> with <see cref="Type"/> <see cref="EventType.AudioDevice.Added"/> for every audio device it discovers during initialization.
/// After that, <see cref="AudioDeviceEvent"/>s with <see cref="Type"/> <see cref="EventType.AudioDevice.Added"/> will only arrive when an audio device is hotplugged during the program's run.
/// </remarks>
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
[StructLayout(LayoutKind.Sequential)]
public struct AudioDeviceEvent : ICommonEvent<AudioDeviceEvent>, IFormattable, ISpanFormattable
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly string DebuggerDisplay => ToString(formatProvider: CultureInfo.InvariantCulture);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private static bool Accepts(EventType type) => type >= EventType.AudioDevice.Added && type <= EventType.AudioDevice.FormatChanged;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static bool ICommonEvent<AudioDeviceEvent>.Accepts(EventType type) => Accepts(type);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static ref AudioDeviceEvent ICommonEvent<AudioDeviceEvent>.GetReference(ref Event @event) => ref @event.ADevice;

	private CommonEvent<AudioDeviceEvent> mCommon;
	private uint mWhich;
	private CBool mRecording;
	private readonly byte mPadding1, mPadding2, mPadding3;

	/// <inheritdoc/>
	public EventType<AudioDeviceEvent> Type
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
	/// Gets or sets the audio device ID for the device being <see cref="EventType.AudioDevice.Added">added</see>, <see cref="EventType.AudioDevice.Removed">removed</see>, or <see cref="EventType.AudioDevice.FormatChanged">changed</see>
	/// </summary>
	/// <value>
	/// The audio device ID for the device being <see cref="EventType.AudioDevice.Added">added</see>, <see cref="EventType.AudioDevice.Removed">removed</see>, or <see cref="EventType.AudioDevice.FormatChanged">changed</see>
	/// </value>
	public uint AudioDeviceId
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mWhich;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mWhich = value;
	}

	/// <summary>
	/// Gets or sets a value indicating if the <see cref="AudioDeviceId">specific audio device</see> is a recording device or a playback device
	/// </summary>
	/// <value>
	/// A value indicating if the <see cref="AudioDeviceId">specific audio device</see> is a recording device (when <c><see langword="true"/></c>) or a playback device (when <c><see langword="false"/></c>)
	/// </value>
	public bool IsRecordingDevice
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mRecording;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mRecording = value;
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
			nameof(AudioDeviceId)}: {AudioDeviceId.ToString(format, formatProvider)}, {
			nameof(IsRecordingDevice)}: {IsRecordingDevice} }}";

	/// <inheritdoc/>
	public readonly bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format = default, IFormatProvider? provider = default)
	{
		charsWritten = 0;

		return SpanFormat.TryWrite("{ ", ref destination, ref charsWritten)
			&& ICommonEvent.TryPartialFormat(in this, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite($", {nameof(AudioDeviceId)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(AudioDeviceId, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite($", {nameof(IsRecordingDevice)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(IsRecordingDevice, ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(" }", ref destination, ref charsWritten);
	}

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static implicit operator Event(in AudioDeviceEvent @event) => new(in @event);

	/// <remarks>
	/// <para>
	/// The <see cref="Event.Type"/> of the given <paramref name="event"/> must be either <see cref="EventType.AudioDevice.Added"/>, <see cref="EventType.AudioDevice.Removed"/>, or <see cref="EventType.AudioDevice.FormatChanged"/>.
	/// Otherwise, it will lead the method to throw an <see cref="ArgumentException"/>!
	/// </para>
	/// </remarks>
	/// <exception cref="ArgumentException">
	/// The <see cref="Event.Type"/> of the given <paramref name="event"/> was neither <see cref="EventType.AudioDevice.Added"/>, <see cref="EventType.AudioDevice.Removed"/>, nor <see cref="EventType.AudioDevice.FormatChanged"/>
	/// </exception>
	/// <inheritdoc/>
	public static explicit operator AudioDeviceEvent(in Event @event)
	{
		if (!Accepts(@event.Type))
		{
			failEventArgumentIsNotAudioDeviceEvent();
		}

		return @event.ADevice;

		[DoesNotReturn]
		static void failEventArgumentIsNotAudioDeviceEvent() => throw new ArgumentException($"{nameof(@event)} must be an {nameof(AudioDeviceEvent)} by {nameof(Type)}", paramName: nameof(@event));
	}
}
