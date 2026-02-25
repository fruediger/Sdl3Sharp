using Sdl3Sharp.SourceGeneration;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Sdl3Sharp.Video.Windowing.Drivers;

/// <summary>
/// Represents the Android windowing driver
/// </summary>
/// <remarks>
/// <para>
/// This windowing backend is only available on Android platforms.
/// </para>
/// </remarks>
public sealed partial class Android : IWindowingDriver
{
	/// <summary>
	/// The name of the Android windowing driver
	/// </summary>
	/// <remarks>
	/// <para>
	/// The value of this constant is equal to <c>"android"</c>.
	/// </para>
	/// </remarks>
	public const string Name = "android";

	[NotNull] static string? IWindowingDriver.Name { get => Name; }

	[FormattedConstant($"{Name}\0")] private static partial ReadOnlySpan<byte> NameAscii { get; }

	static ReadOnlySpan<byte> IWindowingDriver.NameAscii { get => NameAscii; }

	private Android() { }
}