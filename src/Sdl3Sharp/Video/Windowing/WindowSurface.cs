using Sdl3Sharp.Internal;
using Sdl3Sharp.Video.Drawing;
using Sdl3Sharp.Video.Rendering;
using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Video.Windowing;

/// <summary>
/// Represents the surface of a window
/// </summary>
/// <remarks>
/// <para>
/// <see cref="WindowSurface"/>s (coming from the <see cref="Window.Surface"/> property of a <see cref="Window"/>) can be used as an alternative to using a <see cref="Renderer"/> on the window.
/// </para>
/// <para>
/// Do <em>not</em> attempt to use a <see cref="WindowSurface"/> together with a <see cref="Renderer"/> on the same <see cref="Window"/>!
/// They are meant to be used mutually exclusively for the same <see cref="Window"/>.
/// </para>
/// <para>
/// For the most part <see cref="WindowSurface"/>s are not thread-safe, and most of their properties and methods should only be accessed from the main thread!
/// </para>
/// </remarks>
public sealed partial class WindowSurface : Surface
{
	private Window? mWindow;

	internal unsafe WindowSurface(Window window, SDL_Surface* surface, bool register) :
		base(surface, register)
		=> mWindow = window;

	internal bool DontDestroy { get; set; } = false;

	/// <summary>
	/// Gets a value indicating whether the surface is still valid
	/// </summary>
	/// <value>
	/// A value indicating whether the surface is still valid
	/// </value>
	/// <remarks>
	/// <para>
	/// A <see cref="WindowSurface"/> can become invalid:
	/// <list type="bullet">
	/// <item><description>If it is <see cref="Surface.Dispose()">disposed</see></description></item>
	/// <item><description>If the associated <see cref="Window"/> is resized</description></item>
	/// <item><description>If the associated <see cref="Window"/> is <see cref="Window.Dispose()">disposed</see>/destroyed</description></item>
	/// </list>
	/// In any of these cases, the <see cref="Window.Surface"/> property of the associated <see cref="Window"/> will reference a new <see cref="WindowSurface"/> instance that is valid and can be used instead of the invalid one.
	/// </para>
	/// <para>
	/// Do not attempt to use an invalid <see cref="WindowSurface"/>! You are responsible for ensuring that you only use valid <see cref="WindowSurface"/> instances.
	/// </para>
	/// </remarks>
	public bool IsValid { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get { unsafe { return Pointer is not null; } } }

	/// <summary>
	/// Gets or sets the vertical synchronization (VSync) mode or interval for the surface of a window
	/// </summary>
	/// <value>
	/// The vertical synchronization (VSync) mode or interval for the surface of a window
	/// </value>
	/// <remarks>
	/// <para>
	/// You can set the value of this property to <see cref="WindowSurfaceVSync.Disabled"/> to disable VSync,
	/// <see cref="WindowSurfaceVSync.Adaptive"/> to enable late swap tearing (adaptive VSync) if supported,
	/// or use the <see cref="WindowSurfaceVSyncExtensions.Interval(int)"/> method to specify a custom VSync interval.
	/// You can specify a custom interval of <c>1</c> to synchronize the surface with <em>every</em> vertical refresh,
	/// <c>2</c> to synchronize it with <em>every second</em> vertical refresh, and so on.
	/// </para>
	/// <para>
	/// When a window surface is newly created, the value of this property defaults to <see cref="WindowSurfaceVSync.Disabled"/>.
	/// </para>
	/// <para>
	/// This property should only be accessed from the main thread.
	/// </para>
	/// </remarks>
	/// <exception cref="SdlException">When setting or getting this property, SDL failed with an error (check <see cref="Error.TryGet(out string?)"/> for more information)</exception>
	public WindowSurfaceVSync VSync
	{
		get
		{
			unsafe
			{
				Unsafe.SkipInit(out WindowSurfaceVSync vsync);

				SdlErrorHelper.ThrowIfFailed(SDL_GetWindowSurfaceVSync(mWindow is not null ? mWindow.Pointer : null, &vsync));

				return vsync;
			}
		}
		set
		{
			unsafe
			{
				SdlErrorHelper.ThrowIfFailed(SDL_SetWindowSurfaceVSync(mWindow is not null ? mWindow.Pointer : null, value));
			}
		}
	}

	/// <summary>
	/// Gets the associated <see cref="Window"/> of the surface
	/// </summary>
	/// <value>
	/// The associated <see cref="Window"/> of the surface, that is the window for which this surface is the <see cref="Window.Surface"/> instance, or <c><see langword="null"/></c> if this surface is not <see cref="IsValid">valid</see> anymore
	/// </value>
	public Window? Window { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mWindow; }

	private protected override unsafe void Destroy(SDL_Surface* surface)
	{
		if (!DontDestroy)
		{
			SDL_DestroyWindowSurface(mWindow is not null ? mWindow.Pointer : null);
		}
	}

