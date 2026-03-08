using System;
using System.Diagnostics.CodeAnalysis;

namespace Sdl3Sharp.Video.Windowing;

/// <summary>
/// Provides extension methods for <see cref="WindowSurfaceVSync"/>
/// </summary>
public static class WindowSurfaceVSyncExtensions
{
	extension(WindowSurfaceVSync)
	{
		/// <summary>
		/// Creates a <see cref="WindowSurfaceVSync"/> from a given interval
		/// </summary>
		/// <param name="interval">The interval in which the <see cref="Window.Surface"/> of the <see cref="Window"/> should be synchronized, in number of vertical refreshes</param>
		/// <returns>A <see cref="WindowSurfaceVSync"/> value representing the given interval</returns>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="interval"/> is less than <see cref="WindowSurfaceVSync.Disabled"/> (<c>0</c>)</exception>
		/// <exception cref="ArgumentException"><paramref name="interval"/> is equal to <see cref="WindowSurfaceVSync.Adaptive"/> (<c>-1</c>) or <see cref="WindowSurfaceVSync.Disabled"/> (<c>0</c>)</exception>
		/// <remarks>
		/// <para>
		/// If <paramref name="interval"/> is <c>1</c> the <see cref="Window.Surface"/> of the <see cref="Window"/> will be synchronized with <em>every</em> vertical refresh,
		/// if <paramref name="interval"/> is <c>2</c> it will be synchronized with <em>every second</em> vertical refresh, and so on.
		/// </para>
		/// <para>
		/// If you want to disable vertical synchronization, use <see cref="WindowSurfaceVSync.Disabled"/> instead.
		/// If you want to enable adaptive vertical synchronization, use <see cref="WindowSurfaceVSync.Adaptive"/> instead.
		/// </para>
		/// </remarks>
		public static WindowSurfaceVSync Interval(int interval)
		{
			switch (interval)
			{
				case < (int)WindowSurfaceVSync.Adaptive:
					failIntervalArgumentOutOfRange();
					break;

				case (int)WindowSurfaceVSync.Adaptive or (int)WindowSurfaceVSync.Disabled:
					failIntervalArgumentIsAdaptiveOrDisabled();
					break;
			}
			return unchecked((WindowSurfaceVSync)interval);

			[DoesNotReturn]
			static void failIntervalArgumentOutOfRange() => throw new ArgumentOutOfRangeException(nameof(interval), $"{nameof(interval)} must be greater than {(int)WindowSurfaceVSync.Disabled}");

			[DoesNotReturn]
			static void failIntervalArgumentIsAdaptiveOrDisabled() => throw new ArgumentException($"{nameof(interval)} must not be equal to {nameof(WindowSurfaceVSync.Adaptive)} or {nameof(WindowSurfaceVSync.Disabled)}. Please use the defined enum members for that.", nameof(interval));
		}
	}
}
