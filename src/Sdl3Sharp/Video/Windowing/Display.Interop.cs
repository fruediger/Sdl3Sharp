using Sdl3Sharp.Internal.Interop;
using Sdl3Sharp.SourceGeneration;
using Sdl3Sharp.Video.Drawing;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Video.Windowing;

partial struct Display
{
	/// <summary>
	/// Gets the closest match to the requested display mode
	/// </summary>
	/// <param name="displayID">The instance ID of the display to query</param>
	/// <param name="w">The width in pixels of the desired display mode</param>
	/// <param name="h">The height in pixels of the desired display mode</param>
	/// <param name="refresh_rate">The refresh rate of the desired display mode, or 0.0f for the desktop refresh rate</param>
	/// <param name="include_high_density_modes">Boolean to include high density modes in the search</param>
	/// <param name="closest">A pointer filled in with the closest display mode equal to or larger than the desired mode</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// The available display modes are scanned and closest is filled in with the <c><paramref name="closest"/></c> mode matching the requested mode and returned.
	/// The mode format and refresh rate default to the desktop mode if they are set to 0.
	/// The modes are scanned with size being first priority, format being second priority, and finally checking the refresh rate.
	/// If all the available modes are too small, then false is returned.
	/// </para>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetClosestFullscreenDisplayMode">SDL_GetClosestFullscreenDisplayMode</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_GetClosestFullscreenDisplayMode(uint displayID, int w, int h, float refresh_rate, CBool include_high_density_modes, DisplayMode* closest);

	/// <summary>
	/// Gets information about the current display mode
	/// </summary>
	/// <param name="displayID">The instance ID of the display to query</param>
	/// <returns>Returns a pointer to the desktop display mode or NULL on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// There's a difference between this function and <see href="https://wiki.libsdl.org/SDL3/SDL_GetDesktopDisplayMode">SDL_GetDesktopDisplayMode</see>() when SDL runs fullscreen and has changed the resolution.
	/// In that case this function will return the current display mode, and not the previous native display mode.
	/// </para>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetCurrentDisplayMode">SDL_GetCurrentDisplayMode</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial DisplayMode* SDL_GetCurrentDisplayMode(uint displayID);

	/// <summary>
	/// Get the orientation of a display
	/// </summary>
	/// <param name="displayID">The instance ID of the display to query</param>
	/// <returns>Returns the <see href="https://wiki.libsdl.org/SDL3/SDL_DisplayOrientation">SDL_DisplayOrientation</see> enum value of the display, or <see href="https://wiki.libsdl.org/SDL3/SDL_ORIENTATION_UNKNOWN">SDL_ORIENTATION_UNKNOWN</see> if it isn't available</returns>
	/// <remarks>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetCurrentDisplayOrientation">SDL_GetCurrentDisplayOrientation</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal static partial DisplayOrientation SDL_GetCurrentDisplayOrientation(uint displayID);

	/// <summary>
	/// Gets information about the desktop's display mode
	/// </summary>
	/// <param name="displayID">The instance ID of the display to query</param>
	/// <returns>Returns a pointer to the desktop display mode or NULL on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// There's a difference between this function and <see href="https://wiki.libsdl.org/SDL3/SDL_GetCurrentDisplayMode">SDL_GetCurrentDisplayMode</see>() when SDL runs fullscreen and has changed the resolution.
	/// In that case this function will return the previous native display mode, and not the current display mode.
	/// </para>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetDesktopDisplayMode">SDL_GetDesktopDisplayMode</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial DisplayMode* SDL_GetDesktopDisplayMode(uint displayID);

	/// <summary>
	/// Gets the desktop area represented by a display
	/// </summary>
	/// <param name="displayID">The instance ID of the display to query</param>
	/// <param name="rect">The <see href="https://wiki.libsdl.org/SDL3/SDL_Rect">SDL_Rect</see> structure filled in with the display bounds</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// The primary display is often located at (0,0), but may be placed at a different location depending on monitor layout.
	/// </para>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetDisplayBounds">SDL_GetDisplayBounds</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_GetDisplayBounds(uint displayID, Rect<int>* rect);

