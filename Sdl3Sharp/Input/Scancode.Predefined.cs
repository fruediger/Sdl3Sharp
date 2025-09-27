using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Input;

partial struct Scancode
{
    /// <summary>Gets a representative for an unknown <see cref="Scancode"/></summary>
    /// <value>A representative for an unknown <see cref="Scancode"/></value>
    public static Scancode Unknown { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Unknown); }

    /// <summary>Gets the <see cref="Scancode"/> for the <kbd>A</kbd> key</summary>
    /// <value>The <see cref="Scancode"/> for the <kbd>A</kbd> key</value>
    public static Scancode A { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.A); }

    /// <summary>Gets the <see cref="Scancode"/> for the <kbd>B</kbd> key</summary>
    /// <value>The <see cref="Scancode"/> for the <kbd>B</kbd> key</value>
    public static Scancode B { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.B); }

    /// <summary>Gets the <see cref="Scancode"/> for the <kbd>C</kbd> key</summary>
    /// <value>The <see cref="Scancode"/> for the <kbd>C</kbd> key</value>
    public static Scancode C { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.C); }

    /// <summary>Gets the <see cref="Scancode"/> for the <kbd>D</kbd> key</summary>
    /// <value>The <see cref="Scancode"/> for the <kbd>D</kbd> key</value>
    public static Scancode D { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.D); }

    /// <summary>Gets the <see cref="Scancode"/> for the <kbd>E</kbd> key</summary>
    /// <value>The <see cref="Scancode"/> for the <kbd>E</kbd> key</value>
    public static Scancode E { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.E); }

    /// <summary>Gets the <see cref="Scancode"/> for the <kbd>F</kbd> key</summary>
    /// <value>The <see cref="Scancode"/> for the <kbd>F</kbd> key</value>
    public static Scancode F { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.F); }

    /// <summary>Gets the <see cref="Scancode"/> for the <kbd>G</kbd> key</summary>
    /// <value>The <see cref="Scancode"/> for the <kbd>G</kbd> key</value>
    public static Scancode G { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.G); }

    /// <summary>Gets the <see cref="Scancode"/> for the <kbd>H</kbd> key</summary>
    /// <value>The <see cref="Scancode"/> for the <kbd>H</kbd> key</value>
    public static Scancode H { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.H); }

    /// <summary>Gets the <see cref="Scancode"/> for the <kbd>I</kbd> key</summary>
    /// <value>The <see cref="Scancode"/> for the <kbd>I</kbd> key</value>
    public static Scancode I { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.I); }

    /// <summary>Gets the <see cref="Scancode"/> for the <kbd>J</kbd> key</summary>
    /// <value>The <see cref="Scancode"/> for the <kbd>J</kbd> key</value>
    public static Scancode J { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.J); }

    /// <summary>Gets the <see cref="Scancode"/> for the <kbd>K</kbd> key</summary>
    /// <value>The <see cref="Scancode"/> for the <kbd>K</kbd> key</value>
    public static Scancode K { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.K); }

    /// <summary>Gets the <see cref="Scancode"/> for the <kbd>L</kbd> key</summary>
    /// <value>The <see cref="Scancode"/> for the <kbd>L</kbd> key</value>
    public static Scancode L { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.L); }

    /// <summary>Gets the <see cref="Scancode"/> for the <kbd>M</kbd> key</summary>
    /// <value>The <see cref="Scancode"/> for the <kbd>M</kbd> key</value>
    public static Scancode M { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.M); }

    /// <summary>Gets the <see cref="Scancode"/> for the <kbd>N</kbd> key</summary>
    /// <value>The <see cref="Scancode"/> for the <kbd>N</kbd> key</value>
    public static Scancode N { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.N); }

    /// <summary>Gets the <see cref="Scancode"/> for the <kbd>O</kbd> key</summary>
    /// <value>The <see cref="Scancode"/> for the <kbd>O</kbd> key</value>
    public static Scancode O { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.O); }

    /// <summary>Gets the <see cref="Scancode"/> for the <kbd>P</kbd> key</summary>
    /// <value>The <see cref="Scancode"/> for the <kbd>P</kbd> key</value>
    public static Scancode P { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.P); }

    /// <summary>Gets the <see cref="Scancode"/> for the <kbd>Q</kbd> key</summary>
    /// <value>The <see cref="Scancode"/> for the <kbd>Q</kbd> key</value>
    public static Scancode Q { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Q); }

    /// <summary>Gets the <see cref="Scancode"/> for the <kbd>R</kbd> key</summary>
    /// <value>The <see cref="Scancode"/> for the <kbd>R</kbd> key</value>
    public static Scancode R { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.R); }

    /// <summary>Gets the <see cref="Scancode"/> for the <kbd>S</kbd> key</summary>
    /// <value>The <see cref="Scancode"/> for the <kbd>S</kbd> key</value>
    public static Scancode S { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.S); }

    /// <summary>Gets the <see cref="Scancode"/> for the <kbd>T</kbd> key</summary>
    /// <value>The <see cref="Scancode"/> for the <kbd>T</kbd> key</value>
    public static Scancode T { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.T); }

    /// <summary>Gets the <see cref="Scancode"/> for the <kbd>U</kbd> key</summary>
    /// <value>The <see cref="Scancode"/> for the <kbd>U</kbd> key</value>
    public static Scancode U { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.U); }

    /// <summary>Gets the <see cref="Scancode"/> for the <kbd>V</kbd> key</summary>
    /// <value>The <see cref="Scancode"/> for the <kbd>V</kbd> key</value>
    public static Scancode V { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.V); }

    /// <summary>Gets the <see cref="Scancode"/> for the <kbd>W</kbd> key</summary>
    /// <value>The <see cref="Scancode"/> for the <kbd>W</kbd> key</value>
    public static Scancode W { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.W); }

    /// <summary>Gets the <see cref="Scancode"/> for the <kbd>X</kbd> key</summary>
    /// <value>The <see cref="Scancode"/> for the <kbd>X</kbd> key</value>
    public static Scancode X { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.X); }

    /// <summary>Gets the <see cref="Scancode"/> for the <kbd>Y</kbd> key</summary>
    /// <value>The <see cref="Scancode"/> for the <kbd>Y</kbd> key</value>
    public static Scancode Y { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Y); }

    /// <summary>Gets the <see cref="Scancode"/> for the <kbd>Z</kbd> key</summary>
    /// <value>The <see cref="Scancode"/> for the <kbd>Z</kbd> key</value>
    public static Scancode Z { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Z); }

    /// <summary>Gets the <see cref="Scancode"/> for the <kbd>1</kbd> key</summary>
    /// <value>The <see cref="Scancode"/> for the <kbd>1</kbd> key</value>
    public static Scancode _1 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind._1); }

    /// <summary>Gets the <see cref="Scancode"/> for the <kbd>2</kbd> key</summary>
    /// <value>The <see cref="Scancode"/> for the <kbd>2</kbd> key</value>
    public static Scancode _2 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind._2); }

    /// <summary>Gets the <see cref="Scancode"/> for the <kbd>3</kbd> key</summary>
    /// <value>The <see cref="Scancode"/> for the <kbd>3</kbd> key</value>
    public static Scancode _3 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind._3); }

    /// <summary>Gets the <see cref="Scancode"/> for the <kbd>4</kbd> key</summary>
    /// <value>The <see cref="Scancode"/> for the <kbd>4</kbd> key</value>
    public static Scancode _4 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind._4); }

    /// <summary>Gets the <see cref="Scancode"/> for the <kbd>5</kbd> key</summary>
    /// <value>The <see cref="Scancode"/> for the <kbd>5</kbd> key</value>
    public static Scancode _5 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind._5); }

    /// <summary>Gets the <see cref="Scancode"/> for the <kbd>6</kbd> key</summary>
    /// <value>The <see cref="Scancode"/> for the <kbd>6</kbd> key</value>
    public static Scancode _6 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind._6); }

    /// <summary>Gets the <see cref="Scancode"/> for the <kbd>_7</kbd> key</summary>
    /// <value>The <see cref="Scancode"/> for the <kbd>_7</kbd> key</value>
    public static Scancode _7 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind._7); }

    /// <summary>Gets the <see cref="Scancode"/> for the <kbd>8</kbd> key</summary>
    /// <value>The <see cref="Scancode"/> for the <kbd>8</kbd> key</value>
    public static Scancode _8 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind._8); }

    /// <summary>Gets the <see cref="Scancode"/> for the <kbd>9</kbd> key</summary>
    /// <value>The <see cref="Scancode"/> for the <kbd>9</kbd> key</value>
    public static Scancode _9 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind._9); }

    /// <summary>Gets the <see cref="Scancode"/> for the <kbd>0</kbd> key</summary>
    /// <value>The <see cref="Scancode"/> for the <kbd>0</kbd> key</value>
    public static Scancode _0 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind._0); }

    /// <summary>Gets the <see cref="Scancode"/> for the <kbd>⏎</kbd> (return / enter) key</summary>
    /// <value>The <see cref="Scancode"/> for the <kbd>⏎</kbd> (return / enter) key</value>
    public static Scancode Return { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Return); }

    /// <summary>Gets the <see cref="Scancode"/> for the <kbd>ESC</kbd> (escape) key</summary>
    /// <value>The <see cref="Scancode"/> for the <kbd>ESC</kbd> (escape) key</value>
    public static Scancode Escape { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Escape); }

    /// <summary>Gets the <see cref="Scancode"/> for the <kbd>⌫</kbd> (back space) key</summary>
    /// <value>The <see cref="Scancode"/> for the <kbd>⌫</kbd> (back space) key</value>
    public static Scancode Backspace { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Backspace); }

    /// <summary>Gets the <see cref="Scancode"/> for the <kbd>⭾</kbd> (tab) key</summary>
    /// <value>The <see cref="Scancode"/> for the <kbd>⭾</kbd> (tab) key</value>
    public static Scancode Tab { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Tab); }

    /// <summary>Gets the <see cref="Scancode"/> for the <kbd>⎵</kbd> (space) key</summary>
    /// <value>The <see cref="Scancode"/> for the <kbd>⎵</kbd> (space) key</value>
    public static Scancode Space { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Space); }

    /// <summary>Gets the <see cref="Scancode"/> for the <kbd>-</kbd> key</summary>
    /// <value>The <see cref="Scancode"/> for the <kbd>-</kbd> key</value>
    public static Scancode Minus { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Minus); }

    /// <summary>Gets the <see cref="Scancode"/> for the <kbd>=</kbd> key</summary>
    /// <value>The <see cref="Scancode"/> for the <kbd>=</kbd> key</value>
    public static Scancode Equal { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Equal); }

    /// <summary>Gets the <see cref="Scancode"/> for the <kbd>[</kbd> key</summary>
    /// <value>The <see cref="Scancode"/> for the <kbd>[</kbd> key</value>
    public static Scancode LeftBracket { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.LeftBracket); }

    /// <summary>Gets the <see cref="Scancode"/> for the <kbd>]</kbd> key</summary>
    /// <value>The <see cref="Scancode"/> for the <kbd>]</kbd> key</value>
    public static Scancode RightBracket { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.RightBracket); }

    /// <summary>Gets the <see cref="Scancode"/> for the <kbd>\</kbd> key in US layouts</summary>
    /// <value>The <see cref="Scancode"/> for the <kbd>\</kbd> key in US layouts</value>
    /// <remarks>
    /// <para>
    /// This key Located at the lower left of the <kbd>⏎</kbd> (return / enter) key on ISO keyboards and at the right end of the <kbd>Q</kbd><kbd>W</kbd><kbd>E</kbd><kbd>R</kbd><kbd>T</kbd><kbd>Y</kbd> row on ANSI keyboards.
    /// </para>
    /// <para>
    /// This key produces:
    /// <list type="bullet">
    ///     <item><description>'\' (back slash) and '|' (vertical line) in a US layout and in a UK Mac layout</description></item>
    ///     <item><description>'#' (number sign) and '~' (tilde) in a UK Windows layout</description></item>
    ///     <item><description>'$' (dollar sign) and '£' (pound sign) in a Swiss German layout</description></item>
    ///     <item><description>'#' (number sign) and ''' (apostrophe) in a German layout</description></item>
    ///     <item><description>'`' (accent grave) and '£' (pound sign) in a French Mac layout</description></item>
    ///     <item><description>'*' (accent grave) and 'µ' (micro sign) in a French Windows layout</description></item>
    /// </list>
    /// </para>
    /// </remarks>
    public static Scancode Backslash { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Backslash); }

    /// <summary>Gets the alternative <see cref="Scancode"/> that ISO USB keyboards use for <see cref="Backslash"/></summary>
    /// <value>The alternative <see cref="Scancode"/> that ISO USB keyboards use for <see cref="Backslash"/></value>
    /// <remarks>
    /// <para>
    /// All relevant operating systems treat this <see cref="Scancode"/> and <see cref="Backslash"/> identically.
    /// </para>
    /// <para>
    /// You should not generate this <see cref="Scancode"/>, nor should you rely on this <see cref="Scancode"/> getting generated, because SDL will never do that with most keyboards.
    /// </para>
    /// </remarks>
    public static Scancode NonUsHash { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.NonUsHash); }

    /// <summary>Gets the <see cref="Scancode"/> for the <kbd>;</kbd> key</summary>
    /// <value>The <see cref="Scancode"/> for the <kbd>;</kbd> key</value>
    public static Scancode Semicolon { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Semicolon); }

    /// <summary>Gets the <see cref="Scancode"/> for the <kbd>'</kbd> key</summary>
    /// <value>The <see cref="Scancode"/> for the <kbd>'</kbd> key</value>
    public static Scancode Apostrophe { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Apostrophe); }

    /// <summary>Gets the <see cref="Scancode"/> for the <kbd>`</kbd> key in US layouts</summary>
    /// <value>The <see cref="Scancode"/> for the <kbd>`</kbd> key in US layouts</value>
    /// <remarks>
    /// <para>
    /// This key Located at the top left corner.
    /// </para>
    /// <para>
    /// This key produces:
    /// <list type="bullet">
    ///     <item><description>'`' (accent grave) and '~' (tilde) in a US Windows layout and in US and UK Mac layouts on ASNI keyboards</description></item>
    ///     <item><description>'`' (accent grave) and '¬' (not sign) in a UK Windows layout</description></item>
    ///     <item><description>'§' (section sign) and '±' (plus-minus sign) in US and UK Mac layouts on ISO keyboards</description></item>
    ///     <item><description>'§' (section sign) and '°' (degree sign) in a Swiss German layout (for Macs only on ISO keyboards)</description></item>
    ///     <item><description>'^' (accent circumflex) and '°' (degree sign) in a German layout (for Macs only on ISO keyboards)</description></item>
    ///     <item><description>'²' (superscript two) and '~' (tilde) in a French Windows layout</description></item>
    ///     <item><description>'@' (commercial at sign) and '#' (number sign) in a French Mac layout on ISO keyboards</description></item>
    ///     <item><description>'&lt;' (less than sign) and '&gt;' (greater than sign) in a Swiss German, German, or French Mac layout on ANSI keyboards</description></item>
    /// </list>
    /// </para>
    /// </remarks>
    public static Scancode Grave { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Grave); }

    /// <summary>Gets the <see cref="Scancode"/> for the <kbd>,</kbd> key</summary>
    /// <value>The <see cref="Scancode"/> for the <kbd>,</kbd> key</value>
    public static Scancode Comma { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Comma); }

    /// <summary>Gets the <see cref="Scancode"/> for the <kbd>.</kbd> key</summary>
    /// <value>The <see cref="Scancode"/> for the <kbd>.</kbd> key</value>
    public static Scancode Period { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Period); }

    /// <summary>Gets the <see cref="Scancode"/> for the <kbd>/</kbd> key</summary>
    /// <value>The <see cref="Scancode"/> for the <kbd>/</kbd> key</value>
    public static Scancode Slash { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Slash); }

    /// <summary>Gets the <see cref="Scancode"/> for the <kbd>⇪</kbd> (caps lock) key</summary>
    /// <value>The <see cref="Scancode"/> for the <kbd>⇪</kbd> (caps lock) key</value>
    public static Scancode CapsLock { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.CapsLock); }

    /// <summary>Gets the <see cref="Scancode"/> for the <kbd>F1</kbd> key</summary>
    /// <value>The <see cref="Scancode"/> for the <kbd>F1</kbd> key</value>
    public static Scancode F1 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.F1); }

    /// <summary>Gets the <see cref="Scancode"/> for the <kbd>F2</kbd> key</summary>
    /// <value>The <see cref="Scancode"/> for the <kbd>F2</kbd> key</value>
    public static Scancode F2 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.F2); }

    /// <summary>Gets the <see cref="Scancode"/> for the <kbd>F3</kbd> key</summary>
    /// <value>The <see cref="Scancode"/> for the <kbd>F3</kbd> key</value>
    public static Scancode F3 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.F3); }

    /// <summary>Gets the <see cref="Scancode"/> for the <kbd>F4</kbd> key</summary>
    /// <value>The <see cref="Scancode"/> for the <kbd>F4</kbd> key</value>
    public static Scancode F4 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.F4); }

    /// <summary>Gets the <see cref="Scancode"/> for the <kbd>F5</kbd> key</summary>
    /// <value>The <see cref="Scancode"/> for the <kbd>F5</kbd> key</value>
    public static Scancode F5 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.F5); }

    /// <summary>Gets the <see cref="Scancode"/> for the <kbd>F6</kbd> key</summary>
    /// <value>The <see cref="Scancode"/> for the <kbd>F6</kbd> key</value>
    public static Scancode F6 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.F6); }

    /// <summary>Gets the <see cref="Scancode"/> for the <kbd>F7</kbd> key</summary>
    /// <value>The <see cref="Scancode"/> for the <kbd>F7</kbd> key</value>
    public static Scancode F7 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.F7); }

    /// <summary>Gets the <see cref="Scancode"/> for the <kbd>F8</kbd> key</summary>
    /// <value>The <see cref="Scancode"/> for the <kbd>F8</kbd> key</value>
    public static Scancode F8 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.F8); }

    /// <summary>Gets the <see cref="Scancode"/> for the <kbd>F9</kbd> key</summary>
    /// <value>The <see cref="Scancode"/> for the <kbd>F9</kbd> key</value>
    public static Scancode F9 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.F9); }

    /// <summary>Gets the <see cref="Scancode"/> for the <kbd>F10</kbd> key</summary>
    /// <value>The <see cref="Scancode"/> for the <kbd>F10</kbd> key</value>
    public static Scancode F10 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.F10); }

    /// <summary>Gets the <see cref="Scancode"/> for the <kbd>F11</kbd> key</summary>
    /// <value>The <see cref="Scancode"/> for the <kbd>F11</kbd> key</value>
    public static Scancode F11 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.F11); }

    /// <summary>Gets the <see cref="Scancode"/> for the <kbd>F12</kbd> key</summary>
    /// <value>The <see cref="Scancode"/> for the <kbd>F12</kbd> key</value>
    public static Scancode F12 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.F12); }

    /// <summary>Gets the <see cref="Scancode"/> for the <kbd>PRINT SCREEN</kbd> (print screen) key</summary>
    /// <value>The <see cref="Scancode"/> for the <kbd>PRINT SCREEN</kbd> (print screen) key</value>
    public static Scancode PrintScreen { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.PrintScreen); }

    /// <summary>Gets the <see cref="Scancode"/> for the <kbd>SCROLL</kbd> (scroll lock) key</summary>
    /// <value>The <see cref="Scancode"/> for the <kbd>SCROLL</kbd> (scroll lock) key</value>
    public static Scancode ScrollLock { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.ScrollLock); }

    /// <summary>Gets the <see cref="Scancode"/> for the <kbd>PAUSE</kbd> (pause / break) key</summary>
    /// <value>The <see cref="Scancode"/> for the <kbd>PAUSE</kbd> (pause / break) key</value>
    public static Scancode Pause { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Pause); }

    /// <summary>Gets the <see cref="Scancode"/> for the <kbd>INSERT</kbd> (insert) key</summary>
    /// <value>The <see cref="Scancode"/> for the <kbd>INSERT</kbd> (insert) key</value>
    /// <remarks>
    /// <para>
    /// On some Mac keyboards this is actual send as the <see cref="Scancode"/> for the <kbd>HELP</kbd> (help) key instead of <see cref="Help"/>.
    /// </para>
    /// </remarks>
    public static Scancode Insert { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Insert); }

    /// <summary>Gets the <see cref="Scancode"/> for the <kbd>HOME</kbd> (home) key</summary>
    /// <value>The <see cref="Scancode"/> for the <kbd>HOME</kbd> (home) key</value>
    public static Scancode Home { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Home); }

    /// <summary>Gets the <see cref="Scancode"/> for the <kbd>PAGE UP</kbd> (page up) key</summary>
    /// <value>The <see cref="Scancode"/> for the <kbd>PAGE UP</kbd> (page up) key</value>
    public static Scancode PageUp { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.PageUp); }

    /// <summary>Gets the <see cref="Scancode"/> for the <kbd>DELETE</kbd> (delete) key</summary>
    /// <value>The <see cref="Scancode"/> for the <kbd>DELETE</kbd> (delete) key</value>
    public static Scancode Delete { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Delete); }

    /// <summary>Gets the <see cref="Scancode"/> for the <kbd>END</kbd> (end) key</summary>
    /// <value>The <see cref="Scancode"/> for the <kbd>END</kbd> (end) key</value>
    public static Scancode End { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.End); }

    /// <summary>Gets the <see cref="Scancode"/> for the <kbd>PAGE DOWN</kbd> (page down) key</summary>
    /// <value>The <see cref="Scancode"/> for the <kbd>PAGE DOWN</kbd> (page down) key</value>
    public static Scancode PageDown { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.PageDown); }

    /// <summary>Gets the <see cref="Scancode"/> for the <kbd>→</kbd> (right arrow) key</summary>
    /// <value>The <see cref="Scancode"/> for the <kbd>→</kbd> (right arrow) key</value>
    public static Scancode Right { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Right); }

    /// <summary>Gets the <see cref="Scancode"/> for the <kbd>←</kbd> (left arrow) key</summary>
    /// <value>The <see cref="Scancode"/> for the <kbd>←</kbd> (left arrow) key</value>
    public static Scancode Left { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Left); }

    /// <summary>Gets the <see cref="Scancode"/> for the <kbd>↓</kbd> (down arrow) key</summary>
    /// <value>The <see cref="Scancode"/> for the <kbd>↓</kbd> (down arrow) key</value>
    public static Scancode Down { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Down); }

    /// <summary>Gets the <see cref="Scancode"/> for the <kbd>↑</kbd> (up arrow) key</summary>
    /// <value>The <see cref="Scancode"/> for the <kbd>↑</kbd> (up arrow) key</value>
    public static Scancode Up { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Up); }

    /// <summary>Gets the <see cref="Scancode"/> for the <kbd>NUM</kbd> (num lock) key on PCs, or the <kbd>CLEAR</kbd> (clear) key on Macs</summary>
    /// <value>The <see cref="Scancode"/> for the <kbd>NUM</kbd> (num lock) key on PCs, or the <kbd>CLEAR</kbd> (clear) key on Macs</value>
    public static Scancode NumLockOrClear { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.NumLockOrClear); }    

    /// <summary>Gets the <see cref="Scancode"/> for the additional key, located between the left <kbd>⇧</kbd> (shift) key and the <kbd>Y</kbd> key, on ISO keyboards</summary>
    /// <value>The <see cref="Scancode"/> for the additional key, located between the left <kbd>⇧</kbd> (shift) key and the <kbd>Y</kbd> key, on ISO keyboards</value>
    /// <remarks>
    /// <para>
    /// This is the additional key that ISO keyboards have over ANSI ones, located between the left <kbd>⇧</kbd> (shift) key and the <kbd>Y</kbd> key.
    /// </para>
    /// <para>
    /// This key produces:
    /// <list type="bullet">
    ///     <item><description>'`' (accent grave) and '~' (tilde) in a US or UK Mac layout</description></item>
    ///     <item><description>'\' (back slash) and '|' (vertical line) in a US or UK Windows layout</description></item>
    ///     <item><description>'&lt;' (less than sign) and '&gt;' (greater than sign) in a Swiss German, German, or French layout</description></item>
    /// </list>
    /// </para>
    /// </remarks>
    public static Scancode NonUsBackslash { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.NonUsBackslash); }

    /// <summary>Gets the <see cref="Scancode"/> for the <kbd>☰</kbd> (context menu) key on Windows, or the <kbd>COMPOSE</kbd> (compose) key elsewhere</summary>
    /// <value>The <see cref="Scancode"/> for the <kbd>☰</kbd> (context menu) key on Windows, or the <kbd>COMPOSE</kbd> (compose) key elsewhere</value>
    public static Scancode Application { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Application); }

    /// <summary>Gets zhe <see cref="Scancode"/> that <em>might be</em> for the <kbd>POWER</kbd> (power) key on some Mac keyboards</summary>
    /// <value>The <see cref="Scancode"/> that <em>might be</em> for the <kbd>POWER</kbd> (power) key on some Mac keyboards</value>
    /// <remarks>
    /// <para>
    /// Do <em>not</em> rely on this, as the USB document says that this is a status flag instead of a physical key
    /// </para>
    /// </remarks>
    [Experimental(diagnosticId: "SDL5010")] //TODO: make SDL5010 the diagnostics id for unknown or uncertain Keycodes or Scancodes
    public static Scancode Power { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Power); }

    /// <summary>Gets the <see cref="Scancode"/> for the <kbd>F13</kbd> key</summary>
    /// <value>The <see cref="Scancode"/> for the <kbd>F13</kbd> key</value>
    public static Scancode F13 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.F13); }

    /// <summary>Gets the <see cref="Scancode"/> for the <kbd>F14</kbd> key</summary>
    /// <value>The <see cref="Scancode"/> for the <kbd>F14</kbd> key</value>
    public static Scancode F14 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.F14); }

    /// <summary>Gets the <see cref="Scancode"/> for the <kbd>F15</kbd> key</summary>
    /// <value>The <see cref="Scancode"/> for the <kbd>F15</kbd> key</value>
    public static Scancode F15 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.F15); }

    /// <summary>Gets the <see cref="Scancode"/> for the <kbd>F16</kbd> key</summary>
    /// <value>The <see cref="Scancode"/> for the <kbd>F16</kbd> key</value>
    public static Scancode F16 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.F16); }

    /// <summary>Gets the <see cref="Scancode"/> for the <kbd>F17</kbd> key</summary>
    /// <value>The <see cref="Scancode"/> for the <kbd>F17</kbd> key</value>
    public static Scancode F17 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.F17); }

    /// <summary>Gets the <see cref="Scancode"/> for the <kbd>F18</kbd> key</summary>
    /// <value>The <see cref="Scancode"/> for the <kbd>F18</kbd> key</value>
    public static Scancode F18 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.F18); }

    /// <summary>Gets the <see cref="Scancode"/> for the <kbd>F19</kbd> key</summary>
    /// <value>The <see cref="Scancode"/> for the <kbd>F19</kbd> key</value>
    public static Scancode F19 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.F19); }

    /// <summary>Gets the <see cref="Scancode"/> for the <kbd>F20</kbd> key</summary>
    /// <value>The <see cref="Scancode"/> for the <kbd>F20</kbd> key</value>
    public static Scancode F20 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.F20); }

    /// <summary>Gets the <see cref="Scancode"/> for the <kbd>F21</kbd> key</summary>
    /// <value>The <see cref="Scancode"/> for the <kbd>F21</kbd> key</value>
    public static Scancode F21 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.F21); }

    /// <summary>Gets the <see cref="Scancode"/> for the <kbd>F22</kbd> key</summary>
    /// <value>The <see cref="Scancode"/> for the <kbd>F22</kbd> key</value>
    public static Scancode F22 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.F22); }

    /// <summary>Gets the <see cref="Scancode"/> for the <kbd>F23</kbd> key</summary>
    /// <value>The <see cref="Scancode"/> for the <kbd>F23</kbd> key</value>
    public static Scancode F23 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.F23); }

    /// <summary>Gets the <see cref="Scancode"/> for the <kbd>F24</kbd> key</summary>
    /// <value>The <see cref="Scancode"/> for the <kbd>F24</kbd> key</value>
    public static Scancode F24 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.F24); }

    /// <summary>Gets the <see cref="Scancode"/> for the <kbd>EXECUTE</kbd> (execute) key</summary>
    /// <value>The <see cref="Scancode"/> for the <kbd>EXECUTE</kbd> (execute) key</value>
    public static Scancode Execute { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Execute); }

    /// <summary>Gets the <see cref="Scancode"/> for the <kbd>HELP</kbd> (help) key</summary>
    /// <value>The <see cref="Scancode"/> for the <kbd>HELP</kbd> (help) key</value>
    public static Scancode Help { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Help); }

    /// <summary>Gets the <see cref="Scancode"/> for the <kbd>☰</kbd> (menu / show menu) key</summary>
    /// <value>The <see cref="Scancode"/> for the <kbd>☰</kbd> (menu / show menu) key</value>
    public static Scancode Menu { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Menu); }

    /// <summary>Gets the <see cref="Scancode"/> for the <kbd>SELECT</kbd> (select) key</summary>
    /// <value>The <see cref="Scancode"/> for the <kbd>SELECT</kbd> (select) key</value>
    public static Scancode Select { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Select); }

    /// <summary>Gets the <see cref="Scancode"/> for the <kbd>STOP</kbd> (stop) key</summary>
    /// <value>The <see cref="Scancode"/> for the <kbd>STOP</kbd> (stop) key</value>
    public static Scancode Stop { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Stop); }

    /// <summary>Gets the <see cref="Scancode"/> for the <kbd>AGAIN</kbd> (again / redo) key</summary>
    /// <value>The <see cref="Scancode"/> for the <kbd>AGAIN</kbd> (again / redo) key</value>
    public static Scancode Again { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Again); }

    /// <summary>Gets the <see cref="Scancode"/> for the <kbd>UNDO</kbd> (undo) key</summary>
    /// <value>The <see cref="Scancode"/> for the <kbd>UNDO</kbd> (undo) key</value>
    public static Scancode Undo { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Undo); }

    /// <summary>Gets the <see cref="Scancode"/> for the <kbd>CUT</kbd> (cut) key</summary>
    /// <value>The <see cref="Scancode"/> for the <kbd>CUT</kbd> (cut) key</value>
    public static Scancode Cut { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Cut); }

    /// <summary>Gets the <see cref="Scancode"/> for the <kbd>COPY</kbd> (copy) key</summary>
    /// <value>The <see cref="Scancode"/> for the <kbd>COPY</kbd> (copy) key</value>
    public static Scancode Copy { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Copy); }

    /// <summary>Gets the <see cref="Scancode"/> for the <kbd>PASTE</kbd> (paste) key</summary>
    /// <value>The <see cref="Scancode"/> for the <kbd>PASTE</kbd> (paste) key</value>
    public static Scancode Paste { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Paste); }

    /// <summary>Gets the <see cref="Scancode"/> for the <kbd>🔍</kbd> (find) key</summary>
    /// <value>The <see cref="Scancode"/> for the <kbd>🔍</kbd> (find) key</value>
    public static Scancode Find { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Find); }

    /// <summary>Gets the <see cref="Scancode"/> for the <kbd>🔇</kbd> (mute volume) key</summary>
    /// <value>The <see cref="Scancode"/> for the <kbd>🔇</kbd> (mute volume) key</value>
    public static Scancode Mute { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Mute); }

    /// <summary>Gets the <see cref="Scancode"/> for the <kbd>🔊</kbd> (volume up) key</summary>
    /// <value>The <see cref="Scancode"/> for the <kbd>🔊</kbd> (volume up) key</value>
    public static Scancode VolumeUp { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.VolumeUp); }

    /// <summary>Gets the <see cref="Scancode"/> for the <kbd>🔉</kbd> (volume down) key</summary>
    /// <value>The <see cref="Scancode"/> for the <kbd>🔉</kbd> (volume down) key</value>
    public static Scancode VolumeDown { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.VolumeDown); } 

    /* The original source (https://github.com/libsdl-org/SDL/blob/main/include/SDL3/SDL_scancode.h#L250) is not sure whether to enable these or not
     *  LockingCapsLock
     *  LockingCapsLock
     *  LockingScrollLock
     */

