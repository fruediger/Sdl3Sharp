using Sdl3Sharp.Internal.Interop;
using Sdl3Sharp.SourceGeneration;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Utilities;

partial class SystemInfo
{
	/// <summary>
	/// Determine the L1 cache line size of the CPU
	/// </summary>
	/// <returns>Returns the L1 cache line size of the CPU, in bytes</returns>
	/// <remarks>
	/// <para>
	/// This is useful for determining multi-threaded structure padding or SIMD prefetch sizes.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetCPUCacheLineSize">SDL_GetCPUCacheLineSize</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal static partial int SDL_GetCPUCacheLineSize();

	/// <summary>
	/// Get the number of logical CPU cores available
	/// </summary>
	/// <returns>Returns the total number of logical CPU cores. On CPUs that include technologies such as hyperthreading, the number of logical cores may be more than the number of physical cores.</returns>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetNumLogicalCPUCores">SDL_GetNumLogicalCPUCores</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal static partial int SDL_GetNumLogicalCPUCores();

	/// <summary>
	/// Report the alignment this system needs for SIMD allocations
	/// </summary>
	/// <returns>Returns the alignment in bytes needed for available, known SIMD instructions</returns>
	/// <remarks>
	/// <para>
	/// This will return the minimum number of bytes to which a pointer must be aligned to be compatible with SIMD instructions on the current machine.
	/// For example, if the machine supports SSE only, it will return 16, but if it supports AVX-512F, it'll return 64 (etc).
	/// This only reports values for instruction sets SDL knows about, so if your SDL build doesn't have <see href="https://wiki.libsdl.org/SDL3/SDL_HasAVX512F">SDL_HasAVX512F</see>(),
	/// then it might return 16 for the SSE support it sees and not 64 for the AVX-512 instructions that exist but SDL doesn't know about.
	/// Plan accordingly.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetSIMDAlignment">SDL_GetSIMDAlignment</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal static partial nuint SDL_GetSIMDAlignment();

	/// <summary>
	/// Get the amount of RAM configured in the system
	/// </summary>
	/// <returns>Returns the amount of RAM configured in the system in MiB</returns>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetSystemRAM">SDL_GetSystemRAM</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal static partial int SDL_GetSystemRAM();

	/// <summary>
	/// Determine whether the CPU has AltiVec features
	/// </summary>
	/// <returns>Returns true if the CPU has AltiVec features or false if not</returns>
	/// <remarks>
	/// <para>
	/// This always returns false on CPUs that aren't using PowerPC instruction sets.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_HasAltiVec">SDL_HasAltiVec</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal static partial CBool SDL_HasAltiVec();

	/// <summary>
	/// Determine whether the CPU has ARM SIMD (ARMv6) features
	/// </summary>
	/// <returns>Returns true if the CPU has ARM SIMD features or false if not</returns>
	/// <remarks>
	/// <para>
	/// This is different from ARM NEON, which is a different instruction set.
	/// </para>
	/// <para>
	/// This always returns false on CPUs that aren't using ARM instruction sets.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_HasARMSIMD"></seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal static partial CBool SDL_HasARMSIMD();

	/// <summary>
	/// Determine whether the CPU has AVX features
	/// </summary>
	/// <returns>Returns true if the CPU has AVX features or false if not</returns>
	/// <remarks>
	/// <para>
	/// This always returns false on CPUs that aren't using Intel instruction sets.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_HasAVX">SDL_HasAVX</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal static partial CBool SDL_HasAVX();

	/// <summary>
	/// Determine whether the CPU has AVX2 features
	/// </summary>
	/// <returns>Returns true if the CPU has AVX2 features or false if not</returns>
	/// <remarks>
	/// <para>
	/// This always returns false on CPUs that aren't using Intel instruction sets.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_HasAVX2">SDL_HasAVX2</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal static partial CBool SDL_HasAVX2();

