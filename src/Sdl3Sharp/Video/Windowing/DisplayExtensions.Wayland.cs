using Sdl3Sharp.Video.Windowing.Drivers;
using System;

namespace Sdl3Sharp.Video.Windowing;

partial class DisplayExtensions
{
	extension(Display<Wayland>.PropertyNames)
	{
		/// <summary>
		/// The name of a <em>read-only</em> property <see cref="Properties">property</see> that holds a pointer to the Wayland <c><see href="https://wayland.freedesktop.org/docs/html/apa.html#protocol-spec-wl_output">wl_output</see></c> associated with the display
		/// </summary>
		public static string WaylandWlOutputPointer => "SDL.display.Wayland.wl_output";
	}

	extension(Display<Wayland> display)
	{
		/// <summary>
		/// Gets a pointer to the Wayland <c><see href="https://wayland.freedesktop.org/docs/html/apa.html#protocol-spec-wl_output">wl_output</see></c> associated with the display
		/// </summary>
		/// <value>
		/// A pointer to the Wayland <c><see href="https://wayland.freedesktop.org/docs/html/apa.html#protocol-spec-wl_output">wl_output</see></c> associated with the display
		/// </value>
		/// <remarks>
		/// <para>
		/// The value of this property can be directly cast to a <c><see href="https://wayland.freedesktop.org/docs/html/apa.html#protocol-spec-wl_output">wl_output</see>*</c> pointer.
		/// </para>
		/// <para>
		/// This property should only be accessed from the main thread.
		/// </para>
		/// </remarks>
		public IntPtr WaylandWlOutput => display?.Properties?.TryGetPointerValue(Display<Wayland>.PropertyNames.WaylandWlOutputPointer, out var wlOutput) is true
			? wlOutput
			: default;
	}
}
