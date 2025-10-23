using Sdl3Sharp.Utilities;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Sdl3Sharp.IO;

/// <summary>
/// A stream that is backed by dynamically allocated memory
/// </summary>
public sealed partial class DynamicMemoryStream : Stream
{
	private interface IUnsafeConstructorDispatch;

	private unsafe DynamicMemoryStream(IUnsafeConstructorDispatch? _ = default) : base(SDL_IOFromDynamicMem())
	{
		if (Context is null)
		{
			failCouldNotCreateDynamicMemoryStream();
		}

		[DoesNotReturn]
		static void failCouldNotCreateDynamicMemoryStream() => throw new SdlException($"Could not create the {nameof(DynamicMemoryStream)}");
	}

	/// <summary>
	/// Creates a new <see cref="DynamicMemoryStream"/>
	/// </summary>
	public DynamicMemoryStream() :
#pragma warning disable IDE0034 // Keep it that way for explicitness sake
		this(default(IUnsafeConstructorDispatch?))
#pragma warning restore IDE0034
	{ }

	/// <summary>
	/// Gets or sets the chunk size used for memory allocations
	/// </summary>
	/// <value>
	/// The chunk size used for memory allocations
	/// </value>
	/// <remarks>
	/// <para>
	/// Memory will be allocated in multiples of this size, defaulting to <c>1024</c>.
	/// </para>
	/// </remarks>
	/// <seealso cref="PropertyNames.ChunkSizeNumber"/>
	public uint ChunkSize
	{
		get => Properties?.TryGetNumberValue(PropertyNames.ChunkSizeNumber, out var chunkSize) is true
			? unchecked((uint)chunkSize) // 'uint' should be okay here; nobody in their right mind would set a chunk size larger than 'uint.MaxValue'
			: default;

		set => Properties?.TrySetNumberValue(PropertyNames.ChunkSizeNumber, value);
	}

	/// <summary>
	/// Gets or sets the pointer to the internal memory of the stream
	/// </summary>
	/// <value>
	/// The pointer to the internal memory of the stream
	/// </value>
	/// <remarks>
	/// <para>
	/// This property can be set to <c><see cref="System.IntPtr.Zero"/></c> to transfer ownership of the memory to the application, which should free the memory with <see cref="Utilities.NativeMemory.Free(void*)"/>.
	/// If this is done, the next operation on the stream must be <see cref="Stream.TryClose"/> or better <see cref="Stream.Dispose()"/>.
	/// Don't try to attempt this unless you really know what you are doing.
	/// </para>
	/// <para>
	/// For a safer way to transfer ownership of the memory, use <see cref="TryGetMemoryManagerAndDispose(out Utilities.NativeMemoryManager?)"/> instead.
	/// </para>
	/// </remarks>
	/// <seealso cref="PropertyNames.MemoryPointer"/>
	public IntPtr Memory
	{
		get => Properties?.TryGetPointerValue(PropertyNames.MemoryPointer, out var memory) is true
			? memory
			: default;

		set => Properties?.TrySetNumberValue(PropertyNames.MemoryPointer, value);
	}

	/// <summary>
	/// Tries to transfer ownership of the internal memory of the stream to a <see cref="NativeMemoryManager"/>, disposing the stream in the process
	/// </summary>
	/// <param name="memoryManager">The <see cref="NativeMemoryManager"/> that owns the memory, when this method returns <c><see langword="true"/></c>; otherwise, <c><see langword="null"/></c></param>
	/// <returns><c><see langword="true"/></c>, if the memory ownership was successfully transferred to a new <see cref="NativeMemoryManager"/>; otherwise, <c><see langword="false"/></c></returns>
	/// <remarks>
	/// <para>
	/// If this method returns <c><see langword="true"/></c>, the stream is disposed and no longer usable. The stream does not get disposed, if this method returns <c><see langword="false"/></c>.
	/// </para>
	/// <para>
	/// The resulting <see cref="NativeMemoryManager"/> should be <see cref="NativeMemoryManager.Dispose()">disposed</see> when the memory it's managing is no longer needed. That also frees the allocated memory.
	/// </para>
	/// </remarks>
	public bool TryGetMemoryManagerAndDispose([NotNullWhen(true)] out NativeMemoryManager? memoryManager)
	{
		unsafe
		{
			var length = Length;

			if (!(Properties is { } properties
				&& properties.TryGetPointerValue(PropertyNames.MemoryPointer, out var memory)
				&& properties.TrySetPointerValue(PropertyNames.MemoryPointer, 0)))
			{
				memoryManager = null;
				return false;
			}

			memoryManager = new(unchecked((void*)memory), (nuint)length, &NativeMemory.SDL_free);

			Dispose();

			return true;
		}
	}
}
