using System.Diagnostics;

namespace Sdl3Sharp.Utilities;

partial class FileDialog
{
	public static class PropertyNames
	{
		public const string	FiltersPointer = "SDL_PROP_FILE_DIALOG_FILTERS_POINTER";
		public const string	NFiltersNumber = "SDL_PROP_FILE_DIALOG_NFILTERS_NUMBER";
		public const string	WindowPointer  = "SDL_PROP_FILE_DIALOG_WINDOW_POINTER";
		public const string	LocationString = "SDL_PROP_FILE_DIALOG_LOCATION_STRING";
		public const string	ManyBoolean    = "SDL_PROP_FILE_DIALOG_MANY_BOOLEAN";
		public const string	TitleString    = "SDL_PROP_FILE_DIALOG_TITLE_STRING";
		public const string	AcceptString   = "SDL_PROP_FILE_DIALOG_ACCEPT_STRING";
		public const string	CancelString   = "SDL_PROP_FILE_DIALOG_CANCEL_STRING";
	}
}
