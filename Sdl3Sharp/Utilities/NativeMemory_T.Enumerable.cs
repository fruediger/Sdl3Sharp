using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Utilities;

partial struct NativeMemory<T> : IEnumerable<T>
{
	/// <summary>
	/// Enumerates references to the values of type <typeparamref name="T"/> in an <see cref="NativeMemory{T}">allocated memory buffer of type <typeparamref name="T"/></see>
	/// </summary>
	public struct Enumerator : IEnumerator<T>
	{
		private unsafe T* mCurrent;
		private unsafe readonly T* mEnd;
		private NativeMemoryPin? mPin;

		/// <summary>
		/// Creates a new <see cref="Enumerator"/> for a specified <see cref="NativeMemory{T}">allocated memory buffer of type <typeparamref name="T"/></see>
		/// </summary>
		/// <param name="nativeMemory">The <see cref="NativeMemory{T}">allocated memory buffer of type <typeparamref name="T"/></see> which should be enumerated</param>
		/// <remarks>
		/// <para>
		/// Note: This operation <see cref="Pin">pins</see> the given <paramref name="nativeMemory"/>.
		/// </para>
		/// </remarks> 
		public Enumerator(NativeMemory<T> nativeMemory)
		{
			unsafe
			{
				nativeMemory.Validate();

				mPin = nativeMemory.Pin();

				if (nativeMemory.TryGetNonNullStartPointer(out var start))
				{
					mCurrent = start - 1;
					mEnd = start + nativeMemory.Length;
				}
				else
				{
					mEnd = mCurrent = null;
				}
			}
		}

		/// <inheritdoc cref="IEnumerator{T}.Current"/>
		public readonly ref T Current { get { unsafe { return ref Unsafe.AsRef<T>(mCurrent); } } }		

		/// <inheritdoc/>
		readonly T IEnumerator<T>.Current { get => Current; }

		/// <inheritdoc/>
		readonly object IEnumerator.Current { get => Current; }

		/// <remarks>
		/// <para>
		/// Note: This operation unpins the pin created by constructring this instance of <see cref="Enumerator"/>.
		/// </para>
		/// </remarks>
		/// <inheritdoc/>
		public void Dispose()
		{
			unsafe
			{
				mCurrent = mEnd;

				if (mPin is not null)
				{
					mPin.Dispose();
					mPin = null;
				}
			}
		}

		/// <inheritdoc/>
		public bool MoveNext() { unsafe { return ++mCurrent < mEnd; } }

		[DoesNotReturn]
		readonly void IEnumerator.Reset() => throw new NotSupportedException();
	}
	
	/// <inheritdoc cref="IEnumerable{T}.GetEnumerator"/>
	public readonly Enumerator GetEnumerator() => new(this);

	/// <inheritdoc/>
	readonly IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();

	/// <inheritdoc/>
	readonly IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
