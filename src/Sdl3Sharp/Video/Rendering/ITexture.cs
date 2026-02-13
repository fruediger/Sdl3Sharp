using Sdl3Sharp.Utilities;
using Sdl3Sharp.Video.Blending;
using Sdl3Sharp.Video.Coloring;
using Sdl3Sharp.Video.Drawing;
using Sdl3Sharp.Video.Rendering.Drivers;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Sdl3Sharp.Video.Rendering;

/// <summary>
/// Represents a texture, created by an <see cref="IRenderer"/>
/// </summary>
/// <remarks>
/// <para>
/// This is an efficient driver-specific representation of pixel data.
/// </para>
/// <para>
/// You can create new textures using the <see cref="IRenderer.TryCreateTexture(PixelFormat, TextureAccess, int, int, out ITexture?)"/>,
/// <see cref="IRenderer.TryCreateTexture(out ITexture?, ColorSpace?, PixelFormat?, TextureAccess?, int?, int?, Palette?, float?, float?, Properties?)"/>,
/// or <see cref="IRenderer.TryCreateTextureFromSurface(Surface, out ITexture?)"/> instance methods on an <see cref="IRenderer"/> instance.
/// </para>
/// <para>
/// Please remember to dispose <see cref="ITexture"/>s <em>before</em> disposing the <see cref="IRenderer"/> that created them!
/// Using an <see cref="ITexture"/> after its associated <see cref="IRenderer"/> has been disposed can lead to undefined behavior, including corruption and crashes.
/// </para>
/// <para>
/// <see cref="ITexture"/>s are not driver-agnostic! Most of the time instance of this interface are of the concrete <see cref="Texture{TDriver}"/> type with a specific <see cref="IDriver">rendering driver</see> as the type argument.
/// However, the <see cref="ITexture"/> interface exists as an abstraction to use them in common rendering operations with the <see cref="IRenderer"/> instance that created them.
/// </para>
/// <para>
/// To specify an concrete texture type, use <see cref="Texture{TDriver}"/> with a rendering driver that implements the <see cref="IDriver"/> interface (e.g. <see cref="Texture{TDriver}">Texture&lt;<see cref="OpenGl">OpenGl</see>&gt;</see>).
/// </para>
/// </remarks>
public partial interface ITexture : IDisposable
{
	/// <summary>
	/// Gets the access mode of the texture
	/// </summary>
	/// <value>
	/// The access mode of the texture
	/// </value>
	/// <remarks>
	/// <para>
	/// The value of this property determines whether the texture is lockable and/or can be used as a render target.
	/// </para>
	/// </remarks>
	TextureAccess Access { get; }

	/// <summary>
	/// Gets or sets the additional alpha modulation value multiplied into render copy operations
	/// </summary>
	/// <value>
	/// The additional alpha modulation value multiplied into render copy operations, in the range <c>0</c> to <c>255</c>, where <c>0</c> is fully transparent and <c>255</c> is fully opaque
	/// </value>
	/// <remarks>
	/// <para>
	/// When this texture is rendered, during the copy operation the source alpha value is modulated by this alpha value according to the following formula:
	/// <code>
	/// srcA = srcA * (alpha / 255)
	/// </code>
	/// </para>
	/// <para>
	/// Alpha modulation is not always supported by the renderer.
	/// </para>
	/// <para>
	/// The value of this property is equivalent to <c><see cref="AlphaModFloat"/> * 255</c>, rounded to the nearest integer.
	/// </para>
	/// <para>
	/// This property should only be accessed from the main thread.
	/// </para>
	/// </remarks>
	byte AlphaMod { get; set; }

	/// <summary>
	/// Gets or sets the additional alpha modulation value multiplied into render copy operations
	/// </summary>
	/// <value>
	/// The additional alpha modulation value multiplied into render copy operations, in the range <c>0.0</c> to <c>1.0</c>, where <c>0.0</c> is fully transparent and <c>1.0</c> is fully opaque
	/// </value>
	/// <remarks>
	/// <para>
	/// When this texture is rendered, during the copy operation the source alpha value is modulated by this alpha value according to the following formula:
	/// <code>
	/// srcA = srcA * alpha
	/// </code>
	/// </para>
	/// <para>
	/// Alpha modulation is not always supported by the renderer.
	/// </para>
	/// <para>
	/// The value of this property is equivalent to <c><see cref="AlphaMod"/> / 255</c>.
	/// </para>
	/// <para>
	/// This property should only be accessed from the main thread.
	/// </para>
	/// </remarks>
	float AlphaModFloat { get; set; }

	/// <summary>
	/// Gets or sets the blend mode used for texture copy operations
	/// </summary>
	/// <value>
	/// The blend mode used for texture copy operations
	/// </value>
	/// <remarks>
	/// <para>
	/// This property should only be accessed from the main thread.
	/// </para>
	/// </remarks>
	/// <exception cref="SdlException">
	/// When setting this property, the specified blend is <see cref="BlendMode.Invalid"/> or not supported by the renderer (check <see cref="Error.TryGet(out string?)"/> for more information)
	/// </exception>
	BlendMode BlendMode { get; set; }

	/// <summary>
	/// Gets or sets the additional color modulation value multiplied into render copy operations
	/// </summary>
	/// <value>
	/// The additional color modulation value multiplied into render copy operations
	/// </value>
	/// <remarks>
	/// <para>
	/// When this texture is rendered, during the copy operation each source color channel is modulated by the appropriate color value according to the following formula:
	/// <code>
	/// srcC = srcC * (color / 255)
	/// </code>
	/// </para>
	/// <para>
	/// Color modulation is not always supported by the renderer.
	/// </para>
	/// <para>
	/// The value of this property is equivalent to <c>(<see cref="ColorModFloat"/>.R * 255, <see cref="ColorModFloat"/>.G * 255, <see cref="ColorModFloat"/>.B * 255)</c>, each component rounded to the nearest integer.
	/// </para>
	/// <para>
	/// This property should only be accessed from the main thread.
	/// </para>
	/// </remarks>
	(byte R, byte G, byte B) ColorMod { get; set; }

	/// <summary>
	/// Gets or sets the additional color modulation value multiplied into render copy operations
	/// </summary>
	/// <value>
	/// The additional color modulation value multiplied into render copy operations
	/// </value>
	/// <remarks>
	/// <para>
	/// When this texture is rendered, during the copy operation each source color channel is modulated by the appropriate color value according to the following formula:
	/// <code>
	/// srcC = srcC * color
	/// </code>
	/// </para>
	/// <para>
	/// Color modulation is not always supported by the renderer.
	/// </para>
	/// <para>
	/// The value of this property is equivalent to <c>(<see cref="ColorMod"/>.R / 255, <see cref="ColorMod"/>.G / 255, <see cref="ColorMod"/>.B / 255)</c>.
	/// </para>
	/// <para>
	/// This property should only be accessed from the main thread.
	/// </para>
	/// </remarks>
	(float R, float G, float B) ColorModFloat { get; set; }

	/// <summary>
	/// Gets the color space of the texture
	/// </summary>
	/// <value>
	/// The color space of the texture
	/// </value>
	ColorSpace ColorSpace { get; }

	/// <summary>
	/// Gets the pixel format of the texture
	/// </summary>
	/// <value>
	/// The pixel format of the texture
	/// </value>
	PixelFormat Format { get; }

	/// <summary>
	/// Gets the maximum dynamic range, in terms of the <see cref="SdrWhitePoint">SDR white point</see>
	/// </summary>
	/// <value>
	/// The maximum dynamic range, in terms of the <see cref="SdrWhitePoint">SDR white point</see>
	/// </value>
	/// <remarks>
	/// <para>
	/// This property is used for HDR10 and floating point textures.
	/// </para>
	/// <para>
	/// The value of this property defaults to <c>1.0</c> for SDR textures, <c>4.0</c> for HDR10 textures, and has no default for floating point textures.
	/// </para>
	/// </remarks>
	float HdrHeadroom { get; }

	/// <summary>
	/// Gets the height of the texture
	/// </summary>
	/// <value>
	/// The height of the texture, in pixels
	/// </value>
	int Height { get; }

#if SDL3_4_0_OR_GREATER

	/// <summary>
	/// Gets or sets the palette used by the texture
	/// </summary>
	/// <value>
	/// The palette used by the texture, or <c><see langword="null"/></c> if the texture doesn't use a palette
	/// </value>
	/// <remarks>
	/// <para>
	/// A palette is only used by textures with an <em>indexed</em> <see cref="PixelFormat">pixel format</see>.
	/// </para>
	/// <para>
	/// A single <see cref="Coloring.Palette"/> can be shared between multiple textures.
	/// </para>
	/// </remarks>
	/// <exception cref="SdlException">
	/// When setting this property, the specified palette doesn't match the texture's pixel format (check <see cref="Error.TryGet(out string?)"/> for more information)
	/// </exception>
	Palette? Palette { get; set; }

#endif

	internal unsafe SDL_Texture* Pointer { get; }

	/// <summary>
	/// Gets the properties associated with the texture
	/// </summary>
	/// <value>
	/// The properties associated with the texture, or <c><see langword="null"/></c> if the properties could not be retrieved successfully (check <see cref="Error.TryGet(out string?)"/> for more information)
	/// </value>
	Properties? Properties { get; }

	/// <summary>
	/// Gets the renderer that created the texture
	/// </summary>
	/// <value>
	/// The renderer that created the texture, or <c><see langword="null"/></c> if the renderer could not be retrieved successfully (check <see cref="Error.TryGet(out string?)"/> for more information)
	/// </value>
	IRenderer? Renderer { get; }

	/// <summary>
	/// Gets or sets the scale mode used for texture scale operations
	/// </summary>
	/// <value>
	/// The scale mode used for texture scale operations
	/// </value>
	/// <remarks>
	/// <para>
	/// The default value of this property is <see cref="ScaleMode.Linear"/>.
	/// </para>
	/// <para>
	/// This property should only be accessed from the main thread.
	/// </para>
	/// </remarks>
	/// <exception cref="SdlException">
	/// When setting this property, the specified scale is <see cref="ScaleMode.Invalid"/> or none of the defined scale modes in <see cref="ScaleMode"/>
	/// </exception>
	ScaleMode ScaleMode { get; set; }

