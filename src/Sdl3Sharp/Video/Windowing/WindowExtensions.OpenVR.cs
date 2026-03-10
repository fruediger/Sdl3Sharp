using Sdl3Sharp.Video.Windowing.Drivers;

namespace Sdl3Sharp.Video.Windowing;

partial class WindowExtensions
{
	extension(Window<OpenVR>.PropertyNames)
	{
		/// <summary>
		/// The name of a <em>read-only</em> <see cref="Window.Properties">property</see> that holds
		/// the OpenVR overlay handle ID associated for the associated overlay window
		/// </summary>
		public static string OpenVROverlayIdNumber => "SDL.window.create.openvr.overlay_id";
	}

	extension(Window<OpenVR> window)
	{
		/// <summary>
		/// Gets the OpenVR overlay handle ID associated for the associated overlay window
		/// </summary>
		/// <value>
		/// The OpenVR overlay handle ID associated for the associated overlay window
		/// </value>
		/// <remarks>
		/// <para>
		/// This property should only be accessed from the main thread.
		/// </para>
		/// </remarks>
		public ulong OpenVROverlayId => window?.Properties?.TryGetNumberValue(Window<OpenVR>.PropertyNames.OpenVROverlayIdNumber, out var openVROverlayId) is true
			? unchecked((ulong)openVROverlayId)
			: default;
	}
}
