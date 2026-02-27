using Sdl3Sharp.Internal;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sdl3Sharp.Video.Windowing;

/// <summary>
/// Represents a window position, which can be either a definite window coordinate value, a centered position, or an undefined position
/// </summary>
/// <remarks>
/// <para>
/// If the <see cref="WindowPosition"/> represents a centered position or an undefined position, it can be optionally associated with a specific <see cref="IDisplay"/> where the centered or undefined position is applicable.
/// Otherwise, if the <see cref="WindowPosition"/> isn't associated with any specific <see cref="IDisplay"/>, then the centered or undefined position is applicable to the primary display.
/// </para>
/// <para>
/// You can use the <see cref="TryGetValue(out int)"/>, <see cref="TryGetCentered(out IDisplay?)"/>, and <see cref="TryGetUndefined(out IDisplay?)"/> methods to determine whether a <see cref="WindowPosition"/> represents a definite window coordinate value, a centered position, or an undefined position,
/// and to extract the associated value or <see cref="IDisplay"/>, if any, accordingly.
/// </para>
/// </remarks>
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
[StructLayout(LayoutKind.Sequential)]
public readonly struct WindowPosition :
	IEquatable<WindowPosition>, IFormattable, ISpanFormattable, IEqualityOperators<WindowPosition, WindowPosition, bool>
{
	private const int mUndefinedMask = 0x1FFF_0000,
		              mCenteredMask  = 0x2FFF_0000,
		              mKindMask      = unchecked((int)0xFFFF_0000);

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly string DebuggerDisplay => ToString(formatProvider: CultureInfo.InvariantCulture);

	private readonly int mValue;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private static WindowPosition CenteredImpl(uint displayId) => new(unchecked(mCenteredMask | (int)(displayId & ~mKindMask)));

	/// <summary>
	/// Gets a <see cref="WindowPosition"/> representing a centered position on the primary display
	/// </summary>
	/// <value>
	/// A <see cref="WindowPosition"/> representing a centered position on the primary display
	/// </value>
	/// <remarks>
	/// <para>
	/// If you want to specify a centered position on a specific display, you can use the <see cref="CenteredOn(IDisplay)"/> method instead.
	/// </para>
	/// </remarks>
	public static WindowPosition Centered { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => CenteredImpl(0); }

	/// <summary>
	/// Gets a <see cref="WindowPosition"/> representing a centered position on the specified display
	/// </summary>
	/// <param name="display">The display on which to center the window coordinate</param>
	/// <returns>A <see cref="WindowPosition"/> representing a centered position on the specified display</returns>
	/// <remarks>
	/// <para>
	/// If you want to specify a centered position on the primary display, you can use the <see cref="Centered"/> property instead.
	/// </para>
	/// <para>
	/// <see cref="IDisplay"/>s used by this method are technically limited to having an <see cref="IDisplay.Id"/> of <c>65535</c> or less.
	/// However, in practice, this shouldn't be an issue since display ids very rarely exceed this value.
	/// Note that this method doesn't fail if given a display with an id larger than <c>65535</c>, it instead always just takes the lower 16 bits of the display id.
	/// </para>
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static WindowPosition CenteredOn(IDisplay display)
		=> CenteredImpl(display?.Id ?? 0); // technically, display could be null here, but we don't throw, we just treat it as if it were a display with id 0, which is the primary display

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private static WindowPosition UndefinedImpl(uint displayId) => new(unchecked(mUndefinedMask | (int)(displayId & ~mKindMask)));

	/// <summary>
	/// Gets a <see cref="WindowPosition"/> representing an undefined position on the primary display
	/// </summary>
	/// <value>
	/// A <see cref="WindowPosition"/> representing an undefined position on the primary display
	/// </value>
	/// <remarks>
	/// <para>
	/// This can be used to represent a "don't know" or "don't care" position, which is always going to be treated as if it were on the primary display.
	/// If you want to specify an undefined position on a specific display, you can use the <see cref="UndefinedOn(IDisplay)"/> method instead.
	/// </para>
	/// </remarks>
	public static WindowPosition Undefined { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => UndefinedImpl(0); }

	/// <summary>
	/// Gets a <see cref="WindowPosition"/> representing an undefined position on the specified display
	/// </summary>
	/// <param name="display">The display on which to specify the undefined window coordinate</param>
	/// <returns>A <see cref="WindowPosition"/> representing an undefined position on the specified display</returns>
	/// <remarks>
	/// <para>
	/// This can be used to represent a "don't know" or "don't care" position on a specific display, which is always going to be treated as if it were on the specified display.
	/// If you want to specify an undefined position on the primary display, you can use the <see cref="Undefined"/> property instead.
	/// </para>
	/// <para>
	/// <see cref="IDisplay"/>s used by this method are technically limited to having an <see cref="IDisplay.Id"/> of <c>65535</c> or less.
	/// However, in practice, this shouldn't be an issue since display ids very rarely exceed this value.
	/// Note that this method doesn't fail if given a display with an id larger than <c>65535</c>, it instead always just takes the lower 16 bits of the display id.
	/// </para>
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static WindowPosition UndefinedOn(IDisplay display)
		=> UndefinedImpl(display?.Id ?? 0);  // technically, display could be null here, but we don't throw, we just treat it as if it were a display with id 0, which is the primary display

	/// <summary>
	/// Creates a new <see cref="WindowPosition"/> representing the specified definite window coordinate
	/// </summary>
	/// <param name="value">The definite window coordinate value</param>
	/// <remarks>
	/// <para>
	/// This constructor can be used to create a <see cref="WindowPosition"/> with a specific, definite window coordinate for its value.
	/// </para>
	/// <para>
	/// <paramref name="value"/>s passed to specify the window coordinate are technically limited to <em>not</em> be in the range from <c>536805376</c> (<c>0x1FFF0000</c>) to <c>805306367</c> (<c>0x2FFFFFFF</c>) (inclusive).
	/// However, in practice, this shouldn't be an issue since window coordinate values very rarely are within this range.
	/// Note that this constructor doesn't fail if a given value is within this range, it instead just accepts the given value as is.
	/// This means that, given such a value, the resulting <see cref="WindowPosition"/> may represent false or invalid window positions.
	/// </para>
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
#pragma warning disable IDE0290 // No, I'd like to have a dedicated documentation for this constructor and using primary constructor would make separating documentation for the type and for the constructor more difficult
	public WindowPosition(int value) => mValue = value;
#pragma warning restore IDE0290

	/// <inheritdoc/>
	public readonly override bool Equals([NotNullWhen(true)] object? obj) => obj is WindowPosition other && Equals(other);

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly bool Equals(WindowPosition other) => mValue == other.mValue;

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
	public readonly string ToString(string? format, IFormatProvider? formatProvider) => this switch
	{
		_ when TryGetCentered(out var display) => $"Centered{(display is { Id: var id } ? $" on display {id.ToString(format, formatProvider)}" : string.Empty)}",
		_ when TryGetUndefined(out var display) => $"Undefined{(display is { Id: var id } ? $" on display {id.ToString(format, formatProvider)}" : string.Empty)}",
		_ when TryGetValue(out var value) => value.ToString(format, formatProvider),
		_ => string.Empty // This shouldn't really be possible considering that 'mValue' is used exhaustively by the previous cases
	};

	/// <inheritdoc/>
	public readonly bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format = default, IFormatProvider? provider = default)
	{
		charsWritten = 0;

		switch (this)
		{
			case { } when TryGetUndefined(out var display):
				{
					if (!SpanFormat.TryWrite("Undefined", ref destination, ref charsWritten))
					{
						return false;
					}

					if (display is { Id: var displayId })
					{
						return SpanFormat.TryWrite(" on display ", ref destination, ref charsWritten)
							&& SpanFormat.TryWrite(displayId, ref destination, ref charsWritten, format, provider);
					}

					return true;
				}
			
			case { } when TryGetCentered(out var display):
				{
					if (!SpanFormat.TryWrite("Centered", ref destination, ref charsWritten))
					{
						return false;
					}

					if (display is { Id: var displayId })
					{
						return SpanFormat.TryWrite(" on display ", ref destination, ref charsWritten)
							&& SpanFormat.TryWrite(displayId, ref destination, ref charsWritten, format, provider);
					}

					return true;
				}

			case { } when TryGetValue(out var value):
				return SpanFormat.TryWrite(value, ref destination, ref charsWritten, format, provider);
		}

		return false; // This shouldn't really be possible considering that 'mValue' is used exhaustively by the previous switch cases
	}

	/// <summary>
	/// Tries to determine whether this <see cref="WindowPosition"/> represents a centered position and, if so, gets the associated <see cref="IDisplay"/>, if any
	/// </summary>
	/// <param name="display">The associated <see cref="IDisplay"/>, or <c><see langword="null"/></c> if this <see cref="WindowPosition"/> represents a centered position on the primary display</param>
	/// <returns><c><see langword="true"/></c>, if this <see cref="WindowPosition"/> represents a centered position; otherwise, <c><see langword="false"/></c></returns>
	/// <remarks>
	/// <para>
	/// If this method returns <c><see langword="true"/></c> and the value of the <paramref name="display"/> parameter is <c><see langword="null"/></c>, then this <see cref="WindowPosition"/> represents a centered position on the primary display.
	/// The value of the <paramref name="display"/> parameter is always going to be <c><see langword="null"/></c>, if this method returns <c><see langword="false"/></c>.
	/// </para>
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly bool TryGetCentered(out IDisplay? display)
	{
		if ((mValue & mKindMask) is mCenteredMask)
		{
			return IDisplay.TryGetOrCreate(unchecked((uint)(mValue & ~mKindMask)), out display);
		}

		display = null;
		return false;
	}

	/// <summary>
	/// Tries to determine whether this <see cref="WindowPosition"/> represents an undefined position and, if so, gets the associated <see cref="IDisplay"/>, if any
	/// </summary>
	/// <param name="display">The associated <see cref="IDisplay"/>, or <c><see langword="null"/></c> if this <see cref="WindowPosition"/> represents an undefined position on the primary display</param>
	/// <returns><c><see langword="true"/></c>, if this <see cref="WindowPosition"/> represents an undefined position; otherwise, <c><see langword="false"/></c></returns>
	/// <remarks>
	/// <para>
	/// If this method returns <c><see langword="true"/></c> and the value of the <paramref name="display"/> parameter is <c><see langword="null"/></c>, then this <see cref="WindowPosition"/> represents an undefined position on the primary display.
	/// The value of the <paramref name="display"/> parameter is always going to be <c><see langword="null"/></c>, if this method returns <c><see langword="false"/></c>.
	/// </para>
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly bool TryGetUndefined(out IDisplay? display)
	{
		if ((mValue & mKindMask) is mUndefinedMask)
		{
			return IDisplay.TryGetOrCreate(unchecked((uint)(mValue & ~mKindMask)), out display);
		}

		display = null;
		return false;
	}

	/// <summary>
	/// Tries to determine whether this <see cref="WindowPosition"/> represents a definite window coordinate and, if so, gets the associated value
	/// </summary>
	/// <param name="value">The definite window coordinate value</param>
	/// <returns><c><see langword="true"/></c>, if this <see cref="WindowPosition"/> represents a definite window coordinate; otherwise, <c><see langword="false"/></c></returns>
	/// <remarks>
	/// <para>
	/// This method will return <c><see langword="true"/></c> if and only if <see cref="TryGetCentered(out IDisplay?)"/> and <see cref="TryGetUndefined(out IDisplay?)"/> would both return <c><see langword="false"/></c>.
	/// </para>
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly bool TryGetValue(out int value)
	{
		if ((mValue & mKindMask) is not (mUndefinedMask or mCenteredMask))
		{
			value = mValue;
			return true;
		}

		value = default;
		return false;
	}

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator ==(WindowPosition left, WindowPosition right) => left.mValue == right.mValue;


	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator !=(WindowPosition left, WindowPosition right) => left.mValue != right.mValue;

	/// <summary>
	/// Converts a definite window coordinate value into a <see cref="WindowPosition"/> representing that value
	/// </summary>
	/// <param name="value">The definite window coordinate value to convert</param>
	/// <remarks>
	/// <para>
	/// <paramref name="value"/>s passed to specify the window coordinate are technically limited to <em>not</em> be in the range from <c>536805376</c> to <c>805306367</c> (inclusive).
	/// However, in practice, this shouldn't be an issue since window coordinate values very rarely are within this range.
	/// Note that this operator doesn't fail if a given value is within this range, it instead just accepts the given value as is.
	/// This means that, given such a value, the resulting <see cref="WindowPosition"/> may represent false or invalid window positions.
	/// </para>
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static implicit operator WindowPosition(int value) => new(value);

	/// <summary>
	/// Converts this <see cref="WindowPosition"/> into a definite window coordinate value
	/// </summary>
	/// <param name="position">The <see cref="WindowPosition"/> to convert</param>
	/// <remarks>
	/// <para>
	/// Note that this operator will <see langword="throw"/> an <see cref="InvalidOperationException"/> if the <see cref="WindowPosition"/> does not represent a definite window coordinate value.
	/// </para>
	/// </remarks>
	/// <exception cref="InvalidOperationException">The <see cref="WindowPosition"/> does not represent a definite window coordinate value</exception>
	public static explicit operator int(WindowPosition position)
	{
		if (!position.TryGetValue(out var value))
		{
			failNotAValue();
		}

		return value;

		[DoesNotReturn]
		static void failNotAValue() => throw new InvalidOperationException($"The {nameof(WindowPosition)} does not represent a definite value");
	}
}
