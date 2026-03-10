using Sdl3Sharp.Video.Windowing.Drivers;
using System;

namespace Sdl3Sharp.Video.Windowing;

partial class WindowExtensions
{
	extension(Window<Qnx>.PropertyNames)
	{
		/// <summary>
		/// The name of a <em>read-only</em> <see cref="Window.Properties">property</see> that holds
		/// the <c><see href="https://www.qnx.com/developers/docs/8.0/com.qnx.doc.screen/topic/screen_window_t.html">screen_window_t</see></c> associated with the window
		/// </summary>
		public static string QnxWindowPointer => "SDL.window.qnx.window";

		/// <summary>
		/// The name of a <em>read-only</em> <see cref="Window.Properties">property</see> that holds
		/// the <c>EGLSurface</c> associated with the window
		/// </summary>
		public static string QnxSurfacePointer => "SDL.window.qnx.surface";
	}

	extension(Window<Qnx> window)
	{
		/// <summary>
		/// Gets the <c><see href="https://www.qnx.com/developers/docs/8.0/com.qnx.doc.screen/topic/screen_window_t.html">screen_window_t</see></c> associated with this window
		/// </summary>
		/// <value>
		/// The <c><see href="https://www.qnx.com/developers/docs/8.0/com.qnx.doc.screen/topic/screen_window_t.html">screen_window_t</see></c> associated with this window
		/// </value>
		/// <remarks>
		/// <para>
		/// The value of this property can be directly cast to a <c><see href="https://www.qnx.com/developers/docs/8.0/com.qnx.doc.screen/topic/screen_window_t.html">screen_window_t</see></c> handle.
		/// </para>
		/// <para>
		/// This property should only be accessed from the main thread.
		/// </para>
		/// </remarks>
		public IntPtr QnxWindow => window?.Properties?.TryGetPointerValue(Window<Qnx>.PropertyNames.QnxWindowPointer, out var qnxWindowPtr) is true
			? qnxWindowPtr
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
		public IntPtr QnxSurface => window?.Properties?.TryGetPointerValue(Window<Qnx>.PropertyNames.QnxSurfacePointer, out var qnxSurfacePtr) is true
			? qnxSurfacePtr
			: default;
	}
}
