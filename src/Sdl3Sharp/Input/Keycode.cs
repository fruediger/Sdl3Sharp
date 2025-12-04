using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Sdl3Sharp.Input;

/// <summary>
/// Represents a virtual key
/// </summary>
/// <remarks>
/// <para>
/// This represents a virtual key, <em>which is different to a <see cref="Scancode">keyboard scancode</see></em>.
/// </para>
/// </remarks>
public enum Keycode : uint
{
	/// <summary>Do not use directly</summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("Do not use directly.")]
	ExtendedMask = 1u << 29,

	/// <summary>Do not use directly</summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("Do not use directly.")]
	ScancodeMask = 1u << 30,

	/// <summary>A representative for an unknown <see cref="Keycode"/></summary>
	Unknown = 0,

	/// <summary>The <see cref="Keycode"/> for the <c>'\r'</c> (return) key; analogous to the <kbd>⏎</kbd> (return / enter) key</summary>
	Return = '\r',

	/// <summary>The <see cref="Keycode"/> for the <c>'\e'</c> (escape) key; analogous to the <kbd>ESC</kbd> (escape) key</summary>
	Escape = '\e',

	/// <summary>The <see cref="Keycode"/> for the <c>'\b'</c> (back space) key; analogous to the <kbd>⌫</kbd> (back space) key</summary>
	Backspace = '\b',

	/// <summary>The <see cref="Keycode"/> for the <c>'\t'</c> (tab) key; analogous to the <kbd>⭾</kbd> (tab) key</summary>
	Tab = '\t',

	/// <summary>The <see cref="Keycode"/> for the <c>' '</c> (space) key; analogous to the <kbd>⎵</kbd> (space) key</summary>
	Space = ' ',

	/// <summary>The <see cref="Keycode"/> for the <c>'!'</c> (exclamation mark) key</summary>
	ExclamationMark = '!',

	/// <summary>The <see cref="Keycode"/> for the <c>'"'</c> (double apostrophe) key</summary>
	DoubleApostrophe = '"',

	/// <summary>The <see cref="Keycode"/> for the <c>'#'</c> (hash sign) key</summary>
	Hash = '#',

	/// <summary>The <see cref="Keycode" /> for the <c>'$'</c> (percent sign) key</summary>
	Dollar = '$',

	/// <summary>The <see cref="Keycode"/> for the <c>'%'</c> (percent sign) key</summary>
	Percent = '%',

	/// <summary>The <see cref="Keycode"/> for the <c>'&amp;'</c> (ampersand) key</summary>
	Ampersand = '&',

	/// <summary>The <see cref="Keycode"/> for the <c>'\''</c> (apostrophe) key</summary>
	Apostrophe = '\'',

	/// <summary>The <see cref="Keycode"/> for the <c>'('</c> (left parenthesis) key</summary>
	LeftParenthesis = '(',

	/// <summary>The <see cref="Keycode"/> for the <c>')'</c> (right parenthesis) key</summary>
	RightParenthesis = ')',

	/// <summary>The <see cref="Keycode"/> for the <c>'*'</c> (asterisk) key</summary>
	Asterisk = '*',

	/// <summary>The <see cref="Keycode"/> for the <c>'+'</c> (plus sign) key</summary>
	Plus = '+',

	/// <summary>The <see cref="Keycode"/> for the <c>','</c> (comma) key</summary>
	Comma = ',',

	/// <summary>The <see cref="Keycode"/> for the <c>'-'</c> (minus sign) key</summary>
	Minus = '-',

	/// <summary>The <see cref="Keycode"/> for the <c>'.'</c> (period) key</summary>
	Period = '.',

	/// <summary>The <see cref="Keycode"/> for the <c>'/'</c> (slash) key</summary>
	Slash = '/',

	/// <summary>The <see cref="Keycode"/> for the <c>'0'</c> key; analogous to the <kbd>0</kbd> key</summary>
	_0 = '0',

	/// <summary>The <see cref="Keycode"/> for the <c>'1'</c> key; analogous to the <kbd>1</kbd> key</summary>
	_1 = '1',

	/// <summary>The <see cref="Keycode"/> for the <c>'2'</c> key; analogous to the <kbd>2</kbd> key</summary>
	_2 = '2',

	/// <summary>The <see cref="Keycode"/> for the <c>'3'</c> key; analogous to the <kbd>3</kbd> key</summary>
	_3 = '3',

	/// <summary>The <see cref="Keycode"/> for the <c>'4'</c> key; analogous to the <kbd>4</kbd> key</summary>
	_4 = '4',

	/// <summary>The <see cref="Keycode"/> for the <c>'5'</c> key; analogous to the <kbd>5</kbd> key</summary>
	_5 = '5',

	/// <summary>The <see cref="Keycode"/> for the <c>'6'</c> key; analogous to the <kbd>6</kbd> key</summary>
	_6 = '6',

	/// <summary>The <see cref="Keycode"/> for the <c>'7'</c> key; analogous to the <kbd>7</kbd> key</summary>
	_7 = '7',

	/// <summary>The <see cref="Keycode"/> for the <c>'8'</c> key; analogous to the <kbd>8</kbd> key</summary>
	_8 = '8',

	/// <summary>The <see cref="Keycode"/> for the <c>'9'</c> key; analogous to the <kbd>9</kbd> key</summary>
	_9 = '9',

	/// <summary>The <see cref="Keycode"/> for the <c>':'</c> (colon) key</summary>
	Colon = ':',

