using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp;

partial struct LogCategory
{
	/// <summary>
	/// Gets the log category <c>Application</c>
	/// </summary>
	/// <value>
	/// The log category <c>Application</c>
	/// </value>
	public static LogCategory Application { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Application); }

	/// <summary>
	/// Gets the log category <c>Error</c>
	/// </summary>
	/// <value>
	/// The log category <c>Error</c>
	/// </value>
	public static LogCategory Error { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Error); }

	/// <summary>
	/// Gets the log category <c>Assert</c>
	/// </summary>
	/// <value>
	/// The log category <c>Assert</c>
	/// </value>
	public static LogCategory Assert { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Assert); }

	/// <summary>
	/// Gets the log category <c>System</c>
	/// </summary>
	/// <value>
	/// The log category <c>System</c>
	/// </value>
	public static LogCategory System { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.System); }

	/// <summary>
	/// Gets the log category <c>Audio</c>
	/// </summary>
	/// <value>
	/// The log category <c>Audio</c>
	/// </value>
	public static LogCategory Audio { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Audio); }

	/// <summary>
	/// Gets the log category <c>Video</c>
	/// </summary>
	/// <value>
	/// The log category <c>Video</c>
	/// </value>
	public static LogCategory Video { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Video); }

	/// <summary>
	/// Gets the log category <c>Render</c>
	/// </summary>
	/// <value>
	/// The log category <c>Render</c>
	/// </value>
	public static LogCategory Render { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Render); }

	/// <summary>
	/// Gets the log category <c>Input</c>
	/// </summary>
	/// <value>
	/// The log category <c>Input</c>
	/// </value>
	public static LogCategory Input { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Input); }

	/// <summary>
	/// Gets the log category <c>Test</c>
	/// </summary>
	/// <value>
	/// The log category <c>Test</c>
	/// </value>
	public static LogCategory Test { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Test); }

	/// <summary>
	/// Gets the log category <c>Gpu</c>
	/// </summary>
	/// <value>
	/// The log category <c>Gpu</c>
	/// </value>
	public static LogCategory Gpu { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Gpu); }

	/// <summary>
	/// Gets a custom log category
	/// </summary>
	/// <param name="customValue">The custom value of the resulting log category; used to identify the log category</param>
	/// <returns>A custom log category identified by <c><paramref name="customValue"/></c></returns>
	/// <exception cref="ArgumentOutOfRangeException"><c><paramref name="customValue"/></c> is less than <c>0</c> or not less than <c>2147483628</c></exception>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static LogCategory Custom(int customValue)
	{
		if (customValue is < 0 or >= int.MaxValue - (int)Kind.Custom)
		{
			failCustomValueArgumentOutOfRange();
		}

		return new(unchecked(Kind.Custom + customValue));

		[DoesNotReturn]
		static void failCustomValueArgumentOutOfRange() => throw new ArgumentOutOfRangeException(nameof(customValue));
	}
}
