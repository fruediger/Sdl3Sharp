using Sdl3Sharp.SourceGeneration;
using System;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Internal.Interop.NativeImportConditions;

internal sealed class Is64BitProcess : INativeImportCondition
{
	private Is64BitProcess() { }

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static bool INativeImportCondition.Evaluate() => Environment.Is64BitProcess;
}
