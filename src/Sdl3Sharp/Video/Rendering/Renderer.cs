using Sdl3Sharp.Events;
using Sdl3Sharp.Internal;
using Sdl3Sharp.Internal.Interop;
using Sdl3Sharp.Utilities;
using Sdl3Sharp.Video.Blending;
using Sdl3Sharp.Video.Coloring;
using Sdl3Sharp.Video.Drawing;
using Sdl3Sharp.Video.Gpu;
using Sdl3Sharp.Video.Rendering.Drivers;
using Sdl3Sharp.Video.Windowing;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

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
/// <see cref="Renderer{TDriver}"/> are concrete renderer types, associate with a specific rendering driver.
/// They are used for driver-specific rendering operations with <see cref="Texture{TDriver}"/>s that were created by them.
/// </para>
/// <para>
/// If you want to use them in a more general way, you can use them as <see cref="IRenderer"/> instances, which serve as abstractions to use them for common rendering operations with <see cref="ITexture"/> instance that were created by them.
/// </para>
/// </remarks>
public sealed partial class Renderer<TDriver> : IRenderer
	where TDriver : notnull, IRenderingDriver // we don't need to worry about putting type argument independent code in the Renderer<TDriver> class,
									          // because TDriver surely is always going to be a reference type
									          // (that's because all of our predefined drivers types are reference types and it's impossible for user code to implement the IRenderingDriver interface),
									          // and the JIT will share code for all reference type instantiations
{
	private unsafe IRenderer.SDL_Renderer* mRenderer;

	internal unsafe Renderer(IRenderer.SDL_Renderer* renderer, bool register)
	{
		mRenderer = renderer;

		if (register)
		{
			IRenderer.Register(this);
		}
	}

	/// <inheritdoc/>
	~Renderer() => DisposeImpl(forget: true);

	/// <inheritdoc/>
	public Rect<int> ClippingRect
	{
		get
		{
			unsafe
			{
				Unsafe.SkipInit(out Rect<int> rect);

				ErrorHelper.ThrowIfFailed(IRenderer.SDL_GetRenderClipRect(mRenderer, &rect), filterError: IRenderer.GetInvalidRendererErrorMessage());

				return rect;
			}
		}

		set
		{
			unsafe
			{
				ErrorHelper.ThrowIfFailed(IRenderer.SDL_SetRenderClipRect(mRenderer, &value), filterError: IRenderer.GetInvalidRendererErrorMessage());
			}
		}
	}

	/// <inheritdoc/>
	public float ColorScale
	{
		get
		{
			unsafe
			{
				Unsafe.SkipInit(out float scale);

				ErrorHelper.ThrowIfFailed(IRenderer.SDL_GetRenderColorScale(mRenderer, &scale), filterError: IRenderer.GetInvalidRendererErrorMessage());

				return scale;
			}
		}

		set
		{
			unsafe
			{
				ErrorHelper.ThrowIfFailed(IRenderer.SDL_SetRenderColorScale(mRenderer, value), filterError: IRenderer.GetInvalidRendererErrorMessage());
			}
		}
	}

	/// <inheritdoc/>
	public (int Width, int Height) CurrentOutputSize
	{
		get
		{
			unsafe
			{
				Unsafe.SkipInit(out (int Width, int Height) result);

				ErrorHelper.ThrowIfFailed(IRenderer.SDL_GetCurrentRenderOutputSize(mRenderer, &result.Width, &result.Height), filterError: IRenderer.GetInvalidRendererErrorMessage());

				return result;
			}
		}
	}

#if SDL3_4_0_OR_GREATER

	/// <inheritdoc/>
	public ScaleMode DefaultTextureScaleMode
	{
		get
		{
			unsafe
			{
				Unsafe.SkipInit(out ScaleMode scaleMode);

				ErrorHelper.ThrowIfFailed(IRenderer.SDL_GetDefaultTextureScaleMode(mRenderer, &scaleMode), filterError: IRenderer.GetInvalidRendererErrorMessage());

				return scaleMode;
			}
		}

		set
		{
			unsafe
			{
				ErrorHelper.ThrowIfFailed(IRenderer.SDL_SetDefaultTextureScaleMode(mRenderer, value), filterError: IRenderer.GetInvalidRendererErrorMessage());
			}
		}
	}

#endif

	/// <inheritdoc/>
	public BlendMode DrawBlendMode
	{
		get
		{
			unsafe
			{
				Unsafe.SkipInit(out BlendMode blendMode);

				ErrorHelper.ThrowIfFailed(IRenderer.SDL_GetRenderDrawBlendMode(mRenderer, &blendMode), filterError: IRenderer.GetInvalidRendererErrorMessage());

				return blendMode;
			}
		}

		set
		{
			unsafe
			{
				ErrorHelper.ThrowIfFailed(IRenderer.SDL_SetRenderDrawBlendMode(mRenderer, value), filterError: IRenderer.GetInvalidRendererErrorMessage());
				// throws if value is BlendMode.Invalid or value is an unsupported blend mode for the renderer
				// Although the offical SDL docs say that "If the blend mode is not supported, the closest supported mode is chosen.",
				// that doesn't appear to be the case looking at the SDL source code.
				// It just fails early if the blend mode is not supported and doesn't try to choose a "closest supported mode".
			}
		}
	}

	/// <inheritdoc/>
	public Color<byte> DrawColor
	{
		get
		{
			unsafe
			{
				Unsafe.SkipInit(out (byte R, byte G, byte B, byte A) color);

				ErrorHelper.ThrowIfFailed(IRenderer.SDL_GetRenderDrawColor(mRenderer, &color.R, &color.G, &color.B, &color.A), filterError: IRenderer.GetInvalidRendererErrorMessage());

				return Unsafe.BitCast<(byte R, byte G, byte B, byte A), Color<byte>>(color);
			}
		}

		set
		{
			unsafe
			{
				ErrorHelper.ThrowIfFailed(IRenderer.SDL_SetRenderDrawColor(mRenderer, value.R, value.G, value.B, value.A), filterError: IRenderer.GetInvalidRendererErrorMessage());
			}
		}
	}

	/// <inheritdoc/>
	public Color<float> DrawColorFloat
	{
		get
		{
			unsafe
			{
				Unsafe.SkipInit(out (float R, float G, float B, float A) color);

				ErrorHelper.ThrowIfFailed(IRenderer.SDL_GetRenderDrawColorFloat(mRenderer, &color.R, &color.G, &color.B, &color.A), filterError: IRenderer.GetInvalidRendererErrorMessage());

				return Unsafe.BitCast<(float R, float G, float B, float A), Color<float>>(color);
			}
		}

		set
		{
			unsafe
			{
				ErrorHelper.ThrowIfFailed(IRenderer.SDL_SetRenderDrawColorFloat(mRenderer, value.R, value.G, value.B, value.A), filterError: IRenderer.GetInvalidRendererErrorMessage());
			}
		}
	}

	/// <inheritdoc/>
	public float HdrHeadroom => Properties?.TryGetFloatValue(IRenderer.PropertyNames.HdrHeadroomFloat, out var hdrHeadroom) is true
		? hdrHeadroom
		: default;

	/// <inheritdoc/>
	public bool IsClippingEnabled
	{
		get
		{
			unsafe
			{
				return IRenderer.SDL_RenderClipEnabled(mRenderer);
			}
		}
	}

	/// <inheritdoc/>
	public bool IsHdrEnabled => Properties?.TryGetBooleanValue(IRenderer.PropertyNames.HdrEnabledBoolean, out var isHdrEnabled) is true
		&& isHdrEnabled;

	/// <inheritdoc/>
	public bool IsViewportSet
	{
		get
		{
			unsafe
			{
				return IRenderer.SDL_RenderViewportSet(mRenderer);
			}
		}

	}

	/// <inheritdoc/>
	public (int Width, int Height, RendererLogicalPresentation Mode) LogicalPresentation
	{
		get
		{
			unsafe
			{
				Unsafe.SkipInit(out (int Width, int Height, RendererLogicalPresentation Mode) result);

				ErrorHelper.ThrowIfFailed(IRenderer.SDL_GetRenderLogicalPresentation(mRenderer, &result.Width, &result.Height, &result.Mode), filterError: IRenderer.GetInvalidRendererErrorMessage());

				return result;
			}
		}

		set
		{
			unsafe
			{
				ErrorHelper.ThrowIfFailed(IRenderer.SDL_SetRenderLogicalPresentation(mRenderer, value.Width, value.Height, value.Mode), filterError: IRenderer.GetInvalidRendererErrorMessage());
			}
		}
	}

	/// <inheritdoc/>
	public Rect<float> LogicalPresentationRect
	{
		get
		{
			unsafe
			{
				Unsafe.SkipInit(out Rect<float> rect);

				ErrorHelper.ThrowIfFailed(IRenderer.SDL_GetRenderLogicalPresentationRect(mRenderer, &rect), filterError: IRenderer.GetInvalidRendererErrorMessage());

				return rect;
			}
		}
	}

	/// <inheritdoc/>
	public int MaximumTextureSize
	{
		get => Properties?.TryGetNumberValue(IRenderer.PropertyNames.MaxTextureSizeNumber, out var size) is true
			? unchecked((int)size)
			: default;
	}

	/// <inheritdoc/>
	public string? Name
	{
		get
		{
			unsafe
			{
				return Utf8StringMarshaller.ConvertToManaged(IRenderer.SDL_GetRendererName(mRenderer));
			}
		}
	}

	/// <inheritdoc/>
	public ColorSpace OutputColorSpace => Properties?.TryGetNumberValue(IRenderer.PropertyNames.OutputColorSpaceNumber, out var colorSpace) is true
		? unchecked((ColorSpace)colorSpace)
		: default;

	/// <inheritdoc/>
	public (int Width, int Height) OutputSize
	{
		get
		{
			unsafe
			{
				Unsafe.SkipInit(out (int Width, int Height) result);

				ErrorHelper.ThrowIfFailed(IRenderer.SDL_GetRenderOutputSize(mRenderer, &result.Width, &result.Height), filterError: IRenderer.GetInvalidRendererErrorMessage());

				return result;
			}
		}
	}

	/// <inheritdoc/>
	public Properties? Properties
	{
		get
		{
			unsafe
			{
				return IRenderer.SDL_GetRendererProperties(mRenderer) switch
				{
					0 => null,
					var id => Properties.GetOrCreate(sdl: null, id)
				};
			}
		}
	}

	internal unsafe IRenderer.SDL_Renderer* Pointer { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mRenderer; }

	unsafe IRenderer.SDL_Renderer* IRenderer.Pointer { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => Pointer; }

	/// <inheritdoc/>
	public Rect<int> SafeArea
	{
		get
		{
			unsafe
			{
				Unsafe.SkipInit(out Rect<int> rect);

				ErrorHelper.ThrowIfFailed(IRenderer.SDL_GetRenderSafeArea(mRenderer, &rect), filterError: IRenderer.GetInvalidRendererErrorMessage());

				return rect;
			}
		}
	}

	/// <inheritdoc/>
	public (float ScaleX, float ScaleY) Scale
	{
		get
		{
			unsafe
			{
				Unsafe.SkipInit(out (float ScaleX, float ScaleY) scale);

				ErrorHelper.ThrowIfFailed(IRenderer.SDL_GetRenderScale(mRenderer, &scale.ScaleX, &scale.ScaleY), filterError: IRenderer.GetInvalidRendererErrorMessage());

				return scale;
			}
		}

		set
		{
			unsafe
			{
				ErrorHelper.ThrowIfFailed(IRenderer.SDL_SetRenderScale(mRenderer, value.ScaleX, value.ScaleY), filterError: IRenderer.GetInvalidRendererErrorMessage());
			}
		}
	}

	/// <inheritdoc/>
	public float SdrWhitePoint => Properties?.TryGetFloatValue(IRenderer.PropertyNames.SdrWhitePointFloat, out var sdrWhitePoint) is true
		? sdrWhitePoint
		: default;

	/// <inheritdoc/>
	public IEnumerable<PixelFormat> SupportedTextureFormats
	{
		get
		{
			if (Properties?.TryGetPointerValue(IRenderer.PropertyNames.TextureFormatsPointer, out var textureFormatsPtr) is true)
			{
				unsafe
				{
					if (unchecked((PixelFormat*)textureFormatsPtr) is null)
					{
						failTextureFormatsNull();
					}
				}

				var startPtr = textureFormatsPtr;
				var currentPtr = startPtr;

				while (true)
				{
					Unsafe.SkipInit(out PixelFormat current);
					unsafe
					{
						current = *unchecked((PixelFormat*)currentPtr++);
					}

					if (current is PixelFormat.Unknown)
					{
						break;
					}

					yield return current;

					if (Properties?.TryGetPointerValue(IRenderer.PropertyNames.TextureFormatsPointer, out textureFormatsPtr) is not true
						|| startPtr != textureFormatsPtr)
					{
						failTextureFormatsChanged();
					}
				}
			}

			[DoesNotReturn]
			static void failTextureFormatsNull() => throw new InvalidOperationException(message: "The reference to the texture formats is invalid");

			[DoesNotReturn]
			static void failTextureFormatsChanged() => throw new InvalidOperationException(message: "The reference to the texture formats has changed or the texture formats list itself has changed during the enumeration");
		}
	}

	/// <inheritdoc/>
	public bool SupportsNonPowerOfTwoTextureWrapping => Properties?.TryGetBooleanValue(IRenderer.PropertyNames.TextureWrappingBoolean, out var supportsWrapping) is true
		&& supportsWrapping;

	private void SetTargetImpl<TTexture>(TTexture? value)
		where TTexture : notnull, ITexture
	{
		unsafe
		{
			ITexture.SDL_Texture* texturePtr;
			if (value is null)
			{
				texturePtr = null;
			}
			else
			{
				texturePtr = value.Pointer;

				// This is actually one of the rare cases where we need to check if the object has been disposed manually.
				// That's because SDL_SetRenderTarget(..., null) has special semantics, in that it resets the render target to the window, if no texture is explicitly set as the render target (the texture argument is NULL).
				// Of course, if the user gives us a non-null Texture (managed), they expect it to be set as the SDL_Texture (native) render target.
				// If the given Texture has already been disposed, there's no SDL_Texture to set, its underlying SDL_Texture* pointer will be null.
				if (texturePtr is null)
				{
					failValueArgumentDisposed();
				}
			}

			ErrorHelper.ThrowIfFailed(IRenderer.SDL_SetRenderTarget(Pointer, texturePtr), filterError: IRenderer.GetInvalidRendererErrorMessage());
		}

		[DoesNotReturn]
		static void failValueArgumentDisposed() => throw new ObjectDisposedException(nameof(value), message: $"The given {nameof(ITexture)} has already been disposed");
	}

	/// <inheritdoc cref="IRenderer.Target"/>
	public Texture<TDriver>? Target
	{
		get
		{
			unsafe
			{
				Texture<TDriver>.TryGetOrCreate(IRenderer.SDL_GetRenderTarget(mRenderer), out var texture);
				return texture;
			}
		}

		set => SetTargetImpl(value);
	}

	/// <inheritdoc/>
	ITexture? IRenderer.Target
	{
		get => Target;
		set => SetTargetImpl(value);
	}

#if SDL3_4_0_OR_GREATER

	/// <inheritdoc/>
	public (TextureAddressMode UMode, TextureAddressMode VMode) TextureAddressMode
	{
		get
		{
			unsafe
			{
				Unsafe.SkipInit(out (TextureAddressMode UMode, TextureAddressMode VMode) modes);

				ErrorHelper.ThrowIfFailed(IRenderer.SDL_GetRenderTextureAddressMode(mRenderer, &modes.UMode, &modes.VMode), filterError: IRenderer.GetInvalidRendererErrorMessage());

				return modes;
			}
		}

		set
		{
			unsafe
			{
				ErrorHelper.ThrowIfFailed(IRenderer.SDL_SetRenderTextureAddressMode(mRenderer, value.UMode, value.VMode), filterError: IRenderer.GetInvalidRendererErrorMessage());
			}
		}
	}

#endif

	/// <inheritdoc/>
	public Rect<int> Viewport
	{
		get
		{
			unsafe
			{
				Unsafe.SkipInit(out Rect<int> rect);

				ErrorHelper.ThrowIfFailed(IRenderer.SDL_GetRenderViewport(mRenderer, &rect), filterError: IRenderer.GetInvalidRendererErrorMessage());

				return rect;
			}
		}

		set
		{
			unsafe
			{
				ErrorHelper.ThrowIfFailed(IRenderer.SDL_SetRenderViewport(mRenderer, &value), filterError: IRenderer.GetInvalidRendererErrorMessage());
			}
		}
	}

	/// <inheritdoc/>
	public RendererVSync VSync
	{
		get
		{
			unsafe
			{
				Unsafe.SkipInit(out RendererVSync vsync);

				ErrorHelper.ThrowIfFailed(IRenderer.SDL_GetRenderVSync(mRenderer, &vsync), filterError: IRenderer.GetInvalidRendererErrorMessage());

				return vsync;
			}
		}

		set
		{
			unsafe
			{
				ErrorHelper.ThrowIfFailed(IRenderer.SDL_SetRenderVSync(mRenderer, value), filterError: IRenderer.GetInvalidRendererErrorMessage());
			}
		}
	}

	/// <inheritdoc/>
	public Window? Window
	{
		get
		{
			unsafe
			{
				Window.TryGetOrCreate(IRenderer.SDL_GetRenderWindow(mRenderer), out var window);

				return window;
			}
		}
	}

	/// <inheritdoc/>
	public void Dispose()
	{
		GC.SuppressFinalize(this);
		DisposeImpl(forget: true);
	}

	private void DisposeImpl(bool forget)
	{
		unsafe
		{
			if (mRenderer is not null)
			{
				if (forget)
				{
					IRenderer.Deregister(this);
				}

				IRenderer.SDL_DestroyRenderer(mRenderer);
				mRenderer = null;
			}
		}
	}

	void IRenderer.Dispose(bool disposing, bool forget) => DisposeImpl(forget);

	/// <inheritdoc/>
	public void ResetClippingRect()
	{
		unsafe
		{
			ErrorHelper.ThrowIfFailed(IRenderer.SDL_SetRenderClipRect(mRenderer, rect: null), filterError: IRenderer.GetInvalidRendererErrorMessage());
		}
	}

	/// <inheritdoc/>
	public void ResetTarget()
	{
		unsafe
		{
			ErrorHelper.ThrowIfFailed(IRenderer.SDL_SetRenderTarget(mRenderer, texture: null), filterError: IRenderer.GetInvalidRendererErrorMessage());
		}
	}

	/// <inheritdoc/>
	public void ResetViewport()
	{
		unsafe
		{
			ErrorHelper.ThrowIfFailed(IRenderer.SDL_SetRenderViewport(mRenderer, rect: null), filterError: IRenderer.GetInvalidRendererErrorMessage());
		}
	}

	/// <inheritdoc/>
	public bool TryClear()
	{
		unsafe
		{
			return IRenderer.SDL_RenderClear(mRenderer);
		}
	}

	/// <inheritdoc/>
	public bool TryConvertRenderToWindowCoordinates(float x, float y, out float windowX, out float windowY)
	{
		unsafe
		{
			Unsafe.SkipInit(out float xTmp);
			Unsafe.SkipInit(out float yTmp);

			bool result = IRenderer.SDL_RenderCoordinatesToWindow(mRenderer, x, y, &xTmp, &yTmp);

			windowX = xTmp;
			windowY = yTmp;

			return result;
		}
	}

	/// <inheritdoc/>
	public bool TryConvertWindowToRenderCoordinates(float windowX, float windowY, out float x, out float y)
	{
		unsafe
		{
			Unsafe.SkipInit(out float xTmp);
			Unsafe.SkipInit(out float yTmp);

			bool result = IRenderer.SDL_RenderCoordinatesFromWindow(mRenderer, windowX, windowY, &xTmp, &yTmp);

			x = xTmp;
			y = yTmp;

			return result;
		}
	}

	/// <inheritdoc cref="IRenderer.TryCreateTexture(PixelFormat, TextureAccess, int, int, out ITexture?)"/>
	public bool TryCreateTexture(PixelFormat format, TextureAccess access, int width, int height, [NotNullWhen(true)] out Texture<TDriver>? texture)
	{
		unsafe
		{
			var texturePtr = ITexture.SDL_CreateTexture(mRenderer, format, access, width, height);

			if (texturePtr is null)
			{
				texture = null;
				return false;
			}

			texture = new(texturePtr, register: true);
			return true;
		}
	}

	/// <inheritdoc/>
	bool IRenderer.TryCreateTexture(PixelFormat format, TextureAccess access, int width, int height, [NotNullWhen(true)] out ITexture? texture)
	{
		var result = TryCreateTexture(format, access, width, height, out var typedTexture);

		texture = typedTexture;

		return result;
	}

#if SDL3_4_0_OR_GREATER
	/// <inheritdoc cref="IRenderer.TryCreateTexture(out ITexture?, ColorSpace?, PixelFormat?, TextureAccess?, int?, int?, Palette?, float?, float?, Properties?)"/>
#else
	/// <inheritdoc cref="IRenderer.TryCreateTexture(out ITexture?, ColorSpace?, PixelFormat?, TextureAccess?, int?, int?, float?, float?, Properties?)"/>
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
					propertiesUsed.TrySetNumberValue(ITexture.PropertyNames.CreateColorSpaceNumber, unchecked((uint)colorSpaceValue));
				}

				if (format is PixelFormat formatValue)
				{
					propertiesUsed.TrySetNumberValue(ITexture.PropertyNames.CreateFormatNumber, unchecked((uint)formatValue));
				}

				if (access is TextureAccess accessValue)
				{
					propertiesUsed.TrySetNumberValue(ITexture.PropertyNames.CreateAccessNumber, unchecked((int)accessValue));
				}

				if (width is int widthValue)
				{
					// actually, width and height are required when creating a texture, but we'll let SDL fail and set the error message accordingly

					propertiesUsed.TrySetNumberValue(ITexture.PropertyNames.CreateWidthNumber, widthValue);
				}

				if (height is int heightValue)
				{
					// actually, width and height are required when creating a texture, but we'll let SDL fail and set the error message accordingly

					propertiesUsed.TrySetNumberValue(ITexture.PropertyNames.CreateHeightNumber, heightValue);
				}

