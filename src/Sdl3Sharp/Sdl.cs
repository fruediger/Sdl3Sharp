using Sdl3Sharp.Internal;
using Sdl3Sharp.IO;
using Sdl3Sharp.Utilities;
using Sdl3Sharp.Video.Windowing;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices.Marshalling;

namespace Sdl3Sharp;

/// <summary>
/// Represents the lifetime of SDL
/// </summary>
/// <remarks>
/// <para>
/// You must create an instance of <see cref="Sdl"/> in order to use most of the API. Creating an instance of <see cref="Sdl"/> initializes SDL (while <see cref="Dispose">disposing</see> it shuts down SDL).
/// </para>
/// <para>
/// There can be only a single instance of <see cref="Sdl"/> at a time!
/// </para>
/// </remarks>
public sealed partial class Sdl : IDisposable
{
	private enum LifetimeState { Uninitialized = default, Initializing, BeforeRun, Running, AfterRun, Disposing, Disposed }

	private static SimpleSpinYieldLock mLock = new();
	private static volatile bool mSdlExists = false;

	private volatile LifetimeState mLifetimeState;
	private AppBase? mRunningApp;

	/// <summary>
	/// Creates a new <see cref="Sdl"/> instance and initializes SDL
	/// </summary>
	/// <param name="buildAction">A <see cref="BuildAction"/> that is performed right before initializing SDL. Use the provided <see cref="Builder"/> argument to perfom some preliminaries before SDL gets initialized.</param>
	/// <remarks>
	/// <para>
	/// Creating a <see cref="Sdl"/> instance initializes SDL (while <see cref="Dispose">disposing</see> it quits SDL).
	/// </para>
	/// <para>
	/// There can be only a single instance of <see cref="Sdl"/> at a time! Trying to create another one, while one already exist, results in an <see cref="InvalidOperationException"/>.
	/// </para>
	/// <para>
	/// The file I/O (for example: <see cref="FileStream"/>) and threading (<see cref="SDL_CreateThread"/>) subsystems are initialized by default.
	/// Message boxes (<see cref="MessageBox.TryShowSimple(MessageBoxFlags, string, string, Window?)"/> and <see cref="MessageBox.TryShow(out int)"/>) also attempt to work without initializing the <see cref="SubSystems.Video">video sub system</see>,
	/// in hopes of being useful in showing an error dialog even before SDL initializes correclty.
	/// Logging (such as <see cref="Log.Info(string)"/>) works without initialization, too.
	/// </para>
	/// <para>
	/// You must specifically initialize other subsystems (either by using <see cref="Builder.InitializeSubSystems(SubSystems)"/> or by using <see cref="InitializeSubSystems(SubSystems)"/>), if you use them in your application.
	/// </para>
	/// <para>
	/// Consider reporting some basic metadata about your application inside the <paramref name="buildAction"/>, by using one of the <c>*SetMetadata</c> methods.
	/// </para>
	/// <para>
	/// In contrast to most of the remaining API which uses the <c>Try</c>-method pattern, this constructor intentionally fails by throwing an exception.
	/// If you want to handle failures wrap the call to this constructor in a <c><see langword="try"/></c>-block,
	/// and check <see cref="Error.TryGet(out string?)"/> for more information when <c><see langword="catch"/></c>ing a <see cref="SdlException"/>.
	/// </para>
	/// </remarks>
	/// <exception cref="InvalidOperationException">There already exists an instance of <see cref="Sdl"/></exception>
	/// <exception cref="SdlException">Couldn't initialize SDL (check <see cref="Error.TryGet(out string?)"/> for more information)</exception>
	public Sdl(BuildAction? buildAction = default)
	{
		mLock.Enter(0);
		try
		{
			if (mSdlExists)
			{
				failSdlAlreadyExists();
			}

			mSdlExists = true;

			mLifetimeState = LifetimeState.Initializing;
			mRunningApp = null;

			var subSystems = SubSystems.None;

			buildAction?.Invoke(new(this, ref subSystems));

			if (!SDL_Init(subSystems))
			{
				mLifetimeState = LifetimeState.Disposed;
				mSdlExists = false;

				failCouldNotInitializeSDL();
			}

			mLifetimeState = LifetimeState.BeforeRun;
		}
		finally
		{
			mLock.Exit(0);
		}

		[DoesNotReturn]
		static void failSdlAlreadyExists() => throw new InvalidOperationException($"There can be at most a single instance of {nameof(Sdl)} at a time. An instance of {nameof(Sdl)} already exists.");

		[DoesNotReturn]
		static void failCouldNotInitializeSDL() => throw new SdlException("SDL could not be initialized");
	}

