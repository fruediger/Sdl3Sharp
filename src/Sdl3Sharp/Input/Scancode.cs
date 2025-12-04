using System.Diagnostics.CodeAnalysis;

namespace Sdl3Sharp.Input;

/// <summary>
/// Represents a keyboard scancode
/// </summary>
/// <remarks>
/// <para>
/// This represents a scancode for a key on a keyboard, <em>which is different to a <see cref="Keycode">virtual key</see></em>.
/// </para>
/// </remarks>
public enum Scancode
{
	/// <summary>A representative for an unknown <see cref="Scancode"/></summary>
	Unknown = 0,

	/// <summary>The <see cref="Scancode"/> for the <kbd>A</kbd> key</summary>
	A = 4,

	/// <summary>The <see cref="Scancode"/> for the <kbd>B</kbd> key</summary>
	B = 5,

	/// <summary>The <see cref="Scancode"/> for the <kbd>C</kbd> key</summary>
	C = 6,

	/// <summary>The <see cref="Scancode"/> for the <kbd>D</kbd> key</summary>
	D = 7,

	/// <summary>The <see cref="Scancode"/> for the <kbd>E</kbd> key</summary>
	E = 8,

	/// <summary>The <see cref="Scancode"/> for the <kbd>F</kbd> key</summary>
	F = 9,

	/// <summary>The <see cref="Scancode"/> for the <kbd>G</kbd> key</summary>
	G = 10,

	/// <summary>The <see cref="Scancode"/> for the <kbd>H</kbd> key</summary>
	H = 11,

	/// <summary>The <see cref="Scancode"/> for the <kbd>I</kbd> key</summary>
	I = 12,

	/// <summary>The <see cref="Scancode"/> for the <kbd>J</kbd> key</summary>
	J = 13,

	/// <summary>The <see cref="Scancode"/> for the <kbd>K</kbd> key</summary>
	K = 14,

	/// <summary>The <see cref="Scancode"/> for the <kbd>L</kbd> key</summary>
	L = 15,

	/// <summary>The <see cref="Scancode"/> for the <kbd>M</kbd> key</summary>
	M = 16,

	/// <summary>The <see cref="Scancode"/> for the <kbd>N</kbd> key</summary>
	N = 17,

	/// <summary>The <see cref="Scancode"/> for the <kbd>O</kbd> key</summary>
	O = 18,

	/// <summary>The <see cref="Scancode"/> for the <kbd>P</kbd> key</summary>
	P = 19,

	/// <summary>The <see cref="Scancode"/> for the <kbd>Q</kbd> key</summary>
	Q = 20,

	/// <summary>The <see cref="Scancode"/> for the <kbd>R</kbd> key</summary>
	R = 21,

	/// <summary>The <see cref="Scancode"/> for the <kbd>S</kbd> key</summary>
	S = 22,

	/// <summary>The <see cref="Scancode"/> for the <kbd>T</kbd> key</summary>
	T = 23,

	/// <summary>The <see cref="Scancode"/> for the <kbd>U</kbd> key</summary>
	U = 24,

	/// <summary>The <see cref="Scancode"/> for the <kbd>V</kbd> key</summary>
	V = 25,

	/// <summary>The <see cref="Scancode"/> for the <kbd>W</kbd> key</summary>
	W = 26,

	/// <summary>The <see cref="Scancode"/> for the <kbd>X</kbd> key</summary>
	X = 27,

	/// <summary>The <see cref="Scancode"/> for the <kbd>Y</kbd> key</summary>
	Y = 28,

	/// <summary>The <see cref="Scancode"/> for the <kbd>Z</kbd> key</summary>
	Z = 29,

	/// <summary>The <see cref="Scancode"/> for the <kbd>1</kbd> key</summary>
	_1 = 30,

	/// <summary>The <see cref="Scancode"/> for the <kbd>2</kbd> key</summary>
	_2 = 31,

	/// <summary>The <see cref="Scancode"/> for the <kbd>3</kbd> key</summary>
	_3 = 32,

	/// <summary>The <see cref="Scancode"/> for the <kbd>4</kbd> key</summary>
	_4 = 33,

	/// <summary>The <see cref="Scancode"/> for the <kbd>5</kbd> key</summary>
	_5 = 34,

	/// <summary>The <see cref="Scancode"/> for the <kbd>6</kbd> key</summary>
	_6 = 35,

	/// <summary>The <see cref="Scancode"/> for the <kbd>_7</kbd> key</summary>
	_7 = 36,

	/// <summary>The <see cref="Scancode"/> for the <kbd>8</kbd> key</summary>
	_8 = 37,

