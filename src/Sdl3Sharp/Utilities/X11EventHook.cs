using System;

namespace Sdl3Sharp.Utilities;

/// <summary>
/// Represents a method to handle messages directly from the X11 event loop
/// </summary>
/// <param name="xevent">A pointer to an <see href="https://www.x.org/releases/current/doc/libX11/libX11/libX11.html#Event_Structures">Xlib XEvent union</see> to process</param>
/// <returns><c><see langword="true"/></c> to let the event continue on; otherwise, <c><see langword="false"/></c> to drop it</returns>
/// <remarks>
/// <para>
/// This callback may modify the event, and should return <c><see langword="true"/></c> if the event should continue to be processed,
/// or <c><see langword="false"/></c> to prevent further processing.
/// </para>
/// <para>
/// As this is processing an event directly from the X11 event loop, this callback should do the minimum required work and return quickly.
/// </para>
/// <para>
/// This may only be called (by SDL) from the thread handling the X11 event loop.
/// </para>
/// </remarks>
public delegate bool X11EventHook(IntPtr xevent);
