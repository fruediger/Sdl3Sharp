using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Utilities;

/// <summary>
/// Provides properties to check the current system's configuration
/// </summary>
/// <remarks>
/// <para>
/// These properties are largely concerned with indicating if the system has access to various SIMD instruction sets,
/// but also has other important info to share, such as <see cref="RamSize">the system's RAM size</see> or <see cref="CpuLogicalCoreCount">the number of logical CPU cores</see>.
/// </para>
/// </remarks>
public static partial class SystemInfo
{
	/// <summary>
	/// Gets the L1 cache size of the CPU in bytes
	/// </summary>
	/// <value>
	/// The L1 cache size of the CPU in bytes
	/// </value>
	/// <remarks>
	/// <para>
	/// This is useful for determining multi-threaded structure padding or SIMD prefetch sizes.
	/// </para>
	/// </remarks>
	public static int CpuCacheLineSize { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => SDL_GetCPUCacheLineSize(); }

	/// <summary>
	/// Gets the number of logical CPU cores available in the system
	/// </summary>
	/// <value>
	/// The number of logical CPU cores available in the system
	/// </value>
	public static int CpuLogicalCoreCount { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => SDL_GetNumLogicalCPUCores(); }

	/// <summary>
	/// Gets a value indicating whether the CPU has AltiVec features
	/// </summary>
	/// <value>
	/// A value indicating whether the CPU has AltiVec features
	/// </value>
	/// <remarks>
	/// <para>
	/// The value of this property will always be <c><see langword="false"/></c> on CPUs that aren't using PowerPC instruction sets.
	/// </para>
	/// </remarks>
	public static bool HasAltiVec { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => SDL_HasAltiVec(); }

	/// <summary>
	/// Gets a value indicating whether the CPU has ARM SIMD (ARMv6) features
	/// </summary>
	/// <value>
	/// A value indicating whether the CPU has ARM SIMD (ARMv6) features
	/// </value>
	/// <remarks>
	/// <para>
	/// <see cref="HasArmSimd">ARM SIMD (ARMv6) features</see> are different from <see cref="HasNeon">ARM NEON features</see>, which are a different instruction set.
	/// </para>
	/// <para>
	/// The value of this property will always be <c><see langword="false"/></c> on CPUs that aren't using ARM instruction sets.
	/// </para>
	/// </remarks>
	public static bool HasArmSimd { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => SDL_HasARMSIMD(); }

	/// <summary>
	/// Gets a value indicating whether the CPU has AVX features
	/// </summary>
	/// <value>
	/// A value indicating whether the CPU has AVX features
	/// </value>
	/// <remarks>
	/// <para>
	/// The value of this property will always be <c><see langword="false"/></c> on CPUs that aren't using Intel instruction sets.
	/// </para>
	/// </remarks>
	public static bool HasAvx { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => SDL_HasAVX(); }