	/// <summary>The <see cref="Keycode"/> for the <c>';'</c> (semicolon) key</summary>
	Semicolon = ';',

	/// <summary>The <see cref="Keycode"/> for the <c>'&lt;'</c> (less than sign) key</summary>
	Less = '<',

	/// <summary>The <see cref="Keycode"/> for the <c>'='</c> (equal sign) key</summary>
	Equals = '=',

	/// <summary>The <see cref="Keycode"/> for the <c>'&gt;'</c> (greater than sign) key</summary>
	Greater = '>',

	/// <summary>The <see cref="Keycode"/> for the <c>'?'</c> (question mark) key</summary>
	QuestionMark = '?',

	/// <summary>The <see cref="Keycode"/> for the <c>'@'</c> (at sign) key</summary>
	At = '@',

	/// <summary>The <see cref="Keycode"/> for the <c>'['</c> (left bracket) key</summary>
	LeftBracket = '[',

	/// <summary>The <see cref="Keycode"/> for the <c>'\\'</c> (back slash) key</summary>
	Backslash = '\\',

	/// <summary>The <see cref="Keycode"/> for the <c>']'</c> (right bracket) key</summary>
	RightBracket = ']',

	/// <summary>The <see cref="Keycode"/> for the <c>'^'</c> (caret / accent circumflex) key</summary>
	Caret = '^',

	/// <summary>The <see cref="Keycode"/> for the <c>'_'</c> (underscore) key</summary>
	Underscore = '_',

	/// <summary>The <see cref="Keycode"/> for the <c>'`'</c> (accent grave) key</summary>
	Grave = '`',

	/// <summary>The <see cref="Keycode"/> for the <c>'a'</c> key; analogous to the <kbd>A</kbd> key</summary>
	A = 'a',

	/// <summary>The <see cref="Keycode"/> for the <c>'b'</c> key; analogous to the <kbd>B</kbd> key</summary>
	B = 'b',

	/// <summary>The <see cref="Keycode"/> for the <c>'c'</c> key; analogous to the <kbd>C</kbd> key</summary>
	C = 'c',

	/// <summary>The <see cref="Keycode"/> for the <c>'d'</c> key; analogous to the <kbd>D</kbd> key</summary>
	D = 'd',

	/// <summary>The <see cref="Keycode"/> for the <c>'e'</c> key; analogous to the <kbd>E</kbd> key</summary>
	E = 'e',

	/// <summary>The <see cref="Keycode"/> for the <c>'f'</c> key; analogous to the <kbd>F</kbd> key</summary>
	F = 'f',

	/// <summary>The <see cref="Keycode"/> for the <c>'g'</c> key; analogous to the <kbd>G</kbd> key</summary>
	G = 'g',

	/// <summary>The <see cref="Keycode"/> for the <c>'h'</c> key; analogous to the <kbd>H</kbd> key</summary>
	H = 'h',

	/// <summary>The <see cref="Keycode"/> for the <c>'i'</c> key; analogous to the <kbd>I</kbd> key</summary>
	I = 'i',

	/// <summary>The <see cref="Keycode"/> for the <c>'j'</c> key; analogous to the <kbd>J</kbd> key</summary>
	J = 'j',

	/// <summary>The <see cref="Keycode"/> for the <c>'k'</c> key; analogous to the <kbd>K</kbd> key</summary>
	K = 'k',

	/// <summary>The <see cref="Keycode"/> for the <c>'l'</c> key; analogous to the <kbd>L</kbd> key</summary>
	L = 'l',

	/// <summary>The <see cref="Keycode"/> for the <c>'m'</c> key; analogous to the <kbd>M</kbd> key</summary>
	M = 'm',

	/// <summary>The <see cref="Keycode"/> for the <c>'n'</c> key; analogous to the <kbd>N</kbd> key</summary>
	N = 'n',

	/// <summary>The <see cref="Keycode"/> for the <c>'o'</c> key; analogous to the <kbd>O</kbd> key</summary>
	O = 'o',

	/// <summary>The <see cref="Keycode"/> for the <c>'p'</c> key; analogous to the <kbd>P</kbd> key</summary>
	P = 'p',

	/// <summary>The <see cref="Keycode"/> for the <c>'q'</c> key; analogous to the <kbd>Q</kbd> key</summary>
	Q = 'q',

	/// <summary>The <see cref="Keycode"/> for the <c>'r'</c> key; analogous to the <kbd>R</kbd> key</summary>
	R = 'r',

	/// <summary>The <see cref="Keycode"/> for the <c>'s'</c> key; analogous to the <kbd>S</kbd> key</summary>
	S = 's',

	/// <summary>The <see cref="Keycode"/> for the <c>'t'</c> key; analogous to the <kbd>T</kbd> key</summary>
	T = 't',

	/// <summary>The <see cref="Keycode"/> for the <c>'u'</c> key; analogous to the <kbd>U</kbd> key</summary>
	U = 'u',

	/// <summary>The <see cref="Keycode"/> for the <c>'v'</c> key; analogous to the <kbd>V</kbd> key</summary>
	V = 'v',

	/// <summary>The <see cref="Keycode"/> for the <c>'w'</c> key; analogous to the <kbd>W</kbd> key</summary>
	W = 'w',

	/// <summary>The <see cref="Keycode"/> for the <c>'x'</c> key; analogous to the <kbd>X</kbd> key</summary>
	X = 'x',

