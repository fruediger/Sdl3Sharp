using Sdl3Sharp.Internal;
using System;
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
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private string DebuggerDisplay => ToString(formatProvider: CultureInfo.InvariantCulture);

	private unsafe SDL_Palette* mPalette;

	/// <summary>
	/// Creates a new <see cref="Palette"/> with a specified number of colors
	/// </summary>
	/// <param name="length">The number of color entries in the palette</param>
	/// <remarks>
	/// <para>
	/// All palette entries are initialized to be fully white and fully opaque. Use <see cref="TrySetColors(ReadOnlySpan{Color{byte}}, int)"/> to change the colors in the palette.
	/// </para>
	/// </remarks>
	/// <exception cref="SdlException">Couldn't create the <see cref="Palette"/> (check <see cref="Error.TryGet(out string?)"/> for more information)</exception>
	public Palette(int length)
	{
		unsafe
		{
			mPalette = SDL_CreatePalette(length);

			if (mPalette is null)
			{
				failCouldNotCreatePalette();
			}
		}
			
		[DoesNotReturn]
		static void failCouldNotCreatePalette() => throw new SdlException("Could not create the palette");
	}

	/// <inheritdoc/>
	~Palette() => DisposeImpl();

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

	internal unsafe SDL_Palette* Pointer { get => mPalette; }

	/// <inheritdoc/>
	/// <remarks>
	/// <para>
	/// Don't use the value of the <see cref="Colors"/> property or any copies or slices of that value (<see cref="ReadOnlySpan{T}"/>) after disposing the <see cref="Palette"/>!
	/// </para>
	/// </remarks>
	public void Dispose()
	{
		DisposeImpl();
		GC.SuppressFinalize(this);
	}

	private void DisposeImpl()
	{
		unsafe
		{
			if (mPalette is not null)
			{
				SDL_DestroyPalette(mPalette);

				mPalette = null;
			}
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
