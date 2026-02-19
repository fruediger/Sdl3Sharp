namespace Sdl3Sharp.Video.Windowing;

partial class Clipboard
{
	/// <summary>
	/// Represents a delegates that responds with clipboad data for a requested mime type
	/// </summary>
	/// <param name="mimeType">The requested mime type of the clipboard data to retrieve, or <c><see langword="null"/></c> when the clipboard is cleared</param>
	/// <returns>A byte array containing the retrieved clipboard data for the requested mime type, or an empty array or <c><see langword="null"/></c> if no data should be send to the "receiver"</returns>
	/// <remarks>
	/// <para>
	/// Returning an empty array or <c><see langword="null"/></c> will cause no data to be sent to the "receiver".
	/// It is up to the receiver to handle this.
	/// Essentially returning no data is more or less undefined behavior and may cause breakage in receiving applications. 
	/// </para>
	/// <para>
	/// This delegate is invoked with <c><see langword="null"/></c> for its <paramref name="mimeType"/> when the clipboard is cleared or new data is set.
	/// The clipboard is automatically cleared when <see cref="Sdl.Dispose">SDL shuts down</see>.
	/// </para>
	/// </remarks>
	public delegate byte[]? DataGetter(string? mimeType);
}
