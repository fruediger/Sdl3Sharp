using Sdl3Sharp.Video.Rendering.Drivers;

namespace Sdl3Sharp.Video.Rendering;

partial class TextureExtensions
{
	extension(Texture<Vulkan>.PropertyNames)
	{
		/// <summary>
		/// The name of a <see cref="RendererExtensions.TryCreateTexture(Renderer{Vulkan}, out Texture{Vulkan}?, Coloring.ColorSpace?, Coloring.PixelFormat?, TextureAccess?, int?, int?, Coloring.Palette?, float?, float?, ulong?, uint?, Properties?)">property used when creating a <see cref="Texture{TDriver}">Texture&lt;<see cref="Vulkan">Vulkan</see>&gt;</see></see>
		/// that holds the <c><see href="https://docs.vulkan.org/refpages/latest/refpages/source/VkImage.html">VkImage</see></c> associated with the texture, if you want to wrap an existing texture
		/// </summary>
		public static string CreateVulkanTextureNumber => "SDL.texture.create.vulkan.texture";

		/// <summary>
		/// The name of a <see cref="RendererExtensions.TryCreateTexture(Renderer{Vulkan}, out Texture{Vulkan}?, Coloring.ColorSpace?, Coloring.PixelFormat?, TextureAccess?, int?, int?, Coloring.Palette?, float?, float?, ulong?, uint?, Properties?)">property used when creating a <see cref="Texture{TDriver}">Texture&lt;<see cref="Vulkan">Vulkan</see>&gt;</see></see>
		/// that holds the <c><see href="https://docs.vulkan.org/refpages/latest/refpages/source/VkImageLayout.html">VkImageLayout</see></c> for the <c><see href="https://docs.vulkan.org/refpages/latest/refpages/source/VkImage.html">VkImage</see></c>
		/// </summary>
		/// <remarks>
		/// The value of the associated property default to <c>VK_IMAGE_LAYOUT_SHADER_READ_ONLY_OPTIMAL</c>.
		/// </remarks>
		public static string CreateVulkanLayoutNumber => "SDL.texture.create.vulkan.layout";

		/// <summary>
		/// The name of a <em>read-only</em> <see cref="ITexture.Properties">property</see> that holds
		/// the <c><see href="https://docs.vulkan.org/refpages/latest/refpages/source/VkImage.html">VkImage</see></c> associated with the texture
		/// </summary>
		public static string VulkanTextureNumber => "SDL.texture.vulkan.texture";
	}

	extension(Texture<Vulkan> texture)
	{
		/// <summary>
		/// Gets the <c><see href="https://docs.vulkan.org/refpages/latest/refpages/source/VkImage.html">VkImage</see></c> associated with the texture
		/// </summary>
		/// <value>
		/// The <c><see href="https://docs.vulkan.org/refpages/latest/refpages/source/VkImage.html">VkImage</see></c> associated with the texture
		/// </value>
		/// <remarks>
		/// <para>
		/// The value of this property can be directly cast to a <c><see href="https://docs.vulkan.org/refpages/latest/refpages/source/VkImage.html">VkImage</see></c> handle.
		/// </para>
		/// </remarks>
		public ulong VulkanTexture => texture?.Properties?.TryGetNumberValue(Texture<Vulkan>.PropertyNames.VulkanTextureNumber, out var vulkanTexture) is true
			? unchecked((ulong)vulkanTexture)
			: default;
	}
}
