using Sdl3Sharp.Events;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices.Marshalling;

namespace Sdl3Sharp.Input;

/// <summary>
/// Provides extension methods for <see cref="Keycode"/>
/// </summary>
public static partial class KeycodeExtensions
{
	extension(Keycode)
	{
		/// <summary>
		/// Tries to get the <see cref="Keycode"/> for a key from a human-readable name
		/// </summary>
		/// <param name="name">The human-readable key name to look up a <see cref="Keycode"/> for</param>
		/// <param name="keycode">The <see cref="Keycode"/> for the key with the given human-readable <paramref name="name"/>, when this method returns <c><see langword="true"/></c>; otherwise <see cref="Keycode.Unknown"/></param>
		/// <returns><c><see langword="true"/></c> if the <see cref="Keycode"/> for a key with the given human-readable <paramref name="name"/> was found; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
		public static bool TryGetFromName(string name, out Keycode keycode)
		{
			unsafe
			{
				var nameUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(name);

				try
				{
					keycode = SDL_GetKeyFromName(nameUtf8);

					return keycode is not Keycode.Unknown;
				}
				finally
				{
					Utf8StringMarshaller.Free(nameUtf8);
				}
			}
		}

		/// <summary>
		/// Tries to get the <see cref="Keycode"/> corresponding to a given <see cref="Scancode"/> according to the current keyboard layout
		/// </summary>
		/// <param name="scancode">The <see cref="Scancode"/> to look up a <see cref="Keycode"/> for</param>
		/// <param name="modifier">The modifier state to use when translating the <see cref="Scancode"/> to a <see cref="Keycode"/></param>
		/// <param name="keycode">The <see cref="Keycode"/> corresponding to the given <paramref name="scancode"/>, when this method returns <c><see langword="true"/></c>; otherwise <see cref="Keycode.Unknown"/></param>
		/// <returns><c><see langword="true"/></c> if the <see cref="Keycode"/> corresponding to the given <paramref name="scancode"/> was found; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
		/// <remarks>
		/// <para>
		/// This method is equivalent to <c><see cref="TryGetFromScancode(Scancode, Keymod, bool, out Keycode)">TryGetFromScancode(<paramref name="scancode"/>, <paramref name="modifier"/>, <see langword="false"/>, <see langword="out"/> <paramref name="keycode"/>)</see></c>.
		/// </para>
		/// </remarks>
		public static bool TryGetFromScancode(Scancode scancode, Keymod modifier, out Keycode keycode)
			=> TryGetFromScancode(scancode, modifier, false, out keycode);

		/// <summary>
		/// Tries to get the <see cref="Keycode"/> corresponding to a given <see cref="Scancode"/> according to the current keyboard layout
		/// </summary>
		/// <param name="scancode">The <see cref="Scancode"/> to look up a <see cref="Keycode"/> for</param>
		/// <param name="modifier">The modifier state to use when translating the <see cref="Scancode"/> to a <see cref="Keycode"/></param>
		/// <param name="usedInKeyEvents"><c><see langword="true"/></c> if the <paramref name="keycode"/> will be used in key events (e.g. <see cref="KeyboardEvent"/>)</param>
		/// <param name="keycode">The <see cref="Keycode"/> corresponding to the given <paramref name="scancode"/>, when this method returns <c><see langword="true"/></c>; otherwise <see cref="Keycode.Unknown"/></param>
		/// <returns><c><see langword="true"/></c> if the <see cref="Keycode"/> corresponding to the given <paramref name="scancode"/> was found; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
		/// <remarks>
		/// <para>
		/// If you want to get the keycode as it would be delivered in key events (e.g. <see cref="KeyboardEvent"/>), including options specified via <see cref="Hint.KeycodeOptions"/>,
		/// then you should pass <paramref name="usedInKeyEvents"/> as <c><see langword="true"/></c>.
		/// Otherwise this function simply translates the given <paramref name="scancode"/> based on the given <paramref name="modifier"/> state.
		/// </para>
		/// </remarks>
		public static bool TryGetFromScancode(Scancode scancode, Keymod modifier, bool usedInKeyEvents, out Keycode keycode)
		{
			keycode = SDL_GetKeyFromScancode(scancode, modifier, usedInKeyEvents);

			return keycode is not Keycode.Unknown || scancode is Scancode.Unknown;
		}
	}

	extension(Keycode keycode)
	{
		/// <summary>
		/// Tries to get a human-readable name for the current <see cref="Keycode"/>s key
		/// </summary>
		/// <param name="name">The name of the current <see cref="Keycode"/>s key, when this method returns <c><see langword="true"/></c>; otherwise the empty <see langword="string"/></param>
		/// <returns><c><see langword="true"/></c> if a human-readable name for the current <see cref="Keycode"/>s key was found; otherwise, <c><see langword="false"/></c></returns>
		/// <remarks>
		/// <para>
		/// Letters will be presented in their uppercase form, if applicable.
		/// </para>
		/// </remarks>
		public bool TryGetName([MaybeNullWhen(false)] out string name)
		{
			unsafe
			{
				name = Utf8StringMarshaller.ConvertToManaged(SDL_GetKeyName(keycode));

				return !string.IsNullOrEmpty(name);
			}
		}
	}
}