	/// <summary>
	/// Gets the defining value for 100% white
	/// </summary>
	/// <value>
	/// The defining value for 100% white
	/// </value>
	/// <remarks>
	/// <para>
	/// This property is used for HDR10 and floating point textures.
	/// </para>
	/// <para>
	/// The value of this property defines the value of 100% diffuse white, with higher values being displayed in the <see cref="HdrHeadroom">High Dynamic Range headroom</see>.
	/// </para>
	/// <para>
	/// The value defaults to <c>100</c> for HDR textures and <c>1.0</c> for other textures.
	/// </para>
	/// </remarks>
	float SdrWhitePoint { get; }

	/// <summary>
	/// Gets the size of the texture as floating point values
	/// </summary>
	/// <value>
	/// The size (<see cref="Width">width</see>, <see cref="Height">height</see>) of the texture as floating point values, in pixels
	/// </value>
	/// <remarks>
	/// <para>
	/// This property should only be accessed from the main thread.
	/// </para>
	/// </remarks>
	(float Width, float Height) Size { get; }

	/// <summary>
	/// Gets the width of the texture
	/// </summary>
	/// <value>
	/// The width of the texture, in pixels
	/// </value>
	int Width { get; }

	private protected void Dispose(bool disposing, bool forget);

	/// <summary>
	/// Tries to lock an area of the texture for write-only pixel access and set up a pixel memory manager
	/// </summary>
	/// <param name="rect">The area of the texture to lock for write-only pixel access</param>
	/// <param name="pixelManager">The pixel memory manager meant to be used to access the texture's pixels, if this method returns <c><see langword="true"/></c>; otherwise, <c><see langword="null"/></c></param>
	/// <returns><c><see langword="true"/></c>, if the texture was successfully locked for write-only pixel access successfully and the <paramref name="pixelManager"/> was created; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method is meant to be used as a simpler and safer alternative to using <see cref="TryUnsafeLock(in Rect{int}, out Utilities.NativeMemory, out int)"/> and <see cref="UnsafeUnlock"/>.
	/// </para>
	/// <para>
	/// This method fails and returns <c><see langword="false"/></c> if the texture's <see cref="Access"/> is not <see cref="TextureAccess.Streaming"/>.
	/// </para>
	/// <para>
	/// If the <paramref name="pixelManager"/> was created successfully, you can use its <see cref="TexturePixelMemoryManager.Memory"/> property to write the pixel memory of the locked area, using this texture's <see cref="Format"/>.
	/// Once you're done accessing the pixel memory, you should dispose the <paramref name="pixelManager"/> to unlock the texture and apply the changes.
	/// </para>
	/// <para>
	/// As an optimization, the pixels made available for editing don't necessarily contain the old texture data.
	/// This is a write-only operation, and if you need to keep a copy of the texture data you should do that at the application level.
	/// </para>
	/// <para>
	/// <em>Warning</em>: Please note that locking a texture is intended to be write-only; it will not guarantee the previous contents of the texture will be provided.
	/// You <em>must</em> fully initialize any area of a texture that you locked before unlocking it (disposing the <paramref name="pixelManager"/>), as the pixels might otherwise be uninitialized memory.
	/// E.g., if you locked a texture and immediately unlocked it again without writing any pixel data, the texture could end up in a corrupted state, depending on the renderer in use.
	/// </para>
	/// <para>
	/// This method should only be called from the main thread.
	/// </para>
	/// </remarks>
	/// <example>
	/// Example usage:
	/// <code>
	/// ITexture texture;
	/// 
	/// ...
	/// 
	/// if (texture.TryLock(in rect, out var pixelManager))
	/// {
	///		using (pixelManager)
	///		{
	///			// Write-only access to the texture's pixels as a Surface through 'pixelManager.Memory' using 'pixelManager.Pitch'
	///			// Make sure to fully initialize ALL the pixels locked
	///			
	///			...
	///		}
	///	}
	/// </code>
	/// </example>
	bool TryLock(in Rect<int> rect, [NotNullWhen(true)] out TexturePixelMemoryManager? pixelManager);

	/// <summary>
	/// Tries to lock the entire texture for write-only pixel access and set up a pixel memory manager
	/// </summary>
	/// <param name="pixelManager">The pixel memory manager meant to be used to access the texture's pixels, if this method returns <c><see langword="true"/></c>; otherwise, <c><see langword="null"/></c></param>
	/// <returns><c><see langword="true"/></c>, if the texture was successfully locked for write-only pixel access successfully and the <paramref name="pixelManager"/> was created; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method is meant to be used as a simpler and safer alternative to using <see cref="TryUnsafeLock(out Utilities.NativeMemory, out int)"/> and <see cref="UnsafeUnlock"/>.
	/// </para>
	/// <para>
	/// This method fails and returns <c><see langword="false"/></c> if the texture's <see cref="Access"/> is not <see cref="TextureAccess.Streaming"/>.
	/// </para>
	/// <para>
	/// If the <paramref name="pixelManager"/> was created successfully, you can use its <see cref="TexturePixelMemoryManager.Memory"/> property to write the pixel memory of the entire texture, using this texture's <see cref="Format"/>.
	/// Once you're done accessing the pixel memory, you should dispose the <paramref name="pixelManager"/> to unlock the texture and apply the changes.
	/// </para>
	/// <para>
	/// As an optimization, the pixels made available for editing don't necessarily contain the old texture data.
	/// This is a write-only operation, and if you need to keep a copy of the texture data you should do that at the application level.
	/// </para>
	/// <para>
	/// <em>Warning</em>: Please note that locking a texture is intended to be write-only; it will not guarantee the previous contents of the texture will be provided.
	/// You <em>must</em> fully initialize any area of a texture that you locked before unlocking it (disposing the <paramref name="pixelManager"/>), as the pixels might otherwise be uninitialized memory.
	/// E.g., if you locked a texture and immediately unlocked it again without writing any pixel data, the texture could end up in a corrupted state, depending on the renderer in use.
	/// </para>
	/// <para>
	/// This method should only be called from the main thread.
	/// </para>
	/// </remarks>
	/// <example>
	/// Example usage:
	/// <code>
	/// ITexture texture;
	/// 
	/// ...
	/// 
	/// if (texture.TryLock(out var pixelManager))
	/// {
	///		using (pixelManager)
	///		{
	///			// Write-only access to the texture's pixels as a Surface through 'pixelManager.Memory' using 'pixelManager.Pitch'
	///			// Make sure to fully initialize ALL the pixels locked
	///			
	///			...
	///		}
	///	}
	/// </code>
	/// </example>
	bool TryLock([NotNullWhen(true)] out TexturePixelMemoryManager? pixelManager);

	/// <summary>
	/// Tries to lock an area of the texture for write-only pixel access and set up a surface manager
	/// </summary>
	/// <param name="rect">The area of the texture to lock for write-only pixel access</param>
	/// <param name="surfaceManager">The surface manager meant to be used to access the texture's pixels, if this method returns <c><see langword="true"/></c>; otherwise, <c><see langword="null"/></c></param>
	/// <returns><c><see langword="true"/></c>, if the texture was successfully locked for write-only pixel access successfully and the <paramref name="surfaceManager"/> was created; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method is meant to be used as a simpler and safer alternative to using <see cref="TryUnsafeLockToSurface(in Rect{int}, out Surface?)"/> and <see cref="UnsafeUnlock"/>.
	/// </para>
	/// <para>
	/// This method fails and returns <c><see langword="false"/></c> if the texture's <see cref="Access"/> is not <see cref="TextureAccess.Streaming"/>.
	/// </para>
	/// <para>
	/// If the <paramref name="surfaceManager"/> was created successfully, you can use its <see cref="TextureSurfaceManager.Surface"/> property to access the pixels of the locked area as a <see cref="Surface"/>.
	/// Once you're done accessing the pixels, you should dispose the <paramref name="surfaceManager"/> to unlock the texture and apply the changes.
	/// </para>
	/// <para>
	/// As an optimization, the pixels made available for editing don't necessarily contain the old texture data.
	/// This is a write-only operation, and if you need to keep a copy of the texture data you should do that at the application level.
	/// </para>
	/// <para>
	/// <em>Warning</em>: Please note that locking a texture is intended to be write-only; it will not guarantee the previous contents of the texture will be provided.
	/// You <em>must</em> fully initialize any area of a texture that you locked before unlocking it (disposing the <paramref name="surfaceManager"/>), as the pixels might otherwise be uninitialized memory.
	/// E.g., if you locked a texture and immediately unlocked it again without writing any pixel data, the texture could end up in a corrupted state, depending on the renderer in use.
	/// </para>
	/// <para>
	/// This method should only be called from the main thread.
	/// </para>
	/// </remarks>
	/// <example>
	/// Example usage:
	/// <code>
	/// ITexture texture;
	/// 
	/// ...
	/// 
	/// if (texture.TryLockToSurface(in rect, out var surfaceManager))
	/// {
	///		using (surfaceManager)
	///		{
	///			// Write-only access to the texture's pixels as a Surface through 'surfaceManager.Surface'
	///			// Make sure to fully initialize ALL the pixels locked
	///			
	///			...
	///		}
	///	}
	/// </code>
	/// </example>
	bool TryLockToSurface(in Rect<int> rect, [NotNullWhen(true)] out TextureSurfaceManager? surfaceManager);

