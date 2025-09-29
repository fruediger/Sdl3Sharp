using System.Diagnostics.CodeAnalysis;

namespace Sdl3Sharp;

/// <summary>
/// Provides extensions for <see cref="Sdl.Builder"/>
/// </summary>
public static class SdlBuilderExtensions
{
	/// <summary>
	/// Sets the human-readable name of the application
	/// </summary>
	/// <param name="builder">The <see cref="Sdl.Builder"/> to use</param>
	/// <param name="appName">The name of the app, like <c>"My Game 2: Bad Guy's Revenge!"</c></param>
	/// <param name="throwOnFailure">A value indicating whether to fail silently (<c><see langword="false"/></c>), or to throw an exception on failure (<c><see langword="true"/></c>)</param>
	/// <returns>The current <see cref="Sdl.Builder"/> so that additional calls can be chained</returns>
	/// <remarks>
	/// <para>
	/// You can optionally provide metadata about your app to SDL. This is not required, but strongly encouraged.
	/// </para>
	/// <para>
	/// The metadata will show up anywhere the OS shows the name of the application separately from window titles, such as volume control applets, etc.
	/// </para>
	/// <para>
	/// In contrast to most of the remaining API which uses the <c>Try</c>-method pattern, this method intentionally fails silently, if <paramref name="throwOnFailure"/> is <c><see langword="false"/></c>,
	/// or fails by throwing an exception, if <paramref name="throwOnFailure"/> is <c><see langword="true"/></c>.
	/// If you want to handle failures wrap the call to this method with <paramref name="throwOnFailure"/> set to <c><see langword="true"/></c> in a <c><see langword="try"/></c>-block,
	/// and check <see cref="Error.TryGet(out string?)"/> for more information when <c><see langword="catch"/></c>ing a <see cref="SdlException"/>.
	/// </para>
	/// </remarks>
	/// <inheritdoc cref="SetMetadata(Sdl.Builder, string, string?, bool)"/>
	public static Sdl.Builder SetAppName(this Sdl.Builder builder, string? appName, bool throwOnFailure = false)
		=> builder.SetMetadata(Sdl.Metadata.Name, appName, throwOnFailure);

	/// <summary>
	/// Sets the human-readable copyright notice
	/// </summary>
	/// <param name="builder">The <see cref="Sdl.Builder"/> to use</param>
	/// <param name="appCopyright">The copyright notice, like <c>"Copyright (c) 2024 MojoWorkshop, LLC"</c> or whatnot</param>
	/// <param name="throwOnFailure">A value indicating whether to fail silently (<c><see langword="false"/></c>), or to throw an exception on failure (<c><see langword="true"/></c>)</param>
	/// <returns>The current <see cref="Sdl.Builder"/> so that additional calls can be chained</returns>
	/// <remarks>
	/// <para>
	/// You can optionally provide metadata about your app to SDL. This is not required, but strongly encouraged.
	/// </para>
	/// <para>
	/// Keep the metadata to one line, don't paste a copy of a whole software license in here.
	/// </para>
	/// <para>
	/// In contrast to most of the remaining API which uses the <c>Try</c>-method pattern, this method intentionally fails silently, if <paramref name="throwOnFailure"/> is <c><see langword="false"/></c>,
	/// or fails by throwing an exception, if <paramref name="throwOnFailure"/> is <c><see langword="true"/></c>.
	/// If you want to handle failures wrap the call to this method with <paramref name="throwOnFailure"/> set to <c><see langword="true"/></c> in a <c><see langword="try"/></c>-block,
	/// and check <see cref="Error.TryGet(out string?)"/> for more information when <c><see langword="catch"/></c>ing a <see cref="SdlException"/>.
	/// </para>
	/// </remarks>
	/// <inheritdoc cref="SetMetadata(Sdl.Builder, string, string?, bool)"/>
	public static Sdl.Builder SetAppCopyright(this Sdl.Builder builder, string? appCopyright, bool throwOnFailure = false)
		=> builder.SetMetadata(Sdl.Metadata.Copyright, appCopyright, throwOnFailure);

