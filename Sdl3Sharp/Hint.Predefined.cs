using System.Runtime.CompilerServices;

namespace Sdl3Sharp;

partial struct Hint
{
	/// <summary>
	/// Gets a hint specifying the behavior of Alt+Tab while the keyboard is grabbed
	/// </summary>
	/// <value>
	/// A hint specifing the behavior of Alt+Tab while the keyboard is grabbed
	/// </value>
	/// <remarks>
	/// <para>
	/// By default, SDL emulates Alt+Tab functionality while the keyboard is grabbed and your window is full-screen. This prevents the user from getting stuck in your application if you've enabled keyboard grab.
	/// </para>
	/// <para>
	/// The hint can be set to the following values:
	/// <list type="bullet">
	///		<item>
	///			<term><c>"0"</c></term>
	///			<description>SDL will not handle Alt+Tab. Your application is responsible for handling Alt+Tab while the keyboard is grabbed.</description>
	///		</item>
	///		<item>
	///			<term><c>"1"</c></term>
	///			<description>SDL will minimize your window when Alt+Tab is pressed (default)</description>
	///		</item>
	/// </list>
	/// </para>
	/// <para>
	/// This hint can be set anytime.
	/// </para>
	/// </remarks>
	public static Hint AllowAltTabWhileGrabbed { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new("SDL_ALLOW_ALT_TAB_WHILE_GRABBED"); }

	/// <summary>SDL_HINT_ANDROID_*</summary>
	public static class Android
	{
		/// <summary>
		/// Gets a hint to control whether the SDL activity is allowed to be re-created
		/// </summary>
		/// <value>
		/// A hint to control whether the SDL activity is allowed to be re-created
		/// </value>
		/// <remarks>
		/// <para>
		/// If this hint is true, the activity can be recreated on demand by the OS, and Java static data and C++ static data remain with their current values. If this hint is false, then SDL will call <c>exit()</c> when you return from your main function and the application will be terminated and then started fresh each time.
		/// </para>
		/// <para>
		/// The hint can be set to the following values:
		/// <list type="bullet">
		///		<item>
		///			<term><c>"0"</c></term>
		///			<description>The application starts fresh at each launch (default)</description>
		///		</item>
		///		<item>
		///			<term><c>"1"</c></term>
		///			<description>The application activity can be recreated by the OS</description>
		///		</item>
		/// </list>
		/// </para>
		/// <para> 
		/// This hint can be set anytime.
		/// </para>
		/// </remarks>
		public static Hint AllowRecreateActivity { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new("SDL_ANDROID_ALLOW_RECREATE_ACTIVITY"); }

		/// <summary>
		/// Gets a hint to control whether the event loop will block itself when the app is paused
		/// </summary>
		/// <value>
		/// A hint to control whether the event loop will block itself when the app is paused
		/// </value>
		/// <remarks>
		/// <para>
		/// The hint can be set to the following values:
		/// <list type="bullet">
		///		<item>
		///			<term><c>"0"</c></term>
		///			<description>Non blocking</description>
		///		</item>
		///		<item>
		///			<term><c>"1"</c></term>
		///			<description>Blocking (default)</description>
		///		</item>
		/// </list>
		/// </para>
		/// <para> 
		/// This hint should be set before <see cref="Sdl(Sdl.BuildAction?)">SDL is initialized</see>.
		/// </para>
		/// </remarks>
		public static Hint BlockOnPause { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new("SDL_ANDROID_BLOCK_ON_PAUSE"); }

		/// <summary>
		/// Gets a hint to control whether low latency audio should be enabled
		/// </summary>
		/// <value>
		/// A hint to control whether low latency audio should be enabled
		/// </value>
		/// <remarks>
		/// <para>
		/// Some devices have poor quality output when this is enabled, but this is usually an improvement in audio latency.
		/// </para>
		/// <para>
		/// The hint can be set to the following values:
		/// <list type="bullet">
		///		<item>
		///			<term><c>"0"</c></term>
		///			<description>Low latency audio is not enabled</description>
		///		</item>
		///		<item>
		///			<term><c>"1"</c></term>
		///			<description>Low latency audio is enabled (default)</description>
		///		</item>
		/// </list>
		/// </para>
		/// <para>
		/// This hint should be set before <see cref="SubSystem.Audio">SDL audio</see> is initialized.
		/// </para>
		/// </remarks>
		public static Hint LowLatencyAudio { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new("SDL_ANDROID_LOW_LATENCY_AUDIO"); }

		/// <summary>
		/// Gets a hint to control whether we trap the Android back button to handle it manually
		/// </summary>
		/// <value>
		/// A hint to control whether we trap the Android back button to handle it manually
		/// </value>
		/// <remarks>
		/// <para>
		/// This is necessary for the right mouse button to work on some Android devices, or to be able to trap the back button for use in your code reliably. If this hint is true, the back button will show up as an <see cref="SDL_EVENT_KEY_DOWN"/> / <see cref="SDL_EVENT_KEY_UP"/> pair with a keycode of <see cref="SDL_SCANCODE_AC_BACK"/>.
		/// </para>
		/// <para>
		/// The hint can be set to the following values:
		/// <list type="bullet">
		///		<item>
		///			<term><c>"0"</c></term>
		///			<description>Back button will be handled as usual for system (default)</description>
		///		</item>
		///		<item>
		///			<term><c>"1"</c></term>
		///			<description>Back button will be trapped, allowing you to handle the key press manually (This will also let right mouse click work on systems where the right mouse button functions as back)</description>
		///		</item>
		/// </list>
		/// </para>
		/// <para>
		/// This hint can be set anytime.
		/// </para>
		/// </remarks>
		public static Hint TrapBackButton { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new("SDL_ANDROID_TRAP_BACK_BUTTON"); }
	}

	/// <summary>
	/// Gets a hint setting the app ID string
	/// </summary>
	/// <value>
	/// A hint setting the app ID string
	/// </value>
	/// <remarks>
	/// <para>
	/// This string is used by desktop compositors to identify and group windows together, as well as match applications with associated desktop settings and icons.
	/// </para>
	/// <para>
	/// This will override <see cref="Sdl.Metadata.Identifier"/>, if set by the application.
	/// </para>
	/// <para>
	/// This hint should be set before <see cref="Sdl(Sdl.BuildAction?)">SDL is initialized</see>.
	/// </para>
	/// </remarks>
	public static Hint AppId { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new("SDL_APP_ID"); }

	/// <summary>
	/// Gets a hint setting the application name
	/// </summary>
	/// <value>
	/// A hint setting the application name
	/// </value>
	/// <remarks>
	/// <para>
	/// This hint lets you specify the application name sent to the OS when required. For example, this will often appear in volume control applets for audio streams, and in lists of applications which are inhibiting the screensaver. You should use a string that describes your program ("My Game 2: The Revenge").
	/// </para>
	/// <para>
	/// This will override <see cref="Sdl.Metadata.Name"/>, if set by the application.
	/// </para>
	/// <para>
	/// This hint should be set before <see cref="Sdl(Sdl.BuildAction?)">SDL is initialized</see>.
	/// </para>
	/// </remarks>
	public static Hint AppName { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new("SDL_APP_NAME"); }

