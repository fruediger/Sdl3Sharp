using Sdl3Sharp.SourceGeneration;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Internal.Interop.NativeImportConditions;

internal sealed class IsLLP64 : INativeImportCondition
{
	private IsLLP64() { }

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static bool INativeImportCondition.Evaluate() => INativeImportCondition.Evaluate<IsWindows>(); // currently Windows seems to be the only supported platform using the LLP64 data model
}
