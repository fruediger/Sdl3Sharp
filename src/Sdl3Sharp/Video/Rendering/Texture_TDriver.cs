using Sdl3Sharp.Video.Coloring;
using Sdl3Sharp.Video.Drawing;
using Sdl3Sharp.Video.Gpu;
using Sdl3Sharp.Video.Rendering.Drivers;
using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Video.Rendering;

/// <summary>
/// Represents a texture, created by a <see cref="Renderer{TDriver}"/>
/// </summary>
/// <typeparam name="TDriver">The rendering driver type associated with this texture</typeparam>
/// <remarks>
/// <para>
/// This is an efficient driver-specific representation of pixel data.
/// </para>
/// <para>
/// You can create new textures using the <see cref="Renderer{TDriver}.TryCreateTexture(PixelFormat, TextureAccess, int, int, out Texture{TDriver}?)"/>,
/// <see cref="Renderer{TDriver}.TryCreateTexture(out Texture{TDriver}?, ColorSpace?, PixelFormat?, TextureAccess?, int?, int?, Palette?, float?, float?, Properties?)"/>,
/// or <see cref="Renderer{TDriver}.TryCreateTextureFromSurface(Surface, out Texture{TDriver}?)"/> instance methods on a <see cref="Renderer{TDriver}"/> instance.
/// </para>
/// <para>
/// Additionally, there are some driver-specific methods for creating textures, such as
/// <see cref="RendererExtensions.TryCreateTexture(Renderer{Direct3D11}, out Texture{Direct3D11}?, ColorSpace?, PixelFormat?, TextureAccess?, int?, int?, Palette?, float?, float?, nint?, nint?, nint?, Properties?)">Direct3D 11</see>,
/// <see cref="RendererExtensions.TryCreateTexture(Renderer{Direct3D12}, out Texture{Direct3D12}?, ColorSpace?, PixelFormat?, TextureAccess?, int?, int?, Palette?, float?, float?, nint?, nint?, nint?, Properties?)">Direct3D 12</see>,
/// <see cref="RendererExtensions.TryCreateTexture(Renderer{Drivers.Gpu}, out Texture{Drivers.Gpu}?, ColorSpace?, PixelFormat?, TextureAccess?, int?, int?, Palette?, float?, float?, GpuTexture?, GpuTexture?, GpuTexture?, GpuTexture?, Properties?)">GPU</see>,
/// <see cref="RendererExtensions.TryCreateTexture(Renderer{Metal}, out Texture{Metal}?, ColorSpace?, PixelFormat?, TextureAccess?, int?, int?, Palette?, float?, float?, nint?, Properties?)">Metal</see>,
/// <see cref="RendererExtensions.TryCreateTexture(Renderer{OpenGL}, out Texture{OpenGL}?, ColorSpace?, PixelFormat?, TextureAccess?, int?, int?, Palette?, float?, float?, uint?, uint?, uint?, uint?, Properties?)">OpenGL</see>,
/// <see cref="RendererExtensions.TryCreateTexture(Renderer{OpenGLEs2}, out Texture{OpenGLEs2}?, ColorSpace?, PixelFormat?, TextureAccess?, int?, int?, Palette?, float?, float?, uint?, uint?, uint?, uint?, Properties?)">OpenGL ES 2</see>,
/// and <see cref="RendererExtensions.TryCreateTexture(Renderer{Vulkan}, out Texture{Vulkan}?, ColorSpace?, PixelFormat?, TextureAccess?, int?, int?, Palette?, float?, float?, ulong?, uint?, Properties?)">Vulkan</see>.
/// </para>
/// <para>
/// Please remember to dispose <see cref="Texture{TDriver}"/>s <em>before</em> disposing the <see cref="Renderer{TDriver}"/> that created them!
/// Using an <see cref="Texture{TDriver}"/> after its associated <see cref="Renderer{TDriver}"/> has been disposed can lead to undefined behavior, including corruption and crashes.
/// </para>
/// <para>
/// <see cref="Texture{TDriver}"/>s are concrete texture types, associated with a specific rendering driver.
/// They are used in driver-specific rendering operations with the <see cref="Renderer{TDriver}"/> that created them.
/// </para>
/// <para>
/// If you want to use them in a more general way, you can use them as <see cref="Texture"/> instances, which serve as abstractions to use them in common rendering operations with the <see cref="Renderer"/> instance that created them.
/// </para>
/// </remarks>
public sealed partial class Texture<TDriver> : Texture
	where TDriver : notnull, IRenderingDriver // we don't need to worry about putting type argument independent code in the Renderer<TDriver> class,
									          // because TDriver surely is always going to be a reference type
									          // (that's because all of our predefined drivers types, implementing IRenderingDriver, are reference types and it's impossible for user code to implement the IRenderingDriver interface),
									          // and the JIT will share code for all reference type instantiations
{
	internal unsafe Texture(SDL_Texture* texture, bool register) :
		base(texture, register)
	{ }

	private protected override Renderer? GetRendererImpl() => Renderer;

	/// <inheritdoc cref="Texture.Renderer"/>
	public new Renderer<TDriver>? Renderer
	{
		get
		{
			unsafe
			{
				Renderer<TDriver>.TryGetOrCreate(SDL_GetRendererFromTexture(Pointer), out var renderer);
				return renderer;
			}
		}
	}

	internal unsafe static bool TryGetOrCreate(SDL_Texture* texture, [NotNullWhen(true)] out Texture<TDriver>? result)
		=> Texture.TryGetOrCreate(texture, out result);

	private protected sealed override bool TryLockImpl(in Rect<int> rect, [NotNullWhen(true)] out TexturePixelMemoryManager? pixelManager)
	{
		var result = TryLock(in rect, out var typedPixelManager);

		pixelManager = typedPixelManager;

		return result;
	}

	// Hide the non-specialized base method in favor of a strongly-typed overload and divert the user to it
	/// <summary>Use <see cref="TryLock(in Rect{int}, out TexturePixelMemoryManager{TDriver}?)"/> instead</summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	[OverloadResolutionPriority(-1)]
	[Obsolete(
		$"Use {nameof(TryLock)}(in {nameof(Rect<>)}<int>, out {nameof(TexturePixelMemoryManager<>)}<{nameof(TDriver)}>?) instead.",
		error: true
	)]
	public new bool TryLock(in Rect<int> rect, [NotNullWhen(true)] out TexturePixelMemoryManager? pixelManager)
		=> base.TryLock(in rect, out pixelManager);

	/// <inheritdoc cref="Texture.TryLock(in Rect{int}, out TexturePixelMemoryManager?)"/>
	public bool TryLock(in Rect<int> rect, [NotNullWhen(true)] out TexturePixelMemoryManager<TDriver>? pixelManager)
		=> TexturePixelMemoryManager<TDriver>.TryCreate(this, in rect, out pixelManager);

	private protected sealed override bool TryLockImpl([NotNullWhen(true)] out TexturePixelMemoryManager? pixelManager)
	{
		var result = TryLock(out var typedPixelManager);

		pixelManager = typedPixelManager;

		return result;
	}

	// Hide the non-specialized base method in favor of a strongly-typed overload and divert the user to it
	/// <summary>Use <see cref="TryLock(out TexturePixelMemoryManager{TDriver}?)"/> instead</summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	[OverloadResolutionPriority(-1)]
	[Obsolete(
		$"Use {nameof(TryLock)}(out {nameof(TexturePixelMemoryManager<>)}<{nameof(TDriver)}>?) instead.",
		error: true
	)]
	public new bool TryLock([NotNullWhen(true)] out TexturePixelMemoryManager? pixelManager)
		=> base.TryLock(out pixelManager);

	/// <inheritdoc cref="Texture.TryLock(out TexturePixelMemoryManager?)"/>
	public bool TryLock([NotNullWhen(true)] out TexturePixelMemoryManager<TDriver>? pixelManager)
		=> TexturePixelMemoryManager<TDriver>.TryCreate(this, out pixelManager);

	private protected sealed override bool TryLockToSurfaceImpl(in Rect<int> rect, [NotNullWhen(true)] out TextureSurfaceManager? surfaceManager)
	{
		var result = TryLockToSurface(in rect, out var typedSurfaceManager);

		surfaceManager = typedSurfaceManager;

		return result;
	}

	// Hide the non-specialized base method in favor of a strongly-typed overload and divert the user to it
	/// <summary>Use <see cref="TryLockToSurface(in Rect{int}, out TextureSurfaceManager{TDriver}?)"/> instead</summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	[OverloadResolutionPriority(-1)]
	[Obsolete(
		$"Use {nameof(TryLockToSurface)}(in {nameof(Rect<>)}<int>, out {nameof(TextureSurfaceManager<>)}<{nameof(TDriver)}>?) instead.",
		error: true
	)]
	public new bool TryLockToSurface(in Rect<int> rect, [NotNullWhen(true)] out TextureSurfaceManager? surfaceManager)
		=> base.TryLockToSurface(in rect, out surfaceManager);

	/// <inheritdoc cref="Texture.TryLockToSurface(in Rect{int}, out TextureSurfaceManager?)"/>
	public bool TryLockToSurface(in Rect<int> rect, [NotNullWhen(true)] out TextureSurfaceManager<TDriver>? surfaceManager)
		=> TextureSurfaceManager<TDriver>.TryCreate(this, in rect, out surfaceManager);

	private protected sealed override bool TryLockToSurfaceImpl([NotNullWhen(true)] out TextureSurfaceManager? surfaceManager)
	{
		var result = TryLockToSurface(out var typedSurfaceManager);

		surfaceManager = typedSurfaceManager;

		return result;
	}

	// Hide the non-specialized base method in favor of a strongly-typed overload and divert the user to it
	/// <summary>Use <see cref="TryLockToSurface(in Rect{int}, out TextureSurfaceManager{TDriver}?)"/> instead</summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	[OverloadResolutionPriority(-1)]
	[Obsolete(
		$"Use {nameof(TryLockToSurface)}(in {nameof(Rect<>)}<int>, out {nameof(TextureSurfaceManager<>)}<{nameof(TDriver)}>?) instead.",
		error: true
	)]
	public new bool TryLockToSurface([NotNullWhen(true)] out TextureSurfaceManager? surfaceManager)
		=> base.TryLockToSurface(out surfaceManager);

	/// <inheritdoc cref="Texture.TryLockToSurface(out TextureSurfaceManager?)"/>
	public bool TryLockToSurface([NotNullWhen(true)] out TextureSurfaceManager<TDriver>? surfaceManager)
		=> TextureSurfaceManager<TDriver>.TryCreate(this, out surfaceManager);
}
