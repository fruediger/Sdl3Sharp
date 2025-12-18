using Sdl3Sharp.Ffi;
using Sdl3Sharp.Utilities;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Sdl3Sharp.IO;

/// <summary>
/// Provides extension methods for <see cref="Stream"/>
/// </summary>
public static class StreamExtensions
{
	/// <summary>
	/// Creates a .NET (CLR) <see cref="System.IO.Stream"/> wrapping the provided SDL <see cref="Stream"/>
	/// </summary>
	/// <param name="sdlStream">The SDL <see cref="Stream"/> to wrap</param>
	/// <param name="leaveOpen">A value indicating whether to leave the underlying SDL stream open when the wrapper is disposed</param>
	/// <param name="canRead">An overwrite flag indicating whether the stream can be read from</param>
	/// <param name="canSeek">An overwrite flag indicating whether the stream can be seeked</param>
	/// <param name="canWrite">An overwrite flag indicating whether the stream can be written to</param>
	/// <returns>A <see cref="SdlToClrStream"/> wrapper around the SDL <see cref="Stream"/></returns>
	/// <remarks>
	/// <para>
	/// The <see cref="SdlToClrStream"/> constructor does its best to detect the capabilities of the provided SDL <see cref="Stream"/>.
	/// But in some cases it might not be possible to determine whether the stream can be read from, seeked or written to, or the detection might be wrong.
	/// In those cases, you can use the optional <paramref name="canRead"/>, <paramref name="canSeek"/> and <paramref name="canWrite"/> parameters to explicitly specify the capabilities of the stream.
	/// Note that, for example specifying <c><see langword="true"/></c> for <paramref name="canWrite"/> for an inherently read-only stream will not magically make it writable, but it will make this wrapper report that it is writable.
	/// </para>
	/// <para>
	/// Please remember to <see cref="System.IO.Stream.Dispose()">dispose</see> the returned <see cref="SdlToClrStream"/> instance when you are done using it to free up any resources.
	/// </para>
	/// </remarks>
	/// <exception cref="ArgumentNullException"><paramref name="sdlStream"/> is <c><see langword="null"/></c></exception>
	public static SdlToClrStream ToClrStream(this Stream sdlStream, bool leaveOpen = false, bool? canRead = default, bool? canSeek = default, bool? canWrite = default)
		=> new(sdlStream, leaveOpen, canRead, canSeek, canWrite);

	/// <summary>
	/// Creates an SDL <see cref="Stream"/> wrapping the provided .NET (CLR) <see cref="System.IO.Stream"/>
	/// </summary>
	/// <param name="clrStream">The CLR <see cref="System.IO.Stream"/> to wrap</param>
	/// <param name="leaveOpen">A value indicating whether to leave the underlying CLR stream open when the wrapper is disposed</param>
	/// <returns>A <see cref="ClrToSdlStream"/> wrapper around the CLR <see cref="System.IO.Stream"/></returns>
	/// <remarks>
	/// <para>
	/// Please remember to <see cref="Stream.Dispose()">dispose</see> the returned <see cref="ClrToSdlStream"/> instance when you are done using it to free up any resources.
	/// </para>
	/// </remarks>
	/// <exception cref="ArgumentNullException"><paramref name="clrStream"/> is <c><see langword="null"/></c></exception>
	public static Stream ToSdlStream(this System.IO.Stream clrStream, bool leaveOpen = false)
		=> new ClrToSdlStream(clrStream, leaveOpen);

