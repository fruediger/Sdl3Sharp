using Sdl3Sharp.Utilities;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Video;

/// <summary>
/// Manages and represents the pixel data of a locked <see cref="Surface"/>
/// </summary>
public sealed class SurfacePixelMemoryManager : NativeMemoryManagerBase
{
	private Surface? mSurface;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private unsafe SurfacePixelMemoryManager(Surface surface)
	{
		mSurface = surface;
	}

	/// <summary>
	/// Gets a value indicating whether the <see cref="Surface"/> is currently locked (pinned)
	/// </summary>
	/// <value>
	/// <c><see langword="true"/></c>, if the <see cref="Surface"/> is currently locked (pinned); otherwise, <c><see langword="false"/></c> (<see cref="Surface"/> is <c><see langword="null"/></c> in that case)
	/// </value>
	public override bool IsPinned
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		get => mSurface is not null; // The surface's pixels are "pinned" as long as the surface is locked
	}

	/// <inheritdoc/>
	public override nuint Length
	{
		get
		{
			unsafe
			{
				return mSurface is { Pointer: var surface } && surface is not null
					? unchecked((nuint)surface->H * (nuint)surface->Pitch)
					: default;
			}
		}
	}

	/// <summary>
	/// Gets a <see cref="NativeMemory"/> representing the pixel data of the locked <see cref="Surface"/>
	/// </summary>
	/// <value>
	/// A <see cref="NativeMemory"/> representing the pixel data of the locked <see cref="Surface"/>, or <see cref="NativeMemory.Empty"/> if the <see cref="Surface"/> is not locked
	/// </value>
	/// <remarks>
	/// <para>
	/// The pixel data is in the pixel format specified by the <see cref="Surface.Format"/> of the <see cref="Surface"/>.
	/// </para>
	/// <para>
	/// Please keep the <see cref="Surface.Pitch"/> per vertical pixel row in mind when processing the continuous pixel data.
	/// </para>
	/// </remarks>
	public override NativeMemory Memory
	{
		get
		{
			unsafe
			{
				return mSurface is { Pointer: var surface } && surface is not null
					? new(this, 0, unchecked((nuint)surface->H * (nuint)surface->Pitch))
					: NativeMemory.Empty;
			}
		}
	}

	/// <inheritdoc/>
	public override IntPtr Pointer
	{
		get
		{
			unsafe
			{
				return mSurface is { Pointer: var surface } && surface is not null
					? unchecked((IntPtr)surface->Pixels)
					: default;
			}
		}
	}

	/// <summary>
	/// Gets the locked <see cref="Surface"/>
	/// </summary>
	/// <value>
	/// The locked <see cref="Surface"/>, or <c><see langword="null"/></c> if the <see cref="Surface"/> is not locked
	/// </value>
	public Surface? Surface { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mSurface; }

	internal override unsafe void* RawPointer => mSurface is { Pointer: var surface } && surface is not null
		? surface->Pixels
		: default;

	// unnecessary to override: the base implemenation does nothing anyways
	// protected override void AddPin(ulong oldPinCounter, ulong newPinCounter);

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	protected override ulong DecreasePinCounter(ulong pinCounter) => pinCounter; // Do nothing

	/// <summary>
	/// Disposes the <see cref="SurfacePixelMemoryManager"/>, unlocking the associated <see cref="Surface"/>
	/// </summary>
	/// <inheritdoc/>
	/// <remarks>
	/// <para>
	/// Calling this method will unlock the associated <see cref="Surface"/>, if it's still locked, making its pixel data inaccessible until it is locked again.
	/// </para>
	/// </remarks>
	protected override void Dispose(bool disposing)
	{
		unsafe
		{
			if (mSurface is not null)
			{
				Surface.SDL_UnlockSurface(mSurface.Pointer);

				mSurface = null;
			}

			base.Dispose(disposing);
		}
	}

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	protected override ulong IncreasePinCounter(ulong pinCounter) => pinCounter; // Do nothing

	// unnecessary to override: the base implemenation does nothing anyways
	// protected override void RemovePin(ulong oldPinCounter, ulong newPinCounter);

	internal static bool TryCreate(Surface surface, [NotNullWhen(true)] out SurfacePixelMemoryManager? pixelManager)
	{
		unsafe
		{
			if (!(surface is not null && (bool)Surface.SDL_LockSurface(surface.Pointer)))
			{
				pixelManager = null;
				return false;
			}

			pixelManager = new(surface);
			return true;
		}
	}
}
