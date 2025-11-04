using Sdl3Sharp.Internal.Interop;
using Sdl3Sharp.SourceGeneration;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.IO;

partial class ReadOnlyMemoryStream
{
	/// <summary>
	/// Use this function to prepare a read-only memory buffer for use with <see href="https://wiki.libsdl.org/SDL3/SDL_IOStream">SDL_IOStream</see>
	/// </summary>
	/// <param name="mem">A pointer to a read-only buffer to feed an <see href="https://wiki.libsdl.org/SDL3/SDL_IOStream">SDL_IOStream</see> stream</param>
	/// <param name="size">The buffer size, in bytes.</param>
	/// <returns>Returns a pointer to a new <see href="https://wiki.libsdl.org/SDL3/SDL_IOStream">SDL_IOStream</see> structure or NULL on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// This function sets up an <see href="https://wiki.libsdl.org/SDL3/SDL_IOStream">SDL_IOStream</see> struct based on a memory area of a certain size.
	/// It assumes the memory area is not writable.
	/// </para>
	/// <para>
	/// Attempting to write to this <see href="https://wiki.libsdl.org/SDL3/SDL_IOStream">SDL_IOStream</see> stream will report an error without writing to the memory buffer.
	/// </para>
	/// <para>
	/// This memory buffer is not copied by the <see href="https://wiki.libsdl.org/SDL3/SDL_IOStream">SDL_IOStream</see>; the pointer you provide must remain valid until you close the stream.
	/// </para>
	/// <para>
	/// If you need to write to a memory buffer, you should use <see href="https://wiki.libsdl.org/SDL3/SDL_IOFromMem">SDL_IOFromMem</see>() with a writable buffer of memory instead.
	/// </para>
	/// <para>
	/// The following properties will be set at creation time by SDL:
	/// <list type="bullet">
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_IOSTREAM_MEMORY_POINTER"><c>SDL_PROP_IOSTREAM_MEMORY_POINTER</c></see></term>
	///			<description>This will be the <c><paramref name="mem"/></c> parameter that was passed to this function</description>
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_IOSTREAM_MEMORY_SIZE_NUMBER"><c>SDL_PROP_IOSTREAM_MEMORY_SIZE_NUMBER</c></see></term>
	///			<description>This will be the <c><paramref name="size"/></c> parameter that was passed to this function</description>
	///		</item>
	/// </list>
	/// Additionally, the following properties are recognized:
	/// <list type="bullet">
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_IOSTREAM_MEMORY_FREE_FUNC_POINTER"><c>SDL_PROP_IOSTREAM_MEMORY_FREE_FUNC_POINTER</c></see></term>
	///			<description>
	///				If this property is set to a non-NULL value it will be interpreted as a function of <see href="https://wiki.libsdl.org/SDL3/SDL_free_func">SDL_free_func</see> type and called with the passed <c><paramref name="mem"/></c> pointer when closing the stream.
	///				By default it is unset, i.e., the memory will not be freed.
	///			</description>
	///		</item>
	/// </list>
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_IOFromConstMem">SDL_IOFromConstMem</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial SDL_IOStream* SDL_IOFromConstMem(void* mem, nuint size);
}