// TODO: document the following Scancodes or not?
#pragma warning disable CS1591

    /// <remarks>Used on Asian keyboards, see footnotes in USB keyboard documentation</remarks>
    public static Scancode International1 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.International1); }

    public static Scancode International2 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.International2); }

    /// <remarks>Yen</remarks>
    public static Scancode International3 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.International3); }

    public static Scancode International4 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.International4); }

    public static Scancode International5 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.International5); }

    public static Scancode International6 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.International6); }

    public static Scancode International7 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.International7); }

    public static Scancode International8 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.International8); }

    public static Scancode International9 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.International9); }

    /// <remarks>Hangul/English toggle</remarks>
    public static Scancode Language1 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Language1); }

    /// <remarks>Hanja conversion</remarks>
    public static Scancode Language2 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Language2); }

    /// <remarks>Katakana</remarks>
    public static Scancode Language3 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Language3); }

    /// <remarks>Hiragana</remarks>
    public static Scancode Language4 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Language4); }

    /// <remarks>Zenkaku/Hankaku</remarks>
    public static Scancode Language5 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Language5); }

    [Experimental(diagnosticId: "SDL5011")] // TODO: make SDL5011 the diagnostics id for 'reserved scancodes - do not use'
    public static Scancode Language6 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Language6); }

    [Experimental(diagnosticId: "SDL5011")] // TODO: make SDL5011 the diagnostics id for 'reserved scancodes - do not use'
    public static Scancode Language7 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Language7); }

    [Experimental(diagnosticId: "SDL5011")] // TODO: make SDL5011 the diagnostics id for 'reserved scancodes - do not use'
    public static Scancode Language8 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Language8); }

    [Experimental(diagnosticId: "SDL5011")] // TODO: make SDL5011 the diagnostics id for 'reserved scancodes - do not use'
    public static Scancode Language9 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Language9); }

