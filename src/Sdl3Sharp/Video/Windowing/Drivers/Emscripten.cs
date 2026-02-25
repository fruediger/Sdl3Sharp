using Sdl3Sharp.SourceGeneration;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Sdl3Sharp.Video.Windowing.Drivers;

/// <summary>
/// Represents the Emscripten windowing driver
/// </summary>
/// <remarks>
/// <para>
/// This windowing backend is only available on <see href="https://emscripten.org/">Emscripten</see> platforms.
/// </para>
/// </remarks>
public sealed partial class Emscripten : IWindowingDriver
{
	/// <summary>
	/// The name of the Emscripten windowing driver
	/// </summary>
	/// <remarks>
	/// <para>
	/// The value of this constant is equal to <c>"emscripten"</c>.
	/// </para>
	/// </remarks>
	public const string Name = "emscripten";

	[NotNull] static string? IWindowingDriver.Name { get => Name; }

	[FormattedConstant($"{Name}\0")] private static partial ReadOnlySpan<byte> NameAscii { get; }

	static ReadOnlySpan<byte> IWindowingDriver.NameAscii { get => NameAscii; }

	private Emscripten() { }
}
