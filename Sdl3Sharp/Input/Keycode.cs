using Sdl3Sharp.Events;
using Sdl3Sharp.Internal;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace Sdl3Sharp.Input;

/// <summary>
/// Represents a virtual key
/// </summary>
/// <remarks>
/// <para>
/// This represents a virtual key, which is different to a <see cref="Scancode">keyboard scancode</see>.
/// </para>
/// </remarks>
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
[StructLayout(LayoutKind.Sequential)]
public readonly partial struct Keycode :
	IEquatable<Keycode>, IFormattable, ISpanFormattable, IEqualityOperators<Keycode, Keycode, bool>
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly string DebuggerDisplay => ToString();

	private readonly Kind mKind;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private Keycode(Kind kind) => mKind = kind;

	/// <inheritdoc/>
	public override bool Equals([NotNullWhen(true)] object? obj) => obj is Keycode other && Equals(other);

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly bool Equals(Keycode other) => mKind == other.mKind;

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly override int GetHashCode() => mKind.GetHashCode();

	/// <inheritdoc/>
	public readonly override string ToString()
		=> TryGetName(out var name) || (name = KnownKindToString(mKind)) is not null
			? name
			: $"0x{unchecked((uint)mKind):X8}";

	/// <inheritdoc/>
	readonly string IFormattable.ToString(string? format, IFormatProvider? formatProvider) => ToString();

	/// <inheritdoc cref="ISpanFormattable.TryFormat(Span{char}, out int, ReadOnlySpan{char}, IFormatProvider?)"/>
	public readonly bool TryFormat(Span<char> destination, out int charsWritten)
	{
		charsWritten = 0;

		return TryGetName(out var name) || (name = KnownKindToString(mKind)) is not null
			? SpanFormat.TryWrite(name, ref destination, ref charsWritten)
			: SpanFormat.TryWrite("0x", ref destination, ref charsWritten) && SpanFormat.TryWrite(unchecked((uint)mKind), ref destination, ref charsWritten, "X8");
	}	

	/// <inheritdoc/>
	readonly bool ISpanFormattable.TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider) => TryFormat(destination, out charsWritten);

	/// <summary>
	/// Tries to get the <see cref="Keycode"/> for a key from a human-readable name
	/// </summary>
	/// <param name="name">The human-readable key name to look up a <see cref="Keycode"/> for</param>
	/// <param name="keycode">The <see cref="Keycode"/> for the key with the given human-readable <paramref name="name"/>, when this method returns <c><see langword="true"/></c>; otherwise <see cref="Unknown"/></param>
	/// <returns><c><see langword="true"/></c> if the <see cref="Keycode"/> for a key with the given human-readable <paramref name="name"/> was found; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	public static bool TryGetFromName(string name, out Keycode keycode)
	{
		unsafe
		{
			var nameUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(name);

			try
			{
				keycode = SDL_GetKeyFromName(nameUtf8);

				return keycode.mKind is not Kind.Unknown;
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
	/// <param name="keycode">The <see cref="Keycode"/> corresponding to the given <paramref name="scancode"/>, when this method returns <c><see langword="true"/></c>; otherwise <see cref="Unknown"/></param>
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
	/// <param name="keycode">The <see cref="Keycode"/> corresponding to the given <paramref name="scancode"/>, when this method returns <c><see langword="true"/></c>; otherwise <see cref="Unknown"/></param>
	/// <returns><c><see langword="true"/></c> if the <see cref="Keycode"/> corresponding to the given <paramref name="scancode"/> was found; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// If you want to get the keycode as it would be delivered in key events (e.g. <see cref="KeyboardEvent"/>), including options specified in <see cref="Hint.KeycodeOptions"/>,
	/// then you should pass <paramref name="usedInKeyEvents"/> as <c><see langword="true"/></c>.
	/// Otherwise this function simply translates the given <paramref name="scancode"/> based on the given <paramref name="modifier"/> state.
	/// </para>
	/// </remarks>
	public static bool TryGetFromScancode(Scancode scancode, Keymod modifier, bool usedInKeyEvents, out Keycode keycode)
	{
		keycode = SDL_GetKeyFromScancode(scancode, modifier, usedInKeyEvents);

		return keycode.mKind is not Kind.Unknown || scancode == Scancode.Unknown;
	}

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
	public readonly bool TryGetName([MaybeNullWhen(false)] out string name)
	{
		unsafe
		{
			name = Utf8StringMarshaller.ConvertToManaged(SDL_GetKeyName(this));

			return !string.IsNullOrEmpty(name);
		}
	}

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator ==(Keycode left, Keycode right) => left.mKind == right.mKind;

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator !=(Keycode left, Keycode right) => left.mKind != right.mKind;
}
