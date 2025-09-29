using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Input;

partial struct Keycode
{
    /// <summary>Gets a representative for an unknown <see cref="Keycode"/></summary>
    /// <value>A representative for an unknown <see cref="Keycode"/></value>
    public static Keycode Unknown { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Unknown); }

    /// <summary>Gets the <see cref="Keycode"/> for the <c>'\r'</c> (return) key; analogous to the <kbd>⏎</kbd> (return / enter) key</summary>
    /// <value>The <see cref="Keycode"/> for the <c>'\r'</c> (return) key; analogous to the <kbd>⏎</kbd> (return / enter) key</value>
    public static Keycode Return { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Return); }

    /// <summary>Gets the <see cref="Keycode"/> for the <c>'\e'</c> (escape) key; analogous to the <kbd>ESC</kbd> (escape) key</summary>
    /// <value>The <see cref="Keycode"/> for the <c>'\e'</c> (escape) key; analogous to the <kbd>ESC</kbd> (escape) key</value>
    public static Keycode Escape { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Escape); }

    /// <summary>Gets the <see cref="Keycode"/> for the <c>'\b'</c> (back space) key; analogous to the <kbd>⌫</kbd> (back space) key</summary>
    /// <value>The <see cref="Keycode"/> for the <c>'\b'</c> (back space) key; analogous to the <kbd>⌫</kbd> (back space) key</value>
    public static Keycode Backspace { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Backspace); }

    /// <summary>Gets the <see cref="Keycode"/> for the <c>'\t'</c> (tab) key; analogous to the <kbd>⭾</kbd> (tab) key</summary>
    /// <value>The <see cref="Keycode"/> for the <c>'\t'</c> (tab) key; analogous to the <kbd>⭾</kbd> (tab) key</value>
    public static Keycode Tab { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Tab); }

    /// <summary>Gets the <see cref="Keycode"/> for the <c>' '</c> (space) key; analogous to the <kbd>⎵</kbd> (space) key</summary>
    /// <value>The <see cref="Keycode"/> for the <c>' '</c> (space) key; analogous to the <kbd>⎵</kbd> (space) key</value>
    public static Keycode Space { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Space); }

    /// <summary>Gets the <see cref="Keycode"/> for the <c>'!'</c> (exclamation mark) key</summary>
    /// <value>The <see cref="Keycode"/> for the <c>'!'</c> (exclamation mark) key</value>
    public static Keycode ExclamationMark { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.ExclamationMark); }

    /// <summary>Gets the <see cref="Keycode"/> for the <c>'"'</c> (double apostrophe) key</summary>
    /// <value>The <see cref="Keycode"/> for the <c>'"'</c> (double apostrophe) key</value>
    public static Keycode DoubleApostrophe { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.DoubleApostrophe); }

    /// <summary>Gets the <see cref="Keycode"/> for the <c>'#'</c> (hash sign) key</summary>
    /// <value>The <see cref="Keycode"/> for the <c>'#'</c> (hash sign) key</value>
    public static Keycode Hash { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Hash); }

    /// <summary>Gets the <see cref="Keycode"/> for the <c>'$'</c> (dollar sign) key</summary>
    /// <value>The <see cref="Keycode"/> for the <c>'$'</c> (dollar sign) key</value>
    public static Keycode Dollar { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Dollar); }

    /// <summary>Gets the <see cref="Keycode"/> for the <c>'%'</c> (percent sign) key</summary>
    /// <value>The <see cref="Keycode"/> for the <c>'$'</c> (percent sign) key</value>
    public static Keycode Percent { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Percent); }

    /// <summary>Gets the <see cref="Keycode"/> for the <c>'&amp;'</c> (ampersand) key</summary>
    /// <value>The <see cref="Keycode"/> for the <c>'&amp;'</c> (ampersand) key</value>
    public static Keycode Ampersand { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Ampersand); }

    /// <summary>Gets the <see cref="Keycode"/> for the <c>'\''</c> (apostrophe) key</summary>
    /// <value>The <see cref="Keycode"/> for the <c>'\''</c> (apostrophe) key</value>
    public static Keycode Apostrophe { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Apostrophe); }

    /// <summary>Gets the <see cref="Keycode"/> for the <c>'('</c> (left parenthesis) key</summary>
    /// <value>The <see cref="Keycode"/> for the <c>'('</c> (left parenthesis) key</value>
    public static Keycode LeftParenthesis { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.LeftParenthesis); }

    /// <summary>Gets the <see cref="Keycode"/> for the <c>')'</c> (right parenthesis) key</summary>
    /// <value>The <see cref="Keycode"/> for the <c>')'</c> (right parenthesis) key</value>
    public static Keycode RightParenthesis { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.RightParenthesis); }

    /// <summary>Gets the <see cref="Keycode"/> for the <c>'*'</c> (asterisk) key</summary>
    /// <value>The <see cref="Keycode"/> for the <c>'*'</c> (asterisk) key</value>
    public static Keycode Asterisk { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Asterisk); }

    /// <summary>Gets the <see cref="Keycode"/> for the <c>'+'</c> (plus sign) key</summary>
    /// <value>The <see cref="Keycode"/> for the <c>'+'</c> (plus sign) key</value>
    public static Keycode Plus { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Plus); }

    /// <summary>Gets the <see cref="Keycode"/> for the <c>','</c> (comma) key</summary>
    /// <value>The <see cref="Keycode"/> for the <c>','</c> (comma) key</value>
    public static Keycode Comma { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Plus); }

    /// <summary>Gets the <see cref="Keycode"/> for the <c>'-'</c> (minus sign) key</summary>
    /// <value>The <see cref="Keycode"/> for the <c>'-'</c> (minus sign) key</value>
    public static Keycode Minus { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Minus); }

    /// <summary>Gets the <see cref="Keycode"/> for the <c>'.'</c> (period) key</summary>
    /// <value>The <see cref="Keycode"/> for the <c>'.'</c> (period) key</value>
    public static Keycode Period { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Period); }

    /// <summary>Gets the <see cref="Keycode"/> for the <c>'/'</c> (slash) key</summary>
    /// <value>The <see cref="Keycode"/> for the <c>'/'</c> (slash) key</value>
    public static Keycode Slash { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Slash); }

    /// <summary>Gets the <see cref="Keycode"/> for the <c>'0'</c> key; analogous to the <kbd>0</kbd> key</summary>
    /// <value>The <see cref="Keycode"/> for the <c>'0'</c> key; analogous to the <kbd>0</kbd> key</value>
    public static Keycode _0 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind._0); }

    /// <summary>Gets the <see cref="Keycode"/> for the <c>'1'</c> key; analogous to the <kbd>1</kbd> key</summary>
    /// <value>The <see cref="Keycode"/> for the <c>'1'</c> key; analogous to the <kbd>1</kbd> key</value>
    public static Keycode _1 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind._1); }

    /// <summary>Gets the <see cref="Keycode"/> for the <c>'2'</c> key; analogous to the <kbd>2</kbd> key</summary>
    /// <value>The <see cref="Keycode"/> for the <c>'2'</c> key; analogous to the <kbd>2</kbd> key</value>
    public static Keycode _2 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind._2); }

    /// <summary>Gets the <see cref="Keycode"/> for the <c>'3'</c> key; analogous to the <kbd>3</kbd> key</summary>
    /// <value>The <see cref="Keycode"/> for the <c>'3'</c> key; analogous to the <kbd>3</kbd> key</value>
    public static Keycode _3 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind._3); }

    /// <summary>Gets the <see cref="Keycode"/> for the <c>'4'</c> key; analogous to the <kbd>4</kbd> key</summary>
    /// <value>The <see cref="Keycode"/> for the <c>'4'</c> key; analogous to the <kbd>4</kbd> key</value>
    public static Keycode _4 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind._4); }

    /// <summary>Gets the <see cref="Keycode"/> for the <c>'5'</c> key; analogous to the <kbd>5</kbd> key</summary>
    /// <value>The <see cref="Keycode"/> for the <c>'5'</c> key; analogous to the <kbd>5</kbd> key</value>
    public static Keycode _5 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind._5); }

    /// <summary>Gets the <see cref="Keycode"/> for the <c>'6'</c> key; analogous to the <kbd>6</kbd> key</summary>
    /// <value>The <see cref="Keycode"/> for the <c>'6'</c> key; analogous to the <kbd>6</kbd> key</value>
    public static Keycode _6 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind._6); }

    /// <summary>Gets the <see cref="Keycode"/> for the <c>'7'</c> key; analogous to the <kbd>7</kbd> key</summary>
    /// <value>The <see cref="Keycode"/> for the <c>'7'</c> key; analogous to the <kbd>7</kbd> key</value>
    public static Keycode _7 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind._7); }

    /// <summary>Gets the <see cref="Keycode"/> for the <c>'8'</c> key; analogous to the <kbd>8</kbd> key</summary>
    /// <value>The <see cref="Keycode"/> for the <c>'8'</c> key; analogous to the <kbd>8</kbd> key</value>
    public static Keycode _8 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind._8); }

    /// <summary>Gets the <see cref="Keycode"/> for the <c>'9'</c> key; analogous to the <kbd>9</kbd> key</summary>
    /// <value>The <see cref="Keycode"/> for the <c>'9'</c> key; analogous to the <kbd>9</kbd> key</value>
    public static Keycode _9 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind._9); }

    /// <summary>Gets the <see cref="Keycode"/> for the <c>':'</c> (colon) key</summary>
    /// <value>The <see cref="Keycode"/> for the <c>':'</c> (colon) key</value>
    public static Keycode Colon { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Colon); }

    /// <summary>Gets the <see cref="Keycode"/> for the <c>';'</c> (semicolon) key</summary>
    /// <value>The <see cref="Keycode"/> for the <c>';'</c> (semicolon) key</value>
    public static Keycode Semicolon { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Semicolon); }

    /// <summary>Gets the <see cref="Keycode"/> for the <c>'&lt;'</c> (less than sign) key</summary>
    /// <value>The <see cref="Keycode"/> for the <c>'&lt;'</c> (less than sign) key</value>
    public static Keycode Less { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Less); }

    /// <summary>Gets the <see cref="Keycode"/> for the <c>'='</c> (equal sign) key</summary>
    /// <value>The <see cref="Keycode"/> for the <c>'='</c> (equal sign) key</value>
    public static Keycode Equal { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Equal); }

    /// <summary>Gets the <see cref="Keycode"/> for the <c>'&gt;'</c> (greater than sign) key</summary>
    /// <value>The <see cref="Keycode"/> for the <c>'&gt;'</c> (greater than sign) key</value>
    public static Keycode Greater { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Greater); }

    /// <summary>Gets the <see cref="Keycode"/> for the <c>'?'</c> (question mark) key</summary>
    /// <value>The <see cref="Keycode"/> for the <c>'?'</c> (question mark) key</value>
    public static Keycode QuestionMark { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.QuestionMark); }

    /// <summary>Gets the <see cref="Keycode"/> for the <c>'@'</c> (at sign) key</summary>
    /// <value>The <see cref="Keycode"/> for the <c>'@'</c> (at sign) key</value>
    public static Keycode At { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.At); }

    /// <summary>Gets the <see cref="Keycode"/> for the <c>'['</c> (left bracket) key</summary>
    /// <value>The <see cref="Keycode"/> for the <c>'['</c> (left bracket) key</value>
    public static Keycode LeftBracket { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.LeftBracket); }

    /// <summary>Gets the <see cref="Keycode"/> for the <c>'\\'</c> (back slash) key</summary>
    /// <value>The <see cref="Keycode"/> for the <c>'\\'</c> (back slash) key</value>
    public static Keycode Backslash { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Backslash); }

    /// <summary>Gets the <see cref="Keycode"/> for the <c>']'</c> (right bracket) key</summary>
    /// <value>The <see cref="Keycode"/> for the <c>']'</c> (right bracket) key</value>
    public static Keycode RightBracket { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.RightBracket); }

    /// <summary>Gets the <see cref="Keycode"/> for the <c>'^'</c> (caret / accent circumflex) key</summary>
    /// <value>The <see cref="Keycode"/> for the <c>'^'</c> (caret / accent circumflex) key</value>
    public static Keycode Caret { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Caret); }

    /// <summary>Gets the <see cref="Keycode"/> for the <c>'_'</c> (underscore) key</summary>
    /// <value>The <see cref="Keycode"/> for the <c>'_'</c> (underscore) key</value>
    public static Keycode Underscore { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Underscore); }

    /// <summary>Gets the <see cref="Keycode"/> for the <c>'`'</c> (accent grave) key</summary>
    /// <value>The <see cref="Keycode"/> for the <c>'`'</c> (accent grave) key</value>
    public static Keycode Grave { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Grave); }

    /// <summary>Gets the <see cref="Keycode"/> for the <c>'a'</c> key; analogous to the <kbd>A</kbd> key</summary>
    /// <value>The <see cref="Keycode"/> for the <c>'a'</c> key; analogous to the <kbd>A</kbd> key</value>
    public static Keycode A { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.A); }

    /// <summary>Gets the <see cref="Keycode"/> for the <c>'b'</c> key; analogous to the <kbd>B</kbd> key</summary>
    /// <value>The <see cref="Keycode"/> for the <c>'b'</c> key; analogous to the <kbd>B</kbd> key</value>
    public static Keycode B { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.B); }

    /// <summary>Gets the <see cref="Keycode"/> for the <c>'c'</c> key; analogous to the <kbd>C</kbd> key</summary>
    /// <value>The <see cref="Keycode"/> for the <c>'c'</c> key; analogous to the <kbd>C</kbd> key</value>
    public static Keycode C { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.C); }

    /// <summary>Gets the <see cref="Keycode"/> for the <c>'d'</c> key; analogous to the <kbd>D</kbd> key</summary>
    /// <value>The <see cref="Keycode"/> for the <c>'d'</c> key; analogous to the <kbd>D</kbd> key</value>
    public static Keycode D { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.D); }

    /// <summary>Gets the <see cref="Keycode"/> for the <c>'e'</c> key; analogous to the <kbd>E</kbd> key</summary>
    /// <value>The <see cref="Keycode"/> for the <c>'e'</c> key; analogous to the <kbd>E</kbd> key</value>
    public static Keycode E { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.E); }

    /// <summary>Gets the <see cref="Keycode"/> for the <c>'f'</c> key; analogous to the <kbd>F</kbd> key</summary>
    /// <value>The <see cref="Keycode"/> for the <c>'f'</c> key; analogous to the <kbd>F</kbd> key</value>
    public static Keycode F { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.F); }

    /// <summary>Gets the <see cref="Keycode"/> for the <c>'g'</c> key; analogous to the <kbd>G</kbd> key</summary>
    /// <value>The <see cref="Keycode"/> for the <c>'g'</c> key; analogous to the <kbd>G</kbd> key</value>
    public static Keycode G { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.G); }

    /// <summary>Gets the <see cref="Keycode"/> for the <c>'h'</c> key; analogous to the <kbd>H</kbd> key</summary>
    /// <value>The <see cref="Keycode"/> for the <c>'h'</c> key; analogous to the <kbd>H</kbd> key</value>
    public static Keycode H { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.H); }

    /// <summary>Gets the <see cref="Keycode"/> for the <c>'i'</c> key; analogous to the <kbd>I</kbd> key</summary>
    /// <value>The <see cref="Keycode"/> for the <c>'i'</c> key; analogous to the <kbd>I</kbd> key</value>
    public static Keycode I { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.I); }

    /// <summary>Gets the <see cref="Keycode"/> for the <c>'j'</c> key; analogous to the <kbd>J</kbd> key</summary>
    /// <value>The <see cref="Keycode"/> for the <c>'j'</c> key; analogous to the <kbd>J</kbd> key</value>
    public static Keycode J { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.J); }

    /// <summary>Gets the <see cref="Keycode"/> for the <c>'k'</c> key; analogous to the <kbd>K</kbd> key</summary>
    /// <value>The <see cref="Keycode"/> for the <c>'k'</c> key; analogous to the <kbd>K</kbd> key</value>
    public static Keycode K { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.K); }

    /// <summary>Gets the <see cref="Keycode"/> for the <c>'l'</c> key; analogous to the <kbd>L</kbd> key</summary>
    /// <value>The <see cref="Keycode"/> for the <c>'l'</c> key; analogous to the <kbd>L</kbd> key</value>
    public static Keycode L { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.L); }

    /// <summary>Gets the <see cref="Keycode"/> for the <c>'m'</c> key; analogous to the <kbd>M</kbd> key</summary>
    /// <value>The <see cref="Keycode"/> for the <c>'m'</c> key; analogous to the <kbd>M</kbd> key</value>
    public static Keycode M { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.M); }

    /// <summary>Gets the <see cref="Keycode"/> for the <c>'n'</c> key; analogous to the <kbd>N</kbd> key</summary>
    /// <value>The <see cref="Keycode"/> for the <c>'n'</c> key; analogous to the <kbd>N</kbd> key</value>
    public static Keycode N { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.N); }

    /// <summary>Gets the <see cref="Keycode"/> for the <c>'o'</c> key; analogous to the <kbd>O</kbd> key</summary>
    /// <value>The <see cref="Keycode"/> for the <c>'o'</c> key; analogous to the <kbd>O</kbd> key</value>
    public static Keycode O { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.O); }

    /// <summary>Gets the <see cref="Keycode"/> for the <c>'p'</c> key; analogous to the <kbd>P</kbd> key</summary>
    /// <value>The <see cref="Keycode"/> for the <c>'p'</c> key; analogous to the <kbd>P</kbd> key</value>
    public static Keycode P { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.P); }

    /// <summary>Gets the <see cref="Keycode"/> for the <c>'q'</c> key; analogous to the <kbd>Q</kbd> key</summary>
    /// <value>The <see cref="Keycode"/> for the <c>'q'</c> key; analogous to the <kbd>Q</kbd> key</value>
    public static Keycode Q { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Q); }

    /// <summary>Gets the <see cref="Keycode"/> for the <c>'r'</c> key; analogous to the <kbd>R</kbd> key</summary>
    /// <value>The <see cref="Keycode"/> for the <c>'r'</c> key; analogous to the <kbd>R</kbd> key</value>
    public static Keycode R { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.R); }

    /// <summary>Gets the <see cref="Keycode"/> for the <c>'s'</c> key; analogous to the <kbd>S</kbd> key</summary>
    /// <value>The <see cref="Keycode"/> for the <c>'s'</c> key; analogous to the <kbd>S</kbd> key</value>
    public static Keycode S { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.S); }

    /// <summary>Gets the <see cref="Keycode"/> for the <c>'t'</c> key; analogous to the <kbd>T</kbd> key</summary>
    /// <value>The <see cref="Keycode"/> for the <c>'t'</c> key; analogous to the <kbd>T</kbd> key</value>
    public static Keycode T { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.T); }

    /// <summary>Gets the <see cref="Keycode"/> for the <c>'u'</c> key; analogous to the <kbd>U</kbd> key</summary>
    /// <value>The <see cref="Keycode"/> for the <c>'u'</c> key; analogous to the <kbd>U</kbd> key</value>
    public static Keycode U { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.U); }

    /// <summary>Gets the <see cref="Keycode"/> for the <c>'v'</c> key; analogous to the <kbd>V</kbd> key</summary>
    /// <value>The <see cref="Keycode"/> for the <c>'v'</c> key; analogous to the <kbd>V</kbd> key</value>
    public static Keycode V { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.V); }

    /// <summary>Gets the <see cref="Keycode"/> for the <c>'w'</c> key; analogous to the <kbd>W</kbd> key</summary>
    /// <value>The <see cref="Keycode"/> for the <c>'w'</c> key; analogous to the <kbd>W</kbd> key</value>
    public static Keycode W { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.W); }

    /// <summary>Gets the <see cref="Keycode"/> for the <c>'x'</c> key; analogous to the <kbd>X</kbd> key</summary>
    /// <value>The <see cref="Keycode"/> for the <c>'x'</c> key; analogous to the <kbd>X</kbd> key</value>
    public static Keycode X { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.X); }

    /// <summary>Gets the <see cref="Keycode"/> for the <c>'y'</c> key; analogous to the <kbd>Y</kbd> key</summary>
    /// <value>The <see cref="Keycode"/> for the <c>'y'</c> key; analogous to the <kbd>Y</kbd> key</value>
    public static Keycode Y { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Y); }

    /// <summary>Gets the <see cref="Keycode"/> for the <c>'z'</c> key; analogous to the <kbd>Z</kbd> key</summary>
    /// <value>The <see cref="Keycode"/> for the <c>'z'</c> key; analogous to the <kbd>Z</kbd> key</value>
    public static Keycode Z { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Z); }

    /// <summary>Gets the <see cref="Keycode"/> for the <c>'{'</c> (left brace) key</summary>
    /// <value>The <see cref="Keycode"/> for the <c>'{'</c> (left brace) key</value>
    public static Keycode LeftBrace { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.LeftBrace); }

    /// <summary>Gets the <see cref="Keycode"/> for the <c>'|'</c> (pipe / vertical bar) key</summary>
    /// <value>The <see cref="Keycode"/> for the <c>'|'</c> (pipe / vertical bar) key</value>
    public static Keycode Pipe { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Pipe); }

    /// <summary>Gets the <see cref="Keycode"/> for the <c>'}'</c> (right brace) key</summary>
    /// <value>The <see cref="Keycode"/> for the <c>'}'</c> (right brace) key</value>
    public static Keycode RightBrace { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.RightBrace); }

    /// <summary>Gets the <see cref="Keycode"/> for the <c>'~'</c> (tilde) key</summary>
    /// <value>The <see cref="Keycode"/> for the <c>'~'</c> (tilde) key</value>
    public static Keycode Tilde { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Tilde); }

    /// <summary>Gets the <see cref="Keycode"/> for the <c>'\x7F'</c> (delete) key</summary>
    /// <value>The <see cref="Keycode"/> for the <c>'\x7F'</c> (delete) key</value>
    public static Keycode Delete { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Delete); }

    /// <summary>Gets the <see cref="Keycode"/> for the <c>'±'</c> (plus-minus sign) key</summary>
    /// <value>The <see cref="Keycode"/> for the <c>'±'</c> (plus-minus sign) key</value>
    public static Keycode PlusMinus { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.PlusMinus); }    

    /// <summary>Gets the <see cref="Keycode"/> for the <kbd>⇪</kbd> (caps lock) key</summary>
    /// <value>The <see cref="Keycode"/> for the <kbd>⇪</kbd> (caps lock) key</value>
    public static Keycode CapsLock { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.CapsLock); }

    /// <summary>Gets the <see cref="Keycode"/> for the <kbd>F1</kbd> key</summary>
    /// <value>The <see cref="Keycode"/> for the <kbd>F1</kbd> key</value>
    public static Keycode F1 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.F1); }

    /// <summary>Gets the <see cref="Keycode"/> for the <kbd>F2</kbd> key</summary>
    /// <value>The <see cref="Keycode"/> for the <kbd>F2</kbd> key</value>
    public static Keycode F2 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.F2); }

    /// <summary>Gets the <see cref="Keycode"/> for the <kbd>F3</kbd> key</summary>
    /// <value>The <see cref="Keycode"/> for the <kbd>F3</kbd> key</value>
    public static Keycode F3 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.F3); }

    /// <summary>Gets the <see cref="Keycode"/> for the <kbd>F4</kbd> key</summary>
    /// <value>The <see cref="Keycode"/> for the <kbd>F4</kbd> key</value>
    public static Keycode F4 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.F4); }

    /// <summary>Gets the <see cref="Keycode"/> for the <kbd>F5</kbd> key</summary>
    /// <value>The <see cref="Keycode"/> for the <kbd>F5</kbd> key</value>
    public static Keycode F5 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.F5); }

    /// <summary>Gets the <see cref="Keycode"/> for the <kbd>F6</kbd> key</summary>
    /// <value>The <see cref="Keycode"/> for the <kbd>F6</kbd> key</value>
    public static Keycode F6 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.F6); }

    /// <summary>Gets the <see cref="Keycode"/> for the <kbd>F7</kbd> key</summary>
    /// <value>The <see cref="Keycode"/> for the <kbd>F7</kbd> key</value>
    public static Keycode F7 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.F7); }

    /// <summary>Gets the <see cref="Keycode"/> for the <kbd>F8</kbd> key</summary>
    /// <value>The <see cref="Keycode"/> for the <kbd>F8</kbd> key</value>
    public static Keycode F8 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.F8); }

    /// <summary>Gets the <see cref="Keycode"/> for the <kbd>F9</kbd> key</summary>
    /// <value>The <see cref="Keycode"/> for the <kbd>F9</kbd> key</value>
    public static Keycode F9 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.F9); }

    /// <summary>Gets the <see cref="Keycode"/> for the <kbd>F10</kbd> key</summary>
    /// <value>The <see cref="Keycode"/> for the <kbd>F10</kbd> key</value>
    public static Keycode F10 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.F10); }

    /// <summary>Gets the <see cref="Keycode"/> for the <kbd>F11</kbd> key</summary>
    /// <value>The <see cref="Keycode"/> for the <kbd>F11</kbd> key</value>
    public static Keycode F11 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.F11); }

    /// <summary>Gets the <see cref="Keycode"/> for the <kbd>F12</kbd> key</summary>
    /// <value>The <see cref="Keycode"/> for the <kbd>F12</kbd> key</value>
    public static Keycode F12 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.F12); }

    /// <summary>Gets the <see cref="Keycode"/> for the <kbd>PRINT SCREEN</kbd> (print screen) key</summary>
    /// <value>The <see cref="Keycode"/> for the <kbd>PRINT SCREEN</kbd> (print screen) key</value>
    public static Keycode PrintScreen { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.PrintScreen); }

    /// <summary>Gets the <see cref="Keycode"/> for the <kbd>SCROLL</kbd> (scroll lock) key</summary>
    /// <value>The <see cref="Keycode"/> for the <kbd>SCROLL</kbd> (scroll lock) key</value>
    public static Keycode ScrollLock { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.ScrollLock); }

    /// <summary>Gets the <see cref="Keycode"/> for the <kbd>PAUSE</kbd> (pause / break) key</summary>
    /// <value>The <see cref="Keycode"/> for the <kbd>PAUSE</kbd> (pause / break) key</value>
    public static Keycode Pause { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Pause); }

    /// <summary>Gets the <see cref="Keycode"/> for the <kbd>INSERT</kbd> (insert) key</summary>
    /// <value>The <see cref="Keycode"/> for the <kbd>INSERT</kbd> (insert) key</value>
    /// <remarks>
    /// <para>
    /// On some Mac keyboards this is actual send as the <see cref="Keycode"/> for the <kbd>HELP</kbd> (help) key instead of <see cref="Help"/>.
    /// </para>
    /// </remarks>
    public static Keycode Insert { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Insert); }

    /// <summary>Gets the <see cref="Keycode"/> for the <kbd>HOME</kbd> (home) key</summary>
    /// <value>The <see cref="Keycode"/> for the <kbd>HOME</kbd> (home) key</value>
    public static Keycode Home { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Home); }

    /// <summary>Gets the <see cref="Keycode"/> for the <kbd>PAGE UP</kbd> (page up) key</summary>
    /// <value>The <see cref="Keycode"/> for the <kbd>PAGE UP</kbd> (page up) key</value>
    public static Keycode PageUp { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.PageUp); }

    /// <summary>Gets the <see cref="Keycode"/> for the <kbd>END</kbd> (end) key</summary>
    /// <value>The <see cref="Keycode"/> for the <kbd>END</kbd> (end) key</value>
    public static Keycode End { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.End); }

    /// <summary>Gets the <see cref="Keycode"/> for the <kbd>PAGE DOWN</kbd> (page down) key</summary>
    /// <value>The <see cref="Keycode"/> for the <kbd>PAGE DOWN</kbd> (page down) key</value>
    public static Keycode PageDown { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.PageDown); }

    /// <summary>Gets the <see cref="Keycode"/> for the <kbd>→</kbd> (right arrow) key</summary>
    /// <value>The <see cref="Keycode"/> for the <kbd>→</kbd> (right arrow) key</value>
    public static Keycode Right { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Right); }

    /// <summary>Gets the <see cref="Keycode"/> for the <kbd>←</kbd> (left arrow) key</summary>
    /// <value>The <see cref="Keycode"/> for the <kbd>←</kbd> (left arrow) key</value>
    public static Keycode Left { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Left); }

    /// <summary>Gets the <see cref="Keycode"/> for the <kbd>↓</kbd> (down arrow) key</summary>
    /// <value>The <see cref="Keycode"/> for the <kbd>↓</kbd> (down arrow) key</value>
    public static Keycode Down { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Down); }

    /// <summary>Gets the <see cref="Keycode"/> for the <kbd>↑</kbd> (up arrow) key</summary>
    /// <value>The <see cref="Keycode"/> for the <kbd>↑</kbd> (up arrow) key</value>
    public static Keycode Up { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Up); }

    /// <summary>Gets the <see cref="Keycode"/> for the <kbd>NUM</kbd> (num lock) key on PCs, or the <kbd>CLEAR</kbd> (clear) key on Macs</summary>
    /// <value>The <see cref="Keycode"/> for the <kbd>NUM</kbd> (num lock) key on PCs, or the <kbd>CLEAR</kbd> (clear) key on Macs</value>
    public static Keycode NumLockOrClear { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.NumLockOrClear); }

    /// <summary>Gets the <see cref="Keycode"/> for the <kbd>☰</kbd> (context menu) key on Windows, or the <kbd>COMPOSE</kbd> (compose) key elsewhere</summary>
    /// <value>The <see cref="Keycode"/> for the <kbd>☰</kbd> (context menu) key on Windows, or the <kbd>COMPOSE</kbd> (compose) key elsewhere</value>
    public static Keycode Application { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Application); }

    /// <summary>Gets zhe <see cref="Keycode"/> that <em>might be</em> for the <kbd>POWER</kbd> (power) key on some Mac keyboards</summary>
    /// <value>The <see cref="Keycode"/> that <em>might be</em> for the <kbd>POWER</kbd> (power) key on some Mac keyboards</value>
    /// <remarks>
    /// <para>
    /// Do <em>not</em> rely on this, as the USB document says that this is a status flag instead of a physical key
    /// </para>
    /// </remarks>
    [Experimental(diagnosticId: "SDL5010")] //TODO: make SDL5010 the diagnostics id for unknown or uncertain Keycodes or Scancodes
    public static Keycode Power { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Power); }

    /// <summary>Gets the <see cref="Keycode"/> for the <kbd>F13</kbd> key</summary>
    /// <value>The <see cref="Keycode"/> for the <kbd>F13</kbd> key</value>
    public static Keycode F13 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.F13); }

    /// <summary>Gets the <see cref="Keycode"/> for the <kbd>F14</kbd> key</summary>
    /// <value>The <see cref="Keycode"/> for the <kbd>F14</kbd> key</value>
    public static Keycode F14 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.F14); }

    /// <summary>Gets the <see cref="Keycode"/> for the <kbd>F15</kbd> key</summary>
    /// <value>The <see cref="Keycode"/> for the <kbd>F15</kbd> key</value>
    public static Keycode F15 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.F15); }

    /// <summary>Gets the <see cref="Keycode"/> for the <kbd>F16</kbd> key</summary>
    /// <value>The <see cref="Keycode"/> for the <kbd>F16</kbd> key</value>
    public static Keycode F16 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.F16); }

    /// <summary>Gets the <see cref="Keycode"/> for the <kbd>F17</kbd> key</summary>
    /// <value>The <see cref="Keycode"/> for the <kbd>F17</kbd> key</value>
    public static Keycode F17 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.F17); }

    /// <summary>Gets the <see cref="Keycode"/> for the <kbd>F18</kbd> key</summary>
    /// <value>The <see cref="Keycode"/> for the <kbd>F18</kbd> key</value>
    public static Keycode F18 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.F18); }

    /// <summary>Gets the <see cref="Keycode"/> for the <kbd>F19</kbd> key</summary>
    /// <value>The <see cref="Keycode"/> for the <kbd>F19</kbd> key</value>
    public static Keycode F19 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.F19); }

    /// <summary>Gets the <see cref="Keycode"/> for the <kbd>F20</kbd> key</summary>
    /// <value>The <see cref="Keycode"/> for the <kbd>F20</kbd> key</value>
    public static Keycode F20 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.F20); }

    /// <summary>Gets the <see cref="Keycode"/> for the <kbd>F21</kbd> key</summary>
    /// <value>The <see cref="Keycode"/> for the <kbd>F21</kbd> key</value>
    public static Keycode F21 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.F21); }

    /// <summary>Gets the <see cref="Keycode"/> for the <kbd>F22</kbd> key</summary>
    /// <value>The <see cref="Keycode"/> for the <kbd>F22</kbd> key</value>
    public static Keycode F22 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.F22); }

    /// <summary>Gets the <see cref="Keycode"/> for the <kbd>F23</kbd> key</summary>
    /// <value>The <see cref="Keycode"/> for the <kbd>F23</kbd> key</value>
    public static Keycode F23 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.F23); }

    /// <summary>Gets the <see cref="Keycode"/> for the <kbd>F24</kbd> key</summary>
    /// <value>The <see cref="Keycode"/> for the <kbd>F24</kbd> key</value>
    public static Keycode F24 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.F24); }

    /// <summary>Gets the <see cref="Keycode"/> for the <kbd>EXECUTE</kbd> (execute) key</summary>
    /// <value>The <see cref="Keycode"/> for the <kbd>EXECUTE</kbd> (execute) key</value>
    public static Keycode Execute { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Execute); }

    /// <summary>Gets the <see cref="Keycode"/> for the <kbd>HELP</kbd> (help) key</summary>
    /// <value>The <see cref="Keycode"/> for the <kbd>HELP</kbd> (help) key</value>
    public static Keycode Help { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Help); }

    /// <summary>Gets the <see cref="Keycode"/> for the <kbd>☰</kbd> (menu / show menu) key</summary>
    /// <value>The <see cref="Keycode"/> for the <kbd>☰</kbd> (menu / show menu) key</value>
    public static Keycode Menu { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Menu); }

    /// <summary>Gets the <see cref="Keycode"/> for the <kbd>SELECT</kbd> (select) key</summary>
    /// <value>The <see cref="Keycode"/> for the <kbd>SELECT</kbd> (select) key</value>
    public static Keycode Select { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Select); }

    /// <summary>Gets the <see cref="Keycode"/> for the <kbd>STOP</kbd> (stop) key</summary>
    /// <value>The <see cref="Keycode"/> for the <kbd>STOP</kbd> (stop) key</value>
    public static Keycode Stop { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Stop); }

    /// <summary>Gets the <see cref="Keycode"/> for the <kbd>AGAIN</kbd> (again / redo) key</summary>
    /// <value>The <see cref="Keycode"/> for the <kbd>AGAIN</kbd> (again / redo) key</value>
    public static Keycode Again { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Again); }

    /// <summary>Gets the <see cref="Keycode"/> for the <kbd>UNDO</kbd> (undo) key</summary>
    /// <value>The <see cref="Keycode"/> for the <kbd>UNDO</kbd> (undo) key</value>
    public static Keycode Undo { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Undo); }

    /// <summary>Gets the <see cref="Keycode"/> for the <kbd>CUT</kbd> (cut) key</summary>
    /// <value>The <see cref="Keycode"/> for the <kbd>CUT</kbd> (cut) key</value>
    public static Keycode Cut { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Cut); }

    /// <summary>Gets the <see cref="Keycode"/> for the <kbd>COPY</kbd> (copy) key</summary>
    /// <value>The <see cref="Keycode"/> for the <kbd>COPY</kbd> (copy) key</value>
    public static Keycode Copy { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Copy); }

    /// <summary>Gets the <see cref="Keycode"/> for the <kbd>PASTE</kbd> (paste) key</summary>
    /// <value>The <see cref="Keycode"/> for the <kbd>PASTE</kbd> (paste) key</value>
    public static Keycode Paste { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Paste); }

    /// <summary>Gets the <see cref="Keycode"/> for the <kbd>🔍</kbd> (find) key</summary>
    /// <value>The <see cref="Keycode"/> for the <kbd>🔍</kbd> (find) key</value>
    public static Keycode Find { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Find); }

    /// <summary>Gets the <see cref="Keycode"/> for the <kbd>🔇</kbd> (mute volume) key</summary>
    /// <value>The <see cref="Keycode"/> for the <kbd>🔇</kbd> (mute volume) key</value>
    public static Keycode Mute { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Mute); }

    /// <summary>Gets the <see cref="Keycode"/> for the <kbd>🔊</kbd> (volume up) key</summary>
    /// <value>The <see cref="Keycode"/> for the <kbd>🔊</kbd> (volume up) key</value>
    public static Keycode VolumeUp { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.VolumeUp); }

    /// <summary>Gets the <see cref="Keycode"/> for the <kbd>🔉</kbd> (volume down) key</summary>
    /// <value>The <see cref="Keycode"/> for the <kbd>🔉</kbd> (volume down) key</value>
    public static Keycode VolumeDown { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.VolumeDown); } 

    /* The original source (https://github.com/libsdl-org/SDL/blob/main/include/SDL3/SDL_Keycode.h#L250) is not sure whether to enable these or not
     *  LockingCapsLock
     *  LockingCapsLock
     *  LockingScrollLock
     */

