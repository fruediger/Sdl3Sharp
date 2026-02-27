using Sdl3Sharp.SourceGeneration;
using Sdl3Sharp.SourceGeneration.RegisterDriver;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Video.Windowing.Drivers;

/// <summary>
/// Represents the Vivante EGL windowing driver
/// </summary>
[RegisterDriver(Name)]
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

	[NotNull] static string? IWindowingDriver.Name { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => Name; }

	[FormattedConstant($"{Name}\0")] private static partial ReadOnlySpan<byte> NameAscii { get; }

	static ReadOnlySpan<byte> IWindowingDriver.NameAscii { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => NameAscii; }

	private Vivante() { }
}