	/// <summary>The <see cref="Keycode"/> for the <c>'y'</c> key; analogous to the <kbd>Y</kbd> key</summary>
	Y = 'y',

	/// <summary>The <see cref="Keycode"/> for the <c>'z'</c> key; analogous to the <kbd>Z</kbd> key</summary>
	Z = 'z',

	/// <summary>The <see cref="Keycode"/> for the <c>'{'</c> (left brace) key</summary>
	LeftBrace = '{',

	/// <summary>The <see cref="Keycode"/> for the <c>'|'</c> (pipe / vertical bar) key</summary>
	Pipe = '|',

	/// <summary>The <see cref="Keycode"/> for the <c>'}'</c> (right brace) key</summary>
	RightBrace = '}',

	/// <summary>The <see cref="Keycode"/> for the <c>'~'</c> (tilde) key</summary>
	Tilde = '~',

	/// <summary>The <see cref="Keycode"/> for the <c>'\x7F'</c> (delete) key</summary>
	Delete = '\x7F',

	/// <summary>The <see cref="Keycode"/> for the <c>'±'</c> (plus-minus sign) key</summary>
	PlusMinus = '\xB1',

#pragma warning disable CS0618 // That's only allowed here

	/// <summary>The <see cref="Keycode"/> for the <kbd>⇪</kbd> (caps lock) key</summary>
	CapsLock = ScancodeMask | Scancode.CapsLock,

	/// <summary>The <see cref="Keycode"/> for the <kbd>F1</kbd> key</summary>
	F1 = ScancodeMask | Scancode.F1,

	/// <summary>The <see cref="Keycode"/> for the <kbd>F2</kbd> key</summary>
	F2 = ScancodeMask | Scancode.F2,

	/// <summary>The <see cref="Keycode"/> for the <kbd>F3</kbd> key</summary>
	F3 = ScancodeMask | Scancode.F3,

	/// <summary>The <see cref="Keycode"/> for the <kbd>F4</kbd> key</summary>
	F4 = ScancodeMask | Scancode.F4,

	/// <summary>The <see cref="Keycode"/> for the <kbd>F5</kbd> key</summary>
	F5 = ScancodeMask | Scancode.F5,

	/// <summary>The <see cref="Keycode"/> for the <kbd>F6</kbd> key</summary>
	F6 = ScancodeMask | Scancode.F6,

	/// <summary>The <see cref="Keycode"/> for the <kbd>F7</kbd> key</summary>
	F7 = ScancodeMask | Scancode.F7,

	/// <summary>The <see cref="Keycode"/> for the <kbd>F8</kbd> key</summary>
	F8 = ScancodeMask | Scancode.F8,

	/// <summary>The <see cref="Keycode"/> for the <kbd>F9</kbd> key</summary>
	F9 = ScancodeMask | Scancode.F9,

	/// <summary>The <see cref="Keycode"/> for the <kbd>F10</kbd> key</summary>
	F10 = ScancodeMask | Scancode.F10,

	/// <summary>The <see cref="Keycode"/> for the <kbd>F11</kbd> key</summary>
	F11 = ScancodeMask | Scancode.F11,

	/// <summary>The <see cref="Keycode"/> for the <kbd>F12</kbd> key</summary>
	F12 = ScancodeMask | Scancode.F12,

	/// <summary>The <see cref="Keycode"/> for the <kbd>PRINT SCREEN</kbd> (print screen) key</summary>
	PrintScreen = ScancodeMask | Scancode.PrintScreen,

	/// <summary>The <see cref="Keycode"/> for the <kbd>SCROLL</kbd> (scroll lock) key</summary>
	ScrollLock = ScancodeMask | Scancode.ScrollLock,

	/// <summary>The <see cref="Keycode"/> for the <kbd>PAUSE</kbd> (pause / break) key</summary>
	Pause = ScancodeMask | Scancode.Pause,

	/// <summary>The <see cref="Keycode"/> for the <kbd>INSERT</kbd> (insert) key</summary>
	Insert = ScancodeMask | Scancode.Insert,

	/// <summary>The <see cref="Keycode"/> for the <kbd>HOME</kbd> (home) key</summary>
	Home = ScancodeMask | Scancode.Home,

	/// <summary>The <see cref="Keycode"/> for the <kbd>PAGE UP</kbd> (page up) key</summary>
	PageUp = ScancodeMask | Scancode.PageUp,

	/// <summary>The <see cref="Keycode"/> for the <kbd>END</kbd> (end) key</summary>
	End = ScancodeMask | Scancode.End,

	/// <summary>The <see cref="Keycode"/> for the <kbd>PAGE DOWN</kbd> (page down) key</summary>
	PageDown = ScancodeMask | Scancode.PageDown,

	/// <summary>The <see cref="Keycode"/> for the <kbd>→</kbd> (right arrow) key</summary>
	Right = ScancodeMask | Scancode.Right,

	/// <summary>The <see cref="Keycode"/> for the <kbd>←</kbd> (left arrow) key</summary>
	Left = ScancodeMask | Scancode.Left,

	/// <summary>The <see cref="Keycode"/> for the <kbd>↓</kbd> (down arrow) key</summary>
	Down = ScancodeMask | Scancode.Down,

	/// <summary>The <see cref="Keycode" /> for the <kbd>↑</kbd> (up arrow) key</summary>
	Up = ScancodeMask | Scancode.Up,