	/// <summary>
	/// Tries to lock the entire texture for write-only pixel access and set up a surface manager
	/// </summary>
	/// <param name="surfaceManager">The surface manager meant to be used to access the texture's pixels, if this method returns <c><see langword="true"/></c>; otherwise, <c><see langword="null"/></c></param>
	/// <returns><c><see langword="true"/></c>, if the texture was successfully locked for write-only pixel access successfully and the <paramref name="surfaceManager"/> was created; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method is meant to be used as a simpler and safer alternative to using <see cref="TryUnsafeLockToSurface(out Surface?)"/> and <see cref="UnsafeUnlock"/>.
	/// </para>
	/// <para>
	/// This method fails and returns <c><see langword="false"/></c> if the texture's <see cref="Access"/> is not <see cref="TextureAccess.Streaming"/>.
	/// </para>
	/// <para>
	/// If the <paramref name="surfaceManager"/> was created successfully, you can use its <see cref="TextureSurfaceManager.Surface"/> property to access the pixels of the entire texture as a <see cref="Surface"/>.
	/// Once you're done accessing the pixels, you should dispose the <paramref name="surfaceManager"/> to unlock the texture and apply the changes.
	/// </para>
	/// <para>
	/// As an optimization, the pixels made available for editing don't necessarily contain the old texture data.
	/// This is a write-only operation, and if you need to keep a copy of the texture data you should do that at the application level.
	/// </para>
	/// <para>
	/// <em>Warning</em>: Please note that locking a texture is intended to be write-only; it will not guarantee the previous contents of the texture will be provided.
	/// You <em>must</em> fully initialize any area of a texture that you locked before unlocking it (disposing the <paramref name="surfaceManager"/>), as the pixels might otherwise be uninitialized memory.
	/// E.g., if you locked a texture and immediately unlocked it again without writing any pixel data, the texture could end up in a corrupted state, depending on the renderer in use.
	/// </para>
	/// <para>
	/// This method should only be called from the main thread.
	/// </para>
	/// </remarks>
	/// <example>
	/// Example usage:
	/// <code>
	/// ITexture texture;
	/// 
	/// ...
	/// 
	/// if (texture.TryLockToSurface(out var surfaceManager))
	/// {
	///		using (surfaceManager)
	///		{
	///			// Write-only access to the texture's pixels as a Surface through 'surfaceManager.Surface'
	///			// Make sure to fully initialize ALL the pixels locked
	///			
	///			...
	///		}
	///	}
	/// </code>
	/// </example>
	bool TryLockToSurface([NotNullWhen(true)] out TextureSurfaceManager? surfaceManager);

	/// <summary>
	/// Tries to lock an area of the texture for write-only pixel access
	/// </summary>
	/// <param name="rect">The area of the texture to lock for write-only pixel access</param>
	/// <param name="pixels">The pixel memory of the locked area, if this method returns <c><see langword="true"/></c></param>
	/// <param name="pitch">
	/// The pitch of the locked area, in bytes;
	/// that is the length between the start of a row of pixels and the start of the next row of pixels (which may be greater than the width of the locked area, expressed in bytes)
	/// </param>
	/// <returns><c><see langword="true"/></c>, if the texture was successfully locked for write-only pixel access successfully; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method is meant to be used in conjuction with <see cref="UnsafeUnlock"/>, if you want to access the texture's pixels directly in a faster and more efficient way.
	/// If you're locking for a simpler and safer way to access the texture's pixels, consider using <see cref="TryLock(in Rect{int}, out TexturePixelMemoryManager)"/> instead.
	/// </para>
	/// <para>
	/// Once you're done accessing the pixel memory, you should call <see cref="UnsafeUnlock"/> to unlock the texture and apply the changes.
	/// </para>
	/// <para>
	/// This method fails and returns <c><see langword="false"/></c> if the texture's <see cref="Access"/> is not <see cref="TextureAccess.Streaming"/>.
	/// </para>
	/// <para>
	/// As an optimization, the pixels made available for editing don't necessarily contain the old texture data.
	/// This is a write-only operation, and if you need to keep a copy of the texture data you should do that at the application level.
	/// </para>
	/// <para>
	/// <em>Warning</em>: Please note that locking a texture is intended to be write-only; it will not guarantee the previous contents of the texture will be provided.
	/// You <em>must</em> fully initialize any area of a texture that you locked before unlocking it, as the pixels might otherwise be uninitialized memory.
	/// E.g., if you locked a texture and immediately unlocked it again without writing any pixel data, the texture could end up in a corrupted state, depending on the renderer in use.
	/// </para>
	/// <para>
	/// This method should only be called from the main thread.
	/// </para>
	/// </remarks>
	/// <example>
	/// Example usage:
	/// <code>
	/// ITexture texture;
	/// 
	/// ...
	/// 
	/// if (texture.TryUnsafeLock(in rect, out NativeMemory pixels, out var pitch))
	/// {
	///		// Write-only access to the pixel memory using 'pitch'
	///		// Make sure to fully initialize ALL the pixels locked
	///		
	///		...
	///		
	///		texture.UnsafeUnlock();
	/// }
	/// </code>
	/// </example>
	bool TryUnsafeLock(in Rect<int> rect, out Utilities.NativeMemory pixels, out int pitch);

	/// <summary>
	/// Tries to lock an area of the texture for write-only pixel access
	/// </summary>
	/// <param name="rect">The area of the texture to lock for write-only pixel access</param>
	/// <param name="pixels">The pixel memory of the locked area, if this method returns <c><see langword="true"/></c></param>
	/// <param name="pitch">
	/// The pitch of the locked area, in bytes;
	/// that is the length between the start of a row of pixels and the start of the next row of pixels (which may be greater than the width of the locked area, expressed in bytes)
	/// </param>
	/// <returns><c><see langword="true"/></c>, if the texture was successfully locked for write-only pixel access successfully; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method is meant to be used in conjuction with <see cref="UnsafeUnlock"/>, if you want to access the texture's pixels directly in a faster and more efficient way.
	/// If you're locking for a simpler and safer way to access the texture's pixels, consider using <see cref="TryLock(in Rect{int}, out TexturePixelMemoryManager)"/> instead.
	/// </para>
	/// <para>
	/// Once you're done accessing the pixel memory, you should call <see cref="UnsafeUnlock"/> to unlock the texture and apply the changes.
	/// </para>
	/// <para>
	/// This method fails and returns <c><see langword="false"/></c> if the texture's <see cref="Access"/> is not <see cref="TextureAccess.Streaming"/>.
	/// </para>
	/// <para>
	/// As an optimization, the pixels made available for editing don't necessarily contain the old texture data.
	/// This is a write-only operation, and if you need to keep a copy of the texture data you should do that at the application level.
	/// </para>
	/// <para>
	/// <em>Warning</em>: Please note that locking a texture is intended to be write-only; it will not guarantee the previous contents of the texture will be provided.
	/// You <em>must</em> fully initialize any area of a texture that you locked before unlocking it, as the pixels might otherwise be uninitialized memory.
	/// E.g., if you locked a texture and immediately unlocked it again without writing any pixel data, the texture could end up in a corrupted state, depending on the renderer in use.
	/// </para>
	/// <para>
	/// This method should only be called from the main thread.
	/// </para>
	/// </remarks>
	/// <example>
	/// Example usage:
	/// <code>
	/// ITexture texture;
	/// 
	/// ...
	/// 
	/// if (texture.TryUnsafeLock(in rect, out Span&lt;byte&gt; pixels, out var pitch))
	/// {
	///		// Write-only access to the pixel memory using 'pitch'
	///		// Make sure to fully initialize ALL the pixels locked
	///		
	///		...
	///		
	///		texture.UnsafeUnlock();
	/// }
	/// </code>
	/// </example>
	bool TryUnsafeLock(in Rect<int> rect, out Span<byte> pixels, out int pitch);

	/// <summary>
	/// Tries to lock an area of the texture for write-only pixel access
	/// </summary>
	/// <param name="rect">The area of the texture to lock for write-only pixel access</param>
	/// <param name="pixels">The pixel memory of the locked area, if this method returns <c><see langword="true"/></c></param>
	/// <param name="pitch">
	/// The pitch of the locked area, in bytes;
	/// that is the length between the start of a row of pixels and the start of the next row of pixels (which may be greater than the width of the locked area, expressed in bytes)
	/// </param>
	/// <returns><c><see langword="true"/></c>, if the texture was successfully locked for write-only pixel access successfully; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method is meant to be used in conjuction with <see cref="UnsafeUnlock"/>, if you want to access the texture's pixels directly in a faster and more efficient way.
	/// If you're locking for a simpler and safer way to access the texture's pixels, consider using <see cref="TryLock(in Rect{int}, out TexturePixelMemoryManager)"/> instead.
	/// </para>
	/// <para>
	/// Once you're done accessing the pixel memory, you should call <see cref="UnsafeUnlock"/> to unlock the texture and apply the changes.
	/// </para>
	/// <para>
	/// This method fails and returns <c><see langword="false"/></c> if the texture's <see cref="Access"/> is not <see cref="TextureAccess.Streaming"/>.
	/// </para>
	/// <para>
	/// As an optimization, the pixels made available for editing don't necessarily contain the old texture data.
	/// This is a write-only operation, and if you need to keep a copy of the texture data you should do that at the application level.
	/// </para>
	/// <para>
	/// <em>Warning</em>: Please note that locking a texture is intended to be write-only; it will not guarantee the previous contents of the texture will be provided.
	/// You <em>must</em> fully initialize any area of a texture that you locked before unlocking it, as the pixels might otherwise be uninitialized memory.
	/// E.g., if you locked a texture and immediately unlocked it again without writing any pixel data, the texture could end up in a corrupted state, depending on the renderer in use.
	/// </para>
	/// <para>
	/// This method should only be called from the main thread.
	/// </para>
	/// </remarks>
	/// <example>
	/// Example usage:
	/// <code>
	/// ITexture texture;
	/// 
	/// ...
	/// 
	/// if (texture.TryUnsafeLock(in rect, out void* pixels, out var pitch))
	/// {
	///		// Write-only access to the pixel memory using 'pitch'
	///		// Make sure to fully initialize ALL the pixels locked
	///		
	///		...
	///		
	///		texture.UnsafeUnlock();
	/// }
	/// </code>
	/// </example>
	unsafe bool TryUnsafeLock(in Rect<int> rect, out void* pixels, out int pitch);

