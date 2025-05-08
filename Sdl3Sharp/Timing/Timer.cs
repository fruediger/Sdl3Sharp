using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sdl3Sharp.Timing;

[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
public sealed partial class Timer :
	IDisposable, Sdl.IDisposeReceiver, IEquatable<Timer>, IFormattable, ISpanFormattable
{
	private WeakReference<Sdl>? mSdlReference;
	private GCHandle mWrapperHandle;
	private uint mId;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private string DebuggerDisplay => ToString(formatProvider: CultureInfo.InvariantCulture);

	public Timer(Sdl sdl, uint millisecondsInterval, TimerMillisecondsCallback callback)
	{
		if (sdl is null)
		{
			failSdlArgumentNull();
		}

		if (millisecondsInterval is 0)
		{
			failMillisecondsIntervalArgumentIsZero();
		}

		if (callback is null)
		{
			failCallbackArgumentIsNull();
		}

		unsafe
		{
			mWrapperHandle = GCHandle.Alloc(new MillisecondsCallbackWrapper(this, callback), GCHandleType.Normal);

			try
			{
				mId = SDL_AddTimer(millisecondsInterval, &MillisecondsCallbackWrapper.TimerCallback, unchecked((void*)GCHandle.ToIntPtr(mWrapperHandle)));

				if (mId is 0)
				{
					failCouldNotAddTimer();
				}

				if (!sdl.TryRegisterDisposable(this))
				{
					failCouldNotRegisterWithSdl();
				}

				mSdlReference = new(sdl);
			}
			catch
			{
				mWrapperHandle.Free();
				mWrapperHandle = default;

				throw;
			}
		}

		[DoesNotReturn]
		static void failSdlArgumentNull() => throw new ArgumentNullException(nameof(sdl));

		[DoesNotReturn]
		static void failMillisecondsIntervalArgumentIsZero() => throw new ArgumentException($"The {nameof(millisecondsInterval)} argument must be greater than zero", nameof(millisecondsInterval));

		[DoesNotReturn]
		static void failCallbackArgumentIsNull() => throw new ArgumentNullException(nameof(callback));

		[DoesNotReturn]
		static void failCouldNotAddTimer() => throw new SdlException("Could not add the timer");

		[DoesNotReturn]
		static void failCouldNotRegisterWithSdl() => throw new InvalidOperationException($"Couldn't register the {nameof(Timer)} with the {nameof(Sdl)} instance");
	}

	public Timer(Sdl sdl, ulong nanosecondsInterval, TimerNanosecondsCallback callback)
	{
		if (sdl is null)
		{
			failSdlArgumentNull();
		}

		if (nanosecondsInterval is 0)
		{
			failNanosecondsIntervalArgumentIsZero();
		}

		if (callback is null)
		{
			failCallbackArgumentIsNull();
		}

		unsafe
		{
			mWrapperHandle = GCHandle.Alloc(new NanosecondsCallbackWrapper(this, callback), GCHandleType.Normal);

			try
			{
				mId = SDL_AddTimerNS(nanosecondsInterval, &NanosecondsCallbackWrapper.NSTimerCallback, unchecked((void*)GCHandle.ToIntPtr(mWrapperHandle)));

				if (mId is 0)
				{
					failCouldNotAddTimer();
				}

				if (!sdl.TryRegisterDisposable(this))
				{
					failCouldNotRegisterWithSdl();
				}

				mSdlReference = new(sdl);
			}
			catch
			{
				mWrapperHandle.Free();
				mWrapperHandle = default;

				throw;
			}
		}

		[DoesNotReturn]
		static void failSdlArgumentNull() => throw new ArgumentNullException(nameof(sdl));

		[DoesNotReturn]
		static void failNanosecondsIntervalArgumentIsZero() => throw new ArgumentException($"The {nameof(nanosecondsInterval)} argument must be greater than zero", nameof(nanosecondsInterval));

		[DoesNotReturn]
		static void failCallbackArgumentIsNull() => throw new ArgumentNullException(nameof(callback));

		[DoesNotReturn]
		static void failCouldNotAddTimer() => throw new SdlException("Could not add the timer");

		[DoesNotReturn]
		static void failCouldNotRegisterWithSdl() => throw new InvalidOperationException($"Couldn't register the {nameof(Timer)} with the {nameof(Sdl)} instance");
	}

	~Timer() => Dispose(deregister: true);

	public static ulong MillisecondTicks => SDL_GetTicks();

	public static ulong NanosecondTicks => SDL_GetTicksNS();

	public static ulong PerformanceCounter => SDL_GetPerformanceCounter();

	public static ulong PerformanceFrequency => SDL_GetPerformanceFrequency();

	public uint Id { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mId; }

	public void Dispose()
	{
		GC.SuppressFinalize(this);
		Dispose(deregister: true);
	}

	void Sdl.IDisposeReceiver.DisposeFromSdl(Sdl sdl)
	{
#pragma warning disable IDE0079
#pragma warning disable CA1816
		GC.SuppressFinalize(this);
#pragma warning restore CA1816
#pragma warning restore IDE0079
		Dispose(deregister: false);
	}

	private void Dispose(bool deregister)
	{
		if (mWrapperHandle is { IsAllocated: true, Target: CallbackWrapper wrapper })
		{
			try
			{
				wrapper.Dispose();
			}
			finally
			{
				mWrapperHandle.Free();
			}
		}

		mWrapperHandle = default;

		if (mId is not 0)
		{
			if (mSdlReference is not null)
			{
				if (deregister && mSdlReference.TryGetTarget(out var sdl))
				{
					sdl.TryDeregisterDisposable(this);
				}

				mSdlReference = null;

				SDL_RemoveTimer(mId);
			}

			mId = 0;
		}
	}

	public override bool Equals(object? obj) => Equals(obj as Timer);

	public bool Equals(Timer? other) => other is { mId: var otherId } && mId == otherId;

	public override int GetHashCode() => mId.GetHashCode();

	public override string ToString() => ToString(format: default, formatProvider: default);

	public string ToString(IFormatProvider? formatProvider) => ToString(format: default, formatProvider);

	public string ToString(string? format) => ToString(format, formatProvider: default);

	public string ToString(string? format, IFormatProvider? formatProvider)
		=> $"{{ {nameof(Id)}: {mId.ToString(format, formatProvider)} }}";

	public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format = default, IFormatProvider? provider = default)
	{
		static bool tryWriteSpan(ReadOnlySpan<char> value, ref Span<char> destination, ref int charsWritten)
		{
			var result = value.TryCopyTo(destination);

			if (result)
			{
				destination = destination[value.Length..];
				charsWritten += value.Length;
			}

			return result;
		}

		static bool tryWriteUInt(uint value, ref Span<char> destination, ref int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
		{
			var result = value.TryFormat(destination, out var tmp, format, provider);

			if (result)
			{
				destination = destination[tmp..];
				charsWritten += tmp;
			}

			return result;
		}

		charsWritten = 0;

		return tryWriteSpan($"{{ {nameof(Id)}: ", ref destination, ref charsWritten)
			&& tryWriteUInt(mId, ref destination, ref charsWritten, format, provider)
			&& tryWriteSpan(" }", ref destination, ref charsWritten);
	}
}
