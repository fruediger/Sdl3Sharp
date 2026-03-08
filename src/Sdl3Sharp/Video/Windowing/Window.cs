using Sdl3Sharp.Events;
using Sdl3Sharp.Internal;
using Sdl3Sharp.Utilities;
using Sdl3Sharp.Video.Coloring;
using Sdl3Sharp.Video.Drawing;
using Sdl3Sharp.Video.Rendering;
using Sdl3Sharp.Video.Rendering.Drivers;
using Sdl3Sharp.Video.Windowing.Drivers;
using System;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace Sdl3Sharp.Video.Windowing;

public abstract partial class Window : IDisposable
{
	private static readonly ConcurrentDictionary<IntPtr, WeakReference<Window>> mKnownInstances = [];

	private unsafe SDL_Window* mWindow;
	private GCHandle mSelfHandle = default;

	private protected unsafe Window(SDL_Window* window, bool register)
	{
		mWindow = window;
		mSelfHandle = GCHandle.Alloc(this, GCHandleType.Weak);

		// Register a 'WindowDestroy' event watch, so we get notified when a window gets destroyed,
		// even if it's done through other means (e.g. a child window getting destroyed alongside its parent).
		// This way we can automatically dispose the corresponding Window instance and remove it,
		// when the native window gets destroyed.
		Sdl.SDL_AddEventWatch(&EventWatchDestroyImpl, unchecked((void*)GCHandle.ToIntPtr(mSelfHandle)));

		if (register)
		{
			if (window is not null)
			{
				mKnownInstances.AddOrUpdate(unchecked((IntPtr)window), addRef, updateRef, this);
			}

			static WeakReference<Window> addRef(IntPtr window, Window newWindow) => new(newWindow);

			static WeakReference<Window> updateRef(IntPtr window, WeakReference<Window> previousWindowRef, Window newWindow)
			{
				if (previousWindowRef.TryGetTarget(out var previousWindow))
				{
#pragma warning disable IDE0079
#pragma warning disable CA1816
					GC.SuppressFinalize(previousWindow);
#pragma warning restore CA1816
#pragma warning restore IDE0079

					previousWindow.Dispose(disposing: true, forget: false);
				}

				previousWindowRef.SetTarget(newWindow);

				return previousWindowRef;
			}
		}
	}

	~Window() => Dispose(disposing: false, forget: true);

	/// <summary>
	/// Gets a collection of all currently valid windows
	/// </summary>
	/// <value>
	/// A collection of all currently valid windows
	/// </value>
	/// <para>
	/// This property should only be accessed from the main thread.
	/// </para>
	/// <exception cref="SdlException">Couldn't get the list of windows</exception>
	public static Window[] AllWindows
	{
		get
		{
			unsafe
			{
				Unsafe.SkipInit(out int count);

				var windows = SDL_GetWindows(&count);

				if (windows is null)
				{
					failCouldNotGetWindows();
				}

				try
				{
					if (count is not > 0)
					{
						return [];
					}

					var result = GC.AllocateUninitializedArray<Window>(count);

					var windowsPtr = windows;
					foreach (ref var window in result.AsSpan())
					{
						// here we rely on that SDL_GetWindows doesn't include null pointers in the returned array,
						// but I'm pretty sure that's a reasonable assumption to make
						TryGetOrCreate(*windowsPtr++, out window!);
					}

					return result;
				}
				finally
				{
					Utilities.NativeMemory.SDL_free(windows);
				}
			}

			[DoesNotReturn]
			static void failCouldNotGetWindows() => throw new SdlException("Couldn't get the list of windows");
		}
	}

	/// <summary>
	/// Gets or sets the range of aspect ratios for the window's client area
	/// </summary>
	/// <value>
	/// The range of aspect ratios for the window's client area.
	/// The minimum is the lower limit of the aspect ratio, or <c>0</c> for no lower limit set.
	/// The maximum is the upper limit of the aspect ratio, or <c>0</c> for no upper limit set.
	/// </value>
	/// <remarks>
	/// <para>
	/// The aspect ratio is the ratio of width divided by height, e.g. <c>2560</c> by <c>1600</c> would be <c>1.6</c>.
	/// Larger aspect ratios are wider and smaller aspect ratios are narrower.
	/// </para>
	/// <para>
	/// If the window is in a fixed-size state, such as maximized or fullscreen, when setting this property,
	/// the request will be deferred until the window exits that state and becomes resizable again.
	/// </para>
	/// <para>
	/// On some windowing backends, a request to change this setting is asynchronous and the new window aspect ratio may not have have been applied immediately upon setting this property.
	/// If an immediate change is required, you can call <see cref="TrySync"/> to block until the changes have taken effect.
	/// </para>
	/// <para>
	/// When the window size changes, an <see cref="EventType.WindowResized"/> event will be emitted with the new window dimensions.
	/// Note that the new dimensions may not match the exact aspect ratio requested, as some windowing backends can restrict the window size in certain scenarios
	/// (e.g. constraining the size of the content area to remain within the usable desktop bounds).
	/// Additionally, windowing backends can also deny the request to change this setting altogether.
	/// </para>
	/// <para>
	/// This property should only be accessed from the main thread.
	/// </para>
	/// </remarks>
	/// <exception cref="SdlException">When setting or getting this property, SDL failed with an error (check <see cref="Error.TryGet(out string?)"/> for more information)</exception>
	public (float Minimum, float Maximum) AspectRatio
	{
		get
		{
			unsafe
			{
				Unsafe.SkipInit(out (float Minimum, float Maximum) aspectRatio);

				ErrorHelper.ThrowIfFailed(SDL_GetWindowAspectRatio(mWindow, &aspectRatio.Minimum, &aspectRatio.Maximum));

				return aspectRatio;
			}
		}

		set
		{
			unsafe
			{
				ErrorHelper.ThrowIfFailed(SDL_SetWindowAspectRatio(mWindow, value.Minimum, value.Maximum));
			}
		}
	}

	/// <summary>
	/// Gets the size of the window's borders (decorations) around the client area
	/// </summary>
	/// <value>
	/// The size of the window's borders (decorations) around the client area, in pixels.
	/// </value>
	/// <remarks>
	/// <para>
	/// Note that if this property fails to retrieve the borders size, the size values will be initialized to <c>(0, 0, 0, 0)</c> instead.
	/// </para>
	/// <para>
	/// This property can fail to retrieve the borders size on platforms where the window has not yet been decorated by the display server (for example, immediately after creating the <see cref="Window"/>).
	/// It is recommended that you wait at least until the window has been presented and composited, so that the window backend has a chance to decorate the window and provide the border dimensions to SDL.
	/// </para>
	/// <para>
	/// This property can also fail to retrieve the borders size if getting that information is not supported.
	/// </para>
	/// <para>
	/// This property should only be accessed from the main thread.
	/// </para>
	/// </remarks>
	public (int Top, int Left, int Bottom, int Right) BordersSize
	{
		get
		{
			unsafe
			{
				Unsafe.SkipInit(out (int Top, int Left, int Bottom, int Right) borderSize);

				SDL_GetWindowBordersSize(mWindow, &borderSize.Top, &borderSize.Left, &borderSize.Bottom, &borderSize.Right);

				return borderSize;
			}
		}
	}

	private protected abstract Display GetDisplayImpl();

	/// <summary>
	/// Gets the display associated with the window
	/// </summary>
	/// <value>
	/// The display associated with the window, which is the display containing the center of the window
	/// </value>
	/// <remarks>
	/// <para>
	/// The value of the property sometimes reports the <see cref="Display.PrimaryDisplay">primary display</see> instead,
	/// if SDL couldn't determine the actual display the window is on.
	/// </para>
	/// <para>
	/// Accessing this property can also <see langword="throw"/> an <see cref="SdlException"/> under some rare circumstances, for example if there are no connected displays at all.
	/// </para>
	/// <para>
	/// This property should only be accessed from the main thread.
	/// </para>
	/// </remarks>
	/// <exception cref="SdlException">Couldn't get the display for the window (check <see cref="Error.TryGet(out string?)"/> for more information)</exception>
	public Display Display => GetDisplayImpl();

	/// <summary>
	/// Gets the content display scale relative to the window's pixel size
	/// </summary>
	/// <value>
	/// The content display scale relative to the window's pixel size
	/// </value>
	/// <remarks>
	/// <para>
	/// The value of this property is a combination of the window pixel density and the display content scale, and is the expected scale for displaying content in this window.
	/// For example, if a <c>3840</c> by <c>2160</c> window had a display scale of <c>2.0</c>,
	/// the user expects the content to take twice as many pixels and be the same physical size as if it were being displayed in a <c>1920</c> by <c>1080</c> window with a display scale of <c>1.0</c>.
	/// </para>
	/// <para>
	/// Conceptually this value corresponds to the scale display setting, and is updated when that setting is changed, or the window moves to a display with a different scale setting.
	/// </para>
	/// <para>
	/// This property should only be accessed from the main thread.
	/// </para>
	/// </remarks>
	public float DisplayScale
	{
		get
		{
			unsafe
			{
				return SDL_GetWindowDisplayScale(mWindow);
			}
		}
	}

	/// <summary>
	/// Gets the flags associated with the window
	/// </summary>
	/// <value>
	/// The flags associated with the window
	/// </value>
	/// <remarks>
	/// <para>
	/// This property should only be accessed from the main thread.
	/// </para>
	/// </remarks>
	public WindowFlags Flags
	{
		get
		{
			unsafe
			{
				return SDL_GetWindowFlags(mWindow);
			}
		}
	}

	/// <summary>
	/// Gets or sets the <see cref="DisplayMode"/> to use when the window is visible at fullscreen
	/// </summary>
	/// <value>
	/// The <see cref="DisplayMode"/> to use when the window is visible at fullscreen, or <c><see langword="null"/></c> for borderless fullscreen desktop mode
	/// </value>
	/// <remarks>
	/// <para>
	/// This property only effects the display mode used when the window is in fullscreen mode.
	/// To change the window's size when it's not in fullscreen mode, use the <see cref="Size"/> property instead.
	/// </para>
	/// <para>
	/// If the window is currently in the fullscreen state, a request to change this setting is asynchronous and the new display mode may not have have been applied immediately upon setting this property.
	/// If an immediate change is required, you can call <see cref="TrySync"/> to block until the changes have taken effect.
	/// </para>
	/// <para>
	/// When the new display mode takes effect, an <see cref="EventType.WindowResized"/> and/or an <see cref="EventType.WindowPixelSizeChanged"/> event will be emitted with the new display mode's dimensions.
	/// </para>
	/// <para>
	/// This property should only be accessed from the main thread.
	/// </para>
	/// </remarks>
	/// <exception cref="SdlException">When setting this property, SDL failed with an error (check <see cref="Error.TryGet(out string?)"/> for more information)</exception>
	public DisplayMode? FullscreenMode
	{
		get
		{
			unsafe
			{
				DisplayMode.TryGetOrCreateUnmanaged(SDL_GetWindowFullscreenMode(mWindow), out var mode);
				return mode;
			}
		}

		set
		{
			unsafe
			{
				if (value is not null)
				{
					fixed (DisplayMode.SDL_DisplayMode* mode = &value.Mode)
					{
						ErrorHelper.ThrowIfFailed(SDL_SetWindowFullscreenMode(mWindow, mode));
					}
				}
				else
				{
					ErrorHelper.ThrowIfFailed(SDL_SetWindowFullscreenMode(mWindow, null));
				}
			}
		}
	}

