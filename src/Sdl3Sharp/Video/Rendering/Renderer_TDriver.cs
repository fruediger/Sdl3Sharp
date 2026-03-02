using Sdl3Sharp.Utilities;
using Sdl3Sharp.Video.Coloring;
using Sdl3Sharp.Video.Drawing;
using Sdl3Sharp.Video.Gpu;
using Sdl3Sharp.Video.Rendering.Drivers;
using Sdl3Sharp.Video.Windowing;
using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Video.Rendering;

/// <summary>
/// Represents a rendering context (renderer)
/// </summary>
/// <typeparam name="TDriver">The rendering driver type associated with this renderer</typeparam>
/// <remarks>
/// <para>
/// This is used to perform driver-specific 2D rendering operations, most commonly to a <see cref="Windowing.Window"/> or an off-screen render target.
/// </para>
/// <para>
/// You can create new renderers for a <see cref="Windowing.Window"/> using the <see cref="Window.TryCreateRenderer{TDriver}(out Renderer{TDriver}?)"/>
/// or <see cref="Window.TryCreateRenderer{TDriver}(out Renderer{TDriver}?, ColorSpace?, RendererVSync?, Properties?)"/>
/// instance methods on a <see cref="Windowing.Window"/> instance.
/// </para>
/// <para>
/// Additionally, there are some driver-specific methods for creating renderers, such as
/// <see cref="Window.TryCreateRenderer(out Renderer{Drivers.Gpu}?, GpuDevice?)"/>,
/// <see cref="Window.TryCreateRenderer(out Renderer{Drivers.Gpu}?, ColorSpace?, RendererVSync?, GpuDevice?, bool?, bool?, bool?, Properties?)"/>,
/// <see cref="Window.TryCreateRenderer(out Renderer{Vulkan}?, ColorSpace?, RendererVSync?, nint?, ulong?, nint?, nint?, uint?, uint?, Properties?)"/>,
/// <see cref="Surface.TryCreateRenderer(out Renderer{Software}?)"/>,
/// <see cref="Surface.TryCreateRenderer(out Renderer{Software}?, ColorSpace?, Properties?)"/>,
/// and <see cref="RendererExtensions.TryCreate(out Renderer{Drivers.Gpu}?, GpuDevice?, Window?)"/>.
/// </para>
/// <para>
/// If you create textures using an <see cref="Renderer{TDriver}"/>, please remember to dispose them <em>before</em> disposing the renderer.
/// <em>Do not</em> dispose the associated <see cref="Windowing.Window"/> before disposing the <see cref="Renderer{TDriver}"/> either!
/// Using an <see cref="Renderer{TDriver}"/> after its associated <see cref="Windowing.Window"/> has been disposed can cause undefined behavior, including crashes.
/// </para>
/// <para>
/// For the most part <see cref="Renderer{TDriver}"/>s are not thread-safe, and most of their properties and methods should only be accessed from the main thread!
/// </para>
/// <para>
/// <see cref="Renderer{TDriver}"/>s are concrete renderer types, associate with a specific rendering driver.
/// They are used for driver-specific rendering operations with <see cref="Texture{TDriver}"/>s that were created by them.
/// </para>
/// <para>
/// If you want to use them in a more general way, you can use them as <see cref="Renderer"/> instances, which serve as abstractions to use them for common rendering operations with <see cref="Texture"/> instance that were created by them.
/// </para>
/// </remarks>
public sealed partial class Renderer<TDriver> : Renderer
	where TDriver : notnull, IRenderingDriver // we don't need to worry about putting type argument independent code in the Renderer<TDriver> class,
									          // because TDriver surely is always going to be a reference type
									          // (that's because all of our predefined drivers types are reference types and it's impossible for user code to implement the IRenderingDriver interface),
									          // and the JIT will share code for all reference type instantiations
{
	internal unsafe Renderer(SDL_Renderer* renderer, bool register) :
		base(renderer, register)
	{ }

	private protected sealed override Texture? GetTargetImpl() => Target;	

	/// <inheritdoc cref="Renderer.Target"/>
	public new Texture<TDriver>? Target
	{
		get
		{
			unsafe
			{
				Texture<TDriver>.TryGetOrCreate(SDL_GetRenderTarget(Pointer), out var result);
				return result;
			}
		}

		set => base.Target = value;
	}

	// You might be asking why we have this complicated pattern of an abstract "*Impl" method in the base class
	// and a non-virtual "base" method that just delegates to the "*Impl" method
	// and then the need to hide the base method in the derived class;
	// why not just have a virtual/abtract method in the base class directly (e.g. "TryCreateTexture"), override it here,
	// and don't have to hide the base method?
	// The reason for this pattern is to hide the publicly visible inheritance relationship in metadata from the user.

	private protected sealed override bool TryCreateTextureImpl(PixelFormat format, TextureAccess access, int width, int height, [NotNullWhen(true)] out Texture? texture)
	{
		var result = TryCreateTexture(format, access, width, height, out var typedTexture);

		texture = typedTexture;

		return result;
	}

	// Hide the non-specialized base method in favor of a strongly-typed overload and divert the user to it
	/// <summary>Use <see cref="TryCreateTexture(PixelFormat, TextureAccess, int, int, out Texture{TDriver}?)"/> instead</summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	[OverloadResolutionPriority(-1)]
	[Obsolete(
		$"Use {nameof(TryCreateTexture)}({nameof(PixelFormat)}, {nameof(TextureAccess)}, int, int, out {nameof(Texture<>)}<{nameof(TDriver)}>?) instead.",
		error: true
	)]
	public new bool TryCreateTexture(PixelFormat format, TextureAccess access, int width, int height, [NotNullWhen(true)] out Texture? texture)
		=> base.TryCreateTexture(format, access, width, height, out texture);

	/// <inheritdoc cref="Renderer.TryCreateTexture(PixelFormat, TextureAccess, int, int, out Texture?)"/>	
	public bool TryCreateTexture(PixelFormat format, TextureAccess access, int width, int height, [NotNullWhen(true)] out Texture<TDriver>? texture)
	{
		unsafe
		{
			var texturePtr = Texture.SDL_CreateTexture(Pointer, format, access, width, height);

			if (texturePtr is null)
			{
				texture = null;
				return false;
			}

			texture = new(texturePtr, register: true);
			return true;
		}
	}

	private protected sealed override bool TryCreateTextureImpl([NotNullWhen(true)] out Texture? texture, ColorSpace? colorSpace = null, PixelFormat? format = null, TextureAccess? access = null, int? width = null, int? height = null,
#if SDL3_4_0_OR_GREATER
		Palette? palette = null,
#endif
		float? sdrWhitePoint = null, float? hdrHeadroom = null, Properties? properties = null)
	{
		var result = TryCreateTexture(out var typedTexture, colorSpace, format, access, width, height,
#if SDL3_4_0_OR_GREATER
			palette,
#endif
			sdrWhitePoint, hdrHeadroom, properties
		);

		texture = typedTexture;

		return result;
	}

	// Hide the non-specialized base method in favor of a strongly-typed overload and divert the user to it
#if SDL3_4_0_OR_GREATER
	/// <summary>Use <see cref="TryCreateTexture(out Texture{TDriver}?, ColorSpace?, PixelFormat?, TextureAccess?, int?, int?, Palette?, float?, float?, Properties?)"/> instead</summary>
#else
	/// <summary>Use <see cref="TryCreateTexture(out Texture{TDriver}?, ColorSpace?, PixelFormat?, TextureAccess?, int?, int?, float?, float?, Properties?)"/> instead</summary>
#endif
	[EditorBrowsable(EditorBrowsableState.Never)]
	[OverloadResolutionPriority(-1)]
	[Obsolete(
		$"Use {nameof(TryCreateTexture)}(out {nameof(Texture<>)}<{nameof(TDriver)}>?, {nameof(ColorScale)}?, {nameof(PixelFormat)}?, {nameof(TextureAccess)}?, int?, int?, "
#if SDL3_4_0_OR_GREATER
		+ $"{nameof(Palette)}?, "
#endif
		+ $"float?, float?, {nameof(Sdl3Sharp.Properties)}?) instead.",
		error: true
	)]
	public new bool TryCreateTexture([NotNullWhen(true)] out Texture? texture, ColorSpace? colorSpace = default, PixelFormat? format = default, TextureAccess? access = default, int? width = default, int? height = default,
#if SDL3_4_0_OR_GREATER
		Palette? palette = default,
#endif
		float? sdrWhitePoint = default, float? hdrHeadroom = default, Properties? properties = default)
		=> base.TryCreateTexture(out texture, colorSpace, format, access, width, height,
#if SDL3_4_0_OR_GREATER
			palette,
#endif
			sdrWhitePoint, hdrHeadroom, properties
		);

#if SDL3_4_0_OR_GREATER
	/// <inheritdoc cref="Renderer.TryCreateTexture(out Texture?, ColorSpace?, PixelFormat?, TextureAccess?, int?, int?, Palette?, float?, float?, Properties?)"/>
#else
	/// <inheritdoc cref="Renderer.TryCreateTexture(out Texture?, ColorSpace?, PixelFormat?, TextureAccess?, int?, int?, float?, float?, Properties?)"/>
#endif
	public bool TryCreateTexture([NotNullWhen(true)] out Texture<TDriver>? texture, ColorSpace? colorSpace = default, PixelFormat? format = default, TextureAccess? access = default, int? width = default, int? height = default,
#if SDL3_4_0_OR_GREATER
		Palette? palette = default,
#endif
		float? sdrWhitePoint = default, float? hdrHeadroom = default, Properties? properties = default)
	{
		unsafe
		{
			Properties propertiesUsed;
			Unsafe.SkipInit(out ColorSpace? colorSpaceBackup);
			Unsafe.SkipInit(out PixelFormat? formatBackup);
			Unsafe.SkipInit(out TextureAccess? accessBackup);
			Unsafe.SkipInit(out int? widthBackup);
			Unsafe.SkipInit(out int? heightBackup);
#if SDL3_4_0_OR_GREATER
			Unsafe.SkipInit(out IntPtr? paletteBackup);
#endif
			Unsafe.SkipInit(out float? sdrWhitePointBackup);
			Unsafe.SkipInit(out float? hdrHeadroomBackup);

			if (properties is null)
			{
				propertiesUsed = [];

				if (colorSpace is ColorSpace colorSpaceValue)
				{
					propertiesUsed.TrySetNumberValue(Texture.PropertyNames.CreateColorSpaceNumber, unchecked((uint)colorSpaceValue));
				}

				if (format is PixelFormat formatValue)
				{
					propertiesUsed.TrySetNumberValue(Texture.PropertyNames.CreateFormatNumber, unchecked((uint)formatValue));
				}

				if (access is TextureAccess accessValue)
				{
					propertiesUsed.TrySetNumberValue(Texture.PropertyNames.CreateAccessNumber, unchecked((int)accessValue));
				}

				if (width is int widthValue)
				{
					// actually, width and height are required when creating a texture, but we'll let SDL fail and set the error message accordingly

					propertiesUsed.TrySetNumberValue(Texture.PropertyNames.CreateWidthNumber, widthValue);
				}

				if (height is int heightValue)
				{
					// actually, width and height are required when creating a texture, but we'll let SDL fail and set the error message accordingly

					propertiesUsed.TrySetNumberValue(Texture.PropertyNames.CreateHeightNumber, heightValue);
				}

#if SDL3_4_0_OR_GREATER

				if (palette is { Pointer: var palettePtr })
				{
					propertiesUsed.TrySetPointerValue(Texture.PropertyNames.CreatePalettePointer, unchecked((IntPtr)palettePtr));
				}

#endif

				if (sdrWhitePoint is float sdrWhitePointValue)
				{
					propertiesUsed.TrySetFloatValue(Texture.PropertyNames.CreateSdrWhitePointFloat, sdrWhitePointValue);
				}

				if (hdrHeadroom is float hdrHeadroomValue)
				{
					propertiesUsed.TrySetFloatValue(Texture.PropertyNames.CreateHdrHeadroomFloat, hdrHeadroomValue);
				}
			}
			else
			{
				propertiesUsed = properties;

				if (colorSpace is ColorSpace colorSpaceValue)
				{
					colorSpaceBackup = propertiesUsed.TryGetNumberValue(Texture.PropertyNames.CreateColorSpaceNumber, out var existingColorSpaceValue)
						? unchecked((ColorSpace)existingColorSpaceValue)
						: null;

					propertiesUsed.TrySetNumberValue(Texture.PropertyNames.CreateColorSpaceNumber, unchecked((uint)colorSpaceValue));
				}

				if (format is PixelFormat formatValue)
				{
					formatBackup = propertiesUsed.TryGetNumberValue(Texture.PropertyNames.CreateFormatNumber, out var existingFormatValue)
						? unchecked((PixelFormat)existingFormatValue)
						: null;

					propertiesUsed.TrySetNumberValue(Texture.PropertyNames.CreateFormatNumber, unchecked((uint)formatValue));
				}

				if (access is TextureAccess accessValue)
				{
					accessBackup = propertiesUsed.TryGetNumberValue(Texture.PropertyNames.CreateAccessNumber, out var existingAccessValue)
						? unchecked((TextureAccess)existingAccessValue)
						: null;

					propertiesUsed.TrySetNumberValue(Texture.PropertyNames.CreateAccessNumber, unchecked((int)accessValue));
				}

				if (width is int widthValue)
				{
					// actually, width and height are required when creating a texture, but they could already exist in the given properties and if not we'll let SDL fail and set the error message accordingly

					widthBackup = propertiesUsed.TryGetNumberValue(Texture.PropertyNames.CreateWidthNumber, out var existingWidthValue)
						? unchecked((int)existingWidthValue)
						: null;

					propertiesUsed.TrySetNumberValue(Texture.PropertyNames.CreateWidthNumber, widthValue);
				}

				if (height is int heightValue)
				{
					// actually, width and height are required when creating a texture, but they could already exist in the given properties and if not we'll let SDL fail and set the error message accordingly

					heightBackup = propertiesUsed.TryGetNumberValue(Texture.PropertyNames.CreateHeightNumber, out var existingHeightValue)
						? unchecked((int)existingHeightValue)
						: null;

					propertiesUsed.TrySetNumberValue(Texture.PropertyNames.CreateHeightNumber, heightValue);
				}

#if SDL3_4_0_OR_GREATER

				if (palette is { Pointer: var palettePtr })
				{
					paletteBackup = propertiesUsed.TryGetPointerValue(Texture.PropertyNames.CreatePalettePointer, out var existingPalettePtr)
						? existingPalettePtr
						: null;

					propertiesUsed.TrySetPointerValue(Texture.PropertyNames.CreatePalettePointer, unchecked((IntPtr)palettePtr));
				}

#endif

				if (sdrWhitePoint is float sdrWhitePointValue)
				{
					sdrWhitePointBackup = propertiesUsed.TryGetFloatValue(Texture.PropertyNames.CreateSdrWhitePointFloat, out var existingSdrWhitePointValue)
						? existingSdrWhitePointValue
						: null;

					propertiesUsed.TrySetFloatValue(Texture.PropertyNames.CreateSdrWhitePointFloat, sdrWhitePointValue);
				}

				if (hdrHeadroom is float hdrHeadroomValue)
				{
					hdrHeadroomBackup = propertiesUsed.TryGetFloatValue(Texture.PropertyNames.CreateHdrHeadroomFloat, out var existingHdrHeadroomValue)
						? existingHdrHeadroomValue
						: null;

					propertiesUsed.TrySetFloatValue(Texture.PropertyNames.CreateHdrHeadroomFloat, hdrHeadroomValue);
				}
			}

			try
			{
				var texturePtr = Texture.SDL_CreateTextureWithProperties(Pointer, propertiesUsed.Id);

				if (texturePtr is null)
				{
					texture = null;
					return false;
				}

				texture = new(texturePtr, register: true);
				return true;
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

					if (colorSpace.HasValue)
					{
						if (colorSpaceBackup is ColorSpace colorSpaceValue)
						{
							propertiesUsed.TrySetNumberValue(Texture.PropertyNames.CreateColorSpaceNumber, unchecked((uint)colorSpaceValue));

						}
						else
						{
							propertiesUsed.TryRemove(Texture.PropertyNames.CreateColorSpaceNumber);
						}
					}

					if (format.HasValue)
					{
						if (formatBackup is PixelFormat formatValue)
						{
							propertiesUsed.TrySetNumberValue(Texture.PropertyNames.CreateFormatNumber, unchecked((uint)formatValue));
						}
						else
						{
							propertiesUsed.TryRemove(Texture.PropertyNames.CreateFormatNumber);
						}
					}

					if (access.HasValue)
					{
						if (accessBackup is TextureAccess accessValue)
						{
							propertiesUsed.TrySetNumberValue(Texture.PropertyNames.CreateAccessNumber, unchecked((int)accessValue));
						}
						else
						{
							propertiesUsed.TryRemove(Texture.PropertyNames.CreateAccessNumber);
						}
					}

					if (width.HasValue)
					{
						if (widthBackup is int widthValue)
						{
							propertiesUsed.TrySetNumberValue(Texture.PropertyNames.CreateWidthNumber, widthValue);
						}
						else
						{
							propertiesUsed.TryRemove(Texture.PropertyNames.CreateWidthNumber);
						}
					}

					if (height.HasValue)
					{
						if (heightBackup is int heightValue)
						{
							propertiesUsed.TrySetNumberValue(Texture.PropertyNames.CreateHeightNumber, heightValue);
						}
						else
						{
							propertiesUsed.TryRemove(Texture.PropertyNames.CreateHeightNumber);
						}
					}

#if SDL3_4_0_OR_GREATER

					if (palette is not null)
					{
						if (paletteBackup is IntPtr palettePtr)
						{
							propertiesUsed.TrySetPointerValue(Texture.PropertyNames.CreatePalettePointer, palettePtr);
						}
						else
						{
							propertiesUsed.TryRemove(Texture.PropertyNames.CreatePalettePointer);
						}
					}

#endif

					if (sdrWhitePoint.HasValue)
					{
						if (sdrWhitePointBackup is float sdrWhitePointValue)
						{
							propertiesUsed.TrySetFloatValue(Texture.PropertyNames.CreateSdrWhitePointFloat, sdrWhitePointValue);
						}
						else
						{
							propertiesUsed.TryRemove(Texture.PropertyNames.CreateSdrWhitePointFloat);
						}
					}

					if (hdrHeadroom.HasValue)
					{
						if (hdrHeadroomBackup is float hdrHeadroomValue)
						{
							propertiesUsed.TrySetFloatValue(Texture.PropertyNames.CreateHdrHeadroomFloat, hdrHeadroomValue);
						}
						else
						{
							propertiesUsed.TryRemove(Texture.PropertyNames.CreateHdrHeadroomFloat);
						}
					}
				}
			}
		}
	}

	private protected sealed override bool TryCreateTextureFromSurfaceImpl(Surface surface, [NotNullWhen(true)] out Texture? texture)
	{
		var result = TryCreateTextureFromSurface(surface, out var typedTexture);

		texture = typedTexture;

		return result;
	}

	// Hide the non-specialized base method in favor of a strongly-typed overload and divert the user to it
	/// <summary>Use <see cref="TryCreateTextureFromSurface(Surface, out Texture{TDriver}?)"/> instead</summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	[OverloadResolutionPriority(-1)]
	[Obsolete(
		$"Use {nameof(TryCreateTextureFromSurface)}({nameof(Surface)}, out {nameof(Texture<>)}<{nameof(TDriver)}>?) instead.",
		error: true
	)]
	public new bool TryCreateTextureFromSurface(Surface surface, [NotNullWhen(true)] out Texture? texture)
		=> base.TryCreateTextureFromSurface(surface, out texture);

	/// <inheritdoc cref="Renderer.TryCreateTextureFromSurface(Surface, out Texture?)"/>
	public bool TryCreateTextureFromSurface(Surface surface, [NotNullWhen(true)] out Texture<TDriver>? texture)
	{
		unsafe
		{
			var texturePtr = Texture.SDL_CreateTextureFromSurface(Pointer, surface is not null ? surface.Pointer : null);

			if (texturePtr is null)
			{
				texture = null;
				return false;
			}

			texture = new(texturePtr, register: true);
			return true;
		}
	}

	internal unsafe static bool TryGetOrCreate(SDL_Renderer* renderer, [NotNullWhen(true)] out Renderer<TDriver>? result)
		=> Renderer.TryGetOrCreate(renderer, out result);

	// Hide the non-specialized base method in favor of a strongly-typed overload and divert the user to it
	/// <summary>Use <see cref="TryRenderGeometry(ReadOnlySpan{Vertex}, ReadOnlySpan{int}, Texture{TDriver}?)"/> instead</summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	[OverloadResolutionPriority(-1)]
	[Obsolete(
		$"Use {nameof(TryRenderGeometry)}({nameof(ReadOnlySpan<>)}<{nameof(Vertex)}>, {nameof(ReadOnlySpan<>)}<int>, {nameof(Texture<>)}<{nameof(TDriver)}>?) instead.",
		error: true
	)]
	public new bool TryRenderGeometry(ReadOnlySpan<Vertex> vertices, ReadOnlySpan<int> indices, Texture? texture)
		=> base.TryRenderGeometry(vertices, indices, texture);

	/// <inheritdoc cref="Renderer.TryRenderGeometry(ReadOnlySpan{Vertex}, ReadOnlySpan{int}, Texture?)"/>
	public bool TryRenderGeometry(ReadOnlySpan<Vertex> vertices, ReadOnlySpan<int> indices, Texture<TDriver>? texture = default)
		=> base.TryRenderGeometry(vertices, indices, texture);

	// Hide the non-specialized base method in favor of a strongly-typed overload and divert the user to it
	/// <summary>Use <see cref="TryRenderGeometry(ReadOnlySpan{Vertex}, Texture{TDriver}?)"/> instead</summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	[OverloadResolutionPriority(-1)]
	[Obsolete(
		$"Use {nameof(TryRenderGeometry)}({nameof(ReadOnlySpan<>)}<{nameof(Vertex)}>, {nameof(Texture<>)}<{nameof(TDriver)}>?) instead.",
		error: true
	)]
	public new bool TryRenderGeometry(ReadOnlySpan<Vertex> vertices, Texture? texture)
		=> base.TryRenderGeometry(vertices, texture);

	/// <inheritdoc cref="Renderer.TryRenderGeometry(ReadOnlySpan{Vertex}, Texture?)" />
	public bool TryRenderGeometry(ReadOnlySpan<Vertex> vertices, Texture<TDriver>? texture = default)
		=> base.TryRenderGeometry(vertices, texture);

	// Hide the non-specialized base method in favor of a strongly-typed overload and divert the user to it
	/// <summary>Use <see cref="TryRenderGeometryRaw(ReadOnlyNativeMemory{float}, int, ReadOnlyNativeMemory{Color{float}}, int, ReadOnlyNativeMemory{float}, int, ReadOnlySpan{int}, Texture{TDriver}?)"/> instead</summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	[OverloadResolutionPriority(-1)]
	[Obsolete(
		$"Use {nameof(TryRenderGeometryRaw)}({nameof(ReadOnlyNativeMemory<>)}<float>, int, {nameof(ReadOnlyNativeMemory<>)}<{nameof(Color<>)}<float>>, int, {nameof(ReadOnlyNativeMemory<>)}<float>, int, {nameof(ReadOnlySpan<>)}<int>, {nameof(Texture<>)}<{nameof(TDriver)}>?) instead.",
		error: true
	)]
	public new bool TryRenderGeometryRaw(ReadOnlyNativeMemory<float> xy, int xyStride, ReadOnlyNativeMemory<Color<float>> colors, int colorStride, ReadOnlyNativeMemory<float> uv, int uvStride, ReadOnlySpan<int> indices, Texture? texture)
		=> base.TryRenderGeometryRaw(xy, xyStride, colors, colorStride, uv, uvStride, indices, texture);

	/// <inheritdoc cref="Renderer.TryRenderGeometryRaw(ReadOnlyNativeMemory{float}, int, ReadOnlyNativeMemory{Color{float}}, int, ReadOnlyNativeMemory{float}, int, ReadOnlySpan{int}, Texture?)" />
	public bool TryRenderGeometryRaw(ReadOnlyNativeMemory<float> xy, int xyStride, ReadOnlyNativeMemory<Color<float>> colors, int colorStride, ReadOnlyNativeMemory<float> uv, int uvStride, ReadOnlySpan<int> indices, Texture<TDriver>? texture = default)
		=> base.TryRenderGeometryRaw(xy, xyStride, colors, colorStride, uv, uvStride, indices, texture);

	// Hide the non-specialized base method in favor of a strongly-typed overload and divert the user to it
	/// <summary>Use <see cref="TryRenderGeometryRaw(ReadOnlyNativeMemory{float}, int, ReadOnlyNativeMemory{Color{float}}, int, ReadOnlyNativeMemory{float}, int, ReadOnlySpan{short}, Texture{TDriver}?)"/> instead</summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	[OverloadResolutionPriority(-1)]
	[Obsolete(
		$"Use {nameof(TryRenderGeometryRaw)}({nameof(ReadOnlyNativeMemory<>)}<float>, int, {nameof(ReadOnlyNativeMemory<>)}<{nameof(Color<>)}<float>>, int, {nameof(ReadOnlyNativeMemory<>)}<float>, int, {nameof(ReadOnlySpan<>)}<short>, {nameof(Texture<>)}<{nameof(TDriver)}>?) instead.",
		error: true
	)]
	public new bool TryRenderGeometryRaw(ReadOnlyNativeMemory<float> xy, int xyStride, ReadOnlyNativeMemory<Color<float>> colors, int colorStride, ReadOnlyNativeMemory<float> uv, int uvStride, ReadOnlySpan<short> indices, Texture? texture)
		=> base.TryRenderGeometryRaw(xy, xyStride, colors, colorStride, uv, uvStride, indices, texture);

	/// <inheritdoc cref="Renderer.TryRenderGeometryRaw(ReadOnlyNativeMemory{float}, int, ReadOnlyNativeMemory{Color{float}}, int, ReadOnlyNativeMemory{float}, int, ReadOnlySpan{short}, Texture?)" />
	public bool TryRenderGeometryRaw(ReadOnlyNativeMemory<float> xy, int xyStride, ReadOnlyNativeMemory<Color<float>> colors, int colorStride, ReadOnlyNativeMemory<float> uv, int uvStride, ReadOnlySpan<short> indices, Texture<TDriver>? texture = default)
		=> base.TryRenderGeometryRaw(xy, xyStride, colors, colorStride, uv, uvStride, indices, texture);

	// Hide the non-specialized base method in favor of a strongly-typed overload and divert the user to it
	/// <summary>Use <see cref="TryRenderGeometryRaw(ReadOnlyNativeMemory{float}, int, ReadOnlyNativeMemory{Color{float}}, int, ReadOnlyNativeMemory{float}, int, ReadOnlySpan{sbyte}, Texture{TDriver}?)"/> instead</summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	[OverloadResolutionPriority(-1)]
	[Obsolete(
		$"Use {nameof(TryRenderGeometryRaw)}({nameof(ReadOnlyNativeMemory<>)}<float>, int, {nameof(ReadOnlyNativeMemory<>)}<{nameof(Color<>)}<float>>, int, {nameof(ReadOnlyNativeMemory<>)}<float>, int, {nameof(ReadOnlySpan<>)}<sbyte>, {nameof(Texture<>)}<{nameof(TDriver)}>?) instead.",
		error: true
	)]
	public new bool TryRenderGeometryRaw(ReadOnlyNativeMemory<float> xy, int xyStride, ReadOnlyNativeMemory<Color<float>> colors, int colorStride, ReadOnlyNativeMemory<float> uv, int uvStride, ReadOnlySpan<sbyte> indices, Texture? texture)
		=> base.TryRenderGeometryRaw(xy, xyStride, colors, colorStride, uv, uvStride, indices, texture);

	/// <inheritdoc cref="Renderer.TryRenderGeometryRaw(ReadOnlyNativeMemory{float}, int, ReadOnlyNativeMemory{Color{float}}, int, ReadOnlyNativeMemory{float}, int, ReadOnlySpan{sbyte}, Texture?)" />
	public bool TryRenderGeometryRaw(ReadOnlyNativeMemory<float> xy, int xyStride, ReadOnlyNativeMemory<Color<float>> colors, int colorStride, ReadOnlyNativeMemory<float> uv, int uvStride, ReadOnlySpan<sbyte> indices, Texture<TDriver>? texture = default)
		=> base.TryRenderGeometryRaw(xy, xyStride, colors, colorStride, uv, uvStride, indices, texture);

	// Hide the non-specialized base method in favor of a strongly-typed overload and divert the user to it
	/// <summary>Use <see cref="TryRenderGeometryRaw(ReadOnlyNativeMemory{float}, int, ReadOnlyNativeMemory{Color{float}}, int, ReadOnlyNativeMemory{float}, int, Texture{TDriver}?)"/> instead</summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	[OverloadResolutionPriority(-1)]
	[Obsolete(
		$"Use {nameof(TryRenderGeometryRaw)}({nameof(ReadOnlyNativeMemory<>)}<float>, int, {nameof(ReadOnlyNativeMemory<>)}<{nameof(Color<>)}<float>>, int, {nameof(ReadOnlyNativeMemory<>)}<float>, int, {nameof(Texture<>)}<{nameof(TDriver)}>?) instead.",
		error: true
	)]
	public new bool TryRenderGeometryRaw(ReadOnlyNativeMemory<float> xy, int xyStride, ReadOnlyNativeMemory<Color<float>> colors, int colorStride, ReadOnlyNativeMemory<float> uv, int uvStride, Texture? texture)
		=> base.TryRenderGeometryRaw(xy, xyStride, colors, colorStride, uv, uvStride, texture);

	/// <inheritdoc cref="Renderer.TryRenderGeometryRaw(ReadOnlyNativeMemory{float}, int, ReadOnlyNativeMemory{Color{float}}, int, ReadOnlyNativeMemory{float}, int, Texture?)"/>
	public bool TryRenderGeometryRaw(ReadOnlyNativeMemory<float> xy, int xyStride, ReadOnlyNativeMemory<Color<float>> colors, int colorStride, ReadOnlyNativeMemory<float> uv, int uvStride, Texture<TDriver>? texture = default)
		=> base.TryRenderGeometryRaw(xy, xyStride, colors, colorStride, uv, uvStride, texture);

	// Hide the non-specialized base method in favor of a strongly-typed overload and divert the user to it
	/// <summary>Use <see cref="TryRenderGeometryRaw(ReadOnlySpan{float}, int, ReadOnlySpan{Color{float}}, int, ReadOnlySpan{float}, int, ReadOnlySpan{int}, Texture{TDriver}?)"/> instead</summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	[OverloadResolutionPriority(-1)]
	[Obsolete(
		$"Use {nameof(TryRenderGeometryRaw)}({nameof(ReadOnlySpan<>)}<float>, int, {nameof(ReadOnlySpan<>)}<{nameof(Color<>)}<float>>, int, {nameof(ReadOnlySpan<>)}<float>, int, {nameof(ReadOnlySpan<>)}<int>, {nameof(Texture<>)}<{nameof(TDriver)}>?) instead.",
		error: true
	)]
	public new bool TryRenderGeometryRaw(ReadOnlySpan<float> xy, int xyStride, ReadOnlySpan<Color<float>> colors, int colorStride, ReadOnlySpan<float> uv, int uvStride, ReadOnlySpan<int> indices, Texture? texture)
		=> base.TryRenderGeometryRaw(xy, xyStride, colors, colorStride, uv, uvStride, indices, texture);

	/// <inheritdoc cref="Renderer.TryRenderGeometryRaw(ReadOnlySpan{float}, int, ReadOnlySpan{Color{float}}, int, ReadOnlySpan{float}, int, ReadOnlySpan{int}, Texture?)" />
	public bool TryRenderGeometryRaw(ReadOnlySpan<float> xy, int xyStride, ReadOnlySpan<Color<float>> colors, int colorStride, ReadOnlySpan<float> uv, int uvStride, ReadOnlySpan<int> indices, Texture<TDriver>? texture = default)
		=> base.TryRenderGeometryRaw(xy, xyStride, colors, colorStride, uv, uvStride, indices, texture);

	// Hide the non-specialized base method in favor of a strongly-typed overload and divert the user to it
	/// <summary>Use <see cref="TryRenderGeometryRaw(ReadOnlySpan{float}, int, ReadOnlySpan{Color{float}}, int, ReadOnlySpan{float}, int, ReadOnlySpan{short}, Texture{TDriver}?)"/> instead</summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	[OverloadResolutionPriority(-1)]
	[Obsolete(
		$"Use {nameof(TryRenderGeometryRaw)}({nameof(ReadOnlySpan<>)}<float>, int, {nameof(ReadOnlySpan<>)}<{nameof(Color<>)}<float>>, int, {nameof(ReadOnlySpan<>)}<float>, int, {nameof(ReadOnlySpan<>)}<short>, {nameof(Texture<>)}<{nameof(TDriver)}>?) instead.",
		error: true
	)]
	public new bool TryRenderGeometryRaw(ReadOnlySpan<float> xy, int xyStride, ReadOnlySpan<Color<float>> colors, int colorStride, ReadOnlySpan<float> uv, int uvStride, ReadOnlySpan<short> indices, Texture? texture)
		=> base.TryRenderGeometryRaw(xy, xyStride, colors, colorStride, uv, uvStride, indices, texture);

	/// <inheritdoc cref="Renderer.TryRenderGeometryRaw(ReadOnlySpan{float}, int, ReadOnlySpan{Color{float}}, int, ReadOnlySpan{float}, int, ReadOnlySpan{short}, Texture?)" />
	public bool TryRenderGeometryRaw(ReadOnlySpan<float> xy, int xyStride, ReadOnlySpan<Color<float>> colors, int colorStride, ReadOnlySpan<float> uv, int uvStride, ReadOnlySpan<short> indices, Texture<TDriver>? texture = default)
		=> base.TryRenderGeometryRaw(xy, xyStride, colors, colorStride, uv, uvStride, indices, texture);

	// Hide the non-specialized base method in favor of a strongly-typed overload and divert the user to it
	/// <summary>Use <see cref="TryRenderGeometryRaw(ReadOnlySpan{float}, int, ReadOnlySpan{Color{float}}, int, ReadOnlySpan{float}, int, ReadOnlySpan{sbyte}, Texture{TDriver}?)"/> instead</summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	[OverloadResolutionPriority(-1)]
	[Obsolete(
		$"Use {nameof(TryRenderGeometryRaw)}({nameof(ReadOnlySpan<>)}<float>, int, {nameof(ReadOnlySpan<>)}<{nameof(Color<>)}<float>>, int, {nameof(ReadOnlySpan<>)}<float>, int, {nameof(ReadOnlySpan<>)}<byte>, {nameof(Texture<>)}<{nameof(TDriver)}>?) instead.",
		error: true
	)]
	public new bool TryRenderGeometryRaw(ReadOnlySpan<float> xy, int xyStride, ReadOnlySpan<Color<float>> colors, int colorStride, ReadOnlySpan<float> uv, int uvStride, ReadOnlySpan<sbyte> indices, Texture? texture)
		=> base.TryRenderGeometryRaw(xy, xyStride, colors, colorStride, uv, uvStride, indices, texture);

	/// <inheritdoc cref="Renderer.TryRenderGeometryRaw(ReadOnlySpan{float}, int, ReadOnlySpan{Color{float}}, int, ReadOnlySpan{float}, int, ReadOnlySpan{sbyte}, Texture?)" />
	public bool TryRenderGeometryRaw(ReadOnlySpan<float> xy, int xyStride, ReadOnlySpan<Color<float>> colors, int colorStride, ReadOnlySpan<float> uv, int uvStride, ReadOnlySpan<sbyte> indices, Texture<TDriver>? texture = default)
		=> base.TryRenderGeometryRaw(xy, xyStride, colors, colorStride, uv, uvStride, indices, texture);

	// Hide the non-specialized base method in favor of a strongly-typed overload and divert the user to it
	/// <summary>Use <see cref="TryRenderGeometryRaw(ReadOnlySpan{float}, int, ReadOnlySpan{Color{float}}, int, ReadOnlySpan{float}, int, Texture{TDriver}?)"/> instead</summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	[OverloadResolutionPriority(-1)]
	[Obsolete(
		$"Use {nameof(TryRenderGeometryRaw)}({nameof(ReadOnlySpan<>)}<float>, int, {nameof(ReadOnlySpan<>)}<{nameof(Color<>)}<float>>, int, {nameof(ReadOnlySpan<>)}<float>, int, {nameof(Texture<>)}<{nameof(TDriver)}>?) instead.",
		error: true
	)]
	public new bool TryRenderGeometryRaw(ReadOnlySpan<float> xy, int xyStride, ReadOnlySpan<Color<float>> colors, int colorStride, ReadOnlySpan<float> uv, int uvStride, Texture? texture)
		=> base.TryRenderGeometryRaw(xy, xyStride, colors, colorStride, uv, uvStride, texture);

	/// <inheritdoc cref="Renderer.TryRenderGeometryRaw(ReadOnlySpan{float}, int, ReadOnlySpan{Color{float}}, int, ReadOnlySpan{float}, int, Texture?)"/>
	public bool TryRenderGeometryRaw(ReadOnlySpan<float> xy, int xyStride, ReadOnlySpan<Color<float>> colors, int colorStride, ReadOnlySpan<float> uv, int uvStride, Texture<TDriver>? texture = default)
		=> base.TryRenderGeometryRaw(xy, xyStride, colors, colorStride, uv, uvStride, texture);

	// Hide the non-specialized base method in favor of a strongly-typed overload and divert the user to it
	/// <summary>Use <see cref="TryRenderGeometryRaw(float*, int, Color{float}*, int, float*, int, int, int*, int, Texture{TDriver}?)"/> instead</summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	[OverloadResolutionPriority(-1)]
	[Obsolete(
		$"Use {nameof(TryRenderGeometryRaw)}(float*, int, {nameof(Color<>)}<float>*, int, float*, int, int, int*, int, {nameof(Texture<>)}<{nameof(TDriver)}>?) instead.",
		error: true
	)]
	public new unsafe bool TryRenderGeometryRaw(float* xy, int xyStride, Color<float>* colors, int colorStride, float* uv, int uvStride, int verticesCount, int* indices, int indicesCount, Texture? texture)
		=> base.TryRenderGeometryRaw(xy, xyStride, colors, colorStride, uv, uvStride, verticesCount, indices, indicesCount, texture);

	/// <inheritdoc cref="Renderer.TryRenderGeometryRaw(float*, int, Color{float}*, int, float*, int, int, int*, int, Texture?)" />
	public unsafe bool TryRenderGeometryRaw(float* xy, int xyStride, Color<float>* colors, int colorStride, float* uv, int uvStride, int verticesCount, int* indices, int indicesCount, Texture<TDriver>? texture = default)
		=> base.TryRenderGeometryRaw(xy, xyStride, colors, colorStride, uv, uvStride, verticesCount, indices, indicesCount, texture);

	// Hide the non-specialized base method in favor of a strongly-typed overload and divert the user to it
	/// <summary>Use <see cref="TryRenderGeometryRaw(float*, int, Color{float}*, int, float*, int, int, short*, int, Texture{TDriver}?)"/> instead</summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	[OverloadResolutionPriority(-1)]
	[Obsolete(
		$"Use {nameof(TryRenderGeometryRaw)}(float*, int, {nameof(Color<>)}<float>*, int, float*, int, int, short*, int, {nameof(Texture<>)}<{nameof(TDriver)}>?) instead.",
		error: true
	)]
	public new unsafe bool TryRenderGeometryRaw(float* xy, int xyStride, Color<float>* colors, int colorStride, float* uv, int uvStride, int verticesCount, short* indices, int indicesCount, Texture? texture)
		=> base.TryRenderGeometryRaw(xy, xyStride, colors, colorStride, uv, uvStride, verticesCount, indices, indicesCount, texture);

	/// <inheritdoc cref="Renderer.TryRenderGeometryRaw(float*, int, Color{float}*, int, float*, int, int, short*, int, Texture?)" />
	public unsafe bool TryRenderGeometryRaw(float* xy, int xyStride, Color<float>* colors, int colorStride, float* uv, int uvStride, int verticesCount, short* indices, int indicesCount, Texture<TDriver>? texture = default)
		=> base.TryRenderGeometryRaw(xy, xyStride, colors, colorStride, uv, uvStride, verticesCount, indices, indicesCount, texture);

	// Hide the non-specialized base method in favor of a strongly-typed overload and divert the user to it
	/// <summary>Use <see cref="TryRenderGeometryRaw(float*, int, Color{float}*, int, float*, int, int, sbyte*, int, Texture{TDriver}?)"/> instead</summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	[OverloadResolutionPriority(-1)]
	[Obsolete(
		$"Use {nameof(TryRenderGeometryRaw)}(float*, int, {nameof(Color<>)}<float>*, int, float*, int, int, sbyte*, int, {nameof(Texture<>)}<{nameof(TDriver)}>?) instead.",
		error: true
	)]
	public new unsafe bool TryRenderGeometryRaw(float* xy, int xyStride, Color<float>* colors, int colorStride, float* uv, int uvStride, int verticesCount, sbyte* indices, int indicesCount, Texture? texture)
		=> base.TryRenderGeometryRaw(xy, xyStride, colors, colorStride, uv, uvStride, verticesCount, indices, indicesCount, texture);

	/// <inheritdoc cref="Renderer.TryRenderGeometryRaw(float*, int, Color{float}*, int, float*, int, int, sbyte*, int, Texture?)" />
	public unsafe bool TryRenderGeometryRaw(float* xy, int xyStride, Color<float>* colors, int colorStride, float* uv, int uvStride, int verticesCount, sbyte* indices, int indicesCount, Texture<TDriver>? texture = default)
		=> base.TryRenderGeometryRaw(xy, xyStride, colors, colorStride, uv, uvStride, verticesCount, indices, indicesCount, texture);

	// Hide the non-specialized base method in favor of a strongly-typed overload and divert the user to it
	/// <summary>Use <see cref="TryRenderGeometryRaw(float*, int, Color{float}*, int, float*, int, int, Texture{TDriver}?)"/> instead</summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	[OverloadResolutionPriority(-1)]
	[Obsolete(
		$"Use {nameof(TryRenderGeometryRaw)}(float*, int, {nameof(Color<>)}<float>*, int, float*, int, int, {nameof(Texture<>)}<{nameof(TDriver)}>?) instead.",
		error: true
	)]
	public new unsafe bool TryRenderGeometryRaw(float* xy, int xyStride, Color<float>* colors, int colorStride, float* uv, int uvStride, int verticesCount, Texture? texture)
		=> base.TryRenderGeometryRaw(xy, xyStride, colors, colorStride, uv, uvStride, verticesCount, texture);

	/// <inheritdoc cref="Renderer.TryRenderGeometryRaw(float*, int, Color{float}*, int, float*, int, int, Texture?)"/>
	public unsafe bool TryRenderGeometryRaw(float* xy, int xyStride, Color<float>* colors, int colorStride, float* uv, int uvStride, int verticesCount, Texture<TDriver>? texture = default)
		=> base.TryRenderGeometryRaw(xy, xyStride, colors, colorStride, uv, uvStride, verticesCount, texture);

	// Hide the non-specialized base method in favor of a strongly-typed overload and divert the user to it
	/// <summary>Use <see cref="TryRenderTexture(in Rect{float}, Texture{TDriver}, in Rect{float})"/> instead</summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	[OverloadResolutionPriority(-1)]
	[Obsolete(
		$"Use {nameof(TryRenderTexture)}(in {nameof(Rect<>)}<float>, {nameof(Texture<>)}<{nameof(TDriver)}>, in {nameof(Rect<>)}<float>) instead.",
		error: true
	)]
	public new bool TryRenderTexture(in Rect<float> destinationRect, Texture texture, in Rect<float> sourceRect)
		=> base.TryRenderTexture(in destinationRect, texture, in sourceRect);

	/// <inheritdoc cref="Renderer.TryRenderTexture(in Rect{float}, Texture, in Rect{float})"/>
	public bool TryRenderTexture(in Rect<float> destinationRect, Texture<TDriver> texture, in Rect<float> sourceRect)
		=> base.TryRenderTexture(in destinationRect, texture, in sourceRect);

	// Hide the non-specialized base method in favor of a strongly-typed overload and divert the user to it
	/// <summary>Use <see cref="TryRenderTexture(in Rect{float}, Texture{TDriver})"/> instead</summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	[OverloadResolutionPriority(-1)]
	[Obsolete(
		$"Use {nameof(TryRenderTexture)}(in {nameof(Rect<>)}<float>, {nameof(Texture<>)}<{nameof(TDriver)}>) instead.",
		error: true
	)]
	public new bool TryRenderTexture(in Rect<float> destinationRect, Texture texture)
		=> base.TryRenderTexture(in destinationRect, texture);

	/// <inheritdoc cref="Renderer.TryRenderTexture(in Rect{float}, Texture)"/>
	public bool TryRenderTexture(in Rect<float> destinationRect, Texture<TDriver> texture)
		=> base.TryRenderTexture(in destinationRect, texture);

	// Hide the non-specialized base method in favor of a strongly-typed overload and divert the user to it
	/// <summary>Use <see cref="TryRenderTexture(Texture{TDriver}, in Rect{float})"/> instead</summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	[OverloadResolutionPriority(-1)]
	[Obsolete(
		$"Use {nameof(TryRenderTexture)}({nameof(Texture<>)}<{nameof(TDriver)}>, in {nameof(Rect<>)}<float>) instead.",
		error: true
	)]
	public new bool TryRenderTexture(Texture texture, in Rect<float> sourceRect)
		=> base.TryRenderTexture(texture, in sourceRect);

	/// <inheritdoc cref="Renderer.TryRenderTexture(Texture, in Rect{float})"/>
	public bool TryRenderTexture(Texture<TDriver> texture, in Rect<float> sourceRect)
		=> base.TryRenderTexture(texture, in sourceRect);

	// Hide the non-specialized base method in favor of a strongly-typed overload and divert the user to it
	/// <summary>Use <see cref="TryRenderTexture(Texture{TDriver})"/> instead</summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	[OverloadResolutionPriority(-1)]
	[Obsolete(
		$"Use {nameof(TryRenderTexture)}({nameof(Texture<>)}<{nameof(TDriver)}>) instead.",
		error: true
	)]
	public new bool TryRenderTexture(Texture texture)
		=> base.TryRenderTexture(texture);

	/// <inheritdoc cref="Renderer.TryRenderTexture(Texture)"/>
	public bool TryRenderTexture(Texture<TDriver> texture)
		=> base.TryRenderTexture(texture);

	// Hide the non-specialized base method in favor of a strongly-typed overload and divert the user to it
	/// <summary>Use <see cref="TryRenderTexture9Grid(in Rect{float}, Texture{TDriver}, in Rect{float}, float, float, float, float, float)"/> instead</summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	[OverloadResolutionPriority(-1)]
	[Obsolete(
		$"Use {nameof(TryRenderTexture9Grid)}(in {nameof(Rect<>)}<float>, {nameof(Texture<>)}<{nameof(TDriver)}>, in {nameof(Rect<>)}<float>, float, float, float, float, float) instead.",
		error: true
	)]
	public new bool TryRenderTexture9Grid(in Rect<float> destinationRect, Texture texture, in Rect<float> sourceRect, float leftWidth, float rightWidth, float topHeight, float bottomHeight, float scale)
		=> base.TryRenderTexture9Grid(in destinationRect, texture, in sourceRect, leftWidth, rightWidth, topHeight, bottomHeight, scale);

	/// <inheritdoc cref="Renderer.TryRenderTexture9Grid(in Rect{float}, Texture, in Rect{float}, float, float, float, float, float)"/>
	public bool TryRenderTexture9Grid(in Rect<float> destinationRect, Texture<TDriver> texture, in Rect<float> sourceRect, float leftWidth, float rightWidth, float topHeight, float bottomHeight, float scale)
		=> base.TryRenderTexture9Grid(in destinationRect, texture, in sourceRect, leftWidth, rightWidth, topHeight, bottomHeight, scale);

	// Hide the non-specialized base method in favor of a strongly-typed overload and divert the user to it
	/// <summary>Use <see cref="TryRenderTexture9Grid(in Rect{float}, Texture{TDriver}, float, float, float, float, float)"/> instead</summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	[OverloadResolutionPriority(-1)]
	[Obsolete(
		$"Use {nameof(TryRenderTexture9Grid)}(in {nameof(Rect<>)}<float>, {nameof(Texture<>)}<{nameof(TDriver)}>, float, float, float, float, float) instead.",
		error: true
	)]
	public new bool TryRenderTexture9Grid(in Rect<float> destinationRect, Texture texture, float leftWidth, float rightWidth, float topHeight, float bottomHeight, float scale)
		=> base.TryRenderTexture9Grid(in destinationRect, texture, leftWidth, rightWidth, topHeight, bottomHeight, scale);

	/// <inheritdoc cref="Renderer.TryRenderTexture9Grid(in Rect{float}, Texture, float, float, float, float, float)"/>
	public bool TryRenderTexture9Grid(in Rect<float> destinationRect, Texture<TDriver> texture, float leftWidth, float rightWidth, float topHeight, float bottomHeight, float scale)
		=> base.TryRenderTexture9Grid(in destinationRect, texture, leftWidth, rightWidth, topHeight, bottomHeight, scale);

	// Hide the non-specialized base method in favor of a strongly-typed overload and divert the user to it
	/// <summary>Use <see cref="TryRenderTexture9Grid(Texture{TDriver}, in Rect{float}, float, float, float, float, float)"/> instead</summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	[OverloadResolutionPriority(-1)]
	[Obsolete(
		$"Use {nameof(TryRenderTexture9Grid)}({nameof(Texture<>)}<{nameof(TDriver)}>, in {nameof(Rect<>)}<float>, float, float, float, float, float) instead.",
		error: true
	)]
	public new bool TryRenderTexture9Grid(Texture texture, in Rect<float> sourceRect, float leftWidth, float rightWidth, float topHeight, float bottomHeight, float scale)
		=> base.TryRenderTexture9Grid(texture, in sourceRect, leftWidth, rightWidth, topHeight, bottomHeight, scale);

	/// <inheritdoc cref="Renderer.TryRenderTexture9Grid(Texture, in Rect{float}, float, float, float, float, float)"/>
	public bool TryRenderTexture9Grid(Texture<TDriver> texture, in Rect<float> sourceRect, float leftWidth, float rightWidth, float topHeight, float bottomHeight, float scale)
		=> base.TryRenderTexture9Grid(texture, in sourceRect, leftWidth, rightWidth, topHeight, bottomHeight, scale);

	// Hide the non-specialized base method in favor of a strongly-typed overload and divert the user to it
	/// <summary>Use <see cref="TryRenderTexture9Grid(Texture{TDriver}, float, float, float, float, float)"/> instead</summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	[OverloadResolutionPriority(-1)]
	[Obsolete(
		$"Use {nameof(TryRenderTexture9Grid)}({nameof(Texture<>)}<{nameof(TDriver)}>, float, float, float, float, float) instead.",
		error: true
	)]
	public new bool TryRenderTexture9Grid(Texture texture, float leftWidth, float rightWidth, float topHeight, float bottomHeight, float scale)
		=> base.TryRenderTexture9Grid(texture, leftWidth, rightWidth, topHeight, bottomHeight, scale);

	/// <inheritdoc cref="Renderer.TryRenderTexture9Grid(Texture, float, float, float, float, float)"/>
	public bool TryRenderTexture9Grid(Texture<TDriver> texture, float leftWidth, float rightWidth, float topHeight, float bottomHeight, float scale)
		=> base.TryRenderTexture9Grid(texture, leftWidth, rightWidth, topHeight, bottomHeight, scale);

