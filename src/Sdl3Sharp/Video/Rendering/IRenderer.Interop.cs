using Sdl3Sharp.Internal;
using Sdl3Sharp.Internal.Interop;
using Sdl3Sharp.SourceGeneration;
using Sdl3Sharp.Video.Blending;
using Sdl3Sharp.Video.Coloring;
using Sdl3Sharp.Video.Drawing;
using Sdl3Sharp.Video.Gpu;
using Sdl3Sharp.Video.Windowing;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using static Sdl3Sharp.Video.Surface;

namespace Sdl3Sharp.Video.Rendering;

partial interface IRenderer
{
	// opaque struct
	[StructLayout(LayoutKind.Sequential, Size = 0)]
	internal readonly struct SDL_Renderer;

	[FormattedConstant(ErrorHelper.ParameterInvalidErrorFormat, nameof(renderer))]
	private protected unsafe static partial ReadOnlySpan<byte> GetInvalidRendererErrorMessage(SDL_Renderer* renderer = default);

#if SDL3_4_0_OR_GREATER

	/// <summary>
	/// Create a 2D GPU rendering context
	/// </summary>
	/// <param name="device">The GPU device to use with the renderer, or NULL to create a device</param>
	/// <param name="window">The window where rendering is displayed, or NULL to create an offscreen renderer</param>
	/// <returns>Returns a valid rendering context or NULL if there was an error; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// The GPU device to use is passed in as a parameter. If this is NULL, then a device will be created normally and can be retrieved using <see href="https://wiki.libsdl.org/SDL3/SDL_GetGPURendererDevice">SDL_GetGPURendererDevice</see>().
	/// </para>
	/// <para>
	/// The window to use is passed in as a parameter. If this is NULL, then this will become an offscreen renderer.
	/// In that case, you should call <see href="https://wiki.libsdl.org/SDL3/SDL_SetRenderTarget">SDL_SetRenderTarget</see>() to setup rendering to a texture, and then call <see href="https://wiki.libsdl.org/SDL3/SDL_RenderPresent">SDL_RenderPresent</see>() normally to complete drawing a frame.
	/// </para>
	/// <para>
	/// If this function is called with a valid GPU device, it should be called on the thread that created the device.
	/// If this function is called with a valid window, it should be called on the thread that created the window.
	/// </para>
	/// </remarks>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial SDL_Renderer* SDL_CreateGPURenderer(GpuDevice.SDL_GPUDevice* device, Window.SDL_Window* window);

#endif

	/// <summary>
	/// Create a 2D rendering context for a window
	/// </summary>
	/// <param name="window">The window where rendering is displayed</param>
	/// <param name="name">The name of the rendering driver to initialize, or NULL to let SDL choose one</param>
	/// <returns>Returns a valid rendering context or NULL if there was an error; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// If you want a specific renderer, you can specify its name here.
	/// A list of available renderers can be obtained by calling <see href="https://wiki.libsdl.org/SDL3/SDL_GetRenderDriver">SDL_GetRenderDriver</see>() multiple times, with indices from 0 to <see href="https://wiki.libsdl.org/SDL3/SDL_GetNumRenderDrivers">SDL_GetNumRenderDrivers</see>()-1.
	/// If you don't need a specific renderer, specify NULL and SDL will attempt to choose the best option for you, based on what is available on the user's system.
	/// </para>
	/// <para>
	/// If <c><paramref name="name"/></c> is a comma-separated list, SDL will try each name, in the order listed, until one succeeds or all of them fail.
	/// </para>
	/// <para>
	/// By default the rendering size matches the window size in pixels, but you can call <see href="https://wiki.libsdl.org/SDL3/SDL_SetRenderLogicalPresentation">SDL_SetRenderLogicalPresentation</see>() to change the content size and scaling options.
	/// </para>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_CreateRenderer">SDL_CreateRenderer</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial SDL_Renderer* SDL_CreateRenderer(Window.SDL_Window* window, byte* name);

	/// <summary>
	/// Create a 2D rendering context for a window, with the specified properties
	/// </summary>
	/// <param name="props">The properties to use</param>
	/// <returns>Returns a valid rendering context or NULL if there was an error; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// These are the supported properties:
	/// <list type="bullet">
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_RENDERER_CREATE_NAME_STRING"><c>SDL_PROP_RENDERER_CREATE_NAME_STRING</c></see></term>
	///			<description>The name of the rendering driver to use, if a specific one is desired</description>			
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_RENDERER_CREATE_WINDOW_POINTER"><c>SDL_PROP_RENDERER_CREATE_WINDOW_POINTER</c></see></term>
	///			<description>The window where rendering is displayed, required if this isn't a software renderer using a surface</description>			
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_RENDERER_CREATE_OUTPUT_COLORSPACE_NUMBER"><c>SDL_PROP_RENDERER_CREATE_OUTPUT_COLORSPACE_NUMBER</c></see></term>
	///			<description>
	///				An <see href="https://wiki.libsdl.org/SDL3/SDL_Colorspace">SDL_Colorspace</see> value describing the colorspace for output to the display, defaults to <see href="https://wiki.libsdl.org/SDL3/SDL_COLORSPACE_SRGB">SDL_COLORSPACE_SRGB</see>.
	///				The direct3d11, direct3d12, and metal renderers support <see href="https://wiki.libsdl.org/SDL3/SDL_COLORSPACE_SRGB_LINEAR">SDL_COLORSPACE_SRGB_LINEAR</see>, which is a linear color space and supports HDR output.
	///				If you select <see href="https://wiki.libsdl.org/SDL3/SDL_COLORSPACE_SRGB_LINEAR">SDL_COLORSPACE_SRGB_LINEAR</see>, drawing still uses the sRGB colorspace, but values can go beyond 1.0 and float (linear) format textures can be used for HDR content.
	///			</description>			
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_RENDERER_CREATE_PRESENT_VSYNC_NUMBER"><c>SDL_PROP_RENDERER_CREATE_PRESENT_VSYNC_NUMBER</c></see></term>
	///			<description>
	///				Non-zero if you want present synchronized with the refresh rate.
	///				This property can take any value that is supported by <see href="https://wiki.libsdl.org/SDL3/SDL_SetRenderVSync">SDL_SetRenderVSync</see>() for the renderer.
	///			</description>			
	///		</item>
	/// </list>
	/// With the SDL GPU renderer (since SDL 3.4.0):
	/// <list type="bullet">
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_RENDERER_CREATE_NAME_STRING"><c>SDL_PROP_RENDERER_CREATE_GPU_DEVICE_POINTER</c></see></term>
	///			<description>The device to use with the renderer, optional</description>			
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_RENDERER_CREATE_GPU_SHADERS_SPIRV_BOOLEAN"><c>SDL_PROP_RENDERER_CREATE_GPU_SHADERS_SPIRV_BOOLEAN</c></see></term>
	///			<description>The app is able to provide SPIR-V shaders to <see href="https://wiki.libsdl.org/SDL3/SDL_GPURenderState">SDL_GPURenderState</see>, optional</description>			
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_RENDERER_CREATE_GPU_SHADERS_DXIL_BOOLEAN"><c>SDL_PROP_RENDERER_CREATE_GPU_SHADERS_DXIL_BOOLEAN</c></see></term>
	///			<description>The app is able to provide DXIL shaders to <see href="https://wiki.libsdl.org/SDL3/SDL_GPURenderState">SDL_GPURenderState</see>, optional</description>			
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_RENDERER_CREATE_GPU_SHADERS_MSL_BOOLEAN"><c>SDL_PROP_RENDERER_CREATE_GPU_SHADERS_MSL_BOOLEAN</c></see></term>
	///			<description>The app is able to provide MSL shaders to <see href="https://wiki.libsdl.org/SDL3/SDL_GPURenderState">SDL_GPURenderState</see>, optional</description>			
	///		</item>
	/// </list>
	/// With the vulkan renderer:
	/// <list type="bullet">
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_RENDERER_CREATE_VULKAN_INSTANCE_POINTER"><c>SDL_PROP_RENDERER_CREATE_VULKAN_INSTANCE_POINTER</c></see></term>
	///			<description>The VkInstance to use with the renderer, optional</description>			
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_RENDERER_CREATE_VULKAN_SURFACE_NUMBER"><c>SDL_PROP_RENDERER_CREATE_VULKAN_SURFACE_NUMBER</c></see></term>
	///			<description>The VkSurfaceKHR to use with the renderer, optional</description>			
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_RENDERER_CREATE_VULKAN_PHYSICAL_DEVICE_POINTER"><c>SDL_PROP_RENDERER_CREATE_VULKAN_PHYSICAL_DEVICE_POINTER</c></see></term>
	///			<description>The VkPhysicalDevice to use with the renderer, optional</description>			
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_RENDERER_CREATE_VULKAN_DEVICE_POINTER"><c>SDL_PROP_RENDERER_CREATE_VULKAN_DEVICE_POINTER</c></see></term>
	///			<description>The VkDevice to use with the renderer, optional</description>			
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_RENDERER_CREATE_VULKAN_GRAPHICS_QUEUE_FAMILY_INDEX_NUMBER"><c>SDL_PROP_RENDERER_CREATE_VULKAN_GRAPHICS_QUEUE_FAMILY_INDEX_NUMBER</c></see></term>
	///			<description>The queue family index used for rendering</description>			
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_RENDERER_CREATE_VULKAN_PRESENT_QUEUE_FAMILY_INDEX_NUMBER"><c>SDL_PROP_RENDERER_CREATE_VULKAN_PRESENT_QUEUE_FAMILY_INDEX_NUMBER</c></see></term>
	///			<description>The queue family index used for presentation</description>			
	///		</item>
	/// </list>
	/// </para>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_CreateRendererWithProperties">SDL_CreateRendererWithProperties</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial SDL_Renderer* SDL_CreateRendererWithProperties(uint props);

