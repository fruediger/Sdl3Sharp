using System.Runtime.InteropServices;

namespace Sdl3Sharp.IO;

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public unsafe delegate void MemoryStreamFreeFunc(void* mem);
