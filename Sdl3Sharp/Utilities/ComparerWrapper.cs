using System;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Utilities;

internal abstract class ComparerWrapper
{
	internal unsafe abstract int Compare(void* a, void* b);
}

internal sealed class ComparerWrapper<T>(Comparer<T> comparer) : ComparerWrapper, IDisposable
	where T : unmanaged
{
	private Comparer<T>? mComparer = comparer;

	internal override unsafe int Compare(void* a, void* b)
	{
		if (a is not null && b is not null && mComparer is not null)
		{
			return mComparer(ref Unsafe.AsRef<T>(a), ref Unsafe.AsRef<T>(b));
		}

		return default;
	}
	
	public void Dispose()
	{
		mComparer = null;
	}
}