	/// <summary>
	/// Create a 2D software rendering context for a surface
	/// </summary>
	/// <param name="surface">The <see href="https://wiki.libsdl.org/SDL3/SDL_Surface">SDL_Surface</see> structure representing the surface where rendering is done</param>
	/// <returns>Returns a valid rendering context or NULL if there was an error; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// Two other API which can be used to create <see href="https://wiki.libsdl.org/SDL3/SDL_Renderer">SDL_Renderer</see>: <see href="https://wiki.libsdl.org/SDL3/SDL_CreateRenderer">SDL_CreateRenderer</see>() and <see href="https://wiki.libsdl.org/SDL3/SDL_CreateWindowAndRenderer">SDL_CreateWindowAndRenderer</see>().
	/// These can also create a software renderer, but they are intended to be used with an <see href="https://wiki.libsdl.org/SDL3/SDL_Window">SDL_Window</see> as the final destination and not an <see href="https://wiki.libsdl.org/SDL3/SDL_Surface">SDL_Surface</see>.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_CreateSoftwareRenderer">SDL_CreateSoftwareRenderer</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial SDL_Renderer* SDL_CreateSoftwareRenderer(SDL_Surface* surface);

	/// <summary>
	/// Destroy the rendering context for a window and free all associated textures
	/// </summary>
	/// <param name="renderer">The rendering context</param>
	/// <remarks>
	/// <para>
	/// This should be called before destroying the associated window.
	/// </para>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_DestroyRenderer">SDL_DestroyRenderer</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial void SDL_DestroyRenderer(SDL_Renderer* renderer);

	/// <summary>
	/// Force the rendering context to flush any pending commands and state
	/// </summary>
	/// <param name="renderer">The rendering context</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// You do not need to (and in fact, shouldn't) call this function unless you are planning to call into OpenGL/Direct3D/Metal/whatever directly, in addition to using an <see href="https://wiki.libsdl.org/SDL3/SDL_Renderer">SDL_Renderer</see>.
	/// </para>
	/// <para>
	/// This is for a very-specific case: if you are using SDL's render API, and you plan to make OpenGL/D3D/whatever calls in addition to SDL render API calls.
	/// If this applies, you should call this function between calls to SDL's render API and the low-level API you're using in cooperation.
	/// </para>
	/// <para>
	/// In all other cases, you can ignore this function.
	/// </para>
	/// <para>
	/// This call makes SDL flush any pending rendering work it was queueing up to do later in a single batch, and marks any internal cached state as invalid, so it'll prepare all its state again later, from scratch.
	/// </para>
	/// <para>
	/// This means you do not need to save state in your rendering code to protect the SDL renderer.
	/// However, there lots of arbitrary pieces of Direct3D and OpenGL state that can confuse things; you should use your best judgment and be prepared to make changes if specific state needs to be protected.
	/// </para>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_FlushRenderer">SDL_FlushRenderer</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_FlushRenderer(SDL_Renderer* renderer);

	/// <summary>
	/// Get the current output size in pixels of a rendering context
	/// </summary>
	/// <param name="renderer">The rendering context</param>
	/// <param name="w">A pointer filled in with the current width</param>
	/// <param name="h">A pointer filled in with the current height</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// If a rendering target is active, this will return the size of the rendering target in pixels, otherwise return the value of <see href="https://wiki.libsdl.org/SDL3/SDL_GetRenderOutputSize">SDL_GetRenderOutputSize</see>().
	/// </para>
	/// <para>
	/// Rendering target or not, the output will be adjusted by the current logical presentation state, dictated by <see href="https://wiki.libsdl.org/SDL3/SDL_SetRenderLogicalPresentation">SDL_SetRenderLogicalPresentation</see>().
	/// </para>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetCurrentRenderOutputSize">SDL_GetCurrentRenderOutputSize</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_GetCurrentRenderOutputSize(SDL_Renderer* renderer, int* w, int* h);

#if SDL3_4_0_OR_GREATER

	/// <summary>
	/// Get default texture scale mode of the given renderer
	/// </summary>
	/// <param name="renderer">The renderer to get data from</param>
	/// <param name="scale_mode">A <see href="https://wiki.libsdl.org/SDL3/SDL_ScaleMode">SDL_ScaleMode</see> filled with current default scale mode. See <see href="https://wiki.libsdl.org/SDL3/SDL_SetDefaultTextureScaleMode">SDL_SetDefaultTextureScaleMode</see>() for the meaning of the value</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetDefaultTextureScaleMode">SDL_GetDefaultTextureScaleMode</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_GetDefaultTextureScaleMode(SDL_Renderer* renderer, ScaleMode* scale_mode);

#endif

	/// <summary>
	/// Get the clip rectangle for the current target
	/// </summary>
	/// <param name="renderer">The rendering context</param>
	/// <param name="rect">An <see href="https://wiki.libsdl.org/SDL3/SDL_Rect">SDL_Rect</see> structure filled in with the current clipping area or an empty rectangle if clipping is disabled</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// Each render target has its own clip rectangle. This function gets the cliprect for the current render target.
	/// </para>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetRenderClipRect">SDL_GetRenderClipRect</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_GetRenderClipRect(SDL_Renderer* renderer, Rect<int>* rect);

	/// <summary>
	/// Get the color scale used for render operations
	/// </summary>
	/// <param name="renderer">The rendering context</param>
	/// <param name="scale">A pointer filled in with the current color scale value</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// Each render target has its own clip rectangle. This function gets the cliprect for the current render target.
	/// </para>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetRenderColorScale">SDL_GetRenderColorScale</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_GetRenderColorScale(SDL_Renderer* renderer, float* scale);

	/// <summary>
	/// Get the blend mode used for drawing operations
	/// </summary>
	/// <param name="renderer">The rendering context</param>
	/// <param name="blendMode">A pointer filled in with the current <see href="https://wiki.libsdl.org/SDL3/SDL_BlendMode">SDL_BlendMode</see></param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// Each render target has its own clip rectangle. This function gets the cliprect for the current render target.
	/// </para>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetRenderDrawBlendMode">SDL_GetRenderDrawBlendMode</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_GetRenderDrawBlendMode(SDL_Renderer* renderer, BlendMode* blendMode);

	/// <summary>
	/// Get the color used for drawing operations (Rect, Line and Clear)
	/// </summary>
	/// <param name="renderer">The rendering context</param>
	/// <param name="r">A pointer filled in with the red value used to draw on the rendering target</param>
	/// <param name="g">A pointer filled in with the green value used to draw on the rendering target</param>
	/// <param name="b">A pointer filled in with the blue value used to draw on the rendering target</param>
	/// <param name="a">A pointer filled in with the alpha value used to draw on the rendering target; usually <see href="https://wiki.libsdl.org/SDL3/SDL_ALPHA_OPAQUE">SDL_ALPHA_OPAQUE</see> (255)</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// Each render target has its own clip rectangle. This function gets the cliprect for the current render target.
	/// </para>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetRenderDrawColor">SDL_GetRenderDrawColor</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_GetRenderDrawColor(SDL_Renderer* renderer, byte* r, byte* g, byte* b, byte* a);

	/// <summary>
	/// Get the color used for drawing operations (Rect, Line and Clear)
	/// </summary>
	/// <param name="renderer">The rendering context</param>
	/// <param name="r">A pointer filled in with the red value used to draw on the rendering target</param>
	/// <param name="g">A pointer filled in with the green value used to draw on the rendering target</param>
	/// <param name="b">A pointer filled in with the blue value used to draw on the rendering target</param>
	/// <param name="a">A pointer filled in with the alpha value used to draw on the rendering target</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// Each render target has its own clip rectangle. This function gets the cliprect for the current render target.
	/// </para>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetRenderDrawColorFloat">SDL_GetRenderDrawColorFloat</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_GetRenderDrawColorFloat(SDL_Renderer* renderer, float* r, float* g, float* b, float* a);

