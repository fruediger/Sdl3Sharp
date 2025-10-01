using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace Sdl3Sharp.Windowing;

/// <summary>
/// Provides methods for accessing the system clipboard, both for reading information from other processes and publishing information of its own
/// </summary>
/// <remarks>
/// <para>
/// For interaction with the clipboard to be available, the <see cref="SubSystem.Video"/> sub system must be initialized
/// (either while <see cref="Sdl(Sdl.BuildAction?)">initializing SDL</see> or by <see cref="Sdl.InitializeSubSystems(SubSystemSet)">initializing it on its own</see>).
/// </para>
/// </remarks>
public static partial class Clipboard
{
	/// <summary>
	/// Determines whether there is data in the clipboard for the provided mime type
	/// </summary> 
	/// <param name="mimeType">The mime type to check for data in the clipboard</param>
	/// <returns><see langword="true"/> if clipboard data for the specified mime type is available; otherwise, <see langword="false"/></returns>
	/// <remarks> 
	/// <para>
	/// This method should only be called on the main thread.
	/// </para>
	/// </remarks>
	public static bool HasData(string mimeType)
	{
		unsafe
		{
			var mimeTypeUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(mimeType);

			try
			{
				return SDL_HasClipboardData(mimeTypeUtf8);
			}
			finally
			{
				Utf8StringMarshaller.Free(mimeTypeUtf8);
			}
		}
	}

	/// <summary>
	/// Determines whether there is textual data available on the clipboard and it's not an empty string
	/// </summary>
	/// <returns><see langword="true"/> if the clipboard contains non-empty textual data; otherwise, <see langword="false"/>.</returns>
	/// <remarks>
	/// <para>
	/// This method should only be called on the main thread.
	/// </para>
	/// </remarks>
	public static bool HasText() => SDL_HasClipboardText();

	/// <summary>
	/// Tries to clear the clipboard data
	/// </summary>
	/// <returns><c><see langword="true"/></c>, if the clipboard was cleared successfully; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method should only be called on the main thread.
	/// </para>
	/// </remarks>
	public static bool TryClearData() => SDL_ClearClipboardData();

	/// <summary>
	/// Tries to get the clipboard data for a given mime type
	/// </summary>
	/// <param name="mimeType">The mime type to read from the clipboard</param>
	/// <param name="data">The clipboard data for the specified mime type, or <c><see langword="null"/></c> if the clipboard was not successfully obtained</param>
	/// <returns><c><see langword="true"/></c>, if the clipboard data for the specified mime type was successfully obtained; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// Note: Textual data (e.g. UTF8 encoded text) might not be null terminated.
	/// </para>
	/// <para>
	/// To get textual data from the clipboard, you can conveniently use <see cref="TryGetText(out string?)"/> instead.
	/// </para>
	/// <para>
	/// This method should only be called on the main thread.
	/// </para>
	/// </remarks>
	public static bool TryGetData(string mimeType, [NotNullWhen(true)] out byte[]? data)
	{
		unsafe
		{
			var mimeTypeUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(mimeType);
			nuint size;

			try
			{
				if (SDL_GetClipboardData(mimeTypeUtf8, &size) is var resultPtr && resultPtr is not null)
				{
					try
					{
						if (size is 0)
						{
							data = [];
							return true;
						}

						data = size is <= int.MaxValue
							? GC.AllocateUninitializedArray<byte>(unchecked((int)size))
							: new byte[size];

						fixed (byte* dataPtr = data)
						{
							Buffer.MemoryCopy(resultPtr, dataPtr, size, size);
						}

						return true;
					}
					finally
					{
						Utilities.NativeMemory.SDL_free(resultPtr);
					}
				}			

				data = null;
				return false;
			}
			finally
			{
				Utf8StringMarshaller.Free(mimeTypeUtf8);
			}
		}
	}

	/// <summary>
	/// Tries to retrieve a list of mime types available in the clipboard
	/// </summary>
	/// <param name="mimeTypes">The list of mime types available in the clipboard, or <c><see langword="null"/></c> if the list of mime types was not successfully obtained</param>
	/// <returns><c><see langword="true"/></c>, if the list of mime types available in the clipboard was successfully obtained; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method should only be called on the main thread.
	/// </para>
	/// </remarks>
	public static bool TryGetMimeTypes([NotNullWhen(true)] out string[]? mimeTypes)
	{
		unsafe
		{
			nuint numMimeTypes;

			if (SDL_GetClipboardMimeTypes(&numMimeTypes) is var resultPtr && resultPtr is not null)
			{
				try
				{
					if (numMimeTypes is 0)
					{
						mimeTypes = [];
						return true;
					}

					mimeTypes = numMimeTypes is <= int.MaxValue
						? GC.AllocateUninitializedArray<string>(unchecked((int)numMimeTypes))
						: new string[numMimeTypes];

					for (var i = (nuint)0; i < numMimeTypes; i++)
					{
						mimeTypes[i] = Utf8StringMarshaller.ConvertToManaged(resultPtr[i])!;
					}

					return true;
				}
				finally
				{
					Utilities.NativeMemory.SDL_free(resultPtr);
				}
			}

			mimeTypes = null;
			return false;
		}
	}