	/// <summary>The <see cref="Scancode"/> for the <kbd>9</kbd> key</summary>
	_9 = 38,

	/// <summary>The <see cref="Scancode"/> for the <kbd>0</kbd> key</summary>
	_0 = 39,

	/// <summary>The <see cref="Scancode"/> for the <kbd>⏎</kbd> (return / enter) key</summary>
	Return = 40,

	/// <summary>The <see cref="Scancode"/> for the <kbd>ESC</kbd> (escape) key</summary>
	Escape = 41,

	/// <summary>The <see cref="Scancode"/> for the <kbd>⌫</kbd> (back space) key</summary>
	Backspace = 42,

	/// <summary>The <see cref="Scancode"/> for the <kbd>⭾</kbd> (tab) key</summary>
	Tab = 43,

	/// <summary>The <see cref="Scancode"/> for the <kbd>⎵</kbd> (space) key</summary>
	Space = 44,

	/// <summary>The <see cref="Scancode"/> for the <kbd>-</kbd> key</summary>
	Minus = 45,

	/// <summary>The <see cref="Scancode"/> for the <kbd>=</kbd> key</summary>
	Equals = 46,

	/// <summary>The <see cref="Scancode"/> for the <kbd>[</kbd> key</summary>
	LeftBracket = 47,

	/// <summary>The <see cref="Scancode"/> for the <kbd>]</kbd> key</summary>
	RightBracket = 48,

	/// <summary>The <see cref="Scancode"/> for the <kbd>\</kbd> key in US layouts</summary>
	/// <remarks>
	/// <para>
	/// This key Located at the lower left of the <kbd>⏎</kbd> (return / enter) key on ISO keyboards and at the right end of the <kbd>Q</kbd><kbd>W</kbd><kbd>E</kbd><kbd>R</kbd><kbd>T</kbd><kbd>Y</kbd> row on ANSI keyboards.
	/// </para>
	/// <para>
	/// This key produces:
	/// <list type="bullet">
	///		<item><description>'\' (back slash) and '|' (vertical line) in a US layout and in a UK Mac layout</description></item>
	///		<item><description>'#' (number sign) and '~' (tilde) in a UK Windows layout</description></item>
	///		<item><description>'$' (dollar sign) and '£' (pound sign) in a Swiss German layout</description></item>
	///		<item><description>'#' (number sign) and ''' (apostrophe) in a German layout</description></item>
	///		<item><description>'`' (accent grave) and '£' (pound sign) in a French Mac layout</description></item>
	///		<item><description>'*' (asterisk) and 'µ' (micro sign) in a French Windows layout</description></item>
	///	</list>
	///	</para>
	/// </remarks>
	Backslash = 49,

	/// <summary>The alternative <see cref="Scancode"/> that ISO USB keyboards use for <see cref="Backslash"/></summary>
	/// <remarks>
	/// <para>
	/// All relevant operating systems treat this <see cref="Scancode"/> and <see cref="Backslash"/> identically.
	/// </para>
	/// <para>
	/// You should not generate this <see cref="Scancode"/>, nor should you rely on this <see cref="Scancode"/> getting generated, because SDL will never do that with most keyboards.
	/// </para>
	/// </remarks>
	NonUsHash = 50,

	/// <summary>The <see cref="Scancode"/> for the <kbd>;</kbd> key</summary>
	Semicolon = 51,

	/// <summary>The <see cref="Scancode"/> for the <kbd>'</kbd> key</summary>
	Apostrophe = 52,

	/// <summary>The <see cref="Scancode"/> for the <kbd>`</kbd> key in US layouts</summary>
	/// <remarks>
	/// <para>
	/// This key Located at the top left corner.
	/// </para>
	/// <para>
	/// This key produces:
	/// <list type="bullet">
	///		<item><description>'`' (accent grave) and '~' (tilde) in a US Windows layout and in US and UK Mac layouts on ASNI keyboards</description></item>
	///		<item><description>'`' (accent grave) and '¬' (not sign) in a UK Windows layout</description></item>
	///		<item><description>'§' (section sign) and '±' (plus-minus sign) in US and UK Mac layouts on ISO keyboards</description></item>
	///		<item><description>'§' (section sign) and '°' (degree sign) in a Swiss German layout (for Macs only on ISO keyboards)</description></item>
	///		<item><description>'^' (accent circumflex) and '°' (degree sign) in a German layout (for Macs only on ISO keyboards)</description></item>
	///		<item><description>'²' (superscript two) and '~' (tilde) in a French Windows layout</description></item>
	///		<item><description>'@' (commercial at sign) and '#' (number sign) in a French Mac layout on ISO keyboards</description></item>
	///		<item><description>'&lt;' (less than sign) and '&gt;' (greater than sign) in a Swiss German, German, or French Mac layout on ANSI keyboards</description></item>
	///	</list>
	///	</para>
	/// </remarks>
	Grave = 53,

