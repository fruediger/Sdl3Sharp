namespace Sdl3Sharp;

/// <summary>
/// Represents return values for the <see cref="AppBase"/>s main methods
/// </summary>
public enum AppResult
{
	/// <summary>A value that requests that the app continue from the <see cref="AppBase"/>s main methods</summary>
	Continue,

	/// <summary>A value that requests termination with success from the <see cref="AppBase"/>s main methods</summary>
	Success,

	/// <summary>A value that requests termination with error from the <see cref="AppBase"/>s main methods</summary>
	Failure
}
