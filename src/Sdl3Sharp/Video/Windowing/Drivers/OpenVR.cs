using Sdl3Sharp.SourceGeneration;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Sdl3Sharp.Video.Windowing.Drivers;

/// <summary>
/// Represents the OpenVR windowing driver
/// </summary>
/// <remarks>
/// <para>
/// This windowing backend is only available on platforms that support <see href="https://en.wikipedia.org/wiki/OpenVR">OpenVR</see>.
/// </para>
/// </remarks>
public sealed partial class OpenVR : IWindowingDriver
{
	/// <summary>
	/// The name of the OpenVR windowing driver
	/// </summary>
	/// <remarks>
	/// <para>
	/// The value of this constant is equal to <c>"openvr"</c>.
	/// </para>
	/// </remarks>
	public const string Name = "openvr";

	[NotNull] static string? IWindowingDriver.Name { get => Name; }

	[FormattedConstant($"{Name}\0")] private static partial ReadOnlySpan<byte> NameAscii { get; }

	static ReadOnlySpan<byte> IWindowingDriver.NameAscii { get => NameAscii; }

	private OpenVR() { }
}
