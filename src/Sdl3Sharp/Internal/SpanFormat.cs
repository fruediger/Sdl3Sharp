using System;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Internal;

internal static class SpanFormat
{
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool TryWrite(ReadOnlySpan<char> value, ref Span<char> destination, ref int charsWritten)
	{
		var result = value.TryCopyTo(destination);

		if (result)
		{
			destination = destination[value.Length..];
			charsWritten += value.Length;
		}

		return result;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool TryWrite(bool value, ref Span<char> destination, ref int charsWritten)
	{
		var result = value.TryFormat(destination, out var tmpCharsWritten);

		if (result)
		{
			destination = destination[tmpCharsWritten..];
			charsWritten += tmpCharsWritten;
		}

		return result;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool TryWrite<T>(in T value, ref Span<char> destination, ref int charsWritten, ReadOnlySpan<char> format = default, IFormatProvider? provider = default)
		where T : ISpanFormattable, allows ref struct
	{
		var result = value.TryFormat(destination, out var tmpCharsWritten, format, provider);

		if (result)
		{
			destination = destination[tmpCharsWritten..];
			charsWritten += tmpCharsWritten;
		}

		return result;
	}
}
