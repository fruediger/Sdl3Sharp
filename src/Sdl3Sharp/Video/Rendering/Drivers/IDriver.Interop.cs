using Sdl3Sharp.Internal.Interop;
using Sdl3Sharp.SourceGeneration;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Video.Rendering.Drivers;

partial interface IDriver
{
	/// <summary>
	/// Get the number of 2D rendering drivers available for the current display
	/// </summary>
	/// <returns>Returns the number of built in render drivers</returns>
	/// <remarks>
	/// <para>
	/// A render driver is a set of code that handles rendering and texture management on a particular display.
	/// Normally there is only one, but some drivers may have several available with different capabilities.
	/// </para>
	/// <para>
	/// There may be none if SDL was compiled without render support.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetNumRenderDrivers">SDL_GetNumRenderDrivers</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal static partial int SDL_GetNumRenderDrivers();

	/// <summary>
	/// Use this function to get the name of a built in 2D rendering driver
	/// </summary>
	/// <param name="index">The index of the rendering driver; the value ranges from 0 to <see href="https://wiki.libsdl.org/SDL3/SDL_GetNumRenderDrivers">SDL_GetNumRenderDrivers</see>() - 1</param>
	/// <returns>Returns the name of the rendering driver at the requested index, or NULL if an invalid index was specified</returns>
	/// <remarks>
	/// <para>
	/// The list of rendering drivers is given in the order that they are normally initialized by default; the drivers that seem more reasonable to choose first (as far as the SDL developers believe) are earlier in the list.
	/// </para>
	/// <para>
	/// The names of drivers are all simple, low-ASCII identifiers, like "opengl", "direct3d12" or "metal". These never have Unicode characters, and are not meant to be proper names.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetRenderDriver">SDL_GetRenderDriver</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial byte* SDL_GetRenderDriver(int index);
}
