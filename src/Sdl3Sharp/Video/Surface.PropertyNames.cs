namespace Sdl3Sharp.Video;

partial class Surface
{
	public static class PropertyNames
	{
		public const string SdrWhitePointFloat = "SDL_PROP_SURFACE_SDR_WHITE_POINT_FLOAT";

		public const string HdrHeadroomFloat = "SDL_PROP_SURFACE_HDR_HEADROOM_FLOAT";

		public const string TonemapOperatorString = "SDL_PROP_SURFACE_TONEMAP_OPERATOR_STRING";

		public const string HotspotXNumber = "SDL_PROP_SURFACE_HOTSPOT_X_NUMBER";

		public const string HotspotYNumber = "SDL_PROP_SURFACE_HOTSPOT_Y_NUMBER";

#if SDL3_4_0_OR_GREATER
		public const string RotationFloat = "SDL_PROP_SURFACE_ROTATION_FLOAT";
#endif
	}
}
