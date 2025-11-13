using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sdl3Sharp.Video.Coloring;

[DebuggerTypeProxy(typeof(DebuggerTypeProxy))]
partial class Palette
{
	private sealed class DebuggerTypeProxy(Palette palette)
	{
		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
		public Color<byte>[] Colors
		{
			get
			{
				unsafe
				{
					if (palette is null || palette.mPalette is null || palette.mPalette->Colors is null || palette.mPalette->NColors is not > 0)
					{
						return [];
					}

					var result = GC.AllocateUninitializedArray<Color<byte>>(palette.mPalette->NColors);

					MemoryMarshal.CreateReadOnlySpan(ref Unsafe.AsRef<Color<byte>>(palette.mPalette->Colors), palette.mPalette->NColors).CopyTo(result.AsSpan());

					return result;
				}
			}
		}
	}
}
