namespace Sdl3Sharp.IO;

/// <summary>
/// Represents the type of an asynchronous I/O operation
/// </summary>
public enum AsyncIOTaskType
{
	/// <summary>A reading operation</summary>
	Read,

	/// <summary>A writing operation</summary>
	Write,

	/// <summary>A closing operation</summary>
	Close
}
