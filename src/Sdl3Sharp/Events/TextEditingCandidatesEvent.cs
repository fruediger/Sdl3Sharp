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
	[FieldOffset(0)] internal TextEditingCandidatesEvent EditCandidates;

	/// <summary>
	/// Creates a new <see cref="Event"/> from a <see cref="TextEditingCandidatesEvent"/>
	/// </summary>
	/// <param name="event">The <see cref="TextEditingCandidatesEvent"/> to store into the newly created <see cref="Event"/></param>
	public Event(in TextEditingCandidatesEvent @event) :
#pragma warning disable IDE0034 // Leave it for explicitness sake
		this(default(IUnsafeConstructorDispatch?))
#pragma warning restore IDE0034
		=> EditCandidates = @event;
}

/// <summary>
/// Represents an event that occurs when the list of keyboard IME candidates is displayed, shall be displayed, or changes
/// </summary>
/// <remarks>
/// <para>
/// Associated <see cref="EventType"/>s:
/// <list type="bullet">
/// <item><description><see cref="EventType.TextEditingCandidates"/></description></item>
/// </list>
/// </para>
/// </remarks>
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
[StructLayout(LayoutKind.Sequential)]
public struct TextEditingCandidatesEvent : ICommonEvent<TextEditingCandidatesEvent>, IFormattable, ISpanFormattable
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly string DebuggerDisplay => ToString(formatProvider: CultureInfo.InvariantCulture);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private static bool Accepts(EventType type) => type is EventType.TextEditingCandidates;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static bool ICommonEvent<TextEditingCandidatesEvent>.Accepts(EventType type) => Accepts(type);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static ref TextEditingCandidatesEvent ICommonEvent<TextEditingCandidatesEvent>.GetReference(ref Event @event) => ref @event.EditCandidates;

	private CommonEvent mCommon;
	private uint mWindowID;
	private unsafe readonly byte** mCandidates;
	private int mNumCandidates;
	private int mSelectedCandidate;
	private CBool mHorizontal;
	private readonly byte mPadding1, mPadding2, mPadding3;

	/// <remarks>
	/// <para>
	/// When setting this property, the value must be <see cref="EventType.TextEditingCandidates"/>.
	/// Otherwise, it will lead the property to throw an <see cref="ArgumentException"/>!
	/// </para>
	/// </remarks>
	/// <exception cref="ArgumentException">
	/// When setting this property, the value was not <see cref="EventType.TextEditingCandidates"/>
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
			static void failValueArgumentIsNotValid() => throw new ArgumentException($"The given {nameof(value)} is not a valid value for the {nameof(Type)} of a {nameof(TextEditingCandidatesEvent)}", paramName: nameof(value));
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
	/// Gets the list of keyboard IME candidates
	/// </summary>
	/// <value>
	/// The list of keyboard IME candidates
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
	public string[] Candidates
	{
		readonly get
		{
			unsafe
			{
				var candidates = mCandidates;
				var numCandidates = mNumCandidates;

				if (candidates is null || numCandidates is not > 0)
				{
					return [];
				}

				var result = GC.AllocateUninitializedArray<string>(numCandidates);

				foreach (ref var candidate in result.AsSpan())
				{
					candidate = Utf8StringMarshaller.ConvertToManaged(*candidates++) ?? string.Empty;
				}

				return result;
			}
		}

		[Obsolete($"Setting {nameof(Candidates)} is not supported yet.")]
		[DoesNotReturn]
		set => throw new NotSupportedException($"Setting {nameof(Candidates)} is not supported");
	}

	/// <summary>
	/// Gets or sets the index of the selected keyboard IME candidate into the <see cref="Candidates"/> list
	/// </summary>
	/// <value>
	/// The index of the selected keyboard IME candidate into the <see cref="Candidates"/> list, or <c>-1</c> if no candidate is selected
	/// </value>
	public int SelectedCandidate
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mSelectedCandidate;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mSelectedCandidate = value is not < 0 ? value : -1;
	}

	/// <summary>
	/// Gets or sets a value indicating whether the list of keyboard IME candidates is horizontal
	/// </summary>
	/// <value>
	/// A value indicating whether the list of keyboard IME candidates is horizontal
	/// </value>
	public bool IsHorizontal
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => mHorizontal;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => mHorizontal = value;
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
							.Append($", {nameof(WindowId)}: ")
							.Append(WindowId.ToString(format, formatProvider))
							.Append($", {nameof(Candidates)}: [");

				var candidates = mCandidates;
				var candidatesEnd = mCandidates + mNumCandidates;

				if (candidates is not null && candidates < candidatesEnd)
				{
					builder.Append(" \"")
						   .Append(Utf8StringMarshaller.ConvertToManaged(*candidates++) ?? string.Empty)
						   .Append('"');

					while (candidates < candidatesEnd)
					{
						builder.Append(", \"")
							   .Append(Utf8StringMarshaller.ConvertToManaged(*candidates++) ?? string.Empty)
							   .Append('"');
					}

					builder.Append(' ');
				}

				return builder.Append($"], {nameof(SelectedCandidate)}: ")
							  .Append(SelectedCandidate.ToString(format, formatProvider))
							  .Append($", {nameof(IsHorizontal)}: ")
							  .Append(IsHorizontal)
							  .Append(" }")
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
				&& SpanFormat.TryWrite($", {nameof(WindowId)}: ", ref destination, ref charsWritten)
				&& SpanFormat.TryWrite(WindowId, ref destination, ref charsWritten, format, provider)
				&& SpanFormat.TryWrite($", {nameof(Candidates)}: [", ref destination, ref charsWritten)))
			{
				return false;
			}

			var candidates = mCandidates;
			var candidatesEnd = candidates + mNumCandidates;

			if (candidates is not null && candidates < candidatesEnd)
			{
				if ( !(SpanFormat.TryWrite(" \"", ref destination, ref charsWritten)
					&& SpanFormat.TryWriteUtf8(MemoryMarshal.CreateReadOnlySpanFromNullTerminated(*candidates++), ref destination, ref charsWritten)
					&& SpanFormat.TryWrite('"', ref destination, ref charsWritten)))
				{
					return false; 
				}

				while (candidates < candidatesEnd)
				{
					if ( !(SpanFormat.TryWrite(", \"", ref destination, ref charsWritten)
						&& SpanFormat.TryWriteUtf8(MemoryMarshal.CreateReadOnlySpanFromNullTerminated(*candidates++), ref destination, ref charsWritten)
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

			return SpanFormat.TryWrite($"], {nameof(SelectedCandidate)}: ", ref destination, ref charsWritten)
				&& SpanFormat.TryWrite(SelectedCandidate, ref destination, ref charsWritten, format, provider)
				&& SpanFormat.TryWrite($", {nameof(IsHorizontal)}: ", ref destination, ref charsWritten)
				&& SpanFormat.TryWrite(IsHorizontal, ref destination, ref charsWritten)
				&& SpanFormat.TryWrite(" }", ref destination, ref charsWritten);
		}
	}

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static implicit operator Event(in TextEditingCandidatesEvent @event) => new(in @event);

	/// <remarks>
	/// <para>
	/// The <see cref="Event.Type"/> of the given <paramref name="event"/> must be <see cref="EventType.TextEditingCandidates"/>.
	/// Otherwise, it will lead the method to throw an <see cref="ArgumentException"/>!
	/// </para>
	/// </remarks>
	/// <exception cref="ArgumentException">
	/// The <see cref="Event.Type"/> of the given <paramref name="event"/> was not <see cref="EventType.TextEditingCandidates"/>
	/// </exception>
	/// <inheritdoc/>
	public static explicit operator TextEditingCandidatesEvent(in Event @event)
	{
		if (!Accepts(@event.Type))
		{
			failEventArgumentIsNotTextEditingCandidateEvent();
		}

		return @event.EditCandidates;

		[DoesNotReturn]
		static void failEventArgumentIsNotTextEditingCandidateEvent() => throw new ArgumentException($"{nameof(@event)} must be an {nameof(TextEditingCandidatesEvent)} by {nameof(Type)}", paramName: nameof(@event));
	}
}
