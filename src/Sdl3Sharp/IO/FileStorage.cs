using Sdl3Sharp.Utilities;
using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.Marshalling;

namespace Sdl3Sharp.IO;

/// <summary>
/// A storage container for the local file system
/// </summary>
/// <remarks>
/// <para>
/// This type is primarily provided for development.
/// Portable applications should use <see cref="TitleStorage"/> for access to application data and <see cref="UserStorage"/> for access to user data instead.
/// </para>
/// </remarks>
public sealed partial class FileStorage : Storage
{
	private interface IUnsafeConstructorDispatch;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private unsafe static byte* Utf8Convert(string? str, out byte* strUtf8) => strUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(str);

	/// <exception cref="SdlException">The <see cref="FileStorage"/> could not be opened (check <see cref="Error.TryGet(out string?)"/> for more information)</exception>
	private unsafe FileStorage(string? path, IUnsafeConstructorDispatch? _ = default) :
		base(SDL_OpenFileStorage(Utf8Convert(path, out var pathUtf8)))
	{
		try
		{
			if (Pointer is null)
			{
				failCouldNotCreateFileStorage();
			}
		}
		finally
		{
			Utf8StringMarshaller.Free(pathUtf8);
		}

		[DoesNotReturn]
		static void failCouldNotCreateFileStorage() => throw new SdlException($"Could not open the {nameof(FileStorage)}");
	}

	/// <summary>
	/// Creates a new <see cref="FileStorage"/> for a specified base path
	/// </summary>
	/// <param name="path">The base path to be prepended to all storage paths, or <c><see langword="null"/></c> for no base path</param>
	/// <inheritdoc cref="FileStorage(string?, IUnsafeConstructorDispatch?)"/>
	public FileStorage(string? path) :
#pragma warning disable IDE0034 // Keep it that way for explicitness sake
		this(path, default(IUnsafeConstructorDispatch?))
#pragma warning restore IDE0034
	{ }

	/// <summary>Calls to this property are not supported</summary>
	/// <value>Not supported</value>
	/// <exception cref="NotSupportedException">always</exception>
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("Calls to this property are not supported. This property will always throw an exception. Use the IsReady property instead.")]
#pragma warning disable CS0809
	protected override bool IsReadyCore
#pragma warning restore CS0809
	{
		[DoesNotReturn]
		get => throw new NotSupportedException("Calls to this property are not supported.");
	}

	/// <summary>Calls to this property are not supported</summary>
	/// <value>Not supported</value>
	/// <exception cref="NotSupportedException">always</exception>
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("Calls to this property are not supported. This property will always throw an exception. Use the RemainingSpace property instead.")]
#pragma warning disable CS0809
	protected override ulong RemainingSpaceCore
#pragma warning restore CS0809
	{
		[DoesNotReturn]
		get => throw new NotSupportedException("Calls to this property are not supported.");
	}

	/// <summary>Calls to this method are not supported</summary>
	/// <returns>Not supported</returns>
	/// <exception cref="NotSupportedException">always</exception>	
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("Calls to this method are not supported. This method will always throw an exception. Use the TryClose() method instead.")]
	[DoesNotReturn]
#pragma warning disable CS0809
	protected override bool CloseCore() => throw new NotSupportedException("Calls to this method are not supported.");
#pragma warning restore CS0809

	/// <summary>Calls to this method are not supported</summary>
	/// <param name="oldPath">Not supported</param>
	/// <param name="newPath">Not supported</param>
	/// <returns>Not supported</returns>
	/// <exception cref="NotSupportedException">always</exception>	
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("Calls to this method are not supported. This method will always throw an exception. Use the TryCopyFile(string, string) method instead.")]
	[DoesNotReturn]
#pragma warning disable CS0809
	protected override bool CopyFileCore(string oldPath, string newPath) => throw new NotSupportedException("Calls to this method are not supported.");
