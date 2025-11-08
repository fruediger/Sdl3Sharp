using Sdl3Sharp.Utilities;
using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.Marshalling;

namespace Sdl3Sharp.IO;

/// <summary>
/// A storage area for a user's unique read/write file system
/// </summary>
public sealed partial class UserStorage : Storage
{
	private interface IUnsafeConstructorDispatch;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private unsafe static byte* Utf8Convert(string? str, out byte* strUtf8) => strUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(str);

	/// <exception cref="SdlException">The <see cref="UserStorage"/> could not be opened (check <see cref="Error.TryGet(out string?)"/> for more information)</exception>
	private unsafe UserStorage(string orgName, string appName, Properties? properties, IUnsafeConstructorDispatch? _ = default) :
		base(SDL_OpenUserStorage(Utf8Convert(orgName, out var orgNameUtf8), Utf8Convert(appName, out var appNameUtf8), properties?.Id ?? 0))
	{
		try
		{
			if (Pointer is null)
			{
				failCouldNotCreateUserStorage();
			}
		}
		finally
		{
			Utf8StringMarshaller.Free(appNameUtf8);
			Utf8StringMarshaller.Free(orgNameUtf8);
		}

		[DoesNotReturn]
		static void failCouldNotCreateUserStorage() => throw new SdlException($"Could not open the {nameof(UserStorage)}");
	}

	/// <summary>
	/// Creates a new <see cref="UserStorage"/>
	/// </summary>
	/// <param name="orgName">The name of your organization</param>
	/// <param name="appName">The name of your application</param>
	/// <param name="properties">An optional group of properties that may contain backend-specific information</param>
	/// <remarks>
	/// <para>
	/// While a <see cref="TitleStorage"/> can generally be kept open throughout runtime, a <see cref="UserStorage"/> should only be opened when the application is ready to read or write files.
	/// This allows the backend to properly batch file operations and flush them when the container has been closed; ensuring safe and optimal save I/O.
	/// </para>
	/// </remarks>
	/// <inheritdoc cref="UserStorage(string, string, Properties?, IUnsafeConstructorDispatch?)"/>
	public UserStorage(string orgName, string appName, Properties? properties = null) :
#pragma warning disable IDE0034 // Keep it that way for explicitness sake
		this(orgName, appName, properties, default(IUnsafeConstructorDispatch?))
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
