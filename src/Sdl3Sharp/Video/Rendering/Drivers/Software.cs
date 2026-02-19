using Sdl3Sharp.SourceGeneration;
using Sdl3Sharp.SourceGeneration.RegisterDriver;
using Sdl3Sharp.Video.Windowing;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Video.Rendering.Drivers;

/// <summary>
/// Represents the software rendering driver
/// </summary>
/// <remarks>
/// <para>
/// This rendering backend is not hardware-accelerated, but is supported almost everywhere.
/// A software renderer is also needed when rendering should be done to a <see cref="Surface"/> directly instead of to a <see cref="Window"/> or to an off-screen render target.
/// </para>
/// </remarks>
[RegisterDriver(Name)]
public sealed partial class Software : IDriver
{
	/// <summary>
	/// The name of the software rendering driver
	/// </summary>
	/// <remarks>
	/// <para>
	/// The value of this constant is equal to <c>"software"</c>.
	/// </para>
	/// </remarks>
	public const string Name = "software";

	[NotNull] static string? IDriver.Name { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => Name; }

	[FormattedConstant($"{Name}\0")] private static partial ReadOnlySpan<byte> NameAscii { get; }

	static ReadOnlySpan<byte> IDriver.NameAscii { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => NameAscii; }

	private Software() { }
}