	/// <summary>
	/// Tries to lock the entire texture for write-only pixel access
	/// </summary>
	/// <param name="pixels">The pixel memory of the texture, if this method returns <c><see langword="true"/></c></param>
	/// <param name="pitch">
	/// The pitch of the texture, in bytes;
	/// that is the length between the start of a row of pixels and the start of the next row of pixels (which may be greater than the width of the texture, expressed in bytes)
	/// </param>
	/// <returns><c><see langword="true"/></c>, if the texture was successfully locked for write-only pixel access successfully; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method is meant to be used in conjuction with <see cref="UnsafeUnlock"/>, if you want to access the texture's pixels directly in a faster and more efficient way.
	/// If you're locking for a simpler and safer way to access the texture's pixels, consider using <see cref="TryLock(out TexturePixelMemoryManager)"/> instead.
	/// </para>
	/// <para>
	/// Once you're done accessing the pixel memory, you should call <see cref="UnsafeUnlock"/> to unlock the texture and apply the changes.
	/// </para>
	/// <para>
	/// This method fails and returns <c><see langword="false"/></c> if the texture's <see cref="Access"/> is not <see cref="TextureAccess.Streaming"/>.
	/// </para>
	/// <para>
	/// As an optimization, the pixels made available for editing don't necessarily contain the old texture data.
	/// This is a write-only operation, and if you need to keep a copy of the texture data you should do that at the application level.
	/// </para>
	/// <para>
	/// <em>Warning</em>: Please note that locking a texture is intended to be write-only; it will not guarantee the previous contents of the texture will be provided.
	/// You <em>must</em> fully initialize any area of a texture that you locked before unlocking it, as the pixels might otherwise be uninitialized memory.
	/// E.g., if you locked a texture and immediately unlocked it again without writing any pixel data, the texture could end up in a corrupted state, depending on the renderer in use.
	/// </para>
	/// <para>
	/// This method should only be called from the main thread.
	/// </para>
	/// </remarks>
	/// <example>
	/// Example usage:
	/// <code>
	/// ITexture texture;
	/// 
	/// ...
	/// 
	/// if (texture.TryUnsafeLock(out NativeMemory pixels, out var pitch))
	/// {
	///		// Write-only access to the pixel memory using 'pitch'
	///		// Make sure to fully initialize ALL the pixels locked
	///		
	///		...
	///		
	///		texture.UnsafeUnlock();
	/// }
	/// </code>
	/// </example>
	bool TryUnsafeLock(out Utilities.NativeMemory pixels, out int pitch);

	/// <summary>
	/// Tries to lock the entire texture for write-only pixel access
	/// </summary>
	/// <param name="pixels">The pixel memory of the texture, if this method returns <c><see langword="true"/></c></param>
	/// <param name="pitch">
	/// The pitch of the texture, in bytes;
	/// that is the length between the start of a row of pixels and the start of the next row of pixels (which may be greater than the width of the texture, expressed in bytes)
	/// </param>
	/// <returns><c><see langword="true"/></c>, if the texture was successfully locked for write-only pixel access successfully; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method is meant to be used in conjuction with <see cref="UnsafeUnlock"/>, if you want to access the texture's pixels directly in a faster and more efficient way.
	/// If you're locking for a simpler and safer way to access the texture's pixels, consider using <see cref="TryLock(out TexturePixelMemoryManager)"/> instead.
	/// </para>
	/// <para>
	/// Once you're done accessing the pixel memory, you should call <see cref="UnsafeUnlock"/> to unlock the texture and apply the changes.
	/// </para>
	/// <para>
	/// This method fails and returns <c><see langword="false"/></c> if the texture's <see cref="Access"/> is not <see cref="TextureAccess.Streaming"/>.
	/// </para>
	/// <para>
	/// As an optimization, the pixels made available for editing don't necessarily contain the old texture data.
	/// This is a write-only operation, and if you need to keep a copy of the texture data you should do that at the application level.
	/// </para>
	/// <para>
	/// <em>Warning</em>: Please note that locking a texture is intended to be write-only; it will not guarantee the previous contents of the texture will be provided.
	/// You <em>must</em> fully initialize any area of a texture that you locked before unlocking it, as the pixels might otherwise be uninitialized memory.
	/// E.g., if you locked a texture and immediately unlocked it again without writing any pixel data, the texture could end up in a corrupted state, depending on the renderer in use.
	/// </para>
	/// <para>
	/// This method should only be called from the main thread.
	/// </para>
	/// </remarks>
	/// <example>
	/// Example usage:
	/// <code>
	/// ITexture texture;
	/// 
	/// ...
	/// 
	/// if (texture.TryUnsafeLock(out Span&lt;byte&gt; pixels, out var pitch))
	/// {
	///		// Write-only access to the pixel memory using 'pitch'
	///		// Make sure to fully initialize ALL the pixels locked
	///		
	///		...
	///		
	///		texture.UnsafeUnlock();
	/// }
	/// </code>
	/// </example>
	bool TryUnsafeLock(out Span<byte> pixels, out int pitch);

	/// <summary>
	/// Tries to lock the entire texture for write-only pixel access
	/// </summary>
	/// <param name="pixels">The pixel memory of the texture, if this method returns <c><see langword="true"/></c></param>
	/// <param name="pitch">
	/// The pitch of the texture, in bytes;
	/// that is the length between the start of a row of pixels and the start of the next row of pixels (which may be greater than the width of the texture, expressed in bytes)
	/// </param>
	/// <returns><c><see langword="true"/></c>, if the texture was successfully locked for write-only pixel access successfully; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method is meant to be used in conjuction with <see cref="UnsafeUnlock"/>, if you want to access the texture's pixels directly in a faster and more efficient way.
	/// If you're locking for a simpler and safer way to access the texture's pixels, consider using <see cref="TryLock(out TexturePixelMemoryManager)"/> instead.
	/// </para>
	/// <para>
	/// Once you're done accessing the pixel memory, you should call <see cref="UnsafeUnlock"/> to unlock the texture and apply the changes.
	/// </para>
	/// <para>
	/// This method fails and returns <c><see langword="false"/></c> if the texture's <see cref="Access"/> is not <see cref="TextureAccess.Streaming"/>.
	/// </para>
	/// <para>
	/// As an optimization, the pixels made available for editing don't necessarily contain the old texture data.
	/// This is a write-only operation, and if you need to keep a copy of the texture data you should do that at the application level.
	/// </para>
	/// <para>
	/// <em>Warning</em>: Please note that locking a texture is intended to be write-only; it will not guarantee the previous contents of the texture will be provided.
	/// You <em>must</em> fully initialize any area of a texture that you locked before unlocking it, as the pixels might otherwise be uninitialized memory.
	/// E.g., if you locked a texture and immediately unlocked it again without writing any pixel data, the texture could end up in a corrupted state, depending on the renderer in use.
	/// </para>
	/// <para>
	/// This method should only be called from the main thread.
	/// </para>
	/// </remarks>
	/// <example>
	/// Example usage:
	/// <code>
	/// ITexture texture;
	/// 
	/// ...
	/// 
	/// if (texture.TryUnsafeLock(out void* pixels, out var pitch))
	/// {
	///		// Write-only access to the pixel memory using 'pitch'
	///		// Make sure to fully initialize ALL the pixels locked
	///		
	///		...
	///		
	///		texture.UnsafeUnlock();
	/// }
	/// </code>
	/// </example>
	unsafe bool TryUnsafeLock(out void* pixels, out int pitch);

	/// <summary>
	/// Tries to lock an area of the texture for write-only pixel access and set up a <see cref="Surface"/> for it
	/// </summary>
	/// <param name="rect">The area of the texture to lock for write-only pixel access</param>
	/// <param name="surface">The surface meant to be used to access the texture's pixels, if this method returns <c><see langword="true"/></c>; otherwise, <c><see langword="null"/></c></param>
	/// <returns><c><see langword="true"/></c>, if the texture was successfully locked for write-only pixel access and the <see cref="Surface"/> was created; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method is meant to be used in conjuction with <see cref="UnsafeUnlock"/>, if you want to access the texture's pixels directly in a faster and more efficient way.
	/// If you're locking for a simpler and safer way to access the texture's pixels, consider using <see cref="TryLockToSurface(in Rect{int}, out TextureSurfaceManager?)"/> instead.
	/// </para>
	/// <para>
	/// Once you're done accessing the pixel memory, you should call <see cref="UnsafeUnlock"/> to unlock the texture and apply the changes.
	/// </para>
	/// <para>
	/// This method fails and returns <c><see langword="false"/></c> if the texture's <see cref="Access"/> is not <see cref="TextureAccess.Streaming"/>.
	/// </para>
	/// <para>
	/// As an optimization, the pixels made available for editing don't necessarily contain the old texture data.
	/// This is a write-only operation, and if you need to keep a copy of the texture data you should do that at the application level.
	/// </para>
	/// <para>
	/// <em>Warning</em>: Please note that locking a texture is intended to be write-only; it will not guarantee the previous contents of the texture will be provided.
	/// You <em>must</em> fully initialize any area of a texture that you locked before unlocking it, as the pixels might otherwise be uninitialized memory.
	/// E.g., if you locked a texture and immediately unlocked it again without writing any pixel data, the texture could end up in a corrupted state, depending on the renderer in use.
	/// </para>
	/// <para>
	/// This method should only be called from the main thread.
	/// </para>
	/// </remarks>
	/// <example>
	/// Example usage:
	/// <code>
	/// ITexture texture;
	///
	/// ...
	///
	/// if (texture.TryUnsafeLockToSurface(in rect, out var surface))
	/// {
	///		// Write-only access to the pixel memory using 'surface'
	///		// Make sure to fully initialize ALL the pixels locked
	///		
	///		...
	///		
	///		texture.UnsafeUnlock();
	/// }
	/// </code>
	/// </example>
	bool TryUnsafeLockToSurface(in Rect<int> rect, [NotNullWhen(true)] out Surface? surface);

