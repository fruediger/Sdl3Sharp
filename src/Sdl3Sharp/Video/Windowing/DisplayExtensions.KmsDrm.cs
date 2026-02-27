using Sdl3Sharp.Video.Windowing.Drivers;

namespace Sdl3Sharp.Video.Windowing;

partial class DisplayExtensions
{
	extension(Display<KmsDrm>.PropertyNames)
	{
		/// <summary>
		/// The name of a <em>read-only</em> property <see cref="Properties">property</see> that holds the "panel orientation" of the display in degrees of clockwise rotation
		/// </summary>
		/// <remarks>
		/// <para>
		/// The associated property is provided only as a hint, and the application is responsible for any coordinate transformations needed to conform to the requested display orientation.
		/// </para>
		/// </remarks>
		public static string KmsDrmPanelOrientationNumber => "SDL.display.KMSDRM.panel_orientation";
	}

	extension(Display<KmsDrm> display)
	{
		/// <summary>
		/// Gets the "panel orientation" of the display
		/// </summary>
		/// <value>
		/// The "panel orientation" of the display, in degrees of clockwise rotation
		/// </value>
		/// <remarks>
		/// <para>
		/// This property is provided only as a hint, and the application is responsible for any coordinate transformations needed to conform to the requested display orientation.
		/// </para>
		/// <para>
		/// This property should only be accessed from the main thread.
		/// </para>
		/// </remarks>
		public long KmsDrmPanelOrientation => display?.Properties?.TryGetNumberValue(Display<KmsDrm>.PropertyNames.KmsDrmPanelOrientationNumber, out var orientation) is true
			? orientation
			: default;
	}
}