	/// <inheritdoc/>
	/// <inheritdoc cref="DisposeImpl"/>
	~Sdl()
	{
		mLifetimeState = LifetimeState.Disposing;

		DisposeImpl();
	}

	/// <summary>
	/// Gets the global <see cref="Properties">group</see> of SDL properties
	/// </summary>
	/// <value>
	/// The global <see cref="Properties">group</see> of SDL properties, if those could get successfully retrieved; otherwise, <c><see langword="null"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)
	/// </value>
	public static Properties? GlobalProperties => SDL_GetGlobalProperties() switch
	{
		0 => null,
		var id => Properties.GetOrCreate(sdl: null, id)
	};
	
	/// <summary>
	/// Gets the revision string of the currently loaded native SDL library
	/// </summary>
	/// <value>
	/// The revision string of the currently loaded native SDL library
	/// </value>
	/// <remarks>
	/// <para>
	/// The revision is arbitrary string (a hash value) uniquely identifying the exact revision of the SDL library in use, and is only useful in comparing against other revisions. It is <em>NOT</em> an incrementing number.
	/// </para>
	/// <para>
	/// If SDL wasn't built from a git repository with the appropriate tools, this will return an empty string.
	/// </para>
	/// <para>
	/// You shouldn't use this value for anything but logging it for debugging purposes. The string is not intended to be reliable in any way.
	/// </para>
	/// </remarks>
	public static string? Revision
	{
		get
		{
			unsafe
			{
				return Utf8StringMarshaller.ConvertToManaged(SDL_GetRevision());
			}
		}
	}

	/// <summary>
	/// Gets the version of the currently loaded native SDL library
	/// </summary>
	/// <value>
	/// The version of the currently loaded native SDL library
	/// </value>
	public static Version Version => SDL_GetVersion();

	/// <summary>
	/// Gets the <see cref="Environment">set of environment variables</see> for the current process
	/// </summary>
	/// <value>
	/// The <see cref="Environment">set of environment variables</see> for the current process, if those could get successfully retrieved; otherwise, <c><see langword="null"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)
	/// </value>
	/// <remarks>
	/// <para>
	/// This <see cref="Environment">set of environment variables</see> is initialized at application start and is not affected by external calls to modify process environments (e.g. setenv() or unsetenv()) after that point.
	/// </para>
	/// <para>
	/// To modify this <see cref="Environment">set of environment variables</see> use <see cref="Utilities.Environment.TrySetVariable(string, string, bool)"/> or <see cref="Utilities.Environment.TryUnsetVariable(string)"/>.
	/// Changes made in this way will not persist outside of SDL, and especially not after <see cref="Dispose">SDL is shut down</see>.
	/// </para>
	/// <para>
	/// If you want for changes to persist in the C runtime environment after <see cref="Dispose">SDL is shut down</see>, use <see cref="Utilities.Environment.TrySetProcessVariableUnsafe(string, string, bool)"/> or <see cref="Utilities.Environment.TryUnsetProcessVariableUnsafe(string)"/>.
	/// </para>
	/// </remarks>
	/// <inheritdoc cref="Utilities.Environment.Environment(Sdl, Utilities.Environment.SDL_Environment*)"/>
	public Utilities.Environment? ProcessEnvironment
	{
		get
		{
			unsafe
			{
				if (Utilities.Environment.SDL_GetEnvironment() is var environementPtr && environementPtr is not null)
				{
					return new(this, environementPtr);
				}

				return null;
			}
		}
	}

