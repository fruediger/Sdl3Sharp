using Sdl3Sharp.Video.Windowing.Drivers;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Video.Windowing;

partial class WindowExtensions
{
	extension(Window<Windows>.PropertyNames)
	{
		/// <summary>
		/// The name of a <see cref="TryCreate(out Window{Windows}?, bool?, bool?, bool?, bool?, WindowFlags?, bool?, bool?, int?, bool?, bool?, bool?, bool?, bool?, bool?, bool?, bool?, bool?, Window?, bool?, string?, bool?, bool?, bool?, bool?, int?, WindowPosition?, WindowPosition?, nint?, nint?, Properties?)">property used when creating a <see cref="Window{TDriver}">Window&lt;<see cref="Windows">Windows</see>&gt;</see></see>
		/// that hold the <c><see href="https://learn.microsoft.com/en-us/windows/win32/winprog/windows-data-types#HWND">HWND</see></c> handle associated with the window, if you want to wrap an existing window
		/// </summary>
		public static string CreateWindowsHWndPointer => "SDL.window.create.win32.hwnd";

		/// <summary>
		/// The name of a <see cref="TryCreate(out Window{Windows}?, bool?, bool?, bool?, bool?, WindowFlags?, bool?, bool?, int?, bool?, bool?, bool?, bool?, bool?, bool?, bool?, bool?, bool?, Window?, bool?, string?, bool?, bool?, bool?, bool?, int?, WindowPosition?, WindowPosition?, nint?, nint?, Properties?)">property used when creating a <see cref="Window{TDriver}">Window&lt;<see cref="Windows">Windows</see>&gt;</see></see>
		/// that hold the <c><see href="https://learn.microsoft.com/en-us/windows/win32/winprog/windows-data-types#HWND">HWND</see></c> handle of another window to share its pixel format with
		/// </summary>
		/// <remarks>
		/// <para>
		/// Specifying the associated property is purely optional, but can be useful for OpenGL windows.
		/// </para>
		/// </remarks>
		public static string CreateWindowsPixelFormatHWndPointer => "SDL.window.create.win32.pixel_format_hwnd";

		/// <summary>
		/// The name of a <em>read-only</em> <see cref="Window.Properties">property</see> that holds
		/// the <c><see href="https://learn.microsoft.com/en-us/windows/win32/winprog/windows-data-types#HWND">HWND</see></c> handle associated with the window
		/// </summary>
		public static string WindowsHWndPointer => "SDL.window.win32.hwnd";

		/// <summary>
		/// The name of a <em>read-only</em> <see cref="Window.Properties">property</see> that holds
		/// the <c><see href="https://learn.microsoft.com/en-us/windows/win32/winprog/windows-data-types#HDC">HDC</see></c> handle associated with the window
		/// </summary>
		public static string WindowsHDCPointer => "SDL.window.win32.hdc";

		/// <summary>
		/// The name of a <em>read-only</em> <see cref="Window.Properties">property</see> that holds
		/// the <c><see href="https://learn.microsoft.com/en-us/windows/win32/winprog/windows-data-types#HINSTANCE">HINSTANCE</see></c> handle associated with the window
		/// </summary>
		public static string WindowsInstancePointer => "SDL.window.win32.instance";
	}

