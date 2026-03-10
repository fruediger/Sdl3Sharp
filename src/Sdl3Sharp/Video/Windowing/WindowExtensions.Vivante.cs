using Sdl3Sharp.Video.Windowing.Drivers;
using System;

namespace Sdl3Sharp.Video.Windowing;

partial class WindowExtensions
{
	extension(Window<Vivante>.PropertyNames)
	{
		/// <summary>
		/// The name of a <em>read-only</em> <see cref="Window.Properties">property</see> that holds
		/// the <c>EGLNativeDisplayType</c> associated with the window
		/// </summary>
		public static string VivanteDisplayPointer => "SDL.window.vivante.display";

		/// <summary>
		/// The name of a <em>read-only</em> <see cref="Window.Properties">property</see> that holds
		/// the <c>EGLNativeWindowType</c> associated with the window
		/// </summary>
		public static string VivanteWindowPointer => "SDL.window.vivante.window";

		/// <summary>
		/// The name of a <em>read-only</em> <see cref="Window.Properties">property</see> that holds
		/// the <c>EGLSurface</c> associated with the window
		/// </summary>
		public static string VivanteSurfacePointer => "SDL.window.vivante.surface";
	}

	extension(Window<Vivante> window)
	{
		/// <summary>
		/// Gets the <c>EGLNativeDisplayType</c> associated with this window
		/// </summary>
		/// <value>
		/// The <c>EGLNativeDisplayType</c> associated with this window
		/// </value>
		/// <remarks>
		/// <para>
		/// The value of this property can be directly cast to an <c>EGLNativeDisplayType</c> handle.
		/// </para>
		/// <para>
		/// This property should only be accessed from the main thread.
		/// </para>
		/// </remarks>
		public IntPtr VivanteDisplay => window?.Properties?.TryGetPointerValue(Window<Vivante>.PropertyNames.VivanteDisplayPointer, out var vivanteDisplayPtr) is true
			? vivanteDisplayPtr
			: default;

		/// <summary>
		/// Gets the <c>EGLNativeWindowType</c> associated with this window
		/// </summary>
		/// <value>
		/// The <c>EGLNativeWindowType</c> associated with this window
		/// </value>
		/// <remarks>
		/// <para>
		/// The value of this property can be directly cast to an <c>EGLNativeWindowType</c> handle.
		/// </para>
		/// <para>
		/// This property should only be accessed from the main thread.
		/// </para>
		/// </remarks>
		public IntPtr VivanteWindow => window?.Properties?.TryGetPointerValue(Window<Vivante>.PropertyNames.VivanteWindowPointer, out var vivanteWindowPtr) is true
			? vivanteWindowPtr
			: default;

		/// <summary>
		/// Gets the <c>EGLSurface</c> associated with this window
		/// </summary>
		/// <value>
		/// The <c>EGLSurface</c> associated with this window
		/// </value>
		/// <remarks>
		/// <para>
		/// The value of this property can be directly cast to an <c>EGLSurface</c> handle.
		/// </para>
		/// <para>
		/// This property should only be accessed from the main thread.
		/// </para>
		/// </remarks>
		public IntPtr VivanteSurface => window?.Properties?.TryGetPointerValue(Window<Vivante>.PropertyNames.VivanteSurfacePointer, out var vivanteSurfacePtr) is true
			? vivanteSurfacePtr
			: default;
	}
}
