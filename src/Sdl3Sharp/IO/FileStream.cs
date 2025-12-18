using Sdl3Sharp.Utilities;
using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.Marshalling;

namespace Sdl3Sharp.IO;

/// <summary>
/// A stream that is backed by a named file on the filesystem
/// </summary>
public sealed partial class FileStream : Stream
{
	private interface IUnsafeConstructorDispatch;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private unsafe static byte* Utf8Convert(string? str, out byte* strUtf8) => strUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(str);

	/// <exception cref="ArgumentException">The combination of <paramref name="access"/> and <paramref name="mode"/> is invalid</exception>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private static string ValidateMode(FileAccess access, FileMode mode)
	{
		if (!TryGetModeString(access, mode, out var modeString))
		{
			failArgumentsInvalid();
		}

		return modeString;

		[DoesNotReturn]
		static void failArgumentsInvalid() => throw new ArgumentException($"Invalid combination of values for the {nameof(access)} and {nameof(mode)} arguments");
	}

	/// <exception cref="ArgumentException">The combination of <paramref name="access"/>, <paramref name="mode"/>, and <paramref name="kind"/> is invalid</exception>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private static string ValidateMode(FileAccess access, FileMode mode, FileKind kind)
	{
		if (!TryGetModeString(access, mode, kind, out var modeString))
		{
			failArgumentsInvalid();
		}

		return modeString;

		[DoesNotReturn]
		static void failArgumentsInvalid() => throw new ArgumentException($"Invalid combination of values for the {nameof(access)}, {nameof(mode)}, and {nameof(kind)} arguments");
	}

	private readonly string mFileName;
	private readonly string mModeString;

	/// <exception cref="SdlException">The <see cref="FileStream"/> could not be created (check <see cref="Error.TryGet(out string?)"/> for more information)</exception>
	private unsafe FileStream(string fileName, string modeString, IUnsafeConstructorDispatch? _ = default) :
		base(SDL_IOFromFile(Utf8Convert(fileName, out var fileNameUtf8), Utf8Convert(modeString, out var modeStringUtf8))) // base(SDL_IOStream*) does neither throw nor fail
	{
		try
		{
			if (Pointer is null)
			{
				failCouldNotCreateFileStream();
			}

			// We store them just for debugging purposes and for SdlToClrStream to identify if the stream is readable or not 
			mFileName = fileName;
			mModeString = modeString;
		}
		finally
		{
			Utf8StringMarshaller.Free(modeStringUtf8);
			Utf8StringMarshaller.Free(fileNameUtf8);
		}

		[DoesNotReturn]
		static void failCouldNotCreateFileStream() => throw new SdlException($"Could not create the {nameof(FileStream)}");
	}

	/// <summary>
	/// Creates a new <see cref="FileStream"/> for a specified file name and mode string
	/// </summary>
	/// <param name="fileName">The name of the file to open</param>
	/// <param name="modeString">The mode string to use when opening the file</param>
	/// <remarks>
	/// <para>
	/// The mode strings used by this method are roughly the same as the ones used by the C standard library's <c>fopen</c> function.
	/// You can use <see cref="TryGetModeString(FileAccess, FileMode, out string?)"/> or <see cref="TryGetModeString(FileAccess, FileMode, FileKind, out string?)"/> to construct valid mode strings.
	/// </para>
	/// <para>
	/// On Android, this can be used to open <c>"content://"</c> URIs. As a fallback, this will transparently open a matching <paramref name="fileName"/> in the app's assets.
	/// </para>
	/// </remarks>
	/// <inheritdoc cref="FileStream(string, string, IUnsafeConstructorDispatch?)"/>
	public FileStream(string fileName, string modeString) :
#pragma warning disable IDE0034 // Keep it that way for explicitness sake
		this(fileName, modeString, default(IUnsafeConstructorDispatch?))		
#pragma warning restore IDE0034
	{ }

