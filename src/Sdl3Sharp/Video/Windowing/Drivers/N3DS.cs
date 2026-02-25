using Sdl3Sharp.SourceGeneration;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Sdl3Sharp.Video.Windowing.Drivers;

/// <summary>
/// Represents the Nintendo 3DS windowing driver
/// </summary>
/// <remarks>
/// <para>
/// This windowing backend is experimental and is <em>not supported</em>.
/// </para>
/// </remarks>
// TODO: make SDL3002 the diagnostic id for "unsupported windowing driver"
// TODO: add message to the Experimental attribute
[Experimental("SDL3002")]
public sealed partial class N3DS : IWindowingDriver
{
	/// <summary>
	/// The name of the Nintendo 3DS windowing driver
	/// </summary>
	/// <remarks>
	/// <para>
	/// The value of this constant is equal to <c>"n3ds"</c>.
	/// </para>
	/// </remarks>
	public const string Name = "n3ds";

	[NotNull] static string? IWindowingDriver.Name { get => Name; }

	[FormattedConstant($"{Name}\0")] private static partial ReadOnlySpan<byte> NameAscii { get; }

	static ReadOnlySpan<byte> IWindowingDriver.NameAscii { get => NameAscii; }

	private N3DS() { }
}
