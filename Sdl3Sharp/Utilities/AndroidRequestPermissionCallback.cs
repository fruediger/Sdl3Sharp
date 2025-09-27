namespace Sdl3Sharp.Utilities;

/// <summary>
/// Represents a method that handles the response to a call to <see cref="Platform.TryRequestAndroidPermission(string, Sdl3Sharp.Utilities.AndroidRequestPermissionCallback)"/>
/// </summary>
/// <param name="permission">The Android-specific permission name that was requested</param>
/// <param name="granted">A value indicating whether the <paramref name="permission"/> is granted (<c><see langword="true"/></c>) or denied (<c><see langword="false"/></c>)</param>
public delegate void AndroidRequestPermissionCallback(string permission, bool granted);
