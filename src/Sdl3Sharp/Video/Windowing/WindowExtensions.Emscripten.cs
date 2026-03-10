using Sdl3Sharp.Internal;
using Sdl3Sharp.Video.Windowing.Drivers;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Video.Windowing;

partial class WindowExtensions
{
	extension(Window<Emscripten>.PropertyNames)
	{
		/// <summary>
		/// The name of a <see cref="TryCreate(out Window{Emscripten}?, bool?, bool?, bool?, bool?, WindowFlags?, bool?, bool?, int?, bool?, bool?, bool?, bool?, bool?, bool?, bool?, bool?, bool?, Window?, bool?, string?, bool?, bool?, bool?, bool?, int?, WindowPosition?, WindowPosition?, string?, string?, Properties?)">property used when creating a <see cref="Window{TDriver}">Window&lt;<see cref="Emscripten">Emscripten</see>&gt;</see></see>
		/// that holds the ID given to the canvas element to use for the window
		/// </summary>
		/// <remarks>
		/// <para>
		/// The value of the associated property should start with a <c>#</c> character.
		/// </para>
		/// </remarks>
		public static string CreateEmscriptenCanvasIdString => "SDL.window.create.emscripten.canvas_id";

		/// <summary>
		/// The name of a <see cref="TryCreate(out Window{Emscripten}?, bool?, bool?, bool?, bool?, WindowFlags?, bool?, bool?, int?, bool?, bool?, bool?, bool?, bool?, bool?, bool?, bool?, bool?, Window?, bool?, string?, bool?, bool?, bool?, bool?, int?, WindowPosition?, WindowPosition?, string?, string?, Properties?)">property used when creating a <see cref="Window{TDriver}">Window&lt;<see cref="Emscripten">Emscripten</see>&gt;</see></see>
		/// that holds the identifier to use to override the binding element for keyboard inputs for the canvas element used for the window
		/// </summary>
		/// <remarks>
		/// <para>
		/// The value of the associated property can be one of the following:
		/// <list type="bullet">
		///		<item>
		///			<term><c>"#window"</c></term>
		///			<description>The javascript window object. This is the default.</description>
		///		</item>
		///		<item>
		///			<term><c>"#document"</c></term>
		///			<description>The javascript document object</description>
		///		</item>
		///		<item>
		///			<term><c>"#screen"</c></term>
		///			<description>The javascript <c>window.screen</c> object</description>
		///		</item>
		///		<item>
		///			<term><c>"#canvas"</c></term>
		///			<description>The WebGL canvas element</description>
		///		</item>
		///		<item>
		///			<term><c>"#none"</c></term>
		///			<description>Don't bind anything at all</description>
		///		</item>
		///		<item>
		///			<term>Any other string without a leading <c>#</c> character</term>
		///			<description>Bind to the element on the page with that ID</description>
		///		</item>
		/// </list>
		/// </para>
		/// </remarks>
		public static string CreateEmscriptenKeyboardElementString => "SDL.window.create.emscripten.keyboard_element";

		/// <summary>
		/// The name of a <em>read-only</em> <see cref="Window.Properties">property</see> that holds
		/// the ID of the canvas element used for the window
		/// </summary>
		public static string EmscriptenCanvasIdString => "SDL.window.emscripten.canvas_id";

		/// <summary>
		/// The name of a <em>read-only</em> <see cref="Window.Properties">property</see> that holds
		/// the identifier of the element used for keyboard inputs for the canvas element used for the window
		/// </summary>
		public static string EmscriptenKeyboardElementString => "SDL.window.emscripten.keyboard_element";
	}

