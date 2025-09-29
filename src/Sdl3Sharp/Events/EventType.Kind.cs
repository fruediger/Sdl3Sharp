using System.Diagnostics.CodeAnalysis;

namespace Sdl3Sharp.Events;

partial struct EventType
{
	internal enum Kind : uint
	{
		/// <summary>SDL_EVENT_FIRST</summary>
		First = 0,

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
		[Experimental("SDL6010")] //TODO: make 'SDL6010' the diagnostics id for 'reserved events for private platforms'
		Private0 = 0x4000,

		/// <summary>SDL_EVENT_PRIVATE1</summary>
		/// <remarks>Reserved event for private platforms. Do not use.</remarks>
		[Experimental("SDL6010")] //TODO: make 'SDL6010' the diagnostics id for 'reserved events for private platforms'
		Private1,

		/// <summary>SDL_EVENT_PRIVATE2</summary>
		/// <remarks>Reserved event for private platforms. Do not use.</remarks>
		[Experimental("SDL6010")] //TODO: make 'SDL6010' the diagnostics id for 'reserved events for private platforms'
		Private2,

		/// <summary>SDL_EVENT_PRIVATE3</summary>
		/// <remarks>Reserved event for private platforms. Do not use.</remarks>
		[Experimental("SDL6010")] //TODO: make 'SDL6010' the diagnostics id for 'reserved events for private platforms'
		Private3,

		#endregion

		#region Internal events

		/// <summary>SDL_EVENT_POLL_SENTINEL</summary>
		PollSentinel = 0x7F00,

		/// <summary>SDL_EVENT_USER</summary>
		User = 0x8000,		

		#endregion

		/// <summary>SDL_EVENT_LAST</summary>
		Last = 0xFFFF,
	}
}
