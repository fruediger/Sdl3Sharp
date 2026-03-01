namespace Sdl3Sharp.IO;

partial class MemoryStream
{
	/// <summary>
	/// Provides property names for <see cref="MemoryStream"/> <see cref="Stream.Properties">properties</see>
	/// </summary>
	public new sealed class PropertyNames : Stream.PropertyNames
	{
		/// <summary>
		/// The name of a <see cref="Stream.Properties">property</see> that holds the pointer to the memory buffer that the <see cref="MemoryStream"/> was initialized with
		/// </summary>
		/// <seealso cref="Memory"/>
		public const string MemoryPointer = "SDL_PROP_IOSTREAM_MEMORY_POINTER";

		/// <summary>
		/// The name of a <see cref="Stream.Properties">property</see> that holds the size, in bytes, of the memory buffer that the <see cref="MemoryStream"/> was initialized with
		/// </summary>
		/// <seealso cref="Size"/>
		public const string SizeNumber = "SDL_PROP_IOSTREAM_MEMORY_SIZE_NUMBER";

		/// <summary>
		/// The name of a <see cref="Stream.Properties">property</see> that holds the pointer to a function that will be called to free the memory buffer when the <see cref="MemoryStream"/> is <see cref="Stream.TryClose">closed</see> or <see cref="Stream.Dispose()">disposed</see>
		/// </summary>
		/// <remarks>
		/// <para>
		/// If the value of the property is unset or <c><see langword="null"/></c>, the unmanaged memory will not be freed when the <see cref="MemoryStream"/> is <see cref="Stream.TryClose">closed</see> or <see cref="Stream.Dispose()">disposed</see>.
		/// Changing the value of the property will also change the automatic freeing behavior.
		/// </para>
		/// </remarks>
		/// <seealso cref="FreeFunc"/>
		public const string FreeFuncPointer = "FREE_FUNC_POINTER";

		private PropertyNames() { }
	}
}