// TODO: document the following Keycodes!
#pragma warning disable CS1591

    public static Keycode AltErase { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.AltErase); }

    public static Keycode SysReq { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.SysReq); }

    public static Keycode Cancel { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Cancel); }

    public static Keycode Clear { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Clear); }

    public static Keycode Prior { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Prior); }

    public static Keycode Return2 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Return2); }

    public static Keycode Separator { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Separator); }

    public static Keycode Out { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Out); }

    public static Keycode Oper { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Oper); }

    public static Keycode ClearAgain { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.ClearAgain); }

    public static Keycode CrSel { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.CrSel); }

    public static Keycode ExSel { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.ExSel); }

    public static Keycode ThousandsSeparator { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.ThousandsSeparator); }

    public static Keycode DecimalSeparator { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.DecimalSeparator); }

    public static Keycode CurrencyUnit { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.CurrencyUnit); }

    public static Keycode CurrencySubunit { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.CurrencySubunit); }

#pragma warning restore CS1591
    
    /// <summary>Gets the <see cref="Keycode"/> for the left <kbd>CTRL</kbd> (left control) key</summary>
    /// <value>The <see cref="Keycode"/> for the left <kbd>CTRL</kbd> (left control) key</value>
    public static Keycode LeftControl { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.LeftControl); }

    /// <summary>Gets the <see cref="Keycode"/> for the left <kbd>⇧</kbd> (left shift) key</summary>
    /// <value>The <see cref="Keycode"/> for the left <kbd>⇧</kbd> (left shift) key</value>
    public static Keycode LeftShift { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.LeftShift); }

    /// <summary>Gets the <see cref="Keycode"/> for the left <kbd>ALT</kbd> (left alt) key</summary>
    /// <value>The <see cref="Keycode"/> for the left <kbd>ALT</kbd> (left alt) key</value>
    public static Keycode LeftAlt { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.LeftAlt); }

    /// <summary>Gets the <see cref="Keycode"/> for the left <kbd>⊞</kbd> (left Windows) key on PCs, left <kbd>⌘</kbd> (left command) key on Macs, or sometimes called left meta key</summary>
    /// <value>The <see cref="Keycode"/> for the left <kbd>⊞</kbd> (left Windows) key on PCs, left <kbd>⌘</kbd> (left command) key on Macs, or sometimes called left meta key</value>
    public static Keycode LeftGui { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.LeftGui); }

    /// <summary>Gets the <see cref="Keycode"/> for the right <kbd>CTRL</kbd> (right control) key</summary>
    /// <value>The <see cref="Keycode"/> for the right <kbd>CTRL</kbd> (right control) key</value>
    public static Keycode RightControl { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.RightControl); }

    /// <summary>Gets the <see cref="Keycode"/> for the right <kbd>⇧</kbd> (right shift) key</summary>
    /// <value>The <see cref="Keycode"/> for the right <kbd>⇧</kbd> (right shift) key</value>
    public static Keycode RightShift { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.RightShift); }

    /// <summary>Gets the <see cref="Keycode"/> for the right <kbd>ALT</kbd> (right alt) key, or sometimes called <kbd>ALT GR</kbd> (alt gr.) key</summary>
    /// <value>The <see cref="Keycode"/> for the right <kbd>ALT</kbd> (right alt) key, or sometimes called <kbd>ALT GR</kbd> (alt gr.) key</value>
    public static Keycode RightAlt { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.RightAlt); }

    /// <summary>Gets the <see cref="Keycode"/> for the right <kbd>⊞</kbd> (right Windows) key on PCs, right <kbd>⌘</kbd> (right command) key on Macs, or sometimes called right meta key</summary>
    /// <value>The <see cref="Keycode"/> for the right <kbd>⊞</kbd> (right Windows) key on PCs, right <kbd>⌘</kbd> (right command) key on Macs, or sometimes called right meta key</value>
    public static Keycode RightGui { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.RightGui); }