	/// <summary>The <see cref="Scancode"/> for the <kbd>,</kbd> key</summary>
	Comma = 54,

	/// <summary>The <see cref="Scancode"/> for the <kbd>.</kbd> key</summary>
	Period = 55,

	/// <summary>The <see cref="Scancode"/> for the <kbd>/</kbd> key</summary>
	Slash = 56,

	/// <summary>The <see cref="Scancode"/> for the <kbd>⇪</kbd> (caps lock) key</summary>
	CapsLock = 57,

	/// <summary>The <see cref="Scancode"/> for the <kbd>F1</kbd> key</summary>
	F1 = 58,

	/// <summary>The <see cref="Scancode"/> for the <kbd>F2</kbd> key</summary>
	F2 = 59,

	/// <summary>The <see cref="Scancode"/> for the <kbd>F3</kbd> key</summary>
	F3 = 60,

	/// <summary>The <see cref="Scancode"/> for the <kbd>F4</kbd> key</summary>
	F4 = 61,

	/// <summary>The <see cref="Scancode"/> for the <kbd>F5</kbd> key</summary>
	F5 = 62,

	/// <summary>The <see cref="Scancode"/> for the <kbd>F6</kbd> key</summary>
	F6 = 63,

	/// <summary>The <see cref="Scancode"/> for the <kbd>F7</kbd> key</summary>
	F7 = 64,

	/// <summary>The <see cref="Scancode"/> for the <kbd>F8</kbd> key</summary>
	F8 = 65,

	/// <summary>The <see cref="Scancode"/> for the <kbd>F9</kbd> key</summary>
	F9 = 66,

	/// <summary>The <see cref="Scancode"/> for the <kbd>F10</kbd> key</summary>
	F10 = 67,

	/// <summary>The <see cref="Scancode"/> for the <kbd>F11</kbd> key</summary>
	F11 = 68,

	/// <summary>The <see cref="Scancode"/> for the <kbd>F12</kbd> key</summary>
	F12 = 69,

	/// <summary>The <see cref="Scancode"/> for the <kbd>PRINT SCREEN</kbd> (print screen) key</summary>
	PrintScreen = 70,

	/// <summary>The <see cref="Scancode"/> for the <kbd>SCROLL</kbd> (scroll lock) key</summary>
	ScrollLock = 71,

	/// <summary>The <see cref="Scancode"/> for the <kbd>PAUSE</kbd> (pause / break) key</summary>
	Pause = 72,

	/// <summary>The <see cref="Scancode"/> for the <kbd>INSERT</kbd> (insert) key</summary>
	/// <remarks>
	/// <para>
	/// On some Mac keyboards this is actual send as the <see cref="Scancode"/> for the <kbd>HELP</kbd> (help) key instead of <see cref="Help"/>.
	/// </para>
	/// </remarks>
	Insert = 73,

	/// <summary>The <see cref="Scancode"/> for the <kbd>HOME</kbd> (home) key</summary>
	Home = 74,

	/// <summary>The <see cref="Scancode"/> for the <kbd>PAGE UP</kbd> (page up) key</summary>
	PageUp = 75,

	/// <summary>The <see cref="Scancode"/> for the <kbd>DELETE</kbd> (delete) key</summary>
	Delete = 76,

	/// <summary>The <see cref="Scancode" /> for the <kbd>END</kbd> (end) key</summary>
	End = 77,

	/// <summary>The <see cref="Scancode"/> for the <kbd>PAGE DOWN</kbd> (page down) key</summary>
	PageDown = 78,

	/// <summary>The <see cref="Scancode"/> for the <kbd>→</kbd> (right arrow) key</summary>
	Right = 79,

	/// <summary>The <see cref="Scancode"/> for the <kbd>←</kbd> (left arrow) key</summary>
	Left = 80,

	/// <summary>The <see cref="Scancode"/> for the <kbd>↓</kbd> (down arrow) key</summary>
	Down = 81,

	/// <summary>The <see cref="Scancode"/> for the <kbd>↑</kbd> (up arrow) key</summary>
	Up = 82,

	/// <summary>The <see cref="Scancode"/> for the <kbd>NUM</kbd> (num lock) key on PCs, or the <kbd>CLEAR</kbd> (clear) key on Macs</summary>
	NumLockOrClear = 83,

