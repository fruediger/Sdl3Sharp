using System.Runtime.CompilerServices;

namespace Sdl3Sharp;

partial struct LogPriority
{
	/// <summary>
	/// Gets the log priority <c>Invalid</c>
	/// </summary>
	/// <value>
	/// The log priority <c>Invalid</c>
	/// </value>
	/// <remarks>
	/// The log priority <c>Invalid</c> is not a real log priority. Instead it is used to indicate invalid logging messages which should not get logged.
	/// </remarks>
	public static LogPriority Invalid { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Invalid); }

	/// <summary>
	/// Gets the log priority <c>Trace</c>
	/// </summary>
	/// <value>
	/// The log priority <c>Trace</c>
	/// </value>
	public static LogPriority Trace { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Trace); }

	/// <summary>
	/// Gets the log priority <c>Verbose</c>
	/// </summary>
	/// <value>
	/// The log priority <c>Verbose</c>
	/// </value>
	public static LogPriority Verbose { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Verbose); }

	/// <summary>
	/// Gets the log priority <c>Debug</c>
	/// </summary>
	/// <value>
	/// The log priority <c>Debug</c>
	/// </value>
	public static LogPriority Debug { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Debug); }

	/// <summary>
	/// Gets the log priority <c>Info</c>
	/// </summary>
	/// <value>
	/// The log priority <c>Info</c>
	/// </value>
	public static LogPriority Info { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Info); }

	/// <summary>
	/// Gets the log priority <c>Warn</c>
	/// </summary>
	/// <value>
	/// The log priority <c>Warn</c>
	/// </value>
	public static LogPriority Warn { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Warn); }

	/// <summary>
	/// Gets the log priority <c>Error</c>
	/// </summary>
	/// <value>
	/// The log priority <c>Error</c>
	/// </value>
	public static LogPriority Error { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Error); }

	/// <summary>
	/// Gets the log priority <c>Critical</c>
	/// </summary>
	/// <value>
	/// The log priority <c>Critical</c>
	/// </value>
	public static LogPriority Critical { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Critical); }
}
