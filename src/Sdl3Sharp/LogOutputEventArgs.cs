namespace Sdl3Sharp;

/// <summary>
/// Provides mutable data for the <see cref="Log.Output"/> event
/// </summary>
public sealed class LogOutputEventArgs
{
	/// <summary>
	/// Gets or sets a value indicating whether a <see cref="LogOutputEventHandler"/> handled the logging message it was invoked with
	/// </summary>
	/// <value>
	/// A value indicating whether a <see cref="LogOutputEventHandler"/> handled its logging message
	/// </value>
	/// <remarks>
	/// <para>
	/// Inside a <see cref="LogOutputEventHandler"/>: set this propery to <c><see langword="true"/></c> to indicate that the <see cref="LogOutputEventHandler"/> handled the given logging message.
	/// </para>
	/// <para>
	/// It is possible that a <see cref="LogOutputEventHandler"/> is invoked with a logging message which was already handled by a previous <see cref="LogOutputEventHandler"/>.
	/// You can check for this inside a <see cref="LogOutputEventHandler"/> by examine if the value of <see cref="Handled"/> is already set to <c><see langword="true"/></c>.
	/// </para>
	/// <para>
	/// If a logging message remains unhandled after a completed raise of <see cref="Log.Output"/> (that is <see cref="Handled"/> is <c><see langword="false"/></c>)
	/// and <see cref="Log.UseDefaultOutputForUnhandledMessages"/> is set to <c><see langword="true"/></c>, the logging messages will be passed to the default SDL logging output.
	/// </para>
	/// </remarks>
	public bool Handled { get; set; } = false;
}