	/// <summary>The <see cref="Keycode"/> for the <kbd>NUM</kbd> (num lock) key on PCs, or the <kbd>CLEAR</kbd> (clear) key on Macs</summary>
	NumLockOrClear = ScancodeMask | Scancode.NumLockOrClear,

	/// <summary>The <see cref="Keycode"/> for the <kbd>/</kbd> (divide) key on the keypad</summary>
	KeypadDivide = ScancodeMask | Scancode.KeypadDivide,

	/// <summary>The <see cref="Keycode"/> for the <kbd>*</kbd> (multiply) key on the keypad</summary>
	KeypadMultiply = ScancodeMask | Scancode.KeypadMultiply,

	/// <summary>The <see cref="Keycode" /> for the <kbd>-</kbd> (minus) key on the keypad</summary>
	KeypadMinus = ScancodeMask | Scancode.KeypadMinus,

	/// <summary>The <see cref="Keycode"/> for the <kbd>+</kbd> (divide) key on the keypad</summary>
	KeypadPlus = ScancodeMask | Scancode.KeypadPlus,

	/// <summary>The <see cref="Keycode"/> for the <kbd>⏎</kbd> (return / enter) key on the keypad</summary>
	KeypadEnter = ScancodeMask | Scancode.KeypadEnter,

	/// <summary>The <see cref="Keycode"/> for the <kbd>1</kbd> key on the keypad</summary>
	Keypad1 = ScancodeMask | Scancode.Keypad1,

	/// <summary>The <see cref="Keycode"/> for the <kbd>2</kbd> key on the keypad</summary>
	Keypad2 = ScancodeMask | Scancode.Keypad2,

	/// <summary>The <see cref="Keycode"/> for the <kbd>3</kbd> key on the keypad</summary>
	Keypad3 = ScancodeMask | Scancode.Keypad3,

	/// <summary>The <see cref="Keycode"/> for the <kbd>4</kbd> key on the keypad</summary>
	Keypad4 = ScancodeMask | Scancode.Keypad4,

	/// <summary>The <see cref="Keycode"/> for the <kbd>5</kbd> key on the keypad</summary>
	Keypad5 = ScancodeMask | Scancode.Keypad5,

	/// <summary>The <see cref="Keycode"/> for the <kbd>6</kbd> key on the keypad</summary>
	Keypad6 = ScancodeMask | Scancode.Keypad6,

	/// <summary>The <see cref="Keycode"/> for the <kbd>7</kbd> key on the keypad</summary>
	Keypad7 = ScancodeMask | Scancode.Keypad7,

	/// <summary>The <see cref="Keycode"/> for the <kbd>8</kbd> key on the keypad</summary>
	Keypad8 = ScancodeMask | Scancode.Keypad8,

	/// <summary>The <see cref="Keycode"/> for the <kbd>9</kbd> key on the keypad</summary>
	Keypad9 = ScancodeMask | Scancode.Keypad9,

	/// <summary>The <see cref="Keycode"/> for the <kbd>0</kbd> key on the keypad</summary>
	Keypad0 = ScancodeMask | Scancode.Keypad0,

	/// <summary>The <see cref="Keycode"/> for the <kbd>.</kbd> key on the keypad</summary>
	KeypadPeriod = ScancodeMask | Scancode.KeypadPeriod,

	/// <summary>The <see cref="Keycode"/> for the <kbd>☰</kbd> (context menu) key on Windows, or the <kbd>COMPOSE</kbd> (compose) key elsewhere</summary>
	Application = ScancodeMask | Scancode.Application,

	/// <summary>The <see cref="Keycode"/> that <em>might be</em> for the <kbd>POWER</kbd> (power) key on some Mac keyboards</summary>
	/// <remarks>
	/// <para>
	/// Do <em>not</em> rely on this, as the USB document says that this is a status flag instead of a physical key.
	/// </para>
	/// </remarks>
	[Experimental(diagnosticId: "SDL5010")] //TODO: make SDL5010 the diagnostics id for unknown or uncertain Keycodes or Scancodes
	Power = ScancodeMask | Scancode.Power,

	/// <summary>The <see cref="Keycode" /> for the <kbd>=</kbd> key</summary>
	KeypadEquals = ScancodeMask | Scancode.KeypadEquals,

	/// <summary>The <see cref="Keycode"/> for the <kbd>F13</kbd> key</summary>
	F13 = ScancodeMask | Scancode.F13,

	/// <summary>The <see cref="Keycode"/> for the <kbd>F14</kbd> key</summary>
	F14 = ScancodeMask | Scancode.F14,

	/// <summary>The <see cref="Keycode"/> for the <kbd>F15</kbd> key</summary>
	F15 = ScancodeMask | Scancode.F15,

	/// <summary>The <see cref="Keycode"/> for the <kbd>F16</kbd> key</summary>
	F16 = ScancodeMask | Scancode.F16,

	/// <summary>The <see cref="Keycode"/> for the <kbd>F17</kbd> key</summary>
	F17 = ScancodeMask | Scancode.F17,

	/// <summary>The <see cref="Keycode"/> for the <kbd>F18</kbd> key</summary>
	F18 = ScancodeMask | Scancode.F18,

	/// <summary>The <see cref="Keycode"/> for the <kbd>F19</kbd> key</summary>
	F19 = ScancodeMask | Scancode.F19,

	/// <summary>The <see cref="Keycode"/> for the <kbd>F20</kbd> key</summary>
	F20 = ScancodeMask | Scancode.F20,

