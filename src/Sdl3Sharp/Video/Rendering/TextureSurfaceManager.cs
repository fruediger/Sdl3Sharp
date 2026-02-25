using Sdl3Sharp.Video.Drawing;
using Sdl3Sharp.Video.Rendering.Drivers;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Video.Rendering;

/// <summary>
/// Manages and represents the entire pixel data of a locked <see cref="ITexture"/> or a partial rectangle of it as a <see cref="Video.Surface"/>
/// </summary>
public abstract class TextureSurfaceManager : IDisposable
{
	private Surface? mSurface;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private protected TextureSurfaceManager(Surface surface)
		=> mSurface = surface;

	/// <inheritdoc/>
	~TextureSurfaceManager() => Dispose(disposing: false);

	/// <summary>
	/// Gets the <see cref="Video.Surface"/> representing the locked area of the <see cref="Texture"/>
	/// </summary>
	/// <value>
	/// The <see cref="Video.Surface"/> representing the locked area of the <see cref="Texture"/>, or <c><see langword="null"/></c> if the <see cref="Texture"/> is not locked
	/// </value>
	/// <remarks>
	/// <para>
	/// As an optimization, the pixel data of the <see cref="Surface"/> made available for editing don't necessarily contain the old (at the time of locking) texture data.
	/// </para>
	/// <para>
	/// Notice: The <see cref="Video.Surface"/>'s pixel memory is <em>write-only</em>, and if you need to keep a copy of the texture data you should do that at the application level.
	/// </para>
	/// <para>
	/// You must <see cref="Dispose()">dispose</see> the <see cref="TextureSurfaceManager"/> to unlock the <see cref="Texture"/> and apply the changes made to the pixel data of the <see cref="Surface"/>.
	/// </para>
	/// </remarks>
	public Surface? Surface { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mSurface; }

	/// <summary>
	/// Gets the locked texture
	/// </summary>
	/// <value>
	/// The locked texture, or <c><see langword="null"/></c> if the texture is not locked
	/// </value>
	public abstract ITexture? Texture { get; }

	/// <summary>
	/// Disposes this <see cref="TexturePixelMemoryManager"/>, unlocking the associated <see cref="Texture"/>
	/// </summary>
	/// <remarks>
	/// Calling this method will unlock the associated <see cref="Texture"/>, if it's still locked, applying any changes made to the pixel data of the <see cref="Surface"/>, and rendering the <see cref="Surface"/> invalid and making its pixel memory inaccessible.
	/// </remarks>
	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	private protected virtual void Dispose(bool disposing)
	{
		mSurface?.Dispose();
		mSurface = null;
	}
}

/// <summary>
/// Manages and represents the entire pixel data of a locked <see cref="Texture{TDriver}"/> or a partial rectangle of it as a <see cref="Video.Surface"/>
/// </summary>
/// <typeparam name="TDriver">
/// The type of the rendering driver associated with the <see cref="Texture{TDriver}"/>
/// </typeparam>
public sealed class TextureSurfaceManager<TDriver> : TextureSurfaceManager
	where TDriver : notnull, IRenderingDriver
{
	private Texture<TDriver>? mTexture;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	internal TextureSurfaceManager(Texture<TDriver> texture, Surface surface)
		: base(surface)
		=> mTexture = texture;

	private protected override void Dispose(bool disposing)
	{
		unsafe
		{
			if (mTexture is not null)
			{
				ITexture.SDL_UnlockTexture(mTexture.Pointer);

				mTexture = null;
			}

			base.Dispose(disposing);
		}
	}

	/// <inheritdoc/>
	public override Texture<TDriver>? Texture { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mTexture; }

	internal static bool TryCreate(Texture<TDriver> texture, in Rect<int> rect, [NotNullWhen(true)] out TextureSurfaceManager<TDriver>? surfaceManager)
	{
		unsafe
		{
			Surface.SDL_Surface* surfacePtr;

			fixed (Rect<int>* rectPtr = &rect)
			{
				if (!(texture is not null && (bool)ITexture.SDL_LockTextureToSurface(texture.Pointer, rectPtr, &surfacePtr)))
				{
					surfaceManager = null;
					return false;
				}

				if (!Surface.TryGetOrCreate(surfacePtr, out var surface))
				{
					// if we somehow fail to create the surface, we need to unlock the texture in order for the native surface to be safely disposed
					ITexture.SDL_UnlockTexture(texture.Pointer);

					surfaceManager = null;
					return false;
				}

				// Surface.TryGetOrCreate did increase the ref count of the surface by now
				// that means we borrowed the surface on the managed side
				// and despite what the SDL docs say, we need to call SDL_DestroySurface on it ourselves to decrease the ref count of the native surface (once we dispose the owning surface manager)

				surfaceManager = new(texture, surface);
				return true;
			}
		}
	}

	internal static bool TryCreate(Texture<TDriver> texture, [NotNullWhen(true)] out TextureSurfaceManager<TDriver>? surfaceManager)
	{
		unsafe
		{
			Surface.SDL_Surface* surfacePtr;

			if (!(texture is not null && (bool)ITexture.SDL_LockTextureToSurface(texture.Pointer, null, &surfacePtr)))
			{
				surfaceManager = null;
				return false;
			}

			if (!Surface.TryGetOrCreate(surfacePtr, out var surface))
			{
				// if we somehow fail to create the surface, we need to unlock the texture in order for the native surface to be safely disposed
				ITexture.SDL_UnlockTexture(texture.Pointer);

				surfaceManager = null;
				return false;
			}

			// Surface.TryGetOrCreate did increase the ref count of the surface by now
			// that means we borrowed the surface on the managed side
			// and despite what the SDL docs say, we need to call SDL_DestroySurface on it ourselves to decrease the ref count of the native surface (once we dispose the owning surface manager)

			surfaceManager = new(texture, surface);
			return true;
		}
	}
}