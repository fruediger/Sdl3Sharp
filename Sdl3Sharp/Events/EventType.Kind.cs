using System;

namespace Sdl3Sharp.Events;

partial struct EventType
{
	internal enum Kind : uint
	{
		#region Application events

		/// <summary>SDL_EVENT_QUIT</summary>
		Quit = 0x100,

		/// <summary>SDL_EVENT_TERMINATING</summary>
		Terminating,

		/// <summary>SDL_EVENT_LOW_MEMORY</summary>
		LowMemory,

		/// <summary>SDL_EVENT_WILL_ENTER_BACKGROUND</summary>
		WillEnterBackground,

		/// <summary>SDL_EVENT_DID_ENTER_BACKGROUND</summary>
		DidEnterBackground,

		/// <summary>SDL_EVENT_WILL_ENTER_FOREGROUND</summary>
		WillEnterForeground,

		/// <summary>SDL_EVENT_DID_ENTER_FOREGROUND</summary>
		DidEnterForeground,

		/// <summary>SDL_EVENT_LOCALE_CHANGED</summary>
		LocaleChanged,

		/// <summary>SDL_EVENT_SYSTEM_THEME_CHANGED</summary>
		SystemThemeChanged,

		#endregion

		#region Display events

		/// <summary>SDL_EVENT_DISPLAY_ORIENTATION</summary>
		DisplayOrientation = 0x151,

		/// <summary>SDL_EVENT_DISPLAY_ADDED</summary>
		DisplayAdded,

		/// <summary>SDL_EVENT_DISPLAY_REMOVED</summary>
		DisplayRemoved,

		/// <summary>SDL_EVENT_DISPLAY_MOVED</summary>
		DisplayMoved,

		/// <summary>SDL_EVENT_DISPLAY_DESKTOP_MODE_CHANGED</summary>
		DisplayDesktopModeChanged,

		/// <summary>SDL_EVENT_DISPLAY_CURRENT_MODE_CHANGED</summary>
		DisplayCurrentModeChanged,

		/// <summary>SDL_EVENT_DISPLAY_CONTENT_SCALE_CHANGED</summary>
		DisplayContentScaleChanged,

		#endregion

		#region Window events

		/// <summary>SDL_EVENT_WINDOW_SHOWN</summary>
		WindowShown = 0x202,

		/// <summary>SDL_EVENT_WINDOW_HIDDEN</summary>
		WindowHidden,

		/// <summary>SDL_EVENT_WINDOW_EXPOSED</summary>
		WindowExposed,

		/// <summary>SDL_EVENT_WINDOW_MOVED</summary>
		WindowMoved,

		/// <summary>SDL_EVENT_WINDOW_RESIZED</summary>
		WindowResized,

		/// <summary>SDL_EVENT_WINDOW_PIXEL_SIZE_CHANGED</summary>
		WindowPixelSizeChanged,

		/// <summary>SDL_EVENT_WINDOW_METAL_VIEW_RESIZED</summary>
		WindowMetalViewResized,

		/// <summary>SDL_EVENT_WINDOW_MINIMIZED</summary>
		WindowMinimized,

		/// <summary>SDL_EVENT_WINDOW_MAXIMIZED</summary>
		WindowMaximized,

		/// <summary>SDL_EVENT_WINDOW_RESTORED</summary>
		WindowRestored,

		/// <summary>SDL_EVENT_WINDOW_MOUSE_ENTER</summary>
		WindowMouseEnter,

		/// <summary>SDL_EVENT_WINDOW_MOUSE_LEAVE</summary>
		WindowMouseLeave,

		/// <summary>SDL_EVENT_WINDOW_FOCUS_GAINED</summary>
		WindowFocusGained,

		/// <summary>SDL_EVENT_WINDOW_FOCUS_LOST</summary>
		WindowFocusLost,

		/// <summary>SDL_EVENT_WINDOW_CLOSE_REQUESTED</summary>
		WindowCloseRequested,

		/// <summary>SDL_EVENT_WINDOW_HIT_TEST</summary>
		WindowHitTest,

		/// <summary>SDL_EVENT_WINDOW_ICCPROF_CHANGED</summary>
		WindowIccProfileChnaged,

		/// <summary>SDL_EVENT_WINDOW_DISPLAY_CHANGED</summary>
		WindowDisplayChanged,

		/// <summary>SDL_EVENT_WINDOW_DISPLAY_SCALE_CHANGED</summary>
		WindowDisplayScaleChanged,

