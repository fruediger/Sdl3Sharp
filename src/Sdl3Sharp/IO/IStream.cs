using Sdl3Sharp.Utilities;

namespace Sdl3Sharp.IO;

/// <summary>
/// Represents a custom <see cref="Stream"/> implementation
/// </summary>
/// <remarks>
/// <para>
/// To create a <see cref="Stream"/> from a custom implementation use the <see cref="Stream(IStream)"/> constructor.
/// </para>
/// </remarks>
public interface IStream
{
	/// <summary>
	/// Gets the total length, in bytes, of the stream
	/// </summary>
	/// <value>
	/// The total length, in bytes, of the stream, or <c>-1</c> on failure
	/// </value>
	/// <seealso cref="Stream.Length"/>
	long Length { get; }

	/// <summary>
	/// Seeks within the stream
	/// </summary>
	/// <param name="offset">The offset to seek to</param>
	/// <param name="whence">The reference point for the seek operation</param>
	/// <returns>The absolute offset from the start of the stream after seeking, or <c>-1</c> on failure</returns>
	/// <seealso cref="Stream.TrySeek(long, StreamWhence, out long)"/>
	long Seek(long offset, StreamWhence whence);

	/// <summary>
	/// Reads into specified data from the stream
	/// </summary>
	/// <param name="data">The <see cref="NativeMemory">memory buffer</see> to read data into</param>
	/// <param name="status">
	/// The <see cref="StreamStatus"/> of the stream.
	/// The value of the referenced <see cref="StreamStatus"/> could change on failure and should not necessarily change on success.
	/// </param>
	/// <returns>The number of bytes read from the stream</returns>
	/// <seealso cref="Stream.TryRead(NativeMemory, out nuint)"/>
	nuint Read(NativeMemory data, ref StreamStatus status);

	/// <summary>
	/// Writes all specified data into the stream
	/// </summary>
	/// <param name="data">The <see cref="ReadOnlyNativeMemory">memory buffer</see> containing all the data to be written into the stream</param>
	/// <param name="status">
	/// The <see cref="StreamStatus"/> of the stream.
	/// The value of the referenced <see cref="StreamStatus"/> could change on failure and should not necessarily change on success.
	/// </param>
	/// <returns>The number of bytes read from the stream</returns>
	/// <seealso cref="Stream.TryWrite(ReadOnlyNativeMemory, out nuint)"/>
	nuint Write(ReadOnlyNativeMemory data, ref StreamStatus status);

	/// <summary>
	/// Flushes any buffered data in the stream
	/// </summary>
	/// <param name="status">
	/// The <see cref="StreamStatus"/> of the stream.
	/// The value of the referenced <see cref="StreamStatus"/> could change on failure and should not necessarily change on success.
	/// </param>
	/// <returns><c><see langword="true"/></c> if the flush was successful; otherwise, <c><see langword="false"/></c></returns>
	/// <seealso cref="Stream.TryFlush"/>
	bool Flush(ref StreamStatus status);

	/// <summary>
	/// Closes the stream and release associated resources
	/// </summary>
	/// <returns><c><see langword="true"/></c> if the stream was closed successfully; otherwise, <c><see langword="false"/></c></returns>
	/// <seealso cref="Stream.TryClose"/>
	bool Close();
}