	/// <summary>
	/// Get the name of a renderer
	/// </summary>
	/// <param name="renderer">The rendering context</param>
	/// <returns>Returns the name of the selected renderer, or NULL on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetRendererName">SDL_GetRendererName</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial byte* SDL_GetRendererName(SDL_Renderer* renderer);

	/// <summary>
	/// Get the properties associated with a renderer
	/// </summary>
	/// <param name="renderer">Tthe rendering context</param>
	/// <returns>Returns a valid property ID on success or 0 on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// The following read-only properties are provided by SDL:
	/// <list type="bullet">
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_RENDERER_NAME_STRING"><c>SDL_PROP_RENDERER_NAME_STRING</c></see></term>
	///			<description>The name of the rendering driver</description>
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_RENDERER_WINDOW_POINTER"><c>SDL_PROP_RENDERER_WINDOW_POINTER</c></see></term>
	///			<description>The window where rendering is displayed, if any</description>
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_RENDERER_SURFACE_POINTER"><c>SDL_PROP_RENDERER_SURFACE_POINTER</c></see></term>
	///			<description>The surface where rendering is displayed, if this is a software renderer without a window</description>
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_RENDERER_VSYNC_NUMBER"><c>SDL_PROP_RENDERER_VSYNC_NUMBER</c></see></term>
	///			<description>The current vsync setting</description>
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_RENDERER_MAX_TEXTURE_SIZE_NUMBER"><c>SDL_PROP_RENDERER_MAX_TEXTURE_SIZE_NUMBER</c></see></term>
	///			<description>The maximum texture width and height</description>
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_RENDERER_TEXTURE_FORMATS_POINTER"><c>SDL_PROP_RENDERER_TEXTURE_FORMATS_POINTER</c></see></term>
	///			<description>
	///				A (const <see href="https://wiki.libsdl.org/SDL3/SDL_PixelFormat">SDL_PixelFormat</see> *) array of pixel formats,
	///				terminated with <see href="https://wiki.libsdl.org/SDL3/SDL_PIXELFORMAT_UNKNOWN">SDL_PIXELFORMAT_UNKNOWN</see>,
	///				representing the available texture formats for this renderer
	///			</description>
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_RENDERER_TEXTURE_WRAPPING_BOOLEAN"><c>SDL_PROP_RENDERER_TEXTURE_WRAPPING_BOOLEAN</c></see></term>
	///			<description>True if the renderer supports <see href="https://wiki.libsdl.org/SDL3/SDL_TEXTURE_ADDRESS_WRAP">SDL_TEXTURE_ADDRESS_WRAP</see> on non-power-of-two textures</description>
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_RENDERER_OUTPUT_COLORSPACE_NUMBER"><c>SDL_PROP_RENDERER_OUTPUT_COLORSPACE_NUMBER</c></see></term>
	///			<description>An <see href="https://wiki.libsdl.org/SDL3/SDL_Colorspace">SDL_Colorspace</see> value describing the colorspace for output to the display, defaults to <see href="https://wiki.libsdl.org/SDL3/SDL_COLORSPACE_SRGB">SDL_COLORSPACE_SRGB</see></description>
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_RENDERER_HDR_ENABLED_BOOLEAN"><c>SDL_PROP_RENDERER_HDR_ENABLED_BOOLEAN</c></see></term>
	///			<description>
	///				True if the output colorspace is <see href="https://wiki.libsdl.org/SDL3/SDL_COLORSPACE_SRGB_LINEAR">SDL_COLORSPACE_SRGB_LINEAR</see> and the renderer is showing on a display with HDR enabled.
	///				This property can change dynamically when <see href="https://wiki.libsdl.org/SDL3/SDL_EVENT_WINDOW_HDR_STATE_CHANGED">SDL_EVENT_WINDOW_HDR_STATE_CHANGED</see> is sent.
	///			</description>
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_RENDERER_SDR_WHITE_POINT_FLOAT"><c>SDL_PROP_RENDERER_SDR_WHITE_POINT_FLOAT</c></see></term>
	///			<description>
	///				The value of SDR white in the <see href="https://wiki.libsdl.org/SDL3/SDL_COLORSPACE_SRGB_LINEAR">SDL_COLORSPACE_SRGB_LINEAR</see> colorspace. When HDR is enabled, this value is automatically multiplied into the color scale.
	///				This property can change dynamically when <see href="https://wiki.libsdl.org/SDL3/SDL_EVENT_WINDOW_HDR_STATE_CHANGED">SDL_EVENT_WINDOW_HDR_STATE_CHANGED</see> is sent.
	///			</description>
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_RENDERER_HDR_HEADROOM_FLOAT"><c>SDL_PROP_RENDERER_HDR_HEADROOM_FLOAT</c></see></term>
	///			<description>
	///				The additional high dynamic range that can be displayed, in terms of the SDR white point. When HDR is not enabled, this will be 1.0.
	///				This property can change dynamically when <see href="https://wiki.libsdl.org/SDL3/SDL_EVENT_WINDOW_HDR_STATE_CHANGED">SDL_EVENT_WINDOW_HDR_STATE_CHANGED</see> is sent.
	///			</description>
	///		</item>
	/// </list>
	/// With the direct3d renderer:
	/// <list type="bullet">
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_RENDERER_D3D9_DEVICE_POINTER"><c>SDL_PROP_RENDERER_D3D9_DEVICE_POINTER</c></see></term>
	///			<description>The IDirect3DDevice9 associated with the renderer</description>
	///		</item>
	/// </list>
	/// With the direct3d11 renderer:
	/// <list type="bullet">
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_RENDERER_D3D11_DEVICE_POINTER"><c>SDL_PROP_RENDERER_D3D11_DEVICE_POINTER</c></see></term>
	///			<description>The ID3D11Device associated with the renderer</description>
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_RENDERER_D3D11_SWAPCHAIN_POINTER"><c>SDL_PROP_RENDERER_D3D11_SWAPCHAIN_POINTER</c></see></term>
	///			<description>The IDXGISwapChain1 associated with the renderer. This may change when the window is resized.</description>
	///		</item>
	/// </list>
	/// With the direct3d12 renderer:
	/// <list type="bullet">
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_RENDERER_D3D12_DEVICE_POINTER"><c>SDL_PROP_RENDERER_D3D12_DEVICE_POINTER</c></see></term>
	///			<description>The ID3D12Device associated with the renderer</description>
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_RENDERER_D3D12_SWAPCHAIN_POINTER"><c>SDL_PROP_RENDERER_D3D12_SWAPCHAIN_POINTER</c></see></term>
	///			<description>The IDXGISwapChain4 associated with the renderer</description>
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_RENDERER_D3D12_COMMAND_QUEUE_POINTER"><c>SDL_PROP_RENDERER_D3D12_COMMAND_QUEUE_POINTER</c></see></term>
	///			<description>The ID3D12CommandQueue associated with the renderer</description>
	///		</item>
	/// </list>
	/// With the vulkan renderer:
	/// <list type="bullet">
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_RENDERER_VULKAN_INSTANCE_POINTER"><c>SDL_PROP_RENDERER_VULKAN_INSTANCE_POINTER</c></see></term>
	///			<description>The VkInstance associated with the renderer</description>
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_RENDERER_VULKAN_SURFACE_NUMBER"><c>SDL_PROP_RENDERER_VULKAN_SURFACE_NUMBER</c></see></term>
	///			<description>The VkSurfaceKHR associated with the renderer</description>
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_RENDERER_VULKAN_PHYSICAL_DEVICE_POINTER"><c>SDL_PROP_RENDERER_VULKAN_PHYSICAL_DEVICE_POINTER</c></see></term>
	///			<description>The VkPhysicalDevice associated with the renderer</description>
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_RENDERER_VULKAN_DEVICE_POINTER"><c>SDL_PROP_RENDERER_VULKAN_DEVICE_POINTER</c></see></term>
	///			<description>The VkDevice associated with the renderer</description>
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_RENDERER_VULKAN_GRAPHICS_QUEUE_FAMILY_INDEX_NUMBER"><c>SDL_PROP_RENDERER_VULKAN_GRAPHICS_QUEUE_FAMILY_INDEX_NUMBER</c></see></term>
	///			<description>The queue family index used for rendering</description>
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_RENDERER_VULKAN_PRESENT_QUEUE_FAMILY_INDEX_NUMBER"><c>SDL_PROP_RENDERER_VULKAN_PRESENT_QUEUE_FAMILY_INDEX_NUMBER</c></see></term>
	///			<description>The queue family index used for presentation</description>
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_RENDERER_VULKAN_SWAPCHAIN_IMAGE_COUNT_NUMBER"><c>SDL_PROP_RENDERER_VULKAN_SWAPCHAIN_IMAGE_COUNT_NUMBER</c></see></term>
	///			<description>The number of swapchain images, or potential frames in flight, used by the Vulkan renderer</description>
	///		</item>
	/// </list>
	/// With the gpu renderer:
	/// <list type="bullet">
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_RENDERER_GPU_DEVICE_POINTER"><c>SDL_PROP_RENDERER_GPU_DEVICE_POINTER</c></see></term>
	///			<description>The <see href="https://wiki.libsdl.org/SDL3/SDL_GPUDevice">SDL_GPUDevice</see> associated with the renderer</description>
	///		</item>
	/// </list>
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetRendererProperties">SDL_GetRendererProperties</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial uint SDL_GetRendererProperties(SDL_Renderer* renderer);

