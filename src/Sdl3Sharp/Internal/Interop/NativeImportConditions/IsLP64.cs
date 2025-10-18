using Sdl3Sharp.SourceGeneration;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Internal.Interop.NativeImportConditions;

internal sealed class IsLP64 : INativeImportCondition
{
	private IsLP64() { }

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static bool INativeImportCondition.Evaluate() => INativeImportCondition.Evaluate<AndAlso<Is64BitProcess, Not<IsWindows>>>(); // currently Windows seems to be the only supported platform using the LLP64 data model
}
