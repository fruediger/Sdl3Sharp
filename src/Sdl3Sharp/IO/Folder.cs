namespace Sdl3Sharp.IO;

/// <summary>
/// Represents the type of OS-provided default folders for a specific purpose
/// </summary>
public enum Folder
{
	/// <summary>The user's home directory</summary>
	/// <remarks>
	/// <para>
	/// The folder which contains all of the current user's data, preferences, and documents.
	/// It usually contains most of the other folders.
	/// If a requested folder does not exist, the home folder can be considered a safe fallback to store a user's documents.
	/// </para>
	/// </remarks>
	Home,

	/// <summary>The user's desktop folder</summary>
	/// <remarks>
	/// <para>
	/// The folder of files that are displayed on the desktop.
	/// Note that the existence of a desktop folder does not guarantee that the system does show icons on its desktop; certain GNU/Linux distros with a graphical environment may not have desktop icons.
	/// </para>
	/// </remarks>
	Desktop,

	/// <summary>The user's documents folder</summary>
	/// <remarks>
	/// <para>
	/// User document files, possibly application-specific.
	/// This is a good place to save a user's projects.
	/// </para>
	/// </remarks>
	Documents,

	/// <summary>The user's downloads folder</summary>
	/// <remarks>
	/// <para>
	/// Standard folder for user files downloaded from the internet.
	/// </para>
	/// </remarks>
	Downloads,

	/// <summary>The user's music folder</summary>
	/// <remarks>
	/// <para>
	/// Music files that can be played using a standard music player (mp3, ogg, ...).
	/// </para>
	/// </remarks>
	Music,

	/// <summary>The user's pictures folder</summary>
	/// <remarks>
	/// <para>
	/// Image files that can be displayed using a standard viewer (png, jpg, ...).
	/// </para>
	/// </remarks>
	Pictures,

	/// <summary>The user's folder for files to be shared with others</summary>
	/// <remarks>
	/// <para>
	/// Files that are meant to be shared with other users on the same computer.
	/// </para>
	/// </remarks>
	PublicShare,

	/// <summary>The user's saved games folder</summary>
	/// <remarks>
	/// <para>
	/// Save files for games.
	/// </para>
	/// </remarks>
	SavedGames,

	/// <summary>The user's screenshots folder</summary>
	/// <remarks>
	/// <para>
	/// Application screenshots.
	/// </para>
	/// </remarks>
	Screenshots,

	/// <summary>The user's templates folder</summary>
	/// <remarks>
	/// <para>
	/// Template files to be used when the user requests the desktop environment to create a new file in a certain folder, such as "New Text File.txt".
	/// Any file in the Templates folder can be used as a starting point for a new file.
	/// </para>
	/// </remarks>
	Templates,

	/// <summary>The user's videos folder</summary>
	/// <remarks>
	/// <para>
	/// Video files that can be played using a standard video player (mp4, webm, ...).
	/// </para>
	/// </remarks>
	Videos
}
