using System.Runtime.CompilerServices;
using System.Text;

namespace Sdl3Sharp.Utilities;

/// <summary>
/// Provide methods to determine if specified character fall within certain character classes
/// </summary>
public static partial class Character
{
	/// <summary>
	/// Determines whether a character is alphabetical (a letter) or numerical (a number digit)
	/// </summary>
	/// <param name="character">The character to check whether it's alphabetical or numerical</param>
	/// <returns><c><see langword="true"/></c>, if the <paramref name="character"/> is an alphabetical or a numerical character; otherwise, <c><see langword="false"/></c></returns>
	/// <remarks>
	/// <para>
	/// <em>WARNING</em>: Regardless of the system locale, this will only treat ASCII values for English <c>0x30</c> (<c>'0'</c>) through <c>0x39</c> (<c>'9'</c>), <c>0x41</c> (<c>'A'</c>) through <c>0x5A</c> (<c>'Z'</c>), and <c>0x61</c> (<c>'a'</c>) through <c>0x7A</c> (<c>'z'</c>) as <c><see langword="true"/></c>.
	/// </para>
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool IsAlphaNumeric(int character) => SDL_isalnum(character) is not 0;

	/// <summary>
	/// Determines whether a character rune is alphabetical (a letter) or numerical (a number digit)
	/// </summary>
	/// <param name="rune">The character rune to check whether it's alphabetical or numerical</param>
	/// <returns><c><see langword="true"/></c>, if the <paramref name="rune"/> is an alphabetical or a numerical character rune; otherwise, <c><see langword="false"/></c></returns>
	/// <remarks>
	/// <para>
	/// <em>WARNING</em>: Regardless of the system locale, this will only treat ASCII values for English <c>0x30</c> (<c>'0'</c>) through <c>0x39</c> (<c>'9'</c>), <c>0x41</c> (<c>'A'</c>) through <c>0x5A</c> (<c>'Z'</c>), and <c>0x61</c> (<c>'a'</c>) through <c>0x7A</c> (<c>'z'</c>) as <c><see langword="true"/></c>.
	/// </para>
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool IsAlphaNumeric(Rune rune) => SDL_isalnum(rune.Value) is not 0;

	/// <summary>
	/// Determines whether a character is alphabetical (a letter)
	/// </summary>
	/// <param name="character">The character to check whether it's alphabetical</param>
	/// <returns><c><see langword="true"/></c>, if the <paramref name="character"/> is an alphabetical character; otherwise, <c><see langword="false"/></c></returns>
	/// <remarks>
	/// <para>
	/// <em>WARNING</em>: Regardless of the system locale, this will only treat ASCII values for English <c>0x41</c> (<c>'A'</c>) through <c>0x5A</c> (<c>'Z'</c>) and <c>0x61</c> (<c>'a'</c>) through <c>0x7A</c> (<c>'z'</c>) as <c><see langword="true"/></c>.
	/// </para>
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool IsAlpha(int character) => SDL_isalpha(character) is not 0;

	/// <summary>
	/// Determines whether a character rune is alphabetical (a letter)
	/// </summary>
	/// <param name="rune">The character rune to check whether it's alphabetical</param>
	/// <returns><c><see langword="true"/></c>, if the <paramref name="rune"/> is an alphabetical character rune; otherwise, <c><see langword="false"/></c></returns>
	/// <remarks>
	/// <para>
	/// <em>WARNING</em>: Regardless of the system locale, this will only treat ASCII values for English <c>0x41</c> (<c>'A'</c>) through <c>0x5A</c> (<c>'Z'</c>) and <c>0x61</c> (<c>'a'</c>) through <c>0x7A</c> (<c>'z'</c>) as <c><see langword="true"/></c>.
	/// </para>
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool IsAlpha(Rune rune) => SDL_isalpha(rune.Value) is not 0;

