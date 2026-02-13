using Sdl3Sharp.Internal.Interop;
using Sdl3Sharp.SourceGeneration;
using Sdl3Sharp.Video.Rendering;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Events;

partial class EventExtensions
{
	/// <summary>
	/// Convert the coordinates in an event to render coordinates
	/// </summary>
	/// <param name="renderer">The rendering context</param>
	/// <param name="event">The event to modify</param>
	/// <returns>Returns true if the event is converted or doesn't need conversion, or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// This takes into account several states:
	/// <list type="bullet">
	/// <item><description>The window dimensions</description></item>
	/// <item><description>The logical presentation settings (<see href="https://wiki.libsdl.org/SDL3/SDL_SetRenderLogicalPresentation">SDL_SetRenderLogicalPresentation</see>)</description></item>
	/// <item><description>The scale (<see href="https://wiki.libsdl.org/SDL3/SDL_SetRenderScale">SDL_SetRenderScale</see>)</description></item>
	/// <item><description>The viewport (<see href="https://wiki.libsdl.org/SDL3/SDL_SetRenderViewport">SDL_SetRenderViewport</see>)</description></item>
	/// </list>
	/// </para>
	/// <para>
	/// Various event types are converted with this function: mouse, touch, pen, etc.
	/// </para>
	/// <para>
	/// Touch coordinates are converted from normalized coordinates in the window to non-normalized rendering coordinates.
	/// </para>
	/// <para>
	/// Relative mouse coordinates (xrel and yrel event fields) are <em>also</em> converted.
	/// Applications that do not want these fields converted should use <see href="https://wiki.libsdl.org/SDL3/SDL_RenderCoordinatesFromWindow">SDL_RenderCoordinatesFromWindow</see>() on the specific event fields instead of converting the entire event structure.
	/// </para>
	/// <para>
	/// Once converted, coordinates may be outside the rendering area.
	/// </para>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_ConvertEventToRenderCoordinates">SDL_ConvertEventToRenderCoordinates</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_ConvertEventToRenderCoordinates(IRenderer.SDL_Renderer* renderer, Event* @event);

	/* TODO: implement when Window is implemented
	 * 
	/// <summary>
	/// Get window associated with an event
	/// </summary>
	/// <param name="event">An event containing a <c>windowID</c></param>
	/// <returns>Returns the associated window on success or NULL if there is none</returns>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetWindowFromEvent">SDL_GetWindowFromEvent</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial SDL_Window* SDL_RegisterEvents(Event* @event);
	*
	*/

#if SDL3_4_0_OR_GREATER

	// TODO: IMPLEMENT! <- might require a total refactor of the Event API
	/*
	/// <summary>
	/// Generate an English description of an event
	/// </summary>
	/// <param name="event">An event to describe. May be NULL.</param>
	/// <param name="buf">The buffer to fill with the description string. May be NULL.</param>
	/// <param name="buflen">The maximum bytes that can be written to <c>buf</c></param>
	/// <returns>Returns number of bytes needed for the full string, not counting the null-terminator byte</returns>
	/// <remarks>
	/// <para>
	/// This will fill <c><paramref name="buf"/></c> with a null-terminated string that might look something like this:
	/// <code>
	/// SDL_EVENT_MOUSE_MOTION (timestamp=1140256324 windowid=2 which=0 state=0 x=492.99 y=139.09 xrel=52 yrel=6)
	/// </code>
	/// The exact format of the string is not guaranteed; it is intended for logging purposes, to be read by a human, and not parsed by a computer.
	/// </para>
	/// <para>
	/// The returned value follows the same rules as <see href="https://wiki.libsdl.org/SDL3/SDL_snprintf">SDL_snprintf</see>(): <c><paramref name="buf"/></c> will always be NULL-terminated (unless <c><paramref name="buflen"/></c> is zero), and will be truncated if <c><paramref name="buflen"/></c> is too small.
	/// The return code is the number of bytes needed for the complete string, not counting the NULL-terminator, whether the string was truncated or not.
	/// Unlike <see href="https://wiki.libsdl.org/SDL3/SDL_snprintf">SDL_snprintf</see>(), though, this function never returns -1.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetEventDescription">SDL_GetEventDescription</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial int SDL_GetEventDescription(Event* @event, byte* buf, int buflen);
	*/

#endif
}
