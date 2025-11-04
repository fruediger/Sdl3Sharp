using System;
using System.Diagnostics.CodeAnalysis;

namespace Sdl3Sharp.IO;

/// <summary>
/// Represents a queue for asynchronous I/O operations
/// </summary>
public sealed partial class AsyncIOQueue : IDisposable
{
	private unsafe SDL_AsyncIOQueue* mPtr;

	/// <summary>
	/// Creates a new <see cref="AsyncIOQueue"/>
	/// </summary>
	/// <exception cref="SdlException">The <see cref="AsyncIOQueue"/> could not be created</exception>
	public AsyncIOQueue()
	{
		unsafe
		{
			mPtr = SDL_CreateAsyncIOQueue();

			if (mPtr is null)
			{
				failCouldNotAsyncIOQueue();
			}
		}

		[DoesNotReturn]
		static void failCouldNotAsyncIOQueue() => throw new SdlException($"Could not create the {nameof(AsyncIOQueue)}");
	}

	/// <inheritdoc/>
	~AsyncIOQueue() => DisposeImpl();

	internal unsafe SDL_AsyncIOQueue* Pointer { get => mPtr; }

	/// <inheritdoc/>
	public void Dispose()
	{
		DisposeImpl();
		GC.SuppressFinalize(this);
	}

	private unsafe void DisposeImpl()
	{
		if (mPtr is not null)
		{
			SDL_DestroyAsyncIOQueue(mPtr);

			mPtr = null;
		}
	}

	/// <summary>
	/// Wakes up any threads waiting for an asynchronous I/O outcome (<see cref="TryWaitForOutcome(out AsyncIOOutcome?, int)"/>) from this queue
	/// </summary>
	/// <remarks>
	/// <para>
	/// This method will unblock any threads that are sleeping in a call to <see cref="TryWaitForOutcome(out AsyncIOOutcome?, int)"/> for this queue, and cause them to return from that method.
	/// </para>
	/// <para>
	/// This method can be useful when <see cref="Dispose">disposing</see> a queue to make sure nothing is waiting on it indefinitely.
	/// In this case, once the call to this method completes, the caller should take measures to make sure any previously-blocked threads have returned from their wait and will not touch the queue again.
	/// </para>
	/// </remarks>
	public void Signal()
	{
		unsafe
		{
			SDL_SignalAsyncIOQueue(mPtr);
		}
	}

	/// <summary>
	/// Tries to get an asynchronous I/O outcome (<see cref="AsyncIOOutcome"/>) from this queue without blocking
	/// </summary>
	/// <param name="outcome">The <see cref="AsyncIOOutcome"/> representing the result of a finished asynchronous I/O operation, when this method returns <c><see langword="true"/></c>; otherwise, <c><see langword="null"/></c></param>
	/// <returns><c><see langword="true"/></c>, if there was a finished asynchronous I/O operation in the queue; otherwise, <c><see langword="false"/></c></returns>
	/// <remarks>
	/// <para>
	/// If an asynchronous I/O operation assigned to this queue has finished, this method will return <c><see langword="true"/></c> and fill in <paramref name="outcome"/> with the details of result of that operation.
	/// If no asynchronous I/O operation in the queue has finished, this method will return <c><see langword="false"/></c> immediately. This method does not block.
	/// </para>
	/// <para>
	/// If an asynchronous I/O operation has finished, this method will clean up its internal resources, and then the operation will be removed from the queue.
	/// </para>
	/// <para>
	/// It is safe for multiple threads to call this method on the same <see cref="AsyncIOQueue"/> instance at once; a finished asynchronous I/O operation will only be removed once and it's <see cref="AsyncIOOutcome"/> will only be returned to one of the threads.
	/// </para>
	/// </remarks>
	public bool TryGetOutcome([NotNullWhen(true)] out AsyncIOOutcome? outcome)
	{
		unsafe
		{
			bool result;

			outcome = new();
			fixed (AsyncIOOutcome.SDL_AsyncIOOutcome* outcomePtr = &outcome.Outcome)
			{
				result = SDL_GetAsyncIOResult(mPtr, outcomePtr);
			}

			if (!result)
			{
				outcome = null;

				return false;
			}

			return true;
		}
	}

	/// <summary>
	/// Tries to wait for an asynchronous I/O outcome (<see cref="AsyncIOOutcome"/>) from this queue
	/// </summary>
	/// <param name="outcome">The <see cref="AsyncIOOutcome"/> representing the result of a finished asynchronous I/O operation, when this method returns <c><see langword="true"/></c>; otherwise, <c><see langword="null"/></c></param>
	/// <param name="timeoutMs">The maximum time to wait for an outcome, in milliseconds, or <c>-1</c> to wait indefinitely</param>
	/// <returns><c><see langword="true"/></c>, if there was a finished asynchronous I/O operation in the queue, or if an asynchronous I/O operation finishes before the queue was <see cref="Signal">signaled</see> or timed out; otherwise, <c><see langword="false"/></c></returns>
	/// <remarks>
	/// <para>
	/// This method puts the caller thread to sleep until there an asynchronous I/O operation assigned to the queue that has finished.
	/// </para>
	/// <para>
	/// If an asynchronous I/O operation assigned to this queue has finished, this method will return <c><see langword="true"/></c> and fill in <paramref name="outcome"/> with the details of result of that operation.
	/// If no asynchronous I/O operation in the queue has finished until the queue was <see cref="Signal">signaled</see> or timed out, this method will return <c><see langword="false"/></c>.
	/// </para>
	/// <para>
	/// If an asynchronous I/O operation has finished, this method will clean up its internal resources, and then the operation will be removed from the queue.
	/// </para>
	/// <para>
	/// It is safe for multiple threads to call this method on the same <see cref="AsyncIOQueue"/> instance at once; a finished asynchronous I/O operation will only be removed once and it's <see cref="AsyncIOOutcome"/> will only be returned to one of the threads.
	/// </para>
	/// <para>
	/// Please note that by the nature of some platforms, more than one waiting thread may wake to handle to outcome of a single asynchronous I/O operation, but only one thread will obtain it and it's call to this method will return <c><see langword="true"/></c>,
	/// while the return value of the other awoken threads will be <c><see langword="false"/></c>. So <paramref name="timeoutMs"/> is a <em>maximum</em> wait time, and this method may return <c><see langword="false"/></c> sooner.
	/// </para>
	/// <para>
	/// This method may return <c><see langword="false"/></c> sooner if there was a system error, the OS inadvertently awoke multiple threads, or if <see cref="Signal"/> was called on the <see cref="AsyncIOQueue"/> instance to wake up all waiting threads without a finished asynchronous I/O operation.
	/// </para>
	/// <para>
	/// A <paramref name="timeoutMs"/> can be used to specify a maximum wait time, but rather than polling, it is possible to have a <paramref name="timeoutMs"/> value of <c>-1</c> (its default value) to wait indefinitely, and use <see cref="Signal"/> to wake up the waiting threads later.
	/// </para>
	/// </remarks>
	public bool TryWaitForOutcome([NotNullWhen(true)] out AsyncIOOutcome? outcome, int timeoutMs = -1)
	{
		unsafe
		{
			bool result;

			outcome = new();
			fixed (AsyncIOOutcome.SDL_AsyncIOOutcome* outcomePtr = &outcome.Outcome)
			{
				result = SDL_WaitAsyncIOResult(mPtr, outcomePtr, timeoutMs);
			}

			if (!result)
			{
				outcome = null;

				return false;
			}

			return true;
		}
	}
}
