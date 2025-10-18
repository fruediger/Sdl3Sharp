using Sdl3Sharp;
using Sdl3Sharp.Timing;
using Sdl3Sharp.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;

NativeMemory.TrySetMemoryFunctions(new LoggedMemoryFunctions());

Hint.MainCallbackRate.TrySetValue("120");

Log.Message(LogCategory.Application, LogPriority.Info, "%s %s from SDL version %i.%i.%i", "Hello", "World", Sdl.Version.Major, Sdl.Version.Minor, Sdl.Version.Micro);

MessageBox.TryShowSimple(MessageBoxFlags.Information, "Hello World", $"...from SDL version {Sdl.Version}");

using var sdl = new Sdl(builder => builder
	.SetMetadata("Frame Counter", "0.1", appIdentifier: null)
	.InitializeSubSystems([SubSystem.Video, SubSystem.Events, SubSystem.Audio])
);

return sdl.Run(new App(), args);

file class App : AppBase
{
	private volatile uint mCounter = 0;
	private Timer? mTimer;

	protected override AppResult OnInitialize(Sdl sdl, string[] args)
	{
		var counter = mCounter;
		var ticks = Timer.NanosecondTicks;

		mTimer = new(sdl, 1000, (timer, interval) =>
		{
			(counter, ticks, var lastCounter, var lastTicks) = (mCounter, Timer.NanosecondTicks, counter, ticks);

			Log.Info($"{unchecked((1_000_000_000d * (counter - lastCounter)) / (ticks - lastTicks)).Unitize(out var prefix):0.000} {prefix}f/s");

			return interval;
		});

		return Continue;
	}

	protected override AppResult OnIterate(Sdl sdl)
	{
		return mCounter++ < uint.MaxValue ? Continue : Success;
	}

	protected override void OnQuit(Sdl sdl, AppResult result)
	{
		mTimer?.Dispose();
		mTimer = null;
	}
}