	/// <summary>SDL_HINT_APPLE_TV_*</summary>
	public static class AppleTV
	{
		/// <summary>
		/// Gets a hint controlling whether controllers used with the Apple TV generate UI events
		/// </summary>
		/// <value>
		/// A hint controlling whether controllers used with the Apple TV generate UI events
		/// </value>
		/// <remarks>
		/// <para>
		/// When UI events are generated by controller input, the app will be backgrounded when the Apple TV remote's menu button is pressed, and when the pause or B buttons on gamepads are pressed.
		/// </para>
		/// <para>
		/// More information about properly making use of controllers for the Apple TV can be found here: <see href="https://developer.apple.com/tvos/human-interface-guidelines/remote-and-controllers/"/>.
		/// </para>
		/// <para>
		/// The hint can be set to the following values:
		/// <list type="bullet">
		///		<item>
		///			<term><c>"0"</c></term>
		///			<description>Controller input does not generate UI events (default)</description>
		///		</item>
		///		<item>
		///			<term><c>"1"</c></term>
		///			<description>Controller input generates UI events</description>
		///		</item>
		/// </list>
		/// </para>
		/// <para>
		/// This hint can be set anytime.
		/// </para>
		/// </remarks>
		public static Hint ControllerUIEvents { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new("SDL_APPLE_TV_CONTROLLER_UI_EVENTS"); }

		/// <summary>
		/// Gets a hint controlling whether the Apple TV remote's joystick axes will automatically match the rotation of the remote
		/// </summary>
		/// <value>
		/// A hint controlling whether the Apple TV remote's joystick axes will automatically match the rotation of the remote
		/// </value>
		/// <remarks>
		/// <para>
		/// The hint can be set to the following values:
		/// <list type="bullet">
		///		<item>
		///			<term><c>"0"</c></term>
		///			<description>Remote orientation does not affect joystick axes (default)</description>
		///		</item>
		///		<item>
		///			<term><c>"1"</c></term>
		///			<description>Joystick axes are based on the orientation of the remote</description>
		///		</item>
		/// </list>
		/// </para>
		/// <para>
		/// This hint can be set anytime.
		/// </para>
		/// </remarks>
		public static Hint RemoteAllowRotation { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new("SDL_APPLE_TV_REMOTE_ALLOW_ROTATION"); }
	}

	/// <summary>
	/// Gets a hint controlling response to <see cref="SDL_assert"/> failures
	/// </summary>
	/// <value>
	/// A hint controlling response to <see cref="SDL_assert"/> failures
	/// </value>
	/// <remarks>
	/// <para>
	/// The hint can be set to the following case-sensitive values:
	/// <list type="bullet">
	///		<item>
	///			<term><c>"abort"</c></term>
	///			<description>Program terminates immediately</description>
	///		</item>
	///		<item>
	///			<term><c>"break"</c></term>
	///			<description>Program triggers a debugger breakpoint</description>
	///		</item>
	///		<item>
	///			<term><c>"retry"</c></term>
	///			<description>Program reruns the <see cref="SDL_assert"/>'s test again</description>
	///		</item>
	///		<item>
	///			<term><c>"ignore"</c></term>
	///			<description>Program continues on, ignoring this assertion failure this time</description>
	///		</item>
	///		<item>
	///			<term><c>"always_ignore"</c></term>
	///			<description>Program continues on, ignoring this assertion failure for the rest of the run</description>
	///		</item>
	/// </list>
	/// </para>
	/// <para>
	/// Note that <see cref="SDL_SetAssertionHandler"/> offers a programmatic means to deal with assertion failures through a callback, and this hint is largely intended to be used via environment variables by end users and automated tools.
	/// </para>
	/// <para>
	/// This hint should be set before an assertion failure is triggered and can be changed at any time.
	/// </para>
	/// </remarks>
	public static Hint Assert { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new("SDL_ASSERT"); }

	/// <summary>SDL_HINT_AUDIO_*</summary>
	public static class Audio
	{
		/// <summary>SDL_HINT_AUDIO_ALSA_*</summary>
		public static class Alsa
		{
			/// <summary>
			/// Gets a hint specifying the default ALSA audio device name
			/// </summary>
			/// <value>
			/// A hint specifying the default ALSA audio device name
			/// </value>
			/// <remarks>
			/// <para>
			/// This hint is a specific audio device to open when the "default" audio device is used.
			/// </para>
			/// <para>
			/// This hint will be ignored when opening the default playback device if <see cref="DefaultPlaybackDevice">Hint.Audio.Alsa.DefaultPlaybackDevice</see> is set, or when opening the default recording device if <see cref="DefaultPlaybackDevice">Hint.Audio.Alsa.DefaultPlaybackDevice</see> is set.
			/// </para>
			/// <para>
			/// This hint should be set before <see cref="">an audio device is opened</see>.
			/// </para>
			/// </remarks>
			public static Hint DefaultDevice { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new("SDL_AUDIO_ALSA_DEFAULT_DEVICE"); }

			/// <summary>
			/// Gets a hint specifying the default ALSA audio playback device name
			/// </summary>
			/// <value>
			/// A hint specifying the default ALSA audio playback device name
			/// </value>
			/// <remarks>
			/// <para>
			/// This hint is a specific audio device to open for playback, when the "default" audio device is used.
			/// </para>
			/// <para>
			/// If this hint isn't set, SDL will check <see cref="DefaultDevice">Hint.Audio.Alsa.DefaultDevice</see> before choosing a reasonable default.
			/// </para>
			/// <para>
			/// This hint should be set before <see cref="">an audio device is opened</see>.
			/// </para>
			/// </remarks>
			public static Hint DefaultPlaybackDevice { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new("SDL_AUDIO_ALSA_DEFAULT_PLAYBACK_DEVICE"); }

			/// <summary>
			/// Gets a hint specifying the default ALSA audio recording device name
			/// </summary>
			/// <value>
			/// A hint specifying the default ALSA audio recording device name
			/// </value>
			/// <remarks>
			/// <para>
			/// This hint is a specific audio device to open for recording, when the "default" audio device is used.
			/// </para>
			/// <para>
			/// If this hint isn't set, SDL will check <see cref="DefaultDevice">Hint.Audio.Alsa.DefaultDevice</see> before choosing a reasonable default.
			/// </para>
			/// <para>
			/// This hint should be set before <see cref="">an audio device is opened</see>.
			/// </para>
			/// </remarks>
			public static Hint DefaultRecordingDevice { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new("SDL_AUDIO_ALSA_DEFAULT_RECORDING_DEVICE"); }
		}

		/// <summary>
		/// Gets a hint controlling the audio category on iOS and macOS
		/// </summary>
		/// <value>
		/// A hint controlling the audio category on iOS and macOS
		/// </value>
		/// <remarks>
		/// <para>
		/// The hint can be set to the following values:
		/// <list type="bullet">
		///		<item>
		///			<term><c>"ambient"</c></term>
		///			<description>Use the <c>AVAudioSessionCategoryAmbient</c> audio category, will be muted by the phone mute switch (default)</description>
		///		</item>
		///		<item>
		///			<term><c>"playback"</c></term>
		///			<description>Use the <c>AVAudioSessionCategoryPlayback</c> category</description>
		///		</item>
		/// </list>
		/// </para>
		/// <para>
		/// For more information, see Apple's documentation: <see href="https://developer.apple.com/library/content/documentation/Audio/Conceptual/AudioSessionProgrammingGuide/AudioSessionCategoriesandModes/AudioSessionCategoriesandModes.html"/>.
		/// </para>
		/// <para>
		/// This hint should be set before <see cref="">an audio device is opened</see>.
		/// </para>
		/// </remarks>
		public static Hint Category { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new("SDL_AUDIO_CATEGORY"); }

