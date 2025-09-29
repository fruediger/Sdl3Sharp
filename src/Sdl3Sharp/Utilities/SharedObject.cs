using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices.Marshalling;

namespace Sdl3Sharp.Utilities;

/// <summary>
/// Represents a system-dependent code library (shared object) that is programmatically loadable at runtime
/// </summary>
/// <remarks>
/// <para>
/// Windows calls these "DLLs", Linux calls them "shared libraries", etc.
/// </para>
/// </remarks>
public sealed partial class SharedObject : IDisposable
{
	private unsafe SDL_SharedObject* mHandle = null;

	/// <summary>
	/// Create and loads a new <see cref="SharedObject"/> from a specified file
	/// </summary>
	/// <param name="file">The system-dependent name of the shared object file. Could be a file path or some kind of special name.</param>
	/// <remarks>
	/// <para>
	/// Creating a new <see cref="SharedObject"/> instance also loads the specified shared object file through system-specific manners.
	/// </para>
	/// <para>
	/// In contrast to most of the remaining API which uses the <c>Try</c>-method pattern, this constructor intentionally fails by throwing an exception.
	/// If you want to handle failures wrap the call to this constructor in a <c><see langword="try"/></c>-block,
	/// and check <see cref="Error.TryGet(out string?)"/> for more information when <c><see langword="catch"/></c>ing a <see cref="SdlException"/>.
	/// </para>
	/// </remarks>
	/// <exception cref="SdlException">Couldn't load the specified shared object file (check <see cref="Error.TryGet(out string?)"/> for more information)</exception>
	public SharedObject(string file)
	{
		unsafe
		{
			var fileUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(file);

			try
			{
				mHandle = SDL_LoadObject(fileUtf8);

				if (mHandle is null)
				{
					failCouldNotLoadSharedObject(file);
				}
			}
			finally
			{
				Utf8StringMarshaller.Free(fileUtf8);
			}
		}

		[DoesNotReturn]
		static void failCouldNotLoadSharedObject(string file) => throw new SdlException($"Could not load a shared object from {(file is not null ? $"\"{file}\"" : "null")}");
	}

	/// <remarks>
	/// <para>
	/// Note: after <see cref="Dispose">disposing</see> a <see cref="SharedObject"/>, all of the resulting symbol handles from calls to <see cref="TryLoadSymbol(string, out nint)"/> during its lifetime are now invalid!
	/// Do <em>not</em> try to dereference such a handle after <see cref="Dispose">disposing</see> the respective <see cref="SharedObject"/>!
	/// </para>
	/// </remarks>
	/// <inheritdoc/>
	~SharedObject() => DisposeImpl();

	/// <remarks>
	/// <para>
	/// Note: after <see cref="Dispose">disposing</see> a <see cref="SharedObject"/>, all of the resulting symbol handles from calls to <see cref="TryLoadSymbol(string, out nint)"/> during its lifetime are now invalid!
	/// Do <em>not</em> try to dereference such a handle after <see cref="Dispose">disposing</see> the respective <see cref="SharedObject"/>!
	/// </para>
	/// </remarks>
	/// <inheritdoc/>
	public void Dispose()
	{
		DisposeImpl();
		GC.SuppressFinalize(this);
	}

	private void DisposeImpl()
	{
		unsafe
		{
			if (mHandle is not null)
			{
				SDL_UnloadObject(mHandle);

				mHandle = null;
			}
		}
	}

	/// <summary>
	/// Tries to look up the handle of a named symbol in the <see cref="SharedObject"/>
	/// </summary>
	/// <param name="name">The name of the symbol to look up</param>
	/// <param name="symbol">The handle the symbol with the specified <paramref name="name"/> when this method returns <c><see langword="true"/></c>; otherwise, <c><see cref="IntPtr.Zero"/></c></param>
	/// <returns><c><see langword="true"/></c> if a symbol with the specified <paramref name="name"/> was found and succesfully loaded; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method can only look up exported C symbol names. Other languages may have name mangling and intrinsic language support that varies from compiler to compiler.
	/// </para>
	/// <para>
	/// If you want to call a resulting <paramref name="symbol"/> as a function, make sure you declare the function pointer signature with the same calling convention as the actual library function.
	/// Your code will crash mysteriously if you do not do this.
	/// </para>
	/// <para>
	/// Note: Resulting <paramref name="symbol"/> handles are no longer valid after <see cref="Dispose">disposing</see> the respective <see cref="SharedObject"/>!
	/// </para>
	/// </remarks>
	public bool TryLoadSymbol(string name, out IntPtr symbol)
	{
		unsafe
		{
			var nameUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(name);

			try
			{
				symbol = unchecked((IntPtr)SDL_LoadFunction(mHandle, nameUtf8));

				return symbol != IntPtr.Zero;
			}
			finally
			{
				Utf8StringMarshaller.Free(nameUtf8);
			}
		}
	}
}