// TODO: document the following Keycodes!
#pragma warning disable CS1591

    // the original source (https://github.com/libsdl-org/SDL/blob/main/include/SDL3/SDL_Keycode.h#L346) is not sure if adding this is necessary
    public static Keycode Mode { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Mode); }

    public static Keycode Sleep { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Sleep); }

    public static Keycode Wake { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Wake); }

    public static Keycode ChannelIncrement { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.ChannelIncrement); }

    public static Keycode ChannelDecrement { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.ChannelDecrement); }
    
#pragma warning restore CS1591

    /// <summary>Provides predefined <see cref="Keycode"/>s for keys on the keypad</summary>
    public static class Keypad
    {
        /// <summary>Gets the <see cref="Keycode"/> for the <kbd>/</kbd> (divide) key on the keypad</summary>
        /// <value>The <see cref="Keycode"/> for the <kbd>/</kbd> (divide) key on the keypad</value>
        public static Keycode Divide { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.KeypadDivide); }

        /// <summary>Gets the <see cref="Keycode"/> for the <kbd>*</kbd> (multiply) key on the keypad</summary>
        /// <value>The <see cref="Keycode"/> for the <kbd>*</kbd> (multiply) key on the keypad</value>
        public static Keycode Multiply { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.KeypadMultiply); }

        /// <summary>Gets the <see cref="Keycode"/> for the <kbd>-</kbd> (minus) key on the keypad</summary>
        /// <value>The <see cref="Keycode"/> for the <kbd>-</kbd> (minus) key on the keypad</value>
        public static Keycode Minus { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.KeypadMinus); }

        /// <summary>Gets the <see cref="Keycode"/> for the <kbd>+</kbd> (divide) key on the keypad</summary>
        /// <value>The <see cref="Keycode"/> for the <kbd>+</kbd> (divide) key on the keypad</value>
        public static Keycode Plus { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.KeypadPlus); }

        /// <summary>Gets the <see cref="Keycode"/> for the <kbd>⏎</kbd> (return / enter) key on the keypad</summary>
        /// <value>The <see cref="Keycode"/> for the <kbd>⏎</kbd> (return / enter) key on the keypad</value>
        public static Keycode Enter { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.KeypadEnter); }

        /// <summary>Gets the <see cref="Keycode"/> for the <kbd>1</kbd> key on the keypad</summary>
        /// <value>The <see cref="Keycode"/> for the <kbd>1</kbd> key on the keypad</value>
        public static Keycode _1 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Keypad1); }

        /// <summary>Gets the <see cref="Keycode"/> for the <kbd>2</kbd> key on the keypad</summary>
        /// <value>The <see cref="Keycode"/> for the <kbd>2</kbd> key on the keypad</value>
        public static Keycode _2 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Keypad2); }

        /// <summary>Gets the <see cref="Keycode"/> for the <kbd>3</kbd> key on the keypad</summary>
        /// <value>The <see cref="Keycode"/> for the <kbd>3</kbd> key on the keypad</value>
        public static Keycode _3 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Keypad3); }

        /// <summary>Gets the <see cref="Keycode"/> for the <kbd>4</kbd> key on the keypad</summary>
        /// <value>The <see cref="Keycode"/> for the <kbd>4</kbd> key on the keypad</value>
        public static Keycode _4 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Keypad4); }

        /// <summary>Gets the <see cref="Keycode"/> for the <kbd>5</kbd> key on the keypad</summary>
        /// <value>The <see cref="Keycode"/> for the <kbd>5</kbd> key on the keypad</value>
        public static Keycode _5 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Keypad5); }

        /// <summary>Gets the <see cref="Keycode"/> for the <kbd>6</kbd> key on the keypad</summary>
        /// <value>The <see cref="Keycode"/> for the <kbd>6</kbd> key on the keypad</value>
        public static Keycode _6 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Keypad6); }

        /// <summary>Gets the <see cref="Keycode"/> for the <kbd>7</kbd> key on the keypad</summary>
        /// <value>The <see cref="Keycode"/> for the <kbd>7</kbd> key on the keypad</value>
        public static Keycode _7 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Keypad7); }

        /// <summary>Gets the <see cref="Keycode"/> for the <kbd>8</kbd> key on the keypad</summary>
        /// <value>The <see cref="Keycode"/> for the <kbd>8</kbd> key on the keypad</value>
        public static Keycode _8 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Keypad8); }

        /// <summary>Gets the <see cref="Keycode"/> for the <kbd>9</kbd> key on the keypad</summary>
        /// <value>The <see cref="Keycode"/> for the <kbd>9</kbd> key on the keypad</value>
        public static Keycode _9 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Keypad9); }

        /// <summary>Gets the <see cref="Keycode"/> for the <kbd>0</kbd> key on the keypad</summary>
        /// <value>The <see cref="Keycode"/> for the <kbd>0</kbd> key on the keypad</value>
        public static Keycode _0 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Keypad0); }

        /// <summary>Gets the <see cref="Keycode"/> for the <kbd>.</kbd> key on the keypad</summary>
        /// <value>The <see cref="Keycode"/> for the <kbd>.</kbd> key on the keypad</value>
        public static Keycode Period { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.KeypadPeriod); }

        /// <summary>Gets the <see cref="Keycode"/> for the <kbd>=</kbd> key</summary>
        /// <value>The <see cref="Keycode"/> for the <kbd>=</kbd> key</value>
        public static Keycode Equal { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.KeypadEqual); }

        /// <summary>Gets the <see cref="Keycode"/> for the <kbd>,</kbd> key on the keypad</summary>
        /// <value>The <see cref="Keycode"/> for the <kbd>,</kbd> key on the keypad</value>
        public static Keycode Comma { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.KeypadComma); }

        /// <summary>Gets the <see cref="Keycode"/> for the <kbd>=</kbd> AS400 key on the keypad</summary>
        /// <value>The <see cref="Keycode"/> for the <kbd>=</kbd> AS400 key on the keypad</value>
        public static Keycode EqualAS400 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.KeypadEqualAS400); }