	/// <summary>
	/// Sets the human-readable name of the creator/developer/maker of this app
	/// </summary>
	/// <param name="builder">The <see cref="Sdl.Builder"/> to use</param>
	/// <param name="appCreator">The name of the creator/developer/maker of this app like <c>"MojoWorkshop, LLC"</c></param>
	/// <param name="throwOnFailure">A value indicating whether to fail silently (<c><see langword="false"/></c>), or to throw an exception on failure (<c><see langword="true"/></c>)</param>
	/// <returns>The current <see cref="Sdl.Builder"/> so that additional calls can be chained</returns>
	/// <remarks>
	/// <para>
	/// You can optionally provide metadata about your app to SDL. This is not required, but strongly encouraged.
	/// </para>
	/// <para>
	/// In contrast to most of the remaining API which uses the <c>Try</c>-method pattern, this method intentionally fails silently, if <paramref name="throwOnFailure"/> is <c><see langword="false"/></c>,
	/// or fails by throwing an exception, if <paramref name="throwOnFailure"/> is <c><see langword="true"/></c>.
	/// If you want to handle failures wrap the call to this method with <paramref name="throwOnFailure"/> set to <c><see langword="true"/></c> in a <c><see langword="try"/></c>-block,
	/// and check <see cref="Error.TryGet(out string?)"/> for more information when <c><see langword="catch"/></c>ing a <see cref="SdlException"/>.
	/// </para>
	/// </remarks>
	/// <inheritdoc cref="SetMetadata(Sdl.Builder, string, string?, bool)"/>
	public static Sdl.Builder SetAppCreator(this Sdl.Builder builder, string? appCreator, bool throwOnFailure = false)
		=> builder.SetMetadata(Sdl.Metadata.Creator, appCreator, throwOnFailure);

	/// <summary>
	/// Sets a unique string that identifies this app
	/// </summary>
	/// <param name="builder">The <see cref="Sdl.Builder"/> to use</param>
	/// <param name="appIdentifier">The identifier of this app in reverse-domain format, like <c>"com.example.mygame2"</c></param>
	/// <param name="throwOnFailure">A value indicating whether to fail silently (<c><see langword="false"/></c>), or to throw an exception on failure (<c><see langword="true"/></c>)</param>
	/// <returns>The current <see cref="Sdl.Builder"/> so that additional calls can be chained</returns>
	/// <remarks>
	/// <para>
	/// You can optionally provide metadata about your app to SDL. This is not required, but strongly encouraged.
	/// </para>
	/// <para>
	/// The metadata is used by desktop compositors to identify and group windows together, as well as match applications with associated desktop settings and icons.
	/// If you plan to package your application in a container such as Flatpak, the app ID should match the name of your Flatpak container as well.
	/// </para>
	/// <para>
	/// In contrast to most of the remaining API which uses the <c>Try</c>-method pattern, this method intentionally fails silently, if <paramref name="throwOnFailure"/> is <c><see langword="false"/></c>,
	/// or fails by throwing an exception, if <paramref name="throwOnFailure"/> is <c><see langword="true"/></c>.
	/// If you want to handle failures wrap the call to this method with <paramref name="throwOnFailure"/> set to <c><see langword="true"/></c> in a <c><see langword="try"/></c>-block,
	/// and check <see cref="Error.TryGet(out string?)"/> for more information when <c><see langword="catch"/></c>ing a <see cref="SdlException"/>.
	/// </para>
	/// </remarks>
	/// <inheritdoc cref="SetMetadata(Sdl.Builder, string, string?, bool)"/>
	public static Sdl.Builder SetAppIdentifier(this Sdl.Builder builder, string? appIdentifier, bool throwOnFailure = false)
		=> builder.SetMetadata(Sdl.Metadata.Identifier, appIdentifier, throwOnFailure);

