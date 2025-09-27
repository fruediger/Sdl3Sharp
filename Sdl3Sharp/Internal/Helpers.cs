using System;
using System.Buffers.Binary;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Internal;

internal static class Helpers
{
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static Int128 FromLittleEndianInt128(Int128 value) => BitConverter.IsLittleEndian
		? value
		: BinaryPrimitives.ReverseEndianness(value);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static Int128 FromBigEndianInt128(Int128 value) => BitConverter.IsLittleEndian
		? BinaryPrimitives.ReverseEndianness(value)
		: value;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static IntPtr FromLittleEndianIntPtr(IntPtr value) => BitConverter.IsLittleEndian
		? value
		: BinaryPrimitives.ReverseEndianness(value);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static IntPtr FromBigEndianIntPtr(IntPtr value) => BitConverter.IsLittleEndian
		? BinaryPrimitives.ReverseEndianness(value)
		: value;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static UInt128 FromLittleEndianUInt128(UInt128 value) => BitConverter.IsLittleEndian
		? value
		: BinaryPrimitives.ReverseEndianness(value);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static UInt128 FromBigEndianUInt128(UInt128 value) => BitConverter.IsLittleEndian
		? BinaryPrimitives.ReverseEndianness(value)
		: value;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static UIntPtr FromLittleEndianUIntPtr(UIntPtr value) => BitConverter.IsLittleEndian
		? value
		: BinaryPrimitives.ReverseEndianness(value);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static UIntPtr FromBigEndianUIntPtr(UIntPtr value) => BitConverter.IsLittleEndian
		? BinaryPrimitives.ReverseEndianness(value)
		: value;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static IntPtr ToBigEndianIntPtr(IntPtr value) => BitConverter.IsLittleEndian
		? BinaryPrimitives.ReverseEndianness(value)
		: value;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static IntPtr ToLittleEndianIntPtr(IntPtr value) => BitConverter.IsLittleEndian
		? value
		: BinaryPrimitives.ReverseEndianness(value);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static Int128 ToBigEndianInt128(Int128 value) => BitConverter.IsLittleEndian
		? BinaryPrimitives.ReverseEndianness(value)
		: value;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static Int128 ToLittleEndianInt128(Int128 value) => BitConverter.IsLittleEndian
		? value
		: BinaryPrimitives.ReverseEndianness(value);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static UIntPtr ToBigEndianUIntPtr(UIntPtr value) => BitConverter.IsLittleEndian
		? BinaryPrimitives.ReverseEndianness(value)
		: value;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static UIntPtr ToLittleEndianUIntPtr(UIntPtr value) => BitConverter.IsLittleEndian
		? value
		: BinaryPrimitives.ReverseEndianness(value);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static UInt128 ToBigEndianUInt128(UInt128 value) => BitConverter.IsLittleEndian
		? BinaryPrimitives.ReverseEndianness(value)
		: value;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static UInt128 ToLittleEndianUInt128(UInt128 value) => BitConverter.IsLittleEndian
		? value
		: BinaryPrimitives.ReverseEndianness(value);
}
