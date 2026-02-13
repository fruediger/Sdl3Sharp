using System;
using System.Diagnostics.CodeAnalysis;

namespace Sdl3Sharp.Video.Rendering;

/// <summary>
/// Provides extension methods for <see cref="RendererVSync"/>
/// </summary>
public static class RendererVSyncExtensions
{
	extension(RendererVSync)
	{
		/// <summary>
		/// Creates a <see cref="RendererVSync"/> value from a given interval
		/// </summary>
		/// <param name="interval">The interval in which the present of the <see cref="IRenderer"/> should be synchronized, in number of vertical refreshes</param>
		/// <returns>A <see cref="RendererVSync"/> value representing the given interval</returns>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="interval"/> is less than <see cref="RendererVSync.Disabled"/> (<c>0</c>)</exception>
		/// <exception cref="ArgumentException"><paramref name="interval"/> is equal to <see cref="RendererVSync.Adaptive"/> (<c>-1</c>) or <see cref="RendererVSync.Disabled"/> (<c>0</c>)</exception>
		/// <remarks>
		/// <para>
		/// If <paramref name="interval"/> is <c>1</c> the present of the <see cref="IRenderer"/> will be synchronized with <em>every</em> vertical refresh,
		/// if <paramref name="interval"/> is <c>2</c> it will be synchronized with <em>every second</em> vertical refresh, and so on.
		/// </para>
		/// <para>
		/// If you want to disable vertical synchronization, use <see cref="RendererVSync.Disabled"/> instead.
		/// If you want to enable adaptive vertical synchronization, use <see cref="RendererVSync.Adaptive"/> instead.
		/// </para>
		/// </remarks>
		public static RendererVSync Interval(int interval)
		{
			switch (interval)
			{
				case < (int)RendererVSync.Adaptive:
					failIntervalArgumentOutOfRange();
					break;

				case (int)RendererVSync.Adaptive or (int)RendererVSync.Disabled:
					failIntervalArgumentIsAdaptiveOrDisabled();
					break;
			}

			return unchecked((RendererVSync)interval);

			[DoesNotReturn]
			static void failIntervalArgumentOutOfRange() => throw new ArgumentOutOfRangeException(nameof(interval), $"{nameof(interval)} must be greater than {(int)RendererVSync.Disabled}");

			[DoesNotReturn]
			static void failIntervalArgumentIsAdaptiveOrDisabled() => throw new ArgumentException($"{nameof(interval)} must not be equal to {nameof(RendererVSync.Adaptive)} or {nameof(RendererVSync.Disabled)}. Please use the defined enum members for that.", nameof(interval));
		}
	}
}
