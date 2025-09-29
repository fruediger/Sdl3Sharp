using Sdl3Sharp.Timing;

namespace Sdl3Sharp.Utilities;

/// <summary>
/// Provides methods to generate and seed pseudo-random numbers
/// </summary>
public static partial class Random
{
	/// <summary>
	/// Generates a pseudo-random integer number less than a specified positive integer
	/// </summary>
	/// <param name="n">The number of possible outcomes. Should be positive.</param>
	/// <returns>A pseudo-random value in the range from inclusive <c>0</c> to inclusive <c><paramref name="n"/>-1</c>, if <paramref name="n"/> is greater than <c>0</c>; otherwise <c>0</c></returns>
	/// <remarks>
	/// <para>
	/// This method should be faster and of better quality than C's <c>rand() % n</c>. Odds are roughly 99.9% even for <paramref name="n"/> = 1 million.
	/// Evenness is better for smaller <paramref name="n"/>, and much worse as <paramref name="n"/> gets bigger.
	/// </para>
	/// <para>
	/// If you want to generate a pseudo-random number in the full 32-bit integer range, use <see cref="NextBits()"/> (see the examples given).
	/// </para>
	/// <para>
	/// If you want reproducible output, be sure to <see cref="SetSeed(ulong)">initialize the pseudo-random number generator with a seed</see> first (<see cref="SetSeed(ulong)"/>).
	/// </para> 
	/// <para>
	/// There are no guarantees as to the quality of the random sequence produced, and this should not be used for security (cryptography, passwords) or where money is on the line (loot-boxes, casinos).
	/// There are many alternatives with different characteristics available to meet more serious needs.
	/// </para>
	/// <para>
	/// All calls to <see cref="Next(int)"/>, <see cref="NextBits()"/>, <see cref="NextFloat()"/>, or <see cref="SetSeed(ulong)"/> (the "global" <see cref="Random"/> APIs) should be made from the same single thread.
	/// </para>
	/// </remarks>
	/// <example>
	/// <code>
	///		// simulate a D6 dice throw ('+ 1' moves the output range from 0..5 to 1..6)
	///		int d6 = Random.Next(6) + 1;
	///		
	///		// generate a pseudo-random number in the full 32-bit integer range (int.MinValue..int.MaxValue)
	///		// in contrast to Random.Next(int), this also generates negative values
	///		int fullIntRange = unchecked((int)Random.NextBits());
	/// </code>
	/// </example>
	public static int Next(int n) => SDL_rand(n);

	/// <summary>
	/// Generates a pseudo-random integer number less than a specified positive integer
	/// </summary>
	/// <param name="state">The current random number generator state</param>
	/// <param name="n">The number of possible outcomes. Should be positive.</param>
	/// <returns>A pseudo-random value in the range from inclusive <c>0</c> to inclusive <c><paramref name="n"/>-1</c>, if <paramref name="n"/> is greater than <c>0</c>; otherwise <c>0</c></returns>
	/// <remarks>
	/// <para>
	/// Instead of using a shared "global" state like <see cref="Next(int)"/>, you can use a "local" <paramref name="state"/> here.
	/// This enables you to have multiple random number generates with different <paramref name="state"/>s at the same time.
	/// </para>
	/// <para>
	/// This method should be faster and of better quality than C's <c>rand() % n</c>. Odds are roughly 99.9% even for <paramref name="n"/> = 1 million.
	/// Evenness is better for smaller <paramref name="n"/>, and much worse as <paramref name="n"/> gets bigger.
	/// </para>
	/// <para>
	/// If you want to generate a pseudo-random number in the full 32-bit integer range, use <see cref="NextBits(ref ulong)"/> (see the examples given).
	/// </para>
	/// <para>
	/// There are no guarantees as to the quality of the random sequence produced, and this should not be used for security (cryptography, passwords) or where money is on the line (loot-boxes, casinos).
	/// There are many alternatives with different characteristics available to meet more serious needs.
	/// </para>
	/// <para>
	/// This method is thread-safe, as long as the <paramref name="state"/> used is only shared in a thread-safe manner and no parallel concurrent calls to either <see cref="Next(ref ulong, int)"/>, <see cref="NextBits(ref ulong)"/>, or <see cref="NextFloat(ref ulong)"/> are made.
	/// </para>
	/// </remarks>
	/// <example>
	/// <code>
	///		// simulate a D6 dice throw ('+ 1' moves the output range from 0..5 to 1..6)
	///		int d6 = Random.Next(ref state, 6) + 1;
	///		
	///		// generate a pseudo-random number in the full 32-bit integer range (int.MinValue..int.MaxValue)
	///		// in contrast to Random.Next(ref ulong, int), this also generates negative values
	///		int fullIntRange = unchecked((int)Random.NextBits(ref state));
	/// </code>
	/// </example>
	public static int Next(ref ulong state, int n)
	{
		unsafe
		{
			fixed (ulong* statePtr = &state)
			{
				return SDL_rand_r(statePtr, n);
			}
		}
	}	

	/// <summary>
	/// Generates pseudo-random unsigned 32-bit integer
	/// </summary>
	/// <returns>A pseudo-random value in the range from inclusive <c><see cref="uint.MinValue"/></c> to inclusive <see cref="uint.MaxValue"/></returns>
	/// <remarks>
	/// <para>
	/// This method generates 32 pseudo-random bits and returns them packed as an <see cref="uint"/> value.
	/// </para>
	/// <para>
	/// If you want to generate a pseudo-random integer in a specified positive range, use <see cref="Next(int)"/> instead.
	/// </para>
	/// <para>
	/// If you want reproducible output, be sure to <see cref="SetSeed(ulong)">initialize the pseudo-random number generator with a seed</see> first (<see cref="SetSeed(ulong)"/>).
	/// </para> 
	/// <para>
	/// There are no guarantees as to the quality of the random sequence produced, and this should not be used for security (cryptography, passwords) or where money is on the line (loot-boxes, casinos).
	/// There are many alternatives with different characteristics available to meet more serious needs.
	/// </para>
	/// <para>
	/// All calls to <see cref="Next(int)"/>, <see cref="NextBits()"/>, <see cref="NextFloat()"/>, or <see cref="SetSeed(ulong)"/> (the "global" <see cref="Random"/> APIs) should be made from the same single thread.
	/// </para>
	/// </remarks>
	public static uint NextBits() => SDL_rand_bits();

