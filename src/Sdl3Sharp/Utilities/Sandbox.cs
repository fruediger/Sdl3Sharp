namespace Sdl3Sharp.Utilities;

/// <summary>
/// Represents the kind of an application sandbox environment
/// </summary>
public enum Sandbox
{
	/// <summary>No sandbox environment</summary>
	None = 0,

	/// <summary>The application seems to be sandboxed, but in an unknown kind of container</summary>
	UnknownContainer,

	/// <summary><see href="https://en.wikipedia.org/wiki/Flatpak">Flatpak</see> sandbox environment</summary>
	Flatpak,

	/// <summary><see href="https://en.wikipedia.org/wiki/Snap_(software)">Snap</see> sandbox environment</summary>
	Snap,

	/// <summary><see href="https://developer.apple.com/documentation/security/app-sandbox">macOS App Sandbox</see> environment</summary>
	MacOS
}
