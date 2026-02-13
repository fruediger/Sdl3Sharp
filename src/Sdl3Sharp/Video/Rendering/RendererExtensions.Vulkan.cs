using Sdl3Sharp.Video.Coloring;
using Sdl3Sharp.Video.Rendering.Drivers;
using Sdl3Sharp.Windowing;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Video.Rendering;

partial class RendererExtensions
{
	extension(Renderer<Vulkan>.PropertyNames)
	{
		/// <summary>
		/// The name of a <see cref="Window.TryCreateRenderer(out Renderer{Vulkan}?, ColorSpace?, RendererVSync?, nint?, ulong?, nint?, nint?, uint?, uint?, Properties?)">property used when creating a <see cref="Renderer{TDriver}">Renderer&lt;<see cref="Vulkan">Vulkan</see>&gt;</see></see>
		/// that holds a <c><see href="https://docs.vulkan.org/refpages/latest/refpages/source/VkInstance.html"/>VkInstance</c> to use with the renderer
		/// </summary>
		public static string CreateVulkanInstancePointer => "SDL.renderer.create.vulkan.instance";

		/// <summary>
		/// The name of a <see cref="Window.TryCreateRenderer(out Renderer{Vulkan}?, ColorSpace?, RendererVSync?, nint?, ulong?, nint?, nint?, uint?, uint?, Properties?)">property used when creating a <see cref="Renderer{TDriver}">Renderer&lt;<see cref="Vulkan">Vulkan</see>&gt;</see></see>
		/// that holds the <c><see href="https://docs.vulkan.org/refpages/latest/refpages/source/VkSurfaceKHR.html"/>VkSurfaceKHR</c> to use with the renderer
		/// </summary>
		public static string CreateVulkanSurfaceNumber => "SDL.renderer.create.vulkan.surface";

		/// <summary>
		/// The name of a <see cref="Window.TryCreateRenderer(out Renderer{Vulkan}?, ColorSpace?, RendererVSync?, nint?, ulong?, nint?, nint?, uint?, uint?, Properties?)">property used when creating a <see cref="Renderer{TDriver}">Renderer&lt;<see cref="Vulkan">Vulkan</see>&gt;</see></see>
		/// that holds a <c><see href="https://docs.vulkan.org/refpages/latest/refpages/source/VkPhysicalDevice.html"/>VkPhysicalDevice</c> to use with the renderer
		/// </summary>
		public static string CreateVulkanPhysicalDevicePointer => "SDL.renderer.create.vulkan.physical_device";

		/// <summary>
		/// The name of a <see cref="Window.TryCreateRenderer(out Renderer{Vulkan}?, ColorSpace?, RendererVSync?, nint?, ulong?, nint?, nint?, uint?, uint?, Properties?)">property used when creating a <see cref="Renderer{TDriver}">Renderer&lt;<see cref="Vulkan">Vulkan</see>&gt;</see></see>
		/// that holds a <c><see href="https://docs.vulkan.org/refpages/latest/refpages/source/VkDevice.html"/>VkDevice</c> to use with the renderer
		/// </summary>
		public static string CreateVulkanDevicePointer => "SDL.renderer.create.vulkan.device";

		/// <summary>
		/// The name of a <see cref="Window.TryCreateRenderer(out Renderer{Vulkan}?, ColorSpace?, RendererVSync?, nint?, ulong?, nint?, nint?, uint?, uint?, Properties?)">property used when creating a <see cref="Renderer{TDriver}">Renderer&lt;<see cref="Vulkan">Vulkan</see>&gt;</see></see>
		/// that holds the queue family index used for rendering
		/// </summary>
		public static string CreateVulkanGraphicsQueueFamilyIndexNumber => "SDL.renderer.create.vulkan.graphics_queue_family_index";

