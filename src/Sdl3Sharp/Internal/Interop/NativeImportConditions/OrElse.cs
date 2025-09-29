using Sdl3Sharp.SourceGeneration;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Internal.Interop.NativeImportConditions;

internal sealed class OrElse<TLeftCondition, TRightCondition> : INativeImportCondition
	where TLeftCondition : INativeImportCondition
	where TRightCondition : INativeImportCondition
{
	private OrElse() { }

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool Evaluate() => INativeImportCondition.Evaluate<TLeftCondition>() || INativeImportCondition.Evaluate<TRightCondition>();
}
