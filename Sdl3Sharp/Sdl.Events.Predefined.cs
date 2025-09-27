using Sdl3Sharp.Events;
using Sdl3Sharp.Internal.Events;

namespace Sdl3Sharp;

partial class Sdl
{
	private void DisposeEventHandlers()
	{
		if (mDidEnterBackgroundHandler is not null)
		{
			mDidEnterBackgroundHandler.Dispose();
			mDidEnterBackgroundHandler = null;
		}

		if (mDidEnterForegroundHandler is not null)
		{
			mDidEnterForegroundHandler.Dispose();
			mDidEnterForegroundHandler = null;
		}

		if (mLocaleChangedHandler is not null)
		{
			mLocaleChangedHandler.Dispose();
			mLocaleChangedHandler = null;
		}

		if (mLowMemoryHandler is not null)
		{
			mLowMemoryHandler.Dispose();
			mLowMemoryHandler = null;
		}

		if (mQuitHandler is not null)
		{
			mQuitHandler.Dispose();
			mQuitHandler = null;
		}

		if (mSystemThemeChangedHandler is not null)
		{
			mSystemThemeChangedHandler.Dispose();
			mSystemThemeChangedHandler = null;
		}

		if (mTerminatingHandler is not null)
		{
			mTerminatingHandler.Dispose();
			mTerminatingHandler = null;
		}
		
		if (mWillEnterBackgroundHandler is not null)
		{
			mWillEnterBackgroundHandler.Dispose();
			mWillEnterBackgroundHandler = null;
		}

		if (mWillEnterForegroundHandler is not null)
		{
			mWillEnterForegroundHandler.Dispose();
			mWillEnterForegroundHandler = null;
		}
	}

	private EventWatchHandlerWithEventHandler<Sdl, Event>? mDidEnterBackgroundHandler = null;

	public event ReadOnlyEventHandler<Sdl, Event>? DidEnterBackground
	{
		add
		{
			if (value is not null)
			{
				(mDidEnterBackgroundHandler ??= new(this, EventType.Application.DidEnterBackground)).EventHandler += value;
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

	private EventWatchHandlerWithEventHandler<Sdl, Event>? mDidEnterForegroundHandler = null;

	public event ReadOnlyEventHandler<Sdl, Event>? DidEnterForeground
	{
		add
		{
			if (value is not null)
			{
				(mDidEnterForegroundHandler ??= new(this, EventType.Application.DidEnterForeground)).EventHandler += value;
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

	private EventWatchHandlerWithEventHandler<Sdl, Event>? mLocaleChangedHandler = null;

	public event ReadOnlyEventHandler<Sdl, Event>? LocaleChanged
	{
		add
		{
			if (value is not null)
			{
				(mLocaleChangedHandler ??= new(this, EventType.Application.LocaleChanged)).EventHandler += value;
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

	private EventWatchHandlerWithEventHandler<Sdl, Event>? mLowMemoryHandler = null;

	public event ReadOnlyEventHandler<Sdl, Event>? LowMemory
	{
		add
		{
			if (value is not null)
			{
				(mLowMemoryHandler ??= new(this, EventType.Application.LowMemory)).EventHandler += value;
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

	private EventWatchHandlerWithEventHandler<Sdl, QuitEvent>? mQuitHandler = null;

	public event ReadOnlyEventHandler<Sdl, QuitEvent>? Quit
	{
		add
		{
			if (value is not null)
			{
				(mQuitHandler ??= new(this, EventType.Application.Quit)).EventHandler += value;
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

	private EventWatchHandlerWithEventHandler<Sdl, Event>? mSystemThemeChangedHandler = null;

	public event ReadOnlyEventHandler<Sdl, Event>? SystemThemeChanged
	{
		add
		{
			if (value is not null)
			{
				(mSystemThemeChangedHandler ??= new(this, EventType.Application.SystemThemeChanged)).EventHandler += value;
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

	private EventWatchHandlerWithEventHandler<Sdl, Event>? mTerminatingHandler = null;

	public event ReadOnlyEventHandler<Sdl, Event>? Terminating
	{
		add
		{
			if (value is not null)
			{
				(mTerminatingHandler ??= new(this, EventType.Application.Terminating)).EventHandler += value;
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

	private EventWatchHandlerWithEventHandler<Sdl, Event>? mWillEnterBackgroundHandler = null;

	public event ReadOnlyEventHandler<Sdl, Event>? WillEnterBackground
	{
		add
		{
			if (value is not null)
			{
				(mWillEnterBackgroundHandler ??= new(this, EventType.Application.WillEnterBackground)).EventHandler += value;
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

	private EventWatchHandlerWithEventHandler<Sdl, Event>? mWillEnterForegroundHandler = null;

	public event ReadOnlyEventHandler<Sdl, Event>? WillEnterForeground
	{
		add
		{
			if (value is not null)
			{
				(mWillEnterForegroundHandler ??= new(this, EventType.Application.WillEnterForeground)).EventHandler += value;
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
