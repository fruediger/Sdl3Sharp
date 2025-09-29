using Sdl3Sharp.Internal;
using Sdl3Sharp.Utilities;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace Sdl3Sharp.IO;

public abstract partial class Stream : IDisposable
{
	private unsafe SDL_IOStream* mContext = null;
	private GCHandle mImplementationHandle = default;

	/// <remarks>
	/// <para>
	/// This constructor does neither <see langword="throw"/> nor fail otherwise
	/// </para>
	/// </remarks>
	private protected unsafe Stream(SDL_IOStream* context) => mContext = context;

	/// <summary>
	/// 
	/// </summary>
	/// <param name="implementation"></param>
	/// <exception cref="SdlException"></exception>
	/// <inheritdoc cref="SDL_IOStreamInterface(IStream, out GCHandle)"/>
	public Stream(IStream implementation)
	{
		unsafe
		{
			var iface = new SDL_IOStreamInterface(implementation, out mImplementationHandle);

			mContext = SDL_OpenIO(&iface, unchecked((void*)GCHandle.ToIntPtr(mImplementationHandle)));

			if (mContext is null)
			{
				mImplementationHandle.Free();
				mImplementationHandle = default;				

				failCouldNotCreateStream();
			}
		}
			
		[DoesNotReturn]
		static void failCouldNotCreateStream() => throw new SdlException("Could not create the custom IO stream");
	}

	~Stream() => Dispose(disposing: false, close: true);

	private protected unsafe SDL_IOStream* Context { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mContext; }

	public long Length
	{
		get
		{
			unsafe
			{
				return SDL_GetIOSize(mContext);
			}
		}
	}

	public Properties? Properties
	{
		get
		{
			unsafe
			{
				return SDL_GetIOProperties(mContext) switch
				{
					0 => null,
					var id => Properties.GetOrCreate(sdl: null, id)
				};
			}
		}
	}

	public long Position
	{
		get
		{
			unsafe
			{
				return SDL_TellIO(mContext);
			}
		}
	}

