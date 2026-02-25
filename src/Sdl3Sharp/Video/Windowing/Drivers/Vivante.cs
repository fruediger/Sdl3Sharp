using Sdl3Sharp.SourceGeneration;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Sdl3Sharp.Video.Windowing.Drivers;

/// <summary>
/// Represents the Vivante EGL windowing driver
/// </summary>
public sealed partial class Vivante : IWindowingDriver
{
	/// <summary>
	/// The name of the Vivante EGL windowing driver
	/// </summary>
	/// <remarks>
	/// <para>
	/// The value of this constant is equal to <c>"vivante"</c>.
	/// </para>
	/// </remarks>
	public const string Name = "vivante";

	[NotNull] static string? IWindowingDriver.Name { get => Name; }

	[FormattedConstant($"{Name}\0")] private static partial ReadOnlySpan<byte> NameAscii { get; }

	static ReadOnlySpan<byte> IWindowingDriver.NameAscii { get => NameAscii; }

	private Vivante() { }
}