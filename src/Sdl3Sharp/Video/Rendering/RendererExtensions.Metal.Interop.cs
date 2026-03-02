using Sdl3Sharp.Internal.Interop;
using Sdl3Sharp.SourceGeneration;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Video.Rendering;

partial class RendererExtensions
{
	/// <summary>
	/// Get the Metal command encoder for the current frame
	/// </summary>
	/// <param name="renderer">The renderer to query</param>
	/// <returns>Returns an <c>id&lt;MTLRenderCommandEncoder&gt;</c> on success, or NULL if the renderer isn't a Metal renderer or there was an error</returns>
	/// <remarks>
	/// <para>
	/// This function returns <c>void *</c>, so SDL doesn't have to include Metal's headers, but it can be safely cast to an <c>id&lt;MTLRenderCommandEncoder&gt;</c>.
	/// </para>
	/// <para>
	/// This will return NULL if Metal refuses to give SDL a drawable to render to, which might happen if the window is hidden/minimized/offscreen.
	/// This doesn't apply to command encoders for render targets, just the window's backbuffer.
	/// Check your return values!
	/// </para>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetRenderMetalCommandEncoder">SDL_GetRenderMetalCommandEncoder</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial void* SDL_GetRenderMetalCommandEncoder(Renderer.SDL_Renderer* renderer);

	/// <summary>
	/// Get the CAMetalLayer associated with the given Metal renderer
	/// </summary>
	/// <param name="renderer">The renderer to query</param>
	/// <returns>Returns a <c>CAMetalLayer *</c> on success, or NULL if the renderer isn't a Metal renderer</returns>
	/// <remarks>
	/// <para>
	/// This function returns <c>void *</c>, so SDL doesn't have to include Metal's headers, but it can be safely cast to a <c>CAMetalLayer *</c>.
	/// </para>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetRenderMetalLayer">SDL_GetRenderMetalLayer</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial void* SDL_GetRenderMetalLayer(Renderer.SDL_Renderer* renderer);
}