	/// <summary>
	/// Sets the type of application
	/// </summary>
	/// <param name="builder">The <see cref="Sdl.Builder"/> to use</param>
	/// <param name="appType">The type of application</param>
	/// <param name="throwOnFailure">A value indicating whether to fail silently (<c><see langword="false"/></c>), or to throw an exception on failure (<c><see langword="true"/></c>)</param>
	/// <returns>The current <see cref="Sdl.Builder"/> so that additional calls can be chained</returns>
	/// <remarks>
	/// <para>
	/// You can optionally provide metadata about your app to SDL. This is not required, but strongly encouraged.
	/// </para>
	/// <para>
	/// Currently the metadata can be <c>"game"</c> for a video game, <c>"mediaplayer"</c> for a media player, or generically <c>"application"</c> if nothing else applies.
	/// Future versions of SDL might add new types.
	/// </para>
	/// <para>
	/// In contrast to most of the remaining API which uses the <c>Try</c>-method pattern, this method intentionally fails silently, if <paramref name="throwOnFailure"/> is <c><see langword="false"/></c>,
	/// or fails by throwing an exception, if <paramref name="throwOnFailure"/> is <c><see langword="true"/></c>.
	/// If you want to handle failures wrap the call to this method with <paramref name="throwOnFailure"/> set to <c><see langword="true"/></c> in a <c><see langword="try"/></c>-block,
	/// and check <see cref="Error.TryGet(out string?)"/> for more information when <c><see langword="catch"/></c>ing a <see cref="SdlException"/>.
	/// </para>
	/// </remarks>
	/// <inheritdoc cref="SetMetadata(Sdl.Builder, string, string?, bool)"/>
	public static Sdl.Builder SetAppType(this Sdl.Builder builder, string? appType, bool throwOnFailure = false)
		=> builder.SetMetadata(Sdl.Metadata.Type, appType, throwOnFailure);

	/// <summary>
	/// Sets a URL to the app on the web
	/// </summary>
	/// <param name="builder">The <see cref="Sdl.Builder"/> to use</param>
	/// <param name="appUrl">The URL of this app</param>
	/// <param name="throwOnFailure">A value indicating whether to fail silently (<c><see langword="false"/></c>), or to throw an exception on failure (<c><see langword="true"/></c>)</param>
	/// <returns>The current <see cref="Sdl.Builder"/> so that additional calls can be chained</returns>
	/// <remarks>
	/// <para>
	/// You can optionally provide metadata about your app to SDL. This is not required, but strongly encouraged.
	/// </para>
	/// <para>
	/// The URL maybe a product page, or a storefront, or even a GitHub repository, for user's further information.
	/// </para>
	/// <para>
	/// In contrast to most of the remaining API which uses the <c>Try</c>-method pattern, this method intentionally fails silently, if <paramref name="throwOnFailure"/> is <c><see langword="false"/></c>,
	/// or fails by throwing an exception, if <paramref name="throwOnFailure"/> is <c><see langword="true"/></c>.
	/// If you want to handle failures wrap the call to this method with <paramref name="throwOnFailure"/> set to <c><see langword="true"/></c> in a <c><see langword="try"/></c>-block,
	/// and check <see cref="Error.TryGet(out string?)"/> for more information when <c><see langword="catch"/></c>ing a <see cref="SdlException"/>.
	/// </para>
	/// </remarks>
	/// <inheritdoc cref="SetMetadata(Sdl.Builder, string, string?, bool)"/>
	public static Sdl.Builder SetAppUrl(this Sdl.Builder builder, string? appUrl, bool throwOnFailure = false)
		=> builder.SetMetadata(Sdl.Metadata.Url, appUrl, throwOnFailure);

