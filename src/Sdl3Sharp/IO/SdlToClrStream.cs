using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Sdl3Sharp.IO;

/// <summary>
/// Represents a .NET (CLR) <see cref="System.IO.Stream"/> that wraps an SDL <see cref="Stream"/>
/// </summary>
public sealed class SdlToClrStream : System.IO.Stream
{
	private static void DetectCapabilities(Stream sdlStream, ref bool? canRead, ref bool? canSeek, ref bool? canWrite)
	{
		switch (sdlStream)
		{
			case DynamicMemoryStream or MemoryStream:
				canRead = true;
				canSeek = true;
				canWrite = true;
				break;

			case FileStream { ModeString: string modestring } when modestring.AsSpan() is var mode:
				canRead ??= mode.Contains("r", StringComparison.InvariantCultureIgnoreCase)
						 || mode.Contains("w+", StringComparison.InvariantCultureIgnoreCase)
						 || mode.Contains("a+", StringComparison.InvariantCultureIgnoreCase);
				canSeek ??= !mode.Contains("a", StringComparison.InvariantCultureIgnoreCase)
						 || mode.Contains("a+", StringComparison.InvariantCultureIgnoreCase);
				canWrite ??= mode.Contains("r+", StringComparison.InvariantCultureIgnoreCase)
						  || mode.Contains("w", StringComparison.InvariantCultureIgnoreCase)
						  || mode.Contains("a", StringComparison.InvariantCultureIgnoreCase);
				break;

			case ReadOnlyMemoryStream:
				canRead = true;
				canSeek = true;
				canWrite = false;
				break;
		}
	}

	private Stream? mSdlStream;
	private readonly bool mLeaveOpen;
	private bool mCanRead, mCanSeek, mCanWrite;

	/// <summary>
	/// Creates a new <see cref="SdlToClrStream"/> instance wrapping the provided SDL <see cref="Stream"/>
	/// </summary>
	/// <param name="sdlStream">The SDL <see cref="Stream"/> to wrap</param>
	/// <param name="leaveOpen">A value indicating whether to leave the underlying SDL stream open when this stream is disposed</param>
	/// <param name="canRead">An overwrite flag indicating whether the stream can be read from</param>
	/// <param name="canSeek">An overwrite flag indicating whether the stream can be seeked</param>
	/// <param name="canWrite">An overwrite flag indicating whether the stream can be written to</param>
	/// <remarks>
	/// <para>
	/// This constructor does its best to detect the capabilities of the provided SDL <see cref="Stream"/>.
	/// But in some cases it might not be possible to determine whether the stream can be read from, seeked or written to, or the detection might be wrong.
	/// In those cases, you can use the optional <paramref name="canRead"/>, <paramref name="canSeek"/> and <paramref name="canWrite"/> parameters to explicitly specify the capabilities of the stream.
	/// Note that, for example specifying <c><see langword="true"/></c> for <paramref name="canWrite"/> for an inherently read-only stream will not magically make it writable, but it will make this wrapper report that it is writable.
	/// </para>
	/// </remarks>
	/// <exception cref="ArgumentNullException"><paramref name="sdlStream"/> is <c><see langword="null"/></c></exception>
	public SdlToClrStream(Stream sdlStream, bool leaveOpen = false, bool? canRead = default, bool? canSeek = default, bool? canWrite = default)
	{
		if (sdlStream is null)
		{
			failSdlStreamArgumentNull();
		}

		mSdlStream = sdlStream;
		mLeaveOpen = leaveOpen;

		DetectCapabilities(sdlStream, ref canRead, ref canSeek, ref canWrite);

		// we are being optimistic here
		mCanRead = canRead ?? true;
		mCanSeek = canSeek ?? true;
		mCanWrite = canWrite ?? true;

		[DoesNotReturn]
		static void failSdlStreamArgumentNull() => throw new ArgumentNullException(nameof(sdlStream));
	}

	/// <inheritdoc/>
	public override bool CanRead => mCanRead;

	/// <inheritdoc/>
	public override bool CanSeek => mCanSeek;

	/// <inheritdoc/>
	public override bool CanWrite => mCanWrite;

	/// <inheritdoc/>
	public override long Length => ValidatedSdlStream.Length;

	/// <inheritdoc/>
	public override long Position
	{
		get => ValidatedSdlStream.Position;
		set
		{
			if (!ValidatedSdlStream.TrySeek(value, StreamWhence.Set, out _))
			{
				// we are so bold to say the stream isn't seekable at all
				mCanSeek = false;

				failCouldNotSeekSdlStream();
			}

			[DoesNotReturn]
			static void failCouldNotSeekSdlStream() => throw new System.IO.IOException("Could not seek the underlying SDL stream to the requested position");
		}
	}

	/// <summary>
	/// Gets the underlying SDL <see cref="Stream"/>
	/// </summary>
	/// <value>
	/// The underlying SDL <see cref="Stream"/>
	/// </value>
	public Stream? SdlStream => mSdlStream;

