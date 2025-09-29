using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sdl3Sharp.Timing;

/// <summary>
/// Represents a calendar date and time, broken down into its parts
/// </summary>
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
[StructLayout(LayoutKind.Sequential)]
public readonly partial struct DateTime
	: IEquatable<DateTime>, IFormattable, ISpanFormattable, IEqualityOperators<DateTime, DateTime, bool>
{
	private readonly int mYear, mMonth, mDay, mHour, mMinute, mSecond, mNanosecond, mDayOfWeek, mUtcOffset;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private string DebuggerDisplay => ToString(formatProvider: CultureInfo.InvariantCulture);

	/// <summary>
	/// Creates a new <see cref="DateTime"/>
	/// </summary>
	/// <param name="year">The year part of the <see cref="DateTime"/></param>
	/// <param name="month">The month part of the <see cref="DateTime"/> (in the range from inclusive <c>1</c> to inclusive <c>12</c>)</param>
	/// <param name="day">The day part of the <see cref="DateTime"/> (in the range from inclusive <c>1</c> to inclusive <c>28</c>, <c>29</c>, <c>30</c>, or <c>31</c> depending on the <paramref name="month"/> and the <paramref name="year"/>)</param>
	/// <param name="hour">The hour part of the <see cref="DateTime"/> (in the range from inclusive <c>0</c> to inclusive <c>23</c>)</param>
	/// <param name="minute">The minute part of the <see cref="DateTime"/> (in the range from inclusive <c>0</c> to inclusive <c>59</c>)</param>
	/// <param name="second">The second part of the <see cref="DateTime"/> (in the range from inclusive <c>0</c> to inclusive <c>59</c>)</param>
	/// <param name="nanosecond">The nanosecond part of the <see cref="DateTime"/> (in the range from inclusive <c>0</c> to inclusive <c>999999999</c>)</param>
	/// <param name="utcOffset">The number of seconds the <see cref="DateTime"/> (as a local time) is east of UTC</param>
	/// <remarks>
	/// In contrast to most of the remaining API which uses the <c>Try</c>-method pattern, this constructor intentionally fails by throwing an exception.
	/// If you want to handle failures wrap the call to this constructor in a <c><see langword="try"/></c>-block,
	/// and check <see cref="Error.TryGet(out string?)"/> for more information when <c><see langword="catch"/></c>ing a <see cref="SdlException"/>.
	/// </remarks>
	/// <exception cref="ArgumentOutOfRangeException">
	/// <paramref name="month"/> is less than <c>1</c> or greater than <c>12</c>
	/// - or -
	/// <paramref name="day"/> is less than <c>1</c> or greater than <c>31</c>
	/// - or -
	/// <paramref name="hour"/> is less than <c>0</c> or greater than <c>23</c>
	/// - or -
	/// <paramref name="minute"/> is less than <c>0</c> or greater than <c>59</c>
	/// - or -
	/// <paramref name="second"/> is less than <c>0</c> or greater than <c>59</c>
	/// - or -
	/// <paramref name="nanosecond"/> is less than <c>0</c> or greater than <c>999999999</c>
	/// </exception>
	/// <exception cref="SdlException">Couldn't get the day of the week from SDL (check <see cref="Error.TryGet(out string?)"/> for more information)</exception>
	public DateTime(int year = 0, int month = 1, int day = 1, int hour = 0, int minute = 0, int second = 0, int nanosecond = 0, int utcOffset = 0)
	{
		if (month is < 1 or > 12)
		{
			failMonthArgumentOutOfRange();
		}

		if (day is < 1 or > 31)
		{
			failDayArgumentOutOfRange();
		}
		
		if (hour is < 0 or > 23)
		{
			failHourArgumentOutOfRange();
		}

		if (minute is < 0 or > 59)
		{
			failMinuteArgumentOutOfRange();
		}
		
		if (second is < 0 or > 59)
		{
			failSecondArgumentOutOfRange();
		}

		if (nanosecond is < 0 or > 999_999_999)
		{
			failNanosecondArgumentOutOfRange();
		}

		var dayOfWeek = SDL_GetDayOfWeek(year, month, day);

		if (dayOfWeek is < 0 or > 6)
		{
			failCouldNotGetDayOfWeek();
		}

		mYear = year;
		mMonth = month;
		mDay = day;
		mHour = hour;
		mMinute = minute;
		mSecond = second;
		mNanosecond = nanosecond;
		mDayOfWeek = dayOfWeek;
		mUtcOffset = utcOffset;

		[DoesNotReturn]
		static void failMonthArgumentOutOfRange() => throw new ArgumentOutOfRangeException(nameof(month));

		[DoesNotReturn]
		static void failDayArgumentOutOfRange() => throw new ArgumentOutOfRangeException(nameof(day));

		[DoesNotReturn]
		static void failHourArgumentOutOfRange() => throw new ArgumentOutOfRangeException(nameof(hour));

		[DoesNotReturn]
		static void failMinuteArgumentOutOfRange() => throw new ArgumentOutOfRangeException(nameof(minute));

		[DoesNotReturn]
		static void failSecondArgumentOutOfRange() => throw new ArgumentOutOfRangeException(nameof(second));

		[DoesNotReturn]
		static void failNanosecondArgumentOutOfRange() => throw new ArgumentOutOfRangeException(nameof(nanosecond));

		[DoesNotReturn]
		static void failCouldNotGetDayOfWeek() => throw new SdlException("Couldn't get the day of the week from SDL");
	}

	/// <summary>
	/// Gets the year part of the <see cref="DateTime"/>
	/// </summary>
	/// <value>
	/// The year part of the <see cref="DateTime"/>
	/// </value>
	public readonly int Year { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mYear; }

	/// <summary>
	/// Gets the month part of the <see cref="DateTime"/>
	/// </summary>
	/// <value>
	/// The month part of the <see cref="DateTime"/> (in the range from inclusive <c>1</c> to inclusive <c>12</c>)
	/// </value>
	public readonly int Month { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mMonth; }

	/// <summary>
	/// Gets the day part of the <see cref="DateTime"/>
	/// </summary>
	/// <value>
	/// The day part of the <see cref="DateTime"/> (in the range from inclusive <c>1</c> to inclusive <c>28</c>, <c>29</c>, <c>30</c>, or <c>31</c> depending on the <see cref="Month"/> and <see cref="Year"/> of the <see cref="DateTime"/>)
	/// </value>
	public readonly int Day { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mDay; }

	/// <summary>
	/// Gets the hour part of the <see cref="DateTime"/>
	/// </summary>
	/// <value>
	/// The hour part of the <see cref="DateTime"/> (in the range from inclusive <c>0</c> to inclusive <c>23</c>)
	/// </value>
	public readonly int Hour { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mHour; }

	/// <summary>
	/// Gets the minute part of the <see cref="DateTime"/>
	/// </summary>
	/// <value>
	/// The minute part of the <see cref="DateTime"/> (in the range from inclusive <c>0</c> to inclusive <c>59</c>)
	/// </value>
	public readonly int Minute { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mMinute; }

	/// <summary>
	/// Gets the second part of the <see cref="DateTime"/>
	/// </summary>
	/// <value>
	/// The second part of the <see cref="DateTime"/> (in the range from inclusive <c>0</c> to inclusive <c>59</c>)
	/// </value>
	public readonly int Second { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mSecond; }

	/// <summary>
	/// Gets the nanosecond part of the <see cref="DateTime"/>
	/// </summary>
	/// <value>
	/// The nanosecond part of the <see cref="DateTime"/> (in the range from inclusive <c>0</c> to inclusive <c>999999999</c>)
	/// </value>
	public readonly int Nanosecond { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mNanosecond; }

	/// <summary>
	/// Gets the day of the week represented by the <see cref="DateTime"/>
	/// </summary>
	/// <value>
	/// The day of the week represented by the <see cref="DateTime"/> (<see cref="DayOfWeek.Sunday"/>, <see cref="DayOfWeek.Monday"/>, <see cref="DayOfWeek.Tuesday"/>, <see cref="DayOfWeek.Wednesday"/>, <see cref="DayOfWeek.Thursday"/>, <see cref="DayOfWeek.Friday"/>, or <see cref="DayOfWeek.Saturday"/>)
	/// </value>
	public readonly DayOfWeek DayOfWeek { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => unchecked((DayOfWeek)mDayOfWeek); }

	/// <summary>
	/// Gets the number of seconds the <see cref="DateTime"/> (as a local time) is east of UTC
	/// </summary>
	/// <value>
	/// The number of seconds the <see cref="DateTime"/> (as a local time) is east of UTC
	/// </value>
	public readonly int UtcOffset { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mUtcOffset; }

	/// <summary>
	/// Gets the day of year represented by the <see cref="DateTime"/>
	/// </summary>
	/// <value>
	/// The day of year represented by the <see cref="DateTime"/> (in the range from inclusive <c>0</c> to inclusive <c>364</c> or <c>365</c> depending on the <see cref="Year"/> of the <see cref="DateTime"/>)
	/// </value>
	/// <exception cref="SdlException">Couldn't get the day of year from SDL (check <see cref="Error.TryGet(out string?)"/> for more information)</exception>
	/// <remarks>
	/// In contrast to most of the remaining API which uses the <c>Try</c>-method pattern, this property intentionally fails by throwing an exception.
	/// If you want to handle failures wrap the call to this property in a <c><see langword="try"/></c>-block,
	/// and check <see cref="Error.TryGet(out string?)"/> for more information when <c><see langword="catch"/></c>ing a <see cref="SdlException"/>.
	/// </remarks>
	public readonly int DayOfYear
	{		
		get
		{
			// this property uses fail-by-throw instead of a Try-pattern, since errors should be really 'exceptional' for correctly initialized 'DateFormat's
			if (!TryGetDayOfYear(mYear, mMonth, mDay, out var dayOfYear))
			{
				failCouldNotGetDayOfYear();
			}

			return dayOfYear;

			[DoesNotReturn]
			static void failCouldNotGetDayOfYear() => throw new SdlException("Couldn't get the day of year from SDL");
		}
	}

	/// <summary>
	/// Gets the number of days in the month represented by the <see cref="DateTime"/>
	/// </summary>
	/// <value>
	/// The number of days in the month represented by the <see cref="DateTime"/> (<c>28</c>, <c>29</c>, <c>30</c>, or <c>31</c> depending on the <see cref="Month"/> and <see cref="Year"/> of the <see cref="DateTime"/>)
	/// </value>
	/// <exception cref="SdlException">Couldn't get the days of the month from SDL (check <see cref="Error.TryGet(out string?)"/> for more information)</exception>
	/// <remarks>
	/// In contrast to most of the remaining API which uses the <c>Try</c>-method pattern, this property intentionally fails by throwing an exception.
	/// If you want to handle failures wrap the call to this property in a <c><see langword="try"/></c>-block,
	/// and check <see cref="Error.TryGet(out string?)"/> for more information when <c><see langword="catch"/></c>ing a <see cref="SdlException"/>.
	/// </remarks>
	public readonly int DaysInMonth
	{
		get
		{
			// this property uses fail-by-throw instead of a Try-pattern, since errors should be really 'exceptional' for correctly initialized 'DateFormat's
			if (!TryGetDaysInMonth(mYear, mMonth, out var daysInMonth))
			{
				failCouldNotGetDaysInMonth();
			}

			return daysInMonth;

			[DoesNotReturn]
			static void failCouldNotGetDaysInMonth() => throw new SdlException("Couldn't get the days of the month from SDL");
		}
	}

	/// <summary>
	/// Tries to convert a <see cref="Time"/> to a <see cref="DateTime"/>
	/// </summary>
	/// <param name="time">The <see cref="Time"/> to be converted</param>
	/// <param name="dateTime">The resulting <see cref="DateTime"/></param>
	/// <param name="convertToLocalTime">A value indicating whether the resulting <paramref name="dateTime"/> should be expressed in local time (if <c><see langword="true"/></c>), or otherwise be in Universal Coordinated Time (UTC) (if <c><see langword="false"/></c>)</param>
	/// <returns><c><see langword="true"/></c>, if the given <see cref="Time"/> could be successfully converted to a <see cref="DateTime"/>; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	public static bool TryConvertFromTime(Time time, out DateTime dateTime, bool convertToLocalTime = true) => time.TryConvertToDateTime(out dateTime, convertToLocalTime);

	/// <summary>
	/// Tries to get the day of the week for a specific <paramref name="year"/>, a specific <paramref name="month"/>, and a specific <paramref name="day"/>
	/// </summary>
	/// <param name="year">The year part of the date</param>
	/// <param name="month">The month part of the date</param>
	/// <param name="day">The day part of the date</param>
	/// <param name="dayOfWeek">The resulting day of the week for the given date (<see cref="DayOfWeek.Sunday"/>, <see cref="DayOfWeek.Monday"/>, <see cref="DayOfWeek.Tuesday"/>, <see cref="DayOfWeek.Wednesday"/>, <see cref="DayOfWeek.Thursday"/>, <see cref="DayOfWeek.Friday"/>, or <see cref="DayOfWeek.Saturday"/>)</param>
	/// <returns><c><see langword="true"/></c>, if the given date is valid and the resulting day of week was successfully determined; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	public static bool TryGetDayOfWeek(int year, int month, int day, out DayOfWeek dayOfWeek)
	{
		dayOfWeek = unchecked((DayOfWeek)SDL_GetDayOfWeek(year, month, day));

		// this may seem a little bit convoluted, but this way is future proof, if some day the layout of 'System.DayOfWeek' should change
		return unchecked((int)dayOfWeek) is not (< 0 or > 6);
	}

	/// <summary>
	/// Tries to get the day of year for a specific <paramref name="year"/>, a specific <paramref name="month"/>, and a specific <paramref name="day"/>
	/// </summary>
	/// <param name="year">The year part of the date</param>
	/// <param name="month">The month part of the date</param>
	/// <param name="day">The day part of the date</param>
	/// <param name="dayOfYear">The resulting day of year for the given date (in the range from inclusive <c>0</c> to inclusive <c>364</c> or <c>365</c> depending on the <paramref name="year"/>)</param>
	/// <returns><c><see langword="true"/></c>, if the given date is valid and the resulting day of year was successfully determined; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	public static bool TryGetDayOfYear(int year, int month, int day, out int dayOfYear)
	{
		dayOfYear = SDL_GetDayOfYear(year, month, day);

		return dayOfYear is not (< 0 or > 365);
	}

	/// <summary>
	/// Tries to get the days in a month for a specific <paramref name="year"/> and a specific <paramref name="month"/>
	/// </summary>
	/// <param name="year">The year part of the date</param>
	/// <param name="month">The month part of the date</param>
	/// <param name="daysInMonth">The resulting days in the month for the given date (<c>28</c>, <c>29</c>, <c>30</c>, or <c>31</c> depending on the <paramref name="month"/> and the <paramref name="year"/>)</param>
	/// <returns><c><see langword="true"/></c>, if the given date is valid and the resulting days in the month were successfully determined; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	public static bool TryGetDaysInMonth(int year, int month, out int daysInMonth)
	{
		daysInMonth = SDL_GetDaysInMonth(year, month);

		return daysInMonth is not (< 0 or > 31);
	}

	/// <summary>
	/// Tries to get the current preferred date and formats for the system locale
	/// </summary>
	/// <param name="dateFormat">The current preferred date format</param>
	/// <param name="timeFormat">The current preferred time format</param>
	/// <returns><c><see langword="true"/></c>, if the current preferred date and time formats could be successfully retrieved; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This might be a "slow" call that has to query the operating system.
	/// It's best to ask for this once and save the results.
	/// However, the preferred formats can change, usually because the user has changed a system preference outside of your program.
	/// </para>
	/// <para>
	/// To get the current preferred date format only, use <see cref="DateFormat.TryGetLocalePreference(out DateFormat)"/> instead.
	/// </para>
	/// <para>
	/// To get the current preferred time format only, use <see cref="TimeFormat.TryGetLocalePreference(out TimeFormat)"/> instead.
	/// </para>
	/// </remarks>
	public static bool TryGetLocalePreferences(out DateFormat dateFormat, out TimeFormat timeFormat)
	{
		unsafe
		{
			Unsafe.SkipInit(out DateFormat dateTmp);
			Unsafe.SkipInit(out TimeFormat timeTmp);

			// we use local variables here and then copy them back into the given references,
			// so that we're safe, if the given references happen to point to the managed heap
			bool result = SDL_GetDateTimeLocalePreferences(dateFormat: &dateTmp, timeFormat: &timeTmp);

			dateFormat = dateTmp;
			timeFormat = timeTmp;

			return result;
		}
	}

	/// <summary>
	/// Deconstructs the <see cref="DateTime"/>
	/// </summary>
	/// <param name="year">The year part of the <see cref="DateTime"/></param>
	/// <param name="month">The month part of the <see cref="DateTime"/> (in the range from inclusive <c>1</c> to inclusive <c>12</c>)</param>
	/// <param name="day">The day part of the <see cref="DateTime"/> (in the range from inclusive <c>1</c> to inclusive <c>28</c>, <c>29</c>, <c>30</c>, or <c>31</c> depending on the <paramref name="month"/> and <paramref name="year"/>)</param>
	/// <param name="hour">The hour part of the <see cref="DateTime"/> (in the range from inclusive <c>0</c> to inclusive <c>23</c>)</param>
	/// <param name="minute">The minute part of the <see cref="DateTime"/> (in the range from inclusive <c>0</c> to inclusive <c>59</c>)</param>
	/// <param name="second">The second part of the <see cref="DateTime"/> (in the range from inclusive <c>0</c> to inclusive <c>59</c>)</param>
	/// <param name="nanosecond">The nanosecond part of the <see cref="DateTime"/> (in the range from inclusive <c>0</c> to inclusive <c>999999999</c>)</param>
	/// <param name="dayOfWeek">The day of the week represented by the <see cref="DateTime"/> (<see cref="DayOfWeek.Sunday"/>, <see cref="DayOfWeek.Monday"/>, <see cref="DayOfWeek.Tuesday"/>, <see cref="DayOfWeek.Wednesday"/>, <see cref="DayOfWeek.Thursday"/>, <see cref="DayOfWeek.Friday"/>, or <see cref="DayOfWeek.Saturday"/>)</param>
	/// <param name="utcOffset">The number of seconds the <see cref="DateTime"/> (as a local time) is east of UTC</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly void Deconstruct(out int year, out int month, out int day, out int hour, out int minute, out int second, out int nanosecond, out DayOfWeek dayOfWeek, out int utcOffset)
	{
		year = mYear;
		month = mMonth;
		day = mDay;
		hour = mHour;
		minute = mMinute;
		second = mSecond;
		nanosecond = mNanosecond;
		dayOfWeek = unchecked((DayOfWeek)mDayOfWeek);
		utcOffset = mUtcOffset;
	}

	/// <inheritdoc/>
	public readonly override bool Equals([NotNullWhen(true)] object? obj) => obj is DateTime other && Equals(in other);

	/// <inheritdoc/>
	readonly bool IEquatable<DateTime>.Equals(DateTime other) => Equals(in other);

	/// <inheritdoc cref="IEquatable{T}.Equals(T)"/>
	public readonly bool Equals(in DateTime other)
		=> mYear == other.mYear
		&& mMonth == other.mMonth
		&& mDay == other.mDay
		&& mHour == other.mHour
		&& mMinute == other.mMinute
		&& mSecond == other.mSecond
		&& mNanosecond == other.mNanosecond
		/* && mDayOfWeek == other.mDayOfWeek // we intentionally ignore the day of the week */
		&& mUtcOffset == other.mUtcOffset;

	/// <inheritdoc/>
	public readonly override int GetHashCode() => HashCode.Combine(
		mYear,
		mMonth,
		mDay,
		mHour,
		mMinute,
		mSecond,
		mNanosecond,
		/* mDayOfWeek, // we intentionally ignore the day of the week */
		mUtcOffset
	);

	/// <summary>
	/// Converts to <see cref="DateTime"/> to a <see cref="DateTimeOffset"/>
	/// </summary>
	/// <param name="nanosecondsRemainder">The remaining nanoseconds part of the <see cref="DateTime"/> which could not be expressed in the resulting <see cref="DateTimeOffset"/></param>
	/// <returns>A <see cref="DateTimeOffset"/> representing a similiar date and time as the <see cref="DateTime"/></returns>
	public readonly DateTimeOffset ToDateTimeOffset(out int nanosecondsRemainder)
	{
		(_, var milliseconds, var microseconds, nanosecondsRemainder) = Time.NanosecondsToSeconds(mNanosecond);

		return new DateTimeOffset(
			mYear, mMonth, mDay, mHour, mMinute, mSecond, milliseconds, microseconds,
			TimeSpan.FromMinutes(unchecked(((mUtcOffset + 30) / 60) + (mUtcOffset >> (8 * sizeof(int) - 1)))) // <- round to the nearest minute away from zero ('mUtfOffset' is an 'int', so '>>' is a signed right shift)
		);
	}

	/// <inheritdoc/>
	public readonly override string ToString() => ToString(format: default, formatProvider: default);

	/// <inheritdoc cref="IFormattable.ToString(string?, IFormatProvider?)"/>
	public readonly string ToString(IFormatProvider? formatProvider) => ToString(format: default, formatProvider);

	/// <inheritdoc cref="IFormattable.ToString(string?, IFormatProvider?)"/>
	public readonly string ToString(string? format) => ToString(format, formatProvider: default);

	/// <inheritdoc/>
	public readonly string ToString(string? format, IFormatProvider? formatProvider) => ToDateTimeOffset(out _).ToString(format, formatProvider);

	/// <summary>
	/// Tries to convert the <see cref="DateTime"/> to a <see cref="Time"/>
	/// </summary>
	/// <param name="time">The resulting <see cref="Time"/></param>
	/// <returns><c><see langword="true"/></c>, if the <see cref="DateTime"/> could be successfully converted to a <see cref="Time"/>; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	public readonly bool TryConvertToTime(out Time time) => Time.TryConvertFromDateTime(in this, out time);

	/// <inheritdoc/>
	public readonly bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format = default, IFormatProvider? provider = default) => ToDateTimeOffset(out _).TryFormat(destination, out charsWritten, format, provider);

	/// <inheritdoc cref="IEqualityOperators{TSelf, TOther, TResult}.operator==(TSelf, TOther)"/>
	public static bool operator ==(in DateTime left, in DateTime right) => left.Equals(in right);

	/// <inheritdoc/>
	static bool IEqualityOperators<DateTime, DateTime, bool>.operator ==(DateTime left, DateTime right) => left == right;

	/// <inheritdoc cref="IEqualityOperators{TSelf, TOther, TResult}.operator!=(TSelf, TOther)"/>
	public static bool operator !=(in DateTime left, in DateTime right) => !left.Equals(in right);

	/// <inheritdoc/>
	static bool IEqualityOperators<DateTime, DateTime, bool>.operator !=(DateTime left, DateTime right) => left != right;
}
