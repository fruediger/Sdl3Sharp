using System;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Video.Windowing.Drivers;

internal sealed class GenericFallbackWindowingDriver : IWindowingDriver
{
	static string? IWindowingDriver.Name { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => null; }

	static ReadOnlySpan<byte> IWindowingDriver.NameAscii { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => []; }

	private GenericFallbackWindowingDriver() { }
}