	/// <summary>
	/// Get device independent resolution and presentation mode for rendering
	/// </summary>
	/// <param name="renderer">The rendering context</param>
	/// <param name="w">An int filled with the logical presentation width</param>
	/// <param name="h">An int filled with the logical presentation height</param>
	/// <param name="mode">A variable filled with the logical presentation mode being used</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// This function gets the width and height of the logical rendering output, or 0 if a logical resolution is not enabled.
	/// </para>
	/// <para>
	/// Each render target has its own logical presentation state. This function gets the state for the current render target.
	/// </para>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetRenderLogicalPresentation">SDL_GetRenderLogicalPresentation</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_GetRenderLogicalPresentation(SDL_Renderer* renderer, int* w, int* h, RendererLogicalPresentation* mode);

	/// <summary>
	/// Get the final presentation rectangle for rendering
	/// </summary>
	/// <param name="renderer">The rendering context</param>
	/// <param name="rect">A pointer filled in with the final presentation rectangle, may be NULL</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// This function returns the calculated rectangle used for logical presentation, based on the presentation mode and output size.
	/// If logical presentation is disabled, it will fill the rectangle with the output size, in pixels.
	/// </para>
	/// <para>
	/// Each render target has its own logical presentation state. This function gets the rectangle for the current render target.
	/// </para>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetRenderLogicalPresentationRect">SDL_GetRenderLogicalPresentationRect</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_GetRenderLogicalPresentationRect(SDL_Renderer* renderer, Rect<float>* rect);

	/// <summary>
	/// Get the output size in pixels of a rendering context
	/// </summary>
	/// <param name="renderer">The rendering context</param>
	/// <param name="w">A pointer filled in with the width in pixels</param>
	/// <param name="h">A pointer filled in with the height in pixels</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// This returns the true output size in pixels, ignoring any render targets or logical size and presentation.
	/// </para>
	/// <para>
	/// For the output size of the current rendering target, with logical size adjustments, use <see href="https://wiki.libsdl.org/SDL3/SDL_GetCurrentRenderOutputSize">SDL_GetCurrentRenderOutputSize</see>() instead.
	/// </para>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetRenderOutputSize">SDL_GetRenderOutputSize</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_GetRenderOutputSize(SDL_Renderer* renderer, int* w, int* h);

	/// <summary>
	/// Get the safe area for rendering within the current viewport
	/// </summary>
	/// <param name="renderer">The rendering context</param>
	/// <param name="rect">A pointer filled in with the area that is safe for interactive content</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// Some devices have portions of the screen which are partially obscured or not interactive, possibly due to on-screen controls, curved edges, camera notches, TV overscan, etc.
	/// This function provides the area of the current viewport which is safe to have interactible content.
	/// You should continue rendering into the rest of the render target, but it should not contain visually important or interactible content.
	/// </para>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetRenderSafeArea">SDL_GetRenderSafeArea</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_GetRenderSafeArea(SDL_Renderer* renderer, Rect<int>* rect);

	/// <summary>
	/// Get the drawing scale for the current target
	/// </summary>
	/// <param name="renderer">The rendering context</param>
	/// <param name="scaleX">A pointer filled in with the horizontal scaling factor</param>
	/// <param name="scaleY">A pointer filled in with the vertical scaling factor</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// Each render target has its own scale. This function gets the scale for the current render target.
	/// </para>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetRenderScale">SDL_GetRenderScale</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_GetRenderScale(SDL_Renderer* renderer, float* scaleX, float* scaleY);

	/// <summary>
	/// Get the current render target
	/// </summary>
	/// <param name="renderer">The rendering context</param>
	/// <returns>Returns the current render target or NULL for the default render target</returns>
	/// <remarks>
	/// <para>
	/// The default render target is the window for which the renderer was created, and is reported a NULL here.
	/// </para>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetRenderTarget">SDL_GetRenderTarget</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial ITexture.SDL_Texture* SDL_GetRenderTarget(SDL_Renderer* renderer);

#if SDL3_4_0_OR_GREATER

	/// <summary>
	/// Get the texture addressing mode used in <see href="https://wiki.libsdl.org/SDL3/SDL_RenderGeometry">SDL_RenderGeometry</see>()
	/// </summary>
	/// <param name="renderer">The rendering context</param>
	/// <param name="u_mode">A pointer filled in with the <see href="https://wiki.libsdl.org/SDL3/SDL_TextureAddressMode">SDL_TextureAddressMode</see> to use for horizontal texture coordinates in <see href="https://wiki.libsdl.org/SDL3/SDL_RenderGeometry">SDL_RenderGeometry</see>(), may be NULL</param>
	/// <param name="v_mode">A pointer filled in with the <see href="https://wiki.libsdl.org/SDL3/SDL_TextureAddressMode">SDL_TextureAddressMode</see> to use for vertical texture coordinates in <see href="https://wiki.libsdl.org/SDL3/SDL_RenderGeometry">SDL_RenderGeometry</see>(), may be NULL</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetRenderTextureAddressMode">SDL_GetRenderTextureAddressMode</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_GetRenderTextureAddressMode(SDL_Renderer* renderer, TextureAddressMode* u_mode, TextureAddressMode* v_mode);

#endif

	/// <summary>
	/// Get the drawing area for the current target
	/// </summary>
	/// <param name="renderer">The rendering context</param>
	/// <param name="rect">An <see href="https://wiki.libsdl.org/SDL3/SDL_Rect">SDL_Rect</see> structure filled in with the current drawing area</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// Each render target has its own viewport. This function gets the viewport for the current render target.
	/// </para>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetRenderViewport">SDL_GetRenderViewport</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_GetRenderViewport(SDL_Renderer* renderer, Rect<int>* rect);

	/// <summary>
	/// Get VSync of the given renderer
	/// </summary>
	/// <param name="renderer">The renderer to toggle.</param>
	/// <param name="vsync">An int filled with the current vertical refresh sync interval. See <see href="https://wiki.libsdl.org/SDL3/SDL_SetRenderVSync">SDL_SetRenderVSync</see>() for the meaning of the value.</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetRenderVSync">SDL_GetRenderVSync</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_GetRenderVSync(SDL_Renderer* renderer, RendererVSync* vsync);

	/// <summary>
	/// Get the window associated with a renderer
	/// </summary>
	/// <param name="renderer">The renderer to query.</param>
	/// <returns>Returns the window on success or NULL on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetRenderWindow">SDL_GetRenderWindow</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial Window.SDL_Window* SDL_GetRenderWindow(SDL_Renderer* renderer);

	/// <summary>
	/// Clear the current rendering target with the drawing color
	/// </summary>
	/// <param name="renderer">The rendering context</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// This function clears the entire rendering target, ignoring the viewport and the clip rectangle.
	/// Note, that clearing will also set/fill all pixels of the rendering target to current renderer draw color, so make sure to invoke <see href="https://wiki.libsdl.org/SDL3/SDL_SetRenderDrawColor">SDL_SetRenderDrawColor</see>() when needed.
	/// </para>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_RenderClear">SDL_RenderClear</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_RenderClear(SDL_Renderer* renderer);

	/// <summary>
	/// Get whether clipping is enabled on the given render target
	/// </summary>
	/// <param name="renderer">The rendering context</param>
	/// <returns>Returns true if clipping is enabled or false if not; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// Each render target has its own clip rectangle. This function checks the cliprect for the current render target.
	/// </para>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_RenderClipEnabled">SDL_RenderClipEnabled</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_RenderClipEnabled(SDL_Renderer* renderer);