#if SDL3_4_0_OR_GREATER

	// Hide the non-specialized base method in favor of a strongly-typed overload and divert the user to it
	/// <summary>Use <see cref="TryRenderTexture9GridTiled(in Rect{float}, Texture{TDriver}, in Rect{float}, float, float, float, float, float, float)"/> instead</summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	[OverloadResolutionPriority(-1)]
	[Obsolete(
		$"Use {nameof(TryRenderTexture9GridTiled)}(in {nameof(Rect<>)}<float>, {nameof(Texture<>)}<{nameof(TDriver)}>, in {nameof(Rect<>)}<float>, float, float, float, float, float, float) instead.",
		error: true
	)]
	public new bool TryRenderTexture9GridTiled(in Rect<float> destinationRect, Texture texture, in Rect<float> sourceRect, float leftWidth, float rightWidth, float topHeight, float bottomHeight, float scale, float tileScale)
		=> base.TryRenderTexture9GridTiled(in destinationRect, texture, in sourceRect, leftWidth, rightWidth, topHeight, bottomHeight, scale, tileScale);

	/// <inheritdoc cref="Renderer.TryRenderTexture9GridTiled(in Rect{float}, Texture, in Rect{float}, float, float, float, float, float, float)"/>
	public bool TryRenderTexture9GridTiled(in Rect<float> destinationRect, Texture<TDriver> texture, in Rect<float> sourceRect, float leftWidth, float rightWidth, float topHeight, float bottomHeight, float scale, float tileScale)
		=> base.TryRenderTexture9GridTiled(in destinationRect, texture, in sourceRect, leftWidth, rightWidth, topHeight, bottomHeight, scale, tileScale);

	// Hide the non-specialized base method in favor of a strongly-typed overload and divert the user to it
	/// <summary>Use <see cref="TryRenderTexture9GridTiled(in Rect{float}, Texture{TDriver}, float, float, float, float, float, float)"/> instead</summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	[OverloadResolutionPriority(-1)]
	[Obsolete(
		$"Use {nameof(TryRenderTexture9GridTiled)}(in {nameof(Rect<>)}<float>, {nameof(Texture<>)}<{nameof(TDriver)}>, float, float, float, float, float, float) instead.",
		error: true
	)]
	public new bool TryRenderTexture9GridTiled(in Rect<float> destinationRect, Texture texture, float leftWidth, float rightWidth, float topHeight, float bottomHeight, float scale, float tileScale)
		=> base.TryRenderTexture9GridTiled(in destinationRect, texture, leftWidth, rightWidth, topHeight, bottomHeight, scale, tileScale);

	/// <inheritdoc cref="Renderer.TryRenderTexture9GridTiled(in Rect{float}, Texture, float, float, float, float, float, float)"/>
	public bool TryRenderTexture9GridTiled(in Rect<float> destinationRect, Texture<TDriver> texture, float leftWidth, float rightWidth, float topHeight, float bottomHeight, float scale, float tileScale)
		=> base.TryRenderTexture9GridTiled(in destinationRect, texture, leftWidth, rightWidth, topHeight, bottomHeight, scale, tileScale);

	// Hide the non-specialized base method in favor of a strongly-typed overload and divert the user to it
	/// <summary>Use <see cref="TryRenderTexture9GridTiled(Texture{TDriver}, in Rect{float}, float, float, float, float, float, float)"/> instead</summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	[OverloadResolutionPriority(-1)]
	[Obsolete(
		$"Use {nameof(TryRenderTexture9GridTiled)}({nameof(Texture<>)}<{nameof(TDriver)}>, in {nameof(Rect<>)}<float>, float, float, float, float, float, float) instead.",
		error: true
	)]
	public new bool TryRenderTexture9GridTiled(Texture texture, in Rect<float> sourceRect, float leftWidth, float rightWidth, float topHeight, float bottomHeight, float scale, float tileScale)
		=> base.TryRenderTexture9GridTiled(texture, in sourceRect, leftWidth, rightWidth, topHeight, bottomHeight, scale, tileScale);

	/// <inheritdoc cref="Renderer.TryRenderTexture9GridTiled(Texture, in Rect{float}, float, float, float, float, float, float)"/>
	public bool TryRenderTexture9GridTiled(Texture<TDriver> texture, in Rect<float> sourceRect, float leftWidth, float rightWidth, float topHeight, float bottomHeight, float scale, float tileScale)
		=> base.TryRenderTexture9GridTiled(texture, in sourceRect, leftWidth, rightWidth, topHeight, bottomHeight, scale, tileScale);

	// Hide the non-specialized base method in favor of a strongly-typed overload and divert the user to it
	/// <summary>Use <see cref="TryRenderTexture9GridTiled(Texture{TDriver}, float, float, float, float, float, float)"/> instead</summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	[OverloadResolutionPriority(-1)]
	[Obsolete(
		$"Use {nameof(TryRenderTexture9GridTiled)}({nameof(Texture<>)}<{nameof(TDriver)}>, float, float, float, float, float, float) instead.",
		error: true
	)]
	public new bool TryRenderTexture9GridTiled(Texture texture, float leftWidth, float rightWidth, float topHeight, float bottomHeight, float scale, float tileScale)
		=> base.TryRenderTexture9GridTiled(texture, leftWidth, rightWidth, topHeight, bottomHeight, scale, tileScale);

	/// <inheritdoc cref="Renderer.TryRenderTexture9GridTiled(Texture, float, float, float, float, float, float)"/>
	public bool TryRenderTexture9GridTiled(Texture<TDriver> texture, float leftWidth, float rightWidth, float topHeight, float bottomHeight, float scale, float tileScale)
		=> base.TryRenderTexture9GridTiled(texture, leftWidth, rightWidth, topHeight, bottomHeight, scale, tileScale);

