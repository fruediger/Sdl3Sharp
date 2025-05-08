using Sdl3Sharp.SourceGeneration;
using System.Runtime.InteropServices;

namespace Sdl3Sharp.Interop;

internal sealed class Library : INativeImportLibrary
{
	static (string? libraryName, DllImportSearchPath? searchPath) INativeImportLibrary.GetLibraryNameAndSearchPath() => (
		"sdl3",
		DllImportSearchPath.AssemblyDirectory | DllImportSearchPath.UseDllDirectoryForDependencies | DllImportSearchPath.ApplicationDirectory | DllImportSearchPath.UserDirectories
	);

	static void INativeImportLibrary.AfterSuccessfullyLoaded(string libraryName, DllImportSearchPath? searchPath)
	{
		// immediately fix the SDL logging system to use the managed bridge, so we don't miss any logs:
		Log.SetSDLOutputCallback();
	}
}
