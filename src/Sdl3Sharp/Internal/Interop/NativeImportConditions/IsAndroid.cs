using Sdl3Sharp.SourceGeneration;
using System;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Internal.Interop.NativeImportConditions;

internal sealed class IsAndroid : INativeImportCondition
{
	private IsAndroid() { }

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static bool INativeImportCondition.Evaluate() => OperatingSystem.IsAndroid();
}
