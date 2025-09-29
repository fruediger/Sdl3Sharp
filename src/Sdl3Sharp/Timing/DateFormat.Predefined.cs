using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Timing;

partial struct DateFormat
{
	/// <summary>
	/// Gets the "Year / Month / Day" date format
	/// </summary>
	/// <value>
	/// The "Year / Month / Day" date format
	/// </value>
	public static DateFormat YearMonthDay { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.YyyyMmDd); }

	/// <summary>
	/// Gets the "Day / Month / Year" date format
	/// </summary>
	/// <value>
	/// The "Day / Month / Year" date format
	/// </value>
	public static DateFormat DayMonthYear { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.DdMmYyyy); }

	/// <summary>
	/// Gets the "Month / Day / Year" date format
	/// </summary>
	/// <value>
	/// The "Month / Day / Year" date format
	/// </value>
	public static DateFormat MonthDayYear { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.MmDdYyyy); }
}