// TODO: document the following Keycodes!
#pragma warning disable CS1591

        public static Keycode _00 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Keypad00); }

        public static Keycode _000 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Keypad000); }

#pragma warning restore CS1591        

        /// <summary>Gets the <see cref="Keycode"/> for the <kbd>(</kbd> key on the keypad</summary>
        /// <value>The <see cref="Keycode"/> for the <kbd>(</kbd> key on the keypad</value>
        public static Keycode LeftParenthesis { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.KeypadLeftParenthesis); }

        /// <summary>Gets the <see cref="Keycode"/> for the <kbd>)</kbd> key on the keypad</summary>
        /// <value>The <see cref="Keycode"/> for the <kbd>)</kbd> key on the keypad</value>
        public static Keycode RightParenthesis { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.KeypadRightParenthesis); }

        /// <summary>Gets the <see cref="Keycode"/> for the <kbd>{</kbd> key on the keypad</summary>
        /// <value>The <see cref="Keycode"/> for the <kbd>{</kbd> key on the keypad</value>
        public static Keycode LeftBrace { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.KeypadLeftBrace); }

        /// <summary>Gets the <see cref="Keycode"/> for the <kbd>}</kbd> key on the keypad</summary>
        /// <value>The <see cref="Keycode"/> for the <kbd>}</kbd> key on the keypad</value>
        public static Keycode RightBrace { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.KeypadRightBrace); }

        /// <summary>Gets the <see cref="Keycode"/> for the <kbd>⭾</kbd> (tab) key on the keypad</summary>
        /// <value>The <see cref="Keycode"/> for the <kbd>⭾</kbd> (tab) key on the keypad</value>
        public static Keycode Tab { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.KeypadTab); }

        /// <summary>Gets the <see cref="Keycode"/> for the <kbd>⌫</kbd> (back space) key on the keypad</summary>
        /// <value>The <see cref="Keycode"/> for the <kbd>⌫</kbd> (back space) key on the keypad</value>
        public static Keycode Backspace { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.KeypadBackspace); }

        /// <summary>Gets the <see cref="Keycode"/> for the <kbd>A</kbd> key on the keypad</summary>
        /// <value>The <see cref="Keycode"/> for the <kbd>A</kbd> key on the keypad</value>
        public static Keycode A { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.KeypadA); }

        /// <summary>Gets the <see cref="Keycode"/> for the <kbd>B</kbd> key on the keypad</summary>
        /// <value>The <see cref="Keycode"/> for the <kbd>B</kbd> key on the keypad</value>
        public static Keycode B { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.KeypadB); }

        /// <summary>Gets the <see cref="Keycode"/> for the <kbd>C</kbd> key on the keypad</summary>
        /// <value>The <see cref="Keycode"/> for the <kbd>C</kbd> key on the keypad</value>
        public static Keycode C { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.KeypadC); }

        /// <summary>Gets the <see cref="Keycode"/> for the <kbd>D</kbd> key on the keypad</summary>
        /// <value>The <see cref="Keycode"/> for the <kbd>D</kbd> key on the keypad</value>
        public static Keycode D { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.KeypadD); }

        /// <summary>Gets the <see cref="Keycode"/> for the <kbd>E</kbd> key on the keypad</summary>
        /// <value>The <see cref="Keycode"/> for the <kbd>E</kbd> key on the keypad</value>
        public static Keycode E { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.KeypadE); }

        /// <summary>Gets the <see cref="Keycode"/> for the <kbd>F</kbd> key on the keypad</summary>
        /// <value>The <see cref="Keycode"/> for the <kbd>F</kbd> key on the keypad</value>
        public static Keycode F { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.KeypadF); }

        /// <summary>Gets the <see cref="Keycode"/> for the <kbd>XOR</kbd> (xor; sometimes '^') key on the keypad</summary>
        /// <value>The <see cref="Keycode"/> for the <kbd>XOR</kbd> (xor; sometimes '^') key on the keypad</value>
        public static Keycode Xor { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.KeypadXor); }

        /// <summary>Gets the <see cref="Keycode"/> for the <kbd>xʸ</kbd> (power / exponentiation; sometimes '^') key on the keypad</summary>
        /// <value>The <see cref="Keycode"/> for the <kbd>xʸ</kbd> (power / exponentiation; sometimes '^') key on the keypad</value>
        public static Keycode Power { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.KeypadPower); }

        /// <summary>Gets the <see cref="Keycode"/> for the <kbd>%</kbd> key on the keypad</summary>
        /// <value>The <see cref="Keycode"/> for the <kbd>%</kbd> key on the keypad</value>
        public static Keycode Percent { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.KeypadPercent); }

        /// <summary>Gets the <see cref="Keycode"/> for the <kbd>&lt;</kbd> key on the keypad</summary>
        /// <value>The <see cref="Keycode"/> for the <kbd>&lt;</kbd> key on the keypad</value>
        public static Keycode Less { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.KeypadLess); }

        /// <summary>Gets the <see cref="Keycode"/> for the <kbd>&gt;</kbd> key on the keypad</summary>
        /// <value>The <see cref="Keycode"/> for the <kbd>&gt;</kbd> key on the keypad</value>
        public static Keycode Greater { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.KeypadGreater); }

        /// <summary>Gets the <see cref="Keycode"/> for the <kbd>&amp;</kbd> (and; sometimes 'AND') key on the keypad</summary>
        /// <value>The <see cref="Keycode"/> for the <kbd>&amp;</kbd> (and; sometimes 'AND') key on the keypad</value>
        public static Keycode Ampersand { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.KeypadAmpersand); }

        /// <summary>Gets the <see cref="Keycode"/> for the <kbd>&amp;&amp;</kbd> key on the keypad</summary>
        /// <value>The <see cref="Keycode"/> for the <kbd>&amp;&amp;</kbd> key on the keypad</value>
        public static Keycode DoubleAmpersand { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.KeypadDoubleAmpersand); }

        /// <summary>Gets the <see cref="Keycode"/> for the <kbd>|</kbd> (or; sometimes 'OR') key on the keypad</summary>
        /// <value>The <see cref="Keycode"/> for the <kbd>|</kbd> (or; sometimes 'OR') key on the keypad</value>
        public static Keycode VerticalBar { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.KeypadVerticalBar); }

        /// <summary>Gets the <see cref="Keycode"/> for the <kbd>||</kbd> key on the keypad</summary>
        /// <value>The <see cref="Keycode"/> for the <kbd>||</kbd> key on the keypad</value>
        public static Keycode DoubleVerticalBar { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.KeypadDoubleVerticalBar); }

        /// <summary>Gets the <see cref="Keycode"/> for the <kbd>:</kbd> key on the keypad</summary>
        /// <value>The <see cref="Keycode"/> for the <kbd>:</kbd> key on the keypad</value>
        public static Keycode Colon { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.KeypadColon); }

        /// <summary>Gets the <see cref="Keycode"/> for the <kbd>#</kbd> key on the keypad</summary>
        /// <value>The <see cref="Keycode"/> for the <kbd>#</kbd> key on the keypad</value>
        public static Keycode Hash { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.KeypadHash); }

        /// <summary>Gets the <see cref="Keycode"/> for the <kbd>⎵</kbd> (space) key on the keypad</summary>
        /// <value>The <see cref="Keycode"/> for the <kbd>⎵</kbd> (space) key on the keypad</value>
        public static Keycode Space { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.KeypadSpace); }

        /// <summary>Gets the <see cref="Keycode"/> for the <kbd>@</kbd> key on the keypad</summary>
        /// <value>The <see cref="Keycode"/> for the <kbd>@</kbd> key on the keypad</value>
        public static Keycode At { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.KeypadAt); }

        /// <summary>Gets the <see cref="Keycode"/> for the <kbd>!</kbd> key on the keypad</summary>
        /// <value>The <see cref="Keycode"/> for the <kbd>!</kbd> key on the keypad</value>
        public static Keycode ExclamationMark { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.KeypadExclamationMark); }

        /// <summary>Gets the <see cref="Keycode"/> for the <kbd>MEM STORE</kbd> (mem store) key on the keypad</summary>
        /// <value>The <see cref="Keycode"/> for the <kbd>MEM STORE</kbd> (mem store) key on the keypad</value>
        public static Keycode MemStore { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.KeypadMemStore); }

        /// <summary>Gets the <see cref="Keycode"/> for the <kbd>MEM RECALL</kbd> (mem recall) key on the keypad</summary>
        /// <value>The <see cref="Keycode"/> for the <kbd>MEM RECALL</kbd> (mem recall) key on the keypad</value>
        public static Keycode MemRecall { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.KeypadMemRecall); }

        /// <summary>Gets the <see cref="Keycode"/> for the <kbd>MEM CLEAR</kbd> (mem clear) key on the keypad</summary>
        /// <value>The <see cref="Keycode"/> for the <kbd>MEM CLEAR</kbd> (mem clear) key on the keypad</value>
        public static Keycode MemClear { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.KeypadMemClear); }

        /// <summary>Gets the <see cref="Keycode"/> for the <kbd>MEM +</kbd> (mem add) key on the keypad</summary>
        /// <value>The <see cref="Keycode"/> for the <kbd>MEM +</kbd> (mem add) key on the keypad</value>
        public static Keycode MemAdd { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.KeypadMemAdd); }

        /// <summary>Gets the <see cref="Keycode"/> for the <kbd>MEM -</kbd> (mem subtract) key on the keypad</summary>
        /// <value>The <see cref="Keycode"/> for the <kbd>MEM -</kbd> (mem subtract) key on the keypad</value>
        public static Keycode MemSubtract { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.KeypadMemSubtract); }

        /// <summary>Gets the <see cref="Keycode"/> for the <kbd>MEM *</kbd> (mem multiply) key on the keypad</summary>
        /// <value>The <see cref="Keycode"/> for the <kbd>MEM *</kbd> (mem multiply) key on the keypad</value>
        public static Keycode MemMultiply { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.KeypadMemMultiply); }

        /// <summary>Gets the <see cref="Keycode"/> for the <kbd>MEM /</kbd> (mem divide) key on the keypad</summary>
        /// <value>The <see cref="Keycode"/> for the <kbd>MEM /</kbd> (mem divide) key on the keypad</value>
        public static Keycode MemDivide { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.KeypadMemDivide); }

        /// <summary>Gets the <see cref="Keycode"/> for the <kbd>±</kbd> (plus-minus) key on the keypad</summary>
        /// <value>The <see cref="Keycode"/> for the <kbd>±</kbd> (plus-minus) key on the keypad</value>
        public static Keycode PlusMinus { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.KeypadPlusMinus); }

        /// <summary>Gets the <see cref="Keycode"/> for the <kbd>CLEAR</kbd> (clear) key on the keypad</summary>
        /// <value>The <see cref="Keycode"/> for the <kbd>CLEAR</kbd> (clear) key on the keypad</value>
        public static Keycode Clear { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.KeypadClear); }

