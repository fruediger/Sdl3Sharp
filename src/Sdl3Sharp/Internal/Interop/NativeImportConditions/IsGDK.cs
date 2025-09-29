using Sdl3Sharp.SourceGeneration;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Internal.Interop.NativeImportConditions;

internal sealed class IsGDK : INativeImportCondition
{
	private IsGDK() { }

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static bool INativeImportCondition.Evaluate()
#if ENABLE_GDK
		=> true;
#else
		=> false; // there's currently no way to build for the MS Game Development Kit
#endif
}
