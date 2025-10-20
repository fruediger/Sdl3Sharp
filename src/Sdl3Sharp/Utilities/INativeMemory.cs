using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Utilities;

internal interface INativeMemory : IEquatable<ReadOnlyNativeMemory>
{
	internal ReadOnlyNativeMemory AsReadOnlyNativeMemory { get; }
}

internal interface INativeMemory<TSelf> : INativeMemory, IEqualityOperators<TSelf, ReadOnlyNativeMemory, bool>
	where TSelf : struct, INativeMemory<TSelf>
{
	public static abstract implicit operator ReadOnlyNativeMemory(TSelf nativeMemory);

	ReadOnlyNativeMemory INativeMemory.AsReadOnlyNativeMemory { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => (TSelf)this; }
}
