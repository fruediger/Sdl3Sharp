using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sdl3Sharp.Utilities;

/// <summary>
/// Represents an allocated <em>read-only</em> native memory buffer of unspecified type
/// </summary>
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
[StructLayout(LayoutKind.Sequential)]
public readonly partial struct ReadOnlyNativeMemory :
	INativeMemory<ReadOnlyNativeMemory>, IEquatable<ReadOnlyNativeMemory>, IFormattable, ISpanFormattable, IEqualityOperators<ReadOnlyNativeMemory, ReadOnlyNativeMemory, bool>
{
	private readonly NativeMemory mNativeMemory;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	internal ReadOnlyNativeMemory(NativeMemory nativeMemory) => mNativeMemory = nativeMemory;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private string DebuggerDisplay => ToString(CultureInfo.InvariantCulture);
	
	/// <summary>
	/// Gets an empty <see cref="ReadOnlyNativeMemory"/>
	/// </summary>
	/// <value>
	/// An empty <see cref="ReadOnlyNativeMemory"/>
	/// </value>
	public static ReadOnlyNativeMemory Empty { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(NativeMemory.Empty); }

	/// <summary>
	/// Gets a value indicating whether the allocated memory buffer is empty
	/// </summary>
	/// <value>
	/// A value indicating whether the allocated memory buffer is empty
	/// </value>
	/// <seealso cref="Empty"/>
	public readonly bool IsEmpty { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mNativeMemory.IsEmpty; }

	/// <summary>
	/// Gets a value indicating whether the underlying <see cref="NativeMemoryManager"/> of this allocated memory buffer is pinned
	/// </summary>
	/// <value>
	/// A value indicating whether the underlying <see cref="NativeMemoryManager"/> of this allocated memory buffer is pinned
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
	/// A valid <see cref="ReadOnlyNativeMemory"/> might become invalid after the underlying <see cref="NativeMemoryManager"/> changed (e.g. by calling <see cref="NativeMemory.TryRealloc(ref NativeMemoryManager?, nuint)"/> on it).
	/// </para>
	/// </remarks>
	public readonly bool IsValid { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mNativeMemory.IsValid; }

	/// <summary>
	/// Gets the number of <em>bytes</em> in the allocated memory buffer
	/// </summary>
	/// <value>
	/// The number of <em>bytes</em> in the allocated memory buffer
	/// </value>
	public readonly nuint Length { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mNativeMemory.Length; }

	internal readonly NativeMemoryManager? MemoryManager { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mNativeMemory.MemoryManager; }

	internal readonly NativeMemory NativeMemory { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mNativeMemory; }

	internal readonly nuint OffsetOrPointer { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mNativeMemory.OffsetOrPointer; }

	/// <summary>
	/// Gets a pointer to the start of the allocated memory buffer
	/// </summary>
	/// <value>
	/// A pointer to the start of the allocated memory buffer
	/// </value>
	public readonly IntPtr Pointer { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mNativeMemory.Pointer; }

	internal unsafe readonly void* RawPointer { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mNativeMemory.RawPointer; }

	/// <inheritdoc/>
	public readonly override bool Equals([NotNullWhen(true)] object? obj) => obj switch
	{
		ReadOnlyNativeMemory other => Equals(other),
		INativeMemory { AsReadOnlyNativeMemory: var other } => Equals(other),
		_ => false
	};

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly bool Equals(ReadOnlyNativeMemory other) => mNativeMemory.Equals(other);

	/// <inheritdoc/>
	public readonly override int GetHashCode() => mNativeMemory.GetHashCode();

	/// <summary>
	/// Pins the underlying <see cref="NativeMemoryManager"/>
	/// </summary>
	/// <returns>A <see cref="NativeMemoryPin">pin</see> pinning the underlying <see cref="NativeMemoryManager"/></returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly NativeMemoryPin Pin() => mNativeMemory.Pin();

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

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static implicit INativeMemory<ReadOnlyNativeMemory>.operator ReadOnlyNativeMemory(ReadOnlyNativeMemory nativeMemory) => nativeMemory;

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator ==(ReadOnlyNativeMemory left, ReadOnlyNativeMemory right) => left.Equals(right);

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator !=(ReadOnlyNativeMemory left, ReadOnlyNativeMemory right) => !(left == right);
}