#pragma warning restore CS0809

	/// <summary>Calls to this method are not supported</summary>
	/// <param name="path">Not supported</param>
	/// <returns>Not supported</returns>
	/// <exception cref="NotSupportedException">always</exception>	
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("Calls to this method are not supported. This method will always throw an exception. Use the TryCreateDirectory(string) method instead.")]
	[DoesNotReturn]
#pragma warning disable CS0809
	protected override bool CreateDirectoryCore(string path) => throw new NotSupportedException("Calls to this method are not supported.");
#pragma warning restore CS0809

	/// <summary>Calls to this method are not supported</summary>
	/// <param name="path">Not supported</param>
	/// <param name="callback">Not supported</param>
	/// <returns>Not supported</returns>
	/// <exception cref="NotSupportedException">always</exception>	
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("Calls to this method are not supported. This method will always throw an exception. Use the TryEnumerateDirectory(string?, EnumerateDirectoryCallback) method instead.")]
	[DoesNotReturn]
#pragma warning disable CS0809
	protected override bool EnumerateDirectoryCore(string path, EnumerateDirectoryCallback callback) => throw new NotSupportedException("Calls to this method are not supported.");
#pragma warning restore CS0809

	/// <summary>Calls to this method are not supported</summary>
	/// <param name="path">Not supported</param>
	/// <param name="info">Not supported</param>
	/// <returns>Not supported</returns>
	/// <exception cref="NotSupportedException">always</exception>	
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("Calls to this method are not supported. This method will always throw an exception. Use the TryGetPathInfo(string, out PathInfo) method instead.")]
	[DoesNotReturn]
#pragma warning disable CS0809
	protected override bool GetPathInfoCore(string path, out PathInfo info) => throw new NotSupportedException("Calls to this method are not supported.");
#pragma warning restore CS0809

	/// <summary>Calls to this method are not supported</summary>
	/// <param name="path">Not supported</param>
	/// <param name="destination">Not supported</param>
	/// <returns>Not supported</returns>
	/// <exception cref="NotSupportedException">always</exception>	
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("Calls to this method are not supported. This method will always throw an exception. Use the TryReadFile(string, NativeMemory) method instead.")]
	[DoesNotReturn]
#pragma warning disable CS0809
	protected override bool ReadFileCore(string path, NativeMemory destination) => throw new NotSupportedException("Calls to this method are not supported.");
#pragma warning restore CS0809

	/// <summary>Calls to this method are not supported</summary>
	/// <param name="path">Not supported</param>
	/// <returns>Not supported</returns>
	/// <exception cref="NotSupportedException">always</exception>	
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("Calls to this method are not supported. This method will always throw an exception. Use the TryRemovePath(string) method instead.")]
	[DoesNotReturn]
#pragma warning disable CS0809
	protected override bool RemovePathCore(string path) => throw new NotSupportedException("Calls to this method are not supported.");
#pragma warning restore CS0809

	/// <summary>Calls to this method are not supported</summary>
	/// <param name="oldPath">Not supported</param>
	/// <param name="newPath">Not supported</param>
	/// <returns>Not supported</returns>
	/// <exception cref="NotSupportedException">always</exception>	
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("Calls to this method are not supported. This method will always throw an exception. Use the TryRenamePath(string, string) method instead.")]
	[DoesNotReturn]
#pragma warning disable CS0809
	protected override bool RenamePathCore(string oldPath, string newPath) => throw new NotSupportedException("Calls to this method are not supported.");
#pragma warning restore CS0809

	/// <summary>Calls to this method are not supported</summary>
	/// <param name="path">Not supported</param>
	/// <param name="source">Not supported</param>
	/// <returns>Not supported</returns>
	/// <exception cref="NotSupportedException">always</exception>	
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("Calls to this method are not supported. This method will always throw an exception. Use the TryWriteFile(string, ReadOnlyNativeMemory) method instead.")]
	[DoesNotReturn]
#pragma warning disable CS0809
	protected override bool WriteFileCore(string path, ReadOnlyNativeMemory source) => throw new NotSupportedException("Calls to this method are not supported.");
#pragma warning restore CS0809
}
