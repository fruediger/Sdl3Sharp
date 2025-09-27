using Sdl3Sharp.Utilities;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Sdl3Sharp.IO;

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

	public DynamicMemoryStream() :
#pragma warning disable IDE0034 // Keep it that way for explicitness sake
		this(default(IUnsafeConstructorDispatch?))
#pragma warning restore IDE0034
	{ }

	public long ChunkSize
	{
		get => Properties?.TryGetNumberValue(PropertyNames.ChunkSizeNumber, out var chunkSize) is true
			? chunkSize
			: default;

		set => Properties?.TrySetNumberValue(PropertyNames.ChunkSizeNumber, value);
	}

	public NativeMemoryManager? MemoryManager
	{
		get => Properties?.TryGetPointerValue(PropertyNames.MemoryPointer, out var memory) is true
			? memory
			: default;

		set => Properties?.TrySetNumberValue(PropertyNames.MemoryPointer, value);
	}
}
