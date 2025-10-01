using Sdl3Sharp.Events;
using Sdl3Sharp.Internal;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace Sdl3Sharp;

/// <summary>
/// Represents locale identification data
/// </summary>
/// <param name="language">The language identifier for the <see cref="Locale"/></param>
/// <param name="country">The country identifier for the <see cref="Locale"/>, or <c><see langword="null"/></c> if there's none</param>
/// <remarks>
/// <para>
/// Locale identification data is split into a spoken <see cref="Language">language</see>, like English, and an optional <see cref="Country">country</see>, like Canada.
/// </para>
/// </remarks>
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
[StructLayout(LayoutKind.Sequential)]
[method: SetsRequiredMembers]
public readonly partial struct Locale(string language, string? country = null) :
	IEquatable<Locale>, IFormattable, ISpanFormattable, IEqualityOperators<Locale, Locale, bool>
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly string DebuggerDisplay => ToString();

	/// <summary>
	/// Gets the language identifier for the <see cref="Locale"/>
	/// </summary>
	/// <value>
	/// The language identifier for the <see cref="Locale"/>
	/// </value>
	/// <remarks>
	/// <para>
	/// The value for this property will be in <see href="https://en.wikipedia.org/wiki/ISO_639">ISO-639 format</see>, e.g. English would be <c>"en"</c>.
	/// </para>
	/// </remarks>
	public readonly required string Language
	{
		get => field;
		init => field = value.ToLowerInvariant();
	} = language.ToLowerInvariant();

	/// <summary>
	/// Gets the country identifier for the <see cref="Locale"/>
	/// </summary>
	/// <value>
	/// The country identifier for the <see cref="Locale"/>, if there's any; otherwise, <c><see langword="null"/></c>
	/// </value>
	/// <remarks>
	/// <para>
	/// The value for this property will be in <see href="https://en.wikipedia.org/wiki/ISO_3166">ISO-3166 format</see>, e.g. Canada would be <c>"CA"</c>, if there's a country identifier to the <see cref="Locale"/>;
	/// otherwise, the value for this property will be <c><see langword="null"/></c>.
	/// </para>
	/// </remarks>	
	public readonly string? Country
	{
		get => field;
		init => field = value?.ToUpperInvariant();
	} = country?.ToUpperInvariant();

	/// <summary>
	/// Tries to get the user's preferred <see cref="Locale">locales</see>
	/// </summary>
	/// <param name="locales">The user's preferred <see cref="Locale">locales</see> when this method returns <c><see langword="true"/></c>; otherwise, <c><see langword="null"/></c></param>
	/// <returns><c><see langword="true"/></c>, if the user's preferred locales could be successfully retrieved; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// The returned list of locales are in the order of the user's preference. For example, a German citizen that is fluent in US English and knows enough Japanese to navigate around Tokyo might have a list like: de, en-US, jp.
	/// Someone from England might prefer British English (where "color" is spelled "colour", etc), but will settle for anything like it: en-GB, en.
	/// </para>
	/// <para>
	/// This method returns <c><see langword="false"/></c> together with <paramref name="locales"/> set to <c><see langword="null"/></c> on failure,
	/// including when the platform does not supply preferred locale information at all.
	/// </para>
	/// <para>
	/// This might be a "slow" call that has to query the operating system.
	/// It's best to ask for this once and save the results.
	/// However, this list can change, usually because the user has changed a system preference outside of your program;
	/// SDL will send an <see cref="EventType.Application.LocaleChanged"/> event (see also <see cref="Sdl.LocaleChanged"/>) in this case, if possible, and you can call this method again to get an updated copy of preferred locales.
	/// </para>
	/// </remarks>
	public static bool TryGetPreferredLocales([NotNullWhen(true)] out Locale[]? locales)
	{
		unsafe
		{
			int count;

			var resultLocales = SDL_GetPreferredLocales(&count);

			if (resultLocales is null)
			{
				locales = null;

				return false;
			}

			if (count is not > 0)
			{
				locales = [];

				return true;
			}			

			locales = GC.AllocateUninitializedArray<Locale>(count);

			var localesPtrs = resultLocales;
			foreach (ref var locale in locales.AsSpan())
			{
				if (*localesPtrs is null || (*localesPtrs)->Language is null)
				{
					break;
				}			

				locale = new(
					Utf8StringMarshaller.ConvertToManaged((*localesPtrs)->Language)!,
					Utf8StringMarshaller.ConvertToManaged((*localesPtrs)->Country)
				);

				localesPtrs++;
			}

			Utilities.NativeMemory.SDL_free(resultLocales);

			return true;
		}
	}

	/// <summary>
	/// Deconstructs the <see cref="Locale"/> into its <see cref="Language">language identifier part</see> and its <see cref="Country">country identifier part</see>
	/// </summary>
	/// <param name="language">The language identifier for the <see cref="Locale"/></param>
	/// <param name="country">The country identifier for the <see cref="Locale"/>, if there's any; otherwise, <c><see langword="null"/></c></param>
	/// <remarks>
	/// <para>
	/// The resulting value for the <paramref name="language"/> parameter will be in <see href="https://en.wikipedia.org/wiki/ISO_639">ISO-639 format</see>, e.g. English would be <c>"en"</c>.
	/// </para>
	/// <para>
	/// The resulting value for the <paramref name="country"/> parameter will be in <see href="https://en.wikipedia.org/wiki/ISO_3166">ISO-3166 format</see>, e.g. Canada would be <c>"CA"</c>, if there's a country identifier to the <see cref="Locale"/>;
	/// otherwise, the value will be <c><see langword="null"/></c>.
	/// </para>
	/// </remarks> 
	/// <seealso cref="Language"/>
	/// <seealso cref="Country"/>
	public void Deconstruct(out string language, out string? country) { language = Language; country = Country; }

	/// <inheritdoc/>
	public readonly override bool Equals([NotNullWhen(true)] object? obj) => obj is Locale other && Equals(other);

	/// <inheritdoc/>
	public readonly bool Equals(Locale other)
		=> string.Equals(Language, other.Language, StringComparison.InvariantCultureIgnoreCase)
		&& string.Equals(Country, other.Country, StringComparison.InvariantCultureIgnoreCase);

	/// <inheritdoc/>
	public readonly override int GetHashCode() => HashCode.Combine(Language, Country);

	/// <inheritdoc/>
	public readonly override string ToString()
		=> $"{Language}{Country switch { var country when !string.IsNullOrWhiteSpace(country) => $"-{country}", _ => string.Empty }}";

	/// <inheritdoc/>
	readonly string IFormattable.ToString(string? format, IFormatProvider? formatProvider) => ToString();

	/// <inheritdoc cref="ISpanFormattable.TryFormat(Span{char}, out int, ReadOnlySpan{char}, IFormatProvider?)"/>
	public readonly bool TryFormat(Span<char> destination, out int charsWritten)
	{
		charsWritten = 0;

		if (!SpanFormat.TryWrite(Language, ref destination, ref charsWritten))
		{
			return false;
		}

		var country = Country;

		return string.IsNullOrWhiteSpace(country)
			|| (SpanFormat.TryWrite('-', ref destination, ref charsWritten)	&& SpanFormat.TryWrite(country, ref destination, ref charsWritten));
	}

	/// <inheritdoc/>
	readonly bool ISpanFormattable.TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider) => TryFormat(destination, out charsWritten);

	/// <inheritdoc/>
	public static bool operator ==(Locale left, Locale right) => left.Equals(right);

	/// <inheritdoc/>
	public static bool operator !=(Locale left, Locale right) => !(left == right);
}
