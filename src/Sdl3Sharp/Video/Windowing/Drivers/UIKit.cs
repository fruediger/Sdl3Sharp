using Sdl3Sharp.SourceGeneration;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Sdl3Sharp.Video.Windowing.Drivers;

/// <summary>
/// Represents the UIKit windowing driver
/// </summary>
/// <remarks>
/// <para>
/// This windowing backend is only available on Apple platforms (macOS, iOS, tvOS and watchOS).
/// </para>
/// </remarks>
public sealed partial class UIKit : IWindowingDriver
{
	/// <summary>
	/// The name of the UIKit windowing driver
	/// </summary>
	/// <remarks>
	/// <para>
	/// The value of this constant is equal to <c>"uikit"</c>.
	/// </para>
	/// </remarks>
	public const string Name = "uikit";

	[NotNull] static string? IWindowingDriver.Name { get => Name; }

	[FormattedConstant($"{Name}\0")] private static partial ReadOnlySpan<byte> NameAscii { get; }

	static ReadOnlySpan<byte> IWindowingDriver.NameAscii { get => NameAscii; }

	private UIKit() { }
}