	/// <summary>
	/// Gets a value indicating whether the CPU has AVX2 features
	/// </summary>
	/// <value>
	/// A value indicating whether the CPU has AVX2 features
	/// </value>
	/// <remarks>
	/// <para>
	/// The value of this property will always be <c><see langword="false"/></c> on CPUs that aren't using Intel instruction sets.
	/// </para>
	/// </remarks>
	public static bool HasAvx2 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => SDL_HasAVX2(); }

	/// <summary>
	/// Gets a value indicating whether the CPU has AVX-512F (foundation) features
	/// </summary>
	/// <value>
	/// A value indicating whether the CPU has AVX-512F (foundation) features
	/// </value>
	/// <remarks>
	/// <para>
	/// The value of this property will always be <c><see langword="false"/></c> on CPUs that aren't using Intel instruction sets.
	/// </para>
	/// </remarks>
	public static bool HasAvx512F { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => SDL_HasAVX512F(); }

	/// <summary>
	/// Gets a value indicating whether the CPU has LASX (LOONGARCH SIMD) features
	/// </summary>
	/// <value>
	/// A value indicating whether the CPU has LASX (LOONGARCH SIMD) features
	/// </value>
	/// <remarks>
	/// <para>
	/// The value of this property will always be <c><see langword="false"/></c> on CPUs that aren't using LOONGARCH instruction sets.
	/// </para>
	/// </remarks>
	public static bool HasLasx { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => SDL_HasLASX(); }

	/// <summary>
	/// Gets a value indicating whether the CPU has LSX (LOONGARCH SIMD) features
	/// </summary>
	/// <value>
	/// A value indicating whether the CPU has LSX (LOONGARCH SIMD) features
	/// </value>
	/// <remarks>
	/// <para>
	/// The value of this property will always be <c><see langword="false"/></c> on CPUs that aren't using LOONGARCH instruction sets.
	/// </para>
	/// </remarks>
	public static bool HasLsx { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => SDL_HasLSX(); }

	/// <summary>
	/// Gets a value indicating whether the CPU has MMX features
	/// </summary>
	/// <value>
	/// A value indicating whether the CPU has MMX features
	/// </value>
	/// <remarks>
	/// <para>
	/// The value of this property will always be <c><see langword="false"/></c> on CPUs that aren't using Intel instruction sets.
	/// </para>
	/// </remarks>
	public static bool HasMmx { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => SDL_HasMMX(); }

	/// <summary>
	/// Gets a value indicating whether the CPU has NEON (ARM SIMD) features
	/// </summary>
	/// <value>
	/// A value indicating whether the CPU has NEON (ARM SIMD) features
	/// </value>
	/// <remarks>
	/// <para>
	/// The value of this property will always be <c><see langword="false"/></c> on CPUs that aren't using ARM instruction sets.
	/// </para>
	/// </remarks>
	public static bool HasNeon { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => SDL_HasNEON(); }

	/// <summary>
	/// Gets a value indicating whether the CPU has SSE features
	/// </summary>
	/// <value>
	/// A value indicating whether the CPU has SSE features
	/// </value>
	/// <remarks>
	/// <para>
	/// The value of this property will always be <c><see langword="false"/></c> on CPUs that aren't using Intel instruction sets.
	/// </para>
	/// </remarks>
	public static bool HasSse { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => SDL_HasSSE(); }

	/// <summary>
	/// Gets a value indicating whether the CPU has SSE2 features
	/// </summary>
	/// <value>
	/// A value indicating whether the CPU has SSE2 features
	/// </value>
	/// <remarks>
	/// <para>
	/// The value of this property will always be <c><see langword="false"/></c> on CPUs that aren't using Intel instruction sets.
	/// </para>
	/// </remarks>
	public static bool HasSse2 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => SDL_HasSSE2(); }

	/// <summary>
	/// Gets a value indicating whether the CPU has SSE3 features
	/// </summary>
	/// <value>
	/// A value indicating whether the CPU has SSE3 features
	/// </value>
	/// <remarks>
	/// <para>
	/// The value of this property will always be <c><see langword="false"/></c> on CPUs that aren't using Intel instruction sets.
	/// </para>
	/// </remarks>
	public static bool HasSse3 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => SDL_HasSSE3(); }

	/// <summary>
	/// Gets a value indicating whether the CPU has SSE4.1 features
	/// </summary>
	/// <value>
	/// A value indicating whether the CPU has SSE4.1 features
	/// </value>
	/// <remarks>
	/// <para>
	/// The value of this property will always be <c><see langword="false"/></c> on CPUs that aren't using Intel instruction sets.
	/// </para>
	/// </remarks>
	public static bool HasSse41 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => SDL_HasSSE41(); }

	/// <summary>
	/// Gets a value indicating whether the CPU has SSE4.2 features
	/// </summary>
	/// <value>
	/// A value indicating whether the CPU has SSE4.2 features
	/// </value>
	/// <remarks>
	/// <para>
	/// The value of this property will always be <c><see langword="false"/></c> on CPUs that aren't using Intel instruction sets.
	/// </para>
	/// </remarks>
	public static bool HasSse42 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => SDL_HasSSE42(); }

	/// <summary>
	/// Gets the amount of RAM configured in the system in <see href="https://en.wikipedia.org/wiki/Byte#Multiple-byte_units">MiB</see>
	/// </summary>
	/// <value>
	/// The amount of RAM configured in the system in <see href="https://en.wikipedia.org/wiki/Byte#Multiple-byte_units">MiB</see>
	/// </value>
	public static int RamSize { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => SDL_GetSystemRAM(); }

	/// <summary>
	/// Gets the alignment this system needs for available and known SIMD instructions in bytes
	/// </summary>
	/// <value>
	/// The alignment this system needs for available and known SIMD instructions in bytes
	/// </value>
	/// <remarks>
	/// <para>
	/// The value of this property is the minimum number of bytes to which a pointer must be aligned to be compatible with SIMD instructions on the current machine.
	/// For example, if the machine supports SSE only, it will be <c>16</c>, but if it supports AVX-512F, it will be <c>64</c> (etc).
	/// </para>
	/// <para>
	/// The values of this property are only for instruction sets SDL knows about, so if your SDL build doesn't have <see cref="HasAvx512F"/>,
	/// then it might be <c>16</c> for the SSE support it sees and not <c>64</c> for the AVX-512 instructions that exist but SDL doesn't know about.
	/// Plan accordingly.
	/// </para>
	/// </remarks>
	public static nuint SimdAlignment { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => SDL_GetSIMDAlignment(); }
}
