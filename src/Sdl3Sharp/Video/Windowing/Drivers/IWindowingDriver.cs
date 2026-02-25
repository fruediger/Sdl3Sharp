using System;
using System.Collections.Immutable;
using System.Runtime.InteropServices.Marshalling;

namespace Sdl3Sharp.Video.Windowing.Drivers;

public partial interface IWindowingDriver
{
	// See Sdl3Sharp.Video.Rendering.Drivers.IRenderingDriver for more information about why this interface is designed this way and why we don't need something like CRTP here.

	// The available windowing driver list never changes because it's hard-compiled into the loaded build of SDL,
	// that's why we can cache it statically here
	private static ImmutableArray<string>? mAvailableDriverNames;

	/// <summary>
	/// Gets the list of names of available windowing drivers in the loaded build of SDL
	/// </summary>
	/// <value>
	/// The list of names of available windowing drivers in the loaded build of SDL
	/// </value>
	/// <remarks>
	/// <para>
	/// The list of available windowing drivers is only retrieved once and then cached afterwards,
	/// so the value of this property won't change during the lifetime of the application.
	/// </para>
	/// <para>
	/// The list is in the order that windowing drivers are normally checked during the initialization of SDL's <see cref="SubSystems.Video">video subsystem</see>.
	/// </para>
	/// <para>
	/// The names of all pre-defined windowing drivers are all simple, low-ASCII identifiers, like "cocoa", "x11" or "windows".
	/// These never have Unicode characters, and are not meant to be proper names.
	/// </para>
	/// <para>
	/// You can use the value of the <see cref="Name"/> properties of individual windowing drivers types (e.g. <see cref="Direct3D11.Name"/>) to check for the availability of a certain windowing driver;
	/// alternatively you can check the <see cref="WindowingDriverExtensions.get_IsAvailable{TDriver}"/> property for that.
	/// </para>
	/// <para>
	/// This property should only be accessed from the main thread.
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
					var count = SDL_GetNumVideoDrivers();
					var builder = ImmutableArray.CreateBuilder<string>(count);

					for (var i = 0; i < count; i++)
					{
						builder.Add(Utf8StringMarshaller.ConvertToManaged(SDL_GetVideoDriver(i))!);
					}

					return builder.ToImmutable();
				}
			}
		}
	}

	/// <summary>
	/// Gets the name of the currently initialized windowing driver
	/// </summary>
	/// <value>
	/// The name of the currently initialized windowing driver, or <c><see langword="null"/></c> if no windowing driver has been initialized (e.g. SDL's <see cref="SubSystems.Video">video subsystem</see> is not initialized)
	/// </value>
	/// <remarks>
	/// <para>
	/// The value of this property is contained in the list of <see cref="AvailableDriverNames"/>, if it's not <c><see langword="null"/></c>;
	/// otherwise, it means no windowing driver has been initialized (e.g. SDL's <see cref="SubSystems.Video">video subsystem</see> is not initialized).
	/// </para>
	/// <para>
	/// This property should only be accessed from the main thread.
	/// </para>
	/// </remarks>
	public static string? CurrentDriverName
	{
		get
		{
			unsafe
			{
				return Utf8StringMarshaller.ConvertToManaged(SDL_GetCurrentVideoDriver());
			}
		}
	}

	/// <summary>
	/// Gets the name of the windowing driver
	/// </summary>
	/// <value>
	/// The name of the windowing driver, or <c><see langword="null"/></c> if the driver doesn't have a name (although all pre-defined windowing drivers do have a non-<c><see langword="null"/></c> name)
	/// </value>
	/// <remarks>
	/// <para>
	/// The names of all pre-defined windowing drivers are all simple, low-ASCII identifiers, like "cocoa", "x11" or "windows".
	/// These never have Unicode characters, and are not meant to be proper names.
	/// </para>
	/// </remarks>
	static abstract string? Name { get; }

	internal static abstract ReadOnlySpan<byte> NameAscii { get; }
}
