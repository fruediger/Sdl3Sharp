using Sdl3Sharp.Events;

namespace Sdl3Sharp.Video.Rendering;

public partial class Renderer
{
	private unsafe SDL_Renderer* mRenderer;

	public bool TryAddVulkanRenderSemaphores(uint waitStateMask, long waitSemaphore, long signalSemaphore)
	{
		unsafe
		{
			return SDL_AddVulkanRenderSemaphores(mRenderer, waitStateMask, waitSemaphore, signalSemaphore);
		}
	}

	public bool TryConvertEventToRenderCoordinates(ref Event @event)
	{
		unsafe
		{
			fixed (Event* eventPtr = &@event)
			{
				return SDL_ConvertEventToRenderCoordinates(mRenderer, eventPtr);
			}
		}
	}
}
