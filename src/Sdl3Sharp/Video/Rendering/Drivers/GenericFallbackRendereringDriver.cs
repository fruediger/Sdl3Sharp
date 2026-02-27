using System;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Video.Rendering.Drivers;

internal sealed class GenericFallbackRendereringDriver : IRenderingDriver
{
	static string? IRenderingDriver.Name { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => null; }

	static ReadOnlySpan<byte> IRenderingDriver.NameAscii { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => []; }

	private GenericFallbackRendereringDriver() { }
}
