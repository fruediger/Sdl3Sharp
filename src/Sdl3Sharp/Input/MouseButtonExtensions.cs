using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Input;

/// <summary>
/// Provides extension methods for <see cref="MouseButton"/>
/// </summary>
public static class MouseButtonExtensions
{
	extension(MouseButton)
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static MouseButtonFlags operator ~(MouseButton value) => ~value.AsFlags();

		// This doesn't make much sense:
		// public static MouseButtonFlags operator &(MouseButton left, MouseButton right);

		/// <summary>
		/// Combines a <see cref="MouseButton"/> and a <see cref="MouseButtonFlags"/> into a <see cref="MouseButtonFlags"/> using a bitwise AND operation
		/// </summary>
		/// <param name="left">The <see cref="MouseButton"/> to combine</param>
		/// <param name="right">The <see cref="MouseButtonFlags"/> to combine</param>
		/// <returns>The combined <see cref="MouseButtonFlags"/></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static MouseButtonFlags operator &(MouseButton left, MouseButtonFlags right) => unchecked(left.AsFlags() & right);

		/// <summary>
		/// Combines a <see cref="MouseButtonFlags"/> and a <see cref="MouseButton"/> into a <see cref="MouseButtonFlags"/> using a bitwise AND operation
		/// </summary>
		/// <param name="left">The <see cref="MouseButtonFlags"/> to combine</param>
		/// <param name="right">The <see cref="MouseButton"/> to combine</param>
		/// <returns>The combined <see cref="MouseButtonFlags"/></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static MouseButtonFlags operator &(MouseButtonFlags left, MouseButton right) => unchecked(left & right.AsFlags());

		/// <summary>
		/// Combines two <see cref="MouseButton"/> values into a <see cref="MouseButtonFlags"/> using a bitwise OR operation
		/// </summary>
		/// <param name="left">The first <see cref="MouseButton"/> to combine</param>
		/// <param name="right">The second <see cref="MouseButton"/> to combine</param>
		/// <returns>The combined <see cref="MouseButtonFlags"/></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static MouseButtonFlags operator |(MouseButton left, MouseButton right) => unchecked(left.AsFlags() | right.AsFlags());

		/// <summary>
		/// Combines a <see cref="MouseButton"/> and a <see cref="MouseButtonFlags"/> into a <see cref="MouseButtonFlags"/> using a bitwise OR operation
		/// </summary>
		/// <param name="left">The <see cref="MouseButton"/> to combine</param>
		/// <param name="right">The <see cref="MouseButtonFlags"/> to combine</param>
		/// <returns>The combined <see cref="MouseButtonFlags"/></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static MouseButtonFlags operator |(MouseButton left, MouseButtonFlags right) => unchecked(left.AsFlags() | right);

		/// <summary>
		/// Combines a <see cref="MouseButtonFlags"/> and a <see cref="MouseButton"/> into a <see cref="MouseButtonFlags"/> using a bitwise OR operation
		/// </summary>
		/// <param name="left">The <see cref="MouseButtonFlags"/> to combine</param>
		/// <param name="right">The <see cref="MouseButton"/> to combine</param>
		/// <returns>The combined <see cref="MouseButtonFlags"/></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static MouseButtonFlags operator |(MouseButtonFlags left, MouseButton right) => unchecked(left | right.AsFlags());

		// This doesn't make much sense:
		// public static MouseButtonFlags operator ^(MouseButton left, MouseButton right);

		/// <summary>
		/// Combines a <see cref="MouseButton"/> and a <see cref="MouseButtonFlags"/> into a <see cref="MouseButtonFlags"/> using a bitwise XOR operation
		/// </summary>
		/// <param name="left">The <see cref="MouseButton"/> to combine</param>
		/// <param name="right">The <see cref="MouseButtonFlags"/> to combine</param>
		/// <returns>The combined <see cref="MouseButtonFlags"/></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static MouseButtonFlags operator ^(MouseButton left, MouseButtonFlags right) => unchecked(left.AsFlags() ^ right);

		/// <summary>
		/// Combines a <see cref="MouseButtonFlags"/> and a <see cref="MouseButton"/> into a <see cref="MouseButtonFlags"/> using a bitwise XOR operation
		/// </summary>
		/// <param name="left">The <see cref="MouseButtonFlags"/> to combine</param>
		/// <param name="right">The <see cref="MouseButton"/> to combine</param>
		/// <returns>The combined <see cref="MouseButtonFlags"/></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static MouseButtonFlags operator ^(MouseButtonFlags left, MouseButton right) => unchecked(left ^ right.AsFlags());
	}

	extension(MouseButton button)
	{
		/// <summary>
		/// Converts the <see cref="MouseButton"/> to its corresponding <see cref="MouseButtonFlags"/>
		/// </summary>
		/// <returns>The corresponding <see cref="MouseButtonFlags"/> to the <see cref="MouseButton"/></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public MouseButtonFlags AsFlags() => unchecked((MouseButtonFlags)(1u << ((byte)button - 1)));
	}
}