	/// <summary>
	/// Determines whether a character is blank (a space or tab character)
	/// </summary>
	/// <param name="character">The character to check whether it's blank</param>
	/// <returns><c><see langword="true"/></c>, if the <paramref name="character"/> is a blank character; otherwise, <c><see langword="false"/></c></returns>
	/// <remarks>
	/// <para>
	/// <em>WARNING</em>: Regardless of the system locale, this will only treat ASCII values <c>0x09</c> (<c>'\t'</c>) or <c>0x20</c> (<c>' '</c>) as <c><see langword="true"/></c>.
	/// </para>
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool IsBlank(int character) => SDL_isblank(character) is not 0;

	/// <summary>
	/// Determines whether a character rune is blank (a space or tab character)
	/// </summary>
	/// <param name="rune">The character rune to check whether it's blank</param>
	/// <returns><c><see langword="true"/></c>, if the <paramref name="rune"/> is a blank character rune; otherwise, <c><see langword="false"/></c></returns>
	/// <remarks>
	/// <para>
	/// <em>WARNING</em>: Regardless of the system locale, this will only treat ASCII values <c>0x09</c> (<c>'\t'</c>) or <c>0x20</c> (<c>' '</c>) as <c><see langword="true"/></c>.
	/// </para>
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool IsBlank(Rune rune) => SDL_isblank(rune.Value) is not 0;

	/// <summary>
	/// Determines whether a character is a control character
	/// </summary>
	/// <param name="character">The character to check whether it's a control character</param>
	/// <returns><c><see langword="true"/></c>, if the <paramref name="character"/> is a control character; otherwise, <c><see langword="false"/></c></returns>
	/// <remarks>
	/// <para>
	/// <em>WARNING</em>: Regardless of the system locale, this will only treat ASCII values <c>0x00</c> through <c>0x1F</c>, and <c>0x7F</c>, as <c><see langword="true"/></c>.
	/// </para>
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool IsControl(int character) => SDL_iscntrl(character) is not 0;

	/// <summary>
	/// Determines whether a character rune is a control character rune
	/// </summary>
	/// <param name="rune">The character rune to check whether it's a control character rune</param>
	/// <returns><c><see langword="true"/></c>, if the <paramref name="rune"/> is a control character rune; otherwise, <c><see langword="false"/></c></returns>
	/// <remarks>
	/// <para>
	/// <em>WARNING</em>: Regardless of the system locale, this will only treat ASCII values <c>0x00</c> through <c>0x1F</c>, and <c>0x7F</c>, as <c><see langword="true"/></c>.
	/// </para>
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool IsControl(Rune rune) => SDL_iscntrl(rune.Value) is not 0;

	/// <summary>
	/// Determines whether a character is numerical (a number digit)
	/// </summary>
	/// <param name="character">The character to check whether it's numerical</param>
	/// <returns><c><see langword="true"/></c>, if the <paramref name="character"/> is a numerical character; otherwise, <c><see langword="false"/></c></returns>
	/// <remarks>
	/// <para>
	/// <em>WARNING</em>: Regardless of the system locale, this will only treat ASCII values <c>0x30</c> (<c>'0'</c>) through <c>0x39</c> (<c>'9'</c>) as <c><see langword="true"/></c>.
	/// </para>
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool IsDigit(int character) => SDL_isdigit(character) is not 0;

	/// <summary>
	/// Determines whether a character rune is numerical (a number digit)
	/// </summary>
	/// <param name="rune">The character rune to check whether it's numerical</param>
	/// <returns><c><see langword="true"/></c>, if the <paramref name="rune"/> is a numerical character rune; otherwise, <c><see langword="false"/></c></returns>
	/// <remarks>
	/// <para>
	/// <em>WARNING</em>: Regardless of the system locale, this will only treat ASCII values <c>0x30</c> (<c>'0'</c>) through <c>0x39</c> (<c>'9'</c>) as <c><see langword="true"/></c>.
	/// </para>
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool IsDigit(Rune rune) => SDL_isdigit(rune.Value) is not 0;