	/// <summary>The <see cref="Scancode"/> for the <kbd>/</kbd> (divide) key on the keypad</summary>
	KeypadDivide = 84,

	/// <summary>The <see cref="Scancode"/> for the <kbd>*</kbd> (multiply) key on the keypad</summary>
	KeypadMultiply = 85,

	/// <summary>The <see cref="Scancode"/> for the <kbd>-</kbd> (minus) key on the keypad</summary>
	KeypadMinus = 86,

	/// <summary>The <see cref="Scancode"/> for the <kbd>+</kbd> (divide) key on the keypad</summary>
	KeypadPlus = 87,

	/// <summary>The <see cref="Scancode"/> for the <kbd>⏎</kbd> (return / enter) key on the keypad</summary>
	KeypadEnter = 88,

	/// <summary>The <see cref="Scancode"/> for the <kbd>1</kbd> key on the keypad</summary>
	Keypad1 = 89,

	/// <summary>The <see cref="Scancode"/> for the <kbd>2</kbd> key on the keypad</summary>
	Keypad2 = 90,

	/// <summary>The <see cref="Scancode"/> for the <kbd>3</kbd> key on the keypad</summary>
	Keypad3 = 91,

	/// <summary>The <see cref="Scancode"/> for the <kbd>4</kbd> key on the keypad</summary>
	Keypad4 = 92,

	/// <summary>The <see cref="Scancode"/> for the <kbd>5</kbd> key on the keypad</summary>
	Keypad5 = 93,

	/// <summary>The <see cref="Scancode"/> for the <kbd>6</kbd> key on the keypad</summary>
	Keypad6 = 94,

	/// <summary>The <see cref="Scancode"/> for the <kbd>7</kbd> key on the keypad</summary>
	Keypad7 = 95,

	/// <summary>The <see cref="Scancode"/> for the <kbd>8</kbd> key on the keypad</summary>
	Keypad8 = 96,

	/// <summary>The <see cref="Scancode"/> for the <kbd>9</kbd> key on the keypad</summary>
	Keypad9 = 97,

	/// <summary>The <see cref="Scancode"/> for the <kbd>0</kbd> key on the keypad</summary>
	Keypad0 = 98,

	/// <summary>The <see cref="Scancode"/> for the <kbd>.</kbd> key on the keypad</summary>
	KeypadPeriod = 99,

	/// <summary>The <see cref="Scancode"/> for the additional key, located between the left <kbd>⇧</kbd> (shift) key and the <kbd>Y</kbd> key, on ISO keyboards</summary>
	/// <remarks>
	/// <para>
	/// This is the additional key that ISO keyboards have over ANSI ones, located between the left <kbd>⇧</kbd> (shift) key and the <kbd>Y</kbd> key.
	/// </para>
	/// <para>
	/// This key produces:
	/// <list type="bullet">
	///		<item><description>'`' (accent grave) and '~' (tilde) in a US or UK Mac layout</description></item>
	///		<item><description>'\' (back slash) and '|' (vertical line) in a US or UK Windows layout</description></item>
	///		<item><description>'&lt;' (less than sign) and '&gt;' (greater than sign) in a Swiss German, German, or French layout</description></item>
	///	</list>
	///	</para>
	/// </remarks>
	NonUsBackslash = 100,

	/// <summary>The <see cref="Scancode"/> for the <kbd>☰</kbd> (context menu) key on Windows, or the <kbd>COMPOSE</kbd> (compose) key elsewhere</summary>
	Application = 101,

	/// <summary>The <see cref="Scancode"/> that <em>might be</em> for the <kbd>POWER</kbd> (power) key on some Mac keyboards</summary>
	/// <remarks>
	/// <para>
	/// Do <em>not</em> rely on this, as the USB document says that this is a status flag instead of a physical key.
	/// </para>
	/// </remarks>
	[Experimental(diagnosticId: "SDL5010")] //TODO: make SDL5010 the diagnostics id for unknown or uncertain Keycodes or Scancodes
	Power = 102,

	/// <summary>The <see cref="Scancode"/> for the <kbd>=</kbd> key on the keypad</summary>
	KeypadEquals = 103,

	/// <summary>The <see cref="Scancode"/> for the <kbd>F13</kbd> key</summary>
	F13 = 104,

	/// <summary>The <see cref="Scancode"/> for the <kbd>F14</kbd> key</summary>
	F14 = 105,

	/// <summary>The <see cref="Scancode"/> for the <kbd>F15</kbd> key</summary>
	F15 = 106,

	/// <summary>The <see cref="Scancode"/> for the <kbd>F16</kbd> key</summary>
	F16 = 107,

