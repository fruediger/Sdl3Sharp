using Sdl3Sharp.Video.Rendering.Drivers;
using System;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace Sdl3Sharp.Video.Rendering;

partial interface IRenderer
{
	private static readonly ConcurrentDictionary<IntPtr, WeakReference<IRenderer>> mKnownInstances = [];

	internal static void Register<TRenderer>(TRenderer renderer)
		where TRenderer : notnull, IRenderer
	{
		unsafe
		{
			if (renderer is { Pointer: var rendererPtr } && rendererPtr is not null)
			{
				// Conversly to textures, renderers are not ref-counted, so there's no ref counter manipulating shenanigans here

				mKnownInstances.AddOrUpdate(unchecked((IntPtr)rendererPtr), addRef, updateRef, renderer);
			}

			static WeakReference<IRenderer> addRef(IntPtr renderer, TRenderer newRenderer) => new(newRenderer);

			static WeakReference<IRenderer> updateRef(IntPtr renderer, WeakReference<IRenderer> previousRendererRef, TRenderer newRenderer)
			{
				if (previousRendererRef.TryGetTarget(out IRenderer? previousRenderer))
				{
#pragma warning disable IDE0079
#pragma warning disable CA1816
					GC.SuppressFinalize(previousRenderer);
#pragma warning restore CA1816
#pragma warning restore IDE0079

					// Conversly to textures, renderers are not ref-counted, so there's no ref counter manipulating shenanigans here
					previousRenderer.Dispose(disposing: true, forget: false);
				}

				previousRendererRef.SetTarget(newRenderer);

				return previousRendererRef;
			}
		}
	}

	internal static void Deregister<TRenderer>(TRenderer renderer)
		where TRenderer : notnull, IRenderer
	{
		unsafe
		{
			if (renderer is { Pointer: var rendererPtr })
			{
				mKnownInstances.TryRemove(unchecked((IntPtr)rendererPtr), out _);
			}
		}
	}

	internal unsafe static bool TryGetOrCreate(SDL_Renderer* renderer, [NotNullWhen(true)] out IRenderer? result)
	{
		if (renderer is null)
		{
			result = null;
			return false;
		}

		var textureRef = mKnownInstances.GetOrAdd(unchecked((IntPtr)renderer), createRef);

		if (!textureRef.TryGetTarget(out result))
		{
			textureRef.SetTarget(result = create(renderer));
		}

		return true;

		static WeakReference<IRenderer> createRef(IntPtr renderer) => new(create(unchecked((SDL_Renderer*)renderer)));

		static IRenderer create(SDL_Renderer* renderer)
		{
			// Conversly to textures, renderers are not ref-counted, so there's no ref counter manipulating shenanigans here

			if (!TryCreateFromRegisteredDriver(renderer, register: false, out var result))
			{
				result = new Renderer<GenericFallbackRendereringDriver>(renderer, register: false);
			}

			return result;
		}
	}

	internal unsafe static bool TryGetOrCreate<TDriver>(SDL_Renderer* renderer, [NotNullWhen(true)] out Renderer<TDriver>? result)
		where TDriver : notnull, IRenderingDriver
	{
		if (renderer is null)
		{
			result = default;
			return false;
		}

		var rendererRef = mKnownInstances.GetOrAdd(unchecked((IntPtr)renderer), createRef);

		if (!rendererRef.TryGetTarget(out var baseResult))
		{
			rendererRef.SetTarget(result = create(renderer));
		}
		else if (baseResult is Renderer<TDriver> typedResult)
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
			// so we need to fully recreate the managed instance with the new type here

			result = create(renderer);
		}

		return true;

		static WeakReference<IRenderer> createRef(IntPtr renderer) => new(create(unchecked((SDL_Renderer*)renderer)));

		static Renderer<TDriver> create(SDL_Renderer* renderer)
		{
			// Conversly to textures, renderers are not ref-counted, so there's no ref counter manipulating shenanigans here

			return new(renderer, register: false);
		}
	}
}