	/// <summary>
	/// Determines whether a character is any "printable" character except for a space character
	/// </summary>
	/// <param name="character">The character to check whether it's "printable" except for space</param>
	/// <returns><c><see langword="true"/></c>, if the <paramref name="character"/> is a "printable" character except for a space character; otherwise, <c><see langword="false"/></c></returns>
	/// <remarks>
	/// <para>
	/// Be advised that "printable" has a definition that goes back to text terminals from the dawn of computing,
	/// making this a sort of special case function that is not suitable for Unicode (or most any) text management.
	/// </para>
	/// <para>
	/// <em>WARNING</em>: Regardless of the system locale, this will only treat ASCII values <c>0x21</c> (<c>'!'</c>) through <c>0x7E</c> (<c>'~'</c>) as <c><see langword="true"/></c>.
	/// </para>
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool IsGraphic(int character) => SDL_isgraph(character) is not 0;

	/// <summary>
	/// Determines whether a character rune is any "printable" character rune except for a space character
	/// </summary>
	/// <param name="rune">The character rune to check whether it's "printable" except for space</param>
	/// <returns><c><see langword="true"/></c>, if the <paramref name="rune"/> is a "printable" character rune except for a space character; otherwise, <c><see langword="false"/></c></returns>
	/// <remarks>
	/// <para>
	/// Be advised that "printable" has a definition that goes back to text terminals from the dawn of computing,
	/// making this a sort of special case function that is not suitable for Unicode (or most any) text management.
	/// </para>
	/// <para>
	/// <em>WARNING</em>: Regardless of the system locale, this will only treat ASCII values <c>0x21</c> (<c>'!'</c>) through <c>0x7E</c> (<c>'~'</c>) as <c><see langword="true"/></c>.
	/// </para>
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool IsGraphic(Rune rune) => SDL_isgraph(rune.Value) is not 0;	

	/// <summary>
	/// Determines whether a character is numerical for a hexadecimal number (a hexadecimal number digit)
	/// </summary>
	/// <param name="character">The character to check whether it's numerical for a hexadecimal number</param>
	/// <returns><c><see langword="true"/></c>, if the <paramref name="character"/> is a numerical character for a hexadecimal number; otherwise, <c><see langword="false"/></c></returns>
	/// <remarks>
	/// <para>
	/// <em>WARNING</em>: Regardless of the system locale, this will only treat ASCII values for English <c>0x30</c> (<c>'0'</c>) through <c>0x39</c> (<c>'9'</c>), <c>0x41</c> (<c>'A'</c>) through <c>0x46</c> (<c>'F'</c>), and <c>0x61</c> (<c>'a'</c>) through <c>0x66</c> (<c>'f'</c>) as <c><see langword="true"/></c>.
	/// </para>
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool IsHexDigit(int character) => SDL_isxdigit(character) is not 0;

	/// <summary>
	/// Determines whether a character rune is numerical for a hexadecimal number (a hexadecimal number digit)
	/// </summary>
	/// <param name="rune">The character rune to check whether it's numerical for a hexadecimal number</param>
	/// <returns><c><see langword="true"/></c>, if the <paramref name="rune"/> is a numerical character rune for a hexadecimal number; otherwise, <c><see langword="false"/></c></returns>
	/// <remarks>
	/// <para>
	/// <em>WARNING</em>: Regardless of the system locale, this will only treat ASCII values for English <c>0x30</c> (<c>'0'</c>) through <c>0x39</c> (<c>'9'</c>), <c>0x41</c> (<c>'A'</c>) through <c>0x46</c> (<c>'F'</c>), and <c>0x61</c> (<c>'a'</c>) through <c>0x66</c> (<c>'f'</c>) as <c><see langword="true"/></c>.
	/// </para>
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool IsHexDigit(Rune rune) => SDL_isxdigit(rune.Value) is not 0;