	public StreamStatus Status
	{
		get
		{
			unsafe
			{
				return SDL_GetIOStatus(mContext);
			}
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true, close: true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing, bool close)
	{
		unsafe
		{
			if (mContext is not null)
			{
				if (close)
				{
					SDL_CloseIO(mContext);
				}

				mContext = null;
			}

			if (mImplementationHandle.IsAllocated)
			{
				mImplementationHandle.Free();
			}
		}
	}

	public bool TryClose()
	{
		unsafe
		{
			return SDL_CloseIO(mContext);
		}
	}

	public bool TryFlush()
	{
		unsafe
		{
			return SDL_FlushIO(mContext);
		}
	}

	public bool TryLoad([NotNullWhen(true)] out NativeMemoryManager? data, bool closeAfterwards = false)
	{
		unsafe
		{
			nuint dataSize;
			void* dataPtr;
			try
			{
				dataPtr = SDL_LoadFile_IO(mContext, &dataSize, closeAfterwards);
			}
			finally
			{
				if (closeAfterwards)
				{
					Dispose(disposing: true, close: false /* SDL_LoadFile_IO already closed the stream */);
				}
			}

			if (dataPtr is null)
			{
				data = null;
				return false;
			}

			data = new(dataPtr, dataSize, &Utilities.NativeMemory.SDL_free);
			return true;
		}
	}

	public bool TryRead(Utilities.NativeMemory data, out nuint bytesRead)
	{
		unsafe
		{
			if (!data.IsValid)
			{
				bytesRead = 0;
				return false;
			}

			bytesRead = SDL_ReadIO(mContext, data.RawPointer, data.Length);

			return bytesRead is not 0;
		}
	}

	public bool TryRead(Span<byte> data, out int bytesRead)
	{
		unsafe
		{
			fixed (byte* ptr = data)
			{
				bytesRead = unchecked((int)SDL_ReadIO(mContext, ptr, unchecked((nuint)data.Length)));
			}

			return bytesRead is not 0;
		}
	}

	public unsafe bool TryRead(void* data, nuint size, out nuint bytesRead)
	{
		unsafe
		{
			bytesRead = SDL_WriteIO(mContext, data, size);

			return bytesRead is not 0;
		}
	}

	public bool TryRead<T>(out T value, out nuint bytesRead)
		where T : unmanaged, allows ref struct
	{
		unsafe
		{
			fixed (T* ptr = &value)
			{
				bytesRead = SDL_ReadIO(mContext, &ptr, unchecked((nuint)Unsafe.SizeOf<T>()));

				return bytesRead == unchecked((nuint)Unsafe.SizeOf<T>());
			}
		}
	}

	public bool TryReadDoubleBE(out double value)
	{
		unsafe
		{
			ulong localValue;

			var result = SDL_ReadU64BE(mContext, &localValue);

			value = BitConverter.UInt64BitsToDouble(localValue);

			return result;
		}
	}

	public bool TryReadDoubleLE(out double value)
	{
		unsafe
		{
			ulong localValue;

			var result = SDL_ReadU64LE(mContext, &localValue);

			value = BitConverter.UInt64BitsToDouble(localValue);

			return result;
		}
	}

	public bool TryReadHalfBE(out Half value)
	{
		unsafe
		{
			ushort localValue;

			var result = SDL_ReadU16BE(mContext, &localValue);

			value = BitConverter.UInt16BitsToHalf(localValue);

			return result;
		}
	}

	public bool TryReadHalfLE(out Half value)
	{
		unsafe
		{
			ushort localValue;

			var result = SDL_ReadU16LE(mContext, &localValue);

			value = BitConverter.UInt16BitsToHalf(localValue);

			return result;
		}
	}

	public bool TryReadInt128BE(out Int128 value)
	{
		unsafe
		{
			Int128 localValue;

			var result = SDL_ReadIO(mContext, &localValue, unchecked((nuint)Unsafe.SizeOf<Int128>())) == unchecked((nuint)Unsafe.SizeOf<Int128>());

			value = Helpers.FromBigEndianInt128(localValue);

			return result;
		}
	}

	public bool TryReadUInt128LE(out Int128 value)
	{
		unsafe
		{
			Int128 localValue;

			var result = SDL_ReadIO(mContext, &localValue, unchecked((nuint)Unsafe.SizeOf<Int128>())) == unchecked((nuint)Unsafe.SizeOf<Int128>());

			value = Helpers.FromLittleEndianInt128(localValue);

			return result;
		}
	}

	public bool TryReadInt16BE(out short value)
	{
		unsafe
		{
			short localValue;

			var result = SDL_ReadS16BE(mContext, &localValue);

			value = localValue;

			return result;
		}
	}

	public bool TryReadInt16LE(out short value)
	{
		unsafe
		{
			short localValue;

			var result = SDL_ReadS16LE(mContext, &localValue);

			value = localValue;

			return result;
		}
	}

	public bool TryReadInt32BE(out int value)
	{
		unsafe
		{
			int localValue;

			var result = SDL_ReadS32BE(mContext, &localValue);

			value = localValue;

			return result;
		}
	}

	public bool TryReadInt32LE(out int value)
	{
		unsafe
		{
			int localValue;

			var result = SDL_ReadS32LE(mContext, &localValue);

			value = localValue;

			return result;
		}
	}

	public bool TryReadInt64BE(out long value)
	{
		unsafe
		{
			long localValue;

			var result = SDL_ReadS64BE(mContext, &localValue);

			value = localValue;

			return result;
		}
	}

	public bool TryReadInt64LE(out long value)
	{
		unsafe
		{
			long localValue;

			var result = SDL_ReadS64LE(mContext, &localValue);

			value = localValue;

			return result;
		}
	}

	public bool TryReadInt8(out sbyte value)
	{
		unsafe
		{
			sbyte localValue;

			var result = SDL_ReadS8(mContext, &localValue);

			value = localValue;

			return result;
		}
	}

	public bool TryReadIntPtrBE(out nint value)
	{
		unsafe
		{
			nint localValue;

			var result = SDL_ReadIO(mContext, &localValue, unchecked((nuint)Unsafe.SizeOf<nint>())) == unchecked((nuint)Unsafe.SizeOf<nint>());

			value = Helpers.FromBigEndianIntPtr(localValue);

			return result;
		}
	}

	public bool TryReadIntPtrLE(out nint value)
	{
		unsafe
		{
			nint localValue;

			var result = SDL_ReadIO(mContext, &localValue, unchecked((nuint)Unsafe.SizeOf<nint>())) == unchecked((nuint)Unsafe.SizeOf<nint>());

			value = Helpers.FromLittleEndianIntPtr(localValue);

			return result;
		}
	}

	public bool TryReadSingleBE(out float value)
	{
		unsafe
		{
			uint localValue;

			var result = SDL_ReadU32BE(mContext, &localValue);

			value = BitConverter.UInt32BitsToSingle(localValue);

			return result;
		}
	}

	public bool TryReadSingleLE(out float value)
	{
		unsafe
		{
			uint localValue;

			var result = SDL_ReadU32LE(mContext, &localValue);

			value = BitConverter.UInt32BitsToSingle(localValue);

			return result;
		}
	}

	public bool TryReadUInt128BE(out UInt128 value)
	{
		unsafe
		{
			UInt128 localValue;

			var result = SDL_ReadIO(mContext, &localValue, unchecked((nuint)Unsafe.SizeOf<UInt128>())) == unchecked((nuint)Unsafe.SizeOf<UInt128>());

			value = Helpers.FromBigEndianUInt128(localValue);

			return result;
		}
	}

	public bool TryReadUInt128LE(out UInt128 value)
	{
		unsafe
		{
			UInt128 localValue;

			var result = SDL_ReadIO(mContext, &localValue, unchecked((nuint)Unsafe.SizeOf<UInt128>())) == unchecked((nuint)Unsafe.SizeOf<UInt128>());

			value = Helpers.FromLittleEndianUInt128(localValue);

			return result;
		}
	}

	public bool TryReadUInt16BE(out ushort value)
	{
		unsafe
		{
			ushort localValue;

			var result = SDL_ReadU16BE(mContext, &localValue);

			value = localValue;

			return result;
		}
	}

	public bool TryReadUInt16LE(out ushort value)
	{
		unsafe
		{
			ushort localValue;

			var result = SDL_ReadU16LE(mContext, &localValue);

			value = localValue;

			return result;
		}
	}

	public bool TryReadUInt32BE(out uint value)
	{
		unsafe
		{
			uint localValue;

			var result = SDL_ReadU32BE(mContext, &localValue);

			value = localValue;

			return result;
		}
	}

	public bool TryReadUInt32LE(out uint value)
	{
		unsafe
		{
			uint localValue;

			var result = SDL_ReadU32LE(mContext, &localValue);

			value = localValue;

			return result;
		}
	}

	public bool TryReadUInt64BE(out ulong value)
	{
		unsafe
		{
			ulong localValue;

			var result = SDL_ReadU64BE(mContext, &localValue);

			value = localValue;

			return result;
		}
	}

	public bool TryReadUInt64LE(out ulong value)
	{
		unsafe
		{
			ulong localValue;

			var result = SDL_ReadU64LE(mContext, &localValue);

			value = localValue;

			return result;
		}
	}

	public bool TryReadUInt8(out byte value)
	{
		unsafe
		{
			byte localValue;

			var result = SDL_ReadU8(mContext, &localValue);

			value = localValue;

			return result;
		}
	}

	public bool TryReadUIntPtrBE(out nuint value)
	{
		unsafe
		{
			nuint localValue;

			var result = SDL_ReadIO(mContext, &localValue, unchecked((nuint)Unsafe.SizeOf<nuint>())) == unchecked((nuint)Unsafe.SizeOf<nuint>());

			value = Helpers.FromBigEndianUIntPtr(localValue);

			return result;
		}
	}

	public bool TryReadUIntPtrLE(out nuint value)
	{
		unsafe
		{
			nuint localValue;

			var result = SDL_ReadIO(mContext, &localValue, unchecked((nuint)Unsafe.SizeOf<nuint>())) == unchecked((nuint)Unsafe.SizeOf<nuint>());

			value = Helpers.FromLittleEndianUIntPtr(localValue);

			return result;
		}
	}

	public bool TrySave(Utilities.NativeMemory data, bool closeAfterwards = false)
	{
		unsafe
		{
			try
			{
				return data.IsValid && (bool)SDL_SaveFile_IO(mContext, data.RawPointer, data.Length, closeAfterwards);
			}
			finally
			{
				if (closeAfterwards)
				{
					Dispose(disposing: true, close: false /* SDL_SaveFile_IO already closed the stream */);
				}
			}
		}
	}

	public bool TrySave(ReadOnlySpan<byte> data, bool closeAfterwards = false)
	{
		unsafe
		{
			fixed(byte* ptr = data)
			{
				try
				{
					return SDL_SaveFile_IO(mContext, ptr, unchecked((nuint)data.Length), closeAfterwards);
				}
				finally
				{
					if (closeAfterwards)
					{
						Dispose(disposing: true, close: false /* SDL_SaveFile_IO already closed the stream */);
					}
				}
			}
		}
	}

	public unsafe bool TrySave(void* data, nuint size, bool closeAfterwards = false)
	{
		unsafe
		{
			try
			{
				return SDL_SaveFile_IO(mContext, data, size, closeAfterwards);
			}
			finally
			{
				if (closeAfterwards)
				{
					Dispose(disposing: true, close: false /* SDL_SaveFile_IO already closed the stream */);
				}
			}
		}
	}

	public bool TrySeek(long offset, StreamWhence whence, out long absoluteOffset)
	{
		unsafe
		{
			absoluteOffset = SDL_SeekIO(mContext, offset, whence);

			return absoluteOffset is not -1;
		}
	}

	public bool TryWrite(Utilities.NativeMemory data, out nuint bytesWritten)
	{
		unsafe
		{
			if (!data.IsValid)
			{
				bytesWritten = 0;
				return false;
			}

			bytesWritten = SDL_WriteIO(mContext, data.RawPointer, data.Length);

			return bytesWritten == data.Length;
		}
	}

	public bool TryWrite(ReadOnlySpan<byte> data, out int bytesWritten)
	{
		unsafe
		{
			fixed(byte* ptr = data)
			{
				bytesWritten = unchecked((int)SDL_WriteIO(mContext, ptr, unchecked((nuint)data.Length)));
			}

			return bytesWritten == data.Length;
		}
	}

	public unsafe bool TryWrite(void* data, nuint size, out nuint bytesWritten)
	{
		unsafe
		{
			bytesWritten = SDL_WriteIO(mContext, data, size);

			return bytesWritten == size;
		}
	}

	public bool TryWrite<T>(in T value, out int bytesWritten)
		where T : unmanaged, allows ref struct
	{
		unsafe
		{
			fixed (T* ptr = &value)
			{
				bytesWritten = unchecked((int)SDL_WriteIO(mContext, ptr, unchecked((nuint)Unsafe.SizeOf<T>())));

				return bytesWritten == Unsafe.SizeOf<T>();
			}
		}
	}

	public bool TryWrite(string text, out nuint bytesWritten)
	{
		unsafe
		{
			var textUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(text.Replace("%", "%%"));
			try
			{
				bytesWritten = SDL_IOprintf(mContext, textUtf8);

				return bytesWritten is not 0 || string.IsNullOrEmpty(text);
			}
			finally
			{
				Utf8StringMarshaller.Free(textUtf8);
			}
		}
	}

	public bool TryWriteDoubleBE(double value)
	{
		unsafe
		{
			return SDL_WriteU64BE(mContext, BitConverter.DoubleToUInt64Bits(value));
		}
	}

	public bool TryWriteDoubleLE(double value)
	{
		unsafe
		{
			return SDL_WriteU64LE(mContext, BitConverter.DoubleToUInt64Bits(value));
		}
	}

	public bool TryWriteHalfBE(Half value)
	{
		unsafe
		{
			return SDL_WriteU16BE(mContext, BitConverter.HalfToUInt16Bits(value));
		}
	}

	public bool TryWriteHalfLE(Half value)
	{
		unsafe
		{
			return SDL_WriteU16LE(mContext, BitConverter.HalfToUInt16Bits(value));
		}
	}

	public bool TryWriteInt128BE(Int128 value)
	{
		unsafe
		{
			var swappedValue = Helpers.ToBigEndianInt128(value);

			return SDL_WriteIO(mContext, &swappedValue, unchecked((nuint)Unsafe.SizeOf<Int128>())) == unchecked((nuint)Unsafe.SizeOf<Int128>());
		}
	}

	public bool TryWriteInt128LE(Int128 value)
	{
		unsafe
		{
			var swappedValue = Helpers.ToLittleEndianInt128(value);

			return SDL_WriteIO(mContext, &swappedValue, unchecked((nuint)Unsafe.SizeOf<Int128>())) == unchecked((nuint)Unsafe.SizeOf<Int128>());
		}
	}

	public bool TryWriteInt16BE(short value)
	{
		unsafe
		{
			return SDL_WriteS16BE(mContext, value);
		}
	}

	public bool TryWriteInt16LE(short value)
	{
		unsafe
		{
			return SDL_WriteS16LE(mContext, value);
		}
	}

	public bool TryWriteInt32BE(int value)
	{
		unsafe
		{
			return SDL_WriteS32BE(mContext, value);
		}
	}

	public bool TryWriteInt32LE(int value)
	{
		unsafe
		{
			return SDL_WriteS32LE(mContext, value);
		}
	}

	public bool TryWriteInt64BE(long value)
	{
		unsafe
		{
			return SDL_WriteS64BE(mContext, value);
		}
	}

	public bool TryWriteInt64LE(long value)
	{
		unsafe
		{
			return SDL_WriteS64LE(mContext, value);
		}
	}

	public bool TryWriteInt8(sbyte value)
	{
		unsafe
		{
			return SDL_WriteS8(mContext, value);
		}
	}

	public bool TryWriteIntPtrBE(nint value)
	{
		unsafe
		{
			var swappedValue = Helpers.ToBigEndianIntPtr(value);

			return SDL_WriteIO(mContext, &swappedValue, unchecked((nuint)Unsafe.SizeOf<nint>())) == unchecked((nuint)Unsafe.SizeOf<nint>());
		}
	}

	public bool TryWriteIntPtrLE(nint value)
	{
		unsafe
		{
			var swappedValue = Helpers.ToLittleEndianIntPtr(value);

			return SDL_WriteIO(mContext, &swappedValue, unchecked((nuint)Unsafe.SizeOf<nint>())) == unchecked((nuint)Unsafe.SizeOf<nint>());
		}
	}

	public bool TryWriteSingleBE(float value)
	{
		unsafe
		{
			return SDL_WriteU32BE(mContext, BitConverter.SingleToUInt32Bits(value));
		}
	}

	public bool TryWriteSingleLE(float value)
	{
		unsafe
		{
			return SDL_WriteU32LE(mContext, BitConverter.SingleToUInt32Bits(value));
		}
	}

	public bool TryWriteUInt128BE(UInt128 value)
	{
		unsafe
		{
			var swappedValue = Helpers.ToBigEndianUInt128(value);

			return SDL_WriteIO(mContext, &swappedValue, unchecked((nuint)Unsafe.SizeOf<UInt128>())) == unchecked((nuint)Unsafe.SizeOf<UInt128>());
		}
	}

	public bool TryWriteUInt128LE(UInt128 value)
	{
		unsafe
		{
			var swappedValue = Helpers.ToLittleEndianUInt128(value);

			return SDL_WriteIO(mContext, &swappedValue, unchecked((nuint)Unsafe.SizeOf<UInt128>())) == unchecked((nuint)Unsafe.SizeOf<UInt128>());
		}
	}

	public bool TryWriteUInt16BE(ushort value)
	{
		unsafe
		{
			return SDL_WriteU16BE(mContext, value);
		}
	}

	public bool TryWriteUInt16LE(ushort value)
	{
		unsafe
		{
			return SDL_WriteU16LE(mContext, value);
		}
	}

	public bool TryWriteUInt32BE(uint value)
	{
		unsafe
		{
			return SDL_WriteU32BE(mContext, value);
		}
	}

	public bool TryWriteUInt32LE(uint value)
	{
		unsafe
		{
			return SDL_WriteU32LE(mContext, value);
		}
	}

	public bool TryWriteUInt64BE(ulong value)
	{
		unsafe
		{
			return SDL_WriteU64BE(mContext, value);
		}
	}

	public bool TryWriteUInt64LE(ulong value)
	{
		unsafe
		{
			return SDL_WriteU64LE(mContext, value);
		}
	}

	public bool TryWriteUInt8(byte value)
	{
		unsafe
		{
			return SDL_WriteU8(mContext, value);
		}
	}

	public bool TryWriteUIntPtrBE(nuint value)
	{
		unsafe
		{
			var swappedValue = Helpers.ToBigEndianUIntPtr(value);

			return SDL_WriteIO(mContext, &swappedValue, unchecked((nuint)Unsafe.SizeOf<nuint>())) == unchecked((nuint)Unsafe.SizeOf<nuint>());
		}
	}

	public bool TryWriteUInt128LE(nuint value)
	{
		unsafe
		{
			var swappedValue = Helpers.ToLittleEndianUIntPtr(value);

			return SDL_WriteIO(mContext, &swappedValue, unchecked((nuint)Unsafe.SizeOf<nuint>())) == unchecked((nuint)Unsafe.SizeOf<nuint>());
		}
	}
}
