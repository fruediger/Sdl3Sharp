namespace Sdl3Sharp.IO;

partial class DynamicMemoryStream
{
	/// <summary>
	/// Provides property names for <see cref="DynamicMemoryStream"/> <see cref="Stream.Properties">properties</see>
	/// </summary>
	public static class PropertyNames
	{
		/// <summary>
		/// The name of a <see cref="Properties">property</see> that holds the pointer to the internal memory of the stream
		/// </summary>
		/// <remarks>
		/// <para>
		/// The property can be set to <c><see cref="System.IntPtr.Zero"/></c> (<c><see langword="null"/></c>) to transfer ownership of the memory to the application, which should free the memory with <see cref="Utilities.NativeMemory.Free(void*)"/>.
		/// If this is done, the next operation on the stream must be <see cref="Stream.TryClose"/> or better <see cref="Stream.Dispose()"/>.
		/// </para>
		/// <para>
		/// For a safer way to transfer ownership of the memory, use <see cref="TryGetMemoryManagerAndDispose(out Utilities.NativeMemoryManager?)"/> instead.
		/// </para>
		/// </remarks>
		/// <seealso cref="Memory"/>
		public const string MemoryPointer = "SDL_PROP_IOSTREAM_DYNAMIC_MEMORY_POINTER";

		/// <summary>
		/// The name of a <see cref="Properties">property</see> that holds the chunk size used for memory allocations
		/// </summary>
		/// <remarks>
		/// <para>
		/// Memory will be allocated in multiples of this size, defaulting to <c>1024</c>.
		/// </para>
		/// </remarks>
		/// <seealso cref="ChunkSize"/>
		public const string ChunkSizeNumber = "SDL_PROP_IOSTREAM_DYNAMIC_CHUNKSIZE_NUMBER";
	}
}