	/// <summary>
	/// Get a point in render coordinates when given a point in window coordinates
	/// </summary>
	/// <param name="renderer">The rendering context</param>
	/// <param name="window_x">The x coordinate in window coordinates</param>
	/// <param name="window_y">The y coordinate in window coordinates</param>
	/// <param name="x">A pointer filled with the x coordinate in render coordinates</param>
	/// <param name="y">A pointer filled with the y coordinate in render coordinates</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// This takes into account several states:
	/// <list type="bullet">
	///		<item><description>The window dimensions</description></item>		
	///		<item><description>The logical presentation settings (<see href="https://wiki.libsdl.org/SDL3/SDL_SetRenderLogicalPresentation">SDL_SetRenderLogicalPresentation</see>)</description></item>
	///		<item><description>The scale (<see href="https://wiki.libsdl.org/SDL3/SDL_SetRenderScale">SDL_SetRenderScale</see>)</description></item>
	///		<item><description>The viewport (<see href="https://wiki.libsdl.org/SDL3/SDL_SetRenderViewport">SDL_SetRenderViewport</see>)</description></item>		
	/// </list>
	/// </para>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_RenderCoordinatesFromWindow">SDL_RenderCoordinatesFromWindow</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_RenderCoordinatesFromWindow(SDL_Renderer* renderer, float window_x, float window_y, float* x, float* y);

	/// <summary>
	/// Get a point in window coordinates when given a point in render coordinates
	/// </summary>
	/// <param name="renderer">The rendering context</param>
	/// <param name="x">The x coordinate in render coordinates</param>
	/// <param name="y">The y coordinate in render coordinates</param>
	/// <param name="window_x">A pointer filled with the x coordinate in window coordinates</param>
	/// <param name="window_y">A pointer filled with the y coordinate in window coordinates</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// This takes into account several states:
	/// <list type="bullet">
	///		<item><description>The window dimensions</description></item>		
	///		<item><description>The logical presentation settings (<see href="https://wiki.libsdl.org/SDL3/SDL_SetRenderLogicalPresentation">SDL_SetRenderLogicalPresentation</see>)</description></item>
	///		<item><description>The scale (<see href="https://wiki.libsdl.org/SDL3/SDL_SetRenderScale">SDL_SetRenderScale</see>)</description></item>
	///		<item><description>The viewport (<see href="https://wiki.libsdl.org/SDL3/SDL_SetRenderViewport">SDL_SetRenderViewport</see>)</description></item>		
	/// </list>
	/// </para>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_RenderCoordinatesToWindow">SDL_RenderCoordinatesToWindow</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_RenderCoordinatesToWindow(SDL_Renderer* renderer, float x, float y, float* window_x, float* window_y);

	/// <summary>
	/// Draw debug text to an <see href="https://wiki.libsdl.org/SDL3/SDL_Renderer">SDL_Renderer</see>
	/// </summary>
	/// <param name="renderer">The renderer which should draw a line of text</param>
	/// <param name="x">The x coordinate where the top-left corner of the text will draw</param>
	/// <param name="y">The y coordinate where the top-left corner of the text will draw</param>
	/// <param name="str">The string to render</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// This function will render a string of text to an SDL_Renderer. Note that this is a convenience function for debugging, with severe limitations, and not intended to be used for production apps and games.
	/// </para>
	/// <para>
	/// Among these limitations:
	/// <list type="bullet">
	///		<item><description>It accepts UTF-8 strings, but will only renders ASCII characters</description></item>
	///		<item><description>It has a single, tiny size (8x8 pixels). You can use logical presentation or <see href="https://wiki.libsdl.org/SDL3/SDL_SetRenderScale">SDL_SetRenderScale</see>() to adjust it.</description></item>
	///		<item><description>It uses a simple, hardcoded bitmap font. It does not allow different font selections and it does not support truetype, for proper scaling.</description></item>
	///		<item><description>It does no word-wrapping and does not treat newline characters as a line break. If the text goes out of the window, it's gone.</description></item>
	/// </list>
	/// </para>
	/// <para>
	/// For serious text rendering, there are several good options, such as <see href="https://wiki.libsdl.org/SDL3/SDL_ttf">SDL_ttf</see>, stb_truetype, or other external libraries.
	/// </para>
	/// <para>
	/// On first use, this will create an internal texture for rendering glyphs. This texture will live until the renderer is destroyed.
	/// </para>
	/// <para>
	/// The text is drawn in the color specified by <see href="https://wiki.libsdl.org/SDL3/SDL_SetRenderDrawColor">SDL_SetRenderDrawColor</see>().
	/// </para>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_RenderDebugText">SDL_RenderDebugText</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_RenderDebugText(SDL_Renderer* renderer, float x, float y, byte* str);

	/// <summary>
	/// Draw debug text to an <see href="https://wiki.libsdl.org/SDL3/SDL_Renderer">SDL_Renderer</see>
	/// </summary>
	/// <param name="renderer">The renderer which should draw a line of text</param>
	/// <param name="x">The x coordinate where the top-left corner of the text will draw</param>
	/// <param name="y">The y coordinate where the top-left corner of the text will draw</param>
	/// <param name="fmt">The format string to draw</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// This function will render a printf()-style format string to a renderer.
	/// Note that this is a convenience function for debugging, with severe limitations, and is not intended to be used for production apps and games.
	/// </para>
	/// <para>
	/// For the full list of limitations and other useful information, see <see href="https://wiki.libsdl.org/SDL3/SDL_RenderDebugText">SDL_RenderDebugText</see>.
	/// </para>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_RenderDebugTextFormat">SDL_RenderDebugTextFormat</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_RenderDebugTextFormat(SDL_Renderer* renderer, float x, float y, byte* fmt);

	/// <seealso cref="SDL_RenderDebugTextFormat(SDL_Renderer*, float, float, byte*)"/>
	[NativeImportSymbol<Library>(nameof(SDL_RenderDebugTextFormat), Kind = NativeImportSymbolKind.Reference)]
	internal static partial ref readonly byte SDL_RenderDebugTextFormat_var();

	/// <summary>
	/// Fill a rectangle on the current rendering target with the drawing color at subpixel precision
	/// </summary>
	/// <param name="renderer">The renderer which should fill a rectangle</param>
	/// <param name="rect">A pointer to the destination rectangle, or NULL for the entire rendering target</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_RenderFillRect">SDL_RenderFillRect</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_RenderFillRect(SDL_Renderer* renderer, Rect<float>* rect);

	/// <summary>
	/// Fill some number of rectangles on the current rendering target with the drawing color at subpixel precision
	/// </summary>
	/// <param name="renderer">The renderer which should fill multiple rectangles</param>
	/// <param name="rects">A pointer to an array of destination rectangles</param>
	/// <param name="count">The number of rectangles</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_RenderFillRect">SDL_RenderFillRect</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_RenderFillRects(SDL_Renderer* renderer, Rect<float>* rects, int count);

	/// <summary>
	/// Render a list of triangles, optionally using a texture and indices into the vertex array Color and alpha modulation is done per vertex
	/// (<see href="https://wiki.libsdl.org/SDL3/SDL_SetTextureColorMod">SDL_SetTextureColorMod</see> and <see href="https://wiki.libsdl.org/SDL3/SDL_SetTextureAlphaMod">SDL_SetTextureAlphaMod</see> are ignored)
	/// </summary>
	/// <param name="renderer">The rendering context</param>
	/// <param name="texture">(optional) The SDL texture to use</param>
	/// <param name="vertices">Vertices</param>
	/// <param name="num_vertices">Number of vertices</param>
	/// <param name="indices">(optional) An array of integer indices into the 'vertices' array, if NULL all vertices will be rendered in sequential order</param>
	/// <param name="num_indices">Number of indices</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_RenderGeometry">SDL_RenderGeometry</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_RenderGeometry(SDL_Renderer* renderer, ITexture.SDL_Texture* texture, Vertex* vertices, int num_vertices, int* indices, int num_indices);

	/// <summary>
	/// Render a list of triangles, optionally using a texture and indices into the vertex arrays Color and alpha modulation is done per vertex
	/// (<see href="https://wiki.libsdl.org/SDL3/SDL_SetTextureColorMod">SDL_SetTextureColorMod</see> and <see href="https://wiki.libsdl.org/SDL3/SDL_SetTextureAlphaMod">SDL_SetTextureAlphaMod</see> are ignored)
	/// </summary>
	/// <param name="renderer">The rendering context</param>
	/// <param name="texture">(optional) The SDL texture to use</param>
	/// <param name="xy">Vertex positions</param>
	/// <param name="xy_stride">Byte size to move from one element to the next element</param>
	/// <param name="color">Vertex colors (as <see href="">SDL_FColor</see>)</param>
	/// <param name="color_stride">Byte size to move from one element to the next element</param>
	/// <param name="uv">Vertex normalized texture coordinates</param>
	/// <param name="uv_stride">Byte size to move from one element to the next element</param>
	/// <param name="num_vertices">Number of vertices</param>
	/// <param name="indices">(optional) An array of indices into the 'vertices' arrays, if NULL all vertices will be rendered in sequential order</param>
	/// <param name="num_indices">Number of indices</param>
	/// <param name="size_indices">Index size: 1 (byte), 2 (short), 4 (int)</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_RenderGeometryRaw">SDL_RenderGeometryRaw</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_RenderGeometryRaw(SDL_Renderer* renderer, ITexture.SDL_Texture* texture, float* xy, int xy_stride, Color<float>* color, int color_stride, float* uv, int uv_stride, int num_vertices, void* indices, int num_indices, int size_indices);

