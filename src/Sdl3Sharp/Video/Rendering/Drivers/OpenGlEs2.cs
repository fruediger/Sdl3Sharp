using Sdl3Sharp.SourceGeneration;
using Sdl3Sharp.SourceGeneration.RegisterDriver;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Video.Rendering.Drivers;

/// <summary>
/// Represents the OpenGL ES 2 rendering driver
/// </summary>
/// <remarks>
/// <para>
/// This rendering backend is primarily only available on mobile and embedded platforms, but may be available on some desktop platforms as well.
/// </para>
/// </remarks>
[RegisterDriver(Name)]
public sealed partial class OpenGLEs2 : IRenderingDriver
{
	/// <summary>
	/// The name of the OpenGL ES 2 rendering driver
	/// </summary>
	/// <remarks>
	/// <para>
	/// The value of this constant is equal to <c>"opengles2"</c>.
	/// </para>
	/// </remarks>
	public const string Name = "opengles2";

	[NotNull] static string? IRenderingDriver.Name { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => Name; }

	[FormattedConstant($"{Name}\0")] private static partial ReadOnlySpan<byte> NameAscii { get; }

	static ReadOnlySpan<byte> IRenderingDriver.NameAscii { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => NameAscii; }

	private OpenGLEs2() { }
}
