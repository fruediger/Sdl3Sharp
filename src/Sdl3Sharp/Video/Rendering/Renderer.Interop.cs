using Sdl3Sharp.Events;
using Sdl3Sharp.Internal.Interop;
using Sdl3Sharp.SourceGeneration;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sdl3Sharp.Video.Rendering;

partial class Renderer
{
	// opaque struct
	[StructLayout(LayoutKind.Sequential, Size = 0)]
	internal readonly struct SDL_Renderer;

	/// <summary>
	/// Add a set of synchronization semaphores for the current frame
	/// </summary>
	/// <param name="renderer">The rendering context</param>
	/// <param name="wait_state_mask">The VkPipelineStageFlags for the wait</param>
	/// <param name="wait_semaphore">A VkSempahore to wait on before rendering the current frame, or 0 if not needed</param>
	/// <param name="signal_semaphore">A VkSempahore that SDL will signal when rendering for the current frame is complete, or 0 if not needed</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// The Vulkan renderer will wait for <c><paramref name="wait_semaphore"/></c> before submitting rendering commands and signal <c><paramref name="signal_semaphore"/></c> after rendering commands are complete for this frame.
	/// </para>
	/// <para>
	/// This should be called each frame that you want semaphore synchronization.
	/// The Vulkan renderer may have multiple frames in flight on the GPU, so you should have multiple semaphores that are used for synchronization.
	/// Querying <see href="https://wiki.libsdl.org/SDL3/SDL_PROP_RENDERER_VULKAN_SWAPCHAIN_IMAGE_COUNT_NUMBER">SDL_PROP_RENDERER_VULKAN_SWAPCHAIN_IMAGE_COUNT_NUMBER</see> will give you the maximum number of semaphores you'll need.
	/// </para>
	/// <para>
	/// It is <em>NOT</em> safe to call this function from two threads at once.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_AddVulkanRenderSemaphores">SDL_AddVulkanRenderSemaphores</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_AddVulkanRenderSemaphores(SDL_Renderer* renderer, uint wait_state_mask, long wait_semaphore, long signal_semaphore);

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
	internal unsafe static partial CBool SDL_ConvertEventToRenderCoordinates(SDL_Renderer* renderer, Event* @event);
}
