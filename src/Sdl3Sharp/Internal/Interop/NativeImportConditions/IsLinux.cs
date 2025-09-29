using Sdl3Sharp.SourceGeneration;
using System;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Internal.Interop.NativeImportConditions;

internal sealed class IsLinux : INativeImportCondition
{
	private IsLinux() { }

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static bool INativeImportCondition.Evaluate() => OperatingSystem.IsLinux();
}
