using Sdl3Sharp.Internal.Interop;
using Sdl3Sharp.SourceGeneration;
using Sdl3Sharp.Video.Windowing;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sdl3Sharp.Utilities;

partial class MessageBox
{	
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_MessageBoxButtonData">SDL_MessageBoxButtonData</seealso>
	[StructLayout(LayoutKind.Sequential)]
	internal unsafe struct SDL_MessageBoxButtonData
	{
		public MessageBoxButtonFlags Flags;
		public int ButtonId;
		public byte* Text;
	}

	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_MessageBoxColorType"></seealso>
	internal enum SDL_MessageBoxColorType
	{
		Background,
		Text,
		ButtonBorder,
		ButtonBackground,
		ButtonSelected,

		Count
	}

	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_MessageBoxColorScheme">SDL_MessageBoxColorScheme</seealso>
	[StructLayout(LayoutKind.Sequential)]
	internal struct SDL_MessageBoxColorScheme
	{
		public ColorsArray Colors;

		[InlineArray((int)SDL_MessageBoxColorType.Count)]
		public struct ColorsArray
		{
			private MessageBoxColor _;

			public MessageBoxColor this[SDL_MessageBoxColorType colorType]
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] readonly get => this[unchecked((int)colorType)];
				[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] set => this[unchecked((int)colorType)] = value;
			}
		}
	}

	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_MessageBoxData">SDL_MessageBoxData</seealso>
	[StructLayout(LayoutKind.Sequential)]
	internal unsafe struct SDL_MessageBoxData
	{
		public MessageBoxFlags Flags;
		public Window.SDL_Window* Window;
		public byte* Title;
		public byte* Message;

		public int NumButtons;
		public SDL_MessageBoxButtonData* Buttons;

		public SDL_MessageBoxColorScheme* ColorScheme;
	}

	/// <summary>
	/// Create a modal message box
	/// </summary>
	/// <param name="messageboxdata">The <see href="">SDL_MessageBoxData</see> structure with title, text and other options</param>
	/// <param name="buttonid">The pointer to which user id of hit button should be copied</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information.</returns>
	/// <remarks>
	/// <para>
	/// If your needs aren't complex, it might be easier to use <see href="https://wiki.libsdl.org/SDL3/SDL_ShowSimpleMessageBox">SDL_ShowSimpleMessageBox</see>.
	/// </para>
	/// <para>
	/// This function should be called on the thread that created the parent window, or on the main thread if the messagebox has no parent.
	/// It will block execution of that thread until the user clicks a button or closes the messagebox.
	/// </para>
	/// <para>
	/// This function may be called at any time, even before <see href="https://wiki.libsdl.org/SDL3/SDL_Init">SDL_Init()</see>.
	/// This makes it useful for reporting errors like a failure to create a renderer or OpenGL context.
	/// </para>
	/// <para>
	/// On X11, SDL rolls its own dialog box with X11 primitives instead of a formal toolkit like GTK+ or Qt.
	/// </para>
	/// <para>
	/// Note that if <see href="https://wiki.libsdl.org/SDL3/SDL_Init">SDL_Init</see>() would fail because there isn't any available video target, this function is likely to fail for the same reasons.
	/// If this is a concern, check the return value from this function and fall back to writing to stderr if you can.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_ShowMessageBox">SDL_ShowMessageBox</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_ShowMessageBox(SDL_MessageBoxData* messageboxdata, int* buttonid);

	/// <summary>
	/// Display a simple modal message box
	/// </summary>
	/// <param name="flags">An <see href="https://wiki.libsdl.org/SDL3/SDL_MessageBoxFlags">SDL_MessageBoxFlags</see> value</param>
	/// <param name="title">UTF-8 title text</param>
	/// <param name="message">UTF-8 message text</param>
	/// <param name="window">The parent window, or NULL for no parent</param>
	/// <returns>Returns true on success or false on failure; call <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see>() for more information</returns>
	/// <remarks>
	/// <para>
	/// If your needs aren't complex, this function is preferred over <see href="https://wiki.libsdl.org/SDL3/SDL_ShowMessageBox">SDL_ShowMessageBox</see>.
	/// </para>
	/// <para>
	/// <c><paramref name="flags"/></c> may be any of the following:
	/// <list type="bullet">
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_MESSAGEBOX_ERROR"><c>SDL_MESSAGEBOX_ERROR</c></see></term>
	///			<description>error dialog</description>
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_MESSAGEBOX_WARNING"><c>SDL_MESSAGEBOX_WARNING</c></see></term>
	///			<description>error dialog</description>
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3/SDL_MESSAGEBOX_INFORMATION"><c>SDL_MESSAGEBOX_INFORMATION</c></see></term>
	///			<description>error dialog</description>
	///		</item>
	/// </list>
	/// </para>
	/// <para>
	/// This function should be called on the thread that created the parent window, or on the main thread if the messagebox has no parent.
	/// It will block execution of that thread until the user clicks a button or closes the messagebox.
	/// </para>
	/// <para>
	/// This function may be called at any time, even before <see href="https://wiki.libsdl.org/SDL3/SDL_Init">SDL_Init</see>().
	/// This makes it useful for reporting errors like a failure to create a renderer or OpenGL context.
	/// </para>
	/// <para>
	/// On X11, SDL rolls its own dialog box with X11 primitives instead of a formal toolkit like GTK+ or Qt.
	/// </para>
	/// <para>
	/// Note that if <see href="https://wiki.libsdl.org/SDL3/SDL_Init">SDL_Init</see>() would fail because there isn't any available video target, this function is likely to fail for the same reasons.
	/// If this is a concern, check the return value from this function and fall back to writing to stderr if you can.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_ShowSimpleMessageBox">SDL_ShowSimpleMessageBox</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial CBool SDL_ShowSimpleMessageBox(MessageBoxFlags flags, byte* title, byte* message, Window.SDL_Window* window);
}
