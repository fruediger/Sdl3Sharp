using Microsoft.VisualBasic;
using Sdl3Sharp.Internal.Interop;
using Sdl3Sharp.SourceGeneration;
using System;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.IO;

partial class DynamicMemoryStream
{
	/// <summary>
	/// Use this function to create an <see href="https://wiki.libsdl.org/SDL3/SDL_IOStream">SDL_IOStream</see> that is backed by dynamically allocated memory
	/// </summary>
	/// <returns>Returns a pointer to a new <see href="https://wiki.libsdl.org/SDL3/SDL_IOStream">SDL_IOStream</see> structure or NULL on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// This supports the following properties to provide access to the memory and control over allocations:
	/// <list type="bullet">
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_IOSTREAM_DYNAMIC_MEMORY_POINTER"><c>SDL_PROP_IOSTREAM_DYNAMIC_MEMORY_POINTER</c></see></term>
	///			<description>
	///				A pointer to the internal memory of the stream.
	///				This can be set to NULL to transfer ownership of the memory to the application, which should free the memory with <see href="https://wiki.libsdl.org/SDL3/SDL_free">SDL_free</see>().
	///				If this is done, the next operation on the stream must be <see href="https://wiki.libsdl.org/SDL3/SDL_CloseIO">SDL_CloseIO</see>().
	///			</description>
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_IOSTREAM_DYNAMIC_CHUNKSIZE_NUMBER"><c>SDL_PROP_IOSTREAM_DYNAMIC_CHUNKSIZE_NUMBER</c></see></term>
	///			<description>Memory will be allocated in multiples of this size, defaulting to 1024.</description>
	///		</item>
	/// </list>
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_IOFromDynamicMem">SDL_IOFromDynamicMem</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial SDL_IOStream* SDL_IOFromDynamicMem();
}