	/// <summary>The <see cref="Keycode"/> for the <kbd>F21</kbd> key</summary>
	F21 = ScancodeMask | Scancode.F21,

	/// <summary>The <see cref="Keycode"/> for the <kbd>F22</kbd> key</summary>
	F22 = ScancodeMask | Scancode.F22,

	/// <summary>The <see cref="Keycode"/> for the <kbd>F23</kbd> key</summary>
	F23 = ScancodeMask | Scancode.F23,

	/// <summary>The <see cref="Keycode"/> for the <kbd>F24</kbd> key</summary>
	F24 = ScancodeMask | Scancode.F24,

	/// <summary>The <see cref="Keycode"/> for the <kbd>EXECUTE</kbd> (execute) key</summary>
	Execute = ScancodeMask | Scancode.Execute,

	/// <summary>The <see cref="Keycode"/> for the <kbd>HELP</kbd> (help) key</summary>
	Help = ScancodeMask | Scancode.Help,

	/// <summary>The <see cref="Keycode"/> for the <kbd>☰</kbd> (menu / show menu) key</summary>
	Menu = ScancodeMask | Scancode.Menu,

	/// <summary>The <see cref="Keycode"/> for the <kbd>SELECT</kbd> (select) key</summary>
	Select = ScancodeMask | Scancode.Select,

	/// <summary>The <see cref="Keycode"/> for the <kbd>STOP</kbd> (stop) key</summary>
	Stop = ScancodeMask | Scancode.Stop,

	/// <summary>The <see cref="Keycode"/> for the <kbd>AGAIN</kbd> (again / redo) key</summary>
	Again = ScancodeMask | Scancode.Again,

	/// <summary>The <see cref="Keycode"/> for the <kbd>UNDO</kbd> (undo) key</summary>
	Undo = ScancodeMask | Scancode.Undo,

	/// <summary>The <see cref="Keycode"/> for the <kbd>CUT</kbd> (cut) key</summary>
	Cut = ScancodeMask | Scancode.Cut,

	/// <summary>The <see cref="Keycode"/> for the <kbd>COPY</kbd> (copy) key</summary>
	Copy = ScancodeMask | Scancode.Copy,

	/// <summary>The <see cref="Keycode"/> for the <kbd>PASTE</kbd> (paste) key</summary>
	Paste = ScancodeMask | Scancode.Paste,

	/// <summary>The <see cref="Keycode"/> for the <kbd>🔍</kbd> (find) key</summary>
	Find = ScancodeMask | Scancode.Find,

	/// <summary>The <see cref="Keycode"/> for the <kbd>🔇</kbd> (mute volume) key</summary>
	Mute = ScancodeMask | Scancode.Mute,

	/// <summary>The <see cref="Keycode"/> for the <kbd>🔊</kbd> (volume up) key</summary>
	VolumeUp = ScancodeMask | Scancode.VolumeUp,

	/// <summary>The <see cref="Keycode"/> for the <kbd>🔉</kbd> (volume down) key</summary>
	VolumeDown = ScancodeMask | Scancode.VolumeDown,

	/// <summary>The <see cref="Keycode" /> for the <kbd>,</kbd> key on the keypad</summary>
	KeypadComma = ScancodeMask | Scancode.KeypadComma,

	/// <summary>The <see cref="Keycode"/> for the <kbd>=</kbd> AS400 key on the keypad</summary>
	KeypadEqualsAS400 = ScancodeMask | Scancode.KeypadEqualsAS400,

	// TODO: document the following Keycodes!
#pragma warning disable CS1591

	AltErase = ScancodeMask | Scancode.AltErase,

	SysReq = ScancodeMask | Scancode.SysReq,

	Cancel = ScancodeMask | Scancode.Cancel,

	Clear = ScancodeMask | Scancode.Clear,

	Prior = ScancodeMask | Scancode.Prior,

	Return2 = ScancodeMask | Scancode.Return2,

	Separator = ScancodeMask | Scancode.Separator,

	Out = ScancodeMask | Scancode.Out,

	Oper = ScancodeMask | Scancode.Oper,

	ClearAgain = ScancodeMask | Scancode.ClearAgain,

	CrSel = ScancodeMask | Scancode.CrSel,

	ExSel = ScancodeMask | Scancode.ExSel,

	Keypad00 = ScancodeMask | Scancode.Keypad00,

	Keypad000 = ScancodeMask | Scancode.Keypad000,

	ThousandsSeparator = ScancodeMask | Scancode.ThousandsSeparator,

	DecimalSeparator = ScancodeMask | Scancode.DecimalSeparator,

	CurrencyUnit = ScancodeMask | Scancode.CurrencyUnit,

	CurrencySubunit = ScancodeMask | Scancode.CurrencySubunit,

#pragma warning restore CS1591

	/// <summary>The <see cref="Keycode"/> for the <kbd>(</kbd> key on the keypad</summary>
	KeypadLeftParenthesis = ScancodeMask | Scancode.KeypadLeftParenthesis,

	/// <summary>The <see cref="Keycode"/> for the <kbd>)</kbd> key on the keypad</summary>
	KeypadRightParenthesis = ScancodeMask | Scancode.KeypadRightParenthesis,

	/// <summary>The <see cref="Keycode"/> for the <kbd>{</kbd> key on the keypad</summary>
	KeypadLeftBrace = ScancodeMask | Scancode.KeypadLeftBrace,

