using Sdl3Sharp.SourceGeneration;
using Sdl3Sharp.SourceGeneration.RegisterDriver;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Video.Windowing.Drivers;

/// <summary>
/// Represents the RISC OS windowing driver
/// </summary>
/// <remarks>
/// <para>
/// This windowing backend is only available on <see href="https://en.wikipedia.org/wiki/RISC_OS">RISC OS</see> platforms.
/// </para>
/// </remarks>
[RegisterDriver(Name)]
public sealed partial class RiscOS : IWindowingDriver
{
	/// <summary>
	/// The name of the RISC OS windowing driver
	/// </summary>
	/// <remarks>
	/// <para>
	/// The value of this constant is equal to <c>"riscos"</c>.
	/// </para>
	/// </remarks>
	public const string Name = "riscos";

	[NotNull] static string? IWindowingDriver.Name { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => Name; }

	[FormattedConstant($"{Name}\0")] private static partial ReadOnlySpan<byte> NameAscii { get; }

	static ReadOnlySpan<byte> IWindowingDriver.NameAscii { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => NameAscii; }

	private RiscOS() { }
}
