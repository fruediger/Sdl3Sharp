using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Input;

/// <summary>
/// Provides extension methods for <see cref="MouseButtonFlags"/>
/// </summary>
public static class MouseButtonFlagsExtensions
{
	extension(MouseButtonFlags flags)
	{
		/// <summary>
		/// Determines whether the <see cref="MouseButtonFlags"/> contains the specified <see cref="MouseButton"/>.
		/// </summary>
		/// <param name="button">The <see cref="MouseButton"/> to check for</param>
		/// <returns><c><see langword="true"/></c>, if the <see cref="MouseButtonFlags"/> contains the specified <see cref="MouseButton"/>; otherwise, <c><see langword="false"/></c></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public bool HasButton(MouseButton button) => unchecked((flags & button.AsFlags()) is not 0);

		/// <summary>
		/// Determines whether the <see cref="MouseButtonFlags"/> contains all of the specified <see cref="MouseButtonFlags"/>
		/// </summary>
		/// <param name="buttonFlags">The <see cref="MouseButtonFlags"/> to check for</param>
		/// <returns><c><see langword="true"/></c>, if the <see cref="MouseButtonFlags"/> contains all of the specified <see cref="MouseButtonFlags"/>; otherwise, <c><see langword="false"/></c></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public bool HasAllButtons(MouseButtonFlags buttonFlags) => unchecked((flags & buttonFlags) == buttonFlags);

		/// <summary>
		/// Determines whether the <see cref="MouseButtonFlags"/> contains any of the specified <see cref="MouseButtonFlags"/>
		/// </summary>
		/// <param name="buttonFlags">The <see cref="MouseButtonFlags"/> to check for</param>
		/// <returns><c><see langword="true"/></c>, if the <see cref="MouseButtonFlags"/> contains any of the specified <see cref="MouseButtonFlags"/>; otherwise, <c><see langword="false"/></c></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public bool HasAnyButtons(MouseButtonFlags buttonFlags) => unchecked((flags & buttonFlags) is not 0);
	}
}
