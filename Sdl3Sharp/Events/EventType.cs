using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sdl3Sharp.Events;

/// <summary>
/// Represents an event type for an <see cref="Event"/>
/// </summary>
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
[StructLayout(LayoutKind.Sequential)]
public readonly partial struct EventType :
	IEquatable<EventType>, IFormattable, ISpanFormattable, IEqualityOperators<EventType, EventType, bool>
{
	private readonly Kind mKind;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly string DebuggerDisplay => ToString(formatProvider: CultureInfo.InvariantCulture);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private EventType(Kind kind) => mKind = kind;

	/// <inheritdoc/>
	public readonly override bool Equals([NotNullWhen(true)] object? obj) => obj is EventType other && Equals(other);

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly bool Equals(EventType other) => mKind == other.mKind;

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly override int GetHashCode() => mKind.GetHashCode();

	/// <inheritdoc/>
	public readonly override string ToString() => ToString(format: default, formatProvider: default);

	/// <inheritdoc cref="IFormattable.ToString(string?, IFormatProvider?)"/>
	public readonly string ToString(IFormatProvider? formatProvider) => ToString(format: default, formatProvider);

	/// <inheritdoc cref="IFormattable.ToString(string?, IFormatProvider?)"/>
	public readonly string ToString(string? format) => ToString(format, formatProvider: default);

	/// <inheritdoc/>
	public readonly string ToString(string? format, IFormatProvider? formatProvider) => mKind switch
	{
		>= Kind.User and <= Kind.Last => $"{nameof(User)}({unchecked(mKind - Kind.User).ToString(format, formatProvider)})",

		_ => KnownKindToString(mKind) switch
		{
			string knonwKind => knonwKind,

			_ => mKind.ToString(format)
		}
	};

	/// <inheritdoc/>
	public readonly bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format = default, IFormatProvider? provider = default)
	{
		static bool tryWriteSpan(ReadOnlySpan<char> value, ref Span<char> destination, ref int charsWritten)
		{
			var result = value.TryCopyTo(destination);

			if (result)
			{
				destination = destination[value.Length..];
				charsWritten += value.Length;
			}

			return result;
		}

		static bool tryWriteUInt(uint value, ref Span<char> destination, ref int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
		{
			var result = value.TryFormat(destination, out var tmp, format, provider);

			if (result)
			{
				destination = destination[tmp..];
				charsWritten += tmp;
			}

			return result;
		}

		static bool tryWriteChar(char value, ref Span<char> destination, ref int charsWritten)
		{
			if (destination.Length is > 0)
			{
				destination[0] = value;
				charsWritten += 1;

				return true;
			}

			return false;
		}

		static bool tryWriteKind(Kind value, ref Span<char> destination, ref int charsWritten, ReadOnlySpan<char> format)
		{
			var result = Enum.TryFormat(value, destination, out var tmp, format);

			if (result)
			{
				destination = destination[tmp..];
				charsWritten += tmp;
			}

			return result;
		}

		charsWritten = 0;

		return mKind switch
		{
			>= Kind.User and <= Kind.Last
				=> tryWriteSpan($"{nameof(User)}(", ref destination, ref charsWritten)
				&& tryWriteUInt(unchecked(mKind - Kind.User), ref destination, ref charsWritten, format, provider)
				&& tryWriteChar(')', ref destination, ref charsWritten),

			_ => KnownKindToString(mKind) switch
			{
				string knownKind => tryWriteSpan(knownKind, ref destination, ref charsWritten),

				_ => tryWriteKind(mKind, ref destination, ref charsWritten, format)
			}
		};
	}

	/// <summary>
	/// Tries to get the user value used to identify this user event type
	/// </summary>
	/// <param name="userValue">The user value used to identify this user event type</param>
	/// <returns><c><see langword="true"/></c> if this instance represents a user event type and <paramref name="userValue"/> is the user value which identifies it; otherwise, <c><see langword="false"/></c></returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly bool TryGetUserValue(out int userValue)
	{
		if (mKind is >= Kind.User and <= Kind.Last)
		{
			userValue = unchecked((int)(mKind - Kind.User));

			return true;
		}

		userValue = default;

		return false;
	}

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator ==(EventType left, EventType right) => left.mKind == right.mKind;

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator !=(EventType left, EventType right) => right.mKind != left.mKind;
}
