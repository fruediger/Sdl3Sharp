using System;

namespace Sdl3Sharp.Input;

/// <summary>
/// Represents keyboard modifier keys 
/// </summary>
[Flags]
public enum Keymod : ushort
{
	/// <summary>No modifier key is down</summary>
	None = 0x0000,

	/// <summary>The left <kbd>⇧</kbd> (shift) key is down</summary>
	LeftShift = 0x0001,

	/// <summary>The right <kbd>⇧</kbd> (shift) key is down</summary>
	RightShift = 0x0002,
	
	/// <summary>The "Level 5 Shift" key is down</summary>
	Level5Shift = 0x0004,

	/// <summary>The left <kbd>CTRL</kbd> (left control) key is down</summary>
	LeftControl = 0x0040,

	/// <summary>The right <kbd>CTRL</kbd> (right control) key is down</summary>
	RightControl = 0x0080,

	/// <summary>The left <kbd>ALT</kbd> (left alt) key is down</summary>
	LeftAlt = 0x0100,

	/// <summary>The right <kbd>ALT</kbd> (right alt) key, or sometimes called <kbd>ALT GR</kbd> (alt gr.) key, is down</summary>
	RightAlt = 0x0200,

	/// <summary>The left <kbd>⊞</kbd> (left Windows) key on PCs, left <kbd>⌘</kbd> (left command) key on Macs, or sometimes called left meta key, is down</summary>
	LeftGui = 0x0400,

	/// <summary>The right <kbd>⊞</kbd> (right Windows) key on PCs, right <kbd>⌘</kbd> (right command) key on Macs, or sometimes called right meta key, is down</summary>
	RightGui = 0x0800,

	/// <summary>The <kbd>NUM</kbd> (num lock), may be located on an extended keypad, is down</summary>
	NumLock = 0x1000,

	/// <summary>The <kbd>⇪</kbd> (caps lock) key is down</summary>
	CapsLock = 0x2000,

// TODO: document the following Keymods!
#pragma warning disable CS1591

	Mode = 0x4000,

#pragma warning restore CS1591

	/// <summary>The <kbd>SCROLL</kbd> (scroll lock) key is down</summary>
	ScrollLock = 0x8000,

	/// <summary>Both <kbd>CTRL</kbd> (control) keys combined</summary>
	/// <remarks>
	/// <para>
	/// This is effectively <c><see cref="LeftControl"/> | <see cref="RightControl"/></c>.
	/// </para>
	/// </remarks>
	/// <example>
	/// Use this to determine if <em>any</em> of the <kbd>CTRL</kbd> (control) keys are down:
	/// <code>
	///		(modifier &amp; <see cref="Control"/>) <see langword="is"/> <see langword="not"/> <see cref="None"/> // any 'CTRL' key is down in 'modifier'
	/// </code>
	/// Use this to determine if <em>all</em> of the <kbd>CTRL</kbd> (control) keys are down:
	/// <code>
	///		(modifier &amp; <see cref="Control"/>) <see langword="is"/> <see cref="Control"/> // all 'CTRL' keys are down in 'modifier'
	/// </code>
	/// </example>
	Control = LeftControl | RightControl,

	/// <summary>Both <kbd>⇧</kbd> (shift) keys combined</summary>
	/// <remarks>
	/// <para>
	/// This is effectively <c><see cref="LeftShift"/> | <see cref="RightShift"/></c>.
	/// </para>
	/// </remarks>
	/// <example>
	/// Use this to determine if <em>any</em> of the <kbd>⇧</kbd> (shift) keys are down:
	/// <code>
	///		(modifier &amp; <see cref="Shift"/>) <see langword="is"/> <see langword="not"/> <see cref="None"/> // 'true' when any 'SHIFT' key is down in 'modifier'
	/// </code>
	/// Use this to determine if <em>all</em> of the <kbd>⇧</kbd> (shift) keys are down:
	/// <code>
	///		(modifier &amp; <see cref="Shift"/>) <see langword="is"/> <see cref="Shift"/> // 'true' when all 'SHIFT' keys are down in 'modifier'
	/// </code>
	/// </example>
	Shift = LeftShift | RightShift,

	/// <summary>Both <kbd>ALT</kbd> (alt) keys combined</summary>
	/// <remarks>
	/// <para>
	/// This is effectively <c><see cref="LeftAlt"/> | <see cref="RightAlt"/></c>.
	/// </para>
	/// </remarks>
	/// <example>
	/// Use this to determine if <em>any</em> of the <kbd>ALT</kbd> (alt) keys are down:
	/// <code>
	///		(modifier &amp; <see cref="Alt"/>) <see langword="is"/> <see langword="not"/> <see cref="None"/> // 'true' when any 'ALT' key is down in 'modifier'
	/// </code>
	/// Use this to determine if <em>all</em> of the <kbd>ALT</kbd> (alt) keys are down:
	/// <code>
	///		(modifier &amp; <see cref="Alt"/>) <see langword="is"/> <see cref="Alt"/> // 'true' when all 'ALT' keys are down in 'modifier'
	/// </code>
	/// </example>
	Alt = LeftAlt | RightAlt,

	/// <summary>Both <kbd>⊞</kbd> (Windows) key on PCs, <kbd>⌘</kbd> (command) keys on Macs, or sometimes called meta keys, combined</summary>
	/// <remarks>
	/// <para>
	/// This is effectively <c><see cref="LeftGui"/> | <see cref="RightGui"/></c>.
	/// </para>
	/// </remarks>
	/// <example>
	/// Use this to determine if <em>any</em> of the <kbd>⊞</kbd> (Windows) keys are down:
	/// <code>
	///		(modifier &amp; <see cref="Gui"/>) <see langword="is"/> <see langword="not"/> <see cref="None"/> // 'true' when any 'WINDOWS' key is down in 'modifier'
	/// </code>
	/// Use this to determine if <em>all</em> of the <kbd>⊞</kbd> (Windows) keys are down:
	/// <code>
	///		(modifier &amp; <see cref="Gui"/>) <see langword="is"/> <see cref="Gui"/> // 'true' when all 'WINDOWS' keys are down in 'modifier'
	/// </code>
	/// </example>
	Gui = LeftGui | RightGui,
}