	/// <summary>
	/// Creates a new <see cref="FileStream"/> for a specified file name, <see cref="FileAccess">access</see>, and <see cref="FileMode">mode</see>
	/// </summary>
	/// <param name="fileName">The name of the file to open</param>
	/// <param name="access">The <see cref="FileAccess"/> value representing the access mode</param>
	/// <param name="mode">The <see cref="FileMode"/> value representing the file mode</param>
	/// <remarks>
	/// <para>
	/// On Android, this can be used to open <c>"content://"</c> URIs. As a fallback, this will transparently open a matching <paramref name="fileName"/> in the app's assets.
	/// </para>
	/// </remarks>
	/// <inheritdoc cref="FileStream(string, string, IUnsafeConstructorDispatch?)"/>
	/// <inheritdoc cref="ValidateMode(FileAccess, FileMode)"/>
	public FileStream(string fileName, FileAccess access, FileMode mode) : this(fileName, ValidateMode(access, mode))
	{ }

	/// <summary>
	/// Creates a new <see cref="FileStream"/> for a specified file name, <see cref="FileAccess">access</see>, <see cref="FileMode">mode</see>, and <see cref="FileKind">kind</see>
	/// </summary>
	/// <param name="fileName">The name of the file to open</param>
	/// <param name="access">The <see cref="FileAccess"/> value representing the access mode</param>
	/// <param name="mode">The <see cref="FileMode"/> value representing the file mode</param>
	/// <param name="kind">The <see cref="FileKind"/> value representing the file kind</param>
	/// <remarks>
	/// <para>
	/// On Android, this can be used to open <c>"content://"</c> URIs. As a fallback, this will transparently open a matching <paramref name="fileName"/> in the app's assets.
	/// </para>
	/// </remarks>
	/// <inheritdoc cref="FileStream(string, string, IUnsafeConstructorDispatch?)"/>
	/// <inheritdoc cref="ValidateMode(FileAccess, FileMode, FileKind)"/>
	public FileStream(string fileName, FileAccess access, FileMode mode, FileKind kind) : this(fileName, ValidateMode(access, mode, kind))
	{ }

	/// <summary>Calls to this property are not supported</summary>
	/// <value>Not supported</value>
	/// <exception cref="NotSupportedException">always</exception>
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("Calls to this property are not supported. This property will always throw an exception. Use the Length property instead.")]
#pragma warning disable CS0809
	protected sealed override long LengthCore
#pragma warning restore CS0809
	{
		[DoesNotReturn]
		get => throw new NotSupportedException("Calls to this property are not supported.");
	}

	/// <summary>Calls to this method are not supported</summary>
	/// <param name="offset">Not supported</param>
	/// <param name="whence">Not supported</param>
	/// <returns>Not supported</returns>
	/// <exception cref="NotSupportedException">always</exception>	
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("Calls to this method are not supported. This method will always throw an exception. Use the TrySeek(long, StreamWhence, out long) method instead.")]
	[DoesNotReturn]
#pragma warning disable CS0809
	protected sealed override long SeekCore(long offset, StreamWhence whence) => throw new NotSupportedException("Calls to this method are not supported.");
#pragma warning restore CS0809

	/// <summary>Calls to this method are not supported</summary>
	/// <param name="data">Not supported</param>
	/// <param name="status">Not supported</param>
	/// <returns>Not supported</returns>
	/// <exception cref="NotSupportedException">always</exception>	
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("Calls to this method are not supported. This method will always throw an exception. Use the TryRead(NativeMemory, out nuint) method instead.")]
	[DoesNotReturn]
#pragma warning disable CS0809
	protected sealed override nuint ReadCore(NativeMemory data, ref StreamStatus status) => throw new NotSupportedException("Calls to this method are not supported.");
#pragma warning restore CS0809

	/// <summary>Calls to this method are not supported</summary>
	/// <param name="data">Not supported</param>
	/// <param name="status">Not supported</param>
	/// <returns>Not supported</returns>
	/// <exception cref="NotSupportedException">always</exception>	
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("Calls to this method are not supported. This method will always throw an exception. Use the TryWrite(ReadOnlyNativeMemory, out nuint) method instead.")]
	[DoesNotReturn]
