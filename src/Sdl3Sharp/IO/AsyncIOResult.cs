namespace Sdl3Sharp.IO;

/// <summary>
/// Represents possible outcomes of an asynchronous I/O operation
/// </summary>
public enum AsyncIOResult
{
	/// <summary>The asynchronous I/O operation completed successfully</summary>
	Complete,

	/// <summary>The asynchronous I/O operation failed (check <see cref="Error.TryGet(out string?)"/> for more information)</summary>
	Failure,

	/// <summary>The asynchronous I/O operation was canceled before completing</summary>
	Canceled
}