	/// <summary>
	/// Disposes the <see cref="Sdl"/> instance and quits SDL
	/// </summary>
	/// <exception cref="InvalidOperationException">The <see cref="Sdl"/> instance is currently running. You cannot dispose an running <see cref="Sdl"/>.</exception>
	/// <inheritdoc cref="DisposeImpl"/>
	public void Dispose()
	{
		mLock.Enter(0);
		try
		{
			switch (mLifetimeState)
			{
				case LifetimeState.Running: failSdlIsRunning(); break;
				case LifetimeState.Disposing or LifetimeState.Disposed: return;
			}

			mLifetimeState = LifetimeState.Disposing;
		}
		finally
		{
			mLock.Exit(0);
		}

		GC.SuppressFinalize(this);
		DisposeImpl();

		[DoesNotReturn]
		static void failSdlIsRunning() => throw new InvalidOperationException($"Cannot dispose a running instance of {nameof(Sdl)}");
	}

	/// <exception cref="AggregateException">One or more registered <see cref="IDisposeReceiver"/> threw an exception during the call to their <see cref="IDisposeReceiver.DisposeFromSdl(Sdl)"/></exception>
	private void DisposeImpl()
	{
		// we don't lock and we don't check for the LifetimeState here,
		// since all calls to this come from either a already checked state or from the finalizer

		var exceptions = mRegisteredDisposeReceivers.Count switch
		{
			var count when count is > 0 => new List<Exception>(count),
			_ => null
		};

		try
		{
			foreach ((var reference, _) in mRegisteredDisposeReceivers)
			{
				if (reference?.TryGetTarget(out var disposeReceiver) is true)
				{
					try
					{
						disposeReceiver.DisposeFromSdl(this);
					}
					catch (Exception exception)
					{
						exceptions!.Add(exception);
					}
				}
			}

			if (exceptions?.Count is > 0)
			{
				throw new AggregateException(exceptions);
			}
		}
		finally
		{
			try
			{
				try
				{
					exceptions?.Clear();

					mRegisteredDisposeReceivers.Clear();				

					// Call additional dipose composites	
					DisposeEventHandlers();
					DisposeEventQueue();
				}
				finally
				{
					SDL_Quit();
				}
			}
			finally
			{
				mLifetimeState = LifetimeState.Disposed;

				mSdlExists = false;
			}
		}
	}

	/// <summary>
	/// Gets a mask of currently initialized <see cref="SubSystems"/>
	/// </summary>
	/// <param name="subSystems">The <see cref="SubSystems"/> to check for</param>
	/// <returns>
	/// A <see cref="SubSystems"/> representing all of the currently initialized <see cref="SubSystems"/>, if <paramref name="subSystems"/> is <see cref="SubSystems.None"/>;
	/// otherwise, a <see cref="SubSystems"/> representing the currently initialized <see cref="SubSystems"/> from the ones specified in the given <paramref name="subSystems"/>
	/// </returns>
	/// <remarks>
	/// <para>
	/// If you just want to check the initialization state <see cref="SubSystems"/>, you can use the <see cref="SubSystemsExtensions.get_IsInitialized(SubSystems)"/> extension property.
	/// </para>
	/// </remarks>
#pragma warning disable IDE0079
#pragma warning disable CA1822 // this is intentionally an instance method
	public SubSystems GetInitializedSubSystems(SubSystems subSystems) => SDL_WasInit(subSystems);
#pragma warning restore CA1822
#pragma warning restore IDE0079

