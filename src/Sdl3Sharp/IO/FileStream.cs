using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.Marshalling;

namespace Sdl3Sharp.IO;

public sealed partial class FileStream : Stream
{
	private interface IUnsafeConstructorDispatch;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private static unsafe byte* Utf8Convert(string? str, out byte* strUtf8) => strUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(str);

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

	private unsafe FileStream(string fileName, string modeString, IUnsafeConstructorDispatch? _ = default) :
		base(SDL_IOFromFile(Utf8Convert(fileName, out var fileNameUtf8), Utf8Convert(modeString, out var modeStringUtf8))) // base(SDL_IOStream*) does neither throw nor fail
	{
		try
		{
			if (Context is null)
			{
				failCouldNotCreateDynamicMemoryStream();
			}
		}
		finally
		{
			Utf8StringMarshaller.Free(modeStringUtf8);
			Utf8StringMarshaller.Free(fileNameUtf8);
		}

		[DoesNotReturn]
		static void failCouldNotCreateDynamicMemoryStream() => throw new SdlException($"Could not create the {nameof(FileStream)}");
	}

	public FileStream(string fileName, string modeString) :
#pragma warning disable IDE0034 // Keep it that way for explicitness sake
		this(fileName, modeString, default(IUnsafeConstructorDispatch?))		
#pragma warning restore IDE0034
	{ }

	// TODO: inheritdoc
	public FileStream(string fileName, FileAccess access, FileMode mode) : this(fileName, ValidateMode(access, mode))
	{ }

	// TODO: inheritdoc
	public FileStream(string fileName, FileAccess access, FileMode mode, FileKind kind) : this(fileName, ValidateMode(access, mode, kind))
	{ }

	public IntPtr AndroidAAsset
		=> Properties?.TryGetPointerValue(PropertyNames.AndroidAAssetPointer, out var androidAAsset) is true
			? androidAAsset
			: default;

	public long FileDescriptor
		=> Properties?.TryGetNumberValue(PropertyNames.FileDecriptorNumber, out var fileDescriptor) is true
			? fileDescriptor
			: -1;

	public IntPtr StdioFile
		=> Properties?.TryGetPointerValue(PropertyNames.StdioFilePointer, out var stdioFile) is true
			? stdioFile
			: default;

	public IntPtr WindowsHandle
		=> Properties?.TryGetPointerValue(PropertyNames.WindowsHandlePointer, out var windowsHandle) is true
			? windowsHandle
			: default;

	public static bool TryGetModeString(FileAccess access, FileMode mode, [NotNullWhen(true)] out string? modeString)
		=> (access, mode) switch
		{
			(FileAccess.None,      _)                       => modeString = "",
			(FileAccess.Read,      FileMode.Open)           => modeString = "r",			
			(FileAccess.Write,     FileMode.EraseOrCreate)  => modeString = "w",			
			(FileAccess.Write,     FileMode.AppendOrCreate) => modeString = "a",
			(FileAccess.ReadWrite, FileMode.Open)           => modeString = "r+",
			(FileAccess.ReadWrite, FileMode.EraseOrCreate)  => modeString = "w+",
			(FileAccess.ReadWrite, FileMode.AppendOrCreate) => modeString = "a+",
			_ => modeString = null
		} is not null;

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