	/// <summary>
	/// Gets the window that currently has the input grab, if any
	/// </summary>
	/// <value>
	/// The window that currently has the input grab, or <c><see langword="null"/></c> if there's no such window
	/// </value>
	/// <remarks>
	/// <para>
	/// This property should only be accessed from the main thread.
	/// </para>
	/// </remarks>
	public static Window? GrabbedWindow
	{
		get
		{
			unsafe
			{
				TryGetOrCreate(SDL_GetGrabbedWindow(), out var window);
				return window;
			}
		}
	}

	/// <summary>
	/// Gets or sets a value indicating whether the window has grabbed keyboard input
	/// </summary>
	/// <value>
	/// A value indicating whether the window has grabbed keyboard input
	/// </value>
	/// <remarks>
	/// <para>
	/// Keyboard grab enables capture of system keyboard shortcuts like <kbd>Alt</kbd>+<kbd>Tab</kbd> or the <kbd>Meta</kbd>/<kbd>Super</kbd> key.
	/// Note that not all system keyboard shortcuts can be captured by applications (one notable example is <kbd>Ctrl</kbd>+<kbd>Alt</kbd>+<kbd>Del</kbd> on Windows).
	/// </para>
	/// <para>
	/// This property is primarily intended for use by specialized applications such as VNC clients or VM frontends. Normal games should not use keyboard grab.
	/// </para>
	/// <para>
	/// When keyboard grab is enabled, SDL will continue to handle <kbd>Alt</kbd>+<kbd>Tab</kbd> when the window is fullscreen to ensure the user is not trapped in your application.
	/// If you have a custom keyboard shortcut to exit fullscreen mode, you may suppress this behavior with a setting of <see cref="Hint.AllowAltTabWhileGrabbed"/>.
	/// </para>
	/// <para>
	/// If the keyboard grab is enabled on a window while another window currently has the keyboard grab,
	/// the other window loses its grab in favor of the window where the grab was just enabled.
	/// </para>
	/// <para>
	/// This property should only be accessed from the main thread.
	/// </para>
	/// </remarks>
	/// <exception cref="SdlException">When setting this property, SDL failed with an error (check <see cref="Error.TryGet(out string?)"/> for more information)</exception>
	public bool HasKeyboardGrab
	{
		get
		{
			unsafe
			{
				return SDL_GetWindowKeyboardGrab(mWindow);
			}
		}

		set
		{
			unsafe
			{
				ErrorHelper.ThrowIfFailed(SDL_SetWindowKeyboardGrab(mWindow, value));
			}
		}
	}

	/// <summary>
	/// Gets or sets a value indicating whether the window has grabbed mouse input
	/// </summary>
	/// <value>
	/// A value indicating whether the window has grabbed mouse input
	/// </value>
	/// <remarks>
	/// <para>
	/// Mouse grab confines the mouse cursor to the area of the window.
	/// </para>
	/// <para>
	/// This property should only be accessed from the main thread.
	/// </para>
	/// </remarks>
	/// <exception cref="SdlException">When setting this property, SDL failed with an error (check <see cref="Error.TryGet(out string?)"/> for more information)</exception>
	public bool HasMouseGrab
	{
		get
		{
			unsafe
			{
				return SDL_GetWindowMouseGrab(mWindow);
			}
		}

		set
		{
			unsafe
			{
				ErrorHelper.ThrowIfFailed(SDL_SetWindowMouseGrab(mWindow, value));
			}
		}
	}

	/// <summary>
	/// Gets a value indicating whether the window has a <see cref="Video.Surface"/> associated with it
	/// </summary>
	/// <value>
	/// A value indicating whether the window has a <see cref="Video.Surface"/> associated with it
	/// </value>
	/// <remarks>
	/// <para>
	/// If the value of this property is <see langword="true"/>, you can expect a non-<c><see langword="null"/></c> value for the <see cref="Surface"/> property, and vice versa.
	/// </para>
	/// <para>
	/// To reset (destroy) the surface associated with the window, you can use <see cref="TryResetSurface"/>.
	/// </para>
	/// <para>
	/// This property should only be accessed from the main thread.
	/// </para>
	/// </remarks>
	public bool HasSurface
	{
		get
		{
			unsafe
			{
				return SDL_WindowHasSurface(mWindow);
			}
		}
	}

	public float HdrHeadroom => Properties?.TryGetFloatValue(PropertyNames.HdrHeadroomFloat, out var hdrHeadroom) is true
		? hdrHeadroom
		: default;

	private HitTest? mHitTest = null;

	/// <summary>
	/// Gets or sets the custom hit test deciding whether a window region has special properties
	/// </summary>
	/// <value>
	/// The custom hit test deciding whether a window region has special properties, or <c><see langword="null"/></c> if no custom hit test is set for the window
	/// </value>
	/// <remarks>
	/// <para>
	/// Normally windows are dragged and resized by decorations provided by the system window manager (a title bar, borders, etc), but for some applications, it makes sense to drag them from somewhere else inside the window itself.
	/// For example, you might want to create a borderless window that the user can drag from any part, or you to simulate your own title bar, etc.
	/// </para>
	/// <para>
	/// This property lets the application provide a delegate that's called to determine whether certain regions of the window are special.
	/// This delegate is run during event processing when SDL needs to tell the operating system to treat a region of the window specially. This process is called "hit testing."
	/// </para>
	/// <para>
	/// Mouse input may not be delivered to your application if it is within a special area.
	/// The operating system will often apply that input to moving the window or resizing the window and not deliver it to the application.
	/// </para>
	/// <para>
	/// Specifying <c><see langword="null"/></c> for this property disables hit testing. Hit testing is disabled by default.
	/// </para>
	/// <para>
	/// Some platforms may not support this functionality and will <see langword="throw"/> a <see cref="SdlException"/> when you attempt to set this property, even if you attempt to set it to <c><see langword="null"/></c> to disable hit testing.
	/// </para>
	/// <para>
	/// The provided delegate may be called at any time, and it being called does not indicate any specific behavior.
	/// For example, on Windows, the delegate certainly might called when the operating system is trying to decide whether to drag your window.
	/// But it will be called for lot of other reasons too, some of which might be unrelated to anything you probably care about and <em>even when the mouse isn't actually at the location it is testing</em>.
	/// Since the delegate can be called at any time, you should try to keep it as efficient as possible and try to avoid doing any heavy or potentially time-consuming/blocking work in it.
	/// </para>
	/// <para>
	/// This property should only be accessed from the main thread.
	/// </para>
	/// </remarks>
	/// <exception cref="SdlException">When setting this property, SDL failed with an error (check <see cref="Error.TryGet(out string?)"/> for more information)</exception>
	public HitTest? HitTest
	{
		get
		{
			// SDL doesn't provide a way to get the current hit test callback,
			// but since we need to keep the managed callback alive through a reference anyway,
			// we can just return that.
			// Note that this doesn't necessarily reflect a hit test callback that was set externally,
			// but that's probably fine since we expect the user to use the managed API exclusively anyway.

			return mHitTest;
		}

		set
		{
			unsafe
			{
				if (value is null)
				{
					var oldHitTest = mHitTest;
					mHitTest = null;

					if (!(bool)SDL_SetWindowHitTest(mWindow, null, null))
					{
						mHitTest = oldHitTest;

						ErrorHelper.ThrowIfFailed();
					}
				}
				else
				{
					if (!mSelfHandle.IsAllocated)
					{
						failDisposed();
					}

					var oldHitTest = mHitTest;
					mHitTest = value;

					if (!(bool)SDL_SetWindowHitTest(mWindow, &HitTestImpl, unchecked((void*)GCHandle.ToIntPtr(mSelfHandle))))
					{
						mHitTest = oldHitTest;

						ErrorHelper.ThrowIfFailed();
					}
				}
			}

			[DoesNotReturn]
			static void failDisposed() => throw new ObjectDisposedException(nameof(Window), "The window has already been disposed");
		}
	}

	public Surface Icon
	{
		set
		{
			unsafe
			{
				ErrorHelper.ThrowIfFailed(SDL_SetWindowIcon(mWindow, value is not null ? value.Pointer : null));
			}
		}
	}

	public uint Id
	{
		get
		{
			unsafe
			{
				return SDL_GetWindowID(mWindow);
			}
		}
	}

	public bool IsAlwaysOnTop
	{
		get => (Flags & WindowFlags.AlwaysOnTop) is not 0;

		set
		{
			unsafe
			{
				ErrorHelper.ThrowIfFailed(SDL_SetWindowAlwaysOnTop(mWindow, value));
			}
		}
	}

	public bool IsBordered
	{
		get => (Flags & WindowFlags.Borderless) is 0;

		set
		{
			unsafe
			{
				ErrorHelper.ThrowIfFailed(SDL_SetWindowBordered(mWindow, value));
			}
		}
	}

	public bool IsFocusable
	{
		get => (Flags & WindowFlags.NotFocusable) is 0;

		set
		{
			unsafe
			{
				ErrorHelper.ThrowIfFailed(SDL_SetWindowFocusable(mWindow, value));
			}
		}
	}

	public bool IsFullscreen
	{
		get => (Flags & WindowFlags.Fullscreen) is not 0;

		set
		{
			unsafe
			{
				ErrorHelper.ThrowIfFailed(SDL_SetWindowFullscreen(mWindow, value));
			}
		}
	}

	public bool IsHdrEnabled => Properties?.TryGetBooleanValue(PropertyNames.HdrEnabledBoolean, out var hdrEnabled) is true
		&& hdrEnabled;

	public bool IsModal
	{
		get => (Flags & WindowFlags.Modal) is not 0;

		set
		{
			unsafe
			{
				ErrorHelper.ThrowIfFailed(SDL_SetWindowModal(mWindow, value));
			}
		}
	}

	public bool IsResizable
	{
		get => (Flags & WindowFlags.Resizable) is not 0;

		set
		{
			unsafe
			{
				ErrorHelper.ThrowIfFailed(SDL_SetWindowResizable(mWindow, value));
			}
		}
	}

	public bool IsValid { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get { unsafe { return mWindow is not null; } } }

	public (int Width, int Height) MaximumSize
	{
		get
		{
			unsafe
			{
				// For some reason getters on SDL windows aren't setting safe defaults for out parameters,
				// before returning early because of failure (SDL does this literally everywhere else),
				// therefore we have to safely initialize the return value ourselves in case SDL doesn't do it.
				// So Unsafe.SkipInit for us.
				(int Width, int Height) maxSize = default;

				SDL_GetWindowMaximumSize(mWindow, &maxSize.Width, &maxSize.Height);

				return maxSize;
			}
		}

		set
		{
			unsafe
			{
				ErrorHelper.ThrowIfFailed(SDL_SetWindowMaximumSize(mWindow, value.Width, value.Height));
			}
		}
	}