	/// <summary>
	/// Sets the version of the app
	/// </summary>
	/// <param name="builder">The <see cref="Sdl.Builder"/> to use</param>
	/// <param name="appVersion">The version of the app</param>
	/// <param name="throwOnFailure">A value indicating whether to fail silently (<c><see langword="false"/></c>), or to throw an exception on failure (<c><see langword="true"/></c>)</param>
	/// <returns>The current <see cref="Sdl.Builder"/> so that additional calls can be chained</returns>
	/// <remarks>
	/// <para>
	/// You can optionally provide metadata about your app to SDL. This is not required, but strongly encouraged.
	/// </para>
	/// <para>
	/// There are no rules on format, so <c>"1.0.3beta2"</c> and <c>"April 22nd, 2024"</c> and a git hash are all valid options.
	/// </para>
	/// <para>
	/// In contrast to most of the remaining API which uses the <c>Try</c>-method pattern, this method intentionally fails silently, if <paramref name="throwOnFailure"/> is <c><see langword="false"/></c>,
	/// or fails by throwing an exception, if <paramref name="throwOnFailure"/> is <c><see langword="true"/></c>.
	/// If you want to handle failures wrap the call to this method with <paramref name="throwOnFailure"/> set to <c><see langword="true"/></c> in a <c><see langword="try"/></c>-block,
	/// and check <see cref="Error.TryGet(out string?)"/> for more information when <c><see langword="catch"/></c>ing a <see cref="SdlException"/>.
	/// </para>
	/// </remarks>
	/// <inheritdoc cref="SetMetadata(Sdl.Builder, string, string?, bool)"/>
	public static Sdl.Builder SetAppVersion(this Sdl.Builder builder, string? appVersion, bool throwOnFailure = false)
		=> builder.SetMetadata(Sdl.Metadata.Version, appVersion, throwOnFailure);

	/// <summary>
	/// Sets metadata about your app
	/// </summary>
	/// <param name="builder">The <see cref="Sdl.Builder"/> to use</param>
	/// <param name="name">The name of the metadata</param>
	/// <param name="value">The value of the metadata, or <c><see langword="null"/></c> to remove that metadata</param>
	/// <param name="throwOnFailure">A value indicating whether to fail silently (<c><see langword="false"/></c>), or to throw an exception on failure (<c><see langword="true"/></c>)</param>
	/// <returns>The current <see cref="Sdl.Builder"/> so that additional calls can be chained</returns>
	/// <remarks>
	/// <para>
	/// You can optionally provide metadata about your app to SDL. This is not required, but strongly encouraged.
	/// </para>
	/// <para>
	/// There are several locations where SDL can make use of metadata (an "About" box in the macOS menu bar, the name of the app can be shown on some audio mixers, etc).
	/// Any piece of metadata can be left out, if a specific detail doesn't make sense for the app.
	/// </para>
	/// <para>
	/// See <see cref="Sdl.Metadata"/> for a overview over the available metadata properties and their meanings.
	/// </para>
	/// <para>
	/// Multiple calls to this method with the same <paramref name="name"/> value are allowed, but various state might not change once it has been already set up.
	/// </para>
	/// <para>
	/// In contrast to most of the remaining API which uses the <c>Try</c>-method pattern, this method intentionally fails silently, if <paramref name="throwOnFailure"/> is <c><see langword="false"/></c>,
	/// or fails by throwing an exception, if <paramref name="throwOnFailure"/> is <c><see langword="true"/></c>.
	/// If you want to handle failures wrap the call to this method with <paramref name="throwOnFailure"/> set to <c><see langword="true"/></c> in a <c><see langword="try"/></c>-block,
	/// and check <see cref="Error.TryGet(out string?)"/> for more information when <c><see langword="catch"/></c>ing a <see cref="SdlException"/>.
	/// </para>
	/// </remarks>
	/// <exception cref="SdlException">Couldn't set the metadata (check <see cref="Error.TryGet(out string?)"/> for more information); only when <paramref name="throwOnFailure"/> is <c><see langword="true"/></c></exception>
	/// <inheritdoc cref="Sdl.Builder.TrySetMetadata(string, string?)"/>
	public static Sdl.Builder SetMetadata(this Sdl.Builder builder, string name, string? value, bool throwOnFailure = false)
	{
		if (!builder.TrySetMetadata(name, value) && throwOnFailure)
		{
			failCouldNotSetMetadata(name);
		}

		return builder;

		[DoesNotReturn]
		static void failCouldNotSetMetadata(string name) => throw new SdlException($"Could not set the metadata \"{name}\"");
	}

