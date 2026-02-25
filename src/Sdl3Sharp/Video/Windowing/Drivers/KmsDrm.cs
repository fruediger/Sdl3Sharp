using Sdl3Sharp.SourceGeneration;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Sdl3Sharp.Video.Windowing.Drivers;

/// <summary>
/// Represents the Linux KMS/DRM windowing driver
/// </summary>
/// <remarks>
/// <para>
/// This windowing backend is only available on Linux platforms with <see href="https://www.kernel.org/doc/html/latest/gpu/introduction.html">Kernel Mode Setting (KMS) and Direct Rendering Manager (DRM)</see> support.
/// </para>
/// </remarks>
public sealed partial class KmsDrm : IWindowingDriver
{
	/// <summary>
	/// The name of the Linux KMS/DRM windowing driver
	/// </summary>
	/// <remarks>
	/// <para>
	/// The value of this constant is equal to <c>"kmsdrm"</c>.
	/// </para>
	/// </remarks>
	public const string Name = "kmsdrm";

	[NotNull] static string? IWindowingDriver.Name { get => Name; }

	[FormattedConstant($"{Name}\0")] private static partial ReadOnlySpan<byte> NameAscii { get; }

	static ReadOnlySpan<byte> IWindowingDriver.NameAscii { get => NameAscii; }

	private KmsDrm() { }
}
