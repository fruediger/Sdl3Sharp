using Sdl3Sharp.Video.Rendering;
using Sdl3Sharp.Video.Rendering.Drivers;

namespace Sdl3Sharp.Video.Blending;

/// <summary>
/// Provides extension methods for <see cref="BlendMode"/>
/// </summary>
public static partial class BlendModeExtensions
{
	extension(BlendMode)
	{
		/// <summary>
		/// Composes a custom <see cref="BlendMode"/> for renderers
		/// </summary>
		/// <param name="sourceColorFactor">The <see cref="BlendFactor"/> applied to the red, green, and blue components of the source pixels</param>
		/// <param name="destinationColorFactor">The <see cref="BlendFactor"/> applied to the red, green, and blue components of the destination pixels</param>
		/// <param name="colorOperation">The <see cref="BlendOperation"/> used to combine the red, green, and blue components of the source and destination pixels</param>
		/// <param name="sourceAlphaFactor">The <see cref="BlendFactor"/> applied to the alpha component of the source pixels</param>
		/// <param name="destinationAlphaFactor">The <see cref="BlendFactor"/> applied to the alpha component of the destination pixels</param>
		/// <param name="alphaOperation">The <see cref="BlendOperation"/> used to combine the alpha component of the source and destination pixels</param>
		/// <returns>A custom <see cref="BlendMode"/> that represents the chosen factors and operations</returns>
		/// <remarks>
		/// <para>
		/// A custom <see cref="BlendMode"/> controls how the pixels from a drawing operation (source) get combined with the pixels from the render target (destination).
		/// First, the components of the source and destination pixels get multiplied with their blend factors.
		/// Then, the blend operation takes the two products and calculates the result that will get stored in the render target.
		/// </para>
		/// <para>
		/// Expressed in pseudocode, it would look like this:
		/// <code>
		/// dstRGB = colorOperation(srcRGB * srcColorFactor, dstRGB * dstColorFactor);
		/// dstA = alphaOperation(srcA * srcAlphaFactor, dstA * dstAlphaFactor);
		/// </code>
		/// Where the functions <c>colorOperation(src, dst)</c> and <c>alphaOperation(src, dst)</c> can return one of the following:
		/// <list type="bullet">
		/// <item><description><c>src + dst</c> (<see cref="BlendOperation.Add"/>)</description></item>
		/// <item><description><c>src - dst</c> (<see cref="BlendOperation.Subtract"/>)</description></item>
		/// <item><description><c>dst - src</c> (<see cref="BlendOperation.ReverseSubtract"/>)</description></item>
		/// <item><description><c>min(src, dst) (<see cref="BlendOperation.Minimum"/>)</c></description></item>
		/// <item><description><c>max(src, dst) (<see cref="BlendOperation.Maximum"/>)</c></description></item>
		/// </list>
		/// </para>
		/// <para>
		/// The red, green, and blue components are always multiplied with the respective factors of the <see cref="BlendFactor"/>. The alpha factor is not used.
		/// </para>
		/// <para>
		/// The alpha component is always multiplied with the alpha factor of the <see cref="BlendFactor"/>. The other factors are not used in the alpha calculation.
		/// </para>
		/// <para>
		/// Support for these custom blend modes varies for each renderer.
		/// To check if a specific <see cref="BlendMode"/> is supported, create an <see cref="IRenderer"/>, use it's <see cref="IRenderer.DrawBlendMode"/> property, and then <see langword="try"/>-<see langword="catch"/> for a <see cref="SdlException"/>.
		/// If there was an exception, check <see cref="Error.TryGet(out string?)"/> if it's about an unsupported blend mode.
		/// Alternatively, you can do the same procedure with an <see cref="ITexture"/> and it's <see cref="ITexture.BlendMode"/> property.
		/// </para>
		/// <para>
		/// The following list describes the support of custom blend modes for each rendering driver. All rendering drivers support the blend modes defined in the <see cref="BlendMode"/> enumeration.
		/// <list type="bullet">
		///		<item>
		///			<term><see cref="Direct3D9"/></term>
		///			<description>
		///			Supports all operations with all factors.
		///			However, some factors produce unexpected results with the <see cref="BlendOperation.Minimum"/> and <see cref="BlendOperation.Maximum"/> operations.
		///			</description>
		///		</item>
		///		<item>
		///			<term><see cref="Direct3D11"/></term>
		///			<description>Same as Direct3D 9.</description>
		///		</item>
		///		<item>
		///			<term><see cref="OpenGL"/></term>
		///			<description>
		///			Supports the <see cref="BlendOperation.Add"/> operation with all factors.
		///			OpenGL versions 1.1, 1.2, and 1.3 do not work correctly here.
		///			</description>
		///		</item>
		///		<item>
		///			<term><see cref="OpenGLEs2"/></term>
		///			<description>
		///			Supports the <see cref="BlendOperation.Add"/>, <see cref="BlendOperation.Subtract"/>, and <see cref="BlendOperation.ReverseSubtract"/> operations with all factors.
		///			</description>
		///		</item>
		///		<item>
		///			<term><see cref="PlayStationPortable"/></term>
		///			<description>No custom blend mode support.</description>
		///		</item>
		///		<item>
		///			<term><see cref="Software"/></term>
		///			<description>No custom blend mode support.</description>
		///		</item>
		/// </list>
		/// </para>
		/// <para>
		/// Some renderers do not provide an alpha component for the default render target.
		/// The <see cref="BlendFactor.DestinationAlpha"/> and <see cref="BlendFactor.OneMinusDestinationAlpha"/> factors do not have an effect in this case.
		/// </para>
		/// <para>
		/// The properties <see cref="IRenderer.DrawBlendMode"/> and <see cref="ITexture.BlendMode"/> accept the <see cref="BlendMode"/>s returned by this method if the renderer supports it.
		/// </para>
		/// </remarks>
		public static BlendMode ComposeCustom(
			BlendFactor sourceColorFactor,
			BlendFactor destinationColorFactor,
			BlendOperation colorOperation,
			BlendFactor sourceAlphaFactor,
			BlendFactor destinationAlphaFactor,
			BlendOperation alphaOperation)
			=> SDL_ComposeCustomBlendMode(sourceColorFactor, destinationColorFactor, colorOperation, sourceAlphaFactor, destinationAlphaFactor, alphaOperation);
	}
}