// TODO: document the following Keycodes!
#pragma warning disable CS1591

        public static Keycode ClearEntry { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.KeypadClearEntry); }

        public static Keycode Binary { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.KeypadBinary); }
    
        public static Keycode Octal { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.KeypadOctal); }
    
        public static Keycode Decimal { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.KeypadDecimal); }
    
        public static Keycode Hexadecimal { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.KeypadHexadecimal); }
    
#pragma warning restore CS1591
    }

    /// <summary>Provides predefined <see cref="Keycode"/>s for media control keys</summary>
    public static class Media
    {
        /// <summary>Gets the <see cref="Keycode"/> for the <kbd>⏵</kbd> (media play) key</summary>
        /// <value>The <see cref="Keycode"/> for the <kbd>⏵</kbd> (media play) key</value>
        public static Keycode Play { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.MediaPlay); }

        /// <summary>Gets the <see cref="Keycode"/> for the <kbd>⏸</kbd> (media pause) key</summary>
        /// <value>The <see cref="Keycode"/> for the <kbd>⏸</kbd> (media pause) key</value>
        public static Keycode Pause { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.MediaPause); }

        /// <summary>Gets the <see cref="Keycode"/> for the <kbd>⏺</kbd> (media record) key</summary>
        /// <value>The <see cref="Keycode"/> for the <kbd>⏺</kbd> (media record) key</value>
        public static Keycode Record { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.MediaRecord); }

        /// <summary>Gets the <see cref="Keycode"/> for the <kbd>⏩</kbd> (media fast forward) key</summary>
        /// <value>The <see cref="Keycode"/> for the <kbd>⏩</kbd> (media fast forward) key</value>
        public static Keycode FastForward { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.MediaFastForward); }

        /// <summary>Gets the <see cref="Keycode"/> for the <kbd>⏪</kbd> (media rewind) key</summary>
        /// <value>The <see cref="Keycode"/> for the <kbd>⏪</kbd> (media rewind) key</value>
        public static Keycode Rewind { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.MediaRewind); }

        /// <summary>Gets the <see cref="Keycode"/> for the <kbd>⏭</kbd> (media next track) key</summary>
        /// <value>The <see cref="Keycode"/> for the <kbd>⏭</kbd> (media next track) key</value>
        public static Keycode NextTrack { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.MediaNextTrack); }

        /// <summary>Gets the <see cref="Keycode"/> for the <kbd>⏮</kbd> (media previous track) key</summary>
        /// <value>The <see cref="Keycode"/> for the <kbd>⏮</kbd> (media previous track) key</value>
        public static Keycode PreviousTrack { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.MediaPreviousTrack); }

        /// <summary>Gets the <see cref="Keycode"/> for the <kbd>⏹</kbd> (media stop) key</summary>
        /// <value>The <see cref="Keycode"/> for the <kbd>⏹</kbd> (media stop) key</value>
        public static Keycode Stop { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.MediaStop); }

        /// <summary>Gets the <see cref="Keycode"/> for the <kbd>⏏</kbd> (media eject) key</summary>
        /// <value>The <see cref="Keycode"/> for the <kbd>⏏</kbd> (media eject) key</value>
        public static Keycode Eject { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.MediaEject); }

        /// <summary>Gets the <see cref="Keycode"/> for the <kbd>⏯</kbd> (play-pause eject) key</summary>
        /// <value>The <see cref="Keycode"/> for the <kbd>⏯</kbd> (play-pause eject) key</value>
        public static Keycode PlayPause { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.MediaPlayPause); }

