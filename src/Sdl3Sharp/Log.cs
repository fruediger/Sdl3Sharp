using Sdl3Sharp.Ffi;
using Sdl3Sharp.Internal;
using System;
using System.Runtime.CompilerServices;
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
	/// <remarks>
	/// <para>
	/// The <paramref name="message"/> parameter is treated as a C-style <c>printf</c> format string. 
	/// This means that any <c>%</c> characters are interpreted as format specifiers. 
	/// To include a literal <c>%</c> character in the output, you must escape it by writing <c>%%</c>.
	/// </para>
	/// </remarks>
	public static void Critical(LogCategory category, string message)
	{
		unsafe
		{
			var messageUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(message);

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
	/// Logs a <paramref name="format"/> string with <see cref="LogPriority.Critical"/>
	/// </summary>
	/// <param name="category">The category of the message</param>
	/// <param name="format">The C-style <c>printf</c> format string</param>
	/// <param name="args">The arguments corresponding to the format specifiers in <paramref name="format"/></param>
	/// <remarks>
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
	/// Consider using <see cref="Critical(LogCategory, string)"/> instead when possible, as it may be more efficient. 
	/// In many cases, you can use C# string interpolation to construct the message before logging.
	/// </para>
	/// </remarks>
	public static void Critical(LogCategory category, string format, params ReadOnlySpan<object> args)
		=> Variadic.Invoke(in SDL_LogCritical_var(), 2, [category, format, ..args]);

	/// <summary>
	/// Logs a <paramref name="message"/> with <see cref="LogPriority.Debug"/>
	/// </summary>
	/// <param name="category">The category of the message</param>
	/// <param name="message">The message to be logged</param>
	/// <remarks>
	/// <para>
	/// The <paramref name="message"/> parameter is treated as a C-style <c>printf</c> format string. 
	/// This means that any <c>%</c> characters are interpreted as format specifiers. 
	/// To include a literal <c>%</c> character in the output, you must escape it by writing <c>%%</c>.
	/// </para>
	/// </remarks>
	public static void Debug(LogCategory category, string message)
	{
		unsafe
		{
			var messageUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(message);

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
	/// Logs a <paramref name="format"/> string with <see cref="LogPriority.Debug"/>
	/// </summary>
	/// <param name="category">The category of the message</param>
	/// <param name="format">The C-style <c>printf</c> format string</param>
	/// <param name="args">The arguments corresponding to the format specifiers in <paramref name="format"/></param>
	/// <remarks>
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
	/// Consider using <see cref="Debug(LogCategory, string)"/> instead when possible, as it may be more efficient. 
	/// In many cases, you can use C# string interpolation to construct the message before logging.
	/// </para>
	/// </remarks>
	public static void Debug(LogCategory category, string format, params ReadOnlySpan<object> args)
		=> Variadic.Invoke(in SDL_LogDebug_var(), 2, [category, format, ..args]);

	/// <summary>
	/// Logs a <paramref name="message"/> with <see cref="LogPriority.Error"/>
	/// </summary>
	/// <param name="category">The category of the message</param>
	/// <param name="message">The message to be logged</param>
	/// <remarks>
	/// <para>
	/// The <paramref name="message"/> parameter is treated as a C-style <c>printf</c> format string. 
	/// This means that any <c>%</c> characters are interpreted as format specifiers. 
	/// To include a literal <c>%</c> character in the output, you must escape it by writing <c>%%</c>.
	/// </para>
	/// </remarks>
	public static void Error(LogCategory category, string message)
	{
		unsafe
		{
			var messageUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(message);

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
	/// Logs a <paramref name="format"/> string with <see cref="LogPriority.Error"/>
	/// </summary>
	/// <param name="category">The category of the message</param>
	/// <param name="format">The C-style <c>printf</c> format string</param>
	/// <param name="args">The arguments corresponding to the format specifiers in <paramref name="format"/></param>
	/// <remarks>
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
	/// Consider using <see cref="Error(LogCategory, string)"/> instead when possible, as it may be more efficient. 
	/// In many cases, you can use C# string interpolation to construct the message before logging.
	/// </para>
	/// </remarks>
	public static void Error(LogCategory category, string format, params ReadOnlySpan<object> args)
		=> Variadic.Invoke(in SDL_LogError_var(), 2, [category, format, ..args]);

	/// <summary>
	/// Logs a <paramref name="message"/> with <see cref="LogCategory.Application"/> and <see cref="LogPriority.Info"/>
	/// </summary>
	/// <param name="message">The message to be logged</param>
	/// <remarks>
	/// <para>
	/// The <paramref name="message"/> parameter is treated as a C-style <c>printf</c> format string. 
	/// This means that any <c>%</c> characters are interpreted as format specifiers. 
	/// To include a literal <c>%</c> character in the output, you must escape it by writing <c>%%</c>.
	/// </para>
	/// </remarks>
	public static void Info(string message)
	{
		unsafe
		{
			var messageUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(message);

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
	/// Logs a <paramref name="format"/> string with <see cref="LogCategory.Application"/> and <see cref="LogPriority.Info"/>
	/// </summary>
	/// <param name="format">The C-style <c>printf</c> format string</param>
	/// <param name="args">The arguments corresponding to the format specifiers in <paramref name="format"/></param>
	/// <remarks>
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
	/// Consider using <see cref="Info(string)"/> instead when possible, as it may be more efficient. 
	/// In many cases, you can use C# string interpolation to construct the message before logging.
	/// </para>
	/// </remarks>
	public static void Info(string format, params ReadOnlySpan<object> args)
		=> Variadic.Invoke(in SDL_Log_var(), 1, [format, ..args]);

	/// <summary>
	/// Logs a <paramref name="message"/> with <see cref="LogPriority.Info"/>
	/// </summary>
	/// <param name="category">The category of the message</param>
	/// <param name="message">The message to be logged</param>
	/// <remarks>
	/// <para>
	/// The <paramref name="message"/> parameter is treated as a C-style <c>printf</c> format string. 
	/// This means that any <c>%</c> characters are interpreted as format specifiers. 
	/// To include a literal <c>%</c> character in the output, you must escape it by writing <c>%%</c>.
	/// </para>
	/// </remarks>
	public static void Info(LogCategory category, string message)
	{
		unsafe
		{
			var messageUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(message);

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
	/// Logs a <paramref name="format"/> string with <see cref="LogPriority.Info"/>
	/// </summary>
	/// <param name="category">The category of the message</param>
	/// <param name="format">The C-style <c>printf</c> format string</param>
	/// <param name="args">The arguments corresponding to the format specifiers in <paramref name="format"/></param>
	/// <remarks>
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
	/// Consider using <see cref="Info(LogCategory, string)"/> instead when possible, as it may be more efficient. 
	/// In many cases, you can use C# string interpolation to construct the message before logging.
	/// </para>
	/// </remarks>
	public static void Info(LogCategory category, string format, params ReadOnlySpan<object> args)
		=> Variadic.Invoke(in SDL_LogInfo_var(), 2, [category, format, ..args]);

	/// <summary>
	/// Logs a <paramref name="message"/> with a specific <paramref name="category"/> and <paramref name="priority"/>
	/// </summary>
	/// <param name="category">The category of the message</param>
	/// <param name="priority">The priority of the message</param>
	/// <param name="message">The message to be logged</param>
	/// <remarks>
	/// <para>
	/// The <paramref name="message"/> parameter is treated as a C-style <c>printf</c> format string. 
	/// This means that any <c>%</c> characters are interpreted as format specifiers. 
	/// To include a literal <c>%</c> character in the output, you must escape it by writing <c>%%</c>.
	/// </para>
	/// </remarks>
	public static void Message(LogCategory category, LogPriority priority, string message)
	{
		unsafe
		{
			var messageUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(message);

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
	/// Logs a <paramref name="format"/> string with a specific <paramref name="category"/> and <paramref name="priority"/>
	/// </summary>
	/// <param name="category">The category of the message</param>
	/// <param name="priority">The priority of the message</param>
	/// <param name="format">The C-style <c>printf</c> format string</param>
	/// <param name="args">The arguments corresponding to the format specifiers in <paramref name="format"/></param>
	/// <remarks>
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
	/// Consider using <see cref="Message(LogCategory, LogPriority, string)"/> instead when possible, as it may be more efficient. 
	/// In many cases, you can use C# string interpolation to construct the message before logging.
	/// </para>
	/// </remarks>
	public static void Message(LogCategory category, LogPriority priority, string format, params ReadOnlySpan<object> args)
		=> Variadic.Invoke(in SDL_LogMessage_var(), 3, [category, priority, format, ..args]);

	/// <summary>
	/// Logs a <paramref name="message"/> with <see cref="LogPriority.Trace"/>
	/// </summary>
	/// <param name="category">The category of the message</param>
	/// <param name="message">The message to be logged</param>
	/// <remarks>
	/// <para>
	/// The <paramref name="message"/> parameter is treated as a C-style <c>printf</c> format string. 
	/// This means that any <c>%</c> characters are interpreted as format specifiers. 
	/// To include a literal <c>%</c> character in the output, you must escape it by writing <c>%%</c>.
	/// </para>
	/// </remarks>
	public static void Trace(LogCategory category, string message)
	{
		unsafe
		{
			var messageUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(message);

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
	/// Logs a <paramref name="format"/> string with <see cref="LogPriority.Trace"/>
	/// </summary>
	/// <param name="category">The category of the message</param>
	/// <param name="format">The C-style <c>printf</c> format string</param>
	/// <param name="args">The arguments corresponding to the format specifiers in <paramref name="format"/></param>
	/// <remarks>
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
	/// Consider using <see cref="Trace(LogCategory, string)"/> instead when possible, as it may be more efficient. 
	/// In many cases, you can use C# string interpolation to construct the message before logging.
	/// </para>
	/// </remarks>
	public static void Trace(LogCategory category, string format, params ReadOnlySpan<object> args)
		=> Variadic.Invoke(in SDL_LogTrace_var(), 2, [category, format, ..args]);

	/// <summary>
	/// Logs a <paramref name="message"/> with <see cref="LogPriority.Verbose"/>
	/// </summary>
	/// <param name="category">The category of the message</param>
	/// <param name="message">The message to be logged</param>
	/// <remarks>
	/// <para>
	/// The <paramref name="message"/> parameter is treated as a C-style <c>printf</c> format string. 
	/// This means that any <c>%</c> characters are interpreted as format specifiers. 
	/// To include a literal <c>%</c> character in the output, you must escape it by writing <c>%%</c>.
	/// </para>
	/// </remarks>
	public static void Verbose(LogCategory category, string message)
	{
		unsafe
		{
			var messageUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(message);

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
	/// Logs a <paramref name="format"/> string with <see cref="LogPriority.Verbose"/>
	/// </summary>
	/// <param name="category">The category of the message</param>
	/// <param name="format">The C-style <c>printf</c> format string</param>
	/// <param name="args">The arguments corresponding to the format specifiers in <paramref name="format"/></param>
	/// <remarks>
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
	/// Consider using <see cref="Verbose(LogCategory, string)"/> instead when possible, as it may be more efficient. 
	/// In many cases, you can use C# string interpolation to construct the message before logging.
	/// </para>
	/// </remarks>
	public static void Verbose(LogCategory category, string format, params ReadOnlySpan<object> args)
		=> Variadic.Invoke(in SDL_LogVerbose_var(), 2, [category, format, ..args]);

	/// <summary>
	/// Logs a <paramref name="message"/> with <see cref="LogPriority.Warn"/>
	/// </summary>
	/// <param name="category">The category of the message</param>
	/// <param name="message">The message to be logged</param>
	/// <remarks>
	/// <para>
	/// The <paramref name="message"/> parameter is treated as a C-style <c>printf</c> format string. 
	/// This means that any <c>%</c> characters are interpreted as format specifiers. 
	/// To include a literal <c>%</c> character in the output, you must escape it by writing <c>%%</c>.
	/// </para>
	/// </remarks>
	public static void Warn(LogCategory category, string message)
	{
		unsafe
		{
			var messageUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(message);

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

	/// <summary>
	/// Logs a <paramref name="format"/> string with <see cref="LogPriority.Warn"/>
	/// </summary>
	/// <param name="category">The category of the message</param>
	/// <param name="format">The C-style <c>printf</c> format string</param>
	/// <param name="args">The arguments corresponding to the format specifiers in <paramref name="format"/></param>
	/// <remarks>
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
	/// Consider using <see cref="Warn(LogCategory, string)"/> instead when possible, as it may be more efficient. 
	/// In many cases, you can use C# string interpolation to construct the message before logging.
	/// </para>
	/// </remarks>
	public static void Warn(LogCategory category, string format, params ReadOnlySpan<object> args)
		=> Variadic.Invoke(in SDL_LogWarn_var(), 2, [category, format, ..args]);

	internal static unsafe void SetSDLOutputCallback() => SDL_SetLogOutputFunction(&LogOutputCallback, null);
}