		/// <summary>
		/// Gets a hint controlling the default audio channel count
		/// </summary>
		/// <value>
		/// A hint controlling the default audio channel count
		/// </value>
		/// <remarks>
		/// <para>
		/// If the application doesn't specify the audio channel count when opening the device, this hint can be used to specify a default channel count that will be used. This defaults to <c>"1"</c> for recording and <c>"2"</c> for playback devices.
		/// </para>
		/// <para>
		/// This hint should be set before <see cref="">an audio device is opened</see>.
		/// </para>
		/// </remarks>
		public static Hint Channels { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new("SDL_AUDIO_CHANNELS"); }

		/// <summary>SDL_HINT_AUDIO_DEVICE_*</summary>
		public static class Device
		{
			/// <summary>
			/// Gets a hint specifying an application icon name for an audio device
			/// </summary>
			/// <value>
			/// A hint specifying an application icon name for an audio device
			/// </value>
			/// <remarks>
			/// <para>
			/// Some audio backends (such as Pulseaudio and Pipewire) allow you to set an XDG icon name for your application. Among other things, this icon might show up in a system control panel that lets the user adjust the volume on specific audio streams instead of using one giant master volume slider. Note that this is unrelated to the icon used by the windowing system, which may be set with <see cref="SDL_SetWindowIcon"/> (or via desktop file on Wayland).
			/// </para>
			/// <para>
			/// Setting this to <c>""</c> or leaving it unset will have SDL use a reasonable default, <c>"applications-games"</c>, which is likely to be installed. See <see href="https://specifications.freedesktop.org/icon-theme-spec/icon-theme-spec-latest.html"/> and <see href="https://specifications.freedesktop.org/icon-naming-spec/icon-naming-spec-latest.html"/> for the relevant XDG icon specs.
			/// </para>
			/// <para>
			/// This hint should be set before <see cref="">an audio device is opened</see>.
			/// </para>
			/// </remarks>
			public static Hint AppIconName { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new("SDL_AUDIO_DEVICE_APP_ICON_NAME"); }

			/// <summary>
			/// Gets a hint controlling device buffer size
			/// </summary>
			/// <value>
			/// A hint controlling device buffer size
			/// </value>
			/// <remarks>
			/// <para>
			/// This hint is an integer <c>&gt; 0</c>, that represents the size of the device's buffer in sample frames (stereo audio data in 16-bit format is 4 bytes per sample frame, for example).
			/// </para>
			/// <para>
			/// SDL3 generally decides this value on behalf of the app, but if for some reason the app needs to dictate this (because they want either lower latency or higher throughput <em>AND ARE WILLING TO DEAL WITH</em> what that might require of the app), they can specify it.
			/// </para>
			/// <para>
			/// SDL will try to accommodate this value, but there is no promise you'll get the buffer size requested. Many platforms won't honor this request at all, or might adjust it.
			/// </para>
			/// <para>
			/// This hint should be set before <see cref="">an audio device is opened</see>.
			/// </para>
			/// </remarks>
			public static Hint SampleFrames { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new("SDL_AUDIO_DEVICE_SAMPLE_FRAMES"); }

			/// <summary>
			/// Gets a hint specifying an audio stream name for an audio device
			/// </summary>
			/// <value>
			/// A hint specifying an audio stream name for an audio device
			/// </value>
			/// <remarks>
			/// <para>
			/// Some audio backends (such as PulseAudio) allow you to describe your audio stream. Among other things, this description might show up in a system control panel that lets the user adjust the volume on specific audio streams instead of using one giant master volume slider.
			/// </para>
			/// <para>
			/// This hints lets you transmit that information to the OS. The contents of this hint are used while opening an audio device. You should use a string that describes your what your program is playing (<c>"audio stream"</c> is probably sufficient in many cases, but this could be useful for something like <c>"team chat"</c> if you have a headset playing VoIP audio separately).
			/// </para>
			/// <para>
			/// Setting this to <c>""</c> or leaving it unset will have SDL use a reasonable default: <c>"audio stream"</c> or something similar.
			/// </para>
			/// <para>
			/// Note that while this talks about audio streams, this is an OS-level concept, so it applies to a physical audio device in this case, and not an <see cref="SDL_AudioStream"/>, nor an SDL logical audio device.
			/// </para>
			/// <para>
			/// This hint should be set before <see cref="">an audio device is opened</see>.
			/// </para>
			/// </remarks>
			public static Hint StreamName { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new("SDL_AUDIO_DEVICE_STREAM_NAME"); }

			/// <summary>
			/// Gets a hint specifying an application role for an audio device
			/// </summary>
			/// <value>
			/// A hint specifying an application role for an audio device
			/// </value>
			/// <remarks>
			/// <para>
			/// Some audio backends (such as Pipewire) allow you to describe the role of your audio stream. Among other things, this description might show up in a system control panel or software for displaying and manipulating media playback/recording graphs.
			/// </para>
			/// <para>
			/// This hints lets you transmit that information to the OS. The contents of this hint are used while opening an audio device. You should use a string that describes your what your program is playing (Game, Music, Movie, etc...).
			/// </para>
			/// <para>
			/// Setting this to <c>""</c> or leaving it unset will have SDL use a reasonable default: <c>"Game"</c> or something similar.
			/// </para>
			/// <para>
			/// Note that while this talks about audio streams, this is an OS-level concept, so it applies to a physical audio device in this case, and not an <see cref="SDL_AudioStream"/>, nor an SDL logical audio device.
			/// </para>
			/// <para>
			/// This hint should be set before <see cref="">an audio device is opened</see>.
			/// </para>
			/// </remarks>
			public static Hint StreamRole { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new("SDL_AUDIO_DEVICE_STREAM_ROLE"); }
		}

		/// <summary>SDL_HINT_AUDIO_DISK_*</summary>
		public static class Disk
		{
			/// <summary>
			/// Gets a hint specifying the input file when recording audio using the disk audio driver
			/// </summary>
			/// <value>
			/// A hint specifying the input file when recording audio using the disk audio driver
			/// </value>
			/// <remarks>
			/// <para>
			/// This defaults to <c>"sdlaudio-in.raw"</c>.
			/// </para>
			/// <para>
			/// This hint should be set before <see cref="">an audio device is opened</see>.
			/// </para>
			/// </remarks>
			public static Hint InputFile { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new("SDL_AUDIO_DISK_INPUT_FILE"); }

			/// <summary>
			/// Gets a hint specifying the output file when playing audio using the disk audio driver
			/// </summary>
			/// <value>
			/// A hint specifying the output file when playing audio using the disk audio driver
			/// </value>
			/// <remarks>
			/// <para>
			/// This defaults to <c>"sdlaudio.raw"</c>.
			/// </para>
			/// <para>
			/// This hint should be set before <see cref="">an audio device is opened</see>.
			/// </para>
			/// </remarks>
			public static Hint OutputFile { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new("SDL_AUDIO_DISK_OUTPUT_FILE"); }

			/// <summary>
			/// Gets a hint controlling the audio rate when using the disk audio driver
			/// </summary>
			/// <value>
			/// A hint controlling the audio rate when using the disk audio driver
			/// </value>
			/// <remarks>
			/// <para>
			/// The disk audio driver normally simulates real-time for the audio rate that was specified, but you can use this hint to adjust this rate higher or lower down to 0. The default value is <c>"1.0"</c>.
			/// </para>
			/// <para>
			/// This hint should be set before <see cref="">an audio device is opened</see>.
			/// </para>
			/// </remarks>
			public static Hint Timescale { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new("SDL_AUDIO_DISK_TIMESCALE"); }
		}