	/// <summary>
	/// Determine whether the CPU has AVX-512F (foundation) features
	/// </summary>
	/// <returns>Returns true if the CPU has AVX-512F features or false if not</returns>
	/// <remarks>
	/// <para>
	/// This always returns false on CPUs that aren't using Intel instruction sets.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_HasAVX512F">SDL_HasAVX512F</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal static partial CBool SDL_HasAVX512F();

	/// <summary>
	/// Determine whether the CPU has LASX (LOONGARCH SIMD) features
	/// </summary>
	/// <returns>Returns true if the CPU has LOONGARCH LASX features or false if not</returns>
	/// <remarks>
	/// <para>
	/// This always returns false on CPUs that aren't using LOONGARCH instruction sets.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_HasLASX">SDL_HasLASX</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal static partial CBool SDL_HasLASX();

	/// <summary>
	/// Determine whether the CPU has LSX (LOONGARCH SIMD) features
	/// </summary>
	/// <returns>Returns true if the CPU has LOONGARCH LSX features or false if not</returns>
	/// <remarks>
	/// <para>
	/// This always returns false on CPUs that aren't using LOONGARCH instruction sets.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_HasLSX">SDL_HasLSX</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal static partial CBool SDL_HasLSX();

	/// <summary>
	/// Determine whether the CPU has MMX features
	/// </summary>
	/// <returns>Returns true if the CPU has MMX features or false if not</returns>
	/// <remarks>
	/// <para>
	/// This always returns false on CPUs that aren't using Intel instruction sets.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_HasMMX">SDL_HasMMX</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal static partial CBool SDL_HasMMX();

	/// <summary>
	/// Determine whether the CPU has NEON (ARM SIMD) features
	/// </summary>
	/// <returns>Returns true if the CPU has ARM NEON features or false if not</returns>
	/// <remarks>
	/// <para>
	/// This always returns false on CPUs that aren't using ARM instruction sets.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_HasNEON">SDL_HasNEON</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal static partial CBool SDL_HasNEON();

	/// <summary>
	/// Determine whether the CPU has SSE features
	/// </summary>
	/// <returns>Returns true if the CPU has SSE features or false if not</returns>
	/// <remarks>
	/// <para>
	/// This always returns false on CPUs that aren't using Intel instruction sets.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_HasSSE">SDL_HasSSE</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal static partial CBool SDL_HasSSE();

	/// <summary>
	/// Determine whether the CPU has SSE2 features
	/// </summary>
	/// <returns>Returns true if the CPU has SSE2 features or false if not</returns>
	/// <remarks>
	/// <para>
	/// This always returns false on CPUs that aren't using Intel instruction sets.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_HasSSE2">SDL_HasSSE2</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal static partial CBool SDL_HasSSE2();

	/// <summary>
	/// Determine whether the CPU has SSE3 features
	/// </summary>
	/// <returns>Returns true if the CPU has SSE3 features or false if not</returns>
	/// <remarks>
	/// <para>
	/// This always returns false on CPUs that aren't using Intel instruction sets.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_HasSSE3">SDL_HasSSE3</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal static partial CBool SDL_HasSSE3();

	/// <summary>
	/// Determine whether the CPU has SSE4.1 features
	/// </summary>
	/// <returns>Returns true if the CPU has SSE4.1 features or false if not</returns>
	/// <remarks>
	/// <para>
	/// This always returns false on CPUs that aren't using Intel instruction sets.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_HasSSE41">SDL_HasSSE41</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal static partial CBool SDL_HasSSE41();

	/// <summary>
	/// Determine whether the CPU has SSE4.2 features
	/// </summary>
	/// <returns>Returns true if the CPU has SSE4.2 features or false if not</returns>
	/// <remarks>
	/// <para>
	/// This always returns false on CPUs that aren't using Intel instruction sets.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_HasSSE42"></seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal static partial CBool SDL_HasSSE42();
}
