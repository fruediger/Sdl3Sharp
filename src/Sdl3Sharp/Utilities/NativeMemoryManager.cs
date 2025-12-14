using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Utilities;

/// <summary>
/// Manages and represents allocated native memory
/// </summary>
/// <remarks>
/// <para>
/// Note: Always remember to <see cref="NativeMemoryManagerBase.Dispose()">dispose</see> <see cref="NativeMemoryManager"/> when the memory they're managing is no longer needed. That also frees the allocated memory.
/// </para>
/// </remarks>
/// <seealso cref="NativeMemory"/>
public sealed class NativeMemoryManager : NativeMemoryManagerBase
{
	private unsafe void* mPointer;
	private nuint mLength;
	private unsafe delegate* managed<void*, void> mFree;

	internal unsafe NativeMemoryManager(void* pointer, nuint length, delegate* managed<void*, void> free)
	{
		mPointer = pointer;
		mLength = length;
		mFree = free;
	}

	internal unsafe delegate* managed<void*, void> Free { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mFree; }

	/// <inheritdoc/>
	public override nuint Length { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mLength; }

	/// <inheritdoc/>
	public override IntPtr Pointer { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get { unsafe { return unchecked((IntPtr)mPointer); } } }

	internal override unsafe void* RawPointer { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mPointer; }

	/// <remarks>
	/// <para>
	/// This operation will free the allocated memory region this <see cref="NativeMemoryManager"/> represents.
	/// </para>
	/// </remarks>
	/// <exception cref="InvalidOperationException"></exception>
	/// <inheritdoc/>
	protected override void Dispose(bool disposing)
	{
		unsafe
		{
			if (RawPointer is not null)
			{
				if (disposing && IsPinned)
				{
					failPinned();
				}

				if (mFree is not null)
				{
					mFree(RawPointer);
					mFree = null;
				}

				mPointer = null;
			}

			mLength = 0;

			base.Dispose(disposing);
		}

		[DoesNotReturn]
		static void failPinned() => throw new InvalidOperationException($"Cannot dispose a pinned {nameof(NativeMemoryManager)}, Dispose all active {nameof(NativeMemoryPin)}s first.");
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	internal unsafe void SetFree(delegate* managed<void*, void> free) { mFree = free; }

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	internal void SetLength(nuint length) { mLength = length; }

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	internal unsafe void SetRawPointer(void* pointer) { mPointer = pointer; }
}
