using Sdl3Sharp.Video.Coloring;
using Sdl3Sharp.Video.Rendering;
using Sdl3Sharp.Video.Rendering.Drivers;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Windowing;

public partial class Window
{
	internal unsafe SDL_Window* WindowPtr;

	/// <summary>
	/// Tries to create a new <see cref="IRenderer"/> for this window
	/// </summary>
	/// <param name="renderer">The resulting <see cref="IRenderer"/>, if the method returns <see langword="true"/>; otherwise, <see langword="null"/></param>
	/// <param name="driverNames">An optional list of driver names to try, in order of preference. An empty list (the default) lets SDL automatically choose the best available driver for you.</param>
	/// <returns><c><see langword="true"/></c>, if the renderer was created successfully; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// If you want to try specific rendering drivers, you can provide their names for the <paramref name="driverNames"/> parameter.
	/// You can use the <see cref="IDriver.Name"/> property of the pre-defined drivers for this (e.g. <see cref="OpenGl.Name"/>),
	/// or you can get the list of all available drivers at runtime using <see cref="IDriver.AvailableDriverNames"/> property.
	/// </para>
	/// <para>
	/// SDL will attempt to create a renderer using each of the specified driver names in order, and will return the first one that succeeds.
	/// </para>
	/// <para>
	/// Leaving the <paramref name="driverNames"/> parameter empty (the default) lets SDL automatically choose the best available driver for you,
	/// which is usually what you want unless you have specific requirements or want to test multiple drivers.
	/// </para>
	/// <para>
	/// The default renderering size of the resulting <paramref name="renderer"/> will match the size of the window in pixels,
	/// but you can change the content size and scaling later using the <see cref="IRenderer.LogicalPresentation"/> property if needed.
	/// </para>
	/// <para>
	/// The resulting <paramref name="renderer"/> will be of the <see cref="Renderer{TDriver}"/> type with a specific <see cref="IDriver">rendering driver</see> as the type argument.
	/// If you need driver-specific functionality, you can type check and cast the resulting <paramref name="renderer"/> to the appropriate <see cref="Renderer{TDriver}"/> type later
	/// or you can use the <see cref="TryCreateRenderer{TDriver}(out Renderer{TDriver}?)"/> method alternatively.
	/// </para>
	/// <para>
	/// This method should only be called from the main thread.
	/// </para>
	/// </remarks>
	public bool TryCreateRenderer([NotNullWhen(true)] out IRenderer? renderer, params ReadOnlySpan<string> driverNames)
	{
		unsafe
		{
			byte* joinedNames = null;
			try
			{
				if (driverNames.Length is > 0)
				{
					// driver names will be joined into a single, ','-separated, null-terminated ASCII string on the stack

					var totalLength = 0;
					foreach (var name in driverNames)
					{
						totalLength += name.Length;
					}

					joinedNames = unchecked((byte*)Utilities.NativeMemory.Malloc(unchecked((nuint)(
						totalLength                // total number of characters
						+ (driverNames.Length - 1) // number of ',' separators
						+ 1                        // null-terminator
					))));

					if (joinedNames is null)
					{
						renderer = null;
						return false;
					}

					var index = 0;
					while (true)
					{
						var name = driverNames[index];

						foreach (var ch in name)
						{
							if (ch is '\0')
							{
								break;
							}

							*joinedNames++ = ch is <= '\x7F'
								? unchecked((byte)ch)
								: (byte)'\xFF'; // replace non-ASCII characters with placeholder ('\xFF' is invalid in UTF-8 and therefore safe to use here)
						}

						if (!(++index < driverNames.Length))
						{
							break;
						}

						*joinedNames++ = (byte)',';
					}

					*joinedNames = (byte)'\0';
				}

				var rendererPtr = IRenderer.SDL_CreateRenderer(WindowPtr, name: joinedNames);

				if (rendererPtr is null)
				{
					renderer = null;
					return false;
				}

				return IRenderer.TryCreateFromRegisteredDriver(rendererPtr, register: true, out renderer);
			}
			finally
			{
				Utilities.NativeMemory.Free(joinedNames);
			}
		}
	}

	private unsafe bool TryCreateRenderer<TDriver>([NotNullWhen(true)] out Renderer<TDriver>? renderer, byte* driverName)
		where TDriver : notnull, IDriver
	{
		var rendererPtr = IRenderer.SDL_CreateRenderer(WindowPtr, name: driverName);

		if (rendererPtr is null)
		{
			renderer = null;
			return false;
		}

		renderer = new(rendererPtr, register: true);
		return true;
	}

