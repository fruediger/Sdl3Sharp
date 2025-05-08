using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sdl3Sharp.Interop;

[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
[StructLayout(LayoutKind.Sequential)]
[method: MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
internal readonly struct CBool(bool value) :
	IComparable, IComparable<bool>, IComparable<CBool>, IEquatable<bool>, IEquatable<CBool>, IFormattable, ISpanFormattable, IParsable<CBool>, ISpanParsable<CBool>, IEqualityOperators<CBool, CBool, bool>
{
	private readonly byte mValue = unchecked(Unsafe.BitCast<bool, byte>(value));

	public readonly bool AsClrBool { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => Unsafe.BitCast<byte, bool>(mValue); }

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly string DebuggerDisplay => ToString(CultureInfo.InvariantCulture);

	public readonly int CompareTo(object? obj)
	{
		return obj switch
		{
			null => 1,
			bool other => CompareTo(other),
			CBool other => CompareTo(other),
			_ => failObjArgumentIsNeitherBoolNorCBool()
		};

		[DoesNotReturn]
		static int failObjArgumentIsNeitherBoolNorCBool() => throw new ArgumentException(message: $"{nameof(obj)} is neither of type {nameof(Boolean)} nor of type {nameof(CBool)}", paramName: nameof(obj));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly int CompareTo(bool other) => AsClrBool.CompareTo(other);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly int CompareTo(CBool other) => AsClrBool.CompareTo(other.AsClrBool);

	public readonly override bool Equals([NotNullWhen(true)] object? obj) => obj switch
	{
		bool other => Equals(other),
		CBool other => Equals(other),
		_ => false
	};

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly override int GetHashCode() => AsClrBool.GetHashCode();

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly bool Equals(bool other) => AsClrBool.Equals(other);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly bool Equals(CBool other) => AsClrBool.Equals(other.AsClrBool);

	public readonly override string ToString() => AsClrBool.ToString();

	public readonly string ToString(IFormatProvider? provider) => AsClrBool.ToString(provider);

	readonly string IFormattable.ToString(string? format, IFormatProvider? formatProvider) => ToString(provider: formatProvider);

	public readonly bool TryFormat(Span<char> destination, out int charsWritten) => AsClrBool.TryFormat(destination, out charsWritten);

	readonly bool ISpanFormattable.TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider) => TryFormat(destination, out charsWritten);

	public static CBool Parse(string value) => new(bool.Parse(value));

	static CBool IParsable<CBool>.Parse(string s, IFormatProvider? provider) => Parse(value: s);

	public static CBool Parse(ReadOnlySpan<char> value) => new(bool.Parse(value));

	static CBool ISpanParsable<CBool>.Parse(ReadOnlySpan<char> s, IFormatProvider? provider) => Parse(value: s);

	public static bool TryParse(string? value, out CBool result)
	{
		var @return = bool.TryParse(value, out var boolResult);
		result = new(boolResult);
		return @return;
	}

	static bool IParsable<CBool>.TryParse(string? s, IFormatProvider? provider, out CBool result) => TryParse(value: s, out result);

	public static bool TryParse(ReadOnlySpan<char> value, out CBool result)
	{
		var @return = bool.TryParse(value, out var boolResult);
		result = new(boolResult);
		return @return;
	}

	static bool ISpanParsable<CBool>.TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out CBool result) => TryParse(value: s, out result);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator false(CBool value) => !value.AsClrBool;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator true(CBool cBool) => cBool.AsClrBool;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static CBool operator !(CBool value) => new(!value.AsClrBool);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static CBool operator &(CBool left, CBool right) => new(left.AsClrBool & right.AsClrBool);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static CBool operator |(CBool left, CBool right) => new(left.AsClrBool | right.AsClrBool);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static CBool operator ^(CBool left, CBool right) => new(left.AsClrBool ^ right.AsClrBool);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator ==(CBool left, CBool right) => left.AsClrBool == right.AsClrBool;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator !=(CBool left, CBool right) => left.AsClrBool != right.AsClrBool;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static implicit operator CBool(bool value) => new(value);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static implicit operator bool(CBool value) => value.AsClrBool;
}
