using Sdl3Sharp.SourceGeneration;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Sdl3Sharp.Video.Windowing.Drivers;

/// <summary>
/// Represents the Raspberry Pi windowing driver
/// </summary>
/// <remarks>
/// <para>
/// This windowing backend is only available on <see href="https://www.raspberrypi.org/">Raspberry Pi</see> platforms.
/// </para>
/// </remarks>
public sealed partial class RaspberryPi : IWindowingDriver
{
	/// <summary>
	/// The name of the Raspberry Pi windowing driver
	/// </summary>
	/// <remarks>
	/// <para>
	/// The value of this constant is equal to <c>"rpi"</c>.
	/// </para>
	/// </remarks>
	public const string Name = "rpi";

	[NotNull] static string? IWindowingDriver.Name { get => Name; }

	[FormattedConstant($"{Name}\0")] private static partial ReadOnlySpan<byte> NameAscii { get; }

	static ReadOnlySpan<byte> IWindowingDriver.NameAscii { get => NameAscii; }

	private RaspberryPi() { }
}
