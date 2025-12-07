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
	public Event(in AudioDeviceEvent @event) :
#pragma warning disable IDE0034 // Leave it for explicitness sake
		this(default(IUnsafeConstructorDispatch?))
#pragma warning restore IDE0034
		=> ADevice = @event;
}

/// <summary>
/// Represents an event that occurs when an audio device is being <see cref="EventType.AudioDeviceAdded">added</see>, <see cref="EventType.AudioDeviceRemoved">removed</see>, or <see cref="EventType.AudioDeviceFormatChanged">changed</see>
/// </summary>
/// <remarks>
/// <para>
/// SDL will send an <see cref="AudioDeviceEvent"/> with <see cref="Type"/> <see cref="EventType.AudioDeviceAdded"/> for every audio device it discovers during initialization.
/// After that, <see cref="AudioDeviceEvent"/>s with <see cref="Type"/> <see cref="EventType.AudioDeviceAdded"/> will only arrive when an audio device is hotplugged during the application's runtime.
/// </para>
/// <para>
/// Associated <see cref="EventType"/>s:
/// <list type="bullet">
/// <item><description><see cref="EventType.AudioDeviceAdded"/></description></item> 
/// <item><description><see cref="EventType.AudioDeviceRemoved"/></description></item> 
/// <item><description><see cref="EventType.AudioDeviceFormatChanged"/></description></item>
/// </list>
/// </para>
/// </remarks>
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
[StructLayout(LayoutKind.Sequential)]
public struct AudioDeviceEvent : ICommonEvent<AudioDeviceEvent>, IFormattable, ISpanFormattable
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly string DebuggerDisplay => ToString(formatProvider: CultureInfo.InvariantCulture);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private static bool Accepts(EventType type) => type is >= EventType.AudioDeviceAdded and <= EventType.AudioDeviceFormatChanged;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static bool ICommonEvent<AudioDeviceEvent>.Accepts(EventType type) => Accepts(type);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static ref AudioDeviceEvent ICommonEvent<AudioDeviceEvent>.GetReference(ref Event @event) => ref @event.ADevice;

	private CommonEvent mCommon;
	private uint mWhich;
	private CBool mRecording;
	private readonly byte mPadding1, mPadding2, mPadding3;

	/// <remarks>
	/// <para>
	/// When setting this property, the value must be either <see cref="EventType.AudioDeviceAdded"/>, <see cref="EventType.AudioDeviceRemoved"/>, or <see cref="EventType.AudioDeviceFormatChanged"/>.
	/// Otherwise, it will lead the property to throw an <see cref="ArgumentException"/>!
	/// </para>
	/// </remarks>
	/// <exception cref="ArgumentException">
	/// When setting this property, the value was neither <see cref="EventType.AudioDeviceAdded"/>, <see cref="EventType.AudioDeviceRemoved"/>, nor <see cref="EventType.AudioDeviceFormatChanged"/>
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
			static void failValueArgumentIsNotValid() => throw new ArgumentException($"The given {nameof(value)} is not a valid value for the {nameof(Type)} of an {nameof(AudioDeviceEvent)}", paramName: nameof(value));
		}
	}

	/// <inheritdoc/>
	public ulong Timestamp
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mCommon.Timestamp;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mCommon.Timestamp = value;
	}

	/// <summary>
	/// Gets or sets the audio device ID for the <see cref="AudioDevice"/> being <see cref="EventType.AudioDeviceAdded">added</see>, <see cref="EventType.AudioDeviceRemoved">removed</see>, or <see cref="EventType.AudioDeviceFormatChanged">changed</see>
	/// </summary>
	/// <value>
	/// The audio device IDfor the <see cref="AudioDevice"/> being <see cref="EventType.AudioDeviceAdded">added</see>, <see cref="EventType.AudioDeviceRemoved">removed</see>, or <see cref="EventType.AudioDeviceFormatChanged">changed</see>
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
	{
		var builder = Shared.StringBuilder;
		try
		{
			return ICommonEvent.PartiallyAppend(in this, builder.Append("{ "), format)
							   .Append($", {nameof(AudioDeviceId)}: ")
							   .Append(AudioDeviceId.ToString(format, formatProvider))
							   .Append($", {nameof(IsRecordingDevice)}: ")
							   .Append(IsRecordingDevice)
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
			&& SpanFormat.TryWrite($", {nameof(AudioDeviceId)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(AudioDeviceId, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite($", {nameof(IsRecordingDevice)}", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(IsRecordingDevice, ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(" }", ref destination, ref charsWritten);
	}

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static implicit operator Event(in AudioDeviceEvent @event) => new(in @event);

	/// <remarks>
	/// <para>
	/// The <see cref="Event.Type"/> of the given <paramref name="event"/> must be either <see cref="EventType.AudioDeviceAdded"/>, <see cref="EventType.AudioDeviceRemoved"/>, or <see cref="EventType.AudioDeviceFormatChanged"/>.
	/// Otherwise, it will lead the method to throw an <see cref="ArgumentException"/>!
	/// </para>
	/// </remarks>
	/// <exception cref="ArgumentException">
	/// The <see cref="Event.Type"/> of the given <paramref name="event"/> was neither <see cref="EventType.AudioDeviceAdded"/>, <see cref="EventType.AudioDeviceRemoved"/>, nor <see cref="EventType.AudioDeviceFormatChanged"/>
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
