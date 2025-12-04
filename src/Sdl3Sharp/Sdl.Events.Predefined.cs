using Sdl3Sharp.Events;
using Sdl3Sharp.Internal.Events;

namespace Sdl3Sharp;

partial class Sdl
{
	private void DisposeEventHandlers()
	{
		mDidEnterBackgroundHandler?.Dispose();
		mDidEnterBackgroundHandler = null;

		mDidEnterForegroundHandler?.Dispose();
		mDidEnterForegroundHandler = null;

		mLocaleChangedHandler?.Dispose();
		mLocaleChangedHandler = null;

		mLowMemoryHandler?.Dispose();
		mLowMemoryHandler = null;

		mQuitHandler?.Dispose();
		mQuitHandler = null;

		mSystemThemeChangedHandler?.Dispose();
		mSystemThemeChangedHandler = null;

		mTerminatingHandler?.Dispose();
		mTerminatingHandler = null;

		mWillEnterBackgroundHandler?.Dispose();
		mWillEnterBackgroundHandler = null;

		mWillEnterForegroundHandler?.Dispose();
		mWillEnterForegroundHandler = null;
	}

	private EventWatchHandler<Sdl, Event>? mDidEnterBackgroundHandler = null;

	/// <summary>
	/// Occurs when the application has entered the background
	/// </summary>
	/// <remarks>
	/// <para>
	/// The application may not get any CPU for some time.
	/// </para>
	/// <para>
	/// Raised on iOS in <c>applicationDidEnterBackground()</c>.
	/// Raised on Android in <c>onPause()</c>.
	/// </para>
	/// </remarks>
	public event EventHandler<Sdl, Event>? DidEnterBackground
	{
		add
		{
			if (value is not null)
			{
				(mDidEnterBackgroundHandler ??= new(this, EventType.DidEnterBackground)).EventHandler += value;
			}
		}

		remove
		{
			if (value is not null && mDidEnterBackgroundHandler is not null && (mDidEnterBackgroundHandler.EventHandler -= value) is null)
			{
				mDidEnterBackgroundHandler.Dispose();
				mDidEnterBackgroundHandler = null;
			}
		}
	}

	private EventWatchHandler<Sdl, Event>? mDidEnterForegroundHandler = null;

	/// <summary>
	/// Occurs when the application has entered the foreground
	/// </summary>
	/// <remarks>
	/// <para>
	/// The application is now interactive.
	/// </para>
	/// <para>
	/// Raised on iOS in <c>applicationDidBecomeActive()</c>.
	/// Raised on Android in <c>onResume()</c>.
	/// </para>
	/// </remarks>
	public event EventHandler<Sdl, Event>? DidEnterForeground
	{
		add
		{
			if (value is not null)
			{
				(mDidEnterForegroundHandler ??= new(this, EventType.DidEnterForeground)).EventHandler += value;
			}
		}

		remove
		{
			if (value is not null && mDidEnterForegroundHandler is not null && (mDidEnterForegroundHandler.EventHandler -= value) is null)
			{
				mDidEnterForegroundHandler.Dispose();
				mDidEnterForegroundHandler = null;
			}
		}
	}

	private EventWatchHandler<Sdl, Event>? mLocaleChangedHandler = null;

	/// <summary>
	/// Occurs when the user's locale preferences have changed
	/// </summary>
	public event EventHandler<Sdl, Event>? LocaleChanged
	{
		add
		{
			if (value is not null)
			{
				(mLocaleChangedHandler ??= new(this, EventType.LocaleChanged)).EventHandler += value;
			}
		}

		remove
		{
			if (value is not null && mLocaleChangedHandler is not null && (mLocaleChangedHandler.EventHandler -= value) is null)
			{
				mLocaleChangedHandler.Dispose();
				mLocaleChangedHandler = null;
			}
		}
	}

	private EventWatchHandler<Sdl, Event>? mLowMemoryHandler = null;

	/// <summary>
	/// Occurs when the application is running low on memory
	/// </summary>
	/// <remarks>
	/// <para>
	/// Free some memory if possible.
	/// </para>
	/// <para>
	/// Raised on iOS in <c>applicationDidReceiveMemoryWarning()</c>.
	/// Raised on Android in <c>onTrimMemory()</c>.
	/// </para>
	/// </remarks>
	public event EventHandler<Sdl, Event>? LowMemory
	{
		add
		{
			if (value is not null)
			{
				(mLowMemoryHandler ??= new(this, EventType.LowMemory)).EventHandler += value;
			}
		}

		remove
		{
			if (value is not null && mLowMemoryHandler is not null && (mLowMemoryHandler.EventHandler -= value) is null)
			{
				mLowMemoryHandler.Dispose();
				mLowMemoryHandler = null;
			}
		}
	}

