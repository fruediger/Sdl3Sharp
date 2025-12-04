using Sdl3Sharp.Internal;
using System;
using System.Buffers;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
using System.Text;

namespace Sdl3Sharp.Events;

partial struct Event
{
	[FieldOffset(0)] internal TextEditingEvent Edit;

	/// <summary>
	/// Creates a new <see cref="Event"/> from a <see cref="TextEditingEvent"/>
	/// </summary>
	/// <param name="event">The <see cref="TextEditingEvent"/> to store into the newly created <see cref="Event"/></param>
	public Event(in TextEditingEvent @event) :
#pragma warning disable IDE0034 // Leave it for explicitness sake
		this(default(IUnsafeConstructorDispatch?))
#pragma warning restore IDE0034
		=> Edit = @event;
}

/// <summary>
/// Represents an event that occurs when <see cref="EventType.TextEditing">text editing</see> happens
/// </summary>
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
[StructLayout(LayoutKind.Sequential)]
public struct TextEditingEvent : ICommonEvent<TextEditingEvent>, IFormattable, ISpanFormattable
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly string DebuggerDisplay => ToString(formatProvider: CultureInfo.InvariantCulture);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private static bool Accepts(EventType type) => type is EventType.TextEditing;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static bool ICommonEvent<TextEditingEvent>.Accepts(EventType type) => Accepts(type);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static ref TextEditingEvent ICommonEvent<TextEditingEvent>.GetReference(ref Event @event) => ref @event.Edit;

	private CommonEvent mCommon;
	private uint mWindowID;
	private unsafe readonly byte* mText;
	private int mStart;
	private int mLength;

	/// <remarks>
	/// <para>
	/// When setting this property, the value must be <see cref="EventType.TextEditing"/>.
	/// Otherwise, it will lead the property to throw an <see cref="ArgumentException"/>!
	/// </para>
	/// </remarks>
	/// <exception cref="ArgumentException">
	/// When setting this property, the value was not <see cref="EventType.TextEditing"/>
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
			static void failValueArgumentIsNotValid() => throw new ArgumentException($"The given {nameof(value)} is not a valid value for the {nameof(Type)} of an {nameof(TextEditingEvent)}", paramName: nameof(value));
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
	/// Gets the editing text
	/// </summary>
	/// <value>
	/// The editing text
	/// </value>
	/// <remarks>
	/// <para>
	/// Reading this property can be very expensive, you should consider caching it's value.
	/// If you want to get the <see cref="Text"/>, <see cref="Start"/>, and <see cref="Length"/> simultaneously, consider using the <see cref="GetTextStartAndLength"/> method as it's way more efficient when getting all three quantities at once. 
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

	/// <summary>
	/// Gets or set the starting cursor of the selected editing text
	/// </summary>
	/// <value>
	/// The starting cursor of the selected editing text, as a position in number of characters, or <c>-1</c> if not set
	/// </value>
	/// <remarks>
	/// <para>
	/// The starting cursor is the position, in number of characters, where new typing will be inserted into the editing text.
	/// </para>
	/// <para>
	/// Reading and writing this property can be very expensive, you should consider caching it's value.
	/// If you want to get the <see cref="Text"/>, <see cref="Start"/>, and <see cref="Length"/> simultaneously, consider using the <see cref="GetTextStartAndLength"/> method as it's way more efficient when getting all three quantities at once. 
	/// </para>
	/// </remarks>
	public int Start
	{
		readonly get
		{
			unsafe
			{
				if (mStart is < 0)
				{
					return -1;
				}

				var text = MemoryMarshal.CreateReadOnlySpanFromNullTerminated(mText);
				var startUtf16 = 0;

				for (var scalar = 0;

					 scalar < mStart
					 && text.Length is > 0
					 && Rune.DecodeFromUtf8(text, out var rune, out var bytesConsumed) is OperationStatus.Done;

					 startUtf16 += rune.Utf16SequenceLength,
					 scalar++,
					 text = text[bytesConsumed..]
				)
				{ }

				return startUtf16;
			}
		}

		set
		{
			unsafe
			{
				if (value is < 0)
				{
					mStart = -1;
					return;
				}

				var text = MemoryMarshal.CreateReadOnlySpanFromNullTerminated(mText);
				var scalar = 0;

				for (var startUtf16 = 0;

					 startUtf16 < value
					 && text.Length is > 0
					 && Rune.DecodeFromUtf8(text, out var rune, out var bytesConsumed) is OperationStatus.Done;

					 scalar++,
					 startUtf16 += rune.Utf16SequenceLength,
					 text = text[bytesConsumed..]
				)
				{ }

				mStart = scalar;
			}
		}
	}

	internal int StartUtf8
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mStart;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mStart = value;
	}

	/// <summary>
	/// Gets or sets the length of the selected editing text
	/// </summary>
	/// <value>
	/// The length of the selected editing text, as a length in number of characters, or <c>-1</c> if not set
	/// </value>
	/// <remarks>
	/// <para>
	/// The length is the number of characters that will be replaced by new typing.
	/// </para>
	/// <para>
	/// Reading and writing this property can be very expensive, you should consider caching it's value.
	/// If you want to get the <see cref="Text"/>, <see cref="Start"/>, and <see cref="Length"/> simultaneously, consider using the <see cref="GetTextStartAndLength"/> method as it's way more efficient when getting all three quantities at once. 
	/// </para>
	/// </remarks>
	public int Length
	{
		readonly get
		{
			unsafe
			{
				if (mLength is < 0)
				{
					return -1;
				}

				var text = MemoryMarshal.CreateReadOnlySpanFromNullTerminated(mText);
				var lengthUtf16 = 0;

				for (var scalar = 0;

					 scalar < mStart
					 && text.Length is > 0
					 && Rune.DecodeFromUtf8(text, out _, out var bytesConsumed) is OperationStatus.Done;

					 scalar++,
					 text = text[bytesConsumed..]
				)
				{ }

				for (var scalar = 0;

					scalar < mLength
					&& text.Length is > 0
					&& Rune.DecodeFromUtf8(text, out var rune, out int bytesConsumed) is OperationStatus.Done;

					lengthUtf16 += rune.Utf16SequenceLength,
					scalar++,
					text = text[bytesConsumed..])
				{ }

				return lengthUtf16;
			}
		}

		set
		{
			unsafe
			{
				if (value is < 0)
				{
					mLength = -1;
					return;
				}

				var text = MemoryMarshal.CreateReadOnlySpanFromNullTerminated(mText);
				var scalar = 0;

				for (;

					 scalar < mStart
					 && text.Length is > 0
					 && Rune.DecodeFromUtf8(text, out _, out var bytesConsumed) is OperationStatus.Done;

					 scalar++,
					 text = text[bytesConsumed..]
				)
				{ }

				scalar = 0;

				for (var lengthUtf16 = 0;

					lengthUtf16 < value
					&& text.Length is > 0
					&& Rune.DecodeFromUtf8(text, out var rune, out int bytesConsumed) is OperationStatus.Done;

					lengthUtf16 += rune.Utf16SequenceLength,
					scalar++,
					text = text[bytesConsumed..])
				{ }

				mLength = scalar;
			}
		}
	}

	internal int LengthUtf8
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mLength;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mLength = value;
	}

	/// <summary>
	/// Gets the editing text, the starting cursor of the selected editing text, and the length of the selected editing text simultaneously
	/// </summary>
	/// <returns>The editing text, the starting cursor of the selected editing text, as a position in number of characters, or <c>-1</c> if not set, and length of the selected editing text, as a length in number of characters, or <c>-1</c> if not set</returns>
	/// <remarks>
	/// <para>
	/// The starting cursor is the position, in number of characters, where new typing will be inserted into the editing text.
	/// </para>
	/// <para>
	/// The resulting length is the number of characters that will be replaced by new typing.
	/// </para>
	/// <para>
	/// Calling this method can be very expensive, you should consider caching it's return values.
	/// </para>
	/// </remarks>
	public readonly (string? Text, int Start, int Length) GetTextStartAndLength()
	{
		unsafe
		{
			var text = Utf8StringMarshaller.ConvertToManaged(mText);

			if (mStart is < 0 && mLength is < 0)
			{
				return (text, Start: -1, Length: -1);
			}

			var span = text.AsSpan();
			var startUtf16 = 0;
			var lengthUtf16 = 0;

			for (var scalar = 0;

				 scalar < mStart
				 && span.Length is > 0
				 && Rune.DecodeFromUtf16(span, out _, out var charsConsumed) is OperationStatus.Done;

				 startUtf16 += charsConsumed,
				 scalar++,
				 span = span[charsConsumed..]
				)
			{ }

			if (mLength is < 0)
			{
				lengthUtf16 = -1;

				// we know that mStart can't be < 0
			}
			else
			{
				if (mStart is < 0)
				{
					startUtf16 = -1;
				}

				for (var scalar = 0;

					 scalar < mLength
					 && span.Length is > 0
					 && Rune.DecodeFromUtf16(span, out _, out int charsConsumed) is OperationStatus.Done;

					 lengthUtf16 += charsConsumed,
					 scalar++,
					 span = span[charsConsumed..]
					)
				{ }
			}

			return (text, startUtf16, lengthUtf16);
		}
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
			var (text, start, length) = GetTextStartAndLength();

			ICommonEvent.PartiallyAppend(in this, builder.Append("{ "), format)
						.Append($", {nameof(WindowId)}: ")
						.Append(WindowId.ToString(format, formatProvider))
						.Append($", {nameof(Text)}: ");

			if (text is not null)
			{
				builder.Append('"')
					   .Append(text)
					   .Append('"');
			}
			else
			{
				builder.Append("null");
			}

			return builder.Append($", {nameof(Start)}: ")
				          .Append(start.ToString(format, formatProvider))
						  .Append($", {nameof(Length)}: ")
						  .Append(length.ToString(format, formatProvider))
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
		var (text, start, length) = GetTextStartAndLength();

		charsWritten = 0;

		return SpanFormat.TryWrite("{ ", ref destination, ref charsWritten)
			&& ICommonEvent.TryPartiallyFormat(in this, ref destination, ref charsWritten, format)
			&& SpanFormat.TryWrite($", {nameof(WindowId)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(WindowId, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite($", {nameof(Text)}", ref destination, ref charsWritten)
			&& (text is not null
				?  SpanFormat.TryWrite('"', ref destination, ref charsWritten)
				&& SpanFormat.TryWrite(text, ref destination, ref charsWritten)
				&& SpanFormat.TryWrite('"', ref destination, ref charsWritten)
				:  SpanFormat.TryWrite("null", ref destination, ref charsWritten))
			&& SpanFormat.TryWrite($", {nameof(Start)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(start, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite($", {nameof(LengthUtf8)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(length, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite(" }", ref destination, ref charsWritten);
	}

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static implicit operator Event(in TextEditingEvent @event) => new(in @event);

	/// <remarks>
	/// <para>
	/// The <see cref="Event.Type"/> of the given <paramref name="event"/> must be <see cref="EventType.TextEditing"/>.
	/// Otherwise, it will lead the method to throw an <see cref="ArgumentException"/>!
	/// </para>
	/// </remarks>
	/// <exception cref="ArgumentException">
	/// The <see cref="Event.Type"/> of the given <paramref name="event"/> was not <see cref="EventType.TextEditing"/>
	/// </exception>
	/// <inheritdoc/>
	public static explicit operator TextEditingEvent(in Event @event)
	{
		if (!Accepts(@event.Type))
		{
			failEventArgumentIsNotTextEditingEvent();
		}

		return @event.Edit;

		[DoesNotReturn]
		static void failEventArgumentIsNotTextEditingEvent() => throw new ArgumentException($"{nameof(@event)} must be a {nameof(TextEditingEvent)} by {nameof(Type)}", paramName: nameof(@event));
	}
}
