namespace Sdl3Sharp.Video.Windowing;

partial struct Display
{
	/// <summary>
	/// Provides property names for <see cref="Display"/> <see cref="Properties">properties</see>
	/// </summary>
	public static class PropertyNames
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

		/// <summary>
		/// The name of a <em>read-only</em> property <see cref="Properties">property</see> that holds the "panel orientation" of the display in degrees of clockwise rotation
		/// </summary>
		/// <remarks>
		/// <para>
		/// The associated property is provided only as a hint, and the application is responsible for any coordinate transformations needed to conform to the requested display orientation.
		/// </para>
		/// </remarks>
		public const string KmsDrmPanelOrientationNumber = "SDL.display.KMSDRM.panel_orientation";

		/// <summary>
		/// The name of a <em>read-only</em> property <see cref="Properties">property</see> that holds a pointer to the Wayland <c><see href="https://wayland.freedesktop.org/docs/html/apa.html#protocol-spec-wl_output">wl_output</see></c> associated with the display
		/// </summary>
		public const string WaylandWlOutputPointer = "SDL.display.Wayland.wl_output";

		/// <summary>
		/// The name of a <em>read-only</em> property <see cref="Properties">property</see> that holds the Windows <c><see href="https://learn.microsoft.com/en-us/windows/win32/gdi/hmonitor-and-the-device-context">HMONITOR</see></c> handle associated with the display
		/// </summary>
		public const string WindowsHMonitorPointer = "SDL.display.windows.hmonitor";
	}
}
