using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace Sdl3Sharp.Internal;

internal static class Shared
{
	[ThreadStatic] private static StringBuilder? mStringBuilder;

	public static StringBuilder StringBuilder { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mStringBuilder ??= new(); }
}
