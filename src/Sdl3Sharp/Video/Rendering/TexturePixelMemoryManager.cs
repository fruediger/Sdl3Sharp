using Sdl3Sharp.Utilities;
using Sdl3Sharp.Video.Coloring;
using Sdl3Sharp.Video.Drawing;
using Sdl3Sharp.Video.Rendering.Drivers;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Video.Rendering;

/// <summary>
/// Manages and represents the entire pixel memory of a locked <see cref="Texture"/> or a partial rectangle of it
/// </summary>
public abstract class TexturePixelMemoryManager : NativeMemoryManagerBase
{
	private unsafe void* mPixels;
	private nuint mPitch;
	private nuint mRowLength;
	private nuint mRowCount;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private protected unsafe TexturePixelMemoryManager(void* pixels, int pitch, int rowLength, int rowCount)
	{
		mPixels = pixels;
		mPitch = unchecked((nuint)pitch);
		mRowLength = unchecked((nuint)rowLength);
		mRowCount = unchecked((nuint)rowCount);
	}

	/// <summary>
	/// Gets a value indicating whether the <see cref="Texture"/> is currently locked (pinned)
	/// </summary>
	/// <value>
	/// <see langword="true"/> if the <see cref="Texture"/> is currently locked (pinned); otherwise, <see langword="false"/> (<see cref="Texture"/> is <c><see langword="null"/></c> in that case)
	/// </value>
	public abstract override bool IsPinned { get; }

	/// <inheritdoc />
	public sealed override nuint Length
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		get => unchecked(mPitch * mRowCount);
	}

	/// <summary>
	/// Gets a <see cref="NativeMemory"/> representing the pixel memory of the locked <see cref="Texture"/>
	/// </summary>
	/// <value>
	/// A <see cref="NativeMemory"/> representing the pixel memory of the locked <see cref="Texture"/>, or <see cref="NativeMemory.Empty"/> if the <see cref="Texture"/> is not locked
	/// </value>
	/// <remarks>
	/// <para>
	/// The pixel memory data is in the pixel format specified by the <see cref="Texture.Format"/> property of the <see cref="Texture"/>.
	/// </para>
	/// <para>
	/// The pixel memory might be longer than the actual pixel data that's safe to access, due to padding at the end of each row.
	/// Especially if the <see cref="Texture"/> was <see cref="Texture.TryLock(in Rect{int}, out TexturePixelMemoryManager?)">locked with a rectangle</see> smaller than the full size of the texture.
	/// </para>
	/// <para>
	/// The pixel memory is laid out in rows, where each row is <see cref="RowLength"/> pixels long, and there are <see cref="RowCount"/> rows.
	/// Between the start of each row, there are <see cref="Pitch"/> bytes.
	/// The <see cref="NativeMemory"/> returned by this property starts at the beginning of the first row.
	/// </para>
	/// <para>
	/// As an optimization, the pixel data made available for editing don't necessarily contain the old (at the time of locking) texture data.
	/// </para>
	/// <para>
	/// Notice: The pixel memory is <em>write-only</em>, and if you need to keep a copy of the texture data you should do that at the application level.
	/// </para>
	/// <para>
	/// You must <see cref="IDisposable.Dispose">dispose</see> the <see cref="TexturePixelMemoryManager"/> to unlock the <see cref="Texture"/> and apply the changes made to the pixel data.
	/// </para>
	/// </remarks>
	/// <example>
	/// This leads to the following row-wise access pattern:
	/// <code>
	/// Texture texture;
	/// Rect&lt;int&gt; rect;
	/// 
	/// ...
	/// 
	/// if (texture.TryLock(rect, out var pixelManager))
	/// {
	///		using (pixelManager)
	///		{
	///			var memory = (NativeMemory&lt;byte&gt;)pixelManager.Memory;	
	///			var bytesPerPixel = (nuint)pixelManager.Texture!.Format.BytesPerPixel;		
	/// 
	///			for (nuint row = 0; row &lt; pixelManager.RowCount; row++)
	///			{
	///				var pixels = memory.Slice(0, pixelManager.RowLength * bytesPerPixel).Span;
	///
	///				// Process the pixels in this row
	///				...
	///
	///				memory = memory.Slice(pixelManager.Pitch);
	///			}
	///		}
	/// }
	/// </code>
	/// You can also access individual pixels directly using the following pattern:
	/// <code>
	/// Texture texture;
	/// Rect&lt;int&gt; rect;
	/// 
	/// ...
	/// 
	/// if (texture.TryLock(rect, out var pixelManager))
	/// {
	///		using (pixelManager)
	///		{
	///			var memory = (NativeMemory&lt;byte&gt;)pixelManager.Memory;
	///			var bytesPerPixel = (nuint)pixelManager.Texture!.Format.BytesPerPixel;
	///
	///			nuint x, y;
	///
	///			// Access the pixel at (x, y)
	///			ref var pixel = ref memory.Slice(y * pixelManager.Pitch + x * bytesPerPixel).Span[0];
	///			
	///			...
	///		}
	/// }
	/// </code>
	/// </example>
	public sealed override NativeMemory Memory
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		get
		{
			unsafe
			{
				return IsPinned && mPixels is not null
					? new(this, 0, Length)
					: NativeMemory.Empty;
			}
		}
	}

	/// <summary>
	/// Gets the pitch of the pixel memory, in bytes
	/// </summary>
	/// <value>
	/// The pitch of the pixel memory, in bytes
	/// </value>
	/// <remarks>
	/// <para>
	/// The pitch is the length of a single row of pixel data in bytes, including any padding bytes at the end of the row.
	/// Notice that that's not necessarily equal to <c><see cref="RowLength">RowLength</see> * <see cref="Texture">Texture</see>.<see cref="Texture.Format">Format</see>.<see cref="PixelFormatExtensions.get_BytesPerPixel(PixelFormat)">BytesPerPixel</see></c>.
	/// Especially if the <see cref="Texture"/> was <see cref="Texture.TryLock(in Rect{int}, out TexturePixelMemoryManager?)">locked with a rectangle</see> smaller than the full size of the texture.
	/// </para>
	/// </remarks>
	public nuint Pitch { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mPitch; }

	/// <inheritdoc />
	public sealed override IntPtr Pointer { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get { unsafe { return unchecked((IntPtr)mPixels); } } }

	internal sealed override unsafe void* RawPointer { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mPixels; }

	/// <summary>
	/// Gets the number of rows in the pixel memory, in pixels
	/// </summary>
	/// <value>
	/// The number of rows in the pixel memory, in pixels
	/// </value>
	public nuint RowCount { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mRowCount; }

	/// <summary>
	/// Gets the length of each row in the pixel memory, in pixels
	/// </summary>
	/// <value>
	/// The length of each row in the pixel memory, in pixels
	/// </value>
	/// <remarks>
	/// <para>
	/// To get the length of each row in bytes, use <c><see cref="RowLength">RowLength</see> * <see cref="Texture">Texture</see>.<see cref="Texture.Format">Format</see>.<see cref="PixelFormatExtensions.get_BytesPerPixel(PixelFormat)">BytesPerPixel</see></c>.
	/// </para>
	/// </remarks>
	public nuint RowLength { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mRowLength; }

	/// <summary>
	/// Gets the locked texture
	/// </summary>
	/// <value>
	/// The locked texture, or <c><see langword="null"/></c> if the texture is not locked
	/// </value>
	public abstract Texture? Texture { get; }

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	protected sealed override void AddPin(ulong oldPinCounter, ulong newPinCounter) => base.AddPin(oldPinCounter, newPinCounter); // fixing this override to the base implementation, which does nothing

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	protected sealed override ulong DecreasePinCounter(ulong pinCounter) => pinCounter; // Do nothing

	/// <summary>
	/// Disposes the <see cref="TexturePixelMemoryManager"/>, unlocking the associated <see cref="Texture"/>
	/// </summary>
	/// <inheritdoc/>
	/// <remarks>
	/// <para>
	/// Calling this method unlocks the associated <see cref="Texture"/>, if it's still locked, applying any changes made to the pixel data, and making its pixel memory inaccessible until it's locked again.
	/// </para>
	/// </remarks>
	protected override void Dispose(bool disposing)
	{
		unsafe
		{
			mPixels = null;
			mPitch = 0;
			mRowLength = 0;
			mRowCount = 0;
			mRowLength = 0;

			base.Dispose(disposing);
		}
	}

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	protected sealed override ulong IncreasePinCounter(ulong pinCounter) => pinCounter; // Do nothing

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	protected sealed override void RemovePin(ulong oldPinCounter, ulong newPinCounter) => base.RemovePin(oldPinCounter, newPinCounter); // fixing this override to the base implementation, which does nothing
}