file sealed class LoggedMemoryFunctions : INativeMemoryFunctions
{
	private readonly INativeMemoryFunctions mPreviousMemoryFunctions = NativeMemory.GetMemoryFunctions();
	private readonly Dictionary<IntPtr, nuint> mAllocations = [];

	public unsafe void* Calloc(nuint elementCount, nuint elementSize)
	{
		var result = mPreviousMemoryFunctions.Calloc(elementCount, elementSize);
		
		var size = unchecked(elementCount * elementSize);

		_ = Console.Out.WriteLineAsync($"\x1b[2m{nameof(Calloc)}({nameof(elementCount)}: {elementCount}, {nameof(elementSize)}: {elementSize}) returned {(result is not null ? $"0x{unchecked((IntPtr)result).ToString(Unsafe.SizeOf<IntPtr>() is <= 4 ? "X8" : "X16")} ({unchecked((decimal)size).Unitize(out var prefix, scale: 1024):0.###}{(prefix is not null ? $"{prefix}i" : "")}b allocated)": "null")}\x1b[0m");

		if (result is not null)
		{
			mAllocations[unchecked((IntPtr)result)] = size;
		}

		return result;
	}

	public unsafe void Free(void* memory)
	{	
		mPreviousMemoryFunctions.Free(memory);

		_ = Console.Out.WriteLineAsync($"\x1b[2m{nameof(Free)}({nameof(memory)}: {(memory is not null ? $"0x{unchecked((IntPtr)memory).ToString(Unsafe.SizeOf<IntPtr>() is <= 4 ? "X8" : "X16")}": "null")}){(mAllocations.TryGetValue(unchecked((IntPtr)memory), out var size) ? $" ({unchecked((decimal)size).Unitize(out var prefix, scale: 1024):0.###}{(prefix is not null ? $"{prefix}i" : "")}b freed)" : "")}\x1b[0m");

		mAllocations.Remove(unchecked((IntPtr)memory));
	}

	public unsafe void* Malloc(nuint size)
	{
		var result = mPreviousMemoryFunctions.Malloc(size);

		_ = Console.Out.WriteLineAsync($"\x1b[2m{nameof(Malloc)}({nameof(size)}: {size}) returned {(result is not null ? $"0x{unchecked((IntPtr)result).ToString(Unsafe.SizeOf<IntPtr>() is <= 4 ? "X8" : "X16")} ({unchecked((decimal)size).Unitize(out var prefix, scale: 1024):0.###}{(prefix is not null ? $"{prefix}i" : "")}b allocated)": "null")}\x1b[0m");

		if (result is not null)
		{
			mAllocations[unchecked((IntPtr)result)] = size;
		}

		return result;
	}

	public unsafe void* Realloc(void* memory, nuint newSize)
	{
		if (!mAllocations.TryGetValue(unchecked((IntPtr)memory), out var size))
		{
			size = 0;
		}

		var result = mPreviousMemoryFunctions.Realloc(memory, newSize);

		_ = Console.Out.WriteLineAsync($"\x1b[2m{nameof(Realloc)}({nameof(memory)}: {(memory is not null ? $"0x{unchecked((IntPtr)memory).ToString(Unsafe.SizeOf<IntPtr>() is <= 4 ? "X8" : "X16")}" : "null")}, {nameof(newSize)}: {newSize}) returned {(result is not null ? $"0x{unchecked((IntPtr)result).ToString(Unsafe.SizeOf<IntPtr>() is <= 4 ? "X8" : "X16")} ({unchecked((decimal)(newSize > size is var allocated && allocated ? unchecked(newSize - size) : unchecked(size - newSize))).Unitize(out var prefix, scale: 1024):0.###}{(prefix is not null ? $"{prefix}i" : "")}b {(allocated ? "allocated" : "freed")})": "null")}\x1b[0m");

		if (result is not null)
		{
			mAllocations[unchecked((IntPtr)result)] = newSize;
		}

		mAllocations.Remove(unchecked((IntPtr)memory));

		return result;
	}
}

file static class Binary<T>
	where T : INumberBase<T>
{
	public interface IValued
	{
		static abstract T Value { get; }
	}

	public abstract class Recursive<TSelf>
		where TSelf : Recursive<TSelf>, IValued
	{
		public sealed class _0 : Recursive<_0>, IValued
		{
			public static T Value { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => TSelf.Value * (T.One + T.One) + T.Zero; }

			private _0() { }
		}

		public sealed class _1 : Recursive<_1>, IValued
		{
			public static T Value { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => TSelf.Value * (T.One + T.One) + T.One; }

			private _1() { }
		}

		private protected Recursive() { }
	}

	public sealed class _0 : Recursive<_0>, IValued
	{
		public static T Value { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => T.Zero; }

		private _0() { }
	}

	public sealed class _1 : Recursive<_1>, IValued
	{
		public static T Value { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => T.One; }

		private _1() { }
	}
}

file static class ReferenceExtensions
{
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static T Unitize<T>(this T value, out char? prefix, T? scale = default, T? upscaleThreshold = default, T? downscaleThreshold = default, ReadOnlySpan<char> prefixes = default)
		where T : class, INumber<T>
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		static T defaultScale() => Binary<T>._0._1._1._1._1._1._0._1._0._0._0.Value; /* 0b011_1110_1000 = 1_000 */

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		static T defaultUpscaleThreshold() => Binary<T>._1._0._0._0._1._0._0._1._1._0._0.Value; /* 0b100_0100_1100 = 1_100 */

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		static T defaultDownscaleThreshold() => Binary<T>._0._1._1._1._0._0._0._0._1._0._0.Value; /* 0b011_1000_0100 = 900 */

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		static ReadOnlySpan<char> defaultPrefixes() => ['q', 'r', 'y', 'z', 'a', 'f', 'p', 'n', 'µ', 'm', /* mid-point */ 'k', 'M', 'G', 'T', 'P', 'E', 'Z', 'Y', 'R', 'Q'];

		if (prefixes.IsEmpty)
		{
			prefixes = defaultPrefixes();
		}

		var (prefixIndex, rem) = int.DivRem(prefixes.Length, 2);

		if (rem is not 0)
		{
			failPrefixesNotEvenNumbered();
		}

		if(!T.IsZero(value))
		{		
			scale ??= defaultScale();
			downscaleThreshold ??= defaultDownscaleThreshold();		

			if (value > downscaleThreshold)
			{
				value /= scale;

				while (value > downscaleThreshold && prefixIndex + 1 is var nextPrefixIndex && nextPrefixIndex < prefixes.Length)
				{				
					value /= scale;
					prefixIndex = nextPrefixIndex;
				}

				prefix = prefixes[prefixIndex];
			}
			else
			{
				upscaleThreshold ??= defaultUpscaleThreshold();

				var nextValue = value * scale;
				if (nextValue < upscaleThreshold)
				{
					value = nextValue;
					--prefixIndex;
				
					while(prefixIndex - 1 is var nextPrefixIndex && nextPrefixIndex >= 0 && (nextValue = value * scale) < upscaleThreshold)
					{					
						value = nextValue;
						prefixIndex = nextPrefixIndex;
					}

					prefix = prefixes[prefixIndex];
				}
				else
				{
					prefix = null;
				}
			}
		}
		else
		{
			prefix = null;
		}

		return value;

		[DoesNotReturn]
		static void failPrefixesNotEvenNumbered() => throw new System.ArgumentException("The number of prefixes must be an even number.", nameof(prefixes));
	}
}

file static class ValueExtensions
{
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static T Unitize<T>(this T value, out char? prefix, T? scale = default, T? upscaleThreshold = default, T? downscaleThreshold = default, ReadOnlySpan<char> prefixes = default)
		where T : struct, INumber<T>
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		static T defaultScale() => Binary<T>._0._1._1._1._1._1._0._1._0._0._0.Value; /* 0b011_1110_1000 = 1_000 */

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		static T defaultUpscaleThreshold() => Binary<T>._1._0._0._0._1._0._0._1._1._0._0.Value; /* 0b100_0100_1100 = 1_100 */

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		static T defaultDownscaleThreshold() => Binary<T>._0._1._1._1._0._0._0._0._1._0._0.Value; /* 0b011_1000_0100 = 900 */

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		static ReadOnlySpan<char> defaultPrefixes() => ['q', 'r', 'y', 'z', 'a', 'f', 'p', 'n', 'µ', 'm', /* mid-point */ 'k', 'M', 'G', 'T', 'P', 'E', 'Z', 'Y', 'R', 'Q'];

		if (prefixes.IsEmpty)
		{
			prefixes = defaultPrefixes();
		}

		var (prefixIndex, rem) = int.DivRem(prefixes.Length, 2);

		if (rem is not 0)
		{
			failPrefixesNotEvenNumbered();
		}

		if (!T.IsZero(value))
		{
			var nonNullScale = scale ?? defaultScale();
			var nonNullDownscaleThreshold = downscaleThreshold ?? defaultDownscaleThreshold();

			if (value > nonNullDownscaleThreshold)
			{
				value /= nonNullScale;

				while (value > nonNullDownscaleThreshold && prefixIndex + 1 is var nextPrefixIndex && nextPrefixIndex < prefixes.Length)
				{				
					value /= nonNullScale;
					prefixIndex = nextPrefixIndex;
				}

				prefix = prefixes[prefixIndex];
			}
			else
			{
				var nonNullUpscaleThreshold = upscaleThreshold ?? defaultUpscaleThreshold();

				var nextValue = value * nonNullScale;
				if (nextValue < nonNullUpscaleThreshold)
				{
					value = nextValue;
					--prefixIndex;
				
					while(prefixIndex - 1 is var nextPrefixIndex && nextPrefixIndex >= 0 && (nextValue = value * nonNullScale) < nonNullUpscaleThreshold)
					{					
						value = nextValue;
						prefixIndex = nextPrefixIndex;
					}

					prefix = prefixes[prefixIndex];
				}
				else
				{
					prefix = null;
				}
			}
		}
		else
		{
			prefix = null;
		}

		return value;

		[DoesNotReturn]
		static void failPrefixesNotEvenNumbered() => throw new System.ArgumentException("The number of prefixes must be an even number.", nameof(prefixes));
	}
}
