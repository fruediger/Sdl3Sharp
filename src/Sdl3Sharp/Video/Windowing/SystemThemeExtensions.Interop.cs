using Sdl3Sharp.Internal.Interop;
using Sdl3Sharp.SourceGeneration;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Video.Windowing;

partial class SystemThemeExtensions
{
	/// <summary>
	/// Get the current system theme
	/// </summary>
	/// <returns>Returns the current system theme, light, dark, or unknown</returns>
	/// <remarks>
	/// <para>
	/// This function should only be called on the main thread.
	/// </para>
	/// </remarks>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal static partial SystemTheme SDL_GetSystemTheme();
}
