using Sdl3Sharp.Interop;
using Sdl3Sharp.SourceGeneration;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Utilities;

partial class Random
{
	/// <summary>
	/// Generate a pseudo-random number less than n for positive n
	/// </summary>
	/// <param name="n">The number of possible outcomes. n must be positive.</param>
	/// <returns>Returns a random value in the range of [0 .. n-1]</returns>
	/// <remarks>
	/// <para>
	/// The method used is faster and of better quality than <c>rand() % n</c>. Odds are roughly 99.9% even for n = 1 million.
	/// Evenness is better for smaller n, and much worse as n gets bigger.
	/// </para>
	/// <para>
	/// Example: to simulate a d6 use <c>SDL_rand(6) + 1</c> The +1 converts 0..5 to 1..6
	/// </para>
	/// <para>
	/// If you want to generate a pseudo-random number in the full range of <see href="https://wiki.libsdl.org/SDL3/Sint32">Sint32</see>, you should use: (<see href="https://wiki.libsdl.org/SDL3/Sint32">Sint32</see>)<see href="https://wiki.libsdl.org/SDL3/SDL_rand_bits">SDL_rand_bits</see>()
	/// </para>
	/// <para>
	/// If you want reproducible output, be sure to initialize with SDL_srand() first.
	/// </para>
	/// <para>
	/// There are no guarantees as to the quality of the random sequence produced, and this should not be used for security (cryptography, passwords) or where money is on the line (loot-boxes, casinos).
	/// There are many random number libraries available with different characteristics and you should pick one of those to meet any serious needs.
	/// </para>
	/// <para>
	/// All calls should be made from a single thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_rand">SDL_rand</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal static partial int SDL_rand(int n);

	/// <summary>
	/// Generate 32 pseudo-random bits.
	/// </summary>
	/// <returns>Returns a random value in the range of [0-<see href="https://wiki.libsdl.org/SDL3/SDL_MAX_UINT32">SDL_MAX_UINT32</see>]</returns>
	/// <remarks>
	/// <para>
	/// You likely want to use <see href="https://wiki.libsdl.org/SDL3/SDL_rand">SDL_rand</see>() to get a pseudo-random number instead.
	/// </para>
	/// <para>
	/// There are no guarantees as to the quality of the random sequence produced, and this should not be used for security (cryptography, passwords) or where money is on the line (loot-boxes, casinos).
	/// There are many random number libraries available with different characteristics and you should pick one of those to meet any serious needs.
	/// </para>
	/// <para>
	/// All calls should be made from a single thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_rand_bits">SDL_rand_bits</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal static partial uint SDL_rand_bits();

	/// <summary>
	/// Generate 32 pseudo-random bits
	/// </summary>
	/// <param name="state">A pointer to the current random number state, this may not be NULL</param>
	/// <returns>Returns a random value in the range of [0-<see href="https://wiki.libsdl.org/SDL3/SDL_MAX_UINT32">SDL_MAX_UINT32</see>]</returns>
	/// <remarks>
	/// <para>
	/// You likely want to use <see href="https://wiki.libsdl.org/SDL3/SDL_rand_r">SDL_rand_r</see>() to get a pseudo-random number instead.
	/// </para>
	/// <para>
	/// There are no guarantees as to the quality of the random sequence produced, and this should not be used for security (cryptography, passwords) or where money is on the line (loot-boxes, casinos).
	/// There are many random number libraries available with different characteristics and you should pick one of those to meet any serious needs.
	/// </para>
	/// <para>
	/// This function is thread-safe, as long as the state pointer isn't shared between threads.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_rand_bits_r">SDL_rand_bits_r</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial uint SDL_rand_bits_r(ulong* state);

