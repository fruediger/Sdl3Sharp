/*
namespace Sdl3Sharp.Threading;

partial struct InitState
{
	/// <summary>
	/// Finish an initialization state transition
	/// </summary>
	/// <param name="state">the initialization state to check</param>
	/// <param name="initialized">the new initialization state</param>
	/// <remarks>
	/// This function sets the status of the passed in state to <see href="https://wiki.libsdl.org/SDL3/SDL_INIT_STATUS_INITIALIZED"><c>SDL_INIT_STATUS_INITIALIZED</c></see> or <see href="https://wiki.libsdl.org/SDL3/SDL_INIT_STATUS_UNINITIALIZED"><c>SDL_INIT_STATUS_UNINITIALIZED</c></see> and allows any threads waiting for the status to proceed
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_SetInitialized">SDL_SetInitialized</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl), typeof(CallConvSuppressGCTransition)])]
	internal unsafe static partial void SDL_SetInitialized(InitState* state, CBool initialized);

	/// <summary>
	/// Return whether initialization should be done
	/// </summary>
	/// <param name="state">the initialization state to check</param>
	/// <returns>Returns true if initialization needs to be done, false otherwise</returns>
	/// <remarks>
	/// This function checks the passed in state and if initialization should be done, sets the status to <see href="https://wiki.libsdl.org/SDL3/SDL_INIT_STATUS_INITIALIZING"><c>SDL_INIT_STATUS_INITIALIZING</c></see> and returns true.
	/// If another thread is already modifying this state, it will wait until that's done before returning.
	/// 
	/// If this function returns true, the calling code must call <see href="https://wiki.libsdl.org/SDL3/SDL_SetInitialized">SDL_SetInitialized</see>() to complete the initialization
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_ShouldInit">SDL_ShouldInit</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl), typeof(CallConvSuppressGCTransition)])]
	internal unsafe static partial CBool SDL_ShouldInit(InitState* state);

	/// <summary>
	/// Return whether cleanup should be done
	/// </summary>
	/// <param name="state">the initialization state to check</param>
	/// <returns>Returns true if cleanup needs to be done, false otherwise</returns>
	/// <remarks>
	/// This function checks the passed in state and if cleanup should be done, sets the status to <see href="https://wiki.libsdl.org/SDL3/SDL_INIT_STATUS_UNINITIALIZING"><c>SDL_INIT_STATUS_UNINITIALIZING</c></see> and returns true.
	///
	/// If this function returns true, the calling code must call <see href="https://wiki.libsdl.org/SDL3/SDL_SetInitialized">SDL_SetInitialized</see>() to complete the cleanup.
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_ShouldQuit">SDL_ShouldQuit</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl), typeof(CallConvSuppressGCTransition)])]
	internal unsafe static partial CBool SDL_ShouldQuit(InitState* state);
}
*/