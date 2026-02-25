using Sdl3Sharp.SourceGeneration;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Sdl3Sharp.Video.Windowing.Drivers;

/// <summary>
/// Represents the Wayland windowing driver
/// </summary>
/// <remarks>
/// <para>
/// This windowing backend is only available on platforms that support the Wayland display server protocol.
/// </para>
/// </remarks>
public sealed partial class Wayland : IWindowingDriver
{
	/// <summary>
	/// The name of the Wayland windowing driver
	/// </summary>
	/// <remarks>
	/// <para>
	/// The value of this constant is equal to <c>"wayland"</c>.
	/// </para>
	/// </remarks>
	public const string Name = "wayland";

	[NotNull] static string? IWindowingDriver.Name { get => Name; }

	[FormattedConstant($"{Name}\0")] private static partial ReadOnlySpan<byte> NameAscii { get; }

	static ReadOnlySpan<byte> IWindowingDriver.NameAscii { get => NameAscii; }

	private Wayland() { }
}