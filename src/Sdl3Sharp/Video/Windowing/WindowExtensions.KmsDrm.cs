using Sdl3Sharp.Video.Windowing.Drivers;
using System;

namespace Sdl3Sharp.Video.Windowing;

partial class WindowExtensions
{
	extension(Window<KmsDrm>.PropertyNames)
	{
		public static string KmsDrmDeviceIndexNumber => "SDL.window.create.kmsdrm.dev_index";

		public static string KmsDrmDrmFdNumber => "SDL.window.kmsdrm.drm_fd";

		public static string KmsDrmGbmDevicePointer => "SDL.window.kmsdrm.gbm_dev";
	}

	extension(Window<KmsDrm> window)
	{
		public uint KmsDrmDeviceIndex => window?.Properties?.TryGetNumberValue(Window<KmsDrm>.PropertyNames.KmsDrmDeviceIndexNumber, out var kmsDrmDeviceIndex) is true
			? unchecked((uint)kmsDrmDeviceIndex)
			: default;

		public int KmsDrmDrmFileDescriptor => window?.Properties?.TryGetNumberValue(Window<KmsDrm>.PropertyNames.KmsDrmDrmFdNumber, out var kmsDrmDrmFd) is true
			? unchecked((int)kmsDrmDrmFd)
			: default;

		public IntPtr KmsDrmGbmDevice => window?.Properties?.TryGetPointerValue(Window<KmsDrm>.PropertyNames.KmsDrmGbmDevicePointer, out var kmsDrmGbmDevicePtr) is true
			? kmsDrmGbmDevicePtr
			: default;
	}
}
