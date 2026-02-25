using Sdl3Sharp.SourceGeneration;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Sdl3Sharp.Video.Windowing.Drivers;

/// <summary>
/// Represents the SDL offscreen-video windowing driver
/// </summary>
/// <remarks>
/// <para>
/// This windowing backend is the "offscreen-video" driver, which is a special driver that allows SDL to create windows without actually displaying them on the screen.
/// It is primarily intended for testing purposes, and may not support all features of a normal windowing driver.
/// </para>
/// </remarks>
public sealed partial class Offscreen : IWindowingDriver
{
	/// <summary>
	/// The name of the SDL offscreen-video windowing driver
	/// </summary>
	/// <remarks>
	/// <para>
	/// The value of this constant is equal to <c>"offscreen"</c>.
	/// </para>
	/// </remarks>
	public const string Name = "offscreen";

	[NotNull] static string? IWindowingDriver.Name { get => Name; }

	[FormattedConstant($"{Name}\0")] private static partial ReadOnlySpan<byte> NameAscii { get; }

	static ReadOnlySpan<byte> IWindowingDriver.NameAscii { get => NameAscii; }

	private Offscreen() { }
}