		/// <summary>
		/// The name of a <see cref="Window.TryCreateRenderer(out Renderer{Vulkan}?, ColorSpace?, RendererVSync?, nint?, ulong?, nint?, nint?, uint?, uint?, Properties?)">property used when creating a <see cref="Renderer{TDriver}">Renderer&lt;<see cref="Vulkan">Vulkan</see>&gt;</see></see>
		/// that holds the queue family index used for presentation
		/// </summary>
		public static string CreateVulkanPresentQueueFamilyIndexNumber => "SDL.renderer.create.vulkan.present_queue_family_index";

		/// <summary>
		/// The name of a <em>read-only</em> <see cref="IRenderer.Properties">property</see> that holds
		/// a <c><see href="https://docs.vulkan.org/refpages/latest/refpages/source/VkInstance.html"/>VkInstance</c> associated with the renderer
		/// </summary>
		public static string VulkanInstancePointer => "SDL.renderer.vulkan.instance";

		/// <summary>
		/// The name of a <em>read-only</em> <see cref="IRenderer.Properties">property</see> that holds
		/// a <c><see href="https://docs.vulkan.org/refpages/latest/refpages/source/VkSurfaceKHR.html"/>VkSurfaceKHR</c> associated with the renderer
		/// </summary>
		public static string VulkanSurfaceNumber => "SDL.renderer.vulkan.surface";

		/// <summary>
		/// The name of a <em>read-only</em> <see cref="IRenderer.Properties">property</see> that holds
		/// a <c><see href="https://docs.vulkan.org/refpages/latest/refpages/source/VkPhysicalDevice.html"/>VkPhysicalDevice</c> associated with the renderer
		/// </summary>
		public static string VulkanPhysicalDevicePointer => "SDL.renderer.vulkan.physical_device";

		/// <summary>
		/// The name of a <em>read-only</em> <see cref="IRenderer.Properties">property</see> that holds
		/// a <c><see href="https://docs.vulkan.org/refpages/latest/refpages/source/VkDevice.html"/>VkDevice</c> associated with the renderer
		/// </summary>
		public static string VulkanDevicePointer => "SDL.renderer.vulkan.device";

		/// <summary>
		/// The name of a <em>read-only</em> <see cref="IRenderer.Properties">property</see> that holds
		/// the queue family index used for rendering
		/// </summary>
		public static string VulkanGraphicsQueueFamilyIndexNumber => "SDL.renderer.vulkan.graphics_queue_family_index";

		/// <summary>
		/// The name of a <em>read-only</em> <see cref="IRenderer.Properties">property</see> that holds
		/// the queue family index used for presentation
		/// </summary>
		public static string VulkanPresentQueueFamilyIndexNumber => "SDL.renderer.vulkan.present_queue_family_index";

		/// <summary>
		/// The name of a <em>read-only</em> <see cref="IRenderer.Properties">property</see> that holds
		/// the number of swap-chain images, or potential frames in flight, used by the renderer
		/// </summary>
		public static string VulkanSwapChainImageCountNumber => "SDL.renderer.vulkan.swapchain_image_count";
	}

	extension(Renderer<Vulkan> renderer)
	{
		/// <summary>
		/// Gets the <c><see href="https://docs.vulkan.org/refpages/latest/refpages/source/VkDevice.html"/>VkDevice</c> associated with the renderer
		/// </summary>
		/// <value>
		/// The <c><see href="https://docs.vulkan.org/refpages/latest/refpages/source/VkDevice.html"/>VkDevice</c> associated with the renderer
		/// </value>
		/// <remarks>
		/// <para>
		/// The value of this property can be directly cast to a <c><see href="https://docs.vulkan.org/refpages/latest/refpages/source/VkDevice.html"/>VkDevice</c> handle.
		/// </para>
		/// </remarks>
		public IntPtr VulkanDevice => renderer?.Properties?.TryGetPointerValue(Renderer<Vulkan>.PropertyNames.VulkanDevicePointer, out var vulkanDevice) is true
			? vulkanDevice
			: default;

