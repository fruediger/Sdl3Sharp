using System;
using System.Runtime.InteropServices;

namespace Sdl3Sharp.Threading;

public static partial class ThreadLocal
{
	internal abstract class Box
	{
		public abstract void Reset();
	}

	public static void Cleanup() => SDL_CleanupTLS();
}

public sealed class ThreadLocal<T>
{
	private new sealed class Box : ThreadLocal.Box
	{
		public required T Value;

		public override void Reset() => Value = default!;
	}

	private protected AtomicInt32 mId;

	private protected ThreadLocal() => mId.Set(0);

	public bool TryGetValue(out T value)
	{
		unsafe
		{
			fixed (AtomicInt32* idPtr = &mId)
			{
				var valuePtr = ThreadLocal.SDL_GetTLS(idPtr);

				if (valuePtr is not null && GCHandle.FromIntPtr(unchecked((IntPtr)valuePtr)) is { IsAllocated: true, Target: Box { Value: var result } })
				{
					value = result;

					return true;
				}

				value = default!;

				return false;
			}
		}
	}

	public bool TrySetValue(T value)
	{
		unsafe
		{
			fixed (AtomicInt32* idPtr = &mId)
			{
				var valuePtr = ThreadLocal.SDL_GetTLS(idPtr);

				if (valuePtr is not null && GCHandle.FromIntPtr(unchecked((IntPtr)valuePtr)) is { IsAllocated: true, Target: Box box })
				{
					box.Value = value;

					return true;
				}

				box = new Box { Value = value };

				var gcHandle = GCHandle.Alloc(box, GCHandleType.Normal);

				var result = false;
				try
				{
					result = ThreadLocal.SDL_SetTLS(idPtr, unchecked((void*)GCHandle.ToIntPtr(gcHandle)), &ThreadLocal.TLSDestructorCallback);

					return result;
				}
				finally
				{
					if (!result)
					{
						box.Reset();

						gcHandle.Free();
					}
				}
			}
		}
	}
}
