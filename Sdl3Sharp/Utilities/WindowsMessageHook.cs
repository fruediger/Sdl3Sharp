using System;

namespace Sdl3Sharp.Utilities;

/// <summary>
/// Represents a method to handle messages directly from the Windows event loop
/// </summary>
/// <param name="msg">A pointer to a <see href="https://learn.microsoft.com/en-us/windows/win32/api/winuser/ns-winuser-msg">Win32 event structure</see> to process</param>
/// <returns><c><see langword="true"/></c> to let the event continue on; otherwise, <c><see langword="false"/></c> to drop it</returns>
/// <remarks>
/// <para>
/// This callback may modify the message, and should return <c><see langword="true"/></c> if the message should continue to be processed,
/// or <c><see langword="false"/></c> to prevent further processing.
/// </para>
/// <para>
/// As this is processing a message directly from the Windows event loop, this callback should do the minimum required work and return quickly.
/// </para>
/// <para>
/// This may only be called (by SDL) from the thread handling the Windows event loop.
/// </para>
/// </remarks>
public delegate bool WindowsMessageHook(IntPtr msg);