		/// <summary>
		/// Gets the queue family index used for rendering by the renderer
		/// </summary>
		/// <value>
		/// The queue family index used for rendering by the renderer
		/// </value>
		public uint VulkanGraphicsQueueFamilyIndex => renderer?.Properties?.TryGetNumberValue(Renderer<Vulkan>.PropertyNames.VulkanGraphicsQueueFamilyIndexNumber, out var vulkanGraphicsQueueFamilyIndex) is true
			? unchecked((uint)vulkanGraphicsQueueFamilyIndex)
			: default;

		/// <summary>
		/// Gets the <c><see href="https://docs.vulkan.org/refpages/latest/refpages/source/VkInstance.html"/>VkInstance</c> associated with the renderer
		/// </summary>
		/// <value>
		/// The <c><see href="https://docs.vulkan.org/refpages/latest/refpages/source/VkInstance.html"/>VkInstance</c> associated with the renderer
		/// </value>
		/// <remarks>
		/// <para>
		/// The value of this property can be directly cast to a <c><see href="https://docs.vulkan.org/refpages/latest/refpages/source/VkInstance.html"/>VkInstance</c> handle.
		/// </para>
		/// </remarks>
		public IntPtr VulkanInstance => renderer?.Properties?.TryGetPointerValue(Renderer<Vulkan>.PropertyNames.VulkanInstancePointer, out var vulkanInstance) is true
			? vulkanInstance
			: default;

		/// <summary>
		/// Gets the <c><see href="https://docs.vulkan.org/refpages/latest/refpages/source/VkPhysicalDevice.html"/>VkPhysicalDevice</c> associated with the renderer
		/// </summary>
		/// <value>
		/// The <c><see href="https://docs.vulkan.org/refpages/latest/refpages/source/VkPhysicalDevice.html"/>VkPhysicalDevice</c> associated with the renderer
		/// </value>
		/// <remarks>
		/// <para>
		/// The value of this property can be directly cast to a <c><see href="https://docs.vulkan.org/refpages/latest/refpages/source/VkPhysicalDevice.html"/>VkPhysicalDevice</c> handle.
		/// </para>
		/// </remarks>
		public IntPtr VulkanPhysicalDevice => renderer?.Properties?.TryGetPointerValue(Renderer<Vulkan>.PropertyNames.VulkanPhysicalDevicePointer, out var vulkanPhysicalDevice) is true
			? vulkanPhysicalDevice
			: default;

		/// <summary>
		/// Gets the queue family index used for presentation by the renderer
		/// </summary>
		/// <value>
		/// The queue family index used for presentation by the renderer
		/// </value>
		public uint VulkanPresentQueueFamilyIndex => renderer?.Properties?.TryGetNumberValue(Renderer<Vulkan>.PropertyNames.VulkanPresentQueueFamilyIndexNumber, out var vulkanPresentQueueFamilyIndex) is true
			? unchecked((uint)vulkanPresentQueueFamilyIndex)
			: default;

		/// <summary>
		/// Gets the number of swap-chain images, or potential frames in flight, used by the renderer
		/// </summary>
		/// <value>
		/// The number of swap-chain images, or potential frames in flight, used by the renderer
		/// </value>
		public uint VulkanSwapChainImageCount => renderer?.Properties?.TryGetNumberValue(Renderer<Vulkan>.PropertyNames.VulkanSwapChainImageCountNumber, out var vulkanSwapChainImageCount) is true
			? unchecked((uint)vulkanSwapChainImageCount)
			: default;

