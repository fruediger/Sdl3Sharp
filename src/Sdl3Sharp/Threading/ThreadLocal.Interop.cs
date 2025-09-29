using Sdl3Sharp.Internal.Interop;
using Sdl3Sharp.SourceGeneration;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using unsafe SDL_TLSDestructorCallback = delegate* unmanaged[Cdecl]<void*, void>;

namespace Sdl3Sharp.Threading;

partial class ThreadLocal
{
	[UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static void TLSDestructorCallback(void* value)
	{
		if (value is not null && GCHandle.FromIntPtr((IntPtr)value) is { IsAllocated: true, Target: Box box } gcHandle)
		{
			box.Reset();

			gcHandle.Free();
		}
	}

	/// <summary>
	/// Cleanup all TLS data for this thread
	/// </summary>
	/// <remarks>
	/// If you are creating your threads outside of SDL and then calling SDL functions, you should call this function before your thread exits, to properly clean up SDL memory
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_CleanupTLS">SDL_CleanupTLS</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal static partial void SDL_CleanupTLS();

	/// <summary>
	/// Get the current thread's value associated with a thread local storage ID
	/// </summary>
	/// <param name="id">a pointer to the thread local storage ID, may not be NULL</param>
	/// <returns>Returns the value associated with the ID for the current thread or NULL if no value has been set; call SDL_GetError() for more information</returns>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_GetTLS">SDL_GetTLS</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial void* SDL_GetTLS(AtomicInt32* id);

	/// <summary>
	/// Set the current thread's value associated with a thread local storage ID
	/// </summary>
	/// <param name="id">a pointer to the thread local storage ID, may not be NULL</param>
	/// <param name="value">the value to associate with the ID for the current thread</param>
	/// <param name="destructor">a function called when the thread exits, to free the value, may be NULL</param>
	/// <returns>Returns true on success or false on failure; call SDL_GetError() for more information</returns>
	/// <remarks>
	/// If the thread local storage ID is not initialized (the value is 0), a new ID will be created in a thread-safe way, so all calls using a pointer to the same ID will refer to the same local storage.
	///
	/// Note that replacing a value from a previous call to this function on the same thread does <em>not</em> call the previous value's destructor!
	///
	/// <c>destructor</c> can be NULL; it is assumed that <c>value</c> does not need to be cleaned up if so.
	/// </remarks>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_SetTLS(AtomicInt32* id, void* value, SDL_TLSDestructorCallback destructor);
}
