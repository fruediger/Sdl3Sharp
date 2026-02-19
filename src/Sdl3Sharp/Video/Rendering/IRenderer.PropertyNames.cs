using Sdl3Sharp.Events;
using Sdl3Sharp.Video.Windowing;
using Sdl3Sharp.Video.Coloring;
using Sdl3Sharp.Video.Rendering.Drivers;

namespace Sdl3Sharp.Video.Rendering;

partial interface IRenderer
{
	/// <summary>
	/// Provides property names for <see cref="IRenderer"/> <see cref="Properties">properties</see>
	/// </summary>
	public abstract class PropertyNames
	{
		/// <summary>
		/// The name of a <see cref="Window.TryCreateRenderer(out IRenderer?, string?, ColorSpace?, RendererVSync?, Properties?)">property used when creating an <see cref="IRenderer"/></see>
		/// that holds the name of the rendering driver that the renderer should use
		/// </summary>
		/// <remarks>
		/// <para>
		/// If this property is specified when creating a renderer, SDL will attempt to use that specific rendering driver and if it fails, the renderer creation will fail.
		/// Otherwise, if this property is not specified when creating a renderer, SDL will attempt to choose the "best" rendering driver available for you.
		/// </para>
		/// </remarks>
		public const string CreateNameString = "SDL.renderer.create.name";

		/// <summary>
		/// The name of a <see cref="Window.TryCreateRenderer(out IRenderer?, string?, ColorSpace?, RendererVSync?, Properties?)">property used when creating an <see cref="IRenderer"/></see>
		/// that holds a pointer to the <see cref="Window"/> that the renderer should be associated with
		/// </summary>
		/// <remarks>
		/// <para>
		/// Specifying this property is required when creating a renderer, except when you want to create a software renderer that's associated with a <see cref="Surface"/> instead,
		/// in which case you're required to specify the <see cref="RendererExtensions.get_CreateSoftwareSurfacePointer"/> property instead.
		/// </para>
		/// <para>
		/// If you don't specify this property (or <see cref="RendererExtensions.get_CreateSoftwareSurfacePointer"/> when creating a software renderer), the renderer creation will fail.
		/// </para>
		/// </remarks>
		public const string CreateWindowPointer = "SDL.renderer.create.window";

		/// <summary>
		/// The name of a <see cref="Window.TryCreateRenderer(out IRenderer?, string?, ColorSpace?, RendererVSync?, Properties?)">property used when creating an <see cref="IRenderer"/></see>
		/// that hold the color space that the renderer should use for presenting to the output display
		/// </summary>
		/// <remarks>
		/// <para>
		/// The <see cref="Direct3D11">Direct3D 11</see>, <see cref="Direct3D12">Direct3D 12</see>, and <see cref="Metal">Metal</see> renderers support <see cref="ColorSpace.SrgbLinear"/>,
		/// which is a linear color space and supports HDR output. In that case, drawing still uses the sRGB color space, but individual values can go beyond <c>1.0</c>
		/// and floating point textures can be used for HDR content.
		/// This defaults to <see cref="ColorSpace.Srgb"/>.
		/// </para>
		/// </remarks>
		public const string CreateOutputColorSpaceNumber = "SDL.renderer.create.output_colorspace";

		/// <summary>
		/// The name of a <see cref="Window.TryCreateRenderer(out IRenderer?, string?, ColorSpace?, RendererVSync?, Properties?)">property used when creating an <see cref="IRenderer"/></see>
		/// that holds the initial vsync setting of the renderer
		/// </summary>
		/// <remarks>
		/// <para>
		/// See <see cref="RendererVSync"/> and <see cref="RendererVSyncExtensions"/> for more information about the possible values of this property and their meaning.
		/// </para>
		/// </remarks>
		public const string CreatePresentVSyncNumber = "SDL.renderer.create.present_vsync";

		/// <summary>
		/// The name of a <em>read-only</em> <see cref="Properties">property</see> that holds the name of the rendering driver of the renderer
		/// </summary>
		/// <remarks>
		/// <para>
		/// The value of the associated property can be compared against the <see cref="IDriver.Name">name</see> of any pre-defined rendering driver implementing the <see cref="IDriver"/> interface to determine whether the renderer is using that driver.
		/// </para>
		/// </remarks>
		public const string NameString = "SDL.renderer.name";

