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
/// Represents an allocated native memory buffer of elements of type <typeparamref name="T"/>
/// </summary>
/// <typeparam name="T">The type of the elements in the native memory buffer</typeparam>
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
[StructLayout(LayoutKind.Sequential)]
public readonly partial struct NativeMemory<T> :
	INativeMemory<NativeMemory<T>>, IEquatable<NativeMemory<T>>, IFormattable, ISpanFormattable, IEqualityOperators<NativeMemory<T>, NativeMemory<T>, bool>
	where T : unmanaged
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
	internal unsafe NativeMemory(T* pointer, nuint length)
	{
		mMemoryManager = null;
		mOffsetOrPointer = unchecked((nuint)pointer);
		mLength = length;
	}

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private string DebuggerDisplay => ToString(CultureInfo.InvariantCulture);

	private readonly nuint ByteOffset { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => unchecked(mOffsetOrPointer * ((nuint)Unsafe.SizeOf<T>() switch { 0 => 1, var size => size })); }

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
	public readonly bool IsEmpty
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		get
		{
			unsafe
			{
				return (mMemoryManager is not null
					? mMemoryManager.RawPointer is null
					: unchecked((T*)mOffsetOrPointer) is null)
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
	/// A valid <see cref="NativeMemory{T}"/> might become invalid after the underlying <see cref="NativeMemoryManager"/> changed (e.g. by calling <see cref="NativeMemory.TryRealloc(ref NativeMemoryManager?, nuint)"/> on it).
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
					? mMemoryManager.RawPointer is not null && ByteOffset <= mMemoryManager.Length && ByteLength <= unchecked(mMemoryManager.Length - ByteOffset)
					: unchecked((T*)mOffsetOrPointer) is not null || mLength is 0;
			}
		}
	}

	/// <summary>
	/// Gets a reference to the element of type <typeparamref name="T"/> at a specified index into the allocated memory buffer
	/// </summary>
	/// <value>
	/// A reference to the element of type <typeparamref name="T"/> at a specified index into the allocated memory buffer
	/// </value>
	/// <param name="index">The index of the element of type <typeparamref name="T"/> into the allocated memory buffer to get a reference to</param>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is greater or equal to <see cref="Length"/></exception>
	/// <inheritdoc cref="GetValidPointer"/>
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

				return ref Unsafe.AsRef<T>(unchecked(GetValidPointer() + index));
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

	internal readonly nuint OffsetOrPointer { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mOffsetOrPointer; }

	/// <summary>
	/// Gets a pointer to the start of the allocated memory buffer
	/// </summary>
	/// <value>
	/// A pointer to the start of the allocated memory buffer
	/// </value>
	public readonly IntPtr Pointer { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get { unsafe { return unchecked((IntPtr)RawPointer); } } }

	internal unsafe readonly T* RawPointer
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		get => mMemoryManager is not null
			? mMemoryManager.RawPointer is var pointer && pointer is not null
				? unchecked((T*)pointer + mOffsetOrPointer)
				: null
			: unchecked((T*)mOffsetOrPointer);
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
	/// <inheritdoc cref="GetValidPointer"/>
	public readonly Span<T> Span { get { unsafe { return MemoryMarshal.CreateSpan(ref Unsafe.AsRef<T>(GetValidPointer()), unchecked((int)System.Math.Min(mLength, int.MaxValue))); } } }

	/// <inheritdoc/>
	public readonly override bool Equals([NotNullWhen(true)] object? obj) => obj switch
	{
		NativeMemory<T> other => Equals(other),
		ReadOnlyNativeMemory other => Equals(other),
		INativeMemory { AsReadOnlyNativeMemory: var other } => Equals(other),
		_ => false
	};

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly bool Equals(ReadOnlyNativeMemory other) => ((NativeMemory)this).Equals(other);

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly bool Equals(NativeMemory<T> other)
		=> ReferenceEquals(mMemoryManager, other.mMemoryManager)
		&& mOffsetOrPointer == other.mOffsetOrPointer
		&& mLength == other.mLength;

	/// <inheritdoc/>
	public readonly override int GetHashCode() => ((NativeMemory)this).GetHashCode();

	/// <exception cref="InvalidOperationException">The <see cref="NativeMemory{T}"/> instance is invalid (the underlying <see cref="NativeMemoryManager"/> might have changed)</exception>
	[DoesNotReturn]
	private static void FailInvalid() => throw new InvalidOperationException($"The current {nameof(NativeMemory<>)} instance is invalid. Most likely that's because the current {nameof(NativeMemory<>)} instance was invalid from the beginning or because of a change in the underlying {nameof(NativeMemoryManager)}.");

	/// <inheritdoc cref="FailInvalid"/>
	private unsafe readonly T* GetValidPointer()
	{
		T* pointer;
		if (mMemoryManager is not null)
		{
			pointer = unchecked((T*)mMemoryManager.RawPointer);

			if (pointer is null || ByteOffset > mMemoryManager.Length || ByteLength > unchecked(mMemoryManager.Length - ByteOffset))
			{
				FailInvalid();
			}

			pointer += mOffsetOrPointer;
		}
		else
		{
			pointer = unchecked((T*)mOffsetOrPointer);

			if (pointer is null && mLength is not 0)
			{
				FailInvalid();
			}
		}

		return pointer;
	}

	/// <inheritdoc cref="FailInvalid"/>
	private unsafe readonly nuint GetValidNewOffsetOrPointer(nuint start)
	{
		nuint newOffsetOrPointer;
		if (mMemoryManager is not null)
		{
			var pointer = unchecked((T*)mMemoryManager.RawPointer);

			if (pointer is null || ByteOffset > mMemoryManager.Length || ByteLength > unchecked(mMemoryManager.Length - ByteOffset))
			{
				FailInvalid();
			}

			newOffsetOrPointer = unchecked(mOffsetOrPointer + start);
		}
		else
		{
			var pointer = unchecked((T*)mOffsetOrPointer);

			if (pointer is null && mLength is not 0)
			{
				FailInvalid();
			}

			newOffsetOrPointer = unchecked((nuint)(pointer + start));
		}

		return newOffsetOrPointer;
	}

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
	/// <inheritdoc cref="GetValidNewOffsetOrPointer(nuint)"/>
	public readonly NativeMemory<T> Slice(nuint start)
	{
		unsafe
		{
			if (start > mLength)
			{
				failStartArgumentOutOfRange();
			}

			var newOffsetOrPointer = GetValidNewOffsetOrPointer(start);

			return new(mMemoryManager, newOffsetOrPointer, unchecked(mLength - start));
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
	/// <inheritdoc cref="GetValidNewOffsetOrPointer(nuint)"/>
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
			
			var newOffsetOrPointer = GetValidNewOffsetOrPointer(start);

			return new(mMemoryManager, newOffsetOrPointer, length);
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
	public static explicit operator NativeMemory<T>(NativeMemory nativeMemory)
	{
		unsafe
		{
			// what happens here is actually quite hard to explain, but I did the math on paper and triple checked it,
			// so I'm absolutely *not* confident that everything is correct

			var size = (nuint)Unsafe.SizeOf<T>() switch { 0 => 1u, var s => s };
			var (q, r) = unchecked(nuint.DivRem(nativeMemory.OffsetOrPointer, size));	
			var d = unchecked((nuint)Unsafe.BitCast<bool, byte>(r is not 0));

			return new(
				nativeMemory.MemoryManager,
				nativeMemory.MemoryManager is not null
					? unchecked(q + d)
					: d is not 0
						? unchecked(size - r) switch { var a when a <= nativeMemory.Length => unchecked(nativeMemory.OffsetOrPointer + a), _ => unchecked((nuint)(void*)null) }
						: nativeMemory.OffsetOrPointer,
				unchecked((r + nativeMemory.Length) / size) switch { var l when l > d => unchecked(l - d), _ => 0 }
			);
		}
	}

	/// <summary>
	/// Converts an <see cref="NativeMemory{T}">allocated memory buffer of type <typeparamref name="T"/></see> to an <see cref="NativeMemory">allocated memory buffer of unspecified type</see>
	/// </summary>
	/// <param name="nativeMemory">The <see cref="NativeMemory{T}">allocated memory buffer of type <typeparamref name="T"/></see> to convert to an <see cref="NativeMemory">allocated memory buffer of unspecified type</see></param>
	/// <returns>An <see cref="NativeMemory">allocated memory buffer of unspecified type</see> spanning the exact same memory region as the given <paramref name="nativeMemory"/></returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static implicit operator NativeMemory(NativeMemory<T> nativeMemory)
	{
		var size = (nuint)Unsafe.SizeOf<T>() switch { 0 => 1u, var s => s };;
		return new(
			nativeMemory.MemoryManager,
			nativeMemory.MemoryManager is not null
				? unchecked(nativeMemory.OffsetOrPointer * size)
				: nativeMemory.OffsetOrPointer,
			unchecked(nativeMemory.Length * size)
		);
	}

	/// <summary>
	/// Converts an <see cref="NativeMemory{T}">allocated memory buffer of type <typeparamref name="T"/></see> to an <see cref="ReadOnlyNativeMemory">allocated <em>read-only</em> memory buffer of unspecified type</see>
	/// </summary>
	/// <param name="nativeMemory">The <see cref="NativeMemory{T}">allocated memory buffer of type <typeparamref name="T"/></see> to convert to an <see cref="ReadOnlyNativeMemory">allocated <em>read-only</em> memory buffer of unspecified type</see></param>
	/// <returns>An <see cref="ReadOnlyNativeMemory">allocated <em>read-only</em> memory buffer of unspecified type</see> spanning the exact same memory region as the given <paramref name="nativeMemory"/></returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static implicit operator ReadOnlyNativeMemory(NativeMemory<T> nativeMemory) => new(nativeMemory);

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator ==(NativeMemory<T> left, NativeMemory<T> right) => left.Equals(right);

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator ==(NativeMemory<T> left, ReadOnlyNativeMemory right) => left.Equals(right);

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator !=(NativeMemory<T> left, NativeMemory<T> right) => !(left == right);

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator !=(NativeMemory<T> left, ReadOnlyNativeMemory right) => !(left == right);
}