		/// <summary>
		/// Gets the <c><see href="https://docs.vulkan.org/refpages/latest/refpages/source/VkSurfaceKHR.html"/>VkSurfaceKHR</c> associated with the renderer
		/// </summary>
		/// <value>
		/// The <c><see href="https://docs.vulkan.org/refpages/latest/refpages/source/VkSurfaceKHR.html"/>VkSurfaceKHR</c> associated with the renderer
		/// </value>
		/// <remarks>
		/// <para>
		/// The value of this property can be directly cast to a <c><see href="https://docs.vulkan.org/refpages/latest/refpages/source/VkSurfaceKHR.html"/>VkSurfaceKHR</c> handle.
		/// </para>
		/// </remarks>
		public ulong VulkanSurface => renderer?.Properties?.TryGetNumberValue(Renderer<Vulkan>.PropertyNames.VulkanSurfaceNumber, out var vulkanSurface) is true
			? unchecked((ulong)vulkanSurface)
			: default;

		/// <summary>
		/// Tries to add a set synchronization semaphores for the current frame
		/// </summary>
		/// <param name="waitStateMask">The <c><see href="https://docs.vulkan.org/refpages/latest/refpages/source/VkPipelineStageFlags.html"/>VkPipelineStageFlags</c> for the wait</param>
		/// <param name="waitSemaphore">
		/// A <c><see href="https://docs.vulkan.org/refpages/latest/refpages/source/VkSemaphore.html"/>VkSemaphore</c> handle to wait on before rendering the current frame, or <c>0</c> if not needed.
		/// Must be directly cast to a <see cref="long"/> from a <c><see href="https://docs.vulkan.org/refpages/latest/refpages/source/VkSemaphore.html"/>VkSemaphore</c> handle.
		/// </param>
		/// <param name="signalSemaphore">
		/// A <c><see href="https://docs.vulkan.org/refpages/latest/refpages/source/VkSemaphore.html"/>VkSemaphore</c> handle that SDL will signal when rendering for the current frame is complete, or <c>0</c> if not needed.
		/// Must be directly cast to a <see cref="long"/> from a <c><see href="https://docs.vulkan.org/refpages/latest/refpages/source/VkSemaphore.html"/>VkSemaphore</c> handle.
		/// </param>
		/// <returns><c><see langword="true"/></c>, if the semaphores were successfully added for this frame; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
		/// <remarks>
		/// <para>
		/// The Vulkan renderer will wait for <paramref name="waitSemaphore"/> before submitting rendering commands and signal <paramref name="signalSemaphore"/> after rendering commands are completed for this frame.
		/// </para>
		/// <para>
		/// This method should be called each frame that you want semaphore synchronization.
		/// The Vulkan renderer may have multiple frames in flight on the GPU, so you should have multiple semaphores that are used for synchronization.
		/// The value of the <see cref="get_VulkanSwapChainImageCount(Renderer{Vulkan})"/> property gives you the maximum number of semaphores you'll need.
		/// </para>
		/// <para>
		/// It is <em>not</em> safe to call this method from two different threads at once.
		/// </para>
		/// </remarks>
		public bool TryAddVulkanRenderSemaphores(uint waitStateMask, long waitSemaphore, long signalSemaphore)
		{
			unsafe
			{
				return SDL_AddVulkanRenderSemaphores(renderer is not null ? renderer.Pointer : null, waitStateMask, waitSemaphore, signalSemaphore);
			}
		}

