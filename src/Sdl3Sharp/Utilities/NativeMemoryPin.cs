using System;

namespace Sdl3Sharp.Utilities;

/// <summary>
/// Represents a pin on a <see cref="NativeMemoryManager"/>
/// </summary>
public sealed class NativeMemoryPin : IDisposable
{
	private NativeMemoryManager? mMemoryManager;

	internal NativeMemoryPin(NativeMemoryManager? memoryManager)
	{
		mMemoryManager = memoryManager;
		memoryManager?.IncreasePinCounter();
	}

	/// <inheritdoc/>
	~NativeMemoryPin() => DisposeImpl();

	/// <summary>Unpins the referenced <see cref="NativeMemoryManager"/></summary>
	/// <remarks>
	/// <para>
	/// If there are still active pins on the referenced <see cref="NativeMemoryManager"/>, it will remain pinned until all pins are disposed.
	/// </para>
	/// </remarks>
	public void Dispose()
	{
		DisposeImpl();
		GC.SuppressFinalize(this);
	}

	private void DisposeImpl()
	{
		if (mMemoryManager is not null)
		{
			mMemoryManager.DecreasePinCounter();
			mMemoryManager = null;
		}
	}
}
