using Sdl3Sharp.Video.Rendering.Drivers;
using System;

namespace Sdl3Sharp.Video.Rendering;

partial class RendererExtensions
{
	extension(Renderer<Direct3D9>.PropertyNames)
	{
		/// <summary>
		/// The name of a <em>read-only</em> <see cref="Renderer.Properties">property</see> that holds
		/// a pointer to the <c><see href="https://docs.microsoft.com/en-us/windows/win32/api/d3d9/nn-d3d9-idirect3ddevice9">IDirect3DDevice9</see></c> associated with the renderer
		/// </summary>
		public static string Direct3D9DevicePointer => "SDL.renderer.d3d9.device";
	}

	extension(Renderer<Direct3D9> renderer)
	{
		/// <summary>
		/// Gets the <c><see href="https://docs.microsoft.com/en-us/windows/win32/api/d3d9/nn-d3d9-idirect3ddevice9">IDirect3DDevice9</see></c> associated with the renderer
		/// </summary>
		/// <value>
		/// The <c><see href="https://docs.microsoft.com/en-us/windows/win32/api/d3d9/nn-d3d9-idirect3ddevice9">IDirect3DDevice9</see></c> associated with the renderer
		/// </value>
		/// <remarks>
		/// <para>
		/// The value of this property can be directly cast to an <c><see href="https://docs.microsoft.com/en-us/windows/win32/api/d3d9/nn-d3d9-idirect3ddevice9">IDirect3DDevice9</see>*</c> pointer.
		/// </para>
		/// </remarks>
		public IntPtr Direct3D9Device => renderer?.Properties?.TryGetPointerValue(Renderer<Direct3D9>.PropertyNames.Direct3D9DevicePointer, out var direct3D9Device) is true
			? direct3D9Device
			: default;
	}
}
