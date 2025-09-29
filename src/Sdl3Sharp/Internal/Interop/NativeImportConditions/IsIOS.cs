using Sdl3Sharp.SourceGeneration;
using System;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Internal.Interop.NativeImportConditions;

internal sealed class IsIOS : INativeImportCondition
{
	private IsIOS() { }

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static bool INativeImportCondition.Evaluate() => OperatingSystem.IsIOS();
}
