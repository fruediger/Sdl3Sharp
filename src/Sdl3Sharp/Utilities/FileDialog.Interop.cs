using Sdl3Sharp.Internal.Interop;
using Sdl3Sharp.SourceGeneration;
using Sdl3Sharp.Video.Windowing;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using unsafe SDL_DialogFileCallback = delegate* unmanaged[Cdecl]<void*, byte**, int, void>;

namespace Sdl3Sharp.Utilities;

partial class FileDialog
{	
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_DialogFileFilter">SDL_DialogFileFilter</seealso>
	[StructLayout(LayoutKind.Sequential)]
	internal unsafe struct SDL_DialogFileFilter
	{
		public byte* Name;
		public byte* Pattern;
	}

	[UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
	private unsafe static void DialogFileCallback(void* userdata, byte** filelist, int filter)
	{
		if (userdata is not null && GCHandle.FromIntPtr(unchecked((nint)userdata)) is { IsAllocated: true, Target: FileDialogCallbackWrapper wrapper } gcHandle)
		{
			wrapper.Invoke(filelist, filter);

			wrapper.Dispose();

			gcHandle.Free();
		}
	}

	/// <summary>
	/// Create and launch a file dialog with the specified properties
	/// </summary>
	/// <param name="type">The type of file dialog</param>
	/// <param name="callback">A function pointer to be invoked when the user selects a file and accepts, or cancels the dialog, or an error occurs</param>
	/// <param name="userdata">An optional pointer to pass extra data to the callback when it will be invoked</param>
	/// <param name="props">The properties to use</param>
	/// <remarks>
	/// <para>
	/// These are the supported properties:
	/// <list type="bullet">
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_FILE_DIALOG_FILTERS_POINTER"><c>SDL_PROP_FILE_DIALOG_FILTERS_POINTER</c></see></term>
	///			<description>
	///				A pointer to a list of <see href="https://wiki.libsdl.org/SDL3/SDL_DialogFileFilter">SDL_DialogFileFilter</see> structs, which will be used as filters for file-based selections.
	///				Ignored if the dialog is an "Open Folder" dialog.
	///				If non-NULL, the array of filters must remain valid at least until the callback is invoked.
	///			</description>
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_FILE_DIALOG_NFILTERS_NUMBER"><c>SDL_PROP_FILE_DIALOG_NFILTERS_NUMBER</c></see></term>
	///			<description>The number of filters in the array of filters, if it exists</description>
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_FILE_DIALOG_WINDOW_POINTER"><c>SDL_PROP_FILE_DIALOG_WINDOW_POINTER</c></see></term>
	///			<description>The window that the dialog should be modal for</description>
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_FILE_DIALOG_LOCATION_STRING"><c>SDL_PROP_FILE_DIALOG_LOCATION_STRING</c></see></term>
	///			<description>The default folder or file to start the dialog at</description>
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_FILE_DIALOG_MANY_BOOLEAN"><c>SDL_PROP_FILE_DIALOG_MANY_BOOLEAN</c></see></term>
	///			<description>true to allow the user to select more than one entry</description>
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_FILE_DIALOG_TITLE_STRING"><c>SDL_PROP_FILE_DIALOG_TITLE_STRING</c></see></term>
	///			<description>The title for the dialog</description>
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_FILE_DIALOG_ACCEPT_STRING"><c>SDL_PROP_FILE_DIALOG_ACCEPT_STRING</c></see></term>
	///			<description>The label that the accept button should have</description>
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_PROP_FILE_DIALOG_CANCEL_STRING"><c>SDL_PROP_FILE_DIALOG_CANCEL_STRING</c></see></term>
	///			<description>The label that the cancel button should have</description>
	///		</item>
	/// </list>
	/// </para>
	/// <para>
	/// Note that each platform may or may not support any of the properties.
	/// </para>
	/// <para>
	/// This function should be called only from the main thread.
	/// The callback may be invoked from the same thread or from a different one, depending on the OS's constraints.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_ShowFileDialogWithProperties">SDL_ShowFileDialogWithProperties</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial void SDL_ShowFileDialogWithProperties(FileDialogType type, SDL_DialogFileCallback callback, void* userdata, uint props);

	/// <summary>
	/// Displays a dialog that lets the user select a file on their filesystem
	/// </summary>
	/// <param name="callback">A function pointer to be invoked when the user selects a file and accepts, or cancels the dialog, or an error occurs</param>
	/// <param name="userdata">An optional pointer to pass extra data to the callback when it will be invoked</param>
	/// <param name="window">The window that the dialog should be modal for, may be NULL. Not all platforms support this option.</param>
	/// <param name="filters">A list of filters, may be NULL. Not all platforms support this option, and platforms that do support it may allow the user to ignore the filters. If non-NULL, it must remain valid at least until the callback is invoked.</param>
	/// <param name="nfilters">The number of filters. Ignored if filters is NULL.</param>
	/// <param name="default_location">The default folder or file to start the dialog at, may be NULL. Not all platforms support this option.</param>
	/// <param name="allow_many">If non-zero, the user will be allowed to select multiple entries. Not all platforms support this option.</param>
	/// <remarks>
	/// <para>
	/// This is an asynchronous function; it will return immediately, and the result will be passed to the callback.
	/// </para>
	/// <para>
	/// The callback will be invoked with a null-terminated list of files the user chose.
	/// The list will be empty if the user canceled the dialog, and it will be NULL if an error occurred.
	/// </para>
	/// <para>
	/// Note that the callback may be called from a different thread than the one the function was invoked on.
	/// </para>
	/// <para>
	/// Depending on the platform, the user may be allowed to input paths that don't yet exist.
	/// </para>
	/// <para>
	/// On Linux, dialogs may require XDG Portals, which requires DBus, which requires an event-handling loop.
	/// Apps that do not use SDL to handle events should add a call to <see cref="Sdl.PumpEvents"/> in their main loop.
	/// </para>
	/// <para>
	/// This function should be called only from the main thread.
	/// The callback may be invoked from the same thread or from a different one, depending on the OS's constraints.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_ShowOpenFileDialog">SDL_ShowOpenFileDialog</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial void SDL_ShowOpenFileDialog(SDL_DialogFileCallback callback, void* userdata, Window.SDL_Window* window, SDL_DialogFileFilter* filters, int nfilters, byte* default_location, CBool allow_many);

	/// <summary>
	/// Displays a dialog that lets the user select a folder on their filesystem
	/// </summary>
	/// <param name="callback">A function pointer to be invoked when the user selects a file and accepts, or cancels the dialog, or an error occurs</param>
	/// <param name="userdata">An optional pointer to pass extra data to the callback when it will be invoked</param>
	/// <param name="window">The window that the dialog should be modal for, may be NULL. Not all platforms support this option.</param>
	/// <param name="default_location">The default folder or file to start the dialog at, may be NULL. Not all platforms support this option.</param>
	/// <param name="allow_many">If non-zero, the user will be allowed to select multiple entries. Not all platforms support this option.</param>
	/// <remarks>
	/// <para>
	/// This is an asynchronous function; it will return immediately, and the result will be passed to the callback.
	/// </para>
	/// <para>
	/// The callback will be invoked with a null-terminated list of files the user chose.
	/// The list will be empty if the user canceled the dialog, and it will be NULL if an error occurred.
	/// </para>
	/// <para>
	/// Note that the callback may be called from a different thread than the one the function was invoked on.
	/// </para>
	/// <para>
	/// Depending on the platform, the user may be allowed to input paths that don't yet exist.
	/// </para>
	/// <para>
	/// On Linux, dialogs may require XDG Portals, which requires DBus, which requires an event-handling loop.
	/// Apps that do not use SDL to handle events should add a call to <see cref="Sdl.PumpEvents"/> in their main loop.
	/// </para>
	/// <para>
	/// This function should be called only from the main thread.
	/// The callback may be invoked from the same thread or from a different one, depending on the OS's constraints.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_ShowOpenFolderDialog">SDL_ShowOpenFolderDialog</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial void SDL_ShowOpenFolderDialog(SDL_DialogFileCallback callback, void* userdata, Window.SDL_Window* window, byte* default_location, CBool allow_many);

	/// <summary>
	/// Displays a dialog that lets the user choose a new or existing file on their filesystem
	/// </summary>
	/// <param name="callback">A function pointer to be invoked when the user selects a file and accepts, or cancels the dialog, or an error occurs</param>
	/// <param name="userdata">An optional pointer to pass extra data to the callback when it will be invoked</param>
	/// <param name="window">The window that the dialog should be modal for, may be NULL. Not all platforms support this option.</param>
	/// <param name="filters">A list of filters, may be NULL. Not all platforms support this option, and platforms that do support it may allow the user to ignore the filters. If non-NULL, it must remain valid at least until the callback is invoked.</param>
	/// <param name="nfilters">The number of filters. Ignored if filters is NULL.</param>
	/// <param name="default_location">The default folder or file to start the dialog at, may be NULL. Not all platforms support this option.</param>
	/// <remarks>
	/// <para>
	/// This is an asynchronous function; it will return immediately, and the result will be passed to the callback.
	/// </para>
	/// <para>
	/// The callback will be invoked with a null-terminated list of files the user chose.
	/// The list will be empty if the user canceled the dialog, and it will be NULL if an error occurred.
	/// </para>
	/// <para>
	/// Note that the callback may be called from a different thread than the one the function was invoked on.
	/// </para>
	/// <para>
	/// The chosen file may or may not already exist.
	/// </para>
	/// <para>
	/// On Linux, dialogs may require XDG Portals, which requires DBus, which requires an event-handling loop.
	/// Apps that do not use SDL to handle events should add a call to <see cref="Sdl.PumpEvents"/> in their main loop.
	/// </para>
	/// <para>
	/// This function should be called only from the main thread.
	/// The callback may be invoked from the same thread or from a different one, depending on the OS's constraints.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_ShowOpenFileDialog">SDL_ShowOpenFileDialog</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial void SDL_ShowSaveFileDialog(SDL_DialogFileCallback callback, void* userdata, Window.SDL_Window* window, SDL_DialogFileFilter* filters, int nfilters, byte* default_location);
}