	/// <summary>The <see cref="Keycode"/> for the <kbd>}</kbd> key on the keypad</summary>
	KeypadRightBrace = ScancodeMask | Scancode.KeypadRightBrace,

	/// <summary>The <see cref="Keycode"/> for the <kbd>⭾</kbd> (tab) key on the keypad</summary>
	KeypadTab = ScancodeMask | Scancode.KeypadTab,

	/// <summary>The <see cref="Keycode"/> for the <kbd>⌫</kbd> (back space) key on the keypad</summary>
	KeypadBackspace = ScancodeMask | Scancode.KeypadBackspace,

	/// <summary>The <see cref="Keycode"/> for the <kbd>A</kbd> key on the keypad</summary>
	KeypadA = ScancodeMask | Scancode.KeypadA,

	/// <summary>The <see cref="Keycode"/> for the <kbd>B</kbd> key on the keypad</summary>
	KeypadB = ScancodeMask | Scancode.KeypadB,

	/// <summary>The <see cref="Keycode"/> for the <kbd>C</kbd> key on the keypad</summary>
	KeypadC = ScancodeMask | Scancode.KeypadC,

	/// <summary>The <see cref="Keycode"/> for the <kbd>D</kbd> key on the keypad</summary>
	KeypadD = ScancodeMask | Scancode.KeypadD,

	/// <summary>The <see cref="Keycode"/> for the <kbd>E</kbd> key on the keypad</summary>
	KeypadE = ScancodeMask | Scancode.KeypadE,

	/// <summary>The <see cref="Keycode"/> for the <kbd>F</kbd> key on the keypad</summary>
	KeypadF = ScancodeMask | Scancode.KeypadF,

	/// <summary>The <see cref="Keycode"/> for the <kbd>XOR</kbd> (xor; sometimes <c>^</c>) key on the keypad</summary>
	KeypadXor = ScancodeMask | Scancode.KeypadXor,

	/// <summary>The <see cref="Keycode" /> for the <kbd>xʸ</kbd> (power / exponentiation; sometimes <c>^</c>) key on the keypad</summary>
	KeypadPower = ScancodeMask | Scancode.KeypadPower,

	/// <summary>The <see cref="Keycode"/> for the <kbd>%</kbd> key on the keypad</summary>
	KeypadPercent = ScancodeMask | Scancode.KeypadPercent,

	/// <summary>The <see cref="Keycode"/> for the <kbd>&lt;</kbd> key on the keypad</summary>
	KeypadLess = ScancodeMask | Scancode.KeypadLess,

	/// <summary>The <see cref="Keycode"/> for the <kbd>&gt;</kbd> key on the keypad</summary>
	KeypadGreater = ScancodeMask | Scancode.KeypadGreater,

	/// <summary>The <see cref="Keycode"/> for the <kbd>&amp;</kbd> (and; sometimes <c>AND</c>) key on the keypad</summary>
	KeypadAmpersand = ScancodeMask | Scancode.KeypadAmpersand,

	/// <summary>The <see cref="Keycode"/> for the <kbd>&amp;&amp;</kbd> key on the keypad</summary>
	KeypadDoubleAmpersand = ScancodeMask | Scancode.KeypadDoubleAmpersand,

	/// <summary>The <see cref="Keycode"/> for the <kbd>|</kbd> (or; sometimes <c>OR</c>) key on the keypad</summary>
	KeypadVerticalBar = ScancodeMask | Scancode.KeypadVerticalBar,

	/// <summary>The <see cref="Keycode"/> for the <kbd>||</kbd> key on the keypad</summary>
	KeypadDoubleVerticalBar = ScancodeMask | Scancode.KeypadDoubleVerticalBar,

	/// <summary>The <see cref="Keycode"/> for the <kbd>:</kbd> key on the keypad</summary>
	KeypadColon = ScancodeMask | Scancode.KeypadColon,

	/// <summary>The <see cref="Keycode"/> for the <kbd>#</kbd> key on the keypad</summary>
	KeypadHash = ScancodeMask | Scancode.KeypadHash,

	/// <summary>The <see cref="Keycode"/> for the <kbd>⎵</kbd> (space) key on the keypad</summary>
	KeypadSpace = ScancodeMask | Scancode.KeypadSpace,

	/// <summary>The <see cref="Keycode"/> for the <kbd>@</kbd> key on the keypad</summary>
	KeypadAt = ScancodeMask | Scancode.KeypadAt,

	/// <summary>The <see cref="Keycode"/> for the <kbd>!</kbd> key on the keypad</summary>
	KeypadExclamationMark = ScancodeMask | Scancode.KeypadExclamationMark,

	/// <summary>The <see cref="Keycode"/> for the <kbd>MEM STORE</kbd> (mem store) key on the keypad</summary>
	KeypadMemStore = ScancodeMask | Scancode.KeypadMemStore,

	/// <summary>The <see cref="Keycode"/> for the <kbd>MEM RECALL</kbd> (mem recall) key on the keypad</summary>
	KeypadMemRecall = ScancodeMask | Scancode.KeypadMemRecall,

	/// <summary>The <see cref="Keycode"/> for the <kbd>MEM CLEAR</kbd> (mem clear) key on the keypad</summary>
	KeypadMemClear = ScancodeMask | Scancode.KeypadMemClear,

	/// <summary>The <see cref="Keycode"/> for the <kbd>MEM +</kbd> (mem add) key on the keypad</summary>
	KeypadMemAdd = ScancodeMask | Scancode.KeypadMemAdd,

