using Sdl3Sharp.Video.Windowing;
using System;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices.Marshalling;

namespace Sdl3Sharp.Video.Rendering.Drivers;

// It's 2026 and we still can't use preprocessor directives in XML documentation, so we have to duplicate the entire documentation just for a tiny addition in SDL 3.4.
// This is frustrating.
// Although, there are two *unanswered* issues about this on the csharplang repo ("unanswered" in this context means what they are tagged as in GitHub, there's still some discussion in the comments),
// https://github.com/dotnet/csharplang/discussions/295 and https://github.com/dotnet/csharplang/discussions/7567, from 2017 and 2023 respectively.

#if SDL3_4_0_OR_GREATER
/// <summary>
/// Represents a rendering driver used to specify the rendering backend in <see cref="Renderer{TDriver}"/> and <see cref="Texture{TDriver}"/>
/// </summary>
/// <remarks>
/// <para>
/// There are some pre-defined rendering driver that SDL comes with.
/// Not all of them are necessarily available in every environment.
/// You can check the <see cref="AvailableDriverNames"/> property to see which rendering drivers are available in the current environment.
/// The rendering drivers that SDL comes with are:
/// <list type="bullet">
///		<item>
///			<term><see cref="Direct3D11"/></term>
///			<description>Direct3D 11 backend (only available on Windows)</description>
///		</item>
///		<item>
///			<term><see cref="Direct3D12"/></term>
///			<description>Direct3D 12 backend (only available on Windows)</description>
///		</item>
///		<item>
///			<term><see cref="Direct3D9"/></term>
///			<description>Direct3D 9 backend (only available on Windows)</description>
///		</item>
///		<item>
///			<term><see cref="Metal"/></term>
///			<description>Metal backend (only available on macOS and iOS)</description>
///		</item>
///		<item>
///			<term><see cref="NGage"/></term>
///			<description>Nokia N-Gage backend (not supported; experimental only)</description>
///		</item>
///		<item>
///			<term><see cref="OpenGl"/></term>
///			<description>OpenGL backend</description>
///		</item>
///		<item>
///			<term><see cref="OpenGlEs2"/></term>
///			<description>OpenGL for Embedded Systems 2 backend (primarily only available on mobile and embedded platforms, but may be available on some desktop platforms as well)</description>
///		</item>
///		<item>
///			<term><see cref="PlayStation2"/></term>
///			<description>Sony PlayStation 2 backend (not supported; experimental only)</description>
///		</item>
///		<item>
///			<term><see cref="PlayStationPortable"/></term>
///			<description>Sony PlayStation Portable backend (not supported; experimental only)</description>
///		</item>
///		<item>
///			<term><see cref="PlayStationVita"/></term>
///			<description>Sony PlayStation Vita backend (not supported; experimental only)</description>
///		</item>
///		<item>
///			<term><see cref="Vulkan"/></term>
///			<description>Vulkan backend</description>
///		</item>
///		<item>
///			<term><see cref="Gpu"/></term>
///			<description>GPU backend (only available on platforms that support the SDL GPU render API, which are primarly platforms that support either <see cref="Vulkan"/>, <see cref="Metal"/>, or <see cref="Direct3D12"/> as well)</description>
///		</item>
///		<item>
///			<term><see cref="Software"/></term>
///			<description>Software rendering backend (supported almost everywhere; not hardware-accelerated; needed when rendering should be done to a <see cref="Surface"/> directly instead of to a <see cref="Window"/> or to an off-screen render target)</description>
///		</item>
/// </list>
/// Please note that some of these rendering drivers require platforms that .NET doesn't run on (yet). These drivers are currently unsupported and marked as <see cref="ExperimentalAttribute">experimental</see> (e.g. <see cref="NGage"/>, <see cref="PlayStation2"/>, <see cref="PlayStationPortable"/>, and <see cref="PlayStationVita"/>).
/// </para>
/// </remarks>
#else
/// <summary>
/// Represents a rendering driver used to specify the rendering backend in <see cref="Renderer{TDriver}"/> and <see cref="Texture{TDriver}"/>
/// </summary>
/// <remarks>
/// <para>
/// There are some pre-defined rendering driver that SDL comes with.
/// Not all of them are necessarily available in every environment.
/// You can check the <see cref="AvailableDriverNames"/> property to see which rendering drivers are available in the current environment.
/// The rendering drivers that SDL comes with are:
/// <list type="bullet">
///		<item>
///			<term><see cref="Direct3D11"/></term>
///			<description>Direct3D 11 backend (only available on Windows)</description>
///		</item>
///		<item>
///			<term><see cref="Direct3D12"/></term>
///			<description>Direct3D 12 backend (only available on Windows)</description>
///		</item>
///		<item>
///			<term><see cref="Direct3D9"/></term>
///			<description>Direct3D 9 backend (only available on Windows)</description>
///		</item>
///		<item>
///			<term><see cref="Metal"/></term>
///			<description>Metal backend (only available on macOS and iOS)</description>
///		</item>
///		<item>
///			<term><see cref="NGage"/></term>
///			<description>Nokia N-Gage backend (not supported; experimental only)</description>
///		</item>
///		<item>
///			<term><see cref="OpenGl"/></term>
///			<description>OpenGL backend</description>
///		</item>
///		<item>
///			<term><see cref="OpenGlEs2"/></term>
///			<description>OpenGL for Embedded Systems 2 backend (primarily only available on mobile and embedded platforms, but may be available on some desktop platforms as well)</description>
///		</item>
///		<item>
///			<term><see cref="PlayStation2"/></term>
///			<description>Sony PlayStation 2 backend (not supported; experimental only)</description>
///		</item>
///		<item>
///			<term><see cref="PlayStationPortable"/></term>
///			<description>Sony PlayStation Portable backend (not supported; experimental only)</description>
///		</item>
///		<item>
///			<term><see cref="PlayStationVita"/></term>
///			<description>Sony PlayStation Vita backend (not supported; experimental only)</description>
///		</item>
///		<item>
///			<term><see cref="Vulkan"/></term>
///			<description>Vulkan backend</description>
///		</item>
///		<item>
///			<term><see cref="Software"/></term>
///			<description>Software rendering backend (supported almost everywhere; not hardware-accelerated; needed when rendering should be done to a <see cref="Surface"/> directly instead of to a <see cref="Window"/> or to an off-screen render target)</description>
///		</item>
/// </list>
/// Please note that some of these rendering drivers require platforms that .NET doesn't run on (yet). These drivers are currently unsupported and marked as <see cref="ExperimentalAttribute">experimental</see> (e.g. <see cref="NGage"/>, <see cref="PlayStation2"/>, <see cref="PlayStationPortable"/>, and <see cref="PlayStationVita"/>).
/// </para>
/// </remarks>
#endif
public partial interface IDriver
{
	// We don't need to use CRTP or something like that here, just to prevent IDriver from being used as a generic type argument
	// IDriver contains a "static abstract" member, which is already enough to prevent it from being used as such,
	// since static abstract members are required to be implemented by type arguments, so only type that implement IDriver can be used as type arguments.
	// The only real issue with that is that a user would get a weird or confusing compile-time error message, if they try to use IDriver directly as a type argument,
	// but on the other hand, IDriver contains an "internal" member to be implemented, and if an user would try to implement IDriver themselves,
	// they would get an even more weird or confusing compile-time error message.
	// So we live with that and call it a "feature by design".

