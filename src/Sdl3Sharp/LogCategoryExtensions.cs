using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp;

/// <summary>
/// Provides extension methods and properties for <see cref="LogCategory"/>
/// </summary>
public static partial class LogCategoryExtensions
{
	extension(LogCategory)
	{
		/// <summary>
		/// Gets a custom log category
		/// </summary>
		/// <param name="customValue">The custom value of the resulting log category; used to identify the log category</param>
		/// <returns>A custom log category identified by <c><paramref name="customValue"/></c></returns>
		/// <exception cref="ArgumentOutOfRangeException"><c><paramref name="customValue"/></c> is less than <c>0</c> or not less than <c>2147483628</c></exception>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static LogCategory Custom(int customValue)
		{
#pragma warning disable CS0618 // Here's one of the few places we're allowed to use this (internally)
			if (customValue is < 0 or >= int.MaxValue - (int)LogCategory.Custom)
			{
				failCustomValueArgumentOutOfRange();
			}

			return unchecked(LogCategory.Custom + customValue);

			[DoesNotReturn]
			static void failCustomValueArgumentOutOfRange() => throw new ArgumentOutOfRangeException(nameof(customValue));
#pragma warning restore CS0618
		}

		/// <summary>
		/// Resets the <see cref="LogPriority">priorities</see> of all <see cref="LogCategory">log categories</see> to their default value
		/// </summary>
		/// <remarks>
		/// <para>
		/// This is called by <see cref="Sdl.Dispose"/>
		/// </para>
		/// </remarks>
		public static void ResetPriorityForAll() => SDL_ResetLogPriorities();

		/// <summary>
		/// Sets the <see cref="LogPriority">priorities</see> of all <see cref="LogCategory">log categories</see> to a specific value
		/// </summary>
		/// <param name="priority">The priority to assign</param>
		public static void SetPriorityForAll(LogPriority priority) => SDL_SetLogPriorities(priority);
	}

	extension(LogCategory category)
	{
		/// <summary>
		/// Gets or sets the priority of this log category
		/// </summary>
		/// <value>
		/// The priority of this log category
		/// </value>
		public LogPriority Priority
		{
			get => SDL_GetLogPriority(category);
			set => SDL_SetLogPriority(category, value);
		}

		/// <summary>
		/// Logs a <paramref name="message"/> with this log category and <see cref="LogPriority.Critical"/>
		/// </summary>
		/// <param name="message">The message to be logged</param>
		/// <remarks>
		/// <para>
		/// The <paramref name="message"/> parameter is treated as a C-style <c>printf</c> format string. 
		/// This means that any <c>%</c> characters are interpreted as format specifiers. 
		/// To include a literal <c>%</c> character in the output, you must escape it by writing <c>%%</c>.
		/// </para>
		/// </remarks>
		public void LogCritical(string message) => Log.Critical(category, message);

		/// <summary>
		/// Logs a <paramref name="format"/> string with this log category and <see cref="LogPriority.Critical"/>
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
		/// Consider using <see cref="LogCritical(LogCategory, string)"/> instead when possible, as it may be more efficient. 
		/// In many cases, you can use C# string interpolation to construct the message before logging.
		/// </para>
		/// </remarks>
		public void LogCritical(string format, params ReadOnlySpan<object> args) => Log.Critical(category, format, args);

		/// <summary>
		/// Logs a <paramref name="message"/> with this log category and <see cref="LogPriority.Debug"/>
		/// </summary>
		/// <param name="message">The message to be logged</param>
		/// <remarks>
		/// <para>
		/// The <paramref name="message"/> parameter is treated as a C-style <c>printf</c> format string. 
		/// This means that any <c>%</c> characters are interpreted as format specifiers. 
		/// To include a literal <c>%</c> character in the output, you must escape it by writing <c>%%</c>.
		/// </para>
		/// </remarks>
		public void LogDebug(string message) => Log.Debug(category, message);

		/// <summary>
		/// Logs a <paramref name="format"/> string with this log category and <see cref="LogPriority.Debug"/>
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
		/// Consider using <see cref="LogDebug(LogCategory, string)"/> instead when possible, as it may be more efficient. 
		/// In many cases, you can use C# string interpolation to construct the message before logging.
		/// </para>
		/// </remarks>
		public void LogDebug(string format, params ReadOnlySpan<object> args) => Log.Debug(category, format, args);

		/// <summary>
		/// Logs a <paramref name="message"/> with this log category and <see cref="LogPriority.Error"/>
		/// </summary>
		/// <param name="message">The message to be logged</param>
		/// <remarks>
		/// <para>
		/// The <paramref name="message"/> parameter is treated as a C-style <c>printf</c> format string. 
		/// This means that any <c>%</c> characters are interpreted as format specifiers. 
		/// To include a literal <c>%</c> character in the output, you must escape it by writing <c>%%</c>.
		/// </para>
		/// </remarks>
		public void LogError(string message) => Log.Error(category, message);

		/// <summary>
		/// Logs a <paramref name="format"/> string with this log category and <see cref="LogPriority.Error"/>
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
		/// Consider using <see cref="LogError(LogCategory, string)"/> instead when possible, as it may be more efficient. 
		/// In many cases, you can use C# string interpolation to construct the message before logging.
		/// </para>
		/// </remarks>
		public void LogError(string format, params ReadOnlySpan<object> args) => Log.Error(category, format, args);

		/// <summary>
		/// Logs a <paramref name="message"/> with this log category and <see cref="LogPriority.Info"/>
		/// </summary>
		/// <param name="message">The message to be logged</param>
		/// <remarks>
		/// <para>
		/// The <paramref name="message"/> parameter is treated as a C-style <c>printf</c> format string. 
		/// This means that any <c>%</c> characters are interpreted as format specifiers. 
		/// To include a literal <c>%</c> character in the output, you must escape it by writing <c>%%</c>.
		/// </para>
		/// </remarks>
		public void LogInfo(string message) => Log.Info(category, message);

