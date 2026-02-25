namespace Sdl3Sharp.Video.Rendering.Drivers;

/// <summary>
/// Provides extension methods and properties for <see cref="IRenderingDriver"/> implementing rendering driver types
/// </summary>
public static class RenderingDriverExtensions
{
	private static class Cache<TDriver>
		where TDriver : IRenderingDriver
	{
		// The available rendering driver list never changes during the lifetime of the application,
		// and, in addition to that, all pre-defined rendering driver types never change their name (it's essentially a compile-time constant),
		// so that's why we can cache the availability of each driver statically here.
		public static bool? IsAvailable;
	}

	extension<TDriver>(TDriver)
		where TDriver : IRenderingDriver
	{
		/// <summary>
		/// Gets a value indicating whether the rendering driver is available in the current environment
		/// </summary>
		/// <value>
		/// A value indicating whether the rendering driver is available in the current environment
		/// </value>
		/// <remarks>
		/// <para>
		/// The availability of a certain rendering driver is only checked once and then cached afterwards,
		/// so the value of this property for individual rendering drivers won't change during the lifetime of the application.
		/// </para>
		/// <para>
		/// This property effectively checks whether or not the name of the rendering driver is present in <see cref="IRenderingDriver.AvailableDriverNames"/>.
		/// </para>
		/// </remarks>
		public static bool IsAvailable => Cache<TDriver>.IsAvailable ??= TDriver.Name switch { string name => IRenderingDriver.AvailableDriverNames.Contains(name), _ => false };
	}
}
