using Sdl3Sharp.Video.Windowing.Drivers;
using System;

namespace Sdl3Sharp.Video.Windowing;

partial class WindowExtensions
{
	extension(Window<KmsDrm>.PropertyNames)
	{
		/// <summary>
		/// The name of a <em>read-only</em> <see cref="Window.Properties">property</see> that holds
		/// the device index of the KMS/DRM device associated with the window
		/// </summary>
		/// <remarks>
		/// <para>
		/// E.g., the value of the associated property is the <c>X</c> in <c>/dev/dri/cardX</c>.
		/// </para>
		/// </remarks>
		public static string KmsDrmDeviceIndexNumber => "SDL.window.create.kmsdrm.dev_index";
		
		/// <summary>
		/// The name of a <em>read-only</em> <see cref="Window.Properties">property</see> that holds
		/// the DRM file descriptor associated with the window
		/// </summary>
		public static string KmsDrmDrmFdNumber => "SDL.window.kmsdrm.drm_fd";

		/// <summary>
		/// The name of a <em>read-only</em> <see cref="Window.Properties">property</see> that holds
		/// a pointer to the GBM device associated with the window
		/// </summary>
		public static string KmsDrmGbmDevicePointer => "SDL.window.kmsdrm.gbm_dev";
	}

	extension(Window<KmsDrm> window)
	{
		/// <summary>
		/// Gets the device index of the KMS/DRM device associated with this window
		/// </summary>
		/// <value>
		/// The device index of the KMS/DRM device associated with this window
		/// </value>
		/// <remarks>
		/// <para>
		/// E.g., the value of this property is the <c>X</c> in <c>/dev/dri/cardX</c>.
		/// </para>
		/// <para>
		/// This property should only be accessed from the main thread.
		/// </para>
		/// </remarks>
		public uint KmsDrmDeviceIndex => window?.Properties?.TryGetNumberValue(Window<KmsDrm>.PropertyNames.KmsDrmDeviceIndexNumber, out var kmsDrmDeviceIndex) is true
			? unchecked((uint)kmsDrmDeviceIndex)
			: default;

		/// <summary>
		/// Gets the DRM file descriptor associated with this window
		/// </summary>
		/// <value>
		/// The DRM file descriptor associated with this window
		/// </value>
		/// <remarks>
		/// <para>
		/// This property should only be accessed from the main thread.
		/// </para>
		/// </remarks>
		public int KmsDrmDrmFileDescriptor => window?.Properties?.TryGetNumberValue(Window<KmsDrm>.PropertyNames.KmsDrmDrmFdNumber, out var kmsDrmDrmFd) is true
			? unchecked((int)kmsDrmDrmFd)
			: default;

		/// <summary>
		/// Gets a pointer to the GBM device associated with this window
		/// </summary>
		/// <value>
		/// A pointer to the GBM device associated with this window
		/// </value>
		/// <remarks>
		/// <para>
		/// This property should only be accessed from the main thread.
		/// </para>
		/// </remarks>
		public IntPtr KmsDrmGbmDevice => window?.Properties?.TryGetPointerValue(Window<KmsDrm>.PropertyNames.KmsDrmGbmDevicePointer, out var kmsDrmGbmDevicePtr) is true
			? kmsDrmGbmDevicePtr
			: default;
	}
}