// TODO: document the following Keycodes!
#pragma warning disable CS1591

        public static Keycode Select { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.MediaSelect); }

#pragma warning restore CS1591
    }

    /// <summary>Provides predefined <see cref="Keycode"/>s for application control keys</summary>
    public static class ApplicationControl
    {
        /// <summary>Gets the <see cref="Keycode"/> for the application control "<em>New</em>" key</summary>
        /// <value>The <see cref="Keycode"/> for the application control "<em>New</em>" key</value>
        public static Keycode New { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.ApplicationControlNew); }

        /// <summary>Gets the <see cref="Keycode"/> for the application control "<em>Open</em>" key</summary>
        /// <value>The <see cref="Keycode"/> for the application control "<em>Open</em>" key</value>
        public static Keycode Open { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.ApplicationControlOpen); }

        /// <summary>Gets the <see cref="Keycode"/> for the application control "<em>Close</em>" key</summary>
        /// <value>The <see cref="Keycode"/> for the application control "<em>Close</em>" key</value>
        public static Keycode Close { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.ApplicationControlClose); }

        /// <summary>Gets the <see cref="Keycode"/> for the application control "<em>Exit</em>" key</summary>
        /// <value>The <see cref="Keycode"/> for the application control "<em>Exit</em>" key</value>
        public static Keycode Exit { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.ApplicationControlExit); }

        /// <summary>Gets the <see cref="Keycode"/> for the application control "<em>Save</em>" key</summary>
        /// <value>The <see cref="Keycode"/> for the application control "<em>Save</em>" key</value>
        public static Keycode Save { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.ApplicationControlSave); }

        /// <summary>Gets the <see cref="Keycode"/> for the application control "<em>Print</em>" key</summary>
        /// <value>The <see cref="Keycode"/> for the application control "<em>Print</em>" key</value>
        public static Keycode Print { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.ApplicationControlPrint); }

        /// <summary>Gets the <see cref="Keycode"/> for the application control "<em>Properties</em>" key</summary>
        /// <value>The <see cref="Keycode"/> for the application control "<em>Properties</em>" key</value>
        public static Keycode Properties { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.ApplicationControlProperties); }

        /// <summary>Gets the <see cref="Keycode"/> for the application control "<em>Search</em>" key</summary>
        /// <value>The <see cref="Keycode"/> for the application control "<em>Search</em>" key</value>
        public static Keycode Search { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.ApplicationControlSearch); }

        /// <summary>Gets the <see cref="Keycode"/> for the application control "<em>Home</em>" key</summary>
        /// <value>The <see cref="Keycode"/> for the application control "<em>Home</em>" key</value>
        public static Keycode Home { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.ApplicationControlHome); }

        /// <summary>Gets the <see cref="Keycode"/> for the application control "<em>Back</em>" key</summary>
        /// <value>The <see cref="Keycode"/> for the application control "<em>Back</em>" key</value>
        public static Keycode Back { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.ApplicationControlBack); }

        /// <summary>Gets the <see cref="Keycode"/> for the application control "<em>Forward</em>" key</summary>
        /// <value>The <see cref="Keycode"/> for the application control "<em>Forward</em>" key</value>
        public static Keycode Forward { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.ApplicationControlForward); }

        /// <summary>Gets the <see cref="Keycode"/> for the application control "<em>Stop</em>" key</summary>
        /// <value>The <see cref="Keycode"/> for the application control "<em>Stop</em>" key</value>
        public static Keycode Stop { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.ApplicationControlStop); }

        /// <summary>Gets the <see cref="Keycode"/> for the application control "<em>Refresh</em>" key</summary>
        /// <value>The <see cref="Keycode"/> for the application control "<em>Refresh</em>" key</value>
        public static Keycode Refresh { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.ApplicationControlRefresh); }

        /// <summary>Gets the <see cref="Keycode"/> for the application control "<em>Bookmarks</em>" key</summary>
        /// <value>The <see cref="Keycode"/> for the application control "<em>Bookmarks</em>" key</value>
        public static Keycode Bookmarks { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.ApplicationControlBookmarks); }
    }

    /// <summary>Provides predefined <see cref="Keycode"/>s for keys on mobiles</summary>
    public static class Mobile
    {
        /// <summary>Gets the <see cref="Keycode"/> for left software defined key on mobiles</summary>
        /// <value>The <see cref="Keycode"/> for left software defined key on mobiles</value>
        /// <remarks>
        /// <para>
        /// This key is usually situated below the display on phones and used as a multi-function feature key for selecting a software defined function shown on the bottom left of the display.
        /// </para>
        /// </remarks>
        public static Keycode SoftwareLeft { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.SoftwareLeft); }

        /// <summary>Gets the <see cref="Keycode"/> for right software defined key on mobiles</summary>
        /// <value>The <see cref="Keycode"/> for right software defined key on mobiles</value>
        /// <remarks>
        /// <para>
        /// This key is usually situated below the display on phones and used as a multi-function feature key for selecting a software defined function shown on the bottom right of the display.
        /// </para>
        /// </remarks>
        public static Keycode SoftwareRight { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.SoftwareRight); }


        /// <summary>Gets the <see cref="Keycode"/> for the <kbd>📞</kbd> (phone call) key on mobiles</summary>
        /// <value>The <see cref="Keycode"/> for the <kbd>📞</kbd> (phone call) key on mobiles</value>
        public static Keycode Call { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Call); }

        /// <summary>Gets the <see cref="Keycode"/> for the <kbd>END 📞</kbd> (end phone call) key on mobiles</summary>
        /// <value>The <see cref="Keycode"/> for the <kbd>END 📞</kbd> (end phone call) key on mobiles</value>
        public static Keycode EndCall { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.EndCall); }
    }

    /// <summary>Provides predefined <see cref="Keycode"/>s for keys which do not map to certain <see cref="Scancode"/>s or to certain Unicode code points</summary>
    public static class Extended
    {
        /// <summary>Gets the <see cref="Keycode"/> for the "Left Tab" key</summary>
        /// <value>The <see cref="Keycode"/> for the "Left Tab" key</value>
        public static Keycode LeftTab { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.LeftTab); }

        /// <summary>Gets the <see cref="Keycode"/> for the "Level 5 Shift" key</summary>
        /// <value>The <see cref="Keycode"/> for the "Level 5 Shift" key</value>
        public static Keycode Level5Shift { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Level5Shift); }

        /// <summary>Gets the <see cref="Keycode"/> for the "Multi-key Compose" key</summary>
        /// <value>The <see cref="Keycode"/> for the "Level 5 Shift" key</value>
        public static Keycode MultiKeyCompose { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.MultiKeyCompose); }

        /// <summary>Gets the <see cref="Keycode"/> for the "Left Meta" key</summary>
        /// <value>The <see cref="Keycode"/> for the "Left Meta" key</value>
        public static Keycode LeftMeta { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.LeftMeta); }

        /// <summary>Gets the <see cref="Keycode"/> for the "Right Meta" key</summary>
        /// <value>The <see cref="Keycode"/> for the "Right Meta" key</value>
        public static Keycode RightMeta { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.RightMeta); }

        /// <summary>Gets the <see cref="Keycode"/> for the "Left Hyper" key</summary>
        /// <value>The <see cref="Keycode"/> for the "Left Hyper" key</value>
        public static Keycode LeftHyper { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.LeftHyper); }

        /// <summary>Gets the <see cref="Keycode"/> for the "Right Hyper" key</summary>
        /// <value>The <see cref="Keycode"/> for the "Right Hyper" key</value>
        public static Keycode RightHyper { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.RightHyper); }
    }

    private static string? KnownKindToString(Kind kind) => kind switch
    {
        Kind.Unknown => nameof(Unknown),
        Kind.Return => nameof(Return),
        Kind.Escape => nameof(Escape),
        Kind.Backspace => nameof(Backspace),
        Kind.Tab => nameof(Tab),
        Kind.Space => nameof(Space),
        Kind.ExclamationMark => nameof(ExclamationMark),
        Kind.DoubleApostrophe => nameof(DoubleApostrophe),
        Kind.Hash => nameof(Hash),
        Kind.Dollar => nameof(Dollar),
        Kind.Percent => nameof(Percent),
        Kind.Ampersand => nameof(Ampersand),
        Kind.Apostrophe => nameof(Apostrophe),
        Kind.LeftParenthesis => nameof(LeftParenthesis),
        Kind.RightParenthesis => nameof(RightParenthesis),
        Kind.Asterisk => nameof(Asterisk),
        Kind.Plus => nameof(Plus),
        Kind.Comma => nameof(Comma),
        Kind.Minus => nameof(Minus),
        Kind.Period => nameof(Period),
        Kind.Slash => nameof(Slash),
        Kind._0 => nameof(_0),
        Kind._1 => nameof(_1),
        Kind._2 => nameof(_2),
        Kind._3 => nameof(_3),
        Kind._4 => nameof(_4),
        Kind._5 => nameof(_5),
        Kind._6 => nameof(_6),
        Kind._7 => nameof(_7),       
        Kind._8 => nameof(_8),
        Kind._9 => nameof(_9),
        Kind.Colon => nameof(Colon),
        Kind.Semicolon => nameof(Semicolon),
        Kind.Less => nameof(Less),
        Kind.Equal => nameof(Equal),
        Kind.Greater => nameof(Greater),
        Kind.QuestionMark => nameof(QuestionMark),
        Kind.At => nameof(At),
        Kind.LeftBracket => nameof(LeftBracket),
        Kind.Backslash => nameof(Backslash),
        Kind.RightBracket => nameof(RightBracket),
        Kind.Caret => nameof(Caret),
        Kind.Underscore => nameof(Underscore),
        Kind.Grave => nameof(Grave),
        Kind.A => nameof(A),
        Kind.B => nameof(B),
        Kind.C => nameof(C),
        Kind.D => nameof(D),
        Kind.E => nameof(E),
        Kind.F => nameof(F),
        Kind.G => nameof(G),
        Kind.H => nameof(H),
        Kind.I => nameof(I),
        Kind.J => nameof(J),
        Kind.K => nameof(K),
        Kind.L => nameof(L),
        Kind.M => nameof(M),
        Kind.N => nameof(N),
        Kind.O => nameof(O),
        Kind.P => nameof(P),
        Kind.Q => nameof(Q),
        Kind.R => nameof(R),
        Kind.S => nameof(S),
        Kind.T => nameof(T),
        Kind.U => nameof(U),
        Kind.V => nameof(V),
        Kind.W => nameof(W),
        Kind.X => nameof(X),
        Kind.Y => nameof(Y),
        Kind.Z => nameof(Z),
        Kind.LeftBrace => nameof(LeftBrace),
        Kind.Pipe => nameof(Pipe),
        Kind.RightBrace => nameof(RightBrace),
        Kind.Tilde => nameof(Tilde),
        Kind.Delete => nameof(Delete),
        Kind.PlusMinus => nameof(PlusMinus),
        Kind.CapsLock => nameof(CapsLock),
        Kind.F1 => nameof(F1),
        Kind.F2 => nameof(F2),
        Kind.F3 => nameof(F3),
        Kind.F4 => nameof(F4),
        Kind.F5 => nameof(F5),
        Kind.F6 => nameof(F6),
        Kind.F7 => nameof(F7),
        Kind.F8 => nameof(F8),
        Kind.F9 => nameof(F9),
        Kind.F10 => nameof(F10),
        Kind.F11 => nameof(F11),
        Kind.F12 => nameof(F12),
        Kind.PrintScreen => nameof(PrintScreen),
        Kind.ScrollLock => nameof(ScrollLock),
        Kind.Pause => nameof(Pause),
        Kind.Insert => nameof(Insert),
        Kind.Home => nameof(Home),
        Kind.PageUp => nameof(PageUp),
        Kind.End => nameof(End),
        Kind.PageDown => nameof(PageDown),
        Kind.Right => nameof(Right),
        Kind.Left => nameof(Left),
        Kind.Down => nameof(Down),
        Kind.Up => nameof(Up),
        Kind.NumLockOrClear => nameof(NumLockOrClear),
        Kind.KeypadDivide => $"{nameof(Keypad)}.{nameof(Keypad.Divide)}",
        Kind.KeypadMultiply => $"{nameof(Keypad)}.{nameof(Keypad.Multiply)}",        
        Kind.KeypadMinus => $"{nameof(Keypad)}.{nameof(Keypad.Minus)}",
        Kind.KeypadPlus => $"{nameof(Keypad)}.{nameof(Keypad.Plus)}",
        Kind.KeypadEnter => $"{nameof(Keypad)}.{nameof(Keypad.Enter)}",
        Kind.Keypad1 => $"{nameof(Keypad)}.{nameof(Keypad._1)}",
        Kind.Keypad2 => $"{nameof(Keypad)}.{nameof(Keypad._2)}",
        Kind.Keypad3 => $"{nameof(Keypad)}.{nameof(Keypad._3)}",
        Kind.Keypad4 => $"{nameof(Keypad)}.{nameof(Keypad._4)}",
        Kind.Keypad5 => $"{nameof(Keypad)}.{nameof(Keypad._5)}",
        Kind.Keypad6 => $"{nameof(Keypad)}.{nameof(Keypad._6)}",
        Kind.Keypad7 => $"{nameof(Keypad)}.{nameof(Keypad._7)}",
        Kind.Keypad8 => $"{nameof(Keypad)}.{nameof(Keypad._8)}",
        Kind.Keypad9 => $"{nameof(Keypad)}.{nameof(Keypad._9)}",
        Kind.Keypad0 => $"{nameof(Keypad)}.{nameof(Keypad._0)}",
        Kind.KeypadPeriod => $"{nameof(Keypad)}.{nameof(Keypad.Period)}",
        Kind.Application => nameof(Application),

#pragma warning disable SDL5010
        Kind.Power => nameof(Power),
#pragma warning restore SDL5010
        
        Kind.KeypadEqual => $"{nameof(Keypad)}.{nameof(Keypad.Equal)}",
        Kind.F13 => nameof(F13),
        Kind.F14 => nameof(F14),
        Kind.F15 => nameof(F15),
        Kind.F16 => nameof(F16),
        Kind.F17 => nameof(F17),
        Kind.F18 => nameof(F18),
        Kind.F19 => nameof(F19),
        Kind.F20 => nameof(F20),
        Kind.F21 => nameof(F21),
        Kind.F22 => nameof(F22),
        Kind.F23 => nameof(F23),
        Kind.F24 => nameof(F24),
        Kind.Execute => nameof(Execute),
        Kind.Help => nameof(Help),
        Kind.Menu => nameof(Menu),
        Kind.Select => nameof(Select),
        Kind.Stop => nameof(Stop),
        Kind.Again => nameof(Again),
        Kind.Undo => nameof(Undo),
        Kind.Cut => nameof(Cut),
        Kind.Copy => nameof(Copy),
        Kind.Paste => nameof(Paste),
        Kind.Find => nameof(Find),
        Kind.Mute => nameof(Mute),
        Kind.VolumeUp => nameof(VolumeUp),
        Kind.VolumeDown => nameof(VolumeDown),
        Kind.KeypadComma => $"{nameof(Keypad)}.{nameof(Keypad.Comma)}",
        Kind.KeypadEqualAS400 => $"{nameof(Keypad)}.{nameof(Keypad.EqualAS400)}",
        Kind.AltErase => nameof(AltErase),
        Kind.SysReq => nameof(SysReq),
        Kind.Cancel => nameof(Cancel),
        Kind.Clear => nameof(Clear),
        Kind.Prior => nameof(Prior),
        Kind.Return2 => nameof(Return2),
        Kind.Separator => nameof(Separator),
        Kind.Out => nameof(Out),
        Kind.Oper => nameof(Oper),
        Kind.ClearAgain => nameof(ClearAgain),
        Kind.CrSel => nameof(CrSel),
        Kind.ExSel => nameof(ExSel),
        Kind.Keypad00 => $"{nameof(Keypad)}.{nameof(Keypad._00)}",        
        Kind.Keypad000 => $"{nameof(Keypad)}.{nameof(Keypad._000)}",
        Kind.ThousandsSeparator => nameof(ThousandsSeparator),
        Kind.DecimalSeparator => nameof(DecimalSeparator),
        Kind.CurrencyUnit => nameof(CurrencyUnit),
        Kind.CurrencySubunit => nameof(CurrencySubunit),
        Kind.KeypadLeftParenthesis => $"{nameof(Keypad)}.{nameof(Keypad.LeftParenthesis)}", 
        Kind.KeypadRightParenthesis => $"{nameof(Keypad)}.{nameof(Keypad.RightParenthesis)}",
        Kind.KeypadLeftBrace => $"{nameof(Keypad)}.{nameof(Keypad.LeftBrace)}", 
        Kind.KeypadRightBrace => $"{nameof(Keypad)}.{nameof(Keypad.RightBrace)}",
        Kind.KeypadTab => $"{nameof(Keypad)}.{nameof(Keypad.Tab)}",
        Kind.KeypadBackspace => $"{nameof(Keypad)}.{nameof(Keypad.Backspace)}",        
        Kind.KeypadA => $"{nameof(Keypad)}.{nameof(Keypad.A)}",
        Kind.KeypadB => $"{nameof(Keypad)}.{nameof(Keypad.B)}",
        Kind.KeypadC => $"{nameof(Keypad)}.{nameof(Keypad.C)}",
        Kind.KeypadD => $"{nameof(Keypad)}.{nameof(Keypad.D)}",
        Kind.KeypadE => $"{nameof(Keypad)}.{nameof(Keypad.E)}",
        Kind.KeypadF => $"{nameof(Keypad)}.{nameof(Keypad.F)}",        
        Kind.KeypadXor => $"{nameof(Keypad)}.{nameof(Keypad.Xor)}",   
        Kind.KeypadPower => $"{nameof(Keypad)}.{nameof(Keypad.Power)}",
        Kind.KeypadPercent => $"{nameof(Keypad)}.{nameof(Keypad.Percent)}",
        Kind.KeypadLess => $"{nameof(Keypad)}.{nameof(Keypad.Less)}",
        Kind.KeypadGreater => $"{nameof(Keypad)}.{nameof(Keypad.Greater)}",
        Kind.KeypadAmpersand => $"{nameof(Keypad)}.{nameof(Keypad.Ampersand)}",
        Kind.KeypadDoubleAmpersand => $"{nameof(Keypad)}.{nameof(Keypad.DoubleAmpersand)}",
        Kind.KeypadVerticalBar => $"{nameof(Keypad)}.{nameof(Keypad.VerticalBar)}",
        Kind.KeypadDoubleVerticalBar => $"{nameof(Keypad)}.{nameof(Keypad.DoubleVerticalBar)}",
        Kind.KeypadColon => $"{nameof(Keypad)}.{nameof(Keypad.Colon)}",
        Kind.KeypadHash => $"{nameof(Keypad)}.{nameof(Keypad.Hash)}",
        Kind.KeypadSpace => $"{nameof(Keypad)}.{nameof(Keypad.Space)}",
        Kind.KeypadAt => $"{nameof(Keypad)}.{nameof(Keypad.At)}",
        Kind.KeypadExclamationMark => $"{nameof(Keypad)}.{nameof(Keypad.ExclamationMark)}",
        Kind.KeypadMemStore => $"{nameof(Keypad)}.{nameof(Keypad.MemStore)}",
        Kind.KeypadMemRecall => $"{nameof(Keypad)}.{nameof(Keypad.MemRecall)}",
        Kind.KeypadMemClear => $"{nameof(Keypad)}.{nameof(Keypad.MemClear)}",
        Kind.KeypadMemAdd => $"{nameof(Keypad)}.{nameof(Keypad.MemAdd)}",
        Kind.KeypadMemSubtract => $"{nameof(Keypad)}.{nameof(Keypad.MemSubtract)}",
        Kind.KeypadMemMultiply => $"{nameof(Keypad)}.{nameof(Keypad.MemMultiply)}",
        Kind.KeypadMemDivide => $"{nameof(Keypad)}.{nameof(Keypad.MemDivide)}",
        Kind.KeypadPlusMinus => $"{nameof(Keypad)}.{nameof(Keypad.PlusMinus)}",
        Kind.KeypadClear => $"{nameof(Keypad)}.{nameof(Keypad.Clear)}",
        Kind.KeypadClearEntry => $"{nameof(Keypad)}.{nameof(Keypad.ClearEntry)}",
        Kind.KeypadBinary => $"{nameof(Keypad)}.{nameof(Keypad.Binary)}",
        Kind.KeypadOctal => $"{nameof(Keypad)}.{nameof(Keypad.Octal)}",
        Kind.KeypadDecimal => $"{nameof(Keypad)}.{nameof(Keypad.Decimal)}",
        Kind.KeypadHexadecimal => $"{nameof(Keypad)}.{nameof(Keypad.Hexadecimal)}",
        Kind.LeftControl => nameof(LeftControl),
        Kind.LeftShift => nameof(LeftShift),
        Kind.LeftAlt => nameof(LeftAlt),
        Kind.LeftGui => nameof(LeftGui),
        Kind.RightControl => nameof(RightControl),
        Kind.RightShift => nameof(RightShift),
        Kind.RightAlt => nameof(RightAlt),
        Kind.RightGui => nameof(RightGui),
        Kind.Mode => nameof(Mode),
        Kind.Sleep => nameof(Sleep),
        Kind.Wake => nameof(Wake),
        Kind.ChannelIncrement => nameof(ChannelIncrement),
        Kind.ChannelDecrement => nameof(ChannelDecrement),
        Kind.MediaPlay => $"{nameof(Media)}.{nameof(Media.Play)}",
        Kind.MediaPause => $"{nameof(Media)}.{nameof(Media.Pause)}",
        Kind.MediaRecord => $"{nameof(Media)}.{nameof(Media.Record)}",
        Kind.MediaFastForward => $"{nameof(Media)}.{nameof(Media.FastForward)}",
        Kind.MediaRewind => $"{nameof(Media)}.{nameof(Media.Rewind)}",
        Kind.MediaNextTrack => $"{nameof(Media)}.{nameof(Media.NextTrack)}",
        Kind.MediaPreviousTrack => $"{nameof(Media)}.{nameof(Media.PreviousTrack)}",
        Kind.MediaStop => $"{nameof(Media)}.{nameof(Media.Stop)}",
        Kind.MediaEject => $"{nameof(Media)}.{nameof(Media.Eject)}",
        Kind.MediaPlayPause => $"{nameof(Media)}.{nameof(Media.PlayPause)}",
        Kind.MediaSelect => $"{nameof(Media)}.{nameof(Media.Select)}",
        Kind.ApplicationControlNew => $"{nameof(ApplicationControl)}.{nameof(ApplicationControl.New)}",
        Kind.ApplicationControlOpen => $"{nameof(ApplicationControl)}.{nameof(ApplicationControl.Open)}",
        Kind.ApplicationControlClose => $"{nameof(ApplicationControl)}.{nameof(ApplicationControl.Close)}",
        Kind.ApplicationControlExit => $"{nameof(ApplicationControl)}.{nameof(ApplicationControl.Exit)}",
        Kind.ApplicationControlSave => $"{nameof(ApplicationControl)}.{nameof(ApplicationControl.Save)}",
        Kind.ApplicationControlPrint => $"{nameof(ApplicationControl)}.{nameof(ApplicationControl.Print)}",
        Kind.ApplicationControlProperties => $"{nameof(ApplicationControl)}.{nameof(ApplicationControl.Properties)}",
        Kind.ApplicationControlSearch => $"{nameof(ApplicationControl)}.{nameof(ApplicationControl.Search)}",
        Kind.ApplicationControlHome => $"{nameof(ApplicationControl)}.{nameof(ApplicationControl.Home)}",
        Kind.ApplicationControlBack => $"{nameof(ApplicationControl)}.{nameof(ApplicationControl.Back)}",
        Kind.ApplicationControlForward => $"{nameof(ApplicationControl)}.{nameof(ApplicationControl.Forward)}",
        Kind.ApplicationControlStop => $"{nameof(ApplicationControl)}.{nameof(ApplicationControl.Stop)}",
        Kind.ApplicationControlRefresh => $"{nameof(ApplicationControl)}.{nameof(ApplicationControl.Refresh)}",
        Kind.ApplicationControlBookmarks => $"{nameof(ApplicationControl)}.{nameof(ApplicationControl.Bookmarks)}",
        Kind.SoftwareLeft => $"{nameof(Mobile)}.{nameof(Mobile.SoftwareLeft)}",
        Kind.SoftwareRight => $"{nameof(Mobile)}.{nameof(Mobile.SoftwareRight)}",
        Kind.Call => $"{nameof(Mobile)}.{nameof(Mobile.Call)}",
        Kind.EndCall => $"{nameof(Mobile)}.{nameof(Mobile.EndCall)}",
        Kind.LeftTab => $"{nameof(Extended)}.{nameof(Extended.LeftTab)}",
        Kind.Level5Shift => $"{nameof(Extended)}.{nameof(Extended.Level5Shift)}",
        Kind.MultiKeyCompose => $"{nameof(Extended)}.{nameof(Extended.MultiKeyCompose)}",
        Kind.LeftMeta => $"{nameof(Extended)}.{nameof(Extended.LeftMeta)}",
        Kind.RightMeta => $"{nameof(Extended)}.{nameof(Extended.RightMeta)}",
        Kind.LeftHyper => $"{nameof(Extended)}.{nameof(Extended.LeftHyper)}",
        Kind.RightHyper => $"{nameof(Extended)}.{nameof(Extended.RightHyper)}",

        _ => default
    };
}
