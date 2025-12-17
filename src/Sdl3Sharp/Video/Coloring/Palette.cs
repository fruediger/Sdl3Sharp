using Sdl3Sharp.Internal;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sdl3Sharp.Video.Coloring;

/// <summary>
/// Represents a palette of colors
/// </summary>
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
public sealed partial class Palette : IDisposable, IFormattable, ISpanFormattable
{
	private interface IUnsafeConstructorDispatch;

	private static readonly ConcurrentDictionary<IntPtr, WeakReference<Palette>> mKnownInstances = [];

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private string DebuggerDisplay => ToString(formatProvider: CultureInfo.InvariantCulture);

	private unsafe SDL_Palette* mPalette;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private unsafe Palette(SDL_Palette* palette, IUnsafeConstructorDispatch? _ = default) => mPalette = palette;

	/// <remarks>
	/// <para>
	/// Use this only for newly created SDL palettes that start with native <see cref="SDL_Palette.RefCount"/> equal to <c>1</c>;
	/// do not call <see cref="TryGetOrCreate"/> for such owner instances.
	/// </para>
	/// </remarks>
	internal unsafe Palette(SDL_Palette* palette) :
#pragma warning disable IDE0034 // Keep it that way for explicitness sake
		this(palette, default(IUnsafeConstructorDispatch?))
#pragma warning restore IDE0034
	{
		if (palette is not null)
		{
			// Neither addRef nor updateRef increase the native ref counter for a very simple reason:
			// If we're on this constructor path, we created the native instance ourselves, so its ref counter is already set to 1.
			// That's totally right, since atm the managed wrapper is the sole borrower of a reference to the native instance.

			mKnownInstances.AddOrUpdate(unchecked((IntPtr)palette), addRef, updateRef, this);
		}

		static WeakReference<Palette> addRef(IntPtr palette, Palette newPalette) => new(newPalette);

		static WeakReference<Palette> updateRef(IntPtr palette, WeakReference<Palette> previousPaletteRef, Palette newPalette)
		{
			if (previousPaletteRef.TryGetTarget(out var previousPalette))
			{
#pragma warning disable IDE0079
#pragma warning disable CA1816
				GC.SuppressFinalize(previousPalette);
#pragma warning restore CA1816
#pragma warning restore IDE0079

				// Dispose calls SDL_DestroyPalette and already decreases the ref count, so we don't need to do it here manually
				previousPalette.Dispose(forget: false);
			}

			previousPaletteRef.SetTarget(newPalette);

			return previousPaletteRef;
		}
	}

	/// <exception cref="SdlException">Couldn't create the <see cref="Palette"/> (check <see cref="Error.TryGet(out string?)"/> for more information)</exception>
	private unsafe Palette(int length, IUnsafeConstructorDispatch? _ = default) :
		this(SDL_CreatePalette(length))
	{
		if (mPalette is null)
		{
			failCouldNotCreatePalette();
		}

		[DoesNotReturn]
		static void failCouldNotCreatePalette() => throw new SdlException("Could not create the palette");
	}

	/// <summary>
	/// Creates a new <see cref="Palette"/> with a specified number of colors
	/// </summary>
	/// <param name="length">The number of color entries in the palette</param>
	/// <remarks>
	/// <para>
	/// All palette entries are initialized to be fully white and fully opaque. Use <see cref="TrySetColors(ReadOnlySpan{Color{byte}}, int)"/> to change the colors in the palette.
	/// </para>
	/// </remarks>
	public Palette(int length) :
#pragma warning disable IDE0034 // Keep it that way for explicitness sake
		this(length, default(IUnsafeConstructorDispatch?))
#pragma warning restore
	{ }

	/// <inheritdoc/>
	~Palette() => Dispose(forget: true);

	/// <summary>
	/// Gets the colors in the palette
	/// </summary>
	/// <value>
	/// The colors in the palette
	/// </value>
	public ReadOnlySpan<Color<byte>> Colors
	{
		get
		{
			unsafe
			{
				if (mPalette is null || mPalette->Colors is null)
				{
					return [];
				}

				return MemoryMarshal.CreateReadOnlySpan(ref Unsafe.AsRef<Color<byte>>(mPalette->Colors), mPalette->NColors);
			}
		}
	}

	internal unsafe SDL_Palette* Pointer { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mPalette; }

