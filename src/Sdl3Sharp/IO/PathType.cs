namespace Sdl3Sharp.IO;

/// <summary>
/// Represents the type of a file system entry
/// </summary>
public enum PathType
{
	/// <summary>The path does not exist</summary>
	None,

	/// <summary>The path represents a file</summary>
	File,

	/// <summary>The path represents a directory</summary>
	Directory,

	/// <summary>The path represents another kind of file system entry</summary>
	/// <remarks>
	/// <para>
	/// This could be something like a device node, a named pipe, or a completely different kind of file system entry, but <em>not</em> a symbolic link, as they are always followed transparently.
	/// </para>
	/// </remarks>
	Other
}
