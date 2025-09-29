using Sdl3Sharp.Internal;
using Sdl3Sharp.Internal.Interop;
using Sdl3Sharp.Windowing;
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
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
#pragma warning disable IDE0034 // Leave it for explicitness sake
	public Event(in ClipboardEvent @event) : this(default(IUnsafeConstructorDispatch?)) => Clipboard = @event;
#pragma warning restore IDE0034
}

/// <summary>
/// Represents an event that occurs when the <see cref="Clipboard">clipboard</see> is being <see cref="EventType.Clipboard.Updated">updated</see>
/// </summary>
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
[StructLayout(LayoutKind.Sequential)]
public struct ClipboardEvent : ICommonEvent<ClipboardEvent>, IFormattable, ISpanFormattable
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly string DebuggerDisplay => ToString(formatProvider: CultureInfo.InvariantCulture);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private static bool Accepts(EventType type) => type == EventType.Clipboard.Updated;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static bool ICommonEvent<ClipboardEvent>.Accepts(EventType type) => Accepts(type);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static ref ClipboardEvent ICommonEvent<ClipboardEvent>.GetReference(ref Event @event) => ref @event.Clipboard;

	private CommonEvent<ClipboardEvent> mCommon;
	private CBool mOwner;
	private readonly int mNumMimeTypes;
	private readonly unsafe byte** mMimeTypes;

	/// <inheritdoc/>
	public EventType<ClipboardEvent> Type
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
	/// Gets or sets a value indicating if the application or SDL "owns" the clipboard (it's an internal update)
	/// </summary>
	/// <value>
	/// A value indicating if the application or SDL "owns" the clipboard (it's an internal update)
	/// </value>
	public bool IsOwned
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mOwner;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mOwner = value;
	}

	/// <summary>
	/// Gets a list of mime types available in the clipboard through the <see cref="EventType.Clipboard.Updated">update</see>
	/// </summary>
	/// <value>
	/// A list of mime types available in the clipboard through the <see cref="EventType.Clipboard.Updated">update</see>
	/// </value>
	/// <remarks>
	/// <para>
	/// Setting this property is currently not supported. Doing so will lead the setter to throw a <see cref="NotImplementedException"/>!
	/// </para>
	/// </remarks>
	/// <exception cref="NotImplementedException">Trying to set this property</exception>
	public string[]? MimeTypes
	{
		readonly get
		{
			unsafe
			{
				if (mMimeTypes is null)
				{
					return null;
				}

				if (mNumMimeTypes is not > 0)
				{
					return [];
				}

				var mimeTypes = GC.AllocateUninitializedArray<string>(mNumMimeTypes);

				var mimeTypePtr = mMimeTypes;
				foreach (ref var mimeType in mimeTypes.AsSpan())
				{
					mimeType = Utf8StringMarshaller.ConvertToManaged(*mimeTypePtr++);
				}

				return mimeTypes;
			}
		}

		[DoesNotReturn, Experimental("SDL2001")] // TODO: make SDL2001 the diagnostic id for 'Yet not supported API'
		set => throw new NotImplementedException($"Setting {nameof(MimeTypes)} is yet not supported.");
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
				ICommonEvent.PartialAppend(
					builder.Append("{ "),
					in this, format, formatProvider
				)
					.Append($", {nameof(IsOwned)}: ")
					.Append(IsOwned)
					.Append($", {nameof(MimeTypes)}: ");

				if (mMimeTypes is null)
				{
					builder.Append("null");
				}
				else if (mNumMimeTypes is not > 0)
				{
					builder.Append("[]");
				}
				else
				{
					builder.Append("[ ");

					var mimeTypesPtr = mMimeTypes;
					var mimeTypeUtf16 = Utf8StringMarshaller.ConvertToManaged(*mimeTypesPtr);

					if (mimeTypeUtf16 is not null)
					{
						builder.Append('"')
							.Append(mimeTypeUtf16)
							.Append('"');
					}
					else
					{
						builder.Append("null");
					}

					var mimeTypesEnd = mimeTypesPtr + mNumMimeTypes;
					while (++mimeTypesPtr < mimeTypesEnd)
					{
						builder.Append(", ");

						mimeTypeUtf16 = Utf8StringMarshaller.ConvertToManaged(*mimeTypesPtr);

						if (mimeTypeUtf16 is not null)
						{
							builder.Append('"')
								.Append(mimeTypeUtf16)
								.Append('"');
						}
						else
						{
							builder.Append("null");
						}
					}

					builder.Append(" ]");
				}

				builder.Append(" }");

				return builder.ToString();
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

			if(  !(SpanFormat.TryWrite(" {", ref destination, ref charsWritten)
				&& ICommonEvent.TryPartialFormat(in this, ref destination, ref charsWritten, format, provider)
				&& SpanFormat.TryWrite($", {nameof(IsOwned)}: ", ref destination, ref charsWritten)
				&& SpanFormat.TryWrite(IsOwned, ref destination, ref charsWritten)
				&& SpanFormat.TryWrite($", {nameof(MimeTypes)}: ", ref destination, ref charsWritten)))
			{
				return false;
			}

			if (MimeTypes is null)
			{
				if (!SpanFormat.TryWrite("null", ref destination, ref charsWritten))
				{
					return false;
				}
			}
			else if (mNumMimeTypes is not > 0)
			{
				if (!SpanFormat.TryWrite("[]", ref destination, ref charsWritten))
				{
					return false;
				}
			}
			else
			{
				if (!SpanFormat.TryWrite("[ ", ref destination, ref charsWritten))
				{
					return false;
				}

				var mimeTypesPtr = mMimeTypes;
				var mimeTypeUtf16 = Utf8StringMarshaller.ConvertToManaged(*mimeTypesPtr);

				if (mimeTypeUtf16 is not null)
				{
					if ( !(SpanFormat.TryWrite('"', ref destination, ref charsWritten)
						&& SpanFormat.TryWrite(mimeTypeUtf16, ref destination, ref charsWritten)
						&& SpanFormat.TryWrite('"', ref destination, ref charsWritten)))
					{
						return false;
					}
				}
				else
				{
					if (!SpanFormat.TryWrite("null", ref destination, ref charsWritten))
					{
						return false;
					}
				}

				var mimeTypesEnd = mimeTypesPtr + mNumMimeTypes;
				while (++mimeTypesPtr < mimeTypesEnd)
				{
					if (!SpanFormat.TryWrite(", ", ref destination, ref charsWritten))
					{
						return false;
					}

					mimeTypeUtf16 = Utf8StringMarshaller.ConvertToManaged(*mimeTypesPtr);

					if (mimeTypeUtf16 is not null)
					{
						if ( !(SpanFormat.TryWrite('"', ref destination, ref charsWritten)
							&& SpanFormat.TryWrite(mimeTypeUtf16, ref destination, ref charsWritten)
							&& SpanFormat.TryWrite('"', ref destination, ref charsWritten)))
						{
							return false;
						}
					}
					else
					{
						if (!SpanFormat.TryWrite("null", ref destination, ref charsWritten))
						{
							return false;
						}
					}
				}

				if (!SpanFormat.TryWrite(" ]", ref destination, ref charsWritten))
				{
					return false;
				}
			}

			return SpanFormat.TryWrite(" }", ref destination, ref charsWritten);
		}
	}

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static implicit operator Event(in ClipboardEvent @event) => new(in @event);

	/// <remarks>
	/// <para>
	/// The <see cref="Event.Type"/> of the given <paramref name="event"/> must be <see cref="EventType.Clipboard.Updated"/>.
	/// Otherwise, it will lead the method to throw an <see cref="ArgumentException"/>!
	/// </para>
	/// </remarks>
	/// <exception cref="ArgumentException">
	/// The <see cref="Event.Type"/> of the given <paramref name="event"/> was not <see cref="EventType.Clipboard.Updated"/>
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
