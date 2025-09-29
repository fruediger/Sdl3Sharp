using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Internal;

internal sealed class WeakReferenceEqualityComparer<T> : IEqualityComparer<WeakReference<T>>
	where T : class
{
	public static readonly WeakReferenceEqualityComparer<T> Instance = new();

	public static bool Equals(WeakReference<T>? x, WeakReference<T>? y)
		=> ReferenceEquals(x, y)
		|| x?.TryGetTarget(out var xTarget) is true
			&& y?.TryGetTarget(out var yTarget) is true
			&& ReferenceEquals(xTarget, yTarget)
		;

	public static int GetHashCode(WeakReference<T>? obj)
		=> obj?.TryGetTarget(out var target) is true
			? RuntimeHelpers.GetHashCode(target)
			: RuntimeHelpers.GetHashCode(obj);

	bool IEqualityComparer<WeakReference<T>>.Equals(WeakReference<T>? x, WeakReference<T>? y) => Equals(x, y);

	int IEqualityComparer<WeakReference<T>>.GetHashCode(WeakReference<T>? obj) => GetHashCode(obj);
}