	/// <summary>
	/// Tries to create a new <see cref="Renderer{TDriver}"/> for this window
	/// </summary>
	/// <typeparam name="TDriver">The rendering driver type associated with the resulting renderer</typeparam>
	/// <param name="renderer">The resulting <see cref="Renderer{TDriver}"/>, if the method returns <see langword="true"/>; otherwise, <see langword="null"/></param>
	/// <returns><c><see langword="true"/></c>, if the renderer was created successfully; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// The resulting <paramref name="renderer"/> will be of the <see cref="Renderer{TDriver}"/> type with the specified <typeparamref name="TDriver"/> as the type argument.
	/// </para>
	/// <para>
	/// This method should only be called from the main thread.
	/// </para>
	/// </remarks>
	public bool TryCreateRenderer<TDriver>([NotNullWhen(true)] out Renderer<TDriver>? renderer)
		where TDriver : notnull, IDriver
	{
		unsafe
		{
			if (!TDriver.NameAscii.IsEmpty)
			{
				fixed (byte* driverName = TDriver.NameAscii)
				{
					return TryCreateRenderer(out renderer, driverName);
				}
			}
			else
			{
				return TryCreateRenderer(out renderer, driverName: null);
			}
		}
	}

	/// <summary>
	/// Tries to create a new <see cref="IRenderer"/> for this window
	/// </summary>
	/// <param name="renderer">The resulting <see cref="IRenderer"/>, if the method returns <see langword="true"/>; otherwise, <see langword="null"/></param>
	/// <param name="driverName">The name of the rendering driver to use, or <see langword="null"/> to let SDL automatically choose the best available driver for you</param>
	/// <param name="outputColorSpace">
	/// The color space to be used by renderer for presenting to the output display.
	/// The <see cref="Direct3D11">Direct3D 11</see>, <see cref="Direct3D12">Direct3D 12</see>, and <see cref="Metal">Metal</see> renderers support <see cref="ColorSpace.SrgbLinear"/>,
	/// which is a linear color space and supports HDR output. In that case, drawing still uses the sRGB color space, but individual values can go beyond <c>1.0</c>
	/// and floating point textures can be used for HDR content.
	/// If this parameter is <see langword="null"/> (the default), the output color space defaults to <see cref="ColorSpace.Srgb"/>.
	/// </param>
	/// <param name="presentVSync">
	/// The vertical synchronization (VSync) mode or interval to be used by the renderer.
	/// Can be specified to be <see cref="RendererVSync.Disabled"/> to disable VSync,
	/// <see cref="RendererVSync.Adaptive"/> to enable late swap tearing (adaptive VSync) if supported,
	/// or the result of the <see cref="RendererVSyncExtensions.Interval(int)"/> method to specify a custom VSync interval.
	/// You can specify a custom interval of <c>1</c> to synchronize to present of the renderer with <em>every</em> vertical refresh,
	/// <c>2</c> to synchronize it with <em>every second</em> vertical refresh, and so on.
	/// If this parameter is <see langword="null"/> (the default), the VSync mode defaults to <see cref="RendererVSync.Disabled"/>.
	/// </param>
	/// <param name="properties">Additional properties to use when creating the renderer</param>
	/// <returns><c><see langword="true"/></c>, if the renderer was created successfully; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// The resulting <paramref name="renderer"/> will be of the <see cref="Renderer{TDriver}"/> type with a specific <see cref="IDriver">rendering driver</see> as the type argument.
	/// If you need driver-specific functionality, you can type check and cast the resulting <paramref name="renderer"/> to the appropriate <see cref="Renderer{TDriver}"/> type later
	/// or you can use the <see cref="TryCreateRenderer{TDriver}(out Renderer{TDriver}?)"/> method alternatively.
	/// </para>
	/// <para>
	/// This method should only be called from the main thread.
	/// </para>
	/// </remarks>
	public bool TryCreateRenderer([NotNullWhen(true)] out IRenderer? renderer, string? driverName = default, ColorSpace? outputColorSpace = default, RendererVSync? presentVSync = default, Properties? properties = default)
	{
		unsafe
		{
			Properties propertiesUsed;
			Unsafe.SkipInit(out string? driverNameBackup);
			Unsafe.SkipInit(out IntPtr? windowBackup);
			Unsafe.SkipInit(out ColorSpace? outputColorSpaceBackup);
			Unsafe.SkipInit(out RendererVSync? presentVSyncBackup);

			if (properties is null)
			{
				propertiesUsed = [];

				if (driverName is not null)
				{
					propertiesUsed.TrySetStringValue(IRenderer.PropertyNames.CreateNameString, driverName);
				}

				// setting SDL_PROP_RENDERER_CREATE_WINDOW_POINTER is required, except for when we want to create a software renderer,
				// and we'll handle software renderers separately via Renderer<Software>.TryCreateForSurface
				propertiesUsed.TrySetPointerValue(IRenderer.PropertyNames.CreateWindowPointer, unchecked((IntPtr)WindowPtr));

				if (outputColorSpace is ColorSpace outputColorSpaceValue)
				{
					propertiesUsed.TrySetNumberValue(IRenderer.PropertyNames.CreateOutputColorSpaceNumber, unchecked((uint)outputColorSpaceValue));
				}

				if (presentVSync is RendererVSync presentVSyncValue)
				{
					propertiesUsed.TrySetNumberValue(IRenderer.PropertyNames.CreatePresentVSyncNumber, unchecked((int)presentVSyncValue));
				}
			}
			else
			{
				propertiesUsed = properties;

				if (driverName is not null)
				{
					driverNameBackup = propertiesUsed.TryGetStringValue(IRenderer.PropertyNames.CreateNameString, out var existingDriverName)
						? existingDriverName
						: null;

					propertiesUsed.TrySetStringValue(IRenderer.PropertyNames.CreateNameString, driverName);
				}

				windowBackup = propertiesUsed.TryGetPointerValue(IRenderer.PropertyNames.CreateWindowPointer, out var existingWindowPtr)
					? existingWindowPtr
					: null;

				// setting SDL_PROP_RENDERER_CREATE_WINDOW_POINTER is required, except for when we want to create a software renderer,
				// and we'll handle software renderers separately via Renderer<Software>.TryCreateForSurface
				propertiesUsed.TrySetPointerValue(IRenderer.PropertyNames.CreateWindowPointer, unchecked((IntPtr)WindowPtr));

				if (outputColorSpace is ColorSpace outputColorSpaceValue)
				{
					outputColorSpaceBackup = propertiesUsed.TryGetNumberValue(IRenderer.PropertyNames.CreateOutputColorSpaceNumber, out var existingOutputColorSpaceValue)
						? unchecked((ColorSpace)existingOutputColorSpaceValue)
						: null;

					propertiesUsed.TrySetNumberValue(IRenderer.PropertyNames.CreateOutputColorSpaceNumber, unchecked((uint)outputColorSpaceValue));
				}

				if (presentVSync is RendererVSync presentVSyncValue)
				{
					presentVSyncBackup = propertiesUsed.TryGetNumberValue(IRenderer.PropertyNames.CreatePresentVSyncNumber, out var existingPresentVSyncValue)
						? unchecked((RendererVSync)existingPresentVSyncValue)
						: null;

					propertiesUsed.TrySetNumberValue(IRenderer.PropertyNames.CreatePresentVSyncNumber, unchecked((int)presentVSyncValue));
				}
			}

			try
			{
				var rendererPtr = IRenderer.SDL_CreateRendererWithProperties(propertiesUsed.Id);

				if (rendererPtr is null)
				{
					renderer = null;
					return false;
				}

				return IRenderer.TryCreateFromRegisteredDriver(rendererPtr, register: true, out renderer);
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

					if (driverName is not null)
					{
						if (driverNameBackup is not null)
						{
							propertiesUsed.TrySetStringValue(IRenderer.PropertyNames.CreateNameString, driverNameBackup);
						}
						else
						{
							propertiesUsed.TryRemove(IRenderer.PropertyNames.CreateNameString);
						}
					}

					if (windowBackup is IntPtr windowPtr)
					{
						propertiesUsed.TrySetPointerValue(IRenderer.PropertyNames.CreateWindowPointer, windowPtr);
					}
					else
					{
						propertiesUsed.TryRemove(IRenderer.PropertyNames.CreateWindowPointer);
					}

					if (outputColorSpace.HasValue)
					{
						if (outputColorSpaceBackup is ColorSpace outputColorSpaceValue)
						{
							propertiesUsed.TrySetNumberValue(IRenderer.PropertyNames.CreateOutputColorSpaceNumber, unchecked((uint)outputColorSpaceValue));
						}
						else
						{
							propertiesUsed.TryRemove(IRenderer.PropertyNames.CreateOutputColorSpaceNumber);
						}
					}

					if (presentVSync.HasValue)
					{
						if (presentVSyncBackup is RendererVSync presentVSyncValue)
						{
							propertiesUsed.TrySetNumberValue(IRenderer.PropertyNames.CreatePresentVSyncNumber, unchecked((int)presentVSyncValue));
						}
						else
						{
							propertiesUsed.TryRemove(IRenderer.PropertyNames.CreatePresentVSyncNumber);
						}
					}
				}
			}
		}
	}

	// I know, I know, this isn't quite DRY..., but think about it: trying to abstract this into a shared method would require us to use some sort of aspect oriented programming.
	// We could achieve this with zero-cost abstractions by using static abstract interface members,
	// but than we would need to declare the interface, do the implementations by defining individual types for each one, and introduce a new generic parameter for the accepting method (and maybe more).
	// Or we could just duplicate the code, which is way simpler in this case.

	private unsafe bool TryCreateRenderer<TDriver>([NotNullWhen(true)] out Renderer<TDriver>? renderer, byte* driverName, ColorSpace? outputColorSpace = default, RendererVSync? presentVSync = default, Properties? properties = default)
		where TDriver : notnull, IDriver
	{
		unsafe
		{
			Properties propertiesUsed;
			Unsafe.SkipInit(out string? driverNameBackup);
			Unsafe.SkipInit(out IntPtr? windowBackup);
			Unsafe.SkipInit(out ColorSpace? outputColorSpaceBackup);
			Unsafe.SkipInit(out RendererVSync? presentVSyncBackup);

			if (properties is null)
			{
				propertiesUsed = [];

				if (driverName is not null)
				{
					propertiesUsed.TrySetNativeStringValue(IRenderer.PropertyNames.CreateNameString, driverName);
				}

				// setting SDL_PROP_RENDERER_CREATE_WINDOW_POINTER is required, except for when we want to create a software renderer,
				// and we'll handle software renderers separately via Renderer<Software>.TryCreateForSurface
				propertiesUsed.TrySetPointerValue(IRenderer.PropertyNames.CreateWindowPointer, unchecked((IntPtr)WindowPtr));

				if (outputColorSpace is ColorSpace outputColorSpaceValue)
				{
					propertiesUsed.TrySetNumberValue(IRenderer.PropertyNames.CreateOutputColorSpaceNumber, unchecked((uint)outputColorSpaceValue));
				}

				if (presentVSync is RendererVSync presentVSyncValue)
				{
					propertiesUsed.TrySetNumberValue(IRenderer.PropertyNames.CreatePresentVSyncNumber, unchecked((int)presentVSyncValue));
				}
			}
			else
			{
				propertiesUsed = properties;

				driverNameBackup = propertiesUsed.TryGetStringValue(IRenderer.PropertyNames.CreateNameString, out var existingDriverName)
					? existingDriverName
					: null;

				// definitely overwrite existing driver name, even if null
				if (driverName is not null)
				{
					propertiesUsed.TrySetNativeStringValue(IRenderer.PropertyNames.CreateNameString, driverName);
				}
				else
				{
					propertiesUsed.TryRemove(IRenderer.PropertyNames.CreateNameString);
				}

				windowBackup = propertiesUsed.TryGetPointerValue(IRenderer.PropertyNames.CreateWindowPointer, out var existingWindowPtr)
					? existingWindowPtr
					: null;

				// setting SDL_PROP_RENDERER_CREATE_WINDOW_POINTER is required, except for when we want to create a software renderer,
				// and we'll handle software renderers separately via Renderer<Software>.TryCreateForSurface
				propertiesUsed.TrySetPointerValue(IRenderer.PropertyNames.CreateWindowPointer, unchecked((IntPtr)WindowPtr));

				if (outputColorSpace is ColorSpace outputColorSpaceValue)
				{
					outputColorSpaceBackup = propertiesUsed.TryGetNumberValue(IRenderer.PropertyNames.CreateOutputColorSpaceNumber, out var existingOutputColorSpaceValue)
						? unchecked((ColorSpace)existingOutputColorSpaceValue)
						: null;

					propertiesUsed.TrySetNumberValue(IRenderer.PropertyNames.CreateOutputColorSpaceNumber, unchecked((uint)outputColorSpaceValue));
				}

				if (presentVSync is RendererVSync presentVSyncValue)
				{
					presentVSyncBackup = propertiesUsed.TryGetNumberValue(IRenderer.PropertyNames.CreatePresentVSyncNumber, out var existingPresentVSyncValue)
						? unchecked((RendererVSync)existingPresentVSyncValue)
						: null;

					propertiesUsed.TrySetNumberValue(IRenderer.PropertyNames.CreatePresentVSyncNumber, unchecked((int)presentVSyncValue));
				}
			}

			try
			{
				var rendererPtr = IRenderer.SDL_CreateRendererWithProperties(propertiesUsed.Id);

				if (rendererPtr is null)
				{
					renderer = null;
					return false;
				}

				renderer = new(rendererPtr, register: true);
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

					if (driverNameBackup is not null)
					{
						propertiesUsed.TrySetStringValue(IRenderer.PropertyNames.CreateNameString, driverNameBackup);
					}
					else
					{
						propertiesUsed.TryRemove(IRenderer.PropertyNames.CreateNameString);
					}

					if (windowBackup is IntPtr windowPtr)
					{
						propertiesUsed.TrySetPointerValue(IRenderer.PropertyNames.CreateWindowPointer, windowPtr);
					}
					else
					{
						propertiesUsed.TryRemove(IRenderer.PropertyNames.CreateWindowPointer);
					}

					if (outputColorSpace.HasValue)
					{
						if (outputColorSpaceBackup is ColorSpace outputColorSpaceValue)
						{
							propertiesUsed.TrySetNumberValue(IRenderer.PropertyNames.CreateOutputColorSpaceNumber, unchecked((uint)outputColorSpaceValue));
						}
						else
						{
							propertiesUsed.TryRemove(IRenderer.PropertyNames.CreateOutputColorSpaceNumber);
						}
					}

					if (presentVSync.HasValue)
					{
						if (presentVSyncBackup is RendererVSync presentVSyncValue)
						{
							propertiesUsed.TrySetNumberValue(IRenderer.PropertyNames.CreatePresentVSyncNumber, unchecked((int)presentVSyncValue));
						}
						else
						{
							propertiesUsed.TryRemove(IRenderer.PropertyNames.CreatePresentVSyncNumber);
						}
					}
				}
			}
		}
	}

	/// <summary>
	/// Tries to create a new <see cref="Renderer{TDriver}"/> for this window
	/// </summary>
	/// <typeparam name="TDriver">The rendering driver type associated with the resulting renderer</typeparam>
	/// <param name="renderer">The resulting <see cref="Renderer{TDriver}"/>, if the method returns <see langword="true"/>; otherwise, <see langword="null"/></param>
	/// <param name="outputColorSpace">
	/// The color space to be used by renderer for presenting to the output display.
	/// The <see cref="Direct3D11">Direct3D 11</see>, <see cref="Direct3D12">Direct3D 12</see>, and <see cref="Metal">Metal</see> renderers support <see cref="ColorSpace.SrgbLinear"/>,
	/// which is a linear color space and supports HDR output. In that case, drawing still uses the sRGB color space, but individual values can go beyond <c>1.0</c>
	/// and floating point textures can be used for HDR content.
	/// If this parameter is <see langword="null"/> (the default), the output color space defaults to <see cref="ColorSpace.Srgb"/>.
	/// </param>
	/// <param name="presentVSync">
	/// The vertical synchronization (VSync) mode or interval to be used by the renderer.
	/// Can be specified to be <see cref="RendererVSync.Disabled"/> to disable VSync,
	/// <see cref="RendererVSync.Adaptive"/> to enable late swap tearing (adaptive VSync) if supported,
	/// or the result of the <see cref="RendererVSyncExtensions.Interval(int)"/> method to specify a custom VSync interval.
	/// You can specify a custom interval of <c>1</c> to synchronize to present of the renderer with <em>every</em> vertical refresh,
	/// <c>2</c> to synchronize it with <em>every second</em> vertical refresh, and so on.
	/// If this parameter is <see langword="null"/> (the default), the VSync mode defaults to <see cref="RendererVSync.Disabled"/>.
	/// </param>
	/// <param name="properties">Additional properties to use when creating the renderer</param>
	/// <returns><c><see langword="true"/></c>, if the renderer was created successfully; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// The resulting <paramref name="renderer"/> will be of the <see cref="Renderer{TDriver}"/> type with the specified <typeparamref name="TDriver"/> as the type argument.
	/// </para>
	/// <para>
	/// This method should only be called from the main thread.
	/// </para>
	/// </remarks>
	public bool TryCreateRenderer<TDriver>([NotNullWhen(true)] out Renderer<TDriver>? renderer, ColorSpace? outputColorSpace = default, RendererVSync? presentVSync = default, Properties? properties = default)
		where TDriver : notnull, IDriver
	{
		unsafe
		{
			if (!TDriver.NameAscii.IsEmpty)
			{
				fixed (byte* driverName = TDriver.NameAscii)
				{
					return TryCreateRenderer(out renderer, driverName, outputColorSpace, presentVSync, properties);
				}

			}
			else
			{
				return TryCreateRenderer(out renderer, driverName: null, outputColorSpace, presentVSync, properties);
			}
		}
	}

	// TODO: IMPLEMENT!
	internal unsafe static bool TryGetOrCreate(SDL_Window* window, [NotNullWhen(true)] out Window? result)
	{
		result = default;
		return false;
	}
}
