using Sdl3Sharp.SourceGeneration;
using Sdl3Sharp.SourceGeneration.RegisterDriver;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Video.Windowing.Drivers;

/// <summary>
/// Represents the Raspberry Pi windowing driver
/// </summary>
/// <remarks>
/// <para>
/// This windowing backend is only available on <see href="https://www.raspberrypi.org/">Raspberry Pi</see> platforms.
/// </para>
/// </remarks>
[RegisterDriver(Name)]
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

	[NotNull] static string? IWindowingDriver.Name { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => Name; }

	[FormattedConstant($"{Name}\0")] private static partial ReadOnlySpan<byte> NameAscii { get; }

	static ReadOnlySpan<byte> IWindowingDriver.NameAscii { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => NameAscii; }

	private RaspberryPi() { }
}
