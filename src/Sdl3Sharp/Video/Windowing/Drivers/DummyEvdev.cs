using Sdl3Sharp.SourceGeneration;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Video.Windowing.Drivers;

/// <summary>
/// Represents the SDL "dummy video" (sometimes called "null video") windowing driver with <see href="https://en.wikipedia.org/wiki/Evdev">evdev</see>
/// </summary>
/// <remarks>
/// <para>
/// This windowing backend is only available on platforms that support <see href="https://en.wikipedia.org/wiki/Evdev">evdev</see>.
/// </para>
/// </remarks>
public sealed partial class DummyEvdev : IWindowingDriver
{
	/// <summary>
	/// The name of the "dummy video" (sometimes called "null video") windowing driver with <see href="https://en.wikipedia.org/wiki/Evdev">evdev</see>
	/// </summary>
	/// <remarks>
	/// <para>
	/// The value of this constant is equal to <c>"evdev"</c>.
	/// </para>
	/// </remarks>
	public const string Name = "evdev";

	[NotNull] static string? IWindowingDriver.Name { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => Name; }

	[FormattedConstant($"{Name}\0")] private static partial ReadOnlySpan<byte> NameAscii { get; }

	static ReadOnlySpan<byte> IWindowingDriver.NameAscii { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => NameAscii; }

	private DummyEvdev() { }
}
