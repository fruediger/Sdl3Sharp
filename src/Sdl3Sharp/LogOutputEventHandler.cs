namespace Sdl3Sharp;

/// <summary>
/// Represents a method that handles logging messages (e.g. prints them)
/// </summary>
/// <param name="category">The category of the logging message</param>
/// <param name="priority">The priority of the logging message</param>
/// <param name="message">The message to be logged</param>
/// <param name="eventArgs">A <see cref="LogOutputEventArgs"/> that contains the mutable event data</param>
public delegate void LogOutputEventHandler(LogCategory category, LogPriority priority, string message, LogOutputEventArgs eventArgs);