	/// <summary>
	/// Generates pseudo-random unsigned 32-bit integer
	/// </summary>
	/// <param name="state">The current random number generator state</param>
	/// <returns>A pseudo-random value in the range from inclusive <c><see cref="uint.MinValue"/></c> to inclusive <see cref="uint.MaxValue"/></returns>
	/// <remarks> 
	/// <para>
	/// Instead of using a shared "global" state like <see cref="NextBits()"/>, you can use a "local" <paramref name="state"/> here.
	/// This enables you to have multiple random number generates with different <paramref name="state"/>s at the same time.
	/// </para>
	/// <para>
	/// This method generates 32 pseudo-random bits and returns them packed as an <see cref="uint"/> value.
	/// </para>
	/// <para>
	/// If you want to generate a pseudo-random integer in a specified positive range, use <see cref="Next(ref ulong, int)"/> instead.
	/// </para>
	/// <para>
	/// There are no guarantees as to the quality of the random sequence produced, and this should not be used for security (cryptography, passwords) or where money is on the line (loot-boxes, casinos).
	/// There are many alternatives with different characteristics available to meet more serious needs.
	/// </para>
	/// <para>
	/// This method is thread-safe, as long as the <paramref name="state"/> used is only shared in a thread-safe manner and no parallel concurrent calls to either <see cref="Next(ref ulong, int)"/>, <see cref="NextBits(ref ulong)"/>, or <see cref="NextFloat(ref ulong)"/> are made.
	/// </para>
	/// </remarks>
	public static uint NextBits(ref ulong state)
	{
		unsafe
		{
			fixed (ulong* statePtr = &state)
			{
				return SDL_rand_bits_r(statePtr);
			}
		}
	}

	/// <summary>
	/// Generates a uniform pseudo-random floating point number in the range from inclusive <c>0.0</c> to exclusive <c>1.0</c>
	/// </summary>
	/// <returns>A pseudo-random value in the range from inclusive <c>0.0</c> to exclusive <c>1.0</c></returns>
	/// <remarks>
	/// <para>
	/// If you want reproducible output, be sure to <see cref="SetSeed(ulong)">initialize the pseudo-random number generator with a seed</see> first (<see cref="SetSeed(ulong)"/>).
	/// </para> 
	/// <para>
	/// There are no guarantees as to the quality of the random sequence produced, and this should not be used for security (cryptography, passwords) or where money is on the line (loot-boxes, casinos).
	/// There are many alternatives with different characteristics available to meet more serious needs.
	/// </para>
	/// <para>
	/// All calls to <see cref="Next(int)"/>, <see cref="NextBits()"/>, <see cref="NextFloat()"/>, or <see cref="SetSeed(ulong)"/> (the "global" <see cref="Random"/> APIs) should be made from the same single thread.
	/// </para>
	/// </remarks>
	public static float NextFloat() => SDL_randf();

	/// <summary>
	/// Generates a uniform pseudo-random floating point number in the range from inclusive <c>0.0</c> to exclusive <c>1.0</c>
	/// </summary>
	/// <returns>A pseudo-random value in the range from inclusive <c>0.0</c> to exclusive <c>1.0</c></returns>
	/// <remarks>
	/// <para>
	/// Instead of using a shared "global" state like <see cref="NextFloat()"/>, you can use a "local" <paramref name="state"/> here.
	/// This enables you to have multiple random number generates with different <paramref name="state"/>s at the same time.
	/// </para>
	/// <para>
	/// There are no guarantees as to the quality of the random sequence produced, and this should not be used for security (cryptography, passwords) or where money is on the line (loot-boxes, casinos).
	/// There are many alternatives with different characteristics available to meet more serious needs.
	/// </para>
	/// <para>
	/// This method is thread-safe, as long as the <paramref name="state"/> used is only shared in a thread-safe manner and no parallel concurrent calls to either <see cref="Next(ref ulong, int)"/>, <see cref="NextBits(ref ulong)"/>, or <see cref="NextFloat(ref ulong)"/> are made.
	/// </para>
	/// </remarks>
	public static float NextFloat(ref ulong state)
	{
		unsafe
		{
			fixed (ulong* statePtr = &state)
			{
				return SDL_randf_r(statePtr);
			}
		}
	}

	/// <summary>
	/// Sets the seed of the pseudo-random number generator
	/// </summary>
	/// <param name="seed">The value to use as a random number seed, or <c>0</c> to use <see cref="Timer.PerformanceCounter"/> instead</param>
	/// <remarks>
	/// <para>
	/// Reusing the same value for the seed will cause <see cref="Next(int)"/>, <see cref="NextBits()"/>, and <see cref="NextFloat()"/> to repeat the same stream of pseudo-random numbers.
	/// </para>
	/// <para>
	/// All calls to <see cref="Next(int)"/>, <see cref="NextBits()"/>, <see cref="NextFloat()"/>, or <see cref="SetSeed(ulong)"/> (the "global" <see cref="Random"/> APIs) should be made from the same single thread.
	/// </para>
	/// </remarks>
	public static void SetSeed(ulong seed) => SDL_srand(seed);
}
