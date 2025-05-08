using Sdl3Sharp;
using Sdl3Sharp.Timing;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;

using var sdl = new Sdl(builder => builder.SetMetadata("Frame Counter", "0.1", appIdentifier: null));

return sdl.Run(new App(), args);

file class App : AppBase
{
	private volatile uint mCounter = 0;
	private Timer? mTimer;

	protected override AppResult OnInitialize(Sdl app, string[] args)
	{
		var counter = mCounter;
		var ticks = Timer.NanosecondTicks;

		mTimer = new(app, 250, (timer, interval) =>
		{
			(counter, ticks, var lastCounter, var lastTicks) = (mCounter, Timer.NanosecondTicks, counter, ticks);

			Log.Info($"{unchecked((1_000_000_000d * (counter - lastCounter)) / (ticks - lastTicks)).Unitize(out var prefix):0.000} {prefix}f/s");

			return interval;
		});

		return Continue;
	}

	protected override AppResult OnIterate(Sdl app)
	{
		return mCounter++ < uint.MaxValue ? Continue : Success;
	}

	protected override void OnQuit(Sdl app, AppResult result)
	{
		mTimer?.Dispose();
		mTimer = null;
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
