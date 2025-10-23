namespace Sdl3Sharp.IO;

/// <summary>
/// Defines constants to describe the kind of a file
/// </summary>
public enum FileKind
{
	/// <summary>The file kind is unspecified</summary>
	Unspecified = default,

	/// <summary>The file is a binary file</summary>
	Binary,

	/// <summary>The file is a text file</summary>
	Text,
}
