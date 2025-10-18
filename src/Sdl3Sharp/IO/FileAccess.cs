using System;
using System.ComponentModel;

namespace Sdl3Sharp.IO;

/// <summary>
/// Defines constants for controlling how to access the contents of a file
/// </summary>
[Flags]
public enum FileAccess
{
	/// <summary>Doesn't allow for neither reading nor writing the contents of a file</summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	None      = 0b00,

	/// <summary>Allows for reading the contents of a file</summary>
	Read      = 0b01,

	/// <summary>Allows for writing the contents of a file</summary>
	Write     = 0b10,

	/// <summary>Allows for reading and writing the contents of a file</summary>
	ReadWrite = Read | Write,
}
