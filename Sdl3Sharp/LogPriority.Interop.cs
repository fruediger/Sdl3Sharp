using Sdl3Sharp.Internal.Interop;
using Sdl3Sharp.SourceGeneration;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp;

partial struct LogPriority
{
	/// <summary>
	/// Set the text prepended to log messages of a given priority
	/// </summary>
	/// <param name="priority">the <see href="https://wiki.libsdl.org/SDL3/SDL_LogPriority">SDL_LogPriority</see> to modify</param>
	/// <param name="prefix">the prefix to use for that log priority, or NULL to use no prefix</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// By default <see href="https://wiki.libsdl.org/SDL3/SDL_LOG_PRIORITY_INFO">SDL_LOG_PRIORITY_INFO</see> and below have no prefix, and <see href="https://wiki.libsdl.org/SDL3/SDL_LOG_PRIORITY_WARN">SDL_LOG_PRIORITY_WARN</see> and higher have a prefix showing their priority, e.g. "WARNING: ".
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_SetLogPriorityPrefix">SDL_SetLogPriorityPrefix</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_SetLogPriorityPrefix(LogPriority priority, byte* prefix);
}