#pragma warning disable CS0809
	protected sealed override nuint WriteCore(ReadOnlyNativeMemory data, ref StreamStatus status) => throw new NotSupportedException("Calls to this method are not supported.");
#pragma warning restore CS0809

	/// <summary>Calls to this method are not supported</summary>
	/// <param name="status">Not supported</param>
	/// <returns>Not supported</returns>
	/// <exception cref="NotSupportedException">always</exception>	
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("Calls to this method are not supported. This method will always throw an exception. Use the TryFlush() method instead.")]
	[DoesNotReturn]
#pragma warning disable CS0809
	protected sealed override bool FlushCore(ref StreamStatus status) => throw new NotSupportedException("Calls to this method are not supported.");
#pragma warning restore CS0809

	/// <summary>Calls to this method are not supported</summary>
	/// <returns>Not supported</returns>
	/// <exception cref="NotSupportedException">always</exception>	
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("Calls to this method are not supported. This method will always throw an exception. Use the TryClose() method instead.")]
	[DoesNotReturn]
#pragma warning disable CS0809
	protected sealed override bool CloseCore() => throw new NotSupportedException("Calls to this method are not supported.");
#pragma warning restore CS0809

	/// <summary>
	/// Gets the pointer to the Android NDK <c>AAsset</c> that the <see cref="FileStream"/> is using to access the filesystem
	/// </summary>
	/// <value>
	/// The pointer to the Android NDK <c>AAsset</c> that the <see cref="FileStream"/> is using to access the filesystem. This can be cast to an Android NDK <c>AAsset*</c>.
	/// </value>
	/// <remarks>
	/// <para>
	/// If SDL uses some other method to access the filesystem, this property will not be set and returns <c><see langword="default"/>(<see cref="IntPtr"/>)</c>.
	/// </para>
	/// </remarks>
	/// <seealso cref="PropertyNames.AndroidAAssetPointer"/>
	public IntPtr AndroidAAsset
		=> Properties?.TryGetPointerValue(PropertyNames.AndroidAAssetPointer, out var androidAAsset) is true
			? androidAAsset
			: default;

	/// <summary>
	/// Gets the file descriptor number that the <see cref="FileStream"/> is using to access the filesystem
	/// </summary>
	/// <value>
	/// The file descriptor number that the <see cref="FileStream"/> is using to access the filesystem. This is a C file descriptor number.
	/// </value>
	/// <seealso cref="PropertyNames.FileDescriptorNumber"/>
	public int FileDescriptor
		=> Properties?.TryGetNumberValue(PropertyNames.FileDescriptorNumber, out var fileDescriptor) is true
			? unchecked((int)fileDescriptor) // POSIX file descriptors should always fit into 'int' (they are 'int' by definition)
			: -1;

	internal string FileName { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mFileName; }

	internal string ModeString { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mModeString; }

	/// <summary>
	/// Gets the pointer to the C standard library <c>FILE</c> that the <see cref="FileStream"/> is using to access the filesystem
	/// </summary>
	/// <value>
	/// The pointer to the C standard library <c>FILE</c> that the <see cref="FileStream"/> is using to access the filesystem. This can be cast to a C standard library <c>FILE*</c>.
	/// </value>
	/// <remarks>
	/// <para>
	/// If SDL uses some other method to access the filesystem, this property will not be set and returns <c><see langword="default"/>(<see cref="IntPtr"/>)</c>.
	/// </para>
	/// <para>
	/// <em>NOTE</em>: The value of this property is highly dependent on the C standard library and the compiler the underlying native SDL library was built with.
	/// Using that value without knowing these settings or using it on differing platforms may lead to at least undefined behavior or even result in a crash!
	/// Do not rely on the value of this property unless you really know what you are doing.
	/// </para>
	/// </remarks>
	/// <seealso cref="PropertyNames.StdioFilePointer"/>
	public IntPtr StdioFile
		=> Properties?.TryGetPointerValue(PropertyNames.StdioFilePointer, out var stdioFile) is true
			? stdioFile
			: default;

	/// <summary>
	/// Gets the Windows <c>HANDLE</c> that the <see cref="FileStream"/> is using to access the filesystem
	/// </summary>
	/// <value>
	/// The Windows <c>HANDLE</c> that the <see cref="FileStream"/> is using to access the filesystem. This can be cast to a Windows <c>HANDLE</c>.
	/// </value>
	/// <remarks>
	/// <para>
	/// If SDL uses some other method to access the filesystem, this property will not be set and returns <c><see langword="default"/>(<see cref="IntPtr"/>)</c>.
	/// </para>
	/// </remarks>
	/// <see cref="PropertyNames.WindowsHandlePointer"/>
	public IntPtr WindowsHandle
		=> Properties?.TryGetPointerValue(PropertyNames.WindowsHandlePointer, out var windowsHandle) is true
			? windowsHandle
			: default;

	/// <summary>
	/// Tries to get the mode string for the specified <see cref="FileAccess">access</see> and <see cref="FileMode">mode</see>
	/// </summary>
	/// <param name="access">The <see cref="FileAccess"/> value representing the access mode</param>
	/// <param name="mode">The <see cref="FileMode"/> value representing the file mode</param>
	/// <param name="modeString">The resulting mode string, when this method returns <c><see langword="true"/></c>; otherwise, <c><see langword="null"/></c></param>
	/// <returns><c><see langword="true"/></c>, if the mode string was successfully retrieved; otherwise, <c><see langword="false"/></c></returns>
	/// <remarks>
	/// <para>
	/// A mode string constructed by this method is roughly the same as the ones used by the C standard library's <c>fopen</c> function.
	/// </para>
	/// </remarks>
	public static bool TryGetModeString(FileAccess access, FileMode mode, [NotNullWhen(true)] out string? modeString)
		=> (access, mode) switch
		{
			(FileAccess.None,      _)                  => modeString = "",
			(FileAccess.Read,      FileMode.Open)      => modeString = "r",
			(FileAccess.Write,     FileMode.Create)    => modeString = "w",			
			(FileAccess.Write,     FileMode.CreateNew) => modeString = "wx",
			(FileAccess.Write,     FileMode.Append)    => modeString = "a",
			(FileAccess.ReadWrite, FileMode.Open)      => modeString = "r+",
			(FileAccess.ReadWrite, FileMode.Create)    => modeString = "w+",
			(FileAccess.ReadWrite, FileMode.CreateNew) => modeString = "w+x",
			(FileAccess.ReadWrite, FileMode.Append)    => modeString = "a+",
			_ => modeString = null
		} is not null;

	/// <summary>
	/// Tries to get the mode string for the specified <see cref="FileAccess">access</see>, <see cref="FileMode">mode</see>, and <see cref="FileKind">kind</see>
	/// </summary>
	/// <param name="access">The <see cref="FileAccess"/> value representing the access mode</param>
	/// <param name="mode">The <see cref="FileMode"/> value representing the file mode</param>
	/// <param name="kind">The <see cref="FileKind"/> value representing the file kind</param>
	/// <param name="modeString">The resulting mode string, when this method returns <c><see langword="true"/></c>; otherwise, <c><see langword="null"/></c></param>
	/// <returns><c><see langword="true"/></c>, if the mode string was successfully retrieved; otherwise, <c><see langword="false"/></c></returns>
	/// <remarks>
	/// <para>
	/// A mode string constructed by this method is roughly the same as the ones used by the C standard library's <c>fopen</c> function.
	/// </para>
	/// </remarks>
	public static bool TryGetModeString(FileAccess access, FileMode mode, FileKind kind, [NotNullWhen(true)] out string? modeString)
	{
		if (!TryGetModeString(access, mode, out modeString))
		{
			return false;
		}

		if (kind is FileKind.Binary)
		{
			modeString += "b";
		}
		else if (kind is FileKind.Text)
		{
			modeString += "t";
		}

		return true;
	}
}