	/// <summary>The <see cref="Scancode"/> for the <kbd>F17</kbd> key</summary>
	F17 = 108,

	/// <summary>The <see cref="Scancode"/> for the <kbd>F18</kbd> key</summary>
	F18 = 109,

	/// <summary>The <see cref="Scancode"/> for the <kbd>F19</kbd> key</summary>
	F19 = 110,

	/// <summary>The <see cref="Scancode"/> for the <kbd>F20</kbd> key</summary>
	F20 = 111,

	/// <summary>The <see cref="Scancode"/> for the <kbd>F21</kbd> key</summary>
	F21 = 112,

	/// <summary>The <see cref="Scancode"/> for the <kbd>F22</kbd> key</summary>
	F22 = 113,

	/// <summary>The <see cref="Scancode"/> for the <kbd>F23</kbd> key</summary>
	F23 = 114,

	/// <summary>The <see cref="Scancode"/> for the <kbd>F24</kbd> key</summary>
	F24 = 115,

	/// <summary>The <see cref="Scancode"/> for the <kbd>EXECUTE</kbd> (execute) key</summary>
	Execute = 116,

	/// <summary>The <see cref="Scancode"/> for the <kbd>HELP</kbd> (help) key</summary>
	Help = 117,

	/// <summary>The <see cref="Scancode"/> for the <kbd>☰</kbd> (menu / show menu) key</summary>
	Menu = 118,

	/// <summary>The <see cref="Scancode"/> for the <kbd>SELECT</kbd> (select) key</summary>
	Select = 119,

	/// <summary>The <see cref="Scancode"/> for the <kbd>STOP</kbd> (stop) key</summary>
	Stop = 120,

	/// <summary>The <see cref="Scancode"/> for the <kbd>AGAIN</kbd> (again / redo) key</summary>
	Again = 121,

	/// <summary>The <see cref="Scancode"/> for the <kbd>UNDO</kbd> (undo) key</summary>
	Undo = 122,

	/// <summary>The <see cref="Scancode"/> for the <kbd>CUT</kbd> (cut) key</summary>
	Cut = 123,

	/// <summary>The <see cref="Scancode"/> for the <kbd>COPY</kbd> (copy) key</summary>
	Copy = 124,

	/// <summary>The <see cref="Scancode"/> for the <kbd>PASTE</kbd> (paste) key</summary>
	Paste = 125,

	/// <summary>The <see cref="Scancode"/> for the <kbd>🔍</kbd> (find) key</summary>
	Find = 126,

	/// <summary>The <see cref="Scancode"/> for the <kbd>🔇</kbd> (mute volume) key</summary>
	Mute = 127,

	/// <summary>The <see cref="Scancode"/> for the <kbd>🔊</kbd> (volume up) key</summary>
	VolumeUp = 128,

	/// <summary>The <see cref="Scancode"/> for the <kbd>🔉</kbd> (volume down) key</summary>
	VolumeDown = 129,

	/* The original source (https://github.com/libsdl-org/SDL/blob/main/include/SDL3/SDL_scancode.h#L250) is not sure whether to enable these or not
	 *  LockingCapsLock = 130,
	 *  LockingCapsLock = 130,
	 *  LockingScrollLock = 132,
	 */

	/// <summary>The <see cref="Scancode"/> for the <kbd>,</kbd> key on the keypad</summary>
	KeypadComma = 133,

	/// <summary>The <see cref="Scancode"/> for the <kbd>=</kbd> AS400 key on the keypad</summary>
	KeypadEqualsAS400 = 134,

	// TODO: document the following Scancodes or not?
#pragma warning disable CS1591

	/// <remarks>
	/// <para>
	/// Used on Asian keyboards, see footnotes in USB keyboard documentation.
	/// </para>
	/// </remarks>
	International1 = 135,

	International2 = 136,

	/// <remarks>
	/// <para>
	/// Yen.
	/// </para>
	/// </remarks>
	International3 = 137,

	International4 = 138,

	International5 = 139,

	International6 = 140,

	International7 = 141,

	International8 = 142,

	International9 = 143,

	/// <remarks>
	/// <para>
	/// Hangul/English toggle.
	/// </para>
	/// </remarks>
	Language1 = 144,

	/// <remarks>
	/// <para>
	/// Hanja conversion.
	/// </para>
	/// </remarks>
	Language2 = 145,

	/// <remarks>
	/// <para>
	/// Katakana.
	/// </para>
	/// </remarks>
	Language3 = 146,

	/// <remarks>
	/// <para>
	/// Hiragana.
	/// </para>
	/// </remarks>
	Language4 = 147,

