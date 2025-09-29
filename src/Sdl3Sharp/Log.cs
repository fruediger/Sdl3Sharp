using System.Runtime.InteropServices.Marshalling;

namespace Sdl3Sharp;

/// <summary>
/// Provides simple logging routines with messages using <see cref="LogPriority">priorities</see> and <see cref="LogCategory">categories</see>
/// </summary>
public static partial class Log
{
	/// <summary>
	/// Gets or sets a value that indicates whether unhandled logging messages should get passed to the default SDL logging output
	/// </summary>
	/// <value>
	/// A value that indicates whether unhandled logging messages should get passed to the default SDL logging output
	/// </value>
	public static bool UseDefaultOutputForUnhandledMessages { get; set; } = true;

	/// <summary>
	/// Raised for each logging message to be handled
	/// </summary>
	public static event LogOutputEventHandler? Output;

	/// <summary>
	/// Logs a <paramref name="message"/> with <see cref="LogPriority.Critical"/>
	/// </summary>
	/// <param name="category">The category of the message</param>
	/// <param name="message">The message to be logged</param>
	public static void Critical(LogCategory category, string message)
	{
		unsafe
		{
			var messageUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(message.Replace("%", "%%"));

			try
			{
				SDL_LogCritical(category, messageUtf8);
			}
			finally
			{
				Utf8StringMarshaller.Free(messageUtf8);
			}
		}
	}

	/// <summary>
	/// Logs a <paramref name="message"/> with <see cref="LogPriority.Debug"/>
	/// </summary>
	/// <param name="category">The category of the message</param>
	/// <param name="message">The message to be logged</param>
	public static void Debug(LogCategory category, string message)
	{
		unsafe
		{
			var messageUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(message.Replace("%", "%%"));

			try
			{
				SDL_LogDebug(category, messageUtf8);
			}
			finally
			{
				Utf8StringMarshaller.Free(messageUtf8);
			}
		}
	}

	/// <summary>
	/// Logs a <paramref name="message"/> with <see cref="LogPriority.Error"/>
	/// </summary>
	/// <param name="category">The category of the message</param>
	/// <param name="message">The message to be logged</param>
	public static void Error(LogCategory category, string message)
	{
		unsafe
		{
			var messageUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(message.Replace("%", "%%"));

			try
			{
				SDL_LogError(category, messageUtf8);
			}
			finally
			{
				Utf8StringMarshaller.Free(messageUtf8);
			}
		}
	}

	/// <summary>
	/// Logs a <paramref name="message"/> with <see cref="LogCategory.Application"/> and <see cref="LogPriority.Info"/>
	/// </summary>
	/// <param name="message">The message to be logged</param>
	public static void Info(string message)
	{
		unsafe
		{
			var messageUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(message.Replace("%", "%%"));

			try
			{
				SDL_Log(messageUtf8);
			}
			finally
			{
				Utf8StringMarshaller.Free(messageUtf8);
			}
		}
	}

	/// <summary>
	/// Logs a <paramref name="message"/> with <see cref="LogPriority.Info"/>
	/// </summary>
	/// <param name="category">The category of the message</param>
	/// <param name="message">The message to be logged</param>
	public static void Info(LogCategory category, string message)
	{
		unsafe
		{
			var messageUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(message.Replace("%", "%%"));

			try
			{
				SDL_LogInfo(category, messageUtf8);
			}
			finally
			{
				Utf8StringMarshaller.Free(messageUtf8);
			}
		}
	}

	/// <summary>
	/// Logs a <paramref name="message"/> with a specific <paramref name="category"/> and <paramref name="priority"/>
	/// </summary>
	/// <param name="category">The category of the message</param>
	/// <param name="priority">The priority of the message</param>
	/// <param name="message">The message to be logged</param>
	public static void Message(LogCategory category, LogPriority priority, string message)
	{
		unsafe
		{
			var messageUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(message.Replace("%", "%%"));

			try
			{
				SDL_LogMessage(category, priority, messageUtf8);
			}
			finally
			{
				Utf8StringMarshaller.Free(messageUtf8);
			}
		}
	}

	/// <summary>
	/// Logs a <paramref name="message"/> with <see cref="LogPriority.Trace"/>
	/// </summary>
	/// <param name="category">The category of the message</param>
	/// <param name="message">The message to be logged</param>
	public static void Trace(LogCategory category, string message)
	{
		unsafe
		{
			var messageUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(message.Replace("%", "%%"));

			try
			{
				SDL_LogTrace(category, messageUtf8);
			}
			finally
			{
				Utf8StringMarshaller.Free(messageUtf8);
			}
		}
	}

	/// <summary>
	/// Logs a <paramref name="message"/> with <see cref="LogPriority.Verbose"/>
	/// </summary>
	/// <param name="category">The category of the message</param>
	/// <param name="message">The message to be logged</param>
	public static void Verbose(LogCategory category, string message)
	{
		unsafe
		{
			var messageUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(message.Replace("%", "%%"));

			try
			{
				SDL_LogVerbose(category, messageUtf8);
			}
			finally
			{
				Utf8StringMarshaller.Free(messageUtf8);
			}
		}
	}

	/// <summary>
	/// Logs a <paramref name="message"/> with <see cref="LogPriority.Warn"/>
	/// </summary>
	/// <param name="category">The category of the message</param>
	/// <param name="message">The message to be logged</param>
	public static void Warn(LogCategory category, string message)
	{
		unsafe
		{
			var messageUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(message.Replace("%", "%%"));

			try
			{
				SDL_LogWarn(category, messageUtf8);
			}
			finally
			{
				Utf8StringMarshaller.Free(messageUtf8);
			}
		}
	}

	internal static unsafe void SetSDLOutputCallback() => SDL_SetLogOutputFunction(&LogOutputCallback, null);
}
