namespace Sdl3Sharp.Video.Windowing;

partial interface IDisplay
{
	/// <summary>
	/// Provides property names for <see cref="IDisplay"/> <see cref="Properties">properties</see>
	/// </summary>
	public abstract class PropertyNames
	{
		/// <summary>
		/// The name of a <em>read-only</em> property <see cref="Properties">property</see> that holds a boolean value indicating whether the display has HDR headroom above the SDR white point
		/// </summary>
		/// <remarks>
		/// <para>
		/// The associated property is for informational and diagnostic purposes only, as not all platforms provide this information at the display level.
		/// </para>
		/// </remarks>
		public const string HdrEnabledBoolean = "SDL.display.HDR_enabled";

		private protected PropertyNames() { }
	}
}
