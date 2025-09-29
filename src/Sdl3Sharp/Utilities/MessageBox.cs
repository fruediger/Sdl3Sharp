using Sdl3Sharp.Windowing;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.Marshalling;

namespace Sdl3Sharp.Utilities;

public partial class MessageBox
{
	private readonly List<MessageBoxButton> mButtons = [];

	public IList<MessageBoxButton> Buttons => mButtons;

	public MessageBoxColorScheme? ColorScheme { get; set; }

	public MessageBoxFlags Flags { get; set; }

	public required string Message { get; set; }

	public Window? ParentWindow { get; set; }

	public required string Title { get; set; }

	public bool TryShow(out int resultButtonId)
	{
		unsafe
		{
			var utf8Title = Utf8StringMarshaller.ConvertToUnmanaged(Title);
			var utf8Message = Utf8StringMarshaller.ConvertToUnmanaged(Message);

			try
			{
				var numButtons = mButtons.Count;
				var buttons = NativeMemoryManager.Malloc<SDL_MessageBoxButtonData>(unchecked((nuint)numButtons));
				
				try
				{
					var k = 0;
					foreach (var button in mButtons)
					{
						if (k >= numButtons)
						{
							break;
						}

						buttons[k++] = new() { Flags = button.Flags, ButtonId = button.Id, Text = Utf8StringMarshaller.ConvertToUnmanaged(button.Text) };
					}
					numButtons = k;

					try
					{
						var messageBoxData = new SDL_MessageBoxData
						{
							Flags = Flags,
							Window = ParentWindow is not null
								? ParentWindow.WindowPtr
								: null,
							Title = utf8Title,
							Message = utf8Message,
							NumButtons = numButtons,
							Buttons = buttons,
							ColorScheme = ColorScheme is { Data: var colorScheme }
								? &colorScheme
								: null
						};

						Unsafe.SkipInit(out int localButtonId);

						var result = SDL_ShowMessageBox(&messageBoxData, &localButtonId);

						resultButtonId = localButtonId;

						return result;
					}
					finally
					{
						while (numButtons > 0)
						{
							Utf8StringMarshaller.Free(buttons[--numButtons].Text);
						}
					}
				}
				finally
				{
					NativeMemoryManager.Free(buttons);
				}
			}
			finally
			{
				Utf8StringMarshaller.Free(utf8Message);
				Utf8StringMarshaller.Free(utf8Title);
			}
		}
	}

	public static bool TryShowSimple(MessageBoxFlags flags, string title, string message, Window? parentWindow = null)
	{
		unsafe
		{
			var utf8Title = Utf8StringMarshaller.ConvertToUnmanaged(title);
			var utf8Message = Utf8StringMarshaller.ConvertToUnmanaged(message);

			try
			{
				return SDL_ShowSimpleMessageBox(flags, utf8Title, utf8Message, parentWindow is not null ? parentWindow.WindowPtr : null);
			}
			finally
			{
				Utf8StringMarshaller.Free(utf8Message);
				Utf8StringMarshaller.Free(utf8Title);
			}
		}
	}
}
