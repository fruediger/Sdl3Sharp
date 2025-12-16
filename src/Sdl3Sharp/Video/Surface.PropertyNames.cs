namespace Sdl3Sharp.Video;

partial class Surface
{
	/// <summary>
	/// Provides property names for <see cref="Surface"/> <see cref="Properties">properties</see>
	/// </summary>
	public static class PropertyNames
	{
		/// <summary>
		/// The name of a <see cref="Properties">property</see> that holds the defining value for 100% diffuse white for HDR10 and floating point <see cref="Surface"/>s
		/// </summary>
		/// <remarks>
		/// <para>
		/// For HDR10 and floating point <see cref="Surface"/>s, the value of the property defines the value of 100% diffuse white, with higher values being displayed in the <see cref="HdrHeadroomFloat">High Dynamic Range headroom</see>.
		/// This defaults to <c>203</c> for HDR10 surfaces and <c>1.0</c> for floating point surfaces.
		/// </para>
		/// </remarks>
		public const string SdrWhitePointFloat = "SDL_PROP_SURFACE_SDR_WHITE_POINT_FLOAT";

		/// <summary>
		/// The name of a <see cref="Properties">property</see> that holds the maximum dynamic range for HDR10 and floating point <see cref="Surface"/>s
		/// </summary>
		/// <remarks>
		/// <para>
		/// For HDR10 and floating point <see cref="Surface"/>s, the value of the property defines the maximum dynamic range used by the content, in terms of the <see cref="SdrWhitePointFloat">SDR white point</see>.
		/// This defaults to <c>0.0</c>, which disables <see cref="TonemapOperatorString">tone mapping</see>.
		/// </para>
		/// </remarks>
		public const string HdrHeadroomFloat = "SDL_PROP_SURFACE_HDR_HEADROOM_FLOAT";

		/// <summary>
		/// The name of a <see cref="Properties">property</see> that holds the expression of the tone mapping operator used when compressing from a higher dynamic range to a lower dynamic range
		/// </summary>
		/// <remarks>
		/// <para>
		/// The value of the property defines the tone mapping operator used when compressing from a <see cref="Surface"/> with high dynamic range to another with lower dynamic range.
		/// Currently this supports the following values:
		/// <list type="bullet">
		///		<item>
		///			<term><c>"chrome"</c></term>
		///			<description>The same tone mapping that Chrome uses for HDR content</description>
		///		</item>
		///		<item>
		///			<term><c>"*=N"</c> where <c>N</c> is a floating point number</term>
		///			<description><c>N</c> is a floating point scale factor applied in linear. E.g. <c>"*=0.5"</c>.</description>
		///		</item>
		///		<item>
		///			<term><c>"none"</c></term>
		///			<description>Tone mapping is disabled</description>
		///		</item>
		/// </list>
		/// This defaults to <c>"chrome"</c>.
		/// </para>
		/// </remarks>
		public const string TonemapOperatorString = "SDL_PROP_SURFACE_TONEMAP_OPERATOR_STRING";

		/// <summary>
		/// The name of a <see cref="Properties">property</see> that holds the hotspot pixel offset from the left edge for a cursor
		/// </summary>
		/// <remarks>
		/// <para>
		/// The value of the property defines the pixel offset to the hotspot from the left edge of the <see cref="Surface"/> used as a cursor.
		/// </para>
		/// </remarks>
		public const string HotspotXNumber = "SDL_PROP_SURFACE_HOTSPOT_X_NUMBER";

		/// <summary>
		/// The name of a <see cref="Properties">property</see> that holds the hotspot pixel offset from the top edge for a cursor
		/// </summary>
		/// <remarks>
		/// <para>
		/// The value of the property defines the pixel offset to the hotspot from the top edge of the <see cref="Surface"/> used as a cursor.
		/// </para>
		/// </remarks>
		public const string HotspotYNumber = "SDL_PROP_SURFACE_HOTSPOT_Y_NUMBER";

#if SDL3_4_0_OR_GREATER
		/// <summary>
		/// The name of a <see cref="Properties">property</see> that holds the rotation angle in degrees for the <see cref="Surface"/>
		/// </summary>
		/// <remarks>
		/// <para>
		/// The value of the property defines the number of degrees a <see cref="Surface"/>'s pixel data is meant to be rotated clockwise to make the image right-side up.
		/// This defaults to <c>0</c>.
		/// </para>
		/// <para>
		/// This is used by the camera API, if a mobile device is oriented differently than what its camera provides (i.e. the camera always provides portrait images but the phone is being held in landscape orientation).
		/// </para>
		/// </remarks>
		public const string RotationFloat = "SDL_PROP_SURFACE_ROTATION_FLOAT";
#endif
	}
}