#if SDL3_4_0_OR_GREATER

				if (palette is { Pointer: var palettePtr })
				{
					propertiesUsed.TrySetPointerValue(ITexture.PropertyNames.CreatePalettePointer, unchecked((IntPtr)palettePtr));
				}

#endif

				if (sdrWhitePoint is float sdrWhitePointValue)
				{
					propertiesUsed.TrySetFloatValue(ITexture.PropertyNames.CreateSdrWhitePointFloat, sdrWhitePointValue);
				}

				if (hdrHeadroom is float hdrHeadroomValue)
				{
					propertiesUsed.TrySetFloatValue(ITexture.PropertyNames.CreateHdrHeadroomFloat, hdrHeadroomValue);
				}
			}
			else
			{
				propertiesUsed = properties;

				if (colorSpace is ColorSpace colorSpaceValue)
				{
					colorSpaceBackup = propertiesUsed.TryGetNumberValue(ITexture.PropertyNames.CreateColorSpaceNumber, out var existingColorSpaceValue)
						? unchecked((ColorSpace)existingColorSpaceValue)
						: null;

					propertiesUsed.TrySetNumberValue(ITexture.PropertyNames.CreateColorSpaceNumber, unchecked((uint)colorSpaceValue));
				}

				if (format is PixelFormat formatValue)
				{
					formatBackup = propertiesUsed.TryGetNumberValue(ITexture.PropertyNames.CreateFormatNumber, out var existingFormatValue)
						? unchecked((PixelFormat)existingFormatValue)
						: null;

					propertiesUsed.TrySetNumberValue(ITexture.PropertyNames.CreateFormatNumber, unchecked((uint)formatValue));
				}

				if (access is TextureAccess accessValue)
				{
					accessBackup = propertiesUsed.TryGetNumberValue(ITexture.PropertyNames.CreateAccessNumber, out var existingAccessValue)
						? unchecked((TextureAccess)existingAccessValue)
						: null;

					propertiesUsed.TrySetNumberValue(ITexture.PropertyNames.CreateAccessNumber, unchecked((int)accessValue));
				}

				if (width is int widthValue)
				{
					// actually, width and height are required when creating a texture, but they could already exist in the given properties and if not we'll let SDL fail and set the error message accordingly

					widthBackup = propertiesUsed.TryGetNumberValue(ITexture.PropertyNames.CreateWidthNumber, out var existingWidthValue)
						? unchecked((int)existingWidthValue)
						: null;

					propertiesUsed.TrySetNumberValue(ITexture.PropertyNames.CreateWidthNumber, widthValue);
				}

				if (height is int heightValue)
				{
					// actually, width and height are required when creating a texture, but they could already exist in the given properties and if not we'll let SDL fail and set the error message accordingly

					heightBackup = propertiesUsed.TryGetNumberValue(ITexture.PropertyNames.CreateHeightNumber, out var existingHeightValue)
						? unchecked((int)existingHeightValue)
						: null;

					propertiesUsed.TrySetNumberValue(ITexture.PropertyNames.CreateHeightNumber, heightValue);
				}

#if SDL3_4_0_OR_GREATER

				if (palette is { Pointer: var palettePtr })
				{
					paletteBackup = propertiesUsed.TryGetPointerValue(ITexture.PropertyNames.CreatePalettePointer, out var existingPalettePtr)
						? existingPalettePtr
						: null;

					propertiesUsed.TrySetPointerValue(ITexture.PropertyNames.CreatePalettePointer, unchecked((IntPtr)palettePtr));
				}

#endif

				if (sdrWhitePoint is float sdrWhitePointValue)
				{
					sdrWhitePointBackup = propertiesUsed.TryGetFloatValue(ITexture.PropertyNames.CreateSdrWhitePointFloat, out var existingSdrWhitePointValue)
						? existingSdrWhitePointValue
						: null;

					propertiesUsed.TrySetFloatValue(ITexture.PropertyNames.CreateSdrWhitePointFloat, sdrWhitePointValue);
				}

				if (hdrHeadroom is float hdrHeadroomValue)
				{
					hdrHeadroomBackup = propertiesUsed.TryGetFloatValue(ITexture.PropertyNames.CreateHdrHeadroomFloat, out var existingHdrHeadroomValue)
						? existingHdrHeadroomValue
						: null;

					propertiesUsed.TrySetFloatValue(ITexture.PropertyNames.CreateHdrHeadroomFloat, hdrHeadroomValue);
				}
			}

			try
			{
				var texturePtr = ITexture.SDL_CreateTextureWithProperties(mRenderer, propertiesUsed.Id);

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
							propertiesUsed.TrySetNumberValue(ITexture.PropertyNames.CreateColorSpaceNumber, unchecked((uint)colorSpaceValue));

						}
						else
						{
							propertiesUsed.TryRemove(ITexture.PropertyNames.CreateColorSpaceNumber);
						}
					}

					if (format.HasValue)
					{
						if (formatBackup is PixelFormat formatValue)
						{
							propertiesUsed.TrySetNumberValue(ITexture.PropertyNames.CreateFormatNumber, unchecked((uint)formatValue));
						}
						else
						{
							propertiesUsed.TryRemove(ITexture.PropertyNames.CreateFormatNumber);
						}
					}

					if (access.HasValue)
					{
						if (accessBackup is TextureAccess accessValue)
						{
							propertiesUsed.TrySetNumberValue(ITexture.PropertyNames.CreateAccessNumber, unchecked((int)accessValue));
						}
						else
						{
							propertiesUsed.TryRemove(ITexture.PropertyNames.CreateAccessNumber);
						}
					}

					if (width.HasValue)
					{
						if (widthBackup is int widthValue)
						{
							propertiesUsed.TrySetNumberValue(ITexture.PropertyNames.CreateWidthNumber, widthValue);
						}
						else
						{
							propertiesUsed.TryRemove(ITexture.PropertyNames.CreateWidthNumber);
						}
					}

					if (height.HasValue)
					{
						if (heightBackup is int heightValue)
						{
							propertiesUsed.TrySetNumberValue(ITexture.PropertyNames.CreateHeightNumber, heightValue);
						}
						else
						{
							propertiesUsed.TryRemove(ITexture.PropertyNames.CreateHeightNumber);
						}
					}

