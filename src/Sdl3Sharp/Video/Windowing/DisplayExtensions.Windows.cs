using Sdl3Sharp.Video.Windowing.Drivers;
using System;

namespace Sdl3Sharp.Video.Windowing;

partial class DisplayExtensions
{
	extension(Display<Windows>.PropertyNames)
	{
		/// <summary>
		/// The name of a <em>read-only</em> property <see cref="Properties">property</see> that holds the <c><see href="https://learn.microsoft.com/en-us/windows/win32/winprog/windows-data-types#HMONITOR">HMONITOR</see></c> handle associated with the display
		/// </summary>
		public static string WindowsHMonitorPointer => "SDL.display.windows.hmonitor";
	}

	extension(Display<Windows> display)
	{
		/// <summary>
		/// Gets the <c><see href="https://learn.microsoft.com/en-us/windows/win32/winprog/windows-data-types#HMONITOR">HMONITOR</see></c> handle associated with the display
		/// </summary>
		/// <value>
		/// The <c><see href="https://learn.microsoft.com/en-us/windows/win32/winprog/windows-data-types#HMONITOR">HMONITOR</see></c> handle associated with the display
		/// </value>
		/// <remarks>
		/// <para>
		/// The value of this property can be directly cast to a <c><see href="https://learn.microsoft.com/en-us/windows/win32/winprog/windows-data-types#HMONITOR">HMONITOR</see></c> handle.
		/// </para>
		/// <para>
		/// This property should only be accessed from the main thread.
		/// </para>
		/// </remarks>
		public IntPtr WindowsHMonitor => display?.Properties?.TryGetPointerValue(Display<Windows>.PropertyNames.WindowsHMonitorPointer, out var hMonitor) is true
			? hMonitor
			: default;
	}
}
