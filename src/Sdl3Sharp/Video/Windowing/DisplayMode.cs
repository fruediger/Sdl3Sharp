using Sdl3Sharp.Video.Coloring;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sdl3Sharp.Video.Windowing;

[StructLayout(LayoutKind.Sequential)]
public readonly partial struct DisplayMode
{
	private readonly uint mDisplayID;
	private readonly PixelFormat mFormat;
	private readonly int mW;
	private readonly int mH;
	private readonly float mPixelDensity;
	private readonly float mRefreshRate;
	private readonly int mRefreshRateNumerator;
	private readonly int mRefreshRateDenominator;
	private unsafe readonly SDL_DisplayModeData* mInternal;

	public readonly Display Display { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(mDisplayID); }

	public readonly PixelFormat Format { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mFormat; }

	public readonly int Height { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mH; }

	public readonly float PixelDensity { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mPixelDensity; }

	public readonly float RefreshRate { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mRefreshRate; }

	public readonly (int Numerator, int Denominator) RefreshRateRatio { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => (mRefreshRateNumerator, mRefreshRateDenominator); }

	public readonly int Width { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mW; }
}
