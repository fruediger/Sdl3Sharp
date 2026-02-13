using Sdl3Sharp.Video.Coloring;
using Sdl3Sharp.Video.Rendering;
using Sdl3Sharp.Video.Rendering.Drivers;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Windowing;

partial class Window
{
	/// <summary>
	/// Tries to create a new Vulkan renderer for this window
	/// </summary>
	/// <inheritdoc cref="TryCreateRenderer{TDriver}(out Renderer{TDriver}?, ColorSpace?, RendererVSync?, Properties?)"/>
	/// <param name="vulkanInstance">
	/// The <c><see href="https://docs.vulkan.org/refpages/latest/refpages/source/VkInstance.html"/>VkInstance</c> to use with the renderer.
	/// Must be directly cast to an <see cref="IntPtr"/> from an <c><see href="https://docs.vulkan.org/refpages/latest/refpages/source/VkInstance.html"/>VkInstance</c> handle.
	/// </param>
	/// <param name="vulkanSurface">
	/// The <c><see href="https://docs.vulkan.org/refpages/latest/refpages/source/VkSurfaceKHR.html"/>VkSurfaceKHR</c> to use with the renderer.
	/// Must be directly cast to an <see cref="ulong"/> from a <c><see href="https://docs.vulkan.org/refpages/latest/refpages/source/VkSurfaceKHR.html"/>VkSurfaceKHR</c> handle.
	/// </param>
	/// <param name="vulkanPhysicalDevice">
	/// The <c><see href="https://docs.vulkan.org/refpages/latest/refpages/source/VkPhysicalDevice.html"/>VkPhysicalDevice</c> to use with the renderer.
	/// Must be directly cast to an <see cref="IntPtr"/> from a <c><see href="https://docs.vulkan.org/refpages/latest/refpages/source/VkPhysicalDevice.html"/>VkPhysicalDevice</c> handle.
	/// </param>
	/// <param name="vulkanDevice">
	/// The <c><see href="https://docs.vulkan.org/refpages/latest/refpages/source/VkDevice.html"/>VkDevice</c> to use with the renderer.
	/// Must be directly cast to an <see cref="IntPtr"/> from a <c><see href="https://docs.vulkan.org/refpages/latest/refpages/source/VkDevice.html"/>VkDevice</c> handle.
	/// </param>
	/// <param name="vulkanGraphicsQueueFamilyIndex">The queue family index used for rendering</param>
	/// <param name="vulkanPresentQueueFamilyIndex">The queue family index used for presentation</param>
#pragma warning disable CS1573 // we get these from inheritdoc
	public bool TryCreateRenderer([NotNullWhen(true)] out Renderer<Vulkan>? renderer, ColorSpace? outputColorSpace = default, RendererVSync? presentVSync = default,
		IntPtr? vulkanInstance = default, ulong? vulkanSurface = default, IntPtr? vulkanPhysicalDevice = default, IntPtr? vulkanDevice = default, uint? vulkanGraphicsQueueFamilyIndex = default, uint? vulkanPresentQueueFamilyIndex = default, Properties? properties = default)
#pragma warning restore CS1573
	{
		Properties propertiesUsed;
		Unsafe.SkipInit(out IntPtr? vulkanInstanceBackup);
		Unsafe.SkipInit(out ulong? vulkanSurfaceBackup);
		Unsafe.SkipInit(out IntPtr? vulkanPhysicalDeviceBackup);
		Unsafe.SkipInit(out IntPtr? vulkanDeviceBackup);
		Unsafe.SkipInit(out uint? vulkanGraphicsQueueFamilyIndexBackup);
		Unsafe.SkipInit(out uint? vulkanPresentQueueFamilyIndexBackup);

		if (properties is null)
		{
			propertiesUsed = [];

			if (vulkanInstance is IntPtr vulkanInstancePtr)
			{
				properties.TrySetPointerValue(Renderer<Vulkan>.PropertyNames.CreateVulkanInstancePointer, vulkanInstancePtr);
			}

			if (vulkanSurface is ulong vulkanSurfaceValue)
			{
				properties.TrySetNumberValue(Renderer<Vulkan>.PropertyNames.CreateVulkanSurfaceNumber, unchecked((long)vulkanSurfaceValue));
			}

			if (vulkanPhysicalDevice is IntPtr vulkanPhysicalDevicePtr)
			{
				properties.TrySetPointerValue(Renderer<Vulkan>.PropertyNames.CreateVulkanPhysicalDevicePointer, vulkanPhysicalDevicePtr);
			}

			if (vulkanDevice is IntPtr vulkanDevicePtr)
			{
				properties.TrySetPointerValue(Renderer<Vulkan>.PropertyNames.CreateVulkanDevicePointer, vulkanDevicePtr);
			}

			if (vulkanGraphicsQueueFamilyIndex is uint vulkanGraphicsQueueFamilyIndexValue)
			{
				properties.TrySetNumberValue(Renderer<Vulkan>.PropertyNames.CreateVulkanGraphicsQueueFamilyIndexNumber, vulkanGraphicsQueueFamilyIndexValue);
			}

			if (vulkanPresentQueueFamilyIndex is uint vulkanPresentQueueIndexValue)
			{
				properties.TrySetNumberValue(Renderer<Vulkan>.PropertyNames.CreateVulkanPresentQueueFamilyIndexNumber, vulkanPresentQueueIndexValue);
			}
		}
		else
		{
			propertiesUsed = properties;

			if (vulkanInstance is IntPtr vulkanInstancePtr)
			{
				vulkanInstanceBackup = propertiesUsed.TryGetPointerValue(Renderer<Vulkan>.PropertyNames.CreateVulkanInstancePointer, out var existingVulkanInstancePtr)
					? existingVulkanInstancePtr
					: null;

				propertiesUsed.TrySetPointerValue(Renderer<Vulkan>.PropertyNames.CreateVulkanInstancePointer, vulkanInstancePtr);
			}

			if (vulkanSurface is ulong vulkanSurfaceValue)
			{
				vulkanSurfaceBackup = propertiesUsed.TryGetNumberValue(Renderer<Vulkan>.PropertyNames.CreateVulkanSurfaceNumber, out var existingVulkanSurfaceValue)
					? unchecked((ulong)existingVulkanSurfaceValue)
					: null;

				propertiesUsed.TrySetNumberValue(Renderer<Vulkan>.PropertyNames.CreateVulkanSurfaceNumber, unchecked((long)vulkanSurfaceValue));
			}

			if (vulkanPhysicalDevice is IntPtr vulkanPhysicalDevicePtr)
			{
				vulkanPhysicalDeviceBackup = propertiesUsed.TryGetPointerValue(Renderer<Vulkan>.PropertyNames.CreateVulkanPhysicalDevicePointer, out var existingVulkanPhysicalDevicePtr)
					? existingVulkanPhysicalDevicePtr
					: null;

				propertiesUsed.TrySetPointerValue(Renderer<Vulkan>.PropertyNames.CreateVulkanPhysicalDevicePointer, vulkanPhysicalDevicePtr);
			}

			if (vulkanDevice is IntPtr vulkanDevicePtr)
			{
				vulkanDeviceBackup = propertiesUsed.TryGetPointerValue(Renderer<Vulkan>.PropertyNames.CreateVulkanDevicePointer, out var existingVulkanDevicePtr)
					? existingVulkanDevicePtr
					: null;

				propertiesUsed.TrySetPointerValue(Renderer<Vulkan>.PropertyNames.CreateVulkanDevicePointer, vulkanDevicePtr);
			}

			if (vulkanGraphicsQueueFamilyIndex is uint vulkanGraphicsQueueFamilyIndexValue)
			{
				vulkanGraphicsQueueFamilyIndexBackup = propertiesUsed.TryGetNumberValue(Renderer<Vulkan>.PropertyNames.CreateVulkanGraphicsQueueFamilyIndexNumber, out var existingVulkanGraphicsQueueFamilyIndexValue)
					? (uint)existingVulkanGraphicsQueueFamilyIndexValue
					: null;

				propertiesUsed.TrySetNumberValue(Renderer<Vulkan>.PropertyNames.CreateVulkanGraphicsQueueFamilyIndexNumber, vulkanGraphicsQueueFamilyIndexValue);
			}

			if (vulkanPresentQueueFamilyIndex is uint vulkanPresentQueueIndexValue)
			{
				vulkanPresentQueueFamilyIndexBackup = propertiesUsed.TryGetNumberValue(Renderer<Vulkan>.PropertyNames.CreateVulkanPresentQueueFamilyIndexNumber, out var existingVulkanPresentQueueFamilyIndexValue)
					? (uint)existingVulkanPresentQueueFamilyIndexValue
					: null;

				propertiesUsed.TrySetNumberValue(Renderer<Vulkan>.PropertyNames.CreateVulkanPresentQueueFamilyIndexNumber, vulkanPresentQueueIndexValue);
			}
		}

		try
		{
			return TryCreateRenderer<Vulkan>(out renderer, outputColorSpace, presentVSync, propertiesUsed);
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

				if (vulkanInstance.HasValue)
				{
					if (vulkanInstanceBackup is IntPtr vulkanInstancePtr)
					{
						propertiesUsed.TrySetPointerValue(Renderer<Vulkan>.PropertyNames.CreateVulkanInstancePointer, vulkanInstancePtr);
					}
					else
					{
						propertiesUsed.TryRemove(Renderer<Vulkan>.PropertyNames.CreateVulkanInstancePointer);
					}
				}

				if (vulkanSurface.HasValue)
				{
					if (vulkanSurfaceBackup is ulong vulkanSurfaceValue)
					{
						propertiesUsed.TrySetNumberValue(Renderer<Vulkan>.PropertyNames.CreateVulkanSurfaceNumber, unchecked((long)vulkanSurfaceValue));
					}
					else
					{
						propertiesUsed.TryRemove(Renderer<Vulkan>.PropertyNames.CreateVulkanSurfaceNumber);
					}
				}

				if (vulkanPhysicalDevice.HasValue)
				{
					if (vulkanPhysicalDeviceBackup is IntPtr vulkanPhysicalDevicePtr)
					{
						propertiesUsed.TrySetPointerValue(Renderer<Vulkan>.PropertyNames.CreateVulkanPhysicalDevicePointer, vulkanPhysicalDevicePtr);
					}
					else
					{
						propertiesUsed.TryRemove(Renderer<Vulkan>.PropertyNames.CreateVulkanPhysicalDevicePointer);
					}
				}

				if (vulkanDevice.HasValue)
				{
					if (vulkanDeviceBackup is IntPtr vulkanDevicePtr)
					{
						propertiesUsed.TrySetPointerValue(Renderer<Vulkan>.PropertyNames.CreateVulkanDevicePointer, vulkanDevicePtr);
					}
					else
					{
						propertiesUsed.TryRemove(Renderer<Vulkan>.PropertyNames.CreateVulkanDevicePointer);
					}
				}

				if (vulkanGraphicsQueueFamilyIndex.HasValue)
				{
					if (vulkanGraphicsQueueFamilyIndexBackup is uint vulkanGraphicsQueueFamilyIndexValue)
					{
						propertiesUsed.TrySetNumberValue(Renderer<Vulkan>.PropertyNames.CreateVulkanGraphicsQueueFamilyIndexNumber, vulkanGraphicsQueueFamilyIndexValue);
					}
					else
					{
						propertiesUsed.TryRemove(Renderer<Vulkan>.PropertyNames.CreateVulkanGraphicsQueueFamilyIndexNumber);
					}
				}

				if (vulkanPresentQueueFamilyIndex.HasValue)
				{
					if (vulkanPresentQueueFamilyIndexBackup is uint vulkanPresentQueueFamilyIndexValue)
					{
						propertiesUsed.TrySetNumberValue(Renderer<Vulkan>.PropertyNames.CreateVulkanPresentQueueFamilyIndexNumber, vulkanPresentQueueFamilyIndexValue);
					}
					else
					{
						propertiesUsed.TryRemove(Renderer<Vulkan>.PropertyNames.CreateVulkanPresentQueueFamilyIndexNumber);
					}
				}
			}
		}
	}
}
