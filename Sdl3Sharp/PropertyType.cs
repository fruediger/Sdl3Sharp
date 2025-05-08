namespace Sdl3Sharp;

/// <summary>
/// Represents the type of the value of a property within a <see cref="Properties">group of properties</see>
/// </summary>
public enum PropertyType
{
	/// <summary>Invalid</summary>
	Invalid,

	/// <summary>Pointer</summary>
	Pointer,

	/// <summary>String</summary>
	String,

	/// <summary>Number</summary>
	Number,

	/// <summary>Float</summary>
	Float,

	/// <summary>Boolean</summary>
	Boolean
}