	public (int Width, int Height) MinimumSize
	{
		get
		{
			unsafe
			{
				// For some reason getters on SDL windows aren't setting safe defaults for out parameters,
				// before returning early because of failure (SDL does this literally everywhere else),
				// therefore we have to safely initialize the return value ourselves in case SDL doesn't do it.
				// So Unsafe.SkipInit for us.
				(int Width, int Height) minSize = default;

				SDL_GetWindowMinimumSize(mWindow, &minSize.Width, &minSize.Height);

				return minSize;
			}
		}

		set
		{
			unsafe
			{
				ErrorHelper.ThrowIfFailed(SDL_SetWindowMinimumSize(mWindow, value.Width, value.Height));
			}
		}
	}

	public Rect<int>? MouseRect
	{
		get
		{
			unsafe
			{
				return SDL_GetWindowMouseRect(mWindow) switch
				{
					null => null,
					var rect => *rect
				};
			}
		}

		set
		{
			unsafe
			{
				fixed (Rect<int>* rect = &Nullable.GetValueRefOrDefaultRef(in value))
				{
					ErrorHelper.ThrowIfFailed(SDL_SetWindowMouseRect(mWindow, value.HasValue ? rect : null));
				}
			}
		}
	}

	public float Opacity
	{
		get
		{
			unsafe
			{
				return SDL_GetWindowOpacity(mWindow);
			}
		}

		set
		{
			unsafe
			{
				ErrorHelper.ThrowIfFailed(SDL_SetWindowOpacity(mWindow, value));
			}
		}
	}

	public Window? Parent
	{
		get
		{
			unsafe
			{
				TryGetOrCreate(SDL_GetWindowParent(mWindow), out var parent);
				return parent;
			}
		}

		set
		{
			unsafe
			{
				ErrorHelper.ThrowIfFailed(SDL_SetWindowParent(mWindow, value is not null ? value.Pointer : null));
			}
		}
	}

	public float PixelDensity
	{
		get
		{
			unsafe
			{
				return SDL_GetWindowPixelDensity(mWindow);
			}
		}
	}

	public PixelFormat PixelFormat
	{
		get
		{
			unsafe
			{
				return SDL_GetWindowPixelFormat(mWindow);
			}
		}
	}

	internal unsafe SDL_Window* Pointer { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mWindow; }

	public (WindowPosition X, WindowPosition Y) Position
	{
		get
		{
			unsafe
			{
				// For some reason getters on SDL windows aren't setting safe defaults for out parameters,
				// before returning early because of failure (SDL does this literally everywhere else),
				// therefore we have to safely initialize the return value ourselves in case SDL doesn't do it.
				// So Unsafe.SkipInit for us.
				(WindowPosition X, WindowPosition Y) position = default;

				SDL_GetWindowPosition(mWindow, unchecked((int*)&position.X), unchecked((int*)&position.Y));

				return position;
			}
		}

		set
		{
			unsafe
			{
				ErrorHelper.ThrowIfFailed(SDL_SetWindowPosition(mWindow, Unsafe.BitCast<WindowPosition, int>(value.X), Unsafe.BitCast<WindowPosition, int>(value.Y)));
			}
		}
	}

	public ProgressState ProgressState
	{
		get
		{
			unsafe
			{
				return SDL_GetWindowProgressState(mWindow);
			}
		}

		set
		{
			unsafe
			{
				ErrorHelper.ThrowIfFailed(SDL_SetWindowProgressState(mWindow, value));
			}
		}
	}

	public float ProgressValue
	{
		get
		{
			unsafe
			{
				return SDL_GetWindowProgressValue(mWindow);
			}
		}

		set
		{
			unsafe
			{
				ErrorHelper.ThrowIfFailed(SDL_SetWindowProgressValue(mWindow, value));
			}
		}
	}

	public Properties? Properties
	{
		get
		{
			unsafe
			{
				return SDL_GetWindowProperties(mWindow) switch
				{
					0 => null,
					var id => Properties.GetOrCreate(sdl: null, id)
				};
			}
		}
	}

	public Renderer? Renderer
	{
		get
		{
			unsafe
			{
				Renderer.TryGetOrCreate(SDL_GetRenderer(mWindow), out var renderer);
				return renderer;
			}
		}
	}

	public Rect<int> SafeArea
	{
		get
		{
			unsafe
			{
				// SDL_GetWindowSafeArea seems to be one of the few exceptions to the rule of getters on SDL windows
				// not safely initializing out parameters before returning early because of failure.
				// SDL_GetWindowSafeArea actually sets of its out parameters to zero in case of failure.
				Unsafe.SkipInit(out Rect<int> rect);

				SDL_GetWindowSafeArea(mWindow, &rect);

				return rect;
			}
		}
	}

	public float SdrWhiteLevel => Properties?.TryGetFloatValue(PropertyNames.SdrWhiteLevelFloat, out var sdrWhiteLevel) is true
		? sdrWhiteLevel
		: default;

	public Surface? Shape
	{
		get
		{
			unsafe
			{
				return Properties?.TryGetPointerValue(PropertyNames.ShapePointer, out var shapePtr) is true
					&& Surface.TryGetOrCreate(unchecked((Surface.SDL_Surface*)shapePtr), out var shape)
					? shape
					: default;
			}
		}

		set
		{
			unsafe
			{
				ErrorHelper.ThrowIfFailed(SDL_SetWindowShape(mWindow, value is not null ? value.Pointer : null));
			}
		}
	}

	public (int Width, int Height) Size
	{
		get
		{
			unsafe
			{
				// For some reason getters on SDL windows aren't setting safe defaults for out parameters,
				// before returning early because of failure (SDL does this literally everywhere else),
				// therefore we have to safely initialize the return value ourselves in case SDL doesn't do it.
				// So Unsafe.SkipInit for us.
				(int Width, int Height) size = default;

				SDL_GetWindowSize(mWindow, &size.Width, &size.Height);

				return size;
			}
		}

		set
		{
			unsafe
			{
				ErrorHelper.ThrowIfFailed(SDL_SetWindowSize(mWindow, value.Width, value.Height));
			}
		}
	}

	public (int Width, int Height) SizeInPixels
	{
		get
		{
			unsafe
			{
				// For some reason getters on SDL windows aren't setting safe defaults for out parameters,
				// before returning early because of failure (SDL does this literally everywhere else),
				// therefore we have to safely initialize the return value ourselves in case SDL doesn't do it.
				// So Unsafe.SkipInit for us.
				(int Width, int Height) sizeInPixels = default;

				SDL_GetWindowSizeInPixels(mWindow, &sizeInPixels.Width, &sizeInPixels.Height);

				return sizeInPixels;
			}
		}
	}

	// TODO: doc TryResetSurface
	public Surface? Surface
	{
		get
		{
			unsafe
			{
				Surface.TryGetOrCreate(SDL_GetWindowSurface(mWindow), out var surface);
				return surface;
			}
		}
	}

	public WindowSurfaceVSync SurfaceVSync
	{
		get
		{
			unsafe
			{
				// For some reason getters on SDL windows aren't setting safe defaults for out parameters,
				// before returning early because of failure (SDL does this literally everywhere else),
				// therefore we have to safely initialize the return value ourselves in case SDL doesn't do it.
				// So Unsafe.SkipInit for us.
				WindowSurfaceVSync vsync = default;

				SDL_GetWindowSurfaceVSync(mWindow, &vsync);

				return vsync;
			}
		}

		set
		{
			unsafe
			{
				ErrorHelper.ThrowIfFailed(SDL_SetWindowSurfaceVSync(mWindow, value));
			}
		}
	}

	public string Title
	{
		get
		{
			unsafe
			{
				return Utf8StringMarshaller.ConvertToManaged(SDL_GetWindowTitle(mWindow)) ?? string.Empty;
			}
		}

		set
		{
			unsafe
			{
				var valueUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(value);
				try
				{
					ErrorHelper.ThrowIfFailed(SDL_SetWindowTitle(mWindow, valueUtf8));
				}
				finally
				{
					Utf8StringMarshaller.Free(valueUtf8);
				}
			}
		}
	}

	public void Dispose()
	{
		GC.SuppressFinalize(this);
		Dispose(disposing: true, forget: true);
	}

	private protected virtual void Dispose(bool disposing, bool forget)
	{
		unsafe
		{
			if (mWindow is not null)
			{
				if (forget)
				{
					mKnownInstances.TryRemove(unchecked((IntPtr)mWindow), out _);
				}

				if (mSelfHandle.IsAllocated)
				{
					// Hopefully 'EventWatchDestroyImpl' doesn't change its address during the lifetime of the window (it shouldn't)
					// and 'GCHandle.ToIntPtr' always returns the same value for the same GCHandle (it should).
					Sdl.SDL_RemoveEventWatch(&EventWatchDestroyImpl, unchecked((void*)GCHandle.ToIntPtr(mSelfHandle)));

					mSelfHandle.Free();
					mSelfHandle = default;
				}

				SDL_DestroyWindow(mWindow);
				mWindow = null;
			}
		}
	}

	public static bool TryCreate(string title, int width, int height, [NotNullWhen(true)] out Window? window, WindowFlags flags = WindowFlags.None)
	{
		unsafe
		{
			var titleUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(title);
			try
			{
				var windowPtr = SDL_CreateWindow(titleUtf8, width, height, flags);

				if (windowPtr is null)
				{
					window = null;
					return false;
				}

				window = RegisteredDriverOrGenericFallbackDriverFactory.Create(windowPtr, register: true);
				return true;
			}
			finally
			{
				Utf8StringMarshaller.Free(titleUtf8);
			}
		}
	}

