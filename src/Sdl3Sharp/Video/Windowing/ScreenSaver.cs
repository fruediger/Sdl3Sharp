using Sdl3Sharp.Internal;

namespace Sdl3Sharp.Video.Windowing;

/// <summary>
/// Provides functionality related to the screen saver
/// </summary>
public static partial class ScreenSaver
{
	/// <summary>
	/// Gets or sets a value indicating whether a screen saver is allowed to blank the screen
	/// </summary>
	/// <value>
	/// A value indicating whether a screen saver is allowed to blank the screen
	/// </value>
	/// <remarks>
	/// <para>
	/// If you disable the screen saver, it will be automatically re-enabled when SDL quits.
	/// </para>
	/// <para>
	/// The screen saver is disabled by default, but you can change this default using the <see cref="Hint.Video.AllowScreenSaver"/> hint.
	/// </para>
	/// </remarks>
	public static bool IsEnabled
	{
		get => SDL_ScreenSaverEnabled();

		set
		{
			if (value)
			{
				SdlErrorHelper.ThrowIfFailed(SDL_EnableScreenSaver());
			}
			else
			{
				SdlErrorHelper.ThrowIfFailed(SDL_DisableScreenSaver());
			}
		}
	}
}
