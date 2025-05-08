using Sdl3Sharp.Interop;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sdl3Sharp;

/// <summary>
/// Represents a sub system which can be initialized by SDL and used in your application
/// </summary>
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
[StructLayout(LayoutKind.Sequential)]
public readonly partial struct SubSystem
	: IEquatable<SubSystem>, IFormattable, ISpanFormattable, IEqualityOperators<SubSystem, SubSystem, bool>
{
	internal readonly InitFlags InitFlag;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private string DebuggerDisplay => ToString(formatProvider: CultureInfo.InvariantCulture);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	internal SubSystem(InitFlags initFlag) => InitFlag = initFlag;

	/// <summary>
	/// Gets a value indicating whether the <see cref="SubSystem"/> is currently initialized
	/// </summary>
	/// <value>
	/// A value indicating whether the <see cref="SubSystem"/> is currently initialized
	/// </value>
	/// <remarks>
	/// If you want to check the initialization state of multiple <see cref="SubSystem"/> at once, you can use <see cref="Sdl.GetInitializedSubSystems(SubSystemSet)"/> instead
	/// </remarks>
	public readonly bool IsInitialized => Sdl.SDL_WasInit(InitFlag) == InitFlag;

	/// <inheritdoc/>
	public readonly override bool Equals([NotNullWhen(true)] object? obj) => obj is SubSystem other && Equals(other);

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly bool Equals(SubSystem other) => InitFlag == other.InitFlag;

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly override int GetHashCode() => InitFlag.GetHashCode();

	/// <inheritdoc/>
	public readonly override string ToString() => ToString(format: default, formatProvider: default);

	/// <inheritdoc cref="IFormattable.ToString(string?, IFormatProvider?)"/>
	public readonly string ToString(IFormatProvider? formatProvider) => ToString(format: default, formatProvider);

	/// <inheritdoc cref="IFormattable.ToString(string?, IFormatProvider?)"/>
	public readonly string ToString(string? format) => ToString(format, formatProvider: default);

	/// <inheritdoc/>
	public readonly string ToString(string? format, IFormatProvider? formatProvider) => KnownInitFlagToString(InitFlag) switch
	{
		string knownInitFlag => knownInitFlag,
		_ => unchecked((uint)InitFlag).ToString(format, formatProvider)
	};

	/// <inheritdoc/>
	public readonly bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format = default, IFormatProvider? provider = default)
	{
		static bool tryWriteSpan(ReadOnlySpan<char> value, ref Span<char> destination, ref int charsWritten)
		{
			var result = value.TryCopyTo(destination);

			if (result)
			{
				destination = destination[value.Length..];
				charsWritten += value.Length;
			}

			return result;
		}

		static bool tryWriteUInt(uint value, ref Span<char> destination, ref int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
		{
			var result = value.TryFormat(destination, out var tmp, format, provider);

			if (result)
			{
				destination = destination[tmp..];
				charsWritten += tmp;
			}

			return result;
		}

		charsWritten = 0;

		return KnownInitFlagToString(InitFlag) switch
		{
			string knownInitFlag => tryWriteSpan(knownInitFlag, ref destination, ref charsWritten),
			_ => tryWriteUInt(unchecked((uint)InitFlag), ref destination, ref charsWritten, format, provider)
		};
	}

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator ==(SubSystem left, SubSystem right) => left.InitFlag == right.InitFlag;

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator !=(SubSystem left, SubSystem right) => left.InitFlag != right.InitFlag;
}
