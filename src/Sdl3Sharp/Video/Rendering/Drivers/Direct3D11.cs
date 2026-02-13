using Sdl3Sharp.SourceGeneration;
using Sdl3Sharp.SourceGeneration.RegisterDriver;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Video.Rendering.Drivers;

/// <summary>
/// Represents the Direct3D 11 rendering driver
/// </summary>
/// <remarks>
/// <para>
/// This rendering backend is only available on Windows platforms.
/// </para>
/// </remarks>
[RegisterDriver(Name)]
public sealed partial class Direct3D11 : IDriver
{
	/// <summary>
	/// The name of the Direct3D 11 rendering driver
	/// </summary>
	/// <remarks>
	/// <para>
	/// The value of this constant is equal to <c>"direct3d11"</c>.
	/// </para>
	/// </remarks>
	public const string Name = "direct3d11";

	[NotNull] static string? IDriver.Name { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => Name; }

	[FormattedConstant($"{Name}\0")] private static partial ReadOnlySpan<byte> NameAscii { get; }

	static ReadOnlySpan<byte> IDriver.NameAscii { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => NameAscii; }


	private Direct3D11() { }
}
