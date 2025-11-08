namespace Sdl3Sharp.IO;

/// <summary>
/// Represents a method that will be called for each entry in a directory during enumeration
/// </summary>
/// <param name="directoryName">The directory that is being enumerated. Can be the empty string when enumerating the root of a <see cref="Storage"/> container.</param>
/// <param name="entryName">The name of the entry being enumerated</param>
/// <returns>
/// How to proceed with the enumeration:
/// <list type="bullet">
///		<item>
///			<term><see cref="EnumerationResult.Failure"/></term>
///			<description>Terminate the enumeration with an error</description>
///		</item>
///		<item>
///			<term><see cref="EnumerationResult.Success"/></term>
///			<description>Terminate the enumeration with success</description>
///		</item>
///		<item>
///			<term><see cref="EnumerationResult.Continue"/></term>
///			<description>Continue with the enumeration</description>
///		</item>
/// </list>
/// </returns>
/// <remarks>
/// <para>
/// Enumeration of directory entries will continue until either all entries have been enumerated, or the enumeration was requested to terminate.
/// </para>
/// <para>
/// <paramref name="directoryName"/> is guaranteed to end with a directory separator character (<c>'\\'</c> on Windows; <c>'/'</c> on most other platforms) if it's not the empty string.
/// It can be the empty string (<c>""</c>) when enumerating the root of a <see cref="Storage"/> container.
/// </para>
/// </remarks>
public delegate EnumerationResult EnumerateDirectoryCallback(string directoryName, string entryName);
