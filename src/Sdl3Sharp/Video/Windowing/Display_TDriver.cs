using Sdl3Sharp.Video.Windowing.Drivers;
using System.Diagnostics.CodeAnalysis;

namespace Sdl3Sharp.Video.Windowing;

/// <summary>
/// Represents a display connected to the system
/// </summary>
/// <typeparam name="TDriver">The windowing driver associated with this display</typeparam>
/// <remarks>
/// <para>
/// The <see cref="Display.Id"/> of a display is unique, remains unchanged while the display is connected to the system, and is never reused for the lifetime of the application.
/// If a display is disconnected and then reconnected, it will get assigned a new <see cref="Display.Id"/>.
/// </para>
/// <para>
/// For the most part <see cref="Display{TDriver}"/>s are not thread-safe, and most of their properties and methods should only be accessed from the main thread!
/// </para>
/// <para>
/// <see cref="Display{TDriver}"/>s are concrete display types, associated with a specific windowing driver.
/// </para>
/// <para>
/// If you want to use them in a more general way, you can use them as <see cref="Display"/> instances, which serve as common abstractions.
/// </para>
/// </remarks>
public sealed partial class Display<TDriver> : Display
	where TDriver : IWindowingDriver
{
	internal Display(uint displayId, bool register) :
		base(displayId, register)
	{ }

	internal static bool TryGetOrCreate(uint displayId, [NotNullWhen(true)] out Display<TDriver>? result)
		=> Display.TryGetOrCreate(displayId, out result);
}
