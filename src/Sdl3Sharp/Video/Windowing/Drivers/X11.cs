using Sdl3Sharp.SourceGeneration;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Sdl3Sharp.Video.Windowing.Drivers;

/// <summary>
/// Represents the X11 windowing driver
/// </summary>
/// <remarks>
/// <para>
/// This windowing backend is only available on platforms that support the X11 display server protocol.
/// </para>
/// </remarks>
public sealed partial class X11 : IWindowingDriver
{
	/// <summary>
	/// The name of the X11 windowing driver
	/// </summary>
	/// <remarks>
	/// <para>
	/// The value of this constant is equal to <c>"x11"</c>.
	/// </para>
	/// </remarks>
	public const string Name = "x11";

	[NotNull] static string? IWindowingDriver.Name { get => Name; }

	[FormattedConstant($"{Name}\0")] private static partial ReadOnlySpan<byte> NameAscii { get; }

	static ReadOnlySpan<byte> IWindowingDriver.NameAscii { get => NameAscii; }

	private X11() { }
}