	/// <remarks>
	/// <para>
	/// Zenkaku/Hankaku.
	/// </para>
	/// </remarks>
	Language5 = 148,

	[Experimental(diagnosticId: "SDL5011")] // TODO: make SDL5011 the diagnostics id for 'reserved scancodes - do not use'
	Language6 = 149,

	[Experimental(diagnosticId: "SDL5011")] // TODO: make SDL5011 the diagnostics id for 'reserved scancodes - do not use'
	Language7 = 150,

	[Experimental(diagnosticId: "SDL5011")] // TODO: make SDL5011 the diagnostics id for 'reserved scancodes - do not use'
	Language8 = 151,

	[Experimental(diagnosticId: "SDL5011")] // TODO: make SDL5011 the diagnostics id for 'reserved scancodes - do not use'
	Language9 = 152,

#pragma warning restore CS1591

	// TODO: document the following Scancodes!
#pragma warning disable CS1591

	AltErase = 153,

	SysReq = 154,

	Cancel = 155,

	Clear = 156,

	Prior = 157,

	Return2 = 158,

	Separator = 159,

	Out = 160,

	Oper = 161,

	ClearAgain = 162,

	CrSel = 163,

	ExSel = 164,

	Keypad00 = 176,

	Keypad000 = 177,

	ThousandsSeparator = 178,

	DecimalSeparator = 179,

	CurrencyUnit = 180,

	CurrencySubunit = 181,

#pragma warning restore CS1591

	/// <summary>The <see cref="Scancode"/> for the <kbd>(</kbd> key on the keypad</summary>
	KeypadLeftParenthesis = 182,

	/// <summary>The <see cref="Scancode"/> for the <kbd>)</kbd> key on the keypad</summary>
	KeypadRightParenthesis = 183,

	/// <summary>The <see cref="Scancode"/> for the <kbd>{</kbd> key on the keypad</summary>
	KeypadLeftBrace = 184,

	/// <summary>The <see cref="Scancode"/> for the <kbd>}</kbd> key on the keypad</summary>
	KeypadRightBrace = 185,

	/// <summary>The <see cref="Scancode"/> for the <kbd>⭾</kbd> (tab) key on the keypad</summary>
	KeypadTab = 186,

	/// <summary>The <see cref="Scancode"/> for the <kbd>⌫</kbd> (back space) key on the keypad</summary>
	KeypadBackspace = 187,

	/// <summary>The <see cref="Scancode"/> for the <kbd>A</kbd> key on the keypad</summary>
	KeypadA = 188,

	/// <summary>The <see cref="Scancode"/> for the <kbd>B</kbd> key on the keypad</summary>
	KeypadB = 189,

	/// <summary>The <see cref="Scancode"/> for the <kbd>C</kbd> key on the keypad</summary>
	KeypadC = 190,

	/// <summary>The <see cref="Scancode"/> for the <kbd>D</kbd> key on the keypad</summary>
	KeypadD = 191,

	/// <summary>The <see cref="Scancode"/> for the <kbd>E</kbd> key on the keypad</summary>
	KeypadE = 192,

	/// <summary>The <see cref="Scancode"/> for the <kbd>F</kbd> key on the keypad</summary>
	KeypadF = 193,

	/// <summary>The <see cref="Scancode"/> for the <kbd>XOR</kbd> (xor; sometimes <c>^</c>) key on the keypad</summary>
	KeypadXor = 194,

	/// <summary>The <see cref="Scancode"/> for the <kbd>xʸ</kbd> (power / exponentiation; sometimes <c>^</c>) key on the keypad</summary>
	KeypadPower = 195,

	/// <summary>The <see cref="Scancode"/> for the <kbd>%</kbd> key on the keypad</summary>
	KeypadPercent = 196,

	/// <summary>The <see cref="Scancode"/> for the <kbd>&lt;</kbd> key on the keypad</summary>
	KeypadLess = 197,

	/// <summary>The <see cref="Scancode"/> for the <kbd>&gt;</kbd> key on the keypad</summary>
	KeypadGreater = 198,

	/// <summary>The <see cref="Scancode"/> for the <kbd>&amp;</kbd> (and; sometimes <c>AND</c>) key on the keypad</summary>
	KeypadAmpersand = 199,

	/// <summary>The <see cref="Scancode"/> for the <kbd>&amp;&amp;</kbd> key on the keypad</summary>
	KeypadDoubleAmpersand = 200,

	/// <summary>The <see cref="Scancode"/> for the <kbd>|</kbd> (or; sometimes <c>OR</c>) key on the keypad</summary>
	KeypadVerticalBar = 201,