#pragma warning restore CS1591

// TODO: document the following Scancodes!
#pragma warning disable CS1591

    public static Scancode AltErase { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.AltErase); }

    public static Scancode SysReq { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.SysReq); }

    public static Scancode Cancel { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Cancel); }

    public static Scancode Clear { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Clear); }

    public static Scancode Prior { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Prior); }

    public static Scancode Return2 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Return2); }

    public static Scancode Separator { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Separator); }

    public static Scancode Out { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Out); }

    public static Scancode Oper { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Oper); }

    public static Scancode ClearAgain { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.ClearAgain); }

    public static Scancode CrSel { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.CrSel); }

    public static Scancode ExSel { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.ExSel); }

    public static Scancode ThousandsSeparator { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.ThousandsSeparator); }

    public static Scancode DecimalSeparator { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.DecimalSeparator); }

    public static Scancode CurrencyUnit { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.CurrencyUnit); }

    public static Scancode CurrencySubunit { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.CurrencySubunit); }

#pragma warning restore CS1591
    
    /// <summary>Gets the <see cref="Scancode"/> for the left <kbd>CTRL</kbd> (left control) key</summary>
    /// <value>The <see cref="Scancode"/> for the left <kbd>CTRL</kbd> (left control) key</value>
    public static Scancode LeftControl { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.LeftControl); }

    /// <summary>Gets the <see cref="Scancode"/> for the left <kbd>⇧</kbd> (left shift) key</summary>
    /// <value>The <see cref="Scancode"/> for the left <kbd>⇧</kbd> (left shift) key</value>
    public static Scancode LeftShift { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.LeftShift); }

    /// <summary>Gets the <see cref="Scancode"/> for the left <kbd>ALT</kbd> (left alt) key</summary>
    /// <value>The <see cref="Scancode"/> for the left <kbd>ALT</kbd> (left alt) key</value>
    public static Scancode LeftAlt { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.LeftAlt); }

    /// <summary>Gets the <see cref="Scancode"/> for the left <kbd>⊞</kbd> (left Windows) key on PCs, left <kbd>⌘</kbd> (left command) key on Macs, or sometimes called left meta key</summary>
    /// <value>The <see cref="Scancode"/> for the left <kbd>⊞</kbd> (left Windows) key on PCs, left <kbd>⌘</kbd> (left command) key on Macs, or sometimes called left meta key</value>
    public static Scancode LeftGui { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.LeftGui); }

    /// <summary>Gets the <see cref="Scancode"/> for the right <kbd>CTRL</kbd> (right control) key</summary>
    /// <value>The <see cref="Scancode"/> for the right <kbd>CTRL</kbd> key (right control) key</value>
    public static Scancode RightControl { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.RightControl); }

    /// <summary>Gets the <see cref="Scancode"/> for the right <kbd>⇧</kbd> (right shift) key</summary>
    /// <value>The <see cref="Scancode"/> for the right <kbd>⇧</kbd> (right shift) key</value>
    public static Scancode RightShift { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.RightShift); }

    /// <summary>Gets the <see cref="Scancode"/> for the right <kbd>ALT</kbd> (right alt) key, or sometimes called <kbd>ALT GR</kbd> (alt gr.) key</summary>
    /// <value>The <see cref="Scancode"/> for the right <kbd>ALT</kbd> (right alt) key, or sometimes called <kbd>ALT GR</kbd> (alt gr.) key</value>
    public static Scancode RightAlt { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.RightAlt); }

    /// <summary>Gets the <see cref="Scancode"/> for the right <kbd>⊞</kbd> (right Windows) key on PCs, right <kbd>⌘</kbd> (right command) key on Macs, or sometimes called right meta key</summary>
    /// <value>The <see cref="Scancode"/> for the right <kbd>⊞</kbd> (right Windows) key on PCs, right <kbd>⌘</kbd> (right command) key on Macs, or sometimes called right meta key</value>
    public static Scancode RightGui { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.RightGui); }

