using Sdl3Sharp.Internal;
using Sdl3Sharp.Internal.Interop;
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
	[FieldOffset(0)] internal ClipboardEvent Clipboard;

	/// <summary>
	/// Creates a new <see cref="Event"/> from a <see cref="ClipboardEvent"/>
	/// </summary>
	/// <param name="event">The <see cref="ClipboardEvent"/> to store into the newly created <see cref="Event"/></param>
	public Event(in ClipboardEvent @event) :
#pragma warning disable IDE0034 // Leave it for explicitness sake
		this(default(IUnsafeConstructorDispatch?))
#pragma warning restore IDE0034
		=> Clipboard = @event;
}

/// <summary>
/// Represents an event that occurs when the contents of the clipboard have changed
/// </summary>
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
[StructLayout(LayoutKind.Sequential)]
public struct ClipboardEvent : ICommonEvent<ClipboardEvent>, IFormattable, ISpanFormattable
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly string DebuggerDisplay => ToString(formatProvider: CultureInfo.InvariantCulture);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private static bool Accepts(EventType type) => type is EventType.ClipboardUpdated;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static bool ICommonEvent<ClipboardEvent>.Accepts(EventType type) => Accepts(type);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static ref ClipboardEvent ICommonEvent<ClipboardEvent>.GetReference(ref Event @event) => ref @event.Clipboard;

	private CommonEvent mCommon;
	private CBool mOwner;
	private int mNumMimeTypes;
	private unsafe byte** mMimeTypes;

	/// <remarks>
	/// <para>
	/// When setting this property, the value must be <see cref="EventType.ClipboardUpdated"/>.
	/// Otherwise, it will lead the property to throw an <see cref="ArgumentException"/>!
	/// </para>
	/// </remarks>
	/// <exception cref="ArgumentException">
	/// When setting this property, the value was not <see cref="EventType.ClipboardUpdated"/>
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
			static void failValueArgumentIsNotValid() => throw new ArgumentException($"The given {nameof(value)} is not a valid value for the {nameof(Type)} of a {nameof(ClipboardEvent)}", paramName: nameof(value));
		}
	}

	/// <inheritdoc/>
	public ulong Timestamp
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mCommon.Timestamp;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mCommon.Timestamp = value;
	}

	/// <summary>
	/// Gets or sets a value indicating whether SDL "owns" the clipboard, meaning it's an internal update event
	/// </summary>
	/// <value>
	/// A value indicating whether SDL "owns" the clipboard, meaning it's an internal update event
	/// </value>
	public bool IsOwned
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mOwner;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mOwner = value;
	}

	/// <summary>
	/// Gets a list of currently available mime types
	/// </summary>
	/// <value>
	/// A list of currently available mime types
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
	public string[] MimeTypes
	{
		readonly get
		{
			unsafe
			{
				var mimeTypes = mMimeTypes;
				var numMimeTypes = mNumMimeTypes;

				if (mimeTypes is null || numMimeTypes is not > 0)
				{
					return [];
				}

				var result = GC.AllocateUninitializedArray<string>(numMimeTypes);

				foreach (ref var mimeType in result.AsSpan())
				{
					mimeType = Utf8StringMarshaller.ConvertToManaged(*mimeTypes++)!;
				}

				return result;
			}
		}

		[Obsolete($"Setting {nameof(MimeTypes)} is not supported yet.")]
		[DoesNotReturn]
		set => throw new NotSupportedException($"Setting {nameof(MimeTypes)} is not supported");
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
		unsafe
		{
			var builder = Shared.StringBuilder;
			try
			{
				ICommonEvent.PartiallyAppend(in this, builder.Append("{ "), format)
							.Append($", {nameof(IsOwned)}: ")
							.Append(IsOwned)
							.Append($", {nameof(MimeTypes)}: [");

				var mimeTypes = mMimeTypes;
				var mimeTypesEnd = mimeTypes + mNumMimeTypes;

				if (mimeTypes is not null && mimeTypes < mimeTypesEnd)
				{
					builder.Append(" \"")
						   .Append(Utf8StringMarshaller.ConvertToManaged(*mimeTypes++)!)
						   .Append('"');

					while (mimeTypes < mimeTypesEnd)
					{
						builder.Append(" \"")
							   .Append(Utf8StringMarshaller.ConvertToManaged(*mimeTypes++)!)
							   .Append('"');
					}

					builder.Append(' ');
				}

				return builder.Append("] }")
							  .ToString();
			}
			finally
			{
				builder.Clear();
			}
		}
	}

	/// <inheritdoc/>
	public readonly bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format = default, IFormatProvider? provider = default)
	{
		unsafe
		{
			charsWritten = 0;

			if ( !(SpanFormat.TryWrite("{ ", ref destination, ref charsWritten)
				&& ICommonEvent.TryPartiallyFormat(in this, ref destination, ref charsWritten, format)
				&& SpanFormat.TryWrite($", {nameof(IsOwned)}: ", ref destination, ref charsWritten)
				&& SpanFormat.TryWrite(IsOwned, ref destination, ref charsWritten)
				&& SpanFormat.TryWrite($", {nameof(MimeTypes)}: [", ref destination, ref charsWritten)))
			{
				return false;
			}

			var mimeTypes = mMimeTypes;
			var mimeTypesEnd = mimeTypes + mNumMimeTypes;

			if (mimeTypes is not null && mimeTypes < mimeTypesEnd)
			{
				if ( !(SpanFormat.TryWrite(" \"", ref destination, ref charsWritten)
					&& SpanFormat.TryWriteUtf8(MemoryMarshal.CreateReadOnlySpanFromNullTerminated(*mimeTypes++), ref destination, ref charsWritten)
					&& SpanFormat.TryWrite('"', ref destination, ref charsWritten)))
				{
					return false;
				}

				while (mimeTypes < mimeTypesEnd)
				{
					if ( !(SpanFormat.TryWrite(" \"", ref destination, ref charsWritten)
						&& SpanFormat.TryWriteUtf8(MemoryMarshal.CreateReadOnlySpanFromNullTerminated(*mimeTypes++), ref destination, ref charsWritten)
						&& SpanFormat.TryWrite('"', ref destination, ref charsWritten)))
					{
						return false;
					}
				}

				if (!SpanFormat.TryWrite(' ', ref destination, ref charsWritten))
				{
					return false; 
				}
			}

			return SpanFormat.TryWrite("] }", ref destination, ref charsWritten);
		}
	}

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static implicit operator Event(in ClipboardEvent @event) => new(in @event);

	/// <remarks>
	/// <para>
	/// The <see cref="Event.Type"/> of the given <paramref name="event"/> must be <see cref="EventType.ClipboardUpdated"/>.
	/// Otherwise, it will lead the method to throw an <see cref="ArgumentException"/>!
	/// </para>
	/// </remarks>
	/// <exception cref="ArgumentException">
	/// The <see cref="Event.Type"/> of the given <paramref name="event"/> was not <see cref="EventType.ClipboardUpdated"/>
	/// </exception>
	/// <inheritdoc/>
	public static explicit operator ClipboardEvent(in Event @event)
	{
		if (!Accepts(@event.Type))
		{
			failEventArgumentIsNotClipboardEvent();
		}

		return @event.Clipboard;

		[DoesNotReturn]
		static void failEventArgumentIsNotClipboardEvent() => throw new ArgumentException($"{nameof(@event)} must be a {nameof(ClipboardEvent)} by {nameof(Type)}", paramName: nameof(@event));
	}
}