	extension(Window<Windows>)
	{
		/// <inheritdoc cref="Window.TryCreate(out Window?, bool?, bool?, bool?, bool?, WindowFlags?, bool?, bool?, int?, bool?, bool?, bool?, bool?, bool?, bool?, bool?, bool?, bool?, Window?, bool?, string?, bool?, bool?, bool?, bool?, int?, WindowPosition?, WindowPosition?, Properties?)"/>
		/// <param name="windowsHWnd">The <c><see href="https://learn.microsoft.com/en-us/windows/win32/winprog/windows-data-types#HWND">HWND</see></c> handle associated with the window, if you want to wrap an existing window</param>
		/// <param name="windowsPixelFormatHWnd">
		/// The <c><see href="https://learn.microsoft.com/en-us/windows/win32/winprog/windows-data-types#HWND">HWND</see></c> handle of another window to share its pixel format with.
		/// Specifying this is purely optional, but can be useful for OpenGL windows.
		/// </param>
#pragma warning disable CS1573 // we get these from inheritdoc
		public static bool TryCreate([NotNullWhen(true)] out Window<Windows>? window, bool? alwaysOnTop = default, bool? bordered = default, bool? constrainPopup = default, bool? externalGraphicsContext = default, WindowFlags? flags = default,
			bool? focusable = default, bool? fullscreen = default, int? height = default, bool? hidden = default, bool? highPixelDensity = default, bool? maximized = default, bool? menu = default, bool? metal = default,
			bool? minimized = default, bool? modal = default, bool? mouseGrabbed = default, bool? openGL = default, Window? parent = default, bool? resizable = default, string? title = default, bool? transparent = default,
			bool? tooltip = default, bool? utility = default, bool? vulkan = default, int? width = default, WindowPosition? x = default, WindowPosition? y = default,
			IntPtr? windowsHWnd = default, IntPtr? windowsPixelFormatHWnd = default, Properties? properties = default)
#pragma warning restore CS1573
		{
			if (!Windows.IsActive)
			{
				window = null;
				return false;
			}

			Properties propertiesUsed;
			Unsafe.SkipInit(out IntPtr? windowsHWndBackup);
			Unsafe.SkipInit(out IntPtr? windowsPixelFormatHWndBackup);

			if (properties is null)
			{
				propertiesUsed = [];

				if (windowsHWnd is IntPtr windowsHWndValue)
				{
					propertiesUsed.TrySetPointerValue(Window<Windows>.PropertyNames.CreateWindowsHWndPointer, windowsHWndValue);
				}

				if (windowsPixelFormatHWnd is IntPtr windowsPixelFormatHWndValue)
				{
					propertiesUsed.TrySetPointerValue(Window<Windows>.PropertyNames.CreateWindowsPixelFormatHWndPointer, windowsPixelFormatHWndValue);
				}
			}
			else
			{
				propertiesUsed = properties;

				if (windowsHWnd is IntPtr windowsHWndValue)
				{
					windowsHWndBackup = propertiesUsed.TryGetPointerValue(Window<Windows>.PropertyNames.CreateWindowsHWndPointer, out var existingWindowsHWnd) is true
						? existingWindowsHWnd
						: null;

					propertiesUsed.TrySetPointerValue(Window<Windows>.PropertyNames.CreateWindowsHWndPointer, windowsHWndValue);
				}

				if (windowsPixelFormatHWnd is IntPtr windowsPixelFormatHWndValue)
				{
					windowsPixelFormatHWndBackup = propertiesUsed.TryGetPointerValue(Window<Windows>.PropertyNames.CreateWindowsPixelFormatHWndPointer, out var existingWindowsPixelFormatHWnd) is true
						? existingWindowsPixelFormatHWnd
						: null;

					propertiesUsed.TrySetPointerValue(Window<Windows>.PropertyNames.CreateWindowsPixelFormatHWndPointer, windowsPixelFormatHWndValue);
				}
			}

			try
			{
				return Window.TryCreateUnchecked(
					out window, alwaysOnTop, bordered, constrainPopup, externalGraphicsContext, flags,
					focusable, fullscreen, height, hidden, highPixelDensity, maximized, menu, metal,
					minimized, modal, mouseGrabbed, openGL, parent, resizable, title, transparent,
					tooltip, utility, vulkan, width, x, y, propertiesUsed
				);
			}
			finally
			{
				if (properties is null)
				{
					// propertiesUsed was just a temporary instance we created for this call, so we need to dispose it now

					propertiesUsed.Dispose();
				}
				else
				{
					// we restored the original properties values from the given properties instance

					if (windowsHWnd.HasValue)
					{
						if (windowsHWndBackup is IntPtr windowsHWndValue)
						{
							propertiesUsed.TrySetPointerValue(Window<Windows>.PropertyNames.CreateWindowsHWndPointer, windowsHWndValue);
						}
						else
						{
							propertiesUsed.TryRemove(Window<Windows>.PropertyNames.CreateWindowsHWndPointer);
						}
					}

					if (windowsPixelFormatHWnd.HasValue)
					{
						if (windowsPixelFormatHWndBackup is IntPtr windowsPixelFormatHWndValue)
						{
							propertiesUsed.TrySetPointerValue(Window<Windows>.PropertyNames.CreateWindowsPixelFormatHWndPointer, windowsPixelFormatHWndValue);
						}
						else
						{
							propertiesUsed.TryRemove(Window<Windows>.PropertyNames.CreateWindowsPixelFormatHWndPointer);
						}
					}
				}
			}
		}
	}

