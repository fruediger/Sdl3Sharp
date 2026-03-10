using Sdl3Sharp.Video.Windowing.Drivers;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Video.Windowing;

partial class WindowExtensions
{
	extension(Window<UIKit>.PropertyNames)
	{
		/// <summary>
		/// The name of a <see cref="TryCreate(out Window{UIKit}?, bool?, bool?, bool?, bool?, WindowFlags?, bool?, bool?, int?, bool?, bool?, bool?, bool?, bool?, bool?, bool?, bool?, bool?, Window?, bool?, string?, bool?, bool?, bool?, bool?, int?, WindowPosition?, WindowPosition?, nint?, Properties?)">property used when creating a <see cref="Window{TDriver}">Window&lt;<see cref="UIKit">UIKit</see>&gt;</see></see>
		/// that holds a pointer to the <c>(__unsafe_unretained) <see href="https://developer.apple.com/documentation/UIKit/UIWindowScene">UIWindowScene</see></c> associated with the window
		/// </summary>
		/// <remarks>
		/// <para>
		/// The value of the associated property defaults to the active window scene.
		/// </para>
		/// </remarks>
		public static string CreateUIKitWindowScenePointer => "SDL.window.create.uikit.windowscene";

		/// <summary>
		/// The name of a <em>read-only</em> <see cref="Window.Properties">property</see> that holds
		/// a pointer to the <c>(__unsafe_unretained) <see href="https://developer.apple.com/documentation/UIKit/UIWindow">UIWindow</see></c> associated with the window
		/// </summary>
		public static string UIKitWindowPointer => "SDL.window.uikit.window";

		/// <summary>
		/// The name of a <em>read-only</em> <see cref="Window.Properties">property</see> that holds
		/// the <c><see href="https://developer.apple.com/documentation/objectivec/nsinteger">NSInteger</see></c> tag associated with the window's Metal view
		/// </summary>
		public static string UIKitMetalViewTagNumber => "SDL.window.uikit.metal_view_tag";

		/// <summary>
		/// The name of a <em>read-only</em> <see cref="Window.Properties">property</see> that holds
		/// the OpenGL view's framebuffer object
		/// </summary>
		/// <remarks>
		/// <para>
		/// The OpenGL view's framebuffer object must be bound when rendering to the screen using OpenGL.
		/// </para>
		/// </remarks>
		public static string UIKitOpenGLFrameBufferNumber => "SDL.window.uikit.opengl.framebuffer";

		// TODO: replace "SDL_GL_SwapWindow" with the corresponding binding method when it's implemented
		/// <summary>
		/// The name of a <em>read-only</em> <see cref="Window.Properties">property</see> that holds
		/// the OpenGL view's renderbuffer object
		/// </summary>
		/// <remarks>
		/// <para>
		/// The OpenGL view's renderbuffer object must be bound when <see cref="SDL_GL_SwapWindow"/> is called.
		/// </para>
		/// </remarks>
		public static string UIKitOpenGLRenderBufferNumber => "SDL.window.uikit.opengl.renderbuffer";

		/// <summary>
		/// The name of a <em>read-only</em> <see cref="Window.Properties">property</see> that holds
		/// the OpenGL view's resolve framebuffer object, when MSAA is used
		/// </summary>
		public static string UIKitOpenGLResolveFrameBufferNumber => "SDL.window.uikit.opengl.resolve_framebuffer";
	}