	/// <summary>
	/// Draw a line on the current rendering target at subpixel precision
	/// </summary>
	/// <param name="renderer">The renderer which should draw a line</param>
	/// <param name="x1">The x coordinate of the start point</param>
	/// <param name="y1">The y coordinate of the start point</param>
	/// <param name="x2">The x coordinate of the end point</param>
	/// <param name="y2">The y coordinate of the end point</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_RenderLine">SDL_RenderLine</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_RenderLine(SDL_Renderer* renderer, float x1, float y1, float x2, float y2);

	/// <summary>
	/// Draw a series of connected lines on the current rendering target at subpixel precision
	/// </summary>
	/// <param name="renderer">The renderer which should draw multiple lines</param>
	/// <param name="points">The points along the lines</param>
	/// <param name="count">The number of points, drawing count-1 lines</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_RenderLines">SDL_RenderLines</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_RenderLines(SDL_Renderer* renderer, Point<float>* points, int count);

	/// <summary>
	/// Draw a point on the current rendering target at subpixel precision
	/// </summary>
	/// <param name="renderer">The renderer which should draw a point</param>
	/// <param name="x">The x coordinate of the point</param>
	/// <param name="y">The y coordinate of the point</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_RenderPoint">SDL_RenderPoint</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_RenderPoint(SDL_Renderer* renderer, float x, float y);

	/// <summary>
	/// Draw multiple points on the current rendering target at subpixel precision
	/// </summary>
	/// <param name="renderer">The renderer which should draw multiple points</param>
	/// <param name="points">The points to draw</param>
	/// <param name="count">The number of points to draw</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_RenderPoints">SDL_RenderPoints</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_RenderPoints(SDL_Renderer* renderer, Point<float>* points, int count);

	/// <summary>
	/// Update the screen with any rendering performed since the previous call
	/// </summary>
	/// <param name="renderer">The rendering context</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// SDL's rendering functions operate on a backbuffer; that is, calling a rendering function such as <see href="https://wiki.libsdl.org/SDL3/SDL_RenderLine">SDL_RenderLine</see>() does not directly put a line on the screen, but rather updates the backbuffer.
	/// As such, you compose your entire scene and present the composed backbuffer to the screen as a complete picture.
	/// </para>
	/// <para>
	/// Therefore, when using SDL's rendering API, one does all drawing intended for the frame, and then calls this function once per frame to present the final drawing to the user.
	/// </para>
	/// <para>
	/// The backbuffer should be considered invalidated after each present; do not assume that previous contents will exist between frames.
	/// You are strongly encouraged to call <see href="https://wiki.libsdl.org/SDL3/SDL_RenderClear">SDL_RenderClear</see>() to initialize the backbuffer before starting each new frame's drawing, even if you plan to overwrite every pixel.
	/// </para>
	/// <para>
	/// Please note, that in case of rendering to a texture - there is <em>no need</em> to call <see href="https://wiki.libsdl.org/SDL3/SDL_RenderPresent"><c>SDL_RenderPresent</c></see> after drawing needed objects to a texture, and should not be done;
	/// you are only required to change back the rendering target to default via <c>SDL_SetRenderTarget(renderer, NULL)</c> afterwards, as textures by themselves do not have a concept of backbuffers.
	/// Calling <see href="https://wiki.libsdl.org/SDL3/SDL_RenderPresent">SDL_RenderPresent</see> while rendering to a texture will fail.
	/// </para>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_RenderPresent">SDL_RenderPresent</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_RenderPresent(SDL_Renderer* renderer);

	/// <summary>
	/// Read pixels from the current rendering target
	/// </summary>
	/// <param name="renderer">The rendering context</param>
	/// <param name="rect">An <see href="https://wiki.libsdl.org/SDL3/SDL_Rect">SDL_Rect</see> structure representing the area to read, which will be clipped to the current viewport, or NULL for the entire viewport</param>
	/// <returns>Returns a new <see href="https://wiki.libsdl.org/SDL3/SDL_Surface">SDL_Surface</see> on success or NULL on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// The returned surface contains pixels inside the desired area clipped to the current viewport, and should be freed with <see href="https://wiki.libsdl.org/SDL3/SDL_DestroySurface">SDL_DestroySurface</see>().
	/// </para>
	/// <para>
	/// Note that this returns the actual pixels on the screen, so if you are using logical presentation you should use <see href="https://wiki.libsdl.org/SDL3/SDL_GetRenderLogicalPresentationRect">SDL_GetRenderLogicalPresentationRect</see>() to get the area containing your content.
	/// </para>
	/// <para>
	/// <em>WARNING</em>: This is a very slow operation, and should not be used frequently. If you're using this on the main rendering target, it should be called after rendering and before <see href="https://wiki.libsdl.org/SDL3/SDL_RenderPresent">SDL_RenderPresent</see>().
	/// </para>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_RenderReadPixels">SDL_RenderReadPixels</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial Surface.SDL_Surface* SDL_RenderReadPixels(SDL_Renderer* renderer, Rect<int>* rect);

	/// <summary>
	/// Draw a rectangle on the current rendering target at subpixel precision
	/// </summary>
	/// <param name="renderer">The renderer which should draw a rectangle</param>
	/// <param name="rect">A pointer to the destination rectangle, or NULL to outline the entire rendering target</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_RenderRect">SDL_RenderRect</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_RenderRect(SDL_Renderer* renderer, Rect<float>* rect);

	/// <summary>
	/// Draw some number of rectangles on the current rendering target at subpixel precision
	/// </summary>
	/// <param name="renderer">The renderer which should draw multiple rectangles</param>
	/// <param name="rects">A pointer to an array of destination rectangles</param>
	/// <param name="count">The number of rectangles</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_RenderRects">SDL_RenderRects</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_RenderRects(SDL_Renderer* renderer, Rect<float>* rects, int count);

	/// <summary>
	/// Copy a portion of the texture to the current rendering target at subpixel precision
	/// </summary>
	/// <param name="renderer">The renderer which should copy parts of a texture</param>
	/// <param name="texture">The source texture</param>
	/// <param name="srcrect">A pointer to the source rectangle, or NULL for the entire texture</param>
	/// <param name="dstrect">A pointer to the destination rectangle, or NULL for the entire rendering target</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_RenderTexture">SDL_RenderTexture</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_RenderTexture(SDL_Renderer* renderer, ITexture.SDL_Texture* texture, Rect<float>* srcrect, Rect<float>* dstrect);

	/// <summary>
	/// Perform a scaled copy using the 9-grid algorithm to the current rendering target at subpixel precision
	/// </summary>
	/// <param name="renderer">The renderer which should copy parts of a texture</param>
	/// <param name="texture">The source texture</param>
	/// <param name="srcrect">The <see href="https://wiki.libsdl.org/SDL3/SDL_Rect">SDL_Rect</see> structure representing the rectangle to be used for the 9-grid, or NULL to use the entire texture</param>
	/// <param name="left_width">The width, in pixels, of the left corners in <c><paramref name="srcrect"/></c></param>
	/// <param name="right_width">The width, in pixels, of the right corners in <c><paramref name="srcrect"/></c></param>
	/// <param name="top_height">The height, in pixels, of the top corners in <c><paramref name="srcrect"/></c></param>
	/// <param name="bottom_height">The height, in pixels, of the bottom corners in <c><paramref name="srcrect"/></c></param>
	/// <param name="scale">The scale used to transform the corner of <c><paramref name="srcrect"/></c> into the corner of <c><paramref name="dstrect"/></c>, or 0.0f for an unscaled copy</param>
	/// <param name="dstrect">A pointer to the destination rectangle, or NULL for the entire rendering target</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// The pixels in the texture are split into a 3x3 grid, using the different corner sizes for each corner, and the sides and center making up the remaining pixels.
	/// The corners are then scaled using <c><paramref name="scale"/></c> and fit into the corners of the destination rectangle.
	/// The sides and center are then stretched into place to cover the remaining destination rectangle.
	/// </para>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_RenderTexture9Grid">SDL_RenderTexture9Grid</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_RenderTexture9Grid(SDL_Renderer* renderer, ITexture.SDL_Texture* texture, Rect<float>* srcrect, float left_width, float right_width, float top_height, float bottom_height, float scale, Rect<float>* dstrect);

#if SDL3_4_0_OR_GREATER

