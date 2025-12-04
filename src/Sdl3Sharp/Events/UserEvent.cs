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
	[FieldOffset(0)] internal UserEvent User;

	/// <summary>
	/// Creates a new <see cref="Event"/> from an <see cref="UserEvent"/>
	/// </summary>
	/// <param name="event">The <see cref="UserEvent"/> to store into the newly created <see cref="Event"/></param>
	public Event(in UserEvent @event) :
#pragma warning disable IDE0034 // Leave it for explicitness sake
		this(default(IUnsafeConstructorDispatch?))
#pragma warning restore IDE0034
		=> User = @event;
}

/// <summary>
/// Represents an user defined event
/// </summary>
/// <remarks>
/// <para>
/// An <see cref="UserEvent"/> is unique; it is never created by SDL, but only by the application.
/// The event can be pushed onto the event queue using <see cref="Sdl.TryPushEvent(in Event)"/>.
/// The contents of the structure members are completely up to the programmer; the only requirement is that it's <see cref="Type"/> is a <em>registered</em> user defined event type obtained from <see cref="EventTypeExtensions.TryRegister(out EventType)"/> or <see cref="EventTypeExtensions.TryRegister(Span{EventType})"/>.
/// </para>
/// <para>
/// If you want to populate the <em>managed</em> user defined data slots, <see cref="Data1"/> and <see cref="Data2"/>, you'll need to handle their lifetimes via an <see cref="UserEventLifetime"/> instance.
/// You can then use it's <see cref="UserEventLifetime.GetEvent(EventType, ulong, uint, int)"/> method to obtain an <see cref="UserEvent"/> where you'll be able to use it's <em>managed</em> user defined data slots, <see cref="Data1"/> and <see cref="Data2"/>. 
/// </para>
/// <para>
/// If you don't intend to use non-blittable or managed data, you can always use the <em>raw</em> user defined data slots, <see cref="RawData1"/> and <see cref="RawData2"/>, freely.
/// </para>
/// <para>
/// Note: You shouldn't access the any of the <em>raw</em> user defined data slots when you use the corresponding <em>managed</em> user defined data slots.
/// E.g., overwriting the value of <see cref="RawData1"/> will lead to a loss of data in <see cref="Data1"/> and <see cref="Data1"/> won't be accessable anymore.
/// </para>
/// </remarks>
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
[StructLayout(LayoutKind.Sequential)]
public struct UserEvent : ICommonEvent<UserEvent>, IFormattable, ISpanFormattable
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly string DebuggerDisplay => ToString(formatProvider: CultureInfo.InvariantCulture);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private static bool Accepts(EventType type)
#pragma warning disable CS0618 // Here's one of the few places we're allowed to use this (internally)
		=> type is >= EventType.User and <= EventType.Last;