	extension(Window<UIKit>)
	{
		/// <inheritdoc cref="Window.TryCreate(out Window?, bool?, bool?, bool?, bool?, WindowFlags?, bool?, bool?, int?, bool?, bool?, bool?, bool?, bool?, bool?, bool?, bool?, bool?, Window?, bool?, string?, bool?, bool?, bool?, bool?, int?, WindowPosition?, WindowPosition?, Properties?)"/>
		/// <param name="uiKitWindowScene">
		/// A pointer to a <c>(__unsafe_unretained) <see href="https://developer.apple.com/documentation/UIKit/UIWindowScene">UIWindowScene</see></c> to associate with the created window.
		/// Must be directly cast to an <see cref="IntPtr"/> from a <c>(__unsafe_unretained) <see href="https://developer.apple.com/documentation/UIKit/UIWindowScene">UIWindowScene</see>*</c> pointer.
		/// If this parameter is not provided or <c><see langword="null"/></c> (the default), the associated window scene will default to the active window scene.
		/// </param>
#pragma warning disable CS1573 // we get these from inheritdoc
		public static bool TryCreate([NotNullWhen(true)] out Window<UIKit>? window, bool? alwaysOnTop = default, bool? bordered = default, bool? constrainPopup = default, bool? externalGraphicsContext = default, WindowFlags? flags = default,
			bool? focusable = default, bool? fullscreen = default, int? height = default, bool? hidden = default, bool? highPixelDensity = default, bool? maximized = default, bool? menu = default, bool? metal = default,
			bool? minimized = default, bool? modal = default, bool? mouseGrabbed = default, bool? openGL = default, Window? parent = default, bool? resizable = default, string? title = default, bool? transparent = default,
			bool? tooltip = default, bool? utility = default, bool? vulkan = default, int? width = default, WindowPosition? x = default, WindowPosition? y = default,
			IntPtr? uiKitWindowScene = default, Properties? properties = default)
#pragma warning restore CS1573
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
					out window, alwaysOnTop, bordered, constrainPopup, externalGraphicsContext, flags,
					focusable, fullscreen, height, hidden, highPixelDensity, maximized, menu, metal,
					minimized, modal, mouseGrabbed, openGL, parent, resizable, title, transparent,
					tooltip, utility, vulkan, width, x, y, propertiesUsed
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
		/// <summary>
		/// Gets a pointer to the <c>(__unsafe_unretained) <see href="https://developer.apple.com/documentation/UIKit/UIWindow">UIWindow</see></c> associated with this window
		/// </summary>
		/// <value>
		/// A pointer to the <c>(__unsafe_unretained) <see href="https://developer.apple.com/documentation/UIKit/UIWindow">UIWindow</see></c> associated with this window
		/// </value>
		/// <remarks>
		/// <para>
		/// The value of this property can be directly cast to an <c>(__unsafe_unretained) <see href="https://developer.apple.com/documentation/UIKit/UIWindow">UIWindow</see>*</c> pointer.
		/// </para>
		/// <para>
		/// This property should only be accessed from the main thread.
		/// </para>
		/// </remarks>
		public IntPtr UIKitWindow => window?.Properties?.TryGetPointerValue(Window<UIKit>.PropertyNames.UIKitWindowPointer, out var uiKitWindowPtr) is true
			? uiKitWindowPtr
			: default;

		/// <summary>
		/// Gets the <c><see href="https://developer.apple.com/documentation/objectivec/nsinteger">NSInteger</see></c> tag associated with the window's Metal view
		/// </summary>
		/// <value>
		/// The <c><see href="https://developer.apple.com/documentation/objectivec/nsinteger">NSInteger</see></c> tag associated with the window's Metal view
		/// </value>
		/// <remarks>
		/// <para>
		/// The value of this property can be directly cast to an <c><see href="https://developer.apple.com/documentation/objectivec/nsinteger">NSInteger</see></c>.
		/// </para>
		/// <para>
		/// This property should only be accessed from the main thread.
		/// </para>
		/// </remarks>
		public nint UIKitMetalViewTag => window?.Properties?.TryGetNumberValue(Window<UIKit>.PropertyNames.UIKitMetalViewTagNumber, out var uiKitMetalViewTag) is true
			? unchecked((nint)uiKitMetalViewTag)
			: default;

		/// <summary>
		/// Gets the OpenGL view's framebuffer object
		/// </summary>
		/// <value>
		/// The OpenGL view's framebuffer object
		/// </value>
		/// <remarks>
		/// <para>
		/// The OpenGL view's framebuffer object must be bound when rendering to the screen using OpenGL.
		/// </para>
		/// <para>
		/// This property should only be accessed from the main thread.
		/// </para>
		/// </remarks>
		public uint UIKitOpenGLFrameBuffer => window?.Properties?.TryGetNumberValue(Window<UIKit>.PropertyNames.UIKitOpenGLFrameBufferNumber, out var uiKitOpenGLFrameBuffer) is true
			? unchecked((uint)uiKitOpenGLFrameBuffer)
			: default;

		// TODO: replace "SDL_GL_SwapWindow" with the corresponding binding method when it's implemented
		/// <summary>
		/// Gets the OpenGL view's renderbuffer object
		/// </summary>
		/// <value>
		/// The OpenGL view's renderbuffer object
		/// </value>
		/// <remarks>
		/// <para>
		/// The OpenGL view's renderbuffer object must be bound when <see cref="SDL_GL_SwapWindow"/> is called.
		/// </para>
		/// <para>
		/// This property should only be accessed from the main thread.
		/// </para>
		/// </remarks>
		public uint UIKitOpenGLRenderBuffer => window?.Properties?.TryGetNumberValue(Window<UIKit>.PropertyNames.UIKitOpenGLRenderBufferNumber, out var uiKitOpenGLRenderBuffer) is true
			? unchecked((uint)uiKitOpenGLRenderBuffer)
			: default;

		/// <summary>
		/// Gets the OpenGL view's resolve framebuffer object, when MSAA is used
		/// </summary>
		/// <value>
		/// The OpenGL view's resolve framebuffer object, when MSAA is used
		/// </value>
		public uint UIKitOpenGLResolveFrameBuffer => window?.Properties?.TryGetNumberValue(Window<UIKit>.PropertyNames.UIKitOpenGLResolveFrameBufferNumber, out var uiKitOpenGLResolveFrameBuffer) is true
			? unchecked((uint)uiKitOpenGLResolveFrameBuffer)
			: default;
	}
}