// TODO: document the following Scancodes!
#pragma warning disable CS1591

    // the original source (https://github.com/libsdl-org/SDL/blob/main/include/SDL3/SDL_scancode.h#L346) is not sure if adding this is necessary
    public static Scancode Mode { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Mode); }

    public static Scancode Sleep { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Sleep); }

    public static Scancode Wake { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Wake); }

    public static Scancode ChannelIncrement { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.ChannelIncrement); }

    public static Scancode ChannelDecrement { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.ChannelDecrement); }
    
#pragma warning restore CS1591

    /// <summary>Provides predefined <see cref="Scancode"/>s for keys on the keypad</summary>
    public static class Keypad
    {
        /// <summary>Gets the <see cref="Scancode"/> for the <kbd>/</kbd> (divide) key on the keypad</summary>
        /// <value>The <see cref="Scancode"/> for the <kbd>/</kbd> (divide) key on the keypad</value>
        public static Scancode Divide { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.KeypadDivide); }

        /// <summary>Gets the <see cref="Scancode"/> for the <kbd>*</kbd> (multiply) key on the keypad</summary>
        /// <value>The <see cref="Scancode"/> for the <kbd>*</kbd> (multiply) key on the keypad</value>
        public static Scancode Multiply { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.KeypadMultiply); }

        /// <summary>Gets the <see cref="Scancode"/> for the <kbd>-</kbd> (minus) key on the keypad</summary>
        /// <value>The <see cref="Scancode"/> for the <kbd>-</kbd> (minus) key on the keypad</value>
        public static Scancode Minus { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.KeypadMinus); }

        /// <summary>Gets the <see cref="Scancode"/> for the <kbd>+</kbd> (divide) key on the keypad</summary>
        /// <value>The <see cref="Scancode"/> for the <kbd>+</kbd> (divide) key on the keypad</value>
        public static Scancode Plus { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.KeypadPlus); }

        /// <summary>Gets the <see cref="Scancode"/> for the <kbd>⏎</kbd> (return / enter) key on the keypad</summary>
        /// <value>The <see cref="Scancode"/> for the <kbd>⏎</kbd> (return / enter) key on the keypad</value>
        public static Scancode Enter { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.KeypadEnter); }

        /// <summary>Gets the <see cref="Scancode"/> for the <kbd>1</kbd> key on the keypad</summary>
        /// <value>The <see cref="Scancode"/> for the <kbd>1</kbd> key on the keypad</value>
        public static Scancode _1 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Keypad1); }

        /// <summary>Gets the <see cref="Scancode"/> for the <kbd>2</kbd> key on the keypad</summary>
        /// <value>The <see cref="Scancode"/> for the <kbd>2</kbd> key on the keypad</value>
        public static Scancode _2 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Keypad2); }

        /// <summary>Gets the <see cref="Scancode"/> for the <kbd>3</kbd> key on the keypad</summary>
        /// <value>The <see cref="Scancode"/> for the <kbd>3</kbd> key on the keypad</value>
        public static Scancode _3 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Keypad3); }

        /// <summary>Gets the <see cref="Scancode"/> for the <kbd>4</kbd> key on the keypad</summary>
        /// <value>The <see cref="Scancode"/> for the <kbd>4</kbd> key on the keypad</value>
        public static Scancode _4 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Keypad4); }

        /// <summary>Gets the <see cref="Scancode"/> for the <kbd>5</kbd> key on the keypad</summary>
        /// <value>The <see cref="Scancode"/> for the <kbd>5</kbd> key on the keypad</value>
        public static Scancode _5 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Keypad5); }

        /// <summary>Gets the <see cref="Scancode"/> for the <kbd>6</kbd> key on the keypad</summary>
        /// <value>The <see cref="Scancode"/> for the <kbd>6</kbd> key on the keypad</value>
        public static Scancode _6 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Keypad6); }

        /// <summary>Gets the <see cref="Scancode"/> for the <kbd>7</kbd> key on the keypad</summary>
        /// <value>The <see cref="Scancode"/> for the <kbd>7</kbd> key on the keypad</value>
        public static Scancode _7 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Keypad7); }

        /// <summary>Gets the <see cref="Scancode"/> for the <kbd>8</kbd> key on the keypad</summary>
        /// <value>The <see cref="Scancode"/> for the <kbd>8</kbd> key on the keypad</value>
        public static Scancode _8 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Keypad8); }

        /// <summary>Gets the <see cref="Scancode"/> for the <kbd>9</kbd> key on the keypad</summary>
        /// <value>The <see cref="Scancode"/> for the <kbd>9</kbd> key on the keypad</value>
        public static Scancode _9 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Keypad9); }

        /// <summary>Gets the <see cref="Scancode"/> for the <kbd>0</kbd> key on the keypad</summary>
        /// <value>The <see cref="Scancode"/> for the <kbd>0</kbd> key on the keypad</value>
        public static Scancode _0 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Keypad0); }

        /// <summary>Gets the <see cref="Scancode"/> for the <kbd>.</kbd> key on the keypad</summary>
        /// <value>The <see cref="Scancode"/> for the <kbd>.</kbd> key on the keypad</value>
        public static Scancode Period { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.KeypadPeriod); }

        /// <summary>Gets the <see cref="Scancode"/> for the <kbd>=</kbd> key</summary>
        /// <value>The <see cref="Scancode"/> for the <kbd>=</kbd> key</value>
        public static Scancode Equal { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.KeypadEqual); }

        /// <summary>Gets the <see cref="Scancode"/> for the <kbd>,</kbd> key on the keypad</summary>
        /// <value>The <see cref="Scancode"/> for the <kbd>,</kbd> key on the keypad</value>
        public static Scancode Comma { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.KeypadComma); }

        /// <summary>Gets the <see cref="Scancode"/> for the <kbd>=</kbd> AS400 key on the keypad</summary>
        /// <value>The <see cref="Scancode"/> for the <kbd>=</kbd> AS400 key on the keypad</value>
        public static Scancode EqualAS400 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.KeypadEqualAS400); }

