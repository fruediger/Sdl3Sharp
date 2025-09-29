using Sdl3Sharp.Internal;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sdl3Sharp;

/// <summary>
/// Represents a log category
/// </summary>
/// <seealso cref="Log"/>
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
[StructLayout(LayoutKind.Sequential)]
public readonly partial struct LogCategory :
	IEquatable<LogCategory>, IFormattable, ISpanFormattable, IEqualityOperators<LogCategory, LogCategory, bool>
{
	private readonly Kind mKind;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly string DebuggerDisplay => ToString(formatProvider: CultureInfo.InvariantCulture);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private LogCategory(Kind kind) => mKind = kind;

	/// <summary>
	/// Resets the <see cref="Priority">priorities</see> of all <see cref="LogCategory">log categories</see> to their default value
	/// </summary>
	/// <remarks>
	/// This is called by <see cref="Sdl.Dispose"/>
	/// </remarks>
	public static void ResetPriorityForAll() => SDL_ResetLogPriorities();

	/// <summary>
	/// Sets the <see cref="Priority">priorities</see> of all <see cref="LogCategory">log categories</see> to a specific value
	/// </summary>
	/// <param name="priority">The priority to assign</param>
	public static void SetPriorityForAll(LogPriority priority) => SDL_SetLogPriorities(priority);

	/// <summary>
	/// Gets or sets the priority of this log category
	/// </summary>
	/// <value>
	/// The priority of this log category
	/// </value>
	public readonly LogPriority Priority
	{
		get => SDL_GetLogPriority(this);
		set => SDL_SetLogPriority(this, value);
	}

	/// <inheritdoc/>
	public readonly override bool Equals([NotNullWhen(true)] object? obj) => obj is LogCategory other && Equals(other);

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly bool Equals(LogCategory other) => mKind == other.mKind;

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly override int GetHashCode() => mKind.GetHashCode();

	/// <summary>
	/// Logs a <paramref name="message"/> with this log category and <see cref="LogPriority.Critical"/>
	/// </summary>
	/// <param name="message">The message to be logged</param>
	public readonly void LogCritical(string message) => Log.Critical(this, message);

	/// <summary>
	/// Logs a <paramref name="message"/> with this log category and <see cref="LogPriority.Debug"/>
	/// </summary>
	/// <param name="message">The message to be logged</param>
	public readonly void LogDebug(string message) => Log.Debug(this, message);

	/// <summary>
	/// Logs a <paramref name="message"/> with this log category and <see cref="LogPriority.Error"/>
	/// </summary>
	/// <param name="message">The message to be logged</param>
	public readonly void LogError(string message) => Log.Error(this, message);

	/// <summary>
	/// Logs a <paramref name="message"/> with this log category and <see cref="LogPriority.Info"/>
	/// </summary>
	/// <param name="message">The message to be logged</param>
	public readonly void LogInfo(string message) => Log.Info(this, message);

	/// <summary>
	/// Logs a <paramref name="message"/> with this log category and a specific <paramref name="priority"/>
	/// </summary> 
	/// <param name="priority">The priority of the message</param>
	/// <param name="message">The message to be logged</param>
	public readonly void LogMessage(LogPriority priority, string message) => Log.Message(this, priority, message);

	/// <summary>
	/// Logs a <paramref name="message"/> with this log category and <see cref="LogPriority.Trace"/>
	/// </summary>
	/// <param name="message">The message to be logged</param>
	public readonly void LogTrace(string message) => Log.Trace(this, message);

	/// <summary>
	/// Logs a <paramref name="message"/> with this log category and <see cref="LogPriority.Verbose"/>
	/// </summary>
	/// <param name="message">The message to be logged</param>
	public readonly void LogVerbose(string message) => Log.Verbose(this, message);

	/// <summary>
	/// Logs a <paramref name="message"/> with this log category and <see cref="LogPriority.Warn"/>
	/// </summary>
	/// <param name="message">The message to be logged</param>
	public readonly void LogWarn(string message) => Log.Warn(this, message);

	/// <inheritdoc/>
	public readonly override string ToString() => ToString(format: default, formatProvider: default);

	/// <inheritdoc cref="ToString(string?, IFormatProvider?)"/>
	public readonly string ToString(IFormatProvider? formatProvider) => ToString(format: default, formatProvider);

	/// <inheritdoc cref="ToString(string?, IFormatProvider?)"/>
	public readonly string ToString(string? format) => ToString(format, formatProvider: default);

	/// <inheritdoc/>
	public readonly string ToString(string? format, IFormatProvider? formatProvider) => mKind switch
	{
		>= Kind.Custom => $"{nameof(Custom)}({unchecked(mKind - Kind.Custom).ToString(format, formatProvider)})",
		_ => mKind.ToString(format)
	};

	/// <inheritdoc/>
	public readonly bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format = default, IFormatProvider? provider = default)
	{
		charsWritten = 0;

		return mKind switch
		{
			>= Kind.Custom
				=> SpanFormat.TryWrite($"{nameof(Custom)}(", ref destination, ref charsWritten)
				&& SpanFormat.TryWrite(unchecked(mKind - Kind.Custom), ref destination, ref charsWritten, format, provider)
				&& SpanFormat.TryWrite(')', ref destination, ref charsWritten),

			_ => SpanFormat.TryWrite(mKind, ref destination, ref charsWritten, format)
		};
	}

	/// <summary>
	/// Tries to get the custom value used to identify this custom log category
	/// </summary>
	/// <param name="customValue">The custom value used to identify this custom log category</param>
	/// <returns><c><see langword="true"/></c> if this instance represents a custom log category and <paramref name="customValue"/> is the custom value which identifies it; otherwise, <c><see langword="false"/></c></returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly bool TryGetCustomValue(out int customValue)
	{
		if (mKind >= Kind.Custom)
		{
			customValue = unchecked(mKind - Kind.Custom);

			return true;
		}

		customValue = default;

		return false;
	}

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator ==(LogCategory left, LogCategory right) => left.mKind == right.mKind;

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator !=(LogCategory left, LogCategory right) => left.mKind != right.mKind;
}
