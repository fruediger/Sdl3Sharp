using System;

namespace Sdl3Sharp.IO;

[Flags]
public enum FileAccess
{
	Read      = 0b01,
	Write     = 0b10,
	ReadWrite = Read | Write,
}
