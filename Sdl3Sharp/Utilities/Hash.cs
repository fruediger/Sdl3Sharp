using System;

namespace Sdl3Sharp.Utilities;

/// <summary>
/// Provides methods to calculate checksums and hash values for data blocks
/// </summary>
public static partial class Hash
{
	/// <summary>
	/// Calculates and accumulates a CRC-16 checksum hash value
	/// </summary>
	/// <param name="crc">The current checksum for the data set, or <c>0</c> for a new data set</param>
	/// <param name="data">The new block of data to be added to the checksum</param>
	/// <returns>A CRC-16 checksum hash value of all blocks in the data set</returns>
	/// <remarks>
	/// Calls to this method can be chained by passing in the previous return value as the argument for the <paramref name="crc"/> parameter to add a new block of data to the checksum.
	/// This allows for the data set to be streamed. If you want to start a new data set, you can simply pass in <c>0</c> as the argument for the <paramref name="crc"/> parameter.
	/// </remarks>
	/// <seealso href="https://en.wikipedia.org/wiki/Cyclic_redundancy_check"/>
	public static ushort Crc16(ushort crc, Memory<byte> data)
	{
		unsafe
		{
			using var pin = data.Pin();

			return SDL_crc16(crc, pin.Pointer, unchecked((nuint)data.Length));
		}
	}

	/// <summary>
	/// Calculates and accumulates a CRC-32 checksum hash value
	/// </summary>
	/// <param name="crc">The current checksum for the data set, or <c>0</c> for a new data set</param>
	/// <param name="data">The new block of data to be added to the checksum</param>
	/// <returns>A CRC-32 checksum hash value of all blocks in the data set</returns>
	/// <remarks>
	/// Calls to this method can be chained by passing in the previous return value as the argument for the <paramref name="crc"/> parameter to add a new block of data to the checksum.
	/// This allows for the data set to be streamed. If you want to start a new data set, you can simply pass in <c>0</c> as the argument for the <paramref name="crc"/> parameter.
	/// </remarks>
	/// <seealso href="https://en.wikipedia.org/wiki/Cyclic_redundancy_check"/>
	public static uint Crc32(uint crc, Memory<byte> data)
	{
		unsafe
		{
			using var pin = data.Pin();

			return SDL_crc32(crc, pin.Pointer, unchecked((nuint)data.Length));
		}
	}

	/// <summary>
	/// Calculates a 32-bit MurmurHash3 hash value for a block of data
	/// </summary>
	/// <param name="data">The block of data to be hashed</param>
	/// <param name="seed">A value that alters the resulting hash value</param>
	/// <returns>A MurmurHash3 hash value for the given <paramref name="data"/></returns>
	/// <remarks>
	/// <para>
	/// A <paramref name="seed"/> value may be specified, which changes the final results consistently, but this does not work like <see cref="Crc16(ushort, Memory{byte})"/> or <see cref="Crc32(uint, Memory{byte})"/>,
	/// as you can't feed a previous result from a call to this method back into itself as the next <paramref name="seed"/> value to calculate a hash in chunks;
	/// it won't produce the same hash as it would if the same data was provided in a single call.
	/// </para>
	/// <para>
	/// If you aren't sure what to provide for the <paramref name="seed"/> value, <c>0</c> is fine.
	/// </para>
	/// <para>
	/// MurmurHash3 is not cryptographically secure, so it shouldn't be used for hashing top-secret data.
	/// </para>
	/// </remarks>
	/// <seealso href="https://en.wikipedia.org/wiki/MurmurHash"/>
	public static uint Murmur3_32(Memory<byte> data, uint seed = 0)
	{
		unsafe
		{
			using var pin = data.Pin();

			return SDL_murmur3_32(pin.Pointer, unchecked((nuint)data.Length), seed);
		}
	}
}
