using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.Marshalling;

namespace Sdl3Sharp;

/// <summary>
/// Represents a hinting option to an application which can be changed via an environment variable or programmatically
/// </summary>
/// <param name="name">The name of the hint</param>
/// <inheritdoc cref="ValidateName(string)"/>
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
[method: MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
public readonly partial struct Hint(string name) :
	IEquatable<Hint>, IFormattable, ISpanFormattable, IEqualityOperators<Hint, Hint, bool>
{
	/// <exception cref="ArgumentException"><c><paramref name="name"/></c> is either <c><see langword="null"/></c>, empty, or consists only of white-space characters</exception>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private static string ValidateName(string name)
	{
		if (string.IsNullOrWhiteSpace(name))
		{
			failNameArgumentNullOrWhiteSpace();
		}

		return name;

		[DoesNotReturn]
		static void failNameArgumentNullOrWhiteSpace() => throw new ArgumentException(message: $"{nameof(name)} must neither be null, empty, nor consists only of white-space characters", paramName: nameof(name));
	}

	private static readonly Dictionary<string, HintValueChangedEventHandler?> mValueChangedEventHandlers = [];

	private readonly string mName = ValidateName(name);

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly string DebuggerDisplay => ToString();

	/// <summary>
	/// Reset all hints to their default value
	/// </summary>
	/// <remarks>
	/// This will reset all hints to the value of the associated environment variable, or <c><see langword="null"/></c> if the environment isn't set. The <see cref="ValueChanged"/> event will be raised normally with this change.
	/// </remarks>
	public static void ResetValueForAll() => SDL_ResetHints();

	/// <summary>
	/// Gets the name of the hint
	/// </summary>
	/// <value>
	/// The name of the hint
	/// </value>
	public readonly string Name { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mName; }

	/// <summary>
	/// Raised when the value of a hint has changed
	/// </summary>
	public readonly event HintValueChangedEventHandler? ValueChanged
	{
		add
		{
			if (string.IsNullOrWhiteSpace(mName))
			{
				return;
			}

			if (!mValueChangedEventHandlers.TryGetValue(mName, out var eventHandler))
			{
				eventHandler = null;

				unsafe
				{
					var nameUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(mName);

					try
					{
						SDL_AddHintCallback(nameUtf8, &HintCallback, null);
					}
					finally
					{
						Utf8StringMarshaller.Free(nameUtf8);
					}
				}
			}

			mValueChangedEventHandlers[mName] = eventHandler + value;
		}

		remove
		{
			if (string.IsNullOrWhiteSpace(mName))
			{
				return;
			}

			if (mValueChangedEventHandlers.TryGetValue(mName, out var eventHandler) && eventHandler is not null)
			{
				eventHandler -= value;

				if (eventHandler is not null)
				{
					mValueChangedEventHandlers[mName] = eventHandler;
				}
				else
				{
					mValueChangedEventHandlers.Remove(mName);

					unsafe
					{
						var nameUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(mName);

						try
						{
							SDL_RemoveHintCallback(nameUtf8, &HintCallback, null);
						}
						finally
						{
							Utf8StringMarshaller.Free(nameUtf8);
						}
					}
				}
			}
		}
	}

	/// <inheritdoc/>
	public readonly override bool Equals([NotNullWhen(true)] object? obj) => obj is Hint other && Equals(other);

	/// <inheritdoc/>
	public readonly bool Equals(Hint other) => string.Equals(mName, other.mName, StringComparison.InvariantCulture);


	/// <summary>
	/// Gets a boolean value representation of the value of the hint
	/// </summary>
	/// <param name="defaultValue">A value to use if the hint value does not exist</param>
	/// <returns><c><paramref name="defaultValue"/></c> if the hint value is <c><see langword="null"/></c> or empty;
	/// otherwise, <c><see langword="true"/></c> if the hint value is <em>neither</em> <c>"0"</c> <em>nor</em> <c>"false"</c>;
	/// otherwise, <c><see langword="false"/></c> </returns>
	public readonly bool GetBooleanValue(bool defaultValue)
	{
		if (!string.IsNullOrWhiteSpace(mName))
		{
			unsafe
			{
				var nameUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(mName);

				try
				{
					return SDL_GetHintBoolean(nameUtf8, defaultValue);
				}
				finally
				{
					Utf8StringMarshaller.Free(nameUtf8);
				}
			}
		}

		return defaultValue;
	}

	/// <inheritdoc/>
	public readonly override int GetHashCode() => mName.GetHashCode();

	/// <inheritdoc/>
	public readonly override string ToString() => mName;

	/// <inheritdoc/>
	readonly string IFormattable.ToString(string? format, IFormatProvider? formatProvider) => ToString();

	/// <inheritdoc cref="ISpanFormattable.TryFormat(Span{char}, out int, ReadOnlySpan{char}, IFormatProvider?)"/>
	public readonly bool TryFormat(Span<char> destination, out int charsWritten)
	{
		if (mName.TryCopyTo(destination))
		{
			charsWritten = mName.Length;

			return true;
		}

		charsWritten = 0;

		return false;
	}

	/// <inheritdoc/>
	bool ISpanFormattable.TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider) => TryFormat(destination, out charsWritten);

	/// <summary>
	/// Tries to retrieve the value of the hint
	/// </summary>
	/// <param name="value">The value of the hint, or <c><see langword="null"/></c> if the value of the hint does not exist</param>
	/// <returns><c><see langword="true"/></c> if the hint has a value set; otherwise, <c><see langword="false"/></c></returns>
	public readonly bool TryGetValue([NotNullWhen(true)] out string? value)
	{
		if (!string.IsNullOrWhiteSpace(mName))
		{
			unsafe
			{
				var nameUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(mName);

				try
				{
					var result = Utf8StringMarshaller.ConvertToManaged(SDL_GetHint(nameUtf8));

					if (result is not null)
					{
						value = result;

						return true;
					}
				}
				finally
				{
					Utf8StringMarshaller.Free(nameUtf8);
				}
			}
		}

		value = null;

		return false;
	}

	/// <summary>
	/// Tries to reset the hint to its default value
	/// </summary>
	/// <returns><c><see langword="true"/></c> if the hint was successfully reset to its default value; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// This will reset a hint to the value of the associated environment variable, or <c><see langword="null"/></c> if the environment isn't set. The <see cref="ValueChanged"/> event will be raised normally with this change.
	/// </remarks>
	public readonly bool TryResetValue()
	{
		if (!string.IsNullOrWhiteSpace(mName))
		{
			unsafe
			{
				var nameUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(mName);

				try
				{
					return SDL_ResetHint(nameUtf8);
				}
				finally
				{
					Utf8StringMarshaller.Free(nameUtf8);
				}
			}
		}

		return false;
	}

	/// <summary>
	/// Tries to set the value of the hint with <see cref="HintPriority.Normal">normal priority</see>.
	/// </summary>
	/// <param name="value">The value to assign to the hint; use <c><see langword="null"/></c> to indicate that the hint has no value set</param>
	/// <returns><c><see langword="true"/></c> if the value of the hint was successfully set; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// Hints will not be set if there is an existing override hint or environment variable that takes precedence.
	/// You can use <see cref="TrySetValue(string?, HintPriority)"/> to set the hint with <see cref="HintPriority.Override">override priority</see> instead.
	/// </remarks>
	public readonly bool TrySetValue(string? value)
	{
		if (!string.IsNullOrWhiteSpace(mName))
		{
			unsafe
			{
				var nameUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(mName);
				var valueUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(value);

				try
				{
					return SDL_SetHint(nameUtf8, valueUtf8);
				}
				finally
				{
					Utf8StringMarshaller.Free(valueUtf8);
					Utf8StringMarshaller.Free(nameUtf8);
				}
			}
		}

		return false;
	}

	/// <summary>
	/// Tries to set the value of the hint with a specific <see cref="HintPriority">priority</see>
	/// </summary>
	/// <param name="value">The value to assign to the hint; use <c><see langword="null"/></c> to indicate that the hint has no value set</param>
	/// <param name="priority">The priority of the hint</param>
	/// <returns><c><see langword="true"/></c> if the value of the hint was successfully set; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// The priority controls the behavior when setting a hint that already has a value.
	/// Hints will replace existing hints of their own priority and lower.
	/// Environment variables are considered to have <see cref="HintPriority.Override">override priority</see>.
	/// </remarks>
	public readonly bool TrySetValue(string? value, HintPriority priority)
	{
		if (!string.IsNullOrWhiteSpace(mName))
		{
			unsafe
			{
				var nameUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(mName);
				var valueUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(value);

				try
				{
					return SDL_SetHintWithPriority(nameUtf8, valueUtf8, priority);
				}
				finally
				{
					Utf8StringMarshaller.Free(valueUtf8);
					Utf8StringMarshaller.Free(nameUtf8);
				}
			}
		}

		return false;
	}

	/// <inheritdoc/>
	public static bool operator ==(Hint left, Hint right) => left.Equals(right);

	/// <inheritdoc/>
	public static bool operator !=(Hint left, Hint right) => !(left == right);
}