	extension(Window<Emscripten>)
	{
		/// <inheritdoc cref="Window.TryCreate(out Window?, bool?, bool?, bool?, bool?, WindowFlags?, bool?, bool?, int?, bool?, bool?, bool?, bool?, bool?, bool?, bool?, bool?, bool?, Window?, bool?, string?, bool?, bool?, bool?, bool?, int?, WindowPosition?, WindowPosition?, Properties?)" />
		/// <param name="emscriptenCanvasId">The ID of the canvas element to use for the window. The value of this parameter should start with a <c>#</c> character.</param>
		/// <param name="emscriptenKeyboardElement">
		/// The identifier to use to override the binding element for keyboard inputs for the canvas element used for the window.
		/// The value of this parameter can be one of the following:
		/// <list type="bullet">
		///		<item>
		///			<term><c>"#window"</c></term>
		///			<description>The javascript window object. This is the default.</description>
		///		</item>
		///		<item>
		///			<term><c>"#document"</c></term>
		///			<description>The javascript document object</description>
		///		</item>
		///		<item>
		///			<term><c>"#screen"</c></term>
		///			<description>The javascript <c>window.screen</c> object</description>
		///		</item>
		///		<item>
		///			<term><c>"#canvas"</c></term>
		///			<description>The WebGL canvas element</description>
		///		</item>
		///		<item>
		///			<term><c>"#none"</c></term>
		///			<description>Don't bind anything at all</description>
		///		</item>
		///		<item>
		///			<term>Any other string without a leading <c>#</c> character</term>
		///			<description>Bind to the element on the page with that ID</description>
		///		</item>
		/// </list>
		/// </param>
#pragma warning disable CS1573 // we get these from inheritdoc
		public static bool TryCreate([NotNullWhen(true)] out Window<Emscripten>? window, bool? alwaysOnTop = default, bool? bordered = default, bool? constrainPopup = default, bool? externalGraphicsContext = default, WindowFlags? flags = default,
			bool? focusable = default, bool? fullscreen = default, int? height = default, bool? hidden = default, bool? highPixelDensity = default, bool? maximized = default, bool? menu = default, bool? metal = default,
			bool? minimized = default, bool? modal = default, bool? mouseGrabbed = default, bool? openGL = default, Window? parent = default, bool? resizable = default, string? title = default, bool? transparent = default,
			bool? tooltip = default, bool? utility = default, bool? vulkan = default, int? width = default, WindowPosition? x = default, WindowPosition? y = default,
			string? emscriptenCanvasId = default, string? emscriptenKeyboardElement = default, Properties? properties = default)
#pragma warning restore CS1573
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
					out window, alwaysOnTop, bordered, constrainPopup, externalGraphicsContext, flags,
					focusable, fullscreen, height, hidden, highPixelDensity, maximized, menu, metal,
					minimized, modal, mouseGrabbed, openGL, parent, resizable, title, transparent,
					tooltip, utility, vulkan, width, x, y, propertiesUsed
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
		/// <summary>
		/// Gets the ID of the canvas element used for this window
		/// </summary>
		/// <value>
		/// The ID of the canvas element used for this window
		/// </value>
		/// <remarks>
		/// <para>
		/// This property should only be accessed from the main thread.
		/// </para>
		/// </remarks>
		public string? EmscriptenCanvasId => window?.Properties?.TryGetStringValue(Window<Emscripten>.PropertyNames.EmscriptenCanvasIdString, out var emscriptenCanvasId) is true
			? emscriptenCanvasId
			: default;

		/// <summary>
		/// Gets the identifier of the element used for keyboard inputs for the canvas element used for this window
		/// </summary>
		/// <value>
		/// The identifier of the element used for keyboard inputs for the canvas element used for this window
		/// </value>
		/// <remarks>
		/// <para>
		/// This property should only be accessed from the main thread.
		/// </para>
		/// </remarks>
		public string? EmscriptenKeyboardElement => window?.Properties?.TryGetStringValue(Window<Emscripten>.PropertyNames.EmscriptenKeyboardElementString, out var emscriptenKeyboardElement) is true
			? emscriptenKeyboardElement
			: default;

#if SDL3_4_0_OR_GREATER

		/// <summary>
		/// Gets or sets a value indicating whether the window should be fill the current document space
		/// </summary>
		/// <value>
		/// A value indicating whether the window should be fill the current document space
		/// </value>
		/// <remarks>
		/// <para>
		/// When fill-document mode is enabled, the canvas element fills the entire document.
		/// Resize events will be generated as the browser window is resized, as that will adjust the canvas size as well.
		/// The canvas will cover anything else on the page, including any controls provided by Emscripten in its generated HTML file
		/// (in fact, any elements on the page that aren't the canvas will be moved into a hidden <c>div</c> element).
		/// </para>
		/// <para>
		/// Often times this is desirable for a browser-based game, but it means several things that are usually expected to work for windows on other platforms might not work as expected,
		/// such as minimum window sizes and aspect ratios.
		/// </para>
		/// <para>
		/// The value of this property is reflected as the <see cref="WindowFlags.FillDocument"/> flag in the <see cref="Window.Flags"/> property of the window.
		/// </para>
		/// <para>
		/// This property should only be accessed from the main thread.
		/// </para>
		/// </remarks>
		/// <exception cref="SdlException">When setting this property, SDL failed with an error (check <see cref="Error.TryGet(out string?)"/> for more information)</exception>
		public bool IsFillDocument
		{
			get => (window?.Flags & WindowFlags.FillDocument) is not 0;

			set
			{
				unsafe
				{
					SdlErrorHelper.ThrowIfFailed(SDL_SetWindowFillDocument(window is not null ? window.Pointer : null, value));
				}
			}
		}

#endif
	}
}
