using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sdl3Sharp.Events;

partial struct EventType
{
	/// <summary>
	/// Represents an enumerable collection of <see cref="EventType"/>
	/// </summary>
	/// <remarks>
	/// Primarily used by <see cref="TryRegister(int, out Enumerable)"/> as a result type.
	/// </remarks>
	[StructLayout(LayoutKind.Sequential)]
	public readonly struct Enumerable : IEnumerable<EventType>
	{
		private readonly EventType mStart;
		private readonly uint mLength;

		internal Enumerable(EventType start, uint length)
			=> (mStart, mLength) = (start, length);

		/// <summary>
		/// Enumerates <see cref="Enumerable"/>
		/// </summary>
		/// <param name="eventTypeEnumerable"></param>
		[StructLayout(LayoutKind.Sequential)]
		[method: MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public struct Enumerator(Enumerable eventTypeEnumerable) : IEnumerator<EventType>
		{
			private EventType mCurrent = new(unchecked(eventTypeEnumerable.mStart.mKind - 1));
			private readonly EventType mEnd = new(unchecked(eventTypeEnumerable.mStart.mKind + eventTypeEnumerable.mLength));

			/// <inheritdoc/>
			public readonly EventType Current { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mCurrent; }

			/// <inheritdoc/>
			readonly object IEnumerator.Current => Current;

			/// <inheritdoc/>
			[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
			public bool MoveNext() => (mCurrent = new(unchecked(mCurrent.mKind + 1))) < mEnd;

			/// <inheritdoc/>
			readonly void IDisposable.Dispose() { }

			/// <inheritdoc/>
			/// <exception cref="NotSupportedException">always</exception>
			[DoesNotReturn]
			readonly void IEnumerator.Reset() => throw new NotSupportedException();
		}

		/// <inheritdoc cref="IEnumerable{T}.GetEnumerator" />
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public readonly Enumerator GetEnumerator() => new(this);

		/// <inheritdoc/>
		IEnumerator<EventType> IEnumerable<EventType>.GetEnumerator() => GetEnumerator();

		/// <inheritdoc/>
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	}
}
