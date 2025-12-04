namespace Sdl3Sharp;

/// <summary>
/// Represents a log priority
/// </summary>
public enum LogPriority
{
	/// <summary>The log priority <em>Invalid</em></summary>
	/// <remarks>
	/// The log priority <c>Invalid</c> is not a real log priority. Instead it is used to indicate invalid logging messages which should not get logged.
	/// </remarks>
	Invalid,

	/// <summary>The log priority <em>Trace</em></summary>
	Trace,

	/// <summary>The log priority <em>Verbose</em></summary>
	Verbose,

	/// <summary>The log priority <em>Debug</em></summary>
	Debug,

	/// <summary>The log priority <em>Info</em></summary>
	Info,

	/// <summary>The log priority <em>Warn</em></summary>
	Warn,

	/// <summary>The log priority <em>Error</em></summary>
	Error,

	/// <summary>The log priority <em>Critical</em></summary>
	Critical
}