	/// <summary>
	/// Tries to lock the entire texture for write-only pixel access and set up a <see cref="Surface"/> for it
	/// </summary>
	/// <param name="surface">The surface meant to be used to access the texture's pixels, if this method returns <c><see langword="true"/></c>; otherwise, <c><see langword="null"/></c></param>
	/// <returns><c><see langword="true"/></c>, if the texture was successfully locked for write-only pixel access and the <see cref="Surface"/> was created; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method is meant to be used in conjuction with <see cref="UnsafeUnlock"/>, if you want to access the texture's pixels directly in a faster and more efficient way.
	/// If you're locking for a simpler and safer way to access the texture's pixels, consider using <see cref="TryLockToSurface(out TextureSurfaceManager?)"/> instead.
	/// </para>
	/// <para>
	/// Once you're done accessing the pixel memory, you should call <see cref="UnsafeUnlock"/> to unlock the texture and apply the changes.
	/// </para>
	/// <para>
	/// This method fails and returns <c><see langword="false"/></c> if the texture's <see cref="Access"/> is not <see cref="TextureAccess.Streaming"/>.
	/// </para>
	/// <para>
	/// As an optimization, the pixels made available for editing don't necessarily contain the old texture data.
	/// This is a write-only operation, and if you need to keep a copy of the texture data you should do that at the application level.
	/// </para>
	/// <para>
	/// <em>Warning</em>: Please note that locking a texture is intended to be write-only; it will not guarantee the previous contents of the texture will be provided.
	/// You <em>must</em> fully initialize any area of a texture that you locked before unlocking it, as the pixels might otherwise be uninitialized memory.
	/// E.g., if you locked a texture and immediately unlocked it again without writing any pixel data, the texture could end up in a corrupted state, depending on the renderer in use.
	/// </para>
	/// <para>
	/// This method should only be called from the main thread.
	/// </para>
	/// </remarks>
	/// <example>
	/// Example usage:
	/// <code>
	/// ITexture texture;
	///
	/// ...
	///
	/// if (texture.TryUnsafeLockToSurface(out var surface))
	/// {
	///		// Write-only access to the pixel memory using 'surface'
	///		// Make sure to fully initialize ALL the pixels locked
	///		
	///		...
	///		
	///		texture.UnsafeUnlock();
	/// }
	/// </code>
	/// </example>
	bool TryUnsafeLockToSurface([NotNullWhen(true)] out Surface? surface);

	/// <summary>
	/// Tries to update an area of the texture with new pixel data
	/// </summary>
	/// <param name="rect">The area of the texture to update with the new pixel data</param>
	/// <param name="pixels">The new pixel data to be copied into the specified area of the texture, in the <see cref="Format">pixel format</see> of the texture</param>
	/// <param name="pitch">
	/// The pitch used in the given <paramref name="pixels"/> data, in bytes;
	/// that is the length between the start of a row of pixels and the start of the next row of pixels (which may be greater than the pixel width of the given <paramref name="pixels"/> data, expressed in bytes)
	/// </param>
	/// <returns><c><see langword="true"/></c>, if the texture was successfully updated with the new pixel data; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// The given <paramref name="pixels"/> data must be in the <see cref="Format">pixel format</see> of the texture.
	/// </para>
	/// <para>
	/// This is a fairly slow operation, intended for use with <see cref="TextureAccess.Static">static</see> textures that do not change often.
	/// </para>
	/// <para>
	/// If the texture is intended to be updated often, you should prefer to create the texture as <see cref="TextureAccess.Streaming">streaming</see> and use one of the locking mechanisms to manipulate the texture's pixels (e.g. <see cref="TryLock(in Rect{int}, out TexturePixelMemoryManager)"/>) instead.
	/// While this method still works with <see cref="TextureAccess.Streaming">streaming</see> textures, for optimization reasons you may not get the pixels back if you lock the texture afterward.
	/// </para>
	/// <para>
	/// Using this method for NV12, NV21, YV12 or IYUV textures is perfectly fine as long as the given <paramref name="pixels"/> data is a contiguous block of NV12/N21 planes or Y and UV planes, respectively, in the proper order.
	/// If not, you should use <see cref="TryUpdateNv(in Rect{int}, ReadOnlyNativeMemory{byte}, int, ReadOnlyNativeMemory{byte}, int)"/> or <see cref="TryUpdateYuv(in Rect{int}, ReadOnlyNativeMemory{byte}, int, ReadOnlyNativeMemory{byte}, int, ReadOnlyNativeMemory{byte}, int)"/>, respectively, instead.
	/// </para>
	/// <para>
	/// This method should only be called from the main thread.
	/// </para>
	/// </remarks>
	bool TryUpdate(in Rect<int> rect, ReadOnlyNativeMemory pixels, int pitch);

	/// <summary>
	/// Tries to update an area of the texture with new pixel data
	/// </summary>
	/// <param name="rect">The area of the texture to update with the new pixel data</param>
	/// <param name="pixels">The new pixel data to be copied into the specified area of the texture, in the <see cref="Format">pixel format</see> of the texture</param>
	/// <param name="pitch">
	/// The pitch used in the given <paramref name="pixels"/> data, in bytes;
	/// that is the length between the start of a row of pixels and the start of the next row of pixels (which may be greater than the pixel width of the given <paramref name="pixels"/> data, expressed in bytes)
	/// </param>
	/// <returns><c><see langword="true"/></c>, if the texture was successfully updated with the new pixel data; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// The given <paramref name="pixels"/> data must be in the <see cref="Format">pixel format</see> of the texture.
	/// </para>
	/// <para>
	/// This is a fairly slow operation, intended for use with <see cref="TextureAccess.Static">static</see> textures that do not change often.
	/// </para>
	/// <para>
	/// If the texture is intended to be updated often, you should prefer to create the texture as <see cref="TextureAccess.Streaming">streaming</see> and use one of the locking mechanisms to manipulate the texture's pixels (e.g. <see cref="TryLock(in Rect{int}, out TexturePixelMemoryManager)"/>) instead.
	/// While this method still works with <see cref="TextureAccess.Streaming">streaming</see> textures, for optimization reasons you may not get the pixels back if you lock the texture afterward.
	/// </para>
	/// <para>
	/// Using this method for NV12, NV21, YV12 or IYUV textures is perfectly fine as long as the given <paramref name="pixels"/> data is a contiguous block of NV12/N21 planes or Y and UV planes, respectively, in the proper order.
	/// If not, you should use <see cref="TryUpdateNv(in Rect{int}, ReadOnlySpan{byte}, int, ReadOnlySpan{byte}, int)"/> or <see cref="TryUpdateYuv(in Rect{int}, ReadOnlySpan{byte}, int, ReadOnlySpan{byte}, int, ReadOnlySpan{byte}, int)"/>, respectively, instead.
	/// </para>
	/// <para>
	/// This method should only be called from the main thread.
	/// </para>
	/// </remarks>
	bool TryUpdate(in Rect<int> rect, ReadOnlySpan<byte> pixels, int pitch);

	/// <summary>
	/// Tries to update an area of the texture with new pixel data
	/// </summary>
	/// <param name="rect">The area of the texture to update with the new pixel data</param>
	/// <param name="pixels">The new pixel data to be copied into the specified area of the texture, in the <see cref="Format">pixel format</see> of the texture</param>
	/// <param name="pitch">
	/// The pitch used in the given <paramref name="pixels"/> data, in bytes;
	/// that is the length between the start of a row of pixels and the start of the next row of pixels (which may be greater than the pixel width of the given <paramref name="pixels"/> data, expressed in bytes)
	/// </param>
	/// <returns><c><see langword="true"/></c>, if the texture was successfully updated with the new pixel data; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// The given <paramref name="pixels"/> data must be in the <see cref="Format">pixel format</see> of the texture.
	/// </para>
	/// <para>
	/// This is a fairly slow operation, intended for use with <see cref="TextureAccess.Static">static</see> textures that do not change often.
	/// </para>
	/// <para>
	/// If the texture is intended to be updated often, you should prefer to create the texture as <see cref="TextureAccess.Streaming">streaming</see> and use one of the locking mechanisms to manipulate the texture's pixels (e.g. <see cref="TryLock(in Rect{int}, out TexturePixelMemoryManager)"/>) instead.
	/// While this method still works with <see cref="TextureAccess.Streaming">streaming</see> textures, for optimization reasons you may not get the pixels back if you lock the texture afterward.
	/// </para>
	/// <para>
	/// Using this method for NV12, NV21, YV12 or IYUV textures is perfectly fine as long as the given <paramref name="pixels"/> data is a contiguous block of NV12/N21 planes or Y and UV planes, respectively, in the proper order.
	/// If not, you should use <see cref="TryUpdateYuv(in Rect{int}, byte*, int, byte*, int, byte*, int)"/> or <see cref="TryUpdateYuv(in Rect{int}, byte*, int, byte*, int, byte*, int)"/>, respectively, instead.
	/// </para>
	/// <para>
	/// This method should only be called from the main thread.
	/// </para>
	/// </remarks>
	unsafe bool TryUpdate(in Rect<int> rect, void* pixels, int pitch);

	/// <summary>
	/// Tries to update the entire texture with new pixel data
	/// </summary>
	/// <param name="pixels">The new pixel data to be copied into the entirety of the texture, in the <see cref="Format">pixel format</see> of the texture</param>
	/// <param name="pitch">
	/// The pitch used in the given <paramref name="pixels"/> data, in bytes;
	/// that is the length between the start of a row of pixels and the start of the next row of pixels (which may be greater than the pixel width of the given <paramref name="pixels"/> data, expressed in bytes)
	/// </param>
	/// <returns><c><see langword="true"/></c>, if the texture was successfully updated with the new pixel data; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// The given <paramref name="pixels"/> data must be in the <see cref="Format">pixel format</see> of the texture.
	/// </para>
	/// <para>
	/// This is a fairly slow operation, intended for use with <see cref="TextureAccess.Static">static</see> textures that do not change often.
	/// </para>
	/// <para>
	/// If the texture is intended to be updated often, you should prefer to create the texture as <see cref="TextureAccess.Streaming">streaming</see> and use one of the locking mechanisms to manipulate the texture's pixels (e.g. <see cref="TryLock(out TexturePixelMemoryManager)"/>) instead.
	/// While this method still works with <see cref="TextureAccess.Streaming">streaming</see> textures, for optimization reasons you may not get the pixels back if you lock the texture afterward.
	/// </para>
	/// <para>
	/// Using this method for NV12, NV21, YV12 or IYUV textures is perfectly fine as long as the given <paramref name="pixels"/> data is a contiguous block of NV12/N21 planes or Y and UV planes, respectively, in the proper order.
	/// If not, you should use <see cref="TryUpdateNv(ReadOnlyNativeMemory{byte}, int, ReadOnlyNativeMemory{byte}, int)"/> or <see cref="TryUpdateYuv(ReadOnlyNativeMemory{byte}, int, ReadOnlyNativeMemory{byte}, int, ReadOnlyNativeMemory{byte}, int)"/>, respectively, instead.
	/// </para>
	/// <para>
	/// This method should only be called from the main thread.
	/// </para>
	/// </remarks>
	bool TryUpdate(ReadOnlyNativeMemory pixels, int pitch);