	// The available rendering driver list never changes during the lifetime of the application,
	// that's why we can cache it statically here
	private static ImmutableArray<string>? mAvailableDriverNames;

	/// <summary>
	/// Gets the list of names of available rendering drivers in the current environment
	/// </summary>
	/// <value>
	/// The list of names of available rendering drivers in the current environment
	/// </value>
	/// <remarks>
	/// <para>
	/// The list of available rendering drivers is only retrieved once and then cached afterwards,
	/// so the value of this property won't change during the lifetime of the application.
	/// </para>
	/// <para>
	/// The list is in the order that rendering drivers are normally initialized by default; the drivers that seem more reasonable to choose first (as far as the SDL developers believe) are earlier in the list.
	/// </para>
	/// <para>
	/// The names of all pre-defined rendering drivers are all simple, low-ASCII identifiers, like "opengl", "direct3d12" or "metal".
	/// These never have Unicode characters, and are not meant to be proper names.
	/// </para>
	/// <para>
	/// You can use the value of the <see cref="Name"/> properties of individual rendering drivers types (e.g. <see cref="Direct3D11.Name"/>) to check for the availability of a certain rendering driver;
	/// alternatively you can check the <see cref="DriverExtensions.get_IsAvailable{TDriver}"/> property for that.
	/// </para>
	/// </remarks>
	public static ImmutableArray<string> AvailableDriverNames
	{
		get
		{
			return mAvailableDriverNames ??= buildAvailableDrivers();

			// build the available drivers list once
			static ImmutableArray<string> buildAvailableDrivers()
			{
				unsafe
				{
					var count = SDL_GetNumRenderDrivers();
					var builders = ImmutableArray.CreateBuilder<string>(count);

					for (var i = 0; i < count; i++)
					{
						builders.Add(Utf8StringMarshaller.ConvertToManaged(SDL_GetRenderDriver(i))!);
					}

					return builders.ToImmutable();
				}
			}
		}
	}

	/// <summary>
	/// Gets the name of the rendering driver
	/// </summary>
	/// <value>
	/// The name of the rendering driver, or <c><see langword="null"/></c> if the driver doesn't have a name (although all pre-defined rendering available to the user do have a non-<c><see langword="null"/></c> name)
	/// </value>
	/// <remarks>
	/// <para>
	/// The names of all pre-defined rendering drivers are all simple, low-ASCII identifiers, like <c>"opengl"</c>, <c>"direct3d12"</c> or <c>"metal"</c>.
	/// These never have Unicode characters, and are not meant to be proper names.
	/// </para>
	/// </remarks>
	static abstract string? Name { get; }

	internal static abstract ReadOnlySpan<byte> NameAscii { get; }
}
