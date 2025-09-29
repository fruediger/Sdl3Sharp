using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace Sdl3Sharp;

partial class Properties
{
	/// <summary>
	/// Enumerates the names of the properties in a group of properties
	/// </summary>
	public struct Enumerator : IEnumerator<string>
	{
		[UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
		private unsafe static void EnumeratePropertiesCallback(void* userdata, uint _, byte* name)
		{
			if (userdata is not null && GCHandle.FromIntPtr(unchecked((IntPtr)userdata)) is { IsAllocated: true, Target: List<string> list })
			{
				var nameUtf16 = Utf8StringMarshaller.ConvertToManaged(name);

				if (!string.IsNullOrWhiteSpace(nameUtf16))
				{
					list.Add(nameUtf16);
				}
			}
		}

		private List<string>.Enumerator mListEnumerator;

		private Enumerator(List<string>.Enumerator listEnumerator) => mListEnumerator = listEnumerator;

		/// <summary>
		/// Tries to create a new <see cref="Enumerator"/> for a <see cref="Properties">group of properties</see>
		/// </summary>
		/// <param name="propertyGroup">The group of properties to enumerate</param>
		/// <param name="enumerator">The newly created enumerator for the <see cref="Properties">group of properties</see>, when this mehtod returns <c><see langword="true"/></c>; otherwise, <c><see langword="default"/>(<see cref="Enumerator"/>)</c></param>
		/// <returns><c><see langword="true"/></c> if a new <see cref="Enumerator"/> was successfully created for the <see cref="Properties">group of properties</see>; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
		/// <remarks>
		/// NOTE: The actual enumeration of the properties within the group of properties is performed during the call to this method
		/// </remarks>
		/// <exception cref="ArgumentNullException"><paramref name="propertyGroup"/> is <c><see langword="null"/></c></exception>
		public static bool TryCreate(Properties propertyGroup, out Enumerator enumerator)
		{
			if (propertyGroup is null)
			{
				failPropertyGroupArgumentNull();
			}

			unsafe
			{
				var list = new List<string>();
				var gcHandle = GCHandle.Alloc(list, GCHandleType.Normal);

				try
				{
					if (SDL_EnumerateProperties(propertyGroup.mId, &EnumeratePropertiesCallback, unchecked((void*)GCHandle.ToIntPtr(gcHandle))))
					{
						enumerator = new(list.GetEnumerator());

						return true;
					}
				}
				finally
				{
					gcHandle.Free();
				}

				enumerator = default;

				return false;
			}

			[DoesNotReturn]
			static void failPropertyGroupArgumentNull() => throw new ArgumentNullException(nameof(propertyGroup));
		}

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
			static void reset<TEnumerator>(ref TEnumerator enumerator) where TEnumerator : struct, IEnumerator => enumerator.Reset();

			reset(ref mListEnumerator);
		}
	}

	/// <summary>
	/// Gets an <see cref="Enumerator"/> that enumerates the names of the properties in the group of properties
	/// </summary>
	/// <returns>An <see cref="Enumerator"/> that enumerates the names of the properties in the group of properties</returns>
	/// <exception cref="SdlException">Couldn't create an <see cref="Enumerator"/> for the group of properties (check <see cref="Error.TryGet(out string?)"/> for more information)</exception>
	/// <remarks>
	/// In contrast to most of the remaining API which uses the <c>Try</c>-method pattern, this method intentionally fails by throwing an exception.
	/// If you want to handle failures wrap the call to this method in a <c><see langword="try"/></c>-block,
	/// and check <see cref="Error.TryGet(out string?)"/> for more information when <c><see langword="catch"/></c>ing a <see cref="SdlException"/>.
	/// </remarks>
	public Enumerator GetEnumerator()
	{
		if (!Enumerator.TryCreate(this, out var enumerator))
		{
			failCouldNotGetEnumerator();
		}

		return enumerator;

		[DoesNotReturn]
		static void failCouldNotGetEnumerator() => throw new SdlException(message: $"Could not create an {nameof(Enumerator)} instance for the given {nameof(Properties)} instance");
	}

	/// <inheritdoc/>
	IEnumerator<string> IEnumerable<string>.GetEnumerator() => GetEnumerator();

	/// <inheritdoc/>
	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