	/// <summary>
	/// Sets basic metadata about your app
	/// </summary>
	/// <param name="builder">The <see cref="Sdl.Builder"/> to use</param>
	/// <param name="appName">The name of the application (<c>"My Game 2: Bad Guy's Revenge!"</c>)</param>
	/// <param name="appVersion">The version of the application (<c>"1.0.0beta5"</c> or a git hash, or whatever makes sense)</param>
	/// <param name="appIdentifier">A unique string in reverse-domain format that identifies this app (<c>"com.example.mygame2"</c>)</param>
	/// <param name="throwOnFailure">A value indicating whether to fail silently (<c><see langword="false"/></c>), or to throw an exception on failure (<c><see langword="true"/></c>)</param>
	/// <returns>The current <see cref="Sdl.Builder"/> so that additional calls can be chained</returns>
	/// <remarks>
	/// <para>
	/// You can optionally provide metadata about your app to SDL. This is not required, but strongly encouraged.
	/// </para>
	/// <para>
	/// There are several locations where SDL can make use of metadata (an "About" box in the macOS menu bar, the name of the app can be shown on some audio mixers, etc).
	/// Any piece of metadata can be left out as a <c><see langword="null"/></c> value, if a specific detail doesn't make sense for the app.
	/// </para>
	/// <para>
	/// Passing a <c><see langword="null"/></c> value removes any previous metadata.
	/// </para>
	/// <para>
	/// Multiple calls to this method are allowed, but various state might not change once it has been already set up.
	/// </para>
	/// <para>
	/// This is a simplified interface for the most important information. You can supply significantly more detailed metadata with <see cref="SetMetadata(Sdl.Builder, string, string?, bool)"/>.
	/// </para>
	/// <para>
	/// In contrast to most of the remaining API which uses the <c>Try</c>-method pattern, this method intentionally fails silently, if <paramref name="throwOnFailure"/> is <c><see langword="false"/></c>,
	/// or fails by throwing an exception, if <paramref name="throwOnFailure"/> is <c><see langword="true"/></c>.
	/// If you want to handle failures wrap the call to this method with <paramref name="throwOnFailure"/> set to <c><see langword="true"/></c> in a <c><see langword="try"/></c>-block,
	/// and check <see cref="Error.TryGet(out string?)"/> for more information when <c><see langword="catch"/></c>ing a <see cref="SdlException"/>.
	/// </para>
	/// </remarks>
	/// <exception cref="SdlException">Couldn't set the metadata (check <see cref="Error.TryGet(out string?)"/> for more information); only when <paramref name="throwOnFailure"/> is <c><see langword="true"/></c></exception>
	/// <inheritdoc cref="Sdl.Builder.TrySetMetadata(string?, string?, string?)"/>
	public static Sdl.Builder SetMetadata(this Sdl.Builder builder, string? appName, string? appVersion, string? appIdentifier, bool throwOnFailure = false)
	{
		if (!builder.TrySetMetadata(appName, appVersion, appIdentifier) && throwOnFailure)
		{
			failCouldNotSetMetadata();
		}

		return builder;

		[DoesNotReturn]
		static void failCouldNotSetMetadata() => throw new SdlException($"Could not set the metadata");
	}
}
