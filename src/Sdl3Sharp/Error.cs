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
	/// This method does not do any memory allocation on the native side
	/// </remarks>
	public static bool OutOfMemory() => SDL_OutOfMemory();

	/// <summary>
	/// Sets the SDL error message for the current thread
	/// </summary>
	/// <param name="message">The error message to be set</param>
	/// <returns><c><see langword="false"/></c> (always)</returns>
	/// <remarks>
	/// Calling this method will replace any previous error message that was set for this thread
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
	/// Tries to retrieve a message about the last error that occurred on the current thread
	/// </summary>
	/// <param name="message">A message with information about the specific error that occurred, or <c><see langword="null"/></c> or an empty string if there hasn't been an error message set since the last call to <see cref="Clear"/></param>
	/// <returns><c><see langword="true"/></c> if there was a non-<c><see langword="null"/></c> and non-empty error <paramref name="message"/>; otherwise <c><see langword="false"/></c></returns>
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

			return !string.IsNullOrEmpty(message);
		}
	}
}