		/// <summary>
		/// Gets a hint that specifies an audio backend to use
		/// </summary>
		/// <value>
		/// A hint that specifies an audio backend to use
		/// </value>
		/// <remarks>
		/// <para>
		/// By default, SDL will try all available audio backends in a reasonable order until it finds one that can work, but this hint allows the app or user to force a specific driver, such as <c>"pipewire"</c> if, say, you are on PulseAudio but want to try talking to the lower level instead.
		/// </para>
		/// <para>
		/// This hint should be set before <see cref="Sdl(Sdl.BuildAction?)">SDL is initialized</see>.
		/// </para>
		/// </remarks>
		public static Hint Driver { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new("SDL_AUDIO_DRIVER"); }

		/// <summary>SDL_HINT_AUDIO_DUMMY_*</summary>
		public static class Dummy
		{
			/// <summary>
			/// Gets a hint controlling the audio rate when using the dummy audio driver
			/// </summary>
			/// <value>
			/// A hint controlling the audio rate when using the dummy audio driver
			/// </value>
			/// <remarks>
			/// <para>
			/// The dummy audio driver normally simulates real-time for the audio rate that was specified, but you can use this hint to adjust this rate higher or lower down to 0. The default value is <c>"1.0"</c>.
			/// </para>
			/// <para>
			/// This hint should be set before <see cref="">an audio device is opened</see>.
			/// </para>
			/// </remarks>
			public static Hint Timescale { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new("SDL_AUDIO_DUMMY_TIMESCALE"); }
		}

		/// <summary>
		/// Gets a hint controlling the default audio format
		/// </summary>
		/// <value>
		/// A hint controlling the default audio format
		/// </value>
		/// <remarks>
		/// <para>
		/// If the application doesn't specify the audio format when opening the device, this hint can be used to specify a default format that will be used.
		/// </para>
		/// <para>
		/// The hint can be set to the following values:
		/// <list type="bullet">
		///		<item>
		///			<term><c>"U8"</c></term>
		///			<description>Unsigned 8-bit audio</description>
		///		</item>
		///		<item>
		///			<term><c>"S8"</c></term>
		///			<description>Signed 8-bit audio</description>
		///		</item>
		///		<item>
		///			<term><c>"S16LE"</c></term>
		///			<description>Signed 16-bit little-endian audio</description>
		///		</item>
		///		<item>
		///			<term><c>"S16BE"</c></term>
		///			<description>Signed 16-bit big-endian audio</description>
		///		</item>
		///		<item>
		///			<term><c>"S16"</c></term>
		///			<description>Signed 16-bit native-endian audio (default)</description>
		///		</item>
		///		<item>
		///			<term><c>"S32LE"</c></term>
		///			<description>Signed 32-bit little-endian audio</description>
		///		</item>
		///		<item>
		///			<term><c>"S32BE"</c></term>
		///			<description>Signed 32-bit big-endian audio</description>
		///		</item>
		///		<item>
		///			<term><c>"S32"</c></term>
		///			<description>Signed 32-bit native-endian audio</description>
		///		</item>
		///		<item>
		///			<term><c>"F32LE"</c></term>
		///			<description>Floating point little-endian audio</description>
		///		</item>
		///		<item>
		///			<term><c>"F32BE"</c></term>
		///			<description>Floating point big-endian audio</description>
		///		</item>
		///		<item>
		///			<term><c>"F32"</c></term>
		///			<description>Floating point native-endian audio</description>
		///		</item>
		/// </list>
		/// </para>
		/// <para>
		/// This hint should be set before <see cref="">an audio device is opened</see>.
		/// </para>
		/// </remarks>
		public static Hint Format { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new("SDL_AUDIO_FORMAT"); }

		/// <summary>
		/// Gets a hint controlling the default audio frequency
		/// </summary>
		/// <value>
		/// A hint controlling the default audio frequency
		/// </value>
		/// <remarks>
		/// <para>
		/// If the application doesn't specify the audio frequency when opening the device, this hint can be used to specify a default frequency that will be used. This defaults to <c>"44100"</c>.
		/// </para>
		/// <para>
		/// This hint should be set before <see cref="">an audio device is opened</see>.
		/// </para>
		/// </remarks>
		public static Hint Frequency { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new("SDL_AUDIO_FREQUENCY"); }

		/// <summary>
		/// Gets a hint that causes SDL to not ignore audio "monitors"
		/// </summary>
		/// <value>
		/// A hint that causes SDL to not ignore audio "monitors"
		/// </value>
		/// <remarks>
		/// <para>
		/// This is currently only used by the PulseAudio driver.
		/// </para>
		/// <para>
		/// By default, SDL ignores audio devices that aren't associated with physical hardware. Changing this hint to <c>"1"</c> will expose anything SDL sees that appears to be an audio source or sink. This will add "devices" to the list that the user probably doesn't want or need, but it can be useful in scenarios where you want to hook up SDL to some sort of virtual device, etc.
		/// </para>
		/// <para>
		/// The hint can be set to the following values:
		/// <list type="bullet">
		///		<item>
		///			<term><c>"0"</c></term>
		///			<description>Audio monitor devices will be ignored (default)</description>
		///		</item>
		///		<item>
		///			<term><c>"1"</c></term>
		///			<description>Audio monitor devices will show up in the device list</description>
		///		</item>
		/// </list>
		/// </para>
		/// <para>
		/// This hint should be set before <see cref="Sdl(Sdl.BuildAction?)">SDL is initialized</see>.
		/// </para>
		/// </remarks>
		public static Hint IncludeMonitors { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new("SDL_AUDIO_INCLUDE_MONITORS"); }
	}

	/// <summary>
	/// Gets a hint controlling whether SDL updates joystick state when getting input events
	/// </summary>
	/// <value>
	/// A hint controlling whether SDL updates joystick state when getting input events
	/// </value>
	/// <remarks>
	/// <para>
	/// The hint can be set to the following values:
	/// <list type="bullet">
	///		<item>
	///			<term><c>"0"</c></term>
	///			<description>You'll call <see cref="SDL_UpdateJoysticks"/>() manually</description>
	///		</item>
	///		<item>
	///			<term><c>"1"</c></term>
	///			<description>SDL will automatically call <see cref="SDL_UpdateJoysticks"/>() (default)</description>
	///		</item>
	/// </list>
	/// </para>
	/// <para>
	/// This hint can be set anytime.
	/// </para>
	/// </remarks>
	public static Hint AutoUpdateJoysticks { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new("SDL_AUTO_UPDATE_JOYSTICKS"); }

	/// <summary>
	/// Gets a hint controlling whether SDL updates sensor state when getting input events
	/// </summary>
	/// <value>
	/// A hint controlling whether SDL updates sensor state when getting input events
	/// </value>
	/// <remarks>
	/// <para>
	/// The hint can be set to the following values:
	/// <list type="bullet">
	///		<item>
	///			<term><c>"0"</c></term>
	///			<description>You'll call <see cref="SDL_UpdateSensors"/>() manually</description>
	///		</item>
	///		<item>
	///			<term><c>"1"</c></term>
	///			<description>SDL will automatically call <see cref="SDL_UpdateSensors"/>() (default)</description>
	///		</item>
	/// </list>
	/// </para>
	/// <para>
	/// This hint can be set anytime.
	/// </para>
	/// </remarks>
	public static Hint AutoUpdateSensors { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new("SDL_AUTO_UPDATE_SENSORS"); }


