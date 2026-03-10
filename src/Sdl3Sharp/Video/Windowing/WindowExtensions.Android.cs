using Sdl3Sharp.Video.Windowing.Drivers;
using System;

namespace Sdl3Sharp.Video.Windowing;

partial class WindowExtensions
{
	extension(Window<Android>.PropertyNames)
	{
		/// <summary>
		/// The name of a <em>read-only</em> <see cref="Window.Properties">property</see> that holds
		/// a pointer to <c><see href="https://developer.android.com/ndk/reference/group/a-native-window">ANativeWindow</see></c> associated with the window
		/// </summary>
		public static string AndroidWindowPointer => "SDL.window.android.window";

		/// <summary>
		/// The name of a <em>read-only</em> <see cref="Window.Properties">property</see> that holds
		/// the <c>EGLSurface</c> associated with the window
		/// </summary>
		public static string AndroidSurfacePointer => "SDL.window.android.surface";
	}

	extension(Window<Android> window)
	{
		/// <summary>
		/// Gets a pointer to the <c><see href="https://developer.android.com/ndk/reference/group/a-native-window">ANativeWindow</see></c> associated with this window
		/// </summary>
		/// <value>
		/// A pointer to the <c><see href="https://developer.android.com/ndk/reference/group/a-native-window">ANativeWindow</see></c> associated with this window
		/// </value>
		/// <remarks>
		/// <para>
		/// The value of this property can be directly cast to an <c><see href="https://developer.android.com/ndk/reference/group/a-native-window">ANativeWindow</see>*</c> pointer.
		/// </para>
		/// <para>
		/// This property should only be accessed from the main thread.
		/// </para>
		/// </remarks>
		public IntPtr AndroidWindow => window?.Properties?.TryGetPointerValue(Window<Android>.PropertyNames.AndroidWindowPointer, out var androidWindowPtr) is true
			? androidWindowPtr
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
		public IntPtr AndroidSurface => window?.Properties?.TryGetPointerValue(Window<Android>.PropertyNames.AndroidSurfacePointer, out var androidSurfacePtr) is true
			? androidSurfacePtr
			: default;
	}
}