	/// <summary>
	/// Tries to update the entire texture with new pixel data
	/// </summary>
	/// <param name="pixels">The new pixel data to be copied into the entirety of the texture, in the <see cref="Format">pixel format</see> of the texture</param>
	/// <param name="pitch">
	/// The pitch used in the given <paramref name="pixels"/> data, in bytes;
	/// that is the length between the start of a row of pixels and the start of the next row of pixels (which may be greater than the pixel width of the given <paramref name="pixels"/> data, expressed in bytes)
	/// </param>
	/// <returns><c><see langword="true"/></c>, if the texture was successfully updated with the new pixel data; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// The given <paramref name="pixels"/> data must be in the <see cref="Format">pixel format</see> of the texture.
	/// </para>
	/// <para>
	/// This is a fairly slow operation, intended for use with <see cref="TextureAccess.Static">static</see> textures that do not change often.
	/// </para>
	/// <para>
	/// If the texture is intended to be updated often, you should prefer to create the texture as <see cref="TextureAccess.Streaming">streaming</see> and use one of the locking mechanisms to manipulate the texture's pixels (e.g. <see cref="TryLock(out TexturePixelMemoryManager)"/>) instead.
	/// While this method still works with <see cref="TextureAccess.Streaming">streaming</see> textures, for optimization reasons you may not get the pixels back if you lock the texture afterward.
	/// </para>
	/// <para>
	/// Using this method for NV12, NV21, YV12 or IYUV textures is perfectly fine as long as the given <paramref name="pixels"/> data is a contiguous block of NV12/N21 planes or Y and UV planes, respectively, in the proper order.
	/// If not, you should use <see cref="TryUpdateNv(ReadOnlySpan{byte}, int, ReadOnlySpan{byte}, int)"/> or <see cref="TryUpdateYuv(ReadOnlySpan{byte}, int, ReadOnlySpan{byte}, int, ReadOnlySpan{byte}, int)"/>, respectively, instead.
	/// </para>
	/// <para>
	/// This method should only be called from the main thread.
	/// </para>
	/// </remarks>
	bool TryUpdate(ReadOnlySpan<byte> pixels, int pitch);

	/// <summary>
	/// Tries to update the entire texture with new pixel data
	/// </summary>
	/// <param name="pixels">The new pixel data to be copied into the entirety of the texture, in the <see cref="Format">pixel format</see> of the texture</param>
	/// <param name="pitch">
	/// The pitch used in the given <paramref name="pixels"/> data, in bytes;
	/// that is the length between the start of a row of pixels and the start of the next row of pixels (which may be greater than the pixel width of the given <paramref name="pixels"/> data, expressed in bytes)
	/// </param>
	/// <returns><c><see langword="true"/></c>, if the texture was successfully updated with the new pixel data; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// The given <paramref name="pixels"/> data must be in the <see cref="Format">pixel format</see> of the texture.
	/// </para>
	/// <para>
	/// This is a fairly slow operation, intended for use with <see cref="TextureAccess.Static">static</see> textures that do not change often.
	/// </para>
	/// <para>
	/// If the texture is intended to be updated often, you should prefer to create the texture as <see cref="TextureAccess.Streaming">streaming</see> and use one of the locking mechanisms to manipulate the texture's pixels (e.g. <see cref="TryLock(out TexturePixelMemoryManager)"/>) instead.
	/// While this method still works with <see cref="TextureAccess.Streaming">streaming</see> textures, for optimization reasons you may not get the pixels back if you lock the texture afterward.
	/// </para>
	/// <para>
	/// Using this method for NV12, NV21, YV12 or IYUV textures is perfectly fine as long as the given <paramref name="pixels"/> data is a contiguous block of NV12/N21 planes or Y and UV planes, respectively, in the proper order.
	/// If not, you should use <see cref="TryUpdateNv(byte*, int, byte*, int)"/> or <see cref="TryUpdateYuv(byte*, int, byte*, int, byte*, int)"/>, respectively, instead.
	/// </para>
	/// <para>
	/// This method should only be called from the main thread.
	/// </para>
	/// </remarks>
	unsafe bool TryUpdate(void* pixels, int pitch);

	/// <summary>
	/// Tries to update an area of a planar NV12 or NV21 texture with new pixel data
	/// </summary>
	/// <param name="rect">The area of the texture to update with the new pixel data</param>
	/// <param name="yPixels">The new pixel data for the Y plane of the texture, to be copied into the specified area of the texture</param>
	/// <param name="yPitch">
	/// The pitch used in the given <paramref name="yPixels"/> data, in bytes;
	/// that is the length between the start of a row of pixels and the start of the next row of pixels (which may be greater than the pixel width of the given <paramref name="yPixels"/> data, expressed in bytes)
	/// </param>
	/// <param name="uvPixels">The new pixel data for the UV plane of the texture, to be copied into the specified area of the texture</param>
	/// <param name="uvPitch">
	/// The pitch used in the given <paramref name="uvPixels"/> data, in bytes;
	/// that is the length between the start of a row of pixels and the start of the next row of pixels (which may be greater than the pixel width of the given <paramref name="uvPixels"/> data, expressed in bytes)
	/// </param>
	/// <returns><c><see langword="true"/></c>, if the texture was successfully updated with the new pixel data; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method is meant as an alternative to <see cref="TryUpdate(in Rect{int}, ReadOnlyNativeMemory, int)"/> for planar NV12 or NV21 textures when your pixel data for the individual planes is not stored as a contiguous block or you need to specify different pitch values for the individual planes.
	/// Please see the documentation of <see cref="TryUpdate(in Rect{int}, ReadOnlyNativeMemory, int)"/> for more general information about update operations, as the details mentioned there also apply to this method.
	/// </para>
	/// <para>
	/// This method should only be called from the main thread.
	/// </para>
	/// </remarks>
	bool TryUpdateNv(in Rect<int> rect, ReadOnlyNativeMemory<byte> yPixels, int yPitch, ReadOnlyNativeMemory<byte> uvPixels, int uvPitch);

	/// <summary>
	/// Tries to update an area of a planar NV12 or NV21 texture with new pixel data
	/// </summary>
	/// <param name="rect">The area of the texture to update with the new pixel data</param>
	/// <param name="yPixels">The new pixel data for the Y plane of the texture, to be copied into the specified area of the texture</param>
	/// <param name="yPitch">
	/// The pitch used in the given <paramref name="yPixels"/> data, in bytes;
	/// that is the length between the start of a row of pixels and the start of the next row of pixels (which may be greater than the pixel width of the given <paramref name="yPixels"/> data, expressed in bytes)
	/// </param>
	/// <param name="uvPixels">The new pixel data for the UV plane of the texture, to be copied into the specified area of the texture</param>
	/// <param name="uvPitch">
	/// The pitch used in the given <paramref name="uvPixels"/> data, in bytes;
	/// that is the length between the start of a row of pixels and the start of the next row of pixels (which may be greater than the pixel width of the given <paramref name="uvPixels"/> data, expressed in bytes)
	/// </param>
	/// <returns><c><see langword="true"/></c>, if the texture was successfully updated with the new pixel data; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method is meant as an alternative to <see cref="TryUpdate(in Rect{int}, ReadOnlySpan{byte}, int)"/> for planar NV12 or NV21 textures when your pixel data for the individual planes is not stored as a contiguous block or you need to specify different pitch values for the individual planes.
	/// Please see the documentation of <see cref="TryUpdate(in Rect{int}, ReadOnlySpan{byte}, int)"/> for more general information about update operations, as the details mentioned there also apply to this method.
	/// </para>
	/// <para>
	/// This method should only be called from the main thread.
	/// </para>
	/// </remarks>
	bool TryUpdateNv(in Rect<int> rect, ReadOnlySpan<byte> yPixels, int yPitch, ReadOnlySpan<byte> uvPixels, int uvPitch);

	/// <summary>
	/// Tries to update an area of a planar NV12 or NV21 texture with new pixel data
	/// </summary>
	/// <param name="rect">The area of the texture to update with the new pixel data</param>
	/// <param name="yPixels">The new pixel data for the Y plane of the texture, to be copied into the specified area of the texture</param>
	/// <param name="yPitch">
	/// The pitch used in the given <paramref name="yPixels"/> data, in bytes;
	/// that is the length between the start of a row of pixels and the start of the next row of pixels (which may be greater than the pixel width of the given <paramref name="yPixels"/> data, expressed in bytes)
	/// </param>
	/// <param name="uvPixels">The new pixel data for the UV plane of the texture, to be copied into the specified area of the texture</param>
	/// <param name="uvPitch">
	/// The pitch used in the given <paramref name="uvPixels"/> data, in bytes;
	/// that is the length between the start of a row of pixels and the start of the next row of pixels (which may be greater than the pixel width of the given <paramref name="uvPixels"/> data, expressed in bytes)
	/// </param>
	/// <returns><c><see langword="true"/></c>, if the texture was successfully updated with the new pixel data; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method is meant as an alternative to <see cref="TryUpdate(in Rect{int}, void*, int)"/> for planar NV12 or NV21 textures when your pixel data for the individual planes is not stored as a contiguous block or you need to specify different pitch values for the individual planes.
	/// Please see the documentation of <see cref="TryUpdate(in Rect{int}, void*, int)"/> for more general information about update operations, as the details mentioned there also apply to this method.
	/// </para>
	/// <para>
	/// This method should only be called from the main thread.
	/// </para>
	/// </remarks>
	unsafe bool TryUpdateNv(in Rect<int> rect, byte* yPixels, int yPitch, byte* uvPixels, int uvPitch);

