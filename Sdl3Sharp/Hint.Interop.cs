using Sdl3Sharp.Interop;
using Sdl3Sharp.SourceGeneration;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
using unsafe HintCallback = delegate* unmanaged[Cdecl]<void*, byte*, byte*, byte*, void>;

namespace Sdl3Sharp;

partial struct Hint
{
	[UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
	private unsafe static void HintCallback(void* userdata, byte* name, byte* oldValue, byte* newValue)
	{
		var nameUtf16 = Utf8StringMarshaller.ConvertToManaged(name);

		if (!string.IsNullOrWhiteSpace(nameUtf16) && mValueChangedEventHandlers.TryGetValue(nameUtf16, out var eventHandler))
		{
			eventHandler?.Invoke(sender: new(nameUtf16), Utf8StringMarshaller.ConvertToManaged(oldValue), Utf8StringMarshaller.ConvertToManaged(newValue));
		}
	}

	/// <summary>
	/// Add a function to watch a particular hint
	/// </summary>
	/// <param name="name">the hint to watch</param>
	/// <param name="callback">An <see href="https://wiki.libsdl.org/SDL3/SDL_HintCallback">SDL_HintCallback</see> function that will be called when the hint value changes</param>
	/// <param name="userdata">a pointer to pass to the callback function</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// The callback function is called <em>during</em> this function, to provide it an initial value, and again each time the hint's value changes
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_AddHintCallback">SDL_AddHintCallback</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_AddHintCallback(byte* name, HintCallback callback, void* userdata);

	/// <summary>
	/// Get the value of a hint
	/// </summary>
	/// <param name="name">the hint to query</param>
	/// <returns>Returns the string value of a hint or NULL if the hint isn't set</returns>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetHint">SDL_GetHint</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial byte* SDL_GetHint(byte* name);

	/// <summary>
	/// Get the boolean value of a hint variable
	/// </summary>
	/// <param name="name">the name of the hint to get the boolean value from</param>
	/// <param name="default_value">the value to return if the hint does not exist</param>
	/// <returns>Returns the boolean value of a hint or the provided default value if the hint does not exist</returns>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetHintBoolean">SDL_GetHintBoolean</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_GetHintBoolean(byte* name, CBool default_value);

	/// <summary>
	/// Remove a function watching a particular hint
	/// </summary>
	/// <param name="name">the hint being watched</param>
	/// <param name="callback">an <see href="https://wiki.libsdl.org/SDL3/SDL_HintCallback">SDL_HintCallback</see> function that will be called when the hint value changes</param>
	/// <param name="userdata">a pointer being passed to the callback function</param>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_RemoveHintCallback">SDL_RemoveHintCallback</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial void SDL_RemoveHintCallback(byte* name, HintCallback callback, void* userdata);

	/// <summary>
	/// Reset a hint to the default value
	/// </summary>
	/// <param name="name">the hint to set</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// This will reset a hint to the value of the environment variable, or NULL if the environment isn't set. Callbacks will be called normally with this change.
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_ResetHint">SDL_ResetHint</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_ResetHint(byte* name);

	/// <summary>
	/// Reset all hints to the default values
	/// </summary>
	/// <remarks>
	/// This will reset all hints to the value of the associated environment variable, or NULL if the environment isn't set. Callbacks will be called normally with this change.
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_ResetHints">SDL_ResetHints</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal static partial void SDL_ResetHints();

	/// <summary>
	/// Set a hint with normal priority
	/// </summary>
	/// <param name="name">the hint to set</param>
	/// <param name="value">the value of the hint variable</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// Hints will not be set if there is an existing override hint or environment variable that takes precedence. You can use <see href="https://wiki.libsdl.org/SDL3/SDL_SetHintWithPriority">SDL_SetHintWithPriority</see>() to set the hint with override priority instead.
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_SetHint">SDL_SetHint</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_SetHint(byte* name, byte* value);

	/// <summary>
	/// Set a hint with a specific priority
	/// </summary>
	/// <param name="name">the hint to set</param>
	/// <param name="value">the value of the hint variable</param>
	/// <param name="priority">the <see href="https://wiki.libsdl.org/SDL3/SDL_HintPriority">SDL_HintPriority</see> level for the hint</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// The priority controls the behavior when setting a hint that already has a value. Hints will replace existing hints of their priority and lower. Environment variables are considered to have override priority.
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_SetHintWithPriority">SDL_SetHintWithPriority</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_SetHintWithPriority(byte* name, byte* value, HintPriority priority);
}