// TODO: document the following Scancodes!
#pragma warning disable CS1591

        public static Scancode _00 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Keypad00); }

        public static Scancode _000 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Keypad000); }

#pragma warning restore CS1591        

        /// <summary>Gets the <see cref="Scancode"/> for the <kbd>(</kbd> key on the keypad</summary>
        /// <value>The <see cref="Scancode"/> for the <kbd>(</kbd> key on the keypad</value>
        public static Scancode LeftParenthesis { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.KeypadLeftParenthesis); }

        /// <summary>Gets the <see cref="Scancode"/> for the <kbd>)</kbd> key on the keypad</summary>
        /// <value>The <see cref="Scancode"/> for the <kbd>)</kbd> key on the keypad</value>
        public static Scancode RightParenthesis { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.KeypadRightParenthesis); }

        /// <summary>Gets the <see cref="Scancode"/> for the <kbd>{</kbd> key on the keypad</summary>
        /// <value>The <see cref="Scancode"/> for the <kbd>{</kbd> key on the keypad</value>
        public static Scancode LeftBrace { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.KeypadLeftBrace); }

        /// <summary>Gets the <see cref="Scancode"/> for the <kbd>}</kbd> key on the keypad</summary>
        /// <value>The <see cref="Scancode"/> for the <kbd>}</kbd> key on the keypad</value>
        public static Scancode RightBrace { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.KeypadRightBrace); }

        /// <summary>Gets the <see cref="Scancode"/> for the <kbd>⭾</kbd> (tab) key on the keypad</summary>
        /// <value>The <see cref="Scancode"/> for the <kbd>⭾</kbd> (tab) key on the keypad</value>
        public static Scancode Tab { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.KeypadTab); }

        /// <summary>Gets the <see cref="Scancode"/> for the <kbd>⌫</kbd> (back space) key on the keypad</summary>
        /// <value>The <see cref="Scancode"/> for the <kbd>⌫</kbd> (back space) key on the keypad</value>
        public static Scancode Backspace { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.KeypadBackspace); }

        /// <summary>Gets the <see cref="Scancode"/> for the <kbd>A</kbd> key on the keypad</summary>
        /// <value>The <see cref="Scancode"/> for the <kbd>A</kbd> key on the keypad</value>
        public static Scancode A { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.KeypadA); }

        /// <summary>Gets the <see cref="Scancode"/> for the <kbd>B</kbd> key on the keypad</summary>
        /// <value>The <see cref="Scancode"/> for the <kbd>B</kbd> key on the keypad</value>
        public static Scancode B { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.KeypadB); }

        /// <summary>Gets the <see cref="Scancode"/> for the <kbd>C</kbd> key on the keypad</summary>
        /// <value>The <see cref="Scancode"/> for the <kbd>C</kbd> key on the keypad</value>
        public static Scancode C { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.KeypadC); }

        /// <summary>Gets the <see cref="Scancode"/> for the <kbd>D</kbd> key on the keypad</summary>
        /// <value>The <see cref="Scancode"/> for the <kbd>D</kbd> key on the keypad</value>
        public static Scancode D { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.KeypadD); }

        /// <summary>Gets the <see cref="Scancode"/> for the <kbd>E</kbd> key on the keypad</summary>
        /// <value>The <see cref="Scancode"/> for the <kbd>E</kbd> key on the keypad</value>
        public static Scancode E { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.KeypadE); }

        /// <summary>Gets the <see cref="Scancode"/> for the <kbd>F</kbd> key on the keypad</summary>
        /// <value>The <see cref="Scancode"/> for the <kbd>F</kbd> key on the keypad</value>
        public static Scancode F { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.KeypadF); }

        /// <summary>Gets the <see cref="Scancode"/> for the <kbd>XOR</kbd> (xor; sometimes '^') key on the keypad</summary>
        /// <value>The <see cref="Scancode"/> for the <kbd>XOR</kbd> (xor; sometimes '^') key on the keypad</value>
        public static Scancode Xor { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.KeypadXor); }

        /// <summary>Gets the <see cref="Scancode"/> for the <kbd>xʸ</kbd> (power / exponentiation; sometimes '^') key on the keypad</summary>
        /// <value>The <see cref="Scancode"/> for the <kbd>xʸ</kbd> (power / exponentiation; sometimes '^') key on the keypad</value>
        public static Scancode Power { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.KeypadPower); }

        /// <summary>Gets the <see cref="Scancode"/> for the <kbd>%</kbd> key on the keypad</summary>
        /// <value>The <see cref="Scancode"/> for the <kbd>%</kbd> key on the keypad</value>
        public static Scancode Percent { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.KeypadPercent); }

        /// <summary>Gets the <see cref="Scancode"/> for the <kbd>&lt;</kbd> key on the keypad</summary>
        /// <value>The <see cref="Scancode"/> for the <kbd>&lt;</kbd> key on the keypad</value>
        public static Scancode Less { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.KeypadLess); }

        /// <summary>Gets the <see cref="Scancode"/> for the <kbd>&gt;</kbd> key on the keypad</summary>
        /// <value>The <see cref="Scancode"/> for the <kbd>&gt;</kbd> key on the keypad</value>
        public static Scancode Greater { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.KeypadGreater); }

        /// <summary>Gets the <see cref="Scancode"/> for the <kbd>&amp;</kbd> (and; sometimes 'AND') key on the keypad</summary>
        /// <value>The <see cref="Scancode"/> for the <kbd>&amp;</kbd> (and; sometimes 'AND') key on the keypad</value>
        public static Scancode Ampersand { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.KeypadAmpersand); }

        /// <summary>Gets the <see cref="Scancode"/> for the <kbd>&amp;&amp;</kbd> key on the keypad</summary>
        /// <value>The <see cref="Scancode"/> for the <kbd>&amp;&amp;</kbd> key on the keypad</value>
        public static Scancode DoubleAmpersand { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.KeypadDoubleAmpersand); }

        /// <summary>Gets the <see cref="Scancode"/> for the <kbd>|</kbd> (or; sometimes 'OR') key on the keypad</summary>
        /// <value>The <see cref="Scancode"/> for the <kbd>|</kbd> (or; sometimes 'OR') key on the keypad</value>
        public static Scancode VerticalBar { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.KeypadVerticalBar); }

        /// <summary>Gets the <see cref="Scancode"/> for the <kbd>||</kbd> key on the keypad</summary>
        /// <value>The <see cref="Scancode"/> for the <kbd>||</kbd> key on the keypad</value>
        public static Scancode DoubleVerticalBar { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.KeypadDoubleVerticalBar); }

        /// <summary>Gets the <see cref="Scancode"/> for the <kbd>:</kbd> key on the keypad</summary>
        /// <value>The <see cref="Scancode"/> for the <kbd>:</kbd> key on the keypad</value>
        public static Scancode Colon { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.KeypadColon); }

        /// <summary>Gets the <see cref="Scancode"/> for the <kbd>#</kbd> key on the keypad</summary>
        /// <value>The <see cref="Scancode"/> for the <kbd>#</kbd> key on the keypad</value>
        public static Scancode Hash { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.KeypadHash); }

        /// <summary>Gets the <see cref="Scancode"/> for the <kbd>⎵</kbd> (space) key on the keypad</summary>
        /// <value>The <see cref="Scancode"/> for the <kbd>⎵</kbd> (space) key on the keypad</value>
        public static Scancode Space { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.KeypadSpace); }

        /// <summary>Gets the <see cref="Scancode"/> for the <kbd>@</kbd> key on the keypad</summary>
        /// <value>The <see cref="Scancode"/> for the <kbd>@</kbd> key on the keypad</value>
        public static Scancode At { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.KeypadAt); }

        /// <summary>Gets the <see cref="Scancode"/> for the <kbd>!</kbd> key on the keypad</summary>
        /// <value>The <see cref="Scancode"/> for the <kbd>!</kbd> key on the keypad</value>
        public static Scancode ExclamationMark { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.KeypadExclamationMark); }

        /// <summary>Gets the <see cref="Scancode"/> for the <kbd>MEM STORE</kbd> (mem store) key on the keypad</summary>
        /// <value>The <see cref="Scancode"/> for the <kbd>MEM STORE</kbd> (mem store) key on the keypad</value>
        public static Scancode MemStore { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.KeypadMemStore); }

        /// <summary>Gets the <see cref="Scancode"/> for the <kbd>MEM RECALL</kbd> (mem recall) key on the keypad</summary>
        /// <value>The <see cref="Scancode"/> for the <kbd>MEM RECALL</kbd> (mem recall) key on the keypad</value>
        public static Scancode MemRecall { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.KeypadMemRecall); }

        /// <summary>Gets the <see cref="Scancode"/> for the <kbd>MEM CLEAR</kbd> (mem clear) key on the keypad</summary>
        /// <value>The <see cref="Scancode"/> for the <kbd>MEM CLEAR</kbd> (mem clear) key on the keypad</value>
        public static Scancode MemClear { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.KeypadMemClear); }

        /// <summary>Gets the <see cref="Scancode"/> for the <kbd>MEM +</kbd> (mem add) key on the keypad</summary>
        /// <value>The <see cref="Scancode"/> for the <kbd>MEM +</kbd> (mem add) key on the keypad</value>
        public static Scancode MemAdd { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.KeypadMemAdd); }

        /// <summary>Gets the <see cref="Scancode"/> for the <kbd>MEM -</kbd> (mem subtract) key on the keypad</summary>
        /// <value>The <see cref="Scancode"/> for the <kbd>MEM -</kbd> (mem subtract) key on the keypad</value>
        public static Scancode MemSubtract { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.KeypadMemSubtract); }

        /// <summary>Gets the <see cref="Scancode"/> for the <kbd>MEM *</kbd> (mem multiply) key on the keypad</summary>
        /// <value>The <see cref="Scancode"/> for the <kbd>MEM *</kbd> (mem multiply) key on the keypad</value>
        public static Scancode MemMultiply { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.KeypadMemMultiply); }

        /// <summary>Gets the <see cref="Scancode"/> for the <kbd>MEM /</kbd> (mem divide) key on the keypad</summary>
        /// <value>The <see cref="Scancode"/> for the <kbd>MEM /</kbd> (mem divide) key on the keypad</value>
        public static Scancode MemDivide { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.KeypadMemDivide); }

        /// <summary>Gets the <see cref="Scancode"/> for the <kbd>±</kbd> (plus-minus) key on the keypad</summary>
        /// <value>The <see cref="Scancode"/> for the <kbd>±</kbd> (plus-minus) key on the keypad</value>
        public static Scancode PlusMinus { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.KeypadPlusMinus); }

        /// <summary>Gets the <see cref="Scancode"/> for the <kbd>CLEAR</kbd> (clear) key on the keypad</summary>
        /// <value>The <see cref="Scancode"/> for the <kbd>CLEAR</kbd> (clear) key on the keypad</value>
        public static Scancode Clear { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.KeypadClear); }

