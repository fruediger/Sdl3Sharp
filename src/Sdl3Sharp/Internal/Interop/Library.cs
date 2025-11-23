using Sdl3Sharp.SourceGeneration;
using System.Runtime.InteropServices;

namespace Sdl3Sharp.Internal.Interop;

internal sealed class Library : INativeImportLibrary
{
	static (string? libraryName, DllImportSearchPath? searchPath) INativeImportLibrary.GetLibraryNameAndSearchPath() => (
		"SDL3", // remember that some platforms treat this as case-sensitive!
		DllImportSearchPath.AssemblyDirectory | DllImportSearchPath.UseDllDirectoryForDependencies | DllImportSearchPath.ApplicationDirectory | DllImportSearchPath.UserDirectories
	);

	static void INativeImportLibrary.AfterSuccessfullyLoaded(string libraryName, DllImportSearchPath? searchPath)
	{
		unsafe
		{
			// immediately fix the SDL logging system to use the managed bridge, so we don't miss any logs:
			Log.SetSDLOutputCallback();

			// try to use SDL's allocators for ffi too
			Ffi.Ffi.Allocator.Alloc = &NativeMemory.Alloc;
			Ffi.Ffi.Allocator.Free = &NativeMemory.Free;
		}
	}
}