/// <summary>
/// Manages and represents the entire pixel memory of a locked <see cref="Texture{TDriver}"/> or a partial rectangle of it
/// </summary>
/// <typeparam name="TDriver">
/// The type of the rendering driver associated with the <see cref="Texture{TDriver}"/>
/// </typeparam>
public sealed class TexturePixelMemoryManager<TDriver> : TexturePixelMemoryManager
	where TDriver : notnull, IRenderingDriver
{
	private Texture<TDriver>? mTexture;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	internal unsafe TexturePixelMemoryManager(Texture<TDriver> texture, void* pixels, int pitch, int rowLength, int rowCount)
		: base(pixels, pitch, rowLength, rowCount)
		=> mTexture = texture;

	/// <inheritdoc/>
	public override bool IsPinned { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mTexture is not null; } // The texture's pixels are "pinned" as long as the texture is locked

	/// <inheritdoc/>
	public override Texture<TDriver>? Texture { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mTexture; }

	/// <inheritdoc/>
	protected override void Dispose(bool disposing)
	{
		unsafe
		{
			if (mTexture is not null)
			{
				Rendering.Texture.SDL_UnlockTexture(mTexture.Pointer);

				mTexture = null;
			}

			base.Dispose(disposing);
		}
	}

	internal static bool TryCreate(Texture<TDriver> texture, in Rect<int> rect, [NotNullWhen(true)] out TexturePixelMemoryManager<TDriver>? pixelManager)
	{
		unsafe
		{
			void* pixels;
			Unsafe.SkipInit(out int pitch);

			fixed (Rect<int>* rectPtr = &rect)
			{
				if (!(texture is not null && (bool)Rendering.Texture.SDL_LockTexture(texture.Pointer, rectPtr, &pixels, &pitch)))
				{
					pixelManager = null;
					return false;
				}
			}

			pixelManager = new(texture, pixels, pitch, rect.Width, rect.Height);
			return true;
		}
	}

	internal static bool TryCreate(Texture<TDriver> texture, [NotNullWhen(true)] out TexturePixelMemoryManager<TDriver>? pixelManager)
	{
		unsafe
		{
			void* pixels;
			Unsafe.SkipInit(out int pitch);

			if (!(texture is not null && (bool)Rendering.Texture.SDL_LockTexture(texture.Pointer, null, &pixels, &pitch)))
			{
				pixelManager = null;
				return false;
			}

			pixelManager = new(texture, pixels, pitch, texture.Width, texture.Height);
			return true;
		}
	}
}