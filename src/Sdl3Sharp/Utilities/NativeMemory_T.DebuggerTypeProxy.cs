using System.Diagnostics;

namespace Sdl3Sharp.Utilities;

[DebuggerTypeProxy(typeof(NativeMemory<>.DebuggerTypeProxy))]
partial struct NativeMemory<T>
{
	private sealed class DebuggerTypeProxy(NativeMemory<T> nativeMemory)
	{
		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
		public T[] Values
		{
			get
			{
				const uint maxElements = 256;

				try
				{
					return [..nativeMemory.Slice(0, System.Math.Min(nativeMemory.Length, maxElements))];
				}
				catch
				{
					return [];
				}
			}
		}
	}
}