	/// <summary>
	/// Gets a hint to control whether or not to prevent SDL from using version 4 of the bitmap header when saving BMPs
	/// </summary>
	/// <value>
	/// A hint to control whether or not to prevent SDL from using version 4 of the bitmap header when saving BMPs
	/// </value>
	/// <remarks>
	/// <para>
	/// The bitmap header version 4 is required for proper alpha channel support and SDL will use it when required. Should this not be desired, this hint can force the use of the 40 byte header version which is supported everywhere.
	/// </para>
	/// <para>
	/// The hint can be set to the following values:
	/// <list type="bullet">
	///		<item>
	///			<term><c>"0"</c></term>
	///			<description>Surfaces with a colorkey or an alpha channel are saved to a 32-bit BMP file with an alpha mask. SDL will use the bitmap header version 4 and set the alpha mask accordingly. (default)</description>
	///		</item>
	///		<item>
	///			<term><c>"1"</c></term>
	///			<description>Surfaces with a colorkey or an alpha channel are saved to a 32-bit BMP file without an alpha mask. The alpha channel data will be in the file, but applications are going to ignore it.</description>
	///		</item>
	/// </list>
	/// </para>
	/// <para>
	/// This hint can be set anytime.
	/// </para>
	/// </remarks>
	public static Hint BmpSaveLegacyFormat { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new("SDL_BMP_SAVE_LEGACY_FORMAT"); }

	/// <summary>SDL_HINT_CAMERA_*</summary>
	public static class Camera
	{
		/// <summary>
		/// Get a hint that decides what camera backend to use
		/// </summary>
		/// <value>
		/// A hint that decides what camera backend to use
		/// </value>
		/// <remarks>
		/// <para>
		/// By default, SDL will try all available camera backends in a reasonable order until it finds one that can work, but this hint allows the app or user to force a specific target, such as <c>"directshow"</c> if, say, you are on Windows Media Foundations but want to try DirectShow instead.
		/// </para>
		/// <para>
		/// The default value is unset, in which case SDL will try to figure out the best camera backend on your behalf. This hint needs to be set before <see cref="Sdl(Sdl.BuildAction?)">the <see cref="Sdl"/> instance is created</see> to be useful.
		/// </para>
		/// </remarks>
		public static Hint Driver { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new("SDL_CAMERA_DRIVER"); }
	}

	/// <summary>
	/// Gets a hint that limits what CPU features are available
	/// </summary>
	/// <value>
	/// A hint that limits what CPU features are available
	/// </value>
	/// <remarks>
	/// <para>
	/// By default, SDL marks all features the current CPU supports as available. This hint allows to limit these to a subset.
	/// </para>
	/// <para>
	/// When the hint is unset, or empty, SDL will enable all detected CPU features.
	/// </para>
	/// <para>
	/// The hint can be set to a comma separated list containing the following items:
	/// <list type="bullet">
	///		<item><description><c>"all"</c></description></item>
	///		<item><description><c>"altivec"</c></description></item>
	///		<item><description><c>"sse"</c></description></item>
	///		<item><description><c>"sse2"</c></description></item>
	///		<item><description><c>"sse3"</c></description></item>
	///		<item><description><c>"sse41"</c></description></item>
	///		<item><description><c>"sse42"</c></description></item>
	///		<item><description><c>"avx"</c></description></item>
	///		<item><description><c>"avx2"</c></description></item>
	///		<item><description><c>"avx512f"</c></description></item>
	///		<item><description><c>"arm-simd"</c></description></item>
	///		<item><description><c>"neon"</c></description></item>
	///		<item><description><c>"lsx"</c></description></item>
	///		<item><description><c>"lasx"</c></description></item>
	/// </list>
	/// </para>
	/// <para>
	/// The items can be prefixed by <c>'+'</c>/<c>'-'</c> to add/remove features.
	/// </para>
	/// </remarks>
	public static Hint CpuFeatureMask { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new("SDL_CPU_FEATURE_MASK"); }

	/// <summary>
	/// Gets a hint serving as an override for <see cref="SDL_GetDisplayUsableBounds"/>()
	/// </summary>
	/// <value>
	/// A hint serving as an override for <see cref="SDL_GetDisplayUsableBounds"/>()
	/// </value>
	/// <remarks>
	/// <para>
	/// If set, this hint will override the expected results for <see cref="SDL_GetDisplayUsableBounds"/>() for display index 0. Generally you don't want to do this, but this allows an embedded system to request that some of the screen be reserved for other uses when paired with a well-behaved application.
	/// </para>
	/// <para>
	/// The contents of this hint must be 4 comma-separated integers, the first is the bounds x, then y, width and height, in that order.
	/// </para>
	/// <para>
	/// This hint can be set anytime.
	/// </para>
	/// </remarks>
	public static Hint DisplayUsableBounds { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new("SDL_DISPLAY_USABLE_BOUNDS"); }

	/// <summary>SDL_HINT_EGL_*</summary>
	public static class Egl
	{
		/// <summary>
		/// Gets a hint specifying the EGL library to load
		/// </summary>
		/// <value>
		/// A hint specifying the EGL library to load
		/// </value>
		/// <remarks>
		/// This hint should be set before <see cref="">creating an OpenGL window</see> or <see cref="">creating an OpenGL context</see>. This hint is only considered if SDL is using EGL to manage OpenGL contexts. If this hint isn't set, SDL will choose a reasonable default.
		/// </remarks>
		public static Hint Library { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new("SDL_EGL_LIBRARY"); }
	}

	/// <summary>SDL_HINT_EMSCRIPTEN_*</summary>
	public static class Emscripten
	{
		/// <summary>
		/// Gets a hint to disable giving back control to the browser automatically when running with asyncify
		/// </summary>
		/// <value>
		/// A hint to disable giving back control to the browser automatically when running with asyncify
		/// </value>
		/// <remarks>
		/// <para>
		/// With <c>-s ASYNCIFY</c>, SDL calls <c>emscripten_sleep</c> during operations such as refreshing the screen or polling events.
		/// </para>
		/// <para>
		/// This hint only applies to the emscripten platform.
		/// </para>
		/// <para>
		/// The hint can be set to the following values:
		/// <list type="bullet">
		///		<item>
		///			<term><c>"0"</c></term>
		///			<description>Disable <c>emscripten_sleep</c> calls (if you give back browser control manually or use asyncify for other purposes)</description>
		///		</item>
		///		<item>
		///			<term><c>"1"</c></term>
		///			<description>Enable <c>emscripten_sleep</c> calls (default)</description>
		///		</item>
		/// </list>
		/// </para>
		/// <para>
		/// This hint can be set anytime.
		/// </para>
		/// </remarks>
		public static Hint Asyncify { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new("SDL_EMSCRIPTEN_ASYNCIFY"); }

		/// <summary>
		/// Gets a hint specifying the CSS selector used for the "default" window/canvas
		/// </summary>
		/// <value>
		/// A hint specifying the CSS selector used for the "default" window/canvas
		/// </value>
		/// <remarks>
		/// <para>
		/// This hint only applies to the emscripten platform.
		/// </para>
		/// <para>
		/// The default value is <c>"#canvas"</c>.
		/// </para>
		/// <para>
		/// This hint should be set before <see cref="">creating a window</see>.
		/// </para>
		/// </remarks>
		public static Hint CanvasSelector { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new("SDL_EMSCRIPTEN_CANVAS_SELECTOR"); }