	/// <summary>The <see cref="Keycode"/> for the <kbd>MEM -</kbd> (mem subtract) key on the keypad</summary>
	KeypadMemSubtract = ScancodeMask | Scancode.KeypadMemSubtract,

	/// <summary>The <see cref="Keycode"/> for the <kbd>MEM *</kbd> (mem multiply) key on the keypad</summary>
	KeypadMemMultiply = ScancodeMask | Scancode.KeypadMemMultiply,

	/// <summary>The <see cref="Keycode"/> for the <kbd>MEM /</kbd> (mem divide) key on the keypad</summary>
	KeypadMemDivide = ScancodeMask | Scancode.KeypadMemDivide,

	/// <summary>The <see cref="Keycode"/> for the <kbd>±</kbd> (plus-minus) key on the keypad</summary>
	KeypadPlusMinus = ScancodeMask | Scancode.KeypadPlusMinus,

	/// <summary>The <see cref="Keycode"/> for the <kbd>CLEAR</kbd> (clear) key on the keypad</summary>
	KeypadClear = ScancodeMask | Scancode.KeypadClear,

	// TODO: document the following Keycodes!
#pragma warning disable CS1591

	KeypadClearEntry = ScancodeMask | Scancode.KeypadClearEntry,

	KeypadBinary = ScancodeMask | Scancode.KeypadBinary,

	KeypadOctal = ScancodeMask | Scancode.KeypadOctal,

	KeypadDecimal = ScancodeMask | Scancode.KeypadDecimal,

	KeypadHexadecimal = ScancodeMask | Scancode.KeypadHexadecimal,

#pragma warning restore CS1591

	/// <summary>The <see cref="Keycode"/> for the left <kbd>CTRL</kbd> (left control) key</summary>
	LeftControl = ScancodeMask | Scancode.LeftControl,

	/// <summary>The <see cref="Keycode"/> for the left <kbd>⇧</kbd> (left shift) key</summary>
	LeftShift = ScancodeMask | Scancode.LeftShift,

	/// <summary>The <see cref="Keycode"/> for the left <kbd>ALT</kbd> (left alt) key</summary>
	LeftAlt = ScancodeMask | Scancode.LeftAlt,

	/// <summary>The <see cref="Keycode"/> for the left <kbd>⊞</kbd> (left Windows) key on PCs, left <kbd>⌘</kbd> (left command) key on Macs, or sometimes called left meta key</summary>
	LeftGui = ScancodeMask | Scancode.LeftGui,

	/// <summary>The <see cref="Keycode"/> for the right <kbd>CTRL</kbd> (right control) key</summary>
	RightControl = ScancodeMask | Scancode.RightControl,

	/// <summary>The <see cref="Keycode"/> for the right <kbd>⇧</kbd> (right shift) key</summary>
	RightShift = ScancodeMask | Scancode.RightShift,

	/// <summary>The <see cref="Keycode"/> for the right <kbd>ALT</kbd> (right alt) key, or sometimes called <kbd>ALT GR</kbd> (alt gr.) key</summary>
	RightAlt = ScancodeMask | Scancode.RightAlt,

	/// <summary>The <see cref="Keycode"/> for the right <kbd>⊞</kbd> (right Windows) key on PCs, right <kbd>⌘</kbd> (right command) key on Macs, or sometimes called right meta key</summary>
	RightGui = ScancodeMask | Scancode.RightGui,

	// TODO: document the following Keycodes!
#pragma warning disable CS1591

	Mode = ScancodeMask | Scancode.Mode,

	Sleep = ScancodeMask | Scancode.Sleep,

	Wake = ScancodeMask | Scancode.Wake,

	ChannelIncrement = ScancodeMask | Scancode.ChannelIncrement,

	ChannelDecrement = ScancodeMask | Scancode.ChannelDecrement,

#pragma warning restore CS1591

	/// <summary>The <see cref="Keycode"/> for the <kbd>⏵</kbd> (media play) key</summary>
	MediaPlay = ScancodeMask | Scancode.MediaPlay,

	/// <summary>The <see cref="Keycode"/> for the <kbd>⏸</kbd> (media pause) key</summary>
	MediaPause = ScancodeMask | Scancode.MediaPause,

	/// <summary>The <see cref="Keycode"/> for the <kbd>⏺</kbd> (media record) key</summary>
	MediaRecord = ScancodeMask | Scancode.MediaRecord,

	/// <summary>The <see cref="Keycode"/> for the <kbd>⏩</kbd> (media fast forward) key</summary>
	MediaFastForward = ScancodeMask | Scancode.MediaFastForward,

	/// <summary>The <see cref="Keycode"/> for the <kbd>⏪</kbd> (media rewind) key</summary>
	MediaRewind = ScancodeMask | Scancode.MediaRewind,

	/// <summary>The <see cref="Keycode"/> for the <kbd>⏭</kbd> (media next track) key</summary>
	MediaNextTrack = ScancodeMask | Scancode.MediaNextTrack,

	/// <summary>The <see cref="Keycode"/> for the <kbd>⏮</kbd> (media previous track) key</summary>
	MediaPreviousTrack = ScancodeMask | Scancode.MediaPreviousTrack,

	/// <summary>The <see cref="Keycode"/> for the <kbd>⏹</kbd> (media stop) key</summary>
	MediaStop = ScancodeMask | Scancode.MediaStop,

	/// <summary>The <see cref="Keycode"/> for the <kbd>⏏</kbd> (media eject) key</summary>
	MediaEject = ScancodeMask | Scancode.MediaEject,

