namespace Sdl3Sharp.Video.Blending;

/// <summary>
/// Represents blend operations used to combine pixel components in blending operations
/// </summary>
public enum BlendOperation
{
	/// <summary>Additive blending</summary>
	/// <remarks>
	/// <para>
	/// <code>dst + src</code>
	/// </para>
	/// <para>
	/// Supported by all renderers.
	/// </para>
	/// </remarks>
	Add = 0x1,

	/// <summary>Subtractive blending</summary>
	/// <remarks>
	/// <para>
	/// <code>
	/// src - dst
	/// </code>
	/// </para>
	/// <para>
	/// Supported by Direct3D, OpenGL, OpenGLES, and Vulkan.
	/// </para>
	/// </remarks>
	Subtract = 0x2,

	/// <summary>Reverse subtractive blending</summary>
	/// <remarks>
	/// <para>
	/// <code>
	/// dst - src
	/// </code>
	/// </para>
	/// <para>
	/// Supported by Direct3D, OpenGL, OpenGLES, and Vulkan.
	/// </para>
	/// </remarks>
	ReverseSubtract = 0x3,

	/// <summary>Minimum blending</summary>
	/// <remarks>
	/// <para>
	/// <code>
	/// min(dst, src)
	/// </code>
	/// </para>
	/// <para>
	/// Supported by Direct3D, OpenGL, OpenGLES, and Vulkan.
	/// </para>
	/// </remarks>
	Minimum = 0x4,

	/// <summary>Maximum blending</summary>
	/// <remarks>
	/// <para>
	/// <code>
	/// max(dst, src)
	/// </code>
	/// </para>
	/// <para>
	/// Supported by Direct3D, OpenGL, OpenGLES, and Vulkan.
	/// </para>
	/// </remarks>
	Maximum = 0x5
}
