using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Video.Coloring;

/// <summary>
/// Provides extension methods and properties for <see cref="ColorSpace"/>
/// </summary>
public static class ColorSpaceExtensions
{
	extension(ColorSpace)
	{
		/// <summary>
		/// Creates a custom <see cref="ColorSpace"/> from specified color type, color range, color primaries, transfer characteristics, matrix coefficients and chroma sampling location values
		/// </summary>
		/// <param name="type">The color type of the <see cref="ColorSpace"/></param>
		/// <param name="range">The color range of the <see cref="ColorSpace"/></param>
		/// <param name="primaries">The color primaries of the <see cref="ColorSpace"/></param>
		/// <param name="transfer">The transfer characteristics of the <see cref="ColorSpace"/></param>
		/// <param name="matrix">The matrix coefficients of the <see cref="ColorSpace"/></param>
		/// <param name="chroma">The chroma sampling location of the <see cref="ColorSpace"/></param>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static ColorSpace Custom(ColorType type, ColorRange range, ColorPrimaries primaries, TransferCharacteristics transfer, MatrixCoefficients matrix, ChromaLocation chroma)
			=> unchecked((ColorSpace)(
				  (((uint)type      & 0x0fu) << 28)
				| (((uint)range     & 0x0fu) << 24)
				| (((uint)chroma    & 0x0fu) << 20)
				| (((uint)primaries & 0x1fu) << 10)
				| (((uint)transfer  & 0x1fu) << 5)
				| (((uint)matrix    & 0x1fu) << 0)
			));
	}

	extension(ColorSpace colorspace)
	{
		/// <summary>
		/// Gets the chroma sampling location of the <see cref="ColorSpace"/>
		/// </summary>
		/// <value>
		/// The chroma sampling location of the <see cref="ColorSpace"/>
		/// </value> 
		public ChromaLocation Chroma { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => unchecked((ChromaLocation)(((uint)colorspace >> 20) & 0x0fu)); }

		/// <summary>
		/// Gets a value indicating whether the <see cref="ColorSpace"/> uses the full color range
		/// </summary>
		/// <value>
		/// A value indicating whether the <see cref="ColorSpace"/> uses the full color range
		/// </value>
		public bool IsFullRange { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => colorspace.Range is ColorRange.Full; }

		/// <summary>
		/// Gets a value indicating whether the <see cref="ColorSpace"/> uses a limited color range
		/// </summary>
		/// <value>
		/// A value indicating whether the <see cref="ColorSpace"/> uses a limited color range
		/// </value>
		public bool IsLimitedRange { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => colorspace.Range is not ColorRange.Full; }

		/// <summary>
		/// Gets a value indicating whether the <see cref="ColorSpace"/> uses <see cref="MatrixCoefficients.Bt2020Ncl">BT2020 non-constant luminance</see> matrix coefficients
		/// </summary>
		/// <value>
		/// A value indicating whether the <see cref="ColorSpace"/> uses <see cref="MatrixCoefficients.Bt2020Ncl">BT2020 non-constant luminance</see> matrix coefficients
		/// </value>
		public bool IsMatrixBt2020Ncl { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => colorspace.Matrix is MatrixCoefficients.Bt2020Ncl; }

		/// <summary>
		/// Gets a value indicating whether the <see cref="ColorSpace"/> uses <see cref="MatrixCoefficients.Bt601">BT601</see> (or <see cref="MatrixCoefficients.Bt470Bg">BT470BG</see>) matrix coefficients
		/// </summary>
		/// <value>
		/// A value indicating whether the <see cref="ColorSpace"/> uses <see cref="MatrixCoefficients.Bt601">BT601</see> (or <see cref="MatrixCoefficients.Bt470Bg">BT470BG</see>) matrix coefficients
		/// </value>
		public bool IsMatrixBt601 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => colorspace.Matrix is MatrixCoefficients.Bt601 or MatrixCoefficients.Bt470Bg; }

		/// <summary>
		/// Gets a value indicating whether the <see cref="ColorSpace"/> uses <see cref="MatrixCoefficients.Bt709">BT709</see> matrix coefficients
		/// </summary>
		/// <value>
		/// A value indicating whether the <see cref="ColorSpace"/> uses <see cref="MatrixCoefficients.Bt709">BT709</see> matrix coefficients
		/// </value>
		public bool IsMatrixBt709 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => colorspace.Matrix is MatrixCoefficients.Bt709; }

		/// <summary>
		/// Gets the matrix coefficients of the <see cref="ColorSpace"/>
		/// </summary>
		/// <value>
		/// The matrix coefficients of the <see cref="ColorSpace"/>
		/// </value>
		public MatrixCoefficients Matrix { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => unchecked((MatrixCoefficients)(((uint)colorspace >> 0) & 0x1fu)); }

		/// <summary>
		/// Gets the color primaries of the <see cref="ColorSpace"/>
		/// </summary>
		/// <value>
		/// The color primaries of the <see cref="ColorSpace"/>
		/// </value>
		public ColorPrimaries Primaries { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => unchecked((ColorPrimaries)(((uint)colorspace >> 10) & 0x1fu)); }

		/// <summary>
		/// Gets the color range of the <see cref="ColorSpace"/>
		/// </summary>
		/// <value>
		/// The color range of the <see cref="ColorSpace"/>
		/// </value>
		public ColorRange Range { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => unchecked((ColorRange)(((uint)colorspace >> 24) & 0x0fu)); }

		/// <summary>
		/// Gets the transfer characteristics of the <see cref="ColorSpace"/>
		/// </summary>
		/// <value>
		/// The transfer characteristics of the <see cref="ColorSpace"/>
		/// </value>
		public TransferCharacteristics Transfer { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => unchecked((TransferCharacteristics)(((uint)colorspace >> 5) & 0x1fu)); }

		/// <summary>
		/// Gets the color type of the <see cref="ColorSpace"/>
		/// </summary>
		/// <value>
		/// The color type of the <see cref="ColorSpace"/>
		/// </value>
		public ColorType Type { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => unchecked((ColorType)(((uint)colorspace >> 28) & 0x0fu)); }
	}
}