		/// <summary>
		/// Gets a hint serving as an override for the binding element for keyboard inputs for Emscripten builds
		/// </summary>
		/// <value>
		/// A hint serving as an override for the binding element for keyboard inputs for Emscripten builds
		/// </value>
		/// <remarks>
		/// <para>
		/// This hint only applies to the emscripten platform.
		/// </para>
		/// <para>
		/// The hint can be one of:
		/// <list type="bullet">
		///		<item>
		///			<term><c>"#window"</c></term>
		///			<description>The javascript window object (default)</description>
		///		</item>
		///		<item>
		///			<term><c>"#document"</c></term>
		///			<description>The javascript document object</description>
		///		</item>
		///		<item>
		///			<term><c>#screen"</c></term>
		///			<description>The javascript window.screen object</description>
		///		</item>
		///		<item>
		///			<term><c>"#canvas"</c></term>
		///			<description>The WebGL canvas element</description>
		///		</item>
		///		<item>
		///			<term><c>"#none"</c></term>
		///			<description>Don't bind anything at all</description>
		///		</item>
		///		<item>
		///			<term>Any other string <em>without</em> a leading <c>#</c> sign</term>
		///			<description>The element on the page with that ID</description>
		///		</item>
		/// </list>
		/// </para>
		/// <para>
		/// This hint should be set before <see cref="">creating a window</see>.
		/// </para>
		/// </remarks>
		public static Hint KeyboardElement { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new("SDL_EMSCRIPTEN_KEYBOARD_ELEMENT"); }
	}

	/// <summary>
	/// Gets a hint that controls whether the on-screen keyboard should be shown when text input is active
	/// </summary>
	/// <value>
	/// A hint that controls whether the on-screen keyboard should be shown when text input is active
	/// </value>
	/// <remarks>
	/// <para>
	/// The hint can be set to the following values:
	/// <list type="bullet">
	///		<item>
	///			<term><c>"auto"</c></term>
	///			<description>The on-screen keyboard will be shown if there is no physical keyboard attached (default)</description>
	///		</item>
	///		<item>
	///			<term><c>"0"</c></term>
	///			<description>Do not show the on-screen keyboard</description>
	///		</item>
	///		<item>
	///			<term><c>"1"</c></term>
	///			<description>Show the on-screen keyboard, if available</description>
	///		</item>
	/// </list>
	/// </para>
	/// <para>
	/// This hint must be set before <see cref="SDL_StartTextInput"/>() is called.
	/// </para>
	/// </remarks>
	public static Hint EnableScreenKeyboard { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new("SDL_ENABLE_SCREEN_KEYBOARD"); }


	/// <summary>
	/// Gets a hint containing a list of evdev devices to use if udev is not available
	/// </summary>
	/// <value>
	/// A hint containing a list of evdev devices to use if udev is not available
	/// </value>
	/// <remarks>
	/// <para>
	/// The list of devices is in the form:
	/// <c>deviceclass:path[,deviceclass:path[,...]]</c>,
	/// where <c>deviceclass</c> is an integer representing the <see cref="SDL_UDEV_deviceclass"/> and <c>path</c> is the full path to the event device.
	/// </para>
	/// <para>
	/// This hint should be set before <see cref="Sdl(Sdl.BuildAction?)">SDL is initialized</see>.
	/// </para>
	/// </remarks>
	public static Hint EvDevDevices { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new("SDL_EVDEV_DEVICES"); }

	/// <summary>
	/// Gets a hint controlling verbosity of the logging of SDL events pushed onto the internal queue
	/// </summary>
	/// <value>
	/// A hint controlling verbosity of the logging of SDL events pushed onto the internal queue
	/// </value>
	/// <remarks>
	/// <para>
	/// The hint can be set to the following values, from least to most verbose:
	/// <list type="bullet">
	///		<item>
	///			<term><c>"0"</c></term>
	///			<description>Don't log any events. (default)</description>
	///		</item>
	///		<item>
	///			<term><c>"1"</c></term>
	///			<description>Log most events (other than the really spammy ones)</description>
	///		</item>
	///		<item>
	///			<term><c>"2"</c></term>
	///			<description>Include mouse and finger motion events</description>
	///		</item>
	/// </list>
	/// </para>
	/// <para>
	/// This is generally meant to be used to debug SDL itself, but can be useful for application developers that need better visibility into what is going on in the event queue.
	/// Logged events are sent through <see cref="Log.Info(string)"/>, which means by default they appear on stdout on most platforms or maybe <see href="https://learn.microsoft.com/de-de/windows/win32/api/debugapi/nf-debugapi-outputdebugstringa">OutputDebugString</see>() on Windows
	/// (when <see cref="Log.UseDefaultOutputForUnhandledMessages"/> is <c><see langword="true"/></c>), and can be funneled by the app with the <see cref="Log.Output"/> event, etc.
	/// </para>
	/// <para>
	/// This hint can be set anytime.
	/// </para>
	/// </remarks>
	public static Hint EventLogging { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new("SDL_EVENT_LOGGING"); }

	/// <summary>
	/// Gets a hint that specifies a dialog backend to use
	/// </summary>
	/// <value>
	/// A hint that specifies a dialog backend to use
	/// </value>
	/// <remarks>
	/// <para>
	/// By default, SDL will try all available dialog backends in a reasonable order until it finds one that can work, but this hint allows the app or user to force a specific target.
	/// </para>
	/// <para>
	/// If the specified target does not exist or is not available, the dialog-related calls will fail.
	/// </para>
	/// <para>
	/// This hint currently only applies to platforms using the generic "Unix" dialog implementation, but may be extended to more platforms in the future.
	/// Note that some Unix and Unix-like platforms have their own implementation, such as macOS and Haiku.
	/// </para>
	/// <para>
	/// The hint can be set to the following values:
	/// <list type="bullet">
	///		<item>
	///			<term><c><see langword="null"/></c></term>
	///			<description>Select automatically (default, all platforms)</description>
	///		</item>
	///		<item>
	///			<term><c>"portal"</c></term>
	///			<description>Use XDG Portals through DBus (Unix only)</description>
	///		</item>
	///		<item>
	///			<term><c>"zenity"</c></term>
	///			<description>Use the Zenity program (Unix only)</description>
	///		</item>
	/// </list>
	/// More options may be added in the future.
	/// </para>
	/// <para>
	/// This hint can be set anytime.
	/// </para>
	/// </remarks>
	public static Hint FileDialogDriver { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new("SDL_FILE_DIALOG_DRIVER"); }

	/// <summary>
	/// Gets a hint controlling whether raising the window should be done more forcefully
	/// </summary>
	/// <value>
	/// A hint controlling whether raising the window should be done more forcefully
	/// </value>
	/// <remarks>
	/// <para>
	/// The hint can be set to the following values:
	/// <list type="bullet">
	///		<item>
	///			<term><c>"0"</c></term>
	///			<description>Honor the OS policy for raising windows (default)</description>
	///		</item>
	///		<item>
	///			<term><c>"1"</c></term>
	///			<description>Force the window to be raised, overriding any OS policy</description>
	///		</item>
	/// </list>
	/// </para>
	/// <para>
	/// At present, this is only an issue under MS Windows, which makes it nearly impossible to programmatically move a window to the foreground, for "security" reasons.
	/// See <see href="http://stackoverflow.com/a/34414846"/> for a discussion.
	/// </para>
	/// <para>
	/// This hint can be set anytime.
	/// </para>
	/// </remarks>
	public static Hint ForceRaiseWindow { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new("SDL_FORCE_RAISEWINDOW"); }