// TODO: document the following Scancodes!
#pragma warning disable CS1591

        public static Scancode ClearEntry { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.KeypadClearEntry); }

        public static Scancode Binary { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.KeypadBinary); }
    
        public static Scancode Octal { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.KeypadOctal); }
    
        public static Scancode Decimal { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.KeypadDecimal); }
    
        public static Scancode Hexadecimal { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.KeypadHexadecimal); }
    
#pragma warning restore CS1591
    }

    /// <summary>Provides predefined <see cref="Scancode"/>s for media control keys</summary>
    public static class Media
    {
        /// <summary>Gets the <see cref="Scancode"/> for the <kbd>⏵</kbd> (media play) key</summary>
        /// <value>The <see cref="Scancode"/> for the <kbd>⏵</kbd> (media play) key</value>
        public static Scancode Play { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.MediaPlay); }

        /// <summary>Gets the <see cref="Scancode"/> for the <kbd>⏸</kbd> (media pause) key</summary>
        /// <value>The <see cref="Scancode"/> for the <kbd>⏸</kbd> (media pause) key</value>
        public static Scancode Pause { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.MediaPause); }

        /// <summary>Gets the <see cref="Scancode"/> for the <kbd>⏺</kbd> (media record) key</summary>
        /// <value>The <see cref="Scancode"/> for the <kbd>⏺</kbd> (media record) key</value>
        public static Scancode Record { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.MediaRecord); }

        /// <summary>Gets the <see cref="Scancode"/> for the <kbd>⏩</kbd> (media fast forward) key</summary>
        /// <value>The <see cref="Scancode"/> for the <kbd>⏩</kbd> (media fast forward) key</value>
        public static Scancode FastForward { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.MediaFastForward); }

        /// <summary>Gets the <see cref="Scancode"/> for the <kbd>⏪</kbd> (media rewind) key</summary>
        /// <value>The <see cref="Scancode"/> for the <kbd>⏪</kbd> (media rewind) key</value>
        public static Scancode Rewind { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.MediaRewind); }

        /// <summary>Gets the <see cref="Scancode"/> for the <kbd>⏭</kbd> (media next track) key</summary>
        /// <value>The <see cref="Scancode"/> for the <kbd>⏭</kbd> (media next track) key</value>
        public static Scancode NextTrack { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.MediaNextTrack); }

        /// <summary>Gets the <see cref="Scancode"/> for the <kbd>⏮</kbd> (media previous track) key</summary>
        /// <value>The <see cref="Scancode"/> for the <kbd>⏮</kbd> (media previous track) key</value>
        public static Scancode PreviousTrack { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.MediaPreviousTrack); }

        /// <summary>Gets the <see cref="Scancode"/> for the <kbd>⏹</kbd> (media stop) key</summary>
        /// <value>The <see cref="Scancode"/> for the <kbd>⏹</kbd> (media stop) key</value>
        public static Scancode Stop { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.MediaStop); }

        /// <summary>Gets the <see cref="Scancode"/> for the <kbd>⏏</kbd> (media eject) key</summary>
        /// <value>The <see cref="Scancode"/> for the <kbd>⏏</kbd> (media eject) key</value>
        public static Scancode Eject { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.MediaEject); }

        /// <summary>Gets the <see cref="Scancode"/> for the <kbd>⏯</kbd> (play-pause eject) key</summary>
        /// <value>The <see cref="Scancode"/> for the <kbd>⏯</kbd> (play-pause eject) key</value>
        public static Scancode PlayPause { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.MediaPlayPause); }

