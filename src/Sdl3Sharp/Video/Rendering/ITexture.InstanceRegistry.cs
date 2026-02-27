using Sdl3Sharp.Video.Rendering.Drivers;
using System;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace Sdl3Sharp.Video.Rendering;

partial interface ITexture
{
	private static readonly ConcurrentDictionary<IntPtr, WeakReference<ITexture>> mKnownInstances = [];

	internal static void Register<TTexture>(TTexture texture)
		where TTexture : notnull, ITexture
	{
		unsafe
		{
			if (texture is { Pointer: var texturePtr } && texturePtr is not null)
			{
				// Neither addRef nor updateRef increase the native ref counter for a very simple reason:
				// This method should only be called on a constructor path, where we created the native instance ourselves; thus, its ref counter is already set to 1.
				// That's totally right, since atm the managed wrapper is the sole borrower of a reference to the native instance.

				mKnownInstances.AddOrUpdate(unchecked((IntPtr)texturePtr), addRef, updateRef, texture);
			}

			static WeakReference<ITexture> addRef(IntPtr texture, TTexture newTexture) => new(newTexture);

			static WeakReference<ITexture> updateRef(IntPtr texture, WeakReference<ITexture> previousTextureRef, TTexture newTexture)
			{
				if (previousTextureRef.TryGetTarget(out ITexture? previousTexture))
				{
#pragma warning disable IDE0079
#pragma warning disable CA1816
					GC.SuppressFinalize(previousTexture);
#pragma warning restore CA1816
#pragma warning restore IDE0079

					// Dispose should call SDL_DestroyTexture and in turn decrease the ref count, so we don't need to do it here manually
					previousTexture.Dispose(disposing: true, forget: false);
				}

				previousTextureRef.SetTarget(newTexture);

				return previousTextureRef;
			}
		}
	}

	internal static void Deregister<TTexture>(TTexture texture)
		where TTexture : notnull, ITexture
	{
		unsafe
		{
			if (texture is { Pointer: var texturePtr })
			{
				mKnownInstances.TryRemove(unchecked((IntPtr)texturePtr), out _);
			}
		}
	}

	internal unsafe static bool TryGetOrCreate(SDL_Texture* texture, [NotNullWhen(true)] out ITexture? result)
	{
		if (texture is null)
		{
			result = null;
			return false;
		}

		var textureRef = mKnownInstances.GetOrAdd(unchecked((IntPtr)texture), createRef);

		if (!textureRef.TryGetTarget(out result))
		{
			textureRef.SetTarget(result = create(texture));
		}

		return true;

		static WeakReference<ITexture> createRef(IntPtr texture) => new(create(unchecked((SDL_Texture*)texture)));

		static ITexture create(SDL_Texture* texture)
		{
			// create is called in both cases, either we register the instance for the first time,
			// or a managed instance was GC'ed and we need to recreate it (potentially for a different native instance).
			// In both cases, that's the ideal place to increase the native ref counter.
			// "Borrow" an additional native reference for remembering the managed instance
			texture->RefCount++;

			// try to identify the best matching registered driver for the texture and create the managed wrapper accordingly,
			// if that fails, fall back to the generic unknown driver
			if (!TryCreateFromRegisteredDriver(texture, register: false, out var result))
			{
				result = new Texture<GenericFallbackRendereringDriver>(texture, register: false);
			}

			return result;
		}
	}

	private protected unsafe static bool TryGetOrCreate<TDriver>(SDL_Texture* texture, [NotNullWhen(true)] out Texture<TDriver>? result)
		where TDriver : notnull, IRenderingDriver
	{
		if (texture is null)
		{
			result = default;
			return false;
		}

		var textureRef = mKnownInstances.GetOrAdd(unchecked((IntPtr)texture), createRef);

		if (!textureRef.TryGetTarget(out var baseResult))
		{
			textureRef.SetTarget(result = create(texture));
		}
		else if (baseResult is Texture<TDriver> typedResult)
		{
			// we optimistically assume that everything's fine, if the managed types match

			result = typedResult;
		}
		else if (baseResult.Pointer is not null)
		{
			// this also means that baseResult.Pointer == texture
			// this indicates that we actually need the texture to be of a different managed type than it currently is,
			// we should just fail in that case

			result = default;
			return false;
		}
		else
		{
			// this indicates that we somehow managed to not properly forget a managed instance that was disposed,
			// so we need to fully recreate the managed instance with the new type here, including increasing the native ref counter

			result = create(texture);
		}

		return true;

		static WeakReference<ITexture> createRef(IntPtr texture) => new(create(unchecked((SDL_Texture*)texture)));

		static Texture<TDriver> create(SDL_Texture* texture)
		{
			// create is called in both cases, either we register the instance for the first time,
			// or a managed instance was GC'ed and we need to recreate it (potentially for a different native instance).
			// In both cases, that's the ideal place to increase the native ref counter.
			// "Borrow" an additional native reference for remembering the managed instance
			texture->RefCount++;

			return new(texture, register: false);
		}
	}
}