	/// <summary>
	/// Tries to update an entire planar NV12 or NV21 texture with new pixel data
	/// </summary>
	/// <param name="yPixels">The new pixel data for the Y plane of the texture, to be copied into the entirety of the texture</param>
	/// <param name="yPitch">
	/// The pitch used in the given <paramref name="yPixels"/> data, in bytes;
	/// that is the length between the start of a row of pixels and the start of the next row of pixels (which may be greater than the pixel width of the given <paramref name="yPixels"/> data, expressed in bytes)
	/// </param>
	/// <param name="uvPixels">The new pixel data for the UV plane of the texture, to be copied into the entirety of the texture</param>
	/// <param name="uvPitch">
	/// The pitch used in the given <paramref name="uvPixels"/> data, in bytes;
	/// that is the length between the start of a row of pixels and the start of the next row of pixels (which may be greater than the pixel width of the given <paramref name="uvPixels"/> data, expressed in bytes)
	/// </param>
	/// <returns><c><see langword="true"/></c>, if the texture was successfully updated with the new pixel data; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method is meant as an alternative to <see cref="TryUpdate(ReadOnlyNativeMemory, int)"/> for planar NV12 or NV21 textures when your pixel data for the individual planes is not stored as a contiguous block or you need to specify different pitch values for the individual planes.
	/// Please see the documentation of <see cref="TryUpdate(ReadOnlyNativeMemory, int)"/> for more general information about update operations, as the details mentioned there also apply to this method.
	/// </para>
	/// <para>
	/// This method should only be called from the main thread.
	/// </para>
	/// </remarks>
	bool TryUpdateNv(ReadOnlyNativeMemory<byte> yPixels, int yPitch, ReadOnlyNativeMemory<byte> uvPixels, int uvPitch);

	/// <summary>
	/// Tries to update an entire planar NV12 or NV21 texture with new pixel data
	/// </summary>
	/// <param name="yPixels">The new pixel data for the Y plane of the texture, to be copied into the entirety of the texture</param>
	/// <param name="yPitch">
	/// The pitch used in the given <paramref name="yPixels"/> data, in bytes;
	/// that is the length between the start of a row of pixels and the start of the next row of pixels (which may be greater than the pixel width of the given <paramref name="yPixels"/> data, expressed in bytes)
	/// </param>
	/// <param name="uvPixels">The new pixel data for the UV plane of the texture, to be copied into the entirety of the texture</param>
	/// <param name="uvPitch">
	/// The pitch used in the given <paramref name="uvPixels"/> data, in bytes;
	/// that is the length between the start of a row of pixels and the start of the next row of pixels (which may be greater than the pixel width of the given <paramref name="uvPixels"/> data, expressed in bytes)
	/// </param>
	/// <returns><c><see langword="true"/></c>, if the texture was successfully updated with the new pixel data; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method is meant as an alternative to <see cref="TryUpdate(ReadOnlySpan{byte}, int)"/> for planar NV12 or NV21 textures when your pixel data for the individual planes is not stored as a contiguous block or you need to specify different pitch values for the individual planes.
	/// Please see the documentation of <see cref="TryUpdate(ReadOnlySpan{byte}, int)"/> for more general information about update operations, as the details mentioned there also apply to this method.
	/// </para>
	/// <para>
	/// This method should only be called from the main thread.
	/// </para>
	/// </remarks>
	bool TryUpdateNv(ReadOnlySpan<byte> yPixels, int yPitch, ReadOnlySpan<byte> uvPixels, int uvPitch);

	/// <summary>
	/// Tries to update an entire planar NV12 or NV21 texture with new pixel data
	/// </summary>
	/// <param name="yPixels">The new pixel data for the Y plane of the texture, to be copied into the entirety of the texture</param>
	/// <param name="yPitch">
	/// The pitch used in the given <paramref name="yPixels"/> data, in bytes;
	/// that is the length between the start of a row of pixels and the start of the next row of pixels (which may be greater than the pixel width of the given <paramref name="yPixels"/> data, expressed in bytes)
	/// </param>
	/// <param name="uvPixels">The new pixel data for the UV plane of the texture, to be copied into the entirety of the texture</param>
	/// <param name="uvPitch">
	/// The pitch used in the given <paramref name="uvPixels"/> data, in bytes;
	/// that is the length between the start of a row of pixels and the start of the next row of pixels (which may be greater than the pixel width of the given <paramref name="uvPixels"/> data, expressed in bytes)
	/// </param>
	/// <returns><c><see langword="true"/></c>, if the texture was successfully updated with the new pixel data; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method is meant as an alternative to <see cref="TryUpdate(void*, int)"/> for planar NV12 or NV21 textures when your pixel data for the individual planes is not stored as a contiguous block or you need to specify different pitch values for the individual planes.
	/// Please see the documentation of <see cref="TryUpdate(void*, int)"/> for more general information about update operations, as the details mentioned there also apply to this method.
	/// </para>
	/// <para>
	/// This method should only be called from the main thread.
	/// </para>
	/// </remarks>
	unsafe bool TryUpdateNv(byte* yPixels, int yPitch, byte* uvPixels, int uvPitch);

	/// <summary>
	/// Tries to update an area of a planar YV12 or IYUV texture with new pixel data
	/// </summary>
	/// <param name="rect">The area of the texture to update with the new pixel data</param>
	/// <param name="yPixels">The new pixel data for the Y plane of the texture, to be copied into the specified area of the texture</param>
	/// <param name="yPitch">
	/// The pitch used in the given <paramref name="yPixels"/> data, in bytes;
	/// that is the length between the start of a row of pixels and the start of the next row of pixels (which may be greater than the pixel width of the given <paramref name="yPixels"/> data, expressed in bytes)
	/// </param>
	/// <param name="uPixels">The new pixel data for the U plane of the texture, to be copied into the specified area of the texture</param>
	/// <param name="uPitch">
	/// The pitch used in the given <paramref name="uPixels"/> data, in bytes;
	/// that is the length between the start of a row of pixels and the start of the next row of pixels (which may be greater than the pixel width of the given <paramref name="uPixels"/> data, expressed in bytes)
	/// </param>
	/// <param name="vPixels">The new pixel data for the V plane of the texture, to be copied into the specified area of the texture</param>
	/// <param name="vPitch">
	/// The pitch used in the given <paramref name="vPixels"/> data, in bytes;
	/// that is the length between the start of a row of pixels and the start of the next row of pixels (which may be greater than the pixel width of the given <paramref name="vPixels"/> data, expressed in bytes)
	/// </param>
	/// <returns><c><see langword="true"/></c>, if the texture was successfully updated with the new pixel data; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method is meant as an alternative to <see cref="TryUpdate(in Rect{int}, ReadOnlyNativeMemory, int)"/> for planar YV12 or IYUV textures when your pixel data for the individual planes is not stored as a contiguous block or you need to specify different pitch values for the individual planes.
	/// Please see the documentation of <see cref="TryUpdate(in Rect{int}, ReadOnlyNativeMemory, int)"/> for more general information about update operations, as the details mentioned there also apply to this method.
	/// </para>
	/// <para>
	/// This method should only be called from the main thread.
	/// </para>
	/// </remarks>
	bool TryUpdateYuv(in Rect<int> rect, ReadOnlyNativeMemory<byte> yPixels, int yPitch, ReadOnlyNativeMemory<byte> uPixels, int uPitch, ReadOnlyNativeMemory<byte> vPixels, int vPitch);

	/// <summary>
	/// Tries to update an area of a planar YV12 or IYUV texture with new pixel data
	/// </summary>
	/// <param name="rect">The area of the texture to update with the new pixel data</param>
	/// <param name="yPixels">The new pixel data for the Y plane of the texture, to be copied into the specified area of the texture</param>
	/// <param name="yPitch">
	/// The pitch used in the given <paramref name="yPixels"/> data, in bytes;
	/// that is the length between the start of a row of pixels and the start of the next row of pixels (which may be greater than the pixel width of the given <paramref name="yPixels"/> data, expressed in bytes)
	/// </param>
	/// <param name="uPixels">The new pixel data for the U plane of the texture, to be copied into the specified area of the texture</param>
	/// <param name="uPitch">
	/// The pitch used in the given <paramref name="uPixels"/> data, in bytes;
	/// that is the length between the start of a row of pixels and the start of the next row of pixels (which may be greater than the pixel width of the given <paramref name="uPixels"/> data, expressed in bytes)
	/// </param>
	/// <param name="vPixels">The new pixel data for the V plane of the texture, to be copied into the specified area of the texture</param>
	/// <param name="vPitch">
	/// The pitch used in the given <paramref name="vPixels"/> data, in bytes;
	/// that is the length between the start of a row of pixels and the start of the next row of pixels (which may be greater than the pixel width of the given <paramref name="vPixels"/> data, expressed in bytes)
	/// </param>
	/// <returns><c><see langword="true"/></c>, if the texture was successfully updated with the new pixel data; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method is meant as an alternative to <see cref="TryUpdate(in Rect{int}, ReadOnlySpan{byte}, int)"/> for planar YV12 or IYUV textures when your pixel data for the individual planes is not stored as a contiguous block or you need to specify different pitch values for the individual planes.
	/// Please see the documentation of <see cref="TryUpdate(in Rect{int}, ReadOnlySpan{byte}, int)"/> for more general information about update operations, as the details mentioned there also apply to this method.
	/// </para>
	/// <para>
	/// This method should only be called from the main thread.
	/// </para>
	/// </remarks>
	bool TryUpdateYuv(in Rect<int> rect, ReadOnlySpan<byte> yPixels, int yPitch, ReadOnlySpan<byte> uPixels, int uPitch, ReadOnlySpan<byte> vPixels, int vPitch);

