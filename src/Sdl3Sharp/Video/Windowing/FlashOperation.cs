namespace Sdl3Sharp.Video.Windowing;

/// <summary>
/// Represents the operation to perform when <see cref="IWindow.TryFlash(Sdl3Sharp.Video.Windowing.FlashOperation)">flashing</see> a <see cref="IWindow"/>
/// </summary>
public enum FlashOperation
{
	/// <summary>Cancels any window flash states</summary>
	Cancel,

	/// <summary>Flashes the window briefly to get the user's attention</summary>
	Briefly,

	/// <summary>Flashes the window until it gets focused by the user</summary>
	UntilFocused
}
