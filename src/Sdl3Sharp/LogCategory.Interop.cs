using Sdl3Sharp.Internal.Interop;
using Sdl3Sharp.SourceGeneration;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp;

partial struct LogCategory
{
	/// <summary>
	/// Get the priority of a particular log category
	/// </summary>
	/// <param name="category">the category to query</param>
	/// <returns>Returns the <see href="https://wiki.libsdl.org/SDL3/SDL_LogPriority">SDL_LogPriority</see> for the requested category</returns>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetLogPriority">SDL_GetLogPriority</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal static partial LogPriority SDL_GetLogPriority(LogCategory category);

	/// <summary>
	/// Reset all priorities to default
	/// </summary>
	/// <remarks>
	/// This is called by <see href="https://wiki.libsdl.org/SDL3/SDL_Quit">SDL_Quit</see>()
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_ResetLogPriorities">SDL_ResetLogPriorities</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal static partial void SDL_ResetLogPriorities();

	/// <summary>
	/// Set the priority of all log categories
	/// </summary>
	/// <param name="priority">the <see href="https://wiki.libsdl.org/SDL3/SDL_LogPriority">SDL_LogPriority</see> to assign</param>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_SetLogPriorities">SDL_SetLogPriorities</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal static partial void SDL_SetLogPriorities(LogPriority priority);

	/// <summary>
	/// Set the priority of a particular log category
	/// </summary>
	/// <param name="category">the category to assign a priority to</param>
	/// <param name="priority">the <see href="https://wiki.libsdl.org/SDL3/SDL_LogPriority">SDL_LogPriority</see> to assign</param>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_SetLogPriority">SDL_SetLogPriority</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal static partial void SDL_SetLogPriority(LogCategory category, LogPriority priority);
}