	/// <summary>
	/// Tries to update an area of a planar YV12 or IYUV texture with new pixel data
	/// </summary>
	/// <param name="rect">The area of the texture to update with the new pixel data</param>
	/// <param name="yPixels">The new pixel data for the Y plane of the texture, to be copied into the specified area of the texture</param>
	/// <param name="yPitch">
	/// The pitch used in the given <paramref name="yPixels"/> data, in bytes;
	/// that is the length between the start of a row of pixels and the start of the next row of pixels (which may be greater than the pixel width of the given <paramref name="yPixels"/> data, expressed in bytes)
	/// </param>
	/// <param name="uPixels">The new pixel data for the U plane of the texture, to be copied into the specified area of the texture</param>
	/// <param name="uPitch">
	/// The pitch used in the given <paramref name="uPixels"/> data, in bytes;
	/// that is the length between the start of a row of pixels and the start of the next row of pixels (which may be greater than the pixel width of the given <paramref name="uPixels"/> data, expressed in bytes)
	/// </param>
	/// <param name="vPixels">The new pixel data for the V plane of the texture, to be copied into the specified area of the texture</param>
	/// <param name="vPitch">
	/// The pitch used in the given <paramref name="vPixels"/> data, in bytes;
	/// that is the length between the start of a row of pixels and the start of the next row of pixels (which may be greater than the pixel width of the given <paramref name="vPixels"/> data, expressed in bytes)
	/// </param>
	/// <returns><c><see langword="true"/></c>, if the texture was successfully updated with the new pixel data; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method is meant as an alternative to <see cref="TryUpdate(in Rect{int}, void*, int)"/> for planar YV12 or IYUV textures when your pixel data for the individual planes is not stored as a contiguous block or you need to specify different pitch values for the individual planes.
	/// Please see the documentation of <see cref="TryUpdate(in Rect{int}, void*, int)"/> for more general information about update operations, as the details mentioned there also apply to this method.
	/// </para>
	/// <para>
	/// This method should only be called from the main thread.
	/// </para>
	/// </remarks>
	unsafe bool TryUpdateYuv(in Rect<int> rect, byte* yPixels, int yPitch, byte* uPixels, int uPitch, byte* vPixels, int vPitch);

	/// <summary>
	/// Tries to update an entire planar YV12 or IYUV texture with new pixel data
	/// </summary>
	/// <param name="yPixels">The new pixel data for the Y plane of the texture, to be copied into the entirety of the texture</param>
	/// <param name="yPitch">
	/// The pitch used in the given <paramref name="yPixels"/> data, in bytes;
	/// that is the length between the start of a row of pixels and the start of the next row of pixels (which may be greater than the pixel width of the given <paramref name="yPixels"/> data, expressed in bytes)
	/// </param>
	/// <param name="uPixels">The new pixel data for the U plane of the texture, to be copied into the entirety of the texture</param>
	/// <param name="uPitch">
	/// The pitch used in the given <paramref name="uPixels"/> data, in bytes;
	/// that is the length between the start of a row of pixels and the start of the next row of pixels (which may be greater than the pixel width of the given <paramref name="uPixels"/> data, expressed in bytes)
	/// </param>
	/// <param name="vPixels">The new pixel data for the V plane of the texture, to be copied into the entirety of the texture</param>
	/// <param name="vPitch">
	/// The pitch used in the given <paramref name="vPixels"/> data, in bytes;
	/// that is the length between the start of a row of pixels and the start of the next row of pixels (which may be greater than the pixel width of the given <paramref name="vPixels"/> data, expressed in bytes)
	/// </param>
	/// <returns><c><see langword="true"/></c>, if the texture was successfully updated with the new pixel data; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method is meant as an alternative to <see cref="TryUpdate(ReadOnlyNativeMemory, int)"/> for planar YV12 or IYUV textures when your pixel data for the individual planes is not stored as a contiguous block or you need to specify different pitch values for the individual planes.
	/// Please see the documentation of <see cref="TryUpdate(ReadOnlyNativeMemory, int)"/> for more general information about update operations, as the details mentioned there also apply to this method.
	/// </para>
	/// <para>
	/// This method should only be called from the main thread.
	/// </para>
	/// </remarks>
	bool TryUpdateYuv(ReadOnlyNativeMemory<byte> yPixels, int yPitch, ReadOnlyNativeMemory<byte> uPixels, int uPitch, ReadOnlyNativeMemory<byte> vPixels, int vPitch);

	/// <summary>
	/// Tries to update an entire planar YV12 or IYUV texture with new pixel data
	/// </summary>
	/// <param name="yPixels">The new pixel data for the Y plane of the texture, to be copied into the entirety of the texture</param>
	/// <param name="yPitch">
	/// The pitch used in the given <paramref name="yPixels"/> data, in bytes;
	/// that is the length between the start of a row of pixels and the start of the next row of pixels (which may be greater than the pixel width of the given <paramref name="yPixels"/> data, expressed in bytes)
	/// </param>
	/// <param name="uPixels">The new pixel data for the U plane of the texture, to be copied into the entirety of the texture</param>
	/// <param name="uPitch">
	/// The pitch used in the given <paramref name="uPixels"/> data, in bytes;
	/// that is the length between the start of a row of pixels and the start of the next row of pixels (which may be greater than the pixel width of the given <paramref name="uPixels"/> data, expressed in bytes)
	/// </param>
	/// <param name="vPixels">The new pixel data for the V plane of the texture, to be copied into the entirety of the texture</param>
	/// <param name="vPitch">
	/// The pitch used in the given <paramref name="vPixels"/> data, in bytes;
	/// that is the length between the start of a row of pixels and the start of the next row of pixels (which may be greater than the pixel width of the given <paramref name="vPixels"/> data, expressed in bytes)
	/// </param>
	/// <returns><c><see langword="true"/></c>, if the texture was successfully updated with the new pixel data; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method is meant as an alternative to <see cref="TryUpdate(ReadOnlySpan{byte}, int)"/> for planar YV12 or IYUV textures when your pixel data for the individual planes is not stored as a contiguous block or you need to specify different pitch values for the individual planes.
	/// Please see the documentation of <see cref="TryUpdate(ReadOnlySpan{byte}, int)"/> for more general information about update operations, as the details mentioned there also apply to this method.
	/// </para>
	/// <para>
	/// This method should only be called from the main thread.
	/// </para>
	/// </remarks>
	bool TryUpdateYuv(ReadOnlySpan<byte> yPixels, int yPitch, ReadOnlySpan<byte> uPixels, int uPitch, ReadOnlySpan<byte> vPixels, int vPitch);

	/// <summary>
	/// Tries to update an entire planar YV12 or IYUV texture with new pixel data
	/// </summary>
	/// <param name="yPixels">The new pixel data for the Y plane of the texture, to be copied into the entirety of the texture</param>
	/// <param name="yPitch">
	/// The pitch used in the given <paramref name="yPixels"/> data, in bytes;
	/// that is the length between the start of a row of pixels and the start of the next row of pixels (which may be greater than the pixel width of the given <paramref name="yPixels"/> data, expressed in bytes)
	/// </param>
	/// <param name="uPixels">The new pixel data for the U plane of the texture, to be copied into the entirety of the texture</param>
	/// <param name="uPitch">
	/// The pitch used in the given <paramref name="uPixels"/> data, in bytes;
	/// that is the length between the start of a row of pixels and the start of the next row of pixels (which may be greater than the pixel width of the given <paramref name="uPixels"/> data, expressed in bytes)
	/// </param>
	/// <param name="vPixels">The new pixel data for the V plane of the texture, to be copied into the entirety of the texture</param>
	/// <param name="vPitch">
	/// The pitch used in the given <paramref name="vPixels"/> data, in bytes;
	/// that is the length between the start of a row of pixels and the start of the next row of pixels (which may be greater than the pixel width of the given <paramref name="vPixels"/> data, expressed in bytes)
	/// </param>
	/// <returns><c><see langword="true"/></c>, if the texture was successfully updated with the new pixel data; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method is meant as an alternative to <see cref="TryUpdate(void*, int)"/> for planar YV12 or IYUV textures when your pixel data for the individual planes is not stored as a contiguous block or you need to specify different pitch values for the individual planes.
	/// Please see the documentation of <see cref="TryUpdate(void*, int)"/> for more general information about update operations, as the details mentioned there also apply to this method.
	/// </para>
	/// <para>
	/// This method should only be called from the main thread.
	/// </para>
	/// </remarks>
	unsafe bool TryUpdateYuv(byte* yPixels, int yPitch, byte* uPixels, int uPitch, byte* vPixels, int vPitch);

	/// <summary>
	/// Unlocks the texture after write-only pixel access
	/// </summary>
	/// <remarks>
	/// <para>
	/// This method is meant to be used in conjuction with any of the "TryUnsafeLock" or "TryUnsafeLockToSurface" methods (e.g. <see cref="TryUnsafeLock(in Rect{int}, out NativeMemory, out int)"/>), if you want to access the texture's pixels directly in a faster and more efficient way.
	/// If you're locking for a simpler and safer way to access the texture's pixels, consider using any of the "TryLock" or "TryLockToSurface" methods (e.g. <see cref="TryLock(in Rect{int}, out TexturePixelMemoryManager?)"/>) instead.
	/// </para>
	/// <para>
	/// <em>Warning</em>: Please note that locking a texture is intended to be write-only; it will not guarantee the previous contents of the texture will be provided.
	/// You <em>must</em> fully initialize any area of a texture that you locked before unlocking it, as the pixels might otherwise be uninitialized memory.
	/// E.g., if you locked a texture and immediately unlocked it again without writing any pixel data, the texture could end up in a corrupted state, depending on the renderer in use.
	/// </para>
	/// <para>
	/// This method should only be called from the main thread.
	/// </para>
	/// </remarks>
	/// <example>
	/// Example usage:
	/// <code>
	/// ITexture texture;
	/// 
	/// ...
	/// 
	/// if (texture.TryUnsafeLock(out NativeMemory pixels, out var pitch))
	/// {
	///		// Write-only access to the pixel memory using 'pitch'
	///		// Be sure to fully initialize ALL the pixels locked
	///		
	///		...
	///		
	///		texture.UnsafeUnlock();
	/// }
	/// </code>
	/// </example>
	void UnsafeUnlock();
}
