using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices.Marshalling;

namespace Sdl3Sharp.Video.Windowing;

/// <summary>
/// Provides methods for accessing the system's so called primary selection
/// </summary>
/// <remarks>
/// <para>
/// The X11 and Wayland video targets have a concept of the "primary selection" in addition to the usual <see cref="Clipboard">clipboard</see>.
/// This is generally highlighted (but not explicitly copied) text from various apps.
/// While the methods for interacting with the primary selection are also offered on platforms without this concept, on those platforms only text set by the app will be kept for later retrieval by the very same app;
/// the operating system will not ever attempt to change the text externally if it doesn't support a primary selection.
/// </para>
/// <para>
/// For interaction with the primary selection to be available, the <see cref="SubSystems.Video"/> sub system must be initialized
/// (either while <see cref="Sdl(Sdl.BuildAction?)">initializing SDL</see> or by <see cref="Sdl.InitializeSubSystems(Sdl3Sharp.SubSystems)">initializing it on its own</see>).
/// </para>
/// </remarks>
public static partial class PrimarySelection
{
	/// <summary>
	/// Determines whether there is text available in the primary selection and it's not an empty string
	/// </summary>
	/// <returns><see langword="true"/> if the primary selection contains non-empty text; otherwise, <see langword="false"/>.</returns>
	/// <remarks>
	/// <para>
	/// This method should only be called on the main thread.
	/// </para>
	/// </remarks>
	public static bool HasText() => SDL_HasPrimarySelectionText();

	/// <summary>
	/// Tries to get text from the primary selection
	/// </summary>
	/// <param name="text">The text from the primary selection, or <c><see langword="null"/></c> if text from the primary selection couldn't get successfully obtained</param>
	/// <returns><c><see langword="true"/></c>, if text from the primary selection was successfully obtained; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// This method may result an empty string for <paramref name="text"/>, if there was not enough memory left for a copy of the primary selection's content.
	/// </para>
	/// <para>
	/// This method should only be called on the main thread.
	/// </para>
	/// </remarks>
	public static bool TryGetText([NotNullWhen(true)] out string? text)
	{
		unsafe
		{
			if (SDL_GetPrimarySelectionText() is var resultPtr && resultPtr is not null)
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
	/// Tries to set text as the primary selection
	/// </summary>
	/// <param name="text">The text to store in the primary selection</param>
	/// <returns><c><see langword="true"/></c>, if the text was successfully stored in the primary selection; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks> 
	/// <para>
	/// This method should only be called on the main thread.
	/// </para>
	/// </remarks>
	public static bool TrySetText(string? text)
	{
		unsafe
		{
			var utf8Text = Utf8StringMarshaller.ConvertToUnmanaged(text);

			try
			{
				return SDL_SetPrimarySelectionText(utf8Text);
			}
			finally
			{
				Utf8StringMarshaller.Free(utf8Text);
			}
		}
	}
}