	/// <summary>
	/// Gets a hint controlling how 3D acceleration is used to accelerate the SDL screen surface
	/// </summary>
	/// <value>
	/// A hint controlling how 3D acceleration is used to accelerate the SDL screen surface
	/// </value>
	/// <remarks>
	/// <para>
	/// SDL can try to accelerate the SDL screen surface by using streaming textures with a 3D rendering engine. This hint controls whether and how this is done.
	/// </para>
	/// <para>
	/// The hint can be set to the following values:
	/// <list type="bullet">
	///		<item>
	///			<term><c>"0"</c></term>
	///			<description>Disable 3D acceleration</description>
	///		</item>
	///		<item>
	///			<term><c>"1"</c></term>
	///			<description>Enable 3D acceleration, using the default renderer (default)</description>
	///		</item>
	///		<item>
	///			<term>Any other string</term>
	///			<description>Enable 3D acceleration, using the specified <em>valid</em> rendering drivers (e.g. <c>"direct3d"</c>, <c>"opengl"</c>, etc.)</description>
	///		</item>
	/// </list>
	/// </para>
	/// <para>
	/// This hint should be set before calling <see cref="SDL_GetWindowSurface"/>()
	/// </para>
	/// </remarks>
	public static Hint FramebufferAcceleration { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new("SDL_FRAMEBUFFER_ACCELERATION"); }

	/// <summary>SDL_HINT_GAMECONTROLLER*</summary>
	public static class GameController
	{

		/// <summary>
		/// Gets a hint that lets you manually hint extra gamecontroller db entries
		/// </summary>
		/// <value>
		/// A hint that lets you manually hint extra gamecontroller db entries
		/// </value>
		/// <remarks>
		/// <para>
		/// The hint should be newline delimited rows of gamecontroller config data, see <see cref="">SDL_gamepad</see>.h
		/// </para>
		/// <para>
		/// You can update mappings after SDL is initialized with <see cref="SDL_GetGamepadMappingForGUID"/>() and <see cref="SDL_AddGamepadMapping"/>()
		/// </para>
		/// <para>
		/// This hint should be set before <see cref="Sdl(Sdl.BuildAction?)">SDL is initialized</see>.
		/// </para>
		/// </remarks>
		public static Hint Config { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new("SDL_GAMECONTROLLERCONFIG"); }

		/// <summary>
		/// Gets a hint that lets you provide a file with extra gamecontroller db entries
		/// </summary>
		/// <value>
		/// A hint that lets you provide a file with extra gamecontroller db entries
		/// </value>
		/// <remarks>
		/// <para>
		/// The file should contain lines of gamecontroller config data, see <see cref="">SDL_gamepad</see>.h
		/// </para>
		/// <para>
		/// You can update mappings after SDL is initialized with <see cref="SDL_GetGamepadMappingForGUID"/>() and <see cref="SDL_AddGamepadMapping"/>()
		/// </para>
		/// <para>
		/// This hint should be set before <see cref="Sdl(Sdl.BuildAction?)">SDL is initialized</see>.
		/// </para>
		/// </remarks>
		public static Hint ConfigFile { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new("SDL_GAMECONTROLLERCONFIG_FILE"); }

		/// <summary>
		/// Gets a hint containing a list of devices to skip when scanning for game controllers
		/// </summary>
		/// <value>
		/// A hint containing a list of devices to skip when scanning for game controllers
		/// </value>
		/// <remarks>
		/// <para>
		/// The format of this hint is a comma separated list of USB VID/PID pairs in hexadecimal form, e.g. <c>"0xAAAA/0xBBBB,0xCCCC/0xDDDD"</c>.
		/// </para>
		/// <para>
		/// The hint can also take the form of <c>"@file"</c>, in which case the named <c>file</c> will be loaded and interpreted as the value of the hint.
		/// </para>
		/// <para>
		/// This hint can be set anytime.
		/// </para>
		/// </remarks>
		public static Hint IgnoreDevices { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new("SDL_GAMECONTROLLER_IGNORE_DEVICES"); }

		/// <summary>
		/// Gets a hint that if set, controls to skip all devices when scanning for game controllers except for the ones listed in this hint
		/// </summary>
		/// <value>
		/// A hint that if set, controls to skip all devices when scanning for game controllers except for the ones listed in this hint
		/// </value>
		/// <remarks>
		/// <para>
		/// The format of this hint is a comma separated list of USB VID/PID pairs in hexadecimal form, e.g. <c>"0xAAAA/0xBBBB,0xCCCC/0xDDDD"</c>.
		/// </para>
		/// <para>
		/// The hint can also take the form of <c>"@file"</c>, in which case the named <c>file</c> will be loaded and interpreted as the value of the hint.
		/// </para>
		/// <para>
		/// This hint can be set anytime.
		/// </para>
		/// </remarks>
		public static Hint IgnoreDevicesExcept { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new("SDL_GAMECONTROLLER_IGNORE_DEVICES_EXCEPT"); }

		/// <summary>
		/// Gets a hint that controls whether the device's built-in accelerometer and gyro should be used as sensors for gamepads
		/// </summary>
		/// <value>
		/// A hint that controls whether the device's built-in accelerometer and gyro should be used as sensors for gamepads
		/// </value>
		/// <remarks>
		/// <para>
		/// The hint can be set to the following values:
		/// <list type="bullet">
		///		<item>
		///			<term><c>"0"</c></term>
		///			<description>Sensor fusion is disabled</description>
		///		</item>
		///		<item>
		///			<term><c>"1"</c></term>
		///			<description>Sensor fusion is enabled for all controllers that lack sensors</description>
		///		</item>
		/// </list>
		/// </para>
		/// <para>
		/// Or the hint can be a comma separated list of USB VID/PID pairs in hexadecimal form, e.g. <c>"0xAAAA/0xBBBB,0xCCCC/0xDDDD"</c>.
		/// </para>
		/// <para>
		/// The hint can also take the form of <c>"@file"</c>, in which case the named <c>file</c> will be loaded and interpreted as the value of the hint.
		/// </para>
		/// <para>
		/// This hint should be set before <see cref="">a gamepad is opened</see>.
		/// </para>
		/// </remarks>
		public static Hint SensorFusion { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new("SDL_GAMECONTROLLER_SENSOR_FUSION"); }

		/// <summary>
		/// Gets a hint that overrides the automatic controller type detection
		/// </summary>
		/// <value>
		/// A hint that overrides the automatic controller type detection
		/// </value>
		/// <remarks>
		/// <para>
		/// The hint should be comma separated entries, in the form: <c>"VID/PID=type"</c>, where the <c>VID</c> and <c>PID</c> should be hexadecimal with exactly 4 digits, e.g. <c>0x00fd</c>.
		/// </para>
		/// <para>
		/// This hint affects what low level protocol is used with the HIDAPI driver.
		/// </para>
		/// <para>
		/// The <c>type</c> can be set to the following values:
		/// <list type="bullet">
		///		<item><description><c>Xbox360</c></description></item>
		///		<item><description><c>XboxOne</c></description></item>
		///		<item><description><c>PS3</c></description></item>
		///		<item><description><c>PS4</c></description></item>
		///		<item><description><c>PS5</c></description></item>
		///		<item><description><c>SwitchPro</c></description></item>
		/// </list>
		/// </para>
		/// <para>
		/// This hint should be set before <see cref="Sdl(Sdl.BuildAction?)">SDL is initialized</see>.
		/// </para>
		/// </remarks>
		public static Hint Type { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new("SDL_GAMECONTROLLERTYPE"); }
	}

