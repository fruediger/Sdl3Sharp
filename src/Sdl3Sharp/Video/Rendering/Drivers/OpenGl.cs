using Sdl3Sharp.SourceGeneration;
using Sdl3Sharp.SourceGeneration.RegisterDriver;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Video.Rendering.Drivers;

/// <summary>
/// Represents the OpenGL rendering driver
/// </summary>
[RegisterDriver(Name)]
public sealed partial class OpenGL : IRenderingDriver
{
	/// <summary>
	/// The name of the OpenGL rendering driver
	/// </summary>
	/// <remarks>
	/// <para>
	/// The value of this constant is equal to <c>"opengl"</c>.
	/// </para>
	/// </remarks>
	public const string Name = "opengl";

	[NotNull] static string? IRenderingDriver.Name { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => Name; }

	[FormattedConstant($"{Name}\0")] private static partial ReadOnlySpan<byte> NameAscii { get; }

	static ReadOnlySpan<byte> IRenderingDriver.NameAscii { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => NameAscii; }

	private OpenGL() { }
}
