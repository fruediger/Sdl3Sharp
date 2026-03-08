using Sdl3Sharp.Internal;
using Sdl3Sharp.Video.Windowing.Drivers;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Video.Windowing;

partial class WindowExtensions
{
	extension(Window<Emscripten>.PropertyNames)
	{
		public static string CreateEmscriptenCanvasIdString => "SDL.window.create.emscripten.canvas_id";

		public static string CreateEmscriptenKeyboardElementString => "SDL.window.create.emscripten.keyboard_element";

		public static string EmscriptenCanvasIdString => "SDL.window.emscripten.canvas_id";

		public static string EmscriptenKeyboardElementString => "SDL.window.emscripten.keyboard_element";
	}

	extension(Window<Emscripten>)
	{
		public static bool TryCreate([NotNullWhen(true)] out Window<Cocoa>? window, bool? alwaysOnTop = default, bool? bordered = default, bool? constrainPopup = default, bool? externalGraphicsContext = default, bool? focusable = default,
			bool? fullscreen = default, int? height = default, bool? hidden = default, bool? highPixelDensity = default, bool? maximized = default, bool? menu = default, bool? metal = default, bool? minimized = default,
			bool? modal = default, bool? mouseGrabbed = default, bool? openGL = default, Window? parent = default, bool? resizable = default, string? title = default, bool? transparent = default, bool? tooltip = default,
			bool? utility = default, bool? vulkan = default, int? width = default, int? x = default, int? y = default,
			string? emscriptenCanvasId = default, string? emscriptenKeyboardElement = default, Properties? properties = default)
		{
			if (!Emscripten.IsActive)
			{
				window = null;
				return false;
			}

			Properties propertiesUsed;
			Unsafe.SkipInit(out string? emscriptenCanvasIdBackup);
			Unsafe.SkipInit(out string? emscriptenKeyboardElementBackup);

			if (properties is null)
			{
				propertiesUsed = [];

				if (emscriptenCanvasId is string emscriptenCanvasIdValue)
				{
					propertiesUsed.TrySetStringValue(Window<Emscripten>.PropertyNames.CreateEmscriptenCanvasIdString, emscriptenCanvasIdValue);
				}

				if (emscriptenKeyboardElement is string emscriptenKeyboardElementValue)
				{
					propertiesUsed.TrySetStringValue(Window<Emscripten>.PropertyNames.CreateEmscriptenKeyboardElementString, emscriptenKeyboardElementValue);
				}
			}
			else
			{
				propertiesUsed = properties;

				if (emscriptenCanvasId is string emscriptenCanvasIdValue)
				{
					emscriptenCanvasIdBackup = propertiesUsed.TryGetStringValue(Window<Emscripten>.PropertyNames.CreateEmscriptenCanvasIdString, out var exisitingEmscriptenCanvasId) is true
						? exisitingEmscriptenCanvasId
						: null;

					propertiesUsed.TrySetStringValue(Window<Emscripten>.PropertyNames.CreateEmscriptenCanvasIdString, emscriptenCanvasIdValue);
				}

				if (emscriptenKeyboardElement is string emscriptenKeyboardElementValue)
				{
					emscriptenKeyboardElementBackup = propertiesUsed.TryGetStringValue(Window<Emscripten>.PropertyNames.CreateEmscriptenKeyboardElementString, out var exisitingEmscriptenKeyboardElement) is true
						? exisitingEmscriptenKeyboardElement
						: null;

					propertiesUsed.TrySetStringValue(Window<Emscripten>.PropertyNames.CreateEmscriptenKeyboardElementString, emscriptenKeyboardElementValue);
				}
			}

			try
			{
				return Window.TryCreateUnchecked(
					out window, alwaysOnTop, bordered, constrainPopup, externalGraphicsContext, focusable,
					fullscreen, height, hidden, highPixelDensity, maximized, menu, metal, minimized,
					modal, mouseGrabbed, openGL, parent, resizable, title, transparent, tooltip,
					utility, vulkan, width, x, y, propertiesUsed
				);
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

					if (emscriptenCanvasId is not null)
					{
						if (emscriptenCanvasIdBackup is string emscriptenCanvasIdValue)
						{
							propertiesUsed.TrySetStringValue(Window<Emscripten>.PropertyNames.CreateEmscriptenCanvasIdString, emscriptenCanvasIdValue);
						}
						else
						{
							propertiesUsed.TryRemove(Window<Emscripten>.PropertyNames.CreateEmscriptenCanvasIdString);
						}
					}

					if (emscriptenKeyboardElement is not null)
					{
						if (emscriptenKeyboardElementBackup is string emscriptenKeyboardElementValue)
						{
							propertiesUsed.TrySetStringValue(Window<Emscripten>.PropertyNames.CreateEmscriptenKeyboardElementString, emscriptenKeyboardElementValue);
						}
						else
						{
							propertiesUsed.TryRemove(Window<Emscripten>.PropertyNames.CreateEmscriptenKeyboardElementString);
						}
					}
				}
			}
		}
	}

	extension(Window<Emscripten> window)
	{
		public string? EmscriptenCanvasId => window?.Properties?.TryGetStringValue(Window<Emscripten>.PropertyNames.EmscriptenCanvasIdString, out var emscriptenCanvasId) is true
			? emscriptenCanvasId
			: default;

		public string? EmscriptenKeyboardElement => window?.Properties?.TryGetStringValue(Window<Emscripten>.PropertyNames.EmscriptenKeyboardElementString, out var emscriptenKeyboardElement) is true
			? emscriptenKeyboardElement
			: default;

#if SDL3_4_0_OR_GREATER

		public bool IsFillDocument
		{
			get => (window?.Flags & WindowFlags.FillDocument) is not 0;

			set
			{
				unsafe
				{
					ErrorHelper.ThrowIfFailed(SDL_SetWindowFillDocument(window is not null ? window.Pointer : null, value));
				}
			}
		}

#endif
	}
}