	/// <summary>
	/// Gets the currently initialized <see cref="SubSystems"/>
	/// </summary>
	/// <returns>A <see cref="SubSystems"/> representing all of the currently initialized <see cref="SubSystems"/></returns>
	/// <remarks>
	/// <para>
	/// If you just want to check the initialization state <see cref="SubSystems"/>, you can use the <see cref="SubSystemsExtensions.get_IsInitialized(SubSystems)"/> extension property.
	/// </para>
	/// </remarks>
#pragma warning disable IDE0079
#pragma warning disable CA1822 // this is intentionally an instance method
	public SubSystems GetInitializedSubSystems() => SDL_WasInit(SubSystems.None);
#pragma warning restore CA1822
#pragma warning restore IDE0079

	/// <summary>
	/// Get metadata about your app
	/// </summary>
	/// <param name="name">The name of the metadata</param>
	/// <returns>The current value of the metadata, if it's set; otherwise, the default value for the metadata, or <c><see langword="null"/></c>, if it has no default value</returns>
	/// <remarks>
	/// <para>
	/// This returns metadata previously set using one of the <c>*SetMetadata</c> methods on the <see cref="Builder"/> that was used to create the current <see cref="Sdl"/> instance.
	/// </para>
	/// <para>
	/// See <see cref="Metadata"/> for a overview over the available metadata properties and their meanings.
	/// </para>
	/// </remarks>
#pragma warning disable IDE0079
#pragma warning disable CA1822 // this is intentionally an instance method
	public string? GetMetadata(string name)
#pragma warning restore CA1822
#pragma warning restore IDE0079
	{
		unsafe
		{
			var nameUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(name);

			try
			{
				return Utf8StringMarshaller.ConvertToManaged(SDL_GetAppMetadataProperty(nameUtf8));
			}
			finally
			{
				Utf8StringMarshaller.Free(nameUtf8);
			}
		}
	}

	/// <summary>
	/// Initializes certain <see cref="SubSystems">sub systems</see>
	/// </summary>
	/// <param name="subSystems"><see cref="SubSystems">Sub systems</see> to initialize</param>
	/// <returns>A <see cref="SubSystemInit"/> that handles the lifetime of the sub systems</returns>
	/// <remarks>
	/// <para>
	/// <see cref="SubSystems"/> are reference counted through <see cref="SubSystemInit"/>s.
	/// This method increases the reference count for the <paramref name="subSystems"/>.
	/// </para>
	/// <para>
	/// <see cref="SubSystems"/> can get shut down through <see cref="SubSystemInit.Dispose()"/> or <see cref="Dispose"/>.
	/// </para>
	/// <para>
	/// In contrast to most of the remaining API which uses the <c>Try</c>-method pattern, this method intentionally fails by throwing an exception.
	/// If you want to handle failures wrap the call to this method in a <c><see langword="try"/></c>-block,
	/// and check <see cref="Error.TryGet(out string?)"/> for more information when <c><see langword="catch"/></c>ing a <see cref="SdlException"/>.
	/// </para>
	/// </remarks>
	/// <inheritdoc cref="SubSystemInit(Sdl, SubSystems)"/>
	public SubSystemInit InitializeSubSystems(SubSystems subSystems) => new(this, subSystems);

	/// <param name="app">The <see cref="AppBase"/> to execute</param>
	/// <param name="args">A collection of arguments to pass to <see cref="AppBase.OnInitialize(Sdl, string[])"/></param>
	/// <inheritdoc cref="RunImpl{TArgumentsEnumerator}(AppBase, TArgumentsEnumerator, int)"/>
	public int Run(AppBase app, ReadOnlySpan<string> args)
		=> RunImpl(app, args.GetSpanEnumerator(), args.Length);

