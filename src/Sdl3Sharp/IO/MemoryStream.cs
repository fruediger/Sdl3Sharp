using Sdl3Sharp.Utilities;
using System;
using System.Buffers;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sdl3Sharp.IO;

/// <summary>
/// A stream that is backed by a provided memory buffer
/// </summary>
public sealed partial class MemoryStream : Stream
{	
	private interface IUnsafeConstructorDispatch;

	/// <exception cref="ArgumentException"><paramref name="nativeMemory"/> is invalid</exception>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private static IntPtr ValidateAndPinNativeMemory(Utilities.NativeMemory nativeMemory, out NativeMemoryPin nativeMemoryPin)
	{
		if (!nativeMemory.IsValid)
		{
			failNativeMemoryArgumentInvalid();
		}

		nativeMemoryPin = nativeMemory.Pin();

		return nativeMemory.Pointer;

		[DoesNotReturn]
		static void failNativeMemoryArgumentInvalid() => throw new ArgumentException(message: $"{nameof(nativeMemory)} is invalid", paramName: nameof(nativeMemory));
	}

	/// <exception cref="InvalidOperationException"><paramref name="memory"/> could not be pinned</exception>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private unsafe static IntPtr ValidateAndPinMemory(Memory<byte> memory, out MemoryHandle memoryHandle)
	{
		memoryHandle = memory.Pin();

		return unchecked((IntPtr)memoryHandle.Pointer);
	}

	private NativeMemoryPin? mNativeMemoryPin;
	private MemoryHandle mMemoryHandle;

	private unsafe MemoryStream(IntPtr mem, nuint size, IUnsafeConstructorDispatch? _ = default) :
		base(SDL_IOFromMem(unchecked((void*)mem), size))
	{ }

	/// <summary>
	/// Creates a new <see cref="MemoryStream"/> from a specified <see cref="Utilities.NativeMemory">allocated memory buffer</see>
	/// </summary>
	/// <param name="nativeMemory">The <see cref="Utilities.NativeMemory">allocated memory buffer</see> to use for the stream</param>
	/// <remarks>
	/// This pins the specified <see cref="Utilities.NativeMemory">allocated memory buffer</see> for the lifetime of the <see cref="MemoryStream"/>.
	/// </remarks>
	/// <inheritdoc cref="ValidateAndPinNativeMemory(Utilities.NativeMemory, out NativeMemoryPin)"/>
	public MemoryStream(Utilities.NativeMemory nativeMemory) :
#pragma warning disable IDE0034 // Keep it that way for explicitness sake
		this(ValidateAndPinNativeMemory(nativeMemory, out var nativeMemoryPin), nativeMemory.Length, default(IUnsafeConstructorDispatch?))
#pragma warning restore IDE0034
	{
		mNativeMemoryPin = nativeMemoryPin;
	}

	/// <summary>
	/// Creates a new <see cref="MemoryStream"/> from a specified <see cref="Memory{T}"/>
	/// </summary>
	/// <param name="memory">The memory to use for the stream</param>
	/// <remarks>
	/// <para>
	/// This pins the specified <see cref="Memory{T}"/> for the lifetime of the <see cref="MemoryStream"/>.
	/// </para>
	/// </remarks>
	/// <inheritdoc cref="ValidateAndPinMemory(Memory{byte}, out MemoryHandle)"/>
	public MemoryStream(Memory<byte> memory) :
#pragma warning disable IDE0034 // Keep it that way for explicitness sake
		this(ValidateAndPinMemory(memory, out var memoryHandle), unchecked((nuint)memory.Length), default(IUnsafeConstructorDispatch?))
#pragma warning restore IDE0034
	{
		mMemoryHandle = memoryHandle;
	}

	/// <summary>
	/// Creates a new <see cref="MemoryStream"/> from a specified unmanaged memory and size
	/// </summary>
	/// <param name="mem">A pointer to the unmanaged memory to use for the stream</param>
	/// <param name="size">The size of the unmanaged memory</param>
	/// <param name="freeFunc">An optional delegate that is called to free the unmanaged memory when the stream is <see cref="Stream.TryClose">closed</see> or <see cref="Stream.Dispose()">disposed</see></param>
	/// <remarks>
	/// <para>
	/// If <paramref name="freeFunc"/> is unset or <c><see langword="null"/></c>, the unmanaged memory will not be freed when the stream is <see cref="Stream.TryClose">closed</see> or <see cref="Stream.Dispose()">disposed</see>.
	/// This parameter is reflected by the <see cref="FreeFunc"/> property. Changing the value of the <see cref="FreeFunc"/> property after construction also changes the automatic freeing behavior.
	/// </para>
	/// </remarks>
	public unsafe MemoryStream(void* mem, nuint size, MemoryStreamFreeFunc? freeFunc = null) :
#pragma warning disable IDE0034 // Keep it that way for explicitness sake
		this(unchecked((IntPtr)mem), size, default(IUnsafeConstructorDispatch?))
#pragma warning restore IDE0034
	{
		if (freeFunc is not null)
		{
			FreeFunc = freeFunc;
		}
	}

	/// <summary>Calls to this property are not supported</summary>
	/// <value>Not supported</value>
	/// <exception cref="NotSupportedException">always</exception>
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("Calls to this property are not supported. This property will always throw an exception. Use the Length property instead.")]
#pragma warning disable CS0809
	protected sealed override long LengthCore
