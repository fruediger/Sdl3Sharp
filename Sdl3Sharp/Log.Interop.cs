using Sdl3Sharp.Interop;
using Sdl3Sharp.SourceGeneration;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
using unsafe LogOutputFunction = delegate* unmanaged[Cdecl]<void*, Sdl3Sharp.LogCategory, Sdl3Sharp.LogPriority, byte*, void>;

namespace Sdl3Sharp;

partial class Log
{
	private static readonly LogOutputEventArgs mOutputEventArgs = new();

	[UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
	private unsafe static void LogOutputCallback(void* userdata, LogCategory category, LogPriority priority, byte* message)
	{
		mOutputEventArgs.Handled = false;

		if (Utf8StringMarshaller.ConvertToManaged(message) is { } messageUtf16)
		{
			Output?.Invoke(category, priority, messageUtf16, mOutputEventArgs);
		}

		if (!mOutputEventArgs.Handled && UseDefaultOutputForUnhandledMessages)
		{
			SDL_GetDefaultLogOutputFunction()(userdata, category, priority, message);
		}
	}

	/// <summary>
	/// Get the default log output function
	/// </summary>
	/// <returns>Returns the default log output callback</returns>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetDefaultLogOutputFunction">SDL_GetDefaultLogOutputFunction</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial LogOutputFunction SDL_GetDefaultLogOutputFunction();

	/// <summary>
	/// Get the current log output function
	/// </summary>
	/// <param name="callback">an <see href="https://wiki.libsdl.org/SDL3/SDL_LogOutputFunction">SDL_LogOutputFunction</see> filled in with the current log callback</param>
	/// <param name="userdata">a pointer filled in with the pointer that is passed to <c>callback</c></param>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetLogOutputFunction">SDL_GetLogOutputFunction</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial void SDL_GetLogOutputFunction(LogOutputFunction* callback, void** userdata);

	/// <summary>
	/// Log a message with <see href="https://wiki.libsdl.org/SDL3/SDL_LOG_CATEGORY_APPLICATION">SDL_LOG_CATEGORY_APPLICATION</see> and <see href="https://wiki.libsdl.org/SDL3/SDL_LOG_PRIORITY_INFO">SDL_LOG_PRIORITY_INFO</see>
	/// </summary>
	/// <param name="fmt">a printf() style message format string</param>
	/// <remarks>
	/// NOTE: Regarding CLR-interop: Since there is currently no clean and platform-/runtime-indepent way to indirectly call external functions with variadic arguments,
	/// those arguments are omitted from this method signature. Instead format your message on the CLR side and set <c><paramref name="fmt"/></c> to that already formatted string.
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_Log">SDL_Log</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial void SDL_Log(byte* fmt);

	/// <summary>
	/// Log a message with <see href="https://wiki.libsdl.org/SDL3/SDL_LOG_PRIORITY_CRITICAL">SDL_LOG_PRIORITY_CRITICAL</see>
	/// </summary>
	/// <param name="category">the category of the message</param>
	/// <param name="fmt">a printf() style message format string</param>
	/// <remarks>
	/// NOTE: Regarding CLR-interop: Since there is currently no clean and platform-/runtime-indepent way to indirectly call external functions with variadic arguments,
	/// those arguments are omitted from this method signature. Instead format your message on the CLR side and set <c><paramref name="fmt"/></c> to that already formatted string.
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_LogCritical">SDL_LogCritical</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial void SDL_LogCritical(LogCategory category, byte* fmt);

	/// <summary>
	/// Log a message with <see href="https://wiki.libsdl.org/SDL3/SDL_LOG_PRIORITY_DEBUG">SDL_LOG_PRIORITY_DEBUG</see>
	/// </summary>
	/// <param name="category">the category of the message</param>
	/// <param name="fmt">a printf() style message format string</param>
	/// <remarks>
	/// NOTE: Regarding CLR-interop: Since there is currently no clean and platform-/runtime-indepent way to indirectly call external functions with variadic arguments,
	/// those arguments are omitted from this method signature. Instead format your message on the CLR side and set <c><paramref name="fmt"/></c> to that already formatted string.
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_LogDebug">SDL_LogDebug</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial void SDL_LogDebug(LogCategory category, byte* fmt);

	/// <summary>
	/// Log a message with <see href="https://wiki.libsdl.org/SDL3/SDL_LOG_PRIORITY_ERROR">SDL_LOG_PRIORITY_ERROR</see>
	/// </summary>
	/// <param name="category">the category of the message</param>
	/// <param name="fmt">a printf() style message format string</param>
	/// <remarks>
	/// NOTE: Regarding CLR-interop: Since there is currently no clean and platform-/runtime-indepent way to indirectly call external functions with variadic arguments,
	/// those arguments are omitted from this method signature. Instead format your message on the CLR side and set <c><paramref name="fmt"/></c> to that already formatted string.
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_LogError">SDL_LogError</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial void SDL_LogError(LogCategory category, byte* fmt);

	/// <summary>
	/// Log a message with <see href="https://wiki.libsdl.org/SDL3/SDL_LOG_PRIORITY_INFO">SDL_LOG_PRIORITY_INFO</see>
	/// </summary>
	/// <param name="category">the category of the message</param>
	/// <param name="fmt">a printf() style message format string</param>
	/// <remarks>
	/// NOTE: Regarding CLR-interop: Since there is currently no clean and platform-/runtime-indepent way to indirectly call external functions with variadic arguments,
	/// those arguments are omitted from this method signature. Instead format your message on the CLR side and set <c><paramref name="fmt"/></c> to that already formatted string.
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_LogInfo">SDL_LogInfo</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial void SDL_LogInfo(LogCategory category, byte* fmt);

	/// <summary>
	/// Log a message with the specified category and priority
	/// </summary>
	/// <param name="category">the category of the message</param>
	/// <param name="priority">the priority of the message</param>
	/// <param name="fmt">a printf() style message format string</param>
	/// <remarks>
	/// NOTE: Regarding CLR-interop: Since there is currently no clean and platform-/runtime-indepent way to indirectly call external functions with variadic arguments,
	/// those arguments are omitted from this method signature. Instead format your message on the CLR side and set <c><paramref name="fmt"/></c> to that already formatted string.
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_LogMessage">SDL_LogMessage</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial void SDL_LogMessage(LogCategory category, LogPriority priority, byte* fmt);

	/// <summary>
	/// Log a message with <see href="https://wiki.libsdl.org/SDL3/SDL_LOG_PRIORITY_TRACE">SDL_LOG_PRIORITY_TRACE</see>
	/// </summary>
	/// <param name="category">the category of the message</param>
	/// <param name="fmt">a printf() style message format string</param>
	/// <remarks>
	/// NOTE: Regarding CLR-interop: Since there is currently no clean and platform-/runtime-indepent way to indirectly call external functions with variadic arguments,
	/// those arguments are omitted from this method signature. Instead format your message on the CLR side and set <c><paramref name="fmt"/></c> to that already formatted string.
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_LogTrace">SDL_LogTrace</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial void SDL_LogTrace(LogCategory category, byte* fmt);

	/// <summary>
	/// Log a message with <see href="https://wiki.libsdl.org/SDL3/SDL_LOG_PRIORITY_VERBOSE">SDL_LOG_PRIORITY_VERBOSE</see>
	/// </summary>
	/// <param name="category">the category of the message</param>
	/// <param name="fmt">a printf() style message format string</param>
	/// <remarks>
	/// NOTE: Regarding CLR-interop: Since there is currently no clean and platform-/runtime-indepent way to indirectly call external functions with variadic arguments,
	/// those arguments are omitted from this method signature. Instead format your message on the CLR side and set <c><paramref name="fmt"/></c> to that already formatted string.
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_LogVerbose">SDL_LogVerbose</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial void SDL_LogVerbose(LogCategory category, byte* fmt);

	/// <summary>
	/// Log a message with <see href="https://wiki.libsdl.org/SDL3/SDL_LOG_PRIORITY_WARN">SDL_LOG_PRIORITY_WARN</see>
	/// </summary>
	/// <param name="category">the category of the message</param>
	/// <param name="fmt">a printf() style message format string</param>
	/// <remarks>
	/// NOTE: Regarding CLR-interop: Since there is currently no clean and platform-/runtime-indepent way to indirectly call external functions with variadic arguments,
	/// those arguments are omitted from this method signature. Instead format your message on the CLR side and set <c><paramref name="fmt"/></c> to that already formatted string.
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_LogWarn">SDL_LogWarn</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial void SDL_LogWarn(LogCategory category, byte* fmt);

	/// <summary>
	/// Replace the default log output function with one of your own
	/// </summary>
	/// <param name="callback">an <see href="https://wiki.libsdl.org/SDL3/SDL_LogOutputFunction">SDL_LogOutputFunction</see> to call instead of the default</param>
	/// <param name="userdata">a pointer that is passed to <c>callback</c></param>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_SetLogOutputFunction">SDL_SetLogOutputFunction</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial void SDL_SetLogOutputFunction(LogOutputFunction callback, void* userdata);
}
