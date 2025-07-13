using Sdl3Sharp.Interop;
using Sdl3Sharp.SourceGeneration;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Utilities;

partial class Hash
{	
	/// <summary>
	/// Calculate a CRC-16 value
	/// </summary>
	/// <param name="crc">The current checksum for this data set, or 0 for a new data set</param>
	/// <param name="data">A new block of data to add to the checksum</param>
	/// <param name="len">The size, in bytes, of the new block of data.</param>
	/// <returns>Returns a CRC-16 checksum value of all blocks in the data set</returns>
	/// <remarks>
	/// <para>
	/// <see href="https://en.wikipedia.org/wiki/Cyclic_redundancy_check"/>
	/// </para>
	/// <para>
	/// This function can be called multiple times, to stream data to be checksummed in blocks.
	/// Each call must provide the previous CRC-16 return value to be updated with the next block.
	/// The first call to this function for a set of blocks should pass in a zero CRC value.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_crc16">SDL_crc16</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial ushort SDL_crc16(ushort crc, void* data, nuint len);
	
	/// <summary>
	/// Calculate a CRC-32 value
	/// </summary>
	/// <param name="crc">The current checksum for this data set, or 0 for a new data set</param>
	/// <param name="data">A new block of data to add to the checksum</param>
	/// <param name="len">The size, in bytes, of the new block of data.</param>
	/// <returns>Returns a CRC-32 checksum value of all blocks in the data set</returns>
	/// <remarks>
	/// <para>
	/// <see href="https://en.wikipedia.org/wiki/Cyclic_redundancy_check"/>
	/// </para>
	/// <para>
	/// This function can be called multiple times, to stream data to be checksummed in blocks.
	/// Each call must provide the previous CRC-32 return value to be updated with the next block.
	/// The first call to this function for a set of blocks should pass in a zero CRC value.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_crc32">SDL_crc32</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial uint SDL_crc32(uint crc, void* data, nuint len);

	/// <summary>
	/// Calculate a 32-bit MurmurHash3 value for a block of data
	/// </summary>
	/// <param name="data">The data to be hashed</param>
	/// <param name="len">The size of data, in bytes</param>
	/// <param name="seed">A value that alters the final hash value</param>
	/// <returns>Returns a Murmur3 32-bit hash value</returns>
	/// <remarks>
	/// <para>
	/// <see href="https://en.wikipedia.org/wiki/MurmurHash"/>
	/// </para>
	/// <para>
	/// A seed may be specified, which changes the final results consistently, but this does not work like <see href="https://wiki.libsdl.org/SDL3/SDL_crc16">SDL_crc16</see> and <see href="https://wiki.libsdl.org/SDL3/SDL_crc32">SDL_crc32</see>:
	/// you can't feed a previous result from this function back into itself as the next seed value to calculate a hash in chunks;
	/// it won't produce the same hash as it would if the same data was provided in a single call.
	/// </para>
	/// <para>
	/// If you aren't sure what to provide for a seed, zero is fine. Murmur3 is not cryptographically secure, so it shouldn't be used for hashing top-secret data.
	/// </para>
	/// </remarks>	
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_murmur3_32">SDL_murmur3_32</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial uint SDL_murmur3_32(void* data, nuint len, uint seed);
}