	private static bool TryCreate<TFactory, TWindow>([NotNullWhen(true)] out TWindow? window, bool? alwaysOnTop = default, bool? bordered = default, bool? constrainPopup = default, bool? externalGraphicsContext = default,bool? focusable = default,
		bool? fullscreen = default, int? height = default, bool? hidden = default, bool? highPixelDensity = default, bool? maximized = default, bool? menu = default, bool? metal = default, bool? minimized = default,
		bool? modal = default, bool? mouseGrabbed = default, bool? openGL = default, Window? parent = default, bool? resizable = default, string? title = default, bool? transparent = default, bool? tooltip = default,
		bool? utility = default, bool? vulkan = default, int? width = default, int? x = default, int? y = default, Properties? properties = default)
		where TFactory : notnull, IFactory<TWindow>
		where TWindow : notnull, Window
	{
		unsafe
		{
			Properties propertiesUsed;
			Unsafe.SkipInit(out bool? alwaysOnTopBackup);
			Unsafe.SkipInit(out bool? borderedBackup);
			Unsafe.SkipInit(out bool? constrainPopupBackup);
			Unsafe.SkipInit(out bool? externalGraphicsContextBackup);
			Unsafe.SkipInit(out bool? focusableBackup);
			Unsafe.SkipInit(out bool? fullscreenBackup);
			Unsafe.SkipInit(out int? heightBackup);
			Unsafe.SkipInit(out bool? hiddenBackup);
			Unsafe.SkipInit(out bool? highPixelDensityBackup);
			Unsafe.SkipInit(out bool? maximizedBackup);
			Unsafe.SkipInit(out bool? menuBackup);
			Unsafe.SkipInit(out bool? metalBackup);
			Unsafe.SkipInit(out bool? minimizedBackup);
			Unsafe.SkipInit(out bool? modalBackup);
			Unsafe.SkipInit(out bool? mouseGrabbedBackup);
			Unsafe.SkipInit(out bool? openGLBackup);
			Unsafe.SkipInit(out IntPtr? parentBackup);
			Unsafe.SkipInit(out bool? resizableBackup);
			Unsafe.SkipInit(out string? titleBackup);
			Unsafe.SkipInit(out bool? transparentBackup);
			Unsafe.SkipInit(out bool? tooltipBackup);
			Unsafe.SkipInit(out bool? utilityBackup);
			Unsafe.SkipInit(out bool? vulkanBackup);
			Unsafe.SkipInit(out int? widthBackup);
			Unsafe.SkipInit(out int? xBackup);
			Unsafe.SkipInit(out int? yBackup);

			if (properties is null)
			{
				propertiesUsed = [];

				if (alwaysOnTop is bool alwaysOnTopValue)
				{
					propertiesUsed.TrySetBooleanValue(PropertyNames.CreateAlwaysOnTopBoolean, alwaysOnTopValue);
				}

				if (bordered is bool borderedValue)
				{
					propertiesUsed.TrySetBooleanValue(PropertyNames.CreateBorderlessBoolean, !borderedValue);
				}

				if (constrainPopup is bool constrainPopupValue)
				{
					propertiesUsed.TrySetBooleanValue(PropertyNames.CreateConstrainPopupBoolean, constrainPopupValue);
				}

				if (externalGraphicsContext is bool externalGraphicsContextValue)
				{
					propertiesUsed.TrySetBooleanValue(PropertyNames.CreateExternalGraphicsContextBoolean, externalGraphicsContextValue);
				}

				if (focusable is bool focusableValue)
				{
					propertiesUsed.TrySetBooleanValue(PropertyNames.CreateFocusableBoolean, focusableValue);
				}

				if (fullscreen is bool fullscreenValue)
				{
					propertiesUsed.TrySetBooleanValue(PropertyNames.CreateFullscreenBoolean, fullscreenValue);
				}

				if (height is int heightValue)
				{
					propertiesUsed.TrySetNumberValue(PropertyNames.CreateHeightNumber, heightValue);
				}

				if (width is int widthValue)
				{
					propertiesUsed.TrySetNumberValue(PropertyNames.CreateWidthNumber, widthValue);
				}

				if (hidden is bool hiddenValue)
				{
					propertiesUsed.TrySetBooleanValue(PropertyNames.CreateHiddenBoolean, hiddenValue);
				}

				if (highPixelDensity is bool highPixelDensityValue)
				{
					propertiesUsed.TrySetBooleanValue(PropertyNames.CreateHighPixelDensityBoolean, highPixelDensityValue);
				}

				if (maximized is bool maximizedValue)
				{
					propertiesUsed.TrySetBooleanValue(PropertyNames.CreateMaximizedBoolean, maximizedValue);
				}

				if (menu is bool menuValue)
				{
					propertiesUsed.TrySetBooleanValue(PropertyNames.CreateMenuBoolean, menuValue);
				}

				if (metal is bool metalValue)
				{
					propertiesUsed.TrySetBooleanValue(PropertyNames.CreateMetalBoolean, metalValue);
				}

				if (minimized is bool minimizedValue)
				{
					propertiesUsed.TrySetBooleanValue(PropertyNames.CreateMinimizedBoolean, minimizedValue);
				}

				if (modal is bool modalValue)
				{
					propertiesUsed.TrySetBooleanValue(PropertyNames.CreateModalBoolean, modalValue);
				}

				if (mouseGrabbed is bool mouseGrabbedValue)
				{
					propertiesUsed.TrySetBooleanValue(PropertyNames.CreateMouseGrabbedBoolean, mouseGrabbedValue);
				}

				if (openGL is bool openGLValue)
				{
					propertiesUsed.TrySetBooleanValue(PropertyNames.CreateOpenGLBoolean, openGLValue);
				}

				if (parent is not null)
				{
					propertiesUsed.TrySetPointerValue(PropertyNames.CreateParentPointer, unchecked((IntPtr)parent.mWindow));
				}

				if (resizable is bool resizableValue)
				{
					propertiesUsed.TrySetBooleanValue(PropertyNames.CreateResizableBoolean, resizableValue);
				}

				if (title is not null)
				{
					propertiesUsed.TrySetStringValue(PropertyNames.CreateTitleString, title);
				}

				if (tooltip is bool tooltipValue)
				{
					propertiesUsed.TrySetBooleanValue(PropertyNames.CreateTooltipBoolean, tooltipValue);
				}

				if (transparent is bool transparentValue)
				{
					propertiesUsed.TrySetBooleanValue(PropertyNames.CreateTransparentBoolean, transparentValue);
				}

				if (utility is bool utilityValue)
				{
					propertiesUsed.TrySetBooleanValue(PropertyNames.CreateUtilityBoolean, utilityValue);
				}

				if (vulkan is bool vulkanValue)
				{
					propertiesUsed.TrySetBooleanValue(PropertyNames.CreateVulkanBoolean, vulkanValue);
				}

				if (x is int xValue)
				{
					propertiesUsed.TrySetNumberValue(PropertyNames.CreateXNumber, xValue);
				}

				if (y is int yValue)
				{
					propertiesUsed.TrySetNumberValue(PropertyNames.CreateYNumber, yValue);
				}
			}
			else
			{
				propertiesUsed = properties;

				if (alwaysOnTop is bool alwaysOnTopValue)
				{
					alwaysOnTopBackup = propertiesUsed.TryGetBooleanValue(PropertyNames.CreateAlwaysOnTopBoolean, out var existingAlwaysOnTopValue)
						? existingAlwaysOnTopValue
						: null;

					propertiesUsed.TrySetBooleanValue(PropertyNames.CreateAlwaysOnTopBoolean, alwaysOnTopValue);
				}

				if (bordered is bool borderedValue)
				{
					borderedBackup = propertiesUsed.TryGetBooleanValue(PropertyNames.CreateBorderlessBoolean, out var existingBorderlessValue)
						? !existingBorderlessValue
						: null;

					propertiesUsed.TrySetBooleanValue(PropertyNames.CreateBorderlessBoolean, !borderedValue);
				}

				if (constrainPopup is bool constrainPopupValue)
				{
					constrainPopupBackup = propertiesUsed.TryGetBooleanValue(PropertyNames.CreateConstrainPopupBoolean, out var existingConstrainPopupValue)
						? existingConstrainPopupValue
						: null;

					propertiesUsed.TrySetBooleanValue(PropertyNames.CreateConstrainPopupBoolean, constrainPopupValue);
				}

				if (externalGraphicsContext is bool externalGraphicsContextValue)
				{
					externalGraphicsContextBackup = propertiesUsed.TryGetBooleanValue(PropertyNames.CreateExternalGraphicsContextBoolean, out var existingExternalGraphicsContextValue)
						? existingExternalGraphicsContextValue
						: null;

					propertiesUsed.TrySetBooleanValue(PropertyNames.CreateExternalGraphicsContextBoolean, externalGraphicsContextValue);
				}

				if (focusable is bool focusableValue)
				{
					focusableBackup = propertiesUsed.TryGetBooleanValue(PropertyNames.CreateFocusableBoolean, out var existingFocusableValue)
						? existingFocusableValue
						: null;

					propertiesUsed.TrySetBooleanValue(PropertyNames.CreateFocusableBoolean, focusableValue);
				}

				if (fullscreen is bool fullscreenValue)
				{
					fullscreenBackup = propertiesUsed.TryGetBooleanValue(PropertyNames.CreateFullscreenBoolean, out var existingFullscreenValue)
						? existingFullscreenValue
						: null;

					propertiesUsed.TrySetBooleanValue(PropertyNames.CreateFullscreenBoolean, fullscreenValue);
				}

				if (height is int heightValue)
				{
					heightBackup = propertiesUsed.TryGetNumberValue(PropertyNames.CreateHeightNumber, out var existingHeightValue)
						? unchecked((int)existingHeightValue)
						: null;

					propertiesUsed.TrySetNumberValue(PropertyNames.CreateHeightNumber, heightValue);
				}

				if (hidden is bool hiddenValue)
				{
					hiddenBackup = propertiesUsed.TryGetBooleanValue(PropertyNames.CreateHiddenBoolean, out var existingHiddenValue)
						? existingHiddenValue
						: null;

					propertiesUsed.TrySetBooleanValue(PropertyNames.CreateHiddenBoolean, hiddenValue);
				}

				if (highPixelDensity is bool highPixelDensityValue)
				{
					highPixelDensityBackup = propertiesUsed.TryGetBooleanValue(PropertyNames.CreateHighPixelDensityBoolean, out var existingHighPixelDensityValue)
						? existingHighPixelDensityValue
						: null;

					propertiesUsed.TrySetBooleanValue(PropertyNames.CreateHighPixelDensityBoolean, highPixelDensityValue);
				}

				if (maximized is bool maximizedValue)
				{
					maximizedBackup = propertiesUsed.TryGetBooleanValue(PropertyNames.CreateMaximizedBoolean, out var existingMaximizedValue)
						? existingMaximizedValue
						: null;

					propertiesUsed.TrySetBooleanValue(PropertyNames.CreateMaximizedBoolean, maximizedValue);
				}

				if (menu is bool menuValue)
				{
					menuBackup = propertiesUsed.TryGetBooleanValue(PropertyNames.CreateMenuBoolean, out var existingMenuValue)
						? existingMenuValue
						: null;

					propertiesUsed.TrySetBooleanValue(PropertyNames.CreateMenuBoolean, menuValue);
				}

				if (metal is bool metalValue)
				{
					metalBackup = propertiesUsed.TryGetBooleanValue(PropertyNames.CreateMetalBoolean, out var existingMetalValue)
						? existingMetalValue
						: null;

					propertiesUsed.TrySetBooleanValue(PropertyNames.CreateMetalBoolean, metalValue);
				}

				if (minimized is bool minimizedValue)
				{
					minimizedBackup = propertiesUsed.TryGetBooleanValue(PropertyNames.CreateMinimizedBoolean, out var existingMinimizedValue)
						? existingMinimizedValue
						: null;

					propertiesUsed.TrySetBooleanValue(PropertyNames.CreateMinimizedBoolean, minimizedValue);
				}

				if (modal is bool modalValue)
				{
					modalBackup = propertiesUsed.TryGetBooleanValue(PropertyNames.CreateModalBoolean, out var existingModalValue)
						? existingModalValue
						: null;

					propertiesUsed.TrySetBooleanValue(PropertyNames.CreateModalBoolean, modalValue);
				}

				if (mouseGrabbed is bool mouseGrabbedValue)
				{
					mouseGrabbedBackup = propertiesUsed.TryGetBooleanValue(PropertyNames.CreateMouseGrabbedBoolean, out var existingMouseGrabbedValue)
						? existingMouseGrabbedValue
						: null;

					propertiesUsed.TrySetBooleanValue(PropertyNames.CreateMouseGrabbedBoolean, mouseGrabbedValue);
				}

				if (openGL is bool openGLValue)
				{
					openGLBackup = propertiesUsed.TryGetBooleanValue(PropertyNames.CreateOpenGLBoolean, out var existingOpenGLValue)
						? existingOpenGLValue
						: null;

					propertiesUsed.TrySetBooleanValue(PropertyNames.CreateOpenGLBoolean, openGLValue);
				}

				if (parent is not null)
				{
					parentBackup = propertiesUsed.TryGetPointerValue(PropertyNames.CreateParentPointer, out var existingParentValue)
						? existingParentValue
						: null;

					propertiesUsed.TrySetPointerValue(PropertyNames.CreateParentPointer, unchecked((IntPtr)parent.mWindow));
				}

				if (resizable is bool resizableValue)
				{
					resizableBackup = propertiesUsed.TryGetBooleanValue(PropertyNames.CreateResizableBoolean, out var existingResizableValue)
						? existingResizableValue
						: null;

					propertiesUsed.TrySetBooleanValue(PropertyNames.CreateResizableBoolean, resizableValue);
				}

				if (title is not null)
				{
					titleBackup = propertiesUsed.TryGetStringValue(PropertyNames.CreateTitleString, out var existingTitleValue)
						? existingTitleValue
						: null;

					propertiesUsed.TrySetStringValue(PropertyNames.CreateTitleString, title);
				}

				if (transparent is bool transparentValue)
				{
					transparentBackup = propertiesUsed.TryGetBooleanValue(PropertyNames.CreateTransparentBoolean, out var existingTransparentValue)
						? existingTransparentValue
						: null;

					propertiesUsed.TrySetBooleanValue(PropertyNames.CreateTransparentBoolean, transparentValue);
				}

				if (tooltip is bool tooltipValue)
				{
					tooltipBackup = propertiesUsed.TryGetBooleanValue(PropertyNames.CreateTooltipBoolean, out var existingTooltipValue)
						? existingTooltipValue
						: null;

					propertiesUsed.TrySetBooleanValue(PropertyNames.CreateTooltipBoolean, tooltipValue);
				}

				if (utility is bool utilityValue)
				{
					utilityBackup = propertiesUsed.TryGetBooleanValue(PropertyNames.CreateUtilityBoolean, out var existingUtilityValue)
						? existingUtilityValue
						: null;

					propertiesUsed.TrySetBooleanValue(PropertyNames.CreateUtilityBoolean, utilityValue);
				}

				if (vulkan is bool vulkanValue)
				{
					vulkanBackup = propertiesUsed.TryGetBooleanValue(PropertyNames.CreateVulkanBoolean, out var existingVulkanValue)
						? existingVulkanValue
						: null;

					propertiesUsed.TrySetBooleanValue(PropertyNames.CreateVulkanBoolean, vulkanValue);
				}

				if (width is int widthValue)
				{
					widthBackup = propertiesUsed.TryGetNumberValue(PropertyNames.CreateWidthNumber, out var existingWidthValue)
						? unchecked((int)existingWidthValue)
						: null;

					propertiesUsed.TrySetNumberValue(PropertyNames.CreateWidthNumber, widthValue);
				}

				if (x is int xValue)
				{
					xBackup = propertiesUsed.TryGetNumberValue(PropertyNames.CreateXNumber, out var existingXValue)
						? unchecked((int)existingXValue)
						: null;

					propertiesUsed.TrySetNumberValue(PropertyNames.CreateXNumber, xValue);
				}

				if (y is int yValue)
				{
					yBackup = propertiesUsed.TryGetNumberValue(PropertyNames.CreateYNumber, out var existingYValue)
						? unchecked((int)existingYValue)
						: null;
					propertiesUsed.TrySetNumberValue(PropertyNames.CreateYNumber, yValue);
				}
			}

			try
			{
				var windowPtr = SDL_CreateWindowWithProperties(propertiesUsed.Id);

				if (windowPtr is null)
				{
					window = null;
					return false;
				}

				window = TFactory.Create(windowPtr, register: true);
				return true;
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

					if (alwaysOnTop.HasValue)
					{
						if (alwaysOnTopBackup is bool alwaysOnTopValue)
						{
							propertiesUsed.TrySetBooleanValue(PropertyNames.CreateAlwaysOnTopBoolean, alwaysOnTopValue);
						}
						else
						{
							propertiesUsed.TryRemove(PropertyNames.CreateAlwaysOnTopBoolean);
						}
					}

					if (bordered.HasValue)
					{
						if (borderedBackup is bool borderedValue)
						{
							propertiesUsed.TrySetBooleanValue(PropertyNames.CreateBorderlessBoolean, !borderedValue);
						}
						else
						{
							propertiesUsed.TryRemove(PropertyNames.CreateBorderlessBoolean);
						}
					}

					if (constrainPopup.HasValue)
					{
						if (constrainPopupBackup is bool constrainPopupValue)
						{
							propertiesUsed.TrySetBooleanValue(PropertyNames.CreateConstrainPopupBoolean, constrainPopupValue);
						}
						else
						{
							propertiesUsed.TryRemove(PropertyNames.CreateConstrainPopupBoolean);
						}
					}

					if (externalGraphicsContext.HasValue)
					{
						if (externalGraphicsContextBackup is bool externalGraphicsContextValue)
						{
							propertiesUsed.TrySetBooleanValue(PropertyNames.CreateExternalGraphicsContextBoolean, externalGraphicsContextValue);
						}
						else
						{
							propertiesUsed.TryRemove(PropertyNames.CreateExternalGraphicsContextBoolean);
						}
					}

					if (focusable.HasValue)
					{
						if (focusableBackup is bool focusableValue)
						{
							propertiesUsed.TrySetBooleanValue(PropertyNames.CreateFocusableBoolean, focusableValue);
						}
						else
						{
							propertiesUsed.TryRemove(PropertyNames.CreateFocusableBoolean);
						}
					}

					if (fullscreen.HasValue)
					{
						if (fullscreenBackup is bool fullscreenValue)
						{
							propertiesUsed.TrySetBooleanValue(PropertyNames.CreateFullscreenBoolean, fullscreenValue);
						}
						else
						{
							propertiesUsed.TryRemove(PropertyNames.CreateFullscreenBoolean);
						}
					}

					if (height.HasValue)
					{
						if (heightBackup is int heightValue)
						{
							propertiesUsed.TrySetNumberValue(PropertyNames.CreateHeightNumber, heightValue);
						}
						else
						{
							propertiesUsed.TryRemove(PropertyNames.CreateHeightNumber);
						}
					}

					if (hidden.HasValue)
					{
						if (hiddenBackup is bool hiddenValue)
						{
							propertiesUsed.TrySetBooleanValue(PropertyNames.CreateHiddenBoolean, hiddenValue);
						}
						else
						{
							propertiesUsed.TryRemove(PropertyNames.CreateHiddenBoolean);
						}
					}

					if (highPixelDensity.HasValue)
					{
						if (highPixelDensityBackup is bool highPixelDensityValue)
						{
							propertiesUsed.TrySetBooleanValue(PropertyNames.CreateHighPixelDensityBoolean, highPixelDensityValue);
						}
						else
						{
							propertiesUsed.TryRemove(PropertyNames.CreateHighPixelDensityBoolean);
						}
					}

					if (maximized.HasValue)
					{
						if (maximizedBackup is bool maximizedValue)
						{
							propertiesUsed.TrySetBooleanValue(PropertyNames.CreateMaximizedBoolean, maximizedValue);
						}
						else
						{
							propertiesUsed.TryRemove(PropertyNames.CreateMaximizedBoolean);
						}
					}

					if (menu.HasValue)
					{
						if (menuBackup is bool menuValue)
						{
							propertiesUsed.TrySetBooleanValue(PropertyNames.CreateMenuBoolean, menuValue);
						}
						else
						{
							propertiesUsed.TryRemove(PropertyNames.CreateMenuBoolean);
						}
					}

					if (metal.HasValue)
					{
						if (metalBackup is bool metalValue)
						{
							propertiesUsed.TrySetBooleanValue(PropertyNames.CreateMetalBoolean, metalValue);
						}
						else
						{
							propertiesUsed.TryRemove(PropertyNames.CreateMetalBoolean);
						}
					}

					if (minimized.HasValue)
					{
						if (minimizedBackup is bool minimizedValue)
						{
							propertiesUsed.TrySetBooleanValue(PropertyNames.CreateMinimizedBoolean, minimizedValue);
						}
						else
						{
							propertiesUsed.TryRemove(PropertyNames.CreateMinimizedBoolean);
						}
					}

					if (modal.HasValue)
					{
						if (modalBackup is bool modalValue)
						{
							propertiesUsed.TrySetBooleanValue(PropertyNames.CreateModalBoolean, modalValue);
						}
						else
						{
							propertiesUsed.TryRemove(PropertyNames.CreateModalBoolean);
						}
					}

					if (mouseGrabbed.HasValue)
					{
						if (mouseGrabbedBackup is bool mouseGrabbedValue)
						{
							propertiesUsed.TrySetBooleanValue(PropertyNames.CreateMouseGrabbedBoolean, mouseGrabbedValue);
						}
						else
						{
							propertiesUsed.TryRemove(PropertyNames.CreateMouseGrabbedBoolean);
						}
					}

					if (openGL.HasValue)
					{
						if (openGLBackup is bool openGLValue)
						{
							propertiesUsed.TrySetBooleanValue(PropertyNames.CreateOpenGLBoolean, openGLValue);
						}
						else
						{
							propertiesUsed.TryRemove(PropertyNames.CreateOpenGLBoolean);
						}
					}

					if (parent is not null)
					{
						if (parentBackup is IntPtr parentValue)
						{
							propertiesUsed.TrySetPointerValue(PropertyNames.CreateParentPointer, parentValue);
						}
						else
						{
							propertiesUsed.TryRemove(PropertyNames.CreateParentPointer);
						}
					}

					if (resizable.HasValue)
					{
						if (resizableBackup is bool resizableValue)
						{
							propertiesUsed.TrySetBooleanValue(PropertyNames.CreateResizableBoolean, resizableValue);
						}
						else
						{
							propertiesUsed.TryRemove(PropertyNames.CreateResizableBoolean);
						}
					}

					if (title is not null)
					{
						if (titleBackup is string titleValue)
						{
							propertiesUsed.TrySetStringValue(PropertyNames.CreateTitleString, titleValue);
						}
						else
						{
							propertiesUsed.TryRemove(PropertyNames.CreateTitleString);
						}
					}

					if (transparent.HasValue)
					{
						if (transparentBackup is bool transparentValue)
						{
							propertiesUsed.TrySetBooleanValue(PropertyNames.CreateTransparentBoolean, transparentValue);
						}
						else
						{
							propertiesUsed.TryRemove(PropertyNames.CreateTransparentBoolean);
						}
					}

					if (tooltip.HasValue)
					{
						if (tooltipBackup is bool tooltipValue)
						{
							propertiesUsed.TrySetBooleanValue(PropertyNames.CreateTooltipBoolean, tooltipValue);
						}
						else
						{
							propertiesUsed.TryRemove(PropertyNames.CreateTooltipBoolean);
						}
					}

					if (utility.HasValue)
					{
						if (utilityBackup is bool utilityValue)
						{
							propertiesUsed.TrySetBooleanValue(PropertyNames.CreateUtilityBoolean, utilityValue);
						}
						else
						{
							propertiesUsed.TryRemove(PropertyNames.CreateUtilityBoolean);
						}
					}

					if (vulkan.HasValue)
					{
						if (vulkanBackup is bool vulkanValue)
						{
							propertiesUsed.TrySetBooleanValue(PropertyNames.CreateVulkanBoolean, vulkanValue);
						}
						else
						{
							propertiesUsed.TryRemove(PropertyNames.CreateVulkanBoolean);
						}
					}

					if (width.HasValue)
					{
						if (widthBackup is int widthValue)
						{
							propertiesUsed.TrySetNumberValue(PropertyNames.CreateWidthNumber, widthValue);
						}
						else
						{
							propertiesUsed.TryRemove(PropertyNames.CreateWidthNumber);
						}
					}

					if (x.HasValue)
					{
						if (xBackup is int xValue)
						{
							propertiesUsed.TrySetNumberValue(PropertyNames.CreateXNumber, xValue);
						}
						else
						{
							propertiesUsed.TryRemove(PropertyNames.CreateXNumber);
						}
					}

					if (y.HasValue)
					{
						if (yBackup is int yValue)
						{
							propertiesUsed.TrySetNumberValue(PropertyNames.CreateYNumber, yValue);
						}
						else
						{
							propertiesUsed.TryRemove(PropertyNames.CreateYNumber);
						}
					}
				}
			}
		}
	}

	public static bool TryCreate([NotNullWhen(true)] out Window? window, bool? alwaysOnTop = default, bool? bordered = default, bool? constrainPopup = default, bool? externalGraphicsContext = default, bool? focusable = default,
		bool? fullscreen = default, int? height = default, bool? hidden = default, bool? highPixelDensity = default, bool? maximized = default, bool? menu = default, bool? metal = default, bool? minimized = default,
		bool? modal = default, bool? mouseGrabbed = default, bool? openGL = default, Window? parent = default, bool? resizable = default, string? title = default, bool? transparent = default, bool? tooltip = default,
		bool? utility = default, bool? vulkan = default, int? width = default, int? x = default, int? y = default, Properties? properties = default)
		=> TryCreate<RegisteredDriverOrGenericFallbackDriverFactory, Window>(
			out window, alwaysOnTop, bordered, constrainPopup, externalGraphicsContext, focusable,
			fullscreen, height, hidden, highPixelDensity, maximized, menu, metal, minimized,
			modal, mouseGrabbed, openGL, parent, resizable, title, transparent, tooltip,
			utility, vulkan, width, x, y, properties
		);

	internal static bool TryCreate<TDriver>([NotNullWhen(true)] out Window<TDriver>? window, bool? alwaysOnTop = default, bool? bordered = default, bool? constrainPopup = default, bool? externalGraphicsContext = default, bool? focusable = default,
		bool? fullscreen = default, int? height = default, bool? hidden = default, bool? highPixelDensity = default, bool? maximized = default, bool? menu = default, bool? metal = default, bool? minimized = default,
		bool? modal = default, bool? mouseGrabbed = default, bool? openGL = default, Window? parent = default, bool? resizable = default, string? title = default, bool? transparent = default, bool? tooltip = default,
		bool? utility = default, bool? vulkan = default, int? width = default, int? x = default, int? y = default, Properties? properties = default)
		where TDriver : notnull, IWindowingDriver
	{
		if (!WindowingDriverExtensions.get_IsActive<TDriver>())
		{
			window = null;
			return false;
		}

		return TryCreateUnchecked(
			out window, alwaysOnTop, bordered, constrainPopup, externalGraphicsContext, focusable,
			fullscreen, height, hidden, highPixelDensity, maximized, menu, metal, minimized,
			modal, mouseGrabbed, openGL, parent, resizable, title, transparent, tooltip,
			utility, vulkan, width, x, y, properties
		);
	}

	internal static bool TryCreateUnchecked<TDriver>([NotNullWhen(true)] out Window<TDriver>? window, bool? alwaysOnTop = default, bool? bordered = default, bool? constrainPopup = default, bool? externalGraphicsContext = default, bool? focusable = default,
		bool? fullscreen = default, int? height = default, bool? hidden = default, bool? highPixelDensity = default, bool? maximized = default, bool? menu = default, bool? metal = default, bool? minimized = default,
		bool? modal = default, bool? mouseGrabbed = default, bool? openGL = default, Window? parent = default, bool? resizable = default, string? title = default, bool? transparent = default, bool? tooltip = default,
		bool? utility = default, bool? vulkan = default, int? width = default, int? x = default, int? y = default, Properties? properties = default)
		where TDriver : notnull, IWindowingDriver
		=> TryCreate<Factory<TDriver>, Window<TDriver>>(
			out window, alwaysOnTop, bordered, constrainPopup, externalGraphicsContext, focusable,
			fullscreen, height, hidden, highPixelDensity, maximized, menu, metal, minimized,
			modal, mouseGrabbed, openGL, parent, resizable, title, transparent, tooltip,
			utility, vulkan, width, x, y, properties
		);

	public static bool TryCreateWithRenderer(string title, int width, int height, [NotNullWhen(true)] out Window? window, [NotNullWhen(true)] out Renderer? renderer, WindowFlags flags = WindowFlags.None)
	{
		unsafe
		{
			var titleUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(title);
			try
			{
				SDL_Window* windowPtr;
				Renderer.SDL_Renderer* rendererPtr;

				if (!(bool)SDL_CreateWindowAndRenderer(titleUtf8, width, height, flags, &windowPtr, &rendererPtr)
					|| windowPtr is null || rendererPtr is null)
				{
					window = null;
					renderer = null;
					return false;
				}

				if (!Renderer.TryGetOrCreate(rendererPtr, out renderer))
				{
					SDL_DestroyWindow(windowPtr);

					window = null;
					renderer = null;
					return false;
				}

				window = RegisteredDriverOrGenericFallbackDriverFactory.Create(windowPtr, register: true);
				return true;
			}
			finally
			{
				Utf8StringMarshaller.Free(titleUtf8);
			}
		}
	}

	private protected abstract bool TryCreatePopupImpl(int x, int y, int width, int height, [NotNullWhen(true)] out Window? popupWindow, WindowFlags flags = default);

	public bool TryCreatePopup(int x, int y, int width, int height, [NotNullWhen(true)] out Window? popupWindow, WindowFlags flags = default)
		=> TryCreatePopupImpl(x, y, width, height, out popupWindow, flags);

	/// <summary>
	/// Tries to create a new <see cref="Renderer"/> for this window
	/// </summary>
	/// <param name="renderer">The resulting <see cref="Renderer"/>, if the method returns <see langword="true"/>; otherwise, <see langword="null"/></param>
	/// <param name="driverNames">An optional list of driver names to try, in order of preference. An empty list (the default) lets SDL automatically choose the best available driver for you.</param>
	/// <returns><c><see langword="true"/></c>, if the renderer was created successfully; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// If you want to try specific rendering drivers, you can provide their names for the <paramref name="driverNames"/> parameter.
	/// You can use the <see cref="IRenderingDriver.Name"/> property of the pre-defined drivers for this (e.g. <see cref="OpenGL.Name"/>),
	/// or you can get the list of all available drivers at runtime using <see cref="IRenderingDriver.AvailableDriverNames"/> property.
	/// </para>
	/// <para>
	/// SDL will attempt to create a renderer using each of the specified driver names in order, and will return the first one that succeeds.
	/// </para>
	/// <para>
	/// Leaving the <paramref name="driverNames"/> parameter empty (the default) lets SDL automatically choose the best available driver for you,
	/// which is usually what you want unless you have specific requirements or want to test multiple drivers.
	/// </para>
	/// <para>
	/// The default renderering size of the resulting <paramref name="renderer"/> will match the size of the window in pixels,
	/// but you can change the content size and scaling later using the <see cref="Renderer.LogicalPresentation"/> property if needed.
	/// </para>
	/// <para>
	/// The resulting <paramref name="renderer"/> will be of the <see cref="Renderer{TDriver}"/> type with a specific <see cref="IRenderingDriver">rendering driver</see> as the type argument.
	/// If you need driver-specific functionality, you can type check and cast the resulting <paramref name="renderer"/> to the appropriate <see cref="Renderer{TDriver}"/> type later
	/// or you can use the <see cref="TryCreateRenderer{TDriver}(out Renderer{TDriver}?)"/> method alternatively.
	/// </para>
	/// <para>
	/// This method should only be called from the main thread.
	/// </para>
	/// </remarks>
	public bool TryCreateRenderer([NotNullWhen(true)] out Renderer? renderer, params ReadOnlySpan<string> driverNames)
	{
		unsafe
		{
			byte* joinedNames = null;
			try
			{
				if (driverNames.Length is > 0)
				{
					// driver names will be joined into a single, ','-separated, null-terminated ASCII string on the stack

					var totalLength = 0;
					foreach (var name in driverNames)
					{
						totalLength += name.Length;
					}

					joinedNames = unchecked((byte*)Utilities.NativeMemory.Malloc(unchecked((nuint)(
						totalLength                // total number of characters
						+ (driverNames.Length - 1) // number of ',' separators
						+ 1                        // null-terminator
					))));

					if (joinedNames is null)
					{
						renderer = null;
						return false;
					}

					var index = 0;
					var joinedNamesPtr = joinedNames;
					while (true)
					{
						var name = driverNames[index];

						foreach (var ch in name)
						{
							if (ch is '\0')
							{
								break;
							}

							*joinedNamesPtr++ = ch is <= '\x7F'
								? unchecked((byte)ch)
								: (byte)'\xFF'; // replace non-ASCII characters with placeholder ('\xFF' is invalid in UTF-8 and therefore safe to use here)
						}

						if (!(++index < driverNames.Length))
						{
							break;
						}

						*joinedNamesPtr++ = (byte)',';
					}

					*joinedNamesPtr = (byte)'\0';
				}

				return Renderer.TryGetOrCreate(Renderer.SDL_CreateRenderer(mWindow, name: joinedNames), out renderer);
			}
			finally
			{
				Utilities.NativeMemory.Free(joinedNames);
			}
		}
	}

	private unsafe bool TryCreateRenderer<TDriver>([NotNullWhen(true)] out Renderer<TDriver>? renderer, byte* driverName)
		where TDriver : notnull, IRenderingDriver
	{
		var rendererPtr = Renderer.SDL_CreateRenderer(mWindow, name: driverName);

		if (rendererPtr is null)
		{
			renderer = null;
			return false;
		}

		renderer = new(rendererPtr, register: true);
		return true;
	}

	/// <summary>
	/// Tries to create a new <see cref="Renderer{TDriver}"/> for this window
	/// </summary>
	/// <typeparam name="TDriver">The rendering driver type associated with the resulting renderer</typeparam>
	/// <param name="renderer">The resulting <see cref="Renderer{TDriver}"/>, if the method returns <see langword="true"/>; otherwise, <see langword="null"/></param>
	/// <returns><c><see langword="true"/></c>, if the renderer was created successfully; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// The resulting <paramref name="renderer"/> will be of the <see cref="Renderer{TDriver}"/> type with the specified <typeparamref name="TDriver"/> as the type argument.
	/// </para>
	/// <para>
	/// This method should only be called from the main thread.
	/// </para>
	/// </remarks>
	public bool TryCreateRenderer<TDriver>([NotNullWhen(true)] out Renderer<TDriver>? renderer)
		where TDriver : notnull, IRenderingDriver
	{
		unsafe
		{
			if (!TDriver.NameAscii.IsEmpty)
			{
				fixed (byte* driverName = TDriver.NameAscii)
				{
					return TryCreateRenderer(out renderer, driverName);
				}
			}
			else
			{
				return TryCreateRenderer(out renderer, driverName: null);
			}
		}
	}

	/// <summary>
	/// Tries to create a new <see cref="Renderer"/> for this window
	/// </summary>
	/// <param name="renderer">The resulting <see cref="Renderer"/>, if the method returns <see langword="true"/>; otherwise, <see langword="null"/></param>
	/// <param name="driverName">The name of the rendering driver to use, or <see langword="null"/> to let SDL automatically choose the best available driver for you</param>
	/// <param name="outputColorSpace">
	/// The color space to be used by renderer for presenting to the output display.
	/// The <see cref="Direct3D11">Direct3D 11</see>, <see cref="Direct3D12">Direct3D 12</see>, and <see cref="Metal">Metal</see> renderers support <see cref="ColorSpace.SrgbLinear"/>,
	/// which is a linear color space and supports HDR output. In that case, drawing still uses the sRGB color space, but individual values can go beyond <c>1.0</c>
	/// and floating point textures can be used for HDR content.
	/// If this parameter is <see langword="null"/> (the default), the output color space defaults to <see cref="ColorSpace.Srgb"/>.
	/// </param>
	/// <param name="presentVSync">
	/// The vertical synchronization (VSync) mode or interval to be used by the renderer.
	/// Can be specified to be <see cref="RendererVSync.Disabled"/> to disable VSync,
	/// <see cref="RendererVSync.Adaptive"/> to enable late swap tearing (adaptive VSync) if supported,
	/// or the result of the <see cref="RendererVSyncExtensions.Interval(int)"/> method to specify a custom VSync interval.
	/// You can specify a custom interval of <c>1</c> to synchronize to present of the renderer with <em>every</em> vertical refresh,
	/// <c>2</c> to synchronize it with <em>every second</em> vertical refresh, and so on.
	/// If this parameter is <see langword="null"/> (the default), the VSync mode defaults to <see cref="RendererVSync.Disabled"/>.
	/// </param>
	/// <param name="properties">Additional properties to use when creating the renderer</param>
	/// <returns><c><see langword="true"/></c>, if the renderer was created successfully; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// The resulting <paramref name="renderer"/> will be of the <see cref="Renderer{TDriver}"/> type with a specific <see cref="IRenderingDriver">rendering driver</see> as the type argument.
	/// If you need driver-specific functionality, you can type check and cast the resulting <paramref name="renderer"/> to the appropriate <see cref="Renderer{TDriver}"/> type later
	/// or you can use the <see cref="TryCreateRenderer{TDriver}(out Renderer{TDriver}?)"/> method alternatively.
	/// </para>
	/// <para>
	/// This method should only be called from the main thread.
	/// </para>
	/// </remarks>
	public bool TryCreateRenderer([NotNullWhen(true)] out Renderer? renderer, string? driverName = default, ColorSpace? outputColorSpace = default, RendererVSync? presentVSync = default, Properties? properties = default)
	{
		unsafe
		{
			Properties propertiesUsed;
			Unsafe.SkipInit(out string? driverNameBackup);
			Unsafe.SkipInit(out IntPtr? windowBackup);
			Unsafe.SkipInit(out ColorSpace? outputColorSpaceBackup);
			Unsafe.SkipInit(out RendererVSync? presentVSyncBackup);

			if (properties is null)
			{
				propertiesUsed = [];

				if (driverName is not null)
				{
					propertiesUsed.TrySetStringValue(Renderer.PropertyNames.CreateNameString, driverName);
				}

				// setting SDL_PROP_RENDERER_CREATE_WINDOW_POINTER is required, except for when we want to create a software renderer,
				// and we'll handle software renderers separately via Renderer<Software>.TryCreateForSurface
				propertiesUsed.TrySetPointerValue(Renderer.PropertyNames.CreateWindowPointer, unchecked((IntPtr)mWindow));

				if (outputColorSpace is ColorSpace outputColorSpaceValue)
				{
					propertiesUsed.TrySetNumberValue(Renderer.PropertyNames.CreateOutputColorSpaceNumber, unchecked((uint)outputColorSpaceValue));
				}

				if (presentVSync is RendererVSync presentVSyncValue)
				{
					propertiesUsed.TrySetNumberValue(Renderer.PropertyNames.CreatePresentVSyncNumber, unchecked((int)presentVSyncValue));
				}
			}
			else
			{
				propertiesUsed = properties;

				if (driverName is not null)
				{
					driverNameBackup = propertiesUsed.TryGetStringValue(Renderer.PropertyNames.CreateNameString, out var existingDriverName)
						? existingDriverName
						: null;

					propertiesUsed.TrySetStringValue(Renderer.PropertyNames.CreateNameString, driverName);
				}

				windowBackup = propertiesUsed.TryGetPointerValue(Renderer.PropertyNames.CreateWindowPointer, out var existingWindowPtr)
					? existingWindowPtr
					: null;

				// setting SDL_PROP_RENDERER_CREATE_WINDOW_POINTER is required, except for when we want to create a software renderer,
				// and we'll handle software renderers separately via Renderer<Software>.TryCreateForSurface
				propertiesUsed.TrySetPointerValue(Renderer.PropertyNames.CreateWindowPointer, unchecked((IntPtr)mWindow));

				if (outputColorSpace is ColorSpace outputColorSpaceValue)
				{
					outputColorSpaceBackup = propertiesUsed.TryGetNumberValue(Renderer.PropertyNames.CreateOutputColorSpaceNumber, out var existingOutputColorSpaceValue)
						? unchecked((ColorSpace)existingOutputColorSpaceValue)
						: null;

					propertiesUsed.TrySetNumberValue(Renderer.PropertyNames.CreateOutputColorSpaceNumber, unchecked((uint)outputColorSpaceValue));
				}

				if (presentVSync is RendererVSync presentVSyncValue)
				{
					presentVSyncBackup = propertiesUsed.TryGetNumberValue(Renderer.PropertyNames.CreatePresentVSyncNumber, out var existingPresentVSyncValue)
						? unchecked((RendererVSync)existingPresentVSyncValue)
						: null;

					propertiesUsed.TrySetNumberValue(Renderer.PropertyNames.CreatePresentVSyncNumber, unchecked((int)presentVSyncValue));
				}
			}

			try
			{
				return Renderer.TryGetOrCreate(Renderer.SDL_CreateRendererWithProperties(propertiesUsed.Id), out renderer);
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

					if (driverName is not null)
					{
						if (driverNameBackup is not null)
						{
							propertiesUsed.TrySetStringValue(Renderer.PropertyNames.CreateNameString, driverNameBackup);
						}
						else
						{
							propertiesUsed.TryRemove(Renderer.PropertyNames.CreateNameString);
						}
					}

					if (windowBackup is IntPtr windowPtr)
					{
						propertiesUsed.TrySetPointerValue(Renderer.PropertyNames.CreateWindowPointer, windowPtr);
					}
					else
					{
						propertiesUsed.TryRemove(Renderer.PropertyNames.CreateWindowPointer);
					}

					if (outputColorSpace.HasValue)
					{
						if (outputColorSpaceBackup is ColorSpace outputColorSpaceValue)
						{
							propertiesUsed.TrySetNumberValue(Renderer.PropertyNames.CreateOutputColorSpaceNumber, unchecked((uint)outputColorSpaceValue));
						}
						else
						{
							propertiesUsed.TryRemove(Renderer.PropertyNames.CreateOutputColorSpaceNumber);
						}
					}

					if (presentVSync.HasValue)
					{
						if (presentVSyncBackup is RendererVSync presentVSyncValue)
						{
							propertiesUsed.TrySetNumberValue(Renderer.PropertyNames.CreatePresentVSyncNumber, unchecked((int)presentVSyncValue));
						}
						else
						{
							propertiesUsed.TryRemove(Renderer.PropertyNames.CreatePresentVSyncNumber);
						}
					}
				}
			}
		}
	}

	// I know, I know, this isn't quite DRY..., but think about it: trying to abstract this into a shared method would require us to use some sort of aspect oriented programming.
	// We could achieve this with zero-cost abstractions by using static abstract interface members,
	// but than we would need to declare the interface, do the implementations by defining individual types for each one, and introduce a new generic parameter for the accepting method (and maybe more).
	// Or we could just duplicate the code, which is way simpler in this case.

	private unsafe bool TryCreateRenderer<TDriver>([NotNullWhen(true)] out Renderer<TDriver>? renderer, byte* driverName, ColorSpace? outputColorSpace = default, RendererVSync? presentVSync = default, Properties? properties = default)
		where TDriver : notnull, IRenderingDriver
	{
		unsafe
		{
			Properties propertiesUsed;
			Unsafe.SkipInit(out string? driverNameBackup);
			Unsafe.SkipInit(out IntPtr? windowBackup);
			Unsafe.SkipInit(out ColorSpace? outputColorSpaceBackup);
			Unsafe.SkipInit(out RendererVSync? presentVSyncBackup);

			if (properties is null)
			{
				propertiesUsed = [];

				if (driverName is not null)
				{
					propertiesUsed.TrySetNativeStringValue(Renderer.PropertyNames.CreateNameString, driverName);
				}

				// setting SDL_PROP_RENDERER_CREATE_WINDOW_POINTER is required, except for when we want to create a software renderer,
				// and we'll handle software renderers separately via Renderer<Software>.TryCreateForSurface
				propertiesUsed.TrySetPointerValue(Renderer.PropertyNames.CreateWindowPointer, unchecked((IntPtr)mWindow));

				if (outputColorSpace is ColorSpace outputColorSpaceValue)
				{
					propertiesUsed.TrySetNumberValue(Renderer.PropertyNames.CreateOutputColorSpaceNumber, unchecked((uint)outputColorSpaceValue));
				}

				if (presentVSync is RendererVSync presentVSyncValue)
				{
					propertiesUsed.TrySetNumberValue(Renderer.PropertyNames.CreatePresentVSyncNumber, unchecked((int)presentVSyncValue));
				}
			}
			else
			{
				propertiesUsed = properties;

				driverNameBackup = propertiesUsed.TryGetStringValue(Renderer.PropertyNames.CreateNameString, out var existingDriverName)
					? existingDriverName
					: null;

				// definitely overwrite existing driver name, even if null
				if (driverName is not null)
				{
					propertiesUsed.TrySetNativeStringValue(Renderer.PropertyNames.CreateNameString, driverName);
				}
				else
				{
					propertiesUsed.TryRemove(Renderer.PropertyNames.CreateNameString);
				}

				windowBackup = propertiesUsed.TryGetPointerValue(Renderer.PropertyNames.CreateWindowPointer, out var existingWindowPtr)
					? existingWindowPtr
					: null;

				// setting SDL_PROP_RENDERER_CREATE_WINDOW_POINTER is required, except for when we want to create a software renderer,
				// and we'll handle software renderers separately via Renderer<Software>.TryCreateForSurface
				propertiesUsed.TrySetPointerValue(Renderer.PropertyNames.CreateWindowPointer, unchecked((IntPtr)mWindow));

				if (outputColorSpace is ColorSpace outputColorSpaceValue)
				{
					outputColorSpaceBackup = propertiesUsed.TryGetNumberValue(Renderer.PropertyNames.CreateOutputColorSpaceNumber, out var existingOutputColorSpaceValue)
						? unchecked((ColorSpace)existingOutputColorSpaceValue)
						: null;

					propertiesUsed.TrySetNumberValue(Renderer.PropertyNames.CreateOutputColorSpaceNumber, unchecked((uint)outputColorSpaceValue));
				}

				if (presentVSync is RendererVSync presentVSyncValue)
				{
					presentVSyncBackup = propertiesUsed.TryGetNumberValue(Renderer.PropertyNames.CreatePresentVSyncNumber, out var existingPresentVSyncValue)
						? unchecked((RendererVSync)existingPresentVSyncValue)
						: null;

					propertiesUsed.TrySetNumberValue(Renderer.PropertyNames.CreatePresentVSyncNumber, unchecked((int)presentVSyncValue));
				}
			}

			try
			{
				var rendererPtr = Renderer.SDL_CreateRendererWithProperties(propertiesUsed.Id);

				if (rendererPtr is null)
				{
					renderer = null;
					return false;
				}

				renderer = new(rendererPtr, register: true);
				return true;
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

					if (driverNameBackup is not null)
					{
						propertiesUsed.TrySetStringValue(Renderer.PropertyNames.CreateNameString, driverNameBackup);
					}
					else
					{
						propertiesUsed.TryRemove(Renderer.PropertyNames.CreateNameString);
					}

					if (windowBackup is IntPtr windowPtr)
					{
						propertiesUsed.TrySetPointerValue(Renderer.PropertyNames.CreateWindowPointer, windowPtr);
					}
					else
					{
						propertiesUsed.TryRemove(Renderer.PropertyNames.CreateWindowPointer);
					}

					if (outputColorSpace.HasValue)
					{
						if (outputColorSpaceBackup is ColorSpace outputColorSpaceValue)
						{
							propertiesUsed.TrySetNumberValue(Renderer.PropertyNames.CreateOutputColorSpaceNumber, unchecked((uint)outputColorSpaceValue));
						}
						else
						{
							propertiesUsed.TryRemove(Renderer.PropertyNames.CreateOutputColorSpaceNumber);
						}
					}

					if (presentVSync.HasValue)
					{
						if (presentVSyncBackup is RendererVSync presentVSyncValue)
						{
							propertiesUsed.TrySetNumberValue(Renderer.PropertyNames.CreatePresentVSyncNumber, unchecked((int)presentVSyncValue));
						}
						else
						{
							propertiesUsed.TryRemove(Renderer.PropertyNames.CreatePresentVSyncNumber);
						}
					}
				}
			}
		}
	}

	/// <summary>
	/// Tries to create a new <see cref="Renderer{TDriver}"/> for this window
	/// </summary>
	/// <typeparam name="TDriver">The rendering driver type associated with the resulting renderer</typeparam>
	/// <param name="renderer">The resulting <see cref="Renderer{TDriver}"/>, if the method returns <see langword="true"/>; otherwise, <see langword="null"/></param>
	/// <param name="outputColorSpace">
	/// The color space to be used by renderer for presenting to the output display.
	/// The <see cref="Direct3D11">Direct3D 11</see>, <see cref="Direct3D12">Direct3D 12</see>, and <see cref="Metal">Metal</see> renderers support <see cref="ColorSpace.SrgbLinear"/>,
	/// which is a linear color space and supports HDR output. In that case, drawing still uses the sRGB color space, but individual values can go beyond <c>1.0</c>
	/// and floating point textures can be used for HDR content.
	/// If this parameter is <see langword="null"/> (the default), the output color space defaults to <see cref="ColorSpace.Srgb"/>.
	/// </param>
	/// <param name="presentVSync">
	/// The vertical synchronization (VSync) mode or interval to be used by the renderer.
	/// Can be specified to be <see cref="RendererVSync.Disabled"/> to disable VSync,
	/// <see cref="RendererVSync.Adaptive"/> to enable late swap tearing (adaptive VSync) if supported,
	/// or the result of the <see cref="RendererVSyncExtensions.Interval(int)"/> method to specify a custom VSync interval.
	/// You can specify a custom interval of <c>1</c> to synchronize to present of the renderer with <em>every</em> vertical refresh,
	/// <c>2</c> to synchronize it with <em>every second</em> vertical refresh, and so on.
	/// If this parameter is <see langword="null"/> (the default), the VSync mode defaults to <see cref="RendererVSync.Disabled"/>.
	/// </param>
	/// <param name="properties">Additional properties to use when creating the renderer</param>
	/// <returns><c><see langword="true"/></c>, if the renderer was created successfully; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// The resulting <paramref name="renderer"/> will be of the <see cref="Renderer{TDriver}"/> type with the specified <typeparamref name="TDriver"/> as the type argument.
	/// </para>
	/// <para>
	/// This method should only be called from the main thread.
	/// </para>
	/// </remarks>
	public bool TryCreateRenderer<TDriver>([NotNullWhen(true)] out Renderer<TDriver>? renderer, ColorSpace? outputColorSpace = default, RendererVSync? presentVSync = default, Properties? properties = default)
		where TDriver : notnull, IRenderingDriver
	{
		unsafe
		{
			if (!TDriver.NameAscii.IsEmpty)
			{
				fixed (byte* driverName = TDriver.NameAscii)
				{
					return TryCreateRenderer(out renderer, driverName, outputColorSpace, presentVSync, properties);
				}

			}
			else
			{
				return TryCreateRenderer(out renderer, driverName: null, outputColorSpace, presentVSync, properties);
			}
		}
	}

	public bool TryFlash(FlashOperation operation)
	{
		unsafe
		{
			return SDL_FlashWindow(mWindow, operation);
		}
	}

	public static bool TryGetFromId(uint id, [NotNullWhen(true)] out Window? result)
	{
		unsafe
		{
			return TryGetOrCreate(SDL_GetWindowFromID(id), out result);
		}
	}
	public bool TryGetICCProfile([NotNullWhen(true)] out NativeMemoryManager? iccProfileData)
	{
		unsafe
		{
			if (mWindow is null)
			{
				// For some reason SDL_GetWindowICCProfile isn't null-safe, so we have to check for null ourselves

				iccProfileData = null;
				return false;
			}

			Unsafe.SkipInit(out nuint size);

			var iccPtr = SDL_GetWindowICCProfile(mWindow, &size);

			if (iccPtr is null)
			{
				// SDL_GetWindowICCProfile returns null if there was an error, at least that's what the documentation says
				// I don't know if the various implementations of SDL_GetWindowICCProfile for different windowing backends
				// actually return something non-null when a window doesn't have an ICC profile.
				// But this doesn't quite matter, as the user is encouraged to check Error.TryGet when this method returns false anyway.

				iccProfileData = null;
				return false;
			}

			iccProfileData = new(iccPtr, size, &Utilities.NativeMemory.SDL_free);
			return true;
		}
	}

	internal unsafe static bool TryGetOrCreate(SDL_Window* window, [NotNullWhen(true)] out Window? result)
	{
		if (window is null)
		{
			result = null;
			return false;
		}

		var windowRef = mKnownInstances.GetOrAdd(unchecked((IntPtr)window), createRef);

		if (!windowRef.TryGetTarget(out result))
		{
			windowRef.SetTarget(result = create(window));
		}

		return true;

		static WeakReference<Window> createRef(IntPtr window) => new(create(unchecked((SDL_Window*)window)));

		static Window create(SDL_Window* window)
		{
			if (!TryCreateFromRegisteredDriver(window, register: false, out var result))
			{
				result = new Window<GenericFallbackWindowingDriver>(window, register: false);
			}

			return result;
		}
	}

	internal unsafe static bool TryGetOrCreate<TDriver>(SDL_Window* window, [NotNullWhen(true)] out Window<TDriver>? result)
		where TDriver : notnull, IWindowingDriver
	{
		if (window is null)
		{
			result = null;
			return false;
		}

		var windowRef = mKnownInstances.GetOrAdd(unchecked((IntPtr)window), createRef);

		if (!windowRef.TryGetTarget(out var baseResult))
		{
			windowRef.SetTarget(result = create(window));
		}
		else if (baseResult is Window<TDriver> typedResult)
		{
			// that's just fine

			result = typedResult;
		}
		else if (baseResult.Pointer is not null)
		{
			// this also means that baseResult.Pointer == texture
			// this indicates that we actually need the window to be of a different managed type than it currently is,
			// and we can't just recreate windows (like we do for displays),
			// therefore we should just fail in that case

			result = null;
			return false;
		}
		else
		{
			// this indicates that we somehow managed to not properly forget a managed instance that was disposed,
			// so we need to fully recreate the managed instance with the new type here

			result = create(window);
		}

		return true;

		static WeakReference<Window> createRef(IntPtr window) => new(create(unchecked((SDL_Window*)window)));

		static Window<TDriver> create(SDL_Window* window)
		{
			return new(window, register: false);
		}
	}

	public bool TryHide()
	{
		unsafe
		{
			return SDL_HideWindow(mWindow);
		}
	}

	public bool TryMaximize()
	{
		unsafe
		{
			return SDL_MaximizeWindow(mWindow);
		}
	}

	public bool TryMinimize()
	{
		unsafe
		{
			return SDL_MinimizeWindow(mWindow);
		}
	}

	public bool TryRaise()
	{
		unsafe
		{
			return SDL_RaiseWindow(mWindow);
		}
	}

	public bool TryResetSurface()
	{
		unsafe
		{
			return SDL_DestroyWindowSurface(mWindow);
		}
	}

	public bool TryRestore()
	{
		unsafe
		{
			return SDL_RestoreWindow(mWindow);
		}
	}

	public bool TryShow()
	{
		unsafe
		{
			return SDL_ShowWindow(mWindow);
		}
	}

	public bool TryShowSystemMenu(int x, int y)
	{
		unsafe
		{
			return SDL_ShowWindowSystemMenu(mWindow, x, y);
		}
	}

	public bool TrySync()
	{
		unsafe
		{
			return SDL_SyncWindow(mWindow);
		}
	}

	public bool TryUpdateSurface()
	{
		unsafe
		{
			return SDL_UpdateWindowSurface(mWindow);
		}
	}

	public bool TryUpdateSurfaceRects(ReadOnlySpan<Rect<int>> rects)
	{
		unsafe
		{
			fixed (Rect<int>* rectsPtr = rects)
			{
				return SDL_UpdateWindowSurfaceRects(mWindow, rectsPtr, rects.Length);
			}
		}
	}
}
