using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace Sdl3Sharp;

/// <summary>
/// Represents a log priority
/// </summary>
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
[StructLayout(LayoutKind.Sequential)]
public readonly partial struct LogPriority :
	IComparable, IComparable<LogPriority>, IEquatable<LogPriority>, IFormattable, ISpanFormattable, IComparisonOperators<LogPriority, LogPriority, bool>, IEqualityOperators<LogPriority, LogPriority, bool>
{
	private readonly Kind mKind;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly string DebuggerDisplay => ToString();

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private LogPriority(Kind kind) => mKind = kind;

	/// <inheritdoc/>
	/// <exception cref="ArgumentException"><c><paramref name="obj"/></c> is not of type <see cref="LogPriority"/></exception>
	public readonly int CompareTo(object? obj)
	{
		return obj switch
		{
			null => 1,
			LogPriority other => CompareTo(other),
			_ => failObjArgumentIsNotLogPriority()
		};

		[DoesNotReturn]
		static int failObjArgumentIsNotLogPriority() => throw new ArgumentException(message: $"{nameof(obj)} is not of type {nameof(LogPriority)}", paramName: nameof(obj));
	}

	/// <inheritdoc/>
	public readonly int CompareTo(LogPriority other) => unchecked((int)mKind).CompareTo(unchecked((int)other.mKind));

	/// <inheritdoc/>
	public readonly override bool Equals([NotNullWhen(true)] object? obj) => obj is LogPriority other && Equals(other);

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly bool Equals(LogPriority other) => mKind == other.mKind;

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

	/// <summary>
	/// Tries to set the text prepended to logging messages of a this log priority
	/// </summary>
	/// <param name="prefix">The prefix to use for this log priority, or <c><see langword="null"/></c> to use no prefix</param>
	/// <returns><c><see langword="true"/></c> if the prefix was successfully set for this log priority; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	public readonly bool TrySetPrefix(string? prefix)
	{
		unsafe
		{
			var utf8Prefix = Utf8StringMarshaller.ConvertToUnmanaged(prefix);

			try
			{
				return SDL_SetLogPriorityPrefix(this, utf8Prefix);
			}
			finally
			{
				Utf8StringMarshaller.Free(utf8Prefix);
			}
		}
	}

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator >(LogPriority left, LogPriority right) => left.mKind > right.mKind;

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator >=(LogPriority left, LogPriority right) => left.mKind >= right.mKind;

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator <(LogPriority left, LogPriority right) => left.mKind < right.mKind;

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator <=(LogPriority left, LogPriority right) => left.mKind <= right.mKind;

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator ==(LogPriority left, LogPriority right) => left.mKind == right.mKind;

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator !=(LogPriority left, LogPriority right) => left.mKind != right.mKind;
}