#pragma warning restore CS0809
	{
		[DoesNotReturn]
		get => throw new NotSupportedException("Calls to this property are not supported.");
	}

	/// <summary>Calls to this method are not supported</summary>
	/// <param name="offset">Not supported</param>
	/// <param name="whence">Not supported</param>
	/// <returns>Not supported</returns>
	/// <exception cref="NotSupportedException">always</exception>	
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("Calls to this method are not supported. This method will always throw an exception. Use the TrySeek(long, StreamWhence, out long) method instead.")]
	[DoesNotReturn]
#pragma warning disable CS0809
	protected sealed override long SeekCore(long offset, StreamWhence whence) => throw new NotSupportedException("Calls to this method are not supported.");
#pragma warning restore CS0809

	/// <summary>Calls to this method are not supported</summary>
	/// <param name="data">Not supported</param>
	/// <param name="status">Not supported</param>
	/// <returns>Not supported</returns>
	/// <exception cref="NotSupportedException">always</exception>	
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("Calls to this method are not supported. This method will always throw an exception. Use the TryRead(NativeMemory, out nuint) method instead.")]
	[DoesNotReturn]
#pragma warning disable CS0809
	protected sealed override nuint ReadCore(Utilities.NativeMemory data, ref StreamStatus status) => throw new NotSupportedException("Calls to this method are not supported.");
#pragma warning restore CS0809

	/// <summary>Calls to this method are not supported</summary>
	/// <param name="data">Not supported</param>
	/// <param name="status">Not supported</param>
	/// <returns>Not supported</returns>
	/// <exception cref="NotSupportedException">always</exception>	
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("Calls to this method are not supported. This method will always throw an exception. Use the TryWrite(ReadOnlyNativeMemory, out nuint) method instead.")]
	[DoesNotReturn]
#pragma warning disable CS0809
	protected sealed override nuint WriteCore(ReadOnlyNativeMemory data, ref StreamStatus status) => throw new NotSupportedException("Calls to this method are not supported.");
#pragma warning restore CS0809

	/// <summary>Calls to this method are not supported</summary>
	/// <param name="status">Not supported</param>
	/// <returns>Not supported</returns>
	/// <exception cref="NotSupportedException">always</exception>	
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("Calls to this method are not supported. This method will always throw an exception. Use the TryFlush() method instead.")]
	[DoesNotReturn]
#pragma warning disable CS0809
	protected sealed override bool FlushCore(ref StreamStatus status) => throw new NotSupportedException("Calls to this method are not supported.");
#pragma warning restore CS0809

	/// <summary>Calls to this method are not supported</summary>
	/// <returns>Not supported</returns>
	/// <exception cref="NotSupportedException">always</exception>	
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("Calls to this method are not supported. This method will always throw an exception. Use the TryClose() method instead.")]
	[DoesNotReturn]
#pragma warning disable CS0809
	protected sealed override bool CloseCore() => throw new NotSupportedException("Calls to this method are not supported.");
#pragma warning restore CS0809

	/// <summary>
	/// Gets a pointer to the memory buffer that the <see cref="MemoryStream"/> was initialized with
	/// </summary>
	/// <value>
	/// A pointer to the memory buffer that the <see cref="MemoryStream"/> was initialized with
	/// </value>
	/// <seealso cref="PropertyNames.MemoryPointer"/>
	public IntPtr Memory => Properties?.TryGetPointerValue(PropertyNames.MemoryPointer, out var memory) is true
		? memory
		: default;

	/// <summary>
	/// Gets the size, in bytes, of the memory buffer that the <see cref="MemoryStream"/> was initialized with
	/// </summary>
	/// <value>
	/// The size, in bytes, of the memory buffer that the <see cref="MemoryStream"/> was initialized with
	/// </value>
	/// <seealso cref="PropertyNames.SizeNumber"/>
	public nuint Size => Properties?.TryGetNumberValue(PropertyNames.SizeNumber, out var size) is true
		? unchecked((nuint)size)
		: default;

	/// <summary>
	/// Gets or sets a delegate that will be called to free the memory buffer when the <see cref="MemoryStream"/> is <see cref="Stream.TryClose">closed</see> or <see cref="Stream.Dispose()">disposed</see>
	/// </summary>
	/// <value>
	/// A delegate that will be called to free the memory buffer when the <see cref="MemoryStream"/> is <see cref="Stream.TryClose">closed</see> or <see cref="Stream.Dispose()">disposed</see>
	/// </value>
	/// <remarks>
	/// <para>
	/// If the value of this property is <c><see langword="null"/></c>, the memory buffer will not be freed when the <see cref="MemoryStream"/> is <see cref="Stream.TryClose">closed</see> or <see cref="Stream.Dispose()">disposed</see>.
	/// Changing the value of this property will also change the automatic freeing behavior.
	/// </para>
	/// </remarks>
	public MemoryStreamFreeFunc? FreeFunc
	{
		get => Properties?.TryGetPointerValue(PropertyNames.FreeFuncPointer, out var freeFunc) is true
			? Marshal.GetDelegateForFunctionPointer<MemoryStreamFreeFunc>(freeFunc)
			: null;

		set => Properties?.TrySetPointerValue(PropertyNames.FreeFuncPointer, value is not null
			? Marshal.GetFunctionPointerForDelegate(value)
			: 0);
	}

	/// <inheritdoc/>
	protected override void Dispose(bool disposing, bool close)
	{
		unsafe
		{
			base.Dispose(disposing, close);

			if (mNativeMemoryPin is not null)
			{
				mNativeMemoryPin.Dispose();

				mNativeMemoryPin = null;
			}

			if (mMemoryHandle.Pointer is not null)
			{
				mMemoryHandle.Dispose();
			}
		}
	}
}
