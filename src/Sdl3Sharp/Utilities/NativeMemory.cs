using Sdl3Sharp.Internal;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sdl3Sharp.Utilities;

/// <summary>
/// Represents an allocated native memory buffer of unspecified type.
/// Also provides methods for managing native memory allocations, including managing SDL's internal allocators.
/// </summary>
/// <remarks>
/// <para>
/// Note: Some of the static methods in this class require the caller to manually free the allocated memory using the appropriate free method (e.g., <see cref="Free(void*)"/> or <see cref="AlignedFree(void*)"/>).
/// Failure to do so may result in memory leaks. 
/// </para>
/// </remarks>
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
[StructLayout(LayoutKind.Sequential)]
public readonly partial struct NativeMemory :
	INativeMemory<NativeMemory>, IEquatable<NativeMemory>, IFormattable, ISpanFormattable, IEqualityOperators<NativeMemory, NativeMemory, bool>
{
	private readonly NativeMemoryManager? mMemoryManager;
	private readonly nuint mOffsetOrPointer;
	private readonly nuint mLength;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	internal NativeMemory(NativeMemoryManager? memoryManager, nuint offset, nuint length)
	{
		mMemoryManager = memoryManager;
		mOffsetOrPointer = offset;
		mLength = length;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	internal unsafe NativeMemory(void* pointer, nuint length)
	{
		mMemoryManager = null;
		mOffsetOrPointer = unchecked((nuint)pointer);
		mLength = length;
	}

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private string DebuggerDisplay => ToString(CultureInfo.InvariantCulture);

	/// <summary>
	/// Gets an empty <see cref="NativeMemory"/>
	/// </summary>
	/// <value>
	/// An empty <see cref="NativeMemory"/>
	/// </value>
	public static NativeMemory Empty { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => default; }

	/// <summary>
	/// Gets a value indicating whether the allocated memory buffer is empty
	/// </summary>
	/// <value>
	/// A value indicating whether the allocated memory buffer is empty
	/// </value>
	/// <seealso cref="Empty"/>
	public readonly bool IsEmpty
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		get
		{
			unsafe
			{
				return (mMemoryManager is not null
					? mMemoryManager.RawPointer is null
					: unchecked((void*)mOffsetOrPointer) is null)
					|| mLength is 0;
			}
		}
	}

	/// <summary>
	/// Gets a value indicating whether the underlying <see cref="NativeMemoryManager"/> of this allocated memory buffer is pinned
	/// </summary>
	/// <value>
	/// A value indicating whether the underlying <see cref="NativeMemoryManager"/> of this allocated memory buffer is pinned
	/// </value>
	/// <seealso cref="Pin"/>
	[MemberNotNullWhen(true, nameof(mMemoryManager), nameof(MemoryManager))]
	public readonly bool IsPinned { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mMemoryManager?.IsPinned is true; }

	/// <summary>
	/// Gets a value indicating whether the allocated memory buffer is valid
	/// </summary>
	/// <value>
	/// A value indicating whether the allocated memory buffer is valid
	/// </value>
	/// <remarks>
	/// <para>
	/// A valid <see cref="NativeMemory"/> might become invalid after the underlying <see cref="NativeMemoryManager"/> changed (e.g. by calling <see cref="TryRealloc(ref NativeMemoryManager?, nuint)"/> on it).
	/// </para>
	/// </remarks>
	public readonly bool IsValid
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		get
		{
			unsafe
			{
				return mMemoryManager is not null
					? mMemoryManager.RawPointer is not null && mOffsetOrPointer <= mMemoryManager.Length && mLength <= unchecked(mMemoryManager.Length - mOffsetOrPointer)
					: unchecked((void*)mOffsetOrPointer) is not null || mLength is 0;
			}
		}
	}

	/// <summary>
	/// Gets the number of <em>bytes</em> in the allocated memory buffer
	/// </summary>
	/// <value>
	/// The number of <em>bytes</em> in the allocated memory buffer
	/// </value>
	public readonly nuint Length { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mLength; }

	internal readonly NativeMemoryManager? MemoryManager { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mMemoryManager; }

	internal readonly nuint OffsetOrPointer { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mOffsetOrPointer; }

	/// <summary>
	/// Gets a pointer to the start of the allocated memory buffer
	/// </summary>
	/// <value>
	/// A pointer to the start of the allocated memory buffer
	/// </value>
	public readonly IntPtr Pointer { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get { unsafe { return unchecked((IntPtr)RawPointer); } } }

	internal unsafe readonly void* RawPointer
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		get => mMemoryManager is not null
			? mMemoryManager.RawPointer is var pointer && pointer is not null
				? unchecked((byte*)pointer + mOffsetOrPointer)
				: null
			: unchecked((void*)mOffsetOrPointer);
	}

	/// <inheritdoc/>
	public readonly override bool Equals([NotNullWhen(true)] object? obj) => obj switch
	{
		NativeMemory other => Equals(other),
		ReadOnlyNativeMemory other => Equals(other),
		INativeMemory { AsReadOnlyNativeMemory: var other } => Equals(other),
		_ => false
	};

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly bool Equals(ReadOnlyNativeMemory other) => Equals(other.NativeMemory);

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly bool Equals(NativeMemory other)
		=> ReferenceEquals(mMemoryManager, other.mMemoryManager)
		&& mOffsetOrPointer == other.mOffsetOrPointer
		&& mLength == other.mLength;

	/// <inheritdoc/>
	public readonly override int GetHashCode() => HashCode.Combine(RuntimeHelpers.GetHashCode(mMemoryManager), mOffsetOrPointer, mLength);

	/// <summary>
	/// Pins the underlying <see cref="NativeMemoryManager"/>
	/// </summary>
	/// <returns>A <see cref="NativeMemoryPin">pin</see> pinning the underlying <see cref="NativeMemoryManager"/></returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly NativeMemoryPin Pin() => new(mMemoryManager);

	/// <inheritdoc/>
	public readonly override string ToString() => ToString(format: default, formatProvider: default);

	/// <inheritdoc cref="IFormattable.ToString(string?, IFormatProvider?)"/>
	public readonly string ToString(IFormatProvider? formatProvider) => ToString(format: default, formatProvider);

	/// <inheritdoc cref="IFormattable.ToString(string?, IFormatProvider?)"/>
	public readonly string ToString(string? format) => ToString(format, formatProvider: default);

	/// <inheritdoc/>
	public readonly string ToString(string? format, IFormatProvider? formatProvider)
		=> $"{{ {nameof(Length)}: {Length} }}";

	/// <inheritdoc/>
	public readonly bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format = default, IFormatProvider? provider = default)
	{
		charsWritten = 0;

		return SpanFormat.TryWrite($"{{ {nameof(Length)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(Length, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite(" }", ref destination, ref charsWritten);
	}

	/// <summary>
	/// Converts an <see cref="NativeMemory">allocated memory buffer of unspecified type</see> to an <see cref="ReadOnlyNativeMemory">allocated <em>read-only</em> memory buffer of unspecified type</see>
	/// </summary>
	/// <param name="nativeMemory">The <see cref="NativeMemory">allocated memory buffer of unspecified type</see> to convert to an <see cref="ReadOnlyNativeMemory">allocated <em>read-only</em> memory buffer of unspecified type</see></param>
	/// <returns>An <see cref="ReadOnlyNativeMemory">allocated <em>read-only</em> memory buffer of unspecified type</see> spanning the exact same memory region as the given <paramref name="nativeMemory"/></returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static implicit operator ReadOnlyNativeMemory(NativeMemory nativeMemory) => new(nativeMemory);

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator ==(NativeMemory left, NativeMemory right) => left.Equals(right);

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator ==(NativeMemory left, ReadOnlyNativeMemory right) => left.Equals(right);

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator !=(NativeMemory left, NativeMemory right) => !(left == right);

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator !=(NativeMemory left, ReadOnlyNativeMemory right) => !(left == right);
}