using Sdl3Sharp.Video.Windowing.Drivers;

namespace Sdl3Sharp.Video.Windowing;

partial class Window
{
	private interface IFactory<TWindow>
		where TWindow : notnull, Window
	{
		public unsafe static abstract TWindow Create(SDL_Window* window, bool register);
	}

	private sealed class RegisteredDriverOrGenericFallbackDriverFactory : IFactory<Window>
	{
		public unsafe static Window Create(SDL_Window* window, bool register)
		{
			if (!TryCreateFromRegisteredDriver(window, register, out var result))
			{
				result = new Window<GenericFallbackWindowingDriver>(window, register);
			}

			return result;
		}
		private RegisteredDriverOrGenericFallbackDriverFactory() { }
	}

	private sealed class Factory<TDriver> : IFactory<Window<TDriver>>
		where TDriver : notnull, IWindowingDriver
	{
		public static unsafe Window<TDriver> Create(SDL_Window* window, bool register)
			=> new Window<TDriver>(window, register);

		private Factory() { }
	}
}
