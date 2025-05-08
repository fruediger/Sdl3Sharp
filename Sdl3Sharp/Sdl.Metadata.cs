namespace Sdl3Sharp;

partial class Sdl
{
	/// <summary>
	/// Provides predefined metadata names for <see cref="GetMetadata(string)"/> and the <c>*SetMetadata</c> methods on a <see cref="Builder"/>
	/// </summary>
	public static class Metadata
	{
		/// <summary>
		/// The human-readable name of the application
		/// </summary>
		/// <remarks>
		/// The name of the app, like <c>"My Game 2: Bad Guy's Revenge!"</c>.
		/// The metadata will show up anywhere the OS shows the name of the application separately from window titles, such as volume control applets, etc.
		/// The metadata defaults to <c>"SDL Application"</c>.
		/// </remarks>
		public const string Name = "SDL.app.metadata.name";

		/// <summary>
		/// The version of the app
		/// </summary>
		/// <remarks>
		/// There are no rules on format, so <c>"1.0.3beta2"</c> and <c>"April 22nd, 2024"</c> and a git hash are all valid options.
		/// The metadata has no default.
		/// </remarks>
		public const string Version = "SDL.app.metadata.version";

		/// <summary>
		/// A unique string that identifies this app
		/// </summary>
		/// <remarks>
		/// The metadata must be in reverse-domain format, like <c>"com.example.mygame2"</c>.
		/// The metadata is used by desktop compositors to identify and group windows together, as well as match applications with associated desktop settings and icons.
		/// If you plan to package your application in a container such as Flatpak, the app ID should match the name of your Flatpak container as well.
		/// The metadata has no default.
		/// </remarks>
		public const string Identifier = "SDL.app.metadata.identifier";

		/// <summary>
		/// The human-readable name of the creator/developer/maker of this app
		/// </summary>
		/// <remarks>
		/// The name of the creator/developer/maker of this app like <c>"MojoWorkshop, LLC"</c>
		/// </remarks>
		public const string Creator = "SDL.app.metadata.creator";

		/// <summary>
		/// The human-readable copyright notice
		/// </summary>
		/// <remarks>
		/// The copyright notice, like <c>"Copyright (c) 2024 MojoWorkshop, LLC"</c> or whatnot.
		/// Keep the metadata to one line, don't paste a copy of a whole software license in here.
		/// The metadata has no default.
		/// </remarks>
		public const string Copyright = "SDL.app.metadata.copyright";

		/// <summary>
		/// A URL to the app on the web.
		/// </summary>
		/// <remarks>
		/// The URL maybe a product page, or a storefront, or even a GitHub repository, for user's further information.
		/// The metadata has no default.
		/// </remarks>
		public const string Url = "SDL.app.metadata.url";

		/// <summary>
		/// The type of application
		/// </summary>
		/// <remarks>
		/// Currently the metadata can be <c>"game"</c> for a video game, <c>"mediaplayer"</c> for a media player, or generically <c>"application"</c> if nothing else applies.
		/// Future versions of SDL might add new types.
		/// The metadata defaults to <c>"application"</c>.
		/// </remarks>
		public const string Type = "SDL.app.metadata.type";
	}
}
