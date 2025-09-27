using System;

namespace Sdl3Sharp.Utilities;

/// <summary>
/// Represents a bit mask describing the state of the external storage for an Android application
/// </summary>
/// <remarks>
/// See the official Android developer guide for more information: <see href="http://developer.android.com/guide/topics/data/data-storage.html"/>.
/// </remarks>
[Flags]
public enum AndroidExternalStorageState : uint
{
	/// <summary>The external storage is unavailable</summary>
	Unavailable = 0x00,

	/// <summary>The external storage can be read</summary>
	Read = 0x01,

	/// <summary>The external storage can be written</summary>
	Write = 0x02
}
