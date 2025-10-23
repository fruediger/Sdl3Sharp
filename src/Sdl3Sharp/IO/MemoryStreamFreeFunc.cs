using System.Runtime.InteropServices;

namespace Sdl3Sharp.IO;

/// <summary>
/// Represents a method that is used to free the memory buffer that a <see cref="MemoryStream"/> or a <see cref="ReadOnlyMemoryStream"/> was initialized with, when the stream is <see cref="Stream.TryClose">closed</see> or <see cref="Stream.Dispose()">disposed</see>
/// </summary>
/// <param name="mem">A pointer to the memory buffer to free. Always non-<c><see langword="null"/></c>.</param>
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public unsafe delegate void MemoryStreamFreeFunc(void* mem);
