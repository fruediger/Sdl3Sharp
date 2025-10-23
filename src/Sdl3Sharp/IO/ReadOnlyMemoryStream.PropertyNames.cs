namespace Sdl3Sharp.IO;

partial class ReadOnlyMemoryStream
{
	/// <summary>
	/// Provides property names for <see cref="ReadOnlyMemoryStream"/> <see cref="Stream.Properties">properties</see>
	/// </summary>
	public static class PropertyNames
	{
		/// <summary>
		/// The name of a <see cref="Stream.Properties">property</see> that holds the pointer to the memory buffer that the <see cref="ReadOnlyMemoryStream"/> was initialized with
		/// </summary>
		/// <seealso cref="Memory"/>
		public const string MemoryPointer = MemoryStream.PropertyNames.MemoryPointer;

		/// <summary>
		/// The name of a <see cref="Stream.Properties">property</see> that holds the size, in bytes, of the memory buffer that the <see cref="ReadOnlyMemoryStream"/> was initialized with
		/// </summary>
		/// <seealso cref="Size"/>
		public const string SizeNumber = MemoryStream.PropertyNames.SizeNumber;

		/// <summary>
		/// The name of a <see cref="Stream.Properties">property</see> that holds the pointer to a function that will be called to free the memory buffer when the <see cref="ReadOnlyMemoryStream"/> is <see cref="Stream.TryClose">closed</see> or <see cref="Stream.Dispose()">disposed</see>
		/// </summary>
		/// <remarks>
		/// <para>
		/// If the value of the property is unset or <c><see langword="null"/></c>, the unmanaged memory will not be freed when the <see cref="ReadOnlyMemoryStream"/> is <see cref="Stream.TryClose">closed</see> or <see cref="Stream.Dispose()">disposed</see>.
		/// Changing the value of the property will also change the automatic freeing behavior.
		/// </para>
		/// </remarks>
		/// <seealso cref="FreeFunc"/>
		public const string FreeFuncPointer = MemoryStream.PropertyNames.FreeFuncPointer;
	}
}
