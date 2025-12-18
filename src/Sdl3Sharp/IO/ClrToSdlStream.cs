using Sdl3Sharp.Utilities;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.IO;

/// <summary>
/// Represents an SDL <see cref="Stream"/> that wraps a .NET (CLR) <see cref="System.IO.Stream"/>
/// </summary>
/// <remarks>
/// <para>
/// Please note that operations on this stream are potentially throwing.
/// They won't throw inevitably, but exceptions from the underlying CLR stream will propagate through this wrapper.
/// </para>
/// </remarks>
public sealed class ClrToSdlStream : Stream
{
	private System.IO.Stream? mClrStream;
	private readonly bool mLeaveOpen;

	/// <summary>
	/// Creates a new <see cref="ClrToSdlStream"/> instance wrapping the provided CLR <see cref="System.IO.Stream"/>
	/// </summary>
	/// <param name="clrStream">The CLR <see cref="System.IO.Stream"/> to wrap</param>
	/// <param name="leaveOpen">A value indicating whether to leave the underlying CLR stream open when this stream is disposed</param>
	/// <exception cref="ArgumentNullException"><paramref name="clrStream"/> is <c><see langword="null"/></c></exception>
	public ClrToSdlStream(System.IO.Stream clrStream, bool leaveOpen = false)
	{
		if (clrStream is null)
		{
			failClrStreamArgumentNull();
		}

		mClrStream = clrStream;
		mLeaveOpen = leaveOpen;

		[DoesNotReturn]
		static void failClrStreamArgumentNull() => throw new ArgumentNullException(nameof(clrStream));
	}

	/// <summary>
	/// Gets the underlying CLR <see cref="System.IO.Stream"/>
	/// </summary>
	/// <value>
	/// The underlying CLR <see cref="System.IO.Stream"/>
	/// </value>
	public System.IO.Stream? ClrStream => mClrStream;

	/// <inheritdoc/>
	protected override long LengthCore => mClrStream?.Length ?? -1;

	/// <inheritdoc/>
	protected override void Dispose(bool disposing, bool close)
	{
		try
		{
			CloseCore();
		}
		finally
		{
			base.Dispose(disposing, close);
		}
	}

	/// <inheritdoc/>
	protected override long SeekCore(long offset, StreamWhence whence)
	{
		if (mClrStream is not { CanSeek: true })
		{
			return -1;
		}

		return mClrStream.Seek(offset, Unsafe.BitCast<StreamWhence, System.IO.SeekOrigin>(whence));
	}

	/// <inheritdoc/>
	protected override nuint ReadCore(NativeMemory data, ref StreamStatus status)
	{
		if (mClrStream is null)
		{
			status = StreamStatus.Error;
			return 0;
		}

		if (!mClrStream.CanRead)
		{
			status = StreamStatus.WriteOnly;
			return 0;
		}

		if (!data.IsValid || data.IsEmpty)
		{
			return 0;
		}

		try
		{
			var bytesRead = (nuint)0;

			var byteData = (NativeMemory<byte>)data;
			for (var span = byteData.Span; !span.IsEmpty; byteData = byteData.Slice(unchecked((nuint)span.Length)), span = byteData.Span)
			{
				switch (mClrStream.Read(span))
				{
					case < 0:
						status = StreamStatus.Error;
						return bytesRead;

					case 0:
						status = StreamStatus.Eof;
						return bytesRead;

					case var read when read < span.Length:
						bytesRead += (nuint)read;
						return bytesRead;

					case var read:
						bytesRead += (nuint)read;
						break;
				}
			}

			return bytesRead;
		}
		catch
		{
			status = StreamStatus.Error;
			throw;
		}
	}

	/// <inheritdoc/>
	protected override nuint WriteCore(ReadOnlyNativeMemory data, ref StreamStatus status)
	{
		if (mClrStream is null)
		{
			status = StreamStatus.Error;
			return 0;
		}

		if (mClrStream.CanWrite is false)
		{
			status = StreamStatus.ReadOnly;
			return 0;
		}

		try
		{
			var bytesWritten = (nuint)0;

			var byteData = (ReadOnlyNativeMemory<byte>)data;
			for (var span = byteData.Span; !span.IsEmpty; byteData = byteData.Slice(unchecked((nuint)span.Length)), span = byteData.Span)
			{
				mClrStream.Write(span);
				bytesWritten += (nuint)span.Length;
			}

			return bytesWritten;
		}
		catch
		{
			status = StreamStatus.Error;
			throw;
		}
	}

	/// <inheritdoc/>
	protected override bool FlushCore(ref StreamStatus status)
	{
		if (mClrStream is null)
		{
			status = StreamStatus.Error;
			return false;
		}

		try
		{
			mClrStream.Flush();
			return true;
		}
		catch
		{
			status = StreamStatus.Error;
			throw;
		}
	}

	/// <inheritdoc/>
	protected override bool CloseCore()
	{
		if (mClrStream is not null)
		{
			if (!mLeaveOpen)
			{
				mClrStream.Dispose();
			}

			mClrStream = null;
		}

		return true;
	}
}
