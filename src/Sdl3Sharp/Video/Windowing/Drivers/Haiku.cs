using Sdl3Sharp.SourceGeneration;
using Sdl3Sharp.SourceGeneration.RegisterDriver;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Video.Windowing.Drivers;

/// <summary>
/// Represents the Haiku windowing driver
/// </summary>
/// <remarks>
/// <para>
/// This windowing backend is only available on <see href="https://www.haiku-os.org/">Haiku OS</see> platforms.
/// </para>
/// </remarks>
[RegisterDriver(Name)]
public sealed partial class Haiku : IWindowingDriver
{
	/// <summary>
	/// The name of the Haiku windowing driver
	/// </summary>
	/// <remarks>
	/// <para>
	/// The value of this constant is equal to <c>"haiku"</c>.
	/// </para>
	/// </remarks>
	public const string Name = "haiku";

	[NotNull] static string? IWindowingDriver.Name { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => Name; }

	[FormattedConstant($"{Name}\0")] private static partial ReadOnlySpan<byte> NameAscii { get; }

	static ReadOnlySpan<byte> IWindowingDriver.NameAscii { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => NameAscii; }

	private Haiku() { }
}