	// this overload exist just for completeness's sake, 'TimeSpan' is the way more csharp-y way of communicating timeout values
	/// <param name="timeout">
	/// The time to wait for the <paramref name="destination"/> stream to become ready or fail if exceeded.
	/// Fractional nanoseconds will get <see cref="System.Math.Ceiling(double)">rounded to next greater</see> whole nanosecond value.
	/// Must not be negative and must represent a <see cref="System.Math.Ceiling(double)">rounded</see> <see cref="TimeSpan.TotalNanoseconds">nanoseconds value</see> that is less or equal to <see cref="ulong.MaxValue"/>.
	/// If <c><see langword="null"/></c>, waits indefinitely. If <c><see cref="TimeSpan.Zero"/></c>, fails immediately.
	/// </param>
	/// <exception cref="ArgumentOutOfRangeException">
	/// <paramref name="timeout"/> is negative or the <see cref="System.Math.Ceiling(double)">rounded</see> <see cref="TimeSpan.TotalNanoseconds">nanosecond value</see> <paramref name="timeout"/> represents is greater than <see cref="ulong.MaxValue"/>
	/// </exception>
	/// <exception cref="InvalidOperationException">The internal timer SDL uses may have been reset or overflowed during the execution. Only happens when <paramref name="timeout"/> is neither <c><see langword="null"/></c> nor <c><see cref="TimeSpan.Zero"/></c>.</exception>
	/// <inheritdoc cref="TryCopyTo(Stream, Stream, out nuint, out nuint, ulong?, nuint)"/>	
#pragma warning disable CS1573 // we're getting the missing parameter descriptions from 'inheritdoc'
	public static bool TryCopyTo(this Stream source, Stream destination, out nuint bytesRead, out nuint bytesWritten, TimeSpan? timeout, nuint bufferSize = 4096)
#pragma warning restore CS1573
	{
		return TryCopyTo(source, destination, out bytesRead, out bytesWritten,
			nanosecondsTimeout: timeout switch
			{
				null => null,
				TimeSpan t when t == TimeSpan.Zero => 0,
				TimeSpan t when t > TimeSpan.Zero && System.Math.Ceiling(t.TotalNanoseconds) is var ns && ns <= ulong.MaxValue => (ulong)ns,
				_ => failTimeoutArgumentOutOfRange()
			},
			bufferSize
		);

		[DoesNotReturn]
		static ulong? failTimeoutArgumentOutOfRange() => throw new ArgumentOutOfRangeException(nameof(timeout));
	}

	// this overload exist just for completeness's sake, most SDL APIs provide an additional way to specify a 'uint' milliseconds time, if there's a 'ulong' nanoseconds time API
	/// <param name="millisecondsTimeout">
	/// The number of milliseconds to wait for the <paramref name="destination"/> stream to become ready or fail if exceeded.
	/// If <c><see langword="null"/></c>, waits indefinitely. If <c>0</c>, fails immediately.
	/// </param>
	/// <exception cref="InvalidOperationException">The internal timer SDL uses may have been reset or overflowed during the execution. Only happens when <paramref name="millisecondsTimeout"/> is neither <c><see langword="null"/></c> nor <c>0</c>.</exception>
	/// <inheritdoc cref="TryCopyTo(Stream, Stream, out nuint, out nuint, ulong?, nuint)"/>
#pragma warning disable CS1573 // we're getting the missing parameter descriptions from 'inheritdoc'
	public static bool TryCopyTo(this Stream source, Stream destination, out nuint bytesRead, out nuint bytesWritten, uint? millisecondsTimeout, nuint bufferSize = 4096)
#pragma warning restore CS1573 
		=> TryCopyTo(source, destination, out bytesRead, out bytesWritten, nanosecondsTimeout: millisecondsTimeout * (ulong)Timing.Time.NanosecondsPerMillisecond, bufferSize);