	/// <summary>
	/// Determines whether a character is lower case
	/// </summary>
	/// <param name="character">The character to check whether it's lower case</param>
	/// <returns><c><see langword="true"/></c>, if the <paramref name="character"/> is a lower case character; otherwise, <c><see langword="false"/></c></returns>
	/// <remarks>
	/// <para>
	/// <em>WARNING</em>: Regardless of the system locale, this will only treat ASCII values for English <c>0x61</c> (<c>'a'</c>) through <c>0x7A</c> (<c>'z'</c>) as <c><see langword="true"/></c>.
	/// </para>
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool IsLower(int character) => SDL_islower(character) is not 0;

	/// <summary>
	/// Determines whether a character rune is lower case
	/// </summary>
	/// <param name="rune">The character rune to check whether it's lower case</param>
	/// <returns><c><see langword="true"/></c>, if the <paramref name="rune"/> is a lower case character rune; otherwise, <c><see langword="false"/></c></returns>
	/// <remarks>
	/// <para>
	/// <em>WARNING</em>: Regardless of the system locale, this will only treat ASCII values for English <c>0x61</c> (<c>'a'</c>) through <c>0x7A</c> (<c>'z'</c>) as <c><see langword="true"/></c>.
	/// </para>
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool IsLower(Rune rune) => SDL_islower(rune.Value) is not 0;

	/// <summary>
	/// Determines whether a character is "printable"
	/// </summary>
	/// <param name="character">The character to check whether it's "printable"</param>
	/// <returns><c><see langword="true"/></c>, if the <paramref name="character"/> is a "printable" character; otherwise, <c><see langword="false"/></c></returns>
	/// <remarks>
	/// <para>
	/// Be advised that "printable" has a definition that goes back to text terminals from the dawn of computing,
	/// making this a sort of special case function that is not suitable for Unicode (or most any) text management.
	/// </para>
	/// <para>
	/// <em>WARNING</em>: Regardless of the system locale, this will only treat ASCII values <c>0x20</c> (<c>' '</c>) through <c>0x7E</c> (<c>'~'</c>) as <c><see langword="true"/></c>.
	/// </para>
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool IsPrintable(int character) => SDL_isprint(character) is not 0;

	/// <summary>
	/// Determines whether a character rune is "printable"
	/// </summary>
	/// <param name="rune">The character rune to check whether it's "printable"</param>
	/// <returns><c><see langword="true"/></c>, if the <paramref name="rune"/> is a "printable" character rune; otherwise, <c><see langword="false"/></c></returns>
	/// <remarks>
	/// <para>
	/// Be advised that "printable" has a definition that goes back to text terminals from the dawn of computing,
	/// making this a sort of special case function that is not suitable for Unicode (or most any) text management.
	/// </para>
	/// <para>
	/// <em>WARNING</em>: Regardless of the system locale, this will only treat ASCII values <c>0x20</c> (<c>' '</c>) through <c>0x7E</c> (<c>'~'</c>) as <c><see langword="true"/></c>.
	/// </para>
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool IsPrintable(Rune rune) => SDL_isprint(rune.Value) is not 0;

	/// <summary>
	/// Determines whether a character is a punctuation mark
	/// </summary>
	/// <param name="character">The character to check whether it's a punctuation mark</param>
	/// <returns><c><see langword="true"/></c>, if the <paramref name="character"/> is a punctuation mark character; otherwise, <c><see langword="false"/></c></returns>
	/// <remarks>
	/// <para>
	/// <em>WARNING</em>: Regardless of the system locale, this is equivalent to <c><see cref="IsGraphic(int)"/> &amp;&amp; !<see cref="IsAlphaNumeric(int)"/></c>.
	/// </para>
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool IsPunctuation(int character) => SDL_ispunct(character) is not 0;

	/// <summary>
	/// Determines whether a character rune is a punctuation mark
	/// </summary>
	/// <param name="rune">The character rune to check whether it's a punctuation mark</param>
	/// <returns><c><see langword="true"/></c>, if the <paramref name="rune"/> is a punctuation mark character rune; otherwise, <c><see langword="false"/></c></returns>
	/// <remarks>
	/// <para>
	/// <em>WARNING</em>: Regardless of the system locale, this is equivalent to <c><see cref="IsGraphic(Rune)"/> &amp;&amp; !<see cref="IsAlphaNumeric(Rune)"/></c>.
	/// </para>
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool IsPunctuation(Rune rune) => SDL_ispunct(rune.Value) is not 0;

