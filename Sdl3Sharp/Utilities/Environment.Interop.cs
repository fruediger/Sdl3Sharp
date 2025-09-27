using Sdl3Sharp.Internal.Interop;
using Sdl3Sharp.SourceGeneration;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sdl3Sharp.Utilities;

partial class Environment
{
	// Used for opaque pointers
	[StructLayout(LayoutKind.Sequential, Size = 0)]
	internal readonly struct SDL_Environment;
	
	/// <summary>
	/// Create a set of environment variables
	/// </summary>
	/// <param name="populated">true to initialize it from the C runtime environment, false to create an empty environment</param>
	/// <returns>Returns a pointer to the new environment or NULL on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// If <c><paramref name="populated"/></c> is false, it is safe to call this function from any thread, otherwise it is safe if no other threads are calling setenv() or unsetenv()
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_CreateEnvironment">SDL_CreateEnvironment</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial SDL_Environment* SDL_CreateEnvironment(CBool populated);
	
	/// <summary>
	/// Destroy a set of environment variables
	/// </summary>
	/// <param name="env">the environment to destroy</param>
	/// <remarks>
	/// It is safe to call this function from any thread, as long as the environment is no longer in use.
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_DestroyEnvironment">SDL_DestroyEnvironment</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial void SDL_DestroyEnvironment(SDL_Environment* env);

	/// <summary>
	/// Get the process environment
	/// </summary>
	/// <returns>Returns a pointer to the environment for the process or NULL on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// This is initialized at application start and is not affected by setenv() and unsetenv() calls after that point.
	/// Use <see href="https://wiki.libsdl.org/SDL3/SDL_SetEnvironmentVariable">SDL_SetEnvironmentVariable</see>() and <see href="https://wiki.libsdl.org/SDL3/SDL_UnsetEnvironmentVariable">SDL_UnsetEnvironmentVariable</see>() if you want to modify this environment,
	/// or <see href="https://wiki.libsdl.org/SDL3/SDL_setenv_unsafe">SDL_setenv_unsafe</see>() or <see href="https://wiki.libsdl.org/SDL3/SDL_unsetenv_unsafe">SDL_unsetenv_unsafe</see>() if you want changes to persist in the C runtime environment after <see href="https://wiki.libsdl.org/SDL3/SDL_Quit">SDL_Quit</see>().
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetEnvironment">SDL_GetEnvironment</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial SDL_Environment* SDL_GetEnvironment();

	/// <summary>
	/// Get the value of a variable in the environment
	/// </summary>
	/// <param name="env">the environment to query</param>
	/// <param name="name">the name of the variable to get</param>
	/// <returns>Returns a pointer to the value of the variable or NULL if it can't be found</returns>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetEnvironmentVariable">SDL_GetEnvironmentVariable</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial byte* SDL_GetEnvironmentVariable(SDL_Environment* env, byte* name);

	/// <summary>
	/// Get all variables in the environment
	/// </summary>
	/// <param name="env">the environment to query</param>
	/// <returns>Returns a NULL terminated array of pointers to environment variables in the form "variable=value" or NULL on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information. This is a single allocation that should be freed with <see href="https://wiki.libsdl.org/SDL3/SDL_free">SDL_free</see>() when it is no longer needed.</returns>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetEnvironmentVariables">SDL_GetEnvironmentVariables</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial byte** SDL_GetEnvironmentVariables(SDL_Environment* env);

	/// <summary>
	/// Set the value of a variable in the environment
	/// </summary>
	/// <param name="env">the environment to modify</param>
	/// <param name="name">the name of the variable to set</param>
	/// <param name="value">the value of the variable to set</param>
	/// <param name="overwrite">true to overwrite the variable if it exists, false to return success without setting the variable if it already exists</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_SetEnvironmentVariable">SDL_SetEnvironmentVariable</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_SetEnvironmentVariable(SDL_Environment* env, byte* name, byte* value, CBool overwrite);

	/// <summary>
	/// Clear a variable from the environment
	/// </summary>
	/// <param name="env">the environment to modify</param>
	/// <param name="name">the name of the variable to unset</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_UnsetEnvironmentVariable">SDL_UnsetEnvironmentVariable</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_UnsetEnvironmentVariable(SDL_Environment* env, byte* name);

	/// <summary>
	/// Get the value of a variable in the environment
	/// </summary>
	/// <param name="name">the name of the variable to get</param>
	/// <returns>Returns a pointer to the value of the variable or NULL if it can't be found</returns>
	/// <remarks>
	/// This function uses SDL's cached copy of the environment and is thread-safe.
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_getenv">SDL_getenv</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial byte* SDL_getenv(byte* name);

	/// <summary>
	/// Get the value of a variable in the environment
	/// </summary>
	/// <param name="name">the name of the variable to get</param>
	/// <returns>Returns a pointer to the value of the variable or NULL if it can't be found</returns>
	/// <remarks>
	/// This function bypasses SDL's cached copy of the environment and is not thread-safe.
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_getenv_unsafe">SDL_getenv_unsafe</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial byte* SDL_getenv_unsafe(byte* name);

	/// <summary>
	/// Set the value of a variable in the environment
	/// </summary>
	/// <param name="name">the name of the variable to set</param>
	/// <param name="value">the value of the variable to set</param>
	/// <param name="overwrite">1 to overwrite the variable if it exists, 0 to return success without setting the variable if it already exists</param>
	/// <returns>Returns 0 on success, -1 on error</returns>
	/// <remarks>
	/// This function is not thread safe, consider using <see href="https://wiki.libsdl.org/SDL3/SDL_SetEnvironmentVariable">SDL_SetEnvironmentVariable</see>() instead.
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_setenv_unsafe">SDL_setenv_unsafe</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial int SDL_setenv_unsafe(byte* name, byte* value, int overwrite);

	/// <summary>
	/// Clear a variable from the environment
	/// </summary>
	/// <param name="name">the name of the variable to unset</param>
	/// <returns>Returns 0 on success, -1 on error</returns>
	/// <remarks>
	/// This function is not thread safe, consider using <see href="https://wiki.libsdl.org/SDL3/SDL_UnsetEnvironmentVariable">SDL_UnsetEnvironmentVariable</see>() instead.
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_unsetenv_unsafe">SDL_unsetenv_unsafe</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial int SDL_unsetenv_unsafe(byte* name);
}
