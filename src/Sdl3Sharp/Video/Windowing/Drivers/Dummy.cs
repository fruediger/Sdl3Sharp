using Sdl3Sharp.SourceGeneration;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Sdl3Sharp.Video.Windowing.Drivers;

/// <summary>
/// Represents the SDL "dummy video" (sometimes called "null video") windowing driver
/// </summary>
public sealed partial class Dummy : IWindowingDriver
{
	/// <summary>
	/// The name of the "dummy video" (sometimes called "null video") windowing driver
	/// </summary>
	/// <remarks>
	/// <para>
	/// The value of this constant is equal to <c>"dummy"</c>.
	/// </para>
	/// </remarks>
	public const string Name = "dummy";

	[NotNull] static string? IWindowingDriver.Name { get => Name; }

	[FormattedConstant($"{Name}\0")] private static partial ReadOnlySpan<byte> NameAscii { get; }

	static ReadOnlySpan<byte> IWindowingDriver.NameAscii { get => NameAscii; }

	private Dummy() { }
}
