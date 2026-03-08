using Sdl3Sharp.Video.Windowing.Drivers;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Video.Windowing;

partial class WindowExtensions
{
	extension(Window<UIKit>.PropertyNames)
	{
		public static string CreateUIKitWindowScenePointer => "SDL.window.create.uikit.windowscene";

		public static string UIKitWindowPointer => "SDL.window.uikit.window";

		public static string UIKitMetalViewTagNumber => "SDL.window.uikit.metal_view_tag";

		public static string UIKitOpenGLFrameBufferNumber => "SDL.window.uikit.opengl.framebuffer";

		public static string UIKitOpenGLRenderBufferNumber => "SDL.window.uikit.opengl.renderbuffer";

		public static string UIKitOpenGLResolveFrameBufferNumber => "SDL.window.uikit.opengl.resolve_framebuffer";
	}

	extension(Window<UIKit>)
	{
		public static bool TryCreate([NotNullWhen(true)] out Window<UIKit>? window, bool? alwaysOnTop = default, bool? bordered = default, bool? constrainPopup = default, bool? externalGraphicsContext = default, bool? focusable = default,
			bool? fullscreen = default, int? height = default, bool? hidden = default, bool? highPixelDensity = default, bool? maximized = default, bool? menu = default, bool? metal = default, bool? minimized = default,
			bool? modal = default, bool? mouseGrabbed = default, bool? openGL = default, Window? parent = default, bool? resizable = default, string? title = default, bool? transparent = default, bool? tooltip = default,
			bool? utility = default, bool? vulkan = default, int? width = default, int? x = default, int? y = default,
			IntPtr? uiKitWindowScene = default, Properties? properties = default)
		{
			if (!UIKit.IsActive)
			{
				window = null;
				return false;
			}

			Properties propertiesUsed;
			Unsafe.SkipInit(out IntPtr? uiKitWindowSceneBackup);

			if (properties is null)
			{
				propertiesUsed = [];

				if (uiKitWindowScene is IntPtr uiKitWindowSceneValue)
				{
					propertiesUsed.TrySetPointerValue(Window<UIKit>.PropertyNames.CreateUIKitWindowScenePointer, uiKitWindowSceneValue);
				}
			}
			else
			{
				propertiesUsed = properties;

				if (uiKitWindowScene is IntPtr uiKitWindowSceneValue)
				{
					uiKitWindowSceneBackup = propertiesUsed.TryGetPointerValue(Window<UIKit>.PropertyNames.CreateUIKitWindowScenePointer, out var exisitingUIKitWindowScene) is true
						? exisitingUIKitWindowScene
						: null;

					propertiesUsed.TrySetPointerValue(Window<UIKit>.PropertyNames.CreateUIKitWindowScenePointer, uiKitWindowSceneValue);
				}
			}

			try
			{
				return Window.TryCreateUnchecked(
					out window, alwaysOnTop, bordered, constrainPopup, externalGraphicsContext, focusable,
					fullscreen, height, hidden, highPixelDensity, maximized, menu, metal, minimized,
					modal, mouseGrabbed, openGL, parent, resizable, title, transparent, tooltip,
					utility, vulkan, width, x, y, propertiesUsed
				);
			}
			finally
			{
				if (properties is null)
				{
					// propertiesUsed was just a temporary instance we created for this call, so we need to dispose it now

					propertiesUsed.Dispose();
				}
				else
				{
					// we restored the original properties values from the given properties instance

					if (uiKitWindowScene.HasValue)
					{
						if (uiKitWindowSceneBackup is IntPtr uiKitWindowSceneValue)
						{
							propertiesUsed.TrySetPointerValue(Window<UIKit>.PropertyNames.CreateUIKitWindowScenePointer, uiKitWindowSceneValue);
						}
						else
						{
							propertiesUsed.TryRemove(Window<UIKit>.PropertyNames.CreateUIKitWindowScenePointer);
						}
					}
				}
			}
		}
	}

	extension(Window<UIKit> window)
	{
		public IntPtr UIKitWindow => window?.Properties?.TryGetPointerValue(Window<UIKit>.PropertyNames.UIKitWindowPointer, out var uiKitWindowPtr) is true
			? uiKitWindowPtr
			: default;

		public nint UIKitMetalViewTag => window?.Properties?.TryGetNumberValue(Window<UIKit>.PropertyNames.UIKitMetalViewTagNumber, out var uiKitMetalViewTag) is true
			? unchecked((nint)uiKitMetalViewTag)
			: default;

		public uint UIKitOpenGLFrameBuffer => window?.Properties?.TryGetNumberValue(Window<UIKit>.PropertyNames.UIKitOpenGLFrameBufferNumber, out var uiKitOpenGLFrameBuffer) is true
			? unchecked((uint)uiKitOpenGLFrameBuffer)
			: default;

		public uint UIKitOpenGLRenderBuffer => window?.Properties?.TryGetNumberValue(Window<UIKit>.PropertyNames.UIKitOpenGLRenderBufferNumber, out var uiKitOpenGLRenderBuffer) is true
			? unchecked((uint)uiKitOpenGLRenderBuffer)
			: default;

		public uint UIKitOpenGLResolveFrameBuffer => window?.Properties?.TryGetNumberValue(Window<UIKit>.PropertyNames.UIKitOpenGLResolveFrameBufferNumber, out var uiKitOpenGLResolveFrameBuffer) is true
			? unchecked((uint)uiKitOpenGLResolveFrameBuffer)
			: default;
	}
}
