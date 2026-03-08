using Sdl3Sharp.Video.Windowing.Drivers;

namespace Sdl3Sharp.Video.Windowing;

partial class WindowExtensions
{
	extension(Window<OpenVR>.PropertyNames)
	{
		public static string OpenVROverlayIdNumber => "SDL.window.create.openvr.overlay_id";
	}

	extension(Window<OpenVR> window)
	{
		public ulong OpenVROverlayId => window?.Properties?.TryGetNumberValue(Window<OpenVR>.PropertyNames.OpenVROverlayIdNumber, out var openVROverlayId) is true
			? unchecked((ulong)openVROverlayId)
			: default;
	}
}