	/// <summary>
	/// Gets the content scale of a display
	/// </summary>
	/// <param name="displayID">The instance ID of the display to query</param>
	/// <returns>Returns the content scale of the display, or 0.0f on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// The content scale is the expected scale for content based on the DPI settings of the display.
	/// For example, a 4K display might have a 2.0 (200%) display scale, which means that the user expects UI elements to be twice as big on this display, to aid in readability.
	/// </para>
	/// <para>
	/// After window creation, <see href="https://wiki.libsdl.org/SDL3/SDL_GetWindowDisplayScale">SDL_GetWindowDisplayScale</see>() should be used to query the content scale factor for individual windows instead of querying the display for a window and calling this function,
	/// as the per-window content scale factor may differ from the base value of the display it is on, particularly on high-DPI and/or multi-monitor desktop configurations.
	/// </para>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetDisplayContentScale">SDL_GetDisplayContentScale</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal static partial float SDL_GetDisplayContentScale(uint displayID);

	/// <summary>
	/// Gets the display containing a point
	/// </summary>
	/// <param name="point">The point to query</param>
	/// <returns>Returns the instance ID of the display containing the point or 0 on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetDisplayForPoint">SDL_GetDisplayForPoint</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial uint SDL_GetDisplayForPoint(Point<int>* point);

	/// <summary>
	/// Gets the display primarily containing a rect
	/// </summary>
	/// <param name="rect">The rect to query</param>
	/// <returns>Returns the instance ID of the display entirely containing the rect or closest to the center of the rect on success or 0 on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetDisplayForRect">SDL_GetDisplayForRect</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial uint SDL_GetDisplayForRect(Rect<int>* rect);

	/// <summary>
	/// Gets the name of a display in UTF-8 encoding
	/// </summary>
	/// <param name="displayID">Tthe instance ID of the display to query</param>
	/// <returns>Returns the name of a display or NULL on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetDisplayName">SDL_GetDisplayName</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial byte* SDL_GetDisplayName(uint displayID);

	/// <summary>
	/// Gets the properties associated with a display
	/// </summary>
	/// <param name="displayID">The instance ID of the display to query</param>
	/// <returns>Returns a valid property ID on success or 0 on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// The following read-only properties are provided by SDL:
	/// <list type="bullet">
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_DISPLAY_HDR_ENABLED_BOOLEAN"><c>SDL_PROP_DISPLAY_HDR_ENABLED_BOOLEAN</c></see></term>
	///			<description>True if the display has HDR headroom above the SDR white point. This is for informational and diagnostic purposes only, as not all platforms provide this information at the display level.</description>
	///		</item>
	/// </list>
	/// On KMS/DRM:
	/// <list type="bullet">
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_DISPLAY_KMSDRM_PANEL_ORIENTATION_NUMBER"><c>SDL_PROP_DISPLAY_KMSDRM_PANEL_ORIENTATION_NUMBER</c></see></term>
	///			<description>The "panel orientation" property for the display in degrees of clockwise rotation. Note that this is provided only as a hint, and the application is responsible for any coordinate transformations needed to conform to the requested display orientation.</description>
	///		</item>
	/// </list>
	/// On Wayland:
	/// <list type="bullet">
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_DISPLAY_WAYLAND_WL_OUTPUT_POINTER"><c>SDL_PROP_DISPLAY_WAYLAND_WL_OUTPUT_POINTER</c></see></term>
	///			<description>the wl_output associated with the display</description>
	///		</item>
	/// </list>
	/// On Windows:
	/// <list type="bullet">
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_DISPLAY_WINDOWS_HMONITOR_POINTER"><c>SDL_PROP_DISPLAY_WINDOWS_HMONITOR_POINTER</c></see></term>
	///			<description>the monitor handle (HMONITOR) associated with the display</description>
	///		</item>
	/// </list>
	/// </para>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetDisplayProperties">SDL_GetDisplayProperties</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial uint SDL_GetDisplayProperties(uint displayID);

