using System;
using System.Buffers;
using System.Runtime.CompilerServices;
using System.Text.Unicode;

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

	public static bool TryWriteUtf8(ReadOnlySpan<byte> source, ref Span<char> destination, ref int charsWritten, bool replaceInvalidSequences = true, bool isFinalBlock = true)
	{
		var result = Utf8.ToUtf16(source, destination, out var bytesRead, out var tmpCharsWritten, replaceInvalidSequences, isFinalBlock) is OperationStatus.Done;

		if (result)
		{
			destination = destination[tmpCharsWritten..];
			charsWritten += tmpCharsWritten;
		}

		return result;
	}
}
