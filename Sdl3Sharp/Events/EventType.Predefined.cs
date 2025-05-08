using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Events;

partial struct EventType
{
	/// <summary>
	/// Provides <see cref="EventType">event types</see> for application events
	/// </summary>
	public static class Application
	{
		/// <summary>
		/// Gets the event type <c>Application.Quit</c>
		/// </summary>
		/// <value>
		/// The event type <c>Application.Quit</c>
		/// </value>
		/// <remarks>
		/// User-requested quit
		/// </remarks>
		public static EventType Quit { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Quit); }

		/// <summary>
		/// Gets the event type <c>Application.Terminating</c>
		/// </summary>
		/// <value>
		/// The event type <c>Application.Terminating</c>
		/// </value>
		/// <remarks>
		/// The application is being terminated by the OS. This event must be handled in a callback set with <see cref="SDL_AddEventWatch"/>().
		/// Called on iOS in <c>applicationWillTerminate()</c>.
		/// Called on Android in <c>onDestroy()</c>.
		/// </remarks>
		public static EventType Terminating { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Terminating); }

		/// <summary>
		/// Gets the event type <c>Application.LowMemory</c>
		/// </summary>
		/// <value>
		/// The event type <c>Application.LowMemory</c>
		/// </value>
		/// <remarks>
		/// The application is low on memory, free memory if possible. This event must be handled in a callback set with <see cref="SDL_AddEventWatch"/>().
		/// Called on iOS in <c>applicationDidReceiveMemoryWarning</c>().
		/// Called on Android in <c>onTrimMemory</c>().
		/// </remarks>
		public static EventType LowMemory { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.LowMemory); }

		/// <summary>
		/// Gets the event type <c>Application.WillEnterBackground</c>
		/// </summary>
		/// <value>
		/// The event type <c>Application.WillEnterBackground</c>
		/// </value>
		/// <remarks>
		/// The application is about to enter the background. This event must be handled in a callback set with <see cref="SDL_AddEventWatch"/>().
		/// Called on iOS in <c>applicationWillResignActive()</c>.
		/// Called on Android in <c>onPause()</c>.
		/// </remarks>
		public static EventType WillEnterBackground { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.WillEnterBackground); }

		/// <summary>
		/// Gets the event type <c>Application.DidEnterBackground</c>
		/// </summary>
		/// <value>
		/// The event type <c>Application.DidEnterBackground</c>
		/// </value>
		/// <remarks>
		/// The application did enter the background and may not get CPU for some time. This event must be handled in a callback set with <see cref="SDL_AddEventWatch"/>().
		/// Called on iOS in <c>applicationDidEnterBackground()</c>.
		/// Called on Android in <c>onPause()</c>.
		/// </remarks>
		public static EventType DidEnterBackground { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.DidEnterBackground); }

		/// <summary>
		/// Gets the event type <c>Application.WillEnterForeground</c>
		/// </summary>
		/// <value>
		/// The event type <c>Application.WillEnterForeground</c>
		/// </value>
		/// <remarks>
		/// The application is about to enter the foreground. This event must be handled in a callback set with <see cref="SDL_AddEventWatch"/>().
		/// Called on iOS in <c>applicationWillEnterForeground()</c>.
		/// Called on Android in <c>onResume()</c>.
		/// </remarks>
		public static EventType WillEnterForeground { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.WillEnterForeground); }

		/// <summary>
		/// Gets the event type <c>Application.DidEnterForeground</c>
		/// </summary>
		/// <value>
		/// The event type <c>Application.DidEnterForeground</c>
		/// </value>
		/// <remarks>
		/// The application is now interactive. This event must be handled in a callback set with <see cref="SDL_AddEventWatch"/>().
		/// Called on iOS in <c>applicationDidBecomeActive()</c>.
		/// Called on Android in <c>onResume()</c>.
		/// </remarks>
		public static EventType DidEnterForeground { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.DidEnterForeground); }

		/// <summary>
		/// Gets the event type <c>Application.LocaleChanged</c>
		/// </summary>
		/// <value>
		/// The event type <c>Application.LocaleChanged</c>
		/// </value>
		/// <remarks>
		/// The user's locale preferences have changed
		/// </remarks>
		public static EventType LocaleChanged { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.LocaleChanged); }

		/// <summary>
		/// Gets the event type <c>Application.SystemThemeChanged</c>
		/// </summary>
		/// <value>
		/// The event type <c>Application.SystemThemeChanged</c>
		/// </value>
		/// <remarks>
		/// The system theme changed
		/// </remarks>
		public static EventType SystemThemeChanged { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.SystemThemeChanged); }
	}

	/// <summary>
	/// Provides <see cref="EventType">event types</see> for display events
	/// </summary>
	public static class Display
	{
		/// <summary>
		/// Gets the event type <c>Display.OrientationChanged</c>
		/// </summary>
		/// <value>
		/// The event type <c>Display.OrientationChanged</c>
		/// </value>
		/// <remarks>
		/// The display orientation has changed to <see cref="data1"/>
		/// </remarks>
		public static EventType OrientationChanged { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.DisplayOrientation); }

		/// <summary>
		/// Gets the event type <c>Display.Added</c>
		/// </summary>
		/// <value>
		/// The event type <c>Display.Added</c>
		/// </value>
		/// <remarks>
		/// A display has been added to the system
		/// </remarks>
		public static EventType Added { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.DisplayAdded); }

		/// <summary>
		/// Gets the event type <c>Display.Removed</c>
		/// </summary>
		/// <value>
		/// The event type <c>Display.Removed</c>
		/// </value>
		/// <remarks>
		/// A display has been removed from the system
		/// </remarks>
		public static EventType Removed { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.DisplayRemoved); }

		/// <summary>
		/// Gets the event type <c>Display.Moved</c>
		/// </summary>
		/// <value>
		/// The event type <c>Display.Moved</c>
		/// </value>
		/// <remarks>
		/// A display has changed position
		/// </remarks>
		public static EventType Moved { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.DisplayMoved); }

		/// <summary>
		/// Gets the event type <c>Display.DesktopModeChanged</c>
		/// </summary>
		/// <value>
		/// The event type <c>Display.DesktopModeChanged</c>
		/// </value>
		/// <remarks>
		/// A display has changed desktop mode
		/// </remarks>
		public static EventType DesktopModeChanged { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.DisplayDesktopModeChanged); }

		/// <summary>
		/// Gets the event type <c>Display.CurrentModeChanged</c>
		/// </summary>
		/// <value>
		/// The event type <c>Display.CurrentModeChanged</c>
		/// </value>
		/// <remarks>
		/// A display has changed current mode
		/// </remarks>
		public static EventType CurrentModeChanged { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.DisplayCurrentModeChanged); }

		/// <summary>
		/// Gets the event type <c>Display.ContentScaleChanged</c>
		/// </summary>
		/// <value>
		/// The event type <c>Display.ContentScaleChanged</c>
		/// </value>
		/// <remarks>
		/// A display has changed content scale
		/// </remarks>
		public static EventType ContentScaleChanged { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.DisplayContentScaleChanged); }
	}

	/// <summary>
	/// Provides <see cref="EventType">event types</see> for window events
	/// </summary>
	public static class Window
	{
		/// <summary>
		/// Gets the event type <c>Window.Shown</c>
		/// </summary>
		/// <value>
		/// The event type <c>Window.Shown</c>
		/// </value>
		/// <remarks>
		/// A window has been shown
		/// </remarks>
		public static EventType Shown { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.WindowShown); }

		/// <summary>
		/// Gets the event type <c>Window.Hidden</c>
		/// </summary>
		/// <value>
		/// The event type <c>Window.Hidden</c>
		/// </value>
		/// <remarks>
		/// A window has been hidden
		/// </remarks>
		public static EventType Hidden { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.WindowHidden); }

		/// <summary>
		/// Gets the event type <c>Window.Exposed</c>
		/// </summary>
		/// <value>
		/// The event type <c>Window.Exposed</c>
		/// </value>
		/// <remarks>
		/// A window has been exposed and should be redrawn, and can be redrawn directly from event watchers for this event
		/// </remarks>
		public static EventType Exposed { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.WindowExposed); }

		/// <summary>
		/// Gets the event type <c>Window.Moved</c>
		/// </summary>
		/// <value>
		/// The event type <c>Window.Moved</c>
		/// </value>
		/// <remarks>
		/// A window has been moved to <see cref="data1"/>, <see cref="data2"/>
		/// </remarks>
		public static EventType Moved { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.WindowMoved); }

		/// <summary>
		/// Gets the event type <c>Window.Resized</c>
		/// </summary>
		/// <value>
		/// The event type <c>Window.Resized</c>
		/// </value>
		/// <remarks>
		/// A window has been resized to <see cref="data1"/>×<see cref="data2"/>
		/// </remarks>
		public static EventType Resized { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.WindowResized); }

		/// <summary>
		/// Gets the event type <c>Window.PixelSizeChanged</c>
		/// </summary>
		/// <value>
		/// The event type <c>Window.PixelSizeChanged</c>
		/// </value>
		/// <remarks>
		/// The pixel size of a window has changed to <see cref="data1"/>×<see cref="data2"/>
		/// </remarks>
		public static EventType PixelSizeChanged { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.WindowPixelSizeChanged); }

		/// <summary>
		/// Gets the event type <c>Window.MetalViewResized</c>
		/// </summary>
		/// <value>
		/// The event type <c>Window.MetalViewResized</c>
		/// </value>
		/// <remarks>
		/// The pixel size of a Metal view associated with a window has changed
		/// </remarks>
		public static EventType MetalViewResized { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.WindowMetalViewResized); }

		/// <summary>
		/// Gets the event type <c>Window.Minimized</c>
		/// </summary>
		/// <value>
		/// The event type <c>Window.Minimized</c>
		/// </value>
		/// <remarks>
		/// A window has been minimized
		/// </remarks>
		public static EventType Minimized { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.WindowMinimized); }

		/// <summary>
		/// Gets the event type <c>Window.Maximized</c>
		/// </summary>
		/// <value>
		/// The event type <c>Window.Maximized</c>
		/// </value>
		/// <remarks>
		/// A window has been maximized
		/// </remarks>
		public static EventType Maximized { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.WindowMaximized); }

		/// <summary>
		/// Gets the event type <c>Window.Restored</c>
		/// </summary>
		/// <value>
		/// The event type <c>Window.Restored</c>
		/// </value>
		/// <remarks>
		/// A window has been restored to normal size and position
		/// </remarks>
		public static EventType Restored { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.WindowRestored); }

		/// <summary>
		/// Gets the event type <c>Window.MouseEnter</c>
		/// </summary>
		/// <value>
		/// The event type <c>Window.MouseEnter</c>
		/// </value>
		/// <remarks>
		/// A window has gained mouse focus
		/// </remarks>
		public static EventType MouseEnter { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.WindowMouseEnter); }

		/// <summary>
		/// Gets the event type <c>Window.MouseLeave</c>
		/// </summary>
		/// <value>
		/// The event type <c>Window.MouseLeave</c>
		/// </value>
		/// <remarks>
		/// A window has lost mouse focus
		/// </remarks>
		public static EventType MouseLeave { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.WindowMouseLeave); }

		/// <summary>
		/// Gets the event type <c>Window.FocusGained</c>
		/// </summary>
		/// <value>
		/// The event type <c>Window.FocusGained</c>
		/// </value>
		/// <remarks>
		/// A window has gained keyboard focus
		/// </remarks>
		public static EventType FocusGained { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.WindowFocusGained); }

		/// <summary>
		/// Gets the event type <c>Window.FocusLost</c>
		/// </summary>
		/// <value>
		/// The event type <c>Window.FocusLost</c>
		/// </value>
		/// <remarks>
		/// A window has lost keyboard focus
		/// </remarks>
		public static EventType FocusLost { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.WindowFocusLost); }

		/// <summary>
		/// Gets the event type <c>Window.CloseRequested</c>
		/// </summary>
		/// <value>
		/// The event type <c>Window.CloseRequested</c>
		/// </value>
		/// <remarks>
		/// The window manager requests that a window should be closed
		/// </remarks>
		public static EventType CloseRequested { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.WindowCloseRequested); }

		/// <summary>
		/// Gets the event type <c>Window.HitTest</c>
		/// </summary>
		/// <value>
		/// The event type <c>Window.HitTest</c>
		/// </value>
		/// <remarks>
		/// A window had a hit test that wasn't <see cref="SDL_HITTEST_NORMAL"/>
		/// </remarks>
		public static EventType HitTest { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.WindowHitTest); }

		/// <summary>
		/// Gets the event type <c>Window.IccProfileChanged</c>
		/// </summary>
		/// <value>
		/// The event type <c>Window.IccProfileChanged</c>
		/// </value>
		/// <remarks>
		/// The ICC profile of a window's display has changed
		/// </remarks>
		public static EventType IccProfileChanged { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.WindowIccProfileChnaged); }

		/// <summary>
		/// Gets the event type <c>Window.DisplayChanged</c>
		/// </summary>
		/// <value>
		/// The event type <c>Window.DisplayChanged</c>
		/// </value>
		/// <remarks>
		/// A window has been moved to display <see cref="data1"/>
		/// </remarks>
		public static EventType DisplayChanged { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.WindowDisplayChanged); }

		/// <summary>
		/// Gets the event type <c>Window.DisplayScaleChanged</c>
		/// </summary>
		/// <value>
		/// The event type <c>Window.DisplayScaleChanged</c>
		/// </value>
		/// <remarks>
		/// A window's display scale has been changed
		/// </remarks>
		public static EventType DisplayScaleChanged { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.WindowDisplayScaleChanged); }

		/// <summary>
		/// Gets the event type <c>Window.SafeAreaChanged</c>
		/// </summary>
		/// <value>
		/// The event type <c>Window.SafeAreaChanged</c>
		/// </value>
		/// <remarks>
		/// A window's safe area has been changed
		/// </remarks>
		public static EventType SafeAreaChanged { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.WindowSafeAreaChanged); }

		/// <summary>
		/// Gets the event type <c>Window.Occluded</c>
		/// </summary>
		/// <value>
		/// The event type <c>Window.Occluded</c>
		/// </value>
		/// <remarks>
		/// A window has been occluded
		/// </remarks>
		public static EventType Occluded { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.WindowOccluded); }

		/// <summary>
		/// Gets the event type <c>Window.EnterFullscreen</c>
		/// </summary>
		/// <value>
		/// The event type <c>Window.EnterFullscreen</c>
		/// </value>
		/// <remarks>
		/// A window has entered fullscreen mode
		/// </remarks>
		public static EventType EnterFullscreen { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.WindowEnterFullscreen); }

		/// <summary>
		/// Gets the event type <c>Window.LeaveFullscreen</c>
		/// </summary>
		/// <value>
		/// The event type <c>Window.LeaveFullscreen</c>
		/// </value>
		/// <remarks>
		/// A window has left fullscreen mode
		/// </remarks>
		public static EventType LeaveFullscreen { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.WindowLeaveFullscreen); }

		/// <summary>
		/// Gets the event type <c>Window.Destroyed</c>
		/// </summary>
		/// <value>
		/// The event type <c>Window.Destroyed</c>
		/// </value>
		/// <remarks>
		/// The window with the associated ID is being or has been destroyed.
		/// If this message is being handled in an event watcher, the window handle is still valid and can still be used to retrieve any properties associated with the window.
		/// Otherwise, the handle has already been destroyed and all resources associated with it are invalid.
		/// </remarks>
		public static EventType Destroyed { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.WindowDestroyed); }

		/// <summary>
		/// Gets the event type <c>Window.HdrStateChanged</c>
		/// </summary>
		/// <value>
		/// The event type <c>Window.HdrStateChanged</c>
		/// </value>
		/// <remarks>
		/// A window's HDR properties have changed
		/// </remarks>
		public static EventType HdrStateChanged { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.WindowHdrStateChanged); }
	}

	/// <summary>
	/// Provides <see cref="EventType">event types</see> for keyboard events
	/// </summary>
	public static class Keyboard
	{
		/// <summary>
		/// Gets the event type <c>Keyboard.KeyDown</c>
		/// </summary>
		/// <value>
		/// The event type <c>Keyboard.KeyDown</c>
		/// </value>
		/// <remarks>
		/// Key pressed
		/// </remarks>
		public static EventType KeyDown { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.KeyDown); }

		/// <summary>
		/// Gets the event type <c>Keyboard.KeyUp</c>
		/// </summary>
		/// <value>
		/// The event type <c>Keyboard.KeyUp</c>
		/// </value>
		/// <remarks>
		/// Key released
		/// </remarks>
		public static EventType KeyUp { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.KeyUp); }

		/// <summary>
		/// Gets the event type <c>Keyboard.TextEditing</c>
		/// </summary>
		/// <value>
		/// The event type <c>Keyboard.TextEditing</c>
		/// </value>
		/// <remarks>
		/// Keyboard text editing (composition)
		/// </remarks>
		public static EventType TextEditing { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.TextEditing); }

		/// <summary>
		/// Gets the event type <c>Keyboard.TextInput</c>
		/// </summary>
		/// <value>
		/// The event type <c>Keyboard.TextInput</c>
		/// </value>
		/// <remarks>
		/// Keyboard text input
		/// </remarks>
		public static EventType TextInput { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.TextInput); }

		/// <summary>
		/// Gets the event type <c>Keyboard.KeymapChanged</c>
		/// </summary>
		/// <value>
		/// The event type <c>Keyboard.KeymapChanged</c>
		/// </value>
		/// <remarks>
		/// Keymap changed due to a system event such as an input language or keyboard layout change
		/// </remarks>
		public static EventType KeymapChanged { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.KeymapChanged); }

		/// <summary>
		/// Gets the event type <c>Keyboard.Added</c>
		/// </summary>
		/// <value>
		/// The event type <c>Keyboard.Added</c>
		/// </value>
		/// <remarks>
		/// A new keyboard has been inserted into the system
		/// </remarks>
		public static EventType Added { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.KeyboardAdded); }

		/// <summary>
		/// Gets the event type <c>Keyboard.Removed</c>
		/// </summary>
		/// <value>
		/// The event type <c>Keyboard.Removed</c>
		/// </value>
		/// <remarks>
		/// A keyboard has been removed
		/// </remarks>
		public static EventType Removed { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.KeyboardRemoved); }

		/// <summary>
		/// Gets the event type <c>Keyboard.TextEditingCandidates</c>
		/// </summary>
		/// <value>
		/// The event type <c>Keyboard.TextEditingCandidates</c>
		/// </value>
		/// <remarks>
		/// Keyboard text editing candidates
		/// </remarks>
		public static EventType TextEditingCandidates { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.TextEditingCandidates); }
	}

	/// <summary>
	/// Provides <see cref="EventType">event types</see> for mouse events
	/// </summary>
	public static class Mouse
	{
		/// <summary>
		/// Gets the event type <c>Mouse.Motion</c>
		/// </summary>
		/// <value>
		/// The event type <c>Mouse.Motion</c>
		/// </value>
		/// <remarks>
		/// Mouse moved
		/// </remarks>
		public static EventType Motion { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.MouseMotion); }

		/// <summary>
		/// Gets the event type <c>Mouse.ButtonDown</c>
		/// </summary>
		/// <value>
		/// The event type <c>Mouse.ButtonDown</c>
		/// </value>
		/// <remarks>
		/// Mouse button pressed
		/// </remarks>
		public static EventType ButtonDown { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.MouseButtonDown); }

		/// <summary>
		/// Gets the event type <c>Mouse.ButtonUp</c>
		/// </summary>
		/// <value>
		/// The event type <c>Mouse.ButtonUp</c>
		/// </value>
		/// <remarks>
		/// Mouse button released
		/// </remarks>
		public static EventType ButtonUp { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.MouseButtonUp); }

		/// <summary>
		/// Gets the event type <c>Mouse.WheelMotion</c>
		/// </summary>
		/// <value>
		/// The event type <c>Mouse.WheelMotion</c>
		/// </value>
		/// <remarks>
		/// Mouse wheel motion
		/// </remarks>
		public static EventType WheelMotion { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.MouseWheel); }

		/// <summary>
		/// Gets the event type <c>Mouse.Added</c>
		/// </summary>
		/// <value>
		/// The event type <c>Mouse.Added</c>
		/// </value>
		/// <remarks>
		/// A new mouse has been inserted into the system
		/// </remarks>
		public static EventType Added { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.MouseAdded); }

		/// <summary>
		/// Gets the event type <c>Mouse.Removed</c>
		/// </summary>
		/// <value>
		/// The event type <c>Mouse.Removed</c>
		/// </value>
		/// <remarks>
		/// A mouse has been removed
		/// </remarks>
		public static EventType Removed { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.MouseRemoved); }
	}

	/// <summary>
	/// Provides <see cref="EventType">event types</see> for joystick events
	/// </summary>
	public static class Joystick
	{
		/// <summary>
		/// Gets the event type <c>Joystick.AxisMotion</c>
		/// </summary>
		/// <value>
		/// The event type <c>Joystick.AxisMotion</c>
		/// </value>
		/// <remarks>
		/// Joystick axis motion
		/// </remarks>
		public static EventType AxisMotion { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.JoystickAxisMotion); }

		/// <summary>
		/// Gets the event type <c>Joystick.BallMotion</c>
		/// </summary>
		/// <value>
		/// The event type <c>Joystick.BallMotion</c>
		/// </value>
		/// <remarks>
		/// Joystick trackball motion
		/// </remarks>
		public static EventType BallMotion { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.JoystickBallMotion); }

		/// <summary>
		/// Gets the event type <c>Joystick.HatMotion</c>
		/// </summary>
		/// <value>
		/// The event type <c>Joystick.HatMotion</c>
		/// </value>
		/// <remarks>
		/// Joystick hat position change
		/// </remarks>
		public static EventType HatMotion { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.JoystickHatMotion); }

		/// <summary>
		/// Gets the event type <c>Joystick.ButtonDown</c>
		/// </summary>
		/// <value>
		/// The event type <c>Joystick.ButtonDown</c>
		/// </value>
		/// <remarks>
		/// Joystick button pressed
		/// </remarks>
		public static EventType ButtonDown { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.JoystickButtonDown); }

		/// <summary>
		/// Gets the event type <c>Joystick.ButtonUp</c>
		/// </summary>
		/// <value>
		/// The event type <c>Joystick.ButtonUp</c>
		/// </value>
		/// <remarks>
		/// Joystick button released
		/// </remarks>
		public static EventType ButtonUp { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.JoystickButtonUp); }

		/// <summary>
		/// Gets the event type <c>Joystick.Added</c>
		/// </summary>
		/// <value>
		/// The event type <c>Joystick.Added</c>
		/// </value>
		/// <remarks>
		/// A new joystick has been inserted into the system
		/// </remarks>
		public static EventType Added { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.JoystickAdded); }

		/// <summary>
		/// Gets the event type <c>Joystick.Removed</c>
		/// </summary>
		/// <value>
		/// The event type <c>Joystick.Removed</c>
		/// </value>
		/// <remarks>
		/// An opened joystick has been removed
		/// </remarks>
		public static EventType Removed { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.JoystickRemoved); }

		/// <summary>
		/// Gets the event type <c>Joystick.BatteryUpdated</c>
		/// </summary>
		/// <value>
		/// The event type <c>Joystick.BatteryUpdated</c>
		/// </value>
		/// <remarks>
		/// Joystick battery level change
		/// </remarks>
		public static EventType BatteryUpdated { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.JoystickBatteryUpdated); }

		/// <summary>
		/// Gets the event type <c>Joystick.UpdateCompleted</c>
		/// </summary>
		/// <value>
		/// The event type <c>Joystick.UpdateCompleted</c>
		/// </value>
		/// <remarks>
		/// Joystick update is complete
		/// </remarks>
		public static EventType UpdateCompleted { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.JoystickUpdateCompleted); }
	}

	/// <summary>
	/// Provides <see cref="EventType">event types</see> for gamepad events
	/// </summary>
	public static class Gamepad
	{
		/// <summary>
		/// Gets the event type <c>Gamepad.AxisMotion</c>
		/// </summary>
		/// <value>
		/// The event type <c>Gamepad.AxisMotion</c>
		/// </value>
		/// <remarks>
		/// Gamepad axis motion
		/// </remarks>
		public static EventType AxisMotion { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.GamepadAxisMotion); }

		/// <summary>
		/// Gets the event type <c>Gamepad.ButtonDown</c>
		/// </summary>
		/// <value>
		/// The event type <c>Gamepad.ButtonDown</c>
		/// </value>
		/// <remarks>
		/// Gamepad button pressed
		/// </remarks>
		public static EventType ButtonDown { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.GamepadButtonDown); }

		/// <summary>
		/// Gets the event type <c>Gamepad.ButtonUp</c>
		/// </summary>
		/// <value>
		/// The event type <c>Gamepad.ButtonUp</c>
		/// </value>
		/// <remarks>
		/// Gamepad button released
		/// </remarks>
		public static EventType ButtonUp { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.GamepadButtonUp); }

		/// <summary>
		/// Gets the event type <c>Gamepad.Added</c>
		/// </summary>
		/// <value>
		/// The event type <c>Gamepad.Added</c>
		/// </value>
		/// <remarks>
		/// A new gamepad has been inserted into the system
		/// </remarks>
		public static EventType Added { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.GamepadAdded); }

		/// <summary>
		/// Gets the event type <c>Gamepad.Removed</c>
		/// </summary>
		/// <value>
		/// The event type <c>Gamepad.Removed</c>
		/// </value>
		/// <remarks>
		/// A gamepad has been removed
		/// </remarks>
		public static EventType Removed { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.GamepadRemoved); }

		/// <summary>
		/// Gets the event type <c>Gamepad.Remapped</c>
		/// </summary>
		/// <value>
		/// The event type <c>Gamepad.Remapped</c>
		/// </value>
		/// <remarks>
		/// Gamepad mapping was updated
		/// </remarks>
		public static EventType Remapped { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.GamepadRemapped); }

		/// <summary>
		/// Gets the event type <c>Gamepad.TouchpadDown</c>
		/// </summary>
		/// <value>
		/// The event type <c>Gamepad.TouchpadDown</c>
		/// </value>
		/// <remarks>
		/// Gamepad touchpad was touched
		/// </remarks>
		public static EventType TouchpadDown { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.GamepadTouchpadDown); }

		/// <summary>
		/// Gets the event type <c>Gamepad.TouchpadMotion</c>
		/// </summary>
		/// <value>
		/// The event type <c>Gamepad.TouchpadMotion</c>
		/// </value>
		/// <remarks>
		/// Gamepad touchpad finger was moved
		/// </remarks>
		public static EventType TouchpadMotion { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.GamepadTouchpadMotion); }

		/// <summary>
		/// Gets the event type <c>Gamepad.TouchpadUp</c>
		/// </summary>
		/// <value>
		/// The event type <c>Gamepad.TouchpadUp</c>
		/// </value>
		/// <remarks>
		/// Gamepad touchpad finger was lifted
		/// </remarks>
		public static EventType TouchpadUp { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.GamepadTouchpadUp); }

		/// <summary>
		/// Gets the event type <c>Gamepad.SensorUpdated</c>
		/// </summary>
		/// <value>
		/// The event type <c>Gamepad.SensorUpdated</c>
		/// </value>
		/// <remarks>
		/// Gamepad sensor was updated
		/// </remarks>
		public static EventType SensorUpdated { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.GamepadSensorUpdated); }

		/// <summary>
		/// Gets the event type <c>Gamepad.UpdateCompleted</c>
		/// </summary>
		/// <value>
		/// The event type <c>Gamepad.UpdateCompleted</c>
		/// </value>
		/// <remarks>
		/// Gamepad update is complete
		/// </remarks>
		public static EventType UpdateCompleted { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.GamepadUpdateCompleted); }

		/// <summary>
		/// Gets the event type <c>Gamepad.SteamHandleUpdated</c>
		/// </summary>
		/// <value>
		/// The event type <c>Gamepad.SteamHandleUpdated</c>
		/// </value>
		/// <remarks>
		/// Gamepad Steam handle has changed
		/// </remarks>
		public static EventType SteamHandleUpdated { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.GamepadSteamHandleUpdated); }
	}

	/// <summary>
	/// Provides <see cref="EventType">event types</see> for touch events
	/// </summary>
	public static class Touch
	{
		/// <summary>
		/// Gets the event type <c>Touch.FingerDown</c>
		/// </summary>
		/// <value>
		/// The event type <c>Touch.FingerDown</c>
		/// </value>
		public static EventType FingerDown { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.FingerDown); }

		/// <summary>
		/// Gets the event type <c>Touch.FingerUp</c>
		/// </summary>
		/// <value>
		/// The event type <c>Touch.FingerUp</c>
		/// </value>
		public static EventType FingerUp { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.FingerUp); }

		/// <summary>
		/// Gets the event type <c>Touch.FingerMotion</c>
		/// </summary>
		/// <value>
		/// The event type <c>Touch.FingerMotion</c>
		/// </value>
		public static EventType FingerMotion { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.FingerMotion); }

		/// <summary>
		/// Gets the event type <c>Touch.FingerCanceled</c>
		/// </summary>
		/// <value>
		/// The event type <c>Touch.FingerCanceled</c>
		/// </value>
		public static EventType FingerCanceled { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.FingerCanceled); }
	}

	/// <summary>
	/// Provides <see cref="EventType">event types</see> for clipboard events
	/// </summary>
	public static class Clipboard
	{
		/// <summary>
		/// Gets the event type <c>Clipboard.Updated</c>
		/// </summary>
		/// <value>
		/// The event type <c>Clipboard.Updated</c>
		/// </value>
		/// <remarks>
		/// The clipboard or primary selection changed
		/// </remarks>
		public static EventType Updated { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.ClipboardUpdated); }
	}

	/// <summary>
	/// Provides <see cref="EventType">event types</see> for drag and drop events
	/// </summary>
	public static class DragAndDrop
	{
		/// <summary>
		/// Gets the event type <c>DragAndDrop.File</c>
		/// </summary>
		/// <value>
		/// The event type <c>DragAndDrop.File</c>
		/// </value>
		/// <remarks>
		/// The system requests a file open
		/// </remarks>
		public static EventType File { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.DropFile); }

		/// <summary>
		/// Gets the event type <c>DragAndDrop.Text</c>
		/// </summary>
		/// <value>
		/// The event type <c>DragAndDrop.Text</c>
		/// </value>
		/// <remarks>
		/// text/plain drag-and-drop event
		/// </remarks>
		public static EventType Text { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.DropText); }

		/// <summary>
		/// Gets the event type <c>DragAndDrop.Begin</c>
		/// </summary>
		/// <value>
		/// The event type <c>DragAndDrop.Begin</c>
		/// </value>
		/// <remarks>
		/// A new set of drops is beginning (<see langword="null"/> <see cref="filename"/>)
		/// </remarks>
		public static EventType Begin { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.DropBegin); }

		/// <summary>
		/// Gets the event type <c>DragAndDrop.Completed</c>
		/// </summary>
		/// <value>
		/// The event type <c>DragAndDrop.Completed</c>
		/// </value>
		/// <remarks>
		/// Current set of drops is now complete (<see langword="null"/> <see cref="filename"/>)
		/// </remarks>
		public static EventType Completed { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.DropCompleted); }

		/// <summary>
		/// Gets the event type <c>DragAndDrop.Position</c>
		/// </summary>
		/// <value>
		/// The event type <c>DragAndDrop.Position</c>
		/// </value>
		/// <remarks>
		/// Position while moving over the window
		/// </remarks>
		public static EventType Position { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.DropPosition); }
	}

	/// <summary>
	/// Provides <see cref="EventType">event types</see> for audio hotplug events
	/// </summary>
	public static class AudioDevice
	{
		/// <summary>
		/// Gets the event type <c>AudioDevice.Added</c>
		/// </summary>
		/// <value>
		/// The event type <c>AudioDevice.Added</c>
		/// </value>
		/// <remarks>
		/// A new audio device is available
		/// </remarks>
		public static EventType Added { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.AudioDeviceAdded); }

		/// <summary>
		/// Gets the event type <c>AudioDevice.Removed</c>
		/// </summary>
		/// <value>
		/// The event type <c>AudioDevice.Removed</c>
		/// </value>
		/// <remarks>
		/// An audio device has been removed
		/// </remarks>
		public static EventType Removed { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.AudioDeviceRemoved); }

		/// <summary>
		/// Gets the event type <c>AudioDevice.FormatChanged</c>
		/// </summary>
		/// <value>
		/// The event type <c>AudioDevice.FormatChanged</c>
		/// </value>
		/// <remarks>
		/// An audio device's format has been changed by the system
		/// </remarks>
		public static EventType FormatChanged { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.AudioDeviceFormatChanged); }
	}

	/// <summary>
	/// Provides <see cref="EventType">event types</see> for sensor events
	/// </summary>
	public static class Sensor
	{
		/// <summary>
		/// Gets the event type <c>Sensor.Updated</c>
		/// </summary>
		/// <value>
		/// The event type <c>Sensor.Updated</c>
		/// </value>
		/// <remarks>
		/// A sensor was updated
		/// </remarks>
		public static EventType Updated { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.SensorUpdated); }
	}

	/// <summary>
	/// Provides <see cref="EventType">event types</see> for pressure-sensitive pen events
	/// </summary>
	public static class Pen
	{
		/// <summary>
		/// Gets the event type <c>Pen.ProximityIn</c>
		/// </summary>
		/// <value>
		/// The event type <c>Pen.ProximityIn</c>
		/// </value>
		/// <remarks>
		/// Pressure-sensitive pen has become available
		/// </remarks>
		public static EventType ProximityIn { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.PenProximityIn); }

		/// <summary>
		/// Gets the event type <c>Pen.ProximityOut</c>
		/// </summary>
		/// <value>
		/// The event type <c>Pen.ProximityOut</c>
		/// </value>
		/// <remarks>
		/// Pressure-sensitive pen has become unavailable
		/// </remarks>
		public static EventType ProximityOut { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.PenProximityOut); }

		/// <summary>
		/// Gets the event type <c>Pen.Down</c>
		/// </summary>
		/// <value>
		/// The event type <c>Pen.Down</c>
		/// </value>
		/// <remarks>
		/// Pressure-sensitive pen touched drawing surface
		/// </remarks>
		public static EventType Down { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.PenDown); }

		/// <summary>
		/// Gets the event type <c>Pen.Up</c>
		/// </summary>
		/// <value>
		/// The event type <c>Pen.Up</c>
		/// </value>
		/// <remarks>
		/// Pressure-sensitive pen stopped touching drawing surface
		/// </remarks>
		public static EventType Up { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.PenUp); }

		/// <summary>
		/// Gets the event type <c>Pen.ButtonDown</c>
		/// </summary>
		/// <value>
		/// The event type <c>Pen.ButtonDown</c>
		/// </value>
		/// <remarks>
		/// Pressure-sensitive pen button pressed
		/// </remarks>
		public static EventType ButtonDown { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.PenButtonDown); }

		/// <summary>
		/// Gets the event type <c>Pen.ButtonUp</c>
		/// </summary>
		/// <value>
		/// The event type <c>Pen.ButtonUp</c>
		/// </value>
		/// <remarks>
		/// Pressure-sensitive pen button released
		/// </remarks>
		public static EventType ButtonUp { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.PenButtonUp); }

		/// <summary>
		/// Gets the event type <c>Pen.Motion</c>
		/// </summary>
		/// <value>
		/// The event type <c>Pen.Motion</c>
		/// </value>
		/// <remarks>
		/// Pressure-sensitive pen is moving on the tablet
		/// </remarks>
		public static EventType Motion { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.PenMotion); }

		/// <summary>
		/// Gets the event type <c>Pen.AxisChanged</c>
		/// </summary>
		/// <value>
		/// The event type <c>Pen.AxisChanged</c>
		/// </value>
		/// <remarks>
		/// Pressure-sensitive pen angle/pressure/etc changed
		/// </remarks>
		public static EventType AxisChanged { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.PenAxis); }
	}

	/// <summary>
	/// Provides <see cref="EventType">event types</see> for camera hotplug events
	/// </summary>
	public static class CameraDevice
	{
		/// <summary>
		/// Gets the event type <c>CameraDevice.Added</c>
		/// </summary>
		/// <value>
		/// The event type <c>CameraDevice.Added</c>
		/// </value>
		/// <remarks>
		/// A new camera device is available
		/// </remarks>
		public static EventType Added { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.CameraDeviceAdded); }

		/// <summary>
		/// Gets the event type <c>CameraDevice.Removed</c>
		/// </summary>
		/// <value>
		/// The event type <c>CameraDevice.Removed</c>
		/// </value>
		/// <remarks>
		/// A camera device has been removed
		/// </remarks>
		public static EventType Removed { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.CameraDeviceRemoved); }

		/// <summary>
		/// Gets the event type <c>CameraDevice.Approved</c>
		/// </summary>
		/// <value>
		/// The event type <c>CameraDevice.Approved</c>
		/// </value>
		/// <remarks>
		/// A camera device has been approved for use by the user
		/// </remarks>
		public static EventType Approved { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.CameraDeviceApproved); }

		/// <summary>
		/// Gets the event type <c>CameraDevice.Denied</c>
		/// </summary>
		/// <value>
		/// The event type <c>CameraDevice.Denied</c>
		/// </value>
		/// <remarks>
		/// A camera device has been denied for use by the user
		/// </remarks>
		public static EventType Denied { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.CameraDeviceDenied); }
	}

	/// <summary>
	/// Provides <see cref="EventType">event types</see> for render events
	/// </summary>
	public static class Render
	{
		/// <summary>
		/// Gets the event type <c>Render.TargetsReset</c>
		/// </summary>
		/// <value>
		/// The event type <c>Render.TargetsReset</c>
		/// </value>
		/// <remarks>
		/// The render targets have been reset and their contents need to be updated
		/// </remarks>
		public static EventType TargetsReset { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.RenderTargetsReset); }

		/// <summary>
		/// Gets the event type <c>Render.DeviceReset</c>
		/// </summary>
		/// <value>
		/// The event type <c>Render.DeviceReset</c>
		/// </value>
		/// <remarks>
		/// The device has been reset and all textures need to be recreated
		/// </remarks>
		public static EventType DeviceReset { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.RenderDeviceReset); }

		/// <summary>
		/// Gets the event type <c>Render.DeviceLost</c>
		/// </summary>
		/// <value>
		/// The event type <c>Render.DeviceLost</c>
		/// </value>
		/// <remarks>
		/// The device has been lost and can't be recovered
		/// </remarks>
		public static EventType DeviceLost { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.RenderDeviceLost); }
	}

	/// <summary>
	/// Gets a user event type
	/// </summary>
	/// <param name="userValue">The user value of the resulting event type; used to identify the event type</param>
	/// <returns>A user event type identified by <c><paramref name="userValue"/></c></returns>
	/// <exception cref="ArgumentOutOfRangeException"><c><paramref name="userValue"/></c> is less than <c>0</c> or greater than <c>32767</c></exception>
	/// <remarks>
	/// User events should be allocated with <see cref="SDL_RegisterEvents"/>()
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static EventType User(int userValue)
	{
		if (userValue is < 0 or > (int)(Kind.Last - Kind.User))
		{
			failUserValueArgumentOutOfRange();
		}

		return new(unchecked(Kind.User + (uint)userValue));

		[DoesNotReturn]
		static void failUserValueArgumentOutOfRange() => throw new ArgumentOutOfRangeException(nameof(userValue));
	}
}