		/// <summary>
		/// The name of a <em>read-only</em> <see cref="Properties">property</see> that holds the window associated with the renderer, if any
		/// </summary>
		/// <remarks>
		/// <para>
		/// The value of the associated property can be the null pointer if the renderer is a software renderer that's associated with a <see cref="Surface"/> instead.
		/// </para>
		/// </remarks>
		public const string WindowPointer = "SDL.renderer.window";

		/// <summary>
		/// The name of a <em>read-only</em> <see cref="Properties">property</see> that holds the current vsync setting of the renderer
		/// </summary>
		/// <remarks>
		/// <para>
		/// See <see cref="RendererVSync"/> and <see cref="RendererVSyncExtensions"/> for more information about the possible values of this property and their meaning.
		/// </para>
		/// </remarks>
		public const string VSyncNumber = "SDL.renderer.vsync";

		/// <summary>
		/// The name of a <em>read-only</em> <see cref="Properties">property</see> that holds the maximum texture size supported by the renderer
		/// </summary>
		/// <remarks>
		/// <para>
		/// The value of the associated property is in regards to both width and height of the texture.
		/// E.g. if the value is <c>4096</c>, then the renderer supports textures up to <c>4096</c>⨯<c>4096</c> in size.
		/// </para>
		/// </remarks>
		public const string MaxTextureSizeNumber = "SDL.renderer.max_texture_size";

		/// <summary>
		/// The name of a <em>read-only</em> <see cref="Properties">property</see> that holds a pointer to an array of pixel formats supported by textures created by the renderer
		/// </summary>
		/// <remarks>
		/// <para>
		/// The array pointed by the value of the associated property is terminated by a <see cref="PixelFormat.Unknown"/> value.
		/// </para>
		/// </remarks>
		public const string TextureFormatsPointer = "SDL.renderer.texture_formats";

		/// <summary>
		/// The name of a <em>read-only</em> <see cref="Properties">property</see> that holds whether the renderer supports texture address wrapping on non-power-of-two textures
		/// </summary>
		/// <remarks>
		/// <para>
		/// The value of the associated property determines whether the renderer supports <see cref="Rendering.TextureAddressMode.Wrap"/> on textures that don't have power-of-two dimensions.
		/// </para>
		/// </remarks>
		public const string TextureWrappingBoolean = "SDL.renderer.texture_wrapping";

		/// <summary>
		/// The name of a <em>read-only</em> <see cref="Properties">property</see> that holds the color space used by the renderer for presenting to the output display
		/// </summary>
		/// <remarks>
		/// <para>
		/// The value of the associated property defaults to <see cref="ColorSpace.Srgb"/>.
		/// </para>
		/// </remarks>
		public const string OutputColorSpaceNumber = "SDL.renderer.output_colorspace";

		/// <summary>
		/// The name of a <em>read-only</em> <see cref="Properties">property</see> that holds whether the renderer is presenting to a display with HDR enabled
		/// </summary>
		/// <remarks>
		/// <para>
		/// The value of the associated property can change dynamically at runtime when a <see cref="EventType.WindowHdrStateChanged"/> event (<see cref="WindowEvent"/>) is sent.
		/// </para>
		/// </remarks>
		public const string HdrEnabledBoolean = "SDL.renderer.HDR_enabled";

		/// <summary>
		/// The name of a <em>read-only</em> <see cref="Properties">property</see> that holds the SDR white point in the <see cref="ColorSpace.SrgbLinear"/> color space
		/// </summary>
		/// <remarks>
		/// <para>
		/// The value of the associated property is automatically multiplied into the color scale when HDR is enabled.
		/// This property can change dynamically at runtime when a <see cref="EventType.WindowHdrStateChanged"/> event (<see cref="WindowEvent"/>) is sent.
		/// </para>
		/// </remarks>
		public const string SdrWhitePointFloat = "SDL.renderer.SDR_white_point";

		/// <summary>
		/// The name of a <em>read-only</em> <see cref="Properties">property</see> that holds the additional high dynamic range that can be displayed, in terms of the <see cref="SdrWhitePoint">SDR white point</see>
		/// </summary>
		/// <remarks>
		/// <para>
		/// The value of the associated property will be <c>1.0</c> when HDR is not enabled.
		/// This property can change dynamically at runtime when a <see cref="EventType.WindowHdrStateChanged"/> event (<see cref="WindowEvent"/>) is sent.
		/// </para>
		/// </remarks>
		public const string HdrHeadroomFloat = "SDL.renderer.HDR_headroom";

		private protected PropertyNames() { }
	}
}