	/// <summary>
	/// Perform a scaled copy using the 9-grid algorithm to the current rendering target at subpixel precision
	/// </summary>
	/// <param name="renderer">The renderer which should copy parts of a texture</param>
	/// <param name="texture">The source texture</param>
	/// <param name="srcrect">The <see href="https://wiki.libsdl.org/SDL3/SDL_Rect">SDL_Rect</see> structure representing the rectangle to be used for the 9-grid, or NULL to use the entire texture</param>
	/// <param name="left_width">The width, in pixels, of the left corners in <c><paramref name="srcrect"/></c></param>
	/// <param name="right_width">The width, in pixels, of the right corners in <c><paramref name="srcrect"/></c></param>
	/// <param name="top_height">The height, in pixels, of the top corners in <c><paramref name="srcrect"/></c></param>
	/// <param name="bottom_height">The height, in pixels, of the bottom corners in <c><paramref name="srcrect"/></c></param>
	/// <param name="scale">The scale used to transform the corner of <c><paramref name="srcrect"/></c> into the corner of <c><paramref name="dstrect"/></c>, or 0.0f for an unscaled copy</param>
	/// <param name="dstrect">A pointer to the destination rectangle, or NULL for the entire rendering target</param>
	/// <param name="tileScale">The scale used to transform the borders and center of <c><paramref name="srcrect"/></c> into the borders and middle of <c><paramref name="dstrect"/></c>, or 1.0f for an unscaled copy</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// The pixels in the texture are split into a 3x3 grid, using the different corner sizes for each corner, and the sides and center making up the remaining pixels.
	/// The corners are then scaled using <c><paramref name="scale"/></c> and fit into the corners of the destination rectangle.
	/// The sides and center are then tiled into place to cover the remaining destination rectangle.
	/// </para>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_RenderTexture9GridTiled">SDL_RenderTexture9GridTiled</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_RenderTexture9GridTiled(SDL_Renderer* renderer, ITexture.SDL_Texture* texture, Rect<float>* srcrect, float left_width, float right_width, float top_height, float bottom_height, float scale, Rect<float>* dstrect, float tileScale);

#endif

	/// <summary>
	/// Copy a portion of the source texture to the current rendering target, with affine transform, at subpixel precision
	/// </summary>
	/// <param name="renderer">The renderer which should copy parts of a texture</param>
	/// <param name="texture">The source texture</param>
	/// <param name="srcrect">A pointer to the source rectangle, or NULL for the entire texture</param>
	/// <param name="origin">A pointer to a point indicating where the top-left corner of srcrect should be mapped to, or NULL for the rendering target's origin</param>
	/// <param name="right">A pointer to a point indicating where the top-right corner of srcrect should be mapped to, or NULL for the rendering target's top-right corner</param>
	/// <param name="down">A pointer to a point indicating where the bottom-left corner of srcrect should be mapped to, or NULL for the rendering target's bottom-left corner</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_RenderTexture9GridTiled">SDL_RenderTexture9GridTiled</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_RenderTextureAffine(SDL_Renderer* renderer, ITexture.SDL_Texture* texture, Rect<float>* srcrect, Point<float>* origin, Point<float>* right, Point<float>* down);

	/// <summary>
	/// Copy a portion of the source texture to the current rendering target, with rotation and flipping, at subpixel precision
	/// </summary>
	/// <param name="renderer">The renderer which should copy parts of a texture</param>
	/// <param name="texture">The source texture</param>
	/// <param name="srcrect">A pointer to the source rectangle, or NULL for the entire texture</param>
	/// <param name="dstrect">A pointer to the destination rectangle, or NULL for the entire rendering target</param>
	/// <param name="angle">An angle in degrees that indicates the rotation that will be applied to dstrect, rotating it in a clockwise direction</param>
	/// <param name="center">A pointer to a point indicating the point around which dstrect will be rotated (if NULL, rotation will be done around dstrect.w/2, dstrect.h/2)</param>
	/// <param name="flip">An <see href="https://wiki.libsdl.org/SDL3/SDL_FlipMode">SDL_FlipMode</see> value stating which flipping actions should be performed on the texture</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_RenderTextureRotated">SDL_RenderTextureRotated</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_RenderTextureRotated(SDL_Renderer* renderer, ITexture.SDL_Texture* texture, Rect<float>* srcrect, Rect<float>* dstrect, double angle, Point<float>* center, FlipMode flip);

	/// <summary>
	/// Tile a portion of the texture to the current rendering target at subpixel precision
	/// </summary>
	/// <param name="renderer">The renderer which should copy parts of a texture</param>
	/// <param name="texture">The source texture</param>
	/// <param name="srcrect">A pointer to the source rectangle, or NULL for the entire texture</param>
	/// <param name="scale">The scale used to transform srcrect into the destination rectangle, e.g. a 32x32 texture with a scale of 2 would fill 64x64 tiles</param>
	/// <param name="dstrect">A pointer to the destination rectangle, or NULL for the entire rendering target</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// The pixels in <c><paramref name="srcrect"/></c> will be repeated as many times as needed to completely fill <c><paramref name="dstrect"/></c>.
	/// </para>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_RenderTextureTiled">SDL_RenderTextureTiled</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_RenderTextureTiled(SDL_Renderer* renderer, ITexture.SDL_Texture* texture, Rect<float>* srcrect, float scale, Rect<float>* dstrect);

	/// <summary>
	/// Return whether an explicit rectangle was set as the viewport
	/// </summary>
	/// <param name="renderer">The rendering context</param>
	/// <returns>Returns true if the viewport was set to a specific rectangle, or false if it was set to NULL (the entire target)</returns>
	/// <remarks>
	/// <para>
	/// This is useful if you're saving and restoring the viewport and want to know whether you should restore a specific rectangle or NULL.
	/// </para>
	/// <para>
	/// Each render target has its own viewport. This function checks the viewport for the current render target.
	/// </para>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_RenderViewportSet">SDL_RenderViewportSet</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_RenderViewportSet(SDL_Renderer* renderer);

#if SDL3_4_0_OR_GREATER

	/// <summary>
	/// Set default scale mode for new textures for given renderer
	/// </summary>
	/// <param name="renderer">The renderer to update</param>
	/// <param name="scale_mode">The scale mode to change to for new textures</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// When a renderer is created, scale_mode defaults to <see href="https://wiki.libsdl.org/SDL3/SDL_SCALEMODE_LINEAR">SDL_SCALEMODE_LINEAR</see>.
	/// </para>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_SetDefaultTextureScaleMode">SDL_SetDefaultTextureScaleMode</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_SetDefaultTextureScaleMode(SDL_Renderer* renderer, ScaleMode scale_mode);

#endif

	/// <summary>
	/// Set the clip rectangle for rendering on the specified target
	/// </summary>
	/// <param name="renderer">The rendering context</param>
	/// <param name="rect">An <see href="https://wiki.libsdl.org/SDL3/SDL_Rect">SDL_Rect</see> structure representing the clip area, relative to the viewport, or NULL to disable clipping</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// Each render target has its own clip rectangle. This function sets the cliprect for the current render target.
	/// </para>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_SetRenderClipRect">SDL_SetRenderClipRect</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_SetRenderClipRect(SDL_Renderer* renderer, Rect<int>* rect);

	/// <summary>
	/// Set the color scale used for render operations
	/// </summary>
	/// <param name="renderer">The rendering context</param>
	/// <param name="scale">The color scale value</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// The color scale is an additional scale multiplied into the pixel color value while rendering.
	/// This can be used to adjust the brightness of colors during HDR rendering, or changing HDR video brightness when playing on an SDR display.
	/// </para>
	/// <para>
	/// The color scale does not affect the alpha channel, only the color brightness.
	/// </para>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_SetRenderColorScale">SDL_SetRenderColorScale</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_SetRenderColorScale(SDL_Renderer* renderer, float scale);

	/// <summary>
	/// Set the blend mode used for drawing operations (Fill and Line)
	/// </summary>
	/// <param name="renderer">The rendering context</param>
	/// <param name="blendMode">The <see href="https://wiki.libsdl.org/SDL3/SDL_BlendMode">SDL_BlendMode</see> to use for blending</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// If the blend mode is not supported, the closest supported mode is chosen.
	/// </para>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_SetRenderDrawBlendMode">SDL_SetRenderDrawBlendMode</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_SetRenderDrawBlendMode(SDL_Renderer* renderer, BlendMode blendMode);

