using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sdl3Sharp;

/// <summary>
/// Represents a version consisting of a <see cref="Major">major</see>-, <see cref="Minor">minor</see>-, and a <see cref="Micro">micro</see>-component
/// </summary>
/// <param name="major">The major version component of the version</param>
/// <param name="minor">The minor version component of the version</param>
/// <param name="micro">The micro version component of the version</param>
/// <inheritdoc cref="ValidateAndCombineComponents(int, int, int)"/>
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
[StructLayout(LayoutKind.Sequential)]
[method: MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
public readonly struct Version(int major, int minor, int micro) :
	IComparable, IComparable<Version>, IEquatable<Version>, IFormattable, ISpanFormattable, IComparisonOperators<Version, Version, bool>, IEqualityOperators<Version, Version, bool>
{
	/// <exception cref="ArgumentOutOfRangeException">
	/// <c><paramref name="major"/></c> is less than <c>0</c> or not less than <c>1000</c>
	/// - or -
	/// <c><paramref name="minor"/></c> is less than <c>0</c> or not less than <c>1000</c>
	/// - or -
	/// <c><paramref name="micro"/></c> is less than <c>0</c> or not less than <c>1000</c>
	/// </exception>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private static int ValidateAndCombineComponents(int major, int minor, int micro)
	{
		if (major is < 0 or >= 1_000)
		{
			failMajorArgumentOutOfRange();
		}

		if (minor is < 0 or >= 1_000)
		{
			failMinorArgumentOutOfRange();
		}

		if (micro is < 0 or >= 1_000)
		{
			failMicroArgumentOutOfRange();
		}

		return major * 1_000_000 + minor * 1_000 + micro;

		[DoesNotReturn]
		static void failMajorArgumentOutOfRange() => throw new ArgumentOutOfRangeException(nameof(major));

		[DoesNotReturn]
		static void failMinorArgumentOutOfRange() => throw new ArgumentOutOfRangeException(nameof(minor));

		[DoesNotReturn]
		static void failMicroArgumentOutOfRange() => throw new ArgumentOutOfRangeException(nameof(micro));
	}

	private readonly int mValue = ValidateAndCombineComponents(major, minor, micro);

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly string DebuggerDisplay => ToString(formatProvider: CultureInfo.InvariantCulture);

	/// <summary>
	/// Gets the major version component of the version
	/// </summary>
	/// <value>
	/// The major version component of the version
	/// </value>
	public readonly int Major { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => unchecked(mValue / 1_000_000); }

	/// <summary>
	/// Gets the minor version component of the version
	/// </summary>
	/// <value>
	/// The minor version component of the version
	/// </value>
	public readonly int Minor { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => unchecked(mValue / 1_000 % 1_000); }

	/// <summary>
	/// Gets the micro version component of the version
	/// </summary>
	/// <value>
	/// The micro version component of the version
	/// </value>
	public readonly int Micro { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => unchecked(mValue % 1_000); }

	/// <inheritdoc/>
	public readonly int CompareTo(object? obj)
	{
		return obj switch
		{
			null => 1,
			Version other => CompareTo(other),
			_ => failObjArgumentIsNotVersion()
		};

		[DoesNotReturn]
		static int failObjArgumentIsNotVersion() => throw new ArgumentException(message: $"{nameof(obj)} is not of type {nameof(Version)}", paramName: nameof(obj));
	}

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly int CompareTo(Version other) => mValue.CompareTo(other.mValue);

	/// <summary>
	/// Deconstructs the version into its components
	/// </summary>
	/// <param name="major">The major version component of the version</param>
	/// <param name="minor">The minor version component of the version</param>
	/// <param name="micro">The micro version component of the version</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly void Deconstruct(out int major, out int minor, out int micro) { major = Major; minor = Minor; micro = Micro; }

	/// <inheritdoc/>
	public readonly override bool Equals([NotNullWhen(true)] object? obj) => obj is Version other && Equals(other);

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly bool Equals(Version other) => mValue == other.mValue;

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly override int GetHashCode() => mValue.GetHashCode();

	/// <inheritdoc/>
	public readonly override string ToString() => ToString(format: default, formatProvider: default);

	/// <inheritdoc cref="IFormattable.ToString(string?, IFormatProvider?)"/>
	public readonly string ToString(IFormatProvider? formatProvider) => ToString(format: default, formatProvider);

	/// <inheritdoc cref="IFormattable.ToString(string?, IFormatProvider?)"/>
	public readonly string ToString(string? format) => ToString(format, formatProvider: default);

	/// <inheritdoc/>
	public readonly string ToString(string? format, IFormatProvider? formatProvider)
		=> $"{Major.ToString(format, formatProvider)}.{Minor.ToString(format, formatProvider)}{Micro switch { var micro when micro is not 0 => $".{micro.ToString(format, formatProvider)}", _ => string.Empty }}";

	/// <inheritdoc/>
	public readonly bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format = default, IFormatProvider? provider = default)
	{
		static bool tryWriteInt(int value, ref Span<char> destination, ref int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
		{
			var result = value.TryFormat(destination, out var tmp, format, provider);

			if (result)
			{
				destination = destination[tmp..];
				charsWritten += tmp;
			}

			return result;
		}

		static bool tryWriteChar(char value, ref Span<char> destination, ref int charsWritten)
		{
			if (destination.Length is > 0)
			{
				destination[0] = value;
				charsWritten += 1;

				return true;
			}

			return false;
		}

		charsWritten = 0;

		return tryWriteInt(Major, ref destination, ref charsWritten, format, provider)
			&& tryWriteChar('.', ref destination, ref charsWritten)
			&& tryWriteInt(Minor, ref destination, ref charsWritten, format, provider)
			&& Micro switch
			{
				var micro when micro is not 0
					=> tryWriteChar('.', ref destination, ref charsWritten)
					&& tryWriteInt(micro, ref destination, ref charsWritten, format, provider),

				_ => true
			};
	}

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator >(Version left, Version right) => left.mValue > right.mValue;

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator >=(Version left, Version right) => left.mValue >= right.mValue;

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator <(Version left, Version right) => left.mValue < right.mValue;

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator <=(Version left, Version right) => left.mValue <= right.mValue;

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator ==(Version left, Version right) => left.mValue == right.mValue;

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator !=(Version left, Version right) => left.mValue != right.mValue;

	/// <summary>
	/// Converts a <see cref="System.Version"/> into a <see cref="Version"/>
	/// </summary>
	/// <param name="value">The <see cref="System.Version"/> to convert</param>
	/// <returns>The converted <see cref="Version"/></returns>
	/// <remarks>
	/// If the <see cref="System.Version.Build"/> component of the given <paramref name="value"/> is undefined (<c>-1</c>), the resulting <see cref="Version"/> will have its <see cref="Micro"/> component set to <c>0</c>
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static explicit operator Version(System.Version value) => new(value.Major, value.Minor, value.Build switch { < 0 => 0, var build => build });

	/// <summary>
	/// Converts a <see cref="Version"/> into a <see cref="System.Version"/>
	/// </summary>
	/// <param name="value">The <see cref="Version"/> to convert</param>
	/// <returns>The converted <see cref="System.Version"/></returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static explicit operator System.Version(Version value) => new(value.Major, value.Minor, value.Micro);
}
