using Sdl3Sharp.Internal;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Runtime.InteropServices;

namespace Sdl3Sharp.Utilities;

[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
[StructLayout(LayoutKind.Sequential)]
[method: SetsRequiredMembers]
public readonly struct MessageBoxColorScheme(MessageBoxColor background, MessageBoxColor text, MessageBoxColor buttonBorder, MessageBoxColor buttonBackground, MessageBoxColor buttonSelected) :
	IEquatable<MessageBoxColorScheme>, IFormattable, ISpanFormattable, IEqualityOperators<MessageBoxColorScheme, MessageBoxColorScheme, bool>
{
	internal readonly MessageBox.SDL_MessageBoxColorScheme Data = new()
	{
		Colors =
		{
			[MessageBox.SDL_MessageBoxColorType.Background] = background,
			[MessageBox.SDL_MessageBoxColorType.Text] = text,
			[MessageBox.SDL_MessageBoxColorType.ButtonBorder] = buttonBorder,
			[MessageBox.SDL_MessageBoxColorType.ButtonBackground] = buttonBackground,
			[MessageBox.SDL_MessageBoxColorType.ButtonSelected] = buttonSelected
		}
	};
	
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly string DebuggerDisplay => ToString(formatProvider: CultureInfo.InvariantCulture);

	public readonly required MessageBoxColor Background
	{
		get => Data.Colors[MessageBox.SDL_MessageBoxColorType.Background];
		init => Data.Colors[MessageBox.SDL_MessageBoxColorType.Background] = value;
	}

	public readonly required MessageBoxColor ButtonBorder
	{
		get => Data.Colors[MessageBox.SDL_MessageBoxColorType.ButtonBorder];
		init => Data.Colors[MessageBox.SDL_MessageBoxColorType.ButtonBorder] = value;
	}

	public readonly required MessageBoxColor ButtonBackground
	{
		get => Data.Colors[MessageBox.SDL_MessageBoxColorType.ButtonBackground];
		init => Data.Colors[MessageBox.SDL_MessageBoxColorType.ButtonBackground] = value;
	}

	public readonly required MessageBoxColor ButtonSelected
	{
		get => Data.Colors[MessageBox.SDL_MessageBoxColorType.ButtonSelected];
		init => Data.Colors[MessageBox.SDL_MessageBoxColorType.ButtonSelected] = value;
	}

	public readonly required MessageBoxColor Text
	{
		get => Data.Colors[MessageBox.SDL_MessageBoxColorType.Text];
		init => Data.Colors[MessageBox.SDL_MessageBoxColorType.Text] = value;
	}

	public void Deconstruct(out MessageBoxColor background, out MessageBoxColor text, out MessageBoxColor buttonBorder, out MessageBoxColor buttonBackground, out MessageBoxColor buttonSelected)
	{ background = Background; text = Text; buttonBorder = ButtonBorder; buttonBackground = ButtonBackground; buttonSelected = ButtonSelected; }

	public override bool Equals([NotNullWhen(true)] object? obj) => obj is MessageBoxColorScheme other && Equals(other);

	public readonly bool Equals(in MessageBoxColorScheme other)
		=> Background == other.Background
		&& Text == other.Text
		&& ButtonBorder == other.ButtonBorder
		&& ButtonBackground == other.ButtonBackground
		&& ButtonSelected == other.ButtonSelected;

	readonly bool IEquatable<MessageBoxColorScheme>.Equals(MessageBoxColorScheme other) => Equals(other);

	public readonly override int GetHashCode() => HashCode.Combine(Background, Text, ButtonBorder, ButtonBackground, ButtonSelected);

	public readonly override string ToString() => ToString(format: default, formatProvider: default);

	public readonly string ToString(IFormatProvider? formatProvider) => ToString(format: default, formatProvider);

	public readonly string ToString(string? format) => ToString(format, formatProvider: default);

	public readonly string ToString(string? format, IFormatProvider? formatProvider)
		=> $"{{ {
			nameof(Background)}: {Background.ToString(format, formatProvider)}, {
			nameof(Text)}: {Text.ToString(format, formatProvider)}, {
			nameof(ButtonBorder)}: {ButtonBorder.ToString(format, formatProvider)}, {
			nameof(ButtonBackground)}: {ButtonBackground.ToString(format, formatProvider)}, {
			nameof(ButtonSelected)}: {ButtonSelected.ToString(format, formatProvider)} }}";

	public readonly bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format = default, IFormatProvider? provider = default)
	{
		charsWritten = 0;

		return SpanFormat.TryWrite($"{{ {nameof(Background)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(Background, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite($", {nameof(Text)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(Text, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite($", {nameof(ButtonBorder)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(ButtonBorder, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite($", {nameof(ButtonBackground)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(ButtonBackground, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite($", {nameof(ButtonSelected)}: ", ref destination, ref charsWritten)
			&& SpanFormat.TryWrite(ButtonSelected, ref destination, ref charsWritten, format, provider)
			&& SpanFormat.TryWrite(" }", ref destination, ref charsWritten);
	}

	public static bool operator ==(in MessageBoxColorScheme left, in MessageBoxColorScheme right) => left.Equals(right);

	static bool IEqualityOperators<MessageBoxColorScheme, MessageBoxColorScheme, bool>.operator ==(MessageBoxColorScheme left, MessageBoxColorScheme right) => left == right;

	public static bool operator !=(in MessageBoxColorScheme left, in MessageBoxColorScheme right) => !(left == right);

	static bool IEqualityOperators<MessageBoxColorScheme, MessageBoxColorScheme, bool>.operator !=(MessageBoxColorScheme left, MessageBoxColorScheme right) => left != right;
}