#endif

	// Hide the non-specialized base method in favor of a strongly-typed overload and divert the user to it
	/// <summary>Use <see cref="TryRenderTextureAffine(in Point{float}?, in Point{float}?, in Point{float}?, Texture{TDriver}, in Rect{float})"/> instead</summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	[OverloadResolutionPriority(-1)]
	[Obsolete(
		$"Use {nameof(TryRenderTextureAffine)}(in {nameof(Point<>)}<float>?, in {nameof(Point<>)}<float>?, in {nameof(Point<>)}<float>?, {nameof(Texture<>)}<{nameof(TDriver)}>, in {nameof(Rect<>)}<float>) instead.",
		error: true
	)]
	public new bool TryRenderTextureAffine(in Point<float>? destinationOrigin, in Point<float>? destinationRight, in Point<float>? destinationDown, Texture texture, in Rect<float> sourceRect)
		=> base.TryRenderTextureAffine(in destinationOrigin, in destinationRight, in destinationDown, texture, in sourceRect);

	/// <inheritdoc cref="Renderer.TryRenderTextureAffine(in Point{float}?, in Point{float}?, in Point{float}?, Texture, in Rect{float})"/>
	public bool TryRenderTextureAffine(in Point<float>? destinationOrigin, in Point<float>? destinationRight, in Point<float>? destinationDown, Texture<TDriver> texture, in Rect<float> sourceRect)
		=> base.TryRenderTextureAffine(in destinationOrigin, in destinationRight, in destinationDown, texture, in sourceRect);

	// Hide the non-specialized base method in favor of a strongly-typed overload and divert the user to it
	/// <summary>Use <see cref="TryRenderTextureAffine(in Point{float}?, in Point{float}?, in Point{float}?, Texture{TDriver})"/> instead</summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	[OverloadResolutionPriority(-1)]
	[Obsolete(
		$"Use {nameof(TryRenderTextureAffine)}(in {nameof(Point<>)}<float>?, in {nameof(Point<>)}<float>?, in {nameof(Point<>)}<float>?, {nameof(Texture<>)}<{nameof(TDriver)}>) instead.",
		error: true
	)]
	public new bool TryRenderTextureAffine(in Point<float>? destinationOrigin, in Point<float>? destinationRight, in Point<float>? destinationDown, Texture texture)
		=> base.TryRenderTextureAffine(in destinationOrigin, in destinationRight, in destinationDown, texture);

	/// <inheritdoc cref="Renderer.TryRenderTextureAffine(in Point{float}?, in Point{float}?, in Point{float}?, Texture)"/>
	public bool TryRenderTextureAffine(in Point<float>? destinationOrigin, in Point<float>? destinationRight, in Point<float>? destinationDown, Texture<TDriver> texture)
		=> base.TryRenderTextureAffine(in destinationOrigin, in destinationRight, in destinationDown, texture);

	// Hide the non-specialized base method in favor of a strongly-typed overload and divert the user to it
	/// <summary>Use <see cref="TryRenderTextureRotated(in Rect{float}, Texture{TDriver}, in Rect{float}, double, in Point{float}?, FlipMode)"/> instead</summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	[OverloadResolutionPriority(-1)]
	[Obsolete(
		$"Use {nameof(TryRenderTextureRotated)}(in {nameof(Rect<>)}<float>, {nameof(Texture<>)}<{nameof(TDriver)}>, in {nameof(Rect<>)}<float>, double, in {nameof(Point<>)}<float>?, {nameof(FlipMode)}) instead.",
		error: true
	)]
	public new bool TryRenderTextureRotated(in Rect<float> destinationRect, Texture texture, in Rect<float> sourceRect, double angle, in Point<float>? centerPoint, FlipMode flip)
		=> base.TryRenderTextureRotated(in destinationRect, texture, in sourceRect, angle, in centerPoint, flip);

	/// <inheritdoc cref="Renderer.TryRenderTextureRotated(in Rect{float}, Texture, in Rect{float}, double, in Point{float}?, FlipMode)"/>
	public bool TryRenderTextureRotated(in Rect<float> destinationRect, Texture<TDriver> texture, in Rect<float> sourceRect, double angle, in Point<float>? centerPoint = null, FlipMode flip = FlipMode.None)
		=> base.TryRenderTextureRotated(in destinationRect, texture, in sourceRect, angle, in centerPoint, flip);

	// Hide the non-specialized base method in favor of a strongly-typed overload and divert the user to it
	/// <summary>Use <see cref="TryRenderTextureRotated(in Rect{float}, Texture{TDriver}, double, in Point{float}?, FlipMode)"/> instead</summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	[OverloadResolutionPriority(-1)]
	[Obsolete(
		$"Use {nameof(TryRenderTextureRotated)}(in {nameof(Rect<>)}<float>, {nameof(Texture<>)}<{nameof(TDriver)}>, double, in {nameof(Point<>)}<float>?, {nameof(FlipMode)}) instead.",
		error: true
	)]
	public new bool TryRenderTextureRotated(in Rect<float> destinationRect, Texture texture, double angle, in Point<float>? centerPoint, FlipMode flip)
		=> base.TryRenderTextureRotated(in destinationRect, texture, angle, in centerPoint, flip);

	/// <inheritdoc cref="Renderer.TryRenderTextureRotated(in Rect{float}, Texture, double, in Point{float}?, FlipMode)"/>
	public bool TryRenderTextureRotated(in Rect<float> destinationRect, Texture<TDriver> texture, double angle, in Point<float>? centerPoint = null, FlipMode flip = FlipMode.None)
		=> base.TryRenderTextureRotated(in destinationRect, texture, angle, in centerPoint, flip);

	// Hide the non-specialized base method in favor of a strongly-typed overload and divert the user to it
	/// <summary>Use <see cref="TryRenderTextureRotated(Texture{TDriver}, in Rect{float}, double, in Point{float}?, FlipMode)"/> instead</summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	[OverloadResolutionPriority(-1)]
	[Obsolete(
		$"Use {nameof(TryRenderTextureRotated)}({nameof(Texture<>)}<{nameof(TDriver)}>, in {nameof(Rect<>)}<float>, double, in {nameof(Point<>)}<float>?, {nameof(FlipMode)}) instead.",
		error: true
	)]
	public new bool TryRenderTextureRotated(Texture texture, in Rect<float> sourceRect, double angle, in Point<float>? centerPoint, FlipMode flip)
		=> base.TryRenderTextureRotated(texture, in sourceRect, angle, in centerPoint, flip);

	/// <inheritdoc cref="Renderer.TryRenderTextureRotated(Texture, in Rect{float}, double, in Point{float}?, FlipMode)"/>
	public bool TryRenderTextureRotated(Texture<TDriver> texture, in Rect<float> sourceRect, double angle, in Point<float>? centerPoint = null, FlipMode flip = FlipMode.None)
		=> base.TryRenderTextureRotated(texture, in sourceRect, angle, in centerPoint, flip);

	// Hide the non-specialized base method in favor of a strongly-typed overload and divert the user to it
	/// <summary>Use <see cref="TryRenderTextureRotated(Texture{TDriver}, double, in Point{float}?, FlipMode)"/> instead</summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	[OverloadResolutionPriority(-1)]
	[Obsolete(
		$"Use {nameof(TryRenderTextureRotated)}({nameof(Texture<>)}<{nameof(TDriver)}>, double, in {nameof(Point<>)}<float>?, {nameof(FlipMode)}) instead.",
		error: true
	)]
	public new bool TryRenderTextureRotated(Texture texture, double angle, in Point<float>? centerPoint, FlipMode flip)
		=> base.TryRenderTextureRotated(texture, angle, in centerPoint, flip);

	/// <inheritdoc cref="Renderer.TryRenderTextureRotated(Texture, double, in Point{float}?, FlipMode)"/>
	public bool TryRenderTextureRotated(Texture<TDriver> texture, double angle, in Point<float>? centerPoint = null, FlipMode flip = FlipMode.None)
		=> base.TryRenderTextureRotated(texture, angle, in centerPoint, flip);

	// Hide the non-specialized base method in favor of a strongly-typed overload and divert the user to it
	/// <summary>Use <see cref="TryRenderTextureTiled(in Rect{float}, Texture{TDriver}, in Rect{float}, float)"/> instead</summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	[OverloadResolutionPriority(-1)]
	[Obsolete(
		$"Use {nameof(TryRenderTextureTiled)}(in {nameof(Rect<>)}<float>, {nameof(Texture<>)}<{nameof(TDriver)}>, in {nameof(Rect<>)}<float>, float) instead.",
		error: true
	)]
	public new bool TryRenderTextureTiled(in Rect<float> destinationRect, Texture texture, in Rect<float> sourceRect, float scale)
		=> base.TryRenderTextureTiled(in destinationRect, texture, in sourceRect, scale);

	/// <inheritdoc cref="Renderer.TryRenderTextureTiled(in Rect{float}, Texture, in Rect{float}, float)"/>
	public bool TryRenderTextureTiled(in Rect<float> destinationRect, Texture<TDriver> texture, in Rect<float> sourceRect, float scale)
		=> base.TryRenderTextureTiled(in destinationRect, texture, in sourceRect, scale);

	// Hide the non-specialized base method in favor of a strongly-typed overload and divert the user to it
	/// <summary>Use <see cref="TryRenderTextureTiled(in Rect{float}, Texture{TDriver}, float)"/> instead</summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	[OverloadResolutionPriority(-1)]
	[Obsolete(
		$"Use {nameof(TryRenderTextureTiled)}(in {nameof(Rect<>)}<float>, {nameof(Texture<>)}<{nameof(TDriver)}>, float) instead.",
		error: true
	)]
	public new bool TryRenderTextureTiled(in Rect<float> destinationRect, Texture texture, float scale)
		=> base.TryRenderTextureTiled(in destinationRect, texture, scale);

	/// <inheritdoc cref="Renderer.TryRenderTextureTiled(in Rect{float}, Texture, float)"/>
	public bool TryRenderTextureTiled(in Rect<float> destinationRect, Texture<TDriver> texture, float scale)
		=> base.TryRenderTextureTiled(in destinationRect, texture, scale);

	// Hide the non-specialized base method in favor of a strongly-typed overload and divert the user to it
	/// <summary>Use <see cref="TryRenderTextureTiled(Texture{TDriver}, in Rect{float}, float)"/> instead</summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	[OverloadResolutionPriority(-1)]
	[Obsolete(
		$"Use {nameof(TryRenderTextureTiled)}({nameof(Texture<>)}<{nameof(TDriver)}>, in {nameof(Rect<>)}<float>, float) instead.",
		error: true
	)]
	public new bool TryRenderTextureTiled(Texture texture, in Rect<float> sourceRect, float scale)
		=> base.TryRenderTextureTiled(texture, in sourceRect, scale);

	/// <inheritdoc cref="Renderer.TryRenderTextureTiled(Texture, in Rect{float}, float)"/>
	public bool TryRenderTextureTiled(Texture<TDriver> texture, in Rect<float> sourceRect, float scale)
		=> base.TryRenderTextureTiled(texture, in sourceRect, scale);

	// Hide the non-specialized base method in favor of a strongly-typed overload and divert the user to it
	/// <summary>Use <see cref="TryRenderTextureTiled(Texture{TDriver}, float)"/> instead</summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	[OverloadResolutionPriority(-1)]
	[Obsolete(
		$"Use {nameof(TryRenderTextureTiled)}({nameof(Texture<>)}<{nameof(TDriver)}>, float) instead.",
		error: true
	)]
	public new bool TryRenderTextureTiled(Texture texture, float scale)
		=> base.TryRenderTextureTiled(texture, scale);

	/// <inheritdoc cref="Renderer.TryRenderTextureTiled(Texture, float)"/>
	public bool TryRenderTextureTiled(Texture<TDriver> texture, float scale)
		=> base.TryRenderTextureTiled(texture, scale);
}
