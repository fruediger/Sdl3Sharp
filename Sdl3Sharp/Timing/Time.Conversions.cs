using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Timing;

partial struct Time
{
	public const long MillisecondsPerSecond = 1_000;

	public const long MicrosecondsPerSecond = 1_000_000;

	public const long NanosecondsPerSecond = 1_000_000_000;

	public const long NanosecondsPerMillisecond = 1_000_000;

	public const long NanosecondsPerMicrosecond = 1_000;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static long SecondsToNanoseconds(long seconds) => seconds * NanosecondsPerSecond;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static long NanosecondsToWholeSeconds(long nanoseconds) => nanoseconds / NanosecondsPerSecond;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static long MillisecondsToNanoseconds(long milliseconds) => milliseconds * NanosecondsPerMillisecond;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static long NanosecondsToWholeMilliseconds(long nanoseconds) => nanoseconds / NanosecondsPerMillisecond;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static long MicrosecondsToNanoseconds(long microseconds) => microseconds * NanosecondsPerMicrosecond;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static long NanosecondsToWholeMicroseconds(long nanoseconds) => nanoseconds / NanosecondsPerMicrosecond;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static (long seconds, int milliseconds, int microseconds, int nanoseconds) NanosecondsToSeconds(long nanoseconds)
	{
		(var seconds, nanoseconds) = long.DivRem(nanoseconds, NanosecondsPerSecond);
		(var milliseconds, nanoseconds) = long.DivRem(nanoseconds, NanosecondsPerMillisecond);
		(var microseconds, nanoseconds) = long.DivRem(nanoseconds, NanosecondsPerMicrosecond);
		return (seconds, unchecked((int)milliseconds), unchecked((int)microseconds), unchecked((int)nanoseconds));
	}
}