		/// <summary>SDL_EVENT_WINDOW_SAFE_AREA_CHANGED</summary>
		WindowSafeAreaChanged,

		/// <summary>SDL_EVENT_WINDOW_OCCLUDED</summary>
		WindowOccluded,

		/// <summary>SDL_EVENT_WINDOW_ENTER_FULLSCREEN</summary>
		WindowEnterFullscreen,

		/// <summary>SDL_EVENT_WINDOW_LEAVE_FULLSCREEN</summary>
		WindowLeaveFullscreen,

		/// <summary>SDL_EVENT_WINDOW_DESTROYED</summary>
		WindowDestroyed,

		/// <summary>SDL_EVENT_WINDOW_HDR_STATE_CHANGED</summary>
		WindowHdrStateChanged,

		#endregion

		#region Keyboard events

		/// <summary>SDL_EVENT_KEY_DOWN</summary>
		KeyDown = 0x300,

		/// <summary>SDL_EVENT_KEY_UP</summary>
		KeyUp,

		/// <summary>SDL_EVENT_TEXT_EDITING</summary>
		TextEditing,

		/// <summary>SDL_EVENT_TEXT_INPUT</summary>
		TextInput,

		/// <summary>SDL_EVENT_KEYMAP_CHANGED</summary>
		KeymapChanged,

		/// <summary>SDL_EVENT_KEYBOARD_ADDED</summary>
		KeyboardAdded,

		/// <summary>SDL_EVENT_KEYBOARD_REMOVED</summary>
		KeyboardRemoved,

		/// <summary>SDL_EVENT_TEXT_EDITING_CANDIDATES</summary>
		TextEditingCandidates,

		#endregion

		#region Mouse events

		/// <summary>SDL_EVENT_MOUSE_MOTION</summary>
		MouseMotion = 0x400,

		/// <summary>SDL_EVENT_MOUSE_BUTTON_DOWN</summary>
		MouseButtonDown,

		/// <summary>SDL_EVENT_MOUSE_BUTTON_UP</summary>
		MouseButtonUp,

		/// <summary>SDL_EVENT_MOUSE_WHEEL</summary>
		MouseWheel,

		/// <summary>SDL_EVENT_MOUSE_ADDED</summary>
		MouseAdded,

		/// <summary>SDL_EVENT_MOUSE_REMOVED</summary>
		MouseRemoved,

		#endregion

		#region Joystick events

		/// <summary>SDL_EVENT_JOYSTICK_AXIS_MOTION</summary>
		JoystickAxisMotion = 0x600,

		/// <summary>SDL_EVENT_JOYSTICK_BALL_MOTION</summary>
		JoystickBallMotion,

		/// <summary>SDL_EVENT_JOYSTICK_HAT_MOTION</summary>
		JoystickHatMotion,

		/// <summary>SDL_EVENT_JOYSTICK_BUTTON_DOWN</summary>
		JoystickButtonDown,

		/// <summary>SDL_EVENT_JOYSTICK_BUTTON_UP</summary>
		JoystickButtonUp,

		/// <summary>SDL_EVENT_JOYSTICK_ADDED</summary>
		JoystickAdded,

		/// <summary>SDL_EVENT_JOYSTICK_REMOVED</summary>
		JoystickRemoved,

		/// <summary>SDL_EVENT_JOYSTICK_BATTERY_UPDATED</summary>
		JoystickBatteryUpdated,

		/// <summary>SDL_EVENT_JOYSTICK_UPDATE_COMPLETE</summary>
		JoystickUpdateCompleted,

		#endregion

		#region Gamepad events

		/// <summary>SDL_EVENT_GAMEPAD_AXIS_MOTION</summary>
		GamepadAxisMotion = 0x650,

		/// <summary>SDL_EVENT_GAMEPAD_BUTTON_DOWN</summary>
		GamepadButtonDown,

		/// <summary>SDL_EVENT_GAMEPAD_BUTTON_UP</summary>
		GamepadButtonUp,

		/// <summary>SDL_EVENT_GAMEPAD_ADDED</summary>
		GamepadAdded,

		/// <summary>SDL_EVENT_GAMEPAD_REMOVED</summary>
		GamepadRemoved,

		/// <summary>SDL_EVENT_GAMEPAD_REMAPPED</summary>
		GamepadRemapped,

		/// <summary>SDL_EVENT_GAMEPAD_TOUCHPAD_DOWN</summary>
		GamepadTouchpadDown,

