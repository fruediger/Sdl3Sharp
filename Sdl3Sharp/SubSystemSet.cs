using Sdl3Sharp.Internal;
using Sdl3Sharp.Interop;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sdl3Sharp;

/// <summary>
/// Represents an immutable set of <see cref="SubSystem"/>s
/// </summary>
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
[CollectionBuilder(typeof(SubSystemSet), nameof(Create))]
[StructLayout(LayoutKind.Sequential)]
public readonly partial struct SubSystemSet :
	IEquatable<SubSystemSet>, IFormattable, ISpanFormattable, IReadOnlySet<SubSystem>, IBitwiseOperators<SubSystemSet, SubSystemSet, SubSystemSet>, IEqualityOperators<SubSystemSet, SubSystemSet, bool>
{
	internal readonly InitFlags InitFlags;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private string DebuggerDisplay => ToString(formatProvider: CultureInfo.InvariantCulture);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	internal SubSystemSet(InitFlags initFlags) => InitFlags = initFlags;

	/// <summary>
	/// Creates an <see cref="SubSystemSet"/> that contains the specified <paramref name="subSystems"/> items
	/// </summary>
	/// <param name="subSystems">The <see cref="SubSystem"/> items that resulting <see cref="SubSystemSet"/> should contain</param>
	/// <returns>An <see cref="SubSystemSet"/> that contains the specified <paramref name="subSystems"/> items</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static SubSystemSet Create(params ReadOnlySpan<SubSystem> subSystems)
	{
		var initFlags = default(InitFlags);

		foreach (var subSystem in subSystems)
		{
			initFlags |= subSystem.InitFlag;
		}

		return new(initFlags);
	}

	/// <summary>
	/// Creates an <see cref="SubSystemSet"/> that contains the specified <paramref name="subSystems"/> items
	/// </summary>
	/// <param name="subSystems">The source of the <see cref="SubSystem"/> items that resulting <see cref="SubSystemSet"/> should contain</param>
	/// <returns>An <see cref="SubSystemSet"/> that contains the specified <paramref name="subSystems"/> items</returns>
	public static SubSystemSet Create(IEnumerable<SubSystem> subSystems)
	{
		var initFlags = default(InitFlags);

		foreach (var subSystem in subSystems)
		{
			initFlags |= subSystem.InitFlag;
		}

		return new(initFlags);
	}

	/// <summary>
	/// Gets an empty <see cref="SubSystemSet"/>
	/// </summary>
	/// <value>
	/// An empty <see cref="SubSystemSet"/>
	/// </value>
	public static SubSystemSet Empty { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(0); }

	/// <inheritdoc/>
	public readonly int Count { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => unchecked((int)uint.PopCount((uint)InitFlags)); }

	/// <summary>
	/// Gets a value indicating whether the <see cref="SubSystemSet"/> is empty
	/// </summary>
	/// <value>
	/// A value indicating whether the <see cref="SubSystemSet"/> is empty
	/// </value>
	public readonly bool IsEmpty { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => InitFlags is 0; }

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly bool Contains(SubSystem subSystem) => (InitFlags & subSystem.InitFlag) is not 0;

	/// <inheritdoc/>
	public readonly override bool Equals([NotNullWhen(true)] object? obj) => obj is SubSystemSet other && Equals(other);

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly bool Equals(params SubSystemSet other) => InitFlags == other.InitFlags;

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly override int GetHashCode() => InitFlags.GetHashCode();

	/// <inheritdoc cref="IReadOnlySet{T}.IsProperSubsetOf(IEnumerable{T})"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly bool IsProperSubsetOf(params SubSystemSet other) => !Equals(other) && IsSubsetOf(other);

	/// <inheritdoc/>
	readonly bool IReadOnlySet<SubSystem>.IsProperSubsetOf(IEnumerable<SubSystem> other) => IsProperSubsetOf(Create(other));

	/// <inheritdoc cref="IReadOnlySet{T}.IsProperSupersetOf(IEnumerable{T})"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly bool IsProperSupersetOf(params SubSystemSet other) => other.IsProperSubsetOf(this);

	/// <inheritdoc/>
	readonly bool IReadOnlySet<SubSystem>.IsProperSupersetOf(IEnumerable<SubSystem> other) => IsProperSupersetOf(Create(other));

	/// <inheritdoc cref="IReadOnlySet{T}.IsSubsetOf(IEnumerable{T})"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly bool IsSubsetOf(params SubSystemSet other) => (InitFlags & ~other.InitFlags) is 0;

	/// <inheritdoc/>
	readonly bool IReadOnlySet<SubSystem>.IsSubsetOf(IEnumerable<SubSystem> other) => IsSubsetOf(Create(other));

	/// <inheritdoc cref="IReadOnlySet{T}.IsSubsetOf(IEnumerable{T})"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly bool IsSupersetOf(params SubSystemSet other) => other.IsSubsetOf(this);

	/// <inheritdoc/>
	readonly bool IReadOnlySet<SubSystem>.IsSupersetOf(IEnumerable<SubSystem> other) => IsSupersetOf(Create(other));

	/// <inheritdoc cref="IReadOnlySet{T}.Overlaps(IEnumerable{T})"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly bool Overlaps(params SubSystemSet other) => (InitFlags & other.InitFlags) is not 0;

	/// <inheritdoc/>
	readonly bool IReadOnlySet<SubSystem>.Overlaps(IEnumerable<SubSystem> other) => Overlaps(Create(other));

	/// <inheritdoc/>
	readonly bool IReadOnlySet<SubSystem>.SetEquals(IEnumerable<SubSystem> other) => Equals(Create(other));

	/// <inheritdoc/>
	public readonly override string ToString() => ToString(format: default, formatProvider: default);

	/// <inheritdoc cref="IFormattable.ToString(string?, IFormatProvider?)"/>
	public readonly string ToString(IFormatProvider? formatProvider) => ToString(format: default, formatProvider);

	/// <inheritdoc cref="IFormattable.ToString(string?, IFormatProvider?)"/>
	public readonly string ToString(string? format) => ToString(format, formatProvider: default);

	/// <inheritdoc/>
	public readonly string ToString(string? format, IFormatProvider? formatProvider)
	{
		using var enumerator = GetEnumerator();

		if (enumerator.MoveNext())
		{
			var builder = Shared.StringBuilderPool.Get();

			try
			{
				builder.Clear();
				builder.Append(enumerator.Current.ToString(format, formatProvider));

				while (enumerator.MoveNext())
				{
					builder.Append(", ");
					builder.Append(enumerator.Current.ToString(format, formatProvider));
				}

				return builder.ToString();
			}
			finally
			{
				builder.Clear();
				Shared.StringBuilderPool.Return(builder);
			}
		}

		return "<Empty>";
	}

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

		static bool tryWriteSubSystem(SubSystem subSystem, ref Span<char> destination, ref int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
		{
			var result = subSystem.TryFormat(destination, out var tmp, format, provider);

			if (result)
			{
				destination = destination[tmp..];
				charsWritten += tmp;
			}

			return result;
		}

		charsWritten = 0;

		using var enumerator = GetEnumerator();

		if (enumerator.MoveNext())
		{
			if (!tryWriteSubSystem(enumerator.Current, ref destination, ref charsWritten, format, provider))
			{
				return false;
			}

			while (enumerator.MoveNext())
			{
				if (!tryWriteSpan(", ", ref destination, ref charsWritten) ||
					!tryWriteSubSystem(enumerator.Current, ref destination, ref charsWritten, format, provider))
				{
					return false;
				}
			}

			return true;
		}

		return tryWriteSpan("<Empty>", ref destination, ref charsWritten);
	}

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static SubSystemSet operator ~(SubSystemSet value) => new(~value.InitFlags & InitFlags.AllKnown);

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static SubSystemSet operator &(SubSystemSet left, SubSystemSet right) => new(left.InitFlags & right.InitFlags);

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static SubSystemSet operator |(SubSystemSet left, SubSystemSet right) => new(left.InitFlags | right.InitFlags);

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static SubSystemSet operator ^(SubSystemSet left, SubSystemSet right) => new(left.InitFlags ^ right.InitFlags);

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator ==(SubSystemSet left, SubSystemSet right) => left.InitFlags == right.InitFlags;

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator !=(SubSystemSet left, SubSystemSet right) => left.InitFlags != right.InitFlags;
}
