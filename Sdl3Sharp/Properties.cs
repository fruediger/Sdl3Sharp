using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace Sdl3Sharp;

/// <summary>
/// Represents a group of (named) properties
/// </summary>
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
public sealed partial class Properties :
	IEnumerable<string>, IDisposable, Sdl.IDisposeReceiver, IEquatable<Properties>, IFormattable, ISpanFormattable
{
	/// <exception cref="ArgumentNullException"><c><paramref name="sdl"/></c> is <c><see langword="null"/></c></exception>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	[return: NotNull]
	private static Sdl ValidateSdl([MaybeNull] Sdl sdl)
	{
		if (sdl is null)
		{
			failSdlArgumentNull();
		}

		return sdl;

		[DoesNotReturn]
		static void failSdlArgumentNull() => throw new ArgumentNullException(nameof(sdl));
	}

	/// <exception cref="SdlException">Couldn't create a new property group (check <see cref="Error.TryGet(out string?)"/> for more information)</exception>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private static uint ValidateId(uint id)
	{
		if (id is 0)
		{
			failIdArgumentIsZero();
		}

		return id;

		[DoesNotReturn]
		static void failIdArgumentIsZero() => throw new SdlException($"SDL returned an invalid {nameof(Properties)} with an {nameof(Id)} of 0");
	}

	private WeakReference<Sdl>? mSdlReference;
	private uint mId;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private string DebuggerDisplay => ToString(CultureInfo.InvariantCulture);

	/// <exception cref="InvalidOperationException">Could not register the <see cref="Properties"/> with the given <paramref name="sdl"/> instance</exception>
	internal Properties(Sdl? sdl, uint id)
	{
		if (sdl is not null)
		{
			if (!sdl.TryRegisterDisposable(this))
			{
				SDL_DestroyProperties(id);

				failCouldNotRegisterWithSdl();
			}

			mSdlReference = new(sdl);
		}

		mId = id;

		[DoesNotReturn]
		static void failCouldNotRegisterWithSdl() => throw new InvalidOperationException($"Couldn't register the {nameof(Properties)} with the given {nameof(Sdl)} instance");
	}

	/// <summary>
	/// Creates a new group of properties
	/// </summary>
	/// <param name="sdl">The current <see cref="Sdl"/> instance</param>
	/// <remarks>
	/// In contrast to most of the remaining API which uses the <c>Try</c>-method pattern, this constructor intentionally fails by throwing an exception.
	/// If you want to handle failures wrap the call to this constructor in a <c><see langword="try"/></c>-block,
	/// and check <see cref="Error.TryGet(out string?)"/> for more information when <c><see langword="catch"/></c>ing a <see cref="SdlException"/>.
	/// </remarks>
	/// <inheritdoc cref="ValidateSdl(Sdl)"/>
	/// <inheritdoc cref="ValidateId(uint)"/>
	/// <inheritdoc cref="Properties(Sdl?, uint)"/>
	public Properties(Sdl sdl) : this(ValidateSdl(sdl), ValidateId(SDL_CreateProperties()))
	{ }

	/// <inheritdoc/>
	~Properties() => Dispose(deregister: true);

	/// <summary>
	/// Gets the id of the group of properties
	/// </summary>
	/// <value>
	/// The id of the group of properties
	/// </value>
	/// <remarks>An id value of <c>0</c> indicates an invalid group of properties</remarks>
	public uint Id { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mId; }

	/*
	/// <summary>
	/// Gets a <see cref="Property"/> with a specified <see cref="Property.Name">name</see> within the group of properties
	/// </summary>
	/// <value>
	/// A <see cref="Property"/> with a specified <see cref="Property.Name">name</see> within the group of properties
	/// </value>
	/// <param name="name">The name of the property</param>
	/// <inheritdoc cref="Property(Properties, string)"/>
	public Property this[string name] { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(this, name); }
	*/

	/// <summary>
	/// Determines whether a property with a specified <paramref name="name"/> exists within the group of properties
	/// </summary>
	/// <param name="name">The name of the property to check if it exists within the group of properties</param>
	/// <returns><c><see langword="true"/></c> if the property exists within the group of properties; otherwise, <c><see langword="false"/></c></returns>
	public bool Contains(string name)
	{
		if (string.IsNullOrWhiteSpace(name))
		{
			return false;
		}

		unsafe
		{
			var nameUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(name);

			try
			{
				return SDL_HasProperty(mId, nameUtf8);
			}
			finally
			{
				Utf8StringMarshaller.Free(nameUtf8);
			}
		}
	}

	/// <summary>
	/// Diposes and potentially destroys the group of properties
	/// </summary>
	/// <remarks>
	/// When a group of properties is destroyed, all properties are deleted and their cleanup callbacks will be called, if any.
	/// Alternatively, all properties are automatically destroyed when <see cref="Sdl.Dispose"/> is called.
	/// </remarks>
	public void Dispose()
	{
		GC.SuppressFinalize(this);
		Dispose(deregister: true);
	}

	void Sdl.IDisposeReceiver.DisposeFromSdl(Sdl sdl)
	{
#pragma warning disable IDE0079
#pragma warning disable CA1816
		GC.SuppressFinalize(this);
#pragma warning restore CA1816
#pragma warning restore IDE0079
		Dispose(deregister: false);
	}

	private void Dispose(bool deregister)
	{
		if (mId is not 0)
		{
			if (mSdlReference is not null)
			{
				if (deregister && mSdlReference.TryGetTarget(out var sdl))
				{
					sdl.TryDeregisterDisposable(this);
				}

				mSdlReference = null;

				SDL_DestroyProperties(mId);
			}

			mId = 0;
		}
	}

	/// <inheritdoc/>
	public override bool Equals([NotNullWhen(true)] object? obj) => Equals(obj as Properties);

	/// <inheritdoc/>
	public bool Equals([NotNullWhen(true)] Properties? other) => other is { mId: var otherId } && mId == otherId;

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public override int GetHashCode() => mId.GetHashCode();

	/// <summary>
	/// Gets the boolean value of a property with a specified <paramref name="name"/> within the group of properties
	/// </summary>
	/// <param name="name">The name of the property to get it's boolean value</param>
	/// <param name="defaultValue">The default value of the property, if it does not exist or isn't a boolean property</param>
	/// <returns>The boolean value of the property, if the property exists within the group of properties and it's set as a boolean property; otherwise, <c><paramref name="defaultValue"/></c></returns>
	public bool GetBooleanValueOrDefault(string name, bool defaultValue)
	{
		if (string.IsNullOrWhiteSpace(name))
		{
			return defaultValue;
		}

		unsafe
		{
			var nameUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(name);

			try
			{
				return SDL_GetBooleanProperty(mId, nameUtf8, defaultValue);
			}
			finally
			{
				Utf8StringMarshaller.Free(nameUtf8);
			}
		}
	}

	/// <summary>
	/// Gets the floating point value of a property with a specified <paramref name="name"/> within the group of properties
	/// </summary>
	/// <param name="name">The name of the property to get it's floating point value</param>
	/// <param name="defaultValue">The default value of the property, if it does not exist or isn't a floating point property</param>
	/// <returns>The floating point value of the property, if the property exists within the group of properties and it's set as a floating point property; otherwise, <c><paramref name="defaultValue"/></c></returns>
	public float GetFloatValueOrDefault(string name, float defaultValue)
	{
		if (string.IsNullOrWhiteSpace(name))
		{
			return defaultValue;
		}

		unsafe
		{
			var nameUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(name);

			try
			{
				return SDL_GetFloatProperty(mId, nameUtf8, defaultValue);
			}
			finally
			{
				Utf8StringMarshaller.Free(nameUtf8);
			}
		}
	}

	/// <summary>
	/// Gets the numeric (integer) value of a property with a specified <paramref name="name"/> within the group of properties
	/// </summary>
	/// <param name="name">The name of the property to get it's numeric value</param>
	/// <param name="defaultValue">The default value of the property, if it does not exist or isn't a number property</param>
	/// <returns>The numeric (integer) value of the property, if the property exists within the group of properties and it's set as a number property; otherwise, <c><paramref name="defaultValue"/></c></returns>
	public long GetNumberValueOrDefault(string name, long defaultValue)
	{
		if (string.IsNullOrWhiteSpace(name))
		{
			return defaultValue;
		}

		unsafe
		{
			var nameUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(name);

			try
			{
				return SDL_GetNumberProperty(mId, nameUtf8, defaultValue);
			}
			finally
			{
				Utf8StringMarshaller.Free(nameUtf8);
			}
		}
	}

	/// <summary>
	/// Gets the pointer value of a property with a specified <paramref name="name"/> within the group of properties
	/// </summary>
	/// <param name="name">The name of the property to get it's pointer value</param>
	/// <param name="defaultValue">The default value of the property, if it does not exist or isn't a pointer property</param>
	/// <returns>The pointer value of the property, if the property exists within the group of properties and it's set as a pointer property; otherwise, <c><paramref name="defaultValue"/></c></returns>
	public IntPtr GetPointerValueOrDefault(string name, IntPtr defaultValue)
	{
		if (string.IsNullOrWhiteSpace(name))
		{
			return defaultValue;
		}

		unsafe
		{
			var nameUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(name);

			try
			{
				return unchecked((IntPtr)SDL_GetPointerProperty(mId, nameUtf8, unchecked((void*)defaultValue)));
			}
			finally
			{
				Utf8StringMarshaller.Free(nameUtf8);
			}
		}
	}

	/// <summary>
	/// Gets the string value of a property with a specified <paramref name="name"/> within the group of properties
	/// </summary>
	/// <param name="name">The name of the property to get it's string value</param>
	/// <param name="defaultValue">The default value of the property, if it does not exist or isn't a string property</param>
	/// <returns>The string value of the property, if the property exists within the group of properties and it's set as a string property; otherwise, <c><paramref name="defaultValue"/></c></returns>
	public string? GetStringValueOrDefault(string name, string? defaultValue)
	{
		if (string.IsNullOrWhiteSpace(name))
		{
			return defaultValue;
		}

		unsafe
		{
			var nameUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(name);
			var defaultValueUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(defaultValue);

			try
			{
				return Utf8StringMarshaller.ConvertToManaged(SDL_GetStringProperty(mId, nameUtf8, defaultValueUtf8));
			}
			finally
			{
				Utf8StringMarshaller.Free(defaultValueUtf8);
				Utf8StringMarshaller.Free(nameUtf8);
			}
		}
	}

	/// <summary>
	/// Gets the <see cref="PropertyType">type</see> of a property with a specified <paramref name="name"/> within the group of properties
	/// </summary>
	/// <param name="name">The name of the property to get it's <see cref="PropertyType">type</see></param>
	/// <returns>The <see cref="PropertyType">type</see> of the property, if the property exists and it's set within the group of properties; otherwise, <c><see cref="PropertyType.Invalid"/></c></returns>
	public PropertyType GetType(string name)
	{
		if (string.IsNullOrWhiteSpace(name))
		{
			return PropertyType.Invalid;
		}

		unsafe
		{
			var nameUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(name);

			try
			{
				return SDL_GetPropertyType(mId, nameUtf8);
			}
			finally
			{
				Utf8StringMarshaller.Free(nameUtf8);
			}
		}
	}

	/// <inheritdoc/>
	public override string ToString() => ToString(format: default, formatProvider: default);

	/// <inheritdoc cref="IFormattable.ToString(string?, IFormatProvider?)"/>
	public string ToString(IFormatProvider? formatProvider) => ToString(format: default, formatProvider);

	/// <inheritdoc cref="IFormattable.ToString(string?, IFormatProvider?)"/>
	public string ToString(string? format) => ToString(format, formatProvider: default);

	/// <inheritdoc/>
	public string ToString(string? format, IFormatProvider? formatProvider) => mId switch
	{
		0 => "<Invalid>",
		_ => $"{{ {nameof(Id)}: {mId.ToString(format, formatProvider)} }}"
	};


	/// <summary>
	/// Tries to copy a group of properties to another
	/// </summary>
	/// <param name="destination">The destination group of properties</param>
	/// <returns><c><see langword="true"/></c> if the group of properties was successfully copied to <paramref name="destination"/>; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	public bool TryCopyTo(Properties destination) => SDL_CopyProperties(mId, destination.mId);

	/// <summary>
	/// Tries to enumerate the names of the properties within the group of properties
	/// </summary>
	/// <param name="action">An <see cref="Action{T}"/> to perform on each name of a property of the group of properties</param>
	/// <returns><c><see langword="true"/></c> if the enumeration of the properties of the group of properties were successfully performed; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// The <paramref name="action"/> is called for each property (name) in the group of properties. The properties are locked during enumeration.
	/// </remarks>
	public bool TryForEach(Action<string> action)
	{
		if (action is not null)
		{
			unsafe
			{
				var gcHandle = GCHandle.Alloc(action, GCHandleType.Normal);

				try
				{
					return SDL_EnumerateProperties(mId, &EnumeratePropertiesCallback, unchecked((void*)GCHandle.ToIntPtr(gcHandle)));
				}
				finally
				{
					gcHandle.Free();
				}
			}
		}

		return false;
	}

	/// <inheritdoc/>
	public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format = default, IFormatProvider? provider = default)
	{
		static bool tryWriteSpan(ReadOnlySpan<char> value, ref Span<char> destination, ref int charsWritten)
		{
			var result = value.TryCopyTo(destination);

			if (result)
			{
				destination = destination[value.Length..];
				charsWritten += value.Length;
			}

			return result;
		}

		static bool tryWriteUInt(uint value, ref Span<char> destination, ref int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
		{
			var result = value.TryFormat(destination, out var tmp, format, provider);

			if (result)
			{
				destination = destination[tmp..];
				charsWritten += tmp;
			}

			return result;
		}

		charsWritten = 0;

		return mId switch
		{
			0 => tryWriteSpan("<Invalid>", ref destination, ref charsWritten),
			_ => tryWriteSpan($"{{ {nameof(Id)}: ", ref destination, ref charsWritten)
			  && tryWriteUInt(mId, ref destination, ref charsWritten, format, provider)
			  && tryWriteSpan(" }", ref destination, ref charsWritten)
		};
	}

	/// <summary>
	/// Tries to get the boolean value of a property with a specified <paramref name="name"/> within the group of properties
	/// </summary>
	/// <param name="name">The name of the property to get it's boolean value</param>
	/// <param name="value">The boolean value of the property, when this method returns <c><see langword="true"/></c>; otherwise, <c><see langword="default"/>(<see cref="bool"/>)</c></param>
	/// <returns><c><see langword="true"/></c> if the property exists within the group of properties and it's set as a boolean property; otherwise, <c><see langword="false"/></c></returns>
	public bool TryGetBooleanValue(string name, out bool value)
	{
		if (!string.IsNullOrWhiteSpace(name))
		{
			unsafe
			{
				var nameUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(name);

				try
				{
					if (SDL_GetPropertyType(mId, nameUtf8) is PropertyType.Boolean)
					{
						value = SDL_GetBooleanProperty(mId, nameUtf8, default);

						return true;
					}
				}
				finally
				{
					Utf8StringMarshaller.Free(nameUtf8);
				}
			}
		}

		value = default;

		return false;
	}

	/// <summary>
	/// Tries to get the floating point value of a property with a specified <paramref name="name"/> within the group of properties
	/// </summary>
	/// <param name="name">The name of the property to get it's floating point value</param>
	/// <param name="value">The floating point value of the property, when this method returns <c><see langword="true"/></c>; otherwise, <c><see langword="default"/>(<see cref="float"/>)</c></param>
	/// <returns><c><see langword="true"/></c> if the property exists within the group of properties and it's set as a floating point property; otherwise, <c><see langword="false"/></c></returns>
	public bool TryGetFloatValue(string name, out float value)
	{
		if (!string.IsNullOrWhiteSpace(name))
		{
			unsafe
			{
				var nameUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(name);

				try
				{
					if (SDL_GetPropertyType(mId, nameUtf8) is PropertyType.Float)
					{
						value = SDL_GetFloatProperty(mId, nameUtf8, default);

						return true;
					}
				}
				finally
				{
					Utf8StringMarshaller.Free(nameUtf8);
				}
			}
		}

		value = default;

		return false;
	}

	/// <summary>
	/// Tries to get the numeric (integer) value of a property with a specified <paramref name="name"/> within the group of properties
	/// </summary>
	/// <param name="name">The name of the property to get it's numeric value</param>
	/// <param name="value">The numeric (integer) value of the property, when this method returns <c><see langword="true"/></c>; otherwise, <c><see langword="default"/>(<see cref="long"/>)</c></param>
	/// <returns><c><see langword="true"/></c> if the property exists within the group of properties and it's set as a number property; otherwise, <c><see langword="false"/></c></returns>
	public bool TryGetNumberValue(string name, out long value)
	{
		if (!string.IsNullOrWhiteSpace(name))
		{
			unsafe
			{
				var nameUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(name);

				try
				{
					if (SDL_GetPropertyType(mId, nameUtf8) is PropertyType.Number)
					{
						value = SDL_GetNumberProperty(mId, nameUtf8, default);

						return true;
					}
				}
				finally
				{
					Utf8StringMarshaller.Free(nameUtf8);
				}
			}
		}

		value = default;

		return false;
	}

	/// <summary>
	/// Tries to get the pointer value of a property with a specified <paramref name="name"/> within the group of properties
	/// </summary>
	/// <param name="name">The name of the property to get it's pointer value</param>
	/// <param name="value">The pointer value of the property, when this method returns <c><see langword="true"/></c>; otherwise, <c><see langword="default"/>(<see cref="IntPtr"/>)</c></param>
	/// <returns><c><see langword="true"/></c> if the property exists within the group of properties and it's set as a pointer property; otherwise, <c><see langword="false"/></c></returns>
	public bool TryGetPointerValue(string name, out IntPtr value)
	{
		if (!string.IsNullOrWhiteSpace(name))
		{
			unsafe
			{
				var nameUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(name);

				try
				{
					if (SDL_GetPropertyType(mId, nameUtf8) is PropertyType.Pointer)
					{
						value = unchecked((IntPtr)SDL_GetPointerProperty(mId, nameUtf8, default));

						return true;
					}
				}
				finally
				{
					Utf8StringMarshaller.Free(nameUtf8);
				}
			}
		}

		value = default;

		return false;
	}

	/// <summary>
	/// Tries to get the string value of a property with a specified <paramref name="name"/> within the group of properties
	/// </summary>
	/// <param name="name">The name of the property to get it's string value</param>
	/// <param name="value">The string value of the property, when this method returns <c><see langword="true"/></c>; otherwise, <c><see langword="default"/>(<see cref="string"/>?)</c></param>
	/// <returns><c><see langword="true"/></c> if the property exists within the group of properties and it's set as a string property; otherwise, <c><see langword="false"/></c></returns>
	public bool TryGetStringValue(string name, out string? value)
	{
		if (!string.IsNullOrWhiteSpace(name))
		{
			unsafe
			{
				var nameUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(name);

				try
				{
					if (SDL_GetPropertyType(mId, nameUtf8) is PropertyType.String)
					{
						value = Utf8StringMarshaller.ConvertToManaged(SDL_GetStringProperty(mId, nameUtf8, default));

						return true;
					}
				}
				finally
				{
					Utf8StringMarshaller.Free(nameUtf8);
				}
			}
		}

		value = default;

		return false;		
	}

	/// <summary>
	/// Tries to lock the group of properties
	/// </summary>
	/// <returns><c><see langword="true"/></c> if the group of properties was successfully locked; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// Obtains a multi-threaded lock for these properties. Other threads will wait while trying to lock these properties until they are unlocked. Properties must be unlocked before they are destroyed.
	/// </para>
	/// <para>
	/// The lock is automatically taken when setting individual properties, calling this method is only needed when you want to set several properties atomically or want to guarantee that properties being queried aren't freed in another thread.
	/// </para>
	/// </remarks>
	public bool TryLock() => SDL_LockProperties(mId);

	/// <summary>
	/// Tries to removes a property with a specified <paramref name="name"/> from the group of properties
	/// </summary>
	/// <param name="name">The name of the property to remove</param>
	/// <returns><c><see langword="true"/></c> if the property was successfully removed from the group of properties; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	public bool TryRemove(string name)
	{
		if (string.IsNullOrWhiteSpace(name))
		{
			return false;
		}

		unsafe
		{
			var nameUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(name);

			try
			{
				return SDL_ClearProperty(mId, nameUtf8);
			}
			finally
			{
				Utf8StringMarshaller.Free(nameUtf8);
			}
		}
	}

	/// <summary>
	/// Tries to set the boolean value of a property with a specified <paramref name="name"/> within the group of properties
	/// and potentially add the property to the group of properties, when it not yet exists
	/// </summary>
	/// <param name="name">The name of the property to set</param>
	/// <param name="value">The new value of the property</param>
	/// <returns><c><see langword="true"/></c> if the boolean value of the property was successfully set within the group of properties; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	public bool TrySetBooleanValue(string name, bool value)
	{
		if (string.IsNullOrWhiteSpace(name))
		{
			return false;
		}

		unsafe
		{
			var nameUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(name);

			try
			{
				return SDL_SetBooleanProperty(mId, nameUtf8, value);
			}
			finally
			{
				Utf8StringMarshaller.Free(nameUtf8);
			}
		}
	}

	/// <summary>
	/// Tries to set the floating point value of a property with a specified <paramref name="name"/> within the group of properties
	/// and potentially add the property to the group of properties, when it not yet exists
	/// </summary>
	/// <param name="name">The name of the property to set</param>
	/// <param name="value">The new value of the property</param>
	/// <returns><c><see langword="true"/></c> if the floating point value of the property was successfully set within the group of properties; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	public bool TrySetFloatValue(string name, float value)
	{
		if (string.IsNullOrWhiteSpace(name))
		{
			return false;
		}

		unsafe
		{
			var nameUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(name);

			try
			{
				return SDL_SetFloatProperty(mId, nameUtf8, value);
			}
			finally
			{
				Utf8StringMarshaller.Free(nameUtf8);
			}
		}
	}

	/// <summary>
	/// Tries to set the numeric (integer) value of a property with a specified <paramref name="name"/> within the group of properties
	/// and potentially add the property to the group of properties, when it not yet exists
	/// </summary>
	/// <param name="name">The name of the property to set</param>
	/// <param name="value">The new value of the property</param>
	/// <returns><c><see langword="true"/></c> if the numeric value of the property was successfully set within the group of properties; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	public bool TrySetNumberValue(string name, long value)
	{
		if (string.IsNullOrWhiteSpace(name))
		{
			return false;
		}

		unsafe
		{
			var nameUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(name);

			try
			{
				return SDL_SetNumberProperty(mId, nameUtf8, value);
			}
			finally
			{
				Utf8StringMarshaller.Free(nameUtf8);
			}
		}
	}

	/// <summary>
	/// Tries to set the pointer value of a property with a specified <paramref name="name"/> within the group of properties
	/// and potentially add the property to the group of properties, when it not yet exists
	/// </summary>
	/// <param name="name">The name of the property to set</param>
	/// <param name="value">The new value of the property</param>
	/// <returns><c><see langword="true"/></c> if the pointer value of the property was successfully set within the group of properties; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	public bool TrySetPointerValue(string name, IntPtr value)
	{
		if (string.IsNullOrWhiteSpace(name))
		{
			return false;
		}

		unsafe
		{
			var nameUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(name);

			try
			{
				return SDL_SetPointerProperty(mId, nameUtf8, unchecked((void*)value));
			}
			finally
			{
				Utf8StringMarshaller.Free(nameUtf8);
			}
		}
	}

	/// <summary>
	/// Tries to set the pointer value of a property with a specified <paramref name="name"/> within the group of properties
	/// and potentially add the property to the group of properties, when it not yet exists.
	/// This sets the pointer value of the property together with a <paramref name="cleanup"/> callback that is called when the property is deleted.
	/// </summary>
	/// <param name="name">The name of the property to set</param>
	/// <param name="value">The new value of the property, or <c><see cref="IntPtr.Zero"/></c> (<c><see langword="default"/>(<see cref="IntPtr"/>)</c>) to delete the property</param>
	/// <param name="cleanup">The callback to call when the property is deleted, or <c><see langword="null"/></c> if no cleanup is necessary</param>
	/// <returns><c><see langword="true"/></c> if the pointer value of the property was successfully set within the group of properties; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	/// <remarks>
	/// <para>
	/// The <paramref name="cleanup"/> callback is also called if setting the property fails for any reason.
	/// </para>
	/// <para>
	/// For simply setting basic data types, like numbers, bools, or strings, use <see cref="TrySetNumberValue(string, long)"/>, <see cref="TrySetBooleanValue(string, bool)"/>, or <see cref="TrySetStringValue(string, string?)"/> instead, as those methods will handle cleanup on your behalf.
	/// This method is only for more complex, custom data.
	/// </para>
	/// </remarks>
	public bool TrySetPointerValue(string name, IntPtr value, Action<IntPtr>? cleanup)
	{
		if (string.IsNullOrWhiteSpace(name))
		{
			return false;
		}

		unsafe
		{
			var nameUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(name);

			try
			{
				if (cleanup is not null)
				{
					return SDL_SetPointerPropertyWithCleanup(mId, nameUtf8, unchecked((void*)value), &CleanupPropertyCallback, unchecked((void*)GCHandle.ToIntPtr(GCHandle.Alloc(cleanup, GCHandleType.Normal))));
				}
				else
				{
					return SDL_SetPointerPropertyWithCleanup(mId, nameUtf8, unchecked((void*)value), null, null);
				}
			}
			finally
			{
				Utf8StringMarshaller.Free(nameUtf8);
			}
		}
	}

	/// <summary>
	/// Tries to set the string value of a property with a specified <paramref name="name"/> within the group of properties
	/// and potentially add the property to the group of properties, when it not yet exists
	/// </summary>
	/// <param name="name">The name of the property to set</param>
	/// <param name="value">The new value of the property</param>
	/// <returns><c><see langword="true"/></c> if the string value of the property was successfully set within the group of properties; otherwise, <c><see langword="false"/></c> (check <see cref="Error.TryGet(out string?)"/> for more information)</returns>
	public bool TrySetStringValue(string name, string? value)
	{
		if (string.IsNullOrWhiteSpace(name))
		{
			return false;
		}

		unsafe
		{
			var nameUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(name);
			var valueUtf8 = Utf8StringMarshaller.ConvertToUnmanaged(value);

			try
			{
				return SDL_SetStringProperty(mId, nameUtf8, valueUtf8);
			}
			finally
			{
				Utf8StringMarshaller.Free(valueUtf8);
				Utf8StringMarshaller.Free(nameUtf8);
			}
		}
	}

	/// <summary>
	/// Unlocks the group of properties
	/// </summary>
	public void Unlock() => SDL_UnlockProperties(mId);
}