	/// <summary>
	/// Set the color used for drawing operations
	/// </summary>
	/// <param name="renderer">The rendering context</param>
	/// <param name="r">The red value used to draw on the rendering target</param>
	/// <param name="g">The green value used to draw on the rendering target</param>
	/// <param name="b">The blue value used to draw on the rendering target</param>
	/// <param name="a">The alpha value used to draw on the rendering target; usually <see href="https://wiki.libsdl.org/SDL3/SDL_ALPHA_OPAQUE"><c>SDL_ALPHA_OPAQUE</c></see> (255). Use <see href="https://wiki.libsdl.org/SDL3/SDL_SetRenderDrawBlendMode">SDL_SetRenderDrawBlendMode</see> to specify how the alpha channel is used.</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// Set the color for drawing or filling rectangles, lines, and points, and for <see href="https://wiki.libsdl.org/SDL3/SDL_RenderClear">SDL_RenderClear()</see>.
	/// </para>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_SetRenderDrawColor">SDL_SetRenderDrawColor</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_SetRenderDrawColor(SDL_Renderer* renderer, byte r, byte g, byte b, byte a);

	/// <summary>
	/// Set the color used for drawing operations (Rect, Line and Clear)
	/// </summary>
	/// <param name="renderer">The rendering context</param>
	/// <param name="r">The red value used to draw on the rendering target</param>
	/// <param name="g">The green value used to draw on the rendering target</param>
	/// <param name="b">The blue value used to draw on the rendering target</param>
	/// <param name="a">The alpha value used to draw on the rendering target. Use <see href="https://wiki.libsdl.org/SDL3/SDL_SetRenderDrawBlendMode">SDL_SetRenderDrawBlendMode</see> to specify how the alpha channel is used.</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// Set the color for drawing or filling rectangles, lines, and points, and for <see href="https://wiki.libsdl.org/SDL3/SDL_RenderClear">SDL_RenderClear()</see>.
	/// </para>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_SetRenderDrawColorFloat">SDL_SetRenderDrawColorFloat</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_SetRenderDrawColorFloat(SDL_Renderer* renderer, float r, float g, float b, float a);

	/// <summary>
	/// Set a device-independent resolution and presentation mode for rendering
	/// </summary>
	/// <param name="renderer">The rendering context</param>
	/// <param name="w">The width of the logical resolution</param>
	/// <param name="h">The height of the logical resolution</param>
	/// <param name="mode">The presentation mode</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// This function sets the width and height of the logical rendering output.
	/// The renderer will act as if the current render target is always the requested dimensions, scaling to the actual resolution as necessary.
	/// </para>
	/// <para>
	/// This can be useful for games that expect a fixed size, but would like to scale the output to whatever is available, regardless of how a user resizes a window, or if the display is high DPI.
	/// </para>
	/// <para>
	/// Logical presentation can be used with both render target textures and the renderer's window; the state is unique to each render target, and this function sets the state for the current render target.
	/// It might be useful to draw to a texture that matches the window dimensions with logical presentation enabled, and then draw that texture across the entire window with logical presentation disabled.
	/// Be careful not to render both with logical presentation enabled, however, as this could produce double-letterboxing, etc.
	/// </para>
	/// <para>
	/// You can disable logical coordinates by setting the mode to <see href="https://wiki.libsdl.org/SDL3/SDL_LOGICAL_PRESENTATION_DISABLED">SDL_LOGICAL_PRESENTATION_DISABLED</see>, and in that case you get the full pixel resolution of the render target;
	/// it is safe to toggle logical presentation during the rendering of a frame: perhaps most of the rendering is done to specific dimensions but to make fonts look sharp, the app turns off logical presentation while drawing text, for example.
	/// </para>
	/// <para>
	/// You can convert coordinates in an event into rendering coordinates using <see href="https://wiki.libsdl.org/SDL3/SDL_ConvertEventToRenderCoordinates">SDL_ConvertEventToRenderCoordinates</see>().
	/// </para>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_SetRenderLogicalPresentation">SDL_SetRenderLogicalPresentation</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_SetRenderLogicalPresentation(SDL_Renderer* renderer, int w, int h, RendererLogicalPresentation mode);

	/// <summary>
	/// Set the drawing scale for rendering on the current target
	/// </summary>
	/// <param name="renderer">The rendering context</param>
	/// <param name="scaleX">Zhe horizontal scaling factor</param>
	/// <param name="scaleY">The vertical scaling factor</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// The drawing coordinates are scaled by the x/y scaling factors before they are used by the renderer. This allows resolution independent drawing with a single coordinate system.
	/// </para>
	/// <para>
	/// If this results in scaling or subpixel drawing by the rendering backend, it will be handled using the appropriate quality hints. For best results use integer scaling factors.
	/// </para>
	/// <para>
	/// Each render target has its own scale. This function sets the scale for the current render target.
	/// </para>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_SetRenderScale">SDL_SetRenderScale</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_SetRenderScale(SDL_Renderer* renderer, float scaleX, float scaleY);

	/// <summary>
	/// Set a texture as the current rendering target
	/// </summary>
	/// <param name="renderer">The rendering context</param>
	/// <param name="texture">The targeted texture, which must be created with the <see href="https://wiki.libsdl.org/SDL3/SDL_TEXTUREACCESS_TARGET"><c>SDL_TEXTUREACCESS_TARGET</c></see> flag, or NULL to render to the window instead of a texture</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// The default render target is the window for which the renderer was created. To stop rendering to a texture and render to the window again, call this function with a NULL <c><paramref name="texture"/></c>.
	/// </para>
	/// <para>
	/// Viewport, cliprect, scale, and logical presentation are unique to each render target. Get and set functions for these states apply to the current render target set by this function, and those states persist on each target when the current render target changes.
	/// </para>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_SetRenderTarget">SDL_SetRenderTarget</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_SetRenderTarget(SDL_Renderer* renderer, ITexture.SDL_Texture* texture);

	/// <summary>
	/// Set the texture addressing mode used in <see href="https://wiki.libsdl.org/SDL3/SDL_RenderGeometry">SDL_RenderGeometry</see>()
	/// </summary>
	/// <param name="renderer">The rendering context</param>
	/// <param name="u_mode">The <see href="https://wiki.libsdl.org/SDL3/SDL_TextureAddressMode">SDL_TextureAddressMode</see> to use for horizontal texture coordinates in <see href="https://wiki.libsdl.org/SDL3/SDL_RenderGeometry">SDL_RenderGeometry</see>()</param>
	/// <param name="v_mode">The <see href="https://wiki.libsdl.org/SDL3/SDL_TextureAddressMode">SDL_TextureAddressMode</see> to use for vertical texture coordinates in <see href="https://wiki.libsdl.org/SDL3/SDL_RenderGeometry">SDL_RenderGeometry</see>()</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_SetRenderTarget">SDL_SetRenderTarget</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_SetRenderTextureAddressMode(SDL_Renderer* renderer, TextureAddressMode u_mode, TextureAddressMode v_mode);

	/// <summary>
	/// Set the drawing area for rendering on the current target
	/// </summary>
	/// <param name="renderer">The rendering context</param>
	/// <param name="rect">The <see href="https://wiki.libsdl.org/SDL3/SDL_Rect">SDL_Rect</see> structure representing the drawing area, or NULL to set the viewport to the entire target</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// Drawing will clip to this area (separately from any clipping done with <see href="https://wiki.libsdl.org/SDL3/SDL_SetRenderClipRect">SDL_SetRenderClipRect</see>), and the top left of the area will become coordinate (0, 0) for future drawing commands.
	/// </para>
	/// <para>
	/// The area's width and height must be >= 0.
	/// </para>
	/// <para>
	/// Each render target has its own viewport. This function sets the viewport for the current render target.
	/// </para>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_SetRenderViewport">SDL_SetRenderViewport</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_SetRenderViewport(SDL_Renderer* renderer, Rect<int>* rect);

	/// <summary>
	/// Toggle VSync of the given renderer
	/// </summary>
	/// <param name="renderer">The renderer to toggle</param>
	/// <param name="vsync">The vertical refresh sync interval</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// When a renderer is created, vsync defaults to <see href="https://wiki.libsdl.org/SDL3/SDL_RENDERER_VSYNC_DISABLED">SDL_RENDERER_VSYNC_DISABLED</see>.
	/// </para>
	/// <para>
	/// The <c><paramref name="vsync"/></c> parameter can be 1 to synchronize present with every vertical refresh, 2 to synchronize present with every second vertical refresh, etc.,
	/// <see href="https://wiki.libsdl.org/SDL3/SDL_RENDERER_VSYNC_ADAPTIVE">SDL_RENDERER_VSYNC_ADAPTIVE</see> for late swap tearing (adaptive vsync),
	/// or <see href="https://wiki.libsdl.org/SDL3/SDL_RENDERER_VSYNC_DISABLED">SDL_RENDERER_VSYNC_DISABLED</see> to disable.
	/// Not every value is supported by every driver, so you should check the return value to see whether the requested setting is supported.
	/// </para>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_SetRenderVSync">SDL_SetRenderVSync</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_SetRenderVSync(SDL_Renderer* renderer, RendererVSync vsync);
}
