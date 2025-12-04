using Sdl3Sharp.Internal;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace Sdl3Sharp.Events;

partial struct Event
{
	[FieldOffset(0)] internal TextInputEvent Text;

	/// <summary>
	/// Creates a new <see cref="Event"/> from a <see cref="TextInputEvent"/>
	/// </summary>
	/// <param name="event">The <see cref="TextInputEvent"/> to store into the newly created <see cref="Event"/></param>
	public Event(in TextInputEvent @event) :
#pragma warning disable IDE0034 // Leave it for explicitness sake
		this(default(IUnsafeConstructorDispatch?))
#pragma warning restore IDE0034
		=> Text = @event;
}

/// <summary>
/// Represents an event that occurs when <see cref="EventType.TextInput">text input</see> is happening
/// </summary>
/// <remarks>
/// This event will never be delivered unless text input is enabled by calling <see cref="SDL_StartTextInput"/>. Text input is disabled by default!
/// </remarks>
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
[StructLayout(LayoutKind.Sequential)]
public struct TextInputEvent : ICommonEvent<TextInputEvent>, IFormattable, ISpanFormattable
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly string DebuggerDisplay => ToString(formatProvider: CultureInfo.InvariantCulture);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private static bool Accepts(EventType type) => type is EventType.TextInput;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static bool ICommonEvent<TextInputEvent>.Accepts(EventType type) => Accepts(type);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static ref TextInputEvent ICommonEvent<TextInputEvent>.GetReference(ref Event @event) => ref @event.Text;

	private CommonEvent mCommon;
	private uint mWindowID;
	private unsafe readonly byte* mText;

	/// <remarks>
	/// <para>
	/// When setting this property, the value must be <see cref="EventType.TextInput"/>.
	/// Otherwise, it will lead the property to throw an <see cref="ArgumentException"/>!
	/// </para>
	/// </remarks>
	/// <exception cref="ArgumentException">
	/// When setting this property, the value was not <see cref="EventType.TextInput"/>
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
	/// Gets the input text
	/// </summary>
	/// <value>
	/// The input text
	/// </value>
	/// <remarks>
	/// <para>
	/// Reading this property can be very expensive, you should consider caching it's value.
	/// </para>
	/// <para>
	/// Setting this property is not supported and will lead the property to throw a <see cref="NotSupportedException"/>.
	/// </para>
	/// </remarks>
	/// <exception cref="NotSupportedException">When setting this property</exception>
	public string? Text
	{
		readonly get { unsafe { return Utf8StringMarshaller.ConvertToManaged(mText); } }

		[Obsolete($"Setting {nameof(Text)} is not supported yet.")]
		[DoesNotReturn]
		set => throw new NotSupportedException($"Setting {nameof(Text)} is not supported");
	}

	internal readonly ReadOnlySpan<byte> TextUtf8
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get { unsafe { return MemoryMarshal.CreateReadOnlySpanFromNullTerminated(mText); } }
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
			ICommonEvent.PartiallyAppend(in this, builder.Append("{ "), format)
				         .Append($", {nameof(WindowId)}: ")
						 .Append(WindowId.ToString(format, formatProvider))
						 .Append($", {nameof(Text)}: ");

			if (Text is string text)
			{
				builder.Append('"')
					   .Append(text)
					   .Append('"');
			}
			else
			{
				builder.Append("null");
			}

			return builder.Append(" }")
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
		unsafe
		{
			charsWritten = 0;

			return SpanFormat.TryWrite("{ ", ref destination, ref charsWritten)
				&& ICommonEvent.TryPartiallyFormat(in this, ref destination, ref charsWritten, format)
				&& SpanFormat.TryWrite($", {nameof(WindowId)}: ", ref destination, ref charsWritten)
				&& SpanFormat.TryWrite(WindowId, ref destination, ref charsWritten, format, provider)
				&& SpanFormat.TryWrite($", {nameof(Text)}: ", ref destination, ref charsWritten)
				&& (mText is not null
					?  SpanFormat.TryWrite('"', ref destination, ref charsWritten)
					&& SpanFormat.TryWriteUtf8(MemoryMarshal.CreateReadOnlySpanFromNullTerminated(mText), ref destination, ref charsWritten)
					&& SpanFormat.TryWrite('"', ref destination, ref charsWritten)
					:  SpanFormat.TryWrite("null", ref destination, ref charsWritten))
				&& SpanFormat.TryWrite(" }", ref destination, ref charsWritten);
		}
	}

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static implicit operator Event(in TextInputEvent @event) => new(in @event);

	/// <remarks>
	/// <para>
	/// The <see cref="Event.Type"/> of the given <paramref name="event"/> must be <see cref="EventType.TextInput"/>.
	/// Otherwise, it will lead the method to throw an <see cref="ArgumentException"/>!
	/// </para>
	/// </remarks>
	/// <exception cref="ArgumentException">
	/// The <see cref="Event.Type"/> of the given <paramref name="event"/> was not <see cref="EventType.TextInput"/>
	/// </exception>
	/// <inheritdoc/>
	public static explicit operator TextInputEvent(in Event @event)
	{
		if (!Accepts(@event.Type))
		{
			failEventArgumentIsNotTextInputEvent();
		}

		return @event.Text;

		[DoesNotReturn]
		static void failEventArgumentIsNotTextInputEvent() => throw new ArgumentException($"{nameof(@event)} must be an {nameof(TextInputEvent)} by {nameof(Type)}", paramName: nameof(@event));
	}
}
