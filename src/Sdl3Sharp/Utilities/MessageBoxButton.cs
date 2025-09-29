using Sdl3Sharp.Internal;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Runtime.InteropServices;

namespace Sdl3Sharp.Utilities;

[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
[StructLayout(LayoutKind.Sequential)]
[method: SetsRequiredMembers]
public readonly struct MessageBoxButton(int id, string text, MessageBoxButtonFlags flags = default) :
	IEquatable<MessageBoxButton>, IFormattable, ISpanFormattable, IEqualityOperators<MessageBoxButton, MessageBoxButton, bool>
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly string DebuggerDisplay => ToString(formatProvider: CultureInfo.InvariantCulture);

	public readonly MessageBoxButtonFlags Flags { get; init; } = flags;

	public readonly required int Id { get; init; } = id;

	public readonly required string Text { get; init; } = text;

	public void Deconstruct(out int id, out string text, out MessageBoxButtonFlags flags) { flags = Flags; id = Id; text = Text; }

	public readonly override bool Equals([NotNullWhen(true)] object? obj) => obj is MessageBoxButton other && Equals(other);

	public readonly bool Equals(in MessageBoxButton other) => Flags == other.Flags && Id == other.Id && string.Equals(Text, other.Text);

	readonly bool IEquatable<MessageBoxButton>.Equals(MessageBoxButton other) => Equals(other);

	public readonly override int GetHashCode() => HashCode.Combine(Flags, Id, Text);

	public readonly override string ToString() => ToString(format: default, formatProvider: default);

	public readonly string ToString(IFormatProvider? formatProvider) => ToString(format: default, formatProvider);

	public readonly string ToString(string? format) => ToString(format, formatProvider: default);

	public readonly string ToString(string? format, IFormatProvider? formatProvider)
		=> $"{{ {nameof(Flags)}: {Flags}, {nameof(Id)}: {Id.ToString(format, formatProvider)}, {nameof(Text)}: {(Text is not null ? $"\"{Text}\"" : "null")} }}";

	public readonly bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format = default, IFormatProvider? provider = default)
	{
		charsWritten = 0;

		return SpanFormat.TryWrite($"{{ {nameof(Flags)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(Flags, ref destination, ref charsWritten)
			&& SpanFormat.TryWrite($", {nameof(Id)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(Id, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite($", {nameof(Text)}: ", ref destination, ref charsWritten)
			&& (Text is not null
				? SpanFormat.TryWrite('"', ref destination, ref charsWritten) && SpanFormat.TryWrite(Text, ref destination, ref charsWritten) && SpanFormat.TryWrite('"', ref destination, ref charsWritten)
				: SpanFormat.TryWrite("null", ref destination, ref charsWritten))
			&& SpanFormat.TryWrite(" }", ref destination, ref charsWritten);
	}

	public static bool operator ==(in MessageBoxButton left, in MessageBoxButton right) => left.Equals(right);

	static bool IEqualityOperators<MessageBoxButton, MessageBoxButton, bool>.operator ==(MessageBoxButton left, MessageBoxButton right) => left == right;

	public static bool operator !=(in MessageBoxButton left, in MessageBoxButton right) => !(left == right);

	static bool IEqualityOperators<MessageBoxButton, MessageBoxButton, bool>.operator !=(MessageBoxButton left, MessageBoxButton right) => left != right;
}
