using System;
using System.Collections.Immutable;
using System.Runtime.InteropServices.Marshalling;

namespace Sdl3Sharp.Video.Windowing.Drivers;

/// <summary>
/// Represents a windowing driver used to specify the windowing backend in <see cref="Window{TDriver}"/> and <see cref="Display{TDriver}"/>
/// </summary>
/// <remarks>
/// <para>
/// There are some pre-defined windowing drivers that SDL comes with.
/// Not all of them are necessarily available in every environment.
/// You can check the <see cref="AvailableDriverNames"/> property to see which windowing drivers are available in the current environment.
/// The windowing drivers that SDL comes with are:
/// <list type="bullet">
///		<item>
///			<term><see cref="Cocoa"/></term>
///			<description>Cocoa backend (only available on Apple platforms)</description>
///		</item>
///		<item>
///			<term><see cref="X11"/></term>
///			<description>X11 backend (only available on platforms that support the X11 display server protocol)</description>
///		</item>
///		<item>
///			<term><see cref="Wayland"/></term>
///			<description>Wayland backend (only available on platforms that support the Wayland display server protocol)</description>
///		</item>
///		<item>
///			<term><see cref="Vivante"/></term>
///			<description>Vivante EGL backend</description>
///		</item>
///		<item>
///			<term><see cref="Windows"/></term>
///			<description>Windows backend (only available on Windows platforms)</description>
///		</item>
///		<item>
///			<term><see cref="Haiku"/></term>
///			<description>Haiku backend (only available on Haiku OS platforms)</description>
///		</item>
///		<item>
///			<term><see cref="UIKit"/></term>
///			<description>UIKit backend (only available on Apple platforms)</description>
///		</item>
///		<item>
///			<term><see cref="Android"/></term>
///			<description>Android backend (only available on Android platforms)</description>
///		</item>
///		<item>
///			<term><see cref="PlayStation2"/></term>
///			<description>Sony PlayStation 2 backend</description>
///		</item>
///		<item>
///			<term><see cref="PlayStationPortable"/></term>
///			<description>Sony PlayStation Portable backend</description>
///		</item>
///		<item>
///			<term><see cref="PlayStationVita"/></term>
///			<description>Sony PlayStation Vita backend</description>
///		</item>
///		<item>
///			<term><see cref="N3DS"/></term>
///			<description>Nintendo 3DS backend</description>
///		</item>
///		<item>
///			<term><see cref="NGage"/></term>
///			<description>Nokia N-Gage backend</description>
///		</item>
///		<item>
///			<term><see cref="KmsDrm"/></term>
///			<description>Linux KMS/DRM backend (only available on Linux platforms with <see href="https://www.kernel.org/doc/html/latest/gpu/introduction.html">Kernel Mode Setting (KMS) and Direct Rendering Manager (DRM)</see> support)</description>
///		</item>
///		<item>
///			<term><see cref="RiscOS"/></term>
///			<description>RISC OS backend (only available on RISC OS platforms)</description>
///		</item>
///		<item>
///			<term><see cref="RaspberryPi"/></term>
///			<description>Raspberry Pi backend (only available on Raspberry Pi platforms)</description>
///		</item>
///		<item>
///			<term><see cref="Emscripten"/></term>
///			<description>Emscripten backend (only available on platforms that support Emscripten)</description>
///		</item>
///		<item>
///			<term><see cref="Qnx"/></term>
///			<description>QNX backend (only available on QNX platforms)</description>
///		</item>
///		<item>
///			<term><see cref="Offscreen"/></term>
///			<description>"Offscreen-video" backend</description>
///		</item>
///		<item>
///			<term><see cref="Dummy"/></term>
///			<description>"Dummy video" (sometimes called "null video") backend</description>
///		</item>
///		<item>
///			<term><see cref="DummyEvdev"/></term>
///			<description>"Dummy video" (sometimes called "null video") backend with <see href="https://en.wikipedia.org/wiki/Evdev">evdev</see> (only available on platforms that support <see href="https://en.wikipedia.org/wiki/Evdev">evdev</see>)</description>
///		</item>
///	</list>
///	Please note that some of these windowing drivers require platforms that .NET doesn't run on (yet). These drivers are currently unsupported (e.g. <see cref="NGage"/>, etc.).
///	</para>
/// </remarks>
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
	/// You can use the value of the <see cref="Name"/> properties of individual windowing drivers types (e.g. <see cref="Windows.Name"/>) to check for the availability of a certain windowing driver;
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
