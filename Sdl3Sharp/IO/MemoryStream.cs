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
			// TODO: fail
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
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private unsafe static IntPtr ValidateUnsafeMemory(void* mem)
	{
		if (mem is null)
		{
			// TODO: fail
		}

		return unchecked((IntPtr)mem);
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

	public unsafe MemoryStream(void* mem, nuint size, MemoryStreamFreeFunc? free = default) :
		this(ValidateUnsafeMemory(mem), size, default(IUnsafeConstructorDispatch?))
	{
		
	}

	public IntPtr Memory

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