	/// <summary>
	/// Determines whether a character is a whitespace
	/// </summary>
	/// <param name="character">The character to check whether it's a whitespace</param>
	/// <returns><c><see langword="true"/></c>, if the <paramref name="character"/> is a whitespace character; otherwise, <c><see langword="false"/></c></returns>
	/// <remarks>
	/// <para>
	/// <em>WARNING</em>: Regardless of the system locale, this will only treat the following ASCII values as <c><see langword="true"/></c>:
	/// <list type="bullet">
	///		<item><description><c>0x09</c> (<c>'\t'</c>)</description></item>
	///		<item><description><c>0x0A</c> (<c>'\n'</c>)</description></item>
	///		<item><description><c>0x0B</c> (<c>'\v'</c>)</description></item>
	///		<item><description><c>0x0C</c> (<c>'\f'</c>)</description></item>
	///		<item><description><c>0x0D</c> (<c>'\r'</c>)</description></item>
	///		<item><description><c>0x20</c> (<c>' '</c>)</description></item>
	/// </list>
	/// </para>
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool IsSpace(int character) => SDL_isspace(character) is not 0;

	/// <summary>
	/// Determines whether a character rune is a whitespace
	/// </summary>
	/// <param name="rune">The character rune to check whether it's a whitespace</param>
	/// <returns><c><see langword="true"/></c>, if the <paramref name="rune"/> is a whitespace character rune; otherwise, <c><see langword="false"/></c></returns>
	/// <remarks>
	/// <para>
	/// <em>WARNING</em>: Regardless of the system locale, this will only treat the following ASCII values as <c><see langword="true"/></c>:
	/// <list type="bullet">
	///		<item><description><c>0x09</c> (<c>'\t'</c>)</description></item>
	///		<item><description><c>0x0A</c> (<c>'\n'</c>)</description></item>
	///		<item><description><c>0x0B</c> (<c>'\v'</c>)</description></item>
	///		<item><description><c>0x0C</c> (<c>'\f'</c>)</description></item>
	///		<item><description><c>0x0D</c> (<c>'\r'</c>)</description></item>
	///		<item><description><c>0x20</c> (<c>' '</c>)</description></item>
	/// </list>
	/// </para>
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool IsSpace(Rune rune) => SDL_isspace(rune.Value) is not 0;

	/// <summary>
	/// Determines whether a character is upper case
	/// </summary>
	/// <param name="character">The character to check whether it's upper case</param>
	/// <returns><c><see langword="true"/></c>, if the <paramref name="character"/> is a upper case character; otherwise, <c><see langword="false"/></c></returns>
	/// <remarks>
	/// <para>
	/// <em>WARNING</em>: Regardless of the system locale, this will only treat ASCII values for English <c>0x41</c> (<c>'A'</c>) through <c>0x5A</c> (<c>'Z'</c>) as <c><see langword="true"/></c>.
	/// </para>
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool IsUpper(int character) => SDL_isupper(character) is not 0;

	/// <summary>
	/// Determines whether a character rune is upper case
	/// </summary>
	/// <param name="rune">The character rune to check whether it's upper case</param>
	/// <returns><c><see langword="true"/></c>, if the <paramref name="rune"/> is a upper case character rune; otherwise, <c><see langword="false"/></c></returns>
	/// <remarks>
	/// <para>
	/// <em>WARNING</em>: Regardless of the system locale, this will only treat ASCII values for English <c>0x41</c> (<c>'A'</c>) through <c>0x5A</c> (<c>'Z'</c>) as <c><see langword="true"/></c>.
	/// </para>
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool IsUpper(Rune rune) => SDL_isupper(rune.Value) is not 0;
}
