using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Timing;

partial struct TimeFormat
{
	/// <summary>
	/// Gets the 24-hour time format
	/// </summary>
	/// <value>
	/// The 24-hour time format
	/// </value>
	public static TimeFormat Hours24 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind._24Hr); }

	/// <summary>
	/// Gets the 12-hour time format
	/// </summary>
	/// <value>
	/// The 12-hour time format
	/// </value>
	public static TimeFormat Hours12 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind._12Hr); }
}
