using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.Marshalling;

namespace Sdl3Sharp.Input;

/// <summary>
/// Provides extension methods for <see cref="Scancode"/>
/// </summary>
public static partial class ScancodeExtensions
{
	private static readonly Dictionary<Scancode, IntPtr> mUnmanagedNames = [];

	extension(Scancode)
	{
		/// <summary>
		/// Tries to get the <see cref="Scancode"/> corresponding to a given <see cref="Keycode"/> according to the current keyboard layout
		/// </summary>
		/// <param name="keycode">The <see cref="Keycode"/> to look up a <see cref="Scancode"/> for</param>
		/// <param name="modifier">The modifier state that would be used when the <see cref="Scancode"/> generates this key</param>
		/// <param name="scancode">The <see cref="Scancode"/> corresponding to the given <paramref name="keycode"/>, when this method returns <c><see langword="true"/></c>; otherwise <see cref="Scancode.Unknown"/></param>
		/// <returns><c><see langword="true"/></c> if the <see cref="Scancode"/> corresponding to the given <paramref name="keycode"/> was found; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
		/// <remarks>
		/// <para>
		/// Note that there might be multiple combinations of <paramref name="scancode"/>s and <paramref name="modifier"/> states that can generate the same keycode, this method will just return the first one found.
		/// </para>
		/// </remarks>
		public static bool TryGetFromKeycode(Keycode keycode, out Keymod modifier, out Scancode scancode)
		{
			unsafe
			{
				Unsafe.SkipInit(out Keymod localModifier);

				scancode = SDL_GetScancodeFromKey(keycode, &localModifier);

				modifier = localModifier;

				return scancode is not Scancode.Unknown || keycode is Keycode.Unknown;
			}
		}

		/// <summary>
		/// Tries to get the <see cref="Scancode"/> for a key from a human-readable name
		/// </summary>
		/// <param name="name">The human-readable key name to look up a <see cref="Scancode"/> for</param>
		/// <param name="scancode">The <see cref="Scancode"/> for the key with the given human-readable <paramref name="name"/>, when this method returns <c><see langword="true"/></c>; otherwise <see cref="Scancode.Unknown"/></param>
		/// <returns><c><see langword="true"/></c> if the <see cref="Scancode"/> for a key with the given human-readable <paramref name="name"/> was found; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
		public static bool TryGetFromName(string name, out Scancode scancode)
		{
			unsafe
			{
				var nameUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(name);

				try
				{
					scancode = SDL_GetScancodeFromName(nameUtf8);

					return scancode is not Scancode.Unknown;
				}
				finally
				{
					Utf8StringMarshaller.Free(nameUtf8);
				}
			}
		}
	}

	extension(Scancode scancode)
	{
		/// <summary>
		/// Tries to get a human-readable name for the current <see cref="Scancode"/>s key
		/// </summary>
		/// <param name="name">The name of the current <see cref="Scancode"/>s key, when this method returns <c><see langword="true"/></c>; otherwise the empty <see langword="string"/></param>
		/// <returns><c><see langword="true"/></c> if a human-readable name for the current <see cref="Scancode"/>s key was found; otherwise, <c><see langword="false"/></c></returns>
		/// <remarks>
		/// <para>
		/// <em>Warning</em>: The returned name is by design not stable across platforms, e.g. the name for <see cref="Scancode.LeftGui"/> is <c>"Left GUI"</c> under Linux but <c>"Left Windows"</c> under Microsoft Windows,
		/// and some scancodes like <see cref="Scancode.NonUsBackslash"/> don't have any name at all.
		/// There are even scancodes that share names, e.g. <see cref="Scancode.Return"/> and <see cref="Scancode.Return2"/> (both called <c>"Return"</c>).
		/// This method is therefore unsuitable for creating a stable cross-platform two-way mapping between <see cref="string"/>s and <see cref="Scancode"/>s.
		/// </para>
		/// </remarks>
		public bool TryGetName([MaybeNullWhen(false)] out string name)
		{
			unsafe
			{
				name = Utf8StringMarshaller.ConvertToManaged(SDL_GetScancodeName(scancode));

				return !string.IsNullOrEmpty(name);
			}
		}

		/// <summary>
		/// Tries to set a human-readable name the current <see cref="Scancode"/>
		/// </summary>
		/// <param name="name">The name to use for the current <see cref="Scancode"/></param>
		/// <returns><c><see langword="true"/></c> if the given <paramref name="name"/> was successfully set for the current <see cref="Scancode"/>; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
		public bool TrySetName(string name)
		{
			unsafe
			{
				var nameUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(name);

				var result = SDL_SetScancodeName(scancode, nameUtf8);

				if (result)
				{
					// If we already set a name for this kind of scancode previously from the managed,
					// we can now gracefully free it's memory
					// Note: we're only freeing memory in this circumstance;
					// not freeing the memory for these names at program exit, e.g. when the process exits, shouldn't be a problem, as everything gets cleaned up by the OS anyways
					if (mUnmanagedNames.TryGetValue(scancode, out var previousNameUtf8))
					{
						Utf8StringMarshaller.Free(unchecked((byte*)previousNameUtf8));
					}

					mUnmanagedNames[scancode] = unchecked((IntPtr)nameUtf8);
				}

				return result;
			}
		}
	}
}