		/// <inheritdoc cref="Renderer{TDriver}.TryCreateTexture(out Texture{TDriver}?, ColorSpace?, PixelFormat?, TextureAccess?, int?, int?, Palette?, float?, float?, Properties?)"/>
		/// <param name="vulkanTexture">
		/// A <c><see href="https://docs.vulkan.org/refpages/latest/refpages/source/VkImage.html"/>VkImage</c> handle to associate with the newly created texture, if you want to wrap an existing texture.
		/// Must be directly cast to a <see cref="ulong"/> from a <c><see href="https://docs.vulkan.org/refpages/latest/refpages/source/VkImage.html"/>VkImage</c> handle.
		/// </param>
		/// <param name="vulkanLayout">
		/// A <c><see href="https://docs.vulkan.org/refpages/latest/refpages/source/VkImageLayout.html"/>VkImageLayout</c> value for the <c><see href="https://docs.vulkan.org/refpages/latest/refpages/source/VkImage.html">VkImage</see></c>.
		/// Must be directly cast to a <see cref="uint"/> from a <c><see href="https://docs.vulkan.org/refpages/latest/refpages/source/VkImageLayout.html"/>VkImageLayout</c> value.
		/// </param>
#pragma warning disable CS1573 // we get these from inheritdoc
		public bool TryCreateTexture([NotNullWhen(true)] out Texture<Vulkan>? texture, ColorSpace? colorSpace = default, PixelFormat? format = default, TextureAccess? access = default, int? width = default, int? height = default,
#if SDL3_4_0_OR_GREATER
			Palette? palette = default,
#endif
			float? sdrWhitePoint = default, float? hdrHeadroom = default,
			ulong? vulkanTexture = default, uint? vulkanLayout = default, Properties? properties = default)
#pragma warning restore CS1573
		{
			if (renderer is null)
			{
				texture = null;
				return false;
			}

			Properties propertiesUsed;
			Unsafe.SkipInit(out ulong? vulkanTextureBackup);
			Unsafe.SkipInit(out uint? vulkanLayoutBackup);

			if (properties is null)
			{
				propertiesUsed = [];

				if (vulkanTexture is ulong vulkanTextureValue)
				{
					propertiesUsed.TrySetNumberValue(Texture<Vulkan>.PropertyNames.CreateVulkanTextureNumber, unchecked((long)vulkanTextureValue));
				}

				if (vulkanLayout is uint vulkanLayoutValue)
				{
					propertiesUsed.TrySetNumberValue(Texture<Vulkan>.PropertyNames.CreateVulkanLayoutNumber, vulkanLayoutValue);
				}
			}
			else
			{
				propertiesUsed = properties;

				if (vulkanTexture is ulong vulkanTextureValue)
				{
					vulkanTextureBackup = propertiesUsed.TryGetNumberValue(Texture<Vulkan>.PropertyNames.CreateVulkanTextureNumber, out var existingVulkanTextureValue)
						? unchecked((ulong)existingVulkanTextureValue)
						: null;

					propertiesUsed.TrySetNumberValue(Texture<Vulkan>.PropertyNames.CreateVulkanTextureNumber, unchecked((long)vulkanTextureValue));
				}

				if (vulkanLayout is uint vulkanLayoutValue)
				{
					vulkanLayoutBackup = propertiesUsed.TryGetNumberValue(Texture<Vulkan>.PropertyNames.CreateVulkanLayoutNumber, out var existingVulkanLayoutValue)
						? unchecked((uint)existingVulkanLayoutValue)
						: null;

					propertiesUsed.TrySetNumberValue(Texture<Vulkan>.PropertyNames.CreateVulkanLayoutNumber, vulkanLayoutValue);
				}
			}

			try
			{
				return renderer.TryCreateTexture(out texture, colorSpace, format, access, width, height,
#if SDL3_4_0_OR_GREATER
					palette,
#endif
					sdrWhitePoint, hdrHeadroom, propertiesUsed);
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

					if (vulkanTexture.HasValue)
					{
						if (vulkanTextureBackup is ulong vulkanTextureValue)
						{
							propertiesUsed.TrySetNumberValue(Texture<Vulkan>.PropertyNames.CreateVulkanTextureNumber, unchecked((long)vulkanTextureValue));
						}
						else
						{
							propertiesUsed.TryRemove(Texture<Vulkan>.PropertyNames.CreateVulkanTextureNumber);
						}
					}

					if (vulkanLayout.HasValue)
					{
						if (vulkanLayoutBackup is uint vulkanLayoutValue)
						{
							propertiesUsed.TrySetNumberValue(Texture<Vulkan>.PropertyNames.CreateVulkanLayoutNumber, vulkanLayoutValue);
						}
						else
						{
							propertiesUsed.TryRemove(Texture<Vulkan>.PropertyNames.CreateVulkanLayoutNumber);
						}
					}
				}
			}
		}
	}
}