	extension(Window<Windows> window)
	{
		/// <summary>
		/// Gets the <c><see href="https://learn.microsoft.com/en-us/windows/win32/winprog/windows-data-types#HWND">HWND</see></c> handle associated with this window
		/// </summary>
		/// <value>
		/// The <c><see href="https://learn.microsoft.com/en-us/windows/win32/winprog/windows-data-types#HWND">HWND</see></c> handle associated with this window
		/// </value>
		/// <remarks>
		/// <para>
		/// The value of this property can be directly cast to an <c><see href="https://learn.microsoft.com/en-us/windows/win32/winprog/windows-data-types#HWND">HWND</see></c> handle.
		/// </para>
		/// <para>
		/// This property should only be accessed from the main thread.
		/// </para>
		/// </remarks>
		public IntPtr WindowsHWnd => window?.Properties?.TryGetPointerValue(Window<Windows>.PropertyNames.WindowsHWndPointer, out var windowsHWndPtr) is true
			? windowsHWndPtr
			: default;

		/// <summary>
		/// Gets the <c><see href="https://learn.microsoft.com/en-us/windows/win32/winprog/windows-data-types#HDC">HDC</see></c> handle associated with this window
		/// </summary>
		/// <remarks>
		/// <para>
		/// The value of this property can be directly cast to an <c><see href="https://learn.microsoft.com/en-us/windows/win32/winprog/windows-data-types#HDC">HDC</see></c> handle.
		/// </para>
		/// <para>
		/// This property should only be accessed from the main thread.
		/// </para>
		/// </remarks>
		public IntPtr WindowsHDC => window?.Properties?.TryGetPointerValue(Window<Windows>.PropertyNames.WindowsHDCPointer, out var windowsHDcPtr) is true
			? windowsHDcPtr
			: default;

		/// <summary>
		/// Gets the <c><see href="https://learn.microsoft.com/en-us/windows/win32/winprog/windows-data-types#HINSTANCE">HINSTANCE</see></c> handle associated with this window
		/// </summary>
		/// <value>
		/// The <c><see href="https://learn.microsoft.com/en-us/windows/win32/winprog/windows-data-types#HINSTANCE">HINSTANCE</see></c> handle associated with this window
		/// </value>
		/// <remarks>
		/// <para>
		/// The value of this property can be directly cast to an <c><see href="https://learn.microsoft.com/en-us/windows/win32/winprog/windows-data-types#HINSTANCE">HINSTANCE</see></c> handle.
		/// </para>
		/// <para>
		/// This property should only be accessed from the main thread.
		/// </para>
		/// </remarks>
		public IntPtr WindowsInstance => window?.Properties?.TryGetPointerValue(Window<Windows>.PropertyNames.WindowsInstancePointer, out var windowsInstancePtr) is true
			? windowsInstancePtr
			: default;
	}
}
