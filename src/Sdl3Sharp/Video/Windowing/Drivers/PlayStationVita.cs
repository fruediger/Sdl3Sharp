using Sdl3Sharp.SourceGeneration;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Video.Windowing.Drivers;

/// <summary>
/// Represents the Sony PlayStation Vita windowing driver
/// </summary>
/// <remarks>
/// <para>
/// This windowing backend is experimental and is <em>not supported</em>.
/// </para>
/// </remarks>
public sealed partial class PlayStationVita : IWindowingDriver
{
	/// <summary>
	/// The name of the Sony PlayStation Vita windowing driver
	/// </summary>
	/// <remarks>
	/// <para>
	/// The value of this constant is equal to <c>"vita"</c>.
	/// </para>
	/// </remarks>
	public const string Name = "vita";

	[NotNull] static string? IWindowingDriver.Name { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => Name; }

	[FormattedConstant($"{Name}\0")] private static partial ReadOnlySpan<byte> NameAscii { get; }

	static ReadOnlySpan<byte> IWindowingDriver.NameAscii { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => NameAscii; }

	private PlayStationVita() { }
}