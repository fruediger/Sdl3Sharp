using System;
using System.Diagnostics.CodeAnalysis;

namespace Sdl3Sharp.Internal;

// this exists, so we don't need a reference to 'Microsoft.Extensions.ObjectPool'
internal sealed class SimpleConcurrentPool<T>
	where T : class
{
	private sealed class Node
	{
		public T? Value;
		public Node? Next;
	}

	private SimpleSpinYieldLock mLock = new();

	private volatile Node? mFreeHead, mAcquiredHead;

	public T Get<TFactory>()
		where TFactory : IFactory<T>
	{
		mLock.Enter(0);
		try
		{
			Node node;
			if (mFreeHead is not null)
			{
				node = mFreeHead;

				mFreeHead = node.Next;
			}
			else
			{
				node = new() { Value = TFactory.Create() };
			}

			node.Next = mAcquiredHead;

			mAcquiredHead = node;

			return node.Value!;
		}
		finally
		{
			mLock.Exit(0);
		}
	}

	public T Get<TFactory, TArg>(in TArg arg)
		where TFactory : IFactory<TArg, T>
	{
		mLock.Enter(0);
		try
		{
			Node node;
			if (mFreeHead is not null)
			{
				node = mFreeHead;

				mFreeHead = node.Next;
			}
			else
			{
				node = new() { Value = TFactory.Create(in arg) };
			}

			node.Next = mAcquiredHead;

			mAcquiredHead = node;

			return node.Value!;
		}
		finally
		{
			mLock.Exit(0);
		}
	}

	/// <exception cref="ArgumentNullException"><c><paramref name="value"/></c> is <c><see langword="null"/></c></exception>
	public void Return(T value)
	{
		if (value is null)
		{
			failValueArgumentNull();
		}

		mLock.Enter(0);
		try
		{
			Node node;
			if (mAcquiredHead is not null)
			{
				node = mAcquiredHead;

				mAcquiredHead = mAcquiredHead.Next;
			}
			else
			{
				node = new();
			}

			node.Value = value;
			node.Next = mFreeHead;

			mFreeHead = node;
		}
		finally
		{
			mLock.Exit(0);
		}

		[DoesNotReturn]
		static void failValueArgumentNull() => throw new ArgumentNullException(nameof(value));
	}
}