	/// <summary>
	/// SDL_HINT_GDK_*
	/// </summary>
	public static class Gdk
	{
		/// <summary>
		/// SDL_HINT_GDK_TEXTINPUT_*
		/// </summary>
		public static class TextInput
		{
			/// <summary>
			/// Gets a hint that sets the default text of the TextInput window on GDK platforms
			/// </summary>
			/// <value>
			/// A hint that sets the default text of the TextInput window on GDK platforms
			/// </value>
			/// <remarks>
			/// <para>
			/// This hint is available only if <see cref="SDL_GDK_TEXTINPUT"/> defined.
			/// </para>
			/// <para>
			/// This hint should be set before calling <see cref="SDL_StartTextInput"/>().
			/// </para>
			/// </remarks>
			public static Hint DefaultText { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new("SDL_GDK_TEXTINPUT_DEFAULT_TEXT"); }

			/// <summary>
			/// Gets a hint that sets the description of the TextInput window on GDK platforms
			/// </summary>
			/// <value>
			/// A hint that sets the description of the TextInput window on GDK platforms
			/// </value>
			/// <remarks>
			/// <para>
			/// This hint is available only if <see cref="SDL_GDK_TEXTINPUT"/> defined.
			/// </para>
			/// <para>
			/// This hint should be set before calling <see cref="SDL_StartTextInput"/>().
			/// </para>
			/// </remarks>
			public static Hint Description { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new("SDL_GDK_TEXTINPUT_DESCRIPTION"); }

			/// <summary>
			/// Gets a hint that sets the maximum input length of the TextInput window on GDK platforms
			/// </summary>
			/// <value>
			/// A hint that sets the maximum input length of the TextInput window on GDK platforms
			/// </value>
			/// <remarks>
			/// <para>
			/// The value must be a stringified integer, for example <c>"10"</c> to allow for up to 10 characters of text input.
			/// </para>
			/// <para>
			/// This hint is available only if <see cref="SDL_GDK_TEXTINPUT"/> defined.
			/// </para>
			/// <para>
			/// This hint should be set before calling <see cref="SDL_StartTextInput"/>().
			/// </para>
			/// </remarks>
			public static Hint MaxLength { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new("SDL_GDK_TEXTINPUT_MAX_LENGTH"); }

			/// <summary>
			/// Gets a hint that sets the input scope of the TextInput window on GDK platforms
			/// </summary>
			/// <value>
			/// A hint that sets the input scope of the TextInput window on GDK platforms
			/// </value>
			/// <remarks>
			/// <para>
			/// </para>
			/// Set this hint to change the <c>XGameUiTextEntryInputScope</c> value that will be passed to the window creation function.
			/// The value must be a stringified integer, for example <c>"0"</c> for <c>XGameUiTextEntryInputScope::Default</c>.
			/// <para>
			/// This hint is available only if <see cref="SDL_GDK_TEXTINPUT"/> defined.
			/// </para>
			/// <para>
			/// This hint should be set before calling <see cref="SDL_StartTextInput"/>().
			/// </para>
			/// </remarks>
			public static Hint Scope { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new("SDL_GDK_TEXTINPUT_SCOPE"); }

			/// <summary>
			/// Gets a hint that sets the title of the TextInput window on GDK platforms
			/// </summary>
			/// <value>
			/// A hint that sets the title of the TextInput window on GDK platforms
			/// </value>
			/// <remarks>
			/// <para>
			/// This hint is available only if <see cref="SDL_GDK_TEXTINPUT"/> defined.
			/// </para>
			/// <para>
			/// This hint should be set before calling <see cref="SDL_StartTextInput"/>().
			/// </para>
			/// </remarks>
			public static Hint Title { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new("SDL_GDK_TEXTINPUT_TITLE"); }
		}
	}

	/// <summary>
	/// SDL_HINT_GPU_*
	/// </summary>
	public static class Gpu
	{
		/// <summary>
		/// Gets a hint that specifies a GPU backend to use
		/// </summary>
		/// <value>
		/// A hint that specifies a GPU backend to use
		/// </value>
		/// <remarks>
		/// <para>
		/// By default, SDL will try all available GPU backends in a reasonable order until it finds one that can work,
		/// but this hint allows the app or user to force a specific target, such as <c>"direct3d11"</c> if, say, your hardware supports D3D12 but want to try using D3D11 instead.
		/// </para>
		/// <para>
		/// This hint should be set before any GPU functions are called.
		/// </para>
		/// </remarks>
		public static Hint Driver { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new("SDL_GPU_DRIVER"); }
	}

	// TODO

	/// <summary>
	/// SDL_HINT_JOYSTICK_*
	/// </summary>
	public static class Joystick
	{
		// TODO

		/// <summary>
		/// Gets a hint controlling whether a separate thread should be used for handling joystick detection and raw input messages on Windows
		/// </summary>
		/// <value>
		/// A hint controlling whether a separate thread should be used for handling joystick detection and raw input messages on Windows
		/// </value>
		/// <remarks>
		/// <para>
		/// The hint can be set to the following values:
		/// <list type="bullet">
		///		<item>
		///			<term><c>"0"</c></term>
		///			<description>A separate thread is not used</description>
		///		</item>
		///		<item>
		///			<term><c>"1"</c></term>
		///			<description>A separate thread is used for handling raw input messages (default)</description>
		///		</item>
		/// </list>
		/// </para>
		/// <para>
		/// This hint should be set before <see cref="Sdl(Sdl.BuildAction?)">SDL is initialized</see>.
		/// </para>
		/// </remarks>
		public static Hint Thread { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new("SDL_JOYSTICK_THREAD"); }
	}

	// TODO

	/// <summary>
	/// Gets a hint that request <see cref="AppBase.OnIterate(Sdl)"/> to be called at a specific rate
	/// </summary>
	/// <value>
	/// A hint that request <see cref="AppBase.OnIterate(Sdl)"/> to be called at a specific rate
	/// </value>
	/// <remarks>
	/// <para>
	/// If this hint is set to a number, it represents Hz, so <c>"60"</c> means try to iterate 60 times per second.
	/// <c>"0"</c> means to iterate as fast as possible.
	/// Negative values are illegal, but reserved, in case they are useful in a future revision of SDL.
	/// </para>
	/// <para>
	/// There are other string values that have special meaning.
	/// If set to <c>"waitevent"</c>, <see cref="AppBase.OnIterate(Sdl)"/> will not be called until new event(s) have arrived (and been processed by <see cref="AppBase.OnEvent(Sdl, ref readonly Events.Event)"/>).
	/// This can be useful for apps that are completely idle except in response to input.
	/// </para>
	/// <para>
	/// This hint defaults to 0, and specifying <c><see langword="null"/></c> for the hint's value will restore the default.
	/// </para>
	/// <para>
	/// This hint can be set anytime and is allowed to be changed at anytime.
	/// </para>
	/// </remarks>
	public static Hint MainCallbackRate { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new("SDL_MAIN_CALLBACK_RATE"); }

	// TODO
}
