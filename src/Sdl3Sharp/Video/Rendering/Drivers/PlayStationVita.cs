using Sdl3Sharp.SourceGeneration;
using Sdl3Sharp.SourceGeneration.RegisterDriver;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Video.Rendering.Drivers;

/// <summary>
/// Represents the Sony PlayStation Vita rendering driver
/// </summary>
/// <remarks>
/// <para>
/// This rendering backend is experimental and is <em>not supported</em>.
/// </para>
/// </remarks>
// TODO: make SDL3001 the diagnostic id for "unsupported rendering driver"
// TODO: add message to the Experimental attribute
[Experimental("SDL3001")]
[RegisterDriver(Name)]
public sealed partial class PlayStationVita : IRenderingDriver
{
	/// <summary>
	/// The name of the Sony PlayStation Vita rendering driver
	/// </summary>
	/// <remarks>
	/// <para>
	/// The value of this constant is equal to <c>"VITA gxm"</c>.
	/// </para>
	/// </remarks>
	public const string Name = "VITA gxm";

	[NotNull] static string? IRenderingDriver.Name { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => Name; }

	[FormattedConstant($"{Name}\0")] private static partial ReadOnlySpan<byte> NameAscii { get; }

	static ReadOnlySpan<byte> IRenderingDriver.NameAscii { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => NameAscii; }

	private PlayStationVita() { }
}
