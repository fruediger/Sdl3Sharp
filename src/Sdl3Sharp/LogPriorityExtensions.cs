using System.Runtime.InteropServices.Marshalling;

namespace Sdl3Sharp;

/// <summary>
/// Provides extension methods for <see cref="LogPriority"/>
/// </summary>
public static partial class LogPriorityExtensions
{
	extension(LogPriority priority)
	{
		/// <summary>
		/// Tries to set the text prepended to logging messages of a this log priority
		/// </summary>
		/// <param name="prefix">The prefix to use for this log priority, or <c><see langword="null"/></c> to use no prefix</param>
		/// <returns><c><see langword="true"/></c> if the prefix was successfully set for this log priority; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
		public bool TrySetPrefix(string? prefix)
		{
			unsafe
			{
				var prefixUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(prefix);

				try
				{
					return SDL_SetLogPriorityPrefix(priority, prefixUtf8);
				}
				finally
				{
					Utf8StringMarshaller.Free(prefixUtf8);
				}
			}
		}
	}
}
