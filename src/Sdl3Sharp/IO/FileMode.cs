namespace Sdl3Sharp.IO;

/// <summary>
/// Defines constants for controlling how to open a file
/// </summary>
public enum FileMode
{
	/// <summary>Tries to open an existing file. Fails if the file doesn't exist.</summary>
	Open,

	/// <summary>Creates a new file or overwrites an existing one</summary>
	Create,

	/// <summary>Creates a new file. Fails if the file already exists.</summary>
	CreateNew,

	/// <summary>Appends to the existing file or creates a new one</summary>
	Append
}
