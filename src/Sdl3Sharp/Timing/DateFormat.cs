using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sdl3Sharp.Timing;

/// <summary>
/// Represents a date format (the year, the month of the year, and the day of the month)
/// </summary>
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
[StructLayout(LayoutKind.Sequential)]
public readonly partial struct DateFormat :
	IEquatable<DateFormat>, IFormattable, ISpanFormattable, IEqualityOperators<DateFormat, DateFormat, bool>
{
	private readonly Kind mKind;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly string DebuggerDisplay => ToString(formatProvider: CultureInfo.InvariantCulture);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private DateFormat(Kind kind) => mKind = kind;

	/// <summary>
	/// Tries to get the current preferred date format for the system locale
	/// </summary>
	/// <param name="dateFormat">The current preferred date format</param>
	/// <returns><c><see langword="true"/></c>, if the current preferred date format could be successfully retrieved; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This might be a "slow" call that has to query the operating system.
	/// It's best to ask for this once and save the results.
	/// However, the preferred formats can change, usually because the user has changed a system preference outside of your program.
	/// </para>
	/// <para>
	/// To get the current preferred date format as well as the current preferred time format simultaneously, use <see cref="DateTime.TryGetLocalePreferences(out DateFormat, out TimeFormat)"/> instead.
	/// </para>
	/// </remarks>
	public static bool TryGetLocalePreference(out DateFormat dateFormat)
	{
		unsafe
		{
			Unsafe.SkipInit(out DateFormat tmp);

			// we use a local variable here and then copy it back into the given reference,
			// so that we're safe, if the given reference happens to point to the managed heap
			bool result = DateTime.SDL_GetDateTimeLocalePreferences(dateFormat: &tmp, timeFormat: null); /* <- why does the VS IDE report an error here for an ambiguous overload, when the CS compiler does not? */

			dateFormat = tmp;

			return result;
		}
	}

	/// <inheritdoc/>
	public readonly override bool Equals([NotNullWhen(true)] object? obj) => obj is DateFormat other && Equals(other);

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly bool Equals(DateFormat other) => mKind == other.mKind;

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly override int GetHashCode() => mKind.GetHashCode();

	/// <inheritdoc/>
	public readonly override string ToString() => ToString(format: default, formatProvider: default);

	/// <inheritdoc cref="IFormattable.ToString(string?, IFormatProvider?)"/>
	public readonly string ToString(IFormatProvider? formatProvider) => ToString(format: default, formatProvider);

	/// <inheritdoc cref="IFormattable.ToString(string?, IFormatProvider?)"/>
	public readonly string ToString(string? format) => ToString(format, formatProvider: default);

	/// <inheritdoc/>
	public readonly string ToString(string? format, IFormatProvider? formatProvider) => mKind switch
	{
		Kind.YyyyMmDd => "yyyy/MM/dd",
		Kind.DdMmYyyy => "dd/MM/yyyy",
		Kind.MmDdYyyy => "MM/dd/yyyy",
		var kind => unchecked((int)kind).ToString(format, formatProvider),
	};

	/// <inheritdoc/>
	public readonly bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format = default, IFormatProvider? provider = default) => mKind switch
	{
		Kind.YyyyMmDd => "yyyy/MM/dd".TryCopyTo(destination)
			? (charsWritten = "yyyy/MM/dd".Length) is var _
			: !((charsWritten = 0) is var _),
		Kind.DdMmYyyy => "dd/MM/yyyy".TryCopyTo(destination)
			? (charsWritten = "dd/MM/yyyy".Length) is var _
			: !((charsWritten = 0) is var _),
		Kind.MmDdYyyy => "MM/dd/yyyy".TryCopyTo(destination)
			? (charsWritten = "MM/dd/yyyy".Length) is var _
			: !((charsWritten = 0) is var _),
		var kind => unchecked((int)kind).TryFormat(destination, out charsWritten, format, provider),
	};

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator ==(DateFormat left, DateFormat right) => left.mKind == right.mKind;

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator !=(DateFormat left, DateFormat right) => left.mKind != right.mKind;
}