	/// <inheritdoc/>
	/// <remarks>
	/// <para>
	/// Don't use the value of the <see cref="Colors"/> property or any copies or slices of that value (<see cref="ReadOnlySpan{T}"/>) after disposing the <see cref="Palette"/>!
	/// </para>
	/// </remarks>
	public void Dispose()
	{
		GC.SuppressFinalize(this);
		Dispose(forget: true);
	}

	private unsafe void Dispose(bool forget)
	{
		if (mPalette is not null)
		{
			if (forget)
			{
				mKnownInstances.TryRemove(unchecked((IntPtr)mPalette), out _);
			}

			// SDL_DestroyPalette decreases the native ref counter, so we don't need to do it manually here
			SDL_DestroyPalette(mPalette);

			mPalette = null;
		}
	}

	/// <inheritdoc/>
	public override string ToString() => ToString(format: default, formatProvider: default);

	/// <inheritdoc cref="IFormattable.ToString(string?, IFormatProvider?)"/>
	public string ToString(IFormatProvider? formatProvider) => ToString(format: default, formatProvider);

	/// <inheritdoc cref="IFormattable.ToString(string?, IFormatProvider?)"/>
	public string ToString(string? format) => ToString(format, formatProvider: default);

	/// <inheritdoc/>
	public string ToString(string? format, IFormatProvider? formatProvider)
	{
		unsafe
		{
			return $"{{ {nameof(Colors)}.{nameof(ReadOnlySpan<>.Length)}: {(mPalette is not null && mPalette->Colors is not null ? mPalette->NColors : 0).ToString(format, formatProvider)} }}";
		}
	}

	/// <inheritdoc/>
	public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format = default, IFormatProvider? provider = default)
	{
		unsafe
		{
			charsWritten = 0;

			return SpanFormat.TryWrite($"{{ {nameof(Colors)}.{nameof(ReadOnlySpan<>.Length)}: ", ref destination, ref charsWritten)
				&& SpanFormat.TryWrite(mPalette is not null && mPalette->Colors is not null ? mPalette->NColors : 0, ref destination, ref charsWritten, format, provider)
				&& SpanFormat.TryWrite(" }", ref destination, ref charsWritten);
		}
	}

	/// <remarks>
	/// <para>
	/// Use this only for <see cref="SDL_Palette"/> pointers owned by SDL (e.g. returned from other APIs);
	/// it borrows exactly one native reference for managed tracking and must not be used for owner-created surfaces.
	/// </para>
	/// </remarks>
	internal unsafe static bool TryGetOrCreate(SDL_Palette* palette, [NotNullWhen(true)] out Palette? result)
	{
		if (palette is null)
		{
			result = null;
			return false;
		}

		var paletteRef = mKnownInstances.GetOrAdd(unchecked((IntPtr)palette), createRef);

		if (!paletteRef.TryGetTarget(out result))
		{
			paletteRef.SetTarget(result = create(palette));
		}

		return true;

		static WeakReference<Palette> createRef(IntPtr palette) => new(create(unchecked((SDL_Palette*)palette)));

		static Palette create(SDL_Palette* palette)
		{
			// create is called in both cases, either we register the instance for the first time,
			// or a managed instance was GC'ed and we need to recreate it (potentially for a different native instance).
			// In both cases, that's the ideal place to increase the native ref counter.

			// "Borrow" an additional native reference for remembering the managed instance
			palette->RefCount++;

			// Notice: this calls the non-registering constructor
			return new(palette,
#pragma warning disable IDE0034 // Keep it that way for explicitness sake
				default(IUnsafeConstructorDispatch?)
#pragma warning restore
			);
		}
	}

	/// <summary>
	/// Tries to set a range of colors in the palette
	/// </summary>
	/// <param name="colors">The colors to copy into the palette</param>
	/// <param name="offset">The index of the first palette entry to modify (offset into the palette)</param>
	/// <returns><c><see langword="true"/></c>, if the provided range of <paramref name="colors"/> was successfully copied into the palette; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	public bool TrySetColors(ReadOnlySpan<Color<byte>> colors, int offset = 0)
	{
		unsafe
		{
			fixed (Color<byte>* colorsPtr = colors)
			{
				return SDL_SetPaletteColors(mPalette, colorsPtr, offset, colors.Length);
			}
		}
	}
}
