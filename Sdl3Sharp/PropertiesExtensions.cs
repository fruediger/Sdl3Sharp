using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp;

/// <summary>
/// Provides extensions for <see cref="Properties"/>
/// </summary>
public static class PropertiesExtensions
{
	/// <summary>
	/// Adds a propery with a specified <paramref name="name"/> to a <see cref="Properties">group of <paramref name="properties"/></see>, when the property doesn't yet exist there
	/// </summary>
	/// <param name="properties">The <see cref="Properties">group of properties</see> where to property should be added</param>
	/// <param name="name">The name of the property to be added</param>
	/// <param name="value">The value of the property to be added</param>
	/// <returns><c><see langword="true"/></c>, if the property was successfully added to the <see cref="Properties">group of <paramref name="properties"/></see>; otherwise, <c><see langword="false"/></c></returns>
	/// <remarks>
	/// You shouldn't necessarily call this method directly from user code. It primarily exists to allow <see cref="Properties"/> to be used with collection initialization.
	/// </remarks>
	public static bool Add(this Properties properties, string name, bool value)
		=> !properties.Contains(name) && properties.TrySetBooleanValue(name, value);

	/// <summary>
	/// Adds a propery with a specified <paramref name="name"/> to a <see cref="Properties">group of <paramref name="properties"/></see>, when the property doesn't yet exist there
	/// </summary>
	/// <param name="properties">The <see cref="Properties">group of properties</see> where to property should be added</param>
	/// <param name="name">The name of the property to be added</param>
	/// <param name="value">The value of the property to be added</param>
	/// <returns><c><see langword="true"/></c>, if the property was successfully added to the <see cref="Properties">group of <paramref name="properties"/></see>; otherwise, <c><see langword="false"/></c></returns>
	/// <remarks>
	/// You shouldn't necessarily call this method directly from user code. It primarily exists to allow <see cref="Properties"/> to be used with collection initialization.
	/// </remarks>
	public static bool Add(this Properties properties, string name, float value)
		=> !properties.Contains(name) && properties.TrySetFloatValue(name, value);

	/// <summary>
	/// Adds a propery with a specified <paramref name="name"/> to a <see cref="Properties">group of <paramref name="properties"/></see>, when the property doesn't yet exist there
	/// </summary>
	/// <param name="properties">The <see cref="Properties">group of properties</see> where to property should be added</param>
	/// <param name="name">The name of the property to be added</param>
	/// <param name="value">The value of the property to be added</param>
	/// <returns><c><see langword="true"/></c>, if the property was successfully added to the <see cref="Properties">group of <paramref name="properties"/></see>; otherwise, <c><see langword="false"/></c></returns>
	/// <remarks>
	/// You shouldn't necessarily call this method directly from user code. It primarily exists to allow <see cref="Properties"/> to be used with collection initialization.
	/// </remarks>
	public static bool Add(this Properties properties, string name, long value)
		=> !properties.Contains(name) && properties.TrySetNumberValue(name, value);

	/// <summary>
	/// Adds a propery with a specified <paramref name="name"/> to a <see cref="Properties">group of <paramref name="properties"/></see>, when the property doesn't yet exist there
	/// </summary>
	/// <typeparam name="TPointer">The type of pointer value to be used as the value of the property. This has to be <see cref="IntPtr"/>.</typeparam>
	/// <param name="properties">The <see cref="Properties">group of properties</see> where to property should be added</param>
	/// <param name="name">The name of the property to be added</param>
	/// <param name="value">The value of the property to be added</param>
	/// <returns><c><see langword="true"/></c>, if the property was successfully added to the <see cref="Properties">group of <paramref name="properties"/></see>; otherwise, <c><see langword="false"/></c></returns>
	/// <remarks>
	/// You shouldn't necessarily call this method directly from user code. It primarily exists to allow <see cref="Properties"/> to be used with collection initialization.
	/// </remarks>
	public static bool Add<TPointer>(this Properties properties, string name, TPointer value)
		where TPointer : struct, IBinaryInteger<IntPtr> // Using a generic type parameter here may seem a little convoluted, but this allows for better overload resolution,
														// where this method gets chosen if and only if 'value' is of 'IntPtr'.
														// This makes sure that values of other integer types won't get implicit converted to 'nint',
														// but to 'long' and the respective overload for 'long' (number) types gets chosen.
		=> !properties.Contains(name) && properties.TrySetPointerValue(name, Unsafe.BitCast<TPointer, IntPtr>(value));

	/// <summary>
	/// Adds a propery with a specified <paramref name="name"/> to a <see cref="Properties">group of <paramref name="properties"/></see>, when the property doesn't yet exist there.
	/// This sets the pointer value of the property together with a <paramref name="cleanup"/> callback that is called when the property is deleted.
	/// </summary>
	/// <typeparam name="TPointer">The type of pointer value to be used as the value of the property. This has to be <see cref="IntPtr"/>.</typeparam>
	/// <param name="properties">The <see cref="Properties">group of properties</see> where to property should be added</param>
	/// <param name="name">The name of the property to be added</param>
	/// <param name="value">The value of the property to be added</param>
	/// <param name="cleanup">The callback to call when the property is deleted, or <c><see langword="null"/></c> if no cleanup is necessary</param>
	/// <returns><c><see langword="true"/></c>, if the property was successfully added to the <see cref="Properties">group of <paramref name="properties"/></see>; otherwise, <c><see langword="false"/></c></returns>
	/// <remarks>
	/// You shouldn't necessarily call this method directly from user code. It primarily exists to allow <see cref="Properties"/> to be used with collection initialization.
	/// </remarks>
	public static bool Add<TPointer>(this Properties properties, string name, TPointer value, Action<IntPtr>? cleanup)
		where TPointer : struct, IBinaryInteger<IntPtr> // see above, this is only generic for sake of completeness
		=> !properties.Contains(name) && properties.TrySetPointerValue(name, Unsafe.BitCast<TPointer, IntPtr>(value), cleanup);

	/// <summary>
	/// Adds a propery with a specified <paramref name="name"/> to a <see cref="Properties">group of <paramref name="properties"/></see>, when the property doesn't yet exist there
	/// </summary>
	/// <param name="properties">The <see cref="Properties">group of properties</see> where to property should be added</param>
	/// <param name="name">The name of the property to be added</param>
	/// <param name="value">The value of the property to be added</param>
	/// <returns><c><see langword="true"/></c>, if the property was successfully added to the <see cref="Properties">group of <paramref name="properties"/></see>; otherwise, <c><see langword="false"/></c></returns>
	/// <remarks>
	/// You shouldn't necessarily call this method directly from user code. It primarily exists to allow <see cref="Properties"/> to be used with collection initialization.
	/// </remarks>
	public static bool Add(this Properties properties, string name, string? value)
		=> !properties.Contains(name) && properties.TrySetStringValue(name, value);
}
