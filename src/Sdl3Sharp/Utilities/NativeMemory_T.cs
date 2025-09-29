using Sdl3Sharp.Internal;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sdl3Sharp.Utilities;

/// <summary>
/// Represents an allocated native memory buffer of elements of type <typeparamref name="T"/>
/// </summary>
/// <typeparam name="T">The type of the elements in the native memory buffer</typeparam>
[StructLayout(LayoutKind.Sequential)]
public readonly partial struct NativeMemory<T> :
	INativeMemory, IEquatable<NativeMemory>, IEquatable<NativeMemory<T>>, IFormattable, ISpanFormattable, IEqualityOperators<NativeMemory<T>, NativeMemory, bool>, IEqualityOperators<NativeMemory<T>, NativeMemory<T>, bool>
	where T : unmanaged
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

	private readonly nuint ByteOffset { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => unchecked(mOffset * ((nuint)Unsafe.SizeOf<T>() switch { 0 => 1, var size => size })); }

	private readonly nuint ByteLength { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => unchecked(mLength * ((nuint)Unsafe.SizeOf<T>() switch { 0 => 1, var size => size })); }

	/// <summary>
	/// Gets an empty <see cref="NativeMemory{T}"/>
	/// </summary>
	/// <value>
	/// An empty <see cref="NativeMemory{T}"/>
	/// </value>
	public static NativeMemory<T> Empty { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => default; }

	/// <summary>
	/// Gets a value indicating whether the allocated memory buffer is empty
	/// </summary>
	/// <value>
	/// A value indicating whether the allocated memory buffer is empty
	/// </value>
	/// <seealso cref="Empty"/>
	[MemberNotNullWhen(false, nameof(mMemoryManager), nameof(MemoryManager))]
	public readonly bool IsEmpty { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get { unsafe { return mMemoryManager is null || mMemoryManager.RawPointer is null || mLength is 0; } } }

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
	/// A valid <see cref="NativeMemory{T}"/> might become invalid after the underlying <see cref="NativeMemoryManager"/> changed (e.g. by calling <see cref="NativeMemory.TryRealloc(ref NativeMemoryManager?, nuint)"/> on it).
	/// </para>
	/// </remarks>
	public readonly bool IsValid { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => IsEmpty || (ByteOffset <= mMemoryManager.Length && ByteLength <= unchecked(mMemoryManager.Length - ByteOffset)); }

	/// <summary>
	/// Gets a reference to the element of type <typeparamref name="T"/> at a specified index into the allocated memory buffer
	/// </summary>
	/// <value>
	/// A reference to the element of type <typeparamref name="T"/> at a specified index into the allocated memory buffer
	/// </value>
	/// <param name="index">The index of the element of type <typeparamref name="T"/> into the allocated memory buffer to get a reference to</param>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is greater or equal to <see cref="Length"/></exception>
	/// <inheritdoc cref="Validate"/>
	public readonly ref T this[nuint index]
	{
		get
		{
			unsafe
			{
				if (index >= mLength)
				{
					failIndexArgumentOutOfRange();
				}

				return ref (Validate() && mMemoryManager.RawPointer is var pointer && pointer is not null
					? ref Unsafe.AsRef<T>(unchecked((T*)pointer + mOffset + index))
					: ref Unsafe.NullRef<T>());
			}

			[DoesNotReturn]
			static void failIndexArgumentOutOfRange() => throw new ArgumentOutOfRangeException(nameof(index));
		}
	}

	/// <summary>
	/// Gets the number of elements of type <typeparamref name="T"/> in the allocated memory buffer
	/// </summary>
	/// <value>
	/// The number of elements of type <typeparamref name="T"/> in the allocated memory buffer
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
					? unchecked((IntPtr)((T*)pointer + mOffset))
					: IntPtr.Zero;
			}
		}
	}

	internal unsafe readonly T* RawPointer
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		get => mMemoryManager is not null && mMemoryManager.RawPointer is var pointer && pointer is not null
			? unchecked((T*)pointer + mOffset)
			: null;
	}

	/// <summary>
	/// Gets the allocated memory buffer as a <see cref="Span{T}"/>
	/// </summary>
	/// <value>
	/// The allocated memory buffer as a <see cref="Span{T}"/>
	/// </value>
	/// <remarks>
	/// <para>
	/// The returned <see cref="Span{T}"/> might not cover the whole memory region this <see cref="NativeMemory{T}"/> covers.
	/// This comes from a technical limitation where <see cref="Span{T}"/> uses <see cref="int"/> as the type for it's <see cref="Span{T}.Length"/> property
	/// where as <see cref="NativeMemory{T}"/> uses <see cref="nuint"/> as the type for it's <see cref="NativeMemory{T}.Length"/> property.
	/// Therefore, especially on 64-bit platforms, a <see cref="NativeMemory{T}"/> could cover a larger memory region than <see cref="Span{T}"/> ever could.
	/// If that's the case, you need to do a chunk-based approach, in order to cover the whole memory region with <see cref="Span{T}"/>s.
	/// </para>
	/// </remarks>
	/// <example>
	/// This example demonstrates how you could do a chunk-based approach with <see cref="Span{T}"/>s:
	/// <code>
	/// <see cref="NativeMemory{T}">NativeMemory&lt;T&gt;</see> tempMemory = ...;
	/// for (<see cref="Span{T}">Span&lt;T&gt;</see> span = tempMemory.<see cref="NativeMemory{T}.Span">Span</see>; !span.<see cref="Span{T}.IsEmpty">IsEmpty</see>; tempMemory = tempMemory.<see cref="NativeMemory{T}.Slice(nuint)">Slice</see>((<see cref="nuint"/>)span.<see cref="Span{T}.Length">Length</see>), span = tempMemory.<see cref="NativeMemory{T}.Span">Span</see>)
	/// {
	///		// Do something with the memory in chunks using 'span'
	/// }
	/// </code>
	/// </example>
	/// <inheritdoc cref="Validate"/>
	public readonly Span<T> Span
	{
		get
		{
			unsafe
			{
				return Validate() && mMemoryManager.RawPointer is var pointer && pointer is not null
					? MemoryMarshal.CreateSpan(ref Unsafe.AsRef<T>(unchecked((T*)pointer + mOffset)), unchecked((int)System.Math.Min(mLength, int.MaxValue)))
					: [];
			}
		}
	}

	/// <inheritdoc/>
	public readonly override bool Equals([NotNullWhen(true)] object? obj) => obj switch
	{
		NativeMemory<T> other => Equals(other),
		NativeMemory other => Equals(other),
		INativeMemory { AsNativeMemory: var other } => Equals(other),
		_ => false
	};

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly bool Equals(NativeMemory other) => ((NativeMemory)this).Equals(other);

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly bool Equals(NativeMemory<T> other)
		=> ReferenceEquals(mMemoryManager, other.mMemoryManager)
		&& mOffset == other.mOffset
		&& mLength == other.mLength;

	/// <inheritdoc/>
	public readonly override int GetHashCode() => ((NativeMemory)this).GetHashCode();

	/// <summary>
	/// Pins the underlying <see cref="NativeMemoryManager"/>
	/// </summary>
	/// <returns>A <see cref="NativeMemoryPin">pin</see> pinning the underlying <see cref="NativeMemoryManager"/></returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly NativeMemoryPin Pin() => new(mMemoryManager);

	/// <summary>
	/// Gets a slice of the allocate memory buffer starting at a specified element index
	/// </summary>
	/// <param name="start">The index of an element of type <typeparamref name="T"/> where the resulting <see cref="NativeMemory{T}"/> should start</param>
	/// <returns>A slice of the allocate memory buffer starting at a element index <paramref name="start"/></returns>
	/// <exception cref="ArgumentOutOfRangeException">
	/// <paramref name="start"/> is greater than <see cref="Length"/>
	/// </exception>
	/// <inheritdoc cref="Validate"/>
	public readonly NativeMemory<T> Slice(nuint start)
	{
		unsafe
		{
			if (start > mLength)
			{
				failStartArgumentOutOfRange();
			}

			Validate();

			return new(mMemoryManager, unchecked(mOffset + start), unchecked(mLength - start));
		}

		[DoesNotReturn]
		static void failStartArgumentOutOfRange() => throw new ArgumentOutOfRangeException(nameof(start));
	}

	/// <summary>
	/// Gets a slice of the allocate memory buffer starting at a specified element index and containing a specified number of elements of type <typeparamref name="T"/>
	/// </summary>
	/// <param name="start">The index of an element of type <typeparamref name="T"/> where the resulting <see cref="NativeMemory{T}"/> should start</param>
	/// <param name="length">The number of elements of type <typeparamref name="T"/> that the resulting <see cref="NativeMemory{T}"/> should contain</param>
	/// <returns>A slice of the allocate memory buffer starting at a element index <paramref name="start"/> and containing <paramref name="length"/> number of elements of type <typeparamref name="T"/></returns>
	/// <exception cref="ArgumentOutOfRangeException">
	/// <paramref name="start"/> is greater than <see cref="Length"/>
	/// - or -
	/// <paramref name="start"/> + <paramref name="length"/> is greater than <see cref="Length"/>
	/// </exception>
	/// <inheritdoc cref="Validate"/>
	public readonly NativeMemory<T> Slice(nuint start, nuint length)
	{
		unsafe
		{
			if (start > mLength)
			{
				failStartArgumentOutOfRange();
			}

			if (length > unchecked(mLength - start))
			{
				failLengthArgumentOutOfRange();
			}

			Validate();			

			return new(mMemoryManager, unchecked(mOffset + start), length);
		}

		[DoesNotReturn]
		static void failStartArgumentOutOfRange() => throw new ArgumentOutOfRangeException(nameof(start));

		[DoesNotReturn]
		static void failLengthArgumentOutOfRange() => throw new ArgumentOutOfRangeException(nameof(length));
	}

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

	/// <exception cref="InvalidOperationException">The <see cref="NativeMemory{T}"/> instance is invalid (the underlying <see cref="NativeMemoryManager"/> might have changed)</exception>
	[MemberNotNullWhen(true, nameof(mMemoryManager), nameof(MemoryManager))]
	private readonly bool Validate()
	{
		if (mMemoryManager is null)
		{
			return false;
		}

		if (ByteOffset > mMemoryManager.Length || ByteLength > unchecked(mMemoryManager.Length - ByteOffset))
		{
			failInvalid();
		}

#pragma warning disable CS8775 // MemoryManager is not null iff mMemoryManager is not null
		return true;
#pragma warning restore CS8775

		[DoesNotReturn]
		static void failInvalid() => throw new InvalidOperationException($"The current {nameof(NativeMemory<>)} instance is invalid. Most likely that's because of a change in the underlying {nameof(NativeMemoryManager)}.");
	}

	/// <summary>
	/// Converts an <see cref="NativeMemory{T}">allocated memory buffer of type <typeparamref name="T"/></see> to an <see cref="NativeMemory">allocated memory buffer of unspecified type</see>
	/// </summary>
	/// <param name="nativeMemory">The <see cref="NativeMemory{T}">allocated memory buffer of type <typeparamref name="T"/></see> to convert to an <see cref="NativeMemory">allocated memory buffer of unspecified type</see></param>
	/// <returns>An <see cref="NativeMemory">allocated memory buffer of unspecified type</see> spanning the exact same memory region as the given <paramref name="nativeMemory"/></returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static implicit operator NativeMemory(NativeMemory<T> nativeMemory)
		=> new(nativeMemory.MemoryManager, unchecked(nativeMemory.Offset * ((nuint)Unsafe.SizeOf<T>() switch { 0 => 1, var size => size })), unchecked(nativeMemory.Length * ((nuint)Unsafe.SizeOf<T>() switch { 0 => 1, var size => size })));

	/// <summary>
	/// Converts an <see cref="NativeMemory">allocated memory buffer of unspecified type</see> to an <see cref="NativeMemory{T}">allocated memory buffer of type <typeparamref name="T"/></see> 
	/// </summary>
	/// <param name="nativeMemory">The <see cref="NativeMemory">allocated memory buffer of unspecified type</see> to convert to an <see cref="NativeMemory{T}">allocated memory buffer of type <typeparamref name="T"/></see></param>
	/// <returns>An <see cref="NativeMemory{T}">allocated memory buffer of type <typeparamref name="T"/></see> spanning the same or a similar memory region as the given <paramref name="nativeMemory"/></returns>
	/// <remarks>
	/// <para>
	/// The resulting <see cref="NativeMemory{T}">allocated memory buffer of type <typeparamref name="T"/></see> might not span all of the same memory region of the given <paramref name="nativeMemory"/>.
	/// Depending on the size of an instance of <typeparamref name="T"/>, on how it aligns in the memory region given by <paramref name="nativeMemory"/>, and on the offset from the originally allocated memory's starting point,
	/// the resulting <see cref="NativeMemory{T}">allocated memory buffer of type <typeparamref name="T"/></see> might span a memory region which is a mere segment of the given <paramref name="nativeMemory"/>'s memory region.
	/// Note: This might be still true, even if the given <paramref name="nativeMemory"/>'s <see cref="NativeMemory.Length"/> is a multiple of the size of an instance of type <typeparamref name="T"/>!
	/// </para>
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static implicit operator NativeMemory<T>(NativeMemory nativeMemory)
	{
		var (q, r) = unchecked(nuint.DivRem(nativeMemory.Offset, (nuint)Unsafe.SizeOf<T>() switch { 0 => 1, var size => size }));
		var d = unchecked((nuint)Unsafe.BitCast<bool, byte>(r is not 0));

		return new(nativeMemory.MemoryManager, offset: unchecked(q + d), length: unchecked(((r + nativeMemory.Length) / ((nuint)Unsafe.SizeOf<T>() switch { 0 => 1, var size => size })) - d));
	}

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator ==(NativeMemory<T> left, NativeMemory right) => (NativeMemory)left == right;

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator ==(NativeMemory<T> left, NativeMemory<T> right) => left.Equals(right);

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator !=(NativeMemory<T> left, NativeMemory right) => (NativeMemory)left != right;

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator !=(NativeMemory<T> left, NativeMemory<T> right) => !(left == right);
}
