namespace Sdl3Sharp.IO;

/// <summary>
/// Represents return values from <see cref="EnumerateDirectoryCallback"/>
/// </summary>
public enum EnumerationResult
{
	/// <summary>A value that requests the enumeration to continue</summary>
	Continue,

	/// <summary>A value that requests termination with success</summary>
	Success,

	/// <summary>A value that requests termination with error</summary>
	Failure
}