#pragma warning restore CS0618

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static bool ICommonEvent<UserEvent>.Accepts(EventType type) => Accepts(type);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static ref UserEvent ICommonEvent<UserEvent>.GetReference(ref Event @event) => ref @event.User;

	private CommonEvent mCommon;
	private uint mWindowID;
	private int mCode;
	private unsafe void* mData1;
	private unsafe void* mData2;

	/// <remarks>
	/// <para>
	/// When setting this property, the value must be a valid value for <see cref="UserEvent"/>s.
	/// Otherwise, it will lead the property to throw an <see cref="ArgumentException"/>!
	/// Try registering user defined event type using <see cref="EventTypeExtensions.TryRegister(out EventType)"/> or <see cref="EventTypeExtensions.TryRegister(Span{EventType})"/>.
	/// </para>
	/// </remarks>
	/// <exception cref="ArgumentException">
	/// When setting this property, the value was not valid for <see cref="UserEvent"/>s
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
			static void failValueArgumentIsNotValid() => throw new ArgumentException($"The given {nameof(value)} is not a valid value for the {nameof(Type)} of an {nameof(UserEvent)}", paramName: nameof(value));
		}
	}

	/// <inheritdoc/>
	public ulong Timestamp
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mCommon.Timestamp;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mCommon.Timestamp = value;
	}

	/// <summary>
	/// Gets or sets the window Id of the <see cref="Window"/> associated with this event, if any
	/// </summary>
	/// <value>
	/// The window Id of the <see cref="Window"/> associated with this event, if any, or <c>0</c>
	/// </value>
	public uint WindowId
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mWindowID;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mWindowID = value;
	}

	/// <summary>
	/// Gets or sets the user defined event code
	/// </summary>
	/// <value>
	/// The user defined event code
	/// </value>
	/// <remarks>
	/// <para>
	/// You can freely set this to any value of your liking. For example, you could use this property to further distinguish your user defined events.
	/// </para>
	/// </remarks>
	public int Code
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mCode;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mCode = value;
	}

	/// <summary>
	/// Gets or sets the value of the first <em>managed</em> user defined data slot
	/// </summary>
	/// <value>
	/// The value of the first <em>managed</em> user defined data slot
	/// </value>
	/// <remarks>
	/// <para>
	/// You should only access this property if the user defined event is backed by a <see cref="UserEventLifetime"/>.
	/// If it's not, or the <see cref="UserEventLifetime"/> is already disposed, accessing this property will lead to a <see cref="InvalidOperationException"/> been thrown.
	/// </para>
	/// <para>
	/// You can freely set this to anything of your liking as long as the <see cref="UserEvent"/> is backed by a <see cref="UserEventLifetime"/>.
	/// </para>
	/// <para>
	/// Note: If you intend to use this property, don't overwrite the value of it's associated <em>raw</em> user defined data slot, <see cref="RawData1"/>.
	/// If you do, that will lead to a loss of data in <see cref="Data1"/> and <see cref="Data1"/> won't be accessable anymore.
	/// </para>
	/// </remarks>
	/// <inheritdoc cref="UserEventLifetime.GetData1(ref readonly UserEvent)"/>
	/// <inheritdoc cref="UserEventLifetime.SetData1(ref readonly UserEvent, object?)"/>
	public readonly object? Data1
	{
		get => UserEventLifetime.GetData1(in this);
		set => UserEventLifetime.SetData1(in this, value);
	}

	/// <summary>
	/// Gets or sets the value of the first <em>raw</em> user defined data slot
	/// </summary>
	/// <value>
	/// The value of the first <em>raw</em> user defined data slot
	/// </value><remarks>
	/// <para>
	/// You should not overwrite the value of this property if the user defined event is backed by a <see cref="UserEventLifetime"/>.
	/// If you do, that will lead to a loss of data in <see cref="Data1"/> and <see cref="Data1"/> won't be accessable anymore.
	/// </para>
	/// <para>
	/// You can freely set this to anything of your liking, as long as you don't intend to use <see cref="Data1"/> as a <em>managed</em> user defined data slot.
	/// </para>
	/// </remarks>
	public IntPtr RawData1
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get { unsafe { return unchecked((IntPtr)mData1); } }
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set { unsafe { mData1 = unchecked((void*)value); } }
	}

	/// <summary>
	/// Gets or sets the value of the second <em>managed</em> user defined data slot
	/// </summary>
	/// <value>
	/// The value of the second <em>managed</em> user defined data slot
	/// </value>
	/// <remarks>
	/// <para>
	/// You should only access this property if the user defined event is backed by a <see cref="UserEventLifetime"/>.
	/// If it's not, or the <see cref="UserEventLifetime"/> is already disposed, accessing this property will lead to a <see cref="InvalidOperationException"/> been thrown.
	/// </para>
	/// <para>
	/// Note: If you intend to use this property, don't overwrite the value of it's associated <em>raw</em> user defined data slot, <see cref="RawData2"/>.
	/// If you do, that will lead to a loss of data in <see cref="Data2"/> and <see cref="Data2"/> won't be accessable anymore.
	/// </para>
	/// </remarks>
	/// <inheritdoc cref="UserEventLifetime.GetData2(ref readonly UserEvent)"/>
	/// <inheritdoc cref="UserEventLifetime.SetData2(ref readonly UserEvent, object?)"/>
	public readonly object? Data2
	{
		get => UserEventLifetime.GetData2(in this);
		set => UserEventLifetime.SetData2(in this, value);
	}

	/// <summary>
	/// Gets or sets the value of the second <em>raw</em> user defined data slot
	/// </summary>
	/// <value>
	/// The value of the second <em>raw</em> user defined data slot
	/// </value><remarks>
	/// <para>
	/// You should not overwrite the value of this property if the user defined event is backed by a <see cref="UserEventLifetime"/>.
	/// If you do, that will lead to a loss of data in <see cref="Data2"/> and <see cref="Data2"/> won't be accessable anymore.
	/// </para>
	/// <para>
	/// You can freely set this to anything of your liking, as long as you don't intend to use <see cref="Data2"/> as a <em>managed</em> user defined data slot.
	/// </para>
	/// </remarks>
	public IntPtr RawData2
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get { unsafe { return unchecked((IntPtr)mData2); } }
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set { unsafe { mData2 = unchecked((void*)value); } }
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
						.Append($", {nameof(Code)}: ")
						.Append(Code.ToString(format, formatProvider));

			if (UserEventLifetime.TryGetData1(in this, out var data))
			{
				builder.Append($", {nameof(Data1)}: ");

				switch (data)
				{
					case string str:
						builder.Append('"')
							   .Append(str)
							   .Append('"');
						break;

					case not null:
						builder.Append(data.ToString());
						break;

					default:
						builder.Append("null");
						break;
				}
			}
			else
			{
				builder.Append($", {nameof(RawData1)}: ")
					   .Append(RawData1.ToString(format, formatProvider));
			}

			if (UserEventLifetime.TryGetData2(in this, out data))
			{
				builder.Append($", {nameof(Data2)}: ");

				switch (data)
				{
					case string str:
						builder.Append('"')
							   .Append(str)
							   .Append('"');
						break;

					case not null:
						builder.Append(data.ToString());
						break;

					default:
						builder.Append("null");
						break;
				}
			}
			else
			{
				builder.Append($", {nameof(RawData2)}: ")
					   .Append(RawData2.ToString(format, formatProvider));
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
		charsWritten = 0;

		return SpanFormat.TryWrite("{ ", ref destination, ref charsWritten)
			&& ICommonEvent.TryPartiallyFormat(in this, ref destination, ref charsWritten, format)
			&& SpanFormat.TryWrite($", {nameof(WindowId)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(WindowId, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite($", {nameof(Code)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(Code, ref destination, ref charsWritten, format, provider)
			&& (UserEventLifetime.TryGetData1(in this, out var data)
				?  SpanFormat.TryWrite($", {nameof(Data1)}: ", ref destination, ref charsWritten)
				&& (data switch
				{
					string str           => SpanFormat.TryWrite('"', ref destination, ref charsWritten)
					                     && SpanFormat.TryWrite(str, ref destination, ref charsWritten)
										 && SpanFormat.TryWrite('"', ref destination, ref charsWritten),
					ISpanFormattable fmt => SpanFormat.TryWrite(fmt, ref destination, ref charsWritten),
					not null             => SpanFormat.TryWrite(data.ToString(), ref destination, ref charsWritten),
					_                    => SpanFormat.TryWrite("null", ref destination, ref charsWritten)
				})
				:  SpanFormat.TryWrite($", {nameof(RawData1)}: ", ref destination, ref charsWritten)
				&& SpanFormat.TryWrite(RawData1, ref destination, ref charsWritten, format, provider))
			&& (UserEventLifetime.TryGetData2(in this, out data)
				?  SpanFormat.TryWrite($", {nameof(Data2)}: ", ref destination, ref charsWritten)
				&& (data switch
				{
					string str           => SpanFormat.TryWrite('"', ref destination, ref charsWritten)
										 && SpanFormat.TryWrite(str, ref destination, ref charsWritten)
										 && SpanFormat.TryWrite('"', ref destination, ref charsWritten),
					ISpanFormattable fmt => SpanFormat.TryWrite(fmt, ref destination, ref charsWritten),
					not null             => SpanFormat.TryWrite(data.ToString(), ref destination, ref charsWritten),
					_                    => SpanFormat.TryWrite("null", ref destination, ref charsWritten)
				})
				:  SpanFormat.TryWrite($", {nameof(RawData2)}: ", ref destination, ref charsWritten)
				&& SpanFormat.TryWrite(RawData2, ref destination, ref charsWritten, format, provider))
			&& SpanFormat.TryWrite(" }", ref destination, ref charsWritten);
	}

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static implicit operator Event(in UserEvent @event) => new(in @event);

	/// <remarks>
	/// <para>
	/// The <see cref="Event.Type"/> of the given <paramref name="event"/> must be a valid value for <see cref="UserEvent"/>s.
	/// Otherwise, it will lead the method to throw an <see cref="ArgumentException"/>!
	/// Try registering user defined event type using <see cref="EventTypeExtensions.TryRegister(out EventType)"/> or <see cref="EventTypeExtensions.TryRegister(Span{EventType})"/>.
	/// </para>
	/// </remarks>
	/// <exception cref="ArgumentException">
	/// The <see cref="Event.Type"/> of the given <paramref name="event"/> was not valid for <see cref="UserEvent"/>s
	/// </exception>
	/// <inheritdoc/>
	public static explicit operator UserEvent(in Event @event)
	{
		if (!Accepts(@event.Type))
		{
			failEventArgumentIsNotUserEvent();
		}

		return @event.User;

		[DoesNotReturn]
		static void failEventArgumentIsNotUserEvent() => throw new ArgumentException($"{nameof(@event)} must be an {nameof(UserEvent)} by {nameof(Type)}", paramName: nameof(@event));
	}
}