	[MemberNotNull(nameof(mSdlStream))]
	private Stream ValidatedSdlStream
	{
		get
		{
			if (mSdlStream is null)
			{
				failDisposed();
			}

			return mSdlStream;

			[DoesNotReturn]
			static void failDisposed() => throw new ObjectDisposedException(nameof(SdlToClrStream));
		}
	}

	/// <inheritdoc/>
	protected override void Dispose(bool disposing)
	{
		if (mSdlStream is not null)
		{
			try
			{
				if (!mLeaveOpen)
				{
					mSdlStream.Dispose();
				}

				mSdlStream = null;
			}
			finally
			{
				base.Dispose(disposing);
			}
		}
	}

	/// <inheritdoc/>
	public override void Flush()
	{
		if (!ValidatedSdlStream.TryFlush())
		{
			failCouldNotFlushSdlStream();
		}

		[DoesNotReturn]
		static void failCouldNotFlushSdlStream() => throw new System.IO.IOException("Could not flush the underlying SDL stream");
	}

	/// <inheritdoc/>
	public override int Read(byte[] buffer, int offset, int count)
		=> Read(buffer.AsSpan(offset, count));

	/// <inheritdoc/>
	public override int Read(Span<byte> buffer)
	{
		if (buffer.IsEmpty)
		{
			return 0;
		}

		var sdlStream = ValidatedSdlStream;
		if (!sdlStream.TryRead(buffer, out var bytesRead))
		{
			switch (sdlStream.Status)
			{
				case StreamStatus.Error:
					failCouldNotReadSdlStream();
					break;

				case StreamStatus.WriteOnly:
					mCanRead = false;
					failSdlStreamWriteOnly();
					break;
			}
		}

		// it's okay to return bytesRead here, even in case of an error, as it would be just 0 in that case
		return bytesRead;

		[DoesNotReturn]
		static void failCouldNotReadSdlStream() => throw new System.IO.IOException("Could not read from the underlying SDL stream because there was an I/O error");

		[DoesNotReturn]
		static void failSdlStreamWriteOnly() => throw new NotSupportedException("The underlying SDL stream is write-only");
	}

	/// <inheritdoc/>
	public override long Seek(long offset, System.IO.SeekOrigin origin)
	{
		if (!ValidatedSdlStream.TrySeek(
			offset,
			Unsafe.BitCast<System.IO.SeekOrigin, StreamWhence>(origin),
			out var absoluteOffset
		))
		{
			failCouldNotSeekSdlStream();
		}

		return absoluteOffset;

		[DoesNotReturn]
		static void failCouldNotSeekSdlStream() => throw new System.IO.IOException("Could not seek the underlying SDL stream to the requested position");
	}

	/// <inheritdoc/>
	[DoesNotReturn]
	public override void SetLength(long value) => throw new NotSupportedException("SDL streams do not support changing their length");

	/// <inheritdoc/>
	public override void Write(byte[] buffer, int offset, int count)
		=> Write(buffer.AsSpan(offset, count));

	/// <inheritdoc/>
	public override void Write(ReadOnlySpan<byte> buffer)
	{
		var sdlStream = ValidatedSdlStream;

		while (!buffer.IsEmpty)
		{
			if (!sdlStream.TryWrite(buffer, out var bytesWritten))
			{
				switch (sdlStream.Status)
				{
					case StreamStatus.Ready:
						break;

					case StreamStatus.Error:
						failCouldNotWritSdlStream();
						break;

					case StreamStatus.NotReady:
						{
							var spinWait = new SpinWait();
							while (true)
							{
								spinWait.SpinOnce();

								var status = sdlStream.Status;
								switch(status)
								{
									case StreamStatus.NotReady: continue;

									case StreamStatus.Error:
										failCouldNotWritSdlStream();
										break;

									case StreamStatus.ReadOnly:
										mCanWrite = false;
										failSdlStreamReadOnly();
										break;

									case not StreamStatus.Ready:
										failUnexpectedSdlStreamStatus(status);
										break;
								}

								break;
							}
						}
						break;

					case StreamStatus.ReadOnly:
						mCanWrite = false;
						failSdlStreamReadOnly();
						break;
				}
			}

			buffer = buffer[bytesWritten..];
		}

		[DoesNotReturn]
		static void failCouldNotWritSdlStream() => throw new System.IO.IOException("Could not write to the underlying SDL stream because there was an I/O error");

		[DoesNotReturn]
		static void failSdlStreamReadOnly() => throw new NotSupportedException("The underlying SDL stream is read-only");

		[DoesNotReturn]
		static void failUnexpectedSdlStreamStatus(StreamStatus status) => throw new System.IO.IOException($"Encountered an unexpected status of the underlying SDL stream while writing: {status}");
	}
}