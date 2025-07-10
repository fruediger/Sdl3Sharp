using Sdl3Sharp.SourceGeneration;
using System;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Internal.NativeImportConditions;

internal sealed class IsLLP64 : INativeImportCondition
{
	private IsLLP64() { }

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool Evaluate()
		=> OperatingSystem.IsWindows(); // currently Windows seems to be the only supported platform using the LLP64 data model
}
