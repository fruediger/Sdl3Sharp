#if SDL3_4_0_OR_GREATER

using Sdl3Sharp.Utilities;
using System;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Video.Rendering;

/// <summary>
/// Represents a custom GPU render state
/// </summary>
public sealed partial class GpuRenderState : IDisposable
{
	private unsafe SDL_GPURenderState* mState;

	internal unsafe GpuRenderState(SDL_GPURenderState* state) => mState = state;

	internal unsafe SDL_GPURenderState* Pointer { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mState; }

	/// <inheritdoc/>
	~GpuRenderState() => DisposeImpl();

	/// <inheritdoc/>
	/// <remarks>
	/// <para>
	/// This method should be called on the thread that created the <see cref="Renderer{TDriver}">Renderer</see>&lt;<see cref="Drivers.Gpu">Gpu</see>&gt;.
	/// </para>
	/// </remarks>
	public void Dispose()
	{
		DisposeImpl();
		GC.SuppressFinalize(this);
	}

	private unsafe void DisposeImpl()
	{
		if (mState is not null)
		{
			unsafe
			{
				SDL_DestroyGPURenderState(mState);
				mState = null;
			}
		}
	}

	/// <summary>
	/// Tries to set fragment shader uniform variables in the GPU render state
	/// </summary>
	/// <param name="slotIndex">The fragment uniform slot index to push data to</param>
	/// <param name="data">The client data to write</param>
	/// <returns>
	/// <c><see langword="true"/></c>, if the fragment shader uniform variables were successfully set; otherwise, <c><see langword="false"/> (check <see cref="Error.TryGet(out string?)"/> for more information)</c>
	/// </returns>
	/// <remarks>
	/// <para>
	/// This given <paramref name="data"/> is copied and will be pushed using <see cref="SDL_PushGPUFragmentUniformData"/> during draw call execution.
	/// </para>
	/// <para>
	/// This method should be called on the thread that created the <see cref="Renderer{TDriver}">Renderer</see>&lt;<see cref="Drivers.Gpu">Gpu</see>&gt;.
	/// </para>
	/// </remarks>
	public bool TrySetFragmentUniforms(uint slotIndex, NativeMemory data)
	{
		unsafe
		{
			if (!data.IsValid)
			{
				return false;
			}

			if (data.Length is > uint.MaxValue)
			{
				data = ((NativeMemory<byte>)data).Slice(0, uint.MaxValue);
			}

			return SDL_SetGPURenderStateFragmentUniforms(mState, slotIndex, data.RawPointer, unchecked((uint)data.Length));
		}
	}

	/// <summary>
	/// Tries to set fragment shader uniform variables in the GPU render state
	/// </summary>
	/// <param name="slotIndex">The fragment uniform slot index to push data to</param>
	/// <param name="data">The client data to write</param>
	/// <returns>
	/// <c><see langword="true"/></c>, if the fragment shader uniform variables were successfully set; otherwise, <c><see langword="false"/> (check <see cref="Error.TryGet(out string?)"/> for more information)</c>
	/// </returns>
	/// <remarks>
	/// <para>
	/// This given <paramref name="data"/> is copied and will be pushed using <see cref="SDL_PushGPUFragmentUniformData"/> during draw call execution.
	/// </para>
	/// <para>
	/// This method should be called on the thread that created the <see cref="Renderer{TDriver}">Renderer</see>&lt;<see cref="Drivers.Gpu">Gpu</see>&gt;.
	/// </para>
	/// </remarks>
	public bool TrySetFragmentUniforms(uint slotIndex, ReadOnlySpan<byte> data)
	{
		unsafe
		{
			fixed (byte* dataPtr = data)
			{
				return SDL_SetGPURenderStateFragmentUniforms(mState, slotIndex, dataPtr, unchecked((uint)data.Length));
			}
		}
	}

	/// <summary>
	/// Tries to set fragment shader uniform variables in the GPU render state
	/// </summary>
	/// <param name="slotIndex">The fragment uniform slot index to push data to</param>
	/// <param name="data">The client data to write</param>
	/// <param name="lenght">The length of the data to write</param>
	/// <returns>
	/// <c><see langword="true"/></c>, if the fragment shader uniform variables were successfully set; otherwise, <c><see langword="false"/> (check <see cref="Error.TryGet(out string?)"/> for more information)</c>
	/// </returns>
	/// <remarks>
	/// <para>
	/// This given <paramref name="data"/> is copied and will be pushed using <see cref="SDL_PushGPUFragmentUniformData"/> during draw call execution.
	/// </para>
	/// <para>
	/// This method should be called on the thread that created the <see cref="Renderer{TDriver}">Renderer</see>&lt;<see cref="Drivers.Gpu">Gpu</see>&gt;.
	/// </para>
	/// </remarks>
	public unsafe bool TrySetFragmentUniforms(uint slotIndex, void* data, uint lenght)
	{
		return SDL_SetGPURenderStateFragmentUniforms(mState, slotIndex, data, lenght);
	}
}

#endif
