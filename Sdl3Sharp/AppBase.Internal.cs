using Sdl3Sharp.Events;

namespace Sdl3Sharp;

partial class AppBase
{
	internal AppResult OnInitializeInternal(Sdl sdl, string[] args) => OnInitialize(sdl, args);

	internal AppResult OnIterateInternal(Sdl sdl) => OnIterate(sdl);

	internal AppResult OnEventInternal(Sdl sdl, ref readonly Event @event) => OnEvent(sdl, in @event);

	internal void OnQuitInternal(Sdl sdl, AppResult appResult) => OnQuit(sdl, appResult);
}
