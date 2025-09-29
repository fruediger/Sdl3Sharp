namespace Sdl3Sharp;

partial struct LogPriority
{
	/// <summary>
	/// The predefined log priorities
	/// </summary>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_LogPriority">SDL_LogPriority</seealso>
	internal enum Kind
	{
		/// <summary>SDL_LOG_PRIORITY_INVALID</summary>
		Invalid,

		/// <summary>SDL_LOG_PRIORITY_TRACE</summary>
		Trace,

		/// <summary>SDL_LOG_PRIORITY_VERBOSE</summary>
		Verbose,

		/// <summary>SDL_LOG_PRIORITY_DEBUG</summary>
		Debug,

		/// <summary>SDL_LOG_PRIORITY_INFO</summary>
		Info,

		/// <summary>SDL_LOG_PRIORITY_WARN</summary>
		Warn,

		/// <summary>SDL_LOG_PRIORITY_ERROR</summary>
		Error,

		/// <summary>SDL_LOG_PRIORITY_CRITICAL</summary>
		Critical
	}
}
