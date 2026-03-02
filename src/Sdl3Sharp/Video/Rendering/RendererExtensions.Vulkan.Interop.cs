using Sdl3Sharp.Internal.Interop;
using Sdl3Sharp.SourceGeneration;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Video.Rendering;

partial class RendererExtensions
{
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
	internal unsafe static partial CBool SDL_AddVulkanRenderSemaphores(Renderer.SDL_Renderer* renderer, uint wait_state_mask, long wait_semaphore, long signal_semaphore);
}
