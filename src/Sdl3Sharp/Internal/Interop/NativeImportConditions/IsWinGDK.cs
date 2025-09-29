using Sdl3Sharp.SourceGeneration;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Internal.Interop.NativeImportConditions;

internal sealed class IsWinGDK : INativeImportCondition
{
	private IsWinGDK() { }

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool Evaluate() => INativeImportCondition.Evaluate<AndAlso<IsGDK, IsWindows>>();
}