		/// <summary>
		/// Logs a <paramref name="format"/> string with this log category and <see cref="LogPriority.Info"/>
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
		/// Consider using <see cref="LogInfo(LogCategory, string)"/> instead when possible, as it may be more efficient. 
		/// In many cases, you can use C# string interpolation to construct the message before logging.
		/// </para>
		/// </remarks>
		public void LogInfo(string format, params ReadOnlySpan<object> args) => Log.Info(category, format, args);

		/// <summary>
		/// Logs a <paramref name="message"/> with this log category and a specific <paramref name="priority"/>
		/// </summary>
		/// <param name="priority">The priority of the message</param>
		/// <param name="message">The message to be logged</param>
		/// <remarks>
		/// <para>
		/// The <paramref name="message"/> parameter is treated as a C-style <c>printf</c> format string. 
		/// This means that any <c>%</c> characters are interpreted as format specifiers. 
		/// To include a literal <c>%</c> character in the output, you must escape it by writing <c>%%</c>.
		/// </para>
		/// </remarks>
		public void LogMessage(LogPriority priority, string message) => Log.Message(category, priority, message);

		/// <summary>
		/// Logs a <paramref name="format"/> string with this log category and a specific <paramref name="priority"/>
		/// </summary>
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
		/// Consider using <see cref="LogMessage(LogCategory, LogPriority, string)"/> instead when possible, as it may be more efficient. 
		/// In many cases, you can use C# string interpolation to construct the message before logging.
		/// </para>
		/// </remarks>
		public void LogMessage(LogPriority priority, string format, params ReadOnlySpan<object> args) => Log.Message(category, priority, format, args);

		/// <summary>
		/// Logs a <paramref name="message"/> with this log category and <see cref="LogPriority.Trace"/>
		/// </summary>
		/// <param name="message">The message to be logged</param>
		/// <remarks>
		/// <para>
		/// The <paramref name="message"/> parameter is treated as a C-style <c>printf</c> format string. 
		/// This means that any <c>%</c> characters are interpreted as format specifiers. 
		/// To include a literal <c>%</c> character in the output, you must escape it by writing <c>%%</c>.
		/// </para>
		/// </remarks>
		public void LogTrace(string message) => Log.Trace(category, message);

		/// <summary>
		/// Logs a <paramref name="format"/> string with this log category and <see cref="LogPriority.Trace"/>
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
		/// Consider using <see cref="LogTrace(LogCategory, string)"/> instead when possible, as it may be more efficient. 
		/// In many cases, you can use C# string interpolation to construct the message before logging.
		/// </para>
		/// </remarks>
		public void LogTrace(string format, params ReadOnlySpan<object> args) => Log.Trace(category, format, args);

		/// <summary>
		/// Logs a <paramref name="message"/> with this log category and <see cref="LogPriority.Verbose"/>
		/// </summary>
		/// <param name="message">The message to be logged</param>
		/// <remarks>
		/// <para>
		/// The <paramref name="message"/> parameter is treated as a C-style <c>printf</c> format string. 
		/// This means that any <c>%</c> characters are interpreted as format specifiers. 
		/// To include a literal <c>%</c> character in the output, you must escape it by writing <c>%%</c>.
		/// </para>
		/// </remarks>
		public void LogVerbose(string message) => Log.Verbose(category, message);

		/// <summary>
		/// Logs a <paramref name="format"/> string with this log category and <see cref="LogPriority.Verbose"/>
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
		/// Consider using <see cref="LogVerbose(LogCategory, string)"/> instead when possible, as it may be more efficient. 
		/// In many cases, you can use C# string interpolation to construct the message before logging.
		/// </para>
		/// </remarks>
		public void LogVerbose(string format, params ReadOnlySpan<object> args) => Log.Verbose(category, format, args);

		/// <summary>
		/// Logs a <paramref name="message"/> with this log category and <see cref="LogPriority.Warn"/>
		/// </summary>
		/// <param name="message">The message to be logged</param>
		/// <remarks>
		/// <para>
		/// The <paramref name="message"/> parameter is treated as a C-style <c>printf</c> format string. 
		/// This means that any <c>%</c> characters are interpreted as format specifiers. 
		/// To include a literal <c>%</c> character in the output, you must escape it by writing <c>%%</c>.
		/// </para>
		/// </remarks>
		public void LogWarn(string message) => Log.Warn(category, message);

		/// <summary>
		/// Logs a <paramref name="format"/> string with this log category and <see cref="LogPriority.Warn"/>
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
		/// Consider using <see cref="LogWarn(LogCategory, string)"/> instead when possible, as it may be more efficient. 
		/// In many cases, you can use C# string interpolation to construct the message before logging.
		/// </para>
		/// </remarks>
		public void LogWarn(string format, params ReadOnlySpan<object> args) => Log.Warn(category, format, args);

		/// <summary>
		/// Tries to get the custom value used to identify this custom log category
		/// </summary>
		/// <param name="customValue">The custom value used to identify this custom log category</param>
		/// <returns><c><see langword="true"/></c> if this instance represents a custom log category and <paramref name="customValue"/> is the custom value which identifies it; otherwise, <c><see langword="false"/></c></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public bool TryGetCustomValue(out int customValue)
		{
#pragma warning disable CS0618 // Here's one of the few places we're allowed to use this (internally)
			if (category is >= LogCategory.Custom)
			{
				customValue = unchecked(category - LogCategory.Custom);

				return true;
			}

			customValue = default;

			return false;
#pragma warning restore CS0618
		}
	}
}
