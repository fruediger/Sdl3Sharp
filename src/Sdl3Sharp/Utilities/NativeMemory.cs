using Sdl3Sharp.Internal;
using System;
using System.Diagnostics.CodeAnalysis;
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
[StructLayout(LayoutKind.Sequential)]
public readonly partial struct NativeMemory :
	INativeMemory, IEquatable<NativeMemory>, IFormattable, ISpanFormattable, IEqualityOperators<NativeMemory, NativeMemory, bool>
{
	private readonly NativeMemoryManager? mMemoryManager;
	private readonly nuint mOffset;
	private readonly nuint mLength;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	internal NativeMemory(NativeMemoryManager? memoryManager, nuint offset, nuint length)
	{
		mMemoryManager = memoryManager;
		mOffset = offset;
		mLength = length;
	}

	readonly NativeMemory INativeMemory.AsNativeMemory { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => this; }

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
	[MemberNotNullWhen(false, nameof(mMemoryManager), nameof(MemoryManager))]
	public readonly bool IsEmpty { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get { unsafe { return mMemoryManager is null || unchecked((void*)mMemoryManager.Pointer) is null || mLength is 0; } } }

	/// <summary>
	/// Gets a value indicating whether the underlying <see cref="NativeMemoryManager"/> of this allocated memory buffer is pinned
	/// </summary>
	/// <value>
	/// A value indicating whether the underlying <see cref="NativeMemoryManager"/> of this allocated memory buffer is pinned
	/// </value>
	/// <seealso cref="Pin"/>
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
	public readonly bool IsValid { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => IsEmpty || ( mOffset <= mMemoryManager.Length && mLength <= unchecked(mMemoryManager.Length - mOffset)); }

	/// <summary>
	/// Gets the number of <em>bytes</em> in the allocated memory buffer
	/// </summary>
	/// <value>
	/// The number of <em>bytes</em> in the allocated memory buffer
	/// </value>
	public readonly nuint Length { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mLength; }

	internal readonly NativeMemoryManager? MemoryManager { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mMemoryManager; }

	internal readonly nuint Offset { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mOffset; }

	/// <summary>
	/// Gets a pointer to the start of the allocated memory buffer
	/// </summary>
	/// <value>
	/// A pointer to the start of the allocated memory buffer
	/// </value>
	/// <inheritdoc cref="Validate"/>
	public readonly IntPtr Pointer
	{
		get
		{
			unsafe
			{
				return Validate() && mMemoryManager.RawPointer is var pointer && pointer is not null
					? unchecked((IntPtr)((byte*)pointer + mOffset))
					: IntPtr.Zero;
			}
		}
	}

	internal unsafe readonly void* RawPointer
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		get => mMemoryManager is not null && mMemoryManager.RawPointer is var pointer && pointer is not null
			? unchecked((void*)((byte*)pointer + mOffset))
			: null;
	}

	/// <inheritdoc/>
	public override bool Equals([NotNullWhen(true)] object? obj) => obj switch
	{
		NativeMemory other => Equals(other),
		INativeMemory { AsNativeMemory: var other } => Equals(other),
		_ => false
	};

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly bool Equals(NativeMemory other)
		=> ReferenceEquals(mMemoryManager, other.mMemoryManager)
		&& mOffset == other.mOffset
		&& mLength == other.mLength;

	/// <inheritdoc/>
	public readonly override int GetHashCode() => HashCode.Combine(RuntimeHelpers.GetHashCode(mMemoryManager), mOffset, mLength);

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

	/// <exception cref="InvalidOperationException">The <see cref="NativeMemory"/> instance is invalid (the underlying <see cref="NativeMemoryManager"/> might have changed)</exception>
	[MemberNotNullWhen(true, nameof(mMemoryManager), nameof(MemoryManager))]
	private readonly bool Validate()
	{
		if (mMemoryManager is null)
		{
			return false;
		}

		if (mOffset > mMemoryManager.Length || mLength > unchecked(mMemoryManager.Length - mOffset))
		{
			failInvalid();
		}

#pragma warning disable CS8775 // MemoryManager is not null iff mMemoryManager is not null
		return true;
#pragma warning restore CS8775

		[DoesNotReturn]
		static void failInvalid() => throw new InvalidOperationException($"The current {nameof(NativeMemory)} instance is invalid. Most likely that's because of a change in the underlying {nameof(NativeMemoryManager)}.");
	}

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator ==(NativeMemory left, NativeMemory right) => left.Equals(right);

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator !=(NativeMemory left, NativeMemory right) => !(left == right);
}