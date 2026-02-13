using Sdl3Sharp.Video.Rendering.Drivers;

namespace Sdl3Sharp.Video.Rendering;

partial class RendererExtensions
{
	extension(Renderer<Software>.PropertyNames)
	{
		/// <summary>
		/// The name of a <see cref="Surface.TryCreateRenderer(out Renderer{Software}?, Coloring.ColorSpace?, Properties?)">property used when creating a <see cref="Renderer{TDriver}">Renderer&lt;<see cref="Software">Software</see>&gt;</see></see>
		/// that holds a pointer to the native <c><see href="https://wiki.libsdl.org/SDL3/SDL_Surface">SDL_Surface</see></c> where rendering is displayed to, if you want to create a software renderer without a window
		/// </summary>
		public static string CreateSoftwareSurfacePointer => "SDL.renderer.create.surface";

		/// <summary>
		/// The name of a <em>read-only</em> <see cref="IRenderer.Properties">property</see> that holds
		/// a pointer to the native <c><see href="https://wiki.libsdl.org/SDL3/SDL_Surface">SDL_Surface</see></c> where rendering is displayed to, if the renderer is a software renderer without a window
		/// </summary>
		public static string SoftwareSurfacePointer => "SDL.renderer.surface";
	}

	extension(Renderer<Software> renderer)
	{
		/// <summary>
		/// Gets the <see cref="Surface"/> the software renderer is rendering to
		/// </summary>
		/// <value>
		/// The <see cref="Surface"/> the software renderer is rendering to, or <see langword="null"/> is associated with a <see cref="Windowing.Window"/> instead
		/// </value>
		/// <remarks>
		/// <para>
		/// If the value of this property is <see langword="null"/>, then the renderer is associated with a <see cref="Windowing.Window"/> instead and you can use <see cref="Renderer{TDriver}.Window"/> property to get the window.
		/// </para>
		/// </remarks>
		public Surface? Surface
		{
			get
			{
				unsafe
				{
					return renderer?.Properties?.TryGetPointerValue(Renderer<Software>.PropertyNames.SoftwareSurfacePointer, out var softwareSurfacePtr) is true
						&& Surface.TryGetOrCreate(unchecked((Surface.SDL_Surface*)softwareSurfacePtr), out var softwareSurface)
						? softwareSurface
						: default;
				}
			}
		}
	}
}