	/// <summary>The <see cref="Keycode"/> for the <kbd>⏯</kbd> (play-pause eject) key</summary>
	MediaPlayPause = ScancodeMask | Scancode.MediaPlayPause,

	// TODO: document the following Keycodes!
#pragma warning disable CS1591

	MediaSelect = ScancodeMask | Scancode.MediaSelect,

#pragma warning restore CS1591

	/// <summary>The <see cref="Keycode"/> for the application control "<em>New</em>" key</summary>
	ApplicationControlNew = ScancodeMask | Scancode.ApplicationControlNew,

	/// <summary>The <see cref="Keycode"/> for the application control "<em>Open</em>" key</summary>
	ApplicationControlOpen = ScancodeMask | Scancode.ApplicationControlOpen,

	/// <summary>The <see cref="Keycode"/> for the application control "<em>Close</em>" key</summary>
	ApplicationControlClose = ScancodeMask | Scancode.ApplicationControlClose,

	/// <summary>The <see cref="Keycode"/> for the application control "<em>Exit</em>" key</summary>
	ApplicationControlExit = ScancodeMask | Scancode.ApplicationControlExit,

	/// <summary>The <see cref="Keycode"/> for the application control "<em>Save</em>" key</summary>
	ApplicationControlSave = ScancodeMask | Scancode.ApplicationControlSave,

	/// <summary>The <see cref="Keycode"/> for the application control "<em>Print</em>" key</summary>
	ApplicationControlPrint = ScancodeMask | Scancode.ApplicationControlPrint,

	/// <summary>The <see cref="Keycode"/> for the application control "<em>Properties</em>" key</summary>
	ApplicationControlProperties = ScancodeMask | Scancode.ApplicationControlProperties,

	/// <summary>The <see cref="Keycode"/> for the application control "<em>Search</em>" key</summary>
	ApplicationControlSearch = ScancodeMask | Scancode.ApplicationControlSearch,

	/// <summary>The <see cref="Keycode"/> for the application control "<em>Home</em>" key</summary>
	ApplicationControlHome = ScancodeMask | Scancode.ApplicationControlHome,

	/// <summary>The <see cref="Keycode"/> for the application control "<em>Back</em>" key</summary>
	ApplicationControlBack = ScancodeMask | Scancode.ApplicationControlBack,

	/// <summary>The <see cref="Keycode"/> for the application control "<em>Forward</em>" key</summary>
	ApplicationControlForward = ScancodeMask | Scancode.ApplicationControlForward,

	/// <summary>The <see cref="Keycode"/> for the application control "<em>Stop</em>" key</summary>
	ApplicationControlStop = ScancodeMask | Scancode.ApplicationControlStop,

	/// <summary>The <see cref="Keycode"/> for the application control "<em>Refresh</em>" key</summary>
	ApplicationControlRefresh = ScancodeMask | Scancode.ApplicationControlRefresh,

	/// <summary>The <see cref="Keycode"/> for the application control "<em>Bookmarks</em>" key</summary>
	ApplicationControlBookmarks = ScancodeMask | Scancode.ApplicationControlBookmarks,

	/// <summary>The <see cref="Keycode"/> for left software defined key on mobiles</summary>
	/// <remarks>
	/// <para>
	/// This key is usually situated below the display on phones and used as a multi-function feature key for selecting a software defined function shown on the bottom left of the display.
	/// </para>
	/// </remarks>
	SoftwareLeft = ScancodeMask | Scancode.SoftwareLeft,

	/// <summary>The <see cref="Keycode"/> for right software defined key on mobiles</summary>
	/// <remarks>
	/// <para>
	/// This key is usually situated below the display on phones and used as a multi-function feature key for selecting a software defined function shown on the bottom right of the display.
	/// </para>
	/// </remarks>
	SoftwareRight = ScancodeMask | Scancode.SoftwareRight,

	/// <summary>The <see cref="Keycode"/> for the <kbd>📞</kbd> (phone call) key on mobiles</summary>
	Call = ScancodeMask | Scancode.Call,

	/// <summary>The <see cref="Keycode"/> for the <kbd>END 📞</kbd> (end phone call) key on mobiles</summary>
	EndCall = ScancodeMask | Scancode.EndCall,

#pragma warning restore CS0618

#pragma warning disable CS0618 // That's only allowed here

	/// <summary>The <see cref="Keycode"/> for the "Left Tab" key</summary>
	LeftTab = ExtendedMask | 0x01u,

	/// <summary>The <see cref="Keycode"/> for the "Level 5 Shift" key</summary>
	Level5Shift = ExtendedMask | 0x02u,

	/// <summary>The <see cref="Keycode" /> for the "Level 5 Shift" key</summary>
	MultiKeyCompose = ExtendedMask | 0x03u,

	/// <summary>The <see cref="Keycode" /> for the "Left Meta" key</summary>
	LeftMeta = ExtendedMask | 0x04u,

	/// <summary>The <see cref="Keycode" /> for the "Right Meta" key</summary>
	RightMeta = ExtendedMask | 0x05u,

	/// <summary>The <see cref="Keycode" /> for the "Left Hyper" key</summary>
	LeftHyper = ExtendedMask | 0x06u,

	/// <summary>The <see cref="Keycode" /> for the "Right Hyper" key</summary>
	RightHyper = ExtendedMask | 0x07u

#pragma warning restore CS0618
}