	/// <summary>The <see cref="Scancode"/> for the <kbd>||</kbd> key on the keypad</summary>
	KeypadDoubleVerticalBar = 202,

	/// <summary>The <see cref="Scancode"/> for the <kbd>:</kbd> key on the keypad</summary>
	KeypadColon = 203,

	/// <summary>The <see cref="Scancode"/> for the <kbd>#</kbd> key on the keypad</summary>
	KeypadHash = 204,

	/// <summary>The <see cref="Scancode"/> for the <kbd>⎵</kbd> (space) key on the keypad</summary>
	KeypadSpace = 205,

	/// <summary>The <see cref="Scancode"/> for the <kbd>@</kbd> key on the keypad</summary>
	KeypadAt = 206,

	/// <summary>The <see cref="Scancode"/> for the <kbd>!</kbd> key on the keypad</summary>
	KeypadExclamationMark = 207,

	/// <summary>The <see cref="Scancode"/> for the <kbd>MEM STORE</kbd> (mem store) key on the keypad</summary>
	KeypadMemStore = 208,

	/// <summary>The <see cref="Scancode"/> for the <kbd>MEM RECALL</kbd> (mem recall) key on the keypad</summary>
	KeypadMemRecall = 209,

	/// <summary>The <see cref="Scancode"/> for the <kbd>MEM CLEAR</kbd> (mem clear) key on the keypad</summary>
	KeypadMemClear = 210,

	/// <summary>The <see cref="Scancode"/> for the <kbd>MEM +</kbd> (mem add) key on the keypad</summary>
	KeypadMemAdd = 211,

	/// <summary>The <see cref="Scancode"/> for the <kbd>MEM -</kbd> (mem subtract) key on the keypad</summary>
	KeypadMemSubtract = 212,

	/// <summary>The <see cref="Scancode"/> for the <kbd>MEM *</kbd> (mem multiply) key on the keypad</summary>
	KeypadMemMultiply = 213,

	/// <summary>The <see cref="Scancode"/> for the <kbd>MEM /</kbd> (mem divide) key on the keypad</summary>
	KeypadMemDivide = 214,

	/// <summary>The <see cref="Scancode"/> for the <kbd>±</kbd> (plus-minus) key on the keypad</summary>
	KeypadPlusMinus = 215,

	/// <summary>The <see cref="Scancode"/> for the <kbd>CLEAR</kbd> (clear) key on the keypad</summary>
	KeypadClear = 216,

	// TODO: document the following Scancodes!
#pragma warning disable CS1591

	KeypadClearEntry = 217,

	KeypadBinary = 218,

	KeypadOctal = 219,

	KeypadDecimal = 220,

	KeypadHexadecimal = 221,

#pragma warning restore CS1591

	/// <summary>The <see cref="Scancode"/> for the left <kbd>CTRL</kbd> (left control) key</summary>
	LeftControl = 224,

	/// <summary>The <see cref="Scancode"/> for the left <kbd>⇧</kbd> (left shift) key</summary>
	LeftShift = 225,

	/// <summary>The <see cref="Scancode"/> for the left <kbd>ALT</kbd> (left alt) key</summary>
	LeftAlt = 226,

	/// <summary>The <see cref="Scancode"/> for the left <kbd>⊞</kbd> (left Windows) key on PCs, left <kbd>⌘</kbd> (left command) key on Macs, or sometimes called left meta key</summary>
	LeftGui = 227,

	/// <summary>The <see cref="Scancode"/> for the right <kbd>CTRL</kbd> key (right control) key</summary>
	RightControl = 228,

	/// <summary>The <see cref="Scancode"/> for the right <kbd>⇧</kbd> (right shift) key</summary>
	RightShift = 229,

	/// <summary>The <see cref="Scancode"/> for the right <kbd>ALT</kbd> (right alt) key, or sometimes called <kbd>ALT GR</kbd> (alt gr.) key</summary>
	RightAlt = 230,

	/// <summary>The <see cref="Scancode"/> for the right <kbd>⊞</kbd> (right Windows) key on PCs, right <kbd>⌘</kbd> (right command) key on Macs, or sometimes called right meta key</summary>
	RightGui = 231,

	// TODO: document the following Scancodes!
#pragma warning disable CS1591

	// the original source (https://github.com/libsdl-org/SDL/blob/main/include/SDL3/SDL_scancode.h#L346) is not sure if adding this is necessary
	Mode = 257,

	Sleep = 258,

	Wake = 259,

	ChannelIncrement = 260,

	ChannelDecrement = 261,

#pragma warning restore CS1591

