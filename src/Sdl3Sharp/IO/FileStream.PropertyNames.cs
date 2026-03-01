namespace Sdl3Sharp.IO;

partial class FileStream
{
	/// <summary>
	/// Provides property names for <see cref="FileStream"/> <see cref="Stream.Properties">properties</see>
	/// </summary>
	public new sealed class PropertyNames : Stream.PropertyNames
	{
		/// <summary>
		/// The name of a <see cref="Properties">property</see> that holds the pointer to the Android NDK <c>AAsset</c> that the <see cref="FileStream"/> is using to access the filesystem
		/// </summary>
		/// <remarks>
		/// <para>
		/// The value of the property can be cast to an Android NDK <c>AAsset*</c>.
		/// </para>
		/// <para>
		/// If SDL uses some other method to access the filesystem, the property will not be set.
		/// </para>
		/// </remarks>
		/// <seealso cref="AndroidAAsset"/>
		public const string AndroidAAssetPointer = "SDL_PROP_IOSTREAM_ANDROID_AASSET_POINTER";

		/// <summary>
		/// The name of a <see cref="Properties">property</see> that holds the file descriptor number that the <see cref="FileStream"/> is using to access the filesystem
		/// </summary>
		/// <remarks>
		/// <para>
		/// The value of the property is a C file descriptor number.
		/// </para>
		/// </remarks>
		/// <see cref="FileDescriptor"/>
		public const string FileDescriptorNumber = "SDL_PROP_IOSTREAM_FILE_DESCRIPTOR_NUMBER";

		/// <summary>
		/// The name of a <see cref="Properties">property</see> that holds the pointer to the C standard library <c>FILE</c> that the <see cref="FileStream"/> is using to access the filesystem
		/// </summary>
		/// <remarks>
		/// <para>
		/// The value of the property can be cast to a C standard library <c>FILE*</c>.
		/// </para>
		/// <para>
		/// If SDL uses some other method to access the filesystem, the property will not be set.
		/// </para>
		/// <para>
		/// <em>NOTE</em>: The value of the property is highly dependent on the C standard library and the compiler the underlying native SDL library was built with.
		/// Using that value without knowing these settings or using it on differing platforms may lead to at least undefined behavior or even result in a crash!
		/// Do not rely on the value of that property unless you really know what you are doing.
		/// </para>
		/// </remarks>
		/// <see cref="StdioFile"/>
		public const string StdioFilePointer = "SDL_PROP_IOSTREAM_STDIO_FILE_POINTER";

		/// <summary>
		/// The name of a <see cref="Properties">property</see> that holds the Windows <c>HANDLE</c> that the <see cref="FileStream"/> is using to access the filesystem
		/// </summary>
		/// <remarks>
		/// <para>
		/// The value of the property can be cast to a Windows <c>HANDLE</c>.
		/// </para>
		/// <para>
		/// If SDL uses some other method to access the filesystem, the property will not be set.
		/// </para>
		/// </remarks>
		/// <seealso cref="WindowsHandle"/>
		public const string WindowsHandlePointer = "SDL_PROP_IOSTREAM_WINDOWS_HANDLE_POINTER";

		private PropertyNames() { }
	}
}