	/// <summary>
	/// Gets a list of currently connected displays
	/// </summary>
	/// <param name="count">A pointer filled in with the number of displays returned, may be NULL</param>
	/// <returns>
	/// Returns a 0 terminated array of display instance IDs or NULL on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information.
	/// This should be freed with <see href="https://wiki.libsdl.org/SDL3/SDL_free">SDL_free</see>() when it is no longer needed.
	/// </returns>
	/// <remarks>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetDisplays">SDL_GetDisplays</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial uint* SDL_GetDisplays(int* count);

	/// <summary>
	/// Gets the usable desktop area represented by a display, in screen coordinates
	/// </summary>
	/// <param name="displayID">The instance ID of the display to query</param>
	/// <param name="rect">The <see href="https://wiki.libsdl.org/SDL3/SDL_Rect">SDL_Rect</see> structure filled in with the display bounds</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// This is the same area as <see href="https://wiki.libsdl.org/SDL3/SDL_GetDisplayBounds">SDL_GetDisplayBounds</see>() reports, but with portions reserved by the system removed.
	/// For example, on Apple's macOS, this subtracts the area occupied by the menu bar and dock.
	/// </para>
	/// <para>
	/// Setting a window to be fullscreen generally bypasses these unusable areas, so these are good guidelines for the maximum space available to a non-fullscreen window.
	/// </para>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetDisplayUsableBounds">SDL_GetDisplayUsableBounds</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_GetDisplayUsableBounds(uint displayID, Rect<int>* rect);

	/// <summary>
	/// Gets a list of fullscreen display modes available on a display
	/// </summary>
	/// <param name="displayID">The instance ID of the display to query</param>
	/// <param name="count">A pointer filled in with the number of display modes returned, may be NULL</param>
	/// <returns>
	/// Returns a NULL terminated array of display mode pointers or NULL on failure; <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information.
	/// This is a single allocation that should be freed <see href="https://wiki.libsdl.org/SDL3/SDL_free">SDL_free</see>() when it is no longer needed.
	/// </returns>
	/// <remarks>
	/// <para>
	/// The display modes are sorted in this priority:
	/// <list type="bullet">
	/// <item><description>w -> largest to smallest</description></item> 
	/// <item><description>h -> largest to smallest</description></item>
	/// <item><description>bits per pixel -> more colors to fewer colors</description></item>
	/// <item><description>packed pixel layout -> largest to smallest</description></item>
	/// <item><description>refresh rate -> highest to lowest</description></item>
	/// <item><description>pixel density -> lowest to highest</description></item>
	/// </list>
	/// </para>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetFullscreenDisplayModes">SDL_GetFullscreenDisplayModes</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial DisplayMode** SDL_GetFullscreenDisplayModes(uint displayID, int* count);

	/// <summary>
	/// Gets the orientation of a display when it is unrotated
	/// </summary>
	/// <param name="displayID">The instance ID of the display to query</param>
	/// <returns>Returns the <see href="https://wiki.libsdl.org/SDL3/SDL_DisplayOrientation">SDL_DisplayOrientation</see> enum value of the display, or <see href="https://wiki.libsdl.org/SDL3/SDL_ORIENTATION_UNKNOWN">SDL_ORIENTATION_UNKNOWN</see> if it isn't available</returns>
	/// <remarks>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetNaturalDisplayOrientation">SDL_GetNaturalDisplayOrientation</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal static partial DisplayOrientation SDL_GetNaturalDisplayOrientation(uint displayID);

	/// <summary>
	/// Return the primary display
	/// </summary>
	/// <returns>Returns the instance ID of the primary display on success or 0 on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetPrimaryDisplay">SDL_GetPrimaryDisplay</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal static partial uint SDL_GetPrimaryDisplay();
}
