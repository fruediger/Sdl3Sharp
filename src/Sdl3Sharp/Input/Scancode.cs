using Sdl3Sharp.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace Sdl3Sharp.Input;

/// <summary>
/// Represents a keyboard scancode
/// </summary>
/// <remarks>
/// <para>
/// This represents a scancode for a key on a keyboard, which is different to a <see cref="Keycode">virtual key</see>.
/// </para>
/// </remarks>
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
[StructLayout(LayoutKind.Sequential)]
public readonly partial struct Scancode :
	IEquatable<Scancode>, IFormattable, ISpanFormattable, IEqualityOperators<Scancode, Scancode, bool>
{
	private static readonly Dictionary<Kind, IntPtr> mUnmanagedNames = [];

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly string DebuggerDisplay => ToString();

	private readonly Kind mKind;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private Scancode(Kind kind) => mKind = kind;

	/// <inheritdoc/>
	public readonly override bool Equals([NotNullWhen(true)] object? obj) => obj is Scancode other && Equals(other);

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly bool Equals(Scancode other) => mKind == other.mKind;

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly override int GetHashCode() => mKind.GetHashCode();

	/// <inheritdoc/>
	public readonly override string ToString()
		=> TryGetName(out var name) || (name = KnownKindToString(mKind)) is not null
			? name
			: $"0x{unchecked((int)mKind):X8}";

	/// <inheritdoc/>
	readonly string IFormattable.ToString(string? format, IFormatProvider? formatProvider) => ToString();

	/// <inheritdoc cref="ISpanFormattable.TryFormat(Span{char}, out int, ReadOnlySpan{char}, IFormatProvider?)"/>
	public readonly bool TryFormat(Span<char> destination, out int charsWritten)
	{
		charsWritten = 0;
		
		return TryGetName(out var name) || (name = KnownKindToString(mKind)) is not null
			? SpanFormat.TryWrite(name, ref destination, ref charsWritten)
			: SpanFormat.TryWrite("0x", ref destination, ref charsWritten) && SpanFormat.TryWrite(unchecked((int)mKind), ref destination, ref charsWritten, "X8");
	}

	/// <inheritdoc/>
	readonly bool ISpanFormattable.TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider) => TryFormat(destination, out charsWritten);

	/// <summary>
	/// Tries to get the <see cref="Scancode"/> corresponding to a given <see cref="Keycode"/> according to the current keyboard layout
	/// </summary>
	/// <param name="keycode">The <see cref="Keycode"/> to look up a <see cref="Scancode"/> for</param>
	/// <param name="modifier">The modifier state that would be used when the <see cref="Scancode"/> generates this key</param>
	/// <param name="scancode">The <see cref="Scancode"/> corresponding to the given <paramref name="keycode"/>, when this method returns <c><see langword="true"/></c>; otherwise <see cref="Unknown"/></param>
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

			return scancode.mKind is not Kind.Unknown || keycode == Keycode.Unknown;
		}
	}

	/// <summary>
	/// Tries to get the <see cref="Scancode"/> for a key from a human-readable name
	/// </summary>
	/// <param name="name">The human-readable key name to look up a <see cref="Scancode"/> for</param>
	/// <param name="scancode">The <see cref="Scancode"/> for the key with the given human-readable <paramref name="name"/>, when this method returns <c><see langword="true"/></c>; otherwise <see cref="Unknown"/></param>
	/// <returns><c><see langword="true"/></c> if the <see cref="Scancode"/> for a key with the given human-readable <paramref name="name"/> was found; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	public static bool TryGetFromName(string name, out Scancode scancode)
	{
		unsafe
		{
			var nameUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(name);

			try
			{
				scancode = SDL_GetScancodeFromName(nameUtf8);

				return scancode.mKind is not Kind.Unknown;
			}
			finally
			{
				Utf8StringMarshaller.Free(nameUtf8);
			}
		}
	}

	/// <summary>
	/// Tries to get a human-readable name for the current <see cref="Scancode"/>s key
	/// </summary>
	/// <param name="name">The name of the current <see cref="Scancode"/>s key, when this method returns <c><see langword="true"/></c>; otherwise the empty <see langword="string"/></param>
	/// <returns><c><see langword="true"/></c> if a human-readable name for the current <see cref="Scancode"/>s key was found; otherwise, <c><see langword="false"/></c></returns>
	/// <remarks>
	/// <para>
	/// <em>Warning</em>: The returned name is by design not stable across platforms, e.g. the name for <see cref="LeftGui"/> is <c>"Left GUI"</c> under Linux but <c>"Left Windows"</c> under Microsoft Windows,
	/// and some scancodes like <see cref="NonUsBackslash"/> don't have any name at all.
	/// There are even scancodes that share names, e.g. <see cref="Return"/> and <see cref="Return2"/> (both called <c>"Return"</c>).
	/// This method is therefore unsuitable for creating a stable cross-platform two-way mapping between <see cref="string"/>s and <see cref="Scancode"/>s.
	/// </para>
	/// </remarks>
	public readonly bool TryGetName([MaybeNullWhen(false)] out string name)
	{
		unsafe
		{
			name = Utf8StringMarshaller.ConvertToManaged(SDL_GetScancodeName(this));

			return !string.IsNullOrEmpty(name);
		}
	}

	/// <summary>
	/// Tries to set a human-readable name the current <see cref="Scancode"/>
	/// </summary>
	/// <param name="name">The name to use for the current <see cref="Scancode"/></param>
	/// <returns><c><see langword="true"/></c> if the given <paramref name="name"/> was successfully set for the current <see cref="Scancode"/>; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	public readonly bool TrySetName(string name)
	{
		unsafe
		{
			var nameUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(name);

			var result = SDL_SetScancodeName(this, nameUtf8);

			if (result)
			{
				// If we already set a name for this kind of scancode previously from the managed,
				// we can now gracefully free it's memory
				// Note: we're only freeing memory in this circumstance;
				// not freeing the memory for these names at program exit, e.g. when the process exits, shouldn't be a problem, as every gets cleaned up by the OS anyways
				if (mUnmanagedNames.TryGetValue(mKind, out var previousNameUtf8))
				{
					Utf8StringMarshaller.Free(unchecked((byte*)previousNameUtf8));
				}

				mUnmanagedNames[mKind] = unchecked((IntPtr)nameUtf8);
			}

			return result;
		}
	}

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator ==(Scancode left, Scancode right) => left.mKind == right.mKind;

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator !=(Scancode left, Scancode right) => left.mKind != right.mKind;
}