		/// <summary>SDL_EVENT_GAMEPAD_TOUCHPAD_MOTION</summary>
		GamepadTouchpadMotion,

		/// <summary>SDL_EVENT_GAMEPAD_TOUCHPAD_UP</summary>
		GamepadTouchpadUp,

		/// <summary>SDL_EVENT_GAMEPAD_SENSOR_UPDATE</summary>
		GamepadSensorUpdated,

		/// <summary>SDL_EVENT_GAMEPAD_UPDATE_COMPLETE</summary>
		GamepadUpdateCompleted,

		/// <summary>SDL_EVENT_GAMEPAD_STEAM_HANDLE_UPDATED</summary>
		GamepadSteamHandleUpdated,

		#endregion

		#region Touch events

		/// <summary>SDL_EVENT_FINGER_DOWN</summary>
		FingerDown = 0x700,

		/// <summary>SDL_EVENT_FINGER_UP</summary>
		FingerUp,

		/// <summary>SDL_EVENT_FINGER_MOTION</summary>
		FingerMotion,

		/// <summary>SDL_EVENT_FINGER_CANCELED</summary>
		FingerCanceled,

		#endregion

		#region Clipboard events

		/// <summary>SDL_EVENT_CLIPBOARD_UPDATE</summary>
		ClipboardUpdated = 0x900,

		#endregion

		#region Drag and drop events

		/// <summary>SDL_EVENT_DROP_FILE</summary>
		DropFile = 0x1000,

		/// <summary>SDL_EVENT_DROP_TEXT</summary>
		DropText,

		/// <summary>SDL_EVENT_DROP_BEGIN</summary>
		DropBegin,

		/// <summary>SDL_EVENT_DROP_COMPLETE</summary>
		DropCompleted,

		/// <summary>SDL_EVENT_DROP_POSITION</summary>
		DropPosition,

		#endregion

		#region Audio hotplug events

		/// <summary>SDL_EVENT_AUDIO_DEVICE_ADDED</summary>
		AudioDeviceAdded = 0x1100,

		/// <summary>SDL_EVENT_AUDIO_DEVICE_REMOVED</summary>
		AudioDeviceRemoved,

		/// <summary>SDL_EVENT_AUDIO_DEVICE_FORMAT_CHANGED</summary>
		AudioDeviceFormatChanged,

		#endregion

		#region Sensor events

		/// <summary>SDL_EVENT_SENSOR_UPDATE</summary>
		SensorUpdated = 0x1200,

		#endregion

		#region Pressure-sensitive pen events

		/// <summary>SDL_EVENT_PEN_PROXIMITY_IN</summary>
		PenProximityIn = 0x1300,

		/// <summary>SDL_EVENT_PEN_PROXIMITY_OUT</summary>
		PenProximityOut,

		/// <summary>SDL_EVENT_PEN_DOWN</summary>
		PenDown,

		/// <summary>SDL_EVENT_PEN_UP</summary>
		PenUp,

		/// <summary>SDL_EVENT_PEN_BUTTON_DOWN</summary>
		PenButtonDown,

		/// <summary>SDL_EVENT_PEN_BUTTON_UP</summary>
		PenButtonUp,

		/// <summary>SDL_EVENT_PEN_MOTION</summary>
		PenMotion,

		/// <summary>SDL_EVENT_PEN_AXIS</summary>
		PenAxis,

		#endregion

		#region Camera hotplug events

		/// <summary>SDL_EVENT_CAMERA_DEVICE_ADDED</summary>
		CameraDeviceAdded = 0x1400,

		/// <summary>SDL_EVENT_CAMERA_DEVICE_REMOVED</summary>
		CameraDeviceRemoved,

		/// <summary>SDL_EVENT_CAMERA_DEVICE_APPROVED</summary>
		CameraDeviceApproved,

		/// <summary>SDL_EVENT_CAMERA_DEVICE_DENIED</summary>
		CameraDeviceDenied,

		#endregion

		#region Render events

		/// <summary>SDL_EVENT_RENDER_TARGETS_RESET</summary>
		RenderTargetsReset = 0x2000,

		/// <summary>SDL_EVENT_RENDER_DEVICE_RESET</summary>
		RenderDeviceReset,

		/// <summary>SDL_EVENT_RENDER_DEVICE_LOST</summary>
		RenderDeviceLost,

		#endregion

		#region Reserved events for private platforms

		/// <summary>SDL_EVENT_PRIVATE0</summary>
		/// <remarks>Reserved event for private platforms. Do not use.</remarks>
		[Obsolete("Reserved event for private platforms")]
		Private0 = 0x4000,

