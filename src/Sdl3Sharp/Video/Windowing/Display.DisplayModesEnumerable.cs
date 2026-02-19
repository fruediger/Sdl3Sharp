using Sdl3Sharp.Utilities;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sdl3Sharp.Video.Windowing;

partial struct Display
{
	/// <summary>
	/// Provides an <see cref="Enumerator"/> for the display modes of a specific display
	/// </summary>
	/// <remarks>
	/// <para>
	/// A <see cref="DisplayModesEnumerable"/> specifies what kind of display modes to enumerate and which display to enumerate the display modes of.
	/// </para>
	/// </remarks>
	[StructLayout(LayoutKind.Sequential)]
	public readonly struct DisplayModesEnumerable
	{
		/// <summary>
		/// Enumerates the display modes of a specific display
		/// </summary>
		/// <remarks>
		/// <para>
		/// If the enumeration happens to be empty, you can check <see cref="Error.TryGet(out string?)"/> to see if there was an error while retrieving the display modes.
		/// </para>
		/// </remarks>
		[StructLayout(LayoutKind.Sequential)]
		public struct Enumerator : IDisposable
		{
			private unsafe readonly delegate* managed<void*, void> mFree;
			private unsafe DisplayMode** mModes;
			private int mCount;

			/// <summary>
			/// Creates a new <see cref="Enumerator"/> for the display modes of a specific display
			/// </summary>
			/// <param name="enumerable">The <see cref="DisplayModesEnumerable"/> to specify what kind of display modes to enumerate and which display to enumerate the display modes of</param>
			/// <remarks>
			/// <para>
			/// If the enumeration happens to be empty, you can check <see cref="Error.TryGet(out string?)"/> to see if there was an error while retrieving the display modes.
			/// </para>
			/// </remarks>
			[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
			public Enumerator(DisplayModesEnumerable enumerable)
			{
				unsafe
				{
					Unsafe.SkipInit(out int count);

					if (enumerable.mGetter is not null)
					{
						mFree = enumerable.mFree;
						mModes = enumerable.mGetter(enumerable.mDisplayId, &count) switch
						{
							null => null,
							var modes => modes - 1 // the getter returns a pointer to the first element, but we want to start one before the first element so that the first call to MoveNext() advances to the first element
						};
						mCount = count;
					}
					else
					{
						mFree = null;
						mModes = null;
						mCount = 0;
					}
				}
			}

			/// <summary>
			/// Gets a reference to the current display mode
			/// </summary>
			/// <value>
			/// A reference to the current display mode
			/// </value>
			/// <remarks>
			/// <para>
			/// The resulting reference is only valid until the enumerator is <see cref="Dispose">disposed</see>. Don't use the enumerator nor any references obtained from it after it's disposed.
			/// </para>
			/// </remarks>
			public readonly ref readonly DisplayMode Current
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
				get
				{
					unsafe
					{
						return ref (mModes is not null
							? ref Unsafe.AsRef<DisplayMode>(*mModes)
							: ref Unsafe.NullRef<DisplayMode>() // this is dangerous, but highly unlikely to hit, if MoveNext() is used correctly and the enumerator isn't used after Dispose()
						);
					}
				}
			}

			/// <summary>
			/// Advances the enumerator to the next display mode
			/// </summary>
			/// <returns><see langword="true"/> if the enumerator was successfully advanced to the next display mode; otherwise, <see langword="false"/></returns>
			/// <remarks>
			/// <para>
			/// Don't use the enumerator's <see cref="Current"/> property if this method returns <see langword="false"/>, as its value could be a <see langword="null"/>-reference or a reference to an invalid display mode in that case.
			/// </para>
			/// </remarks>
			[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
			public bool MoveNext()
			{
				unsafe
				{
					if (mModes is null || mCount is not > 0)
					{
						return false;
					}

					mModes++;
					mCount--;

					return true;
				}
			}

			/// <summary>
			/// Disposes the enumerator and frees associated resources
			/// </summary>
			public void Dispose()
			{
				unsafe
				{
					if (mFree is not null)
					{
						mFree(mModes);
					}
					mModes = null;
					mCount = 0;
				}
			}
		}

		private unsafe readonly delegate* managed<uint, int*, DisplayMode**> mGetter;
		private unsafe readonly delegate* managed<void*, void> mFree;
		private readonly uint mDisplayId;

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		internal unsafe DisplayModesEnumerable(delegate* managed<uint, int*, DisplayMode**> getter, delegate* managed<void*, void> free, uint displayId)
		{
			mGetter = getter;
			mFree = free;
			mDisplayId = displayId;
		}

		/// <summary>
		/// Copies the display modes of a specific display to a destination span
		/// </summary>
		/// <param name="destination">The destination span to copy the display modes to</param>
		/// <returns>The number of display modes written to the destination span</returns>
		/// <exception cref="InvalidOperationException">
		/// The display modes couldn't be retrieved successfully (check <see cref="Error.TryGet(out string?)"/> for more information)
		/// - OR -
		/// The <paramref name="destination"/> span is too small to hold the display modes
		/// </exception>
		public readonly int CopyTo(Span<DisplayMode> destination)
		{
			if (!TryCopyTo(destination, out int modesWritten))
			{
				failCouldNotCopyDisplayModes();
			}

			return modesWritten;

			[DoesNotReturn]
			static void failCouldNotCopyDisplayModes() => throw new InvalidOperationException("Could not copy display modes to the destination span");
		}

		/// <summary>
		/// Gets an <see cref="Enumerator"/> that enumerates the display modes of a specific display
		/// </summary>
		/// <returns>An <see cref="Enumerator"/> for the display modes of a specific display</returns>
		/// <remarks>
		/// <para>
		/// If the enumeration happens to be empty, you can check <see cref="Error.TryGet(out string?)"/> to see if there was an error while retrieving the display modes.
		/// </para>
		/// </remarks>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public readonly Enumerator GetEnumerator() => new(this);

		/// <summary>
		/// Tries to copy the display modes of a specific display to a destination span
		/// </summary>
		/// <param name="destination">The destination span to copy the display modes to</param>
		/// <param name="modesWritten">The number of display modes written to the destination span</param>
		/// <returns><c><see langword="true"/></c>, if the display modes were successfully copied to the destination span; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
		/// <remarks>
		/// <para>
		/// If this method returns <c><see langword="false"/></c>, the <paramref name="destination"/> isn't modified at all and <paramref name="modesWritten"/> is set to <c>0</c>.
		/// </para>
		/// </remarks>
		public readonly bool TryCopyTo(Span<DisplayMode> destination, out int modesWritten)
		{
			unsafe
			{
				if (mGetter is null)
				{
					modesWritten = 0;
					return false;
				}

				Unsafe.SkipInit(out int count);
				var modes = mGetter(mDisplayId, &count);
				try
				{
					if (modes is null)
					{
						modesWritten = 0;
						return false;
					}

					if (count is not > 0)
					{
						// an empty array of display modes is still a valid result

						modesWritten = 0;
						return true;
					}

					if (destination.Length < count)
					{
						modesWritten = 0;
						return false;
					}

					modesWritten = count;
					foreach (ref var mode in destination)
					{
						mode = **modes++;

						if (--count is not > 0)
						{
							break;
						}
					}

					return true;
				}
				finally
				{
					if (mFree is not null)
					{
						mFree(modes);
					}
				}
			}
		}
	}
}
