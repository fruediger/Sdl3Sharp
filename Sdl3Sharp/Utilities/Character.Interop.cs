using Sdl3Sharp.Interop;
using Sdl3Sharp.SourceGeneration;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Utilities;

partial class Character
{
	/// <summary>
	/// Query if a character is alphabetic (a letter) or a number
	/// </summary>
	/// <param name="x">A character value to check</param>
	/// <returns>Returns non-zero if x falls within the character class, zero otherwise</returns>
	/// <remarks>
	/// <para>
	/// <em>WARNING</em>: Regardless of system locale, this will only treat ASCII values for English 'a-z', 'A-Z', and '0-9' as true.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_isalnum">SDL_isalnum</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl), typeof(CallConvSuppressGCTransition)])]
	internal static partial int SDL_isalnum(int x);

	/// <summary>
	/// Query if a character is alphabetic (a letter)
	/// </summary>
	/// <param name="x">A character value to check</param>
	/// <returns>Returns non-zero if x falls within the character class, zero otherwise</returns>
	/// <remarks>
	/// <para>
	/// <em>WARNING</em>: Regardless of system locale, this will only treat ASCII values for English 'a-z' and 'A-Z' as true.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_isalpha">SDL_isalpha</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl), typeof(CallConvSuppressGCTransition)])]
	internal static partial int SDL_isalpha(int x);

	/// <summary>
	/// Report if a character is blank (a space or tab)
	/// </summary>
	/// <param name="x">A character value to check</param>
	/// <returns>Returns non-zero if x falls within the character class, zero otherwise</returns>
	/// <remarks>
	/// <para>
	/// <em>WARNING</em>: Regardless of system locale, this will only treat ASCII values 0x20 (space) or 0x9 (tab) as true.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_isblank">SDL_isblank</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl), typeof(CallConvSuppressGCTransition)])]
	internal static partial int SDL_isblank(int x);

	/// <summary>
	/// Report if a character is a control character
	/// </summary>
	/// <param name="x">A character value to check</param>
	/// <returns>Returns non-zero if x falls within the character class, zero otherwise</returns>
	/// <remarks>
	/// <para>
	/// <em>WARNING</em>: Regardless of system locale, this will only treat ASCII values 0 through 0x1F, and 0x7F, as true.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_iscntrl">SDL_iscntrl</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl), typeof(CallConvSuppressGCTransition)])]
	internal static partial int SDL_iscntrl(int x);

	/// <summary>
	/// Report if a character is a numeric digit
	/// </summary>
	/// <param name="x">A character value to check</param>
	/// <returns>Returns non-zero if x falls within the character class, zero otherwise</returns>
	/// <remarks>
	/// <para>
	/// <em>WARNING</em>: Regardless of system locale, this will only treat ASCII values '0' (0x30) through '9' (0x39), as true.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_isdigit">SDL_isdigit</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl), typeof(CallConvSuppressGCTransition)])]
	internal static partial int SDL_isdigit(int x);

	/// <summary>
	/// Report if a character is any "printable" except space
	/// </summary>
	/// <param name="x">A character value to check</param>
	/// <returns>Returns non-zero if x falls within the character class, zero otherwise</returns>
	/// <remarks>
	/// <para>
	/// Be advised that "printable" has a definition that goes back to text terminals from the dawn of computing,
	/// making this a sort of special case function that is not suitable for Unicode (or most any) text management.
	/// </para>
	/// <para>
	/// <em>WARNING</em>: Regardless of system locale, this is equivalent to <c>(SDL_isprint(x)) &amp;&amp; ((x) != ' ')</c>.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_isgraph">SDL_isgraph</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl), typeof(CallConvSuppressGCTransition)])]
	internal static partial int SDL_isgraph(int x);

	/// <summary>
	/// Report if a character is lower case
	/// </summary>
	/// <param name="x">A character value to check</param>
	/// <returns>Returns non-zero if x falls within the character class, zero otherwise</returns>
	/// <remarks>
	/// <para>
	/// <em>WARNING</em>: Regardless of system locale, this will only treat ASCII values 'a' through 'z' as true.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_islower">SDL_islower</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl), typeof(CallConvSuppressGCTransition)])]
	internal static partial int SDL_islower(int x);

	/// <summary>
	/// Report if a character is "printable"
	/// </summary>
	/// <param name="x">A character value to check</param>
	/// <returns>Returns non-zero if x falls within the character class, zero otherwise</returns>
	/// <remarks>
	/// <para>
	/// Be advised that "printable" has a definition that goes back to text terminals from the dawn of computing,
	/// making this a sort of special case function that is not suitable for Unicode (or most any) text management.
	/// </para>
	/// <para>
	/// <em>WARNING</em>: Regardless of system locale, this will only treat ASCII values ' ' (0x20) through '~' (0x7E) as true.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_isprint">SDL_isprint</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl), typeof(CallConvSuppressGCTransition)])]
	internal static partial int SDL_isprint(int x);

	/// <summary>
	/// Report if a character is a punctuation mark
	/// </summary>
	/// <param name="x">A character value to check</param>
	/// <returns>Returns non-zero if x falls within the character class, zero otherwise</returns>
	/// <remarks>
	/// <para>
	/// <em>WARNING</em>: Regardless of system locale, this is equivalent to <c>((SDL_isgraph(x)) &amp;&amp; (!SDL_isalnum(x)))</c>.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_ispunct">SDL_ispunct</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl), typeof(CallConvSuppressGCTransition)])]
	internal static partial int SDL_ispunct(int x);

	/// <summary>
	/// Report if a character is whitespace
	/// </summary>
	/// <param name="x">A character value to check</param>
	/// <returns>Returns non-zero if x falls within the character class, zero otherwise</returns>
	/// <remarks>
	/// <para>
	/// <em>WARNING</em>: Regardless of system locale, this will only treat the following ASCII values as true:
	/// <list type="bullet">
	///		<item><description>space (0x20)</description></item>
	///		<item><description>tab (0x09)</description></item>
	///		<item><description>newline (0x0A)</description></item>
	///		<item><description>vertical tab (0x0B)</description></item>
	///		<item><description>form feed (0x0C)</description></item>
	///		<item><description>return (0x0D)</description></item>
	/// </list>
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_isspace">SDL_isspace</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl), typeof(CallConvSuppressGCTransition)])]
	internal static partial int SDL_isspace(int x);

	/// <summary>
	/// Report if a character is upper case
	/// </summary>
	/// <param name="x">A character value to check</param>
	/// <returns>Returns non-zero if x falls within the character class, zero otherwise</returns>
	/// <remarks>
	/// <para>
	/// <em>WARNING</em>: Regardless of system locale, this will only treat ASCII values 'A' through 'Z' as true.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_isupper">SDL_isupper</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl), typeof(CallConvSuppressGCTransition)])]
	internal static partial int SDL_isupper(int x);

	/// <summary>
	/// Report if a character is a hexadecimal digit
	/// </summary>
	/// <param name="x">A character value to check</param>
	/// <returns>Returns non-zero if x falls within the character class, zero otherwise.</returns>
	/// <remarks>
	/// <para>
	/// <em>WARNING</em>: Regardless of system locale, this will only treat ASCII values 'A' through 'F', 'a' through 'f', and '0' through '9', as true.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3/SDL_isxdigit">SDL_isxdigit</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl), typeof(CallConvSuppressGCTransition)])]
	internal static partial int SDL_isxdigit(int x);
}
