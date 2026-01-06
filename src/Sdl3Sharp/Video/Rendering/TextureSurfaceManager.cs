using Sdl3Sharp.Video.Drawing;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Video.Rendering;

/// <summary>
/// Manages and represents the entire pixel data of a locked <see cref="Rendering.Texture"/> or a partial rectangle of it as a <see cref="Video.Surface"/>
/// </summary>
public sealed class TextureSurfaceManager : IDisposable
{
	private Texture? mTexture;
	private Surface? mSurface;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private TextureSurfaceManager(Texture texture, Surface surface)
	{
		mTexture = texture;
		mSurface = surface;
	}

	/// <inheritdoc/>
	~TextureSurfaceManager() => DisposeImpl();

	/// <summary>
	/// Gets the <see cref="Video.Surface"/> representing the locked area of the <see cref="Rendering.Texture"/>
	/// </summary>
	/// <value>
	/// The <see cref="Video.Surface"/> representing the locked area of the <see cref="Rendering.Texture"/>, or <c><see langword="null"/></c> if the <see cref="Rendering.Texture"/> is not locked
	/// </value>
	/// <remarks>
	/// <para>
	/// As an optimization, the pixel data of the <see cref="Surface"/> made available for editing don't necessarily contain the old (at the time of locking) texture data.
	/// </para>
	/// <para>
	/// Notice: The <see cref="Video.Surface"/>'s pixel memory is <em>write-only</em>, and if you need to keep a copy of the texture data you should do that at the application level.
	/// </para>
	/// <para>
	/// You must <see cref="Dispose">dispose</see> the <see cref="TextureSurfaceManager"/> to unlock the <see cref="Texture"/> and apply the changes made to the pixel data of the <see cref="Surface"/>.
	/// </para>
	/// </remarks>
	public Surface? Surface { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mSurface; }

	/// <summary>
	/// Gets the locked <see cref="Rendering.Texture"/>
	/// </summary>
	/// <value>
	/// The locked <see cref="Rendering.Texture"/>, or <c><see langword="null"/></c> if the <see cref="Rendering.Texture"/> is not locked
	/// </value>
	public Texture? Texture { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mTexture; }

	/// <summary>
	/// Disposes this <see cref="TextureSurfaceManager"/>, unlocking the associated <see cref="Texture"/>
	/// </summary>
	/// <remarks>
	/// Calling this method will unlock the associated <see cref="Texture"/>, if it's still locked, applying any changes made to the pixel data of the <see cref="Surface"/>, and rendering the <see cref="Surface"/> invalid and making its pixel memory inaccessible.
	/// </remarks>
	public void Dispose()
	{
		DisposeImpl();
		GC.SuppressFinalize(this);
	}

	private void DisposeImpl()
	{
		if (mSurface is not null)
		{
			mSurface.Dispose();
			mSurface = null;
		}
	}

	internal static bool TryCreate(Texture texture, in Rect<int> rect, [NotNullWhen(true)] out TextureSurfaceManager? surfaceManager)
	{
		unsafe
		{
			Surface.SDL_Surface* surfacePtr;
			
			fixed (Rect<int>* rectPtr = &rect)
			{
				if (!(texture is not null && (bool)Texture.SDL_LockTextureToSurface(texture.Pointer, rectPtr, &surfacePtr)))
				{
					surfaceManager = null;
					return false;
				}

				if (!Surface.TryGetOrCreate(surfacePtr, out var surface))
				{
					// if we somehow fail to create the surface, we need to unlock the texture in order for the native surface to be safely disposed
					Texture.SDL_UnlockTexture(texture.Pointer);

					surfaceManager = null;
					return false;
				}

				// Surface.TryGetOrCreate did increase the ref count of the surface by now
				// that means we borrowed the surface on the managed side
				// and despite what the SDL docs say, we need to call SDL_DestroySurface on it ourselves to decrease the ref count of the native surface (once we dispose the owning surface manager)

				surfaceManager = new TextureSurfaceManager(texture, surface);
				return true;
			}
		}
	}

	internal static bool TryCreate(Texture texture, [NotNullWhen(true)] out TextureSurfaceManager? surfaceManager)
	{
		unsafe
		{
			Surface.SDL_Surface* surfacePtr;

			if (!(texture is not null && (bool)Texture.SDL_LockTextureToSurface(texture.Pointer, null, &surfacePtr)))
			{
				surfaceManager = null;
				return false;
			}

			if (!Surface.TryGetOrCreate(surfacePtr, out var surface))
			{
				// if we somehow fail to create the surface, we need to unlock the texture in order for the native surface to be safely disposed
				Texture.SDL_UnlockTexture(texture.Pointer);

				surfaceManager = null;
				return false;
			}

			// Surface.TryGetOrCreate did increase the ref count of the surface by now
			// that means we borrowed the surface on the managed side
			// and despite what the SDL docs say, we need to call SDL_DestroySurface on it ourselves to decrease the ref count of the native surface (once we dispose the owning surface manager)

			surfaceManager = new TextureSurfaceManager(texture, surface);
			return true;
		}
	}
}
