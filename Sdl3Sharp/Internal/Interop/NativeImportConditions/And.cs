using Sdl3Sharp.SourceGeneration;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Internal.Interop.NativeImportConditions;

internal sealed class And<TLeftCondition, TRightCondition> : INativeImportCondition
	where TLeftCondition : INativeImportCondition
	where TRightCondition : INativeImportCondition
{
	private And() { }
	
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static bool INativeImportCondition.Evaluate() => INativeImportCondition.Evaluate<TLeftCondition>() & INativeImportCondition.Evaluate<TRightCondition>();
}