		/// <summary>SDL_EVENT_PRIVATE1</summary>
		/// <remarks>Reserved event for private platforms. Do not use.</remarks>
		[Obsolete("Reserved event for private platforms")]
		Private1,

		/// <summary>SDL_EVENT_PRIVATE2</summary>
		/// <remarks>Reserved event for private platforms. Do not use.</remarks>
		[Obsolete("Reserved event for private platforms")]
		Private2,

		/// <summary>SDL_EVENT_PRIVATE3</summary>
		/// <remarks>Reserved event for private platforms. Do not use.</remarks>
		[Obsolete("Reserved event for private platforms")]
		Private3,

		#endregion

		#region Internal events

		/// <summary>SDL_EVENT_POLL_SENTINEL</summary>
		PollSentinel = 0x7F00,

		/// <summary>SDL_EVENT_USER</summary>
		User = 0x8000,

		/// <summary>SDL_EVENT_LAST</summary>
		Last = 0xFFFF,

		#endregion
	}

	private static string? KnownKindToString(Kind kind) => kind switch
	{
		Kind.Quit => $"{nameof(Application)}.{nameof(Application.Quit)}",
		Kind.Terminating => $"{nameof(Application)}.{nameof(Application.Terminating)}",
		Kind.LowMemory => $"{nameof(Application)}.{nameof(Application.LowMemory)}",
		Kind.WillEnterBackground => $"{nameof(Application)}.{nameof(Application.WillEnterBackground)}",
		Kind.DidEnterBackground => $"{nameof(Application)}.{nameof(Application.DidEnterBackground)}",
		Kind.WillEnterForeground => $"{nameof(Application)}.{nameof(Application.WillEnterForeground)}",
		Kind.DidEnterForeground => $"{nameof(Application)}.{nameof(Application.DidEnterForeground)}",
		Kind.LocaleChanged => $"{nameof(Application)}.{nameof(Application.LocaleChanged)}",
		Kind.SystemThemeChanged => $"{nameof(Application)}.{nameof(Application.SystemThemeChanged)}",

		Kind.DisplayOrientation => $"{nameof(Display)}.{nameof(Display.OrientationChanged)}",
		Kind.DisplayAdded => $"{nameof(Display)}.{nameof(Display.Added)}",
		Kind.DisplayRemoved => $"{nameof(Display)}.{nameof(Display.Removed)}",
		Kind.DisplayMoved => $"{nameof(Display)}.{nameof(Display.Moved)}",
		Kind.DisplayDesktopModeChanged => $"{nameof(Display)}.{nameof(Display.DesktopModeChanged)}",
		Kind.DisplayCurrentModeChanged => $"{nameof(Display)}.{nameof(Display.CurrentModeChanged)}",
		Kind.DisplayContentScaleChanged => $"{nameof(Display)}.{nameof(Display.ContentScaleChanged)}",

		Kind.WindowShown => $"{nameof(Window)}.{nameof(Window.Shown)}",
		Kind.WindowHidden => $"{nameof(Window)}.{nameof(Window.Hidden)}",
		Kind.WindowExposed => $"{nameof(Window)}.{nameof(Window.Exposed)}",
		Kind.WindowMoved => $"{nameof(Window)}.{nameof(Window.Moved)}",
		Kind.WindowResized => $"{nameof(Window)}.{nameof(Window.Resized)}",
		Kind.WindowPixelSizeChanged => $"{nameof(Window)}.{nameof(Window.PixelSizeChanged)}",
		Kind.WindowMetalViewResized => $"{nameof(Window)}.{nameof(Window.MetalViewResized)}",
		Kind.WindowMinimized => $"{nameof(Window)}.{nameof(Window.Minimized)}",
		Kind.WindowMaximized => $"{nameof(Window)}.{nameof(Window.Maximized)}",
		Kind.WindowRestored => $"{nameof(Window)}.{nameof(Window.Restored)}",
		Kind.WindowMouseEnter => $"{nameof(Window)}.{nameof(Window.MouseEnter)}",
		Kind.WindowMouseLeave => $"{nameof(Window)}.{nameof(Window.MouseLeave)}",
		Kind.WindowFocusGained => $"{nameof(Window)}.{nameof(Window.FocusGained)}",
		Kind.WindowFocusLost => $"{nameof(Window)}.{nameof(Window.FocusLost)}",
		Kind.WindowCloseRequested => $"{nameof(Window)}.{nameof(Window.CloseRequested)}",
		Kind.WindowHitTest => $"{nameof(Window)}.{nameof(Window.HitTest)}",
		Kind.WindowIccProfileChnaged => $"{nameof(Window)}.{nameof(Window.IccProfileChanged)}",
		Kind.WindowDisplayChanged => $"{nameof(Window)}.{nameof(Window.DisplayChanged)}",
		Kind.WindowDisplayScaleChanged => $"{nameof(Window)}.{nameof(Window.DisplayScaleChanged)}",
		Kind.WindowSafeAreaChanged => $"{nameof(Window)}.{nameof(Window.SafeAreaChanged)}",
		Kind.WindowOccluded => $"{nameof(Window)}.{nameof(Window.Occluded)}",
		Kind.WindowEnterFullscreen => $"{nameof(Window)}.{nameof(Window.EnterFullscreen)}",
		Kind.WindowLeaveFullscreen => $"{nameof(Window)}.{nameof(Window.LeaveFullscreen)}",
		Kind.WindowDestroyed => $"{nameof(Window)}.{nameof(Window.Destroyed)}",
		Kind.WindowHdrStateChanged => $"{nameof(Window)}.{nameof(Window.HdrStateChanged)}",

		Kind.KeyDown => $"{nameof(Keyboard)}.{nameof(Keyboard.KeyDown)}",
		Kind.KeyUp => $"{nameof(Keyboard)}.{nameof(Keyboard.KeyUp)}",
		Kind.TextEditing => $"{nameof(Keyboard)}.{nameof(Keyboard.TextEditing)}",
		Kind.TextInput => $"{nameof(Keyboard)}.{nameof(Keyboard.TextInput)}",
		Kind.KeymapChanged => $"{nameof(Keyboard)}.{nameof(Keyboard.KeymapChanged)}",
		Kind.KeyboardAdded => $"{nameof(Keyboard)}.{nameof(Keyboard.Added)}",
		Kind.KeyboardRemoved => $"{nameof(Keyboard)}.{nameof(Keyboard.Removed)}",
		Kind.TextEditingCandidates => $"{nameof(Keyboard)}.{nameof(Keyboard.TextEditingCandidates)}",

		Kind.MouseMotion => $"{nameof(Mouse)}.{nameof(Mouse.Motion)}",
		Kind.MouseButtonDown => $"{nameof(Mouse)}.{nameof(Mouse.ButtonDown)}",
		Kind.MouseButtonUp => $"{nameof(Mouse)}.{nameof(Mouse.ButtonUp)}",
		Kind.MouseWheel => $"{nameof(Mouse)}.{nameof(Mouse.WheelMotion)}",
		Kind.MouseAdded => $"{nameof(Mouse)}.{nameof(Mouse.Added)}",
		Kind.MouseRemoved => $"{nameof(Mouse)}.{nameof(Mouse.Removed)}",

		Kind.JoystickAxisMotion => $"{nameof(Joystick)}.{nameof(Joystick.AxisMotion)}",
		Kind.JoystickBallMotion => $"{nameof(Joystick)}.{nameof(Joystick.BallMotion)}",
		Kind.JoystickHatMotion => $"{nameof(Joystick)}.{nameof(Joystick.HatMotion)}",
		Kind.JoystickButtonDown => $"{nameof(Joystick)}.{nameof(Joystick.ButtonDown)}",
		Kind.JoystickButtonUp => $"{nameof(Joystick)}.{nameof(Joystick.ButtonUp)}",
		Kind.JoystickAdded => $"{nameof(Joystick)}.{nameof(Joystick.Added)}",
		Kind.JoystickRemoved => $"{nameof(Joystick)}.{nameof(Joystick.Removed)}",
		Kind.JoystickBatteryUpdated => $"{nameof(Joystick)}.{nameof(Joystick.BatteryUpdated)}",
		Kind.JoystickUpdateCompleted => $"{nameof(Joystick)}.{nameof(Joystick.UpdateCompleted)}",

		Kind.GamepadAxisMotion => $"{nameof(Gamepad)}.{nameof(Gamepad.AxisMotion)}",
		Kind.GamepadButtonDown => $"{nameof(Gamepad)}.{nameof(Gamepad.ButtonDown)}",
		Kind.GamepadButtonUp => $"{nameof(Gamepad)}.{nameof(Gamepad.ButtonUp)}",
		Kind.GamepadAdded => $"{nameof(Gamepad)}.{nameof(Gamepad.Added)}",
		Kind.GamepadRemoved => $"{nameof(Gamepad)}.{nameof(Gamepad.Removed)}",
		Kind.GamepadRemapped => $"{nameof(Gamepad)}.{nameof(Gamepad.Remapped)}",
		Kind.GamepadTouchpadDown => $"{nameof(Gamepad)}.{nameof(Gamepad.TouchpadDown)}",
		Kind.GamepadTouchpadMotion => $"{nameof(Gamepad)}.{nameof(Gamepad.TouchpadMotion)}",
		Kind.GamepadTouchpadUp => $"{nameof(Gamepad)}.{nameof(Gamepad.TouchpadUp)}",
		Kind.GamepadSensorUpdated => $"{nameof(Gamepad)}.{nameof(Gamepad.SensorUpdated)}",
		Kind.GamepadUpdateCompleted => $"{nameof(Gamepad)}.{nameof(Gamepad.UpdateCompleted)}",
		Kind.GamepadSteamHandleUpdated => $"{nameof(Gamepad)}.{nameof(Gamepad.SteamHandleUpdated)}",

		Kind.FingerDown => $"{nameof(Touch)}.{nameof(Touch.FingerDown)}",
		Kind.FingerUp => $"{nameof(Touch)}.{nameof(Touch.FingerUp)}",
		Kind.FingerMotion => $"{nameof(Touch)}.{nameof(Touch.FingerMotion)}",
		Kind.FingerCanceled => $"{nameof(Touch)}.{nameof(Touch.FingerCanceled)}",

		Kind.ClipboardUpdated => $"{nameof(Clipboard)}.{nameof(Clipboard.Updated)}",

		Kind.DropFile => $"{nameof(DragAndDrop)}.{nameof(DragAndDrop.File)}",
		Kind.DropText => $"{nameof(DragAndDrop)}.{nameof(DragAndDrop.Text)}",
		Kind.DropBegin => $"{nameof(DragAndDrop)}.{nameof(DragAndDrop.Begin)}",
		Kind.DropCompleted => $"{nameof(DragAndDrop)}.{nameof(DragAndDrop.Completed)}",
		Kind.DropPosition => $"{nameof(DragAndDrop)}.{nameof(DragAndDrop.Position)}",

		Kind.AudioDeviceAdded => $"{nameof(AudioDevice)}.{nameof(AudioDevice.Added)}",
		Kind.AudioDeviceRemoved => $"{nameof(AudioDevice)}.{nameof(AudioDevice.Removed)}",
		Kind.AudioDeviceFormatChanged => $"{nameof(AudioDevice)}.{nameof(AudioDevice.FormatChanged)}",

		Kind.SensorUpdated => $"{nameof(Sensor)}.{nameof(Sensor.Updated)}",

		Kind.PenProximityIn => $"{nameof(Pen)}.{nameof(Pen.ProximityIn)}",
		Kind.PenProximityOut => $"{nameof(Pen)}.{nameof(Pen.ProximityOut)}",
		Kind.PenDown => $"{nameof(Pen)}.{nameof(Pen.Down)}",
		Kind.PenUp => $"{nameof(Pen)}.{nameof(Pen.Up)}",
		Kind.PenButtonDown => $"{nameof(Pen)}.{nameof(Pen.ButtonDown)}",
		Kind.PenButtonUp => $"{nameof(Pen)}.{nameof(Pen.ButtonUp)}",
		Kind.PenMotion => $"{nameof(Pen)}.{nameof(Pen.Motion)}",
		Kind.PenAxis => $"{nameof(Pen)}.{nameof(Pen.AxisChanged)}",

		Kind.CameraDeviceAdded => $"{nameof(CameraDevice)}.{nameof(CameraDevice.Added)}",
		Kind.CameraDeviceRemoved => $"{nameof(CameraDevice)}.{nameof(CameraDevice.Removed)}",
		Kind.CameraDeviceApproved => $"{nameof(CameraDevice)}.{nameof(CameraDevice.Approved)}",
		Kind.CameraDeviceDenied => $"{nameof(CameraDevice)}.{nameof(CameraDevice.Denied)}",

		Kind.RenderTargetsReset => $"{nameof(Render)}.{nameof(Render.TargetsReset)}",
		Kind.RenderDeviceReset => $"{nameof(Render)}.{nameof(Render.DeviceReset)}",
		Kind.RenderDeviceLost => $"{nameof(Render)}.{nameof(Render.DeviceLost)}",

		_ => default
	};
}