#if SDL3_4_0_OR_GREATER

					if (palette is not null)
					{
						if (paletteBackup is IntPtr palettePtr)
						{
							propertiesUsed.TrySetPointerValue(ITexture.PropertyNames.CreatePalettePointer, palettePtr);
						}
						else
						{
							propertiesUsed.TryRemove(ITexture.PropertyNames.CreatePalettePointer);
						}
					}

#endif

					if (sdrWhitePoint.HasValue)
					{
						if (sdrWhitePointBackup is float sdrWhitePointValue)
						{
							propertiesUsed.TrySetFloatValue(ITexture.PropertyNames.CreateSdrWhitePointFloat, sdrWhitePointValue);
						}
						else
						{
							propertiesUsed.TryRemove(ITexture.PropertyNames.CreateSdrWhitePointFloat);
						}
					}

					if (hdrHeadroom.HasValue)
					{
						if (hdrHeadroomBackup is float hdrHeadroomValue)
						{
							propertiesUsed.TrySetFloatValue(ITexture.PropertyNames.CreateHdrHeadroomFloat, hdrHeadroomValue);
						}
						else
						{
							propertiesUsed.TryRemove(ITexture.PropertyNames.CreateHdrHeadroomFloat);
						}
					}
				}
			}
		}
	}

	/// <inheritdoc/>
	bool IRenderer.TryCreateTexture([NotNullWhen(true)] out ITexture? texture, ColorSpace? colorSpace, PixelFormat? format, TextureAccess? access, int? width, int? height, Palette? palette, float? sdrWhitePoint, float? hdrHeadroom, Properties? properties)
	{
		var result = TryCreateTexture(out var typedTexture, colorSpace, format, access, width, height, palette, sdrWhitePoint, hdrHeadroom, properties);

		texture = typedTexture;

		return result;
	}

	/// <inheritdoc cref="IRenderer.TryCreateTextureFromSurface(Surface, out ITexture?)"/>
	public bool TryCreateTextureFromSurface(Surface surface, [NotNullWhen(true)] out Texture<TDriver>? texture)
	{
		unsafe
		{
			var texturePtr = ITexture.SDL_CreateTextureFromSurface(mRenderer, surface is not null ? surface.Pointer : null);

			if (texturePtr is null)
			{
				texture = null;
				return false;
			}

			texture = new(texturePtr, register: true);
			return true;
		}
	}

	/// <inheritdoc/>
	bool IRenderer.TryCreateTextureFromSurface(Surface surface, [NotNullWhen(true)] out ITexture? texture)
	{
		var result = TryCreateTextureFromSurface(surface, out var typedTexture);

		texture = typedTexture;

		return result;
	}

	/// <inheritdoc/>
	public bool TryFlush()
	{
		unsafe
		{
			return IRenderer.SDL_FlushRenderer(mRenderer);
		}
	}

	internal unsafe static bool TryGetOrCreate(IRenderer.SDL_Renderer* renderer, [NotNullWhen(true)] out Renderer<TDriver>? result)
		=> IRenderer.TryGetOrCreate(renderer, out result);

	/// <inheritdoc/>
	public bool TryReadPixels(in Rect<int> rect, [NotNullWhen(true)] out Surface? pixels)
	{
		unsafe
		{
			Surface.SDL_Surface* surfacePtr;
			fixed (Rect<int>* rectPtr = &rect)
			{
				surfacePtr = IRenderer.SDL_RenderReadPixels(mRenderer, rectPtr);
			}

			if (surfacePtr is null)
			{
				pixels = null;
				return false;
			}

			// Interestingly, we can't use Surface.TryGetOrCreate here, because that would *additionally* increase its ref counter,
			// but the resulting surface is detached from the renderer's lifetime, and is explicitly stated as needing to be freed with SDL_DestroySurface.
			// That means, that if we were to use Surface.TryGetOrCreate here, the ref counter would be off by one, resulting in the need to call SDL_DestroySurface twice.
			// Therefore, this is one of the rare cases where we need to call the Surface constructor directly.
			pixels = new Surface(surfacePtr);
			return true;
		}
	}

	/// <inheritdoc/>
	public bool TryReadPixels([NotNullWhen(true)] out Surface? pixels)
	{
		unsafe
		{
			var surfacePtr = IRenderer.SDL_RenderReadPixels(mRenderer, rect: null);

			if (surfacePtr is null)
			{
				pixels = null;
				return false;
			}

			// Interestingly, we can't use Surface.TryGetOrCreate here, because that would *additionally* increase its ref counter,
			// but the resulting surface is detached from the renderer's lifetime, and is explicitly stated as needing to be freed with SDL_DestroySurface.
			// That means, that if we were to use Surface.TryGetOrCreate here, the ref counter would be off by one, resulting in the need to call SDL_DestroySurface twice.
			// Therefore, this is one of the rare cases where we need to call the Surface constructor directly.
			pixels = new Surface(surfacePtr);
			return true;
		}
	}

	/// <inheritdoc/>
	public bool TryRenderDebugText(float x, float y, string text)
	{
		unsafe
		{
			var textUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(text);
			try
			{
				return IRenderer.SDL_RenderDebugText(mRenderer, x, y, textUtf8);
			}
			finally
			{
				Utf8StringMarshaller.Free(textUtf8);
			}
		}
	}

	/// <inheritdoc/>
	public bool TryRenderDebugText(float x, float y, string format, params ReadOnlySpan<object> args)
		=> Variadic.Invoke<CBool>(in IRenderer.SDL_RenderDebugTextFormat_var(), 3, [x, y, format, .. args]);

	/// <inheritdoc/>
	public bool TryRenderFilledRect(in Rect<float> rect)
	{
		unsafe
		{
			fixed (Rect<float>* rectPtr = &rect)
			{
				return IRenderer.SDL_RenderFillRect(mRenderer, rectPtr);
			}
		}
	}

	/// <inheritdoc/>
	public bool TryRenderFilledRect()
	{
		unsafe
		{
			return IRenderer.SDL_RenderFillRect(mRenderer, rect: null);
		}
	}

	/// <inheritdoc/>
	public bool TryRenderFilledRects(ReadOnlySpan<Rect<float>> rects)
	{
		unsafe
		{
			fixed (Rect<float>* rectsPtr = rects)
			{
				return IRenderer.SDL_RenderFillRects(mRenderer, rectsPtr, rects.Length);
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private unsafe static bool TryGetTexturePointer<TTexture>(TTexture? texture, out ITexture.SDL_Texture* texturePtr)
		where TTexture : notnull, ITexture
	{
		if (texture is null)
		{
			texturePtr = null;
			return true;
		}
		else if (texture.Pointer is var pointer && pointer is not null)
		{
			texturePtr = pointer;
			return true;
		}
		else
		{
			// if we were given a non-null texture that has already been disposed, we fail here
			texturePtr = null;
			return false;
		}
	}

	private bool TryRenderGeometryImpl<TTexture>(ReadOnlySpan<Vertex> vertices, ReadOnlySpan<int> indices, TTexture? texture)
		where TTexture : notnull, ITexture
	{
		unsafe
		{
			if (!TryGetTexturePointer(texture, out var texturePtr))
			{
				// if we were given a non-null texture that has already been disposed, we fail early here
				return false;
			}

			fixed (Vertex* verticesPtr = vertices)
			fixed (int* indicesPtr = indices)
			{
				return IRenderer.SDL_RenderGeometry(Pointer, texturePtr, verticesPtr, vertices.Length, indicesPtr, indices.Length);
			}
		}
	}

	/// <inheritdoc cref="IRenderer.TryRenderGeometry(ReadOnlySpan{Vertex}, ReadOnlySpan{int}, ITexture?)"/>
	public bool TryRenderGeometry(ReadOnlySpan<Vertex> vertices, ReadOnlySpan<int> indices, Texture<TDriver>? texture = default)
		=> TryRenderGeometryImpl(vertices, indices, texture);

	/// <inheritdoc/>
	bool IRenderer.TryRenderGeometry(ReadOnlySpan<Vertex> vertices, ReadOnlySpan<int> indices, ITexture? texture)
		=> TryRenderGeometryImpl(vertices, indices, texture);

	private bool TryRenderGeometryImpl<TTexture>(ReadOnlySpan<Vertex> vertices, TTexture? texture)
		where TTexture : notnull, ITexture
	{
		unsafe
		{
			if (!TryGetTexturePointer(texture, out var texturePtr))
			{
				// if we were given a non-null texture that has already been disposed, we fail early here
				return false;
			}

			fixed (Vertex* verticesPtr = vertices)
			{
				return IRenderer.SDL_RenderGeometry(Pointer, texturePtr, verticesPtr, vertices.Length, indices: null, num_indices: 0);
			}
		}
	}

	/// <inheritdoc cref="IRenderer.TryRenderGeometry(ReadOnlySpan{Vertex}, ITexture?)" />
	public bool TryRenderGeometry(ReadOnlySpan<Vertex> vertices, Texture<TDriver>? texture = default)
		=> TryRenderGeometryImpl(vertices, texture);

	/// <inheritdoc/>
	bool IRenderer.TryRenderGeometry(ReadOnlySpan<Vertex> vertices, ITexture? texture)
		=> TryRenderGeometryImpl(vertices, texture);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private static int GetVerticesCount(ReadOnlyNativeMemory<float> xy, int xyStride, ReadOnlyNativeMemory<Color<float>> colors, int colorStride, ReadOnlyNativeMemory<float> uv, int uvStride)
	{
		// calculate the number of vertices based on the lengths and strides of the inputs and selecting the minimum among them
		return unchecked((int)nuint.Min(
			xyStride is > 0 ? unchecked(((ReadOnlyNativeMemory)xy).Length / (nuint)xyStride) : 0,
			nuint.Min(
				colorStride is > 0 ? unchecked(((ReadOnlyNativeMemory)colors).Length / (nuint)colorStride) : 0,
				nuint.Min(
					uvStride is > 0 ? unchecked(((ReadOnlyNativeMemory)uv).Length / (nuint)uvStride) : 0,
					int.MaxValue
				)
			)
		));
	}

	private bool TryRenderGeometryRawImpl<TTexture, TIndex>(ReadOnlyNativeMemory<float> xy, int xyStride, ReadOnlyNativeMemory<Color<float>> colors, int colorStride, ReadOnlyNativeMemory<float> uv, int uvStride, ReadOnlySpan<TIndex> indices, TTexture? texture)
		where TTexture : notnull, ITexture
		where TIndex : unmanaged
	{
		unsafe
		{
			if (!xy.IsValid || !colors.IsValid || !uv.IsValid)
			{
				return false;
			}

			if (!TryGetTexturePointer(texture, out var texturePtr))
			{
				// if we were given a non-null texture that has already been disposed, we fail early here
				return false;
			}

			var verticesCount = GetVerticesCount(xy, xyStride, colors, colorStride, uv, uvStride);

			fixed (TIndex* indicesPtr = indices)
			{
				return IRenderer.SDL_RenderGeometryRaw(Pointer, texturePtr, xy.RawPointer, xyStride, colors.RawPointer, colorStride, uv.RawPointer, uvStride, verticesCount, indicesPtr, indices.Length, Unsafe.SizeOf<TIndex>());
			}
		}
	}

	/// <inheritdoc cref="IRenderer.TryRenderGeometryRaw(ReadOnlyNativeMemory{float}, int, ReadOnlyNativeMemory{Color{float}}, int, ReadOnlyNativeMemory{float}, int, ReadOnlySpan{int}, ITexture?)" />
	public bool TryRenderGeometryRaw(ReadOnlyNativeMemory<float> xy, int xyStride, ReadOnlyNativeMemory<Color<float>> colors, int colorStride, ReadOnlyNativeMemory<float> uv, int uvStride, ReadOnlySpan<int> indices, Texture<TDriver>? texture = default)
		=> TryRenderGeometryRawImpl(xy, xyStride, colors, colorStride, uv, uvStride, indices, texture);

	/// <inheritdoc/>
	bool IRenderer.TryRenderGeometryRaw(ReadOnlyNativeMemory<float> xy, int xyStride, ReadOnlyNativeMemory<Color<float>> colors, int colorStride, ReadOnlyNativeMemory<float> uv, int uvStride, ReadOnlySpan<int> indices, ITexture? texture)
		=> TryRenderGeometryRawImpl(xy, xyStride, colors, colorStride, uv, uvStride, indices, texture);

	/// <inheritdoc cref="IRenderer.TryRenderGeometryRaw(ReadOnlyNativeMemory{float}, int, ReadOnlyNativeMemory{Color{float}}, int, ReadOnlyNativeMemory{float}, int, ReadOnlySpan{short}, ITexture?)" />
	public bool TryRenderGeometryRaw(ReadOnlyNativeMemory<float> xy, int xyStride, ReadOnlyNativeMemory<Color<float>> colors, int colorStride, ReadOnlyNativeMemory<float> uv, int uvStride, ReadOnlySpan<short> indices, Texture<TDriver>? texture = default)
		=> TryRenderGeometryRawImpl(xy, xyStride, colors, colorStride, uv, uvStride, indices, texture);

	/// <inheritdoc/>
	bool IRenderer.TryRenderGeometryRaw(ReadOnlyNativeMemory<float> xy, int xyStride, ReadOnlyNativeMemory<Color<float>> colors, int colorStride, ReadOnlyNativeMemory<float> uv, int uvStride, ReadOnlySpan<short> indices, ITexture? texture)
		=> TryRenderGeometryRawImpl(xy, xyStride, colors, colorStride, uv, uvStride, indices, texture);

	/// <inheritdoc cref="IRenderer.TryRenderGeometryRaw(ReadOnlyNativeMemory{float}, int, ReadOnlyNativeMemory{Color{float}}, int, ReadOnlyNativeMemory{float}, int, ReadOnlySpan{sbyte}, ITexture?)" />
	public bool TryRenderGeometryRaw(ReadOnlyNativeMemory<float> xy, int xyStride, ReadOnlyNativeMemory<Color<float>> colors, int colorStride, ReadOnlyNativeMemory<float> uv, int uvStride, ReadOnlySpan<sbyte> indices, Texture<TDriver>? texture = default)
		=> TryRenderGeometryRawImpl(xy, xyStride, colors, colorStride, uv, uvStride, indices, texture);

	/// <inheritdoc/>
	bool IRenderer.TryRenderGeometryRaw(ReadOnlyNativeMemory<float> xy, int xyStride, ReadOnlyNativeMemory<Color<float>> colors, int colorStride, ReadOnlyNativeMemory<float> uv, int uvStride, ReadOnlySpan<sbyte> indices, ITexture? texture)
		=> TryRenderGeometryRawImpl(xy, xyStride, colors, colorStride, uv, uvStride, indices, texture);

	private bool TryRenderGeometryRawImpl<TTexture>(ReadOnlyNativeMemory<float> xy, int xyStride, ReadOnlyNativeMemory<Color<float>> colors, int colorStride, ReadOnlyNativeMemory<float> uv, int uvStride, TTexture? texture)
		where TTexture : notnull, ITexture
	{
		unsafe
		{
			if (!xy.IsValid || !colors.IsValid || !uv.IsValid)
			{
				return false;
			}

			if (!TryGetTexturePointer(texture, out var texturePtr))
			{
				// if we were given a non-null texture that has already been disposed, we fail early here
				return false;
			}

			var verticesCount = GetVerticesCount(xy, xyStride, colors, colorStride, uv, uvStride);

			return IRenderer.SDL_RenderGeometryRaw(Pointer, texturePtr, xy.RawPointer, xyStride, colors.RawPointer, colorStride, uv.RawPointer, uvStride, verticesCount, indices: null, num_indices: 0, size_indices: 0);
		}
	}

	/// <inheritdoc cref="IRenderer.TryRenderGeometryRaw(ReadOnlyNativeMemory{float}, int, ReadOnlyNativeMemory{Color{float}}, int, ReadOnlyNativeMemory{float}, int, ITexture?)"/>
	public bool TryRenderGeometryRaw(ReadOnlyNativeMemory<float> xy, int xyStride, ReadOnlyNativeMemory<Color<float>> colors, int colorStride, ReadOnlyNativeMemory<float> uv, int uvStride, Texture<TDriver>? texture = default)
		=> TryRenderGeometryRawImpl(xy, xyStride, colors, colorStride, uv, uvStride, texture);

	/// <inheritdoc/>
	bool IRenderer.TryRenderGeometryRaw(ReadOnlyNativeMemory<float> xy, int xyStride, ReadOnlyNativeMemory<Color<float>> colors, int colorStride, ReadOnlyNativeMemory<float> uv, int uvStride, ITexture? texture)
		=> TryRenderGeometryRawImpl(xy, xyStride, colors, colorStride, uv, uvStride, texture);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private static int GetVerticesCount(ReadOnlySpan<float> xy, int xyStride, ReadOnlySpan<Color<float>> colors, int colorStride, ReadOnlySpan<float> uv, int uvStride)
	{
		// calculate the number of vertices based on the lengths and strides of the inputs and selecting the minimum among them
		return int.Min(
			xyStride is > 0 ? unchecked(MemoryMarshal.AsBytes(xy).Length / xyStride) : 0,
			int.Min(
				colorStride is > 0 ? unchecked(MemoryMarshal.AsBytes(colors).Length / colorStride) : 0,
				int.Min(
					uvStride is > 0 ? unchecked(MemoryMarshal.AsBytes(uv).Length / uvStride) : 0,
					int.MaxValue
				)
			)
		);
	}

	private bool TryRenderGeometryRawImpl<TTexture, TIndex>(ReadOnlySpan<float> xy, int xyStride, ReadOnlySpan<Color<float>> colors, int colorStride, ReadOnlySpan<float> uv, int uvStride, ReadOnlySpan<TIndex> indices, TTexture? texture)
		where TTexture : notnull, ITexture
		where TIndex : unmanaged
	{
		unsafe
		{
			if (!TryGetTexturePointer(texture, out var texturePtr))
			{
				// if we were given a non-null texture that has already been disposed, we fail early here
				return false;
			}

			var verticesCount = GetVerticesCount(xy, xyStride, colors, colorStride, uv, uvStride);

			fixed (float* xyPtr = xy)
			fixed (Color<float>* colorsPtr = colors)
			fixed (float* uvPtr = uv)
			fixed (TIndex* indicesPtr = indices)
			{
				return IRenderer.SDL_RenderGeometryRaw(Pointer, texturePtr, xyPtr, xyStride, colorsPtr, colorStride, uvPtr, uvStride, verticesCount, indicesPtr, indices.Length, Unsafe.SizeOf<TIndex>());
			}
		}
	}

	/// <inheritdoc cref="IRenderer.TryRenderGeometryRaw(ReadOnlySpan{float}, int, ReadOnlySpan{Color{float}}, int, ReadOnlySpan{float}, int, ReadOnlySpan{int}, ITexture?)" />
	public bool TryRenderGeometryRaw(ReadOnlySpan<float> xy, int xyStride, ReadOnlySpan<Color<float>> colors, int colorStride, ReadOnlySpan<float> uv, int uvStride, ReadOnlySpan<int> indices, Texture<TDriver>? texture = default)
		=> TryRenderGeometryRawImpl(xy, xyStride, colors, colorStride, uv, uvStride, indices, texture);

	/// <inheritdoc/>
	bool IRenderer.TryRenderGeometryRaw(ReadOnlySpan<float> xy, int xyStride, ReadOnlySpan<Color<float>> colors, int colorStride, ReadOnlySpan<float> uv, int uvStride, ReadOnlySpan<int> indices, ITexture? texture)
		=> TryRenderGeometryRawImpl(xy, xyStride, colors, colorStride, uv, uvStride, indices, texture);

	/// <inheritdoc cref="IRenderer.TryRenderGeometryRaw(ReadOnlySpan{float}, int, ReadOnlySpan{Color{float}}, int, ReadOnlySpan{float}, int, ReadOnlySpan{short}, ITexture?)" />
	public bool TryRenderGeometryRaw(ReadOnlySpan<float> xy, int xyStride, ReadOnlySpan<Color<float>> colors, int colorStride, ReadOnlySpan<float> uv, int uvStride, ReadOnlySpan<short> indices, Texture<TDriver>? texture = default)
		=> TryRenderGeometryRawImpl(xy, xyStride, colors, colorStride, uv, uvStride, indices, texture);

	/// <inheritdoc/>
	bool IRenderer.TryRenderGeometryRaw(ReadOnlySpan<float> xy, int xyStride, ReadOnlySpan<Color<float>> colors, int colorStride, ReadOnlySpan<float> uv, int uvStride, ReadOnlySpan<short> indices, ITexture? texture)
		=> TryRenderGeometryRawImpl(xy, xyStride, colors, colorStride, uv, uvStride, indices, texture);

	/// <inheritdoc cref="IRenderer.TryRenderGeometryRaw(ReadOnlySpan{float}, int, ReadOnlySpan{Color{float}}, int, ReadOnlySpan{float}, int, ReadOnlySpan{sbyte}, ITexture?)" />
	public bool TryRenderGeometryRaw(ReadOnlySpan<float> xy, int xyStride, ReadOnlySpan<Color<float>> colors, int colorStride, ReadOnlySpan<float> uv, int uvStride, ReadOnlySpan<sbyte> indices, Texture<TDriver>? texture = default)
		=> TryRenderGeometryRawImpl(xy, xyStride, colors, colorStride, uv, uvStride, indices, texture);

	/// <inheritdoc/>
	bool IRenderer.TryRenderGeometryRaw(ReadOnlySpan<float> xy, int xyStride, ReadOnlySpan<Color<float>> colors, int colorStride, ReadOnlySpan<float> uv, int uvStride, ReadOnlySpan<sbyte> indices, ITexture? texture)
		=> TryRenderGeometryRawImpl(xy, xyStride, colors, colorStride, uv, uvStride, indices, texture);

	private bool TryRenderGeometryRawImpl<TTexture>(ReadOnlySpan<float> xy, int xyStride, ReadOnlySpan<Color<float>> colors, int colorStride, ReadOnlySpan<float> uv, int uvStride, TTexture? texture)
		where TTexture : notnull, ITexture
	{
		unsafe
		{
			if (!TryGetTexturePointer(texture, out var texturePtr))
			{
				// if we were given a non-null texture that has already been disposed, we fail early here
				return false;
			}

			var verticesCount = GetVerticesCount(xy, xyStride, colors, colorStride, uv, uvStride);

			fixed (float* xyPtr = xy)
			fixed (Color<float>* colorsPtr = colors)
			fixed (float* uvPtr = uv)
			{
				return IRenderer.SDL_RenderGeometryRaw(Pointer, texturePtr, xyPtr, xyStride, colorsPtr, colorStride, uvPtr, uvStride, verticesCount, indices: null, num_indices: 0, size_indices: 0);
			}
		}
	}

	/// <inheritdoc cref="IRenderer.TryRenderGeometryRaw(ReadOnlySpan{float}, int, ReadOnlySpan{Color{float}}, int, ReadOnlySpan{float}, int, ITexture?)"/>
	public bool TryRenderGeometryRaw(ReadOnlySpan<float> xy, int xyStride, ReadOnlySpan<Color<float>> colors, int colorStride, ReadOnlySpan<float> uv, int uvStride, Texture<TDriver>? texture = default)
		=> TryRenderGeometryRawImpl(xy, xyStride, colors, colorStride, uv, uvStride, texture);

	/// <inheritdoc/>
	bool IRenderer.TryRenderGeometryRaw(ReadOnlySpan<float> xy, int xyStride, ReadOnlySpan<Color<float>> colors, int colorStride, ReadOnlySpan<float> uv, int uvStride, ITexture? texture)
		=> TryRenderGeometryRawImpl(xy, xyStride, colors, colorStride, uv, uvStride, texture);

	private unsafe bool TryRenderGeometryRawImpl<TTexture, TIndex>(float* xy, int xyStride, Color<float>* colors, int colorStride, float* uv, int uvStride, int verticesCount, TIndex* indices, int indicesCount, TTexture? texture)
		where TTexture : notnull, ITexture
		where TIndex : unmanaged
	{
		if (!TryGetTexturePointer(texture, out var texturePtr))
		{
			// if we were given a non-null texture that has already been disposed, we fail early here
			return false;
		}

		return IRenderer.SDL_RenderGeometryRaw(Pointer, texturePtr, xy, xyStride, colors, colorStride, uv, uvStride, verticesCount, indices, indicesCount, Unsafe.SizeOf<TIndex>());
	}

	/// <inheritdoc cref="IRenderer.TryRenderGeometryRaw(float*, int, Color{float}*, int, float*, int, int, int*, int, ITexture?)" />
	public unsafe bool TryRenderGeometryRaw(float* xy, int xyStride, Color<float>* colors, int colorStride, float* uv, int uvStride, int verticesCount, int* indices, int indicesCount, Texture<TDriver>? texture = default)
		=> TryRenderGeometryRawImpl(xy, xyStride, colors, colorStride, uv, uvStride, verticesCount, indices, indicesCount, texture);

	/// <inheritdoc/>
	unsafe bool IRenderer.TryRenderGeometryRaw(float* xy, int xyStride, Color<float>* colors, int colorStride, float* uv, int uvStride, int verticesCount, int* indices, int indicesCount, ITexture? texture)
		=> TryRenderGeometryRawImpl(xy, xyStride, colors, colorStride, uv, uvStride, verticesCount, indices, indicesCount, texture);

	/// <inheritdoc cref="IRenderer.TryRenderGeometryRaw(float*, int, Color{float}*, int, float*, int, int, short*, int, ITexture?)" />
	public unsafe bool TryRenderGeometryRaw(float* xy, int xyStride, Color<float>* colors, int colorStride, float* uv, int uvStride, int verticesCount, short* indices, int indicesCount, Texture<TDriver>? texture = default)
		=> TryRenderGeometryRawImpl(xy, xyStride, colors, colorStride, uv, uvStride, verticesCount, indices, indicesCount, texture);

	/// <inheritdoc/>
	unsafe bool IRenderer.TryRenderGeometryRaw(float* xy, int xyStride, Color<float>* colors, int colorStride, float* uv, int uvStride, int verticesCount, short* indices, int indicesCount, ITexture? texture)
		=> TryRenderGeometryRawImpl(xy, xyStride, colors, colorStride, uv, uvStride, verticesCount, indices, indicesCount, texture);

	/// <inheritdoc cref="IRenderer.TryRenderGeometryRaw(float*, int, Color{float}*, int, float*, int, int, sbyte*, int, ITexture?)" />
	public unsafe bool TryRenderGeometryRaw(float* xy, int xyStride, Color<float>* colors, int colorStride, float* uv, int uvStride, int verticesCount, sbyte* indices, int indicesCount, Texture<TDriver>? texture = default)
		=> TryRenderGeometryRawImpl(xy, xyStride, colors, colorStride, uv, uvStride, verticesCount, indices, indicesCount, texture);

	/// <inheritdoc/>
	unsafe bool IRenderer.TryRenderGeometryRaw(float* xy, int xyStride, Color<float>* colors, int colorStride, float* uv, int uvStride, int verticesCount, sbyte* indices, int indicesCount, ITexture? texture)
		=> TryRenderGeometryRawImpl(xy, xyStride, colors, colorStride, uv, uvStride, verticesCount, indices, indicesCount, texture);

	private unsafe bool TryRenderGeometryRawImpl<TTexture>(float* xy, int xyStride, Color<float>* colors, int colorStride, float* uv, int uvStride, int verticesCount, TTexture? texture)
		where TTexture : notnull, ITexture
	{
		if (!TryGetTexturePointer(texture, out var texturePtr))
		{
			// if we were given a non-null texture that has already been disposed, we fail early here
			return false;
		}

		return IRenderer.SDL_RenderGeometryRaw(Pointer, texturePtr, xy, xyStride, colors, colorStride, uv, uvStride, verticesCount, indices: null, num_indices: 0, size_indices: 0);
	}

	/// <inheritdoc cref="IRenderer.TryRenderGeometryRaw(float*, int, Color{float}*, int, float*, int, int, ITexture?)"/>
	public unsafe bool TryRenderGeometryRaw(float* xy, int xyStride, Color<float>* colors, int colorStride, float* uv, int uvStride, int verticesCount, Texture<TDriver>? texture = default)
		=> TryRenderGeometryRawImpl(xy, xyStride, colors, colorStride, uv, uvStride, verticesCount, texture);

	/// <inheritdoc/>
	unsafe bool IRenderer.TryRenderGeometryRaw(float* xy, int xyStride, Color<float>* colors, int colorStride, float* uv, int uvStride, int verticesCount, ITexture? texture)
		=> TryRenderGeometryRawImpl(xy, xyStride, colors, colorStride, uv, uvStride, verticesCount, texture);

	/// <inheritdoc/>
	public bool TryRenderLine(float x1, float y1, float x2, float y2)
	{
		unsafe
		{
			return IRenderer.SDL_RenderLine(mRenderer, x1, y1, x2, y2);
		}
	}

	/// <inheritdoc/>
	public bool TryRenderLines(ReadOnlySpan<Point<float>> points)
	{
		unsafe
		{
			fixed (Point<float>* pointsPtr = points)
			{
				return IRenderer.SDL_RenderLines(mRenderer, pointsPtr, points.Length);
			}
		}
	}

	/// <inheritdoc/>
	public bool TryRenderPoint(float x, float y)
	{
		unsafe
		{
			return IRenderer.SDL_RenderPoint(mRenderer, x, y);
		}
	}

	/// <inheritdoc/>
	public bool TryRenderPoints(ReadOnlySpan<Point<float>> points)
	{
		unsafe
		{
			fixed (Point<float>* pointsPtr = points)
			{
				return IRenderer.SDL_RenderPoints(mRenderer, pointsPtr, points.Length);
			}
		}
	}

	/// <inheritdoc/>
	public bool TryRenderPresent()
	{
		unsafe
		{
			return IRenderer.SDL_RenderPresent(mRenderer);
		}
	}

	/// <inheritdoc/>
	public bool TryRenderRect(in Rect<float> rect)
	{
		unsafe
		{
			fixed (Rect<float>* rectPtr = &rect)
			{
				return IRenderer.SDL_RenderRect(mRenderer, rectPtr);
			}
		}
	}

	/// <inheritdoc/>
	public bool TryRenderRect()
	{
		unsafe
		{
			return IRenderer.SDL_RenderRect(mRenderer, rect: null);
		}
	}

	/// <inheritdoc/>
	public bool TryRenderRects(ReadOnlySpan<Rect<float>> rects)
	{
		unsafe
		{
			fixed (Rect<float>* rectsPtr = rects)
			{
				return IRenderer.SDL_RenderRects(mRenderer, rectsPtr, rects.Length);
			}
		}
	}

	private bool TryRenderTextureImpl<TTexture>(in Rect<float> destinationRect, TTexture texture, in Rect<float> sourceRect)
		where TTexture : notnull, ITexture
	{
		unsafe
		{
			fixed (Rect<float>* dstrect = &destinationRect, srcrect = &sourceRect)
			{
				return IRenderer.SDL_RenderTexture(Pointer, texture is not null ? texture.Pointer : null, srcrect, dstrect);
			}
		}
	}

	/// <inheritdoc cref="IRenderer.TryRenderTexture(in Rect{float}, ITexture, in Rect{float})"/>
	public bool TryRenderTexture(in Rect<float> destinationRect, Texture<TDriver> texture, in Rect<float> sourceRect)
		=> TryRenderTextureImpl(in destinationRect, texture, in sourceRect);

	/// <inheritdoc/>
	bool IRenderer.TryRenderTexture(in Rect<float> destinationRect, ITexture texture, in Rect<float> sourceRect)
		=> TryRenderTextureImpl(in destinationRect, texture, in sourceRect);

	private bool TryRenderTextureImpl<TTexture>(in Rect<float> destinationRect, TTexture texture)
		where TTexture : notnull, ITexture
	{
		unsafe
		{
			fixed (Rect<float>* dstrect = &destinationRect)
			{
				return IRenderer.SDL_RenderTexture(Pointer, texture is not null ? texture.Pointer : null, srcrect: null, dstrect);
			}
		}
	}

	/// <inheritdoc cref="IRenderer.TryRenderTexture(in Rect{float}, ITexture)"/>
	public bool TryRenderTexture(in Rect<float> destinationRect, Texture<TDriver> texture)
		=> TryRenderTextureImpl(in destinationRect, texture);

	/// <inheritdoc/>
	bool IRenderer.TryRenderTexture(in Rect<float> destinationRect, ITexture texture)
		=> TryRenderTextureImpl(in destinationRect, texture);

	private bool TryRenderTextureImpl<TTexture>(TTexture texture, in Rect<float> sourceRect)
		where TTexture : notnull, ITexture
	{
		unsafe
		{
			fixed (Rect<float>* srcrect = &sourceRect)
			{
				return IRenderer.SDL_RenderTexture(Pointer, texture is not null ? texture.Pointer : null, srcrect, dstrect: null);
			}
		}
	}

	/// <inheritdoc cref="IRenderer.TryRenderTexture(ITexture, in Rect{float})"/>
	public bool TryRenderTexture(Texture<TDriver> texture, in Rect<float> sourceRect)
		=> TryRenderTextureImpl(texture, in sourceRect);

	/// <inheritdoc/>
	bool IRenderer.TryRenderTexture(ITexture texture, in Rect<float> sourceRect)
		=> TryRenderTextureImpl(texture, in sourceRect);

	private bool TryRenderTextureImpl<TTexture>(TTexture texture)
		where TTexture : notnull, ITexture
	{
		unsafe
		{
			return IRenderer.SDL_RenderTexture(Pointer, texture is not null ? texture.Pointer : null, srcrect: null, dstrect: null);
		}
	}

	/// <inheritdoc cref="IRenderer.TryRenderTexture(ITexture)"/>
	public bool TryRenderTexture(Texture<TDriver> texture)
		=> TryRenderTextureImpl(texture);

	/// <inheritdoc/>
	bool IRenderer.TryRenderTexture(ITexture texture)
		=> TryRenderTextureImpl(texture);

	private bool TryRenderTexture9GridImpl<TTexture>(in Rect<float> destinationRect, TTexture texture, in Rect<float> sourceRect, float leftWidth, float rightWidth, float topHeight, float bottomHeight, float scale)
		where TTexture : notnull, ITexture
	{
		unsafe
		{
			fixed (Rect<float>* dstrect = &destinationRect, srcrect = &sourceRect)
			{
				return IRenderer.SDL_RenderTexture9Grid(Pointer, texture is not null ? texture.Pointer : null, srcrect, leftWidth, rightWidth, topHeight, bottomHeight, scale, dstrect);
			}
		}
	}

	/// <inheritdoc cref="IRenderer.TryRenderTexture9Grid(in Rect{float}, ITexture, in Rect{float}, float, float, float, float, float)"/>
	public bool TryRenderTexture9Grid(in Rect<float> destinationRect, Texture<TDriver> texture, in Rect<float> sourceRect, float leftWidth, float rightWidth, float topHeight, float bottomHeight, float scale)
		=> TryRenderTexture9GridImpl(in destinationRect, texture, in sourceRect, leftWidth, rightWidth, topHeight, bottomHeight, scale);

	/// <inheritdoc/>
	bool IRenderer.TryRenderTexture9Grid(in Rect<float> destinationRect, ITexture texture, in Rect<float> sourceRect, float leftWidth, float rightWidth, float topHeight, float bottomHeight, float scale)
		=> TryRenderTexture9GridImpl(in destinationRect, texture, in sourceRect, leftWidth, rightWidth, topHeight, bottomHeight, scale);

	private bool TryRenderTexture9GridImpl<TTexture>(in Rect<float> destinationRect, TTexture texture, float leftWidth, float rightWidth, float topHeight, float bottomHeight, float scale)
		where TTexture : notnull, ITexture
	{
		unsafe
		{
			fixed (Rect<float>* dstrect = &destinationRect)
			{
				return IRenderer.SDL_RenderTexture9Grid(Pointer, texture is not null ? texture.Pointer : null, srcrect: null, leftWidth, rightWidth, topHeight, bottomHeight, scale, dstrect);
			}
		}
	}

	/// <inheritdoc cref="IRenderer.TryRenderTexture9Grid(in Rect{float}, ITexture, float, float, float, float, float)"/>
	public bool TryRenderTexture9Grid(in Rect<float> destinationRect, Texture<TDriver> texture, float leftWidth, float rightWidth, float topHeight, float bottomHeight, float scale)
		=> TryRenderTexture9GridImpl(in destinationRect, texture, leftWidth, rightWidth, topHeight, bottomHeight, scale);

	/// <inheritdoc/>
	bool IRenderer.TryRenderTexture9Grid(in Rect<float> destinationRect, ITexture texture, float leftWidth, float rightWidth, float topHeight, float bottomHeight, float scale)
		=> TryRenderTexture9GridImpl(in destinationRect, texture, leftWidth, rightWidth, topHeight, bottomHeight, scale);

	private bool TryRenderTexture9GridImpl<TTexture>(TTexture texture, in Rect<float> sourceRect, float leftWidth, float rightWidth, float topHeight, float bottomHeight, float scale)
		where TTexture : notnull, ITexture
	{
		unsafe
		{
			fixed (Rect<float>* srcrect = &sourceRect)
			{
				return IRenderer.SDL_RenderTexture9Grid(Pointer, texture is not null ? texture.Pointer : null, srcrect, leftWidth, rightWidth, topHeight, bottomHeight, scale, dstrect: null);
			}
		}
	}

	/// <inheritdoc cref="IRenderer.TryRenderTexture9Grid(ITexture, in Rect{float}, float, float, float, float, float)"/>
	public bool TryRenderTexture9Grid(Texture<TDriver> texture, in Rect<float> sourceRect, float leftWidth, float rightWidth, float topHeight, float bottomHeight, float scale)
		=> TryRenderTexture9GridImpl(texture, in sourceRect, leftWidth, rightWidth, topHeight, bottomHeight, scale);

	/// <inheritdoc/>
	bool IRenderer.TryRenderTexture9Grid(ITexture texture, in Rect<float> sourceRect, float leftWidth, float rightWidth, float topHeight, float bottomHeight, float scale)
		=> TryRenderTexture9GridImpl(texture, in sourceRect, leftWidth, rightWidth, topHeight, bottomHeight, scale);

	private bool TryRenderTexture9GridImpl<TTexture>(TTexture texture, float leftWidth, float rightWidth, float topHeight, float bottomHeight, float scale)
		where TTexture : notnull, ITexture
	{
		unsafe
		{
			return IRenderer.SDL_RenderTexture9Grid(Pointer, texture is not null ? texture.Pointer : null, srcrect: null, leftWidth, rightWidth, topHeight, bottomHeight, scale, dstrect: null);
		}
	}

	/// <inheritdoc cref="IRenderer.TryRenderTexture9Grid(ITexture, float, float, float, float, float)"/>
	public bool TryRenderTexture9Grid(Texture<TDriver> texture, float leftWidth, float rightWidth, float topHeight, float bottomHeight, float scale)
		=> TryRenderTexture9GridImpl(texture, leftWidth, rightWidth, topHeight, bottomHeight, scale);

	/// <inheritdoc/>
	bool IRenderer.TryRenderTexture9Grid(ITexture texture, float leftWidth, float rightWidth, float topHeight, float bottomHeight, float scale)
		=> TryRenderTexture9GridImpl(texture, leftWidth, rightWidth, topHeight, bottomHeight, scale);

#if SDL3_4_0_OR_GREATER

	private bool TryRenderTexture9GridTiledImpl<TTexture>(in Rect<float> destinationRect, TTexture texture, in Rect<float> sourceRect, float leftWidth, float rightWidth, float topHeight, float bottomHeight, float scale, float tileScale)
		where TTexture : notnull, ITexture
	{
		unsafe
		{
			fixed (Rect<float>* dstrect = &destinationRect, srcrect = &sourceRect)
			{
				return IRenderer.SDL_RenderTexture9GridTiled(Pointer, texture is not null ? texture.Pointer : null, srcrect, leftWidth, rightWidth, topHeight, bottomHeight, scale, dstrect, tileScale);
			}
		}
	}

	/// <inheritdoc cref="IRenderer.TryRenderTexture9GridTiled(in Rect{float}, ITexture, in Rect{float}, float, float, float, float, float, float)"/>
	public bool TryRenderTexture9GridTiled(in Rect<float> destinationRect, Texture<TDriver> texture, in Rect<float> sourceRect, float leftWidth, float rightWidth, float topHeight, float bottomHeight, float scale, float tileScale)
		=> TryRenderTexture9GridTiledImpl(in destinationRect, texture, in sourceRect, leftWidth, rightWidth, topHeight, bottomHeight, scale, tileScale);

	/// <inheritdoc/>
	bool IRenderer.TryRenderTexture9GridTiled(in Rect<float> destinationRect, ITexture texture, in Rect<float> sourceRect, float leftWidth, float rightWidth, float topHeight, float bottomHeight, float scale, float tileScale)
		=> TryRenderTexture9GridTiledImpl(in destinationRect, texture, in sourceRect, leftWidth, rightWidth, topHeight, bottomHeight, scale, tileScale);

	private bool TryRenderTexture9GridTiledImpl<TTexture>(in Rect<float> destinationRect, TTexture texture, float leftWidth, float rightWidth, float topHeight, float bottomHeight, float scale, float tileScale)
		where TTexture : notnull, ITexture
	{
		unsafe
		{
			fixed (Rect<float>* dstrect = &destinationRect)
			{
				return IRenderer.SDL_RenderTexture9GridTiled(Pointer, texture is not null ? texture.Pointer : null, srcrect: null, leftWidth, rightWidth, topHeight, bottomHeight, scale, dstrect, tileScale);
			}
		}
	}

	/// <inheritdoc cref="IRenderer.TryRenderTexture9GridTiled(in Rect{float}, ITexture, float, float, float, float, float, float)"/>
	public bool TryRenderTexture9GridTiled(in Rect<float> destinationRect, Texture<TDriver> texture, float leftWidth, float rightWidth, float topHeight, float bottomHeight, float scale, float tileScale)
		=> TryRenderTexture9GridTiledImpl(in destinationRect, texture, leftWidth, rightWidth, topHeight, bottomHeight, scale, tileScale);

	/// <inheritdoc/>
	bool IRenderer.TryRenderTexture9GridTiled(in Rect<float> destinationRect, ITexture texture, float leftWidth, float rightWidth, float topHeight, float bottomHeight, float scale, float tileScale)
		=> TryRenderTexture9GridTiledImpl(in destinationRect, texture, leftWidth, rightWidth, topHeight, bottomHeight, scale, tileScale);

	private bool TryRenderTexture9GridTiledImpl<TTexture>(TTexture texture, in Rect<float> sourceRect, float leftWidth, float rightWidth, float topHeight, float bottomHeight, float scale, float tileScale)
		where TTexture : notnull, ITexture
	{
		unsafe
		{
			fixed (Rect<float>* srcrect = &sourceRect)
			{
				return IRenderer.SDL_RenderTexture9GridTiled(Pointer, texture is not null ? texture.Pointer : null, srcrect, leftWidth, rightWidth, topHeight, bottomHeight, scale, dstrect: null, tileScale);
			}
		}
	}

	/// <inheritdoc cref="IRenderer.TryRenderTexture9GridTiled(ITexture, in Rect{float}, float, float, float, float, float, float)"/>
	public bool TryRenderTexture9GridTiled(Texture<TDriver> texture, in Rect<float> sourceRect, float leftWidth, float rightWidth, float topHeight, float bottomHeight, float scale, float tileScale)
		=> TryRenderTexture9GridTiledImpl(texture, in sourceRect, leftWidth, rightWidth, topHeight, bottomHeight, scale, tileScale);

	/// <inheritdoc/>
	bool IRenderer.TryRenderTexture9GridTiled(ITexture texture, in Rect<float> sourceRect, float leftWidth, float rightWidth, float topHeight, float bottomHeight, float scale, float tileScale)
		=> TryRenderTexture9GridTiledImpl(texture, in sourceRect, leftWidth, rightWidth, topHeight, bottomHeight, scale, tileScale);

	private bool TryRenderTexture9GridTiledImpl<TTexture>(TTexture texture, float leftWidth, float rightWidth, float topHeight, float bottomHeight, float scale, float tileScale)
		where TTexture : notnull, ITexture
	{
		unsafe
		{
			return IRenderer.SDL_RenderTexture9GridTiled(Pointer, texture is not null ? texture.Pointer : null, srcrect: null, leftWidth, rightWidth, topHeight, bottomHeight, scale, dstrect: null, tileScale);
		}
	}

	/// <inheritdoc cref="IRenderer.TryRenderTexture9GridTiled(ITexture, float, float, float, float, float, float)"/>
	public bool TryRenderTexture9GridTiled(Texture<TDriver> texture, float leftWidth, float rightWidth, float topHeight, float bottomHeight, float scale, float tileScale)
		=> TryRenderTexture9GridTiledImpl(texture, leftWidth, rightWidth, topHeight, bottomHeight, scale, tileScale);

	/// <inheritdoc/>
	bool IRenderer.TryRenderTexture9GridTiled(ITexture texture, float leftWidth, float rightWidth, float topHeight, float bottomHeight, float scale, float tileScale)
		=> TryRenderTexture9GridTiledImpl(texture, leftWidth, rightWidth, topHeight, bottomHeight, scale, tileScale);

#endif

	private bool TryRenderTextureAffineImpl<TTexture>(in Point<float>? destinationOrigin, in Point<float>? destinationRight, in Point<float>? destinationDown, TTexture texture, in Rect<float> sourceRect)
		where TTexture : notnull, ITexture
	{
		unsafe
		{
			fixed (Point<float>* origin = &Nullable.GetValueRefOrDefaultRef(in destinationOrigin), right = &Nullable.GetValueRefOrDefaultRef(in destinationRight), down = &Nullable.GetValueRefOrDefaultRef(in destinationDown))
			fixed (Rect<float>* srcrect = &sourceRect)
			{
				return IRenderer.SDL_RenderTextureAffine(Pointer, texture is not null ? texture.Pointer : null, srcrect, destinationOrigin.HasValue ? origin : null, destinationRight.HasValue ? right : null, destinationDown.HasValue ? down : null);
			}
		}
	}

	/// <inheritdoc cref="IRenderer.TryRenderTextureAffine(in Point{float}?, in Point{float}?, in Point{float}?, ITexture, in Rect{float})"/>
	public bool TryRenderTextureAffine(in Point<float>? destinationOrigin, in Point<float>? destinationRight, in Point<float>? destinationDown, Texture<TDriver> texture, in Rect<float> sourceRect)
		=> TryRenderTextureAffineImpl(in destinationOrigin, in destinationRight, in destinationDown, texture, in sourceRect);

	/// <inheritdoc/>
	bool IRenderer.TryRenderTextureAffine(in Point<float>? destinationOrigin, in Point<float>? destinationRight, in Point<float>? destinationDown, ITexture texture, in Rect<float> sourceRect)
		=> TryRenderTextureAffineImpl(in destinationOrigin, in destinationRight, in destinationDown, texture, in sourceRect);

	private bool TryRenderTextureAffineImpl<TTexture>(in Point<float>? destinationOrigin, in Point<float>? destinationRight, in Point<float>? destinationDown, TTexture texture)
		where TTexture : notnull, ITexture
	{
		unsafe
		{
			fixed (Point<float>* origin = &Nullable.GetValueRefOrDefaultRef(in destinationOrigin), right = &Nullable.GetValueRefOrDefaultRef(in destinationRight), down = &Nullable.GetValueRefOrDefaultRef(in destinationDown))
			{
				return IRenderer.SDL_RenderTextureAffine(Pointer, texture is not null ? texture.Pointer : null, srcrect: null, destinationOrigin.HasValue ? origin : null, destinationRight.HasValue ? right : null, destinationDown.HasValue ? down : null);
			}
		}
	}

	/// <inheritdoc cref="IRenderer.TryRenderTextureAffine(in Point{float}?, in Point{float}?, in Point{float}?, ITexture)"/>
	public bool TryRenderTextureAffine(in Point<float>? destinationOrigin, in Point<float>? destinationRight, in Point<float>? destinationDown, Texture<TDriver> texture)
		=> TryRenderTextureAffineImpl(in destinationOrigin, in destinationRight, in destinationDown, texture);

	/// <inheritdoc/>
	bool IRenderer.TryRenderTextureAffine(in Point<float>? destinationOrigin, in Point<float>? destinationRight, in Point<float>? destinationDown, ITexture texture)
		=> TryRenderTextureAffineImpl(in destinationOrigin, in destinationRight, in destinationDown, texture);

	private bool TryRenderTextureRotatedImpl<TTexture>(in Rect<float> destinationRect, TTexture texture, in Rect<float> sourceRect, double angle, in Point<float>? centerPoint, FlipMode flip)
		where TTexture : notnull, ITexture
	{
		unsafe
		{
			fixed (Rect<float>* dstrect = &destinationRect, srcrect = &sourceRect)
			fixed (Point<float>* center = &Nullable.GetValueRefOrDefaultRef(in centerPoint))
			{
				return IRenderer.SDL_RenderTextureRotated(Pointer, texture is not null ? texture.Pointer : null, srcrect, dstrect, angle, centerPoint.HasValue ? center : null, flip);
			}
		}
	}

	/// <inheritdoc cref="IRenderer.TryRenderTextureRotated(in Rect{float}, ITexture, in Rect{float}, double, in Point{float}?, FlipMode)"/>
	public bool TryRenderTextureRotated(in Rect<float> destinationRect, Texture<TDriver> texture, in Rect<float> sourceRect, double angle, in Point<float>? centerPoint = null, FlipMode flip = FlipMode.None)
		=> TryRenderTextureRotatedImpl(in destinationRect, texture, in sourceRect, angle, in centerPoint, flip);

	/// <inheritdoc/>
	bool IRenderer.TryRenderTextureRotated(in Rect<float> destinationRect, ITexture texture, in Rect<float> sourceRect, double angle, in Point<float>? centerPoint, FlipMode flip)
		=> TryRenderTextureRotatedImpl(in destinationRect, texture, in sourceRect, angle, in centerPoint, flip);

	private bool TryRenderTextureRotatedImpl<TTexture>(in Rect<float> destinationRect, TTexture texture, double angle, in Point<float>? centerPoint, FlipMode flip)
		where TTexture : notnull, ITexture
	{
		unsafe
		{
			fixed (Rect<float>* dstrect = &destinationRect)
			fixed (Point<float>* center = &Nullable.GetValueRefOrDefaultRef(in centerPoint))
			{
				return IRenderer.SDL_RenderTextureRotated(Pointer, texture is not null ? texture.Pointer : null, srcrect: null, dstrect, angle, centerPoint.HasValue ? center : null, flip);
			}
		}
	}

	/// <inheritdoc cref="IRenderer.TryRenderTextureRotated(in Rect{float}, ITexture, double, in Point{float}?, FlipMode)"/>
	public bool TryRenderTextureRotated(in Rect<float> destinationRect, Texture<TDriver> texture, double angle, in Point<float>? centerPoint = null, FlipMode flip = FlipMode.None)
		=> TryRenderTextureRotatedImpl(in destinationRect, texture, angle, in centerPoint, flip);

	/// <inheritdoc/>
	bool IRenderer.TryRenderTextureRotated(in Rect<float> destinationRect, ITexture texture, double angle, in Point<float>? centerPoint, FlipMode flip)
		=> TryRenderTextureRotatedImpl(in destinationRect, texture, angle, in centerPoint, flip);

	private bool TryRenderTextureRotatedImpl<TTexture>(TTexture texture, in Rect<float> sourceRect, double angle, in Point<float>? centerPoint, FlipMode flip)
		where TTexture : notnull, ITexture
	{
		unsafe
		{
			fixed (Rect<float>* srcrect = &sourceRect)
			fixed (Point<float>* center = &Nullable.GetValueRefOrDefaultRef(in centerPoint))
			{
				return IRenderer.SDL_RenderTextureRotated(Pointer, texture is not null ? texture.Pointer : null, srcrect, dstrect: null, angle, centerPoint.HasValue ? center : null, flip);
			}
		}
	}

	/// <inheritdoc cref="IRenderer.TryRenderTextureRotated(ITexture, in Rect{float}, double, in Point{float}?, FlipMode)"/>
	public bool TryRenderTextureRotated(Texture<TDriver> texture, in Rect<float> sourceRect, double angle, in Point<float>? centerPoint = null, FlipMode flip = FlipMode.None)
		=> TryRenderTextureRotatedImpl(texture, in sourceRect, angle, in centerPoint, flip);

	/// <inheritdoc/>
	bool IRenderer.TryRenderTextureRotated(ITexture texture, in Rect<float> sourceRect, double angle, in Point<float>? centerPoint, FlipMode flip)
		=> TryRenderTextureRotatedImpl(texture, in sourceRect, angle, in centerPoint, flip);

	private bool TryRenderTextureRotatedImpl<TTexture>(TTexture texture, double angle, in Point<float>? centerPoint, FlipMode flip)
		where TTexture : notnull, ITexture
	{
		unsafe
		{
			fixed (Point<float>* center = &Nullable.GetValueRefOrDefaultRef(in centerPoint))
			{
				return IRenderer.SDL_RenderTextureRotated(Pointer, texture is not null ? texture.Pointer : null, srcrect: null, dstrect: null, angle, centerPoint.HasValue ? center : null, flip);
			}
		}
	}

	/// <inheritdoc cref="IRenderer.TryRenderTextureRotated(ITexture, double, in Point{float}?, FlipMode)"/>
	public bool TryRenderTextureRotated(Texture<TDriver> texture, double angle, in Point<float>? centerPoint = null, FlipMode flip = FlipMode.None)
		=> TryRenderTextureRotatedImpl(texture, angle, in centerPoint, flip);

	/// <inheritdoc/>
	bool IRenderer.TryRenderTextureRotated(ITexture texture, double angle, in Point<float>? centerPoint, FlipMode flip)
		=> TryRenderTextureRotatedImpl(texture, angle, in centerPoint, flip);

	private bool TryRenderTextureTiledImpl<TTexture>(in Rect<float> destinationRect, TTexture texture, in Rect<float> sourceRect, float scale)
		where TTexture : notnull, ITexture
	{
		unsafe
		{
			fixed (Rect<float>* dstrect = &destinationRect, srcrect = &sourceRect)
			{
				return IRenderer.SDL_RenderTextureTiled(Pointer, texture is not null ? texture.Pointer : null, srcrect, scale, dstrect);
			}
		}
	}

	/// <inheritdoc cref="IRenderer.TryRenderTextureTiled(in Rect{float}, ITexture, in Rect{float}, float)"/>
	public bool TryRenderTextureTiled(in Rect<float> destinationRect, Texture<TDriver> texture, in Rect<float> sourceRect, float scale)
		=> TryRenderTextureTiledImpl(in destinationRect, texture, in sourceRect, scale);

	/// <inheritdoc/>
	bool IRenderer.TryRenderTextureTiled(in Rect<float> destinationRect, ITexture texture, in Rect<float> sourceRect, float scale)
		=> TryRenderTextureTiledImpl(in destinationRect, texture, in sourceRect, scale);

	private bool TryRenderTextureTiledImpl<TTexture>(in Rect<float> destinationRect, TTexture texture, float scale)
		where TTexture : notnull, ITexture
	{
		unsafe
		{
			fixed (Rect<float>* dstrect = &destinationRect)
			{
				return IRenderer.SDL_RenderTextureTiled(Pointer, texture is not null ? texture.Pointer : null, srcrect: null, scale, dstrect);
			}
		}
	}

	/// <inheritdoc cref="IRenderer.TryRenderTextureTiled(in Rect{float}, ITexture, float)"/>
	public bool TryRenderTextureTiled(in Rect<float> destinationRect, Texture<TDriver> texture, float scale)
		=> TryRenderTextureTiledImpl(in destinationRect, texture, scale);

	/// <inheritdoc/>
	bool IRenderer.TryRenderTextureTiled(in Rect<float> destinationRect, ITexture texture, float scale)
		=> TryRenderTextureTiledImpl(in destinationRect, texture, scale);

	private bool TryRenderTextureTiledImpl<TTexture>(TTexture texture, in Rect<float> sourceRect, float scale)
		where TTexture : notnull, ITexture
	{
		unsafe
		{
			fixed (Rect<float>* srcrect = &sourceRect)
			{
				return IRenderer.SDL_RenderTextureTiled(Pointer, texture is not null ? texture.Pointer : null, srcrect, scale, dstrect: null);
			}
		}
	}

	/// <inheritdoc cref="IRenderer.TryRenderTextureTiled(ITexture, in Rect{float}, float)"/>
	public bool TryRenderTextureTiled(Texture<TDriver> texture, in Rect<float> sourceRect, float scale)
		=> TryRenderTextureTiledImpl(texture, in sourceRect, scale);

	/// <inheritdoc/>
	bool IRenderer.TryRenderTextureTiled(ITexture texture, in Rect<float> sourceRect, float scale)
		=> TryRenderTextureTiledImpl(texture, in sourceRect, scale);

	private bool TryRenderTextureTiledImpl<TTexture>(TTexture texture, float scale)
		where TTexture : notnull, ITexture
	{
		unsafe
		{
			return IRenderer.SDL_RenderTextureTiled(Pointer, texture is not null ? texture.Pointer : null, srcrect: null, scale, dstrect: null);
		}
	}

	/// <inheritdoc cref="IRenderer.TryRenderTextureTiled(ITexture, float)"/>
	public bool TryRenderTextureTiled(Texture<TDriver> texture, float scale)
		=> TryRenderTextureTiledImpl(texture, scale);

	/// <inheritdoc/>
	bool IRenderer.TryRenderTextureTiled(ITexture texture, float scale)
		=> TryRenderTextureTiledImpl(texture, scale);
}