// TODO: document the following Scancodes!
#pragma warning disable CS1591

        public static Scancode Select { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.MediaSelect); }

#pragma warning restore CS1591
    }

    /// <summary>Provides predefined <see cref="Scancode"/>s for application control keys</summary>
    public static class ApplicationControl
    {
        /// <summary>Gets the <see cref="Scancode"/> for the application control "<em>New</em>" key</summary>
        /// <value>The <see cref="Scancode"/> for the application control "<em>New</em>" key</value>
        public static Scancode New { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.ApplicationControlNew); }

        /// <summary>Gets the <see cref="Scancode"/> for the application control "<em>Open</em>" key</summary>
        /// <value>The <see cref="Scancode"/> for the application control "<em>Open</em>" key</value>
        public static Scancode Open { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.ApplicationControlOpen); }

        /// <summary>Gets the <see cref="Scancode"/> for the application control "<em>Close</em>" key</summary>
        /// <value>The <see cref="Scancode"/> for the application control "<em>Close</em>" key</value>
        public static Scancode Close { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.ApplicationControlClose); }

        /// <summary>Gets the <see cref="Scancode"/> for the application control "<em>Exit</em>" key</summary>
        /// <value>The <see cref="Scancode"/> for the application control "<em>Exit</em>" key</value>
        public static Scancode Exit { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.ApplicationControlExit); }

        /// <summary>Gets the <see cref="Scancode"/> for the application control "<em>Save</em>" key</summary>
        /// <value>The <see cref="Scancode"/> for the application control "<em>Save</em>" key</value>
        public static Scancode Save { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.ApplicationControlSave); }

        /// <summary>Gets the <see cref="Scancode"/> for the application control "<em>Print</em>" key</summary>
        /// <value>The <see cref="Scancode"/> for the application control "<em>Print</em>" key</value>
        public static Scancode Print { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.ApplicationControlPrint); }

        /// <summary>Gets the <see cref="Scancode"/> for the application control "<em>Properties</em>" key</summary>
        /// <value>The <see cref="Scancode"/> for the application control "<em>Properties</em>" key</value>
        public static Scancode Properties { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.ApplicationControlProperties); }

        /// <summary>Gets the <see cref="Scancode"/> for the application control "<em>Search</em>" key</summary>
        /// <value>The <see cref="Scancode"/> for the application control "<em>Search</em>" key</value>
        public static Scancode Search { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.ApplicationControlSearch); }

        /// <summary>Gets the <see cref="Scancode"/> for the application control "<em>Home</em>" key</summary>
        /// <value>The <see cref="Scancode"/> for the application control "<em>Home</em>" key</value>
        public static Scancode Home { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.ApplicationControlHome); }

        /// <summary>Gets the <see cref="Scancode"/> for the application control "<em>Back</em>" key</summary>
        /// <value>The <see cref="Scancode"/> for the application control "<em>Back</em>" key</value>
        public static Scancode Back { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.ApplicationControlBack); }

        /// <summary>Gets the <see cref="Scancode"/> for the application control "<em>Forward</em>" key</summary>
        /// <value>The <see cref="Scancode"/> for the application control "<em>Forward</em>" key</value>
        public static Scancode Forward { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.ApplicationControlForward); }

        /// <summary>Gets the <see cref="Scancode"/> for the application control "<em>Stop</em>" key</summary>
        /// <value>The <see cref="Scancode"/> for the application control "<em>Stop</em>" key</value>
        public static Scancode Stop { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.ApplicationControlStop); }

        /// <summary>Gets the <see cref="Scancode"/> for the application control "<em>Refresh</em>" key</summary>
        /// <value>The <see cref="Scancode"/> for the application control "<em>Refresh</em>" key</value>
        public static Scancode Refresh { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.ApplicationControlRefresh); }

        /// <summary>Gets the <see cref="Scancode"/> for the application control "<em>Bookmarks</em>" key</summary>
        /// <value>The <see cref="Scancode"/> for the application control "<em>Bookmarks</em>" key</value>
        public static Scancode Bookmarks { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.ApplicationControlBookmarks); }
    }

    /// <summary>Provides predefined <see cref="Scancode"/>s for keys on mobiles</summary>
    public static class Mobile
    {
        /// <summary>Gets the <see cref="Scancode"/> for left software defined key on mobiles</summary>
        /// <value>The <see cref="Scancode"/> for left software defined key on mobiles</value>
        /// <remarks>
        /// <para>
        /// This key is usually situated below the display on phones and used as a multi-function feature key for selecting a software defined function shown on the bottom left of the display.
        /// </para>
        /// </remarks>
        public static Scancode SoftwareLeft { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.SoftwareLeft); }

        /// <summary>Gets the <see cref="Scancode"/> for right software defined key on mobiles</summary>
        /// <value>The <see cref="Scancode"/> for right software defined key on mobiles</value>
        /// <remarks>
        /// <para>
        /// This key is usually situated below the display on phones and used as a multi-function feature key for selecting a software defined function shown on the bottom right of the display.
        /// </para>
        /// </remarks>
        public static Scancode SoftwareRight { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.SoftwareRight); }


        /// <summary>Gets the <see cref="Scancode"/> for the <kbd>📞</kbd> (phone call) key on mobiles</summary>
        /// <value>The <see cref="Scancode"/> for the <kbd>📞</kbd> (phone call) key on mobiles</value>
        public static Scancode Call { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.Call); }

        /// <summary>Gets the <see cref="Scancode"/> for the <kbd>END 📞</kbd> (end phone call) key on mobiles</summary>
        /// <value>The <see cref="Scancode"/> for the <kbd>END 📞</kbd> (end phone call) key on mobiles</value>
        public static Scancode EndCall { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(Kind.EndCall); }
    }

    private static string? KnownKindToString(Kind kind) => kind switch
    {
        Kind.Unknown => nameof(Unknown),
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
        Kind._1 => nameof(_1),
        Kind._2 => nameof(_2),
        Kind._3 => nameof(_3),
        Kind._4 => nameof(_4),
        Kind._5 => nameof(_5),
        Kind._6 => nameof(_6),
        Kind._7 => nameof(_7),
        Kind._8 => nameof(_8),
        Kind._9 => nameof(_9),
        Kind._0 => nameof(_0),
        Kind.Return => nameof(Return),
        Kind.Escape => nameof(Escape),
        Kind.Backspace => nameof(Backspace),
        Kind.Tab => nameof(Tab),
        Kind.Space => nameof(Space),
        Kind.Minus => nameof(Minus),
        Kind.Equal => nameof(Equal),
        Kind.LeftBracket => nameof(LeftBracket),
        Kind.RightBracket => nameof(RightBracket),
        Kind.Backslash => nameof(Backslash),
        Kind.NonUsHash => nameof(NonUsHash),
        Kind.Semicolon => nameof(Semicolon),
        Kind.Apostrophe => nameof(Apostrophe),
        Kind.Grave => nameof(Grave),
        Kind.Comma => nameof(Comma),
        Kind.Period => nameof(Period),
        Kind.Slash => nameof(Slash),
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
        Kind.Delete => nameof(Delete),
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
        Kind.NonUsBackslash => nameof(NonUsBackslash),
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
        Kind.International1 => nameof(International1),
        Kind.International2 => nameof(International2),
        Kind.International3 => nameof(International3),
        Kind.International4 => nameof(International4),
        Kind.International5 => nameof(International5),
        Kind.International6 => nameof(International6),
        Kind.International7 => nameof(International7),
        Kind.International8 => nameof(International8),
        Kind.International9 => nameof(International9),
        Kind.Language1 => nameof(Language1),
        Kind.Language2 => nameof(Language2),
        Kind.Language3 => nameof(Language3),
        Kind.Language4 => nameof(Language4),
        Kind.Language5 => nameof(Language5),

#pragma warning disable SDL5011

        Kind.Language6 => nameof(Language6),
        Kind.Language7 => nameof(Language7),
        Kind.Language8 => nameof(Language8),
        Kind.Language9 => nameof(Language9),

#pragma warning restore SDL5011

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

        _ => default
    };
}
