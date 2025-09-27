using System;

namespace Sdl3Sharp.Utilities;

[Flags]
public enum MessageBoxFlags : uint
{
	Error              = 0x00000010u,
	Warning            = 0x00000020u,
	Information        = 0x00000040u,
	ButtonsLeftToRight = 0x00000080u,
	ButtonsRightToLeft = 0x00000100u
}
