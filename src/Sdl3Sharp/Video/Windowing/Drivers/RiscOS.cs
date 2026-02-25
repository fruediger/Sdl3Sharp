using Sdl3Sharp.SourceGeneration;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Sdl3Sharp.Video.Windowing.Drivers;

/// <summary>
/// Represents the RISC OS windowing driver
/// </summary>
/// <remarks>
/// <para>
/// This windowing backend is only available on <see href="https://en.wikipedia.org/wiki/RISC_OS">RISC OS</see> platforms.
/// </para>
/// </remarks>
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

	[NotNull] static string? IWindowingDriver.Name { get => Name; }

	[FormattedConstant($"{Name}\0")] private static partial ReadOnlySpan<byte> NameAscii { get; }

	static ReadOnlySpan<byte> IWindowingDriver.NameAscii { get => NameAscii; }

	private RiscOS() { }
}
