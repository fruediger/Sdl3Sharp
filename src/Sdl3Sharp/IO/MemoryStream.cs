using Sdl3Sharp.Utilities;
using System;
using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sdl3Sharp.IO;

public sealed partial class MemoryStream : Stream
{	
	private interface IUnsafeConstructorDispatch;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private unsafe static IntPtr ValidateAndPinMemory(Memory<byte> memory, out MemoryHandle memoryHandle)
	{
		memoryHandle = memory.Pin();

		if (memoryHandle.Pointer is null)
		{
			memoryHandle.Dispose();

			memoryHandle = default;

			failCouldNotPinMemory();
		}

		return unchecked((IntPtr)memoryHandle.Pointer);

		[DoesNotReturn]
		static void failCouldNotPinMemory() => throw new InvalidOperationException($"Could not pin the {nameof(memory)}");
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private static IntPtr ValidateAndPinNativeMemory(Utilities.NativeMemory nativeMemory, out NativeMemoryPin? nativeMemoryPin)
	{
		if (!nativeMemory.IsValid)
		{
			failNativeMemoryArgumentInvalid();
		}

		nativeMemoryPin = nativeMemory.Pin();

		try
		{
			return nativeMemory.Pointer;
		}
		catch
		{
			nativeMemoryPin.Dispose();

			nativeMemoryPin = null;

			throw;
		}

		[DoesNotReturn]
		static void failNativeMemoryArgumentInvalid() => throw new ArgumentException(message: $"{nameof(nativeMemory)} is invalid", paramName: nameof(nativeMemory));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private unsafe static IntPtr ValidateUnsafeMemory(void* mem)
	{
		if (mem is null)
		{
			failMemArgumentNull();
		}

		return unchecked((IntPtr)mem);

		[DoesNotReturn]
		static void failMemArgumentNull() => throw new ArgumentNullException(nameof(mem));
	}

	private MemoryHandle mMemoryHandle;
	private NativeMemoryPin? mNativeMemoryPin;

	private unsafe MemoryStream(IntPtr mem, nuint size, IUnsafeConstructorDispatch? _ = default) :
		base(SDL_IOFromMem(unchecked((void*)mem), size))
	{ }

	public MemoryStream(Memory<byte> memory) :
		this(ValidateAndPinMemory(memory, out var memoryHandle), unchecked((nuint)memory.Length), default(IUnsafeConstructorDispatch?))
	{
		mMemoryHandle = memoryHandle;
	}

	public MemoryStream(Utilities.NativeMemory nativeMemory) :
		this(ValidateAndPinNativeMemory(nativeMemory, out var nativeMemoryPin), nativeMemory.Length, default(IUnsafeConstructorDispatch?))
	{
		mNativeMemoryPin = nativeMemoryPin;
	}

	public unsafe MemoryStream(void* mem, nuint size, MemoryStreamFreeFunc? freeFunc = null) :
		this(ValidateUnsafeMemory(mem), size, default(IUnsafeConstructorDispatch?))
	{
		FreeFunc = freeFunc;
	}

	public IntPtr Memory => Properties?.TryGetPointerValue(PropertyNames.MemoryPointer, out var memory) is true
		? memory
		: default;

	public long Size => Properties?.TryGetNumberValue(PropertyNames.SizeNumber, out var size) is true
		? size
		: default;

	public MemoryStreamFreeFunc? FreeFunc
	{
		get => Properties?.TryGetPointerValue(PropertyNames.FreeFuncPointer, out var freeFunc) is true
			? Marshal.GetDelegateForFunctionPointer<MemoryStreamFreeFunc>(freeFunc)
			: null;

		set => Properties?.TrySetPointerValue(PropertyNames.FreeFuncPointer, value is not null
			? Marshal.GetFunctionPointerForDelegate(value)
			: 0);
	}

	protected override void Dispose(bool disposing, bool close)
	{
		unsafe
		{
			base.Dispose(disposing, close);

			if (mMemoryHandle.Pointer is not null)
			{
				mMemoryHandle.Dispose();
			}

			if (mNativeMemoryPin is not null)
			{
				mNativeMemoryPin.Dispose();

				mNativeMemoryPin = null;
			}
		}
	}
}