	/// <summary>
	/// Tries to get text from the clipboard
	/// </summary>
	/// <param name="text">The text from the clipboard, or <c><see langword="null"/></c> if textual data from the clipboard couldn't get successfully obtained</param>
	/// <returns><c><see langword="true"/></c>, if textual data from the clipboard was successfully obtained; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method may result an empty string for <paramref name="text"/>, if there was not enough memory left for a copy of the clipboard's content.
	/// </para>
	/// <para>
	/// This method should only be called on the main thread.
	/// </para>
	/// </remarks>
	public static bool TryGetText([NotNullWhen(true)] out string? text)
	{
		unsafe
		{
			if (SDL_GetClipboardText() is var resultPtr && resultPtr is not null)
			{
				try
				{
					text = Utf8StringMarshaller.ConvertToManaged(resultPtr);
					return text is not (null or "");
				}
				finally
				{
					Utilities.NativeMemory.SDL_free(resultPtr);
				}
			}

			text = null;
			return false;
		}
	}

	/// <summary>
	/// Tries to offer clipboard data for a given collection of mime types to the operating system
	/// </summary>
	/// <param name="dataGetter">A <see cref="DataGetter"/> delegate which gets invoked when data for a specific mime type from the <paramref name="mimeTypes"/> collection is requested</param>
	/// <param name="mimeTypes">A collection of mime types to offer clipboard data for</param>
	/// <returns><c><see langword="true"/></c>, if the clipboard data was successfully offered to the operating system; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method just tells the operating system that the application is offering clipboard data for each of the provided mime types.
	/// Once another application requests the data for a specific mime type, the provided <paramref name="dataGetter"/> delegate will be invoked, allowing it to generate and respond with the data for the requested mime type.
	/// </para>
	/// <para>
	/// Textual data (e.g. UTF8 encoded text) does not need to be null terminated (e.g. you can directly copy a portion of a document).
	/// </para> 
	/// <para>
	/// To set textual data for the clipboard, you can conveniently use <see cref="TrySetText(string)"/> instead.
	/// </para>
	/// <para>
	/// This method should only be called on the main thread.
	/// </para>
	/// </remarks>
	public static bool TrySetData(DataGetter dataGetter, params ReadOnlySpan<string> mimeTypes)
	{
		unsafe
		{
			if (dataGetter is not null && mimeTypes.Length is > 0)
			{
				var mimeTypesUtf8 = (byte**)Utilities.NativeMemory.Malloc(unchecked((nuint)mimeTypes.Length * (nuint)sizeof(byte*)));

				if (mimeTypesUtf8 is not null)
				{	
					var mimeTypesCount = (nuint)0;

					try
					{
						foreach (var mimeType in mimeTypes)
						{
							mimeTypesUtf8[mimeTypesCount++] = Utf8StringMarshaller.ConvertToUnmanaged(mimeType);
						}

						return SDL_SetClipboardData(&DataGetterWrapper.DataCallback, &DataGetterWrapper.CleanupCallback,
							unchecked((void*)GCHandle.ToIntPtr(GCHandle.Alloc(new DataGetterWrapper(dataGetter), GCHandleType.Normal))),
							mimeTypesUtf8, mimeTypesCount
						);
					}
					finally
					{
						while (mimeTypesCount is > 0)
						{
							Utf8StringMarshaller.Free(mimeTypesUtf8[--mimeTypesCount]);
						}

						Utilities.NativeMemory.Free(mimeTypesUtf8);
					}
				}
			}

			return SDL_SetClipboardData(null, null, null, null, 0);			
		}
	}

	/// <summary>
	/// Tries to set textual data for the clipboard
	/// </summary>
	/// <param name="text">The text to store in the clipboard</param>
	/// <returns><c><see langword="true"/></c>, if the textual data was successfully stored in the clipboard; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks> 
	/// <para>
	/// This method should only be called on the main thread.
	/// </para>
	/// </remarks>
	public static bool TrySetText(string text)
	{
		unsafe
		{
			var textUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(text);

			try
			{
				return SDL_SetClipboardText(textUtf8);
			}
			finally
			{
				Utf8StringMarshaller.Free(textUtf8);
			}
		}
	}
}
