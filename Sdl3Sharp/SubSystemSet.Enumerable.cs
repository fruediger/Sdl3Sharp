using Sdl3Sharp.Interop;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sdl3Sharp;

partial struct SubSystemSet
{
	/// <summary>
	/// Enumerates the <see cref="SubSystem"/> items of a <see cref="SubSystemSet"/>
	/// </summary>
	/// <param name="subSystems">The <see cref="SubSystemSet"/> to enumerate</param>
	[StructLayout(LayoutKind.Sequential)]
	[method: MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public struct Enumerator(SubSystemSet subSystems) : IEnumerator<SubSystem>
	{
		private readonly InitFlags mInitFlags = subSystems.InitFlags;
		private uint mMask = 1;

		/// <inheritdoc/>
		public readonly SubSystem Current { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(mInitFlags & unchecked((InitFlags)(mMask >> 1))); }

		/// <inheritdoc/>
		readonly object IEnumerator.Current => Current;

		/// <inheritdoc/>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public bool MoveNext()
		{
			while (mMask is not 0)
			{
				var mask = unchecked((InitFlags)mMask);
				mMask = unchecked(mMask << 1);

				if ((mInitFlags & mask) is not 0)
				{
					return true;
				}
			}

			return false;
		}

		/// <inheritdoc/>
		readonly void IDisposable.Dispose() { }

		/// <inheritdoc/>
		/// <exception cref="NotSupportedException">always</exception>
		readonly void IEnumerator.Reset() => throw new NotSupportedException();
	}

	/// <inheritdoc cref="IEnumerable{T}.GetEnumerator"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly Enumerator GetEnumerator() => new(this);

	/// <inheritdoc/>
	IEnumerator<SubSystem> IEnumerable<SubSystem>.GetEnumerator() => GetEnumerator();

	/// <inheritdoc/>
	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