	private EventWatchHandler<Sdl, QuitEvent>? mQuitHandler = null;

	/// <summary>
	/// Occurs when the application "requests" to quit
	/// </summary>
	public event EventHandler<Sdl, QuitEvent>? Quit
	{
		add
		{
			if (value is not null)
			{
				(mQuitHandler ??= new(this, EventType.Quit)).EventHandler += value;
			}
		}

		remove
		{
			if (value is not null && mQuitHandler is not null && (mQuitHandler.EventHandler -= value) is null)
			{
				mQuitHandler.Dispose();
				mQuitHandler = null;
			}
		}
	}

	private EventWatchHandler<Sdl, Event>? mSystemThemeChangedHandler = null;

	/// <summary>
	/// Occurs when the system theme has changed
	/// </summary>
	public event EventHandler<Sdl, Event>? SystemThemeChanged
	{
		add
		{
			if (value is not null)
			{
				(mSystemThemeChangedHandler ??= new(this, EventType.SystemThemeChanged)).EventHandler += value;
			}
		}

		remove
		{
			if (value is not null && mSystemThemeChangedHandler is not null && (mSystemThemeChangedHandler.EventHandler -= value) is null)
			{
				mSystemThemeChangedHandler.Dispose();
				mSystemThemeChangedHandler = null;
			}
		}
	}

	private EventWatchHandler<Sdl, Event>? mTerminatingHandler = null;

	/// <summary>
	/// Occurs when the application is being terminated by the OS
	/// </summary>
	/// <remarks>
	/// <para>
	/// Raised on iOS in <c>applicationWillTerminate()</c>.
	/// Raised on Android in <c>onDestroy()</c>.
	/// </para>
	/// </remarks>
	public event EventHandler<Sdl, Event>? Terminating
	{
		add
		{
			if (value is not null)
			{
				(mTerminatingHandler ??= new(this, EventType.Terminating)).EventHandler += value;
			}
		}

		remove
		{
			if (value is not null && mTerminatingHandler is not null && (mTerminatingHandler.EventHandler -= value) is null)
			{
				mTerminatingHandler.Dispose();
				mTerminatingHandler = null;
			}
		}
	}

	private EventWatchHandler<Sdl, Event>? mWillEnterBackgroundHandler = null;

	/// <summary>
	/// Occurs when the application is about to enter the background
	/// </summary>
	/// <remarks>
	/// <para>
	/// Raised on iOS in <c>applicationWillResignActive()</c>.
	/// Raised on Android in <c>onPause()</c>.
	/// </para>
	/// </remarks>
	public event EventHandler<Sdl, Event>? WillEnterBackground
	{
		add
		{
			if (value is not null)
			{
				(mWillEnterBackgroundHandler ??= new(this, EventType.WillEnterBackground)).EventHandler += value;
			}
		}

		remove
		{
			if (value is not null && mWillEnterBackgroundHandler is not null && (mWillEnterBackgroundHandler.EventHandler -= value) is null)
			{
				mWillEnterBackgroundHandler.Dispose();
				mWillEnterBackgroundHandler = null;
			}
		}
	}

	private EventWatchHandler<Sdl, Event>? mWillEnterForegroundHandler = null;

	/// <summary>
	/// Occurs when the application is about to enter the foreground
	/// </summary>
	/// <remarks>
	/// <para>
	/// Raised on iOS in <c>applicationWillEnterForeground()</c>.
	/// Raised on Android in <c>onResume()</c>.
	/// </para>
	/// </remarks>
	public event EventHandler<Sdl, Event>? WillEnterForeground
	{
		add
		{
			if (value is not null)
			{
				(mWillEnterForegroundHandler ??= new(this, EventType.WillEnterForeground)).EventHandler += value;
			}
		}

		remove
		{
			if (value is not null && mWillEnterForegroundHandler is not null && (mWillEnterForegroundHandler.EventHandler -= value) is null)
			{
				mWillEnterForegroundHandler.Dispose();
				mWillEnterForegroundHandler = null;
			}
		}
	}
}
