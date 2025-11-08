using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace Sdl3Sharp.IO;

partial class Storage
{
	/// <summary>
	/// Represents an enumerable collection of entries for a specified directory path within a storage container
	/// </summary>
	/// <param name="storage">The storage container to enumerate</param>
	/// <param name="path">The path to the directory to enumerate, or <c><see langword="null"/></c> or the empty string (<c>""</c>) to request the root of the storage container</param>
	/// <remarks>
	/// <para>
	/// If <paramref name="path"/> is <c><see langword="null"/></c> or the empty string (<c>""</c>), this is treated as a request to enumerate the root of the storage container.
	/// </para>
	/// </remarks>
	/// <inheritdoc cref="ValidateStorage(Storage)"/>
	[method: MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly struct DirectoryEnumerable(Storage storage, string? path) : IEnumerable<string>
	{
		/// <summary>
		/// Enumerates entries in a directory within a storage container specified by a <see cref="DirectoryEnumerable"/> instance
		/// </summary>
		public struct Enumerator : IEnumerator<string>
		{
			[UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
			private unsafe static EnumerationResult EnumerateDirectoryCallback(void* userdata, byte* dirname, byte* fname)
			{
				if (userdata is not null && GCHandle.FromIntPtr(unchecked((IntPtr)userdata)) is { IsAllocated: true, Target: List<string> list })
				{
					if (Utf8StringMarshaller.ConvertToManaged(fname) is string fnameUtf16)
					{
						list.Add(fnameUtf16);
					}

					return EnumerationResult.Continue;
				}

				return EnumerationResult.Failure;
			}

			private List<string>.Enumerator mListEnumerator;

			private Enumerator(List<string>.Enumerator listEnumerator) => mListEnumerator = listEnumerator;

			/// <inheritdoc/>
			public readonly string Current => mListEnumerator.Current;

			/// <inheritdoc/>
			readonly object IEnumerator.Current => Current;

			/// <inheritdoc/>
			public void Dispose() => mListEnumerator.Dispose();

			/// <inheritdoc/>
			public bool MoveNext() => mListEnumerator.MoveNext();

			/// <inheritdoc/>
			void IEnumerator.Reset()
			{
				static void reset<TEnumerator>(ref TEnumerator enumerator) where TEnumerator : IEnumerator => enumerator.Reset();

				reset(ref mListEnumerator);
			}

			/// <summary>
			/// Tries to create a new <see cref="Enumerator"/> for a specified <see cref="DirectoryEnumerable"/>
			/// </summary>
			/// <param name="enumerable">The <see cref="DirectoryEnumerable"/> to enumerate</param>
			/// <param name="enumerator">The newly created enumerator for the <see cref="DirectoryEnumerable"/>, when this method returns <c><see langword="true"/></c>; otherwise, <c><see langword="default"/>(<see cref="Enumerator"/>)</c></param>
			/// <returns><c><see langword="true"/></c>, if the enumerator was created successfully; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
			/// <remarks>
			/// <para>
			/// Note: The actual enumeration of the directory entries is performed during the call to this method.
			/// </para>
			/// </remarks>
			public static bool TryCreate(DirectoryEnumerable enumerable, out Enumerator enumerator)
			{
				unsafe
				{
					var pathUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(enumerable.mPath);

					try
					{
						List<string> list = [];
						var gcHandle = GCHandle.Alloc(list, GCHandleType.Normal);

						try
						{
							if (SDL_EnumerateStorageDirectory(enumerable.mStorage.mStorage, pathUtf8, &EnumerateDirectoryCallback, unchecked((void*)GCHandle.ToIntPtr(gcHandle))))
							{
								enumerator = new(list.GetEnumerator());

								return true;
							}

							enumerator = default;

							return false;
						}
						finally
						{
							gcHandle.Free();
						}
					}
					finally
					{
						Utf8StringMarshaller.Free(pathUtf8);
					}
				}
			}
		}

		/// <exception cref="ArgumentNullException"><paramref name="storage"/> is <c><see langword="null"/></c></exception>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		[return: NotNull]
		private static Storage ValidateStorage([NotNull] Storage storage)
		{
			if (storage is null)
			{
				failStorageArgumentNull();
			}

			return storage;

			[DoesNotReturn]
			static void failStorageArgumentNull() => throw new ArgumentNullException(nameof(storage));
		}

		private readonly Storage mStorage = ValidateStorage(storage);
		private readonly string? mPath = path;

		/// <summary>
		/// Gets an enumerator that enumerates through the entries for this <see cref="DirectoryEnumerable"/>
		/// </summary>
		/// <returns>An <see cref="Enumerator"/> that can be used to enumerate the entries for this <see cref="DirectoryEnumerable"/></returns>
		/// <exception cref="SdlException">Couldn't create an <see cref="Enumerator"/> for this <see cref="DirectoryEnumerable"/> (check <see cref="Error.TryGet(out string?)"/> for more information)</exception>
		public readonly Enumerator GetEnumerator()
		{
			if (!Enumerator.TryCreate(this, out var enumerator))
			{
				failCouldNotGetEnumerator();
			}

			return enumerator;

			[DoesNotReturn]
			static void failCouldNotGetEnumerator() => throw new SdlException(message: $"Could not create an {nameof(Enumerator)} instance for the given {nameof(DirectoryEnumerable)} instance");
		}

		/// <inheritdoc cref="GetEnumerator"/>
		IEnumerator<string> IEnumerable<string>.GetEnumerator() => GetEnumerator();

		/// <inheritdoc cref="GetEnumerator"/>
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}