	/// <typeparam name="TArguments">The source type of collection of arguments to pass to <see cref="AppBase.OnInitialize(Sdl, string[])"/></typeparam>
	/// <param name="app">The <see cref="AppBase"/> to execute</param>
	/// <param name="args">A collection of arguments to pass to <see cref="AppBase.OnInitialize(Sdl, string[])"/></param>
	/// <inheritdoc cref="RunImpl{TArgumentsEnumerator}(AppBase, TArgumentsEnumerator, int)"/>
	public int Run<TArguments>(AppBase app, [AllowNull] in TArguments? args)
		where TArguments : IReadOnlyCollection<string>, allows ref struct
	{
		var (argumentsEnumerator, argumentsCount) = args is not null
			? (args.GetEnumerator(), args.Count)
			: (null, 0);

		return RunImpl(app, argumentsEnumerator, argumentsCount);
	}
	
	/// <summary>
	/// Executes the provided <paramref name="app"/> and <see cref="Dispose">deinitializes the current <see cref="Sdl"/> instance</see> afterwards
	/// </summary>
	/// <returns>A standard Unix main return value (a non-zero value, if there was a failure; otherwise, <c>0</c>)</returns>
	/// <exception cref="ArgumentNullException"><paramref name="app"/> is <c><see langword="null"/></c></exception>
	/// <exception cref="InvalidOperationException">The current <see cref="Sdl"/> instance is currently already executing an <see cref="AppBase"/>. You can only execute a single <see cref="AppBase"/> at a time.</exception>
	/// <exception cref="ObjectDisposedException">The current <see cref="Sdl"/> instance is already disposed</exception>
	/// <remarks>
	/// <para>
	/// It's important to notice, that this method will always <see cref="Dispose">dispose</see> the current <see cref="Sdl"/> instance after it finishes executing the <paramref name="app"/>!
	/// This is unavoidable!
	/// </para>
	/// <para>
	/// You might be able to reuse the provided <paramref name="app"/> after this method returns, but you can't reuse this instance of <see cref="Sdl"/> (as SDL will be shut down).
	/// If you want to use SDL after a call to this method, you must create a new <see cref="Sdl"/> instance and use that one instead.
	/// </para>
	/// </remarks>
	private int RunImpl<TArgumentsEnumerator>(AppBase app, [AllowNull] TArgumentsEnumerator? argumentsEnumerator, int argumentsCount)
		where TArgumentsEnumerator : IEnumerator<string>, allows ref struct
	{
		if (app is null)
		{
			failAppArgumentNull();
		}

		mLock.Enter(0);
		try
		{
			switch (mLifetimeState)
			{
				case LifetimeState.Running: failSdlAlreadyRunning(); break;
				case not LifetimeState.BeforeRun: failSdlDisposed(); break;
			}

			mLifetimeState = LifetimeState.Running;
		}
		finally
		{
			mLock.Exit(0);
		}

		unsafe
		{
			int argc = 0;
			byte** argv;

			if (argumentsEnumerator is not null && argumentsCount is > 0)
			{
				argv = (byte**)NativeMemory.Malloc(unchecked((nuint)argumentsCount * (nuint)sizeof(byte*)));

				if (argv is not null)
				{
					using (argumentsEnumerator)
					{
						while (argumentsEnumerator.MoveNext())
						{
							argv[argc++] = Utf8StringMarshaller.ConvertToUnmanaged(argumentsEnumerator.Current);
						}
					}
				}
			}
			else
			{
				argv = null;
			}

			try
			{
				return RunImpl(app, argc, argv);
			}
			finally
			{
				if (argv is not null)
				{
					while (argc is > 0)
					{
						Utf8StringMarshaller.Free(argv[--argc]);
					}

					NativeMemory.Free(argv);
				}

				mLifetimeState = LifetimeState.AfterRun;
				Dispose();
			}
		}

		[DoesNotReturn]
		static void failAppArgumentNull() => throw new ArgumentNullException(nameof(app));

		[DoesNotReturn]
		static void failSdlAlreadyRunning() => throw new InvalidOperationException($"The {nameof(Sdl)} instance is already running");

		[DoesNotReturn]
		static void failSdlDisposed() => throw new ObjectDisposedException(nameof(Sdl));
	}
}
