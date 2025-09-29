using System;
using System.Diagnostics.CodeAnalysis;

namespace Sdl3Sharp.Utilities;

partial class Environment
{
	private sealed class SdlDisposeReceiver : Sdl.IDisposeReceiver, IDisposable
	{		
		private WeakReference<Sdl>? mSdlReference;
		private WeakReference<Environment>? mEnvironment;

		public SdlDisposeReceiver(Sdl sdl, Environment environment)
		{
			if (sdl is not null)
			{
				if (!sdl.TryRegisterDisposable(this))
				{
					failCouldNotRegisterWithSdl();
				}

				mSdlReference = new WeakReference<Sdl>(sdl);
			}

			mEnvironment = new WeakReference<Environment>(environment);

			[DoesNotReturn]
			static void failCouldNotRegisterWithSdl() => throw new InvalidOperationException($"Couldn't register the {nameof(Environment)} with the given {nameof(Sdl)} instance");
		}

		public Sdl? Sdl => mSdlReference?.TryGetTarget(out var sdl) is true ? sdl : null;

		public void Dispose()
		{
			mSdlReference = null;
			mEnvironment = null;
		}

		void Sdl.IDisposeReceiver.DisposeFromSdl(Sdl sdl)
		{
			if (mEnvironment?.TryGetTarget(out var environment) is true)
			{
				environment.DisposeFromSdl();
			}
		}
	}
}