	/// <summary>
	/// Generate a pseudo-random number less than n for positive n
	/// </summary>
	/// <param name="state">A pointer to the current random number state, this may not be NULL</param>
	/// <param name="n">The number of possible outcomes. n must be positive.</param>
	/// <returns>Returns a random value in the range of [0 .. n-1]</returns>
	/// <remarks>
	/// <para>
	/// The method used is faster and of better quality than <c>rand() % n</c>. Odds are roughly 99.9% even for n = 1 million.
	/// Evenness is better for smaller n, and much worse as n gets bigger.
	/// </para>
	/// <para>
	/// Example: to simulate a d6 use <c>SDL_rand(6) + 1</c> The +1 converts 0..5 to 1..6
	/// </para>
	/// <para>
	/// If you want to generate a pseudo-random number in the full range of <see href="https://wiki.libsdl.org/SDL3/Sint32">Sint32</see>, you should use: (<see href="https://wiki.libsdl.org/SDL3/Sint32">Sint32</see>)<see href="https://wiki.libsdl.org/SDL3/SDL_rand_bits_r">SDL_rand_bits</see>(state)
	/// </para>
	/// <para>
	/// There are no guarantees as to the quality of the random sequence produced, and this should not be used for security (cryptography, passwords) or where money is on the line (loot-boxes, casinos).
	/// There are many random number libraries available with different characteristics and you should pick one of those to meet any serious needs.
	/// </para>
	/// <para>
	/// This function is thread-safe, as long as the state pointer isn't shared between threads.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_rand_r">SDL_rand_r</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial int SDL_rand_r(ulong* state, int n);

	/// <summary>
	/// Generate a uniform pseudo-random floating point number less than 1.0
	/// </summary>
	/// <returns>Returns a random value in the range of [0.0, 1.0)</returns>
	/// <remarks>
	/// <para>
	/// If you want reproducible output, be sure to initialize with <see href="https://wiki.libsdl.org/SDL3/SDL_srand">SDL_srand</see>() first.
	/// </para>
	/// <para>
	/// There are no guarantees as to the quality of the random sequence produced, and this should not be used for security (cryptography, passwords) or where money is on the line (loot-boxes, casinos).
	/// There are many random number libraries available with different characteristics and you should pick one of those to meet any serious needs.
	/// </para>
	/// <para>
	/// All calls should be made from a single thread
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_randf">SDL_randf</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial float SDL_randf();

	/// <summary>
	/// Generate a uniform pseudo-random floating point number less than 1.0
	/// </summary>
	/// <param name="state">A pointer to the current random number state, this may not be NULL</param>
	/// <returns>Returns a random value in the range of [0.0, 1.0)</returns>
	/// <remarks>
	/// <para>
	/// If you want reproducible output, be sure to initialize with <see href="https://wiki.libsdl.org/SDL3/SDL_srand">SDL_srand</see>() first.
	/// </para>
	/// <para>
	/// There are no guarantees as to the quality of the random sequence produced, and this should not be used for security (cryptography, passwords) or where money is on the line (loot-boxes, casinos).
	/// There are many random number libraries available with different characteristics and you should pick one of those to meet any serious needs.
	/// </para>
	/// <para>
	/// This function is thread-safe, as long as the state pointer isn't shared between threads.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_randf_r">SDL_randf_r</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial float SDL_randf_r(ulong* state);

	/// <summary>
	/// Seeds the pseudo-random number generator
	/// </summary>
	/// <param name="seed">The value to use as a random number seed, or 0 to use <see href="https://wiki.libsdl.org/SDL3/SDL_GetPerformanceCounter">SDL_GetPerformanceCounter</see>()</param>
	/// <remarks>
	/// <para>
	/// Reusing the seed number will cause <see href="https://wiki.libsdl.org/SDL3/SDL_rand">SDL_rand</see>() to repeat the same stream of 'random' numbers.
	/// </para>
	/// <para>
	/// This should be called on the same thread that calls <see href="https://wiki.libsdl.org/SDL3/SDL_rand">SDL_rand</see>().
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_srand">SDL_srand</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial void SDL_srand(ulong seed);
}
