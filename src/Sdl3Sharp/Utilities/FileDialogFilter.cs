using Sdl3Sharp.Internal;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Utilities;

[method: SetsRequiredMembers]
public readonly struct FileDialogFilter(string name, string pattern) :
	IEquatable<FileDialogFilter>, IFormattable, ISpanFormattable, IEqualityOperators<FileDialogFilter, FileDialogFilter, bool>
{
	public readonly required string Name
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]init;
	} = name;

	public readonly required string Pattern
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get;
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] init;
	} = pattern;

	public void Deconstruct(out string name, out string pattern) { name = Name; pattern = Pattern; }

	public readonly override bool Equals([NotNullWhen(true)] object? obj) => obj is FileDialogFilter other && Equals(other);

	public readonly bool Equals(in FileDialogFilter other) => string.Equals(Name, other.Name) && string.Equals(Pattern, other.Pattern);

	readonly bool IEquatable<FileDialogFilter>.Equals(FileDialogFilter other) => Equals(other);

	public readonly override int GetHashCode() => HashCode.Combine(Name, Pattern);

	public readonly override string ToString()
		=> $"{{ {nameof(Name)}: {(Name is not null ? $"\"{Name}\"" : "null")}, {nameof(Pattern)}: {(Pattern is not null ? $"\"{Pattern}\"" : "null")} }}";

	readonly string IFormattable.ToString(string? format, IFormatProvider? formatProvider) => ToString();

	public readonly bool TryFormat(Span<char> destination, out int charsWritten)
	{
		charsWritten = 0;

		return SpanFormat.TryWrite($"{{ {nameof(Name)}: ", ref destination, ref charsWritten)
			&& (Name is not null
				? SpanFormat.TryWrite('"', ref destination, ref charsWritten) && SpanFormat.TryWrite(Name, ref destination, ref charsWritten) && SpanFormat.TryWrite('"', ref destination, ref charsWritten)
				: SpanFormat.TryWrite("null", ref destination, ref charsWritten))
			&& SpanFormat.TryWrite($", {nameof(Pattern)}: ", ref destination, ref charsWritten)
			&& (Pattern is not null
				? SpanFormat.TryWrite('"', ref destination, ref charsWritten) && SpanFormat.TryWrite(Pattern, ref destination, ref charsWritten) && SpanFormat.TryWrite('"', ref destination, ref charsWritten)
				: SpanFormat.TryWrite("null", ref destination, ref charsWritten))
			&& SpanFormat.TryWrite(" }", ref destination, ref charsWritten);
	}

	readonly bool ISpanFormattable.TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
		=> TryFormat(destination, out charsWritten);

	public static bool operator ==(in FileDialogFilter left, in FileDialogFilter right) => left.Equals(right);

	static bool IEqualityOperators<FileDialogFilter, FileDialogFilter, bool>.operator ==(FileDialogFilter left, FileDialogFilter right) => left == right;

	public static bool operator !=(in FileDialogFilter left, in FileDialogFilter right) => !(left == right);

	static bool IEqualityOperators<FileDialogFilter, FileDialogFilter, bool>.operator !=(FileDialogFilter left, FileDialogFilter right) => left != right;
}
