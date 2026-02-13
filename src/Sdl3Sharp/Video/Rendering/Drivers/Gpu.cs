#if SDL3_4_0_OR_GREATER

using Sdl3Sharp.SourceGeneration;
using Sdl3Sharp.SourceGeneration.RegisterDriver;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Video.Rendering.Drivers;

/// <summary>
/// Represents the GPU rendering driver
/// </summary>
/// <remarks>
/// <para>
/// The GPU rendering backend uses the SDL GPU render API.
/// </para>
/// </remarks>
[RegisterDriver(Name)]
public sealed partial class Gpu : IDriver
{
	/// <summary>
	/// The name of the GPU rendering driver
	/// </summary>
	/// <remarks>
	/// <para>
	/// The value of this constant is equal to <c>"gpu"</c>.
	/// </para>
	/// </remarks>
	public const string Name = "gpu";

	[NotNull] static string? IDriver.Name { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => Name; }

	[FormattedConstant($"{Name}\0")] private static partial ReadOnlySpan<byte> NameAscii { get; }

	static ReadOnlySpan<byte> IDriver.NameAscii { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => NameAscii; }

	private Gpu() { }
}

#endif