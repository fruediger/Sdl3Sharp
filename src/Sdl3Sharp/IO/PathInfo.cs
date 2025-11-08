using Sdl3Sharp.Internal;
using Sdl3Sharp.Timing;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;

namespace Sdl3Sharp.IO;

/// <summary>
/// Represents information about a file system entry
/// </summary>
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
[StructLayout(LayoutKind.Sequential)]
public readonly struct PathInfo : IFormattable, ISpanFormattable
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private string DebuggerDisplay => ToString(CultureInfo.InvariantCulture);

	private readonly PathType mType;
	private readonly ulong mSize;
	private readonly Time mCreateTime;
	private readonly Time mModifyTime;
	private readonly Time mAccessTime;

	internal PathInfo(PathType type, ulong size, Time creationTime, Time modificationTime, Time accessTime)
	{
		mType = type;
		mSize = size;
		mCreateTime = creationTime;
		mModifyTime = modificationTime;
		mAccessTime = accessTime;
	}

	/// <summary>
	/// Gets the last time the file system entry was read
	/// </summary>
	/// <value>
	/// The last time the file system entry was read
	/// </value>
	public readonly Time AccessTime { get => mAccessTime; }

	/// <summary>
	/// Gets the time the file system entry was created
	/// </summary>
	/// <value>
	/// The time the file system entry was created
	/// </value>
	public readonly Time CreationTime { get => mCreateTime; }

	/// <summary>
	/// Gets the last time the file system entry was modified
	/// </summary>
	/// <value>
	/// The last time the file system entry was modified
	/// </value>
	public readonly Time ModificationTime { get => mModifyTime; }

	/// <summary>
	/// Gets the size, in bytes, of file system entries that are files
	/// </summary>
	/// <value>
	/// The size, in bytes, of file system entries that are files, or <c>0</c>
	/// </value>
	public readonly ulong Length { get => mSize; }

	/// <summary>
	/// Gets the type of the file system entry
	/// </summary>
	/// <value>
	/// The type of the file system entry
	/// </value>
	public readonly PathType Type { get => mType; }

	/// <inheritdoc/>
	public readonly override string ToString() => ToString(format: default, formatProvider: default);

	/// <inheritdoc cref="IFormattable.ToString(string?, IFormatProvider?)"/>
	public readonly string ToString(IFormatProvider? formatProvider) => ToString(format: default, formatProvider);

	/// <inheritdoc cref="IFormattable.ToString(string?, IFormatProvider?)"/>
	public readonly string ToString(string? format) => ToString(format, formatProvider: default);

	/// <inheritdoc/>
	public readonly string ToString(string? format, IFormatProvider? formatProvider)
		=> $"{{ {nameof(Type)}: {mType.ToString(format)}, {
			nameof(Length)}: {mSize.ToString(format, formatProvider)}, {
			nameof(CreationTime)}: {mCreateTime.ToString(format, formatProvider)}, {
			nameof(ModificationTime)}: {mModifyTime.ToString(format, formatProvider)}, {
			nameof(AccessTime)}: {mAccessTime.ToString(format, formatProvider)} }}";

	/// <inheritdoc/>
	public readonly bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format = default, IFormatProvider? provider = default)
	{
		charsWritten = 0;

		return SpanFormat.TryWrite($"{{ {nameof(Type)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(mType, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite($", {nameof(Length)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(mSize, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite($", {nameof(CreationTime)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(mCreateTime, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite($", {nameof(ModificationTime)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(mModifyTime, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite($", {nameof(AccessTime)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(mAccessTime, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite(" }", ref destination, ref charsWritten);
	}
}