	/// <summary>
	/// Tries to copy all data from the source stream into a specified destination stream
	/// </summary>
	/// <param name="source">The source stream to copy data into</param>
	/// <param name="destination">The destination stream to copy data into</param>
	/// <param name="bytesRead">The number of bytes read from the <paramref name="source"/> stream</param>
	/// <param name="bytesWritten">The number of bytes written to the <paramref name="destination"/> stream</param> 
	/// <param name="nanosecondsTimeout">
	/// The number of nanoseconds to wait for the <paramref name="destination"/> stream to become ready or fail if exceeded.
	/// If <c><see langword="null"/></c>, waits indefinitely. If <c>0</c>, fails immediately. Defaults to <c>0</c>.
	/// </param>
	/// <param name="bufferSize">The size of the buffer to use for copying. Defaults to <c>4096</c>.</param>
	/// <returns><c><see langword="true"/></c>, if the data from the <paramref name="source"/> stream was successfully copied to the <paramref name="destination"/> stream; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method tries to copy as many bytes as possible before returning.
	/// <paramref name="bytesRead"/> will be equal to the actual number of bytes read from the stream, while <paramref name="bytesWritten"/> will be equal to the actual number of bytes written to the <paramref name="destination"/> stream, even in the case when not all data could be copied.
	/// You should check the stream's <see cref="Status"/> property as well as the <paramref name="destination"/>'s <see cref="Status"/> property to determine whether a shortened copy operation is recoverable or not.
	/// </para>
	/// <para>
	/// You may want to specify a timeout value for the time to wait for the <paramref name="destination"/> stream to become ready in asynchronous I/O scenarios.
	/// You can specify <c>0</c> to fail immediately, if the <paramref name="destination"/> stream is not ready yet.
	/// You can also specify <c><see langword="null"/></c> to wait indefinitely for the <paramref name="destination"/> stream to become ready.
	/// If the <paramref name="destination"/> stream's <see cref="Stream.Status"/> property is any other value than <see cref="StreamStatus.Ready"/> or <see cref="StreamStatus.NotReady"/>, this methods still fails and returns <c><see langword="false"/></c>.
	/// Please notice that the specified timeout value is only for waiting on the <paramref name="destination"/> stream; if the <paramref name="source"/> stream cannot read in time, this method still fails and returns <c><see langword="false"/></c>
	/// (this behavior is identical to any other of the <c>TryRead*</c> methods of a <see cref="Stream"/>, except this method still returns <c><see langword="true"/></c> when the <paramref name="source"/> stream's <see cref="Stream.Status"/> property is <see cref="StreamStatus.Eof"/> and all data was successfully written).
	/// </para>
	/// <para>
	/// This method is not threadsafe.
	/// </para>
	/// </remarks>
	/// <exception cref="ArgumentNullException"><paramref name="destination"/> is <c><see langword="null"/></c></exception>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="bufferSize"/> is not greater than <c>0</c></exception>
	/// <exception cref="InvalidOperationException">The internal timer SDL uses may have been reset or overflowed during the execution. Only happens when <paramref name="nanosecondsTimeout"/> is neither <c><see langword="null"/></c> nor <c>0</c>.</exception>
	[OverloadResolutionPriority(1)] // we actually want to take this overload if an user just provides a 0, null, etc. literal for nanosecondsTimeout
	public static bool TryCopyTo(this Stream source, Stream destination, out nuint bytesRead, out nuint bytesWritten, ulong? nanosecondsTimeout = 0, nuint bufferSize = 4096)
	{
		if (source is null)
		{
			failSourceArgumentNull();
		}

		if (destination is null)
		{
			failDestinationArgumentNull();
		}

		if (bufferSize is <= 0)
		{
			failBufferSizeArgumentOutOfRange();
		}

		bytesRead = 0;
		bytesWritten = 0;

		// wait for the destination to become ready or timeout before we allocate the needed resources for the first time
		if (!tryWaitForStream(destination, nanosecondsTimeout))
		{
			return false;
		}

		if (!Utilities.NativeMemory.TryMalloc(bufferSize, out var memoryManager))
		{
			return false;
		}

		using (memoryManager)
		{
			var buffer = memoryManager.Memory;
			var byteBuffer = (NativeMemory<byte>)buffer;

			while (source.TryRead(buffer, out var bytesReadTmp))
			{
				bytesRead += bytesReadTmp;

				var workingBuffer = byteBuffer.Slice(0, bytesReadTmp);

				while (true)
				{
					var writeResult = destination.TryWrite(workingBuffer, out var bytesWrittenTmp);

					bytesWritten += bytesWrittenTmp;

					if (writeResult)
					{
						break;
					}

					if (!tryWaitForStream(destination, nanosecondsTimeout))
					{
						return false;
					}

					workingBuffer = workingBuffer.Slice(bytesWrittenTmp);
				}
			}
		}

		return source.Status is StreamStatus.Eof;

		static bool tryWaitForStream(Stream stream, ulong? nanosecondsTimeout)
		{
			var status = stream.Status;

			if (status is StreamStatus.NotReady)
			{
				if (nanosecondsTimeout is 0)
				{
					return false;
				}

				var spinWait = new SpinWait();				
				var startTime = nanosecondsTimeout is not null ? Timing.Timer.NanosecondTicks : 0ul;
				var previousTime = startTime;

				while (true)
				{
					spinWait.SpinOnce();

					status = stream.Status;

					if (status is not StreamStatus.NotReady)
					{
						break;
					}

					if (nanosecondsTimeout is ulong timeout)
					{
						var currentTime = Timing.Timer.NanosecondTicks;

						if (currentTime < previousTime)
						{
							failTimerReset();
						}

						if (unchecked(currentTime - startTime) >= timeout)
						{
							// Timeout
							return false;
						}

						previousTime = currentTime;
					}
				}
			}

			return status is StreamStatus.Ready;
		}
		
		[DoesNotReturn]
		static void failSourceArgumentNull() => throw new ArgumentNullException(nameof(source));

		[DoesNotReturn]
		static void failDestinationArgumentNull() => throw new ArgumentNullException(nameof(destination));

		[DoesNotReturn]
		static void failBufferSizeArgumentOutOfRange() => throw new ArgumentOutOfRangeException(nameof(bufferSize));

		[DoesNotReturn]
		static void failTimerReset() => throw new InvalidOperationException("The internal timer SDL uses may have been reset or overflowed during the execution");
	}
}
