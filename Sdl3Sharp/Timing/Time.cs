using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sdl3Sharp.Timing;

[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
[StructLayout(LayoutKind.Sequential)]
public readonly partial struct Time(long nanosecondsSinceUnixEpoch) :
	IComparable, IComparable<Time>, IEquatable<Time>, IFormattable, ISpanFormattable, IComparisonOperators<Time, Time, bool>, IEqualityOperators<Time, Time, bool>
{
	public static Time FromPosixTime(long seconds, long nanoseconds) => new(unchecked(seconds * NanosecondsPerSecond + nanoseconds));

	public static Time FromWindowsFileTime(long fileTime)
	{
		unsafe
		{
			return SDL_TimeFromWindows(unchecked(*(uint*)&fileTime), unchecked(*(((uint*)&fileTime) + 1)));
		}
	}

	public static bool TryConvertFromDateTime(in DateTime dateTime, out Time time)
	{
		unsafe
		{
			Unsafe.SkipInit(out Time tmp);

			bool result;

			// we use a local variable here and then copy it back into the given reference,
			// so that we're safe, if the given reference happens to point to the managed heap
			//
			// 'DateTime' instances are too big to justify copying them from a temporary variable,
			// instead we fix the incoming reference and pass it directly
			fixed (DateTime* dateTimePtr = &dateTime)
			{
				result = SDL_DateTimeToTime(dateTimePtr, &tmp);
			}

			time = tmp;

			return result;
		}
	}

	public static bool TryGetCurrentTime(out Time time)
	{
		unsafe
		{
			Unsafe.SkipInit(out Time tmp);

			// we use a local variable here and then copy it back into the given reference,
			// so that we're safe, if the given reference happens to point to the managed heap
			bool result = SDL_GetCurrentTime(&tmp);

			time = tmp;

			return result;
		}
	}

	public readonly long NanosecondsSinceUnixEpoch { get => nanosecondsSinceUnixEpoch; }

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly string DebuggerDisplay => ToString(formatProvider: CultureInfo.InvariantCulture);

	public readonly int CompareTo(object? obj)
	{
		return obj switch
		{
			null => 1,
			Time other => CompareTo(other),
			_ => failObjArgumentIsNotTime()
		};

		[DoesNotReturn]
		static int failObjArgumentIsNotTime() => throw new ArgumentException(message: $"{nameof(obj)} is not of type {nameof(Time)}", paramName: nameof(obj));
	}

	public readonly int CompareTo(Time other) => nanosecondsSinceUnixEpoch.CompareTo(other.NanosecondsSinceUnixEpoch);

	public readonly override bool Equals([NotNullWhen(true)] object? obj) => obj is Time other && Equals(other);

	public readonly bool Equals(Time other) => nanosecondsSinceUnixEpoch == other.NanosecondsSinceUnixEpoch;

	public readonly override int GetHashCode() => nanosecondsSinceUnixEpoch.GetHashCode();

	public readonly (long seconds, long nanoseconds) ToPosixTime() => long.DivRem(nanosecondsSinceUnixEpoch, NanosecondsPerSecond);

	public readonly override string ToString() => ToString(format: default, formatProvider: default);

	public readonly string ToString(IFormatProvider? formatProvider) => ToString(format: default, formatProvider);

	public readonly string ToString(string? format) => ToString(format, formatProvider: default);

	public readonly string ToString(string? format, IFormatProvider? formatProvider) => nanosecondsSinceUnixEpoch.ToString(format, formatProvider);

	public readonly long ToWindowFileTime()
	{
		unsafe
		{
			Unsafe.SkipInit(out long fileTime);

			SDL_TimeToWindows(this, unchecked((uint*)&fileTime), unchecked(((uint*)&fileTime) + 1));

			return fileTime;
		}
	}

	public readonly bool TryConvertToDateTime(out DateTime dateTime, bool convertToLocalTime = true)
	{
		unsafe
		{
			// 'DateTime' instances are too big to justify copying them from a temporary variable,
			// instead we fix the outgoing reference and pass it directly to the native function
			fixed (DateTime* dateTimePtr = &dateTime)
			{
				return SDL_TimeToDateTime(this, dateTimePtr, convertToLocalTime);
			}
		}
	}

	public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format = default, IFormatProvider? provider = default) => nanosecondsSinceUnixEpoch.TryFormat(destination, out charsWritten, format, provider);

	public static bool operator >(Time left, Time right) => left.NanosecondsSinceUnixEpoch > right.NanosecondsSinceUnixEpoch;

	public static bool operator >=(Time left, Time right) => left.NanosecondsSinceUnixEpoch >= right.NanosecondsSinceUnixEpoch;

	public static bool operator <(Time left, Time right) => left.NanosecondsSinceUnixEpoch < right.NanosecondsSinceUnixEpoch;

	public static bool operator <=(Time left, Time right) => left.NanosecondsSinceUnixEpoch <= right.NanosecondsSinceUnixEpoch;

	public static bool operator ==(Time left, Time right) => left.NanosecondsSinceUnixEpoch == right.NanosecondsSinceUnixEpoch;

	public static bool operator !=(Time left, Time right) => left.NanosecondsSinceUnixEpoch != right.NanosecondsSinceUnixEpoch;
}