	/// <summary>The <see cref="Scancode"/> for the <kbd>⏵</kbd> (media play) key</summary>
	MediaPlay = 262,

	/// <summary>The <see cref="Scancode"/> for the <kbd>⏸</kbd> (media pause) key</summary>
	MediaPause = 263,

	/// <summary>The <see cref="Scancode"/> for the <kbd>⏺</kbd> (media record) key</summary>
	MediaRecord = 264,

	/// <summary>The <see cref="Scancode"/> for the <kbd>⏩</kbd> (media fast forward) key</summary>
	MediaFastForward = 265,

	/// <summary>The <see cref="Scancode"/> for the <kbd>⏪</kbd> (media rewind) key</summary>
	MediaRewind = 266,

	/// <summary>The <see cref="Scancode"/> for the <kbd>⏭</kbd> (media next track) key</summary>
	MediaNextTrack = 267,

	/// <summary>The <see cref="Scancode"/> for the <kbd>⏮</kbd> (media previous track) key</summary>
	MediaPreviousTrack = 268,

	/// <summary>The <see cref="Scancode"/> for the <kbd>⏹</kbd> (media stop) key</summary>
	MediaStop = 269,

	/// <summary>The <see cref="Scancode"/> for the <kbd>⏏</kbd> (media eject) key</summary>
	MediaEject = 270,

	/// <summary>The <see cref="Scancode"/> for the <kbd>⏯</kbd> (play-pause eject) key</summary>
	MediaPlayPause = 271,

	// TODO: document the following Scancodes!
#pragma warning disable CS1591

	MediaSelect = 272,

#pragma warning restore CS1591

	/// <summary>The <see cref="Scancode"/> for the application control "<em>New</em>" key</summary>
	ApplicationControlNew = 273,

	/// <summary>The <see cref="Scancode"/> for the application control "<em>Open</em>" key</summary>
	ApplicationControlOpen = 274,

	/// <summary>The <see cref="Scancode"/> for the application control "<em>Close</em>" key</summary>
	ApplicationControlClose = 275,

	/// <summary>The <see cref="Scancode"/> for the application control "<em>Exit</em>" key</summary>
	ApplicationControlExit = 276,

	/// <summary>The <see cref="Scancode"/> for the application control "<em>Save</em>" key</summary>
	ApplicationControlSave = 277,

	/// <summary>The <see cref="Scancode"/> for the application control "<em>Print</em>" key</summary>
	ApplicationControlPrint = 278,

	/// <summary>The <see cref="Scancode"/> for the application control "<em>Properties</em>" key</summary>
	ApplicationControlProperties = 279,

	/// <summary>The <see cref="Scancode"/> for the application control "<em>Search</em>" key</summary>
	ApplicationControlSearch = 280,

	/// <summary>The <see cref="Scancode"/> for the application control "<em>Home</em>" key</summary>
	ApplicationControlHome = 281,

	/// <summary>The <see cref="Scancode"/> for the application control "<em>Back</em>" key</summary>
	ApplicationControlBack = 282,

	/// <summary>The <see cref="Scancode"/> for the application control "<em>Forward</em>" key</summary>
	ApplicationControlForward = 283,

	/// <summary>The <see cref="Scancode"/> for the application control "<em>Stop</em>" key</summary>
	ApplicationControlStop = 284,

	/// <summary>The <see cref="Scancode"/> for the application control "<em>Refresh</em>" key</summary>
	ApplicationControlRefresh = 285,

	/// <summary>The <see cref="Scancode"/> for the application control "<em>Bookmarks</em>" key</summary>
	ApplicationControlBookmarks = 286,

	/// <summary>The <see cref="Scancode"/> for left software defined key on mobiles</summary>
	/// <remarks>
	/// <para>
	/// This key is usually situated below the display on phones and used as a multi-function feature key for selecting a software defined function shown on the bottom left of the display.
	/// </para>
	/// </remarks>
	SoftwareLeft = 287,

	/// <summary>The <see cref="Scancode"/> for right software defined key on mobiles</summary>
	/// <remarks>
	/// <para>
	/// This key is usually situated below the display on phones and used as a multi-function feature key for selecting a software defined function shown on the bottom right of the display.
	/// </para>
	/// </remarks>
	SoftwareRight = 288,

	/// <summary>The <see cref="Scancode"/> for the <kbd>📞</kbd> (phone call) key on mobiles</summary>
	Call = 289,

	/// <summary>The <see cref="Scancode"/> for the <kbd>END 📞</kbd> (end phone call) key on mobiles</summary>
	EndCall = 290,
}
