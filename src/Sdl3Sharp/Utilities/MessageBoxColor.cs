using Sdl3Sharp.Internal;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sdl3Sharp.Utilities;

[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
[StructLayout(LayoutKind.Sequential, Pack = sizeof(byte))]
[method: MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
public readonly struct MessageBoxColor(byte r, byte g, byte b) : IEquatable<MessageBoxColor>, IFormattable, ISpanFormattable, IEqualityOperators<MessageBoxColor, MessageBoxColor, bool>
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly string DebuggerDisplay => ToString(formatProvider: CultureInfo.InvariantCulture);

	public readonly byte R
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] init;
	} = r;

	public readonly byte G
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] init;
	} = g;

	public readonly byte B
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] init;
	} = b;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public void Deconstruct(out byte r, out byte g, out byte b) { r = R; g = G; b = B; }

	public readonly override bool Equals([NotNullWhen(true)] object? obj) => obj is MessageBoxColor other && Equals(other);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly bool Equals(MessageBoxColor other) => R == other.R && G == other.G && B == other.B;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly override int GetHashCode() => HashCode.Combine(R, G, B);

	public readonly override string ToString() => ToString(format: default, formatProvider: default);

	public readonly string ToString(IFormatProvider? formatProvider) => ToString(format: default, formatProvider);

	public readonly string ToString(string? format) => ToString(format, formatProvider: default);

	public readonly string ToString(string? format, IFormatProvider? formatProvider)
		=> $"{{ {nameof(R)}: {R.ToString(format, formatProvider)}, {nameof(G)}: {G.ToString(format, formatProvider)}, {nameof(B)}: {B.ToString(format, formatProvider)} }}";

	public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format = default, IFormatProvider? provider = default)
	{
		charsWritten = 0;

		return SpanFormat.TryWrite($"{{ {nameof(R)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(R, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite($", {nameof(G)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(G, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite($", {nameof(B)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(B, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite(" }", ref destination, ref charsWritten);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator ==(MessageBoxColor left, MessageBoxColor right) => left.Equals(right);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator !=(MessageBoxColor left, MessageBoxColor right) => !(left == right);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static implicit operator MessageBoxColor((byte r, byte g, byte b) color) => new(color.r, color.g, color.b);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static implicit operator (byte r, byte g, byte b)(MessageBoxColor color) => (color.R, color.G, color.B);
}
