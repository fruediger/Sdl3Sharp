using System;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Video.Rendering.Drivers;

internal sealed class GenericUnknownDriverFallback : IDriver
{
	static string? IDriver.Name { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => null; }

	static ReadOnlySpan<byte> IDriver.NameAscii { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => []; }

	private GenericUnknownDriverFallback() { }
}
