using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sdl3Sharp.Utilities;

/// <summary>
/// Represents the system's power supply state
/// </summary>
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
[StructLayout(LayoutKind.Sequential)]
public readonly partial struct PowerState : 
	IEquatable<PowerState>, IFormattable, ISpanFormattable, IEqualityOperators<PowerState, PowerState, bool>
{
	private readonly Kind mKind;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly string DebuggerDisplay => ToString();

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private PowerState(Kind kind) => mKind = kind;

	/// <summary>
	/// Gets the current power supply details
	/// </summary>
	/// <param name="secondsLeft">The time, in seconds, of battery life left, or <c>-1</c> if the value couldn't be determined or there's no battery</param>
	/// <param name="percentLeft">The percentage of battery life left (in the range of <c>0</c> through <c>100</c>), or <c>-1</c> if the value couldn't be determined or there's no battery</param>
	/// <returns>
	/// The current battery's <see cref="PowerState"/> including <see cref="Unknown"/> if the value couldn't be determined or <see cref="NoBattery"/> when there's no battery,
	/// or <see cref="Error"/> on failure (check <see cref="Error.TryGet(out string?)"/> for more information)
	/// </returns>
	/// <remarks>
	/// <para>
	/// You should never take a battery status as absolute truth. Batteries (especially failing batteries) are delicate hardware, and the values reported here are best estimates based on what that hardware reports.
	/// It's not uncommon for older batteries to lose stored power much faster than it reports, or completely drain when reporting it has 20 percent left, etc.
	/// </para>
	/// <para>
	/// The battery's status can change at any time; if you are concerned with power state, you should call this method frequently, and perhaps ignore changes until they seem to be stable for a few seconds.
	/// </para>
	/// <para>
	/// It's possible for some platforms to only report the battery life time left (<paramref name="secondsLeft"/>) or the battery percentage left (<paramref name="percentLeft"/>) but not both.
	/// </para>
	/// <para>
	/// On some platforms, retrieving power supply details might be expensive. If you want to display continuous status you could call this method every minute or so.
	/// </para>
	/// </remarks>
	public static PowerState GetInfo(out int secondsLeft, out int percentLeft)
	{
		unsafe
		{
			int localSecondsLeft, localPercentLeft;

			var result = new PowerState(SDL_GetPowerInfo(&localSecondsLeft, &localPercentLeft));

			secondsLeft = localSecondsLeft;
			percentLeft = localPercentLeft;

			return result;
		}
	}

	/// <inheritdoc/>
	public readonly override bool Equals([NotNullWhen(true)] object? obj) => obj is PowerState other && Equals(other);

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly bool Equals(PowerState other) => mKind == other.mKind;

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly override int GetHashCode() => mKind.GetHashCode();

	/// <inheritdoc/>
	public readonly override string ToString() => ToString(format: default);

	/// <inheritdoc cref="IFormattable.ToString(string?, IFormatProvider?)"/>
	public readonly string ToString(string? format) => mKind.ToString(format);

	/// <inheritdoc/>
	readonly string IFormattable.ToString(string? format, IFormatProvider? formatProvider) => ToString(format);	

	/// <inheritdoc cref="ISpanFormattable.TryFormat(Span{char}, out int, ReadOnlySpan{char}, IFormatProvider?)"/>
	public readonly bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format = default) => Enum.TryFormat(mKind, destination, out charsWritten, format);

	/// <inheritdoc/>
	readonly bool ISpanFormattable.TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider) => TryFormat(destination, out charsWritten, format);

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator ==(PowerState left, PowerState right) => left.mKind == right.mKind;

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator !=(PowerState left, PowerState right) => left.mKind != right.mKind;
}
