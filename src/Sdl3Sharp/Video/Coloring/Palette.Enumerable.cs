using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Sdl3Sharp.Video.Coloring;

partial class Palette : IEnumerable<Color<byte>>
{
	/// <summary>
	/// Enumerates the colors in a <see cref="Palette"/>
	/// </summary>
	public struct Enumerator : IEnumerator<Color<byte>>
	{
		private Palette? mPalette;
		private int mIndex;
		private Color<byte> mCurrent;

		/// <summary>
		/// Creates a new <see cref="Enumerator"/> for a specified <see cref="Palette"/>
		/// </summary>
		/// <param name="palette">The palette to enumerate</param>
		/// <exception cref="ArgumentNullException"><paramref name="palette"/> is <c><see langword="null"/></c></exception>
		public Enumerator(Palette palette)
		{
			unsafe
			{
				if (palette is null)
				{
					failPaletteArgumentNull();
				}

				mPalette = palette;
				mIndex = -1;
				mCurrent = default;
			}

			[DoesNotReturn]
			static void failPaletteArgumentNull() => throw new ArgumentNullException(nameof(palette));
		}

		/// <inheritdoc/>
		public readonly Color<byte> Current { get => mCurrent; }

		/// <inheritdoc/>
		readonly object IEnumerator.Current { get => Current; }

		/// <inheritdoc/>
		public bool MoveNext()
		{
			unsafe
			{
				if (mPalette is { mPalette: var palettePtr }
				    && palettePtr is not null
					&& palettePtr->Colors is var colorsPtr
					&& colorsPtr is not null
					&& mIndex + 1 is var nextIndex
					&& nextIndex < palettePtr->NColors)
				{
					mIndex = nextIndex;
					mCurrent = colorsPtr[mIndex];
					return true;
				}

				return false;
			}
		}

		/// <inheritdoc/>
		public void Dispose()
		{
			if (mPalette is not null)
			{
				mPalette = null;
				mCurrent = default;
			}
		}

		/// <inheritdoc/>
		public void Reset()
		{ 
			mIndex = -1;
			mCurrent = default;
		}
	}

	/// <summary>
	/// Gets an <see cref="Enumerator"/> that enumerates the colors in the palette
	/// </summary>
	/// <inheritdoc cref="Enumerator(Palette)"/>
	public Enumerator GetEnumerator() => new(this);
	
	/// <inheritdoc cref="GetEnumerator"/>
	IEnumerator<Color<byte>> IEnumerable<Color<byte>>.GetEnumerator() => GetEnumerator();

	/// <inheritdoc cref="GetEnumerator"/>
	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