	/// <summary>
	/// Dispose the surface
	/// </summary>
	/// <remarks>
	/// <para>
	/// After disposing a <see cref="WindowSurface"/>, it becomes <see cref="IsValid">invalid</see>, and its associated <see cref="Window"/> will have a new <see cref="WindowSurface"/> instance as its <see cref="Window.Surface"/> that is valid and can be used instead of the disposed one.
	/// </para>
	/// <para>
	/// This method should only be called from the main thread.
	/// </para>
	/// </remarks>
	protected override void Dispose(bool disposing, bool forget)
	{
		base.Dispose(disposing, forget);

		mWindow = null;
	}

	/// <summary>Use <see cref="TryGet(SDL_Surface*, out WindowSurface?)"/> instead</summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	[OverloadResolutionPriority(-1)]
	[Obsolete(
		$"Use {nameof(TryGet)}({nameof(SDL_Surface)}*, out {nameof(WindowSurface)}?) instead.",
		error: true
	)]
	internal new unsafe static bool TryGet(SDL_Surface* surface, [NotNullWhen(true)] out Surface? result)
		=> Surface.TryGet(surface, out result);

	internal unsafe static bool TryGet(SDL_Surface* surface, [NotNullWhen(true)] out WindowSurface? result)
	{
		if (Surface.TryGet(surface, out var baseSurface)
			&& baseSurface is WindowSurface typedSurface)
		{
			result = typedSurface;
			return true;
		}

		result = null;
		return false;
	}

	/// <summary>Use <see cref="TryGetOrCreate(Window, out WindowSurface?)"/> instead</summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	[OverloadResolutionPriority(-1)]
	[Obsolete(
		$"Use {nameof(TryGetOrCreate)}({nameof(Window)}, out {nameof(WindowSurface)}?) instead.",
		error: true
	)]
	internal new unsafe static bool TryGetOrCreate(SDL_Surface* surface, [NotNullWhen(true)] out Surface? result)
		=> Surface.TryGetOrCreate(surface, out result);

	internal static bool TryGetOrCreate(Window window, [NotNullWhen(true)] out WindowSurface? result)
	{
		unsafe
		{
			if (window is null)
			{
				result = null;
				return false;
			}

			var surface = SDL_GetWindowSurface(window.Pointer);

			if (surface is null)
			{
				result = null;
				return false;
			}

			if (!TryGet(surface, out result))
			{
				// There are only two possibilities that get us here: Either there was no surface in the cache for the given pointer,
				// or the type of the existing surface in the cache was incorrect.
				// Either way, calling the constructor with register: true will overwrite any existing surface in the cache with the newly correctly typed WindowSurface.
				// This can only happen if, for some reason, we managed to not properly forget a previous surface (since they happen to share the same pointer).

				result = new(window, surface, register: true);
			}

			return true;
		}
	}

	/// <summary>
	/// Tries to copy the entirety of the window surface to the screen
	/// </summary>
	/// <returns><c><see langword="true"/></c>, if the window surface was successfully copied to the screen; otherwise, <c><see langword="false"/></c> (call <see cref="Error.TryGet(out string?)"/> for more information about the failure)</returns>
	/// <remarks>
	/// <para>
	/// This method tries to copy the present window surface's content to the screen, in order to display it as the associated <see cref="Window"/>'s visible content.
	/// To reflect any changes made to the surface, you need to call this method to update the window's content on the screen.
	/// </para>
	/// <para>
	/// This method should only be called from the main thread.
	/// </para>
	/// </remarks>
	public bool TryUpdate()
	{
		unsafe
		{
			return SDL_UpdateWindowSurface(mWindow is not null ? mWindow.Pointer : null);
		}
	}

	/// <summary>
	/// Tries to copy the specified areas of the window surface to the screen
	/// </summary>
	/// <param name="rects">The list of areas of the window surface copy</param>
	/// <returns><c><see langword="true"/></c>, if at least the specified areas of the window surface were successfully copied to the screen; otherwise, <c><see langword="false"/></c> (call <see cref="Error.TryGet(out string?)"/> for more information about the failure)</returns>
	/// <remarks>
	/// <para>
	/// This method tries to copy the present window surface's content to the screen, in order to display it as the associated <see cref="Window"/>'s visible content.
	/// To reflect any changes made to the surface, you need to call this method to update the window's content on the screen.
	/// </para>
	/// <para>
	/// Note that this method will update <em>at least</em> the specified areas, but this is only intended as an optimization;
	/// in practice, this might update more of the screen (or even the entirety of all of the screen), depending on the method used by SDL to send pixels to the system.
	/// </para>
	/// <para>
	/// This method should only be called from the main thread.
	/// </para>
	/// </remarks>
	public bool TryUpdateRects(ReadOnlySpan<Rect<int>> rects)
	{
		unsafe
		{
			fixed (Rect<int>* rectsPtr = rects)
			{
				return SDL_UpdateWindowSurfaceRects(mWindow is not null ? mWindow.Pointer : null, rectsPtr, rects.Length);
			}
		}
	}
}
