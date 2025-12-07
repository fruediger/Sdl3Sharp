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
	[FieldOffset(0)] internal DropEvent Drop;

	/// <summary>
	/// Creates a new <see cref="Event"/> from a <see cref="DropEvent"/>
	/// </summary>
	/// <param name="event">The <see cref="DropEvent"/> to store into the newly created <see cref="Event"/></param>
	public Event(in DropEvent @event) :
#pragma warning disable IDE0034 // Leave it for explicitness sake
		this(default(IUnsafeConstructorDispatch?))
#pragma warning restore IDE0034
		=> Drop = @event;
}

/// <summary>
/// Represents an event that occurs when text data or a file is being drag'n'dropped onto the application
/// </summary>
/// <remarks>
/// <para>
/// Associated <see cref="EventType"/>s:
/// <list type="bullet">
/// <item><description><see cref="EventType.DropFile"/></description></item> 
/// <item><description><see cref="EventType.DropText"/></description></item> 
/// <item><description><see cref="EventType.DropBegin"/></description></item>
/// <item><description><see cref="EventType.DropCompleted"/></description></item>
/// <item><description><see cref="EventType.DropPosition"/></description></item>
/// </list>
/// </para>
/// </remarks>
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
[StructLayout(LayoutKind.Sequential)]
public struct DropEvent : ICommonEvent<DropEvent>, IFormattable, ISpanFormattable
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly string DebuggerDisplay => ToString(formatProvider: CultureInfo.InvariantCulture);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private static bool Accepts(EventType type) => type is >= EventType.DropFile and <= EventType.DropPosition;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static bool ICommonEvent<DropEvent>.Accepts(EventType type) => Accepts(type);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static ref DropEvent ICommonEvent<DropEvent>.GetReference(ref Event @event) => ref @event.Drop;

	private CommonEvent mCommon;
	private uint mWindowID;
	private float mX;
	private float mY;
	private unsafe readonly byte* mSource;
	private unsafe readonly byte* mData;

	/// <remarks>
	/// <para>
	/// When setting this property, the value must be one of the <see cref="EventType"/>.Drop* values.
	/// Otherwise, it will lead the property to throw an <see cref="ArgumentException"/>!
	/// </para>
	/// </remarks>
	/// <exception cref="ArgumentException">
	/// When setting this property, the value was not one of the <see cref="EventType"/>.Drop* values
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
			static void failValueArgumentIsNotValid() => throw new ArgumentException($"The given {nameof(value)} is not a valid value for the {nameof(Type)} of a {nameof(DropEvent)}", paramName: nameof(value));
		}
	}

	/// <inheritdoc/>
	public ulong Timestamp
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mCommon.Timestamp;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mCommon.Timestamp = value;
	}

	/// <summary>
	/// Gets or sets the window Id of the <see cref="Window"/> that was dropped on, if any
	/// </summary>
	/// <value>
	/// The window Id of the <see cref="Window"/> that was dropped on, if any, or <c>0</c>
	/// </value>
	public uint WindowId
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mWindowID;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mWindowID = value;
	}

	/// <summary>
	/// Gets or set the X coordinate, relative to the <see cref="Window"/>
	/// </summary>
	/// <value>
	/// The X coordinate, relative to the <see cref="Window"/>
	/// </value>
	/// <remarks>
	/// <para>
	/// The value of this property is not valid for <see cref="EventType.DropBegin"/> events.
	/// </para>
	/// </remarks>
	public float X
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mX;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mX = value;
	}

	/// <summary>
	/// Gets or set the Y coordinate, relative to the <see cref="Window"/>
	/// </summary>
	/// <value>
	/// The Y coordinate, relative to the <see cref="Window"/>
	/// </value>
	/// <remarks>
	/// <para>
	/// The value of this property is not valid for <see cref="EventType.DropBegin"/> events.
	/// </para>
	/// </remarks>
	public float Y
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mY;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mY = value;
	}

	/// <summary>
	/// Gets the source application that sent this drop event
	/// </summary>
	/// <value>
	/// The source application that sent this drop event, or <c><see langword="null"/></c> if that information isn't available
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
	public string? Source
	{
		readonly get { unsafe { return Utf8StringMarshaller.ConvertToManaged(mSource); } }

		[Obsolete($"Setting {nameof(Source)} is not supported yet.")]
		[DoesNotReturn]
		set => throw new NotSupportedException($"Setting {nameof(Source)} is not supported");
	}

	internal readonly ReadOnlySpan<byte> SourceUtf8
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get { unsafe { return MemoryMarshal.CreateReadOnlySpanFromNullTerminated(mSource); } }
	}

	/// <summary>
	/// Gets the text for <see cref="EventType.DropText"/> events or the file name for <see cref="EventType.DropFile"/> events
	/// </summary>
	/// <value>
	/// The text for <see cref="EventType.DropText"/> events, the file name for <see cref="EventType.DropFile"/> events, or <c><see langword="null"/></c> for other events
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
	public string? Data
	{
		readonly get { unsafe { return Utf8StringMarshaller.ConvertToManaged(mData); } }

		[Obsolete($"Setting {nameof(Source)} is not supported yet.")]
		[DoesNotReturn]
		set => throw new NotSupportedException($"Setting {nameof(Data)} is not supported");
	}

	internal readonly ReadOnlySpan<byte> DataUtf8
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get { unsafe { return MemoryMarshal.CreateReadOnlySpanFromNullTerminated(mData); } }
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
						.Append($", {nameof(X)}: ")
						.Append(X.ToString(format, formatProvider))
						.Append($", {nameof(Y)}: ")
						.Append(Y.ToString(format, formatProvider))
						.Append($", {nameof(Source)}: ");

			if (Source is string source)
			{
				builder.Append('"')
					   .Append(source)
					   .Append('"');
			}
			else
			{
				builder.Append("null");
			}

			builder.Append($", {nameof(Data)}: ");

			if (Data is string data)
			{
				builder.Append('"')
					   .Append(data)
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
				&& SpanFormat.TryWrite($", {nameof(X)}: ", ref destination, ref charsWritten)
				&& SpanFormat.TryWrite(X, ref destination, ref charsWritten, format, provider)
				&& SpanFormat.TryWrite($", {nameof(Y)}: ", ref destination, ref charsWritten)
				&& SpanFormat.TryWrite(Y, ref destination, ref charsWritten, format, provider)
				&& SpanFormat.TryWrite($", {nameof(Source)}: ", ref destination, ref charsWritten)
				&& (mSource is not null
					?  SpanFormat.TryWrite('"', ref destination, ref charsWritten)
					&& SpanFormat.TryWriteUtf8(MemoryMarshal.CreateReadOnlySpanFromNullTerminated(mSource), ref destination, ref charsWritten)
					&& SpanFormat.TryWrite('"', ref destination, ref charsWritten)
					:  SpanFormat.TryWrite("null", ref destination, ref charsWritten))
				&& SpanFormat.TryWrite($", {nameof(Data)}: ", ref destination, ref charsWritten)
				&& (mData is not null
					?  SpanFormat.TryWrite('"', ref destination, ref charsWritten)
					&& SpanFormat.TryWriteUtf8(MemoryMarshal.CreateReadOnlySpanFromNullTerminated(mData), ref destination, ref charsWritten)
					&& SpanFormat.TryWrite('"', ref destination, ref charsWritten)
					:  SpanFormat.TryWrite("null", ref destination, ref charsWritten))
				&& SpanFormat.TryWrite(" }", ref destination, ref charsWritten);
		}
	}

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static implicit operator Event(in DropEvent @event) => new(in @event);

	/// <remarks>
	/// <para>
	/// The <see cref="Event.Type"/> of the given <paramref name="event"/> must be one of the <see cref="EventType"/>.Drop* values.
	/// Otherwise, it will lead the method to throw an <see cref="ArgumentException"/>!
	/// </para>
	/// </remarks>
	/// <exception cref="ArgumentException">
	/// The <see cref="Event.Type"/> of the given <paramref name="event"/> was not one of the <see cref="EventType"/>.Drop* values
	/// </exception>
	/// <inheritdoc/>
	public static explicit operator DropEvent(in Event @event)
	{
		if (!Accepts(@event.Type))
		{
			failEventArgumentIsNotDropEvent();
		}

		return @event.Drop;

		[DoesNotReturn]
		static void failEventArgumentIsNotDropEvent() => throw new ArgumentException($"{nameof(@event)} must be a {nameof(DropEvent)} by {nameof(Type)}", paramName: nameof(@event));
	}
}
