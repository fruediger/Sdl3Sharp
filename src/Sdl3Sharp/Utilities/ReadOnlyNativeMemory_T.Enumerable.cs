using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sdl3Sharp.Utilities;

partial struct ReadOnlyNativeMemory<T> : IEnumerable<T>
{
	/// <summary>
	/// Enumerates <em>read-only</em> references to the values of type <typeparamref name="T"/> in an <see cref="ReadOnlyNativeMemory{T}">allocated <em>read-only</em> memory buffer of type <typeparamref name="T"/></see>
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct Enumerator : IEnumerator<T>
	{
		private NativeMemory<T>.Enumerator mEnumerator;

		/// <summary>
		/// Creates a new <see cref="Enumerator"/> for a specified <see cref="ReadOnlyNativeMemory{T}">allocated <em>read-only</em> memory buffer of type <typeparamref name="T"/></see>
		/// </summary>
		/// <param name="nativeMemory">The <see cref="ReadOnlyNativeMemory{T}">allocated <em>read-only</em> memory buffer of type <typeparamref name="T"/></see> which should be enumerated</param>
		/// <remarks>
		/// <para>
		/// Note: This operation <see cref="Pin">pins</see> the given <paramref name="nativeMemory"/>.
		/// </para>
		/// </remarks> 
		/// <inheritdoc cref="NativeMemory{T}.Enumerator.Enumerator(NativeMemory{T})"/>
		public Enumerator(ReadOnlyNativeMemory<T> nativeMemory) => mEnumerator = new(nativeMemory.NativeMemory);

		/// <inheritdoc cref="IEnumerator{T}.Current"/>
		public readonly ref readonly T Current { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => ref mEnumerator.Current; }		

		/// <inheritdoc/>
		readonly T IEnumerator<T>.Current { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => Current; }

		/// <inheritdoc/>
		readonly object IEnumerator.Current { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => Current; }

		/// <remarks>
		/// <para>
		/// Note: This operation unpins the pin created by constructring this instance of <see cref="Enumerator"/>.
		/// </para>
		/// </remarks>
		/// <inheritdoc/>
		public void Dispose() => mEnumerator.Dispose();

		/// <inheritdoc/>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public bool MoveNext() => mEnumerator.MoveNext();

		[DoesNotReturn]
		readonly void IEnumerator.Reset() => throw new NotSupportedException();
	}

	/// <inheritdoc cref="IEnumerable{T}.GetEnumerator"/>
	/// <inheritdoc cref="Enumerator(ReadOnlyNativeMemory{T})"/>
	public readonly Enumerator GetEnumerator() => new(this);

	/// <inheritdoc/>
	/// <inheritdoc cref="GetEnumerator"/>
	readonly IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();

	/// <inheritdoc/>
	/// <inheritdoc cref="GetEnumerator"/>
	readonly IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
