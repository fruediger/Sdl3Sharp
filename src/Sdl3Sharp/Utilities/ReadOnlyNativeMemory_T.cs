using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sdl3Sharp.Utilities;

/// <summary>
/// Represents an allocated <em>read-only</em> native memory buffer of elements of type <typeparamref name="T"/>
/// </summary>
/// <typeparam name="T">The type of the elements in the native memory buffer</typeparam>
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
[StructLayout(LayoutKind.Sequential)]
public readonly partial struct ReadOnlyNativeMemory<T> :
	INativeMemory<ReadOnlyNativeMemory<T>>, IEquatable<ReadOnlyNativeMemory<T>>, IFormattable, ISpanFormattable, IEqualityOperators<ReadOnlyNativeMemory<T>, ReadOnlyNativeMemory<T>, bool>
	where T : unmanaged
{
	private readonly NativeMemory<T> mNativeMemory;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	internal ReadOnlyNativeMemory(NativeMemory<T> nativeMemory) => mNativeMemory = nativeMemory;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private string DebuggerDisplay => ToString(CultureInfo.InvariantCulture);

	/// <summary>
	/// Gets an empty <see cref="ReadOnlyNativeMemory{T}"/>
	/// </summary>
	/// <value>
	/// An empty <see cref="ReadOnlyNativeMemory{T}"/>
	/// </value>
	public static ReadOnlyNativeMemory<T> Empty { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(NativeMemory<T>.Empty); }

	/// <summary>
	/// Gets a value indicating whether the allocated memory buffer is empty
	/// </summary>
	/// <value>
	/// A value indicating whether the allocated memory buffer is empty
	/// </value>
	/// <seealso cref="Empty"/>
	public readonly bool IsEmpty { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mNativeMemory.IsEmpty; }

	/// <summary>
	/// Gets a value indicating whether the underlying <see cref="NativeMemoryManagerBase"/> of this allocated memory buffer is pinned
	/// </summary>
	/// <value>
	/// A value indicating whether the underlying <see cref="NativeMemoryManagerBase"/> of this allocated memory buffer is pinned
	/// </value>
	/// <seealso cref="Pin"/>
	public readonly bool IsPinned { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mNativeMemory.IsPinned; }

	/// <summary>
	/// Gets a value indicating whether the allocated memory buffer is valid
	/// </summary>
	/// <value>
	/// A value indicating whether the allocated memory buffer is valid
	/// </value>
	/// <remarks>
	/// <para>
	/// A valid <see cref="ReadOnlyNativeMemory{T}"/> might become invalid after the underlying <see cref="NativeMemoryManager"/> changed (e.g. by calling <see cref="NativeMemory.TryRealloc(ref NativeMemoryManager?, nuint)"/> on it).
	/// </para>
	/// </remarks>
	public readonly bool IsValid { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mNativeMemory.IsValid; }

	/// <summary>
	/// Gets a <em>read-only</em> reference to the element of type <typeparamref name="T"/> at a specified index into the allocated memory buffer
	/// </summary>
	/// <value>
	/// A <em>read-only</em> reference to the element of type <typeparamref name="T"/> at a specified index into the allocated memory buffer
	/// </value>
	/// <param name="index">The index of the element of type <typeparamref name="T"/> into the allocated memory buffer to get a <em>read-only</em> reference to</param>
	/// <inheritdoc cref="NativeMemory{T}.this[nuint]"/>
	public readonly ref readonly T this[nuint index] { get => ref mNativeMemory[index]; }

	/// <summary>
	/// Gets the number of elements of type <typeparamref name="T"/> in the allocated memory buffer
	/// </summary>
	/// <value>
	/// The number of elements of type <typeparamref name="T"/> in the allocated memory buffer
	/// </value>
	public readonly nuint Length { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mNativeMemory.Length; }

	internal readonly NativeMemoryManagerBase? MemoryManager { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mNativeMemory.MemoryManager; }

	internal readonly NativeMemory<T> NativeMemory { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mNativeMemory; }

	internal readonly nuint OffsetOrPointer { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mNativeMemory.OffsetOrPointer; }

	/// <summary>
	/// Gets a pointer to the start of the allocated memory buffer
	/// </summary>
	/// <value>
	/// A pointer to the start of the allocated memory buffer
	/// </value>
	public readonly IntPtr Pointer { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mNativeMemory.Pointer; }

	internal unsafe readonly T* RawPointer { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mNativeMemory.RawPointer; }

	/// <summary>
	/// Gets the allocated memory buffer as a <see cref="ReadOnlySpan{T}"/>
	/// </summary>
	/// <value>
	/// The allocated memory buffer as a <see cref="ReadOnlySpan{T}"/>
	/// </value>
	/// <remarks>
	/// <para>
	/// The returned <see cref="ReadOnlySpan{T}"/> might not cover the whole memory region this <see cref="ReadOnlyNativeMemory{T}"/> covers.
	/// This comes from a technical limitation where <see cref="ReadOnlySpan{T}"/> uses <see cref="int"/> as the type for it's <see cref="ReadOnlySpan{T}.Length"/> property
	/// where as <see cref="ReadOnlyNativeMemory{T}"/> uses <see cref="nuint"/> as the type for it's <see cref="ReadOnlyNativeMemory{T}.Length"/> property.
	/// Therefore, especially on 64-bit platforms, a <see cref="ReadOnlyNativeMemory{T}"/> could cover a larger memory region than <see cref="ReadOnlySpan{T}"/> ever could.
	/// If that's the case, you need to do a chunk-based approach, in order to cover the whole memory region with <see cref="ReadOnlySpan{T}"/>s.
	/// </para>
	/// </remarks>
	/// <example>
	/// This example demonstrates how you could do a chunk-based approach with <see cref="ReadOnlySpan{T}"/>s:
	/// <code>
	/// <see cref="ReadOnlyNativeMemory{T}">ReadOnlyNativeMemory&lt;T&gt;</see> tempMemory = ...;
	/// for (<see cref="ReadOnlySpan{T}">ReadOnlySpan&lt;T&gt;</see> span = tempMemory.<see cref="ReadOnlyNativeMemory{T}.Span">Span</see>; !span.<see cref="ReadOnlySpan{T}.IsEmpty">IsEmpty</see>; tempMemory = tempMemory.<see cref="ReadOnlyNativeMemory{T}.Slice(nuint)">Slice</see>((<see cref="nuint"/>)span.<see cref="ReadOnlySpan{T}.Length">Length</see>), span = tempMemory.<see cref="ReadOnlyNativeMemory{T}.Span">Span</see>)
	/// {
	///		// Do something with the memory in chunks using 'span'
	/// }
	/// </code>
	/// </example>
	/// <inheritdoc cref="NativeMemory{T}.Span"/>
	public readonly ReadOnlySpan<T> Span { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mNativeMemory.Span; }

	/// <inheritdoc/>
	public readonly override bool Equals([NotNullWhen(true)] object? obj) => obj switch
	{
		ReadOnlyNativeMemory<T> other => Equals(other),
		ReadOnlyNativeMemory other => Equals(other),
		INativeMemory { AsReadOnlyNativeMemory: var other } => Equals(other),
		_ => false
	};

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly bool Equals(ReadOnlyNativeMemory other) => mNativeMemory.Equals(other);

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly bool Equals(ReadOnlyNativeMemory<T> other) => mNativeMemory.Equals(other.NativeMemory);

	/// <inheritdoc/>
	public readonly override int GetHashCode() => mNativeMemory.GetHashCode();

	/// <summary>
	/// Pins the underlying <see cref="NativeMemoryManager"/>
	/// </summary>
	/// <returns>A <see cref="NativeMemoryPin">pin</see> pinning the underlying <see cref="NativeMemoryManager"/></returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly NativeMemoryPin Pin() => mNativeMemory.Pin();

	/// <summary>
	/// Gets a slice of the allocate memory buffer starting at a specified element index
	/// </summary>
	/// <param name="start">The index of an element of type <typeparamref name="T"/> where the resulting <see cref="ReadOnlyNativeMemory{T}"/> should start</param>
	/// <returns>A slice of the allocate memory buffer starting at a element index <paramref name="start"/></returns>
	/// <inheritdoc cref="NativeMemory{T}.Slice(nuint)"/>
	public readonly ReadOnlyNativeMemory<T> Slice(nuint start) => new(mNativeMemory.Slice(start));

	/// <summary>
	/// Gets a slice of the allocate memory buffer starting at a specified element index and containing a specified number of elements of type <typeparamref name="T"/>
	/// </summary>
	/// <param name="start">The index of an element of type <typeparamref name="T"/> where the resulting <see cref="ReadOnlyNativeMemory{T}"/> should start</param>
	/// <param name="length">The number of elements of type <typeparamref name="T"/> that the resulting <see cref="ReadOnlyNativeMemory{T}"/> should contain</param>
	/// <returns>A slice of the allocate memory buffer starting at a element index <paramref name="start"/> and containing <paramref name="length"/> number of elements of type <typeparamref name="T"/></returns>
	/// <inheritdoc cref="NativeMemory{T}.Slice(nuint, nuint)"/>
	public readonly ReadOnlyNativeMemory<T> Slice(nuint start, nuint length) => new(mNativeMemory.Slice(start, length));

	/// <inheritdoc/>
	public readonly override string ToString() => mNativeMemory.ToString();

	/// <inheritdoc cref="IFormattable.ToString(string?, IFormatProvider?)"/>
	public readonly string ToString(IFormatProvider? formatProvider) => mNativeMemory.ToString(formatProvider);

	/// <inheritdoc cref="IFormattable.ToString(string?, IFormatProvider?)"/>
	public readonly string ToString(string? format) => mNativeMemory.ToString(format);

	/// <inheritdoc/>
	public readonly string ToString(string? format, IFormatProvider? formatProvider) => mNativeMemory.ToString(format, formatProvider);

	/// <inheritdoc/>
	public readonly bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format = default, IFormatProvider? provider = default)
		=> mNativeMemory.TryFormat(destination, out charsWritten, format, provider);

	/// <summary>
	/// Converts an <see cref="NativeMemory">allocated memory buffer of unspecified type</see> to an <see cref="ReadOnlyNativeMemory{T}">allocated <em>read-only</em> memory buffer of type <typeparamref name="T"/></see> 
	/// </summary>
	/// <param name="nativeMemory">The <see cref="NativeMemory">allocated memory buffer of unspecified type</see> to convert to an <see cref="ReadOnlyNativeMemory{T}">allocated <em>read-only</em> memory buffer of type <typeparamref name="T"/></see></param>
	/// <returns>An <see cref="ReadOnlyNativeMemory{T}">allocated <em>read-only</em> memory buffer of type <typeparamref name="T"/></see> spanning the same or a similar memory region as the given <paramref name="nativeMemory"/></returns>
	/// <remarks>
	/// <para>
	/// The resulting <see cref="ReadOnlyNativeMemory{T}">allocated <em>read-only</em> memory buffer of type <typeparamref name="T"/></see> might not span all of the same memory region of the given <paramref name="nativeMemory"/>.
	/// Depending on the size of an instance of <typeparamref name="T"/>, on how it aligns in the memory region given by <paramref name="nativeMemory"/>, and on the offset from the originally allocated memory's starting point,
	/// the resulting <see cref="ReadOnlyNativeMemory{T}">allocated <em>read-only</em> memory buffer of type <typeparamref name="T"/></see> might span a memory region which is a mere segment of the given <paramref name="nativeMemory"/>'s memory region.
	/// Note: This might be still true, even if the given <paramref name="nativeMemory"/>'s <see cref="NativeMemory.Length"/> is a multiple of the size of an instance of type <typeparamref name="T"/>!
	/// </para>
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static explicit operator ReadOnlyNativeMemory<T>(NativeMemory nativeMemory) => new((NativeMemory<T>)nativeMemory);

	/// <summary>
	/// Converts an <see cref="NativeMemory{T}">allocated memory buffer of type <typeparamref name="T"/></see> to an <see cref="ReadOnlyNativeMemory{T}">allocated <em>read-only</em> memory buffer of type <typeparamref name="T"/></see>
	/// </summary>
	/// <param name="nativeMemory">The <see cref="NativeMemory{T}">allocated memory buffer of type <typeparamref name="T"/></see> to convert to an <see cref="ReadOnlyNativeMemory{T}">allocated <em>read-only</em> memory buffer of type <typeparamref name="T"/></see></param>
	/// <returns>An <see cref="ReadOnlyNativeMemory{T}">allocated <em>read-only</em> memory buffer of type <typeparamref name="T"/></see> spanning the exact same memory region as the given <paramref name="nativeMemory"/></returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static implicit operator ReadOnlyNativeMemory<T>(NativeMemory<T> nativeMemory) => new(nativeMemory);

	/// <summary>
	/// Converts an <see cref="ReadOnlyNativeMemory">allocated <em>read-only</em> memory buffer of unspecified type</see> to an <see cref="ReadOnlyNativeMemory{T}">allocated <em>read-only</em> memory buffer of type <typeparamref name="T"/></see> 
	/// </summary>
	/// <param name="nativeMemory">The <see cref="ReadOnlyNativeMemory">allocated <em>read-only</em> memory buffer of unspecified type</see> to convert to an <see cref="ReadOnlyNativeMemory{T}">allocated <em>read-only</em> memory buffer of type <typeparamref name="T"/></see></param>
	/// <returns>An <see cref="ReadOnlyNativeMemory{T}">allocated <em>read-only</em> memory buffer of type <typeparamref name="T"/></see> spanning the same or a similar memory region as the given <paramref name="nativeMemory"/></returns>
	/// <remarks>
	/// <para>
	/// The resulting <see cref="ReadOnlyNativeMemory{T}">allocated <em>read-only</em> memory buffer of type <typeparamref name="T"/></see> might not span all of the same memory region of the given <paramref name="nativeMemory"/>.
	/// Depending on the size of an instance of <typeparamref name="T"/>, on how it aligns in the memory region given by <paramref name="nativeMemory"/>, and on the offset from the originally allocated memory's starting point,
	/// the resulting <see cref="ReadOnlyNativeMemory{T}">allocated <em>read-only</em> memory buffer of type <typeparamref name="T"/></see> might span a memory region which is a mere segment of the given <paramref name="nativeMemory"/>'s memory region.
	/// Note: This might be still true, even if the given <paramref name="nativeMemory"/>'s <see cref="ReadOnlyNativeMemory.Length"/> is a multiple of the size of an instance of type <typeparamref name="T"/>!
	/// </para>
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static explicit operator ReadOnlyNativeMemory<T>(ReadOnlyNativeMemory nativeMemory) => new((NativeMemory<T>)nativeMemory.NativeMemory);

	/// <summary>
	/// Converts an <see cref="ReadOnlyNativeMemory{T}">allocated <em>read-only</em> memory buffer of type <typeparamref name="T"/></see> to an <see cref="NativeMemory">allocated <em>read-only</em> memory buffer of unspecified type</see>
	/// </summary>
	/// <param name="nativeMemory">The <see cref="ReadOnlyNativeMemory{T}">allocated <em>read-only</em> memory buffer of type <typeparamref name="T"/></see> to convert to an <see cref="ReadOnlyNativeMemory">allocated <em>read-only</em> memory buffer of unspecified type</see></param>
	/// <returns>An <see cref="ReadOnlyNativeMemory">allocated <em>read-only</em> memory buffer of unspecified type</see> spanning the exact same memory region as the given <paramref name="nativeMemory"/></returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static implicit operator ReadOnlyNativeMemory(ReadOnlyNativeMemory<T> nativeMemory) => new(nativeMemory.NativeMemory);

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator ==(ReadOnlyNativeMemory<T> left, ReadOnlyNativeMemory<T> right) => left.Equals(right);

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator ==(ReadOnlyNativeMemory<T> left, ReadOnlyNativeMemory right) => left.Equals(right);

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator !=(ReadOnlyNativeMemory<T> left, ReadOnlyNativeMemory<T> right) => !(left == right);

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator !=(ReadOnlyNativeMemory<T> left, ReadOnlyNativeMemory right) => !(left == right);
}
