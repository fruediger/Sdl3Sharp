using Sdl3Sharp.Events;

namespace Sdl3Sharp.Video.Windowing;

/// <summary>
/// Provides extension methods and properties for <see cref="SystemTheme"/>
/// </summary>
public static partial class SystemThemeExtensions
{
	extension(SystemTheme)
	{
		/// <summary>
		/// Gets the current system theme
		/// </summary>
		/// <value>
		/// The current system theme
		/// </value>
		/// <remarks>
		/// <para>
		/// To get notified when the system theme changes, you can listen for <see cref="EventType.SystemThemeChanged"/> events or subscribe to the <see cref="Sdl.SystemThemeChanged"/> event.
		/// </para>
		/// <para>
		/// This property should only be accessed from the main thread.
		/// </para>
		/// </remarks>
		public static SystemTheme Current
		{
			get
			{
				return SDL_GetSystemTheme();
			}
		}
	}
}
