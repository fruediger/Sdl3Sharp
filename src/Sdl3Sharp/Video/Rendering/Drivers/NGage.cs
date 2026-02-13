using Sdl3Sharp.SourceGeneration;
using Sdl3Sharp.SourceGeneration.RegisterDriver;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Video.Rendering.Drivers;

/// <summary>
/// Represents the Nokia N-Gage rendering driver
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
public sealed partial class NGage : IDriver
{
	/// <summary>
	/// The name of the Nokia N-Gage rendering driver
	/// </summary>
	/// <remarks>
	/// <para>
	/// The value of this constant is equal to <c>"N-Gage"</c>.
	/// </para>
	/// </remarks>
	public const string Name = "N-Gage";

	[NotNull] static string? IDriver.Name { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => Name; }

	[FormattedConstant($"{Name}\0")] private static partial ReadOnlySpan<byte> NameAscii { get; }

	static ReadOnlySpan<byte> IDriver.NameAscii { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => NameAscii; }

	private NGage() { }
}
