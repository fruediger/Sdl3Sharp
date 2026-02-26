using Sdl3Sharp.SourceGeneration;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Video.Windowing.Drivers;

/// <summary>
/// Represents the QNX windowing driver
/// </summary>
/// <remarks>
/// <para>
/// This windowing backend is experimental and is <em>not supported</em>.
/// </para>
/// </remarks>
public sealed partial class Qnx : IWindowingDriver
{
	/// <summary>
	/// The name of the QNX windowing driver
	/// </summary>
	/// <remarks>
	/// <para>
	/// The value of this constant is equal to <c>"qnx"</c>.
	/// </para>
	/// </remarks>
	public const string Name = "qnx";

	[NotNull] static string? IWindowingDriver.Name { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => Name; }

	[FormattedConstant($"{Name}\0")] private static partial ReadOnlySpan<byte> NameAscii { get; }

	static ReadOnlySpan<byte> IWindowingDriver.NameAscii { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => NameAscii; }

	private Qnx() { }
}
