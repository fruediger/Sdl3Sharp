using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sdl3Sharp.Utilities;

/// <summary>
/// Provides methods to sort or search in lists of items
/// </summary>
public static partial class Compare
{
	/// <summary>
	/// Performs a binary search on a <em>previously sorted</em> list of items
	/// </summary>
	/// <typeparam name="T">The type of items to search for</typeparam>
	/// <param name="key">A comparable key item equal to the item that is being searched for</param>
	/// <param name="array">The list of items to search in</param>
	/// <param name="comparer">A <see cref="Comparer{T}"/> delegate used to compare the items</param>
	/// <returns>The index into the <paramref name="array"/> where a matching item to the given <paramref name="key"/> was found, if it was found; otherwise, <c>-1</c></returns>
	/// <remarks>
	/// <para>
	/// The list of items (<paramref name="array"/>) must be previously sorted (e.g. by using <see cref="QSort{T}(T[], Comparer{T})"/>) or else the result will be unrelyable.
	/// </para>
	/// <para>
	/// Note: The result of this method is somewhat different to SDL's native equivalent.
	/// While <see href="https://wiki.libsdl.org/SDL3/SDL_bsearch">SDL_bsearch</see>() returns a pointer to the matching item into the given list or returns <c>NULL</c> when no match was found,
	/// this method returns the index to the matching item into the given list (<paramref name="array"/>) or returns <c>-1</c> when no match was found.
	/// </para>
	/// </remarks>
	/// <exception cref="ArgumentNullException">
	/// <paramref name="array"/> was <c><see langword="null"/></c>
	/// - or -
	/// <paramref name="comparer"/> was <c><see langword="null"/></c>
	/// </exception>
	public static int BSearch<T>(in T key, T[] array, Comparer<T> comparer)
		where T : unmanaged
	{
		if (array is null)
		{
			failArrayArgumentNull();
		}

		return BSearch(in key, array.AsSpan(), comparer);

		[DoesNotReturn]
		static void failArrayArgumentNull() => throw new ArgumentNullException(nameof(array));
	}

	/// <summary>
	/// Performs a binary search on a <em>previously sorted</em> list of items
	/// </summary>
	/// <typeparam name="T">The type of items to search for</typeparam>
	/// <param name="key">A comparable key item equal to the item that is being searched for</param>
	/// <param name="span">The list of items to search in</param>
	/// <param name="comparer">A <see cref="Comparer{T}"/> delegate used to compare the items</param>
	/// <returns>The index into the <paramref name="span"/> where a matching item to the given <paramref name="key"/> was found, if it was found; otherwise, <c>-1</c></returns>
	/// <remarks>
	/// <para>
	/// The list of items (<paramref name="span"/>) must be previously sorted (e.g. by using <see cref="QSort{T}(Span{T}, Comparer{T})"/>) or else the result will be unrelyable.
	/// </para>
	/// <para>
	/// Note: The result of this method is somewhat different to SDL's native equivalent.
	/// While <see href="https://wiki.libsdl.org/SDL3/SDL_bsearch">SDL_bsearch</see>() returns a pointer to the matching item into the given list or returns <c>NULL</c> when no match was found,
	/// this method returns the index to the matching item into the given list (<paramref name="span"/>) or returns <c>-1</c> when no match was found.
	/// </para>
	/// </remarks>
	/// <exception cref="ArgumentNullException"><paramref name="comparer"/> was <c><see langword="null"/></c></exception>
	public static int BSearch<T>(in T key, ReadOnlySpan<T> span, Comparer<T> comparer)
		where T : unmanaged
	{
		if (comparer is null)
		{
			failComparerArgumentNull();
		}

		var comparerWrapper = new ComparerWrapper<T>(comparer);
		var gcHandle = GCHandle.Alloc(comparerWrapper, GCHandleType.Normal);

		try
		{
			unsafe
			{
				fixed (T* keyPtr = &key, basePtr = span)
				{
					var resultPtr = (T*)SDL_bsearch_r(keyPtr, basePtr, unchecked((nuint)span.Length), unchecked((nuint)Unsafe.SizeOf<T>()), &CompareCallback, unchecked((void*)GCHandle.ToIntPtr(gcHandle)));

					if (resultPtr is null)
					{
						return -1;
					}

					return unchecked((int)(resultPtr - basePtr));
				}
			}
		}
		finally
		{
			gcHandle.Free();
			comparerWrapper.Dispose();
		}

		[DoesNotReturn]
		static void failComparerArgumentNull() => throw new ArgumentNullException(nameof(comparer));
	}

	/// <summary>
	/// Sorts a list of items
	/// </summary>
	/// <typeparam name="T">The type of items to sort</typeparam>
	/// <param name="array">The list of items to sort</param>
	/// <param name="comparer">A <see cref="Comparer{T}"/> delegate used to compare the items</param>
	/// <remarks>
	/// <para>
	/// After this returns, the contents of the <paramref name="array"/> are sorted in regards to the given <paramref name="comparer"/>.
	/// </para>
	/// </remarks>
	/// <exception cref="ArgumentNullException">
	/// <paramref name="array"/> was <c><see langword="null"/></c>
	/// - or -
	/// <paramref name="comparer"/> was <c><see langword="null"/></c>
	/// </exception>
	public static void QSort<T>(T[] array, Comparer<T> comparer)
		where T : unmanaged
	{
		if (array is null)
		{
			failArrayArgumentNull();
		}

		QSort(array.AsSpan(), comparer);

		[DoesNotReturn]
		static void failArrayArgumentNull() => throw new ArgumentNullException(nameof(array));
	}

	/// <summary>
	/// Sorts a list of items
	/// </summary>
	/// <typeparam name="T">The type of items to sort</typeparam>
	/// <param name="span">The list of items to sort</param>
	/// <param name="comparer">A <see cref="Comparer{T}"/> delegate used to compare the items</param>
	/// <remarks>
	/// <para>
	/// After this returns, the contents of the <paramref name="span"/> are sorted in regards to the given <paramref name="comparer"/>.
	/// </para>
	/// </remarks>
	/// <exception cref="ArgumentNullException"><paramref name="comparer"/> was <c><see langword="null"/></c></exception>
	public static void QSort<T>(Span<T> span, Comparer<T> comparer)
		where T : unmanaged
	{
		if (comparer is null)
		{
			failComparerArgumentNull();
		}

		var comparerWrapper = new ComparerWrapper<T>(comparer);
		var gcHandle = GCHandle.Alloc(comparerWrapper, GCHandleType.Normal);

		try
		{
			unsafe
			{
				fixed (T* basePtr = span)
				{
					SDL_qsort_r(basePtr, unchecked((nuint)span.Length), unchecked((nuint)Unsafe.SizeOf<T>()), &CompareCallback, unchecked((void*)GCHandle.ToIntPtr(gcHandle)));
				}
			}
		}
		finally
		{
			gcHandle.Free();
			comparerWrapper.Dispose();
		}

		[DoesNotReturn]
		static void failComparerArgumentNull() => throw new ArgumentNullException(nameof(comparer));
	}
}
