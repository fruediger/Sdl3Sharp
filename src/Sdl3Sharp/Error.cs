using Sdl3Sharp.Internal;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices.Marshalling;

namespace Sdl3Sharp;

/// <summary>
/// Provides simple error message routines for SDL
/// </summary>
public static partial class Error
{
	/// <summary>
	/// Clears any previous error message for this thread
	/// </summary>
	/// <returns><c><see langword="true"/></c> (always)</returns>
	public static bool Clear() => SDL_ClearError();

	/// <summary>
	/// Sets an error indicating that memory allocation failed
	/// </summary>
	/// <returns><c><see langword="false"/></c> (always)</returns>
	/// <remarks>
	/// <para>
	/// This method does not do any memory allocation on the native side.
	/// </para>
	/// </remarks>
	public static bool OutOfMemory() => SDL_OutOfMemory();

	/// <summary>
	/// Sets the SDL error message for the current thread
	/// </summary>
	/// <param name="message">The error message to be set</param>
	/// <returns><c><see langword="false"/></c> (always)</returns>
	/// <remarks>
	/// <para>
	/// Calling this method will replace any previous error message that was set for this thread.
	/// </para>
	/// </remarks>
	/// <example>
	/// This method always returns <c><see langword="false"/></c>; since SDL frequently uses <c><see langword="false"/></c> to signify a failing result, this leads to the following idiom:
	/// <code>
	/// if (/* condition indicating an error */)
	/// {
	///		return Error.Set("Error message example");
	/// }
	/// </code>
	/// </example>
	public static bool Set(string message)
	{
		unsafe
		{
			var messageUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(message);

			try
			{
				return SDL_SetError(messageUtf8);
			}
			finally
			{
				Utf8StringMarshaller.Free(messageUtf8);
			}
		}
	}

	/// <summary>
	/// Sets the SDL error message to a <paramref name="format"/> string for the current thread
	/// </summary>
	/// <param name="format">The C-style <c>printf</c> format string</param>
	/// <param name="args">The arguments corresponding to the format specifiers in <paramref name="format"/></param>
	/// <returns><c><see langword="false"/></c> (always)</returns>
	/// <remarks>
	/// <para>
	/// Calling this method will replace any previous error message that was set for this thread.
	/// </para>
	/// <para>
	/// The <paramref name="format"/> parameter is interpreted as a C-style <c>printf</c> format string, and 
	/// the <paramref name="args"/> parameter supplies the values for its format specifiers. Supported argument 
	/// types include all integral types up to 64-bit (including <c><see langword="bool"/></c> and <c><see langword="char"/></c>), 
	/// floating point types (<c><see langword="float"/></c> and <c><see langword="double"/></c>), pointer types 
	/// (<c><see cref="IntPtr"/></c> and <c><see cref="UIntPtr"/></c>), and <c><see langword="string"/></c>.
	/// </para>
	/// <para>
	/// For a detailed explanation of C-style <c>printf</c> format strings and their specifiers, see 
	/// <see href="https://en.wikipedia.org/wiki/Printf#Format_specifier"/>.
	/// </para>
	/// <para>
	/// Consider using <see cref="Set(string)"/> instead when possible, as it may be more efficient. 
	/// In many cases, you can use C# string interpolation to construct the message before logging.
	/// </para>
	/// </remarks>
	/// <example>
	/// This method always returns <c><see langword="false"/></c>; since SDL frequently uses <c><see langword="false"/></c> to signify a failing result, this leads to the following idiom:
	/// <code>
	/// if (/* condition indicating an error */)
	/// {
	///		return Error.Set("%s", "Error message example");
	/// }
	/// </code>
	/// </example>
	public static bool Set(string format, params ReadOnlySpan<object> args)
		=> Variadic.Invoke<bool>(in SDL_SetError_var(), 1, [format, .. args]);

	/// <summary>
	/// Tries to retrieve a message about the last error that occurred on the current thread
	/// </summary>
	/// <param name="message">A message with information about the specific error that occurred, or <c><see langword="null"/></c> if there hasn't been an error message set since the last call to <see cref="Clear"/></param>
	/// <returns><c><see langword="true"/></c> if there was a non-<c><see langword="null"/></c> error <paramref name="message"/>; otherwise <c><see langword="false"/></c></returns>
	/// <remarks>
	/// <para>
	/// It is possible for multiple errors to occur before calling <see cref="TryGet(out string?)"/>. Only the last error is returned.
	/// </para>
	/// <para>
	/// The message is only applicable when an SDL has signaled an error. You must check the return values of SDL calls to determine when to appropriately call <see cref="TryGet(out string?)"/>. You should <em>not</em> use the result of <see cref="TryGet(out string?)"/> to decide if an error has occurred! Sometimes SDL will set an error string even when reporting success.
	/// </para>
	/// <para>
	/// SDL will <em>not</em> clear the error message for successful API calls. You <em>must</em> check for failure cases before you can assume the error message applies.
	/// </para>
	/// <para>
	/// Error messages are set per-thread, so an error set in a different thread will not interfere with the current thread's operation.
	/// </para>
	/// </remarks>
	public static bool TryGet([NotNullWhen(true)] out string? message)
	{
		unsafe
		{
			message = Utf8StringMarshaller.ConvertToManaged(SDL_GetError());

			return message is not null;
		}
	}
}
