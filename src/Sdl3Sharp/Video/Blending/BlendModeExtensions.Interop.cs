using Sdl3Sharp.Internal.Interop;
using Sdl3Sharp.SourceGeneration;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Video.Blending;

partial class BlendModeExtensions
{
	/// <summary>
	/// Compose a custom blend mode for renderers
	/// </summary>
	/// <param name="srcColorFactor">The <see href="https://wiki.libsdl.org/SDL3/SDL_BlendFactor">SDL_BlendFactor</see> applied to the red, green, and blue components of the source pixels</param>
	/// <param name="dstColorFactor">The <see href="https://wiki.libsdl.org/SDL3/SDL_BlendFactor">SDL_BlendFactor</see> applied to the red, green, and blue components of the destination pixels</param>
	/// <param name="colorOperation">The <see href="https://wiki.libsdl.org/SDL3/SDL_BlendOperation">SDL_BlendOperation</see> used to combine the red, green, and blue components of the source and destination pixels</param>
	/// <param name="srcAlphaFactor">The <see href="https://wiki.libsdl.org/SDL3/SDL_BlendFactor">SDL_BlendFactor</see> applied to the alpha component of the source pixels</param>
	/// <param name="dstAlphaFactor">The <see href="https://wiki.libsdl.org/SDL3/SDL_BlendFactor">SDL_BlendFactor</see> applied to the alpha component of the destination pixels</param>
	/// <param name="alphaOperation">The <see href="https://wiki.libsdl.org/SDL3/SDL_BlendOperation">SDL_BlendOperation</see> used to combine the alpha component of the source and destination pixels</param>
	/// <returns>Returns an <see href="https://wiki.libsdl.org/SDL3/SDL_BlendMode">SDL_BlendMode</see> that represents the chosen factors and operations</returns>
	/// <remarks>
	/// <para>
	/// The functions <see href="https://wiki.libsdl.org/SDL3/SDL_SetRenderDrawBlendMode">SDL_SetRenderDrawBlendMode</see> and <see href="https://wiki.libsdl.org/SDL3/SDL_SetTextureBlendMode">SDL_SetTextureBlendMode</see>
	/// accept the <see href="https://wiki.libsdl.org/SDL3/SDL_BlendMode">SDL_BlendMode</see> returned by this function if the renderer supports it.
	/// </para>
	/// <para>
	/// A blend mode controls how the pixels from a drawing operation (source) get combined with the pixels from the render target (destination).
	/// First, the components of the source and destination pixels get multiplied with their blend factors.
	/// Then, the blend operation takes the two products and calculates the result that will get stored in the render target.
	/// </para>
	/// <para>
	/// Expressed in pseudocode, it would look like this:
	/// <code>
	/// dstRGB = colorOperation(srcRGB * srcColorFactor, dstRGB * dstColorFactor);
	/// dstA = alphaOperation(srcA* srcAlphaFactor, dstA* dstAlphaFactor);
	/// </code>
	/// Where the functions <c>colorOperation(src, dst)</c> and <c>alphaOperation(src, dst)</c> can return one of the following:
	/// <list type="bullet">
	/// <item><description><c>src + dst</c></description></item>
	/// <item><description><c>src - dst</c></description></item>
	/// <item><description><c>dst - src</c></description></item>
	/// <item><description><c>min(src, dst)</c></description></item>
	/// <item><description><c>max(src, dst)</c></description></item>
	/// </list>
	/// </para>
	/// <para>
	/// The red, green, and blue components are always multiplied with the first, second, and third components of the <see href="https://wiki.libsdl.org/SDL3/SDL_BlendFactor">SDL_BlendFactor</see>, respectively. The fourth component is not used.
	/// </para>
	/// <para>
	/// The alpha component is always multiplied with the fourth component of the <see href="https://wiki.libsdl.org/SDL3/SDL_BlendFactor">SDL_BlendFactor</see>. The other components are not used in the alpha calculation.
	/// </para>
	/// <para>
	/// Support for these blend modes varies for each renderer.
	/// To check if a specific <see href="https://wiki.libsdl.org/SDL3/SDL_BlendMode">SDL_BlendMode</see> is supported,
	/// create a renderer and pass it to either <see href="https://wiki.libsdl.org/SDL3/SDL_SetRenderDrawBlendMode">SDL_SetRenderDrawBlendMode</see> or <see href="https://wiki.libsdl.org/SDL3/SDL_SetTextureBlendMode">SDL_SetTextureBlendMode</see>.
	/// They will return with an error if the blend mode is not supported.
	/// </para>
	/// <para>
	/// This list describes the support of custom blend modes for each renderer.
	/// All renderers support the four blend modes listed in the <see href="https://wiki.libsdl.org/SDL3/SDL_BlendMode">SDL_BlendMode</see> enumeration.
	/// <list type="bullet">
	///		<item>
	///			<term>direct3d</term>
	///			<description>
	///			Supports all operations with all factors.
	///			However, some factors produce unexpected results with <see href="https://wiki.libsdl.org/SDL3/SDL_BLENDOPERATION_MINIMUM">SDL_BLENDOPERATION_MINIMUM</see> and <see href="https://wiki.libsdl.org/SDL3/SDL_BLENDOPERATION_MAXIMUM">SDL_BLENDOPERATION_MAXIMUM</see>.
	///			</description>
	///		</item>
	///		<item>
	///			<term>direct3d11</term>
	///			<description>Same as Direct3D 9.</description>
	///		</item>
	///		<item>
	///			<term>opengl</term>
	///			<description>
	///			Supports the <see href="https://wiki.libsdl.org/SDL3/SDL_BLENDOPERATION_ADD">SDL_BLENDOPERATION_ADD</see> operation with all factors.
	///			OpenGL versions 1.1, 1.2, and 1.3 do not work correctly here.
	///			</description>
	///		</item>
	///		<item>
	///			<term>opengles2</term>
	///			<description>
	///			Supports the <see href="https://wiki.libsdl.org/SDL3/SDL_BLENDOPERATION_ADD">SDL_BLENDOPERATION_ADD</see>, <see href="https://wiki.libsdl.org/SDL3/SDL_BLENDOPERATION_SUBTRACT">SDL_BLENDOPERATION_SUBTRACT</see>,
	///			<see href="https://wiki.libsdl.org/SDL3/SDL_BLENDOPERATION_REV_SUBTRACT">SDL_BLENDOPERATION_REV_SUBTRACT</see> operations with all factors.
	///			</description>
	///		</item>
	///		<item>
	///			<term>psp</term>
	///			<description>No custom blend mode support.</description>
	///		</item>
	///		<item>
	///			<term>software</term>
	///			<description>No custom blend mode support.</description>
	///		</item>
	/// </list>
	/// </para>
	/// <para>
	/// Some renderers do not provide an alpha component for the default render target.
	/// The <see href="https://wiki.libsdl.org/SDL3/SDL_BLENDFACTOR_DST_ALPHA">SDL_BLENDFACTOR_DST_ALPHA</see> and <see href="https://wiki.libsdl.org/SDL3/SDL_BLENDFACTOR_ONE_MINUS_DST_ALPHA">SDL_BLENDFACTOR_ONE_MINUS_DST_ALPHA</see> factors do not have an effect in this case.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_ComposeCustomBlendMode">SDL_ComposeCustomBlendMode</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal static partial BlendMode SDL_ComposeCustomBlendMode(BlendFactor srcColorFactor, BlendFactor dstColorFactor, BlendOperation colorOperation, BlendFactor srcAlphaFactor, BlendFactor dstAlphaFactor, BlendOperation alphaOperation);
}
